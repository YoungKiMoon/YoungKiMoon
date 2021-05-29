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
            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double tankWidth = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);

            if (tankHeight > tankWidth)
            {
                //Vertical
                foreach(ModelScaleModel eachScale in assemblyData.ModelScaleList)
                {
                    returnString = eachScale.VerticalScale;
                    double eachValue = valueService.GetDoubleValue(eachScale.VerticalH);
                    if (tankHeight < eachValue)
                        break;
                }
            }
            else
            {
                //Horizontal
                foreach (ModelScaleModel eachScale in assemblyData.ModelScaleList)
                {
                    returnString = eachScale.HorizontalScale;
                    double eachValue = valueService.GetDoubleValue(eachScale.HorizontalID);
                    if (tankWidth < eachValue)
                        break;
                }
            }
            returnValue = valueService.GetDoubleValue(returnString);
            returnValue +=20;
            return returnValue;
        }
    }


}
