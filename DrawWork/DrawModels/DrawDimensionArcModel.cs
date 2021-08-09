using devDept.Geometry;
using DrawWork.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawDimensionArcModel
    {
        // Position
        public POSITION_TYPE position { get; set; }


        // Point
        public Point3D pointLeft { get; set; }
        public Point3D pointRight { get; set; }

        // dim Height

        public double dimHeight { get; set; }

        public double middleValue { get; set; }

        public string textUserInuput { get; set; }


        // Arc Degree
        public double arcDegree { get; set; }
        public double ext1Degree { get; set; }
        public double ext2Degree { get; set; }


        // Symbol : Arrow 
        public double arrowHeadWidth { get; set; }
        public double arrowHeadHeight { get; set; }
        public double arrowExtLength { get; set; }
        public bool arrowLeftHeadVisible { get; set; }
        public bool arrowRightHeadVisible { get; set; }


        public DimHead_Type arrowLeftSymbol { get; set; }
        public DimHead_Type arrowRightSymbol { get; set; }

        // Ext
        public bool extLineLeftVisible { get; set; }
        public bool extLineRightVisible { get; set; }

        public double extLineLength1 { get; set; }
        public double extLineLength2 { get; set; }
        public double extLinePointGap1 { get; set; }
        public double extLinePointGap2 { get; set; }

        public bool extVisible { get; set; }

        // Text
        public bool textSizeVisible { get; set; }
        public double textHeight { get; set; }
        public double textGap { get; set; }
        public double textSideGap { get; set; }
        public double textMiddleValue { get; set; }


        // scale
        public double scaleValue { get; set; }
        
        public DrawDimensionArcModel()
        {
            position = POSITION_TYPE.TOP;

            dimHeight = 0;

            arrowHeadWidth = 2.5;
            arrowHeadHeight = 0.8;
            arrowExtLength = 2.5;


            extLineLeftVisible = true;
            extLineRightVisible = true;

            extLineLength1 = 1;
            extLineLength2 = 1;

            extLinePointGap1 = 1;
            extLinePointGap2 = 1;


            arrowLeftHeadVisible = true;
            arrowRightHeadVisible = true;


            textHeight = 2.5;
            textGap = 1;
            textSideGap = 2;
            textMiddleValue = 0;

            arrowLeftSymbol = DimHead_Type.Arrow;
            arrowRightSymbol = DimHead_Type.Arrow;

            textSizeVisible = false;

            textHeight = 2.5;
            textGap = 1;
            textSideGap = 2;
            textMiddleValue = 0;


            extVisible = true;
        }
    }
}
