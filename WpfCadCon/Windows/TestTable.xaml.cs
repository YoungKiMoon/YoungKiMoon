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
    /// TestTable.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TestTable : Window
    {
        public TestTable()
        {
            InitializeComponent();
        }

        private void btnCreateImage_Click(object sender, RoutedEventArgs e)
        {
            Image newImage = new Image();
            //newImage.Source = new BitmapImage(new Uri("/CustomImage/EarthLug.png"));
            newImage.Source = new BitmapImage(new Uri(@"/CustomImage/EarthLug.png", UriKind.Relative));
            newImage.OpacityMask = Brushes.White;
            newImage.Stretch = Stretch.Uniform;
            newImage.Margin = new Thickness(10);

            TextBlock newText = new TextBlock();
            newText.FontSize = 12;
            newText.Foreground = Brushes.Black;
            newText.Text = "asdasdfasdf";
            newText.HorizontalAlignment = HorizontalAlignment.Center;
            newText.VerticalAlignment = VerticalAlignment.Center;

            Border newBorder = new Border();
            newBorder.CornerRadius = new CornerRadius(6);
            newBorder.Background = Brushes.LightBlue;
            newBorder.Height = 20;
            newBorder.Margin = new Thickness(10, 0,10,0);
            newBorder.Child = newText;

            spImage.Children.Add(newImage);
            spImage.Children.Add(newBorder);
        }
    }
}
