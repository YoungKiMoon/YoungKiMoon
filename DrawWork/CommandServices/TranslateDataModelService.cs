﻿using AssemblyLib.AssemblyModels;using System;using System.Collections.Generic;using System.Linq;using System.Text;using System.Threading.Tasks;namespace DrawWork.CommandServices{	public class TranslateDataModelService	{		public AssemblyModel assemblyData;				#region assemblyData		public void SetAssemblyData(AssemblyModel selAssembly)		{			assemblyData= selAssembly;		}		#endregion		#region Translate Model Main Switch		public string GetTranslateModelSwitch(string selUsingStr, string selCmdStr, string selCmdNew, int selCmdIndex)		{			string result = "";			switch (selUsingStr)			{				case "generaldd":				case "generaldesigndata":					result = GetTMSGeneralDesignData(selCmdStr,selCmdNew,selCmdIndex);					break;				case "generalcw":				case "generalcapacityweight":					result = GetTMSGeneralCapacityWeight(selCmdStr,selCmdNew,selCmdIndex);					break;				case "generalmasfab":				case "generalmomentandshearforceatbase":					result = GetTMSGeneralMomentAndShearForceAtBase(selCmdStr,selCmdNew,selCmdIndex);					break;				case "generalms":				case "generalmaterialspecs":					result = GetTMSGeneralMaterialSpecs(selCmdStr,selCmdNew,selCmdIndex);					break;				case "generalca":				case "generalcorrosionallowance":					result = GetTMSGeneralCorrosionAllowance(selCmdStr,selCmdNew,selCmdIndex);					break;				case "generale":				case "generalearquake":					result = GetTMSGeneralEarquake(selCmdStr,selCmdNew,selCmdIndex);					break;				case "shellin":				case "shellinput":					result = GetTMSShellInput(selCmdStr,selCmdNew,selCmdIndex);					break;				case "shellout":				case "shelloutput":					result = GetTMSShellOutput(selCmdStr,selCmdNew,selCmdIndex);					break;				case "roofin":				case "roofinput":					result = GetTMSRoofInput(selCmdStr,selCmdNew,selCmdIndex);					break;				case "roofout":				case "roofoutput":					result = GetTMSRoofOutput(selCmdStr,selCmdNew,selCmdIndex);					break;				case "bottomin":				case "bottominput":					result = GetTMSBottomInput(selCmdStr,selCmdNew,selCmdIndex);					break;				case "bottomout":				case "bottomoutput":					result = GetTMSBottomOutput(selCmdStr,selCmdNew,selCmdIndex);					break;				case "nozzlein":				case "nozzleinputmodel":					result = GetTMSNozzleInputModel(selCmdStr,selCmdNew,selCmdIndex);					break;				case "windgirderin":				case "windgirderinput":					result = GetTMSWindGirderInput(selCmdStr,selCmdNew,selCmdIndex);					break;				case "windgirderout":				case "windgirderoutput":					result = GetTMSWindGirderOutput(selCmdStr,selCmdNew,selCmdIndex);					break;				case "insulation":				case "insulationinput":					result = GetTMSInsulationInput(selCmdStr,selCmdNew,selCmdIndex);					break;				case "angle":				case "angleinput":					result = GetTMSAngleInput(selCmdStr,selCmdNew,selCmdIndex);					break;				default:					result = "nothing";					break;			}			return result;		}		#endregion		#region Translate Model Each Switch		private string GetTMSGeneralDesignData(string selCmdStr, string selCmdNew, int selCmdIndex)		{			string result = "";			switch (selCmdStr)			{				case "appliedcode":					result = assemblyData.GeneralDesignData.AppliedCode;					break;				case "shelldesign":					result = assemblyData.GeneralDesignData.ShellDesign;					break;				case "roofdesign":					result = assemblyData.GeneralDesignData.RoofDesign;					break;				case "contents":					result = assemblyData.GeneralDesignData.Contents;					break;				case "designspecgr":					result = assemblyData.GeneralDesignData.DesignSpecGr;					break;				case "measurementunit":					result = assemblyData.GeneralDesignData.MeasurementUnit;					break;				case "rooftype":					result = assemblyData.GeneralDesignData.RoofType;					break;				case "sizenominalid":					result = assemblyData.GeneralDesignData.SizeNominalId;					break;				case "sizetankheight":					result = assemblyData.GeneralDesignData.SizeTankHeight;					break;				case "platewidth":					result = assemblyData.GeneralDesignData.PlateWidth;					break;				case "platemaxlength":					result = assemblyData.GeneralDesignData.PlateMaxLength;					break;				case "pumpingratesin":					result = assemblyData.GeneralDesignData.PumpingRatesIn;					break;				case "pumpingratesout":					result = assemblyData.GeneralDesignData.PumpingRatesOut;					break;				case "opertempmin":					result = assemblyData.GeneralDesignData.OperTempMin;					break;				case "opertempnor":					result = assemblyData.GeneralDesignData.OperTempNor;					break;				case "opertempmax":					result = assemblyData.GeneralDesignData.OperTempMax;					break;				case "designtempmin":					result = assemblyData.GeneralDesignData.DesignTempMin;					break;				case "designtempmax":					result = assemblyData.GeneralDesignData.DesignTempMax;					break;				case "operpressint":					result = assemblyData.GeneralDesignData.OperPressInt;					break;				case "operpressext":					result = assemblyData.GeneralDesignData.OperPressExt;					break;				case "designpressint":					result = assemblyData.GeneralDesignData.DesignPressInt;					break;				case "designpressext":					result = assemblyData.GeneralDesignData.DesignPressExt;					break;				case "vaporpressuremax":					result = assemblyData.GeneralDesignData.VaporPressureMax;					break;				case "setpressureemergencycovermanhole":					result = assemblyData.GeneralDesignData.SetPressureEmergencyCoverManhole;					break;				case "setpressurebreathervalve":					result = assemblyData.GeneralDesignData.SetPressureBreatherValve;					break;				case "setpressurebreathervalvevac":					result = assemblyData.GeneralDesignData.SetPressureBreatherValveVac;					break;				case "testspgr":					result = assemblyData.GeneralDesignData.TestSpGr;					break;				case "roofloadsuniformlive":					result = assemblyData.GeneralDesignData.RoofLoadsUniformLive;					break;				case "roofloadsspecialloading":					result = assemblyData.GeneralDesignData.RoofLoadsSpecialLoading;					break;				case "windvelocity":					result = assemblyData.GeneralDesignData.WindVelocity;					break;				case "rainfallmax":					result = assemblyData.GeneralDesignData.RainFallMax;					break;				case "snowfalltotalaccumulation":					result = assemblyData.GeneralDesignData.SnowFallTotalAccumulation;					break;				case "foundationtype":					result = assemblyData.GeneralDesignData.FoundationType;					break;				case "insulationshell":					result = assemblyData.GeneralDesignData.InsulationShell;					break;				case "insulationroof":					result = assemblyData.GeneralDesignData.InsulationRoof;					break;				case "mdmt":					result = assemblyData.GeneralDesignData.MDMT;					break;				case "dmt":					result = assemblyData.GeneralDesignData.DMT;					break;				default:					result = "nothing";					break;			}			return result;		}		private string GetTMSGeneralCapacityWeight(string selCmdStr, string selCmdNew, int selCmdIndex)		{			string result = "";			switch (selCmdStr)			{				case "empty":					result = assemblyData.GeneralCapacityWeight[selCmdIndex].Empty;					break;				case "operating":					result = assemblyData.GeneralCapacityWeight[selCmdIndex].Operating;					break;				case "fullofwater":					result = assemblyData.GeneralCapacityWeight[selCmdIndex].FullOfWater;					break;				case "insulation":					result = assemblyData.GeneralCapacityWeight[selCmdIndex].Insulation;					break;				case "platformladder":					result = assemblyData.GeneralCapacityWeight[selCmdIndex].PlatformLadder;					break;				case "others":					result = assemblyData.GeneralCapacityWeight[selCmdIndex].Others;					break;				case "liquid":					result = assemblyData.GeneralCapacityWeight[selCmdIndex].Liquid;					break;				case "paintingareaint":					result = assemblyData.GeneralCapacityWeight[selCmdIndex].PaintingAreaInt;					break;				case "paintingareaext":					result = assemblyData.GeneralCapacityWeight[selCmdIndex].PaintingAreaExt;					break;				case "nominalcapacity":					result = assemblyData.GeneralCapacityWeight[selCmdIndex].NominalCapacity;					break;				case "workingcapacity":					result = assemblyData.GeneralCapacityWeight[selCmdIndex].WorkingCapacity;					break;				case "networkingcapacity":					result = assemblyData.GeneralCapacityWeight[selCmdIndex].NetWorkingCapacity;					break;				default:					result = "nothing";					break;			}			return result;		}		private string GetTMSGeneralMomentAndShearForceAtBase(string selCmdStr, string selCmdNew, int selCmdIndex)		{			string result = "";			switch (selCmdStr)			{				case "windmoment":					result = assemblyData.GeneralMomentAndShearForceAtBase[selCmdIndex].WindMoment;					break;				case "seismicmoment":					result = assemblyData.GeneralMomentAndShearForceAtBase[selCmdIndex].SeismicMoment;					break;				case "shearforceempty":					result = assemblyData.GeneralMomentAndShearForceAtBase[selCmdIndex].ShearForceEmpty;					break;				case "shearforceoperating":					result = assemblyData.GeneralMomentAndShearForceAtBase[selCmdIndex].ShearForceOperating;					break;				default:					result = "nothing";					break;			}			return result;		}		private string GetTMSGeneralMaterialSpecs(string selCmdStr, string selCmdNew, int selCmdIndex)		{			string result = "";			switch (selCmdStr)			{				case "shellplates":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].ShellPlates;					break;				case "padsweldedshell":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].PadsWeldedShell;					break;				case "bottomplates":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].BottomPlates;					break;				case "bottomplatesthickness":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].BottomPlatesThickness;					break;				case "bottomplatesslope":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].BottomPlatesSlope;					break;				case "bottomplatesweldjointtype":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].BottomPlatesWeldJointType;					break;				case "bottomplatesbottomstyle":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].BottomPlatesBottomStyle;					break;				case "roofplates":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].RoofPlates;					break;				case "roofplatesthickness":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].RoofPlatesThickness;					break;				case "roofplatesslope":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].RoofPlatesSlope;					break;				case "roofplatesweldjointtype":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].RoofPlatesWeldJointType;					break;				case "roofplatesconeroofsupport":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].RoofPlatesConeRoofSupport;					break;				case "annularplate":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].AnnularPlate;					break;				case "annularplateminwidth":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].AnnularPlateMinWidth;					break;				case "annularplatethickness":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].AnnularPlateThickness;					break;				case "annularplateweldjointtype":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].AnnularPlateWeldJointType;					break;				case "nozzleneckpipeplate":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].NozzleNeckPipePlate;					break;				case "forgedflangescoverscplg":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].ForgedFlangesCoversCplg;					break;				case "plateflangecovers":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].PlateFlangeCovers;					break;				case "internalsupportsweldedtoshell":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].InternalSupportsWeldedToShell;					break;				case "internalpipe":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].InternalPipe;					break;				case "externallug":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].ExternalLug;					break;				case "boltnut":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].BoltNut;					break;				case "anchorboltnut":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].AnchorBoltNut;					break;				case "gasket":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].Gasket;					break;				case "painting":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].Painting;					break;				case "insulation":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].Insulation;					break;				case "nameplateearthlug":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].NamePlateEarthLug;					break;				case "roofstructure":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].RoofStructure;					break;				case "roofstructurecolumn":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].RoofStructureColumn;					break;				case "platformwalkway":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].PlatformWalkway;					break;				case "internalfloatingroof":					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].InternalFloatingRoof;					break;				default:					result = "nothing";					break;			}			return result;		}		private string GetTMSGeneralCorrosionAllowance(string selCmdStr, string selCmdNew, int selCmdIndex)		{			string result = "";			switch (selCmdStr)			{				case "shellplate":					result = assemblyData.GeneralCorrosionAllowance[selCmdIndex].ShellPlate;					break;				case "roofplate":					result = assemblyData.GeneralCorrosionAllowance[selCmdIndex].RoofPlate;					break;				case "bottomplate":					result = assemblyData.GeneralCorrosionAllowance[selCmdIndex].BottomPlate;					break;				case "nozzle":					result = assemblyData.GeneralCorrosionAllowance[selCmdIndex].Nozzle;					break;				case "structureeachside":					result = assemblyData.GeneralCorrosionAllowance[selCmdIndex].StructureEachSide;					break;				case "column":					result = assemblyData.GeneralCorrosionAllowance[selCmdIndex].Column;					break;				case "annularplate":					result = assemblyData.GeneralCorrosionAllowance[selCmdIndex].AnnularPlate;					break;				default:					result = "nothing";					break;			}			return result;		}		private string GetTMSGeneralEarquake(string selCmdStr, string selCmdNew, int selCmdIndex)		{			string result = "";			switch (selCmdStr)			{				case "seismiczone":					result = assemblyData.GeneralEarquake[selCmdIndex].SeismicZone;					break;				case "seismiczonefactor":					result = assemblyData.GeneralEarquake[selCmdIndex].SeismicZoneFactor;					break;				case "sitecoefficient":					result = assemblyData.GeneralEarquake[selCmdIndex].SiteCoefficient;					break;				case "sone":					result = assemblyData.GeneralEarquake[selCmdIndex].SOne;					break;				case "siteclass":					result = assemblyData.GeneralEarquake[selCmdIndex].SiteClass;					break;				case "usegroup":					result = assemblyData.GeneralEarquake[selCmdIndex].UseGroup;					break;				case "importancefactor":					result = assemblyData.GeneralEarquake[selCmdIndex].ImportanceFactor;					break;				default:					result = "nothing";					break;			}			return result;		}		private string GetTMSShellInput(string selCmdStr, string selCmdNew, int selCmdIndex)		{			string result = "";			switch (selCmdStr)			{				case "caos":				case "caofshell":					result = assemblyData.ShellInput[selCmdIndex].CAOfShell;					break;				case "cc":				case "coursecount":					result = assemblyData.ShellInput[selCmdIndex].CourseCount;					break;				case "pc":				case "platecnt":					result = assemblyData.ShellInput[selCmdIndex].PlateCnt;					break;				default:					result = "nothing";					break;			}			return result;		}		private string GetTMSShellOutput(string selCmdStr, string selCmdNew, int selCmdIndex)		{			string result = "";			switch (selCmdStr)			{				case "courseno":					result = assemblyData.ShellOutput[selCmdIndex].CourseNo;					break;				case "minthk":					result = assemblyData.ShellOutput[selCmdIndex].MinThk;					break;				case "startpoint":					result = assemblyData.ShellOutput[selCmdIndex].StartPoint;					break;				case "width":					result = assemblyData.ShellOutput[selCmdIndex].Width;					break;				case "length":					result = assemblyData.ShellOutput[selCmdIndex].Length;					break;				case "repeatno":					result = assemblyData.ShellOutput[selCmdIndex].RepeatNo;					break;				default:					result = "nothing";					break;			}			return result;		}		private string GetTMSRoofInput(string selCmdStr, string selCmdNew, int selCmdIndex)		{			string result = "";			switch (selCmdStr)			{				case "roofslopeone":					result = assemblyData.RoofInput[selCmdIndex].RoofSlopeOne;					break;				case "roofthickness":					result = assemblyData.RoofInput[selCmdIndex].RoofThickness;					break;				case "caofroof":					result = assemblyData.RoofInput[selCmdIndex].CAOfRoof;					break;				case "topangletype":					result = assemblyData.RoofInput[selCmdIndex].TopAngleType;					break;				case "topanesize":					result = assemblyData.RoofInput[selCmdIndex].TopAneSize;					break;				default:					result = "nothing";					break;			}			return result;		}		private string GetTMSRoofOutput(string selCmdStr, string selCmdNew, int selCmdIndex)		{			string result = "";			switch (selCmdStr)			{				case "shellthk":					result = assemblyData.RoofOutput[selCmdIndex].ShellThk;					break;				case "twotcmax":					result = assemblyData.RoofOutput[selCmdIndex].TwoTcMax;					break;				case "anglesize":					result = assemblyData.RoofOutput[selCmdIndex].AngleSize;					break;				case "anglethk":					result = assemblyData.RoofOutput[selCmdIndex].AngleThk;					break;				case "b":					result = assemblyData.RoofOutput[selCmdIndex].B;					break;				case "roofangle":					result = assemblyData.RoofOutput[selCmdIndex].RoofAngle;					break;				case "roofthk":					result = assemblyData.RoofOutput[selCmdIndex].RoofThk;					break;				case "wcstartpoint":					result = assemblyData.RoofOutput[selCmdIndex].WcStartPoint;					break;				case "wc":					result = assemblyData.RoofOutput[selCmdIndex].Wc;					break;				case "twotsortwotbmax":					result = assemblyData.RoofOutput[selCmdIndex].TwoTsOrTwoTbMax;					break;				case "ringwidth":					result = assemblyData.RoofOutput[selCmdIndex].RingWidth;					break;				case "ringthk":					result = assemblyData.RoofOutput[selCmdIndex].RingThk;					break;				case "comprringwidth":					result = assemblyData.RoofOutput[selCmdIndex].ComprRingWidth;					break;				case "outsideprojection":					result = assemblyData.RoofOutput[selCmdIndex].OutsideProjection;					break;				case "overlapofroof":					result = assemblyData.RoofOutput[selCmdIndex].OverlapOfRoof;					break;				default:					result = "nothing";					break;			}			return result;		}		private string GetTMSBottomInput(string selCmdStr, string selCmdNew, int selCmdIndex)		{			string result = "";			switch (selCmdStr)			{				case "bottomslope":					result = assemblyData.BottomInput[selCmdIndex].BottomSlope;					break;				case "bottomthickness":					result = assemblyData.BottomInput[selCmdIndex].BottomThickness;					break;				case "caofbottomplate":					result = assemblyData.BottomInput[selCmdIndex].CAOfBottomPlate;					break;				case "annularplatereqd":					result = assemblyData.BottomInput[selCmdIndex].AnnularPlateReqd;					break;				case "annularplatethickness":					result = assemblyData.BottomInput[selCmdIndex].AnnularPlateThickness;					break;				default:					result = "nothing";					break;			}			return result;		}		private string GetTMSBottomOutput(string selCmdStr, string selCmdNew, int selCmdIndex)		{			string result = "";			switch (selCmdStr)			{				case "shellthk":					result = assemblyData.BottomOutput[selCmdIndex].ShellThk;					break;				case "outsideprojection":					result = assemblyData.BottomOutput[selCmdIndex].OutSideProjection;					break;				case "insideprojection":					result = assemblyData.BottomOutput[selCmdIndex].InsideProjection;					break;				case "odtwo":					result = assemblyData.BottomOutput[selCmdIndex].ODTwo;					break;				case "bottomplateangle":					result = assemblyData.BottomOutput[selCmdIndex].BottomPlateAngle;					break;				case "overlapofannular":					result = assemblyData.BottomOutput[selCmdIndex].OverlapOfAnnular;					break;				default:					result = "nothing";					break;			}			return result;		}		private string GetTMSNozzleInputModel(string selCmdStr, string selCmdNew, int selCmdIndex)		{			string result = "";			switch (selCmdStr)			{				case "position":					result = assemblyData.NozzleInputModel[selCmdIndex].Position;					break;				case "mark":					result = assemblyData.NozzleInputModel[selCmdIndex].Mark;					break;				case "size":					result = assemblyData.NozzleInputModel[selCmdIndex].Size;					break;				case "qty":					result = assemblyData.NozzleInputModel[selCmdIndex].Qty;					break;				case "rating":					result = assemblyData.NozzleInputModel[selCmdIndex].Rating;					break;				case "facing":					result = assemblyData.NozzleInputModel[selCmdIndex].Facing;					break;				case "r":					result = assemblyData.NozzleInputModel[selCmdIndex].R;					break;				case "h":					result = assemblyData.NozzleInputModel[selCmdIndex].H;					break;				case "ort":					result = assemblyData.NozzleInputModel[selCmdIndex].Ort;					break;				case "description":					result = assemblyData.NozzleInputModel[selCmdIndex].Description;					break;				case "remarks":					result = assemblyData.NozzleInputModel[selCmdIndex].Remarks;					break;				case "nozzleposition":					result = assemblyData.NozzleInputModel[selCmdIndex].NozzlePosition;					break;				case "height":					result = assemblyData.NozzleInputModel[selCmdIndex].Height;					break;				default:					result = "nothing";					break;			}			return result;		}		private string GetTMSWindGirderInput(string selCmdStr, string selCmdNew, int selCmdIndex)		{			string result = "";			switch (selCmdStr)			{				case "qty":					result = assemblyData.WindGirderInput[selCmdIndex].Qty;					break;				case "elevation":					result = assemblyData.WindGirderInput[selCmdIndex].Elevation;					break;				case "distance":					result = assemblyData.WindGirderInput[selCmdIndex].Distance;					break;				case "size":					result = assemblyData.WindGirderInput[selCmdIndex].Size;					break;				case "caforoneside":					result = assemblyData.WindGirderInput[selCmdIndex].CAForOneSide;					break;				default:					result = "nothing";					break;			}			return result;		}		private string GetTMSWindGirderOutput(string selCmdStr, string selCmdNew, int selCmdIndex)		{			string result = "";			switch (selCmdStr)			{				case "windgirderno":					result = assemblyData.WindGirderOutput[selCmdIndex].WindGirderNo;					break;				case "windgirderelevation":					result = assemblyData.WindGirderOutput[selCmdIndex].WindGirderElevation;					break;				case "shellcourseno":					result = assemblyData.WindGirderOutput[selCmdIndex].ShellCourseNo;					break;				case "shellthk":					result = assemblyData.WindGirderOutput[selCmdIndex].ShellThk;					break;				case "anglesize":					result = assemblyData.WindGirderOutput[selCmdIndex].AngleSize;					break;				case "anglethik":					result = assemblyData.WindGirderOutput[selCmdIndex].AngleThik;					break;				default:					result = "nothing";					break;			}			return result;		}		private string GetTMSInsulationInput(string selCmdStr, string selCmdNew, int selCmdIndex)		{			string result = "";			switch (selCmdStr)			{				case "shellinsulationrequired":					result = assemblyData.InsulationInput[selCmdIndex].ShellInsulationRequired;					break;				case "shellinsulationthickness":					result = assemblyData.InsulationInput[selCmdIndex].ShellInsulationThickness;					break;				case "roofinsulationrequired":					result = assemblyData.InsulationInput[selCmdIndex].RoofInsulationRequired;					break;				case "roofinsulationthickness":					result = assemblyData.InsulationInput[selCmdIndex].RoofInsulationThickness;					break;				default:					result = "nothing";					break;			}			return result;		}		private string GetTMSAngleInput(string selCmdStr, string selCmdNew, int selCmdIndex)		{			string result = "";			switch (selCmdStr)			{				case "size":					result = assemblyData.AngleInput[selCmdIndex].Size;					break;				case "ab":					result = assemblyData.AngleInput[selCmdIndex].AB;					break;				case "t":					result = assemblyData.AngleInput[selCmdIndex].t;					break;				case "r1":					result = assemblyData.AngleInput[selCmdIndex].R1;					break;				case "r2":					result = assemblyData.AngleInput[selCmdIndex].R2;					break;				case "cd":					result = assemblyData.AngleInput[selCmdIndex].CD;					break;				case "e":					result = assemblyData.AngleInput[selCmdIndex].E;					break;				default:					result = "nothing";					break;			}			return result;		}		#endregion	}}