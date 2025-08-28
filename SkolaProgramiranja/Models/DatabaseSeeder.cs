using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SkolaProgramiranja.Models
{
    public static class DatabaseSeeder
    {
        public static void Seed()
        {
            using (var context = new AppDbContext())
            {
                context.Database.Migrate();
                if (!context.Korisnici.Any())
                {
                    var admin= new Korisnik
                    {
                        Ime = "Admin",
                        Prezime = "Glavni",
                        Email = "admin@skola.com",
                        Lozinka = "admin123",
                        Uloga = "Admin"
                    };

                    var instruktor = new Korisnik
                    {
                        Ime = "Ivan",
                        Prezime = "Instruktor",
                        Email = "instruktor@skola.com",
                        Lozinka = "instruktor123",
                        Uloga = "Instruktor",
                        MoraPromijenitiLozinku = true,
                        SlikaProfilaPath = "Resources/profilna.png" 
                    };

                    context.Korisnici.Add(admin);
                    context.Korisnici.Add(instruktor);
                    context.SaveChanges();

                    var instruktorIzBaza = context.Korisnici.FirstOrDefault(k => k.Email == "instruktor@skola.com");
                    //Dodavanje kursa koji vodi instruktor
                    if(instruktorIzBaza != null)
                    {
                        context.Kursevi.Add(new Kurs
                        {
                            Naziv = "Uvod u C#",
                            Opis = "Osnove programiranja u C# jeziku",
                            InstruktorId = instruktorIzBaza.Id
                        });
                    }
                    
                    context.SaveChanges();
                }   

            }
        }
    }
}   
