using AssemblyLib.AssemblyModels;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawSettingLib.Commons;
using DrawSettingLib.SettingModels;
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

    #region Enum
    public enum RECTANGLEALIGN { NONE, CENTER, LEFT, RIGHT, TOP, BOTTOM, }
    public enum RECTANGLUNVIEW { NONE, LEFT, RIGHT, TOP, BOTTOM, }
    public enum PLATEPOSITION { ZERO, START, MIDDLE, ADD, END, }
    public enum TODOLASTPLATE { NOTTING, SHIFT, ADD, }

    public enum PlateOrientation { }


    public enum PLATEPLACE { TR, TL, BR, BL, HCENTER, VCENTER, LCENTER, RCENTER, TCENTER, BCENTER }
    public enum PLATESHAPE { EMPTY, RECTANGLE, RECTANGLEARC, ARC, WHOLEPLATE, HALFCIRCLE, TWINARC, } // WholePlate : whole , Complete Plate


    #endregion




    public class DrawDetailTempService
    {


        private Model singleModel;
        private AssemblyModel assemblyData;

        private DrawService drawService;

        private ValueService valueService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;

        private DrawShellCourses dsService;
        private DrawShapeServices shapeService;
        private DrawEditingService editingService;


        private AssemblyModelService modelService;
        private DrawShapeCalculationService shapeCalService;

        private DrawPublicFunctionService publicFunctionService;
        public DrawScaleService scaleService { get; set; }

        public DrawBreakSymbols breakService { get; set; }
        public DrawDetailTempService(AssemblyModel selAssembly, object selModel)
        {
            singleModel = selModel as Model;

            assemblyData = selAssembly;

            drawService = new DrawService(selAssembly);
            modelService = new AssemblyModelService(selAssembly);


            valueService = new ValueService();
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();

            dsService = new DrawShellCourses();
            shapeService = new DrawShapeServices();
            editingService = new DrawEditingService();
            shapeCalService = new DrawShapeCalculationService();
            scaleService = new DrawScaleService();
            breakService = new DrawBreakSymbols();
            publicFunctionService = new DrawPublicFunctionService();
        }



        public struct AnswerLastPlateCheck
        {
            public TODOLASTPLATE todoLastPlate;
            public double LastPlateArrayNum;
            public double EndPointX;   //( 0 ----> plate total length)
            public double LastPlateEndPointX;
            public double LastplateLength;

            public void AnswerLastPlateCheckf()
            {
                todoLastPlate = TODOLASTPLATE.NOTTING;
                LastPlateArrayNum = EndPointX = LastPlateEndPointX = 0;
            }
        }

        public struct ArrangePlate
        {
            public bool isSuccess;
            public int countPlate;
            public PLATESHAPE remainderShape;
            public List<double> plateNumberList;
            public List<Entity> drawEntityList;
            public List<Entity> drawEntityDimensionList;


            public double remainderMidLength;



            public void ArrangePlateInit()
            {
                isSuccess = false;
                countPlate = 0;
                remainderShape = PLATESHAPE.EMPTY;
                plateNumberList = new List<double>();
                drawEntityList = new List<Entity>();
                drawEntityDimensionList = new List<Entity>();

                remainderMidLength = 0;
            }
        }

        public struct SamePlateInfo
        {
            public int countPlate;
            public double plateNumber;
            public double bottomLength;



            public tempPlateInfo tempPlateInfo;



            public void SamePlateInfoInit()
            {
                plateNumber = 0;
                countPlate = 0;
                tempPlateInfo = new tempPlateInfo();
            }
        }


        public DrawEntityModel DrawSection_D_D(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {

            DrawEntityModel drawList = new DrawEntityModel();

            // Section DD

            //singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Entity> SPLineList = new List<Entity>();
            List<Entity> splineList = new List<Entity>();
            List<Entity> angleLineList = new List<Entity>();
            List<Entity> topViewHiddenLineList = new List<Entity>();

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);


            double distanceYofRef = 45;  // Start Spline Center Point of AngleTopLine : Angle Top 에서의 Y축으로

            double shellThk = 2;
            double shellHeight = 15;
            double shellIR = 628; // IR : InnerRadius
            double shellOR = shellIR + shellThk; // OR : outerRadius
            double shellDrawAngle = 1.5;  // GuideLine Angle

            double angleLength = 20;
            double angleInnerRadius = 2.5;
            double angleouterRadius = 1.5;
            double angleThk = 2;
            double angleOR = shellOR + angleLength;

            double seamRadius = 2;
            double splineDistance = angleLength * 0.05;
            double overVirtualLine = 3;


            Point3D angleStartPoint = GetSumPoint(referencePoint, 0, 0);


            // Draw : Angle
            List<Line> bottomAngleLineList = GetRectangle(angleStartPoint, angleLength, angleThk, RECTANGLUNVIEW.TOP);
            Line bottomAngleBottom = bottomAngleLineList[0];
            Line bottomAngleRight = bottomAngleLineList[1];
            Line bottomAngleLeft = bottomAngleLineList[2];

            List<Line> topAngleLineList = GetRectangle(GetSumPoint(angleStartPoint, 0, angleLength - angleThk), angleThk, angleLength, RECTANGLUNVIEW.LEFT);
            Line topAngleBottom = topAngleLineList[0];
            Line topAngleRight = topAngleLineList[1];
            Line topAngleTop = topAngleLineList[2];

            // Draw : Fillet Angle
            if (Curve.Fillet(topAngleBottom, topAngleRight, angleouterRadius, true, false, true, true, out Arc arcFilletRight))
                angleLineList.Add(arcFilletRight);
            if (Curve.Fillet(topAngleBottom, bottomAngleRight, angleInnerRadius, false, true, true, true, out Arc arcFilletCenter))
                angleLineList.Add(arcFilletCenter);
            if (Curve.Fillet(bottomAngleBottom, bottomAngleRight, angleouterRadius, true, false, true, true, out Arc arcFilletBottom))
                angleLineList.Add(arcFilletBottom);

            // Draw : Shell with Angle
            Point3D angleShellStartPoint = GetSumPoint(angleStartPoint, angleLength, (angleLength - shellHeight));
            Line angleShellLeft = new Line(GetSumPoint(angleShellStartPoint, 0, 0), GetSumPoint(angleShellStartPoint, 0, angleLength * 1.2));
            Line angleShellRight = new Line(GetSumPoint(angleShellStartPoint, shellThk, 0), GetSumPoint(angleShellStartPoint, shellThk, angleLength * 1.2));



            angleLineList.AddRange(new Entity[] {
                    bottomAngleBottom, bottomAngleRight, bottomAngleLeft,
                    topAngleBottom, topAngleRight, topAngleTop,
                    angleShellLeft, angleShellRight,
            });



            ///////////////////////////////////////
            // Draw Top View  : Spline

            // Setting : Start Point
            Point3D SplineShellCenterPoint = GetSumPoint(angleShellStartPoint, 0, shellHeight + distanceYofRef);
            Point3D circleCenterPoint = GetSumPoint(SplineShellCenterPoint, shellOR, 0);

            // Draw : Shell with Spline  ( Arc )
            Circle circleInnerShell = new Circle(circleCenterPoint, shellIR);
            Circle circleouterShell = new Circle(circleCenterPoint, shellOR);
            Circle circleInnerAngle = new Circle(circleCenterPoint, shellIR + angleLength);
            Circle circleouterAngle = new Circle(circleCenterPoint, shellOR + angleLength);


            // Guide Line (for Intersect)
            double guideLineLength = shellOR + (angleLength * 2);
            Line guideTopLine = new Line(circleCenterPoint, GetSumPoint(circleCenterPoint, -guideLineLength, 0));
            Line guideBottomLine = new Line(circleCenterPoint, GetSumPoint(circleCenterPoint, -guideLineLength, 0));

            //  1.5 degree rotate
            guideTopLine.Rotate(Utility.DegToRad(-shellDrawAngle), Vector3D.AxisZ, circleCenterPoint);
            guideBottomLine.Rotate(Utility.DegToRad(shellDrawAngle), Vector3D.AxisZ, circleCenterPoint);

            Point3D[] intersectAngleTopOut = guideTopLine.IntersectWith(circleouterAngle);
            Point3D[] intersectAngleBottomOut = guideBottomLine.IntersectWith(circleouterAngle);

            //Point3D[] intersectShellTopIn = guideTopLine.IntersectWith(circleInnerShell);
            Point3D[] intersectShellTopOut = guideTopLine.IntersectWith(circleouterShell);
            //Point3D[] intersectShellBottomIn = guideBottomLine.IntersectWith(circleInnerShell);
            Point3D[] intersectShellBottomOut = guideBottomLine.IntersectWith(circleouterShell);

            // Get Guide Line (For Spline)
            Line guideSTopLine = new Line(GetSumPoint(intersectAngleTopOut[0], 0, 0), GetSumPoint(intersectShellTopOut[0], 0, 0));
            Line guideSBottomLine = new Line(GetSumPoint(intersectAngleBottomOut[0], 0, 0), GetSumPoint(intersectShellBottomOut[0], 0, 0));

            // Draw Arc of Angle OutSide
            Arc arcAngleOut = new Arc(intersectAngleTopOut[0], circleCenterPoint, intersectAngleBottomOut[0], true);

            //  +1 degree rotate  ( Length : angleArc < shellArc )
            guideTopLine.Rotate(Utility.DegToRad(-1), Vector3D.AxisZ, circleCenterPoint);
            guideBottomLine.Rotate(Utility.DegToRad(1), Vector3D.AxisZ, circleCenterPoint);

            Point3D[] intersectAngleTopIn = guideTopLine.IntersectWith(circleInnerAngle);
            Point3D[] intersectAngleBottomIn = guideBottomLine.IntersectWith(circleInnerAngle);
            Point3D[] intersectShellTopIn = guideTopLine.IntersectWith(circleInnerShell);
            Point3D[] intersectShellBottomIn = guideBottomLine.IntersectWith(circleInnerShell);
            intersectShellTopOut = guideTopLine.IntersectWith(circleouterShell);
            intersectShellBottomOut = guideBottomLine.IntersectWith(circleouterShell);

            // Draw Right Arc (of Shell)
            Arc arcShellIn = new Arc(intersectShellTopIn[0], circleCenterPoint, intersectShellBottomIn[0], true);
            Arc arcShellOut = new Arc(intersectShellTopOut[0], circleCenterPoint, intersectShellBottomOut[0], true);
            // Guide : Arc of Angle Inside (for intersect with Spline)
            Arc arcAngleInGuide = new Arc(intersectAngleTopIn[0], circleCenterPoint, intersectAngleBottomIn[0], true);


            // Center Line (arcAngleOut.midPoint)
            Line guideSCenterLine = new Line(GetSumPoint(circleCenterPoint, -shellIR + overVirtualLine, 0),
                                             GetSumPoint(circleCenterPoint, -(shellOR + angleLength + overVirtualLine), 0));
            centerLineList.Add(guideSCenterLine);


            // Draw : seamLine and Seam
            Line seamLine = new Line(GetSumPoint(SplineShellCenterPoint, 0, 0), GetSumPoint(SplineShellCenterPoint, 0, 0));
            Arc arcSeamGuide = new Arc(SplineShellCenterPoint, seamRadius, Utility.DegToRad(90), Utility.DegToRad(270));
            Point3D[] arcSeamSidePoint = arcSeamGuide.IntersectWith(arcShellOut);

            // todo : Function()    sort Point
            if (arcSeamSidePoint[0].Y < arcSeamSidePoint[1].Y)
            {
                Point3D tempP = new Point3D(0, 0);
                tempP = GetSumPoint(arcSeamSidePoint[0], 0, 0);

                arcSeamSidePoint[0] = GetSumPoint(arcSeamSidePoint[1], 0, 0);
                arcSeamSidePoint[1] = GetSumPoint(tempP, 0, 0);
            }

            Arc arcSeam = new Arc(arcSeamSidePoint[0], arcSeamGuide.MidPoint, arcSeamSidePoint[1], false);



            // Draw : Bottom Spline 
            List<Point3D> splineBottomPointList = new List<Point3D>();

            // Get Mid Line
            Line bSplineLeftLine = new Line(GetSumPoint(guideSBottomLine.StartPoint, 0, 0), GetSumPoint(guideSBottomLine.MidPoint, 0, 0));
            Line bSplineRightLine = new Line(GetSumPoint(guideSBottomLine.MidPoint, 0, 0), GetSumPoint(guideSBottomLine.EndPoint, 0, 0));
            Line bSplineMidLine = new Line(GetSumPoint(bSplineLeftLine.MidPoint, 0, 0), GetSumPoint(bSplineRightLine.MidPoint, 0, 0));
            Line splineBottomMidLine = (Line)bSplineMidLine.Offset(splineDistance, Vector3D.AxisZ);

            // Add Spline Point
            splineBottomPointList.Add(GetSumPoint(guideSBottomLine.StartPoint, 0, 0));
            splineBottomPointList.Add(GetSumPoint(splineBottomMidLine.StartPoint, 0, 0));
            splineBottomPointList.Add(GetSumPoint(guideSBottomLine.MidPoint, 0, 0));
            splineBottomPointList.Add(GetSumPoint(splineBottomMidLine.EndPoint, 0, 0));
            splineBottomPointList.Add(GetSumPoint(guideSBottomLine.EndPoint, 0, 0));

            Curve bottomSpline = Curve.CubicSplineInterpolation(splineBottomPointList);
            SPLineList.Add(bottomSpline);



            // Draw : top Spline
            List<Point3D> splineTopPointList = new List<Point3D>();

            // Get Mid Line
            Line tSplineLeftLine = new Line(GetSumPoint(guideSTopLine.StartPoint, 0, 0), GetSumPoint(guideSTopLine.MidPoint, 0, 0));
            Line tSplineRightLine = new Line(GetSumPoint(guideSTopLine.MidPoint, 0, 0), GetSumPoint(guideSTopLine.EndPoint, 0, 0));
            Line tSplineMidLine = new Line(GetSumPoint(tSplineLeftLine.MidPoint, 0, 0), GetSumPoint(tSplineRightLine.MidPoint, 0, 0));
            Line splineTopMidLine = (Line)tSplineMidLine.Offset(-splineDistance, Vector3D.AxisZ);

            // Add Spline Point
            splineTopPointList.Add(GetSumPoint(guideSTopLine.StartPoint, 0, 0));
            splineTopPointList.Add(GetSumPoint(splineTopMidLine.StartPoint, 0, 0));
            splineTopPointList.Add(GetSumPoint(guideSTopLine.MidPoint, 0, 0));
            splineTopPointList.Add(GetSumPoint(splineTopMidLine.EndPoint, 0, 0));
            splineTopPointList.Add(GetSumPoint(guideSTopLine.EndPoint, 0, 0));

            Curve topSpline = Curve.CubicSplineInterpolation(splineTopPointList);
            SPLineList.Add(topSpline);


            // Draw : innerArc of angle ( intersect with Spline )
            Point3D[] arcAngleTopPoint = arcAngleInGuide.IntersectWith(topSpline);
            Point3D[] arcAngleBottomPoint = arcAngleInGuide.IntersectWith(bottomSpline);
            // dot Line : arcAngleInside
            Arc arcAngleInside = new Arc(arcAngleTopPoint[0], arcAngleInGuide.MidPoint, arcAngleBottomPoint[0], false);
            topViewHiddenLineList.Add(arcAngleInside);


            // Draw Add all
            splineList.AddRange(new Entity[] {
                    arcShellIn, arcShellOut, arcAngleOut,
                    seamLine, arcSeam,
            });

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            //styleService.SetLayerListEntity(ref SPLineList, layerService.LayerDimension);
            //styleService.SetLayerListEntity(ref angleLineList, layerService.LayerOutLine);
            //styleService.SetLayerListEntity(ref splineList, layerService.LayerOutLine);
            //styleService.SetLayerListEntity(ref topViewHiddenLineList, layerService.LayerHiddenLine);

            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(SPLineList);
            //singleModel.Entities.AddRange(splineList);
            //singleModel.Entities.AddRange(angleLineList);
            //singleModel.Entities.AddRange(topViewHiddenLineList);

            //singleModel.Entities.Regen();
            //singleModel.Invalidate();

            //singleModel.SetView(viewType.Top);
            //singleModel.ZoomFit();
            //singleModel.Refresh();


            // Center Point

            Point3D modelCenterPoint = GetSumPoint(SplineShellCenterPoint, -angleLength/2,-20);
            SetModelCenterPoint(PAPERSUB_TYPE.SectionDD, modelCenterPoint);



            styleService.SetLayerListEntity(ref SPLineList, layerService.LayerDimension);
            styleService.SetLayerListEntity(ref angleLineList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref splineList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref topViewHiddenLineList, layerService.LayerHiddenLine);

            drawList.outlineList.AddRange(SPLineList);
            drawList.outlineList.AddRange(angleLineList);
            drawList.outlineList.AddRange(splineList);
            drawList.outlineList.AddRange(topViewHiddenLineList);


            // Dimension
            string dimInnerRadius = "R30";
            DrawBMLeaderModel dimLeadder1 = new DrawBMLeaderModel() { position = POSITION_TYPE.LEFT, upperText = dimInnerRadius, leaderLength = 10 };
            DrawEntityModel testDimArcLeader01 = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(arcSeamGuide.MidPoint,0, 1), dimLeadder1, scaleValue, Utility.DegToRad(-45));
            drawList.AddDrawEntity(testDimArcLeader01);


            string leaderCourseInfoD = "VERTICAL";
            string leaderCourseInfoC = "JOINT";
            DrawBMLeaderModel leaderInfoModel = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = leaderCourseInfoD, lowerText = leaderCourseInfoC, bmNumber = "", textAlign = POSITION_TYPE.CENTER,arrowHeadVisible=false };
            DrawEntityModel leaderInfoList = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(arcShellIn.MidPoint,3,0), leaderInfoModel, scaleValue);
            drawList.AddDrawEntity(leaderInfoList);

            // Center Line
            // Center Line
            List<Entity> centerLineNewList = new List<Entity>();
            DrawCenterLineModel newCenterModel = new DrawCenterLineModel();
            centerLineNewList.AddRange(editingService.GetCenterLine(GetSumPoint(arcAngleOut.MidPoint, 0, 0), GetSumPoint(arcShellIn.MidPoint, 0, 0), newCenterModel.detailCenterLength, scaleValue));
            styleService.SetLayerListEntity(ref centerLineNewList, layerService.LayerCenterLine);
            drawList.outlineList.AddRange(centerLineNewList);

            // Welding
            List<Entity> weldingList = new List<Entity>();
            DrawWeldSymbols weldSymbols = new DrawWeldSymbols();
            DrawWeldSymbolModel wsModel02 = new DrawWeldSymbolModel();
            wsModel02.position = ORIENTATION_TYPE.TOPLEFT;
            wsModel02.weldTypeUp = WeldSymbol_Type.Fillet;
            wsModel02.weldTypeDown = WeldSymbol_Type.Fillet;
            wsModel02.weldDetailType = WeldSymbolDetail_Type.BothSide;
            wsModel02.weldLength1 = "5";
            wsModel02.tailVisible = false;
            wsModel02.leaderAngle = 60;
            wsModel02.leaderLineLength = 15;
            weldingList.AddRange(weldSymbols.GetWeldSymbol(GetSumPoint(topAngleTop.EndPoint, 0, 0), singleModel, scaleValue, wsModel02));
            styleService.SetLayerListEntity(ref weldingList, layerService.LayerDimension);
            drawList.outlineList.AddRange(weldingList);

            return drawList;
        }




        // new
        public DrawEntityModel DrawAnchorB2N2W_Detail_Type1A_Type3(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue, int TypeNum = 0)
        {
            DrawEntityModel drawList = new DrawEntityModel();

            // AnchorB2N2W_Detail Type1A~Type3
            // TypeNum - 0:Type1A , 1: Type1B, 2: Type2, 3: Type3




            //singleModel.Entities.Clear();

            List<Entity> anchorHeadLineList = new List<Entity>();
            List<Entity> anchorLineList = new List<Entity>();
            List<Entity> centerLineList = new List<Entity>();
            List<Line> lineList = new List<Line>();


            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);


            /////////////////////////////
            //      Information      ////
            /////////////////////////////

            // Info : Head 
            double pipeHeadLength = 130;
            double pipeDiameter = 36;
            double pipeRadius = pipeDiameter / 2;
            double pipeHeadAndNut = 110;
            double pipeNeck = pipeHeadLength - pipeHeadAndNut;

            double nutHeight = 29;
            double nutLength = 63.5;

            double washerThk = 19;
            double washerWidth = 80;
            double washerHoleDiameter = 39;

            // 파이프 최상단 : Nut 윗부분 
            // Washer 밑부분 : 최상단을 뺀 나머지
            double pipeHeadEmtySpace = pipeHeadLength - (nutHeight * 2) - washerThk - pipeNeck;
            double pipeHeadTopDistance = 20;
            double pipeHeadbottomDistance = pipeHeadEmtySpace - pipeHeadTopDistance;

            // Info : Body
            double pipeBodyLength = 0;


            ////////////////////////
            ///  Draw 
            ////////////////////////

            // Center Point Info
            double centerWidth = 0;
            double centerHeight = 0;


            // TypeNum - 0:Type1A , 1: Type1B, 2: Type2, 3: Type3
            switch (TypeNum)
            {
                case 0:
                    {
                        // Info : Body
                        pipeBodyLength = 230;
                        double pipeBottomLength = 10;
                        double pipeMidLength = pipeBodyLength - pipeBottomLength - washerThk;

                        // CenterPoint Info
                        centerWidth = ((washerWidth / 2) - (pipeDiameter / 2)) / 2;
                        centerHeight = pipeBodyLength / 2;

                        // bottom Pipe Start Point
                        Point3D bottomPipeStartPoint = GetSumPoint(referencePoint, (washerWidth / 2) - (pipeDiameter / 2), 0);

                        // Draw : Pipe Bottom Line
                        List<Line> pipeBottomLIneList = GetRectangle(bottomPipeStartPoint, pipeBottomLength, pipeDiameter);

                        // Draw : Bottom Washer 
                        List<Line> washerLineList = GetRectangle(GetSumPoint(referencePoint, 0, pipeBottomLength), washerThk, washerWidth);
                        Line washerLineTop = washerLineList[2];

                        // Draw : Pipe Body Line
                        List<Line> pipeBodyLIneList = GetRectangle(GetSumPoint(bottomPipeStartPoint, 0, pipeBottomLength + washerThk), pipeMidLength, pipeDiameter);
                        Line pipeBodyLineRight = pipeBodyLIneList[1];
                        Line pipeBodyLineLeft = pipeBodyLIneList[3];


                        // Draw : Head
                        Point3D headStartPoint = GetSumPoint(referencePoint, washerWidth / 2, pipeBodyLength);
                        drawList.AddDrawEntity(DrawAnchorHead_type1_2(headStartPoint,scaleValue));


                        anchorLineList.AddRange(new Entity[] {
                                 pipeBodyLineRight, pipeBodyLineLeft,
                        });

                        anchorLineList.AddRange(pipeBottomLIneList);
                        anchorLineList.AddRange(washerLineList);
                        anchorLineList.AddRange(anchorHeadLineList);

                        break;
                    }

                case 1:
                    {
                        // Info : Bopdy
                        pipeBodyLength = 560;
                        double ringBottomHalfLength = 80;
                        double ringRadius = 35;
                        double guideArcAngle = 60;
                        double arcPointX = 212; // One Size


                        // CenterPoint Info
                        centerWidth = ringRadius;
                        centerHeight = pipeBodyLength / 2;

                        // bottom Ring Start Point(Left Bottom)
                        Point3D bottomRingStartPoint = GetSumPoint(referencePoint, 0, 0);

                        // Draw : Ring Bottom Left Line
                        List<Line> ringBottomLIneList = GetRectangle(bottomRingStartPoint, pipeDiameter, arcPointX);
                        Line ringBottomLineBottom = ringBottomLIneList[0];
                        Line ringBottomLineTop = ringBottomLIneList[2];
                        Line ringBottomLineLeft = ringBottomLIneList[3];

                        Line guideLineArcOut = new Line(GetSumPoint(bottomRingStartPoint, -arcPointX, 0), GetSumPoint(bottomRingStartPoint, arcPointX, 0));
                        guideLineArcOut.Rotate(Utility.DegToRad(-guideArcAngle), Vector3D.AxisZ, GetSumPoint(bottomRingStartPoint, arcPointX, 0));

                        Line guideLineArcIn = (Line)guideLineArcOut.Offset(pipeDiameter, Vector3D.AxisZ);


                        // Draw : Pipe Body Line
                        List<Line> pipeBodyLIneList = GetRectangle(GetSumPoint(bottomRingStartPoint, ringBottomHalfLength - pipeRadius, 0), pipeBodyLength, pipeDiameter);
                        Line pipeBodyLineRight = pipeBodyLIneList[1];
                        Line pipeBodyLineLeft = pipeBodyLIneList[3];


                        // Arc Fillet
                        if (Curve.Fillet(guideLineArcIn, ringBottomLineTop, ringRadius, true, false, true, true, out Arc arcFilletInSide))
                        { anchorLineList.Add(arcFilletInSide); }

                        if (Curve.Fillet(guideLineArcOut, ringBottomLineBottom, ringRadius + pipeDiameter, true, false, true, true, out Arc arcFilletOutSide))
                        { anchorLineList.Add(arcFilletOutSide); }

                        if (Curve.Fillet(pipeBodyLineRight, guideLineArcOut, 50 - pipeRadius, false, false, true, true, out Arc arcFilletRight))
                        { anchorLineList.Add(arcFilletRight); }

                        if (Curve.Fillet(pipeBodyLineLeft, guideLineArcIn, 50 + pipeRadius, false, false, true, true, out Arc arcFilletLeft))
                        { anchorLineList.Add(arcFilletLeft); }

                        // Draw : Head
                        Point3D headStartPoint = GetSumPoint(referencePoint, ringBottomHalfLength, pipeBodyLength);
                        drawList.AddDrawEntity(DrawAnchorHead_type1_2(headStartPoint,scaleValue));

                        /*
                        // Move : Head shift
                        Vector3D shiftXY = new Vector3D(referencePoint.X + (washerWidth / 2), referencePoint.Y + pipeBodyLength);
                        foreach (Entity eachLine in anchorHeadLineList)
                        {
                            eachLine.Translate(shiftXY);
                        }
                        /**/

                        anchorLineList.AddRange(new Entity[] {

                                ringBottomLineLeft, ringBottomLineBottom, ringBottomLineTop,
                                guideLineArcOut,guideLineArcIn,
                                pipeBodyLineRight, pipeBodyLineLeft,
                        });

                        break;
                    }

                case 2:
                    {
                        // Info : Bopdy
                        pipeBodyLength = 490;
                        double ringRadius = 90;
                        double ringOutLineRadius = ringRadius + pipeDiameter;
                        double arcfilletAngle = 50;
                        double ringHeight = 320;

                        // CenterPoint Info
                        centerWidth = ringRadius + pipeRadius/2;
                        centerHeight = pipeBodyLength / 1.5;

                        // Calculate Angle : triangle
                        double ringDiagonalRadian = Math.Asin((ringRadius + pipeRadius) / (ringHeight - ringOutLineRadius));
                        double ringDiagonalAngle = Utility.RadToDeg(ringDiagonalRadian);
                        double diagonalCenterAngle = 90 - ringDiagonalAngle;


                        // bottom Ring Start Point(Left Bottom)
                        Point3D bottomRingStartPoint = GetSumPoint(referencePoint, 0, 0);
                        Point3D guideStartPoint = GetSumPoint(bottomRingStartPoint, ringOutLineRadius, ringHeight);
                        Point3D circleCenterPoint = GetSumPoint(bottomRingStartPoint, ringOutLineRadius, ringOutLineRadius);

                        // Draw : Pipe Body Line
                        List<Line> pipeBodyLIneList = GetRectangle(GetSumPoint(bottomRingStartPoint, ringOutLineRadius - pipeRadius, 0), pipeBodyLength, pipeDiameter);
                        Line pipeBodyLineRight = pipeBodyLIneList[1];
                        Line pipeBodyLineLeft = pipeBodyLIneList[3];

                        Line guideVeticalCenterLine = new Line(GetSumPoint(guideStartPoint, 0, 0),
                                                               GetSumPoint(bottomRingStartPoint, ringOutLineRadius, 0));

                        Circle ringInnerCicle = new Circle(GetSumPoint(circleCenterPoint, 0, 0), ringRadius);
                        Circle ringouterCicle = new Circle(GetSumPoint(circleCenterPoint, 0, 0), ringOutLineRadius);


                        Line guidCenterRightLine = new Line(GetSumPoint(circleCenterPoint, 0, 0), GetSumPoint(circleCenterPoint, 0, ringHeight));
                        Line guidCenterLeftLine = new Line(GetSumPoint(circleCenterPoint, 0, 0), GetSumPoint(circleCenterPoint, 0, ringHeight));
                        guidCenterRightLine.Rotate(Utility.DegToRad(-diagonalCenterAngle), Vector3D.AxisZ, GetSumPoint(circleCenterPoint, 0, 0));
                        guidCenterLeftLine.Rotate(Utility.DegToRad(diagonalCenterAngle), Vector3D.AxisZ, GetSumPoint(circleCenterPoint, 0, 0));


                        // Get intersect : Right / Left Point of Circle
                        Point3D[] intersectORCircleP = guidCenterRightLine.IntersectWith(ringouterCicle);
                        Point3D[] intersectIRCircleP = guidCenterRightLine.IntersectWith(ringInnerCicle);
                        Point3D[] intersectOLCircleP = guidCenterLeftLine.IntersectWith(ringouterCicle);
                        Point3D[] intersectILCircleP = guidCenterLeftLine.IntersectWith(ringInnerCicle);


                        Line diagonalROLine = new Line(intersectORCircleP[0], GetSumPoint(intersectORCircleP[0], -pipeBodyLength, 0));
                        diagonalROLine.Rotate(Utility.DegToRad(-diagonalCenterAngle), Vector3D.AxisZ, GetSumPoint(intersectORCircleP[0], 0, 0));
                        Line diagonalRILine = new Line(intersectIRCircleP[0], GetSumPoint(intersectIRCircleP[0], -pipeBodyLength, 0));
                        diagonalRILine.Rotate(Utility.DegToRad(-diagonalCenterAngle), Vector3D.AxisZ, GetSumPoint(intersectIRCircleP[0], 0, 0));
                        Line diagonalLOLine = new Line(intersectOLCircleP[0], GetSumPoint(intersectOLCircleP[0], pipeBodyLength, 0));
                        diagonalLOLine.Rotate(Utility.DegToRad(diagonalCenterAngle), Vector3D.AxisZ, GetSumPoint(intersectOLCircleP[0], 0, 0));
                        Line diagonalLILine = new Line(intersectILCircleP[0], GetSumPoint(intersectILCircleP[0], pipeBodyLength, 0));
                        diagonalLILine.Rotate(Utility.DegToRad(diagonalCenterAngle), Vector3D.AxisZ, GetSumPoint(intersectILCircleP[0], 0, 0));


                        // Ring Left Top Lines
                        Point3D[] circleInnerLineEndPoint = diagonalLILine.IntersectWith(diagonalRILine);
                        Line diagonalCircleInnerLine = new Line(GetSumPoint(intersectILCircleP[0], 0, 0), circleInnerLineEndPoint[0]);

                        Line cicleLeftEndLine = new Line(GetSumPoint(circleInnerLineEndPoint[0], 0, 0), GetSumPoint(circleInnerLineEndPoint[0], -pipeDiameter, 0));
                        cicleLeftEndLine.Rotate(Utility.DegToRad(-ringDiagonalAngle), Vector3D.AxisZ, GetSumPoint(circleInnerLineEndPoint[0], 0, 0));

                        Line diagonalCircleouterLine = new Line(GetSumPoint(cicleLeftEndLine.EndPoint, 0, 0), GetSumPoint(intersectOLCircleP[0], 0, 0));


                        Arc arcouterRing = new Arc(intersectOLCircleP[0], guideVeticalCenterLine.EndPoint, intersectORCircleP[0], false);
                        Arc arcInnerRing = new Arc(intersectILCircleP[0], GetSumPoint(guideVeticalCenterLine.EndPoint, 0, pipeDiameter), intersectIRCircleP[0], false);


                        // Arc Fillet
                        if (Curve.Fillet(pipeBodyLineRight, diagonalROLine, arcfilletAngle, true, false, true, true, out Arc arcFilletInSide))
                        { anchorLineList.Add(arcFilletInSide); }

                        if (Curve.Fillet(pipeBodyLineLeft, diagonalRILine, arcfilletAngle + pipeDiameter, true, false, true, true, out Arc arcFilletOutSide))
                        { anchorLineList.Add(arcFilletOutSide); }


                        // Draw : Head
                        Point3D headStartPoint = GetSumPoint(referencePoint, ringOutLineRadius, pipeBodyLength);
                        drawList.AddDrawEntity(DrawAnchorHead_type1_2( headStartPoint,scaleValue, false));


                        anchorLineList.AddRange(new Entity[] {
                                //ringInnerCicle, ringouterCicle,
                                pipeBodyLineRight, pipeBodyLineLeft,
                                diagonalROLine, diagonalRILine,
                                diagonalCircleouterLine, diagonalCircleInnerLine, cicleLeftEndLine, // Ring Left Top Lines
                                //guideLineArcOut,guideLineArcIn,
                                pipeBodyLineRight, pipeBodyLineLeft,
                                arcouterRing, arcInnerRing,
                        });

                        lineList.AddRange(new Line[] {


                        });


                        break;
                    }

                case 3:
                    {
                        // Info : Body
                        pipeBodyLength = 490;
                        double bottomHeadLength = pipeHeadLength;
                        double pipeMidLength = pipeBodyLength - bottomHeadLength;

                        // CenterPoint Info
                        centerWidth = bottomHeadLength;
                        centerHeight = pipeBodyLength / 2;

                        // Washer 그려질 위치 설정( Left Mid )
                        washerWidth = 150;
                        Point3D washerStartPoint = GetSumPoint(referencePoint, -(100 + washerWidth), 200);

                        // Draw : Washer ( Left Mid )
                        double washerHalfWidth = washerWidth / 2;
                        List<Line> washerLeftTopBox = GetRectangle(washerStartPoint, washerWidth, washerWidth);
                        Circle washerLeftTopCirlce = new Circle(GetSumPoint(washerStartPoint, washerHalfWidth, washerHalfWidth), washerHoleDiameter / 2);


                        // Mirror Head <type3>
                        Point3D bottomHeadStartPoint = GetSumPoint(referencePoint, washerWidth / 2, 0);
                        drawList.AddDrawEntity(DrawAnchorHead_type3(bottomHeadStartPoint,scaleValue));



                        // bottom Pipe Start Point
                        Point3D bottomPipeStartPoint = GetSumPoint(referencePoint, washerHalfWidth - (pipeDiameter / 2), bottomHeadLength);

                        // Draw : Pipe Body Line
                        List<Line> pipeBodyLIneList = GetRectangle(GetSumPoint(bottomPipeStartPoint, 0, 0), pipeMidLength, pipeDiameter);
                        Line pipeBodyLineRight = pipeBodyLIneList[1];
                        Line pipeBodyLineLeft = pipeBodyLIneList[3];


                        // Draw : Head
                        Point3D headStartPoint = GetSumPoint(referencePoint, washerHalfWidth, pipeBodyLength);
                        drawList.AddDrawEntity(DrawAnchorHead_type1_2(headStartPoint, scaleValue, false));


                        anchorLineList.AddRange(new Entity[] {
                                 washerLeftTopCirlce,
                                 pipeBodyLineRight, pipeBodyLineLeft,
                        });

                        anchorLineList.AddRange(washerLeftTopBox);
                        //anchorLineList.AddRange(anchorHeadLineList);

                        break;
                    }

            }

            
            // Center Line : Red color
            Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref anchorLineList, layerService.LayerOutLine);
            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            //styleService.SetLayerListLine(ref lineList, layerService.LayerDimension);

            //singleModel.Entities.AddRange(anchorLineList);
            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(lineList);

            //singleModel.Entities.Regen();
            //singleModel.Invalidate();

            //singleModel.SetView(viewType.Top);
            //singleModel.ZoomFit();
            //singleModel.Refresh();

            // Center Point

            Point3D modelCenterPoint = GetSumPoint(referencePoint,centerWidth , centerHeight);
            SetModelCenterPoint(PAPERSUB_TYPE.AnchorDetail, modelCenterPoint);


            styleService.SetLayerListEntity(ref anchorLineList, layerService.LayerOutLine);
            styleService.SetLayerListLine(ref lineList, layerService.LayerDimension);
            drawList.outlineList.AddRange(anchorLineList);
            drawList.outlineList.AddRange(lineList);

            return drawList;

        }
        public DrawEntityModel DrawAnchorHead_type1_2(Point3D refPoint,  double scaleValue, bool type1 = true)
        {

            DrawEntityModel drawList = new DrawEntityModel();

            // AnchorHead_type1_2

            // Draw Point : Bottom CenterPoint

            // Type1 : Nut * 2 , Washer(O)
            // Type2 : Nut *2, Washer(X)

            List<Entity> anchorHeadLineList = new List<Entity>();
            List<Entity> centerLineList = new List<Entity>();
            List<Line> inlineList = new List<Line>();

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            // Washer 그려질 위치 설정
            Point3D washerStartPoint = GetSumPoint(referencePoint, -250, 160);

            //Point3D zeroPoint = new Point3D(0, 0);

            /////////////////////////////
            //      Information      ////
            /////////////////////////////

            double pipeHeadLength = 130;
            double pipeHeadAndNut = 110;
            double pipeDiameter = 36;

            // type2 일때
            if (!type1)
            {
                pipeHeadLength = 200;
                pipeHeadAndNut = 130;
            }

            double pipeNeck = pipeHeadLength - pipeHeadAndNut;

            double nutHeight = 29;
            double nutLength = 63.5;

            double washerThk = 19;
            double washerWidth = 80;
            double washerHoleDiameter = 39;
            double washeraHoleRadius = washerHoleDiameter / 2;

            /*
            if (!type1) {
                washerThk = 0;
                washerWidth = 0;
            }/**/

            // 파이프 최상단 : Nut 윗부분 임의값
            // Washer 밑부분 : 최상단을 뺀 나머지 임의값
            double pipeHeadEmtySpace = pipeHeadLength - (nutHeight * 2) - washerThk - pipeNeck;
            double pipeHeadTopDistance = 20;   // 20은 임의값  bottom은 나머지 값
            double pipeHeadbottomDistance = pipeHeadEmtySpace - pipeHeadTopDistance;
            if (!type1)
            {

                pipeHeadTopDistance = (pipeHeadAndNut - (nutHeight * 2)) / 2;
                pipeHeadbottomDistance = pipeHeadTopDistance;
            }

            double inLineDistance = 4;
            double pipeNeckDiagonalAngle = 60;



            ////////////////////////
            ///  Draw 
            ////////////////////////


            // Draw : Washer ( Left Top ) - Type 1
            List<Line> washerLeftTopBox = new List<Line>();
            Circle washerLeftTopCirlce = null;
            if (type1)
            {
                washerLeftTopBox = GetRectangle(washerStartPoint, washerWidth, washerWidth);
                washerLeftTopCirlce = new Circle(GetSumPoint(washerStartPoint, washerWidth / 2, washerWidth / 2), washeraHoleRadius);
            }


            // Draw : Pipe Neck Line
            List<Line> pipeBottomLIneList = GetRectangle(GetSumPoint(referencePoint, -(pipeDiameter / 2), 0), pipeNeck, pipeDiameter);
            Line pipeBottomLineRight = pipeBottomLIneList[1];
            Line pipeBottomLineLeft = pipeBottomLIneList[3];

            // Draw : Pipe Head Bottom Emty Space Line  //  임의값
            List<Line> pipeHeadbottomLineList = GetRectangle(GetSumPoint(pipeBottomLineLeft.StartPoint, 0, 0), pipeHeadbottomDistance, pipeDiameter);
            Line pipeHeadbottomLineBottom = pipeHeadbottomLineList[0];
            Line pipeHeadbottomLineRight = pipeHeadbottomLineList[1];
            Line pipeHeadbottomLineLeft = pipeHeadbottomLineList[3];

            Line pipeHeadbottomInLineRight = new Line(GetSumPoint(pipeHeadbottomLineRight.StartPoint, -inLineDistance, 0),
                                                    GetSumPoint(pipeHeadbottomLineRight.EndPoint, -inLineDistance, 0));
            Line pipeHeadbottomInLineLeft = new Line(GetSumPoint(pipeHeadbottomLineLeft.StartPoint, inLineDistance, 0),
                                                    GetSumPoint(pipeHeadbottomLineLeft.EndPoint, inLineDistance, 0));

            Line guideScrewThreadRight = new Line(GetSumPoint(pipeHeadbottomInLineRight.EndPoint, 0, 0), GetSumPoint(pipeHeadbottomInLineRight.EndPoint, 0, -pipeHeadLength));
            Line guideScrewThreadLeft = new Line(GetSumPoint(pipeHeadbottomInLineLeft.EndPoint, 0, 0), GetSumPoint(pipeHeadbottomInLineLeft.EndPoint, 0, -pipeHeadLength));
            guideScrewThreadRight.Rotate(Utility.DegToRad(90 - pipeNeckDiagonalAngle), Vector3D.AxisZ, pipeHeadbottomInLineRight.EndPoint);
            guideScrewThreadLeft.Rotate(Utility.DegToRad(-(90 - pipeNeckDiagonalAngle)), Vector3D.AxisZ, pipeHeadbottomInLineLeft.EndPoint);

            Point3D[] intersectThreadRight = guideScrewThreadRight.IntersectWith(pipeBottomLineRight);
            Point3D[] intersectThreadLeft = guideScrewThreadLeft.IntersectWith(pipeBottomLineLeft);

            Line pipeBottomDiagonalRight = new Line(pipeHeadbottomInLineRight.EndPoint, intersectThreadRight[0]);
            Line pipeBottomDiagonalLeft = new Line(pipeHeadbottomInLineLeft.EndPoint, intersectThreadLeft[0]);



            //pipeNeckDiagonalAngle


            // Draw : Washer 
            List<Line> washerLineList = GetRectangle(GetSumPoint(referencePoint, -(washerWidth / 2), (pipeHeadbottomLineLeft.StartPoint.Y - referencePoint.Y)), washerThk, washerWidth);
            Line washerLineTop = washerLineList[2];



            // Draw : Nut
            List<Entity> nutLineListT1 = GetAnchorNutLine(false);
            /*
            Entity[] eachLines = new Entity[13];
            int i = 0;/**/
            double nut1Y = referencePoint.Y + (washerLineTop.StartPoint.Y - referencePoint.Y);
            if (!type1) { nut1Y = referencePoint.Y + pipeHeadLength - (pipeHeadAndNut / 2) - nutHeight; }
            Vector3D shiftXY = new Vector3D(referencePoint.X - (washerWidth / 2) + ((washerWidth - nutLength) / 2), nut1Y);
            foreach (Entity eachLine in nutLineListT1)
            {
                eachLine.Translate(shiftXY);
                /*
                if (i < 10)
                { eachLines[i] = (Line)eachLine.Clone(); }
                else
                { eachLines[i] = (Arc)eachLine.Clone(); }
                i += 1;
                /**/
            }

            //editingService.SetTranslate(ref nutLineListT1, GetSumPoint(washerLineTop.StartPoint, (washerWidth-nutLength)/2, 0), zeroPoint);  // 와셔위로 이동
            List<Entity> nutLineListT2 = GetAnchorNutLine();
            //editingService.SetTranslate(ref nutLineListT2, GetSumPoint(washerLineTop.StartPoint, (washerWidth - nutLength) / 2, nutHeight), zeroPoint);  // 와셔위로 이동
            double nut2Y = referencePoint.Y + (washerLineTop.StartPoint.Y - referencePoint.Y) + nutHeight;
            if (!type1) { nut2Y = referencePoint.Y + pipeHeadLength - (pipeHeadAndNut / 2); }
            shiftXY = new Vector3D(referencePoint.X - (washerWidth / 2) + ((washerWidth - nutLength) / 2), nut2Y);
            foreach (Entity eachLine in nutLineListT2)
            {
                eachLine.Translate(shiftXY);
            }

            // Draw : Pipe Head Top Emty Space Line  //  임의값
            List<Line> pipeHeadTopLineList = GetRectangle(GetSumPoint(referencePoint, -(pipeDiameter / 2), pipeHeadLength - pipeHeadTopDistance), pipeHeadTopDistance, pipeDiameter);
            Line tempPipeHeadTopRight = pipeHeadTopLineList[1];
            Line tempPipeHeadTopLeft = pipeHeadTopLineList[3];

            // Draw In Line (Left/ Right)
            Line pipeHeadTopInRight = new Line(GetSumPoint(tempPipeHeadTopRight.StartPoint, -inLineDistance, 0),
                                                    GetSumPoint(tempPipeHeadTopRight.EndPoint, -inLineDistance, 0));
            Line pipeHeadTopInLeft = new Line(GetSumPoint(tempPipeHeadTopLeft.StartPoint, inLineDistance, 0),
                                                    GetSumPoint(tempPipeHeadTopLeft.EndPoint, inLineDistance, 0));

            // Get Intersect TopLine width (Left Light ) angle 45

            Line guideDiagonalTopLeft = (Line)pipeHeadTopInLeft.Clone();
            Line guideDiagonalTopRight = (Line)pipeHeadTopInRight.Clone();

            guideDiagonalTopLeft.Rotate(Utility.DegToRad(-45), Vector3D.AxisZ, pipeHeadTopInLeft.StartPoint);
            guideDiagonalTopRight.Rotate(Utility.DegToRad(45), Vector3D.AxisZ, pipeHeadTopInRight.StartPoint);

            Point3D[] pipeHeadTopInLeftStart = tempPipeHeadTopLeft.IntersectWith(guideDiagonalTopLeft);
            Point3D[] pipeHeadTopInRightStart = tempPipeHeadTopRight.IntersectWith(guideDiagonalTopRight);

            // Draw Top1/2,Left,Right Line
            Line pipeHeadTopTop = new Line(pipeHeadTopInLeft.StartPoint, pipeHeadTopInRight.StartPoint);
            Line pipeHeadTopLeft = new Line(pipeHeadTopInLeftStart[0], tempPipeHeadTopLeft.EndPoint);
            Line pipeHeadTopRigth = new Line(pipeHeadTopInRightStart[0], tempPipeHeadTopRight.EndPoint);
            Line pipeHeadTopinTop = new Line(pipeHeadTopLeft.StartPoint, pipeHeadTopRigth.StartPoint);

            // Draw Top Diagonal Line
            Line pipeHeadTopLeftDiagonal = new Line(pipeHeadTopTop.StartPoint, pipeHeadTopLeft.StartPoint);
            Line pipeHeadTopRighttDiagonal = new Line(pipeHeadTopTop.EndPoint, pipeHeadTopRigth.StartPoint);


            // Draw Screen : All
            if (type1)
            {
                anchorHeadLineList.AddRange(washerLineList);
                anchorHeadLineList.AddRange(washerLeftTopBox);
                anchorHeadLineList.Add(washerLeftTopCirlce);
            }

            anchorHeadLineList.AddRange(nutLineListT1);
            anchorHeadLineList.AddRange(nutLineListT2);
            anchorHeadLineList.AddRange(new Entity[] {
                         pipeBottomLineRight, pipeBottomLineLeft,
                         pipeHeadbottomLineRight, pipeHeadbottomLineLeft,
                         pipeHeadTopTop, pipeHeadTopLeft, pipeHeadTopRigth, pipeHeadTopinTop,
                         pipeHeadTopLeftDiagonal, pipeHeadTopRighttDiagonal,

            });

            inlineList.AddRange(new Line[] {
                         pipeHeadbottomLineBottom, pipeHeadbottomInLineRight, pipeHeadbottomInLineLeft,
                         pipeHeadTopInRight, pipeHeadTopInLeft,
                         pipeBottomDiagonalRight, pipeBottomDiagonalLeft,
            });


            // Center Line : Red color
            Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref anchorHeadLineList, layerService.LayerOutLine);
            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            //styleService.SetLayerListLine(ref inlineList, layerService.LayerDimension);

            //singleModel.Entities.AddRange(anchorHeadLineList);
            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(inlineList);


            //return anchorHeadLineList;

            styleService.SetLayerListEntity(ref anchorHeadLineList, layerService.LayerOutLine);
            styleService.SetLayerListLine(ref inlineList, layerService.LayerDimension);

            drawList.outlineList.AddRange(anchorHeadLineList);
            drawList.outlineList.AddRange(inlineList);

            return drawList;


        }
        public DrawEntityModel DrawAnchorHead_type3(Point3D refPoint, double scaleValue, bool flip = true)
        {
            DrawEntityModel drawList = new DrawEntityModel();

            // AnchorHead_type3

            // Draw Point : Bottom CenterPoint

            // Type1 : Nut * 2 , Washer(O)  // Nut type2
            // Type2 : Nut *2, Washer(X)  // Nut type2
            // Type3 : Nut(Nut type1), Washer(O), Nut(Nut type1)

            //singleModel.Entities.Clear();

            List<Entity> anchorHeadLineList = new List<Entity>();
            List<Entity> centerLineList = new List<Entity>();
            List<Entity> inlineList = new List<Entity>();

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);


            /////////////////////////////
            //      Information      ////
            /////////////////////////////

            double pipeHeadLength = 130;
            double pipeHeadAndNut = 110;
            double pipeDiameter = 36;

            double pipeNeck = pipeHeadLength - pipeHeadAndNut;

            double nutHeight = 29;
            double nutLength = 63.5;

            double washerThk = 16;
            double washerWidth = 150;
            double washerHoleDiameter = 39;
            double washeraHoleRadius = washerHoleDiameter / 2;


            // 파이프 최상단 : Nut 윗부분 임의값
            // Washer 밑부분 : 최상단을 뺀 나머지 임의값
            double pipeHeadEmtySpace = pipeHeadLength - (nutHeight * 2) - washerThk - pipeNeck;
            double pipeHeadTopDistance = 20;   // 20은 임의값  bottom은 나머지 값
            double pipeHeadbottomDistance = pipeHeadEmtySpace - pipeHeadTopDistance;


            double inLineDistance = 4;
            double pipeNeckDiagonalAngle = 60;



            ////////////////////////
            ///  Draw 
            ////////////////////////

            // Draw : Pipe Neck Line
            List<Line> pipeBottomLIneList = GetRectangle(GetSumPoint(referencePoint, -(pipeDiameter / 2), 0), pipeNeck, pipeDiameter);
            Line pipeBottomLineRight = pipeBottomLIneList[1];
            Line pipeBottomLineLeft = pipeBottomLIneList[3];

            // Draw : Pipe Head Bottom Emty Space Line  
            List<Line> pipeHeadbottomLineList = GetRectangle(GetSumPoint(pipeBottomLineLeft.StartPoint, 0, 0), pipeHeadbottomDistance, pipeDiameter);
            Line pipeHeadbottomLineBottom = pipeHeadbottomLineList[0];
            Line pipeHeadbottomLineRight = pipeHeadbottomLineList[1];
            Line pipeHeadbottomLineTop = pipeHeadbottomLineList[2];
            Line pipeHeadbottomLineLeft = pipeHeadbottomLineList[3];

            Line pipeHeadbottomInLineRight = new Line(GetSumPoint(pipeHeadbottomLineRight.StartPoint, -inLineDistance, 0),
                                                    GetSumPoint(pipeHeadbottomLineRight.EndPoint, -inLineDistance, 0));
            Line pipeHeadbottomInLineLeft = new Line(GetSumPoint(pipeHeadbottomLineLeft.StartPoint, inLineDistance, 0),
                                                    GetSumPoint(pipeHeadbottomLineLeft.EndPoint, inLineDistance, 0));

            Line guideScrewThreadRight = new Line(GetSumPoint(pipeHeadbottomInLineRight.EndPoint, 0, 0), GetSumPoint(pipeHeadbottomInLineRight.EndPoint, 0, -pipeHeadLength));
            Line guideScrewThreadLeft = new Line(GetSumPoint(pipeHeadbottomInLineLeft.EndPoint, 0, 0), GetSumPoint(pipeHeadbottomInLineLeft.EndPoint, 0, -pipeHeadLength));
            guideScrewThreadRight.Rotate(Utility.DegToRad(90 - pipeNeckDiagonalAngle), Vector3D.AxisZ, pipeHeadbottomInLineRight.EndPoint);
            guideScrewThreadLeft.Rotate(Utility.DegToRad(-(90 - pipeNeckDiagonalAngle)), Vector3D.AxisZ, pipeHeadbottomInLineLeft.EndPoint);

            Point3D[] intersectThreadRight = guideScrewThreadRight.IntersectWith(pipeBottomLineRight);
            Point3D[] intersectThreadLeft = guideScrewThreadLeft.IntersectWith(pipeBottomLineLeft);

            Line pipeBottomDiagonalRight = new Line(pipeHeadbottomInLineRight.EndPoint, intersectThreadRight[0]);
            Line pipeBottomDiagonalLeft = new Line(pipeHeadbottomInLineLeft.EndPoint, intersectThreadLeft[0]);


            // Draw : Nut1

            double nut1Y = pipeHeadbottomLineLeft.StartPoint.Y;
            double nutX = referencePoint.X - (washerWidth / 2) + ((washerWidth - nutLength) / 2);

            List<Entity> nutLineListT1 = GetAnchorNutLine(false);

            Vector3D shiftXY = new Vector3D(nutX, nut1Y);
            foreach (Entity eachLine in nutLineListT1)
            {
                eachLine.Translate(shiftXY);
            }

            foreach (Entity eachLine in nutLineListT1)
            {
                eachLine.Rotate(Utility.DegToRad(180), Vector3D.AxisZ, GetSumPoint(pipeHeadbottomLineTop.MidPoint, 0, nutHeight / 2));
                //eachLine.Rotate(Utility.DegToRad(180), Vector3D.AxisZ, GetSumPoint(referencePoint,0,0));
            }
            /**/



            // Draw : Washer 
            List<Line> washerLineList = GetRectangle(GetSumPoint(referencePoint, -(washerWidth / 2), (pipeHeadbottomLineLeft.StartPoint.Y - referencePoint.Y) + nutHeight), washerThk, washerWidth);
            Line washerLineTop = washerLineList[2];


            // Draw : Nut2
            List<Entity> nutLineListT2 = GetAnchorNutLine(false);
            double nut2Y = washerLineTop.StartPoint.Y;
            shiftXY = new Vector3D(nutX, nut2Y);
            foreach (Entity eachLine in nutLineListT2)
            {
                eachLine.Translate(shiftXY);
            }


            // Draw : Pipe Head Top Emty Space Line  //  임의값
            List<Line> pipeHeadTopLineList = GetRectangle(GetSumPoint(referencePoint, -(pipeDiameter / 2), pipeHeadLength - pipeHeadTopDistance), pipeHeadTopDistance, pipeDiameter);
            Line tempPipeHeadTopRight = pipeHeadTopLineList[1];
            Line tempPipeHeadTopLeft = pipeHeadTopLineList[3];

            // Draw In Line (Left/ Right)
            Line pipeHeadTopInRight = new Line(GetSumPoint(tempPipeHeadTopRight.StartPoint, -inLineDistance, 0),
                                                    GetSumPoint(tempPipeHeadTopRight.EndPoint, -inLineDistance, 0));
            Line pipeHeadTopInLeft = new Line(GetSumPoint(tempPipeHeadTopLeft.StartPoint, inLineDistance, 0),
                                                    GetSumPoint(tempPipeHeadTopLeft.EndPoint, inLineDistance, 0));

            // Get Intersect TopLine width (Left Light ) angle 45

            Line guideDiagonalTopLeft = (Line)pipeHeadTopInLeft.Clone();
            Line guideDiagonalTopRight = (Line)pipeHeadTopInRight.Clone();

            guideDiagonalTopLeft.Rotate(Utility.DegToRad(-45), Vector3D.AxisZ, pipeHeadTopInLeft.StartPoint);
            guideDiagonalTopRight.Rotate(Utility.DegToRad(45), Vector3D.AxisZ, pipeHeadTopInRight.StartPoint);

            Point3D[] pipeHeadTopInLeftStart = tempPipeHeadTopLeft.IntersectWith(guideDiagonalTopLeft);
            Point3D[] pipeHeadTopInRightStart = tempPipeHeadTopRight.IntersectWith(guideDiagonalTopRight);

            // Draw Top1/2,Left,Right Line
            Line pipeHeadTopTop = new Line(pipeHeadTopInLeft.StartPoint, pipeHeadTopInRight.StartPoint);
            Line pipeHeadTopLeft = new Line(pipeHeadTopInLeftStart[0], tempPipeHeadTopLeft.EndPoint);
            Line pipeHeadTopRigth = new Line(pipeHeadTopInRightStart[0], tempPipeHeadTopRight.EndPoint);
            Line pipeHeadTopinTop = new Line(pipeHeadTopLeft.StartPoint, pipeHeadTopRigth.StartPoint);

            // Draw Top Diagonal Line
            Line pipeHeadTopLeftDiagonal = new Line(pipeHeadTopTop.StartPoint, pipeHeadTopLeft.StartPoint);
            Line pipeHeadTopRighttDiagonal = new Line(pipeHeadTopTop.EndPoint, pipeHeadTopRigth.StartPoint);


            // Draw Screen : All
            anchorHeadLineList.AddRange(washerLineList);

            anchorHeadLineList.AddRange(nutLineListT1);
            anchorHeadLineList.AddRange(nutLineListT2);
            anchorHeadLineList.AddRange(new Entity[] {
                         pipeBottomLineRight, pipeBottomLineLeft,
                         pipeHeadbottomLineRight, pipeHeadbottomLineLeft,
                         pipeHeadTopTop, pipeHeadTopLeft, pipeHeadTopRigth, pipeHeadTopinTop,
                         pipeHeadTopLeftDiagonal, pipeHeadTopRighttDiagonal,

            });

            inlineList.AddRange(new Entity[] {
                         pipeHeadbottomLineBottom, pipeHeadbottomInLineRight, pipeHeadbottomInLineLeft,
                         pipeHeadTopInRight, pipeHeadTopInLeft,
                         pipeBottomDiagonalRight, pipeBottomDiagonalLeft,
            });





            // Center Line : Red color
            Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            centerLineList.Add(centerline);

            if (flip)
            {
                List<Entity> mirrorList1 = editingService.GetMirrorEntity(Plane.XZ, anchorHeadLineList, referencePoint.X, referencePoint.Y + 65, true);
                List<Entity> mirrorList2 = editingService.GetMirrorEntity(Plane.XZ, inlineList, referencePoint.X, referencePoint.Y + 65, true);
                styleService.SetLayerListEntity(ref mirrorList1, layerService.LayerOutLine);
                styleService.SetLayerListEntity(ref mirrorList2, layerService.LayerDimension);

                //singleModel.Entities.AddRange(mirrorList1);
                //singleModel.Entities.AddRange(mirrorList2);
                drawList.outlineList.AddRange(mirrorList1);
                drawList.outlineList.AddRange(mirrorList2);

            }
            else
            {
                styleService.SetLayerListEntity(ref anchorHeadLineList, layerService.LayerOutLine);
                styleService.SetLayerListEntity(ref inlineList, layerService.LayerDimension);

                //singleModel.Entities.AddRange(anchorHeadLineList);
                //singleModel.Entities.AddRange(inlineList);
                drawList.outlineList.AddRange(anchorHeadLineList);
                drawList.outlineList.AddRange(inlineList);
            }

            /*
            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            singleModel.Entities.AddRange(centerLineList);

            singleModel.Entities.Regen();
            singleModel.Invalidate();

            singleModel.SetView(viewType.Top);
            singleModel.ZoomFit();
            singleModel.Refresh();
            /**/
            //return anchorHeadLineList;

            

            return drawList;

        }
        public List<Entity> GetAnchorNutLine(bool type2 = true)
        {

            // Anchor Bolt type1, type2

            Point3D referencePoint = new Point3D(0, 0);

            List<Entity> AnchoBoltDrawList = new List<Entity>();



            /////////////////////////////
            //      Information      ////
            /////////////////////////////
            double boltWidth = 55;
            double boltLength = 63.5;
            double boltHeight = 29;
            Point3D centerPoint = GetSumPoint(referencePoint, boltLength / 2, boltWidth / 2);


            // Guide Line ( vetical , horizontal , Circle )
            Line verticalGuideLine = new Line(GetSumPoint(referencePoint, boltLength / 2, boltWidth), GetSumPoint(referencePoint, boltLength / 2, 0));
            Line horizontalGuideLine = new Line(GetSumPoint(referencePoint, 0, boltWidth / 2), GetSumPoint(referencePoint, boltLength, boltWidth / 2));
            Circle circleGuide = new Circle(centerPoint, boltWidth / 2);


            // Guide Line : Bolt TopLeft/Right Diagonal Line , Top Line
            Line diagonalLeftGuide = new Line(GetSumPoint(horizontalGuideLine.StartPoint, 0, 0), GetSumPoint(horizontalGuideLine.EndPoint, 0, 0));
            diagonalLeftGuide.Rotate((Utility.DegToRad(60)), Vector3D.AxisZ, horizontalGuideLine.StartPoint);
            Line topLineGuide = new Line(GetSumPoint(horizontalGuideLine.StartPoint, 0, boltWidth / 2), GetSumPoint(horizontalGuideLine.EndPoint, 0, boltWidth / 2));


            // Get intersect Top Start Point
            Point3D[] intersectTopStartPoint = topLineGuide.IntersectWith(diagonalLeftGuide);
            Point3D topLineStartPoint = GetSumPoint(intersectTopStartPoint[0], 0, 0);
            double topLineLength = boltLength - ((topLineStartPoint.X - referencePoint.X) * 2);

            // Get top/Diagonal Line
            Line topLine = new Line(topLineStartPoint, GetSumPoint(topLineStartPoint, topLineLength, 0));
            Line diagonalLeftLine = new Line(horizontalGuideLine.StartPoint, topLineStartPoint);

            // Get diagonal Mid Point
            Point3D midPoint = diagonalLeftLine.MidPoint;

            // Get intersect DIagonal - Cicle
            //Point3D[] intersectCicle = diagonalLeftLine.IntersectWith(circleGuide);

            double distanceCircleLength = (boltLength - boltWidth) / 2;

            List<Line> testOutLIneList = GetRectangle(GetSumPoint(referencePoint, 0, 0), boltHeight, distanceCircleLength);
            Line outLineRight = testOutLIneList[1];
            Line outLineLeft = testOutLIneList[3];


            // Get Cut length  : Cut bolt
            Line boltDiagonalGuide = new Line(GetSumPoint(outLineLeft.StartPoint, -boltLength, 0), GetSumPoint(outLineRight.StartPoint, 0, 0));
            boltDiagonalGuide.Rotate((Utility.DegToRad(30)), Vector3D.AxisZ, outLineRight.StartPoint);

            Point3D[] intersectLeftOutLine = outLineLeft.IntersectWith(boltDiagonalGuide);

            double cutLength = (outLineLeft.StartPoint.Y - referencePoint.Y) - (intersectLeftOutLine[0].Y - referencePoint.Y);


            /////////////////////////////////////
            ///     Draw Bolt
            /////////////////////////////////////
            double Type1Vertical = 0;

            if (!type2)
            {
                // vertical start Point
                Type1Vertical = cutLength;
            }



            // Out Line : Left Right
            List<Line> outLIneList = GetRectangle(GetSumPoint(referencePoint, 0, cutLength - Type1Vertical), boltHeight - (cutLength * 2) + Type1Vertical, boltLength);
            outLineRight = outLIneList[1];
            outLineLeft = outLIneList[3];

            // Mid Line : Top Bottom
            List<Line> midLIneList = GetRectangle(GetSumPoint(referencePoint, distanceCircleLength, 0), boltHeight, boltWidth);
            Line midLineTop = midLIneList[2];
            Line midLineBottom = midLIneList[0];

            // In Line : Left Right
            List<Line> inLIneList = GetRectangle(GetSumPoint(referencePoint, (topLineStartPoint.X - referencePoint.X), cutLength - Type1Vertical), boltHeight - (cutLength * 2) + Type1Vertical, topLineLength);
            Line inLineRight = inLIneList[1];
            Line inLineLeft = inLIneList[3];


            // Guid Line : vetical Line - SIde/Center
            List<Line> guideLIneList = GetRectangle(GetSumPoint(referencePoint, midPoint.X - referencePoint.X, 0), boltHeight, (boltLength / 2) - (midPoint.X - referencePoint.X));
            Line guideLineCenter = guideLIneList[1];
            Line guideLineLeft = guideLIneList[3];

            double guideLineDistance = (guideLineCenter.StartPoint.X - referencePoint.X) - (guideLineLeft.StartPoint.X - referencePoint.X);
            Line guideLineRight = new Line(GetSumPoint(guideLineCenter.StartPoint, guideLineDistance, 0), GetSumPoint(guideLineCenter.EndPoint, guideLineDistance, 0));


            // Arc
            Arc arcCenterTop = new Arc(inLineLeft.StartPoint, guideLineCenter.StartPoint, inLineRight.StartPoint, false);
            Arc arcLeftTop = new Arc(outLineLeft.StartPoint, guideLineLeft.StartPoint, inLineLeft.StartPoint, false);
            Arc arcRightTop = new Arc(inLineRight.StartPoint, guideLineRight.StartPoint, outLineRight.StartPoint, false);

            Arc arcCenterBottom = null;
            Arc arcLeftBottom = null;
            Arc arcRightBottom = null;
            if (type2) // type2 일때만 아랫쪽 Draw Arc
            {
                arcCenterBottom = new Arc(inLineLeft.EndPoint, guideLineCenter.EndPoint, inLineRight.EndPoint, false);
                arcLeftBottom = new Arc(outLineLeft.EndPoint, guideLineLeft.EndPoint, inLineLeft.EndPoint, false);
                arcRightBottom = new Arc(inLineRight.EndPoint, guideLineRight.EndPoint, outLineRight.EndPoint, false);
            }


            // Cut Line 
            Line cutLineLT = new Line(GetSumPoint(outLineLeft.StartPoint, 0, 0), GetSumPoint(midLineTop.StartPoint, 0, 0));
            Line cutLineRT = new Line(GetSumPoint(outLineRight.StartPoint, 0, 0), GetSumPoint(midLineTop.EndPoint, 0, 0));
            Line cutLineLB = new Line(GetSumPoint(outLineLeft.EndPoint, 0, 0), GetSumPoint(midLineBottom.StartPoint, 0, 0));
            Line cutLineRB = new Line(GetSumPoint(outLineRight.EndPoint, 0, 0), GetSumPoint(midLineBottom.EndPoint, 0, 0));


            AnchoBoltDrawList.AddRange(new Entity[] {

                //verticalGuideLine,horizontalGuideLine,circleGuide,
                //diagonalLeftGuide, topLineGuide, boltDiagonalGuide,

                         outLineRight, outLineLeft,
                         midLineTop, midLineBottom,
                         inLineRight, inLineLeft,
                         cutLineLT, cutLineRT, cutLineLB, cutLineRB,
                         // index[10~12] arcTop  Left / Center / Right
                         arcLeftTop, arcCenterTop,  arcRightTop,
                         ///  arc : 6~11
            });

            if (type2)
            {
                AnchoBoltDrawList.AddRange(new Entity[] {
                         // index[13~15] arcBottom Left / Center / Right
                         arcLeftBottom, arcCenterBottom, arcRightBottom,
                });
            }

            /**/


            return AnchoBoltDrawList;

        }



        public DrawEntityModel ArrangeShellPlate(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {

            DrawEntityModel drawList = new DrawEntityModel();

            DrawBMCalService bmCalService = new DrawBMCalService();

            //singleModel.Entities.Clear();

            // todo : Delete - Do not Using List
            List<Entity> newList = new List<Entity>();
            List<Entity> centerLineList = new List<Entity>();
            List<Entity> plateList = new List<Entity>();
            List<Entity> textList = new List<Entity>();
            List<Entity> horizontalArcList = new List<Entity>();
            List<Line> tableLineList = new List<Line>();
            List<Line> PlateLineList = new List<Line>();
            List<Line> horizontalVList_test = new List<Line>();

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            //      Information      ////
            /////////////////////////////
            // Tank ID
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);

            // Plate Info
            double plateWidth = valueService.GetDoubleValue(assemblyData.ShellInput[0].PlateWidth);
            double plateLength = valueService.GetDoubleValue(assemblyData.ShellInput[0].PlateMaxLength);

            // course Count
            double courseCount = assemblyData.ShellOutput.Count;

            // Course Width
            //double[] sampleCourseWidth = new double[8] { 2438, 2438, 2438, 2438, 2438, 2438, 2438, 2010 };
            // course Thickness
            //double[] sampleCourseThk = new double[] { 30, 20, 20, 16, 16, 14, 14, 12 };

            SingletonData.DimensionsList.Clear();
            double dimCount = 0;

            List<double> sampleCourseWidth = new List<double>();
            List<double> sampleCourseThk = new List<double>();
            foreach (ShellOutputModel eachShell in assemblyData.ShellOutput)
            {
                sampleCourseWidth.Add(valueService.GetDoubleValue(eachShell.PlateWidth));
                sampleCourseThk.Add(valueService.GetDoubleValue(eachShell.Thickness));

                dimCount++;
                DrawDimensionsModel newDimModel = new DrawDimensionsModel();
                newDimModel.No = "S-" + dimCount;
                newDimModel.W = eachShell.PlateWidth;
                newDimModel.Thk = "t" + eachShell.Thickness;
                newDimModel.PartNo = (dimCount - 1).ToString();
                if (newDimModel.PartNo == "0")
                    newDimModel.PartNo = "-";

                SingletonData.DimensionsList.Add(newDimModel);
            }

            // plate Name
            string plateFirstName = "S";
            string plateFullName = null;
            double plateNumberOfSize = 0;

            double textSize = 2.5;
            double scaleTextSize = textSize * scaleValue;
            Text Nametag = null;

            //  Setting  /////
            double plateCount = tankID / plateLength;

            List<double> courseWidthList = new List<double>();
            List<double> courseThkList = new List<double>();

            foreach (double eachWidth in sampleCourseWidth)
                courseWidthList.Add(eachWidth);
            foreach (double eachThk in sampleCourseThk)
                courseThkList.Add(eachThk);


            // Save Infomation
            List<double> resetPlateLength = new List<double>();  // new Plate Length
            List<double> Perimeter = new List<double>();  // 둘레길이
            List<double> shellRadius = new List<double>();  // 쉘 반지름

            List<List<PlateInfo>> PlateInfoListArray = new List<List<PlateInfo>>();
            double sumWidth = 0;

            ///   Draw course
            for (int courseNum = 0; courseNum < courseCount; courseNum++)
            {
                List<PlateInfo> eachPlateList = new List<PlateInfo>();
                PlateInfoListArray.Add(eachPlateList);

                double eachCourseWidth = courseWidthList[courseNum];

                // 조건 안맞을 경우
                //if (eachCourseWidth > plateWidth)
                //{ break; }

                // Plate 행마다 같은 Size
                plateNumberOfSize = courseNum + 1;

                // Calculate : one course Plate Length
                double eachCourseThk = courseThkList[courseNum];
                double eachCourseDiameter = tankID + (eachCourseThk * 2);
                double eachCoursePermiter = Math.PI * eachCourseDiameter;  // 둘레길이
                double eachPlatCount = Math.Ceiling(eachCoursePermiter / plateLength);
                double eachPlateLength = eachCoursePermiter / eachPlatCount;


                // Table Info
                double tempDimX = bmCalService.GetCalShellX(valueService.GetDoubleValue(SingletonData.DimensionsList[courseNum].W),
                                                            eachPlateLength
                                                            );
                tempDimX = Math.Round(tempDimX, 1,MidpointRounding.AwayFromZero);
                SingletonData.DimensionsList[courseNum].Qty = eachPlatCount.ToString();
                SingletonData.DimensionsList[courseNum].L = Math.Round(eachPlateLength,1,MidpointRounding.AwayFromZero).ToString();
                SingletonData.DimensionsList[courseNum].X = tempDimX.ToString();

                shellRadius.Add(eachCourseDiameter);
                Perimeter.Add(eachCoursePermiter);  // 둘레길이
                resetPlateLength.Add(eachPlateLength);  // Plate 한장의 길이 재설정후 저장


                /// shift reset
                double shift = courseNum % 3;
                double shiftX = resetPlateLength[0] * (shift / 3);
                double textExtendPointX = 0;
                double textExtendPointY = courseWidthList[courseNum] / 2;

                // Draw one course
                for (int i = 0; i < eachPlatCount; i++)
                {
                    double sumLength = eachPlateLength * (i + 1);
                    Point3D eachPlatePoint = GetSumPoint(referencePoint, shiftX, sumWidth);
                    PlateInfo eachOnePlate = new PlateInfo(plateFirstName, plateNumberOfSize, eachPlatePoint, eachCourseWidth, sumLength);
                    eachPlateList.Add(eachOnePlate);

                    // Draw Plate line
                    Line PlateLine = new Line(eachOnePlate.plateTopEndPoint, eachOnePlate.plateBottomEndPoint);
                    PlateLineList.Add(PlateLine);
                    PlateLine = new Line(eachOnePlate.plateTopStartPoint, eachOnePlate.plateTopEndPoint);
                    PlateLineList.Add(PlateLine);
                    PlateLine = new Line(eachOnePlate.plateBottomStartPoint, eachOnePlate.plateBottomEndPoint);
                    PlateLineList.Add(PlateLine);
                    PlateLine = new Line(eachOnePlate.plateTopStartPoint, eachOnePlate.plateBottomStartPoint);
                    PlateLineList.Add(PlateLine);

                    // Draw Text : PlateName
                    plateFullName = eachOnePlate.GetPlateName();

                    textExtendPointX = (eachPlateLength * i) + (eachPlateLength / 2);
                    Point3D textPoint = GetSumPoint(eachPlatePoint, textExtendPointX, textExtendPointY);
                    Nametag = new Text(textPoint, plateFullName, scaleTextSize);
                    Nametag.Alignment = Text.alignmentType.MiddleCenter;
                    textList.Add(Nametag);

                }

                /// Sum : width 0~courseNum
                sumWidth += courseWidthList[courseNum];
            }




            ///////////////////////////////////
            //  Draw Table : Cutting plan

            List<Entity> textPlanList = new List<Entity>();

            // table size
            double tableWidth = 140;
            double tableHeadHight = 8;
            double tableRowHight = 7;
            double tablePlanMaxCount = 14;
            double tablePlanCount = 14;
            double tableHight = 0;

            List<double> tableColumnWidth = new List<double> { 15, 25, 25, 25, 15, 15, 20 };


            // one table draw :  ->  Change for loop
            tableHight = (tableHeadHight * 2) + (tablePlanCount * tableRowHight);

            // table Start Point Setting
            double farX = 0;
            double hightY = tableHight;
            Point3D tableStartPoint = GetSumPoint(referencePoint, farX, hightY);


            // topLine
            Line tableLine = new Line(tableStartPoint, GetSumPoint(tableStartPoint, tableWidth, 0));
            tableLineList.Add(tableLine);
            // vertical_left line
            tableLine = new Line(tableStartPoint, GetSumPoint(tableStartPoint, 0, -tableHight));
            tableLineList.Add(tableLine);
            // vertical_right line
            tableLine = new Line(tableStartPoint, GetSumPoint(tableStartPoint, tableWidth, -tableHight));
            tableLineList.Add(tableLine);
            // bottomLine
            tableLine = new Line(GetSumPoint(tableStartPoint, 0, -tableHight), GetSumPoint(tableStartPoint, tableWidth, -tableHight));
            tableLineList.Add(tableLine);





            // Center Line : Red color
            Line centerline = new Line(new Point3D(-1000, 0), new Point3D(Perimeter[0] + 1000, 0));
            centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            //styleService.SetLayerListLine(ref PlateLineList, layerService.LayerOutLine);
            //styleService.SetLayerListEntity(ref textList, layerService.LayerDimension);
            //styleService.SetLayerListLine(ref tableLineList, layerService.LayerOutLine);
            //styleService.SetLayerListEntity(ref textPlanList, layerService.LayerDimension);

            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(PlateLineList);
            //singleModel.Entities.AddRange(textList);
            //singleModel.Entities.AddRange(tableLineList);
            //singleModel.Entities.AddRange(textPlanList);

            ////singleModel.Entities.Add(new Line(new Point3D(0, 200), new Point3D(1000, 800)), layerTTL);

            //singleModel.Entities.Regen();
            //singleModel.Invalidate();


            //singleModel.SetView(viewType.Top);
            //singleModel.ZoomFit();
            //singleModel.Refresh();

            styleService.SetLayerListLine(ref PlateLineList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref textList, layerService.LayerDimension);
            styleService.SetLayerListLine(ref tableLineList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref textPlanList, layerService.LayerDimension);
            drawList.outlineList.AddRange(PlateLineList);
            drawList.outlineList.AddRange(textList);
            drawList.outlineList.AddRange(tableLineList);
            drawList.outlineList.AddRange(textPlanList);

            return drawList;

        }

        public DrawEntityModel DrawPlanTable(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {
            //singleModel.Entities.Clear();

            DrawEntityModel drawList = new DrawEntityModel();

            List<Entity> centerLineList = new List<Entity>();
            List<Entity> textPlanList = new List<Entity>();
            List<Entity> textHeadBottomList = new List<Entity>();
            List<Line> tableLineList = new List<Line>();
            List<Entity> circlePartNo = new List<Entity>();
            List<Line> horizontalLineList = new List<Line>();

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);


            ///////////////////////////////////
            //  Draw Table : Cutting plan


            // table size
            double tableWidth = 140;
            double tableHeadHight1 = 8;
            double tableHeadHight2 = 7;
            double tableRowHight = 7;

            double tablecolumnCount = 7;
            double tableHeadHight = 0;
            double tableBodyHight = 0;
            double tableHight = 0;


            // Very Import ####
            List<double> ColumnWidthList = new List<double> { 15, 25, 25, 25, 15, 15, 20 };
            List<string> columnTitleList = new List<string> { "NO.", "\"L\"", "\"W\"", "\"X\"", "TH'K", "Q'TY", "PART NO." };
            // text info
            double textSize = 2.5;
            double textCircleDia = 7;
            double textCircleRadius = textCircleDia / 2;
            string stringHead = "DIMENSIONS FOR CUTTING (PER MEAN DIA.)";
            string stringBottom = "CUTTING PLAN TABLE";





            // Body
            List<List<string>> PlanArrayAllList = new List<List<string>>();
            double PlanCount = 14;
            //for (int k = 0; k < PlanCount; k++)
            //{
            //    string tempName = (1 + k).ToString();
            //    string planSizeName = "S-" + tempName;
            //    List<string> PlanList = new List<string> { planSizeName, "7362", "2340", "7752.7", "t17", "5", tempName };
            //    PlanArrayAllList.Add(PlanList);
            //}

            foreach(DrawDimensionsModel eachDim in SingletonData.DimensionsList)
            {
                List<string> PlanList = new List<string> { eachDim.No, eachDim.L, eachDim.W, eachDim.X, eachDim.Thk, eachDim.Qty, eachDim.PartNo };
                PlanArrayAllList.Add(PlanList);
            }

            List<List<string>> PlanArrayList = new List<List<string>>();
            //double PlanCount = 14;
            //for (int k = 0; k < PlanCount; k++)
            //{
            //    List<string> PlanList = new List<string> { "S-1", "7362", "2340", "7752.7", "t17", "5", (1 + k).ToString() };
            //    PlanArrayList.Add(PlanList);
            //}

            List<List<List<string>>> PlanArrayAllDivList = new List<List<List<string>>>();

            double tablePlanMaxCount = 14;
            //double tableCount = PlanCount / tablePlanMaxCount;
            //double lastTableCount = PlanCount % tablePlanMaxCount;

            //List<List<string>>newTabel = new List<List<string>>();

            //int allListCount = 0;
            //for (int i = 1; i <= tableCount; i++)
            //{
            //    List<List<string>> newEachTable = new List<List<string>>();
            //    for (int k = 0; k < tablePlanMaxCount; k++)
            //    {
            //        newEachTable.Add(PlanArrayAllList[allListCount]);
            //        allListCount++;
            //    }
            //    PlanArrayAllDivList.Add(newEachTable);
            //}

            //if (lastTableCount > 0)
            //{
            //    List<List<string>> newEachTable2 = new List<List<string>>();
            //    for (int i = 0; i < lastTableCount; i++)
            //    {
            //        newEachTable2.Add(PlanArrayAllList[allListCount]);
            //        allListCount++;
            //    }
            //    PlanArrayAllDivList.Add(newEachTable2);
            //}


            List<List<string>> eachPlanArrayList = new List<List<string>>();
            int allListCount = 0;
            foreach (List<string> eachList in PlanArrayAllList)
            {

                if ((allListCount % tablePlanMaxCount == 0) && allListCount != 0)
                {
                    List<List<string>> neweachPlanArrayList = new List<List<string>>();

                    // Clone
                    foreach (List<string> eachNewList in eachPlanArrayList)
                        neweachPlanArrayList.Add(eachNewList);

                    PlanArrayAllDivList.Add(neweachPlanArrayList);
                    eachPlanArrayList.Clear();
                }
                eachPlanArrayList.Add(eachList);

                allListCount++;
            }
            if (eachPlanArrayList.Count > 0)
            {
                List<List<string>> neweachPlanArrayList = new List<List<string>>();
                // Clone
                foreach (List<string> eachNewList in eachPlanArrayList)
                    neweachPlanArrayList.Add(eachNewList);

                PlanArrayAllDivList.Add(neweachPlanArrayList);
            }

            // one table draw :  ->  Change for loop
            tableHeadHight = (tableHeadHight1 + tableHeadHight2);
            tableBodyHight = (PlanCount * tableRowHight);
            tableHight = tableHeadHight + tableBodyHight;

            // table Start Point Setting
            double farX = 0;
            double hightY = tableHight;
            Point3D tableStartPoint = GetSumPoint(referencePoint, farX, hightY);


            // Draw : Table Out Lines  //
            // topLine
            Line tableLine = new Line(tableStartPoint, GetSumPoint(tableStartPoint, tableWidth, 0));
            tableLineList.Add(tableLine);
            // vertical_left line
            tableLine = new Line(tableStartPoint, GetSumPoint(tableStartPoint, 0, -tableHight));
            tableLineList.Add(tableLine);
            // vertical_right line
            tableLine = new Line(GetSumPoint(tableStartPoint, tableWidth, 0), GetSumPoint(tableStartPoint, tableWidth, -tableHight));
            tableLineList.Add(tableLine);
            // bottomLine
            tableLine = new Line(GetSumPoint(tableStartPoint, 0, -tableHight), GetSumPoint(tableStartPoint, tableWidth, -tableHight));
            tableLineList.Add(tableLine);

            // column Lines
            double sumColumnWidth = 0;
            foreach (double columnWidth in ColumnWidthList)
            {
                sumColumnWidth += columnWidth;
                Point3D columnStartPoint = GetSumPoint(tableStartPoint, sumColumnWidth, -tableHeadHight1);
                tableLine = new Line(columnStartPoint, GetSumPoint(columnStartPoint, 0, -(tableHight - tableHeadHight1)));
                tableLineList.Add(tableLine);
            }


            // Draw : Head second Line
            tableLineList.Add(new Line(GetSumPoint(tableStartPoint, 0, -tableHeadHight1), GetSumPoint(tableStartPoint, tableWidth, -tableHeadHight1)));
            // Draw : Head second Text
            Text textTitle = new Text(GetSumPoint(tableStartPoint, tableWidth / 2, -tableHeadHight1 / 2), stringHead, textSize) { Alignment = Text.alignmentType.MiddleCenter };
            textPlanList.Add(textTitle);

            double sumColWidth = 0;
            for (int i = 0; i < tablecolumnCount; i++)
            {

                double eachColumnWidth = ColumnWidthList[i];
                string eachColumnName = columnTitleList[i];
                Text textColName = new Text(GetSumPoint(tableStartPoint, sumColWidth + (eachColumnWidth / 2), -tableHeadHight1 - (tableHeadHight2 / 2)), eachColumnName, textSize) { Alignment = Text.alignmentType.MiddleCenter };
                textPlanList.Add(textColName);
                sumColWidth += eachColumnWidth;
            }


            // Draw : Text(Plan Data) & row Lines
            double sumRowHight = 0;
            Point3D bodyStartPoint = GetSumPoint(tableStartPoint, 0, -tableHeadHight);


            //////////////////////////////////////////////////////////////////
            ///     Very Very Important !!!!
            ///     //////////////////////////////////////////////////////////
            foreach (List<string> eachPlan in PlanArrayAllDivList[0])    // !!!!! todo : PlanArrayAllDivList[0]   0 -> 반복문 변수로 교체
            {
                double sumBodyColumnWidth = 0;
                double eachRowY = sumRowHight + (tableRowHight / 2);
                double eachColumnWidth = 0;

                for (int i = 0; i < tablecolumnCount; i++)
                {

                    eachColumnWidth = ColumnWidthList[i];
                    string eachPlanName = eachPlan[i];
                    Text textColName = new Text(GetSumPoint(bodyStartPoint, sumBodyColumnWidth + (eachColumnWidth / 2), -eachRowY), eachPlanName, textSize) { Alignment = Text.alignmentType.MiddleCenter };
                    textPlanList.Add(textColName);

                    if (i == tablecolumnCount - 1)
                    {
                        // Draw : Circle of Part No.
                        Circle PartNoCircle = new Circle(GetSumPoint(bodyStartPoint,
                                                                      sumBodyColumnWidth + (eachColumnWidth / 2), -eachRowY),
                                                                      textCircleRadius);
                        circlePartNo.Add(PartNoCircle);
                    }
                    sumBodyColumnWidth += eachColumnWidth;
                }

                sumRowHight += tableRowHight;
            }

            sumRowHight = 0;
            for (int i = 0; i < PlanCount; i++)
            {
                // row Lines
                tableLine = new Line(GetSumPoint(bodyStartPoint, 0, -sumRowHight), GetSumPoint(bodyStartPoint, tableWidth, -sumRowHight));
                tableLineList.Add(tableLine);
                sumRowHight += tableRowHight;
            }


            // Center Line : Red color
            Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            //styleService.SetLayerListEntity(ref textHeadBottomList, layerService.LayerDimension);
            //styleService.SetLayerListLine(ref tableLineList, layerService.LayerOutLine);
            //styleService.SetLayerListLine(ref horizontalLineList, layerService.LayerOutLine);
            //styleService.SetLayerListEntity(ref circlePartNo, layerService.LayerOutLine);
            //styleService.SetLayerListEntity(ref textPlanList, layerService.LayerDimension);

            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(textHeadBottomList);
            //singleModel.Entities.AddRange(tableLineList);
            //singleModel.Entities.AddRange(horizontalLineList);
            //singleModel.Entities.AddRange(textPlanList);
            //singleModel.Entities.AddRange(circlePartNo);


            //singleModel.Entities.Add(new Line(new Point3D(0, 200), new Point3D(1000, 800)), layerTTL);

            //singleModel.Entities.Regen();
            //singleModel.Invalidate();


            //singleModel.SetView(viewType.Top);
            //singleModel.ZoomFit();
            //singleModel.Refresh();



            // Center Point
            double centerHeight = (tableHeadHight1 + tableHeadHight2 + (tableRowHight * PlanCount)) / 2;
            Point3D modelCenterPoint = GetSumPoint(referencePoint, tableWidth/2, centerHeight);
            SetModelCenterPoint(PAPERSUB_TYPE.DimensionsForCutting, modelCenterPoint);



            styleService.SetLayerListEntity(ref textHeadBottomList, layerService.LayerDimension);
            styleService.SetLayerListLine(ref tableLineList, layerService.LayerOutLine);
            styleService.SetLayerListLine(ref horizontalLineList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref circlePartNo, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref textPlanList, layerService.LayerDimension);
            drawList.outlineList.AddRange(textHeadBottomList);
            drawList.outlineList.AddRange(tableLineList);
            drawList.outlineList.AddRange(horizontalLineList);
            drawList.outlineList.AddRange(circlePartNo);
            drawList.outlineList.AddRange(textPlanList);

            return drawList;


        }






        public DrawEntityModel DrawBottomPlateAnnularJointDetail(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {

            DrawEntityModel drawList = new DrawEntityModel();

            // DE4TAIL "A"  CASE 1

            //singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Line> diagonalLineList = new List<Line>();
            List<Line> verticalLineList = new List<Line>();
            List<Line> horizontalLineList = new List<Line>();

            List<List<Point3D>> JointPointList = new List<List<Point3D>>();
            List<double[]> extendPointXList = new List<double[]>();


            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);
            

            /////////////////////////////
            //      Information      ////
            /////////////////////////////

            // distance size info
            double sumYpoint = 0;
            double[] Ypoint = new double[] { 0, (sumYpoint += 3), (sumYpoint += 4), (sumYpoint += 4), (sumYpoint += 50) };
            //int YpointCount = Ypoint.Length;

            // setting Point
            extendPointXList.Add(new double[] {
                0, 110
            });

            extendPointXList.Add(new double[] {
                0, 5, 100, 5, (-5 -20 +80 -13), 35
            });

            extendPointXList.Add(new double[] {
                5, 25, 4, (-4 -25 +100 -20 -4), 4, 20, (-20 +40), 3, (37-13), 13, 4, (-4 -13 +35)
            });

            extendPointXList.Add(new double[] {
                (110 - 5 - 20), 40, 3, 37
            });

            extendPointXList.Add(new double[] {
                (5+25), 4
            });



            //  insert Point : calculate Point and insert
            for (int i = 0; i < Ypoint.Length; i++)
            {
                JointPointList.Add(new List<Point3D>());
                double sumPointX = 0;
                foreach (double eachExtendX in extendPointXList[i])
                {
                    sumPointX += eachExtendX;
                    JointPointList[i].Add(GetSumPoint(referencePoint, sumPointX, Ypoint[i]));
                }
            }


            // Draw HorizontalLine
            horizontalLineList.AddRange(new Line[] { 
                // Y0 Line
                new Line( JointPointList[0][0], JointPointList[0][1] ),
                // Y1 Line
                new Line( JointPointList[1][0], JointPointList[1][3] ),
                new Line( JointPointList[1][4], JointPointList[1][5] ),
                // Y2 Line
                new Line( JointPointList[2][0], JointPointList[2][11] ),
                // Y3 Line
                new Line( JointPointList[3][0], JointPointList[3][3] ),
                new Line( JointPointList[3][2], JointPointList[3][3] ),
            });


            // Draw VerticalLine
            verticalLineList.AddRange(new Line[] { 
                // vertical No.1 Line
                new Line( JointPointList[0][0], JointPointList[1][0] ),
                new Line( JointPointList[0][1], JointPointList[1][3] ),
                // vertical No.2 Line
                new Line( JointPointList[1][1], JointPointList[2][0] ),
                new Line( JointPointList[1][2], JointPointList[2][5] ),
                new Line( JointPointList[1][4], JointPointList[2][8] ),
                //new Line( JointPointList[1][5], JointPointList[2][11] ),
                // vertical No.3 Line
                new Line( JointPointList[2][4], JointPointList[3][0] ),
                new Line( JointPointList[2][9], JointPointList[3][3] ),
                // vertical No.4 Line
                new Line( JointPointList[2][1], JointPointList[4][0] ),
                new Line( JointPointList[2][2], JointPointList[4][1] ),
            });


            // Draw diagonallLine
            diagonalLineList.AddRange(new Line[] { 
                // left
                new Line( JointPointList[2][3], JointPointList[3][0] ),
                // right
                new Line( JointPointList[2][10], JointPointList[3][3] ),
            });




            // Vertical Demention
            /*

             - LEFT Position -
            * t6
            JointPointList[0][0], JointPointList[1][0]
            * t10
            JointPointList[1][1], JointPointList[2][0]

             - Rigth Position -
            # t8 (bottom)
            JointPointList[1][5], JointPointList[2][11]
            # t8 
            JointPointList[2][11], JointPointList[3][3]

            /**/



            // Spline Demention
            /*

             - LEFT Top Position -
            JointPointList[4][0], JointPointList[4][1]

            - Middle Position -
            JointPointList[2][6], JointPointList[3][1]
            JointPointList[2][7], JointPointList[3][2]

             - Rigth Position -
            JointPointList[1][5], JointPointList[2][11]

            /**/



            // Horizontal Demention
            /*

            # 870
            JointPointList[0][0], JointPointList[0][1]

            # 10 : Cut at job Site
            JointPointList[1][0], JointPointList[1][1]

            # BACKING STRIP
            JointPointList[1][2].X, JointPointList[0][0].Y 

            # ANNULAR PLATE
            JointPointList[2][0].X + ??, JointPointList[2][0].Y 

            # 60
            JointPointList[2][0], JointPointList[2][1]

            # t16 SHELL I.D @
            JointPointList[2][1], JointPointList[2][2]

            # t709
            JointPointList[2][2], JointPointList[2][4]

            # 65(MIN.60)
            JointPointList[3][0], JointPointList[2][5]
            // or JointPointList[2][4], JointPointList[2][5]

            # BOTTOM PLATE
            JointPointList[3][0].X + ??, JointPointList[3][0].Y 

            # 26 (right)
            JointPointList[2][8], JointPointList[3][3]
            // or JointPointList[2][8], JointPointList[2][9]

            # ANNULAR PLATE O.D 6620
            JointPointList[2][0]


            /**/



            // Diagonal Demention
            /*

            # 8 (left)
                                  JointPointList[3][0]
            JointPointList[2][3], JointPointList[2][4]

            # 8 (right)
            JointPointList[3][3]
            JointPointList[2][9], JointPointList[2][10]

            /**/


            
            
            
            // Check Line : Red color
            //Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            //centerLineList.Add(centerline);
            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);


            //styleService.SetLayerListLine(ref horizontalLineList, layerService.LayerOutLine);
            styleService.SetLayerListLine(ref verticalLineList, layerService.LayerOutLine);
            styleService.SetLayerListLine(ref diagonalLineList, layerService.LayerOutLine);


            //drawList.outlineList.AddRange(horizontalLineList);
            drawList.outlineList.AddRange(verticalLineList);
            drawList.outlineList.AddRange(diagonalLineList);




            // Center Point
            Point3D modelCenterPoint = GetSumPoint(JointPointList[3][0],0, 10);
            SetModelCenterPoint(PAPERSUB_TYPE.BottomPlateJointAnnularDetail, modelCenterPoint);






            // Dimension : Break
            double tempHeight = 4;
            List<Entity> breakAllList = new List<Entity>();
            breakAllList.AddRange(breakService.GetFlatBreakLine(GetSumPoint(horizontalLineList[3].EndPoint, 0, 0),
                                                            GetSumPoint(horizontalLineList[3].EndPoint, 0, -tempHeight),
                                                            scaleValue, true, true));

            styleService.SetLayerListEntity(ref breakAllList, layerService.LayerDimension);
            drawList.outlineList.AddRange(breakAllList);


            double sLineLength = tempHeight;

            List<Entity> horizontalLineListNew = new List<Entity>();
            foreach (Line eachLine in horizontalLineList)
                horizontalLineListNew.Add(eachLine);
            List<Entity> newSDoubleLineList = breakService.GetSDoubleLine(GetSumPoint(horizontalLineList[4].MidPoint, 0, 0), sLineLength, scaleValue,0,true,270);
            List<Entity> newListList = breakService.GetTrimSDoubleLine(horizontalLineListNew, newSDoubleLineList);
            styleService.SetLayerListEntity(ref newSDoubleLineList, layerService.LayerDimension);
            drawList.outlineList.AddRange(newSDoubleLineList);
            styleService.SetLayerListEntity(ref newListList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(newListList);



            // Dimension : Arc
            //DrawEntityModel testDimArc01 = drawService.Draw_DimensionArc(GetSumPoint(leftTopLine.EndPoint, 0, 0), GetSumPoint(rightTopLine.StartPoint, 0, 0), "top", 12, "60˚", 45, 30, -30, 0, scaleValue, layerService.LayerDimension);
            //drawList.AddDrawEntity(testDimArc01);
            // Dimension : top

            string bottomThk = assemblyData.BottomInput[0].BottomPlateThickness;
            string bottomThkStr = "t" + bottomThk;
            string anchorTopPlateC = assemblyData.AnchorageInput[0].TopPlateC;
            string shellThk = assemblyData.ShellOutput[0].Thickness;
            string annularPlateWidth = assemblyData.BottomInput[0].AnnularPlateWidth;

            double annularOverlapBottom = publicFunctionService.GetDetailAnnularOverlap();
            double annularInsideBeforeBottom = valueService.GetDoubleValue(annularPlateWidth) - annularOverlapBottom - valueService.GetDoubleValue(anchorTopPlateC) - valueService.GetDoubleValue(shellThk);

            string annularUnderOutWidth = "10";
            double annularUnderWidth = (valueService.GetDoubleValue(annularPlateWidth) + valueService.GetDoubleValue(annularPlateWidth) * 2);

            DrawDimensionModel dimModel01 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = "30",
                textLower = "(MIN.25)",
                textSizeVisible = false,
                dimHeight = 15,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity01 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(verticalLineList[4].EndPoint, 0, 0), GetSumPoint(verticalLineList[6].EndPoint, 0, 0), scaleValue, dimModel01);
            drawList.AddDrawEntity(dimEntity01);
            DrawDimensionModel dimModel02 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = annularOverlapBottom.ToString(),
                textLower = "(MIN.60)",
                textSizeVisible = false,
                dimHeight = 30,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity02 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(verticalLineList[5].EndPoint, 0, 0), GetSumPoint(verticalLineList[3].EndPoint, 0, 0), scaleValue, dimModel02);
            drawList.AddDrawEntity(dimEntity02);
            DrawDimensionModel dimModel03 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = annularInsideBeforeBottom.ToString(),
                extLineLeftVisible = false,
                textSizeVisible = false,
                dimHeight = 30,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity03 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(verticalLineList[8].StartPoint, 0, 0), GetSumPoint(verticalLineList[5].EndPoint, 0, 0), scaleValue, dimModel03);
            drawList.AddDrawEntity(dimEntity03);

            DrawDimensionModel dimModel04 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.BOTTOM,
                textUpper = annularUnderWidth.ToString(),
                textSizeVisible = false,
                dimHeight = 22,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity04 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(verticalLineList[0].StartPoint, 0, 0), GetSumPoint(verticalLineList[1].StartPoint, 0, 0), scaleValue, dimModel04);
            drawList.AddDrawEntity(dimEntity04);
            DrawDimensionModel dimModel05 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.BOTTOM,
                textUpper = annularUnderOutWidth,
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                textUpperPosition = POSITION_TYPE.LEFT,
                textSizeVisible = false,
                dimHeight = 15,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity05 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(verticalLineList[0].StartPoint, 0, 0), GetSumPoint(verticalLineList[2].StartPoint, 0, 0), scaleValue, dimModel05);
            drawList.AddDrawEntity(dimEntity05);


            DrawDimensionModel dimModel06 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = anchorTopPlateC,
                extLineRightVisible = false,
                textSizeVisible = false,
                dimHeight = 30 + tempHeight,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity06 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(verticalLineList[2].EndPoint, 0, 0), GetSumPoint(verticalLineList[7].StartPoint, 0, 0), scaleValue, dimModel06);
            drawList.AddDrawEntity(dimEntity06);


            DrawDimensionModel dimModel07 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = "SHELL I.D " +assemblyData.GeneralDesignData[0].SizeNominalID,
                extLineRightVisible = false,
                arrowRightHeadVisible=false,
                textSizeVisible = false,
                dimHeight = 30 + 10,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity07 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(verticalLineList[8].StartPoint, 0, 0), GetSumPoint(verticalLineList[4].EndPoint, 0, 0), scaleValue, dimModel07);
            drawList.AddDrawEntity(dimEntity07);

            
            DrawDimensionModel dimModel08 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = "t" + shellThk,
                extLineLeftVisible = false,
                extLineRightVisible = false,
                arrowLeftHeadOut=true,
                arrowRightHeadVisible = false,
                textUpperPosition = POSITION_TYPE.LEFT,
                textSizeVisible = false,
                dimHeight = 30 +10 ,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity08 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(verticalLineList[7].StartPoint, 0, 0), GetSumPoint(verticalLineList[8].StartPoint, 0, 0), scaleValue, dimModel08);
            drawList.AddDrawEntity(dimEntity08);


            DrawDimensionModel dimModel09 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = "ANNULAR PLATE O.D " + assemblyData.BottomInput[0].AnnularPlateOD,
                extLineRightVisible = false,
                arrowRightHeadVisible = false,
                textSizeVisible = false,
                dimHeight = 30 + 16,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity09 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(verticalLineList[2].EndPoint, 0, 0), GetSumPoint(verticalLineList[4].EndPoint, 0, 0), scaleValue, dimModel09);
            drawList.AddDrawEntity(dimEntity09);



            //DrawDimensionModel dimModel02 = new DrawDimensionModel()
            //{
            //    position = POSITION_TYPE.RIGHT,
            //    textUpper = "t8",
            //    arrowLeftHeadOut = true,
            //    arrowRightHeadOut = true,
            //    arrowRightHeadVisible = false,
            //    textUpperPosition = POSITION_TYPE.LEFT,
            //    textSizeVisible = false,
            //    dimHeight = 12,
            //    scaleValue = scaleValue
            //};
            //DrawEntityModel dimEntity02 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lineList[0].StartPoint, 0, 0), GetSumPoint(lineList[2].EndPoint, 0, 0), scaleValue, dimModel02);
            //drawList.AddDrawEntity(dimEntity02);

            //DrawDimensionModel dimModel04 = new DrawDimensionModel()
            //{
            //    position = POSITION_TYPE.LEFT,
            //    textUpper = "t8",
            //    arrowLeftHeadOut = true,
            //    arrowRightHeadOut = true,
            //    textUpperPosition = POSITION_TYPE.RIGHT,
            //    textSizeVisible = false,
            //    dimHeight = 12,
            //    scaleValue = scaleValue
            //};
            //DrawEntityModel dimEntity04 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lineList[3].StartPoint, 0, 0), GetSumPoint(lineList[6].EndPoint, 0, 0), scaleValue, dimModel04);
            //drawList.AddDrawEntity(dimEntity04);


            //AngleSizeModel eachAngle = new AngleSizeModel();
            //if (assemblyData.RoofAngleOutput.Count > 0)
            //    eachAngle = assemblyData.RoofAngleOutput[0];
            //DrawDimensionModel dimModel03 = new DrawDimensionModel()
            //{
            //    position = POSITION_TYPE.LEFT,
            //    textUpper = "t" + eachAngle.t,
            //    extLineLeftVisible = false,
            //    extLineRightVisible = false,
            //    textSizeVisible = false,
            //    dimHeight = 6,
            //    scaleValue = scaleValue
            //};
            //DrawEntityModel dimEntity03 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(bottomLeftLine.EndPoint, 0, 0), GetSumPoint(leftTopLine.EndPoint, 0, 0), scaleValue, dimModel03);
            //drawList.AddDrawEntity(dimEntity03);

            // Dimension : Welding
            DrawWeldSymbols weldSymbols = new DrawWeldSymbols();
            List<Entity> weldList = new List<Entity>();
            DrawWeldSymbolModel wsModel01 = new DrawWeldSymbolModel();
            wsModel01.position = ORIENTATION_TYPE.TOPRIGHT;
            wsModel01.weldTypeDown = WeldSymbol_Type.Fillet;
            wsModel01.weldDetailType = WeldSymbolDetail_Type.ArrowSide;
            wsModel01.weldSize1 = bottomThk;
            wsModel01.tailVisible = false;
            wsModel01.leaderAngle = 60;
            wsModel01.leaderLineLength = 15;
            weldList.AddRange(weldSymbols.GetWeldSymbol(GetSumPoint(diagonalLineList[1].MidPoint, 0, 0), singleModel, scaleValue, wsModel01));
            DrawWeldSymbolModel wsModel02 = new DrawWeldSymbolModel();
            wsModel02.position = ORIENTATION_TYPE.TOPLEFT;
            wsModel02.weldTypeDown = WeldSymbol_Type.Fillet;
            wsModel02.weldDetailType = WeldSymbolDetail_Type.ArrowSide;
            wsModel02.weldSize1 = bottomThk;
            wsModel02.tailVisible = false;
            wsModel02.leaderAngle = 60;
            wsModel02.leaderLineLength = 15;
            weldList.AddRange(weldSymbols.GetWeldSymbol(GetSumPoint(diagonalLineList[0].MidPoint, 0, 0), singleModel, scaleValue, wsModel02));


            styleService.SetLayerListEntity(ref weldList, layerService.LayerDimension);
            drawList.outlineList.AddRange(weldList);


            //DrawBMLeaderModel leaderInfoModel = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = "HAMMERING", bmNumber = "", textAlign = POSITION_TYPE.CENTER, leaderPointLength = 12, leaderPointRadian = Utility.DegToRad(45) };
            //DrawEntityModel leaderInfoList = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(lineList[6].MidPoint, 0, 0), leaderInfoModel, scaleValue);
            //drawList.AddDrawEntity(leaderInfoList);





            return drawList;
        }

        public DrawEntityModel DrawBottomPlateJointDetail(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {

            DrawEntityModel drawList = new DrawEntityModel();

            // DETAIL "A"  CASE2

            //singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Line> lineList = new List<Line>();

            List<Point3D> JointPointList = new List<Point3D>();


            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);


            /////////////////////////////
            //      Information      ////
            /////////////////////////////



            // setting Lines1
            Line eachLine = null;
            Point3D startPoint = referencePoint;

            // Bottom Lines(Lectangle)
            startPoint = GetSumPoint(referencePoint, (80 + 50 - 15), 0); /* Point extrect /**/ JointPointList.Add(startPoint); // 0
            lineList.Add(eachLine = new Line(startPoint, GetSumPoint(startPoint, -50, 0)));
            lineList.Add(eachLine = new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 0, 5))); JointPointList.Add(eachLine.EndPoint); // 1
            lineList.Add(new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 50, 0))); JointPointList.Add(eachLine.EndPoint); // 2

            // Mid Lines(Lectangle)
            startPoint = GetSumPoint(referencePoint, 0, 5); JointPointList.Add(startPoint); // 3
            lineList.Add(eachLine = new Line(startPoint, GetSumPoint(startPoint, 80, 0))); JointPointList.Add(eachLine.EndPoint); // 4
            lineList.Add(eachLine = new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 0, 5))); JointPointList.Add(eachLine.EndPoint); // 5
            lineList.Add(new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 5, -5)));  /* diagonal Line /**/ JointPointList.Add(GetSumPoint(eachLine.EndPoint, 5, -5)); // 6
            lineList.Add(eachLine = new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, -80, 0))); JointPointList.Add(eachLine.EndPoint); // 7
            lineList.Add(new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 0, -5)));


            // Top Lines(Lectangle)
            startPoint = GetSumPoint(referencePoint, 30, (5 + 5 + 35)); JointPointList.Add(startPoint); // 8
            lineList.Add(eachLine = new Line(startPoint, GetSumPoint(startPoint, 0, -35))); JointPointList.Add(eachLine.EndPoint); // 9
            lineList.Add(eachLine = new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 5, 0))); JointPointList.Add(eachLine.EndPoint); // 10
            lineList.Add(eachLine = new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 0, 35))); JointPointList.Add(eachLine.EndPoint); // 11



            // Vertical Demention
            /*

             - LEFT Position -
            * t10
            JointPointList[3], JointPointList[7]

             - Rigth Position -
            # t10 (bottom)

            /**/



            // Spline Demention
            /*

            - Rigth Position -
            JointPointList[0], JointPointList[2]

            - Top Position -
            JointPointList[8], JointPointList[11]

            /**/



            // Horizontal Demention
            /*

            # BOTTOM PLATE O.D 6620
            JointPointList[3] -----------

            # 60
            JointPointList[7], JointPointList[9]

            # t8 SHELL I.D 6500
            JointPointList[9](y=[8]~[9]), JointPointList[10](y=[8]~[9])

            # 30(MIN.25)
            JointPointList[1], JointPointList[5]

            /**/



            // Diagonal Demention
            /*

            # 10 (left)
            JointPointList[5]
            JointPointList[4], JointPointList[6]

            /**/


            // Center Line : Red color
            //Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            //centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);

            styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(lineList);


            // Center Point
            Point3D modelCenterPoint = GetSumPoint(lineList[6].StartPoint, -22, 10);
            SetModelCenterPoint(PAPERSUB_TYPE.BottomPlateJointDetail, modelCenterPoint);




            // Dimension : Break
            double tempHeight = 5;
            List<Entity> breakAllList = new List<Entity>();
            breakAllList.AddRange(breakService.GetFlatBreakLine(GetSumPoint(lineList[2].EndPoint, 0, 0),
                                                            GetSumPoint(lineList[2].EndPoint, 0, -tempHeight),
                                                            scaleValue, true, true));

            styleService.SetLayerListEntity(ref breakAllList, layerService.LayerDimension);
            drawList.outlineList.AddRange(breakAllList);

            // S Line
            double sLineLength = tempHeight;

            List<Entity> newSLineList1 = breakService.GetSLine(GetSumPoint(lineList[10].EndPoint, 0, 0), sLineLength);
            styleService.SetLayerListEntity(ref newSLineList1, layerService.LayerDimension);
            drawList.outlineList.AddRange(newSLineList1);



            // Dimension : Arc
            //DrawEntityModel testDimArc01 = drawService.Draw_DimensionArc(GetSumPoint(leftTopLine.EndPoint, 0, 0), GetSumPoint(rightTopLine.StartPoint, 0, 0), "top", 12, "60˚", 45, 30, -30, 0, scaleValue, layerService.LayerDimension);
            //drawList.AddDrawEntity(testDimArc01);
            // Dimension : top

            string bottomThk = assemblyData.BottomInput[0].BottomPlateThickness;
            string bottomThkStr = "t" + bottomThk;
            string anchorTopPlateC = assemblyData.AnchorageInput[0].TopPlateC;


            DrawDimensionModel dimModel01 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = "30",
                textLower = "(MIN.25)",
                textSizeVisible = false,
                dimHeight = 13,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity01 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lineList[1].EndPoint, 0, 0), GetSumPoint(lineList[6].StartPoint, 0, 0), scaleValue, dimModel01);
            drawList.AddDrawEntity(dimEntity01);
            //DrawDimensionModel dimModel02 = new DrawDimensionModel()
            //{
            //    position = POSITION_TYPE.TOP,
            //    textUpper = publicFunctionService.GetDetailAnnularOverlap().ToString(),
            //    textLower = "(MIN.60)",
            //    textSizeVisible = false,
            //    dimHeight = 30,
            //    scaleValue = scaleValue
            //};
            //DrawEntityModel dimEntity02 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(verticalLineList[5].EndPoint, 0, 0), GetSumPoint(verticalLineList[3].EndPoint, 0, 0), scaleValue, dimModel02);
            //drawList.AddDrawEntity(dimEntity02);
            DrawDimensionModel dimModel03 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = anchorTopPlateC,
                extLineRightVisible = false,
                textSizeVisible = false,
                dimHeight = 13,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity03 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lineList[7].StartPoint, 0, 0), GetSumPoint(lineList[8].EndPoint, 0, 0), scaleValue, dimModel03);
            drawList.AddDrawEntity(dimEntity03);

            DrawDimensionModel dimModel04 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.BOTTOM,
                textUpper = "BOTTOM PLATE O.D " + assemblyData.BottomInput[0].OD,
                textSizeVisible = false,
                arrowRightHeadVisible=false,
                extLineRightVisible=false,
                dimHeight = 15,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity04 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lineList[3].StartPoint, 0, 0), GetSumPoint(lineList[2].EndPoint, 0, 0), scaleValue, dimModel04);
            drawList.AddDrawEntity(dimEntity04);
            //DrawDimensionModel dimModel05 = new DrawDimensionModel()
            //{
            //    position = POSITION_TYPE.BOTTOM,
            //    textUpper = "10",
            //    arrowLeftHeadOut = true,
            //    arrowRightHeadOut = true,
            //    textUpperPosition = POSITION_TYPE.LEFT,
            //    textSizeVisible = false,
            //    dimHeight = 15,
            //    scaleValue = scaleValue
            //};
            //DrawEntityModel dimEntity05 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(verticalLineList[0].StartPoint, 0, 0), GetSumPoint(verticalLineList[2].StartPoint, 0, 0), scaleValue, dimModel05);
            //drawList.AddDrawEntity(dimEntity05);


            //DrawDimensionModel dimModel06 = new DrawDimensionModel()
            //{
            //    position = POSITION_TYPE.TOP,
            //    textUpper = "60",
            //    extLineRightVisible = false,
            //    textSizeVisible = false,
            //    dimHeight = 30 + tempHeight,
            //    scaleValue = scaleValue
            //};
            //DrawEntityModel dimEntity06 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(verticalLineList[2].EndPoint, 0, 0), GetSumPoint(verticalLineList[7].StartPoint, 0, 0), scaleValue, dimModel06);
            //drawList.AddDrawEntity(dimEntity06);


            DrawDimensionModel dimModel07 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = "SHELL I.D " + assemblyData.GeneralDesignData[0].SizeNominalID,
                extLineRightVisible = false,
                arrowRightHeadVisible = false,
                textSizeVisible = false,
                dimHeight = 20 ,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity07 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lineList[10].StartPoint, 0, 0), GetSumPoint(lineList[2].EndPoint, 0, 0), scaleValue, dimModel07);
            drawList.AddDrawEntity(dimEntity07);


            DrawDimensionModel dimModel08 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = "t" + assemblyData.ShellOutput[0].Thickness,
                extLineLeftVisible = false,
                extLineRightVisible = false,
                arrowLeftHeadOut = true,
                arrowRightHeadVisible = false,
                textUpperPosition = POSITION_TYPE.LEFT,
                textSizeVisible = false,
                dimHeight = 20,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity08 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lineList[8].EndPoint, 0, 0), GetSumPoint(lineList[10].StartPoint, 0, 0), scaleValue, dimModel08);
            drawList.AddDrawEntity(dimEntity08);


            //DrawDimensionModel dimModel09 = new DrawDimensionModel()
            //{
            //    position = POSITION_TYPE.TOP,
            //    textUpper = "ANNULAR PLATE O.D " + assemblyData.BottomInput[0].AnnularPlateOD,
            //    extLineRightVisible = false,
            //    arrowRightHeadVisible = false,
            //    textSizeVisible = false,
            //    dimHeight = 30 + 16,
            //    scaleValue = scaleValue
            //};
            //DrawEntityModel dimEntity09 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(verticalLineList[2].EndPoint, 0, 0), GetSumPoint(verticalLineList[4].EndPoint, 0, 0), scaleValue, dimModel09);
            //drawList.AddDrawEntity(dimEntity09);



            //DrawDimensionModel dimModel02 = new DrawDimensionModel()
            //{
            //    position = POSITION_TYPE.RIGHT,
            //    textUpper = "t8",
            //    arrowLeftHeadOut = true,
            //    arrowRightHeadOut = true,
            //    arrowRightHeadVisible = false,
            //    textUpperPosition = POSITION_TYPE.LEFT,
            //    textSizeVisible = false,
            //    dimHeight = 12,
            //    scaleValue = scaleValue
            //};
            //DrawEntityModel dimEntity02 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lineList[0].StartPoint, 0, 0), GetSumPoint(lineList[2].EndPoint, 0, 0), scaleValue, dimModel02);
            //drawList.AddDrawEntity(dimEntity02);

            //DrawDimensionModel dimModel04 = new DrawDimensionModel()
            //{
            //    position = POSITION_TYPE.LEFT,
            //    textUpper = "t8",
            //    arrowLeftHeadOut = true,
            //    arrowRightHeadOut = true,
            //    textUpperPosition = POSITION_TYPE.RIGHT,
            //    textSizeVisible = false,
            //    dimHeight = 12,
            //    scaleValue = scaleValue
            //};
            //DrawEntityModel dimEntity04 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lineList[3].StartPoint, 0, 0), GetSumPoint(lineList[6].EndPoint, 0, 0), scaleValue, dimModel04);
            //drawList.AddDrawEntity(dimEntity04);


            //AngleSizeModel eachAngle = new AngleSizeModel();
            //if (assemblyData.RoofAngleOutput.Count > 0)
            //    eachAngle = assemblyData.RoofAngleOutput[0];
            //DrawDimensionModel dimModel03 = new DrawDimensionModel()
            //{
            //    position = POSITION_TYPE.LEFT,
            //    textUpper = "t" + eachAngle.t,
            //    extLineLeftVisible = false,
            //    extLineRightVisible = false,
            //    textSizeVisible = false,
            //    dimHeight = 6,
            //    scaleValue = scaleValue
            //};
            //DrawEntityModel dimEntity03 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(bottomLeftLine.EndPoint, 0, 0), GetSumPoint(leftTopLine.EndPoint, 0, 0), scaleValue, dimModel03);
            //drawList.AddDrawEntity(dimEntity03);

            // Dimension : Welding
            DrawWeldSymbols weldSymbols = new DrawWeldSymbols();
            List<Entity> weldList = new List<Entity>();
            DrawWeldSymbolModel wsModel01 = new DrawWeldSymbolModel();
            wsModel01.position = ORIENTATION_TYPE.TOPRIGHT;
            wsModel01.weldTypeDown = WeldSymbol_Type.Fillet;
            wsModel01.weldDetailType = WeldSymbolDetail_Type.ArrowSide;
            wsModel01.weldSize1 = bottomThk;
            wsModel01.tailVisible = false;
            wsModel01.leaderAngle = 60;
            wsModel01.leaderLineLength = 15;
            weldList.AddRange(weldSymbols.GetWeldSymbol(GetSumPoint(lineList[5].MidPoint, 0, 0), singleModel, scaleValue, wsModel01));



            styleService.SetLayerListEntity(ref weldList, layerService.LayerDimension);
            drawList.outlineList.AddRange(weldList);

            return drawList;
        }

        public DrawEntityModel DrawBottomPlateWeldingDetailBB(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue,PaperAreaModel selPaperAreaModel)
        {

            DrawEntityModel drawList = new DrawEntityModel();


            // SECTION "B"-"B"


            //singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Line> lineList = new List<Line>();

            List<Point3D> JointPointList = new List<Point3D>();


            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);


            /////////////////////////////
            //      Information      ////
            /////////////////////////////



            // setting Lines1
            Line eachLine = null;
            Point3D startPoint = referencePoint;

            // Bottom Lines(Lectangle)
            startPoint = GetSumPoint(referencePoint, (50 + 55 - 20), 0); /* Point extrect /**/ JointPointList.Add(startPoint); // 0
            lineList.Add(eachLine = new Line(startPoint, GetSumPoint(startPoint, -55, 0)));
            lineList.Add(eachLine = new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 0, 5))); JointPointList.Add(eachLine.EndPoint); // 1
            lineList.Add(new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 55, 0))); JointPointList.Add(eachLine.EndPoint); // 2

            // Mid Lines(Lectangle)
            startPoint = GetSumPoint(referencePoint, 0, 5); JointPointList.Add(startPoint); // 3
            lineList.Add(eachLine = new Line(startPoint, GetSumPoint(startPoint, 50, 0))); JointPointList.Add(eachLine.EndPoint); // 4
            lineList.Add(eachLine = new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 0, 5))); JointPointList.Add(eachLine.EndPoint); // 5
            lineList.Add(new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 5, -5)));  /* diagonal Line /**/ JointPointList.Add(GetSumPoint(eachLine.EndPoint, 5, -5)); // 6
            lineList.Add(eachLine = new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, -50, 0))); JointPointList.Add(eachLine.EndPoint); // 7


            //// Bottom Lines(Lectangle)
            //Point3D tPoint01 = GetSumPoint(referencePoint, (50 + 55 - 20), 0);
            //Line tLine01 = new Line(GetSumPoint(startPoint,0,0), GetSumPoint(startPoint, -55, 0));
            //Line tLine02 = new Line(GetSumPoint(tLine01.EndPoint, 0, 0), GetSumPoint(tLine01.EndPoint, 0, 5));
            //Line tLine03 = new Line(GetSumPoint(tLine02.EndPoint, 0, 0), GetSumPoint(tLine02.EndPoint, 55, 0));

            //lineList.AddRange(new Line[] { tLine01, tLine02, tLine03 });


            //// Mid Lines(Lectangle)
            //startPoint = GetSumPoint(referencePoint, 0, 5); 
            //lineList.Add(eachLine = new Line(startPoint, GetSumPoint(startPoint, 50, 0)));
            //lineList.Add(eachLine = new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 0, 5))); 
            //lineList.Add(new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 5, -5)));
            //lineList.Add(eachLine = new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, -50, 0)));



            //JointPointList.Add(startPoint); // 0
            //JointPointList.Add(tLine01.StartPoint); // 1
            //JointPointList.Add(tLine01.EndPoint); // 1



            // Spline Demention
            /*

            - Left Position -
            JointPointList[3], JointPointList[7]

            - Rigth Position -
            JointPointList[0], JointPointList[2]

            /**/


            // Horizontal Demention
            /*

            # 26(MIN.)
            JointPointList[1], JointPointList[5]

            /**/


            // Diagonal Demention
            /*

            # 8 (mid)
            JointPointList[5]
            JointPointList[4], JointPointList[6]

            /**/


            // Center Line : Red color
            //Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            //centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);

            drawList.outlineList.AddRange(lineList);


            // Center Point
            Point3D modelCenterPoint = GetSumPoint(eachLine.StartPoint, 0, 10);
            selPaperAreaModel.ModelCenterLocation.X = modelCenterPoint.X;
            selPaperAreaModel.ModelCenterLocation.Y = modelCenterPoint.Y;




            // Dimension : Break
            double tempHeight = 5;
            List<Entity> breakAllList = new List<Entity>();
            breakAllList.AddRange(breakService.GetFlatBreakLine(GetSumPoint(lineList[2].EndPoint, 0, 0),
                                                            GetSumPoint(lineList[2].EndPoint, 0, -tempHeight),
                                                            scaleValue, true, true));
            breakAllList.AddRange(breakService.GetFlatBreakLine(GetSumPoint(lineList[6].EndPoint, 0, 0),
                                                            GetSumPoint(lineList[6].EndPoint, 0, -tempHeight),
                                                            scaleValue, true, true));
           
            styleService.SetLayerListEntity(ref breakAllList, layerService.LayerDimension);
            drawList.outlineList.AddRange(breakAllList);


            // Dimension : Arc
            //DrawEntityModel testDimArc01 = drawService.Draw_DimensionArc(GetSumPoint(leftTopLine.EndPoint, 0, 0), GetSumPoint(rightTopLine.StartPoint, 0, 0), "top", 12, "60˚", 45, 30, -30, 0, scaleValue, layerService.LayerDimension);
            //drawList.AddDrawEntity(testDimArc01);
            // Dimension : top

            string bottomThk= assemblyData.BottomInput[0].BottomPlateThickness; 
            string bottomThkStr = "t" + bottomThk;

            DrawDimensionModel dimModel01 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = "30",
                textLower = "(MIN.25)",
                textSizeVisible = false,
                dimHeight = 15,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity01 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lineList[1].EndPoint, 0, 0), GetSumPoint(lineList[4].EndPoint, 0, 0), scaleValue, dimModel01);
            drawList.AddDrawEntity(dimEntity01);
            DrawDimensionModel dimModel02 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.RIGHT,
                textUpper = bottomThkStr,
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                arrowRightHeadVisible = false,
                textUpperPosition = POSITION_TYPE.LEFT,
                textSizeVisible = false,
                dimHeight = 12,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity02 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lineList[0].StartPoint, 0, 0), GetSumPoint(lineList[2].EndPoint, 0, 0), scaleValue, dimModel02);
            drawList.AddDrawEntity(dimEntity02);

            DrawDimensionModel dimModel04 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.LEFT,
                textUpper = bottomThkStr,
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                textUpperPosition = POSITION_TYPE.RIGHT,
                textSizeVisible = false,
                dimHeight = 12,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity04 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lineList[3].StartPoint, 0, 0), GetSumPoint(lineList[6].EndPoint, 0, 0), scaleValue, dimModel04);
            drawList.AddDrawEntity(dimEntity04);


            //AngleSizeModel eachAngle = new AngleSizeModel();
            //if (assemblyData.RoofAngleOutput.Count > 0)
            //    eachAngle = assemblyData.RoofAngleOutput[0];
            //DrawDimensionModel dimModel03 = new DrawDimensionModel()
            //{
            //    position = POSITION_TYPE.LEFT,
            //    textUpper = "t" + eachAngle.t,
            //    extLineLeftVisible = false,
            //    extLineRightVisible = false,
            //    textSizeVisible = false,
            //    dimHeight = 6,
            //    scaleValue = scaleValue
            //};
            //DrawEntityModel dimEntity03 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(bottomLeftLine.EndPoint, 0, 0), GetSumPoint(leftTopLine.EndPoint, 0, 0), scaleValue, dimModel03);
            //drawList.AddDrawEntity(dimEntity03);

            // Dimension : Welding
            DrawWeldSymbols weldSymbols = new DrawWeldSymbols();
            List<Entity> weldList = new List<Entity>();
            DrawWeldSymbolModel wsModel01 = new DrawWeldSymbolModel();
            wsModel01.position = ORIENTATION_TYPE.TOPRIGHT;
            wsModel01.weldTypeDown = WeldSymbol_Type.Fillet;
            wsModel01.weldDetailType = WeldSymbolDetail_Type.ArrowSide;
            wsModel01.weldSize1 = bottomThk;
            wsModel01.tailVisible = false;
            wsModel01.leaderAngle = 60;
            wsModel01.leaderLineLength = 15;
            weldList.AddRange(weldSymbols.GetWeldSymbol(GetSumPoint(lineList[5].MidPoint, 0, 0), singleModel, scaleValue, wsModel01));


            styleService.SetLayerListEntity(ref weldList, layerService.LayerDimension);
            drawList.outlineList.AddRange(weldList);


            //DrawBMLeaderModel leaderInfoModel = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = "HAMMERING", bmNumber = "", textAlign = POSITION_TYPE.CENTER, leaderPointLength = 12, leaderPointRadian = Utility.DegToRad(45) };
            //DrawEntityModel leaderInfoList = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(lineList[6].MidPoint, 0, 0), leaderInfoModel, scaleValue);
            //drawList.AddDrawEntity(leaderInfoList);




            return drawList;

        }

        public DrawEntityModel DrawBottomPlateWeldingDetailC(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue,PaperAreaModel selPaperAreaModel)
        {

            DrawEntityModel drawList = new DrawEntityModel();

            // Detail "C"

            singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Entity> splineList = new List<Entity>();
            List<Line> lineList = new List<Line>();

            /////////////////////////////
            //      Information      ////
            /////////////////////////////
            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);
            double distanceYofRef = 30;  // Start Point of TopView : RefPoint 에서의 Y축으로 떨어진거리

            Line righthLine = null;
            Line leftLine = null;


            lineList.Add(new Line(referencePoint, GetSumPoint(referencePoint, 105, 0)));  // 0
            lineList.Add(new Line(GetSumPoint(referencePoint, 0, 5), GetSumPoint(referencePoint, 105, 5)));  // 1
            lineList.Add(righthLine = new Line(GetSumPoint(referencePoint, 15 + 30, 10), GetSumPoint(referencePoint, 15 + 30 + 20 + 25, 10)));  // 2
            lineList.Add(new Line(GetSumPoint(referencePoint, 15 + 30, 10), GetSumPoint(referencePoint, 15 + 30, 5)));  // 3
            lineList.Add(new Line(GetSumPoint(referencePoint, 15 + 30, 10), GetSumPoint(referencePoint, 15 + 30 - 5, 5)));  // 4
            lineList.Add(new Line(GetSumPoint(referencePoint, 15 + 30 + 20, 15), GetSumPoint(referencePoint, 15 + 30 + 20, 10)));  // 5
            lineList.Add(new Line(GetSumPoint(referencePoint, 15 + 30 + 20, 15), GetSumPoint(referencePoint, 15 + 30 + 20 + 5, 10)));  // 6


            // offset End Point : intersect Check
            Point3D startPoint = GetSumPoint(referencePoint, 15 + 30, 10);
            Line offsetTestLine = new Line(GetSumPoint(startPoint, -50, 0), GetSumPoint(startPoint, 50, 0));
            offsetTestLine.Rotate((Utility.DegToRad(30)), Vector3D.AxisZ, startPoint);

            leftLine = new Line(GetSumPoint(referencePoint, 15, 5), GetSumPoint(referencePoint, 15 + 30, 5));

            // offset Reference Line
            Line offsetEachLine = GetInterSectLine_withTwoLines(leftLine, offsetTestLine, righthLine);
            lineList.Add(offsetEachLine); // 7

            lineList.Add(new Line(GetSumPoint(referencePoint, 15, 5), offsetEachLine.StartPoint));  // 8


            leftLine = new Line(GetSumPoint(referencePoint, 15, 10), GetSumPoint(referencePoint, 15 + 30, 10));  // 10(가상선)
            righthLine = new Line(GetSumPoint(referencePoint, 15 + 30 + 20, 15), GetSumPoint(referencePoint, 0, 15));  // 11(가상선)
            Line tempoffsetLine = (Line)offsetTestLine.Offset(-5, Vector3D.AxisZ); // offset 가상선

            // offset Line
            lineList.Add(offsetEachLine = GetInterSectLine_withTwoLines(leftLine, tempoffsetLine, righthLine)); // 9

            lineList.Add(new Line(GetSumPoint(referencePoint, 15, 10), offsetEachLine.StartPoint));  // 10
            lineList.Add(new Line(offsetEachLine.EndPoint, GetSumPoint(referencePoint, 15 + 30 + 20, 15)));  // 11

            lineList.Add(righthLine = new Line(GetSumPoint(referencePoint, 15 + 30, 5), GetSumPoint(referencePoint, 15 + 30 + 20 + 25, 5)));  // 12
            /**/



            //////////////////////////////
            // Draw : Top View 
            //////////////////////////////

            List<Entity> topViewLineList = new List<Entity>();
            List<Line> topViewHiddenLineList = new List<Line>();


            Point3D topViewStartPoint = GetSumPoint(referencePoint, 0, distanceYofRef);

            //* bottom Spline *//
            List<Point3D> bottomPointList = new List<Point3D>();

            List<Line> bottomLineList = GetRectangle(topViewStartPoint, 35, 105);
            Line bLineBottom = bottomLineList[0];
            Line bLineRight = bottomLineList[1];
            Line bLineTop = bottomLineList[2];
            Line bLineLeft = bottomLineList[3];

            Line bOutLineLeft = new Line(GetSumPoint(bLineLeft.StartPoint, -2, 0), GetSumPoint(bLineLeft.EndPoint, -2, 0));
            Line bOutLineRight = new Line(GetSumPoint(bLineRight.StartPoint, 2, 0), GetSumPoint(bLineRight.EndPoint, 2, 0));

            Line bOutLineBottom = new Line(GetSumPoint(bLineBottom.StartPoint, 0, -2), GetSumPoint(bLineBottom.EndPoint, 0, -2));
            Line bInLineBottom = new Line(GetSumPoint(bLineBottom.StartPoint, 2, 2), GetSumPoint(bLineBottom.EndPoint, -2, 2));

            /*
            lineList.AddRange(new Line[]{
                bLineBottom, bLineRight,bLineTop, bLineLeft,
                bOutLineLeft, bOutLineRight, bOutLineBottom, bInLineBottom
            });
            /**/

            // spline setting
            double distanceYPoint = (bOutLineLeft.StartPoint.Y - bOutLineLeft.MidPoint.Y) / 2;
            double distanceXPoint = (bOutLineBottom.MidPoint.X - bOutLineBottom.StartPoint.X) / 2;

            bottomPointList.Add(bLineLeft.StartPoint);
            bottomPointList.Add(GetSumPoint(bOutLineLeft.StartPoint, 0, -distanceYPoint));
            bottomPointList.Add(bLineLeft.MidPoint);
            bottomPointList.Add(GetSumPoint(bOutLineLeft.EndPoint, 0, distanceYPoint));
            bottomPointList.Add(bInLineBottom.StartPoint);
            bottomPointList.Add(GetSumPoint(bOutLineBottom.StartPoint, distanceXPoint, 0));
            bottomPointList.Add(bLineBottom.MidPoint);
            bottomPointList.Add(GetSumPoint(bOutLineBottom.EndPoint, -distanceXPoint, 0));
            bottomPointList.Add(bInLineBottom.EndPoint);
            bottomPointList.Add(GetSumPoint(bOutLineRight.EndPoint, 0, +distanceYPoint));
            bottomPointList.Add(bLineRight.MidPoint);
            bottomPointList.Add(GetSumPoint(bOutLineRight.StartPoint, 0, -distanceYPoint));
            bottomPointList.Add(bLineRight.StartPoint);

            Curve bottomSpline = Curve.CubicSplineInterpolation(bottomPointList);
            splineList.Add(bottomSpline);



            //* Left Spline *//
            List<Point3D> leftPointList = new List<Point3D>();

            Point3D leftStartPoint = GetSumPoint(bLineTop.StartPoint, 15, -20);
            List<Line> leftLineList = GetRectangle(leftStartPoint, 45, 50);
            Line leftLineBottom = leftLineList[0];
            Line leftLineRight = leftLineList[1];
            Line leftLineTop = leftLineList[2];
            Line leftLinetLeft = leftLineList[3];

            Point3D leftInLinePoint = GetSumPoint(leftLineTop.StartPoint, 2, -2);
            Point3D[] intersectMidPoint = bLineTop.IntersectWith(leftLineRight);

            // reset Right/bottom line
            leftLineRight.EndPoint = intersectMidPoint[0];
            leftLineBottom.EndPoint = GetSumPoint(leftLineBottom.EndPoint, -20, 0);

            // diagonal Line
            Line diagonalLeftLine = new Line(leftLineRight.EndPoint, leftLineBottom.EndPoint);

            topViewLineList.AddRange(new Line[]{
                                                leftLineBottom, leftLineRight,
                                                diagonalLeftLine
                                                });
            /**/

            // spline setting
            distanceYPoint = distanceXPoint = 0;
            distanceYPoint = (leftLinetLeft.StartPoint.Y - leftLinetLeft.EndPoint.Y) / 4;
            distanceXPoint = (leftLineTop.EndPoint.X - leftLineTop.StartPoint.X) / 4;


            leftPointList.Add(leftLineTop.EndPoint);
            leftPointList.Add(GetSumPoint(leftLineTop.EndPoint, -distanceXPoint, 2));
            leftPointList.Add(leftLineTop.MidPoint);
            leftPointList.Add(GetSumPoint(leftLineTop.StartPoint, distanceXPoint, 2));
            leftPointList.Add(leftInLinePoint);  // left top
            leftPointList.Add(GetSumPoint(leftLinetLeft.StartPoint, -2, -distanceYPoint));
            leftPointList.Add(leftLinetLeft.MidPoint);  // left mid
            leftPointList.Add(GetSumPoint(leftLinetLeft.EndPoint, -2, distanceYPoint));
            leftPointList.Add(leftLinetLeft.EndPoint);  // left bottom

            Curve leftSpline = Curve.CubicSplineInterpolation(leftPointList);
            splineList.Add(leftSpline);
            /**/


            Point3D[] intersectLeftPoint = bLineTop.IntersectWith(leftSpline); // Intersection : top line of bottomPlate & leftSpline of leftPlate
            topViewLineList.Add(new Line(bLineTop.StartPoint, intersectLeftPoint[0])); // Draw : topLeft Line of bottom plate



            //* Right Spline *//
            List<Point3D> rightPointList = new List<Point3D>();

            Point3D rightStartPoint = GetSumPoint(leftLineBottom.EndPoint, 0, 0);
            List<Line> rightLineList = GetRectangle(rightStartPoint, 55, 45);
            Line rightLineBottom = rightLineList[0];
            Line rightLineRight = rightLineList[1];
            Line rightLineTop = rightLineList[2];
            Line rightLinetLeft = rightLineList[3];

            Point3D rightInLinePoint = GetSumPoint(rightLineTop.EndPoint, -2, -2);
            Point3D[] intersectTopPoint = rightLinetLeft.IntersectWith(leftSpline); // left Line of rightPlate & top Spline of leftPlate

            // reset left line
            rightLinetLeft.EndPoint = intersectTopPoint[0];

            // Draw rectangle line (Left/bottom)
            topViewLineList.AddRange(new Line[]{
                                                rightLinetLeft,rightLineBottom
                                                });
            /**/

            // spline setting
            distanceYPoint = distanceXPoint = 0;
            distanceYPoint = (rightLineRight.StartPoint.Y - rightLineRight.EndPoint.Y) / 4;
            distanceXPoint = (rightLineTop.EndPoint.X - rightLineTop.StartPoint.X) / 4;

            rightPointList.Add(rightLineTop.StartPoint);
            rightPointList.Add(GetSumPoint(rightLineTop.StartPoint, distanceXPoint, 2));
            rightPointList.Add(rightLineTop.MidPoint);
            rightPointList.Add(GetSumPoint(rightLineTop.EndPoint, -distanceXPoint, 2));
            rightPointList.Add(rightInLinePoint);  // right top
            rightPointList.Add(GetSumPoint(rightLineRight.StartPoint, 2, -distanceYPoint));
            rightPointList.Add(rightLineRight.MidPoint);  // right mid
            rightPointList.Add(GetSumPoint(rightLineRight.EndPoint, 2, distanceYPoint));
            rightPointList.Add(GetSumPoint(rightLineRight.EndPoint, 0, 0));  // right bottom

            Curve rightSpline = Curve.CubicSplineInterpolation(rightPointList);
            splineList.Add(rightSpline);
            /**/


            Point3D[] intersectRightPoint = bLineTop.IntersectWith(rightSpline); // Intersection : top line of bottom Plate & rightSpline of rightPlate
            topViewLineList.Add(new Line(intersectRightPoint[0], bLineTop.EndPoint)); // Draw : topRight Line of bottom plate


            // Draw : hidden Line
            topViewHiddenLineList.AddRange(new Line[] {
                                                        new Line(intersectLeftPoint[0],intersectMidPoint[0]),
                                                        new Line(intersectMidPoint[0],intersectRightPoint[0]),
                                                        new Line(intersectTopPoint[0],leftLineBottom.EndPoint)
                                                        });


            // Spline Demention
            /*
            - Left Position -
            lineList[0].StartPoint, lineList[1].StartPoint
            lineList[8].StartPoint, lineList[10].StartPoint

            - Rigth Position -
            lineList[0].EndPoint, lineList[1].EndPoint
            lineList[12].EndPoint, lineList[2].EndPoint

            /**/


            // Horizontal Demention
            /*
            - Top Position -
            // 26 (MIN.)
            lineList[2].StartPoint, lineList[11].EndPoint
            /**/


            // Vertical Demention
            /*
            - Left Position -
            lineList[1].StartPoint, lineList[10].StartPoint


            - Rigth Position -

            // bottom t8
            lineList[0].EndPoint, lineList[1].EndPoint
            // top t8
            lineList[1].EndPoint, lineList[2].EndPoint

            /**/


            // Diagonal Demention

            /*
            # (Left) triangle
                                  lineList[3].StartPoint
            lineList[4].EndPoint, lineList[3].EndPoint

            # (Right) triangle
            lineList[5].StartPoint
            lineList[5].EndPoint, lineList[6].EndPoint

            // HAMMERING
            lineList[9].MidPoint

            /**/

            // Center Line : Red color
            //Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            //centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            styleService.SetLayerListEntity(ref splineList, layerService.LayerDimension);
            styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);
            styleService.SetLayerListLine(ref topViewLineList, layerService.LayerOutLine);
            styleService.SetLayerListLine(ref topViewHiddenLineList, layerService.LayerHiddenLine);

            singleModel.Entities.AddRange(centerLineList);
            singleModel.Entities.AddRange(lineList);
            singleModel.Entities.AddRange(topViewLineList);
            singleModel.Entities.AddRange(topViewHiddenLineList);
            singleModel.Entities.Add(bottomSpline);
            singleModel.Entities.Add(leftSpline);
            singleModel.Entities.Add(rightSpline);

            drawList.outlineList.AddRange(splineList);
            drawList.outlineList.AddRange(lineList);
            drawList.outlineList.AddRange(topViewLineList);
            drawList.outlineList.AddRange(topViewHiddenLineList);
            drawList.outlineList.Add(bottomSpline);
            drawList.outlineList.Add(leftSpline);
            drawList.outlineList.Add(rightSpline);


            // Center Point
            Point3D modelCenterPoint = GetSumPoint(rightLineBottom.EndPoint, -37.5, 0);
            selPaperAreaModel.ModelCenterLocation.X = modelCenterPoint.X;
            selPaperAreaModel.ModelCenterLocation.Y = modelCenterPoint.Y;







            // Dimension : Break
            double tempHeight = 5;
            List<Entity> breakAllList = new List<Entity>();
            breakAllList.AddRange( breakService.GetFlatBreakLine(GetSumPoint(lineList[10].StartPoint, 0, 0),
                                                            GetSumPoint(lineList[10].StartPoint, 0, -tempHeight),
                                                            scaleValue,true,false));
            breakAllList.AddRange(breakService.GetFlatBreakLine(GetSumPoint(lineList[1].StartPoint, 0, 0),
                                                            GetSumPoint(lineList[1].StartPoint, 0, -tempHeight),
                                                            scaleValue));
            breakAllList.AddRange(breakService.GetFlatBreakLine(GetSumPoint(lineList[1].EndPoint, 0, 0),
                                                            GetSumPoint(lineList[1].EndPoint, 0, -tempHeight),
                                                            scaleValue));
            breakAllList.AddRange(breakService.GetFlatBreakLine(GetSumPoint(lineList[2].EndPoint, 0, 0),
                                                            GetSumPoint(lineList[2].EndPoint, 0, -tempHeight),
                                                            scaleValue,true,false));
            styleService.SetLayerListEntity(ref breakAllList, layerService.LayerDimension);
            drawList.outlineList.AddRange(breakAllList);


            // Dimension : Arc
            //DrawEntityModel testDimArc01 = drawService.Draw_DimensionArc(GetSumPoint(leftTopLine.EndPoint, 0, 0), GetSumPoint(rightTopLine.StartPoint, 0, 0), "top", 12, "60˚", 45, 30, -30, 0, scaleValue, layerService.LayerDimension);
            //drawList.AddDrawEntity(testDimArc01);
            // Dimension : top
            string bottomThk = "t" + assemblyData.BottomInput[0].BottomPlateThickness;

            DrawDimensionModel dimModel01 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = "30",
                textLower = "(MIN.25)",
                textSizeVisible = false,
                dimHeight = 8,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity01 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lineList[2].StartPoint, 0, 0), GetSumPoint(lineList[11].EndPoint, 0, 0), scaleValue, dimModel01);
            drawList.AddDrawEntity(dimEntity01);
            DrawDimensionModel dimModel02 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.RIGHT,
                textUpper = bottomThk,
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                arrowLeftHeadVisible = false,
                textUpperPosition = POSITION_TYPE.RIGHT,
                textSizeVisible = false,
                dimHeight = 12,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity02 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lineList[1].EndPoint, 0, 0), GetSumPoint(lineList[2].EndPoint, 0, 0), scaleValue, dimModel02);
            drawList.AddDrawEntity(dimEntity02);
            DrawDimensionModel dimModel03 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.RIGHT,
                textUpper = bottomThk,
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                arrowRightHeadVisible = false,
                textUpperPosition = POSITION_TYPE.LEFT,
                textSizeVisible = false,
                dimHeight = 12,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity03 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lineList[0].EndPoint, 0, 0), GetSumPoint(lineList[1].EndPoint, 0, 0), scaleValue, dimModel03);
            drawList.AddDrawEntity(dimEntity03);
            DrawDimensionModel dimModel04 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.LEFT,
                textUpper = bottomThk,
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                textUpperPosition = POSITION_TYPE.RIGHT,
                textSizeVisible = false,
                dimHeight = 12,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity04 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lineList[1].StartPoint, 0, 0), GetSumPoint(lineList[10].StartPoint, 0, 0), scaleValue, dimModel04);
            drawList.AddDrawEntity(dimEntity04);


            //AngleSizeModel eachAngle = new AngleSizeModel();
            //if (assemblyData.RoofAngleOutput.Count > 0)
            //    eachAngle = assemblyData.RoofAngleOutput[0];
            //DrawDimensionModel dimModel03 = new DrawDimensionModel()
            //{
            //    position = POSITION_TYPE.LEFT,
            //    textUpper = "t" + eachAngle.t,
            //    extLineLeftVisible = false,
            //    extLineRightVisible = false,
            //    textSizeVisible = false,
            //    dimHeight = 6,
            //    scaleValue = scaleValue
            //};
            //DrawEntityModel dimEntity03 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(bottomLeftLine.EndPoint, 0, 0), GetSumPoint(leftTopLine.EndPoint, 0, 0), scaleValue, dimModel03);
            //drawList.AddDrawEntity(dimEntity03);

            // Dimension : Welding
            //DrawWeldSymbols weldSymbols = new DrawWeldSymbols();
            //List<Entity> weldList = new List<Entity>();
            //DrawWeldSymbolModel wsModel01 = new DrawWeldSymbolModel();
            //wsModel01.position = ORIENTATION_TYPE.TOPRIGHT;
            //wsModel01.weldTypeUp = WeldSymbol_Type.Square;
            //wsModel01.weldTypeDown = WeldSymbol_Type.V;
            //wsModel01.weldDetailType = WeldSymbolDetail_Type.BothSide;
            //wsModel01.weldFaceDown = WeldFace_Type.Convex;
            //wsModel01.tailVisible = true;
            //wsModel01.specification = "B.G";
            //wsModel01.leaderAngle = 60;
            //wsModel01.leaderLineLength = 18;
            //weldList.AddRange(weldSymbols.GetWeldSymbol(GetSumPoint(arcTop.MidPoint, 0, 0), singleModel, scaleValue, wsModel01));


            //styleService.SetLayerListEntity(ref weldList, layerService.LayerDimension);
            //drawList.outlineList.AddRange(weldList);


            DrawBMLeaderModel leaderInfoModel = new DrawBMLeaderModel() { position = POSITION_TYPE.LEFT, upperText = "HAMMERING", bmNumber = "", textAlign = POSITION_TYPE.CENTER,leaderPointLength=12,leaderPointRadian=Utility.DegToRad(135) };
            DrawEntityModel leaderInfoList = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(lineList[9].MidPoint, 0, 0), leaderInfoModel, scaleValue);
            drawList.AddDrawEntity(leaderInfoList);

            //DrawBMLeaderModel leaderInfoModel2 = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = "INSIDE", bmNumber = "", textAlign = POSITION_TYPE.CENTER, arrowHeadVisible = false };
            //DrawEntityModel leaderInfoList2 = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(bottomRightLine.EndPoint, 1, 0), leaderInfoModel2, scaleValue);
            //drawList.AddDrawEntity(leaderInfoList2);





            return drawList;
        }

        public DrawEntityModel DrawBottomPlateWeldingDetailD(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue, PaperAreaModel selPaperAreaModel)
        {
            DrawEntityModel drawList = new DrawEntityModel();


            // // Detail "D"

            //singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Entity> splineList = new List<Entity>();
            List<Line> lineList = new List<Line>();

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            double distanceYofRef = 30;  // Start Point of TopView : RefPoint 에서의 Y축으로 떨어진거리


            // bottom Plate
            Point3D bottomStartPoint = GetSumPoint(referencePoint, 60 - 20, 0);
            List<Line> bottomPLineList = GetRectangle(bottomStartPoint, 5, 65, RECTANGLUNVIEW.RIGHT);
            Line bPlateBottom = bottomPLineList[0];
            Line bPlateTop = bottomPLineList[1];
            Line bPlateLeft = bottomPLineList[2];

            // Mid Plate
            Point3D midStartPoint = GetSumPoint(referencePoint, 0, 5);
            List<Line> midLineList = GetRectangle(midStartPoint, 5, 60, RECTANGLUNVIEW.LEFT);
            Line mPlateBottom = midLineList[0];
            Line mPlateRight = midLineList[1];
            Line mPlateTop = midLineList[2];

            // Diagonal Line (triangle)
            Point3D diagonalStartPoint = GetSumPoint(mPlateBottom.EndPoint, 5, 0);
            Line diagonalLine = new Line(diagonalStartPoint, mPlateTop.EndPoint);
            // bottom LeftPoint : mPlateBottom.EndPoint



            // Top Plate

            // diagonal of topPlate
            Point3D rotatePoint = mPlateTop.EndPoint;
            Line tPlateIntersectLine1 = new Line(GetSumPoint(referencePoint, 0, 10), GetSumPoint(referencePoint, 100, 10));
            Line tPlateIntersectLine2 = (Line)tPlateIntersectLine1.Offset(-5, Vector3D.AxisZ);
            tPlateIntersectLine1.Rotate(Utility.DegToRad(-30), Vector3D.AxisZ, rotatePoint);
            tPlateIntersectLine2.Rotate(Utility.DegToRad(-30), Vector3D.AxisZ, rotatePoint);

            List<Line> tPlateLeftLine = new List<Line>();
            List<Line> tPlateRightLine = new List<Line>();
            Point3D tPlateLeftStartPoint = GetSumPoint(referencePoint, 15, 10);
            tPlateLeftLine = GetRectangle(tPlateLeftStartPoint, 5, 100);  // EndPoint = 100 임의로 연장한 값 (Intersect 하기 위함)
                                                                          // top Plate Left Lines
            Line tPlateLeftBottom = tPlateLeftLine[0];
            Line tPlateLeftTop = tPlateLeftLine[2];

            Point3D tPlateRightStartPoint = GetSumPoint(referencePoint, 0, 5);// StartPoint = 0 임의로 연장한 값 (Intersect 하기 위함)
            tPlateRightLine = GetRectangle(tPlateRightStartPoint, 5, 60 + 30);
            // top Plate Right Lines
            Line tPlateRightBottom = tPlateRightLine[0];
            Line tPlateRightTop = tPlateRightLine[2];

            // Intersect Lines
            Line tPlateDiagonalLine1 = GetInterSectLine_withTwoLines(tPlateLeftBottom, tPlateIntersectLine1, tPlateRightBottom);
            Line tPlateDiagonalLine2 = GetInterSectLine_withTwoLines(tPlateLeftTop, tPlateIntersectLine2, tPlateRightTop);

            // reset Top plate Line Point
            tPlateLeftBottom.EndPoint = tPlateDiagonalLine1.StartPoint;
            tPlateLeftTop.EndPoint = tPlateDiagonalLine2.StartPoint;

            tPlateRightBottom.StartPoint = tPlateDiagonalLine1.EndPoint;
            tPlateRightTop.StartPoint = tPlateDiagonalLine2.EndPoint;




            lineList.AddRange(new Line[]{
                    bPlateBottom, bPlateTop, bPlateLeft,
                    mPlateBottom, mPlateRight, mPlateTop,
                    tPlateLeftBottom, tPlateLeftTop, tPlateRightBottom, tPlateRightTop,
                    tPlateDiagonalLine1, tPlateDiagonalLine2,
                    diagonalLine,
                    });



            //////////////////////////////
            // Draw : Top View 
            //////////////////////////////

            List<Line> topViewLineList = new List<Line>();
            List<Line> topViewHiddenLineList = new List<Line>();


            Point3D topViewStartPoint = GetSumPoint(referencePoint, 0, distanceYofRef);

            //* top Spline */

            List<Point3D> topPointList = new List<Point3D>();

            Point3D topStartPoint = GetSumPoint(topViewStartPoint, 15, 30);
            List<Line> topLineList = GetRectangle(topStartPoint, 35, 75);
            Line tLineBottom = topLineList[0];
            Line tLineRight = topLineList[1];
            Line tLineTop = topLineList[2];
            Line tLineLeft = topLineList[3];

            lineList.AddRange(new Line[]{
                tLineBottom,
            });

            Point3D tPlateLeftInPoint = GetSumPoint(tLineLeft.StartPoint, 2, -2);
            Point3D tPlateRightInPoint = GetSumPoint(tLineRight.StartPoint, -2, -2);

            // spline point setting
            double distanceYPoint = (tLineLeft.StartPoint.Y - tLineLeft.EndPoint.Y) / 4;
            double distanceXPoint = (tLineBottom.EndPoint.X - tLineBottom.StartPoint.X) / 4;

            topPointList.Add(tLineBottom.StartPoint);
            topPointList.Add(GetSumPoint(tLineBottom.StartPoint, -2, distanceYPoint));
            topPointList.Add(tLineLeft.MidPoint); // left mid
            topPointList.Add(GetSumPoint(tLineLeft.StartPoint, -2, -distanceYPoint));
            topPointList.Add(tPlateLeftInPoint); // top left
            topPointList.Add(GetSumPoint(tLineTop.StartPoint, distanceXPoint, 2));
            topPointList.Add(tLineTop.MidPoint); // top mid
            topPointList.Add(GetSumPoint(tLineTop.EndPoint, -distanceXPoint, 2));
            topPointList.Add(tPlateRightInPoint); // top right
            topPointList.Add(GetSumPoint(tLineRight.StartPoint, 2, -distanceYPoint));
            topPointList.Add(tLineRight.MidPoint);
            topPointList.Add(GetSumPoint(tLineRight.EndPoint, 2, distanceYPoint));
            topPointList.Add(tLineBottom.EndPoint);

            Curve topSpline = Curve.CubicSplineInterpolation(topPointList);
            splineList.Add(topSpline);



            //@ Left Spline @/

            List<Point3D> leftPointList = new List<Point3D>();

            Point3D leftStartPoint = GetSumPoint(topViewStartPoint, 0, 50 - 40);
            List<Line> leftLineList = GetRectangle(leftStartPoint, 40, 60);
            Line leftLineBottom = leftLineList[0];
            Line leftLineRight = leftLineList[1];
            Line leftLineTop = leftLineList[2];
            Line leftLinetLeft = leftLineList[3];

            Point3D hiddenRightSP = leftLineRight.StartPoint; // hidden Right start Point

            Point3D lPlateLeftInPoint = GetSumPoint(leftLinetLeft.EndPoint, 2, 2);

            // spline Point setting
            distanceYPoint = distanceXPoint = 0;
            distanceYPoint = (leftLinetLeft.StartPoint.Y - leftLinetLeft.EndPoint.Y) / 4;
            distanceXPoint = (leftLineBottom.EndPoint.X - leftLineBottom.StartPoint.X) / 4;

            leftPointList.Add(leftLinetLeft.StartPoint);
            leftPointList.Add(GetSumPoint(leftLinetLeft.StartPoint, -2, -distanceYPoint));
            leftPointList.Add(leftLinetLeft.MidPoint); // left mid
            leftPointList.Add(GetSumPoint(leftLinetLeft.EndPoint, -2, distanceYPoint));
            leftPointList.Add(lPlateLeftInPoint);  // bottom start
            leftPointList.Add(GetSumPoint(leftLineBottom.StartPoint, distanceXPoint, -2));
            leftPointList.Add(leftLineBottom.MidPoint);  // bottom mid
            leftPointList.Add(GetSumPoint(leftLineBottom.EndPoint, -distanceXPoint, -2));
            leftPointList.Add(leftLineBottom.EndPoint);  // bottom end

            Curve leftSpline = Curve.CubicSplineInterpolation(leftPointList);
            splineList.Add(leftSpline);
            /**/

            // intersect Point
            Point3D[] intersectTopLeftPoint = leftLineTop.IntersectWith(topSpline);
            Point3D[] intersectMidPoint = leftLineRight.IntersectWith(tLineBottom);

            // Reset Point : top(left) / right line
            leftLineTop.EndPoint = intersectTopLeftPoint[0];
            leftLineRight.StartPoint = intersectMidPoint[0];

            // Draw Line
            topViewLineList.AddRange(new Line[] {
                                            leftLineTop, leftLineRight,
            });




            //@ Right Spline @/
            List<Point3D> rightPointList = new List<Point3D>();

            Point3D rightStartPoint = GetSumPoint(topViewStartPoint, 60 - 20, 0);
            List<Line> rightLineList = GetRectangle(rightStartPoint, 50, 65);
            Line rightLineBottom = rightLineList[0];
            Line rightLineRight = rightLineList[1];
            Line rightLineTop = rightLineList[2];
            Line rightLinetLeft = rightLineList[3];

            Point3D hiddenLeftSP = rightLinetLeft.StartPoint; // hidden Left start Point

            Point3D rightInLinePoint = GetSumPoint(rightLineBottom.EndPoint, -2, 2);


            // spline setting
            distanceYPoint = distanceXPoint = 0;
            distanceYPoint = (rightLinetLeft.StartPoint.Y - rightLinetLeft.EndPoint.Y) / 4;
            distanceXPoint = (rightLineBottom.EndPoint.X - rightLineBottom.StartPoint.X) / 4;

            rightPointList.Add(rightLineBottom.StartPoint);
            rightPointList.Add(GetSumPoint(rightLineBottom.StartPoint, distanceXPoint, -2));
            rightPointList.Add(rightLineBottom.MidPoint); // bottom mid
            rightPointList.Add(GetSumPoint(rightLineBottom.EndPoint, -distanceXPoint, -2));
            rightPointList.Add(rightInLinePoint);  // bottom end
            rightPointList.Add(GetSumPoint(rightLineRight.EndPoint, 2, distanceYPoint));
            rightPointList.Add(rightLineRight.MidPoint);  // right mid
            rightPointList.Add(GetSumPoint(rightLineRight.StartPoint, 2, -distanceYPoint));
            rightPointList.Add(rightLineRight.StartPoint);  // top right

            Curve rightSpline = Curve.CubicSplineInterpolation(rightPointList);
            splineList.Add(rightSpline);
            /**/

            Point3D[] intersectBottomPoint = rightLinetLeft.IntersectWith(leftSpline);
            Point3D[] intersectTopRightPoint = rightLineTop.IntersectWith(topSpline);

            // reset left line
            rightLinetLeft.StartPoint = intersectBottomPoint[0];
            rightLineTop.StartPoint = intersectTopRightPoint[0];

            // Draw rectangle line (Left/bottom)
            topViewLineList.AddRange(new Line[]{
                                                rightLinetLeft, rightLineTop
                                                });



            //@ Draw : hidden Line @
            topViewHiddenLineList.AddRange(new Line[] {
                                                        new Line(intersectTopLeftPoint[0],intersectTopRightPoint[0]),  // top
                                                        new Line(hiddenLeftSP,intersectBottomPoint[0]),  // left
                                                        new Line(hiddenRightSP,intersectMidPoint[0])  // right
                                                        });


            // Center Line : Red color
            //Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            //centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            styleService.SetLayerListEntity(ref splineList, layerService.LayerDimension);
            styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);
            styleService.SetLayerListLine(ref topViewLineList, layerService.LayerOutLine);
            styleService.SetLayerListLine(ref topViewHiddenLineList, layerService.LayerHiddenLine);

            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(lineList);
            //singleModel.Entities.AddRange(topViewLineList);
            //singleModel.Entities.AddRange(topViewHiddenLineList);
            //singleModel.Entities.Add(topSpline);
            //singleModel.Entities.Add(leftSpline);
            //singleModel.Entities.Add(rightSpline);

            //singleModel.Entities.Regen();
            //singleModel.Invalidate();

            //singleModel.SetView(viewType.Top);
            //singleModel.ZoomFit();
            //singleModel.Refresh();

            drawList.outlineList.AddRange(lineList);
            drawList.outlineList.AddRange(topViewLineList);
            drawList.outlineList.AddRange(topViewHiddenLineList);
            drawList.outlineList.Add(topSpline);
            drawList.outlineList.Add(leftSpline);
            drawList.outlineList.Add(rightSpline);



            // Center Point
            Point3D modelCenterPoint = GetSumPoint(leftLineRight.EndPoint, -5,0);
            selPaperAreaModel.ModelCenterLocation.X = modelCenterPoint.X;
            selPaperAreaModel.ModelCenterLocation.Y = modelCenterPoint.Y;







            // Dimension : Break
            double tempHeight = 5;
            List<Entity> breakAllList = new List<Entity>();
            breakAllList.AddRange(breakService.GetFlatBreakLine(GetSumPoint(lineList[5].StartPoint, 0, 0),
                                                            GetSumPoint(lineList[5].StartPoint, 0, -tempHeight),
                                                            scaleValue, true, true));
            breakAllList.AddRange(breakService.GetFlatBreakLine(GetSumPoint(lineList[7].StartPoint, 0, 0),
                                                            GetSumPoint(lineList[7].StartPoint, 0, -tempHeight),
                                                            scaleValue,true,false));
            breakAllList.AddRange(breakService.GetFlatBreakLine(GetSumPoint(lineList[9].EndPoint, 0, 0),
                                                            GetSumPoint(lineList[9].EndPoint, 0, -tempHeight),
                                                            scaleValue, true, false));
            breakAllList.AddRange(breakService.GetFlatBreakLine(GetSumPoint(lineList[1].EndPoint, 0, 0),
                                                            GetSumPoint(lineList[1].EndPoint, 0, -tempHeight),
                                                            scaleValue));
            styleService.SetLayerListEntity(ref breakAllList, layerService.LayerDimension);
            drawList.outlineList.AddRange(breakAllList);


            // Dimension : Arc
            //DrawEntityModel testDimArc01 = drawService.Draw_DimensionArc(GetSumPoint(leftTopLine.EndPoint, 0, 0), GetSumPoint(rightTopLine.StartPoint, 0, 0), "top", 12, "60˚", 45, 30, -30, 0, scaleValue, layerService.LayerDimension);
            //drawList.AddDrawEntity(testDimArc01);
            // Dimension : top

            string bottomThk = "t" + assemblyData.BottomInput[0].BottomPlateThickness;
            DrawDimensionModel dimModel01 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.BOTTOM,
                textUpper = "30",
                textLower = "(MIN.25)",
                textSizeVisible = false,
                dimHeight = 15,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity01 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lineList[2].EndPoint, 0, 0), GetSumPoint(lineList[4].EndPoint, 0, 0), scaleValue, dimModel01);
            drawList.AddDrawEntity(dimEntity01);
            DrawDimensionModel dimModel02 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.RIGHT,
                textUpper = bottomThk,
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                arrowRightHeadVisible = false,
                textUpperPosition = POSITION_TYPE.LEFT,
                textSizeVisible = false,
                dimHeight = 12,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity02 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lineList[2].EndPoint, 0, 0), GetSumPoint(lineList[1].EndPoint, 0, 0), scaleValue, dimModel02);
            drawList.AddDrawEntity(dimEntity02);
            DrawDimensionModel dimModel03 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.RIGHT,
                textUpper = bottomThk,
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                arrowLeftHeadVisible = false,
                textUpperPosition = POSITION_TYPE.RIGHT,
                textSizeVisible = false,
                dimHeight = 27,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity03 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lineList[8].EndPoint, 0, 0), GetSumPoint(lineList[9].EndPoint, 0, 0), scaleValue, dimModel03);
            drawList.AddDrawEntity(dimEntity03);
            DrawDimensionModel dimModel04 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.LEFT,
                textUpper = bottomThk,
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                textUpperPosition = POSITION_TYPE.RIGHT,
                textSizeVisible = false,
                dimHeight = 12,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity04 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lineList[3].StartPoint, 0, 0), GetSumPoint(lineList[5].StartPoint, 0, 0), scaleValue, dimModel04);
            drawList.AddDrawEntity(dimEntity04);


            //AngleSizeModel eachAngle = new AngleSizeModel();
            //if (assemblyData.RoofAngleOutput.Count > 0)
            //    eachAngle = assemblyData.RoofAngleOutput[0];
            //DrawDimensionModel dimModel03 = new DrawDimensionModel()
            //{
            //    position = POSITION_TYPE.LEFT,
            //    textUpper = "t" + eachAngle.t,
            //    extLineLeftVisible = false,
            //    extLineRightVisible = false,
            //    textSizeVisible = false,
            //    dimHeight = 6,
            //    scaleValue = scaleValue
            //};
            //DrawEntityModel dimEntity03 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(bottomLeftLine.EndPoint, 0, 0), GetSumPoint(leftTopLine.EndPoint, 0, 0), scaleValue, dimModel03);
            //drawList.AddDrawEntity(dimEntity03);

            // Dimension : Welding
            //DrawWeldSymbols weldSymbols = new DrawWeldSymbols();
            //List<Entity> weldList = new List<Entity>();
            //DrawWeldSymbolModel wsModel01 = new DrawWeldSymbolModel();
            //wsModel01.position = ORIENTATION_TYPE.TOPRIGHT;
            //wsModel01.weldTypeUp = WeldSymbol_Type.Square;
            //wsModel01.weldTypeDown = WeldSymbol_Type.V;
            //wsModel01.weldDetailType = WeldSymbolDetail_Type.BothSide;
            //wsModel01.weldFaceDown = WeldFace_Type.Convex;
            //wsModel01.tailVisible = true;
            //wsModel01.specification = "B.G";
            //wsModel01.leaderAngle = 60;
            //wsModel01.leaderLineLength = 18;
            //weldList.AddRange(weldSymbols.GetWeldSymbol(GetSumPoint(arcTop.MidPoint, 0, 0), singleModel, scaleValue, wsModel01));


            //styleService.SetLayerListEntity(ref weldList, layerService.LayerDimension);
            //drawList.outlineList.AddRange(weldList);


            DrawBMLeaderModel leaderInfoModel = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = "HAMMERING", bmNumber = "", textAlign = POSITION_TYPE.CENTER, leaderPointLength = 12, leaderPointRadian = Utility.DegToRad(45) };
            DrawEntityModel leaderInfoList = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(lineList[11].MidPoint, 0, 0), leaderInfoModel, scaleValue);
            drawList.AddDrawEntity(leaderInfoList);




            return drawList;
        }


        // Top Angle
        public DrawEntityModel DrawTopAngleJointDetail_View_B(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {

            DrawEntityModel drawList = new DrawEntityModel();

            // TopAngleJointDetail View "B"

            //singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Line> lineList = new List<Line>();

            List<Point3D> JointPointList = new List<Point3D>();


            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);


            /////////////////////////////
            //      Information      ////
            /////////////////////////////

            Line intersectLeftLine = null;
            Line intersectRighthLine = null;
            double distance = 3.2 / 2;

            Line hLeftBottom = new Line(referencePoint, GetSumPoint(referencePoint, 34, 0));
            Line vLeftEnd = new Line(hLeftBottom.EndPoint, GetSumPoint(hLeftBottom.EndPoint, 0, 38));
            Line hLeftTop = new Line(vLeftEnd.EndPoint, GetSumPoint(vLeftEnd.EndPoint, -34, 0));

            // Diagonal Line calculate
            Point3D diagonalStartPoint = GetSumPoint(hLeftTop.StartPoint, 0, -1.6);
            Line tempLine = new Line(GetSumPoint(hLeftTop.StartPoint, 0, -6), GetSumPoint(hLeftTop.EndPoint, 0, -6));
            Line intersectLine = new Line(diagonalStartPoint, GetSumPoint(diagonalStartPoint, -30, 0));
            intersectLine.Rotate(Utility.DegToRad(60), Vector3D.AxisZ, diagonalStartPoint);

            // Get intersect End Point
            Point3D[] diagonalEndPoint = intersectLine.IntersectWith(tempLine);

            Line diagonalLeft = new Line(diagonalStartPoint, diagonalEndPoint[0]);
            Line hLeftMid = new Line(GetSumPoint(hLeftTop.EndPoint, 0, -6), diagonalEndPoint[0]);
            double diagonalXDis = diagonalLeft.StartPoint.X - diagonalLeft.EndPoint.X; // diagonal x - x distance

            Line vLeftMid = new Line(diagonalEndPoint[0], GetSumPoint(hLeftBottom.EndPoint, -diagonalXDis, 0));

            // Draw : Mirror
            Line hRightBottom = MirrorLine(hLeftBottom, Vector2D.AxisX, distance);
            Line vRightEnd = MirrorLine(vLeftEnd, Vector2D.AxisX, distance);
            Line hRightTop = MirrorLine(hLeftTop, Vector2D.AxisX, distance);
            Line diagonalRight = MirrorLine(diagonalLeft, Vector2D.AxisX, distance);
            Line hRightMid = MirrorLine(hLeftMid, Vector2D.AxisX, distance + diagonalXDis);
            Line vRightMid = MirrorLine(vLeftMid, Vector2D.AxisX, distance + diagonalXDis);

            lineList.AddRange(new Line[]{
                    hLeftBottom, vLeftEnd, hLeftTop, diagonalLeft, hLeftMid, vLeftMid,
                    hRightBottom, vRightEnd, hRightTop, diagonalRight, hRightMid, vRightMid
                    });


            /*
            // Horizontal Demention
            /
            - Top Position -
            // 3.2
            hLeftTop.StartPoint, hRightTop.StartPoint

            // 60도 깃발
            hRightTop.StartPoint

            // 아래 60도
            diagonalEndPoint[0] // 좌측

            /**/




            Point3D modelCenterPoint = GetSumPoint(hRightTop.StartPoint, 0, 0);
            SetModelCenterPoint(PAPERSUB_TYPE.TopAngleJoint, modelCenterPoint);


            // Center Line : Red color
            //Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            //centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);

            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(lineList);

            //singleModel.Entities.Regen();
            //singleModel.Invalidate();

            //singleModel.SetView(viewType.Top);
            //singleModel.ZoomFit();
            //singleModel.Refresh();

            drawList.outlineList.AddRange(lineList);


            // Dimension : Break
            List<Entity> leftBreakList = breakService.GetFlatBreakLine(GetSumPoint(hLeftTop.StartPoint, 0, 0),
                                                            GetSumPoint(hLeftBottom.StartPoint,0,0),
                                                            scaleValue);
            styleService.SetLayerListEntity(ref leftBreakList, layerService.LayerDimension);
            drawList.outlineList.AddRange(leftBreakList);

            List<Entity> rightBreakList = breakService.GetFlatBreakLine(GetSumPoint(hRightTop.EndPoint, 0, 0),
                                                GetSumPoint(hRightBottom.EndPoint, 0, 0),
                                                scaleValue);
            styleService.SetLayerListEntity(ref rightBreakList, layerService.LayerDimension);
            drawList.outlineList.AddRange(rightBreakList);

            // Dimension : Arc
            DrawEntityModel testDimArc01 = drawService.Draw_DimensionArc(GetSumPoint(diagonalLeft.StartPoint, 0, 0), GetSumPoint(diagonalRight.EndPoint, 0, 0), "top", 15, "60˚", 45, 150, 210, 0, scaleValue, layerService.LayerDimension);
            drawList.AddDrawEntity(testDimArc01);

            // Dimension : top
            DrawDimensionModel dimModel01 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = "3.2",
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                textUpperPosition = POSITION_TYPE.LEFT,
                textSizeVisible = false,
                dimHeight = 12,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity01 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(hLeftTop.EndPoint, 0, 0), GetSumPoint(hRightTop.StartPoint, 0, 0), scaleValue, dimModel01);
            drawList.AddDrawEntity(dimEntity01);
            DrawDimensionModel dimModel02 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.RIGHT,
                textUpper = "1.6",
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                textUpperPosition = POSITION_TYPE.LEFT,
                textSizeVisible = false,
                dimHeight = 15,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity02 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(diagonalRight.StartPoint, 0, 0), GetSumPoint(hRightTop.StartPoint, 0, 0), scaleValue, dimModel02);
            drawList.AddDrawEntity(dimEntity02);


            AngleSizeModel eachAngle = new AngleSizeModel();
            if (assemblyData.RoofAngleOutput.Count > 0)
                eachAngle = assemblyData.RoofAngleOutput[0];
            DrawDimensionModel dimModel03 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.LEFT,
                textUpper = "t" +eachAngle.t,
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                textUpperPosition = POSITION_TYPE.RIGHT,
                textSizeVisible = false,
                dimHeight = 12,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity03 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(hLeftMid.EndPoint, 0, 0), GetSumPoint(hLeftTop.EndPoint, 0, 0), scaleValue, dimModel03);
            drawList.AddDrawEntity(dimEntity03);

            // Dimension : Welding
            DrawWeldSymbols weldSymbols = new DrawWeldSymbols();
            List<Entity> weldList = new List<Entity>();
            DrawWeldSymbolModel wsModel01 = new DrawWeldSymbolModel();
            wsModel01.position = ORIENTATION_TYPE.TOPRIGHT;
            wsModel01.weldTypeUp = WeldSymbol_Type.V;
            wsModel01.weldTypeDown = WeldSymbol_Type.Square;
            wsModel01.weldDetailType = WeldSymbolDetail_Type.BothSide;
            wsModel01.weldFaceUp = WeldFace_Type.Convex;
            wsModel01.weldFaceDown = WeldFace_Type.Flat;
            wsModel01.weldAngle1 = "60˚";
            wsModel01.machiningStr = "G";
            wsModel01.machiningVisible = true;
            wsModel01.tailVisible = false;
            wsModel01.leaderAngle = 60;
            wsModel01.leaderLineLength = 20;
            weldList.AddRange(weldSymbols.GetWeldSymbol(GetSumPoint(hRightTop.StartPoint, 0, 0), singleModel,scaleValue, wsModel01));

            DrawWeldSymbolModel wsModel02 = new DrawWeldSymbolModel();
            wsModel02.position = ORIENTATION_TYPE.BOTTOMLEFT;
            wsModel02.weldTypeUp = WeldSymbol_Type.Square;
            wsModel02.weldTypeDown = WeldSymbol_Type.V;
            wsModel02.weldDetailType = WeldSymbolDetail_Type.BothSide;
            wsModel02.weldFaceDown = WeldFace_Type.Convex;
            wsModel02.weldAngle2 = "60˚";
            wsModel02.tailVisible = false;
            wsModel02.leaderAngle = 60;
            wsModel02.leaderLineLength = 20;
            weldList.AddRange(weldSymbols.GetWeldSymbol(GetSumPoint(vRightEnd.EndPoint, 0, 0), singleModel, scaleValue, wsModel02));

            styleService.SetLayerListEntity(ref weldList, layerService.LayerDimension);
            drawList.outlineList.AddRange(weldList);


            return drawList;
        }


        // Wind Grider
        public DrawEntityModel DrawTopAngleJointDetail_View_C(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {

            DrawEntityModel drawList = new DrawEntityModel();


            // Wind Girder Joint Detail View "C"

            //singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Line> lineList = new List<Line>();
            List<Entity> virtualList = new List<Entity>();

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);


            /////////////////////////////
            //      Information      ////
            /////////////////////////////
            double distance = 3.2 / 2;


            Line hLeftBottom = new Line(referencePoint, GetSumPoint(referencePoint, 34, 0));
            Line vLeftEnd = new Line(hLeftBottom.EndPoint, GetSumPoint(hLeftBottom.EndPoint, 0, (38 - 6 + 1.6)));

            //// Diagonal Line calculate
            // /* <-------------------------------------------
            Point3D diagonalStartPoint = vLeftEnd.EndPoint;
            Line tempLine = new Line(GetSumPoint(hLeftBottom.StartPoint, 0, 38), GetSumPoint(hLeftBottom.EndPoint, 0, 38)); // 4번 가상선
            Line intersectLine = new Line(diagonalStartPoint, GetSumPoint(diagonalStartPoint, 0, 15));  // 15 = 임의값
            intersectLine.Rotate(Utility.DegToRad(30), Vector3D.AxisZ, diagonalStartPoint);

            // Get intersect End Point
            Point3D[] diagonalEndPoint = intersectLine.IntersectWith(tempLine);
            Line diagonalLeft = new Line(diagonalStartPoint, diagonalEndPoint[0]);

            // diagonal x - x distance
            double diagonalXDis = Math.Abs(diagonalLeft.StartPoint.X - diagonalLeft.EndPoint.X);
            //-----------------------------------------------> */

            Line hLeftTop = new Line(GetSumPoint(hLeftBottom.StartPoint, 0, 38), diagonalEndPoint[0]);
            Line vLeftMid = new Line(diagonalEndPoint[0], GetSumPoint(hLeftBottom.EndPoint, -diagonalXDis, 0));
            Line hLeftDottedLine = new Line(GetSumPoint(hLeftBottom.StartPoint, 0, 38 - 6), GetSumPoint(hLeftBottom.EndPoint, 0, 38 - 6));


            // Mirror
            Line hRightBottom = MirrorLine(hLeftBottom, Vector2D.AxisX, distance);
            Line vRightEnd = MirrorLine(vLeftEnd, Vector2D.AxisX, distance);
            Line diagonalRight = MirrorLine(diagonalLeft, Vector2D.AxisX, distance);
            Line hRightTop = MirrorLine(hLeftTop, Vector2D.AxisX, distance + diagonalXDis);
            Line vRightMid = MirrorLine(vLeftMid, Vector2D.AxisX, distance + diagonalXDis);
            Line hRightDottedLine = MirrorLine(hLeftDottedLine, Vector2D.AxisX, distance);

            /**/
            lineList.AddRange(new Line[]{
                    hLeftBottom, vLeftEnd, hLeftTop, diagonalLeft,  vLeftMid,
                    hRightBottom, vRightEnd, hRightTop, diagonalRight,  vRightMid
                    });

            virtualList.Add(hLeftDottedLine);
            virtualList.Add(hRightDottedLine);


            // Center Line : Red color
            //Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            //centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            //styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);

            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(lineList);

            //singleModel.Entities.Regen();
            //singleModel.Invalidate();

            //singleModel.SetView(viewType.Top);
            //singleModel.ZoomFit();
            //singleModel.Refresh();


            Point3D modelCenterPoint = GetSumPoint(hRightTop.StartPoint, 0, 0);
            SetModelCenterPoint(PAPERSUB_TYPE.WindGirderJoint, modelCenterPoint);


            styleService.SetLayerListLine(ref virtualList, layerService.LayerVirtualLine);
            styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(virtualList);
            drawList.outlineList.AddRange(lineList);




            // Dimension : Break
            List<Entity> leftBreakList = breakService.GetFlatBreakLine(GetSumPoint(hLeftTop.StartPoint, 0, 0),
                                                            GetSumPoint(hLeftBottom.StartPoint, 0, 0),
                                                            scaleValue);
            styleService.SetLayerListEntity(ref leftBreakList, layerService.LayerDimension);
            drawList.outlineList.AddRange(leftBreakList);

            List<Entity> rightBreakList = breakService.GetFlatBreakLine(GetSumPoint(hRightTop.EndPoint, 0, 0),
                                                GetSumPoint(hRightBottom.EndPoint, 0, 0),
                                                scaleValue);
            styleService.SetLayerListEntity(ref rightBreakList, layerService.LayerDimension);
            drawList.outlineList.AddRange(rightBreakList);

            // Dimension : Arc
            DrawEntityModel testDimArc01 = drawService.Draw_DimensionArc(GetSumPoint(diagonalLeft.StartPoint, 0, 0), GetSumPoint(diagonalRight.EndPoint, 0, 0), "top", 18, "60˚", 45, 30, -30, 0, scaleValue, layerService.LayerDimension);
            drawList.AddDrawEntity(testDimArc01);

            // Dimension : top
            DrawDimensionModel dimModel01 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = "3.2",
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                textUpperPosition = POSITION_TYPE.LEFT,
                textSizeVisible = false,
                dimHeight = 15,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity01 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(diagonalLeft.EndPoint, 0, 0), GetSumPoint(diagonalRight.StartPoint, 0, 0), scaleValue, dimModel01);
            drawList.AddDrawEntity(dimEntity01);
            DrawDimensionModel dimModel02 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.RIGHT,
                textUpper = "1.6",
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                extLineLeftVisible = false,
                textUpperPosition = POSITION_TYPE.LEFT,
                textSizeVisible = false,
                dimHeight = 15,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity02 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(hRightDottedLine.StartPoint, 0, 0), GetSumPoint(diagonalRight.StartPoint, 0, 0), scaleValue, dimModel02);
            drawList.AddDrawEntity(dimEntity02);


            AngleSizeModel eachAngle = new AngleSizeModel();
            if (assemblyData.RoofAngleOutput.Count > 0)
                eachAngle = assemblyData.RoofAngleOutput[0];
            DrawDimensionModel dimModel03 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.LEFT,
                textUpper = "t" + eachAngle.t,
                extLineLeftVisible = false,
                extLineRightVisible=false,
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                textUpperPosition = POSITION_TYPE.RIGHT,
                textSizeVisible = false,
                dimHeight = 12,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity03 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(hLeftDottedLine.EndPoint, 0, 0), GetSumPoint(hLeftTop.EndPoint, 0, 0), scaleValue, dimModel03);
            drawList.AddDrawEntity(dimEntity03);

            // Dimension : Welding
            DrawWeldSymbols weldSymbols = new DrawWeldSymbols();
            List<Entity> weldList = new List<Entity>();
            DrawWeldSymbolModel wsModel01 = new DrawWeldSymbolModel();
            wsModel01.position = ORIENTATION_TYPE.TOPRIGHT;
            wsModel01.weldTypeUp = WeldSymbol_Type.Square;
            wsModel01.weldTypeDown = WeldSymbol_Type.V;
            wsModel01.weldDetailType = WeldSymbolDetail_Type.BothSide;
            wsModel01.weldFaceDown = WeldFace_Type.Convex;
            wsModel01.weldAngle2 = "60˚";
            wsModel01.tailVisible = false;
            wsModel01.leaderAngle = 45;
            wsModel01.leaderLineLength = 20;
            weldList.AddRange(weldSymbols.GetWeldSymbol(GetSumPoint(hRightTop.StartPoint, 0, 0), singleModel, scaleValue, wsModel01));

            DrawWeldSymbolModel wsModel02 = new DrawWeldSymbolModel();
            wsModel02.position = ORIENTATION_TYPE.BOTTOMLEFT;
            wsModel02.weldTypeUp = WeldSymbol_Type.Square;
            wsModel02.weldTypeDown = WeldSymbol_Type.V;
            wsModel02.weldDetailType = WeldSymbolDetail_Type.BothSide;
            wsModel02.weldFaceDown = WeldFace_Type.Convex;
            wsModel02.weldAngle2 = "60˚";
            wsModel02.tailVisible = false;
            wsModel02.leaderAngle = 60;
            wsModel02.leaderLineLength = 20;
            weldList.AddRange(weldSymbols.GetWeldSymbol(GetSumPoint(vRightEnd.StartPoint, 0, 0), singleModel, scaleValue, wsModel02));

            styleService.SetLayerListEntity(ref weldList, layerService.LayerDimension);
            drawList.outlineList.AddRange(weldList);




            return drawList;
        }


        // Tp Angle
        public DrawEntityModel DrawTopAngle_Detail_ALL(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue, PaperAreaModel selPaperAreaModel)
        {
            DrawEntityModel drawList = new DrawEntityModel();
            string roofType = assemblyData.RoofCompressionRing[0].CompressionRingType;
            if(roofType.Contains("Detail b"))
            {
                drawList.AddDrawEntity(DrawTopAngle_Detail_b(ref refPoint, ref curPoint, selModel, scaleValue, selPaperAreaModel));
            }
            else if (roofType.Contains("Detail d"))
            {
                drawList.AddDrawEntity(DrawTopAngle_Detail_b(ref refPoint, ref curPoint, selModel, scaleValue, selPaperAreaModel));
            }
            else if (roofType.Contains("Detail e"))
            {
                drawList.AddDrawEntity(DrawTopAngle_Detail_e(ref refPoint, ref curPoint, selModel, scaleValue, selPaperAreaModel));
            }
            else if (roofType.Contains("Detail i"))
            {
                drawList.AddDrawEntity(DrawTopAngle_Detail_i(ref refPoint, ref curPoint, selModel, scaleValue, selPaperAreaModel));
            }
            else if (roofType.Contains("Detail k"))
            {
                drawList.AddDrawEntity(DrawTopAngle_Detail_k(ref refPoint, ref curPoint, selModel, scaleValue, selPaperAreaModel));
            }
                return drawList;
        }
        public DrawEntityModel DrawTopAngle_Detail_b(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue, PaperAreaModel selPaperAreaModel)
        {
            DrawEntityModel drawList = new DrawEntityModel();


            // Top Angle Joint Detail : Detail "A"  1

            //singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Line> lineList = new List<Line>();
            List<Entity> virtualLineList = new List<Entity>();

            double angleR1 = 3.5;
            double angleR2 = angleR1 * 2;
            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            // bottom Plate
            Point3D bottomStartPoint = GetSumPoint(referencePoint, 45, 0);
            List<Line> bottomLineList = GetRectangle(bottomStartPoint, 50, 4, RECTANGLUNVIEW.BOTTOM);
            Line centerRight = bottomLineList[0];
            Line centerTop = bottomLineList[1];
            Line centerLeft = bottomLineList[2];

            // angle
            Line angleTop = new Line(GetSumPoint(referencePoint, 0, 50 + 10), GetSumPoint(referencePoint, 45, 50 + 10));
            Line angleRight = new Line(GetSumPoint(angleTop.EndPoint, 0, 0), GetSumPoint(angleTop.EndPoint, 0, -45));
            Line angleBottom = new Line(GetSumPoint(angleRight.EndPoint, 0, 0), GetSumPoint(angleRight.EndPoint, -4, 0));
            Line angleLeft = new Line(GetSumPoint(angleBottom.EndPoint, 0, 0), GetSumPoint(angleBottom.EndPoint, 0, 45 - 4));
            Line angleTop_low = new Line(GetSumPoint(angleLeft.EndPoint, 0, 0), GetSumPoint(angleLeft.EndPoint, -(45 - 4), 0));
            Line angleTop_left = new Line(GetSumPoint(angleTop_low.EndPoint, 0, 0), GetSumPoint(angleTop.StartPoint, 0, 0));

            Arc arcFillet01;
            if (Curve.Fillet(angleBottom, angleLeft, angleR1, false, false, true, true, out arcFillet01))
                virtualLineList.Add(arcFillet01);
            Arc arcFillet02;
            if (Curve.Fillet(angleTop_low, angleLeft, angleR2, true, true, true, true, out arcFillet02))
                virtualLineList.Add(arcFillet02);
            Arc arcFillet03;
            if (Curve.Fillet(angleTop_low, angleTop_left, angleR1, false, false, true, true, out arcFillet03))
                virtualLineList.Add(arcFillet03);

            // roof Plate
            Point3D roofStartPoint = GetSumPoint(angleTop.EndPoint, -11, 0);
            List<Line> roofPlateLineList = GetRectangle(roofStartPoint, 6, 60, RECTANGLUNVIEW.RIGHT);
            Line roofPlate_bottom = roofPlateLineList[0];
            Line roofPlate_top = roofPlateLineList[1];
            Line roofPlate_left = roofPlateLineList[2];

            // Rotation : Roof Plate
            roofPlate_bottom.Rotate(Utility.DegToRad(9.5), Vector3D.AxisZ, roofStartPoint);
            roofPlate_top.Rotate(Utility.DegToRad(9.5), Vector3D.AxisZ, roofStartPoint);
            roofPlate_left.Rotate(Utility.DegToRad(9.5), Vector3D.AxisZ, roofStartPoint);

            // Diagonal Line
            Point3D diagonalStartPoint = GetSumPoint(roofStartPoint, -6, 0);  // 6은 임의값(Plate width=6)  5인지 확인 필요 
            Line diagonalLine = new Line(diagonalStartPoint, roofPlate_top.StartPoint);
            Line diagonalBottom = new Line(diagonalStartPoint, roofPlate_bottom.StartPoint);


            virtualLineList.AddRange(new Entity[]{
                    centerRight, centerTop, centerLeft,
                    angleTop, angleRight, angleBottom, angleLeft, angleTop_low, angleTop_left,
                    });

            lineList.AddRange(new Line[]{
                    roofPlate_bottom, roofPlate_top, roofPlate_left,
                    diagonalLine, diagonalBottom
                    });

            // Center Line : Red color
            //Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            //centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            //styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);
            //styleService.SetLayerListEntity(ref virtualLineList, layerService.LayerVirtualLine);

            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(lineList);
            //singleModel.Entities.AddRange(virtualLineList);

            //singleModel.Entities.Regen();
            //singleModel.Invalidate();

            //singleModel.SetView(viewType.Top);
            //singleModel.ZoomFit();
            //singleModel.Refresh();

            styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref virtualLineList, layerService.LayerVirtualLine);

            drawList.outlineList.AddRange(lineList);
            drawList.outlineList.AddRange(virtualLineList);


            Point3D modelCenterPoint = GetSumPoint(centerRight.StartPoint, 0, 0);
            selPaperAreaModel.ModelCenterLocation.X = modelCenterPoint.X;
            selPaperAreaModel.ModelCenterLocation.Y = modelCenterPoint.Y;
            

            return drawList;
        }

        public DrawEntityModel DrawTopAngle_Detail_e(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue, PaperAreaModel selPaperAreaModel)
        {

            DrawEntityModel drawList = new DrawEntityModel();


            // Top Angle Joint Detail : Detail "A"  2

            //singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Line> lineList = new List<Line>();
            List<Entity> virtualLineList = new List<Entity>();


            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            // bottom Plate
            List<Line> bottomLineList = GetRectangle(referencePoint, 15, 4, RECTANGLUNVIEW.BOTTOM);
            Line centerRight = bottomLineList[0];
            Line centerTop = bottomLineList[1];
            Line centerLeft = bottomLineList[2];

            // angle
            double angleWidth = 45;
            double angleHeight = 45;
            double angleThk = 4;
            double angleR1 = 3.5;
            double angleR2 = angleR1 * 2;

            Point3D angleStartPoint = GetSumPoint(referencePoint, 0, 15 + angleWidth);
            Line angleTop = new Line(angleStartPoint, GetSumPoint(angleStartPoint, angleWidth, 0));
            Line angleLeft = new Line(angleStartPoint, GetSumPoint(angleStartPoint, 0, -angleHeight));
            Line angleBottom = new Line(GetSumPoint(angleLeft.EndPoint, 0, 0), GetSumPoint(angleLeft.EndPoint, angleThk, 0));
            Line angleRight = new Line(GetSumPoint(angleBottom.EndPoint, 0, 0), GetSumPoint(angleBottom.EndPoint, 0, (angleHeight - angleThk)));
            Line angleTop_low = new Line(GetSumPoint(angleRight.EndPoint, 0, 0), GetSumPoint(angleRight.EndPoint, (angleHeight - angleThk), 0));
            Line angleTop_right = new Line(GetSumPoint(angleTop_low.EndPoint, 0, 0), GetSumPoint(angleTop.EndPoint, 0, 0));

            Arc arcFillet01;
            if (Curve.Fillet(angleBottom, angleRight, angleR1, false, false, true, true, out arcFillet01))
                virtualLineList.Add(arcFillet01);
            Arc arcFillet02;
            if (Curve.Fillet(angleRight, angleTop_low, angleR2, false, false, true, true, out arcFillet02))
                virtualLineList.Add(arcFillet02);
            Arc arcFillet03;
            if (Curve.Fillet(angleTop_low, angleTop_right, angleR1, false, false, true, true, out arcFillet03))
                virtualLineList.Add(arcFillet03);


            // roof Plate
            Point3D roofStartPoint = GetSumPoint(referencePoint, 15, 15 + 45);
            List<Line> roofPlateLineList = GetRectangle(roofStartPoint, 6, 60, RECTANGLUNVIEW.RIGHT);
            Line roofPlate_bottom = roofPlateLineList[0];
            Line roofPlate_top = roofPlateLineList[1];
            Line roofPlate_left = roofPlateLineList[2];

            // Rotation : Roof Plate
            roofPlate_bottom.Rotate(Utility.DegToRad(9.5), Vector3D.AxisZ, roofStartPoint);
            roofPlate_top.Rotate(Utility.DegToRad(9.5), Vector3D.AxisZ, roofStartPoint);
            roofPlate_left.Rotate(Utility.DegToRad(9.5), Vector3D.AxisZ, roofStartPoint);

            // Diagonal Line
            Point3D diagonalStartPoint = GetSumPoint(roofStartPoint, -6, 0);
            Line diagonalLine = new Line(diagonalStartPoint, roofPlate_top.StartPoint);
            Line diagonalBottom = new Line(diagonalStartPoint, roofPlate_bottom.StartPoint);

            virtualLineList.AddRange(new Entity[]{
                    centerRight, centerTop, centerLeft,
                    angleTop, angleRight, angleBottom, angleLeft, angleTop_low, angleTop_right,
                    });

            lineList.AddRange(new Line[]{
                    roofPlate_bottom, roofPlate_top, roofPlate_left,
                    diagonalLine, diagonalBottom
                    });


            // Center Line : Red color
            Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            //centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            //styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);
            //styleService.SetLayerListEntity(ref virtualLineList, layerService.LayerVirtualLine);

            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(lineList);
            //singleModel.Entities.AddRange(virtualLineList);

            //singleModel.Entities.Regen();
            //singleModel.Invalidate();

            //singleModel.SetView(viewType.Top);
            //singleModel.ZoomFit();
            //singleModel.Refresh();


            styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref virtualLineList, layerService.LayerVirtualLine);

            drawList.outlineList.AddRange(lineList);
            drawList.outlineList.AddRange(virtualLineList);



            Point3D modelCenterPoint = GetSumPoint(angleTop.EndPoint, -10, -10);
            selPaperAreaModel.ModelCenterLocation.X = modelCenterPoint.X;
            selPaperAreaModel.ModelCenterLocation.Y = modelCenterPoint.Y;

            return drawList;

        }

        public DrawEntityModel DrawTopAngle_Detail_i(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue, PaperAreaModel selPaperAreaModel)
        {

            DrawEntityModel drawList = new DrawEntityModel();

            // Roof Compression RIng Joint Detail : Detail "A"

            //singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Line> lineList = new List<Line>();
            List<Entity> virtualLineList = new List<Entity>();

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            // bottom Plate
            Point3D bottomStartPoint = GetSumPoint(referencePoint, 25, 0);
            List<Line> bottomLineList = GetRectangle(bottomStartPoint, 25, 4, RECTANGLUNVIEW.BOTTOM);
            Line bottomRight = bottomLineList[0];
            Line bottomTop = bottomLineList[1];
            Line bottomLeft = bottomLineList[2];


            Point3D rotatePoint = GetSumPoint(referencePoint, 25, 25);

            // Mid Plate
            Point3D midStartPoint = GetSumPoint(referencePoint, 0, 25);
            List<Line> midLineList = GetRectangle(midStartPoint, 6, 95);

            // Rotate : mid Plate   
            for (int i = 0; i < midLineList.Count; i++)
                midLineList[i].Rotate(Utility.DegToRad(6), Vector3D.AxisZ, rotatePoint);

            Line midBottom = midLineList[0];
            Line midRight = midLineList[1];
            Line midTop = midLineList[2];
            Line midLeft = midLineList[3];


            // Roof Plate
            Point3D roofStartPoint = GetSumPoint(referencePoint, 95 - 25, 25 + 6);
            List<Line> roofPlateLineList = GetRectangle(roofStartPoint, 3, 45, RECTANGLUNVIEW.RIGHT);

            // Rotate : roof Plate
            for (int i = 0; i < roofPlateLineList.Count; i++)
                roofPlateLineList[i].Rotate(Utility.DegToRad(6), Vector3D.AxisZ, rotatePoint);

            Line roofPlate_bottom = roofPlateLineList[0];
            Line roofPlate_top = roofPlateLineList[1];
            Line roofPlate_left = roofPlateLineList[2];


            // Diagonal Line
            Point3D diagonalStartPoint = GetSumPoint(roofStartPoint, -3, 0);
            Line diagonalLine = new Line(diagonalStartPoint, GetSumPoint(roofStartPoint, 0, 3));
            diagonalLine.Rotate(Utility.DegToRad(6), Vector3D.AxisZ, rotatePoint);
            Line diagonalBottom = new Line(diagonalStartPoint, roofPlate_bottom.StartPoint);

            virtualLineList.AddRange(new Entity[]{
                    bottomRight, bottomTop, bottomLeft,
                    midBottom, midRight, midTop, midLeft,
                    });

            lineList.AddRange(new Line[]{
                    roofPlate_bottom, roofPlate_top, roofPlate_left,
                    diagonalLine, diagonalBottom
                    });


            // Center Line : Red color
            //Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            //centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            //styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);
            //styleService.SetLayerListEntity(ref virtualLineList, layerService.LayerVirtualLine);

            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(lineList);
            //singleModel.Entities.AddRange(virtualLineList);

            //singleModel.Entities.Regen();
            //singleModel.Invalidate();

            //singleModel.SetView(viewType.Top);
            //singleModel.ZoomFit();
            //singleModel.Refresh();

            styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref virtualLineList, layerService.LayerVirtualLine);

            drawList.outlineList.AddRange(lineList);
            drawList.outlineList.AddRange(virtualLineList);


            Point3D modelCenterPoint = GetSumPoint(midBottom.EndPoint, -35, -5);
            selPaperAreaModel.ModelCenterLocation.X = modelCenterPoint.X;
            selPaperAreaModel.ModelCenterLocation.Y = modelCenterPoint.Y;

            return drawList;
        }

        public DrawEntityModel DrawTopAngle_Detail_k(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue, PaperAreaModel selPaperAreaModel)
        {

            DrawEntityModel drawList = new DrawEntityModel();

            // Shell Compression RIng Joint Detail : Detail "A"

            //singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Line> lineList = new List<Line>();
            List<Entity> virtualLineList = new List<Entity>();

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            // bottom Plate
            Point3D bottomStartPoint = GetSumPoint(referencePoint, 30 + 1.5, 0);
            List<Line> bottomLineList = GetRectangle(bottomStartPoint, 20, 4, RECTANGLUNVIEW.BOTTOM);

            Line bottomRight = bottomLineList[0];
            Line bottomTop = bottomLineList[1];
            Line bottomLeft = bottomLineList[2];


            // Mid Plate
            Point3D midStartPoint = GetSumPoint(referencePoint, 30, 20);
            List<Line> midLineList = GetRectangle(midStartPoint, 6, 7, RECTANGLEALIGN.CENTER, 0, 4, RECTANGLUNVIEW.TOP);

            Line midBottom = midLineList[0];
            Line midRight = midLineList[1];
            Line midLeft = midLineList[2];


            // Top Plate
            Point3D topStartPoint = midLeft.StartPoint;
            List<Line> topLineList = GetRectangle(topStartPoint, 50 - 6, 7, RECTANGLUNVIEW.BOTTOM);

            Line topRight = topLineList[0];
            Line topTop = topLineList[1];
            Line topLeft = topLineList[2];


            // Left Plate
            Point3D leftStartPoint = GetSumPoint(referencePoint, 0, 20 + 50 - 12 - 7);
            List<Line> leftLineList = GetRectangle(leftStartPoint, 7, 30);

            Line leftBottom = leftLineList[0];
            Line leftRight = leftLineList[1];
            Line leftTop = leftLineList[2];
            Line leftLeft = leftLineList[3];


            Point3D rotatePoint = topRight.StartPoint;

            // Roof Plate
            Point3D roofStartPoint = rotatePoint;
            List<Line> roofPlateLineList = GetRectangle(roofStartPoint, 3, 60, RECTANGLUNVIEW.RIGHT);  // 3 임의값 , 확인후 재입력할 것

            // Rotate : roof Plate
            for (int i = 0; i < roofPlateLineList.Count; i++)
                roofPlateLineList[i].Rotate(Utility.DegToRad(9.5), Vector3D.AxisZ, rotatePoint);

            Line roofPlate_bottom = roofPlateLineList[0];
            Line roofPlate_top = roofPlateLineList[1];
            Line roofPlate_left = roofPlateLineList[2];


            // Diagonal Line
            Point3D diagonalStartPoint = GetSumPoint(roofStartPoint, -3, 0); // -3 임의 값, 확인 후 재입력할 것
            Line diagonalLine = new Line(diagonalStartPoint, roofPlate_top.StartPoint);
            Line diagonalBottom = new Line(diagonalStartPoint, roofPlate_bottom.StartPoint);

            virtualLineList.AddRange(new Entity[]{
                    bottomRight, bottomTop, bottomLeft,
                    midBottom, midRight, midLeft,
                    topRight, topTop, topLeft,
                    leftBottom, leftRight, leftTop, leftLeft,
                    });

            lineList.AddRange(new Line[]{
                    roofPlate_bottom, roofPlate_top, roofPlate_left,
                    diagonalLine, diagonalBottom
                    });


            // Center Line : Red color
            //Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            //centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            //styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);
            //styleService.SetLayerListEntity(ref virtualLineList, layerService.LayerVirtualLine);

            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(lineList);
            //singleModel.Entities.AddRange(virtualLineList);

            //singleModel.Entities.Regen();
            //singleModel.Invalidate();

            //singleModel.SetView(viewType.Top);
            //singleModel.ZoomFit();
            //singleModel.Refresh();

            styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref virtualLineList, layerService.LayerVirtualLine);

            drawList.outlineList.AddRange(lineList);
            drawList.outlineList.AddRange(virtualLineList);

            Point3D modelCenterPoint = GetSumPoint(topTop.EndPoint, 15, -20);
            selPaperAreaModel.ModelCenterLocation.X = modelCenterPoint.X;
            selPaperAreaModel.ModelCenterLocation.Y = modelCenterPoint.Y;

            return drawList;
        }






        public DrawEntityModel DrawNamePlateBracket(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {

            DrawEntityModel drawList = new DrawEntityModel();

            // NamePlate Bracket

            //singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Entity> lineList = new List<Entity>();
            List<Entity> topviewLineList = new List<Entity>();
            List<Entity> virtualLineList = new List<Entity>();
            List<Entity> hiddenLineList = new List<Entity>();

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);



            ///////////////////////
            ///  CAD Data Setting
            ///////////////////////
            double bracketWidth = 170;
            double bracketThk = 3;
            double bracketLength = 230;
            double holeDistanceOfOutline = 10;
            double InnerDiameter_ID = 9600;
            double diameterHole = 5;
            double plateOverSize = 45;
            double plateThk = 6;
            double distanceBracket = 30;

            // Start Point of TopView : RefPoint 에서의 Y축으로 떨어진거리
            double distanceYofRef = bracketWidth + 150;

            // info.
            double bracketInnerWidth = bracketWidth - (bracketThk * 2);
            double radiusID = InnerDiameter_ID / 2;
            double radiusHole = diameterHole / 2;



            // bottom Plate
            Point3D bracketStartPoint = GetSumPoint(referencePoint, plateOverSize, 0);
            List<Line> bracketLineList = GetRectangle(bracketStartPoint, bracketWidth, bracketLength);
            Line bottomLine = bracketLineList[0];
            Line rightLine = bracketLineList[1];
            Line topLine = bracketLineList[2];
            Line leftLine = bracketLineList[3];

            Line guideLineB = new Line(GetSumPoint(bottomLine.StartPoint, holeDistanceOfOutline, holeDistanceOfOutline), GetSumPoint(bottomLine.EndPoint, -holeDistanceOfOutline, holeDistanceOfOutline));
            Line guideLineT = new Line(GetSumPoint(topLine.StartPoint, holeDistanceOfOutline, -holeDistanceOfOutline), GetSumPoint(topLine.EndPoint, -holeDistanceOfOutline, -holeDistanceOfOutline));

            Circle circleLT = new Circle(guideLineT.StartPoint, radiusHole);
            Circle circleRT = new Circle(guideLineT.EndPoint, radiusHole);
            Circle circleLB = new Circle(guideLineB.StartPoint, radiusHole);
            Circle circleRB = new Circle(guideLineB.EndPoint, radiusHole);

            Line hiddenInnerLeft = (Line)leftLine.Offset(-bracketThk, Vector3D.AxisZ);
            Line hiddenInnerRight = (Line)rightLine.Offset(bracketThk, Vector3D.AxisZ);

            hiddenLineList.AddRange(new Line[] { hiddenInnerLeft, hiddenInnerRight });

            //////////////////
            /// Top View 
            ////////////////// 

            Point3D topViewStartPoint = GetSumPoint(referencePoint, 0, distanceYofRef);
            Point3D topBracketStartPoint = GetSumPoint(topViewStartPoint, plateOverSize, 0);

            // Guide : circle for Arc
            Point3D circleCenter = GetSumPoint(topBracketStartPoint, bracketLength / 2, distanceBracket + radiusID);
            Circle circleOutlinePlate = new Circle(circleCenter, radiusID);
            Circle circleInlinePlate = new Circle(circleCenter, radiusID - plateThk);

            List<Line> guideTrimLine = GetRectangle(topViewStartPoint, distanceBracket * 2, bracketLength + (plateOverSize * 2));
            Line guideTrimLLine = guideTrimLine[3];
            Line guideTrimRLine = guideTrimLine[1];

            // Arc Point : trim guide circle
            Point3D[] arcTrimPoint = null;
            arcTrimPoint = circleOutlinePlate.IntersectWith(guideTrimLLine);
            Point3D arcLeftPoint = arcTrimPoint[0];
            arcTrimPoint = circleOutlinePlate.IntersectWith(guideTrimRLine);
            Point3D arcRightPoint = arcTrimPoint[0];

            // Arc : Inner Point
            Line guideTrimLInLine = new Line(arcLeftPoint, circleCenter);
            Line guideTrimRInLine = new Line(arcRightPoint, circleCenter);

            arcTrimPoint = circleInlinePlate.IntersectWith(guideTrimLInLine);
            Point3D arcInnerLPoint = arcTrimPoint[0];
            arcTrimPoint = circleInlinePlate.IntersectWith(guideTrimRInLine);
            Point3D arcInnerRPoint = arcTrimPoint[0];

            Arc arcPlateOutline = new Arc(circleCenter, arcLeftPoint, arcRightPoint);
            Arc arcPlateInline = new Arc(circleCenter, arcInnerLPoint, arcInnerRPoint);


            // Bracket outer/inner Line
            // width 값 +50 은 Plate Arc와의 Intersect를 위함
            List<Line> bracketLineListB = GetRectangle(topBracketStartPoint, distanceBracket + 50, bracketLength, RECTANGLUNVIEW.TOP);
            Line bottomLineB = bracketLineListB[0];
            Line rightLineB = bracketLineListB[1];
            Line leftLineB = bracketLineListB[2];

            List<Line> bracketLineListT = GetRectangle(GetSumPoint(topBracketStartPoint, bracketThk, bracketThk), distanceBracket + 50, bracketLength - (bracketThk * 2), RECTANGLUNVIEW.TOP);
            Line bottomLineT = bracketLineListT[0];
            Line rightLineT = bracketLineListT[1];
            Line leftLineT = bracketLineListT[2];


            // Bracket Height = Bracket Vertical intersect Arc
            arcTrimPoint = leftLineT.IntersectWith(arcPlateOutline);
            Point3D bracketLHeight = arcTrimPoint[0];

            // reset Bracket Height
            double bracketHeightY = bracketLHeight.Y;
            leftLineT.StartPoint.Y = bracketHeightY;
            rightLineT.StartPoint.Y = bracketHeightY;
            leftLineB.StartPoint.Y = bracketHeightY;
            rightLineB.StartPoint.Y = bracketHeightY;

            Line bracketleftTop = new Line(leftLineT.StartPoint, leftLineB.StartPoint);
            Line bracketRightTop = new Line(rightLineT.StartPoint, rightLineB.StartPoint);

            // Hole의 Side Draw를 위한 Rectangle 시작점
            Point3D innerLeftHolePoint = new Point3D(circleLT.Center.X - radiusHole,topViewStartPoint.Y); //GetSumPoint(topViewStartPoint, circleLT.Center.X - radiusHole, 0);
            Point3D innerRightHolePoint = new Point3D(circleRT.Center.X - radiusHole, topViewStartPoint.Y);

            // Draw : Rectangle of Hole
            List<Line> innerLeftHoleList = GetRectangle(innerLeftHolePoint, bracketThk, diameterHole);
            List<Line> innerRightHoleList = GetRectangle(innerRightHolePoint, bracketThk, diameterHole);


            //if (Curve.Fillet(leftLineB, bottomLineB, radiusHole, false, false, true, true, out Arc arcFillet01))
            //    topviewLineList.Add(arcFillet01);

            //if (Curve.Fillet(bottomLineB, rightLineB, radiusHole, true, false, true, true, out Arc arcFillet02))
            //    topviewLineList.Add(arcFillet02);



            if (Curve.Fillet(bottomLineT, leftLineT, radiusHole, true, true, true, true, out Arc arcFillet03))
                topviewLineList.Add(arcFillet03);
            Arc arcFilletOuter03 = (Arc)arcFillet03.Offset(bracketThk, Vector3D.AxisZ);
            topviewLineList.Add(arcFilletOuter03);

            if (Curve.Fillet(rightLineT, bottomLineT, radiusHole, true, false, true, true, out Arc arcFillet04))
                topviewLineList.Add(arcFillet04);
            Arc arcFilletOuter04 = (Arc)arcFillet04.Offset(-bracketThk, Vector3D.AxisZ);
            topviewLineList.Add(arcFilletOuter04);

            Line bottomLineBNew = new Line(GetSumPoint(arcFilletOuter03.EndPoint,0,0),GetSumPoint(arcFilletOuter04.EndPoint,0,0));
            Line rightLineBNew = new Line(GetSumPoint(rightLineB.StartPoint,0,0),GetSumPoint(arcFilletOuter04.StartPoint,0,0));
            Line leftLineBNew = new Line(GetSumPoint(leftLineB.StartPoint, 0, 0), GetSumPoint(arcFilletOuter03.StartPoint, 0, 0));

            lineList.AddRange(new Entity[]{
                    bottomLine, rightLine, topLine, leftLine,
                    circleLT, circleLB, circleRB, circleRT,
                    });

            topviewLineList.AddRange(new Entity[]{
                    bottomLineBNew, rightLineBNew, leftLineBNew,
                    bottomLineT, rightLineT, leftLineT,
                    bracketleftTop, bracketRightTop
                    });

            virtualLineList.AddRange(new Entity[]{
                    arcPlateOutline, arcPlateInline,
                    });

            hiddenLineList.AddRange(new Entity[]{

                    });

            topviewLineList.AddRange(innerLeftHoleList);
            topviewLineList.AddRange(innerRightHoleList);

            // Center Line : Red color
            Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            //styleService.SetLayerListEntity(ref lineList, layerService.LayerOutLine);
            //styleService.SetLayerListEntity(ref topviewLineList, layerService.LayerOutLine);
            //styleService.SetLayerListEntity(ref virtualLineList, layerService.LayerVirtualLine);
            //styleService.SetLayerListEntity(ref hiddenLineList, layerService.LayerHiddenLine);


            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(lineList);
            //singleModel.Entities.AddRange(topviewLineList);
            //singleModel.Entities.AddRange(virtualLineList);
            //singleModel.Entities.AddRange(hiddenLineList);

            //singleModel.Entities.Regen();
            //singleModel.Invalidate();

            //singleModel.SetView(viewType.Top);
            //singleModel.ZoomFit();
            //singleModel.Refresh();

            // Center Point
            Point3D modelCenterPoint = GetSumPoint(topLine.MidPoint, 0, -10*scaleValue);
            SetModelCenterPoint(PAPERSUB_TYPE.NamePlateBracket, modelCenterPoint);



            styleService.SetLayerListEntity(ref lineList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref topviewLineList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref virtualLineList, layerService.LayerVirtualLine);
            styleService.SetLayerListEntity(ref hiddenLineList, layerService.LayerHiddenLine);


            drawList.outlineList.AddRange(lineList);
            drawList.outlineList.AddRange(topviewLineList);
            drawList.outlineList.AddRange(virtualLineList);
            drawList.outlineList.AddRange(hiddenLineList);

            // S Line
            double sLineLength = plateThk;

            List<Entity> newSLineList1 = breakService.GetSLine(GetSumPoint(arcPlateInline.EndPoint, 0, 0), sLineLength,true,90);
            List<Entity> newSLineList2 = breakService.GetSLine(GetSumPoint(arcPlateInline.StartPoint, 0, 0), sLineLength, false, 90);
            styleService.SetLayerListEntity(ref newSLineList1, layerService.LayerVirtualLine);
            styleService.SetLayerListEntity(ref newSLineList2, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(newSLineList1);
            drawList.outlineList.AddRange(newSLineList2);


            // Center Line
            List<Entity> centerLineNewList = new List<Entity>();
            DrawCenterLineModel newCenterModel = new DrawCenterLineModel();
            centerLineNewList.AddRange(editingService.GetCenterLine(GetSumPoint(innerLeftHolePoint, diameterHole/2, bracketThk), GetSumPoint(innerLeftHolePoint, diameterHole / 2, 0), newCenterModel.detailCenterLength,scaleValue));
            centerLineNewList.AddRange(editingService.GetCenterLine(GetSumPoint(innerRightHolePoint, diameterHole/2, bracketThk), GetSumPoint(innerRightHolePoint, diameterHole / 2, 0), newCenterModel.detailCenterLength, scaleValue));

            centerLineNewList.AddRange(editingService.GetCenterLine(GetSumPoint(circleLT.Center, 0, radiusHole), GetSumPoint(circleLT.Center, 0, -radiusHole), newCenterModel.detailCenterLength, scaleValue));
            centerLineNewList.AddRange(editingService.GetCenterLine(GetSumPoint(circleLT.Center, -radiusHole, 0), GetSumPoint(circleLT.Center, radiusHole, 0), newCenterModel.detailCenterLength, scaleValue));
            centerLineNewList.AddRange(editingService.GetCenterLine(GetSumPoint(circleRT.Center, 0, radiusHole), GetSumPoint(circleRT.Center, 0, -radiusHole), newCenterModel.detailCenterLength, scaleValue));
            centerLineNewList.AddRange(editingService.GetCenterLine(GetSumPoint(circleRT.Center, -radiusHole, 0), GetSumPoint(circleRT.Center, radiusHole, 0), newCenterModel.detailCenterLength, scaleValue));
            centerLineNewList.AddRange(editingService.GetCenterLine(GetSumPoint(circleLB.Center, 0, radiusHole), GetSumPoint(circleLB.Center, 0, -radiusHole), newCenterModel.detailCenterLength, scaleValue));
            centerLineNewList.AddRange(editingService.GetCenterLine(GetSumPoint(circleLB.Center, -radiusHole, 0), GetSumPoint(circleLB.Center, radiusHole, 0), newCenterModel.detailCenterLength, scaleValue));
            centerLineNewList.AddRange(editingService.GetCenterLine(GetSumPoint(circleRB.Center, 0, radiusHole), GetSumPoint(circleRB.Center, 0, -radiusHole), newCenterModel.detailCenterLength, scaleValue));
            centerLineNewList.AddRange(editingService.GetCenterLine(GetSumPoint(circleRB.Center, -radiusHole, 0), GetSumPoint(circleRB.Center, radiusHole, 0), newCenterModel.detailCenterLength, scaleValue));


            centerLineNewList.AddRange(editingService.GetCenterLine(GetSumPoint(bracketStartPoint, bracketLength/2, 0), GetSumPoint(bracketStartPoint, bracketLength / 2 , bracketWidth), newCenterModel.detailCenterLength, scaleValue));
            centerLineNewList.AddRange(editingService.GetCenterLine(GetSumPoint(bracketStartPoint, 0, bracketWidth/2), GetSumPoint(bracketStartPoint, bracketLength , bracketWidth / 2), newCenterModel.detailCenterLength, scaleValue));

            centerLineNewList.AddRange(editingService.GetCenterLine(GetSumPoint(bottomLineBNew.MidPoint, 0, 0), GetSumPoint(arcPlateInline.MidPoint, 0,0), newCenterModel.detailCenterLength*2, scaleValue));

            styleService.SetLayerListEntity(ref centerLineNewList, layerService.LayerCenterLine);
            drawList.centerlineList.AddRange(centerLineNewList);


            // Dimension : Line
            double dimTop01 = 12;
            double dimTop02 = dimTop01 + 7;
            double dimRight01 = 15;
            double dimRight02 = dimRight01 + 7;


            // Dimension : Top
            DrawDimensionModel dimModel1 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = bracketLength.ToString(),
                dimHeight = dimTop02,
                scaleValue = scaleValue
            };
            DrawEntityModel testDimTest01 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(topLine.StartPoint, 0, 0), GetSumPoint(topLine.EndPoint, 0, 0), scaleValue, dimModel1);
            drawList.AddDrawEntity(testDimTest01);
            DrawDimensionModel dimModel2 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = (bracketLength- holeDistanceOfOutline*2).ToString(),
                dimHeight = dimTop01,
                scaleValue = scaleValue
            };
            DrawEntityModel testDimTest02 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(topLine.StartPoint, holeDistanceOfOutline, 0), GetSumPoint(topLine.EndPoint, -holeDistanceOfOutline, 0), scaleValue, dimModel2);
            drawList.AddDrawEntity(testDimTest02);
            DrawDimensionModel dimModel3 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpperPosition = POSITION_TYPE.LEFT,
                extLineLeftVisible = false,
                extLineRightVisible=false,
                arrowRightHeadVisible=false,
                arrowLeftHeadOut=true,
                textUpper = holeDistanceOfOutline.ToString(),
                dimHeight = dimTop01,
                scaleValue = scaleValue
            };
            DrawEntityModel testDimTest03 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(topLine.StartPoint, 0, 0), GetSumPoint(topLine.StartPoint, holeDistanceOfOutline, 0), scaleValue, dimModel3);
            drawList.AddDrawEntity(testDimTest03);
            DrawDimensionModel dimModel4 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpperPosition = POSITION_TYPE.RIGHT,
                extLineLeftVisible = false,
                extLineRightVisible = false,
                arrowLeftHeadVisible = false,
                arrowRightHeadOut = true,
                textUpper = holeDistanceOfOutline.ToString(),
                dimHeight = dimTop01,
                scaleValue = scaleValue
            };
            DrawEntityModel testDimTest04 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(topLine.EndPoint, -holeDistanceOfOutline, 0), GetSumPoint(topLine.EndPoint, 0, 0), scaleValue, dimModel4);
            drawList.AddDrawEntity(testDimTest04);

            // Dimension : Right
            DrawDimensionModel dimModel5 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.RIGHT,
                textUpper = bracketWidth.ToString(),
                dimHeight = dimRight02,
                scaleValue = scaleValue
            };
            DrawEntityModel testDimTest05 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(rightLine.EndPoint, 0, 0), GetSumPoint(rightLine.StartPoint, 0, 0), scaleValue, dimModel5);
            drawList.AddDrawEntity(testDimTest05);
            DrawDimensionModel dimModel6 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.RIGHT,
                textUpper = (bracketWidth - holeDistanceOfOutline*2).ToString(),
                dimHeight = dimRight01,
                scaleValue = scaleValue
            };
            DrawEntityModel testDimTest06 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(rightLine.EndPoint, 0, holeDistanceOfOutline), GetSumPoint(rightLine.StartPoint, 0, -holeDistanceOfOutline), scaleValue, dimModel6);
            drawList.AddDrawEntity(testDimTest06);
            DrawDimensionModel dimModel7 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.RIGHT,
                textUpperPosition = POSITION_TYPE.RIGHT,
                extLineLeftVisible = false,
                extLineRightVisible = false,
                arrowLeftHeadVisible = false,
                arrowRightHeadOut = true,
                textUpper = holeDistanceOfOutline.ToString(),
                dimHeight = dimRight01,
                scaleValue = scaleValue
            };
            DrawEntityModel testDimTest07 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(rightLine.StartPoint, 0, -holeDistanceOfOutline), GetSumPoint(rightLine.StartPoint, 0, 0), scaleValue, dimModel7);
            drawList.AddDrawEntity(testDimTest07);
            DrawDimensionModel dimModel8 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.RIGHT,
                textUpperPosition = POSITION_TYPE.LEFT,
                extLineLeftVisible = false,
                extLineRightVisible = false,
                arrowRightHeadVisible = false,
                arrowLeftHeadOut = true,
                textUpper = holeDistanceOfOutline.ToString(),
                dimHeight = dimRight01,
                scaleValue = scaleValue
            };
            DrawEntityModel testDimTest08 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(rightLine.EndPoint, 0, 0), GetSumPoint(rightLine.EndPoint, 0, holeDistanceOfOutline), scaleValue, dimModel8);
            drawList.AddDrawEntity(testDimTest08);


            // Dimension : Up Shape
            DrawDimensionModel dimModel9 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.RIGHT,
                textUpper = distanceBracket.ToString(),
                dimHeight = dimRight01,
                scaleValue = scaleValue
            };
            DrawEntityModel testDimTest09 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(arcFilletOuter04.EndPoint, 0, 0), GetSumPoint(arcPlateOutline.MidPoint, 0, 0), scaleValue, dimModel9);
            drawList.AddDrawEntity(testDimTest09);

            DrawDimensionModel dimModel10 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpperPosition = POSITION_TYPE.RIGHT,
                arrowLeftHeadVisible=false,
                extLineLeftVisible=false,
                extLineRightVisible=false,
                arrowRightHeadOut=true,
                textUpper = " R3",
                dimHeight = 0,
                scaleValue = scaleValue
            };
            DrawEntityModel testDimTest10 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(arcFillet04.MidPoint, 0, 0), GetSumPoint(arcFillet04.MidPoint, bracketThk, 0), scaleValue, dimModel10,Utility.DegToRad(-45));
            drawList.AddDrawEntity(testDimTest10);

            // Dimension : Leader
            string leaderString01 = "4-Φ5 HOLES";
            string leaderString02 = "FOR M4x15L B/N";
            string leaderBM01 = "10";
            DrawBMLeaderModel dimLeader01 = new DrawBMLeaderModel()
            {
                position = POSITION_TYPE.RIGHT,
                upperText = leaderString01,
                upperSecondText = leaderString02,
                leaderPointLength = dimRight01 *2 / Math.Sqrt(3),
                leaderPointRadian = Utility.DegToRad(-60),
                leaderLength = 20,
                bmNumber = leaderBM01,

            };
            DrawEntityModel dimEntityLeader01 = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(circleLB.Center, radiusHole, -radiusHole), dimLeader01, scaleValue);
            drawList.AddDrawEntity(dimEntityLeader01);




            return drawList;
        }

        public DrawEntityModel DrawEarthLug(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {

            DrawEntityModel drawList = new DrawEntityModel();

            // EARTH LUG

            //singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Entity> lineList = new List<Entity>();
            List<Entity> virtualLineList = new List<Entity>();

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            // Start Point of TopView : RefPoint 에서의 Y축으로 떨어진거리
            double distanceYofRef = 250;

            ///////////////////////
            ///  CAD Data Setting
            ///////////////////////
            double lugWidth = 70;
            double lugLength = 50;
            double lugEdgeRoundR = 10;
            double holeDistanceRight = 23;
            double holeDistanceTop = 17;
            double holeDiameter = 12;
            double holeCenterPointHeight = 200;

            double bottomPlateWidth = 9;
            double bottomPlateLength = 135;
            double VeticalPlateLength = 240 - bottomPlateWidth;
            double VerticalPlateDistanceL = 66;


            // info.
            double radiusHole = holeDiameter / 2;



            // bottom Plate
            Point3D bottomStartPoint = GetSumPoint(referencePoint, 0, 0);
            List<Line> bottomLineList = GetRectangle(bottomStartPoint, bottomPlateWidth, bottomPlateLength, RECTANGLUNVIEW.LEFT);
            Line bBottomLine = bottomLineList[0];
            Line bTopLine = bottomLineList[2];

            // vertical Plate
            Point3D verticalStartPoint = GetSumPoint(referencePoint, VerticalPlateDistanceL, bottomPlateWidth);
            List<Line> verticalLineList = GetRectangle(verticalStartPoint, VeticalPlateLength, bottomPlateWidth, RECTANGLUNVIEW.TOP);
            Line vRightLine = verticalLineList[1];
            Line vLeftLine = verticalLineList[2];
            List<Entity> verticalLineListEntity = new List<Entity>();
            verticalLineListEntity.Add(vRightLine);
            verticalLineListEntity.Add(vLeftLine);

            // Lug Rectangle
            Point3D lugStartPoint = GetSumPoint(referencePoint, VerticalPlateDistanceL + bottomPlateWidth, holeCenterPointHeight + holeDistanceTop - lugWidth);
            List<Line> lugLineList = GetRectangle(lugStartPoint, lugWidth, lugLength);
            Line lugBottomLine = lugLineList[0];
            Line lugRightLine = lugLineList[1];
            Line lugTopLine = lugLineList[2];
            Line lugLeftLine = lugLineList[3];

            // Hole
            Point3D holeCenterPoint = GetSumPoint(lugTopLine.EndPoint, -holeDistanceRight, -holeDistanceTop);
            Circle circleHoleLug = new Circle(holeCenterPoint, radiusHole);

            // Lug Arc
            if (Curve.Fillet(lugTopLine, lugRightLine, lugEdgeRoundR, false, false, true, true, out Arc arcFillet01))
                lineList.Add(arcFillet01);

            if (Curve.Fillet(lugRightLine, lugBottomLine, lugEdgeRoundR, true, false, true, true, out Arc arcFillet02))
                lineList.Add(arcFillet02);


            lineList.AddRange(lugLineList);
            lineList.Add(circleHoleLug);
            virtualLineList.AddRange(bottomLineList);
            //virtualLineList.AddRange(verticalLineList);



            // Center Line : Red color
            //Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            //centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            //styleService.SetLayerListEntity(ref lineList, layerService.LayerOutLine);
            //styleService.SetLayerListEntity(ref virtualLineList, layerService.LayerVirtualLine);


            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(lineList);
            //singleModel.Entities.AddRange(virtualLineList);

            //singleModel.Entities.Regen();
            //singleModel.Invalidate();

            //singleModel.SetView(viewType.Top);
            //singleModel.ZoomFit();
            //singleModel.Refresh();


            // Center Point
            Point3D modelCenterPoint = GetSumPoint(vRightLine.StartPoint, 0, -25);
            SetModelCenterPoint(PAPERSUB_TYPE.EarthLug, modelCenterPoint);


            styleService.SetLayerListEntity(ref lineList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref virtualLineList, layerService.LayerVirtualLine);

            drawList.outlineList.AddRange(lineList);
            drawList.outlineList.AddRange(virtualLineList);




            // S Line
            double sLineLength = bottomPlateWidth;

            List<Entity> newSDoubleLineList = breakService.GetSDoubleLine(GetSumPoint(vLeftLine.EndPoint, 0, 100), sLineLength, scaleValue);
            List<Entity> newListList = breakService.GetTrimSDoubleLine(verticalLineListEntity, newSDoubleLineList);
            styleService.SetLayerListEntity(ref newSDoubleLineList, layerService.LayerDimension);
            drawList.outlineList.AddRange(newSDoubleLineList);
            styleService.SetLayerListEntity(ref newListList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(newListList);

            List<Entity> newSLineList1 = breakService.GetSLine(GetSumPoint(vLeftLine.StartPoint, 0, 0), sLineLength, true, 0);
            List<Entity> newSLineList2 = breakService.GetSLine(GetSumPoint(bTopLine.StartPoint, 0, 0), sLineLength, false, 90);
            styleService.SetLayerListEntity(ref newSLineList1, layerService.LayerDimension);
            styleService.SetLayerListEntity(ref newSLineList2, layerService.LayerDimension);
            drawList.outlineList.AddRange(newSLineList1);
            drawList.outlineList.AddRange(newSLineList2);


            DrawDimensionModel dimModel4 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = "50",
                dimHeight = 12,
                scaleValue = scaleValue
            };
            DrawEntityModel testDimTest04 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lugTopLine.StartPoint, 0, 0), GetSumPoint(lugRightLine.StartPoint, 0, 0), scaleValue, dimModel4);
            drawList.AddDrawEntity(testDimTest04);

            // Dimension : Right
            DrawDimensionModel dimModel5 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.RIGHT,
                textUpper = "300",
                dimHeight = 15,
                scaleValue = scaleValue
            };
            DrawEntityModel testDimTest05 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(bBottomLine.EndPoint, 0, 0), GetSumPoint(circleHoleLug.Center, radiusHole + 7, 0), scaleValue, dimModel5);
            drawList.AddDrawEntity(testDimTest05);



            // Welding
            List<Entity> weldingList = new List<Entity>();
            DrawWeldSymbols weldSymbols = new DrawWeldSymbols();
            DrawWeldSymbolModel wsModel02 = new DrawWeldSymbolModel();
            wsModel02.position = ORIENTATION_TYPE.BOTTOMLEFT;
            wsModel02.weldTypeUp = WeldSymbol_Type.Fillet;
            wsModel02.weldTypeDown = WeldSymbol_Type.Fillet;
            wsModel02.weldDetailType = WeldSymbolDetail_Type.BothSide;
            wsModel02.weldLength1 = "5";
            wsModel02.tailVisible = false;
            wsModel02.leaderAngle = 45;
            wsModel02.leaderLineLength = 17;
            weldingList.AddRange(weldSymbols.GetWeldSymbol(GetSumPoint(lugLeftLine.StartPoint, 0, -20), singleModel, scaleValue, wsModel02));
            styleService.SetLayerListEntity(ref weldingList, layerService.LayerDimension);
            drawList.outlineList.AddRange(weldingList);

            // Dimension : Leader
            string leaderString01 = "Φ12 HOLE";
            string leaderBM01 = "11";
            DrawBMLeaderModel dimLeader01 = new DrawBMLeaderModel()
            {
                position = POSITION_TYPE.RIGHT,
                upperText = leaderString01,
                leaderPointLength = 12,
                leaderPointRadian = Utility.DegToRad(60),
                leaderLength = 15,
                //bmNumber = leaderBM01,

            };
            DrawEntityModel dimEntityLeader01 = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(circleHoleLug.Center, radiusHole, radiusHole), dimLeader01, scaleValue);
            drawList.AddDrawEntity(dimEntityLeader01);

            List<Entity> centerLineNewList = new List<Entity>();
            DrawCenterLineModel newCenterModel = new DrawCenterLineModel();
            centerLineNewList.AddRange(editingService.GetCenterLine(GetSumPoint(circleHoleLug.Center, 0, radiusHole), GetSumPoint(circleHoleLug.Center, 0 ,-radiusHole), newCenterModel.detailCenterLength, scaleValue));
            centerLineNewList.AddRange(editingService.GetCenterLine(GetSumPoint(circleHoleLug.Center, -radiusHole,0), GetSumPoint(circleHoleLug.Center, radiusHole , 0), newCenterModel.detailCenterLength, scaleValue));
            styleService.SetLayerListEntity(ref centerLineNewList, layerService.LayerCenterLine);
            drawList.outlineList.AddRange(centerLineNewList);


            return drawList;

        }

        public DrawEntityModel DrawCompressorRing(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {

            DrawEntityModel drawList = new DrawEntityModel();

            // COMP. RING

            //singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Entity> lineList = new List<Entity>();

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            // Start Point of TopView : RefPoint 에서의 Y축으로 떨어진거리

            ///////////////////////
            ///  CAD Data Setting
            ///////////////////////
            
            double plateLength = valueService.GetDoubleValue( assemblyData.RoofCompressionRing[0].PlateLength);
            double plateThk = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].ShellThicknessThickenedT1);
            double complectionID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            int lastCourseCount = assemblyData.ShellOutput.Count-1;
            double beforeCourseThk =valueService.GetDoubleValue(assemblyData.ShellOutput[lastCourseCount].Thickness);

            double bendingPlateThk = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].WidthD);

            // INFO : after data
            double plateThkAdj = shapeCalService.GetCompRingKTypeThkAdj(plateThk, beforeCourseThk);
            double radiusInner = (complectionID + plateThkAdj * 2) / 2;
            double radiusouter = (complectionID + plateThkAdj * 2 + bendingPlateThk * 2) / 2;

            // INFO : After calculate of data
            double totalBendingPlate = 0;


            //////////////////////////////////////////////////////////
            //// Calculate
            //////////////////////////////////////////////////////////

            totalBendingPlate = GetNeedCompRIngCount(radiusouter, plateLength, bendingPlateThk);

            double eachPlateDegree = 360 / totalBendingPlate;
            double eachPlateHalfDeg = eachPlateDegree / 2;

            // testArc : Bending Plate
            Point3D arcCenterPoint = GetSumPoint(referencePoint, 0, -radiusInner);
            // 90 : 시작점이  X축의 오른쪽 수평선, 기준을 수직으로 만들기 위함
            Arc testArc = new Arc(arcCenterPoint, radiusouter, Utility.DegToRad(90 + eachPlateHalfDeg), Utility.DegToRad(90 - eachPlateHalfDeg));
            double arcHeight = testArc.MidPoint.Y - testArc.StartPoint.Y;
            double arcChordLength = testArc.EndPoint.X - testArc.StartPoint.X;

            // Reset Arc CenterPoint : + arcHeight
            arcCenterPoint = GetSumPoint(arcCenterPoint, arcChordLength / 2, arcHeight);
            Arc innerBendingPlate = new Arc(arcCenterPoint, radiusInner, Utility.DegToRad(90 + eachPlateHalfDeg), Utility.DegToRad(90 - eachPlateHalfDeg));
            Arc outerBendingPlate = new Arc(arcCenterPoint, radiusouter, Utility.DegToRad(90 + eachPlateHalfDeg), Utility.DegToRad(90 - eachPlateHalfDeg));

            Line sideLeftLine = GetDiagonalLineOfArc(radiusouter, arcCenterPoint, eachPlateHalfDeg, innerBendingPlate, outerBendingPlate);
            Line sideRightLine = GetDiagonalLineOfArc(radiusouter, arcCenterPoint, -eachPlateHalfDeg, innerBendingPlate, outerBendingPlate);


            lineList.AddRange(new Entity[] {
                                            innerBendingPlate, outerBendingPlate,
                                            sideLeftLine, sideRightLine,
                                        });

            /*    // Bend Plate 의 ARC / CHD 값 확인
            double innerCHD = innerBendingPlate.EndPoint.X - innerBendingPlate.StartPoint.X;
            double outerCHD = outerBendingPlate.EndPoint.X - outerBendingPlate.StartPoint.X;
            double arcLength = outerBendingPlate.Length();
            /**/

            // Center Line : Red color
            //Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            //centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            //styleService.SetLayerListEntity(ref lineList, layerService.LayerOutLine);


            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(lineList);

            //singleModel.Entities.Regen();
            //singleModel.Invalidate();

            //singleModel.SetView(viewType.Top);
            //singleModel.ZoomFit();
            //singleModel.Refresh();


            styleService.SetLayerListEntity(ref lineList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(lineList);



            // Scale
            PaperAreaModel newPaperModel = SingletonData.PaperArea.GetAreaModel(PAPERMAIN_TYPE.DETAIL, PAPERSUB_TYPE.ComRing);
            double shapeWidth = outerBendingPlate.StartPoint.DistanceTo(outerBendingPlate.EndPoint);
            newPaperModel.ScaleValue=scaleService.GetScaleCalValue(newPaperModel.otherWidth, newPaperModel.otherHeight, shapeWidth, arcHeight);
            scaleValue = newPaperModel.ScaleValue;


            // Dimension
            string dimOuterArc1 = "ARC : " + Math.Round(outerBendingPlate.Length(),1,MidpointRounding.AwayFromZero);
            string dimOuterArc2 = "CHD : " + Math.Round(outerBendingPlate.StartPoint.DistanceTo(outerBendingPlate.EndPoint),1,MidpointRounding.AwayFromZero);

            string dimInnerArc1 = "CHD : " + Math.Round(innerBendingPlate.StartPoint.DistanceTo(innerBendingPlate.EndPoint), 1, MidpointRounding.AwayFromZero); ;
            string dimArcAngle = Math.Round(eachPlateDegree,1,MidpointRounding.AwayFromZero) + "˚";
            string dimThk = bendingPlateThk.ToString();

            string dimOuterRadius = "R" + radiusouter.ToString();
            string dimInnerRadius = "R" + radiusInner.ToString();
            double refdimHeight = 20;
            double refdimMidHeight = 10;
            double refdimArcLength = 20;


            // Dimension : Arc Line
            Arc innerBendingPlateDimLine = new Arc(Plane.XY, arcCenterPoint, radiusInner, GetSumPoint(innerBendingPlate.EndPoint,1*scaleValue,0) ,GetSumPoint(innerBendingPlate.EndPoint, refdimArcLength / 2* scaleValue, 0),true);
            Arc outerBendingPlateDimLine = new Arc(Plane.XY, arcCenterPoint, radiusouter, GetSumPoint(innerBendingPlate.EndPoint, 1 * scaleValue, 0), GetSumPoint(innerBendingPlate.EndPoint, refdimArcLength * scaleValue, 0), true);

            Point3D insertInnerDimPoint = editingService.GetIntersectLength(innerBendingPlateDimLine, 1 * scaleValue,false);
            Point3D insertOuterDimPoint = editingService.GetIntersectLength(outerBendingPlateDimLine, 1 * scaleValue,false);

            // Leader : Bottom
            DrawBMLeaderModel dimLeadder1 = new DrawBMLeaderModel() { position = POSITION_TYPE.BOTTOM, upperText = dimInnerRadius, leaderLength= refdimArcLength *1.5};
            DrawEntityModel testDimArcLeader01 = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(insertInnerDimPoint, 0, 0), dimLeadder1, scaleValue,Utility.DegToRad(-eachPlateHalfDeg));
            drawList.AddDrawEntity(testDimArcLeader01);

            DrawBMLeaderModel dimLeadder2 = new DrawBMLeaderModel() { position = POSITION_TYPE.BOTTOM, upperText = dimOuterRadius, leaderLength = refdimArcLength * 1.5 };
            DrawEntityModel testDimArcLeader02 = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(insertOuterDimPoint, 0, 0), dimLeadder2, scaleValue, Utility.DegToRad(-eachPlateHalfDeg));
            drawList.AddDrawEntity(testDimArcLeader02);




            styleService.SetLayer(ref innerBendingPlateDimLine, layerService.LayerDimension);
            styleService.SetLayer(ref outerBendingPlateDimLine, layerService.LayerDimension);
            drawList.outlineList.Add(innerBendingPlateDimLine);
            drawList.outlineList.Add(outerBendingPlateDimLine);







            DrawDimensionModel dimModel1 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = dimOuterArc1,
                textLower = dimOuterArc2,
                dimHeight = refdimHeight,
                scaleValue = scaleValue
            };
            DrawEntityModel testDimTest01 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(outerBendingPlate.StartPoint, 0, 0), GetSumPoint(outerBendingPlate.EndPoint, 0, 0), scaleValue, dimModel1);
            drawList.AddDrawEntity(testDimTest01);
            DrawDimensionModel dimModel2 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.BOTTOM,
                textUpper = dimInnerArc1,
                dimHeight = refdimHeight/3,
                scaleValue = scaleValue
            };
            DrawEntityModel testDimTest02 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(innerBendingPlate.StartPoint, 0, 0), GetSumPoint(innerBendingPlate.EndPoint, 0, 0), scaleValue, dimModel2);
            drawList.AddDrawEntity(testDimTest02);


            // Dimension : Arc
            double dimleftArcDegree = -180 + eachPlateHalfDeg;
            double dimrightArcDegree = 180 - eachPlateHalfDeg;
            DrawEntityModel testDimArc01 = drawService.Draw_DimensionArc(GetSumPoint(innerBendingPlate.StartPoint, 0, 0), GetSumPoint(innerBendingPlate.EndPoint, 0, 0), "top",  -refdimHeight, dimArcAngle, 45, dimleftArcDegree, dimrightArcDegree, 0, scaleValue, layerService.LayerDimension);
            drawList.AddDrawEntity(testDimArc01);



            DrawDimensionModel dimModel31 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.BOTTOM,
                textUpper = dimThk,
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                textUpperPosition = POSITION_TYPE.RIGHT,
                textSizeVisible = false,
                dimHeight = refdimMidHeight,
                leftPointRotate=false,
                rightPointRotate=true,
                scaleValue = scaleValue
            };
            DrawEntityModel testDimTest031 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(sideLeftLine.StartPoint, -sideLeftLine.Length(), 0), GetSumPoint(sideLeftLine.StartPoint, 0, 0), scaleValue, dimModel31, Utility.DegToRad(-90+ eachPlateHalfDeg));
            drawList.AddDrawEntity(testDimTest031);



            // Center Point
            Point3D modelCenterPoint = GetSumPoint(outerBendingPlate.MidPoint, 0, 20*scaleValue);
            SetModelCenterPoint(PAPERSUB_TYPE.ComRing, modelCenterPoint);



            return drawList;

        }

        public DrawEntityModel DrawCuttingPlan_COMP_Ring(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {

            DrawEntityModel drawList = new DrawEntityModel();

            // Cutting Plan COMP. RING

            //singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Entity> lineList = new List<Entity>();
            List<Entity> virtualLineList = new List<Entity>();

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            // Start Point of TopView : RefPoint 에서의 Y축으로 떨어진거리

            ///////////////////////
            ///  CAD Data Setting
            ///////////////////////

            double plateWidth = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].PlateWidth);
            double plateLength = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].PlateLength);
            double plateThk = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].ShellThicknessThickenedT1);
            double complectionID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            int lastCourseCount = assemblyData.ShellOutput.Count - 1;
            double beforeCourseThk = valueService.GetDoubleValue(assemblyData.ShellOutput[lastCourseCount].Thickness);

            double bendingPlateThk = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].WidthD);

            // INFO : after data
            double plateThkAdj = shapeCalService.GetCompRingKTypeThkAdj(plateThk, beforeCourseThk);
            double radiusInner = (complectionID + plateThkAdj * 2) / 2;
            double radiusouter = (complectionID + plateThkAdj * 2 + bendingPlateThk * 2) / 2;

            //double plateWidth = 2740;
            //double plateLength = 9300;
            //double plateThk = 24;
            //double complectionID = 32400;
            //double beforeCourseThk = 10;

            //double bendingPlateThk = 320;
            double distanceEachbendingPlate = 15;


            // INFO : after data
            //double radiusInner = topRingCenterRadius + (plateThk / 2);
            //double radiusouter = radiusInner + bendingPlateThk;

            // INFO : After calculate of data
            double totalPlate = 0;
            double totalBendingPlate = 0;
            // double bendingPlateLength = 0;
            double maxBendPlateOfOnePlate = 0;
            double averageBendPlateOfOnePlate = 0;


            //////////////////////////////////////////////////////////
            //// Calculate
            //////////////////////////////////////////////////////////

            totalBendingPlate = GetNeedCompRIngCount(radiusouter, plateLength, bendingPlateThk);

            double eachPlateDegree = 360 / totalBendingPlate;
            double eachPlateHalfDeg = eachPlateDegree / 2;

            // testArc : Bending Plate
            Arc testArc = new Arc(referencePoint, radiusouter, Utility.DegToRad(90 + eachPlateHalfDeg), Utility.DegToRad(90 - eachPlateHalfDeg));
            double arcHeight = testArc.MidPoint.Y - testArc.StartPoint.Y;
            double firstBendPlateHeight = arcHeight + bendingPlateThk;
            double arcChordLength = testArc.EndPoint.X - testArc.StartPoint.X;

            // Calculate : MaxCreate bendingPlate of OnePlate ,  total Plate No.
            double emptyWidth = plateWidth - firstBendPlateHeight - distanceEachbendingPlate;
            double spaceYEachBendPlate = bendingPlateThk + distanceEachbendingPlate;
            maxBendPlateOfOnePlate = Math.Truncate(emptyWidth / spaceYEachBendPlate) + 1; // +1 : first BendingPlate

            // totalPlate  : CompRIng을 만들기 위해 필요한 Plate 수량
            totalPlate = Math.Ceiling(totalBendingPlate / maxBendPlateOfOnePlate);
            averageBendPlateOfOnePlate = Math.Ceiling(totalBendingPlate / totalPlate);
            if (averageBendPlateOfOnePlate > maxBendPlateOfOnePlate) { averageBendPlateOfOnePlate = maxBendPlateOfOnePlate; }

            /////////////////////////////////////////////////////////////////////////////////////////

            // Reset Arc CenterPoint : + arcHeight
            double firstArcStartPointY = plateWidth - distanceEachbendingPlate - radiusouter;
            Point3D arcCenterPoint = GetSumPoint(referencePoint, plateLength / 2, firstArcStartPointY);


            //////////////////////////////////////////////////////////
            // Draw BendingPlate
            //////////////////////////////////////////////////////////

            Arc dimFristArc = null;
            for (int i = 0; i < averageBendPlateOfOnePlate; i++)
            {
                Arc outerBendingPlate = new Arc(arcCenterPoint, radiusouter, Utility.DegToRad(90 + eachPlateHalfDeg), Utility.DegToRad(90 - eachPlateHalfDeg));
                Arc innerBendingPlate = new Arc(arcCenterPoint, radiusInner, Utility.DegToRad(90 + eachPlateHalfDeg), Utility.DegToRad(90 - eachPlateHalfDeg));
                if (i == 0) dimFristArc = outerBendingPlate;
                Line sideLeftLine = GetDiagonalLineOfArc(radiusouter, arcCenterPoint, eachPlateHalfDeg, innerBendingPlate, outerBendingPlate);
                Line sideRightLine = GetDiagonalLineOfArc(radiusouter, arcCenterPoint, -eachPlateHalfDeg, innerBendingPlate, outerBendingPlate);

                lineList.AddRange(new Entity[] {
                                            innerBendingPlate, outerBendingPlate,
                                            sideLeftLine, sideRightLine,
                });

                arcCenterPoint.Y -= spaceYEachBendPlate;
            }

            // Draw : Plate
            Point3D plateStartPoint = GetSumPoint(referencePoint, 0, 0);
            List<Line> plateLineList = GetRectangle(plateStartPoint, plateWidth, plateLength);
            Line pTopLine = plateLineList[2];

            // Center Line : Red color
            //Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            //centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            //styleService.SetLayerListEntity(ref lineList, layerService.LayerOutLine);


            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(lineList);

            //singleModel.Entities.Regen();
            //singleModel.Invalidate();

            //singleModel.SetView(viewType.Top);
            //singleModel.ZoomFit();
            //singleModel.Refresh();


            styleService.SetLayerListEntity(ref lineList, layerService.LayerOutLine);
            styleService.SetLayerListLine(ref plateLineList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(plateLineList);
            drawList.outlineList.AddRange(lineList);





            // Scale
            PaperAreaModel newPaperModel = SingletonData.PaperArea.GetAreaModel(PAPERMAIN_TYPE.DETAIL, PAPERSUB_TYPE.ComRingCuttingPlan);
            newPaperModel.ScaleValue = scaleService.GetScaleCalValue(newPaperModel.otherWidth, newPaperModel.otherHeight, plateLength, plateWidth);
            newPaperModel.TitleSubName = "(SCALE : 1/" + newPaperModel.ScaleValue + ")";
            scaleValue = newPaperModel.ScaleValue;


            // Dimension
            double dimTopLength01 = 15;
            double dimTopLength02 = 12;
            double dimLeftLength = 15;
            string dimTop01 = Math.Round(plateLength, 1, MidpointRounding.AwayFromZero).ToString();
            string dimLeft01 = Math.Round(plateWidth, 1, MidpointRounding.AwayFromZero).ToString();
            string dimReqCount = "REQ'D : " + totalPlate + " SH'T";
            string dimLeaderStr01 = "t" + plateThk + " COMP. RING";
            string dimLeaderNumber = "9";

            DrawDimensionModel dimModelTop01 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = dimTop01,
                dimHeight = dimTopLength01,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntityTop01 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(pTopLine.StartPoint, 0, 0), GetSumPoint(pTopLine.StartPoint, plateLength, 0), scaleValue, dimModelTop01);
            drawList.AddDrawEntity(dimEntityTop01);

            DrawDimensionModel dimModelLeft02 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.LEFT,
                textUpper = dimLeft01,
                dimHeight = dimLeftLength,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntityLeft02 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(pTopLine.StartPoint, 0, 0), GetSumPoint(pTopLine.StartPoint, 0, -plateWidth), scaleValue, dimModelLeft02);
            drawList.AddDrawEntity(dimEntityLeft02);

            DrawBMLeaderModel dimLeader01 = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = dimReqCount, arrowHeadVisible = false, leaderLength = 20 };
            DrawEntityModel dimEntityLeader01 = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(pTopLine.StartPoint, plateLength + 1*scaleValue, -plateWidth), dimLeader01, scaleValue);
            drawList.AddDrawEntity(dimEntityLeader01);


            DrawBMLeaderModel dimLeader02 = new DrawBMLeaderModel()
            {
                position = POSITION_TYPE.RIGHT,
                upperText = dimLeaderStr01,
                leaderPointLength = 28,
                leaderPointRadian = Utility.DegToRad(70),
                leaderLength = 20,
                bmNumber = dimLeaderNumber,

            };

            Point3D leaderInsertPoint = GetSumPoint(pTopLine.StartPoint, plateLength * 0.7, -bendingPlateThk / 2);
            Line dimFirstArcLine = new Line(GetSumPoint(leaderInsertPoint, 0, 0), GetSumPoint(leaderInsertPoint, 0, -plateWidth));
            Point3D[] dimFirstInter = dimFristArc.IntersectWith(dimFirstArcLine);
            if (dimFirstInter.Length > 0)
                leaderInsertPoint = GetSumPoint(dimFirstInter[0],0, -bendingPlateThk / 2);

            DrawEntityModel dimEntityLeader02 = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(leaderInsertPoint, 0, 0), dimLeader02, scaleValue);
            drawList.AddDrawEntity(dimEntityLeader02);

            double sLineLength = 3 * scaleValue;

            List<Entity> newSLineList = breakService.GetSLine(GetSumPoint(leaderInsertPoint, 0, 0), sLineLength);
            styleService.SetLayerListEntity(ref newSLineList, layerService.LayerDimension);
            drawList.outlineList.AddRange(newSLineList);


            // Center Point
            Point3D modelCenterPoint = GetSumPoint(pTopLine.MidPoint, 0, 0);
            SetModelCenterPoint(PAPERSUB_TYPE.ComRingCuttingPlan, modelCenterPoint);









            return drawList;

        }

        public DrawEntityModel DrawCuttingPlan_TOP_Ring(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {
            DrawEntityModel drawList = new DrawEntityModel();

            // CuttingPlan_TOP_Ring

            //singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Entity> lineList = new List<Entity>();
            List<Entity> virtualLineList = new List<Entity>();

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            // Start Point of TopView : RefPoint 에서의 Y축으로 떨어진거리

            ///////////////////////
            ///  CAD Data Setting
            ///////////////////////


            double plateWidth = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].PlateWidth);
            double plateLength = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].PlateLength);
            double plateThk = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].ShellThicknessThickenedT1);
            double complectionID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            int lastCourseCount = assemblyData.ShellOutput.Count - 1;
            double beforeCourseThk = valueService.GetDoubleValue(assemblyData.ShellOutput[lastCourseCount].Thickness);

            double bendingPlateThk = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].WidthD);

            // INFO : after data
            double plateThkAdj = shapeCalService.GetCompRingKTypeThkAdj(plateThk, beforeCourseThk);
            double radiusInner = (complectionID + plateThkAdj * 2) / 2;
            double radiusouter = (complectionID + plateThkAdj * 2 + bendingPlateThk * 2) / 2;

            //double plateWidth = 2740;
            //double plateLength = 9300;
            //double plateThk = 24;
            //double complectionID = 32400;
            //double beforeCourseThk = 10;

            double topRingWidth = bendingPlateThk;
            double distanceEachTopRing = 15;


            // INFO : after data
            double topRingCenterRadius = radiusInner;

            // INFO : After calculate of data
            double totalPlate = 0;
            double totalTopRing = 0;
            double topRingLength = 0;
            double maxTopRIngOfOnePlate = 0;
            double averageTopRIngOfOnePlate = 0;


            // INFO : after data, Need for bendingPlate count
            //double bendingPlateThk = 320;
            //double radiusInner = topRingCenterRadius + (plateThk / 2);
            //double radiusouter = radiusInner + bendingPlateThk;
            double totalBendingPlate = 0;


            //////////////////////////////////////////////////////////
            //// Calculate
            //////////////////////////////////////////////////////////

            /*   // Count TopRing  ==  Count COMP.RING  같게하기 위해서 주석처리, 주석내용은 TopRing의 Center Radius 에 의한 원주율로 Plate길이로 나눔
            // Calculate : TopRing Length
            double TopRingCircumference = 2 * topRingCenterRadius * Math.PI;
            double minTopRingCount = Math.Ceiling(TopRingCircumference / plateLength);
            totalTopRing = minTopRingCount;
            topRingLength = TopRingCircumference/ totalTopRing;
            /**/

            // TopRing의 갯수를 COMP.Ring 수량과 같이 설정
            totalTopRing = GetNeedCompRIngCount(radiusouter, plateLength, bendingPlateThk);

            double TopRingCircumference = 2 * topRingCenterRadius * Math.PI;
            topRingLength = TopRingCircumference / totalTopRing;
            /**/

            // Calculate : MaxCreate bendingPlate of OnePlate ,  total Plate No.
            double emptyWidth = plateWidth - topRingWidth;
            double spaceYEachTopRing = topRingWidth + distanceEachTopRing;
            maxTopRIngOfOnePlate = Math.Truncate(emptyWidth / spaceYEachTopRing) + 1; // +1 : first BendingPlate

            // totalPlate  : TopRIng을 만들기 위해 필요한 Plate 수량
            totalPlate = Math.Ceiling(totalTopRing / maxTopRIngOfOnePlate);
            averageTopRIngOfOnePlate = Math.Ceiling(totalTopRing / totalPlate);
            if (averageTopRIngOfOnePlate > maxTopRIngOfOnePlate) { averageTopRIngOfOnePlate = maxTopRIngOfOnePlate; }

            //////////////////////////////////////////////////////////
            // Draw TopRIng
            //////////////////////////////////////////////////////////

            // Draw : Plate
            Point3D plateStartPoint = GetSumPoint(referencePoint, 0, 0);
            List<Line> plateLineList = GetRectangle(plateStartPoint, plateWidth, plateLength);
            Line pTopLine = plateLineList[2];

            Point3D topRingStartPoint = GetSumPoint(pTopLine.StartPoint, 0, 0);

            // Draw : TopRIng
            for (int i = 0; i < averageTopRIngOfOnePlate; i++)
            {
                lineList.AddRange(GetRectangleLT(topRingStartPoint, topRingWidth, topRingLength));

                topRingStartPoint.Y -= spaceYEachTopRing;
            }




            // Center Line : Red color
            //Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            //centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            //styleService.SetLayerListEntity(ref lineList, layerService.LayerOutLine);


            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(lineList);

            //singleModel.Entities.Regen();
            //singleModel.Invalidate();

            //singleModel.SetView(viewType.Top);
            //singleModel.ZoomFit();
            //singleModel.Refresh();


            styleService.SetLayerListEntity(ref lineList, layerService.LayerOutLine);
            styleService.SetLayerListLine(ref plateLineList, layerService.LayerVirtualLine);
            lineList.AddRange(plateLineList);

            drawList.outlineList.AddRange(lineList);


            // Scale
            PaperAreaModel newPaperModel = SingletonData.PaperArea.GetAreaModel(PAPERMAIN_TYPE.DETAIL, PAPERSUB_TYPE.TopRingCuttingPlan);
            newPaperModel.ScaleValue = scaleService.GetScaleCalValue(newPaperModel.otherWidth, newPaperModel.otherHeight, plateLength, plateWidth);
            newPaperModel.TitleSubName = "(SCALE : 1/" + newPaperModel.ScaleValue + ")";
            scaleValue = newPaperModel.ScaleValue;


            // Dimension
            double dimTopLength01 = 12+7;
            double dimTopLength02 = 12;
            double dimLeftLength = 15;
            string dimTop01 = Math.Round(plateLength, 1, MidpointRounding.AwayFromZero).ToString();
            string dimTop02 = Math.Round(topRingLength, 1, MidpointRounding.AwayFromZero).ToString();
            string dimLeft01 = Math.Round(plateWidth, 1, MidpointRounding.AwayFromZero).ToString();
            string dimReqCount= "REQ'D : " + totalPlate + " SH'T";
            string dimLeaderStr01 = "TOP MOST THICKENED";
            string dimLeaderStr02 = "SHELL RING";
            string dimLeaderNumber = "8";

            DrawDimensionModel dimModelTop01 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = dimTop01,
                dimHeight = dimTopLength01,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntityTop01 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(pTopLine.StartPoint, 0, 0), GetSumPoint(pTopLine.StartPoint, plateLength, 0), scaleValue, dimModelTop01);
            drawList.AddDrawEntity(dimEntityTop01);
            DrawDimensionModel dimModelTop02 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = dimTop02,
                dimHeight = dimTopLength02,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntityTop02 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(pTopLine.StartPoint, 0, 0), GetSumPoint(pTopLine.StartPoint, topRingLength, 0), scaleValue, dimModelTop02);
            drawList.AddDrawEntity(dimEntityTop02);
            DrawDimensionModel dimModelLeft02 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.LEFT,
                textUpper = dimLeft01,
                dimHeight = dimLeftLength,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntityLeft02 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(pTopLine.StartPoint, 0, 0), GetSumPoint(pTopLine.StartPoint, 0, -plateWidth), scaleValue, dimModelLeft02);
            drawList.AddDrawEntity(dimEntityLeft02);

            DrawBMLeaderModel dimLeader01 = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = dimReqCount, arrowHeadVisible = false, leaderLength =20 };
            DrawEntityModel dimEntityLeader01 = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(pTopLine.StartPoint, plateLength + 1 * scaleValue, -plateWidth), dimLeader01, scaleValue);
            drawList.AddDrawEntity(dimEntityLeader01);


            DrawBMLeaderModel dimLeader02 = new DrawBMLeaderModel()
            {
                position = POSITION_TYPE.RIGHT,
                upperText = dimLeaderStr01,
                lowerText = dimLeaderStr02,
                leaderPointLength = 30,
                leaderPointRadian = Utility.DegToRad(70),
                leaderLength = 20,
                bmNumber= dimLeaderNumber,

            };

            Point3D leaderInsertPoint = GetSumPoint(pTopLine.StartPoint, plateLength * 0.7, -bendingPlateThk / 2);
            DrawEntityModel dimEntityLeader02 = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(leaderInsertPoint,0,0), dimLeader02, scaleValue);
            drawList.AddDrawEntity(dimEntityLeader02);

            double sLineLength = 3 * scaleValue;
            List<Entity> newSLineList = breakService.GetSLine(GetSumPoint(leaderInsertPoint, 0,0), sLineLength);
            styleService.SetLayerListEntity(ref newSLineList, layerService.LayerDimension);
            drawList.outlineList.AddRange(newSLineList);


            // Center Point
            Point3D modelCenterPoint = GetSumPoint(pTopLine.MidPoint, 0, 0);
            SetModelCenterPoint(PAPERSUB_TYPE.TopRingCuttingPlan, modelCenterPoint);

            return drawList;

        }









        /// </summary>

        //// <summary>  0825 After Noon
        public DrawEntityModel DrawAnchorChair(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {
            // Anchor Chair
            DrawEntityModel drawList = new DrawEntityModel();

            //singleModel.Entities.Clear();
            List<Entity> customEntity = new List<Entity>();
            List<Entity> customLine = new List<Entity>();


            if (assemblyData.AnchorageInput[0].AnchorChairBlot.ToLower() == "yes")
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

                    double A2 = valueService.GetDoubleValue(newAnchor.A2);
                    double C1 = valueService.GetDoubleValue(newAnchor.C1);
                    double B1 = valueService.GetDoubleValue(newAnchor.B1);
                    double F1 = valueService.GetDoubleValue(newAnchor.F1);
                    double H1 = valueService.GetDoubleValue(newAnchor.H1);
                    double G1 = valueService.GetDoubleValue(newAnchor.G1);
                    double C = valueService.GetDoubleValue(newAnchor.C);



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


                    Point3D refCenterPoint = new Point3D(refPoint.X, refPoint.Y);

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



                    customEntity.AddRange(newList);



                    drawList.outlineList.AddRange(customEntity);


                    //--------------------

                    List<Entity> drawFrontList = new List<Entity>();

                    double anchorLegHeight = anchorHeight -T ;

                    List<Point3D> extPoint = new List<Point3D>();

                    




                    // Leg
                    double gapBetweenLegs = E / 2;


                    // selPoint : Bottom Center
                    double scaleDistance = B + 250;
                    Point3D selPoint = GetSumPoint(refCenterPoint, scaleDistance + gapBetweenLegs, 0);


                    drawFrontList.AddRange(shapeService.GetRectangle(out extPoint, GetSumPoint(selPoint, -gapBetweenLegs, 0), T1, anchorLegHeight, 0, 0, 2, new bool[] { false, true, true, true }));
                    drawFrontList.AddRange(shapeService.GetRectangle(out extPoint, GetSumPoint(selPoint, gapBetweenLegs, 0), T1, anchorLegHeight, 0, 0, 3, new bool[] { false, true, true, true }));

                    // Top
                    drawFrontList.AddRange(shapeService.GetRectangle(out extPoint, GetSumPoint(selPoint, -F / 2, anchorLegHeight), F, T, 0, 0, 3));

                    // Top Small
                    drawFrontList.AddRange(shapeService.GetRectangle(out extPoint, GetSumPoint(selPoint, -W / 2, anchorLegHeight + T), W, T2, 0, 0, 3, new bool[] { true, true, false, true }));


                    // Pad
                    double padWidth = F + A2 + A2;
                    double padHeightNew = padHeight-B1;
                    List<Entity> padList = shapeService.GetRectangle(out extPoint, GetSumPoint(selPoint, -padWidth / 2, C1), padWidth, padHeightNew, 0, 0, 3);
                    Curve.Fillet((ICurve)padList[3], (ICurve)padList[0], F1, false, false, true, true, out Arc padArcFillet1);
                    Curve.Fillet((ICurve)padList[0], (ICurve)padList[1], F1, false, false, true, true, out Arc padArcFillet2);
                    Curve.Fillet((ICurve)padList[1], (ICurve)padList[2], F1, false, false, true, true, out Arc padArcFillet3);
                    Curve.Fillet((ICurve)padList[2], (ICurve)padList[3], F1, false, false, true, true, out Arc padArcFillet4);
                    padList.AddRange(new Arc[] { padArcFillet1, padArcFillet2, padArcFillet3, padArcFillet4 });


                    // Center Line
                    DrawCenterLineModel newCenterModel = new DrawCenterLineModel();
                    List<Entity> centerList = editingService.GetCenterLine(GetSumPoint(selPoint, 0, anchorHeight + C1), GetSumPoint(selPoint, 0, 0), newCenterModel.centerLength, scaleValue);
                    styleService.SetLayerListEntity(ref centerList, layerService.LayerCenterLine);

                    drawFrontList.AddRange(padList);
                    styleService.SetLayerListEntity(ref drawFrontList, layerService.LayerOutLine);
                    drawFrontList.AddRange(centerList);

                    drawList.outlineList.AddRange(drawFrontList);



                    // Center Point
                    Point3D modelCenterPoint = GetSumPoint(refCenterPoint, B + 250/2, anchorLegHeight/2);
                    SetModelCenterPoint(PAPERSUB_TYPE.AnchorChair, modelCenterPoint);


                }
            }






            //-------------------------------








            return drawList;

        }
        public DrawEntityModel DrawSettlementCheckPiece(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {

            DrawEntityModel drawList = new DrawEntityModel();

            // Settlement Check Piece

            //singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Entity> lineList = new List<Entity>();
            List<Entity> virtualLineList = new List<Entity>();

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            // Start Point of TopView : RefPoint 에서의 Y축으로 떨어진거리
            double distanceRectangleX = 200;

            ///////////////////////
            ///  CAD Data Setting
            ///////////////////////
            double angleWidth = 65;
            double angleLength = 65;
            double angleThk = 8;
            double angleEdgeRoundR1 = 8.5;
            double angleEdgeRoundR2 = 6;
            double angleHeight = 200;

            double bottomPlateWidth = 9;
            double bottomPlateLength = 135;
            double VeticalPlateLength = 240 - bottomPlateWidth;
            double VerticalPlateDistanceL = 66;


            // bottom Plate
            Point3D bottomStartPoint = GetSumPoint(referencePoint, 0, 0);
            List<Line> bottomLineList = GetRectangle(bottomStartPoint, bottomPlateWidth, bottomPlateLength, RECTANGLUNVIEW.LEFT);
            Line bBottomLine = bottomLineList[0];
            Line bTopLine = bottomLineList[2];

            // vertical Plate
            Point3D verticalStartPoint = GetSumPoint(referencePoint, VerticalPlateDistanceL, bottomPlateWidth);
            List<Line> verticalLineList = GetRectangle(verticalStartPoint, VeticalPlateLength, bottomPlateWidth, RECTANGLUNVIEW.TOP);
            Line vRightLine = verticalLineList[1];
            Line vLeftLine = verticalLineList[2];
            List<Entity> verticalLineListEntity = new List<Entity>();
            verticalLineListEntity.Add(vRightLine);
            verticalLineListEntity.Add(vLeftLine);

            // angle
            Point3D angleStartPoint = GetSumPoint(referencePoint, VerticalPlateDistanceL + bottomPlateWidth, angleHeight);
            Line angleTopLine = new Line(angleStartPoint, GetSumPoint(angleStartPoint, angleLength, 0));
            Line angleTopRLine = new Line(angleTopLine.EndPoint, GetSumPoint(angleTopLine.EndPoint, 0, -angleThk));
            Line angleTopBLine = new Line(angleTopRLine.EndPoint, GetSumPoint(angleTopRLine.EndPoint, -(angleWidth - angleThk), 0));
            Line angleRightLine = new Line(angleTopBLine.EndPoint, GetSumPoint(angleTopBLine.EndPoint, 0, -(angleLength - angleThk)));
            Line angleBottomLine = new Line(angleRightLine.EndPoint, GetSumPoint(angleRightLine.EndPoint, -angleThk, 0));
            Line angleLeftLine = new Line(angleBottomLine.EndPoint, GetSumPoint(angleStartPoint, 0, 0));

            // angle Arc
            if (Curve.Fillet(angleTopRLine, angleTopBLine, angleEdgeRoundR2, false, false, true, true, out Arc arcFillet01))
                lineList.Add(arcFillet01);

            if (Curve.Fillet(angleTopBLine, angleRightLine, angleEdgeRoundR1, false, false, true, true, out Arc arcFillet02))
                lineList.Add(arcFillet02);

            if (Curve.Fillet(angleRightLine, angleBottomLine, angleEdgeRoundR2, false, false, true, true, out Arc arcFillet03))
                lineList.Add(arcFillet03);


            // Right Rectangle
            Point3D rectangleStartPoint = GetSumPoint(angleStartPoint, distanceRectangleX, -angleWidth);
            List<Line> rectangleLineList = GetRectangle(rectangleStartPoint, angleWidth, angleLength);
            Line rectangleTopLine = rectangleLineList[2];
            Line rectangleMidLine = (Line)rectangleTopLine.Offset(angleThk, Vector3D.AxisZ);



            lineList.AddRange(new Entity[] {
                                        angleTopLine, angleTopRLine, angleTopBLine,
                                        angleRightLine, angleBottomLine, angleLeftLine,
                                    });
            lineList.AddRange(rectangleLineList);
            lineList.Add(rectangleMidLine);

            virtualLineList.AddRange(bottomLineList);
            //virtualLineList.AddRange(verticalLineList);



            // Center Line : Red color
            Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            //styleService.SetLayerListEntity(ref lineList, layerService.LayerOutLine);
            //styleService.SetLayerListEntity(ref virtualLineList, layerService.LayerVirtualLine);


            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(lineList);
            //singleModel.Entities.AddRange(virtualLineList);

            //singleModel.Entities.Regen();
            //singleModel.Invalidate();

            //singleModel.SetView(viewType.Top);
            //singleModel.ZoomFit();
            //singleModel.Refresh();


            // Center Point
            Point3D modelCenterPoint = GetSumPoint(vRightLine.StartPoint, distanceRectangleX/2, -25);
            SetModelCenterPoint(PAPERSUB_TYPE.SettlementCheckPiece, modelCenterPoint);


            styleService.SetLayerListEntity(ref lineList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref virtualLineList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(lineList);
            drawList.outlineList.AddRange(virtualLineList);




            // S Line
            double sLineLength = bottomPlateWidth;

            List<Entity> newSDoubleLineList = breakService.GetSDoubleLine(GetSumPoint(vLeftLine.EndPoint, 0, 100), sLineLength, scaleValue);
            List<Entity> newListList = breakService.GetTrimSDoubleLine(verticalLineListEntity, newSDoubleLineList);
            styleService.SetLayerListEntity(ref newSDoubleLineList, layerService.LayerDimension);
            drawList.outlineList.AddRange(newSDoubleLineList);
            styleService.SetLayerListEntity(ref newListList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(newListList);

            List<Entity> newSLineList1 = breakService.GetSLine(GetSumPoint(vLeftLine.StartPoint, 0, 0), sLineLength,true, 0);
            List<Entity> newSLineList2 = breakService.GetSLine(GetSumPoint(bTopLine.StartPoint, 0, 0), sLineLength, false,90);
            styleService.SetLayerListEntity(ref newSLineList1, layerService.LayerDimension);
            styleService.SetLayerListEntity(ref newSLineList2, layerService.LayerDimension);
            drawList.outlineList.AddRange(newSLineList1);
            drawList.outlineList.AddRange(newSLineList2);


            DrawDimensionModel dimModel4 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = "65",
                dimHeight = 12,
                scaleValue = scaleValue
            };
            DrawEntityModel testDimTest04 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(rectangleTopLine.StartPoint,0, 0), GetSumPoint(rectangleTopLine.EndPoint, 0, 0), scaleValue, dimModel4);
            drawList.AddDrawEntity(testDimTest04);

            // Dimension : Right
            DrawDimensionModel dimModel5 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.RIGHT,
                textUpper = "300",
                dimHeight = 15,
                scaleValue = scaleValue
            };
            DrawEntityModel testDimTest05 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(bBottomLine.EndPoint, 0, 0), GetSumPoint(angleTopLine.EndPoint, 0, 0), scaleValue, dimModel5);
            drawList.AddDrawEntity(testDimTest05);



            // Welding
            List<Entity> weldingList = new List<Entity>();
            DrawWeldSymbols weldSymbols = new DrawWeldSymbols();
            DrawWeldSymbolModel wsModel02 = new DrawWeldSymbolModel();
            wsModel02.position = ORIENTATION_TYPE.TOPRIGHT;
            wsModel02.weldTypeUp = WeldSymbol_Type.Fillet;
            wsModel02.weldTypeDown = WeldSymbol_Type.Fillet;
            wsModel02.weldDetailType = WeldSymbolDetail_Type.BothSide;
            wsModel02.weldLength1 = "5";
            wsModel02.tailVisible = false;
            wsModel02.leaderAngle = 60;
            wsModel02.leaderLineLength = 17;
            weldingList.AddRange(weldSymbols.GetWeldSymbol(GetSumPoint(angleTopLine.StartPoint, 0, 0), singleModel, scaleValue, wsModel02));
            styleService.SetLayerListEntity(ref weldingList, layerService.LayerDimension);
            drawList.outlineList.AddRange(weldingList);

            // Dimension : Leader
            string leaderString01 = "L65x65xt8";
            string leaderBM01 = "11";
            DrawBMLeaderModel dimLeader01 = new DrawBMLeaderModel()
            {
                position = POSITION_TYPE.RIGHT,
                upperText = leaderString01,
                leaderPointLength = 12,
                leaderPointRadian = Utility.DegToRad(60),
                leaderLength = 15,
                bmNumber = leaderBM01,

            };
            DrawEntityModel dimEntityLeader01 = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(angleTopLine.EndPoint, -20, 0), dimLeader01, scaleValue);
            drawList.AddDrawEntity(dimEntityLeader01);

            return drawList;

        }
        /// </summary>






        public DrawEntityModel DrawShellPlateChordLength_SectionA(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {

            DrawEntityModel drawList = new DrawEntityModel();
            //Shell Plate Chord Length Section_A

            //singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Entity> arcLineList = new List<Entity>();
            List<Line> lineList = new List<Line>();

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);


            double distanceSize = 3;
            double distancehalfSize = distanceSize / 2;
            double guideRadius = 300;
            double guideRadiusOutLIne = guideRadius + distanceSize;
            double cutAngle = 7;

            // Guide Circle and Line
            Point3D arcMidTopPoint = GetSumPoint(referencePoint, 0, guideRadiusOutLIne);

            Circle guideInnerCircle = new Circle(referencePoint, guideRadius);
            Circle guideouterCircle = new Circle(referencePoint, guideRadiusOutLIne);

            Line guideLineLeft = new Line(referencePoint, GetSumPoint(referencePoint, 0, guideRadiusOutLIne + 10));
            Line guideLineRight = new Line(referencePoint, GetSumPoint(referencePoint, 0, guideRadiusOutLIne + 10));

            guideLineLeft.Rotate(Utility.DegToRad(cutAngle), Vector3D.AxisZ, referencePoint);
            guideLineRight.Rotate(Utility.DegToRad(-cutAngle), Vector3D.AxisZ, referencePoint);

            // Intersect : guideLine with (Circle in / out)
            Point3D[] intersectLeftInP = guideLineLeft.IntersectWith(guideInnerCircle);
            Point3D[] intersectLeftOutP = guideLineLeft.IntersectWith(guideouterCircle);
            Point3D[] intersectRightInP = guideLineRight.IntersectWith(guideInnerCircle);
            Point3D[] intersectRightOutP = guideLineRight.IntersectWith(guideouterCircle);

            // Get offsetLine
            Line offsetLineLeft = new Line(GetSumPoint(intersectLeftOutP[0], 0, 0), GetSumPoint(intersectLeftInP[0], 0, 0));
            Line offsetLineRight = new Line(GetSumPoint(intersectRightOutP[0], 0, 0), GetSumPoint(intersectRightInP[0], 0, 0));

            // Draw offset Lines
            Line leftouterVLine = (Line)offsetLineLeft.Offset(distancehalfSize, Vector3D.AxisZ);
            Line leftInnerVLine = (Line)offsetLineLeft.Offset(-distancehalfSize, Vector3D.AxisZ);
            Line rightInnerVLine = (Line)offsetLineRight.Offset(distancehalfSize, Vector3D.AxisZ);
            Line rightouterVLine = (Line)offsetLineRight.Offset(-distancehalfSize, Vector3D.AxisZ);

            // Draw : Center Arc
            Arc arcouter = new Arc(leftInnerVLine.StartPoint, arcMidTopPoint, rightInnerVLine.StartPoint, false);
            Arc arcInner = new Arc(leftInnerVLine.EndPoint, GetSumPoint(arcMidTopPoint, 0, -distanceSize), rightInnerVLine.EndPoint, false);

            

            // Guide Line 
            guideLineLeft.Rotate(Utility.DegToRad(cutAngle - 2), Vector3D.AxisZ, referencePoint);
            guideLineRight.Rotate(Utility.DegToRad(-(cutAngle - 2)), Vector3D.AxisZ, referencePoint);

            intersectLeftInP = guideLineLeft.IntersectWith(guideInnerCircle);
            intersectLeftOutP = guideLineLeft.IntersectWith(guideouterCircle);
            intersectRightInP = guideLineRight.IntersectWith(guideInnerCircle);
            intersectRightOutP = guideLineRight.IntersectWith(guideouterCircle);


            // Draw End Line : Left/Right End LIne
            Line leftEndLine = new Line(GetSumPoint(intersectLeftOutP[0], 0, 0), GetSumPoint(intersectLeftInP[0], 0, 0));
            Line rightEndLine = new Line(GetSumPoint(intersectRightOutP[0], 0, 0), GetSumPoint(intersectRightInP[0], 0, 0));

            Arc arcLeftOut = new Arc(referencePoint, leftouterVLine.StartPoint, leftEndLine.StartPoint);
            Arc arcLeftIn = new Arc(referencePoint, leftouterVLine.EndPoint, leftEndLine.EndPoint);
            Arc arcRightOut = new Arc(referencePoint, rightouterVLine.StartPoint, rightEndLine.StartPoint);
            Arc arcRightIn = new Arc(referencePoint, rightouterVLine.EndPoint, rightEndLine.EndPoint);

            arcLineList.AddRange(new Entity[] {
                        leftouterVLine, rightInnerVLine,
                        leftInnerVLine, rightouterVLine,
                        arcouter, arcInner,
                        leftEndLine, rightEndLine,
                        arcLeftOut, arcLeftIn, arcRightOut, arcRightIn
                        //guideInnerCircle, guideouterCircle,
            });

            lineList.AddRange(new Line[] {
                        //offsetLineLeft, offsetLineRight,
                        //guideLineLeft, guideLineRight,
                        
            });

            double shiftX = -(leftEndLine.EndPoint.X) + referencePoint.X;
            double shiftY = -(leftEndLine.EndPoint.Y - referencePoint.Y);
            Vector3D moveXY = new Vector3D(shiftX, shiftY);
            foreach (Entity eachEntity in arcLineList)
            { eachEntity.Translate(moveXY); }



            // Center Line : Red color
            Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            //styleService.SetLayerListEntity(ref arcLineList, layerService.LayerOutLine);
            //styleService.SetLayerListLine(ref lineList, layerService.LayerDimension);

            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(lineList);
            //singleModel.Entities.AddRange(arcLineList);

            //singleModel.Entities.Regen();
            //singleModel.Invalidate();

            //singleModel.SetView(viewType.Top);
            //singleModel.ZoomFit();
            //singleModel.Refresh();

            // Center Point
            Point3D modelCenterPoint = arcouter.MidPoint;
            SetModelCenterPoint(PAPERSUB_TYPE.ShellPlateChordLength, modelCenterPoint);



            styleService.SetLayerListEntity(ref arcLineList, layerService.LayerOutLine);
            styleService.SetLayerListLine(ref lineList, layerService.LayerDimension);
            drawList.outlineList.AddRange(lineList);
            drawList.outlineList.AddRange(arcLineList);






            // Dimension : Arc Line


            //Point3D insertInnerDimPoint = editingService.GetIntersectLength(innerBendingPlateDimLine, 1 * scaleValue, false);
            //Point3D insertOuterDimPoint = editingService.GetIntersectLength(outerBendingPlateDimLine, 1 * scaleValue, false);





            DrawDimensionModel dimModel1 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = "3.2",
                arrowLeftHeadOut=true,
                arrowRightHeadOut=true,
                dimHeight = 12,
                scaleValue = scaleValue
            };
            DrawEntityModel testDimTest01 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(arcLeftOut.StartPoint, 0, 0), GetSumPoint(arcLeftOut.StartPoint, 3, 0), scaleValue, dimModel1, Utility.DegToRad(cutAngle));
            drawList.AddDrawEntity(testDimTest01);
            DrawDimensionModel dimModel2 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.TOP,
                textUpper = "3.2",
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                dimHeight = 12,
                scaleValue = scaleValue
            };
            DrawEntityModel testDimTest02 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(arcouter.EndPoint, 0, 0), GetSumPoint(arcouter.EndPoint, 3, 0), scaleValue, dimModel2,Utility.DegToRad(-cutAngle));
            drawList.AddDrawEntity(testDimTest02);


            // Dimension : Arc
            //double dimleftArcDegree = -180 + eachPlateHalfDeg;
            //double dimrightArcDegree = 180 - eachPlateHalfDeg;
            //DrawEntityModel testDimArc01 = drawService.Draw_DimensionArc(GetSumPoint(innerBendingPlate.StartPoint, 0, 0), GetSumPoint(innerBendingPlate.EndPoint, 0, 0), "top", -refdimHeight, dimArcAngle, 45, dimleftArcDegree, dimrightArcDegree, 0, scaleValue, layerService.LayerDimension);
            //drawList.AddDrawEntity(testDimArc01);



            DrawDimensionModel dimModel31 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.BOTTOM,
                textUpper = "CH'D 9783.4" ,
                textLower="(INSIDE)",
                textSizeVisible = false,
                dimHeight = 15,
                leftPointRotate = false,
                rightPointRotate = true,
                scaleValue = scaleValue
            };
            DrawEntityModel testDimTest031 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(arcInner.StartPoint,-1.5 , 0), GetSumPoint(arcInner.EndPoint, 1.5, 0), scaleValue, dimModel31);
            drawList.AddDrawEntity(testDimTest031);








            return drawList;
        }

        public DrawEntityModel DrawVertJointDetail_Set(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue,double one, double two, double three, double four)
        {
            DrawEntityModel drawList = new DrawEntityModel();

            double drawCount = 0;
            if (one > 0)
                drawCount++;
            if (two > 0)
                drawCount++;
            if (three > 0)
                drawCount++;
            if (four > 0)
                drawCount++;


            Point3D centerPoint = new Point3D(refPoint.X, refPoint.Y);
            CDPoint point01 = new CDPoint();
            CDPoint point02 = new CDPoint();
            CDPoint point03 = new CDPoint();
            CDPoint point04 = new CDPoint();

            if (drawCount <= 2) 
            {
                point01 = new CDPoint(centerPoint.X - 60, centerPoint.Y - 25,0);
                point02 = new CDPoint(centerPoint.X +20, centerPoint.Y - 25, 0);
            }
            else
            {
                point01 = new CDPoint(centerPoint.X - 60, centerPoint.Y + 25, 0);
                point02 = new CDPoint(centerPoint.X +20, centerPoint.Y + 25, 0);
                point03 = new CDPoint(centerPoint.X - 60, centerPoint.Y - 25, 0);
                point04 = new CDPoint(centerPoint.X +20, centerPoint.Y - 25, 0);
            }

            if (one == 1)
                drawList.AddDrawEntity(DrawVertJointDetail_Single(ref point01, ref point01, selModel, scaleValue));
            else if (one == 2)
                drawList.AddDrawEntity(DrawVertJointDetail_Double(ref point01, ref point01, selModel, scaleValue));

            if (two == 1)
                drawList.AddDrawEntity(DrawVertJointDetail_Single(ref point02, ref point02, selModel, scaleValue));
            else if (two == 2)
                drawList.AddDrawEntity(DrawVertJointDetail_Double(ref point02, ref point02, selModel, scaleValue));

            if (three == 1)
                drawList.AddDrawEntity(DrawVertJointDetail_Single(ref point03, ref point03, selModel, scaleValue));
            else if (three == 2)
                drawList.AddDrawEntity(DrawVertJointDetail_Double(ref point03, ref point03, selModel, scaleValue));

            if (four == 1)
                drawList.AddDrawEntity(DrawVertJointDetail_Single(ref point04, ref point04, selModel, scaleValue));
            else if (four== 2)
                drawList.AddDrawEntity(DrawVertJointDetail_Double(ref point04, ref point04, selModel, scaleValue));


            // Center Point
            Point3D modelCenterPoint = GetSumPoint(centerPoint, 0, 0);
            SetModelCenterPoint(PAPERSUB_TYPE.VertJointDetail, modelCenterPoint);

            return drawList;
        }
        public DrawEntityModel DrawVertJointDetail_Single(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {
            DrawEntityModel drawList = new DrawEntityModel();
            //Vert. Joint Detail_CRT

            //singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Entity> arcLineList = new List<Entity>();

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            // Infomation
            double distanceBottom = 1.6;
            double bottomOneLength = 19.2;
            double totalLength = 40;
            double heightSize = 8;
            double distanceHeight = 1;
            double distanceTopArc = 1.5;
            double TopAngle = 60;
            double halfAngle = TopAngle / 2;


            // Draw : Bottom Lines
            Line bottomLeftLine = new Line(referencePoint, GetSumPoint(referencePoint, bottomOneLength, 0));
            // hidden? or show : bottomDistanceLine
            Line bottomDistanceLine = new Line(GetSumPoint(bottomLeftLine.EndPoint, 0, 0), GetSumPoint(bottomLeftLine.EndPoint, distanceBottom, 0));
            Line bottomRightLine = new Line(GetSumPoint(bottomDistanceLine.EndPoint, 0, 0), GetSumPoint(bottomDistanceLine.EndPoint, bottomOneLength, 0));

            // Draw : Up Lines - two Line
            Line bottomUpLineLeft = new Line(GetSumPoint(bottomDistanceLine.StartPoint, 0, 0), GetSumPoint(bottomDistanceLine.StartPoint, 0, distanceHeight));
            Line bottomUpLineRight = new Line(GetSumPoint(bottomDistanceLine.EndPoint, 0, 0), GetSumPoint(bottomDistanceLine.EndPoint, 0, distanceHeight));


            // Guide Line : ArcMidPoint / Top,Left,Right Line
            Point3D arcMidPoint = GetSumPoint(bottomDistanceLine.MidPoint, 0, heightSize + distanceTopArc);
            Line guideTopLine = new Line(GetSumPoint(bottomLeftLine.StartPoint, 0, heightSize), GetSumPoint(bottomLeftLine.StartPoint, totalLength, heightSize));
            Line guideLineLeft = new Line(GetSumPoint(bottomUpLineLeft.EndPoint, 0, 0), GetSumPoint(bottomUpLineLeft.EndPoint, 0, totalLength));
            Line guideLineRight = new Line(GetSumPoint(bottomUpLineRight.EndPoint, 0, 0), GetSumPoint(bottomUpLineRight.EndPoint, 0, totalLength));

            guideLineLeft.Rotate(Utility.DegToRad(halfAngle), Vector3D.AxisZ, bottomUpLineLeft.EndPoint);
            guideLineRight.Rotate(Utility.DegToRad(-halfAngle), Vector3D.AxisZ, bottomUpLineRight.EndPoint);

            // Intersect : guideLine with (Circle in / out)
            Point3D[] intersectLeftTop = guideLineLeft.IntersectWith(guideTopLine);
            Point3D[] intersectRightTop = guideLineRight.IntersectWith(guideTopLine);

            // Draw : Diagonal Line
            Line leftDiagonalLine = new Line(GetSumPoint(bottomUpLineLeft.EndPoint, 0, 0), GetSumPoint(intersectLeftTop[0], 0, 0));
            Line rightDiagonalLine = new Line(GetSumPoint(bottomUpLineRight.EndPoint, 0, 0), GetSumPoint(intersectRightTop[0], 0, 0));


            Arc arcTop = new Arc(intersectLeftTop[0], arcMidPoint, intersectRightTop[0], false);
            Line leftTopLine = new Line(GetSumPoint(guideTopLine.StartPoint, 0, 0), GetSumPoint(intersectLeftTop[0], 0, 0));
            Line rightTopLine = new Line(GetSumPoint(intersectRightTop[0], 0, 0), GetSumPoint(guideTopLine.EndPoint, 0, 0));

            arcLineList.AddRange(new Entity[] {
                        bottomLeftLine, bottomRightLine,
                        bottomUpLineLeft, bottomUpLineRight,
                        arcTop,
                        leftTopLine, rightTopLine,
                        leftDiagonalLine, rightDiagonalLine,

                        bottomDistanceLine // hidden? or show
            });


            // Center Line : Red color
            Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            //styleService.SetLayerListEntity(ref arcLineList, layerService.LayerOutLine);

            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(arcLineList);

            //singleModel.Entities.Regen();
            //singleModel.Invalidate();

            //singleModel.SetView(viewType.Top);
            //singleModel.ZoomFit();
            //singleModel.Refresh();

            styleService.SetLayerListEntity(ref arcLineList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(arcLineList);





            // Dimension : Break
            List<Entity> leftBreakList = breakService.GetFlatBreakLine(GetSumPoint(leftTopLine.StartPoint, 0, 0),
                                                            GetSumPoint(bottomLeftLine.StartPoint, 0, 0),
                                                            scaleValue);
            styleService.SetLayerListEntity(ref leftBreakList, layerService.LayerDimension);
            drawList.outlineList.AddRange(leftBreakList);

            List<Entity> rightBreakList = breakService.GetFlatBreakLine(GetSumPoint(rightTopLine.EndPoint, 0, 0),
                                                GetSumPoint(bottomRightLine.EndPoint, 0, 0),
                                                scaleValue);
            styleService.SetLayerListEntity(ref rightBreakList, layerService.LayerDimension);
            drawList.outlineList.AddRange(rightBreakList);

            // Dimension : Arc
            DrawEntityModel testDimArc01 = drawService.Draw_DimensionArc(GetSumPoint(leftTopLine.EndPoint, 0, 0), GetSumPoint(rightTopLine.StartPoint, 0, 0), "top", 12, "60˚", 45, 30, -30, 0, scaleValue, layerService.LayerDimension);
            drawList.AddDrawEntity(testDimArc01);

            // Dimension : top
            DrawDimensionModel dimModel01 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.BOTTOM,
                textUpper = "3.2",
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                textUpperPosition = POSITION_TYPE.LEFT,
                textSizeVisible = false,
                dimHeight = 12,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity01 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(bottomLeftLine.EndPoint, 0, 0), GetSumPoint(bottomRightLine.StartPoint, 0, 0), scaleValue, dimModel01);
            drawList.AddDrawEntity(dimEntity01);
            DrawDimensionModel dimModel02 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.RIGHT,
                textUpper = "1.6",
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                extLineLeftVisible = false,
                textUpperPosition = POSITION_TYPE.LEFT,
                textSizeVisible = false,
                dimHeight = 15,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity02 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(bottomRightLine.StartPoint, 0, 0), GetSumPoint(rightDiagonalLine.StartPoint, 0, 0), scaleValue, dimModel02);
            drawList.AddDrawEntity(dimEntity02);


            AngleSizeModel eachAngle = new AngleSizeModel();
            if (assemblyData.RoofAngleOutput.Count > 0)
                eachAngle = assemblyData.RoofAngleOutput[0];
            DrawDimensionModel dimModel03 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.LEFT,
                textUpper = "t" + eachAngle.t,
                extLineLeftVisible = false,
                extLineRightVisible = false,
                textSizeVisible = false,
                dimHeight = 6,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity03 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(bottomLeftLine.EndPoint, 0, 0), GetSumPoint(leftTopLine.EndPoint, 0, 0), scaleValue, dimModel03);
            drawList.AddDrawEntity(dimEntity03);

            // Dimension : Welding
            DrawWeldSymbols weldSymbols = new DrawWeldSymbols();
            List<Entity> weldList = new List<Entity>();
            DrawWeldSymbolModel wsModel01 = new DrawWeldSymbolModel();
            wsModel01.position = ORIENTATION_TYPE.TOPRIGHT;
            wsModel01.weldTypeUp = WeldSymbol_Type.Square;
            wsModel01.weldTypeDown = WeldSymbol_Type.V;
            wsModel01.weldDetailType = WeldSymbolDetail_Type.BothSide;
            wsModel01.weldFaceDown = WeldFace_Type.Convex;
            wsModel01.tailVisible = true;
            wsModel01.specification = "B.G";
            wsModel01.leaderAngle = 60;
            wsModel01.leaderLineLength = 18;
            weldList.AddRange(weldSymbols.GetWeldSymbol(GetSumPoint(arcTop.MidPoint, 0, 0), singleModel, scaleValue, wsModel01));


            styleService.SetLayerListEntity(ref weldList, layerService.LayerDimension);
            drawList.outlineList.AddRange(weldList);


            DrawBMLeaderModel leaderInfoModel = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = "OUTSIDE", bmNumber = "", textAlign = POSITION_TYPE.CENTER, arrowHeadVisible = false };
            DrawEntityModel leaderInfoList = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(rightTopLine.EndPoint, 1, 0), leaderInfoModel, scaleValue);
            drawList.AddDrawEntity(leaderInfoList);

            DrawBMLeaderModel leaderInfoModel2 = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = "INSIDE", bmNumber = "", textAlign = POSITION_TYPE.CENTER, arrowHeadVisible = false };
            DrawEntityModel leaderInfoList2 = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(bottomRightLine.EndPoint, 1, 0), leaderInfoModel2, scaleValue);
            drawList.AddDrawEntity(leaderInfoList2);




            return drawList;

        }
        public DrawEntityModel DrawVertJointDetail_Double(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {
            DrawEntityModel drawList = new DrawEntityModel();
            //Vert. Joint Detail_DRT

            //singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Entity> arcLineList = new List<Entity>();



            // Infomation
            double distanceBottom = 1.6;
            double leftBottomLength = 15.2;
            double rightBottomLength = 19.2;
            double totalLength = 36;
            double heightSize = 8;
            double topHeight = 4.5;
            double distanceHeight = 1;
            double bottomHeight = 2.5;
            double distanceTopArc = 1.5;
            double distanceBottomArc = 1; 
            double TopAngle = 60;
            double halfAngle = TopAngle / 2;

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y- distanceBottomArc);

            // guide : Mid Lines
            Line guideMidLeftLine = new Line(GetSumPoint(referencePoint, 0, bottomHeight + distanceBottomArc), GetSumPoint(referencePoint, leftBottomLength, bottomHeight + distanceBottomArc));
            // hidden? or show : bottomDistanceLine
            Line guideMidDistanceLine = new Line(GetSumPoint(guideMidLeftLine.EndPoint, 0, 0), GetSumPoint(guideMidLeftLine.EndPoint, distanceBottom, 0));

            // Draw : Up Lines - two Line
            Line midUpLineLeft = new Line(GetSumPoint(guideMidDistanceLine.StartPoint, 0, 0), GetSumPoint(guideMidDistanceLine.StartPoint, 0, distanceHeight));
            Line midUpLineRight = new Line(GetSumPoint(guideMidDistanceLine.EndPoint, 0, 0), GetSumPoint(guideMidDistanceLine.EndPoint, 0, distanceHeight));


            // Guide Line : ArcMidPoint / Top,Left,Right Line
            Point3D arcMidPoint = GetSumPoint(guideMidDistanceLine.MidPoint, 0, topHeight + distanceTopArc + distanceHeight);
            Line guideTopLine = new Line(GetSumPoint(guideMidLeftLine.StartPoint, 0, topHeight + distanceHeight), GetSumPoint(guideMidLeftLine.StartPoint, totalLength, topHeight + distanceHeight));
            Line guideLineLeft = new Line(GetSumPoint(midUpLineLeft.EndPoint, 0, 0), GetSumPoint(midUpLineLeft.EndPoint, 0, totalLength));
            Line guideLineRight = new Line(GetSumPoint(midUpLineRight.EndPoint, 0, 0), GetSumPoint(midUpLineRight.EndPoint, 0, totalLength));

            guideLineLeft.Rotate(Utility.DegToRad(halfAngle), Vector3D.AxisZ, midUpLineLeft.EndPoint);
            guideLineRight.Rotate(Utility.DegToRad(-halfAngle), Vector3D.AxisZ, midUpLineRight.EndPoint);

            // Intersect : guideLine with (Top Line)
            Point3D[] intersectLeftTop = guideLineLeft.IntersectWith(guideTopLine);
            Point3D[] intersectRightTop = guideLineRight.IntersectWith(guideTopLine);

            // Draw : Diagonal Line (Top)
            Line leftDiagonalLine = new Line(GetSumPoint(midUpLineLeft.EndPoint, 0, 0), GetSumPoint(intersectLeftTop[0], 0, 0));
            Line rightDiagonalLine = new Line(GetSumPoint(midUpLineRight.EndPoint, 0, 0), GetSumPoint(intersectRightTop[0], 0, 0));

            // Draw : Top Lines/ Top Arc
            Arc arcTop = new Arc(intersectLeftTop[0], arcMidPoint, intersectRightTop[0], false);
            Line leftTopLine = new Line(GetSumPoint(guideTopLine.StartPoint, 0, 0), GetSumPoint(intersectLeftTop[0], 0, 0));
            Line rightTopLine = new Line(GetSumPoint(intersectRightTop[0], 0, 0), GetSumPoint(guideTopLine.EndPoint, 0, 0));

            // guide : Bottom Lines
            Line guideBottomLine = new Line(GetSumPoint(referencePoint, 0, distanceBottomArc), GetSumPoint(referencePoint, totalLength, distanceBottomArc));
            Point3D ArcMidPointBottom = GetSumPoint(guideMidDistanceLine.MidPoint, 0, -(bottomHeight + distanceBottomArc));

            // guide line
            guideLineLeft.Rotate(Utility.DegToRad(TopAngle * 2), Vector3D.AxisZ, midUpLineLeft.EndPoint);
            guideLineRight.Rotate(Utility.DegToRad(-TopAngle * 2), Vector3D.AxisZ, midUpLineRight.EndPoint);
            guideLineLeft.Translate(0, -distanceHeight);
            guideLineRight.Translate(0, -distanceHeight);

            // Intersect : guideLine with (bottom Line)
            Point3D[] intersectLeftBottom = guideLineLeft.IntersectWith(guideBottomLine);
            Point3D[] intersectRightBottom = guideLineRight.IntersectWith(guideBottomLine);

            // // Draw : Diagonal Line (Top)
            Line leftDiagonalBottomLine = new Line(GetSumPoint(midUpLineLeft.StartPoint, 0, 0), GetSumPoint(intersectLeftBottom[0], 0, 0));
            Line rightDiagonalBottomLine = new Line(GetSumPoint(midUpLineRight.StartPoint, 0, 0), GetSumPoint(intersectRightBottom[0], 0, 0));

            // Draw : Bottom Lines / bottom Arc
            Arc arcBottom = new Arc(intersectLeftBottom[0], ArcMidPointBottom, intersectRightBottom[0], false);
            Line bottomLeftLine = new Line(guideBottomLine.StartPoint, intersectLeftBottom[0]);
            Line bottomRightLine = new Line(intersectRightBottom[0], guideBottomLine.EndPoint);


            arcLineList.AddRange(new Entity[] {
                        bottomLeftLine, bottomRightLine,
                        midUpLineLeft, midUpLineRight,
                        arcTop, arcBottom,
                        leftTopLine, rightTopLine,
                        leftDiagonalLine, rightDiagonalLine,
                        leftDiagonalBottomLine, rightDiagonalBottomLine,
                        //bottomDistanceLine // hidden? or show
            });


            // Center Line : Red color
            Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            centerLineList.Add(centerline);

            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            //styleService.SetLayerListEntity(ref arcLineList, layerService.LayerOutLine);

            //singleModel.Entities.AddRange(centerLineList);
            //singleModel.Entities.AddRange(arcLineList);

            //singleModel.Entities.Regen();
            //singleModel.Invalidate();

            //singleModel.SetView(viewType.Top);
            //singleModel.ZoomFit();
            //singleModel.Refresh();


            styleService.SetLayerListEntity(ref arcLineList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(arcLineList);




            // Dimension : Break
            List<Entity> leftBreakList = breakService.GetFlatBreakLine(GetSumPoint(leftTopLine.StartPoint, 0, 0),
                                                            GetSumPoint(bottomLeftLine.StartPoint, 0, 0),
                                                            scaleValue);
            styleService.SetLayerListEntity(ref leftBreakList, layerService.LayerDimension);
            drawList.outlineList.AddRange(leftBreakList);

            List<Entity> rightBreakList = breakService.GetFlatBreakLine(GetSumPoint(rightTopLine.EndPoint, 0, 0),
                                                GetSumPoint(bottomRightLine.EndPoint, 0, 0),
                                                scaleValue);
            styleService.SetLayerListEntity(ref rightBreakList, layerService.LayerDimension);
            drawList.outlineList.AddRange(rightBreakList);

            // Dimension : Arc
            DrawEntityModel testDimArc01 = drawService.Draw_DimensionArc(GetSumPoint(leftTopLine.EndPoint, 0, 0), GetSumPoint(rightTopLine.StartPoint, 0, 0), "top", 12, "60˚", 45, 30, -30, 0, scaleValue, layerService.LayerDimension);
            drawList.AddDrawEntity(testDimArc01);
            DrawEntityModel testDimArc02 = drawService.Draw_DimensionArc(GetSumPoint(bottomLeftLine.EndPoint, 0, 0), GetSumPoint(bottomRightLine.StartPoint, 0, 0), "top", 12, "60˚", 45, 150, 210, 0, scaleValue, layerService.LayerDimension);
            drawList.AddDrawEntity(testDimArc02);

            // Dimension : top
            DrawDimensionModel dimModel01 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.BOTTOM,
                textUpper = "3.2",
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                textUpperPosition = POSITION_TYPE.LEFT,
                textSizeVisible = false,
                dimHeight = 9,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity01 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(midUpLineLeft.StartPoint, 0, 0), GetSumPoint(midUpLineRight.StartPoint, 0, 0), scaleValue, dimModel01);
            drawList.AddDrawEntity(dimEntity01);
            DrawDimensionModel dimModel02 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.RIGHT,
                textUpper = "1.6",
                arrowLeftHeadOut = true,
                arrowRightHeadOut = true,
                textUpperPosition = POSITION_TYPE.LEFT,
                textSizeVisible = false,
                dimHeight = 15,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity02 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(midUpLineRight.StartPoint, 0, 0), GetSumPoint(midUpLineRight.EndPoint, 0, 0), scaleValue, dimModel02);
            drawList.AddDrawEntity(dimEntity02);


            AngleSizeModel eachAngle = new AngleSizeModel();
            if (assemblyData.RoofAngleOutput.Count > 0)
                eachAngle = assemblyData.RoofAngleOutput[0];
            DrawDimensionModel dimModel03 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.LEFT,
                textUpper = "t" + eachAngle.t,

                textSizeVisible = false,
                dimHeight = 12,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntity03 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(bottomLeftLine.StartPoint, 0, 0), GetSumPoint(leftTopLine.StartPoint, 0, 0), scaleValue, dimModel03);
            drawList.AddDrawEntity(dimEntity03);

            // Dimension : Welding
            DrawWeldSymbols weldSymbols = new DrawWeldSymbols();
            List<Entity> weldList = new List<Entity>();
            DrawWeldSymbolModel wsModel01 = new DrawWeldSymbolModel();
            wsModel01.position = ORIENTATION_TYPE.TOPRIGHT;
            wsModel01.weldTypeUp = WeldSymbol_Type.V;
            wsModel01.weldTypeDown = WeldSymbol_Type.V;
            wsModel01.weldDetailType = WeldSymbolDetail_Type.BothSide;
            wsModel01.weldFaceUp = WeldFace_Type.Flat;
            wsModel01.weldFaceDown = WeldFace_Type.Convex;
            wsModel01.machiningStr = "G";
            wsModel01.machiningVisible = true;
            wsModel01.tailVisible = true;
            wsModel01.specification = "B.G";
            wsModel01.leaderAngle = 60;
            wsModel01.leaderLineLength = 18;
            weldList.AddRange(weldSymbols.GetWeldSymbol(GetSumPoint(arcTop.MidPoint, 0, 0), singleModel, scaleValue, wsModel01));


            styleService.SetLayerListEntity(ref weldList, layerService.LayerDimension);
            drawList.outlineList.AddRange(weldList);



            DrawBMLeaderModel leaderInfoModel = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = "OUTSIDE", bmNumber = "", textAlign = POSITION_TYPE.CENTER, arrowHeadVisible = false };
            DrawEntityModel leaderInfoList = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(rightTopLine.EndPoint, 1, 0), leaderInfoModel, scaleValue);
            drawList.AddDrawEntity(leaderInfoList);

            DrawBMLeaderModel leaderInfoModel2 = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = "INSIDE", bmNumber = "", textAlign = POSITION_TYPE.CENTER, arrowHeadVisible = false };
            DrawEntityModel leaderInfoList2 = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(bottomRightLine.EndPoint, 1, 0), leaderInfoModel2, scaleValue);
            drawList.AddDrawEntity(leaderInfoList2);



            return drawList;

        }




        public DrawEntityModel DrawToleranceLimit(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {
            DrawEntityModel drawList = new DrawEntityModel();
            //Vert. Joint Detail_CRT

            //singleModel.Entities.Clear();

            List<Entity> newList = new List<Entity>();


            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            double tWidth = 80;
            double tHeight = 30;

            Line line01 = new Line(GetSumPoint(referencePoint,0,0), GetSumPoint(referencePoint, 0, tHeight));
            Line line02 = new Line(GetSumPoint(referencePoint, 0, tHeight), GetSumPoint(referencePoint, tWidth, tHeight));
            Line line03 = new Line(GetSumPoint(referencePoint, tWidth, tHeight), GetSumPoint(referencePoint, tWidth, 0));
            Line line04 = new Line(GetSumPoint(referencePoint, 0, 0), GetSumPoint(referencePoint, tWidth, 0));

            newList.Add(line01);
            newList.Add(line02);
            newList.Add(line03);
            newList.Add(line04);

            string horizontalString = "\"L\"±2.0";
            string verticalString = "\"W\"±1.0";
            string crossString = "DIAGONAL TOLERANCE \"X\"±3.0";
            double dimHeight = 15;
            DrawDimensionModel dimBottomMainModel = new DrawDimensionModel()
            {
                position = POSITION_TYPE.BOTTOM,
                textUpper = horizontalString,
                dimHeight = dimHeight,
                scaleValue = scaleValue
            };
            DrawEntityModel dimBottomMain = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(line04.StartPoint,0,0), GetSumPoint(line04.EndPoint,0,0), scaleValue, dimBottomMainModel);
            drawList.AddDrawEntity(dimBottomMain);

            DrawDimensionModel dimRightModel = new DrawDimensionModel()
            {
                position = POSITION_TYPE.RIGHT,
                textUpper = verticalString,
                dimHeight = dimHeight,
                scaleValue = scaleValue
            };
            DrawEntityModel dimRight = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(line03.EndPoint, 0, 0), GetSumPoint(line03.StartPoint, 0, 0), scaleValue, dimRightModel);
            drawList.AddDrawEntity(dimRight);


            // Rotate Angle
            Line angleLIne = new Line(GetSumPoint(line01.StartPoint, 0, 0), GetSumPoint(line02.EndPoint, 0, 0));
            double rotateAngle = editingService.GetAngleOfLine(angleLIne);


            DrawDimensionModel dimCrossModel = new DrawDimensionModel()
            {
                position = POSITION_TYPE.BOTTOM,
                textUpper = crossString,
                dimHeight = 0,
                scaleValue = scaleValue,
            };
            DrawEntityModel dimCross = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(line04.StartPoint, 0, 0), GetSumPoint(line04.StartPoint, angleLIne.Length(), 0), scaleValue, dimCrossModel, rotateAngle);
            drawList.AddDrawEntity(dimCross);


            // Center Point
            Point3D modelCenterPoint =GetSumPoint(referencePoint,tWidth/2,tHeight/2) ;
            SetModelCenterPoint(PAPERSUB_TYPE.ToleranceLimit, modelCenterPoint);


            styleService.SetLayerListEntity(ref newList, layerService.LayerHiddenLine);
            drawList.outlineList.AddRange(newList);

            return drawList;

        }




        // Roof/Bottom Plate Arrangement


        // Entry Point

        // Roof/Bottom Plate Arrangement




        // Entry Point
        public DrawEntityModel DrawBottomCuttingPlan(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {
            DrawEntityModel drawList = new DrawEntityModel();
            PlateArrange_Type arrangeType = PlateArrange_Type.Bottom;
            drawList.AddDrawEntity(DrawBottomRoofArrangement(arrangeType, ref refPoint, ref curPoint, selModel, scaleValue));

            return drawList;
        }
        public DrawEntityModel DrawRoofCuttingPlan(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {
            DrawEntityModel drawList = new DrawEntityModel();
            PlateArrange_Type arrangeType = PlateArrange_Type.Roof;
            drawList.AddDrawEntity(DrawBottomRoofArrangement(arrangeType, ref refPoint, ref curPoint, selModel, scaleValue));

            return drawList;
        }

        private List<tempPlateInfo> GetPlateToTempPlateInfo(List<DrawPlateModel> selModel,double plateWidth,double plateLength)
        {

            


            List<tempPlateInfo> newList = new List<tempPlateInfo>();
            foreach(DrawPlateModel eachModel in selModel)
            {
                double v1 = 0;
                double v2 = 0;
                double h1 = 0;
                double h2 = 0;
                if (eachModel.VLength.Count > 0)
                {
                    if (eachModel.VLength.Count == 1)
                    {
                        v1 = 0;
                        v2 = eachModel.VLength[0];
                    }
                    else
                    {
                        v1 = eachModel.VLength[0];
                        v2 = eachModel.VLength[1];
                    }
                }

                if (eachModel.HLength.Count > 0)
                {
                    if (eachModel.HLength.Count == 1)
                    {
                        h1 = 0;
                        h2 = eachModel.HLength[0];
                    }
                    else
                    {
                        h1 = eachModel.HLength[0];
                        h2 = eachModel.HLength[1];
                    }
                }


                // Position
                //public enum PLATEPLACE { TR, TL, BR, BL, HCENTER, VCENTER, LCENTER, RCENTER, TCENTER, BCENTER }
                PLATEPLACE newPlatePlate = PLATEPLACE.TR;
                switch (eachModel.ArcDirection)
                {
                    case ORIENTATION_TYPE.TOPLEFT:
                        newPlatePlate = PLATEPLACE.TL;
                        break;
                    case ORIENTATION_TYPE.TOPRIGHT:
                        newPlatePlate = PLATEPLACE.TR;
                        break;
                    case ORIENTATION_TYPE.TOPCENTER:
                        newPlatePlate = PLATEPLACE.TCENTER;
                        break;
                    case ORIENTATION_TYPE.BOTTOMLEFT:
                        newPlatePlate = PLATEPLACE.BL;
                        break;
                    case ORIENTATION_TYPE.BOTTOMRIGHT:
                        newPlatePlate = PLATEPLACE.BR;
                        break;
                    case ORIENTATION_TYPE.BOTTOMCENTER:
                        newPlatePlate = PLATEPLACE.BCENTER;
                        break;
                    case ORIENTATION_TYPE.LEFTCENTER:
                        newPlatePlate = PLATEPLACE.LCENTER;
                        break;
                    case ORIENTATION_TYPE.RIGHTCENTER:
                        newPlatePlate = PLATEPLACE.RCENTER;
                        break;
                }

                // Horizontal, Vertical
                bool isHorizontal = true;
                if(eachModel.PlateDirection==PERPENDICULAR_TYPE.Vertical)
                    isHorizontal = false;

                double selRadius = eachModel.Radius;

                tempPlateInfo newModel = new tempPlateInfo(
                    eachModel.DisplayName,
                    eachModel.Number,v1,v2,h1,h2, newPlatePlate,isHorizontal, selRadius);
                newModel.SetPlateShape(plateWidth, plateLength, ref newModel);
                newList.Add(newModel);

                Console.WriteLine("Radius : " + selRadius);
            }

            return newList;
        }

        private DrawEntityModel DrawBottomRoofArrangement(PlateArrange_Type arrangeType,ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {

            DrawEntityModel drawList = new DrawEntityModel();


            
            /**/
            List<tempPlateInfo> orgPlateInfoList = new List<tempPlateInfo>();
            /**/

            // cutting area List
            List<DrawCuttingAreaModel> cuttingAreaList = new List<DrawCuttingAreaModel>();


            // Plate Info
            double plateActualWidth = 0;
            double plateActualLength = 0;

            if (arrangeType == PlateArrange_Type.Roof)
            {
                // 매우 중요 함
                //plateWidth = 2408;
                //plateLength = 9114;
                plateActualWidth = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].PlateWidth);
                plateActualLength = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].PlateLength);
                SetPlateAddLength(SingletonData.RoofPlateInfo);
                orgPlateInfoList.AddRange(GetPlateToTempPlateInfo(SingletonData.RoofPlateInfo, plateActualWidth, plateActualLength));


            }
            else if (arrangeType == PlateArrange_Type.Bottom)
            {
                //plateWidth = 2408;
                //plateLength = 9114;

                plateActualWidth = valueService.GetDoubleValue(assemblyData.BottomInput[0].PlateWidth);
                plateActualLength = valueService.GetDoubleValue(assemblyData.BottomInput[0].PlateLength);

                SetPlateAddLength(SingletonData.BottomPlateInfo);
                orgPlateInfoList.AddRange(GetPlateToTempPlateInfo(SingletonData.BottomPlateInfo, plateActualWidth, plateActualLength));

            }

            // Scale : 매우 중요 함
            double realScaleValue = scaleService.GetScaleCalValue(135, 50, plateActualLength, plateActualWidth);


            // 전체 오름차순 정렬(Plate Number 기준)
            // 같은 번호의 Plate 수량을 구하기 쉽게 정렬
            List<tempPlateInfo> sortPlateInfoList = orgPlateInfoList.OrderBy(a => a.number).ToList();

            drawList.AddDrawEntity(SortPieceOfPlate(ref refPoint, ref curPoint, selModel, realScaleValue, sortPlateInfoList, plateActualWidth, plateActualLength, out cuttingAreaList));

            //Check_RectangleArrange(endPlateInfoList);

            SingletonData.RoofCuttingList.Clear();
            SingletonData.BottomCuttingList.Clear();

            if (arrangeType == PlateArrange_Type.Roof)
            {
                SingletonData.RoofCuttingList.AddRange(cuttingAreaList);
            }
            else if (arrangeType == PlateArrange_Type.Bottom)
            {
                SingletonData.BottomCuttingList.AddRange(cuttingAreaList);
            }
            return drawList;
        }

        public void SetPlateAddLength(List<DrawPlateModel> selList)
        {
            double fullLengthFactor = 30;
            double hallfLengthFactor = fullLengthFactor/2;
            foreach (DrawPlateModel eachPlate in selList)
            {
                if (eachPlate.VLength.Count == 1 && eachPlate.HLength.Count == 1)
                {
                    for (int i = 0; i < eachPlate.VLength.Count; i++)
                        eachPlate.VLength[i] += hallfLengthFactor;
                    for (int i = 0; i < eachPlate.HLength.Count; i++)
                        eachPlate.HLength[i] += hallfLengthFactor;

                }
                else if (eachPlate.VLength.Count == 2 && eachPlate.HLength.Count == 1)
                {
                    for (int i = 0; i < eachPlate.VLength.Count; i++)
                        eachPlate.VLength[i] += hallfLengthFactor;
                    for (int i = 0; i < eachPlate.HLength.Count; i++)
                        eachPlate.HLength[i] += fullLengthFactor;
                }
                else if (eachPlate.VLength.Count == 1 && eachPlate.HLength.Count == 2)
                {
                    for (int i = 0; i < eachPlate.VLength.Count; i++)
                        eachPlate.VLength[i] += fullLengthFactor;
                    for (int i = 0; i < eachPlate.HLength.Count; i++)
                        eachPlate.HLength[i] += hallfLengthFactor;
                }
                else if (eachPlate.VLength.Count == 2 && eachPlate.HLength.Count == 2)
                {
                    for (int i = 0; i < eachPlate.VLength.Count; i++)
                        eachPlate.VLength[i] += fullLengthFactor;
                    for (int i = 0; i < eachPlate.HLength.Count; i++)
                        eachPlate.HLength[i] += fullLengthFactor;
                }

            }
        }

        public List<SamePlateInfo> GetSameShapeList(List<tempPlateInfo> sortedPlateInfo, PLATESHAPE plateShape)
        {
            List<SamePlateInfo> samePlateInfoList = new List<SamePlateInfo>();

            // todo Refactoring : One if

            if (plateShape == PLATESHAPE.WHOLEPLATE)
            {
                foreach (tempPlateInfo eachTempPlateInfo in sortedPlateInfo)
                {
                    if (eachTempPlateInfo.plateShape == plateShape)
                    {
                        SamePlateInfo tempSamePlate = new SamePlateInfo();
                        tempSamePlate.SamePlateInfoInit();

                        tempSamePlate.plateNumber = eachTempPlateInfo.number; // get Plate Number
                        tempSamePlate.countPlate = sortedPlateInfo.FindAll(x => x.plateShape == plateShape).Count; // total wholePlate
                        tempSamePlate.tempPlateInfo = (tempPlateInfo)eachTempPlateInfo.Clone(); // clone

                        samePlateInfoList.Add(tempSamePlate);
                    }
                }
            }
            else
            {
                double[] tempNumber = { 0, 0 };

                foreach (tempPlateInfo eachTempPlateInfo in sortedPlateInfo)
                {
                    if (eachTempPlateInfo.plateShape == plateShape)
                    {
                        tempNumber[1] = eachTempPlateInfo.number; // index 1 : New PlateInfo

                        if (tempNumber[0] != tempNumber[1])        // index 0 : before PlateInfo
                        {
                            SamePlateInfo tempSamePlateInfo = new SamePlateInfo();
                            tempSamePlateInfo.SamePlateInfoInit();

                            tempSamePlateInfo.plateNumber = eachTempPlateInfo.number;  // get Plate Number
                            tempSamePlateInfo.countPlate = sortedPlateInfo.FindAll(x => x.number == eachTempPlateInfo.number).Count; // total SamePlate
                            tempSamePlateInfo.bottomLength = eachTempPlateInfo.bottomLength;
                            tempSamePlateInfo.tempPlateInfo = (tempPlateInfo)eachTempPlateInfo.Clone(); // clone

                            samePlateInfoList.Add(tempSamePlateInfo);

                            tempNumber[0] = tempNumber[1];   // Save index 0 : Current PlateInfo
                        }
                    }
                }

            }


            return samePlateInfoList;
        }





        public DrawEntityModel SortPieceOfPlate(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue, List<tempPlateInfo> sortPlateInfoList, double plateWidth, double plateLength, out List<DrawCuttingAreaModel> cuttingAreaList)
        {
            DrawEntityModel drawList = new DrawEntityModel();


            cuttingAreaList = new List<DrawCuttingAreaModel>();

            // Plate Width, Length  입력 받을 것

            double minDistance = 15;  // 각 Plate간의 최소 근접거리

            List<SamePlateInfo> samePlateListWhole = new List<SamePlateInfo>();

            List<SamePlateInfo> samePlateListRectangle = new List<SamePlateInfo>();
            List<SamePlateInfo> samePlateListRectArc = new List<SamePlateInfo>();
            List<SamePlateInfo> samePlateListArc = new List<SamePlateInfo>();
            List<SamePlateInfo> samePlateListHalfCircle = new List<SamePlateInfo>();
            List<SamePlateInfo> samePlateListTwinArc = new List<SamePlateInfo>();


            // 같은 모양끼리 모은후, 아랫변 기준으로 내림차순 정렬
            samePlateListWhole = GetSameShapeList(sortPlateInfoList, PLATESHAPE.WHOLEPLATE);

            List<SamePlateInfo> samePlateListRectangleOrigin = GetSameShapeList(sortPlateInfoList, PLATESHAPE.RECTANGLE);
            if (samePlateListRectangleOrigin.Count != 0)
            { samePlateListRectangle = samePlateListRectangleOrigin.OrderByDescending(x => x.bottomLength).ToList(); }

            //samePlateListRectangle = GetSameShapeList(sortPlateInfoList, PLATESHAPE.RECTANGLE);
            //if (samePlateListRectangle.Count != 0)
            //{
            //    samePlateListRectangle.Sort(delegate (SamePlateInfo A, SamePlateInfo B) {
            //        if (A.bottomLength < B.bottomLength) return -1;
            //        else return 1;
            //    });
            //}

            List<SamePlateInfo> samePlateListRectArcOrigin = GetSameShapeList(sortPlateInfoList, PLATESHAPE.RECTANGLEARC);
            if (samePlateListRectArcOrigin.Count != 0)
                samePlateListRectArc = samePlateListRectArcOrigin.OrderByDescending(x => x.bottomLength).ToList();

            List<SamePlateInfo> samePlateListArcOrigin = GetSameShapeList(sortPlateInfoList, PLATESHAPE.ARC);
            if (samePlateListArcOrigin.Count != 0)
                samePlateListArc = samePlateListArcOrigin.OrderByDescending(x => x.bottomLength).ToList();

            List<SamePlateInfo> samePlateListHalfCircleOrigin = GetSameShapeList(sortPlateInfoList, PLATESHAPE.HALFCIRCLE);
            if (samePlateListHalfCircleOrigin.Count != 0)
                samePlateListHalfCircle = samePlateListHalfCircleOrigin.OrderByDescending(x => x.bottomLength).ToList();

            List<SamePlateInfo> samePlateListTwinArcOrigin = GetSameShapeList(sortPlateInfoList, PLATESHAPE.TWINARC);
            if (samePlateListTwinArcOrigin.Count != 0)
                samePlateListTwinArc = samePlateListTwinArcOrigin.OrderByDescending(x => x.bottomLength).ToList();



            List<Entity> arrangePlateList = new List<Entity>();
            List<Line> plateLineList = new List<Line>();
            List<Entity> dimLineList = new List<Entity>();
            /*
            List<Entity> newSortPlateList = new List<Entity>();
            List<int> arrangePlateNumList = new List<int>(); // Arraged Plate에 포함되는 eachPlate의 번호 저장 2개 이상일때를 위해서 2중List?
            List<Entity> centerLineList = new List<Entity>();
            /**/



            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            List<List<Entity>> sortLineList = new List<List<Entity>>();
            List<List<Entity>> testDrawSortLineList = new List<List<Entity>>();

            /**/
            //double plateCount = sortPlateInfoList.Count();
            double distancePlate = plateLength * 2; // Plate 간 간격 : WholePlate width 1/2

            ///////////////////////////////////////////////
            // Right Draw : Arrange Plate
            ///////////////////////////////////////////////



            // 정렬순서상 큰것과 작은것 끼리 정렬후 쌓기 
            //countWholePlate: 다 채우고나면 1씩 증가
            //double lineBottomLength = 0;


            // todo : Function()
            // 함수로 바꿀때 인자로 받을 것 ListA, ListB, bool SameList



            double increaseDrawYAxis = 0;


            // Draw : Whole Plate
            ArrangePlate arrangePlate = new ArrangePlate();
            arrangePlate.ArrangePlateInit();

            if (samePlateListWhole.Count != 0)
            {
                // Draw : Plate Box, PlateA
                Point3D drawStartPoint = GetSumPoint(referencePoint, 0, increaseDrawYAxis * (plateWidth + distancePlate));
                List<Line> wholePlateList = GetRectangle(GetSumPoint(drawStartPoint, 0, 0), plateWidth, plateLength);
                plateLineList.AddRange(wholePlateList);
                // Add Paper Area
                cuttingAreaList.Add(new DrawCuttingAreaModel()
                {
                    refPoint = GetSumPoint(drawStartPoint, 0, 0),
                    plateLength = plateLength,
                    plateWidth = plateWidth,
                    displayName = samePlateListWhole[0].tempPlateInfo.name,
                    centerPoint = GetSumPoint(drawStartPoint, plateLength / 2, plateWidth / 2)
                });

                // Leader
                string leaderCourseInfoD = "   REQ'D  : " + valueService.GetOrdinalSheet(samePlateListWhole[0].countPlate);
                DrawBMLeaderModel leaderInfoModel = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = leaderCourseInfoD, bmNumber = "", textAlign = POSITION_TYPE.CENTER, arrowHeadVisible = false };
                DrawEntityModel leaderInfoList = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(drawStartPoint, plateLength, 0), leaderInfoModel, scaleValue);
                dimLineList.AddRange(leaderInfoList.leaderlineList);
                dimLineList.AddRange(leaderInfoList.leaderTextList);
                dimLineList.AddRange(leaderInfoList.leaderArrowList);


                arrangePlate = GetDrawListOfShape(drawStartPoint, samePlateListWhole[0].tempPlateInfo, plateLength,scaleValue);

                arrangePlateList.AddRange(arrangePlate.drawEntityList);
                dimLineList.AddRange(arrangePlate.drawEntityDimensionList);
                increaseDrawYAxis++;

                // Remove : Used(Draw) Info
                samePlateListWhole.RemoveAt(0);
            }


            // Draw : Rectangle Plate
            #region
            ArrangePlate eachArrangePlateA = new ArrangePlate();
            eachArrangePlateA.ArrangePlateInit();
            ArrangePlate eachArrangePlateB = new ArrangePlate();
            eachArrangePlateB.ArrangePlateInit();

            if (samePlateListRectangle.Count != 0)
            {
                for (int i = 0; i < samePlateListRectangle.Count; i++)
                {
                    // Draw : Plate Box

                    bool isFindPair = false;

                    Point3D drawStartPoint = GetSumPoint(referencePoint, 0, increaseDrawYAxis * (plateWidth + distancePlate));
                    plateLineList.AddRange(GetRectangle(drawStartPoint, plateWidth, plateLength));
                    // Add Paper Area
                    cuttingAreaList.Add(new DrawCuttingAreaModel()
                    {
                        refPoint = GetSumPoint(drawStartPoint, 0, 0),
                        plateLength = plateLength,
                        plateWidth = plateWidth,
                        displayName = samePlateListRectangle[0].tempPlateInfo.name,
                        centerPoint = GetSumPoint(drawStartPoint, plateLength / 2, plateWidth / 2)
                    });
                    // Leader
                    string leaderCourseInfoD = "   REQ'D  : " + valueService.GetOrdinalSheet(samePlateListRectangle[i].countPlate);
                    DrawBMLeaderModel leaderInfoModel = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = leaderCourseInfoD, bmNumber = "", textAlign = POSITION_TYPE.CENTER, arrowHeadVisible = false };
                    DrawEntityModel leaderInfoList = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(drawStartPoint, plateLength, 0), leaderInfoModel, scaleValue);
                    dimLineList.AddRange(leaderInfoList.leaderlineList);
                    dimLineList.AddRange(leaderInfoList.leaderTextList);
                    dimLineList.AddRange(leaderInfoList.leaderArrowList);


                    // Draw : Plate
                    eachArrangePlateA = GetDrawListOfShape(drawStartPoint, samePlateListRectangle[i].tempPlateInfo, plateLength, scaleValue);
                    arrangePlateList.AddRange(eachArrangePlateA.drawEntityList);
                    dimLineList.AddRange(eachArrangePlateA.drawEntityDimensionList);
                    eachArrangePlateA.isSuccess = true;

                    /*
                    if ((i + 1) == samePlateListRectangle.Count)
                    {
                        samePlateListRectangle.RemoveAt(i);
                    }
                    else
                    {
                    /**/
                    for (int k = i + 1; k < samePlateListRectangle.Count; k++)
                    {
                        if (samePlateListRectArc.Count == 1) { break; }// just Draw , and save remainderPlateInfo

                        if (eachArrangePlateA.remainderMidLength > samePlateListRectangle[k].tempPlateInfo.bottomLength)
                        {

                            eachArrangePlateB = GetDrawListOfShape(GetSumPoint(drawStartPoint, plateLength - eachArrangePlateA.remainderMidLength, 0),
                                                                    samePlateListRectangle[k].tempPlateInfo, plateLength, scaleValue);
                            arrangePlateList.AddRange(eachArrangePlateB.drawEntityList);
                            dimLineList.AddRange(eachArrangePlateB.drawEntityDimensionList);

                            isFindPair = true;

                            // Remove : Used(Draw) Info
                            samePlateListRectangle.RemoveAt(k);
                            samePlateListRectangle.RemoveAt(i);

                            eachArrangePlateA.ArrangePlateInit();
                            eachArrangePlateB.ArrangePlateInit();
                        }
                        //eachArrangePlate = GetDrawArrangePlate(drawStartPoint, eachArrangePlate, samePlateListRectangle[k], plateWidth, plateLength);
                    }
                    // Remove : Used(Draw) Info
                    /*
                    if(!isFindPair)
                    rectArcArrangePlateA.ArrangePlateInit();
                    rectArcArrangePlateB.ArrangePlateInit();
                    /**/

                    increaseDrawYAxis++;

                }
            }
            #endregion



            // Draw : RectangleArc Plate
            ArrangePlate rectArcArrangePlateA = new ArrangePlate();
            rectArcArrangePlateA.ArrangePlateInit();
            ArrangePlate rectArcArrangePlateB = new ArrangePlate();
            rectArcArrangePlateB.ArrangePlateInit();

            #region
            /*
            if (samePlateListRectArc.Count != 0)
            {

                if (!eachArrangePlateA.isSuccess)
                {
                    for (int t = 0; t < samePlateListRectArc.Count; t++)
                    {
                        // Draw : Plate Box
                        Point3D drawStartPoint = GetSumPoint(referencePoint, 0, increaseDrawYAxis * (plateWidth + distancePlate));
                        plateLineList.AddRange(GetRectangle(drawStartPoint, plateWidth, plateLength));

                        if (eachArrangePlateA.remainderMidLength > samePlateListRectArc[t].tempPlateInfo.bottomLength)
                        {

                            rectArcArrangePlateA = GetDrawListOfShape(GetSumPoint(drawStartPoint, plateLength - eachArrangePlateA.remainderMidLength, 0),
                                                                   samePlateListRectArc[t].tempPlateInfo, plateLength);
                            arrangePlateList.AddRange(eachArrangePlateB.drawEntityList);
                            rectArcArrangePlateA.isSuccess = true;

                            // Remove : Used(Draw) Info
                            if (rectArcArrangePlateA.isSuccess)
                            {
                                samePlateListRectArc.RemoveAt(t);
                                rectArcArrangePlateA.ArrangePlateInit();
                            }

                            break;
                        }
                    }

                    increaseDrawYAxis++;
                }
                /**/
            #endregion

            for (int i = 0; i < samePlateListRectArc.Count; i++)
            {
                // Draw : Plate(Right)
                Point3D drawStartPoint = GetSumPoint(referencePoint, 0, increaseDrawYAxis * (plateWidth + distancePlate));

                bool isFindPair = false;


                rectArcArrangePlateA = GetDrawListOfShape(drawStartPoint, samePlateListRectArc[i].tempPlateInfo, plateLength, scaleValue);
                arrangePlateList.AddRange(rectArcArrangePlateA.drawEntityList);
                dimLineList.AddRange(rectArcArrangePlateA.drawEntityDimensionList);
                rectArcArrangePlateA.isSuccess = true;


                for (int k = (i + 1); k < samePlateListRectArc.Count; k++)
                {

                    if (samePlateListRectArc.Count == 1) { break; }// just Draw , and save remainderPlateInfo

                    rectArcArrangePlateB = GetDrawListOfShape(drawStartPoint, samePlateListRectArc[k].tempPlateInfo, plateLength, scaleValue);

                    // 두 Plate간 MidPoint 거리
                    double arcDistance = rectArcArrangePlateA.remainderMidLength - (plateLength - rectArcArrangePlateB.remainderMidLength);

                    // 거리가 최소간격(15) 이상일때
                    if (arcDistance > minDistance)
                    {
                        foreach (Entity eachRotate in rectArcArrangePlateB.drawEntityList)
                        {
                            eachRotate.Rotate(Utility.DegToRad(180), Vector3D.AxisZ, drawStartPoint);
                        }

                        foreach (Entity eachTranslate in rectArcArrangePlateB.drawEntityList)
                        {
                            eachTranslate.Translate(plateLength, plateWidth);
                        }

                        /**/
                        Point3D[] arcIntersect = ((Arc)rectArcArrangePlateA.drawEntityList[3]).IntersectWith(((Arc)rectArcArrangePlateB.drawEntityList[3]));

                        if (arcIntersect.Length != 0)
                        {
                            // Arc끼리 겹칠때
                            continue;
                        }
                        else
                        {
                            /**/
                            // 가장 가까운 거리 찾기
                            #region
                            Arc arcA = null;
                            Arc arcB = null;

                            arcA = (Arc)rectArcArrangePlateA.drawEntityList[3];
                            arcB = (Arc)rectArcArrangePlateB.drawEntityList[3];

                            double topA_X, midA_X, bottomA_X;
                            double topB_X, midB_X, bottomB_X;

                            topA_X = midA_X = bottomA_X = 0;
                            topB_X = midB_X = bottomB_X = 0;

                            if (arcA.StartPoint.Y > arcA.EndPoint.Y) topA_X = arcA.StartPoint.X;
                            else bottomA_X = arcA.StartPoint.X;

                            if (arcA.StartPoint.Y < arcA.EndPoint.Y) topA_X = arcA.EndPoint.X;
                            else bottomA_X = arcA.EndPoint.X;

                            if (arcB.StartPoint.Y > arcB.EndPoint.Y) topB_X = arcB.StartPoint.X;
                            else bottomB_X = arcB.StartPoint.X;

                            if (arcB.StartPoint.Y < arcB.EndPoint.Y) topB_X = arcB.EndPoint.X;
                            else bottomB_X = arcB.EndPoint.X;

                            double distanceTop = topB_X - topA_X;
                            double distanceBottom = bottomB_X - bottomA_X;
                            double distanceMid = arcB.MidPoint.X - arcA.MidPoint.X;

                            double minDis = Math.Min(distanceTop, distanceBottom);
                            minDis = Math.Min(minDis, distanceMid);
                            #endregion
                            // minDis
                            /**/

                            // 기울기에 따라서 기울기 비슷하면 거리만큼 당기고
                            // 기울기가 차이가 크면 ArcPlate 로 교체
                            #region
                            /*
                            double shiftLeft = 500; // 가까운 거리로 붙이기 (임시) - 이후 다시 겹침확인해야함
                            if (minDis > shiftLeft) 
                            {
                                double shiftLeftFinal = arcDistance - minDis *3; // 가까운 거리의 2배 곡선감안해서

                                if (shiftLeftFinal > 0)
                                {
                                    
                                    Point3D[] arcIntersect2 = ((Arc)rectArcArrangePlateA.drawEntityList[3]).IntersectWith(((Arc)rectArcArrangePlateB.drawEntityList[3]));
                                    {
                                        foreach (Entity eachTranslate in rectArcArrangePlateB.drawEntityList)
                                        {
                                            eachTranslate.Translate(-(shiftLeft-300), 0);
                                        }
                                    }
                                }
                            }
                            /**/
                            #endregion

                            arrangePlateList.AddRange(rectArcArrangePlateB.drawEntityList);

                            dimLineList.AddRange(GetCustomDimension(rectArcArrangePlateB.drawEntityList, scaleValue));
                            samePlateListRectArc.RemoveAt(k);

                            isFindPair = true;
                            break;
                        }
                    }
                    else rectArcArrangePlateB.ArrangePlateInit();

                }

                // RectangleArc 에서 짝을 못찾았을때, Arc검색
                #region

                if (!isFindPair)
                {
                    for (int m = 0; m < samePlateListArc.Count; m++)
                    {
                        ArrangePlate arcArrangePlateB = new ArrangePlate();
                        arcArrangePlateB.ArrangePlateInit();

                        if (samePlateListArc.Count == 1) { break; }// just Draw , and save remainderPlateInfo

                        arcArrangePlateB = GetDrawListOfShape(drawStartPoint, samePlateListArc[m].tempPlateInfo, plateLength, scaleValue);

                        // 두 Plate간 MidPoint 거리
                        double arcDistance = rectArcArrangePlateA.remainderMidLength - (plateLength - arcArrangePlateB.remainderMidLength);

                        // 거리가 최소간격(15) 이상일때
                        if (arcDistance > minDistance)
                        {

                            foreach (Entity eachRotate in arcArrangePlateB.drawEntityList)
                            {
                                eachRotate.Rotate(Utility.DegToRad(180), Vector3D.AxisZ, drawStartPoint);
                            }

                            foreach (Entity eachTranslate in arcArrangePlateB.drawEntityList)
                            {
                                eachTranslate.Translate(plateLength, plateWidth);
                            }

                            /**/
                            Point3D[] arcIntersect = ((Arc)rectArcArrangePlateA.drawEntityList[3]).IntersectWith(((Arc)arcArrangePlateB.drawEntityList[2]));

                            if (arcIntersect.Length != 0)
                            {
                                // Arc끼리 겹칠때
                                continue;
                            }
                            else
                            {
                                arrangePlateList.AddRange(arcArrangePlateB.drawEntityList);
                                dimLineList.AddRange(GetCustomDimension(arcArrangePlateB.drawEntityList,scaleValue));
                                samePlateListArc.RemoveAt(m);

                                isFindPair = true;
                                break;
                            }
                        }
                        else arcArrangePlateB.ArrangePlateInit();

                    }
                }
                /**/
                #endregion



                // Remove : Used(Draw) Info
                rectArcArrangePlateA.ArrangePlateInit();
                rectArcArrangePlateB.ArrangePlateInit();

                plateLineList.AddRange(GetRectangle(drawStartPoint, plateWidth, plateLength));
                // Add Paper Area
                cuttingAreaList.Add(new DrawCuttingAreaModel()
                {
                    refPoint = GetSumPoint(drawStartPoint, 0, 0),
                    plateLength = plateLength,
                    plateWidth = plateWidth,
                    displayName = samePlateListRectArc[i].tempPlateInfo.name,
                    centerPoint = GetSumPoint(drawStartPoint, plateLength / 2, plateWidth / 2)
                });

                // Leader
                string leaderCourseInfoD = "   REQ'D  : " + valueService.GetOrdinalSheet(samePlateListRectArc[i].countPlate);
                DrawBMLeaderModel leaderInfoModel = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = leaderCourseInfoD, bmNumber = "", textAlign = POSITION_TYPE.CENTER, arrowHeadVisible = false };
                DrawEntityModel leaderInfoList = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(drawStartPoint, plateLength, 0), leaderInfoModel, scaleValue);
                dimLineList.AddRange(leaderInfoList.leaderlineList);
                dimLineList.AddRange(leaderInfoList.leaderTextList);
                dimLineList.AddRange(leaderInfoList.leaderArrowList);

                increaseDrawYAxis++;

            }


            // Draw : Arc
            #region

            if (samePlateListArc.Count != 0)
            {

                ArrangePlate arcArrangePlateB = new ArrangePlate();
                rectArcArrangePlateB.ArrangePlateInit();

                for (int i = 0; i < samePlateListArc.Count; i++)
                {
                    // Draw : Plate Box, PlateA
                    Point3D drawStartPoint = GetSumPoint(referencePoint, 0, increaseDrawYAxis * (plateWidth + distancePlate));
                    plateLineList.AddRange(GetRectangle(drawStartPoint, plateWidth, plateLength));
                    // Add Paper Area
                    cuttingAreaList.Add(new DrawCuttingAreaModel()
                    {
                        refPoint = GetSumPoint(drawStartPoint, 0, 0),
                        plateLength = plateLength,
                        plateWidth = plateWidth,
                        displayName = samePlateListArc[i].tempPlateInfo.name,
                        centerPoint = GetSumPoint(drawStartPoint, plateLength / 2, plateWidth / 2)
                    });

                    // Leader
                    string leaderCourseInfoD = "   REQ'D  : " + valueService.GetOrdinalSheet(samePlateListArc[i].countPlate); 
                    DrawBMLeaderModel leaderInfoModel = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = leaderCourseInfoD, bmNumber = "", textAlign = POSITION_TYPE.CENTER, arrowHeadVisible = false };
                    DrawEntityModel leaderInfoList = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(drawStartPoint, plateLength, 0), leaderInfoModel, scaleValue);
                    dimLineList.AddRange(leaderInfoList.leaderlineList);
                    dimLineList.AddRange(leaderInfoList.leaderTextList);
                    dimLineList.AddRange(leaderInfoList.leaderArrowList);


                    // Draw : Arc
                    arcArrangePlateB = GetDrawListOfShape(drawStartPoint, samePlateListArc[i].tempPlateInfo, plateLength, scaleValue);
                    arrangePlateList.AddRange(arcArrangePlateB.drawEntityList);
                    dimLineList.AddRange(arcArrangePlateB.drawEntityDimensionList);
                    increaseDrawYAxis++;

                    // Remove : Used(Draw) Info
                    samePlateListArc.RemoveAt(i);
                    i--;
                }
            }

            /// <------ Delete
            /*
            ///
            ArrangePlate arrangePlate = new ArrangePlate();
            arrangePlate.ArrangePlateInit();

            if (samePlateListWhole.Count != 0)
            {
                // Draw : Plate Box, PlateA
                Point3D drawStartPoint = GetSumPoint(referencePoint, 0, increaseDrawYAxis * (plateWidth + distancePlate));
                List<Line> wholePlateList = GetRectangle(GetSumPoint(drawStartPoint, 0, 0), plateWidth, plateLength);
                plateLineList.AddRange(wholePlateList);

                arrangePlate = GetDrawListOfShape(drawStartPoint, samePlateListWhole[0].tempPlateInfo, plateLength);

                arrangePlateList.AddRange(arrangePlate.drawEntityList);
                increaseDrawYAxis++;

                // Remove : Used(Draw) Info
                samePlateListWhole.RemoveAt(0);
            }
            /**/
            ////// end_Delete ----------->



            /**/
            #endregion


            styleService.SetLayerListEntity(ref arrangePlateList, layerService.LayerOutLine);
            styleService.SetLayerListLine(ref plateLineList, layerService.LayerVirtualLine);
            styleService.SetLayerListEntity(ref dimLineList, layerService.LayerDimension);
            drawList.outlineList.AddRange(dimLineList);
            drawList.outlineList.AddRange(arrangePlateList);
            drawList.outlineList.AddRange(plateLineList);


            return drawList;
        }


        public List<Entity> GetCustomDimension(List<Entity> selList,double scaleValue)
        {
            List<Entity> dimList = new List<Entity>();

            List<Line> lineList = new List<Line>();
            foreach (Entity eachEntity in selList)
            {
                if(eachEntity is Line)
                {
                    Line eachLine = (Line)eachEntity;
                    if(eachLine.StartPoint.Y== eachLine.EndPoint.Y)
                        lineList.Add(eachLine);

                }
            }

            if (lineList.Count > 0)
            {
                List<Line> newLineList = lineList.OrderByDescending(x => x.StartPoint.Y).ToList();
                if (newLineList.Count == 1)
                {
                    // 1반향  아래로
                    // Dimension
                    // Dimension
                    List<Point3D> lowerPoint = new List<Point3D>();
                    lowerPoint.Add(newLineList[0].StartPoint);
                    lowerPoint.Add(newLineList[0].EndPoint);
                    List<Point3D> lowerPointSort = lowerPoint.OrderByDescending(x => x.X).ToList();
                    DrawDimensionModel dimCrossModel2 = new DrawDimensionModel()
                    {
                        position = POSITION_TYPE.BOTTOM,
                        textUpper = Math.Round(newLineList[0].Length(), 1, MidpointRounding.AwayFromZero).ToString(),
                        dimHeight = 12,
                        scaleValue = scaleValue,
                    };
                    DrawEntityModel dimCross2 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lowerPoint[0], 0, 0), GetSumPoint(lowerPoint[1], 0, 0), scaleValue, dimCrossModel2, 0);
                    dimList.AddRange(dimCross2.dimlineList);
                    dimList.AddRange(dimCross2.dimTextList);
                    dimList.AddRange(dimCross2.dimlineExtList);
                    dimList.AddRange(dimCross2.dimArrowList);
                }
                else if (newLineList.Count == 2)
                {
                    List<Point3D> upperPoint = new List<Point3D>();
                    List<Point3D> lowerPoint = new List<Point3D>();
                    upperPoint.Add(newLineList[0].StartPoint);
                    upperPoint.Add(newLineList[0].EndPoint);
                    lowerPoint.Add(newLineList[1].StartPoint);
                    lowerPoint.Add(newLineList[1].EndPoint);
                    List<Point3D> upperPointSort = upperPoint.OrderByDescending(x => x.X).ToList();
                    List<Point3D> lowerPointSort = lowerPoint.OrderByDescending(x => x.X).ToList();
                    // Dimension
                    DrawDimensionModel dimCrossModel = new DrawDimensionModel()
                    {
                        position = POSITION_TYPE.TOP,
                        textUpper = Math.Round(newLineList[0].Length(), 1, MidpointRounding.AwayFromZero).ToString(),
                        dimHeight = 12,
                        scaleValue = scaleValue,
                    };
                    DrawEntityModel dimCross = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(upperPoint[0], 0, 0), GetSumPoint(upperPoint[1], 0, 0), scaleValue, dimCrossModel, 0);
                    dimList.AddRange(dimCross.dimlineList);
                    dimList.AddRange(dimCross.dimTextList);
                    dimList.AddRange(dimCross.dimlineExtList);
                    dimList.AddRange(dimCross.dimArrowList);

                    // Dimension
                    DrawDimensionModel dimCrossModel2 = new DrawDimensionModel()
                    {
                        position = POSITION_TYPE.BOTTOM,
                        textUpper = Math.Round(newLineList[1].Length(), 1, MidpointRounding.AwayFromZero).ToString(),
                        dimHeight = 12,
                        scaleValue = scaleValue,
                    };
                    DrawEntityModel dimCross2 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lowerPoint[0], 0, 0), GetSumPoint(lowerPoint[1], 0, 0), scaleValue, dimCrossModel2, 0);
                    dimList.AddRange(dimCross2.dimlineList);
                    dimList.AddRange(dimCross2.dimTextList);
                    dimList.AddRange(dimCross2.dimlineExtList);
                    dimList.AddRange(dimCross2.dimArrowList);
                }
            }

            return dimList;
        }


        public ArrangePlate GetDrawArrangePlate(Point3D refPoint, ArrangePlate orgArrangePlate, SamePlateInfo samePlateInfo, double plateWidth, double plateLength)
        {
            ArrangePlate newArrangePlate = new ArrangePlate();
            newArrangePlate.ArrangePlateInit();

            switch (orgArrangePlate.remainderShape)
            {
                case PLATESHAPE.EMPTY:
                    {
                        //orgArrangePlate.drawEntityList.AddRange(GetDrawListOfShape(refPoint, samePlateInfo.tempPlateInfo, plateLength));
                        if (samePlateInfo.tempPlateInfo.plateShape == PLATESHAPE.RECTANGLE)
                        {
                            orgArrangePlate.isSuccess = true;
                            newArrangePlate = orgArrangePlate;
                        }
                        break;
                    }

                case PLATESHAPE.RECTANGLE:
                    {
                        if (samePlateInfo.tempPlateInfo.plateShape == PLATESHAPE.RECTANGLE)
                        {
                            if (orgArrangePlate.remainderMidLength > samePlateInfo.bottomLength)
                            {
                                double distance = plateLength - orgArrangePlate.remainderMidLength;
                                Point3D newStartPoint = GetSumPoint(refPoint, distance, 0);
                                //      orgArrangePlate.drawEntityList.AddRange(GetDrawListOfShape(refPoint, samePlateInfo.tempPlateInfo));
                                orgArrangePlate.isSuccess = true;
                            }
                        }
                        else
                        {

                        }
                        break;
                    }
            }



            if (newArrangePlate.isSuccess)
            {
                newArrangePlate.countPlate = orgArrangePlate.countPlate += 1;
                newArrangePlate.plateNumberList.Add(samePlateInfo.plateNumber);


                if (samePlateInfo.tempPlateInfo.plateShape == PLATESHAPE.RECTANGLE)
                {
                    newArrangePlate.remainderMidLength = plateLength - samePlateInfo.tempPlateInfo.bottomLength;
                    newArrangePlate.remainderShape = PLATESHAPE.RECTANGLE;
                }
                else if (samePlateInfo.tempPlateInfo.plateShape == PLATESHAPE.RECTANGLEARC)
                {
                    newArrangePlate.remainderMidLength =
                        plateLength - (samePlateInfo.tempPlateInfo.bottomLength + samePlateInfo.tempPlateInfo.topLength) / 2;
                }
                else if (samePlateInfo.tempPlateInfo.plateShape == PLATESHAPE.ARC)
                {
                    newArrangePlate.remainderMidLength =
                        plateLength - samePlateInfo.tempPlateInfo.bottomLength / 2;
                }

                if (newArrangePlate.countPlate > 1) newArrangePlate.remainderShape = PLATESHAPE.WHOLEPLATE;

            }

            return newArrangePlate;
        }

        public ArrangePlate GetDrawListOfShape(Point3D startPoint, tempPlateInfo TempPlateInfo, double plateLength,double scaleValue)
        {
            // startPoint = LeftBottom point
            ArrangePlate newArrangePlate = new ArrangePlate();
            newArrangePlate.ArrangePlateInit();


            Point3D refPoint = GetSumPoint(startPoint, 0, 0);
            tempPlateInfo tempPlateInfo = (tempPlateInfo)TempPlateInfo.Clone();
            PLATESHAPE plateShape = (PLATESHAPE)tempPlateInfo.plateShape;

            double topLength = tempPlateInfo.topLength;
            double bottomLength = tempPlateInfo.bottomLength;
            double leftWidth = tempPlateInfo.leftWidth;
            double rigthWidth = tempPlateInfo.rightWidth;
            double arcRadius = tempPlateInfo.arcRadius;



            switch (plateShape)
            {
                case PLATESHAPE.RECTANGLEARC:
                    {
                        // Draw Line
                        Line bottomLine = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, tempPlateInfo.bottomLength, 0));
                        Line leftLine = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, 0, tempPlateInfo.leftWidth));
                        Line topLine = new Line(GetSumPoint(leftLine.EndPoint, 0, 0), GetSumPoint(leftLine.EndPoint, tempPlateInfo.topLength, 0));

                        // Draw Arc
                        Arc rightArc = GetDrawArc(arcRadius, topLine.EndPoint, bottomLine.EndPoint);

                        // index[3] = Arc
                        newArrangePlate.drawEntityList.AddRange(new Entity[] { bottomLine, leftLine, topLine, rightArc });

                        newArrangePlate.plateNumberList.Add(tempPlateInfo.number);
                        newArrangePlate.remainderMidLength = plateLength - (rightArc.MidPoint.X - refPoint.X);
                        newArrangePlate.remainderShape = PLATESHAPE.RECTANGLEARC;
                        newArrangePlate.countPlate += 1;

                        // Dimension
                        DrawDimensionModel dimCrossModel = new DrawDimensionModel()
                        {
                            position = POSITION_TYPE.TOP,
                            textUpper = Math.Round(topLine.Length(),1, MidpointRounding.AwayFromZero).ToString(),
                            dimHeight = 12,
                            scaleValue = scaleValue,
                        };
                        DrawEntityModel dimCross = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(topLine.StartPoint, 0, 0), GetSumPoint(topLine.EndPoint,0, 0), scaleValue, dimCrossModel, 0);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross.dimlineList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross.dimTextList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross.dimlineExtList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross.dimArrowList);

                        // Dimension
                        DrawDimensionModel dimCrossModel2 = new DrawDimensionModel()
                        {
                            position = POSITION_TYPE.BOTTOM,
                            textUpper = Math.Round(bottomLine.Length(), 1, MidpointRounding.AwayFromZero).ToString(),
                            dimHeight = 12,
                            scaleValue = scaleValue,
                        };
                        DrawEntityModel dimCross2 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(bottomLine.StartPoint, 0, 0), GetSumPoint(bottomLine.EndPoint, 0, 0), scaleValue, dimCrossModel2, 0);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross2.dimlineList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross2.dimTextList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross2.dimlineExtList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross2.dimArrowList);

                        break;
                    }

                case PLATESHAPE.ARC:
                    {
                        // Draw Line
                        Line bottomLine = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, tempPlateInfo.bottomLength, 0));
                        Line leftLine = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, 0, tempPlateInfo.leftWidth));

                        // Draw Arc
                        Arc rightArc = GetDrawArc(arcRadius, leftLine.EndPoint, bottomLine.EndPoint);

                        newArrangePlate.drawEntityList.AddRange(new Entity[] { bottomLine, leftLine, rightArc });

                        newArrangePlate.plateNumberList.Add(tempPlateInfo.number);
                        newArrangePlate.remainderMidLength = plateLength - (rightArc.MidPoint.X - refPoint.X);
                        newArrangePlate.remainderShape = PLATESHAPE.ARC;
                        newArrangePlate.countPlate += 1;

                        // Dimension
                        DrawDimensionModel dimCrossModel2 = new DrawDimensionModel()
                        {
                            position = POSITION_TYPE.BOTTOM,
                            textUpper = Math.Round(bottomLine.Length(), 1, MidpointRounding.AwayFromZero).ToString(),
                            dimHeight = 12,
                            scaleValue = scaleValue,
                        };
                        DrawEntityModel dimCross2 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(bottomLine.StartPoint, 0, 0), GetSumPoint(bottomLine.EndPoint, 0, 0), scaleValue, dimCrossModel2, 0);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross2.dimlineList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross2.dimTextList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross2.dimlineExtList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross2.dimArrowList);

                        break;
                    }
                case PLATESHAPE.HALFCIRCLE:
                    {
                        break;
                    }

                case PLATESHAPE.TWINARC:
                    {
                        break;
                    }
                case PLATESHAPE.RECTANGLE:
                    {
                        List<Line> recList = GetRectangle(refPoint, leftWidth, topLength);
                        
                        newArrangePlate.drawEntityList.AddRange(recList);

                        newArrangePlate.plateNumberList.Add(tempPlateInfo.number);
                        newArrangePlate.remainderMidLength = plateLength - tempPlateInfo.bottomLength;
                        newArrangePlate.remainderShape = PLATESHAPE.RECTANGLE;
                        newArrangePlate.countPlate += 1;

                        // 위 : 2
                        // 아래 : 0

                        // Dimension
                        DrawDimensionModel dimCrossModel = new DrawDimensionModel()
                        {
                            position = POSITION_TYPE.TOP,
                            textUpper = Math.Round(recList[2].Length(), 1, MidpointRounding.AwayFromZero).ToString(),
                            dimHeight = 12,
                            scaleValue = scaleValue,
                        };
                        DrawEntityModel dimCross = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(recList[2].StartPoint, 0, 0), GetSumPoint(recList[2].EndPoint, 0, 0), scaleValue, dimCrossModel, 0);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross.dimlineList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross.dimTextList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross.dimlineExtList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross.dimArrowList);

                        // Dimension
                        DrawDimensionModel dimCrossModel2 = new DrawDimensionModel()
                        {
                            position = POSITION_TYPE.BOTTOM,
                            textUpper = Math.Round(recList[0].Length(), 1, MidpointRounding.AwayFromZero).ToString(),
                            dimHeight = 12,
                            scaleValue = scaleValue,
                        };
                        DrawEntityModel dimCross2 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(recList[0].StartPoint, 0, 0), GetSumPoint(recList[0].EndPoint, 0, 0), scaleValue, dimCrossModel2, 0);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross2.dimlineList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross2.dimTextList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross2.dimlineExtList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross2.dimArrowList);



                        break;
                    }

                case PLATESHAPE.WHOLEPLATE:
                    {
                        List<Line> recList = GetRectangle(refPoint, leftWidth, topLength);
                        newArrangePlate.drawEntityList.AddRange(recList);


                        // 위 : 2
                        // 왼 : 3

                        // Dimension
                        DrawDimensionModel dimCrossModel = new DrawDimensionModel()
                        {
                            position = POSITION_TYPE.TOP,
                            textUpper = Math.Round(recList[2].Length(), 1, MidpointRounding.AwayFromZero).ToString(),
                            textLower = "(TYP.)",
                            dimHeight = 12,
                            scaleValue = scaleValue,
                        };
                        DrawEntityModel dimCross = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(recList[2].StartPoint, 0, 0), GetSumPoint(recList[2].EndPoint, 0, 0), scaleValue, dimCrossModel, 0);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross.dimlineList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross.dimTextList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross.dimlineExtList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross.dimArrowList);

                        // Dimension
                        DrawDimensionModel dimCrossModel2 = new DrawDimensionModel()
                        {
                            position = POSITION_TYPE.LEFT,
                            textUpper = Math.Round(recList[3].Length(), 1, MidpointRounding.AwayFromZero).ToString(),
                            textLower="(TYP.)",
                            dimHeight = 12,
                            scaleValue = scaleValue,
                        };
                        DrawEntityModel dimCross2 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(recList[3].EndPoint, 0, 0), GetSumPoint(recList[3].StartPoint, 0, 0), scaleValue, dimCrossModel2, 0);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross2.dimlineList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross2.dimTextList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross2.dimlineExtList);
                        newArrangePlate.drawEntityDimensionList.AddRange(dimCross2.dimArrowList);
                        break;
                    }
            }


            return newArrangePlate;
        }


        public Arc GetDrawArc(double Radius, Point3D StartPoint, Point3D EndPoint)
        {
            // Guide for Arc
            Circle topCircle = new Circle(StartPoint, Radius);
            Circle bottomCircle = new Circle(EndPoint, Radius);

            // intersect LeftPoint : (CenterPoint)
            Point3D[] intersectCircle = topCircle.IntersectWith(bottomCircle);

            Point3D arcCenterPoint = new Point3D(0, 0);
            if (intersectCircle[0].X > intersectCircle[1].X)
            { arcCenterPoint = GetSumPoint(intersectCircle[1], 0, 0); }
            else { arcCenterPoint = GetSumPoint(intersectCircle[0], 0, 0); }
            Arc resultArc = new Arc(arcCenterPoint, StartPoint, EndPoint);

            return resultArc;
        }

        public List<tempPlateInfo> GetSortPlateInfoList(List<tempPlateInfo> orgPlateInfo)
        {
            List<tempPlateInfo> sortPlateInfo = new List<tempPlateInfo>();

            // 오름차순 정렬(Plate Number 기준)
            sortPlateInfo = orgPlateInfo.OrderBy(a => a.number).ToList();

            /*
            // Transform : Top Right (TR) 모양으로 (Right Open:Arc)
            foreach (tempPlateInfo eachPlateInfo in orgPlateInfo)
            {
                if (eachPlateInfo.plateShape == PLATESHAPE.RECTANGLEARC || eachPlateInfo.plateShape == PLATESHAPE.ARC)
                { 

                }
            
            }
            /**/



            return sortPlateInfo;
        }







        // Model Center Point : real Time Setting
        public void SetModelCenterPoint(PAPERSUB_TYPE subName,Point3D centerPoint)
        {
            foreach(PaperAreaModel eachArea in SingletonData.PaperArea.AreaList)
            {
                if (eachArea.SubName == subName)
                {
                    eachArea.ModelCenterLocation.X = centerPoint.X;
                    eachArea.ModelCenterLocation.Y = centerPoint.Y;
                }
            }
        }

        

        public double GetNeedCompRIngCount(double radiusouter, double plateLength, double bendingPlateThk)
        {
            Point3D referencePoint = new Point3D(0, 0);
            double totalBendingPlate = 0;

            // Calculate : bendingPlate Length
            double minBendPlateCount = Math.Ceiling((2 * radiusouter * Math.PI) / plateLength);

            /* // 짝수 무시중..  짝수적용시 사용
            if (0 != minBendPlateCount % 2) { totalBendingPlate = minBendPlateCount + 1; }  // Plate 장수를 짝수로 맞춤
            /**/
            totalBendingPlate = minBendPlateCount;

            double eachPlateDegree = 360 / totalBendingPlate;
            double eachPlateHalfDeg = eachPlateDegree / 2;

            // testArc : Bending Plate
            //double arcCenterPointX = pBottomLine.MidPoint.X;
            //Point3D arcCenterPoint = GetSumPoint(referencePoint, arcCenterPointX, -radiusInner);
            Arc testArc = new Arc(referencePoint, radiusouter, Utility.DegToRad(90 + eachPlateHalfDeg), Utility.DegToRad(90 - eachPlateHalfDeg));
            double arcHeight = testArc.MidPoint.Y - testArc.StartPoint.Y;
            double firstBendPlateHeight = arcHeight + bendingPlateThk;
            double arcChordLength = testArc.EndPoint.X - testArc.StartPoint.X;
            // double testArcLength = testArc.Length();

            // plate 길이보다 Bending Plate의 CHD가 길면 BendingPlate 갯수 추가(각도 재계산:각도 작아짐)
            if (arcChordLength > plateLength)
            {
                minBendPlateCount += 1;
                /* // 짝수 무시중..  짝수적용시 사용
                if (0 != minBendPlateCount % 2) { totalBendingPlate = minBendPlateCount + 1; }  // Plate 장수를 짝수로 맞춤
                /**/
                totalBendingPlate = minBendPlateCount;
            }

            return totalBendingPlate;

        }

        public Line GetDiagonalLineOfArc(double radius, Point3D centerPoint, double angle, Arc inLine, Arc outLine)
        {
            Line diagnalLine = null;

            Line guideLine = new Line(centerPoint, outLine.MidPoint);
            guideLine.Rotate(Utility.DegToRad(angle), Vector3D.AxisZ, centerPoint);

            Point3D[] intersectInPoint = guideLine.IntersectWith(inLine);
            Point3D[] intersectOutPoint = guideLine.IntersectWith(outLine);

            return diagnalLine = new Line(intersectInPoint[0], intersectOutPoint[0]);
        }

        public List<Line> GetRectangle(Point3D startPoint, double width, double length, RECTANGLUNVIEW unViewPosition)
        {
            // startPoint = LeftBottom point
            List<Line> rectangleLineList = new List<Line>();

            if (unViewPosition != RECTANGLUNVIEW.BOTTOM)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, 0), GetSumPoint(startPoint, length, 0)));
            if (unViewPosition != RECTANGLUNVIEW.RIGHT)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, length, width), GetSumPoint(startPoint, length, 0)));
            if (unViewPosition != RECTANGLUNVIEW.TOP)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, width), GetSumPoint(startPoint, length, width)));
            if (unViewPosition != RECTANGLUNVIEW.LEFT)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, width), GetSumPoint(startPoint, 0, 0)));

            return rectangleLineList;
        }

        public List<Line> GetRectangleLT(Point3D startPoint, double width, double length, RECTANGLUNVIEW unViewPosition = RECTANGLUNVIEW.NONE)
        {
            // startPoint = LeftTop point
            List<Line> rectangleLineList = new List<Line>();

            if (unViewPosition != RECTANGLUNVIEW.BOTTOM)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, -width), GetSumPoint(startPoint, length, -width)));
            if (unViewPosition != RECTANGLUNVIEW.RIGHT)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, length, 0), GetSumPoint(startPoint, length, -width)));
            if (unViewPosition != RECTANGLUNVIEW.TOP)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, 0), GetSumPoint(startPoint, length, 0)));
            if (unViewPosition != RECTANGLUNVIEW.LEFT)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, 0), GetSumPoint(startPoint, 0, -width)));

            return rectangleLineList;
        }

        public List<Line> GetRectangle(Point3D startPoint, double width, double length, RECTANGLEALIGN align = RECTANGLEALIGN.NONE, double widthRight = 0, double lengthBottom = 0, RECTANGLUNVIEW unViewPosition = RECTANGLUNVIEW.NONE)
        {


            // startPoint = LeftBottom point
            List<Line> rectangleLineList = new List<Line>();

            switch (align)
            {
                case RECTANGLEALIGN.NONE:
                    if (unViewPosition != RECTANGLUNVIEW.BOTTOM)
                        rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, 0), GetSumPoint(startPoint, length, 0)));
                    if (unViewPosition != RECTANGLUNVIEW.RIGHT)
                        rectangleLineList.Add(new Line(GetSumPoint(startPoint, length, width), GetSumPoint(startPoint, length, 0)));
                    if (unViewPosition != RECTANGLUNVIEW.TOP)
                        rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, width), GetSumPoint(startPoint, length, width)));
                    if (unViewPosition != RECTANGLUNVIEW.LEFT)
                        rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, width), GetSumPoint(startPoint, 0, 0)));
                    break;

                case RECTANGLEALIGN.CENTER:
                    double widthMax = width;
                    double lengthMax = length;
                    double jumpSpace = 0;

                    if (widthRight != 0) { break; }
                    if (lengthBottom != 0)
                    {
                        if (length > lengthBottom)
                        {
                            jumpSpace = (lengthMax - lengthBottom) / 2;

                            if (unViewPosition != RECTANGLUNVIEW.BOTTOM)
                                rectangleLineList.Add(new Line(GetSumPoint(startPoint, jumpSpace, 0), GetSumPoint(startPoint, jumpSpace + lengthBottom, 0)));
                            if (unViewPosition != RECTANGLUNVIEW.RIGHT)
                                rectangleLineList.Add(new Line(GetSumPoint(startPoint, length, width), GetSumPoint(startPoint, jumpSpace + lengthBottom, 0)));
                            if (unViewPosition != RECTANGLUNVIEW.TOP)
                                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, width), GetSumPoint(startPoint, length, width)));
                            if (unViewPosition != RECTANGLUNVIEW.LEFT)
                                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, width), GetSumPoint(startPoint, jumpSpace, 0)));
                        }
                        else
                        {
                            /*
                            lengthMax = lengthBottom;
                            jumpSpace = (lengthMax - length) / 2;

                            if (unViewPosition != RECTANGLUNVIEW.BOTTOM)
                                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, 0), GetSumPoint(startPoint, jumpSpace + lengthBottom, 0)));
                            if (unViewPosition != RECTANGLUNVIEW.RIGHT)
                                rectangleLineList.Add(new Line(GetSumPoint(startPoint, length, width), GetSumPoint(startPoint, jumpSpace + lengthBottom, 0)));
                            if (unViewPosition != RECTANGLUNVIEW.TOP)
                                rectangleLineList.Add(new Line(GetSumPoint(startPoint, jumpSpace, width), GetSumPoint(startPoint, length, width)));
                            if (unViewPosition != RECTANGLUNVIEW.LEFT)
                                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, width), GetSumPoint(startPoint, jumpSpace, 0)));
                            /**/
                            break;
                        }

                    }

                    break;

            }


            return rectangleLineList;
        }

        public Line MirrorLine(Line orgLine, Vector2D axis, double distance)
        {
            // Reference Position
            // AxisX = Left, AxisY = Top

            Line mirrorLine = null;

            int axisXY = 0;
            if (axis == Vector2D.AxisY) axisXY = 1;

            switch (axisXY)
            {
                case 0: // AxisX
                    {

                        // vertical Line Type
                        if (orgLine.StartPoint.X == orgLine.EndPoint.X)
                        {
                            if (orgLine.StartPoint.Y < orgLine.EndPoint.Y)
                            {
                                Point3D tempPoint = orgLine.EndPoint;
                                orgLine.EndPoint = orgLine.StartPoint;
                                orgLine.StartPoint = tempPoint;
                            }

                            return mirrorLine = new Line(GetSumPoint(orgLine.StartPoint, distance * 2, 0), GetSumPoint(orgLine.EndPoint, distance * 2, 0));
                        }
                        else
                        {
                            if (orgLine.StartPoint.X > orgLine.EndPoint.X)
                            {
                                Point3D tempPoint = orgLine.EndPoint;
                                orgLine.EndPoint = orgLine.StartPoint;
                                orgLine.StartPoint = tempPoint;
                            }

                            double distanceXLine = Math.Abs(orgLine.EndPoint.X - orgLine.StartPoint.X);
                            return mirrorLine = new Line(GetSumPoint(orgLine.EndPoint, distance * 2, 0), GetSumPoint(orgLine.StartPoint, (distanceXLine * 2) + (distance * 2), 0));
                        }

                    }

                case 1: // AxisY
                    {
                        if (orgLine.StartPoint.Y == orgLine.EndPoint.Y)
                        {
                            if (orgLine.StartPoint.X > orgLine.EndPoint.X)
                            {
                                Point3D tempPoint = orgLine.EndPoint;
                                orgLine.EndPoint = orgLine.StartPoint;
                                orgLine.StartPoint = tempPoint;
                            }

                            return mirrorLine = new Line(GetSumPoint(orgLine.StartPoint, 0, -(distance * 2)), GetSumPoint(orgLine.EndPoint, 0, -(distance * 2)));
                        }
                        else
                        {
                            if (orgLine.StartPoint.Y < orgLine.EndPoint.Y)
                            {
                                Point3D tempPoint = orgLine.EndPoint;
                                orgLine.EndPoint = orgLine.StartPoint;
                                orgLine.StartPoint = tempPoint;
                            }

                            double distanceYLine = Math.Abs(orgLine.StartPoint.Y - orgLine.EndPoint.Y);
                            return mirrorLine = new Line(GetSumPoint(orgLine.EndPoint, 0, -(distance * 2)), GetSumPoint(orgLine.StartPoint, 0, -((distanceYLine * 2) + (distance * 2))));

                            //return mirrorLine = new Line(GetSumPoint(orgLine.EndPoint, 0, -(distance * 2)), GetSumPoint(orgLine.StartPoint, 0, -((orgLine.Length()) + (distance * 2))));
                        }
                    }
            }

            return null;
        }

        public Line GetInterSectLine_withTwoLines(Line leftLine, Line intersectLine, Line rightLine)
        {
            Point3D pointA = new Point3D(0, 0);
            Point3D pointB = new Point3D(0, 0);

            Point3D[] intersectPoint = intersectLine.IntersectWith(leftLine);
            if (intersectPoint.Count() < 2)
            {
                pointA = intersectPoint[0];

                intersectPoint = intersectLine.IntersectWith(rightLine);
                if (intersectPoint.Count() > 1)
                    return null;

                pointB = intersectPoint[0];
            }
            else { return null; }

            // PointA = 왼쪽 겹침점 , PointB = 오른쪽 겸침점
            return new Line(pointA, pointB);
        }

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
    }




    public struct SamePlateInfo
    {
        public int countPlate;
        public double plateNumber;
        public double bottomLength;

        public tempPlateInfo tempPlateInfo;

        public void SamePlateInfoSet()
        {
            plateNumber = 0;
            countPlate = 0;
            tempPlateInfo = new tempPlateInfo();
        }
    }


    public class tempPlateInfo : ICloneable
    {
        public bool horizontal { get; set; }
        public PLATESHAPE plateShape { get; set; }
        public PLATEPLACE platePlace { get; set; }

        public string name { get; set; }
        public double number { get; set; }
        public double topLength { get; set; }
        public double bottomLength { get; set; }
        public double leftWidth { get; set; }
        public double rightWidth { get; set; }
        public double arcRadius { get; set; }


        public object Clone()
        {
            tempPlateInfo newtempPlateInfo = new tempPlateInfo();

            newtempPlateInfo.horizontal = this.horizontal;
            newtempPlateInfo.plateShape = this.plateShape;
            newtempPlateInfo.platePlace = this.platePlace;
            newtempPlateInfo.topLength = this.topLength;
            newtempPlateInfo.bottomLength = this.bottomLength;
            newtempPlateInfo.leftWidth = this.leftWidth;
            newtempPlateInfo.rightWidth = this.rightWidth;
            newtempPlateInfo.arcRadius = this.arcRadius;
            newtempPlateInfo.name = this.name;
            newtempPlateInfo.number = this.number;

            return newtempPlateInfo;
        }


        public tempPlateInfo()
        {
            BasicCon();
        }

        private void BasicCon()
        {
            horizontal = true;
            platePlace = PLATEPLACE.TR;
            plateShape = PLATESHAPE.EMPTY;
            topLength = 0;
            bottomLength = 0;
            leftWidth = 0;
            rightWidth = 0;
            arcRadius = 0;
            name = "";
            number = 1;
        }

        public tempPlateInfo(string Name, double Number, double vertical1, double vertical2, double horizontal1, double horizontal2, PLATEPLACE PlatePlace = PLATEPLACE.TR, bool Horizontal = true, double ArcRadius = 0)
        {
            BasicCon();

            name = Name;
            number = Number;
            horizontal = Horizontal;
            platePlace = PlatePlace;

            rightWidth = vertical1;
            leftWidth = vertical2;
            topLength = horizontal1;
            bottomLength = horizontal2;
            arcRadius = ArcRadius;

            if (!Horizontal)     // Small Size Dia.  - Vertical Plate 사용하지 말고, Horizontal만 적용 ( 반원때문 )
            {
                rightWidth = horizontal1;
                leftWidth = horizontal2;

                topLength = vertical1;
                bottomLength = vertical2;
            }

        }

        public string GetPlateName()
        {
            string plateName = name + "-" + number;
            return plateName;
        }


        public void SetPlateShape(double refPlateWidth, double refPlateLength, ref tempPlateInfo eachTempPlateInfo)
        {
            PLATESHAPE plateShape = PLATESHAPE.EMPTY;

            // 오른쪽만 0(Arc)일때
            if (eachTempPlateInfo.rightWidth == 0 && eachTempPlateInfo.leftWidth != 0)
            {
                plateShape = PLATESHAPE.RECTANGLEARC;

                // Right,Top == 0 이면  Arc
                if (eachTempPlateInfo.topLength == 0) // || eachTempPlateInfo.bottomLength == 0)
                { plateShape = PLATESHAPE.ARC; }
            }

            // Top Bottom 이 같을 때 ( Whole, Rectangle, Center(좌우 Arc) )
            if (eachTempPlateInfo.topLength == eachTempPlateInfo.bottomLength)
            {
                // WholePlate Length 와 같으면 Whole
                if (refPlateLength == eachTempPlateInfo.topLength) { plateShape = PLATESHAPE.WHOLEPLATE; }

                // 좌우 길이가 0이면  Small Dia의 타원(양쪽 Arc)
                else if (eachTempPlateInfo.leftWidth == 0 && eachTempPlateInfo.rightWidth == 0)
                { plateShape = PLATESHAPE.TWINARC; }

                // 그외는 추가된 직사각형
                else
                { plateShape = PLATESHAPE.RECTANGLE; }
            }

            // 좌, 우 ,상 세지점이 0일 때 ( 작은 싸이즈 Top/bottom Center Arc)
            if (eachTempPlateInfo.rightWidth == 0 && eachTempPlateInfo.leftWidth == 0)
            {
                if (eachTempPlateInfo.topLength == 0) { plateShape = PLATESHAPE.HALFCIRCLE; }
            }

            eachTempPlateInfo.plateShape = plateShape;
        }



    }






    public class PlateInfo
    {

        //public Point3D refPoint { get; set; }
        public bool bplateHorizontal { get; set; }
        public PLATEPLACE platePlace { get; set; }
        public PLATEPOSITION platePosition { get; set; }
        public PLATESHAPE plateShape { get; set; }
        //public sPlateName SName { get; set; }
        public double plateNumber { get; set; }

        public string plateName;
        public Point3D plateTopStartPoint { get; set; }
        public Point3D plateTopEndPoint { get; set; }
        public Point3D plateBottomStartPoint { get; set; }
        public Point3D plateBottomEndPoint { get; set; }
        public double plateWidth { get; set; }
        public double Length_Top { get; set; }
        public double Length_Bottom { get; set; }

        /*
        public struct sPlateName
        {
            string sName;
            int Num;
        }
        /**/

        public PlateInfo()
        {
            BasicCon();
        }

        public PlateInfo Clone()
        {
            PlateInfo newPlate = new PlateInfo();
            newPlate.plateName = plateName;

            return newPlate;
        }
        public PlateInfo(bool bHorizontal = true, PLATEPOSITION position = PLATEPOSITION.START, string plateName = null, Point3D TopEndPoint = null, double PlateWidth = 0, double length_Top = 0, double length_Bottom = 0)

        {
            bplateHorizontal = bHorizontal;
            platePosition = position;
            plateTopEndPoint = TopEndPoint;
            plateWidth = PlateWidth;
            Length_Top = length_Top;
            Length_Bottom = length_Bottom;
            plateShape = PLATESHAPE.RECTANGLE;
            platePlace = PLATEPLACE.TR;  // 추후 입력 받을 것

            // Shape
            if (position == PLATEPOSITION.END)
            {
                if (Length_Top == 0 || length_Bottom == 0) { plateShape = PLATESHAPE.ARC; }
                else plateShape = PLATESHAPE.RECTANGLEARC;
            }

            if (bHorizontal)
            {
                plateTopStartPoint = GetSumPoint(TopEndPoint, -Length_Top, 0);
                plateBottomStartPoint = GetSumPoint(plateTopStartPoint, 0, -plateWidth);
                plateBottomEndPoint = GetSumPoint(plateBottomStartPoint, Length_Bottom, 0);
            }
            else
            {
                plateTopStartPoint = GetSumPoint(TopEndPoint, 0, -Length_Top);
                plateBottomStartPoint = GetSumPoint(plateTopStartPoint, plateWidth, 0);
                plateBottomEndPoint = GetSumPoint(plateBottomStartPoint, 0, Length_Bottom);
            }

            // plateShape 정의   Top=Bottom Length : Lectangel,  != : RectangleArc , Top or Bottom_Length == 0 : Arc
            if (Length_Top == Length_Bottom) { plateShape = PLATESHAPE.RECTANGLE; }
            else if (Length_Top == 0 || Length_Bottom == 0)
            { plateShape = PLATESHAPE.ARC; }
            else { plateShape = PLATESHAPE.RECTANGLEARC; }

        }

        /*
        public PlateInfo(Point3D TopLeftPoint, double plateWidth, double length)
        {
            BasicCon();
            plateTopStartPoint = TopLeftPoint;
            plateTopEndPoint = GetSumPoint(TopLeftPoint, length, 0);
            plateBottomStartPoint = GetSumPoint(TopLeftPoint, 0, -length);
            plateBottomEndPoint = GetSumPoint(plateBottomStartPoint, plateWidth, 0);
        }
        /**/
        public PlateInfo(string name, double plateNumberofSize, Point3D BottomLeftPoint, double plateWidth, double length)
        {
            BasicCon();
            plateName = name;
            plateNumber = plateNumberofSize;
            plateTopStartPoint = GetSumPoint(BottomLeftPoint, 0, plateWidth);
            plateTopEndPoint = GetSumPoint(plateTopStartPoint, length, 0);
            plateBottomStartPoint = BottomLeftPoint;
            plateBottomEndPoint = GetSumPoint(BottomLeftPoint, length, 0);
            plateShape = PLATESHAPE.RECTANGLE;
        }

        private void BasicCon()
        {
            bplateHorizontal = true;
            platePlace = PLATEPLACE.TR;
            platePosition = PLATEPOSITION.START;
            plateShape = PLATESHAPE.RECTANGLE;
            plateNumber = 0;
            plateName = null;
            plateTopStartPoint = new Point3D();
            plateTopEndPoint = new Point3D();
            plateBottomStartPoint = new Point3D();
            plateBottomEndPoint = new Point3D();
            plateWidth = 0;
            Length_Top = 0;
            Length_Bottom = 0;
        }


        public string GetPlateName()
        {
            string name = plateName + "-" + plateNumber;
            return name;
        }


        public void ShiftPlate(double ShiftLength)
        {
            plateTopStartPoint.X += ShiftLength;
            plateBottomStartPoint.X += ShiftLength;
            plateTopEndPoint.X += ShiftLength;
            plateBottomEndPoint.X += ShiftLength;
        }
        public void SetPlateWithTopLength(double plateLength_top)
        {
            Length_Top = plateLength_top;
            plateTopEndPoint.X = plateTopStartPoint.X + Length_Top;

        }
        public void SetPlateWithBottomLength(double plateLength_Bottom)
        {
            Length_Bottom = plateLength_Bottom;
            plateBottomEndPoint.X = plateBottomStartPoint.X + Length_Bottom;
        }


        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }

    }



    public class APlateInfo
    {
        public APlateInfo()
        {

        }
        
    }
}
