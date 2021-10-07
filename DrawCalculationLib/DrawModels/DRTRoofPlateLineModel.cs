using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawCalculationLib.DrawModels
{
    public class DRTRoofPlateLineModel
    {
        public DRTRoofPlateLineModel()
        {
            Radius = 0;
            ArcAngle = 0;
            StringLength = 0;

            InnerOverlap = 0;
            OuterOverlap = 0;

            OneDivLength = 0;
        }

        public double Radius { get; set; }
        public double ArcAngle { get; set; }
        public double StringLength { get; set; }

        public double InnerOverlap { get; set; }
        public double OuterOverlap { get; set; }

        public double OneDivLength { get; set; }
    }
}
