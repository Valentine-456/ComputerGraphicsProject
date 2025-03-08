using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ComputerGraphicsProject.Filters.Function
{
    internal class InvertFilter : IImageFilter
    {
        private string _name = "Invert";
        public string FilterName { get { return _name; } }

        public WriteableBitmap Apply(WriteableBitmap input)
        {
            input.Lock();
            int width = input.PixelWidth;
            int height = input.PixelHeight;
            unsafe
            {
                IntPtr buffer = input.BackBuffer;
                int bytesPerPixel = (input.Format.BitsPerPixel + 7) / 8;

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        byte* pixel = (byte*)buffer + (x + y*width)*bytesPerPixel;

                        pixel[0] = (byte)(255 - pixel[0]);
                        pixel[1] = (byte)(255 - pixel[1]);
                        pixel[2] = (byte)(255 - pixel[2]);
                    }
                }
                input.AddDirtyRect(new System.Windows.Int32Rect(0, 0, width, height));
                input.Unlock();
                return input;
            }
        }
    }
}
