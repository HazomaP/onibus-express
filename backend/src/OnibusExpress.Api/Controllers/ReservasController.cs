using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnibusExpress.Application.Services;
using OnibusExpress.Infrastructure.Context; 
using OnibusExpress.Api.Validations; 
using System;
using System.Threading.Tasks;

namespace OnibusExpress.Api.Controllers
{
    [ApiController]
    [Route("api/reservas")]
    public class ReservasController : ControllerBase
    {
        private readonly ReservaApplicationService _reservaService;
        private readonly AppDbContext _context;

        public ReservasController(ReservaApplicationService reservaService, AppDbContext context)
        {
            _reservaService = reservaService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarReservaRequest request)
        {
            if (!CpfValidator.Validar(request.Cpf))
                return BadRequest(new { erro = "CPF inválido." });

            try
            {
                var reserva = await _reservaService.CriarReservaAsync(
                    request.ViagemId, request.Nome, request.Cpf, 
                    request.Email, request.DataNascimento, request.NumeroAssento
                );

                return CreatedAtAction(nameof(ConsultarPorCodigo), new { codigo = reserva.CodigoReserva }, new { reserva.CodigoReserva, reserva.Status });
            }
            catch (ArgumentException ex) { return BadRequest(new { erro = ex.Message }); }
            catch (InvalidOperationException ex) { return Conflict(new { erro = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { erro = "Erro interno", detalhe = ex.Message }); }
        }

        [HttpGet("{codigo}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ConsultarPorCodigo([FromRoute] string codigo)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Viagem).ThenInclude(v => v.Rota)
                .Include(r => r.Passageiro)
                .FirstOrDefaultAsync(r => r.CodigoReserva == codigo);

            if (reserva == null) return NotFound(new { erro = "Reserva não encontrada." });

            return Ok(new 
            { 
                Codigo = reserva.CodigoReserva, Status = reserva.Status, Assento = reserva.NumeroAssento,
                Viagem = new { Origem = reserva.Viagem.Rota.Origem, Destino = reserva.Viagem.Rota.Destino, Partida = reserva.Viagem.DataHoraPartida },
                Passageiro = new { Nome = reserva.Passageiro.Nome, Cpf = reserva.Passageiro.Cpf }
            });
        }

        [HttpDelete("{codigo}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CancelarReserva([FromRoute] string codigo)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Viagem)
                .FirstOrDefaultAsync(r => r.CodigoReserva == codigo);

            if (reserva == null) return NotFound(new { erro = "Reserva não encontrada." });

            if (DateTime.UtcNow > reserva.Viagem.DataHoraPartida.AddHours(-2))
                return BadRequest(new { erro = "Cancelamento negado. Prazo limite de 2 horas expirado." });

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }
    } 

    public record CriarReservaRequest(Guid ViagemId, string Nome, string Cpf, string Email, DateTime DataNascimento, int NumeroAssento);
}