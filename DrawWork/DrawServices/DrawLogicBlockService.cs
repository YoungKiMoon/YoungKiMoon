using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using devDept.Eyeshot;
using devDept.Graphics;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Serialization;
using MColor = System.Windows.Media.Color;

using Color = System.Drawing.Color;

using AssemblyLib.AssemblyModels;

using DrawWork.ValueServices;
using DrawWork.DrawModels;
using DrawWork.Commons;
using DrawWork.DrawStyleServices;
using DrawWork.CutomModels;
using DrawWork.DrawSacleServices;

namespace DrawWork.DrawServices
{
    public class DrawLogicBlockService
    {
        private AssemblyModel assemblyData;

        private ValueService valueService;
        private DrawWorkingPointService workingPointService;

        private DrawReferenceBlockService refBlockService;

        private StyleFunctionService styleService;
        private LayerStyleService layerService;

        private DrawPublicFunctionService publicFunService;
        private DrawLeaderPublicService leaderDataService;
        private DrawImportBlockService blockImportService;
        private DrawEditingService editingService;
        private DrawService drawService;

        private Model singleModel;

        private DrawScaleService scaleService;

        private DrawShapeServices shapeService;

        public DrawLogicBlockService(AssemblyModel selAssembly, Object selModel)
        {
            singleModel = selModel as Model;

            assemblyData = selAssembly;
            valueService = new ValueService();
            workingPointService = new DrawWorkingPointService(selAssembly);
            refBlockService = new DrawReferenceBlockService(selAssembly);

            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();

            publicFunService = new DrawPublicFunctionService();

            leaderDataService = new DrawLeaderPublicService(selAssembly);
            blockImportService = new DrawImportBlockService(selAssembly, selModel as Model);

            editingService = new DrawEditingService();


            drawService = new DrawService(selAssembly);

            scaleService = new DrawScaleService();

            shapeService = new DrawShapeServices();
        }

        public Entity[] DrawBlock_TopAngle(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint)
        {

            int refFirstIndex = 0;

            string selSizeTankHeight = assemblyData.GeneralDesignData[refFirstIndex].SizeTankHeight;

            string selAngleType = assemblyData.RoofCompressionRing[refFirstIndex].CompressionRingType;
            string selAngleSize = assemblyData.RoofCompressionRing[refFirstIndex].AngleSize;

            AngleSizeModel selAngleModel = refBlockService.GetAngleSizeModel(selAngleSize);
            int maxCourse = assemblyData.ShellOutput.Count - 1;

            CDPoint drawPoint = new CDPoint();
            CDPoint mirrorPoint = new CDPoint();

            List<Entity> angleEntityAll = new List<Entity>();

            // Type
            switch (selAngleType)
            {
                case "Detail b":
                    drawPoint = GetSumCDPoint(refPoint,
                                              -valueService.GetDoubleValue(selAngleModel.A)
                                              - valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness),

                                              -valueService.GetDoubleValue(selAngleModel.B)
                                              + valueService.GetDoubleValue(selSizeTankHeight));

                    angleEntityAll.AddRange(refBlockService.DrawReference_Angle(drawPoint, selAngleModel));
                    break;

                case "Detail d":
                    drawPoint = GetSumCDPoint(refPoint,
                                              -valueService.GetDoubleValue(selAngleModel.A),

                                              -valueService.GetDoubleValue(selAngleModel.B)
                                              + valueService.GetDoubleValue(selSizeTankHeight));

                    angleEntityAll.AddRange(refBlockService.DrawReference_Angle(drawPoint, selAngleModel));
                    break;

                case "Detail e":
                    drawPoint = GetSumCDPoint(refPoint,
                                              -valueService.GetDoubleValue(selAngleModel.A)
                                              - valueService.GetDoubleValue(selAngleModel.t),

                                              -valueService.GetDoubleValue(selAngleModel.B)
                                              + valueService.GetDoubleValue(selSizeTankHeight));

                    angleEntityAll.AddRange(refBlockService.DrawReference_Angle(drawPoint, selAngleModel));
                    break;

                case "Detail i":
                    drawPoint = GetSumCDPoint(refPoint,
                                              -valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness),

                                               valueService.GetDoubleValue(selSizeTankHeight));
                    angleEntityAll.AddRange(refBlockService.DrawReference_CompressionRingI(drawPoint, ref refPoint));
                    break;

                case "Detail k":
                    drawPoint = GetSumCDPoint(refPoint,
                                              0,

                                               valueService.GetDoubleValue(selSizeTankHeight));
                    angleEntityAll.AddRange(refBlockService.DrawReference_CompressionRingK(drawPoint));
                    break;

            }



            switch (selAngleType)
            {
                case "Detail e":
                    mirrorPoint = GetSumCDPoint(drawPoint, valueService.GetDoubleValue(selAngleModel.A), 0);
                    Plane pl1 = Plane.YZ;
                    pl1.Origin.X = mirrorPoint.X;
                    pl1.Origin.Y = mirrorPoint.Y;
                    Mirror customMirror = new Mirror(pl1);
                    foreach (Entity eachEntity in angleEntityAll)
                        eachEntity.TransformBy(customMirror);

                    break;

            }

