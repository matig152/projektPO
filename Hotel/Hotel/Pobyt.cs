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
        Pokoj? pokoj;
        List<Gosc> pozostaliGoscie;
        Pracownik zakladajacyRezerwacje;
        string idPobytu;
        static int liczbaPorzadkowa;
        #endregion

        #region Pola Dostepowe
        public DateTime Poczatek { get => poczatek; set => poczatek = value; }
        public DateTime Koniec { get => koniec; set => koniec = value; }
        public Pokoj? Pokoj { get => pokoj; set => pokoj = value; }
        public List<Gosc> PozostaliGoscie { get => pozostaliGoscie; set => pozostaliGoscie = value; }
        public Pracownik ZakladajacyRezerwacje { get => zakladajacyRezerwacje; set => zakladajacyRezerwacje = value; }
        public string IdPobytu { get => idPobytu; set => idPobytu = value; }
        #endregion

        #region Konstruktory
        static Pobyt() { liczbaPorzadkowa = 1; }
        public Pobyt() { }
        public Pobyt(DateTime poczatek, DateTime koniec, Pokoj pokoj, List<Gosc> pozostaliGoscie, Pracownik zakladajacyRezerwacje)
        {
            this.poczatek = poczatek;
            this.koniec = koniec;
            this.pokoj = pokoj;
            this.pozostaliGoscie = pozostaliGoscie;
            this.zakladajacyRezerwacje = zakladajacyRezerwacje;
            idPobytu = $"{poczatek.Year}-{liczbaPorzadkowa:0000}";
            liczbaPorzadkowa++;
        }
        #endregion

        #region Metody
        public int Dlugosc() => koniec.Day - poczatek.Day;
        public int LiczbaOsob() => pozostaliGoscie.Count + 1;
        public decimal Cena()
        {
            decimal cena = 0;
            cena += Dlugosc() * 70; // Cena za rezerwujacego, ktory zawsze jest pelnoletni (70 zl za dobe)
            if (pozostaliGoscie.Count > 0)
            {
                foreach (Gosc g in pozostaliGoscie)
                {
                    if (g.Wiek() > 16) { cena += Dlugosc() * 70; } // 70 zl za kazdego goscia za dobe powyzej 16 lat
                    else { cena += Dlugosc() * 50; } // 50 zl za kazdego goscia za dobe ponizej 16 lat
                }
            }
            return cena;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Pobyt od {poczatek:dd.MM.yyyy} do {koniec:dd.MM.yyyy}");
            sb.AppendLine($"Długość: {Dlugosc()} dni");
            sb.AppendLine($"Liczba osób: {LiczbaOsob()}");
            if (pozostaliGoscie.Count() > 0)
            {
                sb.AppendLine("Pozostali goście: ");
                foreach (Gosc g in pozostaliGoscie) { sb.AppendLine(" - " + g.Imie + " " + g.Nazwisko); }
            }
            sb.AppendLine($"Cena: {Cena()} zł");
            sb.AppendLine($"Rezerwację założył {zakladajacyRezerwacje.Imie} {zakladajacyRezerwacje.Nazwisko} ({zakladajacyRezerwacje.Wydzial})");
            return sb.ToString();
        }
        #endregion
    }



}
