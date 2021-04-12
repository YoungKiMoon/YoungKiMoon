using DrawWork.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.Models
{
    public class CustomBlockModel : Notifier
    {
        public CustomBlockModel()
        {
            Name = "";
            Unit = "";
            BasePoint = "";
            InUse = "";
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
        public string Unit
        {
            get { return _Unit; }
            set
            {
                _Unit = value;
                OnPropertyChanged(nameof(Unit));
            }
        }
        private string _Unit;
        public string BasePoint
        {
            get { return _BasePoint; }
            set
            {
                _BasePoint = value;
                OnPropertyChanged(nameof(BasePoint));
            }
        }
        private string _BasePoint;

        public string InUse
        {
            get { return _InUse; }
            set
            {
                _InUse = value;
                OnPropertyChanged(nameof(InUse));
            }
        }
        private string _InUse;
    }
}
