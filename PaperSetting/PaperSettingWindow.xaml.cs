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
using DrawWork.AssemblyServices;
using DrawLogicLib.Models;
using DrawWork.FileServices;
using DrawWork.Commons;
using PaperSetting.Services;
using DrawWork.DrawSacleServices;
using ExcelDataLib.ExcelServices;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;
using devDept.Graphics;
using devDept.Eyeshot.Translators;
using DrawWork.DWGFileServices;
using Microsoft.Win32;
using DrawWork.ImportServices;

namespace PaperSetting
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PaperSettingWindow : Window
    {
        public string workbookName = "";
        public object activeCustomWorkbook = null;
        public PaperSettingWindow()
        {
            

            InitializeComponent();

            tabDetail.SelectedItem = viewview;

            testDraw.Unlock("UF20-LX12S-KRDSL-F0GT-FD74");
            testModel.Unlock("UF20-LX12S-KRDSL-F0GT-FD74");

            testModel.ActiveViewport.DisplayMode = devDept.Eyeshot.displayType.Wireframe;
            testModel.ActiveViewport.Background.TopColor = new SolidColorBrush(Color.FromRgb(59, 68, 83));


            // Create Setting
            DrawSettingService drawSetting = new DrawSettingService();
            drawSetting.SetModelSpace(testModel);
            drawSetting.SetPaperSpace(testDraw);

            testDraw.PaperColor = new SolidColorBrush(Color.FromRgb(59,68,83));


            workbookName = "TankDesign_0527_1.xlsm";
            workbookName = "TEST 2019-3.xlsm";
            workbookName = "TEST-002.xlsm";
            workbookName = "TEST 2019-1.xlsm";
            //workbookName = "TankDesign_TEST-2.xlsm";


            // Create Block
            PaperBlockService drawBlock = new PaperBlockService(testDraw);
            drawBlock.CreatePaperBlock();

            //ModelDrawService modelService = new ModelDrawService(this.testModel);
            //modelService.CreateSample();


            


        }


        private void Window_ContentRendered(object sender, EventArgs e)
        {
            ExcelApplicationService eDataService = new ExcelApplicationService(-1);
            if (activeCustomWorkbook != null)
            {
                if (eDataService.CheckTABASExcel2(activeCustomWorkbook))
                {
                    // 자동 실행
                    SetAssemblyData();
                }
                else
                {
                    MessageBox.Show("TABAS DESIGN FILE이 아닙니다.", "안내");
                    this.Close();
                }
            }
            else
            {
                // 자동 실행
                SetAssemblyData();
            }

            


        }

        private void Button_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //SampleDraw();
            //CreateDraw();
            //CreateDrawSample();
            CreateDraw1st();

            //SampleDraw();
        }


        public void SetAssemblyData()
        {
            PaperSettingViewModel selView = this.DataContext as PaperSettingViewModel;
            // Assembly

            AssemblyDataService assemblyService = new AssemblyDataService();
            AssemblyModel newTankData = assemblyService.CreateMappingData(workbookName,activeCustomWorkbook);
            //selView.newTankData = newTankData;

            PaperSettingService newSetting = new PaperSettingService();
            selView.PaperList = newSetting.CreateDrawingCRTList(newTankData);
            //selView.PaperList = newSetting.CreateDrawingCRTList();
            //selView.CreateDrawingList(newTankData);
        }


        public void CreateDraw1st()
        {
            PaperSettingViewModel selView = this.DataContext as PaperSettingViewModel;

            //selView.BContents = "TANK 를 생성 중입니다.";
            //BackgroundWorker bgWorker = new BackgroundWorker();

            //bgWorker.WorkerReportsProgress = true;
            //bgWorker.ProgressChanged += (ssender, ee) =>
            //{
            //    if (ee.ProgressPercentage == 99999)
            //    {
            //        customBusyIndicator.IsBusy = false;
            //    }
            //    else
            //    {

            //    }

            //};
            //bgWorker.RunWorkerCompleted += (ssender, ee) =>
            //{
            //    customBusyIndicator.IsBusy = false;
            //};
            //bgWorker.DoWork += (ssender, ee) =>
            //{



            //    try
            //    {



            // SingletonData Reset
            SingletonData.LeaderPublicList.Clear();
            SingletonData.DimPublicList.Clear();

            // Assembly
            AssemblyDataService assemblyService = new AssemblyDataService();
            AssemblyModel newTankData = assemblyService.CreateMappingData(workbookName, activeCustomWorkbook);

            // Logic
            DrawLogicDBService newLogic = new DrawLogicDBService();
            DrawLogicModel newLogicData = newLogic.GetLogicCommand(DrawLogicLib.Commons.LogicFile_Type.GA);


            //DWG Load
            //DWGFileService dwgService = new DWGFileService();
            //string dwgFilePath = dwgService.FileFilecopy();
            //if (dwgFilePath != "")
            //{
            //    BackgroundSettings temp = new BackgroundSettings();
            //    temp.TopColor = new SolidColorBrush(Color.FromRgb(225, 225, 225));
            //    testModel.Viewports[0].Background = temp;

            //    //ReadFileAsync rfa = GetReader(importFileDialog.FileName);
            //    //if (rfa != null)
            //    //    testModel.StartWork(rfa);

            //    Thread.Sleep(100);
            //    ReadAutodesk ra = new ReadAutodesk(dwgFilePath);
            //    if (ra != null)
            //    {
            //        testModel.StartWork(ra);
            //        testModel.Refresh();
            //    }
            //    this.Dispatcher.Invoke((ThreadStart)(() => { }), DispatcherPriority.ApplicationIdle);

            //    Thread.Sleep(1000);
            //}


            // Ligic File
            //TextFileService newFileService = new TextFileService();
            //string[] newComData = newFileService.GetTextFileArray(logicFile.Text);

            // Virtual Design
            //DesignService designS = new DesignService();
            //designS.CreateDesignCRTModel(newTankData);

            // 시트 값을 가져와야 함 : Assembly.Create Mapping Data

            // 버추얼 디자인의 True False

            // Logic을 수동으로 연결

            // 형상 그리는 것은 완료
            List<string> newAll = new List<string>();
            newAll.AddRange(newLogicData.UsingList);
            newAll.AddRange(newLogicData.ReferencePointList);
            foreach (DrawCommandModel eachCommand in newLogicData.CommandList)
                newAll.AddRange(eachCommand.Command);

            //newAll.ToArray()
            DrawScaleService scaleService = new DrawScaleService();
            double autoScale = scaleService.GetAIScale(newTankData);


            IntergrationService newInterService = new IntergrationService("CRT", newTankData, testModel);
            if (newInterService.CreateLogic(autoScale, newAll.ToArray()))
            {

                //bgWorker.ReportProgress(99999);
                MessageBox.Show("생성 완료", "안내");

            }
            else
            {
                //bgWorker.ReportProgress(88888);
                MessageBox.Show("생성 실패", "안내");
            }
            SampleDraw(newTankData, autoScale);


            //    }




            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex);
            //        bgWorker.ReportProgress(88888);
            //    }

            //};
            //bgWorker.RunWorkerAsync();








            //// SingletonData Reset
            //SingletonData.LeaderPublicList.Clear();
            //SingletonData.DimPublicList.Clear();

            //// Assembly
            //AssemblyDataService assemblyService = new AssemblyDataService();
            //AssemblyModel newTankData = assemblyService.CreateMappingData(workbookName,activeCustomWorkbook);

            //// Logic
            //DrawLogicDBService newLogic = new DrawLogicDBService();
            //DrawLogicModel newLogicData = newLogic.GetLogicCommand(DrawLogicLib.Commons.LogicFile_Type.GA);

            //// Ligic File
            ////TextFileService newFileService = new TextFileService();
            ////string[] newComData = newFileService.GetTextFileArray(logicFile.Text);

            //// Virtual Design
            ////DesignService designS = new DesignService();
            ////designS.CreateDesignCRTModel(newTankData);

            //// 시트 값을 가져와야 함 : Assembly.Create Mapping Data

            //// 버추얼 디자인의 True False

            //// Logic을 수동으로 연결

            //// 형상 그리는 것은 완료
            //List<string> newAll = new List<string>();
            //newAll.AddRange(newLogicData.UsingList);
            //newAll.AddRange(newLogicData.ReferencePointList);
            //foreach (DrawCommandModel eachCommand in newLogicData.CommandList)
            //    newAll.AddRange(eachCommand.Command);

            ////newAll.ToArray()
            //DrawScaleService scaleService = new DrawScaleService();
            //double autoScale = scaleService.GetAIScale(newTankData);

            //IntergrationService newInterService = new IntergrationService("CRT", newTankData, testModel);
            //if (newInterService.CreateLogic(autoScale, newAll.ToArray()))
            //{
            //    MessageBox.Show("완료");
            //}
            //else
            //{
            //    //MessageBox.Show("오류");
            //}
            //SampleDraw(newTankData);
        }





        #region Sample Draw
        public void SampleDraw(AssemblyModel assemblyData,double scaleValue)
        {
            PaperSettingViewModel selView = this.DataContext as PaperSettingViewModel;
            PaperDrawService paperService = new PaperDrawService(this.testModel, this.testDraw);
            //paperService.assemblyData = assemblyData;
            paperService.CreatePaperDraw(selView.PaperList,assemblyData, scaleValue);
            //paperService.CreatePaperDraw(selView.PaperListSelectionColl);
        }
        #endregion

        #region Tab Control
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if(e.Source is TabControl)
            {
                int tabIndex = (sender as TabControl).SelectedIndex;
                //TabSelectionEvent(tabIndex);
                if (tabIndex == 4)
                {
                    this.Dispatcher.Invoke((ThreadStart)(() => { }), DispatcherPriority.ApplicationIdle);
                    Thread.Sleep(100);
                    delayFunction();
                }




            }
        }

        private void delayFunction()
        {
            if (dataPaperList.SelectedIndex >= 0)
            {
                if (testDraw.Sheets.Count > 0)
                {
                    if (testDraw.Sheets.Count > dataPaperList.SelectedIndex)
                    {
                        testDraw.Invalidate();
                        int selIndex = dataPaperList.SelectedIndex;
                        testDraw.ActiveSheet = testDraw.Sheets[selIndex];
                        testDraw.ZoomFit();
                        testDraw.Invalidate();
                        testDraw.Refresh();
                        testDraw.ActiveSheet = testDraw.Sheets[selIndex];
                        
                    }

                }
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
                    //paperService.CreatePaperDraw(selView.PaperListSelectionColl);
                    break;
                default:
                    break;
            }
        }
        #endregion




        //public void CreateDraw()
        //{
        //    //Assembly
        //    AssemblyModel newTankData = new AssemblyModel();
        //    newTankData.CreateSampleAssembly();

        //    // Logic
        //    DrawLogicDBService newLogic = new DrawLogicDBService();

        //    IntergrationService newInterService = new IntergrationService("CRT", newTankData, testModel);
        //    string[] newLogicData = newLogic.GetLogicFile(DrawLogicLib.Commons.LogicFile_Type.GA);
        //    //string[] ccccc = null;
        //    if (newInterService.CreateLogic(90, newLogicData))
        //    {
        //        MessageBox.Show("완료");
        //    }
        //    else
        //    {
        //        MessageBox.Show("오류");
        //    }
        //    SampleDraw();
        //}


        //public void CreateDrawSample()
        //{
        //    // Assembly
        //    AssemblyDataService assemblyService = new AssemblyDataService();
        //    AssemblyModel newTankData = assemblyService.CreateMappingData("TankDesign_0520_TEST.xlsm");

        //    // Logic
        //    DrawLogicDBService newLogic = new DrawLogicDBService();
        //    DrawLogicModel newLogicData = newLogic.GetLogicCommand(DrawLogicLib.Commons.LogicFile_Type.GA);

        //    // Ligic File
        //    TextFileService newFileService = new TextFileService();
        //    string[] newComData = newFileService.GetTextFileArray(@"C:\Users\tree\Desktop\CAD\tabas\Sample_DrawLogic.txt");

        //    // Virtual Design
        //    //DesignService designS = new DesignService();
        //    //designS.CreateDesignCRTModel(newTankData);

        //    // 시트 값을 가져와야 함 : Assembly.Create Mapping Data

        //    // 버추얼 디자인의 True False

        //    // Logic을 수동으로 연결

        //    // 형상 그리는 것은 완료
        //    List<string> newAll = new List<string>();
        //    newAll.AddRange(newLogicData.UsingList);
        //    newAll.AddRange(newLogicData.ReferencePointList);
        //    foreach (DrawCommandModel eachCommand in newLogicData.CommandList)
        //        newAll.AddRange(eachCommand.Command);

        //    //newAll.ToArray()

        //    IntergrationService newInterService = new IntergrationService("CRT", newTankData, testModel);
        //    if (newInterService.CreateLogic(90, newComData))
        //    {
        //        MessageBox.Show("완료");
        //    }
        //    else
        //    {
        //        //MessageBox.Show("오류");
        //    }
        //    SampleDraw();
        //}





        #region Create DWG
        private void btnExport_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            //testDraw.Blocks.Clear();
            //testDraw.Sheets.Clear();
            //PaperSettingViewModel selView = this.DataContext as PaperSettingViewModel;
            //PaperDrawService paperService = new PaperDrawService(this.testModel, this.testDraw);
            //paperService.CreatePaperDraw(selView.PaperList,false);try{try{try{


            try
            {
                EYECADService newExport = new EYECADService();
                newExport.TestExport2(this.testModel, this.testDraw);

                MessageBox.Show("도면 생성 완료","안내");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "에러");
            }

            
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
                        int selIndex = dataPaperList.SelectedIndex;
                        testDraw.ActiveSheet = testDraw.Sheets[selIndex];
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

        private void tabDetail_LayoutUpdated(object sender, EventArgs e)
        {
            if(sender!=null)
                MessageBox.Show("sss");
        }

        private void tabDetail_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            MessageBox.Show("sstttts");
        }

        private void btnBlock_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            var importFileDialog = new OpenFileDialog();
            importFileDialog.Filter = "All compatible file types (*.*)|*.step;*.stp;*.iges;*.igs;*.stl;*.obj;*.dwg;*.dxf|Standard for the Exchange of Product Data (*.stp; *.step)|*.stp; *.step|Initial Graphics Exchange Specification (*.igs; *.iges)|*.igs; *.iges|WaveFront OBJ (*.obj)|*.obj|Stereolithography (*.stl)|*.stl|CAD drawings (*.dwg)|*.dwg|Drawing Exchange Format (*.dxf)|*.dxf";
            importFileDialog.AddExtension = true;
            importFileDialog.Title = "Import";
            importFileDialog.CheckFileExists = true;
            importFileDialog.CheckPathExists = true;
            var result = importFileDialog.ShowDialog();
            if (result == true)
            {
                //testModel.Clear();

                //BackgroundSettings temp = new BackgroundSettings();
                //temp.TopColor = new SolidColorBrush(Color.FromRgb(225, 225, 225));
                //testModel.Viewports[0].Background = temp;

                //ReadFileAsync rfa = GetReader(importFileDialog.FileName);
                //if (rfa != null)
                //    testModel.StartWork(rfa);

                ReadAutodesk ra = new ReadAutodesk(importFileDialog.FileName);
                if (ra != null)
                {
                    testModel.StartWork(ra);
                    testModel.Refresh();
                }

                
            }
        }

        private void testModel_WorkCompleted(object sender, devDept.Eyeshot.WorkCompletedEventArgs e)
        {
            
                if (e.WorkUnit is ReadFileAsync)
                {
                    ReadFileAsync rfa = (ReadFileAsync)e.WorkUnit;

                    //ReadFile rf = e.WorkUnit as ReadFile;

                    //ReadFile rf = e.WorkUnit as ReadFile;
                    //if (rf != null)
                    //if(rfa.Entities !=null)

                    // ReadFile rf = new ReadFile(rfa.Stream);

                    if (e.WorkUnit is ReadFileAsyncWithBlocks)
                    {
                        ReadFileAsyncWithBlocks readFileWithBlocks = (ReadFileAsyncWithBlocks)e.WorkUnit;

                        ImportBlockService importBlockS = new ImportBlockService();
                        importBlockS.CreateBlock(readFileWithBlocks, testModel);

                    //devDept.Eyeshot.Block ddd= readFileWithBlocks.Blocks["block_Sample"]);
                    //testModel.Blocks.Add(readFileWithBlocks.Blocks["ssblock"]);

                    //var dd= testModel.Blocks["ssblock"];

                    //foreach(Entity eachEntity in dd.Entities)
                    //{
                    //eachEntity is LinearPath
                    //eachEntity.LayerName = "DashDot1";
                    //eachEntity.LineTypeName = "";
                    //eachEntity.LineTypeMethod = colorMethodType.byLayer;
                    //eachEntity.LayerName = "DashDot";
                    //eachEntity.LineTypeName = "DashDot";
                    //}
                    MessageBox.Show("Block Loading Complete.");
                    }

                

                    //var br3 = new BlockReference(10, 100, 10, "ssblock",testModel.RootBlock.Units,testModel.Blocks,0);

                // 블럭 삽입 방법
                //if (false)
                //{
                //    var br3 = new BlockReference(-1000, 5000, 0, "LADDER-1", 0);
                //    br3.Scale(1);
                //    testModel.Entities.Add(br3, "LayerDimension");
                //}

                //testModel.Entities.Add(br3, "DashDot");

                //                rfa.AddToScene(testModel);
            }
            
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            ReadAutodesk.OnApplicationExit(sender,e);
        }
    }
}
