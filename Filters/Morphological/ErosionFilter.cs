using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ComputerGraphicsProject.Filters.Convolution
{
    internal class ErosionFilter : IImageFilter
    {
        public string FilterName => "Erosion";
        private double[,] Kernel { get; } = {
            { 1, 1, 1 },
            { 1, 1, 1 },
            { 1, 1, 1 }
        };

        public WriteableBitmap Apply(WriteableBitmap input)
        {
            int width = input.PixelWidth;
            int height = input.PixelHeight;
            int kernelOffset = Kernel.GetLength(0) / 2;

            WriteableBitmap output = input.Clone();
            input.Lock();
            output.Lock();

            unsafe
            {
                byte* inputBuffer = (byte*)input.BackBuffer;
                byte* outputBuffer = (byte*)output.BackBuffer;
                int bytesPerPixel = (input.Format.BitsPerPixel + 7) / 8;

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        byte minRed = 255, minGreen = 255, minBlue = 255;

                        for (int i = -kernelOffset; i <= kernelOffset; i++)
                        {
                            for (int j = -kernelOffset; j <= kernelOffset; j++)
                            {
                                int currentX = x + i;
                                int currentY = y + j;

                                byte pixelBlue = 0;
                                byte pixelGreen = 0;
                                byte pixelRed = 0;

                                if (isInsideBorder(currentX, width) && isInsideBorder(currentY, height))
                                {
                                    byte* pixel = inputBuffer + (currentX + currentY * width) * bytesPerPixel;
                                    pixelBlue = pixel[0];
                                    pixelGreen = pixel[1];
                                    pixelRed = pixel[2];
                                }

                                minBlue = Math.Min(minBlue, pixelBlue);
                                minGreen = Math.Min(minGreen, pixelGreen);
                                minRed = Math.Min(minRed, pixelRed);
                            }
                        }

                        byte* outputPixel = outputBuffer + (y * width + x) * bytesPerPixel;
                        outputPixel[0] = minBlue;
                        outputPixel[1] = minGreen;
                        outputPixel[2] = minRed;
                    }
                }
            }

            input.Unlock();
            output.AddDirtyRect(new Int32Rect(0, 0, width, height));
            output.Unlock();
            return output;
        }

        private bool isInsideBorder(int val, int length) => (val >= 0 && val <= length - 1);

        private int Clamp(int value, int min, int max) => Math.Max(min, Math.Min(max, value));
    }
}
