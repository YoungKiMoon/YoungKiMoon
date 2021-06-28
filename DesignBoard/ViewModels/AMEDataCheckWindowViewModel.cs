using DesignBoard.Models;
using DesignBoard.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace DesignBoard.ViewModels
{
    public class AMEDataCheckWindowViewModel : Notifier
    {
        public ObservableCollection<AMECheckModel> AMEList
        {
            get { return _AMEList; }
            set
            {
                _AMEList = value;
                OnPropertyChanged(nameof(AMEList));
            }
        }
        private ObservableCollection<AMECheckModel> _AMEList;
        
        
        public AMEDataCheckWindowViewModel()
        {
            AMEList = new ObservableCollection<AMECheckModel>();
        }


        public ListCollectionView GetAllData()
        {
            ListCollectionView collectionView = new ListCollectionView(AMEList);
            return collectionView;
        }
        public ListCollectionView GetErrorData()
        {
            ListCollectionView collectionView = new ListCollectionView(AMEList);
            collectionView.Filter = (e) =>
            {
                AMECheckModel emp = e as AMECheckModel;
                if (emp.Error)
                    return true;
                return false;
            };
            return collectionView;
        }
        public void ReCheck()
        {
            foreach(AMECheckModel eachModel in AMEList)
            {
                eachModel.Error = false;
            }
        }
    }
}
