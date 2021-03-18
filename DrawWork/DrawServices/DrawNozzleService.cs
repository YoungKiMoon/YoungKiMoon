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

using AssemblyLib.AssemblyModels;

using DrawWork.ValueServices;
using DrawWork.DrawModels;
using DrawWork.Common;

namespace DrawWork.DrawServices
{
    public class DrawNozzleService
    {
        private ValueService valueService;

        public DrawNozzleService()
        {
            valueService = new ValueService();
        }

        public Entity[] DrawNozzle_GA(ref CDPoint refPoint,
                                      string selPosition, 
                                      string selNozzleType,
                                      string selNozzlePosition,
                                      string selNozzleFontSize,
                                      string selLeaderCircleSize,
                                      string selMultiColumn, 
                                      AssemblyModel selAssembly)
        {

            // Shell Spacing
            double shellSpacingLeft = valueService.GetDoubleValue(SingletonData.GAArea.Dimension.Length);
            double shellSpacingRight = 400;
            double shellSpacingTop = 400;
            double shellSpacingBottom = 400;

            // Nozzle Area
            double lowerAreaLeft = refPoint.X- shellSpacingLeft;
            double upperAreaLeft = valueService.GetDoubleValue(selAssembly.GeneralDesignData.SizeTankHeight);
            double lowerAreaRight = 400;
            double upperAreaRight = 400;
            double lowerAreaBottom = 400;
            double upperAreaBottom = 400;
            double lowerAreaTop = 400;
            double upperAreaTop = 400;

            // Type
            switch (selNozzleType)
            {
                case "Bottom":
                    //drawPoint = GetSumPoint(drawPoint, -AB, -AB);

                    break;

            }

            // MutilColumn
            bool multiColumnValue = selMultiColumn == "true" ? true : false;
            

                // Entity
                List<Entity> customEntityList = new List<Entity>();


            // Nozzle List
            List<NozzleInputModel> drawNozzle = new List<NozzleInputModel>();

            // Model
            foreach (NozzleInputModel eachNozzle in selAssembly.NozzleInputModel)
            {
                if (eachNozzle.Position.ToLower() == selPosition)
                {
                    if(eachNozzle.NozzlePosition.ToLower()== selNozzlePosition)
                    {
                        eachNozzle.HeightSort = valueService.GetDoubleValue(eachNozzle.H);
                        // add
                        drawNozzle.Add(eachNozzle);

                        Point3D drawPoint = new Point3D(refPoint.X, refPoint.Y + eachNozzle.HeightSort, 0);
                        List<Entity> customEntity = CreateNozzleModel(drawPoint);
                        foreach (Entity eachEntity in customEntity)
                        {
                            customEntityList.Add(eachEntity);
                        }
                    }
                }
            }


            // Arrangement
            List<NozzleInputModel> drawArrangeNozzle = drawNozzle.OrderBy(x => x.HeightSort).ToList();
            List<Point3D> leaderArrangementHeight = GetArrangeLeaderPosition(new Point3D(refPoint.X, refPoint.Y, 0), shellSpacingLeft, selLeaderCircleSize, drawArrangeNozzle, multiColumnValue);

            // Leader
            int indexArrange = -1;
            foreach (NozzleInputModel eachNozzle in drawArrangeNozzle)
            {
                indexArrange++;
                double currentHeight=leaderArrangementHeight[indexArrange].Y;
                double currentX = leaderArrangementHeight[indexArrange].X;
                //Point3D drawPoint = new Point3D(refPoint.X - shellSpacingLeft, refPoint.Y + currentHeight, 0);
                Point3D drawPoint = new Point3D(currentX, refPoint.Y + currentHeight, 0);
                List < Entity> customEntity = CreateNozzleLeader(drawPoint,eachNozzle.Mark,eachNozzle.Size,selNozzleFontSize, selLeaderCircleSize);
                foreach (Entity eachEntity in customEntity)
                {
                    customEntityList.Add(eachEntity);
                }
            }

            // Line
            List<Entity> customLineEntity = CreateNozzleLeaderLine(new Point3D(refPoint.X, refPoint.Y, 0), shellSpacingLeft, drawArrangeNozzle, leaderArrangementHeight);
            foreach (Entity eachEntity in customLineEntity)
            {
                customEntityList.Add(eachEntity);
            }




            return customEntityList.ToArray();
        }

