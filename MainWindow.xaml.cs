using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ComputerGraphicsProject.Filters;
using ComputerGraphicsProject.Filters.Convolution;
using ComputerGraphicsProject.Filters.Function;
using ComputerGraphicsProject.ToolTabsViews;
using ComputerGraphicsProject.Utils;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;


namespace ComputerGraphicsProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<string> _filterHistory = new ObservableCollection<string>();
        public MainWindow()
        {
            InitializeComponent();
            SelectFunctionFilters_Click(this, new RoutedEventArgs());
            FilterHistoryList.ItemsSource = _filterHistory;
        }

        private void OpenImage_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap image = FileOperations.OpenFile();
            if (image != null)
            {
                OriginalImage.Source = image;
                ProcessedImage.Source = image.Clone();
                _filterHistory.Clear();
                _filterHistory.Add("Image loaded");
            }
        }

        private void SaveImage_Click(Object sender, RoutedEventArgs e)
        {
            if (ProcessedImage.Source is WriteableBitmap bitmap)
            {
                FileOperations.SaveFile(bitmap);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void SelectFunctionFilters_Click(object sender, RoutedEventArgs e)
        {
            FunctionFiltersView toolTab = new FunctionFiltersView();
            toolTab.ApplyInvertFilterRequested += OnApplyInvertFilterRequested;
            toolTab.ApplyBrightenFilterRequested += OnApplyBrightenFilterRequested;
            toolTab.ApplyDarkenFilterRequested += OnApplyDarkenFilterRequested;
            toolTab.ApplyContrastUpFilterRequested += OnApplyContrastUpFilterRequested;
            toolTab.ApplyContrastDownFilterRequested += OnApplyContrastDownFilterRequested;
            toolTab.ApplyGammaExpandFilterRequested += OnApplyGammaExpandFilterRequested;
            toolTab.ApplyGammaCompressFilterRequested += OnApplyGammaCompressFilterRequested;
            ToolTab.Content = toolTab;
        }

        private void SelectConvolutionFilters_Click(object sender, RoutedEventArgs e)
        {
            ConvolutionFiltersView tooltab = new ConvolutionFiltersView();
            tooltab.ApplyBlurFilterRequested += OnApplyBlurFilterRequested;
            tooltab.ApplyGaussianBlurFilterRequested += OnApplyGaussianBlurFilterRequested;
            tooltab.ApplySharpenFilterRequested += OnApplySharpenFilterRequested;
            tooltab.ApplyEdgeDetectionFilterRequested += OnApplyEdgeDetectionFilterRequested;
            tooltab.ApplyEmbossFilterRequested += OnApplyEmbossFilterRequested;
            tooltab.ApplyErosionFilterRequested += OnApplyErosionFilterRequested;
            tooltab.ApplyDilationFilterRequested += OnApplyDilationFilterRequested;
            ToolTab.Content = tooltab;
        }

        private void SelectCustomFunctionFilters_Click(object sender, RoutedEventArgs e)
        {
            CustomFunctionFiltersView tooltab = new CustomFunctionFiltersView();
            tooltab.ApplyCustomFilterRequested += OnApplyCustomFilterRequested;
            ToolTab.Content = tooltab;
        }

        private void RestoreOriginal_Click(object sender, RoutedEventArgs e)
        {
            ProcessedImage.Source = OriginalImage.Source.CloneCurrentValue();
            _filterHistory.Clear();
            _filterHistory.Add("Restored original");
        }


        private void OnApplyInvertFilterRequested(object sender, EventArgs e)
        {
            InvertFilter filter = new InvertFilter();
            ApplyFilter(filter);
        }

        private void OnApplyBrightenFilterRequested(object sender, EventArgs e)
        {
            BrightnessCorrectionFilter filter = new BrightnessCorrectionFilter(10);
            ApplyFilter(filter);
        }

        private void OnApplyDarkenFilterRequested(object sender, EventArgs e)
        {
            BrightnessCorrectionFilter filter = new BrightnessCorrectionFilter(-10);
            ApplyFilter(filter);
        }

        private void OnApplyContrastUpFilterRequested(object sender, EventArgs e)
        {
            ContrastEnhancementFilter filter = new ContrastEnhancementFilter(1.25);
            ApplyFilter(filter);
        }

        private void OnApplyContrastDownFilterRequested(object sender, EventArgs e)
        {
            ContrastEnhancementFilter filter = new ContrastEnhancementFilter(0.8);
            ApplyFilter(filter);
        }

        private void OnApplyGammaExpandFilterRequested(Object sender, EventArgs e)
        {
            GammaCorrectionFilter filter = new GammaCorrectionFilter(2);
            ApplyFilter(filter);
        }

        private void OnApplyGammaCompressFilterRequested(Object sender, EventArgs e)
        {
            GammaCorrectionFilter filter = new GammaCorrectionFilter(0.5);
            ApplyFilter(filter);
        }

        private void OnApplyBlurFilterRequested(object sender, EventArgs e)
        {
            BlurFilter filter = new BlurFilter();
            ApplyFilter(filter);
        }

        private void OnApplyGaussianBlurFilterRequested(object sender, EventArgs e)
        {
            ConvolutionFilter filter = new GaussianBlurFilter();
            ApplyFilter(filter);
        }

        private void OnApplySharpenFilterRequested(object sender, EventArgs e)
        {
            ConvolutionFilter filter = new SharpenFilter();
            ApplyFilter(filter);
        }

        private void OnApplyEdgeDetectionFilterRequested(object sender, EventArgs e)
        {
            ConvolutionFilter filter = new LaplacianEdgeDetectionFilter();
            ApplyFilter(filter);
        }

        private void OnApplyEmbossFilterRequested(object sender, EventArgs e)
        {
            ConvolutionFilter filter = new EmbossFilter();
            ApplyFilter(filter);
        }

        private void OnApplyCustomFilterRequested(object sender, CustomFunctionFilter filter)
        {
            ApplyFilter(filter);
        }

        private void OnApplyErosionFilterRequested(object sender, EventArgs e)
        {
            IImageFilter filter = new ErosionFilter();
            ApplyFilter(filter);
        }

        private void OnApplyDilationFilterRequested(object sender, EventArgs e)
        {
            IImageFilter filter = new DilationFilter();
            ApplyFilter(filter);
        }


        private void ApplyFilter(IImageFilter filter)
        {
            if (ProcessedImage.Source != null)
            {
                ProcessedImage.Source = filter.Apply(ProcessedImage.Source as WriteableBitmap);
                _filterHistory.Add(filter.FilterName);
            }
            else
            {
                MessageBox.Show("No image loaded!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
