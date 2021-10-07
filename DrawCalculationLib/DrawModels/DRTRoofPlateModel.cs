using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawCalculationLib.DrawModels
{
    public class DRTRoofPlateModel
    {
        public DRTRoofPlateModel()
        {
            plateDivLine = 0;
            oneVerticalLength = 0;
            PlateLineList = new List<DRTRoofPlateLineModel>();
        }

        public double plateDivLine { get; set; }

        public double oneVerticalLength { get; set; }

        public List<DRTRoofPlateLineModel> PlateLineList { get; set; }
    }
}
