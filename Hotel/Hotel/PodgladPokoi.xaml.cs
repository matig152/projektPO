using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private RejestrGosci rejestrGosci;

        public PodgladPokoi()
        {
            InitializeComponent();
            listaPokoi = ListaPokoi.OdczytXml("listaPokoi.xml") ?? new ListaPokoi();
            WypelnijListePokoi();
            cbRozmiar.ItemsSource = new ObservableCollection<int>() { 1, 2, 3, 4, 5 };
            cbBudynek.ItemsSource = new ObservableCollection<EnumBudynek>() {EnumBudynek.A, EnumBudynek.B, EnumBudynek.C, EnumBudynek.D };
            cbPietro.ItemsSource = new ObservableCollection<EnumPietro>() { EnumPietro.pierwsze, EnumPietro.drugie, EnumPietro.trzecie };



        }

        private void WypelnijListePokoi()
        {
            lvPokoje.ItemsSource = listaPokoi.pokoje;
        }

        private void Filtruj_Click(object sender, RoutedEventArgs e)
        {
            int rozmiar = cbRozmiar.SelectedIndex + 1;
            int idxBudynek = cbBudynek.SelectedIndex;
            int idxPietro = cbPietro.SelectedIndex;
            List<EnumBudynek> listaBud = new List<EnumBudynek>() { EnumBudynek.A, EnumBudynek.B, EnumBudynek.C, EnumBudynek.D };
            List<EnumPietro> listaPietr = new List<EnumPietro>() { EnumPietro.pierwsze, EnumPietro.drugie, EnumPietro.trzecie };

            //wszystkie parametry podane
            if (rozmiar > 0 && idxBudynek > -1 && idxPietro > -1) { lvPokoje.ItemsSource = listaPokoi.pokoje.Where(x => x.Rozmiar == rozmiar && x.Budynek == listaBud[idxBudynek] && x.Pietro == listaPietr[idxPietro]);return; }
            //sam rozmiar
            if (rozmiar > 0 && idxBudynek == -1 && idxPietro == -1) { lvPokoje.ItemsSource = listaPokoi.pokoje.Where(x => x.Rozmiar == rozmiar); return; }
            //sam budynek
            if (rozmiar == 0 && idxBudynek > -1 && idxPietro == -1) { lvPokoje.ItemsSource = listaPokoi.pokoje.Where(x => x.Budynek == listaBud[idxBudynek]); return; }
            //samo pietro
            if (rozmiar == 0 && idxBudynek == -1 && idxPietro > -1) { lvPokoje.ItemsSource = listaPokoi.pokoje.Where(x =>  x.Pietro == listaPietr[idxPietro]); return; }
            //rozmiar i budynek
            if (rozmiar > 0 && idxBudynek > -1 && idxPietro == -1) { lvPokoje.ItemsSource = listaPokoi.pokoje.Where(x => x.Rozmiar == rozmiar && x.Budynek == listaBud[idxBudynek]); return; }
            //rozmiar i pietro
            if (rozmiar > 0 && idxBudynek == -1 && idxPietro > -1) { lvPokoje.ItemsSource = listaPokoi.pokoje.Where(x => x.Rozmiar == rozmiar && x.Pietro == listaPietr[idxPietro]); return; }
            //pietro i budynek
            if (rozmiar == 0 && idxBudynek > -1 && idxPietro > -1) { lvPokoje.ItemsSource = listaPokoi.pokoje.Where(x => x.Budynek == listaBud[idxBudynek] && x.Pietro == listaPietr[idxPietro]); return; }
        }

        private void Odswiez_Click(object sender, RoutedEventArgs e)
        {
            WypelnijListePokoi(); 
        }
    }

}
