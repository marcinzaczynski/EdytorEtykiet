using BarcodeLib;
using System.Windows;
using System.Windows.Media;

namespace EdytorEtykiet.Interfaces
{
    public interface INowyKodKr
    {
        string Tekst { get; set; }
        TYPE Typ { get; set; }
        bool CzyPokazacTekst { get; set; }
        int Szerokosc { get; set; }
        int Wysokosc { get; set; }
        FontFamily FontFamily { get; set; }
        float FontSize { get; set; }
        FontWeight FontWeight { get; set; }
    }
}
