using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using devDept.Eyeshot;
using devDept.Graphics;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Serialization;

using DrawWork.DrawModels;
using DrawWork.ValueServices;

namespace DrawWork.DrawServices
{
    public class DrawObjectService
    {
        private DrawService drawService;
        private ValueService valueService;

        public DrawObjectService()
        {
            drawService = new DrawService();
            valueService = new ValueService();
        }


        public void DoRefPoint(string[] eachCmd,ref CDPoint refPoint,ref CDPoint curPoint)
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
                            CDPoint newPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            refPoint = newPoint;
                            curPoint.X = refPoint.X;
                            curPoint.Y = refPoint.Y;
                        }
                        break;
                }
            }

        }

        public void DoPoint(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
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
                            CDPoint newPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint = newPoint;
                        }
                        break;
                }
            }

        }

        public Line DoLine(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
                {
                    case "xy1":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "xy2":
                        if (j + 1 <= eachCmd.Length)
                            newPoint2 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            // Create Line
            return drawService.Draw_Line(newPoint1, newPoint2);

        }

        public Line[] DoRectangle(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;



            CDPoint newPoint1 = new CDPoint();
            double newWidth = 0;
            double newHeight = 0;
            CDPoint newSetPoint = new CDPoint();


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "w":
                        if (j + 1 <= eachCmd.Length)
                            newWidth = valueService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "h":
                        if (j + 1 <= eachCmd.Length)
                            newHeight = valueService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }

            }

            // Create Line
            return drawService.Draw_Rectangle(newPoint1, newWidth, newHeight);

        }

        /*
        public void DoRectangleBox(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;



            CDPoint newPoint1 = new CDPoint();
            double newWidth = 0;
            double newHeight = 0;
            CDPoint newSetPoint = new CDPoint();
            double newRotate = 0;

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = DrawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "w":
                        if (j + 1 <= eachCmd.Length)
                            newWidth = ConverterService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "h":
                        if (j + 1 <= eachCmd.Length)
                            newHeight = ConverterService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "r":
                        if (j + 1 <= eachCmd.Length)
                            newRotate = ConverterService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = DrawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }

            }

            // Create Line
            DrawService.Draw_RectangleBox(newPoint1, newWidth, newHeight, newRotate);

        }
        */
        public Text DoText(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;



            CDPoint newPoint1 = new CDPoint();
            string newText = "";
            double newHeight = 0;
            CDPoint newSetPoint = new CDPoint();
            string newAlign = "lb";

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
                {
                    case "xy":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "t":
                        if (j + 1 <= eachCmd.Length)
                            newText = eachCmd[j + 1];
                        break;

                    case "h":
                        if (j + 1 <= eachCmd.Length)
                            newHeight = valueService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "a":
                        if (j + 1 <= eachCmd.Length)
                            newAlign = eachCmd[j + 1];
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }


            }

            // Create Line
            return drawService.Draw_Text(newPoint1, newText, newHeight, newAlign);

        }

        public LinearDim DoDimension(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;


            CDPoint newPoint1 = new CDPoint();
            CDPoint newPoint2 = new CDPoint();
            CDPoint newPoint3 = new CDPoint();
            CDPoint newSetPoint = new CDPoint();
            double newTextHeight = -1;
            double newTextGap = -1;
            double newArrowSize = -1;

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j].ToLower())
                {
                    case "xy1":
                        if (j + 1 <= eachCmd.Length)
                            newPoint1 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "xy2":
                        if (j + 1 <= eachCmd.Length)
                            newPoint2 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "xyh":
                        if (j + 1 <= eachCmd.Length)
                            newPoint3 = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                        break;

                    case "th":
                        if (j + 1 <= eachCmd.Length)
                            newTextHeight = valueService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "tg":
                        if (j + 1 <= eachCmd.Length)
                            newTextGap = valueService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "ah":
                        if (j + 1 <= eachCmd.Length)
                            newArrowSize = valueService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = drawService.GetDrawPoint(eachCmd[j + 1], ref refPoint, ref curPoint);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            return drawService.Draw_Dimension(newPoint1, newPoint2, newPoint3, newTextHeight, newTextGap, newArrowSize, 0);

        }
    }
}
