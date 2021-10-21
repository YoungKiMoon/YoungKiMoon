using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using devDept.Eyeshot;
using devDept.Graphics;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Serialization;

using AssemblyLib.AssemblyModels;
using DrawWork.DrawModels;
using DrawWork.ValueServices;
using DrawWork.Commons;
using DrawWork.DrawWorkingPointSevices;

namespace DrawWork.DrawServices
{
    public class DrawWorkingPointService
    {
        private AssemblyModel assemblyData;

        private ValueService valueService;
        private DrawEditingService editingService;


        public  DRTWorkingPointModel DRTWorkingData
        {
            get 
            {
                if (_DRTWorkingData == null)
                    _DRTWorkingData = GetDRTRoofData();
                return _DRTWorkingData; 
            }
            set { _DRTWorkingData = value; }
        }

        private DRTWorkingPointModel _DRTWorkingData;



        public DrawWorkingPointService(AssemblyModel selAssembly)
        {
            assemblyData = selAssembly;
            valueService = new ValueService();
            editingService = new DrawEditingService();

            DRTWorkingData = null;
        }


        // Text 용
        public CDPoint WorkingPoint(string selPoint, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            return WorkingPointOrigin(CommonMethod.WorkingPointToEnum(selPoint), 0, ref refPoint, ref curPoint);
        }
        public CDPoint WorkingPoint(string selPoint, string selPointValue, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            return WorkingPointOrigin(CommonMethod.WorkingPointToEnum(selPoint), valueService.GetDoubleValue(selPointValue), ref refPoint, ref curPoint);
        }


        // CDPoint 용
        public CDPoint WorkingPoint(WORKINGPOINT_TYPE selPoint, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            return WorkingPointOrigin(selPoint, 0, ref refPoint, ref curPoint);
        }
        public CDPoint WorkingPoint(WORKINGPOINT_TYPE selPoint, string selPointValue, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            return WorkingPointOrigin(selPoint, valueService.GetDoubleValue(selPointValue),ref refPoint,ref curPoint);
        }
        public CDPoint WorkingPoint(WORKINGPOINT_TYPE selPoint, double selPointValue, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            return WorkingPointOrigin(selPoint, selPointValue, ref refPoint, ref curPoint);
        }

