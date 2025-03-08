using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsProject.Filters.Convolution
{
    internal class EmbossFilter : ConvolutionFilter
    {
        public override string FilterName => "South‐east Emboss";

        protected override double[,] Kernel { get; } = {
            { -2, -1, 0 },
            { -1, 1, 1 },
            { 0, 1, 2 }
        };
    }
}
