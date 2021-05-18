using BarcodeLib;
using EdytorEtykiet.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;


namespace EdytorEtykiet.ViewModel
{
    public class NowyKodKrViewModel : INotifyPropertyChanged
    {
        #region PROPERTY CHANGE

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
        #region PROPERTIES

        private bool czyEdycja = false;
        public bool CzyEdycja { get { return czyEdycja; } set { czyEdycja = value; } }

        private int idPola;
        public int IdPola { get { return idPola; } set { idPola = value; UstawNazwe(IdPola); } }
        
        private string nazwa;
        public string Nazwa { get { return nazwa; } set { nazwa = value; OnPropertyChanged("Nazwa"); } }
        
        private string tekst = "0123456789123";
        public string Tekst { get { return tekst; } set { tekst = value; OnPropertyChanged("Tekst"); GenerujPodglad(); } }
        
        private List<TYPE> listaTypow;
        public List<TYPE> ListaTypow { get { return listaTypow; } set { listaTypow = value; OnPropertyChanged("ListaTypow"); } }
        
        private TYPE typ = TYPE.EAN13;
        public TYPE Typ { get { return typ; } set { typ = value; OnPropertyChanged("Typ"); GenerujPodglad(); } }
        
        private int szerokosc = 100;
        public int Szerokosc { get { return szerokosc; } set { szerokosc = value; OnPropertyChanged("Szerokosc"); GenerujPodglad(); } }
        
        private int wysokosc = 100;
        public int Wysokosc { get { return wysokosc; } set { wysokosc = value; OnPropertyChanged("Wysokosc"); GenerujPodglad(); } }
        
        private ImageSource source;
        public ImageSource Source { get { return source; } set { source = value; OnPropertyChanged("Source"); } }
        
        private bool czyPokazacTekst = true;
        public bool CzyPokazacTekst { get { return czyPokazacTekst; } set { czyPokazacTekst = value; OnPropertyChanged("CzyPokazacTekst"); GenerujPodglad(); } }
        
        private FontFamily fontFamily = new FontFamily("Microsoft Sans Serif");
        public FontFamily FontFamily { get { return fontFamily; } set { fontFamily = value; OnPropertyChanged("FontFamily"); } }
        
        private float fontSize = 10;
        public float FontSize { get { return fontSize; } set { fontSize = value; OnPropertyChanged("FontSize"); } }
        
        private FontWeight fontWeight = FontWeights.Normal;
        public FontWeight FontWeight { get { return fontWeight; } set { fontWeight = value; OnPropertyChanged("FontWeight"); } }

        private string info;
        public string Info { get { return info; } set { info = value; OnPropertyChanged("Info"); } }

        #endregion
        #region MAIN

        public NowyKodKrViewModel()
        {
            GenerujPodglad();
        }

        private void UstawNazwe(int _id)
        {
            Nazwa = "KODKR_" + _id.ToString().PadLeft(3, '0');
        }

        public void GenerujPodglad()
        {
            try
            {
                Source = BarcodeHandler.UtworzKod(Typ, Tekst, Szerokosc, Wysokosc, CzyPokazacTekst);
                Info = "";
            }
            catch (Exception ex)
            {
                Source = null;
                Info = ex.Message;
            }
        }

        #endregion
    }
}
