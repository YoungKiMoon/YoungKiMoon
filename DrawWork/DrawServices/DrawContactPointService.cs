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
            CDPoint cpPoint = new CDPoint();
            switch (selPoint)
            {
                case "topangleroofpoint":
                    switch (assemblyData.RoofInput[0].TopAngleType)
                    {
                        case "b":
                            string newAngleSize = assemblyData.RoofInput[0].TopAneSize;
                            foreach (EqualAngleSizeModel eachAngle in assemblyData.AngleInput)
                            {
                                if (eachAngle.Size == newAngleSize)
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
                    }
                    break;

                case "topangleshellpoint":
                    switch (assemblyData.RoofInput[0].TopAngleType)
                    {
                        case "b":
                            string newAngleSize = assemblyData.RoofInput[0].TopAneSize;
                            foreach (EqualAngleSizeModel eachAngle in assemblyData.AngleInput)
                            {
                                if (eachAngle.Size == newAngleSize)
                                {
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
                            }
                            break;
                    }
                    break;

                case "centerlinetoppoint":
                    switch (assemblyData.RoofInput[0].TopAngleType)
                    {
                        case "b":
                            string newAngleSize = assemblyData.RoofInput[0].TopAneSize;
                            foreach (EqualAngleSizeModel eachAngle in assemblyData.AngleInput)
                            {
                                if (eachAngle.Size == newAngleSize)
                                {
                                    // top angle roof point
                                    CDPoint topAngleRoofPoint = ContactPoint("topangleroofpoint", ref refPoint, ref curPoint);

                                    // arctan // X: 1로 고정
                                    double calDegree = Math.Atan2(1, valueService.GetDoubleValue(assemblyData.RoofInput[0].RoofSlopeOne));

                                    double tempWidth = Point3D.Distance(new Point3D(topAngleRoofPoint.X + refPoint.X, 0, 0), new Point3D(refPoint.X + valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeNominalId) / 2, 0, 0));
                                    double tempHeight = tempWidth * calDegree;

                                    // Point
                                    cpPoint.X = refPoint.X 
                                                + valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeNominalId) / 2;
                                    cpPoint.Y = refPoint.Y 
                                                + tempHeight;
                                    break;
                                }
                            }
                            break;
                    }

                    break;
                default:
                    cpPoint = null;
                    break;
            }

            return cpPoint;
        }
    }
}
