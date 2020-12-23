using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testfunction.Utils;

namespace testfunction.Models
{
    public class Shell_In_Num_Model : Notifier
    {

        public Shell_In_Num_Model()
        {
            No = 0;
            ID = 0;
            Height = 0;
            PLWidth = 0;
            PLHeight = 0;
        }

        private double _No;
        public double No
        {
            get { return _No; }
            set
            {
                _No = value;
                OnPropertyChanged(nameof(No));
            }
        }

        private double _ID;
        public double ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        private double _Height;
        public double Height
        {
            get { return _Height; }
            set
            {
                _Height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        private double _PLWidth;
        public double PLWidth
        {
            get { return _PLWidth; }
            set
            {
                _PLWidth = value;
                OnPropertyChanged(nameof(PLWidth));
            }
        }

        private double _PLHeight;
        public double PLHeight
        {
            get { return _PLHeight; }
            set
            {
                _PLHeight = value;
                OnPropertyChanged(nameof(PLHeight));
            }
        }

    }
}
