using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Xml.Serialization;

namespace Hotel
{
    #region Wyjatki
    public class TworzenieNowegoPokojuException : Exception
    {
        public TworzenieNowegoPokojuException() : base("Nie można stworzyć nowego pokoju!") { }
    }
    public class BrakPokojuException : Exception
    {
        public BrakPokojuException() : base("Brak takiego pokoju!") { }
    }
    #endregion
    
    #region Typy Wyliczeniowe
    public enum EnumBudynek { A, B, C, D }
    public enum EnumPietro { pierwsze = 1, drugie = 2, trzecie = 3 }
    public enum EnumNrPokoju { jeden = 1, dwa = 2, trzy = 3, cztery = 4, piec = 5 }
    #endregion

    /// <summary>
    /// Klasa Pokoj ma na celu reperezontowanie informacji o pojedynczym pokoju
    /// oraz reprezentuje zestaw czynności związanych z tymże.
    public class Pokoj : IComparable<Pokoj>
    {
        #region Pola
        EnumBudynek budynek;
        EnumPietro pietro;
        EnumNrPokoju nrPokoju;
        string idPokoju;
        int rozmiar;
        static int liczbaPokoi;
        List<string> idPobytow;
        #endregion

        #region Metody Dostepowe
        public EnumBudynek Budynek { get => budynek; set => budynek = value; }
        public EnumPietro Pietro { get => pietro; set => pietro = value; }
        public EnumNrPokoju NrPokoju { get => nrPokoju; set => nrPokoju = value; }
        public string IdPokoju { get => idPokoju; set => idPokoju = value; }
        public int Rozmiar { get => rozmiar; set => rozmiar = value; }
        public List<string> IdPobytow { get => idPobytow; set => idPobytow = value; }
        #endregion

        #region Konstruktory

        /// <summary>
        /// Konstruktor statyczny ma na celu ustawienie pola statycznego liczbaPokoi na zero.
        /// </summary>
        static Pokoj() { liczbaPokoi = 0; }

        /// <summary>
        /// Podstawowy konstruktor klasy Pokoj.
        /// </summary>
        public Pokoj() { }

        /// <summary>
        /// Konstruktor klasy Pokoj.
        /// </summary>
        /// <param name="budynek"> To wybór jednego z budynków należącego do hotelu. </param>
        /// <param name="pietro"> To wybór piętra w obrębie jednego z budynków Hotelu. </param>
        /// <param name="nrPokoju"> Każdy z pokoi przyjmuje pewien numer, by uprościć wyszukiwanie go przez gości i pracowników. </param>
        /// <param name="rozmiar"> Rozmiar pokoju informuje o ilości osób, które mogą w nim wizytować. </param>
        public Pokoj(EnumBudynek budynek, EnumPietro pietro, EnumNrPokoju nrPokoju, int rozmiar)
        {
            if (liczbaPokoi >= 60) { throw new TworzenieNowegoPokojuException(); }
            this.budynek = budynek;
            this.pietro = pietro;
            this.nrPokoju = nrPokoju;
            idPokoju = $"{(int)pietro}{(int)nrPokoju}{budynek}";
            this.rozmiar = rozmiar;
            idPobytow = new List<string>();
            liczbaPokoi++;
        }
        #endregion

        #region Metody

        /// <summary>
        /// Metoda ma na celu dodanie nowego id pobytu p
        /// do historii odwiedzeń tegoż pokoju.
        /// </summary>
        /// Na przykład:
        /// <code>
        /// PokojNrXWGrandBudapestHotel.DodajIdPobytu("2020-0730-11A")
        /// </code>
        /// Dodaje "11A" do listy pobytów w pokoju PokojNrXWGrandBudapestHotel.
        /// <param name="p"></param>
        public void DodajIdPobytu(string p) => idPobytow.Add(p);

        /// <summary>
        /// Metoda ma na celu odczyt wszystkich pobytów dla pokoju ze zbioru wszystkich pobytów w ogóle.
        /// </summary>
        /// <returns> Lista pobytów w danym pokoju. </returns>
        public List<Pobyt> ListaPobytowWPokoju()
        {
            //WCZYTAJ LISTE WSZYSTKICH POBYTOW
            RejestrGosci? rg = RejestrGosci.OdczytXml("rejestrGosci.xml");
            if (rg is null) { MessageBox.Show("Błąd wczytywania rejestru gosci."); return null; }
            List<Pobyt> listaWszyskichPobytow = rg.ListaWszystkichPobytow();
            // STWORZ LISTE DO KTOREJ BEDA DOPISYWANE POBYTY
            List<Pobyt> lista = new List<Pobyt>();
            foreach (string id in idPobytow) // dla każdego id pobytu...
            {
                foreach(Pobyt p in listaWszyskichPobytow)
                {
                    if(p.IdPobytu == id) { lista.Add(p); } // ...znajdz pobyt w liście wszystkich pobytow
                }
            }
            return lista;
        }

