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

namespace DrawWork.DrawServices
{
    public class DrawWorkingPointService
    {
        private AssemblyModel assemblyData;

        private ValueService valueService;

        public DrawWorkingPointService(AssemblyModel selAssembly)
        {
            assemblyData = selAssembly;
            valueService = new ValueService();
        }

        public CDPoint WorkingPoint(string selPoint, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            return WorkingPointOrigin(CommonMethod.WorkingPointToEnum(selPoint), 0, ref refPoint, ref curPoint);
        }
        public CDPoint WorkingPoint(string selPoint, string selPointValue, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            return WorkingPointOrigin(CommonMethod.WorkingPointToEnum(selPoint), valueService.GetDoubleValue(selPointValue), ref refPoint, ref curPoint);
        }

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
            string selSizeNominalId = assemblyData.GeneralDesignData[refFirstIndex].SizeNominalId;

            CDPoint WPPoint = new CDPoint();
            switch (selPoint)
            {


                // Point : Center

                case WORKINGPOINT_TYPE.PointCenterTopUp:                // 2021-04-22 완료
                    CDPoint topcenterpoint = WorkingPoint(WORKINGPOINT_TYPE.PointCenterTopDown, ref refPoint, ref curPoint);
                    double verticalHeight2 = valueService.GetHypotenuseByWidth(assemblyData.RoofInput[0].RoofSlopeOne, assemblyData.RoofInput[0].RoofThickness);
                    WPPoint = GetSumCDPoint(topcenterpoint, 0, verticalHeight2);
                    break;

                case WORKINGPOINT_TYPE.PointCenterTopDown:              // 2021-04-22 완료

                    // top angle roof point
                    CDPoint topAngleRoofPoint = WorkingPoint(WORKINGPOINT_TYPE.PointLeftRoofDown, ref refPoint, ref curPoint);

                    double tempWidth3 = GetDistanceX(topAngleRoofPoint.X, refPoint.X + valueService.GetDoubleValue(selSizeNominalId) / 2);
                    double tempHeight3 = valueService.GetOppositeByWidth(assemblyData.RoofInput[0].RoofSlopeOne, tempWidth3);

                    WPPoint = GetSumCDPoint(topAngleRoofPoint, tempWidth3, tempHeight3);

                    break;


                case WORKINGPOINT_TYPE.PointCenterBottomUp:             // 2021-04-22 완료
                    CDPoint bottomcenterpoint = WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomDown, ref refPoint, ref curPoint);
                    double verticalHeight = valueService.GetHypotenuseByWidth(assemblyData.BottomInput[0].BottomSlope, assemblyData.BottomInput[0].BottomThickness);
                    WPPoint = GetSumCDPoint(bottomcenterpoint, 0, verticalHeight);
                    break;

                case WORKINGPOINT_TYPE.PointCenterBottomDown:           // 2021-04-22 완료
                    CDPoint bottomleftpoint = WorkingPoint(WORKINGPOINT_TYPE.PointLeftBottomDown, ref refPoint, ref curPoint);

                    double tempWidth2 = GetDistanceX(bottomleftpoint.X, refPoint.X + valueService.GetDoubleValue(selSizeNominalId) / 2);
                    double tempHeight2 = valueService.GetOppositeByWidth(assemblyData.BottomInput[0].BottomSlope, tempWidth2);

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
                                            valueService.GetOppositeByHypotenuse(assemblyData.BottomInput[0].BottomSlope, assemblyData.BottomInput[0].BottomThickness), 
                                            valueService.GetAdjacentByHypotenuse(assemblyData.BottomInput[0].BottomSlope, assemblyData.BottomInput[0].BottomThickness));
                    break;

                case WORKINGPOINT_TYPE.PointLeftBottomDown:             // Entry Point : 2021-04-22 완료
                    WPPoint = PointCenterBottomDown(ref refPoint, ref curPoint);
                    break;


