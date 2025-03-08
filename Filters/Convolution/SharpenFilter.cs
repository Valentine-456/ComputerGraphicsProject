using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsProject.Filters.Convolution
{
    internal class SharpenFilter : ConvolutionFilter
    {
        public override string FilterName => "Sharpen";

        protected override double[,] Kernel { get; } = {
            { 0, -1, 0 }, 
            { -1, 5, -1 }, 
            { 0, -1, 0 }
        };
    }
}
