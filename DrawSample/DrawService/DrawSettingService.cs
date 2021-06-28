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

                styleService.SetLayerListEntity(ref newLineList, layerService.LayerOutLine);


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
                    List<Entity> eachRec=shapeService.GetRectangle(out outList, GetSumPoint(eachPoint[0], -markWidth/2, markHeight/2), markWidth, markHeight, 0, 0, 0);
                    markList.AddRange(eachRec);
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
