using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Linq;
using System;
using Microsoft.Win32;
using System.Text.Json;
using System.IO;
using ComputerGraphicsProject.Filters.Function;

namespace ComputerGraphicsProject.ToolTabsViews
{
    public partial class CustomFunctionFiltersView : UserControl
    {
        private List<Point> functionPoints = new List<Point>();
        public event EventHandler<CustomFunctionFilter> ApplyCustomFilterRequested;

        public CustomFunctionFiltersView()
        {
            InitializeComponent();
            InitializeCustomFunction();
        }

        private void InitializeCustomFunction()
        {
            functionPoints.Clear();
            functionPoints.Add(new Point(0, 0));
            functionPoints.Add(new Point(255, 255));
            DrawFunction();
        }

        private void DrawFunction()
        {
            FunctionPolyline.Points.Clear();
            foreach (var point in functionPoints)
                FunctionPolyline.Points.Add(new Point(point.X, 255 - point.Y));
        }

        private byte RoundToByteBounds(double value) => (byte)(value < 0 ? 0 : (value > 255 ? 255 : value));


        public void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            Point clickedPoint = e.GetPosition(FunctionCanvas);
            byte x = RoundToByteBounds(clickedPoint.X);
            byte y = RoundToByteBounds(255 - clickedPoint.Y);

            functionPoints.Add(new Point(x,y));
            functionPoints = functionPoints.OrderBy(p => p.X).ToList();

            DrawFunction();
        }

        public void Canvas_MouseMove(object sender, MouseEventArgs e)
        {

        }

        public void AddPoint_Click(object sender, RoutedEventArgs e)
        {
            functionPoints.Add(new Point(128, 128));
            functionPoints = functionPoints.OrderBy(p => p.X).ToList();
            DrawFunction();
        }

        public void Reset_Click(object sender, RoutedEventArgs e)
        {
            InitializeCustomFunction();
        }

        public void SaveFilter_Click(object sender, RoutedEventArgs e)
        {
            string filterName = FilterNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(filterName))
            {
                MessageBox.Show("Please enter a filter name before saving.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON Files (*.json)|*.json",
                Title = "Save Custom Function Filter",
                FileName = filterName + ".json"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                SaveFilterToFile(saveFileDialog.FileName);
            }
        }

        private void SaveFilterToFile(string filePath)
        {
            var filterData = new
            {
                Name = FilterNameTextBox.Text.Trim(),
                Points = functionPoints
            };

            string json = JsonSerializer.Serialize(filterData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public void LoadFilter_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON Files (*.json)|*.json",
                Title = "Load Custom Function Filter"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                LoadFilterFromFile(openFileDialog.FileName);
            }
        }

        private void LoadFilterFromFile(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                var filterData = JsonSerializer.Deserialize<CustomFilterData>(json);

                if (filterData != null)
                {
                    FilterNameTextBox.Text = filterData.Name;
                    functionPoints = filterData.Points.OrderBy(p => p.X).ToList();
                    DrawFunction();
                }
            }
            catch
            {
                MessageBox.Show("Error loading filter file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            string filterName = FilterNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(filterName))
            {
                MessageBox.Show("Please enter a filter name before applying.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CustomFunctionFilter customFilter = new CustomFunctionFilter(filterName, functionPoints);
            ApplyCustomFilterRequested?.Invoke(this, customFilter);
        }

        private class CustomFilterData
        {
            public string Name { get; set; }
            public List<Point> Points { get; set; }
        }

    }
}
