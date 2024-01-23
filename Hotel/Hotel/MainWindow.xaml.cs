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
        private ListaPokoi listaPokoi;
        private RejestrGosci rejestrGosci;
        private Kadra kadra;

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
            listaPokoi = wczytajListePokoi() ?? new ListaPokoi();
            rejestrGosci = wczytajRejestrGosci() ?? new RejestrGosci();
            kadra = wczytajKadre() ?? new Kadra();

            //WCZYTAJ DANE -PRZY INICJALIZACJI KAŻDEGO OKNA
            //ListaPokoi? lp = wczytajListePokoi();
            //RejestrGosci? rg = new RejestrGosci();
            //Kadra? kdr = new Kadra();

            //// DODAWANIE GOŚCI
            //Gosc g1 = new Gosc("Kamil", "Stoch", "12345678910", new Adres("Chałubińskiego", 12, "Zakopane", "32-001"), new DateTime(1983, 5, 12), new List<Pobyt>());
            //Gosc g2 = new Gosc("Jan", "Michalak", "12345612910", new Adres("Chałubińskiego", 80, "Zakopane", "32-001"), new DateTime(1980, 5, 10), new List<Pobyt>());
            //Gosc g3 = new Gosc("Grzegorz", "Kurowski", "11245668910", new Adres("Szeroka", 12, "Warszawa", "02-001"), new DateTime(2000, 1, 1), new List<Pobyt>());
            //Gosc g4 = new Gosc("Paweł", "Głaszcz", "12345668910", new Adres("Złota", 130, "Warszawa", "01-001"), new DateTime(1995, 2, 7), new List<Pobyt>());
            //Gosc g5 = new Gosc("Adrian", "Latosiewicz", "10795668910", new Adres("Szeroka", 12, "Warszawa", "01-012"), new DateTime(2003, 3, 6), new List<Pobyt>());
            //Gosc g6 = new Gosc("Andrzej", "Frycz", "12345668576", new Adres("Szeroka", 12, "Warszawa", "03-011"), new DateTime(1966, 4, 5), new List<Pobyt>());
            //Gosc g7 = new Gosc("Tomasz", "Janowski", "12341118910", new Adres("Bracka", 100, "Pabianice", "95-200"), new DateTime(1988, 5, 4), new List<Pobyt>());
            //Gosc g8 = new Gosc("Łukasz", "Stanisławowski", "12345660010", new Adres("Bugaj", 12, "Warszawa", "32-001"), new DateTime(1999, 6, 3), new List<Pobyt>());
            //Gosc g9 = new Gosc("Maciej", "Kąpa", "12345178910", new Adres("Grodzka", 44, "Kraków", "32-032"), new DateTime(1990, 7, 2), new List<Pobyt>());
            //Gosc g10 = new Gosc("Karolina", "Iksińska", "43215668910", new Adres("Kazimierza Wielkiego", 3, "Kraków", "32-001"), new DateTime(2001, 8, 1), new List<Pobyt>());
            //rg.DodajGoscia(g1); //dodaj do rejestru
            //rg.DodajGoscia(g2); //dodaj do rejestru
            //rg.DodajGoscia(g3); //dodaj do rejestru
            //rg.DodajGoscia(g4);
            //rg.DodajGoscia(g5);
            //rg.DodajGoscia(g6);
            //rg.DodajGoscia(g7);
            //rg.DodajGoscia(g8);
            //rg.DodajGoscia(g9);
            //rg.DodajGoscia(g10);

            ////DODAWANIE PRACOWNIKA
            //Pracownik p1 = new Pracownik("Jan", "Kowalski", "12345678901", new Adres("Szeroka", 34, "Kraków", "32-032"), new DateTime(1960, 4, 12), EnumWydzial.Administracja, new DateTime(1979, 01, 01));
            //Pracownik p2 = new Pracownik("Michał", "Nowak", "12345672101", new Adres("Jana Styki", 12, "Kraków", "32-032"), new DateTime(1999, 1, 1), EnumWydzial.Housekeeping, new DateTime(2021, 04, 01));
            //Pracownik p3 = new Pracownik("Karol", "Aranowski", "15545678901", new Adres("Jana Buszka", 32, "Kraków", "32-032"), new DateTime(1987, 4, 11), EnumWydzial.Recepcja, new DateTime(2015, 10, 01));
            //Pracownik p4 = new Pracownik("Andrzej", "Chyra", "12397673301", new Adres("Gramatyka", 1, "Kraków", "32-032"), new DateTime(1988, 1, 12), EnumWydzial.Kuchnia, new DateTime(2008, 01, 12));
            //Pracownik p5 = new Pracownik("Siergiej", "Zelensky", "10045678901", new Adres("Długa", 34, "Kraków", "32-032"), new DateTime(1960, 4, 12), EnumWydzial.Administracja, new DateTime(1979, 01, 01));
            //Pracownik p6 = new Pracownik("Michał", "Śpionek", "11245672101", new Adres("Krótka", 12, "Kraków", "32-032"), new DateTime(2000, 1, 1), EnumWydzial.Recepcja, new DateTime(2021, 04, 01));
            //Pracownik p7 = new Pracownik("Karol", "Masternak", "15958678901", new Adres("Królewska", 32, "Kraków", "32-032"), new DateTime(1987, 4, 11), EnumWydzial.Recepcja, new DateTime(2015, 10, 01));
            //Pracownik p8 = new Pracownik("Andrzej", "Nowak", "12397378901", new Adres("Mała", 11, "Kraków", "32-032"), new DateTime(2000, 1, 12), EnumWydzial.Kuchnia, new DateTime(2023, 01, 12));
            //Pracownik p9 = new Pracownik("Jan", "Grzelak", "12340078901", new Adres("Duża", 8, "Kraków", "32-032"), new DateTime(1960, 4, 12), EnumWydzial.Administracja, new DateTime(1979, 01, 01));
            //Pracownik p10 = new Pracownik("Michał", "Głaszcz", "10005672101", new Adres("Floriańska", 40, "Kraków", "32-032"), new DateTime(1967, 1, 11), EnumWydzial.Administracja, new DateTime(2021, 04, 01));
            //Pracownik p11 = new Pracownik("Karol", "Budka", "15500008901", new Adres("Stolarska", 32, "Kraków", "32-032"), new DateTime(1970, 4, 1), EnumWydzial.Recepcja, new DateTime(1990, 10, 01));
            //Pracownik p12 = new Pracownik("Borys", "Jasiński", "12301978901", new Adres("Szewska", 11, "Kraków", "32-032"), new DateTime(1999, 1, 1), EnumWydzial.Kuchnia, new DateTime(1999, 01, 12));
            //kdr.DodajPracownika(p1); // dodaj do listy pracownikow
            //kdr.DodajPracownika(p2);
            //kdr.DodajPracownika(p3);
            //kdr.DodajPracownika(p4);
            //kdr.DodajPracownika(p5); // dodaj do listy pracownikow
            //kdr.DodajPracownika(p6);
            //kdr.DodajPracownika(p7);
            //kdr.DodajPracownika(p8);
            //kdr.DodajPracownika(p9); // dodaj do listy pracownikow
            //kdr.DodajPracownika(p10);
            //kdr.DodajPracownika(p11);
            //kdr.DodajPracownika(p12);


            //// DODAWANIE REZERWACJI
            //g1.ZalozRezerwacje(new DateTime(2024, 02, 1), new DateTime(2024, 02, 2), new List<Gosc>() { g2, g3 }, p1, lp, false, "12A"); // automatyczny dobór pokoju
            //g2.ZalozRezerwacje(new DateTime(2024, 02, 2), new DateTime(2024, 02, 7), new List<Gosc>() { }, p3, lp, false, "13A"); // automatyczny dobór pokoju
            //g3.ZalozRezerwacje(new DateTime(2024, 02, 3), new DateTime(2024, 02, 8), new List<Gosc>() { }, p3, lp, false, "15A"); // automatyczny dobór pokoju
            //g4.ZalozRezerwacje(new DateTime(2024, 02, 4), new DateTime(2024, 02, 6), new List<Gosc>() { }, p7, lp, false, "13B"); // automatyczny dobór pokoju
            //g5.ZalozRezerwacje(new DateTime(2024, 02, 5), new DateTime(2024, 02, 6), new List<Gosc>() { }, p7, lp, false, "11C"); // automatyczny dobór pokoju
            //g6.ZalozRezerwacje(new DateTime(2024, 02, 6), new DateTime(2024, 02, 10), new List<Gosc>() { }, p7, lp, false, "12C"); // automatyczny dobór pokoju
            //g7.ZalozRezerwacje(new DateTime(2024, 02, 7), new DateTime(2024, 02, 12), new List<Gosc>() { }, p7, lp, false, "12D"); // automatyczny dobór pokoju
            //g8.ZalozRezerwacje(new DateTime(2024, 02, 8), new DateTime(2024, 02, 9), new List<Gosc>() { }, p3, lp, false, "11D"); // automatyczny dobór pokoju
            //g9.ZalozRezerwacje(new DateTime(2024, 02, 9), new DateTime(2024, 02, 10), new List<Gosc>() { }, p10, lp, false, "13D"); // automatyczny dobór pokoju
            //g10.ZalozRezerwacje(new DateTime(2024, 02, 10), new DateTime(2024, 02, 15), new List<Gosc>() { }, p10, lp, false, "14D"); // automatyczny dobór pokoju



            //lp.ZapisXml("listaPokoi.xml");
            //rg.ZapisXml("rejestrGosci.xml");
            //kdr.ZapisXml("kadra.xml");

        }

        private void btnDodajGoscia_Click(object sender, RoutedEventArgs e)
        {
            var oknoDodajGoscia = new DodajGoscia(wczytajRejestrGosci());
            oknoDodajGoscia.ShowDialog();
        }

        private void btnDodajRezerwacje_Click(object sender, RoutedEventArgs e)
        {
            var listaPokoi = wczytajListePokoi();
            Pracownik domyslnyPracownik = new Pracownik(); 
            bool automatycznyDoborPokoju = true; 

            var oknoDodajRezerwacje = new DodajRezerwacje(wczytajRejestrGosci(), wczytajListePokoi(), domyslnyPracownik, automatycznyDoborPokoju);
            oknoDodajRezerwacje.ShowDialog();
        }


        private void btnPodgladPokoi_Click(object sender, RoutedEventArgs e)
        {
            var oknoPodgladPokoi = new PodgladPokoi();
            oknoPodgladPokoi.ShowDialog();
        }

        private void btnPodgladGosci_Click(object sender, RoutedEventArgs e)
        {
            var oknoPodgladGosci = new PodgladGosci();
            oknoPodgladGosci.ShowDialog();
        }

        private void btnPodgladPracownikow_Click(object sender, RoutedEventArgs e)
        {
            var oknoPodgladPracownikow = new PodgladPracownikow();
            oknoPodgladPracownikow.ShowDialog();
        }

        private void btnListaPobytow_Click(object sender, RoutedEventArgs e)
        {
            var oknoListaPobytow = new ListaPobytow();
            oknoListaPobytow.ShowDialog();
        }


    }
}
