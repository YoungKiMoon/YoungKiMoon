using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class CDPoint : ICloneable
    {
        public CDPoint()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }
        public CDPoint(CDPoint newModel)
        {
            X = newModel.X;
            Y = newModel.Y;
            Z = newModel.Z;
        }
        public CDPoint(double selX, double selY, double selZ)
        {
            X = selX;
            Y = selY;
            Z = selZ;
        }
        public object Clone()
        {
            //this.MemberwiseClone();
            CDPoint newModel = new CDPoint();
            newModel.X = X;
            newModel.Y = Y;
            newModel.Z = Z;

            return newModel;
        }

        private double _X;
        public double X
        {
            get { return _X; }
            set { _X = value; }
        }
        private double _Y;
        public double Y
        {
            get { return _Y; }
            set { _Y = value; }
        }
        private double _Z;
        public double Z
        {
            get { return _Z; }
            set { _Z = value; }
        }
    }
}
