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



namespace DrawWork.DrawServices
{
    public class DrawContactPointService
    {
        private AssemblyModel assemblyData;

        private ValueService valueService;

        public DrawContactPointService(AssemblyModel selAssembly)
        {
            assemblyData = selAssembly;
            valueService = new ValueService();
        }

        public CDPoint ContactPoint(string selPoint, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            return ContactPointOrigin(selPoint, "", ref refPoint, ref curPoint);
        }
        public CDPoint ContactPoint(string selPoint, string selPointValue, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            return ContactPointOrigin(selPoint, selPointValue,ref refPoint,ref curPoint);
        }

        private CDPoint ContactPointOrigin(string selPoint, string selPointValue, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            CDPoint cpPoint = new CDPoint();
            switch (selPoint)
            {
                case "topangleroofpoint":
                    switch (assemblyData.RoofInput[0].TopAngleType)
                    {
                        case "b":

                            EqualAngleSizeModel eachAngle = assemblyData.RoofAngleOutput[0];
                            // Max course count
                            int maxCourse = valueService.GetIntValue(assemblyData.ShellInput[0].CourseCount) - 1;
                            // Point
                            cpPoint.X = refPoint.X
                                        - valueService.GetDoubleValue(eachAngle.E)
                                        - valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].MinThk);
                            cpPoint.Y = refPoint.Y
                                        + valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeTankHeight);
                            break;

                            /*
                            string newAngleSize = assemblyData.RoofInput[0].TopAngleSize;
                            foreach (EqualAngleSizeModel eachAngle in assemblyData.AngleIList)
                            {
                                if (eachAngle.SIZE == newAngleSize)
                                {
                                    // Max course count
                                    int maxCourse = valueService.GetIntValue(assemblyData.ShellInput[0].CourseCount) - 1;
                                    // Point
                                    cpPoint.X = refPoint.X 
                                                - valueService.GetDoubleValue(eachAngle.E)
                                                - valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].MinThk);
                                    cpPoint.Y = refPoint.Y 
                                                + valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeTankHeight);
                                    break;
                                }
                            }
                            break;
                            */
                    }
                    break;

