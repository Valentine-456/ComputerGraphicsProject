using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ComputerGraphicsProject.Filters.Function
{
    internal class InvertFilter : FunctionFilter
    {
        public override string FilterName => "Invert";

        public InvertFilter()
        {
            for (int i = 0; i < 256; i++) LookupTable[i] = (byte)(255 - i);
        }
    }
}
