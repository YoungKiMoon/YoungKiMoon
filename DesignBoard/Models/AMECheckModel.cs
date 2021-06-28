using DesignBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignBoard.Models
{
    public class AMECheckModel : Notifier
    {
        public AMECheckModel()
        {
            No = "";
            Error = false;
            Name = "";
            Value = "";
            Information = "";
        }

        public string No
        {
            get { return _No; }
            set
            {
                _No = value;
                OnPropertyChanged(nameof(No));
            }
        }
        private string _No;

        public bool Error
        {
            get { return _Error; }
            set
            {
                _Error = value;
                OnPropertyChanged(nameof(Error));
            }
        }
        private bool _Error;

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

        public string Information
        {
            get { return _Information; }
            set
            {
                _Information = value;
                OnPropertyChanged(nameof(Information));
            }
        }
        private string _Information;
    }
}
