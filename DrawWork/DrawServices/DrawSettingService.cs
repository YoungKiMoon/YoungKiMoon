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
using MColor = System.Windows.Media.Color;
using Color = System.Drawing.Color;
using Environment = devDept.Eyeshot.Environment;
using System.Collections.ObjectModel;
using DrawWork.DrawModels;
using DrawWork.DrawAutomationService;
using System.Windows;
using DrawWork.Commons;

namespace DrawWork.DrawServices
{
    public class DrawSettingService
    {
        public DrawSettingService()
        {

        }
        public void SetModelSpace(Model singleModel)
        {
            singleModel.Invalidate();

            string LayerDashDot = "DashDot";
            singleModel.Layers.Add(new Layer(LayerDashDot, Color.CornflowerBlue));
            singleModel.Layers[LayerDashDot].Color = Color.Red;
            singleModel.Layers[LayerDashDot].LineWeight = 4;

            string LayerDashDot1 = "DashDot1";
            singleModel.Layers.Add(new Layer(LayerDashDot1, Color.Blue));
            singleModel.Layers[LayerDashDot1].Color = Color.Red;
            singleModel.Layers[LayerDashDot1].LineWeight = 4;
            

            singleModel.LineTypes.Add(LayerDashDot, new float[] { 5, -1, 1, -1 });
            singleModel.Layers[LayerDashDot].LineTypeName = LayerDashDot;

            //singleModel.LineTypes.Add(LayerDashDot1, new float[] { 5, -1, 1, -1 });
            singleModel.Layers[LayerDashDot1].LineWeight = 0.5f;
        }

        public void CreateModelSpaceSample(Model singleModel)
        {
            singleModel.Entities.Clear();


            if (false)
            {

                #region 샘플 라인 테스트
                if (false)
                {


                    LinearPath rectBox = new LinearPath(140, 100);
                    rectBox.ColorMethod = colorMethodType.byEntity;
                    rectBox.Color = Color.Green;
                    singleModel.Entities.Add(rectBox);

                    Line cusL = new Line(0, 100, 70, 120);
                    singleModel.Entities.Add(cusL);
                    Line cusR = new Line(70, 120, 140, 100);
                    singleModel.Entities.Add(cusR);


                    LinearPath rectBoxSmall = new LinearPath(20, 16);
                    rectBoxSmall.Translate(180, 0);
                    rectBoxSmall.ColorMethod = colorMethodType.byEntity;
                    rectBoxSmall.Color = Color.Red;
                    singleModel.Entities.Add(rectBoxSmall);

                    //Line cusH = new Line(-500, 0, 500, 0);
                    //Line cusV = new Line(0, -500, 0, 500);
                    //Line cusHV = new Line(-500, -500, 500, 500);
                    //singleModel.Entities.Add(cusH, LayerDashDot);
                    //singleModel.Entities.Add(cusV, LayerDashDot);
                    //singleModel.Entities.Add(cusHV, LayerDashDot);
                }
                #endregion

                #region Composite curve
                if (false)
                {


                    //six composite curve
                    Arc arc = new Arc(0, 0, 0, 113, (Math.PI / 3), (Math.PI * 2 / 3));
                    singleModel.Entities.Add(new Arc(arc.Center, 113, Utility.DegToRad(45), Utility.DegToRad(70)));

                    Line line1 = new Line(-45, 113, -45, 0);
                    //Line line2 = new Line(55, 49, 55, 113); //error
                    Line line2 = new Line(-40, 49, 15, 49);
                    line2.Rotate(Utility.DegToRad(30), Vector3D.AxisZ, new Point3D(-45, 49));
                    singleModel.Entities.Add((Line)line2.Clone(), Color.Blue);

                    singleModel.Entities.Add((Line)line1.Clone(), Color.Green);

                    Arc arcFillet1, arcFillet2, arcFillet3;
                    Curve.Fillet(arc, line1, 9.5, false, false, true, true, out arcFillet1);
                    Curve.Fillet(arc, line2, 9.5, false, false, true, true, out arcFillet2);
                    Curve.Fillet(line1, line2, 9.5, false, false, true, true, out arcFillet3);

                    singleModel.Entities.Add(arcFillet3, Color.Red);
                    //CompositeCurve compositeCurve = new CompositeCurve(arc, line1, line2, arcFillet1, arcFillet2, arcFillet3);
                    //singleModel.Entities.Add(compositeCurve);

                    //for (int i = 0; i < 6; i++)
                    //{
                    //    CompositeCurve compCurve = (CompositeCurve)compositeCurve.Clone();
                    //    compCurve.Rotate((Math.PI / 3) * i, Vector3D.AxisZ);
                    //    singleModel.Entities.Add(compCurve);
                    //}
                }
                #endregion

                #region 병따게 Test
                if (false)
                {


                    // adding the axis of symmetry
                    Line axisX = new Line(-30, 0, 90, 0);
                    axisX.LineTypeMethod = colorMethodType.byEntity;
                    singleModel.Entities.Add(axisX);

                    // drawing segment and arcs
                    Line ln1 = new Line(76, 0, 76, 3);
                    singleModel.Entities.Add(ln1, Color.Red);

                    Arc a1 = new Arc(new Point3D(70, 3, 0), 6, 0, Math.PI / 2);
                    singleModel.Entities.Add(a1, Color.Green);

                    Line ln2 = new Line(70, 9, 28, 9);
                    singleModel.Entities.Add(ln2, Color.AliceBlue);

                    Arc a2 = new Arc(new Point3D(12.52, 9, 0), 1, Math.PI / 2, -Math.PI / 2);
                    singleModel.Entities.Add(a2, Color.Orange);

                    Line ln3 = new Line(-2.5, 0, -2.5, 14);
                    singleModel.Entities.Add(ln3, Color.Black);
                    Line ln4 = new Line(-7.5, 0, -7.5, 14);
                    singleModel.Entities.Add(ln4, Color.Black);

                    Circle cr1 = new Circle(30, 28, 0, 19);
                    Arc a3 = new Arc(new Point3D(0, 14, 0), 7.5, Math.PI);
                    Arc a4 = new Arc(new Point3D(0, 14, 0), 2.5, Math.PI);

                    singleModel.Entities.Add(cr1, Color.Green);
                    singleModel.Entities.Add(a3, Color.Red);
                    singleModel.Entities.Add(a4, Color.Blue);

                    // finding the tangent to two circles
                    Line ln5 = UtilityEx.GetLinesTangentToTwoCircles(cr1, a3)[1];
                    singleModel.Entities.Add(ln5, Color.Blue);
                    Line ln6 = UtilityEx.GetLinesTangentToTwoCircles(a2, a4)[0];
                    singleModel.Entities.Add(ln6, Color.Red);


                    Line lnTest = UtilityEx.GetLinesTangentToCircleFromPoint(a4, new Point3D(2, 0, 0))[0];
                    singleModel.Entities.Add(lnTest, Color.Green);

                    // creating a Fillet
                    Arc a5;
                    Curve.Fillet(ln2, ln5, 19, false, false, true, true, out a5);
                    a5.Reverse();

                    double angle = Vector3D.AngleBetween(a5.Plane.AxisX, cr1.Plane.AxisX);

                    a5.Rotate(angle, a5.Plane.AxisZ, a5.Center);
                    a5.Domain = new Interval(a5.Domain.t0 - angle, a5.Domain.t1 - angle);


                    //singleModel.Entities.Add(a5, Color.Blue);

                    // trimming the curves
                    //Curve.Trim(a3, ln5, true, true);
                    //singleModel.Entities.Add(a3, Color.Black);

                    //Curve.Trim(a4, ln6, true, true);
                    //singleModel.Entities.Add(a4, Color.Black);
                }
                #endregion

                #region 호 그리기 : Arc 사용
                if (false)
                {
                    Arc testArc = new Arc(new Point3D(0, 0, 0), new Point3D(20, 0, 0), new Point3D(0, 20, 0));
                    singleModel.Entities.Add(testArc, Color.Red);
                }

                #endregion

                #region 자르기
                if (false)
                {
                    Circle c1 = new Circle(Plane.XY, new Point2D(0, 0), 40); // Outer circle top right
                    Circle c2 = new Circle(Plane.XY, new Point2D(0, -90), 20); // Outer circle bottom
                    Circle c3 = new Circle(Plane.XY, new Point2D(-41, 0), 20); // Outer circle top left 

                    // Linking arcs
                    Arc a98 = (Arc)UtilityEx.GetCirclesTangentToTwoCircles(c1, c2, 98, true)[3]; // Radius 98 arc
                    Arc a20 = (Arc)UtilityEx.GetCirclesTangentToTwoCircles(c1, c2, 20, true)[1]; // Radius 20 arc
                    Arc a9 = (Arc)UtilityEx.GetCirclesTangentToTwoCircles(c3, c1, 9, true)[0]; // Radius 9 arc

                    Arc a1 = new Arc(c1.Center, c1.Radius, Math.PI * 2);
                    a1.TrimBy(a9.EndPoint, false);
                    a1.TrimBy(a98.StartPoint, true);

                    //Arc a2 = new Arc(c2.Center, c2.Radius, Math.PI * 2);
                    //a2.TrimBy(a98.EndPoint, false);
                    //a2.TrimBy(a20.EndPoint, true);

                    //singleModel.Entities.Add(c1, Color.Yellow);
                    singleModel.Entities.Add(a9, Color.Red);
                    singleModel.Entities.Add(a98, Color.Green);
                    singleModel.Entities.Add(a1, Color.Blue);

                }
                #endregion

                #region Angle
                if (false)
                {
                    // Current Test
                    double AB = 50;
                    double t = 6;
                    double R1 = 6.5;
                    double R2 = 4.5;
                    double CD = 14.4;
                    double E = 10;

                    Point3D drawPoint = new Point3D(0, 0, 0);

                    Line lineA = new Line(new Point3D(0, AB, 0), new Point3D(AB, AB, 0));
                    Line lineAa = new Line(new Point3D(0, AB - t, 0), new Point3D(AB - t, AB - t, 0));
                    Line lineAt = new Line(new Point3D(0, AB, 0), new Point3D(0, AB - t, 0));
                    Line lineB = new Line(new Point3D(AB, AB, 0), new Point3D(AB, 0, 0));
                    Line lineBb = new Line(new Point3D(AB - t, AB - t, 0), new Point3D(AB - t, 0, 0));
                    Line lineBt = new Line(new Point3D(AB - t, 0, 0), new Point3D(AB, 0, 0));


                    //singleModel.Entities.Add(lineB, Color.Red);
                    //singleModel.Entities.Add(lineBb, Color.Green);

                    Arc arcFillet1;
                    if (Curve.Fillet(lineAt, lineAa, R2, false, false, true, true, out arcFillet1))
                        singleModel.Entities.Add(arcFillet1, Color.Blue);
                    Arc arcFillet2;
                    if (Curve.Fillet(lineBb, lineBt, R2, false, false, true, true, out arcFillet2))
                        singleModel.Entities.Add(arcFillet2, Color.Blue);
                    Arc arcFillet3;
                    if (Curve.Fillet(lineAa, lineBb, R1, false, false, true, true, out arcFillet3))
                        singleModel.Entities.Add(arcFillet3, Color.Blue);

                    singleModel.Entities.Add(lineA, Color.Red);
                    singleModel.Entities.Add(lineAa, Color.Green);
                    singleModel.Entities.Add(lineAt, Color.Blue);
                    singleModel.Entities.Add(lineB, Color.Red);
                    singleModel.Entities.Add(lineBb, Color.Green);
                    singleModel.Entities.Add(lineBt, Color.Blue);
                }
                #endregion

                #region Nozzle
                if (false)
                {
                    // Current Test
                    double flangeFaceWidth = 10;
                    double flangeFaceHeight = 80;
                    double flangeFaceInnerWidth = 10;
                    double flangePipeWidth = 24;
                    double flangePipeHeight = flangeFaceHeight - (flangeFaceInnerWidth * 2);
                    double flangePipeInnerWidth = 18;
                    double pipeHeight = flangeFaceHeight - (flangePipeInnerWidth * 2);
                    double pipeWidth = 40;
                    double fullWidth = flangeFaceWidth + flangePipeWidth + pipeWidth;

                    Point3D drawPoint = new Point3D(0, 0, 0);

                    Line lineFFa = new Line(new Point3D(0, 0, 0), new Point3D(0, flangeFaceHeight, 0));
                    Line lineFFb = new Line(new Point3D(flangeFaceWidth, 0, 0), new Point3D(flangeFaceWidth, flangeFaceHeight, 0));
                    Line lineFFc = new Line(new Point3D(0, 0, 0), new Point3D(flangeFaceWidth, 0, 0));
                    Line lineFFd = new Line(new Point3D(0, flangeFaceHeight, 0), new Point3D(flangeFaceWidth, flangeFaceHeight, 0));

                    Line lineFPa = new Line(new Point3D(flangeFaceWidth, flangeFaceInnerWidth + flangePipeHeight, 0), new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth + pipeHeight, 0));
                    Line lineFPb = new Line(new Point3D(flangeFaceWidth, flangeFaceInnerWidth, 0), new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0));
                    Line lineFPc = new Line(new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth + pipeHeight, 0), new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0));

