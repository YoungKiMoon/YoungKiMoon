using DrawCalculationLib.DrawModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawPlateCuttingSectorModel
    {
        public DrawPlateCuttingSectorModel()
        {

            SectorLength = 0;

            RectLength = 0;
            RectWidth = 0;

            SectorDivCount = 0;
            SectorDivOneLength = 0;

            DivLineList = new List<DRTRoofPlateLineModel>();
        }
        public double InnerRadius { get; set; }
        public double OuterRadius { get; set; }

        public double InnerAngle { get; set; }
        public double OuterAngle { get; set; }

        public double SectorLength { get; set; }
        public double RectWidth{ get; set; }
        public double RectLength { get; set; }

        public double SectorDivCount { get; set; }
        public double SectorDivOneLength { get; set; }

        public List<DRTRoofPlateLineModel> DivLineList { get; set; }

    }
}
