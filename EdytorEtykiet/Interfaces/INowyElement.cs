using EdytorEtykiet.Helpers;

namespace EdytorEtykiet.Interfaces
{
    public interface INowyElement
    {
        int IdPola { get; set; }
        TypyPol TypPola { get; }
        string Nazwa { get; set; }
    }
}
