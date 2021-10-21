using AssemblyLib.AssemblyModels;
using DrawWork.Commons;
using DrawWork.DrawServices;
using DrawWork.DrawStyleServices;
using DrawWork.ValueServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawCommonServices
{
    public class DrawPublicService
    {
        private AssemblyModel assemblyData;

        private ValueService valueService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;

        private DrawEditingService editingService;

        public DrawPublicService(AssemblyModel selAssembly)
        {
            assemblyData = selAssembly;

            valueService = new ValueService();
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();


            editingService = new DrawEditingService();
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
                switch (topAngleType)
                {
                    case "Detail b":
                        actualRadius += valueService.GetDoubleValue(eachAngle.E) +
                                        valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness);
                        retrunValue = actualRadius * 2;
                        break;
                    case "Detail d":
                        actualRadius += valueService.GetDoubleValue(eachAngle.E);
                        retrunValue = actualRadius * 2;
                        break;
                    case "Detail e":
                        actualRadius += valueService.GetDoubleValue(eachAngle.E) +
                                        valueService.GetDoubleValue(eachAngle.t);
                        retrunValue = actualRadius * 2;
                        break;
                    case "Detail i":
                        double tempDetaili = GetRoofODByCompressionRingDetailI();
                        double A = valueService.GetDoubleValue(assemblyData.RoofCRTInput[0].DetailIOutsideProjection);
                        double B = valueService.GetDoubleValue(assemblyData.RoofCRTInput[0].DetailIWidth);
                        double C = valueService.GetDoubleValue(assemblyData.RoofCRTInput[0].DetailIOverlap);
                        double insideX = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, B - C);
                        retrunValue = tempDetaili + (insideX) * 2;
                        break;
                    case "Detail k":
                        double maxCourseThk = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness);
                        double maxCourseBeforeThk = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse - 1].Thickness);
                        double t1Width = 0;
                        if (maxCourseThk > maxCourseBeforeThk)
                            t1Width = (maxCourseThk - maxCourseBeforeThk) / 2;
                        actualRadius += -t1Width;
                        retrunValue = actualRadius * 2;
                        break;
                }
            }

            return retrunValue;
        }
        public double GetRoofODByCompressionRingDetailI()
        {
            string tankRoofType = assemblyData.GeneralDesignData[0].RoofType;

            double retrunValue = 0;
            string topAngleType = assemblyData.RoofCompressionRing[0].CompressionRingType;
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankInRadius = tankID / 2;

            double actualRadius = tankInRadius;

            int maxCourse = assemblyData.ShellOutput.Count - 1;
            if (tankRoofType.ToLower() == "crt")
            {
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

                        actualRadius += -valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness)
                                        - thickneesX
                                        + insideX;

                        retrunValue = valueService.GetHypotenuseByWidth(roofSlopeDegree, actualRadius) * 2;
                        break;

                    default:
                        retrunValue = GetRoofOD();
                        break;
                }
            }
            else if (tankRoofType.ToLower() == "drt")
            {
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

                        actualRadius += -valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness)
                                        - thickneesX
                                        + insideX;

                        retrunValue = actualRadius * 2;
                        break;

                    default:
                        retrunValue = GetRoofOD();
                        break;
                }
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
            return valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateID) + GetDetailAnnularOverlap() * 2;
        }

        public double GetBottomRoofOD(double selRoofOD = 0, double selBottomOD = 0)
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




        public double GetBottomFocuntionOD(string selAnnular)
        {
            double outsideProjection = 0;
            double weldingLeg = 9;
            if (selAnnular == "Yes")
                outsideProjection = 60 + weldingLeg;
            else
                outsideProjection = 30 + weldingLeg;

            return outsideProjection;
        }


        public double GetDetailAnnularOverlap()
        {
            return 70;
        }

        #region Angle Type : b, d, e, i, k
        public TopAngle_Type GetCurrentTopAngleType()
        {
            TopAngle_Type retunValue = TopAngle_Type.NotSet;
            string topAngleType = assemblyData.RoofCompressionRing[0].CompressionRingType;

            switch (topAngleType)
            {
                case "Detail b":
                    retunValue = TopAngle_Type.b;
                    break;
                case "Detail d":
                    retunValue = TopAngle_Type.d;
                    break;
                case "Detail e":
                    retunValue = TopAngle_Type.e;
                    break;
                case "Detail i":
                    retunValue = TopAngle_Type.i;
                    break;
                case "Detail k":
                    retunValue = TopAngle_Type.k;
                    break;
            }


            return retunValue;
        }
        public double TopAnglebTypeLastCourseAjdHeight()
        {
            return 10;
        }
        #endregion

        #region Botton
        public bool isAnnular()
        {
            bool returnValue = false;
            string annularYes = assemblyData.BottomInput[0].AnnularPlate;

            if (annularYes.Contains("Yes"))
            {
                returnValue = true; ;
            }

            return returnValue;
        }
        #endregion

        #region Shell Plate Course
        public List<double> GetShellCourseWidthForDrawing()
        {
            List<double> newList = new List<double>();

            foreach(ShellOutputModel eachCourse  in assemblyData.ShellOutput)
            {
                newList.Add(valueService.GetDoubleValue(eachCourse.PlateWidth));
            }

            TopAngle_Type topAngle = GetCurrentTopAngleType();
            if (topAngle == TopAngle_Type.b)
            {
                int maxCount = newList.Count-1;
                newList[maxCount] -= TopAnglebTypeLastCourseAjdHeight();
            }
            if (topAngle == TopAngle_Type.k)
            {
                if (newList.Count > 0)
                    newList.RemoveAt(newList.Count - 1);
            }

            return newList;
        }
        public List<double> GetShellCourseThickneeForDrawing()
        {
            List<double> newList = new List<double>();

            foreach (ShellOutputModel eachCourse in assemblyData.ShellOutput)
            {
                newList.Add(valueService.GetDoubleValue(eachCourse.Thickness));
            }
            TopAngle_Type topAngle = GetCurrentTopAngleType();
            if (topAngle == TopAngle_Type.k)
            {
                if (newList.Count > 0)
                    newList.RemoveAt(newList.Count - 1);
            }
            return newList;
        }
        #endregion
    }
}
