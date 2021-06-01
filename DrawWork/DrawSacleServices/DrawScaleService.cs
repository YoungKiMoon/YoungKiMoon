using AssemblyLib.AssemblyModels;
using DrawWork.ValueServices;
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
        private ValueService valueService;

        public DrawScaleService()
        {
            valueService = new ValueService();
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

        public double GetAIScale(AssemblyModel assemblyData)
        {

            double returnValue = 1;
            string returnString = "1";
            string returnString2 = "1";
            double st1Value = 1;
            double st2Value = 1;
            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double tankWidth = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);

            
            {
                //Vertical
                foreach(ModelScaleModel eachScale in assemblyData.ModelScaleList)
                {
                    returnString = eachScale.VerticalScale;
                    double eachValue = valueService.GetDoubleValue(eachScale.VerticalH);
                    if (tankHeight < eachValue)
                        break;
                }

                //Horizontal
                foreach (ModelScaleModel eachScale in assemblyData.ModelScaleList)
                {
                    returnString2 = eachScale.HorizontalScale;
                    double eachValue = valueService.GetDoubleValue(eachScale.HorizontalH);
                    if (tankHeight < eachValue)
                        break;
                }
            }

            st1Value= valueService.GetDoubleValue(returnString);
            st2Value = valueService.GetDoubleValue(returnString2);
            returnValue = Math.Max(st1Value, st2Value);
            returnValue +=0;
            return returnValue;
        }
    }


}
