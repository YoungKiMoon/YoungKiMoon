using DesignBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignBoard.Models
{
    public class DataModel : Notifier
    {
        public DataModel()
        {
            Name = "";
            Value = "";
            Description = "";
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

        public string Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                OnPropertyChanged(nameof(Value));
            }
        }
        private string _Value;

        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        private string _Description;
    }
}
