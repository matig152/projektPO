using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hotel
{
    public class Pobyt
    {
        #region Pola
        DateTime poczatek;
        DateTime koniec;
        Pokoj pokoj;
        int liczbaOsob;
        Gosc rezerwujacy;
        List<Gosc> pozostaliGoscie;
        Pracownik zakladajacyRezerwacje;
        string numerRezerwacji;
        static int licznosc;
        #endregion

        #region Pola Dostepowe
        public string NumerRezerwacji { get => numerRezerwacji; set => numerRezerwacji = value; }
        public Pokoj Pokoj { get => pokoj; set => pokoj = value; }
        #endregion

        #region Konstruktory
        static Pobyt() { licznosc = 0; }
        public Pobyt(DateTime poczatek, DateTime koniec, Pokoj pokoj, int liczbaOsob, Gosc rezerwujacy, List<Gosc> pozostaliGoscie, Pracownik zakladajacyRezerwacje)
        {
            this.poczatek = poczatek;
            this.koniec = koniec;
            this.pokoj = pokoj;
            this.liczbaOsob = liczbaOsob;
            this.rezerwujacy = rezerwujacy;
            this.pozostaliGoscie = pozostaliGoscie;
            this.zakladajacyRezerwacje = zakladajacyRezerwacje;
            licznosc++;
            numerRezerwacji = $"{licznosc}{poczatek.Year}";
        }
        #endregion

        #region Metody
        public int Dlugosc() => koniec.Day - poczatek.Day;
        
        public decimal Cena()
        {
            decimal cena = 0;
            cena += Dlugosc() * 70; // Cena za rezerwujacego, ktory zawsze jest pelnoletni (70 zl za dobe)
            if(pozostaliGoscie.Count > 0)
            {
                foreach(Gosc g in pozostaliGoscie)
                {
                    if(g.Wiek() > 16) { cena += Dlugosc() * 70; } // 70 zl za kazdego goscia za dobe powyzej 16 lat
                    else { cena += Dlugosc() * 50; } // 50 zl za kazdego goscia za dobe ponizej 16 lat
                }
            }
            return cena;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Pobyt od {poczatek:dd.MM.yyyy} do {koniec:dd.MM.yyyy}");
            sb.AppendLine($"Numer rezerwacji: {numerRezerwacji}");
            sb.AppendLine($"Długość: {Dlugosc()} dni");
            sb.AppendLine($"Liczba osób: {liczbaOsob}");
            sb.AppendLine($"Gość rezerwujący: {rezerwujacy.Imie} {rezerwujacy.Nazwisko} ({rezerwujacy.Pesel})");
            if(pozostaliGoscie.Count() > 0)
            {
                sb.AppendLine("Pozostali goście: ");
                foreach(Gosc g in pozostaliGoscie) { sb.AppendLine(" - " + g.Imie + " " + g.Nazwisko); }
            }
            sb.AppendLine($"Pokój: {pokoj.ToString()}");
            sb.AppendLine($"Cena: {Cena()} zł");
            sb.AppendLine($"Rezerwację założył {zakladajacyRezerwacje.Imie} {zakladajacyRezerwacje.Nazwisko} ({zakladajacyRezerwacje.Wydzial})");
            return sb.ToString();
        }
        #endregion
    }



}
