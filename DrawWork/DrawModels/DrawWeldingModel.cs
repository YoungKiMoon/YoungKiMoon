using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawWeldingModel
    {
        public DrawWeldingModel()
        {
            TopWelding = false;
            BottomWelding = false;
            TopChamfer = false;
            BottomChamfer = false;

            TopWeldingSingle = true;
            BottomWeldingSingle = true;


            ChamferShort = 1;
            ChamferLong = 3;

            LeftDegree = 45;
            RightDegree = 45;

            OtherDistance = 3.2;

            MidDepth = 1.6;
            LeftDepth = 0;
            RightDepth = 0;

            LeftWeldingArc = true;
            RightWeldingArc = true;

            // welding arc = false = flat(Grinder)
        }

        public bool TopWelding{ get; set; }
        public bool BottomWelding { get; set; }

        public bool TopChamfer { get; set; }
        public bool BottomChamfer { get; set; }


        public bool TopWeldingSingle { get; set; }
        public bool BottomWeldingSingle { get; set; }

        public double ChamferShort { get; set; }
        public double ChamferLong { get; set; }

        public double LeftDegree { get; set; }
        public double RightDegree { get; set; }

        public double OtherDistance { get; set; }

        public double MidDepth { get; set; }
        public double LeftDepth { get; set; }
        public double RightDepth { get; set; }

        public bool LeftWeldingArc { get; set; }
        public bool RightWeldingArc { get; set; }
    }
}