            return angleEntityAll.ToArray();
        }

        public Entity[] DrawBlock_TopAngleRightOuter(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint)
        {

            int refFirstIndex = 0;

            string selSizeTankHeight = assemblyData.GeneralDesignData[refFirstIndex].SizeTankHeight;

            string selAngleType = assemblyData.RoofCompressionRing[refFirstIndex].CompressionRingType;
            string selAngleSize = assemblyData.RoofCompressionRing[refFirstIndex].AngleSize;

            AngleSizeModel selAngleModel = refBlockService.GetAngleSizeModel(selAngleSize);
            int maxCourse = assemblyData.ShellOutput.Count - 1;

            CDPoint drawPoint = new CDPoint();
            CDPoint mirrorPoint = new CDPoint();

            List<Entity> angleEntityAll = new List<Entity>();

            // Type
            switch (selAngleType)
            {
                case "Detail b":
                    drawPoint = GetSumCDPoint(refPoint,
                                              -valueService.GetDoubleValue(selAngleModel.A)
                                              - valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness),

                                              -valueService.GetDoubleValue(selAngleModel.B)
                                              + valueService.GetDoubleValue(selSizeTankHeight));

                    angleEntityAll.AddRange(refBlockService.DrawReference_Angle_RightOuter(drawPoint, selAngleModel, selAngleType, ref refPoint, ref curPoint));
                    break;

                case "Detail d":
                    drawPoint = GetSumCDPoint(refPoint,
                                              -valueService.GetDoubleValue(selAngleModel.A),

                                              -valueService.GetDoubleValue(selAngleModel.B)
                                              + valueService.GetDoubleValue(selSizeTankHeight));

                    angleEntityAll.AddRange(refBlockService.DrawReference_Angle_RightOuter(drawPoint, selAngleModel, selAngleType, ref refPoint, ref curPoint));
                    break;

                case "Detail e":
                    drawPoint = GetSumCDPoint(refPoint,
                                              -valueService.GetDoubleValue(selAngleModel.A)
                                              - valueService.GetDoubleValue(selAngleModel.t),

                                              -valueService.GetDoubleValue(selAngleModel.B)
                                              + valueService.GetDoubleValue(selSizeTankHeight));

                    angleEntityAll.AddRange(refBlockService.DrawReference_Angle_RightOuter(drawPoint, selAngleModel, selAngleType, ref refPoint, ref curPoint));
                    break;

                case "Detail i":
                    drawPoint = GetSumCDPoint(refPoint,
                                              -valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness),

                                               valueService.GetDoubleValue(selSizeTankHeight));
                    break;

                case "Detail k":
                    drawPoint = GetSumCDPoint(refPoint,
                                              0,

                                               valueService.GetDoubleValue(selSizeTankHeight));
                    break;

            }


            return angleEntityAll.ToArray();
        }



        public Entity[] DrawBlock_Orientation(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint, double selScaleValue)
        {

            int refFirstIndex = 0;


            double extMainLength = 4;
            double extSubLength = 2;


            List<Entity> newList = new List<Entity>();
            List<Point3D> newOutPoint = new List<Point3D>();

            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[refFirstIndex].SizeNominalID);
            double shellFirstThickness = valueService.GetDoubleValue(assemblyData.ShellOutput[0].Thickness);
            double tankOD = tankID + shellFirstThickness * 2;
            double tankODRadius = tankOD / 2;

            Point3D oriCenterPoint = GetSumPoint(referencePoint, 0, 0);

            // Outer
            Circle cirOD = new Circle(oriCenterPoint, tankODRadius);
            styleService.SetLayer(ref cirOD, layerService.LayerOutLine);
            newList.Add(cirOD);

            // Center Line
            List<Entity> centerVerticalLine = editingService.GetCenterLine(GetSumPoint(oriCenterPoint, 0, tankODRadius), GetSumPoint(oriCenterPoint, 0, -tankODRadius), extMainLength, selScaleValue);
            List<Entity> centerHorizontalLine = editingService.GetCenterLine(GetSumPoint(oriCenterPoint, -tankODRadius, 0), GetSumPoint(oriCenterPoint, tankODRadius, 0), extMainLength, selScaleValue);

            styleService.SetLayerListEntity(ref centerVerticalLine, layerService.LayerCenterLine);
            styleService.SetLayerListEntity(ref centerHorizontalLine, layerService.LayerCenterLine);
            newList.AddRange(centerVerticalLine);
            newList.AddRange(centerHorizontalLine);


            // Annular Type
            if (assemblyData.BottomInput[0].AnnularPlate.ToLower()=="yes")
            {

                double outsideProjection = publicFunService.GetBottomFocuntionOD(assemblyData.BottomInput[0].AnnularPlate);

                double annularInWidth = valueService.GetDoubleValue( assemblyData.BottomInput[0].AnnularPlateWidth);
                double annularCirRadius = tankODRadius - (annularInWidth- outsideProjection);
                Circle cirAnnular = new Circle(oriCenterPoint, annularCirRadius);
                styleService.SetLayer(ref cirAnnular, layerService.LayerHiddenLine);
                newList.Add(cirAnnular);

                double circlePerimeter = valueService.GetCirclePerimeter(annularCirRadius);
                double divWidth = 9144;//9144
                double circleDivNum = Math.Ceiling(circlePerimeter / divWidth);
                double circleDivOneWidth = circlePerimeter / circleDivNum;
                double divOneRadius = valueService.GetRadianByArcLength(circleDivOneWidth, annularCirRadius);

                double startDegree = 10;
                double startRadian = Utility.DegToRad(startDegree);
                double startRadianSum = startRadian;

                for (int i = 0; i < circleDivNum; i++)
                {
                    Line eachLine = new Line(GetSumPoint(oriCenterPoint, 0, 0), GetSumPoint(oriCenterPoint, 0, tankODRadius));

                    eachLine.Rotate(-startRadianSum, Vector3D.AxisZ, GetSumPoint(oriCenterPoint, 0, 0));

                    Point3D[] interPoint = eachLine.IntersectWith(cirAnnular);
                    if (interPoint.Length > 0)
                        eachLine.TrimBy(interPoint[0], true);

                    styleService.SetLayer(ref eachLine, layerService.LayerHiddenLine);
                    newList.Add(eachLine);

                    startRadianSum += divOneRadius;
                }
            }

            //
            if(SingletonData.TankType==TANK_TYPE.CRT || SingletonData.TankType == TANK_TYPE.IFRT)
            {
                DrawStructureService StructureDivService = new DrawStructureService();
                StructureDivService.SetStructureData(SingletonData.TankType, assemblyData.StructureCRTInput[0].SupportingType, assemblyData.RoofCompressionRing[0].CompressionRingType);

                // Column
                if (StructureDivService.columnType == "column")
                {

                    List<Tuple<double, double, double>> columnList = new List<Tuple<double, double, double>>();
                    //columnList.Add(new Tuple<double, double, double>(1, 200, 0));
                    //columnList.Add(new Tuple<double, double, double>(5, 200, 2000));
                    //columnList.Add(new Tuple<double, double, double>(8, 200, 4000));
                    //columnList.Add(new Tuple<double, double, double>(14, 200, 6000));
                    foreach (StructureCRTColumnInputModel eachColumn in assemblyData.StructureCRTColumnInput)
                    {
                        double pipeOD = GetPipeOD(eachColumn.Size);
                        columnList.Add(new Tuple<double, double, double>(valueService.GetDoubleValue(eachColumn.Qty),
                                                                         pipeOD,
                                                                         valueService.GetDoubleValue(eachColumn.Radius)));
                    }

                    foreach (Tuple<double, double, double> eachColumn in columnList)
                    {
                        double columnCount = eachColumn.Item1;
                        double columnPipeOD = eachColumn.Item2;
                        double columnRadius = eachColumn.Item3;
                        if (columnRadius == 0)
                        {
                            // Center : Only One
                            Circle cirColumnCenter = new Circle(GetSumPoint(oriCenterPoint, 0, 0), columnPipeOD / 2);
                            styleService.SetLayer(ref cirColumnCenter, layerService.LayerHiddenLine);
                            newList.Add(cirColumnCenter);
                        }
                        else
                        {
                            Circle cirColumnCenterLine = new Circle(GetSumPoint(oriCenterPoint, 0, 0), columnRadius );
                            styleService.SetLayer(ref cirColumnCenterLine, layerService.LayerCenterLine);
                            newList.Add(cirColumnCenterLine);

                            double circlePerimeter = valueService.GetCirclePerimeter(columnRadius);
                            double circleDivOneWidth = circlePerimeter / columnCount;
                            double divOneRadius = valueService.GetRadianByArcLength(circleDivOneWidth, columnRadius);

                            List<Point3D> cirConnPointList = new List<Point3D>();
                            double startRadianSum = 0;
                            for (int i = 0; i < columnCount; i++)
                            {
                                Line eachLine = new Line(GetSumPoint(oriCenterPoint, 0, 0), GetSumPoint(oriCenterPoint, 0, columnRadius * 2));
                                eachLine.Rotate(-startRadianSum, Vector3D.AxisZ, GetSumPoint(oriCenterPoint, 0, 0));

                                Point3D[] interPoint = eachLine.IntersectWith(cirColumnCenterLine);
                                if (interPoint.Length > 0)
                                {
                                    cirConnPointList.Add(interPoint[0]);

                                    // Pipe
                                    Circle cirColumn = new Circle(GetSumPoint(interPoint[0], 0, 0), columnPipeOD / 2);
                                    styleService.SetLayer(ref cirColumn, layerService.LayerHiddenLine);
                                    newList.Add(cirColumn);

                                    Point3D[] cirInterPoint = eachLine.IntersectWith(cirColumn);
                                    if (cirInterPoint.Length > 0)
                                    {
                                        List<Entity> eachCirColumnCenterLine = editingService.GetCenterLine(GetSumPoint(cirInterPoint[0], 0, 0), GetSumPoint(cirInterPoint[1], 0, 0), extSubLength, selScaleValue);
                                        styleService.SetLayerListEntity(ref eachCirColumnCenterLine, layerService.LayerCenterLine);
                                        newList.AddRange(eachCirColumnCenterLine);
                                    }
                                }
                                startRadianSum += divOneRadius;
                            }

                            // Circle Connection
                            if (cirConnPointList.Count > 1)
                            {
                                List<Entity> circleConnLineList = new List<Entity>();
                                circleConnLineList.Add(new Line(cirConnPointList[0], cirConnPointList[cirConnPointList.Count - 1]));
                                for (int i = 0; i < cirConnPointList.Count - 1; i++)
                                {
                                    circleConnLineList.Add(new Line(cirConnPointList[i], cirConnPointList[i + 1]));
                                }
                                styleService.SetLayerListEntity(ref circleConnLineList, layerService.LayerCenterLine);
                                newList.AddRange(circleConnLineList);
                            }


                        }
                    }
                }
            }



            return newList.ToArray();
        }


        private double GetPipeOD(string selNPS)
        {
            double returnValue = 0;
            foreach (PipeModel eachPipe in assemblyData.PipeList)
            {
                if (eachPipe.NPS == selNPS)
                {
                    returnValue = valueService.GetDoubleValue(eachPipe.OD);
                    break;
                }
            }

            return returnValue;
        }

        public Entity[] DrawBlock_Structure(CDPoint selPoint1, double scaleValue)
        {
            Entity[] returnValue = null;
            switch (SingletonData.TankType)
            {
                case TANK_TYPE.CRT:
                case TANK_TYPE.IFRT:
                    returnValue = DrawBlock_Structure_CRT(selPoint1, scaleValue);
                    break;
                case TANK_TYPE.DRT:
                    returnValue = DrawBlock_Structure_DRT(selPoint1, scaleValue);
                    break;

                case TANK_TYPE.EFRTSingle:
                    // 아직
                    break;
                case TANK_TYPE.EFRTDouble:
                    // 아직
                    break;

            }
            return returnValue;
        }



        public Entity[] DrawBlock_Structure_CRT(CDPoint selPoint1, double scaleValue)
        {
            // Sturcutre Type
            // Type
            DrawStructureService StructureDivService = new DrawStructureService();
            StructureDivService.SetStructureData(SingletonData.TankType, assemblyData.StructureCRTInput[0].SupportingType, assemblyData.RoofCompressionRing[0].CompressionRingType);


            int firstIndex = 0;
            Point3D drawPoint = new Point3D(selPoint1.X, selPoint1.Y, selPoint1.Z);
            CDPoint refPoint = new CDPoint() { X = drawPoint.X, Y = drawPoint.Y };
            CDPoint curPoint = (CDPoint)refPoint.Clone();

            List<Entity> customBlockList = new List<Entity>();

            List<Entity> customLine = new List<Entity>();


            if (StructureDivService.columnType == "column")
            {
                if(assemblyData.StructureCRTColumnInput.Count==0)
                    return customBlockList.ToArray();
                if (assemblyData.StructureCRTRafterInput.Count == 0)
                    return customBlockList.ToArray();
            }
            else if (StructureDivService.columnType == "centering")
            {
                if(assemblyData.StructureCRTRafterInput.Count==0)
                    return customBlockList.ToArray();
            }


                // Roof Slope
                string roofSlopeString = assemblyData.RoofCompressionRing[firstIndex].RoofSlope;
            double roofSlopeDegree = valueService.GetDegreeOfSlope(roofSlopeString);



            StructureCRTColumnBaseSupportModel eachColumnBase = new StructureCRTColumnBaseSupportModel();
            if (assemblyData.StructureCRTColumnBaseSupportOutput.Count > 0)
                eachColumnBase = assemblyData.StructureCRTColumnBaseSupportOutput[firstIndex];

            double bA = valueService.GetDoubleValue(assemblyData.BottomInput[firstIndex].BottomPlateThickness); //-> Bottom Thickness
            double bB = bA; //-> Bottom Thickness 으로 대체
            double bC = valueService.GetDoubleValue(eachColumnBase.D1); //-> D1 으로 대체
            double bD = valueService.GetDoubleValue(eachColumnBase.B);  //-> B 으로 대체
            double bE = valueService.GetDoubleValue(eachColumnBase.A1); //-> A1 으로 대체
            double bF = valueService.GetDoubleValue(eachColumnBase.E1); //-> bE1 으로 대체 ==C1
            double bG = 13;  //-> 13 으로 고정 : oD + 26
            double bH = valueService.GetDoubleValue(eachColumnBase.B1); //-> bB1 으로 대체
            double bI = valueService.GetDoubleValue(eachColumnBase.I);  // OK
            double bJ = valueService.GetDoubleValue(eachColumnBase.J);  // Ok

            //double bA1 = valueService.GetDoubleValue(eachColumnBase.A1);
            //double bB1 = valueService.GetDoubleValue(eachColumnBase.B1);
            //double bC1 = valueService.GetDoubleValue(eachColumnBase.C1);
            //double bD1 = valueService.GetDoubleValue(eachColumnBase.D1);
            //double bE1 = valueService.GetDoubleValue(eachColumnBase.E1);


            // Basic : Center
            PipeModel centerPipe = new PipeModel();
            if (assemblyData.StructureCRTColumnPipeOutput.Count > 0)
                centerPipe = assemblyData.StructureCRTColumnPipeOutput[firstIndex];

            double centerPipeOD = valueService.GetDoubleValue(centerPipe.OD);
            double centerPipeODHalf = centerPipeOD / 2;
            double centerRadius = 0; // Center
            CDPoint centerColumnPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, centerRadius, ref refPoint, ref curPoint);

            // Column Center
            double centerLeftWidthHalf = 0;

            // BlotHole
            //double boltHoleHeight = 0;
            //double boltHoleWidth = 0;
            //boltHoleHeight = valueService.GetDoubleValue(assemblyData.StructureCRTClipSlotHoleOutput[firstIndex].ht);
            //boltHoleWidth = valueService.GetDoubleValue(assemblyData.StructureCRTClipSlotHoleOutput[firstIndex].wd);

            // Centering
            CDPoint centeringClipRight = new CDPoint();

            if (StructureDivService.columnType == "column")
            {
                #region Column Side

                // Girder Count = Column Count -1
                double columnHeightFordimension = 0;
                for (int i = 0; i < assemblyData.StructureCRTGirderInput.Count; i++)
                {
                    #region Column Basic

                    HBeamModel eachHBeam = new HBeamModel();
                    if (assemblyData.StructureCRTColumnHBeamOutput.Count > i)
                        eachHBeam = assemblyData.StructureCRTColumnHBeamOutput[i];

                    StructureCRTColumnSideModel eachColumnSide = new StructureCRTColumnSideModel();
                    if (assemblyData.StructureCRTColumnSideOutput.Count > i + 1)
                        eachColumnSide = assemblyData.StructureCRTColumnSideOutput[i + 1];

                    PipeModel eachPipe = new PipeModel();
                    if (assemblyData.StructureCRTColumnPipeOutput.Count > i + 1)
                        eachPipe = assemblyData.StructureCRTColumnPipeOutput[i + 1];// Start = 2 Column


                    string Size = eachColumnSide.SIZE;
                    double B = valueService.GetDoubleValue(eachColumnSide.B);
                    string C = eachColumnSide.C;
                    double D = valueService.GetDoubleValue(eachColumnSide.D);
                    double E = valueService.GetDoubleValue(eachColumnSide.E);
                    double F = valueService.GetDoubleValue(eachColumnSide.F);
                    double G = valueService.GetDoubleValue(eachColumnSide.G);
                    double H = valueService.GetDoubleValue(eachColumnSide.H);

                    double pipeOD = valueService.GetDoubleValue(eachPipe.OD);
                    double pipeODHalf = pipeOD / 2;

                    double radius = valueService.GetDoubleValue(assemblyData.StructureCRTGirderInput[i].Radius);

                    CDPoint eachColumnPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, radius, ref refPoint, ref curPoint);
                    #endregion

                    #region HBream

                    // Structure A Size
                    double structureSizeABefore = 0;
                    double structureSizeA = 0;
                    if (assemblyData.StructureCRTColumnSideOutput.Count > i)
                        structureSizeABefore = valueService.GetDoubleValue(assemblyData.StructureCRTColumnSideOutput[i].B);
                    structureSizeA = Math.Max(structureSizeABefore, B);

                    CDPoint eachHBeamPoint = (CDPoint)eachColumnPoint.Clone();
                    //eachHBeamPoint.Y = eachHBeamPoint.Y - B;
                    eachHBeamPoint.Y = eachHBeamPoint.Y - structureSizeA;
                    Entity[] eachHBeamEntity = refBlockService.DrawReference_HBeam(eachHBeamPoint, eachHBeam);
                    customBlockList.AddRange(eachHBeamEntity);
                    #endregion

                    // Leader
                    SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                    {
                        leaderPoint = GetSumCDPoint(eachHBeamPoint, 0, 0),
                        lineTextList = new List<string>() {assemblyData.StructureCRTGirderInput[i].Qty + "-" + valueService.GetOrdinalNumber(i+1) + " GIRDERS",
                                                            assemblyData.StructureCRTGirderInput[i].Size },

                        Position = "bottomleft"
                    });


                    #region Side Top Support
                    eachColumnPoint.Y = eachHBeamPoint.Y - valueService.GetDoubleValue(eachHBeam.A);
                    drawPoint.X = eachColumnPoint.X;
                    drawPoint.Y = eachColumnPoint.Y;

                    Line lineTopPad1 = new Line(GetSumPoint(drawPoint, -H - F - pipeODHalf, 0), GetSumPoint(drawPoint, H + F + pipeODHalf, 0));
                    Line lineTopPad2 = new Line(GetSumPoint(drawPoint, -H - F - pipeODHalf, -D), GetSumPoint(drawPoint, H + F + pipeODHalf, -D));
                    Line lineTopPad3 = new Line(GetSumPoint(drawPoint, -H - F - pipeODHalf, 0), GetSumPoint(drawPoint, -H - F - pipeODHalf, -D));
                    Line lineTopPad4 = new Line(GetSumPoint(drawPoint, H + F + pipeODHalf, 0), GetSumPoint(drawPoint, H + F + pipeODHalf, -D));
                    customBlockList.AddRange(new Line[] { lineTopPad1, lineTopPad2, lineTopPad3, lineTopPad4 });

                    Line lineTopLeft1 = new Line(GetSumPoint(drawPoint, -F - pipeODHalf, -D), GetSumPoint(drawPoint, -F - pipeODHalf, -D - G));
                    Line lineTopLeft2 = new Line(GetSumPoint(drawPoint, -pipeODHalf, -D), GetSumPoint(drawPoint, -pipeODHalf, -D - E));
                    Line lineTopLeft3 = new Line(GetSumPoint(drawPoint, -G - pipeODHalf, -D - E), GetSumPoint(drawPoint, -pipeODHalf, -D - E));
                    Line lineTopLeft4 = new Line(GetSumPoint(drawPoint, -G - pipeODHalf, -D - E), GetSumPoint(drawPoint, -F - pipeODHalf, -D - G));
                    customBlockList.AddRange(new Line[] { lineTopLeft1, lineTopLeft2, lineTopLeft3, lineTopLeft4 });


                    Line lineTopRight1 = new Line(GetSumPoint(drawPoint, F + pipeODHalf, -D), GetSumPoint(drawPoint, F + pipeODHalf, -D - G));
                    Line lineTopRight2 = new Line(GetSumPoint(drawPoint, pipeODHalf, -D), GetSumPoint(drawPoint, pipeODHalf, -D - E));
                    Line lineTopRight3 = new Line(GetSumPoint(drawPoint, G + pipeODHalf, -D - E), GetSumPoint(drawPoint, pipeODHalf, -D - E));
                    Line lineTopRight4 = new Line(GetSumPoint(drawPoint, G + pipeODHalf, -D - E), GetSumPoint(drawPoint, F + pipeODHalf, -D - G));
                    customBlockList.AddRange(new Line[] { lineTopRight1, lineTopRight2, lineTopRight3, lineTopRight4 });
                    #endregion

                    #region Support Buttom Support

                    CDPoint eachColumnBasePoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, radius, ref refPoint, ref curPoint);
                    drawPoint.X = eachColumnBasePoint.X;
                    drawPoint.Y = eachColumnBasePoint.Y;

                    CDPoint eachColumnBasePointLeft = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, radius + pipeODHalf + bD + bC + bJ, ref refPoint, ref curPoint);
                    CDPoint eachColumnBasePointRight = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, radius - pipeODHalf - bD - bC - bJ, ref refPoint, ref curPoint);
                    //Line basePad1 = new Line(new Point3D(eachColumnBasePointLeft.X, eachColumnBasePointLeft.Y), new Point3D(eachColumnBasePointRight.X, eachColumnBasePointRight.Y));                    
                    Line basePad2 = new Line(GetSumPoint(eachColumnBasePointLeft, 0, 0), GetSumPoint(eachColumnBasePointLeft, 0, bA));
                    Line basePad3 = new Line(GetSumPoint(eachColumnBasePointLeft, 0, bA), GetSumPoint(eachColumnBasePointRight, 0, bA));
                    Line basePad4 = new Line(GetSumPoint(eachColumnBasePointRight, 0, 0), GetSumPoint(eachColumnBasePointRight, 0, bA));
                    customBlockList.AddRange(new Line[] { basePad2, basePad3, basePad4 });

                    CDPoint eachColumnBasePointLeft2 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, radius + pipeODHalf + bD + bC + bI, ref refPoint, ref curPoint);
                    CDPoint eachColumnBasePointRight2 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, radius - pipeODHalf - bD - bC - bI, ref refPoint, ref curPoint);
                    //Line basePadPad1 = new Line(new Point3D(eachColumnBasePointLeft2.X, eachColumnBasePointLeft2.Y + bA), new Point3D(eachColumnBasePointRight2.X, eachColumnBasePointRight2.Y + bA));
                    Line basePadPad2 = new Line(GetSumPoint(eachColumnBasePointLeft2, 0, bA), GetSumPoint(eachColumnBasePointLeft2, 0, bA + bB));
                    Line basePadPad3 = new Line(GetSumPoint(eachColumnBasePointLeft2, 0, bA + bB), GetSumPoint(eachColumnBasePointRight2, 0, bA + bB));
                    Line basePadPad4 = new Line(GetSumPoint(eachColumnBasePointRight2, 0, bA), GetSumPoint(eachColumnBasePointRight2, 0, bA + bB));
                    customBlockList.AddRange(new Line[] { basePadPad2, basePadPad3, basePadPad4 });

                    CDPoint eachColumnBasePointLeft3 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, radius + pipeODHalf + bD + bC, ref refPoint, ref curPoint);
                    CDPoint eachColumnBasePointRight3 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, radius - pipeODHalf - bD - bC, ref refPoint, ref curPoint);
                    Line baseLeft1 = new Line(GetSumPoint(eachColumnBasePointLeft3, 0, bA + bB), GetSumPoint(eachColumnBasePointLeft3, 0, bA + bB + bF));
                    Line baseRight1 = new Line(GetSumPoint(eachColumnBasePointRight3, 0, bA + bB), GetSumPoint(eachColumnBasePointRight3, 0, bA + bB + bF));
                    customBlockList.AddRange(new Line[] { baseLeft1, baseRight1 });

                    CDPoint eachColumnBasePointLeft4 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, radius + pipeODHalf + bC + bG, ref refPoint, ref curPoint);
                    CDPoint eachColumnBasePointRight4 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, radius - pipeODHalf - bC - bG, ref refPoint, ref curPoint);
                    Line baseLeftLeft1 = new Line(GetSumPoint(eachColumnBasePointLeft4, 0, bA + bB), GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC - bG, bA + bB + bE - bH));
                    //Line baseLeftLeft2 = new Line(GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC - bG, +bA + bB + bE - bH), GetSumPoint(eachColumnBasePoint, 0, bA + bB + bE - bH));
                    Line baseLeftLeft3 = new Line(GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC, +bA + bB + bE - bH), GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC, +bA + bB + bE));
                    Line baseLeftLeft4 = new Line(GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC, +bA + bB + bE), GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC - bF, +bA + bB + bE));
                    Line baseLeftLeft5 = new Line(GetSumPoint(eachColumnBasePointLeft3, 0, +bA + bB + bF), GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC - bF, +bA + bB + bE));
                    customBlockList.AddRange(new Line[] { baseLeftLeft1, baseLeftLeft3, baseLeftLeft4, baseLeftLeft5 });

                    Line baseLeftRight1 = new Line(GetSumPoint(eachColumnBasePointRight4, 0, bA + bB), GetSumPoint(eachColumnBasePoint, pipeODHalf + bC + bG, bA + bB + bE - bH));
                    //Line baseLeftRight2 = new Line(GetSumPoint(eachColumnBasePoint, pipeODHalf + bC + bG, +bA + bB + bE - bH), GetSumPoint(eachColumnBasePoint, 0, bA + bB + bE - bH));
                    Line baseLeftRight3 = new Line(GetSumPoint(eachColumnBasePoint, pipeODHalf + bC, +bA + bB + bE - bH), GetSumPoint(eachColumnBasePoint, pipeODHalf + bC, +bA + bB + bE));
                    Line baseLeftRight4 = new Line(GetSumPoint(eachColumnBasePoint, pipeODHalf + bC, +bA + bB + bE), GetSumPoint(eachColumnBasePoint, pipeODHalf + bC + bF, +bA + bB + bE));
                    Line baseLeftRight5 = new Line(GetSumPoint(eachColumnBasePointRight3, 0, +bA + bB + bF), GetSumPoint(eachColumnBasePoint, pipeODHalf + bC + bF, +bA + bB + bE));
                    customBlockList.AddRange(new Line[] { baseLeftRight1, baseLeftRight3, baseLeftRight4, baseLeftRight5 });

                    Line baseHorizon1 = new Line(GetSumPoint(eachColumnBasePoint, -pipeODHalf - bC - bG, +bA + bB + bE - bH), GetSumPoint(eachColumnBasePoint, pipeODHalf + bC + bG, bA + bB + bE - bH));
                    customBlockList.Add(baseHorizon1);
                    #endregion

                    #region Column
                    Line columnLeft = new Line(GetSumPoint(eachColumnPoint, -pipeODHalf, -D - E), GetSumPoint(eachColumnBasePoint, -pipeODHalf, +bA + bB + bE - bH));
                    Line columnRight = new Line(GetSumPoint(eachColumnPoint, pipeODHalf, -D - E), GetSumPoint(eachColumnBasePoint, pipeODHalf, +bA + bB + bE - bH));
                    customBlockList.AddRange(new Line[] { columnLeft, columnRight });
                    #endregion

                    // 중심선
                    CDPoint eachColumnPointCenterLine = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofUp, radius, ref refPoint, ref curPoint);
                    CDPoint eachColumnBasePointCenterLine = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomDown, radius, ref refPoint, ref curPoint);
                    Line centerLine01 = new Line(GetSumPoint(eachColumnPointCenterLine, 0, 0), GetSumPoint(eachColumnBasePointCenterLine, 0, 0));
                    DrawCenterLineModel centerLineScale = new DrawCenterLineModel();
                    Line centerLine02 = new Line(GetSumPoint(eachColumnPointCenterLine, 0, 0), GetSumPoint(eachColumnPointCenterLine, 0, centerLineScale.exLength * scaleValue));
                    Line centerLine03 = new Line(GetSumPoint(eachColumnBasePointCenterLine, 0, 0), GetSumPoint(eachColumnBasePointCenterLine, 0, -centerLineScale.exLength * scaleValue));
                    styleService.SetLayer(ref centerLine01, layerService.LayerCenterLine);
                    styleService.SetLayer(ref centerLine02, layerService.LayerCenterLine);
                    styleService.SetLayer(ref centerLine03, layerService.LayerCenterLine);

                    customLine.AddRange(new Line[] { centerLine01, centerLine02, centerLine03 });

                    // Leader
                    SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                    {
                        leaderPoint = GetSumCDPoint(columnLeft.EndPoint, 0, columnLeft.Length() * 85 / 100, 0),
                        lineTextList = new List<string>() {assemblyData.StructureCRTColumnInput[i].Qty + "-" + valueService.GetOrdinalNumber(i+2) + " COLUMNS",
                                                            assemblyData.StructureCRTColumnInput[i].Size + "\" " +  assemblyData.StructureCRTColumnInput[i].Schedule},
                        lineLength = -50,
                        Position = "bottomleft"
                    });

                    if (columnHeightFordimension == 0)
                        columnHeightFordimension = columnRight.Length() * 75 / 100;
                    CDPoint columnDimPoint = new CDPoint(columnRight.EndPoint.X - pipeODHalf, refPoint.Y + columnHeightFordimension, 0);
                    // Dimension
                    SingletonData.DimPublicList.Add(new DimensionPointModel()
                    {
                        leftPoint = GetSumCDPoint(columnDimPoint, 0, 0),
                        rightPoint = GetSumCDPoint(columnDimPoint, scaleValue * 30, 0),
                        dimHeight = 0,
                        Text = "R" + radius,
                        Position = "top",
                        leftArrowVisible = true,
                        rightArrowVisible = false,
                        extVisible = false,
                        middleValue = 0
                    });
                }
                #endregion

                #region Column Center

                StructureCRTColumnCenterModel centerTopSupport = new StructureCRTColumnCenterModel();
                if (assemblyData.StructureCRTColumnCenterOutput.Count > 0)
                    centerTopSupport = assemblyData.StructureCRTColumnCenterOutput[firstIndex];

                double tsSize = valueService.GetDoubleValue(centerTopSupport.COLUMN);
                double tsA = valueService.GetDoubleValue(centerTopSupport.A);
                double tsB = valueService.GetDoubleValue(centerTopSupport.B);
                double tsC = valueService.GetDoubleValue(centerTopSupport.C);
                double tsD = valueService.GetDoubleValue(centerTopSupport.D);
                double tsE = valueService.GetDoubleValue(centerTopSupport.E);
                double tsF = valueService.GetDoubleValue(centerTopSupport.F);
                double tsG = valueService.GetDoubleValue(centerTopSupport.G);
                double tsH = valueService.GetDoubleValue(centerTopSupport.H);
                double tsI = valueService.GetDoubleValue(centerTopSupport.I);
                double tsJ = valueService.GetDoubleValue(centerTopSupport.J);
                double tsK = valueService.GetDoubleValue(centerTopSupport.K);

                // A1은 사용 안함
                double padPushWidth = valueService.GetDoubleValue(centerTopSupport.B1);
                double triEdge = valueService.GetDoubleValue(centerTopSupport.C1);
                double padHeight = valueService.GetDoubleValue(centerTopSupport.D1);
                double smallTri = valueService.GetDoubleValue(centerTopSupport.chamferLength);
                smallTri = 0;

                double boltHoleHeight = valueService.GetDoubleValue(centerTopSupport.SlotHoleWidth);
                double boltHoleWidth = valueService.GetDoubleValue(centerTopSupport.SlotHoleLength);

                StructureCRTColumnRafterModel centerFirstRafter = new StructureCRTColumnRafterModel();
                if (assemblyData.StructureCRTColumnRafterOutput.Count > 0)
                    centerFirstRafter = assemblyData.StructureCRTColumnRafterOutput[firstIndex];

                double rA = valueService.GetDoubleValue(centerFirstRafter.A);
                double rB = valueService.GetDoubleValue(centerFirstRafter.B);
                double rC = valueService.GetDoubleValue(centerFirstRafter.C);
                double rD = valueService.GetDoubleValue(centerFirstRafter.D);
                double rE = valueService.GetDoubleValue(centerFirstRafter.E);
                double rBoltHoleOnShell = valueService.GetDoubleValue(centerFirstRafter.BoltHoleOnShell);
                double rBoltHoleOnColumn = valueService.GetDoubleValue(centerFirstRafter.BoltHoleOnColumn);
                double rBoltHoleOnCenter = valueService.GetDoubleValue(centerFirstRafter.BoltHoleOnCenter);
                double rBoltHoleDia = valueService.GetDoubleValue(centerFirstRafter.BoltHoleDia);

                // WP : Left Square Center
                centerLeftWidthHalf = tsB;
                CDPoint centerTopRoofLeftB = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, centerRadius + centerLeftWidthHalf, ref refPoint, ref curPoint);


                // WP : Left Square Center : Center
                double centerRafterHeightHalf = 0;
                if (rBoltHoleOnCenter == 2)
                    centerRafterHeightHalf = rA / 2;
                else
                    centerRafterHeightHalf = (tsK + (tsJ / 2));

                double raHeightHalf = valueService.GetHypotenuseByWidth(roofSlopeDegree, centerRafterHeightHalf);
                CDPoint centerTopLeftSquare = GetSumCDPoint(centerTopRoofLeftB, 0, -raHeightHalf);

                if (rBoltHoleOnCenter == 4)
                {
                    centerTopLeftSquare = GetSumCDPoint(centerTopLeftSquare, -valueService.GetOppositeByHypotenuse(roofSlopeDegree, tsJ / 2), +valueService.GetAdjacentByHypotenuse(roofSlopeDegree, tsJ / 2));
                }

                // WP : Left Pad 
                double centerLeftWidthODHalf = centerPipeODHalf + tsG + 30;// 30 값 고정
                CDPoint centerTopRoofLeft = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, centerRadius + centerLeftWidthODHalf, ref refPoint, ref curPoint);

                // Square : 평행 방향이동 : 아래쪽
                double centerRafterHeight = rA + 20; // MinValue : 20
                double raHeight = valueService.GetHypotenuseByWidth(roofSlopeDegree, centerRafterHeight);
                CDPoint centerTopLeft = GetSumCDPoint(centerTopRoofLeft, 0, -raHeight);


                // Center Top Support : Square
                Line lineSquare1 = new Line(GetSumPoint(centerTopLeftSquare, -tsC / 2, tsE), GetSumPoint(centerTopLeftSquare, tsC / 2, tsE));
                Line lineSquare2 = new Line(GetSumPoint(centerTopLeftSquare, -tsC / 2, tsE), new Point3D(centerTopLeftSquare.X - tsC / 2, centerTopLeft.Y));
                Line lineSquare3 = new Line(GetSumPoint(centerTopLeftSquare, tsC / 2, tsE), new Point3D(centerTopLeftSquare.X + tsC / 2, centerTopLeft.Y));
                customBlockList.AddRange(new Line[] { lineSquare1, lineSquare2, lineSquare3 });


                // Center Top Support : Bolt Hole : 직각 방향 이동 : 90도 아래
                double ellipseWidth = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, tsD / 2);
                double ellipseHeight = valueService.GetOppositeByHypotenuse(roofSlopeDegree, tsD / 2);
                Point3D centerEllipseLeft = GetSumPoint(centerTopLeftSquare, -ellipseWidth, -ellipseHeight);
                Point3D centerEllipseRight = GetSumPoint(centerTopLeftSquare, ellipseWidth, ellipseHeight);

                CompositeCurve leftBolt1 = GetBoltHoleHorizontal(centerEllipseLeft, boltHoleWidth, boltHoleHeight);
                leftBolt1.Rotate(roofSlopeDegree, Vector3D.AxisZ, centerEllipseLeft);
                CompositeCurve leftBolt2 = GetBoltHoleHorizontal(centerEllipseRight, boltHoleWidth, boltHoleHeight);
                leftBolt2.Rotate(roofSlopeDegree, Vector3D.AxisZ, centerEllipseRight);
                customBlockList.AddRange(new CompositeCurve[] { leftBolt1, leftBolt2 });

                //Circle leftBoltcircle1 = new Circle(centerEllipseLeft, boltHoleHeight / 2);
                //Circle leftBoltcircle2 = new Circle(centerEllipseRight, boltHoleHeight / 2);
                //customBlockList.AddRange(new Circle[] { leftBoltcircle1, leftBoltcircle2 });

                List<Entity> circle01 = DrawReference_Hole(centerEllipseLeft, boltHoleHeight / 2, roofSlopeDegree, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                List<Entity> circle02 = DrawReference_Hole(centerEllipseRight, boltHoleHeight / 2, roofSlopeDegree, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                customLine.AddRange(circle01);
                customLine.AddRange(circle02);

                if (rBoltHoleOnCenter == 4)
                {
                    Point3D centerEllipseLeft1 = GetSumPoint(centerEllipseLeft, +valueService.GetOppositeByHypotenuse(roofSlopeDegree, tsJ), -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, tsJ));
                    Point3D centerEllipseRight1 = GetSumPoint(centerEllipseRight, +valueService.GetOppositeByHypotenuse(roofSlopeDegree, tsJ), -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, tsJ));

                    CompositeCurve leftBolt11 = GetBoltHoleHorizontal(centerEllipseLeft1, boltHoleWidth, boltHoleHeight);
                    leftBolt11.Rotate(roofSlopeDegree, Vector3D.AxisZ, centerEllipseLeft1);
                    CompositeCurve leftBolt21 = GetBoltHoleHorizontal(centerEllipseRight1, boltHoleWidth, boltHoleHeight);
                    leftBolt21.Rotate(roofSlopeDegree, Vector3D.AxisZ, centerEllipseRight1);
                    customBlockList.AddRange(new CompositeCurve[] { leftBolt11, leftBolt21 });

                    //Circle leftBoltcircle11 = new Circle(centerEllipseLeft1, boltHoleHeight / 2);
                    //Circle leftBoltcircle21 = new Circle(centerEllipseRight1, boltHoleHeight / 2);
                    //customBlockList.AddRange(new Circle[] { leftBoltcircle11, leftBoltcircle21 });

                    List<Entity> circle03 = DrawReference_Hole(centerEllipseLeft1, boltHoleHeight / 2, roofSlopeDegree, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                    List<Entity> circle04 = DrawReference_Hole(centerEllipseRight1, boltHoleHeight / 2, roofSlopeDegree, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                    customLine.AddRange(circle03);
                    customLine.AddRange(circle04);
                }

                // Center Top Support : Pad


                Line topPad1 = new Line(GetSumPoint(centerTopLeft, 0, 0), GetSumPoint(centerTopLeft, centerLeftWidthODHalf, 0));
                Line topPad2 = new Line(GetSumPoint(centerTopLeft, 0, -padHeight), GetSumPoint(centerTopLeft, centerLeftWidthODHalf, -padHeight));
                Line topPad3 = new Line(GetSumPoint(centerTopLeft, 0, 0), GetSumPoint(centerTopLeft, 0, -padHeight));
                customBlockList.AddRange(new Line[] { topPad1, topPad2, topPad3 });

                Line topTriLeft1 = new Line(GetSumPoint(centerTopLeft, padPushWidth, -padHeight), GetSumPoint(centerTopLeft, padPushWidth, -padHeight - triEdge));
                Line topTriLeft2 = new Line(GetSumPoint(centerTopLeft, padPushWidth + tsG - smallTri, -padHeight), GetSumPoint(centerTopLeft, padPushWidth + tsG, -padHeight - smallTri));
                Line topTriLeft3 = new Line(GetSumPoint(centerTopLeft, padPushWidth + tsG, -padHeight - smallTri), GetSumPoint(centerTopLeft, padPushWidth + tsG, -padHeight - tsH));



                Line topTriLeft4 = new Line(GetSumPoint(centerTopLeft, padPushWidth + tsG - triEdge, -padHeight - tsH), GetSumPoint(centerTopLeft, padPushWidth + tsG, -padHeight - tsH)); // center column top
                Line topTriLeft5 = new Line(GetSumPoint(centerTopLeft, padPushWidth + tsG - triEdge, -padHeight - tsH), GetSumPoint(centerTopLeft, padPushWidth, -padHeight - triEdge));

                customBlockList.AddRange(new Line[] { topTriLeft1, topTriLeft2, topTriLeft3, topTriLeft4, topTriLeft5 });



                // Support Buttom Support
                CDPoint centerColumnBasePoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, centerRadius, ref refPoint, ref curPoint);
                drawPoint.X = centerColumnBasePoint.X;
                drawPoint.Y = centerColumnBasePoint.Y;

                CDPoint centerColumnBasePointLeft = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, centerRadius + centerPipeODHalf + bD + bC + bJ, ref refPoint, ref curPoint);
                Line centerPad2 = new Line(GetSumPoint(centerColumnBasePointLeft, 0, 0), GetSumPoint(centerColumnBasePointLeft, 0, bA));
                Line centerPad3 = new Line(GetSumPoint(centerColumnBasePointLeft, 0, bA), GetSumPoint(centerColumnBasePoint, 0, bA));
                Line centerPad4 = new Line(GetSumPoint(centerColumnBasePoint, 0, 0), GetSumPoint(centerColumnBasePoint, 0, bA));
                customBlockList.AddRange(new Line[] { centerPad2, centerPad3, centerPad4 });

                CDPoint centerColumnBasePointLeft2 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, centerRadius + centerPipeODHalf + bD + bC + bI, ref refPoint, ref curPoint);
                Line centerPadPad2 = new Line(GetSumPoint(centerColumnBasePointLeft2, 0, bA), GetSumPoint(centerColumnBasePointLeft2, 0, bA + bB));
                Line centerPadPad3 = new Line(GetSumPoint(centerColumnBasePointLeft2, 0, bA + bB), GetSumPoint(centerColumnBasePoint, 0, bA + bB));
                Line centerPadPad4 = new Line(GetSumPoint(centerColumnBasePoint, 0, bA), GetSumPoint(centerColumnBasePoint, 0, bA + bB));
                customBlockList.AddRange(new Line[] { centerPadPad2, centerPadPad3, centerPadPad4 });

                CDPoint centerColumnBasePointLeft3 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, centerRadius + centerPipeODHalf + bD + bC, ref refPoint, ref curPoint);
                Line centerLeft1 = new Line(GetSumPoint(centerColumnBasePointLeft3, 0, bA + bB), GetSumPoint(centerColumnBasePointLeft3, 0, bA + bB + bF));
                customBlockList.AddRange(new Line[] { centerLeft1 });

                CDPoint centerColumnBasePointLeft4 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, centerRadius + centerPipeODHalf + bC + bG, ref refPoint, ref curPoint);
                Line centerLeftLeft1 = new Line(GetSumPoint(centerColumnBasePointLeft4, 0, bA + bB), GetSumPoint(centerColumnBasePoint, -centerPipeODHalf - bC - bG, bA + bB + bE - bH));
                Line centerLeftLeft2 = new Line(GetSumPoint(centerColumnBasePoint, -centerPipeODHalf - bC - bG, +bA + bB + bE - bH), GetSumPoint(centerColumnBasePoint, 0, bA + bB + bE - bH));
                Line centerLeftLeft3 = new Line(GetSumPoint(centerColumnBasePoint, -centerPipeODHalf - bC, +bA + bB + bE - bH), GetSumPoint(centerColumnBasePoint, -centerPipeODHalf - bC, +bA + bB + bE));
                Line centerLeftLeft4 = new Line(GetSumPoint(centerColumnBasePoint, -centerPipeODHalf - bC, +bA + bB + bE), GetSumPoint(centerColumnBasePoint, -centerPipeODHalf - bC - bF, +bA + bB + bE));
                Line centerLeftLeft5 = new Line(GetSumPoint(centerColumnBasePointLeft3, 0, +bA + bB + bF), GetSumPoint(centerColumnBasePoint, -centerPipeODHalf - bC - bF, +bA + bB + bE));
                customBlockList.AddRange(new Line[] { centerLeftLeft1, centerLeftLeft2, centerLeftLeft3, centerLeftLeft4, centerLeftLeft5 });

                // Column  : Column Center Top Support 아래쪽 이랑 Base Support 연결
                Line centerColumnLeft = new Line(GetSumPoint(centerTopLeft, padPushWidth + tsG, -padHeight - tsH), GetSumPoint(centerColumnBasePoint, -centerPipeODHalf, +bA + bB + bE - bH));
                customBlockList.AddRange(new Line[] { centerColumnLeft });

                #endregion


                // Leader
                SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                {
                    leaderPoint = GetSumCDPoint(centerColumnLeft.EndPoint, 0, centerColumnLeft.Length() * 85 / 100, 0),
                    lineTextList = new List<string>() {assemblyData.StructureCRTColumnInput[0].Qty+ "-" + valueService.GetOrdinalNumber(1)  + " COLUMNS",
                                                            assemblyData.StructureCRTColumnInput[0].Size + "\" " +  assemblyData.StructureCRTColumnInput[0].Schedule},
                    lineLength = -50,
                    Position = "bottomleft"
                });

            }
            else if (StructureDivService.columnType == "centering")
            {

                StructureCRTCenteringInputModel centeringInput = new StructureCRTCenteringInputModel();
                if (assemblyData.StructureCRTCenterRingInput.Count > 0)
                    centeringInput = assemblyData.StructureCRTCenterRingInput[firstIndex];

                StructureCenteringModel centeringOutput = new StructureCenteringModel();
                if (assemblyData.StructureCRTCenteringOutput.Count > 0)
                    centeringOutput = assemblyData.StructureCRTCenteringOutput[firstIndex];

                double centeringOD = valueService.GetDoubleValue(centeringInput.CenteringOD);
                double centeringID = 0;// 계산함
                double centeringA = valueService.GetDoubleValue(centeringOutput.A);
                double centeringB = valueService.GetDoubleValue(centeringOutput.B);
                double centeringC = valueService.GetDoubleValue(centeringOutput.C);
                double centeringD = valueService.GetDoubleValue(centeringOutput.D);
                double centeringE = valueService.GetDoubleValue(centeringOutput.E);
                double centeringT1 = valueService.GetDoubleValue(centeringOutput.t1);
                double centeringT2 = valueService.GetDoubleValue(centeringOutput.t2);
                //Cal

                centeringID = (centeringOD - (centeringD * 2)) / 2;
                centeringE = (centeringOD + (centeringC * 2)) / 2;
                double centeringIDHalf = centeringOD / 2;

                if (StructureDivService.centeringInEx == "internal")
                {
                    #region CenterRing


                    CDPoint centeringWP = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, centerRadius + centeringE, ref refPoint, ref curPoint);

                    Line cLine01 = new Line(GetSumPoint(centeringWP, 0, 0), GetSumPoint(centeringWP, centeringE, 0));
                    Line cLine02 = new Line(GetSumPoint(centeringWP, 0, -centeringT1), GetSumPoint(centeringWP, centeringE, -centeringT1));
                    Line cLine03 = new Line(GetSumPoint(centeringWP, 0, -centeringA + centeringT1), GetSumPoint(centeringWP, centeringE, -centeringA + centeringT1));
                    Line cLine04 = new Line(GetSumPoint(centeringWP, 0, -centeringA), GetSumPoint(centeringWP, centeringE, -centeringA));

                    Line cLineV01 = new Line(GetSumPoint(centeringWP, 0, 0), GetSumPoint(centeringWP, 0, -centeringT1));
                    Line cLineV02 = new Line(GetSumPoint(centeringWP, centeringC + centeringD, 0), GetSumPoint(centeringWP, centeringC + centeringD, -centeringT1));
                    Line cLineV03 = new Line(GetSumPoint(centeringWP, 0, -centeringA + centeringT1), GetSumPoint(centeringWP, 0, -centeringA));
                    Line cLineV04 = new Line(GetSumPoint(centeringWP, centeringC + centeringD, -centeringA + centeringT1), GetSumPoint(centeringWP, centeringC + centeringD, -centeringA));

                    Line cLineLongV01 = new Line(GetSumPoint(centeringWP, centeringC, -centeringT1), GetSumPoint(centeringWP, centeringC, -centeringA + centeringT1));
                    Line cLineLongV02 = new Line(GetSumPoint(centeringWP, centeringC + centeringT2, -centeringT1), GetSumPoint(centeringWP, centeringC + centeringT2, -centeringA + centeringT1));

                    customBlockList.AddRange(new Line[] { cLine01, cLine02, cLine03, cLine04, cLineV01, cLineV02, cLineV03, cLineV04, cLineLongV01, cLineLongV02 });
                    #endregion

                    // Leader
                    SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                    {
                        leaderPoint = GetSumCDPoint(cLine01.EndPoint, 0, 0),
                        lineTextList = new List<string>() {"C350x100x11x11",
                                                            "CENTER RING"
                                                             },
                        lineLength = 30,
                        Position = "topleft"
                    });


                    #region Clip Center Side
                    StructureClipCenteringSideModel centeringSideClip = new StructureClipCenteringSideModel();
                    if (assemblyData.StructureCRTClipCenteringSideOutput.Count > 0)
                        centeringSideClip = assemblyData.StructureCRTClipCenteringSideOutput[firstIndex];

                    double cClipA = valueService.GetDoubleValue(centeringSideClip.A);
                    double cClipB = valueService.GetDoubleValue(centeringSideClip.B);
                    double cClipC = valueService.GetDoubleValue(centeringSideClip.C);
                    double cClipD = valueService.GetDoubleValue(centeringSideClip.D);
                    double cClipHoleQty = valueService.GetDoubleValue(centeringSideClip.HoleQty);
                    double cClipA1 = valueService.GetDoubleValue(centeringSideClip.A1);
                    double cClipB1 = valueService.GetDoubleValue(centeringSideClip.B1);
                    double cClipC1 = valueService.GetDoubleValue(centeringSideClip.C1);
                    double cClipD1 = valueService.GetDoubleValue(centeringSideClip.D1);
                    double cClipE1 = valueService.GetDoubleValue(centeringSideClip.E1);
                    double cClipF1 = valueService.GetDoubleValue(centeringSideClip.F1);

                    double boltHoleHeight = valueService.GetDoubleValue(centeringSideClip.SlotHoleHt);
                    double boltHoleWidth = valueService.GetDoubleValue(centeringSideClip.SlotHoleWd);

                    // Gap : A1 : 7
                    double centeringGapVertiY = valueService.GetHypotenuseByWidth(roofSlopeDegree, cClipA1);
                    double centeringGapHeightY = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cClipA1);
                    double centeringGapWidthX = valueService.GetOppositeByHypotenuse(roofSlopeDegree, cClipA1);


                    double centeringclipB1MiddelX = valueService.GetAdjacentByHeight(roofSlopeDegree, centeringT1);
                    double centeringclipGapX = valueService.GetAdjacentByHeight(roofSlopeDegree, cClipA1);
                    double centeringclipGapXX = valueService.GetHypotenuseByWidth(roofSlopeDegree, centeringclipGapX);
                    CDPoint centeringClipRightReal = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, centerRadius + centeringE + centeringclipB1MiddelX, ref refPoint, ref curPoint);
                    Point3D centeringClipRightRealPoint = GetSumPoint(centeringClipRightReal, centeringclipGapXX, 0);


                    double firstWidthX = valueService.GetOppositeByWidth(roofSlopeDegree, cClipC);
                    double middleWidth = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cClipD1 + cClipE1 - firstWidthX);
                    CDPoint centeringClipLeft = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, centerRadius + centeringE + middleWidth + cClipC1 + cClipB1, ref refPoint, ref curPoint);
                    Point3D centeringClipLeftPoint = GetSumPoint(centeringClipLeft, 0, -centeringGapVertiY);


                    Line slopeLine01 = new Line(centeringClipRightRealPoint, centeringClipLeftPoint);
                    Line clipLine01 = new Line(centeringClipRightRealPoint, GetSumPoint(centeringWP, +centeringC - cClipF1, -centeringT1));
                    Line clipLine02 = new Line(GetSumPoint(centeringWP, +centeringC - cClipF1, -centeringT1), GetSumPoint(centeringWP, +centeringC, -centeringT1 - cClipF1));
                    Line clipLine03 = new Line(GetSumPoint(centeringWP, +centeringC, -centeringA + centeringT1 + cClipF1), GetSumPoint(centeringWP, +centeringC, -centeringT1 - cClipF1));
                    Line clipLine04 = new Line(GetSumPoint(centeringWP, +centeringC, -centeringA + centeringT1 + cClipF1), GetSumPoint(centeringWP, +centeringC - cClipF1, -centeringA + centeringT1));

                    Point3D centeringClipLeftDownPoint = new Point3D(centeringClipLeftPoint.X, GetSumPoint(centeringWP, 0, -centeringA + centeringT1).Y);
                    Line clipLine05 = new Line(centeringClipLeftDownPoint, GetSumPoint(centeringWP, +centeringC - cClipF1, -centeringA + centeringT1));
                    Line clipLine06 = new Line(centeringClipLeftDownPoint, centeringClipLeftPoint);
                    customBlockList.AddRange(new Line[] { slopeLine01, clipLine01, clipLine02, clipLine03, clipLine04, clipLine05, clipLine06 });

                    // B1 만큼 위치
                    centeringClipRight = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, centerRadius + centeringE + cClipB1, ref refPoint, ref curPoint);

                    if (cClipHoleQty == 2)
                    {
                        // 직각 방향 이동: 아래쪽
                        Point3D clipSideHoleFirstPoint1 = GetSumPoint(centeringClipRight, +valueService.GetOppositeByHypotenuse(roofSlopeDegree, cClipC),
                                                                                          -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cClipC));
                        Point3D clipSideHoleFirstPoint2 = GetSumPoint(clipSideHoleFirstPoint1, -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cClipE1),
                                                                                               -valueService.GetOppositeByHypotenuse(roofSlopeDegree, cClipE1));
                        Point3D clipSideHoleFirstPoint3 = GetSumPoint(clipSideHoleFirstPoint2, -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cClipD1),
                                                                                               -valueService.GetOppositeByHypotenuse(roofSlopeDegree, cClipD1));

                        CompositeCurve clipSideBoltFirst1 = GetBoltHoleHorizontal(clipSideHoleFirstPoint2, boltHoleWidth, boltHoleHeight);
                        clipSideBoltFirst1.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleFirstPoint2);
                        CompositeCurve clipSideBoltFirst2 = GetBoltHoleHorizontal(clipSideHoleFirstPoint3, boltHoleWidth, boltHoleHeight);
                        clipSideBoltFirst2.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleFirstPoint3);
                        customBlockList.AddRange(new CompositeCurve[] { clipSideBoltFirst1, clipSideBoltFirst2 });

                        //Circle ClipSideBoltFirstCicle1 = new Circle(clipSideHoleFirstPoint2, boltHoleHeight / 2);
                        //Circle ClipSideBoltFirstCicle2 = new Circle(clipSideHoleFirstPoint3, boltHoleHeight / 2);
                        //customBlockList.AddRange(new Circle[] { ClipSideBoltFirstCicle1, ClipSideBoltFirstCicle2 });
                        List<Entity> circle01 = DrawReference_Hole(clipSideHoleFirstPoint2, boltHoleHeight / 2, roofSlopeDegree, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                        List<Entity> circle02 = DrawReference_Hole(clipSideHoleFirstPoint3, boltHoleHeight / 2, roofSlopeDegree, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                        customLine.AddRange(circle01);
                        customLine.AddRange(circle02);

                    }
                    else if (cClipHoleQty == 4)
                    {
                        // 직각 방향 이동: 아래쪽
                        Point3D clipSideHoleFirstPoint1 = GetSumPoint(centeringClipRight, +valueService.GetOppositeByHypotenuse(roofSlopeDegree, cClipC),
                                                                                          -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cClipC));
                        Point3D clipSideHoleFirstPoint2 = GetSumPoint(clipSideHoleFirstPoint1, -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cClipE1),
                                                                                               -valueService.GetOppositeByHypotenuse(roofSlopeDegree, cClipE1));
                        Point3D clipSideHoleFirstPoint3 = GetSumPoint(clipSideHoleFirstPoint2, -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cClipD1),
                                                                                               -valueService.GetOppositeByHypotenuse(roofSlopeDegree, cClipD1));

                        CompositeCurve clipSideBoltFirst1 = GetBoltHoleHorizontal(clipSideHoleFirstPoint2, boltHoleWidth, boltHoleHeight);
                        clipSideBoltFirst1.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleFirstPoint2);
                        CompositeCurve clipSideBoltFirst2 = GetBoltHoleHorizontal(clipSideHoleFirstPoint3, boltHoleWidth, boltHoleHeight);
                        clipSideBoltFirst2.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleFirstPoint3);
                        customBlockList.AddRange(new CompositeCurve[] { clipSideBoltFirst1, clipSideBoltFirst2 });

                        //Circle ClipSideBoltFirstCicle1 = new Circle(clipSideHoleFirstPoint2, boltHoleHeight / 2);
                        //Circle ClipSideBoltFirstCicle2 = new Circle(clipSideHoleFirstPoint3, boltHoleHeight / 2);
                        //customBlockList.AddRange(new Circle[] { ClipSideBoltFirstCicle1, ClipSideBoltFirstCicle2 });
                        List<Entity> circle01 = DrawReference_Hole(clipSideHoleFirstPoint2, boltHoleHeight / 2, roofSlopeDegree, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                        List<Entity> circle02 = DrawReference_Hole(clipSideHoleFirstPoint3, boltHoleHeight / 2, roofSlopeDegree, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                        customLine.AddRange(circle01);
                        customLine.AddRange(circle02);


                        // 직각 방향 이동: 아래쪽
                        Point3D clipSideHoleFirstPoint11 = GetSumPoint(centeringClipRight, +valueService.GetOppositeByHypotenuse(roofSlopeDegree, cClipC + cClipD),
                                                                                           -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cClipC + cClipD));
                        Point3D clipSideHoleFirstPoint21 = GetSumPoint(clipSideHoleFirstPoint11, -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cClipE1),
                                                                                                 -valueService.GetOppositeByHypotenuse(roofSlopeDegree, cClipE1));
                        Point3D clipSideHoleFirstPoint31 = GetSumPoint(clipSideHoleFirstPoint21, -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cClipD1),
                                                                                                 -valueService.GetOppositeByHypotenuse(roofSlopeDegree, cClipD1));

                        CompositeCurve clipSideBoltFirst11 = GetBoltHoleHorizontal(clipSideHoleFirstPoint21, boltHoleWidth, boltHoleHeight);
                        clipSideBoltFirst11.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleFirstPoint21);
                        CompositeCurve clipSideBoltFirst21 = GetBoltHoleHorizontal(clipSideHoleFirstPoint31, boltHoleWidth, boltHoleHeight);
                        clipSideBoltFirst21.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleFirstPoint31);
                        customBlockList.AddRange(new CompositeCurve[] { clipSideBoltFirst11, clipSideBoltFirst21 });

                        //Circle ClipSideBoltFirstCicle11 = new Circle(clipSideHoleFirstPoint21, boltHoleHeight / 2);
                        //Circle ClipSideBoltFirstCicle21 = new Circle(clipSideHoleFirstPoint31, boltHoleHeight / 2);
                        //customBlockList.AddRange(new Circle[] { ClipSideBoltFirstCicle11, ClipSideBoltFirstCicle21 });
                        List<Entity> circle011 = DrawReference_Hole(clipSideHoleFirstPoint21, boltHoleHeight / 2, roofSlopeDegree, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                        List<Entity> circle021 = DrawReference_Hole(clipSideHoleFirstPoint31, boltHoleHeight / 2, roofSlopeDegree, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                        customLine.AddRange(circle011);
                        customLine.AddRange(circle021);
                    }
                    #endregion
                }
                else
                {
                    //external
                    #region CenterRing

                    CDPoint centeringWP = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofUp, centerRadius + centeringIDHalf, ref refPoint, ref curPoint);

                    Line cLine01 = new Line(GetSumPoint(centeringWP, -centeringB, centeringA), GetSumPoint(centeringWP, +centeringIDHalf, centeringA));
                    Line cLine02 = new Line(GetSumPoint(centeringWP, -centeringB, centeringA - centeringT1), GetSumPoint(centeringWP, +centeringIDHalf, centeringA - centeringT1));
                    Line cLine03 = new Line(GetSumPoint(centeringWP, -centeringB, centeringA), GetSumPoint(centeringWP, -centeringB, centeringA - centeringT1));
                    Line cLine04 = new Line(GetSumPoint(centeringWP, +centeringC, centeringA), GetSumPoint(centeringWP, +centeringC, centeringA - centeringT1));

                    double centeringPadVertiY = valueService.GetHypotenuseByWidth(roofSlopeDegree, centeringT1);
                    double centeringPadHeightY = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, centeringT1);
                    double centeringPadWidthX = valueService.GetOppositeByHypotenuse(roofSlopeDegree, centeringT1);

                    double centeringVertiHeightY = valueService.GetOppositeByWidth(roofSlopeDegree, centeringT2);


                    Line cLineLongV01 = new Line(GetSumPoint(centeringWP, 0, centeringPadVertiY), GetSumPoint(centeringWP, 0, centeringA - centeringT1));
                    Line cLineLongV02 = new Line(GetSumPoint(centeringWP, centeringT2, centeringVertiHeightY + centeringPadVertiY), GetSumPoint(centeringWP, centeringT2, centeringA - centeringT1));

                    double centeringPadBottomBX = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, centeringB);
                    double centeringPadBottomCX = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, centeringC);

                    CDPoint centeringPadBottonLeft = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofUp, centerRadius + centeringIDHalf + centeringPadBottomBX, ref refPoint, ref curPoint);
                    CDPoint centeringPadBottonRight = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofUp, centerRadius + centeringIDHalf - centeringPadBottomCX, ref refPoint, ref curPoint);

                    Line cLineV01 = new Line(GetSumPoint(centeringPadBottonLeft, -centeringPadWidthX, centeringPadHeightY), GetSumPoint(centeringPadBottonRight, -centeringPadWidthX, centeringPadHeightY));
                    Line cLineV02 = new Line(GetSumPoint(centeringPadBottonRight, 0, 0), GetSumPoint(centeringPadBottonRight, -centeringPadWidthX, centeringPadHeightY));
                    Line cLineV03 = new Line(GetSumPoint(centeringPadBottonLeft, 0, 0), GetSumPoint(centeringPadBottonLeft, -centeringPadWidthX, centeringPadHeightY));
                    Line cLineV04 = new Line(GetSumPoint(centeringPadBottonLeft, 0, 0), GetSumPoint(centeringPadBottonRight, 0, 0));


                    customBlockList.AddRange(new Line[] { cLine01, cLine02, cLine03, cLine04, cLineV01, cLineV02, cLineV03, cLineV04, cLineLongV01, cLineLongV02 });
                    #endregion

                    // Leader
                    SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                    {
                        leaderPoint = GetSumCDPoint(cLine01.EndPoint, 0, 0),
                        lineTextList = new List<string>() {"C350x100x11x11",
                                                            "CENTER RING"
                                                             },
                        lineLength = 30,
                        Position = "topleft"
                    });


                    #region Rafter : Centering : external
                    StructureCenteringRafterModel centeringRafter = new StructureCenteringRafterModel();
                    ChannelModel eachChannel = new ChannelModel();
                    if (assemblyData.StructureCRTCenteringRaterOutput.Count > 0)
                    {
                        centeringRafter = assemblyData.StructureCRTCenteringRaterOutput[firstIndex];
                        eachChannel = GetChannel(assemblyData.StructureCRTCenteringRaterOutput[firstIndex].SIZE);
                    }

                    double cRafterA = valueService.GetDoubleValue(centeringRafter.A);
                    double cRafterA1 = valueService.GetDoubleValue(centeringRafter.A1);
                    double cRafterB1 = valueService.GetDoubleValue(centeringRafter.B1);

                    CDPoint centeringRafterLeftDown = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointLeftRoofUp, ref refPoint, ref curPoint);
                    CDPoint centeringRafterLeftUP = GetSumCDPoint(centeringRafterLeftDown, -valueService.GetOppositeByHypotenuse(roofSlopeDegree, cRafterA), valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cRafterA));
                    CDPoint centeringRafterRightDown = GetSumCDPoint(centeringWP, -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cRafterA1 + centeringB), -valueService.GetOppositeByHypotenuse(roofSlopeDegree, cRafterA1 + centeringB));
                    CDPoint centeringRafterRightUP = GetSumCDPoint(workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofUp, centerRadius + centeringE + cRafterA1, ref refPoint, ref curPoint),
                                                                   0, valueService.GetHypotenuseByWidth(roofSlopeDegree, cRafterA));
                    CDPoint centeringRafterRightRightUp = GetSumCDPoint(centeringWP, -centeringB - cRafterA1, centeringA - centeringT1 - cRafterB1);
                    CDPoint centeringRafterRightRightRightUp = GetSumCDPoint(centeringWP, 0, centeringA - centeringT1 - cRafterB1);

                    double centeringRafeterBottomVerti = valueService.GetHypotenuseByWidth(roofSlopeDegree, centeringT1) + valueService.GetHypotenuseByWidth(roofSlopeDegree, cRafterB1);
                    double centeringRafeterBottomVerti2 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, centeringRafeterBottomVerti);
                    CDPoint centeringRafterRightRightRightDown = GetSumCDPoint(centeringWP, 0, centeringRafeterBottomVerti);
                    CDPoint centeringRafterRightRightDown = GetSumCDPoint(centeringRafterRightDown, -valueService.GetOppositeByHypotenuse(roofSlopeDegree, centeringRafeterBottomVerti2),
                                                                                                             +valueService.GetAdjacentByHypotenuse(roofSlopeDegree, centeringRafeterBottomVerti2));
                    Line rLine01 = new Line(GetSumPoint(centeringRafterLeftDown, 0, 0), GetSumPoint(centeringRafterLeftUP, 0, 0));
                    Line rLine02 = new Line(GetSumPoint(centeringRafterLeftUP, 0, 0), GetSumPoint(centeringRafterRightUP, 0, 0));
                    Line rLine03 = new Line(GetSumPoint(centeringRafterRightUP, 0, 0), GetSumPoint(centeringRafterRightRightUp, 0, 0));
                    Line rLine04 = new Line(GetSumPoint(centeringRafterRightRightUp, 0, 0), GetSumPoint(centeringRafterRightRightRightUp, 0, 0));
                    Line rLine05 = new Line(GetSumPoint(centeringRafterRightRightRightUp, 0, 0), GetSumPoint(centeringRafterRightRightRightDown, 0, 0));
                    Line rLine06 = new Line(GetSumPoint(centeringRafterRightRightRightDown, 0, 0), GetSumPoint(centeringRafterRightRightDown, 0, 0));
                    Line rLine07 = new Line(GetSumPoint(centeringRafterRightRightDown, 0, 0), GetSumPoint(centeringRafterRightDown, 0, 0));
                    Line rLine08 = new Line(GetSumPoint(centeringRafterRightDown, 0, 0), GetSumPoint(centeringRafterLeftDown, 0, 0));

                    customBlockList.AddRange(new Line[] { rLine01, rLine02, rLine03, rLine04, rLine05, rLine06, rLine07, rLine08 });



                    double tankIDHalf = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID) / 2;
                    Point3D leftRafterCenter = GetSumPoint(centeringRafterLeftUP,
                                                  valueService.GetOppositeByHypotenuse(roofSlopeDegree, cRafterA / 2), -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cRafterA / 2));
                    Line rafterCenter01temp = new Line(leftRafterCenter,
                                                  GetSumPoint(leftRafterCenter,
                                                  valueService.GetAdjacentByHypotenuse(roofSlopeDegree, tankIDHalf), +valueService.GetOppositeByHypotenuse(roofSlopeDegree, tankIDHalf)));
                    Point3D[] centerInter = rafterCenter01temp.IntersectWith(rLine05);
                    if (centerInter.Length > 0)
                    {
                        Line rafterCenter01 = new Line(leftRafterCenter, centerInter[0]);
                        styleService.SetLayer(ref rafterCenter01, layerService.LayerCenterLine);
                        customLine.Add(rafterCenter01);
                    }

                    // 숨은선
                    if (eachChannel != null)
                    {
                        double channelT2 = valueService.GetDoubleValue(eachChannel.t2);

                        // 위
                        Point3D leftUpPoint = GetSumPoint(centeringRafterLeftUP,
                                                      valueService.GetOppositeByHypotenuse(roofSlopeDegree, channelT2), -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, channelT2));
                        Line rafterHidden01temp = new Line(leftUpPoint,
                                                  GetSumPoint(leftUpPoint,
                                                  valueService.GetAdjacentByHypotenuse(roofSlopeDegree, tankIDHalf), +valueService.GetOppositeByHypotenuse(roofSlopeDegree, tankIDHalf)));
                        Point3D[] leftUpInter01 = rafterHidden01temp.IntersectWith(rLine03);
                        Point3D[] leftUpInter02 = rafterHidden01temp.IntersectWith(rLine04);
                        Point3D[] leftUpInter03 = rafterHidden01temp.IntersectWith(rLine05);
                        Point3D[] leftUpInter00 = leftUpInter01;
                        if (leftUpInter02.Length > 0)
                            leftUpInter00 = leftUpInter02;
                        if (leftUpInter03.Length > 0)
                            leftUpInter00 = leftUpInter03;
                        if (leftUpInter00.Length > 0)
                        {
                            Line rafterCenter01 = new Line(leftUpPoint, leftUpInter00[0]);
                            styleService.SetLayer(ref rafterCenter01, layerService.LayerHiddenLine);
                            customLine.Add(rafterCenter01);
                        }

                        // 아래
                        Point3D leftDonwPoint = GetSumPoint(centeringRafterLeftDown,
                                                      -valueService.GetOppositeByHypotenuse(roofSlopeDegree, channelT2), +valueService.GetAdjacentByHypotenuse(roofSlopeDegree, channelT2));
                        Line rafterHidden02temp = new Line(leftDonwPoint,
                                                  GetSumPoint(leftDonwPoint,
                                                  valueService.GetAdjacentByHypotenuse(roofSlopeDegree, tankIDHalf), +valueService.GetOppositeByHypotenuse(roofSlopeDegree, tankIDHalf)));
                        Point3D[] leftDownInter01 = rafterHidden02temp.IntersectWith(rLine07);
                        Point3D[] leftDownInter02 = rafterHidden02temp.IntersectWith(rLine06);
                        Point3D[] leftDownInter03 = rafterHidden02temp.IntersectWith(rLine05);
                        Point3D[] leftDownInter00 = leftDownInter01;
                        if (leftDownInter02.Length > 0)
                            leftDownInter00 = leftDownInter02;
                        if (leftDownInter03.Length > 0)
                            leftDownInter00 = leftDownInter03;
                        if (leftDownInter00.Length > 0)
                        {
                            Line rafterCenter01 = new Line(leftDonwPoint, leftDownInter00[0]);
                            styleService.SetLayer(ref rafterCenter01, layerService.LayerHiddenLine);
                            customLine.Add(rafterCenter01);
                        }


                    }




                    #endregion

                    // Leader
                    SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                    {
                        leaderPoint = GetSumCDPoint(rLine02.MidPoint, 0, -30),
                        lineTextList = new List<string>() {assemblyData.StructureCRTRafterInput[firstIndex].Size,
                                                            assemblyData.StructureCRTRafterInput[firstIndex].Qty  + "-" +valueService.GetOrdinalNumber(1) + " RAFTERS"
                                                             },
                        lineLength = 30,
                        Position = "topleft"
                    });
                }

            }


            if (!(StructureDivService.columnType == "centering" && StructureDivService.centeringInEx == "external"))
            {
                // Clip Shell Side : Colum type, Centering Type
                #region Support Clip Shell Side
                StructureClipShellSideModel eachSupportClip = new StructureClipShellSideModel();
                if (assemblyData.StructureCRTClipShellSideOutput.Count > 0)
                    eachSupportClip = assemblyData.StructureCRTClipShellSideOutput[firstIndex];

                double scA = valueService.GetDoubleValue(eachSupportClip.A);
                double scB = valueService.GetDoubleValue(eachSupportClip.B);
                double scC = valueService.GetDoubleValue(eachSupportClip.C);
                double scD = valueService.GetDoubleValue(eachSupportClip.D);
                double scE = valueService.GetDoubleValue(eachSupportClip.E);
                double scF = valueService.GetDoubleValue(eachSupportClip.F);
                double scG = valueService.GetDoubleValue(eachSupportClip.G);
                double scHoleQty = valueService.GetDoubleValue(eachSupportClip.HoleQty);

                double boltHoleHeight = valueService.GetDoubleValue(eachSupportClip.SlotholeHt);
                double boltHoleWidth = valueService.GetDoubleValue(eachSupportClip.SlotholeWd);

                StructureCRTColumnRafterModel lastRafter = new StructureCRTColumnRafterModel();
                if (assemblyData.StructureCRTColumnRafterOutput.Count - 1 >= 0)
                    lastRafter = assemblyData.StructureCRTColumnRafterOutput[assemblyData.StructureCRTColumnRafterOutput.Count - 1];

                double lrA = valueService.GetDoubleValue(lastRafter.A);
                double lrB = valueService.GetDoubleValue(lastRafter.B);
                double lrC = valueService.GetDoubleValue(lastRafter.C);
                double lrD = valueService.GetDoubleValue(lastRafter.D);
                double lrE = valueService.GetDoubleValue(lastRafter.E);
                double lrBoltHoleOnShell = valueService.GetDoubleValue(lastRafter.BoltHoleOnShell);
                double lrBoltHoleOnColumn = valueService.GetDoubleValue(lastRafter.BoltHoleOnColumn);
                double lrBoltHoleOnCenter = valueService.GetDoubleValue(lastRafter.BoltHoleOnCenter);
                double lrBoltHoleDia = valueService.GetDoubleValue(lastRafter.BoltHoleDia);

                if (StructureDivService.columnType == "centering")
                {
                    StructureCenteringRafterModel centeringLastRafter = new StructureCenteringRafterModel();
                    if (assemblyData.StructureCRTCenteringRaterOutput.Count > 0)
                        centeringLastRafter = assemblyData.StructureCRTCenteringRaterOutput[0];// 무조건 1개

                    double ceA = valueService.GetDoubleValue(centeringLastRafter.A);
                    double ceC = valueService.GetDoubleValue(centeringLastRafter.C);
                    double ceD = valueService.GetDoubleValue(centeringLastRafter.D);
                    lrA = ceA;
                    lrD = ceC;
                    lrE = ceD;
                }


                int refFirstIndex = 0;

                string selSizeTankHeight = assemblyData.GeneralDesignData[refFirstIndex].SizeTankHeight;
                Point3D leftTankTop = GetSumPoint(refPoint, 0, valueService.GetDoubleValue(selSizeTankHeight));
                int maxCourse = assemblyData.ShellOutput.Count - 1;

                // Rafter : Bolt
                double rafterSideBoltWidth = valueService.GetDoubleValue(eachSupportClip.F1);
                double rafterSideBoltGap = valueService.GetDoubleValue(eachSupportClip.G1);

                double shellClipTopGap = valueService.GetDoubleValue(eachSupportClip.B1);
                double shellClipPadWidth = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness);
                double shellClipPadInto = valueService.GetDoubleValue(eachSupportClip.C1);
                double shellClipTriTopGap = valueService.GetDoubleValue(eachSupportClip.D1);
                double shellClipTriBottomGap = valueService.GetDoubleValue(eachSupportClip.H1);
                double shellClipTriTopEndGap = valueService.GetDoubleValue(eachSupportClip.E1);

                Line shellClipPad1 = new Line(GetSumPoint(leftTankTop, 0, -shellClipTopGap), GetSumPoint(leftTankTop, 0, -shellClipTopGap - scF));
                Line shellClipPad2 = new Line(GetSumPoint(leftTankTop, shellClipPadWidth, -shellClipTopGap), GetSumPoint(leftTankTop, shellClipPadWidth, -shellClipTopGap - scF));
                Line shellClipPad3 = new Line(GetSumPoint(leftTankTop, 0, -shellClipTopGap), GetSumPoint(leftTankTop, shellClipPadWidth, -shellClipTopGap));
                Line shellClipPad4 = new Line(GetSumPoint(leftTankTop, 0, -shellClipTopGap - scF), GetSumPoint(leftTankTop, shellClipPadWidth, -shellClipTopGap - scF));
                customBlockList.AddRange(new Line[] { shellClipPad1, shellClipPad2, shellClipPad3, shellClipPad4 });

                //CDPoint leftTankRoofTop = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftRoofDown, ref refPoint, ref curPoint);


                double leftTankRoofTopGapWidth = valueService.GetDoubleValue(eachSupportClip.A1);
                // Gap : Slope
                double leftTankRoofTopGapY = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, leftTankRoofTopGapWidth);
                double leftTankRoofTopGapX = valueService.GetOppositeByHypotenuse(roofSlopeDegree, leftTankRoofTopGapWidth);
                // 수직 방향 이동 : 아래쪽
                CDPoint leftTankRoofTopOrigin = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftRoofDown, shellClipTriTopGap, ref refPoint, ref curPoint);
                CDPoint leftTankRoofTop1 = GetSumCDPoint(leftTankRoofTopOrigin, +leftTankRoofTopGapX, -leftTankRoofTopGapY);

                // 중간 : Slope 길이 -> Width로 변환
                double shellBoltWidth = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, rafterSideBoltWidth + rafterSideBoltGap);
                double shellBoltWidthVerticalWidthCal = 0;
                if (scHoleQty == 2)
                {
                    shellBoltWidthVerticalWidthCal = valueService.GetOppositeByHypotenuse(roofSlopeDegree, lrA / 2); ;
                }
                else
                {
                    shellBoltWidthVerticalWidthCal = valueService.GetOppositeByHypotenuse(roofSlopeDegree, lrA / 2); ;
                }

                // 평행 방향 이동 : 아래쪽
                CDPoint leftTankRoofTop2 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftRoofDown, shellClipTriTopGap + shellBoltWidth + shellBoltWidthVerticalWidthCal + shellClipTriTopEndGap, ref refPoint, ref curPoint);
                double leftTankRoofTopGapHeight = valueService.GetHypotenuseByWidth(roofSlopeDegree, leftTankRoofTopGapWidth);
                leftTankRoofTop2.Y = leftTankRoofTop2.Y - leftTankRoofTopGapHeight;

                // Compressionring
                double t1 = valueService.GetDoubleValue(assemblyData.RoofCRTInput[firstIndex].DetailIThickness);
                double thickVertiY = valueService.GetHypotenuseByWidth(roofSlopeDegree, t1);
                double thickneesY = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, t1);
                double thickneesX = valueService.GetOppositeByHypotenuse(roofSlopeDegree, t1);
                if (StructureDivService.originTopAngleType == "Detail i")
                {

                    scB = lrA - t1;
                    scD = scB / 2;
                    scE = scD - leftTankRoofTopGapWidth;

                    //기준 포인트
                    leftTankRoofTop1.Y = leftTankRoofTop1.Y - thickVertiY;
                    leftTankRoofTop2.Y = leftTankRoofTop2.Y - thickVertiY;

                    //Rafter
                    lrA = lrA - t1;
                }

                Line shellClipTri1 = new Line(GetSumPoint(leftTankTop, shellClipPadWidth, -shellClipTopGap - shellClipPadInto), GetSumPoint(leftTankRoofTop1, 0, 0));
                Line shellClipTri2 = new Line(GetSumPoint(leftTankRoofTop1, 0, 0), GetSumPoint(leftTankRoofTop2, 0, 0));
                Line shellClipTri3 = new Line(GetSumPoint(leftTankRoofTop2, 0, 0), GetSumPoint(leftTankRoofTop2, 0, -scC));
                Line shellClipTri4 = new Line(GetSumPoint(leftTankRoofTop2, 0, -scC), GetSumPoint(leftTankTop, shellClipPadWidth + shellClipTriBottomGap, -shellClipTopGap - scF + shellClipPadInto));
                Line shellClipTri5 = new Line(GetSumPoint(leftTankTop, shellClipPadWidth, -shellClipTopGap - scF + shellClipPadInto), GetSumPoint(leftTankTop, shellClipPadWidth + shellClipTriBottomGap, -shellClipTopGap - scF + shellClipPadInto));
                customBlockList.AddRange(new Line[] { shellClipTri1, shellClipTri2, shellClipTri3, shellClipTri4, shellClipTri5 });


                // Clip Shell Side : Bolt Hole
                if (scHoleQty == 2)
                {
                    // 직각 방향 이동: 아래쪽
                    double clipSideHoleFirstSlope1 = lrA / 2;

                    if (StructureDivService.originTopAngleType == "Detail i")
                    {
                        clipSideHoleFirstSlope1 = lrD + t1;
                    }

                    // ird/2 적용하기
                    double clipSideHoleFirstWidth1 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope1);//  clipSideHoleFirstSlope1 * Math.Cos(roofSlopeDegree);
                    double clipSideHoleFirstHeight1 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope1);// clipSideHoleFirstSlope1 * Math.Sin(roofSlopeDegree);
                    Point3D clipSideHoleFirstPoint1 = GetSumPoint(leftTankRoofTopOrigin, clipSideHoleFirstWidth1, -clipSideHoleFirstHeight1);
                    double clipSideHoleFirstSlope2 = rafterSideBoltWidth;
                    double clipSideHoleFirstWidth2 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope2); //clipSideHoleFirstSlope2 * Math.Cos(roofSlopeDegree);
                    double clipSideHoleFirstHeight2 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope2); //clipSideHoleFirstSlope2 * Math.Sin(roofSlopeDegree);
                    Point3D clipSideHoleFirstPoint2 = GetSumPoint(clipSideHoleFirstPoint1, clipSideHoleFirstWidth2, +clipSideHoleFirstHeight2);
                    double clipSideHoleFirstSlope3 = rafterSideBoltGap;
                    double clipSideHoleFirstWidth3 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope3); //clipSideHoleFirstSlope3 * Math.Cos(roofSlopeDegree);
                    double clipSideHoleFirstHeight3 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleFirstSlope3); //clipSideHoleFirstSlope3 * Math.Sin(roofSlopeDegree);
                    Point3D clipSideHoleFirstPoint3 = GetSumPoint(clipSideHoleFirstPoint2, clipSideHoleFirstWidth3, +clipSideHoleFirstHeight3);

                    CompositeCurve clipSideBoltFirst1 = GetBoltHoleHorizontal(clipSideHoleFirstPoint2, boltHoleWidth, boltHoleHeight);
                    clipSideBoltFirst1.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleFirstPoint2);
                    CompositeCurve clipSideBoltFirst2 = GetBoltHoleHorizontal(clipSideHoleFirstPoint3, boltHoleWidth, boltHoleHeight);
                    clipSideBoltFirst2.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleFirstPoint3);
                    customBlockList.AddRange(new CompositeCurve[] { clipSideBoltFirst1, clipSideBoltFirst2 });

                    //Circle ClipSideBoltFirstCicle1 = new Circle(clipSideHoleFirstPoint2, boltHoleHeight / 2);
                    //Circle ClipSideBoltFirstCicle2 = new Circle(clipSideHoleFirstPoint3, boltHoleHeight / 2);
                    //customBlockList.AddRange(new Circle[] { ClipSideBoltFirstCicle1, ClipSideBoltFirstCicle2 });

                    List<Entity> circle01 = DrawReference_Hole(clipSideHoleFirstPoint2, boltHoleHeight / 2, roofSlopeDegree, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                    List<Entity> circle02 = DrawReference_Hole(clipSideHoleFirstPoint3, boltHoleHeight / 2, roofSlopeDegree, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                    customLine.AddRange(circle01);
                    customLine.AddRange(circle02);
                }
                else if (scHoleQty == 4)
                {
                    // 직각 방향 이동: 아래쪽
                    double clipSideHoleSecondSlope1 = lrD;
                    if (StructureDivService.originTopAngleType == "Detail i")
                    {
                        clipSideHoleSecondSlope1 = lrD + t1;
                    }
                    double clipSideHoleSecondWidth1 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleSecondSlope1);// clipSideHoleSecondSlope1 * Math.Cos(roofSlopeDegree);
                    double clipSideHoleSecondHeight1 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleSecondSlope1);// clipSideHoleSecondSlope1 * Math.Sin(roofSlopeDegree);
                    Point3D clipSideHoleSecondPoint1 = GetSumPoint(leftTankRoofTopOrigin, clipSideHoleSecondWidth1, -clipSideHoleSecondHeight1);
                    double clipSideHoleSecondlope2 = rafterSideBoltWidth;
                    double clipSideHoleSecondWidth2 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleSecondlope2); //clipSideHoleSecondlope2* Math.Cos(roofSlopeDegree);
                    double clipSideHoleSecondHeight2 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleSecondlope2); //clipSideHoleSecondlope2* Math.Sin(roofSlopeDegree);
                    Point3D clipSideHoleSecondPoint2 = GetSumPoint(clipSideHoleSecondPoint1, clipSideHoleSecondWidth2, +clipSideHoleSecondHeight2);
                    double clipSideHoleSecondSlope3 = rafterSideBoltGap;
                    double clipSideHoleSecondWidth3 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleSecondSlope3); //clipSideHoleSecondSlope3 * Math.Cos(roofSlopeDegree);
                    double clipSideHoleSecondHeight3 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleSecondSlope3); //clipSideHoleSecondSlope3 * Math.Sin(roofSlopeDegree);
                    Point3D clipSideHoleSecondPoint3 = GetSumPoint(clipSideHoleSecondPoint2, clipSideHoleSecondWidth3, +clipSideHoleSecondHeight3);

                    CompositeCurve clipSideBoltSecond1 = GetBoltHoleHorizontal(clipSideHoleSecondPoint2, boltHoleWidth, boltHoleHeight);
                    clipSideBoltSecond1.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleSecondPoint2);
                    CompositeCurve clipSideBoltSecond2 = GetBoltHoleHorizontal(clipSideHoleSecondPoint3, boltHoleWidth, boltHoleHeight);
                    clipSideBoltSecond2.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleSecondPoint3);
                    customBlockList.AddRange(new CompositeCurve[] { clipSideBoltSecond1, clipSideBoltSecond2 });

                    // 직각 방향 이동: 아래쪽
                    double clipSideHoleSecondSlope4 = clipSideHoleSecondSlope1 + lrE;
                    double clipSideHoleSecondWidth4 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleSecondSlope4); //clipSideHoleSecondSlope4 * Math.Cos(roofSlopeDegree);
                    double clipSideHoleSecondHeight4 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleSecondSlope4);// clipSideHoleSecondSlope4 * Math.Sin(roofSlopeDegree);
                    Point3D clipSideHoleSecondPoint4 = GetSumPoint(leftTankRoofTopOrigin, clipSideHoleSecondWidth4, -clipSideHoleSecondHeight4);
                    double clipSideHoleSecondlope5 = rafterSideBoltWidth;
                    double clipSideHoleSecondWidth5 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleSecondlope5); //clipSideHoleSecondlope5 * Math.Cos(roofSlopeDegree);
                    double clipSideHoleSecondHeight5 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleSecondlope5); //clipSideHoleSecondlope5 * Math.Sin(roofSlopeDegree);
                    Point3D clipSideHoleSecondPoint5 = GetSumPoint(clipSideHoleSecondPoint4, clipSideHoleSecondWidth5, +clipSideHoleSecondHeight5);
                    double clipSideHoleSecondSlope6 = rafterSideBoltGap;
                    double clipSideHoleSecondWidth6 = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, clipSideHoleSecondSlope6); //clipSideHoleSecondSlope6 * Math.Cos(roofSlopeDegree);
                    double clipSideHoleSecondHeight6 = valueService.GetOppositeByHypotenuse(roofSlopeDegree, clipSideHoleSecondSlope6); //clipSideHoleSecondSlope6 * Math.Sin(roofSlopeDegree);
                    Point3D clipSideHoleSecondPoint6 = GetSumPoint(clipSideHoleSecondPoint5, clipSideHoleSecondWidth6, +clipSideHoleSecondHeight6);

                    CompositeCurve clipSideBoltSecond3 = GetBoltHoleHorizontal(clipSideHoleSecondPoint5, boltHoleWidth, boltHoleHeight);
                    clipSideBoltSecond3.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleSecondPoint5);
                    CompositeCurve clipSideBoltSecond4 = GetBoltHoleHorizontal(clipSideHoleSecondPoint6, boltHoleWidth, boltHoleHeight);
                    clipSideBoltSecond4.Rotate(roofSlopeDegree, Vector3D.AxisZ, clipSideHoleSecondPoint6);
                    customBlockList.AddRange(new CompositeCurve[] { clipSideBoltSecond3, clipSideBoltSecond4 });


                    //Circle ClipSideBoltSecondCicle1 = new Circle(clipSideHoleSecondPoint2, boltHoleHeight / 2);
                    //Circle ClipSideBoltSecondCicle2 = new Circle(clipSideHoleSecondPoint3, boltHoleHeight / 2);
                    //Circle ClipSideBoltSecondCicle3 = new Circle(clipSideHoleSecondPoint5, boltHoleHeight / 2);
                    //Circle ClipSideBoltsecondCicle4 = new Circle(clipSideHoleSecondPoint6, boltHoleHeight / 2);
                    //customBlockList.AddRange(new Circle[] { ClipSideBoltSecondCicle1, ClipSideBoltSecondCicle2, ClipSideBoltSecondCicle3, ClipSideBoltsecondCicle4 });


                    List<Entity> circle01 = DrawReference_Hole(clipSideHoleSecondPoint2, boltHoleHeight / 2, roofSlopeDegree, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                    List<Entity> circle02 = DrawReference_Hole(clipSideHoleSecondPoint3, boltHoleHeight / 2, roofSlopeDegree, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                    customLine.AddRange(circle01);
                    customLine.AddRange(circle02);
                    List<Entity> circle011 = DrawReference_Hole(clipSideHoleSecondPoint5, boltHoleHeight / 2, roofSlopeDegree, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                    List<Entity> circle021 = DrawReference_Hole(clipSideHoleSecondPoint6, boltHoleHeight / 2, roofSlopeDegree, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                    customLine.AddRange(circle011);
                    customLine.AddRange(circle021);
                }

                #endregion


                if (StructureDivService.columnType == "column")
                {
                    #region Rafter

                    CDPoint rafterEndPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftRoofDown, shellClipTriTopGap, ref refPoint, ref curPoint);

                    // 수직방향 이동 : 오른쪽으로
                    CDPoint rafterStartPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, centerRadius + centerLeftWidthHalf, ref refPoint, ref curPoint);
                    double rafterStartPointtSlope = rafterSideBoltGap / 2 + rafterSideBoltWidth;
                    double rafterStartPointtWidth = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, rafterStartPointtSlope);
                    double rafterStartPointtHeight = valueService.GetOppositeByHypotenuse(roofSlopeDegree, rafterStartPointtSlope);

                    Point3D rafterStartPoint2 = GetSumPoint(rafterStartPoint, rafterStartPointtWidth, rafterStartPointtHeight);


                    Point3D rafterTempColumnPoint = new Point3D();
                    for (int i = 0; i < assemblyData.StructureCRTColumnRafterOutput.Count; i++)
                    {
                        Point3D rafterStartColumnPoint = new Point3D();
                        if (i == 0)
                        {
                            double rafterWidthHalfHalf = valueService.GetDoubleValue(assemblyData.StructureCRTColumnRafterOutput[i].A) / 2;
                            double rafterO = valueService.GetOppositeByWidth(roofSlopeDegree, rafterWidthHalfHalf);
                            double rafterXXX = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, rafterO);
                            double rafterYYY = valueService.GetOppositeByHypotenuse(roofSlopeDegree, rafterO);
                            rafterStartColumnPoint.X = rafterStartPoint2.X - rafterXXX;
                            rafterStartColumnPoint.Y = rafterStartPoint2.Y - rafterYYY;
                        }
                        else
                        {
                            rafterStartColumnPoint.X = rafterTempColumnPoint.X;
                            rafterStartColumnPoint.Y = rafterTempColumnPoint.Y;
                        }

                        double rafterEachRadius = valueService.GetDoubleValue(assemblyData.StructureCRTRafterInput[i].Radius);
                        CDPoint rafterCurrentColumnPointTemp = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, rafterEachRadius, ref refPoint, ref curPoint);
                        Point3D rafterCurrentColumnPoint = GetSumPoint(rafterCurrentColumnPointTemp, 0, 0);

                        if (i == assemblyData.StructureCRTColumnRafterOutput.Count - 1)
                        {
                            //rafterStartColumnPoint.X = rafterCurrentColumnPoint.X;
                            //rafterStartColumnPoint.Y = rafterCurrentColumnPoint.Y;
                            rafterCurrentColumnPoint.X = rafterEndPoint.X;
                            rafterCurrentColumnPoint.Y = rafterEndPoint.Y;

                        }

                        // Draw Structure

                        // 직각 방향 이동 : 아래로
                        double rafterWidth = valueService.GetDoubleValue(assemblyData.StructureCRTColumnRafterOutput[i].A);
                        double rafterStartSlope = rafterWidth;
                        double rafterStartWidth = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, rafterStartSlope);
                        double rafterStartHeight = valueService.GetOppositeByHypotenuse(roofSlopeDegree, rafterStartSlope);
                        Point3D rafterStartColumnPoint2 = GetSumPoint(rafterStartColumnPoint, rafterStartHeight, -rafterStartWidth);

                        double rafterCurrentSlope = rafterWidth;
                        double rafterCurrentWidth = rafterStartWidth;
                        double rafterCurrentHeight = rafterStartHeight;
                        Point3D rafterCurrentColumnPoint2 = GetSumPoint(rafterCurrentColumnPoint, rafterCurrentHeight, -rafterCurrentWidth);



                        Line rafterSquare1 = new Line(rafterStartColumnPoint, rafterCurrentColumnPoint);
                        Line rafterSquare2 = new Line(rafterStartColumnPoint, rafterStartColumnPoint2);
                        Line rafterSquare3 = new Line(rafterCurrentColumnPoint, rafterCurrentColumnPoint2);
                        Line rafterSquare4 = new Line(rafterStartColumnPoint2, rafterCurrentColumnPoint2);
                        if (StructureDivService.originTopAngleType == "Detail i")
                        {
                            if (i == assemblyData.StructureCRTColumnRafterOutput.Count - 1)
                            {
                                double A = valueService.GetDoubleValue(assemblyData.RoofCRTInput[firstIndex].DetailIOutsideProjection);
                                double B = valueService.GetDoubleValue(assemblyData.RoofCRTInput[firstIndex].DetailIWidth);
                                double compressWidth = B - A - valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness) + 50;// 50이 정해 짐
                                double comPressAdjacent = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, compressWidth);

                                CDPoint leftCompressPointCD = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftRoofDown, comPressAdjacent, ref refPoint, ref curPoint);
                                Point3D leftCompressPoint = new Point3D(leftCompressPointCD.X, leftCompressPointCD.Y);
                                Point3D leftCompressPoint2 = GetSumPoint(leftCompressPoint, +thickneesX, -thickneesY);

                                //rafterCurrentColumnPoint.Y = rafterCurrentColumnPoint.Y - thickVertiY;

                                rafterSquare1 = new Line(rafterStartColumnPoint, leftCompressPoint);
                                Line rafterCompre1 = new Line(leftCompressPoint, leftCompressPoint2);
                                Line rafterCompre2 = new Line(GetSumPoint(rafterCurrentColumnPoint, 0, -thickneesY), leftCompressPoint2);
                                rafterSquare2 = new Line(rafterStartColumnPoint, rafterStartColumnPoint2);
                                rafterSquare3 = new Line(GetSumPoint(rafterCurrentColumnPoint, 0, -thickneesY), rafterCurrentColumnPoint2);
                                rafterSquare4 = new Line(rafterStartColumnPoint2, rafterCurrentColumnPoint2);

                                customBlockList.Add(rafterCompre1);
                                customBlockList.Add(rafterCompre2);
                            }


                        }

                        customBlockList.AddRange(new Line[] { rafterSquare1, rafterSquare2, rafterSquare3, rafterSquare4 });


                        // Leader
                        SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                        {
                            leaderPoint = GetSumCDPoint(rafterSquare1.MidPoint, 0, -30),
                            lineTextList = new List<string>() {assemblyData.StructureCRTRafterInput[i].Size,
                                                            assemblyData.StructureCRTRafterInput[i].Qty + "-" + valueService.GetOrdinalNumber(i+1) + " RAFTERS"
                                                             },
                            lineLength = 30,
                            Position = "topleft"
                        });



                        // Center Line : 중심선
                        double rafterWidthHalf = valueService.GetDoubleValue(assemblyData.StructureCRTColumnRafterOutput[i].A) / 2;
                        double rafterStartSlope2 = rafterWidthHalf;
                        double rafterStartWidth2 = rafterStartWidth / 2;
                        double rafterStartHeight2 = rafterStartHeight / 2;
                        Point3D rafterStartColumnPoint3 = GetSumPoint(rafterStartColumnPoint, rafterStartHeight2, -rafterStartWidth2);

                        double rafterCurrentSlope2 = rafterWidthHalf;
                        double rafterCurrentWidth2 = rafterStartWidth / 2;
                        double rafterCurrentHeight2 = rafterStartHeight / 2;
                        Point3D rafterCurrentColumnPoint4 = GetSumPoint(rafterCurrentColumnPoint, rafterCurrentHeight2, -rafterCurrentWidth2);

                        Line rafterSquareCenter1 = new Line(rafterStartColumnPoint3, rafterCurrentColumnPoint4);
                        styleService.SetLayer(ref rafterSquareCenter1, layerService.LayerCenterLine);
                        customLine.Add(rafterSquareCenter1);

                        // 숨은선
                        ChannelModel eachChannel = GetChannel(assemblyData.StructureCRTColumnRafterOutput[i].SIZE);
                        if (eachChannel != null)
                        {
                            double channelT2 = valueService.GetDoubleValue(eachChannel.t2);
                            Line rafterHidden01 = new Line(GetSumPoint(rafterStartColumnPoint,
                                                          valueService.GetOppositeByHypotenuse(roofSlopeDegree, channelT2), -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, channelT2)),
                                                          GetSumPoint(rafterCurrentColumnPoint,
                                                          valueService.GetOppositeByHypotenuse(roofSlopeDegree, channelT2), -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, channelT2)));
                            Line rafterHidden02 = new Line(GetSumPoint(rafterStartColumnPoint2,
                                                          -valueService.GetOppositeByHypotenuse(roofSlopeDegree, channelT2), +valueService.GetAdjacentByHypotenuse(roofSlopeDegree, channelT2)),
                                                          GetSumPoint(rafterCurrentColumnPoint2,
                                                          -valueService.GetOppositeByHypotenuse(roofSlopeDegree, channelT2), +valueService.GetAdjacentByHypotenuse(roofSlopeDegree, channelT2)));
                            styleService.SetLayer(ref rafterHidden01, layerService.LayerHiddenLine);
                            styleService.SetLayer(ref rafterHidden02, layerService.LayerHiddenLine);
                            customLine.Add(rafterHidden01);
                            customLine.Add(rafterHidden02);
                        }




                        // Current -> Start
                        rafterTempColumnPoint.X = rafterCurrentColumnPoint.X;
                        rafterTempColumnPoint.Y = rafterCurrentColumnPoint.Y;
                    }

                    #endregion
                }
                else if (StructureDivService.columnType == "centering")
                {
                    if (StructureDivService.centeringInEx == "internal")
                    {
                        #region Rafter : Centering
                        StructureCenteringRafterModel centeringRafter = new StructureCenteringRafterModel();
                        ChannelModel eachChannel = new ChannelModel();
                        if (assemblyData.StructureCRTCenteringRaterOutput.Count > 0)
                        {
                            centeringRafter = assemblyData.StructureCRTCenteringRaterOutput[firstIndex];
                            eachChannel = GetChannel(assemblyData.StructureCRTCenteringRaterOutput[firstIndex].SIZE);
                        }

                        double cRafterA = valueService.GetDoubleValue(centeringRafter.A);
                        double cRafterB = valueService.GetDoubleValue(centeringRafter.B);
                        double cRafterC = valueService.GetDoubleValue(centeringRafter.C);
                        double cRafterD = valueService.GetDoubleValue(centeringRafter.D);
                        double cRafterHoleQty = valueService.GetDoubleValue(centeringRafter.HoleQty);
                        double cRafterA1 = valueService.GetDoubleValue(centeringRafter.A1);
                        double cRafterB1 = valueService.GetDoubleValue(centeringRafter.B1);
                        double cRafterC1 = valueService.GetDoubleValue(centeringRafter.C1);
                        double cRafterD1 = valueService.GetDoubleValue(centeringRafter.D1);
                        double cRafterE = valueService.GetDoubleValue(centeringRafter.E);



                        CDPoint leftCenteringRafterEndRef = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftRoofDown, shellClipTriTopGap, ref refPoint, ref curPoint);
                        Point3D leftCenteringRafterEnd01 = GetSumPoint(leftCenteringRafterEndRef, 0, 0);
                        Point3D leftCenteringRafterEnd02 = GetSumPoint(leftCenteringRafterEndRef, +valueService.GetOppositeByHypotenuse(roofSlopeDegree, cRafterB),
                                                                                                  -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cRafterB));
                        Point3D leftCenteringRafterEndMiddle = GetSumPoint(leftCenteringRafterEndRef, +valueService.GetOppositeByHypotenuse(roofSlopeDegree, cRafterB / 2),
                                                                                                      -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cRafterB / 2));

                        CDPoint rightCenterigRafterStartRef = (CDPoint)centeringClipRight.Clone();
                        Point3D rightCenteringRafterStart01 = GetSumPoint(rightCenterigRafterStartRef, 0, 0);
                        Point3D rightCenteringRafterStart02 = GetSumPoint(rightCenterigRafterStartRef, +valueService.GetOppositeByHypotenuse(roofSlopeDegree, cRafterB),
                                                                                                       -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cRafterB));
                        Point3D rightCenteringRafterStartMiddle = GetSumPoint(rightCenterigRafterStartRef, +valueService.GetOppositeByHypotenuse(roofSlopeDegree, cRafterB / 2),
                                                                                                           -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, cRafterB / 2));


                        Line centeringRafterSquare1 = new Line(leftCenteringRafterEnd01, leftCenteringRafterEnd02);
                        Line centeringRafterSquare2 = new Line(rightCenteringRafterStart01, rightCenteringRafterStart02);
                        Line centeringRafterSquare3 = new Line(leftCenteringRafterEnd01, rightCenteringRafterStart01);
                        Line centeringRafterSquare4 = new Line(leftCenteringRafterEnd02, rightCenteringRafterStart02);

                        customBlockList.AddRange(new Line[] { centeringRafterSquare1, centeringRafterSquare2, centeringRafterSquare3, centeringRafterSquare4 });

                        // 중심선
                        Line centeringRafterMiddle = new Line(leftCenteringRafterEndMiddle, rightCenteringRafterStartMiddle);
                        styleService.SetLayer(ref centeringRafterMiddle, layerService.LayerCenterLine);
                        customLine.Add(centeringRafterMiddle);

                        // 숨은선
                        if (eachChannel != null)
                        {
                            double channelT2 = valueService.GetDoubleValue(eachChannel.t2);
                            Line rafterHidden01 = new Line(GetSumPoint(leftCenteringRafterEnd01,
                                                          valueService.GetOppositeByHypotenuse(roofSlopeDegree, channelT2), -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, channelT2)),
                                                          GetSumPoint(rightCenteringRafterStart01,
                                                          valueService.GetOppositeByHypotenuse(roofSlopeDegree, channelT2), -valueService.GetAdjacentByHypotenuse(roofSlopeDegree, channelT2)));
                            Line rafterHidden02 = new Line(GetSumPoint(leftCenteringRafterEnd02,
                                                          -valueService.GetOppositeByHypotenuse(roofSlopeDegree, channelT2), +valueService.GetAdjacentByHypotenuse(roofSlopeDegree, channelT2)),
                                                          GetSumPoint(rightCenteringRafterStart02,
                                                          -valueService.GetOppositeByHypotenuse(roofSlopeDegree, channelT2), +valueService.GetAdjacentByHypotenuse(roofSlopeDegree, channelT2)));
                            styleService.SetLayer(ref rafterHidden01, layerService.LayerHiddenLine);
                            styleService.SetLayer(ref rafterHidden02, layerService.LayerHiddenLine);
                            customLine.Add(rafterHidden01);
                            customLine.Add(rafterHidden02);
                        }



                        #endregion

                        // Leader
                        SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                        {
                            leaderPoint = GetSumCDPoint(centeringRafterSquare3.MidPoint, 0, -30),
                            lineTextList = new List<string>() {assemblyData.StructureCRTRafterInput[firstIndex].Size,
                                                            assemblyData.StructureCRTRafterInput[firstIndex].Qty + "-" + valueService.GetOrdinalNumber(1) + " RAFTERS"
                                                             },
                            lineLength = 30,
                            Position = "topleft"
                        });

                    }
                }
            }

            styleService.SetLayerListEntity(ref customBlockList, layerService.LayerOutLine);

            customBlockList.AddRange(customLine);

            // Centering : External : Right : Mirror
            if (StructureDivService.columnType == "centering" && StructureDivService.centeringInEx == "external")
            {

                CDPoint mirrorPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterTop, 0, ref refPoint, ref curPoint);
                Plane pl1 = Plane.YZ;
                pl1.Origin.X = mirrorPoint.X;
                pl1.Origin.Y = mirrorPoint.Y;
                Mirror customMirror = new Mirror(pl1);
                List<Entity> newMirrorList = new List<Entity>();
                foreach (Entity eachEntity in customBlockList)
                {
                    Entity newEntity = (Entity)eachEntity.Clone();
                    newEntity.TransformBy(customMirror);
                    newMirrorList.Add(newEntity);
                }
                customBlockList.AddRange(newMirrorList);
            }

            return customBlockList.ToArray();
        }


        public Entity[] DrawBlock_Structure_DRT(CDPoint selPoint1, double scaleValue)
        {
            // Sturcutre Type
            // Type
            DrawStructureService StructureDivService = new DrawStructureService();
            StructureDivService.SetStructureData(SingletonData.TankType, assemblyData.StructureDRTInput[0].SupportingType, assemblyData.RoofCompressionRing[0].CompressionRingType);


            int firstIndex = 0;
            Point3D drawPoint = new Point3D(selPoint1.X, selPoint1.Y, selPoint1.Z);
            CDPoint refPoint = new CDPoint() { X = drawPoint.X, Y = drawPoint.Y };
            CDPoint curPoint = (CDPoint)refPoint.Clone();

            List<Entity> customBlockList = new List<Entity>();

            List<Entity> customLine = new List<Entity>();



            // Centering
            CDPoint centeringClipRight = new CDPoint();

            double roofSlopeDegree = workingPointService.DRTWorkingData.CompressionRingDegree;
            int maxCourse = assemblyData.ShellOutput.Count - 1;
            string selSizeTankHeight = assemblyData.GeneralDesignData[firstIndex].SizeTankHeight;
            Point3D leftTankTop = GetSumPoint(refPoint, 0, valueService.GetDoubleValue(selSizeTankHeight));
            double shellMaxThickness = 0;
            if (assemblyData.ShellOutput.Count > 0)
                shellMaxThickness = valueService.GetDoubleValue(assemblyData.ShellOutput[assemblyData.ShellOutput.Count - 1].Thickness);

            double centerRadius = 0; // Center


            // Rafter Check
            if (assemblyData.StructureDRTRafterInput.Count > 0)
            {

            


                // DRT : Only Centering
                if (StructureDivService.columnType == "centering")
                {
                    if (StructureDivService.centeringInEx == "internal")
                    {
                        // Shell Side Pad
                        #region Support Clip Shell Side
                        StructureClipShellSideModel eachSupportClip = new StructureClipShellSideModel();
                        if (assemblyData.StructureDRTClipShellSideOutput.Count > 0)
                            eachSupportClip = assemblyData.StructureDRTClipShellSideOutput[firstIndex];

                        double scA = valueService.GetDoubleValue(eachSupportClip.A);
                        double scB = valueService.GetDoubleValue(eachSupportClip.B);
                        double scC = valueService.GetDoubleValue(eachSupportClip.C);
                        double scD = valueService.GetDoubleValue(eachSupportClip.D);
                        double scE = valueService.GetDoubleValue(eachSupportClip.E);
                        double scF = valueService.GetDoubleValue(eachSupportClip.F);
                        double scG = valueService.GetDoubleValue(eachSupportClip.G);
                        double scHoleQty = valueService.GetDoubleValue(eachSupportClip.HoleQty);

                        double boltHoleHeight = valueService.GetDoubleValue(eachSupportClip.SlotholeHt);
                        double boltHoleWidth = valueService.GetDoubleValue(eachSupportClip.SlotholeWd);


                        double scG1 = valueService.GetDoubleValue(eachSupportClip.G1);
                        double scA1 = valueService.GetDoubleValue(eachSupportClip.A1);

                        double scB1 = valueService.GetDoubleValue(eachSupportClip.B1);
                        double shellClipPadWidth = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness);
                        double scC1 = valueService.GetDoubleValue(eachSupportClip.C1);
                        double scD1 = valueService.GetDoubleValue(eachSupportClip.D1);
                        double scH1 = valueService.GetDoubleValue(eachSupportClip.H1);
                        double scE1 = valueService.GetDoubleValue(eachSupportClip.E1);
                        double scF1 = valueService.GetDoubleValue(eachSupportClip.F1);




                        // Sheel Pad
                        Line shellClipPad1 = new Line(GetSumPoint(leftTankTop, 0, -scB1), GetSumPoint(leftTankTop, 0, -scB1 - scF));
                        Line shellClipPad2 = new Line(GetSumPoint(leftTankTop, shellClipPadWidth, -scB1), GetSumPoint(leftTankTop, shellClipPadWidth, -scB1 - scF));
                        Line shellClipPad3 = new Line(GetSumPoint(leftTankTop, 0, -scB1), GetSumPoint(leftTankTop, shellClipPadWidth, -scB1));
                        Line shellClipPad4 = new Line(GetSumPoint(leftTankTop, 0, -scB1 - scF), GetSumPoint(leftTankTop, shellClipPadWidth, -scB1 - scF));
                        customBlockList.AddRange(new Line[] { shellClipPad1, shellClipPad2, shellClipPad3, shellClipPad4 });






                        // 수직 방향 이동 : 아래쪽
                        CDPoint leftTankRoofTopOrigin = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftRoofDown, scD1, ref refPoint, ref curPoint);

                        Vector3D leftTankRoofTopOriginDirection = new Vector3D(GetSumPoint(leftTankRoofTopOrigin, 0, 0), workingPointService.DRTWorkingData.RoofCenterPoint);
                        leftTankRoofTopOriginDirection.Normalize();

                        Vector3D rafterDirection = new Vector3D(GetSumPoint(leftTankRoofTopOrigin, 0, 0), workingPointService.DRTWorkingData.RoofCenterPoint);
                        rafterDirection.Normalize();

                        Vector3D rafterDirectionUp = new Vector3D(workingPointService.DRTWorkingData.RoofCenterPoint, GetSumPoint(leftTankRoofTopOrigin, 0, 0));
                        rafterDirectionUp.Normalize();

                        // Compression Ring
                        double compressionRingThicknees = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].ThicknessT1);
                        Point3D LeftCompressionPoint = new Point3D();
                        Line newLineCompressionC = null;
                        if (assemblyData.RoofCompressionRing[0].CompressionRingType == "Detail i")
                        {
                            scB = scA - scA1 - compressionRingThicknees;

                            double comDegree = workingPointService.DRTWorkingData.CompressionRingDegree;

                            double leftTankRoofPointHeight = valueService.GetOppositeByWidth(comDegree, shellMaxThickness + scD1);
                            CDPoint leftTankRoofTopOriginCompression = GetSumCDPoint(leftTankTop, scD1, leftTankRoofPointHeight);

                            // Direction
                            Line newDirectionLine = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, 100, 0));
                            newDirectionLine.Rotate(comDegree, Vector3D.AxisZ, GetSumPoint(refPoint, 0, 0));
                            newDirectionLine.Rotate(Utility.DegToRad(-90), Vector3D.AxisZ, GetSumPoint(refPoint, 0, 0));
                            Vector3D compressionDirection = new Vector3D(newDirectionLine.StartPoint, newDirectionLine.EndPoint);
                            compressionDirection.Normalize();
                            // 적용



                            leftTankRoofTopOrigin = GetSumCDPoint(leftTankRoofTopOriginCompression, 0, 0);
                            leftTankRoofTopOriginDirection = compressionDirection;



                            // Compressionring : i type
                            double A = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[firstIndex].OutsideProjectionA);
                            double B = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[firstIndex].WidthB);
                            double C = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[firstIndex].OverlapOfRoofAndCompRingC);
                            double t1 = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[firstIndex].ThicknessT1);

                            // left Compression Point
                            double comLength = B - A;// 임시로 고정, 50 값 수정 해야 함
                            Point3D newLineCompressionA = GetSumPoint(leftTankTop,
                                                            -shellMaxThickness + valueService.GetAdjacentByHypotenuse(comDegree, comLength),
                                                            valueService.GetOppositeByHypotenuse(comDegree, comLength));
                            comLength += 50;// 임시로 고정, 50 값 수정 해야 함
                            Point3D newLineCompressionB = GetSumPoint(leftTankTop,
                                                            -shellMaxThickness + valueService.GetAdjacentByHypotenuse(comDegree, comLength),
                                                            valueService.GetOppositeByHypotenuse(comDegree, comLength));

                            Line lineA = new Line(GetSumPoint(newLineCompressionA, 0, 0), GetSumPoint(newLineCompressionB, 0, 0));

                            newLineCompressionC = new Line(GetSumPoint(newLineCompressionB, 0, 0), GetSumPoint(newLineCompressionB, 0, 0) + (rafterDirectionUp * t1 * 100));
                            Point3D leftTopComressPoint = editingService.GetIntersectWidth(newLineCompressionC, workingPointService.DRTWorkingData.circleRoofLower, 0);

                            Line lineB = new Line(GetSumPoint(leftTopComressPoint, 0, 0), GetSumPoint(newLineCompressionB, 0, 0));

                            customBlockList.Add(lineA);
                            customBlockList.Add(lineB);
                        }



                        Point3D leftTankRoofTop1 = GetSumPoint(leftTankRoofTopOrigin, 0, 0) + (leftTankRoofTopOriginDirection * scA1);

                        Line topPadLine = new Line(GetSumPoint(leftTankRoofTop1, 0, 0), GetSumPoint(leftTankRoofTop1, 0, 0));
                        topPadLine.EndPoint = topPadLine.EndPoint + (leftTankRoofTopOriginDirection * scG1);
                        topPadLine.Rotate(Utility.DegToRad(90), Vector3D.AxisZ, GetSumPoint(leftTankRoofTop1, 0, 0));
                        Point3D leftTankRoofTop2 = GetSumPoint(topPadLine.EndPoint, 0, 0);
                        Line topPadLine2 = new Line(GetSumPoint(leftTankRoofTop2, 0, 0), leftTankRoofTop2 + (leftTankRoofTopOriginDirection * scB));
                        Point3D leftTankRoofTop3 = GetSumPoint(topPadLine2.EndPoint, 0, 0);


                        Line shellClipTri1 = new Line(GetSumPoint(leftTankTop, shellClipPadWidth, -scB1 - scC1), GetSumPoint(leftTankRoofTop1, 0, 0));
                        Line shellClipTri4 = new Line(GetSumPoint(leftTankRoofTop3, 0, 0), GetSumPoint(leftTankTop, shellClipPadWidth + scH1, -scB1 - scF + scC1));
                        Line shellClipTri5 = new Line(GetSumPoint(leftTankTop, shellClipPadWidth, -scB1 - scF + scC1), GetSumPoint(leftTankTop, shellClipPadWidth + scH1, -scB1 - scF + scC1));
                        customBlockList.AddRange(new Line[] { shellClipTri1, topPadLine, topPadLine2, shellClipTri4, shellClipTri5 });



                        // Hole
                        Point3D firstHolePoint = GetSumPoint(leftTankRoofTop1, 0, 0) + (leftTankRoofTopOriginDirection * scC);
                        Point3D secondHolePoint = GetSumPoint(leftTankRoofTop1, 0, 0) + (leftTankRoofTopOriginDirection * scD);


                        Line holeLine01 = new Line(GetSumPoint(firstHolePoint, 0, 0), GetSumPoint(firstHolePoint, 0, 0) + (leftTankRoofTopOriginDirection * scE1));
                        holeLine01.Rotate(Utility.DegToRad(90), Vector3D.AxisZ, GetSumPoint(firstHolePoint, 0, 0));
                        Line holeLine02 = new Line(GetSumPoint(firstHolePoint, 0, 0), GetSumPoint(firstHolePoint, 0, 0) + (leftTankRoofTopOriginDirection * (scE1 + scF1)));
                        holeLine02.Rotate(Utility.DegToRad(90), Vector3D.AxisZ, GetSumPoint(firstHolePoint, 0, 0));
                        Point3D holePoint01 = GetSumPoint(holeLine01.EndPoint, 0, 0);
                        Point3D holePoint02 = GetSumPoint(holeLine02.EndPoint, 0, 0);

                        Vector3D holeAngle = new Vector3D(GetSumPoint(holeLine01.StartPoint, 0, 0), GetSumPoint(holeLine01.EndPoint, 0, 0));

                        CompositeCurve hole01 = GetBoltHoleHorizontal(holePoint01, boltHoleWidth, boltHoleHeight);
                        hole01.Rotate(holeAngle.Angle, Vector3D.AxisZ, holePoint01);
                        CompositeCurve hole02 = GetBoltHoleHorizontal(holePoint02, boltHoleWidth, boltHoleHeight);
                        hole02.Rotate(holeAngle.Angle, Vector3D.AxisZ, holePoint02);
                        customBlockList.AddRange(new CompositeCurve[] { hole01, hole02 });

                        List<Entity> circle01 = DrawReference_Hole(holePoint01, boltHoleHeight / 2, holeAngle.Angle, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                        List<Entity> circle02 = DrawReference_Hole(holePoint02, boltHoleHeight / 2, holeAngle.Angle, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                        customLine.AddRange(circle01);
                        customLine.AddRange(circle02);

                        if (scHoleQty == 4)
                        {
                            Line holeLine03 = (Line)holeLine01.Offset(scD, Vector3D.AxisZ, 0.01, true);// 아래로
                            Line holeLine04 = (Line)holeLine02.Offset(scD, Vector3D.AxisZ, 0.01, true);// 아래로
                            Point3D holePoint03 = GetSumPoint(holeLine03.EndPoint, 0, 0);
                            Point3D holePoint04 = GetSumPoint(holeLine04.EndPoint, 0, 0);

                            CompositeCurve hole03 = GetBoltHoleHorizontal(holePoint03, boltHoleWidth, boltHoleHeight);
                            hole03.Rotate(holeAngle.Angle, Vector3D.AxisZ, holePoint03);
                            CompositeCurve hole04 = GetBoltHoleHorizontal(holePoint04, boltHoleWidth, boltHoleHeight);
                            hole04.Rotate(holeAngle.Angle, Vector3D.AxisZ, holePoint04);
                            customBlockList.AddRange(new CompositeCurve[] { hole03, hole04 });

                            List<Entity> circle03 = DrawReference_Hole(holePoint03, boltHoleHeight / 2, holeAngle.Angle, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                            List<Entity> circle04 = DrawReference_Hole(holePoint04, boltHoleHeight / 2, holeAngle.Angle, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                            customLine.AddRange(circle03);
                            customLine.AddRange(circle04);
                        }

                        #endregion






                        #region CenterRing
                        StructureDRTCenteringInputModel centeringInput = new StructureDRTCenteringInputModel();
                        if (assemblyData.StructureDRTCenteringInput.Count > 0)
                            centeringInput = assemblyData.StructureDRTCenteringInput[firstIndex];

                        StructureCenteringModel centeringOutput = new StructureCenteringModel();
                        if (assemblyData.StructureDRTCenteringOutput.Count > 0)
                            centeringOutput = assemblyData.StructureDRTCenteringOutput[firstIndex];

                        double centeringOD = valueService.GetDoubleValue(centeringInput.CenteringOD);
                        double centeringID = 0;// 계산함
                        double centeringA = valueService.GetDoubleValue(centeringOutput.A);
                        double centeringB = valueService.GetDoubleValue(centeringOutput.B);
                        double centeringC = valueService.GetDoubleValue(centeringOutput.C);
                        double centeringD = valueService.GetDoubleValue(centeringOutput.D);
                        double centeringE = valueService.GetDoubleValue(centeringOutput.E);
                        double centeringT1 = valueService.GetDoubleValue(centeringOutput.t1);
                        double centeringT2 = valueService.GetDoubleValue(centeringOutput.t2);
                        //Cal

                        centeringID = (centeringOD - (centeringD * 2)) / 2;
                        centeringE = (centeringOD + (centeringC * 2)) / 2;
                        double centeringIDHalf = centeringOD / 2;






                        CDPoint centeringWP = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, centerRadius + centeringE, ref refPoint, ref curPoint);
                        //CDPoint centeringWP2 = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofUp, centerRadius + centeringE, ref refPoint, ref curPoint);

                        Point3D centeringSideTopLeft = GetSumPoint(centeringWP, 0, -centeringT1);
                        Point3D centeringSideTopRight = GetSumPoint(centeringWP, centeringC, -centeringT1);
                        Point3D centeringSideBottomLeft = GetSumPoint(centeringSideTopLeft, 0, -centeringB);
                        Point3D centeringSideBottomRight = GetSumPoint(centeringSideTopRight, 0, -centeringB);

                        Line cLine01 = new Line(GetSumPoint(centeringWP, 0, 0), GetSumPoint(centeringWP, centeringE, 0));
                        Line cLine02 = new Line(GetSumPoint(centeringWP, 0, -centeringT1), GetSumPoint(centeringWP, centeringE, -centeringT1));
                        Line cLine03 = new Line(GetSumPoint(centeringWP, 0, -centeringA + centeringT1), GetSumPoint(centeringWP, centeringE, -centeringA + centeringT1));
                        Line cLine04 = new Line(GetSumPoint(centeringWP, 0, -centeringA), GetSumPoint(centeringWP, centeringE, -centeringA));

                        Line cLineV01 = new Line(GetSumPoint(centeringWP, 0, 0), GetSumPoint(centeringWP, 0, -centeringT1));
                        Line cLineV02 = new Line(GetSumPoint(centeringWP, centeringC + centeringD, 0), GetSumPoint(centeringWP, centeringC + centeringD, -centeringT1));
                        Line cLineV03 = new Line(GetSumPoint(centeringWP, 0, -centeringA + centeringT1), GetSumPoint(centeringWP, 0, -centeringA));
                        Line cLineV04 = new Line(GetSumPoint(centeringWP, centeringC + centeringD, -centeringA + centeringT1), GetSumPoint(centeringWP, centeringC + centeringD, -centeringA));

                        Line cLineLongV01 = new Line(GetSumPoint(centeringWP, centeringC, -centeringT1), GetSumPoint(centeringWP, centeringC, -centeringA + centeringT1));
                        Line cLineLongV02 = new Line(GetSumPoint(centeringWP, centeringC + centeringT2, -centeringT1), GetSumPoint(centeringWP, centeringC + centeringT2, -centeringA + centeringT1));

                        customBlockList.AddRange(new Line[] { cLine01, cLine02, cLine03, cLine04, cLineV01, cLineV02, cLineV03, cLineV04, cLineLongV01, cLineLongV02 });
                        #endregion

                        // Leader
                        SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                        {
                            leaderPoint = GetSumCDPoint(cLine01.EndPoint, 0, 0),
                            lineTextList = new List<string>() {"C350x100x11x11",
                                                                "CENTER RING"
                                                                 },
                            lineLength = 30,
                            Position = "topleft"
                        });



                        #region Clip Center Side


                        StructureCenteringRafterModel firstRafter = new StructureCenteringRafterModel();
                        if (assemblyData.StructureDRTCenteringRafterOutput.Count > 0)
                            firstRafter = assemblyData.StructureDRTCenteringRafterOutput[0];

                        double frA = valueService.GetDoubleValue(firstRafter.A);
                        double frB = valueService.GetDoubleValue(firstRafter.B);
                        double frC = valueService.GetDoubleValue(firstRafter.C);
                        double frD = valueService.GetDoubleValue(firstRafter.D);
                        double frE = valueService.GetDoubleValue(firstRafter.E);

                        StructureClipCenteringSideModel centeringSideClip = new StructureClipCenteringSideModel();
                        if (assemblyData.StructureDRTClipCenteringSideOutput.Count > 0)
                            centeringSideClip = assemblyData.StructureDRTClipCenteringSideOutput[firstIndex];

                        double cClipA = valueService.GetDoubleValue(centeringSideClip.A);
                        double cClipB = valueService.GetDoubleValue(centeringSideClip.B);
                        double cClipC = valueService.GetDoubleValue(centeringSideClip.C);
                        double cClipD = valueService.GetDoubleValue(centeringSideClip.D);
                        double cClipE = valueService.GetDoubleValue(centeringSideClip.E);
                        double cClipHoleQty = valueService.GetDoubleValue(centeringSideClip.HoleQty);
                        double cClipA1 = valueService.GetDoubleValue(centeringSideClip.A1);
                        double cClipB1 = valueService.GetDoubleValue(centeringSideClip.B1);
                        double cClipC1 = valueService.GetDoubleValue(centeringSideClip.C1);
                        double cClipD1 = valueService.GetDoubleValue(centeringSideClip.D1);
                        double cClipE1 = valueService.GetDoubleValue(centeringSideClip.E1);
                        double cClipF1 = valueService.GetDoubleValue(centeringSideClip.F1);

                        double centeringBoltHoleHeight = valueService.GetDoubleValue(centeringSideClip.SlotHoleHt);
                        double centeringBboltHoleWidth = valueService.GetDoubleValue(centeringSideClip.SlotHoleWd);



                        // Clip
                        Line Line01 = new Line(GetSumPoint(centeringSideTopLeft, 0, 0), GetSumPoint(centeringSideTopRight, -cClipE1, 0));
                        Line Line02 = new Line(GetSumPoint(centeringSideTopRight, -cClipE1, 0), GetSumPoint(centeringSideTopRight, 0, -cClipE1));
                        Line Line03 = new Line(GetSumPoint(centeringSideTopRight, 0, -cClipE1), GetSumPoint(centeringSideBottomRight, 0, cClipE1));
                        Line Line04 = new Line(GetSumPoint(centeringSideBottomRight, 0, cClipE1), GetSumPoint(centeringSideBottomRight, -cClipE1, 0));
                        Line Line05 = new Line(GetSumPoint(centeringSideBottomRight, -cClipE1, 0), GetSumPoint(centeringSideBottomLeft, 0, 0));



                        // Rafter Height
                        CDPoint rafterStartRightTop = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, centerRadius + centeringE + cClipA1, ref refPoint, ref curPoint);
                        Circle rafterCircleLower = new Circle(GetSumPoint(workingPointService.DRTWorkingData.RoofCenterPoint, 0, 0), workingPointService.DRTWorkingData.DomeRaidus - frB);
                        Line vRafterLine = new Line(GetSumPoint(rafterStartRightTop, 0, 0), GetSumPoint(rafterStartRightTop, 0, -(frB * 2)));
                        Point3D[] rafterStartRightBottomTemp = vRafterLine.IntersectWith(rafterCircleLower);
                        Point3D rafterStartRightBottom = new Point3D();
                        if (rafterStartRightBottomTemp.Length > 0)
                            rafterStartRightBottom = GetSumPoint(rafterStartRightBottomTemp[0], 0, 0);

                        Point3D centeringSideLeftBottom = GetSumPoint(rafterStartRightBottom, -cClipB1 - cClipB1 - cClipC1, cClipB);
                        Point3D centeringSideLeftTop = GetSumPoint(centeringSideLeftBottom, 0, cClipC);
                        Point3D centeringSideRightTop = GetSumPoint(centeringSideLeftTop, +cClipB1 + cClipB1 + cClipC1, 0);


                        Line Line06 = new Line(GetSumPoint(centeringSideBottomLeft, 0, 0), GetSumPoint(centeringSideLeftBottom, 0, 0));
                        Line Line07 = new Line(GetSumPoint(centeringSideLeftBottom, 0, 0), GetSumPoint(centeringSideLeftTop, 0, 0));
                        Line Line08 = new Line(GetSumPoint(centeringSideLeftTop, 0, 0), GetSumPoint(centeringSideRightTop, 0, 0));

                        Line LineFillet01 = new Line(GetSumPoint(centeringSideRightTop, 0, 0), new Point3D(centeringWP.X, centeringSideRightTop.Y));
                        Line LineFillet02 = new Line(GetSumPoint(centeringWP, 0, 0), new Point3D(centeringWP.X, centeringSideRightTop.Y));
                        Arc arcFillet01;
                        if (Curve.Fillet(LineFillet01, LineFillet02, 20, true, false, true, true, out arcFillet01))
                            customBlockList.Add(arcFillet01);

                        customBlockList.AddRange(new Line[] { Line01, Line02, Line03, Line04, Line05, Line06, Line07, Line08, LineFillet01, LineFillet02 });


                        // Hole
                        Line centeringHoleLine01 = new Line(GetSumPoint(centeringSideLeftTop, cClipB1, -cClipD), GetSumPoint(centeringSideLeftTop, cClipB1 + cClipC1, -cClipD));
                        CompositeCurve centringHole01 = GetBoltHoleHorizontal(centeringHoleLine01.StartPoint, centeringBboltHoleWidth, centeringBoltHoleHeight);
                        CompositeCurve centringHole02 = GetBoltHoleHorizontal(centeringHoleLine01.EndPoint, centeringBboltHoleWidth, centeringBoltHoleHeight);
                        customBlockList.AddRange(new CompositeCurve[] { centringHole01, centringHole02 });

                        List<Entity> centeringCircle01 = DrawReference_Hole(centeringHoleLine01.StartPoint, centeringBoltHoleHeight / 2, 0, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                        List<Entity> centeringCircle02 = DrawReference_Hole(centeringHoleLine01.EndPoint, centeringBoltHoleHeight / 2, 0, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                        customLine.AddRange(centeringCircle01);
                        customLine.AddRange(centeringCircle02);

                        if (cClipHoleQty == 4)
                        {
                            Line centeringHoleLine02 = (Line)centeringHoleLine01.Offset(cClipE, Vector3D.AxisZ, 0.01, true);// 아래로
                            CompositeCurve centringHole03 = GetBoltHoleHorizontal(centeringHoleLine02.StartPoint, centeringBboltHoleWidth, centeringBoltHoleHeight);
                            CompositeCurve centringHole04 = GetBoltHoleHorizontal(centeringHoleLine02.EndPoint, centeringBboltHoleWidth, centeringBoltHoleHeight);
                            customBlockList.AddRange(new CompositeCurve[] { centringHole03, centringHole04 });

                            List<Entity> centeringCircle03 = DrawReference_Hole(centeringHoleLine02.StartPoint, centeringBoltHoleHeight / 2, 0, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                            List<Entity> centeringCircle04 = DrawReference_Hole(centeringHoleLine02.EndPoint, centeringBoltHoleHeight / 2, 0, new DrawCenterLineModel() { scaleValue = scaleValue, zeroEx = true });
                            customLine.AddRange(centeringCircle03);
                            customLine.AddRange(centeringCircle04);
                        }
                        #endregion


                        #region Centering Rafter



                        StructureCenteringRafterModel lastRafter = new StructureCenteringRafterModel();
                        if (assemblyData.StructureDRTCenteringRafterOutput.Count > 0)
                            lastRafter = assemblyData.StructureDRTCenteringRafterOutput[assemblyData.StructureDRTCenteringRafterOutput.Count - 1];

                        double lrA = valueService.GetDoubleValue(lastRafter.A);
                        double lrB = valueService.GetDoubleValue(lastRafter.B);
                        double lrC = valueService.GetDoubleValue(lastRafter.C);
                        double lrD = valueService.GetDoubleValue(lastRafter.D);
                        double lrE = valueService.GetDoubleValue(lastRafter.E);
                        double lrJ = valueService.GetDoubleValue(lastRafter.J);

                        Point3D lastRafterPointUp = GetSumPoint(leftTankRoofTopOrigin, 0, 0);
                        Point3D lastRafterPointDown = GetSumPoint(lastRafterPointUp, 0, 0) + (rafterDirection * lrB);
                        Point3D firstRafterPointUp = GetSumPoint(rafterStartRightTop, 0, 0);
                        Point3D firstRafterPointDown = GetSumPoint(rafterStartRightBottom, 0, 0);

                        Line rafterLeftLine = new Line(GetSumPoint(lastRafterPointUp, 0, 0), GetSumPoint(lastRafterPointDown, 0, 0));
                        Line rafterRightLine = new Line(GetSumPoint(firstRafterPointUp, 0, 0), GetSumPoint(firstRafterPointDown, 0, 0));
                        Arc roofBottom = new Arc(workingPointService.DRTWorkingData.RoofCenterPoint, lastRafterPointDown, firstRafterPointDown);






                        // HBeam
                        HBeamModel firstRafterHBeam = new HBeamModel();
                        HBeamModel lastRafterHBeam = new HBeamModel();
                        if (assemblyData.StructureDRTRafterHBeamOutput.Count > 0)
                        {
                            firstRafterHBeam = assemblyData.StructureDRTRafterHBeamOutput[0];
                            lastRafterHBeam = assemblyData.StructureDRTRafterHBeamOutput[assemblyData.StructureDRTRafterHBeamOutput.Count - 1];
                        }
                        double firtHBeamT2 = valueService.GetDoubleValue(firstRafterHBeam.t2);
                        double lastHBeamT2 = valueService.GetDoubleValue(lastRafterHBeam.t2);

                        // HBeam Up
                        Circle firstRafterPointHBeamTopCircle = new Circle(GetSumPoint(workingPointService.DRTWorkingData.RoofCenterPoint, 0, 0), workingPointService.DRTWorkingData.DomeRaidus - firtHBeamT2);
                        Point3D firstRafterPointHBeamTop = editingService.GetIntersectWidth(firstRafterPointHBeamTopCircle, rafterRightLine, 0);
                        Point3D lastRafterPointHBeamTop = editingService.GetIntersectWidth(firstRafterPointHBeamTopCircle, rafterLeftLine, 0);

                        Arc roofHBeamUp = new Arc(workingPointService.DRTWorkingData.RoofCenterPoint, lastRafterPointHBeamTop, firstRafterPointHBeamTop);

                        if (assemblyData.RoofCompressionRing[0].CompressionRingType == "Detail i")
                        {
                            Point3D newPoint = editingService.GetIntersectWidth(firstRafterPointHBeamTopCircle, newLineCompressionC, 0);
                            roofHBeamUp = new Arc(workingPointService.DRTWorkingData.RoofCenterPoint, newPoint, firstRafterPointHBeamTop);
                            Line newLineA = new Line(GetSumPoint(lastRafterPointUp, 0, 0), GetSumPoint(lastRafterPointUp, 0, 0) + (rafterDirection * lrB));
                            Circle newCircle = new Circle(workingPointService.DRTWorkingData.RoofCenterPoint, workingPointService.DRTWorkingData.DomeRaidus - lrB);
                            Point3D newCircleInter = editingService.GetIntersectWidth(newCircle, newLineA, 0);


                            rafterLeftLine = new Line(GetSumPoint(lastRafterPointUp, 0, 0), GetSumPoint(newCircleInter, 0, 0));
                            roofBottom = new Arc(workingPointService.DRTWorkingData.RoofCenterPoint, newCircleInter, firstRafterPointDown);



                        }


                        customBlockList.Add(rafterLeftLine);
                        customBlockList.Add(rafterRightLine);
                        customBlockList.Add(roofBottom);
                        customBlockList.Add(roofHBeamUp);

                        // HBeam Down
                        Circle firstRafterPointHBeamDownCircle = new Circle(GetSumPoint(workingPointService.DRTWorkingData.RoofCenterPoint, 0, 0), workingPointService.DRTWorkingData.DomeRaidus - lrB + firtHBeamT2);
                        Point3D firstRafterPointHBeamDown = editingService.GetIntersectWidth(firstRafterPointHBeamDownCircle, rafterRightLine, 0);
                        Point3D lastRafterPointHBeamDown = editingService.GetIntersectWidth(firstRafterPointHBeamDownCircle, rafterLeftLine, 0);

                        Arc roofHBeamDown = new Arc(workingPointService.DRTWorkingData.RoofCenterPoint, lastRafterPointHBeamDown, firstRafterPointHBeamDown);
                        //customBlockList.Add(roofHBeamDown);

                        // Circle
                        Point3D firstRafterPointHBeamDownAdj = editingService.GetIntersectLength(roofHBeamDown, lrJ, true);
                        Point3D lastRafterPointHBeamDownAdj = editingService.GetIntersectLength(roofHBeamDown, lrJ, false);

                        Point3D firstRafterPointDownAdj = editingService.GetIntersectLength(roofBottom, lrJ, true);
                        Point3D lastRafterPointDownAdj = editingService.GetIntersectLength(roofBottom, lrJ, false);

                        Arc roofHBeamDownAdj = new Arc(workingPointService.DRTWorkingData.RoofCenterPoint, lastRafterPointHBeamDownAdj, firstRafterPointHBeamDownAdj);
                        customBlockList.Add(roofHBeamDownAdj);


                        Line newHBeamVLine01 = new Line(GetSumPoint(lastRafterPointHBeamDownAdj, 0, 0), GetSumPoint(lastRafterPointDownAdj, 0, 0));
                        Line newHBeamVLine02 = new Line(GetSumPoint(firstRafterPointHBeamDownAdj, 0, 0), GetSumPoint(firstRafterPointDownAdj, 0, 0));
                        customBlockList.Add(newHBeamVLine01);
                        customBlockList.Add(newHBeamVLine02);

                        // Middle 용
                        Line newLineAMiddle = new Line(GetSumPoint(lastRafterPointUp, 0, 0), GetSumPoint(lastRafterPointUp, 0, 0) + (rafterDirection * lrB));
                        Circle newCircleMiddle = new Circle(workingPointService.DRTWorkingData.RoofCenterPoint, workingPointService.DRTWorkingData.DomeRaidus - lrB / 2);
                        Point3D newCircleMiddleInterLeft = editingService.GetIntersectWidth(newCircleMiddle, newLineAMiddle, 0);
                        Point3D newCircleMiddleInterRight = editingService.GetIntersectWidth(newCircleMiddle, rafterRightLine, 0);

                        Arc roofMiddle = new Arc(workingPointService.DRTWorkingData.RoofCenterPoint, newCircleMiddleInterLeft, newCircleMiddleInterRight);
                        styleService.SetLayer(ref roofMiddle, layerService.LayerCenterLine);
                        customLine.Add(roofMiddle);

                        #endregion

                        double rafterStartPointValue = 0;
                        double counti = 0;
                        foreach (StructureDRTRafterInputModel eachRafter in assemblyData.StructureDRTRafterInput)
                        {
                            double currentPointValue = valueService.GetDoubleValue(eachRafter.Radius);
                            double currentMidPointValue = (currentPointValue - rafterStartPointValue) / 2;
                            CDPoint rafterPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofUp, currentPointValue - currentMidPointValue, ref refPoint, ref curPoint);

                            // Leader
                            SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                            {
                                leaderPoint = GetSumCDPoint(rafterPoint, 0, -30),
                                lineTextList = new List<string>() {eachRafter.Size,
                                                                eachRafter.Qty + "-" + valueService.GetOrdinalNumber(counti+1) + " RAFTERS"
                                                                 },
                                lineLength = 30,
                                Position = "topleft"
                            });

                            rafterStartPointValue = currentPointValue;
                            counti++;
                        }

                        #region Girder
                        Circle girderCircle = new Circle(GetSumPoint(workingPointService.DRTWorkingData.RoofCenterPoint, 0, 0), workingPointService.DRTWorkingData.DomeRaidus);
                        for (int i = 0; i < assemblyData.StructureDRTGirderInput.Count; i++)
                        {
                            StructureDRTGirderInputModel eachGirder = assemblyData.StructureDRTGirderInput[i];
                            HBeamModel eachHBream = assemblyData.StructureDRTGirderHBeamOutput[i];
                            double girderRadius = valueService.GetDoubleValue(eachGirder.Radius);
                            double hbreamSize = valueService.GetDoubleValue(eachHBream.A);
                            double hbreamSizeHalf = hbreamSize / 2;
                            double girderB = valueService.GetDoubleValue(eachHBream.B);
                            double girderBHalf = girderB / 2;

                            CDPoint eachGirderPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, centerRadius + girderRadius, ref refPoint, ref curPoint);

                            Vector3D girderDirection = new Vector3D(GetSumPoint(eachGirderPoint, 0, 0), workingPointService.DRTWorkingData.RoofCenterPoint);
                            girderDirection.Normalize();
                            Line girderCenterLine = new Line(GetSumPoint(eachGirderPoint, 0, 0), GetSumPoint(eachGirderPoint, 0, 0) + (girderDirection * girderBHalf));
                            Line girderLineRight = (Line)girderCenterLine.Offset(-hbreamSizeHalf, Vector3D.AxisZ, 0.01, true);// 위로
                            Line girderLineLeft = (Line)girderCenterLine.Offset(hbreamSizeHalf, Vector3D.AxisZ, 0.01, true);// 위로

                            Point3D girderLineRightPoint = editingService.GetIntersectWidth(girderCircle, girderLineRight, 0);
                            Line girderLineRightNew = new Line(GetSumPoint(girderLineRightPoint, 0, 0), GetSumPoint(girderLineRightPoint, 0, 0) + (girderDirection * girderBHalf));

                            Point3D girderLineLeftPoint = editingService.GetIntersectWidth(girderCircle, girderLineLeft, 0);
                            Line girderLineLefttNew = new Line(GetSumPoint(girderLineLeftPoint, 0, 0), GetSumPoint(girderLineLeftPoint, 0, 0) + (girderDirection * girderBHalf));

                            Point3D girderCenterPoint = girderLineRightNew.EndPoint;

                            Entity[] eachGirderList = refBlockService.DrawReference_HBeam(GetSumCDPoint(girderCenterPoint, 0, 0), eachHBream);
                            foreach (Entity eachEntity in eachGirderList)
                                eachEntity.Rotate(girderDirection.Angle, Vector3D.AxisZ, GetSumPoint(girderCenterPoint, 0, 0));

                            customBlockList.AddRange(eachGirderList);
                            //customBlockList.Add(girderCenterLine);
                            //customBlockList.Add(girderLineRight);

                            // Leader
                            SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                            {
                                leaderPoint = GetSumCDPoint(girderLineLefttNew.EndPoint, 0, 0),
                                lineTextList = new List<string>() {eachGirder.Qty + "-" + valueService.GetOrdinalNumber(i+1) + " GIRDERS",
                                                                eachGirder.Size },

                                Position = "bottomleft"
                            });
                        }
                        #endregion


                    }
                    else if (StructureDivService.centeringInEx == "external")
                    {

                    }
                }




                styleService.SetLayerListEntity(ref customBlockList, layerService.LayerOutLine);

                customBlockList.AddRange(customLine);

            }

            return customBlockList.ToArray();
        }








        public Entity[] DrawBlock_Roof(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint, double selScaleValue)
        {
            int firstIndex = 0;
            Point3D drawPoint = new Point3D(selPoint1.X, selPoint1.Y, selPoint1.Z);


            List<Entity> customBlockList = new List<Entity>();



            if (SingletonData.TankType == TANK_TYPE.CRT ||
                SingletonData.TankType == TANK_TYPE.IFRT)
            {
                CDPoint LeftUp = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointLeftRoofUp, ref refPoint, ref curPoint);
                CDPoint LeftDown = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointLeftRoofDown, ref refPoint, ref curPoint);
                CDPoint CenterUp = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterTopUp, ref refPoint, ref curPoint);
                CDPoint CenterDown = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterTopDown, ref refPoint, ref curPoint);

                customBlockList.Add(new Line(GetSumPoint(LeftUp, 0, 0), GetSumPoint(CenterUp, 0, 0)));
                customBlockList.Add(new Line(GetSumPoint(LeftUp, 0, 0), GetSumPoint(LeftDown, 0, 0)));
                customBlockList.Add(new Line(GetSumPoint(LeftDown, 0, 0), GetSumPoint(CenterDown, 0, 0)));

                Plane pl1 = Plane.YZ;
                pl1.Origin.X = CenterUp.X;
                pl1.Origin.Y = CenterUp.Y;
                List<Entity> customBlockMirrorList = editingService.GetEntityByMirror(pl1, customBlockList);
                customBlockList.AddRange(customBlockMirrorList);

                styleService.SetLayerListEntity(ref customBlockList, layerService.LayerOutLine);
            }
            else if (SingletonData.TankType == TANK_TYPE.DRT)
            {
                CDPoint LeftUp = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointLeftRoofUp, ref refPoint, ref curPoint);
                CDPoint LeftDown = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointLeftRoofDown, ref refPoint, ref curPoint);
                CDPoint CenterUp = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterTopUp, ref refPoint, ref curPoint);
                CDPoint CenterDown = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterTopDown, ref refPoint, ref curPoint);
                customBlockList.Add(new Arc(workingPointService.DRTWorkingData.RoofCenterPoint, GetSumPoint(LeftUp, 0, 0), GetSumPoint(CenterUp, 0, 0)));
                customBlockList.Add(new Arc(workingPointService.DRTWorkingData.RoofCenterPoint, GetSumPoint(LeftDown, 0, 0), GetSumPoint(CenterDown, 0, 0)));
                customBlockList.Add(new Line(GetSumPoint(LeftUp, 0, 0), GetSumPoint(LeftDown, 0, 0)));

                Plane pl1 = Plane.YZ;
                pl1.Origin.X = CenterUp.X;
                pl1.Origin.Y = CenterUp.Y;
                List<Entity> customBlockMirrorList = editingService.GetEntityByMirror(pl1, customBlockList);
                customBlockList.AddRange(customBlockMirrorList);

                styleService.SetLayerListEntity(ref customBlockList, layerService.LayerOutLine);
            }
            else
            {
                CDPoint LeftDown = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointLeftRoofDown, ref refPoint, ref curPoint);
                CDPoint CenterDown = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterTopDown, ref refPoint, ref curPoint);
                double topRoofLineWidth = (CenterDown.X - LeftDown.X)*2;
                Line newRoofLine = new Line(GetSumPoint(LeftDown, 0, 0), GetSumPoint(LeftDown, topRoofLineWidth, 0));

                styleService.SetLayer(ref newRoofLine, layerService.LayerOutLine);
                customBlockList.Add(newRoofLine);

            }


            // Add
            if (SingletonData.TankType == TANK_TYPE.IFRT)
            {
                customBlockList.AddRange(DrawBlock_FrtSingle(selPoint1, ref refPoint, ref curPoint, selScaleValue));
            }
            else if (SingletonData.TankType == TANK_TYPE.EFRTSingle)
            {
                customBlockList.AddRange(DrawBlock_FrtSingle(selPoint1, ref refPoint, ref curPoint, selScaleValue));
            }
            else if (SingletonData.TankType == TANK_TYPE.EFRTDouble)
            {
                customBlockList.AddRange(DrawBlock_FrtDouble(selPoint1, ref refPoint, ref curPoint, selScaleValue));
            }







            return customBlockList.ToArray();
        }

        public Entity[] DrawBlock_FrtSingle(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint, double selScaleValue)
        {
            int firstIndex = 0;
            Point3D drawPoint = new Point3D(selPoint1.X, selPoint1.Y, selPoint1.Z);

            List<Entity> customBlockList = new List<Entity>();


            DrawFRTBlockService drawFRT = new DrawFRTBlockService();

            List<Entity> newList = new List<Entity>();
            List<Point3D> newOutPoint = new List<Point3D>();


            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);


            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[firstIndex].SizeNominalID);
            double tankIDHalf = tankID / 2;

            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[firstIndex].SizeTankHeight);

            Point3D tankBottomMidPoint = GetSumPoint(referencePoint, tankIDHalf, 0);

            // Fix
            string topSlope = "1/64";
            double topSlopeDegree = valueService.GetDegreeOfSlope(topSlope);


            string bottomSlope = assemblyData.BottomInput[firstIndex].BottomPlateSlope;
            double bottomSlopeDegree = valueService.GetDegreeOfSlope(bottomSlope); ;

            // 매우중요
            double pontoonPositionHeight = drawFRT.GetPontoonPositionHeight(tankHeight);


            double shellLeft = 200;             // Default : Shell to Rim Space

            double pontoonLeftHeight = 800;     // Default : Outer-rim Height
            double pontoonTopLeftExt = 100;
            double pontoonTopRightExt = 15;
            double pontoonBottomLeftExt = 10;
            double pontoonBottomRightExt = 10;
            double pontoonRightFlatWidth = 220;
            double pontoonRightFlatOverlap = 40;

            double pontoonWidth = valueService.GetDoubleValue(assemblyData.RoofIFRTInput[0].PonttonWidth);

                double pontoonLenghtG = 150;    // Default : Ring Plate Height 

            // 중앙까지 계산 해야 함
            double pontoonFlatLength = tankIDHalf - (shellLeft + pontoonWidth + pontoonRightFlatWidth - pontoonRightFlatOverlap);


            // Default : Sheet에 값 있으나 임시로 적용
            double pontoonThkA = 10;
            double pontoonThkB = 5;
            double pontoonThkC = 10;
            double pontoonThkD = 15;
            double pontoonThkE = 15;
            double pontoonThkF = 5;



            //newList.Add(new Line(GetSumPoint(referencePoint, 0, 0), GetSumPoint(referencePoint, 0, tankHeight)));
            //newList.Add(new Line(GetSumPoint(referencePoint, 0, 0), GetSumPoint(referencePoint, tankIDHalf, 0)));

            Point3D ABottomLeftPoint = GetSumPoint(referencePoint, shellLeft, pontoonPositionHeight);
            Point3D BBottomLeftPoint = GetSumPoint(referencePoint, shellLeft + pontoonThkA, pontoonPositionHeight + pontoonLeftHeight);
            Point3D CTopLeftPoint = GetSumPoint(referencePoint, shellLeft - pontoonBottomLeftExt,
                                                pontoonPositionHeight - valueService.GetOppositeByAdjacent(bottomSlopeDegree, pontoonBottomLeftExt + pontoonThkA));
            Point3D DTopRightPoint = GetSumPoint(referencePoint, shellLeft + pontoonWidth,
                                                pontoonPositionHeight + pontoonLeftHeight - valueService.GetOppositeByAdjacent(topSlopeDegree, pontoonWidth - pontoonThkA));
            Point3D DBottomRightPoint = GetSumPoint(referencePoint, shellLeft + pontoonWidth,
                                                pontoonPositionHeight + valueService.GetOppositeByAdjacent(bottomSlopeDegree, pontoonWidth - pontoonThkA));
            Point3D EBottomLeftPoint = GetSumPoint(DBottomRightPoint, 0, pontoonLenghtG + pontoonThkF);
            Point3D FTopLeftPoint = GetSumPoint(EBottomLeftPoint, pontoonRightFlatWidth - pontoonRightFlatOverlap, 0);

            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(ABottomLeftPoint, 0, 0), pontoonThkA, pontoonLeftHeight + pontoonTopLeftExt, 0, 0, 3));
            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(BBottomLeftPoint, 0, 0),
                                valueService.GetHypotenuseByWidth(topSlopeDegree, pontoonWidth - pontoonThkA) +
                                valueService.GetHypotenuseByWidth(topSlopeDegree, pontoonTopRightExt) -
                                valueService.GetOppositeByAdjacent(topSlopeDegree, pontoonThkB), pontoonThkB, -topSlopeDegree, 3, 3));
            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(CTopLeftPoint, 0, 0),
                                valueService.GetHypotenuseByWidth(bottomSlopeDegree, pontoonWidth + pontoonBottomLeftExt) +
                                valueService.GetHypotenuseByWidth(bottomSlopeDegree, pontoonBottomRightExt) -
                                valueService.GetOppositeByAdjacent(bottomSlopeDegree, pontoonThkC), pontoonThkC, bottomSlopeDegree, 0, 0));
            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(DTopRightPoint, 0, 0), pontoonThkD, DTopRightPoint.Y - DBottomRightPoint.Y, 0, 0, 1));
            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(EBottomLeftPoint, 0, 0), pontoonRightFlatWidth, pontoonThkE, 0, 0, 3));
            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(FTopLeftPoint, 0, 0), pontoonFlatLength, pontoonThkF, 0, 0, 0, new bool[] { true, false, true, true }));

            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);


            // Seal
            string sealTypeString = GetFRTSealType();
            if (sealTypeString.Contains("mechanical"))
                sealTypeString = "MECHANICAL SEAL STYTEM";
            else if (sealTypeString.Contains("soft"))
                sealTypeString = "SOFT SEAL STYTEM";
            // Leader
            SingletonData.LeaderPublicList.Add(new LeaderPointModel()
            {

                leaderPoint = GetSumCDPoint(ABottomLeftPoint, -shellLeft-30, pontoonLeftHeight),
                lineTextList = new List<string>() { sealTypeString },
                Position = "topleft"
            });


            // Leader
            SingletonData.LeaderPublicList.Add(new LeaderPointModel()
            {

                leaderPoint = GetSumCDPoint(CTopLeftPoint, pontoonWidth /2,-30),
                lineTextList = new List<string>() { "PONTOON" },
                lineLength = -50,
                Position = "bottomright"
            });

            // Leader
            SingletonData.LeaderPublicList.Add(new LeaderPointModel()
            {

                leaderPoint = GetSumCDPoint(DBottomRightPoint, 1000, -10),
                lineTextList = new List<string>() { "DECK PLATE(t10)" },
                lineLength = -50,
                Position = "bottomright"
            });

            // Deck Support
            List<Point3D> deckSupportPointList = new List<Point3D>();
            List<string> deckSupportNumCountList = new List<string>();
            // flat Thickness 만큼 내려 줘야 함
            Point3D deckBottomPoint = GetSumPoint(FTopLeftPoint, 0, -pontoonThkF);
            deckBottomPoint.X = tankBottomMidPoint.X;
            if (SingletonData.TankType == TANK_TYPE.IFRT)
            {
                foreach (DeckSupportIFRTInputModel eachDeck in assemblyData.DeckSupportIFRTInput)
                {
                    string eachRadius = eachDeck.Radius.Trim();
                    if (eachRadius != "")
                    {
                        deckSupportPointList.Add(GetSumPoint(deckBottomPoint, -valueService.GetDoubleValue(eachRadius), 0));
                        deckSupportNumCountList.Add(eachDeck.Qty);
                    }
                }
            }
            else if (SingletonData.TankType == TANK_TYPE.EFRTSingle)
            {
                foreach (DeckSupportEFRTSingleInputModel eachDeck in assemblyData.DeckSupportEFTSingleInput)
                {
                    string eachRadius = eachDeck.Radius.Trim();
                    if (eachRadius != "")
                    {
                        deckSupportPointList.Add(GetSumPoint(deckBottomPoint, -valueService.GetDoubleValue(eachRadius), 0));
                        deckSupportNumCountList.Add(eachDeck.Qty);
                    }
                }
            }

            double tempDeckSupportLowerHeight = 1250 + 450 +10;
            double tempDeckSupportMidXValue=GetSumPoint(refPoint, tankIDHalf / 2, 0).X;
            string tempDeckSupportPostion = "";
            int tempDeckSupportNum = 0;
            List<Entity> deckSupportList = new List<Entity>();
            foreach (Point3D eachPoint in deckSupportPointList)
            {
                deckSupportList.AddRange(drawFRT.DrawFRT_DeckSupport(eachPoint, selScaleValue));
                // Leader
                tempDeckSupportPostion = "bottomleft";
                if (eachPoint.X > tempDeckSupportMidXValue)
                    tempDeckSupportPostion = "bottomright";
                SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                {
                    leaderPoint = GetSumCDPoint(eachPoint, 0, -tempDeckSupportLowerHeight),
                    lineTextList = new List<string>() { deckSupportNumCountList[tempDeckSupportNum] + "-DECK POSTS", "(3\" SCH.80)" },
                    lineLength = -60,
                    Position = tempDeckSupportPostion
                });
                tempDeckSupportNum++;
            }
            newList.AddRange(deckSupportList);

            // Nozzle
            List<Entity> nozzleList = new List<Entity>();
            List<Entity> nozzleOneList = new List<Entity>();
            foreach (NozzleRoofInputModel eachNozzle in assemblyData.NozzleRoofInputModel)
            {
                if (eachNozzle.AutoBleederVent.ToLower() == "yes")
                    nozzleList.AddRange(drawFRT.DrawFRT_Nozzle_AutoBleederVent(GetSumPoint(deckBottomPoint, -valueService.GetDoubleValue(eachNozzle.R),0),selScaleValue));
                else if (eachNozzle.RimVent.ToLower() == "yes")
                {
                    RimVentNozzleModel selRimVent = null;
                    foreach (RimVentNozzleModel eachRim in assemblyData.RimVentNozzleList)
                        if(eachNozzle.Size== eachRim.NPS)
                            selRimVent = eachRim;

                    if (selRimVent != null)
                    {
                        double rimA = valueService.GetDoubleValue(selRimVent.A);
                        double rimB = valueService.GetDoubleValue(selRimVent.B);
                        Point3D rimWp = GetSumPoint(ABottomLeftPoint, 0, pontoonLeftHeight);
                        Point3D topPoint = GetSumPoint(rimWp, rimA, -valueService.GetOppositeByAdjacent(topSlopeDegree, rimA- pontoonThkA));
                        Point3D leftPoint = GetSumPoint(rimWp, 0, -rimB);

                        nozzleOneList.AddRange(drawFRT.DrawFRT_Nozzle_RimVent(GetSumPoint(topPoint, 0, 0), GetSumPoint(leftPoint, 0, 0),selRimVent,eachNozzle,topSlopeDegree, selScaleValue, ref assemblyData));
                    }
                }
                else if (eachNozzle.RoofDrainSump.ToLower() == "yes")
                {
                    double roofDrainSumpRadius = valueService.GetDoubleValue(eachNozzle.R);
                    Point3D upperPoint = null;
                    Point3D lowerPoint = new Point3D(tankBottomMidPoint.X - roofDrainSumpRadius, DBottomRightPoint.Y + pontoonLenghtG);

                    nozzleOneList.AddRange(drawFRT.DrawFRT_Nozzle_RoofDrainSump(GetSumPoint(refPoint,0,0), GetSumPoint(lowerPoint, 0, 0), upperPoint,  eachNozzle, topSlopeDegree, selScaleValue, ref assemblyData));
                }
            }
            newList.AddRange(nozzleList);

            // Seal
            string sealType = GetFRTSealType();
            if (sealType.Contains("mechanical"))
                newList.AddRange(drawFRT.DrawFRT_SealMechanical(GetSumPoint(ABottomLeftPoint, 0, pontoonLeftHeight + pontoonTopLeftExt)));
            else if (sealType.Contains("soft"))
                newList.AddRange(drawFRT.DrawFRT_SealSoft(GetSumPoint(ABottomLeftPoint, 0, pontoonLeftHeight + pontoonTopLeftExt), selScaleValue));





            // Right Mirror
            Plane mirrorPlane = Plane.YZ;
            mirrorPlane.Origin.X = tankBottomMidPoint.X;
            mirrorPlane.Origin.Y = tankBottomMidPoint.Y;
            List<Entity> newRightList = editingService.GetEntityByMirror(mirrorPlane, newList);
            styleService.SetLayerListEntityExcludingCenterLine(ref newRightList, layerService.LayerHiddenLine);

            customBlockList.AddRange(newList);
            customBlockList.AddRange(newRightList);


            // Rolling Ladder
            if(SingletonData.TankType==TANK_TYPE.EFRTSingle || SingletonData.TankType == TANK_TYPE.EFRTDouble)
            {
                // RunWay
                // ID : 2/3 Point
                double runWayWidth = drawFRT.GetRollingLadderWidth(tankID);
                Point3D runWayPoint = new Point3D(GetSumPoint(referencePoint, tankIDHalf * 2 / 3, 0).X, FTopLeftPoint.Y);
                List<Entity> runWayList = new List<Entity>();
                runWayList.AddRange(drawFRT.DrawFRT_RunWay(runWayPoint, runWayWidth));

                customBlockList.AddRange(runWayList);

                // Rolling Ladder
                double tankLastShellThickness = 0;
                if (assemblyData.ShellOutput.Count > 0)
                    tankLastShellThickness = valueService.GetDoubleValue(assemblyData.ShellOutput[0].Thickness);
                Point3D rollingLadderStartPoint = GetSumPoint(referencePoint, -tankLastShellThickness, tankHeight);
                Point3D rollingLadderEndPoint = GetSumPoint(runWayPoint, 0, 350); // 350 고정
                List<Entity> rollingLadderList = new List<Entity>();
                rollingLadderList.AddRange(drawFRT.DrawFRT_RollingLadder(rollingLadderStartPoint, rollingLadderEndPoint, selScaleValue));

                customBlockList.AddRange(rollingLadderList);
            }

            customBlockList.AddRange(nozzleOneList);


            return customBlockList.ToArray();
        }
        public string GetFRTSealType()
        {
            string sealType = "";
            if (SingletonData.TankType == TANK_TYPE.IFRT)
                 sealType = assemblyData.RoofIFRTInput[0].SelaSystemType.ToLower();
            else if (SingletonData.TankType == TANK_TYPE.EFRTSingle)
                sealType = assemblyData.RoofEFRTSingleInput[0].SelaSystemType.ToLower();
            else if (SingletonData.TankType == TANK_TYPE.EFRTDouble)
                sealType = assemblyData.RoofEFRTDoubleInput[0].SealSystemType.ToLower();

            return sealType;
        }

        public Entity[] DrawBlock_FrtDouble(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint, double selScaleValue)
        {
            int firstIndex = 0;
            Point3D drawPoint = new Point3D(selPoint1.X, selPoint1.Y, selPoint1.Z);

            List<Entity> customBlockList = new List<Entity>();


            DrawFRTBlockService drawFRT = new DrawFRTBlockService();

            List<Entity> newList = new List<Entity>();
            List<Point3D> newOutPoint = new List<Point3D>();


            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);


            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[firstIndex].SizeNominalID);
            double tankIDHalf = tankID / 2;

            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[firstIndex].SizeTankHeight);

            Point3D tankBottomMidPoint = GetSumPoint(referencePoint, tankIDHalf, 0);

            // Fix
            string topSlope = "1/64";
            double topSlopeDegree = valueService.GetDegreeOfSlope(topSlope);


            string bottomSlope = assemblyData.BottomInput[firstIndex].BottomPlateSlope;
            double bottomSlopeDegree = valueService.GetDegreeOfSlope(bottomSlope); ;

            // 매우중요
            double pontoonPositionHeight = drawFRT.GetPontoonPositionHeight(tankHeight);


            double shellLeft = 200;             // Default : Shell to Rim Space

            double pontoonLeftHeight = 800;     // Default : Outer-rim height
            double pontoonTopLeftExt = 100;
            double pontoonTopRightExt = 15;
            double pontoonBottomLeftExt = 10;
            double pontoonBottomRightExt = 10;
            double pontoonRightFlatWidth = 220;
            double pontoonRightFlatOverlap = 40;



            // Add Ring : 매우중요
            List<double> ringList = new List<double>();
            foreach (RingEFRTDoubleInputModel eachRing in assemblyData.RingEFRTDoubleInput)
            {
                string ringRadius = eachRing.Ring.Trim();
                if (ringRadius != "")
                    ringList.Add(valueService.GetDoubleValue(ringRadius));
            }
            // Double Deck : Last Ring Radius : Calculation
            double pontoonWidth = tankIDHalf - ringList.Last() - shellLeft;

            double pontoonThkA = 10;
            double pontoonThkB = 5;
            double pontoonThkC = 10;
            double pontoonThkD = 15;

            double pontoonTopWidth = tankIDHalf - (shellLeft + pontoonThkA);
            double pontoonBottomWidth = tankIDHalf - (shellLeft - pontoonBottomLeftExt);
            double pontoonFlatBottomWidth = tankIDHalf - (shellLeft + pontoonWidth + valueService.GetOppositeByHypotenuse(bottomSlope, pontoonThkC));

            // 중앙까지 계산 해야 함
            //double pontoonFlatLength = tankIDHalf - (shellLeft + pontoonWidth + pontoonRightFlatWidth - pontoonRightFlatOverlap);



            //newList.Add(new Line(GetSumPoint(referencePoint, 0, 0), GetSumPoint(referencePoint, 0, 4000)));
            //newList.Add(new Line(GetSumPoint(referencePoint, 0, 0), GetSumPoint(referencePoint, 4000, 0)));

            Point3D ABottomLeftPoint = GetSumPoint(referencePoint, shellLeft, pontoonPositionHeight);
            Point3D BBottomLeftPoint = GetSumPoint(referencePoint, shellLeft + pontoonThkA, pontoonPositionHeight + pontoonLeftHeight);
            Point3D CTopLeftPoint = GetSumPoint(referencePoint, shellLeft - pontoonBottomLeftExt,
                                                pontoonPositionHeight - valueService.GetOppositeByAdjacent(bottomSlopeDegree, pontoonBottomLeftExt + pontoonThkA));
            Point3D DTopRightPoint = GetSumPoint(referencePoint, shellLeft + pontoonWidth,
                                                pontoonPositionHeight + pontoonLeftHeight - valueService.GetOppositeByAdjacent(topSlopeDegree, pontoonWidth - pontoonThkA));
            Point3D DBottomRightPoint = GetSumPoint(referencePoint, shellLeft + pontoonWidth,
                                                pontoonPositionHeight + valueService.GetOppositeByAdjacent(bottomSlopeDegree, pontoonWidth - pontoonThkA));
            Point3D CFlatTopLeftPoint = GetSumPoint(DBottomRightPoint, valueService.GetOppositeByHypotenuse(bottomSlope, pontoonThkC), 0);

            double DHeight = DTopRightPoint.Y - DBottomRightPoint.Y;

            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(ABottomLeftPoint, 0, 0), pontoonThkA, pontoonLeftHeight + pontoonTopLeftExt, 0, 0, 3));
            List<Entity> upperPlateList = shapeService.GetRectangle(out newOutPoint, GetSumPoint(BBottomLeftPoint, 0, 0),
                             valueService.GetHypotenuseByWidth(topSlopeDegree, pontoonTopWidth) -
                             valueService.GetOppositeByAdjacent(topSlopeDegree, pontoonThkB), pontoonThkB, -topSlopeDegree, 3, 3);
            newList.AddRange(upperPlateList);
            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(CTopLeftPoint, 0, 0),
                             valueService.GetHypotenuseByWidth(bottomSlopeDegree, pontoonWidth + pontoonBottomLeftExt), pontoonThkC, bottomSlopeDegree, 0, 0));
            //newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(DTopRightPoint, 0, 0), pontoonThkD, DHeight, 0, 0, 1));
            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(CFlatTopLeftPoint, 0, 0), pontoonFlatBottomWidth, pontoonThkC, 0, 0, 0));



            foreach (double eachE in ringList)
            {
                double eachRadius = tankBottomMidPoint.X - eachE;
                Point3D eachLeftBottomPoint = GetSumPoint(DBottomRightPoint, 0, 0); // 시작점 매우 중요
                eachLeftBottomPoint.X = eachRadius;
                double eachRingGap = eachLeftBottomPoint.X - DBottomRightPoint.X;
                double eachHeight = DHeight - valueService.GetOppositeByAdjacent(topSlopeDegree, eachRingGap);
                newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(eachLeftBottomPoint, 0, 0), pontoonThkD, eachHeight, 0, 0, 2));
            }


            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);


            // Seal
            string sealTypeString = GetFRTSealType();
            if (sealTypeString.Contains("mechanical"))
                sealTypeString = "MECHANICAL SEAL STYTEM";
            else if (sealTypeString.Contains("soft"))
                sealTypeString = "SOFT SEAL STYTEM";
            // Leader
            SingletonData.LeaderPublicList.Add(new LeaderPointModel()
            {

                leaderPoint = GetSumCDPoint(ABottomLeftPoint, -shellLeft - 30, pontoonLeftHeight),
                lineTextList = new List<string>() { sealTypeString },
                Position = "topleft"
            });

            // Leader
            SingletonData.LeaderPublicList.Add(new LeaderPointModel()
            {

                leaderPoint = GetSumCDPoint(CTopLeftPoint, pontoonWidth / 2,-30),
                lineTextList = new List<string>() { "PONTOON" },
                lineLength = -50,
                Position = "bottomright"
            });

            // Leader
            SingletonData.LeaderPublicList.Add(new LeaderPointModel()
            {

                leaderPoint = GetSumCDPoint(DTopRightPoint, 300, 10),
                lineTextList = new List<string>() { "UPPER DECK(t5)" },
                lineLength = 0,
                Position = "topright"
            });
            // Leader
            SingletonData.LeaderPublicList.Add(new LeaderPointModel()
            {

                leaderPoint = GetSumCDPoint(DBottomRightPoint, 300, -10),
                lineTextList = new List<string>() { "LOWER DECK(t7)" },
                lineLength = -50,
                Position = "bottomright"
            });


            // Deck Support
            List<Point3D> deckSupportPointList = new List<Point3D>();
            List<string> deckSupportNumCountList = new List<string>();
            // flat Thickness 만큼 내려 줘야 함
            Point3D deckBottomPoint = GetSumPoint(DBottomRightPoint, 0, 0);
            deckBottomPoint.X = tankBottomMidPoint.X;

            foreach (DeckSupportEFRTDoubleInputModel eachDeck in assemblyData.DeckSupportEFRTDoubleInput)
            {
                string eachRadius = eachDeck.Radius.Trim();
                if (eachRadius != "")
                {
                    deckSupportPointList.Add(GetSumPoint(deckBottomPoint, -valueService.GetDoubleValue(eachRadius), 0));
                    deckSupportNumCountList.Add(eachDeck.Qty);
                }
            }

            double tempDeckSupportLowerHeight = 1250 + 450 + 10;
            double tempDeckSupportMidXValue = GetSumPoint(refPoint, tankIDHalf / 2, 0).X;
            string tempDeckSupportPostion = "";
            int tempDeckSupportNum = 0;
            List<Entity> deckSupportList = new List<Entity>();
            foreach (Point3D eachPoint in deckSupportPointList)
            {
                deckSupportList.AddRange(drawFRT.DrawFRT_DeckSupport(eachPoint, selScaleValue));
                // Leader
                tempDeckSupportPostion = "bottomleft";
                if (eachPoint.X > tempDeckSupportMidXValue)
                    tempDeckSupportPostion = "bottomright";
                SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                {
                    leaderPoint = GetSumCDPoint(eachPoint, 0, -tempDeckSupportLowerHeight),
                    lineTextList = new List<string>() { deckSupportNumCountList[tempDeckSupportNum] + "-DECK POSTS","(3\" SCH.80)" },
                    lineLength = -60,
                    Position = tempDeckSupportPostion
                });
                tempDeckSupportNum++;
            }
            newList.AddRange(deckSupportList);



            // Nozzle
            List<Entity> nozzleList = new List<Entity>();
            List<Entity> nozzleOneList = new List<Entity>();
            foreach (NozzleRoofInputModel eachNozzle in assemblyData.NozzleRoofInputModel)
            {
                if (eachNozzle.AutoBleederVent.ToLower() == "yes")
                    nozzleList.AddRange(drawFRT.DrawFRT_Nozzle_AutoBleederVent(GetSumPoint(deckBottomPoint, -valueService.GetDoubleValue(eachNozzle.R), 0), selScaleValue));
                else if (eachNozzle.RimVent.ToLower() == "yes")
                {
                    RimVentNozzleModel selRimVent = null;
                    foreach (RimVentNozzleModel eachRim in assemblyData.RimVentNozzleList)
                        if (eachNozzle.Size == eachRim.NPS)
                            selRimVent = eachRim;

                    if (selRimVent != null)
                    {
                        double rimA = valueService.GetDoubleValue(selRimVent.A);
                        double rimB = valueService.GetDoubleValue(selRimVent.B);
                        Point3D rimWp = GetSumPoint(ABottomLeftPoint, 0, pontoonLeftHeight);
                        Point3D topPoint = GetSumPoint(rimWp, rimA, -valueService.GetOppositeByAdjacent(topSlopeDegree, rimA - pontoonThkA));
                        Point3D leftPoint = GetSumPoint(rimWp, 0, -rimB);

                        nozzleOneList.AddRange(drawFRT.DrawFRT_Nozzle_RimVent(GetSumPoint(topPoint, 0, 0), GetSumPoint(leftPoint, 0, 0), selRimVent, eachNozzle, topSlopeDegree, selScaleValue, ref assemblyData));
                    }
                }
                else if (eachNozzle.RoofDrainSump.ToLower() == "yes")
                {
                    double roofDrainSumpRadius = valueService.GetDoubleValue(eachNozzle.R);
                    double adjWidth = 100;
                    Line vUpperLine = new Line(GetSumPoint(tankBottomMidPoint, - roofDrainSumpRadius- adjWidth, 0), GetSumPoint(tankBottomMidPoint, - roofDrainSumpRadius- adjWidth, tankHeight));
                    Point3D vUpperPoint = null;
                    foreach (Entity eachEntity in upperPlateList)
                    {
                        Point3D eachPoint = editingService.GetIntersectWidth(vUpperLine, (ICurve)eachEntity, 0);
                        if (eachPoint.Y != 0)
                        {
                            if (vUpperPoint == null)
                                vUpperPoint = eachPoint;
                            if (vUpperPoint.Y > eachPoint.Y)
                                vUpperPoint = eachPoint;
                        }
                    }
                    Point3D upperPoint = GetSumPoint(vUpperPoint, adjWidth,- valueService.GetOppositeByAdjacent(topSlopeDegree,adjWidth));
                    Point3D lowerPoint = new Point3D(tankBottomMidPoint.X - roofDrainSumpRadius, CFlatTopLeftPoint.Y-pontoonThkC);

                    nozzleOneList.AddRange(drawFRT.DrawFRT_Nozzle_RoofDrainSump(GetSumPoint(refPoint, 0, 0),GetSumPoint(lowerPoint, 0, 0), GetSumPoint(upperPoint, 0, 0), eachNozzle, topSlopeDegree, selScaleValue, ref assemblyData));
                }
            }
            newList.AddRange(nozzleList);

            // Seal
            string sealType = GetFRTSealType();
            if (sealType.Contains("mechanical"))
                newList.AddRange(drawFRT.DrawFRT_SealMechanical(GetSumPoint(ABottomLeftPoint, 0, pontoonLeftHeight + pontoonTopLeftExt)));
            else if (sealType.Contains("soft"))
                newList.AddRange(drawFRT.DrawFRT_SealSoft(GetSumPoint(ABottomLeftPoint, 0, pontoonLeftHeight + pontoonTopLeftExt), selScaleValue));


            // Right Mirror
            Plane mirrorPlane = Plane.YZ;
            mirrorPlane.Origin.X = tankBottomMidPoint.X;
            mirrorPlane.Origin.Y = tankBottomMidPoint.Y;
            List<Entity> newRightList = editingService.GetEntityByMirror(mirrorPlane, newList);
            styleService.SetLayerListEntityExcludingCenterLine(ref newRightList, layerService.LayerHiddenLine);

            customBlockList.AddRange(newList);
            customBlockList.AddRange(newRightList);

            // Rolling Ladder
            if (SingletonData.TankType == TANK_TYPE.EFRTSingle || SingletonData.TankType == TANK_TYPE.EFRTDouble)
            {
                // RunWay
                // ID : 2/3 Point
                double runWayWidth = drawFRT.GetRollingLadderWidth(tankID);
                double runWayX = tankIDHalf * 2 / 3;
                Line vRunWayLine = new Line(GetSumPoint(referencePoint, runWayX, 0), GetSumPoint(referencePoint, runWayX, tankHeight));
                Point3D runWayPoint = null;
                foreach (Entity eachEntity in upperPlateList)
                {
                    Point3D eachPoint = editingService.GetIntersectWidth(vRunWayLine, (ICurve)eachEntity, 0);
                    if (runWayPoint == null)
                        runWayPoint = eachPoint;
                    if (runWayPoint.Y < eachPoint.Y)
                        runWayPoint = eachPoint;
                }

                List<Entity> runWayList = drawFRT.DrawFRT_RunWay(GetSumPoint(runWayPoint, 0, 0), runWayWidth);
                // Rotate
                foreach (Entity eachEntity in runWayList)
                    eachEntity.Rotate(-topSlopeDegree, Vector3D.AxisZ, runWayPoint);

                customBlockList.AddRange(runWayList);

                // Rolling Ladder
                double tankLastShellThickness = 24;
                Point3D rollingLadderStartPoint = GetSumPoint(referencePoint, -tankLastShellThickness, tankHeight);
                Point3D rollingLadderEndPoint = null;
                foreach (Entity eachEntity in runWayList)
                {
                    Point3D eachPoint = editingService.GetIntersectWidth(vRunWayLine, (ICurve)eachEntity, 0);
                    if (rollingLadderEndPoint == null)
                        rollingLadderEndPoint = eachPoint;
                    if (rollingLadderEndPoint.Y < eachPoint.Y)
                        rollingLadderEndPoint = eachPoint;
                }
                List<Entity> rollingLadderList = new List<Entity>();
                rollingLadderList.AddRange(drawFRT.DrawFRT_RollingLadder(rollingLadderStartPoint, rollingLadderEndPoint, selScaleValue));

                customBlockList.AddRange(rollingLadderList);
            }


            customBlockList.AddRange(nozzleOneList);


            return customBlockList.ToArray();
        }

        public Entity[] DrawBlock_Bottom(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            int firstIndex = 0;
            Point3D drawPoint = new Point3D(selPoint1.X, selPoint1.Y, selPoint1.Z);


            List<Entity> customBlockList = new List<Entity>();

            // Shell
            double bottomThk = valueService.GetDoubleValue(assemblyData.ShellOutput[0].Thickness);

            // Roof Slope
            string bottomSlopeString = assemblyData.BottomInput[firstIndex].BottomPlateSlope;
            double bottomSlopeDegree = valueService.GetDegreeOfSlope(bottomSlopeString);

            BottomInputModel bottomBase = assemblyData.BottomInput[firstIndex];
            double bottomThickness = valueService.GetDoubleValue(bottomBase.BottomPlateThickness);
            double annularThickness = valueService.GetDoubleValue(bottomBase.AnnularPlateThickness);
            double annularThickWidth = valueService.GetDoubleValue(bottomBase.AnnularPlateWidth);

            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);

            // 확인 필요
            double overLap = 70;

            double outsideProjection = publicFunService.GetBottomFocuntionOD(bottomBase.AnnularPlate);
            outsideProjection += bottomThk;
            //double weldingLeg = 9;
            //if (bottomBase.AnnularPlate=="Yes")
            //    outsideProjection = 60 + weldingLeg;
            //else
            //    outsideProjection = 30 + weldingLeg;


            CDPoint BottomLeftUp = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointLeftBottomUp, 0, ref refPoint, ref curPoint);
            CDPoint BottomLeftDown = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointLeftBottomDown, 0, ref refPoint, ref curPoint);

            CDPoint BottomRightUp = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomUp, 0, ref refPoint, ref curPoint);
            CDPoint BottomRightDown = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomDown, 0, ref refPoint, ref curPoint);


            // Drawing : Left
            if (bottomBase.AnnularPlate == "Yes")
            {
                CDPoint wpPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointLeftShellBottom, 0, ref refPoint, ref curPoint);
                customBlockList.Add(new Line(GetSumPoint(wpPoint, -outsideProjection, 0), GetSumPoint(wpPoint, annularThickWidth - outsideProjection, 0)));
                Line annularPoint = new Line(GetSumPoint(wpPoint, -outsideProjection, -annularThickness), GetSumPoint(wpPoint, annularThickWidth - outsideProjection, -annularThickness));
                customBlockList.Add(annularPoint);
                customBlockList.Add(new Line(GetSumPoint(wpPoint, -outsideProjection, 0), GetSumPoint(wpPoint, -outsideProjection, -annularThickness)));
                customBlockList.Add(new Line(GetSumPoint(wpPoint, annularThickWidth - outsideProjection, -annularThickness), GetSumPoint(wpPoint, annularThickWidth - outsideProjection, 0)));

                Point3D LeftDimPoint = GetSumPoint(annularPoint.StartPoint, 0, 0);
                CDPoint RightDimPoint = GetSumCDPoint(wpPoint, tankID + outsideProjection, 0);
                SingletonData.DimPublicList.Add(new DimensionPointModel()
                {

                    leftPoint = GetSumCDPoint(LeftDimPoint, 0, 0),
                    rightPoint = GetSumCDPoint(RightDimPoint, 0, 0),
                    dimHeight = 3500,
                    Text = "ANNULAR PLATE O.D " + Math.Round(LeftDimPoint.DistanceTo(GetSumPoint(RightDimPoint, 0, 0)), 0),
                    Position = "bottom"
                });
            }
            else
            {
                double outHeight = valueService.GetOppositeByWidth(bottomSlopeDegree, outsideProjection);
                double outLeftHeight = valueService.GetAdjacentByHypotenuse(bottomSlopeDegree, bottomThickness);
                double outLeftWidth = valueService.GetOppositeByHypotenuse(bottomSlopeDegree, bottomThickness);
                BottomLeftUp = GetSumCDPoint(refPoint, -outsideProjection, -outHeight);
                BottomLeftDown = GetSumCDPoint(BottomLeftUp, +outLeftWidth, -outLeftHeight);


            }

            customBlockList.Add(new Line(GetSumPoint(BottomLeftDown, 0, 0), GetSumPoint(BottomRightDown, 0, 0)));
            customBlockList.Add(new Line(GetSumPoint(BottomLeftUp, 0, 0), GetSumPoint(BottomRightUp, 0, 0)));
            customBlockList.Add(new Line(GetSumPoint(BottomLeftUp, 0, 0), GetSumPoint(BottomLeftDown, 0, 0)));

            styleService.SetLayerListLine(ref customBlockList, layerService.LayerOutLine);

            // Drawing : Right
            CDPoint BottomOuterLeft = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottom, 0, ref refPoint, ref curPoint);
            CDPoint BottomOuterRight = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointRightShellBottom, 0, ref refPoint, ref curPoint);
            Line outerUp = new Line(GetSumPoint(BottomOuterLeft, 0, 0), GetSumPoint(BottomOuterRight, outsideProjection, 0));
            Line outerDown = new Line(GetSumPoint(BottomOuterLeft, 0, -bottomThickness), GetSumPoint(BottomOuterRight, outsideProjection, -bottomThickness));
            Line outerRight = new Line(GetSumPoint(BottomOuterRight, outsideProjection, 0), GetSumPoint(BottomOuterRight, outsideProjection, -bottomThickness));

            styleService.SetLayer(ref outerUp, layerService.LayerOutLine);
            styleService.SetLayer(ref outerDown, layerService.LayerOutLine);
            styleService.SetLayer(ref outerRight, layerService.LayerOutLine);
            customBlockList.Add(outerUp);
            customBlockList.Add(outerDown);
            customBlockList.Add(outerRight);


            if (bottomBase.AnnularPlate != "Yes")
            {
                Point3D LeftDimPoint = GetSumPoint(BottomLeftDown, 0, 0);
                CDPoint RightDimPoint = GetSumCDPoint(outerRight.EndPoint, 0, 0);
                SingletonData.DimPublicList.Add(new DimensionPointModel()
                {

                    leftPoint = GetSumCDPoint(LeftDimPoint, 0, 0),
                    rightPoint = GetSumCDPoint(RightDimPoint, 0, 0),
                    dimHeight = 3500,
                    Text = "BOTTOM PLATE O.D " + Math.Round(LeftDimPoint.DistanceTo(GetSumPoint(RightDimPoint, 0, 0)), 0),
                    Position = "bottom"
                }); ;
            }



            if (bottomBase.AnnularPlate == "Yes")
            {
                // Leader
                SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                {
                    leaderPoint = new CDPoint(outerRight.EndPoint.X, outerRight.EndPoint.Y, 0),
                    lineTextList = new List<string>() { "t" + assemblyData.BottomInput[0].AnnularPlateThickness + " ANNULAR PLATE", },

                    //lineTextList = leaderDataService.GetLeaderLineTextByName("shellinsulation"),
                    //emptyTextList = leaderDataService.GetLeaderEmptyLineTextByName("shellinsulation"),
                    Position = "bottomright"
                });
            }
            else
            {

            }




            return customBlockList.ToArray();
        }

        public Entity[] DrawBlock_Shell(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            int firstIndex = 0;
            Point3D drawPoint = new Point3D(selPoint1.X, selPoint1.Y, selPoint1.Z);


            List<Entity> customBlockList = new List<Entity>();

            // Top Angle
            DrawStructureService StructureDivService = new DrawStructureService();
            StructureDivService.SetStructureData(SingletonData.TankType, assemblyData.StructureCRTInput[0].SupportingType, assemblyData.RoofCompressionRing[0].CompressionRingType);


            // Thank
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[firstIndex].SizeNominalID);
            double tankIDHalf = tankID / 2;
            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[firstIndex].SizeTankHeight);

            double plateWidth = valueService.GetDoubleValue(assemblyData.ShellInput[firstIndex].PlateWidth);
            double plateLength = valueService.GetDoubleValue(assemblyData.ShellInput[firstIndex].PlateMaxLength);

            // Working point
            CDPoint leftShellTopPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointLeftShellTopAdj, 0, ref refPoint, ref curPoint);
            double maxCourseY = leftShellTopPoint.Y - refPoint.Y;


            double courseCountMax = assemblyData.ShellOutput.Count;
            // External : Left : 아래쪽으로 부터 시작
            double courseCount = 0;
            double courseBaseY = 0;
            foreach (ShellOutputModel eachCourse in assemblyData.ShellOutput)
            {
                courseCount++;
                double plateWidthOfCourse = valueService.GetDoubleValue(eachCourse.PlateWidth);
                double plateThicknessOfcourse = valueService.GetDoubleValue(eachCourse.Thickness);

                //if (courseBaseY +plateWidthOfCourse > maxCourseY)
                //    plateWidthOfCourse = maxCourseY-courseBaseY;

                Line courseLeft = new Line(GetSumPoint(refPoint, -plateThicknessOfcourse, courseBaseY), GetSumPoint(refPoint, -plateThicknessOfcourse, courseBaseY + plateWidthOfCourse));
                styleService.SetLayer(ref courseLeft, layerService.LayerOutLine);
                Line courseBottom = new Line(GetSumPoint(refPoint, -plateThicknessOfcourse, courseBaseY), GetSumPoint(refPoint, 0, courseBaseY));
                styleService.SetLayer(ref courseBottom, layerService.LayerOutLine);
                Line courseTop = new Line(GetSumPoint(refPoint, -plateThicknessOfcourse, courseBaseY + plateWidthOfCourse), GetSumPoint(refPoint, 0, courseBaseY + plateWidthOfCourse));
                styleService.SetLayer(ref courseTop, layerService.LayerOutLine);

                // increase : plate width
                courseBaseY += plateWidthOfCourse;

                customBlockList.Add(courseLeft);
                customBlockList.Add(courseBottom);
                if (courseCount == courseCountMax - 1 && StructureDivService.originTopAngleType == "Detail k")
                    break;

                customBlockList.Add(courseTop);



            }

            // External : Right : 아래쪽으로 부터 시작
            CDPoint refPointRight = GetSumCDPoint(refPoint, tankID, 0);
            courseCount = 0;
            courseBaseY = 0;
            foreach (ShellOutputModel eachCourse in assemblyData.ShellOutput)
            {
                courseCount++;
                double plateWidthOfCourse = valueService.GetDoubleValue(eachCourse.PlateWidth);
                double plateThicknessOfcourse = valueService.GetDoubleValue(eachCourse.Thickness);

                //if (courseBaseY + plateWidthOfCourse > maxCourseY)
                //    plateWidthOfCourse = maxCourseY - courseBaseY;

                Line courseLeft = new Line(GetSumPoint(refPointRight, plateThicknessOfcourse, courseBaseY), GetSumPoint(refPointRight, plateThicknessOfcourse, courseBaseY + plateWidthOfCourse));
                styleService.SetLayer(ref courseLeft, layerService.LayerOutLine);
                Line courseBottom = new Line(GetSumPoint(refPointRight, +plateThicknessOfcourse, courseBaseY), GetSumPoint(refPointRight, 0, courseBaseY));
                styleService.SetLayer(ref courseBottom, layerService.LayerOutLine);
                Line courseTop = new Line(GetSumPoint(refPointRight, +plateThicknessOfcourse, courseBaseY + plateWidthOfCourse), GetSumPoint(refPointRight, 0, courseBaseY + plateWidthOfCourse));
                styleService.SetLayer(ref courseTop, layerService.LayerOutLine);

                // increase : plate width
                courseBaseY += plateWidthOfCourse;

                customBlockList.Add(courseLeft);
                customBlockList.Add(courseBottom);

                if (courseCount == courseCountMax - 1 && StructureDivService.originTopAngleType == "Detail k")
                    break;

                customBlockList.Add(courseTop);


            }


            // Internal : Left, Right
            Line internalLeft = null;
            Line internalRight = null;
            switch (StructureDivService.originTopAngleType)
            {
                case "Detail b":
                case "Detail d":
                case "Detail e":
                case "Detail i":
                    internalLeft = new Line(GetSumPoint(refPoint, 0, 0), new Point3D(refPoint.X, leftShellTopPoint.Y));
                    internalRight = new Line(GetSumPoint(refPoint, tankID, 0), new Point3D(refPoint.X + tankID, leftShellTopPoint.Y));
                    break;

                case "Detail k":
                    internalLeft = new Line(GetSumPoint(refPoint, 0, 0), new Point3D(refPoint.X, refPoint.Y + courseBaseY));
                    internalRight = new Line(GetSumPoint(refPoint, tankID, 0), new Point3D(refPoint.X + tankID, refPointRight.Y + courseBaseY));
                    break;
            }

            styleService.SetLayer(ref internalLeft, layerService.LayerOutLine);
            customBlockList.Add(internalLeft);

            styleService.SetLayer(ref internalRight, layerService.LayerHiddenLine);
            customBlockList.Add(internalRight);


            return customBlockList.ToArray();
        }

        public Entity[] DrawBlock_ShellRightOuter(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            int firstIndex = 0;
            Point3D drawPoint = new Point3D(selPoint1.X, selPoint1.Y, selPoint1.Z);

            CDPoint wpPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterTop, 0, ref refPoint, ref curPoint);

            int maxCourse = assemblyData.ShellOutput.Count - 1;


            List<Entity> customBlockList = new List<Entity>();

            // Thank
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[firstIndex].SizeNominalID);
            double tankIDHalf = tankID / 2;
            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[firstIndex].SizeTankHeight);

            double plateWidth = valueService.GetDoubleValue(assemblyData.ShellInput[firstIndex].PlateWidth);
            double plateLength = valueService.GetDoubleValue(assemblyData.ShellInput[firstIndex].PlateMaxLength);

            // Working point
            CDPoint leftShellTopPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointLeftShellTopAdj, 0, ref refPoint, ref curPoint);
            double maxCourseY = leftShellTopPoint.Y - refPoint.Y;

            double tankBaseCourseThk = valueService.GetDoubleValue(assemblyData.ShellOutput[firstIndex].Thickness);
            double tankODBase = tankID + (tankBaseCourseThk * 2) + Math.PI;
            double plateCount = Math.Ceiling(tankODBase / plateLength);
            double plateLengthOne = Math.Round(tankODBase / plateCount, 1, MidpointRounding.AwayFromZero);
            double plateLengthDivThree = Math.Round(plateLengthOne / 3, 1, MidpointRounding.AwayFromZero);

            // External : Left : 아래쪽으로 부터 시작
            double courseBaseY = 0;
            int courseCount = 0;
            foreach (ShellOutputModel eachCourse in assemblyData.ShellOutput)
            {
                double plateWidthOfCourse = valueService.GetDoubleValue(eachCourse.PlateWidth);
                double plateThicknessOfcourse = valueService.GetDoubleValue(eachCourse.Thickness);

                //if (courseBaseY + plateWidthOfCourse > maxCourseY)
                //    plateWidthOfCourse = maxCourseY - courseBaseY;

                // 마지막 라인은 그리지 않는다.
                Line line01 = new Line(new Point3D(wpPoint.X, refPoint.Y + courseBaseY + plateWidthOfCourse), new Point3D(wpPoint.X + tankIDHalf, refPoint.Y + courseBaseY + plateWidthOfCourse));
                customBlockList.Add(line01);

                courseCount++;
                double startPointX = plateLengthOne;
                double courseCountMod = courseCount % 3;
                switch (courseCountMod)
                {
                    case 1:
                        startPointX = plateLengthDivThree * 1;
                        break;
                    case 2:
                        startPointX = plateLengthDivThree * 2;
                        break;

                }
                double verCount = Math.Truncate((tankIDHalf - startPointX) / plateLengthOne) + 1;
                for (int i = 0; i < verCount; i++)
                {
                    double startPointXOne = startPointX + (plateLengthOne * i);
                    Line lineVer01 = new Line(new Point3D(wpPoint.X + startPointXOne, refPoint.Y + courseBaseY), new Point3D(wpPoint.X + startPointXOne, refPoint.Y + courseBaseY + plateWidthOfCourse));
                    customBlockList.Add(lineVer01);
                }

                // increase : plate width
                courseBaseY += plateWidthOfCourse;

                if (courseCount == maxCourse)
                    break;

            }

            //// Layer
            //styleService.SetLayerListLine(ref customBlockList, layerService.LayerOutLine);




            // 마지막 Course
            DrawStructureService StructureDivService = new DrawStructureService();
            StructureDivService.SetStructureData(SingletonData.TankType, assemblyData.StructureCRTInput[0].SupportingType, assemblyData.RoofCompressionRing[0].CompressionRingType);

            CDPoint wpShellTop = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointLeftShellTop, 0, ref refPoint, ref curPoint);

            string selAngleSize = assemblyData.RoofCompressionRing[firstIndex].AngleSize;
            AngleSizeModel selAngleModel = refBlockService.GetAngleSizeModel(selAngleSize);
            if (selAngleModel == null)
                selAngleModel = new AngleSizeModel();

            double A = valueService.GetDoubleValue(selAngleModel.A); // X
            double B = valueService.GetDoubleValue(selAngleModel.B); // Y

            courseCount++;
            double startPointXLast = plateLengthOne;
            double courseCountMod1 = courseCount % 3;
            if (courseCountMod1 == 1)
                startPointXLast = plateLengthDivThree * 1;
            else if (courseCountMod1 == 2)
                startPointXLast = plateLengthDivThree * 2;
            double verCount1 = Math.Truncate((tankIDHalf - startPointXLast) / plateLengthOne) + 1;



            Line lineLastCourse = null;
            switch (StructureDivService.originTopAngleType)
            {
                case "Detail b":
                case "Detail d":
                case "Detail e":
                    lineLastCourse = new Line(new Point3D(wpPoint.X, wpShellTop.Y - B), new Point3D(wpPoint.X + tankIDHalf, wpShellTop.Y - B));
                    for (int i = 0; i < verCount1; i++)
                    {
                        double startPointXOne = startPointXLast + (plateLengthOne * i);
                        Line lineVer01 = new Line(new Point3D(wpPoint.X + startPointXOne, refPoint.Y + courseBaseY), new Point3D(wpPoint.X + startPointXOne, wpShellTop.Y - B));
                        customBlockList.Add(lineVer01);
                    }
                    break;

                case "Detail i":
                    lineLastCourse = new Line(new Point3D(wpPoint.X, wpShellTop.Y), new Point3D(wpPoint.X + tankIDHalf, wpShellTop.Y));
                    for (int i = 0; i < verCount1; i++)
                    {
                        double startPointXOne = startPointXLast + (plateLengthOne * i);
                        Line lineVer01 = new Line(new Point3D(wpPoint.X + startPointXOne, refPoint.Y + courseBaseY), new Point3D(wpPoint.X + startPointXOne, wpShellTop.Y));
                        customBlockList.Add(lineVer01);
                    }
                    break;

                case "Detail k":
                    lineLastCourse = new Line(new Point3D(wpPoint.X, wpShellTop.Y), new Point3D(wpPoint.X + tankIDHalf, wpShellTop.Y));
                    // 그리지 않음
                    break;
            }
            customBlockList.Add(lineLastCourse);



            // Layer
            styleService.SetLayerListLine(ref customBlockList, layerService.LayerOutLine);



            return customBlockList.ToArray();
        }


        private CompositeCurve GetBoltHoleHorizontal(Point3D newCenter, double boltWidth, double boltRadius)
        {

            double boltHeight = boltRadius / 2;
            Point3D newCenterLeft = GetSumPoint(newCenter, -boltWidth / 2 + boltHeight, 0);
            Point3D newCenterRight = GetSumPoint(newCenter, +boltWidth / 2 - boltHeight, 0);

            Arc arcarc1 = new Arc(Plane.XY, newCenterLeft, boltHeight, GetSumPoint(newCenterLeft, 0, boltHeight), GetSumPoint(newCenterLeft, 0, -boltHeight), false);
            Arc arcarc2 = new Arc(Plane.XY, newCenterRight, boltHeight, GetSumPoint(newCenterRight, 0, boltHeight), GetSumPoint(newCenterRight, 0, -boltHeight), true);
            Line ll1 = new Line(arcarc1.StartPoint, arcarc2.EndPoint);
            Line ll2 = new Line(arcarc1.EndPoint, arcarc2.StartPoint);

            CompositeCurve newCom = new CompositeCurve(arcarc1, arcarc2, ll1, ll2);
            return newCom;
        }



        public Entity[] DrawBlock_WindGirder(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint, string mirrorSign)
        {

            int refFirstIndex = 0;
            int selwindGirderCount = valueService.GetIntValue(assemblyData.WindGirderInput[refFirstIndex].Qty);
            List<Entity> windGirderEntity = new List<Entity>();

            int windGirderCount = 0;
            foreach (WindGirderOutputModel eachWindGirder in assemblyData.WindGirderOutput)
            {
                windGirderCount++;

                AngleSizeModel selAngleModel = refBlockService.GetAngleSizeModel(eachWindGirder.Size);

                if (eachWindGirder.Type == "Detail c")
                {
                    // Left
                    CDPoint adjPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftShell, eachWindGirder.Elevation, ref refPoint, ref curPoint);
                    CDPoint drawPoint = GetSumCDPoint(adjPoint, -valueService.GetDoubleValue(selAngleModel.A), 0);
                    Point3D rotatePoint = GetSumPoint(adjPoint, 0, 0);
                    Entity[] angleEntity = refBlockService.DrawReference_Angle(drawPoint, selAngleModel);
                    if (angleEntity != null)
                        foreach (Entity eachEntity in angleEntity)
                            eachEntity.Rotate(UtilityEx.DegToRad(90), Vector3D.AxisZ, rotatePoint);

                    windGirderEntity.AddRange(angleEntity);


                    // Leader
                    if (mirrorSign == "")
                        SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                        {
                            leaderPoint = GetMirrorPoint(GetSumPoint(drawPoint, 0, -valueService.GetDoubleValue(selAngleModel.B)), ref refPoint, ref curPoint, mirrorSign),
                            lineTextList = new List<string>() { windGirderCount + "-WIND GIRDER", eachWindGirder.Size },

                            Position = mirrorSign == "right" ? "bottomright" : "bottomleft"
                        });

                }
                else if (eachWindGirder.Type == "Detail d")
                {
                    // Left
                    CDPoint adjPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftShell, eachWindGirder.Elevation, ref refPoint, ref curPoint);
                    CDPoint drawPoint = GetSumCDPoint(adjPoint, -valueService.GetDoubleValue(selAngleModel.A), -valueService.GetDoubleValue(selAngleModel.B));
                    Entity[] angleEntity = refBlockService.DrawReference_Angle(drawPoint, selAngleModel);

                    List<Entity> leftAngleList = new List<Entity>();
                    leftAngleList.AddRange(angleEntity);

                    // Left : Double
                    if (angleEntity != null)
                    {
                        CDPoint mirrorRefPoint = drawPoint;
                        Plane pl1 = Plane.YZ;
                        pl1.Origin.X = mirrorRefPoint.X;
                        pl1.Origin.Y = mirrorRefPoint.Y;
                        leftAngleList.AddRange(editingService.GetEntityByMirror(pl1, leftAngleList));
                    }

                    windGirderEntity.AddRange(leftAngleList);

                    // Leader
                    if (mirrorSign == "")
                        SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                        {
                            leaderPoint = GetMirrorPoint(GetSumPoint(drawPoint, -valueService.GetDoubleValue(selAngleModel.A), 0), ref refPoint, ref curPoint, mirrorSign),
                            lineTextList = new List<string>() { windGirderCount + "-WIND GIRDER", eachWindGirder.Size },

                            Position = mirrorSign == "right" ? "bottomright" : "bottomleft"
                        });
                }
                else if (eachWindGirder.Type == "Detail e")
                {
                    // 아직 구현 안됨
                }



            }





            return windGirderEntity.ToArray();
        }


        public CDPoint GetMirrorPoint(Point3D selPoint, ref CDPoint refPoint, ref CDPoint curPoint, string mirrorSign)
        {
            CDPoint newMirrorPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomUp, ref refPoint, ref curPoint);

            Point3D newPoint = GetSumPoint(selPoint, 0, 0);
            //Line newLinew = new Line(newPoint, GetSumPoint(newPoint, 0, 10));
            switch (mirrorSign)
            {
                case "right":

                    Plane pl1 = Plane.YZ;
                    pl1.Origin.X = newMirrorPoint.X;
                    pl1.Origin.Y = newMirrorPoint.Y;
                    Mirror customMirror = new Mirror(pl1);
                    newPoint.TransformBy(customMirror);
                    break;
            }

            return new CDPoint(newPoint.X, newPoint.Y, 0);

        }

        public Entity[] DrawBlock_WindGirderRightOuter(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint)
        {

            int refFirstIndex = 0;
            int selwindGirderCount = valueService.GetIntValue(assemblyData.WindGirderInput[refFirstIndex].Qty);
            List<Entity> windGirderEntity = new List<Entity>();

            CDPoint wpPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterTop, 0, ref refPoint, ref curPoint);

            double tankIDHalf = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID) / 2;
            int maxCourse = assemblyData.ShellOutput.Count - 1;
            double topShellTh = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness);

            foreach (WindGirderOutputModel eachWindGirder in assemblyData.WindGirderOutput)
            {

                AngleSizeModel selAngleModel = refBlockService.GetAngleSizeModel(eachWindGirder.Size);
                double selAngelModelT = valueService.GetDoubleValue(selAngleModel.t);
                double selAngelModelA = valueService.GetDoubleValue(selAngleModel.A);
                double selAngelModelB = valueService.GetDoubleValue(selAngleModel.B);
                if (eachWindGirder.Type == "Detail c")
                {
                    // Left
                    CDPoint adjPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftShell, eachWindGirder.Elevation, ref refPoint, ref curPoint);
                    List<Entity> angleEntity = new List<Entity>();

                    angleEntity.Add(new Line(wpPoint.X, adjPoint.Y, wpPoint.X + (wpPoint.X - adjPoint.X), adjPoint.Y));
                    Line hiddenMiddleLine = new Line(wpPoint.X, adjPoint.Y - selAngelModelT, wpPoint.X + (wpPoint.X - adjPoint.X) + selAngelModelT, adjPoint.Y - selAngelModelT);
                    angleEntity.Add(new Line(wpPoint.X, adjPoint.Y - selAngelModelB, wpPoint.X + (wpPoint.X - adjPoint.X) + selAngelModelA, adjPoint.Y - selAngelModelB));

                    styleService.SetLayerListLine(ref angleEntity, layerService.LayerOutLine);
                    styleService.SetLayer(ref hiddenMiddleLine, layerService.LayerHiddenLine);

                    angleEntity.Add(hiddenMiddleLine);
                    windGirderEntity.AddRange(angleEntity);


                }
                else if (eachWindGirder.Type == "Detail d")
                {
                    // Left
                    CDPoint adjPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftShell, eachWindGirder.Elevation, ref refPoint, ref curPoint);
                    CDPoint drawPoint = GetSumCDPoint(adjPoint, -valueService.GetDoubleValue(selAngleModel.A), -valueService.GetDoubleValue(selAngleModel.A));
                    List<Entity> angleEntity = new List<Entity>();

                    angleEntity.Add(new Line(wpPoint.X, adjPoint.Y, wpPoint.X + (wpPoint.X - adjPoint.X), adjPoint.Y));
                    Line hiddenMiddleLine = new Line(wpPoint.X, adjPoint.Y - selAngelModelT, wpPoint.X + (wpPoint.X - adjPoint.X) + (selAngelModelA * 2), adjPoint.Y - selAngelModelT);
                    angleEntity.Add(new Line(wpPoint.X, adjPoint.Y - selAngelModelB, wpPoint.X + (wpPoint.X - adjPoint.X) + (selAngelModelA * 2), adjPoint.Y - selAngelModelB));

                    styleService.SetLayerListLine(ref angleEntity, layerService.LayerOutLine);
                    styleService.SetLayer(ref hiddenMiddleLine, layerService.LayerHiddenLine);

                    angleEntity.Add(hiddenMiddleLine);
                    windGirderEntity.AddRange(angleEntity);

                }
                else if (eachWindGirder.Type == "Detail e")
                {
                    // 아직 구현 안됨
                }

            }





            return windGirderEntity.ToArray();
        }




        public Entity[] DrawBlock_InsulationRoof(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {

            int firstIndex = 0;

            double refLength = 15 * scaleValue;
            double refOverFit = 2;

            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[firstIndex].SizeTankHeight);
            double tankHeightHalf = tankHeight / 2;

            double tankNominalID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[firstIndex].SizeNominalID);
            double tankNominalIDHalf = tankNominalID / 2 / 2;
            double roofSlopeDegree = valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[firstIndex].RoofSlope);

            double insulationThickness = valueService.GetDoubleValue(assemblyData.RoofInsulation[firstIndex].Thickness);

            List<Entity> newList = new List<Entity>();
            if (SingletonData.TankType == TANK_TYPE.DRT || 
                SingletonData.TankType == TANK_TYPE.EFRTSingle ||
                SingletonData.TankType == TANK_TYPE.EFRTDouble)
                return newList.ToArray();

            CDPoint roofInsulationPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofUp, tankNominalIDHalf, ref refPoint, ref curPoint);
            Point3D roofInsulationPointOrigin = GetSumPoint(roofInsulationPoint, 0, 0);
            Line line01 = new Line(GetSumPoint(roofInsulationPoint, -refLength / 2, 0), GetSumPoint(roofInsulationPoint, -refLength / 2, insulationThickness + refOverFit));
            Line line02 = new Line(GetSumPoint(roofInsulationPoint, +refLength / 2, 0), GetSumPoint(roofInsulationPoint, +refLength / 2, insulationThickness + refOverFit));
            Line line03 = new Line(GetSumPoint(roofInsulationPoint, -refLength / 2, insulationThickness), GetSumPoint(roofInsulationPoint, +refLength / 2, insulationThickness ));

            





            newList.Add(line01);
            newList.Add(line02);
            newList.Add(line03);




            styleService.SetLayerListLine(ref newList, layerService.LayerVirtualLine);

            // Rotate
            //if (SingletonData.TankType == TANK_TYPE.DRT)
            //    roofSlopeDegree = editingService.GetAngleOfLine(new Line(GetSumPoint(roofInsulationPoint, 0, 0), workingPointService.DRTWorkingData.RoofCenterPoint));


            foreach (Entity eachEntity in newList)
                eachEntity.Rotate(roofSlopeDegree, Vector3D.AxisZ, roofInsulationPointOrigin);

            if (assemblyData.RoofCRTInput[0].InsulationRequired.ToLower() == "yes" || assemblyData.RoofDRTInput[0].InsulationRequired.ToLower() == "yes")
            {

                // Leader
                SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                {
                    leaderPoint = new CDPoint(line03.MidPoint.X, line03.MidPoint.Y, 0),
                    lineTextList = new List<string>() { "t" + assemblyData.RoofCRTInput[0].InsulationThickness + " INSULATION" },
                    //lineTextList = leaderDataService.GetLeaderLineTextByName("roofinsulation"),
                    //emptyTextList = leaderDataService.GetLeaderEmptyLineTextByName("roofinsulation"),
                    Position = "topleft",
                    lineLength = -20
                });
            }


            return newList.ToArray();
        }

        public Entity[] DrawBlock_InsulationShell(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint,double scaleValue)
        {

            int firstIndex = 0;

            double refLength = 15 * scaleValue;
            double refOverFit = 2;

            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[firstIndex].SizeTankHeight);
            double tankHeightHalf = tankHeight / 2;

            double tankNominalID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[firstIndex].SizeNominalID);
            double tankNominalIDHalf = tankNominalID / 2;
            double roofSlopeDegree = valueService.GetDegreeOfSlope(assemblyData.RoofCRTInput[firstIndex].RoofSlope);

            double insulationThickness = valueService.GetDoubleValue(assemblyData.ShellInput[firstIndex].InsulationThickness);



            CDPoint leftTopPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftShell, tankHeightHalf +refLength/2, ref refPoint, ref curPoint);
            CDPoint leftMiddlePoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftShell, tankHeightHalf, ref refPoint, ref curPoint);
            CDPoint leftBottomPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftShell, tankHeightHalf+ refLength / 2, ref refPoint, ref curPoint);

            if (leftTopPoint.X != leftBottomPoint.X)
            {
                // 한번 위로 올리기
                leftTopPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftShell, tankHeightHalf + refLength / 2 + refLength, ref refPoint, ref curPoint);
                leftMiddlePoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftShell, tankHeightHalf + refLength, ref refPoint, ref curPoint);
                leftBottomPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftShell, tankHeightHalf + refLength / 2 + refLength, ref refPoint, ref curPoint);
            }

            Line line01 = new Line(GetSumPoint(leftTopPoint, 0, 0), GetSumPoint(leftTopPoint, -insulationThickness - refOverFit, 0));
            Line line03 = new Line(GetSumPoint(leftTopPoint, 0, -refLength), GetSumPoint(leftTopPoint, -insulationThickness - refOverFit, -refLength));
            Line line02 = new Line(GetSumPoint(leftTopPoint, -insulationThickness,0 ), GetSumPoint(leftTopPoint, -insulationThickness, -refLength));


            List<Entity> newList = new List<Entity>();
            newList.Add(line01);
            newList.Add(line02);
            newList.Add(line03);

            styleService.SetLayerListLine(ref newList, layerService.LayerVirtualLine);

            if (assemblyData.RoofCRTInput[0].InsulationRequired.ToLower() == "yes" || assemblyData.RoofDRTInput[0].InsulationRequired.ToLower() == "yes")
            {
                // Leader
                SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                {
                    leaderPoint = new CDPoint(line02.MidPoint.X, line02.MidPoint.Y, 0),
                    lineTextList = new List<string>() { "t" + assemblyData.ShellInput[0].InsulationThickness + " INSULATION" },
                    //lineTextList = leaderDataService.GetLeaderLineTextByName("shellinsulation"),
                    //emptyTextList = leaderDataService.GetLeaderEmptyLineTextByName("shellinsulation"),
                    Position = "bottomleft"
                });
            }




            return newList.ToArray();
        }



        public Entity[] DrawBlock_AnchorChair(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {



            List<Entity> customEntity = new List<Entity>();
            List<Entity> customLine = new List<Entity>();

            
            if(assemblyData.AnchorageInput[0].AnchorChairBlot.ToLower() == "yes")
            {
                AnchorChairModel newAnchor = null;
                string boltSize = assemblyData.AnchorageInput[0].AnchorSize;
                foreach (AnchorChairModel eachModel in assemblyData.AnchorChairList)
                {
                    if (eachModel.Size == boltSize)
                    {
                        newAnchor = eachModel;
                        break;
                    }
                }
                if (newAnchor != null)
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

                    double C1 = valueService.GetDoubleValue(newAnchor.C1);
                    double B1 = valueService.GetDoubleValue(newAnchor.B1);
                    double H1 = valueService.GetDoubleValue(newAnchor.H1);
                    double G1 = valueService.GetDoubleValue(newAnchor.G1);
                    double C = valueService.GetDoubleValue(newAnchor.C);

                    //A = 80;
                    //A1 = 40;
                    //B = 120;
                    //E = 80;
                    //F = 135;
                    //T = 25;
                    //T1 = 12;
                    //H = 250;
                    //I = 27;
                    //W = 110;
                    //P = 70;
                    //T2 = 12;
                    //C1 = 50;
                    //B1 = 50;
                    //H1 = 15;
                    //G1 = 15;
                    //C = 36;



                    double shellThickness = 0;
                    if (assemblyData.ShellOutput.Count > 0)
                    {
                        shellThickness = valueService.GetDoubleValue(assemblyData.ShellOutput[0].Thickness);
                    }

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
                    double padHeight = anchorHeight + B1;
                    double anchorCenterWidth = shellThickness + A;

                    double tankNominalID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);

                    List<Entity> newList = new List<Entity>();


                    Point3D refCenterPoint = GetSumPoint(refPoint, tankNominalID + shellThickness, 0);

                    Point3D PadLeftDown = GetSumPoint(refCenterPoint, 0, C1);
                    Point3D PadRightDown = GetSumPoint(refCenterPoint, shellThickness, C1);
                    Point3D PadLeftUp = GetSumPoint(refCenterPoint, 0, padHeight);
                    Point3D PadRightUp = GetSumPoint(refCenterPoint, shellThickness, padHeight);

                    Point3D TopPadLeftUp = GetSumPoint(PadRightUp, 0, -B1);
                    Point3D TopPadLeftDown = GetSumPoint(TopPadLeftUp, 0, -T);
                    Point3D TopPadRightUp = GetSumPoint(TopPadLeftUp, B, 0);
                    Point3D TopPadRightDown = GetSumPoint(TopPadLeftUp, B, -T);


                    Point3D TopSmallTriUpRight = GetSumPoint(TopPadLeftDown, H1, 0);
                    Point3D TopSmallTriDownLeft = GetSumPoint(TopPadLeftDown, 0, -H1);

                    Point3D TopSmallPadLeftUp = GetSumPoint(TopPadLeftUp, A - P / 2, T2);
                    Point3D TopSmallPadLeftDown = GetSumPoint(TopSmallPadLeftUp, 0, -T2);
                    Point3D TopSmallPadRightUp = GetSumPoint(TopSmallPadLeftUp, P, 0);
                    Point3D TopSmallPadDown = GetSumPoint(TopSmallPadRightUp, 0, -T2);

                    Point3D bigTriLeft = GetSumPoint(refCenterPoint, shellThickness + G1, 0);
                    Point3D bigTriRight = GetSumPoint(refCenterPoint, shellThickness + C, 0);

                    Point3D CenterTop = GetSumPoint(TopSmallPadLeftUp, P / 2, 0);
                    Point3D CenterDown = GetSumPoint(refCenterPoint, shellThickness + A, -bottomThickness);


                    newList.Add(new Line(PadLeftDown, PadRightDown));
                    newList.Add(new Line(PadLeftDown, PadLeftUp));
                    newList.Add(new Line(PadLeftUp, PadRightUp));
                    newList.Add(new Line(PadRightDown, PadRightUp));

                    newList.Add(new Line(TopPadLeftUp, TopPadRightUp));
                    newList.Add(new Line(TopPadRightDown, TopPadRightUp));
                    newList.Add(new Line(TopPadLeftDown, TopPadRightDown));

                    newList.Add(new Line(TopSmallPadLeftUp, TopSmallPadLeftDown));
                    newList.Add(new Line(TopSmallPadLeftUp, TopSmallPadRightUp));
                    newList.Add(new Line(TopSmallPadDown, TopSmallPadRightUp));

                    newList.Add(new Line(TopSmallTriUpRight, TopSmallTriDownLeft));


                    newList.Add(new Line(PadRightDown, bigTriLeft));
                    newList.Add(new Line(bigTriLeft, bigTriRight));
                    newList.Add(new Line(bigTriRight, TopPadRightDown));

                    styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);



                    // Center LIne
                    DrawCenterLineModel centerModel = new DrawCenterLineModel();
                    customLine.Add(new Line(CenterTop, CenterDown));
                    customLine.Add(new Line(CenterTop, GetSumPoint(CenterTop, 0, centerModel.exLength * scaleValue)));
                    Line centerbottomLine = new Line(CenterDown, GetSumPoint(CenterDown, 0, -centerModel.exLength * scaleValue));
                    centerbottomLine.EntityData = 1;
                    customLine.Add(centerbottomLine); 

                    styleService.SetLayerListEntity(ref customLine, layerService.LayerCenterLine);
                    
                    // Verical
                    newList.AddRange(customLine);


                    Point3D leaderPoint1 = (Point3D)centerbottomLine.EndPoint;
                    // Mirror
                    CDPoint mirroPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottom,0, ref refPoint, ref curPoint);

                    List<Entity> newEntity = new List<Entity>();
                    Plane pl1 = Plane.YZ;
                    pl1.Origin.X = mirroPoint.X;
                    pl1.Origin.Y = mirroPoint.Y;
                    Mirror customMirror = new Mirror(pl1);
                    foreach (Entity eachEntity in newList)
                    {
                        Entity newEachEntity = (Entity)eachEntity.Clone();
                        newEachEntity.TransformBy(customMirror);
                        newEntity.Add(newEachEntity);
                        if (eachEntity.EntityData != null)
                        {
                            leaderPoint1 = ((Line)newEachEntity).EndPoint;
                        }
                    }

                    customEntity.AddRange(newEntity);
                    customEntity.AddRange(newList);



                    // Dimension
                    Point3D leftDimPoint = GetSumPoint(centerbottomLine.EndPoint, 0, 0);
                    Point3D rightDimPoint = (Point3D)leftDimPoint.Clone();
                    rightDimPoint.TransformBy(customMirror);

                    SingletonData.DimPublicList.Add(new DimensionPointModel()
                    {
                        leftPoint = GetSumCDPoint(rightDimPoint, 0, 0),
                        rightPoint = GetSumCDPoint(leftDimPoint, 0, 0),
                        dimHeight = 4000,
                        Text = "B.C.D " + leftDimPoint.DistanceTo(rightDimPoint),
                        Position = "bottom"
                    });


                    // Leader
                    SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                    {
                        leaderPoint = GetSumCDPoint(leaderPoint1, 0, 0),
                        lineTextList = new List<string>() {assemblyData.AnchorageInput[0].AnchorQty+ "-" + assemblyData.AnchorageInput[0].AnchorSize+ " ANCHOR B/N/W" },

                        Position = "bottomleft"
                    });



                }
            }








            return customEntity.ToArray();
        }


        public Entity[] DrawBlock_Leader(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {

            int firstIndex = 0;

            double refLength = 15 * scaleValue;
            double refOverFit = 2;

            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[firstIndex].SizeTankHeight);
            double tankHeightHalf = tankHeight / 2;

            double tankNominalID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[firstIndex].SizeNominalID);
            double tankNominalIDHalf = tankNominalID / 2;
            //double roofSlopeDegree = valueService.GetDegreeOfSlope(assemblyData.RoofCRTInput[firstIndex].RoofSlope);

            double insulationThickness = valueService.GetDoubleValue(assemblyData.ShellInput[firstIndex].InsulationThickness);



            CDPoint leftTopPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftShell, tankHeightHalf + refLength / 2, ref refPoint, ref curPoint);
            CDPoint leftMiddlePoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftShell, tankHeightHalf, ref refPoint, ref curPoint);
            CDPoint leftBottomPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftShell, tankHeightHalf + refLength / 2, ref refPoint, ref curPoint);

            if (leftTopPoint.X != leftBottomPoint.X)
            {
                // 한번 위로 올리기
                leftTopPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftShell, tankHeightHalf + refLength / 2 + refLength, ref refPoint, ref curPoint);
                leftMiddlePoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftShell, tankHeightHalf + refLength, ref refPoint, ref curPoint);
                leftBottomPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftShell, tankHeightHalf + refLength / 2 + refLength, ref refPoint, ref curPoint);
            }

            Line line01 = new Line(GetSumPoint(leftTopPoint, 0, 0), GetSumPoint(leftTopPoint, -insulationThickness - refOverFit, 0));
            Line line03 = new Line(GetSumPoint(leftTopPoint, 0, -refLength), GetSumPoint(leftTopPoint, -insulationThickness - refOverFit, -refLength));
            Line line02 = new Line(GetSumPoint(leftTopPoint, -insulationThickness, 0), GetSumPoint(leftTopPoint, -insulationThickness, -refLength));


            List<Entity> newList = new List<Entity>();
            newList.Add(line01);
            newList.Add(line02);
            newList.Add(line03);

            styleService.SetLayerListLine(ref newList, layerService.LayerVirtualLine);


            return newList.ToArray();
        }


        public Entity[] DrawBlock_CenterLine(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {
            // 사용 안함

            List<Entity> newList = new List<Entity>();

            double bottomThickness = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateThickness);
            CDPoint basePoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottom, 0, ref refPoint, ref curPoint);
            CDPoint topPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterTopUp, 0, ref refPoint, ref curPoint);

            Line newCenter = new Line(GetSumPoint(basePoint, 0, -bottomThickness), GetSumPoint(topPoint, 0, 0));
            DrawCenterLineModel centerModel = new DrawCenterLineModel();

            Line newCenterUp = new Line(GetSumPoint(newCenter.EndPoint, 0, 0), GetSumPoint(newCenter.EndPoint, 0, centerModel.centerLength * scaleValue));
            Line newCenterDown = new Line(GetSumPoint(newCenter.StartPoint, 0, 0), GetSumPoint(newCenter.StartPoint, 0, -centerModel.centerLength*scaleValue));

            newList.Add(newCenter);
            newList.Add(newCenterUp);
            newList.Add(newCenterDown);

            styleService.SetLayerListEntity(ref newList, layerService.LayerCenterLine);



            return newList.ToArray();
        }


        public Entity[] DrawBlock_EtcBlock(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {
            // 사용 안함

            List<Entity> newList = new List<Entity>();

            double bottomThickness = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateThickness);
            CDPoint etcBasePoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottom, 0, ref refPoint, ref curPoint);

            string earthLugName = "EARTH_LUG";
            Entity earthLug = blockImportService.Draw_ImportBlock(GetSumCDPoint(etcBasePoint,0,0), earthLugName, layerService.LayerBlock);
            if (earthLug !=null)
                newList.Add(earthLug);



            return newList.ToArray();
        }

        public Entity[] DrawBlock_EtcText(CDPoint selPoint1, ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {
            // 사용 안함

            List<Entity> newList = new List<Entity>();





            return newList.ToArray();
        }

        public List<Entity> GetRiquidValue(Point3D selPoint, string selText,double scaleValue)
        {
            //string selText = "HLL 18100";
            double scale = scaleValue;
            double textHeight = 2.5;
            double scaleTextHeight = textHeight * scale;
            double middleLineWidth = 0;

            Point3D riqCenter = selPoint;

            List<Entity> newList = new List<Entity>();

            Point3D triP01 = GetSumPoint(riqCenter, 0, 0);
            Point3D triP02 = GetSumPoint(riqCenter, scaleTextHeight / 2, scaleTextHeight);
            Point3D triP03 = GetSumPoint(riqCenter, -scaleTextHeight / 2, scaleTextHeight);
            Line tri01 = new Line(GetSumPoint(triP01, 0, 0), GetSumPoint(triP02, 0, 0));
            Line tri02 = new Line(GetSumPoint(triP01, 0, 0), GetSumPoint(triP03, 0, 0));
            Line tri03 = new Line(GetSumPoint(triP03, 0, 0), GetSumPoint(triP02, 0, 0));


            Point3D textPoint = GetSumPoint(riqCenter, scaleTextHeight / 2 + scaleTextHeight / 2, 0);

            Text newText01 = new Text(GetSumPoint(textPoint, 0, scaleTextHeight * 0.2), selText, scaleTextHeight);
            newText01.Regen(new RegenParams(0, singleModel));
            newText01.Alignment = Text.alignmentType.BaselineLeft;
            styleService.SetLayer(ref newText01, layerService.LayerDimension);
            middleLineWidth = newText01.BoxSize.X;

            Line newCourseLine = new Line(GetSumPoint(riqCenter, -scaleTextHeight / 2, 0), GetSumPoint(textPoint, middleLineWidth + (middleLineWidth / 3) + scaleTextHeight / 2, 0));

            Point3D triqq01 = GetSumPoint(textPoint, 0, -(scaleTextHeight / 3));
            Point3D triqq02 = GetSumPoint(triqq01, (scaleTextHeight / 2), -(scaleTextHeight / 3));
            Point3D triqq03 = GetSumPoint(triqq02, (scaleTextHeight / 2), -(scaleTextHeight / 3));
            Line triiLine01 = new Line(GetSumPoint(triqq01, 0, 0), GetSumPoint(triqq01, scaleTextHeight * 3, 0));
            Line triiLine02 = new Line(GetSumPoint(triqq02, 0, 0), GetSumPoint(triqq02, scaleTextHeight * 2, 0));
            Line triiLine03 = new Line(GetSumPoint(triqq03, 0, 0), GetSumPoint(triqq03, scaleTextHeight * 1, 0));

            styleService.SetLayer(ref newText01, layerService.LayerDimension);

            newList.Add(newText01);
            newList.Add(tri01);
            newList.Add(tri02);
            newList.Add(tri03);
            newList.Add(triiLine01);
            newList.Add(triiLine02);
            newList.Add(triiLine03);
            newList.Add(newCourseLine);

            styleService.SetLayerListEntity(ref newList, layerService.LayerDimension);

            return newList;
        }



        public DrawEntityModel DrawBlock_CustomDimension(ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {

            int firstIndex = 0;


            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;
            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();

            string newPosition = "top";
            double newDimHeight = 100;
            double newTextHeight = -1;
            double newTextGap = 1;
            double newArrowSize = 2.5;

            string newPrefix = "";
            string newSuffix = "";
            string newText = "";

            string layerName = layerService.LayerDimension;

            DrawEntityModel returnEntity = new DrawEntityModel();

            List<Entity> dimLineList = new List<Entity>();
            List<Entity> dimTextList = new List<Entity>();
            List<Entity> dimLineExtList = new List<Entity>();
            List<Entity> dimArrowList = new List<Entity>();


            List<Dictionary<string, List<Entity>>> allDimensionList = new List<Dictionary<string, List<Entity>>>();


            // Basic Data
            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;




            // Shell Course

            double shellMaxThickness = 0;
            foreach (ShellOutputModel eachShell in assemblyData.ShellOutput)
            {
                double eachThk = valueService.GetDoubleValue(eachShell.Thickness);
                if (shellMaxThickness < eachThk)
                    shellMaxThickness = eachThk;
            }
            double beforeHeight = 0;
            double currentHeight = 0;
            double currentCourse = 0;

            double shellDistance = SingletonData.GAArea.Dimension.AreaSize.Width * scaleValue;
            double ShellCourseDistance = SingletonData.GAArea.Dimension.AreaSize.Width * scaleValue + SingletonData.GAArea.NozzleLeader.AreaSize.Width*scaleValue;


            double lastCourseDimGap = 10 * scaleValue;
            double scaleTextHeight = scaleService.GetOriginValueOfScale(scaleValue, newArrowSize);
            CDPoint shellCoursePoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointRightShellBottom, 0, ref refPoint, ref curPoint);
            foreach(ShellOutputModel eachShell in assemblyData.ShellOutput)
            {
                currentCourse++;
                double courseHeight = valueService.GetDoubleValue(eachShell.PlateWidth);
                currentHeight = beforeHeight + courseHeight;
                newPoint1 = GetSumCDPoint(shellCoursePoint, shellMaxThickness, beforeHeight);
                newPoint2 = GetSumCDPoint(shellCoursePoint, shellMaxThickness, currentHeight);
                newPoint3 = GetSumCDPoint(shellCoursePoint, shellDistance - lastCourseDimGap, beforeHeight + currentHeight / 2);
                newPosition = "right";
                newDimHeight = shellDistance - lastCourseDimGap;

                returnEntity.AddDrawEntity(drawService.Draw_Dimension(newPoint1, newPoint2, newPoint3, newPosition, newDimHeight, newTextHeight, newTextGap, newArrowSize, newPrefix, newSuffix, newText, 0, scaleValue, layerName));

                // text
                double middleLineWidth = 0;
                Point3D newCoursePoint = GetSumPoint(shellCoursePoint, ShellCourseDistance, beforeHeight + courseHeight / 2);
                string courseString1 = valueService.GetOrdinalNumber(currentCourse).ToUpper() + " COURSE " + "t" + eachShell.Thickness;
                Text newText01 = new Text(GetSumPoint(newCoursePoint,0,scaleTextHeight*0.6), courseString1, scaleTextHeight);
                newText01.Regen(new RegenParams(0, singleModel));
                newText01.Alignment = Text.alignmentType.BaselineLeft;
                newText01.ColorMethod = colorMethodType.byEntity;
                newText01.Color = Color.Yellow;
                newText01.LayerName = layerService.LayerDimension;
                middleLineWidth = newText01.BoxSize.X;

                string courseString2 = "(MAT'L : " + eachShell.Material +")";
                Text newText02 = new Text(GetSumPoint( newCoursePoint,0, -scaleTextHeight*0.6 ), courseString2, scaleTextHeight);
                newText02.Regen(new RegenParams(0, singleModel));
                newText02.Alignment = Text.alignmentType.TopLeft;
                newText02.ColorMethod = colorMethodType.byEntity;
                newText02.Color = Color.Yellow;
                newText02.LayerName = layerService.LayerDimension;

                if (newText02.BoxSize.X > middleLineWidth)
                    middleLineWidth = newText02.BoxSize.X;
                Line newCourseLine = new Line(GetSumPoint(newCoursePoint, -scaleTextHeight / 2, 0), GetSumPoint(newCoursePoint, middleLineWidth + scaleTextHeight / 2, 0));
                styleService.SetLayer(ref newCourseLine, layerService.LayerDimension);
                newCourseLine.ColorMethod = colorMethodType.byEntity;
                newCourseLine.Color = Color.Yellow;

                dimTextList.Add(newText01);
                dimTextList.Add(newText02);
                dimLineList.Add(newCourseLine);

                //current Height
                beforeHeight += courseHeight;

            }

            double lastDimGap = 4 * scaleValue;

            // ID
            string idString = "TANK I.D " + assemblyData.GeneralDesignData[0].SizeNominalID;
            returnEntity.AddDrawEntity( drawService.Draw_Dimension(
                GetSumCDPoint(refPoint,0,tankHeight/2), 
                GetSumCDPoint(refPoint, tankID, tankHeight/2), null, "top", 0, newTextHeight, newTextGap, newArrowSize, newPrefix, newSuffix, idString, 0, scaleValue, layerName));

            // Thank Height
            string heightString = "TANK HEIGHT " + assemblyData.GeneralDesignData[0].SizeTankHeight;
            returnEntity.AddDrawEntity( drawService.Draw_Dimension(
                GetSumCDPoint(refPoint, tankID + shellMaxThickness, 0),
                GetSumCDPoint(refPoint, tankID + shellMaxThickness, tankHeight ),
                null, "right", shellDistance - lastDimGap, newTextHeight, newTextGap, newArrowSize, newPrefix, newSuffix, heightString, 0, scaleValue, layerName));

            // Winder Girder
            if (assemblyData.WindGirderInput[0].WindGirderRequired.ToLower() == "yes")
            {
                double currentElevation = 0;
                foreach (WindGirderOutputModel eachWind in assemblyData.WindGirderOutput)
                {
                    double eachElevation = valueService.GetDoubleValue(eachWind.Elevation);
                    returnEntity.AddDrawEntity(drawService.Draw_Dimension(
                        GetSumCDPoint(refPoint, - shellMaxThickness, currentElevation),
                        GetSumCDPoint(refPoint, - shellMaxThickness, eachElevation), null, "left", shellDistance - lastDimGap, newTextHeight, newTextGap, newArrowSize, newPrefix, newSuffix, "", 0, scaleValue, layerName));
                    currentElevation = eachElevation;
                }
            }



            // Base Elevation
            //double baseElevationHeight = scaleValue * 1;
            //double 






            // public Logic
            foreach(DimensionPointModel eachDim in SingletonData.DimPublicList)
            {
                returnEntity.AddDrawEntity(drawService.Draw_Dimension(
                        eachDim.leftPoint,
                        eachDim.rightPoint, null, eachDim.Position, eachDim.dimHeight, newTextHeight, newTextGap, newArrowSize, newPrefix, newSuffix, eachDim.Text, 0, scaleValue, layerName,eachDim.leftArrowVisible,eachDim.rightArrowVisible,eachDim.extVisible, eachDim.middleValue));
            }

            return returnEntity;


        }



        public Dictionary<string, List<Entity>> DrawBlock_EtraDimension(ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {

  

            string layerName = layerService.LayerDimension;

            Dictionary<string, List<Entity>> returnEntity = new Dictionary<string, List<Entity>>();

            List<Entity> dimLineList = new List<Entity>();
            List<Entity> dimTextList = new List<Entity>();

            List<Entity> newList = new List<Entity>();

            newList.AddRange(GetRiquidData(ref refPoint, ref curPoint, scaleValue));

            newList.AddRange(GetBottomSlope(ref refPoint, ref curPoint, scaleValue));

            switch (SingletonData.TankType)
            {
                case TANK_TYPE.CRT:
                case TANK_TYPE.IFRT:
                    newList.AddRange(GetRoofSlope(ref refPoint, ref curPoint, scaleValue));
                    break;
            }



            // Entity Div
            foreach (Entity eachEntity in newList)
            {
                if(eachEntity is Text)
                {
                    dimTextList.Add(eachEntity);
                }
                else
                {
                    dimLineList.Add(eachEntity);
                }
            }



            returnEntity.Add(CommonGlobal.DimLine, dimLineList);
            returnEntity.Add(CommonGlobal.DimText, dimTextList);


            return returnEntity;


        }

        public List<Entity> GetRiquidData(ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {

            List<Entity> newList = new List<Entity>();

            double bottomThickness = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateThickness);
            CDPoint etcBasePoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottom, 0, ref refPoint, ref curPoint);

            Point3D eachPoint = new Point3D();

            // HLL
            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;
            double tankIDHalfHalf = tankIDHalf / 2;
            double tankIDHalfThree = (tankIDHalfHalf) / 3;
            double leftOne = tankIDHalfHalf + tankIDHalfThree;
            double leftTwo = tankIDHalfHalf + tankIDHalfThree + tankIDHalfThree;

            double currentLeft = leftTwo;

            string HHLLValue = assemblyData.GeneralLiquidCapacityWeight[0].HighHIghLiquidLevel.Trim();
            string HLLValue = assemblyData.GeneralLiquidCapacityWeight[0].HighLiquidLevel.Trim();
            string LLLValue = assemblyData.GeneralLiquidCapacityWeight[0].LowLiquidLevel.Trim();
            string LLLLValue = assemblyData.GeneralLiquidCapacityWeight[0].LowLowLiquidLevel.Trim();
            double HHLLNumber = valueService.GetDoubleValue(HHLLValue);
            double HLLNumber = valueService.GetDoubleValue(HLLValue);
            double LLLNumber = valueService.GetDoubleValue(LLLValue);
            double LLLLNumber = valueService.GetDoubleValue(LLLLValue);

            string HHLL = "HHLL " + HHLLValue;
            string HLL = "HLL " + HLLValue;
            string LLL = "LLL " + LLLValue;
            string LLLL = "LLLL " + LLLLValue;

            //
            if (HHLLValue != "")
            {
                if (currentLeft == leftTwo)
                    currentLeft = leftOne;
                else if (currentLeft == leftOne)
                    currentLeft = leftTwo;

                eachPoint = GetSumPoint(refPoint, currentLeft, HHLLNumber);
                newList.AddRange(GetRiquidValue(GetSumPoint(eachPoint, 0, 0), HHLL, scaleValue));
            }

            if (HLLValue != "")
            {
                if (currentLeft == leftTwo)
                    currentLeft = leftOne;
                else if (currentLeft == leftOne)
                    currentLeft = leftTwo;

                eachPoint = GetSumPoint(refPoint, currentLeft, HLLNumber);
                newList.AddRange(GetRiquidValue(GetSumPoint(eachPoint, 0, 0), HLL, scaleValue));
            }

            if (LLLValue != "")
            {
                if (currentLeft == leftTwo)
                    currentLeft = leftOne;
                else if (currentLeft == leftOne)
                    currentLeft = leftTwo;

                eachPoint = GetSumPoint(refPoint, currentLeft, LLLNumber);
                newList.AddRange(GetRiquidValue(GetSumPoint(eachPoint, 0, 0), LLL, scaleValue));
            }

            if (LLLLValue != "")
            {
                if (currentLeft == leftTwo)
                    currentLeft = leftOne;
                else if (currentLeft == leftOne)
                    currentLeft = leftTwo;

                eachPoint = GetSumPoint(refPoint, currentLeft, LLLLNumber);
                newList.AddRange(GetRiquidValue(GetSumPoint(eachPoint, 0, 0), LLLL, scaleValue));
            }

            return newList;
        }

        public List<Entity> GetBottomSlope(ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {
            string selText1 = "12";
            string selText2 = "1";
            double scale = scaleValue;
            double textHeight = 2.5;
            double scaleTextHeight = textHeight * scale;
            double middleLineWidth = 0;

            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;
            double tankIDHalfHalf = tankIDHalf / 2;
            double tankIDHalfThree = (tankIDHalfHalf) / 3;
            double leftOne = tankIDHalfHalf + tankIDHalfThree;
            double leftTwo = tankIDHalfHalf + tankIDHalfThree + tankIDHalfThree;


            string bottomValue = assemblyData.BottomInput[0].BottomPlateSlope;
            if (bottomValue.Contains("/"))
            {
                string[] splitValue=bottomValue.Split(new char[] { '/' });
                selText1 = splitValue[1];
            }
            else
            {
                selText1 = bottomValue.Trim();
            }


            Point3D refCenter = GetSumPoint(refPoint, leftTwo , -tankIDHalfThree / 2);

            List<Entity> newList = new List<Entity>();

            Point3D triP01 = GetSumPoint(refCenter, 0, 0);
            Point3D triP02 = GetSumPoint(triP01, scaleTextHeight * 2 + scaleTextHeight * 3 + scaleTextHeight * 2, 0);
            Point3D triP03 = GetSumPoint(triP02, 0, scaleTextHeight);
            Line tri01 = new Line(GetSumPoint(triP01, 0, 0), GetSumPoint(triP02, 0, 0));
            Line tri02 = new Line(GetSumPoint(triP02, 0, 0), GetSumPoint(triP03, 0, 0));
            Line tri03 = new Line(GetSumPoint(triP03, 0, 0), GetSumPoint(triP01, 0, 0));


            Text newText01 = new Text(GetSumPoint(tri01.MidPoint, 0, -scaleTextHeight * 0.2), selText1, scaleTextHeight);
            newText01.Alignment = Text.alignmentType.TopCenter;
            styleService.SetLayer(ref newText01, layerService.LayerDimension);

            Text newText02 = new Text(GetSumPoint(tri02.MidPoint, +scaleTextHeight * 0.2, 0), selText2, scaleTextHeight);
            newText02.Alignment = Text.alignmentType.MiddleLeft;
            styleService.SetLayer(ref newText02, layerService.LayerDimension);



            newList.Add(tri01);
            newList.Add(tri02);
            newList.Add(tri03);
            styleService.SetLayerListEntity(ref newList, layerService.LayerDimension);

            newList.Add(newText01);
            newList.Add(newText02);

            return newList;
        }

        public List<Entity> GetRoofSlope(ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {
            string selText1 = "12";
            string selText2 = "1";
            double scale = scaleValue;
            double textHeight = 2.5;
            double scaleTextHeight = textHeight * scale;
            double middleLineWidth = 0;

            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;
            double tankIDHalfHalf = tankIDHalf / 2;
            double tankIDHalfThree = (tankIDHalfHalf) / 3;
            double leftOne = tankIDHalfHalf + tankIDHalfThree;
            double leftTwo = tankIDHalfHalf + tankIDHalfThree + tankIDHalfThree;


            string bottomValue = assemblyData.RoofCompressionRing[0].RoofSlope;
            if (bottomValue.Contains("/"))
            {
                string[] splitValue = bottomValue.Split(new char[] { '/' });
                selText1 = splitValue[1];
            }
            else
            {
                selText1 = bottomValue.Trim();
            }

            CDPoint etcBasePoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterTopUp, 0, ref refPoint, ref curPoint);


            Point3D refCenter = GetSumPoint(etcBasePoint,tankIDHalfThree/3, 500);

            List<Entity> newList = new List<Entity>();

            Point3D triP01 = GetSumPoint(refCenter, 0, 0);
            Point3D triP02 = GetSumPoint(triP01, scaleTextHeight * 2 + scaleTextHeight * 3 + scaleTextHeight * 2, 0);
            Point3D triP03 = GetSumPoint(triP02, 0, -scaleTextHeight);
            Line tri01 = new Line(GetSumPoint(triP01, 0, 0), GetSumPoint(triP02, 0, 0));
            Line tri02 = new Line(GetSumPoint(triP02, 0, 0), GetSumPoint(triP03, 0, 0));
            Line tri03 = new Line(GetSumPoint(triP03, 0, 0), GetSumPoint(triP01, 0, 0));


            Text newText01 = new Text(GetSumPoint(tri01.MidPoint, 0, scaleTextHeight * 0.2), selText1, scaleTextHeight);
            newText01.Alignment = Text.alignmentType.BottomCenter;
            styleService.SetLayer(ref newText01, layerService.LayerDimension);

            Text newText02 = new Text(GetSumPoint(tri02.MidPoint, +scaleTextHeight * 0.2, 0), selText2, scaleTextHeight);
            newText02.Alignment = Text.alignmentType.MiddleLeft;
            styleService.SetLayer(ref newText02, layerService.LayerDimension);


            newList.Add(tri01);
            newList.Add(tri02);
            newList.Add(tri03);
            styleService.SetLayerListEntity(ref newList, layerService.LayerDimension);


            newList.Add(newText01);
            newList.Add(newText02);

            return newList;
        }



        private ChannelModel GetChannel(string channelSize)
        {
            ChannelModel returnValue = null;
            foreach (ChannelModel eachChannel in assemblyData.ChannelList)
            {
                if (eachChannel.SIZE == channelSize) 
                {
                    returnValue = eachChannel;
                    break;
                }
            }
            return returnValue;
        }


        public List<Entity> DrawReference_Hole(Point3D selPoint1, double selRadius, double selRotate = 0, DrawCenterLineModel selCenterLine = null)
        {



            List<Entity> newList = new List<Entity>();

            // WP : Left Lower
            Point3D WP = new Point3D(selPoint1.X, selPoint1.Y);
            Point3D WPRotate = WP;


            // Drawing Shape
            Circle newCir01 = new Circle(GetSumPoint(WP, 0, 0), selRadius);

            newList.Add(newCir01);
            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            // Center Line
            if (selCenterLine != null)
            {
                if (selCenterLine.centerLine)
                {
                    // 기본 형상
                    Point3D centerPoint = newCir01.Center;
                    Line centerLine01 = new Line(GetSumPoint(centerPoint, -selRadius - selCenterLine.exLength * selCenterLine.scaleValue, 0),
                                                 GetSumPoint(centerPoint, +selRadius + selCenterLine.exLength * selCenterLine.scaleValue, 0));
                    Line centerLine02 = new Line(GetSumPoint(centerPoint, 0, -selRadius - selCenterLine.exLength * selCenterLine.scaleValue),
                                                 GetSumPoint(centerPoint, 0, +selRadius + selCenterLine.exLength * selCenterLine.scaleValue));

                    styleService.SetLayer(ref centerLine01, layerService.LayerCenterLine);
                    styleService.SetLayer(ref centerLine02, layerService.LayerCenterLine);


                    newList.Add(centerLine01);
                    newList.Add(centerLine02);

                }
            }

            // Rotate
            if (selRotate != 0)
            {
                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }

            return newList;
        }




        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
        private Point3D GetSumPoint(CDPoint selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
        private CDPoint GetSumCDPoint(CDPoint selPoint1, double X, double Y, double Z = 0)
        {
            return new CDPoint(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
        private CDPoint GetSumCDPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new CDPoint(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }




    }
}

