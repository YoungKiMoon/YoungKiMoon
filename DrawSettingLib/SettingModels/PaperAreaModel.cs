using DrawSettingLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingModels
{
    public class PaperAreaModel : Notifier
    {
        public PaperAreaModel()
        {
			Name = "";
			Location = new PointModel();
			Size = new PointModel();

			ModelLocation = new PointModel();
			ScaleValue = new double();
		}

		private string _Name;
		public string Name
		{
			get { return _Name; }
			set
			{
				_Name = value;
				OnPropertyChanged(nameof(Name));
			}
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

		private PointModel _Size;
		public PointModel Size
		{
			get { return _Size; }
			set
			{
				_Size = value;
				OnPropertyChanged(nameof(Size));
			}
		}



		private PointModel _ModelLocation;
		public PointModel ModelLocation
		{
			get { return _ModelLocation; }
			set
			{
				_ModelLocation = value;
				OnPropertyChanged(nameof(ModelLocation));
			}
		}

		private double _ScaleValue;
		public double ScaleValue
		{
			get { return _ScaleValue; }
			set
			{
				_ScaleValue = value;
				OnPropertyChanged(nameof(ScaleValue));
			}
		}

	}
}
