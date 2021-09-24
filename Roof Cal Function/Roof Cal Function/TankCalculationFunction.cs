using CalFunctionLib.FunctionServices;
using System;
using System.Collections.Generic;

namespace Roof_Cal_Function
{
    internal class TankCalculationFunction


    {
        public void GetRoofCRTValue(out double returnA, out double returnB, out double returnC,
            out double returnAA, out double returnAB, out double returnAC, out double returnAD, out double returnAE,
            out double returnBA, out double returnBB, out double returnBC)
        {



            #region    // CRT초기값


            double Case1CRTThetaMax = 0;
            double Case1CRTQTY = 0;
            double Case1CRTTheta = 0;
            double Case2CRTThetaMax = 0;
            double Case2CRTQTY = 0;
            double Case2CRTTheta = 0;


            double

                //Plate
                PlateLength,
                PlateWidth,
                Overlap,
                CuttingLose,

                //Roof

                L0,
                L1,
                CRTCuttingPlateLength,
                CRTRoofOD,
                RealDeg,
                CuttingDeg,
                R, //slope Radius
                r,//top viwe Radius
                Theta,
                a,//밑변
                b,//높이
                c,//빗변
                RealLength, //DomeRoof 펼친 길이
                CRTRoofSlopeDia,
                CRTD0,
                CRTL0,
                CRTL1
                ;





            // CRT Slope 삼각함수
            a = 15; //밑변
            b = 1;  //높이
            c = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)); //빗변

            RealDeg = a / c * 360;
            CuttingDeg = 360 - RealDeg;
            Overlap = 15;


            #endregion


            //기본값

            CRTRoofOD = 20000;
            CRTRoofSlopeDia = CRTRoofOD * c / a;
            double Dia = Math.Ceiling((CRTRoofSlopeDia / 6) / 100);
            CRTD0 = Dia * 100;





            if (CRTD0 < 1000)
            {
                CRTD0 = 1000;
            }

            CRTL1 = (CRTRoofSlopeDia - CRTD0) / 2;
            CRTCuttingPlateLength = CRTL1 + Overlap;


            // L0 = ; //Roof Center Circle Plate
            // L1 =  ; //Roof Cutting Plate Length
            //L2 = L1 + overlap;



            PlateLength = 9144;
            PlateWidth = 2438;
            CuttingLose = 20;
            // R = L0 + L1;
            // r = CRTRoofOD / 2;
            double RadToDdeg = 180.0 / Math.PI;












           // Case 1, 2 나누기 Function

           if (CRTL1 + Overlap + (CuttingLose / 2) <= PlateLength / 2) // Case 1
           {

            //Case1 CRT

            double Case1CRTThetaMaxrad = Math.Atan((PlateWidth - 20) / (CRTD0 / 2 + CRTL1));
            Case1CRTThetaMax = Case1CRTThetaMaxrad * RadToDdeg;
            Case1CRTQTY = Math.Ceiling(360 / Case1CRTThetaMax);
            Case1CRTTheta = RealDeg / Case1CRTQTY;

           }
          else 
           {

             //Case2 CRT

            double Case2ThetaMaxrad = Math.Asin((PlateWidth - 20) / ((CRTD0 / 2) + (CRTD0 / 2 + CRTL1)));
            Case2CRTThetaMax = Case2ThetaMaxrad * RadToDdeg;    //2개를 겹쳐서 왼쪽으로 모으는 계산식

            Case2CRTQTY = Math.Ceiling(RealDeg / Case2CRTThetaMax);
            Case2CRTTheta = RealDeg / Case2CRTQTY;
           }







            // Return
            returnA = CRTL1;
            returnB = CRTD0;
            returnC = CRTCuttingPlateLength;

