using AssemblyLib.AssemblyModels;
using DrawWork.DrawModels;
using DrawWork.ValueServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawServices
{
    public class DrawBMService
    {

        private AssemblyModel assemblyData;
        private ValueService valueService;
        public DrawBMService(AssemblyModel selAssembly)
        {
            assemblyData = selAssembly;
            valueService = new ValueService();
        }

        public List<DrawBMModel> CreateBMModel()
        {
            List<DrawBMModel> newList = new List<DrawBMModel>();

            double bmCount = 0;
            newList.AddRange(GetShellPlate(bmCount));
            bmCount = newList.Count;
            newList.AddRange(GetTopAngle(bmCount));
            bmCount = newList.Count;
            newList.AddRange(GetWindGirder(bmCount));
            bmCount = newList.Count;
            newList.AddRange(GetAnchorChair(bmCount));
            bmCount = newList.Count;
            newList.AddRange(GetEarthLug(bmCount));
            bmCount = newList.Count;
            newList.AddRange(GetSettlementCheckPiece(bmCount));

            return newList;
        }

        public List<DrawBMModel> GetShellPlate(double startNum)
        {
            List<DrawBMModel> newList = new List<DrawBMModel>();

            double bmCount = startNum;
            foreach (ShellOutputModel eachShell in assemblyData.ShellOutput)
            {
                bmCount++;
                DrawBMModel newModel = new DrawBMModel();
                newModel.DWGName = DrawSettingLib.Commons.PAPERMAIN_TYPE.ShellPlateArrangement;
                newModel.Page = 1;
                newModel.No = bmCount.ToString();
                newModel.Name = "SHELL PLATE";
                newModel.Material = eachShell.Material;
                newModel.Dimension = GetDimension(eachShell.Thickness, eachShell.PlateWidth, "");
                newModel.Set = "1";
                newModel.Weight = "";// 현재 공백
                newModel.Remark = "";

                newList.Add(newModel);
            }

            return newList;
        }

        public List<DrawBMModel> GetTopAngle(double startNum)
        {
            List<DrawBMModel> newList = new List<DrawBMModel>();

            double bmCount = startNum ;

            // Angle Top
            string selAngleSize = assemblyData.RoofCompressionRing[0].AngleSize;

            bmCount++;
            DrawBMModel newModel = new DrawBMModel();
            newModel.DWGName = DrawSettingLib.Commons.PAPERMAIN_TYPE.ShellPlateArrangement;
            newModel.Page = 1;
            newModel.No = bmCount.ToString();
            newModel.Name = "TOP ANGLE";
            newModel.Material = "";
            newModel.Dimension = selAngleSize;
            newModel.Set = "1";
            newModel.Weight = "";// 현재 공백
            newModel.Remark = "";

            newList.Add(newModel);
            

            return newList;
        }


        public List<DrawBMModel> GetWindGirder(double startNum)
        {
            List<DrawBMModel> newList = new List<DrawBMModel>();

            double bmCount = startNum ;

            int selwindGirderCount = valueService.GetIntValue(assemblyData.WindGirderInput[0].Qty);

            string windGirderSize = "";
            string windGirderMaterial = "";
            foreach (WindGirderOutputModel eachWindGirder in assemblyData.WindGirderOutput)
            {
                windGirderSize= eachWindGirder.Size;
                windGirderMaterial = eachWindGirder.Material;
                break;
            }

            bmCount++;
            DrawBMModel newModel = new DrawBMModel();
            newModel.DWGName = DrawSettingLib.Commons.PAPERMAIN_TYPE.ShellPlateArrangement;
            newModel.Page = 1;
            newModel.No = bmCount.ToString();
            newModel.Name = "WIND GIRDER";
            newModel.Material = windGirderMaterial;
            newModel.Dimension = windGirderSize;
            newModel.Set = selwindGirderCount.ToString();
            newModel.Weight = "";// 현재 공백
            newModel.Remark = "";

            newList.Add(newModel);


            return newList;
        }

        public List<DrawBMModel> GetAnchorChair(double startNum)
        {
            List<DrawBMModel> newList = new List<DrawBMModel>();

            double bmCount = startNum;

            if (assemblyData.AnchorageInput[0].AnchorChairBlot.Contains("Yes"))
            {
                bmCount++;
                DrawBMModel newModel = new DrawBMModel();
                newModel.DWGName = DrawSettingLib.Commons.PAPERMAIN_TYPE.ShellPlateArrangement;
                newModel.Page = 1;
                newModel.No = bmCount.ToString();
                newModel.Name = "TOP PLATE";
                newModel.Material = assemblyData.GeneralMaterialSpecifications[0].AnchorChairPlate;
                newModel.Dimension = GetDimension(assemblyData.AnchorageInput[0].TopPlateThickness,
                                                  assemblyData.AnchorageInput[0].TopPlateB,
                                                  assemblyData.AnchorageInput[0].TopPlateF);
                newModel.Set = assemblyData.AnchorageInput[0].AnchorQty;
                newModel.Weight = "";// 현재 공백
                newModel.Remark = "";

                newList.Add(newModel);


                bmCount++;
                DrawBMModel newModel2 = new DrawBMModel();
                newModel2.DWGName = DrawSettingLib.Commons.PAPERMAIN_TYPE.ShellPlateArrangement;
                newModel2.Page = 1;
                newModel2.No = bmCount.ToString();
                newModel2.Name = "GUSSET PLATE";
                newModel2.Material = assemblyData.GeneralMaterialSpecifications[0].AnchorChairPlate;
                newModel2.Dimension = GetDimension(assemblyData.AnchorageInput[0].SidePlateThickness,
                                                  assemblyData.AnchorageInput[0].TopPlateB,
                                                  assemblyData.AnchorageInput[0].TopPlateF);
                // 보완 필요
                newModel2.Set = (valueService.GetDoubleValue( assemblyData.AnchorageInput[0].AnchorQty) *2).ToString();
                newModel2.Weight = "";// 현재 공백
                newModel2.Remark = "";

                newList.Add(newModel2);


                if (assemblyData.AnchorageInput[0].ReinforcementPad.Contains("Yes"))
                {
                    bmCount++;
                    DrawBMModel newModel3 = new DrawBMModel();
                    newModel3.DWGName = DrawSettingLib.Commons.PAPERMAIN_TYPE.ShellPlateArrangement;
                    newModel3.Page = 1;
                    newModel3.No = bmCount.ToString();
                    newModel3.Name = "REINF. PAD";
                    newModel3.Material = assemblyData.ShellOutput[0].Material;
                    newModel3.Dimension = GetDimension(assemblyData.ShellOutput[0].Thickness,
                                                      (valueService.GetDoubleValue(assemblyData.AnchorageInput[0].ReinforcementPadWidth)+100).ToString(),
                                                      (valueService.GetDoubleValue(assemblyData.AnchorageInput[0].ReinforcementPadWidth) + 100).ToString());
                    // 보완 필요
                    newModel3.Set = assemblyData.AnchorageInput[0].AnchorQty;
                    newModel3.Weight = "";// 현재 공백
                    newModel3.Remark = "";

                    newList.Add(newModel3);
                }


                // Anchor Bolt / nut
                bmCount++;
                DrawBMModel newModel4 = new DrawBMModel();
                newModel4.DWGName = DrawSettingLib.Commons.PAPERMAIN_TYPE.ShellPlateArrangement;
                newModel4.Page = 1;
                newModel4.No = bmCount.ToString();
                newModel4.Name = "ANCHOR BOLT/2N";
                newModel4.Material = "SEE NOTE 1";
                newModel4.Dimension = "";
                // 보완 필요
                newModel4.Set = "";
                newModel4.Weight = "";// 현재 공백
                newModel4.Remark = "";

                newList.Add(newModel4);


                // Washer Plate
                bmCount++;
                DrawBMModel newModel5 = new DrawBMModel();
                newModel5.DWGName = DrawSettingLib.Commons.PAPERMAIN_TYPE.ShellPlateArrangement;
                newModel5.Page = 1;
                newModel5.No = bmCount.ToString();
                newModel5.Name = "WASHER PLATE";
                newModel5.Material = "SEE NOTE 2";
                newModel5.Dimension = "";
                // 보완 필요
                newModel5.Set = "";
                newModel5.Weight = "";// 현재 공백
                newModel5.Remark = "";

                newList.Add(newModel5);

            }
           


            return newList;
        }



        public List<DrawBMModel> GetEarthLug(double startNum)
        {
            List<DrawBMModel> newList = new List<DrawBMModel>();

            double bmCount = startNum ;

            if (assemblyData.AppurtenancesInput[0].EarthLug.Contains("Yes"))
            {
                string insulationThk = "";
                if (assemblyData.RoofCRTInput[0].InsulationRequired.Contains("Yes"))
                    insulationThk = assemblyData.RoofCRTInput[0].InsulationThickness;
                if (assemblyData.RoofDRTInput[0].InsulationRequired.Contains( "Yes"))
                    insulationThk = assemblyData.RoofDRTInput[0].InsulationThickness;

                bmCount++;
                DrawBMModel newModel = new DrawBMModel();
                newModel.DWGName = DrawSettingLib.Commons.PAPERMAIN_TYPE.ShellPlateArrangement;
                newModel.Page = 1;
                newModel.No = bmCount.ToString();
                newModel.Name = "EARTH LUG";
                newModel.Material = "default";
                newModel.Dimension = GetDimension(assemblyData.AppurtenancesInput[0].EarthLugThickness,
                                                  insulationThk +50,
                                                  assemblyData.AppurtenancesInput[0].ELProjectionFromShell);
                newModel.Set = assemblyData.AppurtenancesInput[0].ELQty; ;
                newModel.Weight = "";// 현재 공백
                newModel.Remark = "";

                newList.Add(newModel);
            }



            return newList;
        }

        public List<DrawBMModel> GetSettlementCheckPiece(double startNum)
        {
            List<DrawBMModel> newList = new List<DrawBMModel>();

            double bmCount = startNum ;

            if (assemblyData.AppurtenancesInput[0].SettlementCheckPiece.Contains("Yes"))
            {
                bmCount++;
                DrawBMModel newModel = new DrawBMModel();
                newModel.DWGName = DrawSettingLib.Commons.PAPERMAIN_TYPE.ShellPlateArrangement;
                newModel.Page = 1;
                newModel.No = bmCount.ToString();
                newModel.Name = "SETTLEMeNT CHECK PIECE";
                newModel.Material = "A36"; // CS : A36, SS : A479-304(304L, 316, 316L)
                newModel.Dimension = "L64x64xt8";
                newModel.Set = "8" ;
                newModel.Weight = "";// 현재 공백
                newModel.Remark = "";

                newList.Add(newModel);
            }



            return newList;
        }

        public List<DrawBMModel> GetNamePlate(double startNum)
        {
            List<DrawBMModel> newList = new List<DrawBMModel>();

            double bmCount = startNum ;

            if (assemblyData.AppurtenancesInput[0].NamePlate.Contains("Yes"))
            {
                bmCount++;
                DrawBMModel newModel = new DrawBMModel();
                newModel.DWGName = DrawSettingLib.Commons.PAPERMAIN_TYPE.ShellPlateArrangement;
                newModel.Page = 1;
                newModel.No = bmCount.ToString();
                newModel.Name = "NAME PLATE BRACKET";
                newModel.Material = assemblyData.ShellOutput[0].Material;
                newModel.Dimension = "t6";
                newModel.Set = "1";
                newModel.Weight = "";// 현재 공백
                newModel.Remark = "";

                newList.Add(newModel);




                bmCount++;
                DrawBMModel newModel2 = new DrawBMModel();
                newModel2.DWGName = DrawSettingLib.Commons.PAPERMAIN_TYPE.ShellPlateArrangement;
                newModel2.Page = 1;
                newModel2.No = bmCount.ToString();
                newModel2.Name = "SET SCREW";
                newModel2.Material = "";
                newModel2.Dimension = "t6";
                newModel2.Set = "1";
                newModel2.Weight = "";// 현재 공백
                newModel2.Remark = "";

                newList.Add(newModel2);
            }



            return newList;
        }


        private string GetDimension(string Thk, string W, string L)
        {
            return "t" + Thk + "x" + W + "X" + L;

        }

    }
}