        /// <summary>
        /// Metoda ma na zapis danych, klasy Pokoj w pliku XML.
        /// </summary>
        /// <param name="nazwaPliku"> Nazwa pliku, w którym dojdzie do zapisu tychże informacji. </param>
        public void ZapisXml(string nazwaPliku)
        {
            using StreamWriter sw = new(nazwaPliku); XmlSerializer xs =
            new(typeof(Pokoj)); xs.Serialize(sw, this);
        }

        /// <summary>
        /// Metoda ma na celu wypisanie informacji na temat wybranego pokoju.
        /// </summary>
        /// <code> PokojNrXWGrandBudapestHotel.ToString() </code>
        /// Wyświetli informacje na temat wizyt w pokoju PokojNrXWGrandBudapestHotel w konsoli.
        /// Pokój 11A, 3-osobowy
        /// - Pobyt numer: 2020-0730-11A
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (idPobytow.Count > 0) { foreach (string p in idPobytow) { sb.AppendLine(" - Pobyt numer: " + p); } }
            return $"Pokój {idPokoju}, {rozmiar}-osobowy \n{sb.ToString()}";
        }

        /// <summary>
        /// Metoda ma na celu ustalenie porządku w trakcie porównywania dwóch obiektów klasy Pokoj.
        /// Sortowanie postępuje rosnąco względem numeru pokoju.
        /// </summary>
        /// <param name="other"> To pokój wzgędem którego porównujemy nasz obiekt.</param>
        /// <returns> -1, 0 lub 1 </returns>
        public int CompareTo(Pokoj? other)
        {
            if (other is null) { return 1; }
            return NrPokoju.CompareTo((other as Pokoj).NrPokoju);
        }
        
