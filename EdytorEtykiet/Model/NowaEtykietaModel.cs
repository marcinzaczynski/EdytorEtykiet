using EdytorEtykiet.Helpers;

namespace EdytorEtykiet.Model
{
    public class NowaEtykietaModel
    {
        private int id_pola;
        public int IdPola { get { return id_pola; } set { id_pola = value; } }

        private TypPola typPola = TypPola.Canvas;
        public TypPola TypPola { get { return typPola; } }
    }
}
