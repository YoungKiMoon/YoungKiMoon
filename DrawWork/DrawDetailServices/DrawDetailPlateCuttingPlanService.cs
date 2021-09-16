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

            
            drawList.AddDrawEntity(DrawCuttingPlan(PlateArrange_Type.Roof, referencePoint, SingletonData.RoofPlateInfo, plateActualWidth, plateActualLength, realScaleValue));
            
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

            drawList.AddDrawEntity(DrawCuttingPlan(PlateArrange_Type.Bottom,referencePoint, SingletonData.BottomPlateInfo, plateActualWidth, plateActualLength, realScaleValue));
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
                        eachPlate.CuttingPlan.Width = plateWidth;
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

        private DrawEntityModel DrawCuttingPlan(PlateArrange_Type arrangeType, Point3D refPoint,List<DrawPlateModel> selPlateList,  double plateActualWidth, double plateActualLength, double scaleValue)
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
            double maximumCuttingInPlate = 6; // 매우 중요
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
                                        double adjOptimalSpacing = newOnePlate.RemainingLength - rightPlate.CuttingPlan.MaxLength;
                                        if (adjOptimalSpacing < optimalSpacing)
                                            optimalSpacing = adjOptimalSpacing;
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
                eachOnePlate.CenterPoint = GetSumPoint(insertPoint, eachOnePlate.PlateLength / 2, eachOnePlate.PlateWidth / 2);
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


            // Singleton Data
            if (arrangeType == PlateArrange_Type.Roof)
            {
                SingletonData.RoofPlateList.Clear();
                SingletonData.RoofPlateList.AddRange(onePlateList);
            }
            else if (arrangeType == PlateArrange_Type.Bottom)
            {
                SingletonData.BottomPlateList.Clear();
                SingletonData.BottomPlateList.AddRange(onePlateList);
            }

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
            string leaderCourseInfoD = "         REQ'D  : " + valueService.GetOrdinalSheet(selModel.Requirement);
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
                //
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
                    if (bLine != null)
                    {
                        DrawDimensionModel bottomDim = new DrawDimensionModel()
                        {
                            position = POSITION_TYPE.BOTTOM,
                            textUpper = Math.Round(bLine.Length(), 1, MidpointRounding.AwayFromZero).ToString(),
                            dimHeight = 12,
                            scaleValue = scaleValue,
                        };
                        DrawEntityModel bottomDimEntity = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(bLine.StartPoint, 0, 0), GetSumPoint(bLine.EndPoint, 0, 0), scaleValue, bottomDim, 0);
                        dimModel.AddDrawEntity(bottomDimEntity);
                    }
                    break;

                case Plate_Type.Rectangle:
                case Plate_Type.RectangleArc:
                case Plate_Type.ArcRectangleArc:
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
                            if (lLine != null)
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

                        }
                            
                        DrawDimensionModel topDim = new DrawDimensionModel()
                        {
                            position = POSITION_TYPE.TOP,
                            textUpper = Math.Round(tLine.Length(), 1, MidpointRounding.AwayFromZero).ToString(),
                            dimHeight = 12,
                            scaleValue = scaleValue,
                        };
                        DrawEntityModel topDimEntity = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(tLine.StartPoint, 0, 0), GetSumPoint(tLine.EndPoint, 0, 0), scaleValue, topDim, 0);
                        dimModel.AddDrawEntity(topDimEntity);
                        DrawDimensionModel bottomDim = new DrawDimensionModel()
                        {
                            position = POSITION_TYPE.BOTTOM,
                            textUpper = Math.Round(bLine.Length(), 1, MidpointRounding.AwayFromZero).ToString(),
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
            Point3D insertTextPoint = GetNumberPoint(selPoint, selPlate, leftPlate,plateWidth);
            Text displayNameText = new Text(Plane.XY, insertTextPoint, selPlate.DisplayName, textHeight * scaleValue) { Alignment = Text.alignmentType.MiddleCenter };
            styleService.SetLayer(ref displayNameText, layerService.LayerDimension);
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
                            upperR = spacingValue + rightPlate.CuttingPlan.MaxLength;
                            lowerR = spacingValue + rightPlate.CuttingPlan.MaxLength;

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
                            lowerR = spacingValue + rightPlate.CuttingPlan.XLengthOfArcTangent + rightPlate.CuttingPlan.MinLength;
                            // Case 2 : 

                            break;
                        case Plate_Type.Segment:
                        case Plate_Type.Rectangle:
                            // 사각형 처럼 처리
                            // Case 1 : 세로로 배열
                            // Case 2 : 가로로 배열
                            upperR = spacingValue + rightPlate.CuttingPlan.MaxLength;
                            lowerR = spacingValue + rightPlate.CuttingPlan.MaxLength;

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



        public Point3D GetNumberPoint(Point3D selPoint, DrawPlateModel selPlate,bool leftPlate,double selPlateWidth)
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
                    if (!leftPlate)
                        distanceX = selPlate.CuttingPlan.MaxLength-distanceX;
                    break;
                case Plate_Type.ArcRectangleArc:
                    distanceX = selPlate.CuttingPlan.MaxLength / 2;
                    distanceY = selPlate.CuttingPlan.Width / 2;

                    break;
                case Plate_Type.Arc:
                    distanceX = selPlate.CuttingPlan.MaxLength / 3;
                    distanceY = selPlate.CuttingPlan.Width / 3;
                    if (!leftPlate)
                    {
                        distanceX = selPlate.CuttingPlan.MaxLength - distanceX;
                        distanceY = selPlateWidth - distanceY;
                    }


                    break;
            }

            Point3D nPoint = GetSumPoint(selPoint, distanceX, distanceY);

            return nPoint;
        }





        // Compression Ring , Annular Plate
        public DrawEntityModel DrawRoofComRingCuttingPlan(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {


            DrawEntityModel drawList = new DrawEntityModel();
            double plateActualWidth = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].PlateWidth);
            double plateActualLength = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].PlateLength);

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);
            // Scale : 매우 중요 함
            double realScaleValue = scaleService.GetScaleCalValue(135, 50, plateActualLength, plateActualWidth);


            SingletonData.RoofComRingPlateList.Clear();


            DrawDetailRoofBottomService roofbottomService = new DrawDetailRoofBottomService(assemblyData, null);

            double cirOD = roofbottomService.GetRoofOuterDiameter();
            double cirODExtend = 0;


            cirODExtend = roofbottomService.GetRoofOD();
            cirOD = cirODExtend;
            if (assemblyData.RoofCompressionRing[0].CompressionRingType.Contains("Detail i"))
                cirOD = roofbottomService.GetRoofODByCompressionRingDetailI();

            double innerWidth = cirODExtend - cirOD;

            if (innerWidth > 0)
                drawList.AddDrawEntity(DrawPlate_CuttingPlan_Divide_V2(PlateArrange_Type.RoofCompressionRing, referencePoint, plateActualWidth, plateActualLength,cirODExtend,innerWidth, realScaleValue));


            return drawList;


        }

        public double GetRoofComRingCutting(out double maxBendPlateOfOnePlate, out double totalBendingPlate)
        {
            double plateActualWidth = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].PlateWidth);
            double plateActualLength = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].PlateLength);

            DrawDetailRoofBottomService roofbottomService = new DrawDetailRoofBottomService(assemblyData, null);

            double cirOD = roofbottomService.GetRoofOuterDiameter();
            double cirODExtend = 0;


            cirODExtend = roofbottomService.GetRoofOD();
            cirOD = cirODExtend;
            if (assemblyData.RoofCompressionRing[0].CompressionRingType.Contains("Detail i"))
                cirOD = roofbottomService.GetRoofODByCompressionRingDetailI();

            double innerWidth = cirODExtend - cirOD;
            double needTotalPlate = 0;
            maxBendPlateOfOnePlate = 0;
            totalBendingPlate = 0;
            if (innerWidth>0)
                needTotalPlate = GetPlateCoutByCutting(plateActualWidth, plateActualLength, cirODExtend, innerWidth,
                                            out maxBendPlateOfOnePlate,
                                            out totalBendingPlate);

            return needTotalPlate;
        }

        public DrawEntityModel DrawBottomAnnularCuttingPlan(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {

            DrawEntityModel drawList = new DrawEntityModel();
            double plateActualWidth = valueService.GetDoubleValue(assemblyData.BottomInput[0].PlateWidth);
            double plateActualLength = valueService.GetDoubleValue(assemblyData.BottomInput[0].PlateLength);

            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);
            // Scale : 매우 중요 함
            double realScaleValue = scaleService.GetScaleCalValue(135, 50, plateActualLength, plateActualWidth);


            SingletonData.BottomAnnularPlateList.Clear();

            DrawDetailRoofBottomService roofbottomService = new DrawDetailRoofBottomService(assemblyData, null);

            double cirOD = 0;
            double cirODExtend = 0;

            cirODExtend = roofbottomService.GetBottomOD();
            cirOD = cirODExtend;
            if (assemblyData.BottomInput[0].AnnularPlate.Contains("Yes"))
                cirOD = roofbottomService.GetAnnularPlateID();

            double innerWidth = (cirODExtend - cirOD)/2;

            if (innerWidth > 0)
                drawList.AddDrawEntity(DrawPlate_CuttingPlan_Divide_V2(PlateArrange_Type.BottomAnnular, referencePoint, plateActualWidth, plateActualLength, cirODExtend, innerWidth, realScaleValue));

            return drawList;


        }

        public double GetBottomAnnularCutting(out double maxBendPlateOfOnePlate, out double totalBendingPlate)
        {
            double plateActualWidth = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].PlateWidth);
            double plateActualLength = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].PlateLength);

            DrawDetailRoofBottomService roofbottomService = new DrawDetailRoofBottomService(assemblyData, null);

            double cirOD = 0;
            double cirODExtend = 0;

            cirODExtend = roofbottomService.GetBottomOD();
            cirOD = cirODExtend;
            if (assemblyData.BottomInput[0].AnnularPlate.Contains("Yes"))
                cirOD = roofbottomService.GetAnnularPlateID();

            double innerWidth = (cirODExtend - cirOD) / 2;
            double needTotalPlate = 0;
            maxBendPlateOfOnePlate = 0;
            totalBendingPlate = 0;
            if (innerWidth > 0)
                needTotalPlate= GetPlateCoutByCutting(plateActualWidth, plateActualLength, cirODExtend, innerWidth,
                                            out maxBendPlateOfOnePlate,
                                            out totalBendingPlate);

            return needTotalPlate;
        }




        private DrawEntityModel DrawPlate_CuttingPlan_Divide_V2(PlateArrange_Type arrangeType, Point3D refPoint,double plateWidth, double plateLength, double radiusOuter,double innerWidth,  double scaleValue)
        {
            // Annular Plate Cutting Plan _ Divide (분할)
            DrawEntityModel drawList = new DrawEntityModel();


            List<DrawOnePlateModel> onePlateList = new List<DrawOnePlateModel>();
            List<Entity> outlinesList = new List<Entity>();
            List<Entity> plateList= new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            ///////////////////////
            ///  CAD Data Setting
            ///////////////////////
            //double plateWidth = 2740;
            //double plateLength = 9300;
            //double radiusouter = 16537;
            //double radiusouter = 24050;
            //double bendingPlateThk = 320;
            double radiusInner = radiusOuter - innerWidth;

            double distanceEachbendingPlate = 15;


            // INFO : After calculate of data
            double needTotalPlate = 0;
            double totalBendingPlate = 0;
            double maxBendPlateOfOnePlate = 0;

            double gapEachBendingPlate = 0;


            bool isRemainderPiece = false;
            double countRemainderPiece = 0;



            ////////////////////////////////////////////// ///////////////////////////////////////////
            needTotalPlate = GetPlateCoutByCutting(plateWidth, plateLength, radiusOuter, innerWidth,
                                                        out maxBendPlateOfOnePlate,
                                                        out totalBendingPlate);
            double eachPlateDegree = 360 / totalBendingPlate;
            double eachPlateHalfDeg = eachPlateDegree / 2;
            gapEachBendingPlate = GetArcSpace(innerWidth, distanceEachbendingPlate, eachPlateHalfDeg);

            // Reset Arc CenterPoint : + arcHeight
            double firstArcStartPointY = plateWidth - distanceEachbendingPlate - radiusOuter;
            //Point3D arcCenterPoint = GetSumPoint(referencePoint, plateLength / 2, firstArcStartPointY);
            //Point3D eachArcCenterPoint = GetSumPoint( arcCenterPoint,0,0);

            //////////////////////////////////////////////////////////
            // Draw BendingPlate
            //////////////////////////////////////////////////////////

            
            double countMaxSheet = Math.Truncate(totalBendingPlate / maxBendPlateOfOnePlate);  // 최대조각으로 된 Sheet 장수
            if ((totalBendingPlate % maxBendPlateOfOnePlate) > 0)
            {
                isRemainderPiece = true;
                countRemainderPiece = totalBendingPlate - (countMaxSheet * maxBendPlateOfOnePlate);  // Sheet 외에 낱개
            }


            int[] countPieceArray = { (int)maxBendPlateOfOnePlate, (int)countRemainderPiece };

            int totalDrawLoop = 1;
            if (isRemainderPiece) totalDrawLoop = 2;


            List<Point3D> outPointList = new List<Point3D>();
            for (int loopCount = 0; loopCount < totalDrawLoop; loopCount++)
            {
                // Draw : Plate
                Point3D plateStartPoint = GetPlateNextPoint(referencePoint, loopCount+1, scaleValue);
                plateList.AddRange(shapeService.GetRectangle(out outPointList, plateStartPoint, plateLength, plateWidth, 0, 0, 3));

                DrawOnePlateModel newOnePlate = new DrawOnePlateModel();
                onePlateList.Add(newOnePlate);
                newOnePlate.PlateWidth = plateWidth;
                newOnePlate.PlateLength = plateLength;
                newOnePlate.InsertPoint = GetSumPoint(plateStartPoint, 0, 0);
                newOnePlate.CenterPoint = GetSumPoint(plateStartPoint, plateLength / 2, plateWidth / 2);

                newOnePlate.Requirement = 1;
                if (loopCount == 0)
                    newOnePlate.Requirement = countMaxSheet;

                Point3D arcCenterPoint = GetSumPoint(plateStartPoint, plateLength / 2, firstArcStartPointY);

                for (int i = 0; i < countPieceArray[loopCount]; i++)
                {
                    Point3D eachArcCenterPoint = GetSumPoint(arcCenterPoint,0,-i * gapEachBendingPlate);

                    Arc outerBendingPlate = new Arc(eachArcCenterPoint, radiusOuter, Utility.DegToRad(90 + eachPlateHalfDeg), Utility.DegToRad(90 - eachPlateHalfDeg));
                    Arc innerBendingPlate = new Arc(eachArcCenterPoint, radiusInner, Utility.DegToRad(90 + eachPlateHalfDeg), Utility.DegToRad(90 - eachPlateHalfDeg));

                    Line sideLeftLine = GetDiagonalLineOfArc(radiusOuter, eachArcCenterPoint, eachPlateHalfDeg, innerBendingPlate, outerBendingPlate);
                    Line sideRightLine = GetDiagonalLineOfArc(radiusOuter, eachArcCenterPoint, -eachPlateHalfDeg, innerBendingPlate, outerBendingPlate);

                    outlinesList.AddRange(new Entity[] {
                                            innerBendingPlate, outerBendingPlate,
                                            sideLeftLine, sideRightLine,
                    });
                }


            }

            styleService.SetLayerListEntity(ref plateList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(plateList);

            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(outlinesList);


            // Singleton Data
            if (arrangeType == PlateArrange_Type.RoofCompressionRing)
            {
                SingletonData.RoofComRingPlateList.Clear();
                SingletonData.RoofComRingPlateList.AddRange(onePlateList);
            }
            else if (arrangeType == PlateArrange_Type.BottomAnnular)
            {
                SingletonData.BottomAnnularPlateList.Clear();
                SingletonData.BottomAnnularPlateList.AddRange(onePlateList);
            }


            return drawList;



            /*
             *  Dimension
             * 
             * arcTop.MidPoint
             * leftTopLine.StartPoint , bottomLeftLine.StartPoint  //  t@
             * rightTopLine.EndPoint , bottomRightLine.EndPoint,   //  OutSide, Inside
             * bottomUpLineRight.StartPoint , bottomUpLineRight.EndPoint   // 1.6
             * bottomDistanceLine  // 3.2
             * 
             */
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

        private double GetPlateCoutByCutting(double plateWidth, double plateLength, double radiusOuter, double innerWidth,
            out double onePlateMaxCount, out double totalBendingPlate)
        {


            ///////////////////////
            ///  CAD Data Setting
            ///////////////////////


            //double radiusouter = 16537;

            double radiusInner = radiusOuter - innerWidth;

            double distanceEachbendingPlate = 15;


            // INFO : After calculate of data
            double needTotalPlate = 0;
            double maxBendPlateOfOnePlate = 0;

            double gapEachBendingPlate = 0;


            bool isRemainderPiece = false;
            double countRemainderPiece = 0;


            //////////////////////////////////////////////////////////
            //// Calculate
            //////////////////////////////////////////////////////////

            totalBendingPlate = GetNeedAnnularCompRingCount(radiusOuter, plateLength, innerWidth);

            double eachPlateDegree = 360 / totalBendingPlate;
            double eachPlateHalfDeg = eachPlateDegree / 2;

            gapEachBendingPlate = GetArcSpace(innerWidth, distanceEachbendingPlate, eachPlateHalfDeg);


            // Clculate : first BendPlate height
            double arcHeight = radiusInner - (radiusInner * Math.Cos(Utility.DegToRad(eachPlateHalfDeg)));
            double firstBendPlateHeight = arcHeight + innerWidth;


            // Calculate : MaxCreate bendingPlate of OnePlate ,  total Plate No.
            double emptyWidth = plateWidth - firstBendPlateHeight - distanceEachbendingPlate;
            maxBendPlateOfOnePlate = Math.Truncate(emptyWidth / gapEachBendingPlate) + 1; // +1 : first BendingPlate
            /*
            double spaceYEachBendPlate = bendingPlateThk + gapEachBendingPlate;
            maxBendPlateOfOnePlate = Math.Truncate(emptyWidth / spaceYEachBendPlate) + 1; // +1 : first BendingPlate
            /**/

            double countMaxSheet = Math.Truncate(totalBendingPlate / maxBendPlateOfOnePlate);  // 최대조각으로 된 Sheet 장수
            if ((totalBendingPlate % maxBendPlateOfOnePlate) > 0)
            {
                isRemainderPiece = true;
                needTotalPlate = countMaxSheet + 1;
                countRemainderPiece = totalBendingPlate - (countMaxSheet * maxBendPlateOfOnePlate);  // Sheet 외에 낱개
            }
            else
            {
                needTotalPlate = countMaxSheet;
            }


            onePlateMaxCount = maxBendPlateOfOnePlate;





            return needTotalPlate;


            /*
             *  Dimension
             * 
             * arcTop.MidPoint
             * leftTopLine.StartPoint , bottomLeftLine.StartPoint  //  t@
             * rightTopLine.EndPoint , bottomRightLine.EndPoint,   //  OutSide, Inside
             * bottomUpLineRight.StartPoint , bottomUpLineRight.EndPoint   // 1.6
             * bottomDistanceLine  // 3.2
             * 
             */
        }

        public double GetArcSpace(double typ, double gap, double halfAngle)
        {
            // A function of calculating the distance to the next panel
            // by receiving the half of the center of arc, the thickness of the panel, and the gap value.
            // typ : thickness of the panel
            // gap : The minimum distance between the panels.



            return typ / Math.Cos(Utility.DegToRad(halfAngle)) + gap;



        }

        public double GetNeedAnnularCompRingCount(double radiusouter, double plateLength, double bendingPlateThk)
        {
            Point3D referencePoint = new Point3D(0, 0);
            double totalBendingPlate = 0;

            // Calculate : bendingPlate Length
            //double minBendPlateCount = Math.Ceiling((2 * radiusouter * Math.PI) / plateLength);
            double minBendPlateCount = Math.Floor((2 * radiusouter * Math.PI) / plateLength);

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


    }
}
