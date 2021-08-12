using AssemblyLib.AssemblyModels;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawWork.DrawModels;
using DrawWork.DrawServices;
using DrawWork.DrawShapes;
using DrawWork.DrawStyleServices;
using DrawWork.ValueServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawDetailServices
{
    public class DrawDetailTempService
    {
        private Model singleModel;
        private AssemblyModel assemblyData;

        private DrawService drawService;


        private ValueService valueService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;

        private DrawShellCourses dsService;
        private DrawBreakSymbols shapeService;
        public DrawDetailTempService(AssemblyModel selAssembly, object selModel)
        {
            singleModel = selModel as Model;

            assemblyData = selAssembly;

            drawService = new DrawService(selAssembly);

            valueService = new ValueService();
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();

            dsService = new DrawShellCourses();
            shapeService = new DrawBreakSymbols();
        }
        public enum RECTANGLEALIGN { NONE, CENTER, LEFT, RIGHT, TOP, BOTTOM, }
        public enum RECTANGLUNVIEW { NONE, LEFT, RIGHT, TOP, BOTTOM, }
        public enum PLATEPOSITION { ZERO, START, MIDDLE, ADD, END, }
        public enum TODOLASTPLATE { NOTTING, SHIFT, ADD, }

        public DrawEntityModel DrawBottomPlateJoint_POINT_Case1(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double scaleValue)
        {

            DrawEntityModel drawList = new DrawEntityModel();

            // DE4TAIL "A"  CASE 1

            singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Line> diagonalLineList = new List<Line>();
            List<Line> verticalLineList = new List<Line>();
            List<Line> horizontalLineList = new List<Line>();

            List<List<Point3D>> JointPointList = new List<List<Point3D>>();
            List<double[]> extendPointXList = new List<double[]>();


            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);
            

            /////////////////////////////
            //      Information      ////
            /////////////////////////////

            // distance size info
            double sumYpoint = 0;
            double[] Ypoint = new double[] { 0, (sumYpoint += 3), (sumYpoint += 4), (sumYpoint += 4), (sumYpoint += 50) };
            //int YpointCount = Ypoint.Length;

            // setting Point
            extendPointXList.Add(new double[] {
                0, 110
            });

            extendPointXList.Add(new double[] {
                0, 5, 100, 5, (-5 -20 +80 -13), 35
            });

            extendPointXList.Add(new double[] {
                5, 25, 4, (-4 -25 +100 -20 -4), 4, 20, (-20 +40), 3, (37-13), 13, 4, (-4 -13 +35)
            });

            extendPointXList.Add(new double[] {
                (110 - 5 - 20), 40, 3, 37
            });

            extendPointXList.Add(new double[] {
                (5+25), 4
            });



            //  insert Point : calculate Point and insert
            for (int i = 0; i < Ypoint.Length; i++)
            {
                JointPointList.Add(new List<Point3D>());
                double sumPointX = 0;
                foreach (double eachExtendX in extendPointXList[i])
                {
                    sumPointX += eachExtendX;
                    JointPointList[i].Add(GetSumPoint(referencePoint, sumPointX, Ypoint[i]));
                }
            }


            // Draw HorizontalLine
            horizontalLineList.AddRange(new Line[] { 
                // Y0 Line
                new Line( JointPointList[0][0], JointPointList[0][1] ),
                // Y1 Line
                new Line( JointPointList[1][0], JointPointList[1][3] ),
                new Line( JointPointList[1][4], JointPointList[1][5] ),
                // Y2 Line
                new Line( JointPointList[2][0], JointPointList[2][11] ),
                // Y3 Line
                new Line( JointPointList[3][0], JointPointList[3][3] ),
                new Line( JointPointList[3][2], JointPointList[3][3] ),
            });


            // Draw VerticalLine
            verticalLineList.AddRange(new Line[] { 
                // vertical No.1 Line
                new Line( JointPointList[0][0], JointPointList[1][0] ),
                new Line( JointPointList[0][1], JointPointList[1][3] ),
                // vertical No.2 Line
                new Line( JointPointList[1][1], JointPointList[2][0] ),
                new Line( JointPointList[1][2], JointPointList[2][5] ),
                new Line( JointPointList[1][4], JointPointList[2][8] ),
                //new Line( JointPointList[1][5], JointPointList[2][11] ),
                // vertical No.3 Line
                new Line( JointPointList[2][4], JointPointList[3][0] ),
                new Line( JointPointList[2][9], JointPointList[3][3] ),
                // vertical No.4 Line
                new Line( JointPointList[2][1], JointPointList[4][0] ),
                new Line( JointPointList[2][2], JointPointList[4][1] ),
            });


            // Draw diagonallLine
            diagonalLineList.AddRange(new Line[] { 
                // left
                new Line( JointPointList[2][3], JointPointList[3][0] ),
                // right
                new Line( JointPointList[2][10], JointPointList[3][3] ),
            });




            // Vertical Demention
            /*

             - LEFT Position -
            * t6
            JointPointList[0][0], JointPointList[1][0]
            * t10
            JointPointList[1][1], JointPointList[2][0]

             - Rigth Position -
            # t8 (bottom)
            JointPointList[1][5], JointPointList[2][11]
            # t8 
            JointPointList[2][11], JointPointList[3][3]

            /**/



            // Spline Demention
            /*

             - LEFT Top Position -
            JointPointList[4][0], JointPointList[4][1]

            - Middle Position -
            JointPointList[2][6], JointPointList[3][1]
            JointPointList[2][7], JointPointList[3][2]

             - Rigth Position -
            JointPointList[1][5], JointPointList[2][11]

            /**/



            // Horizontal Demention
            /*

            # 870
            JointPointList[0][0], JointPointList[0][1]

            # 10 : Cut at job Site
            JointPointList[1][0], JointPointList[1][1]

            # BACKING STRIP
            JointPointList[1][2].X, JointPointList[0][0].Y 

            # ANNULAR PLATE
            JointPointList[2][0].X + ??, JointPointList[2][0].Y 

            # 60
            JointPointList[2][0], JointPointList[2][1]

            # t16 SHELL I.D @
            JointPointList[2][1], JointPointList[2][2]

            # t709
            JointPointList[2][2], JointPointList[2][4]

            # 65(MIN.60)
            JointPointList[3][0], JointPointList[2][5]
            // or JointPointList[2][4], JointPointList[2][5]

            # BOTTOM PLATE
            JointPointList[3][0].X + ??, JointPointList[3][0].Y 

            # 26 (right)
            JointPointList[2][8], JointPointList[3][3]
            // or JointPointList[2][8], JointPointList[2][9]

            # ANNULAR PLATE O.D 6620
            JointPointList[2][0]


            /**/



            // Diagonal Demention
            /*

            # 8 (left)
                                  JointPointList[3][0]
            JointPointList[2][3], JointPointList[2][4]

            # 8 (right)
            JointPointList[3][3]
            JointPointList[2][9], JointPointList[2][10]

            /**/


            
            
            
            // Check Line : Red color
            //Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            //centerLineList.Add(centerline);
            //styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);


            styleService.SetLayerListLine(ref horizontalLineList, layerService.LayerOutLine);
            styleService.SetLayerListLine(ref verticalLineList, layerService.LayerOutLine);
            styleService.SetLayerListLine(ref diagonalLineList, layerService.LayerOutLine);


            drawList.outlineList.AddRange(horizontalLineList);
            drawList.outlineList.AddRange(verticalLineList);
            drawList.outlineList.AddRange(diagonalLineList);

            return drawList;
        }

        public void DrawBottomPlateJoint_LINE_Case2(Model singleModel)
        {

            // DETAIL "A"  CASE2

            singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Line> lineList = new List<Line>();

            List<Point3D> JointPointList = new List<Point3D>();


            Point3D referencePoint = new Point3D(0, 0);


            /////////////////////////////
            //      Information      ////
            /////////////////////////////



            // setting Lines1
            Line eachLine = null;
            Point3D startPoint = referencePoint;

            // Bottom Lines(Lectangle)
            startPoint = GetSumPoint(referencePoint, (80 + 50 - 15), 0); /* Point extrect /**/ JointPointList.Add(startPoint); // 0
            lineList.Add(eachLine = new Line(startPoint, GetSumPoint(startPoint, -50, 0)));
            lineList.Add(eachLine = new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 0, 5))); JointPointList.Add(eachLine.EndPoint); // 1
            lineList.Add(new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 50, 0))); JointPointList.Add(eachLine.EndPoint); // 2

            // Mid Lines(Lectangle)
            startPoint = GetSumPoint(referencePoint, 0, 5); JointPointList.Add(startPoint); // 3
            lineList.Add(eachLine = new Line(startPoint, GetSumPoint(startPoint, 80, 0))); JointPointList.Add(eachLine.EndPoint); // 4
            lineList.Add(eachLine = new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 0, 5))); JointPointList.Add(eachLine.EndPoint); // 5
            lineList.Add(new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 5, -5)));  /* diagonal Line /**/ JointPointList.Add(GetSumPoint(eachLine.EndPoint, 5, -5)); // 6
            lineList.Add(eachLine = new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, -80, 0))); JointPointList.Add(eachLine.EndPoint); // 7
            lineList.Add(new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 0, -5)));


            // Top Lines(Lectangle)
            startPoint = GetSumPoint(referencePoint, 30, (5 + 5 + 35)); JointPointList.Add(startPoint); // 8
            lineList.Add(eachLine = new Line(startPoint, GetSumPoint(startPoint, 0, -35))); JointPointList.Add(eachLine.EndPoint); // 9
            lineList.Add(eachLine = new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 5, 0))); JointPointList.Add(eachLine.EndPoint); // 10
            lineList.Add(eachLine = new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 0, 35))); JointPointList.Add(eachLine.EndPoint); // 11



            // Vertical Demention
            /*

             - LEFT Position -
            * t10
            JointPointList[3], JointPointList[7]

             - Rigth Position -
            # t10 (bottom)

            /**/



            // Spline Demention
            /*

            - Rigth Position -
            JointPointList[0], JointPointList[2]

            - Top Position -
            JointPointList[8], JointPointList[11]

            /**/



            // Horizontal Demention
            /*

            # BOTTOM PLATE O.D 6620
            JointPointList[3] -----------

            # 60
            JointPointList[7], JointPointList[9]

            # t8 SHELL I.D 6500
            JointPointList[9](y=[8]~[9]), JointPointList[10](y=[8]~[9])

            # 30(MIN.25)
            JointPointList[1], JointPointList[5]

            /**/



            // Diagonal Demention
            /*

            # 10 (left)
            JointPointList[5]
            JointPointList[4], JointPointList[6]

            /**/


            // Center Line : Red color
            Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            centerLineList.Add(centerline);

            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);

            singleModel.Entities.AddRange(centerLineList);
            singleModel.Entities.AddRange(lineList);

            singleModel.Entities.Regen();
            singleModel.Invalidate();

            singleModel.SetView(viewType.Top);
            singleModel.ZoomFit();
            singleModel.Refresh();
        }

        public void DrawBottomPlateJoint_LINE_Case3(Model singleModel)
        {

            // SECTION "B"-"B"


            singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Line> lineList = new List<Line>();

            List<Point3D> JointPointList = new List<Point3D>();


            Point3D referencePoint = new Point3D(0, 0);


            /////////////////////////////
            //      Information      ////
            /////////////////////////////



            // setting Lines1
            Line eachLine = null;
            Point3D startPoint = referencePoint;

            // Bottom Lines(Lectangle)
            startPoint = GetSumPoint(referencePoint, (50 + 55 - 20), 0); /* Point extrect /**/ JointPointList.Add(startPoint); // 0
            lineList.Add(eachLine = new Line(startPoint, GetSumPoint(startPoint, -55, 0)));
            lineList.Add(eachLine = new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 0, 5))); JointPointList.Add(eachLine.EndPoint); // 1
            lineList.Add(new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 55, 0))); JointPointList.Add(eachLine.EndPoint); // 2

            // Mid Lines(Lectangle)
            startPoint = GetSumPoint(referencePoint, 0, 5); JointPointList.Add(startPoint); // 3
            lineList.Add(eachLine = new Line(startPoint, GetSumPoint(startPoint, 50, 0))); JointPointList.Add(eachLine.EndPoint); // 4
            lineList.Add(eachLine = new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 0, 5))); JointPointList.Add(eachLine.EndPoint); // 5
            lineList.Add(new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 5, -5)));  /* diagonal Line /**/ JointPointList.Add(GetSumPoint(eachLine.EndPoint, 5, -5)); // 6
            lineList.Add(eachLine = new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, -50, 0))); JointPointList.Add(eachLine.EndPoint); // 7


            //// Bottom Lines(Lectangle)
            //Point3D tPoint01 = GetSumPoint(referencePoint, (50 + 55 - 20), 0);
            //Line tLine01 = new Line(GetSumPoint(startPoint,0,0), GetSumPoint(startPoint, -55, 0));
            //Line tLine02 = new Line(GetSumPoint(tLine01.EndPoint, 0, 0), GetSumPoint(tLine01.EndPoint, 0, 5));
            //Line tLine03 = new Line(GetSumPoint(tLine02.EndPoint, 0, 0), GetSumPoint(tLine02.EndPoint, 55, 0));

            //lineList.AddRange(new Line[] { tLine01, tLine02, tLine03 });


            //// Mid Lines(Lectangle)
            //startPoint = GetSumPoint(referencePoint, 0, 5); 
            //lineList.Add(eachLine = new Line(startPoint, GetSumPoint(startPoint, 50, 0)));
            //lineList.Add(eachLine = new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 0, 5))); 
            //lineList.Add(new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, 5, -5)));
            //lineList.Add(eachLine = new Line(eachLine.EndPoint, GetSumPoint(eachLine.EndPoint, -50, 0)));



            //JointPointList.Add(startPoint); // 0
            //JointPointList.Add(tLine01.StartPoint); // 1
            //JointPointList.Add(tLine01.EndPoint); // 1



            // Spline Demention
            /*

            - Left Position -
            JointPointList[3], JointPointList[7]

            - Rigth Position -
            JointPointList[0], JointPointList[2]

            /**/


            // Horizontal Demention
            /*

            # 26(MIN.)
            JointPointList[1], JointPointList[5]

            /**/


            // Diagonal Demention
            /*

            # 8 (mid)
            JointPointList[5]
            JointPointList[4], JointPointList[6]

            /**/


            // Center Line : Red color
            Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            centerLineList.Add(centerline);

            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);

            singleModel.Entities.AddRange(centerLineList);
            singleModel.Entities.AddRange(lineList);

            singleModel.Entities.Regen();
            singleModel.Invalidate();

            singleModel.SetView(viewType.Top);
            singleModel.ZoomFit();
            singleModel.Refresh();
        }

        public void DrawBottomPlateJoint_Line_Case4(Model singleModel)
        {

            // Detail "C"

            singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Entity> splineList = new List<Entity>();
            List<Line> lineList = new List<Line>();

            /////////////////////////////
            //      Information      ////
            /////////////////////////////
            Point3D referencePoint = new Point3D(0, 0);
            double distanceYofRef = 30;  // Start Point of TopView : RefPoint 에서의 Y축으로 떨어진거리

            Line righthLine = null;
            Line leftLine = null;


            lineList.Add(new Line(referencePoint, GetSumPoint(referencePoint, 105, 0)));  // 0
            lineList.Add(new Line(GetSumPoint(referencePoint, 0, 5), GetSumPoint(referencePoint, 105, 5)));  // 1
            lineList.Add(righthLine = new Line(GetSumPoint(referencePoint, 15 + 30, 10), GetSumPoint(referencePoint, 15 + 30 + 20 + 25, 10)));  // 2
            lineList.Add(new Line(GetSumPoint(referencePoint, 15 + 30, 10), GetSumPoint(referencePoint, 15 + 30, 5)));  // 3
            lineList.Add(new Line(GetSumPoint(referencePoint, 15 + 30, 10), GetSumPoint(referencePoint, 15 + 30 - 5, 5)));  // 4
            lineList.Add(new Line(GetSumPoint(referencePoint, 15 + 30 + 20, 15), GetSumPoint(referencePoint, 15 + 30 + 20, 10)));  // 5
            lineList.Add(new Line(GetSumPoint(referencePoint, 15 + 30 + 20, 15), GetSumPoint(referencePoint, 15 + 30 + 20 + 5, 10)));  // 6


            // offset End Point : intersect Check
            Point3D startPoint = GetSumPoint(referencePoint, 15 + 30, 10);
            Line offsetTestLine = new Line(GetSumPoint(startPoint, -50, 0), GetSumPoint(startPoint, 50, 0));
            offsetTestLine.Rotate((Utility.DegToRad(30)), Vector3D.AxisZ, startPoint);

            leftLine = new Line(GetSumPoint(referencePoint, 15, 5), GetSumPoint(referencePoint, 15 + 30, 5));

            // offset Reference Line
            Line offsetEachLine = GetInterSectLine_withTwoLines(leftLine, offsetTestLine, righthLine);
            lineList.Add(offsetEachLine); // 7

            lineList.Add(new Line(GetSumPoint(referencePoint, 15, 5), offsetEachLine.StartPoint));  // 8


            leftLine = new Line(GetSumPoint(referencePoint, 15, 10), GetSumPoint(referencePoint, 15 + 30, 10));  // 10(가상선)
            righthLine = new Line(GetSumPoint(referencePoint, 15 + 30 + 20, 15), GetSumPoint(referencePoint, 0, 15));  // 11(가상선)
            Line tempoffsetLine = (Line)offsetTestLine.Offset(-5, Vector3D.AxisZ); // offset 가상선

            // offset Line
            lineList.Add(offsetEachLine = GetInterSectLine_withTwoLines(leftLine, tempoffsetLine, righthLine)); // 9

            lineList.Add(new Line(GetSumPoint(referencePoint, 15, 10), offsetEachLine.StartPoint));  // 10
            lineList.Add(new Line(offsetEachLine.EndPoint, GetSumPoint(referencePoint, 15 + 30 + 20, 15)));  // 11

            lineList.Add(righthLine = new Line(GetSumPoint(referencePoint, 15 + 30, 5), GetSumPoint(referencePoint, 15 + 30 + 20 + 25, 5)));  // 12
            /**/



            //////////////////////////////
            // Draw : Top View 
            //////////////////////////////

            List<Entity> topViewLineList = new List<Entity>();
            List<Line> topViewHiddenLineList = new List<Line>();


            Point3D topViewStartPoint = GetSumPoint(referencePoint, 0, distanceYofRef);

            //* bottom Spline *//
            List<Point3D> bottomPointList = new List<Point3D>();

            List<Line> bottomLineList = GetRectangle(topViewStartPoint, 35, 105);
            Line bLineBottom = bottomLineList[0];
            Line bLineRight = bottomLineList[1];
            Line bLineTop = bottomLineList[2];
            Line bLineLeft = bottomLineList[3];

            Line bOutLineLeft = new Line(GetSumPoint(bLineLeft.StartPoint, -2, 0), GetSumPoint(bLineLeft.EndPoint, -2, 0));
            Line bOutLineRight = new Line(GetSumPoint(bLineRight.StartPoint, 2, 0), GetSumPoint(bLineRight.EndPoint, 2, 0));

            Line bOutLineBottom = new Line(GetSumPoint(bLineBottom.StartPoint, 0, -2), GetSumPoint(bLineBottom.EndPoint, 0, -2));
            Line bInLineBottom = new Line(GetSumPoint(bLineBottom.StartPoint, 2, 2), GetSumPoint(bLineBottom.EndPoint, -2, 2));

            /*
            lineList.AddRange(new Line[]{
                bLineBottom, bLineRight,bLineTop, bLineLeft,
                bOutLineLeft, bOutLineRight, bOutLineBottom, bInLineBottom
            });
            /**/

            // spline setting
            double distanceYPoint = (bOutLineLeft.StartPoint.Y - bOutLineLeft.MidPoint.Y) / 2;
            double distanceXPoint = (bOutLineBottom.MidPoint.X - bOutLineBottom.StartPoint.X) / 2;

            bottomPointList.Add(bLineLeft.StartPoint);
            bottomPointList.Add(GetSumPoint(bOutLineLeft.StartPoint, 0, -distanceYPoint));
            bottomPointList.Add(bLineLeft.MidPoint);
            bottomPointList.Add(GetSumPoint(bOutLineLeft.EndPoint, 0, distanceYPoint));
            bottomPointList.Add(bInLineBottom.StartPoint);
            bottomPointList.Add(GetSumPoint(bOutLineBottom.StartPoint, distanceXPoint, 0));
            bottomPointList.Add(bLineBottom.MidPoint);
            bottomPointList.Add(GetSumPoint(bOutLineBottom.EndPoint, -distanceXPoint, 0));
            bottomPointList.Add(bInLineBottom.EndPoint);
            bottomPointList.Add(GetSumPoint(bOutLineRight.EndPoint, 0, +distanceYPoint));
            bottomPointList.Add(bLineRight.MidPoint);
            bottomPointList.Add(GetSumPoint(bOutLineRight.StartPoint, 0, -distanceYPoint));
            bottomPointList.Add(bLineRight.StartPoint);

            Curve bottomSpline = Curve.CubicSplineInterpolation(bottomPointList);
            splineList.Add(bottomSpline);



            //* Left Spline *//
            List<Point3D> leftPointList = new List<Point3D>();

            Point3D leftStartPoint = GetSumPoint(bLineTop.StartPoint, 15, -20);
            List<Line> leftLineList = GetRectangle(leftStartPoint, 45, 50);
            Line leftLineBottom = leftLineList[0];
            Line leftLineRight = leftLineList[1];
            Line leftLineTop = leftLineList[2];
            Line leftLinetLeft = leftLineList[3];

            Point3D leftInLinePoint = GetSumPoint(leftLineTop.StartPoint, 2, -2);
            Point3D[] intersectMidPoint = bLineTop.IntersectWith(leftLineRight);

            // reset Right/bottom line
            leftLineRight.EndPoint = intersectMidPoint[0];
            leftLineBottom.EndPoint = GetSumPoint(leftLineBottom.EndPoint, -20, 0);

            // diagonal Line
            Line diagonalLeftLine = new Line(leftLineRight.EndPoint, leftLineBottom.EndPoint);

            topViewLineList.AddRange(new Line[]{
                                                leftLineBottom, leftLineRight,
                                                diagonalLeftLine
                                                });
            /**/

            // spline setting
            distanceYPoint = distanceXPoint = 0;
            distanceYPoint = (leftLinetLeft.StartPoint.Y - leftLinetLeft.EndPoint.Y) / 4;
            distanceXPoint = (leftLineTop.EndPoint.X - leftLineTop.StartPoint.X) / 4;


            leftPointList.Add(leftLineTop.EndPoint);
            leftPointList.Add(GetSumPoint(leftLineTop.EndPoint, -distanceXPoint, 2));
            leftPointList.Add(leftLineTop.MidPoint);
            leftPointList.Add(GetSumPoint(leftLineTop.StartPoint, distanceXPoint, 2));
            leftPointList.Add(leftInLinePoint);  // left top
            leftPointList.Add(GetSumPoint(leftLinetLeft.StartPoint, -2, -distanceYPoint));
            leftPointList.Add(leftLinetLeft.MidPoint);  // left mid
            leftPointList.Add(GetSumPoint(leftLinetLeft.EndPoint, -2, distanceYPoint));
            leftPointList.Add(leftLinetLeft.EndPoint);  // left bottom

            Curve leftSpline = Curve.CubicSplineInterpolation(leftPointList);
            splineList.Add(leftSpline);
            /**/


            Point3D[] intersectLeftPoint = bLineTop.IntersectWith(leftSpline); // Intersection : top line of bottomPlate & leftSpline of leftPlate
            topViewLineList.Add(new Line(bLineTop.StartPoint, intersectLeftPoint[0])); // Draw : topLeft Line of bottom plate



            //* Right Spline *//
            List<Point3D> rightPointList = new List<Point3D>();

            Point3D rightStartPoint = GetSumPoint(leftLineBottom.EndPoint, 0, 0);
            List<Line> rightLineList = GetRectangle(rightStartPoint, 55, 45);
            Line rightLineBottom = rightLineList[0];
            Line rightLineRight = rightLineList[1];
            Line rightLineTop = rightLineList[2];
            Line rightLinetLeft = rightLineList[3];

            Point3D rightInLinePoint = GetSumPoint(rightLineTop.EndPoint, -2, -2);
            Point3D[] intersectTopPoint = rightLinetLeft.IntersectWith(leftSpline); // left Line of rightPlate & top Spline of leftPlate

            // reset left line
            rightLinetLeft.EndPoint = intersectTopPoint[0];

            // Draw rectangle line (Left/bottom)
            topViewLineList.AddRange(new Line[]{
                                                rightLinetLeft,rightLineBottom
                                                });
            /**/

            // spline setting
            distanceYPoint = distanceXPoint = 0;
            distanceYPoint = (rightLineRight.StartPoint.Y - rightLineRight.EndPoint.Y) / 4;
            distanceXPoint = (rightLineTop.EndPoint.X - rightLineTop.StartPoint.X) / 4;

            rightPointList.Add(rightLineTop.StartPoint);
            rightPointList.Add(GetSumPoint(rightLineTop.StartPoint, distanceXPoint, 2));
            rightPointList.Add(rightLineTop.MidPoint);
            rightPointList.Add(GetSumPoint(rightLineTop.EndPoint, -distanceXPoint, 2));
            rightPointList.Add(rightInLinePoint);  // right top
            rightPointList.Add(GetSumPoint(rightLineRight.StartPoint, 2, -distanceYPoint));
            rightPointList.Add(rightLineRight.MidPoint);  // right mid
            rightPointList.Add(GetSumPoint(rightLineRight.EndPoint, 2, distanceYPoint));
            rightPointList.Add(GetSumPoint(rightLineRight.EndPoint, 0, 0));  // right bottom

            Curve rightSpline = Curve.CubicSplineInterpolation(rightPointList);
            splineList.Add(rightSpline);
            /**/


            Point3D[] intersectRightPoint = bLineTop.IntersectWith(rightSpline); // Intersection : top line of bottom Plate & rightSpline of rightPlate
            topViewLineList.Add(new Line(intersectRightPoint[0], bLineTop.EndPoint)); // Draw : topRight Line of bottom plate


            // Draw : hidden Line
            topViewHiddenLineList.AddRange(new Line[] {
                                                        new Line(intersectLeftPoint[0],intersectMidPoint[0]),
                                                        new Line(intersectMidPoint[0],intersectRightPoint[0]),
                                                        new Line(intersectTopPoint[0],leftLineBottom.EndPoint)
                                                        });


            // Spline Demention
            /*
            - Left Position -
            lineList[0].StartPoint, lineList[1].StartPoint
            lineList[8].StartPoint, lineList[10].StartPoint

            - Rigth Position -
            lineList[0].EndPoint, lineList[1].EndPoint
            lineList[12].EndPoint, lineList[2].EndPoint

            /**/


            // Horizontal Demention
            /*
            - Top Position -
            // 26 (MIN.)
            lineList[2].StartPoint, lineList[11].EndPoint
            /**/


            // Vertical Demention
            /*
            - Left Position -
            lineList[1].StartPoint, lineList[10].StartPoint


            - Rigth Position -

            // bottom t8
            lineList[0].EndPoint, lineList[1].EndPoint
            // top t8
            lineList[1].EndPoint, lineList[2].EndPoint

            /**/


            // Diagonal Demention

            /*
            # (Left) triangle
                                  lineList[3].StartPoint
            lineList[4].EndPoint, lineList[3].EndPoint

            # (Right) triangle
            lineList[5].StartPoint
            lineList[5].EndPoint, lineList[6].EndPoint

            // HAMMERING
            lineList[9].MidPoint

            /**/

            // Center Line : Red color
            Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            centerLineList.Add(centerline);

            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            styleService.SetLayerListEntity(ref splineList, layerService.LayerDimension);
            styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);
            styleService.SetLayerListLine(ref topViewLineList, layerService.LayerOutLine);
            styleService.SetLayerListLine(ref topViewHiddenLineList, layerService.LayerHiddenLine);

            singleModel.Entities.AddRange(centerLineList);
            singleModel.Entities.AddRange(lineList);
            singleModel.Entities.AddRange(topViewLineList);
            singleModel.Entities.AddRange(topViewHiddenLineList);
            singleModel.Entities.Add(bottomSpline);
            singleModel.Entities.Add(leftSpline);
            singleModel.Entities.Add(rightSpline);



            singleModel.Entities.Regen();
            singleModel.Invalidate();

            singleModel.SetView(viewType.Top);
            singleModel.ZoomFit();
            singleModel.Refresh();
        }

        public void DrawBottomPlateJoint_Detail_D_Case5(Model singleModel)
        {

            // // Detail "D"

            singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Entity> splineList = new List<Entity>();
            List<Line> lineList = new List<Line>();

            Point3D referencePoint = new Point3D(0, 0);
            double distanceYofRef = 30;  // Start Point of TopView : RefPoint 에서의 Y축으로 떨어진거리


            // bottom Plate
            Point3D bottomStartPoint = GetSumPoint(referencePoint, 60 - 20, 0);
            List<Line> bottomPLineList = GetRectangle(bottomStartPoint, 5, 65, RECTANGLUNVIEW.RIGHT);
            Line bPlateBottom = bottomPLineList[0];
            Line bPlateTop = bottomPLineList[1];
            Line bPlateLeft = bottomPLineList[2];

            // Mid Plate
            Point3D midStartPoint = GetSumPoint(referencePoint, 0, 5);
            List<Line> midLineList = GetRectangle(midStartPoint, 5, 60, RECTANGLUNVIEW.LEFT);
            Line mPlateBottom = midLineList[0];
            Line mPlateRight = midLineList[1];
            Line mPlateTop = midLineList[2];

            // Diagonal Line (triangle)
            Point3D diagonalStartPoint = GetSumPoint(mPlateBottom.EndPoint, 5, 0);
            Line diagonalLine = new Line(diagonalStartPoint, mPlateTop.EndPoint);
            // bottom LeftPoint : mPlateBottom.EndPoint



            // Top Plate

            // diagonal of topPlate
            Point3D rotatePoint = mPlateTop.EndPoint;
            Line tPlateIntersectLine1 = new Line(GetSumPoint(referencePoint, 0, 10), GetSumPoint(referencePoint, 100, 10));
            Line tPlateIntersectLine2 = (Line)tPlateIntersectLine1.Offset(-5, Vector3D.AxisZ);
            tPlateIntersectLine1.Rotate(Utility.DegToRad(-30), Vector3D.AxisZ, rotatePoint);
            tPlateIntersectLine2.Rotate(Utility.DegToRad(-30), Vector3D.AxisZ, rotatePoint);

            List<Line> tPlateLeftLine = new List<Line>();
            List<Line> tPlateRightLine = new List<Line>();
            Point3D tPlateLeftStartPoint = GetSumPoint(referencePoint, 15, 10);
            tPlateLeftLine = GetRectangle(tPlateLeftStartPoint, 5, 100);  // EndPoint = 100 임의로 연장한 값 (Intersect 하기 위함)
                                                                          // top Plate Left Lines
            Line tPlateLeftBottom = tPlateLeftLine[0];
            Line tPlateLeftTop = tPlateLeftLine[2];

            Point3D tPlateRightStartPoint = GetSumPoint(referencePoint, 0, 5);// StartPoint = 0 임의로 연장한 값 (Intersect 하기 위함)
            tPlateRightLine = GetRectangle(tPlateRightStartPoint, 5, 60 + 30);
            // top Plate Right Lines
            Line tPlateRightBottom = tPlateRightLine[0];
            Line tPlateRightTop = tPlateRightLine[2];

            // Intersect Lines
            Line tPlateDiagonalLine1 = GetInterSectLine_withTwoLines(tPlateLeftBottom, tPlateIntersectLine1, tPlateRightBottom);
            Line tPlateDiagonalLine2 = GetInterSectLine_withTwoLines(tPlateLeftTop, tPlateIntersectLine2, tPlateRightTop);

            // reset Top plate Line Point
            tPlateLeftBottom.EndPoint = tPlateDiagonalLine1.StartPoint;
            tPlateLeftTop.EndPoint = tPlateDiagonalLine2.StartPoint;

            tPlateRightBottom.StartPoint = tPlateDiagonalLine1.EndPoint;
            tPlateRightTop.StartPoint = tPlateDiagonalLine2.EndPoint;




            lineList.AddRange(new Line[]{
                    bPlateBottom, bPlateTop, bPlateLeft,
                    mPlateBottom, mPlateRight, mPlateTop,
                    tPlateLeftBottom, tPlateLeftTop, tPlateRightBottom, tPlateRightTop,
                    tPlateDiagonalLine1, tPlateDiagonalLine2,
                    diagonalLine,
                    });



            //////////////////////////////
            // Draw : Top View 
            //////////////////////////////

            List<Line> topViewLineList = new List<Line>();
            List<Line> topViewHiddenLineList = new List<Line>();


            Point3D topViewStartPoint = GetSumPoint(referencePoint, 0, distanceYofRef);

            //* top Spline */

            List<Point3D> topPointList = new List<Point3D>();

            Point3D topStartPoint = GetSumPoint(topViewStartPoint, 15, 30);
            List<Line> topLineList = GetRectangle(topStartPoint, 35, 75);
            Line tLineBottom = topLineList[0];
            Line tLineRight = topLineList[1];
            Line tLineTop = topLineList[2];
            Line tLineLeft = topLineList[3];

            lineList.AddRange(new Line[]{
                tLineBottom,
            });

            Point3D tPlateLeftInPoint = GetSumPoint(tLineLeft.StartPoint, 2, -2);
            Point3D tPlateRightInPoint = GetSumPoint(tLineRight.StartPoint, -2, -2);

            // spline point setting
            double distanceYPoint = (tLineLeft.StartPoint.Y - tLineLeft.EndPoint.Y) / 4;
            double distanceXPoint = (tLineBottom.EndPoint.X - tLineBottom.StartPoint.X) / 4;

            topPointList.Add(tLineBottom.StartPoint);
            topPointList.Add(GetSumPoint(tLineBottom.StartPoint, -2, distanceYPoint));
            topPointList.Add(tLineLeft.MidPoint); // left mid
            topPointList.Add(GetSumPoint(tLineLeft.StartPoint, -2, -distanceYPoint));
            topPointList.Add(tPlateLeftInPoint); // top left
            topPointList.Add(GetSumPoint(tLineTop.StartPoint, distanceXPoint, 2));
            topPointList.Add(tLineTop.MidPoint); // top mid
            topPointList.Add(GetSumPoint(tLineTop.EndPoint, -distanceXPoint, 2));
            topPointList.Add(tPlateRightInPoint); // top right
            topPointList.Add(GetSumPoint(tLineRight.StartPoint, 2, -distanceYPoint));
            topPointList.Add(tLineRight.MidPoint);
            topPointList.Add(GetSumPoint(tLineRight.EndPoint, 2, distanceYPoint));
            topPointList.Add(tLineBottom.EndPoint);

            Curve topSpline = Curve.CubicSplineInterpolation(topPointList);
            splineList.Add(topSpline);



            //@ Left Spline @/

            List<Point3D> leftPointList = new List<Point3D>();

            Point3D leftStartPoint = GetSumPoint(topViewStartPoint, 0, 50 - 40);
            List<Line> leftLineList = GetRectangle(leftStartPoint, 40, 60);
            Line leftLineBottom = leftLineList[0];
            Line leftLineRight = leftLineList[1];
            Line leftLineTop = leftLineList[2];
            Line leftLinetLeft = leftLineList[3];

            Point3D hiddenRightSP = leftLineRight.StartPoint; // hidden Right start Point

            Point3D lPlateLeftInPoint = GetSumPoint(leftLinetLeft.EndPoint, 2, 2);

            // spline Point setting
            distanceYPoint = distanceXPoint = 0;
            distanceYPoint = (leftLinetLeft.StartPoint.Y - leftLinetLeft.EndPoint.Y) / 4;
            distanceXPoint = (leftLineBottom.EndPoint.X - leftLineBottom.StartPoint.X) / 4;

            leftPointList.Add(leftLinetLeft.StartPoint);
            leftPointList.Add(GetSumPoint(leftLinetLeft.StartPoint, -2, -distanceYPoint));
            leftPointList.Add(leftLinetLeft.MidPoint); // left mid
            leftPointList.Add(GetSumPoint(leftLinetLeft.EndPoint, -2, distanceYPoint));
            leftPointList.Add(lPlateLeftInPoint);  // bottom start
            leftPointList.Add(GetSumPoint(leftLineBottom.StartPoint, distanceXPoint, -2));
            leftPointList.Add(leftLineBottom.MidPoint);  // bottom mid
            leftPointList.Add(GetSumPoint(leftLineBottom.EndPoint, -distanceXPoint, -2));
            leftPointList.Add(leftLineBottom.EndPoint);  // bottom end

            Curve leftSpline = Curve.CubicSplineInterpolation(leftPointList);
            splineList.Add(leftSpline);
            /**/

            // intersect Point
            Point3D[] intersectTopLeftPoint = leftLineTop.IntersectWith(topSpline);
            Point3D[] intersectMidPoint = leftLineRight.IntersectWith(tLineBottom);

            // Reset Point : top(left) / right line
            leftLineTop.EndPoint = intersectTopLeftPoint[0];
            leftLineRight.StartPoint = intersectMidPoint[0];

            // Draw Line
            topViewLineList.AddRange(new Line[] {
                                            leftLineTop, leftLineRight,
            });




            //@ Right Spline @/
            List<Point3D> rightPointList = new List<Point3D>();

            Point3D rightStartPoint = GetSumPoint(topViewStartPoint, 60 - 20, 0);
            List<Line> rightLineList = GetRectangle(rightStartPoint, 50, 65);
            Line rightLineBottom = rightLineList[0];
            Line rightLineRight = rightLineList[1];
            Line rightLineTop = rightLineList[2];
            Line rightLinetLeft = rightLineList[3];

            Point3D hiddenLeftSP = rightLinetLeft.StartPoint; // hidden Left start Point

            Point3D rightInLinePoint = GetSumPoint(rightLineBottom.EndPoint, -2, 2);


            // spline setting
            distanceYPoint = distanceXPoint = 0;
            distanceYPoint = (rightLinetLeft.StartPoint.Y - rightLinetLeft.EndPoint.Y) / 4;
            distanceXPoint = (rightLineBottom.EndPoint.X - rightLineBottom.StartPoint.X) / 4;

            rightPointList.Add(rightLineBottom.StartPoint);
            rightPointList.Add(GetSumPoint(rightLineBottom.StartPoint, distanceXPoint, -2));
            rightPointList.Add(rightLineBottom.MidPoint); // bottom mid
            rightPointList.Add(GetSumPoint(rightLineBottom.EndPoint, -distanceXPoint, -2));
            rightPointList.Add(rightInLinePoint);  // bottom end
            rightPointList.Add(GetSumPoint(rightLineRight.EndPoint, 2, distanceYPoint));
            rightPointList.Add(rightLineRight.MidPoint);  // right mid
            rightPointList.Add(GetSumPoint(rightLineRight.StartPoint, 2, -distanceYPoint));
            rightPointList.Add(rightLineRight.StartPoint);  // top right

            Curve rightSpline = Curve.CubicSplineInterpolation(rightPointList);
            splineList.Add(rightSpline);
            /**/

            Point3D[] intersectBottomPoint = rightLinetLeft.IntersectWith(leftSpline);
            Point3D[] intersectTopRightPoint = rightLineTop.IntersectWith(topSpline);

            // reset left line
            rightLinetLeft.StartPoint = intersectBottomPoint[0];
            rightLineTop.StartPoint = intersectTopRightPoint[0];

            // Draw rectangle line (Left/bottom)
            topViewLineList.AddRange(new Line[]{
                                                rightLinetLeft, rightLineTop
                                                });



            //@ Draw : hidden Line @
            topViewHiddenLineList.AddRange(new Line[] {
                                                        new Line(intersectTopLeftPoint[0],intersectTopRightPoint[0]),  // top
                                                        new Line(hiddenLeftSP,intersectBottomPoint[0]),  // left
                                                        new Line(hiddenRightSP,intersectMidPoint[0])  // right
                                                        });


            // Center Line : Red color
            Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            centerLineList.Add(centerline);

            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            styleService.SetLayerListEntity(ref splineList, layerService.LayerDimension);
            styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);
            styleService.SetLayerListLine(ref topViewLineList, layerService.LayerOutLine);
            styleService.SetLayerListLine(ref topViewHiddenLineList, layerService.LayerHiddenLine);

            singleModel.Entities.AddRange(centerLineList);
            singleModel.Entities.AddRange(lineList);
            singleModel.Entities.AddRange(topViewLineList);
            singleModel.Entities.AddRange(topViewHiddenLineList);
            singleModel.Entities.Add(topSpline);
            singleModel.Entities.Add(leftSpline);
            singleModel.Entities.Add(rightSpline);

            singleModel.Entities.Regen();
            singleModel.Invalidate();

            singleModel.SetView(viewType.Top);
            singleModel.ZoomFit();
            singleModel.Refresh();
        }

        public void DrawTopAngleJointDetail_View_B(Model singleModel)
        {

            // TopAngleJointDetail View "B"

            singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Line> lineList = new List<Line>();

            List<Point3D> JointPointList = new List<Point3D>();


            Point3D referencePoint = new Point3D(0, 0);


            /////////////////////////////
            //      Information      ////
            /////////////////////////////

            Line intersectLeftLine = null;
            Line intersectRighthLine = null;
            double distance = 3.2 / 2;

            Line hLeftBottom = new Line(referencePoint, GetSumPoint(referencePoint, 34, 0));
            Line vLeftEnd = new Line(hLeftBottom.EndPoint, GetSumPoint(hLeftBottom.EndPoint, 0, 38));
            Line hLeftTop = new Line(vLeftEnd.EndPoint, GetSumPoint(vLeftEnd.EndPoint, -34, 0));

            // Diagonal Line calculate
            Point3D diagonalStartPoint = GetSumPoint(hLeftTop.EndPoint, 0, -1.6);
            Line tempLine = new Line(GetSumPoint(hLeftTop.StartPoint, 0, -6), GetSumPoint(hLeftTop.EndPoint, 0, -6));
            Line intersectLine = new Line(diagonalStartPoint, GetSumPoint(diagonalStartPoint, -30, 0));
            intersectLine.Rotate(Utility.DegToRad(60), Vector3D.AxisZ, diagonalStartPoint);

            // Get intersect End Point
            Point3D[] diagonalEndPoint = intersectLine.IntersectWith(tempLine);


            Line diagonalLeft = new Line(diagonalStartPoint, diagonalEndPoint[0]);
            Line hLeftMid = new Line(GetSumPoint(hLeftTop.StartPoint, 0, -6), diagonalEndPoint[0]);
            double diagonalXDis = Math.Abs(diagonalLeft.StartPoint.X - diagonalLeft.EndPoint.X); // diagonal x - x distance

            Line vLeftMid = new Line(diagonalEndPoint[0], GetSumPoint(hLeftBottom.EndPoint, -diagonalXDis, 0));

            // Draw : Mirror
            Line hRightBottom = MirrorLine(hLeftBottom, Vector2D.AxisX, distance);
            Line vRightEnd = MirrorLine(vLeftEnd, Vector2D.AxisX, distance);
            Line hRightTop = MirrorLine(hLeftTop, Vector2D.AxisX, distance);
            Line diagonalRight = MirrorLine(diagonalLeft, Vector2D.AxisX, distance);
            Line hRightMid = MirrorLine(hLeftMid, Vector2D.AxisX, distance + diagonalXDis);
            Line vRightMid = MirrorLine(vLeftMid, Vector2D.AxisX, distance + diagonalXDis);

            lineList.AddRange(new Line[]{
                    hLeftBottom, vLeftEnd, hLeftTop, diagonalLeft, hLeftMid, vLeftMid,
                    hRightBottom, vRightEnd, hRightTop, diagonalRight, hRightMid, vRightMid
                    });






            /*
            // Horizontal Demention
            /
            - Top Position -
            // 3.2
            hLeftTop.StartPoint, hRightTop.StartPoint

            // 60도 깃발
            hRightTop.StartPoint

            // 아래 60도
            diagonalEndPoint[0] // 좌측

            /**/





            // Center Line : Red color
            Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            centerLineList.Add(centerline);

            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);

            singleModel.Entities.AddRange(centerLineList);
            singleModel.Entities.AddRange(lineList);

            singleModel.Entities.Regen();
            singleModel.Invalidate();

            singleModel.SetView(viewType.Top);
            singleModel.ZoomFit();
            singleModel.Refresh();
        }

        public void DrawTopAngleJointDetail_View_C(Model singleModel)
        {

            // Wind Girder Joint Detail View "C"

            singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Line> lineList = new List<Line>();

            Point3D referencePoint = new Point3D(0, 0);


            /////////////////////////////
            //      Information      ////
            /////////////////////////////
            double distance = 3.2 / 2;


            Line hLeftBottom = new Line(referencePoint, GetSumPoint(referencePoint, 34, 0));
            Line vLeftEnd = new Line(hLeftBottom.EndPoint, GetSumPoint(hLeftBottom.EndPoint, 0, (38 - 6 + 1.6)));

            //// Diagonal Line calculate
            // /* <-------------------------------------------
            Point3D diagonalStartPoint = vLeftEnd.EndPoint;
            Line tempLine = new Line(GetSumPoint(hLeftBottom.StartPoint, 0, 38), GetSumPoint(hLeftBottom.EndPoint, 0, 38)); // 4번 가상선
            Line intersectLine = new Line(diagonalStartPoint, GetSumPoint(diagonalStartPoint, 0, 15));  // 15 = 임의값
            intersectLine.Rotate(Utility.DegToRad(30), Vector3D.AxisZ, diagonalStartPoint);

            // Get intersect End Point
            Point3D[] diagonalEndPoint = intersectLine.IntersectWith(tempLine);
            Line diagonalLeft = new Line(diagonalStartPoint, diagonalEndPoint[0]);

            // diagonal x - x distance
            double diagonalXDis = Math.Abs(diagonalLeft.StartPoint.X - diagonalLeft.EndPoint.X);
            //-----------------------------------------------> */

            Line hLeftTop = new Line(GetSumPoint(hLeftBottom.StartPoint, 0, 38), diagonalEndPoint[0]);
            Line vLeftMid = new Line(diagonalEndPoint[0], GetSumPoint(hLeftBottom.EndPoint, -diagonalXDis, 0));
            Line hLeftDottedLine = new Line(GetSumPoint(hLeftBottom.StartPoint, 0, 38 - 6), GetSumPoint(hLeftBottom.EndPoint, 0, 38 - 6));


            // Mirror
            Line hRightBottom = MirrorLine(hLeftBottom, Vector2D.AxisX, distance);
            Line vRightEnd = MirrorLine(vLeftEnd, Vector2D.AxisX, distance);
            Line diagonalRight = MirrorLine(diagonalLeft, Vector2D.AxisX, distance);
            Line hRightTop = MirrorLine(hLeftTop, Vector2D.AxisX, distance + diagonalXDis);
            Line vRightMid = MirrorLine(vLeftMid, Vector2D.AxisX, distance + diagonalXDis);
            Line hRightDottedLine = MirrorLine(hLeftDottedLine, Vector2D.AxisX, distance);

            /**/
            lineList.AddRange(new Line[]{
                    hLeftBottom, vLeftEnd, hLeftTop, diagonalLeft, hLeftDottedLine, vLeftMid,
                    hRightBottom, vRightEnd, hRightTop, diagonalRight, hRightDottedLine, vRightMid
                    });




            // Center Line : Red color
            Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            centerLineList.Add(centerline);

            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);

            singleModel.Entities.AddRange(centerLineList);
            singleModel.Entities.AddRange(lineList);

            singleModel.Entities.Regen();
            singleModel.Invalidate();

            singleModel.SetView(viewType.Top);
            singleModel.ZoomFit();
            singleModel.Refresh();
        }

        public void DrawTopAngle_JointDetail_Detail_A1(Model singleModel)
        {

            // Top Angle Joint Detail : Detail "A"  1

            singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Line> lineList = new List<Line>();
            List<Entity> virtualLineList = new List<Entity>();

            double angleR1 = 3.5;
            double angleR2 = angleR1 * 2;
            Point3D referencePoint = new Point3D(0, 0);

            // bottom Plate
            Point3D bottomStartPoint = GetSumPoint(referencePoint, 45, 0);
            List<Line> bottomLineList = GetRectangle(bottomStartPoint, 50, 4, RECTANGLUNVIEW.BOTTOM);
            Line centerRight = bottomLineList[0];
            Line centerTop = bottomLineList[1];
            Line centerLeft = bottomLineList[2];

            // angle
            Line angleTop = new Line(GetSumPoint(referencePoint, 0, 50 + 10), GetSumPoint(referencePoint, 45, 50 + 10));
            Line angleRight = new Line(GetSumPoint(angleTop.EndPoint, 0, 0), GetSumPoint(angleTop.EndPoint, 0, -45));
            Line angleBottom = new Line(GetSumPoint(angleRight.EndPoint, 0, 0), GetSumPoint(angleRight.EndPoint, -4, 0));
            Line angleLeft = new Line(GetSumPoint(angleBottom.EndPoint, 0, 0), GetSumPoint(angleBottom.EndPoint, 0, 45 - 4));
            Line angleTop_low = new Line(GetSumPoint(angleLeft.EndPoint, 0, 0), GetSumPoint(angleLeft.EndPoint, -(45 - 4), 0));
            Line angleTop_left = new Line(GetSumPoint(angleTop_low.EndPoint, 0, 0), GetSumPoint(angleTop.StartPoint, 0, 0));

            Arc arcFillet01;
            if (Curve.Fillet(angleBottom, angleLeft, angleR1, false, false, true, true, out arcFillet01))
                virtualLineList.Add(arcFillet01);
            Arc arcFillet02;
            if (Curve.Fillet(angleTop_low, angleLeft, angleR2, true, true, true, true, out arcFillet02))
                virtualLineList.Add(arcFillet02);
            Arc arcFillet03;
            if (Curve.Fillet(angleTop_low, angleTop_left, angleR1, false, false, true, true, out arcFillet03))
                virtualLineList.Add(arcFillet03);

            // roof Plate
            Point3D roofStartPoint = GetSumPoint(angleTop.EndPoint, -11, 0);
            List<Line> roofPlateLineList = GetRectangle(roofStartPoint, 6, 60, RECTANGLUNVIEW.RIGHT);
            Line roofPlate_bottom = roofPlateLineList[0];
            Line roofPlate_top = roofPlateLineList[1];
            Line roofPlate_left = roofPlateLineList[2];

            // Rotation : Roof Plate
            roofPlate_bottom.Rotate(Utility.DegToRad(9.5), Vector3D.AxisZ, roofStartPoint);
            roofPlate_top.Rotate(Utility.DegToRad(9.5), Vector3D.AxisZ, roofStartPoint);
            roofPlate_left.Rotate(Utility.DegToRad(9.5), Vector3D.AxisZ, roofStartPoint);

            // Diagonal Line
            Point3D diagonalStartPoint = GetSumPoint(roofStartPoint, -6, 0);  // 6은 임의값(Plate width=6)  5인지 확인 필요 
            Line diagonalLine = new Line(diagonalStartPoint, roofPlate_top.StartPoint);
            Line diagonalBottom = new Line(diagonalStartPoint, roofPlate_bottom.StartPoint);


            virtualLineList.AddRange(new Entity[]{
                    centerRight, centerTop, centerLeft,
                    angleTop, angleRight, angleBottom, angleLeft, angleTop_low, angleTop_left,
                    });

            lineList.AddRange(new Line[]{
                    roofPlate_bottom, roofPlate_top, roofPlate_left,
                    diagonalLine, diagonalBottom
                    });

            // Center Line : Red color
            Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            centerLineList.Add(centerline);

            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref virtualLineList, layerService.LayerVirtualLine);

            singleModel.Entities.AddRange(centerLineList);
            singleModel.Entities.AddRange(lineList);
            singleModel.Entities.AddRange(virtualLineList);

            singleModel.Entities.Regen();
            singleModel.Invalidate();

            singleModel.SetView(viewType.Top);
            singleModel.ZoomFit();
            singleModel.Refresh();
        }

        public void DrawTopAngle_JointDetail_Detail_A2(Model singleModel)
        {

            // Top Angle Joint Detail : Detail "A"  2

            singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Line> lineList = new List<Line>();
            List<Entity> virtualLineList = new List<Entity>();


            Point3D referencePoint = new Point3D(0, 0);

            // bottom Plate
            List<Line> bottomLineList = GetRectangle(referencePoint, 15, 4, RECTANGLUNVIEW.BOTTOM);
            Line centerRight = bottomLineList[0];
            Line centerTop = bottomLineList[1];
            Line centerLeft = bottomLineList[2];

            // angle
            double angleWidth = 45;
            double angleHeight = 45;
            double angleThk = 4;
            double angleR1 = 3.5;
            double angleR2 = angleR1 * 2;

            Point3D angleStartPoint = GetSumPoint(referencePoint, 0, 15 + angleWidth);
            Line angleTop = new Line(angleStartPoint, GetSumPoint(angleStartPoint, angleWidth, 0));
            Line angleLeft = new Line(angleStartPoint, GetSumPoint(angleStartPoint, 0, -angleHeight));
            Line angleBottom = new Line(GetSumPoint(angleLeft.EndPoint, 0, 0), GetSumPoint(angleLeft.EndPoint, angleThk, 0));
            Line angleRight = new Line(GetSumPoint(angleBottom.EndPoint, 0, 0), GetSumPoint(angleBottom.EndPoint, 0, (angleHeight - angleThk)));
            Line angleTop_low = new Line(GetSumPoint(angleRight.EndPoint, 0, 0), GetSumPoint(angleRight.EndPoint, (angleHeight - angleThk), 0));
            Line angleTop_right = new Line(GetSumPoint(angleTop_low.EndPoint, 0, 0), GetSumPoint(angleTop.EndPoint, 0, 0));

            Arc arcFillet01;
            if (Curve.Fillet(angleBottom, angleRight, angleR1, false, false, true, true, out arcFillet01))
                virtualLineList.Add(arcFillet01);
            Arc arcFillet02;
            if (Curve.Fillet(angleRight, angleTop_low, angleR2, false, false, true, true, out arcFillet02))
                virtualLineList.Add(arcFillet02);
            Arc arcFillet03;
            if (Curve.Fillet(angleTop_low, angleTop_right, angleR1, false, false, true, true, out arcFillet03))
                virtualLineList.Add(arcFillet03);


            // roof Plate
            Point3D roofStartPoint = GetSumPoint(referencePoint, 15, 15 + 45);
            List<Line> roofPlateLineList = GetRectangle(roofStartPoint, 6, 60, RECTANGLUNVIEW.RIGHT);
            Line roofPlate_bottom = roofPlateLineList[0];
            Line roofPlate_top = roofPlateLineList[1];
            Line roofPlate_left = roofPlateLineList[2];

            // Rotation : Roof Plate
            roofPlate_bottom.Rotate(Utility.DegToRad(9.5), Vector3D.AxisZ, roofStartPoint);
            roofPlate_top.Rotate(Utility.DegToRad(9.5), Vector3D.AxisZ, roofStartPoint);
            roofPlate_left.Rotate(Utility.DegToRad(9.5), Vector3D.AxisZ, roofStartPoint);

            // Diagonal Line
            Point3D diagonalStartPoint = GetSumPoint(roofStartPoint, -6, 0);
            Line diagonalLine = new Line(diagonalStartPoint, roofPlate_top.StartPoint);
            Line diagonalBottom = new Line(diagonalStartPoint, roofPlate_bottom.StartPoint);

            virtualLineList.AddRange(new Entity[]{
                    centerRight, centerTop, centerLeft,
                    angleTop, angleRight, angleBottom, angleLeft, angleTop_low, angleTop_right,
                    });

            lineList.AddRange(new Line[]{
                    roofPlate_bottom, roofPlate_top, roofPlate_left,
                    diagonalLine, diagonalBottom
                    });


            // Center Line : Red color
            Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            centerLineList.Add(centerline);

            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref virtualLineList, layerService.LayerVirtualLine);

            singleModel.Entities.AddRange(centerLineList);
            singleModel.Entities.AddRange(lineList);
            singleModel.Entities.AddRange(virtualLineList);

            singleModel.Entities.Regen();
            singleModel.Invalidate();

            singleModel.SetView(viewType.Top);
            singleModel.ZoomFit();
            singleModel.Refresh();

        }

        public void DrawRoof_CompressionRIng_JointDetail_Detail_A(Model singleModel)
        {

            // Roof Compression RIng Joint Detail : Detail "A"

            singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Line> lineList = new List<Line>();
            List<Entity> virtualLineList = new List<Entity>();

            Point3D referencePoint = new Point3D(0, 0);

            // bottom Plate
            Point3D bottomStartPoint = GetSumPoint(referencePoint, 25, 0);
            List<Line> bottomLineList = GetRectangle(bottomStartPoint, 25, 4, RECTANGLUNVIEW.BOTTOM);
            Line bottomRight = bottomLineList[0];
            Line bottomTop = bottomLineList[1];
            Line bottomLeft = bottomLineList[2];


            Point3D rotatePoint = GetSumPoint(referencePoint, 25, 25);

            // Mid Plate
            Point3D midStartPoint = GetSumPoint(referencePoint, 0, 25);
            List<Line> midLineList = GetRectangle(midStartPoint, 6, 95);

            // Rotate : mid Plate   
            for (int i = 0; i < midLineList.Count; i++)
                midLineList[i].Rotate(Utility.DegToRad(6), Vector3D.AxisZ, rotatePoint);

            Line midBottom = midLineList[0];
            Line midRight = midLineList[1];
            Line midTop = midLineList[2];
            Line midLeft = midLineList[3];


            // Roof Plate
            Point3D roofStartPoint = GetSumPoint(referencePoint, 95 - 25, 25 + 6);
            List<Line> roofPlateLineList = GetRectangle(roofStartPoint, 3, 45, RECTANGLUNVIEW.RIGHT);

            // Rotate : roof Plate
            for (int i = 0; i < roofPlateLineList.Count; i++)
                roofPlateLineList[i].Rotate(Utility.DegToRad(6), Vector3D.AxisZ, rotatePoint);

            Line roofPlate_bottom = roofPlateLineList[0];
            Line roofPlate_top = roofPlateLineList[1];
            Line roofPlate_left = roofPlateLineList[2];


            // Diagonal Line
            Point3D diagonalStartPoint = GetSumPoint(roofStartPoint, -3, 0);
            Line diagonalLine = new Line(diagonalStartPoint, GetSumPoint(roofStartPoint, 0, 3));
            diagonalLine.Rotate(Utility.DegToRad(6), Vector3D.AxisZ, rotatePoint);
            Line diagonalBottom = new Line(diagonalStartPoint, roofPlate_bottom.StartPoint);

            virtualLineList.AddRange(new Entity[]{
                    bottomRight, bottomTop, bottomLeft,
                    midBottom, midRight, midTop, midLeft,
                    });

            lineList.AddRange(new Line[]{
                    roofPlate_bottom, roofPlate_top, roofPlate_left,
                    diagonalLine, diagonalBottom
                    });


            // Center Line : Red color
            Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            centerLineList.Add(centerline);

            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref virtualLineList, layerService.LayerVirtualLine);

            singleModel.Entities.AddRange(centerLineList);
            singleModel.Entities.AddRange(lineList);
            singleModel.Entities.AddRange(virtualLineList);

            singleModel.Entities.Regen();
            singleModel.Invalidate();

            singleModel.SetView(viewType.Top);
            singleModel.ZoomFit();
            singleModel.Refresh();
        }

        public void DrawShell_CompressionRIng_JointDetail_Detail_A(Model singleModel)
        {

            // Shell Compression RIng Joint Detail : Detail "A"

            singleModel.Entities.Clear();

            List<Entity> centerLineList = new List<Entity>();
            List<Line> lineList = new List<Line>();
            List<Entity> virtualLineList = new List<Entity>();

            Point3D referencePoint = new Point3D(0, 0);

            // bottom Plate
            Point3D bottomStartPoint = GetSumPoint(referencePoint, 30 + 1.5, 0);
            List<Line> bottomLineList = GetRectangle(bottomStartPoint, 20, 4, RECTANGLUNVIEW.BOTTOM);

            Line bottomRight = bottomLineList[0];
            Line bottomTop = bottomLineList[1];
            Line bottomLeft = bottomLineList[2];


            // Mid Plate
            Point3D midStartPoint = GetSumPoint(referencePoint, 30, 20);
            List<Line> midLineList = GetRectangle(midStartPoint, 6, 7, RECTANGLEALIGN.CENTER, 0, 4, RECTANGLUNVIEW.TOP);

            Line midBottom = midLineList[0];
            Line midRight = midLineList[1];
            Line midLeft = midLineList[2];


            // Top Plate
            Point3D topStartPoint = midLeft.StartPoint;
            List<Line> topLineList = GetRectangle(topStartPoint, 50 - 6, 7, RECTANGLUNVIEW.BOTTOM);

            Line topRight = topLineList[0];
            Line topTop = topLineList[1];
            Line topLeft = topLineList[2];


            // Left Plate
            Point3D leftStartPoint = GetSumPoint(referencePoint, 0, 20 + 50 - 12 - 7);
            List<Line> leftLineList = GetRectangle(leftStartPoint, 7, 30);

            Line leftBottom = leftLineList[0];
            Line leftRight = leftLineList[1];
            Line leftTop = leftLineList[2];
            Line leftLeft = leftLineList[3];


            Point3D rotatePoint = topRight.StartPoint;

            // Roof Plate
            Point3D roofStartPoint = rotatePoint;
            List<Line> roofPlateLineList = GetRectangle(roofStartPoint, 3, 60, RECTANGLUNVIEW.RIGHT);  // 3 임의값 , 확인후 재입력할 것

            // Rotate : roof Plate
            for (int i = 0; i < roofPlateLineList.Count; i++)
                roofPlateLineList[i].Rotate(Utility.DegToRad(9.5), Vector3D.AxisZ, rotatePoint);

            Line roofPlate_bottom = roofPlateLineList[0];
            Line roofPlate_top = roofPlateLineList[1];
            Line roofPlate_left = roofPlateLineList[2];


            // Diagonal Line
            Point3D diagonalStartPoint = GetSumPoint(roofStartPoint, -3, 0); // -3 임의 값, 확인 후 재입력할 것
            Line diagonalLine = new Line(diagonalStartPoint, roofPlate_top.StartPoint);
            Line diagonalBottom = new Line(diagonalStartPoint, roofPlate_bottom.StartPoint);

            virtualLineList.AddRange(new Entity[]{
                    bottomRight, bottomTop, bottomLeft,
                    midBottom, midRight, midLeft,
                    topRight, topTop, topLeft,
                    leftBottom, leftRight, leftTop, leftLeft,
                    });

            lineList.AddRange(new Line[]{
                    roofPlate_bottom, roofPlate_top, roofPlate_left,
                    diagonalLine, diagonalBottom
                    });


            // Center Line : Red color
            Line centerline = new Line(new Point3D(-20, 0), new Point3D(20, 0));
            centerLineList.Add(centerline);

            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            styleService.SetLayerListLine(ref lineList, layerService.LayerOutLine);
            styleService.SetLayerListEntity(ref virtualLineList, layerService.LayerVirtualLine);

            singleModel.Entities.AddRange(centerLineList);
            singleModel.Entities.AddRange(lineList);
            singleModel.Entities.AddRange(virtualLineList);

            singleModel.Entities.Regen();
            singleModel.Invalidate();

            singleModel.SetView(viewType.Top);
            singleModel.ZoomFit();
            singleModel.Refresh();
        }


        public List<Line> GetRectangle(Point3D startPoint, double width, double length, RECTANGLUNVIEW unViewPosition)
        {
            // startPoint = LeftBottom point
            List<Line> rectangleLineList = new List<Line>();

            if (unViewPosition != RECTANGLUNVIEW.BOTTOM)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, 0), GetSumPoint(startPoint, length, 0)));
            if (unViewPosition != RECTANGLUNVIEW.RIGHT)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, length, width), GetSumPoint(startPoint, length, 0)));
            if (unViewPosition != RECTANGLUNVIEW.TOP)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, width), GetSumPoint(startPoint, length, width)));
            if (unViewPosition != RECTANGLUNVIEW.LEFT)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, width), GetSumPoint(startPoint, 0, 0)));

            return rectangleLineList;
        }

        public List<Line> GetRectangle(Point3D startPoint, double width, double length, RECTANGLEALIGN align = RECTANGLEALIGN.NONE, double widthRight = 0, double lengthBottom = 0, RECTANGLUNVIEW unViewPosition = RECTANGLUNVIEW.NONE)
        {
            // startPoint = LeftBottom point
            List<Line> rectangleLineList = new List<Line>();

            switch (align)
            {
                case RECTANGLEALIGN.NONE:
                    if (unViewPosition != RECTANGLUNVIEW.BOTTOM)
                        rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, 0), GetSumPoint(startPoint, length, 0)));
                    if (unViewPosition != RECTANGLUNVIEW.RIGHT)
                        rectangleLineList.Add(new Line(GetSumPoint(startPoint, length, width), GetSumPoint(startPoint, length, 0)));
                    if (unViewPosition != RECTANGLUNVIEW.TOP)
                        rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, width), GetSumPoint(startPoint, length, width)));
                    if (unViewPosition != RECTANGLUNVIEW.LEFT)
                        rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, width), GetSumPoint(startPoint, 0, 0)));
                    break;

                case RECTANGLEALIGN.CENTER:
                    double widthMax = width;
                    double lengthMax = length;

                    if (widthRight != 0) { }
                    if (lengthBottom != 0)
                    {
                        double jumpSpace = (lengthMax - lengthBottom) / 2;

                        if (unViewPosition != RECTANGLUNVIEW.BOTTOM)
                            rectangleLineList.Add(new Line(GetSumPoint(startPoint, jumpSpace, 0), GetSumPoint(startPoint, jumpSpace + lengthBottom, 0)));
                        if (unViewPosition != RECTANGLUNVIEW.RIGHT)
                            rectangleLineList.Add(new Line(GetSumPoint(startPoint, length, width), GetSumPoint(startPoint, jumpSpace + lengthBottom, 0)));
                        if (unViewPosition != RECTANGLUNVIEW.TOP)
                            rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, width), GetSumPoint(startPoint, length, width)));
                        if (unViewPosition != RECTANGLUNVIEW.LEFT)
                            rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, width), GetSumPoint(startPoint, jumpSpace, 0)));

                        if (length < lengthBottom) lengthMax = lengthBottom;
                        {

                        }

                    }

                    break;

            }


            return rectangleLineList;
        }

        public Line MirrorLine(Line orgLine, Vector2D axis, double distance)
        {
            // Reference Position
            // AxisX = Left, AxisY = Top

            Line mirrorLine = null;

            int axisXY = 0;
            if (axis == Vector2D.AxisY) axisXY = 1;

            switch (axisXY)
            {
                case 0: // AxisX
                    {

                        // vertical Line Type
                        if (orgLine.StartPoint.X == orgLine.EndPoint.X)
                        {
                            if (orgLine.StartPoint.Y < orgLine.EndPoint.Y)
                            {
                                Point3D tempPoint = orgLine.EndPoint;
                                orgLine.EndPoint = orgLine.StartPoint;
                                orgLine.StartPoint = tempPoint;
                            }

                            return mirrorLine = new Line(GetSumPoint(orgLine.StartPoint, distance * 2, 0), GetSumPoint(orgLine.EndPoint, distance * 2, 0));
                        }
                        else
                        {
                            if (orgLine.StartPoint.X > orgLine.EndPoint.X)
                            {
                                Point3D tempPoint = orgLine.EndPoint;
                                orgLine.EndPoint = orgLine.StartPoint;
                                orgLine.StartPoint = tempPoint;
                            }

                            double distanceXLine = Math.Abs(orgLine.EndPoint.X - orgLine.StartPoint.X);
                            return mirrorLine = new Line(GetSumPoint(orgLine.EndPoint, distance * 2, 0), GetSumPoint(orgLine.StartPoint, (distanceXLine * 2) + (distance * 2), 0));
                        }

                    }

                case 1: // AxisY
                    {
                        if (orgLine.StartPoint.Y == orgLine.EndPoint.Y)
                        {
                            if (orgLine.StartPoint.X > orgLine.EndPoint.X)
                            {
                                Point3D tempPoint = orgLine.EndPoint;
                                orgLine.EndPoint = orgLine.StartPoint;
                                orgLine.StartPoint = tempPoint;
                            }

                            return mirrorLine = new Line(GetSumPoint(orgLine.StartPoint, 0, -(distance * 2)), GetSumPoint(orgLine.EndPoint, 0, -(distance * 2)));
                        }
                        else
                        {
                            if (orgLine.StartPoint.Y < orgLine.EndPoint.Y)
                            {
                                Point3D tempPoint = orgLine.EndPoint;
                                orgLine.EndPoint = orgLine.StartPoint;
                                orgLine.StartPoint = tempPoint;
                            }

                            double distanceYLine = Math.Abs(orgLine.StartPoint.Y - orgLine.EndPoint.Y);
                            return mirrorLine = new Line(GetSumPoint(orgLine.EndPoint, 0, -(distance * 2)), GetSumPoint(orgLine.StartPoint, 0, -((distanceYLine * 2) + (distance * 2))));

                            //return mirrorLine = new Line(GetSumPoint(orgLine.EndPoint, 0, -(distance * 2)), GetSumPoint(orgLine.StartPoint, 0, -((orgLine.Length()) + (distance * 2))));
                        }
                    }
            }

            return null;
        }

        public Line GetInterSectLine_withTwoLines(Line leftLine, Line intersectLine, Line rightLine)
        {
            Point3D pointA = new Point3D(0, 0);
            Point3D pointB = new Point3D(0, 0);

            Point3D[] intersectPoint = intersectLine.IntersectWith(leftLine);
            if (intersectPoint.Count() < 2)
            {
                pointA = intersectPoint[0];

                intersectPoint = intersectLine.IntersectWith(rightLine);
                if (intersectPoint.Count() > 1)
                    return null;

                pointB = intersectPoint[0];
            }
            else { return null; }

            // PointA = 왼쪽 겹침점 , PointB = 오른쪽 겸침점
            return new Line(pointA, pointB);
        }

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
    }
}
