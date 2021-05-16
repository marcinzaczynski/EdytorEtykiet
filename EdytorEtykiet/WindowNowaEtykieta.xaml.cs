using System.Collections.Generic;
using System.Drawing.Printing;
using System.Windows;
using System.Windows.Input;

namespace EdytorEtykiet
{
    /// <summary>
    /// Interaction logic for WindowNowaEtykieta.xaml
    /// </summary>
    public partial class WindowNowaEtykieta : Window
    {
        public bool WindowResult = false;

        public WindowNowaEtykieta()
        {
            InitializeComponent();
            PobierzDrukarki();
        }

        private void PobierzDrukarki()
        {
            NowaEtykietaVM.ZainstalowaneDrukarki = new List<string>();

            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                NowaEtykietaVM.ZainstalowaneDrukarki.Add(printer);
            }
        }

        private void CommandOk_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WindowResult = true;
            Close();
        }

        private void CommandOk_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // NAZWA MUSI BYĆ WPISANA A ROZMIAR ETYKIETY WIĘKSZY OD 0
            if (!string.IsNullOrWhiteSpace(NowaEtykietaVM.NazwaEtykiety) && NowaEtykietaVM.SzerPx > 0 && NowaEtykietaVM.WysPx > 0)
            {
                e.CanExecute = true;
            }
        }

        private void CommandAnuluj_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void CommandAnuluj_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
