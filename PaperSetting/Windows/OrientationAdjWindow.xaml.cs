using devDept.Eyeshot;
using DrawWork.DrawServices;
using PaperSetting.DrawSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PaperSetting.Windows
{
    /// <summary>
    /// OrientationAdjWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OrientationAdjWindow : Window
    {
        private DrawAdj drawService;

        public OrientationAdjWindow()
        {
            InitializeComponent();

            testModel.Unlock("UF20-LX12S-KRDSL-F0GT-FD74");

            testModel.Renderer = rendererType.OpenGL;
            testModel.ActiveViewport.DisplayMode = devDept.Eyeshot.displayType.Wireframe;
            testModel.ActiveViewport.Background.TopColor = new SolidColorBrush(Color.FromRgb(59, 68, 83));

            // Create Setting
            DrawSettingService drawSetting = new DrawSettingService();
            drawSetting.SetModelSpace(testModel);

            drawService = new DrawAdj();
        }

        private void btnRebuild_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bool nozzleValue = false;
            if (cbNozzle.IsChecked == true)
                nozzleValue = true;
            bool structureValue = false;
            if (cbStructure.IsChecked == true)
                structureValue = true;
            drawService.drawAllLayer(testModel, nozzleValue, structureValue);

            testModel.Entities.Regen();
            testModel.Invalidate();


        }

        private void btnStartAdj_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Auto Adj
            for(int i = 0; i < 360; i++)
            {
                Thread.Sleep(400);
                drawService.RotateLayer(testModel);
                drawService.Detection(testModel);
                if (drawService.DetectionPointList.Count == 0)
                    break;
            }
        }

        private void btnDetection_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // On/Off Red Box
            drawService.Detection(testModel);
        }

        private void cbNozzle_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (cbNozzle.IsChecked==false)
            {
                
            }
        }

        private void cbStructure_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (cbNozzle.IsChecked == false)
            {
                
            }
        }

        private void btnRegen_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            testModel.Entities.RegenAllCurved(0.0005);
            testModel.Entities.Regen();
        }
    }
}
