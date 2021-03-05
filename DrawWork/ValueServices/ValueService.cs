using System;
using System.Collections.Generic;
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
                MessageBox.Show(expression ,"계산 오류" );
                return 0;
            }


        }
    }
}
