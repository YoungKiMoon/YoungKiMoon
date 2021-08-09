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

			CenterPoint = new PointModel();
			ViewCenterPoint = new PointModel();
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


		// Center Point
		private PointModel _CenterPoint;
		public PointModel CenterPoint
		{
			get { return _CenterPoint; }
			set
			{
				_CenterPoint = value;
				OnPropertyChanged(nameof(CenterPoint));
			}
		}

		private PointModel _ViewCenterPoint;
		public PointModel ViewCenterPoint
		{
			get { return _ViewCenterPoint; }
			set
			{
				_ViewCenterPoint = value;
				OnPropertyChanged(nameof(ViewCenterPoint));
			}
		}



		public void SetMainAssemblySize(double refX,double refY, double selWidth, double selHeight)
        {
			MainAssembly.AreaSize.Width = selWidth;
			MainAssembly.AreaSize.Height = selHeight;

			UpdateArea();

			SetCenterPoint(refX + selWidth / 2, refY + selHeight / 2);

		}

		public void SetCenterPoint(double selX, double selY)
        {
			CenterPoint.X= selX;
			CenterPoint.Y= selY;

			ViewCenterPoint.X = CenterPoint.X+ ShellCourse.AreaSize.Width/2;
			ViewCenterPoint.Y= CenterPoint.Y;
		}

		public void UpdateArea()
        {
			double assemblyWidth = MainAssembly.AreaSize.Width;
			double assemblyHeight = MainAssembly.AreaSize.Height;

			MainAssembly.BoxSize.Width = assemblyWidth;
			MainAssembly.BoxSize.Height = assemblyHeight;

			assemblyWidth += Dimension.AreaSize.Width * 2;
			assemblyHeight+= Dimension.AreaSize.Height * 2;

			Dimension.BoxSize.Width = assemblyWidth;
			Dimension.BoxSize.Height = assemblyHeight;

			assemblyWidth += NozzleLeader.AreaSize.Width * 2;
			assemblyHeight += NozzleLeader.AreaSize.Height * 2;

			NozzleLeader.BoxSize.Width = assemblyWidth;
			NozzleLeader.BoxSize.Height = assemblyHeight;

			assemblyWidth += ShellCourse.AreaSize.Width;
			assemblyHeight += ShellCourse.AreaSize.Height;

			ShellCourse.BoxSize.Width = assemblyWidth;
			ShellCourse.BoxSize.Height = assemblyHeight;

			assemblyWidth += ViewPortMain.AreaSize.Width;
			assemblyHeight += ViewPortMain.AreaSize.Height;

			ViewPortMain.BoxSize.Width = assemblyWidth;
			ViewPortMain.BoxSize.Height = assemblyHeight;


		}
	}
}
