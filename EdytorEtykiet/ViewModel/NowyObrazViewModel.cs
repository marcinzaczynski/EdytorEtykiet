using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using EdytorEtykiet.Helpers;
using EdytorEtykiet.Model;

namespace EdytorEtykiet.ViewModel
{
    public class NowyObrazViewModel : INotifyPropertyChanged
    {
        // ========================= PROPERTY CHANGE ===========================
        #region region PROPERTY CHANGE
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
        // ========================= PROPERTIES ================================
        #region region PROPERTIES
        private bool _CzyEdycja = false;
        public bool CzyEdycja { get { return _CzyEdycja; } set { _CzyEdycja = value; } }

        private int _IdPola;
        public int IdPola { get { return _IdPola; } set { _IdPola = value; UstawNazwe(IdPola); } }
        
        private string _Nazwa;
        public string Nazwa { get { return _Nazwa; } set { _Nazwa = value; OnPropertyChanged("Nazwa"); } }

        private double _Szerokosc;
        public double Szerokosc { get { return _Szerokosc; } set { _Szerokosc = value; OnPropertyChanged("Szerokosc"); } }

        private double _Wysokosc;
        public double Wysokosc { get { return _Wysokosc; } set { _Wysokosc = value; OnPropertyChanged("Wysokosc"); } }

        private double _Proporcja;
        public double Proporcja { get { return _Proporcja; } set { _Proporcja = value; } }

        private ImageSource _Obraz;
        public ImageSource Obraz { get { return _Obraz; } set { _Obraz = value; OnPropertyChanged("Obraz"); } }

        private string _PelnaSciezka;
        public string PelnaSciezka { get { return _PelnaSciezka; } set { _PelnaSciezka = value; OnPropertyChanged("PelnaSciezka"); } }

        private string _NazwaPliku;
        public string NazwaPliku { get { return _NazwaPliku; } set { _NazwaPliku = value; OnPropertyChanged("NazwaPliku"); } }

        private int _KatObrotu = 0;
        public int KatObrotu { get { return _KatObrotu; } set { _KatObrotu = value; OnPropertyChanged("Obrot"); } }
        #endregion
        // ========================= MAIN ======================================
        #region region MAIN
        public NowyObrazViewModel()
        {
        }

        private void UstawNazwe(int _id)
        {
            Nazwa = "OBR_" + _id.ToString().PadLeft(3, '0');
        }

        public void Odswiez(NowyObrazViewModel dc)
        {
            //TXTCzyEdycja 
            IdPola = dc.IdPola;
            Nazwa = dc.Nazwa;
            Obraz = dc.Obraz;
            Szerokosc = dc.Szerokosc;
            Wysokosc = dc.Wysokosc;
            NazwaPliku = dc.NazwaPliku;
            PelnaSciezka = dc.PelnaSciezka;
            KatObrotu = dc.KatObrotu;

        }

        public bool WczytajObraz()
        {
            var imgSciezka = Globals.WybierzObraz();
            if (imgSciezka != null)
            {
                PelnaSciezka = imgSciezka;
                NazwaPliku = Path.GetFileName(imgSciezka);
                
                Obraz = new ImageSourceConverter().ConvertFromString(imgSciezka) as ImageSource;
                Proporcja = (Obraz.Width / Obraz.Height);
                Szerokosc = Obraz.Width;
                Wysokosc = Obraz.Height;


            }
            return false;
        }

        public void DostosujWysokosc()
        {
            Wysokosc = Math.Round((Szerokosc / Proporcja), 0);
        }

        public void DostosujSzerokosc()
        {
            Szerokosc = Math.Round((Wysokosc * Proporcja), 0);
        }

        private void Obroc()
        {
            RotateTransform rotateTransform = new RotateTransform(_KatObrotu);

            //Obraz2.LayoutTransform = rotateTransform;
            //Wysokosc = Obraz.Width;
            //Szerokosc = Obraz.Height;
        }
        #endregion
    }
}
