using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawWork.Commons;
using DrawWork.DrawServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawShapes
{
    public class DrawBreakSymbols
    {
        public DrawEditingService editingService;


        public DrawBreakSymbols() 
        {
            editingService = new DrawEditingService();
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
                List<Entity> upperLineList = GetSLine(upperPoint, selWidth, selMirror, 0, selAngleType, selfactor);
                List<Entity> lowerLineList = GetSLine(lowerPoint, selWidth, selMirror, 0, selAngleType, selfactor);

                if (selRotate != 0)
                {
                    newList.AddRange(editingService.GetRotate(upperLineList, GetSumPoint(selPoint, 0, 0), Utility.DegToRad(selRotate)));
                    newList.AddRange(editingService.GetRotate(lowerLineList, GetSumPoint(selPoint, 0, 0), Utility.DegToRad(selRotate)));
                }
                else
                {
                    newList.AddRange(upperLineList);
                    newList.AddRange(lowerLineList);
                }
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






        public List<Entity> GetFlatBreakLine(Point3D selStartPoint, Point3D selEndPoint, double selScaleValue, bool startExt = true,bool endExt=true)
        {
            List<Entity> newList = new List<Entity>();

            double extLineLenght = 2 * selScaleValue;
            Line newLine = new Line(GetSumPoint(selStartPoint, 0, 0), GetSumPoint(selEndPoint, 0, 0));
            Point3D midPoint = GetSumPoint(newLine.MidPoint, 0, 0);
            double angle = editingService.GetAngleOfLine(newLine);

            if(startExt)
                editingService.SetExtendLine(ref newLine, extLineLenght, false);
            if(endExt)
                editingService.SetExtendLine(ref newLine, extLineLenght, true);

            double midLength = 1;
            double midCrossLength = 2;
            Line leftLine = new Line(GetSumPoint(midPoint, -midLength, 0), GetSumPoint(midPoint, -midLength, -midCrossLength));
            Line rightLine = new Line(GetSumPoint(midPoint, +midLength, 0), GetSumPoint(midPoint, +midLength, +midCrossLength));
            Line crossLine = new Line(GetSumPoint(leftLine.EndPoint, 0, 0), GetSumPoint(rightLine.EndPoint, 0, 0));
            leftLine.Rotate(angle, Vector3D.AxisZ, GetSumPoint(midPoint, 0, 0));
            rightLine.Rotate(angle, Vector3D.AxisZ, GetSumPoint(midPoint, 0, 0));
            crossLine.Rotate(angle, Vector3D.AxisZ, GetSumPoint(midPoint, 0, 0));

            Line leftExtLine = new Line(GetSumPoint(newLine.StartPoint, 0, 0), GetSumPoint(leftLine.StartPoint, 0, 0));
            Line rightExtLine = new Line(GetSumPoint(newLine.EndPoint, 0, 0), GetSumPoint(rightLine.StartPoint, 0, 0));
            //if (startExt)
            //    newList.Add(leftExtLine);
            //if (endExt)
            ///    newList.Add(rightExtLine);
            newList.AddRange(new Entity[] { leftExtLine, rightExtLine, leftLine, rightLine, crossLine });

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
