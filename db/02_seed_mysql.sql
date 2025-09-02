-- 02_seed_mysql.sql
-- Punjenje testnim podacima za skola_programiranja

USE skola_programiranja;

START TRANSACTION;

DELETE FROM Prisustva;
DELETE FROM KorisnikKurs;
DELETE FROM EvidencijeCasa;
DELETE FROM PlanoviCasa;
DELETE FROM Kursevi;
DELETE FROM Korisnici;

INSERT INTO Korisnici (Id, Ime, Prezime, Email, Lozinka, Uloga, MoraPromijenitiLozinku) VALUES
(1,'Admin','Admin','admin@skola.com','Admin123!','Admin',0),
(2,'Iva','Instruktorka','instruktor@skola.com','Instruktor123!','Instruktor',1),
(3,'Petra','Petrović','petra@skola.com','Ucenik123!','Ucenik',0),
(4,'Marko','Marković','marko@skola.com','Ucenik123!','Ucenik',0),
(5,'Ana','Anić','ana@skola.com','Ucenik123!','Ucenik',0);

INSERT INTO Kursevi (Id, Naziv, Opis, InstruktorId) VALUES
(1,'C# Osnove','Uvod u C#, OOP, WPF, EF Core',2),
(2,'WPF Napredni','Binding, MVVM, styling, Material Design',2);

INSERT INTO PlanoviCasa (Id, KursId, Datum, Tema) VALUES
(1,1,'2025-09-05','Uvod i instalacija'),
(2,1,'2025-09-12','Sintaksa i tipovi'),
(3,1,'2025-09-19','Klase i objekti'),
(4,2,'2025-09-06','WPF pregled i XAML'),
(5,2,'2025-09-13','Data binding i komande');

INSERT INTO EvidencijeCasa (Id, InstruktorId, KursId, KursNaziv, Datum, TemaCasa, Opis, PrisutniUcenici, TrajanjeMin) VALUES
(1,2,1,'C# Osnove','2025-09-05','Uvod i instalacija','Instalirali alate, Hello World','Petra; Marko; Ana',90),
(2,2,1,'C# Osnove','2025-09-12','Sintaksa i tipovi','Tipovi podataka, operatori','Petra; Marko',90),
(3,2,2,'WPF Napredni','2025-09-06','WPF pregled i XAML','Pregled layout panela i osnovnih kontrola','Petra',90);

INSERT INTO KorisnikKurs (KorisniciId, KurseviId) VALUES
(3,1),(4,1),(5,1),  -- troje na C# Osnove
(3,2);              -- Petra i na WPF

INSERT INTO Prisustva (Id, EvidencijaCasaId, UcenikId, Prisutan) VALUES
(1,1,3,1),(2,1,4,1),(3,1,5,0),  -- prvi čas C# Osnove
(4,2,3,1),(5,2,4,0),            -- drugi čas C# Osnove
(6,3,3,1);                      -- prvi čas WPF (samo Petra)

COMMIT;
