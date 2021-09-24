using System;

namespace Roof_Cal_Function
{
    class Program
    {
        static void Main(string[] args)
        {
            TankCalculationFunction calFunction = new TankCalculationFunction();

            calFunction.GetRoofCRTValue(out double CRTL1, out double CRTD0, out double CRTCuttingPlateLength,
                out double Case1CRTThetaMax, out double Case1CRTQTY, out double Case1CRTTheta, out double RealDeg, out double CuttingDeg, 
                out double Case2CRTThetaMax, out double Case2CRTQTY, out double Case2CRTTheta);


            double DRTOverlap = 25;


            // CRT
            Console.WriteLine("RealDeg =" + RealDeg);
            Console.WriteLine("CuttingDeg =" + CuttingDeg);

            Console.WriteLine("CRTD0 =" + CRTD0);
            Console.WriteLine("CRTL1 =" + CRTL1);
            Console.WriteLine("CRTCuttingPlateLength = " + CRTCuttingPlateLength);

            //Case1
            Console.WriteLine(" ");
            Console.WriteLine("Case1, CRT");
            Console.WriteLine("Case1CRTThetaMax = " + Case1CRTThetaMax);
            Console.WriteLine("Case1CRTQTY = " + Case1CRTQTY);
            Console.WriteLine("Case1CRTTheta =" + Case1CRTTheta);

            //Case2

            Console.WriteLine(" ");
            Console.WriteLine("Case2, CRT");
            Console.WriteLine("Case2CRTThetaMax = " + Case2CRTThetaMax);
            Console.WriteLine("Case2CRTQTY = " + Case2CRTQTY);
            Console.WriteLine("Case2CRTTheta = " + Case2CRTTheta);



            //DRT

            calFunction.GetRoofDRTValue(out double DRTRoofR, out double DRTQTY, out double DRTSlopeOD, out double DRTSlopeL0, out double DRTSlopeL1,
                out double DivideLayer, out double CutLayer01Lg, out double CutLayer02Lg, out double CutLayer03Lg,
                out double DRTRoofODTheta, out double DRTCenterCircletheta,
                out double Layer01PointLengthTheta, out double Layer02PointLengthTheta, out double Layer03PointLengthTheta);

            Console.WriteLine(" ");
            Console.WriteLine("Case1, DRT");
            Console.WriteLine("DRTRoofR = " + DRTRoofR);
            Console.WriteLine("DRTQTY = " + DRTQTY);
            Console.WriteLine("DRTSlopeOD = " + DRTSlopeOD);
            Console.WriteLine("DRTSlopeL0 = " + DRTSlopeL0);
            Console.WriteLine("DRTSlopeL1 = " + DRTSlopeL1);
            Console.WriteLine("Center Circle Radius = " + DRTSlopeL0 + DRTOverlap * 2);
            Console.WriteLine("DRTRoofODTheta =" + DRTRoofODTheta);
            Console.WriteLine("DRTCenterCircletheta = " + DRTCenterCircletheta);
            Console.WriteLine(" ");

            Console.WriteLine("DivideLayer = " + DivideLayer);
            Console.WriteLine("CutLayer01Lg = " + CutLayer01Lg);
            Console.WriteLine("CutLayer02Lg = " + CutLayer02Lg);
            Console.WriteLine("CutLayer03Lg = " + CutLayer03Lg);
            Console.WriteLine("Layer01PointLengthTheta = " + Layer01PointLengthTheta);
            Console.WriteLine("Layer02PointLengthTheta = " + Layer02PointLengthTheta);
            Console.WriteLine("Layer03PointLengthTheta = " + Layer03PointLengthTheta);


            calFunction.GetRoofDRTValue_New();




        }




    }
}
