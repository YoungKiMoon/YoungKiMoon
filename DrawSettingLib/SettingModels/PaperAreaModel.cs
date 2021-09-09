using DrawSettingLib.Commons;
using DrawSettingLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingModels
{
    public class PaperAreaModel : Notifier
    {
        public PaperAreaModel()
        {
			Name = PAPERMAIN_TYPE.NotSet;
			DWGName = PAPERMAIN_TYPE.NotSet;
			SubName = PAPERSUB_TYPE.NotSet;
			Location = new PointModel();
			Size = new PointModel();

			ReferencePoint = new PointModel();
			TargetPoint = new PointModel();

			ModelCenterLocation = new PointModel();
			ScaleValue = 1;

			// Grid
			Column = 1;
			Row = 1;
			ColumnSpan = 1;
			RowSpan = 1;
			IsFix = false;
			IsRepeat = false;
			GroupName = "";

			TitleName = "";
			TitleSubName = "";


			// Scale Value =0 일 때 사용
			otherHeight = 0;
			otherWidth = 0;

			// View ID
			viewID = 0;

			// Page
			Page = 1;

			visible = false;
		}

		public PaperAreaModel CustomClone()
        {
			PaperAreaModel newModel = new PaperAreaModel();

			newModel.DWGName = DWGName;

			newModel.Name = Name;
			newModel.SubName = SubName;
			newModel.Location.X = Location.X;
			newModel.Location.Y = Location.Y;
			newModel.Size.X = Size.X;
			newModel.Size.Y = Size.Y;

			newModel.ReferencePoint.X = ReferencePoint.X;
			newModel.ReferencePoint.Y = ReferencePoint.Y;
			newModel.TargetPoint.X = TargetPoint.X;
			newModel.TargetPoint.Y = TargetPoint.Y;

			newModel.ModelCenterLocation.X = ModelCenterLocation.X;
			newModel.ModelCenterLocation.Y = ModelCenterLocation.Y;

			newModel.ScaleValue = ScaleValue;

			newModel.Column = Column;
			newModel.Row = Row;
			newModel.ColumnSpan = ColumnSpan;
			newModel.RowSpan = RowSpan;
			newModel.IsFix = IsFix;
			newModel.IsRepeat = IsRepeat;
			newModel.GroupName = GroupName;

			newModel.TitleName = TitleName;
			newModel.TitleSubName = TitleSubName;

			newModel.otherHeight = otherHeight;
			newModel.otherWidth = otherWidth;

			newModel.viewID = viewID;
			newModel.Page = Page;


			newModel.visible = visible;

			return newModel;
		}


		public bool visible { get; set; }

		public double viewID { get; set; }

		public  double otherHeight { get; set; }
		public double otherWidth { get; set; }


		public double Page { get; set; }

		private PAPERMAIN_TYPE _Name;
		public PAPERMAIN_TYPE Name
		{
			get { return _Name; }
			set
			{
				_Name = value;
				OnPropertyChanged(nameof(Name));
			}
		}

		private PAPERMAIN_TYPE _DWGName;
		public PAPERMAIN_TYPE DWGName
		{
			get { return _DWGName; }
			set
			{
				_DWGName = value;
				OnPropertyChanged(nameof(DWGName));
			}
		}

		public PAPERSUB_TYPE SubName { get; set; }

		public PointModel ReferencePoint { get; set; }
		public PointModel TargetPoint { get; set; }

		private PointModel _Location;
		public PointModel Location
		{
			get { return _Location; }
			set
			{
				_Location = value;
				OnPropertyChanged(nameof(Location));
			}
		}

		private PointModel _Size;
		public PointModel Size
		{
			get { return _Size; }
			set
			{
				_Size = value;
				OnPropertyChanged(nameof(Size));
			}
		}



		private PointModel _ModelCenterLocation;
		public PointModel ModelCenterLocation
		{
			get { return _ModelCenterLocation; }
			set
			{
				_ModelCenterLocation = value;
				OnPropertyChanged(nameof(ModelCenterLocation));
			}
		}

		private double _ScaleValue;
		public double ScaleValue
		{
			get { return _ScaleValue; }
			set
			{
				_ScaleValue = value;
				OnPropertyChanged(nameof(ScaleValue));
			}
		}

		private double _Column;
		public double Column
		{
			get { return _Column; }
			set
			{
				_Column = value;
				OnPropertyChanged(nameof(Column));
			}
		}
		private double _Row;
		public double Row
		{
			get { return _Row; }
			set
			{
				_Row = value;
				OnPropertyChanged(nameof(Row));
			}
		}

		private double _ColumnSpan;
		public double ColumnSpan
		{
			get { return _ColumnSpan; }
			set
			{
				_ColumnSpan = value;
				OnPropertyChanged(nameof(ColumnSpan));
			}
		}
		private double _RowSpan;
		public double RowSpan
		{
			get { return _RowSpan; }
			set
			{
				_RowSpan = value;
				OnPropertyChanged(nameof(RowSpan));
			}
		}

		
		private bool _IsFix;
		public bool IsFix
		{
			get { return _IsFix; }
			set
			{
				_IsFix = value;
				OnPropertyChanged(nameof(IsFix));
			}
		}

		private bool _IsRepeat;
		public bool IsRepeat
		{
			get { return _IsRepeat; }
			set
			{
				_IsRepeat = value;
				OnPropertyChanged(nameof(IsRepeat));
			}
		}

		private string _GroupName;
		public string GroupName
		{
			get { return _GroupName; }
			set
			{
				_GroupName = value;
				OnPropertyChanged(nameof(GroupName));
			}
		}

		private double _Priority;
		public double Priority
		{
			get { return _Priority; }
			set
			{
				_Priority = value;
				OnPropertyChanged(nameof(Priority));
			}
		}

		private string _TitleName;
		public string TitleName
		{
			get { return _TitleName; }
			set
			{
				_TitleName = value;
				OnPropertyChanged(nameof(TitleName));
			}
		}

		private string _TitleSubName;
		public string TitleSubName
		{
			get { return _TitleSubName; }
			set
			{
				_TitleSubName = value;
				OnPropertyChanged(nameof(TitleSubName));
			}
		}
	}
}
