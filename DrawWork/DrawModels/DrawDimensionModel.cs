using DrawWork.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawDimensionModel
    {
        // Position
        public POSITION_TYPE position { get; set; }

        // Dim Height
        public double dimHeight { get; set; }

        // Symbol : Arrow 
        public double arrowHeadWidth { get; set; }
        public double arrowHeadHeight { get; set; }
        public double arrowExtLength { get; set; }
        public bool arrowLeftHeadVisible { get; set; }
        public bool arrowRightHeadVisible { get; set; }


        public DimHead_Type arrowLeftSymbol { get; set; }
        public DimHead_Type arrowRightSymbol { get; set; }

        public bool arrowLeftHeadOut { get; set; }
        public bool arrowRightHeadOut { get; set; }

        // Symbol : Circle
        public double circleRadius { get; set; }


        // Ext Line

        public bool extLineLeftVisible { get; set; }
        public bool extLineRightVisible { get; set; }

        public double extLineLength1 { get; set; }
        public double extLineLength2 { get; set; }
        public double extLinePointGap1 { get; set; }
        public double extLinePointGap2 { get; set; }

        // Circle : Number
        public string leftBMNumber { get; set; }
        public string rightBMNumber { get; set; }

        // Text
        public bool textSizeVisible { get; set; }

        public int textRoundNumber { get; set; }
        public double textHeight { get; set; }
        public double textGap { get; set; }
        public double textSideGap { get; set; }
        public double textMiddleValue { get; set; }


        public string textUpper { get; set; }
        public POSITION_TYPE textUpperPosition { get; set; }

        public string textLower { get; set; }
        public POSITION_TYPE textLowerPosition { get; set; }



        // Scale
        public double scaleValue { get; set; }


        
        public DrawDimensionModel()
        {
            SetDefaultValue();
        }

        private void SetDefaultValue()
        {
            position = POSITION_TYPE.TOP;

            dimHeight = 0;

            arrowHeadWidth = 2.5;
            arrowHeadHeight = 0.8;
            arrowExtLength = 2.5;

            circleRadius = 0.5;

            extLineLeftVisible = true;
            extLineRightVisible = true;

            extLineLength1 = 1;
            extLineLength2 = 1;

            extLinePointGap1 = 1;
            extLinePointGap2 = 1;

            leftBMNumber = "";
            rightBMNumber = "";
            
            textSizeVisible = false;

            textRoundNumber = 1;

            textHeight = 2.5;
            textGap = 1;
            textSideGap = 2;
            textMiddleValue = 0;

            arrowLeftHeadVisible = true;
            arrowRightHeadVisible = true;

            arrowLeftSymbol = DimHead_Type.Arrow;
            arrowRightSymbol = DimHead_Type.Arrow;

            arrowLeftHeadOut = false;
            arrowRightHeadOut = false;

            textUpper = "";
            textUpperPosition = POSITION_TYPE.CENTER;

            textLower= "";
            textLowerPosition = POSITION_TYPE.CENTER;

            scaleValue = 1;
        }
    }
}
