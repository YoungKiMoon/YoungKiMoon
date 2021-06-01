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
using DrawWork.ValueServices;
using AssemblyLib.AssemblyModels;
using DrawWork.DrawStyleServices;
using DrawWork.DrawModels;
using DrawWork.Commons;

namespace DrawWork.DrawServices
{
    public class DrawNozzleBlockService
    {
        private ValueService valueService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;


        private DrawEditingService editingService;
        private DrawWorkingPointService workingService;


        public DrawNozzleBlockService(AssemblyModel selAssembly)
        {
            valueService = new ValueService();
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();

            editingService = new DrawEditingService();
            workingService = new DrawWorkingPointService(selAssembly);
        }
        public List<Entity> DrawReference_Nozzle_Elbow(out List<Point3D> selOutputPointList, Point3D selPoint1, int selPointNumber, ElbowModel selElbow, string selType = "lr", string selDegree = "90", double selRotate = 0, DrawCenterLineModel selCenterLine = null)
        {

            double OD = valueService.GetDoubleValue(selElbow.OD);
            double A = valueService.GetDoubleValue(selElbow.LRA);
            double B = valueService.GetDoubleValue(selElbow.LRB);
            double D = valueService.GetDoubleValue(selElbow.SRD);

            List<Entity> newList = new List<Entity>();

            // WP : Left Lower
            Point3D WP = new Point3D(selPoint1.X, selPoint1.Y);

            double AD = 0;
            if (selType == "lr")
                AD = A;
            else if (selType == "sr")
                AD = D;

            double centerMiddle = OD / 2;

            Point3D WPArc = null;
            Point3D WPRotate = null;


            // Drawing Shape
            Line line00 = null;
            Line line01 = null;

            if (selDegree == "90")
            {
                double boxWidth = centerMiddle + AD;
                WPArc = GetSumPoint(WP, boxWidth, 0);
                WPRotate = WPArc;

                line00 = new Line(GetSumPoint(WP, 0, 0), GetSumPoint(WP, OD, 0));
                line01 = new Line(GetSumPoint(WP, boxWidth, boxWidth), GetSumPoint(WP, boxWidth, boxWidth - OD));

                Arc arc01 = new Arc(WPArc, line00.StartPoint, line01.StartPoint);
                Arc arc02 = new Arc(WPArc, line00.EndPoint, line01.EndPoint);
                newList.Add(line00);
                newList.Add(line01);
                newList.Add(arc01);
                newList.Add(arc02);

            }
            else if (selDegree == "45")
            {

                double arcOne = B + B * Math.Sqrt(2);
                double boxWidth = centerMiddle + arcOne;
                WPArc = GetSumPoint(WP, boxWidth, 0);
                WPRotate = WPArc;

                line00 = new Line(GetSumPoint(WP, 0, 0), GetSumPoint(WP, OD, 0));
                line01 = (Line)line00.Clone();
                line01.Rotate(Utility.DegToRad(-45), Vector3D.AxisZ, WPArc);

                Arc arc01 = new Arc(WPArc, line00.StartPoint, line01.StartPoint);
                Arc arc02 = new Arc(WPArc, line00.EndPoint, line01.EndPoint);
                newList.Add(line00);
                newList.Add(line01);
                newList.Add(arc01);
                newList.Add(arc02);
            }


            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            // Center Line
            if (selCenterLine != null)
            {
                if (selCenterLine.centerLine)
                {
                    // 기본 형상
                    Point3D zeroPoint = line00.MidPoint;
                    Point3D onePoint = line01.MidPoint;
                    Arc arcCenter = new Arc(WPArc, zeroPoint, onePoint);
                    styleService.SetLayer(ref arcCenter, layerService.LayerCenterLine);
                    newList.Add(arcCenter);

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
                        Line oneLine = new Line(GetSumPoint(onePoint, 0, 0), GetSumPoint(onePoint, selCenterLine.exLength * selCenterLine.scaleValue, 0));
                        if (selDegree == "45")
                            oneLine.Rotate(Utility.DegToRad(45), Vector3D.AxisZ, onePoint);
                        styleService.SetLayer(ref oneLine, layerService.LayerCenterLine);
                        newList.Add(oneLine);
                    }
                }
            }

            // Rotate
            if (selRotate != 0)
            {
                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }

            // Rotate After : Set Point : Translate
            if (selPointNumber >= 0)
            {
                if (selPointNumber == 0)
                    line00.EntityData = line00.MidPoint;
                else if (selPointNumber == 1)
                    line01.EntityData = line01.MidPoint;
            }

            // Translate
            if (selPointNumber > -1)
            {
                Point3D translatePoint = GetTranslatePoint(ref newList);
                if (translatePoint != null)
                    SetTranslate(ref newList, WP, translatePoint);

            }

            selOutputPointList = new List<Point3D>();
            selOutputPointList.Add(line00.MidPoint);
            selOutputPointList.Add(line01.MidPoint);

            return newList;
        }


