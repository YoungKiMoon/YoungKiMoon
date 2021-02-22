using DrawWork.DrawModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawServices
{
    public class DrawObjectLogicService
    {
        private DrawObjectService drawObject;

        #region CONSTRUCTOR
        public DrawObjectLogicService()
        {
            drawObject = new DrawObjectService();
        }
        #endregion

        public void DrawObjectLogic(string[] eachCmd,ref CDPoint refPoint, ref CDPoint curPoint)
        {

            string cmdObject = eachCmd[0].ToLower();

            switch (cmdObject)
            {

                case "refpoint":
                    drawObject.DoRefPoint(eachCmd,ref refPoint, ref curPoint);
                    break;

                case "point":
                    drawObject.DoPoint(eachCmd, ref refPoint, ref curPoint);
                    break;

                case "line":
                    drawObject.DoLine(eachCmd, ref refPoint, ref curPoint);
                    break;

                case "text":
                    drawObject.DoText(eachCmd, ref refPoint, ref curPoint);
                    break;

                case "rec":
                    drawObject.DoRectangle(eachCmd, ref refPoint, ref curPoint);
                    break;

                case "rectangle":
                    drawObject.DoRectangle(eachCmd, ref refPoint, ref curPoint);
                    break;

                case "dim":
                case "dimline":
                    drawObject.DoDimension(eachCmd, ref refPoint, ref curPoint);
                    break;

            }

        }
    }
}
