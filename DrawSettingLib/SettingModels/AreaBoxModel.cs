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
			AreaSize = new SizeModel();
			BoxSize = new SizeModel();
			Margin = new AreaMarginModel();
        }

		private SizeModel _AreaSize;
		public SizeModel AreaSize
		{
			get { return _AreaSize; }
			set
			{
				_AreaSize = value;
				OnPropertyChanged(nameof(AreaSize));
			}
		}

		private SizeModel _BoxSize;
		public SizeModel BoxSize
		{
			get { return _BoxSize; }
			set
			{
				_BoxSize = value;
				OnPropertyChanged(nameof(BoxSize));
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
