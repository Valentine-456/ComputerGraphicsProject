using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ComputerGraphicsProject.Filters.Function
{
    internal class BrightnessCorrectionFilter : FunctionFilter
    {
        private int BrightnessCoefficient { get; }

        public override string FilterName => $"Brightness Adjustment ({BrightnessCoefficient})";

        public BrightnessCorrectionFilter(int brightnessCoefficient)
        {
            BrightnessCoefficient = brightnessCoefficient;
            for (int i = 0; i < 256; i++)
                LookupTable[i] = RoundToByteBounds(i + BrightnessCoefficient);
        }
    }
}