                // Point : Roof
                case WORKINGPOINT_TYPE.PointLeftRoofUp:                 // 2021-04-22 완료
                    CDPoint topleftpoint = WorkingPoint(WORKINGPOINT_TYPE.PointLeftRoofDown, ref refPoint, ref curPoint);
                    WPPoint = GetSumCDPoint(topleftpoint,
                                            valueService.GetOppositeByHypotenuse(assemblyData.RoofInput[0].RoofSlopeOne, assemblyData.RoofInput[0].RoofThickness),
                                            valueService.GetAdjacentByHypotenuse(assemblyData.RoofInput[0].RoofSlopeOne, assemblyData.RoofInput[0].RoofThickness));
                    break;

                case WORKINGPOINT_TYPE.PointLeftRoofDown:               // Entiry Point : 2021-04-22 완료
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
                    CDPoint tempCenterRoofPoint2 = WorkingPoint(WORKINGPOINT_TYPE.PointCenterTopUp, ref refPoint, ref curPoint);
                    double tempRightRoofHeight2 = valueService.GetOppositeByWidth(assemblyData.RoofInput[0].RoofSlopeOne, selPointValue);

                    WPPoint = GetSumCDPoint(tempCenterRoofPoint2, -selPointValue, -tempRightRoofHeight2);
                    break;
                    
                case WORKINGPOINT_TYPE.AdjCenterRoofDown:               // 2021-04-22 완료
                    CDPoint tempCenterRoofPoint = WorkingPoint(WORKINGPOINT_TYPE.PointCenterTopDown, ref refPoint, ref curPoint);
                    double tempLeftRoofHeight = valueService.GetOppositeByWidth(assemblyData.RoofInput[0].RoofSlopeOne, selPointValue);

                    WPPoint = GetSumCDPoint(tempCenterRoofPoint, -selPointValue, -tempLeftRoofHeight);
                    break;

