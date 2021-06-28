using DesignBoard.Models;
using DesignBoard.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DesignBoard.ViewModels
{
    public class DataInputWindowViewModel : Notifier
    {
        public ObservableCollection<DataListModel> DataList
        {
            get { return _DataList; }
            set
            {
                _DataList = value;
                OnPropertyChanged(nameof(DataList));
            }
        }
        private ObservableCollection<DataListModel> _DataList;

        public DataListModel currentDataList
        {
            get { return _currentDataList; }
            set
            {
                _currentDataList = value;
                OnPropertyChanged(nameof(currentDataList));
            }
        }
        private DataListModel _currentDataList;


        public ObservableCollection<ListBoxItemModel> GroupList
        {
            get { return _GroupList; }
            set
            {
                _GroupList = value;
                OnPropertyChanged(nameof(GroupList));
            }
        }
        private ObservableCollection<ListBoxItemModel> _GroupList;


        public List<ImageModel> ImageList;

        public DataInputWindowViewModel()
        {
            DataList = new ObservableCollection<DataListModel>();
            currentDataList = new DataListModel();
            GroupList = new ObservableCollection<ListBoxItemModel>();
        }

        public void RefreshGroupList()
        {
            GroupList.Clear();
            foreach(DataListModel eachModel in DataList)
            {
                ListBoxItemModel newListBoxItem = new ListBoxItemModel() { Name = eachModel.Name };
                GroupList.Add(newListBoxItem);
            }

        }

        public void ChangeCurrentList(string selListName)
        {
            currentDataList = new DataListModel();
            foreach (DataListModel eachModel in DataList)
            {
                if (eachModel.Name == selListName)
                {
                    currentDataList = eachModel;
                    break;
                }
            }
        }
    }
}
