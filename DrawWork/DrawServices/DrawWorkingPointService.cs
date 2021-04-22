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

        public CDPoint ContactPoint(string selPoint, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            return ContactPointOrigin(CommonMethod.WorkingPointToEnum(selPoint), "", ref refPoint, ref curPoint);
        }
        public CDPoint ContactPoint(string selPoint, string selPointValue, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            return ContactPointOrigin(CommonMethod.WorkingPointToEnum(selPoint), selPointValue, ref refPoint, ref curPoint);
        }

        public CDPoint ContactPoint(WORKINGPOINT_TYPE selPoint, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            return ContactPointOrigin(selPoint, "", ref refPoint, ref curPoint);
        }
        public CDPoint ContactPoint(WORKINGPOINT_TYPE selPoint, string selPointValue, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            return ContactPointOrigin(selPoint, selPointValue,ref refPoint,ref curPoint);
        }

        private CDPoint ContactPointOrigin(WORKINGPOINT_TYPE selPoint, string selPointValue, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            CDPoint cpPoint = new CDPoint();
            switch (selPoint)
            {

                // Point : Center

                case WORKINGPOINT_TYPE.PointCenterTopUp:
                    break;

                case WORKINGPOINT_TYPE.PointCenterTopDown:

                    // top angle roof point
                    CDPoint topAngleRoofPoint = ContactPoint(WORKINGPOINT_TYPE.PointLeftRoofDown, ref refPoint, ref curPoint);

                    double tempWidth3 = Point3D.Distance(new Point3D(topAngleRoofPoint.X, 0, 0), new Point3D(refPoint.X + valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeNominalId) / 2, 0, 0));
                    double tempHeight3 = valueService.GetOppositeByWidth(assemblyData.RoofInput[0].RoofSlopeOne, tempWidth3);

                    cpPoint = GetSumCDPoint(refPoint,
                                            valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeNominalId) / 2,
                                            valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeTankHeight) + tempHeight3);

                    break;


                case WORKINGPOINT_TYPE.PointCenterBottomUp:
                    break;

                case WORKINGPOINT_TYPE.PointCenterBottomDown:


                    CDPoint bottomleftpoint = ContactPoint(WORKINGPOINT_TYPE.PointLeftBottomDown, ref refPoint, ref curPoint);

                    double tempWidth2 = Point3D.Distance(new Point3D(bottomleftpoint.X, 0, 0), new Point3D(refPoint.X + valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeNominalId) / 2, 0, 0));
                    double tempHeight2 = valueService.GetOppositeByWidth(assemblyData.BottomInput[0].BottomSlope, tempWidth2);

                    cpPoint = GetSumCDPoint(refPoint,
                                            valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeNominalId) / 2,
                                            valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomThickness) + tempHeight2);
                    break;


                case WORKINGPOINT_TYPE.PointCenterTop:
                    break;

                case WORKINGPOINT_TYPE.PointCenterBottom:
                    break;


                // Point : Bottom
                case WORKINGPOINT_TYPE.PointLeftBottomUp:
                    break;

                case WORKINGPOINT_TYPE.PointLeftBottomDown:
                    double tempHeight = valueService.GetOppositeByWidth(assemblyData.BottomInput[0].BottomSlope, assemblyData.BottomInput[0].BottomThickness);
                    cpPoint.X = refPoint.X
                                + valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateWidth)
                                - valueService.GetDoubleValue(assemblyData.BottomOutput[0].ShellThk)
                                - valueService.GetDoubleValue(assemblyData.BottomOutput[0].OutSideProjection)
                                - valueService.GetDoubleValue(assemblyData.BottomOutput[0].OverlapOfAnnular)
                                - tempHeight;
                    cpPoint.Y = refPoint.Y
                                + valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomThickness);

                    break;


                // Point : Roof
                case WORKINGPOINT_TYPE.PointLeftRoofUp:
                    break;

                case WORKINGPOINT_TYPE.PointLeftRoofDown:
                    cpPoint = PointTopAngleRoof(ref refPoint, ref curPoint);
                    break;



                // Point : Shell
                case WORKINGPOINT_TYPE.PointLeftShellTop:
                    break;
                case WORKINGPOINT_TYPE.PointLeftShellTopAdj:
                    cpPoint = PointTopAngleShell(ref refPoint, ref curPoint);
                    break;
                case WORKINGPOINT_TYPE.PointReference:
                case WORKINGPOINT_TYPE.PointLeftShellBottom:
                    cpPoint = GetSumCDPoint(refPoint, 0, 0);
                    break;

                case WORKINGPOINT_TYPE.PointRightShellTop:
                    break;
                case WORKINGPOINT_TYPE.PointRightShellTopAdj:
                    break;
                case WORKINGPOINT_TYPE.PointRightShellBottom:
                    break;


                // Adj : Roof
                case WORKINGPOINT_TYPE.AdjCenterRoofUp:
                    break;
                case WORKINGPOINT_TYPE.AdjCenterRoofDown:
                    // Point : Center Top
                    CDPoint tempRightRootPoint = ContactPoint(WORKINGPOINT_TYPE.PointCenterTopDown, ref refPoint, ref curPoint);
                    // Adj : selPointValue : Distance
                    double tempRightRoofHeight = valueService.GetOppositeByWidth(assemblyData.RoofInput[0].RoofSlopeOne, selPointValue);

                    cpPoint = GetSumCDPoint(tempRightRootPoint,
                                            valueService.GetDoubleValue(selPointValue),
                                            tempRightRoofHeight);
                    break;
                case WORKINGPOINT_TYPE.AdjLeftRoofUp:
                    break;
                case WORKINGPOINT_TYPE.AdjLeftRoofDown:
                    CDPoint tempLeftRootPoint = ContactPoint(WORKINGPOINT_TYPE.PointCenterTopDown, ref refPoint, ref curPoint);
                    double tempTankLeftHalf = valueService.GetDoubleValue(selPointValue) - valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeNominalId) / 2;
                    double tempLeftRoofHeight = valueService.GetOppositeByWidth(assemblyData.RoofInput[0].RoofSlopeOne, tempTankLeftHalf.ToString());

                    // Point
                    cpPoint.X = tempLeftRootPoint.X
                                + tempTankLeftHalf;
                    cpPoint.Y = tempLeftRootPoint.Y
                                + tempLeftRoofHeight;
                    break;


                // Adj : Bottom
                case WORKINGPOINT_TYPE.AdjCenterBottomUp:
                    break;
                case WORKINGPOINT_TYPE.AdjCenterBottomDown:
                    CDPoint tempCenterBottomPoint = ContactPoint(WORKINGPOINT_TYPE.PointCenterBottomDown, ref refPoint, ref curPoint);
                    double tempCenterBottomHeight = valueService.GetOppositeByWidth(assemblyData.BottomInput[0].BottomSlope, selPointValue);

                    cpPoint = GetSumCDPoint(tempCenterBottomPoint,
                                            valueService.GetDoubleValue(selPointValue),
                                            tempCenterBottomHeight);
                    break;


                // Adj : Shell
                case WORKINGPOINT_TYPE.AdjLeftShell:
                    cpPoint = AdjLeftShell(selPointValue, ref refPoint, ref curPoint);
                    break;
                case WORKINGPOINT_TYPE.AdjRightShell:
                    cpPoint = AdjRightShell(selPointValue, ref refPoint, ref curPoint);
                    break;



                default:
                    cpPoint = null;
                    break;
            }

            return cpPoint;
        }


        // Adj : Left Shell
        private CDPoint AdjLeftShell( string selHeight, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // Shell Bottom Y = 0

            double currentHeight= valueService.GetDoubleValue(selHeight);
            double currentThickness = GetShellThicknessAccordingToHeight(selHeight);

            CDPoint newPoint = GetSumCDPoint(refPoint, -currentThickness, currentHeight);
            return newPoint;
        }
        private CDPoint AdjRightShell(string selHeight, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // Shell Bottom Y = 0

            double currentHeight = valueService.GetDoubleValue(selHeight);
            double currentThickness = GetShellThicknessAccordingToHeight(selHeight);
            double tankWidth = valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeNominalId);

            CDPoint newPoint = GetSumCDPoint(refPoint, tankWidth + currentThickness, currentHeight);
            return newPoint;
        }
        public double GetShellThicknessAccordingToHeight(string selHeight)
        {
            double currentHeight = valueService.GetDoubleValue(selHeight);
            double currentThickness = 0;

            int maxCourse = valueService.GetIntValue(assemblyData.ShellInput[0].CourseCount);
            double sumHeight = 0;
            for (int i = 0; i < maxCourse; i++)
            {
                double plateWidth = valueService.GetDoubleValue(assemblyData.ShellOutput[i].Width);
                sumHeight += plateWidth;
                if (currentHeight < sumHeight)
                {
                    double minThickness = valueService.GetDoubleValue(assemblyData.ShellOutput[i].MinThk);
                    currentThickness = minThickness;
                    break;
                }
            }
            return currentThickness;
        }

        // Point : Top Angle Roof
        private CDPoint PointTopAngleRoof(ref CDPoint refPoint, ref CDPoint curPoint)
        {
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

                                             + valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeTankHeight));

                    break;

                case "d":
                    // Point
                    newPoint = GetSumCDPoint(refPoint,
                                             - valueService.GetDoubleValue(eachAngle.E),

                                             + valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeTankHeight));


                    break;

                case "e":
                    // Point
                    newPoint = GetSumCDPoint(refPoint,
                                             + valueService.GetDoubleValue(eachAngle.E)
                                             - valueService.GetDoubleValue(eachAngle.t),

                                             + valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeTankHeight));


                    break;


            }

            return newPoint;
        }
        // Point : Top Angel Shell
        private CDPoint PointTopAngleShell(ref CDPoint refPoint, ref CDPoint curPoint)
        {
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

                                                   + valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeTankHeight)
                                                   - valueService.GetDoubleValue(assemblyData.RoofOutput[0].TwoTcMax));

                    break;

                case "d":
                case "e":

                    // Point
                    newPoint = GetSumCDPoint(refPoint,
                                                   0,

                                                   + valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeTankHeight)
                                                   - valueService.GetDoubleValue(eachAngle.AB));

                    break;
            }

            return newPoint;
        }



        private CDPoint GetSumCDPoint(CDPoint selPoint1, double X, double Y, double Z = 0)
        {
            return new CDPoint(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
    }
}
