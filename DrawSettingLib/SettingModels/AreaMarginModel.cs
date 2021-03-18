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
			InnerMargin = "";
			OuterMargin = "";

		}

		private string _InnerMargin;
		public string InnerMargin
		{
			get { return _InnerMargin; }
			set
			{
				_InnerMargin = value;
				OnPropertyChanged(nameof(InnerMargin));
			}
		}

		private string _OuterMargin;
		public string OuterMargin
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
