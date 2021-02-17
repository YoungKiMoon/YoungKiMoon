using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawServices
{
    public class DrawObjectLogicService
    {
        public void DrawObjectLogic(string[] eachCmd)
        {

            string cmdObject = eachCmd[0].ToLower();

            switch (cmdObject)
            {

                case "refpoint":
                    DrawObject_RefPoint(eachCmd);
                    break;

                case "point":
                    DrawObject_Point(eachCmd);
                    break;

                case "line":
                    DrawObject_Line(eachCmd);
                    break;

                case "text":
                    DrawObject_Text(eachCmd);
                    break;

                case "rec":
                    DrawObject_Rectangle(eachCmd);
                    break;

                case "rectangle":
                    DrawObject_RectangleBox(eachCmd);
                    break;

                case "dim":
                case "dimline":
                    DrawObject_Dimension(eachCmd);
                    break;

            }

        }
    }
}
