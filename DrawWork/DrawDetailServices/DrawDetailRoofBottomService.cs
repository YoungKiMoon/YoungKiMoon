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
        }

        public DrawEntityModel GetRoofArrange(ref CDPoint refPoint, ref CDPoint curPoint, double scaleValue)
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

            cirOD = 86000;
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

            drawEntity =GetPlateArrange(plateType, referencePoint, cirOD, cirODExtend, plateActualWidth, plateActualLength, scaleValue);

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




            // Draw Center Cut
            List<Entity> centerCutList = DrawCenterCutPlane(arrangeType, circleExtendValue, circleExtendRadius, plateLineList, plateHorizontalList, referencePoint, scaleValue);

            drawEntity.outlineList.AddRange(centerCutList);


            // Additional
            // Text
            foreach (Line eachLine in plateLineList)
            {
                textList.Add(new Text(eachLine.EndPoint, eachLine.EndPoint.X.ToString(), 200));
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


        private double GetRoofOuterDiameter()
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

            double distanceValue = 800;
            double scaleDistanceValue = distanceValue * scaleValue;

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




            Point3D yCenterPoint = GetSumPoint(centerPoint, circleLength + scaleDistanceValue, 0);
            Point3D yTopPoint = new Point3D(yCenterPoint.X, YList[0]);
            Point3D yBottomPoint = new Point3D(yCenterPoint.X, YList[YList.Count-1]);


            if (arrangeType == PlateArrange_Type.Bottom)
            {
                if (circleExtendValue)
                {
                    YList.RemoveAt(YList.Count - 1);
                    YList.RemoveAt(0);
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
            List<Line> PlateUpperCenterLineList = new List<Line>();

            Line eachPlateBefore = null;
            Line eachHLineBeforeT = null;
            Line eachHLineBeforeB = null;
            Point3D eachTopPoint = GetSumPoint(yTopPlatePoint,0,0);
            Point3D eachBottomPoint = null;
            double plateOffsetValue = 0;
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
                        PlateUpperCenterLineList.Add(new Line(GetSumPoint(middleInterPoint[0], 0, 0), GetSumPoint(middleInterPoint[1],0,0)));
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
            if (startX == 0)
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
            if(beforeX== centerPoint.X)
            {
                // 역방향
                optSizeCaseList.Add(maxLength);                     // Case 5 : Plate Max
                optSizeCaseList.Add(plateWidth * 3);                // Case 4 : Plate Width * 3
                //optSizeCaseList.Add(plateWidth + plateWidth / 2);   // Case 3 : plate Width + Plate Width / 2
                //optSizeCaseList.Add(plateLength / 2);               // Case 3 : plate Length / 2
                optSizeCaseList.Add(plateWidth * 2 + plateWidth /2);                // Case 2 : Plate Width * 2
                optSizeCaseList.Add(plateWidth * 2);                // Case 2 : Plate Width * 2
                optSizeCaseList.Add(plateWidth);                    // Case 1 : Plate Width



            }
            else
            {
                optSizeCaseList.Add(plateWidth);                    // Case 1 : Plate Width
                optSizeCaseList.Add(plateWidth * 2);                // Case 2 : Plate Width * 2
                optSizeCaseList.Add(plateWidth * 2 + plateWidth / 2);                // Case 2 : Plate Width * 2
                //optSizeCaseList.Add(plateLength / 2);               // Case 3 : plate Length / 2
                optSizeCaseList.Add(plateWidth + plateWidth / 2);   // Case 3 : plate Width + Plate Width / 2
                optSizeCaseList.Add(plateWidth * 3);                // Case 4 : Plate Width * 3
                optSizeCaseList.Add(maxLength);                     // Case 5 : Plate Max
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
                returnValue = -plateWidth / 2;
            }
            else
            {
                // Case : 1
                returnValue = -plateWidth / 2;

                // Case : 2
                returnValue = 0;
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
            
            double lastPlateLength = 4000;

            if (cirOD <13000)
                lastPlateLength = 2400;

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



        public List<Entity> GetBottomPlateJoint(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue, out Dictionary<string, List<Entity>> selDim)
        {
            List<Entity> newList = new List<Entity>();
            selDim = new Dictionary<string, List<Entity>>();




            return newList;
        }



        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }

    }
}
