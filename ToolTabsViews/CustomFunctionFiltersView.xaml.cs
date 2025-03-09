using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Linq;
using System;

namespace ComputerGraphicsProject.ToolTabsViews
{
    public partial class CustomFunctionFiltersView : UserControl
    {
        private List<Point> functionPoints = new List<Point>();

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

        }
    }
}
