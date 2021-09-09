using DesignBoard.Commons;
using DesignBoard.Models;
using DesignBoard.Services;
using DesignBoard.ViewModels;
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
using System.Windows.Shapes;

namespace DesignBoard.Windows
{
    /// <summary>
    /// DataInputWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DataInputWindow : Window
    {
        #region One Click
        private long _oneClickFirsttime = (long)0;

        public bool One_Click()
        {
            bool flag;
            long ticks = DateTime.Now.Ticks;
            if (ticks - this._oneClickFirsttime >= (long)4000000)
            {
                this._oneClickFirsttime = ticks;
                flag = true;
            }
            else
            {
                this._oneClickFirsttime = ticks;
                flag = false;
            }
            return flag;
        }
        #endregion

        public DataInputWindow()
        {
            InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);

        }


        private void btnClose_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void btnApply_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Data Applied.", "Infomation");
            Close();
        }

        private void btnWaterSpray_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (One_Click())
            {
                FileService newFileService = new FileService();
                FileModel selFile = newFileService.GetFile(OpenFile_Type.WaterSpray);

            }
        }

        private void btnFoamSystem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (One_Click())
            {
                FileService newFileService = new FileService();
                FileModel selFile = newFileService.GetFile(OpenFile_Type.FoamSystem);

            }
        }

        public void SetData(ObservableCollection<DataListModel> DataList, List<ImageModel> imageList)
        {
            DataInputWindowViewModel selView = this.DataContext as DataInputWindowViewModel;
            selView.DataList = DataList;
            selView.ImageList = imageList;
            selView.RefreshGroupList();
        }


        private void groupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItemModel selItem = groupList.SelectedItem as ListBoxItemModel;

            if (selItem != null)
            {
                DataInputWindowViewModel selView = this.DataContext as DataInputWindowViewModel;
                selView.ChangeCurrentList(selItem.Name);

                // image
                imageArea.Children.Clear();
                if (selItem.Name == "ROOF")
                    AddImage(selView.ImageList);
            }
        }

        public void AddImage(List<ImageModel> selImageList)
        {

            foreach (ImageModel eachImage in selImageList)
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
                else if (newImage.Height > 400)
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
    }
}
