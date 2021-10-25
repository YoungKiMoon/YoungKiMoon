using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using devDept.Eyeshot;
using devDept.Graphics;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Serialization;
using MColor = System.Windows.Media.Color;
using Color = System.Drawing.Color;
using Environment = devDept.Eyeshot.Environment;
using System.Collections.ObjectModel;

using DrawWork.DrawModels;
using DrawWork.DrawAutomationService;
using DrawWork.Commons;
using DrawWork.DrawStyleServices;
using DrawWork.DrawCustomObjectModels;

using AssemblyLib.AssemblyModels;
using DrawWork.ValueServices;
using DrawWork.CutomModels;
using DrawWork.DrawShapes;
using DrawWork.DrawSacleServices;
using DrawWork.DrawDetailServices;
using DrawWork.AssemblyServices;

namespace DrawWork.DrawServices
{
    public class DrawSettingService
    {

        private ValueService valueService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;
        private DrawEditingService editingService;
        private DrawShapeServices shapeService;

        public DrawSettingService()
        {
            valueService = new ValueService();
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();
            editingService = new DrawEditingService();
            shapeService = new DrawShapeServices();
        }
        public void SetModelSpace(Model singleModel)
        {
            singleModel.Invalidate();

            LineStyleService lineStyle = new LineStyleService();
            singleModel.LineTypes.AddRange(lineStyle.GetDefaultStyle());

            LayerStyleService layerStyle = new LayerStyleService();
            singleModel.Layers.AddRange(layerStyle.GetDefaultStyle());

            TextStyleService textService = new TextStyleService();
            singleModel.TextStyles.AddRange(textService.GetDefaultStyle());
        }
        
        public void SetPaperSpace(Drawings singleDraw)
        {
            singleDraw.Invalidate();

            LineStyleService lineStyle = new LineStyleService();
            singleDraw.LineTypes.AddRange(lineStyle.GetDefaultStyle());

            LayerStyleService layerStyle = new LayerStyleService();
            singleDraw.Layers.AddRange(layerStyle.GetDefaultStyle());

            TextStyleService textService = new TextStyleService();
            singleDraw.TextStyles.AddRange(textService.GetDefaultStyle());
        }






        public void CreateModelSpaceSample(Model singleModel)
        {
            bool visibleFalse = false;
            singleModel.Entities.Clear();

            #region Sample
            //if (false)
            //{

            //    #region 샘플 라인 테스트
            //    if (false)
            //    {


            //        LinearPath rectBox = new LinearPath(140, 100);
            //        rectBox.ColorMethod = colorMethodType.byEntity;
            //        rectBox.Color = Color.Green;
            //        singleModel.Entities.Add(rectBox);

            //        Line cusL = new Line(0, 100, 70, 120);
            //        singleModel.Entities.Add(cusL);
            //        Line cusR = new Line(70, 120, 140, 100);
            //        singleModel.Entities.Add(cusR);


            //        LinearPath rectBoxSmall = new LinearPath(20, 16);
            //        rectBoxSmall.Translate(180, 0);
            //        rectBoxSmall.ColorMethod = colorMethodType.byEntity;
            //        rectBoxSmall.Color = Color.Red;
            //        singleModel.Entities.Add(rectBoxSmall);

            //        //Line cusH = new Line(-500, 0, 500, 0);
            //        //Line cusV = new Line(0, -500, 0, 500);
            //        //Line cusHV = new Line(-500, -500, 500, 500);
            //        //singleModel.Entities.Add(cusH, LayerDashDot);
            //        //singleModel.Entities.Add(cusV, LayerDashDot);
            //        //singleModel.Entities.Add(cusHV, LayerDashDot);
            //    }
            //    #endregion

            //    #region Composite curve
            //    if (false)
            //    {


            //        //six composite curve
            //        Arc arc = new Arc(0, 0, 0, 113, (Math.PI / 3), (Math.PI * 2 / 3));
            //        singleModel.Entities.Add(new Arc(arc.Center, 113, Utility.DegToRad(45), Utility.DegToRad(70)));

            //        Line line1 = new Line(-45, 113, -45, 0);
            //        //Line line2 = new Line(55, 49, 55, 113); //error
            //        Line line2 = new Line(-40, 49, 15, 49);
            //        line2.Rotate(Utility.DegToRad(30), Vector3D.AxisZ, new Point3D(-45, 49));
            //        singleModel.Entities.Add((Line)line2.Clone(), Color.Blue);

            //        singleModel.Entities.Add((Line)line1.Clone(), Color.Green);

            //        Arc arcFillet1, arcFillet2, arcFillet3;
            //        Curve.Fillet(arc, line1, 9.5, false, false, true, true, out arcFillet1);
            //        Curve.Fillet(arc, line2, 9.5, false, false, true, true, out arcFillet2);
            //        Curve.Fillet(line1, line2, 9.5, false, false, true, true, out arcFillet3);

            //        singleModel.Entities.Add(arcFillet3, Color.Red);
            //        //CompositeCurve compositeCurve = new CompositeCurve(arc, line1, line2, arcFillet1, arcFillet2, arcFillet3);
            //        //singleModel.Entities.Add(compositeCurve);

            //        //for (int i = 0; i < 6; i++)
            //        //{
            //        //    CompositeCurve compCurve = (CompositeCurve)compositeCurve.Clone();
            //        //    compCurve.Rotate((Math.PI / 3) * i, Vector3D.AxisZ);
            //        //    singleModel.Entities.Add(compCurve);
            //        //}
            //    }
            //    #endregion

            //    #region 병따게 Test
            //    if (false)
            //    {


            //        // adding the axis of symmetry
            //        Line axisX = new Line(-30, 0, 90, 0);
            //        axisX.LineTypeMethod = colorMethodType.byEntity;
            //        singleModel.Entities.Add(axisX);

            //        // drawing segment and arcs
            //        Line ln1 = new Line(76, 0, 76, 3);
            //        singleModel.Entities.Add(ln1, Color.Red);

            //        Arc a1 = new Arc(new Point3D(70, 3, 0), 6, 0, Math.PI / 2);
            //        singleModel.Entities.Add(a1, Color.Green);

            //        Line ln2 = new Line(70, 9, 28, 9);
            //        singleModel.Entities.Add(ln2, Color.AliceBlue);

            //        Arc a2 = new Arc(new Point3D(12.52, 9, 0), 1, Math.PI / 2, -Math.PI / 2);
            //        singleModel.Entities.Add(a2, Color.Orange);

            //        Line ln3 = new Line(-2.5, 0, -2.5, 14);
            //        singleModel.Entities.Add(ln3, Color.Black);
            //        Line ln4 = new Line(-7.5, 0, -7.5, 14);
            //        singleModel.Entities.Add(ln4, Color.Black);

            //        Circle cr1 = new Circle(30, 28, 0, 19);
            //        Arc a3 = new Arc(new Point3D(0, 14, 0), 7.5, Math.PI);
            //        Arc a4 = new Arc(new Point3D(0, 14, 0), 2.5, Math.PI);

            //        singleModel.Entities.Add(cr1, Color.Green);
            //        singleModel.Entities.Add(a3, Color.Red);
            //        singleModel.Entities.Add(a4, Color.Blue);

            //        // finding the tangent to two circles
            //        Line ln5 = UtilityEx.GetLinesTangentToTwoCircles(cr1, a3)[1];
            //        singleModel.Entities.Add(ln5, Color.Blue);
            //        Line ln6 = UtilityEx.GetLinesTangentToTwoCircles(a2, a4)[0];
            //        singleModel.Entities.Add(ln6, Color.Red);


            //        Line lnTest = UtilityEx.GetLinesTangentToCircleFromPoint(a4, new Point3D(2, 0, 0))[0];
            //        singleModel.Entities.Add(lnTest, Color.Green);

            //        // creating a Fillet
            //        Arc a5;
            //        Curve.Fillet(ln2, ln5, 19, false, false, true, true, out a5);
            //        a5.Reverse();

            //        double angle = Vector3D.AngleBetween(a5.Plane.AxisX, cr1.Plane.AxisX);

            //        a5.Rotate(angle, a5.Plane.AxisZ, a5.Center);
            //        a5.Domain = new Interval(a5.Domain.t0 - angle, a5.Domain.t1 - angle);


            //        //singleModel.Entities.Add(a5, Color.Blue);

            //        // trimming the curves
            //        //Curve.Trim(a3, ln5, true, true);
            //        //singleModel.Entities.Add(a3, Color.Black);

            //        //Curve.Trim(a4, ln6, true, true);
            //        //singleModel.Entities.Add(a4, Color.Black);
            //    }
            //    #endregion

            //    #region 호 그리기 : Arc 사용
            //    if (false)
            //    {
            //        Arc testArc = new Arc(new Point3D(0, 0, 0), new Point3D(20, 0, 0), new Point3D(0, 20, 0));
            //        singleModel.Entities.Add(testArc, Color.Red);
            //    }

            //    #endregion

            //    #region 자르기
            //    if (false)
            //    {
            //        Circle c1 = new Circle(Plane.XY, new Point2D(0, 0), 40); // Outer circle top right
            //        Circle c2 = new Circle(Plane.XY, new Point2D(0, -90), 20); // Outer circle bottom
            //        Circle c3 = new Circle(Plane.XY, new Point2D(-41, 0), 20); // Outer circle top left 

            //        // Linking arcs
            //        Arc a98 = (Arc)UtilityEx.GetCirclesTangentToTwoCircles(c1, c2, 98, true)[3]; // Radius 98 arc
            //        Arc a20 = (Arc)UtilityEx.GetCirclesTangentToTwoCircles(c1, c2, 20, true)[1]; // Radius 20 arc
            //        Arc a9 = (Arc)UtilityEx.GetCirclesTangentToTwoCircles(c3, c1, 9, true)[0]; // Radius 9 arc

            //        Arc a1 = new Arc(c1.Center, c1.Radius, Math.PI * 2);
            //        a1.TrimBy(a9.EndPoint, false);
            //        a1.TrimBy(a98.StartPoint, true);

            //        //Arc a2 = new Arc(c2.Center, c2.Radius, Math.PI * 2);
            //        //a2.TrimBy(a98.EndPoint, false);
            //        //a2.TrimBy(a20.EndPoint, true);

            //        //singleModel.Entities.Add(c1, Color.Yellow);
            //        singleModel.Entities.Add(a9, Color.Red);
            //        singleModel.Entities.Add(a98, Color.Green);
            //        singleModel.Entities.Add(a1, Color.Blue);

            //    }
            //    #endregion

            //    #region Angle
            //    if (false)
            //    {
            //        // Current Test
            //        double AB = 50;
            //        double t = 6;
            //        double R1 = 6.5;
            //        double R2 = 4.5;
            //        double CD = 14.4;
            //        double E = 10;

            //        Point3D drawPoint = new Point3D(0, 0, 0);

            //        Line lineA = new Line(new Point3D(0, AB, 0), new Point3D(AB, AB, 0));
            //        Line lineAa = new Line(new Point3D(0, AB - t, 0), new Point3D(AB - t, AB - t, 0));
            //        Line lineAt = new Line(new Point3D(0, AB, 0), new Point3D(0, AB - t, 0));
            //        Line lineB = new Line(new Point3D(AB, AB, 0), new Point3D(AB, 0, 0));
            //        Line lineBb = new Line(new Point3D(AB - t, AB - t, 0), new Point3D(AB - t, 0, 0));
            //        Line lineBt = new Line(new Point3D(AB - t, 0, 0), new Point3D(AB, 0, 0));


            //        //singleModel.Entities.Add(lineB, Color.Red);
            //        //singleModel.Entities.Add(lineBb, Color.Green);

            //        Arc arcFillet1;
            //        if (Curve.Fillet(lineAt, lineAa, R2, false, false, true, true, out arcFillet1))
            //            singleModel.Entities.Add(arcFillet1, Color.Blue);
            //        Arc arcFillet2;
            //        if (Curve.Fillet(lineBb, lineBt, R2, false, false, true, true, out arcFillet2))
            //            singleModel.Entities.Add(arcFillet2, Color.Blue);
            //        Arc arcFillet3;
            //        if (Curve.Fillet(lineAa, lineBb, R1, false, false, true, true, out arcFillet3))
            //            singleModel.Entities.Add(arcFillet3, Color.Blue);

            //        singleModel.Entities.Add(lineA, Color.Red);
            //        singleModel.Entities.Add(lineAa, Color.Green);
            //        singleModel.Entities.Add(lineAt, Color.Blue);
            //        singleModel.Entities.Add(lineB, Color.Red);
            //        singleModel.Entities.Add(lineBb, Color.Green);
            //        singleModel.Entities.Add(lineBt, Color.Blue);
            //    }
            //    #endregion

            //    #region Nozzle
            //    if (false)
            //    {
            //        // Current Test
            //        double flangeFaceWidth = 10;
            //        double flangeFaceHeight = 80;
            //        double flangeFaceInnerWidth = 10;
            //        double flangePipeWidth = 24;
            //        double flangePipeHeight = flangeFaceHeight - (flangeFaceInnerWidth * 2);
            //        double flangePipeInnerWidth = 18;
            //        double pipeHeight = flangeFaceHeight - (flangePipeInnerWidth * 2);
            //        double pipeWidth = 40;
            //        double fullWidth = flangeFaceWidth + flangePipeWidth + pipeWidth;

            //        Point3D drawPoint = new Point3D(0, 0, 0);

            //        Line lineFFa = new Line(new Point3D(0, 0, 0), new Point3D(0, flangeFaceHeight, 0));
            //        Line lineFFb = new Line(new Point3D(flangeFaceWidth, 0, 0), new Point3D(flangeFaceWidth, flangeFaceHeight, 0));
            //        Line lineFFc = new Line(new Point3D(0, 0, 0), new Point3D(flangeFaceWidth, 0, 0));
            //        Line lineFFd = new Line(new Point3D(0, flangeFaceHeight, 0), new Point3D(flangeFaceWidth, flangeFaceHeight, 0));

            //        Line lineFPa = new Line(new Point3D(flangeFaceWidth, flangeFaceInnerWidth + flangePipeHeight, 0), new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth + pipeHeight, 0));
            //        Line lineFPb = new Line(new Point3D(flangeFaceWidth, flangeFaceInnerWidth, 0), new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0));
            //        Line lineFPc = new Line(new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth + pipeHeight, 0), new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0));

            //        Line linePa = new Line(new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0), new Point3D(flangeFaceWidth + flangePipeWidth + pipeWidth, flangePipeInnerWidth, 0));
            //        Line linePb = new Line(new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth + pipeHeight, 0), new Point3D(flangeFaceWidth + flangePipeWidth + pipeWidth, flangePipeInnerWidth + pipeHeight, 0));

            //        singleModel.Entities.AddRange(new Entity[] { lineFFa, lineFFb, lineFFc, lineFFd, lineFPa, lineFPb, lineFPc, linePa, linePb });
            //    }
            //    #endregion

            //    #region Nozzle Leader
            //    if (false)
            //    {
            //        // Current Test
            //        double cirRadius = 100;
            //        double cirDiameter = cirRadius * 2;
            //        double cirTextSize = 30;
            //        string textUpperStr = "asdf";
            //        string textLowerStr = "dadsf";
            //        Point3D drawPoint = new Point3D(0, 0, 0);

            //        Circle circleCenter = new Circle(cirRadius, cirRadius, 0, cirRadius);
            //        Line lineCenter = new Line(new Point3D(0, cirRadius, 0), new Point3D(cirDiameter, cirRadius, 0));
            //        Line lineCenter3 = new Line(new Point3D(0, cirRadius, 0), new Point3D(cirDiameter, cirRadius - 20, 0));
            //        Line lineCenter2 = new Line(new Point3D(0, cirRadius, 0), new Point3D(cirDiameter, cirRadius + 20, 0));
            //        Vector3D ssPlane = new Vector3D();
            //        ssPlane.Y = 200;
            //        Line lineCenter4 = (Line)lineCenter2.Offset(500, ssPlane);
            //        Line lineCenter5 = (Line)lineCenter3.Offset(-500, Vector3D.AxisZ);
            //        Text textUpper = new Text(cirRadius, cirRadius + (cirRadius / 2), 0, textUpperStr, cirTextSize);
            //        textUpper.Alignment = Text.alignmentType.MiddleCenter;
            //        Text textLower = new Text(cirRadius, (cirRadius / 2), 0, textUpperStr, cirTextSize);
            //        textLower.Alignment = Text.alignmentType.MiddleCenter;

            //        singleModel.Entities.AddRange(new Entity[] { circleCenter, lineCenter, lineCenter2, lineCenter3, lineCenter4, lineCenter5, textUpper, textLower });
            //    }
            //    #endregion

            //    #region Dimension
            //    if (false)
            //    {

            //        // Y axis
            //        Line axisY = new Line(0, -45, 0, 45)
            //        {
            //            LineTypeMethod = colorMethodType.byEntity,
            //        };


            //        Mirror m = new Mirror(Plane.ZY);

            //        // Building geometry - External Outline

            //        Line lT1 = new Line(34.8, 30.2, 34.8, 44.6);
            //        Line lT2 = new Line(28.8, 30.2, 28.8, 44.6);
            //        Line lT3 = new Line(25.6, 32.6, 0.0, 32.6);
            //        Circle c = new Circle(Plane.XY, new Point3D(31.8, 28, 0), 13.2);
            //        Circle c_mir = (Circle)c.Clone();
            //        c_mir.TransformBy(m);
            //        Point3D int1 = Curve.Intersection(c, lT1)[0];
            //        Point3D int2 = Curve.Intersection(c, lT2)[0];
            //        Point3D int3 = Curve.Intersection(c, lT3)[0];

            //        singleModel.Entities.AddRange(new Entity[] { axisY, lT1, lT2, lT3, c, c_mir });
            //        // External outline
            //        Line eo1 = new Line(0.0, -36, 17.8, -36);
            //        Line eo2 = new Line(17.8, -36, 17.8, -42);
            //        Line eo3 = new Line(17.8, -42, 51, -42);
            //        Line eo4 = new Line(51, -42, 51, -12.6);
            //        Line eo5 = new Line(51, -12.6, 43.8, -12.6);
            //        Line eo7 = new Line(43.8, 0.6, 51, 0.6);
            //        Arc eo6 = new Arc(Plane.XY, new Point3D(43.8, -6.0, 0.0), 6.6, eo5.EndPoint, eo7.StartPoint, true);
            //        Line eo8 = new Line(51, 0.6, 51, 22.6);
            //        Line eo9 = new Line(51, 22.6, 45, 22.6);
            //        Line eo10 = new Line(45, 22.6, 45, 28);
            //        Arc eo11 = new Arc(Plane.XY, new Point3D(31.8, 28, 0), 13.2, eo10.EndPoint, int1, false);
            //        Line eo12 = new Line(int1.X, int1.Y, 34.8, 44.6);
            //        Line eo13 = new Line(34.8, 44.6, 28.8, 44.6);
            //        Line eo14 = new Line(28.8, 44.6, int2.X, int2.Y);
            //        Arc eo15 = new Arc(Plane.XY, new Point3D(31.8, 28, 0), 13.2, int2, int3, false);
            //        Line eo16 = new Line(int3.X, int3.Y, 0, int3.Y);


            //        singleModel.Entities.AddRange(new Entity[] { eo1, eo2, eo3, eo4, eo5, eo6, eo7, eo8, eo9, eo10, eo11, eo12, eo13, eo14, eo15, eo16 });

            //        // Holes
            //        Circle c1 = new Circle(Plane.XY, new Point3D(31.8, 28, 0), 6);
            //        Circle c2 = (Circle)c1.Clone();
            //        c2.TransformBy(m);
            //        CompositeCurve h1 = CompositeCurve.CreateHexagon(31.8, 28, 6);
            //        //h1.Rotate(Math.PI / 6, Vector3D.AxisZ, new Point3D(31.8, 28, 0));
            //        h1.TransformBy(m);

            //        singleModel.Entities.AddRange(new Entity[] { h1 });




            //        float txtH = 2.0f;
            //        DiametricDim ddd1 = new DiametricDim(c1, new Point3D(20.0, 20.0, 0.0), txtH)
            //        {
            //            ArrowsLocation = elementPositionType.Outside,
            //            TextPrefix = "2-Ø"
            //        };

            //        singleModel.Entities.AddRange(new Entity[] { ddd1 });


            //        Plane left = new Plane(Point3D.Origin, Vector3D.AxisY, -1 * Vector3D.AxisX); // plane.XY, vertical writing;
            //        Plane right = new Plane(Point3D.Origin, Vector3D.AxisY, -1 * Vector3D.AxisX); // plane.XY, vertical writing;

            //        LinearDim dd1 = new LinearDim(Plane.XY, new Point2D(-28.8, -10), new Point2D(-34.8, -10), new Point2D(-38.0, 49.6), txtH)
            //        {
            //            ArrowsLocation = elementPositionType.Outside
            //        };

            //        LinearDim dd2 = new LinearDim(Plane.XY, new Point2D(-28, -10), new Point2D(-34, -10), new Point2D(-38.0, 30), txtH)
            //        {
            //            ArrowsLocation = elementPositionType.Outside
            //        };

            //        LinearDim dd3 = new LinearDim(Plane.XY, new Point2D(-28, -10), new Point2D(-34, -10), new Point2D(0, 30), txtH)
            //        {
            //            ArrowsLocation = elementPositionType.Inside
            //        };


            //        LinearDim dd4 = new LinearDim(left, new Point2D(-36.0, -4.0), new Point2D(-15.57, -22.9), new Point2D(-25.79, -4.0), txtH)
            //        {
            //            ArrowsLocation = elementPositionType.Inside
            //        };

            //        LinearDim dd5 = new LinearDim(Plane.XY, new Point2D(-36.0, -4.0), new Point2D(-15.57, -22.9), new Point2D(-25.79, -4.0), txtH)
            //        {
            //            ArrowsLocation = elementPositionType.Inside
            //        };

            //        LinearDim dd6 = new LinearDim(Plane.YX, new Point2D(43.8, -6.0), new Point2D(51.0, -20.0), new Point2D(54.0, -20.0), txtH)
            //        {
            //            ArrowsLocation = elementPositionType.Outside
            //        };

            //        //LinearDim dimTop = new LinearDim(Plane.XY, new Point2D(0, 19300), new Point2D(32400, 19300), new Point2D(16200,23300), 1000)
            //        LinearDim dimTop = new LinearDim(Plane.XY, new Point2D(-10, 20), new Point2D(50, 20), new Point2D(25, 30), txtH)
            //        {
            //            ArrowsLocation = elementPositionType.Inside
            //        };
            //        Plane ccleft = new Plane(new Point3D(0, 0), Vector3D.AxisY, -1 * Vector3D.AxisX); // plane.XY, vertical writing;
            //        // 0,10  0,50  -10, 25
            //        LinearDim dd8 = new LinearDim(ccleft, new Point2D(10, 0), new Point2D(50, 0), new Point2D(25, 10), txtH)
            //        {
            //            ArrowsLocation = elementPositionType.Inside
            //        };
            //        Plane ccright = new Plane(new Point3D(0, 0), -1 * Vector3D.AxisY, Vector3D.AxisX); // plane.XY, vertical writing;
            //        // 20,10  20,-50  30, -25
            //        LinearDim dd9 = new LinearDim(ccright, new Point2D(-10, 20), new Point2D(50, 20), new Point2D(25, 30), txtH)
            //        {
            //            ArrowsLocation = elementPositionType.Inside
            //        };

            //        Plane ccbottom = new Plane(new Point3D(0, 0), -1 * Vector3D.AxisY, Vector3D.AxisX); // plane.XY, vertical writing;
            //        // 20,10  20,-50  30, -25
            //        LinearDim dd10 = new LinearDim(ccbottom, new Point2D(-10, 20), new Point2D(50, 20), new Point2D(25, 30), txtH)
            //        {
            //            ArrowsLocation = elementPositionType.Inside
            //        };

            //        singleModel.Entities.Add(dd1, Color.Red);
            //        singleModel.Entities.Add(dd2, Color.Green);
            //        singleModel.Entities.Add(dd3, Color.Blue);
            //        singleModel.Entities.Add(dd4, Color.Yellow);
            //        singleModel.Entities.Add(dd5, Color.YellowGreen);
            //        singleModel.Entities.Add(dd6, Color.Orange);

            //        singleModel.Entities.Add(dimTop, Color.SkyBlue);
            //        //singleModel.Entities.Add(dd8, Color.OrangeRed);
            //        singleModel.Entities.Add(dd9, Color.LightSeaGreen);
            //        singleModel.Entities.Add(dd10, Color.Red);

            //        LinearDim dimTop2 = new LinearDim(Plane.XY, new Point2D(-10, 20), new Point2D(50, 20), new Point2D(25, 30), txtH)
            //        {
            //            ArrowsLocation = elementPositionType.Inside
            //        };
            //        dimTop2.ScaleOverall = 3;
            //        singleModel.Entities.Add(dimTop2, Color.Red);
            //        //singleModel.Entities.AddRange(new Entity[] { dd1,dd2,dd3,dd4,dd5,dd6 });

            //    }
            //    #endregion

            //    #region Dimensino 2
            //    if (false)
            //    {


            //        Point3D leaderPoint = new Point3D(0, 0, 0);
            //        double leaderLength = 15;
            //        double leaderTriShort = leaderLength / 2;
            //        double leaderTriLong = Math.Tan(UtilityEx.DegToRad(60)) * leaderTriShort;
            //        double leaderCenterX = leaderPoint.X + leaderTriShort;
            //        double leaderCenterY = leaderPoint.Y + leaderTriLong;

            //        List<Point3D> leaderPoints = new List<Point3D>();
            //        leaderPoints.Add(leaderPoint);
            //        leaderPoints.Add(new Point3D(leaderCenterX, leaderCenterY, 0));
            //        leaderPoints.Add(new Point3D(leaderCenterX + 40, leaderCenterY, 0));
            //        Leader leader1 = new Leader(Plane.XY, leaderPoints);
            //        singleModel.Entities.Add(leader1, Color.Red);

            //        //leader1.Scale = 30;
            //        //leader1.LineTypeScale = 30;
            //        Text newText = new Text(leaderCenterX + 1, leaderCenterY, 0, "LAaBbCc", 2.5);
            //        newText.Alignment = Text.alignmentType.BaselineLeft;
            //        //singleModel.FontFamily.FamilyNames= "ROMANS";

            //        Text newText2 = new Text(leaderCenterX + 1, leaderCenterY + 1, 0, "LAaBbCc", 2.5);
            //        newText2.Alignment = Text.alignmentType.BaselineLeft;

            //        singleModel.Entities.Add(newText, Color.Blue);
            //        singleModel.Entities.Add(newText2, Color.Red);


            //        Point3D textCenter = new Point3D();
            //        LinearDim newDimRight;
            //        LinearDim newDimLeft;
            //        LinearDim newDimTop;

            //        LinearDim newDimBottom;

            //        CDPoint selPoint1 = new CDPoint();
            //        CDPoint selPoint2 = new CDPoint();
            //        CDPoint selPoint3 = new CDPoint();


            //        // Top
            //        selPoint1.X = 110;
            //        selPoint1.Y = 200;
            //        selPoint2.X = 160;
            //        selPoint2.Y = 220;
            //        double selDimHeight = 34;
            //        double selTextHeight = 2.5;

            //        textCenter.X = selPoint1.X + (selPoint2.X - selPoint1.X) / 2;
            //        textCenter.Y = Math.Max(selPoint1.Y, selPoint2.Y) + selDimHeight;
            //        newDimTop = new LinearDim(Plane.XY, new Point3D(selPoint1.X, selPoint1.Y),
            //                                new Point3D(selPoint2.X, selPoint2.Y),
            //                                new Point3D(textCenter.X, textCenter.Y), selTextHeight);

            //        newDimTop.ArrowsLocation = elementPositionType.Inside;
            //        newDimTop.TextPrefix = "t";
            //        newDimTop.TextSuffix = "s";
            //        newDimTop.TextOverride = "asdfads";

            //        // Text Gap
            //        newDimTop.TextGap = 1;
            //        newDimTop.ExtLineOffset = 1;
            //        newDimTop.ExtLineExt = 1;
            //        newDimTop.ArrowheadSize = 2.5;
            //        newDimTop.ScaleOverall = 1;
            //        newDimTop.LinearScale = 1;


            //        newDimTop.Regen(new RegenParams(0, singleModel));
            //        //Mesh[] ss1 = newDimTop.ConvertToMesh(singleModel);
            //        ICurve[] ss2 = newDimTop.ConvertToCurves(singleModel);
            //        LinearPath[] ss3 = newDimTop.ConvertToLinearPaths(0,singleModel); // 외곽
            //        Region[] ss4 = newDimTop.ConvertToRegions(singleModel);
            //        Surface[] ss5 = newDimTop.ConvertToSurfaces(singleModel);

            //        //singleModel.Entities.AddRange(ss5);
            //        foreach (ICurve each in ss2)
            //            singleModel.Entities.Add(each as Entity);



            //        // Left
            //        selPoint1.X = 110;
            //        selPoint1.Y = 200;
            //        selPoint2.X = 90;
            //        selPoint2.Y = 250;

            //        selPoint1.X = -10;
            //        selPoint1.Y = 200;
            //        selPoint2.X = 10;
            //        selPoint2.Y = 250;

            //        textCenter.X = selPoint1.Y + (selPoint2.Y - selPoint1.Y) / 2;
            //        textCenter.Y = Math.Min(selPoint1.X, selPoint2.X) - selDimHeight;
            //        Plane planeLeft = new Plane(new Point3D(selPoint1.X, selPoint1.Y), Vector3D.AxisY, -1 * Vector3D.AxisX);
            //        newDimLeft = new LinearDim(planeLeft, new Point3D(selPoint1.X, selPoint1.Y),
            //                                new Point3D(selPoint2.X, selPoint2.Y),
            //                                new Point3D(textCenter.Y, textCenter.X), selTextHeight);

            //        newDimLeft.ArrowsLocation = elementPositionType.Inside;


            //        // right
            //        selPoint1.X = 110;
            //        selPoint1.Y = 200;
            //        selPoint2.X = 90;
            //        selPoint2.Y = 250;

            //        textCenter.X = selPoint1.Y + (selPoint2.Y - selPoint1.Y) / 2;
            //        textCenter.Y = Math.Max(selPoint1.X, selPoint2.X) + selDimHeight;
            //        Plane planeRight = new Plane(new Point3D(selPoint1.X, selPoint1.Y), Vector3D.AxisY, -1 * Vector3D.AxisX);
            //        newDimRight = new LinearDim(planeRight, new Point3D(selPoint1.X, selPoint1.Y),
            //                                new Point3D(selPoint2.X, selPoint2.Y),
            //                                new Point3D(textCenter.Y, textCenter.X), selTextHeight);

            //        newDimLeft.ArrowsLocation = elementPositionType.Inside;


            //        // bottom
            //        selPoint1.X = 110;
            //        selPoint1.Y = 200;
            //        selPoint2.X = 160;
            //        selPoint2.Y = 220;

            //        textCenter.X = selPoint1.X + (selPoint2.X - selPoint1.X) / 2;
            //        textCenter.Y = Math.Min(selPoint1.Y, selPoint2.Y) - selDimHeight;
            //        newDimBottom = new LinearDim(Plane.XY, new Point3D(selPoint1.X, selPoint1.Y),
            //                                new Point3D(selPoint2.X, selPoint2.Y),
            //                                new Point3D(textCenter.X, textCenter.Y), selTextHeight);

            //        newDimBottom.ArrowsLocation = elementPositionType.Inside;

            //        //singleModel.Entities.Add(newDimTop, Color.Red);
            //        singleModel.Entities.Add(newDimBottom, Color.Gray);

            //        singleModel.Entities.Add(newDimRight, Color.Blue);
            //        singleModel.Entities.Add(newDimLeft, Color.Green);
            //    }
            //    #endregion

            //    #region Line Break
            //    if (false)
            //    {




            //    }
            //    #endregion

            //    #region Line Break : ing
            //    if (false)
            //    {


            //        AutomationDimensionService autoDimService = new AutomationDimensionService();

            //        // Reference Circle
            //        double breakRadius = 10;

            //        List<Entity> outlineList = new List<Entity>();
            //        List<Entity> centerlineList = new List<Entity>();
            //        List<Entity> dimlineList = new List<Entity>();
            //        List<Entity> dimTextList = new List<Entity>();
            //        List<Entity> dimlineExtList = new List<Entity>();
            //        List<Entity> leaderlineList = new List<Entity>();
            //        List<Entity> leaderTextList = new List<Entity>();
            //        List<Entity> nozzlelineList = new List<Entity>();
            //        List<Entity> nozzleMarkList = new List<Entity>();

            //        // 세로
            //        outlineList.Add(new Line(new Point3D(10, -20, 0), new Point3D(10, 1110, 0)));
            //        centerlineList.Add(new Line(new Point3D(110, -20, 0), new Point3D(110, 1110, 0)));
            //        dimlineList.Add(new Line(new Point3D(210, -20, 0), new Point3D(210, 1110, 0)));
            //        Text dimText01 = new Text(new Point3D(0, -310, 0), "Dimension Line ABCDEFGabcdefg12345", 50);

            //        dimText01.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
            //        dimTextList.Add(dimText01);
            //        dimlineExtList.Add(new Line(new Point3D(410, -20, 0), new Point3D(410, 1110, 0)));
            //        leaderlineList.Add(new Line(new Point3D(510, -20, 0), new Point3D(510, 1110, 0)));
            //        Text leaderText01 = new Text(new Point3D(0, -610, 0), "Leader Line ABCDEFGabcdefg12345", 50);
            //        leaderText01.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
            //        leaderTextList.Add(leaderText01);
            //        nozzlelineList.Add(new Line(new Point3D(710, -20, 0), new Point3D(710, 1110, 0)));
            //        nozzleMarkList.Add(new Line(new Point3D(810, -20, 0), new Point3D(810, 1110, 0)));

            //        // 가로
            //        outlineList.Add(new Line(new Point3D(-20, 10, 0), new Point3D(1110, 10, 0)));
            //        centerlineList.Add(new Line(new Point3D(-20, 110, 0), new Point3D(1110, 110, 0)));
            //        dimlineList.Add(new Line(new Point3D(-20, 210, 0), new Point3D(1110, 210, 0)));
            //        Text dimText02 = new Text(new Point3D(-20, 310, 0), "Dimension Line ABCDEFGabcdefg12345", 50);
            //        dimTextList.Add(dimText02);
            //        dimlineExtList.Add(new Line(new Point3D(-20, 410, 0), new Point3D(1110, 410, 0)));
            //        leaderlineList.Add(new Line(new Point3D(-20, 510, 0), new Point3D(1110, 510, 0)));
            //        Text leaderText02 = new Text(new Point3D(-20, 610, 0), "Leader Line ABCDEFGabcdefg12345", 50);
            //        leaderTextList.Add(leaderText02);
            //        nozzlelineList.Add(new Line(new Point3D(-20, 710, 0), new Point3D(1110, 710, 0)));
            //        nozzleMarkList.Add(new Line(new Point3D(-20, 810, 0), new Point3D(1110, 810, 0)));


            //        // Text to Box : 수정 최소단위 사각형으로
            //        List<Entity> dimTextLinearPathList = new List<Entity>();
            //        foreach (Entity eachEntity in dimTextList)
            //        {
            //            eachEntity.Regen(new RegenParams(0, singleModel));
            //            Point3D[] textBoxSize = eachEntity.Vertices;
            //            LinearPath newRec = new LinearPath(textBoxSize);
            //            newRec.Regen(new RegenParams(0, singleModel));

            //            Point3D minPoint = GetMinPointXY(textBoxSize);
            //            Point3D maxPoint = GetMaxPointXY(textBoxSize);
            //            if (newRec.BoxSize.Y > breakRadius)
            //            {
            //                double quotient = Math.Truncate(newRec.BoxSize.Y / breakRadius);
            //                double quotientcc = newRec.BoxSize.Y / quotient;

            //                for (int i = 1; i <= quotient; i++)
            //                {
            //                    Line newHorizontal = new Line(minPoint.X, minPoint.Y + breakRadius * i, maxPoint.X, minPoint.Y + breakRadius * i);
            //                    dimTextLinearPathList.Add(newHorizontal);
            //                }
            //            }
            //            if (newRec.BoxSize.X > breakRadius)
            //            {
            //                double quotient = Math.Truncate(newRec.BoxSize.X / breakRadius);
            //                double quotientcc = newRec.BoxSize.X / quotient;

            //                for (int i = 1; i <= quotient; i++)
            //                {
            //                    Line newVertical = new Line(minPoint.X + breakRadius * i, minPoint.Y, minPoint.X + breakRadius * i, maxPoint.Y);
            //                    dimTextLinearPathList.Add(newVertical);
            //                }
            //            }


            //            dimTextLinearPathList.Add(newRec);
            //        }
            //        List<Entity> leaderTextLinearPathList = new List<Entity>();
            //        foreach (Entity eachEntity in leaderTextList)
            //        {
            //            eachEntity.Regen(new RegenParams(0, singleModel));
            //            Point3D[] textBoxSize = eachEntity.Vertices;
            //            LinearPath newRec = new LinearPath(textBoxSize);
            //            newRec.Regen(new RegenParams(0, singleModel));

            //            Point3D minPoint = GetMinPointXY(textBoxSize);
            //            Point3D maxPoint = GetMaxPointXY(textBoxSize);
            //            if (newRec.BoxSize.Y > breakRadius)
            //            {
            //                double quotient = Math.Truncate(newRec.BoxSize.Y / breakRadius);
            //                double quotientcc = newRec.BoxSize.Y / quotient;

            //                for (int i = 1; i <= quotient; i++)
            //                {
            //                    Line newHorizontal = new Line(minPoint.X, minPoint.Y + breakRadius * i, maxPoint.X, minPoint.Y + breakRadius * i);
            //                    dimTextLinearPathList.Add(newHorizontal);
            //                }
            //            }
            //            if (newRec.BoxSize.X > breakRadius)
            //            {
            //                double quotient = Math.Truncate(newRec.BoxSize.X / breakRadius);
            //                double quotientcc = newRec.BoxSize.X / quotient;

            //                for (int i = 1; i <= quotient; i++)
            //                {
            //                    Line newVertical = new Line(minPoint.X + breakRadius * i, minPoint.Y, minPoint.X + breakRadius * i, maxPoint.Y);
            //                    dimTextLinearPathList.Add(newVertical);
            //                }
            //            }

            //            leaderTextLinearPathList.Add(newRec);
            //        }

            //        // All list
            //        Dictionary<string, List<Entity>> allEntityDic = new Dictionary<string, List<Entity>>();
            //        allEntityDic.Add(CommonGlobal.OutLine, outlineList);
            //        allEntityDic.Add(CommonGlobal.CenterLine, centerlineList);
            //        allEntityDic.Add(CommonGlobal.DimLine, dimlineList);
            //        allEntityDic.Add(CommonGlobal.DimText, dimTextLinearPathList); // text 대신 leader path
            //        allEntityDic.Add(CommonGlobal.DimLineExt, dimlineExtList);
            //        allEntityDic.Add(CommonGlobal.LeaderLine, leaderlineList);
            //        allEntityDic.Add(CommonGlobal.LeaderText, leaderTextLinearPathList); // text 대신 leader path
            //        allEntityDic.Add(CommonGlobal.NozzleLine, nozzlelineList);
            //        allEntityDic.Add(CommonGlobal.NozzleMark, nozzleMarkList);

            //        // Break





            //        Dictionary<string, List<Entity>> allEntityNewDic = new Dictionary<string, List<Entity>>();

            //        // Line Break
            //        List<string> allEntityName = allEntityDic.Keys.ToList();
            //        List<List<Entity>> allEntityList = allEntityDic.Values.ToList();
            //        for (int i = 0; i < allEntityName.Count; i++)
            //        {
            //            string eachTarget = allEntityName[i];
            //            List<Entity> eachTargetList = allEntityList[i];
            //            List<Entity> newEachTargetList = new List<Entity>();

            //            // 문자 제외
            //            if (!eachTarget.Contains("text"))
            //            {
            //                foreach (ICurve targetLine in eachTargetList)
            //                {
            //                    // 1. 겹치는 점 찾기
            //                    Dictionary<Point3D, int> intersectPointDic = new Dictionary<Point3D, int>();
            //                    for (int j = 0; j < allEntityName.Count; j++)
            //                    {
            //                        string eachReference = allEntityName[j];
            //                        // 끊기 여부
            //                        if (autoDimService.GetDimensionBreak(eachTarget, eachReference))
            //                        {
            //                            List<Entity> eachReferenceList = allEntityList[j];

            //                            foreach (ICurve refereceLine in eachReferenceList)
            //                            {
            //                                Point3D[] eachPointArray = targetLine.IntersectWith(refereceLine);
            //                                foreach (Point3D eachPoint in eachPointArray)
            //                                {
            //                                    if (!intersectPointDic.ContainsKey(eachPoint))
            //                                    {
            //                                        intersectPointDic.Add(eachPoint, j);
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }

