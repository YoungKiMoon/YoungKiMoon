using AssemblyLib.AssemblyModels;
using devDept.Eyeshot;
using DrawWork.Commons;
using DrawWork.DrawDetailServices;
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

        private DrawDetailPlateCuttingPlanService detailService;
        public DrawBMService(AssemblyModel selAssembly,Model selModel)
        {
            assemblyData = selAssembly;

            detailService =  new DrawDetailPlateCuttingPlanService(assemblyData, selModel);
            valueService = new ValueService();

        }

        public List<DrawBMModel> CreateBMModel()
        {
            List<DrawBMModel> newList = new List<DrawBMModel>();

            double bmCount = 0;
            // DWG : ShellPlateArrangement
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


            bmCount = 0;
            newList.AddRange(GetBottomPlate(bmCount));
            bmCount = newList.Count;
            newList.AddRange(GetBottomAnnularPlate(bmCount));



            bmCount = 0;
            newList.AddRange(GetRoofPlate(bmCount));
            bmCount = newList.Count;
            newList.AddRange(GetRoofComRingPlate(bmCount));





            return newList;
        }

        public List<DrawBMModel> GetShellPlate(double startNum)
        {

            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double plateLength = valueService.GetDoubleValue(assemblyData.ShellInput[0].PlateMaxLength);

            string tempTankNo = assemblyData.GeneralDesignData[0].RoofType +  " " +
                                assemblyData.GeneralDesignData[0].SizeNominalID + "*" +
                                assemblyData.GeneralDesignData[0].SizeTankHeight ;

            double tankCount = 1;

            List<DrawBMModel> newList = new List<DrawBMModel>();

            double bmCount = startNum;
            foreach (ShellOutputModel eachShell in assemblyData.ShellOutput)
            {
                bmCount++;

                double eachCourseThk = valueService.GetDoubleValue(eachShell.Thickness);
                double eachCourseDiameter = tankID + (eachCourseThk * 2);
                double eachCoursePermiter = Math.PI * eachCourseDiameter;  // 둘레길이
                double eachPlateCount = Math.Ceiling(eachCoursePermiter / plateLength);
                double eachPlateLength = eachCoursePermiter / eachPlateCount;

                // Fab Margin = 2*10;
                double fabMarginPlateLength = eachPlateLength + 2 * 10;
                string fabMarginPlateLengthStr = Math.Round(fabMarginPlateLength, 1, MidpointRounding.AwayFromZero).ToString();

                double fabMarginPlateWidth = valueService.GetDoubleValue( eachShell.PlateWidth) + 2*10;
                string fabMarginPlateWidthStr = fabMarginPlateWidth.ToString();

                DrawBMModel newModel = new DrawBMModel();
                newModel.DWGName = DrawSettingLib.Commons.PAPERMAIN_TYPE.ShellPlateArrangement;
                newModel.Page = 1;
                newModel.No = bmCount.ToString();
                newModel.Name = "SHELL PLATE";
                newModel.Material = eachShell.Material;
                newModel.Dimension = GetDimension(eachShell.Thickness, eachShell.PlateWidth, Math.Round(eachPlateLength, 1, MidpointRounding.AwayFromZero).ToString());
                newModel.Set = eachPlateCount.ToString();
                newModel.Weight = "";// 현재 공백
                newModel.Remark = "";

                // Export
                newModel.ExportData.PORNo = "";
                newModel.ExportData.TankNo = tempTankNo;
                newModel.ExportData.TankName = "";
                newModel.ExportData.Description = bmCount + " Shell Platte";
                newModel.ExportData.Material = eachShell.Material;
                newModel.ExportData.DimThk = eachShell.Thickness;
                newModel.ExportData.DimWidth = fabMarginPlateWidthStr;
                newModel.ExportData.DimLength = fabMarginPlateLengthStr;
                newModel.ExportData.DimQty = eachPlateCount.ToString();

                newModel.ExportData.TankQty = tankCount.ToString();
                newModel.ExportData.TotalQty = (tankCount * eachPlateCount).ToString();
                


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







        public List<DrawBMModel> GetBottomPlate(double startNum)
        {

            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double plateLength = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateLength);
            double plateWidth = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateWidth);
            double plateThk = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateThickness);

            string bottomMaterial = assemblyData.GeneralMaterialSpecifications[0].BottomPlate;
            string tempTankNo = assemblyData.GeneralDesignData[0].RoofType + " " +
                                assemblyData.GeneralDesignData[0].SizeNominalID + "*" +
                                assemblyData.GeneralDesignData[0].SizeTankHeight;

            double tankCount = 1;

            List<DrawBMModel> newList = new List<DrawBMModel>();

            double bmCount = startNum;

            bmCount++;


            

            CDPoint refPoint = new CDPoint();
            CDPoint curPoint = new CDPoint();
            //detailService.DrawBottomCuttingPlan(ref refPoint, ref curPoint,null,1);

            double bottomPlateCount = 0;
            foreach (DrawOnePlateModel eachPlate in SingletonData.BottomPlateList)
            {
                bottomPlateCount += eachPlate.Requirement;
            }



            if (bottomPlateCount > 0)
            {
                DrawBMModel newModel = new DrawBMModel();
                newModel.DWGName = DrawSettingLib.Commons.PAPERMAIN_TYPE.BottomPlateCuttingPlan;
                newModel.Page = 1;
                newModel.No = "B" + bmCount.ToString();
                newModel.Name = "BOTTOM PLATE";
                newModel.Material = bottomMaterial;
                newModel.Dimension = GetDimension(plateThk.ToString(), plateWidth.ToString(), plateLength.ToString());
                newModel.Set = bottomPlateCount.ToString();
                newModel.Weight = "";// 현재 공백
                newModel.Remark = "";

                // Export
                newModel.ExportData.PORNo = "";
                newModel.ExportData.TankNo = tempTankNo;
                newModel.ExportData.TankName = "";
                newModel.ExportData.Description = "Bottom Plate";
                newModel.ExportData.Material = bottomMaterial;
                newModel.ExportData.DimThk = plateThk.ToString();
                newModel.ExportData.DimWidth = plateWidth.ToString();
                newModel.ExportData.DimLength = plateLength.ToString();
                newModel.ExportData.DimQty = bottomPlateCount.ToString();

                newModel.ExportData.TankQty = tankCount.ToString();
                newModel.ExportData.TotalQty = (tankCount * bottomPlateCount).ToString();



                newList.Add(newModel);
            }






            return newList;
        }
        public List<DrawBMModel> GetBottomAnnularPlate(double startNum)
        {

            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double plateLength = valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateLength);
            double plateWidth = valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateWidth);
            double plateThk = valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateThickness);

            string bottomAnnularMaterial = assemblyData.GeneralMaterialSpecifications[0].AnnularPlate;



            string backingStripMaterial = "A283-C";
            double annularWidth = valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateWidth);
            double backingStripThk = 6;
            string backingStripThkStr =  backingStripThk.ToString();
            string backingStripWidth = "50";//고정
            string backingStripLength = (10 + annularWidth + 10).ToString();




            string tempTankNo = assemblyData.GeneralDesignData[0].RoofType + " " +
                                assemblyData.GeneralDesignData[0].SizeNominalID + "*" +
                                assemblyData.GeneralDesignData[0].SizeTankHeight;

            double tankCount = 1;

            List<DrawBMModel> newList = new List<DrawBMModel>();

            double bmCount = startNum;

            bmCount++;



            double maxPlate = detailService.GetBottomAnnularCutting(out double maxBendPlateofOnePlate, out double totalBendingPlate);

            if (maxPlate > 0)
            {
                DrawBMModel newModel = new DrawBMModel();
                newModel.DWGName = DrawSettingLib.Commons.PAPERMAIN_TYPE.BottomPlateCuttingPlan;
                newModel.Page = 1;
                newModel.No = "AP";
                newModel.Name = "ANNULAR PLATE";
                newModel.Material = bottomAnnularMaterial;
                newModel.Dimension = GetDimension(plateThk.ToString(), plateWidth.ToString(), plateLength.ToString());
                newModel.Set = maxPlate.ToString();
                newModel.Weight = "";// 현재 공백
                newModel.Remark = "";

                // Export
                newModel.ExportData.PORNo = "";
                newModel.ExportData.TankNo = tempTankNo;
                newModel.ExportData.TankName = "";
                newModel.ExportData.Description = "Annular Plate";
                newModel.ExportData.Material = bottomAnnularMaterial;
                newModel.ExportData.DimThk = plateThk.ToString();
                newModel.ExportData.DimWidth = plateWidth.ToString();
                newModel.ExportData.DimLength = plateLength.ToString();
                newModel.ExportData.DimQty = maxPlate.ToString();

                newModel.ExportData.TankQty = tankCount.ToString();
                newModel.ExportData.TotalQty = (tankCount * maxPlate).ToString();


                newList.Add(newModel);

                // 2021-09-29 Samsung : 제외

                DrawBMModel newModelBS = new DrawBMModel();
                newModelBS.DWGName = DrawSettingLib.Commons.PAPERMAIN_TYPE.BottomPlateCuttingPlan;
                newModelBS.Page = 1;
                newModelBS.No = "BS";
                newModelBS.Name = "BACKING STRIP";
                newModelBS.Material = backingStripMaterial;
                newModelBS.Dimension = GetDimension(backingStripThkStr, backingStripWidth.ToString(), backingStripLength.ToString());
                newModelBS.Set = totalBendingPlate.ToString();
                newModelBS.Weight = "";// 현재 공백
                newModelBS.Remark = "";

                // Export
                newModelBS.ExportData.PORNo = "";
                newModelBS.ExportData.TankNo = tempTankNo;
                newModelBS.ExportData.TankName = "";
                newModelBS.ExportData.Description = "Backing Strip Flat Bar";
                newModelBS.ExportData.Material = backingStripMaterial;
                newModelBS.ExportData.DimThk = backingStripThk.ToString();
                newModelBS.ExportData.DimWidth = backingStripWidth.ToString();
                newModelBS.ExportData.DimLength = backingStripLength.ToString();
                newModelBS.ExportData.DimQty = totalBendingPlate.ToString();

                newModelBS.ExportData.TankQty = tankCount.ToString();
                newModelBS.ExportData.TotalQty = (tankCount * totalBendingPlate).ToString();


                newList.Add(newModelBS);

            }






            return newList;
        }



        public List<DrawBMModel> GetRoofPlate(double startNum)
        {

            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double plateLength = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].PlateLength);
            double plateWidth = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].PlateWidth);
            double plateThk = valueService.GetDoubleValue(assemblyData.GeneralMaterialSpecifications[0].RoofPlateThickness);

            string bottomMaterial = assemblyData.GeneralMaterialSpecifications[0].RoofPlate;
            string tempTankNo = assemblyData.GeneralDesignData[0].RoofType + " " +
                                assemblyData.GeneralDesignData[0].SizeNominalID + "*" +
                                assemblyData.GeneralDesignData[0].SizeTankHeight;

            double tankCount = 1;

            List<DrawBMModel> newList = new List<DrawBMModel>();

            double bmCount = startNum;

            bmCount++;

            CDPoint refPoint = new CDPoint();
            CDPoint curPoint = new CDPoint();
            //detailService.DrawRoofCuttingPlan(ref refPoint, ref curPoint, null, 1);

            double roofPlateCount = 0;
            foreach (DrawOnePlateModel eachPlate in SingletonData.RoofPlateList)
            {
                roofPlateCount += eachPlate.Requirement;
            }




            if (roofPlateCount > 0)
            {
                DrawBMModel newModel = new DrawBMModel();
                newModel.DWGName = DrawSettingLib.Commons.PAPERMAIN_TYPE.RoofPlateCuttingPlan;
                newModel.Page = 1;
                newModel.No = bmCount.ToString();
                newModel.Name = "ROOF PLATE";
                newModel.Material = bottomMaterial;
                newModel.Dimension = GetDimension(plateThk.ToString(), plateWidth.ToString(), plateLength.ToString());
                newModel.Set = roofPlateCount.ToString();
                newModel.Weight = "";// 현재 공백
                newModel.Remark = "";

                // Export
                newModel.ExportData.PORNo = "";
                newModel.ExportData.TankNo = tempTankNo;
                newModel.ExportData.TankName = "";
                newModel.ExportData.Description = "Roof Plate";
                newModel.ExportData.Material = bottomMaterial;
                newModel.ExportData.DimThk = plateThk.ToString();
                newModel.ExportData.DimWidth = plateWidth.ToString();
                newModel.ExportData.DimLength = plateLength.ToString();
                newModel.ExportData.DimQty = roofPlateCount.ToString();

                newModel.ExportData.TankQty = tankCount.ToString();
                newModel.ExportData.TotalQty = (tankCount * roofPlateCount).ToString();



                newList.Add(newModel);
            }






            return newList;
        }


        public List<DrawBMModel> GetRoofComRingPlate(double startNum)
        {

            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double plateLength = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].PlateLength);
            double plateWidth = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].PlateWidth);
            double plateThk = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].ThicknessT1); // i Type

            string comRingMaterial = "";
            string tempTankNo = assemblyData.GeneralDesignData[0].RoofType + " " +
                                assemblyData.GeneralDesignData[0].SizeNominalID + "*" +
                                assemblyData.GeneralDesignData[0].SizeTankHeight;

            double tankCount = 1;

            List<DrawBMModel> newList = new List<DrawBMModel>();

            double bmCount = startNum;

            bmCount++;


            double maxPlate = detailService.GetRoofComRingCutting(out double maxBendPlateofOnePlate, out double totalBendingPlate);


            if (maxPlate > 0)
            {
                DrawBMModel newModel = new DrawBMModel();
                newModel.DWGName = DrawSettingLib.Commons.PAPERMAIN_TYPE.RoofPlateCuttingPlan;
                newModel.Page = 1;
                newModel.No = "CR";
                newModel.Name = "COM. RING PLATE";
                newModel.Material = comRingMaterial;
                newModel.Dimension = GetDimension(plateThk.ToString(), plateWidth.ToString(), plateLength.ToString());
                newModel.Set = maxPlate.ToString();
                newModel.Weight = "";// 현재 공백
                newModel.Remark = "";

                // Export
                newModel.ExportData.PORNo = "";
                newModel.ExportData.TankNo = tempTankNo;
                newModel.ExportData.TankName = "";
                newModel.ExportData.Description = "Compression Ring Plate";
                newModel.ExportData.Material = comRingMaterial;
                newModel.ExportData.DimThk = plateThk.ToString();
                newModel.ExportData.DimWidth = plateWidth.ToString();
                newModel.ExportData.DimLength = plateLength.ToString();
                newModel.ExportData.DimQty = maxPlate.ToString();

                newModel.ExportData.TankQty = tankCount.ToString();
                newModel.ExportData.TotalQty = (tankCount * maxPlate).ToString();



                newList.Add(newModel);




            }






            return newList;
        }



        private string GetDimension(string Thk, string W, string L)
        {
            return "t" + Thk + "x" + W + "X" + L;

        }






        public List<List<string>> GetBMListVER01()
        {
            List<List<string>> bmList = new List<List<string>>();

            // Adj
            foreach (DrawBMModel eachBM in SingletonData.BMList)
            {
                if (eachBM.ExportData.Description != "")
                {
                    // 넓이와 길이 정수 : 소수 첫째 자리 올림
                    double tempDimWidth = valueService.GetDoubleValue(eachBM.ExportData.DimWidth);
                    eachBM.ExportData.DimWidth = valueService.IntRound(tempDimWidth, 0).ToString();
                    double tempDimLength = valueService.GetDoubleValue(eachBM.ExportData.DimLength);
                    eachBM.ExportData.DimLength = valueService.IntRound(tempDimLength, 0).ToString();

                }


            }

            foreach (DrawBMModel eachBM in SingletonData.BMList)
            {
                if (eachBM.ExportData.Description != "")
                {
                    switch (eachBM.Name)
                    {
                        case "SHELL PLATE":
                        case "BOTTOM PLATE":
                        case "ANNULAR PLATE":
                        case "ROOF PLATE":
                        case "COM. RING PLATE":
                            List<string> newBM = new List<string>();
                            newBM.Add(eachBM.ExportData.PORNo);
                            newBM.Add(eachBM.ExportData.TankNo);
                            newBM.Add(eachBM.ExportData.TankName);
                            newBM.Add(eachBM.ExportData.Description);
                            newBM.Add(eachBM.ExportData.Material);

                            newBM.Add(eachBM.ExportData.DimThk);
                            // 넓이와 길이 정수 : 소수 첫째 자리 올림
                            double tempDimWidth = valueService.GetDoubleValue(eachBM.ExportData.DimWidth);
                            double tempDimLength = valueService.GetDoubleValue(eachBM.ExportData.DimLength);
                            newBM.Add(valueService.IntRound(tempDimWidth, 0).ToString());
                            newBM.Add(valueService.IntRound(tempDimLength, 0).ToString());
                            newBM.Add(eachBM.ExportData.DimQty);

                            newBM.Add(eachBM.ExportData.UnitWeight);

                            newBM.Add(eachBM.ExportData.TankQty);

                            newBM.Add(eachBM.ExportData.Margin);

                            newBM.Add(eachBM.ExportData.TotalQty);
                            newBM.Add(eachBM.ExportData.TotalWeight);
                            newBM.Add(eachBM.ExportData.Remarks);

                            bmList.Add(newBM);
                            break;
                    }

                }


            }

            

            return bmList;
        }
    }
}
