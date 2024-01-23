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
        public void DodajIdPobytu(string p) => idPobytow.Add(p);

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
        public void ZapisXml(string nazwaPliku)
        {
            using StreamWriter sw = new(nazwaPliku); XmlSerializer xs =
            new(typeof(Pokoj)); xs.Serialize(sw, this);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (idPobytow.Count > 0) { foreach (string p in idPobytow) { sb.AppendLine(" - Pobyt numer: " + p); } }
            return $"Pokój {idPokoju}, {rozmiar}-osobowy \n{sb.ToString()}";
        }
        
        public int CompareTo(Pokoj? other)
        {
            if (other is null) { return 1; }
            return NrPokoju.CompareTo((other as Pokoj).NrPokoju);
        }
        
        #endregion
    }

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
        public Pokoj WybierzPokoj(string idpokoju) 
        {
            Pokoj? pokoj = pokoje.Where(x => x.IdPokoju == idpokoju).First();
            if (pokoj is not null) { return pokoj; }
            else { throw new BrakPokojuException(); }
        }

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
        public void ZapisXml(string nazwaPliku)
        {
            using StreamWriter sw = new(nazwaPliku);
            XmlSerializer xs = new(typeof(ListaPokoi));
            xs.Serialize(sw, this);
        }
        public static ListaPokoi? OdczytXml(string nazwaPliku)
        {
            using StreamReader sr = new(nazwaPliku);
            XmlSerializer xs = new(typeof(ListaPokoi));
            return (ListaPokoi?)xs.Deserialize(sr);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Pokoj p in pokoje) { sb.AppendLine(p.ToString()); }
            return sb.ToString();
        }
        public Pokoj ZnajdzWolnyPokoj(int liczbaOsob)
        {
            Pokoj p = pokoje.FirstOrDefault();
            if (p == null) { throw new BrakPokojuException(); }
            return p;
        }
        #endregion
    }
}
