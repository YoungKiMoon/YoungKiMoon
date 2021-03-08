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

        private ValueService valueService;

        public DrawContactPointService()
        {
            valueService = new ValueService();
        }

        public CDPoint ContactPoint(string selPoint, ref CDPoint refPoint, ref CDPoint curPoint, AssemblyModel selAssembly)
        {
            CDPoint cpPoint = new CDPoint();
            switch (selPoint)
            {
                case "topangleroofpoint":
                    switch (selAssembly.RoofInput[0].TopAngleType)
                    {
                        case "b":
                            string newAngleSize = selAssembly.RoofInput[0].TopAneSize;
                            foreach (EqualAngleSizeModel eachAngle in selAssembly.AngleInput)
                            {
                                if (eachAngle.Size == newAngleSize)
                                {
                                    // Point
                                    cpPoint.X = refPoint.X - valueService.GetDoubleValue(eachAngle.E);
                                    cpPoint.Y = valueService.GetDoubleValue(selAssembly.GeneralDesignData.SizeTankHeight);
                                    break;
                                }
                            }
                            break;
                    }
                    break;

                case "topangleshellpoint":
                    switch (selAssembly.RoofInput[0].TopAngleType)
                    {
                        case "b":
                            string newAngleSize = selAssembly.RoofInput[0].TopAneSize;
                            foreach (EqualAngleSizeModel eachAngle in selAssembly.AngleInput)
                            {
                                if (eachAngle.Size == newAngleSize)
                                {
                                    // Point
                                    cpPoint.X = refPoint.X;
                                    cpPoint.Y = valueService.GetDoubleValue(selAssembly.GeneralDesignData.SizeTankHeight)
                                                - valueService.GetDoubleValue(selAssembly.RoofOutput[0].TwoTcMax);
                                    break;
                                }
                            }
                            break;
                    }
                    break;

                case "centerlinetoppoint":
                    switch (selAssembly.RoofInput[0].TopAngleType)
                    {
                        case "b":
                            string newAngleSize = selAssembly.RoofInput[0].TopAneSize;
                            foreach (EqualAngleSizeModel eachAngle in selAssembly.AngleInput)
                            {
                                if (eachAngle.Size == newAngleSize)
                                {
                                    // top angle roof point
                                    CDPoint topAngleRoofPoint = ContactPoint("topangleroofpoint", ref refPoint, ref curPoint, selAssembly);

                                    // arctan // X: 1로 고정
                                    double calDegree = Math.Atan2(1, valueService.GetDoubleValue(selAssembly.RoofInput[0].RoofSlopeOne));

                                    double tempWidth = Point3D.Distance(new Point3D(topAngleRoofPoint.X + refPoint.X, 0, 0), new Point3D(refPoint.X + valueService.GetDoubleValue(selAssembly.GeneralDesignData.SizeNominalId) / 2, 0, 0));
                                    double tempHeight = tempWidth * calDegree;

                                    // Point
                                    cpPoint.X = refPoint.X + valueService.GetDoubleValue(selAssembly.GeneralDesignData.SizeNominalId) / 2;
                                    cpPoint.Y = tempHeight;
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
