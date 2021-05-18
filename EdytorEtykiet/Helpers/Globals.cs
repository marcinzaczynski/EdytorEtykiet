using EdytorEtykiet.Interfaces;
using EdytorEtykiet.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Xml.Serialization;

namespace EdytorEtykiet.Helpers
{
    public class Globals
    {
        public static BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
        public static BitmapImage ImageToImageSource(System.Drawing.Image img)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                img.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        /// <summary>
        /// Zwraca pełną ścieżkę do pliku
        /// </summary>
        /// <returns></returns>
        public static string WybierzObraz()
        {
            OpenFileDialog ofd = new OpenFileDialog(); ;

            ofd.Filter = "Obrazy (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;,*.png";
            ofd.Filter = "Wszystkie pliki (*.*)|*.*";
            var result = ofd.ShowDialog();

            if (result == true)
            {
                return ofd.FileName;
            }

            return null;
        }

        public static bool ZapiszPlik(List<INowyElement> _etykieta)
        {

            //XDocument d = new XDocument(
            //    new XComment("This is a comment."),
            //        new XProcessingInstruction("xml-stylesheet", "href='mystyle.css' title='Compact' type='text/css'"),
            //        new XElement("Etykieta",
            //            new XElement("Book",
            //                new XElement("Title", "Artifacts of Roman Civilization"),
            //                new XElement("Author", "Moreno, Jordao")
            //                ),
            //            new XElement("Book",
            //                new XElement("Title", "Midieval Tools and Implements"),
            //                new XElement("Author", "Gazit, Inbar")
            //                )
            //            ),
            //        new XComment("This is another comment.")
            //        );
            //d.Declaration = new XDeclaration("1.0", "utf-8", "true");
            ////Console.WriteLine(d);
            //d.Save("test.xml");


            XElement element =
            new XElement("WzorEtykiety",
                (from item in _etykieta
                 where item.TypPola == TypyPol.Txt
                 select new XElement(item.TypPola.ToString(),
                     new XElement("IdPola", (item as NowyTekstModel).IdPola),
                     new XElement("Nazwa", (item as NowyTekstModel).Nazwa),
                     new XElement("Tekst", (item as NowyTekstModel).Tekst),
                     new XElement("CzyJestRamka", (item as NowyTekstModel).CzyJestRamka),
                     new XElement("KolorRamki", (item as NowyTekstModel).KolorRamki),
                     new XElement("GruboscRamki", (item as NowyTekstModel).GruboscRamki),
                     new XElement("FontFamily", (item as NowyTekstModel).FontFamily),
                     new XElement("FontSize", (item as NowyTekstModel).FontSize),
                     new XElement("FontWeight", (item as NowyTekstModel).FontWeight),
                     new XElement("FontStyle", (item as NowyTekstModel).FontStyle),
                     new XElement("Wysokosc", (item as NowyTekstModel).Wysokosc),
                     new XElement("Szerokosc", (item as NowyTekstModel).Szerokosc),
                     new XElement("AutoDopasowanie", (item as NowyTekstModel).AutoDopasowanie),
                     new XElement("WyrownanieWPoziomie", (item as NowyTekstModel).WyrownanieWPoziomie),
                     new XElement("WyrownanieWPionie", (item as NowyTekstModel).WyrownanieWPionie)
                     )
                ),
                (from item in _etykieta
                 where item.TypPola == TypyPol.Pic
                 select new XElement(item.TypPola.ToString(),
                     new XElement("IdPola", (item as NowyObrazModel).IdPola),
                     new XElement("Nazwa", (item as NowyObrazModel).Nazwa),
                     new XElement("PelnaSciezka", (item as NowyObrazModel).PelnaSciezka),
                     new XElement("KatObrotu", (item as NowyObrazModel).KatObrotu)
                     )
                ),
                (from item in _etykieta
                 where item.TypPola == TypyPol.Barcode
                 select new XElement(item.TypPola.ToString(),
                     new XElement("IdPola", (item as NowyKodKrModel).IdPola),
                     new XElement("Nazwa", (item as NowyKodKrModel).Nazwa),
                     new XElement("Tekst", (item as NowyKodKrModel).Tekst),
                     new XElement("Typ", (item as NowyKodKrModel).Typ),
                     new XElement("CzyPokazacTekst", (item as NowyKodKrModel).CzyPokazacTekst),
                     new XElement("Szerokosc", (item as NowyKodKrModel).Szerokosc),
                     new XElement("Wysokosc", (item as NowyKodKrModel).Wysokosc),
                     new XElement("FontFamily", (item as NowyKodKrModel).FontFamily),
                     new XElement("FontSize", (item as NowyKodKrModel).FontSize),
                     new XElement("FontWeight", (item as NowyKodKrModel).FontWeight)
                     )
                )
            );

            //var mystrXAML = XamlWriter.Save(_etykieta);

            //mystrXAML = mystrXAML.Replace("<lch:", "<"); // FIX FOR BARCODES

            SaveFileDialog dlg = new SaveFileDialog();

            dlg.DefaultExt = ".etk";
            dlg.Filter = "GEOSoft - etykieta (.etk)|*.etk";
            dlg.RestoreDirectory = true;

            var result = (bool)dlg.ShowDialog();

            if (result)
            {
                try
                {
                    //FileStream filestream = File.Create(dlg.FileName);
                    //StreamWriter streamwriter = new StreamWriter(filestream);
                    //streamwriter.Write(FormatXml(mystrXAML));
                    //streamwriter.Close();
                    //filestream.Close();
                    //File.CreateText(dlg.FileName);
                    element.Save(dlg.FileName);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return result;
        }

        private static string FormatXml(string xml)
        {
            try
            {
                XDocument doc = XDocument.Parse(xml);

                XNamespace ns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

                // USUNIĘCIE MARGINESÓW Z XMLa
                //doc.Descendants(ns + "Label")
                //    .Where(x=> (string)x.Attribute("Name") == "MarginT" || (string)x.Attribute("Name") == "MarginB" || (string)x.Attribute("Name") == "MarginL" || (string)x.Attribute("Name") == "MarginR")
                //    .Remove();


                return doc.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void OtworzPlik()
        {
            OpenFileDialog dlg = new OpenFileDialog();


            dlg.DefaultExt = ".etk";
            dlg.Filter = "GEOSoft - etykieta (.etk)|*.etk";

            var result = (bool)dlg.ShowDialog();

            if (result)
            {
                string aa = File.ReadAllText(dlg.FileName);
                try
                {
                    StringReader stringReader = new StringReader(aa);
                    XmlReader xmlReader = XmlReader.Create(stringReader);
                    EtykietaCanvas et = (EtykietaCanvas)XamlReader.Load(xmlReader);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
