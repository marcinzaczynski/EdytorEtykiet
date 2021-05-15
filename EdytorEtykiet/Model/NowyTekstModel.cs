using EdytorEtykiet.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EdytorEtykiet.Model
{
    public class NowyTekstModel : Label
    {
        private int id_pola;
        public int IdPola { get { return id_pola; } set { id_pola = value; } }
        
        private TypPola typPola = TypPola.Txt;
        public TypPola TypPola { get { return typPola; } }

        public NowyTekstModel() : base()
        {

        }
    }
}
