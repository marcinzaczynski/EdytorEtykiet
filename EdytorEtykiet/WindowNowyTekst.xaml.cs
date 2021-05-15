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
using System.Windows.Forms;
using EdytorEtykiet.ViewModel;
using EdytorEtykiet.Helpers;
using EdytorEtykiet.Model;

namespace EdytorEtykiet
{
    /// <summary>
    /// Interaction logic for WindowNowyTekst.xaml
    /// </summary>
    public partial class WindowNowyTekst : Window
    {
        public static event DodajNowyElementDelegat NowyTekstEvent;
        public static event EdytujElementDelegat EdytujEvent;

        public NowyTekstModel NowyTekst;

        FontDialog fontDialog = new FontDialog();

        //public WindowNowyTekst(System.Windows.Controls.Label nowy_tekst = null, int id_pola = 0)
        public WindowNowyTekst(NowyTekstModel nowy_tekst = null, int id_pola = 0)
        {
            InitializeComponent();

            if (nowy_tekst != null)
            {
                
                var dc = nowy_tekst.DataContext as NowyTekstViewModel;
                NowyTekstVM.Odswiez(dc);
                NowyTekstVM.TXTCzyEdycja = true;
            } else
            { 
                NowyTekstVM.TXTIdPola = id_pola; 
            }

            
            NowyTekst = new NowyTekstModel();
            NowyTekst.IdPola = NowyTekstVM.TXTIdPola;
            NowyTekst.Padding = new Thickness(0,0,0,0);
            NowyTekst.DataContext = NowyTekstVM;
        }

        private void CommandOk_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //NowyTekstVM.Czcionkas = fontDialog;
            //NowyTekst.Name = NowyTekstVM.TXTNazwa;
            AppHandler.BindData(NowyTekst);

            var nameExist = MainWindow.ListaElementow.Where(c => c.Name == NowyTekstVM.TXTNazwa).FirstOrDefault();

            

            if (NowyTekstVM.TXTCzyEdycja)
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
                    System.Windows.MessageBox.Show($"Element o nazwie {NowyTekstVM.TXTNazwa} jest już dodany. Zmień nazwę.", "Zatwierdzenie zmian.", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            Close();
        }

        private void CommandOk_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NowyTekstVM.TXTNazwa))
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

        private void ButtonZmienCzcionke(object sender, RoutedEventArgs e)
        {
            fontDialog.ShowColor = false;
            fontDialog.ShowApply = false;
            fontDialog.ShowEffects = false;
            fontDialog.ShowHelp = false;
            fontDialog.AllowScriptChange = false;
            if (fontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                NowyTekstVM.TXTFontFamily = new FontFamily(fontDialog.Font.Name);
                NowyTekstVM.TXTFontSize = fontDialog.Font.Size;
                NowyTekstVM.TXTFontWeight = fontDialog.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                NowyTekstVM.TXTFontStyle = fontDialog.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !AppHandler.IsTextAllowed(e.Text);
        }

        private void WyrPozL_Button_Click(object sender, RoutedEventArgs e)
        {
            NowyTekstVM.TXTWyrownanieWPoziomie = System.Windows.HorizontalAlignment.Left;
        }

        private void WyrPozS_Button_Click(object sender, RoutedEventArgs e)
        {
            NowyTekstVM.TXTWyrownanieWPoziomie = System.Windows.HorizontalAlignment.Center;
        }

        private void WyrPozP_Button_Click(object sender, RoutedEventArgs e)
        {
            NowyTekstVM.TXTWyrownanieWPoziomie = System.Windows.HorizontalAlignment.Right;
        }

        private void WyrPionG_Button_Click(object sender, RoutedEventArgs e)
        {
            NowyTekstVM.TXTWyrownanieWPionie = System.Windows.VerticalAlignment.Top;
        }

        private void WyrPionS_Button_Click(object sender, RoutedEventArgs e)
        {
            NowyTekstVM.TXTWyrownanieWPionie = System.Windows.VerticalAlignment.Center;
        }

        private void WyrPionD_Button_Click(object sender, RoutedEventArgs e)
        {
            NowyTekstVM.TXTWyrownanieWPionie = System.Windows.VerticalAlignment.Bottom;
        }
    }
}
