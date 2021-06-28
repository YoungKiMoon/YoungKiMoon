using DesignBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignBoard.Models
{
    public class DataCheckModel : Notifier
    {
        public DataCheckModel()
        {
            Type = "";
            Component = "";
            Name = "";
            Value = "";
            Information = "";
        }

        public string Type
        {
            get { return _Type; }
            set
            {
                _Type = value;
                OnPropertyChanged(nameof(Type));
            }
        }
        private string _Type;


        public string Component
        {
            get { return _Component; }
            set
            {
                _Component = value;
                OnPropertyChanged(nameof(Component));
            }
        }
        private string _Component;


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
