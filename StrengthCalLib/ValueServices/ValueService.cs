using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StrengthCalLib.ValueServices
{
    public class ValueService
    {
        public ValueService()
        {

        }
        public bool CheckIntValue(string selValue)
        {
            try
            {
                int intValue = 0;
                if (int.TryParse(selValue, out intValue))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }


        }
        public int GetIntValue(string selValue)
        {
            // Default Value
            int intValue = 0;

            if (!int.TryParse(selValue, out intValue))
                intValue = 0;
            return intValue;
        }
        public double GetDoubleValue(string selValue)
        {
            // Default Value
            double doubleValue = 0;

            if (!double.TryParse(selValue, out doubleValue))
                doubleValue = 0;
            return doubleValue;
        }

        public double IntRound(double Value, int Digit)
        {
            double Temp = Math.Pow(10.0, Digit);
            return Math.Round(Value * Temp) / Temp;
        }
        public double Evaluate(string expression)
        {
            try
            {
                System.Data.DataTable table = new System.Data.DataTable();
                return Convert.ToDouble(table.Compute(expression, String.Empty));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //MessageBox.Show(expression, "계산 오류");
                return 0;
            }
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

        // 공백 제거
        public string GetAllTrim(string selStr)
        {
            return selStr.Replace(" ", "");
        }



        public string GetOrdinalNumber(int selNumber)
        {
            return GetOrdinalNumber(Convert.ToDouble(selNumber));
        }
        public string GetOrdinalNumber(double selNumber)
        {
            string returnValue = "";
            if (selNumber == 1)
                returnValue = "ST";
            else if (selNumber == 2)
                returnValue = "ND";
            else if (selNumber == 3)
                returnValue = "RD";
            else
                returnValue = "TH";


            return selNumber + returnValue;
        }

        public double GetRadianByArcLength(double selLength, double selRadius)
        {
            return selLength / selRadius;
        }

        public double GetCirclePerimeter(double selRadius)
        {
            return Math.PI * selRadius * 2;
        }


        #region Development : 정방향

        // Length
        private double GetLengthOfOneDegree(double selCircum)
        {
            return selCircum / 360;
        }
        public double GetLengthOfDegree(double selDegree, double selCircum)
        {
            return selDegree * GetLengthOfOneDegree(selCircum);
        }



        public double GetShiftDegree(double startDegree, double selDegree)
        {
            return (startDegree + selDegree) % 360;
        }
        public double GetLengthOfDegreeByStartDegree(double startDegree, double selDegree, double selCircum)
        {
            return GetLengthOfDegree(GetShiftDegree(startDegree, selDegree), selCircum);
        }

        public double GetShiftReverseDegree(double startDegree, double selDegree)
        {
            double realDegree = selDegree % 360;
            double calDegree = startDegree - realDegree;
            if (calDegree < 0)
                calDegree = 360 + calDegree;
            return calDegree;
        }
        public double GetLengthOfReverseDegreeByStartDegree(double startDegree, double selDegree, double selCircum)
        {
            return GetLengthOfDegree(GetShiftReverseDegree(startDegree, selDegree), selCircum);
        }


        // Degree
        public double GetDegreeOfLength(double selCount, double selCircum)
        {
            return selCircum / selCount / 360;
        }

        #endregion

        // Round Up
        public double GetRoundUp(double value, int decimalpoint)
        {
            // 올림
            //var value = 2.521;
            //var decimalpoint = 0;
            double result = Math.Round(value, decimalpoint);
            if (result < value)
            {
                result += Math.Pow(10, -decimalpoint);
            }
            return result;
        }

        // Round Up : Int
        public double GetIntRoundUp(double value, int digit)
        {
            double x = Math.Pow(10, digit);
            return Math.Ceiling(value / (x)) * x;
        }

        // Round Down
        public double GetRoundDown(double value, int decimalpoint)
        {
            // 올림
            //var value = 2.521;
            //var decimalpoint = 0;
            
            //  내림
            double result = Math.Floor(value * Math.Pow(10, decimalpoint)) / Math.Pow(10, decimalpoint);

            return result;
        }
        
    }
}
