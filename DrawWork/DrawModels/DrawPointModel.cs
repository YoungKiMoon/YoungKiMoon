using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawPointModel : ICloneable
    {
        public DrawPointModel()
        {
            currentPoint = new CDPoint();
            referencePoint = new CDPoint();
        }

        public object Clone()
        {
            DrawPointModel newModel = new DrawPointModel();
            newModel.currentPoint = currentPoint.Clone() as CDPoint;
            newModel.referencePoint = referencePoint.Clone() as CDPoint;
            return newModel;
        }

        private CDPoint _currentPoint;
        public CDPoint currentPoint
        {
            get { return _currentPoint; }
            set { _currentPoint = value; }
        }

        private CDPoint _referencePoint;
        public CDPoint referencePoint
        {
            get { return _referencePoint; }
            set { _referencePoint = value; }
        }

        
    }
}
