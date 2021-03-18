using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawDimStyle : ICloneable
    {
        public DrawDimStyle()
        {
            arrowheadSize = 2.5;
            extensionLine = 1;
            extensionLinesOffset = 1;
            textGap = 1;
        }

        public object Clone()
        {
            DrawDimStyle newModel = new DrawDimStyle();
            newModel.arrowheadSize = arrowheadSize;
            newModel.extensionLine = extensionLine;
            newModel.extensionLinesOffset = extensionLinesOffset;
            newModel.textGap = textGap;
            return newModel;
        }
        // lineColor
        // lineStyle
        // lineWeight

        // extensionLine
        // extensionLInesOffset

        // arrowheadType
        // arrowheadSize

        private double _arrowheadSize;
        public double arrowheadSize
        {
            get { return _arrowheadSize; }
            set { _arrowheadSize = value; }
        }

        private double _extensionLine;
        public double extensionLine
        {
            get { return _extensionLine; }
            set { _extensionLine = value; }
        }
        private double _extensionLinesOffset;
        public double extensionLinesOffset
        {
            get { return _extensionLinesOffset; }
            set { _extensionLinesOffset = value; }
        }

        private double _textGap;
        public double textGap
        {
            get { return _textGap; }
            set { _textGap = value; }
        }
    }
}
