using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

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
        public int NrDomu
        {
            get => nrDomu; set
            {
                if (value <= 0) { throw new NieprawidlowyNumerException(); }
                else { nrDomu = value; }
            }
        }
        public int NrLokalu
        {
            get => nrLokalu; set
            {
                if (value < 0) { throw new NieprawidlowyNumerException(); }
                else { nrLokalu = value; }
            }
        }
        public string Miejscowosc { get => miejscowosc; set => miejscowosc = value; }
        public string KodPocztowy { get => kodPocztowy; set => kodPocztowy = value; }
        #endregion

        #region Konstruktory
        public Adres() { }
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
        public DateTime DataUrodzenia { get => dataUrodzenia; set => dataUrodzenia = value; }
        public string Pesel
        {
            get => pesel; set
            {
                if(Regex.IsMatch(value, @"[0-9]{11}")) { pesel = value; }
                else { throw new ZlyPeselException(); } 
            }
        }
        public Adres Adres { get => adres; set => adres = value; }
        #endregion

        #region Konstruktory
        public Osoba() { }
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
        public override string ToString()
        {
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
        List<Pobyt> listaPobytow;
        #endregion

        #region Metody Dostepowe
        public List<Pobyt> ListaPobytow { get => listaPobytow; }
        #endregion

        #region Konstruktory
        public Gosc() { listaPobytow = new List<Pobyt>(); }
        public Gosc(string imie, string nazwisko, string pesel, Adres adres, DateTime dataUrodzenia, List<Pobyt> listaPobytow) : base(imie, nazwisko, pesel, adres, dataUrodzenia)
        {
            this.listaPobytow = listaPobytow;
        }
        #endregion

        #region Metody
        public void ZalozRezerwacje(DateTime poczatek, DateTime koniec, List<Gosc> pozostaliGoscie, Pracownik zakladajacyRezerwacje, ListaPokoi listaPokoi, bool automatycznie = true, string idPokoju = "")
        {
            // Znajdź Pokoj
            Pokoj? pokoj = listaPokoi.WybierzPokoj("11A");
            if(automatycznie) {
                int rozmiar = pozostaliGoscie.Count();
                rozmiar += 1;
                pokoj = listaPokoi.WybierzPokojAutomatycznie(rozmiar, poczatek, koniec); 
                if(pokoj is null) { MessageBox.Show("Brak wolnych pokoi dla podanych parametrów!"); return; }
            }
            else { pokoj = listaPokoi.WybierzPokoj(idPokoju); }

            Pobyt pobyt = new Pobyt(poczatek, koniec, pokoj, pozostaliGoscie, zakladajacyRezerwacje);
            listaPobytow.Add(pobyt);
            pokoj.IdPobytow.Add(pobyt.IdPobytu);
        }


        public Pobyt? WyszukajPobytPoId(string idpobytu) => listaPobytow.Where(x => x.IdPobytu == idpobytu).First();
        
        

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Gość: " + base.ToString());
            if (listaPobytow.Count() > 0)
            {
                sb.AppendLine("Pobyty:");
                foreach (Pobyt p in listaPobytow) { sb.AppendLine(" - " + p.ToString()); }

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
        public DateTime DataWstapienia { get => dataWstapienia; set => dataWstapienia = value; }
        #endregion

        #region Konstruktory

        public Pracownik() { }
        public Pracownik(string imie, string nazwisko, string pesel, Adres adres, DateTime dataUrodzenia, EnumWydzial wydzial, DateTime dataWstapienia) : base(imie, nazwisko, pesel, adres, dataUrodzenia)
        {
            this.wydzial = wydzial;
            this.dataWstapienia = dataWstapienia;
        }
        #endregion

        #region Metody
        public int LataDoswiadczenia() => DateTime.Now.Year - dataWstapienia.Year;
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Imie} {Nazwisko} ({Wydzial})");
            //sb.AppendLine($"Wydział: {wydzial}");
            //sb.AppendLine($"Doświadczenie: {LataDoswiadczenia()} lat");
            return sb.ToString();
        }
        #endregion
    }

    public class RejestrGosci
    {
        #region Pola
        List<Gosc> listaGosci;
        #endregion

        #region Metody Dostepowe
        public List<Gosc> ListaGosci { get => listaGosci; set => listaGosci = value; }
        #endregion

        #region Konstruktory
        public RejestrGosci()
        {
            listaGosci = new List<Gosc>();
        }
        #endregion

        #region Metody
        public void DodajGoscia(Gosc g)
        {

            listaGosci.Add(g);
        }
        
        public void ZapisXml(string nazwaPliku)
        {
            using StreamWriter sw = new(nazwaPliku);
            XmlSerializer xs = new(typeof(RejestrGosci));
            xs.Serialize(sw, this);
        }
        public static RejestrGosci? OdczytXml(string nazwaPliku)
        {
            using StreamReader sr = new(nazwaPliku);
            XmlSerializer xs = new(typeof(RejestrGosci));
            return (RejestrGosci?)xs.Deserialize(sr);
        }

        public List<Pobyt> ListaWszystkichPobytow()
        {
            List<Pobyt> lista = new List<Pobyt>();
            foreach(Gosc g in listaGosci)
            {
                foreach(Pobyt p in g.ListaPobytow) { lista.Add(p); }
            }
            return lista;
        }
        public List<Gosc> FiltrujGosciPoIdPobytu(string idPobytu)
        {
            List<Gosc> res = new List<Gosc>();
            foreach (Gosc g in listaGosci)
            {
                if (g.ListaPobytow.Count() != 0)
                {
                    foreach (Pobyt p in g.ListaPobytow)
                    {
                        if (idPobytu == p.IdPobytu)
                        {
                            res.Add(g);
                            break;
                        }
                    }
                }
            }
            return res;
        }

        public List<Gosc> FiltrujGosciPoImieniu(string imie)
        {
            List<Gosc> res = new List<Gosc>();
            foreach (Gosc g in listaGosci)
            {
                if (g.Imie == imie)
                {
                    res.Add(g);
                }
            }
            return res;
        }

        public List<Gosc> FiltrujGosciPoNazwisku(string nazwisko)
        {
            List<Gosc> res = new List<Gosc>();
            foreach (Gosc g in listaGosci)
            {
                if (g.Nazwisko == nazwisko)
                {
                    res.Add(g);
                }
            }
            return res;
        }

        public List<Gosc> FiltrujGosciPoDacie(DateTime data)
        {
            List<Gosc> res = new List<Gosc>();
            foreach (Gosc g in listaGosci)
            {
                foreach (Pobyt p in g.ListaPobytow)
                {
                    if (p.Poczatek <= data && data <= p.Koniec)
                    {
                        res.Add(g);
                        break;
                    }
                }
            }
            return res;
        }

        public List<Pobyt> FiltrujPobyty(DateTime data)
        {
            List<Pobyt> res = new List<Pobyt>();
            foreach (Gosc g in listaGosci)
            {
                foreach (Pobyt p in g.ListaPobytow)
                {
                    if (p.Poczatek <= data && data <= p.Koniec)
                    {
                        res.Add(p);
                        break;
                    }
                }
            }
            return res;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(Gosc g in listaGosci) { sb.Append(g.ToString() + "\n"); }
            return sb.ToString();
        }
        #endregion
    }

    public class Kadra
    {
        #region Pola
        List<Pracownik> listaPracownikow;
        #endregion

        #region Metody Dostepowe
        public List<Pracownik> ListaPracownikow { get => listaPracownikow; set => listaPracownikow = value; }
        #endregion

        #region Konstruktory
        public Kadra()
        {
            listaPracownikow = new List<Pracownik>();
        }
        #endregion

        #region Metody
        public List<Pracownik> FiltrujPracownikowPoWydziale(EnumWydzial wydzial)
        {
            List<Pracownik> res = new List<Pracownik>();
            foreach (Pracownik p in listaPracownikow)
            {
                if (p.Wydzial == wydzial)
                {
                    res.Add(p);
                }
            }
            return res;
        }
        public List<Pracownik> FiltrujPracownikowPoImieniu(string imie)
        {
            List<Pracownik> res = new List<Pracownik>();
            foreach (Pracownik p in listaPracownikow)
            {
                if (p.Imie == imie)
                {
                    res.Add(p);
                }
            }
            return res;
        }
        public List<Pracownik> FiltrujPracownikowPoNazwisku(string nazwisko)
        {
            List<Pracownik> res = new List<Pracownik>();
            foreach (Pracownik p in listaPracownikow)
            {
                if (p.Nazwisko == nazwisko)
                {
                    res.Add(p);
                }
            }
            return res;
        }
        public List<Pracownik> FiltrujPracownikowMinimalnaIlosciLatDoswiadczenia(int lataDoswiadczenia)
        {
            List<Pracownik> res = new List<Pracownik>();
            foreach (Pracownik p in listaPracownikow)
            {
                if (p.LataDoswiadczenia() >= lataDoswiadczenia)
                {
                    res.Add(p);
                }
            }
            return res;
        }
        public void DodajPracownika(Pracownik p)
        {
            listaPracownikow.Add(p);
        }

        public void ZapisXml(string nazwaPliku)
        {
            using StreamWriter sw = new(nazwaPliku);
            XmlSerializer xs = new(typeof(Kadra));
            xs.Serialize(sw, this);
        }
        public static Kadra? OdczytXml(string nazwaPliku)
        {
            using StreamReader sr = new(nazwaPliku);
            XmlSerializer xs = new(typeof(Kadra));
            return (Kadra?)xs.Deserialize(sr);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Pracownik g in listaPracownikow) { sb.Append(g.ToString() + "\n"); }
            return sb.ToString();
        }
        #endregion
    }

}
