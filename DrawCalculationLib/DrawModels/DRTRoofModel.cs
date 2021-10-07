using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawCalculationLib.DrawModels
{
    public class DRTRoofModel
    {
        public DRTRoofModel()
        {
            DRTOverlap = 0;

            plateLength = 0;
            plateWidth = 0;

            tankID = 0;
            DRTCurvature = 0;
            DRTRoofDiameter = 0;
            DRTRoofRadius = 0;

            DRTRoofStringLength = 0;

            platePieceOverlap = 0;
            centerCircleHalfArcLength = 0;
            centerCircleArcLength = 0;
            centerCircleAngle = 0;
            centerCircleStringLength = 0;

            centerStartAngle = 0;

            DRTRoofArcLength = 0;
            DRTRoofArcAngle = 0;
            DRTArrngeArcLength = 0;


            layerCount = 0;

            layerList = new List<DRTLayerModel>();
            DRTRoofPlateList = new List<DRTRoofPlateModel>();
        }


        // Basic Info
        public double DRTOverlap { get; set; }

        public double plateLength { get; set; }
        public double plateWidth { get; set; }

        public double tankID { get; set; }
        public double DRTCurvature { get; set; }
        public double DRTRoofDiameter { get; set; }
        public double DRTRoofRadius { get; set; }

        public double DRTRoofStringLength { get; set; }



        // Center Circle
        public double platePieceOverlap { get; set; }
        public double centerCircleHalfArcLength { get; set; }
        public double centerCircleArcLength { get; set; }
        public double centerCircleAngle { get; set; }
        public double centerCircleStringLength { get; set; }


        public double centerStartAngle { get; set; }



        // Arrnage Arc Length
        public double DRTRoofArcLength { get; set; }
        public double DRTRoofArcAngle { get; set; }
        public double DRTArrngeArcLength { get; set; }


        // Lyaer
        public double layerCount { get; set; }
        public List<DRTLayerModel> layerList { get; set; }
        public List<DRTRoofPlateModel> DRTRoofPlateList { get; set; }


    }
}
