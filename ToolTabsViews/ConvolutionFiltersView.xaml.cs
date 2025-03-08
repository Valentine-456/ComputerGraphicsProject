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
        public event EventHandler ApplyGaussianBlurFilterRequested;
        public event EventHandler ApplySharpenFilterRequested;
        public event EventHandler ApplyEdgeDetectionFilterRequested;
        public event EventHandler ApplyEmbossFilterRequested;

        public ConvolutionFiltersView()
        {
            InitializeComponent();
        }

        private void Blur_Click(object sender, RoutedEventArgs e)
        {
            ApplyBlurFilterRequested?.Invoke(this, EventArgs.Empty);
        }

        private void GaussianBlur_Click(object sender, RoutedEventArgs e)
        {
            ApplyGaussianBlurFilterRequested?.Invoke(this, EventArgs.Empty);
        }

        private void Sharpen_Click(object sender, RoutedEventArgs e)
        {
            ApplySharpenFilterRequested?.Invoke(this, EventArgs.Empty);
        }

        private void EdgeDetection_Click(object sender, RoutedEventArgs e)
        {
            ApplyEdgeDetectionFilterRequested?.Invoke(this, EventArgs.Empty);
        }

        private void Emboss_Click(object sender, RoutedEventArgs e)
        {
            ApplyEmbossFilterRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
