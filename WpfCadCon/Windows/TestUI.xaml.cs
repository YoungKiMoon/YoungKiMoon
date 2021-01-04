using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfCadCon.Windows
{
    /// <summary>
    /// TestUI.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TestUI : Window
    {
        public TestUI()
        {
            InitializeComponent();
            ChangeProcess(0);
        }

        private void Ellipse_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            SolidColorBrush activeColor = new SolidColorBrush(Color.FromRgb(110, 128, 147));
            SolidColorBrush deactiveColor = new SolidColorBrush(Color.FromRgb(217, 222, 227));
            SolidColorBrush currentColor = new SolidColorBrush(Color.FromRgb(26, 202, 142));


            Ellipse currentItem = sender as Ellipse;
            int currentIndex = Grid.GetColumn(currentItem.Parent as Grid);
2020
            ChangeProcess(currentIndex);

        }

        private void ChangeProcess(int currentIndex)
        {
            SolidColorBrush activeColor = new SolidColorBrush(Color.FromRgb(110, 128, 147));
            SolidColorBrush deactiveColor = new SolidColorBrush(Color.FromRgb(217, 222, 227));
            SolidColorBrush currentColor = new SolidColorBrush(Color.FromRgb(26, 202, 142));

            // Item
            foreach (Grid eachItem in gridProcessItem.Children.OfType<Grid>())
            {
                int colValue = Grid.GetColumn(eachItem);
                if (currentIndex == colValue)
                {
                    foreach (Ellipse eachEllipse in eachItem.Children.OfType<Ellipse>())
                        if (eachEllipse.Width == 10)
                            eachEllipse.Visibility = Visibility.Visible;
                        else
                            eachEllipse.Fill = currentColor;

                    foreach (Polygon eachPolygon in eachItem.Children.OfType<Polygon>())
                        eachPolygon.Visibility = Visibility.Hidden;
                }
                else if (currentIndex > colValue)
                {
                    foreach (Ellipse eachEllipse in eachItem.Children.OfType<Ellipse>())
                        if (eachEllipse.Width == 10)
                            eachEllipse.Visibility = Visibility.Hidden;
                        else
                            eachEllipse.Fill = activeColor;

                    foreach (Polygon eachPolygon in eachItem.Children.OfType<Polygon>())
                        eachPolygon.Visibility = Visibility.Visible;
                }
                else if (currentIndex < colValue)
                {
                    foreach (Ellipse eachEllipse in eachItem.Children.OfType<Ellipse>())
                        if (eachEllipse.Width == 10)
                            eachEllipse.Visibility = Visibility.Visible;
                        else
                            eachEllipse.Fill = deactiveColor;

                    foreach (Polygon eachPolygon in eachItem.Children.OfType<Polygon>())
                        eachPolygon.Visibility = Visibility.Hidden;
                }
            }

            // Line
            foreach (Rectangle eachItem in gridProcessLine.Children.OfType<Rectangle>())
            {
                int colValue = Grid.GetColumn(eachItem);
                if (currentIndex >= colValue)
                {
                    eachItem.Fill = activeColor;
                }
                else if (currentIndex < colValue)
                {
                    eachItem.Fill = deactiveColor;
                }
            }

            // Line pad
            foreach (Polygon eachItem in gridProcessLine.Children.OfType<Polygon>())
            {
                int colValue = Grid.GetColumn(eachItem);
                if (currentIndex >= colValue)
                {
                    eachItem.Fill = activeColor;
                }
                else if (currentIndex < colValue)
                {
                    eachItem.Fill = deactiveColor;
                }
            }

            // Text
            foreach (TextBlock eachItem in gridProcessText.Children.OfType<TextBlock>())
            {
                int colValue = Grid.GetColumn(eachItem);
                if (currentIndex >= colValue)
                {
                    eachItem.Foreground = activeColor;
                }
                else if (currentIndex < colValue)
                {
                    eachItem.Foreground = deactiveColor;
                }
            }
        }
    }
}
