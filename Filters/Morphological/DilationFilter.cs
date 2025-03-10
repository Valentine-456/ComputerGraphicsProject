using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ComputerGraphicsProject.Filters.Convolution
{
    internal class DilationFilter : IImageFilter
    {
        public string FilterName => "Dilation";
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
                        byte maxRed = 0, maxGreen = 0, maxBlue = 0;

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

                                maxBlue = Math.Max(maxBlue, pixelBlue);
                                maxGreen = Math.Max(maxGreen, pixelGreen);
                                maxRed = Math.Max(maxRed, pixelRed);
                            }
                        }

                        byte* outputPixel = outputBuffer + (y * width + x) * bytesPerPixel;
                        outputPixel[0] = maxBlue;
                        outputPixel[1] = maxGreen;
                        outputPixel[2] = maxRed;
                    }
                }
            }

            input.Unlock();
            output.AddDirtyRect(new Int32Rect(0, 0, width, height));
            output.Unlock();
            return output;
        }

        private bool isInsideBorder(int val, int length)=> (val >= 0 && val <= length - 1);

        private int Clamp(int value, int min, int max) => Math.Max(min, Math.Min(max, value));
    }
}
