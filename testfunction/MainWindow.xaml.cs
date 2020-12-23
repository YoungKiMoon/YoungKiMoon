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
using testfunction.ViewModels;

namespace testfunction
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

        // Button : Click
        private void Border_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindowViewModel selView=this.DataContext as MainWindowViewModel;
            selView.CalculationShell();
        }

        private void Border_PreviewMouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            MainWindowViewModel selView = this.DataContext as MainWindowViewModel;
            selView.CalculationShell2();
        }

        private void Border_PreviewMouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {
            MainWindowViewModel selView = this.DataContext as MainWindowViewModel;
            selView.CalculationShell3();
        }
    }
}
