using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawWork.Commons;
using DrawWork.DrawModels;
using DrawWork.DrawSacleServices;
using DrawWork.DrawServices;
using DrawWork.DrawStyleServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawShapes
{
    public class DrawWeldSymbols
    {
        private StyleFunctionService styleService;
        private LayerStyleService layerService;

        private DrawEditingService editingService;
        private DrawScaleService scaleService;

        public DrawWeldSymbols()
        {
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();

            editingService = new DrawEditingService();
            scaleService = new DrawScaleService();
        }

        public List<Entity> GetWeldSymbolMirror(Point3D selPoint, Model singleModel,double selScale,  DrawWeldSymbolModel selModel)
        {
            List<Entity> newList = new List<Entity>();

            double scaleArrowHeight = scaleService.GetOriginValueOfScale(selScale, selModel.arrowHeadHeight);
            double scaleArrowWidth = scaleService.GetOriginValueOfScale(selScale, selModel.arrowHeadWidth);

            double scaleLeaderLineLength = scaleService.GetOriginValueOfScale(selScale, selModel.leaderLineLength);
            double scaleAllAroundWeldRadius= scaleService.GetOriginValueOfScale(selScale, selModel.allRoundWeldRadius);
            double scaleReferenceLineLength= scaleService.GetOriginValueOfScale(selScale, selModel.leaderLineLength);
            double scaleTailLineLength= scaleService.GetOriginValueOfScale(selScale, 3);
            double scaleFieldLength= scaleService.GetOriginValueOfScale(selScale, selModel.fieldLength);
            double scaleFieldFlagHeight = scaleService.GetOriginValueOfScale(selScale, selModel.fieldFlagHeight);
            double scaleFieldFlagWidth = scaleService.GetOriginValueOfScale(selScale, selModel.fieldFlagWidth);
            double scaleArrowHeadWidth = scaleService.GetOriginValueOfScale(selScale, selModel.arrowHeadWidth);
            double scaleArrowHeadHeight = scaleService.GetOriginValueOfScale(selScale, selModel.arrowHeadHeight);

            double scaleTextHeight= scaleService.GetOriginValueOfScale(selScale, 2.5);

            // Position : Leader Line
            POSITION_TYPE modelPosition = POSITION_TYPE.NotSet;
            double positionRotateValue = 0;

            switch (selModel.position)
            {
                case ORIENTATION_TYPE.TOPLEFT:
                    positionRotateValue = Utility.DegToRad(120);
                    modelPosition = POSITION_TYPE.LEFT;
                    break;
                case ORIENTATION_TYPE.BOTTOMLEFT:
                    positionRotateValue = Utility.DegToRad(-120);
                    modelPosition = POSITION_TYPE.LEFT;
                    break;
                case ORIENTATION_TYPE.TOPRIGHT:
                    positionRotateValue = Utility.DegToRad(60);
                    modelPosition = POSITION_TYPE.RIGHT;
                    break;
                case ORIENTATION_TYPE.BOTTOMRIGHT:
                    positionRotateValue = Utility.DegToRad(-60);
                    modelPosition = POSITION_TYPE.RIGHT;
                    break;
            }

            // Arrow : Right
            Line arrowLine01 = new Line(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, scaleArrowHeadWidth, -scaleArrowHeadHeight / 2));
            arrowLine01.Rotate(positionRotateValue, Vector3D.AxisZ, GetSumPoint(selPoint, 0, 0));
            Line arrowLine02 = new Line(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, scaleArrowHeadWidth, +scaleArrowHeadHeight / 2));
            arrowLine02.Rotate(positionRotateValue, Vector3D.AxisZ, GetSumPoint(selPoint, 0, 0));
            Line arrowLine03 = new Line(GetSumPoint(selPoint, scaleArrowHeadWidth, +scaleArrowHeadHeight / 2), GetSumPoint(selPoint, scaleArrowHeadWidth, -scaleArrowHeadHeight / 2));
            arrowLine03.Rotate(positionRotateValue, Vector3D.AxisZ, GetSumPoint(selPoint, 0, 0));



            // Leader Line : Rotate
            Line leaderLine = new Line(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, scaleLeaderLineLength, 0));
            leaderLine.Rotate(positionRotateValue, Vector3D.AxisZ,GetSumPoint(selPoint,0,0));

            // All Around Point
            Point3D allAroundPoint = GetSumPoint(leaderLine.EndPoint, 0, 0);
            if (!selModel.leaderLineVisible)
                allAroundPoint = GetSumPoint(selPoint, 0, 0); // Point Adj
            Circle allAroundCir = new Circle(GetSumPoint(allAroundPoint, 0, 0), scaleAllAroundWeldRadius);




            double scaleS= scaleService.GetOriginValueOfScale(selScale, 6);
            double scaleOne = scaleService.GetOriginValueOfScale(selScale, 1);
            double scaleOnePointFive = scaleService.GetOriginValueOfScale(selScale, 1.5);
            double scaleTwo = scaleService.GetOriginValueOfScale(selScale, 2);
            double scaleTwoPointFive = scaleService.GetOriginValueOfScale(selScale, 2.5);
            double scaleTwoPointTwoFive = scaleService.GetOriginValueOfScale(selScale, 2.25);
            double scaleThree = scaleService.GetOriginValueOfScale(selScale, 3);
            double scaleFourPointTwo = scaleService.GetOriginValueOfScale(selScale, 4.2);
            double scaleFivePointFive = scaleService.GetOriginValueOfScale(selScale, 5.5);
            double scaleSix = scaleService.GetOriginValueOfScale(selScale, 6);
            double scaleSevenPointFive = scaleService.GetOriginValueOfScale(selScale, 7.5);
            double scaleNine = scaleService.GetOriginValueOfScale(selScale, 9);
            double scaleTen = scaleService.GetOriginValueOfScale(selScale, 10);

            double rotate45 = Utility.DegToRad(45);
            double rotate60 = Utility.DegToRad(60);





            double currentReferenceLength = 0;
            List<double> textLength = new List<double>();

            List<Entity> weldList = new List<Entity>();
            List<Entity> weldTextList = new List<Entity>();

            // Both
            if (selModel.weldDetailType != WeldSymbolDetail_Type.BothSide)
            {
                selModel.weldSize2 = selModel.weldSize1;

                selModel.weldLength2 = selModel.weldLength1;
                selModel.weldQuantity2 = selModel.weldQuantity1;
                selModel.weldPitch2 = selModel.weldPitch1;

                selModel.weldAngle2 = selModel.weldAngle1;

                selModel.weldRoot2 = selModel.weldRoot1;
            }

            // Left Right
            double sizeLength =0;
            double lnpLength = 0;

            Text.alignmentType lnpUpAlign = Text.alignmentType.BaselineLeft;
            Text.alignmentType lnpDownAlign = Text.alignmentType.TopLeft;
            switch (modelPosition)
            {
                case POSITION_TYPE.LEFT:
                    sizeLength = -scaleThree;
                    lnpLength = -scaleNine - scaleOne;
                    lnpUpAlign = Text.alignmentType.BaselineRight;
                    lnpDownAlign = Text.alignmentType.TopRight;
                    break;
                case POSITION_TYPE.RIGHT:
                    sizeLength = scaleThree;
                    lnpLength = scaleNine + scaleOne;
                    lnpUpAlign = Text.alignmentType.BaselineLeft;
                    lnpDownAlign = Text.alignmentType.TopLeft;
                    break;
            }

            Point3D tempPoint = GetSumPoint(selPoint, 0, 0);
            
            // OhterSide : Up
            if (selModel.weldDetailType == WeldSymbolDetail_Type.OtherSide || selModel.weldDetailType == WeldSymbolDetail_Type.BothSide)
            {
                // S
                string sizeStr = selModel.weldSize1;
                if(sizeStr!="")
                    weldTextList.Add(new Text(GetSumPoint(allAroundPoint, sizeLength, scaleTwoPointTwoFive), sizeStr, scaleTextHeight) {Alignment=Text.alignmentType.MiddleCenter });

                // L(n)P
                string lnpStr = "";
                if (selModel.weldLength1 != "")
                    lnpStr = selModel.weldLength1;
                if (selModel.weldQuantity1 != "")
                    lnpStr += "(" + selModel.weldQuantity1 + ")";
                if(selModel.weldPitch1 !="")
                    lnpStr+= "-" + selModel.weldPitch1;
                if (lnpStr != "")
                {
                    Text lnpText = new Text(GetSumPoint(allAroundPoint, lnpLength, scaleOne), lnpStr, scaleTextHeight) { Alignment = lnpUpAlign };
                    lnpText.Regen(new RegenParams(0, singleModel));
                    textLength.Add(lnpText.BoxSize.X);
                    weldTextList.Add(lnpText);
                }


                // Weld Type : 계속 추가 해야 함
                double faceWidth = 0;
                Point3D currentPoint = GetSumPoint(allAroundPoint, 0, 0);
                switch (selModel.weldTypeUp)
                {
                    case WeldSymbol_Type.Fillet:
                        faceWidth = scaleFourPointTwo;
                        currentPoint = GetSumPoint(currentPoint, scaleSix, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, scaleThree)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, scaleThree), GetSumPoint(currentPoint, scaleThree, 0)));
                        currentPoint = GetSumPoint(currentPoint, 0, scaleThree);
                        break;

                    case WeldSymbol_Type.V:
                        double vLength = scaleService.GetOriginValueOfScale(selScale, 3 / Math.Sqrt(3));
                        faceWidth = vLength * 2;
                        currentPoint = GetSumPoint(currentPoint, scaleSevenPointFive, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, vLength, scaleThree)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, -vLength, scaleThree)));
                        currentPoint = GetSumPoint(currentPoint, 0, scaleThree);
                        break;
                    case WeldSymbol_Type.Bevel:
                        faceWidth = scaleThree;
                        currentPoint = GetSumPoint(currentPoint, scaleSix, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, scaleThree)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, scaleThree, scaleThree)));
                        currentPoint = GetSumPoint(currentPoint, scaleOnePointFive, scaleThree);
                        break;
                    case WeldSymbol_Type.Square:
                        faceWidth = scaleTwoPointFive;
                        currentPoint = GetSumPoint(currentPoint, scaleSix, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, scaleThree)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, scaleTwoPointFive, 0), GetSumPoint(currentPoint, scaleTwoPointFive, scaleThree)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwoPointFive/2, scaleThree);
                        break;
                    case WeldSymbol_Type.Plug:
                        faceWidth = scaleThree;
                        currentPoint = GetSumPoint(currentPoint, scaleSix, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, scaleTwo)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, scaleThree, 0), GetSumPoint(currentPoint, scaleThree, scaleTwo)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, scaleTwo), GetSumPoint(currentPoint, scaleThree, scaleTwo)));
                        currentPoint = GetSumPoint(currentPoint, scaleThree / 2, scaleTwo);
                        break;
                    case WeldSymbol_Type.FlareV:
                        faceWidth = scaleTwo + scaleThree *2;
                        currentPoint = GetSumPoint(currentPoint, scaleSix, 0);
                        weldList.Add(new Arc(GetSumPoint(currentPoint, -scaleThree, 0), GetSumPoint(currentPoint, -scaleThree, scaleThree), GetSumPoint(currentPoint, 0, 0)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, scaleTwo + scaleThree, 0), GetSumPoint(currentPoint, scaleTwo, 0), GetSumPoint(currentPoint, scaleTwo + scaleThree, scaleThree)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwo / 2, scaleThree);
                        break;
                    case WeldSymbol_Type.FlareBevel:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, scaleSix, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint,0,0),GetSumPoint(currentPoint,0,scaleThree)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, scaleTwo + scaleThree, 0), GetSumPoint(currentPoint, scaleTwo, 0), GetSumPoint(currentPoint, scaleTwo + scaleThree, scaleThree)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwo / 2, scaleThree);
                        break;
                    case WeldSymbol_Type.FlangeV:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, scaleSix, 0);
                        weldList.Add(new Arc(GetSumPoint(currentPoint, -scaleThree, scaleThree), GetSumPoint(currentPoint, 0, scaleThree), GetSumPoint(currentPoint, -scaleThree, 0)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, scaleTwo + scaleThree, scaleThree), GetSumPoint(currentPoint, scaleTwo, scaleThree), GetSumPoint(currentPoint, scaleTwo + scaleThree, 0)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwo / 2, scaleThree);
                        break;
                    case WeldSymbol_Type.FlangeBevel:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, scaleSix, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, scaleThree)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, scaleTwo + scaleThree, scaleThree), GetSumPoint(currentPoint, scaleTwo, scaleThree), GetSumPoint(currentPoint, scaleTwo + scaleThree, 0)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwo / 2, scaleThree);
                        break;
                    case WeldSymbol_Type.J:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, scaleSix, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, scaleFivePointFive)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, 0, scaleFivePointFive), GetSumPoint(currentPoint, scaleThree, scaleFivePointFive), GetSumPoint(currentPoint,0, scaleTwoPointFive)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwo / 2, scaleFivePointFive);
                        break;
                    case WeldSymbol_Type.U:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, scaleSevenPointFive, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, scaleTwoPointFive)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, -scaleThree, scaleFivePointFive), GetSumPoint(currentPoint, 0, scaleTwoPointFive), GetSumPoint(currentPoint, scaleThree, scaleFivePointFive),false));
                        currentPoint = GetSumPoint(currentPoint, 0, scaleFivePointFive);
                        break;

                }


                // Root
                if(selModel.weldRoot1 != "")
                {
                    currentPoint = GetSumPoint(currentPoint, 0, scaleOnePointFive);
                    tempPoint = (Point3D)currentPoint.Clone();
                    if (modelPosition == POSITION_TYPE.LEFT)
                        editingService.SetMirrorPoint(Plane.YZ, ref tempPoint, GetSumPoint(allAroundPoint, 0, 0));

                    weldTextList.Add(new Text(GetSumPoint(tempPoint, 0,0),selModel.weldRoot1,scaleOnePointFive) { Alignment = Text.alignmentType.MiddleCenter });
                    // Angle
                    if (selModel.weldAngle1 != "")
                    {
                        currentPoint = GetSumPoint(currentPoint, 0, scaleTwoPointFive);
                        tempPoint = (Point3D)currentPoint.Clone();
                        if (modelPosition == POSITION_TYPE.LEFT)
                            editingService.SetMirrorPoint(Plane.YZ, ref tempPoint, GetSumPoint(allAroundPoint, 0, 0));
                        weldTextList.Add(new Text(GetSumPoint(tempPoint, 0, 0), selModel.weldAngle1, scaleTwoPointFive) { Alignment = Text.alignmentType.MiddleCenter });
                        currentPoint = GetSumPoint(currentPoint, 0, scaleOne);
                    }
                }
                else
                {
                    // Angle
                    if (selModel.weldAngle1 != "")
                    {
                        currentPoint = GetSumPoint(currentPoint, 0, scaleOnePointFive);
                        tempPoint = (Point3D)currentPoint.Clone();
                        if (modelPosition == POSITION_TYPE.LEFT)
                            editingService.SetMirrorPoint(Plane.YZ, ref tempPoint, GetSumPoint(allAroundPoint, 0, 0));
                        weldTextList.Add(new Text(GetSumPoint(tempPoint, 0, 0), selModel.weldAngle1, scaleTwoPointFive) { Alignment = Text.alignmentType.MiddleCenter });
                        currentPoint = GetSumPoint(currentPoint, 0, scaleOne);
                    }
                }

                // Face
                Point3D facePoint = GetSumPoint(currentPoint,0,0);
                double faceRotate = 0;

                switch (selModel.weldTypeDown)
                {
                    case WeldSymbol_Type.Fillet:
                        if (weldList[weldList.Count-1] is Line)
                            facePoint = GetSumPoint(((Line)weldList[weldList.Count - 1]).MidPoint, scaleOne/Math.Sqrt(2) , scaleOne/Math.Sqrt(2));
                        faceRotate = -rotate45;
                        break;
                    default:
                        facePoint = GetSumPoint(facePoint, 0, scaleOne);
                        break;
                }
                 

                switch (selModel.weldFaceUp)
                {
                    case WeldFace_Type.Convex:
                        Arc convexArc = new Arc(GetSumPoint(facePoint, -faceWidth / 2, 0), GetSumPoint(facePoint, 0, scaleOne), GetSumPoint(facePoint,faceWidth/2, 0),false);
                        convexArc.Rotate(faceRotate, Vector3D.AxisZ, GetSumPoint(facePoint, 0, 0));
                        weldList.Add(convexArc);
                        break;
                    case WeldFace_Type.Concave:
                        Arc concaveArc = new Arc(GetSumPoint(facePoint, -faceWidth / 2, scaleOne), GetSumPoint(facePoint, 0, 0), GetSumPoint(facePoint, faceWidth / 2, scaleOne),false);
                        concaveArc.Rotate(faceRotate, Vector3D.AxisZ, GetSumPoint(facePoint, 0, 0));
                        weldList.Add(concaveArc);
                        break;
                    case WeldFace_Type.Flat:
                        Line flatLine = new Line(GetSumPoint(facePoint, -faceWidth / 2, 0), GetSumPoint(facePoint, faceWidth / 2, 0));
                        flatLine.Rotate(faceRotate, Vector3D.AxisZ, GetSumPoint(facePoint, 0, 0));
                        weldList.Add(flatLine);

                        // Only Flat Matching
                        if (selModel.machiningVisible)
                        {
                            Line flatOffsetLine = (Line)flatLine.Offset(-scaleTwo, Vector3D.AxisZ);
                            if (modelPosition == POSITION_TYPE.LEFT)
                                editingService.SetMirrorLine(Plane.YZ, ref flatOffsetLine, GetSumPoint(allAroundPoint, 0, 0));
                            Text matchingText = new Text(GetSumPoint(flatOffsetLine.MidPoint, 0, 0), selModel.machiningStr, scaleTextHeight) { Alignment = Text.alignmentType.MiddleCenter };
                            weldTextList.Add(matchingText);
                        }

                        break;
                }

            }

            // ArrowSide : Down
            if(selModel.weldDetailType==WeldSymbolDetail_Type.ArrowSide || selModel.weldDetailType == WeldSymbolDetail_Type.BothSide)
            {
                // S
                string sizeStr = selModel.weldSize2;
                if (sizeStr != "")
                    weldTextList.Add(new Text(GetSumPoint(allAroundPoint, sizeLength, -scaleTwoPointTwoFive), sizeStr, scaleTextHeight) { Alignment = Text.alignmentType.MiddleCenter });

                // L(n)P
                string lnpStr = "";
                if (selModel.weldLength2 != "")
                    lnpStr = selModel.weldLength2;
                if (selModel.weldQuantity2 != "")
                    lnpStr += "(" + selModel.weldQuantity2 + ")";
                if (selModel.weldPitch2 != "")
                    lnpStr += "-" + selModel.weldPitch2;
                if (lnpStr != "")
                {
                    Text lnpText = new Text(GetSumPoint(allAroundPoint, lnpLength, -scaleOne), lnpStr, scaleTextHeight) { Alignment = lnpDownAlign };
                    lnpText.Regen(new RegenParams(0, singleModel));
                    textLength.Add(lnpText.BoxSize.X);
                    weldTextList.Add(lnpText);
                }


                // Weld Type : 계속 추가 해야 함
                double faceWidth = 0;
                Point3D currentPoint = GetSumPoint(allAroundPoint, 0, 0);
                switch (selModel.weldTypeDown)
                {
                    case WeldSymbol_Type.Fillet:
                        faceWidth = scaleFourPointTwo;
                        currentPoint = GetSumPoint(currentPoint, scaleSix, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, -scaleThree)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, -scaleThree), GetSumPoint(currentPoint, scaleThree, 0)));
                        currentPoint = GetSumPoint(currentPoint, 0, -scaleThree);
                        break;
                    case WeldSymbol_Type.V:
                        double vLength = scaleService.GetOriginValueOfScale(selScale, 3 / Math.Sqrt(3));
                        faceWidth = vLength * 2;
                        currentPoint = GetSumPoint(currentPoint, scaleSevenPointFive, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, vLength, -scaleThree)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, -vLength, -scaleThree)));
                        currentPoint = GetSumPoint(currentPoint, 0, -scaleThree);
                        break;
                    case WeldSymbol_Type.Bevel:
                        faceWidth = scaleThree;
                        currentPoint = GetSumPoint(currentPoint, scaleSix, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, -scaleThree)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, scaleThree, -scaleThree)));
                        currentPoint = GetSumPoint(currentPoint, scaleOnePointFive, -scaleThree);
                        break;
                    case WeldSymbol_Type.Square:
                        faceWidth = scaleTwoPointFive;
                        currentPoint = GetSumPoint(currentPoint, scaleSix, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, -scaleThree)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, scaleTwoPointFive, 0), GetSumPoint(currentPoint, scaleTwoPointFive, -scaleThree)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwoPointFive / 2, -scaleThree);
                        break;
                    case WeldSymbol_Type.Plug:
                        faceWidth = scaleThree;
                        currentPoint = GetSumPoint(currentPoint, scaleSix, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, -scaleTwo)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, scaleThree, 0), GetSumPoint(currentPoint, scaleThree, -scaleTwo)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, -scaleTwo), GetSumPoint(currentPoint, scaleThree, -scaleTwo)));
                        currentPoint = GetSumPoint(currentPoint, scaleThree / 2, -scaleTwo);
                        break;
                    case WeldSymbol_Type.FlareV:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, scaleSix, 0);
                        weldList.Add(new Arc(GetSumPoint(currentPoint, -scaleThree, 0), GetSumPoint(currentPoint, -scaleThree, -scaleThree), GetSumPoint(currentPoint, 0, 0)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, scaleTwo + scaleThree, 0), GetSumPoint(currentPoint, scaleTwo, 0), GetSumPoint(currentPoint, scaleTwo + scaleThree, -scaleThree)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwo / 2, -scaleThree);
                        break;
                    case WeldSymbol_Type.FlareBevel:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, scaleSix, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, -scaleThree)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, scaleTwo + scaleThree, 0), GetSumPoint(currentPoint, scaleTwo, 0), GetSumPoint(currentPoint, scaleTwo + scaleThree, -scaleThree)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwo / 2, -scaleThree);
                        break;
                    case WeldSymbol_Type.FlangeV:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, scaleSix, 0);
                        weldList.Add(new Arc(GetSumPoint(currentPoint, -scaleThree, -scaleThree), GetSumPoint(currentPoint, 0, -scaleThree), GetSumPoint(currentPoint, -scaleThree, 0)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, scaleTwo + scaleThree, -scaleThree), GetSumPoint(currentPoint, scaleTwo, -scaleThree), GetSumPoint(currentPoint, scaleTwo + scaleThree, 0)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwo / 2, -scaleThree);
                        break;
                    case WeldSymbol_Type.FlangeBevel:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, scaleSix, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, -scaleThree)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, scaleTwo + scaleThree, -scaleThree), GetSumPoint(currentPoint, scaleTwo, -scaleThree), GetSumPoint(currentPoint, scaleTwo + scaleThree, 0)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwo / 2, -scaleThree);
                        break;
                    case WeldSymbol_Type.J:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, scaleSix, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, -scaleFivePointFive)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, 0, -scaleFivePointFive), GetSumPoint(currentPoint, scaleThree, -scaleFivePointFive), GetSumPoint(currentPoint, 0, -scaleTwoPointFive)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwo / 2, -scaleFivePointFive);
                        break;
                    case WeldSymbol_Type.U:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, scaleSevenPointFive, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, -scaleTwoPointFive)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, -scaleThree, -scaleFivePointFive), GetSumPoint(currentPoint, 0, -scaleTwoPointFive), GetSumPoint(currentPoint, scaleThree, -scaleFivePointFive), false));
                        currentPoint = GetSumPoint(currentPoint, 0, -scaleFivePointFive);
                        break;

                }


                // Root
                if (selModel.weldRoot2 != "")
                {
                    currentPoint = GetSumPoint(currentPoint, 0, -scaleOnePointFive);
                    tempPoint = (Point3D)currentPoint.Clone();
                    if (modelPosition == POSITION_TYPE.LEFT)
                        editingService.SetMirrorPoint(Plane.YZ, ref tempPoint, GetSumPoint(allAroundPoint, 0, 0));
                    weldTextList.Add(new Text(GetSumPoint(tempPoint, 0, 0), selModel.weldRoot2, scaleOnePointFive) { Alignment = Text.alignmentType.MiddleCenter });
                    // Angle
                    if (selModel.weldAngle2 != "")
                    {
                        currentPoint = GetSumPoint(currentPoint, 0, -scaleTwoPointFive);
                        tempPoint = (Point3D)currentPoint.Clone();
                        if (modelPosition == POSITION_TYPE.LEFT)
                            editingService.SetMirrorPoint(Plane.YZ, ref tempPoint, GetSumPoint(allAroundPoint, 0, 0));
                        weldTextList.Add(new Text(GetSumPoint(tempPoint, 0, 0), selModel.weldAngle2, scaleTwoPointFive) { Alignment = Text.alignmentType.MiddleCenter });
                        currentPoint = GetSumPoint(currentPoint, 0, -scaleOne);
                    }
                }
                else
                {
                    // Angle
                    if (selModel.weldAngle2 != "")
                    {
                        currentPoint = GetSumPoint(currentPoint, 0, -scaleOnePointFive);
                        tempPoint = (Point3D)currentPoint.Clone();
                        if (modelPosition == POSITION_TYPE.LEFT)
                            editingService.SetMirrorPoint(Plane.YZ, ref tempPoint, GetSumPoint(allAroundPoint, 0, 0));
                        weldTextList.Add(new Text(GetSumPoint(tempPoint, 0, 0), selModel.weldAngle2, scaleTwoPointFive) { Alignment = Text.alignmentType.MiddleCenter });
                        currentPoint = GetSumPoint(currentPoint, 0, -scaleOne);
                    }
                }

                // Face
                Point3D facePoint = GetSumPoint(currentPoint, 0, 0);
                double faceRotate = 0;

                switch (selModel.weldTypeDown)
                {
                    case WeldSymbol_Type.Fillet:
                        if (weldList[weldList.Count - 1] is Line)
                            facePoint = GetSumPoint(((Line)weldList[weldList.Count - 1]).MidPoint,scaleOne / Math.Sqrt(2),- scaleOne / Math.Sqrt(2));
                        faceRotate = rotate45;
                        break;
                    default:
                        facePoint = GetSumPoint(facePoint, 0, -scaleOne);
                        break;
                }


                switch (selModel.weldFaceDown)
                {
                    case WeldFace_Type.Convex:
                        Arc convexArc = new Arc(GetSumPoint(facePoint, -faceWidth / 2, 0), GetSumPoint(facePoint, 0, -scaleOne), GetSumPoint(facePoint, faceWidth / 2, 0), false);
                        convexArc.Rotate(faceRotate, Vector3D.AxisZ, GetSumPoint(facePoint, 0, 0));
                        weldList.Add(convexArc);
                        break;
                    case WeldFace_Type.Concave:
                        Arc concaveArc = new Arc(GetSumPoint(facePoint, -faceWidth / 2, -scaleOne), GetSumPoint(facePoint, 0, 0), GetSumPoint(facePoint, faceWidth / 2, -scaleOne), false);
                        concaveArc.Rotate(faceRotate, Vector3D.AxisZ, GetSumPoint(facePoint, 0, 0));
                        weldList.Add(concaveArc);
                        break;
                    case WeldFace_Type.Flat:
                        Line flatLine = new Line(GetSumPoint(facePoint, -faceWidth / 2, 0), GetSumPoint(facePoint, faceWidth / 2, 0));
                        flatLine.Rotate(faceRotate, Vector3D.AxisZ, GetSumPoint(facePoint, 0, 0));
                        weldList.Add(flatLine);

                        // Only Flat Matching
                        if (selModel.machiningVisible)
                        {
                            Line flatOffsetLine = (Line)flatLine.Offset(scaleTwo, Vector3D.AxisZ);
                            if(modelPosition==POSITION_TYPE.LEFT)
                                editingService.SetMirrorLine(Plane.YZ, ref flatOffsetLine, GetSumPoint(allAroundPoint, 0, 0));
                            Text matchingText = new Text(GetSumPoint(flatOffsetLine.MidPoint, 0, 0), selModel.machiningStr, scaleTextHeight) { Alignment = Text.alignmentType.MiddleCenter };
                            weldTextList.Add(matchingText);
                        }

                        break;
                }

            }

            // Reference Line : Length : adj
            double maxTextLength = 0;
            foreach (double eachLength in textLength)
                if (maxTextLength < eachLength)
                    maxTextLength = eachLength;

            currentReferenceLength = scaleNine + maxTextLength + scaleTwo;

            // Minmum Length
            if (currentReferenceLength < scaleLeaderLineLength)
                currentReferenceLength = scaleLeaderLineLength;


            // Reference, Tail, Field
            Text.alignmentType tailTextAlign = Text.alignmentType.BaselineLeft;
            Line referenceLine = null;
            Line tailUpLine = null;
            Line tailDownLine = null;
            Line fieldLine = null;
            Line fieldTriTop = null;
            Line fieldTriBottom = null;

            switch (modelPosition)
            {
                case POSITION_TYPE.LEFT:
                    tailTextAlign = Text.alignmentType.BaselineRight;
                    referenceLine =new Line(GetSumPoint(allAroundPoint, 0, 0), GetSumPoint(allAroundPoint, -currentReferenceLength, 0));
                    tailUpLine = new Line(GetSumPoint(referenceLine.EndPoint, 0, 0), GetSumPoint(referenceLine.EndPoint, -scaleTailLineLength, scaleTailLineLength));
                    tailDownLine = new Line(GetSumPoint(referenceLine.EndPoint, 0, 0), GetSumPoint(referenceLine.EndPoint, -scaleTailLineLength, -scaleTailLineLength));
                    fieldLine = new Line(GetSumPoint(allAroundPoint, 0, 0), GetSumPoint(allAroundPoint, 0, scaleFieldLength));
                    fieldTriTop = new Line(GetSumPoint(fieldLine.EndPoint, 0, 0), GetSumPoint(fieldLine.EndPoint, -scaleFieldFlagWidth, -scaleFieldFlagHeight / 2));
                    fieldTriBottom = new Line(GetSumPoint(fieldLine.EndPoint, 0, -scaleFieldFlagHeight), GetSumPoint(fieldLine.EndPoint, -scaleFieldFlagWidth, -scaleFieldFlagHeight / 2));
                    break;

                case POSITION_TYPE.RIGHT:
                    tailTextAlign = Text.alignmentType.BaselineLeft;
                    referenceLine = new Line(GetSumPoint(allAroundPoint, 0, 0), GetSumPoint(allAroundPoint, currentReferenceLength, 0));
                    tailUpLine = new Line(GetSumPoint(referenceLine.EndPoint, 0, 0), GetSumPoint(referenceLine.EndPoint, scaleTailLineLength, scaleTailLineLength));
                    tailDownLine = new Line(GetSumPoint(referenceLine.EndPoint, 0, 0), GetSumPoint(referenceLine.EndPoint, scaleTailLineLength, -scaleTailLineLength));
                    fieldLine = new Line(GetSumPoint(allAroundPoint, 0, 0), GetSumPoint(allAroundPoint, 0, scaleFieldLength));
                    fieldTriTop = new Line(GetSumPoint(fieldLine.EndPoint, 0, 0), GetSumPoint(fieldLine.EndPoint, scaleFieldFlagWidth, -scaleFieldFlagHeight / 2));
                    fieldTriBottom = new Line(GetSumPoint(fieldLine.EndPoint, 0, -scaleFieldFlagHeight), GetSumPoint(fieldLine.EndPoint, scaleFieldFlagWidth, -scaleFieldFlagHeight / 2));
                    break;
            }

            if (modelPosition == POSITION_TYPE.LEFT) 
            {
                editingService.GetMirrorEntity(Plane.YZ, weldList, allAroundPoint.X, allAroundPoint.Y);
            }
                

            // Tail Specification
            List<Text> tailSpecList = new List<Text>();
            if (selModel.specification != "")
            {
                if (selModel.specification.Contains(System.Environment.NewLine))
                {
                    string[] tempSpecification = selModel.specification.Split(new[] { System.Environment.NewLine }, StringSplitOptions.None);
                    if (tempSpecification.Length > 0)
                    {
                        int maxCount = 3;
                        int specCount = 0;
                        foreach (string eachSpec in tempSpecification)
                            if (eachSpec.Trim() != "")
                            {
                                specCount++;
                                tailSpecList.Add(new Text(GetSumPoint(allAroundPoint, 0, 0), eachSpec.Trim(), scaleTextHeight));
                                if (specCount == maxCount)
                                    break;
                            }
                    }
                }
                else
                {
                    tailSpecList.Add(new Text(GetSumPoint(allAroundPoint, 0, 0), selModel.specification, scaleTextHeight));
                }
            }
            if (tailSpecList.Count > 0)
            {
                foreach (Text eachText in tailSpecList)
                    eachText.Alignment = tailTextAlign;

                double scaleTailGap= scaleService.GetOriginValueOfScale(selScale, 1.75);
                double scaleTailTextGap = scaleService.GetOriginValueOfScale(selScale, 3.5);
                if (tailSpecList.Count == 1)
                {
                    tailSpecList[0].InsertionPoint = GetSumPoint(tailDownLine.EndPoint, 0, scaleTailGap);
                }
                else if (tailSpecList.Count == 2)
                {
                    tailSpecList[1].InsertionPoint = GetSumPoint(tailDownLine.EndPoint, 0, 0);
                    tailSpecList[0].InsertionPoint = GetSumPoint(tailSpecList[1].InsertionPoint, 0, scaleTailTextGap);
                }
                else if (tailSpecList.Count == 3)
                {
                    tailSpecList[2].InsertionPoint = GetSumPoint(tailDownLine.EndPoint, 0, -scaleTailGap);
                    tailSpecList[1].InsertionPoint = GetSumPoint(tailSpecList[2].InsertionPoint, 0, scaleTailTextGap);
                    tailSpecList[0].InsertionPoint = GetSumPoint(tailSpecList[1].InsertionPoint, 0, scaleTailTextGap);
                }
            }


            // Entity : Add
            if (selModel.arrowHeadVisible)
            {
                newList.Add(arrowLine01);
                newList.Add(arrowLine02);
                newList.Add(arrowLine03);
            }

            if (selModel.leaderLineVisible)
                newList.Add(leaderLine);
            if (selModel.allRoundWeldVisible)
                newList.Add(allAroundCir);

            if (selModel.tailVisible)
            {
                newList.Add(tailUpLine);
                newList.Add(tailDownLine);
            }
            if (selModel.fieldWeldVisible)
            {
                newList.Add(fieldLine);
                newList.Add(fieldTriTop);
                newList.Add(fieldTriBottom);
                newList.AddRange(tailSpecList);
            }
            if (weldList.Count > 0)
                newList.AddRange(weldList);
            if (weldTextList.Count > 0)
                newList.AddRange(weldTextList);

            newList.Add(referenceLine);

            return newList;
        }



        public List<Entity> GetWeldSymbol(Point3D selPoint, Model singleModel, double selScale, DrawWeldSymbolModel selModel)
        {
            List<Entity> newList = new List<Entity>();

            double scaleArrowHeight = scaleService.GetOriginValueOfScale(selScale, selModel.arrowHeadHeight);
            double scaleArrowWidth = scaleService.GetOriginValueOfScale(selScale, selModel.arrowHeadWidth);

            double scaleLeaderLineLength = scaleService.GetOriginValueOfScale(selScale, selModel.leaderLineLength);
            double scaleAllAroundWeldRadius = scaleService.GetOriginValueOfScale(selScale, selModel.allRoundWeldRadius);
            double scaleReferenceLineLength = scaleService.GetOriginValueOfScale(selScale, selModel.referenceLength);
            double scaleTailLineLength = scaleService.GetOriginValueOfScale(selScale, 3);
            double scaleFieldLength = scaleService.GetOriginValueOfScale(selScale, selModel.fieldLength);
            double scaleFieldFlagHeight = scaleService.GetOriginValueOfScale(selScale, selModel.fieldFlagHeight);
            double scaleFieldFlagWidth = scaleService.GetOriginValueOfScale(selScale, selModel.fieldFlagWidth);
            double scaleArrowHeadWidth = scaleService.GetOriginValueOfScale(selScale, selModel.arrowHeadWidth);
            double scaleArrowHeadHeight = scaleService.GetOriginValueOfScale(selScale, selModel.arrowHeadHeight);

            double scaleTextHeight = scaleService.GetOriginValueOfScale(selScale, 2.5);

            // Position : Leader Line
            POSITION_TYPE modelPosition = POSITION_TYPE.NotSet;
            double positionRotateValue = 0;

            double leaderAngle = selModel.leaderAngle;
            double leaderAngleTrans = 180 -leaderAngle;
            switch (selModel.position)
            {
                case ORIENTATION_TYPE.TOPLEFT:
                    positionRotateValue = Utility.DegToRad(leaderAngleTrans);
                    modelPosition = POSITION_TYPE.LEFT;
                    break;
                case ORIENTATION_TYPE.BOTTOMLEFT:
                    positionRotateValue = Utility.DegToRad(-leaderAngleTrans);
                    modelPosition = POSITION_TYPE.LEFT;
                    break;
                case ORIENTATION_TYPE.TOPRIGHT:
                    positionRotateValue = Utility.DegToRad(leaderAngle);
                    modelPosition = POSITION_TYPE.RIGHT;
                    break;
                case ORIENTATION_TYPE.BOTTOMRIGHT:
                    positionRotateValue = Utility.DegToRad(-leaderAngle);
                    modelPosition = POSITION_TYPE.RIGHT;
                    break;
            }

            // Arrow : Right
            Line arrowLine01 = new Line(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, scaleArrowHeadWidth, -scaleArrowHeadHeight / 2));
            arrowLine01.Rotate(positionRotateValue, Vector3D.AxisZ, GetSumPoint(selPoint, 0, 0));
            Line arrowLine02 = new Line(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, scaleArrowHeadWidth, +scaleArrowHeadHeight / 2));
            arrowLine02.Rotate(positionRotateValue, Vector3D.AxisZ, GetSumPoint(selPoint, 0, 0));
            Line arrowLine03 = new Line(GetSumPoint(selPoint, scaleArrowHeadWidth, +scaleArrowHeadHeight / 2), GetSumPoint(selPoint, scaleArrowHeadWidth, -scaleArrowHeadHeight / 2));
            arrowLine03.Rotate(positionRotateValue, Vector3D.AxisZ, GetSumPoint(selPoint, 0, 0));



            // Leader Line : Rotate
            Line leaderLine = new Line(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, scaleLeaderLineLength, 0));
            leaderLine.Rotate(positionRotateValue, Vector3D.AxisZ, GetSumPoint(selPoint, 0, 0));

            // All Around Point
            Point3D allAroundPoint = GetSumPoint(leaderLine.EndPoint, 0, 0);
            if (!selModel.leaderLineVisible)
                allAroundPoint = GetSumPoint(selPoint, 0, 0); // Point Adj
            Circle allAroundCir = new Circle(GetSumPoint(allAroundPoint, 0, 0), scaleAllAroundWeldRadius);




            double scaleS = scaleService.GetOriginValueOfScale(selScale, 6);
            double scaleOne = scaleService.GetOriginValueOfScale(selScale, 1);
            double scaleOnePointFive = scaleService.GetOriginValueOfScale(selScale, 1.5);
            double scaleTwo = scaleService.GetOriginValueOfScale(selScale, 2);
            double scaleTwoPointFive = scaleService.GetOriginValueOfScale(selScale, 2.5);
            double scaleTwoPointTwoFive = scaleService.GetOriginValueOfScale(selScale, 2.25);
            double scaleThree = scaleService.GetOriginValueOfScale(selScale, 3);
            double scaleFourPointTwo = scaleService.GetOriginValueOfScale(selScale, 4.2);
            double scaleFive = scaleService.GetOriginValueOfScale(selScale, 5);
            double scaleFivePointFive = scaleService.GetOriginValueOfScale(selScale, 5.5);
            double scaleSix = scaleService.GetOriginValueOfScale(selScale, 6);
            double scaleSevenPointFive = scaleService.GetOriginValueOfScale(selScale, 7.5);
            double scaleNine = scaleService.GetOriginValueOfScale(selScale, 9);
            double scaleTen = scaleService.GetOriginValueOfScale(selScale, 10);

            double rotate45 = Utility.DegToRad(45);
            double rotate60 = Utility.DegToRad(60);





            double currentReferenceLength = 0;
            List<double> textLength = new List<double>();

            List<Entity> weldList = new List<Entity>();
            List<Entity> weldTextList = new List<Entity>();

            // Both
            if (selModel.weldDetailType != WeldSymbolDetail_Type.BothSide)
            {
                selModel.weldSize2 = selModel.weldSize1;

                selModel.weldLength2 = selModel.weldLength1;
                selModel.weldQuantity2 = selModel.weldQuantity1;
                selModel.weldPitch2 = selModel.weldPitch1;

                selModel.weldAngle2 = selModel.weldAngle1;

                selModel.weldRoot2 = selModel.weldRoot1;
            }

            // Left Right
            double sizeLength = 0;
            double lnpLength = 0;

            // Current Point Length
            double currentSix = 0;
            double currentSevenPointFive = 0;
            double currentTwo = 0;
            double currentThree = 0;
            double currentTwoPointFive = 0;
            double currentFive = 0;

            Text.alignmentType lnpUpAlign = Text.alignmentType.BaselineLeft;
            Text.alignmentType lnpDownAlign = Text.alignmentType.TopLeft;
            switch (modelPosition)
            {
                case POSITION_TYPE.LEFT:
                    sizeLength = -scaleThree;
                    lnpLength = -scaleNine - scaleOne;
                    lnpUpAlign = Text.alignmentType.BaselineRight;
                    lnpDownAlign = Text.alignmentType.TopRight;

                    currentSix = -scaleSix;
                    currentSevenPointFive = -scaleSevenPointFive;
                    currentTwo = -scaleTwo;
                    currentThree = -scaleThree;
                    currentTwoPointFive = -scaleTwoPointFive;
                    currentFive = -scaleFive;
                    break;
                case POSITION_TYPE.RIGHT:
                    sizeLength = scaleThree;
                    lnpLength = scaleNine + scaleOne;
                    lnpUpAlign = Text.alignmentType.BaselineLeft;
                    lnpDownAlign = Text.alignmentType.TopLeft;

                    currentSix = scaleSix;
                    currentSevenPointFive = scaleSevenPointFive;
                    break;
            }

            Point3D tempPoint = GetSumPoint(selPoint, 0, 0);

            // OhterSide : Up
            if (selModel.weldDetailType == WeldSymbolDetail_Type.OtherSide || selModel.weldDetailType == WeldSymbolDetail_Type.BothSide)
            {
                // S
                string sizeStr = selModel.weldSize1;
                if (sizeStr != "")
                    weldTextList.Add(new Text(GetSumPoint(allAroundPoint, sizeLength, scaleTwoPointTwoFive), sizeStr, scaleTextHeight) { Alignment = Text.alignmentType.MiddleCenter });

                // L(n)P
                string lnpStr = "";
                if (selModel.weldLength1 != "")
                    lnpStr = selModel.weldLength1;
                if (selModel.weldQuantity1 != "")
                    lnpStr += "(" + selModel.weldQuantity1 + ")";
                if (selModel.weldPitch1 != "")
                    lnpStr += "-" + selModel.weldPitch1;
                if (lnpStr != "")
                {
                    Text lnpText = new Text(GetSumPoint(allAroundPoint, lnpLength, scaleOne), lnpStr, scaleTextHeight) { Alignment = lnpUpAlign };
                    lnpText.Regen(new RegenParams(0, singleModel));
                    textLength.Add(lnpText.BoxSize.X);
                    weldTextList.Add(lnpText);
                }


                // Weld Type : 계속 추가 해야 함
                double faceWidth = 0;
                Point3D currentPoint = GetSumPoint(allAroundPoint, 0, 0);
                switch (selModel.weldTypeUp)
                {
                    case WeldSymbol_Type.Fillet:
                        faceWidth = scaleFourPointTwo;
                        currentPoint = GetSumPoint(currentPoint, currentSix + currentThree, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, scaleThree)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, scaleThree), GetSumPoint(currentPoint, scaleThree, 0)));
                        currentPoint = GetSumPoint(currentPoint, 0, scaleThree);
                        break;

                    case WeldSymbol_Type.V:
                        double vLength = scaleService.GetOriginValueOfScale(selScale, 3 / Math.Sqrt(3));
                        faceWidth = vLength * 2;
                        currentPoint = GetSumPoint(currentPoint, currentSevenPointFive, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, vLength, scaleThree)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, -vLength, scaleThree)));
                        currentPoint = GetSumPoint(currentPoint, 0, scaleThree);
                        break;
                    case WeldSymbol_Type.Bevel:
                        faceWidth = scaleThree;
                        currentPoint = GetSumPoint(currentPoint, currentSix + currentThree, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, scaleThree)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, scaleThree, scaleThree)));
                        currentPoint = GetSumPoint(currentPoint, scaleOnePointFive, scaleThree);
                        break;
                    case WeldSymbol_Type.Square:
                        faceWidth = scaleTwoPointFive;
                        currentPoint = GetSumPoint(currentPoint, currentSix + currentThree, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, scaleThree)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, scaleTwoPointFive, 0), GetSumPoint(currentPoint, scaleTwoPointFive, scaleThree)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwoPointFive / 2, scaleThree);
                        break;
                    case WeldSymbol_Type.Plug:
                        faceWidth = scaleThree;
                        currentPoint = GetSumPoint(currentPoint, currentSix + currentThree, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, scaleTwo)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, scaleThree, 0), GetSumPoint(currentPoint, scaleThree, scaleTwo)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, scaleTwo), GetSumPoint(currentPoint, scaleThree, scaleTwo)));
                        currentPoint = GetSumPoint(currentPoint, scaleThree / 2, scaleTwo);
                        break;
                    case WeldSymbol_Type.FlareV:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, currentSix + currentThree, 0);
                        weldList.Add(new Arc(GetSumPoint(currentPoint, -scaleThree, 0), GetSumPoint(currentPoint, -scaleThree, scaleThree), GetSumPoint(currentPoint, 0, 0)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, scaleTwo + scaleThree, 0), GetSumPoint(currentPoint, scaleTwo, 0), GetSumPoint(currentPoint, scaleTwo + scaleThree, scaleThree)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwo / 2, scaleThree);
                        break;
                    case WeldSymbol_Type.FlareBevel:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, currentSix + currentFive, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, scaleThree)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, scaleTwo + scaleThree, 0), GetSumPoint(currentPoint, scaleTwo, 0), GetSumPoint(currentPoint, scaleTwo + scaleThree, scaleThree)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwo / 2, scaleThree);
                        break;
                    case WeldSymbol_Type.FlangeV:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, currentSix + currentTwo, 0);
                        weldList.Add(new Arc(GetSumPoint(currentPoint, -scaleThree, scaleThree), GetSumPoint(currentPoint, 0, scaleThree), GetSumPoint(currentPoint, -scaleThree, 0)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, scaleTwo + scaleThree, scaleThree), GetSumPoint(currentPoint, scaleTwo, scaleThree), GetSumPoint(currentPoint, scaleTwo + scaleThree, 0)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwo / 2, scaleThree);
                        break;
                    case WeldSymbol_Type.FlangeBevel:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, currentSix + currentFive, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, scaleThree)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, scaleTwo + scaleThree, scaleThree), GetSumPoint(currentPoint, scaleTwo, scaleThree), GetSumPoint(currentPoint, scaleTwo + scaleThree, 0)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwo / 2, scaleThree);
                        break;
                    case WeldSymbol_Type.J:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, currentSix + currentThree, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, scaleFivePointFive)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, 0, scaleFivePointFive), GetSumPoint(currentPoint, scaleThree, scaleFivePointFive), GetSumPoint(currentPoint, 0, scaleTwoPointFive)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwo / 2, scaleFivePointFive);
                        break;
                    case WeldSymbol_Type.U:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, currentSevenPointFive, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, scaleTwoPointFive)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, -scaleThree, scaleFivePointFive), GetSumPoint(currentPoint, 0, scaleTwoPointFive), GetSumPoint(currentPoint, scaleThree, scaleFivePointFive), false));
                        currentPoint = GetSumPoint(currentPoint, 0, scaleFivePointFive);
                        break;

                }


                // Root
                if (selModel.weldRoot1 != "")
                {
                    currentPoint = GetSumPoint(currentPoint, 0, scaleOnePointFive);
                    tempPoint = (Point3D)currentPoint.Clone();
                    if (modelPosition == POSITION_TYPE.LEFT)
                        editingService.SetMirrorPoint(Plane.YZ, ref tempPoint, GetSumPoint(allAroundPoint, 0, 0));

                    weldTextList.Add(new Text(GetSumPoint(tempPoint, 0, 0), selModel.weldRoot1, scaleOnePointFive) { Alignment = Text.alignmentType.MiddleCenter });
                    // Angle
                    if (selModel.weldAngle1 != "")
                    {
                        currentPoint = GetSumPoint(currentPoint, 0, scaleTwoPointFive);
                        tempPoint = (Point3D)currentPoint.Clone();
                        if (modelPosition == POSITION_TYPE.LEFT)
                            editingService.SetMirrorPoint(Plane.YZ, ref tempPoint, GetSumPoint(allAroundPoint, 0, 0));
                        weldTextList.Add(new Text(GetSumPoint(tempPoint, 0, 0), selModel.weldAngle1, scaleTwoPointFive) { Alignment = Text.alignmentType.MiddleCenter });
                        currentPoint = GetSumPoint(currentPoint, 0, scaleOne);
                    }
                }
                else
                {
                    // Angle
                    if (selModel.weldAngle1 != "")
                    {
                        currentPoint = GetSumPoint(currentPoint, 0, scaleOnePointFive);
                        tempPoint = (Point3D)currentPoint.Clone();
                        if (modelPosition == POSITION_TYPE.LEFT)
                            editingService.SetMirrorPoint(Plane.YZ, ref tempPoint, GetSumPoint(allAroundPoint, 0, 0));
                        weldTextList.Add(new Text(GetSumPoint(tempPoint, 0, 0), selModel.weldAngle1, scaleTwoPointFive) { Alignment = Text.alignmentType.MiddleCenter });
                        currentPoint = GetSumPoint(currentPoint, 0, scaleOne);
                    }
                }

                // Face
                Point3D facePoint = GetSumPoint(currentPoint, 0, 0);
                double faceRotate = 0;

                switch (selModel.weldTypeUp)
                {
                    case WeldSymbol_Type.Fillet:
                        if (weldList[weldList.Count - 1] is Line)
                            facePoint = GetSumPoint(((Line)weldList[weldList.Count - 1]).MidPoint, scaleOne / Math.Sqrt(2), scaleOne / Math.Sqrt(2));
                        faceRotate = -rotate45;
                        break;
                    default:
                        facePoint = GetSumPoint(facePoint, 0, scaleOne);
                        break;
                }


                switch (selModel.weldFaceUp)
                {
                    case WeldFace_Type.Convex:
                        Arc convexArc = new Arc(GetSumPoint(facePoint, -faceWidth / 2, 0), GetSumPoint(facePoint, 0, scaleOne), GetSumPoint(facePoint, faceWidth / 2, 0), false);
                        convexArc.Rotate(faceRotate, Vector3D.AxisZ, GetSumPoint(facePoint, 0, 0));
                        weldList.Add(convexArc);
                        break;
                    case WeldFace_Type.Concave:
                        Arc concaveArc = new Arc(GetSumPoint(facePoint, -faceWidth / 2, scaleOne), GetSumPoint(facePoint, 0, 0), GetSumPoint(facePoint, faceWidth / 2, scaleOne), false);
                        concaveArc.Rotate(faceRotate, Vector3D.AxisZ, GetSumPoint(facePoint, 0, 0));
                        weldList.Add(concaveArc);
                        break;
                    case WeldFace_Type.Flat:
                        Line flatLine = new Line(GetSumPoint(facePoint, -faceWidth / 2, 0), GetSumPoint(facePoint, faceWidth / 2, 0));
                        flatLine.Rotate(faceRotate, Vector3D.AxisZ, GetSumPoint(facePoint, 0, 0));
                        weldList.Add(flatLine);

                        // Only Flat Matching
                        if (selModel.machiningVisible)
                        {
                            Line flatOffsetLine = (Line)flatLine.Offset(-scaleTwo, Vector3D.AxisZ);
                            if (modelPosition == POSITION_TYPE.LEFT)
                                editingService.SetMirrorLine(Plane.YZ, ref flatOffsetLine, GetSumPoint(allAroundPoint, 0, 0));
                            Text matchingText = new Text(GetSumPoint(flatOffsetLine.MidPoint, 0, 0), selModel.machiningStr, scaleTextHeight) { Alignment = Text.alignmentType.MiddleCenter };
                            weldTextList.Add(matchingText);
                        }

                        break;
                }

            }

            // ArrowSide : Down
            if (selModel.weldDetailType == WeldSymbolDetail_Type.ArrowSide || selModel.weldDetailType == WeldSymbolDetail_Type.BothSide)
            {
                // S
                string sizeStr = selModel.weldSize2;
                if (sizeStr != "")
                    weldTextList.Add(new Text(GetSumPoint(allAroundPoint, sizeLength, -scaleTwoPointTwoFive), sizeStr, scaleTextHeight) { Alignment = Text.alignmentType.MiddleCenter });

                // L(n)P
                string lnpStr = "";
                if (selModel.weldLength2 != "")
                    lnpStr = selModel.weldLength2;
                if (selModel.weldQuantity2 != "")
                    lnpStr += "(" + selModel.weldQuantity2 + ")";
                if (selModel.weldPitch2 != "")
                    lnpStr += "-" + selModel.weldPitch2;
                if (lnpStr != "")
                {
                    Text lnpText = new Text(GetSumPoint(allAroundPoint, lnpLength, -scaleOne), lnpStr, scaleTextHeight) { Alignment = lnpDownAlign };
                    lnpText.Regen(new RegenParams(0, singleModel));
                    textLength.Add(lnpText.BoxSize.X);
                    weldTextList.Add(lnpText);
                }


                // Weld Type : 계속 추가 해야 함
                double faceWidth = 0;
                Point3D currentPoint = GetSumPoint(allAroundPoint, 0, 0);
                switch (selModel.weldTypeDown)
                {
                    case WeldSymbol_Type.Fillet:
                        faceWidth = scaleFourPointTwo;
                        currentPoint = GetSumPoint(currentPoint, currentSix + currentThree, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, -scaleThree)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, -scaleThree), GetSumPoint(currentPoint, scaleThree, 0)));
                        currentPoint = GetSumPoint(currentPoint, 0, -scaleThree);
                        break;
                    case WeldSymbol_Type.V:
                        double vLength = scaleService.GetOriginValueOfScale(selScale, 3 / Math.Sqrt(3));
                        faceWidth = vLength * 2;
                        currentPoint = GetSumPoint(currentPoint, currentSevenPointFive, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, vLength, -scaleThree)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, -vLength, -scaleThree)));
                        currentPoint = GetSumPoint(currentPoint, 0, -scaleThree);
                        break;
                    case WeldSymbol_Type.Bevel:
                        faceWidth = scaleThree;
                        currentPoint = GetSumPoint(currentPoint, currentSix + currentThree, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, -scaleThree)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, scaleThree, -scaleThree)));
                        currentPoint = GetSumPoint(currentPoint, scaleOnePointFive, -scaleThree);
                        break;
                    case WeldSymbol_Type.Square:
                        faceWidth = scaleTwoPointFive;
                        currentPoint = GetSumPoint(currentPoint, currentSix+ currentThree, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, -scaleThree)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, scaleTwoPointFive, 0), GetSumPoint(currentPoint, scaleTwoPointFive, -scaleThree)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwoPointFive / 2, -scaleThree);
                        break;
                    case WeldSymbol_Type.Plug:
                        faceWidth = scaleThree;
                        currentPoint = GetSumPoint(currentPoint, currentSix + currentThree, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, -scaleTwo)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, scaleThree, 0), GetSumPoint(currentPoint, scaleThree, -scaleTwo)));
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, -scaleTwo), GetSumPoint(currentPoint, scaleThree, -scaleTwo)));
                        currentPoint = GetSumPoint(currentPoint, scaleThree / 2, -scaleTwo);
                        break;
                    case WeldSymbol_Type.FlareV:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, currentSix + currentThree, 0);
                        weldList.Add(new Arc(GetSumPoint(currentPoint, -scaleThree, 0), GetSumPoint(currentPoint, -scaleThree, -scaleThree), GetSumPoint(currentPoint, 0, 0)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, scaleTwo + scaleThree, 0), GetSumPoint(currentPoint, scaleTwo, 0), GetSumPoint(currentPoint, scaleTwo + scaleThree, -scaleThree)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwo / 2, -scaleThree);
                        break;
                    case WeldSymbol_Type.FlareBevel:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, currentSix + currentFive, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, -scaleThree)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, scaleTwo + scaleThree, 0), GetSumPoint(currentPoint, scaleTwo, 0), GetSumPoint(currentPoint, scaleTwo + scaleThree, -scaleThree)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwo / 2, -scaleThree);
                        break;
                    case WeldSymbol_Type.FlangeV:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, currentSix + currentTwo, 0);
                        weldList.Add(new Arc(GetSumPoint(currentPoint, -scaleThree, -scaleThree), GetSumPoint(currentPoint, 0, -scaleThree), GetSumPoint(currentPoint, -scaleThree, 0)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, scaleTwo + scaleThree, -scaleThree), GetSumPoint(currentPoint, scaleTwo, -scaleThree), GetSumPoint(currentPoint, scaleTwo + scaleThree, 0)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwo / 2, -scaleThree);
                        break;
                    case WeldSymbol_Type.FlangeBevel:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, currentSix + currentFive, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, -scaleThree)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, scaleTwo + scaleThree, -scaleThree), GetSumPoint(currentPoint, scaleTwo, -scaleThree), GetSumPoint(currentPoint, scaleTwo + scaleThree, 0)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwo / 2, -scaleThree);
                        break;
                    case WeldSymbol_Type.J:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, currentSix + currentThree, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, -scaleFivePointFive)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, 0, -scaleFivePointFive), GetSumPoint(currentPoint, scaleThree, -scaleFivePointFive), GetSumPoint(currentPoint, 0, -scaleTwoPointFive)));
                        currentPoint = GetSumPoint(currentPoint, scaleTwo / 2, -scaleFivePointFive);
                        break;
                    case WeldSymbol_Type.U:
                        faceWidth = scaleTwo + scaleThree * 2;
                        currentPoint = GetSumPoint(currentPoint, currentSevenPointFive, 0);
                        weldList.Add(new Line(GetSumPoint(currentPoint, 0, 0), GetSumPoint(currentPoint, 0, -scaleTwoPointFive)));
                        weldList.Add(new Arc(GetSumPoint(currentPoint, -scaleThree, -scaleFivePointFive), GetSumPoint(currentPoint, 0, -scaleTwoPointFive), GetSumPoint(currentPoint, scaleThree, -scaleFivePointFive), false));
                        currentPoint = GetSumPoint(currentPoint, 0, -scaleFivePointFive);
                        break;

                }


                // Root
                if (selModel.weldRoot2 != "")
                {
                    currentPoint = GetSumPoint(currentPoint, 0, -scaleOnePointFive);
                    tempPoint = (Point3D)currentPoint.Clone();
                    if (modelPosition == POSITION_TYPE.LEFT)
                        editingService.SetMirrorPoint(Plane.YZ, ref tempPoint, GetSumPoint(allAroundPoint, 0, 0));
                    weldTextList.Add(new Text(GetSumPoint(tempPoint, 0, 0), selModel.weldRoot2, scaleOnePointFive) { Alignment = Text.alignmentType.MiddleCenter });
                    // Angle
                    if (selModel.weldAngle2 != "")
                    {
                        currentPoint = GetSumPoint(currentPoint, 0, -scaleTwoPointFive);
                        tempPoint = (Point3D)currentPoint.Clone();
                        if (modelPosition == POSITION_TYPE.LEFT)
                            editingService.SetMirrorPoint(Plane.YZ, ref tempPoint, GetSumPoint(allAroundPoint, 0, 0));
                        weldTextList.Add(new Text(GetSumPoint(tempPoint, 0, 0), selModel.weldAngle2, scaleTwoPointFive) { Alignment = Text.alignmentType.MiddleCenter });
                        currentPoint = GetSumPoint(currentPoint, 0, -scaleOne);
                    }
                }
                else
                {
                    // Angle
                    if (selModel.weldAngle2 != "")
                    {
                        currentPoint = GetSumPoint(currentPoint, 0, -scaleOnePointFive);
                        tempPoint = (Point3D)currentPoint.Clone();
                        //if (modelPosition == POSITION_TYPE.LEFT)
                        //    editingService.SetMirrorPoint(Plane.YZ, ref tempPoint, GetSumPoint(allAroundPoint, 0, 0));
                        weldTextList.Add(new Text(GetSumPoint(tempPoint, 0, 0), selModel.weldAngle2, scaleTwoPointFive) { Alignment = Text.alignmentType.MiddleCenter });
                        currentPoint = GetSumPoint(currentPoint, 0, -scaleOne);
                    }
                }

                // Face
                Point3D facePoint = GetSumPoint(currentPoint, 0, 0);
                double faceRotate = 0;

                switch (selModel.weldTypeDown)
                {
                    case WeldSymbol_Type.Fillet:
                        if (weldList[weldList.Count - 1] is Line)
                            facePoint = GetSumPoint(((Line)weldList[weldList.Count - 1]).MidPoint, scaleOne / Math.Sqrt(2), -scaleOne / Math.Sqrt(2));
                        faceRotate = rotate45;
                        break;
                    default:
                        facePoint = GetSumPoint(facePoint, 0, -scaleOne);
                        break;
                }


                switch (selModel.weldFaceDown)
                {
                    case WeldFace_Type.Convex:
                        Arc convexArc = new Arc(GetSumPoint(facePoint, -faceWidth / 2, 0), GetSumPoint(facePoint, 0, -scaleOne), GetSumPoint(facePoint, faceWidth / 2, 0), false);
                        convexArc.Rotate(faceRotate, Vector3D.AxisZ, GetSumPoint(facePoint, 0, 0));
                        weldList.Add(convexArc);
                        break;
                    case WeldFace_Type.Concave:
                        Arc concaveArc = new Arc(GetSumPoint(facePoint, -faceWidth / 2, -scaleOne), GetSumPoint(facePoint, 0, 0), GetSumPoint(facePoint, faceWidth / 2, -scaleOne), false);
                        concaveArc.Rotate(faceRotate, Vector3D.AxisZ, GetSumPoint(facePoint, 0, 0));
                        weldList.Add(concaveArc);
                        break;
                    case WeldFace_Type.Flat:
                        Line flatLine = new Line(GetSumPoint(facePoint, -faceWidth / 2, 0), GetSumPoint(facePoint, faceWidth / 2, 0));
                        flatLine.Rotate(faceRotate, Vector3D.AxisZ, GetSumPoint(facePoint, 0, 0));
                        weldList.Add(flatLine);

                        // Only Flat Matching
                        if (selModel.machiningVisible)
                        {
                            Line flatOffsetLine = (Line)flatLine.Offset(scaleTwo, Vector3D.AxisZ);
                            if (modelPosition == POSITION_TYPE.LEFT)
                                editingService.SetMirrorLine(Plane.YZ, ref flatOffsetLine, GetSumPoint(allAroundPoint, 0, 0));
                            Text matchingText = new Text(GetSumPoint(flatOffsetLine.MidPoint, 0, 0), selModel.machiningStr, scaleTextHeight) { Alignment = Text.alignmentType.MiddleCenter };
                            weldTextList.Add(matchingText);
                        }

                        break;
                }

            }

            // Reference Line : Length : adj
            double maxTextLength = 0;
            foreach (double eachLength in textLength)
                if (maxTextLength < eachLength)
                    maxTextLength = eachLength;

            currentReferenceLength = scaleNine + maxTextLength + scaleTwo;

            // Minmum Length
            if (currentReferenceLength < scaleReferenceLineLength)
                currentReferenceLength = scaleReferenceLineLength;


            // Reference, Tail, Field
            Text.alignmentType tailTextAlign = Text.alignmentType.BaselineLeft;
            Line referenceLine = null;
            Line tailUpLine = null;
            Line tailDownLine = null;
            Line fieldLine = null;
            Line fieldTriTop = null;
            Line fieldTriBottom = null;

            switch (modelPosition)
            {
                case POSITION_TYPE.LEFT:
                    tailTextAlign = Text.alignmentType.BaselineRight;
                    referenceLine = new Line(GetSumPoint(allAroundPoint, 0, 0), GetSumPoint(allAroundPoint, -currentReferenceLength, 0));
                    tailUpLine = new Line(GetSumPoint(referenceLine.EndPoint, 0, 0), GetSumPoint(referenceLine.EndPoint, -scaleTailLineLength, scaleTailLineLength));
                    tailDownLine = new Line(GetSumPoint(referenceLine.EndPoint, 0, 0), GetSumPoint(referenceLine.EndPoint, -scaleTailLineLength, -scaleTailLineLength));
                    fieldLine = new Line(GetSumPoint(allAroundPoint, 0, 0), GetSumPoint(allAroundPoint, 0, scaleFieldLength));
                    fieldTriTop = new Line(GetSumPoint(fieldLine.EndPoint, 0, 0), GetSumPoint(fieldLine.EndPoint, -scaleFieldFlagWidth, -scaleFieldFlagHeight / 2));
                    fieldTriBottom = new Line(GetSumPoint(fieldLine.EndPoint, 0, -scaleFieldFlagHeight), GetSumPoint(fieldLine.EndPoint, -scaleFieldFlagWidth, -scaleFieldFlagHeight / 2));
                    break;

                case POSITION_TYPE.RIGHT:
                    tailTextAlign = Text.alignmentType.BaselineLeft;
                    referenceLine = new Line(GetSumPoint(allAroundPoint, 0, 0), GetSumPoint(allAroundPoint, currentReferenceLength, 0));
                    tailUpLine = new Line(GetSumPoint(referenceLine.EndPoint, 0, 0), GetSumPoint(referenceLine.EndPoint, scaleTailLineLength, scaleTailLineLength));
                    tailDownLine = new Line(GetSumPoint(referenceLine.EndPoint, 0, 0), GetSumPoint(referenceLine.EndPoint, scaleTailLineLength, -scaleTailLineLength));
                    fieldLine = new Line(GetSumPoint(allAroundPoint, 0, 0), GetSumPoint(allAroundPoint, 0, scaleFieldLength));
                    fieldTriTop = new Line(GetSumPoint(fieldLine.EndPoint, 0, 0), GetSumPoint(fieldLine.EndPoint, scaleFieldFlagWidth, -scaleFieldFlagHeight / 2));
                    fieldTriBottom = new Line(GetSumPoint(fieldLine.EndPoint, 0, -scaleFieldFlagHeight), GetSumPoint(fieldLine.EndPoint, scaleFieldFlagWidth, -scaleFieldFlagHeight / 2));
                    break;
            }

            //if (modelPosition == POSITION_TYPE.LEFT)
            //{
            //    editingService.GetMirrorEntity(Plane.YZ, weldList, allAroundPoint.X, allAroundPoint.Y);
            //}


            // Tail Specification
            List<Text> tailSpecList = new List<Text>();
            if (selModel.specification != "")
            {
                if (selModel.specification.Contains(System.Environment.NewLine))
                {
                    string[] tempSpecification = selModel.specification.Split(new[] { System.Environment.NewLine }, StringSplitOptions.None);
                    if (tempSpecification.Length > 0)
                    {
                        int maxCount = 3;
                        int specCount = 0;
                        foreach (string eachSpec in tempSpecification)
                            if (eachSpec.Trim() != "")
                            {
                                specCount++;
                                tailSpecList.Add(new Text(GetSumPoint(allAroundPoint, 0, 0), eachSpec.Trim(), scaleTextHeight));
                                if (specCount == maxCount)
                                    break;
                            }
                    }
                }
                else
                {
                    tailSpecList.Add(new Text(GetSumPoint(allAroundPoint, 0, 0), selModel.specification, scaleTextHeight));
                }
            }
            if (tailSpecList.Count > 0)
            {
                foreach (Text eachText in tailSpecList)
                    eachText.Alignment = tailTextAlign;

                double scaleTailGap = scaleService.GetOriginValueOfScale(selScale, 1.75);
                double scaleTailTextGap = scaleService.GetOriginValueOfScale(selScale, 3.5);
                if (tailSpecList.Count == 1)
                {
                    tailSpecList[0].InsertionPoint = GetSumPoint(tailDownLine.EndPoint, 0, scaleTailGap);
                }
                else if (tailSpecList.Count == 2)
                {
                    tailSpecList[1].InsertionPoint = GetSumPoint(tailDownLine.EndPoint, 0, 0);
                    tailSpecList[0].InsertionPoint = GetSumPoint(tailSpecList[1].InsertionPoint, 0, scaleTailTextGap);
                }
                else if (tailSpecList.Count == 3)
                {
                    tailSpecList[2].InsertionPoint = GetSumPoint(tailDownLine.EndPoint, 0, -scaleTailGap);
                    tailSpecList[1].InsertionPoint = GetSumPoint(tailSpecList[2].InsertionPoint, 0, scaleTailTextGap);
                    tailSpecList[0].InsertionPoint = GetSumPoint(tailSpecList[1].InsertionPoint, 0, scaleTailTextGap);
                }
            }


            // Entity : Add
            if (selModel.arrowHeadVisible)
            {
                newList.Add(arrowLine01);
                newList.Add(arrowLine02);
                newList.Add(arrowLine03);
            }

            if (selModel.leaderLineVisible)
                newList.Add(leaderLine);
            if (selModel.allRoundWeldVisible)
                newList.Add(allAroundCir);

            if (selModel.tailVisible)
            {
                newList.Add(tailUpLine);
                newList.Add(tailDownLine);
            }
            if (selModel.fieldWeldVisible)
            {
                newList.Add(fieldLine);
                newList.Add(fieldTriTop);
                newList.Add(fieldTriBottom);
                newList.AddRange(tailSpecList);
            }
            if (weldList.Count > 0)
                newList.AddRange(weldList);
            if (weldTextList.Count > 0)
                newList.AddRange(weldTextList);

            newList.Add(referenceLine);

            return newList;
        }

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
    }
}
