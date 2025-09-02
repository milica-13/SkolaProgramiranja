CREATE DATABASE IF NOT EXISTS skola_programiranja
  CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE skola_programiranja;

DROP TABLE IF EXISTS Prisustva;
DROP TABLE IF EXISTS KorisnikKurs;
DROP TABLE IF EXISTS EvidencijeCasa;
DROP TABLE IF EXISTS PlanoviCasa;
DROP TABLE IF EXISTS Kursevi;
DROP TABLE IF EXISTS Korisnici;

CREATE TABLE Korisnici (
  Id INT AUTO_INCREMENT PRIMARY KEY,
  Ime VARCHAR(100) NOT NULL,
  Prezime VARCHAR(100) NOT NULL,
  Email VARCHAR(200) NOT NULL UNIQUE,
  Lozinka VARCHAR(200) NOT NULL,
  Uloga VARCHAR(30) NOT NULL,               -- 'Admin' | 'Instruktor' | 'Ucenik'
  Obrazovanje VARCHAR(200) NULL,
  Biografija TEXT NULL,
  Telefon VARCHAR(50) NULL,
  SlikaProfilaPath VARCHAR(255) NOT NULL DEFAULT 'profilna.png',
  MoraPromijenitiLozinku TINYINT(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE Kursevi (
  Id INT AUTO_INCREMENT PRIMARY KEY,
  Naziv VARCHAR(200) NOT NULL,
  Opis TEXT NULL,
  InstruktorId INT NOT NULL,
  CONSTRAINT FK_Kursevi_Instruktor
    FOREIGN KEY (InstruktorId) REFERENCES Korisnici(Id)
    ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX IX_Kursevi_InstruktorId ON Kursevi (InstruktorId);

CREATE TABLE PlanoviCasa (
  Id INT AUTO_INCREMENT PRIMARY KEY,
  KursId INT NOT NULL,
  Datum DATE NOT NULL,
  Tema VARCHAR(255) NOT NULL,
  CONSTRAINT FK_PlanoviCasa_Kurs
    FOREIGN KEY (KursId) REFERENCES Kursevi(Id)
    ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT UQ_PlanoviCasa_Kurs_Datum UNIQUE (KursId, Datum)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX IX_PlanoviCasa_KursId ON PlanoviCasa (KursId);

CREATE TABLE EvidencijeCasa (
  Id INT AUTO_INCREMENT PRIMARY KEY,
  InstruktorId INT NOT NULL,
  KursId INT NOT NULL,
  KursNaziv VARCHAR(200) NOT NULL,
  Datum DATE NOT NULL,
  TemaCasa VARCHAR(255) NOT NULL,
  Opis TEXT NULL,
  PrisutniUcenici TEXT NULL,
  TrajanjeMin INT NOT NULL,
  CONSTRAINT FK_EvidencijeCasa_Instruktor
    FOREIGN KEY (InstruktorId) REFERENCES Korisnici(Id)
    ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT FK_EvidencijeCasa_Kurs
    FOREIGN KEY (KursId) REFERENCES Kursevi(Id)
    ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX IX_EvidencijeCasa_InstruktorId ON EvidencijeCasa (InstruktorId);
CREATE INDEX IX_EvidencijeCasa_KursId ON EvidencijeCasa (KursId);

CREATE TABLE KorisnikKurs (
  KorisniciId INT NOT NULL,
  KurseviId INT NOT NULL,
  PRIMARY KEY (KorisniciId, KurseviId),
  CONSTRAINT FK_KorisnikKurs_Korisnik
    FOREIGN KEY (KorisniciId) REFERENCES Korisnici(Id)
    ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT FK_KorisnikKurs_Kurs
    FOREIGN KEY (KurseviId) REFERENCES Kursevi(Id)
    ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX IX_KorisnikKurs_KurseviId ON KorisnikKurs (KurseviId);

CREATE TABLE Prisustva (
  Id INT AUTO_INCREMENT PRIMARY KEY,
  EvidencijaCasaId INT NOT NULL,
  UcenikId INT NOT NULL,
  Prisutan TINYINT(1) NOT NULL,
  CONSTRAINT UQ_Prisustva UNIQUE (EvidencijaCasaId, UcenikId),
  CONSTRAINT FK_Prisustva_EvidencijaCasa
    FOREIGN KEY (EvidencijaCasaId) REFERENCES EvidencijeCasa(Id)
    ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT FK_Prisustva_Ucenik
    FOREIGN KEY (UcenikId) REFERENCES Korisnici(Id)
    ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE INDEX IX_Prisustva_UcenikId ON Prisustva (UcenikId);
