using ExcelAddIn.Commons;
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

namespace ExcelAddIn.Windows
{
    /// <summary>
    /// TypePlanWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TypePlanWindow : Window
    {
        public ROOF_TYPE selectButton;

        public TypePlanWindow()
        {
            InitializeComponent();
            selectButton =ROOF_TYPE.NotSet;

            btnCRT.BorderThickness = new Thickness(0);
            btnDRT.BorderThickness = new Thickness(0);
            btnIFRT.BorderThickness = new Thickness(0);
            btnEFRTs.BorderThickness = new Thickness(0);
            btnEFRTd.BorderThickness = new Thickness(0);
        }

        private void btnCRT_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            selectButton = ROOF_TYPE.CRT;
            this.Hide();
        }

        private void btnDRT_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            selectButton = ROOF_TYPE.DRT;
            this.Hide();
        }

        private void btnIFRT_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            selectButton = ROOF_TYPE.IFRT;
            this.Hide();
        }

        private void btnEFRTs_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            selectButton = ROOF_TYPE.EFRTSingle;
            this.Hide();
        }
        private void btnEFRTd_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            selectButton = ROOF_TYPE.EFRTDouble;
            this.Hide();
        }

        private void btnCRT_MouseEnter(object sender, MouseEventArgs e)
        {
            btnCRT.BorderThickness = new Thickness(4);
            btnDRT.BorderThickness = new Thickness(0);
            btnIFRT.BorderThickness = new Thickness(0);
            btnEFRTs.BorderThickness = new Thickness(0);
            btnEFRTd.BorderThickness = new Thickness(0);
            textCRT.FontSize = 26;
            textDRT.FontSize = 22;
            textIFRT.FontSize = 22;
            textEFRTs1.FontSize = 22;
            textEFRTs2.FontSize = 10;
            textEFRTd1.FontSize = 22;
            textEFRTd2.FontSize = 10;

        }

        private void btnDRT_MouseEnter(object sender, MouseEventArgs e)
        {
            btnCRT.BorderThickness = new Thickness(0);
            btnDRT.BorderThickness = new Thickness(4);
            btnIFRT.BorderThickness = new Thickness(0);
            btnEFRTs.BorderThickness = new Thickness(0);
            btnEFRTd.BorderThickness = new Thickness(0);
            textCRT.FontSize = 22;
            textDRT.FontSize = 26;
            textIFRT.FontSize = 22;
            textEFRTs1.FontSize = 22;
            textEFRTs2.FontSize = 10;
            textEFRTd1.FontSize = 22;
            textEFRTd2.FontSize = 10;
        }

        private void btnIFRT_MouseEnter(object sender, MouseEventArgs e)
        {
            btnCRT.BorderThickness = new Thickness(0);
            btnDRT.BorderThickness = new Thickness(0);
            btnIFRT.BorderThickness = new Thickness(4);
            btnEFRTs.BorderThickness = new Thickness(0);
            btnEFRTd.BorderThickness = new Thickness(0);
            textCRT.FontSize = 22;
            textDRT.FontSize = 22;
            textIFRT.FontSize = 26;
            textEFRTs1.FontSize = 22;
            textEFRTs2.FontSize = 10;
            textEFRTd1.FontSize = 22;
            textEFRTd2.FontSize = 10;
        }



        private void btnEFRTs_MouseEnter(object sender, MouseEventArgs e)
        {
            btnCRT.BorderThickness = new Thickness(0);
            btnDRT.BorderThickness = new Thickness(0);
            btnIFRT.BorderThickness = new Thickness(0);
            btnEFRTs.BorderThickness = new Thickness(4);
            btnEFRTd.BorderThickness = new Thickness(0);
            textCRT.FontSize = 22;
            textDRT.FontSize = 22;
            textIFRT.FontSize = 22;
            textEFRTs1.FontSize = 26;
            textEFRTs2.FontSize = 12;
            textEFRTd1.FontSize = 22;
            textEFRTd2.FontSize = 10;
        }

        private void btnEFRTd_MouseEnter(object sender, MouseEventArgs e)
        {
            btnCRT.BorderThickness = new Thickness(0);
            btnDRT.BorderThickness = new Thickness(0);
            btnIFRT.BorderThickness = new Thickness(0);
            btnEFRTs.BorderThickness = new Thickness(0);
            btnEFRTd.BorderThickness = new Thickness(4);
            textCRT.FontSize = 22;
            textDRT.FontSize = 22;
            textIFRT.FontSize = 22;
            textEFRTs1.FontSize = 22;
            textEFRTs2.FontSize = 10;
            textEFRTd1.FontSize = 26;
            textEFRTd2.FontSize = 12;
        }
    }
}
