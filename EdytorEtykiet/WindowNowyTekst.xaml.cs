using EdytorEtykiet.Helpers;
using EdytorEtykiet.Model;
using EdytorEtykiet.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace EdytorEtykiet
{
    /// <summary>
    /// Interaction logic for WindowNowyTekst.xaml
    /// </summary>
    public partial class WindowNowyTekst : Window
    {
        public static event DodajNowyElementDelegat2 NowyTekstEvent;
        public static event EdytujElementDelegat2 EdytujEvent;

        public NowyTekstModel NowyTekst = new NowyTekstModel();

        FontDialog fontDialog = new FontDialog();

        public WindowNowyTekst(NowyTekstModel nowy_tekst = null, int id_pola = 0)
        {
            InitializeComponent();

            if (nowy_tekst != null)
            {
                NowyTekstVM.CzyEdycja = true;
                NowyTekstVM.IdPola = nowy_tekst.IdPola;
                NowyTekstVM.Nazwa = nowy_tekst.Nazwa;
                NowyTekstVM.Tekst = nowy_tekst.Tekst;
                NowyTekstVM.CzyJestRamka = nowy_tekst.CzyJestRamka;
                NowyTekstVM.KolorRamki = nowy_tekst.KolorRamki;
                NowyTekstVM.GruboscRamki = nowy_tekst.GruboscRamki;
                NowyTekstVM.FontFamily = nowy_tekst.FontFamily;
                NowyTekstVM.FontSize = nowy_tekst.FontSize;
                NowyTekstVM.FontWeight = nowy_tekst.FontWeight;
                NowyTekstVM.FontStyle = nowy_tekst.FontStyle;
                NowyTekstVM.Wysokosc = nowy_tekst.Wysokosc;
                NowyTekstVM.Szerokosc = nowy_tekst.Szerokosc;
                NowyTekstVM.AutoDopasowanie = nowy_tekst.AutoDopasowanie;
                NowyTekstVM.WyrownanieWPoziomie = nowy_tekst.WyrownanieWPoziomie;
                NowyTekstVM.WyrownanieWPionie = nowy_tekst.WyrownanieWPionie;
            }
            else
            {
                NowyTekstVM.IdPola = id_pola;
            }

        }

        #region COMMANDS

        private void CommandOk_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NowyTekst.IdPola = NowyTekstVM.IdPola;
            NowyTekst.Nazwa = NowyTekstVM.Nazwa;
            NowyTekst.Tekst = NowyTekstVM.Tekst;
            NowyTekst.CzyJestRamka = NowyTekstVM.CzyJestRamka;
            NowyTekst.KolorRamki = NowyTekstVM.KolorRamki;
            NowyTekst.GruboscRamki = NowyTekstVM.GruboscRamki;
            NowyTekst.FontFamily = NowyTekstVM.FontFamily;
            NowyTekst.FontSize = NowyTekstVM.FontSize;
            NowyTekst.FontWeight = NowyTekstVM.FontWeight;
            NowyTekst.FontStyle = NowyTekstVM.FontStyle;
            NowyTekst.Wysokosc = NowyTekstVM.Wysokosc;
            NowyTekst.Szerokosc = NowyTekstVM.Szerokosc;
            NowyTekst.AutoDopasowanie = NowyTekstVM.AutoDopasowanie;
            NowyTekst.WyrownanieWPoziomie = NowyTekstVM.WyrownanieWPoziomie;
            NowyTekst.WyrownanieWPionie = NowyTekstVM.WyrownanieWPionie;  

            var nameExist = MainWindow.ListaElementow.Where(c => c.Name == NowyTekstVM.Nazwa).FirstOrDefault();



            if (NowyTekstVM.CzyEdycja)
            {
                EdytujEvent?.Invoke(NowyTekst);
            }
            else
            {
                if (nameExist == null)
                {
                    NowyTekstEvent?.Invoke(NowyTekst, 2, 2, false);
                }
                else
                {
                    System.Windows.MessageBox.Show($"Element o nazwie {NowyTekstVM.Nazwa} jest już dodany. Zmień nazwę.", "Zatwierdzenie zmian.", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            Close();
        }

        private void CommandOk_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NowyTekstVM.Nazwa))
            {
                e.CanExecute = true;
            }
        }

        private void CommandAnuluj_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NowyTekst = null;
            Close();
        }

        private void CommandAnuluj_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

        private void ButtonZmienCzcionke(object sender, RoutedEventArgs e)
        {
            fontDialog.ShowColor = false;
            fontDialog.ShowApply = false;
            fontDialog.ShowEffects = false;
            fontDialog.ShowHelp = false;
            fontDialog.AllowScriptChange = false;
            if (fontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                NowyTekstVM.FontFamily = new FontFamily(fontDialog.Font.Name);
                NowyTekstVM.FontSize = fontDialog.Font.Size;
                NowyTekstVM.FontWeight = fontDialog.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                NowyTekstVM.FontStyle = fontDialog.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !AppHandler.IsTextAllowed(e.Text);
        }

        private void WyrPozL_Button_Click(object sender, RoutedEventArgs e)
        {
            NowyTekstVM.WyrownanieWPoziomie = System.Windows.HorizontalAlignment.Left;
        }

        private void WyrPozS_Button_Click(object sender, RoutedEventArgs e)
        {
            NowyTekstVM.WyrownanieWPoziomie = System.Windows.HorizontalAlignment.Center;
        }

        private void WyrPozP_Button_Click(object sender, RoutedEventArgs e)
        {
            NowyTekstVM.WyrownanieWPoziomie = System.Windows.HorizontalAlignment.Right;
        }

        private void WyrPionG_Button_Click(object sender, RoutedEventArgs e)
        {
            NowyTekstVM.WyrownanieWPionie = System.Windows.VerticalAlignment.Top;
        }

        private void WyrPionS_Button_Click(object sender, RoutedEventArgs e)
        {
            NowyTekstVM.WyrownanieWPionie = System.Windows.VerticalAlignment.Center;
        }

        private void WyrPionD_Button_Click(object sender, RoutedEventArgs e)
        {
            NowyTekstVM.WyrownanieWPionie = System.Windows.VerticalAlignment.Bottom;
        }
    }
}