                case "topangleshellpoint":
                    switch (assemblyData.RoofInput[0].TopAngleType)
                    {
                        case "b":
                            EqualAngleSizeModel eachAngle = assemblyData.RoofAngleOutput[0];
                            // Max course count
                            int maxCourse = valueService.GetIntValue(assemblyData.ShellInput[0].CourseCount) - 1;
                            // Point
                            cpPoint.X = refPoint.X
                                        - valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].MinThk);
                            cpPoint.Y = refPoint.Y
                                        + valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeTankHeight)
                                        - valueService.GetDoubleValue(assemblyData.RoofOutput[0].TwoTcMax);
                            break;
                    }
                    break;

                case "centerlinetoppoint":
                    switch (assemblyData.RoofInput[0].TopAngleType)
                    {
                        case "b":
                            string newAngleSize = assemblyData.RoofInput[0].TopAngleSize;

                            // top angle roof point
                            CDPoint topAngleRoofPoint = ContactPoint("topangleroofpoint", ref refPoint, ref curPoint);

                            double tempWidth = Point3D.Distance(new Point3D(topAngleRoofPoint.X, 0, 0), new Point3D(refPoint.X + valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeNominalId) / 2, 0, 0));
                            double tempHeight = valueService.GetSlopeOfHeight(assemblyData.RoofInput[0].RoofSlopeOne, tempWidth);

                            // Point
                            cpPoint.X = refPoint.X
                                        + valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeNominalId) / 2;
                            cpPoint.Y = refPoint.Y
                                        + valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeTankHeight)
                                        + tempHeight;
                            break;
                    }
                    break;

                case "bottomleftpoint":
                    switch (assemblyData.RoofInput[0].TopAngleType)
                    {
                        case "b":

                            double tempHeight = valueService.GetSlopeOfHeight(assemblyData.BottomInput[0].BottomSlope, assemblyData.BottomInput[0].BottomThickness);

                            // Point
                            cpPoint.X = refPoint.X
                                        + valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateWidth)
                                        - valueService.GetDoubleValue(assemblyData.BottomOutput[0].ShellThk)
                                        - valueService.GetDoubleValue(assemblyData.BottomOutput[0].OutSideProjection)
                                        - valueService.GetDoubleValue(assemblyData.BottomOutput[0].OverlapOfAnnular)
                                        - tempHeight;
                            cpPoint.Y = refPoint.Y
                                        + valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomThickness);
                            break;
                    }
                    break;

                case "centerlinebottompoint":
                    switch (assemblyData.RoofInput[0].TopAngleType)
                    {
                        case "b":


                            // top angle roof point
                            CDPoint bottomleftpoint = ContactPoint("bottomleftpoint", ref refPoint, ref curPoint);

                            double tempWidth = Point3D.Distance(new Point3D(bottomleftpoint.X, 0, 0), new Point3D(refPoint.X + valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeNominalId) / 2, 0, 0));
                            double tempHeight = valueService.GetSlopeOfHeight(assemblyData.BottomInput[0].BottomSlope, tempWidth);

                            // Point
                            cpPoint.X = refPoint.X
                                        + valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeNominalId) / 2;
                            cpPoint.Y = refPoint.Y
                                        + valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomThickness)
                                        + tempHeight;
                            break;
                    }
                    break;

                // Roof
                case "leftroofpoint":
                    CDPoint tempLeftRootPoint = ContactPoint("centerlinetoppoint", ref refPoint, ref curPoint);
                    double tempTankLeftHalf = valueService.GetDoubleValue(selPointValue) - valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeNominalId) / 2;
                    double tempLeftRoofHeight = valueService.GetSlopeOfHeight(assemblyData.RoofInput[0].RoofSlopeOne, tempTankLeftHalf.ToString());

                    // Point
                    cpPoint.X = tempLeftRootPoint.X
                                + tempTankLeftHalf;
                    cpPoint.Y = tempLeftRootPoint.Y
                                + tempLeftRoofHeight;
                    break;

                case "centerroofpoint":
                    CDPoint tempRightRootPoint = ContactPoint("centerlinetoppoint", ref refPoint, ref curPoint);
                    double tempRightRoofHeight = valueService.GetSlopeOfHeight(assemblyData.RoofInput[0].RoofSlopeOne, selPointValue);

                    // Point
                    cpPoint.X = tempRightRootPoint.X
                                + valueService.GetDoubleValue(selPointValue);
                    cpPoint.Y = tempRightRootPoint.Y
                                + tempRightRoofHeight;
                    break;

                // Bottom
                case "leftbottompoint":
                    CDPoint tempLeftBottomPoint = ContactPoint("bottomleftpoint", ref refPoint, ref curPoint);
                    double tempLeftBottomHeight = valueService.GetSlopeOfHeight(assemblyData.BottomInput[0].BottomSlope, selPointValue);

                    // Point
                    cpPoint.X = tempLeftBottomPoint.X
                                + valueService.GetDoubleValue(selPointValue);
                    cpPoint.Y = tempLeftBottomPoint.Y
                                + tempLeftBottomHeight;
                    break;

                case "centerbottompoint":
                    CDPoint tempCenterBottomPoint = ContactPoint("centerlinebottompoint", ref refPoint, ref curPoint);
                    double tempCenterBottomHeight = valueService.GetSlopeOfHeight(assemblyData.BottomInput[0].BottomSlope, selPointValue);

                    // Point
                    cpPoint.X = tempCenterBottomPoint.X
                                + valueService.GetDoubleValue(selPointValue);
                    cpPoint.Y = tempCenterBottomPoint.Y
                                + tempCenterBottomHeight;
                    break;

                default:
                    cpPoint = null;
                    break;
            }

            return cpPoint;
        }
    }
}
