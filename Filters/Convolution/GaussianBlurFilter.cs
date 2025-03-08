using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsProject.Filters.Convolution
{
    internal class GaussianBlurFilter : ConvolutionFilter
    {
        public override string FilterName => "Gaussian Blurring";

        protected override double[,] Kernel { get; } = {
            {0, 1.0/8, 0},
            {1.0/8, 4.0/8, 1.0/8},
            {0, 1.0/8, 0}
        };
    }
}
