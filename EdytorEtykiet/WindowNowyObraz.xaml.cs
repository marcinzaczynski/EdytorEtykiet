using EdytorEtykiet.Model;
using EdytorEtykiet.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace EdytorEtykiet
{
    /// <summary>
    /// Interaction logic for WindowNowyObraz.xaml
    /// </summary>
    public partial class WindowNowyObraz : Window
    {
        public static event DodajNowyElementDelegat NowyObrazEvent;
        public static event EdytujElementDelegat EdytujEvent;

        public NowyObrazModel NowyObraz = new NowyObrazModel();

        public WindowNowyObraz(NowyObrazModel nowy_obraz = null, int id_pola = 0)
        {
            InitializeComponent();
            if (nowy_obraz != null)
            {
                NowyObrazVM.CzyEdycja = true;
                NowyObrazVM.IdPola = nowy_obraz.IdPola;
                NowyObrazVM.Nazwa = nowy_obraz.Nazwa;
                NowyObrazVM.PelnaSciezka = nowy_obraz.PelnaSciezka;
                NowyObrazVM.KatObrotu = nowy_obraz.KatObrotu;
                NowyObrazVM.Source = new ImageSourceConverter().ConvertFromString(nowy_obraz.PelnaSciezka) as ImageSource;
                Obroc(NowyObrazVM.KatObrotu);
            }
            else
            {
                NowyObrazVM.IdPola = id_pola;
            }
        }

        private void CommandOk_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            NowyObraz.IdPola = NowyObrazVM.IdPola;
            NowyObraz.Nazwa = NowyObrazVM.Nazwa;
            NowyObraz.PelnaSciezka = NowyObrazVM.PelnaSciezka;
            NowyObraz.KatObrotu = NowyObrazVM.KatObrotu;
            

            var nameExist = MainWindow.ListaElementow2.Where(c => c.Nazwa == NowyObrazVM.Nazwa).FirstOrDefault();

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

            ImgPodglad.LayoutTransform = rotateTransform;
        }
    }
}
