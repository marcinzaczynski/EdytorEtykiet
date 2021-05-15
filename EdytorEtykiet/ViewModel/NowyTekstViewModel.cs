using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Globalization;

namespace EdytorEtykiet.ViewModel
{
    public class NowyTekstViewModel : INotifyPropertyChanged
    {
        #region region PROPERTY CHANGE
      
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
        #region region PROPERTIES

        private bool txtCzyEdycja = false;
        public bool TXTCzyEdycja { get { return txtCzyEdycja; } set { txtCzyEdycja = value; } }

        private int txtIdPola;
        public int TXTIdPola { get { return txtIdPola; } set { txtIdPola = value; UstawNazwe(txtIdPola); } }

        private string txtNazwa;
        public string TXTNazwa { get { return txtNazwa; } set { txtNazwa = value; OnPropertyChanged("TXTNazwa"); } }

        private string txtWidocznyTekst = "pole tekstowe";
        public string TXTWidocznyTekst { get { return txtWidocznyTekst; } set { txtWidocznyTekst = value; OnPropertyChanged("TXTWidocznyTekst"); } }

        private bool txtCzyJestRamka;
        public bool TXTCzyJestRamka { get { return txtCzyJestRamka; } set { txtCzyJestRamka = value; OnPropertyChanged("TXTCzyJestRamka");  if (value) { TXTKolorRamki = new SolidColorBrush(Colors.Black); TXTGruboscRamki = 1; } else { TXTKolorRamki = new SolidColorBrush(Colors.White); TXTGruboscRamki = 1; } } }

        private SolidColorBrush txtKolorRamki = new SolidColorBrush(Colors.White);
        public SolidColorBrush TXTKolorRamki { get { return txtKolorRamki; } set { txtKolorRamki = value; OnPropertyChanged("TXTKolorRamki"); } }

        private int txtGruboscRamki;
        public int TXTGruboscRamki { get { return txtGruboscRamki; } set { txtGruboscRamki = value; OnPropertyChanged("TXTGruboscRamki"); } }

        private FontFamily txtFontFamily = new FontFamily("MS Sans Serif");
        public FontFamily TXTFontFamily { get { return txtFontFamily; } set { txtFontFamily = value; OnPropertyChanged("TXTFontFamily"); } }

        private float txtFontSize = 10;
        public float TXTFontSize { get { return txtFontSize; } set { txtFontSize = value; OnPropertyChanged("TXTFontSize"); } }

        private FontWeight txtFontWeight = FontWeights.Normal;
        public FontWeight TXTFontWeight { get { return txtFontWeight; } set { txtFontWeight = value; OnPropertyChanged("TXTFontWeight"); } }

        private FontStyle txtFontStyle = FontStyles.Normal;
        public FontStyle TXTFontStyle { get { return txtFontStyle; } set { txtFontStyle = value; OnPropertyChanged("TXTFontStyle"); } }

        private double txtWysokosc;
        public double TXTWysokosc { get { return txtWysokosc; }  set { txtWysokosc = value; OnPropertyChanged("TXTWysokosc"); } }

        private double txtSzerokosc;
        public double TXTSzerokosc { get { return txtSzerokosc; } set { txtSzerokosc = value; OnPropertyChanged("TXTSzerokosc"); } }

        private bool txtAutoDopasowanie = false;
        public bool TXTAutoDopasowanie { get { return txtAutoDopasowanie; } set { txtAutoDopasowanie = value; OnPropertyChanged("TXTAutoDopasowanie"); DopasujRozmiar(); } }

        private System.Windows.HorizontalAlignment txtWyrownanieWPoziomie;
        public System.Windows.HorizontalAlignment TXTWyrownanieWPoziomie { get { return txtWyrownanieWPoziomie; } set { txtWyrownanieWPoziomie = value; OnPropertyChanged("TXTWyrownanieWPoziomie"); } }

        private System.Windows.VerticalAlignment txtWyrownanieWPionie;
        public System.Windows.VerticalAlignment TXTWyrownanieWPionie { get { return txtWyrownanieWPionie; } set { txtWyrownanieWPionie = value; OnPropertyChanged("TXTWyrownanieWPionie"); } }

        private FontDialog czcionka;
        public FontDialog Czcionkas { get { return czcionka; } set { czcionka = value; OnPropertyChanged("Czcionka"); } }

        #endregion
        // ========================= MAIN ======================================
        public NowyTekstViewModel()
        { }

        private void UstawNazwe(int _id)
        {
            TXTNazwa = "TXT_" + _id.ToString().PadLeft(3, '0');
        }

        private void DopasujRozmiar()
        {
            if (TXTAutoDopasowanie)
            {
                var formattedText = new FormattedText(
                    TXTWidocznyTekst,
                    CultureInfo.CurrentCulture,
                    System.Windows.FlowDirection.LeftToRight,
                    new Typeface(TXTFontFamily, TXTFontStyle, TXTFontWeight, FontStretches.Medium),
                    TXTFontSize,
                    Brushes.Black,
                    new NumberSubstitution(),
                    1
                    );

                TXTSzerokosc = Math.Round(formattedText.Width, 0);
                TXTWysokosc = Math.Round(formattedText.Height, 0);

            }
        }

        public void Odswiez(NowyTekstViewModel dc)
        {
            //TXTCzyEdycja 
            TXTIdPola = dc.TXTIdPola;
            TXTNazwa = dc.TXTNazwa;
            TXTWidocznyTekst = dc.TXTWidocznyTekst;
            TXTCzyJestRamka = dc.TXTCzyJestRamka;
            TXTKolorRamki = dc.TXTKolorRamki;
            TXTGruboscRamki = dc.TXTGruboscRamki;
            TXTFontFamily = dc.TXTFontFamily;
            TXTFontSize = dc.TXTFontSize;
            TXTFontWeight = dc.TXTFontWeight;
            TXTFontStyle = dc.TXTFontStyle;
            TXTWysokosc = dc.TXTWysokosc;
            TXTSzerokosc = dc.TXTSzerokosc;
            TXTAutoDopasowanie = dc.TXTAutoDopasowanie;
            TXTWyrownanieWPoziomie = dc.TXTWyrownanieWPoziomie;
            TXTWyrownanieWPionie = dc.TXTWyrownanieWPionie;
        }
    }
}
