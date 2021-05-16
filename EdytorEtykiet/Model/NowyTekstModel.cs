using EdytorEtykiet.Helpers;
using EdytorEtykiet.Interfaces;
using System.Windows;
using System.Windows.Media;

namespace EdytorEtykiet.Model
{
    public class NowyTekstModel : INowyElement, INowyTekst
    {
        #region FIELDS & PROPERTIES

        #region INowyElement

        private int idPola;
        public int IdPola { get { return idPola; } set { idPola = value; } }

        private TypyPol typPola = TypyPol.Txt;
        public TypyPol TypPola { get { return typPola; } }

        private string nazwa = "pole tekstowe";
        public string Nazwa { get { return nazwa; } set { nazwa = value; } }

        #endregion
        #region INowyTekst

        private string tekst = "pole tekstowe";
        public string Tekst { get { return tekst; } set { tekst = value; } }

        private bool czyJestRamka = false;
        public bool CzyJestRamka { get { return czyJestRamka; } set { czyJestRamka = value; } }

        private SolidColorBrush kolorRamki = Brushes.Black;
        public SolidColorBrush KolorRamki { get { return kolorRamki; } set { kolorRamki = value; } }

        private Thickness gruboscRamki = new Thickness(1);
        public Thickness GruboscRamki { get { return gruboscRamki; } set { gruboscRamki = value; } }

        private FontFamily fontFamily = new FontFamily("MS Sans Serif");
        public FontFamily FontFamily { get { return fontFamily; } set { fontFamily = value; } }

        private float fontSize = 12;
        public float FontSize { get { return fontSize; } set { fontSize = value; } }

        private FontWeight fontWeight = FontWeights.Normal;
        public FontWeight FontWeight { get { return fontWeight; } set { fontWeight = value; } }

        private FontStyle fontStyle = FontStyles.Normal;
        public FontStyle FontStyle { get { return fontStyle; } set { fontStyle = value; } }

        private double wysokosc = 30;
        public double Wysokosc { get { return wysokosc; } set { wysokosc = value; } }

        private double szerokosc = 100;
        public double Szerokosc { get { return szerokosc; } set { szerokosc = value; } }

        private bool autoDopasowanie = false;
        public bool AutoDopasowanie { get { return autoDopasowanie; } set { autoDopasowanie = value; } }

        private HorizontalAlignment wyrownanieWPoziomie = HorizontalAlignment.Left;
        public HorizontalAlignment WyrownanieWPoziomie { get { return wyrownanieWPoziomie; } set { wyrownanieWPoziomie = value; } }

        private VerticalAlignment wyrownanieWPionie = VerticalAlignment.Center;
        public VerticalAlignment WyrownanieWPionie { get { return wyrownanieWPionie; } set { wyrownanieWPionie = value; } }

        #endregion

        #endregion

        public NowyTekstModel() : base()
        {

        }
    }
}
