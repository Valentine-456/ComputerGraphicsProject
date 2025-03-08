using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ComputerGraphicsProject.Filters.Convolution
{
    internal class BlurFilter : IImageFilter
    {
        public string FilterName => "Blur";
        private double[,] Kernel = {
            { 1 / 9.0, 1 / 9.0, 1 / 9.0 },
            { 1 / 9.0, 1 / 9.0, 1 / 9.0 },
            { 1 / 9.0, 1 / 9.0, 1 / 9.0 }
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

                for(int x = 0;  x < width; x++)
                {
                    for(int y = 0; y < height; y++)
                    {
                        double sumGreen = 0;
                        double sumBlue = 0;
                        double sumRed = 0;

                        for(int i = -kernelOffset;  i <= kernelOffset; i++)
                        {
                            for (int j = -kernelOffset; j <= kernelOffset; j++)
                            {
                                int currentX = Clamp(x+i, 0, width - 1);
                                int currentY = Clamp(y+j, 0, height - 1);

                                byte* pixel = (byte*)inputBuffer + (currentX + currentY * width) * bytesPerPixel;
                                double kernelValue = Kernel[i + kernelOffset, j + kernelOffset];

                                sumBlue += pixel[0] * kernelValue;
                                sumGreen += pixel[1] * kernelValue;
                                sumRed += pixel[2] * kernelValue;
                            }
                        }

                        byte* outputPixel = outputBuffer + (y * width + x) * bytesPerPixel;
                        outputPixel[0] = (byte) Clamp((int)sumBlue, 0, 255);
                        outputPixel[1] = (byte) Clamp((int)sumGreen, 0, 255);
                        outputPixel[2] = (byte) Clamp((int)sumRed, 0, 255);
                    }
                }
            }

            input.Unlock();
            output.AddDirtyRect(new Int32Rect(0, 0, width, height));
            output.Unlock();
            return output;
        }

        private int Clamp(int value, int min, int max) => Math.Max(min, Math.Min(max, value));

    }
}
