using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ComputerGraphicsProject.Filters.Function
{
    internal class GammaCorrectionFilter : FunctionFilter
    {
        private double GammaCoefficient { get; }
        public override string FilterName => $"Gamma Correction ({GammaCoefficient})";

        public GammaCorrectionFilter(double gammaCoefficient)
        {
            GammaCoefficient = gammaCoefficient;
            for (int i = 0; i < 256; i++)
            {
                double newValue = 255 * Math.Pow(i / 255.0, GammaCoefficient);
                LookupTable[i] = RoundToByteBounds(newValue);
            }
        }
    }
}
