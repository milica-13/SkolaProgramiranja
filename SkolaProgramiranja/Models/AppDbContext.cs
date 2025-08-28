using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace SkolaProgramiranja.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Kurs> Kursevi { get; set; }
        public DbSet<EvidencijaCasa> EvidencijeCasa { get; set; }
        public DbSet<PlanCasa> PlanoviCasa { get; set; }
        public DbSet<Prisustvo> Prisustva { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            optionsBuilder.UseSqlite(connectionString);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //  optionsBuilder.UseSqlite("Data Source=skola.db");
        //}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Kurs.Instruktor 1:N
            modelBuilder.Entity<Kurs>()
                .HasOne(k => k.Instruktor)
                .WithMany()
                .HasForeignKey(k => k.InstruktorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Korisnik-Kurs N:N (join: KorisnikKurs)
            modelBuilder.Entity<Korisnik>()
                .HasMany(k => k.Kursevi)
                .WithMany(k => k.Polaznici)
                .UsingEntity<Dictionary<string, object>>(
                    "KorisnikKurs",
                    j => j.HasOne<Kurs>().WithMany().HasForeignKey("KurseviId"),
                    j => j.HasOne<Korisnik>().WithMany().HasForeignKey("KorisniciId"),
                    j => { j.ToTable("KorisnikKurs"); j.HasKey("KorisniciId", "KurseviId"); });

            // Prisustvo: jedinstveno po (EvidencijaCasaId, UcenikId)
            modelBuilder.Entity<Prisustvo>()
                .HasIndex(p => new { p.EvidencijaCasaId, p.UcenikId })
                .IsUnique();

            modelBuilder.Entity<PlanCasa>()
                 .HasIndex(p => new { p.KursId, p.Datum })
                 .IsUnique();
        }

    }
}
