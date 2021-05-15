using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media;
using EdytorEtykiet.Helpers;
using BarcodeStandard;
using BarcodeLib;
using System.Windows;


namespace EdytorEtykiet.ViewModel
{
    public class NowyKodKrViewModel : INotifyPropertyChanged
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
        private int _IdPola;
        private string _Nazwa;
        private string _Tekst = "0123456789123";
        private List<TYPE> _ListaTypow;
        private TYPE _Typ = TYPE.EAN13;
        private int _Szerokosc = 100;
        private int _Wysokosc = 100;
        private ImageSource _Obraz;
        private bool _CzyPokazacTekst = true;
        private FontFamily _FontFamily = new FontFamily("Microsoft Sans Serif");
        private float _FontSize = 10;
        private FontWeight _FontWeight = FontWeights.Normal;
        private string _Info;
        public bool CzyEdycja { get { return _CzyEdycja; } set { _CzyEdycja = value; } }
        public int IdPola { get { return _IdPola; } set { _IdPola = value; UstawNazwe(IdPola); } }
        public string Nazwa { get { return _Nazwa; } set { _Nazwa = value; OnPropertyChanged("Nazwa"); } }
        public string Tekst { get { return _Tekst; } set { _Tekst = value; OnPropertyChanged("Tekst"); GenerujPodglad(); } }
        public List<TYPE> ListaTypow { get { return _ListaTypow; } set { _ListaTypow = value; OnPropertyChanged("ListaTypow"); } }
        public TYPE Typ { get { return _Typ; } set { _Typ = value; OnPropertyChanged("Typ"); GenerujPodglad(); } }
        public int Szerokosc { get { return _Szerokosc; } set { _Szerokosc = value; OnPropertyChanged("Szerokosc"); GenerujPodglad(); } }
        public int Wysokosc { get { return _Wysokosc; } set { _Wysokosc = value; OnPropertyChanged("Wysokosc"); GenerujPodglad(); } }
        public ImageSource Obraz { get { return _Obraz; } set { _Obraz = value; OnPropertyChanged("Obraz"); } }
        public bool CzyPokazacTekst { get { return _CzyPokazacTekst; } set { _CzyPokazacTekst = value; OnPropertyChanged("CzyPokazacTekst"); GenerujPodglad(); } }
        public FontFamily FontFamily { get { return _FontFamily; } set { _FontFamily = value; OnPropertyChanged("FontFamily"); } }
        public float FontSize { get { return _FontSize; } set { _FontSize = value; OnPropertyChanged("FontSize"); } }
        public FontWeight FontWeight { get { return _FontWeight; } set { _FontWeight = value; OnPropertyChanged("FontWeight"); } }
        public string Info { get { return _Info; } set { _Info = value; OnPropertyChanged("Info"); } }
        #endregion
        // ========================= MAIN ======================================
        #region region MAIN
        public NowyKodKrViewModel()
        {
            GenerujPodglad();
        }

        private void UstawNazwe(int _id)
        {
            Nazwa = "KODKR_" + _id.ToString().PadLeft(3, '0');
        }

        public void Odswiez(NowyKodKrViewModel dc)
        {
            //TXTCzyEdycja 
            IdPola = dc.IdPola;
            Nazwa = dc.Nazwa;
            Tekst = dc.Tekst;
            Typ = dc.Typ;
            CzyPokazacTekst = dc.CzyPokazacTekst;
            Obraz = dc.Obraz;
            Szerokosc = dc.Szerokosc;
            Wysokosc = dc.Wysokosc;

        }

        public void GenerujPodglad()
        {
            try
            {
                Obraz = BarcodeHandler.UtworzKod(Typ, Tekst, Szerokosc, Wysokosc, CzyPokazacTekst);
                Info = "";
            }
            catch (Exception ex)
            {
                Obraz = null;
                Info = ex.Message;
            }
            
        }
        #endregion
    }
}
