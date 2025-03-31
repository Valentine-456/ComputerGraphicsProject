using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ComputerGraphicsProject.Filters.ColorManipulation
{
    public class YCCConversionFilter : IImageFilter
    {
        public string FilterName => "YCC Conversion + dithering (K=7)";

        private AverageDitheringFilter averageDitheringFilter = new AverageDitheringFilter(7);

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

                        byte Y = Clamp(0.299 * red + 0.587 * green + 0.114 * blue, 0, 255);
                        byte Cb = Clamp(128 - 0.168736 * red - 0.331264 * green + 0.5 * blue, 0, 255);
                        byte Cr = Clamp(128 + 0.5 * red - 0.418688 * green - 0.081312 * blue, 0, 255);

                        pixel[0] = Y;
                        pixel[1] = Cb;
                        pixel[2] = Cr;
                    }
                }
                input.AddDirtyRect(new System.Windows.Int32Rect(0, 0, width, height));
                input.Unlock();
            }

            var ditheredInput = averageDitheringFilter.Apply(input);

            ditheredInput.Lock();
            unsafe
            {
                IntPtr buffer = ditheredInput.BackBuffer;
                int bytesPerPixel = (ditheredInput.Format.BitsPerPixel + 7) / 8;


                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        byte* pixel = (byte*)buffer + (x + y * width) * bytesPerPixel;

                        var Y = pixel[0];
                        var Cb = pixel[1];
                        var Cr = pixel[2];

                        byte blue = Clamp(Y + 1.772 * (Cb - 128), 0, 255);
                        byte green = Clamp(Y - 0.344136 * (Cb - 128) - 0.71436 * (Cr - 128), 0, 255);
                        byte red = Clamp(Y + 1.402 * (Cr - 128), 0, 255);

                        pixel[0] = blue;
                        pixel[1] = green;
                        pixel[2] = red;
                    }
                }
                ditheredInput.AddDirtyRect(new System.Windows.Int32Rect(0, 0, width, height));
                ditheredInput.Unlock();
                return ditheredInput;
            }
        }

            private byte Clamp(double value, double min, double max) => (byte)Math.Max(min, Math.Min(max, value));
        }
    }
