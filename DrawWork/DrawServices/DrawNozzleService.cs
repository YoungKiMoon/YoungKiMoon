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
using MColor = System.Windows.Media.Color;

using AssemblyLib.AssemblyModels;

using DrawWork.ValueServices;
using DrawWork.DrawModels;

namespace DrawWork.DrawServices
{
    public class DrawNozzleService
    {
        private ValueService valueService;

        public DrawNozzleService()
        {
            valueService = new ValueService();
        }

        public Entity[] DrawNozzle_GA(ref CDPoint refPoint, string selNozzleType,string selNozzlePosition, AssemblyModel selAssembly)
        {

            // Shell Spacing
            double shellSpacingLeft = 400;
            double shellSpacingRight = 400;
            double shellSpacingTop = 400;
            double shellSpacingBottom = 400;

            // Nozzle Area
            double lowerAreaLeft = refPoint.Y;
            double upperAreaLeft = valueService.GetDoubleValue(selAssembly.GeneralDesignData.SizeTankHeight);
            double lowerAreaRight = 400;
            double upperAreaRight = 400;
            double lowerAreaBottom = 400;
            double upperAreaBottom = 400;
            double lowerAreaTop = 400;
            double upperAreaTop = 400;

            // Type
            switch (selNozzleType)
            {
                case "Bottom":
                    //drawPoint = GetSumPoint(drawPoint, -AB, -AB);

                    break;

            }

            foreach(NozzleInputModel eachNozzle in selAssembly.NozzleInputModel)
            {
                if (eachNozzle.Position.ToLower() == selNozzlePosition)
                {

                }
            }

            // Line
            // Circle



            List<Entity> customBlockList = new List<Entity>();


            //customBlockList.Add(lineA);

            return customBlockList.ToArray();
        }

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
    }
}
