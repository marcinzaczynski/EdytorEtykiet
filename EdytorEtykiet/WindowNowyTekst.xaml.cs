using SimpleLabelLibrary.Models;
using SimpleLabelLibrary.Helpers;
using EdytorEtykiet.Helpers;
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
        public static event DodajNowyElementDelegat NowyTekstEvent;
        public static event EdytujElementDelegat EdytujEvent;
        public static event FieldExistsDelegate FieldExistsEvent;

        public TextField NowyTekst = new TextField();

        FontDialog fontDialog = new FontDialog();

        public WindowNowyTekst(TextField nowy_tekst = null, int id_pola = 0)
        {
            InitializeComponent();

            if (nowy_tekst != null)
            {
                NowyTekstVM.CzyEdycja = true;
                NowyTekstVM.IdPola = nowy_tekst.Id;
                NowyTekstVM.Nazwa = nowy_tekst.Name;
                NowyTekstVM.Tekst = nowy_tekst.Text;
                NowyTekstVM.CzyJestRamka = nowy_tekst.ShowBorder;
                NowyTekstVM.KolorRamki = nowy_tekst.BorderColor;
                NowyTekstVM.GruboscRamki = nowy_tekst.BorderWidth;
                NowyTekstVM.FontFamily = nowy_tekst.FontFamily;
                NowyTekstVM.FontSize = nowy_tekst.FontSize;
                NowyTekstVM.FontWeight = nowy_tekst.FontWeight;
                NowyTekstVM.FontStyle = nowy_tekst.FontStyle;
                NowyTekstVM.Wysokosc = nowy_tekst.Height;
                NowyTekstVM.Szerokosc = nowy_tekst.Width;
                NowyTekstVM.AutoDopasowanie = nowy_tekst.AutoFit;
                NowyTekstVM.WyrownanieWPoziomie = nowy_tekst.HorizontalAlignement;
                NowyTekstVM.WyrownanieWPionie = nowy_tekst.VerticalAlignement;
            }
            else
            {
                NowyTekstVM.IdPola = id_pola;
            }

        }

        #region COMMANDS

        private void CommandOk_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NowyTekst.Id = NowyTekstVM.IdPola;
            NowyTekst.Name = NowyTekstVM.Nazwa;
            NowyTekst.Text = NowyTekstVM.Tekst;
            NowyTekst.ShowBorder = NowyTekstVM.CzyJestRamka;
            NowyTekst.BorderColor = NowyTekstVM.KolorRamki;
            NowyTekst.BorderWidth = NowyTekstVM.GruboscRamki;
            NowyTekst.FontFamily = NowyTekstVM.FontFamily;
            NowyTekst.FontSize = NowyTekstVM.FontSize;
            NowyTekst.FontWeight = NowyTekstVM.FontWeight;
            NowyTekst.FontStyle = NowyTekstVM.FontStyle;
            NowyTekst.Height = NowyTekstVM.Wysokosc;
            NowyTekst.Width = NowyTekstVM.Szerokosc;
            NowyTekst.AutoFit = NowyTekstVM.AutoDopasowanie;
            NowyTekst.HorizontalAlignement = NowyTekstVM.WyrownanieWPoziomie;
            NowyTekst.VerticalAlignement = NowyTekstVM.WyrownanieWPionie;

            var nameExists = FieldExistsEvent?.Invoke(FieldTypes.Text, NowyTekstVM.Nazwa);



            if (NowyTekstVM.CzyEdycja)
            {
                EdytujEvent?.Invoke(NowyTekst);
            }
            else
            {
                if (nameExists == null)
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
