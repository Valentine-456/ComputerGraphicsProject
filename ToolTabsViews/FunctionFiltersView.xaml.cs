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
    }
}
