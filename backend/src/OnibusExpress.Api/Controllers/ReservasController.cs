using Microsoft.AspNetCore.Mvc;
using OnibusExpress.Application.Services;
using System;
using System.Threading.Tasks;

namespace OnibusExpress.Api.Controllers
{
    [ApiController]
    [Route("api/reservas")]
    public class ReservasController : ControllerBase
    {
        private readonly ReservaApplicationService _reservaService;

        public ReservasController(ReservaApplicationService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarReservaRequest request)
        {
            try
            {
                var reserva = await _reservaService.CriarReservaAsync(
                    request.ViagemId, 
                    request.Nome, 
                    request.Cpf, 
                    request.Email, 
                    request.DataNascimento,
                    request.NumeroAssento
                );

                return CreatedAtAction(nameof(ConsultarPorCodigo), new { codigo = reserva.CodigoReserva }, new { reserva.CodigoReserva, reserva.Status });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { erro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro interno ao processar a reserva.", detalhe = ex.Message });
            }
        }

        [HttpGet("{codigo}")]
        public IActionResult ConsultarPorCodigo(string codigo)
        {
            // Placeholder para manter a rota válida no Swagger. Implementaremos a busca real em breve.
            return Ok(new { Codigo = codigo, Mensagem = "Endpoint de consulta ativo!" });
        }
    }

    public record CriarReservaRequest(Guid ViagemId, string Nome, string Cpf, string Email, DateTime DataNascimento, int NumeroAssento);
}