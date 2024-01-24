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
            List<Gosc> listaGosci = rejestrGosci.ListaGosci;
            List<string> listaNazwisk = new List<string>();
            foreach (Gosc g in listaGosci) { listaNazwisk.Add($"{g.Imie} {g.Nazwisko} ({g.Pesel})"); }
            cbGosc.ItemsSource = listaNazwisk;
        }

        private void UtworzRezerwacje_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Zbierz goscia 
                var wybranyGoscStr = cbGosc.SelectedItem as string;
                Gosc? wybranyGosc = null;
                foreach (Gosc g in rejestrGosci.ListaGosci) { if (wybranyGoscStr == $"{g.Imie} {g.Nazwisko} ({g.Pesel})") { wybranyGosc = g; }; }
                if(wybranyGosc == null) { MessageBox.Show("Błąd wyboru gościa rezerwującego"); }
                //Zbierz zakladajacego
                var zakladajacyString = cbZakladajacy.SelectedItem as string;
                Pracownik? zakladajacy = null;
                foreach(Pracownik p in kdr.ListaPracownikow) { if (zakladajacyString == $"{p.Imie} {p.Nazwisko} ({p.Pesel})") { zakladajacy = p; } ; }
                if(zakladajacy == null) { MessageBox.Show("Błąd wybierania pracownika"); }
                //Zbierz dodatkowych gosci
                var dodatkowiGoscieStr = lbDodatkowiGoscie.SelectedItems.Cast<string>().ToList();
                List<Gosc> dodatkowiGoscie = new List<Gosc>();
                foreach(string goscString in dodatkowiGoscieStr)
                {
                    foreach(Gosc g in rejestrGosci.ListaGosci) { if (goscString == $"{g.Imie} {g.Nazwisko} ({g.Pesel})") { dodatkowiGoscie.Add(g); } ; }
                }


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
                MessageBox.Show($"Utworzono rezerwację dla gościa {wybranyGosc.Imie} {wybranyGosc.Nazwisko}: \n"  + wybranyGosc.ListaPobytow.Last().ToString());
                rejestrGosci.ZapisXml("rejestrGosci.xml");
                listaPokoi.ZapisXml("listaPokoi.xml");
                //W CELU ODŚWIEŻENIA STATYSTYK
                Application.Current.MainWindow.Close();
                MainWindow mw = new MainWindow();
                Application.Current.MainWindow = mw;
                mw.Show();
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
            List<Gosc> listaGosci = rejestrGosci.ListaGosci;
            List<string> listaNazwisk = new List<string>();
            foreach (Gosc g in listaGosci) { listaNazwisk.Add($"{g.Imie} {g.Nazwisko} ({g.Pesel})"); }
            lbDodatkowiGoscie.ItemsSource = listaNazwisk;
        }

        private void WypelnijListePracownikow()
        {
            List<Pracownik> listaPracownikow = kdr.ListaPracownikow;
            List<string> listaNazwisk = new List<string>();
            foreach (Pracownik p in listaPracownikow) { listaNazwisk.Add($"{p.Imie} {p.Nazwisko} ({p.Pesel})"); }
            cbZakladajacy.ItemsSource = listaNazwisk;
        }

    }

}
