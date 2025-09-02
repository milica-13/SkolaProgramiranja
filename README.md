# Škola Programiranja

Desktop aplikacija za upravljanje školom programiranja: kursevima, instruktorima, planovima časova i evidencijom prisustva.  
Izrađena u **C# / .NET 8 (WPF)** uz **Entity Framework Core** i **SQLite**. Aplikacija podržava **teme (Light/Dark/Purple)** i **dvojezičnost (SR/EN)**.

---

## Pregled ekrana

### Početni ekran / Prijava
![Login ekran](./login.png)

### Admin panel — kursevi
![Admin panel](./admin-panel.png)

### Instruktor — pregled rada 
![Evidencija časa](./instruktor-panel.png)

---

##  Funkcionalnosti

###  Administrator
- **CRUD**: kursevi, instruktori, planovi časova, evidencije časova, prisustva.
- **Upis učenika na kurs** (N:N Korisnik–Kurs).
- **Pretraga** po nazivu kursa i/ili instruktoru.
- **Dvoklik** na red u tabeli otvara formu za **uređivanje**.
- **Tema** (Light/Dark/Purple) i **jezik** (SR/EN) iz UI.
- **Odjava**.

###  Instruktor
- Pregled sopstvenih kurseva i **plana rada** (datum/tema).
- Vođenje **evidencije časa**: tema, opis, prisutni učenici, trajanje.
- Brza promjena **teme** i **jezika**.
- **Obavezna promjena lozinke** pri prvoj prijavi.

### Zajedničko
- Material Design stilovi (moderni WPF UI).
- Snackbar poruke (uspjeh/greška), hover/selekt stilovi, jasna navigacija.

---

##  Baza podataka

- **EF Core + SQLite** (Code-First).
- Entiteti:  
  `Korisnik`, `Kurs`, `PlanCasa`, `EvidencijaCasa`, `Prisustvo` i veza M:N `KorisnikKurs`.
- Ograničenja:
  - `Prisustvo`: unikatno **(EvidencijaCasaId, UcenikId)**.
  - `PlanCasa`: unikatno **(KursId, Datum)**.
  - `Kurs.InstruktorId` → FK ka `Korisnik(Id)` (restrikt brisanje).

![Šema baze](./skola_programiranja.png)


---

## Tehnologije

- **.NET 8 / WPF**
- **Entity Framework Core** + **Microsoft.Data.Sqlite (SQLite)**
- **MaterialDesignInXaml** (UI stilovi)
- Visual Studio 2022 / EF Core Tools

---

### Korisničko uputstvo


### 1) Početak i prijava

1. Pokreni aplikaciju.  
   ![Početni ekran / Prijava](./login.png)
2. Unesi **email** i **lozinku**, pa klikni **Prijavi se**.
3. Ako je lozinka pogrešna ili nalog ne postoji, aplikacija prikazuje jasnu poruku o grešci.
4. **Prva prijava**: ako je za nalog postavljeno *MoraPromijenitiLozinku = true*, bićeš preusmjeren(a) na formu za promjenu lozinke prije nastavka rada.

---

### 2) Uloge i pristup

- **Administrator**
  - Upravljanje **kursevima**, **instruktorima** i **upisima polaznika** (M:N Korisnik–Kurs).
  - Pretraga/sortiranje, uređivanje i brisanje zapisa.
  - Promjena **teme** (Light/Dark/Purple) i **jezika** (SR/EN).
  - Odjava sa sistema.

- **Instruktor**
  - Pregled sopstvenih **kurseva**.
  - Rad sa **planom časova** (datum/tema).
  - Vođenje i izmjena **evidencija časova** (tema, opis, prisutni, trajanje).
  - Brza promjena **teme** i **jezika**.
  - Obavezna promjena lozinke pri prvoj prijavi.

---

### 3) Administrator — rad sa kursevima

![Admin panel](./admin-panel.png)

- **Tabela**: ID, Naziv, Opis, Instruktor.
- **Pretraga**: unos ključnih riječi (naziv kursa ili instruktor).
- **Sortiranje**: klik na zaglavlje kolone.
- **CRUD**:
  - **Dodaj** novi kurs (dugme *Dodaj*).
  - **Uredi** postojeći kurs (dugme *Uredi* ili **dvoklik** na red).
  - **Obriši** kurs (uz potvrdu).
- **Upis polaznika**: dodavanje/uklanjanje korisnika na kurs (veza Korisnik–Kurs).
- **Dvoklik** na red u tabeli otvara formu za **uređivanje kursa**.


---

### 4) Instruktor — plan rada i evidencija časova

![Instruktor panel / evidencija](./instruktor-panel.png)

#### 4.1 Izbor kursa
- U vrhu prozora nalazi se **ComboBox** za odabir kursa.
- Izbor kursa filtrira **kalendar**, **plan rada** i **evidencije**.

#### 4.2 Kalendar
- Označeni su termini iz **plana rada** i datumi sačuvanih **evidencija časova**.

#### 4.3 Plan rada
- Svaka stavka sadrži **datum** i **temu** časa.
- Polje **Tema** je uređivo; čuvanje preko ikone **diskete**.
- **Dodaj plan** dodaje novi termin (prečica **Ctrl+N** predlaže naredni logičan datum).

#### 4.4 Evidencija časa
- Unos/izmjena: **Tema časa**, **Opis**, **Prisutni učenici** (lista/tekst), **Trajanje (min)**.
- Lista evidencija je sortirana od najnovije ka starijoj.
- **Dvoklik** na stavku evidencije otvara formu **EvidencijaCasaView** za uređivanje.

---

### 5) Tema i jezik

- **Tema**: **Light / Dark / Purple** (primjenjuje se na cijeli UI).
- **Jezik**: **SR / EN**.
- Aplikacija pamti posljednji izbor i automatski ga primjenjuje pri sljedećem pokretanju.

---

### 6) Odjava

- Klikom na **Odjava** aplikacija se vraća na ekran za prijavu.

---

### Kratke prečice (Keyboard Shortcuts)

- **Ctrl + N** — dodaj novu stavku u **planu rada** (instruktor).
- **Enter** — potvrdi dijalog gdje je primjenjivo (npr. čuvanje).
- **Esc** — zatvori/otkaži trenutni dijalog.

---

### Preduslovi
- **.NET SDK 8**
- **Visual Studio 2022** (Desktop development with .NET)

### 1) Kloniraj repo
```bash
git clone https://github.com/<tvoj-username>/SkolaProgramiranja.git
cd SkolaProgramiranja

