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

using DrawWork.DrawModels;
using DrawWork.ValueServices;

namespace DrawWork.DrawServices
{
    public class DrawService
    {
        private ValueService valueService;

        public DrawService()
        {
            valueService = new ValueService();
        }

        
        public LinearDim Draw_Dimension(CDPoint selPoint1, CDPoint selPoint2, CDPoint selPoint3, double selTextHeight, double selTextGap, double selArrowSize, double selRotate)
        {

            LinearDim newDim = new LinearDim(Plane.XY,
                                             new Point2D(selPoint1.X,selPoint1.Y),
                                             new Point2D(selPoint2.X, selPoint2.Y),
                                             new Point2D(selPoint3.X, selPoint3.Y),selTextHeight);
            if (selTextGap > 0)
                newDim.TextGap = selTextGap;
            if (selArrowSize > 0)
                newDim.ArrowheadSize = selArrowSize;

            newDim.TextLocation = elementPositionType.Outside;
            
            
            return newDim;
        }
        

        public Line Draw_Line(CDPoint selPoint1, CDPoint selPoint2)
        {
            Line newLine = new Line(selPoint1.X, selPoint1.Y, selPoint2.X, selPoint2.Y);
            return newLine;
        }
        public Line Draw_Line(CDPoint selPoint1, CDPoint selPoint2, double selDegree)
        {
            // arctan // X: 1로 고정
            double calDegree = Math.Atan2(1,selDegree);

            double tempWidth = Point3D.Distance(new Point3D(selPoint1.X,selPoint1.Y,selPoint1.Z), new Point3D(selPoint2.X, selPoint2.Y, selPoint2.Z));
            double tempHeight = tempWidth * calDegree;
            Line newLine = new Line(selPoint1.X, selPoint1.Y, selPoint2.X, selPoint1.Y + tempHeight);
            return newLine;
        }

        public Line[] Draw_Rectangle(CDPoint selPoint1, double selWidth, double selHeight)
        {
            Line[] newLine = new Line[4];
            newLine[0] = new Line(selPoint1.X, selPoint1.Y, selPoint1.X + selWidth, selPoint1.Y);
            newLine[1] = new Line(selPoint1.X + selWidth, selPoint1.Y, selPoint1.X + selWidth, selPoint1.Y + selHeight);
            newLine[2] = new Line(selPoint1.X + selWidth, selPoint1.Y + selHeight, selPoint1.X, selPoint1.Y + selHeight);
            newLine[3] = new Line(selPoint1.X, selPoint1.Y + selHeight, selPoint1.X, selPoint1.Y);
            return newLine;
        }


        public Arc Draw_Arc(CDPoint selPoint1, CDPoint selPoint2, CDPoint selPoint3)
        {
            Arc newArc = new Arc(new Point3D(selPoint1.X, selPoint1.Y,0), new Point3D(selPoint2.X, selPoint2.Y, 0), new Point3D(selPoint3.X, selPoint3.Y, 0));
            return newArc;
        }
        public Text Draw_Text(CDPoint selPoint1, string selText, double selHeight, string selAlign)
        {

            Text newText = new Text(selPoint1.X, selPoint1.Y, 0, selText, selHeight);
            switch (selAlign)
            {
                case "c":
                    newText.Alignment = Text.alignmentType.MiddleCenter;
                    break;
            }

            return newText;
        }
        
        public CDPoint GetDrawPoint(string selData, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            CDPoint newPoint = new CDPoint();

            if (selData.Contains(","))
            {
                string[] dataArray = selData.Split(',');

                // X
                string selX = GetPointDataCal(dataArray[0], "x", ref refPoint, ref curPoint);
                newPoint.X = valueService.GetDoubleValue(selX);

                // Y
                string selY = GetPointDataCal(dataArray[1], "y", ref refPoint, ref curPoint);
                newPoint.Y = valueService.GetDoubleValue(selY);




            }

            return newPoint;
        }

        public string GetPointDataCal(string selCmd, string selXY, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            string calStr = "";
            string newStr = "";
            string newValue = "";
            //bool calStart = false;

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
                    //calStart = true;

                    newValue += GetPointDataCalAlpha(newStr, selXY, ref refPoint, ref curPoint);
                    newValue += calStr;

                    newStr = "";
                    calStr = "";
                }
                else
                {
                    newStr += ch;
                }

            }

            newValue += GetPointDataCalAlpha(newStr, selXY, ref refPoint, ref curPoint);



            double doubleValue = valueService.Evaluate(newValue);
            return doubleValue.ToString();


        }
        public string GetPointDataCalAlpha(string selPoint, string selXY, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            string newValue = "";
            string newPoint = selPoint.Replace("@", "");
            if (newPoint == "refpointx")
            {
                newValue = refPoint.X.ToString();
            }
            else if (newPoint == "refpointy")
            {
                newValue = refPoint.Y.ToString();
            }
            else
            {
                if (selXY == "x")
                {
                    newValue = newPoint;
                    if (selPoint.Contains("@"))
                        newValue += "+" + curPoint.X.ToString();
                }
                else if (selXY == "y")
                {
                    newValue = newPoint;
                    if (selPoint.Contains("@"))
                        newValue += "+" + curPoint.Y.ToString();
                }

            }
            return newValue;
        }
    }
}
