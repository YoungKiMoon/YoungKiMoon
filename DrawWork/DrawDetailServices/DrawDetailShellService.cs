using AssemblyLib.AssemblyModels;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawWork.AssemblyServices;
using DrawWork.Commons;
using DrawWork.DrawModels;
using DrawWork.DrawSacleServices;
using DrawWork.DrawServices;
using DrawWork.DrawShapes;
using DrawWork.DrawStyleServices;
using DrawWork.ValueServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawDetailServices
{
    public class DrawDetailShellService
    {
        private Model singleModel;
        private AssemblyModel assemblyData;

        private DrawService drawService;
        private AssemblyModelService modelService;

        private ValueService valueService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;


        private DrawEditingService editingService;
        private DrawShapeServices shapeService;
        private DrawBreakSymbols breakService;

        public DrawDetailShellService(AssemblyModel selAssembly, object selModel)
        {
            singleModel = selModel as Model;

            assemblyData = selAssembly;

            drawService = new DrawService(selAssembly);
            modelService = new AssemblyModelService(selAssembly);

            valueService = new ValueService();
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();

            editingService = new DrawEditingService();
            shapeService = new DrawShapeServices();
            breakService = new DrawBreakSymbols();
        }

        #region Horizontal Joint
        public DrawEntityModel GetShellHorizontalJoint(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {
            // List
            DrawEntityModel drawList = new DrawEntityModel();


            // Reference Point
            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);
            // Tank ID
            double ID = 24500;
            if (assemblyData.GeneralDesignData.Count > 0)
                ID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);

            // Plate Info
            double plateLength = 8000;
            //double plateWidth = 3000;
            if (assemblyData.ShellInput.Count > 0)
            {
                plateLength = valueService.GetDoubleValue(assemblyData.ShellInput[0].PlateMaxLength);
                //plateWidth = valueService.GetDoubleValue(assemblyData.ShellInput[0].PlateWidth);
            }

            

            // Course Height
            double tankHeight = 0;
            if (assemblyData.GeneralDesignData.Count > 0)
                tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double shellCount = assemblyData.ShellOutput.Count;
            double drawHeight = 500;
            double scaleDrawHeight = drawHeight * scaleValue;
            double oneHeight = scaleDrawHeight / shellCount;

            // Course Width
            double[] sampleCourseWidth = new double[8] { 480, 480, 480, 480, 480, 480, 480, 480 };
            // course Thickness
            double[] sampleCourseThk = new double[8] { 30, 25, 23, 16, 16, 14, 14, 12 };


            // Sample Data
            List<double> courseWidthRealList = new List<double>();
            List<double> courseWidthList = new List<double>(sampleCourseWidth);
            List<double> courseThkList = new List<double>(sampleCourseThk);


            List<ShellOutputModel> shellCourses = new List<ShellOutputModel>();
            for (int i = 0; i < courseWidthList.Count; i++)
            {
                ShellOutputModel newShellCourse = new ShellOutputModel();
                newShellCourse.PlateWidth = courseWidthList[i].ToString();
                newShellCourse.Thickness = courseThkList[i].ToString();
                shellCourses.Add(newShellCourse);
            }

            if (assemblyData.ShellOutput.Count > 0)
            {
                courseWidthList.Clear();
                courseThkList.Clear();
                shellCourses.Clear();
                foreach (ShellOutputModel eachShell in assemblyData.ShellOutput)
                {
                    courseWidthRealList.Add(valueService.GetDoubleValue(eachShell.PlateWidth));
                    courseWidthList.Add(oneHeight);
                    courseThkList.Add(valueService.GetDoubleValue(eachShell.Thickness));
                    shellCourses.Add(eachShell);
                }
            }





            // Data
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);

            // Custom Scale
            DrawScaleService scaleService = new DrawScaleService();
            double customScaleValue = scaleService.GetPlateHorizontalJointScale(courseThkList[0]);


            // Dimension Area Size
            double dimAreaLength = 60;
            double scaleDimAreaLength = scaleService.GetOriginValueOfScale(customScaleValue, dimAreaLength);
            double dimAreaGapLength = dimAreaLength + 10;
            //double scaleDimAreaGapLength = scaleService.GetOriginValueOfScale(customScaleValue, dimAreaGapLength);


            // Vertical Position
            double positionBreakSymbol = 0.5;
            double positionCourseText = 0.75;
            double positionCourseInfo = 0.6;
            double positionWeldingInfo = 0.4;
            double positionWeldingGapInfo = 0.3;


            // Dimension Arc Radius
            double dimArcRadius = 10;
            double scaleDimArcRadius = scaleService.GetOriginValueOfScale(customScaleValue, dimArcRadius);




            // Welding Symbols
            List<Point3D> weldPointList = new List<Point3D>();

            // First Dim Point
            Point3D firstDimPoint = null;

            // Logic : Course 1 = Index 1



            List<Point3D> exPoint = new List<Point3D>();
            for (int i = 0; i < 4; i++)
                exPoint.Add(referencePoint);


            int courseIndex = -1;
            double courseWidthSum = 0;
            foreach (ShellOutputModel eachCourse in shellCourses)
            {
                courseIndex++;
                double eachCourseWdithReal = courseWidthRealList[courseIndex];
                double eachCourseWidth = courseWidthList[courseIndex];
                double eachCourseThk = valueService.GetDoubleValue(eachCourse.Thickness);
                double eachCourseThkNext = 0;
                if (courseIndex < shellCourses.Count - 1)
                    eachCourseThkNext = valueService.GetDoubleValue(shellCourses[courseIndex + 1].Thickness);

                bool firstPlate = courseIndex == 0 ? true : false;
                bool lastPlate = courseIndex == shellCourses.Count - 1 ? true : false;


                if (eachCourseWidth > 0 && eachCourseThk > 0)
                {
                    weldPointList.Add((Point3D)exPoint[0].Clone());

                    // One Course
                    DrawSlopeLeaderModel slopeModel = new DrawSlopeLeaderModel();
                    DrawWeldingModel eachWeldingModel = GetOnePlateModel(eachCourseWidth, eachCourseThk, eachCourseThkNext, firstPlate, lastPlate);
                    List<Entity> chamferList = shapeService.shellCourses.GetPlateBlock(out exPoint, 
                        GetSumPoint(exPoint[1], 0, 0), 
                        eachCourseThk, 
                        eachCourseWidth, 0, 0, 2, eachWeldingModel, 
                        new DrawCenterLineModel() { scaleValue = customScaleValue }, 
                        customScaleValue, out slopeModel);

                    // Slope
                    List<Entity> slopeList = new List<Entity>();
                    if (slopeModel != null)
                        slopeList = shapeService.shellCourses.slopeSymbolService.GetSlopeSymbol(slopeModel, customScaleValue);


                    // Double Break Line
                    List<Entity> cahmSDoubleLineList = shapeService.breakSymbols.GetSDoubleLine(GetSumPoint(exPoint[1], 0, -eachCourseWidth / 2), eachCourseThk, customScaleValue, 0.01);
                    List<Entity> newchamferList = shapeService.breakSymbols.GetTrimSDoubleLine(chamferList, cahmSDoubleLineList);


                    // Dimension
                    Point3D dimPoint1 = GetSumPoint(exPoint[3], 0, 0);
                    Point3D dimPoint2 = GetSumPoint(exPoint[0], 0, 0);
                    if (firstDimPoint == null)
                        firstDimPoint = GetSumPoint(dimPoint1, 0, 0);


                    DrawEntityModel dimGapList =new DrawEntityModel();
                    if (!firstPlate)
                    {
                        DrawDimensionModel dimGapModel = new DrawDimensionModel() { position = POSITION_TYPE.LEFT, textUpper = "3.2", dimHeight = dimAreaLength, scaleValue = customScaleValue, extLineLeftVisible = false, extLineRightVisible = false, arrowRightHeadVisible = false, arrowLeftHeadVisible = false };
                        dimGapList = drawService.Draw_DimensionDetail(ref singleModel, new Point3D(firstDimPoint.X, dimPoint1.Y), new Point3D(firstDimPoint.X, dimPoint1.Y + eachWeldingModel.OtherDistance, 0), customScaleValue, dimGapModel);

                        dimPoint1 = GetSumPoint(dimPoint1, 0, eachWeldingModel.OtherDistance);
                    }
                    DrawDimensionModel dimEachCourseModel = new DrawDimensionModel() { position = POSITION_TYPE.LEFT, textUpper = eachCourseWdithReal.ToString(), dimHeight = dimAreaLength, scaleValue = customScaleValue, };
                    DrawEntityModel dimList = drawService.Draw_DimensionDetail(ref singleModel, new Point3D(firstDimPoint.X, dimPoint1.Y), new Point3D(firstDimPoint.X, dimPoint2.Y, 0), customScaleValue, dimEachCourseModel);


                    // Add Entity
                    styleService.SetLayerListEntityExcludingCenterLine(ref newchamferList, layerService.LayerOutLine);
                    drawList.outlineList.AddRange(newchamferList);

                    styleService.SetLayerListEntity(ref cahmSDoubleLineList, layerService.LayerDimension);
                    drawList.outlineList.AddRange(cahmSDoubleLineList);

                    styleService.SetLayerListEntity(ref slopeList, layerService.LayerDimension);
                    drawList.outlineList.AddRange(slopeList);


                    // Add Entity : Dimension
                    drawList.AddDrawEntity(dimList);
                    drawList.AddDrawEntity(dimGapList);


                    
                    // Infomation
                    // Course Text
                    string leaderCourseText = valueService.GetOrdinalNumber(courseIndex + 1) + " COURSE";

                    DrawBMLeaderModel leaderCourseTextModel = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = leaderCourseText, lowerText = "", bmNumber = "" };
                    DrawEntityModel leaderCourseTextList = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(exPoint[2], 0, eachCourseWidth * positionCourseText), leaderCourseTextModel, customScaleValue);
                    drawList.AddDrawEntity(leaderCourseTextList);

                    
                    

                    // Course Circle
                    DrawDimensionModel dimCourseCirModel = new DrawDimensionModel() { position = POSITION_TYPE.TOP, textUpperPosition = POSITION_TYPE.LEFT, textUpper = "t" + eachCourseThk, dimHeight = 0, scaleValue = customScaleValue, extLineLeftVisible = false, extLineRightVisible = false, arrowRightHeadVisible = false, arrowLeftHeadOut = true, leftBMNumber = (courseIndex + 1).ToString() };
                    DrawEntityModel dimCourseCirList = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(exPoint[3], 0, eachCourseWidth * positionCourseText), GetSumPoint(exPoint[3], eachCourseThk, eachCourseWidth * positionCourseText), customScaleValue, dimCourseCirModel);
                    drawList.AddDrawEntity(dimCourseCirList);


                    // Course Info
                    // tankID + course Thickness
                    double courseInfoD = tankID + eachCourseThk;
                    double courseInfoC = Math.Round(Math.PI * courseInfoD,1,MidpointRounding.AwayFromZero);
                    string leaderCourseInfoD = "D=" + courseInfoD;
                    string leaderCourseInfoC = "C=" + courseInfoC;
                    DrawBMLeaderModel leaderCourseInfoModel = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = leaderCourseInfoD, lowerText = leaderCourseInfoC, bmNumber = "", textAlign=POSITION_TYPE.LEFT };
                    DrawEntityModel leaderCourseInfoList = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(exPoint[2], -eachCourseThk / 2, eachCourseWidth * positionCourseInfo), leaderCourseInfoModel, customScaleValue);
                    drawList.AddDrawEntity(leaderCourseInfoList);


                    // Welding Info
                    if (!firstPlate)
                    {
                        if (!eachWeldingModel.BottomWeldingSingle)
                        {
                            DrawDimensionModel dimCourseWeldingModel = new DrawDimensionModel() { position = POSITION_TYPE.TOP, textUpperPosition = POSITION_TYPE.LEFT, textUpper = eachWeldingModel.LeftDepth.ToString(), dimHeight = 0, scaleValue = customScaleValue, extLineLeftVisible = false, extLineRightVisible = false, arrowLeftHeadOut = true, arrowRightSymbol = DimHead_Type.Circle };
                            DrawEntityModel dimCourseWeldingList = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(exPoint[3], 0, eachCourseWidth * positionWeldingInfo), GetSumPoint(exPoint[3], eachCourseThk / 2, eachCourseWidth * positionWeldingInfo), customScaleValue, dimCourseWeldingModel);
                            drawList.AddDrawEntity(dimCourseWeldingList);


                            DrawDimensionModel dimCourseWeldingModel2 = new DrawDimensionModel() { position = POSITION_TYPE.TOP, textUpperPosition = POSITION_TYPE.RIGHT, textUpper = eachWeldingModel.RightDepth.ToString(), dimHeight = 0, scaleValue = customScaleValue, extLineLeftVisible = false, extLineRightVisible = false, arrowRightHeadOut = true, arrowLeftSymbol = DimHead_Type.Circle };
                            DrawEntityModel dimCourseWeldingList2 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(exPoint[3], eachCourseThk / 2, eachCourseWidth * positionWeldingInfo), GetSumPoint(exPoint[3], eachCourseThk, eachCourseWidth * positionWeldingInfo), customScaleValue, dimCourseWeldingModel2);
                            drawList.AddDrawEntity(dimCourseWeldingList2);
                        }





                        // Welding Gap
                        double midGapLength = 0;
                        if (!eachWeldingModel.BottomWeldingSingle)
                            midGapLength = -eachWeldingModel.RightDepth;
                        DrawDimensionModel dimCourseWeldingGapModel = new DrawDimensionModel() { position = POSITION_TYPE.TOP, textUpperPosition = POSITION_TYPE.RIGHT, textUpper = eachWeldingModel.MidDepth.ToString(), dimHeight = 10, scaleValue = customScaleValue, arrowLeftHeadOut = true, arrowRightHeadOut = true };
                        DrawEntityModel dimCourseWeldingGapList = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(exPoint[2], midGapLength - eachWeldingModel.MidDepth, eachWeldingModel.OtherDistance), GetSumPoint(exPoint[2], midGapLength, eachWeldingModel.OtherDistance), customScaleValue, dimCourseWeldingGapModel);
                        drawList.AddDrawEntity(dimCourseWeldingGapList);
                    }



                    // Welding Dimension
                    DrawEntityModel dimCourseLeftWeldingArcList = new DrawEntityModel();
                    DrawEntityModel dimCourseRightWeldingArcList = new DrawEntityModel();
                    if (!firstPlate)
                    {
                        if (eachWeldingModel.BottomWeldingSingle)
                        {
                            dimCourseLeftWeldingArcList = drawService.Draw_DimensionArc(GetSumPoint(exPoint[3], 0, 0), GetSumPoint(exPoint[4], 0, 0), "top", dimArcRadius, "45˚", 45, 90, 45, 0, customScaleValue, layerService.LayerDimension);
                        }
                        else
                        {
                            dimCourseLeftWeldingArcList = drawService.Draw_DimensionArc(GetSumPoint(exPoint[3], 0, 0), GetSumPoint(exPoint[4], 0, 0), "top", dimArcRadius, "45˚", 45, 90, 45, 0, customScaleValue, layerService.LayerDimension);
                            dimCourseRightWeldingArcList = drawService.Draw_DimensionArc(GetSumPoint(exPoint[5], 0, 0), GetSumPoint(exPoint[2], 0, 0), "top",dimArcRadius, "45˚", 45, -45, -90, 0, customScaleValue, layerService.LayerDimension);
                        }

                        drawList.AddDrawEntity(dimCourseLeftWeldingArcList);
                        drawList.AddDrawEntity(dimCourseRightWeldingArcList);

                    }


                }

                courseWidthSum += eachCourseWidth;
            }







            //Bottom
            // Bottom Thickness

            string anchorTopPlateC = assemblyData.AnchorageInput[0].TopPlateC;
            string shellThk = assemblyData.ShellOutput[0].Thickness;

            string bottomString = "";
            double bottomThickness = 0;
            string annularPlate = assemblyData.BottomInput[0].AnnularPlate;
            if (annularPlate.Contains("Yes"))
            {
                bottomString = "ANNULAR " + "PLATE O.D ";
                bottomThickness = valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateThickness);
            }
            else
            {
                bottomString = "BOTTOM " + "PLATE O.D ";
                bottomThickness = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateThickness);
            }
            DrawPublicFunctionService publicFService = new DrawPublicFunctionService();
            double outsideProjection = 0;

            // 첫번째 두께 더하기
            outsideProjection += courseThkList[0] + valueService.GetDoubleValue( anchorTopPlateC);

            

            List<Point3D> outPoint = new List<Point3D>();
            List<Entity> bottomEntityList= shapeService.GetRectangle(out outPoint, GetSumPoint(referencePoint, -outsideProjection, 0), outsideProjection *2, bottomThickness, 0, 0, 0, new bool[] { true, false, true, true });
            styleService.SetLayerListEntity(ref bottomEntityList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(bottomEntityList);

            List<Entity> bottomBreakList = breakService.GetFlatBreakLine(GetSumPoint(referencePoint, outsideProjection * 2 - outsideProjection, 0),
                                                                        GetSumPoint(referencePoint, outsideProjection * 2 - outsideProjection, -bottomThickness),
                                                                        scaleValue);
            styleService.SetLayerListEntity(ref bottomBreakList, layerService.LayerDimension);
            drawList.outlineList.AddRange(bottomBreakList);

            // Dimension : Bottom Thickness
            DrawDimensionModel dimBottomThkModel = new DrawDimensionModel() { position = POSITION_TYPE.LEFT, textUpperPosition = POSITION_TYPE.LEFT, textUpper = "t" + bottomThickness, dimHeight = dimAreaGapLength, scaleValue = customScaleValue };
            DrawEntityModel dimBottomThk= drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(referencePoint, -outsideProjection, 0), GetSumPoint(referencePoint, -outsideProjection, -bottomThickness), customScaleValue, dimBottomThkModel);
            drawList.AddDrawEntity(dimBottomThk);



            // Dimension : Vertical
            double erectionHeight = tankHeight + (3.2 * shellCount);
            DrawDimensionModel dimVerticalModel = new DrawDimensionModel() { position = POSITION_TYPE.LEFT, textUpper = "COMPLECTION HEIGHT " + tankHeight, textLower = "ERECTION HEIGHT " + erectionHeight, dimHeight = dimAreaGapLength, scaleValue = customScaleValue };
            DrawEntityModel dimVertical = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(referencePoint, -outsideProjection, 0), GetSumPoint(referencePoint, 0, courseWidthSum), customScaleValue, dimVerticalModel);

            drawList.AddDrawEntity(dimVertical);


            // Dimension : Bottom
            double dimAreaBottom = 10;
            DrawDimensionModel dimBottomModel = new DrawDimensionModel() { position = POSITION_TYPE.BOTTOM, textUpper = outsideProjection.ToString() , dimHeight = dimAreaBottom, scaleValue = customScaleValue };
            DrawEntityModel dimBottom = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(referencePoint, -outsideProjection, -bottomThickness), GetSumPoint(referencePoint, 0, -bottomThickness), customScaleValue, dimBottomModel);
            drawList.AddDrawEntity(dimBottom);

            // Dimension : Bottom : ID
            DrawDimensionModel dimBottomIDModel = new DrawDimensionModel() { position = POSITION_TYPE.BOTTOM, textUpper = "I.D " + tankID, dimHeight = dimAreaBottom, scaleValue = customScaleValue ,extLineRightVisible=false,arrowRightHeadVisible=false};
            DrawEntityModel dimBottomID = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(referencePoint, 0, -bottomThickness), GetSumPoint(referencePoint, 350, -bottomThickness), customScaleValue, dimBottomIDModel);
            drawList.AddDrawEntity(dimBottomID);

            // Dimension : Bottom : ID
            DrawDimensionModel dimBottomMainModel = new DrawDimensionModel() { position = POSITION_TYPE.BOTTOM, textUpper = bottomString + (tankID + (outsideProjection*2)).ToString(), dimHeight = 20, scaleValue = customScaleValue, extLineRightVisible = false, arrowRightHeadVisible = false };
            DrawEntityModel dimBottomMain = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(referencePoint, -outsideProjection, -bottomThickness), GetSumPoint(referencePoint, 350 , -bottomThickness), customScaleValue, dimBottomMainModel);
            drawList.AddDrawEntity(dimBottomMain);


            // Welding Point
            DrawWeldSymbols weldSymbols = new DrawWeldSymbols();
            List<Entity> newWeldList = new List<Entity>();
            for (int i = 0; i < weldPointList.Count; i++)
            {
                double currentThk = courseThkList[i];
                if (i == 0)
                {
                    DrawWeldSymbolModel wsModel = new DrawWeldSymbolModel();
                    wsModel.position = ORIENTATION_TYPE.TOPLEFT;
                    wsModel.weldTypeUp = WeldSymbol_Type.Fillet;
                    wsModel.weldTypeDown = WeldSymbol_Type.Fillet;
                    wsModel.weldDetailType = WeldSymbolDetail_Type.BothSide;
                    wsModel.weldFaceUp = WeldFace_Type.Concave;
                    wsModel.weldLength1 = bottomThickness.ToString();
                    wsModel.weldLength2 = bottomThickness.ToString();
                    wsModel.tailVisible = false;
                    wsModel.leaderAngle = 45;
                    wsModel.leaderLineLength = 20;

                    newWeldList.AddRange(weldSymbols.GetWeldSymbol(GetSumPoint(weldPointList[i],-currentThk,0), singleModel,customScaleValue, wsModel));
                }
                else
                {
                    DrawWeldSymbolModel wsModel = new DrawWeldSymbolModel();
                    wsModel.position = ORIENTATION_TYPE.TOPLEFT;
                    wsModel.weldTypeUp = WeldSymbol_Type.Square;
                    wsModel.weldTypeDown = WeldSymbol_Type.Bevel;
                    wsModel.weldDetailType = WeldSymbolDetail_Type.BothSide;
                    wsModel.weldFaceDown = WeldFace_Type.Convex;
                    wsModel.specification = "B.G";
                    wsModel.leaderAngle = 45;
                    wsModel.leaderLineLength = 20;

                    newWeldList.AddRange(weldSymbols.GetWeldSymbol(GetSumPoint(weldPointList[i], 0, 0), singleModel, customScaleValue, wsModel));
                }
            }
            styleService.SetLayerListEntity(ref newWeldList, layerService.LayerDimension);

            drawList.outlineList.AddRange(newWeldList);

            return drawList;
        }


        public DrawWeldingModel GetOnePlateModel(double selPlateWidth, double selPlateThk, double selNextPlateThk, bool selFirstPlate, bool selLastPlate)
        {
            DrawWeldingModel newModel = new DrawWeldingModel();

            // First Plate
            if (selFirstPlate)
            {
                newModel.BottomWelding = false;
            }
            else
            {
                newModel.BottomWelding = true;
            }

            // Welding : Single or Double
            newModel.BottomWeldingSingle = true;
            if (selPlateThk >= 21)
            {
                newModel.BottomWeldingSingle = false;
            }

            // Single Degree
            newModel.LeftDegree = 45;
            // double Degree
            newModel.RightDegree = 45;

            // Arc
            newModel.LeftWeldingArc = true;
            newModel.RightWeldingArc = true;

            // Welding Depth 3 = 2 : 1
            double tempRightDepth = Math.Round(selPlateThk * 1 / 3, MidpointRounding.AwayFromZero);
            double tempLeftDepth = selPlateThk - tempRightDepth;
            newModel.LeftDepth = tempLeftDepth;
            newModel.RightDepth = tempRightDepth;

            // Ohter Plate Distance Gap :Fixed Value
            newModel.OtherDistance = 3.2;
            // Welding Mid Gap
            newModel.MidDepth = 1.6;

            // Top Chamfer
            newModel.TopChamfer = true;
            if (selLastPlate)
                newModel.TopChamfer = false;


            // Top Chamfer : 3 : 1
            if (newModel.TopChamfer)
            {
                double tempThk = selPlateThk - selNextPlateThk;
                if (tempThk >= 1)
                {
                    newModel.ChamferShort = tempThk;
                    newModel.ChamferLong = tempThk * 3;

                    if (selPlateWidth + -1 < newModel.ChamferLong)
                        newModel.TopChamfer = false;
                }
                else
                {
                    newModel.TopChamfer = false;
                }
            }



            return newModel;
        }

        #endregion


        #region One Course Shell Plate
        public DrawEntityModel GetOneCourseShellPlate(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {
            // SampleData
            //assemblyData.GeneralDesignData.Clear();
            //assemblyData.ShellInput.Clear();
            //assemblyData.ShellOutput.Clear();
            //assemblyData.AnchorageInput.Clear();

            //assemblyData.GeneralDesignData.Add(new GeneralDesignDataModel() { SizeNominalID = "32400" });
            //assemblyData.ShellInput.Add(new ShellInputModel() { PlateWidth = "2720", PlateMaxLength = "10000" });
            //assemblyData.ShellOutput.Add(new ShellOutputModel() { PlateWidth = "2720" });

            //assemblyData.AnchorageInput.Add(new AnchorageInputModel() { AnchorType = "TYPE II", AnchorQty = "46", AnchorSize = "M30", AnchorStartAngle = "6", AnchorHeight = "350" });

            // 강제적용
            //caleValue = 60;

            // List
            DrawEntityModel drawList = new DrawEntityModel();

            //double plateWidth = valueService.GetDoubleValue( assemblyData.ShellInput[0].PlateWidth);
            double plateMaxLength = valueService.GetDoubleValue(assemblyData.ShellInput[0].PlateMaxLength);

            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double oneShellThk = valueService.GetDoubleValue(assemblyData.ShellOutput[0].Thickness);
            double tankOD = tankID + (oneShellThk * 2);
            double oneCourseWidth = valueService.GetDoubleValue(assemblyData.ShellOutput[0].PlateWidth);


            // Validation
            //if (oneCourseWidth > plateWidth)
            //{
                // Plate 크기가 첫번째 Course 보다 작다
                //Console.WriteLine("Plate 크기가 첫번째 course 보다 작다");
                //return drawList;
            //}

            // One Plate : Length
            double tankCircumference = Math.PI * tankOD;
            double oneCoursePlateCount = 0;
            double onePlateLength = 0;
            SetCourseOnePlate(tankOD, plateMaxLength, out oneCoursePlateCount, out onePlateLength);

            // One Plate : Width
            double onePlateWidth = oneCourseWidth;
            double onePlateDegree = 360 / oneCoursePlateCount;

            // Degree
            double plateStartDegree = 10;


            // Anchorage
            string anchorType = assemblyData.AnchorageInput[0].AnchorType;
            double anchorStartAngle = valueService.GetDoubleValue(assemblyData.AnchorageInput[0].AnchorStartAngle);
            double anchorQty = valueService.GetDoubleValue(assemblyData.AnchorageInput[0].AnchorQty);
            double anchorHeight = valueService.GetDoubleValue(assemblyData.AnchorageInput[0].AnchorHeight);
            string anchorBoltSize = assemblyData.AnchorageInput[0].AnchorSize;

            double anchorOneDegree = 360 / anchorQty;
            

            // AnchorChair Model
            AnchorChairModel anchorChair = modelService.GetAnchorChair(anchorBoltSize);
            if (assemblyData.AnchorageInput[0].AnchorChairBlot.ToLower() != "Yes")
                anchorChair = null;




            // Custom Scale
            DrawScaleService scaleService = new DrawScaleService();

            // Draw : Start
            DrawShellDevModel areaModel = new DrawShellDevModel();
            areaModel.rowCount = 3;
            areaModel.columnCount = 3;
            areaModel.area.oneHeight = 185;
            areaModel.area.oneWidth = 598;
            areaModel.area.sideGap = 22;
            areaModel.area.pageHeight = areaModel.area.oneHeight * areaModel.rowCount;
            areaModel.area.pageWidth = areaModel.area.sideGap + areaModel.area.oneWidth + areaModel.area.sideGap;
            areaModel.area.onePageGap = 200;

            areaModel.SetScaleValue(scaleValue);

            double areaHeightAngle = 17;
            double scaleAreaHeightAngle= scaleService.GetOriginValueOfScale(scaleValue, areaHeightAngle);
            double areaHeightDimStart = areaHeightAngle+15;
            double scaleAreaHeightDimStart = scaleService.GetOriginValueOfScale(scaleValue, areaHeightDimStart);
            double areaHeightDimGap = 7;
            double scaleAreaHeightDimGap = scaleService.GetOriginValueOfScale(scaleValue, areaHeightDimGap);



            double textHeight = 2.5;
            double scaleTextHeight = textHeight * scaleValue;

            List<Entity> plateList = new List<Entity>();
            List<Entity> anchorList = new List<Entity>();
            List<Entity> plateDegreeList = new List<Entity>();
            List<Entity> anchorDegreeList = new List<Entity>();

            List<Point3D> platePointList = new List<Point3D>();
            List<Point3D> anchorPointList = new List<Point3D>();

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y + areaModel.areaScale.oneHeight* areaModel.rowCount);
            Point3D startPoint = GetSumPoint(referencePoint, 0, 0);

            Point3D currentPlatePoint = GetSumPoint(startPoint, 0, 0);
            double currentPlateLength = 0;
            double currentPlateDegree = plateStartDegree;
            for(int i = 1; i <= oneCoursePlateCount; i++)
            {
                currentPlatePoint = GetPointOfLength(startPoint, areaModel, onePlateLength, currentPlateLength, 20);
                // Plate
                List<Point3D> outPlatePointList = new List<Point3D>();
                plateList.AddRange(shapeService.GetRectangle(out outPlatePointList, GetSumPoint(currentPlatePoint, 0, 0), onePlateLength, onePlateWidth, 0, 0, 0));

                string plateDegreeStr = Math.Round(currentPlateDegree, 1, MidpointRounding.AwayFromZero) + "˚";
                plateDegreeList.Add(new Text(GetSumPoint(currentPlatePoint,0,0),plateDegreeStr, scaleTextHeight) { Alignment = Text.alignmentType.BottomCenter});

                if (i % 3 == 0)
                {
                    double currentPlateEndDegree = valueService.GetShiftReverseDegree(currentPlateDegree, onePlateDegree);
                    string plateEndDegreeStr = Math.Round(currentPlateEndDegree, 1, MidpointRounding.AwayFromZero) + "˚";
                    plateDegreeList.Add(new Text(GetSumPoint(currentPlatePoint, onePlateLength, 0), plateEndDegreeStr, scaleTextHeight) { Alignment = Text.alignmentType.BottomCenter });
                }

                currentPlateLength += onePlateLength;
                currentPlateDegree = valueService.GetShiftReverseDegree(currentPlateDegree, onePlateDegree);

                // Point
                platePointList.Add(GetSumPoint(currentPlatePoint, 0, -oneCourseWidth));
                platePointList.Add(GetSumPoint(currentPlatePoint, onePlateLength, -oneCourseWidth));
                anchorPointList.Add(GetSumPoint(currentPlatePoint, 0, -oneCourseWidth));
                anchorPointList.Add(GetSumPoint(currentPlatePoint, onePlateLength, -oneCourseWidth));
            }


            // AnchorChair
            Point3D currentAnchorPoint = GetSumPoint(startPoint, 0, 0);
            for (int i = 1; i <= anchorQty; i++)
            {
                double currentDegreeSize = anchorOneDegree * (i - 1);
                double currentDegree = valueService.GetShiftReverseDegree(anchorStartAngle, currentDegreeSize);
                double currentAnchorLength = valueService.GetLengthOfReverseDegreeByStartDegree(plateStartDegree, currentDegree, tankCircumference);

                currentAnchorPoint = GetPointOfLength(startPoint, areaModel, onePlateLength, currentAnchorLength, 20);
                // Anchor
                anchorList.AddRange(GetAnchorChairFront(GetSumPoint(currentAnchorPoint, 0, -oneCourseWidth), anchorChair, scaleValue));
                // Current Anchor Length

                string anchorDegreeStr = Math.Round(currentDegree, 1, MidpointRounding.AwayFromZero) + "˚";
                Point3D anchorDegreePoint = GetSumPoint(currentAnchorPoint, 0, -scaleAreaHeightAngle - oneCourseWidth);
                Text anchorDegreeText = new Text(GetSumPoint(anchorDegreePoint, 0, 0), anchorDegreeStr, scaleTextHeight) { Alignment = Text.alignmentType.BottomLeft };
                anchorDegreeText.Rotate(Utility.DegToRad(90), Vector3D.AxisZ, GetSumPoint(anchorDegreePoint, 0, 0));
                anchorDegreeList.Add(anchorDegreeText);

                // Point
                anchorPointList.Add(GetSumPoint(currentAnchorPoint, 0, -oneCourseWidth));
            }

            // Dimension : Anchor
            if (anchorPointList.Count > 1)
            {
                Dictionary<double, double> xDic = new Dictionary<double, double>();
                foreach (Point3D eachPoint in anchorPointList)
                    if (!xDic.ContainsKey(eachPoint.Y))
                        xDic.Add(eachPoint.Y, eachPoint.Y);

                List<double> xListDic = xDic.Keys.ToList();
                foreach (double eachY in xListDic)
                {
                    List<Point3D> eachYList = new List<Point3D>();
                    foreach (Point3D eachPoint in anchorPointList)
                        if (eachPoint.Y == eachY)
                            eachYList.Add(eachPoint);

                    List<Point3D> eachYSortList = eachYList.OrderBy(x => x.X).ToList();

                    if (eachYSortList.Count > 0)
                    {
                        Point3D eachStartPoint = eachYSortList[0];
                        double startX = eachStartPoint.X;
                        double currentX = eachStartPoint.X;
                        for (int i = 1; i < eachYSortList.Count; i++)
                        {
                            Point3D eachPoint = eachYSortList[i];
                            if (eachPoint.X - startX > areaModel.areaScale.oneWidth)
                            {
                                startX = eachPoint.X;
                                currentX = eachPoint.X;
                            }
                            else
                            {
                                if (eachPoint.X != eachStartPoint.X)
                                {
                                    DrawDimensionModel dimEachModel = new DrawDimensionModel() { position = POSITION_TYPE.BOTTOM, textSizeVisible = true, dimHeight = areaHeightDimStart, scaleValue = scaleValue, };
                                    DrawEntityModel dimList = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(eachStartPoint, 0, 0), GetSumPoint(eachPoint, 0, 0), scaleValue, dimEachModel);
                                    drawList.AddDrawEntity(dimList);
                                }
                            }

                            eachStartPoint = eachYSortList[i];
                        }
                    }


                }
            }

            // Dimension : Plate
            if (platePointList.Count > 1)
            {
                Dictionary<double, double> xDic = new Dictionary<double, double>();
                foreach (Point3D eachPoint in platePointList)
                    if (!xDic.ContainsKey(eachPoint.Y))
                        xDic.Add(eachPoint.Y, eachPoint.Y);

                List<double> xListDic = xDic.Keys.ToList();
                foreach (double eachY in xListDic)
                {
                    List<Point3D> eachYList = new List<Point3D>();
                    foreach (Point3D eachPoint in platePointList)
                        if (eachPoint.Y == eachY)
                            eachYList.Add(eachPoint);

                    List<Point3D> eachYSortList = eachYList.OrderBy(x => x.X).ToList();

                    if (eachYSortList.Count > 0)
                    {
                        Point3D eachStartPoint = eachYSortList[0];
                        double startX = eachStartPoint.X;
                        double currentX = eachStartPoint.X;
                        for (int i = 1; i < eachYSortList.Count; i++)
                        {
                            Point3D eachPoint = eachYSortList[i];
                            if (eachPoint.X - startX > areaModel.areaScale.oneWidth)
                            {
                                startX = eachPoint.X;
                                currentX = eachPoint.X;
                            }
                            else
                            {
                                if (eachPoint.X != eachStartPoint.X)
                                {
                                    DrawDimensionModel dimEachModel = new DrawDimensionModel() { position = POSITION_TYPE.BOTTOM, textSizeVisible = true, dimHeight = areaHeightDimStart + areaHeightDimGap*3, scaleValue = scaleValue, };
                                    DrawEntityModel dimList = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(eachStartPoint, 0, 0), GetSumPoint(eachPoint, 0, 0), scaleValue, dimEachModel);
                                    drawList.AddDrawEntity(dimList);
                                }
                            }

                            eachStartPoint = eachYSortList[i];
                        }
                    }


                }
            }

            // Style
            styleService.SetLayerListEntity(ref plateDegreeList, layerService.LayerDimension);
            styleService.SetLayerListEntity(ref anchorDegreeList, layerService.LayerDimension);

            styleService.SetLayerListEntityExcludingCenterLine(ref anchorList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref plateList, layerService.LayerOutLine);

            drawList.outlineList.AddRange(anchorList);
            drawList.outlineList.AddRange(plateList);
            drawList.dimTextList.AddRange(plateDegreeList);
            drawList.dimTextList.AddRange(anchorDegreeList);

            return drawList;
        }


        public Point3D GetPointOfLength(Point3D startPoint,DrawShellDevModel areaModel,double oneLength, double selLength, double selLenghtFactor)
        {

            double oneRowLength = oneLength * 3;

            double remainingLength = selLength + selLenghtFactor;
            
            double onePageLength = oneRowLength * areaModel.rowCount;
            double currentPage = Math.Truncate(remainingLength / onePageLength);
            remainingLength = remainingLength - ( currentPage * onePageLength);

            double currentRow = Math.Truncate(remainingLength / oneRowLength);
            remainingLength = remainingLength - (currentRow * oneRowLength);

            double currentX = currentPage * (areaModel.areaScale.pageWidth + areaModel.areaScale.onePageGap) + (remainingLength-selLenghtFactor);
            double currentY = -currentRow * areaModel.areaScale.oneHeight; 
            

            return GetSumPoint(startPoint, currentX, currentY);
        }


        public void SetCourseOnePlate(double tankID, double plateMaxLength, out double oneCoursePlateCount, out double onePlateLength)
        {
            double tankCircumference = Math.PI * tankID;
            oneCoursePlateCount = Math.Ceiling(tankCircumference / plateMaxLength);
            onePlateLength = tankCircumference / oneCoursePlateCount;

        }


        public List<Entity> GetAnchorChairFront(Point3D selPoint, AnchorChairModel newAnchor, double scaleValue)
        {
            List<Entity> drawList = new List<Entity>();

            if(newAnchor != null)
            {
                double A = valueService.GetDoubleValue(newAnchor.A);
                double A1 = valueService.GetDoubleValue(newAnchor.A1);
                double B = valueService.GetDoubleValue(newAnchor.B);
                double E = valueService.GetDoubleValue(newAnchor.E);
                double F = valueService.GetDoubleValue(newAnchor.F);
                double T = valueService.GetDoubleValue(newAnchor.T);
                double T1 = valueService.GetDoubleValue(newAnchor.T1);
                double H = valueService.GetDoubleValue(newAnchor.H);
                double I = valueService.GetDoubleValue(newAnchor.I);
                double W = valueService.GetDoubleValue(newAnchor.W);
                double P = valueService.GetDoubleValue(newAnchor.P);
                double T2 = valueService.GetDoubleValue(newAnchor.T2);

                double A2 = valueService.GetDoubleValue(newAnchor.A2);
                double C1 = valueService.GetDoubleValue(newAnchor.C1);
                double B1 = valueService.GetDoubleValue(newAnchor.B1);
                double F1 = valueService.GetDoubleValue(newAnchor.F1);
                double H1 = valueService.GetDoubleValue(newAnchor.H1);
                double G1 = valueService.GetDoubleValue(newAnchor.G1);
                double C = valueService.GetDoubleValue(newAnchor.C);

                double bottomThickness = 8;
                if (assemblyData.BottomInput[0].AnnularPlate.ToLower() == "yes")
                {
                    bottomThickness = valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateThickness);
                }
                else
                {
                    bottomThickness = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateThickness);
                }

                double anchorHeight = H - bottomThickness;
                double anchorLegHeight = anchorHeight ;

                List<Point3D> extPoint = new List<Point3D>();

                // selPoint : Bottom Center

                // Leg
                double gapBetweenLegs = E / 2;
                drawList.AddRange(shapeService.GetRectangle(out extPoint, GetSumPoint(selPoint,-gapBetweenLegs, 0), T1, anchorLegHeight, 0, 0, 2, new bool[] { false, true, true, true }));
                drawList.AddRange(shapeService.GetRectangle(out extPoint, GetSumPoint(selPoint, gapBetweenLegs, 0), T1, anchorLegHeight, 0, 0, 3, new bool[] { false, true, true, true }));

                // Top
                drawList.AddRange(shapeService.GetRectangle(out extPoint, GetSumPoint(selPoint, -F/2, anchorLegHeight), F, T, 0, 0, 3));

                // Top Small
                drawList.AddRange(shapeService.GetRectangle(out extPoint, GetSumPoint(selPoint, -W / 2, anchorLegHeight + T), W , T2, 0, 0, 3, new bool[] { true, true, false, true }));


                // Pad
                double padWidth = F + A2 + A2;
                double padHeight = anchorHeight ;
                List<Entity> padList = shapeService.GetRectangle(out extPoint, GetSumPoint(selPoint, -padWidth/2, C1), padWidth, padHeight, 0, 0, 3);
                Curve.Fillet((ICurve)padList[3], (ICurve)padList[0], F1, false, false, true, true, out Arc padArcFillet1);
                Curve.Fillet((ICurve)padList[0], (ICurve)padList[1], F1, false, false, true, true, out Arc padArcFillet2);
                Curve.Fillet((ICurve)padList[1], (ICurve)padList[2], F1, false, false, true, true, out Arc padArcFillet3);
                Curve.Fillet((ICurve)padList[2], (ICurve)padList[3], F1, false, false, true, true, out Arc padArcFillet4);
                padList.AddRange(new Arc[] { padArcFillet1, padArcFillet2, padArcFillet3, padArcFillet4 });


                // Center Line
                DrawCenterLineModel newCenterModel = new DrawCenterLineModel();
                List<Entity> centerList = editingService.GetCenterLine(GetSumPoint(selPoint, 0, anchorHeight + C1), GetSumPoint(selPoint, 0, 0), newCenterModel.centerLength, scaleValue);
                styleService.SetLayerListEntity(ref centerList, layerService.LayerCenterLine);

                drawList.AddRange(padList);
                drawList.AddRange(centerList);

            }

            return drawList;
        }

        #endregion

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }

    }
}
