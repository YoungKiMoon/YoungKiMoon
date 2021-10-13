using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawCalculationLib.DrawFunctionServices
{
    public class CRTRoofService
    {
        public void GetRoofCRTValue(out double returnA, out double returnB, out double returnC,
                    out double returnAA, out double returnAB, out double returnAC, out double returnAD, out double returnAE,
                    out double returnBA, out double returnBB, out double returnBC)
        {





            #region // CRT초기값

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
            b = 1; //높이
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
            // L1 = ; //Roof Cutting Plate Length
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
                Case2CRTThetaMax = Case2ThetaMaxrad * RadToDdeg; //2개를 겹쳐서 왼쪽으로 모으는 계산식



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
    }

}
