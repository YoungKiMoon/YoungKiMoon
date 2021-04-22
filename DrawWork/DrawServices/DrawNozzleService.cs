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
using DrawWork.DrawCustomObjectModels;

namespace DrawWork.DrawServices
{
    public class DrawNozzleService
    {
        private AssemblyModel assemblyData;

        private ValueService valueService;
        private DrawWorkingPointService workingPointService;

        public DrawNozzleService(AssemblyModel selAssembly)
        {
            assemblyData = selAssembly;

            valueService = new ValueService();
            workingPointService = new DrawWorkingPointService(selAssembly);
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
            CDPoint centerTopPoint = workingPointService.ContactPoint(WORKINGPOINT_TYPE.PointCenterTopDown, ref refPoint, ref newCurPoint);
            double centerTopHeight = centerTopPoint.Y;


            // MutilColumn
            bool multiColumnValue = selMultiColumn == "true" ? true : false;



            // Nozzle List : Adjust : Sort Value
            List<NozzleInputModel> drawNozzle = new List<NozzleInputModel>();
            foreach(NozzleInputModel eachNozzle in assemblyData.NozzleInputModel)
            {
                eachNozzle.Position = eachNozzle.Position.ToLower();
                eachNozzle.LR = eachNozzle.LR.ToLower();
                eachNozzle.Type = eachNozzle.Type.ToLower();
                eachNozzle.Facing = eachNozzle.Facing.ToLower();

                eachNozzle.ReinforcingPadType = eachNozzle.ReinforcingPadType.ToLower();
                eachNozzle.InletOutlet = eachNozzle.InletOutlet.ToLower();
                eachNozzle.Interal = eachNozzle.Interal.ToLower();
                eachNozzle.Sump = eachNozzle.Sump.ToLower();
                eachNozzle.OtherFlange = eachNozzle.OtherFlange.ToLower();

                // Sort Value
                if (eachNozzle.Position=="shell")
                    eachNozzle.HRSort = valueService.GetDoubleValue(eachNozzle.H);
                else if (eachNozzle.Position == "roof")
                    eachNozzle.HRSort = valueService.GetDoubleValue(eachNozzle.R);


                eachNozzle.LR = eachNozzle.LR.ToLower();
                drawNozzle.Add(eachNozzle);
            }

            // Nozzle List : Sort
            List<NozzleInputModel> drawArrangeNozzle = drawNozzle.OrderBy(x => x.HRSort).ThenBy(x=>x.LR).ThenBy(x=>x.Position).ToList();

            // Nozzle Start Point List : Nozzle
            List<Point3D> NozzlePointList = new List<Point3D>();

            // Nozzle Start Point List : Line
            List<Point3D> NozzleLinePointList = new List<Point3D>();

            // Nozzle : Create Model
            foreach (NozzleInputModel eachNozzle in drawArrangeNozzle)
            {
                // Start Point
                Point3D newNozzlePoint = GetPositionPoint(refPoint, eachNozzle.Position, eachNozzle.LR, eachNozzle.HRSort, 0, sizeNominalId, centerTopHeight, shellSpacing);
                NozzlePointList.Add(newNozzlePoint);

                Point3D newNozzleLinePoint =GetPositionLinePoint(refPoint,newNozzlePoint,eachNozzle, eachNozzle.Position, eachNozzle.LR, eachNozzle.HRSort, 0, sizeNominalId, centerTopHeight, shellSpacing);
                NozzleLinePointList.Add(newNozzleLinePoint);
                //List<Entity> customEntity = CreateNozzleModelPosition(refPoint, newNozzlePoint, eachNozzle, sizeNominalId,centerTopHeight, shellSpacing);
                List<Entity> customEntity = CreateFlangeAll(refPoint, newNozzlePoint, eachNozzle, sizeNominalId, centerTopHeight);

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
                Dictionary<string, List<Entity>> customEntity = CreateNozzleLeader(drawPoint, eachNozzle, selNozzleFontSize, selLeaderCircleSize);
                nozzleEntities.nozzleMarkList.AddRange(customEntity[CommonGlobal.NozzleMark]);
                nozzleEntities.nozzleTextList.AddRange(customEntity[CommonGlobal.NozzleText]);

            }

            // Nozzle : Create Line
            List<Entity> customLineEntity = CreateNozzleLeaderLine(refPoint, shellSpacing, drawArrangeNozzle, leaderMarkPointList, NozzleLinePointList);
            //List<Entity> customLineEntity = CreateNozzleLeaderLine(refPoint, shellSpacing, drawArrangeNozzle, leaderMarkPointList, NozzlePointList);
            nozzleEntities.nozzlelineList.AddRange(customLineEntity);


           // CDPoint ccc = workingPointService.ContactPoint("topangleroofpoint",ref refPoint,ref newCurPoint);
            //Line lineref01 = new Line(new Point3D(ccc.X,ccc.Y),new Point3D(ccc.X,ccc.Y+1000));
            //nozzleEntities.outlineList.Add(lineref01);

            //CDPoint cccc = workingPointService.ContactPoint("leftroofpoint", ref refPoint, ref newCurPoint);
            //Line lineref02 = new Line(new Point3D(cccc.X, cccc.Y), new Point3D(cccc.X, cccc.Y + 1000));
            //nozzleEntities.outlineList.Add(lineref02);

            //CDPoint ccccc = workingPointService.ContactPoint("leftroofpoint", "70", ref refPoint, ref newCurPoint);
            //CDPoint ccccc = workingPointService.ContactPoint("leftroofpoint", "3793.6", ref refPoint, ref newCurPoint);
            //Line lineref03 = new Line(new Point3D(ccccc.X, ccccc.Y), new Point3D(ccccc.X, ccccc.Y + 1000));
            //nozzleEntities.outlineList.Add(lineref03);

            return nozzleEntities;
        }

