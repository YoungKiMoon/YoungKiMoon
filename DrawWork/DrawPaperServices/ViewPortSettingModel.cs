using DrawWork.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawPaperServices
{
    public class ViewPortSettingModel : Notifier
    {
        public ViewPortSettingModel()
        {
            Scale = "";
            TargetX = "";
            TargetY = "";
            LocationX = "";
            LocationY = "";
            SizeX = "";
            SizeY = "";
        }
        public string Scale
        {
            get { return _Scale; }
            set
            {
                _Scale = value;
                OnPropertyChanged(nameof(Scale));
            }
        }
        private string _Scale;

        public string TargetX
        {
            get { return _TargetX; }
            set 
            {
                _TargetX = value;
                OnPropertyChanged(nameof(TargetX));
            }
        }
        private string _TargetX;

        public string TargetY
        {
            get { return _TargetY; }
            set
            {
                _TargetY = value;
                OnPropertyChanged(nameof(TargetY));
            }
        }
        private string _TargetY;

        public string LocationX
        {
            get { return _LocationX; }
            set
            {
                _LocationX = value;
                OnPropertyChanged(nameof(LocationX));
            }
        }
        private string _LocationX;
        public string LocationY
        {
            get { return _LocationY; }
            set
            {
                _LocationY = value;
                OnPropertyChanged(nameof(LocationY));
            }
        }
        private string _LocationY;

        public string SizeX
        {
            get { return _SizeX; }
            set
            {
                _SizeX = value;
                OnPropertyChanged(nameof(SizeX));
            }
        }
        private string _SizeX;
        public string SizeY
        {
            get { return _SizeY; }
            set
            {
                _SizeY = value;
                OnPropertyChanged(nameof(SizeY));
            }
        }
        private string _SizeY;
    }
}
