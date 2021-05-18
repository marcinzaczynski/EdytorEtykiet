using BarcodeLib;
using EdytorEtykiet.Helpers;
using EdytorEtykiet.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EdytorEtykiet.Model
{
    public class NowyKodKrModel : INowyElement, INowyKodKr
    {
        #region FIELDS & PROPERTIES

        #region INowyElement

        private int id_pola;
        public int IdPola { get { return id_pola; } set { id_pola = value; } }

        private TypyPol typPola = TypyPol.Barcode;
        public TypyPol TypPola { get { return typPola; } }

        private string nazwa;
        public string Nazwa { get { return nazwa; } set { nazwa = value; } }

        #endregion
        #region INowyKodKR

        private string _Tekst;
        public string Tekst { get { return _Tekst; } set { _Tekst = value; } }

        private TYPE _Typ;
        public TYPE Typ { get { return _Typ; } set { _Typ = value; } }

        private bool _CzyPokazacTekst = true;
        public bool CzyPokazacTekst { get { return _CzyPokazacTekst; } set { _CzyPokazacTekst = value; } }

        private int szerokosc;
        public int Szerokosc { get { return szerokosc; } set { szerokosc = value; } }

        private int wysokosc;
        public int Wysokosc { get { return wysokosc; } set { wysokosc = value; } }

        private FontFamily fontFamily;
        public FontFamily FontFamily { get { return fontFamily; } set { fontFamily = value; } }

        private float fontSize;
        public float FontSize { get { return fontSize; } set { fontSize = value; } }

        private FontWeight fontWeight;
        public FontWeight FontWeight { get { return fontWeight; } set { fontWeight = value; } }

        #endregion

        #endregion
    }
}
