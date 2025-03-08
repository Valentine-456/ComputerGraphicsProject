using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputerGraphicsProject.Utils
{
    public static class FileOperations
    {
        public static WriteableBitmap OpenFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if(dialog.ShowDialog()  == true)
            {
                Uri uri = new Uri(dialog.FileName);
                BitmapImage bitmapImage = new BitmapImage(uri);
                WriteableBitmap bitmap = new WriteableBitmap(bitmapImage);
                return ConvertToBgra32(bitmap);
            }
            return null;
        }


        public static void SaveFile(WriteableBitmap bitmap)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PNG Files (*.png)|*.png|JPEG Files (*.jpg)|*.jpg|BMP Files (*.bmp)|*.bmp"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));

                using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    encoder.Save(stream);
                }
            }
        }

        private static WriteableBitmap ConvertToBgra32(WriteableBitmap bitmap)
        {
            FormatConvertedBitmap converted = new FormatConvertedBitmap();
            converted.BeginInit();
            converted.Source = bitmap;
            converted.DestinationFormat = PixelFormats.Bgra32;
            converted.EndInit();

            return new WriteableBitmap(converted);
        }
    }
}
