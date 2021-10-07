using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawCalculationLib.DrawModels
{
    public   class DRTLayerModel
    {
        public DRTLayerModel()
        {
            startAngle = 0;

            Count = 0;
            Angle = 0;

            ArcLength = 0;
            ArcLengthOverlap = 0;
            StringLength = 0;


            LayerRoofRadius = 0;

            BeforeLayerAngleFromCenter = 0;
            BeforeHalfArcLengthFromCenter = 0;
            BeforeHalfStringLengthFromCenter = 0;



            PlateList = new List<DRTRoofPlateModel>();
        }


        public double startAngle { get; set; }

        public double Count { get; set; }
        public double Angle { get; set; }

        public double ArcLength { get; set; }
        public double ArcLengthOverlap { get; set; }

        public double StringLength { get; set; }


        public double LayerRoofRadius { get; set; }

        public double BeforeLayerAngleFromCenter { get; set; }
        public double BeforeHalfArcLengthFromCenter { get; set; }
        public double BeforeHalfStringLengthFromCenter { get; set; }

        public double CurrentHalfStringLengthFromCenter { get; set; }

        public List<DRTRoofPlateModel> PlateList { get; set; }
    }
}
