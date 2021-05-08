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
using System.Windows.Navigation;
using System.Windows.Shapes;

using DrawWork.ViewModels;
using DrawWork.DrawBuilders;
using DrawWork.DrawServices;
using DrawWork.CommandModels;
using DrawWork.FileServices;
using DrawWork.Windows;
using Microsoft.Win32;
using devDept.Eyeshot.Translators;
using devDept.Eyeshot;
using devDept.Graphics;
using devDept.Eyeshot.Entities;
using DrawWork.ImportServices;
using DrawWork.DrawStyleServices;
using ExcelDataLib.ExcelServices;
using System.Collections.ObjectModel;
using ExcelDataLib.ExcelModels;
using System.Diagnostics;
using DrawWork.DesignServices;

namespace DrawWork
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        DrawSettingService drawSetting;

        private Drawings tempPaper;

        public MainWindow()
        {
            InitializeComponent();

            this.testModel.Unlock("UF20-LX12S-KRDSL-F0GT-FD74");
            this.testModel.ActionMode = devDept.Eyeshot.actionType.SelectByPick;

            //this.testModel.ActiveViewport.DisplayMode = devDept.Eyeshot.displayType.Rendered;
            //this.testModel.ActiveViewport.DisplayMode = devDept.Eyeshot.displayType.Rendered;

            drawSetting = new DrawSettingService();
            drawSetting.SetModelSpace(testModel);

            logicFile.Text = @"C:\Users\tree\Desktop\CAD\tabas\Sample_DrawLogic.txt";
            dwgFile.Text = @"C:\Users\tree\Desktop\CAD\tabas\Block_Sample.dwg";

            inputScale.Text = "90";
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            drawSetting.CreateModelSpaceSample(testModel);
            //MessageBox.Show("완료");
        }



        // Create
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel selView = this.DataContext as MainWindowViewModel;
            selView.CreateCommandService(testModel);

            string logicFilePath = logicFile.Text;

            if (logicFilePath != "")
            {
                selView.commandData.commandList.Clear();
                TextFileService newFileService = new TextFileService();
                string[] newComData = newFileService.GetTextFileArray(logicFile.Text);
                //selView.commandData.commandList = new List<CommandLineModel>();
                foreach (string eachText in newComData)
                    selView.commandData.commandList.Add(new CommandLineModel(eachText));
            }


            double selScale = Convert.ToDouble(inputScale.Text);
            LogicBuilder testBuilder = selView.GetLogicBuilder(selScale);

            // remove old block
            testModel.Entities.Clear();
            testModel.StartWork(testBuilder);
            MessageBox.Show("완료");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel selView = this.DataContext as MainWindowViewModel;

            AssemblyWindow cc = new AssemblyWindow();
            cc.SetAssembly(selView.TankData);
            cc.Show();
        }

        private void btnCreateDwg_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var exportFileDialog = new SaveFileDialog();
            exportFileDialog.Filter = "CAD drawings(*.dwg)| *.dwg|" + "Drawing Exchange Format (*.dxf)|*.dxf";
            exportFileDialog.AddExtension = true;
            exportFileDialog.Title = "Export";
            exportFileDialog.CheckPathExists = true;
            var result = exportFileDialog.ShowDialog();
            if (result == true)
            {
                WriteAutodeskParams wap = new WriteAutodeskParams(testModel, tempPaper, false, false);
                WriteFileAsync wa = new WriteAutodesk(wap, exportFileDialog.FileName);

                testModel.StartWork(wa);


            }
        }

        private void btnLoadDWG_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            var importFileDialog = new OpenFileDialog();
            importFileDialog.Filter= "All compatible file types (*.*)|*.step;*.stp;*.iges;*.igs;*.stl;*.obj;*.dwg;*.dxf|Standard for the Exchange of Product Data (*.stp; *.step)|*.stp; *.step|Initial Graphics Exchange Specification (*.igs; *.iges)|*.igs; *.iges|WaveFront OBJ (*.obj)|*.obj|Stereolithography (*.stl)|*.stl|CAD drawings (*.dwg)|*.dwg|Drawing Exchange Format (*.dxf)|*.dxf";
            importFileDialog.AddExtension = true;
            importFileDialog.Title = "Import";
            importFileDialog.CheckFileExists = true;
            importFileDialog.CheckPathExists = true;
            var result = importFileDialog.ShowDialog();
            if (result == true)
            {
                //testModel.Clear();

                BackgroundSettings temp = new BackgroundSettings();
                temp.TopColor = new SolidColorBrush(Color.FromRgb(225,225,225));
                testModel.Viewports[0].Background = temp;

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




        private ReadFileAsync GetReader(string fileName)
        {
            string ext = System.IO.Path.GetExtension(fileName);

            if (ext != null)
            {
                ext = ext.TrimStart('.').ToLower();

                switch (ext)
                {
                    case "dwg":
                    case "dxf":

                        ReadAutodesk ra = new ReadAutodesk(fileName);
                        ra.SkipLayouts = false;
                        return ra;

                    case "stl":
                        return new ReadSTL(fileName);
                    case "obj":
                        return new ReadOBJ(fileName);

                }
            }

            return null;
        }

        private void testModel_WorkCompleted(object sender, WorkCompletedEventArgs e)
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
                    MessageBox.Show("Block Import 완료");
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



        // Environment
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            EnvironmentWindow newWin = new EnvironmentWindow();
            EnvironmentWindowViewModel newWinView = newWin.DataContext as EnvironmentWindowViewModel;
            newWinView.SetModelEnvironment(testModel);
            newWinView.CreateEnvironment();
            newWin.Show();

        }


        // Preview
        private void btnPreview_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PreviewWindow cc = new PreviewWindow();
            cc.Owner = this;
            PreviewWindowViewModel previewView = cc.DataContext as PreviewWindowViewModel;
            previewView.previewService.SetModelObject(this.testModel);
            previewView.previewService.SetDrawingsObject(cc.testDraw);

            previewView.viewPortSet.Scale = inputScale.Text;

            tempPaper = cc.testDraw;
            cc.Show();
        }

        private void btnCreateExcel_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindowViewModel selView = this.DataContext as MainWindowViewModel;
            ObservableCollection<ExcelWorkSheetModel> newSheetList = new ObservableCollection<ExcelWorkSheetModel>();

            Stopwatch stopWatch = new Stopwatch(); 
            stopWatch.Start();

            ExcelApplicationService eDataService = new ExcelApplicationService(-1);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            MessageBox.Show(elapsedTime);

            stopWatch.Start();
            newSheetList = eDataService.GetSheetListAll("TankDesign_0428.xlsm");
            stopWatch.Stop();
            TimeSpan ts1 = stopWatch.Elapsed;
            string elapsedTime1 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts1.Hours, ts1.Minutes, ts1.Seconds, ts1.Milliseconds / 10);
            MessageBox.Show(elapsedTime1);

            stopWatch.Start();
            eDataService.GetSheetData(selView.TankData, newSheetList);
            stopWatch.Stop();
            TimeSpan ts2 = stopWatch.Elapsed;
            string elapsedTime2 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts2.Hours, ts2.Minutes, ts2.Seconds, ts2.Milliseconds / 10);
            MessageBox.Show(elapsedTime2);


            DesignService designS = new DesignService();
            designS.CreateDesignCRTModel(selView.TankData);

        }
    }
}
