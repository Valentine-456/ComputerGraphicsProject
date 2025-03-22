using ComputerGraphicsProject.Filters.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ComputerGraphicsProject.Filters.ColorManipulation
{
    public class AverageDitheringFilter : FunctionFilter
    {
        private int LevelsPerChannel;
        public override string FilterName => $"Average Dithering ({LevelsPerChannel} levels)";

        public AverageDitheringFilter(int shades)
        {
            if(shades < 2 || shades > 256) {
                throw new ArgumentException("Number of shades must be from 2 to 256");
            }
            this.LevelsPerChannel = shades;

            double stepLength = 255.0 / (shades - 1);
            for (int i = 0; i < 256; i++)
            {
                int level = (int) Math.Round(i / stepLength);
                LookupTable[i] = RoundToByteBounds(level * stepLength);
            }
        }
    }
}
