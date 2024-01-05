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
        public static ListaPokoi? wczytajListePokoi() //Zaczytanie listy pokojów z pliku XML
        {
            ListaPokoi? listaPokoi = ListaPokoi.OdczytXml("listaPokoi.xml");
            if (listaPokoi is null) { MessageBox.Show("Błąd wczytywania wykazu pokojów."); }
            return listaPokoi;
        }

        public MainWindow()
        {
            InitializeComponent();
            ListaPokoi lp = wczytajListePokoi();
            Pracownik p1 = new Pracownik("Jan", "Kowalski", "12345678901", new Adres("Szeroka", 34, "Kraków", "32-032"), new DateTime(1960, 4, 12), EnumWydzial.Administracja, new DateTime(1979, 01, 01));
            Gosc g1 = new Gosc("Kamil", "Stoch", "12345678910", new Adres("Chałubińskiego", 12, "Zakopane", "32-001"),new DateTime(1983, 5, 8), new List<Pobyt>());
            Gosc g2 = new Gosc("Jan", "Stoch", "12345612910", new Adres("Chałubińskiego", 80, "Zakopane", "32-001"), new DateTime(1980, 5, 8), new List<Pobyt>());
            Gosc g3 = new Gosc("Grzegorz", "Stoch", "12345668910", new Adres("Szeroka", 12, "Warszawa", "32-001"), new DateTime(2010, 5, 8), new List<Pobyt>());
            g1.ZalozRezerwacje(new DateTime(2024, 01, 20), new DateTime(2024, 01, 25), new List<Gosc>(2){ g2, g3 }, p1, lp);
        
            //Pobyt pob1 = new Pobyt(new DateTime(2024, 01, 20), new DateTime(2024, 01, 25), lp.WybierzPokoj("11A"), 3, g1, new List<Gosc>(){ g2,g3 }, p1 );

            TextBox1.Text = g1.ToString();
            //TextBox2.Text = pob1.ToString();
            //TextBox3.Text = "";
        }

        private void MainWindow_Shown(Object sender, EventArgs e)
        {
            
        }

    }
}
