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
using DrawWork.Commons;

namespace DrawWork.DrawServices
{
    public class DrawNozzleService
    {
        private AssemblyModel assemblyData;

        private ValueService valueService;
        private DrawContactPointService contactPointService;

        public DrawNozzleService(AssemblyModel selAssembly)
        {
            assemblyData = selAssembly;

            valueService = new ValueService();
            contactPointService = new DrawContactPointService(selAssembly);
        }

        public DrawEntityModel DrawNozzle_GA(ref CDPoint refPoint,
                                      string selPosition, 
                                      string selNozzleType,
                                      string selNozzlePosition,
                                      string selNozzleFontSize,
                                      string selLeaderCircleSize,
                                      string selMultiColumn)
        {

            DrawEntityModel nozzleEntities = new DrawEntityModel();
            POSITION_TYPE newNozzlePosition = CommonMethod.PositionToEnum(selNozzlePosition);

            // Shell Spacing
            double shellSpacingLeft = valueService.GetDoubleValue(SingletonData.GAArea.Dimension.Length);
            double shellSpacingRight = valueService.GetDoubleValue(SingletonData.GAArea.Dimension.Length); 
            double shellSpacingTop = valueService.GetDoubleValue(SingletonData.GAArea.Dimension.Length); 
            double shellSpacingBottom = valueService.GetDoubleValue(SingletonData.GAArea.Dimension.Length); 

            // Nozzle Area
            double lowerAreaLeft = refPoint.X- shellSpacingLeft;
            double upperAreaLeft = valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeTankHeight);
            double lowerAreaRight = 400;
            double upperAreaRight = 400;
            double lowerAreaBottom = 400;
            double upperAreaBottom = 400;
            double lowerAreaTop = 400;
            double upperAreaTop = 400;

            // Type : 사용하지 않음

            // MutilColumn
            bool multiColumnValue = selMultiColumn == "true" ? true : false;



            // Nozzle List : Adjust
            List<NozzleInputModel> drawNozzle = new List<NozzleInputModel>();
            foreach(NozzleInputModel eachNozzle in assemblyData.NozzleInputModel)
            {
                eachNozzle.HeightSort = valueService.GetDoubleValue(eachNozzle.H); // sort value
                eachNozzle.Position = eachNozzle.Position.ToLower();
                eachNozzle.NozzlePosition = eachNozzle.NozzlePosition.ToLower();
                drawNozzle.Add(eachNozzle);
            }

            // Nozzle List : Sort
            List<NozzleInputModel> drawArrangeNozzle = drawNozzle.OrderBy(x => x.HeightSort).ToList();

            // Create Model
            double sizeNominalId = valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeNominalId);
            CDPoint newCurPoint = new CDPoint();
            CDPoint centerTopPoint = contactPointService.ContactPoint("centerroofpoint",ref refPoint,ref newCurPoint);
            double centerTopHeight = centerTopPoint.Y;
            foreach (NozzleInputModel eachNozzle in drawArrangeNozzle)
            {
                if (eachNozzle.Position == selPosition)
                {
                    if (eachNozzle.NozzlePosition == selNozzlePosition)
                    {
                        // outline
                        List<Entity> customEntity = CreateNozzleModelPosition(CommonMethod.PositionToEnum( eachNozzle.NozzlePosition), eachNozzle.HeightSort,sizeNominalId,centerTopHeight, refPoint);
                        nozzleEntities.outlineList.AddRange(customEntity);
                    }
                }
            }


