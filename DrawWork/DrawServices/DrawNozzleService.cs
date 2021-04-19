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
            DrawPositionValueModel shellSpacing = new DrawPositionValueModel();
            shellSpacing.Left= valueService.GetDoubleValue(SingletonData.GAArea.Dimension.Length);
            shellSpacing.Right= valueService.GetDoubleValue(SingletonData.GAArea.Dimension.Length);
            shellSpacing.Top = valueService.GetDoubleValue(SingletonData.GAArea.Dimension.Length);
            shellSpacing.Bottom = valueService.GetDoubleValue(SingletonData.GAArea.Dimension.Length);

            // Reference Position
            double sizeNominalId = valueService.GetDoubleValue(assemblyData.GeneralDesignData.SizeNominalId);
            CDPoint newCurPoint = new CDPoint();
            CDPoint centerTopPoint = contactPointService.ContactPoint("centerroofpoint", ref refPoint, ref newCurPoint);
            double centerTopHeight = centerTopPoint.Y;


            // MutilColumn
            bool multiColumnValue = selMultiColumn == "true" ? true : false;



            // Nozzle List : Adjust
            List<NozzleInputModel> drawNozzle = new List<NozzleInputModel>();
            foreach(NozzleInputModel eachNozzle in assemblyData.NozzleInputModel)
            {
                eachNozzle.HRSort= valueService.GetDoubleValue(eachNozzle.H); // sort value
                eachNozzle.Position = eachNozzle.Position.ToLower();
                eachNozzle.LR = eachNozzle.LR.ToLower();
                drawNozzle.Add(eachNozzle);
            }

            // Nozzle List : Sort
            List<NozzleInputModel> drawArrangeNozzle = drawNozzle.OrderBy(x => x.HRSort).ToList();

            // Nozzle Start Point List
            List<Point3D> NozzlePointList = new List<Point3D>();
            foreach (NozzleInputModel eachNozzle in drawArrangeNozzle)
            {
                Point3D newNozzlePoint = GetPositionPoint(refPoint, CommonMethod.PositionToEnum(eachNozzle.LR), eachNozzle.HRSort, 0, sizeNominalId, centerTopHeight);
                NozzlePointList.Add(newNozzlePoint);
            }

            // Nozzle : Create Model
            foreach (NozzleInputModel eachNozzle in drawArrangeNozzle)
            {
                List<Entity> customEntity = CreateNozzleModelPosition(CommonMethod.PositionToEnum( eachNozzle.LR), eachNozzle.HRSort, sizeNominalId,centerTopHeight, refPoint);
                // Create Model : OutLine
                nozzleEntities.outlineList.AddRange(customEntity);
            }


            // Nozzel Mark Point List : Create Mark Position Arrangement 
            List<Point3D> leaderMarkPointList = GetArrangeLeaderMarkPosition(  drawArrangeNozzle, selLeaderCircleSize, multiColumnValue, sizeNominalId, centerTopHeight, shellSpacing, refPoint);

            // Nozzle : Create Mark
            int indexArrange = -1;
            foreach (NozzleInputModel eachNozzle in drawArrangeNozzle)
            {
                indexArrange++;
                Point3D drawPoint = leaderMarkPointList[indexArrange];
                Dictionary<string, List<Entity>> customEntity = CreateNozzleLeader(drawPoint, CommonMethod.PositionToEnum(eachNozzle.LR),eachNozzle.Mark,eachNozzle.Size,selNozzleFontSize, selLeaderCircleSize);
                nozzleEntities.nozzleMarkList.AddRange(customEntity[CommonGlobal.NozzleMark]);
                nozzleEntities.nozzleTextList.AddRange(customEntity[CommonGlobal.NozzleText]);

            }

            // Line
            List<Entity> customLineEntity = CreateNozzleLeaderLine(refPoint, shellSpacing, drawArrangeNozzle, leaderMarkPointList, NozzlePointList);
            nozzleEntities.nozzlelineList.AddRange(customLineEntity);



            return nozzleEntities;
        }

        #region Nozzel : Create Model
        private List<Entity> CreateNozzleModelPosition(POSITION_TYPE selNozzlePosition,double selHeightSort,double selSizeNominalID,double selCenterTopHeight, CDPoint refPoint)
        {
            Point3D drawPoint = GetPositionPoint(refPoint, selNozzlePosition, selHeightSort,0, selSizeNominalID, selCenterTopHeight);
            List<Entity> newNozzle = null;
            switch (selNozzlePosition)
            {
                case POSITION_TYPE.LEFT:
                    newNozzle = CreateNozzleModel(drawPoint,POSITION_TYPE.LEFT);
                    break;
                case POSITION_TYPE.RIGHT:
                    newNozzle = CreateNozzleModel(drawPoint,POSITION_TYPE.RIGHT);
                    break;
                case POSITION_TYPE.TOP:
                    newNozzle = CreateNozzleModel(drawPoint,POSITION_TYPE.TOP);
                    break;
                case POSITION_TYPE.BOTTOM:
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
            Point3D adjPoint = (Point3D)drawPoint.Clone();
            adjPoint.X = drawPoint.X - fullWidth;
            adjPoint.Y = drawPoint.Y - (fullHeight/2);

            // Model
            Line lineFFa = new Line(GetSumPoint(adjPoint, 0, 0, 0), GetSumPoint(adjPoint, 0, flangeFaceHeight, 0));
            Line lineFFb = new Line(GetSumPoint(adjPoint, flangeFaceWidth, 0, 0), GetSumPoint(adjPoint, flangeFaceWidth, flangeFaceHeight, 0));
            Line lineFFc = new Line(GetSumPoint(adjPoint, 0, 0, 0), GetSumPoint(adjPoint, flangeFaceWidth, 0, 0));
            Line lineFFd = new Line(GetSumPoint(adjPoint, 0, flangeFaceHeight, 0), GetSumPoint(adjPoint, flangeFaceWidth, flangeFaceHeight, 0));

            Line lineFPa = new Line(GetSumPoint(adjPoint, flangeFaceWidth, flangeFaceInnerWidth + flangePipeHeight, 0), GetSumPoint(adjPoint, flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth + pipeHeight, 0));
            Line lineFPb = new Line(GetSumPoint(adjPoint, flangeFaceWidth, flangeFaceInnerWidth, 0), GetSumPoint(adjPoint, flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0));
            Line lineFPc = new Line(GetSumPoint(adjPoint, flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth + pipeHeight, 0), GetSumPoint(adjPoint, flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0));

            Line linePa = new Line(GetSumPoint(adjPoint, flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0), GetSumPoint(adjPoint, flangeFaceWidth + flangePipeWidth + pipeWidth, flangePipeInnerWidth, 0));
            Line linePb = new Line(GetSumPoint(adjPoint, flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth + pipeHeight, 0), GetSumPoint(adjPoint, flangeFaceWidth + flangePipeWidth + pipeWidth, flangePipeInnerWidth + pipeHeight, 0));

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

        #region Nozzle : Arrange Leader Mark Position
        // 노즐 지시 겹칩 가능
        // Left 영역만 구현
        private List<Point3D> GetArrangeLeaderMarkPosition( List<NozzleInputModel> drawArrangeNozzle, string selLeaderCircleSize, bool multiColumnValue, double selSizeNominalID, double selCenterTopHeight, DrawPositionValueModel selShellSpacing, CDPoint refPoint)
        {
            double arrangeGapHeight = 40;
            double arrangeMaxHeightValue = 0;
            double circleSize = valueService.GetDoubleValue(selLeaderCircleSize);

            // convergence
            double convergenceHeight = 100;
            bool convergenceValue = false;
            double convergenceXY = 0;

            DrawPositionValueModel arrangeMaxHeight = new DrawPositionValueModel();

            DrawPositionValueModel beforeValue = new DrawPositionValueModel();
            DrawPositionValueModel convergencePosition = new DrawPositionValueModel();

            DrawPositionValueModel positionCount = new DrawPositionValueModel();

            double positionCountValue = 0;

            List<Point3D> leaderArrangementHeight = new List<Point3D>();
            foreach (NozzleInputModel eachNozzle in drawArrangeNozzle)
            {

                POSITION_TYPE eachNozzlePosition = CommonMethod.PositionToEnum(eachNozzle.LR);

                // New Value
                double newHeight = eachNozzle.HRSort;

                // Convergence & Before Value
                switch (eachNozzlePosition)
                {
                    case POSITION_TYPE.LEFT:
                        convergenceValue = GetConvergenceValue(newHeight, beforeValue.Left, convergenceHeight);
                        beforeValue.Left = eachNozzle.HRSort;
                        if (multiColumnValue && convergenceValue)
                            convergencePosition.Left -= circleSize;
                        else
                            convergencePosition.Left = 0;

                        positionCount.Left++;
                        positionCountValue = positionCount.Left;
                        convergenceXY = convergencePosition.Left;
                        arrangeMaxHeightValue = arrangeMaxHeight.Left;
                        break;

                    case POSITION_TYPE.RIGHT:
                        convergenceValue = GetConvergenceValue(newHeight, beforeValue.Right, convergenceHeight);
                        beforeValue.Right = eachNozzle.HRSort;
                        if (multiColumnValue && convergenceValue)
                            convergencePosition.Right += circleSize;
                        else
                            convergencePosition.Right = 0;

                        positionCount.Right++;
                        positionCountValue = positionCount.Right;
                        convergenceXY = convergencePosition.Right;
                        arrangeMaxHeightValue = arrangeMaxHeight.Right;

                        break;

                    case POSITION_TYPE.TOP:
                        convergenceValue = GetConvergenceValue(newHeight, beforeValue.Top, convergenceHeight);
                        beforeValue.Top = eachNozzle.HRSort;
                        if (multiColumnValue && convergenceValue)
                            convergencePosition.Top += circleSize;
                        else
                            convergencePosition.Top = 0;

                        positionCount.Top++;
                        positionCountValue = positionCount.Top;
                        convergenceXY = convergencePosition.Top;
                        arrangeMaxHeightValue = arrangeMaxHeight.Top;
                        break;

                    case POSITION_TYPE.BOTTOM:
                        convergenceValue = GetConvergenceValue(newHeight, beforeValue.Bottom, convergenceHeight);
                        beforeValue.Bottom = eachNozzle.HRSort;
                        if (multiColumnValue && convergenceValue)
                            convergencePosition.Bottom -= circleSize;
                        else
                            convergencePosition.Bottom = 0;

                        positionCount.Bottom++;
                        positionCountValue = positionCount.Bottom;
                        convergenceXY = convergencePosition.Bottom ;
                        arrangeMaxHeightValue = arrangeMaxHeight.Bottom;
                        break;
                }

                Point3D convPointXY;

                if (multiColumnValue && convergenceValue)
                {
                    // Convergence
                    convPointXY = GetPositionSpacingPoint(refPoint, eachNozzlePosition, 
                                                                 arrangeMaxHeightValue - circleSize / 2 - arrangeGapHeight,
                                                                 convergenceXY,
                                                                 selSizeNominalID, selCenterTopHeight, selShellSpacing);

                }
                else
                {
                    // Not Convergence
                    if (positionCountValue == 1)
                    {
                        // First
                        convPointXY = GetPositionSpacingPoint(refPoint, eachNozzlePosition,
                                                                    newHeight,
                                                                    convergenceXY,
                                                                    selSizeNominalID, selCenterTopHeight, selShellSpacing);
                    }
                    else
                    {
                        double currentLower = newHeight - circleSize / 2;
                        if (arrangeMaxHeightValue > currentLower)
                            newHeight = arrangeMaxHeightValue + circleSize / 2;

                        convPointXY = GetPositionSpacingPoint(refPoint, eachNozzlePosition, 
                                                                     newHeight,
                                                                     convergenceXY,
                                                                     selSizeNominalID, selCenterTopHeight, selShellSpacing);

                    }
                    
                    // Arrange Height
                    switch (eachNozzlePosition)
                    {
                        case POSITION_TYPE.LEFT:
                            arrangeMaxHeight.Left = newHeight + circleSize / 2 + arrangeGapHeight;
                            
                            break;
                        case POSITION_TYPE.RIGHT:
                            arrangeMaxHeight.Right = newHeight + circleSize / 2 + arrangeGapHeight;
                            arrangeMaxHeightValue = arrangeMaxHeight.Right;
                            break;
                        case POSITION_TYPE.TOP:
                            arrangeMaxHeight.Top = newHeight + circleSize / 2 + arrangeGapHeight;
                            arrangeMaxHeightValue = arrangeMaxHeight.Top;
                            break;
                        case POSITION_TYPE.BOTTOM:
                            arrangeMaxHeight.Bottom = newHeight + circleSize / 2 + arrangeGapHeight;
                            arrangeMaxHeightValue = arrangeMaxHeight.Bottom;
                            break;
                    }
                    

                }

                leaderArrangementHeight.Add(convPointXY);

            }

            return leaderArrangementHeight;
        }
        private bool GetConvergenceValue(double newValue, double beforeValue,double convergenceHeight)
        {
            bool returnValue = false;
            if (convergenceHeight > 0)
                if (newValue - beforeValue <= convergenceHeight)
                    returnValue = true;

            return returnValue;
        }

        #endregion

        #region Nozzle : Create Mark
        private Dictionary<string, List<Entity>> CreateNozzleLeader(Point3D refPoint, POSITION_TYPE selPosition, string textUpperStr, string textLowerStr, string fontSize, string circleSize)
        {
            // Current Test
            double cirDiameter = valueService.GetDoubleValue(circleSize);
            double cirRadius = cirDiameter / 2;
            double cirTextSize = valueService.GetDoubleValue(fontSize);

            // Point Adj
            Point3D drawPoint = new Point3D();
            switch (selPosition)
            {
                case POSITION_TYPE.LEFT:
                    drawPoint.X = refPoint.X - cirDiameter;
                    drawPoint.Y = refPoint.Y - cirRadius;
                    break;
                case POSITION_TYPE.RIGHT:
                    drawPoint.X = refPoint.X ;
                    drawPoint.Y = refPoint.Y - cirRadius;
                    break;
                case POSITION_TYPE.TOP:
                    drawPoint.X = refPoint.X - cirRadius;
                    drawPoint.Y = refPoint.Y + cirDiameter;
                    break;
                case POSITION_TYPE.BOTTOM:
                    drawPoint.X = refPoint.X - cirRadius;
                    drawPoint.Y = refPoint.Y - cirDiameter;
                    break;
            }

            Circle circleCenter = new Circle(GetSumPoint(drawPoint, cirRadius, cirRadius, 0), cirRadius);
            Line lineCenter = new Line(GetSumPoint(drawPoint, 0, cirRadius, 0), GetSumPoint(drawPoint, cirDiameter, cirRadius, 0));
            Text textUpper = new Text(GetSumPoint(drawPoint, cirRadius, cirRadius + (cirRadius / 2), 0), textUpperStr, cirTextSize);
            textUpper.Alignment = Text.alignmentType.MiddleCenter;
            Text textLower = new Text(GetSumPoint(drawPoint, cirRadius, (cirRadius / 2), 0), textLowerStr, cirTextSize);
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

        #endregion

        #region Nozzle : Create Leader Line
        private List<Entity> CreateNozzleLeaderLine(CDPoint refPoint, DrawPositionValueModel selShellSpacing, List<NozzleInputModel> selNozzle, List<Point3D> selLeaderMarkPoint, List<Point3D> selNozzleStartPoint)
        {

            Point3D drawPoint = new Point3D(refPoint.X, refPoint.Y);

            // Entity
            List<Entity> customEntity = new List<Entity>();


            double midPointXGap = 200;
            
            // 노즐 크기에 따라 변경 되어야 함
            double nozzleLength = (10 + 24 + 40);// 나중에 가변으로 변경 해야 함


            DrawPositionValueModel strPoint = new DrawPositionValueModel();
            DrawPositionValueModel endPoint = new DrawPositionValueModel();
            DrawPositionValueModel midPoint = new DrawPositionValueModel();

            // Mid Point
            midPoint.Left = -99999999;
            midPoint.Right = 99999999;
            midPoint.Top = -99999999;
            midPoint.Bottom = 99999999;

            // Convergence Check : 높이가 같을 경우
            DrawPositionValueModel beforeEndPoint = new DrawPositionValueModel();
            int indexArrange = -1;

            foreach(NozzleInputModel eachNozzle in selNozzle)
            {
                indexArrange++;

                Point3D nozzlePoint = selNozzleStartPoint[indexArrange];
                Point3D markPoint = selLeaderMarkPoint[indexArrange];

                POSITION_TYPE eachNozzlePosition = CommonMethod.PositionToEnum(eachNozzle.LR);
                switch (eachNozzlePosition)
                {
                    case POSITION_TYPE.LEFT:
                        if (nozzlePoint.Y == markPoint.Y)
                        {
                            // 직선
                            customEntity.AddRange(CreateBrokenLIne(GetSumPoint(nozzlePoint,-nozzleLength,0), markPoint));
                            
                            if(midPoint.Left< markPoint.X)
                                midPoint.Left = markPoint.X;
                        }
                        else if (nozzlePoint.Y > markPoint.Y)
                        {

                        }
                        else if (nozzlePoint.Y < markPoint.Y)
                        {
                            // 위로
                            if (beforeEndPoint.Left != markPoint.Y)
                                midPoint.Left += midPointXGap; // 오른쪽으로 이동
                            customEntity.AddRange(CreateBrokenLIne(GetSumPoint(nozzlePoint,-nozzleLength,0), markPoint, new Point3D(midPoint.Left, nozzlePoint.Y), new Point3D(midPoint.Left, markPoint.Y)));

                        }
                        beforeEndPoint.Left = markPoint.Y;
                        break;

                    case POSITION_TYPE.RIGHT:
                        if (nozzlePoint.Y == markPoint.Y)
                        {
                            // 직선
                            customEntity.AddRange(CreateBrokenLIne(GetSumPoint(nozzlePoint,+nozzleLength,0), markPoint));

                            if (midPoint.Right > markPoint.X)
                                midPoint.Right = markPoint.X;
                        }
                        else if (nozzlePoint.Y > markPoint.Y)
                        {

                        }
                        else if (nozzlePoint.Y < markPoint.Y)
                        {
                            // 위로
                            if (beforeEndPoint.Right != markPoint.Y)
                                midPoint.Right -= midPointXGap; // 왼쪽으로 이동
                            customEntity.AddRange(CreateBrokenLIne(GetSumPoint(nozzlePoint,+nozzleLength,0), markPoint, new Point3D(midPoint.Right, nozzlePoint.Y), new Point3D(midPoint.Right, markPoint.Y)));

                        }
                        beforeEndPoint.Right = markPoint.Y;
                        break;

                    case POSITION_TYPE.TOP:
                        if (nozzlePoint.X == markPoint.X)
                        {
                            // 직선
                            customEntity.AddRange(CreateBrokenLIne(GetSumPoint(nozzlePoint,0,+nozzleLength), markPoint));
                            if (midPoint.Top > markPoint.Y)
                                midPoint.Top = markPoint.Y;
                        }
                        else if (nozzlePoint.X > markPoint.X)
                        {

                        }
                        else if (nozzlePoint.X < markPoint.X)
                        {
                            // 위로
                            if (beforeEndPoint.Top != markPoint.X)
                                midPoint.Top -= midPointXGap; // 아래쪽으로 이동
                            customEntity.AddRange(CreateBrokenLIne(GetSumPoint(nozzlePoint,0,+nozzleLength), markPoint, new Point3D(nozzlePoint.X, midPoint.Top), new Point3D(markPoint.X, midPoint.Top)));

                        }
                        beforeEndPoint.Top = markPoint.X;
                        break;
                        
                    case POSITION_TYPE.BOTTOM:
                        if (nozzlePoint.X == markPoint.X)
                        {
                            // 직선
                            customEntity.AddRange(CreateBrokenLIne(GetSumPoint(nozzlePoint,0,-nozzleLength), markPoint));
                            if (midPoint.Bottom < markPoint.Y)
                                midPoint.Bottom = markPoint.Y;
                        }
                        else if (nozzlePoint.X > markPoint.X)
                        {

                        }
                        else if (nozzlePoint.X < markPoint.X)
                        {
                            // 위로
                            if (beforeEndPoint.Bottom != markPoint.X)
                                midPoint.Bottom += midPointXGap; // 위쪽으로 이동
                            customEntity.AddRange(CreateBrokenLIne(GetSumPoint(nozzlePoint,0,-nozzleLength), markPoint, new Point3D(nozzlePoint.X, midPoint.Bottom), new Point3D(markPoint.X, midPoint.Bottom)));

                        }
                        beforeEndPoint.Bottom = markPoint.X;
                        break;
                }



              
            }


            return customEntity;
        }
        private List<Entity> CreateBrokenLIne(Point3D startPoint, Point3D endPoint, Point3D midPoint1=null, Point3D midPoint2 = null)
        {
            List<Entity> newEntityList = new List<Entity>();
            if (midPoint1 == null)
            {
                Line lineOne = new Line(startPoint,endPoint);
                newEntityList.Add(lineOne);
            }
            else
            {
                Line lineUpperOne = new Line(startPoint, midPoint1);
                Line lineUpperTwo = new Line(midPoint1, midPoint2);
                Line lineUpperThr = new Line(midPoint2, endPoint);
                newEntityList.Add(lineUpperOne);
                newEntityList.Add(lineUpperTwo);
                newEntityList.Add(lineUpperThr);
            }

            return newEntityList;
        }

        #endregion

        #region Common : Position
        private Point3D GetPositionPoint(CDPoint refPoint, POSITION_TYPE selPosition, double newValue,double convergenceValue, double selSizeNominalID, double selCenterTopHeight)
        {
            Point3D newPoint = null;
            CDPoint adjPoint = null;
            CDPoint curPoint = null;
            switch (selPosition)
            {
                case POSITION_TYPE.LEFT:
                    adjPoint = contactPointService.ContactPoint("leftshelladj", newValue.ToString(),ref refPoint,ref curPoint);
                    newPoint = new Point3D(adjPoint.X + convergenceValue, adjPoint.Y, 0);
                    break;
                case POSITION_TYPE.RIGHT:
                    adjPoint = contactPointService.ContactPoint("rightshelladj", newValue.ToString(), ref refPoint, ref curPoint);
                    newPoint = new Point3D(adjPoint.X + convergenceValue, adjPoint.Y, 0);
                    break;
                case POSITION_TYPE.TOP:
                    newPoint = new Point3D(refPoint.X + newValue, selCenterTopHeight + convergenceValue, 0);
                    break;
                case POSITION_TYPE.BOTTOM:
                    newPoint = new Point3D(refPoint.X + newValue, refPoint.Y + convergenceValue, 0);
                    break;
            }
            return newPoint;
        }
        private Point3D GetPositionSpacingPoint(CDPoint refPoint, POSITION_TYPE selPosition, double newValue,double convergenceValue, double selSizeNominalID, double selCenterTopHeight, DrawPositionValueModel selShellSpacing)
        {
            Point3D newPoint = newPoint = GetPositionPoint(refPoint, selPosition, newValue, convergenceValue, selSizeNominalID, selCenterTopHeight);
            switch (selPosition)
            {
                case POSITION_TYPE.LEFT:
                    newPoint.X -= selShellSpacing.Left;
                    break;
                case POSITION_TYPE.RIGHT:
                    newPoint.X += selShellSpacing.Right;
                    break;
                case POSITION_TYPE.TOP:
                    newPoint.Y += selShellSpacing.Top;
                    break;
                case POSITION_TYPE.BOTTOM:
                    newPoint.Y -= selShellSpacing.Bottom;
                    break;
            }
            return newPoint;
        }
        #endregion

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
    }
}
