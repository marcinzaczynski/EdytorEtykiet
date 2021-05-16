using EdytorEtykiet.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace EdytorEtykiet.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // ========================= PROPERTY CHANGE ===========================
        #region region PROPERTY CHANGE
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
        // =====================================================================
        // ========================= PROPERTIES ================================
        #region region PROPERTIES

        private const int DodatkowaDlugoscMarginesu = 10; // marginesy wychodzą poza Canvas na rogach
        private const int DlugoscWysunieciaMarginesu = 5;// długość wysunięcia marginesu po za canvas
        private double poczatekMarginesu = DlugoscWysunieciaMarginesu * -1;
        private double domyslnaPozycjaMarginesuG;
        private double domyslnaPozycjaMarginesuD;
        private double domyslnaPozycjaMarginesuP;
        private double domyslnaPozycjaMarginesuL;
        private ObservableCollection<ElementEtykiety> listaElementow;
        private string nazwaZaznaczonegoElementu;
        private Visibility widocznoscMarginesow = Visibility.Visible;
        private double szerPx;
        private double wysPx;
        private double pozStartX;
        private double pozStartY;
        private double pozRuchX;
        private double pozRuchY;
        private double ograniczenieL;
        private double ograniczenieP;
        private double ograniczenieG;
        private double ograniczenieD;
        private string nazwaEtykiety;
        public double DlugoscMarginesuPoziomego { get { return SzerPx + DodatkowaDlugoscMarginesu; } }
        public double DlugoscMarginesuPionowego { get { return WysPx + DodatkowaDlugoscMarginesu; } }
        public double PoczatekMarginesu { get { return poczatekMarginesu; } set { poczatekMarginesu = value; OnPropertyChanged("PoczatekMarginesu"); } }
        public double DomyslnaPozycjaMarginesuG { get { return domyslnaPozycjaMarginesuG; } set { domyslnaPozycjaMarginesuG = value; OnPropertyChanged("DomyslnaPozycjaMarginesuG"); } }
        public double DomyslnaPozycjaMarginesuD { get { return domyslnaPozycjaMarginesuD; } set { domyslnaPozycjaMarginesuD = value; OnPropertyChanged("DomyslnaPozycjaMarginesuD"); } }
        public double DomyslnaPozycjaMarginesuP { get { return domyslnaPozycjaMarginesuP; } set { domyslnaPozycjaMarginesuP = value; OnPropertyChanged("DomyslnaPozycjaMarginesuP"); } }
        public double DomyslnaPozycjaMarginesuL { get { return domyslnaPozycjaMarginesuL; } set { domyslnaPozycjaMarginesuL = value; OnPropertyChanged("DomyslnaPozycjaMarginesuL"); } }
        public ObservableCollection<ElementEtykiety> ListaElementow { get { return listaElementow; } set { listaElementow = value; OnPropertyChanged("ListaElementow"); } }
        public string NazwaZaznaczonegoElementu { get { return nazwaZaznaczonegoElementu; } set { nazwaZaznaczonegoElementu = value; OnPropertyChanged("NazwaZaznaczonegoElementu"); } }
        public Visibility WidocznoscMarginesow { get { return widocznoscMarginesow; } set { widocznoscMarginesow = value; OnPropertyChanged("WidocznoscMarginesow"); } }
        public double SzerPx
        {
            get { return szerPx; }
            set
            {
                szerPx = Math.Round(value);
                OnPropertyChanged("SzerPx");

                // odświeżenie długości i pozycje marginesów
                OnPropertyChanged("DlugoscMarginesuPoziomego");
                DomyslnaPozycjaMarginesuL = 0;
                DomyslnaPozycjaMarginesuP = szerPx;
            }
        }
        public double WysPx
        {
            get { return wysPx; }
            set
            {
                wysPx = Math.Round(value);
                OnPropertyChanged("WysPx");

                // odświeżenie długości i pozycje marginesów
                OnPropertyChanged("DlugoscMarginesuPionowego");
                DomyslnaPozycjaMarginesuG = 0;
                DomyslnaPozycjaMarginesuD = wysPx;
            }
        }
        public double PozStartX { get { return pozStartX; } set { pozStartX = value; OnPropertyChanged("PozStartX"); } }
        public double PozStartY { get { return pozStartY; } set { pozStartY = value; OnPropertyChanged("PozStartY"); } }
        public double PozRuchX { get { return pozRuchX; } set { pozRuchX = value; OnPropertyChanged("PozRuchX"); } }
        public double PozRuchY { get { return pozRuchY; } set { pozRuchY = value; OnPropertyChanged("PozRuchY"); } }
        public double OgraniczenieL { get { return ograniczenieL; } set { ograniczenieL = value; OnPropertyChanged("OgraniczenieL"); } }
        public double OgraniczenieP { get { return ograniczenieP; } set { ograniczenieP = value; OnPropertyChanged("OgraniczenieP"); } }
        public double OgraniczenieG { get { return ograniczenieG; } set { ograniczenieG = value; OnPropertyChanged("OgraniczenieG"); } }
        public double OgraniczenieD { get { return ograniczenieD; } set { ograniczenieD = value; OnPropertyChanged("OgraniczenieD"); } }
        public string NazwaEtykiety { get { return nazwaEtykiety; } set { nazwaEtykiety = value; OnPropertyChanged("NazwaEtykiety"); } }
        #endregion
        // ========================= MAIN ======================================
        #region region MAIN
        public MainViewModel()
        {
            UtworzDrzewoElementow();
        }
        #endregion
        // ========================= DRZEWO ELEMENETÓW =========================
        #region region DRZEWO ELEMENTÓW
        private void UtworzDrzewoElementow()
        {
            ListaElementow = new ObservableCollection<ElementEtykiety>();
            ListaElementow.Add(new ElementEtykiety { NazwaElementu = "Etykieta", TypPola = TypyPol.Canvas });
            ListaElementow.Add(new ElementEtykiety { NazwaElementu = "Tekst", TypPola = TypyPol.Txt });
            ListaElementow.Add(new ElementEtykiety { NazwaElementu = "Tekst z bazy danych", TypPola = TypyPol.TxtDb });
            ListaElementow.Add(new ElementEtykiety { NazwaElementu = "Obraz", TypPola = TypyPol.Pic });
            ListaElementow.Add(new ElementEtykiety { NazwaElementu = "Obraz z bazy danych", TypPola = TypyPol.PicDb });
            ListaElementow.Add(new ElementEtykiety { NazwaElementu = "Kod kreskowy", TypPola = TypyPol.Barcode });
            ListaElementow.Add(new ElementEtykiety { NazwaElementu = "Kod kreskowy z bazy danych", TypPola = TypyPol.BarcodeDb });


            ListaElementow[0].Subelementy = new ObservableCollection<ElementEtykiety>();
            ListaElementow[1].Subelementy = new ObservableCollection<ElementEtykiety>();
            ListaElementow[2].Subelementy = new ObservableCollection<ElementEtykiety>();
            ListaElementow[3].Subelementy = new ObservableCollection<ElementEtykiety>();
            ListaElementow[4].Subelementy = new ObservableCollection<ElementEtykiety>();
            ListaElementow[5].Subelementy = new ObservableCollection<ElementEtykiety>();
            ListaElementow[6].Subelementy = new ObservableCollection<ElementEtykiety>();
        }
        #endregion
        // =====================================================================
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
