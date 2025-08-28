using Microsoft.EntityFrameworkCore;

namespace SkolaProgramiranja.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Kurs> Kursevi { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=skola.db");
        }
    }
}
