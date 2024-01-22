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
    /// Logika interakcji dla klasy PodgladPokoi.xaml
    /// </summary>
    public partial class PodgladPokoi : Window
    {
        private ListaPokoi listaPokoi;

        public PodgladPokoi()
        {
            InitializeComponent();
            listaPokoi = ListaPokoi.OdczytXml("listaPokoi.xml") ?? new ListaPokoi();
            WypelnijListePokoi();
        }

        private void WypelnijListePokoi()
        {
            lvPokoje.ItemsSource = listaPokoi.pokoje;
        }

        private void Filtruj_Click(object sender, RoutedEventArgs e)
        {
            DateTime poczatek = dpPoczatek.SelectedDate.GetValueOrDefault(DateTime.MinValue);
            DateTime koniec = dpKoniec.SelectedDate.GetValueOrDefault(DateTime.MaxValue);

            var zajetePokoje = ListaPokoi.PokojeZajeteWPodanymCzasie(poczatek, koniec, "nazwaRejestru.xml");
            lvPokoje.ItemsSource = zajetePokoje;
        }

        private void Odswiez_Click(object sender, RoutedEventArgs e)
        {
            WypelnijListePokoi(); 
        }
    }

}
