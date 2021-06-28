using DesignBoard.Commons;
using DesignBoard.Services;
using DesignBoard.Utils;
using DesignBoard.ViewModels;
using DesignBoard.Windows;
using Microsoft.Win32;
using PaperSetting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesignBoard
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Custom Cursor
        public CustomCursor currentCursor;
        #endregion

        #region Busy Indicator
        private BackgroundWorker BusyWorker = new BackgroundWorker();
        private int BusyAngle = 0;
        private bool IsBusy = false;
        #endregion

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

        public MainWindow()
        {
            InitializeComponent();
            busyAreaSource.Visibility = Visibility.Collapsed;
        }


        #region Window Resizer, Close
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void btnClose_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        #endregion


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        #region Button
        private void btnAME_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (One_Click())
            {

                FileService newFileService = new FileService();
                string selFile = newFileService.GetFile(OpenFile_Type.AMETankData);

                OpenAMEDataCheckWindow();
                MainWindowViewModel selView = this.DataContext as MainWindowViewModel;
                selView.checkList.AMETankCheck.checkColor = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                selView.checkList.DataCheck.checkColor = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            }
        }
        private void btnAMECheck_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (One_Click())
            {
                OpenAMEDataCheckWindow(true);
                MainWindowViewModel selView = this.DataContext as MainWindowViewModel;
                selView.checkList.AMETankCheck.checkColor = new SolidColorBrush(Color.FromArgb(255, 132, 151, 176));
            }

        }



        private void btnExisting_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (One_Click())
            {
                FileService newFileService = new FileService();
                string selFile = newFileService.GetFile(OpenFile_Type.EngData);
                
            }
        }
        private void btnDataInput_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (One_Click())
                OpenDataInputWindow();
        }

        private void btnNozzleInput_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (One_Click())
                OpenNozzleInputWindow();
        }

        private void btnDataCheck_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (One_Click())
                OpenDataCheckWindow();
        }

        private void btnPreview_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (One_Click())
                OpenDrawingListWindow();
        }
        #endregion


        private void OpenAMEDataCheckWindow(bool recheck = false)
        {
            MainWindowViewModel selView = this.DataContext as MainWindowViewModel;

            AMEDataCheckWindow newWindow = new AMEDataCheckWindow();
            if (recheck)
                newWindow.autoRecheckError = true;
            newWindow.SetAMEData(selView.AMEList);
            newWindow.ShowDialog();
        }

        private void OpenDataInputWindow()
        {
            MainWindowViewModel selView = this.DataContext as MainWindowViewModel;

            DataInputWindow newWindow = new DataInputWindow();

            newWindow.SetData(selView.DataList, selView.ImageList);
            newWindow.ShowDialog();
        }

        private void OpenNozzleInputWindow()
        {
            MainWindowViewModel selView = this.DataContext as MainWindowViewModel;

            NozzleInputWindow newWindow = new NozzleInputWindow();

            newWindow.SetData(selView.NozzleList);
            newWindow.ShowDialog();
        }

        private void OpenDataCheckWindow()
        {
            MainWindowViewModel selView = this.DataContext as MainWindowViewModel;

            DataCheckWindow newWindow = new DataCheckWindow();

            newWindow.SetData(selView.DataCheckList);
            newWindow.ShowDialog();
            selView.checkList.DataCheck.checkColor = new SolidColorBrush(Color.FromArgb(255, 132, 151, 176));
        }

        private void OpenDrawingListWindow()
        {
            PaperSettingWindow newWindow = new PaperSettingWindow();
            newWindow.ShowDialog();
        }





        #region Busy : Source
        private void BusyStartSource(bool selBusy)
        {
            CustomCursor selCursor = new CustomCursor();
            selCursor.WaitCursor();

            IsBusy = selBusy;
            BusyAngle = 0;

            int imageMin = 1;
            int imageCurrent = imageMin;
            int imageMax = 90;

            int ringMin = 1;
            int ringCurrent = ringMin;
            int ringMax = 3;

            busyAreaSource.Visibility = Visibility.Visible;

            BusyWorker = new BackgroundWorker();
            BusyWorker.WorkerReportsProgress = true;
            BusyWorker.WorkerSupportsCancellation = true;
            BusyWorker.ProgressChanged += (ssender, ee) =>
            {
                //BusyAngle += 30;
                //if (BusyAngle == 360)
                //    BusyAngle = 0;
                //RotateTransform rt = new RotateTransform();
                //rt.Angle = BusyAngle;
                //rt.CenterX = 50;
                //rt.CenterY = 50;

                //busyImageSource.RenderTransform = rt;
                string imageString = @"/DesignBoard;component/BusyImage/" + imageCurrent + ".png";
                busyImageSource.Source=new BitmapImage(new Uri(@imageString, UriKind.Relative));

                imageCurrent++;
                if (imageMax == imageCurrent)
                    imageCurrent = imageMin;

                //string imageRing = @"/DesignBoard;component/BusyImage/raser_bottom_light0" + ringCurrent + ".png";
                //busysdRing.Source = new BitmapImage(new Uri(imageRing, UriKind.Relative));
                //ringCurrent++;
                //if (ringMax == ringCurrent)
                //    ringCurrent = ringMin;

            };
            BusyWorker.RunWorkerCompleted += (ssender, ee) =>
            {
                busyAreaSource.Visibility = Visibility.Hidden;
                selCursor.Dispose();
            };
            BusyWorker.DoWork += (ssender, ee) =>
            {
                while (IsBusy)
                {
                    BusyWorker.ReportProgress(1);
                    Thread.Sleep(80);
                }
            };
            BusyWorker.RunWorkerAsync();
        }
        #endregion

        private void btnNew_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            BusyStartSource(true);
        }
    }
}
