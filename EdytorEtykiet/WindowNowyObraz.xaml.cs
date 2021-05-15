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
using EdytorEtykiet.Model;
using EdytorEtykiet.ViewModel;
using EdytorEtykiet.Helpers;

namespace EdytorEtykiet
{
    /// <summary>
    /// Interaction logic for WindowNowyObraz.xaml
    /// </summary>
    public partial class WindowNowyObraz : Window
    {
        public static event DodajNowyElementDelegat NowyObrazEvent;
        public static event EdytujElementDelegat EdytujEvent;

        public NowyObrazModel NowyObraz;

        public WindowNowyObraz(NowyObrazModel nowy_obraz = null, int id_pola = 0)
        {
            InitializeComponent();
            if (nowy_obraz != null)
            {

                var dc = nowy_obraz.DataContext as NowyObrazViewModel;
                NowyObrazVM.Odswiez(dc);
                NowyObrazVM.CzyEdycja = true;
                Obroc(NowyObrazVM.KatObrotu);
            }
            else
            {
                NowyObrazVM.IdPola = id_pola;
            }
            NowyObraz = new NowyObrazModel();
            NowyObraz.IdPola = NowyObrazVM.IdPola;
            NowyObraz.DataContext = NowyObrazVM;
        }

        private void CommandOk_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //AppHandler.BindData(NowyObraz); // tylko pola z klasy Image
            
            NowyObraz.Width = NowyObrazVM.Szerokosc;
            NowyObraz.Height = NowyObrazVM.Wysokosc;
            NowyObraz.Source = NowyObrazVM.Obraz;
            NowyObraz.Name = NowyObrazVM.Nazwa;

            NowyObraz.PelnaSciezka = NowyObrazVM.PelnaSciezka;
            NowyObraz.NazwaPliku = NowyObrazVM.NazwaPliku;
            NowyObraz.KatObrotu = NowyObrazVM.KatObrotu;
            NowyObraz.Stretch = Stretch.Uniform;

            var nameExist = MainWindow.ListaElementow.Where(c => c.Name == NowyObrazVM.Nazwa).FirstOrDefault();

            if (NowyObrazVM.CzyEdycja)
            {
                EdytujEvent?.Invoke(NowyObraz);
            }
            else
            {
                if (nameExist == null)
                {
                    NowyObrazEvent?.Invoke(NowyObraz, 2, 2, false);
                }
                else
                {
                    System.Windows.MessageBox.Show($"Element o nazwie {NowyObrazVM.Nazwa} jest już dodany. Zmień nazwę.", "Zatwierdzenie zmian.", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            Close();
        }

        private void CommandOk_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NowyObrazVM.Nazwa) && !string.IsNullOrEmpty(NowyObrazVM.PelnaSciezka))
            {
                e.CanExecute = true;
            }
        }

        private void CommandAnuluj_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NowyObraz = null;
            Close();
        }

        private void CommandAnuluj_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void btnWybierzSciezke_Click(object sender, RoutedEventArgs e)
        {
            NowyObrazVM.WczytajObraz();
        }

        private void btnObroc_Click(object sender, RoutedEventArgs e)
        {
            NowyObrazVM.KatObrotu = NowyObrazVM.KatObrotu % 360;
            NowyObrazVM.KatObrotu = NowyObrazVM.KatObrotu + 90;
            Obroc(NowyObrazVM.KatObrotu);
        }

        private void Obroc(int _kat)
        {
            
            RotateTransform rotateTransform = new RotateTransform(_kat);

            //ImgPodglad.RenderTransform = rotateTransform;
            ImgPodglad.LayoutTransform = rotateTransform;
            //NowyObrazVM.Szerokosc = ImgPodglad.ActualHeight;
            //NowyObrazVM.Wysokosc = ImgPodglad.ActualWidth;
            //Canvas.SetLeft(ImgPodglad, 0);
            //Canvas.SetTop(ImgPodglad, 0);
        }
    }
}
