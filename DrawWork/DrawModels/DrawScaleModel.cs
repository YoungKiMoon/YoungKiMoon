using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawScaleModel
    {
        public DrawScaleModel()
        {
            Value = 1;
        }
        public DrawScaleModel(double selValue)
        {
            Value = selValue;
        }
        public double Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        private double _Value;
    }
}
