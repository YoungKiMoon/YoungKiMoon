using AssemblyLib.AssemblyModels;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
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
    public class DrawDetailRoofBottomService
    {
        private Model singleModel;
        private AssemblyModel assemblyData;

        private DrawService drawService;


        private ValueService valueService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;

        private DrawEditingService editingService;

        private DrawShellCourses dsService;
        private DrawBreakSymbols shapeService;

        private DrawPublicFunctionService publicService;

        public DrawDetailRoofBottomService(AssemblyModel selAssembly, object selModel)
        {
            singleModel = selModel as Model;

            assemblyData = selAssembly;

            drawService = new DrawService(selAssembly);

            valueService = new ValueService();
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();

            editingService = new DrawEditingService();

            dsService = new DrawShellCourses();
            shapeService = new DrawBreakSymbols();
            publicService = new DrawPublicFunctionService();
        }

        // Sample 때문에 만들어짐
        public DrawEntityModel GetRoofSampleArrange(ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {
            DrawEntityModel drawEntity = new DrawEntityModel();
            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            double cirOD = GetRoofOuterDiameter();
            double cirODExtend = 0;
            double plateActualWidth = 2438;
            double plateActualLength = 9144;
            PlateArrange_Type plateType = PlateArrange_Type.Roof;

            // Sample // 12000

            // 193000  : Vertical : 높이가 작아서 계속 밀어 넣는 사례
            // 86000  : Vertical : 높이가 작아서 계속 밀어 넣는 사례
            // 51800  : Vertical : 아주 근소한 차이
            // 32000  : Vertical : 아주 근소한 차이
            // 20000  : 다른 스타일 필요
            // 10000 : Bottom 문제 점
            // 10000 : Roof 문제 점

            cirOD = 32000;
            cirODExtend = cirOD + 1000;
            //cirODExtend = 0;


            // Sample
            if (SingletonData.RoofBottomArrange[0] == "ROOF")
            {
                plateType = PlateArrange_Type.Roof;
            }
            else
            {
                plateType = PlateArrange_Type.Bottom;
            }


            cirOD = valueService.GetDoubleValue(SingletonData.RoofBottomArrange[1]);
            cirODExtend = cirOD + 1000;

            plateActualWidth = valueService.GetDoubleValue(SingletonData.RoofBottomArrange[2]);
            plateActualLength = valueService.GetDoubleValue(SingletonData.RoofBottomArrange[3]); ;

            scaleValue = 10;

            drawEntity = GetPlateArrange(plateType, referencePoint, cirOD, cirODExtend, plateActualWidth, plateActualLength, scaleValue);

            return drawEntity;
        }

        public DrawEntityModel GetRoofArrangement(ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {
            DrawEntityModel drawEntity = new DrawEntityModel();
            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            double cirOD = GetRoofOuterDiameter();
            double cirODExtend = 0;
            double plateActualWidth = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].PlateWidth);
            double plateActualLength = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].PlateLength);
            PlateArrange_Type plateType = PlateArrange_Type.Roof;

            // Sample // 12000

            // 193000  : Vertical : 높이가 작아서 계속 밀어 넣는 사례
            // 86000  : Vertical : 높이가 작아서 계속 밀어 넣는 사례
            // 51800  : Vertical : 아주 근소한 차이
            // 32000  : Vertical : 아주 근소한 차이
            // 20000  : 다른 스타일 필요
            // 10000 : Bottom 문제 점
            // 10000 : Roof 문제 점


            cirODExtend = GetRoofOD();
            cirOD = cirODExtend;
            if (assemblyData.RoofCompressionRing[0].CompressionRingType.Contains("Detail i"))
                cirOD = GetRoofODByCompressionRingDetailI();



            
            drawEntity =GetPlateArrange(plateType, referencePoint, cirOD, cirODExtend, plateActualWidth, plateActualLength, scaleValue);

            return drawEntity;
        }


        public DrawEntityModel GetBottomArrangement(ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
        {
            DrawEntityModel drawEntity = new DrawEntityModel();
            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            double cirOD = GetRoofOuterDiameter();
            double cirODExtend = 0;
            double plateActualWidth = valueService.GetDoubleValue(assemblyData.BottomInput[0].PlateWidth);
            double plateActualLength = valueService.GetDoubleValue(assemblyData.BottomInput[0].PlateLength);
            PlateArrange_Type plateType = PlateArrange_Type.Bottom;

            // Sample // 12000

            // 193000  : Vertical : 높이가 작아서 계속 밀어 넣는 사례
            // 86000  : Vertical : 높이가 작아서 계속 밀어 넣는 사례
            // 51800  : Vertical : 아주 근소한 차이
            // 32000  : Vertical : 아주 근소한 차이
            // 20000  : 다른 스타일 필요
            // 10000 : Bottom 문제 점
            // 10000 : Roof 문제 점

            cirODExtend = GetBottomOD();
            cirOD = cirODExtend;
            if (assemblyData.BottomInput[0].AnnularPlate.Contains("Yes"))
                cirOD = GetAnnularPlateBottomOD();


            drawEntity = GetPlateArrange(plateType, referencePoint, cirOD, cirODExtend, plateActualWidth, plateActualLength, scaleValue);

            return drawEntity;
        }



        private DrawEntityModel GetPlateArrange(PlateArrange_Type arrangeType,Point3D refPoint,double circleOD,double circleODExtend,double plateActualWidth, double plateActualLength, double scaleValue)
        {

            DrawEntityModel drawEntity = new DrawEntityModel();

            List<Entity> circleList = new List<Entity>();
            List<Entity> centerLineList = new List<Entity>();
            List<Entity> virtualList = new List<Entity>();

            List<Entity> plateLineList = new List<Entity>();
            List<Entity> horizontalLineList = new List<Entity>();
            List<Entity> textList = new List<Entity>();

            List<Entity> horizontalList = new List<Entity>();
            List<Entity> horizontalCenterList = new List<Entity>();
            List<Entity> verticalList = new List<Entity>();

            List<Entity> plateHorizontalList = new List<Entity>();

            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            bool circleExtendValue = false;

            double plateOverlap = 30;

            double plateWidth = plateActualWidth - plateOverlap;
            double plateLength = plateActualLength - plateOverlap;

            //double shellSideMinSpacing = 800;
            //double plateSideMinSpacing = 350;

            double circleRadius = circleOD / 2;
            double circleExtendRadius = circleODExtend / 2;


            // Outer : Circle
            Circle outCircle = new Circle(GetSumPoint(referencePoint, 0, 0), circleRadius);
            circleList.Add(outCircle);

            // Outer : Circle : Extend
            if (circleODExtend > 0)
            {
                Circle outCircleExtend = new Circle(GetSumPoint(referencePoint, 0, 0), circleExtendRadius);
                circleList.Add(outCircleExtend);

                circleExtendValue = true;
            }
            else
            {
                circleExtendRadius = circleRadius;
            }

            // Center Line
            double centerLineExt = 20;
            double centerLineLength= circleRadius;
            if (centerLineLength < circleExtendRadius)
                centerLineLength = circleExtendRadius;

            centerLineList.AddRange(editingService.GetCenterLine(GetSumPoint(referencePoint, 0, -centerLineLength), GetSumPoint(referencePoint, 0, centerLineLength), centerLineExt, 90));
            centerLineList.AddRange(editingService.GetCenterLine(GetSumPoint(referencePoint, -centerLineLength, 0), GetSumPoint(referencePoint, centerLineLength, 0), centerLineExt, 90));

            // Plate Horizontal Line
            plateHorizontalList.Add(new Line(GetSumPoint(referencePoint, -centerLineLength, -circleRadius), GetSumPoint(referencePoint, centerLineLength, -circleRadius)));
            plateHorizontalList.Add(new Line(GetSumPoint(referencePoint, -centerLineLength, +circleRadius), GetSumPoint(referencePoint, centerLineLength, +circleRadius)));
            if (circleExtendValue)
            {
                plateHorizontalList.Add(new Line(GetSumPoint(referencePoint, -centerLineLength, -circleExtendRadius), GetSumPoint(referencePoint, centerLineLength, -circleExtendRadius)));
                plateHorizontalList.Add(new Line(GetSumPoint(referencePoint, -centerLineLength, +circleExtendRadius), GetSumPoint(referencePoint, centerLineLength, +circleExtendRadius)));
            }

            // Virtual Line
            double virtualRectLengthFactor = 20;
            double virtualRectLength = GetVirtualRectLength(arrangeType,circleOD, plateWidth,plateLength) + virtualRectLengthFactor;
            double virtualRectLengthHalf = virtualRectLength / 2;
            double virtualRadius = circleRadius + circleRadius * 0.2;
            Line vLineTop = new Line(GetSumPoint(referencePoint, -circleRadius, virtualRectLengthHalf), GetSumPoint(referencePoint, circleRadius, virtualRectLengthHalf));
            Line vLineBottom = new Line(GetSumPoint(referencePoint, -circleRadius, -virtualRectLengthHalf), GetSumPoint(referencePoint, circleRadius, -virtualRectLengthHalf));
            Line vLineLeft = new Line(GetSumPoint(referencePoint, -virtualRectLengthHalf, circleRadius), GetSumPoint(referencePoint, -virtualRectLengthHalf, -circleRadius));
            Line vLineRight = new Line(GetSumPoint(referencePoint, virtualRectLengthHalf, circleRadius), GetSumPoint(referencePoint, virtualRectLengthHalf,-circleRadius));
            virtualList.AddRange(new Entity[] { vLineTop, vLineBottom, vLineLeft, vLineRight });


            // Virtual Line : Top Left
            Point3D[] vTopTopPoint = vLineTop.IntersectWith(outCircle);
            List<Point3D> vTopTopPointList = vTopTopPoint.OrderBy(x => x.X).ToList();
            Point3D[] vTopBottomPoint = vLineBottom.IntersectWith(outCircle);
            List<Point3D> vTopBottomPointList = vTopBottomPoint.OrderBy(x => x.X).ToList();
            Point3D[] vTopLeftPoint = vLineLeft.IntersectWith(outCircle);
            List<Point3D> vTopLeftPointList = vTopLeftPoint.OrderBy(x => x.Y).ToList();
            Point3D[] vTopRightPoint = vLineRight.IntersectWith(outCircle);
            List<Point3D> vTopRightPointList = vTopRightPoint.OrderBy(x => x.Y).ToList();
            Line vLineTopLeft = new Line(GetSumPoint(vTopTopPointList[0], 0, 0), new Point3D(vTopTopPointList[0].X, vTopLeftPointList[1].Y));
            Line vLineTopLeftBottom = new Line(GetSumPoint(vTopLeftPointList[1], 0, 0), new Point3D(vTopTopPointList[0].X, vTopLeftPointList[1].Y));
            Line vLineTopRight = new Line(GetSumPoint(vTopTopPointList[1], 0, 0), new Point3D(vTopTopPointList[1].X, vTopRightPointList[1].Y));
            Line vLineTopRightBottom = new Line(GetSumPoint(vTopRightPointList[1], 0, 0), new Point3D(vTopTopPointList[1].X, vTopRightPointList[1].Y));
            Line vLineBottomLeft = new Line(GetSumPoint(vTopBottomPointList[0], 0, 0), new Point3D(vTopBottomPointList[0].X, vTopLeftPointList[0].Y));
            Line vLineBottomLeftTop = new Line(GetSumPoint(vTopLeftPointList[0], 0, 0), new Point3D(vTopBottomPointList[0].X, vTopLeftPointList[0].Y));
            Line vLineBottomRight = new Line(GetSumPoint(vTopBottomPointList[1], 0, 0), new Point3D(vTopBottomPointList[1].X, vTopRightPointList[0].Y));
            Line vLineBottomRightTop = new Line(GetSumPoint(vTopRightPointList[0], 0, 0), new Point3D(vTopBottomPointList[1].X, vTopRightPointList[0].Y));
            virtualList.AddRange(new Entity[] { vLineTopLeft, vLineTopLeftBottom, vLineTopRight, vLineTopRightBottom, vLineBottomLeft, vLineBottomLeftTop, vLineBottomRight, vLineBottomRightTop });



            // Add PlateList : Same Size
            List<double> addPlateLengthList = new List<double>();



            // Horizontal Box
            List<Point3D> currentUpperPointList = new List<Point3D>();
            List<Point3D> beforeHorizontalList = new List<Point3D>();
            double horizontalLineLength = circleOD/2 + 1000;
            Point3D horizontalCenterPoint = GetSumPoint(referencePoint, 0, 0);
            double horizontalMaxY = vLineTop.StartPoint.Y-horizontalCenterPoint.Y;

            double beforeStartX = -1;
            double rowCount = 1;
            double currentStartX = GetStartXValue(arrangeType,beforeStartX, plateLength);
            double currentStartYFactor = GetStartYValue(arrangeType, plateWidth);
            double currentStartY = rowCount * plateWidth - currentStartYFactor;
            Point3D eachPoint = GetSumPoint(horizontalCenterPoint, 0, currentStartY);

            while (currentStartY < horizontalMaxY)
            {
                bool mirrorValue = true;

                // Center Line
                if (arrangeType==PlateArrange_Type.Roof)
                    if (rowCount == 1)
                    {
                        mirrorValue = false;
                        horizontalLineList.Add(new Line(GetSumPoint(horizontalCenterPoint, -horizontalLineLength, 0), GetSumPoint(horizontalCenterPoint, +horizontalLineLength, 0)));
                    }
                    
                

                
                // Draw : Row Line
                Line eachHorizontalUpper = new Line(GetSumPoint(eachPoint, -horizontalLineLength, 0), GetSumPoint(eachPoint, +horizontalLineLength, 0));
                List<Point3D> eachUpperInterList = GetHorizontalLineInter(eachHorizontalUpper, outCircle);
                Line eachHorizontalLower = new Line(GetSumPoint(eachPoint, -horizontalLineLength, -plateWidth), GetSumPoint(eachPoint, +horizontalLineLength, -plateWidth));
                List<Point3D> eachLowerInterList = GetHorizontalLineInter(eachHorizontalLower, outCircle);
                horizontalLineList.Add(eachHorizontalUpper);
                //if (arrangeType == PlateArrange_Type.Bottom)
                //    horizontalLineList.Add(eachHorizontalLower);

                double eachHorizonCenterX = eachPoint.X + currentStartX;
                // Draw : Horizontal
                List<Entity> tempHorizontalList = DrawHorizontalPlate(plateLength,plateWidth,
                                                                eachPoint, eachHorizonCenterX,
                                                                eachHorizontalUpper.StartPoint.Y, eachHorizontalLower.StartPoint.Y,
                                                                eachUpperInterList, eachLowerInterList,
                                                                ref beforeHorizontalList,
                                                                out List<Point3D> currentHorizontalList,
                                                                ref addPlateLengthList,
                                                                mirrorValue);

                if (arrangeType == PlateArrange_Type.Roof)
                {
                    horizontalList.AddRange(tempHorizontalList);
                }
                else if (arrangeType == PlateArrange_Type.Bottom)
                {
                    if (rowCount == 1)
                    {
                        horizontalCenterList.AddRange(tempHorizontalList);
                    }
                    else
                    {
                        horizontalList.AddRange(tempHorizontalList);
                    }
                }

                // Current Upper Point List
                List<Point3D> tempUpperPointList = currentHorizontalList.ToList();
                tempUpperPointList.AddRange(eachUpperInterList);
                currentUpperPointList = tempUpperPointList.OrderBy(x => x.X).ToList();

                // Next Value
                beforeStartX = currentStartX;
                currentStartX = GetStartXValue(arrangeType,beforeStartX, plateLength);

                rowCount++;
                currentStartY = rowCount * plateWidth - currentStartYFactor;
                eachPoint = GetSumPoint(horizontalCenterPoint, 0, currentStartY);

                beforeHorizontalList = currentHorizontalList;
            }

            // Horizontal
            plateLineList.AddRange(horizontalList);

            // Horizontal : Center List
            if (horizontalCenterList.Count > 0)
                plateLineList.AddRange(horizontalCenterList);

            // Horizontal Line Trim
            plateLineList.AddRange(GetLineByTrimBoth(horizontalLineList, outCircle));

            // Vertical Box
            if (currentUpperPointList.Count > 0)
            {
                Point3D verticalCenterPoint = GetSumPoint(eachPoint, 0, -plateWidth);
                eachPoint = GetSumPoint(verticalCenterPoint, 0, 0);
                currentStartX = GetStartVerticalYValue(beforeStartX, plateWidth);
                double eachVerticalCenterX = verticalCenterPoint.X + currentStartX;
                // Draw : Vertical Line
                verticalList = DrawVerticalPlate(plateLength, plateWidth,
                                                                eachPoint, eachVerticalCenterX,
                                                                currentUpperPointList,
                                                                outCircle);
                plateLineList.AddRange(verticalList);
            }


            // Bottom : Copy or Mirror
            List<Entity> rotateHorizontalLineList = new List<Entity>();
            List<Entity> rotateHorizontalList = new List<Entity>();
            List<Entity> rotateVerticalList = new List<Entity>();
            if (arrangeType == PlateArrange_Type.Roof)
            {
                // 180 회전
                double rotateValue = Utility.DegToRad(180);
                rotateHorizontalLineList = editingService.GetRotate(horizontalLineList, GetSumPoint(referencePoint, 0, 0), rotateValue);
                rotateHorizontalLineList.RemoveAt(0);
                rotateHorizontalList = editingService.GetRotate(horizontalList, GetSumPoint(referencePoint, 0, 0), rotateValue);
                rotateVerticalList = editingService.GetRotate(verticalList, GetSumPoint(referencePoint, 0, 0), rotateValue);
                plateLineList.AddRange(GetLineByTrimBoth(rotateHorizontalLineList, outCircle));
                plateLineList.AddRange(rotateHorizontalList);
                plateLineList.AddRange(rotateVerticalList);
            }
            else if (arrangeType == PlateArrange_Type.Bottom)
            {
                // 180 회전
                double rotateValue = Utility.DegToRad(180);
                rotateHorizontalLineList = editingService.GetRotate(horizontalLineList, GetSumPoint(referencePoint, 0, 0), rotateValue);
                rotateHorizontalList = editingService.GetRotate(horizontalList, GetSumPoint(referencePoint, 0, 0), rotateValue);
                rotateVerticalList = editingService.GetRotate(verticalList, GetSumPoint(referencePoint, 0, 0), rotateValue);
                plateLineList.AddRange(GetLineByTrimBoth(rotateHorizontalLineList, outCircle));
                plateLineList.AddRange(rotateHorizontalList);
                plateLineList.AddRange(rotateVerticalList);
            }


            // Plate Horizontal List
            plateHorizontalList.AddRange(horizontalCenterList);
            plateHorizontalList.AddRange(horizontalLineList);
            plateHorizontalList.AddRange(rotateHorizontalLineList);

            List<Entity> outPlateList = new List<Entity>();
            if(GetCustomArrangement(arrangeType,circleExtendValue,circleExtendRadius,plateWidth,plateLength,referencePoint,scaleValue,out outPlateList))
            {
                plateHorizontalList.Clear();
                plateLineList.Clear();
                plateLineList.AddRange(outPlateList);
            };


            // Draw Center Cut
            List<Entity> centerCutList = DrawCenterCutPlane(arrangeType, circleExtendValue, circleExtendRadius, plateLineList, plateHorizontalList, referencePoint, scaleValue);

            drawEntity.outlineList.AddRange(centerCutList);


            // Additional
            // Text
            foreach (Line eachLine in plateLineList)
            {
                //textList.Add(new Text(eachLine.EndPoint, eachLine.EndPoint.X.ToString(), 200));
            }


            // Create Plate Model
            List<DrawPlateModel> plateModelList = GetPlateModel(arrangeType, plateLineList, plateWidth, circleExtendRadius, GetSumPoint(referencePoint, 0, 0) );

            // Calculation : Cutting Plan Value
            DrawDetailPlateCuttingPlanService newCuttingPlanService = new DrawDetailPlateCuttingPlanService(null, null);
            newCuttingPlanService.SetPlateAddLength(plateModelList); // Length Adj
            newCuttingPlanService.SetCuttingInfo(plateModelList, plateActualWidth, plateActualLength); //

            // Draw Number
            foreach (DrawPlateModel eachPlate in plateModelList)
            {
                if(eachPlate.NumberPoint!=null)
                    textList.Add(new Text(GetSumPoint(eachPlate.NumberPoint, 0, 0), eachPlate.DisplayName, 2.5*scaleValue) { Alignment=Text.alignmentType.MiddleCenter});
            }


            // Tanslate arrange Data
            if (arrangeType == PlateArrange_Type.Bottom)
            {
                SingletonData.BottomPlateInfo.Clear();
                SingletonData.BottomPlateInfo.AddRange(plateModelList);


            }
            else if (arrangeType == PlateArrange_Type.Roof)
            {
                SingletonData.RoofPlateInfo.Clear();
                SingletonData.RoofPlateInfo.AddRange(plateModelList);
            }
                

            styleService.SetLayerListEntity(ref textList, layerService.LayerDimension);
            drawEntity.outlineList.AddRange(textList);
            styleService.SetLayerListEntity(ref plateLineList, layerService.LayerOutLine);
            drawEntity.outlineList.AddRange(plateLineList);
            styleService.SetLayerListEntity(ref virtualList, layerService.LayerVirtualLine);
            //drawEntity.outlineList.AddRange(virtualList);
            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            drawEntity.centerlineList.AddRange(centerLineList);
            styleService.SetLayerListEntity(ref circleList, layerService.LayerOutLine);
            drawEntity.outlineList.AddRange(circleList);
            return drawEntity;
        }






        public List<DrawPlateModel> GetPlateModel_New(PlateArrange_Type arrangeType, List<Entity> selEntities, double plateWidth, double selRadius, Point3D selCenterPoint)
        {
            List<DrawPlateModel> plateList = new List<DrawPlateModel>();

            Dictionary<double, double> xDic = new Dictionary<double, double>();
            Dictionary<double, double> yDic = new Dictionary<double, double>();
            foreach (ICurve eachLine in selEntities)
            {
                if()
            }

            return plateList;
        }




        public List<DrawPlateModel> GetPlateModel(PlateArrange_Type arrangeType,List<Entity> selEntities,double plateWidth,double selRadius,Point3D selCenterPoint)
        {
            List<DrawPlateModel> newPlateList = new List<DrawPlateModel>();

            // Return
            if (selEntities.Count == 0)
                return newPlateList;

            string plateNamePre = "";
            if (arrangeType == PlateArrange_Type.Bottom)
                plateNamePre = "B"; 
            else if (arrangeType == PlateArrange_Type.Roof)
                plateNamePre = "R";

            double centerY = selCenterPoint.Y;
            double centerX = selCenterPoint.X;

            Dictionary<double, List<double>> HLineDic = new Dictionary<double, List<double>>();
            List<ICurve> VLineEntities = new List<ICurve>();
            List<ICurve> HLineEntities = new List<ICurve>();

            foreach (ICurve eachEntity in selEntities)
            {
                if (eachEntity.StartPoint.Y == eachEntity.EndPoint.Y)
                {
                    HLineEntities.Add(eachEntity);
                    if (!HLineDic.ContainsKey(eachEntity.StartPoint.Y))
                    {
                        List<double> tempList = new List<double>();
                        tempList.Add(eachEntity.StartPoint.X);
                        tempList.Add(eachEntity.EndPoint.X);
                        tempList.Sort();
                        HLineDic.Add(eachEntity.StartPoint.Y, tempList);
                    }
                    else
                    {
                        List<double> tempList = HLineDic[eachEntity.StartPoint.Y];
                        tempList.Add(eachEntity.StartPoint.X);
                        tempList.Add(eachEntity.EndPoint.X);
                        tempList.Sort();
                        HLineDic[eachEntity.StartPoint.Y] = tempList;
                    }
                }
                else
                {
                    VLineEntities.Add(eachEntity);
                }
            }



            // 내림 차순 정렬 => 중앙 정렬 : 위가 우선
            List<double> HLineYList = HLineDic.Keys.ToList();
            List<double> HLineYSortDesList = HLineYList.OrderByDescending(x => x).ToList();
            List<double> HLineYSortAscList = HLineYList.OrderBy(x => x).ToList();
            double hMidCount = HLineYSortDesList.Count / 2;
            int hMidIndex = int.Parse( Math.Truncate(hMidCount).ToString());
            double hMidValue = HLineYSortDesList[hMidIndex];

            List<double> HLineYAlignList = new List<double>();
            HLineYAlignList.Add(HLineYSortDesList[hMidIndex]);
            int runPlusCount=hMidIndex;
            int runMinusCount = hMidIndex;
            while (runMinusCount >= 0)
            {
                runMinusCount--;
                runPlusCount++;
                if (runMinusCount > 0)
                    HLineYAlignList.Add(HLineYSortDesList[runMinusCount]);
                if(runPlusCount< HLineYSortDesList.Count)
                    HLineYAlignList.Add(HLineYSortDesList[runPlusCount]);

            }

            // Number Count
            double numberCount = 0;

            // Horizontal Line
            double vFactor = 1; 
            List<ICurve> VEntitiesSort = VLineEntities.OrderBy(x => x.StartPoint.X).ToList();
            
            foreach (double eachLY in HLineYAlignList)
            {
                // Lower, Upper Factor
                double eachUY = eachLY;
                foreach(double eachUpperTempH in HLineYSortAscList)
                {
                    if (eachUpperTempH > eachLY)
                    {
                        eachUY = eachUpperTempH;
                        break;
                    }
                }
                double eachLowerHLine = eachLY - vFactor;
                double eachUpperHLine = eachUY + vFactor;

                // One Line
                List<ICurve> oneVLineList = new List<ICurve>();
                foreach (ICurve eachV in VEntitiesSort)
                    if (eachV.StartPoint.Y >= eachLowerHLine && eachV.EndPoint.Y >= eachLowerHLine &&
                        eachV.StartPoint.Y <= eachUpperHLine && eachV.EndPoint.Y < eachUpperHLine)
                        oneVLineList.Add(eachV);

                // Create Plate
                List<DrawPlateModel> onePlateList = new List<DrawPlateModel>();
                List<double> lowerXEndPoint = HLineDic[eachLY];
                List<double> upperXEndPoint = HLineDic[eachUY];
                double lowerBeforeX = lowerXEndPoint.First();
                double upperBeforeX = upperXEndPoint.First();


                // 세로 라인이 있을 경우
                if (oneVLineList.Count > 0)
                {
                    double firstVLineIndex = 0;
                    double lastVLineIndex = oneVLineList.Count;
                    for (int i = 0; i < oneVLineList.Count; i++)
                    {
                        Line eachOneVLine = (Line)oneVLineList[i];
                        double eachOneVLineLength = eachOneVLine.Length();
                        double eachOneVLineX = eachOneVLine.StartPoint.X;
                        double maxBeforeX = Math.Max(lowerBeforeX, upperBeforeX);

                        double eachLowerLength = eachOneVLineX - lowerBeforeX;
                        double eachUpperLength = eachOneVLineX - upperBeforeX;

                        DrawPlateModel eachOnePlate = new DrawPlateModel();
                        eachOnePlate.Radius = selRadius;
                        eachOnePlate.PlateDirection = PERPENDICULAR_TYPE.Horizontal;

                        // 왼쪽
                        if (i == firstVLineIndex)
                        {
                            // One Line : Left
                            eachOnePlate.ShapeType = Plate_Type.RectangleArc;
                            if (eachUY - 1 >= centerY)
                            {
                                // Left : Upper
                                eachOnePlate.ArcDirection = ORIENTATION_TYPE.TOPLEFT;
                                if (eachOneVLineX < upperXEndPoint[0])
                                {
                                    eachOnePlate.ShapeType = Plate_Type.Arc;
                                    eachOnePlate.HLength.Add(eachLowerLength);
                                }
                                else
                                {
                                    eachOnePlate.HLength.Add(eachUpperLength);
                                    eachOnePlate.HLength.Add(eachLowerLength);
                                    // Center
                                    if (lowerXEndPoint[0] == upperXEndPoint[0])
                                        eachOnePlate.ArcDirection = ORIENTATION_TYPE.LEFTCENTER;
                                }
                            }
                            else
                            {
                                // Left : Lower
                                eachOnePlate.ArcDirection = ORIENTATION_TYPE.BOTTOMLEFT;
                                if (eachOneVLineX < lowerXEndPoint[0])
                                {
                                    eachOnePlate.ShapeType = Plate_Type.Arc;
                                    eachOnePlate.HLength.Add(eachUpperLength);
                                }
                                else
                                {
                                    eachOnePlate.HLength.Add(eachLowerLength);
                                    eachOnePlate.HLength.Add(eachUpperLength);
                                }


                            }
                            eachOnePlate.VLength.Add(eachOneVLineLength);

                            // Plate Add
                            eachOnePlate.NumberPoint = GetNumberPoint(eachOneVLine.MidPoint.Y, maxBeforeX, eachOnePlate.HLength);
                            onePlateList.Add(eachOnePlate);

                            lowerBeforeX = eachOneVLineX;
                            upperBeforeX = eachOneVLineX;
                        }


                        // 오른쪽
                        else if (i == lastVLineIndex)
                        {
                            double minBeforeX = Math.Min(lowerBeforeX, upperBeforeX);
                            DrawPlateModel eachOnePlateLast = new DrawPlateModel();
                            eachOnePlateLast.Radius = selRadius;
                            eachOnePlateLast.PlateDirection = PERPENDICULAR_TYPE.Horizontal;

                            // One Line : Right
                            int lowerEndPointIndex = lowerXEndPoint.Count - 1;
                            int upperEndPointIndex = upperXEndPoint.Count - 1;
                            double lowerEndX = lowerXEndPoint[lowerEndPointIndex];
                            double upperEndX = upperXEndPoint[upperEndPointIndex];
                            eachOnePlateLast.ShapeType = Plate_Type.RectangleArc;
                            if (eachUY - 1 >= centerY)
                            {
                                // Right : Upper
                                eachOnePlateLast.ArcDirection = ORIENTATION_TYPE.TOPRIGHT;
                                if (eachOneVLineX > upperXEndPoint[upperEndPointIndex])
                                {
                                    eachOnePlateLast.ShapeType = Plate_Type.Arc;
                                }
                                else
                                {
                                    eachOnePlateLast.HLength.Add(upperEndX - upperBeforeX);
                                    eachOnePlateLast.HLength.Add(lowerEndX - lowerBeforeX);
                                    if (lowerXEndPoint[lowerEndPointIndex] == upperXEndPoint[upperEndPointIndex])
                                        eachOnePlateLast.ArcDirection = ORIENTATION_TYPE.RIGHTCENTER;
                                }
                            }
                            else
                            {
                                // Right : Lower
                                eachOnePlateLast.ArcDirection = ORIENTATION_TYPE.BOTTOMRIGHT;
                                if (eachOneVLineX > lowerXEndPoint[lowerEndPointIndex])
                                {
                                    eachOnePlateLast.ShapeType = Plate_Type.Arc;
                                    eachOnePlateLast.HLength.Add(upperEndX - upperBeforeX);
                                }
                                else
                                {
                                    eachOnePlateLast.HLength.Add(lowerEndX - lowerBeforeX);
                                    eachOnePlateLast.HLength.Add(upperEndX - upperBeforeX);
                                }

                            }

                            eachOnePlateLast.VLength.Add(eachOneVLineLength);

                            // Plate Add
                            eachOnePlateLast.NumberPoint = GetNumberPoint(eachOneVLine.MidPoint.Y, minBeforeX, eachOnePlateLast.HLength);
                            onePlateList.Add(eachOnePlateLast);
                        }


                        // 중간
                        else
                        {
                            // One Line : Mid
                            eachOnePlate.ShapeType = Plate_Type.Rectangle;
                            eachOnePlate.VLength.Add(eachOneVLineLength);
                            eachOnePlate.VLength.Add(eachOneVLineLength);
                            eachOnePlate.HLength.Add(eachOneVLineX - lowerBeforeX);
                            eachOnePlate.HLength.Add(eachOneVLineX - upperBeforeX);

                            // Plate Add
                            eachOnePlate.NumberPoint = GetNumberPoint(eachOneVLine.MidPoint.Y, maxBeforeX, eachOnePlate.HLength);
                            onePlateList.Add(eachOnePlate);

                            lowerBeforeX = eachOneVLineX;
                            upperBeforeX = eachOneVLineX;

                        }




                        



                    }
                }
                else
                {
                    if(lowerXEndPoint.Count>0 && upperXEndPoint.Count > 0)
                    {
                        // One Plate
                        DrawPlateModel eachOnePlate = new DrawPlateModel();
                        eachOnePlate.Radius = selRadius;
                        eachOnePlate.PlateDirection = PERPENDICULAR_TYPE.Horizontal;
                        eachOnePlate.ArcDirection = ORIENTATION_TYPE.BOTHCENTER;
                        eachOnePlate.ShapeType = Plate_Type.ArcRectangleArc;

                        double vLength = eachUY - eachLY;
                        eachOnePlate.VLength.Add(vLength);
                        eachOnePlate.HLength.Add(lowerXEndPoint[1] - lowerXEndPoint[0]);
                        eachOnePlate.HLength.Add(upperXEndPoint[1] - upperXEndPoint[0]);

                        // Plate Add
                        eachOnePlate.NumberPoint = new Point3D(lowerXEndPoint[0] + eachOnePlate.HLength[0] / 2, eachLY + vLength / 2);
                        onePlateList.Add(eachOnePlate);
                    }


                }



                // 재배열
                newPlateList.AddRange(GetSortListByCenterIndex(ref numberCount, onePlateList));

                // Delete
                foreach (ICurve eachV in oneVLineList)
                    VEntitiesSort.Remove(eachV);
            }

            // Vertical Line
            double upperHLineY = HLineYSortDesList.First();
            double lowerHLineY = HLineYSortDesList.Last();
            List<double> upperXHLineEndPoint = HLineDic[upperHLineY];
            List<double> lowerXHLineEndPoint = HLineDic[lowerHLineY];
            List<ICurve> oneLineVUpperList = new List<ICurve>();
            List<ICurve> oneLineVLowerList = new List<ICurve>();
            Dictionary<double, double> oneLineVUpperDic = new Dictionary<double, double>();
            Dictionary<double, double> oneLineVLowerDic = new Dictionary<double, double>();
            double upperVCount = 0;
            double lowerVCount = 0;
            double vLowerBeforeX = lowerXHLineEndPoint[0];
            double vUpperBeforeX = upperXHLineEndPoint[0];
            double vBeforeLength = 0;
            double vBeforeMidY = 0;
            foreach (ICurve eachV in VEntitiesSort)
            {
                double vLineX = eachV.StartPoint.X;
                double vLineY = eachV.StartPoint.Y;
                
                if (vLineY > centerY)
                {
                    if (!oneLineVUpperDic.ContainsKey(vLineX))
                    {
                        // 신규
                        oneLineVUpperDic.Add(vLineX, eachV.Length());
                        oneLineVUpperList.Add(eachV);
                    }
                    
                }
                else
                {
                    if (!oneLineVLowerDic.ContainsKey(vLineX))
                    {
                        oneLineVLowerDic.Add(vLineX, eachV.Length());
                        oneLineVLowerList.Add(eachV);
                    }
                }
            }


            List<DrawPlateModel> oneUpperPlateList = new List<DrawPlateModel>();
            List<DrawPlateModel> oneLowerPlateList = new List<DrawPlateModel>();
            // Vertical : Upper : Lower == 0    -> Segment
            if (oneLineVUpperList.Count == 0)
            {
                DrawPlateModel eachOnePlate = new DrawPlateModel();
                eachOnePlate.Radius = selRadius;
                eachOnePlate.PlateDirection = PERPENDICULAR_TYPE.Horizontal;
                eachOnePlate.ShapeType = Plate_Type.Segment;
                eachOnePlate.ArcDirection = ORIENTATION_TYPE.TOPCENTER;
                double leftX = valueService.GetMinValueOfList(upperXHLineEndPoint);
                double rightX = valueService.GetMaxValueOfList(upperXHLineEndPoint);
                double stringLength = rightX - leftX;
                eachOnePlate.HLength.Add(stringLength);
                double betweenSegmentLine = valueService.GetLengthBetweenStringAndArc(selRadius, stringLength);
                eachOnePlate.NumberPoint = new Point3D(leftX + stringLength / 2, upperHLineY + betweenSegmentLine / 2);
                oneUpperPlateList.Add(eachOnePlate);
            }
            if (oneLineVLowerList.Count == 0)
            {
                DrawPlateModel eachOnePlate = new DrawPlateModel();
                eachOnePlate.Radius = selRadius;
                eachOnePlate.PlateDirection = PERPENDICULAR_TYPE.Horizontal;
                eachOnePlate.ShapeType = Plate_Type.Segment;
                eachOnePlate.ArcDirection = ORIENTATION_TYPE.BOTTOMCENTER;
                double leftX = valueService.GetMinValueOfList(lowerXHLineEndPoint);
                double rightX = valueService.GetMaxValueOfList(lowerXHLineEndPoint);
                double stringLength = rightX - leftX;
                eachOnePlate.HLength.Add(stringLength);
                double betweenSegmentLine = valueService.GetLengthBetweenStringAndArc(selRadius, stringLength);
                eachOnePlate.NumberPoint = new Point3D(leftX + stringLength / 2, lowerHLineY - betweenSegmentLine / 2);
                oneUpperPlateList.Add(eachOnePlate);
            }

            if(HLineEntities.Count>0 && VLineEntities.Count == 0)
            {
                //모두 가로선만 있을 경우
                // Sagment : Upper, Lower 제외하고 중간 필요함

            }

            // Vertical : Upper
            for (int i = 0; i < oneLineVUpperList.Count; i++)
            {
                Line eachOneV = (Line)oneLineVUpperList[i];
                double eachOneVLength = eachOneV.Length();
                double eachOneVX = eachOneV.StartPoint.X;

                DrawPlateModel eachOnePlate = new DrawPlateModel();
                eachOnePlate.Radius = selRadius;
                eachOnePlate.PlateDirection = PERPENDICULAR_TYPE.Vertical;
                if (i == 0)
                {
                    // One Line : Left
                    eachOnePlate.ShapeType = Plate_Type.RectangleArc;
                    if (vUpperBeforeX - 1 <= centerX)
                    {
                        // Left : Upper
                        eachOnePlate.ArcDirection = ORIENTATION_TYPE.TOPLEFT;
                        eachOnePlate.ShapeType = Plate_Type.Arc;
                        eachOnePlate.HLength.Add(eachOneVX - vUpperBeforeX);
                        if (eachOnePlate.HLength[0] > plateWidth)
                            eachOnePlate.PlateDirection = PERPENDICULAR_TYPE.Horizontal;
                    }
                    
                    eachOnePlate.VLength.Add(eachOneVLength);

                    // Plate Add
                    eachOnePlate.NumberPoint = GetNumberPoint(eachOneV.MidPoint.Y, vUpperBeforeX, eachOnePlate.HLength);
                    eachOnePlate.NumberPoint=GetSumPoint(eachOnePlate.NumberPoint, valueService.GetMinValueOfList( eachOnePlate.HLength) / 4, -eachOneVLength/4);
                    oneUpperPlateList.Add(eachOnePlate);

                    vUpperBeforeX = eachOneVX;
                    vBeforeLength = eachOneVLength;
                    vBeforeMidY = eachOneV.MidPoint.Y;
                }
                else
                {
                    // One Line : Mid
                    eachOnePlate.ShapeType = Plate_Type.RectangleArc;
                    if (eachOneVX > centerX && vUpperBeforeX > centerX)
                    {
                        eachOnePlate.ArcDirection = ORIENTATION_TYPE.TOPRIGHT;
                    }
                    else if(eachOneVX < centerX && vUpperBeforeX < centerX)
                    {
                        eachOnePlate.ArcDirection = ORIENTATION_TYPE.TOPLEFT;
                    }
                    else
                    {
                        double tempRight = eachOneVX - centerX;
                        double tempLeft = centerX - vUpperBeforeX;
                        double tempPlateWidthHalf = plateWidth / 2;
                        double tempAdjFactor = 10;
                        if(tempRight> tempPlateWidthHalf- tempAdjFactor && tempRight< tempPlateWidthHalf+ tempAdjFactor)
                        {
                            eachOnePlate.ArcDirection = ORIENTATION_TYPE.TOPCENTER;
                        }
                        else if(tempRight>tempLeft)
                        {
                            eachOnePlate.ArcDirection = ORIENTATION_TYPE.TOPRIGHT;
                        }
                        else
                        {
                            eachOnePlate.ArcDirection = ORIENTATION_TYPE.TOPLEFT;
                        }
                    }

                    eachOnePlate.VLength.Add(vBeforeLength);
                    eachOnePlate.VLength.Add(eachOneVLength);
                    eachOnePlate.VLength.Sort();
                    eachOnePlate.HLength.Add(eachOneVX - vUpperBeforeX);

                    // Plate Add
                    if (eachOnePlate.ArcDirection == ORIENTATION_TYPE.TOPLEFT)
                        eachOnePlate.NumberPoint = GetNumberPoint(vBeforeMidY, vUpperBeforeX, eachOnePlate.HLength);
                    else
                        eachOnePlate.NumberPoint = GetNumberPoint(eachOneV.MidPoint.Y, vUpperBeforeX, eachOnePlate.HLength);
                    oneUpperPlateList.Add(eachOnePlate);

                    vUpperBeforeX = eachOneVX;
                    vBeforeLength = eachOneVLength;
                    vBeforeMidY = eachOneV.MidPoint.Y;

                }

                if (i == oneLineVUpperList.Count-1)
                {
                    vUpperBeforeX = upperXHLineEndPoint[upperXHLineEndPoint.Count - 1];
                    DrawPlateModel eachOnePlateLast = new DrawPlateModel();
                    eachOnePlateLast.Radius = selRadius;
                    eachOnePlateLast.PlateDirection = PERPENDICULAR_TYPE.Vertical;

                    // One Line : Right
                    eachOnePlateLast.ShapeType = Plate_Type.RectangleArc;
                    if (vUpperBeforeX - 1 >= centerX)
                    {
                        // Right : Upper
                        eachOnePlateLast.ArcDirection = ORIENTATION_TYPE.TOPRIGHT;
                        eachOnePlateLast.ShapeType = Plate_Type.Arc;
                        eachOnePlateLast.HLength.Add(vUpperBeforeX-eachOneVX);
                        if (eachOnePlateLast.HLength[0] > plateWidth)
                            eachOnePlateLast.PlateDirection = PERPENDICULAR_TYPE.Horizontal;
                    }

                    eachOnePlateLast.VLength.Add(eachOneVLength);

                    // Plate Add
                    eachOnePlateLast.NumberPoint = GetNumberPoint(eachOneV.MidPoint.Y, eachOneVX, eachOnePlateLast.HLength);
                    eachOnePlateLast.NumberPoint = GetSumPoint(eachOnePlateLast.NumberPoint, -valueService.GetMinValueOfList(eachOnePlateLast.HLength) / 4, -eachOneVLength / 4);
                    oneUpperPlateList.Add(eachOnePlateLast);

                    vUpperBeforeX = eachOneVX;
                    vBeforeLength = eachOneVLength;
                }

            }
            // 재배열
            newPlateList.AddRange(GetSortListByCenterIndex(ref numberCount, oneUpperPlateList));


            // Vertical : Lower
            for (int i = 0; i < oneLineVLowerList.Count; i++)
            {
                Line eachOneV = (Line)oneLineVLowerList[i];
                double eachOneVLength = eachOneV.Length();
                double eachOneVX = eachOneV.StartPoint.X;

                DrawPlateModel eachOnePlate = new DrawPlateModel();
                eachOnePlate.Radius = selRadius;
                eachOnePlate.PlateDirection = PERPENDICULAR_TYPE.Vertical;
                if (i == 0)
                {
                    // One Line : Left
                    eachOnePlate.ShapeType = Plate_Type.RectangleArc;
                    if (vLowerBeforeX - 1 <= centerX)
                    {
                        // Left : Upper
                        eachOnePlate.ArcDirection = ORIENTATION_TYPE.BOTTOMLEFT;
                        eachOnePlate.ShapeType = Plate_Type.Arc;
                        eachOnePlate.HLength.Add(eachOneVX - vLowerBeforeX);
                        if (eachOnePlate.HLength[0] > plateWidth)
                            eachOnePlate.PlateDirection = PERPENDICULAR_TYPE.Horizontal;
                    }

                    eachOnePlate.VLength.Add(eachOneVLength);

                    // Plate Add
                    eachOnePlate.NumberPoint = GetNumberPoint(eachOneV.MidPoint.Y, vLowerBeforeX, eachOnePlate.HLength);
                    eachOnePlate.NumberPoint = GetSumPoint(eachOnePlate.NumberPoint, valueService.GetMinValueOfList( eachOnePlate.HLength)/ 4, eachOneVLength / 4);
                    oneLowerPlateList.Add(eachOnePlate);

                    vLowerBeforeX = eachOneVX;
                    vBeforeLength = eachOneVLength;
                    vBeforeMidY = eachOneV.MidPoint.Y;
                }
                else
                {
                    // One Line : Mid
                    eachOnePlate.ShapeType = Plate_Type.RectangleArc;
                    if (eachOneVX > centerX && vLowerBeforeX > centerX)
                    {
                        eachOnePlate.ArcDirection = ORIENTATION_TYPE.BOTTOMRIGHT;
                    }
                    else if (eachOneVX < centerX && vLowerBeforeX < centerX)
                    {
                        eachOnePlate.ArcDirection = ORIENTATION_TYPE.BOTTOMLEFT;
                    }
                    else
                    {
                        double tempRight = eachOneVX - centerX;
                        double tempLeft = centerX - vLowerBeforeX;
                        double tempPlateWidthHalf = plateWidth / 2;
                        double tempAdjFactor = 10;
                        if (tempRight > tempPlateWidthHalf - tempAdjFactor && tempRight < tempPlateWidthHalf + tempAdjFactor)
                        {
                            eachOnePlate.ArcDirection = ORIENTATION_TYPE.BOTTOMCENTER;
                        }
                        else if (tempRight > tempLeft)
                        {
                            eachOnePlate.ArcDirection = ORIENTATION_TYPE.BOTTOMRIGHT;
                        }
                        else
                        {
                            eachOnePlate.ArcDirection = ORIENTATION_TYPE.BOTTOMLEFT;
                        }
                    }

                    eachOnePlate.VLength.Add(vBeforeLength);
                    eachOnePlate.VLength.Add(eachOneVLength);
                    eachOnePlate.VLength.Sort();
                    eachOnePlate.HLength.Add(eachOneVX - vLowerBeforeX);

                    // Plate Add
                    if (eachOnePlate.ArcDirection == ORIENTATION_TYPE.BOTTOMLEFT)
                        eachOnePlate.NumberPoint = GetNumberPoint(vBeforeMidY, vLowerBeforeX, eachOnePlate.HLength);
                    else
                        eachOnePlate.NumberPoint = GetNumberPoint(eachOneV.MidPoint.Y, vLowerBeforeX, eachOnePlate.HLength);
                    oneLowerPlateList.Add(eachOnePlate);

                    vLowerBeforeX = eachOneVX;
                    vBeforeLength = eachOneVLength;
                    vBeforeMidY = eachOneV.MidPoint.Y;

                }

                if (i == oneLineVLowerList.Count - 1)
                {
                    vLowerBeforeX = lowerXHLineEndPoint[lowerXHLineEndPoint.Count - 1];
                    DrawPlateModel eachOnePlateLast = new DrawPlateModel();
                    eachOnePlateLast.Radius = selRadius;
                    eachOnePlateLast.PlateDirection = PERPENDICULAR_TYPE.Vertical;

                    // One Line : Right
                    eachOnePlateLast.ShapeType = Plate_Type.RectangleArc;
                    if (vLowerBeforeX - 1 >= centerX)
                    {
                        // Right : Upper
                        eachOnePlateLast.ArcDirection = ORIENTATION_TYPE.TOPRIGHT;
                        eachOnePlateLast.ShapeType = Plate_Type.Arc;
                        eachOnePlateLast.HLength.Add(vLowerBeforeX - eachOneVX);
                        if (eachOnePlateLast.HLength[0] > plateWidth)
                            eachOnePlateLast.PlateDirection = PERPENDICULAR_TYPE.Horizontal;
                    }

                    eachOnePlateLast.VLength.Add(eachOneVLength);

                    // Plate Add
                    eachOnePlateLast.NumberPoint = GetNumberPoint(eachOneV.MidPoint.Y, eachOneVX, eachOnePlateLast.HLength);
                    eachOnePlateLast.NumberPoint = GetSumPoint(eachOnePlateLast.NumberPoint, -valueService.GetMinValueOfList(eachOnePlateLast.HLength) / 4, eachOneVLength / 4);
                    oneLowerPlateList.Add(eachOnePlateLast);

                    vLowerBeforeX = eachOneVX;
                    vBeforeLength = eachOneVLength;
                }

            }
            // 재배열
            newPlateList.AddRange(GetSortListByCenterIndex(ref numberCount, oneLowerPlateList));






            // Numbering
            List<DrawPlateModel> plateSortList = newPlateList.OrderBy(x => x.NumberCount).ToList();
            Dictionary<string, string[]> plateNameDic = new Dictionary<string, string[]>();
            double dicCount = 0;
            foreach (DrawPlateModel eachPlate in plateSortList)
            {
                string numberString = "";
                foreach (double eachVLength in eachPlate.VLength)
                    numberString += "|" + Math.Round(eachVLength, 1, MidpointRounding.AwayFromZero);
                foreach (double eachHLength in eachPlate.HLength)
                    numberString += "|" + Math.Round(eachHLength, 1, MidpointRounding.AwayFromZero);
                if (!plateNameDic.ContainsKey(numberString))
                {
                    // 처음
                    dicCount++;
                    string displayName = plateNamePre + dicCount;
                    eachPlate.DisplayName = displayName;
                    eachPlate.Number = dicCount;
                    plateNameDic.Add(numberString, new string[] { displayName,dicCount.ToString() });

                }
                else
                {
                    // 기존
                    string[] eachString = plateNameDic[numberString];
                    eachPlate.DisplayName = eachString[0];
                    eachPlate.Number = valueService.GetDoubleValue( eachString[1]);
                }
            }

            return plateSortList;
        }


        private List<DrawPlateModel> GetSortListByCenterIndex(ref double numberCount,List<DrawPlateModel> selList)
        {
            List<DrawPlateModel> newList = new List<DrawPlateModel>();

            List<int> reNumberList = new List<int>();
            if (selList.Count > 0)
            {
                reNumberList = GetCenterIndexList(selList.Count);
            }
            foreach (int eachIndex in reNumberList)
            {
                DrawPlateModel eachPlate = selList[eachIndex];
                eachPlate.NumberCount = numberCount++;
                newList.Add(eachPlate);
            }

            return newList;
        }

        private List<int>GetCenterIndexList(double selNumber)
        {
            List<int> newList = new List<int>();
            double hMidCount = selNumber / 2;
            int hMidIndex = int.Parse(Math.Truncate(hMidCount).ToString());
            int runPlusCount = hMidIndex;
            int runMinusCount = hMidIndex;
            newList.Add(hMidIndex);
            while (runMinusCount >= 0)
            {
                runMinusCount--;
                runPlusCount++;
                if (runMinusCount >= 0)
                    newList.Add(runMinusCount);
                if (runPlusCount < selNumber)
                    newList.Add(runPlusCount);

            }


            return newList;
        }
        public Point3D GetNumberPoint(double midY,double X, List<double> selXList)
        {
           
            double minX = valueService.GetMinValueOfList(selXList)/2;
            Point3D newPoint = new Point3D(X + minX, midY);
            return newPoint;
        }




        public  double GetRoofOuterDiameter()
        {
            double roofOD = 0;
            if(assemblyData !=null)
                if (assemblyData.GeneralDesignData.Count > 0)
                {
                    roofOD= valueService.GetDoubleValue( assemblyData.GeneralDesignData[0].SizeNominalID);
                }
            return roofOD;
        }

        private List<Entity> DrawCenterCutPlane(PlateArrange_Type arrangeType,bool circleExtendValue,double circleLength,  
                                                List<Entity> selPlateList, List<Entity> selHLineList, 
                                                Point3D centerPoint,double scaleValue)
        {
            List<Entity> newList = new List<Entity>();

            // Scale : Page
            double distanceValue = 260;
            if (circleLength*2 >= 24801)
                distanceValue = 350;
            double scaleDistanceValue = distanceValue * scaleValue;
            //----------------------------------------------


            double plateOverlapGap = 15;

            double plateVerticalGap = 0.5;
            double scalePlateVerticalGap = plateVerticalGap * scaleValue;

            double slopeAngel = 0;
            if (arrangeType == PlateArrange_Type.Roof)
            {
                slopeAngel = -Utility.DegToRad(6);
            }
            else if (arrangeType == PlateArrange_Type.Bottom)
            {
                slopeAngel = -Utility.DegToRad(4);
            }

            Dictionary<double, double> YDic = new Dictionary<double, double>();
            foreach(Line eachEntity in selPlateList)
            {
                double eachStartY = eachEntity.StartPoint.Y;
                double eachEndY = eachEntity.EndPoint.Y;
                if(eachStartY==eachEndY)
                    if (!YDic.ContainsKey(eachStartY))
                        YDic.Add(eachStartY, eachStartY);
            }
            foreach (Line eachEntity in selHLineList)
            {
                double eachStartY = eachEntity.StartPoint.Y;
                if (!YDic.ContainsKey(eachStartY))
                    YDic.Add(eachStartY, eachStartY);
            }

            List<double> yTempList = YDic.Keys.ToList();

            List<double> YList = yTempList.OrderByDescending(d => d).ToList();




            Point3D yCenterPoint = GetSumPoint(centerPoint, scaleDistanceValue, 0);
            Point3D yTopPoint = new Point3D(yCenterPoint.X, YList[0]);
            Point3D yBottomPoint = new Point3D(yCenterPoint.X, YList[YList.Count-1]);


            if (arrangeType == PlateArrange_Type.Bottom)
            {
                if (circleExtendValue)
                {
                    if (YList.Count > 2)
                    {
                        YList.RemoveAt(YList.Count - 1);
                        YList.RemoveAt(0);
                    }
                }
            }

            double minY = YList[0]; ;
            double maxY = YList[YList.Count-1];
            double yLength = maxY - minY;
            double yLengthHlaf = yLength / 2;

            bool yEven = false;
            double yLengthMiddleCount = Convert.ToDouble(YList.Count) / 2;
            if (Convert.ToDouble(YList.Count) % 2 == 0)
                yEven = true;
            double middleValue = Math.Ceiling(yLengthMiddleCount);
            if (yEven)
                middleValue++;

            List<Line> plateUpperList = new List<Line>();
            List<Line> plateUpperHorizontalLineList = new List<Line>();
            List<Entity> plateEtcList = new List<Entity>();

            double plateLength = circleLength * 2;
            Point3D yTopPlatePoint = new Point3D(yCenterPoint.X, YList[0]);
            Point3D yBottomPlatePoint = new Point3D(yCenterPoint.X, YList[YList.Count-1]);
            plateEtcList.Add(new Line(GetSumPoint(yTopPoint, 0, 0), GetSumPoint(yBottomPlatePoint, 0, 0)));

            for (int i = 1; i < middleValue ; i++)
            {
                double eachY = YList[i-1];
                double eachYNext = YList[i ];
                if (i == middleValue-1)
                {
                    if (yEven)
                    {
                        // 절반 나누기
                        eachYNext = (eachYNext) + ((eachY- eachYNext) / 2);
                    }

                }

                Line newPlate = new Line(GetSumPoint(yTopPoint, 0, 0), GetSumPoint(yTopPoint, 0, -plateLength));
                newPlate.Rotate(slopeAngel,Vector3D.AxisZ,GetSumPoint(yTopPoint,0,0));
                plateUpperList.Add(newPlate);

                Line newPlateHLine = new Line(new Point3D(yTopPoint.X+ plateLength, eachYNext), new Point3D(yTopPoint.X-plateLength, eachYNext));
                plateUpperHorizontalLineList.Add(newPlateHLine);
            }



            List<Line> plateUpperNewList = new List<Line>();
            List<Entity> PlateUpperCenterLineList = new List<Entity>();

            Line eachPlateBefore = null;
            Line eachHLineBeforeT = null;
            Line eachHLineBeforeB = null;
            Point3D eachTopPoint = GetSumPoint(yTopPlatePoint,0,0);
            Point3D eachBottomPoint = null;
            double plateOffsetValue = 0;
            DrawCenterLineModel newCenterModel = new DrawCenterLineModel();
            // 겹치는 부분 자르기
            for (int i = 0; i < plateUpperList.Count ; i++)
            {
                Line eachPlate = plateUpperList[i];
                double offsetValue = 0;
                if (arrangeType == PlateArrange_Type.Roof)
                    offsetValue = plateOffsetValue;
                else
                    offsetValue = -plateOffsetValue;

                Line eachPlateOffset = (Line)eachPlate.Offset(offsetValue, Vector3D.AxisZ);

                // offset
                Line eachHLine = plateUpperHorizontalLineList[i];
                Line eachHLineT = (Line)eachHLine.Offset(+plateOverlapGap, Vector3D.AxisZ);
                Line eachHLineB = (Line)eachHLine.Offset(-plateOverlapGap, Vector3D.AxisZ);

                // 중심선 추가
                Point3D middlePoint = editingService.GetIntersectWidth(eachHLine, eachPlateOffset, 0);
                Circle middleCircle = new Circle(GetSumPoint(middlePoint, 0, 0), plateOverlapGap);
                Point3D[] middleInterPoint = middleCircle.IntersectWith(eachHLine);
                if (middleInterPoint.Length > 0)
                {
                    if(i<plateUpperList.Count-1)
                        PlateUpperCenterLineList.AddRange(
                            editingService.GetCenterLine(GetSumPoint(middleInterPoint[0], 0, 0), 
                                                         GetSumPoint(middleInterPoint[1], 0, 0), 
                                                         newCenterModel.detailCenterLength, scaleValue));
                    //                        PlateUpperCenterLineList.Add(new Line(GetSumPoint(middleInterPoint[0], 0, 0), GetSumPoint(middleInterPoint[1],0,0)));
                }

                // 하단 절단
                if (i == plateUpperList.Count - 1)
                {
                    // 마지막은 중심선
                    eachBottomPoint = editingService.GetIntersectWidth(eachHLine, eachPlateOffset, 0);
                }
                else
                {
                    eachBottomPoint = editingService.GetIntersectWidth(eachHLineB, eachPlateOffset, 0);
                }

                // 상단 절단
                if (eachHLineBeforeT != null)
                {
                    eachTopPoint = editingService.GetIntersectWidth(eachHLineBeforeT, eachPlateOffset, 0);
                }

                // 라인 추가
                Line eachNewLine = new Line(GetSumPoint(eachTopPoint, 0, 0), GetSumPoint(eachBottomPoint, 0, 0));
                plateUpperNewList.Add(eachNewLine);


                eachHLineBeforeT = eachHLineT;

                plateOffsetValue += scalePlateVerticalGap;
            }

            // Left
            // Mirror
            List<Entity> plateLowerNewList = new List<Entity>();
            foreach (Entity eachEntity in plateUpperNewList)
            {
                Entity tempLeftEntity = (Entity)eachEntity.Clone();
                Plane pl1 = Plane.XZ;
                pl1.Origin.X = yCenterPoint.X;
                pl1.Origin.Y = yCenterPoint.Y;
                Mirror customMirror = new Mirror(pl1);
                tempLeftEntity.TransformBy(customMirror);
                plateLowerNewList.Add(tempLeftEntity);

            }
            List<Entity> PlateLowerCenterLineList = new List<Entity>();
            foreach (Entity eachEntity in PlateUpperCenterLineList)
            {
                Entity tempLeftEntity = (Entity)eachEntity.Clone();
                Plane pl1 = Plane.XZ;
                pl1.Origin.X = yCenterPoint.X;
                pl1.Origin.Y = yCenterPoint.Y;
                Mirror customMirror = new Mirror(pl1);
                tempLeftEntity.TransformBy(customMirror);
                PlateLowerCenterLineList.Add(tempLeftEntity);

            }

            styleService.SetLayerListLine(ref plateUpperNewList, layerService.LayerOutLine);
            newList.AddRange(plateUpperNewList);
            styleService.SetLayerListLine(ref PlateUpperCenterLineList, layerService.LayerCenterLine);
            newList.AddRange(PlateUpperCenterLineList);

            styleService.SetLayerListLine(ref plateLowerNewList, layerService.LayerOutLine);
            newList.AddRange(plateLowerNewList);
            styleService.SetLayerListLine(ref PlateLowerCenterLineList, layerService.LayerCenterLine);
            newList.AddRange(PlateLowerCenterLineList);


            styleService.SetLayerListLine(ref plateEtcList, layerService.LayerVirtualLine);
            newList.AddRange(plateEtcList);
            // circleExtendValue
            // true : Bottom : 마지막 평평하게


            
            return newList;
        }

        private List<Entity> DrawVerticalPlate(double plateLength, double plateWidth, Point3D centerPoint, double startX, List<Point3D> beforePoint, Circle outCircle)
        {
            List<Entity> newList = new List<Entity>();

            // Option
            double plateSideMinSpacing = 350;
            double shellSideMinSpacing = 800;
            double maxLengthFactor = 100;

            double verticalHeight = plateLength;

            double currentX = 0;
            double currentXFactor = 0;

            bool mirrorValue = false;
            bool centerCopy = true;

            double upperY = centerPoint.Y+verticalHeight;
            double lowerY = centerPoint.Y;

            // Center 
            if (startX == centerPoint.X)
                centerCopy = false;

            Line cLine = new Line(new Point3D(startX,lowerY), new Point3D(startX,upperY));
            newList.Add(GetLineByTrim(cLine,outCircle));


            // Right
            int vCount = 0;
            double maxX = beforePoint[beforePoint.Count-1].X;
            double minX = beforePoint[0].X;

            currentX = startX + plateWidth;
            currentXFactor = currentX + shellSideMinSpacing;
            while (currentXFactor < maxX)
            {
                vCount++;
                Line rightLine = new Line(new Point3D(currentX, lowerY), new Point3D(currentX, upperY));
                newList.Add(GetLineByTrim(rightLine, outCircle));
                currentX += plateWidth;
                currentXFactor = currentX + shellSideMinSpacing;
            }

            // Start : Arrange
            if (vCount > 0)
            {
                // Delete Value
                // 마지막을 그리지 않았기 때문에
                bool deleteSign = true;
                if ((maxX - shellSideMinSpacing) < currentX && currentX <= maxX)
                    deleteSign = false;

                if ((maxX - shellSideMinSpacing) < currentX && currentX <= maxX)
                {
                    List<Line> addPlateHLineList = new List<Line>();
                    double arrangeCount = 0;
                    int vCurrentCount = newList.Count-1;
                    while (vCurrentCount >= 0)
                    {
                        arrangeCount++;

                        Line lastLine1 = (Line)newList[vCurrentCount];
                        double lastLineHeight1 = lastLine1.Length();
                        double plateWidthSum = plateWidth * arrangeCount;
                        // 마지막 선 높이 비교
                        if (lastLineHeight1 > plateWidthSum)
                        {
                            double remainingHeight = lastLineHeight1 - plateWidthSum;
                            // 남은 높이를 shellSideMinSpacing 와 비교
                            if (remainingHeight > shellSideMinSpacing)
                            {
                                // 한칸 밀어 넣기 -> 
                                Line hLineTemp1 = new Line(GetSumPoint(lastLine1.StartPoint, 0, plateWidthSum), GetSumPoint(lastLine1.StartPoint, plateLength * 10, plateWidthSum));
                                addPlateHLineList.Add(hLineTemp1);

                                // 전체 밀기
                                foreach (Line eachLine in addPlateHLineList)
                                {
                                    eachLine.StartPoint = GetSumPoint(lastLine1.StartPoint, 0, plateWidthSum);
                                }

                                // 종료
                                break;
                            }
                            else
                            {
                                // 기존 것 마지막 꺼 삭제
                                if(vCurrentCount>=2)
                                    newList.RemoveAt(newList.Count-1);
                                // 밀어 넣기 -> 
                                vCurrentCount--;
                                arrangeCount--;

                            }


                        }
                        else
                        {
                            // 마지막 선 높이가 Plate Width 보다 작으면 그리지 않는다. -> 끝

                            // 그냥 줄이기
                            for (int i = newList.Count - 1; i > 1; i--)
                            {
                                Line templastPreviousLine = (Line)newList[i - 1];
                                Line templastLine = (Line)newList[i];

                                // 길이가 작아야 하고
                                if (maxX - templastPreviousLine.StartPoint.X < plateLength)
                                {
                                    // 큰 거 만나면 종료
                                    if (templastPreviousLine.Length() < plateWidth)
                                    {
                                        if (templastLine.Length() < plateWidth)
                                            newList.RemoveAt(i);
                                    }
                                    else
                                    {
                                        break;
                                    }


                                }
                                else
                                {
                                    break;
                                }


                            }

                            // 종료
                            break;


                        }

                    }



                    // 추가
                    if (addPlateHLineList.Count > 0)
                    {
                        foreach(Line eachLine in addPlateHLineList)
                        {
                            newList.Add(GetLineByTrim(eachLine, outCircle));
                        }
                    }
                }
                else
                {
                    // 그냥 줄이기
                    for (int i = newList.Count - 1; i > 1; i--)
                    {
                        Line templastPreviousLine = (Line)newList[i - 1];
                        Line templastLine = (Line)newList[i];

                        // 길이가 작아야 하고
                        if (maxX - templastPreviousLine.StartPoint.X < plateLength)
                        {
                            // 큰 거 만나면 종료
                            if (templastPreviousLine.Length() < plateWidth)
                            {
                                if (templastLine.Length() < plateWidth)
                                    newList.RemoveAt(i);
                            }
                            else
                            {
                                break;
                            }


                        }
                        else
                        {
                            break;
                        }


                    }

                }
            }


            // Left
            // Mirror
            List<Entity> tempRightList = new List<Entity>();
            foreach(Entity eachEntity in newList)
            {
                Entity tempLeftEntity = (Entity)eachEntity.Clone();
                Plane pl1 = Plane.YZ;
                pl1.Origin.X = centerPoint.X;
                pl1.Origin.Y = centerPoint.Y;
                Mirror customMirror = new Mirror(pl1);
                tempLeftEntity.TransformBy(customMirror);
                tempRightList.Add(tempLeftEntity);

            }
            if (!centerCopy)
                tempRightList.RemoveAt(0);

            newList.AddRange(tempRightList);

            return newList;
        }

        private List<Entity> GetLineByTrimBoth(List<Entity> selList, Circle outCircle)
        {
            List<Entity> newList = new List<Entity>();
            foreach(Line eachLine in selList)
            {
                Point3D[] interPoint = eachLine.IntersectWith(outCircle);
                if (interPoint.Length > 1)
                {
                    newList.Add(new Line(GetSumPoint(interPoint[0],0,0),GetSumPoint(interPoint[1],0,0)));
                }
                else
                {
                    newList.Add((Line)eachLine.Clone());
                }
            }

            return newList;
        }
        private Line GetLineByTrim(Line selLine, Circle outCircle)
        {
            Point3D[] interPoint=selLine.IntersectWith(outCircle);
            if (interPoint.Length > 0)
            {
                return new Line(selLine.StartPoint, interPoint[0]);
            }
            else
            {
                return (Line)selLine.Clone();
            }
        }


        private List<Entity> DrawHorizontalPlate(double plateLength,double plateWidth,Point3D centerPoint,double startX,double upperY, double lowerY, 
                                                List<Point3D> selUpperPoint, List<Point3D> selLowerPoint,
                                                ref List<Point3D> beforePoint,
                                                out List<Point3D> currentPoint,
                                                ref List<double> addPlateList,
                                                bool mirrorValue=true)
        {

            List<Entity> newList = new List<Entity>();

            currentPoint = new List<Point3D>();

            // Option
            double shellSideMinSpacing = 800;

            double maxLengthFactor = 100;

            // adjLine
            Line rightLastLine = null;
            Line leftLastLine = null;

            double maxX = selUpperPoint[1].X;
            double maxRightX = selLowerPoint[1].X;
            if (maxX > maxRightX)
            {
                maxX = selLowerPoint[1].X; ;
                maxRightX = selUpperPoint[1].X;
            }
            double minX = selUpperPoint[0].X;
            double minLeftX = selLowerPoint[0].X;
            if (minX < minLeftX)
            {
                minX = selLowerPoint[0].X;
                minLeftX = selUpperPoint[0].X;
            }


            // Center Line : arrange
            bool customCopy = false; 
            if ((minX + shellSideMinSpacing) >= startX && startX >= minLeftX)
            {
                startX = startX + 900;
                customCopy = true;

            }


            // Center 
            Line cLine = new Line(new Point3D(startX, upperY), new Point3D(startX, lowerY));
            newList.Add(cLine);
            if (customCopy)
            {
                Entity cLineCopy = (Entity)cLine.Clone();
                editingService.SetMirrorEntity(Plane.YZ, ref cLineCopy, centerPoint);
                newList.Add(cLineCopy);
            }

            currentPoint.Add(new Point3D(startX, upperY));

            double currentX = 0;
            double currentXFactor = 0;

            // Right
            currentX = startX + plateLength;
            currentXFactor = currentX + shellSideMinSpacing;
            while (currentXFactor < maxX)
            {
                Line rightLine = new Line(new Point3D(currentX, upperY), new Point3D(currentX, lowerY));
                newList.Add(rightLine);
                currentPoint.Add(new Point3D(currentX, upperY));
                currentX += plateLength;
                currentXFactor = currentX + shellSideMinSpacing;
            }

            // Start : Arrange
            if ((maxX - shellSideMinSpacing) <= currentX && currentX <= maxRightX)
            {
                double beforeX = currentX - plateLength;
                double rangeLeftX = maxRightX - (plateLength + maxLengthFactor);
                double rangeRightX = maxX - shellSideMinSpacing - 10;
                double rangeLength = rangeRightX - rangeLeftX;

                double optLength = GetOptimizationPlateLength(POSITION_TYPE.RIGHT, ref beforePoint, ref addPlateList,centerPoint,
                                                             beforeX, rangeLeftX,rangeRightX, plateWidth,plateLength);
                if (optLength > 0)
                {
                    beforeX += optLength;
                    rightLastLine = new Line(new Point3D(beforeX, upperY), new Point3D(beforeX, lowerY));
                    newList.Add(rightLastLine);
                    currentPoint.Add(new Point3D(beforeX, upperY));
                }
            }


            // Left

            currentX = startX - plateLength;
            currentXFactor = currentX - shellSideMinSpacing;
            while (currentXFactor > minX)
            {
                Line leftLine = new Line(new Point3D(currentX, upperY), new Point3D(currentX, lowerY));
                newList.Add(leftLine);
                currentPoint.Add(new Point3D(currentX, upperY));
                currentX -= plateLength;
                currentXFactor = currentX - shellSideMinSpacing;
            }

            if (mirrorValue)
            {
                // Copy : Mirror : Left
                if (rightLastLine != null)
                {
                    leftLastLine = (Line)rightLastLine.Clone();
                    Plane pl1 = Plane.YZ;
                    pl1.Origin.X = centerPoint.X;
                    pl1.Origin.Y = centerPoint.Y;
                    Mirror customMirror = new Mirror(pl1);
                    leftLastLine.TransformBy(customMirror);
                    newList.Add(leftLastLine);
                    currentPoint.Add(GetSumPoint(leftLastLine.StartPoint,0,0));
                }
            }
            else
            {
                // Start : Arrange
                if ((minX + shellSideMinSpacing) >= currentX && currentX >= minLeftX)
                {
                    double beforeX = currentX + plateLength;
                    double rangeLeftX = minX + shellSideMinSpacing + 10;
                    double rangeRightX = minLeftX+ (plateLength + maxLengthFactor);
                    double rangeLength = rangeRightX - rangeLeftX;

                    double optLength = GetOptimizationPlateLength(POSITION_TYPE.RIGHT, ref beforePoint, ref addPlateList,centerPoint,
                                                                beforeX,rangeLeftX,rangeRightX, plateWidth,plateLength);
                    if (optLength > 0)
                    {
                        beforeX -= optLength;
                        leftLastLine = new Line(new Point3D(beforeX, upperY), new Point3D(beforeX, lowerY));
                        newList.Add(leftLastLine);
                        currentPoint.Add(new Point3D(beforeX, upperY));
                    }
                }
            }

            return newList;
        }

        // AddPlate Length : Check
        private double GetOptimizationPlateLength(POSITION_TYPE selPosition, 
                                                    ref List<Point3D> beforePoint, ref List<double> addPlateList,Point3D centerPoint,
                                                    double beforeX, double rangeLeftX, double rangeRightX , double plateWidth,double plateLength)
        {

            double maxLength = 0;
            double minLength = 0;
            if (selPosition == POSITION_TYPE.RIGHT)
            {
                maxLength = rangeRightX - beforeX;
                minLength = rangeLeftX - beforeX;
            }
            else if (selPosition == POSITION_TYPE.LEFT)
            {
                maxLength = beforeX - rangeLeftX;
                minLength = beforeX - rangeRightX;
            }
            double optLength = maxLength;

            List<double> optSizeCaseList = new List<double>();

            switch (plateWidth)
            {
                case 2438 - 30:
                    goto default;
                case 1524 - 30:
                    if (beforeX == centerPoint.X)
                    {
                        // 역방향
                        optSizeCaseList.Add(maxLength);                     // Case 5 : Plate Max
                        optSizeCaseList.Add(plateWidth * 4);                // Case 4 : Plate Width * 3
                                                                            //optSizeCaseList.Add(plateWidth + plateWidth / 2);   // Case 3 : plate Width + Plate Width / 2
                                                                            //optSizeCaseList.Add(plateLength / 2);               // Case 3 : plate Length / 2
                        optSizeCaseList.Add(plateWidth * 2 + plateWidth / 2);                // Case 2 : Plate Width * 2
                        optSizeCaseList.Add(plateWidth * 2);                // Case 2 : Plate Width * 2
                        optSizeCaseList.Add(plateWidth);                    // Case 1 : Plate Width



                    }
                    else
                    {
                        optSizeCaseList.Add(plateWidth);                    // Case 1 : Plate Width
                        optSizeCaseList.Add(plateWidth * 2);                // Case 2 : Plate Width * 2
                        optSizeCaseList.Add(plateWidth * 2 + plateWidth / 2);                // Case 2 : Plate Width * 2
                                                                                             //optSizeCaseList.Add(plateLength / 2);               // Case 3 : plate Length / 2
                        optSizeCaseList.Add(plateWidth * 4);                // Case 4 : Plate Width * 3
                        optSizeCaseList.Add(maxLength);                     // Case 5 : Plate Max
                    }
                    break;
                default:
                    if (beforeX == centerPoint.X)
                    {
                        // 역방향
                        optSizeCaseList.Add(maxLength);                     // Case 5 : Plate Max
                        optSizeCaseList.Add(plateWidth * 3);                // Case 4 : Plate Width * 3
                                                                            //optSizeCaseList.Add(plateWidth + plateWidth / 2);   // Case 3 : plate Width + Plate Width / 2
                                                                            //optSizeCaseList.Add(plateLength / 2);               // Case 3 : plate Length / 2
                        optSizeCaseList.Add(plateWidth * 2 + plateWidth / 3);                // Case 2 : Plate Width * 2
                        optSizeCaseList.Add(plateWidth * 2);                // Case 2 : Plate Width * 2
                        optSizeCaseList.Add(plateWidth);                    // Case 1 : Plate Width



                    }
                    else
                    {
                        optSizeCaseList.Add(plateWidth);                    // Case 1 : Plate Width
                        optSizeCaseList.Add(plateWidth * 2);                // Case 2 : Plate Width * 2
                        optSizeCaseList.Add(plateWidth * 2 + plateWidth / 3);                // Case 2 : Plate Width * 2
                                                                                             //optSizeCaseList.Add(plateLength / 2);               // Case 3 : plate Length / 2
                        optSizeCaseList.Add(plateWidth + plateWidth / 2);   // Case 3 : plate Width + Plate Width / 2
                        optSizeCaseList.Add(plateWidth * 3);                // Case 4 : Plate Width * 3
                        optSizeCaseList.Add(maxLength);                     // Case 5 : Plate Max
                    }
                    break;

            }



            foreach(double eachLength in optSizeCaseList)
            {
                if (eachLength > minLength)
                {
                    if (eachLength < maxLength)
                    {
                        if (CheckWeldingSeamOverlap(selPosition, ref beforePoint, eachLength, beforeX))
                        {
                            optLength = eachLength;
                            addPlateList.Add(optLength);
                            break;
                        }
                    }
                }

            }

            return optLength;
        }
        // Chekck Welding Overlap
        private bool CheckWeldingSeamOverlap(POSITION_TYPE selPosition, ref List<Point3D> beforePoint, double selLength,double beforeX)
        {
            bool returnValue = true;
            double plateSideMinSpacing = 350;
            double minX = 0;
            double maxX= 0;

            if(selPosition == POSITION_TYPE.RIGHT)
            {
                minX = beforeX + selLength - plateSideMinSpacing;
                maxX = beforeX + selLength + plateSideMinSpacing;
            }
            else if (selPosition == POSITION_TYPE.LEFT)
            {
                minX = beforeX - selLength - plateSideMinSpacing;
                maxX = beforeX - selLength + plateSideMinSpacing;
            }

            foreach(Point3D eachPoint in beforePoint)
            {
                double eachPointX = eachPoint.X;
                if(minX<eachPointX && eachPointX < maxX)
                {
                    returnValue = false;
                    break;
                }
            }

            return returnValue;
        }

        // Horizontal Line : Intersect -> Sort -> List
        private List<Point3D> GetHorizontalLineInter(Line selLine,Circle selCircle)
        {
            Point3D[] eachInterPoint = selLine.IntersectWith(selCircle);
            return eachInterPoint.OrderBy(x => x.X).ToList();
        }

        // Start X
        private double GetStartXValue(PlateArrange_Type arrangeType,double beforeNumber,double plateLength)
        {
            double returnValue = 0;
            double leftValue = -plateLength / 2;
            if (arrangeType == PlateArrange_Type.Roof)
            {
                if (beforeNumber == -1)
                {
                    returnValue = -2000;
                }
                else if (beforeNumber == 0)
                {
                    returnValue = leftValue;
                }
                else
                {
                    returnValue = 0;
                }
            }
            else if (arrangeType == PlateArrange_Type.Bottom)
            {
                if (beforeNumber == -1)
                {
                    returnValue = leftValue;
                }
                else if (beforeNumber == 0)
                {
                    returnValue = leftValue;
                }
                else
                {
                    returnValue = 0;
                }
            }
            return returnValue;
        }

        private double GetStartYValue(PlateArrange_Type arrangeType, double plateWidth)
        {
            double returnValue = 0;
            if (arrangeType == PlateArrange_Type.Roof)
            {
                return 0;
            }
            else if (arrangeType == PlateArrange_Type.Bottom)
            {
                return plateWidth / 2;
            }
            return returnValue;
        }
        // Start Y
        private double GetStartVerticalYValue(double beforeX,double plateWidth)
        {
            double returnValue = 0;

            if (beforeX == 0)
            {
                // Case : 1
                //if(plateWidth==2408)
                    //returnValue = 0;
                //else
                    returnValue = -plateWidth / 2;
            }
            else
            {
                // Case : 1
                //if(plateWidth==1524)
                    returnValue = -plateWidth / 2;
                //else

                // Case : 2
                //returnValue = 0;
            }

            return returnValue;
        }

        // Virtual : Rectangle : Length
        private double GetVirtualRectLength(PlateArrange_Type arrangeType,double circleOD, double plateWidth,double plateLength)
        {
            double cirOD = circleOD;
            if (arrangeType == PlateArrange_Type.Bottom)
                cirOD -= plateWidth ;

            double cirRadius = cirOD / 2;
            double plateMaxCount = Math.Ceiling(cirRadius / plateWidth);

            double lastPlateLength = GetLastPlateLength(plateWidth, circleOD);


            double lastPlateCount =Math.Truncate(lastPlateLength / plateWidth);

            // Roof : 반지름에 최소한 한개가 들어가야 함
            if (cirRadius - plateWidth <= plateWidth)
                lastPlateCount = 0;

            double plateVirtualCount = plateMaxCount - lastPlateCount;

            double newLength = plateWidth*(plateVirtualCount-1) *2;

            double lengthFactor = 0;
            if (arrangeType == PlateArrange_Type.Roof)
            {
                lengthFactor = 10;
            }
            else if (arrangeType == PlateArrange_Type.Bottom)
            {
                lengthFactor = 10;
                lengthFactor = plateWidth  +  10;
            }


                return newLength+ lengthFactor;
        }

        private double GetLastPlateLength(double plateWidth,double circleOD)
        {
            double returnValue = 0;

            if (circleOD < 13000)
            {
                returnValue = 2400 * plateWidth / 2408;
            }
            else
            {
                returnValue = 4000 * plateWidth / 2408;
            }
            return Math.Ceiling(returnValue);
        }


        public List<Entity> GetBottomPlateJoint(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue, out Dictionary<string, List<Entity>> selDim)
        {
            List<Entity> newList = new List<Entity>();
            selDim = new Dictionary<string, List<Entity>>();




            return newList;
        }




        public double GetRoofOD()
        {
            double retrunValue = 0;

            string topAngleType = assemblyData.RoofCompressionRing[0].CompressionRingType;
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankInRadius = tankID / 2;

            double actualRadius = tankInRadius;
            AngleSizeModel eachAngle = new AngleSizeModel();
            if (assemblyData.RoofAngleOutput.Count > 0)
                eachAngle = assemblyData.RoofAngleOutput[0];

            string roofSlopeString = assemblyData.RoofCompressionRing[0].RoofSlope;
            double roofSlopeDegree = valueService.GetDegreeOfSlope(roofSlopeString);

            int maxCourse = assemblyData.ShellOutput.Count - 1;

            string tankRoofType = assemblyData.GeneralDesignData[0].RoofType;

            if (tankRoofType.ToLower() == "crt")
            {
                switch (topAngleType)
                {
                    case "Detail b":
                        actualRadius += valueService.GetDoubleValue(eachAngle.E) +
                                        valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness);
                        retrunValue = valueService.GetHypotenuseByWidth(roofSlopeDegree, actualRadius) * 2;
                        break;
                    case "Detail d":
                        actualRadius += valueService.GetDoubleValue(eachAngle.E);
                        retrunValue = valueService.GetHypotenuseByWidth(roofSlopeDegree, actualRadius) * 2;
                        break;
                    case "Detail e":
                        actualRadius += valueService.GetDoubleValue(eachAngle.E) +
                                        valueService.GetDoubleValue(eachAngle.t);
                        retrunValue = valueService.GetHypotenuseByWidth(roofSlopeDegree, actualRadius) * 2;
                        break;
                    case "Detail i":
                        double tempDetaili = GetRoofODByCompressionRingDetailI();
                        double A = valueService.GetDoubleValue(assemblyData.RoofCRTInput[0].DetailIOutsideProjection);
                        double B = valueService.GetDoubleValue(assemblyData.RoofCRTInput[0].DetailIWidth);
                        double C = valueService.GetDoubleValue(assemblyData.RoofCRTInput[0].DetailIOverlap);
                        double insideX = B - C;

                        retrunValue = tempDetaili + (insideX) * 2;
                        break;
                    case "Detail k":
                        double maxCourseThk = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness);
                        double maxCourseBeforeThk = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse - 1].Thickness);
                        double t1Width = 0;
                        if (maxCourseThk > maxCourseBeforeThk)
                            t1Width = (maxCourseThk - maxCourseBeforeThk) / 2;
                        actualRadius += -t1Width;
                        retrunValue = valueService.GetHypotenuseByWidth(roofSlopeDegree, actualRadius) * 2;
                        break;
                }
            }
            else if (tankRoofType.ToLower() == "drt")
            {

            }

            return retrunValue;
        }
        public double GetRoofODByCompressionRingDetailI()
        {
            double retrunValue = 0;
            string topAngleType = assemblyData.RoofCompressionRing[0].CompressionRingType;
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankInRadius = tankID / 2;

            double actualRadius = tankInRadius;

            int maxCourse = assemblyData.ShellOutput.Count - 1;

            switch (topAngleType)
            {
                case "Detail i":
                    string roofSlopeString = assemblyData.RoofCompressionRing[0].RoofSlope;
                    double roofSlopeDegree = valueService.GetDegreeOfSlope(roofSlopeString);

                    double A = valueService.GetDoubleValue(assemblyData.RoofCRTInput[0].DetailIOutsideProjection);
                    double B = valueService.GetDoubleValue(assemblyData.RoofCRTInput[0].DetailIWidth);
                    double C = valueService.GetDoubleValue(assemblyData.RoofCRTInput[0].DetailIOverlap);
                    double t1 = valueService.GetDoubleValue(assemblyData.RoofCRTInput[0].DetailIThickness);

                    double insideX = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, B - A - C);
                    double insideY = valueService.GetOppositeByHypotenuse(roofSlopeDegree, B - A - C);
                    double thicknessY = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, t1);
                    double thickneesX = valueService.GetOppositeByHypotenuse(roofSlopeDegree, t1);

                    actualRadius += - valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness)
                                    - thickneesX
                                    + insideX;

                    retrunValue = valueService.GetHypotenuseByWidth(roofSlopeDegree, actualRadius) *2;
                    break;

                default:
                    retrunValue = GetRoofOD();
                    break;
            }

            return retrunValue;
        }

        public double GetBottomOD()
        {
            double retrunValue = 0;
            double bottomOD = GetBottomPlateOD();
            double annularOD = GetAnnularPlateOD();
            double annularID = GetAnnularPlateID();
            string annularYes = assemblyData.BottomInput[0].AnnularPlate;

            if (annularYes.Contains("Yes"))
            {
                retrunValue = annularOD;
            }
            else
            {
                retrunValue = bottomOD;
            }
            return retrunValue;
        }
        public double GetBottomPlateOD()
        {
            return valueService.GetDoubleValue(assemblyData.BottomInput[0].OD);
        }
        public double GetAnnularPlateOD()
        {
            return valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateOD);
        }
        public double GetAnnularPlateID()
        {
            return valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateID);
        }
        public double GetAnnularPlateBottomOD()
        {
            return valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateID) + publicService.GetDetailAnnularOverlap()*2;
        }

        public double GetBottomRoofOD(double selRoofOD=0, double selBottomOD=0)
        {

            if (selRoofOD == 0) 
                selRoofOD = GetRoofOD();
            if (selBottomOD == 0) 
                selBottomOD = GetBottomOD();

            double retrunValue = selRoofOD;
            if (retrunValue < selBottomOD)
                retrunValue = selBottomOD;

            return retrunValue;
        }


        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }






























        // Custom Arrangement
        public bool GetCustomArrangement(PlateArrange_Type arrangeType, bool circleExtendValue, double circleRadius,
                                                double plateWidth, double plateLength,
                                                Point3D centerPoint, double scaleValue, out List<Entity> plateList)
        {
            bool returnValue = false;
            

            List<Entity> hLineList = new List<Entity>();
            List<Entity> hCenterLineList = new List<Entity>();
            List<Entity> vLineList = new List<Entity>();
            List<Entity> vCenterLineList = new List<Entity>();

            double circleOD = circleRadius * 2;
            double hLineLength = circleOD + 200;
            double hLineLengthRadius = hLineLength / 2;
            double plateWidthHalf = plateWidth / 2;

            Circle outCircle = new Circle(Plane.XY,GetSumPoint(centerPoint,0,0) , circleRadius);

            if (arrangeType == PlateArrange_Type.Roof)
            {
                if (circleOD < 5000)
                {
                    returnValue = true;//초기화

                }
                else if (5000 <= circleOD && circleOD <= 7224)
                {
                    returnValue = true;//초기화
                    hLineList.Add(new Line(GetSumPoint(centerPoint, -hLineLengthRadius, plateWidthHalf), GetSumPoint(centerPoint, hLineLengthRadius, plateWidthHalf)));
                }
                else if (7225 <= circleOD && circleOD <= 9144)
                {
                    returnValue = true;//초기화
                    hLineList.Add(new Line(GetSumPoint(centerPoint, -hLineLengthRadius, plateWidth), GetSumPoint(centerPoint, hLineLengthRadius, plateWidth)));
                    hCenterLineList.Add(new Line(GetSumPoint(centerPoint, -hLineLengthRadius, 0), GetSumPoint(centerPoint, hLineLengthRadius, 0)));
                }
                else if (9145 <= circleOD && circleOD <= 11231)
                {
                    returnValue = false;//초기화
                }
                else if (11232 <= circleOD && circleOD <= 13280)
                {
                    returnValue = false;//초기화
                }
                else if (13281 <= circleOD && circleOD <= 14480)
                {
                    returnValue = false;//초기화
                }
            }
            else if (arrangeType == PlateArrange_Type.Bottom)
            {
                if (circleOD < 5000)
                {
                    returnValue = true;//초기화

                }
                else if (5000 <= circleOD && circleOD <= 7224)
                {
                    returnValue = true;//초기화
                    hLineList.Add(new Line(GetSumPoint(centerPoint, -hLineLengthRadius, plateWidthHalf), GetSumPoint(centerPoint, hLineLengthRadius, plateWidthHalf)));
                }
                else if (7225 <= circleOD && circleOD <= 9144)
                {
                    returnValue = true;//초기화
                    hLineList.Add(new Line(GetSumPoint(centerPoint, -hLineLengthRadius, plateWidth), GetSumPoint(centerPoint, hLineLengthRadius, plateWidth)));
                    hCenterLineList.Add(new Line(GetSumPoint(centerPoint, -hLineLengthRadius, 0), GetSumPoint(centerPoint, hLineLengthRadius, 0)));
                }
                else if (9145 <= circleOD && circleOD <= 11231)
                {
                    returnValue = false;//초기화
                }
                else if (11232 <= circleOD && circleOD <= 13280)
                {
                    returnValue = false;//초기화
                }
                else if (13281 <= circleOD && circleOD <= 14480)
                {
                    returnValue = false;//초기화
                }
            }

            plateList = new List<Entity>();
            if (returnValue)
            {
                // Bottom : Copy or Mirror
                if (arrangeType == PlateArrange_Type.Roof)
                {
                    // 180 회전
                    double rotateValue = Utility.DegToRad(180);
                    plateList.AddRange(GetLineByTrimBoth(hCenterLineList, outCircle));
                    plateList.AddRange(GetLineByTrimBoth(hLineList, outCircle));
                    plateList.AddRange(GetLineByTrimBoth(editingService.GetRotate(hLineList, GetSumPoint(centerPoint, 0, 0), rotateValue), outCircle));

                    plateList.AddRange(vCenterLineList);
                    plateList.AddRange(vLineList);
                    plateList.AddRange(editingService.GetRotate(vLineList, GetSumPoint(centerPoint, 0, 0), rotateValue));

                }
                // Bottom : Copy or Mirror
                if (arrangeType == PlateArrange_Type.Bottom)
                {
                    // 180 회전
                    double rotateValue = Utility.DegToRad(180);
                    plateList.AddRange(GetLineByTrimBoth(hCenterLineList, outCircle));
                    plateList.AddRange(GetLineByTrimBoth(hLineList, outCircle));
                    plateList.AddRange(GetLineByTrimBoth(editingService.GetRotate(hLineList, GetSumPoint(centerPoint, 0, 0), rotateValue), outCircle));

                    plateList.AddRange(vCenterLineList);
                    plateList.AddRange(vLineList);
                    plateList.AddRange(editingService.GetRotate(vLineList, GetSumPoint(centerPoint, 0, 0), rotateValue));

                }
            }

            return returnValue;
        }






    }
}
