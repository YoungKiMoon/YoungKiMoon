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

using DrawWork.SampleData;
using DrawWork.ViewModels;
using DrawWork.DrawBuilders;
using DrawWork.AssemblyModels;

namespace DrawWork
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.testModel.Unlock("UF20-LX12S-KRDSL-F0GT-FD74");
            this.testModel.ActionMode = devDept.Eyeshot.actionType.SelectByPick;

        }
        private void CreateSample()
        {
            ModelDataSample newSample = new ModelDataSample(this.testModel);
            newSample.CreateSample();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel selView = this.DataContext as MainWindowViewModel;
            CreateSample();

            CreateSampleModel();

        }

        private void CreateSampleModel()
        {
            MainWindowViewModel selView = this.DataContext as MainWindowViewModel;

            LogicBuilder testBuilder = selView.GetLogicBuilder();

            // remove old block
            testModel.Entities.Clear();
            testModel.StartWork(testBuilder);
        }
    }
}
