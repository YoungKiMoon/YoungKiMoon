using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingServices
{
    public class ScaleCalService
    {
        public ScaleCalService()
        {

        }

        public double GetScaleValue(double viewWidth, double viewHeight, double modelWidth, double modelHeight)
        {
            double scaleValue = 1;
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

            // AI
            double minFactor = 2;
            double maxFactor = 5;
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
    }
}
