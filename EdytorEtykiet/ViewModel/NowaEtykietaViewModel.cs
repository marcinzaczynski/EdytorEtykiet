using EdytorEtykiet.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;


namespace EdytorEtykiet.ViewModel
{
    public class NowaEtykietaViewModel : INotifyPropertyChanged
    {
        // ========================= PROPERTY CHANGE ===========================
        #region region PROPERTY CHANGE
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
        // ========================= PROPERTIES ================================
        #region region PROPERTIES

        private const double inConst = 25.4;            // 1 in = 25.4 mm

        private List<string> zainstalowaneDrukarki;
        private string wybranaDrukarka;
        private PaperSize wybranyPapier;
        private PrinterResolution wybranaRozdzielczosc;
        private int xDPI;
        private int yDPI;
        private PrinterSettings.PaperSizeCollection rozmiaryPapieru;
        //private PrinterSettings.PrinterResolutionCollection rozdzielczosciDrukarki;
        private List<PrinterResolution> rozdzielczosciDrukarki;

        private string nazwaEtykiety = "Etykieta 1";
        private double szerIn;
        private double wysIn;
        private double szerPx;
        private double wysPx;
        private double szerMm;
        private double wysMm;



        public string NazwaEtykiety
        {
            get { return nazwaEtykiety; }
            set
            {
                nazwaEtykiety = value;
                OnPropertyChanged("NazwaEtykiety");
            }
        }

        public List<string> ZainstalowaneDrukarki
        {
            get { return zainstalowaneDrukarki; }
            set
            {
                zainstalowaneDrukarki = value;
                OnPropertyChanged("ZainstalowaneDrukarki");
            }
        }

        public string WybranaDrukarka
        {
            get { return wybranaDrukarka; }
            set
            {
                wybranaDrukarka = value;
                OnPropertyChanged("WybranaDrukarka");
                WczytajUstawieniaDrukarki();
            }
        }

        public PrinterSettings.PaperSizeCollection RozmiaryPapieru
        {
            get { return rozmiaryPapieru; }
            set
            {
                rozmiaryPapieru = value;
                OnPropertyChanged("RozmiaryPapieru");
            }
        }

        public List<PrinterResolution> RozdzielczosciDrukarki
        {
            get { return rozdzielczosciDrukarki; }
            set
            {
                rozdzielczosciDrukarki = value;
                OnPropertyChanged("RozdzielczosciDrukarki");
            }
        }

        public int XDPI
        {
            get { return xDPI; }
            set
            {
                xDPI = value;
                OnPropertyChanged("XDPI");
                SzerPx = SzerIn * xDPI;
            }
        }

        public int YDPI
        {
            get { return yDPI; }
            set
            {
                yDPI = value;
                OnPropertyChanged("YDPI");
                WysPx = WysIn * yDPI;
            }
        }

        public PrinterResolution WybranaRozdzielczosc
        {
            get { return wybranaRozdzielczosc; }
            set
            {
                wybranaRozdzielczosc = value;
                OnPropertyChanged("WybranaRozdzielczosc");
                UstawDPI(value);
            }
        }
        public PaperSize WybranyPapier
        {
            get { return wybranyPapier; }
            set
            {
                wybranyPapier = value;
                OnPropertyChanged("RozmiaryPapieru");
                UstawParametryEtykiety(value);
            }
        }

        public double SzerIn
        {
            get { return szerIn; }
            set
            {
                szerIn = Math.Round(value, 2);
                OnPropertyChanged("SzerIn");
                SzerMm = inToMm(szerIn);
                SzerPx = szerIn * XDPI;

            }
        }

        public double WysIn
        {
            get { return wysIn; }
            set
            {
                wysIn = Math.Round(value, 2);
                OnPropertyChanged("WysIn");
                WysMm = inToMm(wysIn);
                WysPx = wysIn * YDPI;
            }
        }

        public double SzerMm
        {
            get { return szerMm; }
            set
            {
                szerMm = Math.Round(value, 2);
                OnPropertyChanged("SzerMm");
            }
        }

        public double WysMm
        {
            get { return wysMm; }
            set
            {
                wysMm = Math.Round(value, 2);
                OnPropertyChanged("WysMm");
            }
        }

        public double SzerPx
        {
            get { return szerPx; }
            set
            {
                szerPx = Math.Round(value, 2);
                OnPropertyChanged("SzerPx");
            }
        }

        public double WysPx
        {
            get { return wysPx; }
            set
            {
                wysPx = Math.Round(value, 2);
                OnPropertyChanged("WysPx");
            }
        }

        #endregion
        public void UstawParametryEtykiety(PaperSize rozmiarPapieru)
        {
            if (rozmiarPapieru != null)
            {
                NazwaEtykiety = rozmiarPapieru.PaperName;
                SzerIn = rozmiarPapieru.Width / 100.0;
                WysIn = rozmiarPapieru.Height / 100.0;
            }
        }
        private void WczytajUstawieniaDrukarki()
        {
            var printerSettings = PrinterHandler.PobierzUstawieniaDrukarki(WybranaDrukarka);

            RozmiaryPapieru = printerSettings.PaperSizes;
            //RozdzielczosciDrukarki = printerSettings.PrinterResolutions;
            RozdzielczosciDrukarki = (from w in printerSettings.PrinterResolutions.Cast<PrinterResolution>() where w.X > 0 select w).ToList();
            //DPI = printerSettings.PrinterResolutions.Cast<PrinterResolution>().OrderByDescending(pr => pr.X).First().X;

        }

        private void UstawDPI(PrinterResolution rozdzielczoscDrukarki)
        {
            if (rozdzielczoscDrukarki != null)
            {
                XDPI = rozdzielczoscDrukarki.X;
                YDPI = rozdzielczoscDrukarki.Y;
            }
        }

        private double inToMm(double _in)
        {
            return _in * inConst;
        }
    }
}
