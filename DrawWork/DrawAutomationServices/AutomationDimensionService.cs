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
using DrawWork.Commons;
using DrawWork.DrawModels;

namespace DrawWork.DrawAutomationServices
{
    public class AutomationDimensionService
    {
        public AutomationDimensionService()
        {

        }

        public bool GetDimensionBreak(string targetLine, string referenceLine)
        {
            bool returnValue = false;
            if(targetLine=="outline")
            {
                switch (referenceLine)
                {
                    case "outline":
                        returnValue = false;
                        break;
                    case "centerline":
                        returnValue = false;
                        break;
                    case "dimline":
                        returnValue = false;
                        break;
                    case "dimtext":
                        returnValue = true;
                        break;
                    case "dimlineext":
                        returnValue = false;
                        break;
                    case "dimarrow":
                        returnValue = true;
                        break;
                    case "leaderline":
                        returnValue = false;
                        break;
                    case "leaderarrow":
                        returnValue = false;
                        break;
                    case "leadertext":
                        returnValue = true;
                        break;
                    case "nozzleline":
                        returnValue = false;
                        break;
                    case "nozzlemark":
                        returnValue = false;
                        break;
                };
            }
            else if(targetLine == "centerline")
            {
                switch (referenceLine)
                {
                    case "outline":
                        returnValue = false;
                        break;
                    case "centerline":
                        returnValue = false;
                        break;
                    case "dimline":
                        returnValue = false;
                        break;
                    case "dimtext":
                        returnValue = true;
                        break;
                    case "dimlineext":
                        returnValue = false;
                        break;
                    case "dimarrow":
                        returnValue = false;
                        break;
                    case "leaderline":
                        returnValue = false;
                        break;
                    case "leadertext":
                        returnValue = true;
                        break;
                    case "leaderarrow":
                        returnValue = false;
                        break;
                    case "nozzleline":
                        returnValue = false;
                        break;
                    case "nozzlemark":
                        returnValue = false;
                        break;
                };
            }
            else if (targetLine == "dimline")
            {
                switch (referenceLine)
                {
                    case "outline":
                        returnValue = false;
                        break;
                    case "centerline":
                        returnValue = false;
                        break;
                    case "dimline":
                        returnValue = false;
                        break;
                    case "dimtext":
                        returnValue = false;
                        break;
                    case "dimlineext":
                        returnValue = false;
                        break;
                    case "dimarrow":
                        returnValue = false;
                        break;
                    case "leaderline":
                        returnValue = false;
                        break;
                    case "leadertext":
                        returnValue = false;
                        break;
                    case "leaderarrow":
                        returnValue = false;
                        break;
                    case "nozzleline":
                        returnValue = false;
                        break;
                    case "nozzlemark":
                        returnValue = false;
                        break;
                };
            }
            else if (targetLine == "dimarrow")
            {
                switch (referenceLine)
                {
                    case "outline":
                        returnValue = false;
                        break;
                    case "centerline":
                        returnValue = false;
                        break;
                    case "dimline":
                        returnValue = false;
                        break;
                    case "dimtext":
                        returnValue = false;
                        break;
                    case "dimlineext":
                        returnValue = false;
                        break;
                    case "dimarrow":
                        returnValue = false;
                        break;
                    case "leaderline":
                        returnValue = false;
                        break;
                    case "leadertext":
                        returnValue = false;
                        break;
                    case "leaderarrow":
                        returnValue = false;
                        break;
                    case "nozzleline":
                        returnValue = false;
                        break;
                    case "nozzlemark":
                        returnValue = false;
                        break;
                };
            }
            else if (targetLine == "dimtext")
            {
                switch (referenceLine)
                {
                    case "outline":
                        returnValue = false;
                        break;
                    case "centerline":
                        returnValue = false;
                        break;
                    case "dimline":
                        returnValue = false;
                        break;
                    case "dimtext":
                        returnValue = false;
                        break;
                    case "dimlineext":
                        returnValue = false;
                        break;
                    case "dimarrow":
                        returnValue = false;
                        break;
                    case "leaderline":
                        returnValue = false;
                        break;
                    case "leadertext":
                        returnValue = false;
                        break;
                    case "leaderarrow":
                        returnValue = false;
                        break;
                    case "nozzleline":
                        returnValue = false;
                        break;
                    case "nozzlemark":
                        returnValue = false;
                        break;
                };
            }
            else if (targetLine == "dimlineext")
            {
                switch (referenceLine)
                {
                    case "outline":
                        returnValue = true;
                        break;
                    case "centerline":
                        returnValue = false;
                        break;
                    case "dimline":
                        returnValue = true;
                        break;
                    case "dimtext":
                        returnValue = true;
                        break;
                    case "dimlineext":
                        returnValue = false;
                        break;
                    case "dimarrow":
                        returnValue = true;
                        break;
                    case "leaderline":
                        returnValue = true;
                        break;
                    case "leadertext":
                        returnValue = true;
                        break;
                    case "leaderarrow":
                        returnValue = true;
                        break;
                    case "nozzleline":
                        returnValue = true;
                        break;
                    case "nozzlemark":
                        returnValue = true;
                        break;
                };
            }
            else if (targetLine == "leaderline")
            {
                switch (referenceLine)
                {
                    case "outline":
                        returnValue = false;
                        break;
                    case "centerline":
                        returnValue = false;
                        break;
                    case "dimline":
                        returnValue = true;
                        break;
                    case "dimtext":
                        returnValue = true;
                        break;
                    case "dimlineext":
                        returnValue = false;
                        break;
                    case "dimarrow":
                        returnValue = true;
                        break;
                    case "leaderline":
                        returnValue = false;
                        break;
                    case "leadertext":
                        returnValue = true;
                        break;
                    case "leaderarrow":
                        returnValue = false;
                        break;
                    case "nozzleline":
                        returnValue = false;
                        break;
                    case "nozzlemark":
                        returnValue = true;
                        break;
                };
            }
            else if (targetLine == "leaderarrow")
            {
                switch (referenceLine)
                {
                    case "outline":
                        returnValue = false;
                        break;
                    case "centerline":
                        returnValue = false;
                        break;
                    case "dimline":
                        returnValue = true;
                        break;
                    case "dimtext":
                        returnValue = true;
                        break;
                    case "dimlineext":
                        returnValue = false;
                        break;
                    case "dimarrow":
                        returnValue = true;
                        break;
                    case "leaderline":
                        returnValue = false;
                        break;
                    case "leadertext":
                        returnValue = true;
                        break;
                    case "leaderarrow":
                        returnValue = false;
                        break;
                    case "nozzleline":
                        returnValue = false;
                        break;
                    case "nozzlemark":
                        returnValue = true;
                        break;
                };
            }
            else if (targetLine == "leadertext")
            {
                switch (referenceLine)
                {
                    case "outline":
                        returnValue = false;
                        break;
                    case "centerline":
                        returnValue = false;
                        break;
                    case "dimline":
                        returnValue = false;
                        break;
                    case "dimtext":
                        returnValue = false;
                        break;
                    case "dimlineext":
                        returnValue = false;
                        break;
                    case "dimarrow":
                        returnValue = false;
                        break;
                    case "leaderline":
                        returnValue = false;
                        break;
                    case "leadertext":
                        returnValue = false;
                        break;
                    case "leaderarrow":
                        returnValue = false;
                        break;
                    case "nozzleline":
                        returnValue = false;
                        break;
                    case "nozzlemark":
                        returnValue = false;
                        break;
                };
            }
            else if (targetLine == "nozzleline")
            {
                switch (referenceLine)
                {
                    case "outline":
                        returnValue = true;
                        break;
                    case "centerline":
                        returnValue = false;
                        break;
                    case "dimline":
                        returnValue = true;
                        break;
                    case "dimtext":
                        returnValue = true;
                        break;
                    case "dimlineext":
                        returnValue = true;
                        break;
                    case "dimarrow":
                        returnValue = true;
                        break;
                    case "leaderline":
                        returnValue = true;
                        break;
                    case "leadertext":
                        returnValue = true;
                        break;
                    case "leaderarrow":
                        returnValue = true;
                        break;
                    case "nozzleline":
                        returnValue = false;
                        break;
                    case "nozzlemark":
                        returnValue = true;
                        break;
                };
            }
            else if (targetLine == "nozzlemark")
            {
                switch (referenceLine)
                {
                    case "outline":
                        returnValue = true;
                        break;
                    case "centerline":
                        returnValue = false;
                        break;
                    case "dimline":
                        returnValue = false;
                        break;
                    case "dimtext":
                        returnValue = false;
                        break;
                    case "dimlineext":
                        returnValue = false;
                        break;
                    case "dimarrow":
                        returnValue = false;
                        break;
                    case "leaderline":
                        returnValue = false;
                        break;
                    case "leadertext":
                        returnValue = false;
                        break;
                    case "leaderarrow":
                        returnValue = false;
                        break;
                    case "nozzleline":
                        returnValue = false;
                        break;
                    case "nozzlemark":
                        returnValue = false;
                        break;
                };
            }

            return returnValue;
        }


