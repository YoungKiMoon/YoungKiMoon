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
    }
}
