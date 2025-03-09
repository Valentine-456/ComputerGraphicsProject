using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ComputerGraphicsProject.Filters.Function
{
    public abstract class FunctionFilter : IImageFilter
    {
        protected byte[] LookupTable { get; set; } = new byte[256];
        public abstract string FilterName { get; }

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

                        pixel[0] = LookupTable[pixel[0]];
                        pixel[1] = LookupTable[pixel[1]];
                        pixel[2] = LookupTable[pixel[2]];
                    }
                }
                input.AddDirtyRect(new System.Windows.Int32Rect(0, 0, width, height));
                input.Unlock();
                return input;
            }
        }
        protected byte RoundToByteBounds(int value) => (byte)(value < 0 ? 0 : (value > 255 ? 255 : value));
        protected byte RoundToByteBounds(double value) => (byte)(value < 0 ? 0 : (value > 255 ? 255 : value));
    }
}
