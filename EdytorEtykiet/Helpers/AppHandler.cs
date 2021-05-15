using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Data;

using EdytorEtykiet.Model;

namespace EdytorEtykiet.Helpers
{
    class AppHandler
    {
        public static void BindData(Label _nowy_tekst)
        {
            Binding b1 = new Binding("TXTKolorRamki");
            BindingOperations.SetBinding(_nowy_tekst, Label.BorderBrushProperty, b1);

            Binding b2 = new Binding("TXTGruboscRamki");
            BindingOperations.SetBinding(_nowy_tekst, Label.BorderThicknessProperty, b2);

            Binding b3 = new Binding("TXTSzerokosc");
            BindingOperations.SetBinding(_nowy_tekst, Label.WidthProperty, b3);

            Binding b4 = new Binding("TXTWysokosc");
            BindingOperations.SetBinding(_nowy_tekst, Label.HeightProperty, b4);

            Binding b5 = new Binding("TXTWidocznyTekst");
            BindingOperations.SetBinding(_nowy_tekst, Label.ContentProperty, b5);

            Binding b6 = new Binding("TXTFontFamily");
            BindingOperations.SetBinding(_nowy_tekst, Label.FontFamilyProperty, b6);

            Binding b7 = new Binding("TXTFontSize");
            BindingOperations.SetBinding(_nowy_tekst, Label.FontSizeProperty, b7);

            Binding b8 = new Binding("TXTFontWeight");
            BindingOperations.SetBinding(_nowy_tekst, Label.FontWeightProperty, b8);

            Binding b9 = new Binding("TXTFontStyle");
            BindingOperations.SetBinding(_nowy_tekst, Label.FontStyleProperty, b9);

            Binding b10 = new Binding("TXTNazwa");
            BindingOperations.SetBinding(_nowy_tekst, Label.NameProperty, b10);

        }
        public static void BindData(Image _nowy_kodkr)
        {
            Binding b1 = new Binding("Szerokosc");
            BindingOperations.SetBinding(_nowy_kodkr, Image.WidthProperty, b1);

            Binding b2 = new Binding("Wysokosc");
            BindingOperations.SetBinding(_nowy_kodkr, Image.HeightProperty, b2);

            Binding b3 = new Binding("Obraz");
            BindingOperations.SetBinding(_nowy_kodkr, Image.SourceProperty, b3);

            Binding b4 = new Binding("Nazwa");
            BindingOperations.SetBinding(_nowy_kodkr, Image.NameProperty, b4);
        }

        // TYLKO LICZBY 
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        public static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
    }
}
