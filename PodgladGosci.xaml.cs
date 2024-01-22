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
    /// Logika interakcji dla klasy PodgladGosci.xaml
    /// </summary>
    public partial class PodgladGosci : Window
    {
        private RejestrGosci rejestrGosci;

        public PodgladGosci()
        {
            InitializeComponent();
            rejestrGosci = RejestrGosci.OdczytXml("rejestrGosci.xml") ?? new RejestrGosci();
            WypelnijListeGosci();
        }

        private void WypelnijListeGosci()
        {
            lvGoscie.ItemsSource = rejestrGosci.ListaGosci;
        }

        private void Filtruj_Click(object sender, RoutedEventArgs e)
        {
            var filtr = txtFiltrNazwisko.Text.ToLower();
            var wynik = rejestrGosci.ListaGosci.Where(g => g.Nazwisko.ToLower().Contains(filtr)).ToList();
            lvGoscie.ItemsSource = wynik;
        }

        private void Odswiez_Click(object sender, RoutedEventArgs e)
        {
            WypelnijListeGosci();
        }
    }

}
