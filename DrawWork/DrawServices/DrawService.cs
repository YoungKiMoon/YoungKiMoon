﻿using System;
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
using DrawWork.Commons;
using AssemblyLib.AssemblyModels;
using DrawWork.DrawSacleServices;
using DrawWork.DrawStyleServices;

namespace DrawWork.DrawServices
{
    public class DrawService
    {
        private AssemblyModel assemblyData;

        private DrawWorkingPointService cpService;
        private ValueService valueService;

        private DrawScaleService scaleService;

        private StyleFunctionService styleSerivce;

        public DrawService(AssemblyModel selAssembly)
        {
            assemblyData = selAssembly;
            cpService = new DrawWorkingPointService(selAssembly);

            valueService = new ValueService();
            scaleService = new DrawScaleService();
            styleSerivce = new StyleFunctionService();

        }


        public Line Draw_Line(CDPoint selPoint1, CDPoint selPoint2, string selLayerName)
        {
            Line newLine = new Line(selPoint1.X, selPoint1.Y, selPoint2.X, selPoint2.Y);
            styleSerivce.SetLayer(ref newLine, selLayerName);
            return newLine;
        }
        public Line Draw_Line(CDPoint selPoint1, CDPoint selPoint2, double selDegree, string selDirection,string selLayerName)
        {
            // arctan // X: 1로 고정
            double calDegree = valueService.GetDegreeOfSlope(1,selDegree);

            double tempWidth = 0;
            double tempHeight = 0;
            Line newLine = null;
            if (selDirection == "x")
            {
                tempWidth = Point3D.Distance(new Point3D(selPoint1.X, selPoint1.Y, selPoint1.Z), new Point3D(selPoint2.X, selPoint1.Y, selPoint2.Z));
                tempHeight = valueService.GetOppositeByWidth(calDegree,tempWidth);
                newLine= new Line(selPoint1.X, selPoint1.Y, selPoint2.X, selPoint1.Y + tempHeight);
            }   
            else if(selDirection=="y")
            {
                tempWidth = Point3D.Distance(new Point3D(selPoint1.X, selPoint1.Y, selPoint1.Z), new Point3D(selPoint1.X, selPoint2.Y, selPoint2.Z));
                tempHeight = valueService.GetOppositeByWidth(calDegree, tempWidth);
                newLine = new Line(selPoint1.X, selPoint1.Y, selPoint1.X + tempHeight, selPoint2.Y);
            }
            styleSerivce.SetLayer(ref newLine, selLayerName);
            return newLine;
        }

        public Line[] Draw_Rectangle(CDPoint selPoint1, double selWidth, double selHeight,string selLayerName)
        {
            Line[] newLine = new Line[4];
            newLine[0] = new Line(selPoint1.X, selPoint1.Y, selPoint1.X + selWidth, selPoint1.Y);
            newLine[1] = new Line(selPoint1.X + selWidth, selPoint1.Y, selPoint1.X + selWidth, selPoint1.Y + selHeight);
            newLine[2] = new Line(selPoint1.X + selWidth, selPoint1.Y + selHeight, selPoint1.X, selPoint1.Y + selHeight);
            newLine[3] = new Line(selPoint1.X, selPoint1.Y + selHeight, selPoint1.X, selPoint1.Y);
            styleSerivce.SetLayer(ref newLine, selLayerName);
            return newLine;
        }



        public Arc Draw_Arc(CDPoint selPoint1, CDPoint selPoint2, CDPoint selPoint3,string selLayerName)
        {
            Arc newArc = new Arc(new Point3D(selPoint1.X, selPoint1.Y,0), new Point3D(selPoint2.X, selPoint2.Y, 0), new Point3D(selPoint3.X, selPoint3.Y, 0));
            styleSerivce.SetLayer(ref newArc, selLayerName);
            return newArc;
        }


