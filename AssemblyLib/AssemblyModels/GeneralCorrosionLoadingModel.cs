﻿using AssemblyLib.Utils;using System;using System.Collections.Generic;using System.Collections.ObjectModel;using System.Linq;using System.Text;using System.Threading.Tasks;namespace AssemblyLib.AssemblyModels{	public class GeneralCorrosionLoadingModel : Notifier	{		public GeneralCorrosionLoadingModel()		{					ShellPlate = "";			RoofPlate = "";			BottomPlate = "";			AnnularPlate = "";			Nozzle = "";			StructureEachSide = "";			ColumnEachSide = "";			DeskPlateUpper = "";			DeskPlateLower = "";			WindShear = "";			WindMoment = "";			SeismicShear = "";			SeismicMoment = "";			CounterBalancingWeight = "";		}				private string _ShellPlate;		public string ShellPlate			{				get { return _ShellPlate; }				set				{					_ShellPlate = value;					OnPropertyChanged(nameof(ShellPlate));				}			}				private string _RoofPlate;		public string RoofPlate			{				get { return _RoofPlate; }				set				{					_RoofPlate = value;					OnPropertyChanged(nameof(RoofPlate));				}			}				private string _BottomPlate;		public string BottomPlate			{				get { return _BottomPlate; }				set				{					_BottomPlate = value;					OnPropertyChanged(nameof(BottomPlate));				}			}				private string _AnnularPlate;		public string AnnularPlate			{				get { return _AnnularPlate; }				set				{					_AnnularPlate = value;					OnPropertyChanged(nameof(AnnularPlate));				}			}				private string _Nozzle;		public string Nozzle			{				get { return _Nozzle; }				set				{					_Nozzle = value;					OnPropertyChanged(nameof(Nozzle));				}			}				private string _StructureEachSide;		public string StructureEachSide			{				get { return _StructureEachSide; }				set				{					_StructureEachSide = value;					OnPropertyChanged(nameof(StructureEachSide));				}			}				private string _ColumnEachSide;		public string ColumnEachSide			{				get { return _ColumnEachSide; }				set				{					_ColumnEachSide = value;					OnPropertyChanged(nameof(ColumnEachSide));				}			}				private string _DeskPlateUpper;		public string DeskPlateUpper			{				get { return _DeskPlateUpper; }				set				{					_DeskPlateUpper = value;					OnPropertyChanged(nameof(DeskPlateUpper));				}			}				private string _DeskPlateLower;		public string DeskPlateLower			{				get { return _DeskPlateLower; }				set				{					_DeskPlateLower = value;					OnPropertyChanged(nameof(DeskPlateLower));				}			}				private string _WindShear;		public string WindShear			{				get { return _WindShear; }				set				{					_WindShear = value;					OnPropertyChanged(nameof(WindShear));				}			}				private string _WindMoment;		public string WindMoment			{				get { return _WindMoment; }				set				{					_WindMoment = value;					OnPropertyChanged(nameof(WindMoment));				}			}				private string _SeismicShear;		public string SeismicShear			{				get { return _SeismicShear; }				set				{					_SeismicShear = value;					OnPropertyChanged(nameof(SeismicShear));				}			}				private string _SeismicMoment;		public string SeismicMoment			{				get { return _SeismicMoment; }				set				{					_SeismicMoment = value;					OnPropertyChanged(nameof(SeismicMoment));				}			}				private string _CounterBalancingWeight;		public string CounterBalancingWeight			{				get { return _CounterBalancingWeight; }				set				{					_CounterBalancingWeight = value;					OnPropertyChanged(nameof(CounterBalancingWeight));				}			}			}}