using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testfunction.Utils;

namespace testfunction.Models
{
    public class Shell_Out_Model : Notifier
    {
        public Shell_Out_Model()
        {
            No = "";
            Course = "";
            Thickness = "";
            StartPoint = "";
            OnePLHeight = "";
            OnePLWidth = "";
            Count = "";
        }


        private string _No;
        public string No
        {
            get { return _No; }
            set
            {
                _No = value;
                OnPropertyChanged(nameof(No));
            }
        }

        private string _Course;
        public string Course
        {
            get { return _Course; }
            set
            {
                _Course = value;
                OnPropertyChanged(nameof(Course));
            }
        }

        private string _Thickness;
        public string Thickness
        {
            get { return _Thickness; }
            set
            {
                _Thickness = value;
                OnPropertyChanged(nameof(Thickness));
            }
        }

        private string _StartPoint;
        public string StartPoint
        {
            get { return _StartPoint; }
            set
            {
                _StartPoint = value;
                OnPropertyChanged(nameof(StartPoint));
            }
        }

        private string _OnePLHeight;
        public string OnePLHeight
        {
            get { return _OnePLHeight; }
            set
            {
                _OnePLHeight = value;
                OnPropertyChanged(nameof(OnePLHeight));
            }
        }

        private string _OnePLWidth;
        public string OnePLWidth
        {
            get { return _OnePLWidth; }
            set
            {
                _OnePLWidth = value;
                OnPropertyChanged(nameof(OnePLWidth));
            }
        }

        private string _Count;
        public string Count
        {
            get { return _Count; }
            set
            {
                _Count = value;
                OnPropertyChanged(nameof(Count));
            }
        }

        private string _CountA;
        public string CountA
        {
            get { return _CountA; }
            set
            {
                _CountA = value;
                OnPropertyChanged(nameof(CountA));
            }
        }
    }
}
