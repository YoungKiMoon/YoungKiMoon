using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawServices
{
    public class DrawPublicFunctionService
    {
        public DrawPublicFunctionService()
        {

        }

        public double GetBottomFocuntionOD(string selAnnular)
        {
            double outsideProjection = 0;
            double weldingLeg = 9;
            if (selAnnular == "Yes")
                outsideProjection = 60 + weldingLeg;
            else
                outsideProjection = 30 + weldingLeg;

            return outsideProjection;
        }

    }
}