        public Text Draw_Text(CDPoint selPoint1, string selText, double selHeight, string selAlign,string selLayerName)
        {

            Text newText = new Text(selPoint1.X, selPoint1.Y, 0, selText, selHeight);
            switch (selAlign)
            {
                case "c":
                    newText.Alignment = Text.alignmentType.MiddleCenter;
                    break;
            }

            styleSerivce.SetLayer(ref newText, selLayerName);
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
        private string GetPointDataCal(string selCmd, string selXY, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            string calStr = "";
            string newStr = "";
            string newValue = "";
            //bool calStart = false;

            bool closeBracket = true;
            foreach (char ch in selCmd)
            {
                if (ch == '[')
                    closeBracket = false;
                if (ch == ']')
                    closeBracket = true;

                calStr = getCalculationCharacter(ch);

                if (calStr != "" && closeBracket)
                {
                    //calStart = true;

                    newValue += GetPointDataCalAlpha(newStr,selXY,ref refPoint,ref curPoint);
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
        private string GetPointDataCalAlpha(string selPoint, string selXY, ref CDPoint refPoint, ref CDPoint curPoint)
        {
            string newValue = "";
            string newPoint = selPoint.Replace("@", "");

            // Control Point
            if (newPoint.Contains("cp["))
                newPoint = "cp";

            switch (newPoint)
            {
                case "refpointx":
                    newValue = refPoint.X.ToString();
                    break;
                case "refpointy":
                    newValue = refPoint.Y.ToString();
                    break;

                case "wp":
                case "cp":
                case "cpoint":
                case "contactpoint":
                case "workingpoint":
                    string[] contactArray = GetContactPointArray(selPoint);
                    CDPoint contactPoint= cpService.WorkingPoint(contactArray[0], ref refPoint, ref curPoint);
                    if (contactArray[1] == "x")
                    {
                        newValue = contactPoint.X.ToString();
                    }
                    else if (contactArray[1] == "y")
                    {
                        newValue = contactPoint.Y.ToString();
                    }
                    break;


                default:
                    if (selXY == "x")
                    {
                        newValue = newPoint;
                        if (selPoint.Contains("@"))
                            if (newPoint == "")
                            {
                                newValue = curPoint.X.ToString() + "+" + "0";
                            }
                            else
                            {
                                newValue = curPoint.X.ToString() + "+" + newValue;
                            }
                    }
                    else if (selXY == "y")
                    {
                        newValue = newPoint;
                        if (selPoint.Contains("@"))
                            if (newPoint == "")
                            {
                                newValue = curPoint.Y.ToString() + "+" + "0";
                            }
                            else
                            {
                                newValue = curPoint.Y.ToString() + "+" + newValue;
                            }
                            
                    }
                    break;

            }


            return newValue;
        }
        private string[] GetContactPointArray(string selPoint)
        {
            string contactPointName = "";
            string contactPointValue = "";
            if (selPoint.Contains("["))
            {
                string[] firstArray = selPoint.Split(new char[] { '[' });
                foreach(string eachStr in firstArray)
                {
                    if (eachStr.Contains("]"))
                    {
                        string[] secondArray = eachStr.Split(new char[] { ']' });
                        contactPointName = secondArray[0];
                        if (secondArray[1].Contains("."))
                        {
                            string[] thridArray = secondArray[1].Split(new char[] { '.' });
                            contactPointValue = thridArray[1].Trim();
                        }
                    }
                }
            }
            return new string[2]{ contactPointName,contactPointValue};
        }
        private string getCalculationCharacter(char ch)
        {
            string calStr = "";
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
            return calStr;
        }

        // 사용 안함
        //public Entity[] Draw_DimensionOld(CDPoint selPoint1, CDPoint selPoint2, CDPoint selPoint3, 
        //                            string selPosition, double selDimHeight, double selTextHeight, double selTextGap, double selArrowSize, 
        //                            string selTextPrefix, string selTextSuffix, string selTextUserInput, 
        //                            double selRotate)
        //{
        //    List<Entity> customEntityList = new List<Entity>();

        //    // selPoint3 : 쓰지 않음

        //    // Style
        //    DrawDimStyle newDimStyle = new DrawDimStyle();

        //    // Text Center
        //    CDPoint textCenter = new CDPoint();
        //    LinearDim newDim = null;


        //    switch (selPosition)
        //    {
        //        case "top":
        //            textCenter.X = selPoint1.X + (selPoint2.X - selPoint1.X) / 2;
        //            textCenter.Y = Math.Max(selPoint1.Y, selPoint2.Y) + selDimHeight;
        //            newDim = new LinearDim(Plane.XY, new Point3D(selPoint1.X, selPoint1.Y),
        //                                    new Point3D(selPoint2.X, selPoint2.Y),
        //                                    new Point3D(textCenter.X, textCenter.Y), selTextHeight);
        //            break;

        //        case "left":
        //            textCenter.X = selPoint1.Y + (selPoint2.Y - selPoint1.Y) / 2;
        //            textCenter.Y = Math.Min(selPoint1.X, selPoint2.X) - selDimHeight;
        //            Plane planeLeft = new Plane(new Point3D(selPoint1.X, selPoint1.Y), Vector3D.AxisY, -1 * Vector3D.AxisX);
        //            newDim = new LinearDim(planeLeft, new Point3D(selPoint1.X, selPoint1.Y),
        //                                    new Point3D(selPoint2.X, selPoint2.Y),
        //                                    new Point3D(textCenter.Y, textCenter.X), selTextHeight);
        //            break;

        //        case "right":
        //            textCenter.X = selPoint1.Y + (selPoint2.Y - selPoint1.Y) / 2;
        //            textCenter.Y = Math.Max(selPoint1.X, selPoint2.X) + selDimHeight;
        //            Plane planeRight = new Plane(new Point3D(selPoint1.X, selPoint1.Y), Vector3D.AxisY, -1 * Vector3D.AxisX);
        //            newDim = new LinearDim(planeRight, new Point3D(selPoint1.X, selPoint1.Y),
        //                                    new Point3D(selPoint2.X, selPoint2.Y),
        //                                    new Point3D(textCenter.Y, textCenter.X), selTextHeight);

        //            break;

        //        case "bottom":
        //            textCenter.X = selPoint1.X + (selPoint2.X - selPoint1.X) / 2;
        //            textCenter.Y = Math.Min(selPoint1.Y, selPoint2.Y) - selDimHeight;
        //            newDim = new LinearDim(Plane.XY, new Point3D(selPoint1.X, selPoint1.Y),
        //                                    new Point3D(selPoint2.X, selPoint2.Y),
        //                                    new Point3D(textCenter.X, textCenter.Y), selTextHeight);
        //            break;
        //    }


        //    // Set Default
        //    newDim.ArrowsLocation = elementPositionType.Inside;
        //    newDim.TextLocation = elementPositionType.Inside;

        //    newDim.TextGap = newDimStyle.textGap;
        //    newDim.ExtLineExt = newDimStyle.extensionLine;
        //    newDim.ExtLineOffset = newDimStyle.extensionLinesOffset;
        //    newDim.ArrowheadSize = newDimStyle.arrowheadSize;
        //    if (selTextGap > 0)
        //        //newDim.TextGap = selTextGap;
        //        if (selArrowSize > 0)
        //            newDim.ArrowheadSize = selArrowSize;


        //    newDim.TextPrefix = selTextPrefix;
        //    newDim.TextSuffix = selTextSuffix;
        //    if(selTextUserInput!="")
        //        newDim.TextOverride = selTextUserInput;

        //    customEntityList.Add(newDim);

        //    return customEntityList.ToArray();
        //}
        public Dictionary<string,List<Entity>> Draw_Dimension(CDPoint selPoint1, CDPoint selPoint2, CDPoint selPoint3,
                                    string selPosition, double selDimHeight, double selTextHeight, double selTextGap, double selArrowSize,
                                    string selTextPrefix, string selTextSuffix, string selTextUserInput,
                                    double selRotate,
                                    double selScale,
                                    string selLayerName
                                    )
        {

            Point3D textCenter = new Point3D();

            // Origin
            double selArrowHeight = 0.8;
            double selArrowWidth = 2.5;
        
            double extLine1 = 2;
            double extLine2 = 2;
            double pointGapLine1 = 1;
            double pointGapLine2 = 1;

            double textGap = 1;


            // Default : Force
            selTextHeight = 2.5;

            // Scale
            double scaleArrowHeight = scaleService.GetOriginValueOfScale(selScale, selArrowHeight);
            double scaleArrowWidth = scaleService.GetOriginValueOfScale(selScale, selArrowWidth);

            double scaleExtLine1 = scaleService.GetOriginValueOfScale(selScale, extLine1);
            double scaleExtLine2 = scaleService.GetOriginValueOfScale(selScale, extLine2);
            double scalePointGapLine1 = scaleService.GetOriginValueOfScale(selScale, pointGapLine1);
            double scalePointGapLine2 = scaleService.GetOriginValueOfScale(selScale, pointGapLine2);

            double scaleTextGap = scaleService.GetOriginValueOfScale(selScale, textGap);

            double scaleTextHeight= scaleService.GetOriginValueOfScale(selScale, selTextHeight);


            string selDimtext = "";

            Line dimLine1 = null;
            Line dimLine2 = null;
            Line arrowLine = null;
            Triangle tri1 = null;
            Triangle tri2 = null;
            Text dimText = null;



            // Top
            switch (selPosition)
            {
                case "top":
                    textCenter.X = selPoint1.X + (selPoint2.X - selPoint1.X) / 2;
                    textCenter.Y = Math.Min(selPoint1.Y, selPoint2.Y) + selDimHeight;
                    selDimtext = Point3D.Distance(new Point3D(selPoint1.X, textCenter.Y), new Point3D(selPoint2.X, textCenter.Y)).ToString();

                    arrowLine = new Line(new Point3D(selPoint1.X +1, textCenter.Y), new Point3D(selPoint2.X -1, textCenter.Y));
                    dimLine1 = new Line(new Point3D(selPoint1.X, selPoint1.Y + scalePointGapLine1), new Point3D(selPoint1.X, textCenter.Y + scaleExtLine1));
                    dimLine2 = new Line(new Point3D(selPoint2.X, selPoint2.Y + scalePointGapLine2), new Point3D(selPoint2.X, textCenter.Y + scaleExtLine2));
                    dimText = new Text(new Point3D(textCenter.X, textCenter.Y + scaleTextGap), selDimtext, scaleTextHeight);
                    dimText.Alignment = Text.alignmentType.BaselineCenter;
                    tri1 = new Triangle(new Point3D(selPoint1.X, textCenter.Y), new Point3D(selPoint1.X + scaleArrowWidth, textCenter.Y + scaleArrowHeight / 2), new Point3D(selPoint1.X + scaleArrowWidth, textCenter.Y - scaleArrowHeight / 2));
                    tri2 = new Triangle(new Point3D(selPoint2.X, textCenter.Y), new Point3D(selPoint2.X - scaleArrowWidth, textCenter.Y + scaleArrowHeight / 2), new Point3D(selPoint2.X - scaleArrowWidth, textCenter.Y - scaleArrowHeight / 2));

                    break;

                case "bottom":
                    textCenter.X = selPoint1.X + (selPoint2.X - selPoint1.X) / 2;
                    textCenter.Y = Math.Max(selPoint1.Y, selPoint2.Y) - selDimHeight;
                    selDimtext = Point3D.Distance(new Point3D(selPoint1.X, textCenter.Y), new Point3D(selPoint2.X, textCenter.Y)).ToString();

                    arrowLine = new Line(new Point3D(selPoint1.X+1, textCenter.Y), new Point3D(selPoint2.X -1, textCenter.Y));
                    dimLine1 = new Line(new Point3D(selPoint1.X, selPoint1.Y - scalePointGapLine1), new Point3D(selPoint1.X, textCenter.Y - scaleExtLine1));
                    dimLine2 = new Line(new Point3D(selPoint2.X, selPoint2.Y - scalePointGapLine2), new Point3D(selPoint2.X, textCenter.Y - scaleExtLine2));
                    dimText = new Text(new Point3D(textCenter.X, textCenter.Y + scaleTextGap), selDimtext, scaleTextHeight);
                    dimText.Alignment = Text.alignmentType.BaselineCenter;
                    tri1 = new Triangle(new Point3D(selPoint1.X, textCenter.Y), new Point3D(selPoint1.X + scaleArrowWidth, textCenter.Y + scaleArrowHeight / 2), new Point3D(selPoint1.X + scaleArrowWidth, textCenter.Y - scaleArrowHeight / 2));
                    tri2 = new Triangle(new Point3D(selPoint2.X, textCenter.Y), new Point3D(selPoint2.X - scaleArrowWidth, textCenter.Y + scaleArrowHeight / 2), new Point3D(selPoint2.X - scaleArrowWidth, textCenter.Y - scaleArrowHeight / 2));

                    break;


                case "left":
                    textCenter.X = Math.Max(selPoint1.X, selPoint2.X) - selDimHeight;
                    textCenter.Y = selPoint1.Y + (selPoint2.Y - selPoint1.Y) / 2;
                    selDimtext = Point3D.Distance(new Point3D(textCenter.X, selPoint1.Y), new Point3D(textCenter.X, selPoint2.Y)).ToString();

                    arrowLine = new Line(new Point3D(textCenter.X, selPoint1.Y+1), new Point3D(textCenter.X, selPoint2.Y-1));
                    dimLine1 = new Line(new Point3D(selPoint1.X - scalePointGapLine1, selPoint1.Y), new Point3D(textCenter.X - scaleExtLine1, selPoint1.Y));
                    dimLine2 = new Line(new Point3D(selPoint2.X - scalePointGapLine2, selPoint2.Y), new Point3D(textCenter.X - scaleExtLine2, selPoint2.Y));
                    Plane planeLeft = new Plane(new Point3D(selPoint1.X, selPoint1.Y), Vector3D.AxisY, -1 * Vector3D.AxisX);
                    dimText = new Text(planeLeft, new Point3D(textCenter.X - scaleTextGap, textCenter.Y), selDimtext, scaleTextHeight);
                    dimText.Alignment = Text.alignmentType.BaselineCenter;
                    //dimText.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
                    tri1 = new Triangle(new Point3D(textCenter.X, selPoint1.Y), new Point3D(textCenter.X - scaleArrowHeight / 2, selPoint1.Y + scaleArrowWidth), new Point3D(textCenter.X + scaleArrowHeight / 2, selPoint1.Y + scaleArrowWidth));
                    tri2 = new Triangle(new Point3D(textCenter.X, selPoint2.Y), new Point3D(textCenter.X - scaleArrowHeight / 2, selPoint2.Y - scaleArrowWidth), new Point3D(textCenter.X + scaleArrowHeight / 2, selPoint2.Y - scaleArrowWidth));

                    break;

                case "right":
                    textCenter.X = Math.Min(selPoint1.X, selPoint2.X) + selDimHeight;
                    textCenter.Y = selPoint1.Y + (selPoint2.Y - selPoint1.Y) / 2;
                    selDimtext = Point3D.Distance(new Point3D(textCenter.X, selPoint1.Y), new Point3D(textCenter.X, selPoint2.Y)).ToString();

                    arrowLine = new Line(new Point3D(textCenter.X, selPoint1.Y+1), new Point3D(textCenter.X, selPoint2.Y-1));
                    dimLine1 = new Line(new Point3D(selPoint1.X + scalePointGapLine1, selPoint1.Y), new Point3D(textCenter.X + scaleExtLine1, selPoint1.Y));
                    dimLine2 = new Line(new Point3D(selPoint2.X + scalePointGapLine2, selPoint2.Y), new Point3D(textCenter.X + scaleExtLine2, selPoint2.Y));
                    Plane planeLeft2 = new Plane(new Point3D(selPoint1.X, selPoint1.Y), Vector3D.AxisY, -1 * Vector3D.AxisX);
                    dimText = new Text(planeLeft2, new Point3D(textCenter.X - scaleTextGap, textCenter.Y), selDimtext, scaleTextHeight);
                    dimText.Alignment = Text.alignmentType.BaselineCenter;
                    //dimText.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
                    tri1 = new Triangle(new Point3D(textCenter.X, selPoint1.Y), new Point3D(textCenter.X - scaleArrowHeight / 2, selPoint1.Y + scaleArrowWidth), new Point3D(textCenter.X + scaleArrowHeight / 2, selPoint1.Y + scaleArrowWidth));
                    tri2 = new Triangle(new Point3D(textCenter.X, selPoint2.Y), new Point3D(textCenter.X - scaleArrowHeight / 2, selPoint2.Y - scaleArrowWidth), new Point3D(textCenter.X + scaleArrowHeight / 2, selPoint2.Y - scaleArrowWidth));

                    break;



            }

            if (selTextUserInput != "")
                selDimtext = selTextUserInput;

            if (selTextPrefix != "")
                selDimtext = selTextPrefix + selDimtext;
            if (selTextSuffix != "")
                selDimtext = selDimtext + selTextSuffix;

            dimText.TextString = selDimtext;


            
            List<Entity> dimlineList = new List<Entity>();
            List<Entity> dimTextList = new List<Entity>();
            List<Entity> dimlineExtList = new List<Entity>();
            List<Entity> dimArrowList = new List<Entity>();



            dimlineList.Add(arrowLine);
            dimArrowList.Add(tri1);
            dimArrowList.Add(tri2);
            dimTextList.Add(dimText);
            dimlineExtList.Add(dimLine1);
            dimlineExtList.Add(dimLine2);
            

            Dictionary<string, List<Entity>> customEntityList = new Dictionary<string, List<Entity>>();

            // Layer
            styleSerivce.SetLayerListEntity(ref dimlineList, selLayerName);
            styleSerivce.SetLayerListEntity(ref dimlineExtList, selLayerName);
            styleSerivce.SetLayerListEntity(ref dimArrowList, selLayerName);
            styleSerivce.SetLayerListTextEntity(ref dimTextList, selLayerName);


            customEntityList.Add(CommonGlobal.DimLine, dimlineList);
            customEntityList.Add(CommonGlobal.DimText, dimTextList);
            customEntityList.Add(CommonGlobal.DimLineExt, dimlineExtList);
            customEntityList.Add(CommonGlobal.DimArrow, dimArrowList);


            return customEntityList;
        }

        public Dictionary<string, List<Entity>> Draw_Leader(CDPoint selPoint1, 
                                    string selLength, string selPostion,string selTextHeight,string selLayerHeight,
                                    List<string> selText, List<string> selTextSub, 
                                    Model ssModel,
                                    double selScale,
                                    string selLayerName)

        {

            List<Entity> leaderLine = new List<Entity>();
            List<Entity> leaderArrow = new List<Entity>();
            List<Entity> leaderText = new List<Entity>();

            //selPoint1.X = 10;
            //selPoint1.Y = 10;
            double calDegree = 30;
            double distance = valueService.GetDoubleValue( selLength);

            double selArrowHeight = 0.8;
            double selArrowWidth = 2.5;
            double textHeight = 2.5;
            double textGap = 1;
            double textLayerHeight = 7;

            // Scale

            double scaleArrowHeight = scaleService.GetOriginValueOfScale(selScale, selArrowHeight);
            double scaleArrowWidth = scaleService.GetOriginValueOfScale(selScale, selArrowWidth);
            double scaleTextHeight = scaleService.GetOriginValueOfScale(selScale, textHeight);
            double scaleTextGap = scaleService.GetOriginValueOfScale(selScale, textGap);
            double scaleTextLayerHeight = scaleService.GetOriginValueOfScale(selScale, textLayerHeight);

            //if (selTextHeight != "")
            //    textHeight = valueService.GetDoubleValue(selTextHeight);
            //if (selLayerHeight != "")
            //    textLayerHeight = valueService.GetDoubleValue(selLayerHeight);


            //textLayerHeight = 7 * textHeight / 2.5;
            //textGap = textLayerHeight / 7;


            //selArrowHeight = textHeight;
            //selArrowHeight = textHeight/2;

            Line newLeaderLine = null;
            Triangle newLeaderArrow = null;
            Point3D centerPoint = null;
            double currentTextLayerHeight = 0;

            switch (selPostion)
            {
                case "topright":

                    calDegree = 60;

                    newLeaderLine = new Line(selPoint1.X, selPoint1.Y, selPoint1.X + distance, selPoint1.Y);
                    newLeaderLine.Rotate(Utility.DegToRad(calDegree), Vector3D.AxisZ, new Point3D(selPoint1.X, selPoint1.Y));

                    // arrow
                    newLeaderArrow = new Triangle(new Point3D(selPoint1.X, selPoint1.Y), new Point3D(selPoint1.X + scaleArrowWidth, selPoint1.Y - scaleArrowHeight / 2), new Point3D(selPoint1.X + scaleArrowWidth, selPoint1.Y + scaleArrowHeight / 2));
                    newLeaderArrow.Rotate(Utility.DegToRad(calDegree), Vector3D.AxisZ, new Point3D(selPoint1.X, selPoint1.Y));

                    centerPoint = newLeaderLine.EndPoint;
                    currentTextLayerHeight = 0;
                    foreach (string eachString in selText)
                    {
                        // Text
                        Text newText01 = new Text(new Point3D(centerPoint.X + scaleTextGap, centerPoint.Y + currentTextLayerHeight + scaleTextGap), eachString, scaleTextHeight);
                        newText01.Regen(new RegenParams(0, ssModel));
                        newText01.Alignment = Text.alignmentType.BaselineLeft;

                        // base Line
                        Line newLieDefault = new Line(new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight), new Point3D(centerPoint.X + newText01.BoxSize.X + scaleTextGap*2, centerPoint.Y + currentTextLayerHeight));
                        leaderText.Add(newText01);
                        leaderLine.Add(newLieDefault);

                        // Vertical LIne
                        if (currentTextLayerHeight > 0)
                        {
                            Line newVertialLine = new Line(new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight - scaleTextLayerHeight), new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight));
                            leaderLine.Add(newVertialLine);
                        }

                        currentTextLayerHeight += scaleTextLayerHeight;
                    }


                    leaderLine.Add(newLeaderLine);
                    leaderArrow.Add(newLeaderArrow);

                    break;

                case "bottomright":

                    calDegree = -60;

                    newLeaderLine = new Line(selPoint1.X, selPoint1.Y, selPoint1.X + distance, selPoint1.Y);
                    newLeaderLine.Rotate(Utility.DegToRad(calDegree), Vector3D.AxisZ, new Point3D(selPoint1.X, selPoint1.Y));

                    // arrow
                    newLeaderArrow = new Triangle(new Point3D(selPoint1.X, selPoint1.Y), new Point3D(selPoint1.X + scaleArrowWidth, selPoint1.Y - scaleArrowHeight / 2), new Point3D(selPoint1.X + scaleArrowWidth, selPoint1.Y + scaleArrowHeight / 2));
                    newLeaderArrow.Rotate(Utility.DegToRad(calDegree), Vector3D.AxisZ, new Point3D(selPoint1.X, selPoint1.Y));

                    centerPoint = newLeaderLine.EndPoint;
                    currentTextLayerHeight = 0;
                    foreach (string eachString in selText)
                    {
                        // Text
                        Text newText01 = new Text(new Point3D(centerPoint.X + scaleTextGap, centerPoint.Y + currentTextLayerHeight + scaleTextGap), eachString, scaleTextHeight);
                        newText01.Regen(new RegenParams(0, ssModel));
                        newText01.Alignment = Text.alignmentType.BaselineLeft;

                        // base Line
                        Line newLieDefault = new Line(new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight), new Point3D(centerPoint.X + newText01.BoxSize.X + scaleTextGap*2, centerPoint.Y + currentTextLayerHeight));
                        leaderText.Add(newText01);
                        leaderLine.Add(newLieDefault);

                        // Vertical LIne
                        if (currentTextLayerHeight < 0)
                        {
                            Line newVertialLine = new Line(new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight + scaleTextLayerHeight), new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight));
                            leaderLine.Add(newVertialLine);
                        }

                        currentTextLayerHeight -= scaleTextLayerHeight;
                    }


