using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using EdytorEtykiet.ViewModel;
using System.Xml.Linq;
using System.Xml;
using System.Windows;

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
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "Obrazy (*.bmp, *.jpg, *.png)|*.BMP;*.JPG;,*.PNG";
            var result = ofd.ShowDialog();

            if (result == true)
            {
                return ofd.FileName;
            }

            return null;
        }

        public static bool ZapiszPlik(List<FrameworkElement> _etykieta)
        {
            var mystrXAML = XamlWriter.Save(_etykieta);

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
                    FileStream filestream = File.Create(dlg.FileName);
                    StreamWriter streamwriter = new StreamWriter(filestream);
                    streamwriter.Write(FormatXml(mystrXAML));
                    streamwriter.Close();
                    filestream.Close();
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
