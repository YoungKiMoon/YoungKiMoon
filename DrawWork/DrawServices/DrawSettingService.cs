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
using System.Collections.ObjectModel;
using DrawWork.DrawModels;

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

            singleModel.LineTypes.Add(LayerDashDot, new float[] { 5, -1, 1, -1 });
            singleModel.Layers[LayerDashDot].LineTypeName = LayerDashDot;
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

            }
            else
            {
                Point3D leaderPoint = new Point3D(0, 0, 0);
                double leaderLength = 15;
                double leaderTriShort = leaderLength / 2;
                double leaderTriLong = Math.Tan(UtilityEx.DegToRad(60)) * leaderTriShort;
                double leaderCenterX = leaderPoint.X+leaderTriShort;
                double leaderCenterY = leaderPoint.Y + leaderTriLong;

                List<Point3D> leaderPoints = new List<Point3D>();
                leaderPoints.Add(leaderPoint);
                leaderPoints.Add(new Point3D(leaderCenterX,leaderCenterY, 0));
                leaderPoints.Add(new Point3D(leaderCenterX+40, leaderCenterY, 0));
                Leader leader1 = new Leader(Plane.XY, leaderPoints);
                singleModel.Entities.Add(leader1, Color.Red);

                //leader1.Scale = 30;
                //leader1.LineTypeScale = 30;
                Text newText = new Text(leaderCenterX+1, leaderCenterY, 0, "LAaBbCc", 2.5);
                newText.Alignment = Text.alignmentType.BaselineLeft;
                //singleModel.FontFamily.FamilyNames= "ROMANS";

                Text newText2 = new Text(leaderCenterX + 1, leaderCenterY+1, 0, "LAaBbCc", 2.5);
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

                textCenter.X = selPoint1.X + (selPoint2.X - selPoint1.X)/2;
                textCenter.Y = Math.Max(selPoint1.Y, selPoint2.Y) + selDimHeight;
                newDimTop = new LinearDim(Plane.XY,new Point3D(selPoint1.X, selPoint1.Y),
                                        new Point3D(selPoint2.X, selPoint2.Y),
                                        new Point3D(textCenter.X, textCenter.Y), selTextHeight);

                newDimTop.ArrowsLocation = elementPositionType.Inside;

                // Text Gap
                newDimTop.TextGap = 1;
                newDimTop.ExtLineOffset = 1;
                newDimTop.ExtLineExt = 1;
                newDimTop.ArrowheadSize = 2.5;
                newDimTop.ScaleOverall = 1;
                newDimTop.LinearScale = 1;
                

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

                singleModel.Entities.Add(newDimTop, Color.Red);
                singleModel.Entities.Add(newDimBottom, Color.Gray);

                singleModel.Entities.Add(newDimRight, Color.Blue);
                singleModel.Entities.Add(newDimLeft, Color.Green);




                singleModel.Entities.Regen();
                singleModel.ZoomFit();
            }

        }
    }
}
