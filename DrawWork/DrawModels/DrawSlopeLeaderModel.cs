using devDept.Geometry;
using DrawWork.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawSlopeLeaderModel
    {
        public ORIENTATION_TYPE position { get; set; }

        public double leaderDegree { get; set; }

        public double leaderAngleRadian { get; set; }


        public double leaderLength { get; set; }

        public double heightOneSize { get; set; }
       
        public double textGap{ get; set; }

        public double heightValue { get; set; }
        public double widthValue { get; set; }

        public double textHeight { get; set; }

        public Point3D insertPoint { get; set; }

        public DrawSlopeLeaderModel()
        {
            position = ORIENTATION_TYPE.BOTTOMLEFT;
            
            leaderDegree = 0;
            leaderAngleRadian = 0;


            leaderLength = 5;

            heightOneSize = 2;
            textGap = 1;

            heightValue = 1;
            widthValue = 3;


            textHeight = 2.5;

            insertPoint = null;
        }
    }
}
