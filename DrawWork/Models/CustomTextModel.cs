using DrawWork.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.Models
{
    public class CustomTextModel : Notifier
    {
        public CustomTextModel()
        {
            Name = "";
            FontFamily = "";
            WidthFactor = "";
            FileName = "";
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
        public string FontFamily
        {
            get { return _FontFamily; }
            set
            {
                _FontFamily = value;
                OnPropertyChanged(nameof(FontFamily));
            }
        }
        private string _FontFamily;
        public string WidthFactor
        {
            get { return _WidthFactor; }
            set
            {
                _WidthFactor = value;
                OnPropertyChanged(nameof(WidthFactor));
            }
        }
        private string _WidthFactor;
        public string FileName
        {
            get { return _FileName; }
            set
            {
                _FileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }
        private string _FileName;
    }
}
