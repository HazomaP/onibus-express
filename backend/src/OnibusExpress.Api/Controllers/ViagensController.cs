using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnibusExpress.Infrastructure.Context;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnibusExpress.Api.Controllers
{
    [ApiController]
    [Route("api/viagens")]
    public class ViagensController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ViagensController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Buscar([FromQuery] string? origem, [FromQuery] string? destino, [FromQuery] DateTime? data)
        {
            var query = _context.Viagens.Include(v => v.Rota).AsQueryable();

            // Aplica os filtros de busca se o usuário enviar
            if (!string.IsNullOrEmpty(origem))
                query = query.Where(v => v.Rota.Origem.ToLower().Contains(origem.ToLower()));
            
            if (!string.IsNullOrEmpty(destino))
                query = query.Where(v => v.Rota.Destino.ToLower().Contains(destino.ToLower()));

            if (data.HasValue)
                query = query.Where(v => v.DataHoraPartida.Date == data.Value.Date);

            // Regra: Mostrar apenas viagens que ainda não aconteceram
            query = query.Where(v => v.DataHoraPartida > DateTime.UtcNow);

            var viagens = await query.Select(v => new {
                v.Id,
                Origem = v.Rota.Origem,
                Destino = v.Rota.Destino,
                v.DataHoraPartida,
                v.PrecoBase,
                v.TotalAssentos
            }).ToListAsync();

            return Ok(viagens);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Detalhes(Guid id)
        {
            var viagem = await _context.Viagens
                .Include(v => v.Rota)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (viagem == null) return NotFound(new { erro = "Viagem não encontrada." });

            // Busca quais assentos já foram comprados para essa viagem específica
            var assentosOcupados = await _context.Reservas
                .Where(r => r.ViagemId == id && r.Status == OnibusExpress.Domain.Entities.ReservaStatus.Confirmada)
                .Select(r => r.NumeroAssento)
                .ToListAsync();

            return Ok(new {
                viagem.Id,
                Origem = viagem.Rota.Origem,
                Destino = viagem.Rota.Destino,
                viagem.DataHoraPartida,
                viagem.PrecoBase,
                viagem.TotalAssentos,
                AssentosOcupados = assentosOcupados,
                VagasRestantes = viagem.TotalAssentos - assentosOcupados.Count
            });
        }
    }
}