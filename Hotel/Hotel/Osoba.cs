using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Hotel
{
    public class ZlyPeselException : Exception {}
    public class NieprawidlowyNumerException : Exception {}
    public enum EnumWydzial { Recepcja, Housekeeping, Kuchnia, Administracja}
    public class Adres
	{
        #region Pola
        string ulica;
		int nrDomu;
		int nrLokalu;
		string miejscowosc;
		string kodPocztowy;
        #endregion

        #region Pola Dostepowe
        public string Ulica { get => ulica; set => ulica = value; }
        public int NrDomu { get => nrDomu; set 
            { 
                if(value <= 0) { throw new NieprawidlowyNumerException(); }
                else {  nrDomu = value; }
            }
        }
        public int NrLokalu { get => nrLokalu; set
            {
                if (value < 0) { throw new NieprawidlowyNumerException(); }
                else { nrLokalu = value; }
            }
        }
        public string Miejscowosc { get => miejscowosc; set => miejscowosc = value; }
        public string KodPocztowy { get => kodPocztowy; set => kodPocztowy = value; }
        #endregion
        #region Konstruktory
        public Adres(string ulica, int nrDomu, string miejscowosc, string kodPocztowy)
        {
            this.ulica = ulica;
            this.nrDomu = nrDomu;   
            this.miejscowosc = miejscowosc;
            this.kodPocztowy = kodPocztowy;
            nrLokalu = 0;
        }
        public Adres(string ulica, int nrDomu, int nrLokalu, string miejscowosc, string kodPocztowy) : this(ulica, nrDomu, miejscowosc, kodPocztowy)
        {
            this.nrLokalu = nrLokalu;
        }
        #endregion

        #region Metody
        public override string ToString()
        {
            if (nrLokalu == 0) { return $"ul. {ulica} {nrDomu}, {kodPocztowy} {miejscowosc}"; }
            return $"ul. {ulica} {nrDomu} m. {nrLokalu}, {kodPocztowy} {miejscowosc}";
        }
        #endregion
    }

    public abstract class Osoba
	{
        #region Pola
        string imie;
        string nazwisko;
        string pesel;
        Adres adres;
        DateTime dataUrodzenia;
        #endregion

        #region Metody Dostepowe
        public string Imie { get => imie; set => imie = value; }
        public string Nazwisko { get => nazwisko; set => nazwisko = value; }
        public string Pesel { get => pesel; init { 
                if(Regex.IsMatch(value, "[0-9]{11}"))
                {
                    Pesel = value;
                }
                else
                {
                    throw new ZlyPeselException();
                }
            } 
        }
        public Adres Adres { get => adres; set => adres = value; }
        #endregion

        #region Konstruktory
        public Osoba(string imie, string nazwisko, string pesel, Adres adres, DateTime dataUrodzenia)
        {
            this.imie = imie;
            this.nazwisko = nazwisko;
            this.pesel = pesel;
            this.adres = adres;
            this.dataUrodzenia = dataUrodzenia;
        }
        #endregion

        #region Metody
        public int Wiek() => DateTime.Now.Year - dataUrodzenia.Year;
        public override string ToString() { 
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{imie} {nazwisko}");
            sb.AppendLine($"PESEL: {pesel}");
            sb.AppendLine($"Data urodzenia: {dataUrodzenia:dd.MM.yyyy}, wiek: {Wiek()}");
            sb.AppendLine($"Adres: {adres}");
            return sb.ToString();
        } 
        
        #endregion
    }

    public class Gosc : Osoba
    {
        #region Pola
        List<Pobyt> pobyty;
        #endregion

        #region Metody Dostepowe
        public List<Pobyt> Pobyty { get => pobyty; set => pobyty = value; }
        #endregion

        #region Konstruktory
        public Gosc(string imie, string nazwisko, string pesel, Adres adres, DateTime dataUrodzenia, List<Pobyt> pobyty) : base(imie, nazwisko, pesel, adres, dataUrodzenia)
        {
            this.pobyty = pobyty;
        }
        #endregion

        #region Metody
        public void ZalozRezerwacje(DateTime poczatek, DateTime koniec, List<Gosc> pozostaliGoscie, Pracownik zakladajacyRezerwacje, ListaPokoi listaPokoi)
        {
            Pokoj pokoj = listaPokoi.WybierzPokoj("11A");
            Pobyt pobyt = new Pobyt(poczatek, koniec, pokoj, 1 + pozostaliGoscie.Count(), this, pozostaliGoscie, zakladajacyRezerwacje);
            pobyty.Add(pobyt);
            //pokoj.DodajPobyt(pobyt);
        }
        public decimal Naleznosci() => pobyty.Sum(x => x.Cena());
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Gość: " + base.ToString());
            if(pobyty.Count > 0)
            {
                sb.AppendLine("Pobyty");
                foreach (Pobyt p in pobyty) { sb.AppendLine($" - {p.NumerRezerwacji}, Cena: {p.Cena()}, Pokój: {p.Pokoj}"); }
                sb.AppendLine($"Nalezności razem: {Naleznosci()}");
            }
            else { sb.AppendLine("Brak pobytów."); }
            return sb.ToString();
        }
        #endregion
    }

    public class Pracownik : Osoba
    {
        #region Pola
        EnumWydzial wydzial;
        DateTime dataWstapienia;
        #endregion

        #region Metody Dostepowe
        public EnumWydzial Wydzial { get => wydzial; set => wydzial = value; }
        #endregion

        #region Konstruktory
        public Pracownik(string imie, string nazwisko, string pesel, Adres adres, DateTime dataUrodzenia, EnumWydzial wydzial, DateTime dataWstapienia) : base(imie, nazwisko, pesel, adres, dataUrodzenia)
        {
            this.wydzial = wydzial;
            this.dataWstapienia = dataWstapienia;
        }
        #endregion

        #region Metody
        public int LataDoswiadczenia() => DateTime.Now.Year - dataWstapienia.Year;
        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Pracownik: {base.ToString()}");
            sb.AppendLine($"Wydział: {wydzial}");
            sb.AppendLine($"Doświadczenie: {LataDoswiadczenia()} lat");
            return sb.ToString();
        } 
        #endregion
    }
}
