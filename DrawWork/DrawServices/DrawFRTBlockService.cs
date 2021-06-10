using AssemblyLib.AssemblyModels;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
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

            double upperHeight = 850;
            double lowerHeight = 1250;
            double middleHeight = 1200;
            double middleUpperHeight = 750;
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

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
    }
}