        #region Nozzel : Create Model : Sample
        private List<Entity> CreateNozzleModelPosition(CDPoint refPoint, Point3D startPoint, NozzleInputModel selNozzle, double selSizeNominalID, double selCenterTopHeight, DrawPositionValueModel selShellSpacing)
        {



            //Point3D drawPoint = GetPositionPoint(refPoint, selNozzle.Position, selNozzle.LR, selNozzle.HRSort,0, selSizeNominalID, selCenterTopHeight, selShellSpacing);

            // Sample
             List<Entity> newNozzle =  CreateNozzleModel(refPoint, startPoint, selNozzle);

            //List<Entity> newNozzle = CreateFlangeAll(refPoint, startPoint, selNozzle);

            //List<Entity> newNozzle = new List<Entity>();
            return newNozzle;
        }

        // Sample Nozzle
        private List<Entity> CreateNozzleModel(CDPoint refPoint, Point3D drawPoint, NozzleInputModel selNozzle)
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
            adjPoint.Y = drawPoint.Y - (fullHeight / 2);

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
            switch (selNozzle.Position)
            {
                case "shell":
                    switch (selNozzle.LR)
                    {
                        case "left":
                            break;
                        case "right":
                            foreach (Entity eachEntity in customEntity)
                                eachEntity.Rotate(UtilityEx.DegToRad(-180), Vector3D.AxisZ, drawPoint);
                            break;
                    }
                    break;
                case "roof":
                    switch (selNozzle.LR)
                    {
                        case "left":
                        case "right":
                            foreach (Entity eachEntity in customEntity)
                                eachEntity.Rotate(UtilityEx.DegToRad(-90), Vector3D.AxisZ, drawPoint);
                            break;
                    }
                    break;
            }


            return customEntity;
        }


        #endregion


        #region Nozzel : Create Model



