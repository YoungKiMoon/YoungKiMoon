using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawCenterLineModel
    {
        public DrawCenterLineModel()
        {
            centerLine = true;
            exLength = 0.5;
            zeroEx = false;
            oneEx = false;
            twoEx = false;
            scaleValue = 1;
        }

        public bool centerLine
        {
            get { return _centerLine; }
            set { _centerLine = value; }
        }
        private bool _centerLine;


        public double exLength
        {
            get { return _exLength; }
            set { _exLength = value; }
        }
        private double _exLength;

        public bool zeroEx
        {
            get { return _zeroEx; }
            set { _zeroEx = value; }
        }
        private bool _zeroEx;
        public bool oneEx
        {
            get { return _oneEx; }
            set { _oneEx = value; }
        }
        private bool _oneEx;
        public bool twoEx
        {
            get { return _twoEx; }
            set { _twoEx = value; }
        }
        private bool _twoEx;
        public double scaleValue
        {
            get { return _scaleValue; }
            set { _scaleValue = value; }
        }
        private double _scaleValue;
    }
}