        public List<Entity> DrawReference_Nozzle_Tee(out List<Point3D> selOutputPointList, Point3D selPoint1, int selPointNumber, TeeModel selTee, double selRotate = 0, DrawCenterLineModel selCenterLine = null)
        {

            double OD = valueService.GetDoubleValue(selTee.OD);
            double A = valueService.GetDoubleValue(selTee.A);
            double B = valueService.GetDoubleValue(selTee.B);


            List<Entity> newList = new List<Entity>();

            // WP : Left Lower
            Point3D WP = new Point3D(selPoint1.X, selPoint1.Y);
            double centerMiddle = OD / 2;
            Point3D WPRotate = GetSumPoint(WP, A, centerMiddle);


            // Drawing Shape
            Line line00 = null;
            Line line01 = null;
            Line line02 = null;

            line00 = new Line(GetSumPoint(WPRotate, -centerMiddle, B), GetSumPoint(WPRotate, centerMiddle, B));
            line01 = new Line(GetSumPoint(WP, 0, 0), GetSumPoint(WP, 0, OD));
            line02 = new Line(GetSumPoint(WP, A * 2, 0), GetSumPoint(WP, A * 2, OD));

            Line exLine01 = new Line(GetSumPoint(WP, 0, 0), GetSumPoint(WP, A * 2, 0));
            Line exLine02 = new Line(GetSumPoint(WP, 0, OD), GetSumPoint(WP, A - centerMiddle, OD));
            Line exLine03 = new Line(GetSumPoint(WP, A + centerMiddle, OD), GetSumPoint(WP, A * 2, OD));
            Line exLine04 = new Line(GetSumPoint(WP, A - centerMiddle, OD), GetSumPoint(WPRotate, 0, 0));
            Line exLine05 = new Line(GetSumPoint(WP, A + centerMiddle, OD), GetSumPoint(WPRotate, 0, 0));
            Line exLine06 = new Line(GetSumPoint(WPRotate, -centerMiddle, centerMiddle), GetSumPoint(WPRotate, -centerMiddle, B));
            Line exLine07 = new Line(GetSumPoint(WPRotate, +centerMiddle, centerMiddle), GetSumPoint(WPRotate, centerMiddle, B));

            newList.AddRange(new Line[] { exLine01, exLine02, exLine03, exLine04, exLine05, exLine06, exLine07 });

            newList.Add(line00);
            newList.Add(line01);
            newList.Add(line02);




            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            // Center Line
            if (selCenterLine != null)
            {
                if (selCenterLine.centerLine)
                {
                    // 기본 형상
                    Point3D zeroPoint = line00.MidPoint;
                    Point3D onePoint = line01.MidPoint;
                    Point3D twoPoint = line02.MidPoint;
                    Line centerLine00 = new Line(onePoint, twoPoint);
                    Line centerLine01 = new Line(zeroPoint, GetSumPoint(zeroPoint, 0, -B - centerMiddle));

                    styleService.SetLayer(ref centerLine00, layerService.LayerCenterLine);
                    styleService.SetLayer(ref centerLine01, layerService.LayerCenterLine);
                    newList.Add(centerLine00);
                    newList.Add(centerLine01);

                    // 0
                    if (selCenterLine.zeroEx)
                    {
                        Line zeroLine = new Line(GetSumPoint(zeroPoint, 0, 0), GetSumPoint(zeroPoint, 0, selCenterLine.exLength * selCenterLine.scaleValue));
                        styleService.SetLayer(ref zeroLine, layerService.LayerCenterLine);
                        newList.Add(zeroLine);
                    }

                    // 1
                    if (selCenterLine.oneEx)
                    {
                        Line oneLine = new Line(GetSumPoint(onePoint, 0, 0), GetSumPoint(onePoint, -selCenterLine.exLength * selCenterLine.scaleValue, 0));
                        styleService.SetLayer(ref oneLine, layerService.LayerCenterLine);
                        newList.Add(oneLine);
                    }
                    // 2
                    if (selCenterLine.twoEx)
                    {
                        Line twoLine = new Line(GetSumPoint(twoPoint, 0, 0), GetSumPoint(twoPoint, +selCenterLine.exLength * selCenterLine.scaleValue, 0));
                        styleService.SetLayer(ref twoLine, layerService.LayerCenterLine);
                        newList.Add(twoLine);
                    }
                }
            }

            // Rotate
            if (selRotate != 0)
            {
                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }

            // Rotate After : Set Point : Translate
            if (selPointNumber >= 0)
            {
                if (selPointNumber == 0)
                    line00.EntityData = line00.MidPoint;
                else if (selPointNumber == 1)
                    line01.EntityData = line01.MidPoint;
                else if (selPointNumber == 2)
                    line02.EntityData = line02.MidPoint;
            }

            // Translate
            if (selPointNumber > -1)
            {
                Point3D translatePoint = GetTranslatePoint(ref newList);
                if (translatePoint != null)
                    SetTranslate(ref newList, WP, translatePoint);

            }

            selOutputPointList = new List<Point3D>();
            selOutputPointList.Add(line00.MidPoint);
            selOutputPointList.Add(line01.MidPoint);
            selOutputPointList.Add(line02.MidPoint);

            return newList;
        }

