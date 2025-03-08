using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsProject.Filters.Convolution
{
    internal class LaplacianEdgeDetectionFilter : ConvolutionFilter
    {
        public override string FilterName => "Edge detection";

        protected override double[,] Kernel { get; } = {
            { -1, -1, -1},
            { -1, 8, -1},
            { -1, -1, -1}
        };
    }
}
