using GlobalBankApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalBankApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ContaBancaria> Contas { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ContaBancaria>().HasKey(c => c.Id);
            modelBuilder.Entity<Transacao>().HasKey(t => t.Id);
        }
    }
}