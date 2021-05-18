using BarcodeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;




namespace EdytorEtykiet.Helpers
{
    public class BarcodeHandler
    {
        public static List<TYPE> PobierzListeTypow()
        {
            List<TYPE> listaTypow = Enum.GetValues(typeof(TYPE)).Cast<TYPE>().ToList();
            return listaTypow;
        }

        public static BitmapImage UtworzKod(TYPE _format_kodu, string _tekst, double _szer, double _wys, bool _czy_pokazac_tekst)
        {
            Barcode b = new Barcode();
            b.IncludeLabel = _czy_pokazac_tekst;
            
            return Globals.ImageToImageSource(b.Encode(_format_kodu, _tekst, (int)_szer, (int)_wys));
        }


    }
}
