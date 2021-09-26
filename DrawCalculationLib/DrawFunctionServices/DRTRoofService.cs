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
        public void GetRoofDRTValue_New()
        {




            //Case1 DRT


            //Case2 DRT

            #region





            // Overlap : DRT
            double DRTOverlap = 25;


            // DRT : 가장 아래 둘래 기준으로 갯수 구하는 공식
            // DRTQTY = Math.Ceiling(DRTRoofOD * Math.PI / (PlateWidth - CuttingLose)); // 갯수 구하기


            double plateLength = 9144;
            double plateWidth = 2438;

            double tankID = 35000;
            double DRTCurvature = 1; //곡률
            double DRTRoofDiameter = tankID * DRTCurvature; // Dome 곡률 반영된 R값
            double DRTRoofRadius = DRTRoofDiameter;

            double DRTRoofStringLength = 36000;// 입력을 받아야 함

            #endregion


            #region Center Circle
            double platePieceOverlap = 25;
            // Plate Width => Center Circle Arc Length => Center Circle Angle
            // Center Circle 곡률 반영된 길이, overlap 값을 더해서 최대크기이므로 L0값은 -50
            double centerCircleHalfArcLength = GetOptimalLengthByPlateWidth(ShapeType.Arc, plateWidth, platePieceOverlap);
            double centerCircleArcLength = centerCircleHalfArcLength * 2;
            // Center Circle 각도
            double centerCircleAngle = geoService.GetArcAngleByArcLength(DRTRoofRadius, centerCircleArcLength);
            // Center Circle 지름 곡률x 
            double centerCircleStringLength = geoService.GetStringLengthByArcAngle(DRTRoofRadius, centerCircleAngle);
            // Cutting Plan Length 곡률x

            #endregion


            #region  Arrnage Arc Length


            // DRT Roof 곡률 반영된 길이
            double DRTRoofArcLength = geoService.GetArcLengthByStringLength(DRTRoofRadius, DRTRoofStringLength);
            // DRTRoofR 중심점에서 Dome의 각도 
            double DRTRoofArcAngle = geoService.GetArcAngleByArcLength(DRTRoofRadius, DRTRoofArcLength);
            double DRTArrngeArcLength = (DRTRoofArcLength - centerCircleArcLength) / 2;



            #endregion


            #region   Layer

            // 
            List<double> layerLengthList = new List<double>();
            double layerCount = GetRoofArrangeLayerQty(DRTArrngeArcLength, out layerLengthList);
            List<double> layerLengthOverlapList = GetRoofArrngeLayerOverlap(layerLengthList);


            #endregion


            #region Each Layer : Cutting
            // 각 Layer 가장 큰 원을 기준으로 
            List<double> layerCountList = new List<double>();
            List<double> layerAngleList = new List<double>();
            double beforeLayerLength = centerCircleHalfArcLength;
            for (int i = 0; i < layerLengthOverlapList.Count; i++)
            {
                double eachLayerMaxAngle = GetEachLayerOnePlateArcAngle(
                                                layerLengthOverlapList[i] + beforeLayerLength,
                                                beforeLayerLength,
                                                DRTRoofRadius,
                                                plateWidth,
                                                plateLength
                                                );
                beforeLayerLength += layerLengthOverlapList[i];

                double eachLayerOnePlateCount = Math.Ceiling(360 / eachLayerMaxAngle);
                double eachLayerOnePlateAngle = 360 / eachLayerOnePlateCount;

                layerCountList.Add(eachLayerOnePlateCount);
                layerAngleList.Add(eachLayerOnePlateAngle);
            }

            // 등분 나누는 공식
            double sumLayerLength = centerCircleHalfArcLength;
            double minLayerLength = (plateLength - (GetCuttingLoss() * 4)) / 3;
            for (int i = 0; i < layerLengthOverlapList.Count; i++)
            {
                if (minLayerLength < layerLengthOverlapList[i])
                {
                    double plateDiveLine = 6;
                    double oneVerticalLength = layerLengthOverlapList[i] / plateDiveLine;
                    for (int j = 0; j < plateDiveLine; j++)
                    {
                        double tempArcLength = sumLayerLength + (oneVerticalLength * (j + 1));
                        double tempArcAngle = geoService.GetArcAngleByArcLength(DRTRoofRadius, tempArcLength * 2);
                        double tempLineDiameter = geoService.GetStringLengthByArcAngle(DRTRoofRadius, tempArcAngle);

                        double tempLineCircum = geoService.GetCircleCircumByDiameter(tempLineDiameter);
                        double tempLineHeight = tempLineCircum / layerCountList[i];

                        // tempLine Height : 최종
                    }
                }
                sumLayerLength += layerLengthOverlapList[i];
            }

            #endregion








        }


        public double GetEachLayerOnePlateArcAngle(double layerArcLength, double layerArcBeforeLength, double DRTRoofRadius, double plateWidth, double plateLength)
        {
            double cuttingLoss = GetCuttingLoss();
            double returnValue = 0;

            double layerDiameter = geoService.GetStringLengthByArcAngle(DRTRoofRadius, geoService.GetArcAngleByArcLength(DRTRoofRadius, layerArcLength * 2));
            double layerRadius = layerDiameter / 2;
            double layerBeforeDiamter = geoService.GetStringLengthByArcAngle(DRTRoofRadius, geoService.GetArcAngleByArcLength(DRTRoofRadius, layerArcBeforeLength * 2));
            double layerBeforeRadius = layerBeforeDiamter / 2;

            double layerPlateRadius = layerRadius - plateLength;

            double h1 = 0; // 원에서 가장 먼
            double h2 = 0;
            double h3 = 0;

            //가장 넓은 Layer 반지름 < Plate Length
            if (layerRadius + (cuttingLoss * 2) <= plateLength)
            {
                // Case 3 : h1 <= Width
                h1 = plateWidth - (cuttingLoss * 2);
                double tempLength = geoService.GetArcLengthByStringLength(layerRadius, h1 * 2) / 2; // 절반
                returnValue = geoService.GetArcAngleByArcLength(layerRadius, tempLength);
            }
            else
            {
                // Case 2 : h1 + h3 + g <= Width
                // Case 1 : h1 + h2 + g <= Width : 고려 안함
                // Max Angle
                double tempValue = Math.Asin((plateWidth - (cuttingLoss * 3)) / (layerPlateRadius + layerRadius));
                returnValue = geoService.RadianToDegree(tempValue);


            }

            return returnValue;
        }




        public enum ShapeType
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

        private double GetRoofArrangeLayerQty(double arrangeArcLength, out List<double> layerLengthList)
        {

            // 겹침 고려 안됨

            double layerMaxLength = 6000;
            double layerMinLength = 2000;
            double layerCount = Math.Ceiling((arrangeArcLength) / layerMaxLength);




            // Arc Center 쪽이  Layer 시작
            layerLengthList = new List<double>();

            // 전체 길이를 Max Length로 나누기
            double remainLength = arrangeArcLength;
            for (int i = 0; i < layerCount; i++)
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
            }
            // Min Length 값 보완
            for (int i = 0; i < layerCount; i++)
            {
                if (layerLengthList[i] < layerMinLength)
                {
                    if (i - 1 >= 0)
                    {
                        double tempLength = layerMinLength - layerLengthList[i];
                        layerLengthList[i - 1] -= tempLength;
                        layerLengthList[i] = layerMinLength;
                    }
                }
            }


            return layerCount;
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
