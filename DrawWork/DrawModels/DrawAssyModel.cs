using devDept.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawSettingLib.Commons;

namespace DrawWork.DrawModels
{
    public class DrawAssyModel
    {
        public PAPERMAIN_TYPE mainName { get; set; }
        public PAPERSUB_TYPE subName { get; set; }

        public Point3D refPoint { get; set; }

        public double scaleValue { get; set; }

        public DrawAssyModel()
        {
            mainName = PAPERMAIN_TYPE.NotSet;
            subName = PAPERSUB_TYPE.NotSet;
            refPoint = null;
            scaleValue = 1;
        }
    }
}
