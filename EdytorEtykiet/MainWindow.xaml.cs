using SimpleLabelLibrary;
using SimpleLabelLibrary.Interfaces;
using SimpleLabelLibrary.Helpers;
using SimpleLabelLibrary.Models;
using EdytorEtykiet.Helpers;
using EdytorEtykiet.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace EdytorEtykiet
{

    public delegate void DodajNowyElementDelegat(IDefaultField newElement, double left, double top, bool edit = false);
    public delegate void EdytujElementDelegat(IDefaultField edytowanyElement);
    public delegate bool FieldExistsDelegate(FieldTypes _fieldType, string _fieldName);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    // ========================= MAIN ======================================

    public partial class MainWindow : Window
    {

        object przesuwanyElement;
        double SliderWartoscPowiekszenia = 0.2;
        private SimpleLabel simpleLabel = new SimpleLabel();

        #region MAIN

        public MainWindow()
        {
            InitializeComponent();
            WindowNowyTekst.NowyTekstEvent += new DodajNowyElementDelegat(DodajElementDoEtykiety);
            WindowNowyTekst.EdytujEvent += new EdytujElementDelegat(EdytujElement);
            WindowNowyTekst.FieldExistsEvent += new FieldExistsDelegate(FieldExists);

            WindowNowyKodKr.NowyKodKrEvent += new DodajNowyElementDelegat(DodajElementDoEtykiety);
            WindowNowyKodKr.EdytujEvent += new EdytujElementDelegat(EdytujElement);
            WindowNowyKodKr.FieldExistsEvent += new FieldExistsDelegate(FieldExists);

            WindowNowyObraz.NowyObrazEvent += new DodajNowyElementDelegat(DodajElementDoEtykiety);
            WindowNowyObraz.EdytujEvent += new EdytujElementDelegat(EdytujElement);
            WindowNowyObraz.FieldExistsEvent += new FieldExistsDelegate(FieldExists);

            PreviewMouseMove += this.MouseMove;
            PreviewMouseLeftButtonUp += this.OnPreviewMouseLeftButtonUp;


            MainVM.SzerPx = simpleLabel.FieldsList.Cast<ICanvasField>().Where(fl => fl.FieldType == FieldTypes.Canvas).FirstOrDefault().Width;
            MainVM.WysPx = simpleLabel.FieldsList.Cast<ICanvasField>().Where(fl => fl.FieldType == FieldTypes.Canvas).FirstOrDefault().Height;
            MainVM.NazwaEtykiety = simpleLabel.FieldsList.Cast<ICanvasField>().Where(fl => fl.FieldType == FieldTypes.Canvas).FirstOrDefault().Name;
        }
        #endregion

        #region COMMANDS

        private void Command_OtworzPlik(object sender, ExecutedRoutedEventArgs e)
        {


            XElement element = Globals.OtworzPlik();

            if (element != null)
            { 
                IEnumerable<XElement> allChildElements = element.Elements();
                foreach (var item in allChildElements)
                {
                    if (item.Name == "Txt")
                    {
                        MessageBox.Show(String.Format("jest tekst i ma {0} nodów", item.Nodes().Count()));
                    }
                    if (item.Name == "Pic")
                    {
                        MessageBox.Show(String.Format("jest obraz i ma {0} nodów", item.Nodes().Count()));
                    }
                    if (item.Name == "Barcode")
                    {
                        MessageBox.Show(String.Format("jest kodkr i ma {0} nodów", item.Nodes().Count()));
                    }
                }
            }
        }

        private void Command_ZapiszPLik(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                SliderPowiekszenie.Value = 1;

                //Globals.ZapiszPlik(EtykietaCanvas);
                Globals.ZapiszPlik(simpleLabel.FieldsList);
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

                var el = EtykietaCanvas.Children.Cast<FrameworkElement>().Where(c => !c.Name.Contains("Margines")).ToList();
                
                foreach (FrameworkElement item in el)
                {
                    if (!item.Name.Contains("Margines"))
                    {
                        EtykietaCanvas.Children.Remove(item);
                    }
                }

                simpleLabel.FieldsList.Clear();
                MainVM.UtworzDrzewoElementow();


                MainVM.WysPx = wne.NowaEtykietaVM.WysPx;
                MainVM.SzerPx = wne.NowaEtykietaVM.SzerPx;
                MainVM.NazwaEtykiety = wne.NowaEtykietaVM.NazwaEtykiety;
                MainVM.NazwaZaznaczonegoElementu = String.Empty;


                simpleLabel.SetupCanvasField(wne.NowaEtykietaVM.NazwaEtykiety, Convert.ToInt32(wne.NowaEtykietaVM.SzerPx), Convert.ToInt32(wne.NowaEtykietaVM.WysPx));
                //NowaEtykieta.Wysokosc = Convert.ToInt32(wne.NowaEtykietaVM.WysPx); 
                //NowaEtykieta.Szerokosc = Convert.ToInt32(wne.NowaEtykietaVM.SzerPx);
                //NowaEtykieta.Nazwa = wne.NowaEtykietaVM.NazwaEtykiety;

                EtykietaCanvas.Width = Convert.ToInt32(wne.NowaEtykietaVM.SzerPx);
                EtykietaCanvas.Height = Convert.ToInt32(wne.NowaEtykietaVM.SzerPx);

                //simpleLabel.FieldsList.Add(NowaEtykieta);
            }
        }

        private void Command_NowyTekst(object sender, ExecutedRoutedEventArgs e)
        {
            // potrzebuję listę elementów NowyTekst, żeby w nich wyszukać max IdPola
            var listaNowyTekst = simpleLabel.FieldsList.Where(r => r is TextField).ToList();
            var _IdPola = 1;
            if (listaNowyTekst.Count > 0)
            {
                var _maxIdPola = listaNowyTekst.Cast<TextField>().Max(r => r.Id);
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
            var listaNowyKodKr = simpleLabel.FieldsList.Where(r => r is BarcodeField).ToList();
            var _IdPola = 1;
            if (listaNowyKodKr.Count > 0)
            {
                var _maxIdPola = listaNowyKodKr.Cast<BarcodeField>().Max(r => r.Id);
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
            var listaNowyObraz = simpleLabel.FieldsList.Where(r => r is PictureField).ToList();
            var _IdPola = 1;
            if (listaNowyObraz.Count > 0)
            {
                var _maxIdPola = listaNowyObraz.Cast<PictureField>().Max(r => r.Id);
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
            var elementDoEdycji = simpleLabel.FieldsList.Where(r => r.Name == zaznaczonyElement.Name).FirstOrDefault();
            switch (elementDoEdycji.FieldType)
            {
                case FieldTypes.Barcode:
                    var elBarcode = (elementDoEdycji as BarcodeField);
                    new WindowNowyKodKr(elBarcode).ShowDialog();
                    break;
                case FieldTypes.Text:
                    var elTxt = (elementDoEdycji as TextField);
                    new WindowNowyTekst(elTxt).ShowDialog();
                    break;
                case FieldTypes.Picture:
                    var elPic = (elementDoEdycji as PictureField);
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

                var found = simpleLabel.FieldsList.Find(r => r.Name == zaznaczonyElement.Name);
                if (found != null) simpleLabel.FieldsList.Remove(found);

                EtykietaCanvas.Children.Remove(zaznaczonyElement);

                switch ((zaznaczonyElement as IDefaultField).FieldType)
                {
                    case FieldTypes.Barcode:
                        MainVM.DrzewoElementow[2].Subelementy.Remove(MainVM.DrzewoElementow[2].Subelementy.Where(r => r.NazwaElementu == (zaznaczonyElement as IDefaultField).Name).FirstOrDefault());
                        break;
                    case FieldTypes.Text:
                        MainVM.DrzewoElementow[0].Subelementy.Remove(MainVM.DrzewoElementow[0].Subelementy.Where(r => r.NazwaElementu == (zaznaczonyElement as IDefaultField).Name).FirstOrDefault());
                        break;
                    case FieldTypes.Picture:
                        MainVM.DrzewoElementow[1].Subelementy.Remove(MainVM.DrzewoElementow[1].Subelementy.Where(r => r.NazwaElementu == (zaznaczonyElement as IDefaultField).Name).FirstOrDefault());
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

        private bool FieldExists(FieldTypes _fieldType, string _fieldName)
        {
            var a = simpleLabel.FieldsList.Where(c => c.Name == _fieldName).FirstOrDefault();
            return (a == null) ? false : true;
        }

        private void DodajElementDoEtykiety(IDefaultField _nowe_pole, double left = 5, double top = 5, bool edycja = false)
        {
            FrameworkElement nowyElement;
            switch (_nowe_pole.FieldType)
            {
                case FieldTypes.Canvas:
                    break;
                case FieldTypes.Text:
                    if (!edycja) MainVM.DrzewoElementow[1].Subelementy.Add(new ElementEtykiety { NazwaElementu = _nowe_pole.Name });
                    nowyElement = new Label();
                    (nowyElement as Label).Name = (_nowe_pole as TextField).Name;
                    (nowyElement as Label).Content = (_nowe_pole as TextField).Text;
                    (nowyElement as Label).BorderThickness = (_nowe_pole as TextField).BorderWidth;
                    (nowyElement as Label).BorderBrush = (_nowe_pole as TextField).BorderColor;
                    (nowyElement as Label).FontFamily = (_nowe_pole as TextField).FontFamily;
                    (nowyElement as Label).FontSize = (_nowe_pole as TextField).FontSize;
                    (nowyElement as Label).FontWeight = (_nowe_pole as TextField).FontWeight;
                    (nowyElement as Label).FontStyle = (_nowe_pole as TextField).FontStyle;
                    (nowyElement as Label).Height = (_nowe_pole as TextField).Height;
                    (nowyElement as Label).Width = (_nowe_pole as TextField).Width;
                    (nowyElement as Label).HorizontalContentAlignment = (_nowe_pole as TextField).HorizontalAlignement;
                    (nowyElement as Label).VerticalContentAlignment = (_nowe_pole as TextField).VerticalAlignement;
                    (nowyElement as Label).Padding = new Thickness(0);

                    simpleLabel.FieldsList.Add(_nowe_pole);
                    EtykietaCanvas.Children.Add(nowyElement);
                    Canvas.SetLeft(nowyElement, left);
                    Canvas.SetTop(nowyElement, top);

                    nowyElement.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
                    nowyElement.PreviewMouseLeftButtonUp += this.OnPreviewMouseLeftButtonUp;
                    nowyElement.Cursor = Cursors.SizeAll;
                    break;
                case FieldTypes.TextDb:
                    break;
                case FieldTypes.Picture:
                    if (!edycja) MainVM.DrzewoElementow[3].Subelementy.Add(new ElementEtykiety { NazwaElementu = _nowe_pole.Name });
                    nowyElement = new Image();
                    (nowyElement as Image).Name = (_nowe_pole as PictureField).Name;
                    (nowyElement as Image).Source = new ImageSourceConverter().ConvertFromString((_nowe_pole as PictureField).Path) as ImageSource;
                    RotateTransform rotateTransform = new RotateTransform((_nowe_pole as PictureField).RotationAngle);
                    (nowyElement as Image).LayoutTransform = rotateTransform;

                    simpleLabel.FieldsList.Add(_nowe_pole);
                    EtykietaCanvas.Children.Add(nowyElement);
                    Canvas.SetLeft(nowyElement, left);
                    Canvas.SetTop(nowyElement, top);

                    nowyElement.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
                    nowyElement.PreviewMouseLeftButtonUp += this.OnPreviewMouseLeftButtonUp;
                    nowyElement.Cursor = Cursors.SizeAll;
                    break;
                case FieldTypes.PictureDb:
                    break;
                case FieldTypes.Barcode:
                    if (!edycja) MainVM.DrzewoElementow[5].Subelementy.Add(new ElementEtykiety { NazwaElementu = _nowe_pole.Name });
                    nowyElement = new Image();
                    (nowyElement as Image).Name = (_nowe_pole as BarcodeField).Name;

                    (nowyElement as Image).Source = BarcodeHandler.UtworzKod((_nowe_pole as BarcodeField).BarcodeType
                        , (_nowe_pole as BarcodeField).TextToEncode
                        , (_nowe_pole as BarcodeField).Width
                        , (_nowe_pole as BarcodeField).Height
                        , (_nowe_pole as BarcodeField).ShowTextToEncode);

                    simpleLabel.FieldsList.Add(_nowe_pole);
                    EtykietaCanvas.Children.Add(nowyElement);
                    Canvas.SetLeft(nowyElement, left);
                    Canvas.SetTop(nowyElement, top);

                    nowyElement.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
                    nowyElement.PreviewMouseLeftButtonUp += this.OnPreviewMouseLeftButtonUp;
                    nowyElement.Cursor = Cursors.SizeAll;
                    break;
                case FieldTypes.BarcodeDb:
                    break;
                default:
                    break;
            }
        }

       
        private void EdytujElement(IDefaultField _edytowanyElement)
        {
            var edytowanyElement = ZnajdzElementPoNazwie(_edytowanyElement.Name);

            if (edytowanyElement != null)
            {
                double tmpLeft = Canvas.GetLeft(edytowanyElement);
                double tmpTop = Canvas.GetTop(edytowanyElement);

                //ListaElementow.Remove(edytowanyElement);

                var found = simpleLabel.FieldsList.Find(r => r.Name == edytowanyElement.Name);
                if (found != null) simpleLabel.FieldsList.Remove(found);

                EtykietaCanvas.Children.Remove(edytowanyElement);

                DodajElementDoEtykiety(_edytowanyElement, tmpLeft, tmpTop, true);
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
                    var modelElementu = simpleLabel.FieldsList.Where(l => l.Name == fe.Name).FirstOrDefault();
                    var typElementu = modelElementu.FieldType;
                    // jest problem z obróconym obrazem, bo on po obrocie nie zmienia rozmiarów tylko wygląd. 
                    // obejściem tego jest podmiana wysokości z szerokością dla obiektów klasy NowyObrazModel z NowyObrazModel.KatObrotu = 90 i 270
                    var feWys = fe.ActualHeight;
                    var feSzer = fe.ActualWidth;

                    if ((typElementu == FieldTypes.Picture) && (((modelElementu as PictureField).RotationAngle == 90) || ((modelElementu as PictureField).RotationAngle == 270)))
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

            var toSelect = MainVM.DrzewoElementow[type].Subelementy.Where(r => r.NazwaElementu == name).FirstOrDefault();

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
