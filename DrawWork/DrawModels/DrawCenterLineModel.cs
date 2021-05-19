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
            startEx = false;
            endEx = false;
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

        public bool startEx
        {
            get { return _startEx; }
            set { _startEx = value; }
        }
        private bool _startEx;
        public bool endEx
        {
            get { return _endEx; }
            set { _endEx = value; }
        }
        private bool _endEx;
        public double scaleValue
        {
            get { return _scaleValue; }
            set { _scaleValue = value; }
        }
        private double _scaleValue;
    }
}
