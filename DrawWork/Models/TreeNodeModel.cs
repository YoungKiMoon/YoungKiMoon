using DrawWork.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.Models
{
    public class TreeNodeModel : Notifier
    {
        public TreeNodeModel()
        {
            _Name = "";
            _Children = new ObservableCollection<TreeNodeModel>();
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private ObservableCollection<TreeNodeModel> _Children;
        public ObservableCollection<TreeNodeModel> Children
        {
            get { return _Children; }
            set
            {
                _Children = value;
                OnPropertyChanged(nameof(Children));
            }
        }
    }
}
