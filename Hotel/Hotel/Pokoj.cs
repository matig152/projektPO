using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hotel
{
    public class TworzenieNowegoPokojuException : Exception { }
    public class BrakPokojuException : Exception { }
    public enum EnumBudynek { A, B, C, D }
    public enum EnumPietro { pierwsze = 1, drugie = 2, trzecie = 3 }
    public enum EnumNrPokoju { jeden = 1, dwa = 2, trzy = 3, cztery = 4, piec = 5 }
    public class Pokoj
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
        public Pokoj WybierzPokoj(string id)
        {
            foreach (Pokoj p in pokoje) { if (p.IdPokoju == id) return p; }
            throw new BrakPokojuException();
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
        //TO DO
        //JAK SPRAWDZIC CZY POKOJ JEST WOLNY W DANYM OKRESIE?
        public Pokoj ZnajdzWolnyPokoj(int liczbaOsob)
        {
            Pokoj p = pokoje.FirstOrDefault();
            if (p == null) { throw new BrakPokojuException(); }
            return p;
        }
        #endregion
    }
}
