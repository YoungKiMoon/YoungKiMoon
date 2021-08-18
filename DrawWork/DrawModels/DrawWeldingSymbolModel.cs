using DrawWork.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawWeldingSymbolModel
    {
        // Arrow
        public bool arrowHeadVisible { get; set; }
        public double arrowHeadWidth { get; set; }
        public double arrowHeadHeight { get; set; }

        // Leader Line
        public bool leaderLineVisible { get; set; }
        public double leaderLineLength { get; set; }

        // All Round Weld
        public bool allRoundWeldVisible { get; set; }
        public double allRoundWeldRadius { get; set; }

        // Reference Line
        public double referenceLength { get; set; }

        // Field Weld
        public bool fieldWeldVisible { get; set; }
        public double fieldLength { get; set; }
        public double fieldFlagWidth { get; set; }
        public double fieldFlagHeight { get; set; }

        // Weld Size
        public string weldSize1 { get; set; }
        public string weldSize2 { get; set; }

        // Weld Info
        public string weldLength1 { get; set; }
        public string weldQuantity1 { get; set; }
        public string weldPitch1 { get; set; }

        public string weldLength2 { get; set; }
        public string weldQuantity2 { get; set; }
        public string weldPitch2 { get; set; }

        // Weld Tail
        public bool tailVisible { get; set; }
        public string specification1 { get; set; }
        public string specification2 { get; set; }
        public string specification3 { get; set; }

        // Weld Type
        public WeldSymbol_Type weldType { get; set; }
        public WeldSymbolDetail_Type weldDetailType { get; set; }

        // Weld Face
        public WeldFace_Type weldFace { get; set; }

        // Weld Angle
        public string weldAngle1 { get; set; }
        public string weldAngle2 { get; set; }

        // Weld Root
        public string weldRoot1 { get; set; }
        public string weldRoot2 { get; set; }

        // Machining
        public bool machiningVisible { get; set; }
        public string machiningStr { get; set; }

        public DrawWeldingSymbolModel()
        {
            // Arrow Head
            arrowHeadVisible = true;
            arrowHeadWidth = 2.5;
            arrowHeadHeight = 0.8;

            // Leader Line
            leaderLineVisible = true;
            leaderLineLength = 10;//12

            // All Round Weld
            allRoundWeldVisible = true;
            allRoundWeldRadius = 1.25;

            // Reference Line
            referenceLength = 14;//30

            // Field Weld
            fieldWeldVisible = true;
            fieldLength = 8;
            fieldFlagHeight = 2;
            fieldFlagWidth = 4;

            // Weld Size
            weldSize1 = "";
            weldSize2 = "";

            // Weld Info
            weldLength1 = "";
            weldQuantity1 = "";
            weldPitch1 = "";
            weldLength2 = "";
            weldQuantity2 = "";
            weldPitch2 = "";

            // Weld Tail
            tailVisible = true;
            specification1 = "";
            specification2 = "";
            specification3 = "";

            // Weld Type
            weldType = WeldSymbol_Type.NotSet;
            weldDetailType = WeldSymbolDetail_Type.NotSet;

            // Weld Face
            weldFace = WeldFace_Type.NotSet;

            // Weld Angle
            weldAngle1 = "";
            weldAngle2 = "";

            // Weld Root
            weldRoot1 = "";
            weldRoot2 = "";
            // Machining
            machiningVisible = false;
            machiningStr = "G";
        }

    }
}
