using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

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

        private bool czyEdycja = false;
        public bool CzyEdycja { get { return czyEdycja; } set { czyEdycja = value; } }

        private int idPola;
        public int IdPola { get { return idPola; } set { idPola = value; UstawNazwe(idPola); } }

        private string nazwa;
        public string Nazwa { get { return nazwa; } set { nazwa = value; OnPropertyChanged("Nazwa"); } }

        private string tekst = "pole tekstowe";
        public string Tekst { get { return tekst; } set { tekst = value; OnPropertyChanged("Tekst"); } }

        private bool czyJestRamka;
        public bool CzyJestRamka { get { return czyJestRamka; } set { czyJestRamka = value; OnPropertyChanged("CzyJestRamka"); if (value) { KolorRamki = new SolidColorBrush(Colors.Black); GruboscRamki = new Thickness(1); } else { KolorRamki = new SolidColorBrush(Colors.White); GruboscRamki = new Thickness(1); } } }

        private SolidColorBrush kolorRamki = new SolidColorBrush(Colors.White);
        public SolidColorBrush KolorRamki { get { return kolorRamki; } set { kolorRamki = value; OnPropertyChanged("KolorRamki"); } }

        private Thickness gruboscRamki;
        public Thickness GruboscRamki { get { return gruboscRamki; } set { gruboscRamki = value; OnPropertyChanged("GruboscRamki"); } }

        private FontFamily fontFamily = new FontFamily("MS Sans Serif");
        public FontFamily FontFamily { get { return fontFamily; } set { fontFamily = value; OnPropertyChanged("FontFamily"); } }

        private float fontSize = 10;
        public float FontSize { get { return fontSize; } set { fontSize = value; OnPropertyChanged("FontSize"); } }

        private FontWeight fontWeight = FontWeights.Normal;
        public FontWeight FontWeight { get { return fontWeight; } set { fontWeight = value; OnPropertyChanged("FontWeight"); } }

        private FontStyle fontStyle = FontStyles.Normal;
        public FontStyle FontStyle { get { return fontStyle; } set { fontStyle = value; OnPropertyChanged("FontStyle"); } }

        private double wysokosc;
        public double Wysokosc { get { return wysokosc; } set { wysokosc = value; OnPropertyChanged("Wysokosc"); } }

        private double szerokosc;
        public double Szerokosc { get { return szerokosc; } set { szerokosc = value; OnPropertyChanged("Szerokosc"); } }

        private bool autoDopasowanie = false;
        public bool AutoDopasowanie { get { return autoDopasowanie; } set { autoDopasowanie = value; OnPropertyChanged("AutoDopasowanie"); DopasujRozmiar(); } }

        private System.Windows.HorizontalAlignment wyrownanieWPoziomie;
        public System.Windows.HorizontalAlignment WyrownanieWPoziomie { get { return wyrownanieWPoziomie; } set { wyrownanieWPoziomie = value; OnPropertyChanged("WyrownanieWPoziomie"); } }

        private System.Windows.VerticalAlignment wyrownanieWPionie;
        public System.Windows.VerticalAlignment WyrownanieWPionie { get { return wyrownanieWPionie; } set { wyrownanieWPionie = value; OnPropertyChanged("WyrownanieWPionie"); } }

        private FontDialog czcionka;
        public FontDialog Czcionkas { get { return czcionka; } set { czcionka = value; OnPropertyChanged("Czcionka"); } }

        #endregion
        // ========================= MAIN ======================================
        public NowyTekstViewModel()
        { }

        private void UstawNazwe(int _id)
        {
            Nazwa = "TXT_" + _id.ToString().PadLeft(3, '0');
        }

        private void DopasujRozmiar()
        {
            if (AutoDopasowanie)
            {
                var formattedText = new FormattedText(
                    Tekst,
                    CultureInfo.CurrentCulture,
                    System.Windows.FlowDirection.LeftToRight,
                    new Typeface(FontFamily, FontStyle, FontWeight, FontStretches.Medium),
                    FontSize,
                    Brushes.Black,
                    new NumberSubstitution(),
                    1
                    );

                Szerokosc = formattedText.Width;
                Wysokosc = formattedText.Height;

            }
        }

        public void Odswiez(NowyTekstViewModel dc)
        {
            //TXTCzyEdycja 
            IdPola = dc.IdPola;
            Nazwa = dc.Nazwa;
            Tekst = dc.Tekst;
            CzyJestRamka = dc.CzyJestRamka;
            KolorRamki = dc.KolorRamki;
            GruboscRamki = dc.GruboscRamki;
            FontFamily = dc.FontFamily;
            FontSize = dc.FontSize;
            FontWeight = dc.FontWeight;
            FontStyle = dc.FontStyle;
            Wysokosc = dc.Wysokosc;
            Szerokosc = dc.Szerokosc;
            AutoDopasowanie = dc.AutoDopasowanie;
            WyrownanieWPoziomie = dc.WyrownanieWPoziomie;
            WyrownanieWPionie = dc.WyrownanieWPionie;
        }
    }
}
