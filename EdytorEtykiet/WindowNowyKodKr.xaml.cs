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
    /// Interaction logic for WindowNowyKodKr.xaml
    /// </summary>
    public partial class WindowNowyKodKr : Window
    {
        public static event DodajNowyElementDelegat NowyKodKrEvent;
        public static event EdytujElementDelegat EdytujEvent;

        FontDialog fontDialog = new FontDialog();

        public NowyKodKrModel NowyKodKr = new NowyKodKrModel();
        public WindowNowyKodKr(NowyKodKrModel nowy_kodkr = null, int id_pola = 0)
        {
            InitializeComponent();
            if (nowy_kodkr != null)
            {
                NowyKodKrVM.CzyEdycja = true;
                NowyKodKrVM.IdPola = nowy_kodkr.IdPola;
                NowyKodKrVM.Nazwa = nowy_kodkr.Nazwa;
                NowyKodKrVM.Tekst = nowy_kodkr.Tekst;
                NowyKodKrVM.Typ = nowy_kodkr.Typ;
                NowyKodKrVM.Szerokosc = nowy_kodkr.Szerokosc;
                NowyKodKrVM.Wysokosc = nowy_kodkr.Wysokosc;
                NowyKodKrVM.CzyPokazacTekst = nowy_kodkr.CzyPokazacTekst;
                NowyKodKrVM.FontFamily = nowy_kodkr.FontFamily;
                NowyKodKrVM.FontSize = nowy_kodkr.FontSize;
                NowyKodKrVM.FontWeight = nowy_kodkr.FontWeight;


                NowyKodKrVM.GenerujPodglad();
            }
            else
            {
                NowyKodKrVM.IdPola = id_pola;
            }
            NowyKodKr = new NowyKodKrModel();
            NowyKodKr.IdPola = NowyKodKrVM.IdPola;

            NowyKodKrVM.ListaTypow = BarcodeHandler.PobierzListeTypow();
        }

        #region COMMANDS

        private void CommandOk_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NowyKodKr.IdPola = NowyKodKrVM.IdPola;
            NowyKodKr.Nazwa = NowyKodKrVM.Nazwa;
            NowyKodKr.Tekst = NowyKodKrVM.Tekst;
            NowyKodKr.Typ = NowyKodKrVM.Typ;
            NowyKodKr.CzyPokazacTekst = NowyKodKrVM.CzyPokazacTekst;
            NowyKodKr.Szerokosc = NowyKodKrVM.Szerokosc;
            NowyKodKr.Wysokosc = NowyKodKrVM.Wysokosc;
            NowyKodKr.FontFamily = NowyKodKrVM.FontFamily;
            NowyKodKr.FontSize = NowyKodKrVM.FontSize;
            NowyKodKr.FontWeight = NowyKodKrVM.FontWeight;

            var nameExist = MainWindow.ListaElementow2.Where(c => c.Nazwa == NowyKodKrVM.Nazwa).FirstOrDefault();

            if (NowyKodKrVM.CzyEdycja)
            {
                EdytujEvent?.Invoke(NowyKodKr);
            }
            else
            {
                if (nameExist == null)
                {
                    NowyKodKrEvent?.Invoke(NowyKodKr, 2, 2, false);
                }
                else
                {
                    System.Windows.MessageBox.Show($"Element o nazwie {NowyKodKrVM.Nazwa} jest już dodany. Zmień nazwę.", "Zatwierdzenie zmian.", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            Close();
        }

        private void CommandOk_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (NowyKodKrVM.Source != null)
            {
                e.CanExecute = true;
            }
        }

        private void CommandAnuluj_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NowyKodKr = null;
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
                NowyKodKrVM.FontFamily = new FontFamily(fontDialog.Font.Name);
                NowyKodKrVM.FontSize = fontDialog.Font.Size;
                NowyKodKrVM.FontWeight = fontDialog.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
            }
        }
    }
}
