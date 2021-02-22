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
using DrawWork.AssemblyModels;
using DrawWork.DrawServices;
using DrawWork.CommandModels;
using DrawWork.FileServices;

namespace DrawWork
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        DrawSettingService drawSetting;

        public MainWindow()
        {
            InitializeComponent();

            this.testModel.Unlock("UF20-LX12S-KRDSL-F0GT-FD74");
            this.testModel.ActionMode = devDept.Eyeshot.actionType.SelectByPick;

            drawSetting = new DrawSettingService();
            drawSetting.SetModelSpace(testModel);

            logicFile.Text = @"C:\Users\tree\Desktop\CAD\tabas\Sample_DrawLogic.txt";
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            drawSetting.CreateModelSpaceSample(testModel);
            MessageBox.Show("완료");
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel selView = this.DataContext as MainWindowViewModel;

            string logicFilePath = logicFile.Text;

            if (logicFilePath != "")
            {
                TextFileService newFileService = new TextFileService();
                string[] newComData = newFileService.GetTextFileArray(logicFile.Text);
                selView.commandData.commandList = new List<CommandLineModel>();
                foreach (string eachText in newComData)
                    selView.commandData.commandList.Add(new CommandLineModel { CommandText = eachText });
            }

            LogicBuilder testBuilder = selView.GetLogicBuilder();

            // remove old block
            testModel.Entities.Clear();
            testModel.StartWork(testBuilder);
            MessageBox.Show("완료");
        }
    }
}
