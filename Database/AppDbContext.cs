using Dashboard.BusinessLayer;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
namespace Dashboard.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Parametro> Parametri { get; set; }
        public DbSet<Comunita> Comunita { get; set; }
        public DbSet<TipologiaParametro> TipologiaParametro { get; set; }

        // Eventuali altre DbSet per altre tabelle

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurazioni aggiuntive, se necessario
            modelBuilder.Entity<Comunita>().HasKey(c => c.Comunita_Id);
            modelBuilder.Entity<Parametro>().HasKey(p => p.Parametro_Id);
            modelBuilder.Entity<TipologiaParametro>().HasKey(t => t.TipoParametro_Id);
        }
    }
}
