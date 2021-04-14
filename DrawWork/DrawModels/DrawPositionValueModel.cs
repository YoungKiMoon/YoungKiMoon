using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawPositionValueModel
    {
        public DrawPositionValueModel()
        {
            Left = 0;
            Right = 0;
            Top = 0;
            Bottom = 0;

        }
        public double Left
        {
            get { return _Left; }
            set { _Left = value; }
        }
        private double _Left;

        public double Right
        {
            get { return _Right; }
            set { _Right = value; }
        }
        private double _Right;

        public double Top
        {
            get { return _Top; }
            set { _Top = value; }
        }
        private double _Top;

        public double Bottom
        {
            get { return _Bottom; }
            set { _Bottom = value; }
        }
        private double _Bottom;

    }
}