            returnAA = Case1CRTThetaMax;
            returnAB = Case1CRTQTY;
            returnAC = Case1CRTTheta;
            returnAD = RealDeg;
            returnAE = CuttingDeg;
            returnBA = Case2CRTThetaMax;
            returnBB = Case2CRTQTY;
            returnBC = Case2CRTTheta;




            
           

        }



        //DRT 
        public void GetRoofDRTValue(out double returnDA, out double returnDB, out double returnDC, out double returnDD, out double returnDE,
            out double returnDF, out double returnDG, out double returnDH, out double returnDI, out double returnDJ, out double returnDL,
            out double returnDM, out double returnDN, out double returnDO)
        {


            #region //초기값

            double CutLayer01Lg = 0;
            double CutLayer02Lg = 0;
            double CutLayer03Lg = 0;
            
            double
            Curvature,
            DRTRoofOD,
            DRTSlopeOD,
            DRTSlopeL0,
            DRTSlopeL1,
            DRTRoofR,
            DRTQTY,
            DRTOverlap,
            DRTL0,
            DRTL1,
            DRTL0Theta,
            DRTRoofODTheta,
            DRTCenterCircletheta,
            DivideLayer,
            CuttingLose,
            CuttingLayer01PointLength, // 6등분, 5선
            CuttingLayer02PointLength, // 6등분, 5선
            CuttingLayer03PointLength, // 6등분, 5선
            Layer01PointLengthTheta,
            Layer02PointLengthTheta,
            Layer03PointLengthTheta,



            PlateLength,
            PlateWidth,

            //Layer01 cuttingPoint
            L01P1R, L01P2R, L01P3R, L01P4R, L01P5R,
            L01P1Circum, L01P2Circum, L01P3Circum, L01P4Circum, L01P5Circum,
            L01P1Wd, L01P2Wd, L01P3Wd, L01P4Wd, L01P5Wd,


            L02P1R, L02P2R, L02P3R, L02P4R, L02P5R,
            L02P1Circum, L02P2Circum, L02P3Circum, L02P4Circum, L02P5Circum,
            L02P1Wd, L02P2Wd, L02P3Wd, L02P4Wd, L02P5Wd,


            L03P1R, L03P2R, L03P3R, L03P4R, L03P5R,
            L03P1Circum, L03P2Circum, L03P3Circum, L03P4Circum, L03P5Circum,
            L03P1Wd, L03P2Wd, L03P3Wd, L03P4Wd, L03P5Wd,


            TankID



            //Case1 DRT


            //Case2 DRT

            ;

            
            PlateLength = 9144;
            PlateWidth = 2438;
            DRTOverlap = 25;
            CuttingLose = 15;
            DRTRoofOD = 20000;
            Curvature = 1; //곡률
            DRTQTY = Math.Ceiling(DRTRoofOD * Math.PI / (PlateWidth - 20)); // 갯수 구하기
            TankID = 20000;




            #endregion



            #region  //Function


            DRTRoofR = TankID * Curvature; // Dome 곡률 반영된 R값




            DRTSlopeL0 = PlateWidth - CuttingLose - (DRTOverlap * 2); // Center Circle 곡률 반영된 길이, overlap 값을 더해서 최대크기이므로 L0값은 -50
            DRTL0Theta = DRTSlopeL0 * 2 / 2 * DRTRoofR * Math.PI * 360; // Center Circle 각도

            DRTL0 = 2 * DRTRoofR * Math.Sin(DRTL0Theta / 2); //Center Circle 반지름 곡률x 
            DRTL1 = (DRTRoofOD - DRTL0 * 2) / 2; // Cutting Plan Length 곡률x




            DRTSlopeOD = 2 * DRTRoofR * Math.Asin((DRTRoofOD / (2 * DRTRoofR))); // DRT Roof 곡률 반영된 길이
            DRTRoofODTheta = DRTSlopeOD / (2 * DRTRoofR * Math.PI) * 360;  // DRTRoofR 중심점에서 Dome의 각도 
            DRTCenterCircletheta = (DRTSlopeL0 * 2) / (2 * DRTRoofR * Math.PI) * 360;

            DRTSlopeL1 = DRTSlopeOD / 2 - DRTSlopeL0; // Cutting Plan 의 길이는 DRTOverlap값을 더해야 함



            // Cutting Plan 6등분 적용 Rule = Plate 에 3장 이상 들어가는 경우
            //bool cutPointRule = true;

            //PlateLength >> (CutLayer01Lg * 3) + (CuttingLose * 2); 


            #endregion










            #region     //Roof 단수 결정
         
            DivideLayer = Math.Ceiling((DRTSlopeL1 + DRTOverlap) / 6000);

            //Cutting Plan 길이가 2단 이상인 경우 나누는 방법

            if (DRTSlopeL1 + DRTOverlap <= 6000) // 6000보다 작은 경우 
            {
                CutLayer01Lg = DRTSlopeL1 + DRTOverlap;
                CutLayer02Lg = 0;
                CutLayer03Lg = 0;
            }

            else if (DRTSlopeL1 + (DRTOverlap * 3) <= 12000)  //12000 보다 작은 경우 2단
            {
                if (DRTSlopeL1 + (DRTOverlap * 3) - 6000 <= 2000)  // 2단이 2000보다 작으면 2단을 2000으로 먼저 고정, 1단 나머지 값
                {
                    CutLayer02Lg = 2000;
                    CutLayer01Lg = DRTSlopeL1 + (DRTOverlap * 3) - CutLayer02Lg;
                    CutLayer03Lg = 0;

                }
                else  //2단이 2000보다 크면 1단 6000, 2단 나머지 값 
                {
                    CutLayer01Lg = 6000;
                    CutLayer02Lg = DRTSlopeL1 + (DRTOverlap * 3) - 6000;
                    CutLayer03Lg = 0;

                }


            }
            else if (DRTSlopeL1 + (DRTOverlap * 5) <= 18000) // 18000보다 작은 경우 3단
            {
                CutLayer01Lg = 6000;  // 1단 6000 고정
                if (DRTSlopeL1 + (DRTOverlap * 5) - 12000 <= 2000)  // 3단이 2000보다 작으면 3단을 2000으로 먼저 고정, 2단 나머지 값
                {
                    CutLayer03Lg = 2000;
                    CutLayer02Lg = DRTSlopeL1 + (DRTOverlap * 5) - 8000;
                }
                else  // 3단이 2000보다 크면 2단 6000, 3단 나머지 값
                    CutLayer02Lg = 6000;
                CutLayer03Lg = DRTSlopeL1 + (DRTOverlap * 5) - 12000;

            }



            #endregion



            #region      //등분 나누는 Function  


            // CutLayer01Lg 값에 -> CutLayer01,02,03Lg 값 적용되어야 함*******************************************************************
            // CutPointRule 적용시 포인트 나눔

            double LaerOverlap01 = 0, LaerOverlap02 = 0, LaerOverlap03 = 0;
            double PlusTheta02 = 0, PlusTheta03 = 0;

            if (DivideLayer == 1)
            {
                LaerOverlap01 = 1;
            }

            else if (DivideLayer == 2)
            {
                LaerOverlap01 = 2;
                LaerOverlap02 = 1;
                PlusTheta02 = (CutLayer02Lg - DRTOverlap) / DRTSlopeOD * 360 * 2;   //2단 의 포인트를 나누기 전 먼저 반영되는 Theta
            }
            else if(DivideLayer == 3)
            {
                LaerOverlap01 = 2;
                LaerOverlap02 = 2;
                LaerOverlap03 = 1;
                PlusTheta02 = (CutLayer02Lg - (DRTOverlap * LaerOverlap02)) / DRTSlopeOD * 360 * 2;   //3단 의 포인트를 나누기 전 먼저 반영되는 Theta
                PlusTheta03 = (CutLayer03Lg - DRTOverlap) / DRTSlopeOD * 360 * 2;    //3단 의 포인트를 나누기 전 먼저 반영되는 Theta
            }


            // Layer01 Cutting Point

            CuttingLayer01PointLength = CutLayer01Lg - (DRTOverlap * LaerOverlap01) / 6; //Cutting Plan 등분 길이
            Layer01PointLengthTheta = CuttingLayer01PointLength / (2 * DRTRoofR * Math.PI) * 360; // DRTRoofR 중심점에서의 등분 각도



            // 포인트별 중심선 까지의 거리 R
            L01P1R = DRTRoofR * Math.Sin((DRTRoofODTheta - (Layer01PointLengthTheta * 2) - (PlusTheta02 + PlusTheta03)) / 2);
            L01P2R = DRTRoofR * Math.Sin((DRTRoofODTheta - (Layer01PointLengthTheta * 4) - (PlusTheta02 + PlusTheta03)) / 2);
            L01P3R = DRTRoofR * Math.Sin((DRTRoofODTheta - (Layer01PointLengthTheta * 6) - (PlusTheta02 + PlusTheta03)) / 2);
            L01P4R = DRTRoofR * Math.Sin((DRTRoofODTheta - (Layer01PointLengthTheta * 8) - (PlusTheta02 + PlusTheta03)) / 2);
            L01P5R = DRTRoofR * Math.Sin((DRTRoofODTheta - (Layer01PointLengthTheta * 10) - (PlusTheta02 + PlusTheta03)) / 2);


            //포인트별 Roof의 원둘레
            L01P1Circum = 2 * Math.PI * L01P1R;
            L01P2Circum = 2 * Math.PI * L01P2R;
            L01P3Circum = 2 * Math.PI * L01P3R;
            L01P4Circum = 2 * Math.PI * L01P4R;
            L01P5Circum = 2 * Math.PI * L01P5R;


            //CuttingPlan 포인트별 넓이
            L01P1Wd = L01P1Circum / DRTQTY + (DRTOverlap  * 2);
            L01P2Wd = L01P2Circum / DRTQTY + (DRTOverlap * 2);
            L01P3Wd = L01P3Circum / DRTQTY + (DRTOverlap * 2);
            L01P4Wd = L01P4Circum / DRTQTY + (DRTOverlap * 2);
            L01P5Wd = L01P5Circum / DRTQTY + (DRTOverlap * 2);





            //Layer02 CuttingPoint


            CuttingLayer02PointLength = CutLayer02Lg - (DRTOverlap * LaerOverlap02) / 6; //Cutting Plan 등분 길이
            Layer02PointLengthTheta = CuttingLayer02PointLength / (2 * DRTRoofR * Math.PI) * 360; // DRTRoofR 중심점에서의 등분 각도



            L02P1R = DRTRoofR * Math.Sin((DRTRoofODTheta - (Layer02PointLengthTheta * 2) - (PlusTheta02 + PlusTheta03)) / 2);
            L02P2R = DRTRoofR * Math.Sin((DRTRoofODTheta - (Layer02PointLengthTheta * 4) - (PlusTheta02 + PlusTheta03)) / 2);
            L02P3R = DRTRoofR * Math.Sin((DRTRoofODTheta - (Layer02PointLengthTheta * 6) - (PlusTheta02 + PlusTheta03)) / 2);
            L02P4R = DRTRoofR * Math.Sin((DRTRoofODTheta - (Layer02PointLengthTheta * 8) - (PlusTheta02 + PlusTheta03)) / 2);
            L02P5R = DRTRoofR * Math.Sin((DRTRoofODTheta - (Layer02PointLengthTheta * 10) - (PlusTheta02 + PlusTheta03)) / 2);


            //포인트별 Roof의 원둘레
            L02P1Circum = 2 * Math.PI * L02P1R;
            L02P2Circum = 2 * Math.PI * L02P2R;
            L02P3Circum = 2 * Math.PI * L02P3R;
            L02P4Circum = 2 * Math.PI * L02P4R;
            L02P5Circum = 2 * Math.PI * L02P5R;


            //CuttingPlan 포인트별 넓이
            L02P1Wd = L02P1Circum / DRTQTY + (DRTOverlap * 2);
            L02P2Wd = L02P2Circum / DRTQTY + (DRTOverlap * 2);
            L02P3Wd = L02P3Circum / DRTQTY + (DRTOverlap * 2);
            L02P4Wd = L02P4Circum / DRTQTY + (DRTOverlap * 2);
            L02P5Wd = L02P5Circum / DRTQTY + (DRTOverlap * 2);





            //Layer03 CuttingPoint

            CuttingLayer03PointLength = CutLayer03Lg - (DRTOverlap * LaerOverlap03) / 6; //Cutting Plan 등분 길이
            Layer03PointLengthTheta = CuttingLayer03PointLength / (2 * DRTRoofR * Math.PI) * 360; // DRTRoofR 중심점에서의 등분 각도



            L03P1R = DRTRoofR * Math.Sin((DRTRoofODTheta - (Layer03PointLengthTheta * 2) - (PlusTheta02 + PlusTheta03)) / 2);
            L03P2R = DRTRoofR * Math.Sin((DRTRoofODTheta - (Layer03PointLengthTheta * 4) - (PlusTheta02 + PlusTheta03)) / 2);
            L03P3R = DRTRoofR * Math.Sin((DRTRoofODTheta - (Layer03PointLengthTheta * 6) - (PlusTheta02 + PlusTheta03)) / 2);
            L03P4R = DRTRoofR * Math.Sin((DRTRoofODTheta - (Layer03PointLengthTheta * 8) - (PlusTheta02 + PlusTheta03)) / 2);
            L03P5R = DRTRoofR * Math.Sin((DRTRoofODTheta - (Layer03PointLengthTheta * 10) - (PlusTheta02 + PlusTheta03)) / 2);


            //포인트별 Roof의 원둘레
            L03P1Circum = 2 * Math.PI * L03P1R;
            L03P2Circum = 2 * Math.PI * L03P2R;
            L03P3Circum = 2 * Math.PI * L03P3R;
            L03P4Circum = 2 * Math.PI * L03P4R;
            L03P5Circum = 2 * Math.PI * L03P5R;


            //CuttingPlan 포인트별 넓이
            L03P1Wd = L03P1Circum / DRTQTY + (DRTOverlap * 2);
            L03P2Wd = L03P2Circum / DRTQTY + (DRTOverlap * 2);
            L03P3Wd = L03P3Circum / DRTQTY + (DRTOverlap * 2);
            L03P4Wd = L03P4Circum / DRTQTY + (DRTOverlap * 2);
            L03P5Wd = L03P5Circum / DRTQTY + (DRTOverlap * 2);



            #endregion




            //Case 1, 2 나누기 Function

            // if (CutLayer01Lg + (CuttingLose / 2) <= PlateLength / 2) // Case 1
            // {
            //CRT 




            //  }
            //  else //Case 2
            //   {

            //   }









            // Return
            returnDA = DRTRoofR;
            returnDB = DRTQTY;
            returnDC = DRTSlopeOD;
            returnDD = DRTSlopeL0;
            returnDE = DRTSlopeL1;

            returnDF = DivideLayer;
            returnDG = CutLayer01Lg;
            returnDH = CutLayer02Lg;
            returnDI = CutLayer03Lg;
            returnDJ = DRTRoofODTheta;
            returnDL = DRTCenterCircletheta;
            returnDM = Layer01PointLengthTheta;
            returnDN = Layer02PointLengthTheta;
            returnDO = Layer03PointLengthTheta;



        }



        private double GetCutPointRule(double a, double b, double c) //6등분 적용하는 Function 
        {


            //  if(PlateLength > (CutLayer01Lg * 3) + (CuttingLose * 2))  //조건
            {

            }

            return a;
            //return b;
            //return c;
        }




        //DRT 
        public void GetRoofDRTValue_New()
        {




            //Case1 DRT


            //Case2 DRT

            #region


            GeomertyFunctions geoSerivce = new GeomertyFunctions();


            // Overlap : DRT
            double DRTOverlap = 25;


            // DRT : 가장 아래 둘래 기준으로 갯수 구하는 공식
            // DRTQTY = Math.Ceiling(DRTRoofOD * Math.PI / (PlateWidth - CuttingLose)); // 갯수 구하기


            double plateLength = 9144;
            double plateWidth = 2438;

            double tankID = 35000;
            double DRTCurvature = 1; //곡률
            double DRTRoofDiameter = tankID * DRTCurvature; // Dome 곡률 반영된 R값
            double DRTRoofRadius = DRTRoofDiameter ;

            double DRTRoofStringLength = 36000;// 입력을 받아야 함

            #endregion


            #region Center Circle
            double platePieceOverlap = 25;
            // Plate Width => Center Circle Arc Length => Center Circle Angle
            // Center Circle 곡률 반영된 길이, overlap 값을 더해서 최대크기이므로 L0값은 -50
            double centerCircleHalfArcLength = GetOptimalLengthByPlateWidth(ShapeType.Arc, plateWidth, platePieceOverlap);
            double centerCircleArcLength = centerCircleHalfArcLength * 2;
            // Center Circle 각도
            double centerCircleAngle = geoSerivce.GetArcAngleByArcLength(DRTRoofRadius, centerCircleArcLength);
            // Center Circle 지름 곡률x 
            double centerCircleStringLength = geoSerivce.GetStringLengthByArcAngle(DRTRoofRadius, centerCircleAngle);
            // Cutting Plan Length 곡률x

            #endregion


            #region  Arrnage Arc Length


            // DRT Roof 곡률 반영된 길이
            double DRTRoofArcLength = geoSerivce.GetArcLengthByStringLength(DRTRoofRadius, DRTRoofStringLength);
            // DRTRoofR 중심점에서 Dome의 각도 
            double DRTRoofArcAngle = geoSerivce.GetArcAngleByArcLength(DRTRoofRadius, DRTRoofArcLength);
            double DRTArrngeArcLength = (DRTRoofArcLength - centerCircleArcLength) / 2;



            #endregion


            #region   Layer

            // 
            List<double> layerLengthList = new List<double>();
            double layerCount = GetRoofArrangeLayerQty(DRTArrngeArcLength,out layerLengthList);
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
                if(minLayerLength < layerLengthOverlapList[i])
                {
                    double plateDiveLine = 6;
                    double oneVerticalLength = layerLengthOverlapList[i] / plateDiveLine;
                    for (int j = 0; j < plateDiveLine; j++)
                    {
                        double tempArcLength = sumLayerLength + (oneVerticalLength * (j + 1));
                        double tempArcAngle = geoSerivce.GetArcAngleByArcLength(DRTRoofRadius, tempArcLength * 2);
                        double tempLineDiameter = geoSerivce.GetStringLengthByArcAngle(DRTRoofRadius, tempArcAngle);

                        double tempLineCircum = geoSerivce.GetCircleCircumByDiameter(tempLineDiameter);
                        double tempLineHeight = tempLineCircum/ layerCountList[i];

                        // tempLine Height : 최종
                    }
                }
                sumLayerLength += layerLengthOverlapList[i];
            }

                #endregion








        }


        public double GetEachLayerOnePlateArcAngle(double layerArcLength, double layerArcBeforeLength, double DRTRoofRadius,double plateWidth, double plateLength)
        {
            double cuttingLoss = GetCuttingLoss();
            double returnValue = 0;

            GeomertyFunctions geoService = new GeomertyFunctions();
            double layerDiameter = geoService.GetStringLengthByArcAngle(DRTRoofRadius, geoService.GetArcAngleByArcLength(DRTRoofRadius,layerArcLength*2));
            double layerRadius = layerDiameter / 2;
            double layerBeforeDiamter = geoService.GetStringLengthByArcAngle(DRTRoofRadius, geoService.GetArcAngleByArcLength(DRTRoofRadius, layerArcBeforeLength * 2));
            double layerBeforeRadius = layerBeforeDiamter / 2;

            double layerPlateRadius = layerRadius - plateLength;

            double h1 = 0; // 원에서 가장 먼
            double h2 = 0; 
            double h3 = 0;

            //가장 넓은 Layer 반지름 < Plate Length
            if (layerRadius + (cuttingLoss*2) <= plateLength)
            {
                // Case 3 : h1 <= Width
                h1 = plateWidth-(cuttingLoss * 2);
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
                        returnValue = plateWidth - (cuttingLoss * 1) - (overlapValue *2);
                        break;
                    case ShapeType.Circle:
                    case ShapeType.CircleArc:
                        returnValue = plateWidth - (cuttingLoss * 2) - (overlapValue *2);
                        break;
                }


            return returnValue;
        }

        private double GetLayerCuttingPieceQty(double radius,double plateWidth)
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

        private double GetRoofArrangeLayerQty(double arrangeArcLength,out List<double> layerLengthList)
        {

            // 겹침 고려 안됨

            double layerMaxLength = 6000;
            double layerMinLength = 2000;
            double layerCount = Math.Ceiling((arrangeArcLength) / layerMaxLength);




            // Arc Center 쪽이  Layer 시작
            layerLengthList = new List<double>();

            // 전체 길이를 Max Length로 나누기
            double remainLength = arrangeArcLength;
            for(int i = 0; i < layerCount; i++)
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
                if(layerLengthList[i]< layerMinLength)
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
            for(int i = 0; i < layerCount; i++)
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


        private double GetEachPieceArcLineByPlateLength(double eachLayerLength,  double plateLength)
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

        private double GetLayerPieceQtyByPlateWidth( double currentLayerArcLength, double arcRadius,double centerArcLength, double plateWidth)
        {
            GeomertyFunctions geoSerivce = new GeomertyFunctions();

            
            double layerArcAngle = GetEachLayerAngle(currentLayerArcLength, arcRadius, centerArcLength);

            double eachLayerStringLength = geoSerivce.GetStringLengthByArcAngle(arcRadius, layerArcAngle);
            double eachLayerArcLength = geoSerivce.GetCircleCircumByDiameter(eachLayerStringLength);



            double sectorMaxWidth = GetOptimalLengthByPlateWidth(ShapeType.CircleArc, plateWidth);

            // Sector
            double sectorQty = Math.Ceiling(eachLayerArcLength / sectorMaxWidth);
            return sectorQty;
        }

        private double GetEachLayerAngle(double layerLength, double arcRadius, double centerArcLength)
        {
            GeomertyFunctions geoSerivce = new GeomertyFunctions();

            double currentLayerArcLength = centerArcLength;
            
            currentLayerArcLength += layerLength * 2;
            double eachLayerAngle = geoSerivce.GetArcAngleByArcLength(arcRadius, currentLayerArcLength);
            
            return eachLayerAngle;
        }

    }




}


     
    

    
    










