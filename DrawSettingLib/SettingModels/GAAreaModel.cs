using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DrawSettingLib.Utils;

namespace DrawSettingLib.SettingModels
{
    public class GAAreaModel : Notifier
    {
        public GAAreaModel()
        {
			ViewPortMain = new AreaBoxModel();
			ShellCourse = new AreaBoxModel();
			NozzleLeader = new AreaBoxModel();
			Dimension = new AreaBoxModel();
			MainAssembly = new AreaBoxModel();

		}

		private AreaBoxModel _ViewPortMain;
		public AreaBoxModel ViewPortMain
		{
			get { return _ViewPortMain; }
			set
			{
				_ViewPortMain = value;
				OnPropertyChanged(nameof(ViewPortMain));
			}
		}

		private AreaBoxModel _ShellCourse;
		public AreaBoxModel ShellCourse
		{
			get { return _ShellCourse; }
			set
			{
				_ShellCourse = value;
				OnPropertyChanged(nameof(ShellCourse));
			}
		}

		private AreaBoxModel _NozzleLeader;
		public AreaBoxModel NozzleLeader
		{
			get { return _NozzleLeader; }
			set
			{
				_NozzleLeader = value;
				OnPropertyChanged(nameof(NozzleLeader));
			}
		}

		private AreaBoxModel _Dimension;
		public AreaBoxModel Dimension
		{
			get { return _Dimension; }
			set
			{
				_Dimension = value;
				OnPropertyChanged(nameof(Dimension));
			}
		}

		private AreaBoxModel _MainAssembly;
		public AreaBoxModel MainAssembly
		{
			get { return _MainAssembly; }
			set
			{
				_MainAssembly = value;
				OnPropertyChanged(nameof(MainAssembly));
			}
		}


	}
}