            //                    // 2. 겹치는 점에서 원 그리고 겹치는 선 기준으로 나누기
            //                    if (intersectPointDic.Count > 0)
            //                    {
            //                        List<Point3D> splitPointList = new List<Point3D>();
            //                        List<Point3D> intersectPointList = intersectPointDic.Keys.ToList();
            //                        foreach (Point3D eachPoint in intersectPointList)
            //                        {
            //                            Circle newCircle = new Circle(eachPoint, breakRadius);// 반지름 : breakRadius
            //                            Point3D[] intersectCircleArray = Curve.Intersection(newCircle, targetLine);
            //                            splitPointList.AddRange(intersectCircleArray);

            //                        }

            //                        // 3. 자르기
            //                        if (splitPointList.Count > 0)
            //                        {
            //                            ICurve[] splitLine;
            //                            if (targetLine.SplitBy(splitPointList, out splitLine))
            //                            {
            //                                // 3. 원에 가까운 것 버리기
            //                                foreach (ICurve eachICurve in splitLine)
            //                                {
            //                                    bool addLine = true;
            //                                    Point3D minPoint = Point3D.MidPoint(eachICurve.StartPoint, eachICurve.EndPoint);
            //                                    foreach (Point3D eachIntersect in intersectPointList)
            //                                    {
            //                                        // 3. 버리는 조건
            //                                        if (Point3D.Distance(minPoint, eachIntersect) < breakRadius)
            //                                        {
            //                                            addLine = false;
            //                                            break;
            //                                        }
            //                                    }

            //                                    // 3. 추가
            //                                    if (addLine)
            //                                    {
            //                                        newEachTargetList.Add(eachICurve as Entity);
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        newEachTargetList.Add(targetLine as Entity);
            //                    }
            //                }
            //                allEntityNewDic.Add(eachTarget, newEachTargetList);
            //            }
            //        }

            //        // 문자 추가
            //        singleModel.Entities.AddRange(dimTextList, Color.Blue);
            //        singleModel.Entities.AddRange(leaderTextList, Color.Green);

            //        // Line Break 추가
            //        List<string> allEntityNewName = allEntityNewDic.Keys.ToList();
            //        List<List<Entity>> allEntityNewList = allEntityNewDic.Values.ToList();
            //        for (int i = 0; i < allEntityNewName.Count; i++)
            //        {
            //            List<Entity> eachEntityList = allEntityNewList[i];
            //            string eachEntityName = allEntityNewName[i];
            //            switch (eachEntityName)
            //            {
            //                case "outline":
            //                    singleModel.Entities.AddRange(eachEntityList, Color.Black);
            //                    break;
            //                case "centerline":
            //                    singleModel.Entities.AddRange(eachEntityList, Color.Red);
            //                    break;
            //                case "dimline":
            //                    singleModel.Entities.AddRange(eachEntityList, Color.Blue);
            //                    break;
            //                case "dimtext":
            //                    singleModel.Entities.AddRange(eachEntityList, Color.Blue);
            //                    break;
            //                case "dimlineext":
            //                    singleModel.Entities.AddRange(eachEntityList, Color.BlueViolet);
            //                    break;
            //                case "leaderline":
            //                    singleModel.Entities.AddRange(eachEntityList, Color.Green);
            //                    break;
            //                case "leadertext":
            //                    singleModel.Entities.AddRange(eachEntityList, Color.Green);
            //                    break;
            //                case "nozzleline":
            //                    singleModel.Entities.AddRange(eachEntityList, Color.Gray);
            //                    break;
            //                case "nozzlemark":
            //                    singleModel.Entities.AddRange(eachEntityList, Color.Gray);
            //                    break;
            //            }

            //        }

            //        //singleModel.Entities.AddRange(dimTextLinearPathList, Color.YellowGreen);


            //        //singleModel.Entities.AddRange(outlineList, Color.Black);
            //        //singleModel.Entities.AddRange(centerlineList, Color.Red);
            //        //singleModel.Entities.AddRange(dimlineList, Color.Blue);
            //        //singleModel.Entities.AddRange(dimTextList, Color.Blue);
            //        //singleModel.Entities.AddRange(dimlineExtList, Color.BlueViolet);
            //        //singleModel.Entities.AddRange(leaderlineList, Color.Green);
            //        //singleModel.Entities.AddRange(leaderTextList, Color.Green);
            //        //singleModel.Entities.AddRange(nozzlelineList, Color.Gray);
            //        //singleModel.Entities.AddRange(nozzleMarkList, Color.Gray);


            //        //singleModel.Entities.Add(newCircle1, Color.Red);
            //        //singleModel.Entities.Add(newCircle2, Color.Red);

            //        //singleModel.Entities.Add( aa1,Color.Blue);
            //        //Text cc = new Text(new Point3D(0, -310, 0), "Dimension Line ABCDEFGabcdefg12345", 50);
            //        //cc.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
            //        //singleModel.Entities.Add( cc, Color.Gray);

            //    }
            //    #endregion

            //    #region Arrow
            //    if (false)
            //    {


            //        //Text dimTextVertical = new Text(new Point3D(20, 40), "asdf", 20);
            //        //dimTextVertical.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
            //        Text dimTextVertical2 = new Text(new Point3D(40, -20), "asdf", 20);
            //        dimTextVertical2.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);





            //        double breakRadius1 = 10;
            //        //Triangle newTri=new Triangle(new Point3D(20,20), new Point3D(40,40), new Point3D(0,40));
            //        Triangle newTri = new Triangle(new Point3D(20, 20), new Point3D(40, 0), new Point3D(40, 40));
            //        newTri.ColorMethod = colorMethodType.byEntity;
            //        newTri.Color = Color.Blue;

            //        //singleModel.Entities.Add(newTri);
            //        //newTri.Regen(new RegenParams(0, singleModel));

            //        //Point3D[] ss = new Point3D[3] { new Point3D(20, 20), new Point3D(40, 0), new Point3D(40, 40) };

            //        //newTri.Regen(new RegenParams(0, singleModel));
            //        List<Point3D> newTriPath = new List<Point3D>();
            //        newTriPath.AddRange(newTri.Vertices);

            //        Point3D centerPoint = new Point3D();

            //        if (newTriPath[1].X == newTriPath[2].X)
            //        {
            //            // Left Right
            //            double newDistance = Point3D.Distance(newTriPath[1], newTriPath[2]);
            //            double minValue = Math.Min(newTriPath[1].Y, newTriPath[2].Y);
            //            double quotient = Math.Truncate(newDistance / breakRadius1);

            //            centerPoint.Y = newTriPath[0].Y;
            //            if (newTriPath[0].X < newTriPath[1].X)
            //            {
            //                centerPoint.X = newTriPath[0].X + 1;
            //            }
            //            else
            //            {
            //                centerPoint.X = newTriPath[0].X - 1;
            //            }

            //            for (int i = 1; i <= quotient; i++)
            //            {
            //                Line newLine = new Line(centerPoint, new Point3D(newTriPath[1].X, minValue + breakRadius1 * i));
            //                singleModel.Entities.Add(newLine);
            //            }
            //        }
            //        else if (newTriPath[1].Y == newTriPath[2].Y)
            //        {
            //            // Top Bottom
            //            double newDistance = Point3D.Distance(newTriPath[1], newTriPath[2]);
            //            double minValue = Math.Min(newTriPath[1].X, newTriPath[2].X);
            //            double quotient = Math.Truncate(newDistance / breakRadius1);

            //            centerPoint.X = newTriPath[0].X;
            //            if (newTriPath[0].Y < newTriPath[1].Y)
            //            {
            //                centerPoint.Y = newTriPath[0].Y + 1;
            //            }
            //            else
            //            {
            //                centerPoint.Y = newTriPath[0].Y - 1;
            //            }

            //            for (int i = 1; i <= quotient; i++)
            //            {
            //                Line newLine = new Line(centerPoint, new Point3D(minValue + breakRadius1 * i, newTriPath[1].Y));
            //                singleModel.Entities.Add(newLine);
            //            }
            //        }

            //        newTriPath = new List<Point3D>();
            //        newTriPath.Add(centerPoint);
            //        newTriPath.Add(newTri.Vertices[1]);
            //        newTriPath.Add(newTri.Vertices[2]);
            //        newTriPath.Add(centerPoint);
            //        LinearPath newLinearPath = new LinearPath(newTriPath.ToArray());
            //        singleModel.Entities.Add(newLinearPath);

            //        singleModel.Entities.Add(newLinearPath);
            //    }
            //    #endregion
            //}
            //else
            //{

            //    #region Dimension Sample
            //    if (false)
            //    {
            //        singleModel.Entities.AddRange(GetDimension(new Point3D(10, 10), new Point3D(100, 50), "TOP", 100));
            //        singleModel.Entities.AddRange(GetDimension(new Point3D(10, 10), new Point3D(100, 50), "BOTTOM", 100));
            //        singleModel.Entities.AddRange(GetDimension(new Point3D(-10, 10), new Point3D(-50, 100), "LEFT", 100));
            //        singleModel.Entities.AddRange(GetDimension(new Point3D(-10, 10), new Point3D(-50, 100), "RIGHT", 100));

            //        singleModel.Entities.Add(new LinearPath(new Point3D[5] { new Point3D(10, 10), new Point3D(-10, 10), new Point3D(-10, -10), new Point3D(10, -10), new Point3D(10, 10) }));
            //    }
            //    #endregion



            //    #region Line Break : ing
            //    if (false)
            //    {


            //        AutomationDimensionService autoDimService = new AutomationDimensionService();

            //        // Reference Circle
            //        double breakRadius = 10;

            //        List<Entity> outlineList = new List<Entity>();
            //        List<Entity> centerlineList = new List<Entity>();
            //        List<Entity> dimlineList = new List<Entity>();
            //        List<Entity> dimTextList = new List<Entity>();
            //        List<Entity> dimlineExtList = new List<Entity>();
            //        List<Entity> leaderlineList = new List<Entity>();
            //        List<Entity> leaderTextList = new List<Entity>();
            //        List<Entity> nozzlelineList = new List<Entity>();
            //        List<Entity> nozzleMarkList = new List<Entity>();

            //        // 세로
            //        outlineList.Add(new Line(new Point3D(10, -20, 0), new Point3D(10, 1110, 0)));
            //        centerlineList.Add(new Line(new Point3D(110, -20, 0), new Point3D(110, 1110, 0)));
            //        dimlineList.Add(new Line(new Point3D(210, -20, 0), new Point3D(210, 1110, 0)));
            //        Text dimText01 = new Text(new Point3D(0, -310, 0), "Dimension Line ABCDEFGabcdefg12345", 50);

            //        dimText01.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
            //        dimTextList.Add(dimText01);
            //        dimlineExtList.Add(new Line(new Point3D(410, -20, 0), new Point3D(410, 1110, 0)));
            //        leaderlineList.Add(new Line(new Point3D(510, -20, 0), new Point3D(510, 1110, 0)));
            //        Text leaderText01 = new Text(new Point3D(0, -610, 0), "Leader Line ABCDEFGabcdefg12345", 50);
            //        leaderText01.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
            //        leaderTextList.Add(leaderText01);
            //        nozzlelineList.Add(new Line(new Point3D(710, -20, 0), new Point3D(710, 1110, 0)));
            //        nozzleMarkList.Add(new Line(new Point3D(810, -20, 0), new Point3D(810, 1110, 0)));

            //        // 가로
            //        outlineList.Add(new Line(new Point3D(-20, 10, 0), new Point3D(1110, 10, 0)));
            //        centerlineList.Add(new Line(new Point3D(-20, 110, 0), new Point3D(1110, 110, 0)));
            //        dimlineList.Add(new Line(new Point3D(-20, 210, 0), new Point3D(1110, 210, 0)));
            //        Text dimText02 = new Text(new Point3D(-20, 310, 0), "Dimension Line ABCDEFGabcdefg12345", 50);
            //        dimTextList.Add(dimText02);
            //        dimlineExtList.Add(new Line(new Point3D(-20, 410, 0), new Point3D(1110, 410, 0)));
            //        leaderlineList.Add(new Line(new Point3D(-20, 510, 0), new Point3D(1110, 510, 0)));
            //        Text leaderText02 = new Text(new Point3D(-20, 610, 0), "Leader Line ABCDEFGabcdefg12345", 50);
            //        leaderTextList.Add(leaderText02);
            //        nozzlelineList.Add(new Line(new Point3D(-20, 710, 0), new Point3D(1110, 710, 0)));
            //        nozzleMarkList.Add(new Line(new Point3D(-20, 810, 0), new Point3D(1110, 810, 0)));


            //        // Text to Box : 수정 최소단위 사각형으로
            //        List<Entity> dimTextLinearPathList = new List<Entity>();
            //        foreach (Entity eachEntity in dimTextList)
            //        {
            //            eachEntity.Regen(new RegenParams(0, singleModel));
            //            Point3D[] textBoxSize = eachEntity.Vertices;
            //            LinearPath newRec = new LinearPath(textBoxSize);
            //            newRec.Regen(new RegenParams(0, singleModel));

            //            Point3D minPoint = GetMinPointXY(textBoxSize);
            //            Point3D maxPoint = GetMaxPointXY(textBoxSize);
            //            if (newRec.BoxSize.Y > breakRadius)
            //            {
            //                double quotient = Math.Truncate(newRec.BoxSize.Y / breakRadius);
            //                double quotientcc = newRec.BoxSize.Y / quotient;

            //                for (int i = 1; i <= quotient; i++)
            //                {
            //                    Line newHorizontal = new Line(minPoint.X, minPoint.Y + breakRadius * i, maxPoint.X, minPoint.Y + breakRadius * i);
            //                    dimTextLinearPathList.Add(newHorizontal);
            //                }
            //            }
            //            if (newRec.BoxSize.X > breakRadius)
            //            {
            //                double quotient = Math.Truncate(newRec.BoxSize.X / breakRadius);
            //                double quotientcc = newRec.BoxSize.X / quotient;

            //                for (int i = 1; i <= quotient; i++)
            //                {
            //                    Line newVertical = new Line(minPoint.X + breakRadius * i, minPoint.Y, minPoint.X + breakRadius * i, maxPoint.Y);
            //                    dimTextLinearPathList.Add(newVertical);
            //                }
            //            }


            //            dimTextLinearPathList.Add(newRec);
            //        }
            //        List<Entity> leaderTextLinearPathList = new List<Entity>();
            //        foreach (Entity eachEntity in leaderTextList)
            //        {
            //            eachEntity.Regen(new RegenParams(0, singleModel));
            //            Point3D[] textBoxSize = eachEntity.Vertices;
            //            LinearPath newRec = new LinearPath(textBoxSize);
            //            newRec.Regen(new RegenParams(0, singleModel));

            //            Point3D minPoint = GetMinPointXY(textBoxSize);
            //            Point3D maxPoint = GetMaxPointXY(textBoxSize);
            //            if (newRec.BoxSize.Y > breakRadius)
            //            {
            //                double quotient = Math.Truncate(newRec.BoxSize.Y / breakRadius);
            //                double quotientcc = newRec.BoxSize.Y / quotient;

            //                for (int i = 1; i <= quotient; i++)
            //                {
            //                    Line newHorizontal = new Line(minPoint.X, minPoint.Y + breakRadius * i, maxPoint.X, minPoint.Y + breakRadius * i);
            //                    dimTextLinearPathList.Add(newHorizontal);
            //                }
            //            }
            //            if (newRec.BoxSize.X > breakRadius)
            //            {
            //                double quotient = Math.Truncate(newRec.BoxSize.X / breakRadius);
            //                double quotientcc = newRec.BoxSize.X / quotient;

            //                for (int i = 1; i <= quotient; i++)
            //                {
            //                    Line newVertical = new Line(minPoint.X + breakRadius * i, minPoint.Y, minPoint.X + breakRadius * i, maxPoint.Y);
            //                    dimTextLinearPathList.Add(newVertical);
            //                }
            //            }

            //            leaderTextLinearPathList.Add(newRec);
            //        }

            //        // All list
            //        Dictionary<string, List<Entity>> allEntityDic = new Dictionary<string, List<Entity>>();
            //        allEntityDic.Add(CommonGlobal.OutLine, outlineList);
            //        allEntityDic.Add(CommonGlobal.CenterLine, centerlineList);
            //        allEntityDic.Add(CommonGlobal.DimLine, dimlineList);
            //        allEntityDic.Add(CommonGlobal.DimText, dimTextLinearPathList); // text 대신 leader path
            //        allEntityDic.Add(CommonGlobal.DimLineExt, dimlineExtList);
            //        allEntityDic.Add(CommonGlobal.LeaderLine, leaderlineList);
            //        allEntityDic.Add(CommonGlobal.LeaderText, leaderTextLinearPathList); // text 대신 leader path
            //        allEntityDic.Add(CommonGlobal.NozzleLine, nozzlelineList);
            //        allEntityDic.Add(CommonGlobal.NozzleMark, nozzleMarkList);

            //        // Break





            //        Dictionary<string, List<Entity>> allEntityNewDic = new Dictionary<string, List<Entity>>();

            //        // Line Break
            //        List<string> allEntityName = allEntityDic.Keys.ToList();
            //        List<List<Entity>> allEntityList = allEntityDic.Values.ToList();
            //        for (int i = 0; i < allEntityName.Count; i++)
            //        {
            //            string eachTarget = allEntityName[i];
            //            List<Entity> eachTargetList = allEntityList[i];
            //            List<Entity> newEachTargetList = new List<Entity>();

            //            // 문자 제외
            //            if (!eachTarget.Contains("text"))
            //            {
            //                foreach (ICurve targetLine in eachTargetList)
            //                {
            //                    // 1. 겹치는 점 찾기
            //                    Dictionary<Point3D, int> intersectPointDic = new Dictionary<Point3D, int>();
            //                    for (int j = 0; j < allEntityName.Count; j++)
            //                    {
            //                        string eachReference = allEntityName[j];
            //                        // 끊기 여부
            //                        if (autoDimService.GetDimensionBreak(eachTarget, eachReference))
            //                        {
            //                            List<Entity> eachReferenceList = allEntityList[j];

            //                            foreach (ICurve refereceLine in eachReferenceList)
            //                            {
            //                                Point3D[] eachPointArray = targetLine.IntersectWith(refereceLine);
            //                                foreach (Point3D eachPoint in eachPointArray)
            //                                {
            //                                    if (!intersectPointDic.ContainsKey(eachPoint))
            //                                    {
            //                                        intersectPointDic.Add(eachPoint, j);
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }

            //                    // 2. 겹치는 점에서 원 그리고 겹치는 선 기준으로 나누기
            //                    if (intersectPointDic.Count > 0)
            //                    {
            //                        List<Point3D> splitPointList = new List<Point3D>();
            //                        List<Point3D> intersectPointList = intersectPointDic.Keys.ToList();
            //                        foreach (Point3D eachPoint in intersectPointList)
            //                        {
            //                            Circle newCircle = new Circle(eachPoint, breakRadius);// 반지름 : breakRadius
            //                            Point3D[] intersectCircleArray = Curve.Intersection(newCircle, targetLine);
            //                            splitPointList.AddRange(intersectCircleArray);

            //                        }

            //                        // 3. 자르기
            //                        if (splitPointList.Count > 0)
            //                        {
            //                            ICurve[] splitLine;
            //                            if (targetLine.SplitBy(splitPointList, out splitLine))
            //                            {
            //                                // 3. 원에 가까운 것 버리기
            //                                foreach (ICurve eachICurve in splitLine)
            //                                {
            //                                    bool addLine = true;
            //                                    Point3D minPoint = Point3D.MidPoint(eachICurve.StartPoint, eachICurve.EndPoint);
            //                                    foreach (Point3D eachIntersect in intersectPointList)
            //                                    {
            //                                        // 3. 버리는 조건
            //                                        if (Point3D.Distance(minPoint, eachIntersect) < breakRadius)
            //                                        {
            //                                            addLine = false;
            //                                            break;
            //                                        }
            //                                    }

            //                                    // 3. 추가
            //                                    if (addLine)
            //                                    {
            //                                        newEachTargetList.Add(eachICurve as Entity);
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        newEachTargetList.Add(targetLine as Entity);
            //                    }
            //                }
            //                allEntityNewDic.Add(eachTarget, newEachTargetList);
            //            }
            //        }

            //        // 문자 추가
            //        singleModel.Entities.AddRange(dimTextList, Color.Blue);
            //        singleModel.Entities.AddRange(leaderTextList, Color.Green);

            //        // Line Break 추가
            //        List<string> allEntityNewName = allEntityNewDic.Keys.ToList();
            //        List<List<Entity>> allEntityNewList = allEntityNewDic.Values.ToList();
            //        for (int i = 0; i < allEntityNewName.Count; i++)
            //        {
            //            List<Entity> eachEntityList = allEntityNewList[i];
            //            string eachEntityName = allEntityNewName[i];
            //            switch (eachEntityName)
            //            {
            //                case "outline":
            //                    singleModel.Entities.AddRange(eachEntityList, Color.Black);
            //                    break;
            //                case "centerline":
            //                    singleModel.Entities.AddRange(eachEntityList, Color.Red);
            //                    break;
            //                case "dimline":
            //                    singleModel.Entities.AddRange(eachEntityList, Color.Blue);
            //                    break;
            //                case "dimtext":
            //                    singleModel.Entities.AddRange(eachEntityList, Color.Blue);
            //                    break;
            //                case "dimlineext":
            //                    singleModel.Entities.AddRange(eachEntityList, Color.BlueViolet);
            //                    break;
            //                case "leaderline":
            //                    singleModel.Entities.AddRange(eachEntityList, Color.Green);
            //                    break;
            //                case "leadertext":
            //                    singleModel.Entities.AddRange(eachEntityList, Color.Green);
            //                    break;
            //                case "nozzleline":
            //                    singleModel.Entities.AddRange(eachEntityList, Color.Gray);
            //                    break;
            //                case "nozzlemark":
            //                    singleModel.Entities.AddRange(eachEntityList, Color.Gray);
            //                    break;
            //            }

            //        }

            //        //singleModel.Entities.AddRange(dimTextLinearPathList, Color.YellowGreen);


            //        //singleModel.Entities.AddRange(outlineList, Color.Black);
            //        //singleModel.Entities.AddRange(centerlineList, Color.Red);
            //        //singleModel.Entities.AddRange(dimlineList, Color.Blue);
            //        //singleModel.Entities.AddRange(dimTextList, Color.Blue);
            //        //singleModel.Entities.AddRange(dimlineExtList, Color.BlueViolet);
            //        //singleModel.Entities.AddRange(leaderlineList, Color.Green);
            //        //singleModel.Entities.AddRange(leaderTextList, Color.Green);
            //        //singleModel.Entities.AddRange(nozzlelineList, Color.Gray);
            //        //singleModel.Entities.AddRange(nozzleMarkList, Color.Gray);


            //        //singleModel.Entities.Add(newCircle1, Color.Red);
            //        //singleModel.Entities.Add(newCircle2, Color.Red);

            //        //singleModel.Entities.Add( aa1,Color.Blue);
            //        //Text cc = new Text(new Point3D(0, -310, 0), "Dimension Line ABCDEFGabcdefg12345", 50);
            //        //cc.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
            //        //singleModel.Entities.Add( cc, Color.Gray);

            //    }
            //    #endregion


            //    #region Leader
            //    if (false)
            //    {
            //        List<string> leaderText = new List<string>();
            //        leaderText.Add("B1AAAAaaaaaM");
            //        leaderText.Add("B2AAAAaaasdfaaaM");
            //        leaderText.Add("B3AAAAaaaasdfaaM");
            //        leaderText.Add("B4AAAAaaM");


            //        List<Entity> ccc = GetLeader(new Point3D(1600, 10), 60, "topright", leaderText, leaderText, singleModel);
            //        singleModel.Entities.AddRange(ccc);

            //        List<Entity> cccc = GetLeader(new Point3D(1600, 10), 60, "bottomright", leaderText, leaderText, singleModel);
            //        singleModel.Entities.AddRange(cccc);

            //        List<Entity> ccccc = GetLeader(new Point3D(1600, 10), 60, "topleft", leaderText, leaderText, singleModel);
            //        singleModel.Entities.AddRange(ccccc);

            //        List<Entity> cccccc = GetLeader(new Point3D(1600, 10), 60, "bottomleft", leaderText, leaderText, singleModel);
            //        singleModel.Entities.AddRange(cccccc);


            //    }

            //    #endregion

            //    #region offset
            //    if (false)
            //    {
            //        Line newLine = new Line(new Point3D(0, 0), new Point3D(30, 30));

