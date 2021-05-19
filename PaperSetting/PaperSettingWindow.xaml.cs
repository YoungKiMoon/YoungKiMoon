using PaperSetting.ViewModel;
using PaperSetting.EYEServices;

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
using PaperSetting.Models;
using DrawWork.Windows;
using DrawWork.ViewModels;
using DrawWork.DrawServices;
using AssemblyLib.AssemblyModels;
using DrawLogicLib.DrawLogicFileServices;
using DrawWork.DesignServices;

namespace PaperSetting
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PaperSettingWindow : Window
    {
        public PaperSettingWindow()
        {
            

            InitializeComponent();
            testDraw.Unlock("UF20-LX12S-KRDSL-F0GT-FD74");
            testModel.Unlock("UF20-LX12S-KRDSL-F0GT-FD74");

            testModel.ActiveViewport.DisplayMode = devDept.Eyeshot.displayType.Wireframe;
            testModel.ActiveViewport.Background.TopColor = new SolidColorBrush(Color.FromRgb(59, 68, 83));


            // Create Setting
            DrawSettingService drawSetting = new DrawSettingService();
            drawSetting.SetModelSpace(testModel);
            drawSetting.SetPaperSpace(testDraw);

            testDraw.PaperColor = new SolidColorBrush(Color.FromRgb(59,68,83));

            // Create Block
            PaperBlockService drawBlock = new PaperBlockService(testDraw);
            drawBlock.CreatePaperBlock();

            //ModelDrawService modelService = new ModelDrawService(this.testModel);
            //modelService.CreateSample();



        }


        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if(e.Source is TabControl)
            {
                int tabIndex = (sender as TabControl).SelectedIndex;
                //TabSelectionEvent(tabIndex);
            }
        }
        private void TabSelectionEvent(int tabIndex)
        {
            switch (tabIndex)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    PaperSettingViewModel selView = this.DataContext as PaperSettingViewModel;
                    PaperDrawService paperService = new PaperDrawService(this.testModel, this.testDraw);
                    paperService.CreatePaperDraw(selView.PaperListSelectionColl);
                    break;
                default:
                    break;
            }
        }



        private void Button_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //SampleDraw();
            CreateDraw();
        }

        public void CreateDraw()
        {
            //Assembly
            AssemblyModel newTankData = new AssemblyModel();
            newTankData.CreateSampleAssembly();

            // Logic
            DrawLogicDBService newLogic = new DrawLogicDBService();

            IntergrationService newInterService = new IntergrationService("CRT", newTankData, testModel);
            string[] newLogicData = newLogic.GetLogicFile(DrawLogicLib.Commons.LogicFile_Type.GA);
            //string[] ccccc = null;
            if (newInterService.CreateLogic(90, newLogicData))
            {
                MessageBox.Show("완료");
            }
            else
            {
                MessageBox.Show("오류");
            }
            SampleDraw();
        }


        #region Sample Draw
        public void SampleDraw()
        {
            PaperSettingViewModel selView = this.DataContext as PaperSettingViewModel;
            PaperDrawService paperService = new PaperDrawService(this.testModel, this.testDraw);
            paperService.CreatePaperDraw(selView.PaperList);
            //paperService.CreatePaperDraw(selView.PaperListSelectionColl);
        }
        #endregion

        #region Create DWG
        private void btnExport_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            //testDraw.Blocks.Clear();
            //testDraw.Sheets.Clear();
            //PaperSettingViewModel selView = this.DataContext as PaperSettingViewModel;
            //PaperDrawService paperService = new PaperDrawService(this.testModel, this.testDraw);
            //paperService.CreatePaperDraw(selView.PaperList,false);

            EYECADService newExport = new EYECADService();
            newExport.TestExport(this.testModel, this.testDraw);
        }
        #endregion

        #region Drawing List
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataPaperList.SelectedIndex >= 0)
            {
                if (testDraw.Sheets.Count > 0)
                {
                    if (testDraw.Sheets.Count > dataPaperList.SelectedIndex)
                    {
                        testDraw.ActiveSheet = testDraw.Sheets[dataPaperList.SelectedIndex];
                        testDraw.ZoomFit();
                        testDraw.Invalidate();
                    }

                }
            }
                
        }
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            PaperSettingViewModel selView = this.DataContext as PaperSettingViewModel;
            bool flag = false;
            foreach (PaperDwgModel row in selView.PaperList)
            {
                if(!row.Basic.View)
                    flag = true;
            }
            foreach (PaperDwgModel row in selView.PaperList)
            {
                row.Basic.View = flag;
            }
            (sender as CheckBox).IsChecked = new bool?(flag);
        }
        #endregion


        #region Button : Option
        private void btnPaperOption_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }



        private void btnEnvironment_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            EnvironmentWindow newWin = new EnvironmentWindow();
            EnvironmentWindowViewModel newWinView = newWin.DataContext as EnvironmentWindowViewModel;
            newWinView.SetModelEnvironment(testModel);
            newWinView.CreateEnvironment();
            newWin.Owner = this;
            newWin.Show();
        }

        private void btnTitleBlock_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TitleBlockWindow cc = new TitleBlockWindow();
            cc.Owner = this;
            cc.ShowDialog();
        }
        #endregion
    }
}
