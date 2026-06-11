using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnibusExpress.Infrastructure.Context;
using System.Threading.Tasks;

namespace OnibusExpress.Api.Controllers
{
    [ApiController]
    [Route("api/rotas")]
    public class RotasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RotasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var rotas = await _context.Rotas.ToListAsync();
            return Ok(rotas);
        }
    }
}