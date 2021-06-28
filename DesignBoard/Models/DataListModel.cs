using DesignBoard.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DesignBoard.Models
{
    public class DataListModel : Notifier
    {
        public DataListModel()
        {
            Name = "";
            List = new ObservableCollection<DataModel>();
        }


        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string _Name;

        public ObservableCollection<DataModel> List
        {
            get { return _List; }
            set
            {
                _List = value;
                OnPropertyChanged(nameof(List));
            }
        }
        private ObservableCollection<DataModel> _List;

    }
}
