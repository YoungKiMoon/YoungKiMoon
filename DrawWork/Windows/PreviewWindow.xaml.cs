using DrawWork.DrawServices;
using DrawWork.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace DrawWork.Windows
{
    /// <summary>
    /// PreviewWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PreviewWindow : Window
    {
        DrawSettingService drawSetting;

        public PreviewWindow()
        {
            InitializeComponent();
            testDraw.Unlock("UF20-LX12S-KRDSL-F0GT-FD74");

            drawSetting = new DrawSettingService();
            drawSetting.SetPaperSpace(testDraw);
        }

        private void btnRefresh_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PreviewWindowViewModel selView = this.DataContext as PreviewWindowViewModel;
            selView.previewService.CreateVectorView(selView.viewPortSet);
            MessageBox.Show("Update");
        }
    }
}