        public List<Entity> DrawReference_Nozzle_Reducer(out List<Point3D> selOutputPointList, Point3D selPoint1, int selPointNumber, ReducerModel selReducer, double selRotate = 0, DrawCenterLineModel selCenterLine = null)
        {

            double OD1 = valueService.GetDoubleValue(selReducer.OD1);
            double OD2 = valueService.GetDoubleValue(selReducer.OD2);
            double H = valueService.GetDoubleValue(selReducer.H);
            double A = valueService.GetDoubleValue(selReducer.A);


            List<Entity> newList = new List<Entity>();

            // WP : Left Lower
            Point3D WP = new Point3D(selPoint1.X, selPoint1.Y);
            double centerMiddle1 = OD1 / 2;
            double centerMiddle2 = OD2 / 2;
            A = (OD1 - OD2) / 2;
            double eightA = A / 8;
            double fourH = H / 4;
            Point3D WPRotate = WP;


            // Drawing Shape
            Line line00 = null;
            Line line01 = null;

            line00 = new Line(GetSumPoint(WP, 0, 0), GetSumPoint(WP, OD1, 0));
            line01 = new Line(GetSumPoint(WP, A, H), GetSumPoint(WP, A + OD2, H));

            Arc exArcLeft01 = new Arc(Plane.XY, GetSumPoint(WP, 0, 0), GetSumPoint(WP, eightA, fourH), GetSumPoint(WP, eightA * 4, fourH * 2), true);
            Arc exArcLeft02 = new Arc(Plane.XY, GetSumPoint(WP, eightA * 4, fourH * 2), GetSumPoint(WP, A - eightA, fourH * 3), GetSumPoint(WP, A, H), false);
            Arc exArcRight01 = new Arc(Plane.XY, GetSumPoint(WP, OD1, 0), GetSumPoint(WP, OD1 - eightA, fourH), GetSumPoint(WP, OD1 - eightA * 4, fourH * 2), false);
            Arc exArcRight02 = new Arc(Plane.XY, GetSumPoint(WP, OD1 - eightA * 4, fourH * 2), GetSumPoint(WP, OD1 - (A - eightA), fourH * 3), GetSumPoint(WP, OD1 - A, H), true);

            newList.AddRange(new Arc[] { exArcLeft01, exArcLeft02, exArcRight01, exArcRight02 });

            newList.Add(line00);
            newList.Add(line01);

            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            // Center Line
            if (selCenterLine != null)
            {
                if (selCenterLine.centerLine)
                {
                    // 기본 형상
                    Point3D zeroPoint = line00.MidPoint;
                    Point3D onePoint = line01.MidPoint;
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
                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }

            // Rotate After : Set Point : Translate
            if (selPointNumber >= 0)
            {
                if (selPointNumber == 0)
                    line00.EntityData = line00.MidPoint;
                else if (selPointNumber == 1)
                    line01.EntityData = line01.MidPoint;
            }

            // Translate
            if (selPointNumber > -1)
            {
                Point3D translatePoint = GetTranslatePoint(ref newList);
                if (translatePoint != null)
                    SetTranslate(ref newList, WP, translatePoint);

            }

            selOutputPointList = new List<Point3D>();
            selOutputPointList.Add(line00.MidPoint);
            selOutputPointList.Add(line01.MidPoint);

            return newList;
        }

        public List<Entity> DrawReference_Nozzle_Pipe(out List<Point3D> selOutputPointList, Point3D selPoint1, int selPointNumber, double selOD, double selLength, bool selZeroView = true, bool selOneView = true, double selRotate = 0, DrawCenterLineModel selCenterLine = null)
        {

            double OD = selOD;
            double Length = selLength;


            List<Entity> newList = new List<Entity>();

            // WP : Left Lower
            Point3D WP = new Point3D(selPoint1.X, selPoint1.Y);
            double centerMiddle = OD / 2;
            Point3D WPRotate = WP;


            // Drawing Shape
            Line line00 = null;
            Line line01 = null;

            line00 = new Line(GetSumPoint(WP, 0, 0), GetSumPoint(WP, OD, 0));
            line01 = new Line(GetSumPoint(WP, 0, Length), GetSumPoint(WP, OD, Length));

            Line exLine01 = new Line(GetSumPoint(line00.StartPoint, 0, 0), GetSumPoint(line01.StartPoint, 0, 0));
            Line exLine02 = new Line(GetSumPoint(line00.EndPoint, 0, 0), GetSumPoint(line01.EndPoint, 0, 0));

            newList.AddRange(new Line[] { exLine01, exLine02 });

            newList.Add(line00);
            newList.Add(line01);

            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            // Center Line
            if (selCenterLine != null)
            {
                if (selCenterLine.centerLine)
                {
                    // 기본 형상
                    Point3D zeroPoint = line00.MidPoint;
                    Point3D onePoint = line01.MidPoint;
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
                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }

            // Rotate After : Set Point : Translate
            if (selPointNumber >= 0)
            {
                if (selPointNumber == 0)
                    line00.EntityData = line00.MidPoint;
                else if (selPointNumber == 1)
                    line01.EntityData = line01.MidPoint;
            }

            // Translate
            if (selPointNumber > -1)
            {
                Point3D translatePoint = GetTranslatePoint(ref newList);
                if (translatePoint != null)
                    SetTranslate(ref newList, WP, translatePoint);

            }

            selOutputPointList = new List<Point3D>();
            selOutputPointList.Add(line00.MidPoint);
            selOutputPointList.Add(line01.MidPoint);

            if (!selZeroView)
                newList.Remove(line00);
            if (!selOneView)
                newList.Remove(line01);

            return newList;
        }


        public List<Entity> DrawReference_Nozzle_PipeSlopeAll(out List<Point3D> selOutputPointList, Point3D selPoint1, int selPointNumber, double selOD, double selLength, double selSlopeDgree, double selOverlap = 0, double selRotate = 0, DrawCenterLineModel selCenterLine = null)
        {
            if (SingletonData.TankType == TANK_TYPE.CRT)
            {
                return DrawReference_Nozzle_PipeSlope(out selOutputPointList, selPoint1, selPointNumber, selOD, selOD, selSlopeDgree, selOverlap, selRotate, selCenterLine);
            }
            else
            {
                return DrawReference_Nozzle_PipeArc(out selOutputPointList, selPoint1, selPointNumber, selOD, selOD, selSlopeDgree, selOverlap, selRotate, selCenterLine);
            }
        }
        public List<Entity> DrawReference_Nozzle_PipeSlope(out List<Point3D> selOutputPointList, Point3D selPoint1, int selPointNumber, double selOD, double selLength, double selSlopeDgree, double selOverlap = 0, double selRotate = 0, DrawCenterLineModel selCenterLine = null)
        {

            double OD = selOD;
            double Length = selLength - selOverlap;


            List<Entity> newList = new List<Entity>();

            // WP : Left Lower
            Point3D WP = new Point3D(selPoint1.X, selPoint1.Y);
            double centerMiddle = OD / 2;
            Point3D WPRotate = WP;


            // Drawing Shape
            Line line00 = null;
            Line line01 = null;

            line00 = new Line(GetSumPoint(WP, -OD * 100, 0), GetSumPoint(WP, +OD * 100, 0));
            Line lineTempVLeft = new Line(GetSumPoint(WP, -centerMiddle, +OD * 100), GetSumPoint(WP, -centerMiddle, -OD * 100));
            Line lineTempVRight = new Line(GetSumPoint(WP, centerMiddle, +OD * 100), GetSumPoint(WP, centerMiddle, -OD * 100));
            line00.Rotate(selSlopeDgree, Vector3D.AxisZ, line00.MidPoint);
            Point3D[] leftDownPoint = line00.IntersectWith(lineTempVLeft);
            Point3D[] rightDownPoint = line00.IntersectWith(lineTempVRight);



            line01 = new Line(GetSumPoint(WP, -centerMiddle, Length), GetSumPoint(WP, centerMiddle, Length));

            Line exLine00 = new Line(GetSumPoint(leftDownPoint[0], 0, 0), GetSumPoint(rightDownPoint[0], 0, 0));
            Line exLine01 = new Line(GetSumPoint(leftDownPoint[0], 0, 0), GetSumPoint(line01.StartPoint, 0, 0));
            Line exLine02 = new Line(GetSumPoint(rightDownPoint[0], 0, 0), GetSumPoint(line01.EndPoint, 0, 0));

            newList.AddRange(new Line[] { exLine00, exLine01, exLine02 });

            newList.Add(line00);
            newList.Add(line01);

            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            // Center Line
            if (selCenterLine != null)
            {
                if (selCenterLine.centerLine)
                {
                    // 기본 형상
                    Point3D zeroPoint = exLine00.MidPoint;
                    Point3D onePoint = line01.MidPoint;
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
                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }

            // Rotate After : Set Point : Translate
            if (selPointNumber >= 0)
            {
                if (selPointNumber == 0)
                    line00.EntityData = line00.MidPoint;
                else if (selPointNumber == 1)
                    line01.EntityData = line01.MidPoint;
            }

            // Translate
            if (selPointNumber > -1)
            {
                Point3D translatePoint = GetTranslatePoint(ref newList);
                if (translatePoint != null)
                    SetTranslate(ref newList, WP, translatePoint);

            }

            newList.Remove(line00);

            selOutputPointList = new List<Point3D>();
            selOutputPointList.Add(line00.MidPoint);
            selOutputPointList.Add(line01.MidPoint);

            return newList;
        }
        public List<Entity> DrawReference_Nozzle_PipeArc(out List<Point3D> selOutputPointList, Point3D selPoint1, int selPointNumber, double selOD, double selLength, double selSlopeDgree, double selOverlap = 0, double selRotate = 0, DrawCenterLineModel selCenterLine = null)
        {

            double OD = selOD;
            double Length = selLength - selOverlap;


            List<Entity> newList = new List<Entity>();

            // WP : Left Lower
            Point3D WP = new Point3D(selPoint1.X, selPoint1.Y );
            double centerMiddle = OD / 2;
            Point3D WPRotate = WP;


            // Drawing Shape
            Arc line00 = null;
            Line line01 = null;

            //line00 = new Line(GetSumPoint(WP, -OD * 100, 0), GetSumPoint(WP, +OD * 100, 0));
            Line lineTempVLeft = new Line(GetSumPoint(WP, -centerMiddle, +selLength * 100), GetSumPoint(WP, -centerMiddle, -selLength * 100));
            Line lineTempVRight = new Line(GetSumPoint(WP, centerMiddle, +selLength * 100), GetSumPoint(WP, centerMiddle, -selLength * 100));


            line00 = editingService.GetArcOfPoint(GetSumPoint(workingService.DRTWorkingData.RoofCenterPoint, 0, 0), workingService.DRTWorkingData.DomeRaidus, GetSumPoint(selPoint1, 0, 0), OD);

            Vector3D tempMovement = new Vector3D(0, selLength);
            line00.Translate(tempMovement);
             
            //line00.Rotate(selSlopeDgree, Vector3D.AxisZ, line00.MidPoint);




            Point3D[] leftDownPoint = line00.IntersectWith(lineTempVLeft);
            Point3D[] rightDownPoint = line00.IntersectWith(lineTempVRight);



            line01 = new Line(GetSumPoint(WP, -centerMiddle, Length), GetSumPoint(WP, centerMiddle, Length));

            Line exLine00 = new Line(GetSumPoint(leftDownPoint[0], 0, 0), GetSumPoint(rightDownPoint[0], 0, 0));
            Line exLine01 = new Line(GetSumPoint(leftDownPoint[0], 0, 0), GetSumPoint(line01.StartPoint, 0, 0));
            Line exLine02 = new Line(GetSumPoint(rightDownPoint[0], 0, 0), GetSumPoint(line01.EndPoint, 0, 0));

            newList.AddRange(new Line[] { exLine00, exLine01, exLine02 });

            newList.Add(line00);
            newList.Add(line01);

            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            // Center Line
            if (selCenterLine != null)
            {
                if (selCenterLine.centerLine)
                {
                    // 기본 형상
                    Point3D zeroPoint = exLine00.MidPoint;
                    Point3D onePoint = line01.MidPoint;
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
                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }

            // Rotate After : Set Point : Translate
            if (selPointNumber >= 0)
            {
                if (selPointNumber == 0)
                    line00.EntityData = line00.MidPoint;
                else if (selPointNumber == 1)
                    line01.EntityData = line01.MidPoint;
            }

            // Translate
            if (selPointNumber > -1)
            {
                Point3D translatePoint = GetTranslatePoint(ref newList);
                if (translatePoint != null)
                    SetTranslate(ref newList, WP, translatePoint);

            }

            newList.Remove(line00);

            selOutputPointList = new List<Point3D>();
            selOutputPointList.Add(line00.MidPoint);
            selOutputPointList.Add(line01.MidPoint);

            return newList;
        }







        public List<Entity> DrawReference_Nozzle_Neck(out List<Point3D> selOutputPointList, Point3D selPoint1, int selPointNumber, double selOD1, double selOD2, double selLength, double selRotate = 0, DrawCenterLineModel selCenterLine = null)
        {

            double OD1 = selOD1;
            double OD2 = selOD2;
            double Length = selLength;


            List<Entity> newList = new List<Entity>();

            // WP : Left Lower
            Point3D WP = new Point3D(selPoint1.X, selPoint1.Y);
            double centerMiddle1 = OD1 / 2;
            double centerMiddle2 = OD2 / 2;
            Point3D WPRotate = WP;


            // Drawing Shape
            Line line00 = null;
            Line line01 = null;

            line00 = new Line(GetSumPoint(WP, -centerMiddle1, 0), GetSumPoint(WP, +centerMiddle1, 0));
            line01 = new Line(GetSumPoint(WP, -centerMiddle2, Length), GetSumPoint(WP, +centerMiddle2, Length));

            Line exLine01 = new Line(GetSumPoint(line00.StartPoint, 0, 0), GetSumPoint(line01.StartPoint, 0, 0));
            Line exLine02 = new Line(GetSumPoint(line00.EndPoint, 0, 0), GetSumPoint(line01.EndPoint, 0, 0));

            newList.AddRange(new Line[] { exLine01, exLine02 });

            newList.Add(line00);
            newList.Add(line01);

            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            // Center Line
            if (selCenterLine != null)
            {
                if (selCenterLine.centerLine)
                {
                    // 기본 형상
                    Point3D zeroPoint = line00.MidPoint;
                    Point3D onePoint = line01.MidPoint;
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
                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }

            // Rotate After : Set Point : Translate
            if (selPointNumber >= 0)
            {
                if (selPointNumber == 0)
                    line00.EntityData = line00.MidPoint;
                else if (selPointNumber == 1)
                    line01.EntityData = line01.MidPoint;
            }

            // Translate
            if (selPointNumber > -1)
            {
                Point3D translatePoint = GetTranslatePoint(ref newList);
                if (translatePoint != null)
                    SetTranslate(ref newList, WP, translatePoint);

            }

            selOutputPointList = new List<Point3D>();
            selOutputPointList.Add(line00.MidPoint);
            selOutputPointList.Add(line01.MidPoint);

            return newList;
        }

        public List<Entity> DrawReference_Nozzle_Pad(out List<Point3D> selOutputPointList, Point3D selPoint1, int selPointNumber, double selOD1, double selOD2, double selLength, double selRotate = 0, DrawCenterLineModel selCenterLine = null)
        {

            double OD1 = selOD1;
            double OD2 = selOD2;
            double Length = selLength;


            List<Entity> newList = new List<Entity>();

            // WP : Left Lower
            Point3D WP = new Point3D(selPoint1.X, selPoint1.Y);
            double centerMiddle1 = OD1 / 2;
            double centerMiddle2 = OD2 / 2;
            Point3D WPRotate = WP;


            // Drawing Shape
            Line line00 = null;
            Line line01 = null;
            Line line02 = null;
            Line line03 = null;

            line00 = new Line(GetSumPoint(WP, -centerMiddle1, 0), GetSumPoint(WP, +centerMiddle1, 0));
            line01 = new Line(GetSumPoint(WP, -centerMiddle1, Length), GetSumPoint(WP, +centerMiddle1, Length));

            line02 = new Line(GetSumPoint(WP, -centerMiddle2, 0), GetSumPoint(WP, +centerMiddle2, 0));
            line03 = new Line(GetSumPoint(WP, -centerMiddle2, Length), GetSumPoint(WP, +centerMiddle2, Length));

            // vertical
            Line exLine01 = new Line(GetSumPoint(line00.StartPoint, 0, 0), GetSumPoint(line01.StartPoint, 0, 0));
            Line exLine02 = new Line(GetSumPoint(line00.EndPoint, 0, 0), GetSumPoint(line01.EndPoint, 0, 0));
            Line exLine03 = new Line(GetSumPoint(line02.StartPoint, 0, 0), GetSumPoint(line03.StartPoint, 0, 0));
            Line exLine04 = new Line(GetSumPoint(line02.EndPoint, 0, 0), GetSumPoint(line03.EndPoint, 0, 0));

            // horizontal
            Line exLine05 = new Line(GetSumPoint(line00.StartPoint, 0, 0), GetSumPoint(line02.StartPoint, 0, 0));
            Line exLine06 = new Line(GetSumPoint(line00.EndPoint, 0, 0), GetSumPoint(line02.EndPoint, 0, 0));
            Line exLine07 = new Line(GetSumPoint(line01.StartPoint, 0, 0), GetSumPoint(line03.StartPoint, 0, 0));
            Line exLine08 = new Line(GetSumPoint(line01.EndPoint, 0, 0), GetSumPoint(line03.EndPoint, 0, 0));

            newList.AddRange(new Line[] { exLine01, exLine02, exLine03, exLine04, exLine05, exLine06, exLine07, exLine08 });

            newList.Add(line00);
            newList.Add(line01);
            newList.Add(line02);
            newList.Add(line03);

            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            // Center Line
            if (selCenterLine != null)
            {
                if (selCenterLine.centerLine)
                {
                    // 기본 형상
                    Point3D zeroPoint = line00.MidPoint;
                    Point3D onePoint = line01.MidPoint;
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
                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }

            // Rotate After : Set Point : Translate
            if (selPointNumber >= 0)
            {
                if (selPointNumber == 0)
                    line00.EntityData = line00.MidPoint;
                else if (selPointNumber == 1)
                    line01.EntityData = line01.MidPoint;
            }


            // Translate
            if (selPointNumber > -1)
            {
                Point3D translatePoint = GetTranslatePoint(ref newList);
                if (translatePoint != null)
                    SetTranslate(ref newList, WP, translatePoint);

            }

            newList.Remove(line00);
            newList.Remove(line01);
            newList.Remove(line02);
            newList.Remove(line03);

            selOutputPointList = new List<Point3D>();
            selOutputPointList.Add(line00.MidPoint);
            selOutputPointList.Add(line01.MidPoint);

            return newList;
        }

        public List<Entity> DrawReference_Nozzle_PadSlope(out List<Point3D> selOutputPointList, Point3D selPoint1, int selPointNumber, double selOD1, double selOD2, double selLength, bool refBottom = true, double selRotate = 0, DrawCenterLineModel selCenterLine = null)
        {

            double OD1 = selOD1;
            double OD2 = selOD2;
            double Length = selLength;


            List<Entity> newList = new List<Entity>();

            // WP : Left Lower
            Point3D WP = new Point3D(selPoint1.X, selPoint1.Y);
            double centerMiddle1 = OD1 / 2;
            double centerMiddle2 = OD2 / 2;
            Point3D WPRotate = WP;
            if (!refBottom)
                WPRotate = GetSumPoint(WP, 0, Length);


            // Drawing Shape
            Line lineLeftV = null;
            Line lineRightV = null;
            Line lineCenterV = null;
            Line lineBottom = null;
            Line lineTop = null;

            lineLeftV = new Line(GetSumPoint(WP, -centerMiddle1, -selOD2 * 100), GetSumPoint(WP, -centerMiddle1, selOD2 * 100));
            lineRightV = new Line(GetSumPoint(WP, +centerMiddle1, -selOD2 * 100), GetSumPoint(WP, +centerMiddle1, selOD2 * 100));
            lineCenterV = new Line(GetSumPoint(WP, 0, -Length * 2), GetSumPoint(WP, 0, Length * 2));

            lineBottom = new Line(GetSumPoint(WP, -centerMiddle2, 0), GetSumPoint(WP, +centerMiddle2, 0));
            lineTop = new Line(GetSumPoint(WP, -centerMiddle2, Length), GetSumPoint(WP, +centerMiddle2, Length));

            // vertical
            Line exLine03 = new Line(GetSumPoint(lineTop.StartPoint, 0, 0), GetSumPoint(lineBottom.StartPoint, 0, 0));
            Line exLine04 = new Line(GetSumPoint(lineTop.EndPoint, 0, 0), GetSumPoint(lineBottom.EndPoint, 0, 0));


            newList.AddRange(new Line[] { exLine03, exLine04 });

            //newList.Add(line00);
            //newList.Add(line01);
            newList.Add(lineTop);
            newList.Add(lineBottom);

            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);


            // Rotate
            if (selRotate != 0)
            {
                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }





            // 재조절
            if (true)
            {
                Point3D[] pointTopLeft = lineLeftV.IntersectWith(lineTop);
                Point3D[] pointTopRight = lineRightV.IntersectWith(lineTop);
                Point3D[] pointBottomLeft = lineLeftV.IntersectWith(lineBottom);
                Point3D[] pointBottomRight = lineRightV.IntersectWith(lineBottom);

                Line newLine01 = new Line(GetSumPoint(lineTop.StartPoint, 0, 0), GetSumPoint(pointTopLeft[0], 0, 0));
                Line newLine02 = new Line(GetSumPoint(lineTop.EndPoint, 0, 0), GetSumPoint(pointTopRight[0], 0, 0));
                Line newLine03 = new Line(GetSumPoint(lineBottom.StartPoint, 0, 0), GetSumPoint(pointBottomLeft[0], 0, 0));
                Line newLine04 = new Line(GetSumPoint(lineBottom.EndPoint, 0, 0), GetSumPoint(pointBottomRight[0], 0, 0));
                Line newLine05 = new Line(pointTopLeft[0], pointBottomLeft[0]);
                Line newLine06 = new Line(pointTopRight[0], pointBottomRight[0]);

                styleService.SetLayer(ref newLine01, layerService.LayerOutLine);
                styleService.SetLayer(ref newLine02, layerService.LayerOutLine);
                styleService.SetLayer(ref newLine03, layerService.LayerOutLine);
                styleService.SetLayer(ref newLine04, layerService.LayerOutLine);
                styleService.SetLayer(ref newLine05, layerService.LayerOutLine);
                styleService.SetLayer(ref newLine06, layerService.LayerOutLine);
                newList.AddRange(new Line[] { newLine01, newLine02, newLine03, newLine04, newLine05, newLine06 });
            }


            Point3D[] pointTopCenter = lineCenterV.IntersectWith(lineTop);
            Point3D[] pointBottomCenter = lineCenterV.IntersectWith(lineBottom);

            // Center Line
            if (selCenterLine != null)
            {
                if (selCenterLine.centerLine)
                {

                    // 기본 형상
                    Point3D zeroPoint = pointBottomCenter[0];
                    Point3D onePoint = pointTopCenter[0];
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




            // Translate
            if (selPointNumber > -1)
            {
                Point3D translatePoint = null;
                if (selPointNumber >= 0)
                {
                    if (selPointNumber == 0)
                        translatePoint = pointBottomCenter[0];
                    if (selPointNumber == 1)
                        translatePoint = pointTopCenter[0];

                    if (translatePoint != null)
                        SetTranslate(ref newList, WP, translatePoint);
                }

            }


            newList.Remove(lineTop);
            newList.Remove(lineBottom);

            selOutputPointList = new List<Point3D>();
            selOutputPointList.Add(pointBottomCenter[0]);
            selOutputPointList.Add(pointTopCenter[0]);

            return newList;
        }

        public List<Entity> DrawReference_Nozzle_PadSlopeFlange(out List<Point3D> selOutputPointList, Point3D selPoint1, int selPointNumber, double selOD1, double selOD2, double selLength, bool refBottom = true, double selRotate = 0, DrawCenterLineModel selCenterLine = null)
        {

            double OD1 = selOD1;
            double OD2 = selOD2;
            double Length = selLength;


            List<Entity> newList = new List<Entity>();

            // WP : Left Lower
            Point3D WP = new Point3D(selPoint1.X, selPoint1.Y);
            double centerMiddle1 = OD1 / 2;
            double centerMiddle2 = OD2 / 2;
            Point3D WPRotate = WP;
            if (!refBottom)
                WPRotate = GetSumPoint(WP, 0, Length);


            // Drawing Shape
            Line lineLeftV = null;
            Line lineRightV = null;
            Line lineCenterV = null;
            Line lineBottom = null;
            Line lineTop = null;

            lineLeftV = new Line(GetSumPoint(WP, -centerMiddle1, -selOD2 * 100), GetSumPoint(WP, -centerMiddle1, selOD2 * 100));
            lineRightV = new Line(GetSumPoint(WP, +centerMiddle1, -selOD2 * 100), GetSumPoint(WP, +centerMiddle1, selOD2 * 100));
            lineCenterV = new Line(GetSumPoint(WP, 0, -Length * 2), GetSumPoint(WP, 0, Length * 2));

            lineBottom = new Line(GetSumPoint(WP, -centerMiddle2, 0), GetSumPoint(WP, +centerMiddle2, 0));
            lineTop = new Line(GetSumPoint(WP, -centerMiddle2, Length), GetSumPoint(WP, +centerMiddle2, Length));

            styleService.SetLayer(ref lineBottom, layerService.LayerOutLine);
            styleService.SetLayer(ref lineTop, layerService.LayerOutLine);

            // vertical
            Line exLine03 = new Line(GetSumPoint(lineTop.StartPoint, 0, 0), GetSumPoint(lineBottom.StartPoint, 0, 0));
            Line exLine04 = new Line(GetSumPoint(lineTop.EndPoint, 0, 0), GetSumPoint(lineBottom.EndPoint, 0, 0));


            newList.AddRange(new Line[] { exLine03, exLine04 });

            //newList.Add(line00);
            //newList.Add(line01);
            newList.Add(lineTop);
            newList.Add(lineBottom);

            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);


            // Rotate
            if (selRotate != 0)
            {
                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }





            // 재조절
            if (true)
            {
                Point3D[] pointTopLeft = lineLeftV.IntersectWith(lineTop);
                Point3D[] pointTopRight = lineRightV.IntersectWith(lineTop);
                Point3D[] pointBottomLeft = lineLeftV.IntersectWith(lineBottom);
                Point3D[] pointBottomRight = lineRightV.IntersectWith(lineBottom);

                Line newLine01 = new Line(GetSumPoint(lineTop.StartPoint, 0, 0), GetSumPoint(lineTop.EndPoint, 0, 0));
                //Line newLine02 = new Line(GetSumPoint(lineTop.EndPoint, 0, 0), GetSumPoint(pointTopRight[0], 0, 0));
                Line newLine03 = new Line(GetSumPoint(lineBottom.StartPoint, 0, 0), GetSumPoint(lineBottom.EndPoint, 0, 0));
                //Line newLine04 = new Line(GetSumPoint(lineBottom.EndPoint, 0, 0), GetSumPoint(pointBottomRight[0], 0, 0));
                Line newLine05 = new Line(pointTopLeft[0], pointBottomLeft[0]);
                Line newLine06 = new Line(pointTopRight[0], pointBottomRight[0]);

                styleService.SetLayer(ref newLine01, layerService.LayerOutLine);
                //styleService.SetLayer(ref newLine02, layerService.LayerOutLine);
                styleService.SetLayer(ref newLine03, layerService.LayerOutLine);
                //styleService.SetLayer(ref newLine04, layerService.LayerOutLine);
                styleService.SetLayer(ref newLine05, layerService.LayerOutLine);
                styleService.SetLayer(ref newLine06, layerService.LayerOutLine);
                newList.AddRange(new Line[] { newLine01,  newLine03,  newLine05, newLine06 });
            }


            Point3D[] pointTopCenter = lineCenterV.IntersectWith(lineTop);
            Point3D[] pointBottomCenter = lineCenterV.IntersectWith(lineBottom);

            // Center Line
            if (selCenterLine != null)
            {
                if (selCenterLine.centerLine)
                {

                    // 기본 형상
                    Point3D zeroPoint = pointBottomCenter[0];
                    Point3D onePoint = pointTopCenter[0];
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




            // Translate
            if (selPointNumber > -1)
            {
                Point3D translatePoint = null;
                if (selPointNumber >= 0)
                {
                    if (selPointNumber == 0)
                        translatePoint = pointBottomCenter[0];
                    if (selPointNumber == 1)
                        translatePoint = pointTopCenter[0];

                    if (translatePoint != null)
                        SetTranslate(ref newList, WP, translatePoint);
                }

            }


            newList.Remove(lineTop);
            newList.Remove(lineBottom);

            selOutputPointList = new List<Point3D>();
            selOutputPointList.Add(pointBottomCenter[0]);
            selOutputPointList.Add(pointTopCenter[0]);

            return newList;
        }


        public List<Entity> DrawReference_Nozzle_BlindFlange(out List<Point3D> selOutputPointList, Point3D selPoint1, int selPointNumber, double selOD1, double selOD2, double selLength, double selRotate = 0, DrawCenterLineModel selCenterLine = null)
        {

            double OD1 = selOD1;
            double OD2 = selOD2;
            double Length = selLength;


            List<Entity> newList = new List<Entity>();

            // WP : Left Lower
            Point3D WP = new Point3D(selPoint1.X, selPoint1.Y);
            double centerMiddle1 = OD1 / 2;
            double centerMiddle2 = OD2 / 2;
            Point3D WPRotate = WP;


            // Drawing Shape
            Line line00 = null;
            Line line01 = null;
            Line line02 = null;
            Line line03 = null;

            line00 = new Line(GetSumPoint(WP, -centerMiddle1, 0), GetSumPoint(WP, +centerMiddle1, 0));
            line01 = new Line(GetSumPoint(WP, -centerMiddle1, Length), GetSumPoint(WP, +centerMiddle1, Length));

            line02 = new Line(GetSumPoint(WP, -centerMiddle2, 0), GetSumPoint(WP, +centerMiddle2, 0));
            line03 = new Line(GetSumPoint(WP, -centerMiddle2, Length), GetSumPoint(WP, +centerMiddle2, Length));

            // vertical
            Line exLine01 = new Line(GetSumPoint(line00.StartPoint, 0, 0), GetSumPoint(line01.StartPoint, 0, 0));
            Line exLine02 = new Line(GetSumPoint(line00.EndPoint, 0, 0), GetSumPoint(line01.EndPoint, 0, 0));
            Line exLine03 = new Line(GetSumPoint(line02.StartPoint, 0, 0), GetSumPoint(line03.StartPoint, 0, 0));
            Line exLine04 = new Line(GetSumPoint(line02.EndPoint, 0, 0), GetSumPoint(line03.EndPoint, 0, 0));

            // horizontal
            //Line exLine05 = new Line(GetSumPoint(line00.StartPoint, 0, 0), GetSumPoint(line02.StartPoint, 0, 0));
            //Line exLine06 = new Line(GetSumPoint(line00.EndPoint, 0, 0), GetSumPoint(line02.EndPoint, 0, 0));
            //Line exLine07 = new Line(GetSumPoint(line02.StartPoint, 0, 0), GetSumPoint(line03.StartPoint, 0, 0));
            //Line exLine08 = new Line(GetSumPoint(line02.EndPoint, 0, 0), GetSumPoint(line03.EndPoint, 0, 0));

            newList.AddRange(new Line[] { exLine01, exLine02, exLine03, exLine04 });

            //newList.Add(line00);
            //newList.Add(line01);
            newList.Add(line02);
            newList.Add(line03);

            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            // Center Line
            if (selCenterLine != null)
            {
                if (selCenterLine.centerLine)
                {
                    // 기본 형상
                    Point3D zeroPoint = line00.MidPoint;
                    Point3D onePoint = line01.MidPoint;
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
                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }

            // Translate
            if (selPointNumber > -1)
            {
                Point3D translatePoint = null;
                if (selPointNumber >= 0)
                {
                    if (selPointNumber == 0)
                        translatePoint = line02.MidPoint;
                    else if (selPointNumber == 1)
                        translatePoint = line03.MidPoint;
                }
                if (translatePoint != null)
                    SetTranslate(ref newList, WP, translatePoint);

            }

            //newList.Remove(line00);
            //newList.Remove(line01);
            //newList.Remove(line02);
            //newList.Remove(line03);

            selOutputPointList = new List<Point3D>();
            selOutputPointList.Add(line02.MidPoint);
            selOutputPointList.Add(line03.MidPoint);

            return newList;
        }

        public List<Entity> DrawReference_Nozzle_Flange(out List<Point3D> selOutputPointList, Point3D selPoint1, int selPointNumber, double selOD1, double selOD2, double selLength, double selRotate = 0, DrawCenterLineModel selCenterLine = null)
        {

            double OD1 = selOD1;    // Small
            double OD2 = selOD2;    // Large
            double Length = selLength;


            List<Entity> newList = new List<Entity>();

            // WP : Left Lower
            Point3D WP = new Point3D(selPoint1.X, selPoint1.Y);
            double centerMiddle1 = OD1 / 2;
            double centerMiddle2 = OD2 / 2;
            Point3D WPRotate = WP;


            // Drawing Shape
            Line line00 = null;
            Line line01 = null;
            Line line02 = null;
            Line line03 = null;

            line00 = new Line(GetSumPoint(WP, -centerMiddle1, 0), GetSumPoint(WP, +centerMiddle1, 0));
            line01 = new Line(GetSumPoint(WP, -centerMiddle1, Length), GetSumPoint(WP, +centerMiddle1, Length));

            line02 = new Line(GetSumPoint(WP, -centerMiddle2, 0), GetSumPoint(WP, +centerMiddle2, 0));
            line03 = new Line(GetSumPoint(WP, -centerMiddle2, Length), GetSumPoint(WP, +centerMiddle2, Length));

            // vertical
            //Line exLine01 = new Line(GetSumPoint(line00.StartPoint, 0, 0), GetSumPoint(line01.StartPoint, 0, 0));
            //Line exLine02 = new Line(GetSumPoint(line00.EndPoint, 0, 0), GetSumPoint(line01.EndPoint, 0, 0));
            Line exLine03 = new Line(GetSumPoint(line02.StartPoint, 0, 0), GetSumPoint(line03.StartPoint, 0, 0));
            Line exLine04 = new Line(GetSumPoint(line02.EndPoint, 0, 0), GetSumPoint(line03.EndPoint, 0, 0));

            // horizontal
            //Line exLine05 = new Line(GetSumPoint(line00.StartPoint, 0, 0), GetSumPoint(line02.StartPoint, 0, 0));
            //Line exLine06 = new Line(GetSumPoint(line00.EndPoint, 0, 0), GetSumPoint(line02.EndPoint, 0, 0));
            //Line exLine07 = new Line(GetSumPoint(line02.StartPoint, 0, 0), GetSumPoint(line03.StartPoint, 0, 0));
            //Line exLine08 = new Line(GetSumPoint(line02.EndPoint, 0, 0), GetSumPoint(line03.EndPoint, 0, 0));

            newList.AddRange(new Line[] { exLine03, exLine04 });

            newList.Add(line00);
            newList.Add(line01);
            newList.Add(line02);
            newList.Add(line03);

            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            // Center Line
            if (selCenterLine != null)
            {
                if (selCenterLine.centerLine)
                {
                    // 기본 형상
                    Point3D zeroPoint = line00.MidPoint;
                    Point3D onePoint = line01.MidPoint;
                    Line centerLine00 = new Line(zeroPoint, onePoint);

                    Line centerLine01 = new Line(GetSumPoint(line00.StartPoint, 0, 0), GetSumPoint(line01.StartPoint, 0, 0));
                    Line centerLine02 = new Line(GetSumPoint(line00.EndPoint, 0, 0), GetSumPoint(line01.EndPoint, 0, 0));

                    styleService.SetLayer(ref centerLine00, layerService.LayerCenterLine);
                    styleService.SetLayer(ref centerLine01, layerService.LayerCenterLine);
                    styleService.SetLayer(ref centerLine02, layerService.LayerCenterLine);

                    newList.Add(centerLine00);
                    newList.Add(centerLine01);
                    newList.Add(centerLine02);

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

                    // 3 : 양쪽 다
                    if (selCenterLine.twoEx)
                    {
                        Line oneLine01 = new Line(GetSumPoint(line01.StartPoint, 0, 0), GetSumPoint(line01.StartPoint, 0, +selCenterLine.exLength * selCenterLine.scaleValue));
                        Line oneLine02 = new Line(GetSumPoint(line01.EndPoint, 0, 0), GetSumPoint(line01.EndPoint, 0, +selCenterLine.exLength * selCenterLine.scaleValue));
                        Line oneLine03 = new Line(GetSumPoint(line00.StartPoint, 0, 0), GetSumPoint(line00.StartPoint, 0, -selCenterLine.exLength * selCenterLine.scaleValue));
                        Line oneLine04 = new Line(GetSumPoint(line00.EndPoint, 0, 0), GetSumPoint(line00.EndPoint, 0, -selCenterLine.exLength * selCenterLine.scaleValue));
                        styleService.SetLayer(ref oneLine01, layerService.LayerCenterLine);
                        styleService.SetLayer(ref oneLine02, layerService.LayerCenterLine);
                        styleService.SetLayer(ref oneLine03, layerService.LayerCenterLine);
                        styleService.SetLayer(ref oneLine04, layerService.LayerCenterLine);
                        newList.Add(oneLine01);
                        newList.Add(oneLine02);
                        newList.Add(oneLine03);
                        newList.Add(oneLine04);
                    }

                }
            }

            // Rotate
            if (selRotate != 0)
            {
                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }

            // Translate
            if (selPointNumber > -1)
            {
                Point3D translatePoint = null;
                if (selPointNumber >= 0)
                {
                    if (selPointNumber == 0)
                        translatePoint = line02.MidPoint;
                    else if (selPointNumber == 1)
                        translatePoint = line03.MidPoint;
                }
                if (translatePoint != null)
                    SetTranslate(ref newList, WP, translatePoint);

            }

            newList.Remove(line00);
            newList.Remove(line01);
            //newList.Remove(line02);
            //newList.Remove(line03);

            selOutputPointList = new List<Point3D>();
            selOutputPointList.Add(line00.MidPoint);
            selOutputPointList.Add(line01.MidPoint);

            return newList;
        }


        public List<Entity> DrawReference_Hole(Point3D selPoint1, double selRadius,  double selRotate = 0, DrawCenterLineModel selCenterLine = null)
        {

            

            List<Entity> newList = new List<Entity>();

            // WP : Left Lower
            Point3D WP = new Point3D(selPoint1.X, selPoint1.Y);
            Point3D WPRotate = WP;


            // Drawing Shape
            Circle newCir01 = new Circle(GetSumPoint(WP, 0, 0), selRadius);

            newList.Add(newCir01);
            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            // Center Line
            if (selCenterLine != null)
            {
                if (selCenterLine.centerLine)
                {
                    // 기본 형상
                    Point3D centerPoint = newCir01.Center;
                    Line centerLine01 = new Line(GetSumPoint(centerPoint, -selRadius - selCenterLine.exLength * selCenterLine.scaleValue, 0), 
                                                 GetSumPoint(centerPoint, +selRadius + selCenterLine.exLength * selCenterLine.scaleValue, 0));
                    Line centerLine02 = new Line(GetSumPoint(centerPoint,0, -selRadius - selCenterLine.exLength * selCenterLine.scaleValue),
                                                 GetSumPoint(centerPoint,0, +selRadius + selCenterLine.exLength * selCenterLine.scaleValue));

                    styleService.SetLayer(ref centerLine01, layerService.LayerCenterLine);
                    styleService.SetLayer(ref centerLine02, layerService.LayerCenterLine);


                    newList.Add(centerLine01);
                    newList.Add(centerLine02);

                }
            }

            // Rotate
            if (selRotate != 0)
            {
                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }

            return newList;
        }



        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
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

    }
}
