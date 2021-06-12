using AssemblyLib.AssemblyModels;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawWork.CutomModels;
using DrawWork.DrawModels;
using DrawWork.DrawStyleServices;
using DrawWork.ValueServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawServices
{
    public class DrawFRTBlockService
    {

        private ValueService valueService;
        //private DrawWorkingPointService workingPointService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;
        private DrawEditingService editingService;

        private DrawImportBlockService blockImportService;

        private DrawShapeServices shapeService;

        public DrawFRTBlockService()
        {

            valueService = new ValueService();
            //workingPointService = new DrawWorkingPointService(selAssembly);
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();
            editingService = new DrawEditingService();
            shapeService = new DrawShapeServices();

        }

        public List<Entity> DrawFRT_DeckSupport(Point3D drawPoint,double selScaleValue)
        {
            List<Point3D> newOutPoint = new List<Point3D>();
            List<Entity> newList = new List<Entity>();

            double upperHeight = 850-200;
            double lowerHeight = 1250;
            double middleHeight = 1200+200;
            double middleUpperHeight = 750+200;
            double middleLowerHeight = 450;

            double middleDiameter = 114.3;  // 4 inch
            double upperDiameter = 88.9;    // 3 inch
            double lowerDiameter = 88.9;    // 3 inch

            Point3D middleBlockLeftTop = GetSumPoint(drawPoint, -middleDiameter / 2, middleUpperHeight);
            Point3D upperBlockLeftTop = GetSumPoint(drawPoint, -upperDiameter / 2, middleUpperHeight + upperHeight);
            Point3D lowerBlockLeftTop = GetSumPoint(drawPoint, -lowerDiameter / 2, -middleLowerHeight);

            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(middleBlockLeftTop, 0, 0), middleDiameter, middleHeight, 0, 0, 0));
            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(upperBlockLeftTop, 0, 0), upperDiameter, upperHeight, 0, 0, 0,new bool[] { true, true, false, true }));
            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(lowerBlockLeftTop, 0, 0), lowerDiameter, lowerHeight, 0, 0, 0, new bool[] { false, true, true, true }));

            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            // CenterLIne
            Point3D centerTop = GetSumPoint(drawPoint, 0, middleUpperHeight + upperHeight);
            Point3D centerBottom = GetSumPoint(drawPoint, 0,- middleLowerHeight - lowerHeight);
            List<Entity> centerLineList = new List<Entity>();
            DrawCenterLineModel newCenterModel = new DrawCenterLineModel();

            centerLineList.AddRange( editingService.GetCenterLine(centerTop, centerBottom, newCenterModel.exLength, selScaleValue));
            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            newList.AddRange(centerLineList);

            return newList;
        }

        public List<Entity> DrawFRT_RunWay(Point3D drawPoint, double selRunWayWidth)
        {
            List<Point3D> newOutPoint = new List<Point3D>();
            List<Entity> newList = new List<Entity>();

            double flatThickness = 130;
            double runWayWidth = selRunWayWidth;
            double runWayHeight = 350;


            Point3D leftBlockLeftTop = GetSumPoint(drawPoint, -runWayWidth / 2, runWayHeight);
            Point3D rightBlockLeftTop = GetSumPoint(drawPoint, +runWayWidth / 2 - flatThickness, runWayHeight);
            Point3D topBlockLeftTop = GetSumPoint(drawPoint, -runWayWidth / 2 + flatThickness, runWayHeight);

            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(leftBlockLeftTop, 0, 0), flatThickness, runWayHeight, 0, 0, 0));
            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(rightBlockLeftTop, 0, 0), flatThickness, runWayHeight, 0, 0, 0));
            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(topBlockLeftTop, 0, 0),runWayWidth-(flatThickness*2), flatThickness, 0, 0, 0, new bool[] { true, false, true, false}));

            styleService.SetLayerListEntity(ref newList, layerService.LayerVirtualLine);

            return newList;
        }

        public List<Entity> DrawFRT_RollingLadder(Point3D selStartPoint, Point3D selEndPoint,double selScaleValue)
        {
            List<Entity> newList = new List<Entity>();
            List<Entity> newCenterLineList = new List<Entity>();
            // 고정 값
            Point3D actualStartPoint = GetSumPoint(selStartPoint, 500, 834.8);
            Point3D actualEndPoint = GetSumPoint(selEndPoint, 0, 110);

            double ladderWidth = Point3D.Distance(actualStartPoint, actualEndPoint);
            Point3D startPoint = GetSumPoint(actualStartPoint, 0, 0);
            Point3D endPoint = GetSumPoint(startPoint, ladderWidth, 0);

            double ladderHeight = 830;
            double ladderMiddleHeight = 430;
            double pipeOD = 60.3;
            double pipeODHalf = pipeOD / 2;
            double flatWidth = 60;
            double flatWidthHalf = flatWidth / 2;
            double ladderWheelWidth = 220;
            double ladderWheelWidthHalf = ladderWheelWidth / 2;
            double ladderWheelCenterWidth = 50;
            double rValue = 150;

            double ladderCrossWidth = ladderWidth - pipeOD;
            double ladderNum = Math.Ceiling(ladderCrossWidth / 1769.7);
            double ladderOneWidth = ladderCrossWidth / ladderNum;
            double ladderOneWidthHalf = ladderOneWidth / 2;

            // Center Line
            Line leftCenterLine = new Line(GetSumPoint(startPoint, 0, 0), GetSumPoint(startPoint, 0, ladderHeight));
            Line rightCenterLine = new Line(GetSumPoint(endPoint, 0, 0), GetSumPoint(endPoint, 0, ladderHeight));
            Line topCenterLine = new Line(GetSumPoint(leftCenterLine.EndPoint, 0, 0), GetSumPoint(rightCenterLine.EndPoint, 0, 0));
            Line middleCenterLine = new Line(GetSumPoint(startPoint, pipeODHalf, ladderMiddleHeight), GetSumPoint(endPoint, -pipeODHalf, ladderMiddleHeight));
            Arc arcFillet01;
            Curve.Fillet(leftCenterLine, topCenterLine, rValue, false, false, true, true, out arcFillet01);
            Arc arcFillet02;
            Curve.Fillet(topCenterLine, rightCenterLine, rValue, true, false, true, true, out arcFillet02);
            newCenterLineList.AddRange(new Entity[] { arcFillet01, arcFillet02, leftCenterLine, rightCenterLine, topCenterLine, middleCenterLine});

            

            // Ladder Wheel : Center LIne
            Point3D wheelCenterTop = GetSumPoint(endPoint, 0, ladderWheelWidthHalf);
            Point3D wheelCenterBottom = GetSumPoint(endPoint, 0, -ladderWheelWidthHalf);
            Point3D wheelCenterLeft = GetSumPoint(endPoint, -ladderWheelWidthHalf,0);
            Point3D wheelCenterRight = GetSumPoint(endPoint, ladderWheelWidthHalf,0);
            DrawCenterLineModel newCenterModel = new DrawCenterLineModel();

            newCenterLineList.AddRange(editingService.GetCenterLine(GetSumPoint(wheelCenterTop,0,0), GetSumPoint(wheelCenterBottom,0,0), newCenterModel.exLength, selScaleValue));
            newCenterLineList.AddRange(editingService.GetCenterLine(GetSumPoint(wheelCenterLeft, 0, 0), GetSumPoint(wheelCenterRight, 0, 0), newCenterModel.exLength, selScaleValue));
            styleService.SetLayerListEntity(ref newCenterLineList, layerService.LayerCenterLine);

            // OuterLine
            Arc arcFilletInner01 = (Arc)arcFillet01.Offset(pipeODHalf, Vector3D.AxisZ); ;
            Arc arcFilletInner02 = (Arc)arcFillet02.Offset(pipeODHalf, Vector3D.AxisZ);
            Line leftInnerLine = new Line(GetSumPoint(startPoint, pipeODHalf, -flatWidthHalf), GetSumPoint(arcFilletInner01.StartPoint,0,0));
            Line rightInnerLine = new Line(GetSumPoint(endPoint, -pipeODHalf, -flatWidthHalf), GetSumPoint(arcFilletInner02.EndPoint,0,0));
            Line topInnerLine = new Line(GetSumPoint(arcFilletInner01.EndPoint, 0, 0), GetSumPoint(arcFilletInner02.StartPoint, 0, 0));
            newList.AddRange(new Entity[] { arcFilletInner01, arcFilletInner02, leftInnerLine, rightInnerLine, topInnerLine});

            Arc arcFilletOuter01 = (Arc)arcFillet01.Offset(-pipeODHalf, Vector3D.AxisZ); ;
            Arc arcFilletOuter02 = (Arc)arcFillet02.Offset(-pipeODHalf, Vector3D.AxisZ);
            Line leftOuterLine = new Line(GetSumPoint(startPoint, -pipeODHalf, -flatWidthHalf), GetSumPoint(arcFilletOuter01.StartPoint, 0, 0));
            Line rightOuterLine = new Line(GetSumPoint(endPoint, pipeODHalf, -flatWidthHalf), GetSumPoint(arcFilletOuter02.EndPoint, 0, 0));
            Line topOuterLine = new Line(GetSumPoint(arcFilletOuter01.EndPoint, 0, 0), GetSumPoint(arcFilletOuter02.StartPoint, 0, 0));
            newList.AddRange(new Entity[] { arcFilletOuter01, arcFilletOuter02, leftOuterLine, rightOuterLine, topOuterLine });

            Line middleOuterUpperLine = new Line(GetSumPoint(startPoint, pipeODHalf, ladderMiddleHeight + pipeODHalf), GetSumPoint(endPoint, -pipeODHalf, ladderMiddleHeight + pipeODHalf));
            Line middleOuterLowerLine = new Line(GetSumPoint(startPoint, pipeODHalf, ladderMiddleHeight - pipeODHalf), GetSumPoint(endPoint, -pipeODHalf, ladderMiddleHeight - pipeODHalf));
            Line bottomOuterUpperLine = new Line(GetSumPoint(startPoint, pipeODHalf, pipeODHalf), GetSumPoint(endPoint, -pipeODHalf, pipeODHalf));
            Line bottomOuterLowerLine = new Line(GetSumPoint(startPoint, -pipeODHalf,  - pipeODHalf), GetSumPoint(endPoint, pipeODHalf, - pipeODHalf));
            newList.AddRange(new Entity[] { middleOuterUpperLine, middleOuterLowerLine, bottomOuterUpperLine , bottomOuterLowerLine });


            // Outer Line Cross
            Point3D crossStartPoint = GetSumPoint(startPoint, pipeODHalf, flatWidthHalf);
            Point3D crossMidPoint = GetSumPoint(startPoint, 0, ladderHeight-pipeODHalf);
            //Point3D crossEndPoint = GetSumPoint(startPoint,- pipeODHalf, flatWidthHalf);
            Line refBottom= new Line(GetSumPoint(startPoint, -pipeODHalf*10, pipeODHalf), GetSumPoint(endPoint, pipeODHalf*10, pipeODHalf));
            double currentCrossPoint =0;
            for(int i=0;i< ladderNum; i++)
            {
                Point3D eachStart = GetSumPoint(crossStartPoint, currentCrossPoint, 0);
                Point3D eachMid = GetSumPoint(crossMidPoint, currentCrossPoint + ladderOneWidthHalf, 0);
                Point3D eachEnd = GetSumPoint(crossStartPoint, currentCrossPoint + ladderOneWidth, 0);

                Circle startCircle = new Circle(GetSumPoint(eachStart, 0, 0), flatWidth);
                Circle midCircle = new Circle(GetSumPoint(eachMid, 0, 0), flatWidth);
                Circle endCircle = new Circle(GetSumPoint(eachEnd, 0, 0), flatWidth);
                Line[] startLine = UtilityEx.GetLinesTangentToCircleFromPoint(startCircle, eachMid);
                Line[] midLine = UtilityEx.GetLinesTangentToCircleFromPoint(midCircle, eachStart);
                Line[] midLine2 = UtilityEx.GetLinesTangentToCircleFromPoint(midCircle, eachEnd);
                Line[] endLine = UtilityEx.GetLinesTangentToCircleFromPoint(endCircle, eachMid);

                Point3D[] startInter = refBottom.IntersectWith(startLine[1]);
                Point3D[] midInter = topInnerLine.IntersectWith(midLine[1]);
                Point3D[] midInter2 = topInnerLine.IntersectWith(midLine2[0]);
                Point3D[] endInter = refBottom.IntersectWith(endLine[0]);

                newList.Add(new Line(GetSumPoint(eachStart, 0, 0), GetSumPoint(midInter[0], 0, 0)));
                newList.Add(new Line(GetSumPoint(startInter[0], 0, 0), GetSumPoint(eachMid, 0, 0)));
                newList.Add(new Line(GetSumPoint(endInter[0], 0, 0), GetSumPoint(eachMid, 0, 0)));
                newList.Add(new Line(GetSumPoint(eachEnd, 0, 0), GetSumPoint(midInter2[0], 0, 0)));

                currentCrossPoint += ladderOneWidth;
            }

            // Ladder Wheel : OuterLine
            Circle wheelCircleOuter = new Circle(GetSumPoint(endPoint, 0, 0), ladderWheelWidthHalf);
            Circle wheelCircleInner = new Circle(GetSumPoint(endPoint, 0, 0), ladderWheelCenterWidth/2);
            newList.Add(wheelCircleOuter);
            newList.Add(wheelCircleInner);


            styleService.SetLayerListEntity(ref newList, layerService.LayerVirtualLine);

            newList.AddRange(newCenterLineList);


            // Rotate

            Line angleLine = new Line(GetSumPoint(actualStartPoint, 0, 0), GetSumPoint(actualEndPoint, 0, 0));
            double rotateValue= editingService.GetAngleOfLine(angleLine);
            Point3D rotatePoint = GetSumPoint(actualStartPoint, 0, 0);
            foreach (Entity eachEntity in newList)
                eachEntity.Rotate(rotateValue, Vector3D.AxisZ, rotatePoint);

            return newList;
        }



        // Nozzle
        public List<Entity> DrawFRT_Nozzle_AutoBleederVent(Point3D refPoint,  double selScaleValue)
        {
            DrawFRTBlockService drawFRT = new DrawFRTBlockService();

            Point3D referencePoint = GetSumPoint(refPoint,  0, 0);

            List<Point3D> newOutPoint = new List<Point3D>();
            List<Entity> newList = new List<Entity>();
            List<Entity> newCenterList = new List<Entity>();

            //newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(EBottomLeftPoint, 0, 0), pontoonRightFlatWidth, pontoonThkE, 0, 0, 3));
            //newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(FTopLeftPoint, 0, 0), pontoonFlatLength, pontoonThkF, 0, 0, 0, new bool[] { true, false, true, true }));

            double bottomGap = 30;



            double topPadHeight = 10;
            double topPadWidth = 100 + 19 + 20;

            double padUpHeight = 115;
            double padUpWidth = 100;

            double padUpUpHeight = 1600 - 620 - 110 - 115;
            double padUpUpWidth = padUpWidth - (20 * 2);

            double middleUpHeight = 110;
            double middleUpWidth = 330;

            double middleUpPadHeight = 10;
            double middleUpPadWidth = middleUpWidth - (10 * 2);

            double middleHeight = 650;
            double middleWidth = middleUpWidth - (30 * 2);

            double bottomHeight = 1600 - bottomGap;
            double bottomWidth = 73;

            Point3D refAdjPoint = GetSumPoint(referencePoint, 0, -bottomGap);
            double currentY = -bottomHeight;
            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(refAdjPoint, -bottomWidth / 2, currentY), bottomWidth, bottomHeight, 0, 0, 3, new bool[] { false, true, true, true }));
            currentY += bottomHeight;
            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(refAdjPoint, -middleWidth / 2, currentY), middleWidth, middleHeight, 0, 0, 3, new bool[] { false, true, true, true }));
            currentY += middleHeight;
            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(refAdjPoint, -middleUpWidth / 2, currentY), middleUpWidth, middleUpHeight, 0, 0, 3, new bool[] { true, true, true, true }));
            currentY += middleUpHeight;
            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(refAdjPoint, -middleUpPadWidth / 2, currentY), middleUpPadWidth, middleUpPadHeight, 0, 0, 3, new bool[] { true, true, false, true }));
            currentY += middleUpPadHeight;
            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(refAdjPoint, -padUpWidth / 2, currentY), padUpWidth, padUpHeight, 0, 0, 3, new bool[] { true, true, false, true }));
            currentY += padUpHeight;
            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(refAdjPoint, -padUpUpWidth / 2, currentY), padUpUpWidth, padUpUpHeight, 0, 0, 3, new bool[] { false, true, false, true }));
            currentY += padUpUpHeight;
            newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(refAdjPoint, -topPadWidth / 2, currentY), topPadWidth, topPadHeight, 0, 0, 3, new bool[] { true, true, true, true }));
            currentY += topPadHeight;




            double topHeight = 90;
            double topRadius = 50;
            double topWidth = 100;
            double topPipeWidth = 19;
            Arc topCenterArc = new Arc(Plane.XY, GetSumPoint(refAdjPoint, -topWidth / 2, currentY + topHeight), GetSumPoint(refAdjPoint, 0, currentY + topHeight + topRadius), GetSumPoint(refAdjPoint, topWidth / 2, currentY + topHeight), true);
            Line topLeftLine = new Line(GetSumPoint(refAdjPoint, -topWidth / 2, currentY), GetSumPoint(refAdjPoint, -topWidth / 2, currentY + topHeight));
            Line topRightLine = new Line(GetSumPoint(refAdjPoint, topWidth / 2, currentY), GetSumPoint(refAdjPoint, topWidth / 2, currentY + topHeight));
            newCenterList.AddRange(new Entity[] { topCenterArc, topLeftLine, topRightLine });

            Arc topCenterArcInner = (Arc)topCenterArc.Offset(-topPipeWidth / 2, Vector3D.AxisZ);
            Line topLeftLineInner = (Line)topLeftLine.Offset(topPipeWidth / 2, Vector3D.AxisZ);
            Line topRightInnertLine = (Line)topRightLine.Offset(-topPipeWidth / 2, Vector3D.AxisZ);
            Arc topCenterArcOuter = (Arc)topCenterArc.Offset(topPipeWidth / 2, Vector3D.AxisZ);
            Line topLeftLineOuter = (Line)topLeftLine.Offset(-topPipeWidth / 2, Vector3D.AxisZ);
            Line topRightInnertOuter = (Line)topRightLine.Offset(+topPipeWidth / 2, Vector3D.AxisZ);
            newList.AddRange(new Entity[] { topCenterArcInner, topLeftLineInner, topRightInnertLine, topCenterArcOuter, topLeftLineOuter, topRightInnertOuter });
            // CenterLine
            DrawCenterLineModel newCenterModel = new DrawCenterLineModel();
            List<Entity> centerLongLine = editingService.GetCenterLine(GetSumPoint(refAdjPoint, 0, -bottomHeight), GetSumPoint(refAdjPoint, 0, currentY + topHeight + topRadius + (topPipeWidth / 2)), newCenterModel.exLength, selScaleValue);
            List<Entity> centerLongLine2 = editingService.GetCenterLine(GetSumPoint(refAdjPoint, -(topWidth / 2) - (topPipeWidth / 2), currentY + topHeight), GetSumPoint(refAdjPoint, +(topWidth / 2) + (topPipeWidth / 2), currentY + topHeight), newCenterModel.exLength, selScaleValue);
            newCenterList.AddRange(centerLongLine);
            newCenterList.AddRange(centerLongLine2);

            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref newCenterList, layerService.LayerCenterLine);


            newList.AddRange(newCenterList);
            return newList;
        }

        public List<Entity> DrawFRT_Nozzle_RimVent(Point3D topPoint,Point3D leftPoint, RimVentNozzleModel selRimVent,NozzleRoofInputModel selNozzle,double selTopSlope, double selScaleValue,ref AssemblyModel selAssemblyData)
        {
            DrawNozzleBlockService nozzleBlock=new DrawNozzleBlockService(selAssemblyData);

            List<Entity> customEntity = new List<Entity>();
            // Only : NPS : 2 3 4 6 8

            double rimA = valueService.GetDoubleValue(selRimVent.A);
            double rimC = valueService.GetDoubleValue(selRimVent.C);
            Point3D drawStartPoint = GetSumPoint(topPoint, 0, rimC);

            FlangeOHFModel drawNozzle = null;
            if (selNozzle.Rating.Contains("150"))
            {
                foreach (FlangeOHFModel eachNozzle in selAssemblyData.FlangeOHFList)
                    if (eachNozzle.NPS == selNozzle.Size)
                    {
                        drawNozzle = eachNozzle;
                        break;
                    }
            }
            else if (selNozzle.Rating.Contains("300"))
            {
                foreach (FlangeTHModel eachNozzle in selAssemblyData.FlangeTHList)
                    if (eachNozzle.NPS == selNozzle.Size)
                    {
                        drawNozzle = TransNozzleTHtoOHF(eachNozzle);
                        break;
                    }
            }

            if (drawNozzle != null)
            {
                double G = valueService.GetDoubleValue(drawNozzle.G);
                double OD = valueService.GetDoubleValue(drawNozzle.OD);
                double BCD = valueService.GetDoubleValue(drawNozzle.BCD);
                double R = 0;
                double RRF = valueService.GetDoubleValue(drawNozzle.RRF);
                double RFF = valueService.GetDoubleValue(drawNozzle.RFF);
                double H = valueService.GetDoubleValue(drawNozzle.H);
                double A = 0;
                double AWN = valueService.GetDoubleValue(drawNozzle.AWN);
                double ASO = valueService.GetDoubleValue(drawNozzle.ASO);
                double B = valueService.GetDoubleValue(drawNozzle.B);
                double C = valueService.GetDoubleValue(drawNozzle.C);
                double facingC = 0;

                double DN = valueService.GetDoubleValue(drawNozzle.DN);

                bool facingAdd = false;
                if (selNozzle.Facing.Contains("ff"))
                {
                    R = RFF;
                    facingC = 0;
                }
                else
                {
                    facingAdd = true;
                    facingC = C;
                    R = RRF;
                }
                if (selNozzle.Type.Contains("wn"))
                {
                    A = AWN;
                }
                else
                {
                    A = ASO;
                }



                List<Point3D> currentPoint = new List<Point3D>();
                currentPoint.Add(GetSumPoint(drawStartPoint, 0, 0));

                // Facing
                if (facingAdd)
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Pipe(out currentPoint, currentPoint[0], 1, R, facingC, true, true, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));

                // Flange
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out currentPoint, currentPoint[0], 1, BCD, OD, B - facingC, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = !facingAdd, twoEx = true }));
                // Neck
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Neck(out currentPoint, currentPoint[0], 1, G, H, A - B, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));

                // Pad
                double padThickness = 5; //Pontoon B 고정
                List<Point3D> padOutputPoint = new List<Point3D>();
                Point3D padPoint = GetSumPoint(topPoint, 0, 0);
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PadSlope(out padOutputPoint, GetSumPoint(padPoint, 0,valueService.GetHypotenuseByWidth(selTopSlope, padThickness)), 0, G, G*2, padThickness, true, -selTopSlope, new DrawCenterLineModel() { scaleValue = selScaleValue}));

                ElbowModel newElbow = GetElbow(selNozzle, ref selAssemblyData);
                List<Point3D> outputPointList = new List<Point3D>();
                if (newElbow != null)
                {
                    double elbowWidth = 0;
                    if (selRimVent.ELBOW.ToLower().Contains("sr"))
                        elbowWidth = valueService.GetDoubleValue( newElbow.SRD);
                    else
                        elbowWidth = valueService.GetDoubleValue( newElbow.LRA);

                    double pipeLeftWidth = topPoint.X-leftPoint.X - elbowWidth;
                    Point3D bottomPipeRightPoint = GetSumPoint(leftPoint, pipeLeftWidth, 0);

                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out outputPointList, bottomPipeRightPoint, 1, G, pipeLeftWidth,0, 0, Utility.DegToRad(-90), new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));

                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Elbow(out outputPointList, bottomPipeRightPoint, 1, newElbow, "sr", "90", Utility.DegToRad(180), new DrawCenterLineModel() { scaleValue = selScaleValue }));

                    // Vertical Pipe
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out outputPointList, currentPoint[0], 1, G, Point3D.Distance(outputPointList[0], currentPoint[0]), 0, 0, 0, new DrawCenterLineModel() { scaleValue = selScaleValue}));
                }

            }

            return customEntity;
        }

        public List<Entity> DrawFRT_Nozzle_RoofDrainSump(Point3D refPoint, Point3D selLowerPoint, Point3D selUpperPoint,  NozzleRoofInputModel selNozzle, double selTopSlope, double selScaleValue, ref AssemblyModel selAssemblyData)
        {
            DrawNozzleBlockService nozzleBlock = new DrawNozzleBlockService(selAssemblyData);

            List<Entity> customEntity = new List<Entity>();
            // Only : NPS : 2 3 4 6 8

            double sumpID = 810;
            double sumpOD = 860;
            double sumpThk = 10;
            double sumpBottomHeight = 150;
            double sumpMidHeight = 350;
            double sumpOutWidth = 235;

            double nozzleR = valueService.GetDoubleValue(selNozzle.R);

            Point3D drawStartPoint = GetSumPoint(selLowerPoint, -sumpOD/2 - sumpOutWidth, -sumpMidHeight);

            FlangeOHFModel drawNozzle = null;
            if (selNozzle.Rating.Contains("150"))
            {
                foreach (FlangeOHFModel eachNozzle in selAssemblyData.FlangeOHFList)
                    if (eachNozzle.NPS == selNozzle.Size)
                    {
                        drawNozzle = eachNozzle;
                        break;
                    }
            }
            else if (selNozzle.Rating.Contains("300"))
            {
                foreach (FlangeTHModel eachNozzle in selAssemblyData.FlangeTHList)
                    if (eachNozzle.NPS == selNozzle.Size)
                    {
                        drawNozzle = TransNozzleTHtoOHF(eachNozzle);
                        break;
                    }
            }

            if (drawNozzle != null)
            {
                double G = valueService.GetDoubleValue(drawNozzle.G);
                double OD = valueService.GetDoubleValue(drawNozzle.OD);
                double BCD = valueService.GetDoubleValue(drawNozzle.BCD);
                double R = 0;
                double RRF = valueService.GetDoubleValue(drawNozzle.RRF);
                double RFF = valueService.GetDoubleValue(drawNozzle.RFF);
                double H = valueService.GetDoubleValue(drawNozzle.H);
                double A = 0;
                double AWN = valueService.GetDoubleValue(drawNozzle.AWN);
                double ASO = valueService.GetDoubleValue(drawNozzle.ASO);
                double B = valueService.GetDoubleValue(drawNozzle.B);
                double C = valueService.GetDoubleValue(drawNozzle.C);
                double facingC = 0;

                double DN = valueService.GetDoubleValue(drawNozzle.DN);

                bool facingAdd = false;
                if (selNozzle.Facing.Contains("ff"))
                {
                    R = RFF;
                    facingC = 0;
                }
                else
                {
                    facingAdd = true;
                    facingC = C;
                    R = RRF;
                }

                if (selNozzle.Type.Contains("wn"))
                {
                    A = AWN;
                }
                else
                {
                    A = ASO;
                }

                // Only : S.O
                A = ASO;


                double leftRotate = Utility.DegToRad(90);

                List<Point3D> currentPoint = new List<Point3D>();
                currentPoint.Add(GetSumPoint(drawStartPoint, 0, 0));

                List<Entity> mirrorList = new List<Entity>();
                // Facing
                if (facingAdd)
                    mirrorList.AddRange(nozzleBlock.DrawReference_Nozzle_Pipe(out currentPoint, currentPoint[0], 1, R, facingC, true, true, leftRotate, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));

                // Flange
                mirrorList.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out currentPoint, currentPoint[0], 1, BCD, OD, B - facingC, leftRotate, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = !facingAdd, twoEx = true }));
                // Neck
                mirrorList.AddRange(nozzleBlock.DrawReference_Nozzle_Neck(out currentPoint, currentPoint[0], 1, G, H, A - B, leftRotate, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));


                double leftPipeWidth = 398;
                Point3D pipeLeftStartPoint = GetSumPoint(currentPoint[0], 0, 0);
                double flangeLength = Point3D.Distance(drawStartPoint, pipeLeftStartPoint);

                // Left : Mirror
                List<Point3D> outputPointList = new List<Point3D>();
                Plane mirrorLeftPlane = Plane.YZ;
                mirrorLeftPlane.Origin.X = drawStartPoint.X;
                mirrorLeftPlane.Origin.Y = drawStartPoint.Y;
                List<Entity> leftFlange = editingService.GetEntityByMirror(mirrorLeftPlane, mirrorList);

                Point3D leftPipeStart = GetSumPoint(drawStartPoint, -flangeLength, 0);
                double leftVirtualPipeWidth = 200;
                leftFlange.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out outputPointList, leftPipeStart, 1, G, leftVirtualPipeWidth- flangeLength, 0, 0, -leftRotate, new DrawCenterLineModel() { scaleValue = selScaleValue}));
                styleService.SetLayerListEntityExcludingCenterLine(ref leftFlange, layerService.LayerVirtualLine);
                Point3D LeftStartLinePoint1 = GetSumPoint(outputPointList[0], 0, 0);

                // Right : Mirror
                Plane mirrorRightPlane = Plane.YZ;
                mirrorRightPlane.Origin.X = drawStartPoint.X + leftPipeWidth/2;
                mirrorRightPlane.Origin.Y = drawStartPoint.Y;
                List<Entity> rightFlange = editingService.GetEntityByMirror(mirrorRightPlane, mirrorList);

                customEntity.AddRange(mirrorList);
                // Right : Pipe

                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out outputPointList, pipeLeftStartPoint, 1, G, leftPipeWidth - (flangeLength * 2), 0, 0, leftRotate, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));


                // Right : Mirror
                Point3D pipeRightStartPoint = GetSumPoint(drawStartPoint, leftPipeWidth, 0);
                List<Entity> rightFlangeList = nozzleBlock.DrawReference_Nozzle_Flange(out outputPointList, pipeRightStartPoint, 1, BCD, OD, B - facingC, leftRotate, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true, twoEx = true });
                styleService.SetLayerListEntityExcludingCenterLine(ref rightFlangeList, layerService.LayerVirtualLine);
                Point3D pipeRightA = GetSumPoint(pipeRightStartPoint, 0, G / 2);
                Point3D pipeRightB = GetSumPoint(pipeRightStartPoint, 0, -G / 2);

                // Upper : Mirror
                Point3D pipeRightEndPoint = GetSumPoint(pipeRightStartPoint, 252, 220);
                List<Entity> topFlangeList = nozzleBlock.DrawReference_Nozzle_Flange(out outputPointList, pipeRightEndPoint, 1, BCD, OD, B - facingC, 0, new DrawCenterLineModel() { scaleValue = selScaleValue,zeroEx=true, oneEx = true, twoEx = true });
                styleService.SetLayerListEntityExcludingCenterLine(ref topFlangeList, layerService.LayerVirtualLine);
                Point3D pipeRightAA = GetSumPoint(pipeRightEndPoint, -G / 1.3, -(B - facingC));
                Point3D pipeRightBB = GetSumPoint(pipeRightEndPoint, G / 1.3, -(B - facingC));

                Line pipeRighLineA = new Line(GetSumPoint(pipeRightA, 0, 0), new Point3D(pipeRightAA.X, pipeRightA.Y));
                Line pipeRighLineAA = new Line(GetSumPoint(pipeRightAA, 0, 0), new Point3D(pipeRightAA.X, pipeRightA.Y));
                Line pipeRighLineB = new Line(GetSumPoint(pipeRightB, 0, 0), new Point3D(pipeRightBB.X, pipeRightB.Y));
                Line pipeRighLineBB = new Line(GetSumPoint(pipeRightBB, 0, 0), new Point3D(pipeRightBB.X, pipeRightB.Y));

                double filletR = 60;
                Arc arcFillet01;
                Curve.Fillet(pipeRighLineA, pipeRighLineAA, filletR*2.3, true, false, true, true, out arcFillet01);
                Arc arcFillet02;
                Curve.Fillet(pipeRighLineB, pipeRighLineBB, filletR*2, true, false, true, true, out arcFillet02);

                List<Entity> newShape = new List<Entity>();
                newShape.AddRange(new Entity[] { pipeRighLineA, pipeRighLineAA, pipeRighLineB, pipeRighLineBB, arcFillet01, arcFillet02 });
                styleService.SetLayerListEntityExcludingCenterLine(ref newShape, layerService.LayerVirtualLine);

                customEntity.AddRange(newShape);


                customEntity.AddRange(leftFlange);
                customEntity.AddRange(rightFlange);
                customEntity.AddRange(rightFlangeList);
                customEntity.AddRange(topFlangeList);


                // Sump : Box
                double sumpRealHeight = sumpMidHeight + G + sumpBottomHeight;
                List<Entity> leftSumpList = new List<Entity>();
                Point3D leftTopPoint1 = GetSumPoint(selLowerPoint,-sumpID / 2 - sumpThk,0);
                Point3D leftTopPoint2 = GetSumPoint(leftTopPoint1, 0, -sumpMidHeight + G / 2);
                Point3D leftTopPoint3 = GetSumPoint(leftTopPoint1, 0, -sumpMidHeight - G / 2);
                Point3D leftTopPoint4 = GetSumPoint(leftTopPoint1, 0, -sumpMidHeight - G  - sumpBottomHeight);
                Point3D leftTopBototmPoint1 = GetSumPoint(selLowerPoint, -sumpOD / 2 , -sumpRealHeight);
                Point3D leftTopBototmPoint2 = GetSumPoint(selLowerPoint, 0, -sumpRealHeight);

                leftSumpList.Add(new Line(GetSumPoint(leftTopPoint1, 0, 0), GetSumPoint(leftTopPoint2, 0, 0)));
                leftSumpList.Add(new Line(GetSumPoint(leftTopPoint1, sumpThk, 0), GetSumPoint(leftTopPoint2, sumpThk, 0)));
                leftSumpList.Add(new Line(GetSumPoint(leftTopPoint3, 0, 0), GetSumPoint(leftTopPoint4, 0, 0)));
                leftSumpList.Add(new Line(GetSumPoint(leftTopPoint3, sumpThk, 0), GetSumPoint(leftTopPoint4, sumpThk, 0)));

                leftSumpList.Add(new Line(GetSumPoint(leftTopBototmPoint1, 0, 0), GetSumPoint(leftTopBototmPoint2, 0, 0)));
                leftSumpList.Add(new Line(GetSumPoint(leftTopBototmPoint1, 0, -sumpThk), GetSumPoint(leftTopBototmPoint2, 0, -sumpThk)));
                leftSumpList.Add(new Line(GetSumPoint(leftTopBototmPoint1, 0, 0), GetSumPoint(leftTopBototmPoint1, 0, -sumpThk)));


                List<Entity> rightSumpList = new List<Entity>();
                Point3D rightTopPoint1 = GetSumPoint(selLowerPoint, +sumpID / 2 , 0);
                Point3D rightTopPoint2 = GetSumPoint(selLowerPoint, +sumpID / 2, -sumpRealHeight);
                Point3D rightTopBototmPoint1 = GetSumPoint(selLowerPoint, +sumpOD / 2, -sumpRealHeight);
                Point3D rightTopBototmPoint2 = GetSumPoint(selLowerPoint, 0, -sumpRealHeight);

                rightSumpList.Add(new Line(GetSumPoint(rightTopPoint1, 0, 0), GetSumPoint(rightTopPoint2, 0, 0)));
                rightSumpList.Add(new Line(GetSumPoint(rightTopPoint1, sumpThk, 0), GetSumPoint(rightTopPoint2, sumpThk, 0)));

                rightSumpList.Add(new Line(GetSumPoint(rightTopBototmPoint1, 0, 0), GetSumPoint(rightTopBototmPoint2, 0, 0)));
                rightSumpList.Add(new Line(GetSumPoint(rightTopBototmPoint1, 0, -sumpThk), GetSumPoint(rightTopBototmPoint2, 0, -sumpThk)));
                rightSumpList.Add(new Line(GetSumPoint(rightTopBototmPoint1, 0, 0), GetSumPoint(rightTopBototmPoint1, 0, -sumpThk)));



                // Upper Flat : Double Deck
                if (selUpperPoint != null)
                {
                    double tankWidth = valueService.GetDoubleValue(selAssemblyData.GeneralDesignData[0].SizeNominalID);
                    Point3D midXPoint = GetSumPoint(selUpperPoint, nozzleR, 0);
                    Line midVerticalLine = new Line(new Point3D(midXPoint.X, refPoint.Y), new Point3D(midXPoint.X, selUpperPoint.Y + tankWidth));
                    Line leftSlopeLine = new Line(GetSumPoint(selUpperPoint, -tankWidth, 0), GetSumPoint(selUpperPoint, tankWidth, 0));
                    leftSlopeLine.Rotate(-selTopSlope, Vector3D.AxisZ,selUpperPoint);
                    Point3D midInter = editingService.GetIntersectWidth(midVerticalLine, leftSlopeLine, 0);
                    leftSlopeLine.TrimBy(midInter, false);
                    Plane mirrorPlane = Plane.YZ;
                    mirrorPlane.Origin.X = midXPoint.X;
                    mirrorPlane.Origin.Y = midXPoint.Y;
                    Line rightSlopeLine = (Line)leftSlopeLine.Clone();
                    rightSlopeLine.TransformBy(new Mirror(mirrorPlane));

                    Point3D leftInter = editingService.GetIntersectWidth(leftSlopeLine, new Line(GetSumPoint(leftTopPoint1, 0, 0), GetSumPoint(leftTopPoint1, 0, tankWidth)), 0);
                    Point3D rightInter = new Point3D();
                    Point3D rightInter1 = editingService.GetIntersectWidth(leftSlopeLine, new Line(GetSumPoint(rightTopPoint1, 0, 0), GetSumPoint(rightTopPoint1, 0, tankWidth)), 0);
                    Point3D rightInter2 = editingService.GetIntersectWidth(rightSlopeLine, new Line(GetSumPoint(rightTopPoint1, 0, 0), GetSumPoint(rightTopPoint1, 0, tankWidth)), 0);
                    if (rightInter1.Y > 0)
                        rightInter = rightInter1;
                    else
                        rightInter = rightInter2;

                    //leftSumpList.Add(leftSlopeLine);
                    //leftSumpList.Add(rightSlopeLine);

                    leftSumpList.Add(new Line(GetSumPoint(leftTopPoint1, 0, 0), GetSumPoint(leftInter, 0, 0)));
                    leftSumpList.Add(new Line(GetSumPoint(leftTopPoint1, sumpThk, 0), GetSumPoint(leftInter, sumpThk, 0)));

                    rightSumpList.Add(new Line(GetSumPoint(rightTopPoint1, 0, 0), GetSumPoint(rightInter, 0, 0)));
                    rightSumpList.Add(new Line(GetSumPoint(rightTopPoint1, sumpThk, 0), GetSumPoint(rightInter, sumpThk, 0)));
                }

                styleService.SetLayerListEntity(ref leftSumpList, layerService.LayerOutLine);
                styleService.SetLayerListEntity(ref rightSumpList, layerService.LayerOutLine);

                // 오른쪽 노즐 처리
                if (nozzleR == 0)
                    styleService.SetLayerListEntityExcludingCenterLine(ref rightSumpList, layerService.LayerHiddenLine);

                customEntity.AddRange(leftSumpList);
                customEntity.AddRange(rightSumpList);



                // 기타 : 왼쪽 노즐
                InOutInternalPipe newInternal = null;
                string pipeSize = selNozzle.Size;
                foreach (InOutInternalPipe eachPipe in selAssemblyData.InOutInternalPipeList)
                {
                    if (pipeSize == eachPipe.NPS)
                    {
                        newInternal = eachPipe;
                        break;
                    }
                }
                // Only Shell
                double jointGap = valueService.GetDoubleValue(newInternal.A); // A = Shell

                List<Entity> hoseList = new List<Entity>();
                double bottomThk = 0;
                if (selAssemblyData.BottomInput[0].AnnularPlate.ToLower() == "yes")
                    bottomThk = valueService.GetDoubleValue(selAssemblyData.BottomInput[0].AnnularPlateThickness);
                else
                    bottomThk = valueService.GetDoubleValue(selAssemblyData.BottomInput[0].BottomPlateThickness);

                double selNozzleH = valueService.GetDoubleValue(selNozzle.H);

                Point3D leftBottomNozzlePoint = GetSumPoint(refPoint, jointGap, selNozzleH - bottomThk);

                double hoseHeight =drawStartPoint.Y- leftBottomNozzlePoint.Y;
                double hoseHeightThreeOne = hoseHeight / 3;
                double hoseWidth = selLowerPoint.X - refPoint.X;

                Point3D LeftStartLinePoint2 = new Point3D(selLowerPoint.X - hoseWidth / 5, leftBottomNozzlePoint.Y + hoseHeightThreeOne * 2);
                Point3D LeftStartLinePoint3 = new Point3D(selLowerPoint.X - hoseWidth / 7, leftBottomNozzlePoint.Y + hoseHeightThreeOne );
                Point3D LeftStartLinePoint4 = new Point3D(selLowerPoint.X - hoseWidth / 2, leftBottomNozzlePoint.Y );
                Point3D LeftStartLinePointTwo = new Point3D(selLowerPoint.X - (hoseWidth * 2/ 3), leftBottomNozzlePoint.Y);

                hoseList.Add(new Line(GetSumPoint(LeftStartLinePoint1, 0, 0), GetSumPoint(LeftStartLinePoint2, 0, 0)));
                hoseList.Add(new Line(GetSumPoint(LeftStartLinePoint2, 0, 0), GetSumPoint(LeftStartLinePoint3, 0, 0)));
                hoseList.Add(new Line(GetSumPoint(LeftStartLinePoint3, 0, 0), GetSumPoint(LeftStartLinePoint4, 0, 0)));
                hoseList.Add(new Line(GetSumPoint(LeftStartLinePoint4, 0, 0), GetSumPoint(leftBottomNozzlePoint, 0, 0)));


                hoseList.Add(new Circle(LeftStartLinePoint2, G / 2));
                hoseList.Add(new Circle(LeftStartLinePoint3, G / 2));

                double twoThk = 10;
                hoseList.AddRange(shapeService.GetRectangle(out outputPointList, GetSumPoint(LeftStartLinePointTwo, -twoThk, 100 / 2), twoThk, G, 0, 0, 0));
                hoseList.AddRange(shapeService.GetRectangle(out outputPointList, GetSumPoint(LeftStartLinePointTwo, twoThk, 100 / 2), twoThk, G, 0, 0, 0));


                styleService.SetLayerListEntity(ref hoseList, layerService.LayerVirtualLine);

                customEntity.AddRange(hoseList);


                // Upper Flat : Double Deck
                if (selUpperPoint != null)
                {
                    double tankWidth = valueService.GetDoubleValue(selAssemblyData.GeneralDesignData[0].SizeNominalID);
                    Point3D midXPoint = GetSumPoint(selUpperPoint, nozzleR, 0);
                    Line midVerticalLine = new Line(new Point3D(midXPoint.X, refPoint.Y), new Point3D(midXPoint.X, selUpperPoint.Y));
                    Line leftSlopeLine = new Line(GetSumPoint(selUpperPoint, -tankWidth, 0), GetSumPoint(selUpperPoint, tankWidth, 0));
                    leftSlopeLine.Rotate(selTopSlope, Vector3D.AxisZ);
                    Point3D midInter = editingService.GetIntersectWidth(midVerticalLine, leftSlopeLine, 0);
                    leftSlopeLine.TrimBy(midInter, true);
                    Plane mirrorPlane = Plane.YZ;
                    mirrorPlane.Origin.X = midXPoint.X;
                    mirrorPlane.Origin.Y = midXPoint.Y;
                    Line rightSlopeLine = (Line)leftSlopeLine.Clone();
                    rightSlopeLine.TransformBy(new Mirror(mirrorPlane));

                    Point3D leftInter = editingService.GetIntersectWidth(leftSlopeLine,new Line(GetSumPoint(leftTopPoint1,0,0), GetSumPoint(leftTopPoint1, 0, tankWidth)), 0);
                    Point3D rightInter = new Point3D();
                    Point3D rightInter1 = editingService.GetIntersectWidth(leftSlopeLine, new Line(GetSumPoint(rightTopPoint1, 0, 0), GetSumPoint(rightTopPoint1, 0, tankWidth)), 0);
                    Point3D rightInter2 = editingService.GetIntersectWidth(rightSlopeLine, new Line(GetSumPoint(rightTopPoint1, 0, 0), GetSumPoint(rightTopPoint1, 0, tankWidth)), 0);
                    if (rightInter1.Y > 0)
                        rightInter = rightInter1;
                    else
                        rightInter = rightInter2;

                    leftSumpList.Add(new Line(GetSumPoint(leftTopPoint1, 0, 0), GetSumPoint(leftInter, 0, 0)));
                    leftSumpList.Add(new Line(GetSumPoint(leftTopPoint1, sumpThk, 0), GetSumPoint(leftInter, sumpThk, 0)));

                    rightSumpList.Add(new Line(GetSumPoint(rightTopPoint1, 0, 0), GetSumPoint(rightInter, 0, 0)));
                    rightSumpList.Add(new Line(GetSumPoint(rightTopPoint1, sumpThk, 0), GetSumPoint(rightInter, sumpThk, 0)));
                }


            }

            return customEntity;
        }

        // 임시 적용
        private FlangeOHFModel TransNozzleTHtoOHF(FlangeTHModel selNozzzle)
        {
            FlangeOHFModel newModel = new FlangeOHFModel();
            newModel.DN = selNozzzle.DN;
            newModel.NPS = selNozzzle.NPS;
            newModel.G = selNozzzle.G;
            newModel.OD = selNozzzle.OD;
            newModel.BCD = selNozzzle.BCD;
            newModel.RRF = selNozzzle.RRF;
            newModel.RFF = selNozzzle.RFF;
            newModel.H = selNozzzle.H;
            newModel.AWN = selNozzzle.AWN;
            newModel.ASO = selNozzzle.ASO;
            newModel.B = selNozzzle.B;
            newModel.C = selNozzzle.C;
            newModel.BoltNo = selNozzzle.BoltNo;
            newModel.BoltSize1 = selNozzzle.BoltSize1;
            newModel.BoltSize2 = selNozzzle.BoltSize2;
            newModel.BoltLength = selNozzzle.BoltLength;

            return newModel;
        }

        private ElbowModel GetElbow(NozzleRoofInputModel selNozzle,ref AssemblyModel selAssemblyData)
        {
            ElbowModel returnValue = null;
            string pipeSize = selNozzle.Size;
            foreach (ElbowModel eachPipe in selAssemblyData.ElbowList)
            {
                if (pipeSize == eachPipe.NPS)
                {
                    returnValue = eachPipe;
                    break;
                }
            }
            return returnValue;
        }


        // Moogong Calculation
        public double GetRollingLadderWidth(double tankID)
        {
            double returnValue = 2000;
            if (tankID >= 60000)
                returnValue = 6000;
            else if (tankID >= 50000)
                returnValue = 5000;
            else if (tankID >= 40000)
                returnValue = 4000;
            else if (tankID >= 30000)
                returnValue = 3000;

            return returnValue;
        }

        public double GetPontoonPositionHeight(double tankHeight)
        {
            // Tank Height * 1/5
            // Min : 2000
            double returnValue = tankHeight * 1 / 5;
            if (returnValue < 2000)
                returnValue = 2000;
            return returnValue;
        }


        // Seal
        public List<Entity> DrawFRT_SealMechanical(Point3D drawPoint)
        {
            List<Point3D> newOutPoint = new List<Point3D>();
            List<Entity> newList = new List<Entity>();

            double shellLeftDistance = 200;
            double AThickness = 10;// 고정
            double bottomHeight = 50;
            double bottomWidth = 25;
            double rightHeight = 155;

            Point3D sealPoint = GetSumPoint(drawPoint, 0, 0);

            Point3D cBottomLeftPoint = GetSumPoint(sealPoint, AThickness, -bottomHeight);
            Point3D cBottomRightPoint = GetSumPoint(cBottomLeftPoint, bottomWidth, 0);
            Point3D cTopRightPoint = GetSumPoint(cBottomRightPoint, 0, rightHeight);

            Point3D cTopCurveMidPoint = GetSumPoint(cTopRightPoint, -120 - bottomWidth - AThickness, 100);
            Point3D cTopCurveEndPoint = GetSumPoint(cTopCurveMidPoint, -80, 155);

            Line leftSheetLine = new Line(GetSumPoint(sealPoint, -shellLeftDistance, 0), GetSumPoint(sealPoint, -shellLeftDistance, 1000));

            // First Courve
            Line cBottomLine = new Line(GetSumPoint(cBottomLeftPoint, 0, 0), GetSumPoint(cBottomRightPoint, 0, 0));
            Line cRightLine = new Line(GetSumPoint(cBottomRightPoint, 0, 0), GetSumPoint(cTopRightPoint, 0, 0));

            Arc cUpperArc = new Arc(Plane.XY, cTopRightPoint, cTopCurveMidPoint, cTopCurveEndPoint, true);
            Arc cUpperArcFillet = null;
            Curve.Fillet(cUpperArc, cRightLine, 50, true, false, true, true, out cUpperArcFillet);

            Arc cLowerArc = (Arc)cUpperArc.Offset(bottomWidth, Vector3D.AxisZ);
            Point3D cLeftInter = editingService.GetIntersectWidth(leftSheetLine, cLowerArc, 0);
            cLowerArc.TrimBy(cLeftInter, true);
            Arc cLowerArcFillet = (Arc)cUpperArcFillet.Offset(bottomWidth, Vector3D.AxisZ);
            Line cLeftLine = new Line(GetSumPoint(cLowerArcFillet.EndPoint, 0, 0), GetSumPoint(cBottomLeftPoint, 0, 0));

            // Second Courve
            Arc cLowerLowerArc = new Arc(Plane.XY, GetSumPoint(sealPoint, 0, 0), GetSumPoint(sealPoint, -135, 65), GetSumPoint(sealPoint, -200, 196), true);
            newList.AddRange(new Entity[] { cBottomLine, cRightLine, cUpperArc, cUpperArcFillet, cLowerArc, cLowerArcFillet,  cLowerLowerArc, cLeftLine });


            Point3D lowerShapeWP = GetSumPoint(sealPoint, -shellLeftDistance, -150);
            Line uTopLine = new Line(GetSumPoint(lowerShapeWP, 0, 0), GetSumPoint(lowerShapeWP, 45, 0));

            Line uTopRightSlopeLine = new Line(GetSumPoint(uTopLine.EndPoint, 0, 0), GetSumPoint(uTopLine.EndPoint, 30, -30));
            Line uTopRightLine = new Line(GetSumPoint(uTopRightSlopeLine.EndPoint, 0, 0), GetSumPoint(uTopRightSlopeLine.EndPoint, 0, -70));
            Line uTopBottomLine = new Line(GetSumPoint(uTopRightLine.EndPoint, 0, 0), GetSumPoint(uTopRightLine.EndPoint, -75, 0));

            Point3D uTopRecPoint = GetSumPoint(uTopLine.EndPoint, 0, -48);
            List<Entity> uTopRec = shapeService.GetRectangle(out newOutPoint, GetSumPoint(uTopRecPoint, 0, 0), 25, 550, Utility.DegToRad(15), 1, 1);
            newList.AddRange(uTopRec);
            newList.AddRange(new Entity[] { uTopLine, uTopRightSlopeLine, uTopRightLine });

            // utopRec 겹침
            List<Point3D> uTopRecPointList = new List<Point3D>();
            foreach (Entity eachEntity in uTopRec)
            {
                Point3D eachInter = editingService.GetIntersectWidth(uTopBottomLine, (ICurve)eachEntity, 0);
                if (eachInter.Y > 0)
                    uTopRecPointList.Add(GetSumPoint(eachInter, 0, 0));
            }
            List<Point3D> uTopRecPointListSort = uTopRecPointList.OrderBy(x => x.X).ToList();
            Line uTopBottomLine1 = new Line(GetSumPoint(uTopBottomLine.EndPoint, 0, 0), GetSumPoint(uTopRecPointListSort[0], 0, 0));
            Line uTopBottomLine2 = new Line(GetSumPoint(uTopBottomLine.StartPoint, 0, 0), GetSumPoint(uTopRecPointListSort[1], 0, 0));
            newList.AddRange(new Entity[] { uTopBottomLine1, uTopBottomLine2 });

            // Bottom
            Point3D sealBottonPoint = GetSumPoint(sealPoint, -shellLeftDistance, -900 + 50);
            Line bBottomLine1 = new Line(GetSumPoint(sealBottonPoint, 0, 0), GetSumPoint(sealBottonPoint, shellLeftDistance, 0));
            Line bBottomLine2 = new Line(GetSumPoint(sealBottonPoint, 0, 50), GetSumPoint(sealBottonPoint, shellLeftDistance, 50));

            newList.AddRange(new Entity[] { bBottomLine1, bBottomLine2 });

            // uShape
            Point3D uBottomShape = GetSumPoint(sealBottonPoint, shellLeftDistance, 50 + 500);
            Line uBottomTop = new Line(GetSumPoint(uBottomShape, 0, 0), GetSumPoint(uBottomShape, -55, 0));
            Line uBottomSlope = new Line(GetSumPoint(uBottomTop.EndPoint, 0, 0), GetSumPoint(uBottomTop.EndPoint, -30, -30));

            Line bBottomLeft = new Line(GetSumPoint(uBottomSlope.EndPoint, 0, 0), GetSumPoint(uBottomSlope.EndPoint, 0, -1000));

            // uBottomRec
            Point3D uBottomRecPoint = GetSumPoint(uBottomTop.EndPoint, 0, -25);
            List<Entity> uBottomRec = shapeService.GetRectangle(out newOutPoint, GetSumPoint(uBottomRecPoint, 0, 0), 25, 310, -Utility.DegToRad(27), 0, 0);
            newList.AddRange(uBottomRec);
            newList.AddRange(new Entity[] { uBottomTop, uBottomSlope });

            Line bBottomRight = new Line(GetSumPoint(uBottomRecPoint, 0, -500 + 25), GetSumPoint(uBottomRecPoint, 0, 0));

            // uLeft 겹침
            // utopRec 겹침
            List<Point3D> uBottomRecPointList = new List<Point3D>();
            foreach (Entity eachEntity in uBottomRec)
            {
                Point3D eachInter = editingService.GetIntersectWidth(bBottomLeft, (ICurve)eachEntity, 0);
                if (eachInter.Y > 0)
                    uBottomRecPointList.Add(GetSumPoint(eachInter, 0, 0));
            }
            List<Point3D> uBottomRecPointListSort = uBottomRecPointList.OrderBy(x => x.Y).ToList();
            Line bBottomLeft1 = new Line(GetSumPoint(uBottomSlope.EndPoint, 0, 0), GetSumPoint(uBottomRecPointListSort[1], 0, 0));

            newList.AddRange(new Entity[] { bBottomLeft1 });

            // 맨마지막 겹침
            List<Point3D> uBottomRecPointList1 = new List<Point3D>();
            foreach (Entity eachEntity in uBottomRec)
            {
                Point3D eachInter = editingService.GetIntersectWidth(bBottomRight, (ICurve)eachEntity, 0);
                if (eachInter.Y > 0)
                    uBottomRecPointList1.Add(GetSumPoint(eachInter, 0, 0));
            }
            List<Point3D> uBottomRecPointListSort1 = uBottomRecPointList1.OrderBy(x => x.Y).ToList();


            List<Point3D> uBottomRecPointList2 = new List<Point3D>();
            foreach (Entity eachEntity in uTopRec)
            {
                Point3D eachInter = editingService.GetIntersectWidth(bBottomRight, (ICurve)eachEntity, 0);
                if (eachInter.Y > 0)
                    uBottomRecPointList2.Add(GetSumPoint(eachInter, 0, 0));
            }
            List<Point3D> uBottomRecPointListSort2 = uBottomRecPointList2.OrderBy(x => x.Y).ToList();


            Line bBottomRight1 = new Line(GetSumPoint(bBottomRight.StartPoint, 0, 0), GetSumPoint(uBottomRecPointListSort2[0], 0, 0));
            Line bBottomRight2 = new Line(GetSumPoint(uBottomRecPointListSort2[1], 0, 0), GetSumPoint(uBottomRecPointListSort1[0], 0, 0));
            newList.AddRange(new Entity[] { bBottomRight1, bBottomRight2 });


            styleService.SetLayerListEntity(ref newList, layerService.LayerVirtualLine);

            return newList;
        }

        public List<Entity> DrawFRT_SealSoft(Point3D drawPoint, double selScaleValue)
        {
            List<Point3D> newOutPoint = new List<Point3D>();
            List<Entity> newList = new List<Entity>();

            double shellLeftDistance = 200;

            Point3D sealPoint = GetSumPoint(drawPoint, 0, -100);


            Point3D cTopCurveStartPoint = GetSumPoint(sealPoint, 0, 0);
            Point3D cTopCurveMidPoint = GetSumPoint(cTopCurveStartPoint, -135, 65);
            Point3D cTopCurveEndPoint = GetSumPoint(cTopCurveMidPoint, -65, 131);

            Arc cUpperArc = new Arc(Plane.XY, cTopCurveStartPoint, cTopCurveMidPoint, cTopCurveEndPoint, true);


            Point3D cMidCur1 = GetSumPoint(sealPoint, -shellLeftDistance, -247);
            Point3D cMidCur2 = GetSumPoint(sealPoint, -shellLeftDistance / 2, -185);
            Point3D cMidCur3 = GetSumPoint(sealPoint, 0, -247);

            Arc cMidArc1 = new Arc(Plane.XY, cMidCur1, cMidCur2, cMidCur3, true);

            Point3D cMidCur4 = GetSumPoint(cMidCur1, 0, -306);
            Point3D cMidCur5 = GetSumPoint(cMidCur2, 0, -430);
            Point3D cMidCur6 = GetSumPoint(cMidCur3, 0, -306);

            Arc cMidArc2 = new Arc(Plane.XY, cMidCur4, cMidCur5, cMidCur6,false);

            newList.AddRange(new Entity[] { cUpperArc, cMidArc1, cMidArc2 });
            styleService.SetLayerListEntity(ref newList, layerService.LayerVirtualLine);


            // Vertical Line
            List<Entity> newVerticalList = new List<Entity>();
            double divNumber = 6;
            double divOneWidth = shellLeftDistance / divNumber;
            double divOneWidthHalf = divOneWidth / 2;
            double currentWidth = divOneWidthHalf;
            for (int i = 0; i < divNumber; i++)
            {
                Line refVerticalLine = new Line(GetSumPoint(cMidCur1, currentWidth, 500), GetSumPoint(cMidCur1, currentWidth, -1000));
                Point3D interTop = editingService.GetIntersectWidth(refVerticalLine, cMidArc1, 0);
                Point3D interBottom = editingService.GetIntersectWidth(refVerticalLine, cMidArc2, 0);
                newVerticalList.Add(new Line(GetSumPoint(interTop, 0, 0), GetSumPoint(interBottom, 0, 0)));
                currentWidth += divOneWidth;
            }
            styleService.SetLayerListEntity(ref newVerticalList, layerService.LayerBasicLine);
            newList.AddRange(newVerticalList);

            // Center Line
            DrawCenterLineModel newCenterLineModel = new DrawCenterLineModel();
            List<Entity> centerLine = editingService.GetCenterLine(cMidCur2, cMidCur5, newCenterLineModel.exLength, selScaleValue);
            styleService.SetLayerListEntity(ref centerLine, layerService.LayerCenterLine);
            newList.AddRange(centerLine);

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
    }
}
