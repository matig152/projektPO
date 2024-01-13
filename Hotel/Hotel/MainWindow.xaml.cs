using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualBasic;

namespace Hotel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ListaPokoi? wczytajListePokoi() //Zaczytanie listy pokojów z pliku XML
        {
            ListaPokoi? listaPokoi = ListaPokoi.OdczytXml("listaPokoi.xml");
            if (listaPokoi is null) { MessageBox.Show("Błąd wczytywania wykazu pokojów."); }
            return listaPokoi;
        }
        public static RejestrGosci? wczytajRejestrGosci() //Zaczytanie rejestru gosci z pliku XML
        {
            RejestrGosci? rg = RejestrGosci.OdczytXml("rejestrGosci.xml");
            if (rg is null) { MessageBox.Show("Błąd wczytywania rejestru gosci."); }
            return rg;
        }
        public static Kadra? wczytajKadre() //Zaczytanie listy pracownikow z pliku XML
        {
            Kadra? kdr = Kadra.OdczytXml("kadra.xml");
            if (kdr is null) { MessageBox.Show("Błąd wczytywania kadry."); }
            return kdr;
        }

        public MainWindow()
        {
            InitializeComponent();
            //WCZYTAJ DANE (w plikach jest 60 pokoi, jeden gosc z dwoma pobytami, jeden pracownik)
            ListaPokoi? lp = wczytajListePokoi();
            RejestrGosci? rg = wczytajRejestrGosci();
            Kadra? kdr = wczytajKadre();
            //Utwórz nowego pracownika
            Pracownik p1 = new Pracownik("Jan", "Kowalski", "12345678901", new Adres("Szeroka", 34, "Kraków", "32-032"), new DateTime(1960, 4, 12), EnumWydzial.Administracja, new DateTime(1979, 01, 01));
            kdr.DodajPracownika(p1); // dodaj go do listy pracownikow
            //Utwórz gości
            Gosc g1 = new Gosc("Kamil", "Stoch", "12345678910", new Adres("Chałubińskiego", 12, "Zakopane", "32-001"),new DateTime(1983, 5, 8), new List<Pobyt>());
            Gosc g2 = new Gosc("Jan", "Stoch", "12345612910", new Adres("Chałubińskiego", 80, "Zakopane", "32-001"), new DateTime(1980, 5, 8), new List<Pobyt>());
            Gosc g3 = new Gosc("Grzegorz", "Stoch", "12345668910", new Adres("Szeroka", 12, "Warszawa", "32-001"), new DateTime(2010, 5, 8), new List<Pobyt>());
            rg.DodajGoscia(g1); //dodaj do rejestru
            rg.DodajGoscia(g2); //dodaj do rejestru
            rg.DodajGoscia(g3); //dodaj do rejestru
            //Zakladanie gosciom rezerwacji
            g1.ZalozRezerwacje(new DateTime(2024, 01, 20), new DateTime(2024, 01, 25), new List<Gosc>(2){ g2, g3 }, p1, lp);
            g1.ZalozRezerwacje(new DateTime(2024, 01, 30), new DateTime(2024, 01, 31), new List<Gosc>(2) { g2 }, p1, lp);

            // Wypisz rejestr gosci na okno
            TextBox1.Text = rg.ToString();


        }



    }
}
