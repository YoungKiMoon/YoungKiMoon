using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DrawSettingLib.Utils;

namespace DrawSettingLib.SettingModels
{
    public class GADrawAreaModel : Notifier
    {
        public GADrawAreaModel()
        {
			ViewPortMain = "";
			ShellCourse = "";
			NozzleLeader = "";
			Dimension = "";
			MainAssembly = "";
			InsideLeft = "";
			OutsideRight="";
		}

		private string _ViewPortMain;
		public string ViewPortMain
		{
			get { return _ViewPortMain; }
			set
			{
				_ViewPortMain = value;
				OnPropertyChanged(nameof(ViewPortMain));
			}
		}

		private string _ShellCourse;
		public string ShellCourse
		{
			get { return _ShellCourse; }
			set
			{
				_ShellCourse = value;
				OnPropertyChanged(nameof(ShellCourse));
			}
		}

		private string _NozzleLeader;
		public string NozzleLeader
		{
			get { return _NozzleLeader; }
			set
			{
				_NozzleLeader = value;
				OnPropertyChanged(nameof(NozzleLeader));
			}
		}

		private string _Dimension;
		public string Dimension
		{
			get { return _Dimension; }
			set
			{
				_Dimension = value;
				OnPropertyChanged(nameof(Dimension));
			}
		}

		private string _MainAssembly;
		public string MainAssembly
		{
			get { return _MainAssembly; }
			set
			{
				_MainAssembly = value;
				OnPropertyChanged(nameof(MainAssembly));
			}
		}

		private string _InsideLeft;
		public string InsideLeft
		{
			get { return _InsideLeft; }
			set
			{
				_InsideLeft = value;
				OnPropertyChanged(nameof(InsideLeft));
			}
		}

		private string _OutsideRight;
		public string OutsideRight
		{
			get { return _OutsideRight; }
			set
			{
				_OutsideRight = value;
				OnPropertyChanged(nameof(OutsideRight));
			}
		}
	}
}
