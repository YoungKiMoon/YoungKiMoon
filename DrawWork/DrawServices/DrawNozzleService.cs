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



            // Model
            double flangeFaceWidth = 10;
            double flangeFaceHeight = 80;
            double flangeFaceInnerWidth = 10;
            double flangePipeWidth = 24;
            double flangePipeHeight = flangeFaceHeight - (flangeFaceInnerWidth * 2);
            double flangePipeInnerWidth = 18;
            double pipeHeight = flangeFaceHeight - (flangePipeInnerWidth * 2);
            double pipeWidth = 40;
            double fullWidth = flangeFaceWidth + flangePipeWidth + pipeWidth;

            foreach (NozzleInputModel eachNozzle in selAssembly.NozzleInputModel)
            {
                if (eachNozzle.Position.ToLower() == selNozzlePosition)
                {
                    Point3D drawPoint = new Point3D(0, 0, 0);
                }
            }


            Line lineFFa = new Line(new Point3D(0, 0, 0), new Point3D(0, flangeFaceHeight, 0));
            Line lineFFb = new Line(new Point3D(flangeFaceWidth, 0, 0), new Point3D(flangeFaceWidth, flangeFaceHeight, 0));
            Line lineFFc = new Line(new Point3D(0, 0, 0), new Point3D(flangeFaceWidth, 0, 0));
            Line lineFFd = new Line(new Point3D(0, flangeFaceHeight, 0), new Point3D(flangeFaceWidth, flangeFaceHeight, 0));

            Line lineFPa = new Line(new Point3D(flangeFaceWidth, flangeFaceInnerWidth + flangePipeHeight, 0), new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth + pipeHeight, 0));
            Line lineFPb = new Line(new Point3D(flangeFaceWidth, flangeFaceInnerWidth, 0), new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0));
            Line lineFPc = new Line(new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth + pipeHeight, 0), new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0));

            Line linePa = new Line(new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0), new Point3D(flangeFaceWidth + flangePipeWidth + pipeWidth, flangePipeInnerWidth, 0));
            Line linePb = new Line(new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth + pipeHeight, 0), new Point3D(flangeFaceWidth + flangePipeWidth + pipeWidth, flangePipeInnerWidth + pipeHeight, 0));


            // Line
            // Circle


            // Entity
            List<Entity> customBlockList = new List<Entity>();
            customBlockList.Add(lineFFa);
            customBlockList.Add(lineFFb);
            customBlockList.Add(lineFFc);
            customBlockList.Add(lineFFd);
            customBlockList.Add(lineFPa);
            customBlockList.Add(lineFPb);
            customBlockList.Add(lineFPc);
            customBlockList.Add(linePa);
            customBlockList.Add(linePb);


            return customBlockList.ToArray();
        }

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
    }
}
