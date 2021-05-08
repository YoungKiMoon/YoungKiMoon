using DesignLib.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DesignLib.DesignModels
{
    public class DesignStepModel : Notifier
    {
        public DesignStepModel()
        {
			DisplayName = "";
			StepName = "";

			IsRequired = false;
			IsMulti = false;

			Value = false;
			ValueString = "";

			Assembly = null;
			Options = new ObservableCollection<DesignStepModel>();
		}
		public DesignStepModel(string selDisplayName, string selStepName, bool selMulti, bool selRequired = false, bool selValue = false, string selValueString = "")
        {
			DisplayName = selDisplayName;
			StepName = selStepName;
			if (StepName == "")
				StepName = selDisplayName;

			IsMulti = selMulti;
			IsRequired = selRequired;

			Value = selValue;
			ValueString = selValueString;

			Assembly = null;
			Options = new ObservableCollection<DesignStepModel>();
		}



		public string DisplayName
		{
			get { return _DisplayName; }
			set
			{
				_DisplayName = value;
				OnPropertyChanged(nameof(DisplayName));
			}
		}
		private string _DisplayName;

		public string StepName
		{
			get { return _StepName; }
			set
			{
				_StepName = value;
				OnPropertyChanged(nameof(StepName));
			}
		}
		private string _StepName;

		public bool Value
		{
			get { return _Value; }
			set
			{
				_Value = value;
				OnPropertyChanged(nameof(Value));
			}
		}
		private bool _Value;

		public string ValueString
		{
			get { return _ValueString; }
			set
			{
				_ValueString = value;
				OnPropertyChanged(nameof(ValueString));
			}
		}
		private string _ValueString;

		public bool IsRequired
		{
			get { return _IsRequired; }
			set
			{
				_IsRequired = value;
				OnPropertyChanged(nameof(IsRequired));
			}
		}
		private bool _IsRequired;

		public bool IsMulti
		{
			get { return _IsMulti; }
			set
			{
				_IsMulti = value;
				OnPropertyChanged(nameof(IsMulti));
			}
		}
		private bool _IsMulti;

		public DesignAssemblyModel Assembly
		{
			get { return _Assembly; }
			set
			{
				_Assembly = value;
				OnPropertyChanged(nameof(Assembly));
			}
		}
		private DesignAssemblyModel _Assembly;

		public ObservableCollection<DesignStepModel> Options
		{
			get { return _Options; }
			set
			{
				_Options = value;
				OnPropertyChanged(nameof(Options));
			}
		}
		private ObservableCollection<DesignStepModel> _Options;
	}
}
