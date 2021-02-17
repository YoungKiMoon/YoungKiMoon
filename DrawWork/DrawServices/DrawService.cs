using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using devDept.Eyeshot;
using devDept.Graphics;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Serialization;
using MColor = System.Windows.Media.Color;
//using Color = System.Drawing.Color;


namespace DrawWork.DrawServices
{
    public class DrawService
    {

        private void Draw_Dimension(CAD_Point selPoint1, CAD_Point selPoint2, CAD_Point selPoint3, double selTextHeight, double selTextGap, double selArrowSize, double selRotate)
        {
            double[] point1 = new double[3] { selPoint1.x, selPoint1.y, 0 };
            double[] point2 = new double[3] { selPoint2.x, selPoint2.y, 0 };
            double[] point3 = new double[3] { selPoint3.x, selPoint3.y, 0 };
            var dimline = currentDocument.ModelSpace.AddDimRotated(point1, point2, point3, selRotate);
            if (selTextHeight > 0)
                dimline.TextHeight = selTextHeight;
            if (selTextGap > 0)
                dimline.TextGap = selTextGap;
            if (selArrowSize > 0)
                dimline.ArrowheadSize = selArrowSize;

            dimline.Update();
        }

        private void Draw_Line(CAD_Point selPoint1, CAD_Point selPoint2)
        {
            double[] point1 = new double[3] { selPoint1.x, selPoint1.y, 0 };
            double[] point2 = new double[3] { selPoint2.x, selPoint2.y, 0 };
            currentDocument.ModelSpace.AddLine(point1, point2).Update();
        }
        private void Draw_Rectangle(CAD_Point selPoint1, double selWidth, double selHeight)
        {

            double[] point1 = new double[3] { selPoint1.x, selPoint1.y, 0 };
            double[] point2 = new double[3] { selPoint1.x + selWidth, selPoint1.y, 0 };
            double[] point3 = new double[3] { selPoint1.x + selWidth, selPoint1.y + selHeight, 0 };
            double[] point4 = new double[3] { selPoint1.x, selPoint1.y + selHeight, 0 };

            currentDocument.ModelSpace.AddLine(point1, point2).Update();
            currentDocument.ModelSpace.AddLine(point2, point3).Update();
            currentDocument.ModelSpace.AddLine(point3, point4).Update();
            currentDocument.ModelSpace.AddLine(point4, point1).Update();
        }

        private void Draw_RectangleBox(CAD_Point selPoint1, double selWidth, double selHeight, double selRotate)
        {

            double[] point1 = new double[3] { selPoint1.x, selPoint1.y, 0 };
            double[] point2 = new double[3] { selPoint1.x + selWidth, selPoint1.y, 0 };
            double[] point3 = new double[3] { selPoint1.x + selWidth, selPoint1.y + selHeight, 0 };
            double[] point4 = new double[3] { selPoint1.x, selPoint1.y + selHeight, 0 };

            double rotateAngle = selRotate * 3.141592 / 180;

            var cc = currentDocument.ModelSpace.AddLightWeightPolyline((object)new double[8]
                {   point1[0],point1[1],
                    point2[0],point2[1],
                    point3[0],point3[1],
                    point4[0],point4[1]
                });
            cc.Closed = true;
            cc.Rotate(point2, rotateAngle);
            cc.Update();


        }

        private void Draw_Text(CAD_Point selPoint1, string selText, double selHeight, string selAlign)
        {

            double[] point1 = new double[3] { selPoint1.x, selPoint1.y, 0 };


            var newText = currentDocument.ModelSpace.AddText(selText, point1, selHeight);
            if (selAlign == "c")
            {
                newText.HorizontalAlignment = Autodesk.AutoCAD.Interop.Common.AcHorizontalAlignment.acHorizontalAlignmentMiddle;
                newText.TextAlignmentPoint = point1;
            }
            newText.Update();
        }

        private CAD_Point GetDrawPoint(string selData)
        {
            CAD_Point newPoint = new CAD_Point();

            if (selData.Contains(","))
            {
                string[] dataArray = selData.Split(',');

                // Y
                string selX = GetPointDataCal(dataArray[0], "x");
                newPoint.x = GetDoubleValue(selX);

                // Y
                string selY = GetPointDataCal(dataArray[1], "y");
                newPoint.y = GetDoubleValue(selY);




            }

            return newPoint;
        }
        private string GetPointDataCal(string selCmd, string selXY)
        {
            string calStr = "";
            string newStr = "";
            string newValue = "";
            bool calStart = false;

            foreach (char ch in selCmd)
            {
                switch (ch)
                {
                    case '+':
                        calStr = "+";
                        break;
                    case '-':
                        calStr = "-";
                        break;
                    case '*':
                        calStr = "*";
                        break;
                    case '/':
                        calStr = "/";
                        break;
                }
                if (calStr != "")
                {
                    calStart = true;

                    newValue += GetPointDataCalAlpha(newStr, selXY);
                    newValue += calStr;

                    newStr = "";
                    calStr = "";
                }
                else
                {
                    newStr += ch;
                }

            }

            newValue += GetPointDataCalAlpha(newStr, selXY);



            double doubleValue = Evaluate(newValue);
            return doubleValue.ToString();


        }
        private string GetPointDataCalAlpha(string selPoint, string selXY)
        {
            string newValue = "";
            string newPoint = selPoint.Replace("@", "");
            if (newPoint == "refpointx")
            {
                newValue = referencePoint.x.ToString();
            }
            else if (newPoint == "refpointy")
            {
                newValue = referencePoint.y.ToString();
            }
            else
            {
                if (selXY == "x")
                {
                    newValue = newPoint;
                    if (selPoint.Contains("@"))
                        newValue += "+" + currentPoint.x.ToString();
                }
                else if (selXY == "y")
                {
                    newValue = newPoint;
                    if (selPoint.Contains("@"))
                        newValue += "+" + currentPoint.y.ToString();
                }

            }
            return newValue;
        }

        private int DrawMethod_Repeat(string[] eachCmd)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;

            int result = 0;

            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "rep":
                    case "repeat":
                        if (j + 1 <= eachCmd.Length)
                        {
                            double calDouble = Evaluate(eachCmd[j + 1]);
                            result = Convert.ToInt32(calDouble);
                        }
                        break;

                }

            }

            return result;

        }
    }
}
