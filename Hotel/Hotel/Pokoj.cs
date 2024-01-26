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

    /// <summary>
    /// Wyjątek jest wywoływany w sytuacji, kiedy dochodzi do próby stworzenia pokoju.
    /// </summary>
    public class TworzenieNowegoPokojuException : Exception
    {
        public TworzenieNowegoPokojuException() : base("Nie można stworzyć nowego pokoju!") { }
    }

    /// <summary>
    /// Wyjątek jest wywoływany w sytuacji, kiedy pokój, do którego pragniemy się odwołać nie istnieje. 
    /// </summary>
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
    /// Klasa Pokój przechowuje informacje na temat pojedynczego pokoju i definiuje operacje, które można wykonywać na nim lub na grupie jej instancji.
    /// </summary>
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
        static Pokoj() { liczbaPokoi = 0; }

        public Pokoj() { }
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
        /// Metoda ma na celu dodanie ID pobytu do historii pokoju.
        /// </summary>
        /// <param name="p"> ID pobytu. </param>
        public void DodajIdPobytu(string p) => idPobytow.Add(p);

        /// <summary>
        /// Metoda ma na celu zwrócenie wszsystkich pobytów, jakie miały miejsce w tym pokoju.
        /// </summary>
        /// <returns> Lista pobytów. </returns>
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
        /// Metoda ma na celu zapis informacji zawartych w instancji klasy Pokoj w pliku XML...
        /// </summary>
        /// <param name="nazwaPliku"> ... o podanej nazwie. </param>
        public void ZapisXml(string nazwaPliku)
        {
            using StreamWriter sw = new(nazwaPliku); XmlSerializer xs =
            new(typeof(Pokoj)); xs.Serialize(sw, this);
        }

        /// <summary>
        /// Metoda ma na celu zwrócenie informacji na temat pokoju.
        /// </summary>
        /// <returns> Tekst. </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (idPobytow.Count > 0) { foreach (string p in idPobytow) { sb.AppendLine(" - Pobyt numer: " + p); } }
            return $"Pokój {idPokoju}, {rozmiar}-osobowy \n{sb.ToString()}";
        }

        /// <summary>
        /// Metoda ma na celu ustalenie kolejności w procesie porównywania. Sortowanie będzie następować rosnąco według ID pokoju.
        /// </summary>
        /// <param name="other"></param>
        /// <returns> Wartość -1, 0 lub 1 </returns>
        public int CompareTo(Pokoj? other)
        {
            if (other is null) { return 1; }
            return NrPokoju.CompareTo((other as Pokoj).NrPokoju);
        }
        #endregion
    }


    /// <summary>
    /// Klasa ListaPokoi zawiera informacje na temat zagreogwanej ilości pokoi i pozwala dokonywać operacji na tychże.
    /// </summary>
    public class ListaPokoi
    {
        #region Pola
        public List<Pokoj> pokoje;
        #endregion

        #region Metody Dostepowe
        #endregion

        #region Konstruktory
        public ListaPokoi() { }
        public ListaPokoi(List<Pokoj> lista) { this.pokoje = lista; }
        #endregion

        #region Metody

        /// <summary>
        /// Metoda ma na celu wybór pokoju o podanym ID.
        /// </summary>
        /// <param name="idpokoju"> ID pokoju. </param>
        /// <returns> Pokój. </returns>
        /// <exception cref="BrakPokojuException"></exception>
        public Pokoj WybierzPokoj(string idpokoju) 
        {
            Pokoj? pokoj = pokoje.Where(x => x.IdPokoju == idpokoju).First();
            if (pokoj is not null) { return pokoj; }
            else { throw new BrakPokojuException(); }
        }

        /// <summary>
        /// Metoda ma na celu wybranie pokoju w sposób automatyczny (jeśli jest to możliwe). Przyjmuje informacje o:
        /// </summary>
        /// <param name="rozmiar"> - liczbie gości, </param>
        /// <param name="poczatek"> - początku trwanie planowanego pobytu, </param>
        /// <param name="koniec"> - końcu planowanego pobytu.</param>
        /// <returns> Pokój, który spełnia podane warunki, jeśli ten istnieje. </returns>
        public Pokoj? WybierzPokojAutomatycznie(int rozmiar, DateTime poczatek, DateTime koniec)
        {
            bool pasuje = false;
            int nrPok = 0;
            while (!pasuje) // póki daty się nachodzą, szukamy pokoju
            {
                if (nrPok >= 60) { return null; }
                Pokoj pokoj = pokoje[nrPok];
                if (pokoj.Rozmiar == rozmiar) 
                {
                    List<Pobyt>? listaPobytowPokoju = pokoj.ListaPobytowWPokoju();
                    if (listaPobytowPokoju.Count() == 0) { pasuje = true; return pokoj; }
                    else
                    {
                        foreach (Pobyt p in listaPobytowPokoju)
                        {
                            bool czyNachodzi = p.CzyNachodzi(poczatek, koniec);
                            if (czyNachodzi) { pasuje = false; break; }
                            pasuje = true;
                        }
                        if (pasuje) { return pokoj; }
                        else { nrPok++; }
                    }
                }
                else
                {
                    nrPok++; continue;
                }
            }
            return pokoje[nrPok];
        }

        /// <summary>
        /// Metoda ma za zadanie znaleźć pokoje w podanym rejestrze, w których bytował, bytuje lub bytował będzie podany gość.
        /// </summary>
        /// <param name="gosc"> Gość. </param>
        /// <param name="rejestr"> Rejstr gości. </param>
        /// <returns> Lista pokoi. </returns>
        public static List<Pokoj> FiltrujPokojePoGosciach(Gosc gosc, RejestrGosci rejestr)
        {
            List<Pokoj> res = new List<Pokoj>();
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
            return res;
        }

        /// <summary>
        /// Metoda ma na celu znalezienie pokoi w podanym rejestrze, które są okupowane w podanym czasie (pojedyncza data).
        /// </summary>
        /// <param name="data"> Data. </param>
        /// <param name="rejestr"> Rejestr. </param>
        /// <returns> Lista pokoi. </returns>
        public static List<Pokoj> FiltrujPokojePoDacie(DateTime data, RejestrGosci rejestr)
        {
            List<Pokoj> res = new List<Pokoj>();
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
            return res;
        }

        /// <summary>
        /// Metoda ma na celu znalezienie pokoi w podanym rejestrze, które w podanym zakresie czasu są okupowane.
        /// </summary>
        /// <param name="poczatek"> Początkowa data. </param>
        /// <param name="koniec"> Data końcowa. </param>
        /// <param name="rejestr"> Rejestr. </param>
        /// <returns> Lista pokoi. </returns>
        public static List<Pokoj> PokojeZajeteWPodanymCzasie(DateTime poczatek, DateTime koniec, RejestrGosci rejestr)
        {
            List<Pokoj> res = new List<Pokoj>();
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
            return res;
        }

        /// <summary>
        /// Metoda ma na celu dokonanie zapisu informacji zawartych w instancji klasy ListaPokoi do pliku XML...
        /// </summary>
        /// <param name="nazwaPliku"> ... o podanej nazwie. </param>
        public void ZapisXml(string nazwaPliku)
        {
            using StreamWriter sw = new(nazwaPliku);
            XmlSerializer xs = new(typeof(ListaPokoi));
            xs.Serialize(sw, this);
        }

        /// <summary>
        /// Metoda ma na celu odczyt informacji zapisanych jako lista pokoi z pliku XML...
        /// </summary>
        /// <param name="nazwaPliku">...  o podanej nazwie. </param>
        /// <returns> Lista pokoi. </returns>
        public static ListaPokoi? OdczytXml(string nazwaPliku)
        {
            using StreamReader sr = new(nazwaPliku);
            XmlSerializer xs = new(typeof(ListaPokoi));
            return (ListaPokoi?)xs.Deserialize(sr);
        }

        /// <summary>
        /// Metoda ma na celu zwrócenie informacji na temat pokoi znajdujących się w liście.
        /// </summary>
        /// <returns> Tekst. </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Pokoj p in pokoje) { sb.AppendLine(p.ToString()); }
            return sb.ToString();
        }

        /// <summary>
        /// Metoda ma na celu znalezienie wolengo pokoju o podanej ilości osób.
        /// </summary>
        /// <param name="liczbaOsob"> ilość osób. </param>
        /// <returns> Pokój. </returns>
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
