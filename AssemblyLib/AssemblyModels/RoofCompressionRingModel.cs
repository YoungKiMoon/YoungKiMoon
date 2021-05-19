﻿using AssemblyLib.Utils;using System;using System.Collections.Generic;using System.Collections.ObjectModel;using System.Linq;using System.Text;using System.Threading.Tasks;namespace AssemblyLib.AssemblyModels{	public class RoofCompressionRingModel : Notifier	{		public RoofCompressionRingModel()		{					RoofSlope = "";			DomeRadiusRatio = "";			CompressionRingType = "";			Material = "";			AngleSize = "";			ThicknessT1 = "";			WidthB = "";			OutsideProjectionA = "";			OverlapOfRoofAndCompRingC = "";			ThicknessC = "";			WidthD = "";			ShellThicknessThickenedT1 = "";			ShellWidthThickenedA = "";			DistanceFromShellTopCourseB = "";		}				private string _RoofSlope;		public string RoofSlope			{				get { return _RoofSlope; }				set				{					_RoofSlope = value;					OnPropertyChanged(nameof(RoofSlope));				}			}				private string _DomeRadiusRatio;		public string DomeRadiusRatio			{				get { return _DomeRadiusRatio; }				set				{					_DomeRadiusRatio = value;					OnPropertyChanged(nameof(DomeRadiusRatio));				}			}				private string _CompressionRingType;		public string CompressionRingType			{				get { return _CompressionRingType; }				set				{					_CompressionRingType = value;					OnPropertyChanged(nameof(CompressionRingType));				}			}				private string _Material;		public string Material			{				get { return _Material; }				set				{					_Material = value;					OnPropertyChanged(nameof(Material));				}			}				private string _AngleSize;		public string AngleSize			{				get { return _AngleSize; }				set				{					_AngleSize = value;					OnPropertyChanged(nameof(AngleSize));				}			}				private string _ThicknessT1;		public string ThicknessT1			{				get { return _ThicknessT1; }				set				{					_ThicknessT1 = value;					OnPropertyChanged(nameof(ThicknessT1));				}			}				private string _WidthB;		public string WidthB			{				get { return _WidthB; }				set				{					_WidthB = value;					OnPropertyChanged(nameof(WidthB));				}			}				private string _OutsideProjectionA;		public string OutsideProjectionA			{				get { return _OutsideProjectionA; }				set				{					_OutsideProjectionA = value;					OnPropertyChanged(nameof(OutsideProjectionA));				}			}				private string _OverlapOfRoofAndCompRingC;		public string OverlapOfRoofAndCompRingC			{				get { return _OverlapOfRoofAndCompRingC; }				set				{					_OverlapOfRoofAndCompRingC = value;					OnPropertyChanged(nameof(OverlapOfRoofAndCompRingC));				}			}				private string _ThicknessC;		public string ThicknessC			{				get { return _ThicknessC; }				set				{					_ThicknessC = value;					OnPropertyChanged(nameof(ThicknessC));				}			}				private string _WidthD;		public string WidthD			{				get { return _WidthD; }				set				{					_WidthD = value;					OnPropertyChanged(nameof(WidthD));				}			}				private string _ShellThicknessThickenedT1;		public string ShellThicknessThickenedT1			{				get { return _ShellThicknessThickenedT1; }				set				{					_ShellThicknessThickenedT1 = value;					OnPropertyChanged(nameof(ShellThicknessThickenedT1));				}			}				private string _ShellWidthThickenedA;		public string ShellWidthThickenedA			{				get { return _ShellWidthThickenedA; }				set				{					_ShellWidthThickenedA = value;					OnPropertyChanged(nameof(ShellWidthThickenedA));				}			}				private string _DistanceFromShellTopCourseB;		public string DistanceFromShellTopCourseB			{				get { return _DistanceFromShellTopCourseB; }				set				{					_DistanceFromShellTopCourseB = value;					OnPropertyChanged(nameof(DistanceFromShellTopCourseB));				}			}			}}