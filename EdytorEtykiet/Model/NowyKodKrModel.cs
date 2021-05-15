using BarcodeLib;
using EdytorEtykiet.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EdytorEtykiet.Model
{
    public class NowyKodKrModel : Image
    {
        private int id_pola;
        public int IdPola { get { return id_pola; } set { id_pola = value; } }

        private TypPola typPola = TypPola.Barcode;
        public TypPola TypPola { get { return typPola; } }

        private string _Tekst;
        public string Tekst { get { return _Tekst; } set { _Tekst = value; } }

        private TYPE _Typ;
        public TYPE Typ { get { return _Typ; } set { _Typ = value; } }

        private bool _CzyPokazacTekst = true;
        public bool CzyPokazacTekst { get { return _CzyPokazacTekst; } set { _CzyPokazacTekst = value; } }

        public NowyKodKrModel() : base()
        {

        }
    }
}
