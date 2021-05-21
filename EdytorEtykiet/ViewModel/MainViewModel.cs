using EdytorEtykiet.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace EdytorEtykiet.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region  PROPERTY CHANGE

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
        #region FIELDS & PROPERTIES

        private const int DodatkowaDlugoscMarginesu = 10; // marginesy wychodzą poza Canvas na rogach
        private const int DlugoscWysunieciaMarginesu = 5;// długość wysunięcia marginesu po za canvas
        
        public double DlugoscMarginesuPoziomego { get { return SzerPx + DodatkowaDlugoscMarginesu; } }
        public double DlugoscMarginesuPionowego { get { return WysPx + DodatkowaDlugoscMarginesu; } }


        private Canvas etykietaTlo;
        public Canvas EtykietaTlo { get { return etykietaTlo; } set { etykietaTlo = value; } }

        private double poczatekMarginesu = DlugoscWysunieciaMarginesu * -1;
        public double PoczatekMarginesu { get { return poczatekMarginesu; } set { poczatekMarginesu = value; OnPropertyChanged("PoczatekMarginesu"); } }

        private double domyslnaPozycjaMarginesuG;
        public double DomyslnaPozycjaMarginesuG { get { return domyslnaPozycjaMarginesuG; } set { domyslnaPozycjaMarginesuG = value; OnPropertyChanged("DomyslnaPozycjaMarginesuG"); } }

        private double domyslnaPozycjaMarginesuD;
        public double DomyslnaPozycjaMarginesuD { get { return domyslnaPozycjaMarginesuD; } set { domyslnaPozycjaMarginesuD = value; OnPropertyChanged("DomyslnaPozycjaMarginesuD"); } }
        
        private double domyslnaPozycjaMarginesuP;
        public double DomyslnaPozycjaMarginesuP { get { return domyslnaPozycjaMarginesuP; } set { domyslnaPozycjaMarginesuP = value; OnPropertyChanged("DomyslnaPozycjaMarginesuP"); } }
        
        private double domyslnaPozycjaMarginesuL;
        public double DomyslnaPozycjaMarginesuL { get { return domyslnaPozycjaMarginesuL; } set { domyslnaPozycjaMarginesuL = value; OnPropertyChanged("DomyslnaPozycjaMarginesuL"); } }

        private ObservableCollection<ElementEtykiety> drzewoElementow;
        public ObservableCollection<ElementEtykiety> DrzewoElementow { get { return drzewoElementow; } set { drzewoElementow = value; OnPropertyChanged("DrzewoElementow"); } }

        private string nazwaZaznaczonegoElementu;
        public string NazwaZaznaczonegoElementu { get { return nazwaZaznaczonegoElementu; } set { nazwaZaznaczonegoElementu = value; OnPropertyChanged("NazwaZaznaczonegoElementu"); } }

        private Visibility widocznoscMarginesow = Visibility.Visible;
        public Visibility WidocznoscMarginesow { get { return widocznoscMarginesow; } set { widocznoscMarginesow = value; OnPropertyChanged("WidocznoscMarginesow"); } }

        private double szerPx;
        public double SzerPx { get { return szerPx; } set { szerPx = Math.Round(value); OnPropertyChanged("SzerPx"); OnPropertyChanged("DlugoscMarginesuPoziomego"); DomyslnaPozycjaMarginesuL = 0; DomyslnaPozycjaMarginesuP = szerPx; } }
        
        private double wysPx;
        public double WysPx {get { return wysPx; } set { wysPx = Math.Round(value); OnPropertyChanged("WysPx"); OnPropertyChanged("DlugoscMarginesuPionowego"); DomyslnaPozycjaMarginesuG = 0; DomyslnaPozycjaMarginesuD = wysPx; } }

        private double pozStartX;
        public double PozStartX { get { return pozStartX; } set { pozStartX = value; OnPropertyChanged("PozStartX"); } }
        
        private double pozStartY;
        public double PozStartY { get { return pozStartY; } set { pozStartY = value; OnPropertyChanged("PozStartY"); } }
        
        private double pozRuchX;
        public double PozRuchX { get { return pozRuchX; } set { pozRuchX = value; OnPropertyChanged("PozRuchX"); } }
        
        private double pozRuchY;
        public double PozRuchY { get { return pozRuchY; } set { pozRuchY = value; OnPropertyChanged("PozRuchY"); } }
        
        private double ograniczenieL;
        public double OgraniczenieL { get { return ograniczenieL; } set { ograniczenieL = value; OnPropertyChanged("OgraniczenieL"); } }
        
        private double ograniczenieP;
        public double OgraniczenieP { get { return ograniczenieP; } set { ograniczenieP = value; OnPropertyChanged("OgraniczenieP"); } }
        
        private double ograniczenieG;
        public double OgraniczenieG { get { return ograniczenieG; } set { ograniczenieG = value; OnPropertyChanged("OgraniczenieG"); } }
        
        private double ograniczenieD;
        public double OgraniczenieD { get { return ograniczenieD; } set { ograniczenieD = value; OnPropertyChanged("OgraniczenieD"); } }

        private string nazwaEtykiety;
        public string NazwaEtykiety { get { return nazwaEtykiety; } set { nazwaEtykiety = value; OnPropertyChanged("NazwaEtykiety"); } }

        #endregion
        #region MAIN

        public MainViewModel()
        {
            UtworzDrzewoElementow();
        }

        #endregion
        #region DRZEWO ELEMENTÓW

        public void UtworzDrzewoElementow()
        {
            DrzewoElementow = null;
            DrzewoElementow = new ObservableCollection<ElementEtykiety>();
            DrzewoElementow.Add(new ElementEtykiety { NazwaElementu = "Etykieta", TypPola = TypyPol.Canvas });
            DrzewoElementow.Add(new ElementEtykiety { NazwaElementu = "Tekst", TypPola = TypyPol.Txt });
            DrzewoElementow.Add(new ElementEtykiety { NazwaElementu = "Tekst z bazy danych", TypPola = TypyPol.TxtDb });
            DrzewoElementow.Add(new ElementEtykiety { NazwaElementu = "Obraz", TypPola = TypyPol.Pic });
            DrzewoElementow.Add(new ElementEtykiety { NazwaElementu = "Obraz z bazy danych", TypPola = TypyPol.PicDb });
            DrzewoElementow.Add(new ElementEtykiety { NazwaElementu = "Kod kreskowy", TypPola = TypyPol.Barcode });
            DrzewoElementow.Add(new ElementEtykiety { NazwaElementu = "Kod kreskowy z bazy danych", TypPola = TypyPol.BarcodeDb });


            DrzewoElementow[0].Subelementy = new ObservableCollection<ElementEtykiety>();
            DrzewoElementow[1].Subelementy = new ObservableCollection<ElementEtykiety>();
            DrzewoElementow[2].Subelementy = new ObservableCollection<ElementEtykiety>();
            DrzewoElementow[3].Subelementy = new ObservableCollection<ElementEtykiety>();
            DrzewoElementow[4].Subelementy = new ObservableCollection<ElementEtykiety>();
            DrzewoElementow[5].Subelementy = new ObservableCollection<ElementEtykiety>();
            DrzewoElementow[6].Subelementy = new ObservableCollection<ElementEtykiety>();
        }

        #endregion
    }
    public class ElementEtykiety : INotifyPropertyChanged
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
        public string NazwaElementu { get; set; }
        public ObservableCollection<ElementEtykiety> Subelementy { get; set; }
        public TypyPol TypPola { get; set; }
        private bool jestWybrany;

        public bool JestWybrany { get { return jestWybrany; } set { jestWybrany = value; OnPropertyChanged("JestWybrany"); } }
        #endregion
        // =====================================================================
    }
}
