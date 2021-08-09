
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawSettingLib.Utils;

namespace DrawSettingLib.SettingModels
{
    public class SizeModel : Notifier
    {
        public SizeModel()
        {
			Width = 0;
			Height = 0;

		}
		private double _Width;
		public double Width
		{
			get { return _Width; }
			set
			{
				_Width = value;
				OnPropertyChanged(nameof(Width));
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
	}
}
