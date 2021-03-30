using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DrawWork.ValueServices
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
            catch(Exception ex)
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
        public double Evaluate(string expression)
        {
            try
            {
                System.Data.DataTable table = new System.Data.DataTable();
                return Convert.ToDouble(table.Compute(expression, String.Empty));
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessageBox.Show(expression ,"계산 오류" );
                return 0;
            }
        }
        public double GetAtanOfSlope(string selSlope)
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
            double calDegree = Math.Atan2(selHeight,selWidth);

            return calDegree;
        }

        public double GetSlopeOfHeight(string selSlope, string selWidth)
        {
            return GetSlopeOfHeight(selSlope, GetDoubleValue(selWidth));
        }
        public double GetSlopeOfHeight(string selSlope, double selWidth)
        {
            double tempDegree = GetAtanOfSlope(selSlope);
            double tempWidth = selWidth;
            return tempDegree * tempWidth;
        }
        public double GetSlopeOfWidth(string selSlope, string selHeight)
        {
            return GetSlopeOfWidth(selSlope, GetDoubleValue(selHeight));
        }
        public double GetSlopeOfWidth(string selSlope, double selHeight)
        {
            double tempDegree = GetAtanOfSlope(selSlope);
            double tempHeight = selHeight;
            return tempHeight / tempDegree;
        }
    }
}
