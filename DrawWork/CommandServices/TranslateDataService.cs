using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


using DrawWork.CommandModels;
using DrawWork.ValueServices;
using AssemblyLib.AssemblyModels;

namespace DrawWork.CommandServices
{
    public class TranslateDataService
    {
        public AssemblyModel assemblyData;
        private ValueService valueService;

        #region CONSTRUCTOR
        public TranslateDataService()
        {
            assemblyData = new AssemblyModel();
            valueService = new ValueService();
        }
        public TranslateDataService(AssemblyModel selAssembly)
        {
            assemblyData = new AssemblyModel();
            valueService = new ValueService();
            SetAssemblyData(selAssembly);
        }
        #endregion

        #region AssemblyData
        public void SetAssemblyData(AssemblyModel selAssembly)
        {
            assemblyData = selAssembly;
        }
        #endregion

        #region Translate Command
        public List<string[]> TranslateCommand(List<CommandLineModel> selCmd)
        {
            Regex regex = new Regex(" ");
            List<string[]> resultCmd = new List<string[]>();
            foreach (CommandLineModel eachCmd in selCmd)
            {

                string[] cmdArray = regex.Split(eachCmd.CommandText);
                resultCmd.Add(cmdArray);
            }

            return resultCmd;
        }
        public void TranslateModelData(List<string[]> selCmd)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 2;

            for (int i = 0; i < selCmd.Count; i++)
            {
                string[] eachCmd = selCmd[i];
                if (eachCmd != null)
                {
                    for (int j = refIndex; j < eachCmd.Length; j += 2)
                    {
                        eachCmd[j] = GetModelDataArray(eachCmd[j]);
                    }
                    selCmd[i] = eachCmd;
                }

            }

        }
        private string GetModelDataArray(string selCmd)
        {
            string selArrayStr = "";
            if (selCmd.Contains(","))
            {
                string[] selArray = selCmd.Split(',');
                for (int i = 0; i < selArray.Length; i++)
                {
                    if (selArrayStr != "")
                        selArrayStr += ",";
                    selArrayStr += GetModelDataCal(selArray[i].Trim());

                }
            }
            else
            {
                selArrayStr = GetModelDataCal(selCmd);
            }

            return selArrayStr;
        }
        private string GetModelDataCal(string selCmd)
        {
            string calStr = "";
            string newStr = "";
            string newValue = "";
            bool calStart = false;

            foreach (char ch in selCmd)
            {
                switch (ch)
                {
                    case '+':
                        calStr = "+";
                        break;
                    case '-':
                        calStr = "-";
                        break;
                    case '*':
                        calStr = "*";
                        break;
                    case '/':
                        calStr = "/";
                        break;
                }
                if (calStr != "")
                {
                    calStart = true;

                    newValue += GetModelData(newStr);
                    newValue += calStr;

                    newStr = "";
                    calStr = "";
                }
                else
                {
                    newStr += ch;
                }

            }

            newValue += GetModelData(newStr);
            return newValue;
            /*
            if (calStart)
            {
                double doubleValue = Evaluate(newValue);
                return doubleValue.ToString();
            }
            else
            {
                return newValue;
            }
            */
        }
        private string[] GetModelIndex(string selCmd)
        {

            if (!selCmd.Contains("["))
            {
                string[] selArray = new string[2] { selCmd, "0" };
                return selArray;
            }
            else
            {
                string selCmdNew = selCmd.Replace("]", "");
                return selCmdNew.Split('[');
            }

        }
        private int GetIndexValue(string selIndex)
        {
            int intValue = 1;
            if (!int.TryParse(selIndex, out intValue))
                intValue = 1;
            return intValue - 1;
        }
        #endregion

        #region Translate Method
        public int DrawMethod_Repeat(string[] eachCmd)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;