        // Width : 10 + 24 + 40 고정 -> 나중에 가변으로 변경해야 함
        // 노즐 겹치 구현 암됨
        private List<Entity> CreateNozzleModel(Point3D drawPoint)
        {
            double flangeFaceWidth = 10;
            double flangeFaceHeight = 80;
            double flangeFaceInnerWidth = 10;
            double flangePipeWidth = 24;
            double flangePipeHeight = flangeFaceHeight - (flangeFaceInnerWidth * 2);
            double flangePipeInnerWidth = 18;
            double pipeHeight = flangeFaceHeight - (flangePipeInnerWidth * 2);
            double pipeWidth = 40;
            double fullWidth = flangeFaceWidth + flangePipeWidth + pipeWidth;
            double fullHeight = flangeFaceHeight;

            // Point Adj
            drawPoint.X = drawPoint.X - fullWidth;
            drawPoint.Y = drawPoint.Y - (fullHeight/2);

            // Model
            Line lineFFa = new Line(GetSumPoint(drawPoint,0, 0, 0), GetSumPoint(drawPoint, 0, flangeFaceHeight, 0));
            Line lineFFb = new Line(GetSumPoint(drawPoint, flangeFaceWidth, 0, 0), GetSumPoint(drawPoint, flangeFaceWidth, flangeFaceHeight, 0));
            Line lineFFc = new Line(GetSumPoint(drawPoint,0, 0, 0), GetSumPoint(drawPoint,flangeFaceWidth, 0, 0));
            Line lineFFd = new Line(GetSumPoint(drawPoint,0, flangeFaceHeight, 0), GetSumPoint(drawPoint,flangeFaceWidth, flangeFaceHeight, 0));

            Line lineFPa = new Line(GetSumPoint(drawPoint,flangeFaceWidth, flangeFaceInnerWidth + flangePipeHeight, 0), GetSumPoint(drawPoint,flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth + pipeHeight, 0));
            Line lineFPb = new Line(GetSumPoint(drawPoint,flangeFaceWidth, flangeFaceInnerWidth, 0), GetSumPoint(drawPoint,flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0));
            Line lineFPc = new Line(GetSumPoint(drawPoint,flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth + pipeHeight, 0), GetSumPoint(drawPoint,flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0));

            Line linePa = new Line(GetSumPoint(drawPoint,flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0), GetSumPoint(drawPoint,flangeFaceWidth + flangePipeWidth + pipeWidth, flangePipeInnerWidth, 0));
            Line linePb = new Line(GetSumPoint(drawPoint,flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth + pipeHeight, 0), GetSumPoint(drawPoint,flangeFaceWidth + flangePipeWidth + pipeWidth, flangePipeInnerWidth + pipeHeight, 0));

            // Entity
            List<Entity> customEntity = new List<Entity>();
            customEntity.Add(lineFFa);
            customEntity.Add(lineFFb);
            customEntity.Add(lineFFc);
            customEntity.Add(lineFFd);
            customEntity.Add(lineFPa);
            customEntity.Add(lineFPb);
            customEntity.Add(lineFPc);
            customEntity.Add(linePa);
            customEntity.Add(linePb);


            return customEntity;
        }

        private List<Entity> CreateNozzleLeader(Point3D drawPoint,string textUpperStr,string textLowerStr,string fontSize,string circleSize)
        {
            // Current Test
            double cirDiameter = valueService.GetDoubleValue(circleSize);
            double cirRadius = cirDiameter/2;
            double cirTextSize = valueService.GetDoubleValue(fontSize);

            // Point Adj
            drawPoint.X = drawPoint.X - cirDiameter;
            drawPoint.Y = drawPoint.Y - cirRadius;

            Circle circleCenter = new Circle(GetSumPoint(drawPoint, cirRadius, cirRadius, 0), cirRadius);
            Line lineCenter = new Line(GetSumPoint(drawPoint, 0, cirRadius, 0), GetSumPoint(drawPoint, cirDiameter, cirRadius, 0));
            Text textUpper = new Text(GetSumPoint(drawPoint,cirRadius, cirRadius + (cirRadius / 2), 0), textUpperStr, cirTextSize);
            textUpper.Alignment = Text.alignmentType.MiddleCenter;
            Text textLower = new Text(GetSumPoint(drawPoint,cirRadius, (cirRadius / 2), 0), textLowerStr, cirTextSize);
            textLower.Alignment = Text.alignmentType.MiddleCenter;

            // Entity
            List<Entity> customEntity = new List<Entity>();
            customEntity.Add(circleCenter);
            customEntity.Add(lineCenter);
            customEntity.Add(textUpper);
            customEntity.Add(textLower);



            return customEntity;
        }

