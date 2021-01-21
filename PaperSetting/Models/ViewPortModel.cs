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
        }

        public object Clone()
        {
            ViewPortModel newModel = new ViewPortModel();
            newModel.Location = Location;
            newModel.Size = Size;
            newModel.Scale = Scale;

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
    }
}
