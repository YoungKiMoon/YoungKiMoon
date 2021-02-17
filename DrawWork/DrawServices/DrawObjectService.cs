using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawServices
{
    public class DrawObjectService
    {
        private void DrawObject_RefPoint(string[] eachCmd)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                        {
                            CAD_Point newPoint = GetDrawPoint(eachCmd[j + 1]);
                            referencePoint = newPoint;
                            currentPoint.x = referencePoint.x;
                            currentPoint.y = referencePoint.y;
                        }
                        break;
                }
            }

        }
        private void DrawObject_Point(string[] eachCmd)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                        {
                            CAD_Point newPoint = GetDrawPoint(eachCmd[j + 1]);
                            currentPoint = newPoint;
                        }
                        break;
                }
            }

        }

        private void DrawObject_Line(string[] eachCmd)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CAD_Point newPoint1 = new CAD_Point();
            CAD_Point newPoint2 = new CAD_Point();
            CAD_Point newSetPoint = new CAD_Point();

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
                {
                    case "xy1":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = GetDrawPoint(eachCmd[j + 1]);
                        break;

                    case "xy2":
                        if (j + 1 <= eachCmd.Length)
                            newPoint2 = GetDrawPoint(eachCmd[j + 1]);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = GetDrawPoint(eachCmd[j + 1]);
                            currentPoint.x = newSetPoint.x;
                            currentPoint.y = newSetPoint.y;
                        }
                        break;
                }
            }

            // Create Line
            Draw_Line(newPoint1, newPoint2);

        }
        private void DrawObject_Rectangle(string[] eachCmd)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;



            CAD_Point newPoint1 = new CAD_Point();
            double newWidth = 0;
            double newHeight = 0;
            CAD_Point newSetPoint = new CAD_Point();


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = GetDrawPoint(eachCmd[j + 1]);
                        break;

                    case "w":
                        if (j + 1 <= eachCmd.Length)
                            newWidth = Evaluate(eachCmd[j + 1]);
                        break;

                    case "h":
                        if (j + 1 <= eachCmd.Length)
                            newHeight = Evaluate(eachCmd[j + 1]);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = GetDrawPoint(eachCmd[j + 1]);
                            currentPoint.x = newSetPoint.x;
                            currentPoint.y = newSetPoint.y;
                        }
                        break;
                }

            }

            // Create Line
            Draw_Rectangle(newPoint1, newWidth, newHeight);

        }
        private void DrawObject_RectangleBox(string[] eachCmd)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;



            CAD_Point newPoint1 = new CAD_Point();
            double newWidth = 0;
            double newHeight = 0;
            CAD_Point newSetPoint = new CAD_Point();
            double newRotate = 0;

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = GetDrawPoint(eachCmd[j + 1]);
                        break;

                    case "w":
                        if (j + 1 <= eachCmd.Length)
                            newWidth = Evaluate(eachCmd[j + 1]);
                        break;

                    case "h":
                        if (j + 1 <= eachCmd.Length)
                            newHeight = Evaluate(eachCmd[j + 1]);
                        break;

                    case "r":
                        if (j + 1 <= eachCmd.Length)
                            newRotate = Evaluate(eachCmd[j + 1]);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = GetDrawPoint(eachCmd[j + 1]);
                            currentPoint.x = newSetPoint.x;
                            currentPoint.y = newSetPoint.y;
                        }
                        break;
                }

            }

            // Create Line
            Draw_RectangleBox(newPoint1, newWidth, newHeight, newRotate);

        }

        private void DrawObject_Text(string[] eachCmd)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;



            CAD_Point newPoint1 = new CAD_Point();
            string newText = "";
            double newHeight = 0;
            CAD_Point newSetPoint = new CAD_Point();
            string newAlign = "lb";

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = GetDrawPoint(eachCmd[j + 1]);
                        break;

                    case "t":
                        if (j + 1 <= eachCmd.Length)
                            newText = eachCmd[j + 1];
                        break;

                    case "h":
                        if (j + 1 <= eachCmd.Length)
                            newHeight = Evaluate(eachCmd[j + 1]);
                        break;

                    case "a":
                        if (j + 1 <= eachCmd.Length)
                            newAlign = eachCmd[j + 1];
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = GetDrawPoint(eachCmd[j + 1]);
                            currentPoint.x = newSetPoint.x;
                            currentPoint.y = newSetPoint.y;
                        }
                        break;
                }


            }

            // Create Line
            Draw_Text(newPoint1, newText, newHeight, newAlign);

        }

        private void DrawObject_Dimension(string[] eachCmd)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CAD_Point newPoint1 = new CAD_Point();
            CAD_Point newPoint2 = new CAD_Point();
            CAD_Point newPoint3 = new CAD_Point();
            CAD_Point newSetPoint = new CAD_Point();
            double newTextHeight = -1;
            double newTextGap = -1;
            double newArrowSize = -1;

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
                {
                    case "xy1":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = GetDrawPoint(eachCmd[j + 1]);
                        break;

                    case "xy2":
                        if (j + 1 <= eachCmd.Length)
                            newPoint2 = GetDrawPoint(eachCmd[j + 1]);
                        break;

                    case "xyh":
                        if (j + 1 <= eachCmd.Length)
                            newPoint3 = GetDrawPoint(eachCmd[j + 1]);
                        break;

                    case "th":
                        if (j + 1 <= eachCmd.Length)
                            newTextHeight = Evaluate(eachCmd[j + 1]);
                        break;

                    case "tg":
                        if (j + 1 <= eachCmd.Length)
                            newTextGap = Evaluate(eachCmd[j + 1]);
                        break;

                    case "ah":
                        if (j + 1 <= eachCmd.Length)
                            newArrowSize = Evaluate(eachCmd[j + 1]);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = GetDrawPoint(eachCmd[j + 1]);
                            currentPoint.x = newSetPoint.x;
                            currentPoint.y = newSetPoint.y;
                        }
                        break;
                }
            }

            Draw_Dimension(newPoint1, newPoint2, newPoint3, newTextHeight, newTextGap, newArrowSize, 0);

        }
    }
}
