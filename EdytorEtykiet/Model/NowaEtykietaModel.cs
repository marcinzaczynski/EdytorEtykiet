using EdytorEtykiet.Helpers;

namespace EdytorEtykiet.Model
{
    public class NowaEtykietaModel
    {
        private int id_pola;
        public int IdPola { get { return id_pola; } set { id_pola = value; } }

        private TypyPol typPola = TypyPol.Canvas;
        public TypyPol TypPola { get { return typPola; } }
    }
}
