using DrawWork.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawBMLeaderModel
    {


        public POSITION_TYPE position { get; set; }
        public DrawBMNumberModel bmCircle { get; set; }

        public string bmNumber { get; set; }

        public double arrowHeadWidth { get; set; }
        public double arrowHeadHeight { get; set; }

        public double textHeight { get; set; }
        public string upperText { get; set; }
        public string lowerText { get; set; }

        public double multiLineHeight { get; set; }

        public string upperSecondText { get; set; }

        public POSITION_TYPE textAlign { get; set; }

        public double leaderLength { get; set; }

        public double textSideGap { get; set; }

        public double textGap { get; set; }

        public double leaderPointLength { get; set; }
        public double leaderPointRadian { get; set; }

        public bool arrowHeadVisible { get; set; }
        public DrawBMLeaderModel()
        {

            position = POSITION_TYPE.LEFT;

            bmCircle = new DrawBMNumberModel();
            arrowHeadWidth = 2.5;
            arrowHeadHeight = 0.8;
            arrowHeadVisible = true;

            textHeight = 2.5;
            upperText = "";
            lowerText = "";

            multiLineHeight = 7;
            upperSecondText = "";

            textAlign = POSITION_TYPE.CENTER;

            bmNumber = "";

            leaderLength = 2.5;

            textGap = 1;
            textSideGap = 1;

            leaderPointLength = 0;
            leaderPointRadian = 0;
        }
    }
}
