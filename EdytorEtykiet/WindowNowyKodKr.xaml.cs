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

        public NowyKodKrModel NowyKodKr;
        public WindowNowyKodKr(NowyKodKrModel nowy_kodkr = null, int id_pola = 0)
        {
            InitializeComponent();
            if (nowy_kodkr != null)
            {

                var dc = nowy_kodkr.DataContext as NowyKodKrViewModel;
                NowyKodKrVM.Odswiez(dc);
                NowyKodKrVM.CzyEdycja = true;
            }
            else
            {
                NowyKodKrVM.IdPola = id_pola;
            }
            NowyKodKr = new NowyKodKrModel();
            NowyKodKr.IdPola = NowyKodKrVM.IdPola;
            NowyKodKr.DataContext = NowyKodKrVM;

            NowyKodKrVM.ListaTypow = BarcodeHandler.PobierzListeTypow();
        }

        #region COMMANDS

        private void CommandOk_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AppHandler.BindData(NowyKodKr); // tylko pola z klasy Image

            NowyKodKr.Tekst = NowyKodKrVM.Tekst;
            NowyKodKr.Typ = NowyKodKrVM.Typ;
            NowyKodKr.CzyPokazacTekst = NowyKodKrVM.CzyPokazacTekst;
            var nameExist = MainWindow.ListaElementow.Where(c => c.Name == NowyKodKrVM.Nazwa).FirstOrDefault();



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
            if (NowyKodKrVM.Obraz != null)
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
