using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawSettingLib.Utils;

namespace DrawSettingLib.SettingModels
{
    public class AreaBoxModel : Notifier
    {
        public AreaBoxModel()
        {
			Length = "";
			Width = "";
			Height = "";
			Margin = new AreaMarginModel();
        }

		private string _Length;
		public string Length
		{
			get { return _Length; }
			set
			{
				_Length = value;
				OnPropertyChanged(nameof(Length));
			}
		}

		private string _Width;
		public string Width
		{
			get { return _Width; }
			set
			{
				_Width = value;
				OnPropertyChanged(nameof(Width));
			}
		}

		private string _Height;
		public string Height
		{
			get { return _Height; }
			set
			{
				_Height = value;
				OnPropertyChanged(nameof(Height));
			}
		}

		private AreaMarginModel _Margin;
		public AreaMarginModel Margin
		{
			get { return _Margin; }
			set
			{
				_Margin = value;
				OnPropertyChanged(nameof(Margin));
			}
		}
	}
}
