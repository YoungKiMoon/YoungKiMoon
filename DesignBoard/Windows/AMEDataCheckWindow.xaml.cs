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
    /// AMEDataCheckWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AMEDataCheckWindow : Window
    {
        public bool autoRecheckError=false;

        public AMEDataCheckWindow()
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
            AMEDataCheckWindowViewModel selView = this.DataContext as AMEDataCheckWindowViewModel;
            if(autoRecheckError)
                selView.ReCheck();

            ListCollectionView tempCheck= selView.GetErrorData();
            if (tempCheck.Count == 0)
            {
                MessageBox.Show("Error 데이터가 없습니다.", "안내");
                rbAll.IsChecked = true;
                FilterAllData();
            }
            else
            {
                MessageBox.Show(tempCheck.Count + " 개 : Error 데이터가 있습니다.", "안내");
                rbError.IsChecked = true;
                FilterErrorData();
            }
        }


        #region Filter

        private void rbAll_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FilterAllData();
        }
        private void rbError_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FilterErrorData();
        }
        #endregion

        #region Data & Filter
        public void SetAMEData(ObservableCollection<AMECheckModel> AMEList)
        {
            AMEDataCheckWindowViewModel selView = this.DataContext as AMEDataCheckWindowViewModel;
            selView.AMEList = AMEList;
            FilterAllData();
        }
        public void FilterAllData()
        {
            AMEDataCheckWindowViewModel selView = this.DataContext as AMEDataCheckWindowViewModel;
            dgAME.ItemsSource = selView.GetAllData();
        }
        public void FilterErrorData()
        {
            AMEDataCheckWindowViewModel selView = this.DataContext as AMEDataCheckWindowViewModel;
            dgAME.ItemsSource = selView.GetErrorData();

        }

        #endregion


    }
}
