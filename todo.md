# TODO

## Priprema

- [x] Definisati i pripremiti sve tehnologije koje se koriste: backend, frontend i baza.
- [ ] Kreirati model baze podataka.
- [x] Pripremiti n-layer za backend.
- [ ] Kreiraj i pokreni migracije i popuni testne podatke.
- [ ] Pripremiti strukturu za frontend.

## Korisnici

- [ ] Kreirati pocetnu stranicu na kojoj ce vec biti dostupne sve funkcionalnosti
- [ ] Kreirati registraciju korisnika
- [ ] Kreirati login korisnika
- [ ] Kreirati logout korisnika

## Funkcionalnosti

### Osnovni workflow

- [ ] Korisnik ima svoju lokaciju
- [ ] Aplikacija zahteva putnikovu trenutnu lokaciju i lokaciju destinacije.
- [ ] Aplikacija pronalazi 10 najblizih vozila.
- [ ] Putnik bira vozilo i salje zahtev za rezervaciju.
- [ ] Vozac pregleda zahtev i prihvata ili odbija. Ukoliko je prihvatio, vozilo postaje bukirano.
- [ ] Vozac odvozi putnika na destinaciju, i nakon toga vozilo postaje dostupno.
- [ ] Putnik ocenjuje voznju.

### Dodatne funkcionalnosti

- [ ] Putnik moze da pregleda svoju istoriju voznji.
- [ ] Potrebno je simulirati prihvatanje/odbijanje zahteva. Sanse da vozac odbije zahtev su 25%. **Ovo verovatno znaci da nije potrebno kreirati rolu vozaca kod korisnika, nego se cela aplikacija pravi iz perspektive putnika.**
- [ ] Kada je zahtev prihvacen, potrebno je simulirati voznju do date destinacije (vozac vozi 60 km/h i njegova lokacija se azurira svakih 5 sekundi).
