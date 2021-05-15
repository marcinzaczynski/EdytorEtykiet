using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;

namespace EdytorEtykiet.Helpers
{
    public class PrinterHandler
    {
        public static PrinterSettings PobierzUstawieniaDrukarki(string nazwaDrukarki)
        {
            var settings = new PrinterSettings
            {
                PrinterName = nazwaDrukarki
            };

            return settings;
        }
    }
}
