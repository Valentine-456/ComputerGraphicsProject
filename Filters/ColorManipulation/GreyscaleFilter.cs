using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ComputerGraphicsProject.Filters.ColorManipulation
{
    public class GreyscaleFilter : IImageFilter
    {
        public string FilterName => "Greyscale conversion";

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
                        byte* pixel = (byte*)buffer + (x + y * width) * bytesPerPixel;

                        var blue = pixel[0];
                        var green = pixel[1];
                        var red = pixel[2];

                        byte grey = Clamp(0.299 * red + 0.587 * green + 0.114 * blue, 0, 255);

                        pixel[0] = grey;
                        pixel[1] = grey;
                        pixel[2] = grey;
                    }
                }
                input.AddDirtyRect(new System.Windows.Int32Rect(0, 0, width, height));
                input.Unlock();
                return input;
            }
        }

        private byte Clamp(double value, double min, double max) => (byte)Math.Max(min, Math.Min(max, value));
    }
}
