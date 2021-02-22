using DrawWork.AssemblyModels;
using DrawWork.ViewModels;
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
using System.Windows.Shapes;

namespace DrawWork.Windows
{
    /// <summary>
    /// AssemblyWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AssemblyWindow : Window
    {
        public AssemblyWindow()
        {
            InitializeComponent();
        }

        public void SetAssembly(AssemblyModel selModel)
        {
            AssemblyWindowViewModel selView = this.DataContext as AssemblyWindowViewModel;
            selView.CreateAssembly(selModel);
        }
    }
}
