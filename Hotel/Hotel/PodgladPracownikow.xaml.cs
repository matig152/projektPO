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
    /// Logika interakcji dla klasy PodgladPracownikow.xaml
    /// </summary>
    public partial class PodgladPracownikow : Window
    {
        private Kadra kadra;

        public PodgladPracownikow()
        {
            InitializeComponent();
            kadra = Kadra.OdczytXml("kadra.xml") ?? new Kadra();
            cbWydzial.ItemsSource = Enum.GetValues(typeof(EnumWydzial)).Cast<EnumWydzial>();
            WypelnijListePracownikow();
        }

        private void WypelnijListePracownikow()
        {
            lvPracownicy.ItemsSource = kadra.ListaPracownikow;
        }

        private void Filtruj_Click(object sender, RoutedEventArgs e)
        {
            EnumWydzial wybranyWydzial;
            if (cbWydzial.SelectedIndex > -1)
            {
                wybranyWydzial = (EnumWydzial)Enum.Parse(typeof(EnumWydzial), cbWydzial.SelectedItem.ToString());
            }
            else
            {
                WypelnijListePracownikow();
                return;
            }

            var wynik = kadra.FiltrujPracownikowPoWydziale(wybranyWydzial);
            lvPracownicy.ItemsSource = wynik;
        }

        private void Odswiez_Click(object sender, RoutedEventArgs e)
        {
            WypelnijListePracownikow();
        }
    }

}
