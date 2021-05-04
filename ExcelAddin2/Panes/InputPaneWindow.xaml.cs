using ExcelAddIn.Models;
using ExcelAddIn.PanesViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ExcelAddIn.Panes
{
    /// <summary>
    /// InputPaneWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InputPaneWindow : UserControl
    {
        public InputPaneWindow()
        {
            InitializeComponent();
        }

        public void AddImage(List<ImageModel> selImageList)
        {
            imageArea.Children.Clear();

            foreach(ImageModel eachImage in selImageList)
            {
                Image newImage = new Image();

                newImage.Source = new BitmapImage(new Uri(@eachImage.ImagePath, UriKind.Relative));
                //newImage.OpacityMask = Brushes.White;
                newImage.Stretch = Stretch.Uniform;
                newImage.Margin = new Thickness(10);

                if (newImage.Width > 400)
                {
                    newImage.Width = 400;
                }
                else if(newImage.Height> 400)
                {
                    newImage.Height = 400;
                }


                TextBlock newText = new TextBlock();
                newText.FontSize = 12;
                newText.Foreground = Brushes.Black;
                newText.Text = eachImage.ImageName;
                newText.HorizontalAlignment = HorizontalAlignment.Center;
                newText.VerticalAlignment = VerticalAlignment.Center;

                Border newBorder = new Border();
                newBorder.CornerRadius = new CornerRadius(6);
                newBorder.Background = Brushes.LightBlue;
                newBorder.Height = 20;
                newBorder.Margin = new Thickness(10, 0, 10, 0);
                newBorder.Child = newText;

                imageArea.Children.Add(newImage);
                imageArea.Children.Add(newBorder);
            }
        }

        public void AddTable(ObservableCollection<TableModel> selList)
        {
            InputPaneWindowViewModel selView = this.DataContext as InputPaneWindowViewModel;
            selView.TableData = selList;
            
                    }
    }
}