            // Arrangement
            //List<NozzleInputModel> drawArrangeNozzle = drawNozzle.OrderBy(x => x.HeightSort).ToList();
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
                Dictionary<string, List<Entity>> customEntity = CreateNozzleLeader(drawPoint,eachNozzle.Mark,eachNozzle.Size,selNozzleFontSize, selLeaderCircleSize);
                nozzleEntities.nozzleMarkList.AddRange(customEntity[CommonGlobal.NozzleMark]);
                nozzleEntities.nozzleTextList.AddRange(customEntity[CommonGlobal.NozzleText]);

            }

            // Line
            List<Entity> customLineEntity = CreateNozzleLeaderLine(new Point3D(refPoint.X, refPoint.Y, 0), shellSpacingLeft, drawArrangeNozzle, leaderArrangementHeight);
            nozzleEntities.nozzlelineList.AddRange(customLineEntity);



            return nozzleEntities;
        }

        #region Nozzel : Create Model
        private List<Entity> CreateNozzleModelPosition(POSITION_TYPE selNozzlePosition,double selHeightSort,double selSizeNominalID,double selCenterTopHeight, CDPoint refPoint)
        {
            Point3D drawPoint = null;
            List<Entity> newNozzle = null;
            switch (selNozzlePosition)
            {
                case POSITION_TYPE.LEFT:
                    drawPoint = new Point3D(refPoint.X, refPoint.Y + selHeightSort, 0);
                    newNozzle = CreateNozzleModel(drawPoint,POSITION_TYPE.LEFT);
                    break;
                case POSITION_TYPE.RIGHT:
                    drawPoint = new Point3D(refPoint.X + selSizeNominalID, refPoint.Y + selHeightSort, 0);
                    newNozzle = CreateNozzleModel(drawPoint,POSITION_TYPE.RIGHT);
                    break;
                case POSITION_TYPE.TOP:
                    drawPoint = new Point3D(refPoint.X + selHeightSort, selCenterTopHeight, 0);
                    newNozzle = CreateNozzleModel(drawPoint,POSITION_TYPE.TOP);
                    break;
                case POSITION_TYPE.BOTTOM:
                    drawPoint = new Point3D(refPoint.X + selHeightSort, refPoint.Y, 0);
                    newNozzle = CreateNozzleModel(drawPoint,POSITION_TYPE.BOTTOM);
                    break;
            }

            return newNozzle;
        }
        private List<Entity> CreateNozzleModel(Point3D drawPoint,POSITION_TYPE selPosition)
        {

            // 노즐 겹치 구현 암됨
            // Width : 10 + 24 + 40 고정 -> 나중에 가변으로 변경해야 함
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

            // Rotation
            foreach(Entity eachEntity in customEntity)
            {
                switch (selPosition)
                {
                    case POSITION_TYPE.LEFT:
                        break;
                    case POSITION_TYPE.RIGHT:
                        eachEntity.Rotate(UtilityEx.DegToRad(-180), Vector3D.AxisZ, drawPoint);
                        break;
                    case POSITION_TYPE.TOP:
                        eachEntity.Rotate(UtilityEx.DegToRad(-90), Vector3D.AxisZ, drawPoint);
                        break;
                    case POSITION_TYPE.BOTTOM:
                        eachEntity.Rotate(UtilityEx.DegToRad(90), Vector3D.AxisZ, drawPoint);
                        break;
                }
            }

            return customEntity;
        }

        #endregion



        private Dictionary<string, List<Entity>> CreateNozzleLeader(Point3D drawPoint,string textUpperStr,string textLowerStr,string fontSize,string circleSize)
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
            List<Entity> nozzleMarkList = new List<Entity>();
            nozzleMarkList.Add(circleCenter);
            nozzleMarkList.Add(lineCenter);

            List<Entity> nozzleTextList = new List<Entity>();
            nozzleTextList.Add(textUpper);
            nozzleTextList.Add(textLower);

            Dictionary<string, List<Entity>> customEntity = new Dictionary<string, List<Entity>>();
            customEntity.Add(CommonGlobal.NozzleMark, nozzleMarkList);
            customEntity.Add(CommonGlobal.NozzleText, nozzleTextList);

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
            
            double staPointX = -(10 + 24 + 40);// 나중에 가변으로 변경 해야 함
            double endPointX = - shellSpacingLeft;
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
