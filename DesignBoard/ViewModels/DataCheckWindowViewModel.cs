using DesignBoard.Models;
using DesignBoard.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DesignBoard.ViewModels
{
    public class DataCheckWindowViewModel : Notifier
    {
        public ObservableCollection<DataCheckModel> DataCheckList
        {
            get { return _DataCheckList; }
            set
            {
                _DataCheckList = value;
                OnPropertyChanged(nameof(DataCheckList));
            }
        }
        private ObservableCollection<DataCheckModel> _DataCheckList;



        public DataCheckSummaryModel DataCheckListSummary
        {
            get { return _DataCheckListSummary; }
            set
            {
                _DataCheckListSummary = value;
                OnPropertyChanged(nameof(DataCheckListSummary));
            }
        }
        private DataCheckSummaryModel _DataCheckListSummary;

        public DataCheckWindowViewModel()
        {
            DataCheckList = new ObservableCollection<DataCheckModel>();
            DataCheckListSummary = new DataCheckSummaryModel();
        }

        public void ReCheck()
        {
            for(int i=DataCheckList.Count-1;i>=0;i--)
            {
                DataCheckModel eachModel = DataCheckList[i];
                if (eachModel.Type == "ERROR")
                {
                    DataCheckList.Remove(eachModel);
                }
            }

            RefreshSummary();
        }

        public void RefreshSummary()
        {
            DataCheckListSummary.InfoCount = 0;
            DataCheckListSummary.WarnningCount = 0;
            DataCheckListSummary.ErrorCount = 0;

            foreach (DataCheckModel eachModel in DataCheckList)
            {
                if (eachModel.Type == "INFO")
                    DataCheckListSummary.InfoCount++;
                else if (eachModel.Type == "WARNNING")
                    DataCheckListSummary.WarnningCount++;
                else if (eachModel.Type == "ERROR")
                    DataCheckListSummary.ErrorCount++;
            }
        }

    }
}
