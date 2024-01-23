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
        private Kadra kdr;

        public DodajRezerwacje(RejestrGosci rejestr, ListaPokoi lista, Kadra kadra)
        {
            InitializeComponent();
            rejestrGosci = rejestr;
            listaPokoi = lista;
            kdr = kadra;
            WypelnijListeGosci();
            WypelnijListePokoi();
            WypelnijListeDodatkowychGosci();
            WypelnijListePracownikow();
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
                var zakladajacy = cbZakladajacy.SelectedItem as Pracownik;
                var dodatkowiGoscie = lbDodatkowiGoscie.SelectedItems.Cast<Gosc>().ToList();
                DateTime poczatek = dpPoczatek.SelectedDate.Value;
                DateTime koniec = dpKoniec.SelectedDate.Value;

                if (cbPokoj.SelectedIndex == -1) // Automatyczny dobór pokoju
                {
                    wybranyGosc.ZalozRezerwacje(poczatek, koniec, dodatkowiGoscie, zakladajacy, listaPokoi);
                    
                }
                else // Ręczny dobór pokoju
                {
                    var wybranyPokoj = cbPokoj.SelectedItem as Pokoj;
                    
                    int rozmiar = dodatkowiGoscie.Count();
                    rozmiar++;
                    if(wybranyPokoj.Rozmiar != rozmiar) { MessageBox.Show("Niewłaściwy rozmiar pokoju!"); return; }

                    List<Pobyt> listaPobytowPokoju = wybranyPokoj.ListaPobytowWPokoju();
                    if(listaPobytowPokoju.Count() != 0)
                    {
                        foreach(Pobyt p in listaPobytowPokoju)
                        {
                            if(p.CzyNachodzi(poczatek, koniec)) { MessageBox.Show("Pokój zajęty w podanym terminie!");return; }
                        }
                    }
                    wybranyGosc.ZalozRezerwacje(poczatek, koniec, dodatkowiGoscie, zakladajacy, listaPokoi, false, wybranyPokoj?.IdPokoju);
                    
                    
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

        private void WypelnijListePracownikow()
        {
            cbZakladajacy.ItemsSource = kdr.ListaPracownikow.Where(x => x.Wydzial == EnumWydzial.Recepcja || x.Wydzial == EnumWydzial.Administracja);
            cbZakladajacy.DisplayMemberPath = "Nazwisko";
        }

    }

}
