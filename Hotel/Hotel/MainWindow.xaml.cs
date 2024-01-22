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
            //WCZYTAJ DANE - PRZY INICJALIZACJI KAŻDEGO OKNA
            ListaPokoi? lp = wczytajListePokoi();
            RejestrGosci? rg = wczytajRejestrGosci();
            Kadra? kdr = wczytajKadre();

            // DODAWANIE GOŚCI
            Gosc g1 = new Gosc("Kamil", "Stoch", "12345678910", new Adres("Chałubińskiego", 12, "Zakopane", "32-001"), new DateTime(1983, 5, 8), new List<Pobyt>());
            Gosc g2 = new Gosc("Jan", "Stoch", "12345612910", new Adres("Chałubińskiego", 80, "Zakopane", "32-001"), new DateTime(1980, 5, 8), new List<Pobyt>());
            Gosc g3 = new Gosc("Grzegorz", "Stoch", "12345668910", new Adres("Szeroka", 12, "Warszawa", "32-001"), new DateTime(2010, 5, 8), new List<Pobyt>());
            rg.DodajGoscia(g1); //dodaj do rejestru
            rg.DodajGoscia(g2); //dodaj do rejestru
            rg.DodajGoscia(g3); //dodaj do rejestru

            //DODAWANIE PRACOWNIKA
            Pracownik p1 = new Pracownik("Jan", "Kowalski", "12345678901", new Adres("Szeroka", 34, "Kraków", "32-032"), new DateTime(1960, 4, 12), EnumWydzial.Administracja, new DateTime(1979, 01, 01));
            Pracownik p2 = new Pracownik("Michał", "Nowak", "12345672101", new Adres("Jana Styki", 12, "Kraków", "32-032"), new DateTime(1999, 1, 1), EnumWydzial.Housekeeping, new DateTime(2021, 04, 01));
            Pracownik p3 = new Pracownik("Karol", "Aranowski", "15545678901", new Adres("Jana Buszka", 32, "Kraków", "32-032"), new DateTime(1987, 4, 11), EnumWydzial.Recepcja, new DateTime(2015, 10, 01));
            Pracownik p4 = new Pracownik("Andrzej", "Chyra", "12397678901", new Adres("Gramatyka", 1, "Kraków", "32-032"), new DateTime(1988, 1, 12), EnumWydzial.Kuchnia, new DateTime(1999, 01, 12));
            kdr.DodajPracownika(p1); // dodaj do listy pracownikow
            kdr.DodajPracownika(p2);
            kdr.DodajPracownika(p3);
            kdr.DodajPracownika(p4);


            // DODAWANIE REZERWACJI
            g1.ZalozRezerwacje(new DateTime(2024, 01, 22), new DateTime(2024, 01, 31), new List<Gosc>() { g2, g3}, p1, lp); // automatyczny dobór pokoju
            g3.ZalozRezerwacje(new DateTime(2024, 01, 30), new DateTime(2024, 01, 31), new List<Gosc>(2) { g2 }, p1, lp, false, "14A"); // ręczny dobór pokoju - tu trzeba będzie uzględnić sprawdzenie, czy pokój spełnia wymagania, mogę się tym zająć jak już GUI będzie gotowe

            // PODGLĄD POKOI

            // PODGLĄD GOŚCI
            List<Gosc> lista3 = rg.FiltrujGosciPoIdPobytu("2023-0001");
            string s3 = "";
            foreach (Gosc g in lista3) { s3 += "\n" + (g.ToString()); }
            TextBox1.Text = s3;

            // PODGLĄD PRACOWNIKÓW
            List<Pracownik> lista1 = kdr.FiltrujPracownikowPoWydziale(EnumWydzial.Kuchnia);
            string s = "";
            foreach(Pracownik p in lista1) { s += "\n" + (p.ToString()); }
            //TextBox1.Text = s;

            // WYSZUKIWANIE POBYTÓW
            List<Pobyt> lista2 = rg.FiltrujPobyty(new DateTime(2024, 01, 31));
            string s1 = "";
            foreach (Pobyt p in lista2) { s1 += "\n" + (p.ToString()); }
            //TextBox1.Text = s1;


            // Wypisz rejestr gosci na okno



        }



    }
}