        #endregion
    }

    /// <summary>
    /// Klasa ListaPokoi ma na celu dostarczenie informacji na temat zagregowanej liczby obiektów klasy Pokoj
    /// oraz dokonywania czynności na tychże.
    /// </summary>
    public class ListaPokoi
    {
        #region Pola
        public List<Pokoj> pokoje;
        #endregion

        #region Metody Dostepowe
        #endregion

        #region Konstruktory

        /// <summary>
        /// Podstawowy konstruktor klasy ListaPokoi.
        /// </summary>
        public ListaPokoi() { }
        
        /// <summary>
        /// Konstruktor klasy ListaPokoi. Przyjmuje on argument lista, który przypisuje polu pokoje.
        /// </summary>
        /// <param name="lista"> Lista pokoi </param>
        public ListaPokoi(List<Pokoj> lista) { this.pokoje = lista; }
        #endregion

        #region Metody

        /// <summary>
        /// Metoda ma na celu wyszukanie i zwrócenie pokoju o zadanym id (jeśli taki istnieje).
        /// </summary>
        /// <param name="idpokoju"> To id pokoju, które wyszukujemy. </param>
        /// <exception cref="BrakPokojuException"> Błąd wywołany zostaje w sytuacji kiedy pokój o podnaym id nie istnieje.</exception>
        /// <returns> Pokój o zadanym id.</returns>
        public Pokoj WybierzPokoj(string idpokoju) 
        {
            Pokoj? pokoj = pokoje.Where(x => x.IdPokoju == idpokoju).First();
            if (pokoj is not null) { return pokoj; }
            else { throw new BrakPokojuException(); }
        }

        /// <summary>
        /// Metoda ma na celu wyszukanie pierwszego wolnego pokoju spełniającego podane w parametrach warunki.
        /// </summary>
        /// <param name="rozmiar"> To rozmiar pokoju - ilość osób. </param>
        /// <param name="poczatek"> Data początku właściwej wizyty. </param>
        /// <param name="koniec"> Data zakończenia właściwej wizyty. </param>
        /// <returns> Pierwszy pokój, który spełnia warunki. </returns>
        public Pokoj? WybierzPokojAutomatycznie(int rozmiar, DateTime poczatek, DateTime koniec)
        {
            bool pasuje = false;
            int nrPok = 0;
            while (!pasuje) // póki daty się nachodzą, szukamy pokoju
            {
                if (nrPok >= 60) { return null; }
                Pokoj pokoj = pokoje[nrPok]; // wybierz pokoj
                if(rozmiar != pokoj.Rozmiar) { pasuje = false; } // jeżeli rozmiar się nie zgadza to falsz
                List<Pobyt> listaPobytowPokoju = pokoj.ListaPobytowWPokoju(); // zbierz listę pobytów w pokoju
                foreach (Pobyt p in listaPobytowPokoju)
                {
                    if (p.CzyNachodzi(poczatek, koniec)) { pasuje = false; } // jezeli pobyt nachodzi to falsz
                    else { pasuje = true; break; } // jezeli nie nachodzi, wyjdz z loopa
                }
                nrPok++;
            }
            return pokoje[nrPok];
        }

        /// <summary>
        /// Metoda ma na celu znalezienie listy pokoi, w których czas spędził, spędza lub spędzać będzie podany gość.
        /// </summary>
        /// <param name="gosc"> Gość, którego listę wizyt w pokojach wyszukujemy. </param>
        /// <param name="rejestr"> Rejestr gości, który wczytujemy. To jego przeszukujemy w poszukiwaniu gościa. </param>
        /// <returns> Lista pokoi związanych z bytującym gościem. </returns>
        public static List<Pokoj> FiltrujPokojePoGosciach(Gosc gosc, RejestrGosci? rejestr)
        {
            List<Pokoj> res = new List<Pokoj>();
            if (rejestr is not null)
            {
                List<Gosc>? listaGosci = rejestr.ListaGosci;
                foreach (Gosc g in listaGosci)
                {
                    if (g.Equals(gosc))
                    {
                        foreach (Pobyt p in g.ListaPobytow)
                        {
                            if (!res.Contains(p.Pokoj))
                            {
                                res.Add(p.Pokoj);
                            }
                        }
                        break;
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Metoda ma na celu znalezienie Pokoi, ktore są zajęte w podanej chwili (data).
        /// </summary>
        /// <param name="data"> Data, która wpada w przedziały dat pobytów. </param>
        /// <param name="rejestr"> Rejestr, który przeszukujemy w poszukiwamiu pokoi. </param>
        /// <returns> Lista pokoi, które są zajęte w podanym dniu. </returns>
        public static List<Pokoj> FiltrujPokojePoDacie(DateTime data, RejestrGosci? rejestr)
        {
            List<Pokoj> res = new List<Pokoj>();
            if (rejestr is not null)
            {
                List<Gosc>? listaGosci = rejestr.ListaGosci;
                foreach (Gosc g in listaGosci)
                {
                    foreach (Pobyt p in g.ListaPobytow)
                    {
                        // znajdujemy pokoje, które były okupowane w danym czasie
                        if (p.Poczatek <= data && data <= p.Koniec && !res.Contains(p.Pokoj))
                        {
                            res.Add(p.Pokoj);
                        }
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Metoda ma na celu znalezienie pokoi, które zajęte są w podanym przedziale czasowym.
        /// </summary>
        /// <param name="poczatek"> Data początkowa. </param>
        /// <param name="koniec"> Data końcowa. </param>
        /// <param name="rejestr"></param>
        /// <returns> Zwracamy pokoje zajęte w podanym przedziale czasowym. </returns>
        public static List<Pokoj> PokojeZajeteWPodanymCzasie(DateTime poczatek, DateTime koniec, RejestrGosci? rejestr)
        {
            List<Pokoj> res = new List<Pokoj>();
            if (rejestr is not null)
            {
                List<Gosc>? listaGosci = rejestr.ListaGosci;
                foreach (Gosc g in listaGosci)
                {
                    foreach (Pobyt p in g.ListaPobytow)
                    {
                        // znajdujemy pokoje, które były okupowane w danym czasie
                        if ((poczatek < p.Poczatek && koniec <= p.Koniec)
                            || (poczatek < p.Poczatek && p.Poczatek <= koniec)
                            || (poczatek < p.Koniec) && (p.Koniec <= koniec))
                        {
                            if (!res.Contains(p.Pokoj))
                            {
                                res.Add(p.Pokoj);
                            }
                        }
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Metoda ma na celu zapisanie obecnego obiektu w pliku XML.
        /// </summary>
        /// <param name="nazwaPliku"> Nazwa pliku, w którym zapisujemy obecny obiekt ListaPokoi. </param>
        public void ZapisXml(string nazwaPliku)
        {
            using StreamWriter sw = new(nazwaPliku);
            XmlSerializer xs = new(typeof(ListaPokoi));
            xs.Serialize(sw, this);
        }

        /// <summary>
        /// Metoda ma na celu odczyt obiektu ListaPokoi z pliku o podanej nazwie.
        /// </summary>
        /// <param name="nazwaPliku"> Nazwa pliku, z którego dokonywać będziemy odczytu. </param>
        /// <returns> ListaPokoi </returns>
        public static ListaPokoi? OdczytXml(string nazwaPliku)
        {
            using StreamReader sr = new(nazwaPliku);
            XmlSerializer xs = new(typeof(ListaPokoi));
            return (ListaPokoi?)xs.Deserialize(sr);
        }

        /// <summary>
        /// Metoda wypisuje w konsoli informacje na temat pokoi.
        /// </summary>
        /// <returns> Zwracamy informacje na temat pokoi. </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Pokoj p in pokoje) { sb.AppendLine(p.ToString()); }
            return sb.ToString();
        }

        /// <summary>
        /// Metoda ma na celu znalezienie pierwszego wolnego pokoju.
        /// </summary>
        /// <param name="liczbaOsob"> LiczbaOsob to liczba osób, którą powinniśmy uwzględnić w wyszukiwaniach. </param>
        /// <returns> Zwracamy Pokoj </returns>
        /// <exception cref="BrakPokojuException"></exception>
        public Pokoj ZnajdzWolnyPokoj(int liczbaOsob)
        {
            Pokoj p = pokoje.FirstOrDefault();
            if (p == null) { throw new BrakPokojuException(); }
            return p;
        }
        #endregion
    }
}
