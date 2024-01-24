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

            //Statystyki

            txtBlLiczbaGosci.Text = $"Liczba gości: {rejestrGosci.ListaGosci.Count()}";
            txtBlLiczbaPobytów.Text = $"Liczba pracowników: {kadra.ListaPracownikow.Count()}";
            int liczbaPobytow = 0;
            foreach (Gosc g in rejestrGosci.ListaGosci) { liczbaPobytow += g.ListaPobytow.Count(); }
            txtBlLiczbaPracowników.Text = $"Liczba pobytów: {liczbaPobytow}";


        }

        private void btnDodajGoscia_Click(object sender, RoutedEventArgs e)
        {
            var oknoDodajGoscia = new DodajGoscia(wczytajRejestrGosci());
            oknoDodajGoscia.ShowDialog();
        }

        private void btnDodajRezerwacje_Click(object sender, RoutedEventArgs e)
        {
            var oknoDodajRezerwacje = new DodajRezerwacje(wczytajRejestrGosci(), wczytajListePokoi(), wczytajKadre());
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
