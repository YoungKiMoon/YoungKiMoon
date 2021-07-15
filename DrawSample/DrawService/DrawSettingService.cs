using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using devDept.Eyeshot.Translators;
using devDept.Eyeshot;
using devDept.Graphics;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawSample.Commons;
using System.Windows;
using DrawSample.Models;

using MColor = System.Windows.Media.Color;

using Color = System.Drawing.Color;

namespace DrawSample.DrawService
{
    public class DrawSettingService
    {

        private ValueService valueService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;
        private DrawEditingService editingService;
        private DrawShapeServices shapeService;



        public List<Entity> newLineList = new List<Entity>();
        public List<Entity> cirList = new List<Entity>();
        public List<Point3D[]> interOutlineList = new List<Point3D[]>();
        public List<Point3D[]> interShapeList = new List<Point3D[]>();
        public List<Entity> markList = new List<Entity>();

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


            interOutlineList.Clear();
            interShapeList.Clear();

            cirList.Clear();
            newLineList.Clear();

            singleModel.Entities.Clear();


            if (true)
            {


                Point3D refPoint = new Point3D(1000, 1000);

                // OutLine
                double boxOneWidth = 200;
                double boxOneHeight = 80;
                double boxWidth = boxOneWidth*4;
                double boxHeight = boxOneHeight*6;

                Line boxLeft=new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, 0, boxHeight));
                Line boxRight = new Line(GetSumPoint(refPoint, boxWidth, 0), GetSumPoint(refPoint, boxWidth, boxHeight));
                Line boxTop = new Line(GetSumPoint(refPoint, 0, boxHeight), GetSumPoint(refPoint, boxWidth, boxHeight));
                Line boxBottom = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, boxWidth, 0));

                newLineList.AddRange(new Line[] { boxLeft, boxRight, boxTop, boxBottom });

                double currentX = boxOneWidth;
                for(int i=1; i < 5; i++)
                {
                    Line vertiLine = new Line(GetSumPoint(refPoint, currentX, 0), GetSumPoint(refPoint, currentX, boxHeight));
                    currentX += boxOneWidth;
                    newLineList.Add(vertiLine);
                }


                List<Tuple<double, double>> newVertiList = new List<Tuple<double, double>>();
                newVertiList.Add(new Tuple<double, double>(0, 80*4));
                newVertiList.Add(new Tuple<double, double>(200, 80*2));
                newVertiList.Add(new Tuple<double, double>(400, 80*3));
                newVertiList.Add(new Tuple<double, double>(600, 80*2));
                foreach(Tuple<double,double> eachData in newVertiList)
                {
                    Line vertiLine = new Line(GetSumPoint(refPoint, eachData.Item1, eachData.Item2), GetSumPoint(refPoint, eachData.Item1 +boxOneWidth, eachData.Item2));
                    newLineList.Add(vertiLine);
                }

                styleService.SetLayerListEntity(ref newLineList, layerService.LayerBasicLine);


                // 
                double circleRadius = 30;
                List<Tuple<double, double>> shapeCirList = new List<Tuple<double, double>>();
                shapeCirList.Add(new Tuple<double, double>(100, 80 * 3.5));
                shapeCirList.Add(new Tuple<double, double>(100, 80 * 1.6));
                shapeCirList.Add(new Tuple<double, double>(280, 80 * 4.3));
                shapeCirList.Add(new Tuple<double, double>(320, 80 * 2.5));
                shapeCirList.Add(new Tuple<double, double>(460, 80 * 2.6));
                shapeCirList.Add(new Tuple<double, double>(480, 80 * 2.4));
                shapeCirList.Add(new Tuple<double, double>(700, 80 * 2.2));
                foreach (Tuple<double, double> eachData in shapeCirList)
                {
                    Circle eachCircle = new Circle(GetSumPoint(refPoint, eachData.Item1, eachData.Item2), circleRadius);
                    cirList.Add(eachCircle);
                }

                styleService.SetLayerListEntity(ref cirList, layerService.LayerOutLine);



                singleModel.Entities.AddRange(cirList);
                singleModel.Entities.AddRange(newLineList);
            }

            singleModel.Entities.Regen();
            singleModel.SetView(viewType.Top);
            singleModel.ZoomFit();
            singleModel.Refresh();



        }



        public void ExecuteDetection(out double totalValue,out double outLineValue,out double shapeValue)
        {
            totalValue = 0;
            outLineValue = 0;
            shapeValue = 0;

            interOutlineList.Clear();
            interShapeList.Clear();

            foreach (Line eachOut in newLineList)
            {
                foreach (Entity eachCircle in cirList)
                {
                    Point3D[] newInter = eachOut.IntersectWith((ICurve)eachCircle);
                    if (newInter.Length > 0)
                    {
                        interOutlineList.Add(newInter);
                    }
                }
            }
            outLineValue = interOutlineList.Count;

            foreach (Entity eachCircle in cirList)
            {
                foreach (Circle eachCircle2 in cirList)
                {
                    Point3D[] newInter = eachCircle2.IntersectWith((ICurve)eachCircle);
                    if (newInter.Length > 0)
                    {
                        interShapeList.Add(newInter);
                    }
                }
            }
            shapeValue = interShapeList.Count;
            if (shapeValue > 0)
            {
                shapeValue = shapeValue / 2;
            }

            totalValue = outLineValue + shapeValue;
        }

        public void ExecuteDetection2(out double totalValue, out double outLineValue, out double shapeValue,Model testModel)
        {
            totalValue = 0;
            outLineValue = 0;
            shapeValue = 0;

            CreateMarkValue(false, testModel);

            interOutlineList.Clear();
            interShapeList.Clear();


            foreach (Entity eachOut in testModel.Entities)
            {
                if(eachOut is Circle)
                {
                    foreach (Entity eachOut2 in testModel.Entities)
                    {
                        Point3D[] eachInter = ((ICurve)eachOut).IntersectWith((ICurve)eachOut2);
                        if (eachInter.Length > 0)
                        {
                            foreach(Point3D eachPoint in eachInter)
                                interOutlineList.Add(eachInter);
                        }
                    }
                }
            }

            outLineValue = interOutlineList.Count;
            if (outLineValue > 0)
            {
                outLineValue = outLineValue / 2;
            }

            totalValue = outLineValue ;
        }


        public void CreateMarkValue(bool checkMark, Model singleModel)
        {

            if (checkMark)
            {
                double markWidth = 50;
                double markHeight = 50;
                List<Point3D> outList = new List<Point3D>();
                foreach(Point3D[] eachPoint in interOutlineList)
                {
                    foreach(Point3D eachEachPoint in eachPoint)
                    {
                        List<Entity> eachRec = shapeService.GetRectangle(out outList, GetSumPoint(eachEachPoint, -markWidth / 2, markHeight / 2), markWidth, markHeight, 0, 0, 0);
                        markList.AddRange(eachRec);
                    }
                }
                foreach (Point3D[] eachPoint in interShapeList)
                {
                    List<Entity> eachRec = shapeService.GetRectangle(out outList, GetSumPoint(eachPoint[0], -markWidth / 2, markHeight / 2), markWidth, markHeight, 0, 0, 0);
                    markList.AddRange(eachRec);
                }
                styleService.SetLayerListEntity(ref markList, layerService.LayerCenterLine);

                singleModel.Entities.AddRange(markList);


                singleModel.Entities.Regen();


                singleModel.SetView(viewType.Top);
                singleModel.ZoomFit();
                singleModel.Refresh();

            }
            else
            {
                foreach (Entity eachEntity in markList)
                    singleModel.Entities.Remove(eachEntity);


                markList.Clear();

                singleModel.Entities.Regen();


                singleModel.SetView(viewType.Top);
                singleModel.ZoomFit();
                singleModel.Refresh();
            }
        }




        public void CreateArrangePlate(Model singleModel,string circleDiameter="90000")
        {

            singleModel.Entities.Clear();

            List<Entity> newList = new List<Entity>();
            List<Entity> centerLineList = new List<Entity>();
            List<Entity> plateList = new List<Entity>();
            List<Line> horizontalLineList = new List<Line>();
            List<Tuple<Point3D,Point3D>> horizontalPointList = new List<Tuple<Point3D, Point3D>>();
            List<Line> horizontalVList = new List<Line>();

            List<CombModel> horizontalCombList = new List<CombModel>();

            List<Entity> textList = new List<Entity>();

            Point3D referencePoint = new Point3D(10000000, 10000000);

            if (true)
            {
                double inputDiameter = valueService.GetDoubleValue(circleDiameter);
                double outCircleDiameter = inputDiameter;
                double outCircleRadius = outCircleDiameter/2;

                double plateOverlap = 30;
                double plateOverlapHalf = plateOverlap / 2;

                double plateActualWidth = 2438;
                double plateActualLength = 9144;

                double plateWidth = plateActualWidth-plateOverlapHalf;
                double plateLength = plateActualLength-plateOverlapHalf;

                double horizontalMinSpacing = 800;
                double verticalMinSpacing = 800;
                double horizontalSpacingShift = horizontalMinSpacing+50;

                double intersectWidth = 300;
                double intersectWidthHalf = intersectWidth / 2;

                // IntersectLength
                double intersectLength = 100;


                // Outer : Circle
                Circle outCircle = new Circle(GetSumPoint(referencePoint, 0, 0),outCircleRadius);
                styleService.SetLayer(ref outCircle, layerService.LayerOutLine);
                newList.Add(outCircle);

                // Center Line
                centerLineList.AddRange(editingService.GetCenterLine(GetSumPoint(referencePoint, 0, -outCircleRadius), GetSumPoint(referencePoint, 0, outCircleRadius), 20, 90));
                centerLineList.AddRange(editingService.GetCenterLine(GetSumPoint(referencePoint, -outCircleRadius, 0), GetSumPoint(referencePoint, outCircleRadius,0), 20, 90));
                styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);


                // Outer : Circle : Point : apply Intersect Length
                Point3D outCircleLeftPoint = GetSumPoint(referencePoint, -outCircleRadius - intersectLength, 0);
                Point3D outCircleRighttPoint = GetSumPoint(referencePoint, outCircleRadius + intersectLength, 0);
                Point3D outCircleTopPoint = GetSumPoint(referencePoint, 0, outCircleRadius + intersectLength);
                Point3D outCircleBottomPoint = GetSumPoint(referencePoint, 0, -outCircleRadius - intersectLength);


                // Plate : Basic
                double maxPlateCount =Math.Ceiling( outCircleRadius / plateWidth);
                double refVerticalPlateCount = 2;
                double horizontalPlateCount = maxPlateCount - refVerticalPlateCount;

                double firstPlateLeftLength = 2000;

                #region Plate : Horizontal
                double currentTopY = 0;
                double currentBottomY = plateWidth;
                for (int i = 0; i < horizontalPlateCount; i++)
                {
                    Line vHorizontalLine = new Line(GetSumPoint(outCircleLeftPoint, 0, currentTopY), GetSumPoint(outCircleRighttPoint, 0, currentTopY));
                    Point3D[] vHorizontalLineInter = outCircle.IntersectWith(vHorizontalLine);
                    // 0 겹침 없음, 1이면 최상단과 겹침
                    if (vHorizontalLineInter.Length == 2)
                    {
                        Point3D leftPoint = vHorizontalLineInter[0];
                        Point3D rightPoint = vHorizontalLineInter[1];
                        if (leftPoint.X > rightPoint.X)
                        {
                            leftPoint = vHorizontalLineInter[1];
                            rightPoint = vHorizontalLineInter[0];
                        }
                        // Line
                        Line oneHorizontalLine = new Line(GetSumPoint(leftPoint, 0, 0), GetSumPoint(rightPoint, 0, 0));
                        horizontalLineList.Add(oneHorizontalLine);
                        // Point
                        horizontalPointList.Add(new Tuple<Point3D, Point3D>(leftPoint, rightPoint));
                    }
                    currentTopY += plateWidth;
                }
                #endregion

                #region Create Comb
                // Circle Comb
                for (int i = 1; i < horizontalPointList.Count; i++)
                {
                    Tuple<Point3D, Point3D> horizontalUnderPoint = horizontalPointList[i - 1];
                    Tuple<Point3D, Point3D> horizontalOnPoint = horizontalPointList[i];
                    CombModel newComb = new CombModel();
                    newComb.Teeth.Add(new CombToothModel(horizontalOnPoint.Item1.X, horizontalUnderPoint.Item1.X, horizontalOnPoint.Item1.X + horizontalMinSpacing));
                    newComb.Teeth.Add(new CombToothModel(horizontalOnPoint.Item2.X, horizontalUnderPoint.Item2.X-horizontalMinSpacing, horizontalOnPoint.Item2.X ));
                    horizontalCombList.Add(newComb);
                }

                #endregion


                double ceterX = referencePoint.X;
                double beforeStartX = 0;
                double currentStartX = 0;

                CombModel newUnderComb = null;
                for (int i=1;i< horizontalPlateCount; i++)
                {
                    Line oneHorizontalLine = horizontalLineList[i];
                    double oneHorizontalDiameter = oneHorizontalLine.Length();
                    double oneHorizontalRadius = oneHorizontalDiameter / 2;

                    // Case 1
                    currentStartX = GetStartXValue2(i,plateLength);
                    CombModel newOnComb = GetOneHorizontal(currentStartX, oneHorizontalDiameter, plateLength, intersectWidthHalf);

                    if (i == 1)
                    {
                        // 비교
                    }
                    else
                    {
                        // 비교
                    }
                    newUnderComb = newOnComb;
                }


                styleService.SetLayerListLine(ref horizontalLineList, layerService.LayerOutLine);
                styleService.SetLayerListLine(ref horizontalVList, layerService.LayerOutLine);
                styleService.SetLayerListEntity(ref plateList, layerService.LayerOutLine);

                styleService.SetLayerListTextEntity(ref textList, layerService.LayerBasicLine);
            }



            Circle outCircle1 = new Circle(GetSumPoint(referencePoint, 0, 0), 300);
            styleService.SetLayer(ref outCircle1, layerService.LayerOutLine);
            newList.Add(outCircle1);


            singleModel.Entities.AddRange(horizontalLineList);
            singleModel.Entities.AddRange(horizontalVList);
            singleModel.Entities.AddRange(plateList);

            singleModel.Entities.AddRange(newList);
            singleModel.Entities.AddRange(centerLineList);

            //singleModel.Entities.AddRange(textList);
            singleModel.Entities.Regen();
            //singleModel.Invalidate();
            singleModel.Entities.RegenAllCurved(0.005);
            singleModel.SetView(viewType.Top);
            singleModel.ZoomFit();
            singleModel.Refresh();

        }


        public void CreateThreeD(Model singleModel, out List<Brep> outNozzleList, out List<Brep> outFlangeList)
        {
            Point3D referencePoint = new Point3D(0, 0);

            singleModel.Entities.Clear();

            double shellRadius = 2700;
            double shellThk = 28;
            double shellHeight = 56350;

            double shellSeamCount = 1;
            double oneAngle = 360/shellSeamCount;
            double oneStartAngle = 0;

            double courseCount = 14;
            double courseOneHeight = shellHeight / courseCount;

            // Nozzle data
            double RotateNozzleAngle = 0;
            double nozzleRadius = 375;
            double nozzleOutsideRadius = 500;
            double nozzleInterval = 6750+1000;

            // Flange data
            List<Brep> flangeList = new List<Brep>();


            // Clip data
            List<Brep> ClipList = new List<Brep>();
            double clipCount = 5;
            double clipThick = 300;
            Point3D clipSize= new Point3D(30, 300);
            double startClipAngle = 30;
            double RotateClipAngle = 360 / clipCount;


            // SeamLine data
            List<Brep> horizontalSeamList = new List<Brep>();
            List<Brep> verticalSeamList = new List<Brep>();
            double RotateSeamAngle = 0;
            double SeamLineRadius = 10;
            




            List<Brep> oneCourse = new List<Brep>();
            if (true)
            {
                for (int j = 0; j < courseCount; j++)
                {
                    if (IsEvenNumber(j))
                        oneStartAngle = 0;
                    else
                        oneStartAngle = oneAngle / 1.5;

                    for (int i = 0; i < shellSeamCount; i++)
                    {
                        Region shell = Region.CreatePolygon(Plane.YZ, new Point3D[] { GetSumPoint(referencePoint, shellRadius, 0),
                                                                                      GetSumPoint(referencePoint, shellRadius + shellThk, 0),
                                                                                      GetSumPoint(referencePoint, shellRadius + shellThk, courseOneHeight),
                                                                                      GetSumPoint(referencePoint, shellRadius, courseOneHeight)});
                        
                        Brep shellsd = shell.RevolveAsBrep(Utility.DegToRad(oneStartAngle), Utility.DegToRad(oneAngle), Vector3D.AxisZ, new Point3D(0, 0, referencePoint.Z));

                        //shellsd.ColorMethod = colorMethodType.byLayer;
                        //shellsd.Color = Color.Yellow;
                        shellsd.Selectable = false;
                        oneCourse.Add(shellsd);
                        

                        // Vertical SeamLine Draw
                        Region seamV = Region.CreateCircle(Plane.XY, 0, -(shellRadius + shellThk), SeamLineRadius);
                        Brep verticalSeam = seamV.ExtrudeAsBrep(courseOneHeight);//   .RevolveAsBrep(Utility.DegToRad(360), Vector3D.AxisZ, new Point3D(0, 0, referencePoint.Z));
                        verticalSeam.Rotate(Utility.DegToRad(oneStartAngle+180), Vector3D.AxisZ, new Point3D(0, 0, 0));
                        Vector3D trValue = new Vector3D(0, 0, courseOneHeight*j);
                        verticalSeam.Translate(trValue);

                        verticalSeamList.Add(verticalSeam);
                        

                    }
                    // Horizontal SeamLine Draw
                    Region seamH = Region.CreateCircle(Plane.YZ, shellRadius + shellThk, referencePoint.Y, SeamLineRadius);
                    Brep horizontalSeam = seamH.RevolveAsBrep(Utility.DegToRad(360), Vector3D.AxisZ, new Point3D(0, 0, referencePoint.Z));

                    horizontalSeamList.Add(horizontalSeam);

                    referencePoint.Y += courseOneHeight;
                    oneStartAngle += oneAngle;

                }

            }


            // Nozzle
            List<Brep> nozzleList = new List<Brep>();
            if(true)
            {
                for (int i = 0; i < 8; i++)
                {
                    Region nozzle = Region.CreateCircle(Plane.XZ, 0, ((nozzleInterval*i) + 1400), nozzleRadius);
                    Brep nozzlesd = nozzle.ExtrudeAsBrep(600);
                    Vector3D trValue = new Vector3D(0, -shellRadius);
                    nozzlesd.Rotate(Utility.DegToRad(RotateNozzleAngle), Vector3D.AxisZ, new Point3D(0, shellRadius, 0));
                    nozzlesd.Translate(trValue);

                    Region nozzleInner = Region.CreateCircle(Plane.XZ, 0, ((nozzleInterval * i) + 1400), (nozzleRadius-20));
                    nozzlesd.ExtrudeRemove(nozzleInner, new Interval(0,-(shellRadius+ 600)));


                    nozzle = Region.CreateCircle(Plane.XZ, 0, ((nozzleInterval * i) + 1400), nozzleOutsideRadius);
                    Brep flangesd = nozzle.ExtrudeAsBrep(50);
                    trValue = new Vector3D(0, -(shellRadius+620));
                    flangesd.Rotate(Utility.DegToRad(RotateNozzleAngle), Vector3D.AxisZ, new Point3D(0, shellRadius, 0));
                    flangesd.Translate(trValue);

                    nozzleInner = Region.CreateCircle(Plane.XZ, 0, ((nozzleInterval * i) + 1400), (nozzleRadius - 20));
                    flangesd.ExtrudeRemove(nozzleInner, new Interval(0, -(shellRadius + 1000)));

                    nozzleList.Add(nozzlesd);
                    flangeList.Add(flangesd);


                    // Clip Draw
                    for (int k = 0; k < clipCount; k++)
                    {
                        Region clip = Region.CreatePolygon(Plane.XZ, new Point2D[] {new Point2D(-clipSize.X/2,clipSize.Y/2),
                                                                                    new Point2D(clipSize.X/2,clipSize.Y/2),
                                                                                    new Point2D(clipSize.X/2,-clipSize.Y/2),
                                                                                    new Point2D(-clipSize.X/2,-clipSize.Y/2)});
                        Brep bClip = clip.ExtrudeAsBrep((clipThick+(shellThk*2)));

                        trValue = new Vector3D(0, -shellRadius, ((nozzleInterval * i) + 400- nozzleRadius));
                        

                        
                        bClip.Rotate(Utility.DegToRad(startClipAngle+(RotateClipAngle*k)), Vector3D.AxisZ, new Point3D(0, shellRadius, 0));
                        bClip.Translate(trValue);

                        ClipList.Add(bClip);
                    }
                    
                }

            }


            outNozzleList = nozzleList;
            outFlangeList = flangeList;


            singleModel.Entities.AddRange(oneCourse,Color.FromArgb(125,255,255,0));
            singleModel.Entities.AddRange(nozzleList, Color.Blue);
            singleModel.Entities.AddRange(flangeList, Color.YellowGreen);
            singleModel.Entities.AddRange(ClipList, Color.OrangeRed);
            singleModel.Entities.AddRange(horizontalSeamList, Color.Silver);
            singleModel.Entities.AddRange(verticalSeamList, Color.Silver);

            //singleModel.Entities.Add(rev1);
            //singleModel.Entities.AddRange(textList);
            singleModel.Entities.Regen();
            //singleModel.Invalidate();
            //singleModel.Entities.RegenAllCurved(0.005);
            singleModel.SetView(viewType.Front);
            singleModel.ZoomFit();
            singleModel.Refresh();

        }

        public void CreateThreeD2(Model singleModel)
        {
            Point3D referencePoint = new Point3D(0, 0);

            singleModel.Entities.Clear();

            double shellRadius = 2700;
            double shellThk = 28;
            double shellHeight = 56350;

            double shellSeamCount = 1;
            double oneAngle = 360 / shellSeamCount;
            double oneStartAngle = 0;

            double courseCount = 1;
            double courseOneHeight = shellHeight / courseCount;

            // Nozzle data
            double RotateNozzleAngle = 0;
            double nozzleRadius = 375;
            double nozzleOutsideRadius = 500;
            double nozzleInterval = 6750 + 1000;

            // Flange data
            List<Brep> flangeList = new List<Brep>();


            // Clip data
            List<Brep> ClipList = new List<Brep>();
            double clipCount = 5;
            double clipThick = 300;
            Point3D clipSize = new Point3D(30, 300);
            double startClipAngle = 30;
            double RotateClipAngle = 360 / clipCount;


            // SeamLine data
            List<Brep> horizontalSeamList = new List<Brep>();
            List<Brep> verticalSeamList = new List<Brep>();
            double RotateSeamAngle = 0;
            double SeamLineRadius = 10;





            List<Brep> oneCourse = new List<Brep>();
            if (true)
            {
                for (int j = 0; j < courseCount; j++)
                {
                    if (IsEvenNumber(j))
                        oneStartAngle = 0;
                    else
                        oneStartAngle = oneAngle / 1.5;

                    for (int i = 0; i < shellSeamCount; i++)
                    {
                        Region shell = Region.CreatePolygon(Plane.YZ, new Point3D[] { GetSumPoint(referencePoint, shellRadius, 0),
                                                                                      GetSumPoint(referencePoint, shellRadius + shellThk, 0),
                                                                                      GetSumPoint(referencePoint, shellRadius + shellThk, courseOneHeight),
                                                                                      GetSumPoint(referencePoint, shellRadius, courseOneHeight)});

                        Brep shellsd = shell.RevolveAsBrep(Utility.DegToRad(oneStartAngle), Utility.DegToRad(oneAngle), Vector3D.AxisZ, new Point3D(0, 0, referencePoint.Z));

                        //shellsd.ColorMethod = colorMethodType.byLayer;
                        //shellsd.Color = Color.Yellow;

                        oneCourse.Add(shellsd);


                        // Vertical SeamLine Draw
                        Region seamV = Region.CreateCircle(Plane.XY, 0, -(shellRadius + shellThk), SeamLineRadius);
                        Brep verticalSeam = seamV.ExtrudeAsBrep(courseOneHeight);//   .RevolveAsBrep(Utility.DegToRad(360), Vector3D.AxisZ, new Point3D(0, 0, referencePoint.Z));
                        verticalSeam.Rotate(Utility.DegToRad(oneStartAngle + 180), Vector3D.AxisZ, new Point3D(0, 0, 0));
                        Vector3D trValue = new Vector3D(0, 0, courseOneHeight * j);
                        verticalSeam.Translate(trValue);

                        verticalSeamList.Add(verticalSeam);


                    }
                    // Horizontal SeamLine Draw
                    Region seamH = Region.CreateCircle(Plane.YZ, shellRadius + shellThk, referencePoint.Y, SeamLineRadius);
                    Brep horizontalSeam = seamH.RevolveAsBrep(Utility.DegToRad(360), Vector3D.AxisZ, new Point3D(0, 0, referencePoint.Z));

                    horizontalSeamList.Add(horizontalSeam);

                    referencePoint.Y += courseOneHeight;
                    oneStartAngle += oneAngle;

                }

            }



            Region arrowShape = new devDept.Eyeshot.Entities.Region(new LinearPath(Plane.XZ, new Point2D[]
            {
                new Point2D(0,-2),
                new Point2D(4,-2),
                new Point2D(4,-4),
                new Point2D(10,0),
                new Point2D(4,4),
                new Point2D(4,2),
                new Point2D(0,2),
                new Point2D(0,-2),
            }), Plane.XZ);

            //right arrow
            arrowShape.ExtrudeAsMesh(2, 0.1, Mesh.natureType.Plain);

            arrowShape.Regen(0.1);
            singleModel.TempEntities.Add(arrowShape, Color.Yellow);
            singleModel.TempEntities.UpdateBoundingBox();

            //singleModel.Entities.AddRange(oneCourse, Color.Yellow);
            //singleModel.Entities.AddRange(nozzleList, Color.Blue);
            singleModel.Entities.AddRange(flangeList, Color.YellowGreen);
            singleModel.Entities.AddRange(ClipList, Color.OrangeRed);
            singleModel.Entities.AddRange(horizontalSeamList, Color.Silver);
            singleModel.Entities.AddRange(verticalSeamList, Color.Silver);

            //singleModel.Entities.Add(rev1);
            //singleModel.Entities.AddRange(textList);
            singleModel.Entities.Regen();
            //singleModel.Invalidate();
            //singleModel.Entities.RegenAllCurved(0.005);
            singleModel.SetView(viewType.Front);
            singleModel.ZoomFit();
            singleModel.Refresh();

        }




        public double GetStartXValue(CASE_TYPE caseNumber,bool evenNumber,double plateLength, double horizontalSpacingShift)
        {
            //double firstPlateLeftLength = 4557;
            double firstPlateLeftLength = 2000;
            double returnValue = 0;
            switch (caseNumber)
            {
                case CASE_TYPE.CASE_01:
                    if (evenNumber)
                    {
                        // 좌측 : - 2000
                        returnValue = -firstPlateLeftLength;
                    }
                    else
                    {
                        // 우측
                        returnValue = 0;
                    }
                    break;

                case CASE_TYPE.CASE_02:
                    if (evenNumber)
                    {
                        // 좌측 : - 2000 + 850
                        returnValue = -firstPlateLeftLength+ horizontalSpacingShift;
                    }
                    else
                    {
                        // 중앙
                        returnValue = -plateLength/2;
                    }
                    break;

                case CASE_TYPE.CASE_03:
                    if (evenNumber)
                    {
                        // 양쪽 끼우기
                        returnValue = -firstPlateLeftLength + horizontalSpacingShift;
                    }
                    else
                    {
                        // 중앙 끼우기
                        returnValue = -plateLength / 2;
                    }
                    break;

                case CASE_TYPE.CASE_04:
                    break;
            }

            return returnValue;
        }

        public double GetStartXValue2( int selNumber, double plateLength)
        {
            // Event
            bool evenNumber = IsEvenNumber(selNumber);
            double firstPlateLeftLength = 2000;
            double returnValue = 0;

            if (selNumber == 1)
                return -firstPlateLeftLength;

            if (evenNumber)
            {
                returnValue = 0;
            }
            else
            {
                returnValue = -plateLength/2;
            }

            return returnValue;
        }

        public bool IsEvenNumber(int selNumber)
        {
            return selNumber % 2 == 0;
        }

        public CombModel GetOneHorizontal(double startX,double horizontalLength,double  plateLength,double intersectWidthHalf)
        {
            CombModel newComb = new CombModel();

            double leftX = startX - horizontalLength / 2 -plateLength;
            double rightX = startX + horizontalLength / 2 +plateLength;
            for (double i = startX; i < rightX; i += plateLength)
                newComb.Teeth.Add(new CombToothModel(i, i - intersectWidthHalf, i + intersectWidthHalf));
            for (double i = startX; i > leftX; i -= plateLength)
                newComb.Teeth.Add(new CombToothModel(i, i - intersectWidthHalf, i + intersectWidthHalf));

            return newComb;
        }

        public List<Line> GetHorizontalRightLine(Point3D centerPoint,Point3D startPointY,Circle outCircle,double oneHorizontalRadius,double firstPlateLeftLength,double plateWidth,double plateLength, out double lastLength)
        {
            List<Line> newList = new List<Line>();
            double currentX = centerPoint.X+ firstPlateLeftLength;
            double currentXMax = centerPoint.X + oneHorizontalRadius;
            while (currentX < currentXMax)
            {
                Point3D oneVerticalLineTop = new Point3D(currentX, startPointY.Y);
                Line vOneHorizontalV = new Line(GetSumPoint(oneVerticalLineTop, 0, 0), GetSumPoint(oneVerticalLineTop, 0, plateWidth));
                Point3D[] vOneHorizontalVInter = outCircle.IntersectWith(vOneHorizontalV);
                if (vOneHorizontalVInter.Length > 0)
                {
                    Line oneHorizontalV = new Line(GetSumPoint(oneVerticalLineTop, 0, 0), GetSumPoint(vOneHorizontalVInter[0], 0, 0));
                    newList.Add(oneHorizontalV);
                }
                else
                {
                    newList.Add(vOneHorizontalV);
                }
                currentX += plateLength;
            }

            // Out
             lastLength= currentXMax - currentX;
            if (lastLength < 0)
            {
                lastLength = currentXMax - currentX + plateLength;
            }

            return newList;
        }



        public Point3D GetMinPointXY(Point3D[] selPointArray)
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

        
    }
}

