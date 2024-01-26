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
    #region Wyjątki

    /// <summary>
    /// Wyjątek wywoływany jest w sytuacji, kiedy podano nieprawidłowy pesel.
    /// </summary>
    public class ZlyPeselException : Exception 
    {
        public ZlyPeselException() : base("Nieprawidłowy Pesel!") { }
    }
    
    /// <summary>
    /// Wyjątek wywoływany jest w sytuacji kiedy podano nieprawidłowy numer (lokalu lub domu)
    /// </summary>
    public class NieprawidlowyNumerException : Exception
    {
        public NieprawidlowyNumerException() : base("Nieprawidłowy Numer!") { }
    }
    #endregion

    #region Typy Wyliczeniowe
    public enum EnumWydzial { Recepcja, Housekeeping, Kuchnia, Administracja}
    #endregion


    /// <summary>
    /// Klasa Adres ma za zadanie przechowanie informacji na temat adresu osób.
    /// </summary>
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
        public Adres() { ulica = ""; miejscowosc = ""; kodPocztowy = "00-000"; }
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

        /// <summary>
        /// Metoda zwraca informacje na temat adresu zamieszkania osoby.
        /// </summary>
        /// <returns> Tekst. </returns>
        public override string ToString()
        {
            if (nrLokalu == 0) { return $"ul. {ulica} {nrDomu}, {kodPocztowy} {miejscowosc}"; }
            return $"ul. {ulica} {nrDomu} m. {nrLokalu}, {kodPocztowy} {miejscowosc}";
        }
        #endregion
    }

    /// <summary>
    /// Osoba reprezentuje informacje na temat osoby w zakresie i kontekście w jakim wymaga tego obsługa hotelu. Jest klasą abstrakcyjną.
    /// </summary>
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
        public Osoba() {imie = "Brak"; nazwisko = "Brak"; pesel = "00000000000"; adres = new Adres(); }
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

        /// <summary>
        /// Metoda ma za zadanie odpowiedzenie na pytanie o wiek osoby.
        /// </summary>
        /// <returns> Wiek osoby w latach. </returns>
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

    /// <summary>
    /// Klasa gość ma za zadanie przechowywanie informacji na temat gościa oraz zezwolić m. in. na wybór warunków rezerwacji.
    /// </summary>
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

        /// <summary>
        /// Metoda ma za zadanie umożliwić rezerwację dla gościa (Gosc) o podanych polach:
        /// </summary>
        /// <param name="poczatek"> - początek pobytu, </param>
        /// <param name="koniec"> - koniec pobytu, </param>
        /// <param name="pozostaliGoscie"> - lista pozostałych gości, </param>
        /// <param name="zakladajacyRezerwacje"> - pracownik zakładający rezerwację, </param>
        /// <param name="listaPokoi"> - lista pokoi, </param>
        /// <param name="automatycznie"> - wartość prawda/fałsz określająca, czy przydzielenie ma następować w skutek wyboru narzuconego przez system lub nie,</param>
        /// <param name="idPokoju"> - ID pokoju. </param>
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

        /// <summary>
        /// Metoda ma na celu znalezienie pobytu o podanym ID.
        /// </summary>
        /// <param name="idpobytu"> ID pobytu </param>
        /// <returns> Pobyt (jeśli ten istnieje). </returns>
        public Pobyt? WyszukajPobytPoId(string idpobytu) => listaPobytow.Where(x => x.IdPobytu == idpobytu).First();
        
        
        /// <summary>
        /// Metoda ma na celu wypisanie istotnych informacji dotyczących gościa i jego/jej pobytów.
        /// </summary>
        /// <returns> Tekst. </returns>
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

    /// <summary>
    /// Klasa pracownik przechowuje informacje na temat pracownika hotelu.
    /// </summary>
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

        /// <summary>
        /// Metoda zwraca informację na temat lat doświadczenia pracownika.
        /// </summary>
        /// <returns> Ilość lat doświadczenia. </returns>
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

    /// <summary>
    /// Klasa RejestrGosci posiada informacje na temat gosci zarejestrowanych w hotelu. Pozwala na zapis i odczyt rejestru a także wyszukiwanie gosci czy pobytów spełniających wskazane warunki.
    /// </summary>
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

        /// <summary>
        /// Metoda dodaje gościa do listy istniejących.
        /// </summary>
        /// <param name="g"> w.w. Gość </param>
        public void DodajGoscia(Gosc g)
        {
            listaGosci.Add(g);
        }
        
        /// <summary>
        /// Metoda ma na celu zapisanie informacji zawartych w instancji klasy RejestrGosci do pliku XML...
        /// </summary>
        /// <param name="nazwaPliku"> ... o podanej nazwie. </param>
        public void ZapisXml(string nazwaPliku)
        {
            using StreamWriter sw = new(nazwaPliku);
            XmlSerializer xs = new(typeof(RejestrGosci));
            xs.Serialize(sw, this);
        }

        /// <summary>
        /// Metoda ma na celu odczytanie informacji na zapisanych jako rejestr gości zawartych w pliku XML o ... 
        /// </summary>
        /// <param name="nazwaPliku"></param>
        /// <returns>... o podanej nazwie. </returns>
        public static RejestrGosci? OdczytXml(string nazwaPliku)
        {
            using StreamReader sr = new(nazwaPliku);
            XmlSerializer xs = new(typeof(RejestrGosci));
            return (RejestrGosci?)xs.Deserialize(sr);
        }

        /// <summary>
        /// Metoda ma na celu znalezienie wszsystkich zarejestrowanych pobytów.
        /// </summary>
        /// <returns> Lista pobytów. </returns>
        public List<Pobyt> ListaWszystkichPobytow()
        {
            List<Pobyt> lista = new List<Pobyt>();
            foreach(Gosc g in listaGosci)
            {
                foreach(Pobyt p in g.ListaPobytow) { lista.Add(p); }
            }
            return lista;
        }

        /// <summary>
        /// Metoda ma na celu znalezienie wszsystkich gości, którzy związani są z pobytem o podanym ID.
        /// </summary>
        /// <param name="idPobytu"> ID pobytu. </param>
        /// <returns> Lista gosci. </returns>
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

        /// <summary>
        /// Metoda ma na celu znalezienie wszystkich gosci o podanym imieniu.
        /// </summary>
        /// <param name="imie"> Imię. </param>
        /// <returns> Lista gości. </returns>
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

        /// <summary>
        /// Metoda ma na celu znalezienie Gosci o podanym nazwisku.
        /// </summary>
        /// <param name="nazwisko"> Nazwisko. </param>
        /// <returns> Gość. </returns>
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

        /// <summary>
        /// Metoda ma na celu znalezienie wszystkich gości, których wizyta trwa w podanej dacie.
        /// </summary>
        /// <param name="data"> Data. </param>
        /// <returns> Lista gości. </returns>
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

        /// <summary>
        /// Metoda ma na celu znalezienie pobytów, które mają miejsce o podanej dacie.
        /// </summary>
        /// <param name="data"> Data. </param>
        /// <returns> Lista pobytów. </returns>
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

        /// <summary>
        /// Metoda ma na celu zagregowanie i przedstawienie informacji na temat obecnych na liście gości.  
        /// </summary>
        /// <returns> Tekst. </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(Gosc g in listaGosci) { sb.Append(g.ToString() + "\n"); }
            return sb.ToString();
        }
        #endregion
    }

    /// <summary>
    /// Klasa kadra przechowuje informacje na temat kadry pracowników. Pozwala na zapis i odczyt listy, poszerzenie jej o nowych pracowników i znajdywanie tych, którzy spełniają zadane warunki.
    /// </summary>
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

        /// <summary>
        /// Metoda ma na celu znalezienie wszystkich pracowników pracujących w danym wydziale.
        /// </summary>
        /// <param name="wydzial"> Wydział. </param>
        /// <returns> Lista pracowników. </returns>
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

        /// <summary>
        /// Metoda ma na celu znalezienie pracowników o podanym imieniu.
        /// </summary>
        /// <param name="imie"> Imie. </param>
        /// <returns> Lista pracowników. </returns>
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

        /// <summary>
        /// Metoda ma na celu znalezienie pracowników o podanym nazwisku.
        /// </summary>
        /// <param name="nazwisko"> Nazwisko. </param>
        /// <returns> Lista pracowników. </returns>
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

        /// <summary>
        /// Metoda ma na celu znalezienie pracowników, którzy posiadają daną...
        /// </summary>
        /// <param name="lataDoswiadczenia"> ... minimalną ilość lat doświadczenia. </param>
        /// <returns> Lista pracowników. </returns>
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

        /// <summary>
        /// Metoda ma na celu dodanie pracownika do listy zatrudnionych.
        /// </summary>
        /// <param name="p"> Pracownik. </param>
        public void DodajPracownika(Pracownik p)
        {
            listaPracownikow.Add(p);
        }


        /// <summary>
        /// Metoda ma na celu zapis informacji z instacnji klasy Kadra do pliku XML...
        /// </summary>
        /// <param name="nazwaPliku"> ... o podanej nazwie. </param>
        public void ZapisXml(string nazwaPliku)
        {
            using StreamWriter sw = new(nazwaPliku);
            XmlSerializer xs = new(typeof(Kadra));
            xs.Serialize(sw, this);
        }

        /// <summary>
        /// Metoda ma na celu odczyt informacji zapisanych uprzednio w postaci instancji klasy Kadra z pliku XML...
        /// </summary>
        /// <param name="nazwaPliku"> ... o podanej nazwie</param>
        /// <returns> Instancję klasy kadra lub null. </returns>
        public static Kadra? OdczytXml(string nazwaPliku)
        {
            using StreamReader sr = new(nazwaPliku);
            XmlSerializer xs = new(typeof(Kadra));
            return (Kadra?)xs.Deserialize(sr);
        }

        /// <summary>
        /// Metoda ma na celu przedstawienie informacji na temat kadry pracowników.
        /// </summary>
        /// <returns> Tekst. </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Pracownik g in listaPracownikow) { sb.Append(g.ToString() + "\n"); }
            return sb.ToString();
        }
        #endregion
    }

}
