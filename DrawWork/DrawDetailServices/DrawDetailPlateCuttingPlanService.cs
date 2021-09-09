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
    public class DrawDetailPlateCuttingPlanService
    {
        

        private Model singleModel;
        private AssemblyModel assemblyData;

        private DrawService drawService;


        private ValueService valueService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;

        private DrawEditingService editingService;

        private DrawShellCourses dsService;
        private DrawShapeServices shapeService;

        private DrawPublicFunctionService publicService;

        private DrawScaleService scaleService;

        public DrawDetailPlateCuttingPlanService(AssemblyModel selAssembly, object selModel)
        {
            singleModel = selModel as Model;

            assemblyData = selAssembly;

            drawService = new DrawService(selAssembly);

            valueService = new ValueService();
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();

            editingService = new DrawEditingService();

            dsService = new DrawShellCourses();
            shapeService = new DrawShapeServices();
            publicService = new DrawPublicFunctionService();

            scaleService = new DrawScaleService();
        }




        public DrawEntityModel DrawRoofCuttingPlan(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)

        {
            DrawEntityModel drawList = new DrawEntityModel();
            double plateActualWidth = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].PlateWidth);
            double plateActualLength = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].PlateLength);

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);
            // Scale : 매우 중요 함
            double realScaleValue = scaleService.GetScaleCalValue(135, 50, plateActualLength, plateActualWidth);

            drawList.AddDrawEntity(DrawCuttingPlan(referencePoint, SingletonData.RoofPlateInfo, plateActualWidth, plateActualLength, realScaleValue));
            return drawList;
        }

        public DrawEntityModel DrawBottomCuttingPlan(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)

        {
            DrawEntityModel drawList = new DrawEntityModel();
            double plateActualWidth = valueService.GetDoubleValue(assemblyData.BottomInput[0].PlateWidth);
            double plateActualLength = valueService.GetDoubleValue(assemblyData.BottomInput[0].PlateLength);

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);
            // Scale : 매우 중요 함
            double realScaleValue = scaleService.GetScaleCalValue(135, 50, plateActualLength, plateActualWidth);

            drawList.AddDrawEntity(DrawCuttingPlan(referencePoint, SingletonData.BottomPlateInfo, plateActualWidth, plateActualLength, realScaleValue));
            return drawList;
        }


        public void SetPlateAddLength(List<DrawPlateModel> selList)
        {
            double fullLengthFactor = 30;
            double hallfLengthFactor = fullLengthFactor / 2;
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
        public void SetCuttingInfo(List<DrawPlateModel> selPlateList,double plateWidth, double plateLength)
        {
            // 가로로 배열한다고 가정 함
            // Max Length, Min Length, Height, Line Spacing
            foreach (DrawPlateModel eachPlate in selPlateList)
            {
                if (eachPlate.PlateDirection == PERPENDICULAR_TYPE.Horizontal)
                {
                    eachPlate.CuttingPlan.MinLength = valueService.GetMinValueOfList(eachPlate.HLength);
                    eachPlate.CuttingPlan.MaxLength = valueService.GetMaxValueOfList(eachPlate.HLength);
                    eachPlate.CuttingPlan.Width = valueService.GetMaxValueOfList(eachPlate.VLength);
                }
                else if (eachPlate.PlateDirection == PERPENDICULAR_TYPE.Vertical)
                {
                    eachPlate.CuttingPlan.MinLength = valueService.GetMinValueOfList(eachPlate.VLength);
                    eachPlate.CuttingPlan.MaxLength = valueService.GetMaxValueOfList(eachPlate.VLength);
                    eachPlate.CuttingPlan.Width = valueService.GetMaxValueOfList(eachPlate.HLength);
                }

                switch (eachPlate.ShapeType)
                {
                    case Plate_Type.Segment:
                        eachPlate.CuttingPlan.Width = valueService.GetLengthBetweenStringAndArc(eachPlate.Radius, eachPlate.CuttingPlan.MaxLength);
                        eachPlate.CuttingPlan.LengthBetweenStringAndArc = valueService.GetLengthBetweenStringAndArc(eachPlate.Radius, eachPlate.CuttingPlan.MaxLength);
                        break;

                    case Plate_Type.Arc:
                        if (eachPlate.CuttingPlan.MaxLength > eachPlate.CuttingPlan.Width && eachPlate.CuttingPlan.MaxLength <= plateWidth)
                        {
                            double tempValue = eachPlate.CuttingPlan.Width;
                            eachPlate.CuttingPlan.Width = eachPlate.CuttingPlan.MaxLength;
                            eachPlate.CuttingPlan.MaxLength = tempValue;
                        }
                        eachPlate.CuttingPlan.MinLength =0;
                        eachPlate.CuttingPlan.LengthSpacing = eachPlate.CuttingPlan.MaxLength;
                        eachPlate.CuttingPlan.StringLength = valueService.GetHypotenisuByPythagoras(eachPlate.CuttingPlan.Width , eachPlate.CuttingPlan.LengthSpacing);
                        eachPlate.CuttingPlan.Slope = valueService.GetDegreeOfSlope(eachPlate.CuttingPlan.Width, eachPlate.CuttingPlan.LengthSpacing);
                        eachPlate.CuttingPlan.VMinusSlope = valueService.GetDegreeOfSlope(1, 0) - eachPlate.CuttingPlan.Slope;
                        eachPlate.CuttingPlan.LengthBetweenStringAndArc = valueService.GetLengthBetweenStringAndArc(eachPlate.Radius, eachPlate.CuttingPlan.StringLength);
                        eachPlate.CuttingPlan.XLengthOfArcTangent = valueService.GetHypotenuseByWidth(eachPlate.CuttingPlan.VMinusSlope, eachPlate.CuttingPlan.LengthBetweenStringAndArc);
                        break;
                    case Plate_Type.RectangleArc:
                        eachPlate.CuttingPlan.LengthSpacing = eachPlate.CuttingPlan.MaxLength - eachPlate.CuttingPlan.MinLength;
                        eachPlate.CuttingPlan.StringLength = valueService.GetHypotenisuByPythagoras(eachPlate.CuttingPlan.Width, eachPlate.CuttingPlan.LengthSpacing);
                        eachPlate.CuttingPlan.Slope = valueService.GetDegreeOfSlope(eachPlate.CuttingPlan.Width, eachPlate.CuttingPlan.LengthSpacing);
                        eachPlate.CuttingPlan.VMinusSlope = valueService.GetDegreeOfSlope(1, 0) - eachPlate.CuttingPlan.Slope;
                        eachPlate.CuttingPlan.LengthBetweenStringAndArc = valueService.GetLengthBetweenStringAndArc(eachPlate.Radius, eachPlate.CuttingPlan.StringLength);
                        eachPlate.CuttingPlan.XLengthOfArcTangent = valueService.GetHypotenuseByWidth(eachPlate.CuttingPlan.VMinusSlope, eachPlate.CuttingPlan.LengthBetweenStringAndArc);
                        break;
                    case Plate_Type.ArcRectangleArc:
                        eachPlate.CuttingPlan.StringLength = eachPlate.CuttingPlan.Width;
                        eachPlate.CuttingPlan.LengthSpacing = (eachPlate.CuttingPlan.MaxLength - eachPlate.CuttingPlan.MinLength)/2;
                        eachPlate.CuttingPlan.LengthBetweenStringAndArc = valueService.GetLengthBetweenStringAndArc(eachPlate.Radius, eachPlate.CuttingPlan.StringLength);
                        eachPlate.CuttingPlan.XLengthOfArcTangent = eachPlate.CuttingPlan.LengthBetweenStringAndArc;
                        break;

                }


                // MinMax Sum
                eachPlate.CuttingPlan.MinMaxSumLength = eachPlate.CuttingPlan.MinLength + eachPlate.CuttingPlan.MaxLength;
                //switch (eachPlate.ShapeType)
                //{
                //    case Plate_Type.Segment:
                //    case Plate_Type.Arc:
                //    case Plate_Type.Rectangle:
                //    case Plate_Type.RectangleArc:
                //        eachPlate.CuttingPlan.MinMaxSumLength = eachPlate.CuttingPlan.MinLength + eachPlate.CuttingPlan.MaxLength;
                //        break;
                //    case Plate_Type.ArcRectangleArc:
                //        eachPlate.CuttingPlan.MinMaxSumLength = eachPlate.CuttingPlan.MinLength + eachPlate.CuttingPlan.MaxLength;
                //        break;
                //}
            }
        }

        private DrawEntityModel DrawCuttingPlan(Point3D refPoint,List<DrawPlateModel> selPlateList,  double plateActualWidth, double plateActualLength, double scaleValue)
        {
            DrawEntityModel drawList = new DrawEntityModel();

            // Lenght Adj
            //SetPlateAddLength(selPlateList);
            // Cutting Information
            //SetCuttingInfo(selPlateList,plateActualWidth,plateActualLength);

            // 길이 내림 차순 , 간격 오름 차순
            List<DrawPlateModel> sortPlateList= selPlateList.OrderByDescending(x => x.CuttingPlan.MinMaxSumLength).ThenBy(x => x.ShapeType).ThenBy(x => x.CuttingPlan.LengthSpacing).ToList();

            // 종류별로 1개만
            Dictionary<string, List<DrawPlateModel>> divPlateDic = new Dictionary<string, List<DrawPlateModel>>();
            Dictionary<string, int> divPlateCountDic = new Dictionary<string, int>();
            List<double> divPlateMaxLengthList = new List<double>();
            foreach (DrawPlateModel eachPlate in sortPlateList)
            {
                string displayName = eachPlate.DisplayName;
                if (!divPlateDic.ContainsKey(displayName))
                {
                    // 신규
                    List<DrawPlateModel> eachPlateList = new List<DrawPlateModel>();
                    eachPlateList.Add(eachPlate);
                    divPlateDic.Add(displayName, eachPlateList);
                    divPlateCountDic.Add(displayName, 1);
                    divPlateMaxLengthList.Add(eachPlate.CuttingPlan.MaxLength);
                }
                else
                {
                    // 기존
                    divPlateDic[displayName].Add(eachPlate);
                    divPlateCountDic[displayName]++;
                }
            }

            List<string> divPlateNameList = divPlateCountDic.Keys.ToList();
            List<int> divPlateCountList = divPlateCountDic.Values.ToList();


            // Arrange : One Plate
            double maximumCuttingInPlate = 4; // 매우 중요
            double spacingValue = 30;   // 매우 중요
            bool leftDirection = true;
            List<string> divPlateKeyList = divPlateDic.Keys.ToList();


            double newPlateCount = 0;
            double maxPlateCount = sortPlateList.Count;
            List<DrawOnePlateModel> onePlateList = new List<DrawOnePlateModel>();
            while (onePlateList.Count< maxPlateCount)
            {
                // NewPlate
                newPlateCount++;
                DrawOnePlateModel newOnePlate = new DrawOnePlateModel();
                onePlateList.Add(newOnePlate);
                newOnePlate.PlateWidth = plateActualWidth;
                newOnePlate.PlateLength = plateActualLength;
                newOnePlate.InsertPoint=GetPlateNextPoint(refPoint, newPlateCount, scaleValue);
                newOnePlate.RemainingLength = plateActualLength;

                leftDirection = true;
                bool addPlate = false;
                bool cuttingArrange = true;
                double onePlateArrangeCount = 0;




                while (cuttingArrange)
                {
                    addPlate = false;


                    if (leftDirection)
                    {
                        // Left Plate : 큰것 -> 작은것
                        for (int i = 0; i < divPlateCountList.Count; i++)
                        {
                            if (divPlateMaxLengthList[i] <= newOnePlate.RemainingLength)
                            {
                                if (divPlateCountList[i] > 0)
                                {
                                    
                                    // 왼쪽에 배정
                                    List<DrawPlateModel> leftPlateList = divPlateDic[divPlateNameList[i]];
                                    int leftRemainingIndex = divPlateCountList[i] - 1;
                                    DrawPlateModel leftPlate = leftPlateList[leftRemainingIndex];

                                    leftPlate.CuttingPlan.LeftPlate = leftDirection;
                                    leftPlate.CuttingPlan.InsertPointXLength = plateActualLength - newOnePlate.RemainingLength;
                                    newOnePlate.CuttingPlateList.Add(leftPlate);
                                    newOnePlate.CuttingPlateNameList.Add(leftPlate.DisplayName);
                                    newOnePlate.RemainingLength -= leftPlate.CuttingPlan.MinLength;
                                    if(leftPlate.ShapeType==Plate_Type.ArcRectangleArc)
                                        if(leftPlate.CuttingPlan.MinLength== leftPlate.CuttingPlan.MaxLength)
                                            newOnePlate.RemainingLength -= leftPlate.CuttingPlan.LengthBetweenStringAndArc;

                                    divPlateCountList[i]--;// 1차감
                                    leftDirection = false;
                                    addPlate = true;
                                    break;

                                }
                            }
                        }
                    }
                    else
                    {
                        // Right Plate : 작은 것 -> 큰것
                        for (int i =0; i<divPlateCountList.Count; i++)
                        {
                            if (divPlateMaxLengthList[i] <= newOnePlate.RemainingLength)
                            {
                                if (divPlateCountList[i] > 0)
                                {

                                    // 왼쪽과 비교하면서 배정
                                    List<DrawPlateModel> rightPlateList = divPlateDic[divPlateNameList[i]];
                                    int rightRemainingIndex = divPlateCountList[i] - 1;
                                    DrawPlateModel rightPlate = rightPlateList[rightRemainingIndex];

                                    if (CalOptimalSpacing(newOnePlate.RemainingLength, newOnePlate.CuttingPlateList.Last(), rightPlate, spacingValue, out double optimalSpacing))
                                    {
                                        rightPlate.CuttingPlan.LeftPlate = leftDirection;
                                        rightPlate.CuttingPlan.InsertPointXLength = plateActualLength - newOnePlate.RemainingLength + optimalSpacing;
                                        newOnePlate.CuttingPlateList.Add(rightPlate);
                                        newOnePlate.CuttingPlateNameList.Add(rightPlate.DisplayName);
                                        newOnePlate.RemainingLength -= optimalSpacing + rightPlate.CuttingPlan.MaxLength + spacingValue; // 매우중요 : 왼족 세트와 Spacing

                                        divPlateCountList[i]--;// 1 선 차감
                                        leftDirection = true;
                                        addPlate = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }









                    // Break Condition
                    if (newOnePlate.RemainingLength <= 0)
                        cuttingArrange = false;
                    if (newOnePlate.CuttingPlateList.Count == maximumCuttingInPlate)
                        cuttingArrange = false;
                    if (!addPlate)
                        cuttingArrange = false;

                    
                }




            }


            // Internal Alignment : One Plate
            for (int i = 0; i < onePlateList.Count; i++)
            {
                DrawOnePlateModel eachOnePlate = onePlateList[i];
                if (eachOnePlate.RemainingLength > 0)
                {
                    if (eachOnePlate.CuttingPlateList.Count == 2)
                    {
                        // 2개 : 양쪽 정렬
                        double maxCuttingPlateWidth = eachOnePlate.CuttingPlateList.Max(x => x.CuttingPlan.Width);
                        if (maxCuttingPlateWidth == plateActualWidth)
                        {
                            DrawPlateModel eachCuttingPlate = eachOnePlate.CuttingPlateList.Last();
                            eachCuttingPlate.CuttingPlan.InsertPointXLength += eachOnePlate.RemainingLength + spacingValue; // 매우중요
                        }

                    }
                    else if (eachOnePlate.CuttingPlateList.Count == 4)
                    {
                        // 4개 : 그룹 양쪽 정렬
                        double maxCuttingPlateWidth = eachOnePlate.CuttingPlateList.Max(x => x.CuttingPlan.Width);
                        if (maxCuttingPlateWidth == plateActualWidth)
                        {
                            for (int j = 2; j < eachOnePlate.CuttingPlateList.Count; j++)
                            {
                                DrawPlateModel eachCuttingPlate = eachOnePlate.CuttingPlateList[j];
                                eachCuttingPlate.CuttingPlan.InsertPointXLength += eachOnePlate.RemainingLength + spacingValue; // 매우중요
                                                                                                                                //else
                                                                                                                                //eachCuttingPlate.CuttingPlan.InsertPointXLength += eachOnePlate.RemainingLength ; // 매우중요

                            }
                        }

                    }

                    // 3개 : 정렬 없음
                    // 5개 : 정렬 없음

                }
                // Case : 양쪽 맞춤을 그룹으로 하기 : 보류
            }

            // Deduplication
            Dictionary<string, DrawOnePlateModel> displayNameDic = new Dictionary<string, DrawOnePlateModel>();
            for (int i = onePlateList.Count - 1; i >= 0; i--)
            {
                if (onePlateList[i].CuttingPlateList.Count == 0)
                {
                    onePlateList.RemoveAt(i);
                }
                else
                {
                    string sumOfDisplayName = string.Join("", onePlateList[i].CuttingPlateNameList);
                    if (!displayNameDic.ContainsKey(sumOfDisplayName))
                    {
                        onePlateList[i].Requirement = 1;
                        displayNameDic.Add(sumOfDisplayName, onePlateList[i]);
                    }
                    else
                    {
                        displayNameDic[sumOfDisplayName].Requirement++;
                        onePlateList.RemoveAt(i);
                    }
                }
            }


            // Draw
            List<Entity> arrangeCuttingPlateList = new List<Entity>();
            List<Entity> arrangePlateList = new List<Entity>();
            List<Point3D> outPoint = new List<Point3D>();
            double drawPlateCount = 0;
            foreach (DrawOnePlateModel eachOnePlate in onePlateList)
            {
                DrawEntityModel dimOnePlateModel = new DrawEntityModel();

                drawPlateCount++;
                Point3D insertPoint= GetPlateNextPoint(refPoint, drawPlateCount, scaleValue);
                eachOnePlate.InsertPoint = insertPoint;
                eachOnePlate.PlateOutlineList.AddRange(DrawPlate(eachOnePlate,out dimOnePlateModel, scaleValue));
                arrangePlateList.AddRange(eachOnePlate.PlateOutlineList);
                drawList.AddDrawEntity(dimOnePlateModel);

                double cutIndex = 0;
                foreach (DrawPlateModel eachPlate in eachOnePlate.CuttingPlateList)
                {
                    cutIndex++;
                    DrawEntityModel dimPlateModel = new DrawEntityModel();
                    eachOnePlate.PlateCuttingList.AddRange(DrawCuttingPlate(eachPlate.CuttingPlan.LeftPlate,
                                                            GetSumPoint(eachOnePlate.InsertPoint, eachPlate.CuttingPlan.InsertPointXLength, 0),
                                                            eachPlate,
                                                            scaleValue,
                                                            plateActualWidth,
                                                            plateActualLength,
                                                            cutIndex,
                                                            eachOnePlate.CuttingPlateList.Count,
                                                            eachOnePlate.RemainingLength,
                                                            out dimPlateModel)); 
                    arrangeCuttingPlateList.AddRange(eachOnePlate.PlateCuttingList);
                    drawList.AddDrawEntity(dimPlateModel);
                }
            }

            styleService.SetLayerListEntity(ref arrangePlateList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(arrangeCuttingPlateList);
            drawList.outlineList.AddRange(arrangePlateList);

            return drawList;
        }

        private List<Entity> DrawPlate(DrawOnePlateModel selModel, out DrawEntityModel dimModel,double scaleValue)
        {
            List<Entity> newList = new List<Entity>();
            List<Point3D> outPoint = new List<Point3D>();
            newList.AddRange(shapeService.GetRectangle(out outPoint, 
                                                        GetSumPoint(selModel.InsertPoint, 0, 0),
                                                        selModel.PlateLength, 
                                                        selModel.PlateWidth, 0, 0, 3));

            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);
            // Leader
            string leaderCourseInfoD = "  REQ'D  : " + valueService.GetOrdinalSheet(selModel.Requirement);
            DrawBMLeaderModel leaderInfoModel = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = leaderCourseInfoD, bmNumber = "", textAlign = POSITION_TYPE.CENTER, arrowHeadVisible = false };
            dimModel=drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(selModel.InsertPoint, selModel.PlateLength, 0), leaderInfoModel, scaleValue);


            return newList;
        }

        private List<Entity> DrawCuttingPlate(bool leftPlate, Point3D selPoint,DrawPlateModel selPlate, double scaleValue,
                                                double plateWidth,double plateLength, double cutIndex,double cutMaxIndex,
                                                double remainingLength, out DrawEntityModel dimModel)
        {
            List<Entity> newList = new List<Entity>();

            dimModel = new DrawEntityModel();

            Line bLine = null;
            Line tLine = null;
            Line lLine = null;
            Line rLine = null;
            Arc tArc = null;
            Arc lArc = null;
            Arc rArc = null;
            Text nameText = null;

            Circle cir01 = null;
            Circle cir02 = null;
            Point3D[] cirInter = null;
            Point3D[] cirInterSort = null;



            // Pre Adj
            if (cutIndex == cutMaxIndex)
            {
                if (cutIndex > 2 && leftPlate==true)
                {
                    if (selPlate.ShapeType == Plate_Type.Arc)
                    {
                        // 위로 맞춤
                        leftPlate = false;
                    }
                }
            }



            // Draw
            if (leftPlate)
            {
                // 왼쪽
                switch (selPlate.ShapeType)
                {
                    case Plate_Type.Segment:
                        bLine = new Line(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint,selPlate.CuttingPlan.MaxLength, 0));
                        tArc = new Arc(GetSumPoint(bLine.StartPoint, 0, 0),
                                        GetSumPoint(bLine.MidPoint, 0, selPlate.CuttingPlan.LengthBetweenStringAndArc),
                                        GetSumPoint(bLine.EndPoint, 0, 0),false);
                        break;

                    case Plate_Type.Rectangle:
                        bLine = new Line(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, selPlate.CuttingPlan.MaxLength, 0));
                        tLine = new Line(GetSumPoint(selPoint, 0, selPlate.CuttingPlan.Width), GetSumPoint(selPoint, selPlate.CuttingPlan.MaxLength, selPlate.CuttingPlan.Width));
                        lLine = new Line(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, 0, selPlate.CuttingPlan.Width));
                        rLine = new Line(GetSumPoint(selPoint, selPlate.CuttingPlan.MaxLength, 0), GetSumPoint(selPoint, selPlate.CuttingPlan.MaxLength, selPlate.CuttingPlan.Width));

                        break;
                    case Plate_Type.Arc:
                        bLine = new Line(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, selPlate.CuttingPlan.MaxLength, 0));
                        lLine = new Line(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, 0, selPlate.CuttingPlan.Width));
                        cir01 = new Circle(Plane.XY, GetSumPoint(lLine.EndPoint, 0, 0), selPlate.Radius);
                        cir02 = new Circle(Plane.XY, GetSumPoint(bLine.EndPoint, 0, 0), selPlate.Radius);
                        cirInter = cir01.IntersectWith(cir02);
                        cirInterSort=cirInter.OrderBy(x => x.Y).ThenBy(x=>x.X).ToArray();
                        rArc = new Arc(Plane.XY, GetSumPoint(cirInterSort[0], 0, 0),selPlate.Radius, GetSumPoint(lLine.EndPoint, 0, 0), GetSumPoint(bLine.EndPoint, 0, 0),true);

                        break;

                    case Plate_Type.RectangleArc:
                        bLine = new Line(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, selPlate.CuttingPlan.MaxLength, 0));
                        tLine = new Line(GetSumPoint(selPoint, 0, selPlate.CuttingPlan.Width), GetSumPoint(selPoint, selPlate.CuttingPlan.MinLength, selPlate.CuttingPlan.Width));
                        lLine = new Line(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, 0, selPlate.CuttingPlan.Width));
                        cir01 = new Circle(Plane.XY, GetSumPoint(tLine.EndPoint, 0, 0), selPlate.Radius);
                        cir02 = new Circle(Plane.XY, GetSumPoint(bLine.EndPoint, 0, 0), selPlate.Radius);
                        cirInter = cir01.IntersectWith(cir02);
                        cirInterSort = cirInter.OrderBy(x => x.Y).ThenBy(x => x.X).ToArray();
                        rArc = new Arc(Plane.XY, GetSumPoint(cirInterSort[0], 0, 0), selPlate.Radius, GetSumPoint(tLine.EndPoint, 0, 0), GetSumPoint(bLine.EndPoint, 0, 0),true);
                        break;

                    case Plate_Type.ArcRectangleArc:
                        bLine = new Line(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, selPlate.CuttingPlan.MaxLength, 0));
                        tLine = new Line(GetSumPoint(selPoint, selPlate.CuttingPlan.LengthSpacing, selPlate.CuttingPlan.Width), GetSumPoint(selPoint, selPlate.CuttingPlan.LengthSpacing + selPlate.CuttingPlan.MinLength, selPlate.CuttingPlan.Width));
                        //lLine = new Line(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, 0, selPlate.CuttingPlan.Width));
                        cir01 = new Circle(Plane.XY, GetSumPoint(tLine.EndPoint, 0, 0), selPlate.Radius);
                        cir02 = new Circle(Plane.XY, GetSumPoint(bLine.EndPoint, 0, 0), selPlate.Radius);
                        cirInter = cir01.IntersectWith(cir02);
                        cirInterSort = cirInter.OrderBy(x => x.Y).ThenBy(x => x.X).ToArray();
                        rArc = new Arc(Plane.XY, GetSumPoint(cirInterSort[0], 0, 0), selPlate.Radius, GetSumPoint(tLine.EndPoint, 0, 0), GetSumPoint(bLine.EndPoint, 0, 0), true);
                        lArc = (Arc)rArc.Clone();
                        editingService.SetMirrorArc(Plane.YZ, ref lArc, GetSumPoint(bLine.MidPoint, 0, 0));
                        break;

                }
            }
            else
            {
                switch (selPlate.ShapeType)
                {
                    // 그냥
                    case Plate_Type.Segment:
                        bLine = new Line(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, selPlate.CuttingPlan.MaxLength, 0));
                        tArc = new Arc(GetSumPoint(bLine.StartPoint, 0, 0),
                                        GetSumPoint(bLine.MidPoint, 0, selPlate.CuttingPlan.LengthBetweenStringAndArc),
                                        GetSumPoint(bLine.EndPoint, 0, 0),false);
                        break;
                    // 그냥
                    case Plate_Type.Rectangle:
                        bLine = new Line(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, selPlate.CuttingPlan.MaxLength, 0));
                        tLine = new Line(GetSumPoint(selPoint, 0, selPlate.CuttingPlan.Width), GetSumPoint(selPoint, selPlate.CuttingPlan.MaxLength, selPlate.CuttingPlan.Width));
                        lLine = new Line(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, 0, selPlate.CuttingPlan.Width));
                        rLine = new Line(GetSumPoint(selPoint, selPlate.CuttingPlan.MaxLength, 0), GetSumPoint(selPoint, selPlate.CuttingPlan.MaxLength, selPlate.CuttingPlan.Width));

                        break;
                    // 돌림
                    case Plate_Type.Arc:
                        tLine = new Line(GetSumPoint(selPoint, 0, selPlate.CuttingPlan.Width), GetSumPoint(selPoint, selPlate.CuttingPlan.MaxLength, selPlate.CuttingPlan.Width));
                        rLine = new Line(GetSumPoint(tLine.EndPoint, 0, -selPlate.CuttingPlan.Width), GetSumPoint(tLine.EndPoint, 0, 0));
                        cir01 = new Circle(Plane.XY, GetSumPoint(tLine.StartPoint, 0, 0), selPlate.Radius);
                        cir02 = new Circle(Plane.XY, GetSumPoint(rLine.StartPoint, 0, 0), selPlate.Radius);
                        cirInter = cir01.IntersectWith(cir02);
                        cirInterSort = cirInter.OrderByDescending(x => x.Y).ThenByDescending(x => x.X).ToArray();
                        rArc = new Arc(Plane.XY, GetSumPoint(cirInterSort[0], 0, 0),selPlate.Radius, GetSumPoint(tLine.StartPoint, 0, 0), GetSumPoint(rLine.StartPoint, 0, 0),false);

                        break;

                    case Plate_Type.RectangleArc:
                        tLine = new Line(GetSumPoint(selPoint, 0, selPlate.CuttingPlan.Width), GetSumPoint(selPoint, selPlate.CuttingPlan.MaxLength, selPlate.CuttingPlan.Width));
                        rLine = new Line(GetSumPoint(tLine.EndPoint, 0, 0), GetSumPoint(tLine.EndPoint, 0, -selPlate.CuttingPlan.Width));
                        bLine = new Line(GetSumPoint(rLine.EndPoint, -selPlate.CuttingPlan.MinLength,0), GetSumPoint(rLine.EndPoint, 0, 0));
                        cir01 = new Circle(Plane.XY, GetSumPoint(tLine.StartPoint, 0, 0), selPlate.Radius);
                        cir02 = new Circle(Plane.XY, GetSumPoint(bLine.StartPoint, 0, 0), selPlate.Radius);
                        cirInter = cir01.IntersectWith(cir02);
                        cirInterSort = cirInter.OrderByDescending(x => x.Y).ThenByDescending(x => x.X).ToArray();
                        rArc = new Arc(Plane.XY, GetSumPoint(cirInterSort[0], 0, 0), selPlate.Radius, GetSumPoint(tLine.StartPoint, 0, 0), GetSumPoint(bLine.StartPoint, 0, 0), false);
                        break;

                }
            }
            if (bLine != null)
                newList.Add(bLine);
            if (tLine != null)
                newList.Add(tLine);
            if (lLine != null)
                newList.Add(lLine);
            if (rLine != null)
                newList.Add(rLine);
            if (tArc != null)
                newList.Add(tArc);
            if (lArc != null)
                newList.Add(lArc);
            if (rArc != null)
                newList.Add(rArc);


            // Translate
            if (!leftPlate)
            {
                if (selPlate.ShapeType == Plate_Type.Arc)
                {
                    double transYValue = plateWidth - selPlate.CuttingPlan.Width;
                    editingService.SetTranslate(ref newList, GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, 0, -transYValue));
                }
            }

            // Translate : ArcRectangleArc
            if (selPlate.ShapeType == Plate_Type.ArcRectangleArc)
            {
                if (selPlate.CuttingPlan.MinLength == selPlate.CuttingPlan.MaxLength)
                    editingService.SetTranslate(ref newList, GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, -selPlate.CuttingPlan.LengthBetweenStringAndArc,0));
            }




            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            if (nameText != null)
            {
                styleService.SetLayer(ref nameText, layerService.LayerDimension);
                newList.Add(nameText);
            }



            // Dimension
            

            switch (selPlate.ShapeType)
            {
                case Plate_Type.Segment:

                    break;

                case Plate_Type.Rectangle:
                case Plate_Type.RectangleArc:
                    if (selPlate.CuttingPlan.Width == plateWidth && selPlate.CuttingPlan.MaxLength==plateLength)
                    {
                        DrawDimensionModel leftDim = new DrawDimensionModel()
                        {
                            position = POSITION_TYPE.LEFT,
                            textUpper = Math.Round(selPlate.CuttingPlan.Width, 1, MidpointRounding.AwayFromZero).ToString(),
                            textLower = "(TYP.)",
                            dimHeight = 12,
                            scaleValue = scaleValue,
                        };
                        DrawEntityModel leftDimEntity = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lLine.StartPoint, 0, 0), GetSumPoint(lLine.EndPoint, 0, 0), scaleValue, leftDim, 0);
                        dimModel.AddDrawEntity(leftDimEntity);
                        DrawDimensionModel topDim = new DrawDimensionModel()
                        {
                            position = POSITION_TYPE.TOP,
                            textUpper = Math.Round(selPlate.CuttingPlan.MaxLength, 1, MidpointRounding.AwayFromZero).ToString(),
                            textLower = "(TYP.)",
                            dimHeight = 12,
                            scaleValue = scaleValue,
                        };
                        DrawEntityModel topDimEntity = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(tLine.StartPoint, 0, 0), GetSumPoint(tLine.EndPoint, 0, 0), scaleValue, topDim, 0);
                        dimModel.AddDrawEntity(topDimEntity);
                    }
                    else
                    {
                        if (cutIndex == 1)
                        {
                            DrawDimensionModel leftDim = new DrawDimensionModel()
                            {
                                position = POSITION_TYPE.LEFT,
                                textUpper = Math.Round(selPlate.CuttingPlan.Width, 1, MidpointRounding.AwayFromZero).ToString(),
                                dimHeight = 12,
                                scaleValue = scaleValue,
                            };
                            DrawEntityModel leftDimEntity = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lLine.StartPoint, 0, 0), GetSumPoint(lLine.EndPoint, 0, 0), scaleValue, leftDim, 0);
                            dimModel.AddDrawEntity(leftDimEntity);
                        }
                            
                        DrawDimensionModel topDim = new DrawDimensionModel()
                        {
                            position = POSITION_TYPE.TOP,
                            textUpper = Math.Round(selPlate.CuttingPlan.MaxLength, 1, MidpointRounding.AwayFromZero).ToString(),
                            dimHeight = 12,
                            scaleValue = scaleValue,
                        };
                        DrawEntityModel topDimEntity = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(tLine.StartPoint, 0, 0), GetSumPoint(tLine.EndPoint, 0, 0), scaleValue, topDim, 0);
                        dimModel.AddDrawEntity(topDimEntity);
                        DrawDimensionModel bottomDim = new DrawDimensionModel()
                        {
                            position = POSITION_TYPE.BOTTOM,
                            textUpper = Math.Round(selPlate.CuttingPlan.MaxLength, 1, MidpointRounding.AwayFromZero).ToString(),
                            dimHeight = 12,
                            scaleValue = scaleValue,
                        };
                        DrawEntityModel bottomDimEntity = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(bLine.StartPoint, 0, 0), GetSumPoint(bLine.EndPoint, 0, 0), scaleValue, bottomDim, 0);
                        dimModel.AddDrawEntity(bottomDimEntity);
                            
                    }


                    break;
                case Plate_Type.Arc:

                    Line lrLine = null;
                    if (lLine != null)
                        lrLine = lLine;
                    if (rLine != null)
                        lrLine = rLine;
                    if (cutIndex == 1)
                    {
                        DrawDimensionModel leftDim = new DrawDimensionModel()
                        {
                            position = POSITION_TYPE.LEFT,
                            textUpper = Math.Round(selPlate.CuttingPlan.Width, 1, MidpointRounding.AwayFromZero).ToString(),
                            dimHeight = 12,
                            scaleValue = scaleValue,
                        };
                        DrawEntityModel leftDimEntity = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lrLine.StartPoint, 0, 0), GetSumPoint(lrLine.EndPoint, 0, 0), scaleValue, leftDim, 0);
                        dimModel.AddDrawEntity(leftDimEntity);
                    }
                    if (cutIndex == cutMaxIndex)
                    {
                        if (cutIndex > 1)
                        {
                            DrawDimensionModel rightDim = new DrawDimensionModel()
                            {
                                position = POSITION_TYPE.RIGHT,
                                textUpper = Math.Round(selPlate.CuttingPlan.Width, 1, MidpointRounding.AwayFromZero).ToString(),
                                dimHeight = 12,
                                scaleValue = scaleValue,
                            };
                            DrawEntityModel rightDimEntity = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(lrLine.StartPoint, 0, 0), GetSumPoint(lrLine.EndPoint, 0, 0), scaleValue, rightDim, 0);
                            dimModel.AddDrawEntity(rightDimEntity);
                        }
                    }

                    if (bLine != null)
                    {
                        DrawDimensionModel bottomDim = new DrawDimensionModel()
                        {
                            position = POSITION_TYPE.BOTTOM,
                            textUpper = Math.Round(selPlate.CuttingPlan.MaxLength, 1, MidpointRounding.AwayFromZero).ToString(),
                            dimHeight = 12,
                            scaleValue = scaleValue,
                        };
                        DrawEntityModel bottomDimEntity = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(bLine.StartPoint, 0, 0), GetSumPoint(bLine.EndPoint, 0, 0), scaleValue, bottomDim, 0);
                        dimModel.AddDrawEntity(bottomDimEntity);
                    }
                    if(tLine != null)
                    {
                        DrawDimensionModel topDim = new DrawDimensionModel()
                        {
                            position = POSITION_TYPE.TOP,
                            textUpper = Math.Round(selPlate.CuttingPlan.MaxLength, 1, MidpointRounding.AwayFromZero).ToString(),
                            dimHeight = 12,
                            scaleValue = scaleValue,
                        };
                        DrawEntityModel topDimEntity = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(tLine.StartPoint, 0, 0), GetSumPoint(tLine.EndPoint, 0, 0), scaleValue, topDim, 0);
                        dimModel.AddDrawEntity(topDimEntity);
                    }

                    break;

            }



            // Number
            double textHeight = 2.5;
            Point3D insertTextPoint = GetNumberPoint(selPoint, selPlate, leftPlate);
            Text displayNameText = new Text(Plane.XY, insertTextPoint, selPlate.DisplayName, textHeight * scaleValue) { Alignment = Text.alignmentType.MiddleCenter };
            newList.Add(displayNameText);

            return newList;
        }

        public bool CalOptimalSpacing(double remainingLength,DrawPlateModel leftPlate, DrawPlateModel rightPlate,double spacingValue, out double optimalSpacing)
        {
            // Out
            optimalSpacing = 0;
            bool returnValue = false;

            double upperL = 0;
            double lowerL = 0;
            double upperR = 0;
            double lowerR = 0;

            double correctionValue = 0;
            // 호의 중간 접선
            switch (leftPlate.ShapeType)
            {
                case Plate_Type.Arc:
                case Plate_Type.RectangleArc:
                    // 길이 + 호의접선 + 간격 
                    upperL = spacingValue + leftPlate.CuttingPlan.XLengthOfArcTangent + 0;
                    lowerL = spacingValue + leftPlate.CuttingPlan.XLengthOfArcTangent + leftPlate.CuttingPlan.LengthSpacing;
                    correctionValue = leftPlate.CuttingPlan.XLengthOfArcTangent + spacingValue + rightPlate.CuttingPlan.XLengthOfArcTangent;
                    switch (rightPlate.ShapeType)
                    {
                        case Plate_Type.Arc:
                        case Plate_Type.RectangleArc:
                            // Case 1 : 중앙 호의 접선으로 처리 : 반전
                            upperR = spacingValue + rightPlate.CuttingPlan.XLengthOfArcTangent + rightPlate.CuttingPlan.MaxLength;
                            lowerR = spacingValue + rightPlate.CuttingPlan.XLengthOfArcTangent + rightPlate.CuttingPlan.MinLength;
                            // Case 2 : 
                            break;
                        case Plate_Type.Segment:
                        case Plate_Type.Rectangle:
                            // 사각형 처럼 처리
                            // Case 1 : 세로로 배열
                            // Case 2 : 가로로 배열
                            upperR = spacingValue + 0;
                            lowerR = spacingValue + 0;

                            break;
                    }

                    break;

                case Plate_Type.Segment:
                case Plate_Type.Rectangle:
                    // 길이 + 호의접선 + 간격 
                    upperL = spacingValue + 0;
                    lowerL = spacingValue + 0;
                    correctionValue = 0 ;
                    switch (rightPlate.ShapeType)
                    {
                        case Plate_Type.Arc:
                        case Plate_Type.RectangleArc:
                            // Case 1 : 중앙 호의 접선으로 처리 : 반전
                            upperR = spacingValue + rightPlate.CuttingPlan.XLengthOfArcTangent + rightPlate.CuttingPlan.MaxLength;
                            lowerR = spacingValue + rightPlate.CuttingPlan.XLengthOfArcTangent + +rightPlate.CuttingPlan.MinLength;
                            // Case 2 : 

                            break;
                        case Plate_Type.Segment:
                        case Plate_Type.Rectangle:
                            // 사각형 처럼 처리
                            // Case 1 : 세로로 배열
                            // Case 2 : 가로로 배열
                            upperR = spacingValue + 0;
                            lowerR = spacingValue + 0;

                            break;
                    }
                    break;

            };


            // 점검
            double sumUpperLength = upperL + upperR;
            double sumLowerLength = lowerL + lowerR;
            double maxLength = Math.Max(sumUpperLength, sumLowerLength);
            double minLength = Math.Min(sumUpperLength, sumLowerLength);
            if (remainingLength >= maxLength)
            {
                returnValue = true;
                if (correctionValue == 0)
                {
                    optimalSpacing = spacingValue;
                }
                else 
                {
                    optimalSpacing = maxLength - minLength; // Gap
                    optimalSpacing += correctionValue;
                } 

            }

            return returnValue;
        }


        private Point3D GetPlateNextPoint(Point3D refPoint, double countIndex,double scaleValue)
        {
            double distanceH =200 * scaleValue;
            return GetSumPoint(refPoint, 0, distanceH * (countIndex-1));
        }

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }



        public Point3D GetNumberPoint(Point3D selPoint, DrawPlateModel selPlate,bool leftPlate)
        {
            double distanceX = 0;
            double distanceY = 0;

            // selPoint : 좌측 하단 기준
            switch (selPlate.ShapeType)
            {
                case Plate_Type.Segment:
                case Plate_Type.Rectangle:
                    distanceX = selPlate.CuttingPlan.MaxLength / 2;
                    distanceY = selPlate.CuttingPlan.Width / 2;
                    break;
                case Plate_Type.RectangleArc:
                    distanceX = (selPlate.CuttingPlan.MaxLength + selPlate.CuttingPlan.MinLength) / 4;
                    distanceY = selPlate.CuttingPlan.Width / 2;
                    //if (!leftPlate)
                    //    distanceX += selPlate.CuttingPlan.LengthSpacing;
                    break;
                case Plate_Type.ArcRectangleArc:
                    distanceX = selPlate.CuttingPlan.MaxLength / 2;
                    distanceY = selPlate.CuttingPlan.Width / 2;
                    distanceX += selPlate.CuttingPlan.LengthSpacing;
                    break;
                case Plate_Type.Arc:
                    distanceX = selPlate.CuttingPlan.MaxLength / 3;
                    distanceY = selPlate.CuttingPlan.Width / 3;
                    break;
            }

            Point3D nPoint = GetSumPoint(selPoint, distanceX, distanceY);

            return nPoint;
        }


    }
}
