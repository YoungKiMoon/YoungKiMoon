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
using WpfCadCon.Services;
using WpfCadCon.Windows;

namespace WpfCadCon
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        

        private void btnRun_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CADConService cadCon = new CADConService();
            cadCon.RunProgram();
        }

        private void btnReg_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            CADRegistryService.runReg();
            MessageBox.Show("등록완료");
            
            
        }

        private void btnReg2_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CADRegistryService.runRegDelete();
            MessageBox.Show("삭제완료");
        }

        private void btnReg3_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TestUI cc = new TestUI();
            cc.Show();
        }

        private void btnReg4_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TestTable cc = new TestTable();
            cc.Show();
        }
    }
}
