using DrawWork.DrawModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawServices
{
    public class DrawObjectService
    {
        private DrawService DrawService;
        private DataCoverterService ConverterService;

        public DrawObjectService()
        {
            DrawService = new DrawService();
            ConverterService = new DataCoverterService();
        }


        private void DrawObject_RefPoint(string[] eachCmd,ref CDPoint refPoint,ref CDPoint curPoint)
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
                            CDPoint newPoint = DrawService.GetDrawPoint(eachCmd[j + 1]);
                            refPoint = newPoint;
                            curPoint.X = refPoint.X;
                            curPoint.Y = refPoint.Y;
                        }
                        break;
                }
            }

        }
        private void DrawObject_Point(string[] eachCmd, ref CDPoint curPoint)
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
                            CDPoint newPoint = DrawService.GetDrawPoint(eachCmd[j + 1]);
                            curPoint = newPoint;
                        }
                        break;
                }
            }

        }

        private void DrawObject_Line(string[] eachCmd, ref CDPoint curPoint)
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
                            newPoint1 = DrawService.GetDrawPoint(eachCmd[j + 1]);
                        break;

                    case "xy2":
                        if (j + 1 <= eachCmd.Length)
                            newPoint2 = DrawService.GetDrawPoint(eachCmd[j + 1]);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = DrawService.GetDrawPoint(eachCmd[j + 1]);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            // Create Line
            DrawService.Draw_Line(newPoint1, newPoint2);

        }
        private void DrawObject_Rectangle(string[] eachCmd,ref CDPoint curPoint)
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
                            newPoint1 = DrawService.GetDrawPoint(eachCmd[j + 1]);
                        break;

                    case "w":
                        if (j + 1 <= eachCmd.Length)
                            newWidth = ConverterService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "h":
                        if (j + 1 <= eachCmd.Length)
                            newHeight = ConverterService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = DrawService.GetDrawPoint(eachCmd[j + 1]);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }

            }

            // Create Line
            DrawService.Draw_Rectangle(newPoint1, newWidth, newHeight);

        }
        private void DrawObject_RectangleBox(string[] eachCmd, ref CDPoint curPoint)
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
                            newPoint1 = DrawService.GetDrawPoint(eachCmd[j + 1]);
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
                            newSetPoint = DrawService.GetDrawPoint(eachCmd[j + 1]);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }

            }

            // Create Line
            DrawService.Draw_RectangleBox(newPoint1, newWidth, newHeight, newRotate);

        }

        private void DrawObject_Text(string[] eachCmd, ref CDPoint curPoint)
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
                            newPoint1 = DrawService.GetDrawPoint(eachCmd[j + 1]);
                        break;

                    case "t":
                        if (j + 1 <= eachCmd.Length)
                            newText = eachCmd[j + 1];
                        break;

                    case "h":
                        if (j + 1 <= eachCmd.Length)
                            newHeight = ConverterService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "a":
                        if (j + 1 <= eachCmd.Length)
                            newAlign = eachCmd[j + 1];
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = DrawService.GetDrawPoint(eachCmd[j + 1]);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }


            }

            // Create Line
            DrawService.Draw_Text(newPoint1, newText, newHeight, newAlign);

        }

        private void DrawObject_Dimension(string[] eachCmd, ref CDPoint curPoint)
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
                            newPoint1 = DrawService.GetDrawPoint(eachCmd[j + 1]);
                        break;

                    case "xy2":
                        if (j + 1 <= eachCmd.Length)
                            newPoint2 = DrawService.GetDrawPoint(eachCmd[j + 1]);
                        break;

                    case "xyh":
                        if (j + 1 <= eachCmd.Length)
                            newPoint3 = DrawService.GetDrawPoint(eachCmd[j + 1]);
                        break;

                    case "th":
                        if (j + 1 <= eachCmd.Length)
                            newTextHeight = ConverterService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "tg":
                        if (j + 1 <= eachCmd.Length)
                            newTextGap = ConverterService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "ah":
                        if (j + 1 <= eachCmd.Length)
                            newArrowSize = ConverterService.Evaluate(eachCmd[j + 1]);
                        break;

                    case "sp":
                        if (j + 1 <= eachCmd.Length)
                        {
                            newSetPoint = DrawService.GetDrawPoint(eachCmd[j + 1]);
                            curPoint.X = newSetPoint.X;
                            curPoint.Y = newSetPoint.Y;
                        }
                        break;
                }
            }

            DrawService.Draw_Dimension(newPoint1, newPoint2, newPoint3, newTextHeight, newTextGap, newArrowSize, 0);

        }
    }
}
