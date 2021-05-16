using EdytorEtykiet.Helpers;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Media;

namespace EdytorEtykiet.ViewModel
{
    public class NowyObrazViewModel : INotifyPropertyChanged
    {
        #region PROPERTY CHANGE

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
        #region FIELDS &PROPERTIES

        private bool czyEdycja = false;
        public bool CzyEdycja { get { return czyEdycja; } set { czyEdycja = value; } }

        private int idPola;
        public int IdPola { get { return idPola; } set { idPola = value; UstawNazwe(IdPola); } }

        private string nazwa;
        public string Nazwa { get { return nazwa; } set { nazwa = value; OnPropertyChanged("Nazwa"); } }

        private double width;
        public double Width { get { return width; } set { width = value; OnPropertyChanged("Width"); } }

        private double height;
        public double Height { get { return height; } set { height = value; OnPropertyChanged("Height"); } }

        private double proporcja;
        public double Proporcja { get { return proporcja; } set { proporcja = value; } }

        private ImageSource source;
        public ImageSource Source { get { return source; } set { source = value; OnPropertyChanged("Source"); } }

        private string pelnaSciezka;
        public string PelnaSciezka { get { return pelnaSciezka; } set { pelnaSciezka = value; OnPropertyChanged("PelnaSciezka"); } }

        private string nazwaPliku;
        public string NazwaPliku { get { return nazwaPliku; } set { nazwaPliku = value; WyodrebnijNazwePliku(); OnPropertyChanged("NazwaPliku"); } }

        private int katObrotu = 0;
        public int KatObrotu { get { return katObrotu; } set { katObrotu = value; OnPropertyChanged("KatObrot"); } }

        private Stretch stretch = Stretch.Uniform;
        public Stretch Stretch { get { return stretch; } set { stretch = value; } }

        #endregion
        #region MAIN

        private void UstawNazwe(int _id)
        {
            Nazwa = "OBR_" + _id.ToString().PadLeft(3, '0');
        }

        public bool WczytajObraz()
        {
            var imgSciezka = Globals.WybierzObraz();
            if (imgSciezka != null)
            {
                PelnaSciezka = imgSciezka;

                Source = new ImageSourceConverter().ConvertFromString(imgSciezka) as ImageSource;
                Proporcja = (Source.Width / Source.Height);
                Width = Source.Width;
                Height = Source.Height;
            }
            return false;
        }

        private void WyodrebnijNazwePliku()
        {
            NazwaPliku = Path.GetFileName(PelnaSciezka);
        }

        public void DostosujWysokosc()
        {
            Height = Math.Round((Width / Proporcja), 0);
        }

        public void DostosujSzerokosc()
        {
            Width = Math.Round((Height * Proporcja), 0);
        }

        private void Obroc()
        {
            RotateTransform rotateTransform = new RotateTransform(KatObrotu);

            //Obraz2.LayoutTransform = rotateTransform;
            //Wysokosc = Obraz.Width;
            //Szerokosc = Obraz.Height;
        }

        #endregion
    }
}
