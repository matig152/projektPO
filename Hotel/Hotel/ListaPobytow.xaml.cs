using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hotel
{
    /// <summary>
    /// Logika interakcji dla klasy ListaPobytow.xaml
    /// </summary>
    public partial class ListaPobytow : Window
    {
        private RejestrGosci rejestrGosci;

        public ListaPobytow()
        {
            InitializeComponent();
            rejestrGosci = RejestrGosci.OdczytXml("rejestrGosci.xml") ?? new RejestrGosci();
            WypelnijListePobytow();
        }

        private void WypelnijListePobytow()
        {


            lvPobyty.ItemsSource = rejestrGosci.ListaWszystkichPobytow();
        }

        private void Filtruj_Click(object sender, RoutedEventArgs e)
        {
            DateTime? data = dpData.SelectedDate;
            string goscSzukany = txtGosc.Text.ToLower();
            string numerPokoju = txtNumerPokoju.Text;

            var wynik = rejestrGosci.ListaWszystkichPobytow().Where(pobyt =>
                (!data.HasValue || (pobyt.Poczatek <= data && pobyt.Koniec >= data)) &&
                (string.IsNullOrEmpty(goscSzukany) || rejestrGosci.ListaGosci.Any(gosc =>
                    $"{gosc.Imie} {gosc.Nazwisko}".ToLower().Contains(goscSzukany) &&
                    gosc.ListaPobytow.Contains(pobyt))) &&
                (string.IsNullOrEmpty(numerPokoju) || pobyt.Pokoj.IdPokoju == numerPokoju)).ToList();

            lvPobyty.ItemsSource = wynik;
        }


        private void Odswiez_Click(object sender, RoutedEventArgs e)
        {
            WypelnijListePobytow();
        }
    }

}
