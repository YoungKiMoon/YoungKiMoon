using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DrawSettingLib.Utils;
namespace DrawSettingLib.SettingModels
{
    public class AreaMarginModel : Notifier
    {
        public AreaMarginModel()
        {
			InnerMargin = 0;
			OuterMargin = 0;

		}

		private double _InnerMargin;
		public double InnerMargin
		{
			get { return _InnerMargin; }
			set
			{
				_InnerMargin = value;
				OnPropertyChanged(nameof(InnerMargin));
			}
		}

		private double _OuterMargin;
		public double OuterMargin
		{
			get { return _OuterMargin; }
			set
			{
				_OuterMargin = value;
				OnPropertyChanged(nameof(OuterMargin));
			}
		}
	}
}
