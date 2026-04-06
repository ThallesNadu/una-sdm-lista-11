using GlobalBankApi.Data;
using GlobalBankApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlobalBankApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContasController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<ContaBancaria>> AbrirConta([FromBody] ContaBancaria conta)
        {
            if (conta.Saldo < 0)
            {
                return BadRequest("O saldo inicial não pode ser negativo para contas internacionais.");
            }

            _context.Contas.Add(conta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterContaPorId), new { id = conta.Id }, conta);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> ListarContas()
        {
            var contas = await _context.Contas
                .Select(c => new
                {
                    c.Id,
                    c.Titular,
                    c.NumeroConta,
                    c.Saldo,
                    c.TipoConta
                })
                .ToListAsync();

            return Ok(contas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> ObterContaPorId(int id)
        {
            var conta = await _context.Contas
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.Titular,
                    c.NumeroConta,
                    c.Saldo,
                    c.TipoConta
                })
                .FirstOrDefaultAsync();

            if (conta == null)
            {
                return NotFound("Conta não encontrada.");
            }

            return Ok(conta);
        }
    }
}