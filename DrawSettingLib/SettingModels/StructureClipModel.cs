using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingModels
{
    public class StructureClipModel
    {
        public StructureClipModel()
        {
            InOut = 0;  // 0=In , 1=Out

            Number = 0;
            AngleFromCenter = 0;



            PointLengthFormColumn = 0;
            HorizontalLengthFromCenter = 0;

            ClipSideAngle = 0;
            ColumnSideAngle = 0;
            CenterSideAngle = 0;

            GirderClipAngle = 0;
            GirderClipAngleABS = 0;

            ClipHeight = 0;
            ClipWidth = 0;
        }

        public int InOut { get; set; }

        public double Number { get; set; }
        public double AngleFromCenter { get; set; }



        public double PointLengthFormColumn { get; set; }

        public double HorizontalLengthFromCenter { get; set; }



        public double ClipSideAngle { get; set; }

        public double ColumnSideAngle { get; set; }

        public double CenterSideAngle { get; set; }


        public double GirderClipAngle { get; set; }

        public double GirderClipAngleABS { get; set; }


        public double ClipHeight { get; set; }
        public double ClipWidth { get; set; }

    }
}