        public List<Entity> SetLineBreak(Model singleModel, DrawEntityModel selDrawEntity)
        {
            List<Entity> returnEntity = new List<Entity>();

            List<Entity> outlineList = selDrawEntity.outlineList;
            List<Entity> centerlineList = selDrawEntity.centerlineList;
            List<Entity> dimlineList = selDrawEntity.dimlineList;
            List<Entity> dimTextList = selDrawEntity.dimTextList;
            List<Entity> dimlineExtList = selDrawEntity.dimlineExtList;
            List<Entity> dimArrowList = selDrawEntity.dimArrowList;
            List<Entity> leaderlineList = selDrawEntity.leaderlineList;
            List<Entity> leaderTextList = selDrawEntity.leaderTextList;
            List<Entity> leaderArrowList = selDrawEntity.leaderArrowList;
            List<Entity> nozzlelineList = selDrawEntity.nozzlelineList;
            List<Entity> nozzleMarkList = selDrawEntity.nozzleMarkList;
            List<Entity> nozzleTextList = selDrawEntity.nozzleTextList;



            // Reference Circle
            double breakRadius = 10;


            #region Sample Data
            // 세로
            //outlineList.Add(new Line(new Point3D(10, -20, 0), new Point3D(10, 1110, 0)));
            //centerlineList.Add(new Line(new Point3D(110, -20, 0), new Point3D(110, 1110, 0)));
            //dimlineList.Add(new Line(new Point3D(210, -20, 0), new Point3D(210, 1110, 0)));
            //Text dimText01 = new Text(new Point3D(0, -310, 0), "Dimension Line ABCDEFGabcdefg12345", 50);

            //dimText01.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
            //dimTextList.Add(dimText01);
            //dimlineExtList.Add(new Line(new Point3D(410, -20, 0), new Point3D(410, 1110, 0)));
            //leaderlineList.Add(new Line(new Point3D(510, -20, 0), new Point3D(510, 1110, 0)));
            //Text leaderText01 = new Text(new Point3D(0, -610, 0), "Leader Line ABCDEFGabcdefg12345", 50);
            //leaderText01.Rotate(Utility.DegToRad(90), Vector3D.AxisZ);
            //leaderTextList.Add(leaderText01);
            //nozzlelineList.Add(new Line(new Point3D(710, -20, 0), new Point3D(710, 1110, 0)));
            //nozzleMarkList.Add(new Line(new Point3D(810, -20, 0), new Point3D(810, 1110, 0)));

            //// 가로
            //outlineList.Add(new Line(new Point3D(-20, 10, 0), new Point3D(1110, 10, 0)));
            //centerlineList.Add(new Line(new Point3D(-20, 110, 0), new Point3D(1110, 110, 0)));
            //dimlineList.Add(new Line(new Point3D(-20, 210, 0), new Point3D(1110, 210, 0)));
            //Text dimText02 = new Text(new Point3D(-20, 310, 0), "Dimension Line ABCDEFGabcdefg12345", 50);
            //dimTextList.Add(dimText02);
            //dimlineExtList.Add(new Line(new Point3D(-20, 410, 0), new Point3D(1110, 410, 0)));
            //leaderlineList.Add(new Line(new Point3D(-20, 510, 0), new Point3D(1110, 510, 0)));
            //Text leaderText02 = new Text(new Point3D(-20, 610, 0), "Leader Line ABCDEFGabcdefg12345", 50);
            //leaderTextList.Add(leaderText02);
            //nozzlelineList.Add(new Line(new Point3D(-20, 710, 0), new Point3D(1110, 710, 0)));
            //nozzleMarkList.Add(new Line(new Point3D(-20, 810, 0), new Point3D(1110, 810, 0)));
            #endregion

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

            // Arrow : 수정 최소 단위로
            List<Entity> dimArrowLinearPathList = new List<Entity>();
            foreach (Entity eachEntity in dimArrowList)
            {
                List<Point3D> newTriPath = new List<Point3D>();
                newTriPath.AddRange(eachEntity.Vertices);
                
                Point3D centerPoint = new Point3D();

                if (newTriPath[1].X == newTriPath[2].X)
                {
                    // Left Right
                    double newDistance = Point3D.Distance(newTriPath[1], newTriPath[2]);
                    double minValue = Math.Min(newTriPath[1].Y, newTriPath[2].Y);
                    double quotient = Math.Truncate(newDistance / breakRadius);

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
                        Line newLine = new Line(centerPoint, new Point3D(newTriPath[1].X, minValue + breakRadius * i));
                        dimArrowLinearPathList.Add(newLine);
                    }
                }
                else if (newTriPath[1].Y == newTriPath[2].Y)
                {
                    // Top Bottom
                    double newDistance = Point3D.Distance(newTriPath[1], newTriPath[2]);
                    double minValue = Math.Min(newTriPath[1].X, newTriPath[2].X);
                    double quotient = Math.Truncate(newDistance / breakRadius);

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
                        Line newLine = new Line(centerPoint, new Point3D(minValue + breakRadius * i, newTriPath[1].Y));
                        dimArrowLinearPathList.Add(newLine);
                    }
                }

