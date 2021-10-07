using DrawCalculationLib.DrawModels;
using DrawCalculationLib.FunctionServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawCalculationLib.DrawFunctionServices
{
    public class DRTRoofService
    {

        GeometryService geoService = new GeometryService();
        
        public DRTRoofService()
        {

        }



        //DRT 
        public DRTRoofModel GetRoofDRTValue_New( double tankID,double DRTCurvature, double DRTRoofStringLength, double plateLength, double plateWidth)
        {


            DRTRoofModel newModel = new DRTRoofModel();

            //Case1 DRT


            //Case2 DRT

            #region





            // Overlap : DRT
            newModel.DRTOverlap = 25; // 겹쳐지면 50이 되어야 함


            // DRT : 가장 아래 둘래 기준으로 갯수 구하는 공식
            // DRTQTY = Math.Ceiling(DRTRoofOD * Math.PI / (PlateWidth - CuttingLose)); // 갯수 구하기


            newModel.plateLength = plateLength;
            newModel.plateWidth = plateWidth;

            newModel.tankID = tankID;
            newModel.DRTCurvature = DRTCurvature; //곡률
            newModel.DRTRoofDiameter = newModel.tankID * newModel.DRTCurvature; // Dome 곡률 반영된 R값
            newModel.DRTRoofRadius = newModel.DRTRoofDiameter;

            newModel.DRTRoofStringLength = DRTRoofStringLength;// 입력을 받아야 함

            #endregion


            #region Center Circle
            newModel.platePieceOverlap = 25;
            // Plate Width => Center Circle Arc Length => Center Circle Angle
            // Center Circle 곡률 반영된 길이, overlap 값을 더해서 최대크기이므로 L0값은 -50
            newModel.centerCircleHalfArcLength = GetOptimalLengthByPlateWidth(ShapeType.Arc, newModel.plateWidth, newModel.platePieceOverlap);
            newModel.centerCircleArcLength = newModel.centerCircleHalfArcLength * 2;
            // Center Circle 각도
            newModel.centerCircleAngle = geoService.GetArcAngleByArcLength(newModel.DRTRoofRadius, newModel.centerCircleArcLength);
            // Center Circle 지름 곡률x 
            newModel.centerCircleStringLength = geoService.GetStringLengthByArcAngle(newModel.DRTRoofRadius, newModel.centerCircleAngle);
            // Cutting Plan Length 곡률x

            #endregion


            #region  Arrnage Arc Length


            // DRT Roof 곡률 반영된 길이
            newModel.DRTRoofArcLength = geoService.GetArcLengthByStringLength(newModel.DRTRoofRadius, newModel.DRTRoofStringLength);
            // DRTRoofR 중심점에서 Dome의 각도 
            newModel.DRTRoofArcAngle = geoService.GetArcAngleByArcLength(newModel.DRTRoofRadius, newModel.DRTRoofArcLength);
            newModel.DRTArrngeArcLength = (newModel.DRTRoofArcLength - newModel.centerCircleArcLength) / 2;



            #endregion


            #region   Layer

            List<double> tempOutLayerLengthList = new List<double>();
            newModel.layerCount = GetRoofArrangeLayerQty(newModel.DRTArrngeArcLength,newModel.plateLength, out tempOutLayerLengthList);
            List<double> tempOutLayerLengthOverlapList= GetRoofArrngeLayerOverlap(tempOutLayerLengthList);


            #endregion


            #region Each Layer : Cutting
            // 각 Layer 가장 큰 원을 기준으로 
            double sumOfHalfArcLength = newModel.centerCircleHalfArcLength;

            double beforeLayerStringLength= newModel.centerCircleStringLength/2;
            for ( int i = 0; i < tempOutLayerLengthOverlapList.Count; i++)
            {
                
                double eachLayerMaxAngle = GetEachLayerOnePlateArcAngle(
                                                beforeLayerStringLength - newModel.platePieceOverlap + tempOutLayerLengthOverlapList[i],
                                                beforeLayerStringLength - newModel.platePieceOverlap,
                                                newModel.DRTRoofRadius,
                                                newModel.plateWidth,
                                                newModel.plateLength
                                                );


                DRTLayerModel newLayer = new DRTLayerModel();
                newLayer.ArcLength = tempOutLayerLengthList[i];
                newLayer.ArcLengthOverlap = tempOutLayerLengthOverlapList[i];


                newLayer.LayerRoofRadius = newModel.DRTRoofRadius;
                newLayer.BeforeHalfArcLengthFromCenter = sumOfHalfArcLength;

                newLayer.BeforeLayerAngleFromCenter= geoService.GetArcAngleByArcLength(newLayer.LayerRoofRadius, newLayer.BeforeHalfArcLengthFromCenter *2 );
                newLayer.BeforeHalfStringLengthFromCenter = geoService.GetStringLengthByArcAngle(newLayer.LayerRoofRadius, newLayer.BeforeLayerAngleFromCenter) /2;

                newLayer.CurrentHalfStringLengthFromCenter = geoService.GetStringLengthByArcAngle(newLayer.LayerRoofRadius, geoService.GetArcAngleByArcLength(newLayer.LayerRoofRadius, (newLayer.BeforeHalfArcLengthFromCenter +newLayer.ArcLength) * 2)) / 2;
                newLayer.StringLength = newLayer.CurrentHalfStringLengthFromCenter - newLayer.BeforeHalfStringLengthFromCenter;

                newLayer.Count = Math.Ceiling(360 / eachLayerMaxAngle);
                newLayer.Angle = 360 / newLayer.Count;
                newModel.layerList.Add(newLayer);


                sumOfHalfArcLength += newLayer.ArcLength;
                beforeLayerStringLength = newLayer.CurrentHalfStringLengthFromCenter;
            }


            // 최적의 
            //SetRoofArrangeLayerAdj(newModel.layerList);
            SetRoofArrangeLayerAdj_New(newModel.layerList);

            // 등분 나누는 공식
            double plateDivLine = 6; // 매우 중요
            double sumLayerLength = newModel.centerCircleHalfArcLength;
            double minLayerLength = (newModel.plateLength - (GetCuttingLoss() * 4)) / 3;
            double DRTRoofCircum = geoService.GetCircleCircumByRadius(newModel.DRTRoofRadius);
            for (int i = 0; i < newModel.layerList.Count; i++)
            {

                double arcInnerOverlap = newModel.platePieceOverlap;
                double arcOuterOverlap = newModel.platePieceOverlap;
                if (i == newModel.layerList.Count-1)
                    arcOuterOverlap = 0;

                // Overlap 미적용
                DRTRoofPlateModel newPlateModel = new DRTRoofPlateModel();


                // Div 공식
                double plateDivLineAdj = 1;
                if (minLayerLength < newModel.layerList[i].ArcLength)
                    plateDivLineAdj = plateDivLine;

                
                newPlateModel.plateDivLine = plateDivLineAdj;
                newPlateModel.oneVerticalLength = newModel.layerList[i].ArcLength/ plateDivLineAdj;
                double arcInnerRadius = newModel.layerList[i].BeforeHalfArcLengthFromCenter ;
                for (int j = 0; j <= plateDivLineAdj; j++)
                {
                    DRTRoofPlateLineModel newPlateLineModel = new DRTRoofPlateLineModel();

                    double arcLength = arcInnerRadius + (newPlateModel.oneVerticalLength * j);
                    double centerPointAngle = 360 * arcLength / DRTRoofCircum;
                    //newPlateLineModel.Radius = newModel.DRTRoofRadius * Math.Tan(geoService.DegreeToRadian(centerPointAngle));

                    double tempArcAngle = geoService.GetArcAngleByArcLength(newModel.DRTRoofRadius, arcLength * 2);
                    double tempLineDiameter = geoService.GetStringLengthByArcAngle(newModel.DRTRoofRadius, tempArcAngle);
                    double tempLineRadius = tempLineDiameter / 2;
                    newPlateLineModel.Radius = tempLineRadius;
                    //newPlateLineModel.ArcAngle = 2 * Math.PI * tempLineRadius / (newPlateLineModel.Radius * newModel.layerList[i].Count);
                    //newPlateLineModel.ArcAngle = 2 * Math.PI * tempLineRadius / ( newModel.layerList[i].Count);
                    newPlateLineModel.ArcAngle = 360 / (newModel.layerList[i].Count);
                    newPlateLineModel.StringLength = geoService.GetStringLengthByArcAngle(newPlateLineModel.Radius, newPlateLineModel.ArcAngle);

                    // Overlap
                    newPlateLineModel.InnerOverlap = arcInnerOverlap;
                    newPlateLineModel.OuterOverlap = arcOuterOverlap;

                    // One Div Length
                    newPlateLineModel.OneDivLength = newPlateModel.oneVerticalLength;

                    // tempLine Height : 최종
                    newPlateModel.PlateLineList.Add(newPlateLineModel);
                }
                


                sumLayerLength += newModel.layerList[i].ArcLength;

                newModel.layerList[i].PlateList.Add(newPlateModel);
            }

            #endregion





            return newModel;


        }


        private double GetEachLayerOnePlateArcAngle(double layerStringLength, double beforeLayerStringLength, double DRTRoofRadius, double plateWidth, double plateLength)
        {
            double cuttingLoss = GetCuttingLoss();
            double returnValue = 0;

            //double layerDiameter = geoService.GetStringLengthByArcAngle(DRTRoofRadius, geoService.GetArcAngleByArcLength(DRTRoofRadius, layerArcLength * 2));
            //double layerRadius = layerDiameter / 2;
            //double layerBeforeDiamter = geoService.GetStringLengthByArcAngle(DRTRoofRadius, geoService.GetArcAngleByArcLength(DRTRoofRadius, layerArcBeforeLength * 2));
            //double layerBeforeRadius = layerBeforeDiamter / 2;

            //double layerPlateRadius = layerRadius - plateLength;

            double h1 = 0; // 원에서 가장 먼
            double h2 = 0;
            double h3 = 0;

            //가장 넓은 Layer 반지름 < Plate Length
            if (layerStringLength + (cuttingLoss * 2) <= plateLength)
            {
                // 1단 만, 적용되면 : 2개 들어 갈 수 있도록 최대 높이

                // Case 3 : h1 <= Width
                h1 = plateWidth - (cuttingLoss * 2);
                double tempLength = geoService.GetArcLengthByStringLength(layerStringLength, h1 * 2) / 2; // 절반
                returnValue = geoService.GetArcAngleByArcLength(layerStringLength, tempLength);
            }
            else
            {
                // Plate Length  이후 : 2개 들어 갈 수 있도록 최대 높이

                // Case 2 : h1 + h3 + g <= Width
                // Case 1 : h1 + h2 + g <= Width : 고려 안함
                // Max Angle
                // 최적 계산 : 보류
                //double tempValue = Math.Asin((plateWidth - (cuttingLoss * 3)) / (layerPlateRadius + layerRadius));
                //returnValue = geoService.RadianToDegree(tempValue);


                // Case 3 : h1 <= Width
                h1 = plateWidth - (cuttingLoss * 2);
                double tempLength = geoService.GetArcLengthByStringLength(layerStringLength, h1 * 2) / 2; // 절반
                returnValue = geoService.GetArcAngleByArcLength(layerStringLength, tempLength);


            }

            return returnValue;
        }




        private enum ShapeType
        {
            Circle,
            Arc,
            CircleArc,

        }

        private double GetOptimalLengthByPlateWidth(ShapeType selShape, double plateWidth, double overlapValue = 0)
        {
            double cuttingLoss = GetCuttingLoss();
            double returnValue = 0;

            if (plateWidth > 0)
                switch (selShape)
                {
                    case ShapeType.Arc:
                        returnValue = plateWidth - (cuttingLoss * 1) - (overlapValue * 2);
                        break;
                    case ShapeType.Circle:
                    case ShapeType.CircleArc:
                        returnValue = plateWidth - (cuttingLoss * 2) - (overlapValue * 2);
                        break;
                }


            return returnValue;
        }

        private double GetLayerCuttingPieceQty(double radius, double plateWidth)
        {
            double cuttingLoss = GetCuttingLoss();

            double returnValue = 0;
            if (radius > 0 && plateWidth > 0)
            {
                Math.Ceiling(2 * radius * Math.PI / (plateWidth - (cuttingLoss * 2)));
            }

            return returnValue;
            // DRTQTY = Math.Ceiling(DRTRoofOD * Math.PI / (PlateWidth - CuttingLose)); // 갯수 구하기
        }

        private double GetCuttingLoss()
        {
            return 15;
        }

        private double GetRoofPlateOverlapValue()
        {
            return 25;
        }

        private double GetRoofArrangeLayerQty(double arrangeArcLength,double plateLength, out List<double> layerLengthList)
        {

            // 겹침 고려 안됨

            double layerMaxLength = 6000;
            double layerMinLength = 2000;
            double layerCount = Math.Ceiling((arrangeArcLength) / layerMaxLength);

            double plateOverlap = GetRoofPlateOverlapValue();
            double tempLayerLength = plateLength - plateOverlap * 3;// 3으로
            double realLayerLength = Math.Truncate(tempLayerLength * 0.01) * 100;


            // Arc Center 쪽이  Layer 시작
            layerLengthList = new List<double>();

            // 전체 길이를 Max Length로 나누기
            double remainLength = arrangeArcLength;
            for (int i = 0; i < layerCount; i++)
            {
                if (remainLength > 0)
                {
                    if (remainLength > layerMaxLength)
                    {
                        layerLengthList.Add(layerMaxLength);
                        remainLength -= layerMaxLength;
                    }
                    else
                    {
                        layerLengthList.Add(remainLength);
                        remainLength -= remainLength; ;
                    }

                    layerMaxLength = realLayerLength;
                }
                else
                {
                    break;
                }
            }

            layerCount = layerLengthList.Count;
            // Min Length 값 보완
            for (int i = 0; i < layerCount; i++)
            {
                if (layerLengthList[i] < layerMinLength)
                {
                    if (i - 1 >= 0)
                    {
                        
                        double tempLength = layerMinLength - layerLengthList[i];
                        double tempLayerLastLengthRound = IntRound(tempLength, -1);
                        layerLengthList[i - 1] -= tempLayerLastLengthRound;
                        layerLengthList[i] = layerLengthList[i] + tempLayerLastLengthRound;
                    }
                }
            }


            return layerCount;
        }


        public double IntRound(double Value, int Digit)
        {
            double Temp = Math.Pow(10.0, Digit);
            return Math.Round(Value * Temp) / Temp;
        }

        private void SetRoofArrangeLayerAdj(List<DRTLayerModel> layerList)
        {
            // Angle : Factor
            double angleFactor = 0.01;

            // Outer -> Inner
            for (int i = layerList.Count - 1; i > 0; i--)
            {
                DRTLayerModel refLayer = layerList[i];

                if (i == layerList.Count-1)
                {
                    if (refLayer.Count % 2 == 1)
                    {
                        refLayer.Count++;
                        refLayer.Angle = 360 / refLayer.Count;
                    }
                }

                double refStartAngel = refLayer.startAngle;

                DRTLayerModel currentLayer = layerList[i-1];
                double currentStartAngle = currentLayer.startAngle;

                List<double> refList = GetAngleList(refStartAngel, refLayer.Angle, refLayer.Count);

                bool runOptimalValue = true;
                double sumOfCheckAngle = 0;
                while (runOptimalValue)
                {
                    List<double> currentList = GetAngleList(currentStartAngle, currentLayer.Angle, currentLayer.Count);
                    double checkAngle = 0;
                    if (CheckLayerAngle(currentLayer.BeforeHalfStringLengthFromCenter, refList, currentList,out checkAngle))
                    {
                        runOptimalValue = false;
                        //currentLayer.startAngle += sumOfCheckAngle;
                    }
                    else
                    {
                        sumOfCheckAngle += checkAngle;
                        if (sumOfCheckAngle < currentLayer.Angle)
                        {
                            // 시작 각도 변화 
                            currentStartAngle += checkAngle;
                        }
                        else
                        {
                            // 개수 변화
                            currentLayer.Count++;
                            currentLayer.Angle = 360 / currentLayer.Count;

                            // 시작 각도 초기화
                            sumOfCheckAngle = 0;
                            currentStartAngle = currentLayer.startAngle;
                        }
                    }
                }

                SetStartAngle(refLayer, currentLayer);

            }



        }


        // 369 패턴
        private void SetRoofArrangeLayerAdj_New(List<DRTLayerModel> layerList)
        {
            // Angle : Factor
            double angleFactor = 0.01;

            // 3의 배수 조정
            for (int i = 0; i < layerList.Count; i++)
            {
                DRTLayerModel refLayer = layerList[i];

                double tempCount = refLayer.Count;
                double tempValue = Math.Ceiling(tempCount / 3) * 3;

                refLayer.Count = tempValue;
                refLayer.Angle = 360 / refLayer.Count;
                
            }

            // 시작 각도 
            for (int i = 0; i < layerList.Count-1; i++)
            {
                double tempAngle = GetCircleOptimalAngle(layerList[i].Count, layerList[i+1].Count); ;
                double tempLayerAngleHalf = 0;
                if ((layerList[i + 1].Count % 2) == 0)
                {
                    tempLayerAngleHalf = Math.Ceiling(layerList[i + 1].Count / 3) / LCMExt(layerList[i].Count, layerList[i + 1].Count); 
                }
                else
                {
                    tempLayerAngleHalf = Math.Ceiling(layerList[i + 1].Count / 2) / LCMExt(layerList[i].Count, layerList[i + 1].Count); 
                }
                double tempLayerAngle = tempLayerAngleHalf * layerList[i + 1].Angle;
                for (int j = i + 1; j < layerList.Count ; j++)
                {
                    layerList[j].startAngle += tempAngle; // + tempLayerAngle;
                }
            }

        }


        public double GetCircleOptimalAngle(double firstCount, double secondCount)
        {
            return 360 / LCM(firstCount, secondCount, GCD(firstCount, secondCount)) / 2;
        }

        public double LCMExt(double firstCount, double secondCount)
        {
            return  LCM(firstCount, secondCount, GCD(firstCount, secondCount)) ;
        }

        // 최대 공약수
        public double GCD(double num1, double num2)
        {
            double temp = 0;
            while (num2 != 0)
            {
                temp = num1 % num2;
                num1 = num2;
                num2 = temp;
            }

            return num1;
        }

        // 최소 공배수
        public double LCM(double num1, double num2, double gcd)
        {
            if (gcd == 0) return 0;

            double result = (num1 * num2) / gcd;

            return result;
        }



        private void SetStartAngle(DRTLayerModel refLayer, DRTLayerModel currentLayer)
        {
            // Start Angle 정하기


            double angleRemain = refLayer.Count % currentLayer.Count;
            if (angleRemain == 0)
            {
                // 같거나 배수이며 : 1/2
                currentLayer.startAngle += refLayer.startAngle + refLayer.Angle / 2;
            }
            else
            {
                // 이외는 차이값 : 1/2
                double tempValue = currentLayer.Angle - refLayer.Angle;
                double tempRemain = tempValue % refLayer.Angle;

                if (tempRemain == 0)
                {
                    currentLayer.startAngle += refLayer.startAngle + refLayer.Angle / 2;
                }
                else
                {
                    currentLayer.startAngle += refLayer.startAngle + tempRemain / 2;
                }
            }
        }

        private bool CheckLayerAngle(double selRadius, List<double> refList, List<double> currentList,out double checkAngle)
        {
            bool returnValue = true;

            checkAngle = 0;

            double marginHalf =50;
            double angleMargin = geoService.GetArcAngleByArcLength(selRadius, marginHalf);
            double startAngle = 0;
            double endAngle = 0;

            foreach(double eachAngle in refList)
            {
                startAngle = GetSumOfCircleAngle(eachAngle, -angleMargin);
                endAngle = GetSumOfCircleAngle(eachAngle, +angleMargin);
                foreach (double eachCurrentAngle in currentList)
                {
                    if (startAngle <= eachCurrentAngle && eachCurrentAngle <= endAngle)
                    {

                        checkAngle = endAngle - eachCurrentAngle  + 0.1;
                       
                        returnValue = false;
                        break;
                    }
                }
                if (!returnValue)
                    break;
            }

            return returnValue;
        }

        private double GetSumOfCircleAngle(double refAngle, double newValue)
        {
            double returnValue = refAngle + newValue;
            if (returnValue < 0)
                returnValue = 360 - returnValue;

            return returnValue;
        }

        private List<double> GetAngleList(double startAngle, double perAngle,double plateCount)
        {
            List<double> newList = new List<double>();

            double currentAngle = startAngle;
            for(int i = 1; i <= plateCount; i++)
            {
                newList.Add(currentAngle);
                currentAngle += perAngle;
            }
            return newList;
        }

        private List<double> GetRoofArrngeLayerOverlap(List<double> layerLengthList)
        {
            double overlapValue = GetRoofPlateOverlapValue();
            double layerCount = layerLengthList.Count;

            List<double> layerLengthOverlapList = new List<double>();
            for (int i = 0; i < layerCount; i++)
            {
                if (i == layerCount)
                {
                    layerLengthOverlapList.Add(layerLengthList[i] + overlapValue);
                }
                else
                {
                    layerLengthOverlapList.Add(layerLengthList[i] + overlapValue * 2);
                }
            }


            return layerLengthOverlapList;
        }


        private double GetEachPieceArcLineByPlateLength(double eachLayerLength, double plateLength)
        {


            double cuttingLoss = GetCuttingLoss();
            double returnValue = 0;

            // 차후 더 정확한 Arrange 값 필요 함
            double theeArrangeValue = (eachLayerLength * 3) + (cuttingLoss * 4);
            if (plateLength > theeArrangeValue)
            {
                returnValue = 0;
            }
            else
            {
                returnValue = 6;
            }
            return returnValue;
        }

        private double GetLayerPieceQtyByPlateWidth(double currentLayerArcLength, double arcRadius, double centerArcLength, double plateWidth)
        {

            double layerArcAngle = GetEachLayerAngle(currentLayerArcLength, arcRadius, centerArcLength);

            double eachLayerStringLength = geoService.GetStringLengthByArcAngle(arcRadius, layerArcAngle);
            double eachLayerArcLength = geoService.GetCircleCircumByDiameter(eachLayerStringLength);



            double sectorMaxWidth = GetOptimalLengthByPlateWidth(ShapeType.CircleArc, plateWidth);

            // Sector
            double sectorQty = Math.Ceiling(eachLayerArcLength / sectorMaxWidth);
            return sectorQty;
        }

        private double GetEachLayerAngle(double layerLength, double arcRadius, double centerArcLength)
        {

            double currentLayerArcLength = centerArcLength;

            currentLayerArcLength += layerLength * 2;
            double eachLayerAngle = geoService.GetArcAngleByArcLength(arcRadius, currentLayerArcLength);

            return eachLayerAngle;
        }

    }
}