        // Division : Flange Style
        private List<Entity> CreateFlangeAll(CDPoint refPoint, Point3D drawPoint, NozzleInputModel selNozzle, double selSizeNominalID, double selCenterTopHeight)
        {
            List<Entity> customEntity = new List<Entity>();


            if(selNozzle.OtherFlange.Contains("series a"))
            {
                if (selNozzle.Rating.Contains("150"))
                {
                    NozzleOHFSeriesAModel newNozzle = null;
                    foreach (NozzleOHFSeriesAModel eachNozzle in assemblyData.NozzleOHFSeriesAList)
                    {
                        if (eachNozzle.NPS == selNozzle.Size) 
                        {
                            newNozzle = eachNozzle;
                            customEntity=CreateFlangeSeriesA(refPoint,drawPoint, selNozzle, newNozzle,selSizeNominalID,selCenterTopHeight);
                            break;
                        }
                    }
                }
                else if (selNozzle.Rating.Contains("300"))
                {
                    NozzleTHSeriesAModel newNozzle = null;
                    foreach (NozzleTHSeriesAModel eachNozzle in assemblyData.NozzleTHSeriesAist)
                    {
                        if (eachNozzle.NPS == selNozzle.Size)
                        {
                            newNozzle = eachNozzle;
                            customEntity=CreateFlangeSeriesA(refPoint, drawPoint, selNozzle, TransNozzleATHtoOHF(newNozzle), selSizeNominalID, selCenterTopHeight);
                            break;
                        }
                    }
                }
            }
            else if (selNozzle.OtherFlange.Contains("series b"))
            {
                if (selNozzle.Rating.Contains("150"))
                {
                    NozzleOHFSeriesBModel newNozzle = null;
                    foreach (NozzleOHFSeriesBModel eachNozzle in assemblyData.NozzleOHFSeriesBList)
                    {
                        if (eachNozzle.NPS == selNozzle.Size)
                        {
                            newNozzle = eachNozzle;
                            customEntity = CreateFlangeSeriesB(refPoint, drawPoint, selNozzle, newNozzle, selSizeNominalID, selCenterTopHeight);
                            break;
                        }
                    }
                }
                else if (selNozzle.Rating.Contains("300"))
                {
                    NozzleTHSeriesBModel newNozzle = null;
                    foreach (NozzleTHSeriesBModel eachNozzle in assemblyData.NozzleTHSeriesBist)
                    {
                        if (eachNozzle.NPS == selNozzle.Size)
                        {
                            newNozzle = eachNozzle;
                            customEntity = CreateFlangeSeriesB(refPoint, drawPoint, selNozzle, TransNozzleBTHtoOHF(newNozzle), selSizeNominalID, selCenterTopHeight);
                            break;
                        }
                    }
                }
            }
            else if (selNozzle.Type.Contains("lwn"))
            {
                if (selNozzle.Rating.Contains("150"))
                {
                    NozzleOHFLWNModel newNozzle = null;
                    foreach (NozzleOHFLWNModel eachNozzle in assemblyData.NozzleOHFLWNList)
                    {
                        if (eachNozzle.NPS == selNozzle.Size)
                        {
                            newNozzle = eachNozzle;
                            customEntity = CreateFlangeLWN(refPoint, drawPoint, selNozzle, newNozzle, selSizeNominalID, selCenterTopHeight);
                            break;
                        }
                    }
                }
                else if (selNozzle.Rating.Contains("300"))
                {
                    NozzleTHLWNModel newNozzle = null;
                    foreach (NozzleTHLWNModel eachNozzle in assemblyData.NozzleTHLWNList)
                    {
                        if (eachNozzle.NPS == selNozzle.Size)
                        {
                            newNozzle = eachNozzle;
                            customEntity = CreateFlangeLWN(refPoint, drawPoint, selNozzle, TransNozzleLWNTHtoOHF(newNozzle), selSizeNominalID, selCenterTopHeight);
                            break;
                        }
                    }
                }
            }
            else
            {
                if (selNozzle.Rating.Contains("150"))
                {
                    NozzleOHFModel newNozzle = null;
                    foreach (NozzleOHFModel eachNozzle in assemblyData.NozzleOHFList)
                    {
                        if (eachNozzle.NPS == selNozzle.Size)
                        {
                            newNozzle = eachNozzle;
                            customEntity = CreateFlange(refPoint, drawPoint, selNozzle, newNozzle, selSizeNominalID, selCenterTopHeight);
                            break;
                        }
                    }
                }
                else if (selNozzle.Rating.Contains("300"))
                {
                    NozzleTHModel newNozzle = null;
                    foreach (NozzleTHModel eachNozzle in assemblyData.NozzleTHList)
                    {
                        if (eachNozzle.NPS == selNozzle.Size)
                        {
                            newNozzle = eachNozzle;
                            customEntity = CreateFlange(refPoint, drawPoint, selNozzle, TransNozzleTHtoOHF(newNozzle), selSizeNominalID, selCenterTopHeight);
                            break;
                        }
                    }
                }
            }


            // Rotation
            switch (selNozzle.Position)
            {
                case "shell":
                    switch (selNozzle.LR)
                    {
                        case "left":
                            break;
                        case "right":
                            foreach (Entity eachEntity in customEntity)
                                eachEntity.Rotate(UtilityEx.DegToRad(-180), Vector3D.AxisZ, drawPoint);
                            break;
                    }
                    break;
                case "roof":
                    switch (selNozzle.LR)
                    {
                        case "left":
                        case "right":
                            foreach (Entity eachEntity in customEntity)
                                if(eachEntity.EntityData!=null)
                                    eachEntity.Rotate(UtilityEx.DegToRad(-90), Vector3D.AxisZ, drawPoint);
                                else
                                    eachEntity.Rotate(UtilityEx.DegToRad(-90), Vector3D.AxisZ, drawPoint);
                            break;
                    }
                    break;
            }



            return customEntity;
        }
        private double GetNeckLength(CDPoint refPoint, Point3D drawPoint, NozzleInputModel selNozzle, double selSizeNominalID, double selCenterTopHeight)
        {
            double neckLength = 0;
            double R = valueService.GetDoubleValue(selNozzle.R);
            double H = valueService.GetDoubleValue(selNozzle.H);
            double shellThickness = 0;

            // Roof Slope

            double roofPointHeight = drawPoint.Y - refPoint.Y;



            if (selNozzle.Position == "shell")
            {
                if (selNozzle.LR == "left")
                {
                    shellThickness = workingPointService.GetShellThicknessAccordingToHeight(selNozzle.H);
                    neckLength = R - (selSizeNominalID / 2) - shellThickness;
                }
                else if (selNozzle.LR == "right")
                {
                    shellThickness = workingPointService.GetShellThicknessAccordingToHeight(selNozzle.H);
                    neckLength = R - (selSizeNominalID / 2) - shellThickness;
                }
            }
            else if (selNozzle.Position == "roof")
            {
                if (selNozzle.LR == "left")
                {
                    neckLength = H - roofPointHeight ;
                }
                else if (selNozzle.LR == "right")
                {
                    neckLength = H - roofPointHeight;
                }
            }
            return neckLength;
        }
        private List<Entity> CreateFlangeSeriesA(CDPoint refPoint, Point3D drawPoint,NozzleInputModel selNozzle,NozzleOHFSeriesAModel drawNozzle, double selSizeNominalID, double selCenterTopHeight)
        {
            List<Entity> customEntity = new List<Entity>();

            double G = valueService.GetDoubleValue(drawNozzle.G);
            double OD = valueService.GetDoubleValue(drawNozzle.OD);
            double BCD = valueService.GetDoubleValue(drawNozzle.BCD);
            double R = 0;
            double RRF = valueService.GetDoubleValue(drawNozzle.RRF);
            double RFF = valueService.GetDoubleValue(drawNozzle.RFF);
            double H = valueService.GetDoubleValue(drawNozzle.H);
            double A = valueService.GetDoubleValue(drawNozzle.A);
            double BWN = valueService.GetDoubleValue(drawNozzle.BWN);
            double BBF = valueService.GetDoubleValue(drawNozzle.BBF);
            double C = valueService.GetDoubleValue(drawNozzle.C);

            double gasketT = valueService.GetDoubleValue(assemblyData.NozzleEtcList[0].GasketThickness);

            // Facing
            if (selNozzle.Facing == "rf")
                R = RRF;
            if (selNozzle.Facing == "ff")
                R = RFF;

            // Pipe Thickness
            double pipeThickness = GetPipeThickness(selNozzle.Size);

            // Neck : adjust
            double neckLengthOrigin = GetNeckLength(refPoint, drawPoint, selNozzle, selSizeNominalID, selCenterTopHeight);
            double neckLengthReal = neckLengthOrigin - (BBF + gasketT + A);

            // 전체 크기
            double fullHeight = OD;
            double fullWidth = neckLengthOrigin;



            double sideWidth = (OD - R) / 2;
            double sideWidth2 = (OD - H) / 2;
            double sideWidth3 = (OD - G) / 2;

            // Point Adj : 좌측 상단으로 이동 하여 그림
            Point3D adjPoint = (Point3D)drawPoint.Clone();
            adjPoint.X = drawPoint.X - fullWidth;
            adjPoint.Y = drawPoint.Y + (fullHeight / 2);


            // Blind Flange
            Line lineBFa = new Line(GetSumPoint(adjPoint, 0, 0, 0), GetSumPoint(adjPoint, 0, -OD, 0));

            Line lineBFb = new Line(GetSumPoint(adjPoint, 0, 0, 0), GetSumPoint(adjPoint, BBF, 0, 0));
            Line lineBFc = new Line(GetSumPoint(adjPoint, 0, -OD, 0), GetSumPoint(adjPoint, BBF, -OD, 0));

            Line lineBFd = new Line(GetSumPoint(adjPoint, BBF,0 , 0), GetSumPoint(adjPoint, BBF, -OD, 0));
            //Line lineBFd = new Line(GetSumPoint(adjPoint, BBF, 0, 0), GetSumPoint(adjPoint, BBF, -sideWidth, 0));
            //Line lineBFe = new Line(GetSumPoint(adjPoint, BBF, -OD, 0), GetSumPoint(adjPoint, BBF, -OD + sideWidth, 0));

            Line lineBFf = new Line(GetSumPoint(adjPoint, BBF + C, -sideWidth, 0), GetSumPoint(adjPoint, BBF, -sideWidth, 0));
            Line lineBFg = new Line(GetSumPoint(adjPoint, BBF + C, -OD + sideWidth, 0), GetSumPoint(adjPoint, BBF, -OD + sideWidth, 0));
            Line lineBFh = new Line(GetSumPoint(adjPoint, BBF + C, -sideWidth, 0), GetSumPoint(adjPoint, BBF + C, -OD + sideWidth, 0));
            customEntity.AddRange(new Entity[] { lineBFa, lineBFb, lineBFc, lineBFd, lineBFf, lineBFg, lineBFh });


            // Gasket
            Point3D gasketPoint = (Point3D)adjPoint.Clone();
            gasketPoint.X = gasketPoint.X + BBF;

            Line lineGAa = new Line(GetSumPoint(gasketPoint, 0, -sideWidth, 0), GetSumPoint(gasketPoint, 0, -OD + sideWidth, 0));
            Line lineGAb = new Line(GetSumPoint(gasketPoint, gasketT, -sideWidth, 0), GetSumPoint(gasketPoint, gasketT, -OD + sideWidth, 0));
            Line lineGAc = new Line(GetSumPoint(gasketPoint, 0, -sideWidth, 0), GetSumPoint(gasketPoint, gasketT, -sideWidth, 0));
            Line lineGAd = new Line(GetSumPoint(gasketPoint, 0, -OD + sideWidth, 0), GetSumPoint(gasketPoint, gasketT, -OD + sideWidth, 0));

            double gasketCirWidth = (R - (G - pipeThickness * 2)) / 2;
            Line lineGAe = new Line(GetSumPoint(gasketPoint, 0, -sideWidth - gasketCirWidth, 0), GetSumPoint(gasketPoint, gasketT, -sideWidth - gasketCirWidth, 0));
            Line lineGAf = new Line(GetSumPoint(gasketPoint, 0, -OD + sideWidth + gasketCirWidth, 0), GetSumPoint(gasketPoint, gasketT, -OD + sideWidth + gasketCirWidth, 0));
            customEntity.AddRange(new Entity[] { lineGAa, lineGAb, lineGAc, lineGAd, lineGAe, lineGAf });

            // Flange : Only WN
            Point3D flangePoint = (Point3D)gasketPoint.Clone();
            flangePoint.X = flangePoint.X + gasketT;

            Line lineFa = new Line(GetSumPoint(flangePoint, 0, -sideWidth, 0), GetSumPoint(flangePoint, 0, -OD + sideWidth, 0));

            Line lineFb = new Line(GetSumPoint(flangePoint, 0, -sideWidth, 0), GetSumPoint(flangePoint, C, -sideWidth, 0));
            Line lineFc = new Line(GetSumPoint(flangePoint, 0, -OD + sideWidth, 0), GetSumPoint(flangePoint, C, -OD + sideWidth, 0));

            Line lineFd = new Line(GetSumPoint(flangePoint, C, 0, 0), GetSumPoint(flangePoint, C, -OD, 0));
            Line lineFe = new Line(GetSumPoint(flangePoint, BWN, 0, 0), GetSumPoint(flangePoint, BWN, -OD, 0));

            Line lineFf = new Line(GetSumPoint(flangePoint, BWN, 0, 0), GetSumPoint(flangePoint, C, 0, 0));
            Line lineFg = new Line(GetSumPoint(flangePoint, BWN, -OD, 0), GetSumPoint(flangePoint, C, -OD, 0));

            Line lineFh = new Line(GetSumPoint(flangePoint, A, -sideWidth3, 0), GetSumPoint(flangePoint, A, -OD+sideWidth3, 0));
            Line lineFi = new Line(GetSumPoint(flangePoint, A, -sideWidth3, 0), GetSumPoint(flangePoint, BWN, - sideWidth2, 0));
            Line lineFj = new Line(GetSumPoint(flangePoint, A, -OD +sideWidth3, 0), GetSumPoint(flangePoint, BWN, -OD + sideWidth2, 0));
            customEntity.AddRange(new Entity[] { lineFa,lineFb, lineFc,lineFd, lineFe, lineFf, lineFg, lineFh,lineFi,lineFj});


            // Neck
            Point3D neckPoint = (Point3D)flangePoint.Clone();
            neckPoint.X = neckPoint.X + A;

            if (neckLengthReal > 0)
            {
                Line lineNeckLeft = null;
                Line lineNeckRight = null;
                if (selNozzle.Position == "roof")
                {
                    double neckSlopeHeight = valueService.GetOppositeByWidth(assemblyData.RoofInput[0].RoofSlopeOne,G/2);

                    switch (selNozzle.LR)
                    {
                        case "left":
                            lineNeckLeft = new Line(GetSumPoint(neckPoint, 0, -OD + sideWidth3, 0), GetSumPoint(neckPoint,  neckLengthReal + neckSlopeHeight, -OD + sideWidth3, 0));
                            lineNeckRight = new Line(GetSumPoint(neckPoint, 0, -sideWidth3, 0), GetSumPoint(neckPoint, neckLengthReal - neckSlopeHeight, -sideWidth3, 0));
                            break;
                        case "right":
                            lineNeckLeft = new Line(GetSumPoint(neckPoint, 0, -OD + sideWidth3, 0), GetSumPoint(neckPoint, neckLengthReal - neckSlopeHeight, -OD + sideWidth3, 0));
                            lineNeckRight = new Line(GetSumPoint(neckPoint, 0, -sideWidth3, 0), GetSumPoint(neckPoint, neckLengthReal + neckSlopeHeight, -sideWidth3, 0));
                            break;
                    }
                }
                else
                {
                    lineNeckLeft = new Line(GetSumPoint(neckPoint, 0, -sideWidth3, 0), GetSumPoint(neckPoint, neckLengthReal, -sideWidth3, 0));
                    lineNeckRight = new Line(GetSumPoint(neckPoint, 0, -OD + sideWidth3, 0), GetSumPoint(neckPoint, neckLengthReal, -OD + sideWidth3, 0));
                }
                //lineNeckLeft.AssyName = "neckleft";
                //lineNeckRight.AssyName = "neckright";

                //CustomEntityModel dd = ((CustomEntityModel)lineNeckLeft);
                //CustomLine ffff = dd;
                //lineNeckLeft.EntityData = "aa";

                
                customEntity.Add(lineNeckLeft);
                customEntity.Add(lineNeckRight);
            }


            // Rotate
            //Point3D rotatePoint = (Point3D)drawPoint.Clone();

            // Rotation
            //switch (selNozzle.Position)
            //{
            //    case "shell":
            //        switch (selNozzle.LR)
            //        {
            //            case "left":
            //                break;
            //            case "right":
            //                foreach (Entity eachEntity in customEntity)
            //                    eachEntity.Rotate(UtilityEx.DegToRad(-180), Vector3D.AxisZ, rotatePoint);
            //                break;
            //        }
            //        break;
            //    case "roof":
            //        switch (selNozzle.LR)
            //        {
            //            case "left":
            //            case "right":
            //                foreach (Entity eachEntity in customEntity)
            //                    eachEntity.Rotate(UtilityEx.DegToRad(-90), Vector3D.AxisZ, rotatePoint);
            //                break;
            //        }
            //        break;
            //}




            return customEntity;
        }
        private List<Entity> CreateFlangeSeriesB(CDPoint refPoint, Point3D drawPoint, NozzleInputModel selNozzle, NozzleOHFSeriesBModel drawNozzle, double selSizeNominalID, double selCenterTopHeight)
        {
            List<Entity> customEntity = new List<Entity>();

            return customEntity;
        }
        private List<Entity> CreateFlangeLWN(CDPoint refPoint, Point3D drawPoint, NozzleInputModel selNozzle, NozzleOHFLWNModel drawNozzle, double selSizeNominalID, double selCenterTopHeight)
        {
            List<Entity> customEntity = new List<Entity>();

            return customEntity;
        }
        private List<Entity> CreateFlange(CDPoint refPoint, Point3D drawPoint, NozzleInputModel selNozzle, NozzleOHFModel drawNozzle, double selSizeNominalID, double selCenterTopHeight)
        {
            List<Entity> customEntity = new List<Entity>();

            return customEntity;
        }


