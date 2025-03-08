using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ComputerGraphicsProject.Filters.Function
{
    internal class BrightnessCorrectionFilter: IImageFilter
    {
        public int BrightnessCoefficient { get; set; } = 5;

        public string FilterName => $"Brightness Adjustment ({BrightnessCoefficient})";

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


                        pixel[0] = RoundToByteBounds(pixel[0] + BrightnessCoefficient);
                        pixel[1] = RoundToByteBounds(pixel[1] + BrightnessCoefficient);
                        pixel[2] = RoundToByteBounds(pixel[2] + BrightnessCoefficient);
                    }
                }
                input.AddDirtyRect(new System.Windows.Int32Rect(0, 0, width, height));
                input.Unlock();
                return input;
            }
        }

        private byte RoundToByteBounds(int value) => (byte)(value < 0 ? 0 : (value > 255 ? 255 : value));
    }
}