                newTriPath = new List<Point3D>();
                newTriPath.Add(centerPoint);
                newTriPath.Add(eachEntity.Vertices[1]);
                newTriPath.Add(eachEntity.Vertices[2]);
                newTriPath.Add(centerPoint);
                LinearPath newLinearPath = new LinearPath(newTriPath.ToArray());
                dimArrowLinearPathList.Add(newLinearPath);
            }
            List<Entity> leaderArrowLinearPathList = new List<Entity>();
            foreach (Entity eachEntity in leaderArrowList)
            {
                double degree = 0;
                Triangle newDegreeTri = (Triangle)eachEntity.Clone();
                if (eachEntity.Vertices[0].Y < eachEntity.Vertices[1].Y)
                {
                    if (eachEntity.Vertices[0].X < eachEntity.Vertices[1].X)
                    {
                        //topright 60
                        degree = 60;
                    }
                    else
                    {
                        //topleft 120
                        degree = 120;
                    }
                }
                else
                {
                    if (eachEntity.Vertices[0].X < eachEntity.Vertices[1].X)
                    {
                        //bottomright -60
                        degree = -60;
                    }
                    else
                    {
                        //bottomleft -120
                        degree = 120;
                    }
                }
                newDegreeTri.Rotate(Utility.DegToRad(-degree), Vector3D.AxisZ, eachEntity.Vertices[0]);

                // Degree
                //newLeaderArrow.Rotate(Utility.DegToRad(calDegree), Vector3D.AxisZ, new Point3D(selPoint1.X, selPoint1.Y));

                List<Point3D> newTriPath = new List<Point3D>();
                newTriPath.AddRange(newDegreeTri.Vertices);

                Point3D centerPoint = new Point3D();


                // 왼쪽으로 표족한 삼각형
                double newDistance = Point3D.Distance(newTriPath[1], newTriPath[2]);
                double minValue = Math.Min(newTriPath[1].X, newTriPath[2].X);
                double quotient = Math.Truncate(newDistance / breakRadius);

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
                    Line newLine = new Line(centerPoint, new Point3D(minValue + breakRadius * i, newTriPath[1].Y));
                    newLine.Rotate(Utility.DegToRad(degree), Vector3D.AxisZ, centerPoint);
                    leaderArrowLinearPathList.Add(newLine);
                }
                

