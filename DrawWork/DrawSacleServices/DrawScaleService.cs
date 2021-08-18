using AssemblyLib.AssemblyModels;
using DrawSettingLib.SettingModels;
using DrawWork.DrawDetailServices;
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
                    double eachValue = valueService.GetDoubleValue(eachScale.HorizontalID);
                    if (tankWidth < eachValue)
                        break;
                }
            }

            st1Value= valueService.GetDoubleValue(returnString);
            st2Value = valueService.GetDoubleValue(returnString2);
            returnValue = Math.Max(st1Value, st2Value);
            returnValue +=0;
            return returnValue;
        }

        #region Scale : calculation
        public double GetScaleCalValue(double viewWidth, double viewHeight, double modelWidth, double modelHeight)
        {
            double tempScaleValue = 1;
            if (modelWidth >= modelHeight)
            {
                // Horizontal
                tempScaleValue = Math.Ceiling(modelWidth / viewWidth);
            }
            else
            {
                // Vertical
                tempScaleValue = Math.Ceiling(modelHeight / viewHeight);
            }

            return tempScaleValue;
        }
        #endregion

        #region Scale : GA ( Genernal Assembly )

        // GA Scale
        public double GetGAScaleValue(PaperAreaModel selPaperArea, GAAreaModel selGAArea)
        {
            double viewWidth = selPaperArea.Size.X - selGAArea.Dimension.AreaSize.Width * 2 - selGAArea.NozzleLeader.AreaSize.Width * 2 - selGAArea.ShellCourse.AreaSize.Width;
            double viewHeight = selPaperArea.Size.Y - selGAArea.Dimension.AreaSize.Height * 2 - selGAArea.NozzleLeader.AreaSize.Height * 2;
            double modelWidth = selGAArea.MainAssembly.BoxSize.Width;
            double modelHeight = selGAArea.MainAssembly.BoxSize.Height;



            return GetOrientationScaleValue(viewWidth,viewHeight,modelWidth,modelHeight);
        }

        // Orientation
        public double GetOrientationScaleValue(double viewWidth, double viewHeight, double modelWidth, double modelHeight)
        {
            double tempScaleValue = GetScaleCalValue(viewWidth, viewHeight, modelWidth, modelHeight);

            // GA 적용
            double scaleValue = 1;
            if (tempScaleValue < 22)
            {
                if (!IsEvenNumber(tempScaleValue))
                    tempScaleValue += 1;
                scaleValue = tempScaleValue;
            }
            else if (tempScaleValue == 22)
            {
                scaleValue = 22;
            }
            else if (tempScaleValue > 22)
            {
                if (!IsHalfNumber(tempScaleValue))
                    while (!IsHalfNumber(tempScaleValue))
                        tempScaleValue++;

                scaleValue = tempScaleValue;
            }

            return scaleValue;
        }

        private bool IsEvenNumber(double selNumber)
        {
            return selNumber % 2 == 0;
        }
        private bool IsHalfNumber(double selNumber)
        {
            return selNumber % 5 == 0;
        }

        #endregion


        #region Scale : Detail

        // Plate Horizontal Joint
        public double GetPlateHorizontalJointScale(double oneCourseThk)
        {
            double scaleValue = 1;
            if (oneCourseThk >= 31)
            {
                scaleValue = 8;
            }
            else if (oneCourseThk >= 21)
            {
                scaleValue = 7;
            }
            else
            {
                scaleValue = 6;
            }

            return scaleValue;
        }

        public double GetOneCourseDevelopmentScale(PaperAreaModel selPaperArea,double tankID, double oneCourseWidth, double plateWidth, double plateMaxLength)
        {
            
            // One Plate : Length
            DrawDetailShellService dsService = new DrawDetailShellService( null,null);


            double oneCoursePlateCount = 0;
            double onePlateLength = 0;
            dsService.SetCourseOnePlate(tankID, plateMaxLength, out oneCoursePlateCount, out onePlateLength);

            double maxRow = 3;
            double viewWidth = selPaperArea.Size.X -22 -22; // 매우중요
            double viewHeight = selPaperArea.Size.Y /3; // 매우중요

            double modelWidth = onePlateLength * maxRow;
            double modelHeight = plateWidth;

            return GetScaleCalValue(viewWidth,viewHeight,modelWidth,modelHeight);
        }

        #endregion

    }


}