        private CDPoint WorkingPointOrigin(WORKINGPOINT_TYPE selPoint, double selPointValue, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            int refFirstIndex = 0;

            string selSizeTankHeight = assemblyData.GeneralDesignData[refFirstIndex].SizeTankHeight;
            string selSizeNominalId = assemblyData.GeneralDesignData[refFirstIndex].SizeNominalID;

            string roofSlope = assemblyData.RoofCompressionRing[refFirstIndex].RoofSlope;
            string roofThickness = assemblyData.RoofCompressionRing[refFirstIndex].RoofPlateThickness;
            string roofRadiusRatio = assemblyData.RoofCompressionRing[refFirstIndex].DomeRadiusRatio;

            string bottomSlope = assemblyData.BottomInput[0].BottomPlateSlope;
            string bottomThickness = assemblyData.BottomInput[0].BottomPlateThickness;


            CDPoint WPPoint = new CDPoint();
            switch (selPoint)
            {


                // Point : Center
                case WORKINGPOINT_TYPE.PointReferenceBottom:
                    double lastPlateHeight = 0;
                    if (assemblyData.BottomInput[refFirstIndex].AnnularPlate.ToLower() == "yes")
                    {
                        lastPlateHeight = valueService.GetDoubleValue(assemblyData.BottomInput[refFirstIndex].AnnularPlateThickness);
                    }
                    else
                    {
                        lastPlateHeight = valueService.GetDoubleValue(assemblyData.BottomInput[refFirstIndex].BottomPlateThickness);
                    }

                    WPPoint = GetSumCDPoint(refPoint, 0, -lastPlateHeight);
                    break;



                case WORKINGPOINT_TYPE.PointCenterTopUp:                // 2021-04-22 완료 : 2021-05-28 완료 : 2021-06-10 완료
                    switch (SingletonData.TankType)
                    {
                        case TANK_TYPE.CRT:
                        case TANK_TYPE.IFRT:
                            CDPoint topcenterpoint = WorkingPoint(WORKINGPOINT_TYPE.PointCenterTopDown, ref refPoint, ref curPoint);
                            double verticalHeight2 = valueService.GetHypotenuseByWidth(roofSlope, roofThickness);
                            WPPoint = GetSumCDPoint(topcenterpoint, 0, verticalHeight2);
                            break;
                        case TANK_TYPE.DRT:
                            WPPoint = GetSumCDPoint(DRTWorkingData.PointCenterTopUp, 0, 0);
                            break;
                        case TANK_TYPE.EFRTSingle:
                        case TANK_TYPE.EFRTDouble:
                            WPPoint = GetSumCDPoint(WorkingPoint(WORKINGPOINT_TYPE.PointCenterTop, ref refPoint, ref curPoint), 0, valueService.GetDoubleValue(roofThickness));
                            break;
                    }
                    break;

                case WORKINGPOINT_TYPE.PointCenterTopDown:              // 2021-04-22 완료 : 2021-05-28 완료 : 2021-06-10 완료

                    switch (SingletonData.TankType)
                    {
                        case TANK_TYPE.CRT:
                        case TANK_TYPE.IFRT:
                            // top angle roof point
                            CDPoint topAngleRoofPoint = WorkingPoint(WORKINGPOINT_TYPE.PointLeftRoofDown, ref refPoint, ref curPoint);

                            double tempWidth3 = GetDistanceX(topAngleRoofPoint.X, refPoint.X + valueService.GetDoubleValue(selSizeNominalId) / 2);
                            double tempHeight3 = valueService.GetOppositeByWidth(roofSlope, tempWidth3);

                            WPPoint = GetSumCDPoint(topAngleRoofPoint, tempWidth3, tempHeight3);

                            break;
                        case TANK_TYPE.DRT:
                            WPPoint = GetSumCDPoint(DRTWorkingData.PointCenterTopDown, 0, 0);
                            break;
                        case TANK_TYPE.EFRTSingle:
                        case TANK_TYPE.EFRTDouble:
                            WPPoint = GetSumCDPoint(WorkingPoint(WORKINGPOINT_TYPE.PointCenterTop, ref refPoint, ref curPoint), 0, 0);
                            break;
                    }
                    break;



                case WORKINGPOINT_TYPE.PointCenterBottomUp:             // 2021-04-22 완료 : 문제 있음
                    CDPoint bottomcenterpoint = WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomDown, ref refPoint, ref curPoint);
                    double verticalHeight = valueService.GetHypotenuseByWidth(bottomSlope, bottomThickness);
                    WPPoint = GetSumCDPoint(bottomcenterpoint, 0, verticalHeight);
                    break;

                case WORKINGPOINT_TYPE.PointCenterBottomDown:           // 2021-04-22 완료 : 문제 있음
                    CDPoint bottomleftpoint = WorkingPoint(WORKINGPOINT_TYPE.PointLeftBottomDown, ref refPoint, ref curPoint);

                    double tempWidth2 = GetDistanceX(bottomleftpoint.X, refPoint.X + valueService.GetDoubleValue(selSizeNominalId) / 2);
                    double tempHeight2 = valueService.GetOppositeByWidth(bottomSlope, tempWidth2);

                    WPPoint = GetSumCDPoint(bottomleftpoint, tempWidth2, tempHeight2);
                    break;


                case WORKINGPOINT_TYPE.PointCenterTop:                  // 2021-04-22 완료
                    WPPoint = GetSumCDPoint(refPoint,
                                            valueService.GetDoubleValue(selSizeNominalId) / 2,
                                            valueService.GetDoubleValue(selSizeTankHeight));
                    break;

                case WORKINGPOINT_TYPE.PointCenterBottom:               // 2021-04-22 완료
                    WPPoint = GetSumCDPoint(refPoint, 
                                            valueService.GetDoubleValue(selSizeNominalId) / 2,
                                            0);
                    break;


                // Point : Bottom
                case WORKINGPOINT_TYPE.PointLeftBottomUp:               // 2021-04-22 완료
                    CDPoint bottomleftpoint2 = WorkingPoint(WORKINGPOINT_TYPE.PointLeftBottomDown, ref refPoint, ref curPoint);
                    WPPoint = GetSumCDPoint(bottomleftpoint2, 
                                            valueService.GetOppositeByHypotenuse(bottomSlope, bottomThickness), 
                                            valueService.GetAdjacentByHypotenuse(bottomSlope, bottomThickness));
                    break;

                case WORKINGPOINT_TYPE.PointLeftBottomDown:             // Entry Point : 2021-04-22 완료
                    WPPoint = PointCenterBottomDown(ref refPoint, ref curPoint);
                    break;


                // Point : Roof
                case WORKINGPOINT_TYPE.PointLeftRoofUp:                 // 2021-04-22 완료 : 2021-05-28 완료
                    switch (SingletonData.TankType)
                    {
                        case TANK_TYPE.CRT:
                        case TANK_TYPE.IFRT:
                            CDPoint topleftpoint = WorkingPoint(WORKINGPOINT_TYPE.PointLeftRoofDown, ref refPoint, ref curPoint);
                            WPPoint = GetSumCDPoint(topleftpoint,
                                                    -valueService.GetOppositeByHypotenuse(roofSlope, roofThickness),
                                                    valueService.GetAdjacentByHypotenuse(roofSlope, roofThickness));
                            break;
                        case TANK_TYPE.DRT:
                            WPPoint = GetSumCDPoint(DRTWorkingData.PointLeftRoofUp,0,0);
                            break;
                        case TANK_TYPE.EFRTSingle:
                        case TANK_TYPE.EFRTDouble:
                            WPPoint = GetSumCDPoint(WorkingPoint(WORKINGPOINT_TYPE.PointLeftShellTop, ref refPoint, ref curPoint), 0,valueService.GetDoubleValue( roofThickness));
                            break;
                    }
                    break;

                case WORKINGPOINT_TYPE.PointLeftRoofDown:               // Entiry Point : 2021-04-22 완료 : CRT, DRT, IFRT, EFRT 추가 : 2021-06-10
                    WPPoint = PointTopAngleRoof(ref refPoint, ref curPoint);
                    break;



                // Point : Shell
                case WORKINGPOINT_TYPE.PointLeftShellTop:               // 2021-04-22 완료
                    WPPoint = GetSumCDPoint(refPoint, 0, valueService.GetDoubleValue(selSizeTankHeight));
                    break;

                case WORKINGPOINT_TYPE.PointLeftShellTopAdj:            // 2021-04-22 완료
                    WPPoint = PointTopAngleShell(ref refPoint, ref curPoint);
                    break;

                case WORKINGPOINT_TYPE.PointReference:                  // Point : Reference : 2021-04-22 완료
                case WORKINGPOINT_TYPE.PointLeftShellBottom:            // 2021-04-22 완료
                    WPPoint = GetSumCDPoint(refPoint, 0, 0);
                    break;

                case WORKINGPOINT_TYPE.PointRightShellTop:              // 2021-04-22 완료
                    CDPoint tempLeftShellTop = WorkingPoint(WORKINGPOINT_TYPE.PointLeftShellTop, ref refPoint, ref curPoint);
                    WPPoint = GetSumCDPoint(tempLeftShellTop, valueService.GetDoubleValue(selSizeNominalId), 0);
                    break;

                case WORKINGPOINT_TYPE.PointRightShellTopAdj:           // 2021-04-22 완료
                    CDPoint tempLeftShellTopAdj = WorkingPoint(WORKINGPOINT_TYPE.PointLeftShellTopAdj, ref refPoint, ref curPoint);
                    WPPoint = GetSumCDPoint(tempLeftShellTopAdj, valueService.GetDoubleValue(selSizeNominalId), 0);
                    break;
                case WORKINGPOINT_TYPE.PointRightShellBottom:           // 2021-04-22 완료
                    CDPoint tempLeftShellBottom = WorkingPoint(WORKINGPOINT_TYPE.PointLeftShellBottom, ref refPoint, ref curPoint);
                    WPPoint = GetSumCDPoint(tempLeftShellBottom, valueService.GetDoubleValue(selSizeNominalId), 0);
                    break;


                // Adj : Roof
                case WORKINGPOINT_TYPE.AdjCenterRoofUp:                 // 2021-04-22 완료


                    switch (SingletonData.TankType)
                    {
                        case TANK_TYPE.CRT:
                        case TANK_TYPE.IFRT:
                            CDPoint tempCenterRoofPoint2 = WorkingPoint(WORKINGPOINT_TYPE.PointCenterTopUp, ref refPoint, ref curPoint);
                            double tempRightRoofHeight2 = valueService.GetOppositeByWidth(roofSlope, selPointValue);

                            WPPoint = GetSumCDPoint(tempCenterRoofPoint2, -selPointValue, -tempRightRoofHeight2);
                            break;
                        case TANK_TYPE.EFRTSingle:
                        case TANK_TYPE.EFRTDouble:
                            CDPoint tempCenterRoofPoint3 = WorkingPoint(WORKINGPOINT_TYPE.PointCenterTopUp, ref refPoint, ref curPoint);
                            WPPoint = GetSumCDPoint(tempCenterRoofPoint3, -selPointValue, 0);
                            break;

                        case TANK_TYPE.DRT:
                            WPPoint = GetSumCDPoint(GetDRTRoofPoint(DRTWorkingData.circleRoofUpper, selPointValue), 0, 0);
                            break;
                    }
                    break;

                case WORKINGPOINT_TYPE.AdjCenterRoofDown:               // 2021-04-22 완료

                    switch (SingletonData.TankType)
                    {
                        case TANK_TYPE.CRT:
                        case TANK_TYPE.IFRT:
                            CDPoint tempCenterRoofPoint = WorkingPoint(WORKINGPOINT_TYPE.PointCenterTopDown, ref refPoint, ref curPoint);
                            double tempLeftRoofHeight = valueService.GetOppositeByWidth(roofSlope, selPointValue);

                            WPPoint = GetSumCDPoint(tempCenterRoofPoint, -selPointValue, -tempLeftRoofHeight);
                            break;
                        case TANK_TYPE.EFRTSingle:
                        case TANK_TYPE.EFRTDouble:
                            CDPoint tempCenterRoofPoint1 = WorkingPoint(WORKINGPOINT_TYPE.PointCenterTopDown, ref refPoint, ref curPoint);
                            WPPoint = GetSumCDPoint(tempCenterRoofPoint1, -selPointValue, 0);
                            break;

                        case TANK_TYPE.DRT:
                            WPPoint = GetSumCDPoint(GetDRTRoofPoint(DRTWorkingData.circleRoofLower, selPointValue), 0, 0);
                            break;
                    }
                    break;


                case WORKINGPOINT_TYPE.AdjLeftRoofUp:                   // 2021-04-22 완료
                    double tempTankWidthHalf2 = valueService.GetDoubleValue(selSizeNominalId) / 2 - selPointValue;
                    switch (SingletonData.TankType)
                    {
                        case TANK_TYPE.CRT:
                        case TANK_TYPE.IFRT:
                            CDPoint tempCenterRoofPoint4 = WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofUp, tempTankWidthHalf2.ToString(), ref refPoint, ref curPoint);
                            WPPoint = GetSumCDPoint(tempCenterRoofPoint4, 0, 0);
                            break;
                        case TANK_TYPE.EFRTSingle:
                        case TANK_TYPE.EFRTDouble:
                            CDPoint tempCenterRoofPoint5 = WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofUp, tempTankWidthHalf2.ToString(), ref refPoint, ref curPoint);
                            WPPoint = GetSumCDPoint(tempCenterRoofPoint5, 0, 0);
                            break;
                        case TANK_TYPE.DRT:
                            WPPoint = GetSumCDPoint(GetDRTRoofPoint(DRTWorkingData.circleRoofUpper, tempTankWidthHalf2), 0, 0);
                            break;
                    }
                    break;