                newTriPath = new List<Point3D>();
                newTriPath.Add(centerPoint);
                newTriPath.Add(eachEntity.Vertices[1]);
                newTriPath.Add(eachEntity.Vertices[2]);
                newTriPath.Add(centerPoint);
                LinearPath newLinearPath = new LinearPath(newTriPath.ToArray());
                leaderArrowLinearPathList.Add(newLinearPath);
            }





            // All list
            Dictionary<string, List<Entity>> allEntityDic = new Dictionary<string, List<Entity>>();
            allEntityDic.Add(CommonGlobal.OutLine, outlineList);
            allEntityDic.Add(CommonGlobal.CenterLine, centerlineList);
            allEntityDic.Add(CommonGlobal.DimLine, dimlineList);
            allEntityDic.Add(CommonGlobal.DimText, dimTextLinearPathList); // text 대신 linear path
            allEntityDic.Add(CommonGlobal.DimLineExt, dimlineExtList);
            allEntityDic.Add(CommonGlobal.DimArrow, dimArrowLinearPathList); // arrow 대신 linear path
            allEntityDic.Add(CommonGlobal.LeaderLine, leaderlineList);
            allEntityDic.Add(CommonGlobal.LeaderText, leaderTextLinearPathList); // text 대신 linear path
            allEntityDic.Add(CommonGlobal.LeaderArrow, leaderArrowLinearPathList); // arrow 대신 linear path
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
                if (!eachTarget.Contains("text") && !eachTarget.Contains("arrow"))
                {
                    foreach (ICurve targetLine in eachTargetList)
                    {
                        // 1. 겹치는 점 찾기
                        Dictionary<Point3D, int> intersectPointDic = new Dictionary<Point3D, int>();
                        for (int j = 0; j < allEntityName.Count; j++)
                        {
                            string eachReference = allEntityName[j];
                            // 끊기 여부
                            if (GetDimensionBreak(eachTarget, eachReference))
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
            returnEntity.AddRange(dimTextList);
            returnEntity.AddRange(leaderTextList);
            returnEntity.AddRange(nozzleTextList);
            // 화살표 추가
            returnEntity.AddRange(dimArrowList);
            returnEntity.AddRange(leaderArrowList);

            // Line Break 추가
            List<string> allEntityNewName = allEntityNewDic.Keys.ToList();
            List<List<Entity>> allEntityNewList = allEntityNewDic.Values.ToList();
            for (int i = 0; i < allEntityNewName.Count; i++)
            {
                List<Entity> eachEntityList = allEntityNewList[i];
                returnEntity.AddRange(eachEntityList);
            }


            return returnEntity;
        }


        private Point3D GetMinPointXY(Point3D[] selPointArray)
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
        private Point3D GetMaxPointXY(Point3D[] selPointArray)
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

    }
}
