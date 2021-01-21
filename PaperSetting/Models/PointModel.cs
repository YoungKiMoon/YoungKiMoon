using PaperSetting.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaperSetting.Models
{
    public class PointModel : Notifier,ICloneable
    {
        public PointModel()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public object Clone()
        {
            PointModel newModel = new PointModel();
            newModel.X = X;
            newModel.Y = Y;
            newModel.Z = Z;
            return newModel;
        }

        private double _X;
        public double X
        {
            get { return _X; }
            set
            {
                _X = value;
                OnPropertyChanged(nameof(X));
            }
        }

        private double _Y;
        public double Y
        {
            get { return _Y; }
            set
            {
                _Y = value;
                OnPropertyChanged(nameof(Y));
            }
        }

        private double _Z;
        public double Z
        {
            get { return _Z; }
            set
            {
                _Z = value;
                OnPropertyChanged(nameof(Z));
            }
        }
    }
}
