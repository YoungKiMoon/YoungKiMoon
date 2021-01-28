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

namespace PaperSetting
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PaperSettingWindow : Window
    {
        public PaperSettingWindow()
        {
            Console.WriteLine("aa");
            InitializeComponent();
            ModelDrawService modelService = new ModelDrawService(this.testModel);
            modelService.CreateSample();
        }


        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if(e.Source is TabControl)
            {
                int tabIndex = (sender as TabControl).SelectedIndex;
                TabSelectionEvent(tabIndex);
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
                    //PaperSettingViewModel selView = this.DataContext as PaperSettingViewModel;
                    //PaperDrawService paperService = new PaperDrawService(this.testModel, this.testDraw);
                    //paperService.CreatePaperDraw(selView.PaperListSelectionColl);
                    break;
                default:
                    break;
            }
        }



        private void Button_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PaperSettingViewModel selView = this.DataContext as PaperSettingViewModel;
            PaperDrawService paperService = new PaperDrawService(this.testModel, this.testDraw);
            paperService.CreatePaperDraw(selView.PaperListSelectionColl);
        }
    }
}
