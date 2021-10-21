using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawCalculationLib.FunctionServices
{
    public class GeometryService
    {
        public GeometryService()
        {

        }


        // 원의 둘레 : 지름
        public double GetCircleCircumByDiameter(double diameter)
        {
            double returnValue = 0;
            if (diameter > 0)
                returnValue = GetCircleCircumByRadius(diameter / 2);
            return returnValue;
        }
        // 원의 둘레 : 반지름
        public double GetCircleCircumByRadius(double radius)
        {
            double returnValue = 0;
            if (radius > 0)
                returnValue = 2 * radius * Math.PI;
            return returnValue;
        }

        // 부채꼴 각도 : 호의 길이
        public double GetArcAngleByArcLength(double arcRadius, double arcLength)
        {
            double returnValue = 0;
            if (arcRadius > 0)
            {
                double circleLength = GetCircleCircumByRadius(arcRadius);
                returnValue = arcLength / circleLength * 360;
            }
            // Degree
            return returnValue;
        }

        // 부채꼴 현의 길이 : 부채꼴 각도
        public double GetStringLengthByArcAngle(double arcRadius, double angleDegree)
        {
            double returnValue = 2 * arcRadius * Math.Sin(DegreeToRadian(angleDegree / 2));

            return returnValue;
        }

        // 부채꼴 호의 길이 : 현의 길이
        public double GetArcLengthByStringLength(double arcRadius, double stringLength)
        {
            double returnValue = 2 * arcRadius * Math.Asin(stringLength / (2 * arcRadius));

            return returnValue;
        }

        public double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180;
        }
        public double RadianToDegree(double angle)
        {
            return 180 * angle / Math.PI;
        }




        public double GetDoubleValue(string selValue)
        {
            // Default Value
            double doubleValue = 0;

            if (!double.TryParse(selValue, out doubleValue))
                doubleValue = 0;
            return doubleValue;
        }



        #region Degree Of Slope
        public double GetDegreeOfSlope(string selSlope)
        {
            double selHeight;
            double selWidth;
            if (selSlope.Contains("/"))
            {
                string[] selArray = selSlope.Split(new char[] { '/' });
                selHeight = GetDoubleValue(selArray[0]);
                selWidth = GetDoubleValue(selArray[1]);
            }
            else
            {
                selHeight = 1;
                selWidth = GetDoubleValue(selSlope);
            }

            if (selWidth == 0)
                selHeight = 0;
            return GetDegreeOfSlope(selHeight, selWidth);
        }

        public double GetWidthOfSlope(string selSlope)
        {
            double selHeight = 0;
            double selWidth = 0;
            if (selSlope.Contains("/"))
            {
                string[] selArray = selSlope.Split(new char[] { '/' });
                selHeight = GetDoubleValue(selArray[0]);
                selWidth = GetDoubleValue(selArray[1]);
            }
            else
            {
                selHeight = 1;
                selWidth = GetDoubleValue(selSlope);
            }
            return selWidth;
        }
        public double GetDegreeOfSlope(double selHeight, double selWidth)
        {
            return Math.Atan2(selHeight, selWidth);
        }
        #endregion

        #region Trigonometric Function
        public double GetOppositeByWidth(string selSlope, string selWidth)
        {
            return GetOppositeByWidth(GetDegreeOfSlope(selSlope), GetDoubleValue(selWidth));
        }
        public double GetOppositeByWidth(string selSlope, double selWidth)
        {
            return GetOppositeByWidth(GetDegreeOfSlope(selSlope), selWidth);
        }
        public double GetOppositeByWidth(double selDegree, double selWidth)
        {
            // Tan = O/A
            // O = Tan * A
            return Math.Tan(selDegree) * selWidth;
        }

        public double GetOppositeByHypotenuse(string selSlope, string selHypotenuse)
        {
            return GetOppositeByHypotenuse(GetDegreeOfSlope(selSlope), GetDoubleValue(selHypotenuse));
        }
        public double GetOppositeByHypotenuse(string selSlope, double selHypotenuse)
        {
            return GetOppositeByHypotenuse(GetDegreeOfSlope(selSlope), selHypotenuse);
        }
        public double GetOppositeByHypotenuse(double selDegree, double selHypotenuse)
        {
            // Tan = O/A
            // O = Tan * A
            return Math.Sin(selDegree) * selHypotenuse;
        }

        public double GetOppositeByAdjacent(double selDegree, double selAdjacent)
        {
            return selAdjacent * Math.Tan(selDegree);
        }

        public double GetAdjacentByHeight(string selSlope, string selHeight)
        {
            return GetAdjacentByHeight(GetDegreeOfSlope(selSlope), GetDoubleValue(selHeight));
        }
        public double GetAdjacentByHeight(string selSlope, double selHeight)
        {
            return GetAdjacentByHeight(GetDegreeOfSlope(selSlope), selHeight);
        }
        public double GetAdjacentByHeight(double selDegree, double selHeight)
        {
            // Tan = O/A
            // A = O/Tan
            return selHeight / Math.Tan(selDegree);
        }


        public double GetAdjacentByHypotenuse(string selSlope, string selHypotenuse)
        {
            return GetAdjacentByHypotenuse(GetDegreeOfSlope(selSlope), GetDoubleValue(selHypotenuse));
        }
        public double GetAdjacentByHypotenuse(string selSlope, double selHypotenuse)
        {
            return GetAdjacentByHypotenuse(GetDegreeOfSlope(selSlope), selHypotenuse);
        }
        public double GetAdjacentByHypotenuse(double selDegree, double selHypotenuse)
        {
            // Tan = O/A
            // A = O/Tan
            return selHypotenuse * Math.Cos(selDegree);
        }


        public double GetHypotenuseByWidth(string selSlope, string selWidth)
        {
            return GetHypotenuseByWidth(GetDegreeOfSlope(selSlope), GetDoubleValue(selWidth));
        }
        public double GetHypotenuseByWidth(string selSlope, double selWidth)
        {
            return GetHypotenuseByWidth(GetDegreeOfSlope(selSlope), selWidth);
        }
        public double GetHypotenuseByWidth(double selDegree, double selWidth)
        {
            // Cos = A/H
            // H = A/COS
            return selWidth / Math.Cos(selDegree);
        }




        #endregion

    }
}
