using EdytorEtykiet.Helpers;
using EdytorEtykiet.Interfaces;
using System.Windows.Controls;
using System.Windows.Media;

namespace EdytorEtykiet.Model
{
    public class NowyObrazModel : INowyElement, INowyObraz
    {
        #region FIELDS & PROPERTIES

        #region INowyElement

        private int idPola;
        public int IdPola { get { return idPola; } set { idPola = value; } }

        private TypyPol typPola = TypyPol.Pic;
        public TypyPol TypPola { get { return typPola; } }

        private string nazwa;
        public string Nazwa { get { return nazwa; } set { nazwa = value; } }

        #endregion
        #region INowyObraz

        private string pelnaSciezka;
        public string PelnaSciezka { get { return pelnaSciezka; } set { pelnaSciezka = value; } }

        private int katObrotu;
        public int KatObrotu { get { return katObrotu; } set { katObrotu = value; } }

        private double proporcja;
        public double Proporcja { get { return proporcja; } set { proporcja = value; } }

        private ImageSource source;
        public ImageSource Source { get { return source; } set { source = value; } }

        private Image obraz;
        public Image Obraz { get { return obraz; } set { obraz = value; } }

        private double width;
        public double Width { get { return width; } set { width = value; } }

        private double height;
        public double Height { get { return height; } set { height = value; } }


        #endregion

        #endregion

        public NowyObrazModel()
        {

        }

        private void Obroc()
        {
            RotateTransform rotateTransform = new RotateTransform(KatObrotu);

            //ImgPodglad.RenderTransform = rotateTransform;
            Obraz.LayoutTransform = rotateTransform;
        }
    }
}
