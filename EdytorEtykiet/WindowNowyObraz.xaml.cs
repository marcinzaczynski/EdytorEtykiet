using SimpleLabelLibrary.Models;
using SimpleLabelLibrary.Helpers;
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
        public static event FieldExistsDelegate FieldExistsEvent;

        public PictureField NowyObraz = new PictureField();

        public WindowNowyObraz(PictureField nowy_obraz = null, int id_pola = 0)
        {
            InitializeComponent();
            if (nowy_obraz != null)
            {
                NowyObrazVM.CzyEdycja = true;
                NowyObrazVM.IdPola = nowy_obraz.Id;
                NowyObrazVM.Nazwa = nowy_obraz.Name;
                NowyObrazVM.PelnaSciezka = nowy_obraz.Path;
                NowyObrazVM.KatObrotu = nowy_obraz.RotationAngle;
                NowyObrazVM.Source = new ImageSourceConverter().ConvertFromString(nowy_obraz.Path) as ImageSource;
                Obroc(NowyObrazVM.KatObrotu);
            }
            else
            {
                NowyObrazVM.IdPola = id_pola;
            }
        }

        private void CommandOk_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            NowyObraz.Id = NowyObrazVM.IdPola;
            NowyObraz.Name = NowyObrazVM.Nazwa;
            NowyObraz.Path = NowyObrazVM.PelnaSciezka;
            NowyObraz.RotationAngle = NowyObrazVM.KatObrotu;


            var nameExists = FieldExistsEvent?.Invoke(FieldTypes.Picture, NowyObrazVM.Nazwa);

            if (NowyObrazVM.CzyEdycja)
            {
                EdytujEvent?.Invoke(NowyObraz);
            }
            else
            {
                if (nameExists == null)
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
