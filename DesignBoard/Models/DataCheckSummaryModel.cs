using DesignBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignBoard.Models
{
    public class DataCheckSummaryModel : Notifier
    {
        public DataCheckSummaryModel()
        {
            InfoCount = 0;
            WarnningCount = 0;
            ErrorCount = 0;
        }

        public double InfoCount
        {
            get { return _InfoCount; }
            set
            {
                _InfoCount = value;
                OnPropertyChanged(nameof(InfoCount));
            }
        }
        private double _InfoCount;

        public double WarnningCount
        {
            get { return _WarnningCount; }
            set
            {
                _WarnningCount = value;
                OnPropertyChanged(nameof(WarnningCount));
            }
        }
        private double _WarnningCount;

        public double ErrorCount
        {
            get { return _ErrorCount; }
            set
            {
                _ErrorCount = value;
                OnPropertyChanged(nameof(ErrorCount));
            }
        }
        private double _ErrorCount;
    }
}
