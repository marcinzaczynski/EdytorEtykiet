using EdytorEtykiet.Helpers;
using EdytorEtykiet.Interfaces;

namespace EdytorEtykiet.Model
{
    public class NowaEtykietaModel : INowyElement, INowaEtykieta
    {
        #region FIELDS & PROPERTIES

        #region INowyElement

        private int idPola = 0;
        public int IdPola { get { return idPola; } set { idPola = value; } }

        private TypyPol typPola = TypyPol.Canvas;
        public TypyPol TypPola { get { return typPola; } }

        private string nazwa = "Etykieta";
        public string Nazwa { get { return nazwa; } set { nazwa = value; } }

        #endregion
        #region INowaEtykieta

        private int szerokosc = 400;
        public int Szerokosc { get { return szerokosc; } set { szerokosc = value; } }

        private int wysokosc = 200;
        public int Wysokosc { get { return wysokosc; } set { wysokosc = value; } }

        #endregion

        #endregion
    }
}
