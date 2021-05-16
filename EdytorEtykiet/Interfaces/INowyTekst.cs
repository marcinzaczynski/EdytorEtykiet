using System.Windows;
using System.Windows.Media;

namespace EdytorEtykiet.Interfaces
{
    public interface INowyTekst
    {
        string Tekst { get; set; }
        bool CzyJestRamka { get; set; }
        SolidColorBrush KolorRamki { get; set; }
        Thickness GruboscRamki { get; set; }
        FontFamily FontFamily { get; set; }
        float FontSize { get; set; }
        FontWeight FontWeight { get; set; }
        FontStyle FontStyle { get; set; }
        double Wysokosc { get; set; }
        double Szerokosc { get; set; }
        bool AutoDopasowanie { get; set; }
        HorizontalAlignment WyrownanieWPoziomie { get; set; }
        VerticalAlignment WyrownanieWPionie { get; set; }

    }
}
