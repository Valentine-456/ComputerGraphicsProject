using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System;


namespace ComputerGraphicsProject.ToolTabsViews
{
    public partial class ConvolutionFiltersView : UserControl
    {
        public event EventHandler ApplyBlurFilterRequested;
        public ConvolutionFiltersView()
        {
            InitializeComponent();
        }

        private void Blur_Click(object sender, RoutedEventArgs e)
        {
            ApplyBlurFilterRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
