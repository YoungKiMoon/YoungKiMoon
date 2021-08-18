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
using DrawWork.ValueServices;

namespace DrawWork.DrawServices
{
    public class DrawEditingService
    {
        ValueService valueService;
        public DrawEditingService()
        {
            valueService = new ValueService();
        }


        #region Extend
        public  Line GetExtendLine(Line selLine, double Length, bool endPoint = true)
        {
            if (endPoint)
            {
                Line tempLine = new Line(selLine.StartPoint, selLine.EndPoint);
                Vector3D direction = new Vector3D(selLine.StartPoint, selLine.EndPoint);

                direction.Normalize();
                tempLine.EndPoint = tempLine.EndPoint + direction * Length;

                return tempLine;
            }
            else
            {
                Line tempLine = new Line(selLine.StartPoint, selLine.EndPoint);
                Vector3D direction = new Vector3D(selLine.EndPoint, selLine.StartPoint);

                direction.Normalize();
                tempLine.StartPoint = tempLine.StartPoint + direction * Length;

                return tempLine;
            }
        }
        public void SetExtendLine(ref Line selLine, double Length, bool endPoint = true)
        {
            if (endPoint)
            {

                Vector3D direction = new Vector3D(selLine.StartPoint, selLine.EndPoint);

                direction.Normalize();
                selLine.EndPoint = selLine.EndPoint + direction * Length;

            }
            else
            {
                Vector3D direction = new Vector3D(selLine.EndPoint, selLine.StartPoint);

                direction.Normalize();
                selLine.StartPoint = selLine.StartPoint + direction * Length;

            }
        }

        //public Arc GetExtendArc(Arc selArc, double Length, bool endPoint = true)
        //{
        //    if (endPoint)
        //    {
        //        Vector3D xAxis = new Vector3D(selArc.Center, selArc.EndPoint);
        //        xAxis.Normalize();
        //        Vector3D yAxis = Vector3D.Cross(Vector3D.AxisZ, xAxis);
        //        yAxis.Normalize();
        //        Plane arcPlane = new Plane(selArc.Center, xAxis, yAxis);

        //        Vector2D v1 = new Vector2D(selArc.Center, selArc.EndPoint);
        //        v1.Normalize();
        //        Vector2D v2 = new Vector2D(selArc.Center, intPoint);
        //        v2.Normalize();

        //        Arc newArc = new Arc(arcPlane, arcPlane.Origin, selArc.Radius, 0, arcSpan);

        //        return newArc;
        //    }
        //    else
        //    {
        //        Line tempLine = new Line(selLine.StartPoint, selLine.EndPoint);
        //        Vector3D direction = new Vector3D(selLine.EndPoint, selLine.StartPoint);

        //        direction.Normalize();
        //        tempLine.StartPoint = tempLine.StartPoint + direction * Length;

        //        return tempLine;
        //    }
        //}

        #endregion

        #region Mirror
        public List<Entity> GetMirrorEntity(Plane selPlane, List<Entity> selList, double X, double Y, bool copyCmd = false)
        {
            Plane pl1 = selPlane;
            pl1.Origin.X = X;
            pl1.Origin.Y = Y;
            Mirror customMirror = new Mirror(pl1);
            List<Entity> mirrorList = new List<Entity>();
            if (copyCmd)
            {
                foreach (Entity eachEntity in selList)
                {
                    Entity newEntity = (Entity)eachEntity.Clone();
                    newEntity.TransformBy(customMirror);
                    mirrorList.Add(newEntity);
                }
            }
            else
            {
                foreach (Entity eachEntity in selList)
                {
                    eachEntity.TransformBy(customMirror);
                }
            }


            return mirrorList;
        }

        public List<Entity> GetEntityByMirror(Plane selPlane, List<Entity> selEntity)
        {
            List<Entity> newEntity = new List<Entity>();
            Mirror customMirror = new Mirror(selPlane);
            foreach (Entity eachEntity in selEntity)
            {
                Entity newEachEntity = (Entity)eachEntity.Clone();
                newEachEntity.TransformBy(customMirror);
                newEntity.Add(newEachEntity);
            }
            return newEntity;
        }
        public void SetMirrorEntity(Plane selPlane, ref Entity selEntity, Point3D selPoint)
        {
            Plane pl1 = selPlane;
            pl1.Origin.X = selPoint.X;
            pl1.Origin.Y = selPoint.Y;
            Mirror customMirror = new Mirror(pl1);
            selEntity.TransformBy(customMirror);
        }
        public void SetMirrorEntity(Plane selPlane, ref Triangle selEntity, Point3D selPoint)
        {
            Plane pl1 = selPlane;
            pl1.Origin.X = selPoint.X;
            pl1.Origin.Y = selPoint.Y;
            Mirror customMirror = new Mirror(pl1);
            selEntity.TransformBy(customMirror);
        }
        #endregion

