using EdytorEtykiet.Helpers;
using EdytorEtykiet.Interfaces;
using EdytorEtykiet.Model;
using EdytorEtykiet.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EdytorEtykiet
{
    public delegate void DodajNowyElementDelegat(FrameworkElement newElement, double left, double top, bool edit = false);
    public delegate void EdytujElementDelegat(FrameworkElement edytowanyElement);

    public delegate void DodajNowyElementDelegat2(INowyElement newElement, double left, double top, bool edit = false);
    public delegate void EdytujElementDelegat2(INowyElement edytowanyElement);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    // ========================= MAIN ======================================

    public partial class MainWindow : Window
    {

        object przesuwanyElement;
        double SliderWartoscPowiekszenia = 0.2;
        public static List<FrameworkElement> ListaElementow = new List<FrameworkElement>();
        public static List<INowyElement> ListaElementow2 = new List<INowyElement>();

        #region MAIN
        public MainWindow()
        {
            InitializeComponent();
            WindowNowyTekst.NowyTekstEvent += new DodajNowyElementDelegat2(DodajElementDoEtykiety2);
            WindowNowyTekst.EdytujEvent += new EdytujElementDelegat2(EdytujElement2);

            WindowNowyKodKr.NowyKodKrEvent += new DodajNowyElementDelegat2(DodajElementDoEtykiety2);
            WindowNowyKodKr.EdytujEvent += new EdytujElementDelegat2(EdytujElement2);

            WindowNowyObraz.NowyObrazEvent += new DodajNowyElementDelegat2(DodajElementDoEtykiety2);
            WindowNowyObraz.EdytujEvent += new EdytujElementDelegat2(EdytujElement2);

            PreviewMouseMove += this.MouseMove;
            PreviewMouseLeftButtonUp += this.OnPreviewMouseLeftButtonUp;
        }
        #endregion

        #region COMMANDS

        private void Command_OtworzPlik(object sender, ExecutedRoutedEventArgs e)
        {
            Globals.OtworzPlik();
        }

        private void Command_ZapiszPLik(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                SliderPowiekszenie.Value = 1;

                //Globals.ZapiszPlik(EtykietaCanvas);
                Globals.ZapiszPlik(ListaElementow2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Zapis etykiety", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Command_NowaEtykieta(object sender, ExecutedRoutedEventArgs e)
        {
            WindowNowaEtykieta wne = new WindowNowaEtykieta();

            wne.ShowDialog();

            if (wne.WindowResult)
            {
                MainVM.WysPx = wne.NowaEtykietaVM.WysPx;
                MainVM.SzerPx = wne.NowaEtykietaVM.SzerPx;
                MainVM.NazwaEtykiety = wne.NowaEtykietaVM.NazwaEtykiety;
            }
        }

        private void Command_NowyTekst(object sender, ExecutedRoutedEventArgs e)
        {
            // potrzebuję listę elementów NowyTekst, żeby w nich wyszukać max IdPola
            var listaNowyTekst = ListaElementow2.Where(r => r is NowyTekstModel).ToList();
            var _IdPola = 1;
            if (listaNowyTekst.Count > 0)
            {
                var _maxIdPola = listaNowyTekst.Cast<NowyTekstModel>().Max(r => r.IdPola);
                _IdPola += _maxIdPola;
            }
            else
            {
                _IdPola = 1;
            }

            WindowNowyTekst wnt = new WindowNowyTekst(null, _IdPola);

            wnt.ShowDialog();
        }

        private void Command_NowyKodKr(object sender, ExecutedRoutedEventArgs e)
        {
            var listaNowyKodKr = ListaElementow.Where(r => r is NowyKodKrModel).ToList();
            var _IdPola = 1;
            if (listaNowyKodKr.Count > 0)
            {
                var _maxIdPola = listaNowyKodKr.Cast<NowyKodKrModel>().Max(r => r.IdPola);
                _IdPola += _maxIdPola;
            }
            else
            {
                _IdPola = 1;
            }

            WindowNowyKodKr wnk = new WindowNowyKodKr(null, _IdPola);

            wnk.ShowDialog();
        }

        private void Command_NowyObraz(object sender, ExecutedRoutedEventArgs e)
        {
            var listaNowyObraz = ListaElementow2.Where(r => r is NowyObrazModel).ToList();
            var _IdPola = 1;
            if (listaNowyObraz.Count > 0)
            {
                var _maxIdPola = listaNowyObraz.Cast<NowyObrazModel>().Max(r => r.IdPola);
                _IdPola += _maxIdPola;
            }
            else
            {
                _IdPola = 1;
            }

            WindowNowyObraz wno = new WindowNowyObraz(null, _IdPola);

            wno.ShowDialog();
        }

        private void Command_Zamknij(object sender, ExecutedRoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Command_EdytujElement(object sender, ExecutedRoutedEventArgs e)
        {
            var zaznaczonyElement = ZnajdzElementPoNazwie(MainVM.NazwaZaznaczonegoElementu);
            var elementDoEdycji = ListaElementow2.Where(r => r.Nazwa == zaznaczonyElement.Name).FirstOrDefault();
            switch (elementDoEdycji.TypPola)
            {
                case TypyPol.Barcode:
                    new WindowNowyKodKr().ShowDialog();
                    break;
                case TypyPol.Txt:
                    var elTxt = (elementDoEdycji as NowyTekstModel);
                    new WindowNowyTekst(elTxt).ShowDialog();
                    break;
                case TypyPol.Pic:
                    var elPic = (elementDoEdycji as NowyObrazModel);
                    new WindowNowyObraz(elPic).ShowDialog();
                    break;
                default:
                    break;
            }
        }
        private void Command_CzyMoznaEdytowacElement(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MainVM.NazwaZaznaczonegoElementu))
            {
                e.CanExecute = true;
            }
        }

        private void Command_UsunElement(object sender, ExecutedRoutedEventArgs e)
        {
            var result = MessageBox.Show("Czy na pewno chcesz usunąć ten komponent?", "Potwierdzenie usunięcia", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var zaznaczonyElement = ZnajdzElementPoNazwie(MainVM.NazwaZaznaczonegoElementu);

                ListaElementow.Remove(zaznaczonyElement);
                EtykietaCanvas.Children.Remove(zaznaczonyElement);

                switch ((zaznaczonyElement as INowyElement).TypPola)
                {
                    case TypyPol.Barcode:
                        MainVM.ListaElementow[2].Subelementy.Remove(MainVM.ListaElementow[2].Subelementy.Where(r => r.NazwaElementu == (zaznaczonyElement as INowyElement).Nazwa).FirstOrDefault());
                        break;
                    case TypyPol.Txt:
                        MainVM.ListaElementow[0].Subelementy.Remove(MainVM.ListaElementow[0].Subelementy.Where(r => r.NazwaElementu == (zaznaczonyElement as INowyElement).Nazwa).FirstOrDefault());
                        break;
                    case TypyPol.Pic:
                        MainVM.ListaElementow[1].Subelementy.Remove(MainVM.ListaElementow[1].Subelementy.Where(r => r.NazwaElementu == (zaznaczonyElement as INowyElement).Nazwa).FirstOrDefault());
                        break;
                    default:
                        break;
                }


                MainVM.NazwaZaznaczonegoElementu = default;
            }
        }
        private void Command_CzyMoznaUsunacElement(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MainVM.NazwaZaznaczonegoElementu))
            {
                e.CanExecute = true;
            }
        }

        private void Command_Drukuj(object sender, ExecutedRoutedEventArgs e)
        {

        }
        private void Command_CzyMoznaDrukowac(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion

        #region SKALOWANIE PODGLĄDU ETYKIETY

        private void PowiekszKolkiem(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Delta > 0)
                {
                    SliderPowiekszenie.Value += SliderWartoscPowiekszenia;
                }
                else
                {
                    SliderPowiekszenie.Value -= SliderWartoscPowiekszenia;
                }
            }
        }

        private void SliderPowiekszenie_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var newVal = e.NewValue;

            EtykietaCanvasZmianaSkali.ScaleX = newVal;
            EtykietaCanvasZmianaSkali.ScaleY = newVal;
        }

        private void Button_UstawPowiekszenie(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                switch (btn.Tag)
                {
                    case "Plus":
                        SliderPowiekszenie.Value += SliderWartoscPowiekszenia;
                        break;
                    case "Minus":
                        SliderPowiekszenie.Value -= SliderWartoscPowiekszenia;
                        break;
                    case "Default":
                        SliderPowiekszenie.Value = 1;
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion 

        #region DODAJ / POPRAW ELEMENT

        private void DodajElementDoEtykiety(FrameworkElement _nowe_pole, double left = 5, double top = 5, bool edycja = false)
        {
            if (edycja == false)
            {
                if (_nowe_pole is NowyKodKrModel)
                {
                    MainVM.ListaElementow[5].Subelementy.Add(new ElementEtykiety { NazwaElementu = _nowe_pole.Name });
                }
            }

            ListaElementow.Add(_nowe_pole);
            EtykietaCanvas.Children.Add(_nowe_pole);

            Canvas.SetLeft(_nowe_pole, left);
            Canvas.SetTop(_nowe_pole, top);

            _nowe_pole.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
            _nowe_pole.PreviewMouseLeftButtonUp += this.OnPreviewMouseLeftButtonUp;
            _nowe_pole.Cursor = Cursors.SizeAll;
        }

        private void DodajElementDoEtykiety2(INowyElement _nowe_pole, double left = 5, double top = 5, bool edycja = false)
        {
            FrameworkElement nowyElement;
            switch (_nowe_pole.TypPola)
            {
                case TypyPol.Canvas:
                    break;
                case TypyPol.Txt:
                    if (!edycja) MainVM.ListaElementow[1].Subelementy.Add(new ElementEtykiety { NazwaElementu = _nowe_pole.Nazwa });
                    nowyElement = new Label();
                    (nowyElement as Label).Name = (_nowe_pole as NowyTekstModel).Nazwa;
                    (nowyElement as Label).Content = (_nowe_pole as NowyTekstModel).Tekst;
                    (nowyElement as Label).BorderThickness = (_nowe_pole as NowyTekstModel).GruboscRamki;
                    (nowyElement as Label).BorderBrush = (_nowe_pole as NowyTekstModel).KolorRamki;
                    (nowyElement as Label).FontFamily = (_nowe_pole as NowyTekstModel).FontFamily;
                    (nowyElement as Label).FontSize = (_nowe_pole as NowyTekstModel).FontSize;
                    (nowyElement as Label).FontWeight = (_nowe_pole as NowyTekstModel).FontWeight;
                    (nowyElement as Label).FontStyle = (_nowe_pole as NowyTekstModel).FontStyle;
                    (nowyElement as Label).Height = (_nowe_pole as NowyTekstModel).Wysokosc;
                    (nowyElement as Label).Width = (_nowe_pole as NowyTekstModel).Szerokosc;
                    (nowyElement as Label).HorizontalContentAlignment = (_nowe_pole as NowyTekstModel).WyrownanieWPoziomie;
                    (nowyElement as Label).VerticalContentAlignment = (_nowe_pole as NowyTekstModel).WyrownanieWPionie;
                    (nowyElement as Label).Padding = new Thickness(0);
                    (nowyElement as Label).Background = new SolidColorBrush(Colors.White);
                    (nowyElement as Label).Foreground = new SolidColorBrush(Colors.Black);

                    ListaElementow2.Add(_nowe_pole);
                    EtykietaCanvas.Children.Add(nowyElement);
                    Canvas.SetLeft(nowyElement, left);
                    Canvas.SetTop(nowyElement, top);

                    nowyElement.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
                    nowyElement.PreviewMouseLeftButtonUp += this.OnPreviewMouseLeftButtonUp;
                    nowyElement.Cursor = Cursors.SizeAll;
                    break;
                case TypyPol.TxtDb:
                    break;
                case TypyPol.Pic:
                    if (!edycja) MainVM.ListaElementow[3].Subelementy.Add(new ElementEtykiety { NazwaElementu = _nowe_pole.Nazwa });
                    nowyElement = new Image();
                    (nowyElement as Image).Name = (_nowe_pole as NowyObrazModel).Nazwa;
                    (nowyElement as Image).Source = new ImageSourceConverter().ConvertFromString((_nowe_pole as NowyObrazModel).PelnaSciezka) as ImageSource;
                    RotateTransform rotateTransform = new RotateTransform((_nowe_pole as NowyObrazModel).KatObrotu);
                    (nowyElement as Image).LayoutTransform = rotateTransform;

                    ListaElementow2.Add(_nowe_pole);
                    EtykietaCanvas.Children.Add(nowyElement);
                    Canvas.SetLeft(nowyElement, left);
                    Canvas.SetTop(nowyElement, top);

                    nowyElement.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
                    nowyElement.PreviewMouseLeftButtonUp += this.OnPreviewMouseLeftButtonUp;
                    nowyElement.Cursor = Cursors.SizeAll;
                    break;
                case TypyPol.PicDb:
                    break;
                case TypyPol.Barcode:
                    if (!edycja) MainVM.ListaElementow[5].Subelementy.Add(new ElementEtykiety { NazwaElementu = _nowe_pole.Nazwa });
                    nowyElement = new Image();
                    (nowyElement as Image).Name = (_nowe_pole as NowyKodKrModel).Nazwa;

                    (nowyElement as Image).Source = BarcodeHandler.UtworzKod((_nowe_pole as NowyKodKrModel).Typ
                        , (_nowe_pole as NowyKodKrModel).Tekst
                        , (_nowe_pole as NowyKodKrModel).Szerokosc
                        , (_nowe_pole as NowyKodKrModel).Wysokosc
                        , (_nowe_pole as NowyKodKrModel).CzyPokazacTekst);

                    ListaElementow2.Add(_nowe_pole);
                    EtykietaCanvas.Children.Add(nowyElement);
                    Canvas.SetLeft(nowyElement, left);
                    Canvas.SetTop(nowyElement, top);

                    nowyElement.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
                    nowyElement.PreviewMouseLeftButtonUp += this.OnPreviewMouseLeftButtonUp;
                    nowyElement.Cursor = Cursors.SizeAll;
                    break;
                case TypyPol.BarcodeDb:
                    break;
                default:
                    break;
            }
        }

        private void EdytujElement(FrameworkElement _edytowanyElement)
        {
            var edytowanyElement = ZnajdzElementPoNazwie(_edytowanyElement.Name);

            if (edytowanyElement != null)
            {
                double tmpLeft = Canvas.GetLeft(edytowanyElement);
                double tmpTop = Canvas.GetTop(edytowanyElement);

                ListaElementow.Remove(edytowanyElement);
                EtykietaCanvas.Children.Remove(edytowanyElement);

                DodajElementDoEtykiety(_edytowanyElement, tmpLeft, tmpTop, true);

                //MainCanvas.Children.Add(control);

                //Canvas.SetTop(control, tmpTop);
                //Canvas.SetLeft(control, tmpLeft);
            }
        }

        private void EdytujElement2(INowyElement _edytowanyElement)
        {
            var edytowanyElement = ZnajdzElementPoNazwie(_edytowanyElement.Nazwa);

            if (edytowanyElement != null)
            {
                double tmpLeft = Canvas.GetLeft(edytowanyElement);
                double tmpTop = Canvas.GetTop(edytowanyElement);

                ListaElementow.Remove(edytowanyElement);

                var found = ListaElementow2.Find(r => r.Nazwa == edytowanyElement.Name);
                if (found != null) ListaElementow2.Remove(found);

                EtykietaCanvas.Children.Remove(edytowanyElement);

                DodajElementDoEtykiety2(_edytowanyElement, tmpLeft, tmpTop, true);
            }
        }

        #endregion

        #region ZAZNACZANIE I PORUSZANIE ELEMENTEM
        //Naciśnięcie elementu
        private new void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainVM.NazwaZaznaczonegoElementu = (sender as FrameworkElement).Name;
            PokazInfoOKliknietymElemencie(sender);

            //In this event, we get current mouse position on the control to use it in the MouseMove event.
            var aktualnaPozycja = e.GetPosition(sender as FrameworkElement);
            MainVM.PozStartX = aktualnaPozycja.X;
            MainVM.PozStartY = aktualnaPozycja.Y;

            przesuwanyElement = sender;
        }

        //puszczene elementu
        private void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            przesuwanyElement = null;
        }

        //poruszanie myszą
        private new void MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (przesuwanyElement is FrameworkElement fe)
                {
                    var modelElementu = ListaElementow2.Where(l => l.Nazwa == fe.Name).FirstOrDefault();
                    var typElementu = modelElementu.TypPola;
                    // jest problem z obróconym obrazem, bo on po obrocie nie zmienia rozmiarów tylko wygląd. 
                    // obejściem tego jest podmiana wysokości z szerokością dla obiektów klasy NowyObrazModel z NowyObrazModel.KatObrotu = 90 i 270
                    var feWys = fe.ActualHeight;
                    var feSzer = fe.ActualWidth;

                    if ((typElementu == TypyPol.Pic) && (((modelElementu as NowyObrazModel).KatObrotu == 90) || ((modelElementu as NowyObrazModel).KatObrotu == 270)))
                    {
                        feWys = fe.ActualWidth;
                        feSzer = fe.ActualHeight;
                    }


                    var pozycjaKursora = e.GetPosition(EtykietaCanvas);

                    MainVM.PozRuchX = pozycjaKursora.X - MainVM.PozStartX;
                    MainVM.PozRuchY = pozycjaKursora.Y - MainVM.PozStartY;

                    var marginesL = fe.Name.Contains("MarginesL");
                    var marginesP = fe.Name.Contains("MarginesP");
                    var marginesG = fe.Name.Contains("MarginesG");
                    var marginesD = fe.Name.Contains("MarginesD");

                    // jezeli przesuwam margines to ograniczeniem będzie canvas, a jeżeli przesuwam element etykiety to ograniczeniem będzie margines
                    var ograniczenieL = marginesL ? 0 : (double)MarginesL.GetValue(Canvas.LeftProperty);
                    var ograniczenieP = marginesP ? EtykietaCanvas.ActualWidth : (double)MarginesP.GetValue(Canvas.LeftProperty);

                    var ograniczenieG = marginesG ? 0 : (double)MarginesG.GetValue(Canvas.TopProperty);
                    var ograniczenieD = marginesD ? EtykietaCanvas.ActualHeight : (double)MarginesD.GetValue(Canvas.TopProperty);

                    MainVM.OgraniczenieL = ograniczenieL;
                    MainVM.OgraniczenieP = ograniczenieP;
                    MainVM.OgraniczenieG = ograniczenieG;
                    MainVM.OgraniczenieD = ograniczenieD;

                    if ((MainVM.PozRuchX >= ograniczenieL))
                    {
                        if (marginesG || marginesD) ;
                        else
                        {
                            if (MainVM.PozStartX + MainVM.PozRuchX + (feSzer - MainVM.PozStartX) > ograniczenieP)
                                fe.SetValue(Canvas.LeftProperty, ograniczenieP - feSzer);
                            else
                                fe.SetValue(Canvas.LeftProperty, MainVM.PozRuchX);
                        }
                    }

                    if ((MainVM.PozRuchY >= ograniczenieG))
                    {
                        if (marginesL || marginesP) ;
                        else
                        {
                            if (MainVM.PozStartY + MainVM.PozRuchY + (feWys - MainVM.PozStartY) > ograniczenieD)
                                fe.SetValue(Canvas.TopProperty, ograniczenieD - feWys);
                            else
                                fe.SetValue(Canvas.TopProperty, MainVM.PozRuchY);
                        }
                    }

                }
            }

        }

        private void PokazInfoOKliknietymElemencie(object sender)
        {
            if (sender is FrameworkElement fe)
            {
                if (fe.DataContext is NowyTekstViewModel)
                {
                    //MainVM.CurrentComponentName = tvm.Name;
                    PokazElementNaDrzewie(fe.Name, 0);
                }

                if (fe.DataContext is NowyKodKrViewModel)
                {
                    //MainVM.CurrentComponentName = tvm.Name;
                    PokazElementNaDrzewie(fe.Name, 3);
                }

                if (fe.DataContext is NowyObrazViewModel)
                {
                    //MainVM.CurrentComponentName = tvm.Name;
                    PokazElementNaDrzewie(fe.Name, 2);
                }
                //if (fe is Image ivm)
                //{
                //    MainVM.CurrentComponentName = ivm.Name;
                //    SelectControlOnTree(fe.Name, 1);
                //}

                //if (fe is BarcodeControl bcc)
                //{
                //    MainVM.CurrentComponentName = bcc.Name;
                //    SelectControlOnTree(fe.Name, 2);
                //}

                //if (fe is DbTextModel)
                //{
                //    MainVM.CurrentComponentName = fe.Name;
                //    SelectControlOnTree(fe.Name, 3);
                //}
            }
        }

        private void PokazElementNaDrzewie(string name, int type)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            var toSelect = MainVM.ListaElementow[type].Subelementy.Where(r => r.NazwaElementu == name).FirstOrDefault();

            if (toSelect != null)
            {
                toSelect.JestWybrany = true;
            }
        }

        private FrameworkElement ZnajdzElementPoNazwie(string name)
        {
            //return MainCanvas.Children.Cast<FrameworkElement>().Where(c => c.DataContext is NewTextViewModel).ToList().Where(cc => ((NewTextViewModel)cc.DataContext).Name == name).FirstOrDefault();

            return EtykietaCanvas.Children.Cast<FrameworkElement>().Where(c => c.Name == name).FirstOrDefault();
        }





        #endregion


    }
}
