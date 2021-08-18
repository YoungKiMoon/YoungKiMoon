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
        private LayerStyleService layerService;

        private DrawEditingService editingService;


        public DrawService(AssemblyModel selAssembly)
        {
            assemblyData = selAssembly;
            cpService = new DrawWorkingPointService(selAssembly);

            valueService = new ValueService();
            scaleService = new DrawScaleService();
            styleSerivce = new StyleFunctionService();
            layerService = new LayerStyleService();
            editingService = new DrawEditingService();

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
        public DrawEntityModel Draw_Dimension(CDPoint selPoint1, CDPoint selPoint2, CDPoint selPoint3,
                                    string selPosition, double selDimHeight, double selTextHeight, double selTextGap, double selArrowSize,
                                    string selTextPrefix, string selTextSuffix, string selTextUserInput,
                                    double selRotate,
                                    double selScale,
                                    string selLayerName,
                                    bool leftArrowVisible = true,
                                    bool rightArrowVisible = true,
                                    bool extVisible = true,
                                    double middleValue=0
                                    )
        {

            Point3D textCenter = new Point3D();

            // Origin
            double selArrowHeight = 0.8;
            double selArrowWidth = 2.5;
        
            double extLine1 = 1;
            double extLine2 = 1;
            double pointGapLine1 = 1;
            double pointGapLine2 = 1;

            double textGap = 1;


            // Default : Force
            selTextHeight = 2.5;

            // 겹치지 않게
            double arrowDetail = 0.01;

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

            double middleDistanceH = (selPoint2.X - selPoint1.X) / 2;
            double middleDistanceV = (selPoint2.Y - selPoint1.Y) / 2;



                

            // Top
            switch (selPosition)
            {
                case "top":
                    textCenter.X = selPoint1.X + middleDistanceH + (middleDistanceH * middleValue/100);
                    textCenter.Y = Math.Min(selPoint1.Y, selPoint2.Y) + selDimHeight;
                    selDimtext = Point3D.Distance(new Point3D(selPoint1.X, textCenter.Y), new Point3D(selPoint2.X, textCenter.Y)).ToString();

                    arrowLine = new Line(new Point3D(selPoint1.X + arrowDetail, textCenter.Y), new Point3D(selPoint2.X - arrowDetail, textCenter.Y));
                    dimLine1 = new Line(new Point3D(selPoint1.X, selPoint1.Y + scalePointGapLine1), new Point3D(selPoint1.X, textCenter.Y + scaleExtLine1));
                    dimLine2 = new Line(new Point3D(selPoint2.X, selPoint2.Y + scalePointGapLine2), new Point3D(selPoint2.X, textCenter.Y + scaleExtLine2));
                    dimText = new Text(new Point3D(textCenter.X, textCenter.Y + scaleTextGap), selDimtext, scaleTextHeight);
                    dimText.Alignment = Text.alignmentType.BaselineCenter;
                    
                    tri1 = new Triangle(new Point3D(selPoint1.X, textCenter.Y), new Point3D(selPoint1.X + scaleArrowWidth, textCenter.Y + scaleArrowHeight / 2), new Point3D(selPoint1.X + scaleArrowWidth, textCenter.Y - scaleArrowHeight / 2));
                    tri2 = new Triangle(new Point3D(selPoint2.X, textCenter.Y), new Point3D(selPoint2.X - scaleArrowWidth, textCenter.Y + scaleArrowHeight / 2), new Point3D(selPoint2.X - scaleArrowWidth, textCenter.Y - scaleArrowHeight / 2));

                    break;

                case "bottom":
                    textCenter.X = selPoint1.X + middleDistanceH + (middleDistanceH * middleValue / 100);
                    textCenter.Y = Math.Max(selPoint1.Y, selPoint2.Y) - selDimHeight;
                    selDimtext = Point3D.Distance(new Point3D(selPoint1.X, textCenter.Y), new Point3D(selPoint2.X, textCenter.Y)).ToString();

                    arrowLine = new Line(new Point3D(selPoint1.X+ arrowDetail, textCenter.Y), new Point3D(selPoint2.X - arrowDetail, textCenter.Y));
                    dimLine1 = new Line(new Point3D(selPoint1.X, selPoint1.Y - scalePointGapLine1), new Point3D(selPoint1.X, textCenter.Y - scaleExtLine1));
                    dimLine2 = new Line(new Point3D(selPoint2.X, selPoint2.Y - scalePointGapLine2), new Point3D(selPoint2.X, textCenter.Y - scaleExtLine2));
                    dimText = new Text(new Point3D(textCenter.X, textCenter.Y + scaleTextGap), selDimtext, scaleTextHeight);
                    dimText.Alignment = Text.alignmentType.BaselineCenter;
                    tri1 = new Triangle(new Point3D(selPoint1.X, textCenter.Y), new Point3D(selPoint1.X + scaleArrowWidth, textCenter.Y + scaleArrowHeight / 2), new Point3D(selPoint1.X + scaleArrowWidth, textCenter.Y - scaleArrowHeight / 2));
                    tri2 = new Triangle(new Point3D(selPoint2.X, textCenter.Y), new Point3D(selPoint2.X - scaleArrowWidth, textCenter.Y + scaleArrowHeight / 2), new Point3D(selPoint2.X - scaleArrowWidth, textCenter.Y - scaleArrowHeight / 2));

                    break;


                case "left":
                    textCenter.X = Math.Max(selPoint1.X, selPoint2.X) - selDimHeight;
                    textCenter.Y = selPoint1.Y + middleDistanceV +(middleDistanceV*middleValue/100);
                    selDimtext = Point3D.Distance(new Point3D(textCenter.X, selPoint1.Y), new Point3D(textCenter.X, selPoint2.Y)).ToString();

                    arrowLine = new Line(new Point3D(textCenter.X, selPoint1.Y+ arrowDetail), new Point3D(textCenter.X, selPoint2.Y- arrowDetail));
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
                    textCenter.Y = selPoint1.Y + middleDistanceV + (middleDistanceV * middleValue / 100);
                    selDimtext = Point3D.Distance(new Point3D(textCenter.X, selPoint1.Y), new Point3D(textCenter.X, selPoint2.Y)).ToString();

                    arrowLine = new Line(new Point3D(textCenter.X, selPoint1.Y+ arrowDetail), new Point3D(textCenter.X, selPoint2.Y- arrowDetail));
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
            if(leftArrowVisible)
                dimArrowList.Add(tri1);
            if(rightArrowVisible)
                dimArrowList.Add(tri2);

            dimTextList.Add(dimText);

            if (extVisible)
                dimlineExtList.Add(dimLine1);
            if (extVisible)
                dimlineExtList.Add(dimLine2);


            DrawEntityModel customEntityList = new DrawEntityModel();

            // Layer
            styleSerivce.SetLayerListEntity(ref dimlineList, selLayerName);
            styleSerivce.SetLayerListEntity(ref dimlineExtList, selLayerName);
            styleSerivce.SetLayerListEntity(ref dimArrowList, selLayerName);
            styleSerivce.SetLayerListTextEntity(ref dimTextList, selLayerName);


            customEntityList.dimlineList.AddRange(dimlineList);
            customEntityList.dimTextList.AddRange(dimTextList);
            customEntityList.dimlineExtList.AddRange(dimlineExtList);
            customEntityList.dimArrowList.AddRange(dimArrowList);


            return customEntityList;
        }

        public DrawEntityModel Draw_DimensionDetail(ref Model refSingleModel, Point3D selPoint1, Point3D selPoint2, double selScale, DrawDimensionModel selDimModel)
        {

            Point3D textCenter = new Point3D();

            // Scale
            double scaleArrowHeight = scaleService.GetOriginValueOfScale(selScale, selDimModel.arrowHeadHeight);
            double scaleArrowWidth = scaleService.GetOriginValueOfScale(selScale, selDimModel.arrowHeadWidth);
            double scaleArrowExtLength = scaleService.GetOriginValueOfScale(selScale, selDimModel.arrowExtLength);
            double scaleCircleRadius = scaleService.GetOriginValueOfScale(selScale, selDimModel.circleRadius);

            double scaleExtLine1 = scaleService.GetOriginValueOfScale(selScale, selDimModel.extLineLength1);
            double scaleExtLine2 = scaleService.GetOriginValueOfScale(selScale, selDimModel.extLineLength2);
            double scalePointGapLine1 = scaleService.GetOriginValueOfScale(selScale, selDimModel.extLinePointGap1);
            double scalePointGapLine2 = scaleService.GetOriginValueOfScale(selScale, selDimModel.extLinePointGap2);

            double scaleTextGap = scaleService.GetOriginValueOfScale(selScale, selDimModel.textGap);
            double scaleTextSideGap = scaleService.GetOriginValueOfScale(selScale, selDimModel.textSideGap);
            double scaleTextHeight = scaleService.GetOriginValueOfScale(selScale, selDimModel.textHeight);

            double middleValue = selDimModel.textMiddleValue;
            double dimHeight= scaleService.GetOriginValueOfScale(selScale, selDimModel.dimHeight);

            double scaleLineBreakLength= scaleService.GetOriginValueOfScale(selScale, 1);

            string selDimtextUpper = selDimModel.textUpper;
            string selDimtextLower = selDimModel.textLower;

            string dimSizeTextUpper = selDimtextUpper;
            string dimSizeTextLower = selDimtextLower;

            Line dimLine1 = null;
            Line dimLine2 = null;
            Line arrowLine = null;
            Triangle tri1 = null;
            Triangle tri2 = null;
            Circle cir1 = null;
            Circle cir2 = null;
            Text dimTextUpper = null;
            Text dimTextLower = null;

            List<Line> dimSplitLine1 = new List<Line>();
            List<Line> dimSplitLine2 = new List<Line>();

            double middleDistanceH = (selPoint2.X - selPoint1.X) / 2;
            double middleDistanceV = (selPoint2.Y - selPoint1.Y) / 2;


            // 간격 좁은지 확인
            double minWidth = selDimModel.arrowHeadWidth * 2; ;

            // 겹치지 않게
            double arrowDetail = 0.01;

            // Arrow Line Extend
            double arrowLineLeftExtendLength = 0;
            double arrowLineRightExtendLength = 0;

            // Top
            switch (selDimModel.position)
            {
                case POSITION_TYPE.TOP:
                    // Text Gap
                    if (Math.Abs(selPoint1.X - selPoint2.X) <= minWidth)
                        scaleTextGap = scaleTextGap * 2;

                    textCenter.X = selPoint1.X + middleDistanceH + (middleDistanceH * middleValue / 100);
                    textCenter.Y = Math.Min(selPoint1.Y, selPoint2.Y) + dimHeight;
                    arrowLine = new Line(new Point3D(selPoint1.X + arrowDetail, textCenter.Y), new Point3D(selPoint2.X - arrowDetail, textCenter.Y));
                    dimLine1 = new Line(new Point3D(selPoint1.X, selPoint1.Y + scalePointGapLine1), new Point3D(selPoint1.X, textCenter.Y + scaleExtLine1));
                    dimLine2 = new Line(new Point3D(selPoint2.X, selPoint2.Y + scalePointGapLine2), new Point3D(selPoint2.X, textCenter.Y + scaleExtLine2));
                    if (selDimModel.textSizeVisible)
                    {
                        double dimDistance = Point3D.Distance(new Point3D(selPoint1.X, textCenter.Y), new Point3D(selPoint2.X, textCenter.Y));
                        dimSizeTextUpper = Math.Round(dimDistance, selDimModel.textRoundNumber, MidpointRounding.AwayFromZero).ToString();
                    }
                    dimTextUpper = new Text(new Point3D(textCenter.X, textCenter.Y + scaleTextGap), dimSizeTextUpper, scaleTextHeight) {Alignment=Text.alignmentType.BaselineCenter };
                    dimTextLower= new Text(new Point3D(textCenter.X, textCenter.Y - scaleTextGap), selDimtextLower, scaleTextHeight) { Alignment = Text.alignmentType.TopCenter };

                    if (selDimModel.arrowLeftSymbol == DimHead_Type.Arrow)
                    {
                        tri1 = new Triangle(new Point3D(selPoint1.X, textCenter.Y), new Point3D(selPoint1.X + scaleArrowWidth, textCenter.Y + scaleArrowHeight / 2), new Point3D(selPoint1.X + scaleArrowWidth, textCenter.Y - scaleArrowHeight / 2));
                        if (selDimModel.arrowLeftHeadOut)
                        {
                            editingService.SetMirrorEntity(Plane.YZ, ref tri1, selPoint1);
                            arrowLineLeftExtendLength = scaleArrowWidth + scaleArrowExtLength;
                        }
                    }
                    else if (selDimModel.arrowLeftSymbol == DimHead_Type.Circle)
                    {
                        cir1 = new Circle(new Point3D(selPoint1.X, textCenter.Y), scaleCircleRadius);
                    }

                    if (selDimModel.arrowRightSymbol == DimHead_Type.Arrow)
                    {
                        tri2 = new Triangle(new Point3D(selPoint2.X, textCenter.Y), new Point3D(selPoint2.X - scaleArrowWidth, textCenter.Y + scaleArrowHeight / 2), new Point3D(selPoint2.X - scaleArrowWidth, textCenter.Y - scaleArrowHeight / 2));
                        if (selDimModel.arrowRightHeadOut)
                        {
                            editingService.SetMirrorEntity(Plane.YZ, ref tri2, selPoint2);
                            arrowLineRightExtendLength = scaleArrowWidth + scaleArrowExtLength;
                        }
                    }
                    else if (selDimModel.arrowRightSymbol == DimHead_Type.Circle)
                    {
                        cir2 = new Circle(new Point3D(selPoint2.X, textCenter.Y), scaleCircleRadius);
                    }

                    // Text Position
                    switch(selDimModel.textUpperPosition)
                    {
                        case POSITION_TYPE.LEFT:
                            dimTextUpper.Regen(new RegenParams(0, refSingleModel));
                            arrowLineLeftExtendLength = scaleTextSideGap + dimTextUpper.BoxSize.X + scaleTextSideGap;
                            dimTextUpper.InsertionPoint.X = selPoint1.X  - arrowLineLeftExtendLength / 2;
                            if (selDimModel.arrowLeftSymbol == DimHead_Type.Arrow)
                            {
                                dimTextUpper.InsertionPoint.X -= scaleArrowWidth;
                                arrowLineLeftExtendLength += scaleArrowWidth;
                            }
                            break;
                        case POSITION_TYPE.RIGHT:
                            dimTextUpper.Regen(new RegenParams(0, refSingleModel));
                            arrowLineRightExtendLength =  scaleTextSideGap + dimTextUpper.BoxSize.X + scaleTextSideGap;
                            dimTextUpper.InsertionPoint.X = selPoint2.X + arrowLineRightExtendLength / 2;
                            if (selDimModel.arrowRightSymbol == DimHead_Type.Arrow)
                            {
                                dimTextUpper.InsertionPoint.X += scaleArrowWidth;
                                arrowLineRightExtendLength += scaleArrowWidth;
                            }
                            break;
                    }
                    break;

                case POSITION_TYPE.BOTTOM:
                    // Text Gap
                    if (Math.Abs(selPoint1.X - selPoint2.X) <= minWidth)
                        scaleTextGap = scaleTextGap * 2;

                    textCenter.X = selPoint1.X + middleDistanceH + (middleDistanceH * middleValue / 100);
                    textCenter.Y = Math.Max(selPoint1.Y, selPoint2.Y) - dimHeight;

                    arrowLine = new Line(new Point3D(selPoint1.X + arrowDetail, textCenter.Y), new Point3D(selPoint2.X - arrowDetail, textCenter.Y));
                    dimLine1 = new Line(new Point3D(selPoint1.X, selPoint1.Y - scalePointGapLine1), new Point3D(selPoint1.X, textCenter.Y - scaleExtLine1));
                    dimLine2 = new Line(new Point3D(selPoint2.X, selPoint2.Y - scalePointGapLine2), new Point3D(selPoint2.X, textCenter.Y - scaleExtLine2));

                    if (selDimModel.textSizeVisible)
                    {
                        double dimDistance = Point3D.Distance(new Point3D(selPoint1.X, textCenter.Y), new Point3D(selPoint2.X, textCenter.Y));
                        dimSizeTextUpper = Math.Round(dimDistance, selDimModel.textRoundNumber, MidpointRounding.AwayFromZero).ToString();
                    }
                    dimTextUpper = new Text(new Point3D(textCenter.X, textCenter.Y + scaleTextGap), dimSizeTextUpper, scaleTextHeight) { Alignment = Text.alignmentType.BaselineCenter };
                    dimTextLower = new Text(new Point3D(textCenter.X, textCenter.Y - scaleTextGap), selDimtextLower, scaleTextHeight) { Alignment = Text.alignmentType.TopCenter };


                    tri1 = new Triangle(new Point3D(selPoint1.X, textCenter.Y), new Point3D(selPoint1.X + scaleArrowWidth, textCenter.Y + scaleArrowHeight / 2), new Point3D(selPoint1.X + scaleArrowWidth, textCenter.Y - scaleArrowHeight / 2));
                    tri2 = new Triangle(new Point3D(selPoint2.X, textCenter.Y), new Point3D(selPoint2.X - scaleArrowWidth, textCenter.Y + scaleArrowHeight / 2), new Point3D(selPoint2.X - scaleArrowWidth, textCenter.Y - scaleArrowHeight / 2));

                    break;


                case POSITION_TYPE.LEFT:
                    // Text Gap
                    if (Math.Abs(selPoint1.Y - selPoint2.Y) <= minWidth)
                        scaleTextGap = scaleTextGap * 2;

                    textCenter.X = Math.Max(selPoint1.X, selPoint2.X) - dimHeight;
                    textCenter.Y = selPoint1.Y + middleDistanceV + (middleDistanceV * middleValue / 100);

                    arrowLine = new Line(new Point3D(textCenter.X, selPoint1.Y + arrowDetail), new Point3D(textCenter.X, selPoint2.Y - arrowDetail));
                    dimLine1 = new Line(new Point3D(selPoint1.X - scalePointGapLine1, selPoint1.Y), new Point3D(textCenter.X - scaleExtLine1, selPoint1.Y));
                    dimLine2 = new Line(new Point3D(selPoint2.X - scalePointGapLine2, selPoint2.Y), new Point3D(textCenter.X - scaleExtLine2, selPoint2.Y));

                    if (selDimModel.textSizeVisible)
                    {
                        double dimDistance = Point3D.Distance(new Point3D(textCenter.X, selPoint1.Y), new Point3D(textCenter.X, selPoint2.Y));
                        dimSizeTextUpper = Math.Round(dimDistance, selDimModel.textRoundNumber, MidpointRounding.AwayFromZero).ToString();
                    }
                    Plane planeLeft = new Plane(new Point3D(selPoint1.X, selPoint1.Y), Vector3D.AxisY, -1 * Vector3D.AxisX);
                    dimTextUpper = new Text(planeLeft, new Point3D(textCenter.X - scaleTextGap, textCenter.Y), dimSizeTextUpper, scaleTextHeight) { Alignment = Text.alignmentType.BaselineCenter };
                    dimTextLower = new Text(planeLeft, new Point3D(textCenter.X + scaleTextGap, textCenter.Y), selDimtextLower, scaleTextHeight) { Alignment = Text.alignmentType.TopCenter };

                    //dimText.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
                    tri1 = new Triangle(new Point3D(textCenter.X, selPoint1.Y), new Point3D(textCenter.X - scaleArrowHeight / 2, selPoint1.Y + scaleArrowWidth), new Point3D(textCenter.X + scaleArrowHeight / 2, selPoint1.Y + scaleArrowWidth));
                    tri2 = new Triangle(new Point3D(textCenter.X, selPoint2.Y), new Point3D(textCenter.X - scaleArrowHeight / 2, selPoint2.Y - scaleArrowWidth), new Point3D(textCenter.X + scaleArrowHeight / 2, selPoint2.Y - scaleArrowWidth));

                    break;

                case POSITION_TYPE.RIGHT:
                    // Text Gap
                    if (Math.Abs(selPoint1.Y - selPoint2.Y) <= minWidth)
                        scaleTextGap = scaleTextGap * 2;

                    textCenter.X = Math.Min(selPoint1.X, selPoint2.X) + dimHeight;
                    textCenter.Y = selPoint1.Y + middleDistanceV + (middleDistanceV * middleValue / 100);

                    arrowLine = new Line(new Point3D(textCenter.X, selPoint1.Y + arrowDetail), new Point3D(textCenter.X, selPoint2.Y - arrowDetail));
                    dimLine1 = new Line(new Point3D(selPoint1.X + scalePointGapLine1, selPoint1.Y), new Point3D(textCenter.X + scaleExtLine1, selPoint1.Y));
                    dimLine2 = new Line(new Point3D(selPoint2.X + scalePointGapLine2, selPoint2.Y), new Point3D(textCenter.X + scaleExtLine2, selPoint2.Y));

                    if (selDimModel.textSizeVisible)
                    {
                        double dimDistance = Point3D.Distance(new Point3D(textCenter.X, selPoint1.Y), new Point3D(textCenter.X, selPoint2.Y));
                        dimSizeTextUpper = Math.Round(dimDistance, selDimModel.textRoundNumber, MidpointRounding.AwayFromZero).ToString();
                    }

                    Plane planeLeft2 = new Plane(new Point3D(selPoint1.X, selPoint1.Y), Vector3D.AxisY, -1 * Vector3D.AxisX);
                    dimTextUpper = new Text(planeLeft2, new Point3D(textCenter.X - scaleTextGap, textCenter.Y), dimSizeTextUpper, scaleTextHeight) { Alignment = Text.alignmentType.BaselineCenter };
                    dimTextLower = new Text(planeLeft2, new Point3D(textCenter.X + scaleTextGap, textCenter.Y), selDimtextLower, scaleTextHeight) { Alignment = Text.alignmentType.TopCenter };

                    //dimText.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
                    tri1 = new Triangle(new Point3D(textCenter.X, selPoint1.Y), new Point3D(textCenter.X - scaleArrowHeight / 2, selPoint1.Y + scaleArrowWidth), new Point3D(textCenter.X + scaleArrowHeight / 2, selPoint1.Y + scaleArrowWidth));
                    tri2 = new Triangle(new Point3D(textCenter.X, selPoint2.Y), new Point3D(textCenter.X - scaleArrowHeight / 2, selPoint2.Y - scaleArrowWidth), new Point3D(textCenter.X + scaleArrowHeight / 2, selPoint2.Y - scaleArrowWidth));

                    break;



            }


            // Arrow Line Extend
            if (arrowLineLeftExtendLength > 0)
                editingService.SetExtendLine(ref arrowLine, arrowLineLeftExtendLength, false);

            if (arrowLineRightExtendLength > 0)
                editingService.SetExtendLine(ref arrowLine, arrowLineRightExtendLength, true);


            List<Entity> dimlineList = new List<Entity>();
            List<Entity> dimTextList = new List<Entity>();
            List<Entity> dimlineExtList = new List<Entity>();
            List<Entity> dimArrowList = new List<Entity>();


            // Text
            if (selDimModel.textSizeVisible)
            {
                dimTextList.Add(dimTextUpper);
            }
            else
            {
                if (selDimtextUpper != "")
                    dimTextList.Add(dimTextUpper);
                if (selDimtextLower != "")
                    dimTextList.Add(dimTextLower);

            }

            // Text Break
            if (selDimtextLower != "")
            {

                // TextBox Margin
                Point3D[] tempVer = null;
                Point3D dimLine1StartPoint = null;
                Point3D dimLine1EndPoint = null;
                Point3D dimLine2StartPoint = null;
                Point3D dimLine2EndPoint = null;
                switch (selDimModel.position)
                {
                    case POSITION_TYPE.LEFT:
                        dimTextLower.Regen(new RegenParams(0, refSingleModel));
                        tempVer = dimTextLower.Vertices;
                        tempVer[0].X += scaleLineBreakLength;
                        tempVer[1].X += scaleLineBreakLength;
                        dimLine1StartPoint = dimLine1.StartPoint;
                        dimLine1EndPoint = dimLine1.EndPoint;
                        dimLine2StartPoint = dimLine2.StartPoint;
                        dimLine2EndPoint = dimLine2.EndPoint;
                        break;
                    case POSITION_TYPE.TOP:
                        dimTextLower.Regen(new RegenParams(0, refSingleModel));
                        tempVer = dimTextLower.Vertices;
                        tempVer[0].Y -= scaleLineBreakLength;
                        tempVer[1].Y -= scaleLineBreakLength;
                        dimLine1StartPoint = dimLine1.StartPoint;
                        dimLine1EndPoint = dimLine1.EndPoint;
                        dimLine2StartPoint = dimLine2.StartPoint;
                        dimLine2EndPoint = dimLine2.EndPoint;
                        break;
                    case POSITION_TYPE.RIGHT:
                        dimTextUpper.Regen(new RegenParams(0, refSingleModel));
                        tempVer = dimTextUpper.Vertices;
                        tempVer[2].X -= scaleLineBreakLength;
                        tempVer[3].X -= scaleLineBreakLength;
                        dimLine1StartPoint = dimLine1.EndPoint;
                        dimLine1EndPoint = dimLine1.StartPoint;
                        dimLine2StartPoint = dimLine2.EndPoint;
                        dimLine2EndPoint = dimLine2.StartPoint;
                        break;
                    case POSITION_TYPE.BOTTOM:
                        dimTextUpper.Regen(new RegenParams(0, refSingleModel));
                        tempVer = dimTextUpper.Vertices;
                        tempVer[2].Y += scaleLineBreakLength;
                        tempVer[3].Y += scaleLineBreakLength;
                        dimLine1StartPoint = dimLine1.EndPoint;
                        dimLine1EndPoint = dimLine1.StartPoint;
                        dimLine2StartPoint = dimLine2.EndPoint;
                        dimLine2EndPoint = dimLine2.StartPoint;
                        break;
                }


                Point3D[] leftUpperInter = dimLine1.IntersectWith(new Line(tempVer[2], tempVer[3]));
                if (leftUpperInter.Length > 0)
                    dimSplitLine1.Add(new Line(dimLine1EndPoint, leftUpperInter[0]));
                Point3D[] leftLowerInter = dimLine1.IntersectWith(new Line(tempVer[0], tempVer[1]));
                if (leftLowerInter.Length > 0)
                    dimSplitLine1.Add(new Line(dimLine1StartPoint, leftLowerInter[0]));
                Point3D[] rightUpperInter = dimLine2.IntersectWith(new Line(tempVer[2], tempVer[3]));
                if (rightUpperInter.Length > 0)
                    dimSplitLine2.Add(new Line(dimLine2EndPoint, rightUpperInter[0]));
                Point3D[] rightLowerInter = dimLine2.IntersectWith(new Line(tempVer[0], tempVer[1]));
                if (rightLowerInter.Length > 0)
                    dimSplitLine2.Add(new Line(dimLine2StartPoint, rightLowerInter[0]));

                


            }


            // Arrow Line
            dimlineList.Add(arrowLine);

            // Arrow Symbol
            if (selDimModel.arrowLeftHeadVisible)
            {
                if (selDimModel.arrowLeftSymbol == DimHead_Type.Arrow)
                    dimArrowList.Add(tri1);
                else if (selDimModel.arrowLeftSymbol == DimHead_Type.Circle)
                    dimArrowList.Add(cir1);

            }
            if (selDimModel.arrowRightHeadVisible)
            {
                if (selDimModel.arrowRightSymbol == DimHead_Type.Arrow)
                    dimArrowList.Add(tri2);
                else if (selDimModel.arrowRightSymbol == DimHead_Type.Circle)
                    dimArrowList.Add(cir2);
            }

            // Ext Line
            if (selDimModel.extLineLeftVisible)
            {
                if (dimSplitLine1.Count > 0)
                    dimlineExtList.AddRange(dimSplitLine1);
                else
                    dimlineExtList.Add(dimLine1);
            }
            if (selDimModel.extLineRightVisible)
            {
                if (dimSplitLine2.Count > 0)
                    dimlineExtList.AddRange(dimSplitLine2);
                else
                    dimlineExtList.Add(dimLine2);
            }

            // BMNumber
            Circle leftBMCircle = null;
            Circle rightBMCircle = null;
            Text leftBMText = null;
            Text rightBMText = null;
            DrawBMNumberModel dbNumberModel = new DrawBMNumberModel();
            if (selDimModel.leftBMNumber != "") 
            {
                double scaleBMCircleRadius= scaleService.GetOriginValueOfScale(selScale, dbNumberModel.circleRadius);
                double scaleBMTextHeight = scaleService.GetOriginValueOfScale(selScale, dbNumberModel.textHeight);
                Line tempBMLine = editingService.GetExtendLine(arrowLine, scaleBMCircleRadius, false);
                leftBMCircle = new Circle(tempBMLine.StartPoint, scaleBMCircleRadius);
                leftBMText = new Text(tempBMLine.StartPoint, selDimModel.leftBMNumber, scaleBMTextHeight) {Alignment=Text.alignmentType.MiddleCenter };
                dimArrowList.Add(leftBMCircle);
                dimArrowList.Add(leftBMText);
            }
            if (selDimModel.rightBMNumber != "")
            {
                double scaleBMCircleRadius = scaleService.GetOriginValueOfScale(selScale, dbNumberModel.circleRadius);
                double scaleBMTextHeight = scaleService.GetOriginValueOfScale(selScale, dbNumberModel.textHeight);
                Line tempBMLine = editingService.GetExtendLine(arrowLine, scaleBMCircleRadius, true);
                rightBMCircle = new Circle(tempBMLine.EndPoint, scaleBMCircleRadius);
                rightBMText = new Text(tempBMLine.EndPoint, selDimModel.rightBMNumber, scaleBMTextHeight) { Alignment = Text.alignmentType.MiddleCenter };
                dimArrowList.Add(rightBMCircle);
                dimArrowList.Add(rightBMText);
            }


            DrawEntityModel customEntityList = new DrawEntityModel();

            // Layer
            styleSerivce.SetLayerListEntity(ref dimlineList, layerService.LayerDimension);
            styleSerivce.SetLayerListEntity(ref dimlineExtList, layerService.LayerDimension);
            styleSerivce.SetLayerListEntity(ref dimArrowList, layerService.LayerDimension);
            styleSerivce.SetLayerListTextEntity(ref dimTextList, layerService.LayerDimension);


            customEntityList.dimlineList.AddRange(dimlineList);
            customEntityList.dimTextList.AddRange(dimTextList);
            customEntityList.dimlineExtList.AddRange(dimlineExtList);
            customEntityList.dimArrowList.AddRange(dimArrowList);

            return customEntityList;
        }


        public DrawEntityModel Draw_DimensionArc(Point3D selPoint1, Point3D selPoint2, 
                                    string selPosition, double selDimHeight, 
                                    string selTextUserInput,
                                    double selDegree,
                                    double ext1Degree,
                                    double ext2Degree,
                                    double selRotate, 
                                    double selScale,
                                    string selLayerName,
                                    bool leftArrowVisible = true,
                                    bool rightArrowVisible = true,
                                    bool extVisible = true,
                                    double middleValue = 0
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

            string selTextPrefix = "";
            string selTextSuffix = "";

            // Default : Force
            double selTextHeight = 2.5;



            // Scale
            double scaleArrowHeight = scaleService.GetOriginValueOfScale(selScale, selArrowHeight);
            double scaleArrowWidth = scaleService.GetOriginValueOfScale(selScale, selArrowWidth);

            double scaleExtLine1 = scaleService.GetOriginValueOfScale(selScale, extLine1);
            double scaleExtLine2 = scaleService.GetOriginValueOfScale(selScale, extLine2);
            double scalePointGapLine1 = scaleService.GetOriginValueOfScale(selScale, pointGapLine1);
            double scalePointGapLine2 = scaleService.GetOriginValueOfScale(selScale, pointGapLine2);

            double scaleTextGap = scaleService.GetOriginValueOfScale(selScale, textGap);

            double scaleTextHeight = scaleService.GetOriginValueOfScale(selScale, selTextHeight);


            string selDimtext = "";

            Line dimLine1 = null;
            Line dimLine2 = null;
            Arc arrowLine = null;
            Triangle tri1 = null;
            Triangle tri2 = null;
            Text dimText = null;

            Line vdimLine1 = null;
            Line vdimLine2 = null;
            Line vdimLine1Half = null;
            Line vdimLine2Half = null;

            double middleDistanceH = (selPoint2.X - selPoint1.X) / 2;
            double middleDistanceV = (selPoint2.Y - selPoint1.Y) / 2;


            double dimRadian = Utility.DegToRad(selDegree);
            double ext1Radian = Utility.DegToRad(ext1Degree);
            double ext2Radian = Utility.DegToRad(ext2Degree);

            // Function
            double pointDistance = Point3D.Distance(GetSumPoint(selPoint1,0,0), GetSumPoint(selPoint2,0,0));
            double virtualExtLineLength = pointDistance * 100;

            // Text : Direction
            double textRotationAdj = 0;
            double textOffsetAdj = 0;

            // Top
            switch (selPosition)
            {
                case "top":
                    textOffsetAdj = -scaleTextGap;
                    textRotationAdj = 0;
                    break;

                case "bottom":
                    textOffsetAdj = scaleTextGap;
                    textRotationAdj = Utility.DegToRad(180);
                    break;
            }

            // Draw
            textCenter.X = Math.Max(selPoint1.X, selPoint2.X) - selDimHeight;
            textCenter.Y = selPoint1.Y + middleDistanceV + (middleDistanceV * middleValue / 100);
            selDimtext = Point3D.Distance(new Point3D(textCenter.X, selPoint1.Y), new Point3D(textCenter.X, selPoint2.Y)).ToString();

            //arrowLine = new Line(new Point3D(textCenter.X, selPoint1.Y + 1), new Point3D(textCenter.X, selPoint2.Y - 1));
            vdimLine1 = new Line(GetSumPoint(selPoint1, 0, virtualExtLineLength), GetSumPoint(selPoint1, 0, -virtualExtLineLength));
            vdimLine1.Rotate(ext1Radian, Vector3D.AxisZ, selPoint1);
            vdimLine1Half = new Line(GetSumPoint(selPoint1, 0, 0), GetSumPoint(vdimLine1.StartPoint, 0, 0));
            vdimLine2 = new Line(GetSumPoint(selPoint2, 0, virtualExtLineLength), GetSumPoint(selPoint2, 0, -virtualExtLineLength));
            vdimLine2.Rotate(ext2Radian, Vector3D.AxisZ, selPoint2);
            vdimLine2Half = new Line(GetSumPoint(selPoint2, 0, 0), GetSumPoint(vdimLine2.StartPoint, 0, 0));

            Point3D[] vDimInter = vdimLine1.IntersectWith(vdimLine2);

            if (vDimInter.Length > 0)
            {
                Point3D dimCenterPoint = vDimInter[0];
                double arcRadiusMin = dimCenterPoint.DistanceTo(selPoint1);
                if (arcRadiusMin > dimCenterPoint.DistanceTo(selPoint2))
                    arcRadiusMin = dimCenterPoint.DistanceTo(selPoint2);
                double arcRadiusMax = arcRadiusMin + selDimHeight;
                Circle circleMax = new Circle(GetSumPoint(dimCenterPoint, 0, 0), arcRadiusMax);
                Point3D dimExt1Point = editingService.GetIntersectWidth(circleMax, vdimLine1Half, 0);
                Point3D dimExt2Point = editingService.GetIntersectWidth(circleMax, vdimLine2Half, 0);
                if (dimExt1Point.X != 0 && dimExt2Point.X != 0)
                {
                    Circle circleMax2 = new Circle(GetSumPoint(dimCenterPoint, 0, 0), arcRadiusMax + scaleExtLine1);
                    Point3D dimExt1Point2 = editingService.GetIntersectWidth(circleMax2, vdimLine1Half, 0);
                    Point3D dimExt2Point2 = editingService.GetIntersectWidth(circleMax2, vdimLine2Half, 0);

                    Line tempExt1 = new Line(GetSumPoint(dimExt1Point, 0, 0), GetSumPoint(selPoint1, 0, 0));
                    Point3D tempExt1Point = editingService.GetIntersectLength(tempExt1, scalePointGapLine1);
                    Line tempExt2 = new Line(GetSumPoint(dimExt2Point, 0, 0), GetSumPoint(selPoint2, 0, 0));
                    Point3D tempExt2Point = editingService.GetIntersectLength(tempExt2, scalePointGapLine2);

                    // Final
                    dimLine1 = new Line(GetSumPoint(tempExt1Point, 0, 0), GetSumPoint(dimExt1Point2, 0, 0));
                    dimLine2 = new Line(GetSumPoint(tempExt2Point, 0, 0), GetSumPoint(dimExt2Point2, 0, 0));

                    // arrow line
                    arrowLine = new Arc(dimCenterPoint, dimExt1Point, dimExt2Point);

                    // Text
                    Line vTextLine = new Line(GetSumPoint(dimExt1Point, 0, 0), GetSumPoint(dimExt2Point, 0, 0));
                    double textAngle = editingService.GetAngleOfLine(vTextLine);
                    Arc vArctextLine = (Arc)arrowLine.Offset(textOffsetAdj, Vector3D.AxisZ);
                    Point3D textMidPoint = vArctextLine.MidPoint;
                    dimText = new Text(GetSumPoint(textMidPoint, 0, 0), selDimtext, scaleTextHeight);
                    dimText.Alignment = Text.alignmentType.BaselineCenter;
                    dimText.Rotate(textAngle + textRotationAdj , Vector3D.AxisZ, GetSumPoint(textMidPoint, 0, 0));


                    // Arrow
                    tri1 = new Triangle(GetSumPoint(dimExt1Point, 0, 0), GetSumPoint(dimExt1Point, -scaleArrowHeight / 2, scaleArrowWidth), GetSumPoint(dimExt1Point, scaleArrowHeight / 2, scaleArrowWidth));
                    Point3D tri1Point = editingService.GetIntersectLength(arrowLine, scaleArrowWidth, false);

                    double tri1Angle = Utility.DegToRad(-90) + editingService.GetAngleOfLine(new Line(GetSumPoint(dimExt1Point, 0, 0), GetSumPoint(tri1Point, 0, 0)));
                    tri1.Rotate(tri1Angle, Vector3D.AxisZ, GetSumPoint(dimExt1Point, 0, 0));


                    tri2 = new Triangle(GetSumPoint(dimExt2Point, 0, 0), GetSumPoint(dimExt2Point, -scaleArrowHeight / 2, -scaleArrowWidth), GetSumPoint(dimExt2Point, scaleArrowHeight / 2, -scaleArrowWidth));
                    Point3D tri2Point = editingService.GetIntersectLength(arrowLine, scaleArrowWidth, true);

                    double tri2Angle = Utility.DegToRad(90) + editingService.GetAngleOfLine(new Line(GetSumPoint(dimExt2Point, 0, 0), GetSumPoint(tri2Point, 0, 0)));
                    tri2.Rotate(tri2Angle, Vector3D.AxisZ, GetSumPoint(dimExt2Point, 0, 0));

                }

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
            if (leftArrowVisible)
                dimArrowList.Add(tri1);
            if (rightArrowVisible)
                dimArrowList.Add(tri2);

            dimTextList.Add(dimText);

            if (extVisible)
                dimlineExtList.Add(dimLine1);
            if (extVisible)
                dimlineExtList.Add(dimLine2);

            // Layer
            styleSerivce.SetLayerListEntity(ref dimlineList, selLayerName);
            styleSerivce.SetLayerListEntity(ref dimlineExtList, selLayerName);
            styleSerivce.SetLayerListEntity(ref dimArrowList, selLayerName);
            styleSerivce.SetLayerListTextEntity(ref dimTextList, selLayerName);



            DrawEntityModel customEntityList = new DrawEntityModel();

            customEntityList.dimlineList.AddRange(dimlineList);
            customEntityList.dimTextList.AddRange(dimTextList);
            customEntityList.dimlineExtList.AddRange(dimlineExtList);
            customEntityList.dimArrowList.AddRange(dimArrowList);


            return customEntityList;
        }

        public DrawEntityModel Draw_Leader(CDPoint selPoint1, 
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
                        styleSerivce.SetLayer(ref newText01, selLayerName);
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
                        styleSerivce.SetLayer(ref newText01, selLayerName);
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
                        styleSerivce.SetLayer(ref newText01, selLayerName);
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
                        styleSerivce.SetLayer(ref newText01, selLayerName);
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





            styleSerivce.SetLayerListEntity(ref leaderLine, selLayerName);
            styleSerivce.SetLayerListEntity(ref leaderArrow, selLayerName);
            //styleSerivce.SetLayerListTextEntity(ref leaderText, selLayerName);

            DrawEntityModel customEntityList = new DrawEntityModel();

            customEntityList.leaderlineList.AddRange(leaderLine);
            customEntityList.leaderTextList.AddRange(leaderText);
            customEntityList.leaderArrowList.AddRange(leaderArrow);


            return customEntityList;
        }



        public DrawEntityModel Draw_BMLeader(ref Model refSingleModel,Point3D selPoint,DrawBMLeaderModel selLeaderModel, double selScale)
        {
            List<Entity> leaderLine = new List<Entity>();
            List<Entity> leaderArrow = new List<Entity>();
            List<Entity> leaderText = new List<Entity>();

            double scaleArrowHeight= scaleService.GetOriginValueOfScale(selScale, selLeaderModel.arrowHeadHeight);
            double scaleArrowWidth = scaleService.GetOriginValueOfScale(selScale, selLeaderModel.arrowHeadWidth);
            double scaleCircleRadius= scaleService.GetOriginValueOfScale(selScale, selLeaderModel.bmCircle.circleRadius);
            double scaleCircleNumberHeight= scaleService.GetOriginValueOfScale(selScale, selLeaderModel.bmCircle.textHeight);

            double scaleTextHeight = scaleService.GetOriginValueOfScale(selScale, selLeaderModel.textHeight);
            double scaleTextSideGap = scaleService.GetOriginValueOfScale(selScale, selLeaderModel.textSideGap);
            double scaleTextGap = scaleService.GetOriginValueOfScale(selScale, selLeaderModel.textGap);

            double scaleLeaderLength= scaleService.GetOriginValueOfScale(selScale, selLeaderModel.leaderLength);

            string upperText = selLeaderModel.upperText;
            string lowerText = selLeaderModel.lowerText;
            string bmNumberText = selLeaderModel.bmNumber;

            double leaderLengthMax = scaleLeaderLength;

            // Text Regen에 따른 Box Size 정확하지 않음
            scaleTextSideGap = scaleTextSideGap * 2;

            Triangle tri = null;
            Circle cir = null;
            Text cirText = null;
            Line arrowLine = null;
            Text dimUpperText = null;
            Text dimLowerText = null;

            switch (selLeaderModel.position)
            {
                case POSITION_TYPE.LEFT:
                case POSITION_TYPE.BOTTOM:


                    // Text
                    if (upperText != "")
                    {
                        dimUpperText = new Text(GetSumPoint(selPoint, 0, scaleTextGap), upperText, scaleTextHeight) { Alignment = Text.alignmentType.BaselineCenter };
                        dimUpperText.Regen(new RegenParams(0, refSingleModel));
                        double tempTextWidth = scaleTextSideGap + dimUpperText.BoxSize.X + scaleTextSideGap;
                        if (leaderLengthMax < tempTextWidth)
                            leaderLengthMax = tempTextWidth;
                    }
                    if (lowerText != "")
                    {
                        dimLowerText = new Text(GetSumPoint(selPoint, 0, -scaleTextGap), lowerText, scaleTextHeight) { Alignment = Text.alignmentType.TopCenter };
                        dimLowerText.Regen(new RegenParams(0, refSingleModel));
                        double tempTextWidth = scaleTextSideGap + dimLowerText.BoxSize.X + scaleTextSideGap;
                        if (leaderLengthMax < tempTextWidth)
                            leaderLengthMax = tempTextWidth;
                    }

                    // Text Rotate
                    if (dimUpperText != null)
                        dimUpperText.Translate(-scaleArrowWidth - leaderLengthMax / 2, 0);
                    if (dimLowerText != null)
                        dimLowerText.Translate(-scaleArrowWidth - leaderLengthMax / 2, 0);

                    leaderLengthMax += scaleArrowWidth;

                    // Line
                    arrowLine = new Line(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, -leaderLengthMax, 0));

                    // Arrow
                    tri = new Triangle(GetSumPoint(selPoint,0,0), GetSumPoint(selPoint,-scaleArrowWidth, scaleArrowHeight / 2), GetSumPoint(selPoint, -scaleArrowWidth, -scaleArrowHeight / 2));

                    // circle
                    if(bmNumberText != "")
                    {
                        Point3D cirCenterPoint = GetSumPoint(arrowLine.EndPoint, -scaleCircleRadius,0);
                        cir = new Circle(GetSumPoint(cirCenterPoint, 0, 0), scaleCircleRadius);
                        cirText = new Text(GetSumPoint(cirCenterPoint, 0, 0), bmNumberText, scaleCircleNumberHeight) {Alignment=Text.alignmentType.MiddleCenter };
                    }
                    break;
                case POSITION_TYPE.RIGHT:
                case POSITION_TYPE.TOP:
                    // Text
                    if (upperText != "")
                    {
                        dimUpperText = new Text(GetSumPoint(selPoint, 0, scaleTextGap), upperText, scaleTextHeight) { Alignment = Text.alignmentType.BaselineCenter };
                        dimUpperText.Regen(new RegenParams(0, refSingleModel));
                        double tempTextWidth = scaleTextSideGap + dimUpperText.BoxSize.X + scaleTextSideGap;
                        if (leaderLengthMax < tempTextWidth)
                            leaderLengthMax = tempTextWidth;
                    }
                    if (lowerText != "")
                    {
                        dimLowerText = new Text(GetSumPoint(selPoint, 0, -scaleTextGap), lowerText, scaleTextHeight) { Alignment = Text.alignmentType.TopCenter };
                        dimLowerText.Regen(new RegenParams(0, refSingleModel));
                        double tempTextWidth = scaleTextSideGap + dimLowerText.BoxSize.X + scaleTextSideGap;
                        if (leaderLengthMax < tempTextWidth)
                            leaderLengthMax = tempTextWidth;
                    }

                    // Text Rotate
                    if (dimUpperText != null)
                        dimUpperText.Translate(scaleArrowWidth + leaderLengthMax / 2, 0);
                    if (dimLowerText != null)
                        dimLowerText.Translate(scaleArrowWidth + leaderLengthMax / 2, 0);

                    // Text Align
                    if (selLeaderModel.position == POSITION_TYPE.RIGHT)
                        SetTextAlign(selLeaderModel.textAlign, dimUpperText, dimLowerText);

                    leaderLengthMax += scaleArrowWidth;

                    // Line
                    arrowLine = new Line(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, leaderLengthMax, 0));

                    // Arrow
                    tri = new Triangle(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, scaleArrowWidth, scaleArrowHeight / 2), GetSumPoint(selPoint, scaleArrowWidth, -scaleArrowHeight / 2));

                    // circle
                    if (bmNumberText != "")
                    {
                        Point3D cirCenterPoint = GetSumPoint(arrowLine.EndPoint, scaleCircleRadius, 0);
                        cir = new Circle(GetSumPoint(cirCenterPoint, 0, 0), scaleCircleRadius);
                        cirText = new Text(GetSumPoint(cirCenterPoint, 0, 0), bmNumberText, scaleCircleNumberHeight) { Alignment = Text.alignmentType.MiddleCenter };
                    }
                    break;

            }

            double rotateRadian = 0;
            switch (selLeaderModel.position)
            {
                case POSITION_TYPE.TOP:
                case POSITION_TYPE.BOTTOM:
                    rotateRadian = Utility.DegToRad(90);
                    arrowLine.Rotate(rotateRadian, Vector3D.AxisZ, selPoint);
                    tri.Rotate(rotateRadian, Vector3D.AxisZ, selPoint);
                    if (cir != null) cir.Rotate(rotateRadian, Vector3D.AxisZ, selPoint);
                    if (cirText != null) cirText.InsertionPoint = GetSumPoint(cir.Center, 0, 0);
                    if (dimUpperText != null) dimUpperText.Rotate(rotateRadian, Vector3D.AxisZ, selPoint);
                    if (dimLowerText != null) dimLowerText.Rotate(rotateRadian, Vector3D.AxisZ, selPoint);

                    break;

            }


            if (arrowLine != null)
                leaderArrow.Add(arrowLine);
            if (tri != null)
                leaderArrow.Add(tri);

            if (upperText != "")
                if (dimUpperText != null)
                    leaderText.Add(dimUpperText);

            if (lowerText != "")
                if (dimLowerText != null)
                    leaderText.Add(dimLowerText);

            if (bmNumberText != "")
            {
                if (cir != null)
                    leaderLine.Add(cir);
                if (cirText != null)
                    leaderText.Add(cirText);
            }


            styleSerivce.SetLayerListEntity(ref leaderLine,layerService.LayerDimension);
            styleSerivce.SetLayerListEntity(ref leaderArrow, layerService.LayerDimension);
            styleSerivce.SetLayerListTextEntity(ref leaderText, layerService.LayerDimension);

            

            DrawEntityModel customEntityList = new DrawEntityModel();

            customEntityList.leaderlineList.AddRange(leaderLine);
            customEntityList.leaderTextList.AddRange(leaderText);
            customEntityList.leaderArrowList.AddRange(leaderArrow);

            return customEntityList;
        }

        public void SetTextAlign(POSITION_TYPE selPosition,Text t1, Text t2)
        {
            double t1X = 0;
            double t2X = 0;
            switch (selPosition)
            {
                case POSITION_TYPE.LEFT:
                    t1X = t1.InsertionPoint.X - t1.BoxSize.X / 2;
                    t2X = t2.InsertionPoint.X - t2.BoxSize.X / 2;
                    if (t1X > t2X)
                        t1.InsertionPoint.X -= t1X- t2X;
                    else
                        t2.InsertionPoint.X -= t2X - t1X;
                    break;
            }
           
        }

        public DrawEntityModel Draw_SlopeLeader(ref Model refSingleModel, Point3D selPoint, DrawSlopeLeaderModel selLeaderModel, double selScale)
        {
            List<Entity> leaderLine = new List<Entity>();
            List<Entity> leaderArrow = new List<Entity>();
            List<Entity> leaderText = new List<Entity>();

            //double scaleHeightOneSize = scaleService.GetOriginValueOfScale(selScale, selLeaderModel.heightOneSize);
            //double scaleTextGap = scaleService.GetOriginValueOfScale(selScale, selLeaderModel);
            //double scaleCircleRadius = scaleService.GetOriginValueOfScale(selScale, selLeaderModel.bmCircle.circleRadius);
            //double scaleCircleNumberHeight = scaleService.GetOriginValueOfScale(selScale, selLeaderModel.bmCircle.textHeight);

            //Dictionary<string, List<Entity>> customEntityList = new Dictionary<string, List<Entity>>();

            //styleSerivce.SetLayerListEntity(ref leaderLine, layerService.LayerDimension);
            //styleSerivce.SetLayerListEntity(ref leaderArrow, layerService.LayerDimension);
            //styleSerivce.SetLayerListTextEntity(ref leaderText, layerService.LayerDimension);

            //customEntityList.Add(CommonGlobal.LeaderLine, leaderLine);
            //customEntityList.Add(CommonGlobal.LeaderText, leaderText);
            //customEntityList.Add(CommonGlobal.LeaderArrow, leaderArrow);


            DrawEntityModel customEntityList = new DrawEntityModel();

            customEntityList.leaderlineList.AddRange(leaderLine);
            customEntityList.leaderTextList.AddRange(leaderText);
            customEntityList.leaderArrowList.AddRange(leaderArrow);
            return customEntityList;
        }


        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }

    }
}

