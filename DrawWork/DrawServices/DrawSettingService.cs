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
                if (true)
                {


                    //six composite curve
                    Arc arc = new Arc(0, 0, 0, 113, (Math.PI / 3), (Math.PI * 2 / 3));
                    singleModel.Entities.Add(new Arc(arc.Center, 113, Utility.DegToRad(45), Utility.DegToRad(70)));

                    Line line1 = new Line(-45, 113, -45, 0);
                    //Line line2 = new Line(55, 49, 55, 113); //error
                    Line line2 = new Line(-40, 49, 15, 49);
                    line2.Rotate(Utility.DegToRad(30), Vector3D.AxisZ, new Point3D(-45, 49));
                    singleModel.Entities.Add((Line)line2.Clone(),Color.Blue);

                    singleModel.Entities.Add((Line)line1.Clone(),Color.Green);

                    Arc arcFillet1, arcFillet2, arcFillet3;
                    Curve.Fillet(arc, line1, 9.5, false, false, true, true, out arcFillet1);
                    Curve.Fillet(arc, line2, 9.5, false, false, true, true, out arcFillet2);
                    Curve.Fillet(line1, line2, 9.5, false, false, true, true, out arcFillet3);

                    singleModel.Entities.Add(arcFillet3,Color.Red);
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
            }
            else
            {
                // Current Test
                double flangeFaceWidth = 10;
                double flangeFaceHeight = 80;
                double flangeFaceInnerWidth = 10;
                double flangePipeWidth = 24;
                double flangePipeHeight = flangeFaceHeight-(flangeFaceInnerWidth * 2);
                double flangePipeInnerWidth = 18;
                double pipeHeight = flangeFaceHeight - (flangePipeInnerWidth * 2);
                double pipeWidth = 40;
                double fullWidth = flangeFaceWidth + flangePipeWidth + pipeWidth;

                Point3D drawPoint = new Point3D(0, 0, 0);

                Line lineFFa = new Line(new Point3D(0, 0, 0), new Point3D(0, flangeFaceHeight, 0));
                Line lineFFb = new Line(new Point3D(flangeFaceWidth, 0, 0), new Point3D(flangeFaceWidth, flangeFaceHeight, 0));
                Line lineFFc = new Line(new Point3D(0, 0, 0), new Point3D(flangeFaceWidth, 0, 0));
                Line lineFFd = new Line(new Point3D(0, flangeFaceHeight, 0), new Point3D(flangeFaceWidth, flangeFaceHeight, 0));

                Line lineFPa = new Line(new Point3D(flangeFaceWidth, flangeFaceInnerWidth + flangePipeHeight, 0), new Point3D(flangeFaceWidth+ flangePipeWidth, flangePipeInnerWidth+pipeHeight, 0));
                Line lineFPb = new Line(new Point3D(flangeFaceWidth, flangeFaceInnerWidth , 0), new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0));
                Line lineFPc = new Line(new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth + pipeHeight, 0), new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0));

                Line linePa = new Line(new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0), new Point3D(flangeFaceWidth + flangePipeWidth + pipeWidth, flangePipeInnerWidth, 0));
                Line linePb = new Line(new Point3D(flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth + pipeHeight, 0), new Point3D(flangeFaceWidth + flangePipeWidth + pipeWidth, flangePipeInnerWidth + pipeHeight, 0));

                singleModel.Entities.AddRange(new Entity[]{ lineFFa,lineFFb,lineFFc, lineFFd, lineFPa, lineFPb,lineFPc, linePa,linePb});

            }

            singleModel.Entities.Regen();
            singleModel.ZoomFit();


        }
    }
}
