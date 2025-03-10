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
using System.Windows.Media;

namespace ComputerGraphicsProject.ToolTabsViews
{
    public partial class CustomFunctionFiltersView : UserControl
    {
        private List<Point> functionPoints = new List<Point>();
        private Point? selectedPoint = null;
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
            PointsCanvas.Children.Clear();

            foreach (var point in functionPoints)
            {
                FunctionPolyline.Points.Add(new Point(point.X, 255 - point.Y));
                DrawPointIndicator(point);
            }
        }

        private void DrawPointIndicator(Point point)
        {
            Ellipse circle = new Ellipse
            {
                Width = 8,
                Height = 8,
                Fill = Brushes.Red,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            Canvas.SetLeft(circle, point.X - 4);
            Canvas.SetTop(circle, 255 - point.Y - 4);

            PointsCanvas.Children.Add(circle);
        }

        private byte RoundToByteBounds(double value) => (byte)(value < 0 ? 0 : (value > 255 ? 255 : value));
        private double Clamp(double value, double min, double max) => Math.Max(min, Math.Min(max, value));
        private double CalculateDistance(Point p1, Point p2) => Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));


        public void AddEquidistantPoints_Click(object sender, RoutedEventArgs e)
        {
            functionPoints.Add(new Point(255, 255));
            int size = functionPoints.Count;

            for (int i = 0; i < size - 1; i++)
            {
                double newX = i * (255.0 / (size - 1));
                functionPoints[i] = new Point(newX, functionPoints[i].Y);
            }

            functionPoints[size - 2] = new Point(functionPoints[size - 2].X, functionPoints[size - 2].X);
            DrawFunction();
        }

        public void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            Point clickedPoint = e.GetPosition(FunctionCanvas);
            byte x = RoundToByteBounds(clickedPoint.X);
            byte y = RoundToByteBounds(255 - clickedPoint.Y);

            Point? nearest = functionPoints.OrderBy(p => Math.Abs(p.X - clickedPoint.X)).FirstOrDefault();

            bool isWithinDistance = CalculateDistance(nearest.Value, new Point(x, y)) < 10;
            if (nearest.HasValue && isWithinDistance)
            {
                selectedPoint = nearest;
            }
        }

        public void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedPoint.HasValue && e.LeftButton == MouseButtonState.Pressed)
            {
                double newY = RoundToByteBounds(255 - e.GetPosition(FunctionCanvas).Y);
                int index = functionPoints.IndexOf(selectedPoint.Value);

                if (index > 0 && index < functionPoints.Count - 1)
                {
                    double newX = e.GetPosition(FunctionCanvas).X;
                    double leftBound = functionPoints[index - 1].X + 5;
                    double rightBound = functionPoints[index + 1].X - 5;
                    newX = Clamp(newX, leftBound, rightBound);
                    functionPoints[index] = new Point(newX, newY);

                }
                else
                {
                    functionPoints[index] = new Point(functionPoints[index].X, newY);
                }
                selectedPoint = functionPoints[index];
                DrawFunction();
            }

        }

        public void Canvas_DeletePoint_RightClick(object sender, MouseButtonEventArgs e)
        {
            Point clickedPoint = e.GetPosition(FunctionCanvas);
            byte x = RoundToByteBounds(clickedPoint.X);
            byte y = RoundToByteBounds(255 - clickedPoint.Y);

            Point? nearest = functionPoints.OrderBy(p => Math.Abs(p.X - clickedPoint.X)).FirstOrDefault();

            bool isWithinDistance = CalculateDistance(nearest.Value, new Point(x, y)) < 10;
            if (nearest.HasValue && isWithinDistance)
            {
                int index = functionPoints.IndexOf(nearest.Value);
                if (index > 0 && index < functionPoints.Count - 1)
                {
                    functionPoints.RemoveAt(index);
                    DrawFunction();
                }
            }
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
