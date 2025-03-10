using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerGraphicsProject.Filters.Function
{
    public class CustomFunctionFilter : FunctionFilter
    {
        public override string FilterName { get; }
        public List<Point> FunctionPoints { get; }

        [JsonConstructor]
        public CustomFunctionFilter(string filterName, List<Point> points)
        {
            FilterName = filterName;
            FunctionPoints = points.OrderBy(p => p.X).ToList();

            for (int i = 0; i < 256; i++)
            {
                var left = FunctionPoints.Last(p => p.X <= i);
                var right = FunctionPoints.First(p => p.X >= i);

                if (left.X != right.X)
                {
                    double t = (i - left.X) / (double)(right.X - left.X);
                    double interpolated = left.Y + t * (right.Y - left.Y);
                    LookupTable[i] = RoundToByteBounds(interpolated);
                }
                else
                {
                    LookupTable[i] = RoundToByteBounds(left.Y);
                }
            }
        }
    }
}