                case WORKINGPOINT_TYPE.AdjLeftRoofUp:                   // 2021-04-22 완료
                    double tempTankWidthHalf2 = valueService.GetDoubleValue(selSizeNominalId) / 2 - selPointValue;
                    CDPoint tempCenterRoofPoint4 = WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofUp, tempTankWidthHalf2.ToString(), ref refPoint, ref curPoint);

                    WPPoint = GetSumCDPoint(tempCenterRoofPoint4, 0, 0);
                    break;

                case WORKINGPOINT_TYPE.AdjLeftRoofDown:                 // 2021-04-22 완료
                    double tempTankWidthHalf=  valueService.GetDoubleValue(selSizeNominalId) / 2 - selPointValue;
                    CDPoint tempCenterRoofPoint3 = WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, tempTankWidthHalf.ToString(), ref refPoint, ref curPoint);

                    WPPoint = GetSumCDPoint(tempCenterRoofPoint3, 0, 0);
                    break;


                // Adj : Bottom
                case WORKINGPOINT_TYPE.AdjCenterBottomUp:               // 2021-04-22 완료
                    CDPoint tempCenterBottomPoint2 = WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomUp, ref refPoint, ref curPoint);
                    double tempCenterBottomHeight2 = valueService.GetOppositeByWidth(assemblyData.BottomInput[0].BottomSlope, selPointValue);

                    WPPoint = GetSumCDPoint(tempCenterBottomPoint2, -selPointValue, -tempCenterBottomHeight2);
                    break;

                case WORKINGPOINT_TYPE.AdjCenterBottomDown:             // 2021-04-22 완료
                    CDPoint tempCenterBottomPoint = WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomDown, ref refPoint, ref curPoint);
                    double tempCenterBottomHeight = valueService.GetOppositeByWidth(assemblyData.BottomInput[0].BottomSlope, selPointValue);

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
            string selSizeNominalId = assemblyData.GeneralDesignData[refFirstIndex].SizeNominalId;
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

            int maxCourse = valueService.GetIntValue(assemblyData.ShellInput[0].CourseCount);
            double sumHeight = 0;
            for (int i = 0; i < maxCourse; i++)
            {
                double plateWidth = valueService.GetDoubleValue(assemblyData.ShellOutput[i].Width);
                sumHeight += plateWidth;
                if (selHeight < sumHeight)
                {
                    double minThickness = valueService.GetDoubleValue(assemblyData.ShellOutput[i].MinThk);
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
            int refFirstIndex = 0;

            string selSizeTankHeight = assemblyData.GeneralDesignData[refFirstIndex].SizeTankHeight;
            //string selSizeNominalId = assemblyData.GeneralDesignData[refFirstIndex].SizeNominalId;

            CDPoint newPoint = new CDPoint();
            // Angle Model
            AngleSizeModel eachAngle = assemblyData.RoofAngleOutput[0];
            // Max course count
            int maxCourse = valueService.GetIntValue(assemblyData.ShellInput[0].CourseCount) - 1;

            switch (assemblyData.RoofInput[0].TopAngleType)
            {
                case "b":
                    // Point
                    newPoint = GetSumCDPoint(refPoint,
                                             - valueService.GetDoubleValue(eachAngle.E)
                                             - valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].MinThk),

                                             + valueService.GetDoubleValue(selSizeTankHeight));

                    break;

                case "d":
                    // Point
                    newPoint = GetSumCDPoint(refPoint,
                                             - valueService.GetDoubleValue(eachAngle.E),

                                             + valueService.GetDoubleValue(selSizeTankHeight));


                    break;

                case "e":
                    // Point
                    newPoint = GetSumCDPoint(refPoint,
                                             + valueService.GetDoubleValue(eachAngle.E)
                                             - valueService.GetDoubleValue(eachAngle.t),

                                             + valueService.GetDoubleValue(selSizeTankHeight));


                    break;


            }

            return newPoint;
        }

        private CDPoint PointTopAngleShell(ref CDPoint refPoint, ref CDPoint curPoint)
        {

            int refFirstIndex = 0;

            string selSizeTankHeight = assemblyData.GeneralDesignData[refFirstIndex].SizeTankHeight;
            //string selSizeNominalId = assemblyData.GeneralDesignData[refFirstIndex].SizeNominalId;

            CDPoint newPoint = new CDPoint();
            // Angle Model
            AngleSizeModel eachAngle = assemblyData.RoofAngleOutput[0];
            // Max course count
            int maxCourse = valueService.GetIntValue(assemblyData.ShellInput[0].CourseCount) - 1;

            switch (assemblyData.RoofInput[0].TopAngleType)
            {
                case "b":

                    // Point
                    newPoint = GetSumCDPoint(refPoint,
                                                   - valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].MinThk),

                                                   + valueService.GetDoubleValue(selSizeTankHeight)
                                                   - valueService.GetDoubleValue(assemblyData.RoofOutput[0].TwoTcMax));

                    break;

                case "d":
                case "e":

                    // Point
                    newPoint = GetSumCDPoint(refPoint,
                                                   0,

                                                   + valueService.GetDoubleValue(selSizeTankHeight)
                                                   - valueService.GetDoubleValue(eachAngle.AB));

                    break;
            }

            return newPoint;
        }

        #endregion

        #region Point : Bottom
        private CDPoint PointCenterBottomDown(ref CDPoint refPoint, ref CDPoint curPoint)
        {
            return GetSumCDPoint(refPoint,
                                    + valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateWidth)
                                    - valueService.GetDoubleValue(assemblyData.BottomOutput[0].ShellThk)
                                    - valueService.GetDoubleValue(assemblyData.BottomOutput[0].OutSideProjection)
                                    - valueService.GetDoubleValue(assemblyData.BottomOutput[0].OverlapOfAnnular),

                                    0);
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
    }
}
