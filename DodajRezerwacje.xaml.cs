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
    /// Logika interakcji dla klasy DodajRezerwacje.xaml
    /// </summary>
    public partial class DodajRezerwacje : Window
    {
        private RejestrGosci rejestrGosci;
        private ListaPokoi listaPokoi;
        private Pracownik zakladajacyRezerwacje;
        private bool automatycznyDoborPokoju;

        public DodajRezerwacje(RejestrGosci rejestr, ListaPokoi lista, Pracownik pracownik, bool automatycznyDobor)
        {
            InitializeComponent();
            rejestrGosci = rejestr;
            listaPokoi = lista;
            zakladajacyRezerwacje = pracownik;
            automatycznyDoborPokoju = automatycznyDobor;
            WypelnijListeGosci();
            WypelnijListePokoi();
            WypelnijListeDodatkowychGosci();
        }

        private void WypelnijListeGosci()
        {
            cbGosc.ItemsSource = rejestrGosci.ListaGosci;
            cbGosc.DisplayMemberPath = "Nazwisko"; 
            cbGosc.SelectedValuePath = "Pesel"; 
        }

        private void UtworzRezerwacje_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var wybranyGosc = cbGosc.SelectedItem as Gosc;
                var wybranyPokoj = cbPokoj.SelectedItem as Pokoj; 
                var dodatkowiGoscie = lbDodatkowiGoscie.SelectedItems.Cast<Gosc>().ToList();

                DateTime poczatek = dpPoczatek.SelectedDate.Value;
                DateTime koniec = dpKoniec.SelectedDate.Value;

                wybranyGosc.ZalozRezerwacje(poczatek, koniec, dodatkowiGoscie, zakladajacyRezerwacje, listaPokoi, automatycznyDoborPokoju, wybranyPokoj?.IdPokoju);

                MessageBox.Show("Rezerwacja utworzona pomyślnie");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd: " + ex.Message);
            }
        }

        private void WypelnijListePokoi()
        {
            cbPokoj.ItemsSource = listaPokoi.pokoje; 
            cbPokoj.DisplayMemberPath = "IdPokoju"; 
        }

        private void WypelnijListeDodatkowychGosci()
        {
            lbDodatkowiGoscie.ItemsSource = rejestrGosci.ListaGosci;
            lbDodatkowiGoscie.DisplayMemberPath = "Nazwisko"; 
        }

    }

}
