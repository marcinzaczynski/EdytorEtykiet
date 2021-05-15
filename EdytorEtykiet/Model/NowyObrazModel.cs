using EdytorEtykiet.Helpers;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EdytorEtykiet.Model
{
    public class NowyObrazModel : Image
    {
        private int id_pola;
        public int IdPola { get { return id_pola; } set { id_pola = value; } }
        
        private TypPola typPola = TypPola.Pic;
        public TypPola TypPola { get { return typPola; } }

        private string _PelnaSciezka;
        public string PelnaSciezka { get { return _PelnaSciezka; } set { _PelnaSciezka = value; } }

        private string _NazwaPliku;
        public string NazwaPliku { get { return _NazwaPliku; } set { _NazwaPliku = value; } }

        private int _KatObrotu;
        public int KatObrotu { get { return _KatObrotu; } set { _KatObrotu = value; Obroc(); } }

        private double _Proporcja;
        public double Proporcja { get { return _Proporcja; } set { _Proporcja = value; } }

        public NowyObrazModel() : base()
        {

        }

        private void Obroc()
        {
            RotateTransform rotateTransform = new RotateTransform(_KatObrotu);

            //ImgPodglad.RenderTransform = rotateTransform;
            this.LayoutTransform = rotateTransform;
        }
    }
}
