using GlobalBankApi.Data;
using GlobalBankApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlobalBankApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransacoesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> RegistrarTransacao([FromBody] Transacao transacao)
        {
            var conta = await _context.Contas
                .FirstOrDefaultAsync(c => c.Id == transacao.ContaId);

            if (conta == null)
            {
                return NotFound("Conta não encontrada.");
            }

            if (transacao.Valor <= 0)
            {
                return BadRequest("O valor da transação deve ser maior que zero.");
            }

            var tipo = transacao.Tipo.Trim().ToLower();

            if (tipo == "saque")
            {
                if (transacao.Valor > conta.Saldo)
                {
                    return Conflict("Saldo Insuficiente");
                }

                conta.Saldo -= transacao.Valor;
            }
            else if (tipo == "depósito" || tipo == "deposito")
            {
                conta.Saldo += transacao.Valor;
            }
            else
            {
                return BadRequest("Tipo de transação inválido. Use 'Depósito' ou 'Saque'.");
            }

            transacao.DataTransacao = DateTime.UtcNow;

            _context.Transacoes.Add(transacao);
            _context.Contas.Update(conta);

            await _context.SaveChangesAsync();

            if (transacao.Valor > 10000)
            {
                Console.WriteLine($"🚩 ALERTA DE SEGURANÇA: Transação de alto valor detectada para a conta {conta.NumeroConta}!");
            }

            return Ok(new
            {
                mensagem = "Transação registrada com sucesso.",
                conta.Id,
                conta.NumeroConta,
                saldoAtual = conta.Saldo
            });
        }
        [HttpGet("extrato/{contaId}")]
        public async Task<ActionResult<IEnumerable<object>>> ObterExtrato(int contaId)
        {
            var contaExiste = await _context.Contas.AnyAsync(c => c.Id == contaId);

            if (!contaExiste)
            {
                return NotFound("Conta não encontrada.");
            }

            var extrato = await _context.Transacoes
                .Where(t => t.ContaId == contaId)
                .OrderByDescending(t => t.DataTransacao)
                .Select(t => new
                {
                    t.ContaId,
                    t.Tipo,
                    t.Valor,
                    t.DataTransacao
                })
                .ToListAsync();

            return Ok(extrato);
        }
    }
}