                case WORKINGPOINT_TYPE.AdjLeftRoofDown:                 // 2021-04-22 완료

                    double tempTankWidthHalf = valueService.GetDoubleValue(selSizeNominalId) / 2 - selPointValue;
                    switch (SingletonData.TankType)
                    {
                        case TANK_TYPE.CRT:
                        case TANK_TYPE.IFRT:
                            CDPoint tempCenterRoofPoint3 = WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, tempTankWidthHalf.ToString(), ref refPoint, ref curPoint);
                            WPPoint = GetSumCDPoint(tempCenterRoofPoint3, 0, 0);
                            break;
                        case TANK_TYPE.EFRTSingle:
                        case TANK_TYPE.EFRTDouble:
                            CDPoint tempCenterRoofPoint4 = WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, tempTankWidthHalf.ToString(), ref refPoint, ref curPoint);
                            WPPoint = GetSumCDPoint(tempCenterRoofPoint4, 0, 0);
                            break;
                        case TANK_TYPE.DRT:
                            WPPoint = GetSumCDPoint(GetDRTRoofPoint(DRTWorkingData.circleRoofLower, tempTankWidthHalf), 0, 0);
                            break;
                    }
                    break;



                // Adj : Bottom
                case WORKINGPOINT_TYPE.AdjCenterBottomUp:               // 2021-04-22 완료
                    CDPoint tempCenterBottomPoint2 = WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomUp, ref refPoint, ref curPoint);
                    double tempCenterBottomHeight2 = valueService.GetOppositeByWidth(bottomSlope, selPointValue);

                    WPPoint = GetSumCDPoint(tempCenterBottomPoint2, -selPointValue, -tempCenterBottomHeight2);
                    break;

                case WORKINGPOINT_TYPE.AdjCenterBottomDown:             // 2021-04-22 완료
                    CDPoint tempCenterBottomPoint = WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomDown, ref refPoint, ref curPoint);
                    double tempCenterBottomHeight = valueService.GetOppositeByWidth(bottomSlope, selPointValue);

                    WPPoint = GetSumCDPoint(tempCenterBottomPoint, -selPointValue, -tempCenterBottomHeight);
                    break;


                // Adj : Shell
                case WORKINGPOINT_TYPE.AdjLeftShell:                    // 2021-04-22 완료
                    WPPoint = AdjLeftShell(selPointValue, ref refPoint, ref curPoint);
                    break;

                case WORKINGPOINT_TYPE.AdjRightShell:                   // 2021-04-22 완료
                    WPPoint = AdjRightShell(selPointValue, ref refPoint, ref curPoint);
                    break;



                default:
                    WPPoint = null;
                    break;
            }

            return WPPoint;
        }

        // Point3D 전용
        public Point3D WorkingPointNew(WORKINGPOINT_TYPE selPoint, Point3D refPoint)
        {
            CDPoint refPointTemp = new CDPoint(refPoint.X, refPoint.Y, 0);
            CDPoint curPointTemp = new CDPoint(refPoint.X, refPoint.Y, 0);
            SingletonData.RefPoint = refPointTemp;
            CDPoint returnPoint = WorkingPointOrigin(selPoint, 0, ref refPointTemp, ref curPointTemp);
            return new Point3D(returnPoint.X, returnPoint.Y);
        }

        public Point3D WorkingPointNew(WORKINGPOINT_TYPE selPoint, double selPointValue, Point3D refPoint)
        {
            CDPoint refPointTemp = new CDPoint(refPoint.X, refPoint.Y,0);
            CDPoint curPointTemp = new CDPoint(refPoint.X, refPoint.Y,0);
            CDPoint returnPoint = WorkingPointOrigin(selPoint, selPointValue, ref refPointTemp, ref curPointTemp);
            return new Point3D(returnPoint.X, returnPoint.Y);
        }


        #region Adj : Shell
        private CDPoint AdjLeftShell( double selHeight, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // Shell Bottom Y = 0
            double currentThickness = GetShellThicknessAccordingToHeight(selHeight);
            CDPoint newPoint = GetSumCDPoint(refPoint, -currentThickness, selHeight);
            return newPoint;
        }
        private CDPoint AdjRightShell(double selHeight, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            int refFirstIndex = 0;

            //string selSizeTankHeight = assemblyData.GeneralDesignData[refFirstIndex].SizeTankHeight;
            string selSizeNominalId = assemblyData.GeneralDesignData[refFirstIndex].SizeNominalID;
            // Shell Bottom Y = 0
            double currentThickness = GetShellThicknessAccordingToHeight(selHeight);
            double tankWidth = valueService.GetDoubleValue(selSizeNominalId);

            CDPoint newPoint = GetSumCDPoint(refPoint, tankWidth + currentThickness, selHeight);
            return newPoint;
        }

        public double GetShellThicknessAccordingToHeight(string selHeight)
        {
            return GetShellThicknessAccordingToHeight(valueService.GetDoubleValue(selHeight));
        }
        public double GetShellThicknessAccordingToHeight(double selHeight)
        {
            double currentThickness = 0;

            int maxCourse = assemblyData.ShellOutput.Count;
            double sumHeight = 0;
            for (int i = 0; i < maxCourse; i++)
            {
                double plateWidth = valueService.GetDoubleValue(assemblyData.ShellOutput[i].PlateWidth);
                sumHeight += plateWidth;
                if (selHeight < sumHeight)
                {
                    double minThickness = valueService.GetDoubleValue(assemblyData.ShellOutput[i].Thickness);
                    currentThickness = minThickness;
                    break;
                }
            }
            return currentThickness;
        }
        #endregion

        #region Point : Roof
        private CDPoint PointTopAngleRoof(ref CDPoint refPoint, ref CDPoint curPoint)
        {
            CDPoint newPoint = new CDPoint();
            switch (SingletonData.TankType)
            {
                case TANK_TYPE.CRT:
                case TANK_TYPE.IFRT:
                    newPoint = PointTopAngleRoof_CRT(ref refPoint, ref curPoint); 
                    break;
                case TANK_TYPE.DRT:
                    newPoint = PointTopAngleRoof_DRT(ref refPoint, ref curPoint); // 같음
                    break;
                case TANK_TYPE.EFRTSingle:
                case TANK_TYPE.EFRTDouble:
                    newPoint = PointTopAngleRoof_EFRT(ref refPoint, ref curPoint); // 같음
                    break;

                // 하나로 통합 할 필요성 있음
            }

            return newPoint;
        }
        private CDPoint PointTopAngleRoof_CRT(ref CDPoint refPoint, ref CDPoint curPoint)
        {
            int firstIndex = 0;

            string selSizeTankHeight = assemblyData.GeneralDesignData[firstIndex].SizeTankHeight;
            //string selSizeNominalId = assemblyData.GeneralDesignData[refFirstIndex].SizeNominalId;

            CDPoint newPoint = new CDPoint();
            // Angle Model
            AngleSizeModel eachAngle = new AngleSizeModel();
            if(assemblyData.RoofAngleOutput.Count>0)
                eachAngle=assemblyData.RoofAngleOutput[0];
            // Max course count
            int maxCourse = assemblyData.ShellOutput.Count - 1;

            switch (assemblyData.RoofCompressionRing[0].CompressionRingType)
            {
                case "Detail b":
                    // Point
                    newPoint = GetSumCDPoint(refPoint,
                                             - valueService.GetDoubleValue(eachAngle.E)
                                             - valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness),

                                             + valueService.GetDoubleValue(selSizeTankHeight));

                    break;

                case "Detail d":
                    // Point
                    newPoint = GetSumCDPoint(refPoint,
                                             - valueService.GetDoubleValue(eachAngle.E),

                                             + valueService.GetDoubleValue(selSizeTankHeight));


                    break;

                case "Detail e":
                    // Point
                    newPoint = GetSumCDPoint(refPoint,
                                             + valueService.GetDoubleValue(eachAngle.E)
                                             - valueService.GetDoubleValue(eachAngle.t),

                                             + valueService.GetDoubleValue(selSizeTankHeight));


                    break;

                case "Detail i":
                    // Roof Slope
                    string roofSlopeString = assemblyData.RoofCompressionRing[firstIndex].RoofSlope;
                    double roofSlopeDegree = valueService.GetDegreeOfSlope(roofSlopeString);

                    double A = valueService.GetDoubleValue(assemblyData.RoofCRTInput[firstIndex].DetailIOutsideProjection);
                    double B = valueService.GetDoubleValue(assemblyData.RoofCRTInput[firstIndex].DetailIWidth);
                    double C = valueService.GetDoubleValue(assemblyData.RoofCRTInput[firstIndex].DetailIOverlap);
                    double t1 = valueService.GetDoubleValue(assemblyData.RoofCRTInput[firstIndex].DetailIThickness);

                    double insideX = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, B - A - C);
                    double insideY = valueService.GetOppositeByHypotenuse(roofSlopeDegree, B - A - C);
                    double thicknessY = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, t1);
                    double thickneesX = valueService.GetOppositeByHypotenuse(roofSlopeDegree, t1);

                    newPoint = GetSumCDPoint(refPoint,
                                             - valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness)
                                             - thickneesX
                                             + insideX,

                                             + valueService.GetDoubleValue(selSizeTankHeight)
                                             + thicknessY
                                             + insideY);
                    break;

                case "Detail k":
                    double maxCourseThk = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness);
                    double maxCourseBeforeThk = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse - 1].Thickness);
                    double t1Width = 0;
                    if(maxCourseThk> maxCourseBeforeThk)
                        t1Width = (maxCourseThk - maxCourseBeforeThk) / 2;
                    newPoint = GetSumCDPoint(refPoint,
                                             + t1Width,

                                             + valueService.GetDoubleValue(selSizeTankHeight)
);
                    break;

            }

            return newPoint;
        }
        private CDPoint PointTopAngleRoof_DRT(ref CDPoint refPoint, ref CDPoint curPoint)
        {
            int firstIndex = 0;

            string selSizeTankHeight = assemblyData.GeneralDesignData[firstIndex].SizeTankHeight;
            //string selSizeNominalId = assemblyData.GeneralDesignData[refFirstIndex].SizeNominalId;

            CDPoint newPoint = new CDPoint();
            // Angle Model
            AngleSizeModel eachAngle = new AngleSizeModel();
            if (assemblyData.RoofAngleOutput.Count > 0)
                eachAngle = assemblyData.RoofAngleOutput[0];
            // Max course count
            int maxCourse = assemblyData.ShellOutput.Count - 1;

            switch (assemblyData.RoofCompressionRing[0].CompressionRingType)
            {
                case "Detail b":
                    // Point
                    newPoint = GetSumCDPoint(refPoint,
                                             -valueService.GetDoubleValue(eachAngle.E)
                                             - valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness),

                                             +valueService.GetDoubleValue(selSizeTankHeight));

                    break;

                case "Detail d":
                    // Point
                    newPoint = GetSumCDPoint(refPoint,
                                             -valueService.GetDoubleValue(eachAngle.E),

                                             +valueService.GetDoubleValue(selSizeTankHeight));


                    break;

                case "Detail e":
                    // Point
                    newPoint = GetSumCDPoint(refPoint,
                                             +valueService.GetDoubleValue(eachAngle.E)
                                             - valueService.GetDoubleValue(eachAngle.t),

                                             +valueService.GetDoubleValue(selSizeTankHeight));


                    break;

                case "Detail i":
                    // Roof Slope
                    //string roofSlopeString = assemblyData.RoofCRTInput[firstIndex].RoofSlope;
                    //double roofSlopeDegree = valueService.GetDegreeOfSlope(roofSlopeString);

                    //double A = valueService.GetDoubleValue(assemblyData.RoofCRTInput[firstIndex].DetailIOutsideProjection);
                    //double B = valueService.GetDoubleValue(assemblyData.RoofCRTInput[firstIndex].DetailIWidth);
                    //double C = valueService.GetDoubleValue(assemblyData.RoofCRTInput[firstIndex].DetailIOverlap);
                    //double t1 = valueService.GetDoubleValue(assemblyData.RoofCRTInput[firstIndex].DetailIThickness);

                    //double insideX = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, B - A - C);
                    //double insideY = valueService.GetOppositeByHypotenuse(roofSlopeDegree, B - A - C);
                    //double thicknessY = valueService.GetAdjacentByHypotenuse(roofSlopeDegree, t1);
                    //double thickneesX = valueService.GetOppositeByHypotenuse(roofSlopeDegree, t1);

                    //double shellMaxCourseThickness = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness);
                    //double shellHeight = +valueService.GetDoubleValue(selSizeTankHeight);
                    //Line newSlopeLine=new Line(GetSumPoint(refPoint, -shellMaxCourseThickness, shellHeight), GetSumPoint(refPoint, -shellMaxCourseThickness, shellHeight) +(DRTWorkingData.CompressionRingDirection*(B-A)));

                    // 계산 못함 그대로 가져와야 함.
                    newPoint = GetSumCDPoint(DRTWorkingData.PointLeftRoofDown, 0, 0);

                    break;

                case "Detail k":
                    double maxCourseThk = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness);
                    double maxCourseBeforeThk = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse - 1].Thickness);
                    double t1Width = 0;
                    if (maxCourseThk > maxCourseBeforeThk)
                        t1Width = (maxCourseThk - maxCourseBeforeThk) / 2;
                    newPoint = GetSumCDPoint(refPoint,
                                             +t1Width,

                                             +valueService.GetDoubleValue(selSizeTankHeight)
);
                    break;

            }

            return newPoint;
        }

        private CDPoint PointTopAngleRoof_EFRT(ref CDPoint refPoint, ref CDPoint curPoint)
        {
            int firstIndex = 0;

            string selSizeTankHeight = assemblyData.GeneralDesignData[firstIndex].SizeTankHeight;
            //string selSizeNominalId = assemblyData.GeneralDesignData[refFirstIndex].SizeNominalId;

            CDPoint newPoint = new CDPoint();
            // Angle Model
            AngleSizeModel eachAngle = new AngleSizeModel();
            if (assemblyData.RoofAngleOutput.Count > 0)
                eachAngle = assemblyData.RoofAngleOutput[0];
            // Max course count
            int maxCourse = assemblyData.ShellOutput.Count - 1;

            switch (assemblyData.RoofCompressionRing[0].CompressionRingType)
            {
                case "Detail b":
                    // Point
                    newPoint = GetSumCDPoint(refPoint,
                                             -valueService.GetDoubleValue(eachAngle.E)
                                             - valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness),

                                             +valueService.GetDoubleValue(selSizeTankHeight));

                    break;

                case "Detail d":
                    // Point
                    newPoint = GetSumCDPoint(refPoint,
                                             -valueService.GetDoubleValue(eachAngle.E),

                                             +valueService.GetDoubleValue(selSizeTankHeight));


                    break;

            
            }

            return newPoint;
        }

        private CDPoint PointTopAngleShell(ref CDPoint refPoint, ref CDPoint curPoint)
        {
            CDPoint newPoint = new CDPoint();
            switch (SingletonData.TankType)
            {
                case TANK_TYPE.CRT:
                case TANK_TYPE.DRT:
                case TANK_TYPE.IFRT:
                case TANK_TYPE.EFRTSingle:
                case TANK_TYPE.EFRTDouble:
                    newPoint = PointTopAngleShell_CRT(ref refPoint, ref curPoint);
                    break;
            }

            return newPoint;
        }
        private CDPoint PointTopAngleShell_CRT(ref CDPoint refPoint, ref CDPoint curPoint)
        {

            int firstIndex = 0;

            string selSizeTankHeight = assemblyData.GeneralDesignData[firstIndex].SizeTankHeight;
            //string selSizeNominalId = assemblyData.GeneralDesignData[refFirstIndex].SizeNominalId;

            CDPoint newPoint = new CDPoint();
            // Angle Model
            AngleSizeModel eachAngle = new AngleSizeModel();
            if (assemblyData.RoofAngleOutput.Count>0)
                eachAngle = assemblyData.RoofAngleOutput[0];
            // Max course count
            int maxCourse = assemblyData.ShellOutput.Count - 1;

            switch (assemblyData.RoofCompressionRing[0].CompressionRingType)
            {
                case "Detail b":

                    // Point
                    newPoint = GetSumCDPoint(refPoint,
                                                   - valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness),

                                                   + valueService.GetDoubleValue(selSizeTankHeight)
                                                   - 10);// 10으로 고정 값

                    break;

                case "Detail d":
                case "Detail e":

                    // Point
                    newPoint = GetSumCDPoint(refPoint,
                                                   0,

                                                   + valueService.GetDoubleValue(selSizeTankHeight)
                                                   - valueService.GetDoubleValue(eachAngle.B));

                    break;
                case "Detail i":
                    newPoint = GetSumCDPoint(refPoint,
                                                   0,
                                                   valueService.GetDoubleValue(selSizeTankHeight));

                    break;
                case "Detail k":
                    double maxCourseThk = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness);
                    newPoint = GetSumCDPoint(refPoint,
                                                   0,
                                                   + valueService.GetDoubleValue(selSizeTankHeight));
                    //newPoint = GetSumCDPoint(refPoint,
                    //                               0,
                    //                               +valueService.GetDoubleValue(selSizeTankHeight)
                    //                               - valueService.GetDoubleValue(assemblyData.RoofCRTInput[firstIndex].DetailKShellWidth));
                    break;
            }

            return newPoint;
        }

        #endregion

        #region Point : Bottom
        private CDPoint PointCenterBottomDown(ref CDPoint refPoint, ref CDPoint curPoint)
        {
            CDPoint newPoint = new CDPoint();

            int firstIndex = 0;
            // Shell
            double bottomThk = valueService.GetDoubleValue(assemblyData.ShellOutput[0].Thickness);

            // Roof Slope
            string bottomSlopeString = assemblyData.BottomInput[firstIndex].BottomPlateSlope;
            double bottomSlopeDegree = valueService.GetDegreeOfSlope(bottomSlopeString);

            BottomInputModel bottomBase = assemblyData.BottomInput[firstIndex];
            double bottomThickness = valueService.GetDoubleValue(bottomBase.BottomPlateThickness);
            double annularThickness = valueService.GetDoubleValue(bottomBase.AnnularPlateThickness);
            double annularThickWidth = valueService.GetDoubleValue(bottomBase.AnnularPlateWidth);

            double overLap = 70;

            double outsideProjection = 0;
            double weldingLeg = 9;


            if (bottomBase.AnnularPlate == "Yes")
            {
                //Annulal
                outsideProjection = 60 + weldingLeg;
                newPoint = GetSumCDPoint(refPoint,
                                        + annularThickWidth
                                        - bottomThk
                                        - outsideProjection
                                        - overLap,
                                        0);

            }
            else
            {
                outsideProjection = 30 + weldingLeg;
                newPoint = GetSumCDPoint(refPoint,
                                        - bottomThk
                                        - outsideProjection,

                                        - valueService.GetOppositeByWidth(bottomSlopeDegree, bottomThk+ outsideProjection));
                newPoint = GetSumCDPoint(newPoint,
                                        +valueService.GetOppositeByHypotenuse(bottomSlopeDegree, bottomThickness),
                                        -valueService.GetAdjacentByHypotenuse(bottomSlopeDegree, bottomThickness));
            }




            return newPoint;

        }
        #endregion




        #region DRT WorkingPoint
               
        private DRTWorkingPointModel GetDRTRoofData()
        {
            // 매우 중요
            CDPoint refPoint=SingletonData.RefPoint;
            CDPoint curPoint = new CDPoint();


            DRTWorkingPointModel returnValue = new DRTWorkingPointModel();

            // Input Data
            double sizeID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double roofThickness = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].RoofPlateThickness);

            int maxCourse = assemblyData.ShellOutput.Count;
            double shellMaxCourseThickness = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse - 1].Thickness);

            double domeRadiusRatio = valueService.GetDoubleValue(assemblyData.RoofDRTInput[0].DomeRadiusRatio);
            double domeRadius = domeRadiusRatio * sizeID;

            

            CDPoint leftWoking = WorkingPoint(WORKINGPOINT_TYPE.PointLeftShellTop,ref refPoint,ref curPoint);
            Point3D refPointLeft = GetSumPoint(leftWoking, 0, 0);
            Point3D refPointRight = GetSumPoint(refPointLeft, sizeID, 0);

            // Circle : 2
            Circle cirLeft = new Circle(GetSumPoint(refPointLeft, 0, 0), domeRadius);
            Circle cirRight = new Circle(GetSumPoint(refPointRight, 0, 0), domeRadius);
            Point3D[] cirIntersectArray = cirLeft.IntersectWith(cirRight);

            // Center Circle
            Point3D currentCenterPoint = new Point3D();
            if (cirIntersectArray != null)
            {
                double minValue = 9999999999;
                foreach (Point3D eachPoint in cirIntersectArray)
                {
                    if (minValue > eachPoint.Y)
                    {
                        minValue = eachPoint.Y;
                        currentCenterPoint = GetSumPoint(eachPoint, 0, 0);
                    }
                }
            }
            Circle cirCenter = new Circle(GetSumPoint(currentCenterPoint, 0, 0), domeRadius);


            Point3D leftRoofDownPoint = new Point3D();
            Point3D leftRoofDownPointNew = new Point3D();
            if (assemblyData.RoofCompressionRing[0].CompressionRingType=="Detail i")
            {

                int firstIndex = 0;
                double comA = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[firstIndex].OutsideProjectionA);
                double comB = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[firstIndex].WidthB);
                double comC = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[firstIndex].OverlapOfRoofAndCompRingC);
                double comT1 = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[firstIndex].ThicknessT1);

                // Compression Ring : Rotate
                Line comPressRing = new Line(GetSumPoint(currentCenterPoint, 0, 0), GetSumPoint(refPointLeft, 0, 0));
                comPressRing.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
                // Direction
                Vector3D direction = new Vector3D(comPressRing.EndPoint, comPressRing.StartPoint);
                direction.Normalize();
                // Compression Ring : New : Shift : Thickness
                Line comPressRingNew = new Line(GetSumPoint(refPointLeft, -shellMaxCourseThickness, 0), GetSumPoint(refPointLeft, -shellMaxCourseThickness, 0));
                comPressRingNew.EndPoint = comPressRingNew.EndPoint + direction * (comB - comA);
                // compression Ring Upper : Offset
                Line comPressRingNewUpper = (Line)comPressRingNew.Offset(-comT1, Vector3D.AxisZ, 0.01, true);// 위로

                // compression Ring Upper Overlap Length
                Line comPressRingNewUpperOverlapLength = new Line(GetSumPoint(comPressRingNewUpper.StartPoint, 0, 0), GetSumPoint(comPressRingNewUpper.StartPoint, 0, 0));
                comPressRingNewUpperOverlapLength.EndPoint = comPressRingNewUpperOverlapLength.EndPoint + direction * (comB - comA - comC);
                leftRoofDownPointNew = GetSumPoint(comPressRingNewUpperOverlapLength.EndPoint, 0, 0);

                // compression Ring : Ref Point
                Point3D comPressRingRefPoint = GetSumPoint(comPressRingNewUpper.EndPoint, 0, 0);


                leftRoofDownPoint = GetSumPoint(comPressRingRefPoint, 0, 0);

                // compression Ring : circle
                Circle cirComPress = new Circle(GetSumPoint(comPressRingRefPoint, 0, 0), domeRadius);

                // compression Ring : Ref Point : Line
                Line centerVerticalLine = new Line(GetSumPoint(currentCenterPoint, 0, -domeRadius), GetSumPoint(currentCenterPoint, 0, domeRadius * 3));// Size ID 길이 만큼

                // Intersect : CenterCircle <-> Left Line
                double shiftVertical = 0;
                Point3D[] leftIntersect = centerVerticalLine.IntersectWith(cirComPress);
                if (leftIntersect != null)
                {
                    if (leftIntersect.Length > 0)
                    {
                        currentCenterPoint = leftIntersect[1];
                    }

                }

                returnValue.CompressionRingDirection = direction;
            }
            else
            {
                // Left line
                CDPoint leftAngel = PointTopAngleRoof(ref refPoint, ref curPoint);
                leftRoofDownPoint = GetSumPoint(leftAngel, 0, 0);

                Circle cirComPress = new Circle(GetSumPoint(leftRoofDownPoint, 0, 0), domeRadius);

                // compression Ring : Ref Point : Line
                Line centerVerticalLine = new Line(GetSumPoint(currentCenterPoint, 0, -domeRadius), GetSumPoint(currentCenterPoint, 0, domeRadius * 3));// Size ID 길이 만큼

                // Intersect : CenterCircle <-> CenterLine
                Point3D[] leftIntersect = centerVerticalLine.IntersectWith(cirComPress);
                if (leftIntersect != null)
                {
                    if (leftIntersect.Length > 0)
                    {
                        currentCenterPoint = leftIntersect[1];
                    }

                }
            }




            // 값 세팅
            returnValue.RoofCenterPoint = GetSumPoint(currentCenterPoint, 0, 0);

            returnValue.circleRoofLower = new Circle(currentCenterPoint, domeRadius);
            returnValue.circleRoofUpper = new Circle(currentCenterPoint, domeRadius + roofThickness);

            // 겹침 라인
            Line centerLine = new Line(currentCenterPoint, GetSumPoint(currentCenterPoint, 0, domeRadius * 2));
            Line leftLine = editingService.GetExtendLine(new Line(currentCenterPoint, leftRoofDownPoint), domeRadius * 2);
            Line leftLineNew = editingService.GetExtendLine(new Line(currentCenterPoint, leftRoofDownPointNew), domeRadius * 2);


            Point3D[] inPointLeftUp= leftLine.IntersectWith(returnValue.circleRoofUpper);
            Point3D[] inPointLeftDown = leftLine.IntersectWith(returnValue.circleRoofLower);

            Point3D[] inPointLeftUpNew = leftLineNew.IntersectWith(returnValue.circleRoofUpper);
            Point3D[] inPointLeftDownNew = leftLineNew.IntersectWith(returnValue.circleRoofLower);

            Point3D[] inPointCenterUp = centerLine.IntersectWith(returnValue.circleRoofUpper);
            Point3D[] inPointCenterDown = centerLine.IntersectWith(returnValue.circleRoofLower);

            returnValue.PointCenterTopUp = inPointCenterUp[0];
            returnValue.PointCenterTopDown = inPointCenterDown[0];

            if (assemblyData.RoofCompressionRing[0].CompressionRingType == "Detail i")
            {
                returnValue.PointLeftRoofUp = inPointLeftUpNew[0];
                returnValue.PointLeftRoofDown = inPointLeftDownNew[0];
            }
            else
            {
                returnValue.PointLeftRoofUp = inPointLeftUp[0];
                returnValue.PointLeftRoofDown = inPointLeftDown[0];
            }

            if (assemblyData.RoofCompressionRing[0].CompressionRingType == "Detail i")
            {
                // Degree
                Point3D triA = GetSumPoint(refPoint, 0, 0);
                Line triALine = new Line(GetSumPoint(triA, 0, 0), GetSumPoint(triA, 0, 0));
                triALine.EndPoint = triALine.EndPoint + returnValue.CompressionRingDirection * (100);
                Point3D triB = GetSumPoint(triALine.EndPoint, 0, 0);
                Point3D triC = new Point3D(triB.X, triA.Y);

                double tempDegree = valueService.GetDegreeOfSlope(triC.DistanceTo(triB), triA.DistanceTo(triC));

                returnValue.CompressionRingDegree = tempDegree;
            }


            // Dome Radius
            returnValue.DomeRaidus = domeRadius;


            // 검증 용
            if (false)
            {
                List<Entity> newList = new List<Entity>();
                newList.Add(new Line(refPointLeft, GetSumPoint(refPointLeft, 0, -1000)));
                newList.Add(new Line(refPointRight, GetSumPoint(refPointRight, 0, -1000)));
                newList.Add(cirLeft);
                newList.Add(cirRight);
                newList.Add(cirCenter);
                //newList.Add(leftAngleLine);
                //styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);
                newList.Add(new Line(refPointLeft, GetSumPoint(refPointLeft, -7, 0)));
                newList.Add(new Line(refPointLeft, refPointRight));
                newList.Add(new Circle(currentCenterPoint, domeRadius));
            }

            return returnValue;
        }



        private Point3D GetDRTRoofPoint(Circle selCircle, double selRadius)
        {
            Line vLine = new Line(GetSumPoint(DRTWorkingData.RoofCenterPoint,-selRadius,0), GetSumPoint(DRTWorkingData.RoofCenterPoint, -selRadius, DRTWorkingData.PointCenterTopUp.Y + 99999));
            Point3D[] vPoint = vLine.IntersectWith(selCircle);
            return vPoint[0];
        }
        #endregion




        #region Distance
        private double GetDistanceX(double selX1, double selX2)
        {
            return Point3D.Distance(new Point3D(selX1, 0, 0), new Point3D(selX2, 0, 0));
        }
        private double GetDistanceY(double selY1, double selY2)
        {
            return Point3D.Distance(new Point3D(0,selY1, 0), new Point3D(0, selY2, 0));
        }
        #endregion

        #region Vertical Height By Degree
        private double GetVerticalHeightByDegree(double selDegree,double selHeight)
        {
            double firstA = valueService.GetAdjacentByHypotenuse(selDegree, selHeight);
            double firstO = valueService.GetOppositeByHypotenuse(selDegree, selHeight);
            double secondO = valueService.GetOppositeByWidth(selDegree,firstO);
            return firstA + secondO;
        }
        #endregion



        private CDPoint GetSumCDPoint(CDPoint selPoint1, double X, double Y, double Z = 0)
        {
            return new CDPoint(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }

        private CDPoint GetSumCDPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new CDPoint(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
        private Point3D GetSumPoint(CDPoint selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
    }
}
