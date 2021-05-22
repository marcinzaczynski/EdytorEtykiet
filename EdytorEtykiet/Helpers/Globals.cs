using SimpleLabelLibrary.Interfaces;
using SimpleLabelLibrary.Models;
using SimpleLabelLibrary.Helpers;
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

        public static bool ZapiszPlik(List<IDefaultField> _etykieta)
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
            new XElement("LabelTemplate",
                (from item in _etykieta
                 where item.FieldType == FieldTypes.Text
                 select new XElement(item.FieldType.ToString(),
                     new XElement("Id", (item as TextField).Id),
                     new XElement("Name", (item as TextField).Name),
                     new XElement("Left", (item as TextField).Left),
                     new XElement("Top", (item as TextField).Top),
                     new XElement("Text", (item as TextField).Text),
                     new XElement("ShowBorder", (item as TextField).ShowBorder),
                     new XElement("BorderColor", (item as TextField).BorderColor),
                     new XElement("BorderWidth", (item as TextField).BorderWidth),
                     new XElement("FontFamily", (item as TextField).FontFamily),
                     new XElement("FontSize", (item as TextField).FontSize),
                     new XElement("FontWeight", (item as TextField).FontWeight),
                     new XElement("FontStyle", (item as TextField).FontStyle),
                     new XElement("Height", (item as TextField).Height),
                     new XElement("Width", (item as TextField).Width),
                     new XElement("AutoFit", (item as TextField).AutoFit),
                     new XElement("HorizontalAlignement", (item as TextField).HorizontalAlignement),
                     new XElement("VerticalAlignement", (item as TextField).VerticalAlignement)
                     )
                ),
                (from item in _etykieta
                 where item.FieldType == FieldTypes.Picture
                 select new XElement(item.FieldType.ToString(),
                     new XElement("Id", (item as PictureField).Id),
                     new XElement("Name", (item as PictureField).Name),
                     new XElement("Left", (item as PictureField).Left),
                     new XElement("Top", (item as PictureField).Top),
                     new XElement("Path", (item as PictureField).Path),
                     new XElement("RotationAngle", (item as PictureField).RotationAngle)
                     )
                ),
                (from item in _etykieta
                 where item.FieldType == FieldTypes.Barcode
                 select new XElement(item.FieldType.ToString(),
                     new XElement("Id", (item as BarcodeField).Id),
                     new XElement("Name", (item as BarcodeField).Name),
                     new XElement("Left", (item as BarcodeField).Left),
                     new XElement("Top", (item as BarcodeField).Top),
                     new XElement("TextToEncode", (item as BarcodeField).TextToEncode),
                     new XElement("BarcodeType", (item as BarcodeField).BarcodeType),
                     new XElement("ShowTextToEncode", (item as BarcodeField).ShowTextToEncode),
                     new XElement("Width", (item as BarcodeField).Width),
                     new XElement("Height", (item as BarcodeField).Height),
                     new XElement("FontFamily", (item as BarcodeField).FontFamily),
                     new XElement("FontSize", (item as BarcodeField).FontSize),
                     new XElement("FontWeight", (item as BarcodeField).FontWeight)
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
                    element.Save(dlg.FileName);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return result;
        }

        public static XElement OtworzPlik()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            XElement element = null;

            dlg.DefaultExt = ".etk";
            dlg.Filter = "GEOSoft - etykieta (.etk)|*.etk";

            var result = (bool)dlg.ShowDialog();

            if (result)
            {
                try
                {
                    element = XElement.Load(dlg.FileName);
                }
                catch (Exception)
                {
                    throw;
                    
                }
            }
            return element;
        }
    }
}
