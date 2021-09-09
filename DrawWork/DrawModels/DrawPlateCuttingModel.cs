using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawPlateCuttingModel
    {

        public double MaxLength { get; set; }
        public double MinLength { get; set; }
        public double MinMaxSumLength { get; set; }
        public double Width { get; set; }
        public double LengthSpacing { get; set; }

        public double Slope { get; set; }
        public double VMinusSlope { get; set; }
        public double StringLength { get; set; }
        public double LengthBetweenStringAndArc { get; set; }
        public double XLengthOfArcTangent { get; set; }


        public bool LeftPlate { get; set; }
        public double InsertPointXLength { get; set; }
        public DrawPlateCuttingModel()
        {
            MaxLength = 0;
            MinLength = 0;
            Width = 0;
            LengthSpacing = 0;
            Slope = 0;
            VMinusSlope = 0;
            StringLength = 0;
            LengthBetweenStringAndArc=0;
            XLengthOfArcTangent = 0;

            LeftPlate = true;
            InsertPointXLength = 0;
        }
    }
}