            //        Line customLine = (Line)newLine.Clone();
            //        Plane pl1 = Plane.YZ;
            //        pl1.Origin.X = customLine.EndPoint.X;
            //        pl1.Origin.Y = customLine.EndPoint.Y;
            //        Mirror customMirror = new Mirror(pl1);
            //        customLine.TransformBy(customMirror);

            //        Line newLine2 = new Line(new Point3D(40, 0), new Point3D(40, 30));
            //        newLine2.Rotate(UtilityEx.DegToRad(80), Vector3D.AxisZ, newLine2.StartPoint);
            //        Line newLine3 = (Line)newLine2.Clone();
            //        newLine3.Rotate(-UtilityEx.DegToRad(80), Vector3D.AxisZ, newLine2.StartPoint);
            //        Line customLine2 = (Line)newLine2.Clone();
            //        Plane pl2 = Plane.YZ;
            //        pl2.Origin.X = customLine2.StartPoint.X;
            //        pl2.Origin.Y = customLine2.StartPoint.Y;
            //        Mirror customMirror2 = new Mirror(pl2);
            //        customLine2.TransformBy(customMirror2);

            //        singleModel.Entities.Add(customLine, Color.Black);
            //        singleModel.Entities.Add(newLine, Color.Red);

            //        singleModel.Entities.Add(customLine2, Color.Blue);
            //        singleModel.Entities.Add(newLine2, Color.Green);
            //        singleModel.Entities.Add(newLine3, Color.Gray);



            //    }
            //    #endregion

            //    #region circle
            //    if (false)
            //    {
            //        Circle newCir1 = new Circle(new Point3D(40, 40), 45);
            //        singleModel.Entities.Add(newCir1, Color.Green);

            //        Circle newCir2 = new Circle(new Point3D(40, 40), 23);
            //        singleModel.Entities.Add(newCir2, Color.Green);

            //        Point3D newCenter = new Point3D();
            //        newCenter.X = 40;
            //        newCenter.Y = 40;
            //        double boltHeight = 23;
            //        double boltWidth = 45;

            //        Point3D newCenterLeft = new Point3D();
            //        newCenterLeft.X = newCenter.X - boltWidth + boltHeight;
            //        newCenterLeft.Y = newCenter.Y;
            //        Point3D newCenterRight = new Point3D();
            //        newCenterRight.X = newCenter.X + boltWidth - boltHeight;
            //        newCenterRight.Y = newCenter.Y;

            //        Arc arcarc1 = new Arc(Plane.XY, newCenterLeft, boltHeight, new Point3D(newCenterLeft.X, newCenterLeft.Y+ boltHeight), new Point3D(newCenterLeft.X,newCenterLeft.Y-boltHeight),false);
            //        Arc arcarc2 = new Arc(Plane.XY, newCenterRight, boltHeight, new Point3D(newCenterRight.X, newCenterRight.Y + boltHeight), new Point3D(newCenterRight.X, newCenterRight.Y - boltHeight), true);
            //        Line ll1 = new Line(arcarc1.StartPoint, arcarc2.EndPoint);
            //        Line ll2 = new Line(arcarc1.EndPoint, arcarc2.StartPoint);

            //        CompositeCurve compositeCurve = new CompositeCurve(arcarc1, arcarc2, ll1, ll2);
            //        singleModel.Entities.Add(compositeCurve, Color.Gray);

            //        CompositeCurve compositeCurve2 = (CompositeCurve)compositeCurve.Clone();
            //        compositeCurve2.Rotate(Math.Atan2(1,120), Vector3D.AxisZ);
            //        singleModel.Entities.Add(compositeCurve2, Color.Red);



            //    }
            //    #endregion

            //    #region Group roate
            //    if (true)
            //    {
            //        //Solid newSolid = new Solid(,);

            //        //Triangle tri1 = new Triangle(new Point3D(selPoint1.X, textCenter.Y), new Point3D(selPoint1.X + selArrowHeight * 3, textCenter.Y + selArrowHeight / 2), new Point3D(selPoint1.X + selArrowHeight * 3, textCenter.Y - selArrowHeight / 2));


            //        //singleModel.Entities.Add(newSolid, Color.Green);


            //        Line c1 = new Line(new Point3D(10, 20, 0), new Point3D(10, 100, 0));
            //        Line c2 = new Line(new Point3D(20, 20, 0), new Point3D(20, 100, 0));

            //        Line c11 = (Line)c1.Clone();
            //        Line c22 = (Line)c2.Clone();

            //        c11.Rotate(UtilityEx.DegToRad(-90), Vector3D.AxisZ, new Point3D(15, 20));
            //        c22.Rotate(UtilityEx.DegToRad(-90), Vector3D.AxisZ, new Point3D(15, 20));

            //        singleModel.Entities.Add(c1);
            //        singleModel.Entities.Add(c2);
            //        singleModel.Entities.Add(c11);
            //        singleModel.Entities.Add(c22);
            //    }
            //    #endregion



            //    singleModel.Entities.Regen();
            //    singleModel.ZoomFit();
            //    singleModel.SetView(viewType.Top);
            //}
            #endregion


            #region old Sample
            if (visibleFalse)
            {
                //Solid newSolid = new Solid(,);

                //Triangle tri1 = new Triangle(new Point3D(selPoint1.X, textCenter.Y), new Point3D(selPoint1.X + selArrowHeight * 3, textCenter.Y + selArrowHeight / 2), new Point3D(selPoint1.X + selArrowHeight * 3, textCenter.Y - selArrowHeight / 2));


                //singleModel.Entities.Add(newSolid, Color.Green);


                //Line c1 = new Line(new Point3D(10, 20, 0), new Point3D(10000, 1000, 0));
                //Line c2 = new Line(new Point3D(2000, 20, 0), new Point3D(2000, 1000, 0));

                //Line c11 = (Line)c1.Clone();
                //Line c22 = (Line)c2.Clone();

                //Point3D[] ins= c2.IntersectWith(c1);

                //Line c222 = new Line(ins[0], new Point3D(2000, 1000, 0));

                //singleModel.Entities.Add(c1);
                //singleModel.Entities.Add(c222);

                // 나옴
                Line c2ss = new Line(new Point3D(0, 3, 0), new Point3D(0, 50, 0));
                //singleModel.Entities.Add(c2ss);

                Line c1ss = new Line(new Point3D(0, 1, 0), new Point3D(100, 300, 0));
                //singleModel.Entities.Add(c1ss);

                Line c3ss = new Line(new Point3D(-100, 100, 0), new Point3D(100, 100, 0));
                //singleModel.Entities.Add(c3ss);

                Line c1ss1 = new Line(new Point3D(0, 0, 0), new Point3D(50, 200, 0));
                //singleModel.Entities.Add(c1ss1,Color.Blue);
                Line c1ss2 = new Line(new Point3D(0, 10, 0), new Point3D(50, 10, 0));
                //singleModel.Entities.Add(c1ss2);
                Circle newCir = new Circle(0, 0, 0, 10);
                //singleModel.Entities.Add(newCir);
                //singleModel.Entities.Add(c3ss, Color.Blue);


                //Line c3ss = new Line(new Point3D(0, 0, 0), new Point3D(0, 10000, 0));
                //singleModel.Entities.Add(c3ss, Color.Red);

                //singleModel.Entities.Add(c1);
                //singleModel.Entities.Add(c2);
                //singleModel.Entities.Add(c11);
                //singleModel.Entities.Add(c22);

                //LinearDim ddd = new LinearDim(Plane.XY, new Point2D(10, 10), new Point2D(1000, 500), new Point2D(500, 500), 200);

                //singleModel.Entities.Add(ddd);

                // Triangle
                //Triangle t = new Triangle(5, 5, 0, 10, 5, 0, 7.5, 10, 0);
                //t.ColorMethod = colorMethodType.byEntity;
                //t.Color = Color.Blue;
                //CustomRenderedTriangle customArrow = new CustomRenderedTriangle(t);
                //singleModel.Entities.Add(customArrow);

            }

            if (visibleFalse)
            {
                //HatchRegion hatch = new HatchRegion(sese);
                //HatchRegion hatch = new HatchRegion(region.ContourList);
                //hatch.HatchName = "SOLID";
                //hatch.Color = Color.Red;

                //CustomRenderedHatch hhh = new CustomRenderedHatch(hatch);

                //singleModel.Entities.Add(hhh);

                // Mesh
                // Mesh cc = new Mesh(4, 2, Mesh.natureType.ColorPlain);
                //cc.Color = Color.Red;
                //singleModel.Entities.Add(cc);

                //Mesh dd=Mesh.CreateArrow(40, 10, 4, 5, 5, Mesh.natureType.ColorPlain);
                //dd.Color = Color.Red;

                //CustomRenderedMesh cuMesh = new CustomRenderedMesh(Mesh.CreateSphere(10, 10, 10));

                //singleModel.Entities.Add(cuMesh);


                //Arc Test
                //List<Entity> testLIne = new List<Entity>();

                //Arc tesArc = new Arc(Plane.XY, new Point3D(50, 30), new Point3D(100, 60), new Point3D(150, 30),true);
                //styleService.SetLayer(ref tesArc, layerService.LayerCenterLine);
                //testLIne.Add(tesArc);
                //testLIne.Add( new Line(0, 0, 150, 0));
                //testLIne.Add(new Line(0, 30, 150, 30));
                //testLIne.Add(new Line(0, 60, 150, 60));

                //testLIne.Add(new Line(50, 0, 50, 60));
                //testLIne.Add(new Line(100, 0, 100, 60));
                //testLIne.Add(new Line(150, 0, 150, 60));

                //singleModel.Entities.AddRange(testLIne);
            }




            if (visibleFalse)
            {

                string sss = string.Format("aaa{0}\"_{1}", 1, 2);

                DrawNozzleBlockService nozzleDraw = new DrawNozzleBlockService(null);

                Line newCen01 = new Line(0, 100, 200, 100);
                Line newCen02 = new Line(100, 0, 100, 200);
                singleModel.Entities.Add(newCen01);
                singleModel.Entities.Add(newCen02);

                // Elbow
                ElbowModel newElbow = new ElbowModel() { OD = "33.4", LRA = "38.1", LRB = "22.225", SRD = "25.4" };
                Point3D refPoint1 = new Point3D(100, 100, 0);


                List<Point3D> outputPointList = new List<Point3D>();
                List<Entity> elbowList0 = nozzleDraw.DrawReference_Nozzle_Elbow(out outputPointList, refPoint1, 1, newElbow, "lr", "45", Utility.DegToRad(45), new DrawCenterLineModel() { scaleValue = 90, zeroEx = true, oneEx = true });
                List<Entity> elbowList1 = nozzleDraw.DrawReference_Nozzle_Elbow(out outputPointList, outputPointList[0], 1, newElbow, "lr", "45", Utility.DegToRad(90), new DrawCenterLineModel() { scaleValue = 90, zeroEx = true, oneEx = true });
                singleModel.Entities.AddRange(elbowList0);
                singleModel.Entities.AddRange(elbowList1);


                // Tee
                TeeModel newTee = new TeeModel() { OD = "33.4", A = "38.1", B = "38.1" };
                List<Entity> teeList0 = nozzleDraw.DrawReference_Nozzle_Tee(out outputPointList, outputPointList[0], 1, newTee, 0, new DrawCenterLineModel() { scaleValue = 90, zeroEx = true, oneEx = true, twoEx = true });
                singleModel.Entities.AddRange(teeList0);

                // Reducer
                ReducerModel newReducer = new ReducerModel() { OD1 = "60.3", OD2 = "33.4", H = "72" };
                List<Entity> reducer0 = nozzleDraw.DrawReference_Nozzle_Reducer(out outputPointList, outputPointList[2], 1, newReducer, Utility.DegToRad(90), new DrawCenterLineModel() { scaleValue = 90, zeroEx = true, oneEx = true });
                singleModel.Entities.AddRange(reducer0);

                // Pipe

                List<Entity> pipe0 = nozzleDraw.DrawReference_Nozzle_Pipe(out outputPointList, outputPointList[0], 1, 33.4, 100, false, false, Utility.DegToRad(90), new DrawCenterLineModel() { scaleValue = 90, zeroEx = true, oneEx = true });
                singleModel.Entities.AddRange(pipe0);
                List<Entity> pipe1 = nozzleDraw.DrawReference_Nozzle_Pipe(out outputPointList, outputPointList[0], 1, 33.4, 100, false, false, 0, new DrawCenterLineModel() { scaleValue = 90, zeroEx = true, oneEx = true });
                singleModel.Entities.AddRange(pipe1);
                List<Entity> pipeSlope1 = nozzleDraw.DrawReference_Nozzle_PipeSlope(out outputPointList, outputPointList[0], 1, 33.4, 100, 0, 0, 0, new DrawCenterLineModel() { scaleValue = 90, zeroEx = true, oneEx = true });
                singleModel.Entities.AddRange(pipeSlope1);
                Point3D newPP = GetSumPoint(outputPointList[1], 33.4, 0);
                List<Entity> pipeSlope2 = nozzleDraw.DrawReference_Nozzle_PipeSlope(out outputPointList, newPP, 1, 33.4, 100, Utility.DegToRad(30), 33.4, Utility.DegToRad(90), new DrawCenterLineModel() { scaleValue = 90, zeroEx = true, oneEx = true });
                singleModel.Entities.AddRange(pipeSlope2);


                List<Point3D> outputPointListTest = new List<Point3D>();
                List<Entity> pipeSlope3 = nozzleDraw.DrawReference_Nozzle_PipeSlope(out outputPointListTest, new Point3D(300, 300), 1, 33.4, 100, Utility.DegToRad(-15), 33.4, Utility.DegToRad(90), new DrawCenterLineModel() { scaleValue = 90, zeroEx = true, oneEx = true });
                singleModel.Entities.AddRange(pipeSlope3);

                List<Entity> neck1 = nozzleDraw.DrawReference_Nozzle_Neck(out outputPointList, outputPointList[0], 1, 33.4, 50, 20, Utility.DegToRad(45), new DrawCenterLineModel() { scaleValue = 90, zeroEx = true, oneEx = true });
                singleModel.Entities.AddRange(neck1);

                List<Entity> pad1 = nozzleDraw.DrawReference_Nozzle_Pad(out outputPointList, outputPointList[0], 1, 33.4, 100, 20, Utility.DegToRad(45), new DrawCenterLineModel() { scaleValue = 90, zeroEx = true, oneEx = true });
                singleModel.Entities.AddRange(pad1);

                List<Entity> padSlope1 = nozzleDraw.DrawReference_Nozzle_PadSlope(out outputPointList, outputPointList[0], 1, 33.4, 100, 20, false, Utility.DegToRad(30), new DrawCenterLineModel() { scaleValue = 90, zeroEx = true, oneEx = true });
                singleModel.Entities.AddRange(padSlope1);

                List<Entity> blindFlange1 = nozzleDraw.DrawReference_Nozzle_BlindFlange(out outputPointList, outputPointList[0], 1, 33.4, 100, 20, Utility.DegToRad(30), new DrawCenterLineModel() { scaleValue = 90, zeroEx = true, oneEx = true });
                singleModel.Entities.AddRange(blindFlange1);

                List<Entity> flange1 = nozzleDraw.DrawReference_Nozzle_Flange(out outputPointList, outputPointList[0], 1, 100, 140, 20, Utility.DegToRad(45), new DrawCenterLineModel() { scaleValue = 90, zeroEx = true, oneEx = true, twoEx = true });
                singleModel.Entities.AddRange(flange1);
            }