        #region Intersect
        public Point3D GetIntersectLength(ICurve selCurve,double selLength,bool endPoint=true)
        {
            Point3D newValue = new Point3D();
            if (endPoint)
            {
                newValue = new Point3D(selCurve.EndPoint.X, selCurve.EndPoint.Y);
                Circle lengthCircle = new Circle(selCurve.EndPoint ,selLength);
                Point3D[] interPoint = lengthCircle.IntersectWith(selCurve);
                if (interPoint.Length > 0)
                    newValue = new Point3D( interPoint[0].X, interPoint[0].Y);
                return newValue;
            }
            else
            {
                newValue = new Point3D(selCurve.StartPoint.X, selCurve.StartPoint.Y);
                Circle lengthCircle = new Circle(selCurve.StartPoint, selLength);
                Point3D[] interPoint = lengthCircle.IntersectWith(selCurve);
                if (interPoint.Length > 0)
                    newValue = new Point3D(interPoint[0].X, interPoint[0].Y);
                return newValue;
            }

        }
        public Point3D GetIntersectWidth(ICurve selCurve, ICurve selCurve2,int selNumber)
        {
            Point3D newValue = new Point3D();
            Point3D[] interPoint = selCurve.IntersectWith(selCurve2);
            if (interPoint.Length > selNumber)
                newValue = new Point3D(interPoint[selNumber].X, interPoint[selNumber].Y);
            return newValue;

        }
        #endregion

        #region Angle
        public double GetAngleOfLine(ICurve selCurve)
        {
            Vector3D direction = new Vector3D(selCurve.StartPoint, selCurve.EndPoint);
            direction.Normalize();
            return direction.Angle;
        }
        #endregion


        #region Translate
        public void SetTranslate(ref List<Entity> selList, Point3D refPoint, Point3D currentPoint)
        {
            double distanceY = refPoint.Y - currentPoint.Y;
            double distanceX = refPoint.X - currentPoint.X;
            Vector3D tempMovement = new Vector3D(distanceX, distanceY);
            foreach (Entity eachEntity in selList)
                eachEntity.Translate(tempMovement);
        }
        #endregion

        #region Rotate
        public List<Entity> GetRotate(List<Entity> selList,Point3D selCenterPoint, double selRadian)
        {
            List<Entity> newList = new List<Entity>();
            foreach(Entity eachEntity in selList)
            {
                Entity tempEachEntity = (Entity)eachEntity.Clone();
                tempEachEntity.Rotate(selRadian, Vector3D.AxisZ, selCenterPoint);
                newList.Add(tempEachEntity);
            }
            return newList;
        }
        #endregion


        //
        public Arc GetArcOfPoint(Point3D centerPoint,double selRadius, Point3D selPoint, double selWidth)
        {
            Circle cccc = new Circle(centerPoint, selRadius);

            double outerWidth = 10;

            double selPositionX = selPoint.X - centerPoint.X;
            double selWidthHalf = selWidth / 2 + outerWidth;

            double padHeight = 0;

            Line vLine01 = new Line(GetSumPoint(centerPoint, selPositionX, +selRadius*2), GetSumPoint(centerPoint, selPositionX, -selRadius*2));
            Line vLineleft = (Line)vLine01.Offset(selWidthHalf, Vector3D.AxisZ, 0.01, true);
            Line vLineRight = (Line)vLine01.Offset(-selWidthHalf, Vector3D.AxisZ, 0.01, true);


            Point3D[] vInter = cccc.IntersectWith(vLine01);
            Point3D[] vInterLeft = cccc.IntersectWith(vLineleft);
            Point3D[] vInterRight = cccc.IntersectWith(vLineRight);


            Arc newArc = null;
            if(vInterLeft.Length>0 && vInterRight.Length>0)
                newArc = new Arc(centerPoint, vInterLeft[0], vInterRight[0]);

            return newArc;

        }


        #region Center Line
        public List<Entity> GetCenterLine(Point3D selStartPoint, Point3D selEndPoint, double selExtLength,double selScaleValue)
        {
            List<Entity> newList = new List<Entity>();
            Line newLine = new Line(GetSumPoint(selStartPoint,0,0), GetSumPoint(selEndPoint,0,0));

            Vector3D direction = new Vector3D(newLine.StartPoint, newLine.EndPoint);
            direction.Normalize();
            Line extStartLine = new Line(selStartPoint, selStartPoint - (direction*selExtLength*selScaleValue));
            Line extEndLine = new Line(selEndPoint, selEndPoint + (direction * selExtLength * selScaleValue));
            newList.Add(newLine);
            newList.Add(extStartLine);
            newList.Add(extEndLine);
            return newList;
        }
        #endregion


        

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
    }
}
