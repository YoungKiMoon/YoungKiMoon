using DesignBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace DesignBoard.Models
{
    public class CheckModel : Notifier
    {
        public CheckModel()
        {
            checkColor = new SolidColorBrush( Color.FromArgb(255,132, 151, 176));
        }
        public SolidColorBrush checkColor
        {
            get { return _checkColor; }
            set
            {
                _checkColor = value;
                OnPropertyChanged(nameof(checkColor));
            }
        }
        private SolidColorBrush _checkColor;
    }
}
