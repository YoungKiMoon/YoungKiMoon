using DesignBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignBoard.Models
{
    public class ListBoxItemModel : Notifier
    {
        public ListBoxItemModel()
        {
            Name = "";
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
    }
}
