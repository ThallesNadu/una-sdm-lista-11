using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalBankApi.Models
{
    public class ContaBancaria
    {
        public int Id { get; set; }
        public string Titular { get; set; } 
        public string NumeroConta { get; set; }    
        public decimal Saldo { get; set; }
        public string TipoConta { get; set; }   
    }
}