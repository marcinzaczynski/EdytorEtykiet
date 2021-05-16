using EdytorEtykiet.Helpers;
using System.Windows.Controls;
using System.Windows.Media;

namespace EdytorEtykiet.Interfaces
{
    public interface INowyObraz
    {
        int IdPola { get; set; }
        TypyPol TypPola { get; }
        string PelnaSciezka { get; set; }
        int KatObrotu { get; set; }
        double Proporcja { get; set; }
        Image Obraz { get; set; }
        double Width { get; set; }
        double Height { get; set; }
        Stretch Stretch { get; set; }
    }
}
