using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawWork.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawShapes
{
    public class DrawBreakSymbols
    {
        public DrawBreakSymbols() 
        { 
        }

        public List<Entity> GetSLine(Point3D selPoint, double selWidth, bool selMirror = false, double selRotate = 0, ANGLE_TYPE selAngleType = ANGLE_TYPE.DEGREE, double selfactor = 0)
        {
            List<Entity> newList = new List<Entity>();

            if (selWidth > 0)
            {
                double arcHeight = selWidth * 0.2;
                double selWidthHalf = selWidth / 2;
                if (selMirror)
                    arcHeight = -arcHeight;

                Arc leftArc = new Arc(GetSumPoint(selPoint, -selWidth - selfactor, 0), GetSumPoint(selPoint, -selWidthHalf, arcHeight), GetSumPoint(selPoint, -selfactor, 0), false);
                Arc rightArc = new Arc(GetSumPoint(selPoint, -selfactor, 0), GetSumPoint(selPoint, selWidthHalf, -arcHeight), GetSumPoint(selPoint, selWidth + selfactor, 0), false);
                if (selRotate != 0)
                {
                    //selRotation
                    if (selAngleType == ANGLE_TYPE.DEGREE)
                        selRotate = Utility.DegToRad(selRotate);

                    leftArc.Rotate(selRotate, Vector3D.AxisZ, selPoint);
                    rightArc.Rotate(selRotate, Vector3D.AxisZ, selPoint);
                }

                newList.Add(leftArc);
                newList.Add(rightArc);
            }
            return newList;
        }


        public List<Entity> GetSDoubleLine(Point3D selPoint, double selWidth, double selScale = 1, double selfactor = 0, bool selMirror = false, double selRotate = 0, ANGLE_TYPE selAngleType = ANGLE_TYPE.DEGREE)
        {
            List<Entity> newList = new List<Entity>();

            if (selWidth > 0)
            {
                double distanceFactor = 3;
                double lineDistance = distanceFactor * selScale;
                double lineDistanceHalf = lineDistance / 2;

                Point3D upperPoint = GetSumPoint(selPoint, 0, lineDistanceHalf);
                Point3D lowerPoint = GetSumPoint(selPoint, 0, -lineDistanceHalf);
                List<Entity> upperLineList = GetSLine(upperPoint, selWidth, selMirror, selRotate, selAngleType, selfactor);
                List<Entity> lowerLineList = GetSLine(lowerPoint, selWidth, selMirror, selRotate, selAngleType, selfactor);
                newList.AddRange(upperLineList);
                newList.AddRange(lowerLineList);
            }
            return newList;
        }

        public void SetTrimSLine(ref List<Entity> selEntityList, ref List<Entity> selSLineList, Point3D selCenterPoint, bool farSide = true)
        {
            if (selEntityList.Count > 0)
            {
                if (selSLineList.Count > 0)
                {
                    foreach (ICurve eachEntity in selEntityList)
                    {
                        foreach (ICurve eachS in selSLineList)
                        {
                            Point3D[] eachInter = eachEntity.IntersectWith(eachS);
                            if (eachInter.Length > 0)
                            {
                                if (selCenterPoint.DistanceTo(eachEntity.StartPoint) < selCenterPoint.DistanceTo(eachEntity.EndPoint))
                                {
                                    eachEntity.TrimBy(eachInter[0], farSide ? false : true);
                                }
                                else
                                {
                                    eachEntity.TrimBy(eachInter[0], farSide ? true : false);
                                }
                            }
                        }
                    }
                }
            }
        }


        public List<Entity> GetTrimSDoubleLine(List<Entity> selEntityList, List<Entity> selSLineList, bool centerLineBreak=false )
        {
            List<Entity> newList = new List<Entity>();

            if (selEntityList.Count > 0)
            {
                if (selSLineList.Count > 0)
                {
                    foreach (ICurve eachEntity in selEntityList)
                    {
                        List<Point3D> eachInterList = new List<Point3D>();
                        foreach (ICurve eachS in selSLineList)
                        {
                            Point3D[] eachInter = eachEntity.IntersectWith(eachS);
                            if (eachInter.Length > 0)
                            {
                                eachInterList.AddRange(eachInter);
                            }
                        }

                        if (eachInterList.Count == 2)
                        {

                            // StartLine
                            Point3D startPoint = eachEntity.StartPoint;
                            Point3D nearStartPoint = GetNearPoint(startPoint, eachInterList);
                            Line startLine = new Line(startPoint, nearStartPoint);
                            newList.Add(startLine);

                            // EndLine
                            Point3D endPoint = eachEntity.EndPoint;
                            Point3D nearEndPoint = GetNearPoint(endPoint, eachInterList);
                            Line endLine = new Line(endPoint, nearEndPoint);
                            newList.Add(endLine);

                            // Copy Attributes
                            startLine.CopyAttributesFast((Entity)eachEntity);
                            endLine.CopyAttributesFast((Entity)eachEntity);

                        }
                        else
                        {
                            newList.Add((Entity)eachEntity);
                        }
                    }
                }
                else
                {
                    newList.AddRange(selEntityList);
                }
            }

            return newList;
        }



        // Near Point
        private Point3D GetNearPoint(Point3D selPoint, List<Point3D> selPointList)
        {
            Point3D nearPoint = null;
            double nearPointDistance = 0;
            foreach (Point3D eachPoint in selPointList)
            {
                double tempDistance = selPoint.DistanceTo(eachPoint);
                if (nearPointDistance > tempDistance || nearPoint == null)
                {
                    nearPoint = eachPoint;
                    nearPointDistance = tempDistance;
                }

            }

            return nearPoint;
        }


        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
    }
}
