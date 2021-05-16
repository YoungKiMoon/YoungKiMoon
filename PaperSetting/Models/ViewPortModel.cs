using PaperSetting.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaperSetting.Models
{
    public class ViewPortModel : Notifier,ICloneable
    {
        public ViewPortModel()
        {
            Location = new PointModel();
            Size = new SizeModel();
            Scale = new ScaleModel();

            ScaleStr = "";
            TargetX = "";
            TargetY = "";
            LocationX = "";
            LocationY = "";
            SizeX = "";
            SizeY = "";
        }

        public object Clone()
        {
            ViewPortModel newModel = new ViewPortModel();
            newModel.Location = Location;
            newModel.Size = Size;
            newModel.Scale = Scale;

            newModel.ScaleStr = ScaleStr;
            newModel.TargetX = TargetX;
            newModel.TargetY = TargetY;
            newModel.LocationX = LocationX;
            newModel.LocationY = LocationY;
            newModel.SizeX = SizeX;
            newModel.SizeY = SizeY;

            return newModel;
        }

        private PointModel _Location;
        public PointModel Location
        {
            get { return _Location; }
            set
            {
                _Location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        private SizeModel _Size;
        public SizeModel Size
        {
            get { return _Size; }
            set
            {
                _Size = value;
                OnPropertyChanged(nameof(Size));
            }
        }

        private ScaleModel _Scale;
        public ScaleModel Scale
        {
            get { return _Scale; }
            set
            {
                _Scale = value;
                OnPropertyChanged(nameof(Scale));
            }
        }



        public string ScaleStr
        {
            get { return _ScaleStr; }
            set
            {
                _ScaleStr = value;
                OnPropertyChanged(nameof(ScaleStr));
            }
        }
        private string _ScaleStr;

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
