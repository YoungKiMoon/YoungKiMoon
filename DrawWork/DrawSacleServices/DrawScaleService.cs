using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawSacleServices
{
    public class DrawScaleService
    {
        private double scaleValue;

        public DrawScaleService()
        {
            // Default Value
            scaleValue = 1;
        }
        public DrawScaleService(double selScale)
        {
            SetScale(selScale);
        }
        public void SetScale(double selScale)
        {
            scaleValue = selScale;
        }
        

        public double GetViewScale(double selScale)
        {
            // 1:1
            double tempScale = 1;
            // Vector View Model : Scale : Base Value : 0.001

            double viewBaseValue = 0.001;
            if (selScale > 0)
                tempScale = GetScale(selScale);

            return viewBaseValue * tempScale;
        }
        public double GetScale(double selScale)
        {
            return 1 / selScale;
        }

        public double GetValueByScale(double selScale,double selValue)
        {
            double viewScale = GetScale(selScale);
            return viewScale * viewScale;
        }
        public double GetOriginValueOfScale(double selScale, double selValue)
        {
            return selScale * selValue;
        }
    }


}