        // Translate TH to OHF
        private NozzleOHFSeriesAModel TransNozzleATHtoOHF(NozzleTHSeriesAModel selNozzzle)
        {
            NozzleOHFSeriesAModel newModel = new NozzleOHFSeriesAModel();
            newModel.DN= selNozzzle.DN;
            newModel.NPS = selNozzzle.NPS;
            newModel.G = selNozzzle.G;
            newModel.OD = selNozzzle.OD;
            newModel.BCD = selNozzzle.BCD;
            newModel.RRF = selNozzzle.RRF;
            newModel.RFF = selNozzzle.RFF;
            newModel.H = selNozzzle.H;
            newModel.A = selNozzzle.A;
            newModel.BWN = selNozzzle.BWN;
            newModel.BBF = selNozzzle.BBF;
            newModel.C = selNozzzle.C;
            newModel.BoltNo = selNozzzle.BoltNo;
            newModel.BoltSize1 = selNozzzle.BoltSize1;
            newModel.BoltSize2 = selNozzzle.BoltSize2;
            newModel.BoltLengthWNBF = selNozzzle.BoltLengthWNBF;
            newModel.BoltLengthWNWN = selNozzzle.BoltLengthWNWN;

            return newModel;
        }
        private NozzleOHFSeriesBModel TransNozzleBTHtoOHF(NozzleTHSeriesBModel selNozzzle)
        {
            NozzleOHFSeriesBModel newModel = new NozzleOHFSeriesBModel();
            newModel.DN = selNozzzle.DN;
            newModel.NPS = selNozzzle.NPS;
            newModel.G = selNozzzle.G;
            newModel.OD = selNozzzle.OD;
            newModel.BCD = selNozzzle.BCD;
            newModel.RRF = selNozzzle.RRF;
            newModel.RFF = selNozzzle.RFF;
            newModel.H = selNozzzle.H;
            newModel.A = selNozzzle.A;
            newModel.BWN = selNozzzle.BWN;
            newModel.BBF = selNozzzle.BBF;
            newModel.C = selNozzzle.C;
            newModel.BoltNo = selNozzzle.BoltNo;
            newModel.BoltSize1 = selNozzzle.BoltSize1;
            newModel.BoltSize2 = selNozzzle.BoltSize2;
            newModel.BoltLengthWNBF = selNozzzle.BoltLengthWNBF;
            newModel.BoltLengthWNWN = selNozzzle.BoltLengthWNWN;

            return newModel;
        }
        private NozzleOHFLWNModel TransNozzleLWNTHtoOHF(NozzleTHLWNModel selNozzzle)
        {
            NozzleOHFLWNModel newModel = new NozzleOHFLWNModel();
            newModel.DN = selNozzzle.DN;
            newModel.NPS = selNozzzle.NPS;
            newModel.G = selNozzzle.G;
            newModel.OD = selNozzzle.OD;
            newModel.BCD = selNozzzle.BCD;
            newModel.RRF = selNozzzle.RRF;
            newModel.RFF = selNozzzle.RFF;
            newModel.H = selNozzzle.H;
            newModel.B = selNozzzle.B;
            newModel.C = selNozzzle.C;
            newModel.BoltNo = selNozzzle.BoltNo;
            newModel.BoltSize1 = selNozzzle.BoltSize1;
            newModel.BoltSize2 = selNozzzle.BoltSize2;
            newModel.BoltLength = selNozzzle.BoltLength;

            return newModel;
        }
        private NozzleOHFModel TransNozzleTHtoOHF(NozzleTHModel selNozzzle)
        {
            NozzleOHFModel newModel = new NozzleOHFModel();
            newModel.DN = selNozzzle.DN;
            newModel.NPS = selNozzzle.NPS;
            newModel.G = selNozzzle.G;
            newModel.OD = selNozzzle.OD;
            newModel.BCD = selNozzzle.BCD;
            newModel.RRF = selNozzzle.RRF;
            newModel.RFF = selNozzzle.RFF;
            newModel.H = selNozzzle.H;
            newModel.AWN = selNozzzle.AWN;
            newModel.ASO = selNozzzle.ASO;
            newModel.B = selNozzzle.B;
            newModel.C = selNozzzle.C;
            newModel.BoltNo = selNozzzle.BoltNo;
            newModel.BoltSize1 = selNozzzle.BoltSize1;
            newModel.BoltSize2 = selNozzzle.BoltSize2;
            newModel.BoltLength = selNozzzle.BoltLength;

            return newModel;
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


                // New Value
                double newHeight = eachNozzle.HRSort;


                switch (eachNozzle.Position)
                {
                    case "shell":
                        switch (eachNozzle.LR)
                        {
                            case "left":
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

                            case "right":
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
                        }
                        break;
                    case "roof":
                        switch (eachNozzle.LR)
                        {
                            case "left":
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

                            case "right":
                                convergenceValue = GetConvergenceValue(newHeight, beforeValue.Bottom, convergenceHeight);
                                beforeValue.Bottom = eachNozzle.HRSort;
                                if (multiColumnValue && convergenceValue)
                                    convergencePosition.Bottom += circleSize;
                                else
                                    convergencePosition.Bottom = 0;

                                positionCount.Bottom++;
                                positionCountValue = positionCount.Bottom;
                                convergenceXY = convergencePosition.Bottom;
                                arrangeMaxHeightValue = arrangeMaxHeight.Bottom;
                                break;
                        }
                        break;
                }


                Point3D convPointXY;

                if (multiColumnValue && convergenceValue)
                {
                    // Convergence
                    convPointXY = GetPositionSpacingPoint(refPoint, eachNozzle.Position, eachNozzle.LR, 
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
                        convPointXY = GetPositionSpacingPoint(refPoint, eachNozzle.Position, eachNozzle.LR,
                                                                    newHeight,
                                                                    convergenceXY,
                                                                    selSizeNominalID, selCenterTopHeight, selShellSpacing);
                    }
                    else
                    {
                        double currentLower = newHeight - circleSize / 2;
                        if (arrangeMaxHeightValue > currentLower)
                            newHeight = arrangeMaxHeightValue + circleSize / 2;

                        convPointXY = GetPositionSpacingPoint(refPoint, eachNozzle.Position, eachNozzle.LR,
                                                                     newHeight,
                                                                     convergenceXY,
                                                                     selSizeNominalID, selCenterTopHeight, selShellSpacing);

                    }
                    
                    // Arrange Height
                    switch (eachNozzle.Position)
                    {
                        case "shell":
                            switch (eachNozzle.LR)
                            {
                                case "left":
                                    arrangeMaxHeight.Left = newHeight + circleSize / 2 + arrangeGapHeight;
                                    arrangeMaxHeightValue = arrangeMaxHeight.Left;
                                    break;
                                case "right":
                                    arrangeMaxHeight.Right = newHeight + circleSize / 2 + arrangeGapHeight;
                                    arrangeMaxHeightValue = arrangeMaxHeight.Right;
                                    break;
                            }
                            break;

                        case "roof":
                            switch (eachNozzle.LR)
                            {
                                case "left":
                                    arrangeMaxHeight.Top = newHeight + circleSize / 2 + arrangeGapHeight;
                                    arrangeMaxHeightValue = arrangeMaxHeight.Top;
                                    break;
                                case "right":
                                    arrangeMaxHeight.Bottom = newHeight + circleSize / 2 + arrangeGapHeight;
                                    arrangeMaxHeightValue = arrangeMaxHeight.Bottom;
                                    break;
                            }
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
        private Dictionary<string, List<Entity>> CreateNozzleLeader(Point3D refPoint, NozzleInputModel selNozzle, string fontSize, string circleSize)
        {

            string selPosition = selNozzle.Position;
            string selLR = selNozzle.LR;
            string textUpperStr = selNozzle.Mark;
            string textLowerStr = selNozzle.Size;

            // Current Test
            double cirDiameter = valueService.GetDoubleValue(circleSize);
            double cirRadius = cirDiameter / 2;
            double cirTextSize = valueService.GetDoubleValue(fontSize);

            // Point Adj
            Point3D drawPoint = new Point3D();
            switch (selPosition)
            {
                case "shell":
                    switch (selLR)
                    {
                        case "left":
                            drawPoint.X = refPoint.X - cirDiameter;
                            drawPoint.Y = refPoint.Y - cirRadius;
                            break;
                        case "right":
                            drawPoint.X = refPoint.X;
                            drawPoint.Y = refPoint.Y - cirRadius;
                            break;
                    }
                    break;
                case "roof":
                    switch (selLR)
                    {
                        case "left":
                            drawPoint.X = refPoint.X - cirRadius;
                            drawPoint.Y = refPoint.Y;
                            break;
                        case "right":
                            drawPoint.X = refPoint.X - cirRadius;
                            drawPoint.Y = refPoint.Y;
                            break;
                    }
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

            DrawPositionValueModel maxPoint = new DrawPositionValueModel();

            // Mid Point
            midPoint.Left = -99999999;
            midPoint.Right = 99999999;
            midPoint.Top = -99999999;
            midPoint.Bottom = -99999999;

            // Max Point
            maxPoint.Left = -99999999;
            maxPoint.Right = 99999999;
            maxPoint.Top = 99999999;
            maxPoint.Bottom = 99999999;

            // Convergence Check : 높이가 같을 경우
            DrawPositionValueModel beforeEndPoint = new DrawPositionValueModel();
            int indexArrange = -1;

            foreach(NozzleInputModel eachNozzle in selNozzle)
            {
                indexArrange++;

                Point3D nozzlePoint = selNozzleStartPoint[indexArrange];
                Point3D markPoint = selLeaderMarkPoint[indexArrange];

                POSITION_TYPE eachNozzlePosition = CommonMethod.PositionToEnum(eachNozzle.LR);
                switch (eachNozzle.Position)
                {
                    case "shell":
                        switch (eachNozzle.LR)
                        {
                            case "left":
                                if (beforeEndPoint.Left == markPoint.Y)
                                    continue;

                                if (nozzlePoint.Y == markPoint.Y)
                                {
                                    // 직선
                                    customEntity.AddRange(CreateBrokenLIne(GetSumPoint(nozzlePoint, -nozzleLength, 0), markPoint));

                                    if (midPoint.Left < markPoint.X)
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
                                    customEntity.AddRange(CreateBrokenLIne(GetSumPoint(nozzlePoint, -nozzleLength, 0), markPoint, new Point3D(midPoint.Left, nozzlePoint.Y), new Point3D(midPoint.Left, markPoint.Y)));

                                }
                                beforeEndPoint.Left = markPoint.Y;
                                break;

                            case "right":
                                if (beforeEndPoint.Right == markPoint.Y)
                                    continue;

                                if (nozzlePoint.Y == markPoint.Y)
                                {
                                    // 직선
                                    customEntity.AddRange(CreateBrokenLIne(GetSumPoint(nozzlePoint, +nozzleLength, 0), markPoint));

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
                                    customEntity.AddRange(CreateBrokenLIne(GetSumPoint(nozzlePoint, +nozzleLength, 0), markPoint, new Point3D(midPoint.Right, nozzlePoint.Y), new Point3D(midPoint.Right, markPoint.Y)));

                                }
                                beforeEndPoint.Right = markPoint.Y;
                                break;
                        }
                        break;
                    case "roof":
                        switch (eachNozzle.LR)
                        {
                            case "left":
                                if (beforeEndPoint.Top == markPoint.X)
                                    continue;

                                if (nozzlePoint.X == markPoint.X)
                                {
                                    // 직선
                                    customEntity.AddRange(CreateBrokenLIne(GetSumPoint(nozzlePoint, 0, +nozzleLength), markPoint));
                                    if (midPoint.Top < markPoint.Y)
                                        midPoint.Top = markPoint.Y;
                                }
                                else if (nozzlePoint.X < markPoint.X)
                                {

                                }
                                else if (nozzlePoint.X > markPoint.X)
                                {
                                    // 위로
                                    if (beforeEndPoint.Top != markPoint.X)
                                        midPoint.Top -= midPointXGap; // 아래쪽으로 이동
                                    customEntity.AddRange(CreateBrokenLIne(GetSumPoint(nozzlePoint, 0, +nozzleLength),markPoint, new Point3D(nozzlePoint.X, midPoint.Top), new Point3D(markPoint.X, midPoint.Top)));

                                }
                                beforeEndPoint.Top = markPoint.X;
                                break;

                            case "right":
                                if (beforeEndPoint.Bottom == markPoint.X)
                                    continue;

                                if (nozzlePoint.X == markPoint.X)
                                {
                                    // 직선
                                    customEntity.AddRange(CreateBrokenLIne(GetSumPoint(nozzlePoint, 0, +nozzleLength),markPoint));
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
                                        midPoint.Bottom -= midPointXGap; // 위쪽으로 이동
                                    customEntity.AddRange(CreateBrokenLIne(GetSumPoint(nozzlePoint, 0, +nozzleLength), markPoint, new Point3D(nozzlePoint.X, midPoint.Bottom), new Point3D(markPoint.X, midPoint.Bottom)));

                                }
                                beforeEndPoint.Bottom = markPoint.X;
                                break;
                        }
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
        private Point3D GetPositionPoint(CDPoint refPoint, string selPosition, string selLR, double newValue,double convergenceValue, double selSizeNominalID, double selCenterTopHeight, DrawPositionValueModel selShellSpacing)
        {
            Point3D newPoint = null;
            CDPoint adjPoint = null;
            CDPoint curPoint = null;
            switch (selPosition)
            {
                case "shell":
                    switch (selLR)
                    {
                        case "left":
                            adjPoint = workingPointService.ContactPoint(WORKINGPOINT_TYPE.AdjLeftShell, newValue.ToString(), ref refPoint, ref curPoint);
                            newPoint = new Point3D(adjPoint.X + convergenceValue, adjPoint.Y, 0);
                            break;
                        case "right":
                            adjPoint = workingPointService.ContactPoint(WORKINGPOINT_TYPE.AdjRightShell, newValue.ToString(), ref refPoint, ref curPoint);
                            newPoint = new Point3D(adjPoint.X + convergenceValue, adjPoint.Y, 0);
                            break;
                    }
                    break;
                case "roof":

                    // Roof Slope
                    double roofSlopeHeight = valueService.GetHypotenuseByWidth(assemblyData.RoofInput[0].RoofSlopeOne, assemblyData.RoofInput[0].RoofThickness);
                    //double roofSlopeHeight = roofThickness * Math.Cos(roofSlopeDegree);

                    switch (selLR)
                    {
                        case "left":
                            adjPoint = workingPointService.ContactPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, (-newValue).ToString(), ref refPoint, ref curPoint);
                            newPoint = new Point3D(adjPoint.X , adjPoint.Y + roofSlopeHeight + convergenceValue, 0);
                            break;
                        case "right":
                            adjPoint = workingPointService.ContactPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, (-newValue).ToString(), ref refPoint, ref curPoint);
                            newPoint = new Point3D(adjPoint.X + (newValue*2), adjPoint.Y + roofSlopeHeight + convergenceValue, 0);
                            break;
                    }
                    break;

            }

            return newPoint;
        }
        private Point3D GetPositionLinePoint(CDPoint refPoint,Point3D drawPoint, NozzleInputModel selNozzle, string selPosition, string selLR, double newValue, double convergenceValue, double selSizeNominalID, double selCenterTopHeight, DrawPositionValueModel selShellSpacing)
        {

            double neckLength = 0;
            double R = valueService.GetDoubleValue(selNozzle.R);
            double H = valueService.GetDoubleValue(selNozzle.H);
            double shellThickness = 0;

            // Roof Slope

            double roofPointHeight = drawPoint.Y - refPoint.Y;

            Point3D newPoint = null;


            if (selNozzle.Position == "shell")
            {
                if (selNozzle.LR == "left")
                {
                    shellThickness = workingPointService.GetShellThicknessAccordingToHeight(selNozzle.H);
                    neckLength = R - (selSizeNominalID / 2) - shellThickness;
                    neckLength = R - (selSizeNominalID / 2) - shellThickness;
                    newPoint = GetSumPoint(drawPoint, -neckLength, 0);
                }
                else if (selNozzle.LR == "right")
                {
                    shellThickness = workingPointService.GetShellThicknessAccordingToHeight(selNozzle.H);
                    neckLength = R - (selSizeNominalID / 2) - shellThickness;
                    newPoint = GetSumPoint(drawPoint, +neckLength, 0);
                }
            }
            else if (selNozzle.Position == "roof")
            {
                if (selNozzle.LR == "left")
                {
                    neckLength = H - roofPointHeight;
                    newPoint = GetSumPoint(drawPoint, 0,neckLength, 0);
                }
                else if (selNozzle.LR == "right")
                {
                    neckLength = H - roofPointHeight;
                    newPoint = GetSumPoint(drawPoint, 0, neckLength, 0);
                }
            }



            return newPoint;
        }

        // Only Nozzle Mark Space
        private Point3D GetPositionSpacingPoint(CDPoint refPoint, string selPosition, string selLR, double newValue,double convergenceValue, double selSizeNominalID, double selCenterTopHeight, DrawPositionValueModel selShellSpacing)
        {
            Point3D newPoint = newPoint = GetPositionPoint(refPoint, selPosition,selLR, newValue, convergenceValue, selSizeNominalID, selCenterTopHeight,selShellSpacing);
            switch (selPosition)
            {
                case "shell":
                    switch (selLR)
                    {
                        case "left":
                            //newPoint.X -= selShellSpacing.Left;
                            newPoint.X = refPoint.X - selShellSpacing.Left +convergenceValue;
                            break;
                        case "right":
                            //newPoint.X += selShellSpacing.Right;
                            newPoint.X = refPoint.X + selSizeNominalID + selShellSpacing.Left + convergenceValue;
                            break;
                    }
                    break;

                case "roof":
                    switch (selLR)
                    {
                        case "left":
                            //newPoint.Y += selShellSpacing.Top;  
                            newPoint.Y = selCenterTopHeight + selShellSpacing.Top + convergenceValue;
                            break;
                        case "right":
                            //newPoint.Y += selShellSpacing.Top;
                            newPoint.Y = selCenterTopHeight + selShellSpacing.Top + convergenceValue;
                            break;
                    }
                    break;
            }
            return newPoint;
        }
        #endregion



        private double GetPipeThickness(string selNPS)
        {
            double returnValue = 2.77;
            foreach (PipeModel eachPipe in assemblyData.PipeList)
            {
                if (eachPipe.NPS == selNPS)
                {

                    break;
                }
            }

            return returnValue;
        }
        private double GetPipeSlipOn(string selNPS)
        {
            double returnValue = 5;
            foreach (PipeModel eachPipe in assemblyData.PipeList)
            {
                if (eachPipe.NPS == selNPS)
                {

                    break;
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
