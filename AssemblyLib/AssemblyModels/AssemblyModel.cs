﻿using AssemblyLib.Utils;using System;using System.Collections.Generic;using System.Collections.ObjectModel;using System.Linq;using System.Text;using System.Threading.Tasks;namespace AssemblyLib.AssemblyModels{	public class AssemblyModel : Notifier	{		public AssemblyModel()		{					GeneralDesignData = new GeneralDesignDataModel();			GeneralCapacityWeight = new ObservableCollection<GeneralCapacityWeightModel>();			GeneralMomentAndShearForceAtBase = new ObservableCollection<GeneralMomentAndShearForceAtBaseModel>();			GeneralMaterialSpecs = new ObservableCollection<GeneralMaterialSpecsModel>();			GeneralCorrosionAllowance = new ObservableCollection<GeneralCorrosionAllowanceModel>();			GeneralEarquake = new ObservableCollection<GeneralEarthquakeModel>();			ShellInput = new ObservableCollection<ShellInputModel>();			ShellOuput = new ObservableCollection<ShellOutputModel>();			RoofInput = new ObservableCollection<RoofInputModel>();			RoofOutput = new ObservableCollection<RoofOutputModel>();			BottomInput = new ObservableCollection<BottomInputModel>();			BottomOutput = new ObservableCollection<BottomOutputModel>();			NozzleInputModel = new ObservableCollection<NozzleInputModel>();			WindGirderInput = new ObservableCollection<WindGirderInputModel>();			WindGirderOutput = new ObservableCollection<WindGirderOutputModel>();			InsulationInput = new ObservableCollection<InsulationInputModel>();			AngleInput = new ObservableCollection<EqualAngleSizeModel>();		}				private GeneralDesignDataModel _GeneralDesignData;		public GeneralDesignDataModel GeneralDesignData			{				get { return _GeneralDesignData; }				set				{					_GeneralDesignData = value;					OnPropertyChanged(nameof(GeneralDesignData));				}			}				private ObservableCollection<GeneralCapacityWeightModel> _GeneralCapacityWeight;		public ObservableCollection<GeneralCapacityWeightModel> GeneralCapacityWeight			{				get { return _GeneralCapacityWeight; }				set				{					_GeneralCapacityWeight = value;					OnPropertyChanged(nameof(GeneralCapacityWeight));				}			}				private ObservableCollection<GeneralMomentAndShearForceAtBaseModel> _GeneralMomentAndShearForceAtBase;		public ObservableCollection<GeneralMomentAndShearForceAtBaseModel> GeneralMomentAndShearForceAtBase			{				get { return _GeneralMomentAndShearForceAtBase; }				set				{					_GeneralMomentAndShearForceAtBase = value;					OnPropertyChanged(nameof(GeneralMomentAndShearForceAtBase));				}			}				private ObservableCollection<GeneralMaterialSpecsModel> _GeneralMaterialSpecs;		public ObservableCollection<GeneralMaterialSpecsModel> GeneralMaterialSpecs			{				get { return _GeneralMaterialSpecs; }				set				{					_GeneralMaterialSpecs = value;					OnPropertyChanged(nameof(GeneralMaterialSpecs));				}			}				private ObservableCollection<GeneralCorrosionAllowanceModel> _GeneralCorrosionAllowance;		public ObservableCollection<GeneralCorrosionAllowanceModel> GeneralCorrosionAllowance			{				get { return _GeneralCorrosionAllowance; }				set				{					_GeneralCorrosionAllowance = value;					OnPropertyChanged(nameof(GeneralCorrosionAllowance));				}			}				private ObservableCollection<GeneralEarthquakeModel> _GeneralEarquake;		public ObservableCollection<GeneralEarthquakeModel> GeneralEarquake			{				get { return _GeneralEarquake; }				set				{					_GeneralEarquake = value;					OnPropertyChanged(nameof(GeneralEarquake));				}			}				private ObservableCollection<ShellInputModel> _ShellInput;		public ObservableCollection<ShellInputModel> ShellInput			{				get { return _ShellInput; }				set				{					_ShellInput = value;					OnPropertyChanged(nameof(ShellInput));				}			}				private ObservableCollection<ShellOutputModel> _ShellOuput;		public ObservableCollection<ShellOutputModel> ShellOuput			{				get { return _ShellOuput; }				set				{					_ShellOuput = value;					OnPropertyChanged(nameof(ShellOuput));				}			}				private ObservableCollection<RoofInputModel> _RoofInput;		public ObservableCollection<RoofInputModel> RoofInput			{				get { return _RoofInput; }				set				{					_RoofInput = value;					OnPropertyChanged(nameof(RoofInput));				}			}				private ObservableCollection<RoofOutputModel> _RoofOutput;		public ObservableCollection<RoofOutputModel> RoofOutput			{				get { return _RoofOutput; }				set				{					_RoofOutput = value;					OnPropertyChanged(nameof(RoofOutput));				}			}				private ObservableCollection<BottomInputModel> _BottomInput;		public ObservableCollection<BottomInputModel> BottomInput			{				get { return _BottomInput; }				set				{					_BottomInput = value;					OnPropertyChanged(nameof(BottomInput));				}			}				private ObservableCollection<BottomOutputModel> _BottomOutput;		public ObservableCollection<BottomOutputModel> BottomOutput			{				get { return _BottomOutput; }				set				{					_BottomOutput = value;					OnPropertyChanged(nameof(BottomOutput));				}			}				private ObservableCollection<NozzleInputModel> _NozzleInputModel;		public ObservableCollection<NozzleInputModel> NozzleInputModel			{				get { return _NozzleInputModel; }				set				{					_NozzleInputModel = value;					OnPropertyChanged(nameof(NozzleInputModel));				}			}				private ObservableCollection<WindGirderInputModel> _WindGirderInput;		public ObservableCollection<WindGirderInputModel> WindGirderInput			{				get { return _WindGirderInput; }				set				{					_WindGirderInput = value;					OnPropertyChanged(nameof(WindGirderInput));				}			}				private ObservableCollection<WindGirderOutputModel> _WindGirderOutput;		public ObservableCollection<WindGirderOutputModel> WindGirderOutput			{				get { return _WindGirderOutput; }				set				{					_WindGirderOutput = value;					OnPropertyChanged(nameof(WindGirderOutput));				}			}				private ObservableCollection<InsulationInputModel> _InsulationInput;		public ObservableCollection<InsulationInputModel> InsulationInput			{				get { return _InsulationInput; }				set				{					_InsulationInput = value;					OnPropertyChanged(nameof(InsulationInput));				}			}				private ObservableCollection<EqualAngleSizeModel> _AngleInput;		public ObservableCollection<EqualAngleSizeModel> AngleInput			{				get { return _AngleInput; }				set				{					_AngleInput = value;					OnPropertyChanged(nameof(AngleInput));				}			}						public void CreateSampleAssembly()		{			GeneralCapacityWeightModel GeneralCapacityWeight11 = new GeneralCapacityWeightModel();			GeneralCapacityWeight11.Empty="1";			GeneralCapacityWeight11.Operating="2";			GeneralCapacityWeight11.FullOfWater="3";			GeneralCapacityWeight11.Insulation="4";			GeneralCapacityWeight11.PlatformLadder="5";			GeneralCapacityWeight11.Others="6";			GeneralCapacityWeight11.Liquid="7";			GeneralCapacityWeight11.PaintingAreaInt="8";			GeneralCapacityWeight11.PaintingAreaExt="9";			GeneralCapacityWeight11.NominalCapacity="10";			GeneralCapacityWeight11.WorkingCapacity="11";			GeneralCapacityWeight11.NetWorkingCapacity="12";			GeneralCapacityWeight.Add(GeneralCapacityWeight11);						GeneralCapacityWeightModel GeneralCapacityWeight12 = new GeneralCapacityWeightModel();			GeneralCapacityWeight12.Empty="22";			GeneralCapacityWeight12.Operating="23";			GeneralCapacityWeight12.FullOfWater="24";			GeneralCapacityWeight12.Insulation="25";			GeneralCapacityWeight12.PlatformLadder="26";			GeneralCapacityWeight12.Others="27";			GeneralCapacityWeight12.Liquid="28";			GeneralCapacityWeight12.PaintingAreaInt="29";			GeneralCapacityWeight12.PaintingAreaExt="30";			GeneralCapacityWeight12.NominalCapacity="31";			GeneralCapacityWeight12.WorkingCapacity="32";			GeneralCapacityWeight12.NetWorkingCapacity="33";			GeneralCapacityWeight.Add(GeneralCapacityWeight12);						GeneralMomentAndShearForceAtBaseModel GeneralMomentAndShearForceAtBase11 = new GeneralMomentAndShearForceAtBaseModel();			GeneralMomentAndShearForceAtBase11.WindMoment="5";			GeneralMomentAndShearForceAtBase11.SeismicMoment="6";			GeneralMomentAndShearForceAtBase11.ShearForceEmpty="7";			GeneralMomentAndShearForceAtBase11.ShearForceOperating="8";			GeneralMomentAndShearForceAtBase.Add(GeneralMomentAndShearForceAtBase11);						ShellInputModel ShellInput11 = new ShellInputModel();			ShellInput11.CAOfShell="3";			ShellInput11.CourseCount="6";			ShellInput11.PlateCnt="12";			ShellInput.Add(ShellInput11);						ShellOutputModel ShellOuput11 = new ShellOutputModel();			ShellOuput11.CourseNo="1";			ShellOuput11.MinThk="17";			ShellOuput11.StartPoint="2830.4";			ShellOuput11.Width="2430";			ShellOuput11.Length="8491.2";			ShellOuput11.RepeatNo="1";			ShellOuput.Add(ShellOuput11);						ShellOutputModel ShellOuput12 = new ShellOutputModel();			ShellOuput12.CourseNo="2";			ShellOuput12.MinThk="13";			ShellOuput12.StartPoint="5660.8";			ShellOuput12.Width="2430";			ShellOuput12.Length="8491.2";			ShellOuput12.RepeatNo="1";			ShellOuput.Add(ShellOuput12);						ShellOutputModel ShellOuput13 = new ShellOutputModel();			ShellOuput13.CourseNo="3";			ShellOuput13.MinThk="11";			ShellOuput13.StartPoint="8491.2";			ShellOuput13.Width="2430";			ShellOuput13.Length="0";			ShellOuput13.RepeatNo="0";			ShellOuput.Add(ShellOuput13);						ShellOutputModel ShellOuput14 = new ShellOutputModel();			ShellOuput14.CourseNo="4";			ShellOuput14.MinThk="9";			ShellOuput14.StartPoint="2830.4";			ShellOuput14.Width="2430";			ShellOuput14.Length="8491.2";			ShellOuput14.RepeatNo="1";			ShellOuput.Add(ShellOuput14);						ShellOutputModel ShellOuput15 = new ShellOutputModel();			ShellOuput15.CourseNo="5";			ShellOuput15.MinThk="8";			ShellOuput15.StartPoint="5660.8";			ShellOuput15.Width="2430";			ShellOuput15.Length="8491.2";			ShellOuput15.RepeatNo="1";			ShellOuput.Add(ShellOuput15);						ShellOutputModel ShellOuput16 = new ShellOutputModel();			ShellOuput16.CourseNo="6";			ShellOuput16.MinThk="6";			ShellOuput16.StartPoint="8491.2";			ShellOuput16.Width="2430";			ShellOuput16.Length="0";			ShellOuput16.RepeatNo="0";			ShellOuput.Add(ShellOuput16);						ShellOutputModel ShellOuput17 = new ShellOutputModel();			ShellOuput17.CourseNo="7";			ShellOuput17.MinThk="";			ShellOuput17.StartPoint="2830.4";			ShellOuput17.Width="2430";			ShellOuput17.Length="8491.2";			ShellOuput17.RepeatNo="1";			ShellOuput.Add(ShellOuput17);						ShellOutputModel ShellOuput18 = new ShellOutputModel();			ShellOuput18.CourseNo="8";			ShellOuput18.MinThk="";			ShellOuput18.StartPoint="5660.8";			ShellOuput18.Width="2290";			ShellOuput18.Length="8491.2";			ShellOuput18.RepeatNo="1";			ShellOuput.Add(ShellOuput18);						RoofInputModel RoofInput11 = new RoofInputModel();			RoofInput11.RoofSlopeOne="6";			RoofInput11.RoofThickness="8";			RoofInput11.CAOfRoof="1.5";			RoofInput11.TopAngleType="b";			RoofInput11.TopAneSize="L 150x150x12";			RoofInput.Add(RoofInput11);						RoofOutputModel RoofOutput11 = new RoofOutputModel();			RoofOutput11.ShellThk="6";			RoofOutput11.TwoTcMax="12";			RoofOutput11.AngleSize="150";			RoofOutput11.AngleThk="12";			RoofOutput11.B="40";			RoofOutput11.RoofAngle="9.462322208";			RoofOutput11.RoofThk="8";			RoofOutput11.WcStartPoint="";			RoofOutput11.Wc="";			RoofOutput11.TwoTsOrTwoTbMax="";			RoofOutput11.RingWidth="";			RoofOutput11.RingThk="";			RoofOutput11.ComprRingWidth="";			RoofOutput11.OutsideProjection="";			RoofOutput11.OverlapOfRoof="";			RoofOutput.Add(RoofOutput11);						BottomInputModel BottomInput11 = new BottomInputModel();			BottomInput11.BottomSlope="120";			BottomInput11.BottomThickness="9";			BottomInput11.CAOfBottomPlate="1.5";			BottomInput11.AnnularPlateReqd="Yes";			BottomInput11.AnnularPlateThickness="12";			BottomInput.Add(BottomInput11);						BottomOutputModel BottomOutput11 = new BottomOutputModel();			BottomOutput11.ShellThk="17";			BottomOutput11.OutSideProjection="69";			BottomOutput11.InsideProjection="500";			BottomOutput11.ODTwo="16290";			BottomOutput11.BottomPlateAngle="0.477453777";			BottomOutput11.OverlapOfAnnular="70";			BottomOutput.Add(BottomOutput11);						EqualAngleSizeModel AngleInput11 = new EqualAngleSizeModel();			AngleInput11.Size="L 50x50x6";			AngleInput11.AB="50";			AngleInput11.t="6";			AngleInput11.R1="6.5";			AngleInput11.R2="4.5";			AngleInput11.CD="14.4";			AngleInput11.E="10";			AngleInput.Add(AngleInput11);						EqualAngleSizeModel AngleInput12 = new EqualAngleSizeModel();			AngleInput12.Size="L 50x50x8";			AngleInput12.AB="50";			AngleInput12.t="8";			AngleInput12.R1="6.5";			AngleInput12.R2="4.5";			AngleInput12.CD="15.2";			AngleInput12.E="10";			AngleInput.Add(AngleInput12);						EqualAngleSizeModel AngleInput13 = new EqualAngleSizeModel();			AngleInput13.Size="L 60x60x4";			AngleInput13.AB="60";			AngleInput13.t="4";			AngleInput13.R1="6.5";			AngleInput13.R2="3";			AngleInput13.CD="16.1";			AngleInput13.E="15";			AngleInput.Add(AngleInput13);						EqualAngleSizeModel AngleInput14 = new EqualAngleSizeModel();			AngleInput14.Size="L 60x60x5";			AngleInput14.AB="60";			AngleInput14.t="5";			AngleInput14.R1="6.5";			AngleInput14.R2="3";			AngleInput14.CD="16.6";			AngleInput14.E="15";			AngleInput.Add(AngleInput14);						EqualAngleSizeModel AngleInput15 = new EqualAngleSizeModel();			AngleInput15.Size="L 60x60x6";			AngleInput15.AB="60";			AngleInput15.t="6";			AngleInput15.R1="6.5";			AngleInput15.R2="4.5";			AngleInput15.CD="16.9";			AngleInput15.E="15";			AngleInput.Add(AngleInput15);						EqualAngleSizeModel AngleInput16 = new EqualAngleSizeModel();			AngleInput16.Size="L 60x60x7";			AngleInput16.AB="60";			AngleInput16.t="7";			AngleInput16.R1="6.5";			AngleInput16.R2="4.5";			AngleInput16.CD="17.3";			AngleInput16.E="15";			AngleInput.Add(AngleInput16);						EqualAngleSizeModel AngleInput17 = new EqualAngleSizeModel();			AngleInput17.Size="L 60x60x9";			AngleInput17.AB="60";			AngleInput17.t="9";			AngleInput17.R1="6.5";			AngleInput17.R2="4.5";			AngleInput17.CD="18.1";			AngleInput17.E="15";			AngleInput.Add(AngleInput17);						EqualAngleSizeModel AngleInput18 = new EqualAngleSizeModel();			AngleInput18.Size="L 65x65x6";			AngleInput18.AB="65";			AngleInput18.t="6";			AngleInput18.R1="8.5";			AngleInput18.R2="4";			AngleInput18.CD="18.1";			AngleInput18.E="15";			AngleInput.Add(AngleInput18);						EqualAngleSizeModel AngleInput19 = new EqualAngleSizeModel();			AngleInput19.Size="L 65x65x8";			AngleInput19.AB="65";			AngleInput19.t="8";			AngleInput19.R1="8.5";			AngleInput19.R2="6";			AngleInput19.CD="18.8";			AngleInput19.E="15";			AngleInput.Add(AngleInput19);						EqualAngleSizeModel AngleInput20 = new EqualAngleSizeModel();			AngleInput20.Size="L 65x65x10";			AngleInput20.AB="65";			AngleInput20.t="10";			AngleInput20.R1="8.5";			AngleInput20.R2="6";			AngleInput20.CD="19.6";			AngleInput20.E="15";			AngleInput.Add(AngleInput20);						EqualAngleSizeModel AngleInput21 = new EqualAngleSizeModel();			AngleInput21.Size="L 70x70x6";			AngleInput21.AB="70";			AngleInput21.t="6";			AngleInput21.R1="8.5";			AngleInput21.R2="4";			AngleInput21.CD="19.4";			AngleInput21.E="15";			AngleInput.Add(AngleInput21);						EqualAngleSizeModel AngleInput22 = new EqualAngleSizeModel();			AngleInput22.Size="L 75x75x6";			AngleInput22.AB="75";			AngleInput22.t="6";			AngleInput22.R1="8.5";			AngleInput22.R2="4";			AngleInput22.CD="20.6";			AngleInput22.E="15";			AngleInput.Add(AngleInput22);						EqualAngleSizeModel AngleInput23 = new EqualAngleSizeModel();			AngleInput23.Size="L 75x75x9";			AngleInput23.AB="75";			AngleInput23.t="9";			AngleInput23.R1="8.5";			AngleInput23.R2="6";			AngleInput23.CD="21.7";			AngleInput23.E="20";			AngleInput.Add(AngleInput23);						EqualAngleSizeModel AngleInput24 = new EqualAngleSizeModel();			AngleInput24.Size="L 75x75x12";			AngleInput24.AB="75";			AngleInput24.t="12";			AngleInput24.R1="8.5";			AngleInput24.R2="6";			AngleInput24.CD="22.9";			AngleInput24.E="20";			AngleInput.Add(AngleInput24);						EqualAngleSizeModel AngleInput25 = new EqualAngleSizeModel();			AngleInput25.Size="L 80x80x6";			AngleInput25.AB="80";			AngleInput25.t="6";			AngleInput25.R1="8.5";			AngleInput25.R2="4";			AngleInput25.CD="21.9";			AngleInput25.E="20";			AngleInput.Add(AngleInput25);						EqualAngleSizeModel AngleInput26 = new EqualAngleSizeModel();			AngleInput26.Size="L 90x90x6";			AngleInput26.AB="90";			AngleInput26.t="6";			AngleInput26.R1="10";			AngleInput26.R2="5";			AngleInput26.CD="24.2";			AngleInput26.E="20";			AngleInput.Add(AngleInput26);						EqualAngleSizeModel AngleInput27 = new EqualAngleSizeModel();			AngleInput27.Size="L 90x90x7";			AngleInput27.AB="90";			AngleInput27.t="7";			AngleInput27.R1="10";			AngleInput27.R2="5";			AngleInput27.CD="24.6";			AngleInput27.E="20";			AngleInput.Add(AngleInput27);						EqualAngleSizeModel AngleInput28 = new EqualAngleSizeModel();			AngleInput28.Size="L 90x90x10";			AngleInput28.AB="90";			AngleInput28.t="10";			AngleInput28.R1="10";			AngleInput28.R2="7";			AngleInput28.CD="25.8";			AngleInput28.E="25";			AngleInput.Add(AngleInput28);						EqualAngleSizeModel AngleInput29 = new EqualAngleSizeModel();			AngleInput29.Size="L 90x90x13";			AngleInput29.AB="90";			AngleInput29.t="13";			AngleInput29.R1="10";			AngleInput29.R2="7";			AngleInput29.CD="26.9";			AngleInput29.E="25";			AngleInput.Add(AngleInput29);						EqualAngleSizeModel AngleInput30 = new EqualAngleSizeModel();			AngleInput30.Size="L 100x100x7";			AngleInput30.AB="100";			AngleInput30.t="7";			AngleInput30.R1="10";			AngleInput30.R2="5";			AngleInput30.CD="27.1";			AngleInput30.E="25";			AngleInput.Add(AngleInput30);						EqualAngleSizeModel AngleInput31 = new EqualAngleSizeModel();			AngleInput31.Size="L 100x100x10";			AngleInput31.AB="100";			AngleInput31.t="10";			AngleInput31.R1="10";			AngleInput31.R2="7";			AngleInput31.CD="28.3";			AngleInput31.E="25";			AngleInput.Add(AngleInput31);						EqualAngleSizeModel AngleInput32 = new EqualAngleSizeModel();			AngleInput32.Size="L 100x100x13";			AngleInput32.AB="100";			AngleInput32.t="13";			AngleInput32.R1="10";			AngleInput32.R2="7";			AngleInput32.CD="29.4";			AngleInput32.E="25";			AngleInput.Add(AngleInput32);						EqualAngleSizeModel AngleInput33 = new EqualAngleSizeModel();			AngleInput33.Size="L 120x120x8";			AngleInput33.AB="120";			AngleInput33.t="8";			AngleInput33.R1="12";			AngleInput33.R2="5";			AngleInput33.CD="32.4";			AngleInput33.E="30";			AngleInput.Add(AngleInput33);						EqualAngleSizeModel AngleInput34 = new EqualAngleSizeModel();			AngleInput34.Size="L 130x130x9";			AngleInput34.AB="130";			AngleInput34.t="9";			AngleInput34.R1="12";			AngleInput34.R2="6";			AngleInput34.CD="35.3";			AngleInput34.E="30";			AngleInput.Add(AngleInput34);						EqualAngleSizeModel AngleInput35 = new EqualAngleSizeModel();			AngleInput35.Size="L 130x130x12";			AngleInput35.AB="130";			AngleInput35.t="12";			AngleInput35.R1="12";			AngleInput35.R2="8.5";			AngleInput35.CD="36.4";			AngleInput35.E="35";			AngleInput.Add(AngleInput35);						EqualAngleSizeModel AngleInput36 = new EqualAngleSizeModel();			AngleInput36.Size="L 130x130x15";			AngleInput36.AB="130";			AngleInput36.t="15";			AngleInput36.R1="12";			AngleInput36.R2="8.5";			AngleInput36.CD="37.6";			AngleInput36.E="35";			AngleInput.Add(AngleInput36);						EqualAngleSizeModel AngleInput37 = new EqualAngleSizeModel();			AngleInput37.Size="L 150x150x10";			AngleInput37.AB="150";			AngleInput37.t="10";			AngleInput37.R1="14";			AngleInput37.R2="7";			AngleInput37.CD="40.5";			AngleInput37.E="35";			AngleInput.Add(AngleInput37);						EqualAngleSizeModel AngleInput38 = new EqualAngleSizeModel();			AngleInput38.Size="L 150x150x12";			AngleInput38.AB="150";			AngleInput38.t="12";			AngleInput38.R1="14";			AngleInput38.R2="7";			AngleInput38.CD="41.4";			AngleInput38.E="40";			AngleInput.Add(AngleInput38);						EqualAngleSizeModel AngleInput39 = new EqualAngleSizeModel();			AngleInput39.Size="L 150x150x15";			AngleInput39.AB="150";			AngleInput39.t="15";			AngleInput39.R1="14";			AngleInput39.R2="10";			AngleInput39.CD="42.4";			AngleInput39.E="40";			AngleInput.Add(AngleInput39);						EqualAngleSizeModel AngleInput40 = new EqualAngleSizeModel();			AngleInput40.Size="L 150x150x19";			AngleInput40.AB="150";			AngleInput40.t="19";			AngleInput40.R1="14";			AngleInput40.R2="10";			AngleInput40.CD="44";			AngleInput40.E="40";			AngleInput.Add(AngleInput40);						EqualAngleSizeModel AngleInput41 = new EqualAngleSizeModel();			AngleInput41.Size="L 175x175x12";			AngleInput41.AB="175";			AngleInput41.t="12";			AngleInput41.R1="15";			AngleInput41.R2="11";			AngleInput41.CD="47.3";			AngleInput41.E="45";			AngleInput.Add(AngleInput41);						EqualAngleSizeModel AngleInput42 = new EqualAngleSizeModel();			AngleInput42.Size="L 175x175x15";			AngleInput42.AB="175";			AngleInput42.t="15";			AngleInput42.R1="15";			AngleInput42.R2="11";			AngleInput42.CD="48.5";			AngleInput42.E="45";			AngleInput.Add(AngleInput42);						EqualAngleSizeModel AngleInput43 = new EqualAngleSizeModel();			AngleInput43.Size="L 200x200x15";			AngleInput43.AB="200";			AngleInput43.t="15";			AngleInput43.R1="17";			AngleInput43.R2="12";			AngleInput43.CD="54.7";			AngleInput43.E="50";			AngleInput.Add(AngleInput43);						EqualAngleSizeModel AngleInput44 = new EqualAngleSizeModel();			AngleInput44.Size="L 200x200x20";			AngleInput44.AB="200";			AngleInput44.t="20";			AngleInput44.R1="17";			AngleInput44.R2="12";			AngleInput44.CD="56.7";			AngleInput44.E="50";			AngleInput.Add(AngleInput44);						EqualAngleSizeModel AngleInput45 = new EqualAngleSizeModel();			AngleInput45.Size="L 200x200x25";			AngleInput45.AB="200";			AngleInput45.t="25";			AngleInput45.R1="17";			AngleInput45.R2="12";			AngleInput45.CD="58.7";			AngleInput45.E="50";			AngleInput.Add(AngleInput45);						EqualAngleSizeModel AngleInput46 = new EqualAngleSizeModel();			AngleInput46.Size="L 250x250x25";			AngleInput46.AB="250";			AngleInput46.t="25";			AngleInput46.R1="24";			AngleInput46.R2="12";			AngleInput46.CD="71";			AngleInput46.E="65";			AngleInput.Add(AngleInput46);						EqualAngleSizeModel AngleInput47 = new EqualAngleSizeModel();			AngleInput47.Size="L 250x250x35";			AngleInput47.AB="250";			AngleInput47.t="35";			AngleInput47.R1="24";			AngleInput47.R2="18";			AngleInput47.CD="74.5";			AngleInput47.E="70";			AngleInput.Add(AngleInput47);					}		}}