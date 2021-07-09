using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSample.Models
{
    public class CombModel
    {
        public List<CombToothModel> Teeth { get; set; }
        public CombModel()
        {
            Teeth = new List<CombToothModel>();
        }
    }
    public class CombToothModel
    {
        public double XValue { get; set; }
        public double startValue { get; set; }
        public double endValue { get; set; }



        public CombToothModel()
        {
            XValue = 0;
            startValue = 0;
            endValue = 0;
        }
        public CombToothModel(double selXValue, double selStartValue, double selEndValue)
        {
            XValue = selXValue;
            startValue = selStartValue;
            endValue = selEndValue;
        }
    }

}