            if (visibleFalse)
            {

                double scaleValue = 90;

                ShellManholeModel newShell = new ShellManholeModel();
                double D1 = valueService.GetDoubleValue(newShell.D1);
                double BCD = valueService.GetDoubleValue(newShell.BCD);
                double D2 = valueService.GetDoubleValue(newShell.D2);
                double L = valueService.GetDoubleValue(newShell.L);
                D1 = 500;
                BCD = 667;
                D2 = 730;
                L = 1055;
                double LHalf = L / 2;
                double R = D1 / 2;
                double LCross = valueService.GetAdjacentByHypotenuse(valueService.GetDegreeOfSlope("1"), LHalf);


                List<Entity> newList = new List<Entity>();
                

                Point3D refPoint = new Point3D(300, 300);
                Line vLine01 = new Line(GetSumPoint(refPoint, 0, LHalf), GetSumPoint(refPoint, L * 10, LHalf));
                Line vLine02 = new Line(GetSumPoint(refPoint, -L * 100,0), GetSumPoint(refPoint, L * 10, 0));
                vLine02.Rotate(Utility.DegToRad(-45), Vector3D.AxisZ, refPoint);
                vLine02.Translate(new Vector3D(LCross, LCross));
                Line vLine03 = new Line(GetSumPoint(refPoint,0, -LHalf), GetSumPoint(refPoint, L * 10, -LHalf));
                Line vLine04 = new Line(GetSumPoint(refPoint, -L * 100,0), GetSumPoint(refPoint, L * 10, 0));
                vLine04.Rotate(Utility.DegToRad(45), Vector3D.AxisZ, refPoint);
                vLine04.Translate(new Vector3D(LCross, -LCross));

                Arc arcFillet01;
                Curve.Fillet(vLine01, vLine02, R, false, false, true, true, out arcFillet01);
                Arc arcFillet03;
                Curve.Fillet(vLine03, vLine04, R, false, false, true, true, out arcFillet03);
                Arc arcFillet02;
                Curve.Fillet(vLine02, vLine04, R, true, false, true, true, out arcFillet02);

                Line vLine05 = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, L * 10, 0));
                Point3D[] pointInter01 = vLine05.IntersectWith(arcFillet02);


                Arc newArc01 = new Arc(refPoint, D2 / 2, Utility.DegToRad(90), Utility.DegToRad(-90));
                Arc newArc02 = new Arc(refPoint, BCD / 2, Utility.DegToRad(90), Utility.DegToRad(-90));
                Arc newArc03 = new Arc(refPoint, D1 / 2, Utility.DegToRad(90), Utility.DegToRad(-90));

                newList.AddRange(new Entity[] { vLine01, vLine02, vLine03,vLine04 });
                newList.AddRange(new Entity[] { arcFillet01, arcFillet02, arcFillet03 });
                newList.AddRange(new Entity[] { newArc01, newArc03 });

                styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

                styleService.SetLayer(ref newArc02, layerService.LayerCenterLine);
                newList.Add(newArc02);


                // Center LIne
                DrawCenterLineModel centerModel = new DrawCenterLineModel();
                if (pointInter01.Length > 0)
                {
                    
                    Line centerLine01 = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(pointInter01[0], 0, 0));
                    Line centerLine02 = new Line(GetSumPoint(pointInter01[0], 0, 0), GetSumPoint(pointInter01[0], centerModel.exLength * scaleValue, 0));
                    styleService.SetLayer(ref centerLine01, layerService.LayerCenterLine);
                    styleService.SetLayer(ref centerLine02, layerService.LayerCenterLine);
                    newList.Add(centerLine01);
                    newList.Add(centerLine02);
                }

                // Mirror

                // Verical

                Line centerVLine01 = new Line(GetSumPoint(refPoint, 0, LHalf), GetSumPoint(refPoint, 0, -LHalf));
                Line centerVLine02 = new Line(GetSumPoint(refPoint, 0, LHalf), GetSumPoint(refPoint, 0, LHalf + centerModel.exLength * scaleValue));
                Line centerVLine03 = new Line(GetSumPoint(refPoint, 0, -LHalf), GetSumPoint(refPoint, 0, -LHalf - centerModel.exLength * scaleValue));
                styleService.SetLayer(ref centerVLine01, layerService.LayerCenterLine);
                styleService.SetLayer(ref centerVLine02, layerService.LayerCenterLine);
                styleService.SetLayer(ref centerVLine03, layerService.LayerCenterLine);
                newList.Add(centerVLine01);
                newList.Add(centerVLine02);
                newList.Add(centerVLine03);



                singleModel.Entities.AddRange(newList);
            }


            if (visibleFalse)
            {

                List<Entity> customEntity = new List<Entity>();
                List<Entity> customLine = new List<Entity>();

                AnchorChairModel newAnchor = new AnchorChairModel();


                double A = valueService.GetDoubleValue(newAnchor.A);
                double A1 = valueService.GetDoubleValue(newAnchor.A1);
                double B = valueService.GetDoubleValue(newAnchor.B);
                double E = valueService.GetDoubleValue(newAnchor.E);
                double F = valueService.GetDoubleValue(newAnchor.F);
                double T = valueService.GetDoubleValue(newAnchor.T);
                double T1 = valueService.GetDoubleValue(newAnchor.T1);
                double H = valueService.GetDoubleValue(newAnchor.H);
                double I = valueService.GetDoubleValue(newAnchor.I);
                double W = valueService.GetDoubleValue(newAnchor.W);
                double P = valueService.GetDoubleValue(newAnchor.P);
                double T2 = valueService.GetDoubleValue(newAnchor.T2);

                double C1 = valueService.GetDoubleValue(newAnchor.C1);
                double B1 = valueService.GetDoubleValue(newAnchor.B1);
                double H1 = valueService.GetDoubleValue(newAnchor.H1);
                double G1 = valueService.GetDoubleValue(newAnchor.G1);
                double C = valueService.GetDoubleValue(newAnchor.C);

                A = 80;
                A1 = 40;
                B = 120;
                E = 80;
                F = 135;
                T = 25;
                T1 = 12;
                H = 250;
                I = 27;
                W = 110;
                P = 70;
                T2 = 12;
                C1 = 50;
                B1 = 50;
                H1 = 15;
                G1 = 15;
                C = 36;

                double scaleValue = 90;

                double shellThickness = 24;
                double bottomThickness = 8;

                double anchorHeight = H - bottomThickness;
                double padHeight = anchorHeight + B1;
                double anchorCenterWidth = shellThickness + A;

                List<Entity> newList = new List<Entity>();

                //CDPoint curPoint = new CDPoint();
                //CDPoint bottomPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottomUp, 0, ref refPoint, ref curPoint);


                Point3D refCenterPoint = new Point3D(1000, 1000);

                Point3D PadLeftDown = GetSumPoint(refCenterPoint, 0, C1);
                Point3D PadRightDown = GetSumPoint(refCenterPoint, shellThickness, C1);
                Point3D PadLeftUp = GetSumPoint(refCenterPoint, 0, padHeight);
                Point3D PadRightUp = GetSumPoint(refCenterPoint, shellThickness, padHeight);

                Point3D TopPadLeftUp = GetSumPoint(PadRightUp, 0, -B1);
                Point3D TopPadLeftDown = GetSumPoint(TopPadLeftUp, 0, -T);
                Point3D TopPadRightUp = GetSumPoint(TopPadLeftUp, B, 0);
                Point3D TopPadRightDown = GetSumPoint(TopPadLeftUp, B, -T);


                Point3D TopSmallTriUpRight = GetSumPoint(TopPadLeftDown, H1, 0);
                Point3D TopSmallTriDownLeft = GetSumPoint(TopPadLeftDown, 0, -H1);

                Point3D TopSmallPadLeftUp = GetSumPoint(TopPadLeftUp, A - P / 2, T2);
                Point3D TopSmallPadLeftDown = GetSumPoint(TopSmallPadLeftUp, 0, -T2);
                Point3D TopSmallPadRightUp = GetSumPoint(TopSmallPadLeftUp, P, 0);
                Point3D TopSmallPadDown = GetSumPoint(TopSmallPadRightUp, 0, -T2);

                Point3D bigTriLeft = GetSumPoint(refCenterPoint, shellThickness + G1, 0);
                Point3D bigTriRight = GetSumPoint(refCenterPoint, shellThickness + C, 0);

                Point3D CenterTop = GetSumPoint(TopSmallPadLeftUp, P / 2, 0);
                Point3D CenterDown = GetSumPoint(refCenterPoint, shellThickness + A, -bottomThickness);


                newList.Add(new Line(PadLeftDown, PadRightDown));
                newList.Add(new Line(PadLeftDown, PadLeftUp));
                newList.Add(new Line(PadLeftUp, PadRightUp));
                newList.Add(new Line(PadRightDown, PadRightUp));

                newList.Add(new Line(TopPadLeftUp, TopPadRightUp));
                newList.Add(new Line(TopPadRightDown, TopPadRightUp));
                newList.Add(new Line(TopPadLeftDown, TopPadRightDown));

                newList.Add(new Line(TopSmallPadLeftUp, TopSmallPadLeftDown));
                newList.Add(new Line(TopSmallPadLeftUp, TopSmallPadRightUp));
                newList.Add(new Line(TopSmallPadDown, TopSmallPadRightUp));

                newList.Add(new Line(TopSmallTriUpRight, TopSmallTriDownLeft));


                newList.Add(new Line(PadRightDown, bigTriLeft));
                newList.Add(new Line(bigTriLeft, bigTriRight));
                newList.Add(new Line(bigTriRight, TopPadRightDown));

                styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);



                // Center LIne
                DrawCenterLineModel centerModel = new DrawCenterLineModel();
                customLine.Add(new Line(CenterTop, CenterDown));
                customLine.Add(new Line(CenterTop, GetSumPoint(CenterTop, 0, centerModel.exLength * scaleValue)));
                customLine.Add(new Line(CenterDown, GetSumPoint(CenterDown, 0, -centerModel.exLength * scaleValue)));

                styleService.SetLayerListEntity(ref customLine, layerService.LayerCenterLine);
                // Mirror

                // Verical
                newList.AddRange(customLine);
                customEntity.AddRange(newList);

                singleModel.Entities.AddRange(customEntity);
            }



            if (visibleFalse)
            {
                Point3D refCenterPoint = new Point3D(1000, 1000);

                // View Port 점검
                List<Entity> newnewList = new List<Entity>();
                newnewList.Add(new Line(GetSumPoint(refCenterPoint, 0, 0), GetSumPoint(refCenterPoint, 1000, 1000)));
                newnewList.Add(new Line(GetSumPoint(refCenterPoint, 0, 0), GetSumPoint(refCenterPoint, 0, 1000)));
                newnewList.Add(new Line(GetSumPoint(refCenterPoint, 0, 1000), GetSumPoint(refCenterPoint, 1000, 1000)));
                newnewList.Add(new Line(GetSumPoint(refCenterPoint, 1000, 0), GetSumPoint(refCenterPoint, 1000, 1000)));
                newnewList.Add(new Line(GetSumPoint(refCenterPoint, 1000, 0), GetSumPoint(refCenterPoint, 0, 0)));
                newnewList.Add(new Line(GetSumPoint(refCenterPoint, 0, 1000), GetSumPoint(refCenterPoint, 1000, 0)));

                styleService.SetLayerListEntity(ref newnewList, layerService.LayerCenterLine);


                // 오른쪽 복사
                List<Entity> newnewListCopy = new List<Entity>();
                foreach (Entity eachEn in newnewList)
                {
                    Entity newEn = (Entity)eachEn.Clone();
                    newEn.Translate(2000, 2000);
                    newnewListCopy.Add(newEn);
                }
                styleService.SetLayerListEntity(ref newnewListCopy, layerService.LayerOutLine);
                newnewList.Add(new Line(GetSumPoint(refCenterPoint, 1000, 1000), GetSumPoint(refCenterPoint, 2000, 2000)));

                // 왼쪽 복사
                List<Entity> leftListCopy = new List<Entity>();
                foreach (Entity eachEn in newnewList)
                {
                    Entity newEn = (Entity)eachEn.Clone();
                    newEn.Translate(0, 4000);
                    leftListCopy.Add(newEn);
                }
                styleService.SetLayerListEntity(ref leftListCopy, layerService.LayerVirtualLine);
                newnewList.Add(new Line(GetSumPoint(refCenterPoint, 2000, 3000), GetSumPoint(refCenterPoint, 1000, 4000)));


                singleModel.Entities.AddRange(leftListCopy);
                singleModel.Entities.AddRange(newnewListCopy);
                singleModel.Entities.AddRange(newnewList);
            }

            // Angle
            if (visibleFalse)
            {
                List<Entity> newList = new List<Entity>();


                List<Entity> newRoofList = new List<Entity>();

                // Input Data
                double sizeID = 600;
                double domeRadiusRatio = 1;
                double domeRadius = domeRadiusRatio * sizeID;

                Point3D refPointLeft = new Point3D(10000, 10000);
                Point3D refPointRight = GetSumPoint(refPointLeft, 600, 0);

                // Circle : 2
                Circle cirLeft = new Circle(GetSumPoint(refPointLeft,0,0), domeRadius);
                Circle cirRight = new Circle(GetSumPoint(refPointRight,0,0), domeRadius);
                Point3D[] cirIntersectArray = cirLeft.IntersectWith(cirRight);

                // Center Circle
                Point3D currentCenterPoint = new Point3D();
                if (cirIntersectArray != null)
                {
                    double minValue = 9999999999;
                    foreach (Point3D eachPoint in cirIntersectArray)
                    {
                        if (minValue > eachPoint.Y)
                        {
                            minValue = eachPoint.Y;
                            currentCenterPoint = GetSumPoint(eachPoint, 0, 0);
                        }
                    }
                }
                Circle cirCenter = new Circle(GetSumPoint(currentCenterPoint,0,0), domeRadius);

                // Left line
                Point3D leftRoofDownPoint = new Point3D();
                leftRoofDownPoint = refPointLeft;


                Circle cirComPress = new Circle(GetSumPoint(leftRoofDownPoint, 0, 0), domeRadius);

                // compression Ring : Ref Point : Line
                Line centerVerticalLine = new Line(GetSumPoint(currentCenterPoint, 0, -domeRadius), GetSumPoint(currentCenterPoint, 0, domeRadius * 3));// Size ID 길이 만큼
                

                // Intersect : CenterCircle <-> Left Line
                double shiftVertical = 0;
                Point3D[] leftIntersect = centerVerticalLine.IntersectWith(cirComPress);
                if (leftIntersect != null)
                {
                    if (leftIntersect.Length > 0)
                    {
                        currentCenterPoint = leftIntersect[1];
                    }

                }


                // 검증 용
                if (true)
                {
                    newList.Add(new Line(refPointLeft, GetSumPoint(refPointLeft, 0, -1000)));
                    newList.Add(new Line(refPointRight, GetSumPoint(refPointRight, 0, -1000)));
                    newList.Add(cirLeft);
                    newList.Add(cirRight);
                    newList.Add(cirCenter);
                    newList.Add(cirComPress);
                    newList.Add(centerVerticalLine);

                    styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);
                    newList.Add(new Line(refPointLeft, GetSumPoint(refPointLeft, -7,0)));
                    newList.Add(new Line(refPointLeft, refPointRight));
                    newList.Add(new Circle(currentCenterPoint, domeRadius));
                }


               

                singleModel.Entities.AddRange(newList);
                singleModel.Entities.AddRange(newRoofList);
            }

            // Compression Ring
            if (visibleFalse)
            {
                List<Entity> newList = new List<Entity>();


                List<Entity> newRoofList = new List<Entity>();

                // Input Data
                double sizeID = 600;
                double domeRadiusRatio = 1;
                double domeRadius = domeRadiusRatio * sizeID;

                double shellMaxCourseThickness = 10;
                double comA = 10;
                double comB = 70;
                double comC = 12;
                double comT1 = 2.5;



                Point3D refPointLeft = new Point3D(10000, 10000);
                Point3D refPointRight = GetSumPoint(refPointLeft, 600, 0);

                // Circle : 2
                Circle cirLeft = new Circle(GetSumPoint(refPointLeft, 0, 0), domeRadius);
                Circle cirRight = new Circle(GetSumPoint(refPointRight, 0, 0), domeRadius);
                Point3D[] cirIntersectArray = cirLeft.IntersectWith(cirRight);

                // Center Circle
                Point3D currentCenterPoint = new Point3D();
                if (cirIntersectArray != null)
                {
                    double minValue = 9999999999;
                    foreach (Point3D eachPoint in cirIntersectArray)
                    {
                        if (minValue > eachPoint.Y)
                        {
                            minValue = eachPoint.Y;
                            currentCenterPoint = GetSumPoint(eachPoint, 0, 0);
                        }
                    }
                }
                Circle cirCenter = new Circle(GetSumPoint(currentCenterPoint, 0, 0), domeRadius);

                // Compression Ring : Rotate
                Line comPressRing = new Line(GetSumPoint(currentCenterPoint, 0, 0), GetSumPoint( refPointLeft,0,0));
                comPressRing.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
                // Direction
                Vector3D direction = new Vector3D(comPressRing.EndPoint, comPressRing.StartPoint);
                direction.Normalize();
                // Compression Ring : New : Shift : Thickness
                Line comPressRingNew = new Line(GetSumPoint(refPointLeft, -shellMaxCourseThickness, 0), GetSumPoint(refPointLeft, -shellMaxCourseThickness, 0));
                comPressRingNew.EndPoint = comPressRingNew.EndPoint + direction * (comB -comA);
                // compression Ring Upper : Offset
                Line comPressRingNewUpper = (Line)comPressRingNew.Offset(-comT1, Vector3D.AxisZ, 0.01, true);// 위로


                // compression Ring : Ref Point
                Point3D comPressRingRefPoint = GetSumPoint(comPressRingNewUpper.EndPoint, 0, 0);
                // compression Ring : circle
                Circle cirComPress = new Circle(GetSumPoint(comPressRingRefPoint, 0, 0), domeRadius);

                // compression Ring : Ref Point : Line
                Line centerVerticalLine = new Line(GetSumPoint(currentCenterPoint, 0, 0), GetSumPoint(currentCenterPoint, 0, sizeID*2));// Size ID 길이 만큼

                // Intersect : CenterCircle <-> Left Line
                double shiftVertical = 0;
                Point3D[] leftIntersect = centerVerticalLine.IntersectWith(cirComPress);
                if (leftIntersect != null)
                {
                    if (leftIntersect.Length > 0)
                    {
                        currentCenterPoint = leftIntersect[1];
                    }

                }


                // 검증 용
                if (true)
                {
                    Arc newARC=new Arc(currentCenterPoint, GetSumPoint(comPressRingRefPoint, 0, 0), GetSumPoint(centerVerticalLine.EndPoint, 0, 0));
                    newList.Add(newARC);



                    newList.Add(new Line(refPointLeft, GetSumPoint(refPointLeft, 0, -1000)));
                    newList.Add(new Line(refPointRight, GetSumPoint(refPointRight, 0, -1000)));
                    newList.Add(cirLeft);
                    newList.Add(cirRight);
                    newList.Add(cirCenter);
                    newList.Add(cirComPress);
                    newList.Add(centerVerticalLine);

                    //newList.Add(comPressRing);
                    newList.Add(comPressRingNew);
                    styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);


                    styleService.SetLayer(ref comPressRingNewUpper, layerService.LayerDimension);
                    newList.Add(comPressRingNewUpper);

                    newList.Add(new Line(refPointLeft, GetSumPoint(refPointLeft, shellMaxCourseThickness, 0)));
                    newList.Add(new Line(refPointLeft, refPointRight));
                    newList.Add(new Circle(currentCenterPoint, domeRadius));
                }




                singleModel.Entities.AddRange(newList);
                singleModel.Entities.AddRange(newRoofList);
            }


            if (visibleFalse)
            {
                // End Point : Extend : 종료 방향
                Line aa = new Line(300, 300, 500, 500);
                Line line = (Line)aa.Clone();

                // Create temp line which will intersect boundary curve depending on which end to extend
                Line tempLine = null;
                Vector3D direction = null;
                
                tempLine = new Line(line.StartPoint, line.EndPoint);
                direction = new Vector3D(line.StartPoint, line.EndPoint);

                direction.Normalize();
                tempLine.EndPoint = tempLine.EndPoint + direction * 200;

                tempLine.Translate(10,0);

                styleService.SetLayer(ref aa, layerService.LayerOutLine);
                singleModel.Entities.Add(aa);
                singleModel.Entities.Add(tempLine);

            }
            if (visibleFalse)
            {
                // Start Point : Extend  : 시작 방향
                Line aa = new Line(300, 300, 500, 500);
                Line line = (Line)aa.Clone();

                // Create temp line which will intersect boundary curve depending on which end to extend
                Line tempLine = null;
                Vector3D direction = null;

                tempLine = new Line(line.StartPoint, line.EndPoint);
                direction = new Vector3D(line.EndPoint, line.StartPoint);

                direction.Normalize();
                tempLine.StartPoint = tempLine.StartPoint + direction * 200;

                tempLine.Translate(10, 0);

                styleService.SetLayer(ref aa, layerService.LayerOutLine);
                singleModel.Entities.Add(aa);
                singleModel.Entities.Add(tempLine);

            }

            if (visibleFalse)
            {
                Point3D ccc = new Point3D(100, 100);
                Circle cccc = new Circle(ccc, 100);

                Point3D selPoint = new Point3D(22, 300);

                double selPositionX = selPoint.X- ccc.X;
                double selWidth = 20;
                double selWidthHalf = selWidth / 2;

                double padHeight = 0;

                Line vLine01 = new Line(GetSumPoint(ccc, selPositionX, +200), GetSumPoint(ccc, selPositionX, -200));
                Line vLineleft = (Line)vLine01.Offset(selWidthHalf, Vector3D.AxisZ, 0.01, true);
                Line vLineRight = (Line)vLine01.Offset(-selWidthHalf, Vector3D.AxisZ, 0.01, true);


                Point3D[] vInter = cccc.IntersectWith(vLine01);
                Point3D[] vInterLeft = cccc.IntersectWith(vLineleft);
                Point3D[] vInterRight = cccc.IntersectWith(vLineRight);


                Arc newArc = new Arc(ccc, vInterLeft[0], vInterRight[0]);


                
                styleService.SetLayer(ref newArc, layerService.LayerOutLine);
                singleModel.Entities.Add(newArc);

                singleModel.Entities.Add(vLine01);
                singleModel.Entities.Add(vLineleft);
                singleModel.Entities.Add(vLineRight);

                singleModel.Entities.Add(cccc);

            }



            if (visibleFalse)
            {
                Point3D ccc = new Point3D(100, 100);
                Circle cccc = new Circle(ccc, 100);

                Point3D selPoint = new Point3D(22, 300);

                double selPositionX = selPoint.X - ccc.X;
                double selWidth = 20;
                double selWidthHalf = selWidth / 2;

                double padHeight = 0;

                Line vLine01 = new Line(GetSumPoint(ccc, selPositionX, +200), GetSumPoint(ccc, selPositionX, -200));
                Line vLineleft = (Line)vLine01.Offset(selWidthHalf, Vector3D.AxisZ, 0.01, true);
                Line vLineRight = (Line)vLine01.Offset(-selWidthHalf, Vector3D.AxisZ, 0.01, true);


                Point3D[] vInter = cccc.IntersectWith(vLine01);
                Point3D[] vInterLeft = cccc.IntersectWith(vLineleft);
                Point3D[] vInterRight = cccc.IntersectWith(vLineRight);


                Arc newArc = new Arc(ccc, vInterLeft[0], vInterRight[0]);



                styleService.SetLayer(ref newArc, layerService.LayerOutLine);
                singleModel.Entities.Add(newArc);

                singleModel.Entities.Add(vLine01);
                singleModel.Entities.Add(vLineleft);
                singleModel.Entities.Add(vLineRight);

                singleModel.Entities.Add(cccc);

            }

            if (visibleFalse)
            {
                string selText = "HLL 18100";
                double scale = 20;
                double textHeight = 2.5;
                double scaleTextHeight = textHeight * scale;
                double middleLineWidth = 0;

                Point3D riqCenter = new Point3D(100, 100);

                List<Entity> newList = new List<Entity>();
                
                Point3D triP01 = GetSumPoint(riqCenter, 0, 0);
                Point3D triP02 = GetSumPoint(riqCenter, scaleTextHeight/2, scaleTextHeight);
                Point3D triP03 = GetSumPoint(riqCenter, -scaleTextHeight / 2, scaleTextHeight);
                Line tri01 = new Line(GetSumPoint(triP01, 0, 0), GetSumPoint(triP02, 0, 0));
                Line tri02 = new Line(GetSumPoint(triP01, 0, 0), GetSumPoint(triP03, 0, 0));
                Line tri03 = new Line(GetSumPoint(triP03, 0, 0), GetSumPoint(triP02, 0, 0));


                Point3D textPoint = GetSumPoint(riqCenter, scaleTextHeight / 2 + scaleTextHeight / 2,0);

                Text newText01 = new Text(GetSumPoint(textPoint, 0, scaleTextHeight*0.2), selText, scaleTextHeight);
                newText01.Regen(new RegenParams(0, singleModel));
                newText01.Alignment = Text.alignmentType.BaselineLeft;
                styleService.SetLayer(ref newText01, layerService.LayerDimension);
                middleLineWidth = newText01.BoxSize.X;

                Line newCourseLine = new Line(GetSumPoint(riqCenter, -scaleTextHeight / 2, 0), GetSumPoint(textPoint, middleLineWidth +(middleLineWidth/3) + scaleTextHeight / 2, 0));

                Point3D triqq01 = GetSumPoint(textPoint, 0, -(scaleTextHeight / 3));
                Point3D triqq02 = GetSumPoint(triqq01,  (scaleTextHeight / 2), -(scaleTextHeight / 3));
                Point3D triqq03 = GetSumPoint(triqq02, (scaleTextHeight / 2) , -(scaleTextHeight / 3));
                Line triiLine01 = new Line(GetSumPoint(triqq01, 0, 0), GetSumPoint(triqq01, scaleTextHeight * 3, 0));
                Line triiLine02 = new Line(GetSumPoint(triqq02, 0, 0), GetSumPoint(triqq02, scaleTextHeight * 2, 0));
                Line triiLine03 = new Line(GetSumPoint(triqq03, 0, 0), GetSumPoint(triqq03, scaleTextHeight * 1, 0));

                styleService.SetLayer(ref newText01, layerService.LayerDimension);

                newList.Add(tri01);
                newList.Add(tri02);
                newList.Add(tri03);
                newList.Add(triiLine01);
                newList.Add(triiLine02);
                newList.Add(triiLine03);
                newList.Add(newCourseLine);

                styleService.SetLayerListEntity(ref newList, layerService.LayerDimension);

                singleModel.Entities.Add(newText01);
                singleModel.Entities.AddRange(newList);
            }

            if (visibleFalse)
            {
                string selText1 = "12";
                string selText2 = "1";
                double scale = 20;
                double textHeight = 2.5;
                double scaleTextHeight = textHeight * scale;
                double middleLineWidth = 0;

                Point3D riqCenter = new Point3D(100, 100);

                List<Entity> newList = new List<Entity>();

                Point3D triP01 = GetSumPoint(riqCenter, 0, 0);
                Point3D triP02 = GetSumPoint(triP01, scaleTextHeight *2 + scaleTextHeight * 3 + scaleTextHeight * 2, 0);
                Point3D triP03 = GetSumPoint(triP02, 0, scaleTextHeight);
                Line tri01 = new Line(GetSumPoint(triP01, 0, 0), GetSumPoint(triP02, 0, 0));
                Line tri02 = new Line(GetSumPoint(triP02, 0, 0), GetSumPoint(triP03, 0, 0));
                Line tri03 = new Line(GetSumPoint(triP03, 0, 0), GetSumPoint(triP01, 0, 0));


                Text newText01 = new Text(GetSumPoint(tri01.MidPoint,0 , -scaleTextHeight * 0.2 ), selText1, scaleTextHeight);
                newText01.Alignment = Text.alignmentType.TopCenter;
                styleService.SetLayer(ref newText01, layerService.LayerDimension);

                Text newText02 = new Text(GetSumPoint(tri02.MidPoint, +scaleTextHeight * 0.2, 0), selText2, scaleTextHeight);
                newText02.Alignment = Text.alignmentType.MiddleLeft;
                styleService.SetLayer(ref newText02, layerService.LayerDimension);


                styleService.SetLayerListEntity(ref newList, layerService.LayerDimension);

                newList.Add(tri01);
                newList.Add(tri02);
                newList.Add(tri03);
                newList.Add(newText01);
                newList.Add(newText02);
                singleModel.Entities.AddRange(newList);
            }


            if (visibleFalse)
            {
                string selText1 = "12";
                string selText2 = "1";
                double scale = 20;
                double textHeight = 2.5;
                double scaleTextHeight = textHeight * scale;
                double middleLineWidth = 0;

                Point3D riqCenter = new Point3D(100, 100);

                List<Entity> newList = new List<Entity>();

                Point3D triP01 = GetSumPoint(riqCenter, 0, 0);
                Point3D triP02 = GetSumPoint(triP01, scaleTextHeight * 2 + scaleTextHeight * 3 + scaleTextHeight * 2, 0);
                Point3D triP03 = GetSumPoint(triP02, 0, -scaleTextHeight);
                Line tri01 = new Line(GetSumPoint(triP01, 0, 0), GetSumPoint(triP02, 0, 0));
                Line tri02 = new Line(GetSumPoint(triP02, 0, 0), GetSumPoint(triP03, 0, 0));
                Line tri03 = new Line(GetSumPoint(triP03, 0, 0), GetSumPoint(triP01, 0, 0));


                Text newText01 = new Text(GetSumPoint(tri01.MidPoint, 0, scaleTextHeight * 0.2), selText1, scaleTextHeight);
                newText01.Alignment = Text.alignmentType.BottomCenter;
                styleService.SetLayer(ref newText01, layerService.LayerDimension);

                Text newText02 = new Text(GetSumPoint(tri02.MidPoint, +scaleTextHeight * 0.2, 0), selText2, scaleTextHeight);
                newText02.Alignment = Text.alignmentType.MiddleLeft;
                styleService.SetLayer(ref newText02, layerService.LayerDimension);


                styleService.SetLayerListEntity(ref newList, layerService.LayerDimension);

                newList.Add(tri01);
                newList.Add(tri02);
                newList.Add(tri03);
                newList.Add(newText01);
                newList.Add(newText02);
                singleModel.Entities.AddRange(newList);
            }

            if (visibleFalse)
            {
                string selText1 = "12";
                string selText2 = "1";
                double scale = 20;
                double textHeight = 2.5;
                double scaleTextHeight = textHeight * scale;
                double middleLineWidth = 0;

                Point3D riqCenter = new Point3D(100, 100);

                List<Entity> newList = new List<Entity>();

                Point3D triP01 = GetSumPoint(riqCenter, 0, 0);
                Point3D triP02 = GetSumPoint(triP01, scaleTextHeight * 2 + scaleTextHeight * 3 + scaleTextHeight * 2, 0);
                Point3D triP03 = GetSumPoint(triP02, 0, -scaleTextHeight);
                Line tri01 = new Line(GetSumPoint(triP01, 0, 0), GetSumPoint(triP02, 0, 0));
                Line tri02 = new Line(GetSumPoint(triP02, 0, 0), GetSumPoint(triP03, 0, 0));
                Line tri03 = new Line(GetSumPoint(triP03, 0, 0), GetSumPoint(triP01, 0, 0));


                Text newText01 = new Text(GetSumPoint(tri01.MidPoint, 0, scaleTextHeight * 0.2), selText1, scaleTextHeight);
                newText01.Alignment = Text.alignmentType.BottomCenter;
                styleService.SetLayer(ref newText01, layerService.LayerDimension);

                Text newText02 = new Text(GetSumPoint(tri02.MidPoint, +scaleTextHeight * 0.2, 0), selText2, scaleTextHeight);
                newText02.Alignment = Text.alignmentType.MiddleLeft;
                styleService.SetLayer(ref newText02, layerService.LayerDimension);


                styleService.SetLayerListEntity(ref newList, layerService.LayerDimension);

                newList.Add(tri01);
                newList.Add(tri02);
                newList.Add(tri03);
                newList.Add(newText01);
                newList.Add(newText02);
                singleModel.Entities.AddRange(newList);
            }

            // Single Deck
            if (visibleFalse)
            {
                DrawFRTBlockService drawFRT = new DrawFRTBlockService();

                List<Entity> newList = new List<Entity>();
                List<Point3D> newOutPoint = new List<Point3D>();

                double selScaleValue = 90;

                Point3D referencePoint = new Point3D(1000, 1000);


                double tankID = 40000;
                double tankIDHalf = tankID/2;

                double tankHeight = 25000;

                Point3D tankBottomMidPoint = GetSumPoint(referencePoint, tankIDHalf, 0);

                string topSlope = "1/64";
                double topSlopeDegree = valueService.GetDegreeOfSlope(topSlope);

                string bottomSlope = "1/120";
                double bottomSlopeDegree = valueService.GetDegreeOfSlope(bottomSlope);;

                // 매우중요
                double pontoonPositionHeight = drawFRT.GetPontoonPositionHeight(tankHeight);


                double shellLeft = 200;

                double pontoonLeftHeight = 800;
                double pontoonTopLeftExt = 100;
                double pontoonTopRightExt = 15;
                double pontoonBottomLeftExt = 10;
                double pontoonBottomRightExt = 10;
                double pontoonRightFlatWidth = 220;
                double pontoonRightFlatOverlap = 40;

                double pontoonWidth = 6500;
                double pontoonLenghtG = 150;

                // 중앙까지 계산 해야 함
                double pontoonFlatLength = tankIDHalf - (shellLeft + pontoonWidth + pontoonRightFlatWidth - pontoonRightFlatOverlap);

                double pontoonThkA = 10;
                double pontoonThkB = 10;
                double pontoonThkC = 10;
                double pontoonThkD = 10;
                double pontoonThkE = 10;
                double pontoonThkF = 10;



                newList.Add(new Line(GetSumPoint(referencePoint,0,0), GetSumPoint(referencePoint, 0, tankHeight)));
                newList.Add(new Line(GetSumPoint(referencePoint,0,0), GetSumPoint(referencePoint, tankIDHalf, 0)));

                Point3D ABottomLeftPoint = GetSumPoint(referencePoint, shellLeft, pontoonPositionHeight);
                Point3D BBottomLeftPoint = GetSumPoint(referencePoint, shellLeft + pontoonThkA, pontoonPositionHeight + pontoonLeftHeight);
                Point3D CTopLeftPoint = GetSumPoint(referencePoint, shellLeft - pontoonBottomLeftExt,
                                                    pontoonPositionHeight - valueService.GetOppositeByAdjacent(bottomSlopeDegree, pontoonBottomLeftExt +pontoonThkA));
                Point3D DTopRightPoint = GetSumPoint(referencePoint, shellLeft + pontoonWidth,
                                                    pontoonPositionHeight + pontoonLeftHeight - valueService.GetOppositeByAdjacent(topSlopeDegree, pontoonWidth - pontoonThkA));
                Point3D DBottomRightPoint = GetSumPoint(referencePoint, shellLeft + pontoonWidth,
                                                    pontoonPositionHeight + valueService.GetOppositeByAdjacent(bottomSlopeDegree, pontoonWidth - pontoonThkA));
                Point3D EBottomLeftPoint = GetSumPoint(DBottomRightPoint, 0, pontoonLenghtG + pontoonThkF);
                Point3D FTopLeftPoint = GetSumPoint(EBottomLeftPoint, pontoonRightFlatWidth - pontoonRightFlatOverlap,0);

                newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(ABottomLeftPoint, 0, 0), pontoonThkA, pontoonLeftHeight + pontoonTopLeftExt, 0, 0, 3));
                newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(BBottomLeftPoint, 0, 0),
                                 valueService.GetHypotenuseByWidth(topSlopeDegree, pontoonWidth - pontoonThkA) + 
                                 valueService.GetHypotenuseByWidth(topSlopeDegree, pontoonTopRightExt) -
                                 valueService.GetOppositeByAdjacent(topSlopeDegree,pontoonThkB), pontoonThkB, -topSlopeDegree, 3, 3));
                newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(CTopLeftPoint, 0, 0), 
                                 valueService.GetHypotenuseByWidth(bottomSlopeDegree, pontoonWidth+ pontoonBottomLeftExt) + 
                                 valueService.GetHypotenuseByWidth(bottomSlopeDegree, pontoonBottomRightExt)-
                                 valueService.GetOppositeByAdjacent(bottomSlopeDegree, pontoonThkC), pontoonThkC ,bottomSlopeDegree, 0, 0));
                newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(DTopRightPoint, 0, 0), pontoonThkD, DTopRightPoint.Y-DBottomRightPoint.Y, 0, 0, 1));
                newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(EBottomLeftPoint, 0, 0), pontoonRightFlatWidth, pontoonThkE, 0, 0, 3));
                newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(FTopLeftPoint, 0, 0), pontoonFlatLength, pontoonThkF, 0, 0, 0,new bool[] {true,false,true,true }));

                styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);



                // Deck Support
                List<Point3D> deckSupportPointList = new List<Point3D>();
                // flat Thickness 만큼 내려 줘야 함
                Point3D deckBottomPoint = GetSumPoint(FTopLeftPoint, 0, -pontoonThkF);
                deckSupportPointList.Add(GetSumPoint(deckBottomPoint, -2500, 0));
                deckSupportPointList.Add(GetSumPoint(deckBottomPoint, -1500, 0));
                deckSupportPointList.Add(GetSumPoint(deckBottomPoint, -500, 0));
                deckSupportPointList.Add(GetSumPoint(deckBottomPoint, 1000, 0));
                deckSupportPointList.Add(GetSumPoint(deckBottomPoint, 2000, 0));
                deckSupportPointList.Add(GetSumPoint(deckBottomPoint, 3000, 0));
                deckSupportPointList.Add(GetSumPoint(deckBottomPoint, 4000, 0));
                List<Entity> deckSupportList = new List<Entity>();
                foreach (Point3D eachPoint in deckSupportPointList)
                {
                    deckSupportList.AddRange(drawFRT.DrawFRT_DeckSupport(eachPoint, selScaleValue));
                }
                newList.AddRange(deckSupportList);



                // Right Mirror
                Plane mirrorPlane = Plane.YZ;
                mirrorPlane.Origin.X = tankBottomMidPoint.X;
                mirrorPlane.Origin.Y = tankBottomMidPoint.Y;
                List<Entity> newRightList = editingService.GetEntityByMirror(mirrorPlane, newList);
                styleService.SetLayerListEntityExcludingCenterLine(ref newRightList, layerService.LayerHiddenLine);

                singleModel.Entities.AddRange(newList);
                singleModel.Entities.AddRange(newRightList);




                // RunWay
                // ID : 2/3 Point
                double runWayWidth = drawFRT.GetRollingLadderWidth(tankID);
                Point3D runWayPoint = new Point3D(GetSumPoint(referencePoint, tankIDHalf* 2/ 3, 0).X, FTopLeftPoint.Y);
                List<Entity> runWayList = new List<Entity>();
                runWayList.AddRange(drawFRT.DrawFRT_RunWay(runWayPoint, runWayWidth));

                singleModel.Entities.AddRange(runWayList);


                // Rolling Ladder
                double tankLastShellThickness = 24;
                Point3D rollingLadderStartPoint = GetSumPoint(referencePoint, -tankLastShellThickness, tankHeight);
                Point3D rollingLadderEndPoint = GetSumPoint(runWayPoint, 0, 350); // 350 고정
                List<Entity> rollingLadderList = new List<Entity>();
                rollingLadderList.AddRange(drawFRT.DrawFRT_RollingLadder(rollingLadderStartPoint,rollingLadderEndPoint,selScaleValue));

                singleModel.Entities.AddRange(rollingLadderList);

            }

            // Deouble Deck
            if (visibleFalse)
            {
                DrawFRTBlockService drawFRT = new DrawFRTBlockService();

                List<Entity> newList = new List<Entity>();
                List<Point3D> newOutPoint = new List<Point3D>();

                Point3D referencePoint = new Point3D(1000, 1000);

                double tankID = 20000;
                double tankIDHalf = tankID / 2;

                double tankHeight = 25000;

                double selScaleValue = 90;

                Point3D tankBottomMidPoint = GetSumPoint(referencePoint, tankIDHalf, 0);

                string topSlope = "1/64";
                double topSlopeDegree = valueService.GetDegreeOfSlope(topSlope);

                string bottomSlope = "1/120";
                double bottomSlopeDegree = valueService.GetDegreeOfSlope(bottomSlope); ;

                double bottomHeight = 1000;
                double shellLeft = 200;

                double pontoonLeftHeight = 800;
                double pontoonTopLeftExt = 100;
                double pontoonTopRightExt = 15;
                double pontoonBottomLeftExt = 10;
                double pontoonBottomRightExt = 10;
                double pontoonRightFlatWidth = 220;
                double pontoonRightFlatOverlap = 40;

                double pontoonWidth = 1500;

                double pontoonThkA = 10;
                double pontoonThkB = 10;
                double pontoonThkC = 10;
                double pontoonThkD = 10;

                double pontoonTopWidth = tankIDHalf - (shellLeft + pontoonThkA);
                double pontoonBottomWidth = tankIDHalf - (shellLeft -pontoonBottomLeftExt);
                double pontoonFlatBottomWidth = tankIDHalf - (shellLeft + pontoonWidth + valueService.GetOppositeByHypotenuse(bottomSlope, pontoonThkC));

                // 중앙까지 계산 해야 함
                double pontoonFlatLength = tankIDHalf - (shellLeft + pontoonWidth + pontoonRightFlatWidth - pontoonRightFlatOverlap);



                newList.Add(new Line(GetSumPoint(referencePoint, 0, 0), GetSumPoint(referencePoint, 0, 4000)));
                newList.Add(new Line(GetSumPoint(referencePoint, 0, 0), GetSumPoint(referencePoint, 4000, 0)));

                Point3D ABottomLeftPoint = GetSumPoint(referencePoint, shellLeft, bottomHeight);
                Point3D BBottomLeftPoint = GetSumPoint(referencePoint, shellLeft + pontoonThkA, bottomHeight + pontoonLeftHeight);
                Point3D CTopLeftPoint = GetSumPoint(referencePoint, shellLeft - pontoonBottomLeftExt,
                                                    bottomHeight - valueService.GetOppositeByAdjacent(bottomSlopeDegree, pontoonBottomLeftExt + pontoonThkA));
                Point3D DTopRightPoint = GetSumPoint(referencePoint, shellLeft + pontoonWidth,
                                                    bottomHeight + pontoonLeftHeight - valueService.GetOppositeByAdjacent(topSlopeDegree, pontoonWidth - pontoonThkA));
                Point3D DBottomRightPoint = GetSumPoint(referencePoint, shellLeft + pontoonWidth,
                                                    bottomHeight + valueService.GetOppositeByAdjacent(bottomSlopeDegree, pontoonWidth - pontoonThkA));
                Point3D CFlatTopLeftPoint = GetSumPoint(DBottomRightPoint, valueService.GetOppositeByHypotenuse(bottomSlope,pontoonThkC), 0);

                double DHeight = DTopRightPoint.Y - DBottomRightPoint.Y;

                newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(ABottomLeftPoint, 0, 0), pontoonThkA, pontoonLeftHeight + pontoonTopLeftExt, 0, 0, 3));
                List<Entity> upperPlateList = shapeService.GetRectangle(out newOutPoint, GetSumPoint(BBottomLeftPoint, 0, 0),
                                 valueService.GetHypotenuseByWidth(topSlopeDegree, pontoonTopWidth) -
                                 valueService.GetOppositeByAdjacent(topSlopeDegree, pontoonThkB), pontoonThkB, -topSlopeDegree, 3, 3);
                newList.AddRange(upperPlateList);
                newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(CTopLeftPoint, 0, 0),
                                 valueService.GetHypotenuseByWidth(bottomSlopeDegree, pontoonWidth + pontoonBottomLeftExt), pontoonThkC, bottomSlopeDegree, 0, 0));
                newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(DTopRightPoint, 0, 0), pontoonThkD, DHeight, 0, 0, 1));
                newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(CFlatTopLeftPoint, 0, 0), pontoonFlatBottomWidth, pontoonThkC, 0, 0,0));

                // Add Mid FlatBar
                List<double> midFlatBarList = new List<double>();
                midFlatBarList.Add(300);
                midFlatBarList.Add(600);
                midFlatBarList.Add(800);

                double eachESum = 0;
                foreach(double eachE in midFlatBarList)
                {
                    eachESum += eachE;
                    Point3D eachLeftBottomPoint = GetSumPoint(DBottomRightPoint, eachESum, 0); // 시작점 매우 중요
                    double eachHeight = DHeight - valueService.GetOppositeByAdjacent(topSlopeDegree, eachESum);
                    newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(eachLeftBottomPoint, 0, 0),20, eachHeight, 0, 0, 2));
                }


                styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);


                // Right Mirror
                Plane mirrorPlane = Plane.YZ;
                mirrorPlane.Origin.X = tankBottomMidPoint.X;
                mirrorPlane.Origin.Y = tankBottomMidPoint.Y;
                List<Entity> newRightList = editingService.GetEntityByMirror(mirrorPlane, newList);
                styleService.SetLayerListEntity(ref newRightList, layerService.LayerHiddenLine);

                singleModel.Entities.AddRange(newList);
                singleModel.Entities.AddRange(newRightList);


                // RunWay
                // ID : 2/3 Point
                double runWayWidth = drawFRT.GetRollingLadderWidth(tankID);
                double runWayX = tankIDHalf * 2 / 3;
                Line vRunWayLine = new Line(GetSumPoint(referencePoint, runWayX, 0), GetSumPoint(referencePoint, runWayX, tankHeight));
                Point3D runWayPoint = null;
                foreach(Entity eachEntity in upperPlateList)
                {
                    Point3D eachPoint = editingService.GetIntersectWidth(vRunWayLine, (ICurve)eachEntity,0);
                    if (runWayPoint == null)
                        runWayPoint = eachPoint;
                    if (runWayPoint.Y < eachPoint.Y)
                        runWayPoint = eachPoint;
                }

                List<Entity> runWayList = drawFRT.DrawFRT_RunWay(GetSumPoint(runWayPoint, 0, 0), runWayWidth);
                // Rotate
                foreach (Entity eachEntity in runWayList)
                    eachEntity.Rotate(-topSlopeDegree, Vector3D.AxisZ, runWayPoint);

                singleModel.Entities.AddRange(runWayList);


                // Rolling Ladder
                double tankLastShellThickness = 24;
                Point3D rollingLadderStartPoint = GetSumPoint(referencePoint, -tankLastShellThickness, tankHeight);
                Point3D rollingLadderEndPoint = null;
                foreach (Entity eachEntity in runWayList)
                {
                    Point3D eachPoint = editingService.GetIntersectWidth(vRunWayLine, (ICurve)eachEntity, 0);
                    if (rollingLadderEndPoint == null)
                        rollingLadderEndPoint = eachPoint;
                    if (rollingLadderEndPoint.Y < eachPoint.Y)
                        rollingLadderEndPoint = eachPoint;
                }
                List<Entity> rollingLadderList = new List<Entity>();
                rollingLadderList.AddRange(drawFRT.DrawFRT_RollingLadder(rollingLadderStartPoint, rollingLadderEndPoint, selScaleValue));

                singleModel.Entities.AddRange(rollingLadderList);
            }

            // Orientation
            if (visibleFalse)
            {

                double selScaleValue = 90;
                double extMainLength = 4;
                double extSubLength = 2;


                List<Entity> newList = new List<Entity>();
                List<Point3D> newOutPoint = new List<Point3D>();

                Point3D referencePoint = new Point3D(10000, 10000);

                double tankID = 8000;
                double shellFirstThickness = 24;
                double tankOD = tankID + shellFirstThickness * 2;
                double tankODRadius = tankOD / 2;

                Point3D oriCenterPoint = GetSumPoint(referencePoint, tankODRadius, tankODRadius);

                // Outer
                Circle cirOD = new Circle(oriCenterPoint, tankODRadius);
                styleService.SetLayer(ref cirOD, layerService.LayerOutLine);
                newList.Add(cirOD);

                // Center Line
                List<Entity> centerVerticalLine=editingService.GetCenterLine(GetSumPoint(oriCenterPoint, 0, tankODRadius), GetSumPoint(oriCenterPoint, 0, -tankODRadius), extMainLength, selScaleValue);
                List<Entity> centerHorizontalLine = editingService.GetCenterLine(GetSumPoint(oriCenterPoint, -tankODRadius, 0), GetSumPoint(oriCenterPoint, tankODRadius, 0), extMainLength, selScaleValue);

                styleService.SetLayerListEntity(ref centerVerticalLine, layerService.LayerCenterLine);
                styleService.SetLayerListEntity(ref centerHorizontalLine, layerService.LayerCenterLine);
                newList.AddRange(centerVerticalLine);
                newList.AddRange(centerHorizontalLine);


                // Annular Type
                if (visibleFalse)
                {
                    double annularInWidth = 200;
                    double annularCirRadius = tankODRadius - annularInWidth;
                    Circle cirAnnular = new Circle(oriCenterPoint, annularCirRadius);
                    styleService.SetLayer(ref cirAnnular, layerService.LayerHiddenLine);
                    newList.Add(cirAnnular);

                    double circlePerimeter = valueService.GetCirclePerimeter(annularCirRadius);
                    double divWidth = 4000;//9144
                    double circleDivNum = Math.Ceiling(circlePerimeter / divWidth);
                    double circleDivOneWidth = circlePerimeter / circleDivNum;
                    double divOneRadius = valueService.GetRadianByArcLength(circleDivOneWidth, annularCirRadius);

                    double startDegree = 10;
                    double startRadian = Utility.DegToRad(startDegree);
                    double startRadianSum = startRadian;

                    for(int i=0;i<circleDivNum; i++)
                    {
                        Line eachLine = new Line(GetSumPoint(oriCenterPoint, 0, 0), GetSumPoint(oriCenterPoint, 0, tankODRadius));

                        eachLine.Rotate(-startRadianSum, Vector3D.AxisZ, GetSumPoint(oriCenterPoint, 0, 0));

                        Point3D[] interPoint = eachLine.IntersectWith(cirAnnular);
                        if (interPoint.Length > 0)
                            eachLine.TrimBy(interPoint[0], true);

                        styleService.SetLayer(ref eachLine, layerService.LayerHiddenLine);
                        newList.Add(eachLine);

                        startRadianSum += divOneRadius;
                    }
                }


                // Column
                if (visibleFalse)
                {
                    List<Tuple<double, double,double>> columnList = new List<Tuple<double, double,double>>();
                    columnList.Add(new Tuple<double, double,double>(1, 200,0));
                    columnList.Add(new Tuple<double, double,double>(5, 200,2000));
                    columnList.Add(new Tuple<double, double,double>(8, 200,4000));
                    columnList.Add(new Tuple<double, double, double>(14, 200, 6000));

                    foreach (Tuple<double,double,double> eachColumn in columnList)
                    {
                        double columnCount = eachColumn.Item1;
                        double columnPipeOD = eachColumn.Item2;
                        double columnRadius = eachColumn.Item3;
                        if (columnRadius == 0)
                        {
                            // Center : Only One
                            Circle cirColumnCenter = new Circle(GetSumPoint(oriCenterPoint, 0, 0), columnPipeOD / 2);
                            styleService.SetLayer(ref cirColumnCenter, layerService.LayerHiddenLine);
                            newList.Add(cirColumnCenter);
                        }
                        else
                        {
                            Circle cirColumnCenterLine = new Circle(GetSumPoint(oriCenterPoint, 0, 0), columnRadius / 2);
                            styleService.SetLayer(ref cirColumnCenterLine, layerService.LayerCenterLine);
                            newList.Add(cirColumnCenterLine);

                            double circlePerimeter = valueService.GetCirclePerimeter(columnRadius);
                            double circleDivOneWidth = circlePerimeter / columnCount;
                            double divOneRadius = valueService.GetRadianByArcLength(circleDivOneWidth, columnRadius);

                            List<Point3D> cirConnPointList = new List<Point3D>();
                            double startRadianSum = 0;
                            for (int i = 0; i < columnCount; i++)
                            {
                                Line eachLine = new Line(GetSumPoint(oriCenterPoint, 0, 0), GetSumPoint(oriCenterPoint, 0, columnRadius*2));
                                eachLine.Rotate(-startRadianSum, Vector3D.AxisZ, GetSumPoint(oriCenterPoint, 0, 0));

                                Point3D[] interPoint = eachLine.IntersectWith(cirColumnCenterLine);
                                if (interPoint.Length>0)
                                {
                                    cirConnPointList.Add(interPoint[0]);

                                    // Pipe
                                    Circle cirColumn = new Circle(GetSumPoint(interPoint[0], 0, 0), columnPipeOD / 2);
                                    styleService.SetLayer(ref cirColumn, layerService.LayerHiddenLine);
                                    newList.Add(cirColumn);

                                    Point3D[] cirInterPoint = eachLine.IntersectWith(cirColumn);
                                    if (cirInterPoint.Length > 0)
                                    {
                                        List<Entity> eachCirColumnCenterLine = editingService.GetCenterLine(GetSumPoint(cirInterPoint[0], 0, 0), GetSumPoint(cirInterPoint[1], 0, 0), extSubLength, selScaleValue);
                                        styleService.SetLayerListEntity(ref eachCirColumnCenterLine, layerService.LayerCenterLine);
                                        newList.AddRange(eachCirColumnCenterLine);
                                    }
                                }
                                startRadianSum += divOneRadius;
                            }

                            // Circle Connection
                            if (cirConnPointList.Count > 1)
                            {
                                List<Entity> circleConnLineList = new List<Entity>();
                                circleConnLineList.Add(new Line(cirConnPointList[0], cirConnPointList[cirConnPointList.Count-1]));
                                for (int i = 0; i < cirConnPointList.Count-1; i++)
                                {
                                    circleConnLineList.Add(new Line(cirConnPointList[i], cirConnPointList[i +1]));
                                }
                                styleService.SetLayerListEntity(ref circleConnLineList, layerService.LayerCenterLine);
                                newList.AddRange(circleConnLineList);
                            }


                        }
                    }
                }

                singleModel.Entities.AddRange(newList);



                DrawNozzleBlockService nozzleDraw = new DrawNozzleBlockService(null);

                double nozzleDegree = 130;
                double nozzleDegreeRadian = Utility.DegToRad(nozzleDegree);
                double nozzleRadius = 2300;

                Line nozzleLine = new Line(GetSumPoint(oriCenterPoint, 0, 0), GetSumPoint(oriCenterPoint, 0, nozzleRadius));
                nozzleLine.Rotate(-nozzleDegreeRadian, Vector3D.AxisZ,oriCenterPoint);

                List<Point3D> outputPointList = new List<Point3D>();
                List<Entity> newFlangeTop = nozzleDraw.DrawReference_Nozzle_Flange_Top(out outputPointList, GetSumPoint(nozzleLine.EndPoint,0,0), 0, 200, 300, 400, nozzleRadius, -nozzleDegreeRadian, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, exLength= extSubLength,arcEx=true,twoEx=true });

                Line nozzleLine2 = new Line(GetSumPoint(oriCenterPoint, 0, 0), GetSumPoint(oriCenterPoint, 0, nozzleRadius));
                nozzleLine2.Rotate(-Utility.DegToRad(80), Vector3D.AxisZ, oriCenterPoint);

                Line nozzleLine3 = (Line)nozzleLine2.Offset(1000, Vector3D.AxisZ);


                singleModel.Entities.Add(nozzleLine2);
                singleModel.Entities.Add(nozzleLine3);



                List<Entity> newFlangeTop1 = nozzleDraw.DrawReference_Nozzle_Flange_Top(out outputPointList, GetSumPoint(nozzleLine2.EndPoint, 0, 0), 0, 200, 300, 400, nozzleRadius, -Utility.DegToRad(80), 0, new DrawCenterLineModel() { scaleValue = selScaleValue, exLength = extSubLength, twoEx = true });
                List<Entity> newFlangeTop2 = nozzleDraw.DrawReference_Nozzle_Flange_Top(out outputPointList, GetSumPoint(nozzleLine3.EndPoint, 0, 0), 0, 200, 300, 400, nozzleRadius, -Utility.DegToRad(80), 0, new DrawCenterLineModel() { scaleValue = selScaleValue, exLength = extSubLength, twoEx = true });



                Line nozzleLine4 = new Line(GetSumPoint(oriCenterPoint, 0, 0), GetSumPoint(oriCenterPoint, 0, 5000));
                nozzleLine4.Rotate(-Utility.DegToRad(30), Vector3D.AxisZ, oriCenterPoint);



                
                List<Entity> testFlange = nozzleDraw.DrawReference_Nozzle_PipeTrimByCircle(out outputPointList, ref cirOD,  ref nozzleLine4, nozzleLine4.EndPoint, 100, new DrawCenterLineModel() { scaleValue = selScaleValue, exLength = extSubLength, twoEx = true });
                singleModel.Entities.AddRange(testFlange);

                DrawDimensionService newDim = new DrawDimensionService();

                NozzleInputModel newNozzle = new NozzleInputModel();
                newNozzle.Mark = "N1";
                newNozzle.Size = "NBBB";
                Dictionary<string,List<Entity>> newMark= newDim.DrawDimension_NozzleMark(ref nozzleLine4, true, nozzleLine4.EndPoint, newNozzle, selScaleValue);

                singleModel.Entities.Add(nozzleLine4);
                singleModel.Entities.AddRange(newMark[CommonGlobal.NozzleMark]);
                singleModel.Entities.AddRange(newMark[CommonGlobal.NozzleText]);

                singleModel.Entities.AddRange(newFlangeTop);
                singleModel.Entities.AddRange(newFlangeTop1);
                singleModel.Entities.AddRange(newFlangeTop2);
            }


            // Seal
            if (visibleFalse)
            {

                DrawFRTBlockService drawFRT = new DrawFRTBlockService();

                List<Point3D> newOutPoint = new List<Point3D>();
                List<Entity> newList = new List<Entity>();

                double shellLeftDistance = 200;
                double AThickness = 10;// 고정
                double bottomHeight = 50;
                double bottomWidth = 25;
                double rightHeight = 155;

                Point3D sealPoint = new Point3D(10000, 10000);

                Point3D cBottomLeftPoint = GetSumPoint(sealPoint, AThickness, -bottomHeight);
                Point3D cBottomRightPoint = GetSumPoint(cBottomLeftPoint,  bottomWidth, 0);
                Point3D cTopRightPoint = GetSumPoint(cBottomRightPoint,0 , rightHeight);

                Point3D cTopCurveMidPoint = GetSumPoint(cTopRightPoint, -120 - bottomWidth - AThickness,100);
                Point3D cTopCurveEndPoint = GetSumPoint(cTopCurveMidPoint, -80, 155);

                Line leftSheetLine = new Line(GetSumPoint(sealPoint, -shellLeftDistance, 0), GetSumPoint(sealPoint, -shellLeftDistance, 1000));

                // First Courve
                Line cBottomLine = new Line(GetSumPoint(cBottomLeftPoint, 0, 0), GetSumPoint(cBottomRightPoint, 0, 0));
                Line cRightLine = new Line(GetSumPoint(cBottomRightPoint, 0, 0), GetSumPoint(cTopRightPoint, 0, 0));

                Arc cUpperArc = new Arc(Plane.XY,cTopRightPoint, cTopCurveMidPoint, cTopCurveEndPoint,true);
                Arc cUpperArcFillet = null;
                Curve.Fillet(cUpperArc, cRightLine, 50, true, false, true, true, out cUpperArcFillet);

                Arc cLowerArc = (Arc)cUpperArc.Offset(bottomWidth, Vector3D.AxisZ);
                Point3D cLeftInter = editingService.GetIntersectWidth(leftSheetLine, cLowerArc, 0);
                cLowerArc.TrimBy(cLeftInter, true);
                Arc cLowerArcFillet = (Arc)cUpperArcFillet.Offset(bottomWidth, Vector3D.AxisZ);
                Line cLeftLine = new Line(GetSumPoint(cLowerArcFillet.EndPoint, 0, 0), GetSumPoint(cBottomLeftPoint, 0, 0));

                // Second Courve
                Arc cLowerLowerArc = new Arc(Plane.XY, GetSumPoint(sealPoint,0,0), GetSumPoint(sealPoint,-135,65), GetSumPoint(sealPoint, -200, 196), true);
                newList.AddRange(new Entity[] { cBottomLine, cRightLine, cUpperArc, cUpperArcFillet,cLowerArc, cLowerArcFillet, cLeftLine, cLowerLowerArc });


                Point3D lowerShapeWP = GetSumPoint(sealPoint, -shellLeftDistance, -50);
                Line uTopLine = new Line(GetSumPoint(lowerShapeWP, 0, 0), GetSumPoint(lowerShapeWP, 45, 0));

                Line uTopRightSlopeLine = new Line(GetSumPoint(uTopLine.EndPoint, 0, 0), GetSumPoint(uTopLine.EndPoint, 30, -30));
                Line uTopRightLine = new Line(GetSumPoint(uTopRightSlopeLine.EndPoint, 0, 0), GetSumPoint(uTopRightSlopeLine.EndPoint, 0, -70));
                Line uTopBottomLine = new Line(GetSumPoint(uTopRightLine.EndPoint, 0, 0), GetSumPoint(uTopRightLine.EndPoint, -75, 0));

                Point3D uTopRecPoint = GetSumPoint(uTopLine.EndPoint, 0, -48);
                List<Entity> uTopRec=shapeService.GetRectangle(out newOutPoint, GetSumPoint(uTopRecPoint, 0, 0), 25, 550, Utility.DegToRad(15), 1, 1);
                newList.AddRange(uTopRec);
                newList.AddRange(new Entity[] { uTopLine, uTopRightSlopeLine, uTopRightLine});

                // utopRec 겹침
                List<Point3D> uTopRecPointList = new List<Point3D>();
                foreach(Entity eachEntity in uTopRec)
                {
                    Point3D eachInter = editingService.GetIntersectWidth(uTopBottomLine,(ICurve)eachEntity, 0);
                    if (eachInter.Y > 0)
                        uTopRecPointList.Add(GetSumPoint(eachInter, 0, 0));
                }
                List<Point3D> uTopRecPointListSort=uTopRecPointList.OrderBy(x => x.X).ToList();
                Line uTopBottomLine1 = new Line(GetSumPoint(uTopBottomLine.EndPoint, 0, 0), GetSumPoint(uTopRecPointListSort[0], 0, 0));
                Line uTopBottomLine2 = new Line(GetSumPoint(uTopBottomLine.StartPoint, 0, 0), GetSumPoint(uTopRecPointListSort[1], 0, 0));
                newList.AddRange(new Entity[] { uTopBottomLine1, uTopBottomLine2 });

                // Bottom
                Point3D sealBottonPoint = GetSumPoint(sealPoint, -shellLeftDistance, -800 + 50);
                Line bBottomLine1 = new Line(GetSumPoint(sealBottonPoint, 0, 0), GetSumPoint(sealBottonPoint, shellLeftDistance, 0));
                Line bBottomLine2 = new Line(GetSumPoint(sealBottonPoint, 0, 50), GetSumPoint(sealBottonPoint, shellLeftDistance, 50));

                newList.AddRange(new Entity[] { bBottomLine1, bBottomLine2 });

                // uShape
                Point3D uBottomShape = GetSumPoint(sealBottonPoint, shellLeftDistance, 50 + 500);
                Line uBottomTop= new Line(GetSumPoint(uBottomShape, 0, 0), GetSumPoint(uBottomShape, -55, 0));
                Line uBottomSlope = new Line(GetSumPoint(uBottomTop.EndPoint, 0, 0), GetSumPoint(uBottomTop.EndPoint, -30, -30));
                
                Line bBottomLeft = new Line(GetSumPoint(uBottomSlope.EndPoint, 0, 0), GetSumPoint(uBottomSlope.EndPoint, 0, -1000));

                // uBottomRec
                Point3D uBottomRecPoint = GetSumPoint(uBottomTop.EndPoint, 0, -25);
                List<Entity> uBottomRec = shapeService.GetRectangle(out newOutPoint, GetSumPoint(uBottomRecPoint, 0, 0), 25, 310, -Utility.DegToRad(27), 0, 0);
                newList.AddRange(uBottomRec);
                newList.AddRange(new Entity[] { uBottomTop, uBottomSlope });

                Line bBottomRight = new Line(GetSumPoint(uBottomRecPoint, 0, -500+25), GetSumPoint(uBottomRecPoint, 0, 0));

                // uLeft 겹침
                // utopRec 겹침
                List<Point3D> uBottomRecPointList = new List<Point3D>();
                foreach (Entity eachEntity in uBottomRec)
                {
                    Point3D eachInter = editingService.GetIntersectWidth(bBottomLeft, (ICurve)eachEntity, 0);
                    if (eachInter.Y > 0)
                        uBottomRecPointList.Add(GetSumPoint(eachInter, 0, 0));
                }
                List<Point3D> uBottomRecPointListSort = uBottomRecPointList.OrderBy(x => x.Y).ToList();
                Line bBottomLeft1 = new Line(GetSumPoint(uBottomSlope.EndPoint, 0, 0), GetSumPoint(uBottomRecPointListSort[1], 0, 0));

                newList.AddRange(new Entity[] { bBottomLeft1 });

                // 맨마지막 겹침
                List<Point3D> uBottomRecPointList1 = new List<Point3D>();
                foreach (Entity eachEntity in uBottomRec)
                {
                    Point3D eachInter = editingService.GetIntersectWidth(bBottomRight, (ICurve)eachEntity, 0);
                    if (eachInter.Y > 0)
                        uBottomRecPointList1.Add(GetSumPoint(eachInter, 0, 0));
                }
                List<Point3D> uBottomRecPointListSort1= uBottomRecPointList1.OrderBy(x => x.Y).ToList();


                List<Point3D> uBottomRecPointList2 = new List<Point3D>();
                foreach (Entity eachEntity in uTopRec)
                {
                    Point3D eachInter = editingService.GetIntersectWidth(bBottomRight, (ICurve)eachEntity, 0);
                    if (eachInter.Y > 0)
                        uBottomRecPointList2.Add(GetSumPoint(eachInter, 0, 0));
                }
                List<Point3D> uBottomRecPointListSort2 = uBottomRecPointList2.OrderBy(x => x.Y).ToList();


                Line bBottomRight1 = new Line(GetSumPoint(bBottomRight.StartPoint, 0,0), GetSumPoint(uBottomRecPointListSort2[0], 0, 0));
                Line bBottomRight2 = new Line(GetSumPoint(uBottomRecPointListSort2[1], 0,0), GetSumPoint(uBottomRecPointListSort1[0], 0, 0));
                newList.AddRange(new Entity[] { bBottomRight1, bBottomRight2 });


                styleService.SetLayerListEntity(ref newList, layerService.LayerVirtualLine);
                singleModel.Entities.AddRange(newList);
            }

            if (visibleFalse) 
            {
                double selScaleValue = 90;
                Point3D referencePoint = new Point3D(10000, 10000);

                List<Point3D> newOutPoint = new List<Point3D>();
                List<Entity> newList = new List<Entity>();
                List<Entity> newCenterList = new List<Entity>();

                //newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(EBottomLeftPoint, 0, 0), pontoonRightFlatWidth, pontoonThkE, 0, 0, 3));
                //newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(FTopLeftPoint, 0, 0), pontoonFlatLength, pontoonThkF, 0, 0, 0, new bool[] { true, false, true, true }));

                double bottomGap =30;



                double topPadHeight = 10;
                double topPadWidth = 100 + 19 + 20;

                double padUpHeight = 115;
                double padUpWidth = 100;

                double padUpUpHeight = 1600 - 620 - 110 - 115;
                double padUpUpWidth = padUpWidth - (20 * 2);

                double middleUpHeight = 110;
                double middleUpWidth = 330;

                double middleUpPadHeight = 10;
                double middleUpPadWidth = middleUpWidth - (10 * 2);

                double middleHeight = 650;
                double middleWidth = middleUpWidth - (30 * 2);

                double bottomHeight = 1600 - bottomGap;
                double bottomWidth = 73;

                Point3D refAdjPoint = GetSumPoint(referencePoint, 0, -bottomGap);
                double currentY = - bottomHeight;
                newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(refAdjPoint, -bottomWidth / 2, currentY), bottomWidth, bottomHeight, 0, 0, 3, new bool[] { false, true, true, true }));
                currentY += bottomHeight;
                newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(refAdjPoint, -middleWidth / 2, currentY), middleWidth, middleHeight, 0, 0, 3, new bool[] { false, true, true, true }));
                currentY += middleHeight;
                newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(refAdjPoint, -middleUpWidth / 2, currentY), middleUpWidth, middleUpHeight, 0, 0, 3, new bool[] { true, true, true, true }));
                currentY += middleUpHeight;
                newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(refAdjPoint, -middleUpPadWidth / 2, currentY), middleUpPadWidth, middleUpPadHeight, 0, 0, 3, new bool[] { true, true, false, true }));
                currentY += middleUpPadHeight;
                newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(refAdjPoint, -padUpWidth / 2, currentY), padUpWidth, padUpHeight, 0, 0, 3, new bool[] { true, true, false, true }));
                currentY += padUpHeight;
                newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(refAdjPoint, -padUpUpWidth / 2, currentY), padUpUpWidth, padUpUpHeight, 0, 0, 3, new bool[] { false, true, false, true }));
                currentY += padUpUpHeight;
                newList.AddRange(shapeService.GetRectangle(out newOutPoint, GetSumPoint(refAdjPoint, -topPadWidth / 2, currentY), topPadWidth, topPadHeight, 0, 0, 3, new bool[] { true, true, true, true }));
                currentY += topPadHeight;




                double topHeight = 90;
                double topRadius = 50;
                double topWidth = 100;
                double topPipeWidth = 19;
                Arc topCenterArc = new Arc(Plane.XY, GetSumPoint(refAdjPoint, -topWidth/2, currentY + topHeight), GetSumPoint(refAdjPoint, 0, currentY + topHeight + topRadius), GetSumPoint(refAdjPoint,topWidth/2, currentY + topHeight),true);
                Line topLeftLine = new Line(GetSumPoint(refAdjPoint, -topWidth/2, currentY), GetSumPoint(refAdjPoint, -topWidth/2, currentY +topHeight));
                Line topRightLine = new Line(GetSumPoint(refAdjPoint, topWidth/2, currentY), GetSumPoint(refAdjPoint, topWidth/2, currentY +topHeight));
                newCenterList.AddRange(new Entity[] { topCenterArc, topLeftLine, topRightLine });

                Arc topCenterArcInner = (Arc)topCenterArc.Offset(-topPipeWidth / 2, Vector3D.AxisZ);
                Line topLeftLineInner = (Line)topLeftLine.Offset(topPipeWidth / 2, Vector3D.AxisZ);
                Line topRightInnertLine = (Line)topRightLine.Offset(-topPipeWidth / 2, Vector3D.AxisZ);
                Arc topCenterArcOuter = (Arc)topCenterArc.Offset(topPipeWidth / 2, Vector3D.AxisZ);
                Line topLeftLineOuter = (Line)topLeftLine.Offset(-topPipeWidth / 2, Vector3D.AxisZ);
                Line topRightInnertOuter = (Line)topRightLine.Offset(+topPipeWidth / 2, Vector3D.AxisZ);
                newList.AddRange(new Entity[] { topCenterArcInner, topLeftLineInner, topRightInnertLine, topCenterArcOuter, topLeftLineOuter, topRightInnertOuter });
                // CenterLine
                DrawCenterLineModel newCenterModel = new DrawCenterLineModel();
                List<Entity> centerLongLine = editingService.GetCenterLine(GetSumPoint(refAdjPoint, 0, -bottomHeight), GetSumPoint(refAdjPoint, 0, currentY + topHeight + topRadius +(topPipeWidth/2)), newCenterModel.exLength,selScaleValue);
                List<Entity> centerLongLine2 = editingService.GetCenterLine(GetSumPoint(refAdjPoint, -(topWidth / 2) -(topPipeWidth/2), currentY + topHeight), GetSumPoint(refAdjPoint, +(topWidth / 2) + (topPipeWidth / 2), currentY + topHeight), newCenterModel.exLength, selScaleValue);
                newCenterList.AddRange(centerLongLine);
                newCenterList.AddRange(centerLongLine2);

                styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);
                styleService.SetLayerListEntity(ref newCenterList, layerService.LayerCenterLine);

                singleModel.Entities.AddRange(newList);
                singleModel.Entities.AddRange(newCenterList);
            }

            if (visibleFalse)
            {
                Point3D referencePoint = new Point3D(1000, 1000);
                //List<Entity> tempList = CreateNozzleProjection(referencePoint);

                List<Entity> newList = new List<Entity>();

                double circleBox = 40;
                double circleOD = 22.5;
                double circleRadius = circleOD / 2;

                Point3D circleCenterPoint = GetSumPoint(referencePoint, circleBox / 2, circleBox / 2);
                Circle centerCircle = new Circle(circleCenterPoint, circleRadius);
                newList.Add(centerCircle);


                Point3D circleCenterTopPoint = GetSumPoint(circleCenterPoint, 0, circleRadius);
                Line centerLine= new Line(GetSumPoint(circleCenterPoint, 0, circleRadius), GetSumPoint(circleCenterPoint, 0, -circleRadius));
                Line leftLine1 = (Line)centerLine.Clone();
                leftLine1.Rotate(Utility.DegToRad(15), Vector3D.AxisZ, circleCenterTopPoint);
                Line rightLine1 = (Line)centerLine.Clone();
                rightLine1.Rotate(-Utility.DegToRad(15), Vector3D.AxisZ, circleCenterTopPoint);

                Point3D leftLineIntetr = editingService.GetIntersectWidth(leftLine1, centerCircle, 1);
                Point3D rightLineIntetr = editingService.GetIntersectWidth(rightLine1, centerCircle, 1);

                Line centerSlope = new Line(GetSumPoint(circleCenterTopPoint, 0, 0), GetSumPoint(leftLineIntetr, 0, 0));
                centerSlope.Rotate(Utility.DegToRad(30), Vector3D.AxisZ, leftLineIntetr);
                Point3D centerLineIntetr = editingService.GetIntersectWidth(centerSlope, centerLine, 0);

                Line dLine1 = new Line(GetSumPoint(circleCenterTopPoint, 0, 0), GetSumPoint(leftLineIntetr, 0, 0));
                Line dLine2 = new Line(GetSumPoint(circleCenterTopPoint, 0, 0), GetSumPoint(rightLineIntetr, 0, 0));
                Line dLine3 = new Line(GetSumPoint(centerLineIntetr, 0, 0), GetSumPoint(leftLineIntetr, 0, 0));
                Line dLine4 = new Line(GetSumPoint(centerLineIntetr, 0, 0), GetSumPoint(rightLineIntetr, 0, 0));
                Line dLine5= new Line(GetSumPoint(circleCenterTopPoint, 0, 0), GetSumPoint(centerLineIntetr, 0, 0));

                newList.AddRange(new Line[] { dLine1, dLine2, dLine3, dLine4 });


                styleService.SetLayerListEntity(ref newList, layerService.LayerDimension);



                // Center Line
                double exLength = 2;
                List<Entity> centerLineList = new List<Entity>();
                centerLineList.AddRange(editingService.GetCenterLine(GetSumPoint(circleCenterPoint, 0, circleRadius), GetSumPoint(circleCenterPoint, 0, -circleRadius), exLength, 1));
                centerLineList.AddRange(editingService.GetCenterLine(GetSumPoint(circleCenterPoint, circleRadius, 0), GetSumPoint(circleCenterPoint, -circleRadius, 0), exLength, 1));
                styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
                newList.AddRange(centerLineList);

                // hatch
                Triangle cc = new Triangle(circleCenterTopPoint, leftLineIntetr, centerLineIntetr);
                
                //HatchRegion hatchDirection = new HatchRegion((ICurve)cc );
                //List<ICurve> hatchLine = new List<ICurve>();
                //hatchLine.AddRange(new Line[] { dLine1, dLine2, dLine5 });
                //HatchRegion hatchDirection = new HatchRegion(hatchLine,Plane.XY);
                //hatchDirection.Color = Color.White;
                //hatchDirection.HatchName = "aa";

                //CustomRenderedHatch hhh = new CustomRenderedHatch(hatchDirection);
                //newList.Add(hatchDirection);
                // Text

                double extLength = 4;
                double textHeight = 2.5;
                List<Entity> textList = new List<Entity>();
                Text Text01 = new Text(GetSumPoint(circleCenterPoint, 0, circleRadius + extLength), "0˚", textHeight);
                Text Text02 = new Text(GetSumPoint(circleCenterPoint, circleRadius + extLength +2, 0), "90˚", textHeight);
                Text Text03 = new Text(GetSumPoint(circleCenterPoint, 0, -(circleRadius + extLength )), "180˚", textHeight);
                Text Text04 = new Text(GetSumPoint(circleCenterPoint, -(circleRadius + extLength+2), 0), "270˚", textHeight);
                textList.AddRange(new Text[] { Text01, Text02, Text03, Text04 });
                foreach(Text eachText in textList)
                {
                    eachText.ColorMethod = colorMethodType.byEntity;
                    eachText.Color = Color.Yellow;
                    eachText.LayerName = layerService.LayerDimension;
                    eachText.Alignment = Text.alignmentType.MiddleCenter;
                }

                Text TextTop = new Text(GetSumPoint(circleCenterPoint, 0, circleRadius + extLength +5), "P.N", 3.75);
                TextTop.Alignment = Text.alignmentType.MiddleCenter;
                styleService.SetLayer(ref TextTop, layerService.LayerDimension);
                textList.Add(TextTop);
                newList.AddRange(textList);



                singleModel.Entities.AddRange(newList);
            }

            if (visibleFalse)
            {
                Point3D referencePoint = new Point3D(1000, 1000);
                Line newLine = new Line(GetSumPoint(referencePoint, 0, 0),GetSumPoint(referencePoint, 1000, 0));
                Point3D[] xx = newLine.GetPointsByLength(500);

                Line newLine2 = new Line(GetSumPoint(xx[1], 0, 0), GetSumPoint(xx[1],0 , 100));


                Line newLine11 = new Line(GetSumPoint(referencePoint, 0, 0), GetSumPoint(referencePoint, 1000, 0));
                newLine11.Rotate(-Utility.DegToRad(20), Vector3D.AxisZ, GetSumPoint(referencePoint, 0, 0));
                Point3D[] xx1 = newLine11.GetPointsByLength(500);
                Line newLine22 = new Line(GetSumPoint(xx1[1], 0, 0), GetSumPoint(xx1[1], 0, 500));

                singleModel.Entities.Add(newLine);
                singleModel.Entities.Add(newLine2);
                singleModel.Entities.Add(newLine11);
                singleModel.Entities.Add(newLine22);
            }

            if (visibleFalse)
            {

                Point3D refPoint = new Point3D(1000, 1000);

                List<Entity> newList = new List<Entity>();
                List<Entity> newTextList = new List<Entity>();
                List<Tuple<string, string>> textList = new List<Tuple<string, string>>();


                textList.Add(new Tuple<string, string>("SP","ROOF STRUCTURE CENTERLINE"));
                textList.Add(new Tuple<string, string>("NP", "ROOF PLATE CENTERLINE"));
                textList.Add(new Tuple<string, string>("EL", "BOTTOM PLATE CENTERLINE"));
                textList.Add(new Tuple<string, string>("ST", "SPIRAL STARIRWAY START POINT"));
                textList.Add(new Tuple<string, string>("SS", "SETTLEMENT CHECK PIECE START POINT (10EA)"));
                textList.Add(new Tuple<string, string>("BC", "EARTH LUG START POINT (6EA)"));
                textList.Add(new Tuple<string, string>("RC", "NAME PLATE"));
                textList.Add(new Tuple<string, string>("RS", "1st COURSE SHELL PLATE START POINT"));


                double textHeight = 2.5;
                double cirDia = 11.5470;
                double cirRadius = cirDia / 2;
                double leftGap = 10;
                double currentY = cirRadius;
                foreach(Tuple<string, string> eachTuple in textList)
                {
                    Point3D referencePoint = GetSumPoint(refPoint, cirRadius, currentY);
                    Circle eachCircle = new Circle(GetSumPoint(referencePoint, 0, 0), cirRadius);
                    Line vLine1 = new Line(GetSumPoint(referencePoint, 0, cirDia), GetSumPoint(referencePoint, 0, -cirDia));
                    Line vLine2 = (Line)vLine1.Clone();
                    Line vLine3 = (Line)vLine1.Clone();
                    vLine1.Rotate(Utility.DegToRad(30), Vector3D.AxisZ, GetSumPoint(referencePoint, 0, 0));
                    vLine2.Rotate(-Utility.DegToRad(30), Vector3D.AxisZ, GetSumPoint(referencePoint, 0, 0));
                    vLine3.Rotate(-Utility.DegToRad(90), Vector3D.AxisZ, GetSumPoint(referencePoint, 0, 0));

                    Point3D[] vInter1 = eachCircle.IntersectWith(vLine1);
                    Point3D[] vInter2 = eachCircle.IntersectWith(vLine2);
                    Point3D[] vInter3 = eachCircle.IntersectWith(vLine3);

                    Text eachText = new Text(GetSumPoint(referencePoint, 0, 0), eachTuple.Item1, 3.5);
                    eachText.Alignment = Text.alignmentType.MiddleCenter;
                    styleService.SetLayer(ref eachText, layerService.LayerDimension);
                    newTextList.Add(eachText);

                    Text eachText2 = new Text(GetSumPoint(referencePoint, leftGap, 0), eachTuple.Item2, textHeight);
                    eachText2.Alignment = Text.alignmentType.MiddleLeft;
                    styleService.SetLayer(ref eachText2, layerService.LayerDimension);
                    newTextList.Add(eachText2);

                    newList.Add(new Line(GetSumPoint(vInter1[0], 0, 0), GetSumPoint(vInter2[0], 0, 0)));
                    newList.Add(new Line(GetSumPoint(vInter2[0], 0, 0), GetSumPoint(vInter3[1], 0, 0)));
                    newList.Add(new Line(GetSumPoint(vInter3[1], 0, 0), GetSumPoint(vInter1[1], 0, 0)));
                    newList.Add(new Line(GetSumPoint(vInter1[1], 0, 0), GetSumPoint(vInter2[1], 0, 0)));
                    newList.Add(new Line(GetSumPoint(vInter2[1], 0, 0), GetSumPoint(vInter3[0], 0, 0)));
                    newList.Add(new Line(GetSumPoint(vInter3[0], 0, 0), GetSumPoint(vInter1[0], 0, 0)));

                    styleService.SetLayerListEntity(ref newList, layerService.LayerDimension);
                    newList.AddRange(newTextList);

                    currentY += cirDia + 0.5;
                }




                singleModel.Entities.AddRange(newList);
            }

            if (visibleFalse)
            {
                Point3D refPoint = new Point3D(100000, 100000);

                List<Point3D> outList = new List<Point3D>();
                List<Entity> newList = new List<Entity>();
                double refRadius = 40000;
                Circle refCircle = new Circle(GetSumPoint(refPoint, 0, 0), refRadius);

                double refWidth = 2000;
                double refHeight = 1000;

                double currentY = 0;
                double currentX = 0;
                double maxHeight = refPoint.Y + refRadius;

                bool exitRoof = false;
                do 
                {
                    //가로
                    List<Entity> eachRec=shapeService.GetRectangle(out outList, GetSumPoint(refPoint, currentX, currentY), refWidth, refHeight, 0, 0, 4);
                    currentY += refHeight;
                    //세로
                }
                while (exitRoof);

                singleModel.Entities.AddRange(newList);

            }


            // Arrow : 화살표
            if (visibleFalse)
            {
                List<Entity> arrowList = new List<Entity>();
                double selArrowWidth = 2.5;
                double selArrowHeight = 0.8;
                double selArrowHeightHalf = selArrowHeight / 2;

                Point3D selArrowPoint = new Point3D(30, 30);

                // Arrow
                List<Point3D> newPoint = new List<Point3D>();
                newPoint.Add(selArrowPoint);
                newPoint.Add(new Point3D(selArrowPoint.X + selArrowWidth, selArrowPoint.Y + selArrowHeightHalf));
                newPoint.Add(new Point3D(selArrowPoint.X + selArrowWidth, selArrowPoint.Y - selArrowHeightHalf));
                newPoint.Add(selArrowPoint);
                LinearPath arrowPath = new LinearPath(newPoint);

                arrowList.Add(arrowPath);



                // Horizontal
                int divHeightNumber = 8;
                double divHeight = selArrowHeight / divHeightNumber;
                for (int i = 1; i < divHeightNumber; i++)
                    arrowList.Add(new Line(selArrowPoint, new Point3D(selArrowPoint.X + selArrowWidth, selArrowPoint.Y - selArrowHeightHalf + (divHeight * i))));

                // Vertical
                int divWidthNumber = 9;
                double divWidth = selArrowWidth / divWidthNumber;
                for (int i = 1; i < divWidthNumber; i++)
                {
                    Line tempVLine = new Line(new Point3D(selArrowPoint.X + (divWidth * i), selArrowPoint.Y + selArrowHeight), new Point3D(selArrowPoint.X + (divWidth * i), selArrowPoint.Y - selArrowHeight));
                    Point3D[] tempInterPoint = arrowPath.IntersectWith(tempVLine);
                    arrowList.Add(new Line(tempInterPoint[0], tempInterPoint[1]));


                }

                singleModel.Entities.AddRange(arrowList);
            }

            // Break Test
            if (visibleFalse)
            {
                Point3D refref = new Point3D(10000, 10000);
                Point3D p01 = GetSumPoint(refref, 100, 100);
                Point3D p02 = GetSumPoint(refref, 200, 150);
                Point3D p03 = GetSumPoint(refref, 300, 100);

                Arc arc01 = new Arc(p01, p02, p03,false);

                Line line01 = new Line(GetSumPoint(refref, 100, 0), GetSumPoint(refref, 300, 150));

                Point3D[] lineinter = arc01.IntersectWith(line01);

                arc01.TrimBy(lineinter[0], false);

                Circle circle01 = new Circle(GetSumPoint(refref, 100, 100), 40);


                Point3D[] lineinter2 = arc01.IntersectWith(circle01);

                ICurve sp01 = null;
                ICurve sp02 = null;
                arc01.SplitAt(4, out sp01, out sp02);

                Point3D[] pol01 = arc01.GetPointsByLength(40);

                arc01.TrimBy(pol01[1], false);

                singleModel.Entities.Add(arc01);
                singleModel.Entities.Add(line01);
                singleModel.Entities.Add(circle01);




            }


            if (visibleFalse)
            {
                double customScaleValue = 3;

                DrawBreakSymbols shapeService = new DrawBreakSymbols();

                Point3D refPoint = new Point3D(1000, 1000);
                double selWidth = 50;
                double selLength = 1000;

                Point3D sLinePoint = GetSumPoint(refPoint, 0, 0);


                List<Entity> lineList = new List<Entity>();
                Line line01 = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, 0, -selLength));
                //Line line02 = new Line(GetSumPoint(refPoint, selWidth, 0), GetSumPoint(refPoint, selWidth, -selLength));
                Line line02 = new Line(GetSumPoint(refPoint, selWidth, -selLength), GetSumPoint(refPoint, selWidth, 0));
                lineList.Add(line01);
                lineList.Add(line02);



                List<Entity> newSLineList = shapeService.GetSLine(GetSumPoint(sLinePoint,0,-50), selWidth,true);
                shapeService.SetTrimSLine(ref lineList, ref newSLineList, GetSumPoint(sLinePoint, 0, -2000));
                List<Entity> newSLineList2 = shapeService.GetSLine(GetSumPoint(sLinePoint, 0, -650), selWidth);
                shapeService.SetTrimSLine(ref lineList, ref newSLineList2, GetSumPoint(sLinePoint, 0, 0000));


                List<Entity> newSDoubleLineList = shapeService.GetSDoubleLine(GetSumPoint(sLinePoint, selWidth/2, -300), selWidth,10);

                List<Entity> newListList = shapeService.GetTrimSDoubleLine(lineList, newSDoubleLineList);


                // Chamfer

                double oneCourseHeight = 200;

                DrawShellCourses dsService = new DrawShellCourses();

                List<Point3D> exPoint = new List<Point3D>();

                DrawWeldingModel dwm01 = new DrawWeldingModel()
                {
                    BottomWelding = true,
                    BottomWeldingSingle = false,
                    LeftDegree = 45,
                    RightDegree = 30,
                    LeftDepth = 20,
                    RightDepth = 10,
                    OtherDistance = 3.2,
                    MidDepth = 1.6,
                    LeftWeldingArc = true,
                    RightWeldingArc = true,
                    TopChamfer = true,
                    ChamferShort = 5,
                    ChamferLong= 5 * 3

                };
                DrawWeldingModel dwm02 = new DrawWeldingModel()
                {
                    BottomWelding = true,
                    BottomWeldingSingle = false,
                    LeftDegree = 45,
                    RightDegree = 30,
                    LeftDepth = 20,
                    RightDepth = 10,
                    OtherDistance = 3.2,
                    MidDepth = 1.6,
                    LeftWeldingArc = true,
                    RightWeldingArc = true,
                    TopChamfer = true,
                    ChamferShort = 4,
                    ChamferLong = 4 * 3

                };

                DrawSlopeLeaderModel outSlopeModel = new DrawSlopeLeaderModel();
                List<Entity> chamferList = dsService.GetPlateBlock(out exPoint, refPoint, 34, oneCourseHeight, 0, 0, 0, dwm01, new DrawCenterLineModel() { scaleValue = customScaleValue },customScaleValue, out outSlopeModel);

                List<Entity> chamferList2 = dsService.GetPlateBlock(out exPoint, exPoint[1], 20, oneCourseHeight, 0, 0, 2, dwm02, new DrawCenterLineModel() { scaleValue = customScaleValue },customScaleValue,out outSlopeModel);

                List<Entity> cahmSDoubleLineList = shapeService.GetSDoubleLine(GetSumPoint(refPoint, 30 / 2, -oneCourseHeight/2), 30, customScaleValue);

                List<Entity> newchamferList = shapeService.GetTrimSDoubleLine(chamferList,cahmSDoubleLineList);


                styleService.SetLayerListEntityExcludingCenterLine(ref newchamferList, layerService.LayerOutLine);
                singleModel.Entities.AddRange(newchamferList);
                styleService.SetLayerListEntityExcludingCenterLine(ref chamferList2, layerService.LayerOutLine);
                singleModel.Entities.AddRange(chamferList2);

                singleModel.Entities.AddRange(cahmSDoubleLineList);

                singleModel.Entities.AddRange(newListList);
                singleModel.Entities.AddRange(newSLineList);
                singleModel.Entities.AddRange(newSLineList2);
                singleModel.Entities.AddRange(newSDoubleLineList);

                if (false)
                {
                    DrawEntityModel dimList = new DrawEntityModel();
                    DrawService dService = new DrawService(null);
                    DrawEntityModel testDim01 = dService.Draw_Dimension(GetSumCDPoint(refPoint, 0, -oneCourseHeight), GetSumCDPoint(refPoint, 0, 0), null, "left", 300, 2.5, 2.5, 2.5, "", "", "D", 0, customScaleValue, layerService.LayerDimension);
                    dimList.AddDrawEntity(testDim01);

                    DrawEntityModel testDimArc01 = dService.Draw_DimensionArc(GetSumPoint(refPoint, 0, -30), GetSumPoint(refPoint, 0, 0), "bottom", 50, "D", 45, 120, 100, 0, customScaleValue, layerService.LayerDimension);
                    dimList.AddDrawEntity(testDimArc01);



                    DrawDimensionModel dimModel = new DrawDimensionModel() { position = POSITION_TYPE.LEFT, textUpper = "ABaEEEEEEE", textLower = "EbbEEEEEEE", dimHeight = 40, scaleValue = customScaleValue };
                    DrawEntityModel testDimTest01 = dService.Draw_DimensionDetail(ref singleModel, GetSumPoint(refPoint, 0, -30), GetSumPoint(refPoint, 0, 0), customScaleValue, dimModel);
                    dimList.AddDrawEntity(testDimTest01);


                    DrawDimensionModel dimModel2 = new DrawDimensionModel() { position = POSITION_TYPE.RIGHT, textUpper = "ABaEEEEEEE", textLower = "EbbEEEEEEE", dimHeight = 40, scaleValue = customScaleValue };
                    DrawEntityModel testDimTest02 = dService.Draw_DimensionDetail(ref singleModel, GetSumPoint(refPoint, 0, -30), GetSumPoint(refPoint, 0, 0), customScaleValue, dimModel2);
                    dimList.AddDrawEntity(testDimTest02);


                    DrawDimensionModel dimModel3 = new DrawDimensionModel()
                    {
                        position = POSITION_TYPE.TOP,
                        textUpper = "ABaEEEEEEE",
                        textLower = "EEEEE",
                        arrowLeftHeadOut = true,
                        arrowRightHeadOut = true,
                        textUpperPosition = POSITION_TYPE.RIGHT,
                        arrowRightSymbol = DimHead_Type.Circle,
                        leftBMNumber = "4",
                        textSizeVisible = false,
                        dimHeight = 40,
                        scaleValue = customScaleValue
                    };
                    DrawEntityModel testDimTest03 = dService.Draw_DimensionDetail(ref singleModel, GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, 30, 0), customScaleValue, dimModel3);
                    dimList.AddDrawEntity(testDimTest03);


                    DrawDimensionModel dimModel4 = new DrawDimensionModel() { position = POSITION_TYPE.BOTTOM, textUpper = "ABaEEEEEEE", textLower = "EbbEEEEEEE", dimHeight = 40, scaleValue = customScaleValue };
                    DrawEntityModel testDimTest04 = dService.Draw_DimensionDetail(ref singleModel, GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, 30, 0), customScaleValue, dimModel4);
                    dimList.AddDrawEntity(testDimTest04);


                    DrawBMLeaderModel dimLeadder1 = new DrawBMLeaderModel() { position = POSITION_TYPE.LEFT, upperText = "EEEEEE", lowerText = "bbbb", bmNumber = "4" };
                    DrawEntityModel testDimTest05 = dService.Draw_OneLineLeader(ref singleModel, GetSumPoint(refPoint, -350, 0), dimLeadder1, customScaleValue);
                    dimList.AddDrawEntity(testDimTest05);


                    DrawBMLeaderModel dimLeadder2 = new DrawBMLeaderModel() { position = POSITION_TYPE.BOTTOM, upperText = "aaa", lowerText = "bbbb", bmNumber = "4" };
                    DrawEntityModel testDimTest06 = dService.Draw_OneLineLeader(ref singleModel, GetSumPoint(refPoint, -350, 0), dimLeadder2, customScaleValue);
                    dimList.AddDrawEntity(testDimTest06);


                    DrawBMLeaderModel dimLeadder3 = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = "aaa", lowerText = "bbbb", bmNumber = "4" };
                    DrawEntityModel testDimTest07 = dService.Draw_OneLineLeader(ref singleModel, GetSumPoint(refPoint, -350, 0), dimLeadder3, customScaleValue);
                    dimList.AddDrawEntity(testDimTest07);

                    DrawBMLeaderModel dimLeadder4 = new DrawBMLeaderModel() { position = POSITION_TYPE.TOP, upperText = "EEEEEEE", lowerText = "", bmNumber = "4" };
                    DrawEntityModel testDimTest08 = dService.Draw_OneLineLeader(ref singleModel, GetSumPoint(refPoint, -350, 0), dimLeadder4, customScaleValue);
                    dimList.AddDrawEntity(testDimTest08);


                    singleModel.Entities.AddRange(dimList.GetDrawEntity());
                }
                


               
            }



            #region Sample
            // Plate Arrangement
            //if (true)
            //{

            //    // Reference Point
            //    Point3D refPoint = new Point3D(1000, 1000);

            //    // Tank ID
            //    double ID = 24500;
            //    // Plate Info
            //    double plateLength = 8000;
            //    double PlateWidth = 3000;

            //    // Course Width
            //    double[] sampleCourseWidth = new double[8] { 480, 480, 480, 480, 480, 480, 480, 480 };
            //    // course Thickness
            //    double[] sampleCourseThk = new double[8] { 30, 25, 23, 16, 16, 14, 14, 12 };


            //    // Sample Data
            //    List<double> courseWidthList = new List<double>(sampleCourseWidth);
            //    List<double> courseThkList = new List<double>(sampleCourseThk);

            //    List<ShellOutputModel> shellCourses = new List<ShellOutputModel>();
            //    for (int i = 0; i < courseWidthList.Count; i++)
            //    {
            //        ShellOutputModel newShellCourse = new ShellOutputModel();
            //        newShellCourse.PlateWidth = courseWidthList[i].ToString();
            //        newShellCourse.Thickness = courseThkList[i].ToString();
            //        shellCourses.Add(newShellCourse);
            //    }

            //    // Custom Scale
            //    DrawScaleService scaleService = new DrawScaleService();
            //    double customScaleValue = scaleService.GetPlateHorizontalJointScale(courseThkList[0]);


            //    // Dimension Area Size
            //    double dimAreaLength = 80;
            //    double scaleDimAreaLength = scaleService.GetOriginValueOfScale(customScaleValue, dimAreaLength);
            //    double dimAreaGapLength = dimAreaLength + 16;
            //    //double scaleDimAreaGapLength = scaleService.GetOriginValueOfScale(customScaleValue, dimAreaGapLength);


            //    // Vertical Position
            //    double positionBreakSymbol = 0.5;
            //    double positionCourseText = 0.75;
            //    double positionCourseInfo = 0.6;
            //    double positionWeldingInfo = 0.4;
            //    double positionWeldingGapInfo = 0.3;


            //    // Dimension Arc Radius
            //    double dimArcRadius = 10;
            //    double scaleDimArcRadius = scaleService.GetOriginValueOfScale(customScaleValue, dimArcRadius);




            //    // Dim Service
            //    DrawService drawService = new DrawService(null);

            //    // First Dim Point
            //    Point3D firstDimPoint = null;

            //    // Logic : Course 1 = Index 1
            //    DrawShellCourses dsService = new DrawShellCourses();
            //    DrawBreakSymbols shapeService = new DrawBreakSymbols();



            //    List<Point3D> exPoint = new List<Point3D>();
            //    for (int i = 0; i < 4; i++)
            //        exPoint.Add(refPoint);


            //    // List
            //    List<Entity> drawList = new List<Entity>();

            //    int courseIndex = -1;
            //    double courseWidthSum = 0;
            //    foreach (ShellOutputModel eachCourse in shellCourses)
            //    {
            //        courseIndex++;
            //        double eachCourseWidth = valueService.GetDoubleValue(eachCourse.PlateWidth);
            //        double eachCourseThk = valueService.GetDoubleValue(eachCourse.Thickness);
            //        double eachCourseThkNext = 0;
            //        if (courseIndex < shellCourses.Count - 1)
            //            eachCourseThkNext = valueService.GetDoubleValue(shellCourses[courseIndex + 1].Thickness);

            //        bool firstPlate = courseIndex == 0 ? true : false;
            //        bool lastPlate = courseIndex == shellCourses.Count - 1 ? true : false;


            //        if (eachCourseWidth > 0 && eachCourseThk > 0)
            //        {
            //            // One Course
            //            DrawWeldingModel eachWeldingModel = GetOnePlateModel(eachCourseWidth, eachCourseThk, eachCourseThkNext, firstPlate, lastPlate);
            //            List<Entity> chamferList = dsService.GetPlateBlock(out exPoint, GetSumPoint(exPoint[1], 0, 0), eachCourseThk, eachCourseWidth, 0, 0, 2, eachWeldingModel, new DrawCenterLineModel() { scaleValue = customScaleValue }, customScaleValue, out DrawSlopeLeaderModel slopeModel);

            //            // Slope
            //            List<Entity> slopeList = new List<Entity>();
            //            if (slopeModel != null)
            //                slopeList = dsService.slopeSymbolService.GetSlopeSymbol(slopeModel, customScaleValue);


            //            // Double Break Line
            //            List<Entity> cahmSDoubleLineList = shapeService.GetSDoubleLine(GetSumPoint(exPoint[1], 0, -eachCourseWidth / 2), eachCourseThk, customScaleValue, 0.01);
            //            List<Entity> newchamferList = shapeService.GetTrimSDoubleLine(chamferList, cahmSDoubleLineList);


            //            // Dimension
            //            Point3D dimPoint1 = GetSumPoint(exPoint[3], 0, 0);
            //            Point3D dimPoint2 = GetSumPoint(exPoint[0], 0, 0);
            //            if (firstDimPoint == null)
            //                firstDimPoint = GetSumPoint(dimPoint1, 0, 0);


            //            DrawEntityModel dimGapList = new DrawEntityModel();
            //            if (!firstPlate)
            //            {
            //                DrawDimensionModel dimGapModel = new DrawDimensionModel() { position = POSITION_TYPE.LEFT, textUpper = "3.2", dimHeight = dimAreaLength, scaleValue = customScaleValue, extLineLeftVisible = false, extLineRightVisible = false, arrowRightHeadVisible = false, arrowLeftHeadVisible = false };
            //                dimGapList = drawService.Draw_DimensionDetail(ref singleModel, new Point3D(firstDimPoint.X, dimPoint1.Y), new Point3D(firstDimPoint.X, dimPoint1.Y + eachWeldingModel.OtherDistance, 0), customScaleValue, dimGapModel);

            //                dimPoint1 = GetSumPoint(dimPoint1, 0, eachWeldingModel.OtherDistance);
            //            }
            //            DrawDimensionModel dimEachCourseModel = new DrawDimensionModel() { position = POSITION_TYPE.LEFT, textUpper = "AAAA", dimHeight = dimAreaLength, scaleValue = customScaleValue, };
            //            DrawEntityModel dimList = drawService.Draw_DimensionDetail(ref singleModel, new Point3D(firstDimPoint.X, dimPoint1.Y), new Point3D(firstDimPoint.X, dimPoint2.Y, 0), customScaleValue, dimEachCourseModel);


            //            // Add Entity
            //            styleService.SetLayerListEntityExcludingCenterLine(ref newchamferList, layerService.LayerOutLine);
            //            drawList.AddRange(newchamferList);

            //            styleService.SetLayerListEntity(ref cahmSDoubleLineList, layerService.LayerDimension);
            //            drawList.AddRange(cahmSDoubleLineList);

            //            styleService.SetLayerListEntity(ref slopeList, layerService.LayerDimension);
            //            drawList.AddRange(slopeList);


            //            // Add Entity : Dimension
            //            drawList.AddRange(dimList[CommonGlobal.DimLine]);
            //            drawList.AddRange(dimList[CommonGlobal.DimText]);
            //            drawList.AddRange(dimList[CommonGlobal.DimLineExt]);
            //            drawList.AddRange(dimList[CommonGlobal.DimArrow]);

            //            if (dimGapList.Count > 0)
            //            {
            //                drawList.AddRange(dimGapList[CommonGlobal.DimLine]);
            //                drawList.AddRange(dimGapList[CommonGlobal.DimText]);
            //                drawList.AddRange(dimGapList[CommonGlobal.DimLineExt]);
            //                drawList.AddRange(dimGapList[CommonGlobal.DimArrow]);
            //            }

            //            // Infomation
            //            // Course Text
            //            string leaderCourseText = valueService.GetOrdinalNumber(courseIndex + 1) + " COURSE";

            //            DrawBMLeaderModel leaderCourseTextModel = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = leaderCourseText, lowerText = "", bmNumber = "" };
            //            Dictionary<string, List<Entity>> leaderCourseTextList = drawService.Draw_BMLeader(ref singleModel, GetSumPoint(exPoint[2], 0, eachCourseWidth * positionCourseText), leaderCourseTextModel, customScaleValue);

            //            drawList.AddRange(leaderCourseTextList[CommonGlobal.LeaderLine]);
            //            drawList.AddRange(leaderCourseTextList[CommonGlobal.LeaderText]);
            //            drawList.AddRange(leaderCourseTextList[CommonGlobal.LeaderArrow]);

            //            // Course Circle
            //            DrawDimensionModel dimCourseCirModel = new DrawDimensionModel() { position = POSITION_TYPE.TOP, textUpperPosition = POSITION_TYPE.LEFT, textUpper = "t" + eachCourseThk, dimHeight = 0, scaleValue = customScaleValue, extLineLeftVisible = false, extLineRightVisible = false, arrowRightHeadVisible = false, arrowLeftHeadOut = true, leftBMNumber = (courseIndex + 1).ToString() };
            //            Dictionary<string, List<Entity>> dimCourseCirList = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(exPoint[3], 0, eachCourseWidth * positionCourseText), GetSumPoint(exPoint[3], eachCourseThk, eachCourseWidth * positionCourseText), customScaleValue, dimCourseCirModel);

            //            drawList.AddRange(dimCourseCirList[CommonGlobal.DimLine]);
            //            drawList.AddRange(dimCourseCirList[CommonGlobal.DimText]);
            //            drawList.AddRange(dimCourseCirList[CommonGlobal.DimLineExt]);
            //            drawList.AddRange(dimCourseCirList[CommonGlobal.DimArrow]);

            //            // Course Info
            //            string leaderCourseInfoD = "D=" + "2364";
            //            string leaderCourseInfoC = "C=" + "1234";
            //            DrawBMLeaderModel leaderCourseInfoModel = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = leaderCourseInfoD, lowerText = leaderCourseInfoC, bmNumber = "" };
            //            Dictionary<string, List<Entity>> leaderCourseInfoList = drawService.Draw_BMLeader(ref singleModel, GetSumPoint(exPoint[2], -eachCourseThk / 2, eachCourseWidth * positionCourseInfo), leaderCourseInfoModel, customScaleValue);

            //            drawList.AddRange(leaderCourseInfoList[CommonGlobal.LeaderLine]);
            //            drawList.AddRange(leaderCourseInfoList[CommonGlobal.LeaderText]);
            //            drawList.AddRange(leaderCourseInfoList[CommonGlobal.LeaderArrow]);


            //            // Welding Info
            //            DrawDimensionModel dimCourseWeldingModel = new DrawDimensionModel() { position = POSITION_TYPE.TOP, textUpperPosition = POSITION_TYPE.LEFT, textUpper = eachWeldingModel.LeftDepth.ToString(), dimHeight = 0, scaleValue = customScaleValue, extLineLeftVisible = false, extLineRightVisible = false, arrowLeftHeadOut = true, arrowRightSymbol = DimHead_Type.Circle };
            //            Dictionary<string, List<Entity>> dimCourseWeldingList = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(exPoint[3], 0, eachCourseWidth * positionWeldingInfo), GetSumPoint(exPoint[3], eachCourseThk / 2, eachCourseWidth * positionWeldingInfo), customScaleValue, dimCourseWeldingModel);

            //            drawList.AddRange(dimCourseWeldingList[CommonGlobal.DimLine]);
            //            drawList.AddRange(dimCourseWeldingList[CommonGlobal.DimText]);
            //            drawList.AddRange(dimCourseWeldingList[CommonGlobal.DimLineExt]);
            //            drawList.AddRange(dimCourseWeldingList[CommonGlobal.DimArrow]);

            //            DrawDimensionModel dimCourseWeldingModel2 = new DrawDimensionModel() { position = POSITION_TYPE.TOP, textUpperPosition = POSITION_TYPE.RIGHT, textUpper = eachWeldingModel.RightDepth.ToString(), dimHeight = 0, scaleValue = customScaleValue, extLineLeftVisible = false, extLineRightVisible = false, arrowRightHeadOut = true, arrowLeftSymbol = DimHead_Type.Circle };
            //            Dictionary<string, List<Entity>> dimCourseWeldingList2 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(exPoint[3], eachCourseThk / 2, eachCourseWidth * positionWeldingInfo), GetSumPoint(exPoint[3], eachCourseThk, eachCourseWidth * positionWeldingInfo), customScaleValue, dimCourseWeldingModel2);

            //            drawList.AddRange(dimCourseWeldingList2[CommonGlobal.DimLine]);
            //            drawList.AddRange(dimCourseWeldingList2[CommonGlobal.DimText]);
            //            drawList.AddRange(dimCourseWeldingList2[CommonGlobal.DimLineExt]);
            //            drawList.AddRange(dimCourseWeldingList2[CommonGlobal.DimArrow]);


            //            // Welding Gap
            //            double midGapLength = 0;
            //            if (!eachWeldingModel.BottomWeldingSingle)
            //                midGapLength = -eachWeldingModel.RightDepth;
            //            DrawDimensionModel dimCourseWeldingGapModel = new DrawDimensionModel() { position = POSITION_TYPE.TOP, textUpperPosition = POSITION_TYPE.RIGHT, textUpper = eachWeldingModel.MidDepth.ToString(), dimHeight = 10, scaleValue = customScaleValue, arrowLeftHeadOut = true, arrowRightHeadOut = true };
            //            Dictionary<string, List<Entity>> dimCourseWeldingGapList = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(exPoint[2], midGapLength - eachWeldingModel.MidDepth, eachWeldingModel.OtherDistance), GetSumPoint(exPoint[2], midGapLength, eachWeldingModel.OtherDistance), customScaleValue, dimCourseWeldingGapModel);

            //            drawList.AddRange(dimCourseWeldingGapList[CommonGlobal.DimLine]);
            //            drawList.AddRange(dimCourseWeldingGapList[CommonGlobal.DimText]);
            //            drawList.AddRange(dimCourseWeldingGapList[CommonGlobal.DimLineExt]);
            //            drawList.AddRange(dimCourseWeldingGapList[CommonGlobal.DimArrow]);


            //            // Welding Dimension
            //            Dictionary<string, List<Entity>> dimCourseLeftWeldingArcList = new Dictionary<string, List<Entity>>();
            //            Dictionary<string, List<Entity>> dimCourseRightWeldingArcList = new Dictionary<string, List<Entity>>();
            //            if (!firstPlate)
            //            {
            //                if (eachWeldingModel.BottomWeldingSingle)
            //                {
            //                    dimCourseLeftWeldingArcList = drawService.Draw_DimensionArc(GetSumPoint(exPoint[3], 0, 0), GetSumPoint(exPoint[4], 0, 0), "top", scaleDimArcRadius, "45˚", 45, 90, 45, 0, customScaleValue, layerService.LayerDimension);
            //                }
            //                else
            //                {
            //                    dimCourseLeftWeldingArcList = drawService.Draw_DimensionArc(GetSumPoint(exPoint[3], 0, 0), GetSumPoint(exPoint[4], 0, 0), "top", scaleDimArcRadius, "45˚", 45, 90, 45, 0, customScaleValue, layerService.LayerDimension);
            //                    dimCourseRightWeldingArcList = drawService.Draw_DimensionArc(GetSumPoint(exPoint[5], 0, 0), GetSumPoint(exPoint[2], 0, 0), "top", scaleDimArcRadius, "45˚", 45, -45, -90, 0, customScaleValue, layerService.LayerDimension);
            //                }
            //                if (dimCourseLeftWeldingArcList.Count > 0)
            //                {
            //                    drawList.AddRange(dimCourseLeftWeldingArcList[CommonGlobal.DimLine]);
            //                    drawList.AddRange(dimCourseLeftWeldingArcList[CommonGlobal.DimText]);
            //                    drawList.AddRange(dimCourseLeftWeldingArcList[CommonGlobal.DimLineExt]);
            //                    drawList.AddRange(dimCourseLeftWeldingArcList[CommonGlobal.DimArrow]);
            //                }
            //                if (dimCourseRightWeldingArcList.Count > 0)
            //                {
            //                    drawList.AddRange(dimCourseRightWeldingArcList[CommonGlobal.DimLine]);
            //                    drawList.AddRange(dimCourseRightWeldingArcList[CommonGlobal.DimText]);
            //                    drawList.AddRange(dimCourseRightWeldingArcList[CommonGlobal.DimLineExt]);
            //                    drawList.AddRange(dimCourseRightWeldingArcList[CommonGlobal.DimArrow]);
            //                }
            //            }


            //        }

            //        courseWidthSum += eachCourseWidth;
            //    }



            //    // Dimension : Vertical
            //    DrawDimensionModel dimVerticalModel = new DrawDimensionModel() { position = POSITION_TYPE.LEFT, textUpper = "COMPLECTIOON HEIgHT 26400", textLower = "ERECTION HeIGHT 26432", dimHeight = dimAreaGapLength, scaleValue = customScaleValue };
            //    Dictionary<string, List<Entity>> dimVertical = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, 0, courseWidthSum), customScaleValue, dimVerticalModel);

            //    drawList.AddRange(dimVertical[CommonGlobal.DimLine]);
            //    drawList.AddRange(dimVertical[CommonGlobal.DimText]);
            //    drawList.AddRange(dimVertical[CommonGlobal.DimLineExt]);
            //    drawList.AddRange(dimVertical[CommonGlobal.DimArrow]);

            //    singleModel.Entities.AddRange(drawList);

            //}

            #endregion

            if (visibleFalse)
            {
                Point3D refPoint = new Point3D(1000, 1000);

                Point3D LeftPoint = GetSumPoint(refPoint, 0, 0);
                List<Point3D> pointList = new List<Point3D>();



                Line tempLine = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, 100, 0));

                pointList.Add(GetSumPoint(refPoint, 0, 0));
                pointList.Add(GetSumPoint(refPoint, 25, -10));
                pointList.Add(GetSumPoint(refPoint, 50, 0));
                pointList.Add(GetSumPoint(refPoint, 75, -10));
                pointList.Add(GetSumPoint(refPoint, 100, 0));
                Curve tempSpline = Curve.CubicSplineInterpolation(pointList);

                singleModel.Entities.Add(tempLine);
                singleModel.Entities.Add(tempSpline);
            }

            // 1 Course Development
            if (visibleFalse)
            {
                CDPoint refPoint = new CDPoint(10000, 10000,0);
                CDPoint curPoint = new CDPoint(10000, 10000, 0);

                DrawDetailShellService ddsService = new DrawDetailShellService(null, singleModel);


                double customScaleValue = 0;

                DrawEntityModel drawEntity = ddsService.GetOneCourseShellPlate(ref refPoint, ref curPoint, singleModel, customScaleValue);



                singleModel.Entities.AddRange(drawEntity.GetDrawEntity());
            }

            if (visibleFalse)
            {
                Point3D a = new Point3D(200, 200);
                Point3D b = new Point3D(220, 220);
                Point3D c = new Point3D(240, 200);

                //Arc newArc = new Arc(a, b, c,false);
                Arc newArc = new Arc(a, b, c);

                Arc newArc1 = (Arc)newArc.Clone();
                List<Entity> newList = new List<Entity>();
                newList.Add(newArc1);

                editingService.SetTranslate(ref newList, new Point3D(200, 200), new Point3D(200, 220));

                singleModel.Entities.AddRange(newList);
                styleService.SetLayer(ref newArc, layerService.LayerOutLine);
                singleModel.Entities.Add(newArc);
            }

            if (visibleFalse)
            {
                Point3D referencePoint = new Point3D(10000, 10000);
                DrawWeldSymbols weldSymbols = new DrawWeldSymbols();
                double scaleValue = 10;

                List<Entity> newList = new List<Entity>();

                DrawWeldSymbolModel wsModel = new DrawWeldSymbolModel();
                wsModel.position = ORIENTATION_TYPE.TOPRIGHT;
                wsModel.weldTypeUp = WeldSymbol_Type.U;
                wsModel.weldDetailType = WeldSymbolDetail_Type.BothSide;
                wsModel.weldFaceUp = WeldFace_Type.Convex;
                wsModel.weldSize1 = "12";
                wsModel.weldSize2 = "24";
                //wsModel.weldAngle1 = "90";
                //wsModel.weldAngle2 = "45";
                //wsModel.weldRoot1 = "11";
                //wsModel.weldRoot2 = "22";

                wsModel.weldLength1 = "12";
                wsModel.weldQuantity1 = "3";
                wsModel.weldPitch1 = "123";

                wsModel.specification = "TTXT\r\nCCT\r\nSXST";

                wsModel.machiningVisible = true;

                newList.AddRange(weldSymbols.GetWeldSymbol(referencePoint, singleModel, 10, wsModel));

                styleService.SetLayerListEntity(ref newList, layerService.LayerDimension);
                singleModel.Entities.AddRange(newList);

            }

            // Welding Dime
            if (visibleFalse)
            {
                Point3D referencePoint = new Point3D(10000, 10000);
                DrawWeldSymbols weldSymbols = new DrawWeldSymbols();
                double scaleValue = 10;

                List<Entity> newList = new List<Entity>();

                DrawWeldSymbolModel wsModel = new DrawWeldSymbolModel();
                wsModel.position = ORIENTATION_TYPE.TOPLEFT;
                wsModel.weldTypeUp = WeldSymbol_Type.Square;
                wsModel.weldTypeDown = WeldSymbol_Type.Bevel;
                wsModel.weldDetailType = WeldSymbolDetail_Type.BothSide;
                wsModel.weldFaceDown = WeldFace_Type.Convex;
                wsModel.specification = "B.G";
                wsModel.leaderAngle = 45;
                wsModel.leaderLineLength = 20;

                newList.AddRange(weldSymbols.GetWeldSymbol(referencePoint, singleModel, 10, wsModel));



                DrawWeldSymbolModel wsModel01 = new DrawWeldSymbolModel();
                wsModel01.position = ORIENTATION_TYPE.TOPRIGHT;
                wsModel01.weldTypeUp = WeldSymbol_Type.V;
                wsModel01.weldTypeDown = WeldSymbol_Type.Square;
                wsModel01.weldDetailType = WeldSymbolDetail_Type.BothSide;
                wsModel01.weldFaceUp = WeldFace_Type.Convex;
                wsModel01.weldFaceDown = WeldFace_Type.Flat;
                wsModel01.machiningStr = "G";
                wsModel01.machiningVisible = true;
                wsModel01.tailVisible = false;
                wsModel01.leaderAngle = 60;
                wsModel01.leaderLineLength = 15;

                newList.AddRange(weldSymbols.GetWeldSymbol(GetSumPoint(referencePoint,0,100), singleModel, 10, wsModel01));


                DrawWeldSymbolModel wsModel02 = new DrawWeldSymbolModel();
                wsModel02.position = ORIENTATION_TYPE.TOPLEFT;
                wsModel02.weldTypeUp = WeldSymbol_Type.Fillet;
                wsModel02.weldTypeDown = WeldSymbol_Type.Fillet;
                wsModel02.weldDetailType = WeldSymbolDetail_Type.BothSide;
                wsModel02.weldLength1 = "5";
                wsModel02.tailVisible = false;
                wsModel02.leaderAngle = 60;
                wsModel02.leaderLineLength = 15;

                newList.AddRange(weldSymbols.GetWeldSymbol(GetSumPoint(referencePoint, 0, 200), singleModel, 10, wsModel02));



                styleService.SetLayerListEntity(ref newList, layerService.LayerDimension);
                singleModel.Entities.AddRange(newList);

            }


            if (visibleFalse)
            {
                double customScaleValue = 3;

                DrawBreakSymbols breakService = new DrawBreakSymbols();

                Point3D refPoint = new Point3D(1000, 1000);
                double selWidth = 50;
                double selLength = 1000;

                Point3D sLinePoint = GetSumPoint(refPoint, 0, 0);


                List<Entity> lineList = new List<Entity>();
                Line line01 = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, 0, -selLength));
                //Line line02 = new Line(GetSumPoint(refPoint, selWidth, 0), GetSumPoint(refPoint, selWidth, -selLength));
                Line line02 = new Line(GetSumPoint(refPoint, selWidth, -selLength), GetSumPoint(refPoint, selWidth, 0));
                lineList.Add(line01);
                lineList.Add(line02);



                List<Entity> newSLineList = breakService.GetSLine(GetSumPoint(sLinePoint, 0, -50), selWidth, true);
                breakService.SetTrimSLine(ref lineList, ref newSLineList, GetSumPoint(sLinePoint, 0, -2000));
                List<Entity> newSLineList2 = breakService.GetSLine(GetSumPoint(sLinePoint, 0, -650), selWidth);
                breakService.SetTrimSLine(ref lineList, ref newSLineList2, GetSumPoint(sLinePoint, 0, 0000));


                List<Entity> newSDoubleLineList = breakService.GetSDoubleLine(GetSumPoint(sLinePoint, selWidth / 2, -300), selWidth, 10);

                List<Entity> newListList = breakService.GetTrimSDoubleLine(lineList, newSDoubleLineList);





                // Chamfer

                double oneCourseHeight = 200;


                if (visibleFalse)
                {



                    DrawEntityModel dimList = new DrawEntityModel();


                    // Dimension : Long
                    DrawService dService = new DrawService(null);
                    DrawEntityModel testDim01 = dService.Draw_Dimension(GetSumCDPoint(refPoint, 0, -oneCourseHeight), GetSumCDPoint(refPoint, 0, 0), null, "left", 200, 2.5, 2.5, 2.5, "", "", "D", 0, customScaleValue, layerService.LayerDimension);
                    dimList.AddDrawEntity(testDim01);



                    // Dimension : Arc
                    DrawEntityModel testDimArc01 = dService.Draw_DimensionArc(GetSumPoint(refPoint, 0, -30), GetSumPoint(refPoint, 0, 0), "bottom", 50, "D", 45, 120, 100, 0, customScaleValue, layerService.LayerDimension);
                    dimList.AddDrawEntity(testDimArc01);

                    DrawEntityModel testDimArc02 = dService.Draw_DimensionArc(GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, 0, 30), "top", 50, "D", 45, 120, 100, 0, customScaleValue, layerService.LayerDimension);
                    dimList.AddDrawEntity(testDimArc02);



                    DrawDimensionModel dimModel = new DrawDimensionModel() { position = POSITION_TYPE.LEFT, textUpper = "ABaEEEEEEE", textLower = "EbbEEEEEEE", dimHeight = 40, scaleValue = customScaleValue };
                    DrawEntityModel testDimTest01 = dService.Draw_DimensionDetail(ref singleModel, GetSumPoint(refPoint, 0, -30), GetSumPoint(refPoint, 0, 0), customScaleValue, dimModel);
                    dimList.AddDrawEntity(testDimTest01);


                    DrawDimensionModel dimModel2 = new DrawDimensionModel() { position = POSITION_TYPE.RIGHT, textUpper = "ABaEEEEEEE", textLower = "EbbEEEEEEE", dimHeight = 40, scaleValue = customScaleValue };
                    DrawEntityModel testDimTest02 = dService.Draw_DimensionDetail(ref singleModel, GetSumPoint(refPoint, 0, -30), GetSumPoint(refPoint, 0, 0), customScaleValue, dimModel2);
                    dimList.AddDrawEntity(testDimTest02);


                    DrawDimensionModel dimModel3 = new DrawDimensionModel()
                    {
                        position = POSITION_TYPE.TOP,
                        textUpper = "ABaEEEEEEE",
                        textLower = "EEEEE",
                        arrowLeftHeadOut = true,
                        arrowRightHeadOut = true,
                        textUpperPosition = POSITION_TYPE.RIGHT,
                        arrowRightSymbol = DimHead_Type.Circle,
                        leftBMNumber = "4",
                        textSizeVisible = false,
                        dimHeight = 40,
                        scaleValue = customScaleValue
                    };
                    DrawEntityModel testDimTest03 = dService.Draw_DimensionDetail(ref singleModel, GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, 30, 0), customScaleValue, dimModel3);
                    dimList.AddDrawEntity(testDimTest03);


                    DrawDimensionModel dimModel4 = new DrawDimensionModel() { position = POSITION_TYPE.BOTTOM, textUpper = "ABaEEEEEEE", textLower = "EbbEEEEEEE", dimHeight = 40, scaleValue = customScaleValue };
                    DrawEntityModel testDimTest04 = dService.Draw_DimensionDetail(ref singleModel, GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, 30, 0), customScaleValue, dimModel4);
                    dimList.AddDrawEntity(testDimTest04);




                    DrawDimensionModel dimModel31 = new DrawDimensionModel()
                    {
                        position = POSITION_TYPE.BOTTOM,
                        textUpper = "ABaEEEEEEE",
                        textLower = "EEEEE",
                        arrowLeftHeadOut = true,
                        arrowRightHeadOut = true,
                        textUpperPosition = POSITION_TYPE.RIGHT,
                        rightBMNumber = "4",
                        textSizeVisible = false,
                        dimHeight = 40,
                        scaleValue = customScaleValue
                    };
                    DrawEntityModel testDimTest031 = dService.Draw_DimensionDetail(ref singleModel, GetSumPoint(refPoint, 500, 0), GetSumPoint(refPoint, 530, 0), customScaleValue, dimModel31,Utility.DegToRad(45));
                    dimList.AddDrawEntity(testDimTest031);
                    
                    DrawDimensionModel dimModel32 = new DrawDimensionModel()
                    {
                        position = POSITION_TYPE.RIGHT,
                        textUpper = "ABaEEEEEEE",
                        textLower = "EEEEE",
                        arrowLeftHeadOut = true,
                        arrowRightHeadOut = true,
                        textUpperPosition = POSITION_TYPE.RIGHT,
                        rightBMNumber = "4",
                        textSizeVisible = false,
                        dimHeight = 40,
                        scaleValue = customScaleValue
                    };
                    DrawEntityModel testDimTest032 = dService.Draw_DimensionDetail(ref singleModel, GetSumPoint(refPoint, 500, -30), GetSumPoint(refPoint, 500, 0), customScaleValue, dimModel32, Utility.DegToRad(30));
                    dimList.AddDrawEntity(testDimTest032);
                    
                    DrawDimensionModel dimModel33 = new DrawDimensionModel()
                    {
                        position = POSITION_TYPE.LEFT,
                        textUpper = "ABaEEEEEEE",
                        textLower = "EEEEE",
                        arrowLeftHeadOut = true,
                        arrowRightHeadOut = true,
                        textUpperPosition = POSITION_TYPE.RIGHT,
                        rightBMNumber = "4",
                        textSizeVisible = false,
                        dimHeight = 40,
                        scaleValue = customScaleValue
                    };
                    DrawEntityModel testDimTest033 = dService.Draw_DimensionDetail(ref singleModel, GetSumPoint(refPoint, 500, -30), GetSumPoint(refPoint, 500, 0), customScaleValue, dimModel33, 0);
                    dimList.AddDrawEntity(testDimTest033);





                    // Leader : Left
                    DrawBMLeaderModel dimLeadder1 = new DrawBMLeaderModel() { position = POSITION_TYPE.LEFT, upperText = "EEEEEE", lowerText = "bbbb", bmNumber = "4" };
                    DrawEntityModel testDimTest05 = dService.Draw_OneLineLeader(ref singleModel, GetSumPoint(refPoint, -350, 0), dimLeadder1, customScaleValue);
                    dimList.AddDrawEntity(testDimTest05);

                    // Leader : Bottom
                    DrawBMLeaderModel dimLeadder2 = new DrawBMLeaderModel() { position = POSITION_TYPE.BOTTOM, upperText = "aaa", lowerText = "bbbb", bmNumber = "4" };
                    DrawEntityModel testDimTest06 = dService.Draw_OneLineLeader(ref singleModel, GetSumPoint(refPoint, -350, 0), dimLeadder2, customScaleValue);
                    dimList.AddDrawEntity(testDimTest06);

                    // Leader : Right
                    DrawBMLeaderModel dimLeadder3 = new DrawBMLeaderModel() { position = POSITION_TYPE.RIGHT, upperText = "aaa", lowerText = "bbbb", bmNumber = "4" };
                    DrawEntityModel testDimTest07 = dService.Draw_OneLineLeader(ref singleModel, GetSumPoint(refPoint, -350, 0), dimLeadder3, customScaleValue);
                    dimList.AddDrawEntity(testDimTest07);

                    // Leader : Top
                    DrawBMLeaderModel dimLeadder4 = new DrawBMLeaderModel() { position = POSITION_TYPE.TOP, upperText = "EEEEEEE", lowerText = "", bmNumber = "4" };
                    DrawEntityModel testDimTest08 = dService.Draw_OneLineLeader(ref singleModel, GetSumPoint(refPoint, -350, 0), dimLeadder4, customScaleValue);
                    dimList.AddDrawEntity(testDimTest08);


                    List<Point3D> extPointList = new List<Point3D>();
                    List<Entity> square01 = shapeService.GetRectangle(out extPointList, GetSumPoint(refPoint,0,500), 100, 100, 0, 0, 0);
                    singleModel.Entities.AddRange(square01);

                    // Break
                    List<Entity> breakLineHorizontal =breakService.GetFlatBreakLine(GetSumPoint(refPoint, 110, 500),GetSumPoint(refPoint, 110,400), customScaleValue);

                    DrawEntityModel testDimArc03 = dService.Draw_DimensionArc(GetSumPoint(refPoint, 0, 500), GetSumPoint(refPoint, 100, 500), "top", -20, "D", 45, 184, -184, 0, customScaleValue, layerService.LayerDimension);
                    dimList.AddDrawEntity(testDimArc03);

                    singleModel.Entities.AddRange(breakLineHorizontal);
                    singleModel.Entities.AddRange(dimList.GetDrawEntity());
                }




            }

            // 원의 호 길이
            if (visibleFalse)
            {
                List<Entity> newList = new List<Entity>();
                Point3D refPoint = new Point3D(1000, 1000);
                double circleRadus = 100;
                Circle newCircle = new Circle(Plane.XY, refPoint, circleRadus);


                Line line01 = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, 0, 110));
                line01.Rotate(Utility.DegToRad(-100), Vector3D.AxisZ,GetSumPoint(refPoint,0,0));
                Line line02 = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, 0, 110));
                line02.Rotate(Utility.DegToRad(-150), Vector3D.AxisZ, GetSumPoint(refPoint, 0, 0));

                Point3D[] line01Inter = newCircle.IntersectWith(line01);
                Point3D[] line02Inter = newCircle.IntersectWith(line02);

                
                Line stringLine = new Line(GetSumPoint(line01Inter[0], 0, 0), GetSumPoint(line02Inter[0], 0, 0));

                double offsetValue = valueService.GetLengthBetweenStringAndArc(circleRadus,stringLine.Length());

                Line stringLineOffset = (Line)stringLine.Offset(-offsetValue,Vector3D.AxisZ);

                Line stringLineOffsetLong= editingService.GetExtendLine(stringLineOffset, 30);

                Line stringHorizontal = new Line(GetSumPoint(stringLine.EndPoint, 0, 0), GetSumPoint(stringLine.EndPoint, 30, 0));

                double vHeight = stringLineOffset.StartPoint.Y - stringLineOffset.EndPoint.Y;
                double vWidth = stringLineOffset.EndPoint.X-stringLineOffset.StartPoint.X ;

                double vSloep = valueService.GetDegreeOfSlope(1, 0) - valueService.GetDegreeOfSlope( vHeight , vWidth);
                double resultHx = valueService.GetHypotenuseByWidth(vSloep, offsetValue);

                Point3D[] stringInter = stringHorizontal.IntersectWith(stringLineOffsetLong);

                Line newVer = new Line(GetSumPoint(stringLine.EndPoint, resultHx, 10), GetSumPoint(stringLine.EndPoint, resultHx, -10));
                Console.WriteLine(stringInter[0].X + "\r" + GetSumPoint(stringLine.EndPoint, resultHx, 0).X);

                newList.Add(stringLine);
                newList.Add(stringLineOffsetLong);
                newList.Add(stringHorizontal);
                newList.Add(newVer);

                newList.Add(line01);
                newList.Add(line02);
                newList.Add(newCircle);

                styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);
                singleModel.Entities.AddRange(newList);

                List<Point3D> outPoint = new List<Point3D>();
                List<Entity> refccc = shapeService.GetRectangle(out outPoint, GetSumPoint(refPoint, 0, 0), 100, 100, 0, 0, 3);
                styleService.SetLayerListEntity(ref refccc, layerService.LayerOutLine);

                List<Entity> refcc = shapeService.GetRectangle(out outPoint, GetSumPoint(refPoint, 0, 0), 100, 100, 0, 0,3);
                styleService.SetLayerListEntity(ref refcc, layerService.LayerVirtualLine);

                singleModel.Entities.AddRange(refccc);
                singleModel.Entities.AddRange(refcc);

            }

            #endregion



            #region Structure
            if (true)
            {
                SingletonData.Clear();

                // Assembly
                AssemblyDataService assemblyService = new AssemblyDataService();
                //AssemblyModel newTankData = assemblyService.CreateMappingData(ExcelFile.Text);

                string excelFile = @"C:\Users\tree\Desktop\CAD\TABAS\20211025 제출\TABAS_20211020.xlsm";
                // New Excel Read
                AssemblyModel newTankData = assemblyService.CreateMappingDataNew(excelFile);
                DrawDetailStructureService DDSService = new DrawDetailStructureService(newTankData, singleModel);
                DDSService.CreateStructureCRTModel();

                double scaleValue = 20;
                Point3D refPoint = new Point3D(1000, 1000,0);
                Point3D curPoint = new Point3D(refPoint.X, refPoint.Y,0);
                DrawEntityModel structureDraw = DDSService.DrawDetailRoofStructureAssembly(refPoint,singleModel,scaleValue,null);
                singleModel.Entities.AddRange(structureDraw.GetDrawEntity());

                DrawEntityModel structureOrin = DDSService.DrawDetailRoofStructureOrientation(refPoint, singleModel, scaleValue,null);
                singleModel.Entities.AddRange(structureOrin.GetDrawEntity());

                //DrawEntityModel leeJu = DDSService.DrawDetailCenterColumn_BB(refPoint, singleModel, scaleValue, null);
                //singleModel.Entities.AddRange(leeJu.GetDrawEntity());

                //DDSService.CreateStructureCRTModel();

                //// Jange
                //DrawDetailStructureShareService DDSServiceShare =new DrawDetailStructureShareService(newTankData,singleModel);
                //#region 임시 데이터 배정
                //string colSize = "6";
                //string rafterSize = "C200x80x7.5x11";


                //NColumnBaseSupportModel baseModel = new NColumnBaseSupportModel();
                //foreach (NColumnBaseSupportModel eachModel in newTankData.NColumnBaseSupportList)
                //{
                //    if (eachModel.ColumnSize == colSize)
                //    {
                //        baseModel = eachModel;
                //        break;
                //    }
                //}

                //NColumnCenterTopSupportModel colCNTTopModel = new NColumnCenterTopSupportModel();
                //foreach (NColumnCenterTopSupportModel eachModel in newTankData.NColumnCenterTopSupportList)
                //{
                //    if (eachModel.ColumnSize == colSize)
                //    {
                //        if (eachModel.RafterSize == rafterSize)
                //        {
                //            colCNTTopModel = eachModel;
                //            break;
                //        }
                //    }
                //}


                //#endregion

                //double pipeHeight = 3000;
                //Point3D columnTopSupportWP = GetSumPoint(refPoint, 0, pipeHeight);

                //// pipe test GuideLine
                //Line guidePipeLine = new Line(GetSumPoint(refPoint, 0, 0), columnTopSupportWP);
                //List<Entity> outlineList = new List<Entity>();
                //outlineList.Add(guidePipeLine);
                //styleService.SetLayerListEntity(ref outlineList, layerService.LayerCenterLine);
                //DrawEntityModel newList = new DrawEntityModel();
                //newList.outlineList.AddRange(outlineList);
                //////////////////////////////////////////////////


                //DrawEntityModel structureBaseDraw = DDSServiceShare.DrawColumnBaseSupport(refPoint, singleModel, scaleValue, baseModel);
                //DrawEntityModel structureColCNTTopDraw = DDSServiceShare.DrawColumnCenterTopSupport(columnTopSupportWP, singleModel, scaleValue, colCNTTopModel);


                //singleModel.Entities.AddRange(newList.GetDrawEntity());
                //singleModel.Entities.AddRange(structureBaseDraw.GetDrawEntity());
                //singleModel.Entities.AddRange(structureColCNTTopDraw.GetDrawEntity());
            }

            #endregion


            singleModel.Entities.Regen();
            singleModel.ZoomFit();
            singleModel.SetView(viewType.Top);
            singleModel.Refresh();


            

        }

        // Plate Model
        public DrawWeldingModel GetOnePlateModel(double selPlateWidth, double selPlateThk, double selNextPlateThk, bool selFirstPlate, bool selLastPlate)
        {
            DrawWeldingModel newModel = new DrawWeldingModel();

            // First Plate
            if (selFirstPlate)
            {
                newModel.BottomWelding = false;
            }
            else
            {
                newModel.BottomWelding = true;
            }

            // Welding : Single or Double
            newModel.BottomWeldingSingle = true;
            if(selPlateThk >=21)
            {
                newModel.BottomWeldingSingle = false;
            }

            // Single Degree
            newModel.LeftDegree = 45;
            // double Degree
            newModel.RightDegree = 45;

            // Arc
            newModel.LeftWeldingArc = true;
            newModel.RightWeldingArc = true;

            // Welding Depth 3 = 2 : 1
            double tempRightDepth = Math.Round( selPlateThk * 1 / 3,MidpointRounding.AwayFromZero);
            double tempLeftDepth = selPlateThk - tempRightDepth;
            newModel.LeftDepth = tempLeftDepth;
            newModel.RightDepth = tempRightDepth;

            // Ohter Plate Distance Gap :Fixed Value
            newModel.OtherDistance = 3.2;
            // Welding Mid Gap
            newModel.MidDepth = 1.6;

            // Top Chamfer
            newModel.TopChamfer = true;
            if (selLastPlate)
                newModel.TopChamfer = false;
            

            // Top Chamfer : 3 : 1
            if (newModel.TopChamfer)
            {
                double tempThk = selPlateThk - selNextPlateThk;
                if (tempThk >= 1)
                {
                    newModel.ChamferShort = tempThk;
                    newModel.ChamferLong = tempThk * 3;

                    if (selPlateWidth + -1 < newModel.ChamferLong)
                        newModel.TopChamfer = false;
                }
                else
                {
                    newModel.TopChamfer = false;
                }
            }
                


            return newModel;
        }

        public List<Entity> CreateNozzleProjection(Point3D refPoint)
        {
            List<Point3D> outPointList = new List<Point3D>();
            List<Entity> newList = new List<Entity>();

            TANK_TYPE refTankType = TANK_TYPE.EFRTDouble;
            // Left : Lower

            // Outline
            double boxHeight = 100;
            double boxWidth = 100;
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            newList.AddRange(shapeService.GetRectangle(out outPointList,GetSumPoint(referencePoint,0,0),boxWidth,boxHeight,0,0,3));

            // Tank : Basic
            double tankID = 52;
            double tankRadius = tankID / 2;
            double tankHeight = 48;

            double tankBottomWidth = 60;
            double tankBottomThickness = 1;
            double tankBottomOutWidth = (tankBottomWidth-tankID)/2;

            double tankRoofWidth = 58;
            double tankRoofHeight = 0;
            double tankRoofOutWidth=(tankRoofWidth- tankID) / 2;


            // TankRoof Height
            switch (refTankType)
            {
                case TANK_TYPE.CRT:
                case TANK_TYPE.DRT:
                case TANK_TYPE.IFRT:
                    tankRoofHeight = 4;
                    break;
                case TANK_TYPE.EFRTSingle:
                case TANK_TYPE.EFRTDouble:
                    tankRoofHeight = 0;
                    break;
            }

            Point3D tankPoint = GetSumPoint(referencePoint, 15.6, 24.1);
            Point3D tankBottomPoint = GetSumPoint(tankPoint, -tankBottomOutWidth, 0);
            Point3D tankRoofPoint = GetSumPoint(tankPoint, 0, tankHeight);
            Point3D tankRoofCenterTopPoint = GetSumPoint(tankRoofPoint,tankRadius,tankRoofHeight);

            // Tank : Shell
            newList.AddRange(shapeService.GetRectangle(out outPointList, GetSumPoint(tankPoint, 0, 0), tankID, tankHeight, 0, 0, 3,new bool[] {false,true,false,true }));

            // Tank : Bottom
            newList.AddRange(shapeService.GetRectangle(out outPointList, GetSumPoint(tankBottomPoint, 0, 0), tankBottomWidth, tankBottomThickness, 0, 0, 0));

            // Tank : Roof : Lower
            newList.Add(new Line(GetSumPoint(tankRoofPoint, -tankRoofOutWidth, 0), GetSumPoint(tankRoofPoint, tankID + tankRoofOutWidth, 0)));
            Line rightRoof = null;
            Arc domeRoof = null;

            // TankRoof Height
            switch (refTankType)
            {
                case TANK_TYPE.CRT:
                case TANK_TYPE.IFRT:
                    newList.Add(new Line(GetSumPoint(tankRoofPoint, -tankRoofOutWidth, 0), GetSumPoint(tankRoofCenterTopPoint, 0, 0)));
                    rightRoof = new Line(GetSumPoint(tankRoofPoint, tankID + tankRoofOutWidth, 0), GetSumPoint(tankRoofCenterTopPoint, 0, 0));
                    newList.Add(rightRoof);
                    break;
                case TANK_TYPE.DRT:
                    domeRoof = new Arc(Plane.XY, GetSumPoint(tankRoofPoint, tankID, 0), GetSumPoint(tankRoofCenterTopPoint, 0, 0), GetSumPoint(tankRoofPoint, 0, 0), false);
                    newList.Add(domeRoof);
                    break;
            }


            // Nozzle
            double roofNozzleRadius = 20;
            double shellNozzleElevation = 10;
            double nozzleHeight = 6;
            double flangeOD = 3;
            double flangeODHalf = flangeOD/2;
            Point3D roofNozzlePoint = GetSumPoint(tankRoofPoint, tankRadius + roofNozzleRadius, 0);
            Line roofNozzlePipe = new Line(GetSumPoint(roofNozzlePoint, 0, 0), GetSumPoint(roofNozzlePoint, 0, nozzleHeight));
            Line roofNozzleFlange = new Line(GetSumPoint(roofNozzlePoint, -flangeODHalf, nozzleHeight), GetSumPoint(roofNozzlePoint, flangeODHalf, nozzleHeight));
            newList.Add(roofNozzlePipe);
            newList.Add(roofNozzleFlange);

            Point3D[] nozzlePipeInter = null;

            switch (refTankType)
            {
                case TANK_TYPE.CRT:
                case TANK_TYPE.IFRT:
                    nozzlePipeInter = roofNozzlePipe.IntersectWith(rightRoof);
                    if (nozzlePipeInter.Length > 0)
                        roofNozzlePipe.TrimBy(nozzlePipeInter[0], true);
                    break;
                case TANK_TYPE.DRT:
                    nozzlePipeInter = roofNozzlePipe.IntersectWith(domeRoof);
                    if (nozzlePipeInter.Length > 0)
                        roofNozzlePipe.TrimBy(nozzlePipeInter[0], true);
                    break;
            }



            Point3D shellNozzlePoint = GetSumPoint(tankPoint, tankID, shellNozzleElevation);
            Line shellNozzlePipe = new Line(GetSumPoint(shellNozzlePoint, 0, 0), GetSumPoint(shellNozzlePoint, nozzleHeight, 0));
            Line shellNozzleFlange = new Line(GetSumPoint(shellNozzlePoint, nozzleHeight ,flangeODHalf), GetSumPoint(shellNozzlePoint, nozzleHeight,-flangeODHalf));
            newList.Add(shellNozzlePipe);
            newList.Add(shellNozzleFlange);


            // Pontoon
            double pontoonWidth = 10;
            double pontoonMinHeight = 2.5;
            double pontoonElevation = 23;
            double pontoonShellWidth = 1.5;
            double pontoonSlopeHeight = 1;
            double pontoonSingleFlatHeight = 1;

            Point3D pontoonLeftPoint = GetSumPoint(tankPoint, pontoonShellWidth, pontoonElevation);
            Point3D pontoonRightPoint = GetSumPoint(tankPoint, tankID - pontoonShellWidth, pontoonElevation);


            double ponLeftNozzleRadius = 10;
            double ponRightNozzleRadius = 20;
            Line leftPonNozzleFlangeAll = null;
            Line leftPonNozzlePipeAll = null; ;
            Line rightPonNozzleFlangeAll = null;
            Line rightPonNozzlePipeAll = null;

            Point3D deckPoint = null;
            Point3D pontoonPoint = null;

            switch (refTankType)
            {
                case TANK_TYPE.IFRT:
                case TANK_TYPE.EFRTSingle:
                    if (true)
                    {
                        Line leftPon1 = new Line(GetSumPoint(pontoonLeftPoint, 0, 0),GetSumPoint(pontoonLeftPoint,pontoonWidth,0));
                        Line leftPon2 = new Line(GetSumPoint(pontoonLeftPoint, 0, pontoonMinHeight + pontoonSlopeHeight), GetSumPoint(pontoonLeftPoint, pontoonWidth, pontoonMinHeight));
                        Line leftPon3 = new Line(GetSumPoint(pontoonLeftPoint, 0, pontoonMinHeight + pontoonSlopeHeight), GetSumPoint(pontoonLeftPoint, 0, 0));
                        Line leftPon4 = new Line(GetSumPoint(pontoonLeftPoint, pontoonWidth, 0), GetSumPoint(pontoonLeftPoint, pontoonWidth, pontoonMinHeight));

                        Line rightPon1 = new Line(GetSumPoint(pontoonRightPoint, 0, 0), GetSumPoint(pontoonRightPoint, -pontoonWidth, 0));
                        Line rightPon2 = new Line(GetSumPoint(pontoonRightPoint, 0, pontoonMinHeight + pontoonSlopeHeight), GetSumPoint(pontoonRightPoint, -pontoonWidth, pontoonMinHeight));
                        Line rightPon3 = new Line(GetSumPoint(pontoonRightPoint, 0, pontoonMinHeight + pontoonSlopeHeight), GetSumPoint(pontoonRightPoint, 0, 0));
                        Line rightPon4 = new Line(GetSumPoint(pontoonRightPoint, -pontoonWidth, 0), GetSumPoint(pontoonRightPoint,-pontoonWidth, pontoonMinHeight));

                        Line ponFlat = new Line(GetSumPoint(pontoonLeftPoint, pontoonWidth, pontoonSingleFlatHeight), GetSumPoint(pontoonRightPoint, -pontoonWidth, pontoonSingleFlatHeight));
                        newList.AddRange(new Entity[] { leftPon1, leftPon2, leftPon3, leftPon4, rightPon1, rightPon2, rightPon3, rightPon4, ponFlat });

                        // Leader Point
                        deckPoint = GetSumPoint(ponFlat.MidPoint, 2, 0);
                        pontoonPoint = GetSumPoint(leftPon1.StartPoint, 2, 0);

                        // Left Nozzle
                        Point3D ponLeftNozzlePoint = GetSumPoint(pontoonLeftPoint, -pontoonShellWidth + tankRadius - ponLeftNozzleRadius, pontoonSingleFlatHeight);
                        leftPonNozzlePipeAll = new Line(GetSumPoint(ponLeftNozzlePoint, 0, 0), GetSumPoint(ponLeftNozzlePoint, 0, nozzleHeight));
                        leftPonNozzleFlangeAll = new Line(GetSumPoint(ponLeftNozzlePoint, -flangeODHalf, nozzleHeight), GetSumPoint(ponLeftNozzlePoint, flangeODHalf, nozzleHeight));
                        newList.Add(leftPonNozzlePipeAll);
                        newList.Add(leftPonNozzleFlangeAll);


                        // Right Nozzle
                        Point3D ponRightNozzlePointTemp = GetSumPoint(pontoonLeftPoint, -pontoonShellWidth + tankRadius + ponRightNozzleRadius, pontoonSingleFlatHeight);
                        Point3D ponRightNozzlePoint = null;
                        Line vRightNozzleLine = new Line(GetSumPoint(ponRightNozzlePointTemp, 0, 0), GetSumPoint(ponRightNozzlePointTemp, 0, 100));
                        Point3D[] ponRightInter = vRightNozzleLine.IntersectWith(rightPon2);
                        if (ponRightInter.Length > 0)
                        {
                            ponRightNozzlePoint = ponRightInter[0];
                            double nozzleHeightAdj = nozzleHeight - 1;
                            rightPonNozzlePipeAll = new Line(GetSumPoint(ponRightNozzlePoint, 0, 0), GetSumPoint(ponRightNozzlePoint, 0, nozzleHeightAdj));
                            rightPonNozzleFlangeAll = new Line(GetSumPoint(ponRightNozzlePoint, -flangeODHalf, nozzleHeightAdj), GetSumPoint(ponRightNozzlePoint, flangeODHalf, nozzleHeightAdj));
                            newList.Add(rightPonNozzlePipeAll);
                            newList.Add(rightPonNozzleFlangeAll);
                        }

                    }
                    break;
                case TANK_TYPE.EFRTDouble:
                    if (true)
                    {
                        Line leftPon1 = new Line(GetSumPoint(pontoonLeftPoint, 0, 0), GetSumPoint(pontoonLeftPoint, pontoonWidth, 0));
                        Line leftPon2 = new Line(GetSumPoint(pontoonLeftPoint, 0, pontoonMinHeight + pontoonSlopeHeight), GetSumPoint(pontoonLeftPoint, pontoonWidth, pontoonMinHeight));
                        Line leftPon3 = new Line(GetSumPoint(pontoonLeftPoint, 0, pontoonMinHeight + pontoonSlopeHeight), GetSumPoint(pontoonLeftPoint, 0, 0));
                        Line leftPon4 = new Line(GetSumPoint(pontoonLeftPoint, pontoonWidth, 0), GetSumPoint(pontoonLeftPoint, pontoonWidth, pontoonMinHeight));

                        Line rightPon1 = new Line(GetSumPoint(pontoonRightPoint, 0, 0), GetSumPoint(pontoonRightPoint, -pontoonWidth, 0));
                        Line rightPon2 = new Line(GetSumPoint(pontoonRightPoint, 0, pontoonMinHeight + pontoonSlopeHeight), GetSumPoint(pontoonRightPoint, -pontoonWidth, pontoonMinHeight));
                        Line rightPon3 = new Line(GetSumPoint(pontoonRightPoint, 0, pontoonMinHeight + pontoonSlopeHeight), GetSumPoint(pontoonRightPoint, 0, 0));
                        Line rightPon4 = new Line(GetSumPoint(pontoonRightPoint, -pontoonWidth, 0), GetSumPoint(pontoonRightPoint, -pontoonWidth, pontoonMinHeight));

                        Line ponFlat1 = new Line(GetSumPoint(pontoonLeftPoint, pontoonWidth, pontoonMinHeight), GetSumPoint(pontoonRightPoint, -pontoonWidth, pontoonMinHeight));
                        Line ponFlat2 = new Line(GetSumPoint(pontoonLeftPoint, pontoonWidth, 0), GetSumPoint(pontoonRightPoint, -pontoonWidth, 0));
                        newList.AddRange(new Entity[] { leftPon1, leftPon2, leftPon3, leftPon4, rightPon1, rightPon2, rightPon3, rightPon4, ponFlat1, ponFlat2 });

                        // Leader Point
                        deckPoint = GetSumPoint(ponFlat1.MidPoint, 2, 0);
                        pontoonPoint = GetSumPoint(leftPon1.StartPoint, 2, 0);

                        // Left Nozzle
                        Point3D ponLeftNozzlePoint = GetSumPoint(pontoonLeftPoint, -pontoonShellWidth + tankRadius - ponLeftNozzleRadius, pontoonMinHeight);
                        leftPonNozzlePipeAll = new Line(GetSumPoint(ponLeftNozzlePoint, 0, 0), GetSumPoint(ponLeftNozzlePoint, 0, nozzleHeight));
                        leftPonNozzleFlangeAll = new Line(GetSumPoint(ponLeftNozzlePoint, -flangeODHalf, nozzleHeight), GetSumPoint(ponLeftNozzlePoint, flangeODHalf, nozzleHeight));
                        newList.Add(leftPonNozzlePipeAll);
                        newList.Add(leftPonNozzleFlangeAll);

                        // Right Nozzle
                        Point3D ponRightNozzlePointTemp = GetSumPoint(pontoonLeftPoint, -pontoonShellWidth + tankRadius + ponRightNozzleRadius, pontoonSingleFlatHeight);
                        Point3D ponRightNozzlePoint = null;
                        Line vRightNozzleLine = new Line(GetSumPoint(ponRightNozzlePointTemp, 0, 0), GetSumPoint(ponRightNozzlePointTemp, 0, 100));
                        Point3D[] ponRightInter = vRightNozzleLine.IntersectWith(rightPon2);
                        if (ponRightInter.Length > 0)
                        {
                            ponRightNozzlePoint = ponRightInter[0];
                            double nozzleHeightAdj = nozzleHeight - 1;
                            rightPonNozzlePipeAll = new Line(GetSumPoint(ponRightNozzlePoint, 0, 0), GetSumPoint(ponRightNozzlePoint, 0, nozzleHeightAdj));
                            rightPonNozzleFlangeAll = new Line(GetSumPoint(ponRightNozzlePoint, -flangeODHalf, nozzleHeightAdj), GetSumPoint(ponRightNozzlePoint, flangeODHalf, nozzleHeightAdj));
                            newList.Add(rightPonNozzlePipeAll);
                            newList.Add(rightPonNozzleFlangeAll);
                        }
                    }
                    break;
            }

            
            // Center Line
            double exLength = 2;
            List<Entity> centerLineList = editingService.GetCenterLine(GetSumPoint(tankPoint, tankRadius, -tankBottomThickness), GetSumPoint(tankRoofCenterTopPoint, 0, 0), exLength, 1);
            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);



            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            // Dimension
            double dimTextHeight = 2.5;
            double dimCenterLineHeight = 12;
            double dimCenterLineHeight2 = 12;
            Plane planeLeft = new Plane(Point3D.Origin, Vector3D.AxisY, -1 * Vector3D.AxisX); // plane.XY, vertical writing;

            // Dimension : Roof : R
            if (refTankType==TANK_TYPE.IFRT || refTankType == TANK_TYPE.EFRTSingle || refTankType== TANK_TYPE.EFRTDouble)
                dimCenterLineHeight2 = 6;


            List<LinearDim> dimList = new List<LinearDim>();
            List<Entity> leaderList = new List<Entity>();


            Point3D dimLeft1 = ((ICurve)centerLineList[2]).EndPoint;
            Point3D dimRight1 = roofNozzleFlange.MidPoint;
            Point3D dimMid1 = GetSumPoint(dimLeft1, (dimRight1.X- dimLeft1.X)/2, dimCenterLineHeight);
            dimList.Add(new LinearDim(Plane.XY, dimLeft1, dimRight1, dimMid1, dimTextHeight){TextOverride = "\"R\""});

            // Dimension : Shell : R
            Point3D dimLeft2 = GetSumPoint(tankPoint,tankRadius,shellNozzleElevation+flangeODHalf);
            Point3D dimRight2 = GetSumPoint(shellNozzlePoint, nozzleHeight, flangeODHalf );
            Point3D dimMid2 = GetSumPoint(dimLeft2, (dimRight2.X - dimLeft2.X) / 2, dimCenterLineHeight2);
            dimList.Add(new LinearDim(Plane.XY, dimLeft2, dimRight2, dimMid2, dimTextHeight) { TextOverride = "\"R\"",ShowExtLine1=false });

            // Dimension : Roof : H
            Point3D dimLeft3 = GetSumPoint(roofNozzleFlange.EndPoint, 0,0);
            Point3D dimRight3 = GetSumPoint(tankPoint, tankID +tankBottomOutWidth, -tankBottomThickness);
            Point3D dimMid3 = GetSumPoint(dimRight3,  dimCenterLineHeight*1.4, (dimLeft3.Y - dimRight3.Y) / 2);
            dimList.Add(new LinearDim(planeLeft, dimLeft3, dimRight3, dimMid3, dimTextHeight) { TextOverride = "\"H\""});

            // Dimension : Shell : H
            Point3D dimLeft4 = GetSumPoint(shellNozzleFlange.MidPoint, 0, 0);
            Point3D dimRight4 = GetSumPoint(tankPoint, tankID + tankBottomOutWidth, -tankBottomThickness);
            Point3D dimMid4 = GetSumPoint(dimLeft4, dimCenterLineHeight*0.6 , (dimRight4.Y - dimLeft4.Y) / 2);
            dimList.Add(new LinearDim(planeLeft, dimLeft4, dimRight4, dimMid4, dimTextHeight) { TextOverride = "\"H\"",ArrowsLocation=elementPositionType.Inside, ShowExtLine1 = false });

            if(refTankType==TANK_TYPE.IFRT || refTankType == TANK_TYPE.EFRTSingle  || refTankType == TANK_TYPE.EFRTDouble)
            {
                dimCenterLineHeight = 10;
                if (refTankType == TANK_TYPE.EFRTDouble)
                    dimCenterLineHeight = dimCenterLineHeight - (2.5 / 2);

                Point3D dimPonLeft1 = leftPonNozzleFlangeAll.MidPoint;
                Point3D dimPonRight1 = GetSumPoint(dimPonLeft1, ponLeftNozzleRadius, 0);
                Point3D dimPonMid1 = GetSumPoint(dimPonLeft1, (dimPonRight1.X - dimPonLeft1.X) / 2, dimCenterLineHeight);
                dimList.Add(new LinearDim(Plane.XY, dimPonLeft1, dimPonRight1, dimPonMid1, dimTextHeight) { TextOverride = "\"R\"",ArrowsLocation=elementPositionType.Inside, ShowExtLine2=false });

                Point3D dimPonRight2 = rightPonNozzleFlangeAll.MidPoint;
                Point3D dimPonLeft2 = GetSumPoint(dimPonRight2, -ponRightNozzleRadius, 0);
                Point3D dimPonMid2 = new Point3D(GetSumPoint(dimPonLeft2, (dimPonRight2.X - dimPonLeft2.X) / 2, dimCenterLineHeight).X,dimPonMid1.Y);
                dimList.Add(new LinearDim(Plane.XY, dimPonLeft2, dimPonRight2, dimPonMid2, dimTextHeight) { TextOverride = "\"R\"", ShowExtLine1 = false });

                // Dimension : Shell : H
                Point3D dimPonLeft4 = GetSumPoint(leftPonNozzleFlangeAll.EndPoint, 0, 0);
                Point3D dimPonRight4 = GetSumPoint(leftPonNozzlePipeAll.StartPoint, 0, 0);
                Point3D dimPonMid4 = GetSumPoint(dimPonLeft4, dimCenterLineHeight * 0.4, (dimPonRight4.Y - dimPonLeft4.Y) / 2);
                dimList.Add(new LinearDim(planeLeft, dimPonLeft4, dimPonRight4, dimPonMid4, dimTextHeight) { TextOverride = "\"H\"", ArrowsLocation = elementPositionType.Outside,TextLocation=elementPositionType.Outside, ShowExtLine1 = false });

                // Dimension : Shell : H
                Point3D dimPonLeft5 = GetSumPoint(rightPonNozzleFlangeAll.EndPoint, 0, 0);
                Point3D dimPonRight5 = GetSumPoint(rightPonNozzlePipeAll.StartPoint, 0, 0);
                Point3D dimPonMid5=new Point3D(dimMid4.X, GetSumPoint(dimPonLeft5, dimCenterLineHeight * 0.4, 0).Y);
                dimList.Add(new LinearDim(planeLeft, dimPonLeft5, dimPonRight5, dimPonMid5, dimTextHeight) { TextOverride = "\"H\"", ArrowsLocation = elementPositionType.Outside, TextLocation = elementPositionType.Outside });



                Point3D deckTextPoint = GetSumPoint(deckPoint, 4, 8);
                Leader deckLeader = new Leader(Plane.XY, new Point3D[] { deckPoint, deckTextPoint,GetSumPoint(deckTextPoint,11,0) });
                Text deckText = new Text(GetSumPoint(deckTextPoint, 1,0.5), "DECK", 2.5);
                deckText.Alignment = Text.alignmentType.BaselineLeft;
                leaderList.Add(deckLeader);

                Point3D ponTextPoint = GetSumPoint(pontoonPoint, 3, -14);
                Leader ponLeader = new Leader(Plane.XY, new Point3D[] { pontoonPoint, ponTextPoint, GetSumPoint(ponTextPoint, 18.5, 0) });
                Text ponText = new Text(GetSumPoint(ponTextPoint, 1, 0.5), "PONTOON", 2.5);
                ponText.Alignment = Text.alignmentType.BaselineLeft;
                leaderList.Add(ponLeader);

                styleService.SetLayerListEntity(ref leaderList, layerService.LayerDimension);
                styleService.SetLayer(ref deckText, layerService.LayerDimension);
                styleService.SetLayer(ref ponText, layerService.LayerDimension);
                leaderList.Add(deckText);
                leaderList.Add(ponText);

            }

            foreach (LinearDim eachDim in dimList)
            {
                eachDim.LayerName = layerService.LayerDimension;
                eachDim.LineTypeMethod = colorMethodType.byLayer;
                eachDim.ColorMethod = colorMethodType.byLayer;
            }

            // Text
            List<Entity> textList = new List<Entity>();
            double textHeight = 2.5;
            Point3D textCenter = GetSumPoint(tankPoint, tankID + tankBottomOutWidth + 12 * 1.4 / 2,tankBottomThickness);
            Text newUnderText1 = new Text(GetSumPoint(textCenter,0, -textHeight*2 -0.5), "UNDER", textHeight);
            newUnderText1.Alignment = Text.alignmentType.BottomCenter;
            Text newUnderText2 = new Text(GetSumPoint(textCenter,0,-textHeight*3-1), "OF BTM.", textHeight);
            newUnderText2.Alignment = Text.alignmentType.BottomCenter;
            textList.Add(newUnderText1);
            textList.Add(newUnderText2);

            styleService.SetLayerListEntity(ref textList, layerService.LayerDimension);

            // Title
            List<Entity> titleList = new List<Entity>();
            Point3D titleBaseCenter1 = GetSumPoint(referencePoint, boxWidth / 2, 7);
            Point3D titleBaseCenter2 = GetSumPoint(referencePoint, boxWidth / 2, 8);
            Point3D titleTextCenter = GetSumPoint(referencePoint, boxWidth / 2, 9);

            double baseLineWidth = 65;
            Line titleBaseLine1 = new Line(GetSumPoint(titleBaseCenter1, -baseLineWidth / 2, 0), GetSumPoint(titleBaseCenter1, baseLineWidth / 2, 0));
            styleService.SetLayer(ref titleBaseLine1, layerService.LayerOutLine);
            Line titleBaseLine2 = new Line(GetSumPoint(titleBaseCenter2, -baseLineWidth / 2, 0), GetSumPoint(titleBaseCenter2, baseLineWidth / 2, 0));
            styleService.SetLayer(ref titleBaseLine2, layerService.LayerDimension);
            Text titleText = new Text(titleTextCenter, "NOZZLE PROJECTION", 4);
            titleText.Alignment = Text.alignmentType.BaselineCenter;
            titleText.ColorMethod = colorMethodType.byEntity;
            titleText.Color = Color.Yellow;
            titleList.AddRange(new Entity[] { titleBaseLine1, titleBaseLine2, titleText });

            newList.AddRange(dimList);
            newList.AddRange(centerLineList);
            newList.AddRange(textList);
            newList.AddRange(leaderList);
            newList.AddRange(titleList);


            return newList;
        }


        #region Nozzle : 공통
        private Point3D GetTranslatePoint(ref List<Entity> selList)
        {
            Point3D translatePoint = null;
            foreach (Entity eachEntity in selList)
                if (eachEntity.EntityData != null)
                {
                    translatePoint = (Point3D)eachEntity.EntityData;
                    break;
                }
            return translatePoint;
        }
        private void SetTranslate(ref List<Entity> selList, Point3D refPoint, Point3D currentPoint)
        {
            double distanceY = refPoint.Y - currentPoint.Y;
            double distanceX = refPoint.X - currentPoint.X;
            Vector3D tempMovement = new Vector3D(distanceX, distanceY);
            foreach (Entity eachEntity in selList)
                eachEntity.Translate(tempMovement);
        }
        #endregion










        public Point3D GetMinPointXY(Point3D[] selPointArray)
        {
            Point3D returnValue = null;

            foreach(Point3D eachPoint in selPointArray)
            {
                if (returnValue == null)
                {
                    returnValue = new Point3D();
                    returnValue.X = eachPoint.X;
                    returnValue.Y = eachPoint.Y;
                }
                else
                {
                    if (returnValue.X > eachPoint.X)
                        returnValue.X = eachPoint.X;
                    if (returnValue.Y > eachPoint.Y)
                        returnValue.Y = eachPoint.Y;
                }
            }

            return returnValue;
        }
        public Point3D GetMaxPointXY(Point3D[] selPointArray)
        {
            Point3D returnValue = null;

            foreach (Point3D eachPoint in selPointArray)
            {
                if (returnValue == null)
                {
                    returnValue = new Point3D();
                    returnValue.X = eachPoint.X;
                    returnValue.Y = eachPoint.Y;
                }
                else
                {
                    if (returnValue.X < eachPoint.X)
                        returnValue.X = eachPoint.X;
                    if (returnValue.Y < eachPoint.Y)
                        returnValue.Y = eachPoint.Y;
                }
            }

            return returnValue;
        }

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
        private CDPoint GetSumCDPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new CDPoint(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }

        #region Dimension 최신
        public List<Entity> GetDimension(Point3D selPoint1,Point3D selPoint2, string selType,double selDimHeight)
        {
            List<Entity> newList = new List<Entity>();

            Point3D textCenter = new Point3D();

            double selTextHeight = 2.5;
            double selArrowHeight = 2.5;
            string selDimtext = "AAAAAbbbbbccc";

            double extLine1 = 2;
            double extLine2 = 2;
            double pointGapLine1 = 1;
            double pointGapLine2 = 1;

            double textGap = 1;

            Line dimLine1=null;
            Line dimLine2=null;
            Line arrowLine=null;
            Triangle tri1=null;
            Triangle tri2=null;
            Text dimText=null;



            // Top
            switch (selType)
            {
                case "TOP":
                    textCenter.X = selPoint1.X + (selPoint2.X - selPoint1.X) / 2;
                    textCenter.Y = Math.Min(selPoint1.Y, selPoint2.Y) + selDimHeight;

                    arrowLine = new Line(new Point3D(selPoint1.X, textCenter.Y), new Point3D(selPoint2.X, textCenter.Y));
                    dimLine1 = new Line(new Point3D(selPoint1.X, selPoint1.Y + pointGapLine1), new Point3D(selPoint1.X, textCenter.Y + extLine1));
                    dimLine2 = new Line(new Point3D(selPoint2.X, selPoint2.Y + pointGapLine2), new Point3D(selPoint2.X, textCenter.Y + extLine2));
                    dimText = new Text(new Point3D(textCenter.X, textCenter.Y + textGap), selDimtext, selTextHeight);
                    dimText.Alignment = Text.alignmentType.BaselineCenter;
                    tri1 = new Triangle(new Point3D(selPoint1.X, textCenter.Y), new Point3D(selPoint1.X + selArrowHeight * 3, textCenter.Y + selArrowHeight / 2), new Point3D(selPoint1.X + selArrowHeight * 3, textCenter.Y - selArrowHeight / 2));
                    tri2 = new Triangle(new Point3D(selPoint2.X, textCenter.Y), new Point3D(selPoint2.X - selArrowHeight * 3, textCenter.Y + selArrowHeight / 2), new Point3D(selPoint2.X - selArrowHeight * 3, textCenter.Y - selArrowHeight / 2));

                    break;

                case "BOTTOM":
                    textCenter.X = selPoint1.X + (selPoint2.X - selPoint1.X) / 2;
                    textCenter.Y = Math.Max(selPoint1.Y, selPoint2.Y) - selDimHeight;

                    arrowLine = new Line(new Point3D(selPoint1.X, textCenter.Y), new Point3D(selPoint2.X, textCenter.Y));
                    dimLine1 = new Line(new Point3D(selPoint1.X, selPoint1.Y - pointGapLine1), new Point3D(selPoint1.X, textCenter.Y - extLine1));
                    dimLine2 = new Line(new Point3D(selPoint2.X, selPoint2.Y - pointGapLine2), new Point3D(selPoint2.X, textCenter.Y - extLine2));
                    dimText = new Text(new Point3D(textCenter.X, textCenter.Y + textGap), selDimtext, selTextHeight);
                    dimText.Alignment = Text.alignmentType.BaselineCenter;
                    tri1 = new Triangle(new Point3D(selPoint1.X, textCenter.Y), new Point3D(selPoint1.X + selArrowHeight * 3, textCenter.Y + selArrowHeight / 2), new Point3D(selPoint1.X + selArrowHeight * 3, textCenter.Y - selArrowHeight / 2));
                    tri2 = new Triangle(new Point3D(selPoint2.X, textCenter.Y), new Point3D(selPoint2.X - selArrowHeight * 3, textCenter.Y + selArrowHeight / 2), new Point3D(selPoint2.X - selArrowHeight * 3, textCenter.Y - selArrowHeight / 2));

                    break;


                case "LEFT":
                    textCenter.X = Math.Max(selPoint1.X, selPoint2.X) - selDimHeight;
                    textCenter.Y = selPoint1.Y + (selPoint2.Y - selPoint1.Y) / 2;

                    arrowLine = new Line(new Point3D(textCenter.X, selPoint1.Y), new Point3D(textCenter.X, selPoint2.Y));
                    dimLine1 = new Line(new Point3D(selPoint1.X - pointGapLine1, selPoint1.Y ), new Point3D(textCenter.X - extLine1, selPoint1.Y ));
                    dimLine2 = new Line(new Point3D(selPoint2.X - pointGapLine2, selPoint2.Y ), new Point3D(textCenter.X - extLine2, selPoint2.Y));
                    Plane planeLeft = new Plane(new Point3D(selPoint1.X, selPoint1.Y), Vector3D.AxisY, -1 * Vector3D.AxisX);
                    dimText = new Text(planeLeft,new Point3D(textCenter.X-textGap, textCenter.Y), selDimtext, selTextHeight);
                    dimText.Alignment = Text.alignmentType.BaselineCenter;
                    //dimText.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
                    tri1 = new Triangle(new Point3D(textCenter.X, selPoint1.Y), new Point3D(textCenter.X - selArrowHeight / 2, selPoint1.Y + selArrowHeight * 3), new Point3D(textCenter.X + selArrowHeight / 2, selPoint1.Y + selArrowHeight * 3));
                    tri2 = new Triangle(new Point3D(textCenter.X, selPoint2.Y), new Point3D(textCenter.X - selArrowHeight / 2, selPoint2.Y - selArrowHeight * 3), new Point3D(textCenter.X + selArrowHeight / 2, selPoint2.Y - selArrowHeight * 3));

                    break;

                case "RIGHT":
                    textCenter.X = Math.Min(selPoint1.X, selPoint2.X) + selDimHeight;
                    textCenter.Y = selPoint1.Y + (selPoint2.Y - selPoint1.Y) / 2;

                    arrowLine = new Line(new Point3D(textCenter.X, selPoint1.Y), new Point3D(textCenter.X, selPoint2.Y));
                    dimLine1 = new Line(new Point3D(selPoint1.X + pointGapLine1, selPoint1.Y), new Point3D(textCenter.X + extLine1, selPoint1.Y));
                    dimLine2 = new Line(new Point3D(selPoint2.X + pointGapLine2, selPoint2.Y), new Point3D(textCenter.X + extLine2, selPoint2.Y));
                    Plane planeLeft2 = new Plane(new Point3D(selPoint1.X, selPoint1.Y), Vector3D.AxisY, -1 * Vector3D.AxisX);
                    dimText = new Text(planeLeft2, new Point3D(textCenter.X - textGap, textCenter.Y), selDimtext, selTextHeight);
                    dimText.Alignment = Text.alignmentType.BaselineCenter;
                    //dimText.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
                    tri1 = new Triangle(new Point3D(textCenter.X, selPoint1.Y), new Point3D(textCenter.X - selArrowHeight / 2, selPoint1.Y + selArrowHeight * 3), new Point3D(textCenter.X + selArrowHeight / 2, selPoint1.Y + selArrowHeight * 3));
                    tri2 = new Triangle(new Point3D(textCenter.X, selPoint2.Y), new Point3D(textCenter.X - selArrowHeight / 2, selPoint2.Y - selArrowHeight * 3), new Point3D(textCenter.X + selArrowHeight / 2, selPoint2.Y - selArrowHeight * 3));

                    break;

            }

            tri1.Color = Color.Blue;
            tri1.ColorMethod = colorMethodType.byLayer;
            
            newList.Add(arrowLine);
            newList.Add(dimLine1);
            newList.Add(dimLine2);
            newList.Add(dimText);
            newList.Add(tri1);
            newList.Add(tri2);

            return newList;
        }

        #endregion

        #region Leader 최신
        public List<Entity> GetLeader(Point3D selPoint1, double selLength, string selPostion, List<string> selText, List<string> selTextSub,Model ssModel)
        {
            List<Entity> newList = new List<Entity>();

            selPoint1.X = 10;
            selPoint1.Y = 10;
            double calDegree = 30;
            double distance =selLength;

            double selArrowHeight = 2.5;
            double textHeight = 2.5;
            double textGap = 1;
            double textLayerHeight = 7;


            Line newLeaderLine=null;
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
                    newLeaderArrow = new Triangle(new Point3D(selPoint1.X, selPoint1.Y), new Point3D(selPoint1.X + selArrowHeight * 3, selPoint1.Y - selArrowHeight / 2), new Point3D(selPoint1.X + selArrowHeight * 3, selPoint1.Y + selArrowHeight / 2));
                    newLeaderArrow.Rotate(Utility.DegToRad(calDegree), Vector3D.AxisZ, new Point3D(selPoint1.X, selPoint1.Y));

                    centerPoint = newLeaderLine.EndPoint;
                    currentTextLayerHeight = 0;
                    foreach (string eachString in selText)
                    {
                        // Text
                        Text newText01 = new Text(new Point3D(centerPoint.X + textGap, centerPoint.Y + currentTextLayerHeight + textGap), eachString, textHeight);
                        newText01.Regen(new RegenParams(0, ssModel));
                        newText01.Alignment = Text.alignmentType.BaselineLeft;

                        // base Line
                        Line newLieDefault = new Line(new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight), new Point3D(centerPoint.X + newText01.BoxSize.X + textGap * 2, centerPoint.Y + currentTextLayerHeight));
                        newList.Add(newText01);
                        newList.Add(newLieDefault);

                        // Vertical LIne
                        if (currentTextLayerHeight > 0)
                        {
                            Line newVertialLine = new Line(new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight - textLayerHeight), new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight));
                            newList.Add(newVertialLine);
                        }

                        currentTextLayerHeight += textLayerHeight;
                    }
                    

                    newList.Add(newLeaderLine);
                    newList.Add(newLeaderArrow);

                    break;

                case "bottomright":

                    calDegree = -60;

                    newLeaderLine = new Line(selPoint1.X, selPoint1.Y, selPoint1.X + distance, selPoint1.Y);
                    newLeaderLine.Rotate(Utility.DegToRad(calDegree), Vector3D.AxisZ, new Point3D(selPoint1.X, selPoint1.Y));

                    // arrow
                    newLeaderArrow = new Triangle(new Point3D(selPoint1.X, selPoint1.Y), new Point3D(selPoint1.X + selArrowHeight * 3, selPoint1.Y - selArrowHeight / 2), new Point3D(selPoint1.X + selArrowHeight * 3, selPoint1.Y + selArrowHeight / 2));
                    newLeaderArrow.Rotate(Utility.DegToRad(calDegree), Vector3D.AxisZ, new Point3D(selPoint1.X, selPoint1.Y));

                    centerPoint = newLeaderLine.EndPoint;
                    currentTextLayerHeight = 0;
                    foreach (string eachString in selText)
                    {
                        // Text
                        Text newText01 = new Text(new Point3D(centerPoint.X + textGap, centerPoint.Y + currentTextLayerHeight + textGap), eachString, textHeight);
                        newText01.Regen(new RegenParams(0, ssModel));
                        newText01.Alignment = Text.alignmentType.BaselineLeft;

                        // base Line
                        Line newLieDefault = new Line(new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight), new Point3D(centerPoint.X + newText01.BoxSize.X + textGap * 2, centerPoint.Y + currentTextLayerHeight));
                        newList.Add(newText01);
                        newList.Add(newLieDefault);

                        // Vertical LIne
                        if (currentTextLayerHeight < 0)
                        {
                            Line newVertialLine = new Line(new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight + textLayerHeight), new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight));
                            newList.Add(newVertialLine);
                        }




                        currentTextLayerHeight -= textLayerHeight;
                    }


                    newList.Add(newLeaderLine);
                    newList.Add(newLeaderArrow);

                    break;

                case "topleft":

                    calDegree = 120;

                    newLeaderLine = new Line(selPoint1.X, selPoint1.Y, selPoint1.X + distance, selPoint1.Y);
                    newLeaderLine.Rotate(Utility.DegToRad(calDegree), Vector3D.AxisZ, new Point3D(selPoint1.X, selPoint1.Y));

                    // arrow
                    newLeaderArrow = new Triangle(new Point3D(selPoint1.X, selPoint1.Y), new Point3D(selPoint1.X + selArrowHeight * 3, selPoint1.Y - selArrowHeight / 2), new Point3D(selPoint1.X + selArrowHeight * 3, selPoint1.Y + selArrowHeight / 2));
                    newLeaderArrow.Rotate(Utility.DegToRad(calDegree), Vector3D.AxisZ, new Point3D(selPoint1.X, selPoint1.Y));

                    centerPoint = newLeaderLine.EndPoint;
                    currentTextLayerHeight = 0;
                    foreach (string eachString in selText)
                    {
                        // Text
                        Text newText01 = new Text(new Point3D(centerPoint.X - textGap, centerPoint.Y + currentTextLayerHeight + textGap), eachString, textHeight);
                        newText01.Regen(new RegenParams(0, ssModel));
                        newText01.Alignment = Text.alignmentType.BaselineRight;

                        // base Line
                        Line newLieDefault = new Line(new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight), new Point3D(centerPoint.X - newText01.BoxSize.X - textGap * 2, centerPoint.Y + currentTextLayerHeight));
                        newList.Add(newText01);
                        newList.Add(newLieDefault);

                        // Vertical LIne
                        if (currentTextLayerHeight > 0)
                        {
                            Line newVertialLine = new Line(new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight - textLayerHeight), new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight));
                            newList.Add(newVertialLine);
                        }

                        currentTextLayerHeight += textLayerHeight;
                    }


                    newList.Add(newLeaderLine);
                    newList.Add(newLeaderArrow);

                    break;


                case "bottomleft":

                    calDegree = -120;

                    newLeaderLine = new Line(selPoint1.X, selPoint1.Y, selPoint1.X + distance, selPoint1.Y);
                    newLeaderLine.Rotate(Utility.DegToRad(calDegree), Vector3D.AxisZ, new Point3D(selPoint1.X, selPoint1.Y));

                    // arrow
                    newLeaderArrow = new Triangle(new Point3D(selPoint1.X, selPoint1.Y), new Point3D(selPoint1.X + selArrowHeight * 3, selPoint1.Y - selArrowHeight / 2), new Point3D(selPoint1.X + selArrowHeight * 3, selPoint1.Y + selArrowHeight / 2));
                    newLeaderArrow.Rotate(Utility.DegToRad(calDegree), Vector3D.AxisZ, new Point3D(selPoint1.X, selPoint1.Y));

                    centerPoint = newLeaderLine.EndPoint;
                    currentTextLayerHeight = 0;
                    foreach (string eachString in selText)
                    {
                        // Text
                        Text newText01 = new Text(new Point3D(centerPoint.X - textGap, centerPoint.Y + currentTextLayerHeight + textGap), eachString, textHeight);
                        newText01.Regen(new RegenParams(0, ssModel));
                        newText01.Alignment = Text.alignmentType.BaselineRight;

                        // base Line
                        Line newLieDefault = new Line(new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight), new Point3D(centerPoint.X - newText01.BoxSize.X - textGap * 2, centerPoint.Y + currentTextLayerHeight));
                        newList.Add(newText01);
                        newList.Add(newLieDefault);

                        // Vertical LIne
                        if (currentTextLayerHeight < 0)
                        {
                            Line newVertialLine = new Line(new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight + textLayerHeight), new Point3D(centerPoint.X, centerPoint.Y + currentTextLayerHeight));
                            newList.Add(newVertialLine);
                        }

                        currentTextLayerHeight -= textLayerHeight;
                    }


                    newList.Add(newLeaderLine);
                    newList.Add(newLeaderArrow);

                    break;

            }

            return newList;
        }
        #endregion
    }
}

