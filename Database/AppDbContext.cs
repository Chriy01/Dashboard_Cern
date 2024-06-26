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

        public DbSet<Parametro> Parametro { get; set; }
        public DbSet<Comunita> Comunita { get; set; }
        public DbSet<TipologiaParametro> TipologiaParametro { get; set; }
        public DbSet<Utente> Utente { get; set; }
        public DbSet<Utente_Comunita> Utente_Comunita { get; set; }
        public DbSet<Tipo_Utenza> Tipo_Utenza { get; set; }
        public DbSet<Consumer> Consumer { get; set; }
        public DbSet<Prosumer> Prosumer { get; set; }
        public DbSet<Impianto> Impianto { get; set; }

        // Eventuali altre DbSet per altre tabelle

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configurazioni aggiuntive, se necessario
            modelBuilder.Entity<Comunita>().HasKey(c => c.Comunita_Id);
            modelBuilder.Entity<Parametro>().HasKey(p => p.Parametro_Id);
            modelBuilder.Entity<TipologiaParametro>().HasKey(t => t.TipoParametro_Id);
            modelBuilder.Entity<Utente>().HasKey(u => u.Utente_Id);
            modelBuilder.Entity<Utente_Comunita>().HasKey(uc => uc.Utente_Comunita_Id);
            modelBuilder.Entity<Tipo_Utenza>().HasKey(tu => tu.Tipo_Utenza_Id);
            modelBuilder.Entity<Consumer>().HasKey(c => c.Consumer_Id);
            modelBuilder.Entity<Prosumer>().HasKey(p => p.Prosumer_Id);
            modelBuilder.Entity<Impianto>().HasKey(i  => i.Impianto_Id);
        }
    }
}