        // 노즐 지시 겹칩 가능
        // Left 영역만 구현
        private List<Point3D> GetArrangeLeaderPosition(Point3D drawPoint, double selShellSpacingLeft, string selLeaderCircleSize, List<NozzleInputModel> drawArrangeNozzle, bool convergenceSign = true)
        {
            double arrangeGapHeight = 40;
            double arrangeMaxHeight = 0;
            double circleSize = valueService.GetDoubleValue(selLeaderCircleSize);
            double shellSpacingLeft = selShellSpacingLeft;

            // convergence
            double convergenceHeight = 100;
            bool convergenceValue = false;
            double beforeNewHeight = 0;
            double convergenceX = 0;

            List<Point3D> leaderArrangementHeight = new List<Point3D>();
            foreach (NozzleInputModel eachNozzle in drawArrangeNozzle)
            {
                // Arrange : Bottom Type

                double newHeight = eachNozzle.HeightSort;

                // Convergence
                convergenceValue = false;
                if (beforeNewHeight > 0)
                    if (newHeight - beforeNewHeight <= convergenceHeight)
                    {
                        convergenceValue = true;
                    }
                beforeNewHeight = eachNozzle.HeightSort;

                if (convergenceValue && convergenceSign)
                {
                    // Convergence
                    convergenceX -=circleSize;
                    leaderArrangementHeight.Add(new Point3D(convergenceX, arrangeMaxHeight-circleSize/2 -arrangeGapHeight));
                }
                else
                {
                    if (leaderArrangementHeight.Count == 0)
                    {
                        leaderArrangementHeight.Add(new Point3D(drawPoint.X - shellSpacingLeft, newHeight));
                        arrangeMaxHeight = newHeight + circleSize / 2 + arrangeGapHeight;
                        continue;
                    }


                    double currentLower = newHeight - circleSize / 2;

                    if (arrangeMaxHeight > currentLower)
                        newHeight = arrangeMaxHeight + circleSize / 2;


                    double newFixHeight = newHeight;
                    leaderArrangementHeight.Add(new Point3D(drawPoint.X - shellSpacingLeft, newFixHeight));
                    arrangeMaxHeight = newHeight + circleSize / 2 + arrangeGapHeight;

                    convergenceX = drawPoint.X-shellSpacingLeft;
                }


            }

            return leaderArrangementHeight;
        }

        private List<Entity> CreateNozzleLeaderLine(Point3D drawPoint,double shellSpacingLeft, List<NozzleInputModel> selNozzle, List<Point3D> selArrangeHeight)
        {
            // Entity
            List<Entity> customEntity = new List<Entity>();


            double midPointXGap = 200;
            
            double staPointX = drawPoint.X - (10 + 24 + 40);// 나중에 가변으로 변경 해야 함
            double endPointX = drawPoint.X - shellSpacingLeft;
            double midPointX = endPointX;
            double midPointY = 0;


            // Convergence Check : 높이가 같을 경우
            double beforeEndPointY = 0;
            int indexArrange = -1;
            foreach (NozzleInputModel eachNozzle in selNozzle)
            {
                indexArrange++;
                double staPointY = eachNozzle.HeightSort;
                double endPointY = selArrangeHeight[indexArrange].Y;

                // 직선
                if (staPointY == endPointY)
                {
                    Line lineOne = new Line(GetSumPoint(drawPoint, staPointX, staPointY, 0), GetSumPoint(drawPoint, endPointX, endPointY, 0));
                    customEntity.Add(lineOne);
                    midPointX = endPointX;
                }
                // 시작 보다 아래
                else if(staPointY>endPointY)
                {
                    
                }
                // 시작 보다 높음
                else if (staPointY < endPointY)
                {
                    if(beforeEndPointY!=endPointY)
                        midPointX += midPointXGap;

                    Line lineUpperOne = new Line(GetSumPoint(drawPoint, staPointX, staPointY, 0), GetSumPoint(drawPoint, midPointX, staPointY, 0));
                    Line lineUpperTwo = new Line(GetSumPoint(drawPoint, midPointX, staPointY, 0), GetSumPoint(drawPoint, midPointX, endPointY, 0));
                    Line lineUpperThr = new Line(GetSumPoint(drawPoint, midPointX, endPointY, 0), GetSumPoint(drawPoint, endPointX, endPointY, 0));
                    customEntity.Add(lineUpperOne);
                    customEntity.Add(lineUpperTwo);
                    customEntity.Add(lineUpperThr);
                }

                beforeEndPointY = endPointY;
            }


            return customEntity;
        }

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
    }
}
