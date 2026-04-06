using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalBankApi.Models
{
    public class Transacao
    {
        public int Id { get; set; }
        public int ContaId { get; set; }
        public string Tipo { get; set; } // "Depósito" ou "Saque"
        public decimal Valor { get; set; }
        public DateTime DataTransacao { get; set; }
    }
}