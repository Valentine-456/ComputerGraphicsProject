using ComputerGraphicsProject.Filters.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ComputerGraphicsProject.Filters.ColorManipulation
{
    public class AverageDitheringFilter : IImageFilter
    {
        private int LevelsPerChannel;
        public string FilterName => $"Average Dithering ({LevelsPerChannel} levels)";

        public AverageDitheringFilter(int shades)
        {
            if(shades < 2 || shades > 256) {
                throw new ArgumentException("Number of shades must be from 2 to 256");
            }
            this.LevelsPerChannel = shades;
        }

        public WriteableBitmap Apply(WriteableBitmap input)
        {
            input.Lock();
            int width = input.PixelWidth;
            int height = input.PixelHeight;

            List<List<int>> redLevelsPixels = new List<List<int>>(LevelsPerChannel);
            List<List<int>> blueLevelsPixels = new List<List<int>>(LevelsPerChannel);
            List<List<int>> greenLevelsPixels = new List<List<int>>(LevelsPerChannel);
            for (int i = 0; i < LevelsPerChannel; i++)
            {
                redLevelsPixels.Add(new List<int>());
                greenLevelsPixels.Add(new List<int>());
                blueLevelsPixels.Add(new List<int>());
            }


            IntPtr buffer = input.BackBuffer;
            int bytesPerPixel = (input.Format.BitsPerPixel + 7) / 8;
            int stepLength = (int) Math.Round(255.0 / (LevelsPerChannel - 1));

            unsafe
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        byte* pixel = (byte*)buffer + (x + y * width) * bytesPerPixel;

                        var blue = pixel[0];
                        blueLevelsPixels[blue / stepLength].Add(blue);
                        var green = pixel[1];
                        greenLevelsPixels[green / stepLength].Add(green);
                        var red = pixel[2];
                        redLevelsPixels[red / stepLength].Add(red);
                    }
                }

                List<double> redLevelsAverages = redLevelsPixels.Select(ch => ch.Count > 0 ? ch.Average() : 0).ToList();
                List<double> greenLevelsAverages = greenLevelsPixels.Select(ch => ch.Count > 0 ? ch.Average() : 0).ToList();
                List<double> blueLevelsAverages = blueLevelsPixels.Select(ch => ch.Count > 0 ? ch.Average() : 0).ToList();


                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        byte* pixel = (byte*)buffer + (x + y * width) * bytesPerPixel;

                        pixel[0] = Quantize(pixel[0], blueLevelsAverages, stepLength);
                        pixel[1] = Quantize(pixel[1], greenLevelsAverages, stepLength);
                        pixel[2] = Quantize(pixel[2], redLevelsAverages, stepLength);
                    }
                }
            }
            input.AddDirtyRect(new System.Windows.Int32Rect(0, 0, width, height));
            input.Unlock();
            return input;
        }

        private byte Quantize(byte value, List<double> averages, int stepLength)
        {
            int level = Math.Min(value / stepLength, averages.Count - 1);
            int lower = level * stepLength;
            int upper = Math.Min(255, (level + 1) * stepLength);
            return (byte)(value < averages[level] ? lower : upper);
        }


    }
}