                    leaderLine.Add(newLeaderLine);
                    leaderArrow.Add(newLeaderArrow);

                    break;

                case "topleft":

                    calDegree = 120;

                    newLeaderLine = new Line(selPoint1.X, selPoint1.Y, selPoint1.X + distance, selPoint1.Y);
                    newLeaderLine.Rotate(Utility.DegToRad(calDegree), Vector3D.AxisZ, new Point3D(selPoint1.X, selPoint1.Y));

                    // arrow
                    newLeaderArrow = new Triangle(new Point3D(selPoint1.X, selPoint1.Y), new Point3D(selPoint1.X + scaleArrowWidth, selPoint1.Y - scaleArrowHeight / 2), new Point3D(selPoint1.X + scaleArrowWidth, selPoint1.Y + scaleArrowHeight / 2));
                    newLeaderArrow.Rotate(Utility.DegToRad(calDegree), Vector3D.AxisZ, new Point3D(selPoint1.X, selPoint1.Y));

                    centerPoint = newLeaderLine.EndPoint;
                    currentTextLayerHeight = 0;
                    foreach (string eachString in selText)
                    {
                        // Text
                        Text newText01 = new Text(new Point3D(centerPoint.X - scaleTextGap, centerPoint.Y + currentTextLayerHeight + scaleTextGap), eachString, scaleTextHeight);
                        newText01.Regen(new RegenParams(0, ssModel));
                        newText01.Alignment = Text.alignmentType.BaselineRight;

                        // base Line
                        Line newLieDefault = new Line(new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight), new Point3D(centerPoint.X - newText01.BoxSize.X - scaleTextGap*2, centerPoint.Y + currentTextLayerHeight));
                        leaderText.Add(newText01);
                        leaderLine.Add(newLieDefault);

