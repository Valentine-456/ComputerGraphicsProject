using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputerGraphicsProject.Filters.ColorManipulation
{
    public class KMeansQuantizationFilter : IImageFilter
    {
        private int K;
        private ParallelOptions Options = new ParallelOptions
        {
            MaxDegreeOfParallelism = 10
        };

        public string FilterName => $"K-Means Quantization ({K} colors)";


        public KMeansQuantizationFilter(int k)
        {
            K = k;
        }

        public WriteableBitmap Apply(WriteableBitmap input)
        {
            input.Lock();
            int width = input.PixelWidth;
            int height = input.PixelHeight;
            List<ColorPoint> pixels = new List<ColorPoint>(width * height);
            IntPtr buffer = input.BackBuffer;
            int bytesPerPixel = (input.Format.BitsPerPixel + 7) / 8;

            unsafe
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        byte* pixel = (byte*)buffer + (x + y * width) * bytesPerPixel;
                        pixels.Add(new ColorPoint(pixel[0], pixel[1], pixel[2]));
                    }
                }
            }

            List<ColorPoint> centroids = InitializeCentroids(pixels);
            List<int> nearestCentroidIndices  = Enumerable.Repeat(-1, pixels.Count).ToList();


            bool changed = true;
            int maxIterations = 25;

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                int wasChanged = 0;

                Parallel.For(0, pixels.Count, Options, i =>
                {
                    int newNearestCentroid = GetClosestCentroid(pixels[i], centroids);
                    if (nearestCentroidIndices[i] != newNearestCentroid)
                    {
                        nearestCentroidIndices[i] = newNearestCentroid;
                        Interlocked.Exchange(ref wasChanged, 1);
                    }
                });

                if (wasChanged == 0)
                    break;

                for (int i = 0; i < K; i++)
                {
                    var assignedPoints = pixels
                        .Where((_, index) => nearestCentroidIndices[index] == i)
                        .ToList();

                    if (assignedPoints.Count == 0) continue;

                    centroids[i] = RecalculateCentroid(assignedPoints);
                }
            }


            unsafe
            {
                int i = 0;
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        byte* pixel = (byte*)buffer + (x + y * width) * bytesPerPixel;
                        ColorPoint centroid = centroids[nearestCentroidIndices[i]];

                        pixel[0] = centroid.B;
                        pixel[1] = centroid.G;
                        pixel[2] = centroid.R;

                        i++;
                    }
                }
            }

            input.AddDirtyRect(new System.Windows.Int32Rect(0, 0, width, height));
            input.Unlock();
            return input;
        }

        private List<ColorPoint> InitializeCentroids(List<ColorPoint> pixels) {
            var random = new Random();
            return pixels.OrderBy(pixel => random.Next()).Take(K).ToList();
        }

        private int GetClosestCentroid(ColorPoint pixel, List<ColorPoint> centroids)
        {
            double minDistance = int.MaxValue;
            int closestIndex = 0;
            for (int i = 0; i < centroids.Count; i++)
            {
                double dist = EuclideanDistanceSquared(pixel, centroids[i]);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closestIndex = i;
                }
            }
            return closestIndex;
        }

        private ColorPoint RecalculateCentroid(List<ColorPoint> points)
        {
            byte avgB = (byte)points.Average(p => p.B);
            byte avgG = (byte)points.Average(p => p.G);
            byte avgR = (byte)points.Average(p => p.R);
            return new ColorPoint(avgB, avgG, avgR);
        }

        private int EuclideanDistanceSquared(ColorPoint a, ColorPoint b)
        {
            int db = a.B - b.B;
            int dg = a.G - b.G;
            int dr = a.R - b.R;
            return db * db + dg * dg + dr * dr;
        }

        private struct ColorPoint
        {
            public byte B;
            public byte G;
            public byte R;

            public ColorPoint(byte b, byte g, byte r)
            {
                B = b;
                G = g;
                R = r;
            }
        }
    }
}
