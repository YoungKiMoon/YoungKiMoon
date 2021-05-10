using DesignLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignLib.DesignModels
{
    public class DesignInformationModel : Notifier
    {
		public DesignInformationModel()
        {
			TypeName = "";
		}
		public DesignInformationModel(string selType)
        {
			TypeName = selType;
        }
		public string TypeName
		{
			get { return _TypeName; }
			set
			{
				_TypeName = value;
				OnPropertyChanged(nameof(TypeName));
			}
		}
		private string _TypeName;
	}
}
