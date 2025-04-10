﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsProject.Filters.Convolution
{
    internal class BlurFilter : ConvolutionFilter
    {
        public override string FilterName => "Blurring";

        protected override double[,] Kernel { get; } = {
            { 1 / 9.0, 1 / 9.0, 1 / 9.0 },
            { 1 / 9.0, 1 / 9.0, 1 / 9.0 },
            { 1 / 9.0, 1 / 9.0, 1 / 9.0 }
        };
    }
}
