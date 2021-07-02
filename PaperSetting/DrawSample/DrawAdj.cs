using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawWork.Commons;
using DrawWork.CutomModels;
using DrawWork.DrawModels;
using DrawWork.DrawServices;
using DrawWork.DrawStyleServices;
using DrawWork.ValueServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaperSetting.DrawSample
{
    public class DrawAdj
    {
        private ValueService valueService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;
        private DrawEditingService editingService;
        private DrawShapeServices shapeService;

        
        #region Public List
        List<Entity> structureGirderList = new List<Entity>();
        List<Entity> structureRafterList = new List<Entity>();
        List<Entity> structureColumnList = new List<Entity>();
        List<Entity> structureCenterLineList = new List<Entity>();
        List<Entity> nozzleList = new List<Entity>();

        List<Entity> DetectionList = new List<Entity>();
        public List<Point3D> DetectionPointList = new List<Point3D>();
        #endregion

        #region Rotate adj
        double rotationValue = 0;
        Point3D centerPoint = new Point3D();
        #endregion

        public DrawAdj()
        {
            valueService = new ValueService();
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();
            editingService = new DrawEditingService();
            shapeService = new DrawShapeServices();

            structureGirderList = new List<Entity>();
            structureRafterList = new List<Entity>();
            structureColumnList = new List<Entity>();
            structureCenterLineList = new List<Entity>();
            nozzleList = new List<Entity>();

            DetectionList = new List<Entity>();
            DetectionPointList = new List<Point3D>();
        }


        public void drawAllLayer(Model singleModel,bool nozzleValue, bool structureValue)
        {
            rotationValue = 0;

            if (true)
            {

                // Clear
                singleModel.Entities.Clear();

                structureGirderList.Clear();
                structureRafterList.Clear();
                structureCenterLineList.Clear();
                structureColumnList.Clear();
                nozzleList.Clear();

                // Default Value
                double selScaleValue = 90;
                double extMainLength = 4;
                double extSubLength = 2;


                List<Entity> newList = new List<Entity>();
                List<Point3D> newOutPoint = new List<Point3D>();

                Point3D referencePoint = new Point3D(10000, 10000);

                double tankID = 8000;
                double shellFirstThickness = 0;//<-----임시로 0
                double tankOD = tankID + shellFirstThickness * 2;
                double tankODRadius = tankOD / 2;

                Point3D oriCenterPoint = GetSumPoint(referencePoint, tankODRadius, tankODRadius);

                // public Center Point
                centerPoint = GetSumPoint(oriCenterPoint, 0, 0);

                // Outer
                Circle cirOD = new Circle(oriCenterPoint, tankODRadius);
                styleService.SetLayer(ref cirOD, layerService.LayerOutLine);
                newList.Add(cirOD);

                // Center Line
                List<Entity> centerVerticalLine = editingService.GetCenterLine(GetSumPoint(oriCenterPoint, 0, tankODRadius), GetSumPoint(oriCenterPoint, 0, -tankODRadius), extMainLength, selScaleValue);
                List<Entity> centerHorizontalLine = editingService.GetCenterLine(GetSumPoint(oriCenterPoint, -tankODRadius, 0), GetSumPoint(oriCenterPoint, tankODRadius, 0), extMainLength, selScaleValue);

                styleService.SetLayerListEntity(ref centerVerticalLine, layerService.LayerCenterLine);
                styleService.SetLayerListEntity(ref centerHorizontalLine, layerService.LayerCenterLine);


                structureCenterLineList.AddRange(centerVerticalLine);
                structureCenterLineList.AddRange(centerHorizontalLine);



                // Annular Type
                if (false)
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

                    for (int i = 0; i < circleDivNum; i++)
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

                // Strucutre
                double structureHeight = 0;


                // CutList
                List<Entity> cutList01 = new List<Entity>();
                List<Entity> cutList02 = new List<Entity>();

                if (structureValue)
                {
                    List<Tuple<double, double, double,double,double,double>> columnList = new List<Tuple<double, double, double,double,double,double>>();
                    columnList.Add(new Tuple<double, double, double,double,double,double>(1, 50, 0,6,6,3000));
                    columnList.Add(new Tuple<double, double, double,double,double,double>(5, 50, 3000,6,3,6000));
                    columnList.Add(new Tuple<double, double, double,double,double,double>(10, 50, 6000,8,2.25,8000));


                    int centerNumber = -1;
                    foreach (Tuple<double, double, double,double,double,double> eachColumn in columnList)
                    {
                        centerNumber++;
                        double columnCount = eachColumn.Item1;
                        double columnPipeOD = eachColumn.Item2;
                        double columnRadius = eachColumn.Item3;
                        double columnSNumber = eachColumn.Item4;
                        double columnStartAngle = eachColumn.Item5;
                        double nextRadius = eachColumn.Item6;

                        if (columnRadius == 0)
                        {
                            // Center : Only One
                            Circle cirColumnCenter = new Circle(GetSumPoint(oriCenterPoint, 0, 0), columnPipeOD / 2);
                            styleService.SetLayer(ref cirColumnCenter, layerService.LayerHiddenLine);
                            structureColumnList.Add(cirColumnCenter);
                        }
                        else
                        {
                            Circle cirColumnCenterLine = new Circle(GetSumPoint(oriCenterPoint, 0, 0), columnRadius / 2);
                            styleService.SetLayer(ref cirColumnCenterLine, layerService.LayerCenterLine);
                            structureColumnList.Add(cirColumnCenterLine);

                            double circlePerimeter = valueService.GetCirclePerimeter(columnRadius);
                            double circleDivOneWidth = circlePerimeter / columnCount;
                            double divOneRadius = valueService.GetRadianByArcLength(circleDivOneWidth, columnRadius);

                            List<Point3D> cirConnPointList = new List<Point3D>();
                            double startRadianSum = 0;
                            for (int i = 0; i < columnCount; i++)
                            {
                                Line eachLine = new Line(GetSumPoint(oriCenterPoint, 0, 0), GetSumPoint(oriCenterPoint, 0, columnRadius * 2));
                                eachLine.Rotate(-startRadianSum, Vector3D.AxisZ, GetSumPoint(oriCenterPoint, 0, 0));

                                Point3D[] interPoint = eachLine.IntersectWith(cirColumnCenterLine);
                                if (interPoint.Length > 0)
                                {
                                    cirConnPointList.Add(interPoint[0]);

                                    // Pipe
                                    Circle cirColumn = new Circle(GetSumPoint(interPoint[0], 0, 0), columnPipeOD / 2);
                                    styleService.SetLayer(ref cirColumn, layerService.LayerHiddenLine);
                                    structureColumnList.Add(cirColumn);

                                    Point3D[] cirInterPoint = eachLine.IntersectWith(cirColumn);
                                    if (cirInterPoint.Length > 0)
                                    {
                                        List<Entity> eachCirColumnCenterLine = editingService.GetCenterLine(GetSumPoint(cirInterPoint[0], 0, 0), GetSumPoint(cirInterPoint[1], 0, 0), extSubLength, selScaleValue);
                                        styleService.SetLayerListEntity(ref eachCirColumnCenterLine, layerService.LayerCenterLine);

                                        structureCenterLineList.AddRange(eachCirColumnCenterLine);
                                    }
                                }
                                startRadianSum += divOneRadius;
                            }




                            // Circle Connection
                            if (cirConnPointList.Count > 1)
                            {
                                List<Point3D> extList = new List<Point3D>();
                                List<Entity> circleConnLineList = new List<Entity>();
                                Line connLine01 = new Line(cirConnPointList[0], cirConnPointList[cirConnPointList.Count - 1]);

                                circleConnLineList.Add(connLine01);
                                if (centerNumber == 1)
                                    cutList01.Add((Line)connLine01.Clone());
                                if (centerNumber == 2)
                                    cutList02.Add((Line)connLine01.Clone());

                                // Structure
                                structureHeight = columnPipeOD;
                                structureGirderList.AddRange(GetStructureLineHorizon(structureHeight, cirConnPointList[0], cirConnPointList[cirConnPointList.Count - 1]));
                                for (int i = 0; i < cirConnPointList.Count - 1; i++)
                                {
                                    Line connLine02 = new Line(cirConnPointList[i], cirConnPointList[i + 1]);

                                    circleConnLineList.Add(connLine02);
                                    if (centerNumber == 1)
                                        cutList01.Add((Line)connLine02.Clone());
                                    if (centerNumber == 2)
                                        cutList02.Add((Line)connLine02.Clone());
                                    // Structure
                                    structureGirderList.AddRange(GetStructureLineHorizon(structureHeight, cirConnPointList[i], cirConnPointList[i+1]));
                                }
                                styleService.SetLayerListEntity(ref circleConnLineList, layerService.LayerCenterLine);

                                //structureCenterLineList.AddRange(circleConnLineList);

                            }



                        }


                    }

                    centerNumber = -1;
                    foreach (Tuple<double, double, double, double, double, double> eachColumn in columnList)
                    {
                        centerNumber++;

                        double columnCount = eachColumn.Item1;
                        double columnPipeOD = eachColumn.Item2;
                        double columnRadius = eachColumn.Item3;
                        double columnSNumber = eachColumn.Item4;
                        double columnStartAngle = eachColumn.Item5;
                        double nextRadius = eachColumn.Item6;

                   
                        // Vertical Structure
                        if (true)
                        {

                            double verStructureHeight = 5;
                            double verLength = nextRadius / 2;
                            int startAngle = Convert.ToInt32(columnStartAngle);
                            int startAngle2 = startAngle * 2;
                            for (int i = startAngle; i < 360; i = i + startAngle2)
                            {
                                Line verLine = null;
                                verLine = new Line(GetSumPoint(oriCenterPoint, 0, 0), GetSumPoint(oriCenterPoint, 0, verLength));
                                verLine.Rotate(Utility.DegToRad(i), Vector3D.AxisZ, GetSumPoint(oriCenterPoint, 0, 0));

                                if (centerNumber == 0)
                                {
                                    foreach(Entity eachEntity in cutList01)
                                    {
                                        Point3D[] eachInter = verLine.IntersectWith((Line)eachEntity);
                                        if (eachInter.Length > 0)
                                            verLine= new Line(GetSumPoint(oriCenterPoint, 0, 0), GetSumPoint(eachInter[0], 0, 0));
                                    }
                                }
                                if (centerNumber == 1)
                                {
                                    foreach (Entity eachEntity in cutList02)
                                    {
                                        Point3D[] eachInter = verLine.IntersectWith((Line)eachEntity);
                                        if (eachInter.Length > 0)
                                            verLine = new Line(GetSumPoint(oriCenterPoint, 0, 0), GetSumPoint(eachInter[0], 0, 0));
                                    }
                                    foreach (Entity eachEntity in cutList01)
                                    {
                                        Point3D[] eachInter = verLine.IntersectWith((Line)eachEntity);
                                        if (eachInter.Length > 0)
                                            verLine = new Line(GetSumPoint(eachInter[0], 0, 0), GetSumPoint(verLine.EndPoint, 0, 0));
                                    }

                                }
                                if (centerNumber == 2)
                                {
                                    foreach (Entity eachEntity in cutList02)
                                    {
                                        Point3D[] eachInter = verLine.IntersectWith((Line)eachEntity);
                                        if (eachInter.Length > 0)
                                            verLine = new Line(GetSumPoint(verLine.EndPoint, 0, 0), GetSumPoint(eachInter[0], 0, 0));
                                    }

                                }

                                structureRafterList.AddRange(GetStructureLine(verStructureHeight, GetSumPoint(verLine.StartPoint, 0, 0), GetSumPoint(verLine.EndPoint, 0, 0)));

                            }


                        }
                    }


                }


                



                if (nozzleValue)
                {
                    DrawNozzleBlockService nozzleDraw = new DrawNozzleBlockService(null);

                    double nozzleDegree = 124;
                    double nozzleDegreeRadian = Utility.DegToRad(nozzleDegree);
                    double nozzleRadius = 2300;

                    Line nozzleLine = new Line(GetSumPoint(oriCenterPoint, 0, 0), GetSumPoint(oriCenterPoint, 0, nozzleRadius));
                    nozzleLine.Rotate(-nozzleDegreeRadian, Vector3D.AxisZ, oriCenterPoint);

                    List<Point3D> outputPointList = new List<Point3D>();
                    List<Entity> newFlangeTop = nozzleDraw.DrawReference_Nozzle_Flange_Top(out outputPointList, GetSumPoint(nozzleLine.EndPoint, 0, 0), 0, 100, 120, 140, nozzleRadius, -nozzleDegreeRadian, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, exLength = extSubLength, arcEx = true, twoEx = true });

                    Line nozzleLine2 = new Line(GetSumPoint(oriCenterPoint, 0, 0), GetSumPoint(oriCenterPoint, 0, nozzleRadius));
                    nozzleLine2.Rotate(-Utility.DegToRad(80), Vector3D.AxisZ, oriCenterPoint);

                    Line nozzleLine3 = (Line)nozzleLine2.Offset(1050, Vector3D.AxisZ);


                    //singleModel.Entities.Add(nozzleLine2);
                    //singleModel.Entities.Add(nozzleLine3);
                    Line nozzleLine5 = new Line(GetSumPoint(oriCenterPoint, 0, 0), GetSumPoint(oriCenterPoint, 0, 3400));
                    nozzleLine5.Rotate(-Utility.DegToRad(73.5), Vector3D.AxisZ, oriCenterPoint);
                    List<Entity> newFlangeTop5 = nozzleDraw.DrawReference_Nozzle_Flange_Top(out outputPointList, GetSumPoint(nozzleLine5.EndPoint, 0, 0), 0, 100, 120, 140, nozzleRadius, -Utility.DegToRad(80), 0, new DrawCenterLineModel() { scaleValue = selScaleValue, exLength = extSubLength, twoEx = true });


                    List<Entity> newFlangeTop1 = nozzleDraw.DrawReference_Nozzle_Flange_Top(out outputPointList, GetSumPoint(nozzleLine2.EndPoint, 0, 0), 0, 100, 120, 140, nozzleRadius, -Utility.DegToRad(80), 0, new DrawCenterLineModel() { scaleValue = selScaleValue, exLength = extSubLength, twoEx = true });
                    List<Entity> newFlangeTop2 = nozzleDraw.DrawReference_Nozzle_Flange_Top(out outputPointList, GetSumPoint(nozzleLine3.EndPoint, 0, 0), 0, 100, 120, 140, nozzleRadius, -Utility.DegToRad(80), 0, new DrawCenterLineModel() { scaleValue = selScaleValue, exLength = extSubLength, twoEx = true });



                    Line nozzleLine4 = new Line(GetSumPoint(oriCenterPoint, 0, 0), GetSumPoint(oriCenterPoint, 0, 5000));
                    //nozzleLine4.Rotate(-Utility.DegToRad(30), Vector3D.AxisZ, oriCenterPoint);
                    //singleModel.Entities.Add(nozzleLine4);



                    List<Entity> testFlange = nozzleDraw.DrawReference_Nozzle_PipeTrimByCircle(out outputPointList, ref cirOD, ref nozzleLine4, nozzleLine4.EndPoint, 100, new DrawCenterLineModel() { scaleValue = selScaleValue, exLength = extSubLength, twoEx = true });
                    //singleModel.Entities.AddRange(testFlange);




                    nozzleList.AddRange(newFlangeTop);
                    nozzleList.AddRange(newFlangeTop1);
                    nozzleList.AddRange(newFlangeTop2);
                    nozzleList.AddRange(newFlangeTop5);

                }



                // structure Girder Line
                styleService.SetLayerListEntity(ref structureGirderList, layerService.LayerOutLine);
                singleModel.Entities.AddRange(structureGirderList);
                // structure Rafter
                styleService.SetLayerListEntity(ref structureRafterList, layerService.LayerOutLine);
                singleModel.Entities.AddRange(structureRafterList);
                // structure Column List
                singleModel.Entities.AddRange(structureColumnList);

                // structure Center Line
                singleModel.Entities.AddRange(structureCenterLineList);

                // nozzle List
                singleModel.Entities.AddRange(nozzleList);

                // etc List
                singleModel.Entities.AddRange(newList);

                //
                singleModel.Entities.Regen();
                singleModel.ZoomFit();
            }
        }

        public List<Line> GetStructureLine(double selHeight, Point3D startPoint, Point3D endPoint)
        {
            List<Line> newList = new List<Line>();

            double heightHalf = selHeight / 2;
            Line lineCenter = new Line(GetSumPoint(startPoint, 0, 0), GetSumPoint(endPoint, 0, 0));

            Line line01 = (Line)lineCenter.Offset(heightHalf, Vector3D.AxisZ);
            Line line02 = (Line)lineCenter.Offset(-heightHalf, Vector3D.AxisZ);
            Line line03 = new Line(GetSumPoint(line01.StartPoint, 0, 0), GetSumPoint(line02.StartPoint, 0, 0));
            Line line04 = new Line(GetSumPoint(line01.EndPoint, 0, 0), GetSumPoint(line02.EndPoint, 0, 0));

            newList.Add(line01);
            newList.Add(line02);
            newList.Add(line03);
            newList.Add(line04);

            return newList;
        }

        public List<Line> GetStructureLineHorizon(double selHeight, Point3D startPoint, Point3D endPoint)
        {
            List<Line> newList = new List<Line>();

            double heightHalf = selHeight / 2;
            Line vlineCenter = new Line(GetSumPoint(startPoint, 0, 0), GetSumPoint(endPoint, 0, 0));

            Circle startCir = new Circle(GetSumPoint(startPoint, 0, 0), heightHalf);
            Circle endCir = new Circle(GetSumPoint(endPoint, 0, 0), heightHalf);
            Point3D startInter = editingService.GetIntersectWidth(vlineCenter, startCir, 0);
            Point3D endInter = editingService.GetIntersectWidth(vlineCenter, endCir, 0);


            Line lineCenter = new Line(GetSumPoint(startInter, 0, 0), GetSumPoint(endInter, 0, 0));
            Line line01 = (Line)lineCenter.Offset(heightHalf, Vector3D.AxisZ);
            Line line02 = (Line)lineCenter.Offset(-heightHalf, Vector3D.AxisZ);
            Line line03 = new Line(GetSumPoint(line01.StartPoint, 0, 0), GetSumPoint(line02.StartPoint, 0, 0));
            Line line04 = new Line(GetSumPoint(line01.EndPoint, 0, 0), GetSumPoint(line02.EndPoint, 0, 0));

            newList.Add(line01);
            newList.Add(line02);
            newList.Add(line03);
            newList.Add(line04);

            return newList;
        }


        public void Detection(Model singleModel, bool rotation = false)
        {
            bool detectionVisible = true;
            
            foreach(Entity eachEntity in DetectionList)
            {
                singleModel.Entities.Remove(eachEntity);
            }



            DetectionList.Clear();
            DetectionPointList.Clear();


            // Intersert
            foreach (Entity eachEntity in nozzleList)
            {
                if (eachEntity.LayerName == layerService.LayerOutLine)
                {
                    if(eachEntity is Circle)
                    {
                        Circle eachCircle = eachEntity as Circle;

                        foreach (Entity eachRafter in structureRafterList)
                        {
                            Point3D[] newInter=eachCircle.IntersectWith((ICurve)eachRafter);
                            if (newInter.Length > 0)
                                DetectionPointList.AddRange(newInter);
                        }
                        foreach (Entity eachColumn in structureRafterList)
                        {
                            Point3D[] newInter = eachCircle.IntersectWith((ICurve)eachColumn);
                            if (newInter.Length > 0)
                                DetectionPointList.AddRange(newInter);
                        }
                        foreach (Entity eachGirder in structureRafterList)
                        {
                            Point3D[] newInter = eachCircle.IntersectWith((ICurve)eachGirder);
                            if (newInter.Length > 0)
                                DetectionPointList.AddRange(newInter);
                        }
                    }

                }
            }


            if (detectionVisible)
            {
                double markWidth = 100;
                double markHeight = 100;
                List<Point3D> outList = new List<Point3D>();
                foreach (Point3D eachPoint in DetectionPointList)
                {
                    List<Entity> eachRec = shapeService.GetRectangle(out outList, GetSumPoint(eachPoint, -markWidth / 2, markHeight / 2), markWidth, markHeight, 0, 0, 0);
                    DetectionList.AddRange(eachRec);
                }

                styleService.SetLayerListEntity(ref DetectionList, layerService.LayerHiddenLine);

                
                
                singleModel.Entities.AddRange(DetectionList);
                singleModel.Entities.Regen();
                singleModel.Refresh();
                singleModel.Invalidate();
            }
        }

        public void RotateLayer(Model singleModel)
        {
            double rotateFactor = -1;
            double currentRotation = Utility.DegToRad(rotateFactor);
            foreach(Entity eachEntity in structureCenterLineList)
            {
                eachEntity.Rotate(currentRotation, Vector3D.AxisZ, centerPoint);
            }
            foreach (Entity eachEntity in structureColumnList)
            {
                eachEntity.Rotate(currentRotation, Vector3D.AxisZ, centerPoint);
            }
            foreach (Entity eachEntity in structureGirderList)
            {
                eachEntity.Rotate(currentRotation, Vector3D.AxisZ, centerPoint);
            }
            foreach (Entity eachEntity in structureRafterList)
            {
                eachEntity.Rotate(currentRotation, Vector3D.AxisZ, centerPoint);
            }
            singleModel.Entities.Regen();
            singleModel.Refresh();
            singleModel.Invalidate();
        }

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
    }
}
