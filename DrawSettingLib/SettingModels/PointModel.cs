using DrawSettingLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingModels
{
    public class PointModel : Notifier
    {
        public PointModel()
        {
			X = 0;
			Y = 0;
        }
		public PointModel(double selX, double selY)
		{
			X = selX;
			Y = selY;
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
	}
}