                        // Vertical LIne
                        if (currentTextLayerHeight > 0)
                        {
                            Line newVertialLine = new Line(new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight - scaleTextLayerHeight), new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight));
                            leaderLine.Add(newVertialLine);
                        }

                        currentTextLayerHeight += scaleTextLayerHeight;
                    }


                    leaderLine.Add(newLeaderLine);
                    leaderArrow.Add(newLeaderArrow);

                    break;


                case "bottomleft":

                    calDegree = -120;

                    newLeaderLine = new Line(selPoint1.X, selPoint1.Y, selPoint1.X + distance, selPoint1.Y);
                    newLeaderLine.Rotate(Utility.DegToRad(calDegree), Vector3D.AxisZ, new Point3D(selPoint1.X, selPoint1.Y));

                    // arrow
                    newLeaderArrow = new Triangle(new Point3D(selPoint1.X, selPoint1.Y), new Point3D(selPoint1.X + scaleArrowWidth , selPoint1.Y - scaleArrowHeight / 2), new Point3D(selPoint1.X + scaleArrowWidth, selPoint1.Y + scaleArrowHeight / 2));
                    newLeaderArrow.Rotate(Utility.DegToRad(calDegree), Vector3D.AxisZ, new Point3D(selPoint1.X, selPoint1.Y));

                    centerPoint = newLeaderLine.EndPoint;
                    currentTextLayerHeight = 0;
                    foreach (string eachString in selText)
                    {
                        // Text
                        Text newText01 = new Text(new Point3D(centerPoint.X - scaleTextGap, centerPoint.Y + currentTextLayerHeight + scaleTextGap), eachString, scaleTextHeight);
                        newText01.Regen(new RegenParams(0, ssModel));
                        newText01.Alignment = Text.alignmentType.BaselineRight;

                        // base Line
                        Line newLieDefault = new Line(new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight), new Point3D(centerPoint.X - newText01.BoxSize.X - scaleTextGap * 2, centerPoint.Y + currentTextLayerHeight));
                        leaderText.Add(newText01);
                        leaderLine.Add(newLieDefault);

                        // Vertical LIne
                        if (currentTextLayerHeight < 0)
                        {
                            Line newVertialLine = new Line(new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight + scaleTextLayerHeight), new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight));
                            leaderLine.Add(newVertialLine);
                        }

                        currentTextLayerHeight -= scaleTextLayerHeight;
                    }


                    leaderLine.Add(newLeaderLine);
                    leaderArrow.Add(newLeaderArrow);

                    break;

            }



            Dictionary<string, List<Entity>> customEntityList = new Dictionary<string, List<Entity>>();

            styleSerivce.SetLayerListEntity(ref leaderLine, selLayerName);
            styleSerivce.SetLayerListEntity(ref leaderArrow, selLayerName);
            styleSerivce.SetLayerListTextEntity(ref leaderText, selLayerName);

            customEntityList.Add(CommonGlobal.LeaderLine, leaderLine);
            customEntityList.Add(CommonGlobal.LeaderText, leaderText);
            customEntityList.Add(CommonGlobal.LeaderArrow, leaderArrow);

            return customEntityList;
        }

    }
}

