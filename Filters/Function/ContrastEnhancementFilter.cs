using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ComputerGraphicsProject.Filters.Function
{
    internal class ContrastEnhancementFilter : FunctionFilter
    {
        private double ContrastCoefficient { get; }
        public override string FilterName => $"Contrast Enhancement ({ContrastCoefficient})";

        public ContrastEnhancementFilter(double contrastCoefficient)
        {
            ContrastCoefficient = contrastCoefficient;
            for (int i = 0; i < 256; i++)
            {
                int newValue = (int)((i - 128) * ContrastCoefficient) + 128;
                LookupTable[i] = RoundToByteBounds(newValue);
            }
        }
    }
}
