using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System;

namespace ComputerGraphicsProject.ToolTabsViews
{
    public partial class FunctionFiltersView : UserControl
    {
        public event EventHandler ApplyInvertFilterRequested;
        public event EventHandler ApplyBrightenFilterRequested;
        public event EventHandler ApplyDarkenFilterRequested;
        public event EventHandler ApplyContrastUpFilterRequested;
        public event EventHandler ApplyContrastDownFilterRequested;
        public event EventHandler ApplyGammaExpandFilterRequested;
        public event EventHandler ApplyGammaCompressFilterRequested;

        public FunctionFiltersView()
        {
            InitializeComponent();
        }

        private void Invert_Click(object sender, RoutedEventArgs e)
        {
            ApplyInvertFilterRequested?.Invoke(this, EventArgs.Empty);
        }

        private void Brighten_Click(object sender, RoutedEventArgs e)
        {
            ApplyBrightenFilterRequested?.Invoke(this, EventArgs.Empty);
        }

        private void Darken_Click(object sender, RoutedEventArgs e)
        {
            ApplyDarkenFilterRequested?.Invoke(this, EventArgs.Empty);
        }

        private void Contrast_Up_Click(object sender, RoutedEventArgs e)
        {
            ApplyContrastUpFilterRequested?.Invoke(this, EventArgs.Empty);
        }

        private void Contrast_Down_Click(object sender, RoutedEventArgs e)
        {
            ApplyContrastDownFilterRequested?.Invoke(this, EventArgs.Empty);
        }

        private void Gamma_Expand_Click(object sender, RoutedEventArgs e)
        {
            ApplyGammaExpandFilterRequested?.Invoke(this, EventArgs.Empty);
        }

        private void Gamma_Compress_Click(object sender, RoutedEventArgs e)
        {
            ApplyGammaCompressFilterRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
