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
            rejestrGosci = RejestrGosci.OdczytXml("rejestrGosci.xml");
            listaPokoi = ListaPokoi.OdczytXml("listaPokoi.xml");
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
                var dodatkowiGoscie = lbDodatkowiGoscie.SelectedItems.Cast<Gosc>().ToList();
                DateTime poczatek = dpPoczatek.SelectedDate.Value;
                DateTime koniec = dpKoniec.SelectedDate.Value;

                if (cbPokoj.SelectedIndex == -1) // Automatyczny dobór pokoju
                {
                    wybranyGosc.ZalozRezerwacje(poczatek, koniec, dodatkowiGoscie, zakladajacyRezerwacje, listaPokoi);
                    
                }
                else // Ręczny dobór pokoju
                {
                    var wybranyPokoj = cbPokoj.SelectedItem as Pokoj;
                    wybranyGosc.ZalozRezerwacje(poczatek, koniec, dodatkowiGoscie, zakladajacyRezerwacje, listaPokoi, false, wybranyPokoj?.IdPokoju);
                    // sprawdz czy sie zgadza
                    
                }
                MessageBox.Show("Utworzono rezerwację: \n" + wybranyGosc.ListaPobytow.Last().ToString());
                rejestrGosci.ZapisXml("rejestrGosci.xml");
                listaPokoi.ZapisXml("listaPokoi.xml");
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
