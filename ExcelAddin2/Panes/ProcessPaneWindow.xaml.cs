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
using System.Windows.Navigation;
using System.Windows.Shapes;

using ExcelAddIn.ExcelServices;
using ExcelAddIn.Commons;

namespace ExcelAddIn.Panes
{
    /// <summary>
    /// ProcessPaneWPF.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ProcessPaneWPF : UserControl
    {
        public ProcessPaneWPF()
        {
            InitializeComponent();
            ChangeProcess(0);
        }


        private void Ellipse_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            Ellipse currentItem = sender as Ellipse;
            int currentIndex = Grid.GetColumn(currentItem.Parent as Grid);

            ChangeProcess(currentIndex);
            ChangeSheet(currentIndex);
        }

        private void ChangeProcess(int currentIndex)
        {
            SolidColorBrush activeColor = new SolidColorBrush(Color.FromRgb(110, 128, 147));
            SolidColorBrush deactiveColor = new SolidColorBrush(Color.FromRgb(202, 209, 216));
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

        private void ChangeSheet(int currentIndex)
        {
            switch (currentIndex)
            {
                case 0:
                    ExcelService.ChangeSheet(EXCELSHEET_LIST.SHEET_BASIC);
                    break;
                case 1:
                    ExcelService.ChangeSheet(EXCELSHEET_LIST.SHEET_SHEEL);
                    break;
                case 2:
                    ExcelService.ChangeSheet(EXCELSHEET_LIST.SHEET_ROOF);
                    break;
                case 3:
                    ExcelService.ChangeSheet(EXCELSHEET_LIST.SHEET_BOTTOM);
                    break;

                default:
                    break;
            }
            
        }

        private void sese_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(Globals.Ribbons.Count().ToString());
        }
    }
}
