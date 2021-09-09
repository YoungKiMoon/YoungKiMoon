using devDept.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawCuttingAreaModel
    {
        public Point3D refPoint { get; set; }

        public Point3D centerPoint { get; set; }

        public double plateWidth { get; set; }
        public double plateLength { get; set; }
        public string displayName{get; set;}
        public DrawCuttingAreaModel()
        {
            refPoint = new Point3D();
            centerPoint = new Point3D();
            plateWidth = 0;
            plateLength = 0;
            displayName = "";
        }
    }
}
