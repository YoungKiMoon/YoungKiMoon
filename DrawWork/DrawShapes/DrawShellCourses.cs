using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawWork.DrawModels;
using DrawWork.DrawServices;
using DrawWork.DrawStyleServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawShapes
{
    public class DrawShellCourses
    {
        private StyleFunctionService styleService;
        private LayerStyleService layerService;

        private DrawEditingService editingService;

        public DrawSlopeSymbols slopeSymbolService;

        public DrawShellCourses()
        {
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();

            editingService = new DrawEditingService();

            slopeSymbolService = new DrawSlopeSymbols();
        }
        public List<Entity> GetPlateBlock(out List<Point3D> selOutputPointList, Point3D selPoint, double selWidth, double selHeight, double selRotate, double selRotateCenter, double selTranslateNumber, 
                                            DrawWeldingModel selWeldingModel, DrawCenterLineModel selCenterLine ,double  selScaleValue,out DrawSlopeLeaderModel outSlopeModel)
        {
            selOutputPointList = new List<Point3D>();
            List<Entity> newList = new List<Entity>();

            // Reference Point : Top Left
            Point3D A = GetSumPoint(selPoint, 0, 0);
            Point3D B = GetSumPoint(selPoint, selWidth, 0);
            Point3D C = GetSumPoint(selPoint, selWidth, -selHeight);
            Point3D D = GetSumPoint(selPoint, 0, -selHeight);

            Line lineA = new Line(GetSumPoint(A, 0, 0), GetSumPoint(B, 0, 0));
            Line lineB = new Line(GetSumPoint(B, 0, 0), GetSumPoint(C, 0, 0));
            Line lineC = new Line(GetSumPoint(C, 0, 0), GetSumPoint(D, 0, 0));
            Line lineD = new Line(GetSumPoint(D, 0, 0), GetSumPoint(A, 0, 0));

            Line lineMidVertical = new Line(GetSumPoint(selPoint, selWidth / 2, 0), GetSumPoint(selPoint, selWidth / 2, -selHeight));

            newList.AddRange(new Line[] { lineA, lineB, lineC, lineD });

            outSlopeModel = null;

            // Chamfer
            if (selWeldingModel.TopChamfer)
            {
                if (selWeldingModel.ChamferShort > 0 && selWeldingModel.ChamferLong > 0)
                {
                    lineA.TrimBy(GetSumPoint(A, selWeldingModel.ChamferShort, 0), true);
                    lineD.TrimBy(GetSumPoint(A, 0, -selWeldingModel.ChamferLong), false);
                    Line chamferLine = new Line(GetSumPoint(lineA.StartPoint, 0, 0), GetSumPoint(lineD.EndPoint, 0, 0));
                    newList.Add(chamferLine);

                    // Add : Slope Symbol
                    DrawSlopeLeaderModel newSlopeModel = new DrawSlopeLeaderModel();
                    newSlopeModel.leaderAngleRadian = editingService.GetAngleOfLine(chamferLine);
                    newSlopeModel.heightValue = 1;
                    newSlopeModel.widthValue = 3; //1:3;
                    newSlopeModel.insertPoint = chamferLine.EndPoint;
                    // Very Important
                    outSlopeModel = newSlopeModel;
                    
                }
            }

            List<List<Entity>> RowList = new List<List<Entity>>();
            List<Entity> plateRow1 = new List<Entity>();

            // Trasnlate Informattion
            List<Entity> transList = new List<Entity>();

            // Welding
            Arc leftWeldingArc = null;
            Arc rightWeldingArc = null; ;

            if (selWeldingModel.TopWelding)
            {
                // 아직
            }
            if (selWeldingModel.BottomWelding)
            {
                if (selWeldingModel.BottomWeldingSingle)
                {
                    // Single
                    Point3D bottomLeft = GetSumPoint(C, -selWeldingModel.MidDepth, selWeldingModel.OtherDistance);
                    Point3D bottomRight = GetSumPoint(C, 0, selWeldingModel.OtherDistance);

                    // Flat
                    Line lineBottomFlat = new Line(GetSumPoint(bottomLeft, 0, 0), GetSumPoint(bottomRight, 0, 0));
                    newList.Add(lineBottomFlat);
                    newList.Remove(lineC);

                    // Left
                    Line vlineLeft = new Line(GetSumPoint(bottomLeft, 0, 0), GetSumPoint(bottomLeft, -selWidth * 100, 0));
                    vlineLeft.Rotate(Utility.DegToRad(-selWeldingModel.LeftDegree), Vector3D.AxisZ, GetSumPoint(bottomLeft, 0, 0));
                    Point3D[] vlineLeftInter = vlineLeft.IntersectWith(lineD);
                    if (vlineLeftInter.Length > 0)
                    {
                        newList.Remove(lineD);
                        newList.Add(new Line(GetSumPoint(lineD.EndPoint, 0, 0), GetSumPoint(vlineLeftInter[0], 0, 0)));
                        newList.Add(new Line(GetSumPoint(bottomLeft, 0, 0), GetSumPoint(vlineLeftInter[0], 0, 0)));
                    }

                    // Left : Arc
                    if (vlineLeftInter.Length > 0)
                    {
                        double arcHeight = vlineLeftInter[0].Y - D.Y;
                        double arcWidth = selWidth * 0.15;//0.15 
                        Point3D leftMidPoint = GetSumPoint(D, -arcWidth, arcHeight / 2);
                        Point3D leftTopPoint = GetSumPoint(vlineLeftInter[0], 0, 0);
                        if (selWeldingModel.LeftWeldingArc)
                        {
                            leftWeldingArc = new Arc(leftTopPoint, leftMidPoint, D, false);
                            newList.Add(leftWeldingArc);
                        }
                        else
                        {
                            Line leftLine = new Line(leftTopPoint, D);
                            newList.Add(leftLine);
                        }
                    }

                    newList.Remove(lineB);
                    newList.Add(new Line(GetSumPoint(B, 0, 0), GetSumPoint(bottomRight, 0, 0)));

                }
                else
                {
                    // Double
                    Point3D bottomLeft = GetSumPoint(D, selWeldingModel.LeftDepth-selWeldingModel.MidDepth, selWeldingModel.OtherDistance);
                    Point3D bottomRight = GetSumPoint(C, -selWeldingModel.RightDepth, selWeldingModel.OtherDistance);

                    // Flat
                    Line lineBottomFlat = new Line(GetSumPoint(bottomLeft, 0, 0), GetSumPoint(bottomRight, 0, 0));
                    newList.Add(lineBottomFlat);
                    newList.Remove(lineC);

                    // Left
                    Line vlineLeft = new Line(GetSumPoint(bottomLeft, 0, 0), GetSumPoint(bottomLeft, -selWidth*100, 0));
                    vlineLeft.Rotate(Utility.DegToRad(-selWeldingModel.LeftDegree), Vector3D.AxisZ, GetSumPoint(bottomLeft, 0, 0));
                    Point3D[] vlineLeftInter = vlineLeft.IntersectWith(lineD);
                    if (vlineLeftInter.Length > 0)
                    {
                        newList.Remove(lineD);
                        newList.Add(new Line(GetSumPoint(lineD.EndPoint, 0, 0), GetSumPoint(vlineLeftInter[0], 0, 0)));
                        newList.Add(new Line(GetSumPoint(bottomLeft, 0, 0), GetSumPoint(vlineLeftInter[0], 0, 0)));
                    }

                    // Right
                    Line vlineRight = new Line(GetSumPoint(bottomRight, 0, 0), GetSumPoint(bottomRight, selWidth * 100, 0));
                    vlineRight.Rotate(Utility.DegToRad(selWeldingModel.RightDegree), Vector3D.AxisZ, GetSumPoint(bottomRight, 0, 0));
                    Point3D[] vlineRightInter = vlineRight.IntersectWith(lineB);
                    if (vlineRightInter.Length > 0)
                    {
                        newList.Remove(lineB);
                        newList.Add(new Line(GetSumPoint(B, 0, 0), GetSumPoint(vlineRightInter[0], 0, 0)));
                        newList.Add(new Line(GetSumPoint(bottomRight, 0, 0), GetSumPoint(vlineRightInter[0], 0, 0)));
                    }

                    // Left : Arc
                    if (vlineLeftInter.Length > 0)
                    {
                        double arcHeight = vlineLeftInter[0].Y - D.Y;
                        double arcWidth = selWidth * 0.15;//0.15 
                        Point3D leftMidPoint = GetSumPoint(D, -arcWidth, arcHeight / 2);
                        Point3D leftTopPoint = GetSumPoint(vlineLeftInter[0], 0, 0);
                        if (selWeldingModel.LeftWeldingArc)
                        {
                            leftWeldingArc = new Arc(leftTopPoint, leftMidPoint, D, false);
                            newList.Add(leftWeldingArc);
                        }
                        else
                        {
                            Line leftLine = new Line(leftTopPoint, D);
                            newList.Add(leftLine);
                        }
                    }

                    // Right : Arc
                    if (vlineRightInter.Length > 0)
                    {
                        double arcHeight = vlineRightInter[0].Y - C.Y;
                        double arcWidth = selWidth * 0.07;//0.07 
                        Point3D rightMidPoint = GetSumPoint(C, arcWidth, arcHeight / 2);
                        Point3D rightTopPoint = GetSumPoint(vlineRightInter[0], 0, 0);
                        if (selWeldingModel.RightWeldingArc)
                        {
                            rightWeldingArc = new Arc(rightTopPoint, rightMidPoint, C, false);
                            newList.Add(rightWeldingArc);
                        }
                        else
                        {
                            Line rightLine = new Line(rightTopPoint, C);
                            newList.Add(rightLine);
                        }
                    }
                }
            }

            if (selWeldingModel.BottomChamfer)
            {

            }

            // Center Line
            if (selCenterLine != null)
            {
                if (selCenterLine.centerLine)
                {
                    // 기본 형상
                    Point3D zeroPoint = lineMidVertical.EndPoint;
                    Point3D onePoint = lineMidVertical.StartPoint;
                    Line centerLine00 = new Line(zeroPoint, onePoint);

                    styleService.SetLayer(ref centerLine00, layerService.LayerCenterLine);
                    newList.Add(centerLine00);

                    // 0
                    if (selCenterLine.zeroEx)
                    {
                        Line zeroLine = new Line(GetSumPoint(zeroPoint, 0, 0), GetSumPoint(zeroPoint, 0, -selCenterLine.exLength * selCenterLine.scaleValue));
                        styleService.SetLayer(ref zeroLine, layerService.LayerCenterLine);
                        newList.Add(zeroLine);
                    }

                    // 1
                    if (selCenterLine.oneEx)
                    {
                        Line oneLine = new Line(GetSumPoint(onePoint, 0, 0), GetSumPoint(onePoint, 0, +selCenterLine.exLength * selCenterLine.scaleValue));
                        styleService.SetLayer(ref oneLine, layerService.LayerCenterLine);
                        newList.Add(oneLine);
                    }

                }
            }

            // Rotate
            if (selRotate != 0)
            {
                Point3D WPRotate = GetSumPoint(A, 0, 0);
                if (selRotateCenter == 1)
                    WPRotate = GetSumPoint(B, 0, 0);
                else if (selRotateCenter == 2)
                    WPRotate = GetSumPoint(C, 0, 0);
                else if (selRotateCenter == 3)
                    WPRotate = GetSumPoint(D, 0, 0);

                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }

            // Translate
            if (selTranslateNumber > 0)
            {
                Point3D WPTranslate = new Point3D();
                if (selTranslateNumber == 1)
                    WPTranslate = GetSumPoint(B, 0, 0);
                else if (selTranslateNumber == 2)
                    WPTranslate = GetSumPoint(C, 0, 0);
                else if (selTranslateNumber == 3)
                    WPTranslate = GetSumPoint(D, 0, 0);
                editingService.SetTranslate(ref newList, GetSumPoint(selPoint, 0, 0), WPTranslate);

                // Etc Translate
                if (selWeldingModel.BottomWelding)
                {
                    transList.Add(lineC);
                    editingService.SetTranslate(ref transList, GetSumPoint(selPoint, 0, 0), WPTranslate);
                }
            }


            // selOutputPointList
            selOutputPointList.Add(GetSumPoint(lineA.StartPoint, 0, 0));
            selOutputPointList.Add(GetSumPoint(lineA.EndPoint, 0, 0));
            selOutputPointList.Add(GetSumPoint(lineC.StartPoint, 0, 0));
            selOutputPointList.Add(GetSumPoint(lineC.EndPoint, 0, 0));

            // Output : Welding
            if(leftWeldingArc!=null)
                selOutputPointList.Add(GetSumPoint(leftWeldingArc.StartPoint, 0, 0));
            if (rightWeldingArc != null)
                selOutputPointList.Add(GetSumPoint(rightWeldingArc.StartPoint, 0, 0));


            return newList;
        }


        // 자동 적용시 
        private double getRoundOffOne(double selThk)
        {
            double divValue = 3;
            double oneWidth = Math.Round(selThk / divValue, MidpointRounding.AwayFromZero);
            return oneWidth;
        }

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
    }
}
