using SimpleLabelLibrary.Helpers;
using EdytorEtykiet.Helpers;
using EdytorEtykiet.ViewModel;
using System.Linq;
using SimpleLabelLibrary.Models;
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
        public static event FieldExistsDelegate FieldExistsEvent;

        FontDialog fontDialog = new FontDialog();

        public BarcodeField NowyKodKr = new BarcodeField();
        public WindowNowyKodKr(BarcodeField nowy_kodkr = null, int id_pola = 0)
        {
            InitializeComponent();
            if (nowy_kodkr != null)
            {
                NowyKodKrVM.CzyEdycja = true;
                NowyKodKrVM.IdPola = nowy_kodkr.Id;
                NowyKodKrVM.Nazwa = nowy_kodkr.Name;
                NowyKodKrVM.Tekst = nowy_kodkr.TextToEncode;
                NowyKodKrVM.Typ = nowy_kodkr.BarcodeType;
                NowyKodKrVM.Szerokosc = nowy_kodkr.Width;
                NowyKodKrVM.Wysokosc = nowy_kodkr.Height;
                NowyKodKrVM.CzyPokazacTekst = nowy_kodkr.ShowTextToEncode;
                NowyKodKrVM.FontFamily = nowy_kodkr.FontFamily;
                NowyKodKrVM.FontSize = nowy_kodkr.FontSize;
                NowyKodKrVM.FontWeight = nowy_kodkr.FontWeight;


                NowyKodKrVM.GenerujPodglad();
            }
            else
            {
                NowyKodKrVM.IdPola = id_pola;
            }
            NowyKodKr = new BarcodeField();
            NowyKodKr.Id = NowyKodKrVM.IdPola;

            NowyKodKrVM.ListaTypow = BarcodeHandler.PobierzListeTypow();
        }

        #region COMMANDS

        private void CommandOk_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NowyKodKr.Id = NowyKodKrVM.IdPola;
            NowyKodKr.Name = NowyKodKrVM.Nazwa;
            NowyKodKr.TextToEncode = NowyKodKrVM.Tekst;
            NowyKodKr.BarcodeType = NowyKodKrVM.Typ;
            NowyKodKr.ShowTextToEncode= NowyKodKrVM.CzyPokazacTekst;
            NowyKodKr.Width = NowyKodKrVM.Szerokosc;
            NowyKodKr.Height = NowyKodKrVM.Wysokosc;
            NowyKodKr.FontFamily = NowyKodKrVM.FontFamily;
            NowyKodKr.FontSize = NowyKodKrVM.FontSize;
            NowyKodKr.FontWeight = NowyKodKrVM.FontWeight;

            var nameExists = FieldExistsEvent?.Invoke(FieldTypes.Barcode, NowyKodKrVM.Nazwa);
            //var nameExist = MainWindow.ListaElementow2.Where(c => c.Nazwa == NowyKodKrVM.Nazwa).FirstOrDefault();

            if (NowyKodKrVM.CzyEdycja)
            {
                EdytujEvent?.Invoke(NowyKodKr);
            }
            else
            {
                if (nameExists == null)
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
