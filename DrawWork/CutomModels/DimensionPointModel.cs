using DrawWork.DrawModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.CutomModels
{
    public class DimensionPointModel
    {
        public DimensionPointModel()
        {
            leftPoint = new CDPoint();
            rightPoint = new CDPoint();
            dimHeight = 0;
            Text = "";
            Position = "";
            leftArrowVisible = true;
            rightArrowVisible = true;
            extVisible = true;
            middleValue = 0;
        }

        public CDPoint leftPoint
        {
            get { return _leftPoint; }
            set { _leftPoint = value; }
        }
        private CDPoint _leftPoint;

        public CDPoint rightPoint
        {
            get { return _rightPoint; }
            set { _rightPoint = value; }
        }
        private CDPoint _rightPoint;

        public double dimHeight
        {
            get { return _dimHeight; }
            set { _dimHeight = value; }
        }
        private double _dimHeight;

        public string Position
        {
            get { return _Position; }
            set { _Position = value; }
        }
        private string _Position;

        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }
        private string _Text;

        public bool leftArrowVisible
        {
            get { return _leftArrowVisible; }
            set { _leftArrowVisible = value; }
        }
        private bool _leftArrowVisible;

        public bool rightArrowVisible
        {
            get { return _rightArrowVisible; }
            set { _rightArrowVisible = value; }
        }
        private bool _rightArrowVisible;

        public bool extVisible
        {
            get { return _extVisible; }
            set { _extVisible = value; }
        }
        private bool _extVisible;

        public double middleValue
        {
            get { return _middleValue; }
            set { _middleValue = value; }
        }
        private double _middleValue;
    }
}