                    Line linePa = new Line(new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0), new Point3D(flangeFaceWidth + flangePipeWidth + pipeWidth, flangePipeInnerWidth, 0));
                    Line linePb = new Line(new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth + pipeHeight, 0), new Point3D(flangeFaceWidth + flangePipeWidth + pipeWidth, flangePipeInnerWidth + pipeHeight, 0));

                    singleModel.Entities.AddRange(new Entity[] { lineFFa, lineFFb, lineFFc, lineFFd, lineFPa, lineFPb, lineFPc, linePa, linePb });
                }
                #endregion

                #region Nozzle Leader
                if (false)
                {
                    // Current Test
                    double cirRadius = 100;
                    double cirDiameter = cirRadius * 2;
                    double cirTextSize = 30;
                    string textUpperStr = "asdf";
                    string textLowerStr = "dadsf";
                    Point3D drawPoint = new Point3D(0, 0, 0);

                    Circle circleCenter = new Circle(cirRadius, cirRadius, 0, cirRadius);
                    Line lineCenter = new Line(new Point3D(0, cirRadius, 0), new Point3D(cirDiameter, cirRadius, 0));
                    Line lineCenter3 = new Line(new Point3D(0, cirRadius, 0), new Point3D(cirDiameter, cirRadius - 20, 0));
                    Line lineCenter2 = new Line(new Point3D(0, cirRadius, 0), new Point3D(cirDiameter, cirRadius + 20, 0));
                    Vector3D ssPlane = new Vector3D();
                    ssPlane.Y = 200;
                    Line lineCenter4 = (Line)lineCenter2.Offset(500, ssPlane);
                    Line lineCenter5 = (Line)lineCenter3.Offset(-500, Vector3D.AxisZ);
                    Text textUpper = new Text(cirRadius, cirRadius + (cirRadius / 2), 0, textUpperStr, cirTextSize);
                    textUpper.Alignment = Text.alignmentType.MiddleCenter;
                    Text textLower = new Text(cirRadius, (cirRadius / 2), 0, textUpperStr, cirTextSize);
                    textLower.Alignment = Text.alignmentType.MiddleCenter;

                    singleModel.Entities.AddRange(new Entity[] { circleCenter, lineCenter, lineCenter2, lineCenter3, lineCenter4, lineCenter5, textUpper, textLower });
                }
                #endregion

                #region Dimension
                if (false)
                {

                    // Y axis
                    Line axisY = new Line(0, -45, 0, 45)
                    {
                        LineTypeMethod = colorMethodType.byEntity,
                    };


                    Mirror m = new Mirror(Plane.ZY);

                    // Building geometry - External Outline

                    Line lT1 = new Line(34.8, 30.2, 34.8, 44.6);
                    Line lT2 = new Line(28.8, 30.2, 28.8, 44.6);
                    Line lT3 = new Line(25.6, 32.6, 0.0, 32.6);
                    Circle c = new Circle(Plane.XY, new Point3D(31.8, 28, 0), 13.2);
                    Circle c_mir = (Circle)c.Clone();
                    c_mir.TransformBy(m);
                    Point3D int1 = Curve.Intersection(c, lT1)[0];
                    Point3D int2 = Curve.Intersection(c, lT2)[0];
                    Point3D int3 = Curve.Intersection(c, lT3)[0];

                    singleModel.Entities.AddRange(new Entity[] { axisY, lT1, lT2, lT3, c, c_mir });
                    // External outline
                    Line eo1 = new Line(0.0, -36, 17.8, -36);
                    Line eo2 = new Line(17.8, -36, 17.8, -42);
                    Line eo3 = new Line(17.8, -42, 51, -42);
                    Line eo4 = new Line(51, -42, 51, -12.6);
                    Line eo5 = new Line(51, -12.6, 43.8, -12.6);
                    Line eo7 = new Line(43.8, 0.6, 51, 0.6);
                    Arc eo6 = new Arc(Plane.XY, new Point3D(43.8, -6.0, 0.0), 6.6, eo5.EndPoint, eo7.StartPoint, true);
                    Line eo8 = new Line(51, 0.6, 51, 22.6);
                    Line eo9 = new Line(51, 22.6, 45, 22.6);
                    Line eo10 = new Line(45, 22.6, 45, 28);
                    Arc eo11 = new Arc(Plane.XY, new Point3D(31.8, 28, 0), 13.2, eo10.EndPoint, int1, false);
                    Line eo12 = new Line(int1.X, int1.Y, 34.8, 44.6);
                    Line eo13 = new Line(34.8, 44.6, 28.8, 44.6);
                    Line eo14 = new Line(28.8, 44.6, int2.X, int2.Y);
                    Arc eo15 = new Arc(Plane.XY, new Point3D(31.8, 28, 0), 13.2, int2, int3, false);
                    Line eo16 = new Line(int3.X, int3.Y, 0, int3.Y);


                    singleModel.Entities.AddRange(new Entity[] { eo1, eo2, eo3, eo4, eo5, eo6, eo7, eo8, eo9, eo10, eo11, eo12, eo13, eo14, eo15, eo16 });

                    // Holes
                    Circle c1 = new Circle(Plane.XY, new Point3D(31.8, 28, 0), 6);
                    Circle c2 = (Circle)c1.Clone();
                    c2.TransformBy(m);
                    CompositeCurve h1 = CompositeCurve.CreateHexagon(31.8, 28, 6);
                    //h1.Rotate(Math.PI / 6, Vector3D.AxisZ, new Point3D(31.8, 28, 0));
                    h1.TransformBy(m);

                    singleModel.Entities.AddRange(new Entity[] { h1 });




                    float txtH = 2.0f;
                    DiametricDim ddd1 = new DiametricDim(c1, new Point3D(20.0, 20.0, 0.0), txtH)
                    {
                        ArrowsLocation = elementPositionType.Outside,
                        TextPrefix = "2-Ø"
                    };

                    singleModel.Entities.AddRange(new Entity[] { ddd1 });


                    Plane left = new Plane(Point3D.Origin, Vector3D.AxisY, -1 * Vector3D.AxisX); // plane.XY, vertical writing;
                    Plane right = new Plane(Point3D.Origin, Vector3D.AxisY, -1 * Vector3D.AxisX); // plane.XY, vertical writing;

                    LinearDim dd1 = new LinearDim(Plane.XY, new Point2D(-28.8, -10), new Point2D(-34.8, -10), new Point2D(-38.0, 49.6), txtH)
                    {
                        ArrowsLocation = elementPositionType.Outside
                    };

                    LinearDim dd2 = new LinearDim(Plane.XY, new Point2D(-28, -10), new Point2D(-34, -10), new Point2D(-38.0, 30), txtH)
                    {
                        ArrowsLocation = elementPositionType.Outside
                    };

                    LinearDim dd3 = new LinearDim(Plane.XY, new Point2D(-28, -10), new Point2D(-34, -10), new Point2D(0, 30), txtH)
                    {
                        ArrowsLocation = elementPositionType.Inside
                    };


                    LinearDim dd4 = new LinearDim(left, new Point2D(-36.0, -4.0), new Point2D(-15.57, -22.9), new Point2D(-25.79, -4.0), txtH)
                    {
                        ArrowsLocation = elementPositionType.Inside
                    };

                    LinearDim dd5 = new LinearDim(Plane.XY, new Point2D(-36.0, -4.0), new Point2D(-15.57, -22.9), new Point2D(-25.79, -4.0), txtH)
                    {
                        ArrowsLocation = elementPositionType.Inside
                    };

                    LinearDim dd6 = new LinearDim(Plane.YX, new Point2D(43.8, -6.0), new Point2D(51.0, -20.0), new Point2D(54.0, -20.0), txtH)
                    {
                        ArrowsLocation = elementPositionType.Outside
                    };

                    //LinearDim dimTop = new LinearDim(Plane.XY, new Point2D(0, 19300), new Point2D(32400, 19300), new Point2D(16200,23300), 1000)
                    LinearDim dimTop = new LinearDim(Plane.XY, new Point2D(-10, 20), new Point2D(50, 20), new Point2D(25, 30), txtH)
                    {
                        ArrowsLocation = elementPositionType.Inside
                    };
                    Plane ccleft = new Plane(new Point3D(0, 0), Vector3D.AxisY, -1 * Vector3D.AxisX); // plane.XY, vertical writing;
                    // 0,10  0,50  -10, 25
                    LinearDim dd8 = new LinearDim(ccleft, new Point2D(10, 0), new Point2D(50, 0), new Point2D(25, 10), txtH)
                    {
                        ArrowsLocation = elementPositionType.Inside
                    };
                    Plane ccright = new Plane(new Point3D(0, 0), -1 * Vector3D.AxisY, Vector3D.AxisX); // plane.XY, vertical writing;
                    // 20,10  20,-50  30, -25
                    LinearDim dd9 = new LinearDim(ccright, new Point2D(-10, 20), new Point2D(50, 20), new Point2D(25, 30), txtH)
                    {
                        ArrowsLocation = elementPositionType.Inside
                    };

                    Plane ccbottom = new Plane(new Point3D(0, 0), -1 * Vector3D.AxisY, Vector3D.AxisX); // plane.XY, vertical writing;
                    // 20,10  20,-50  30, -25
                    LinearDim dd10 = new LinearDim(ccbottom, new Point2D(-10, 20), new Point2D(50, 20), new Point2D(25, 30), txtH)
                    {
                        ArrowsLocation = elementPositionType.Inside
                    };

                    singleModel.Entities.Add(dd1, Color.Red);
                    singleModel.Entities.Add(dd2, Color.Green);
                    singleModel.Entities.Add(dd3, Color.Blue);
                    singleModel.Entities.Add(dd4, Color.Yellow);
                    singleModel.Entities.Add(dd5, Color.YellowGreen);
                    singleModel.Entities.Add(dd6, Color.Orange);

                    singleModel.Entities.Add(dimTop, Color.SkyBlue);
                    //singleModel.Entities.Add(dd8, Color.OrangeRed);
                    singleModel.Entities.Add(dd9, Color.LightSeaGreen);
                    singleModel.Entities.Add(dd10, Color.Red);

                    LinearDim dimTop2 = new LinearDim(Plane.XY, new Point2D(-10, 20), new Point2D(50, 20), new Point2D(25, 30), txtH)
                    {
                        ArrowsLocation = elementPositionType.Inside
                    };
                    dimTop2.ScaleOverall = 3;
                    singleModel.Entities.Add(dimTop2, Color.Red);
                    //singleModel.Entities.AddRange(new Entity[] { dd1,dd2,dd3,dd4,dd5,dd6 });

                }
                #endregion

                #region Dimensino 2
                if (false)
                {


                    Point3D leaderPoint = new Point3D(0, 0, 0);
                    double leaderLength = 15;
                    double leaderTriShort = leaderLength / 2;
                    double leaderTriLong = Math.Tan(UtilityEx.DegToRad(60)) * leaderTriShort;
                    double leaderCenterX = leaderPoint.X + leaderTriShort;
                    double leaderCenterY = leaderPoint.Y + leaderTriLong;

                    List<Point3D> leaderPoints = new List<Point3D>();
                    leaderPoints.Add(leaderPoint);
                    leaderPoints.Add(new Point3D(leaderCenterX, leaderCenterY, 0));
                    leaderPoints.Add(new Point3D(leaderCenterX + 40, leaderCenterY, 0));
                    Leader leader1 = new Leader(Plane.XY, leaderPoints);
                    singleModel.Entities.Add(leader1, Color.Red);

                    //leader1.Scale = 30;
                    //leader1.LineTypeScale = 30;
                    Text newText = new Text(leaderCenterX + 1, leaderCenterY, 0, "LAaBbCc", 2.5);
                    newText.Alignment = Text.alignmentType.BaselineLeft;
                    //singleModel.FontFamily.FamilyNames= "ROMANS";

                    Text newText2 = new Text(leaderCenterX + 1, leaderCenterY + 1, 0, "LAaBbCc", 2.5);
                    newText2.Alignment = Text.alignmentType.BaselineLeft;

                    singleModel.Entities.Add(newText, Color.Blue);
                    singleModel.Entities.Add(newText2, Color.Red);


                    Point3D textCenter = new Point3D();
                    LinearDim newDimRight;
                    LinearDim newDimLeft;
                    LinearDim newDimTop;

                    LinearDim newDimBottom;

                    CDPoint selPoint1 = new CDPoint();
                    CDPoint selPoint2 = new CDPoint();
                    CDPoint selPoint3 = new CDPoint();


                    // Top
                    selPoint1.X = 110;
                    selPoint1.Y = 200;
                    selPoint2.X = 160;
                    selPoint2.Y = 220;
                    double selDimHeight = 34;
                    double selTextHeight = 2.5;

                    textCenter.X = selPoint1.X + (selPoint2.X - selPoint1.X) / 2;
                    textCenter.Y = Math.Max(selPoint1.Y, selPoint2.Y) + selDimHeight;
                    newDimTop = new LinearDim(Plane.XY, new Point3D(selPoint1.X, selPoint1.Y),
                                            new Point3D(selPoint2.X, selPoint2.Y),
                                            new Point3D(textCenter.X, textCenter.Y), selTextHeight);

                    newDimTop.ArrowsLocation = elementPositionType.Inside;
                    newDimTop.TextPrefix = "t";
                    newDimTop.TextSuffix = "s";
                    newDimTop.TextOverride = "asdfads";

                    // Text Gap
                    newDimTop.TextGap = 1;
                    newDimTop.ExtLineOffset = 1;
                    newDimTop.ExtLineExt = 1;
                    newDimTop.ArrowheadSize = 2.5;
                    newDimTop.ScaleOverall = 1;
                    newDimTop.LinearScale = 1;

                    
                    newDimTop.Regen(new RegenParams(0, singleModel));
                    //Mesh[] ss1 = newDimTop.ConvertToMesh(singleModel);
                    ICurve[] ss2 = newDimTop.ConvertToCurves(singleModel);
                    LinearPath[] ss3 = newDimTop.ConvertToLinearPaths(0,singleModel); // 외곽
                    Region[] ss4 = newDimTop.ConvertToRegions(singleModel);
                    Surface[] ss5 = newDimTop.ConvertToSurfaces(singleModel);

                    //singleModel.Entities.AddRange(ss5);
                    foreach (ICurve each in ss2)
                        singleModel.Entities.Add(each as Entity);

                    
                                        
                    // Left
                    selPoint1.X = 110;
                    selPoint1.Y = 200;
                    selPoint2.X = 90;
                    selPoint2.Y = 250;

                    selPoint1.X = -10;
                    selPoint1.Y = 200;
                    selPoint2.X = 10;
                    selPoint2.Y = 250;

                    textCenter.X = selPoint1.Y + (selPoint2.Y - selPoint1.Y) / 2;
                    textCenter.Y = Math.Min(selPoint1.X, selPoint2.X) - selDimHeight;
                    Plane planeLeft = new Plane(new Point3D(selPoint1.X, selPoint1.Y), Vector3D.AxisY, -1 * Vector3D.AxisX);
                    newDimLeft = new LinearDim(planeLeft, new Point3D(selPoint1.X, selPoint1.Y),
                                            new Point3D(selPoint2.X, selPoint2.Y),
                                            new Point3D(textCenter.Y, textCenter.X), selTextHeight);

                    newDimLeft.ArrowsLocation = elementPositionType.Inside;


                    // right
                    selPoint1.X = 110;
                    selPoint1.Y = 200;
                    selPoint2.X = 90;
                    selPoint2.Y = 250;

                    textCenter.X = selPoint1.Y + (selPoint2.Y - selPoint1.Y) / 2;
                    textCenter.Y = Math.Max(selPoint1.X, selPoint2.X) + selDimHeight;
                    Plane planeRight = new Plane(new Point3D(selPoint1.X, selPoint1.Y), Vector3D.AxisY, -1 * Vector3D.AxisX);
                    newDimRight = new LinearDim(planeRight, new Point3D(selPoint1.X, selPoint1.Y),
                                            new Point3D(selPoint2.X, selPoint2.Y),
                                            new Point3D(textCenter.Y, textCenter.X), selTextHeight);

                    newDimLeft.ArrowsLocation = elementPositionType.Inside;


                    // bottom
                    selPoint1.X = 110;
                    selPoint1.Y = 200;
                    selPoint2.X = 160;
                    selPoint2.Y = 220;

                    textCenter.X = selPoint1.X + (selPoint2.X - selPoint1.X) / 2;
                    textCenter.Y = Math.Min(selPoint1.Y, selPoint2.Y) - selDimHeight;
                    newDimBottom = new LinearDim(Plane.XY, new Point3D(selPoint1.X, selPoint1.Y),
                                            new Point3D(selPoint2.X, selPoint2.Y),
                                            new Point3D(textCenter.X, textCenter.Y), selTextHeight);

                    newDimBottom.ArrowsLocation = elementPositionType.Inside;

                    //singleModel.Entities.Add(newDimTop, Color.Red);
                    singleModel.Entities.Add(newDimBottom, Color.Gray);

                    singleModel.Entities.Add(newDimRight, Color.Blue);
                    singleModel.Entities.Add(newDimLeft, Color.Green);
                }
                #endregion

                #region Line Break
                if (false)
                {




                }
                #endregion

                #region Line Break : ing
                if (false)
                {


                    AutomationDimensionService autoDimService = new AutomationDimensionService();

                    // Reference Circle
                    double breakRadius = 10;

                    List<Entity> outlineList = new List<Entity>();
                    List<Entity> centerlineList = new List<Entity>();
                    List<Entity> dimlineList = new List<Entity>();
                    List<Entity> dimTextList = new List<Entity>();
                    List<Entity> dimlineExtList = new List<Entity>();
                    List<Entity> leaderlineList = new List<Entity>();
                    List<Entity> leaderTextList = new List<Entity>();
                    List<Entity> nozzlelineList = new List<Entity>();
                    List<Entity> nozzleMarkList = new List<Entity>();

                    // 세로
                    outlineList.Add(new Line(new Point3D(10, -20, 0), new Point3D(10, 1110, 0)));
                    centerlineList.Add(new Line(new Point3D(110, -20, 0), new Point3D(110, 1110, 0)));
                    dimlineList.Add(new Line(new Point3D(210, -20, 0), new Point3D(210, 1110, 0)));
                    Text dimText01 = new Text(new Point3D(0, -310, 0), "Dimension Line ABCDEFGabcdefg12345", 50);

                    dimText01.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
                    dimTextList.Add(dimText01);
                    dimlineExtList.Add(new Line(new Point3D(410, -20, 0), new Point3D(410, 1110, 0)));
                    leaderlineList.Add(new Line(new Point3D(510, -20, 0), new Point3D(510, 1110, 0)));
                    Text leaderText01 = new Text(new Point3D(0, -610, 0), "Leader Line ABCDEFGabcdefg12345", 50);
                    leaderText01.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
                    leaderTextList.Add(leaderText01);
                    nozzlelineList.Add(new Line(new Point3D(710, -20, 0), new Point3D(710, 1110, 0)));
                    nozzleMarkList.Add(new Line(new Point3D(810, -20, 0), new Point3D(810, 1110, 0)));

                    // 가로
                    outlineList.Add(new Line(new Point3D(-20, 10, 0), new Point3D(1110, 10, 0)));
                    centerlineList.Add(new Line(new Point3D(-20, 110, 0), new Point3D(1110, 110, 0)));
                    dimlineList.Add(new Line(new Point3D(-20, 210, 0), new Point3D(1110, 210, 0)));
                    Text dimText02 = new Text(new Point3D(-20, 310, 0), "Dimension Line ABCDEFGabcdefg12345", 50);
                    dimTextList.Add(dimText02);
                    dimlineExtList.Add(new Line(new Point3D(-20, 410, 0), new Point3D(1110, 410, 0)));
                    leaderlineList.Add(new Line(new Point3D(-20, 510, 0), new Point3D(1110, 510, 0)));
                    Text leaderText02 = new Text(new Point3D(-20, 610, 0), "Leader Line ABCDEFGabcdefg12345", 50);
                    leaderTextList.Add(leaderText02);
                    nozzlelineList.Add(new Line(new Point3D(-20, 710, 0), new Point3D(1110, 710, 0)));
                    nozzleMarkList.Add(new Line(new Point3D(-20, 810, 0), new Point3D(1110, 810, 0)));


                    // Text to Box : 수정 최소단위 사각형으로
                    List<Entity> dimTextLinearPathList = new List<Entity>();
                    foreach (Entity eachEntity in dimTextList)
                    {
                        eachEntity.Regen(new RegenParams(0, singleModel));
                        Point3D[] textBoxSize = eachEntity.Vertices;
                        LinearPath newRec = new LinearPath(textBoxSize);
                        newRec.Regen(new RegenParams(0, singleModel));

                        Point3D minPoint = GetMinPointXY(textBoxSize);
                        Point3D maxPoint = GetMaxPointXY(textBoxSize);
                        if (newRec.BoxSize.Y > breakRadius)
                        {
                            double quotient = Math.Truncate(newRec.BoxSize.Y / breakRadius);
                            double quotientcc = newRec.BoxSize.Y / quotient;

                            for (int i = 1; i <= quotient; i++)
                            {
                                Line newHorizontal = new Line(minPoint.X, minPoint.Y + breakRadius * i, maxPoint.X, minPoint.Y + breakRadius * i);
                                dimTextLinearPathList.Add(newHorizontal);
                            }
                        }
                        if (newRec.BoxSize.X > breakRadius)
                        {
                            double quotient = Math.Truncate(newRec.BoxSize.X / breakRadius);
                            double quotientcc = newRec.BoxSize.X / quotient;

                            for (int i = 1; i <= quotient; i++)
                            {
                                Line newVertical = new Line(minPoint.X + breakRadius * i, minPoint.Y, minPoint.X + breakRadius * i, maxPoint.Y);
                                dimTextLinearPathList.Add(newVertical);
                            }
                        }


                        dimTextLinearPathList.Add(newRec);
                    }
                    List<Entity> leaderTextLinearPathList = new List<Entity>();
                    foreach (Entity eachEntity in leaderTextList)
                    {
                        eachEntity.Regen(new RegenParams(0, singleModel));
                        Point3D[] textBoxSize = eachEntity.Vertices;
                        LinearPath newRec = new LinearPath(textBoxSize);
                        newRec.Regen(new RegenParams(0, singleModel));

                        Point3D minPoint = GetMinPointXY(textBoxSize);
                        Point3D maxPoint = GetMaxPointXY(textBoxSize);
                        if (newRec.BoxSize.Y > breakRadius)
                        {
                            double quotient = Math.Truncate(newRec.BoxSize.Y / breakRadius);
                            double quotientcc = newRec.BoxSize.Y / quotient;

                            for (int i = 1; i <= quotient; i++)
                            {
                                Line newHorizontal = new Line(minPoint.X, minPoint.Y + breakRadius * i, maxPoint.X, minPoint.Y + breakRadius * i);
                                dimTextLinearPathList.Add(newHorizontal);
                            }
                        }
                        if (newRec.BoxSize.X > breakRadius)
                        {
                            double quotient = Math.Truncate(newRec.BoxSize.X / breakRadius);
                            double quotientcc = newRec.BoxSize.X / quotient;

                            for (int i = 1; i <= quotient; i++)
                            {
                                Line newVertical = new Line(minPoint.X + breakRadius * i, minPoint.Y, minPoint.X + breakRadius * i, maxPoint.Y);
                                dimTextLinearPathList.Add(newVertical);
                            }
                        }

                        leaderTextLinearPathList.Add(newRec);
                    }

                    // All list
                    Dictionary<string, List<Entity>> allEntityDic = new Dictionary<string, List<Entity>>();
                    allEntityDic.Add(CommonGlobal.OutLine, outlineList);
                    allEntityDic.Add(CommonGlobal.CenterLine, centerlineList);
                    allEntityDic.Add(CommonGlobal.DimLine, dimlineList);
                    allEntityDic.Add(CommonGlobal.DimText, dimTextLinearPathList); // text 대신 leader path
                    allEntityDic.Add(CommonGlobal.DimLineExt, dimlineExtList);
                    allEntityDic.Add(CommonGlobal.LeaderLine, leaderlineList);
                    allEntityDic.Add(CommonGlobal.LeaderText, leaderTextLinearPathList); // text 대신 leader path
                    allEntityDic.Add(CommonGlobal.NozzleLine, nozzlelineList);
                    allEntityDic.Add(CommonGlobal.NozzleMark, nozzleMarkList);

                    // Break





                    Dictionary<string, List<Entity>> allEntityNewDic = new Dictionary<string, List<Entity>>();

                    // Line Break
                    List<string> allEntityName = allEntityDic.Keys.ToList();
                    List<List<Entity>> allEntityList = allEntityDic.Values.ToList();
                    for (int i = 0; i < allEntityName.Count; i++)
                    {
                        string eachTarget = allEntityName[i];
                        List<Entity> eachTargetList = allEntityList[i];
                        List<Entity> newEachTargetList = new List<Entity>();

                        // 문자 제외
                        if (!eachTarget.Contains("text"))
                        {
                            foreach (ICurve targetLine in eachTargetList)
                            {
                                // 1. 겹치는 점 찾기
                                Dictionary<Point3D, int> intersectPointDic = new Dictionary<Point3D, int>();
                                for (int j = 0; j < allEntityName.Count; j++)
                                {
                                    string eachReference = allEntityName[j];
                                    // 끊기 여부
                                    if (autoDimService.GetDimensionBreak(eachTarget, eachReference))
                                    {
                                        List<Entity> eachReferenceList = allEntityList[j];

                                        foreach (ICurve refereceLine in eachReferenceList)
                                        {
                                            Point3D[] eachPointArray = targetLine.IntersectWith(refereceLine);
                                            foreach (Point3D eachPoint in eachPointArray)
                                            {
                                                if (!intersectPointDic.ContainsKey(eachPoint))
                                                {
                                                    intersectPointDic.Add(eachPoint, j);
                                                }
                                            }
                                        }
                                    }
                                }

                                // 2. 겹치는 점에서 원 그리고 겹치는 선 기준으로 나누기
                                if (intersectPointDic.Count > 0)
                                {
                                    List<Point3D> splitPointList = new List<Point3D>();
                                    List<Point3D> intersectPointList = intersectPointDic.Keys.ToList();
                                    foreach (Point3D eachPoint in intersectPointList)
                                    {
                                        Circle newCircle = new Circle(eachPoint, breakRadius);// 반지름 : breakRadius
                                        Point3D[] intersectCircleArray = Curve.Intersection(newCircle, targetLine);
                                        splitPointList.AddRange(intersectCircleArray);

                                    }

                                    // 3. 자르기
                                    if (splitPointList.Count > 0)
                                    {
                                        ICurve[] splitLine;
                                        if (targetLine.SplitBy(splitPointList, out splitLine))
                                        {
                                            // 3. 원에 가까운 것 버리기
                                            foreach (ICurve eachICurve in splitLine)
                                            {
                                                bool addLine = true;
                                                Point3D minPoint = Point3D.MidPoint(eachICurve.StartPoint, eachICurve.EndPoint);
                                                foreach (Point3D eachIntersect in intersectPointList)
                                                {
                                                    // 3. 버리는 조건
                                                    if (Point3D.Distance(minPoint, eachIntersect) < breakRadius)
                                                    {
                                                        addLine = false;
                                                        break;
                                                    }
                                                }

                                                // 3. 추가
                                                if (addLine)
                                                {
                                                    newEachTargetList.Add(eachICurve as Entity);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    newEachTargetList.Add(targetLine as Entity);
                                }
                            }
                            allEntityNewDic.Add(eachTarget, newEachTargetList);
                        }
                    }

                    // 문자 추가
                    singleModel.Entities.AddRange(dimTextList, Color.Blue);
                    singleModel.Entities.AddRange(leaderTextList, Color.Green);

                    // Line Break 추가
                    List<string> allEntityNewName = allEntityNewDic.Keys.ToList();
                    List<List<Entity>> allEntityNewList = allEntityNewDic.Values.ToList();
                    for (int i = 0; i < allEntityNewName.Count; i++)
                    {
                        List<Entity> eachEntityList = allEntityNewList[i];
                        string eachEntityName = allEntityNewName[i];
                        switch (eachEntityName)
                        {
                            case "outline":
                                singleModel.Entities.AddRange(eachEntityList, Color.Black);
                                break;
                            case "centerline":
                                singleModel.Entities.AddRange(eachEntityList, Color.Red);
                                break;
                            case "dimline":
                                singleModel.Entities.AddRange(eachEntityList, Color.Blue);
                                break;
                            case "dimtext":
                                singleModel.Entities.AddRange(eachEntityList, Color.Blue);
                                break;
                            case "dimlineext":
                                singleModel.Entities.AddRange(eachEntityList, Color.BlueViolet);
                                break;
                            case "leaderline":
                                singleModel.Entities.AddRange(eachEntityList, Color.Green);
                                break;
                            case "leadertext":
                                singleModel.Entities.AddRange(eachEntityList, Color.Green);
                                break;
                            case "nozzleline":
                                singleModel.Entities.AddRange(eachEntityList, Color.Gray);
                                break;
                            case "nozzlemark":
                                singleModel.Entities.AddRange(eachEntityList, Color.Gray);
                                break;
                        }

                    }

                    //singleModel.Entities.AddRange(dimTextLinearPathList, Color.YellowGreen);


                    //singleModel.Entities.AddRange(outlineList, Color.Black);
                    //singleModel.Entities.AddRange(centerlineList, Color.Red);
                    //singleModel.Entities.AddRange(dimlineList, Color.Blue);
                    //singleModel.Entities.AddRange(dimTextList, Color.Blue);
                    //singleModel.Entities.AddRange(dimlineExtList, Color.BlueViolet);
                    //singleModel.Entities.AddRange(leaderlineList, Color.Green);
                    //singleModel.Entities.AddRange(leaderTextList, Color.Green);
                    //singleModel.Entities.AddRange(nozzlelineList, Color.Gray);
                    //singleModel.Entities.AddRange(nozzleMarkList, Color.Gray);


                    //singleModel.Entities.Add(newCircle1, Color.Red);
                    //singleModel.Entities.Add(newCircle2, Color.Red);

                    //singleModel.Entities.Add( aa1,Color.Blue);
                    //Text cc = new Text(new Point3D(0, -310, 0), "Dimension Line ABCDEFGabcdefg12345", 50);
                    //cc.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
                    //singleModel.Entities.Add( cc, Color.Gray);

                }
                #endregion

                #region Arrow
                if (false)
                {


                    //Text dimTextVertical = new Text(new Point3D(20, 40), "asdf", 20);
                    //dimTextVertical.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
                    Text dimTextVertical2 = new Text(new Point3D(40, -20), "asdf", 20);
                    dimTextVertical2.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);





                    double breakRadius1 = 10;
                    //Triangle newTri=new Triangle(new Point3D(20,20), new Point3D(40,40), new Point3D(0,40));
                    Triangle newTri = new Triangle(new Point3D(20, 20), new Point3D(40, 0), new Point3D(40, 40));
                    newTri.ColorMethod = colorMethodType.byEntity;
                    newTri.Color = Color.Blue;

                    //singleModel.Entities.Add(newTri);
                    //newTri.Regen(new RegenParams(0, singleModel));

                    //Point3D[] ss = new Point3D[3] { new Point3D(20, 20), new Point3D(40, 0), new Point3D(40, 40) };

                    //newTri.Regen(new RegenParams(0, singleModel));
                    List<Point3D> newTriPath = new List<Point3D>();
                    newTriPath.AddRange(newTri.Vertices);

                    Point3D centerPoint = new Point3D();

                    if (newTriPath[1].X == newTriPath[2].X)
                    {
                        // Left Right
                        double newDistance = Point3D.Distance(newTriPath[1], newTriPath[2]);
                        double minValue = Math.Min(newTriPath[1].Y, newTriPath[2].Y);
                        double quotient = Math.Truncate(newDistance / breakRadius1);

                        centerPoint.Y = newTriPath[0].Y;
                        if (newTriPath[0].X < newTriPath[1].X)
                        {
                            centerPoint.X = newTriPath[0].X + 1;
                        }
                        else
                        {
                            centerPoint.X = newTriPath[0].X - 1;
                        }

                        for (int i = 1; i <= quotient; i++)
                        {
                            Line newLine = new Line(centerPoint, new Point3D(newTriPath[1].X, minValue + breakRadius1 * i));
                            singleModel.Entities.Add(newLine);
                        }
                    }
                    else if (newTriPath[1].Y == newTriPath[2].Y)
                    {
                        // Top Bottom
                        double newDistance = Point3D.Distance(newTriPath[1], newTriPath[2]);
                        double minValue = Math.Min(newTriPath[1].X, newTriPath[2].X);
                        double quotient = Math.Truncate(newDistance / breakRadius1);

                        centerPoint.X = newTriPath[0].X;
                        if (newTriPath[0].Y < newTriPath[1].Y)
                        {
                            centerPoint.Y = newTriPath[0].Y + 1;
                        }
                        else
                        {
                            centerPoint.Y = newTriPath[0].Y - 1;
                        }

                        for (int i = 1; i <= quotient; i++)
                        {
                            Line newLine = new Line(centerPoint, new Point3D(minValue + breakRadius1 * i, newTriPath[1].Y));
                            singleModel.Entities.Add(newLine);
                        }
                    }

                    newTriPath = new List<Point3D>();
                    newTriPath.Add(centerPoint);
                    newTriPath.Add(newTri.Vertices[1]);
                    newTriPath.Add(newTri.Vertices[2]);
                    newTriPath.Add(centerPoint);
                    LinearPath newLinearPath = new LinearPath(newTriPath.ToArray());
                    singleModel.Entities.Add(newLinearPath);

                    singleModel.Entities.Add(newLinearPath);
                }
                #endregion
            }
            else
            {

                #region Dimension Sample
                if (false)
                {
                    singleModel.Entities.AddRange(GetDimension(new Point3D(10, 10), new Point3D(100, 50), "TOP", 100));
                    singleModel.Entities.AddRange(GetDimension(new Point3D(10, 10), new Point3D(100, 50), "BOTTOM", 100));
                    singleModel.Entities.AddRange(GetDimension(new Point3D(-10, 10), new Point3D(-50, 100), "LEFT", 100));
                    singleModel.Entities.AddRange(GetDimension(new Point3D(-10, 10), new Point3D(-50, 100), "RIGHT", 100));

                    singleModel.Entities.Add(new LinearPath(new Point3D[5] { new Point3D(10, 10), new Point3D(-10, 10), new Point3D(-10, -10), new Point3D(10, -10), new Point3D(10, 10) }));
                }
                #endregion



                #region Line Break : ing
                if (false)
                {


                    AutomationDimensionService autoDimService = new AutomationDimensionService();

                    // Reference Circle
                    double breakRadius = 10;

                    List<Entity> outlineList = new List<Entity>();
                    List<Entity> centerlineList = new List<Entity>();
                    List<Entity> dimlineList = new List<Entity>();
                    List<Entity> dimTextList = new List<Entity>();
                    List<Entity> dimlineExtList = new List<Entity>();
                    List<Entity> leaderlineList = new List<Entity>();
                    List<Entity> leaderTextList = new List<Entity>();
                    List<Entity> nozzlelineList = new List<Entity>();
                    List<Entity> nozzleMarkList = new List<Entity>();

                    // 세로
                    outlineList.Add(new Line(new Point3D(10, -20, 0), new Point3D(10, 1110, 0)));
                    centerlineList.Add(new Line(new Point3D(110, -20, 0), new Point3D(110, 1110, 0)));
                    dimlineList.Add(new Line(new Point3D(210, -20, 0), new Point3D(210, 1110, 0)));
                    Text dimText01 = new Text(new Point3D(0, -310, 0), "Dimension Line ABCDEFGabcdefg12345", 50);

                    dimText01.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
                    dimTextList.Add(dimText01);
                    dimlineExtList.Add(new Line(new Point3D(410, -20, 0), new Point3D(410, 1110, 0)));
                    leaderlineList.Add(new Line(new Point3D(510, -20, 0), new Point3D(510, 1110, 0)));
                    Text leaderText01 = new Text(new Point3D(0, -610, 0), "Leader Line ABCDEFGabcdefg12345", 50);
                    leaderText01.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
                    leaderTextList.Add(leaderText01);
                    nozzlelineList.Add(new Line(new Point3D(710, -20, 0), new Point3D(710, 1110, 0)));
                    nozzleMarkList.Add(new Line(new Point3D(810, -20, 0), new Point3D(810, 1110, 0)));

                    // 가로
                    outlineList.Add(new Line(new Point3D(-20, 10, 0), new Point3D(1110, 10, 0)));
                    centerlineList.Add(new Line(new Point3D(-20, 110, 0), new Point3D(1110, 110, 0)));
                    dimlineList.Add(new Line(new Point3D(-20, 210, 0), new Point3D(1110, 210, 0)));
                    Text dimText02 = new Text(new Point3D(-20, 310, 0), "Dimension Line ABCDEFGabcdefg12345", 50);
                    dimTextList.Add(dimText02);
                    dimlineExtList.Add(new Line(new Point3D(-20, 410, 0), new Point3D(1110, 410, 0)));
                    leaderlineList.Add(new Line(new Point3D(-20, 510, 0), new Point3D(1110, 510, 0)));
                    Text leaderText02 = new Text(new Point3D(-20, 610, 0), "Leader Line ABCDEFGabcdefg12345", 50);
                    leaderTextList.Add(leaderText02);
                    nozzlelineList.Add(new Line(new Point3D(-20, 710, 0), new Point3D(1110, 710, 0)));
                    nozzleMarkList.Add(new Line(new Point3D(-20, 810, 0), new Point3D(1110, 810, 0)));


                    // Text to Box : 수정 최소단위 사각형으로
                    List<Entity> dimTextLinearPathList = new List<Entity>();
                    foreach (Entity eachEntity in dimTextList)
                    {
                        eachEntity.Regen(new RegenParams(0, singleModel));
                        Point3D[] textBoxSize = eachEntity.Vertices;
                        LinearPath newRec = new LinearPath(textBoxSize);
                        newRec.Regen(new RegenParams(0, singleModel));

                        Point3D minPoint = GetMinPointXY(textBoxSize);
                        Point3D maxPoint = GetMaxPointXY(textBoxSize);
                        if (newRec.BoxSize.Y > breakRadius)
                        {
                            double quotient = Math.Truncate(newRec.BoxSize.Y / breakRadius);
                            double quotientcc = newRec.BoxSize.Y / quotient;

                            for (int i = 1; i <= quotient; i++)
                            {
                                Line newHorizontal = new Line(minPoint.X, minPoint.Y + breakRadius * i, maxPoint.X, minPoint.Y + breakRadius * i);
                                dimTextLinearPathList.Add(newHorizontal);
                            }
                        }
                        if (newRec.BoxSize.X > breakRadius)
                        {
                            double quotient = Math.Truncate(newRec.BoxSize.X / breakRadius);
                            double quotientcc = newRec.BoxSize.X / quotient;

                            for (int i = 1; i <= quotient; i++)
                            {
                                Line newVertical = new Line(minPoint.X + breakRadius * i, minPoint.Y, minPoint.X + breakRadius * i, maxPoint.Y);
                                dimTextLinearPathList.Add(newVertical);
                            }
                        }


                        dimTextLinearPathList.Add(newRec);
                    }
                    List<Entity> leaderTextLinearPathList = new List<Entity>();
                    foreach (Entity eachEntity in leaderTextList)
                    {
                        eachEntity.Regen(new RegenParams(0, singleModel));
                        Point3D[] textBoxSize = eachEntity.Vertices;
                        LinearPath newRec = new LinearPath(textBoxSize);
                        newRec.Regen(new RegenParams(0, singleModel));

                        Point3D minPoint = GetMinPointXY(textBoxSize);
                        Point3D maxPoint = GetMaxPointXY(textBoxSize);
                        if (newRec.BoxSize.Y > breakRadius)
                        {
                            double quotient = Math.Truncate(newRec.BoxSize.Y / breakRadius);
                            double quotientcc = newRec.BoxSize.Y / quotient;

                            for (int i = 1; i <= quotient; i++)
                            {
                                Line newHorizontal = new Line(minPoint.X, minPoint.Y + breakRadius * i, maxPoint.X, minPoint.Y + breakRadius * i);
                                dimTextLinearPathList.Add(newHorizontal);
                            }
                        }
                        if (newRec.BoxSize.X > breakRadius)
                        {
                            double quotient = Math.Truncate(newRec.BoxSize.X / breakRadius);
                            double quotientcc = newRec.BoxSize.X / quotient;

                            for (int i = 1; i <= quotient; i++)
                            {
                                Line newVertical = new Line(minPoint.X + breakRadius * i, minPoint.Y, minPoint.X + breakRadius * i, maxPoint.Y);
                                dimTextLinearPathList.Add(newVertical);
                            }
                        }

                        leaderTextLinearPathList.Add(newRec);
                    }

                    // All list
                    Dictionary<string, List<Entity>> allEntityDic = new Dictionary<string, List<Entity>>();
                    allEntityDic.Add(CommonGlobal.OutLine, outlineList);
                    allEntityDic.Add(CommonGlobal.CenterLine, centerlineList);
                    allEntityDic.Add(CommonGlobal.DimLine, dimlineList);
                    allEntityDic.Add(CommonGlobal.DimText, dimTextLinearPathList); // text 대신 leader path
                    allEntityDic.Add(CommonGlobal.DimLineExt, dimlineExtList);
                    allEntityDic.Add(CommonGlobal.LeaderLine, leaderlineList);
                    allEntityDic.Add(CommonGlobal.LeaderText, leaderTextLinearPathList); // text 대신 leader path
                    allEntityDic.Add(CommonGlobal.NozzleLine, nozzlelineList);
                    allEntityDic.Add(CommonGlobal.NozzleMark, nozzleMarkList);

                    // Break





                    Dictionary<string, List<Entity>> allEntityNewDic = new Dictionary<string, List<Entity>>();

                    // Line Break
                    List<string> allEntityName = allEntityDic.Keys.ToList();
                    List<List<Entity>> allEntityList = allEntityDic.Values.ToList();
                    for (int i = 0; i < allEntityName.Count; i++)
                    {
                        string eachTarget = allEntityName[i];
                        List<Entity> eachTargetList = allEntityList[i];
                        List<Entity> newEachTargetList = new List<Entity>();

                        // 문자 제외
                        if (!eachTarget.Contains("text"))
                        {
                            foreach (ICurve targetLine in eachTargetList)
                            {
                                // 1. 겹치는 점 찾기
                                Dictionary<Point3D, int> intersectPointDic = new Dictionary<Point3D, int>();
                                for (int j = 0; j < allEntityName.Count; j++)
                                {
                                    string eachReference = allEntityName[j];
                                    // 끊기 여부
                                    if (autoDimService.GetDimensionBreak(eachTarget, eachReference))
                                    {
                                        List<Entity> eachReferenceList = allEntityList[j];

                                        foreach (ICurve refereceLine in eachReferenceList)
                                        {
                                            Point3D[] eachPointArray = targetLine.IntersectWith(refereceLine);
                                            foreach (Point3D eachPoint in eachPointArray)
                                            {
                                                if (!intersectPointDic.ContainsKey(eachPoint))
                                                {
                                                    intersectPointDic.Add(eachPoint, j);
                                                }
                                            }
                                        }
                                    }
                                }

                                // 2. 겹치는 점에서 원 그리고 겹치는 선 기준으로 나누기
                                if (intersectPointDic.Count > 0)
                                {
                                    List<Point3D> splitPointList = new List<Point3D>();
                                    List<Point3D> intersectPointList = intersectPointDic.Keys.ToList();
                                    foreach (Point3D eachPoint in intersectPointList)
                                    {
                                        Circle newCircle = new Circle(eachPoint, breakRadius);// 반지름 : breakRadius
                                        Point3D[] intersectCircleArray = Curve.Intersection(newCircle, targetLine);
                                        splitPointList.AddRange(intersectCircleArray);

                                    }

                                    // 3. 자르기
                                    if (splitPointList.Count > 0)
                                    {
                                        ICurve[] splitLine;
                                        if (targetLine.SplitBy(splitPointList, out splitLine))
                                        {
                                            // 3. 원에 가까운 것 버리기
                                            foreach (ICurve eachICurve in splitLine)
                                            {
                                                bool addLine = true;
                                                Point3D minPoint = Point3D.MidPoint(eachICurve.StartPoint, eachICurve.EndPoint);
                                                foreach (Point3D eachIntersect in intersectPointList)
                                                {
                                                    // 3. 버리는 조건
                                                    if (Point3D.Distance(minPoint, eachIntersect) < breakRadius)
                                                    {
                                                        addLine = false;
                                                        break;
                                                    }
                                                }

                                                // 3. 추가
                                                if (addLine)
                                                {
                                                    newEachTargetList.Add(eachICurve as Entity);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    newEachTargetList.Add(targetLine as Entity);
                                }
                            }
                            allEntityNewDic.Add(eachTarget, newEachTargetList);
                        }
                    }

                    // 문자 추가
                    singleModel.Entities.AddRange(dimTextList, Color.Blue);
                    singleModel.Entities.AddRange(leaderTextList, Color.Green);

                    // Line Break 추가
                    List<string> allEntityNewName = allEntityNewDic.Keys.ToList();
                    List<List<Entity>> allEntityNewList = allEntityNewDic.Values.ToList();
                    for (int i = 0; i < allEntityNewName.Count; i++)
                    {
                        List<Entity> eachEntityList = allEntityNewList[i];
                        string eachEntityName = allEntityNewName[i];
                        switch (eachEntityName)
                        {
                            case "outline":
                                singleModel.Entities.AddRange(eachEntityList, Color.Black);
                                break;
                            case "centerline":
                                singleModel.Entities.AddRange(eachEntityList, Color.Red);
                                break;
                            case "dimline":
                                singleModel.Entities.AddRange(eachEntityList, Color.Blue);
                                break;
                            case "dimtext":
                                singleModel.Entities.AddRange(eachEntityList, Color.Blue);
                                break;
                            case "dimlineext":
                                singleModel.Entities.AddRange(eachEntityList, Color.BlueViolet);
                                break;
                            case "leaderline":
                                singleModel.Entities.AddRange(eachEntityList, Color.Green);
                                break;
                            case "leadertext":
                                singleModel.Entities.AddRange(eachEntityList, Color.Green);
                                break;
                            case "nozzleline":
                                singleModel.Entities.AddRange(eachEntityList, Color.Gray);
                                break;
                            case "nozzlemark":
                                singleModel.Entities.AddRange(eachEntityList, Color.Gray);
                                break;
                        }

                    }

                    //singleModel.Entities.AddRange(dimTextLinearPathList, Color.YellowGreen);


                    //singleModel.Entities.AddRange(outlineList, Color.Black);
                    //singleModel.Entities.AddRange(centerlineList, Color.Red);
                    //singleModel.Entities.AddRange(dimlineList, Color.Blue);
                    //singleModel.Entities.AddRange(dimTextList, Color.Blue);
                    //singleModel.Entities.AddRange(dimlineExtList, Color.BlueViolet);
                    //singleModel.Entities.AddRange(leaderlineList, Color.Green);
                    //singleModel.Entities.AddRange(leaderTextList, Color.Green);
                    //singleModel.Entities.AddRange(nozzlelineList, Color.Gray);
                    //singleModel.Entities.AddRange(nozzleMarkList, Color.Gray);


                    //singleModel.Entities.Add(newCircle1, Color.Red);
                    //singleModel.Entities.Add(newCircle2, Color.Red);

                    //singleModel.Entities.Add( aa1,Color.Blue);
                    //Text cc = new Text(new Point3D(0, -310, 0), "Dimension Line ABCDEFGabcdefg12345", 50);
                    //cc.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
                    //singleModel.Entities.Add( cc, Color.Gray);

                }
                #endregion


                #region Leader
                if (false)
                {
                    List<string> leaderText = new List<string>();
                    leaderText.Add("B1AAAAaaaaaM");
                    leaderText.Add("B2AAAAaaasdfaaaM");
                    leaderText.Add("B3AAAAaaaasdfaaM");
                    leaderText.Add("B4AAAAaaM");


                    List<Entity> ccc = GetLeader(new Point3D(1600, 10), 60, "topright", leaderText, leaderText, singleModel);
                    singleModel.Entities.AddRange(ccc);

                    List<Entity> cccc = GetLeader(new Point3D(1600, 10), 60, "bottomright", leaderText, leaderText, singleModel);
                    singleModel.Entities.AddRange(cccc);

                    List<Entity> ccccc = GetLeader(new Point3D(1600, 10), 60, "topleft", leaderText, leaderText, singleModel);
                    singleModel.Entities.AddRange(ccccc);

                    List<Entity> cccccc = GetLeader(new Point3D(1600, 10), 60, "bottomleft", leaderText, leaderText, singleModel);
                    singleModel.Entities.AddRange(cccccc);


                }

                #endregion

                #region offset
                if (false)
                {
                    Line newLine = new Line(new Point3D(0, 0), new Point3D(30, 30));
                    
                    Line customLine = (Line)newLine.Clone();
                    Plane pl1 = Plane.YZ;
                    pl1.Origin.X = customLine.EndPoint.X;
                    pl1.Origin.Y = customLine.EndPoint.Y;
                    Mirror customMirror = new Mirror(pl1);
                    customLine.TransformBy(customMirror);

                    Line newLine2 = new Line(new Point3D(40, 0), new Point3D(40, 30));
                    newLine2.Rotate(UtilityEx.DegToRad(80), Vector3D.AxisZ, newLine2.StartPoint);
                    Line newLine3 = (Line)newLine2.Clone();
                    newLine3.Rotate(-UtilityEx.DegToRad(80), Vector3D.AxisZ, newLine2.StartPoint);
                    Line customLine2 = (Line)newLine2.Clone();
                    Plane pl2 = Plane.YZ;
                    pl2.Origin.X = customLine2.StartPoint.X;
                    pl2.Origin.Y = customLine2.StartPoint.Y;
                    Mirror customMirror2 = new Mirror(pl2);
                    customLine2.TransformBy(customMirror2);

                    singleModel.Entities.Add(customLine, Color.Black);
                    singleModel.Entities.Add(newLine, Color.Red);

                    singleModel.Entities.Add(customLine2, Color.Blue);
                    singleModel.Entities.Add(newLine2, Color.Green);
                    singleModel.Entities.Add(newLine3, Color.Gray);



                }
                #endregion

                #region circle
                if (true)
                {
                    Circle newCir1 = new Circle(new Point3D(40, 40), 45);
                    singleModel.Entities.Add(newCir1, Color.Green);

                    Circle newCir2 = new Circle(new Point3D(40, 40), 23);
                    singleModel.Entities.Add(newCir2, Color.Green);

                    Point3D newCenter = new Point3D();
                    newCenter.X = 40;
                    newCenter.Y = 40;
                    double boltHeight = 23;
                    double boltWidth = 45;

                    Point3D newCenterLeft = new Point3D();
                    newCenterLeft.X = newCenter.X - boltWidth + boltHeight;
                    newCenterLeft.Y = newCenter.Y;
                    Point3D newCenterRight = new Point3D();
                    newCenterRight.X = newCenter.X + boltWidth - boltHeight;
                    newCenterRight.Y = newCenter.Y;

                    Arc arcarc1 = new Arc(Plane.XY, newCenterLeft, boltHeight, new Point3D(newCenterLeft.X, newCenterLeft.Y+ boltHeight), new Point3D(newCenterLeft.X,newCenterLeft.Y-boltHeight),false);
                    Arc arcarc2 = new Arc(Plane.XY, newCenterRight, boltHeight, new Point3D(newCenterRight.X, newCenterRight.Y + boltHeight), new Point3D(newCenterRight.X, newCenterRight.Y - boltHeight), true);
                    Line ll1 = new Line(arcarc1.StartPoint, arcarc2.EndPoint);
                    Line ll2 = new Line(arcarc1.EndPoint, arcarc2.StartPoint);

                    CompositeCurve compositeCurve = new CompositeCurve(arcarc1, arcarc2, ll1, ll2);
                    singleModel.Entities.Add(compositeCurve, Color.Gray);

                    CompositeCurve compositeCurve2 = (CompositeCurve)compositeCurve.Clone();
                    compositeCurve2.Rotate(Math.Atan2(1,120), Vector3D.AxisZ);
                    singleModel.Entities.Add(compositeCurve2, Color.Red);



                }
                #endregion

                singleModel.Entities.Regen();
                singleModel.ZoomFit();
                singleModel.SetView(viewType.Top);
            }


        }

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
