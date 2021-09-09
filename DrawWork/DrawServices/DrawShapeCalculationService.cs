using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawServices
{
    public class DrawShapeCalculationService
    {
        public DrawShapeCalculationService()
        {

        }

        public double GetCompRingKOD(double selID, double selThk, double beforeThk, double selWidth)
        {

            double selThkAdj = GetCompRingKTypeThkAdj(selThk, beforeThk);
            double returnValue = selID+ selThkAdj + selWidth;
            return returnValue;
        }

        public double GetCompRingKTypeThkAdj(double selThk, double beforeThk)
        {
            double selThkAdj = selThk;
            if (selThk > beforeThk)
                selThkAdj = selThk - ((selThk - beforeThk) / 2);

            return selThkAdj;
        }
    }
}
