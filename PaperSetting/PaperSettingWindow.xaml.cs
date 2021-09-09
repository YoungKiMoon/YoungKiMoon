using PaperSetting.ViewModels;
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
using PaperSetting.Utils;
using DrawWork.DrawBuilders;
using devDept.Eyeshot;
using System.Diagnostics;
using PaperSetting.Windows;
using DrawSettingLib.SettingServices;
using System.Collections.ObjectModel;
using DrawWork.DrawGridServices;
using System.IO;
using DrawWork.DrawDetailServices;
using EPDataLib.ExcelServices;

namespace PaperSetting
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PaperSettingWindow : Window
    {

        #region Custom Cursor
        public CustomCursor currentCursor;
        #endregion

        #region Busy Indicator
        private BackgroundWorker BusyWorker = new BackgroundWorker();
        private int BusyAngle = 0;
        private bool IsBusy = false;
        #endregion

        public double progressCustomi=0;

        public string workbookName = "";
        public object activeCustomWorkbook = null;

        private BlockKeyedCollection tempImportBlocks;
        private BlockKeyedCollection tempImportOrientationBlocks;

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

        public PaperSettingWindow()
        {
            

            InitializeComponent();

            gridLoading.Visibility = Visibility.Visible;

            busyAreaSource.Visibility = Visibility.Hidden;

            tabDetail.SelectedItem = tabletable;

            testDraw.Unlock("UF20-LX12S-KRDSL-F0GT-FD74");
            testModel.Unlock("UF20-LX12S-KRDSL-F0GT-FD74");

            testModel.Renderer = rendererType.OpenGL;
            testModel.ActiveViewport.DisplayMode = devDept.Eyeshot.displayType.Wireframe;
            testModel.ActiveViewport.Background.TopColor = new SolidColorBrush(Color.FromRgb(59, 68, 83));

            testModel.ProgressBar.Visible = true;

            // Create Setting
            DrawSettingService drawSetting = new DrawSettingService();
            drawSetting.SetModelSpace(testModel);
            drawSetting.SetPaperSpace(testDraw);

            testDraw.PaperColor = new SolidColorBrush(Color.FromRgb(59,68,83));


            //workbookName = "TankDesign_0527_1.xlsm";
            //workbookName = "TEST 2019-3.xlsm";
            //workbookName = "TEST-002.xlsm";
            //workbookName = "TEST 2019-1.xlsm";
            //workbookName = "TankDesign_0527-2.xlsm";
            //workbookName = "TankDesign_0528.xlsm";
            //workbookName = "TankDesign_0528-1.xlsm";
            //workbookName = "DRT TEST-1.xlsm";
            //workbookName = "DRT TEST_20210610.xlsm";
            //workbookName = "1.CRT.xlsm";
            //workbookName = "TABAS_20210614+1.xlsm";




            if(workbookName=="")
                workbookName = @"C:\Users\tree\Desktop\CAD\TABAS\20210831 제출\TABAS_20210827-CRT.xlsm";





            //workbookName = "1.CRT_Test.xlsm";
            //workbookName = "TABAS_20210614_blank_CRT.xlsm";
            //workbookName = "TABAS_20210614_data_CRT.xlsm";
            //workbookName = "TABAS_20210614_blank_DRT.xlsm";

            //workbookName = "2.DRT.xlsm";
            //workbookName = "3.IFRT.xlsm";
            //workbookName = "4.FRT_Single.xlsm";
            //workbookName = "5.FRT_Double.xlsm";


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
                    //testBackStart();
                    //SetAssemblyData();
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
                //testBackStart();
                //SetAssemblyData();
            }

            gridLoading.Visibility = Visibility.Collapsed;


        }


        // 실행 버튼
        private void Button_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //SampleDraw();
            //CreateDraw();
            //CreateDrawSample();
            if (One_Click())
            {
                PaperSettingViewModel selView = this.DataContext as PaperSettingViewModel;
                SingletonData.PaperGridVisible = selView.DWGGridVisible;
                //tabDetail.SelectedItem = viewview;
                tabDetail.SelectedItem = previewpreview;


                testDraw.ActiveSheet = null;
                testDraw.Refresh();
                DoEvents();
                //BusyStartSource(true);
                //CreateDraw1st
                testBack();
                
            }

            //SampleDraw();
        }


        //public void testBackStart()
        //{
        //    PaperSettingViewModel selView = this.DataContext as PaperSettingViewModel;
        //    selView.BContents = "TANK DATA 준비 중입니다.";
        //    customBusyIndicator.IsBusy = true;


        //    BackgroundWorker bgWorker = new BackgroundWorker();

        //    bgWorker.WorkerReportsProgress = true;
        //    bgWorker.ProgressChanged += (ssender, ee) =>
        //    {
        //        if (ee.ProgressPercentage == 99999)
        //        {
        //            customBusyIndicator.IsBusy = false;
        //            IsBusy = false;
        //        }
        //        else
        //        {

        //        }

        //    };
        //    bgWorker.RunWorkerCompleted += (ssender, ee) =>
        //    {
        //        //PaperSettingViewModel selView = this.DataContext as PaperSettingViewModel;
        //        DrawScaleService scaleService = new DrawScaleService();
        //        double autoScale = scaleService.GetAIScale(selView.newTankData);
        //        SampleDraw(selView.newTankData);

        //        customBusyIndicator.IsBusy = false;
        //        IsBusy = false;



        //        gridLoading.Visibility = Visibility.Collapsed;
        //    };
        //    bgWorker.DoWork += (ssender, ee) =>
        //    {



        //        try
        //        {
        //            //DoEvents();
        //            //Thread.Sleep(1000);
        //            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
        //            {
        //                //your code
        //                //DoEvents();
        //                SetAssemblyData();
        //            }));

        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    };
        //    bgWorker.RunWorkerAsync();
        //}


        public void testBack()
        {
            PaperSettingViewModel selView = this.DataContext as PaperSettingViewModel;
            selView.BContents = "TANK 를 생성 중입니다.";
            customBusyIndicator.IsBusy = true;


            BackgroundWorker bgWorker = new BackgroundWorker();

            bgWorker.WorkerReportsProgress = true;
            bgWorker.ProgressChanged += (ssender, ee) =>
            {
                if (ee.ProgressPercentage == 99999)
                {
                    customBusyIndicator.IsBusy = false;
                    IsBusy = false;
                }
                else
                {

                }

            };
            bgWorker.RunWorkerCompleted += (ssender, ee) =>
            {
                SampleDraw();
                customBusyIndicator.IsBusy = false;
                IsBusy = false;
            };
            bgWorker.DoWork += (ssender, ee) =>
            {



                try
                {
                    //DoEvents();
                    Thread.Sleep(1000);
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        //your code
                        //DoEvents();
                        CreateDraw1st();


                        //SetAssemblyDataSub();
                        Console.WriteLine("완료");
                    }));

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            };
            bgWorker.RunWorkerAsync();

        }

        public void SetAssemblyData()
        {
            PaperSettingViewModel selView = this.DataContext as PaperSettingViewModel;
            // Assembly
            
            AssemblyDataService assemblyService = new AssemblyDataService();
            AssemblyModel newTankData = assemblyService.CreateMappingDataNew(workbookName,activeCustomWorkbook);
            //selView.newTankData = newTankData;

            selView.newTankData = newTankData;

            SetAssemblyDataSub();
        }

        public void SetAssemblyDataSub()
        {
            PaperSettingViewModel selView = this.DataContext as PaperSettingViewModel;
            PaperSettingService newSetting = new PaperSettingService();
            selView.PaperListSelection = new PaperDwgModel();
            selView.PaperList=newSetting.CreateDrawingCRTList(selView.newTankData,testModel);
            selView.PaperGridList= newSetting.CreatePaperGridList(selView.PaperList);
            
        }


        public void CreateDraw1st()
        {

            bool visibleValue = false;

            // SingletonData Reset
            SingletonData.LeaderPublicList.Clear();
            SingletonData.DimPublicList.Clear();

            // Assembly
            AssemblyDataService assemblyService = new AssemblyDataService();
            AssemblyModel newTankData = assemblyService.CreateMappingDataNew(workbookName, activeCustomWorkbook);

            PaperSettingViewModel selView = this.DataContext as PaperSettingViewModel;
            selView.newTankData=newTankData;

            // Scale Setting : Area
            ModelAreaService areaService = new ModelAreaService();
            PaperAreaService paperAreaService = new PaperAreaService();
            DrawDetailRoofBottomService roofBottomService = new DrawDetailRoofBottomService(newTankData, testModel);

            SingletonData.GAArea = areaService.GetModelAreaData();
            double bottomRoofOD = roofBottomService.GetBottomRoofOD();
            string annularStr = newTankData.BottomInput[0].AnnularPlate;
            string topAngelType = newTankData.RoofCompressionRing[0].CompressionRingType;
            SingletonData.PaperArea.AreaList = paperAreaService.GetPaperAreaData(bottomRoofOD, annularStr, topAngelType);
            // Virtual Design
            DrawDetailVisibleService detailService = new DrawDetailVisibleService(newTankData,testModel);
            detailService.SetDetailVisible(SingletonData.PaperArea.AreaList);

            // BM List
            DrawBMService bmService = new DrawBMService(newTankData);
            SingletonData.BMList = bmService.CreateBMModel();


            if (visibleValue)
            {
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
            }


            LogicBuilder outBuilder = null;


            // Block 보존
            tempImportBlocks = testModel.Blocks;
            //testModel2.Blocks = tempImportBlocks;

            // Delete
            testModel.Entities.Clear();
            testModel.Purge();
            testModel.Invalidate();

            // Create Setting
            DrawSettingService drawSetting = new DrawSettingService();
            drawSetting.SetModelSpace(testModel);
            //drawSetting.SetPaperSpace(testDraw);


            //// Block Loading
            //if (testModel.Blocks.Count == 1)
            //{
            //    if (tempImportBlocks != null)
            //    {
            //        testModel.Blocks = tempImportBlocks;
            //        foreach(devDept.Eyeshot.Block eachBlock in testModel.Blocks)
            //        {
            //            if (eachBlock.Name != "RootBlock")
            //            {
            //                devDept.Eyeshot.Block newBlock = (devDept.Eyeshot.Block)eachBlock.Clone();
            //                testModel2.Blocks.Add(newBlock);
            //            }
            //        }

            //    }
            //}

            if (true)
            {
                // Logic
                DrawLogicDBService newLogic = new DrawLogicDBService();
                DrawLogicModel newLogicData = newLogic.GetLogicCommand(DrawLogicLib.Commons.LogicFile_Type.Detail);
                // 형상 그리는 것은 완료
                List<string> newAll = new List<string>();
                newAll.AddRange(newLogicData.UsingList);
                newAll.AddRange(newLogicData.ReferencePointList);
                foreach (DrawCommandModel eachCommand in newLogicData.CommandList)
                    newAll.AddRange(eachCommand.Command);

                //newAll.ToArray()
                DrawScaleService scaleService = new DrawScaleService();
                double autoScale = scaleService.GetAIScale(newTankData);

                string testName = "ORIENTATION";
                IntergrationService newInterService = new IntergrationService(testName, newTankData, testModel);
                if (newInterService.CreateLogic(autoScale, newAll.ToArray(), out outBuilder))
                {

                    //bgWorker.ReportProgress(99999);
                    //MessageBox.Show("생성 완료", "안내");
                    //DoEvents();

                }
                else
                {
                    //bgWorker.ReportProgress(88888);
                    MessageBox.Show("생성 실패", "안내");
                    //DoEvents();
                }
            }


            if (false)
            {
                // Logic
                DrawLogicDBService newLogic = new DrawLogicDBService();
                DrawLogicModel newLogicData = newLogic.GetLogicCommand(DrawLogicLib.Commons.LogicFile_Type.NozzleOrientation);
                // 형상 그리는 것은 완료
                List<string> newAll = new List<string>();
                newAll.AddRange(newLogicData.UsingList);
                newAll.AddRange(newLogicData.ReferencePointList);
                foreach (DrawCommandModel eachCommand in newLogicData.CommandList)
                    newAll.AddRange(eachCommand.Command);

                //newAll.ToArray()
                DrawScaleService scaleService = new DrawScaleService();
                double autoScale = scaleService.GetAIScale(newTankData);


                IntergrationService newInterService = new IntergrationService("ORIENTATION", newTankData, testModel);
                if (newInterService.CreateLogic(autoScale, newAll.ToArray(), out outBuilder))
                {

                    //bgWorker.ReportProgress(99999);
                    //MessageBox.Show("생성 완료", "안내");
                    //DoEvents();

                }
                else
                {
                    //bgWorker.ReportProgress(88888);
                    MessageBox.Show("생성 실패", "안내");
                    //DoEvents();
                }
            }

            testModel.Entities.Regen();
            testModel.SetView(viewType.Top);

            // fits the model in the viewport
            testModel.ZoomFit();
            testModel.Refresh();


            #region ETC 안쓰는 것
            //SampleDraw(newTankData, autoScale);

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

            #endregion
        }


        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate { }));
        }


        #region Sample Draw
        public void SampleDraw()
        {


            PaperSettingViewModel selView = this.DataContext as PaperSettingViewModel;
            PaperDrawService paperService = new PaperDrawService(this.testModel, this.testDraw);

            //paperService.assemblyData = assemblyData;
            // Clear
            paperService.DrawEntity();

            // Grid 생성
            

            if (selView.newTankData != null)
            {
                PaperSettingService newSetting = new PaperSettingService();

                newSetting.SetCuttingModel();

                // Paper List & Paper Grid List  재 
                selView.PaperList = newSetting.CreateDrawingCRTList(selView.newTankData, testModel);
                selView.PaperGridList = newSetting.CreatePaperGridList(selView.PaperList);

                // Sheet, Frame, TitleBlock, Table,
                paperService.CreatePaperDraw(selView.PaperList, selView.newTankData);

                // Grid
                paperService.CreatePaperGridDraw(selView.PaperGridList, selView.PaperList);


                //paperService.CreatePaperDraw(selView.PaperListSelectionColl);
            }


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
            //TitleBlockWindow cc = new TitleBlockWindow();
            //cc.Owner = this;
            //cc.ShowDialog();


            BusyStartSource(true);
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
            else
            {
                
                    //PaperSettingViewModel selView = this.DataContext as PaperSettingViewModel;
                    //DrawScaleService scaleService = new DrawScaleService();
                    //double autoScale = scaleService.GetAIScale(selView.newTankData);
                    //SampleDraw(selView.newTankData, autoScale);
                testDraw.ZoomFit();
                    
            }
            
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            ReadAutodesk.OnApplicationExit(sender,e);
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

            Stopwatch sw = new Stopwatch();

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
                string imageString = @"/PaperSetting;component/BusyImage/" + imageCurrent + ".png";
                busyImageSource.Source = new BitmapImage(new Uri(@imageString, UriKind.Relative));

                imageCurrent++;
                if (imageMax == imageCurrent)
                    imageCurrent = imageMin;

                //string imageRing = @"/DesignBoard;component/BusyImage/raser_bottom_light0" + ringCurrent + ".png";
                //busysdRing.Source = new BitmapImage(new Uri(imageRing, UriKind.Relative));
                //ringCurrent++;
                //if (ringMax == ringCurrent)
                //    ringCurrent = ringMin;

                if (sw.ElapsedMilliseconds > 7100)
                {
                    IsBusy = false;
                }
            };
            BusyWorker.RunWorkerCompleted += (ssender, ee) =>
            {
                tabDetail.SelectedItem = previewpreview;
                busyAreaSource.Visibility = Visibility.Hidden;
                selCursor.Dispose();

            };
            BusyWorker.DoWork += (ssender, ee) =>
            {
                sw.Start();
                while (IsBusy)
                {
                    BusyWorker.ReportProgress(1);
                    Thread.Sleep(80);
                }
            };
            BusyWorker.RunWorkerAsync();
        }
        #endregion

        private void testModel_ProgressChanged(object sender, devDept.Eyeshot.ProgressChangedEventArgs e)
        {
            progressCustomi++;
            Console.WriteLine(progressCustomi.ToString());
            Console.WriteLine(e.Progress);

            
        }

        private void btnRegen_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.testModel.Entities.RegenAllCurved(0.0005);
            this.testModel.Refresh();

            this.testDraw.Entities.RegenAllCurved(0.0005);
            this.testDraw.Refresh();
            this.testDraw.ZoomFit();
        }



        private void btnOrientationAdj_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OrientationAdjWindow newWindow = new OrientationAdjWindow();
            newWindow.ShowDialog();
        }


        // Text Temp
        private void btnEngLoad_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            List<string> openInfo = new List<string>();
            openInfo.Add("EXISTING DATA LOAD : Engineering Data");
            openInfo.Add("TABAS File Type (*.*)|*.*;");
            openInfo.Add("TABAS File Type (*.xlsm)|*.xlsm;");
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = openInfo[0];
            ofd.Filter = openInfo[1];
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Multiselect = false;

            string tempEngFile = "";
            if (ofd.ShowDialog() == true)
            {
                foreach (string eachFile in ofd.FileNames)
                {
                    //selView.currentNoticeList.ImagePath.Name = System.IO.Path.GetFileName(eachFile);
                    //selView.currentNoticeList.ImagePath.Path = eachFile;
                    tempEngFile = eachFile;
                    break;
                }

            }

            if (File.Exists(tempEngFile))
            {
                workbookName = tempEngFile;
                tbEngFile.Text = tempEngFile;
                SetAssemblyData();

                MessageBox.Show("파일 LOAD 완료");
            }
            else
            {
                MessageBox.Show("파일 경로가 올바르지 않습니다.");
            }
        }

        private void btnEngSave_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string tempEngFile = tbEngFile.Text;
            if (File.Exists(tempEngFile))
            {
                EPService excelService = new EPService();
                if (excelService.SetSaveData(tempEngFile))
                {
                    MessageBox.Show("파일 저장 완료");
                }
            }
            else
            {
                MessageBox.Show("파일 경로가 올바르지 않습니다.");
            }
        }

        private void btnEngCheck_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string tempEngFile = tbEngFile.Text;
            if (File.Exists(tempEngFile))
            {
                EPService excelService = new EPService();
                string tempData=excelService.GetLoadData(tempEngFile);
                    MessageBox.Show("파일 저장 시간" + " : " + tempData);
            }
            else
            {
                MessageBox.Show("파일 경로가 올바르지 않습니다.");
            }
        }
    }
}
