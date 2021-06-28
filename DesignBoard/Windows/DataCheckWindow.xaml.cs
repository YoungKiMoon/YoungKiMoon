using DesignBoard.Models;
using DesignBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DesignBoard.Windows
{
    /// <summary>
    /// DataCheckWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DataCheckWindow : Window
    {
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

        public DataCheckWindow()
        {
            InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }


        private void btnClose_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void btnApply_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("적용 완료", "안내");
            Close();
        }

        private void btnReCheck_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (One_Click())
            {
                DataCheckWindowViewModel selView = this.DataContext as DataCheckWindowViewModel;
                selView.ReCheck();
            }



        }

        public void SetData(ObservableCollection<DataCheckModel> DataCheckList)
        {
            DataCheckWindowViewModel selView = this.DataContext as DataCheckWindowViewModel;
            selView.DataCheckList = DataCheckList;
            selView.RefreshSummary();
        }



        
    }
}