            int result = 0;

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "rep":
                    case "repeat":
                        if (j + 1 <= eachCmd.Length)
                        {
                            double calDouble = valueService.Evaluate(eachCmd[j + 1]);
                            result = Convert.ToInt32(calDouble);
                        }
                        break;

                }

            }

            return result;

        }
        #endregion

        #region Translate Model
        private string GetModelData(string selCmd)
        {
            string result = "0";

            string relativeStr = "";
            if (selCmd.Contains("@"))
                relativeStr = "@";

            string selCmdNew = selCmd.Replace("@", "");

            string[] selCmdArray = GetModelIndex(selCmdNew);

            string selCmdStr = selCmdArray[0].ToLower();
            int selCmdIndex = GetIndexValue(selCmdArray[1]);

            

            #region Translate Model Switch
            result = GetTranslateModelSwitch("", selCmdStr, selCmdNew, selCmdIndex);
            #endregion


            return relativeStr + result;
        }

		#endregion

		#region Translate Model Main Switch
		private string GetTranslateModelSwitch(string selUsingStr, string selCmdStr, string selCmdNew, int selCmdIndex)
		{
			string result = "";
			switch (selUsingStr)
			{
				case "generaldd":
				case "GeneralDesignData":
					result = GetTMSGeneralDesignData(selCmdStr, selCmdNew, selCmdIndex);
					break;
				case "generalcw":
				case "GeneralCapacityWeight":
					result = GetTMSGeneralCapacityWeight(selCmdStr, selCmdNew, selCmdIndex);
					break;
				case "generalmasfab":
				case "GeneralMomentAndShearForceAtBase":
					result = GetTMSGeneralMomentAndShearForceAtBase(selCmdStr, selCmdNew, selCmdIndex);
					break;
				case "generalms":
				case "GeneralMaterialSpecs":
					result = GetTMSGeneralMaterialSpecs(selCmdStr, selCmdNew, selCmdIndex);
					break;
				case "generalca":
				case "GeneralCorrosionAllowance":
					result = GetTMSGeneralCorrosionAllowance(selCmdStr, selCmdNew, selCmdIndex);
					break;
				case "generale":
				case "GeneralEarquake":
					result = GetTMSGeneralEarquake(selCmdStr, selCmdNew, selCmdIndex);
					break;
				case "shelln":
				case "ShellInput":
					result = GetTMSShellInput(selCmdStr, selCmdNew, selCmdIndex);
					break;
				case "shellout":
				case "ShellOutput":
					result = GetTMSShellOutput(selCmdStr, selCmdNew, selCmdIndex);
					break;
				case "roofin":
				case "RoofInput":
					result = GetTMSRoofInput(selCmdStr, selCmdNew, selCmdIndex);
					break;
				case "roofout":
				case "RoofOutput":
					result = GetTMSRoofOutput(selCmdStr, selCmdNew, selCmdIndex);
					break;
				case "bottomin":
				case "BottomInput":
					result = GetTMSBottomInput(selCmdStr, selCmdNew, selCmdIndex);
					break;
				case "bottomout":
				case "BottomOutput":
					result = GetTMSBottomOutput(selCmdStr, selCmdNew, selCmdIndex);
					break;
				case "nozzlein":
				case "NozzleInputModel":
					result = GetTMSNozzleInputModel(selCmdStr, selCmdNew, selCmdIndex);
					break;
				case "windgirderin":
				case "WindGirderInput":
					result = GetTMSWindGirderInput(selCmdStr, selCmdNew, selCmdIndex);
					break;
				case "windgirderout":
				case "WindGirderOutput":
					result = GetTMSWindGirderOutput(selCmdStr, selCmdNew, selCmdIndex);
					break;
				case "insulation":
				case "InsulationInput":
					result = GetTMSInsulationInput(selCmdStr, selCmdNew, selCmdIndex);
					break;
				case "angle":
				case "AngleInput":
					result = GetTMSAngleInput(selCmdStr, selCmdNew, selCmdIndex);
					break;
				default:
					result = selCmdNew;
					break;
			}
			return result;
		}
		#endregion

		#region Translate Model Each Switch
		private string GetTMSGeneralDesignData(string selCmdStr, string selCmdNew, int selCmdIndex)
		{
			string result = "";
			switch (selCmdStr)
			{
				case "AppliedCode":
					result = assemblyData.GeneralDesignData.AppliedCode;
					break;
				case "ShellDesign":
					result = assemblyData.GeneralDesignData.ShellDesign;
					break;
				case "RoofDesign":
					result = assemblyData.GeneralDesignData.RoofDesign;
					break;
				case "Contents":
					result = assemblyData.GeneralDesignData.Contents;
					break;
				case "DesignSpecGr":
					result = assemblyData.GeneralDesignData.DesignSpecGr;
					break;
				case "MeasurementUnit":
					result = assemblyData.GeneralDesignData.MeasurementUnit;
					break;
				case "RoofType":
					result = assemblyData.GeneralDesignData.RoofType;
					break;
				case "SizeNominalId":
					result = assemblyData.GeneralDesignData.SizeNominalId;
					break;
				case "SizeTankHeight":
					result = assemblyData.GeneralDesignData.SizeTankHeight;
					break;
				case "PlateWidth":
					result = assemblyData.GeneralDesignData.PlateWidth;
					break;
				case "PlateMaxLength":
					result = assemblyData.GeneralDesignData.PlateMaxLength;
					break;
				case "PumpingRatesIn":
					result = assemblyData.GeneralDesignData.PumpingRatesIn;
					break;
				case "PumpingRatesOut":
					result = assemblyData.GeneralDesignData.PumpingRatesOut;
					break;
				case "OperTempMin":
					result = assemblyData.GeneralDesignData.OperTempMin;
					break;
				case "OperTempNor":
					result = assemblyData.GeneralDesignData.OperTempNor;
					break;
				case "OperTempMax":
					result = assemblyData.GeneralDesignData.OperTempMax;
					break;
				case "DesignTempMin":
					result = assemblyData.GeneralDesignData.DesignTempMin;
					break;
				case "DesignTempMax":
					result = assemblyData.GeneralDesignData.DesignTempMax;
					break;
				case "OperPressInt":
					result = assemblyData.GeneralDesignData.OperPressInt;
					break;
				case "OperPressExt":
					result = assemblyData.GeneralDesignData.OperPressExt;
					break;
				case "DesignPressInt":
					result = assemblyData.GeneralDesignData.DesignPressInt;
					break;
				case "DesignPressExt":
					result = assemblyData.GeneralDesignData.DesignPressExt;
					break;
				case "VaporPressureMax":
					result = assemblyData.GeneralDesignData.VaporPressureMax;
					break;
				case "SetPressureEmergencyCoverManhole":
					result = assemblyData.GeneralDesignData.SetPressureEmergencyCoverManhole;
					break;
				case "SetPressureBreatherValve":
					result = assemblyData.GeneralDesignData.SetPressureBreatherValve;
					break;
				case "SetPressureBreatherValveVac":
					result = assemblyData.GeneralDesignData.SetPressureBreatherValveVac;
					break;
				case "TestSpGr":
					result = assemblyData.GeneralDesignData.TestSpGr;
					break;
				case "RoofLoadsUniformLive":
					result = assemblyData.GeneralDesignData.RoofLoadsUniformLive;
					break;
				case "RoofLoadsSpecialLoading":
					result = assemblyData.GeneralDesignData.RoofLoadsSpecialLoading;
					break;
				case "WindVelocity":
					result = assemblyData.GeneralDesignData.WindVelocity;
					break;
				case "RainFallMax":
					result = assemblyData.GeneralDesignData.RainFallMax;
					break;
				case "SnowFallTotalAccumulation":
					result = assemblyData.GeneralDesignData.SnowFallTotalAccumulation;
					break;
				case "FoundationType":
					result = assemblyData.GeneralDesignData.FoundationType;
					break;
				case "InsulationShell":
					result = assemblyData.GeneralDesignData.InsulationShell;
					break;
				case "InsulationRoof":
					result = assemblyData.GeneralDesignData.InsulationRoof;
					break;
				case "MDMT":
					result = assemblyData.GeneralDesignData.MDMT;
					break;
				case "DMT":
					result = assemblyData.GeneralDesignData.DMT;
					break;
				default:
					result = selCmdNew;
					break;
			}
			return result;
		}
		private string GetTMSGeneralCapacityWeight(string selCmdStr, string selCmdNew, int selCmdIndex)
		{
			string result = "";
			switch (selCmdStr)
			{
				case "Empty":
					result = assemblyData.GeneralCapacityWeight[selCmdIndex].Empty;
					break;
				case "Operating":
					result = assemblyData.GeneralCapacityWeight[selCmdIndex].Operating;
					break;
				case "FullOfWater":
					result = assemblyData.GeneralCapacityWeight[selCmdIndex].FullOfWater;
					break;
				case "Insulation":
					result = assemblyData.GeneralCapacityWeight[selCmdIndex].Insulation;
					break;
				case "PlatformLadder":
					result = assemblyData.GeneralCapacityWeight[selCmdIndex].PlatformLadder;
					break;
				case "Others":
					result = assemblyData.GeneralCapacityWeight[selCmdIndex].Others;
					break;
				case "Liquid":
					result = assemblyData.GeneralCapacityWeight[selCmdIndex].Liquid;
					break;
				case "PaintingAreaInt":
					result = assemblyData.GeneralCapacityWeight[selCmdIndex].PaintingAreaInt;
					break;
				case "PaintingAreaExt":
					result = assemblyData.GeneralCapacityWeight[selCmdIndex].PaintingAreaExt;
					break;
				case "NominalCapacity":
					result = assemblyData.GeneralCapacityWeight[selCmdIndex].NominalCapacity;
					break;
				case "WorkingCapacity":
					result = assemblyData.GeneralCapacityWeight[selCmdIndex].WorkingCapacity;
					break;
				case "NetWorkingCapacity":
					result = assemblyData.GeneralCapacityWeight[selCmdIndex].NetWorkingCapacity;
					break;
				default:
					result = selCmdNew;
					break;
			}
			return result;
		}
		private string GetTMSGeneralMomentAndShearForceAtBase(string selCmdStr, string selCmdNew, int selCmdIndex)
		{
			string result = "";
			switch (selCmdStr)
			{
				case "WindMoment":
					result = assemblyData.GeneralMomentAndShearForceAtBase[selCmdIndex].WindMoment;
					break;
				case "SeismicMoment":
					result = assemblyData.GeneralMomentAndShearForceAtBase[selCmdIndex].SeismicMoment;
					break;
				case "ShearForceEmpty":
					result = assemblyData.GeneralMomentAndShearForceAtBase[selCmdIndex].ShearForceEmpty;
					break;
				case "ShearForceOperating":
					result = assemblyData.GeneralMomentAndShearForceAtBase[selCmdIndex].ShearForceOperating;
					break;
				default:
					result = selCmdNew;
					break;
			}
			return result;
		}
		private string GetTMSGeneralMaterialSpecs(string selCmdStr, string selCmdNew, int selCmdIndex)
		{
			string result = "";
			switch (selCmdStr)
			{
				case "ShellPlates":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].ShellPlates;
					break;
				case "PadsWeldedShell":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].PadsWeldedShell;
					break;
				case "BottomPlates":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].BottomPlates;
					break;
				case "BottomPlatesThickness":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].BottomPlatesThickness;
					break;
				case "BottomPlatesSlope":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].BottomPlatesSlope;
					break;
				case "BottomPlatesWeldJointType":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].BottomPlatesWeldJointType;
					break;
				case "BottomPlatesBottomStyle":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].BottomPlatesBottomStyle;
					break;
				case "RoofPlates":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].RoofPlates;
					break;
				case "RoofPlatesThickness":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].RoofPlatesThickness;
					break;
				case "RoofPlatesSlope":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].RoofPlatesSlope;
					break;
				case "RoofPlatesWeldJointType":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].RoofPlatesWeldJointType;
					break;
				case "RoofPlatesConeRoofSupport":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].RoofPlatesConeRoofSupport;
					break;
				case "AnnularPlate":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].AnnularPlate;
					break;
				case "AnnularPlateMinWidth":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].AnnularPlateMinWidth;
					break;
				case "AnnularPlateThickness":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].AnnularPlateThickness;
					break;
				case "AnnularPlateWeldJointType":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].AnnularPlateWeldJointType;
					break;
				case "NozzleNeckPipePlate":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].NozzleNeckPipePlate;
					break;
				case "ForgedFlangesCoversCplg":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].ForgedFlangesCoversCplg;
					break;
				case "PlateFlangeCovers":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].PlateFlangeCovers;
					break;
				case "InternalSupportsWeldedToShell":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].InternalSupportsWeldedToShell;
					break;
				case "InternalPipe":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].InternalPipe;
					break;
				case "ExternalLug":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].ExternalLug;
					break;
				case "BoltNut":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].BoltNut;
					break;
				case "AnchorBoltNut":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].AnchorBoltNut;
					break;
				case "Gasket":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].Gasket;
					break;
				case "Painting":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].Painting;
					break;
				case "Insulation":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].Insulation;
					break;
				case "NamePlateEarthLug":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].NamePlateEarthLug;
					break;
				case "RoofStructure":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].RoofStructure;
					break;
				case "RoofStructureColumn":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].RoofStructureColumn;
					break;
				case "PlatformWalkway":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].PlatformWalkway;
					break;
				case "InternalFloatingRoof":
					result = assemblyData.GeneralMaterialSpecs[selCmdIndex].InternalFloatingRoof;
					break;
				default:
					result = selCmdNew;
					break;
			}
			return result;
		}
		private string GetTMSGeneralCorrosionAllowance(string selCmdStr, string selCmdNew, int selCmdIndex)
		{
			string result = "";
			switch (selCmdStr)
			{
				case "ShellPlate":
					result = assemblyData.GeneralCorrosionAllowance[selCmdIndex].ShellPlate;
					break;
				case "RoofPlate":
					result = assemblyData.GeneralCorrosionAllowance[selCmdIndex].RoofPlate;
					break;
				case "BottomPlate":
					result = assemblyData.GeneralCorrosionAllowance[selCmdIndex].BottomPlate;
					break;
				case "Nozzle":
					result = assemblyData.GeneralCorrosionAllowance[selCmdIndex].Nozzle;
					break;
				case "StructureEachSide":
					result = assemblyData.GeneralCorrosionAllowance[selCmdIndex].StructureEachSide;
					break;
				case "Column":
					result = assemblyData.GeneralCorrosionAllowance[selCmdIndex].Column;
					break;
				case "AnnularPlate":
					result = assemblyData.GeneralCorrosionAllowance[selCmdIndex].AnnularPlate;
					break;
				default:
					result = selCmdNew;
					break;
			}
			return result;
		}
		private string GetTMSGeneralEarquake(string selCmdStr, string selCmdNew, int selCmdIndex)
		{
			string result = "";
			switch (selCmdStr)
			{
				case "SeismicZone":
					result = assemblyData.GeneralEarquake[selCmdIndex].SeismicZone;
					break;
				case "SeismicZoneFactor":
					result = assemblyData.GeneralEarquake[selCmdIndex].SeismicZoneFactor;
					break;
				case "SiteCoefficient":
					result = assemblyData.GeneralEarquake[selCmdIndex].SiteCoefficient;
					break;
				case "SOne":
					result = assemblyData.GeneralEarquake[selCmdIndex].SOne;
					break;
				case "SiteClass":
					result = assemblyData.GeneralEarquake[selCmdIndex].SiteClass;
					break;
				case "UseGroup":
					result = assemblyData.GeneralEarquake[selCmdIndex].UseGroup;
					break;
				case "ImportanceFactor":
					result = assemblyData.GeneralEarquake[selCmdIndex].ImportanceFactor;
					break;
				default:
					result = selCmdNew;
					break;
			}
			return result;
		}
		private string GetTMSShellInput(string selCmdStr, string selCmdNew, int selCmdIndex)
		{
			string result = "";
			switch (selCmdStr)
			{
				case "caos":
				case "CAOfShell":
					result = assemblyData.ShellInput[selCmdIndex].CAOfShell;
					break;
				case "cc":
				case "CourseCount":
					result = assemblyData.ShellInput[selCmdIndex].CourseCount;
					break;
				case "pc":
				case "PlateCnt":
					result = assemblyData.ShellInput[selCmdIndex].PlateCnt;
					break;
				default:
					result = selCmdNew;
					break;
			}
			return result;
		}
		private string GetTMSShellOutput(string selCmdStr, string selCmdNew, int selCmdIndex)
		{
			string result = "";
			switch (selCmdStr)
			{
				case "CourseNo":
					result = assemblyData.ShellOutput[selCmdIndex].CourseNo;
					break;
				case "MinThk":
					result = assemblyData.ShellOutput[selCmdIndex].MinThk;
					break;
				case "StartPoint":
					result = assemblyData.ShellOutput[selCmdIndex].StartPoint;
					break;
				case "Width":
					result = assemblyData.ShellOutput[selCmdIndex].Width;
					break;
				case "Length":
					result = assemblyData.ShellOutput[selCmdIndex].Length;
					break;
				case "RepeatNo":
					result = assemblyData.ShellOutput[selCmdIndex].RepeatNo;
					break;
				default:
					result = selCmdNew;
					break;
			}
			return result;
		}
		private string GetTMSRoofInput(string selCmdStr, string selCmdNew, int selCmdIndex)
		{
			string result = "";
			switch (selCmdStr)
			{
				case "RoofSlopeOne":
					result = assemblyData.RoofInput[selCmdIndex].RoofSlopeOne;
					break;
				case "RoofThickness":
					result = assemblyData.RoofInput[selCmdIndex].RoofThickness;
					break;
				case "CAOfRoof":
					result = assemblyData.RoofInput[selCmdIndex].CAOfRoof;
					break;
				case "TopAngleType":
					result = assemblyData.RoofInput[selCmdIndex].TopAngleType;
					break;
				case "TopAneSize":
					result = assemblyData.RoofInput[selCmdIndex].TopAneSize;
					break;
				default:
					result = selCmdNew;
					break;
			}
			return result;
		}
		private string GetTMSRoofOutput(string selCmdStr, string selCmdNew, int selCmdIndex)
		{
			string result = "";
			switch (selCmdStr)
			{
				case "ShellThk":
					result = assemblyData.RoofOutput[selCmdIndex].ShellThk;
					break;
				case "TwoTcMax":
					result = assemblyData.RoofOutput[selCmdIndex].TwoTcMax;
					break;
				case "AngleSize":
					result = assemblyData.RoofOutput[selCmdIndex].AngleSize;
					break;
				case "AngleThk":
					result = assemblyData.RoofOutput[selCmdIndex].AngleThk;
					break;
				case "B":
					result = assemblyData.RoofOutput[selCmdIndex].B;
					break;
				case "RoofAngle":
					result = assemblyData.RoofOutput[selCmdIndex].RoofAngle;
					break;
				case "RoofThk":
					result = assemblyData.RoofOutput[selCmdIndex].RoofThk;
					break;
				case "WcStartPoint":
					result = assemblyData.RoofOutput[selCmdIndex].WcStartPoint;
					break;
				case "Wc":
					result = assemblyData.RoofOutput[selCmdIndex].Wc;
					break;
				case "TwoTsOrTwoTbMax":
					result = assemblyData.RoofOutput[selCmdIndex].TwoTsOrTwoTbMax;
					break;
				case "RingWidth":
					result = assemblyData.RoofOutput[selCmdIndex].RingWidth;
					break;
				case "RingThk":
					result = assemblyData.RoofOutput[selCmdIndex].RingThk;
					break;
				case "ComprRingWidth":
					result = assemblyData.RoofOutput[selCmdIndex].ComprRingWidth;
					break;
				case "OutsideProjection":
					result = assemblyData.RoofOutput[selCmdIndex].OutsideProjection;
					break;
				case "OverlapOfRoof":
					result = assemblyData.RoofOutput[selCmdIndex].OverlapOfRoof;
					break;
				default:
					result = selCmdNew;
					break;
			}
			return result;
		}
		private string GetTMSBottomInput(string selCmdStr, string selCmdNew, int selCmdIndex)
		{
			string result = "";
			switch (selCmdStr)
			{
				case "BottomSlope":
					result = assemblyData.BottomInput[selCmdIndex].BottomSlope;
					break;
				case "BottomThickness":
					result = assemblyData.BottomInput[selCmdIndex].BottomThickness;
					break;
				case "CAOfBottomPlate":
					result = assemblyData.BottomInput[selCmdIndex].CAOfBottomPlate;
					break;
				case "AnnularPlateReqd":
					result = assemblyData.BottomInput[selCmdIndex].AnnularPlateReqd;
					break;
				case "AnnularPlateThickness":
					result = assemblyData.BottomInput[selCmdIndex].AnnularPlateThickness;
					break;
				default:
					result = selCmdNew;
					break;
			}
			return result;
		}
		private string GetTMSBottomOutput(string selCmdStr, string selCmdNew, int selCmdIndex)
		{
			string result = "";
			switch (selCmdStr)
			{
				case "ShellThk":
					result = assemblyData.BottomOutput[selCmdIndex].ShellThk;
					break;
				case "OutSideProjection":
					result = assemblyData.BottomOutput[selCmdIndex].OutSideProjection;
					break;
				case "InsideProjection":
					result = assemblyData.BottomOutput[selCmdIndex].InsideProjection;
					break;
				case "ODTwo":
					result = assemblyData.BottomOutput[selCmdIndex].ODTwo;
					break;
				case "BottomPlateAngle":
					result = assemblyData.BottomOutput[selCmdIndex].BottomPlateAngle;
					break;
				case "OverlapOfAnnular":
					result = assemblyData.BottomOutput[selCmdIndex].OverlapOfAnnular;
					break;
				default:
					result = selCmdNew;
					break;
			}
			return result;
		}
		private string GetTMSNozzleInputModel(string selCmdStr, string selCmdNew, int selCmdIndex)
		{
			string result = "";
			switch (selCmdStr)
			{
				case "Mark":
					result = assemblyData.NozzleInputModel[selCmdIndex].Mark;
					break;
				case "Size":
					result = assemblyData.NozzleInputModel[selCmdIndex].Size;
					break;
				case "Qty":
					result = assemblyData.NozzleInputModel[selCmdIndex].Qty;
					break;
				case "RatingFacing":
					result = assemblyData.NozzleInputModel[selCmdIndex].RatingFacing;
					break;
				case "R":
					result = assemblyData.NozzleInputModel[selCmdIndex].R;
					break;
				case "H":
					result = assemblyData.NozzleInputModel[selCmdIndex].H;
					break;
				case "ORT":
					result = assemblyData.NozzleInputModel[selCmdIndex].ORT;
					break;
				case "Description":
					result = assemblyData.NozzleInputModel[selCmdIndex].Description;
					break;
				case "Remarks":
					result = assemblyData.NozzleInputModel[selCmdIndex].Remarks;
					break;
				default:
					result = selCmdNew;
					break;
			}
			return result;
		}
		private string GetTMSWindGirderInput(string selCmdStr, string selCmdNew, int selCmdIndex)
		{
			string result = "";
			switch (selCmdStr)
			{
				case "Qty":
					result = assemblyData.WindGirderInput[selCmdIndex].Qty;
					break;
				case "Elevation":
					result = assemblyData.WindGirderInput[selCmdIndex].Elevation;
					break;
				case "Distance":
					result = assemblyData.WindGirderInput[selCmdIndex].Distance;
					break;
				case "Size":
					result = assemblyData.WindGirderInput[selCmdIndex].Size;
					break;
				case "CAForOneSide":
					result = assemblyData.WindGirderInput[selCmdIndex].CAForOneSide;
					break;
				default:
					result = selCmdNew;
					break;
			}
			return result;
		}
		private string GetTMSWindGirderOutput(string selCmdStr, string selCmdNew, int selCmdIndex)
		{
			string result = "";
			switch (selCmdStr)
			{
				case "WindGirderNo":
					result = assemblyData.WindGirderOutput[selCmdIndex].WindGirderNo;
					break;
				case "WindGirderElevation":
					result = assemblyData.WindGirderOutput[selCmdIndex].WindGirderElevation;
					break;
				case "ShellCourseNo":
					result = assemblyData.WindGirderOutput[selCmdIndex].ShellCourseNo;
					break;
				case "ShellThk":
					result = assemblyData.WindGirderOutput[selCmdIndex].ShellThk;
					break;
				case "AngleSize":
					result = assemblyData.WindGirderOutput[selCmdIndex].AngleSize;
					break;
				case "AngleThik":
					result = assemblyData.WindGirderOutput[selCmdIndex].AngleThik;
					break;
				default:
					result = selCmdNew;
					break;
			}
			return result;
		}
		private string GetTMSInsulationInput(string selCmdStr, string selCmdNew, int selCmdIndex)
		{
			string result = "";
			switch (selCmdStr)
			{
				case "ShellInsulationRequired":
					result = assemblyData.InsulationInput[selCmdIndex].ShellInsulationRequired;
					break;
				case "ShellInsulationThickness":
					result = assemblyData.InsulationInput[selCmdIndex].ShellInsulationThickness;
					break;
				case "RoofInsulationRequired":
					result = assemblyData.InsulationInput[selCmdIndex].RoofInsulationRequired;
					break;
				case "RoofInsulationThickness":
					result = assemblyData.InsulationInput[selCmdIndex].RoofInsulationThickness;
					break;
				default:
					result = selCmdNew;
					break;
			}
			return result;
		}
		private string GetTMSAngleInput(string selCmdStr, string selCmdNew, int selCmdIndex)
		{
			string result = "";
			switch (selCmdStr)
			{
				case "Size":
					result = assemblyData.AngleInput[selCmdIndex].Size;
					break;
				case "AB":
					result = assemblyData.AngleInput[selCmdIndex].AB;
					break;
				case "t":
					result = assemblyData.AngleInput[selCmdIndex].t;
					break;
				case "R1":
					result = assemblyData.AngleInput[selCmdIndex].R1;
					break;
				case "R2":
					result = assemblyData.AngleInput[selCmdIndex].R2;
					break;
				case "CD":
					result = assemblyData.AngleInput[selCmdIndex].CD;
					break;
				case "E":
					result = assemblyData.AngleInput[selCmdIndex].E;
					break;
				default:
					result = selCmdNew;
					break;
			}
			return result;
		}
		#endregion



	}
}
