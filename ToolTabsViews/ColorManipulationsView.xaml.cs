using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComputerGraphicsProject.ToolTabsViews
{
    /// <summary>
    /// Interaction logic for ColorManipulationsView.xaml
    /// </summary>
    public partial class ColorManipulationsView : UserControl
    {
        public event EventHandler ApplyGreyscaleFilterRequested;

        public ColorManipulationsView()
        {
            InitializeComponent();
        }

        private void Greyscale_Click(object sender, RoutedEventArgs e)
        {
            ApplyGreyscaleFilterRequested?.Invoke(this, EventArgs.Empty);
        }

        private void Dithering_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ColorQuantization_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
