using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ComputerGraphicsProject.Filters
{
    public interface IImageFilter
    {
        string FilterName { get; }
        WriteableBitmap Apply(WriteableBitmap input);
    }
}
