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
using DrawWork.DrawStyleServices;
using DrawWork.CutomModels;
using System.Collections.ObjectModel;

namespace DrawWork.DrawServices
{
    public class DrawNozzleService
    {
        private AssemblyModel assemblyData;

        private ValueService valueService;
        private DrawWorkingPointService workingPointService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;

        private DrawPublicFunctionService publicFunService;

        private DrawNozzleBlockService nozzleBlock;
        private DrawImportBlockService blockImportService;

        public DrawNozzleService(AssemblyModel selAssembly, Object selModel)
        {
            assemblyData = selAssembly;

            valueService = new ValueService();
            workingPointService = new DrawWorkingPointService(selAssembly);
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();

            publicFunService = new DrawPublicFunctionService();

            nozzleBlock = new DrawNozzleBlockService();
            blockImportService = new DrawImportBlockService((Model)selModel);
        }

        #region Nozzle : Intergration : Shell, Roof, FRTRoof
        public List<NozzleInputModel> GetSumNozzleList()
        {
            List<NozzleInputModel> newList = new List<NozzleInputModel>();

            // 첫번재 복사 해서 사용
            foreach (NozzleShellInputModel eachNozzle in assemblyData.NozzleShellInputModel)
            {
                NozzleInputModel newNozzle = new NozzleInputModel();

                newNozzle.Position = eachNozzle.Position.ToLower();
                newNozzle.LR = eachNozzle.LR.ToLower();
                newNozzle.Mark = eachNozzle.Mark;
                newNozzle.Size = eachNozzle.Size;
                newNozzle.SCH = eachNozzle.SCH;

                newNozzle.PipeMaterial = eachNozzle.PipeMaterial;
                newNozzle.Qty = eachNozzle.Qty;
                newNozzle.Flange = eachNozzle.Facing;
                newNozzle.Rating = eachNozzle.Rating;
                newNozzle.Type = eachNozzle.Type.ToLower();

                newNozzle.Facing = eachNozzle.Facing.ToLower();
                newNozzle.FlangeMaterial = eachNozzle.FlangeMaterial;
                newNozzle.R = eachNozzle.R;
                newNozzle.H = eachNozzle.H;
                newNozzle.Ort = eachNozzle.Ort;

                newNozzle.AttachedType = eachNozzle.AttachedType;
                newNozzle.OffsetToCL = eachNozzle.OffsetToCL;
                newNozzle.Description = eachNozzle.Description;
                newNozzle.Remarks = eachNozzle.Remarks;

                // -> Start

                newNozzle.ASMESeries = eachNozzle.ASMESeries.ToLower();

                newNozzle.RePad = eachNozzle.RePad.ToLower();
                newNozzle.RePadType = eachNozzle.RePadType.ToLower();
                newNozzle.InternalPipe = eachNozzle.InternalPipe.ToLower();
                newNozzle.InternalPipeBottomSupt = eachNozzle.InternalPipeBottomSupt.ToLower();
                newNozzle.PipeJoint = eachNozzle.PipeJoint.ToLower();

                newNozzle.Fitting = eachNozzle.Fitting.ToLower();
                newNozzle.OutletDirection = eachNozzle.OutletDirection.ToLower();
                newNozzle.DrainType = eachNozzle.DrainType.ToLower();
                newNozzle.Cover = eachNozzle.Cover.ToLower();
                newNozzle.Manhole = eachNozzle.Manhole.ToLower();

                newNozzle.CleanOut = eachNozzle.CleanOut.ToLower();
                newNozzle.ManholeSupt = eachNozzle.ManholeSupt.ToLower();
                newNozzle.RiserPipe = eachNozzle.RiserPipe.ToLower();
                newNozzle.Mixer = eachNozzle.Mixer.ToLower();

                // Block
                newNozzle.GaugeHatch = eachNozzle.GaugeHatch.ToLower();

                newNozzle.InternalLadder = eachNozzle.InternalLadder.ToLower();
                newNozzle.GooseNeckBirdScreen = eachNozzle.GooseNeckBirdScreen.ToLower();
                newNozzle.FlameArrestor = eachNozzle.FlameArrestor.ToLower();
                newNozzle.BreatherValve = eachNozzle.BreatherValve.ToLower();
                newNozzle.VacuumReliefValve = eachNozzle.VacuumReliefValve.ToLower();

                newNozzle.EmergencyVent = eachNozzle.EmergencyVent.ToLower();
                newNozzle.InternalPipeBended = eachNozzle.InternalPipeBended.ToLower();
                newNozzle.WaterSpray = eachNozzle.WaterSpray.ToLower();
                newNozzle.FoamConn = eachNozzle.FoamConn.ToLower();

                // Sort Value
                if (newNozzle.Position == "shell")
                    newNozzle.HRSort = valueService.GetDoubleValue(eachNozzle.H);
                else if (newNozzle.Position == "roof")
                    newNozzle.HRSort = valueService.GetDoubleValue(eachNozzle.R);


                newList.Add(newNozzle);
            }
            foreach (NozzleRoofInputModel eachNozzle in assemblyData.NozzleRoofInputModel)
            {
                NozzleInputModel newNozzle = new NozzleInputModel();

                newNozzle.Position = eachNozzle.Position.ToLower();
                newNozzle.LR = eachNozzle.LR.ToLower();
                newNozzle.Mark = eachNozzle.Mark;
                newNozzle.Size = eachNozzle.Size;
                newNozzle.SCH = eachNozzle.SCH;

                newNozzle.PipeMaterial = eachNozzle.PipeMaterial;
                newNozzle.Qty = eachNozzle.Qty;
                newNozzle.Flange = eachNozzle.Facing;
                newNozzle.Rating = eachNozzle.Rating;
                newNozzle.Type = eachNozzle.Type.ToLower();

                newNozzle.Facing = eachNozzle.Facing.ToLower();
                newNozzle.FlangeMaterial = eachNozzle.FlangeMaterial;
                newNozzle.R = eachNozzle.R;
                newNozzle.H = eachNozzle.H;
                newNozzle.Ort = eachNozzle.Ort;

                newNozzle.AttachedType = eachNozzle.AttachedType;
                newNozzle.OffsetToCL = eachNozzle.OffsetToCL;
                newNozzle.Description = eachNozzle.Description;
                newNozzle.Remarks = eachNozzle.Remarks;

                // -> Start

                newNozzle.ASMESeries = eachNozzle.ASMESeries.ToLower();

                newNozzle.RePad = eachNozzle.RePad.ToLower();
                newNozzle.RePadType = eachNozzle.RePadType.ToLower();
                newNozzle.InternalPipe = eachNozzle.InternalPipe.ToLower();
                newNozzle.InternalPipeBottomSupt = eachNozzle.InternalPipeBottomSupt.ToLower();
                newNozzle.PipeJoint = eachNozzle.PipeJoint.ToLower();

                newNozzle.Fitting = eachNozzle.Fitting.ToLower();
                newNozzle.OutletDirection = eachNozzle.OutletDirection.ToLower();
                newNozzle.DrainType = eachNozzle.DrainType.ToLower();
                newNozzle.Cover = eachNozzle.Cover.ToLower();
                newNozzle.Manhole = eachNozzle.Manhole.ToLower();

                newNozzle.CleanOut = eachNozzle.CleanOut.ToLower();
                newNozzle.ManholeSupt = eachNozzle.ManholeSupt.ToLower();
                newNozzle.RiserPipe = eachNozzle.RiserPipe.ToLower();
                newNozzle.Mixer = eachNozzle.Mixer.ToLower();

                // Block
                newNozzle.GaugeHatch = eachNozzle.GaugeHatch.ToLower();

                newNozzle.InternalLadder = eachNozzle.InternalLadder.ToLower();
                newNozzle.GooseNeckBirdScreen = eachNozzle.GooseNeckBirdScreen.ToLower();
                newNozzle.FlameArrestor = eachNozzle.FlameArrestor.ToLower();
                newNozzle.BreatherValve = eachNozzle.BreatherValve.ToLower();
                newNozzle.VacuumReliefValve = eachNozzle.VacuumReliefValve.ToLower();

                newNozzle.EmergencyVent = eachNozzle.EmergencyVent.ToLower();
                newNozzle.InternalPipeBended = eachNozzle.InternalPipeBended.ToLower();
                newNozzle.WaterSpray = eachNozzle.WaterSpray.ToLower();
                newNozzle.FoamConn = eachNozzle.FoamConn.ToLower();

                // Sort Value
                if (newNozzle.Position == "shell")
                    newNozzle.HRSort = valueService.GetDoubleValue(eachNozzle.H);
                else if (newNozzle.Position == "roof")
                    newNozzle.HRSort = valueService.GetDoubleValue(eachNozzle.R);


                newList.Add(newNozzle);
            }
            //foreach (NozzleFRTRoofInputModel eachNozzle in assemblyData.NozzleFRTRoofInputModel)
            //{
            //    NozzleInputModel newNozzle = new NozzleInputModel();

            //    newNozzle.Position = eachNozzle.Position.ToLower();
            //    newNozzle.LR = eachNozzle.LR.ToLower();
            //    newNozzle.Mark = eachNozzle.Mark;
            //    newNozzle.Size = eachNozzle.Size;
            //    newNozzle.SCH = eachNozzle.SCH;

            //    newNozzle.PipeMaterial = eachNozzle.PipeMaterial;
            //    newNozzle.Qty = eachNozzle.Qty;
            //    newNozzle.Flange = eachNozzle.Facing;
            //    newNozzle.Rating = eachNozzle.Rating;
            //    newNozzle.Type = eachNozzle.Type.ToLower();

            //    newNozzle.Facing = eachNozzle.Facing.ToLower();
            //    newNozzle.FlangeMaterial = eachNozzle.FlangeMaterial;
            //    newNozzle.R = eachNozzle.R;
            //    newNozzle.H = eachNozzle.H;
            //    newNozzle.Ort = eachNozzle.Ort;

            //    newNozzle.AttachedType = eachNozzle.AttachedType;
            //    newNozzle.OffsetToCL = eachNozzle.OffsetToCL;
            //    newNozzle.Description = eachNozzle.Description;
            //    newNozzle.Remarks = eachNozzle.Remarks;

            //    // -> Start

            //    newNozzle.ASMESeries = eachNozzle.ASMESeries.ToLower();

            //    newNozzle.RePad = eachNozzle.RePad.ToLower();
            //    newNozzle.RePadType = eachNozzle.RePadType.ToLower();
            //    newNozzle.InternalPipe = eachNozzle.InternalPipe.ToLower();
            //    newNozzle.InternalPipeBottomSupt = eachNozzle.InternalPipeBottomSupt.ToLower();
            //    newNozzle.PipeJoint = eachNozzle.PipeJoint.ToLower();

            //    newNozzle.Fitting = eachNozzle.Fitting.ToLower();
            //    newNozzle.OutletDirection = eachNozzle.OutletDirection.ToLower();
            //    newNozzle.DrainType = eachNozzle.DrainType.ToLower();
            //    newNozzle.Cover = eachNozzle.Cover.ToLower();
            //    newNozzle.Manhole = eachNozzle.Manhole.ToLower();

            //    newNozzle.CleanOut = eachNozzle.CleanOut.ToLower();
            //    newNozzle.ManholeSupt = eachNozzle.ManholeSupt.ToLower();
            //    newNozzle.RiserPipe = eachNozzle.RiserPipe.ToLower();
            //    newNozzle.Mixer = eachNozzle.Mixer.ToLower();

            //    // Block
            //    newNozzle.GaugeHatch = eachNozzle.GaugeHatch.ToLower();

            //    newNozzle.InternalLadder = eachNozzle.InternalLadder.ToLower();
            //    newNozzle.GooseNeckBirdScreen = eachNozzle.GooseNeckBirdScreen.ToLower();
            //    newNozzle.FlameArrestor = eachNozzle.FlameArrestor.ToLower();
            //    newNozzle.BreatherValve = eachNozzle.BreatherValve.ToLower();
            //    newNozzle.VacuumReliefValve = eachNozzle.VacuumReliefValve.ToLower();

            //    newNozzle.EmergencyVent = eachNozzle.EmergencyVent.ToLower();
            //    newNozzle.InternalPipeBended = eachNozzle.InternalPipeBended.ToLower();
            //    newNozzle.WaterSpray = eachNozzle.WaterSpray.ToLower();
            //    newNozzle.FoamConn = eachNozzle.FoamConn.ToLower();

            //    // Sort Value
            //    if (newNozzle.Position == "shell")
            //        newNozzle.HRSort = valueService.GetDoubleValue(eachNozzle.H);
            //    else if (newNozzle.Position == "roof")
            //        newNozzle.HRSort = valueService.GetDoubleValue(eachNozzle.R);


            //    newList.Add(newNozzle);
            //}

            return newList;
        }
        #endregion

        public DrawEntityModel DrawNozzle_GA(ref CDPoint refPoint,
                                      string selPosition, 
                                      string selNozzleType,
                                      string selNozzlePosition,
                                      string selNozzleFontSize,
                                      string selLeaderCircleSize,
                                      string selMultiColumn,
                                      string selLayerName,
                                      double selScaleValue)
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
            int refFirstIndex = 0;

            //string selSizeTankHeight = assemblyData.GeneralDesignData[refFirstIndex].SizeTankHeight;
            string selSizeNominalId = assemblyData.GeneralDesignData[refFirstIndex].SizeNominalID;

            double sizeNominalId = valueService.GetDoubleValue(selSizeNominalId);
            CDPoint newCurPoint = new CDPoint();
            CDPoint centerTopPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterTopUp, ref refPoint, ref newCurPoint);
            double centerTopHeight = centerTopPoint.Y;


            // Entity
            List<Entity> customOutlineEntity = new List<Entity>();
            List<Entity> customMarkEntity = new List<Entity>();
            List<Entity> customTextEntity = new List<Entity>();


            // MutilColumn
            bool multiColumnValue = selMultiColumn == "true" ? true : false;



            // Nozzle List : Adjust : Sort Value
            List<NozzleInputModel> drawNozzle = GetSumNozzleList();

            // Nozzle : point : adjust : Drain : Lower 타입 때문에 : 필수
            // 높이 조절
            CDPoint curPoint = new CDPoint();
            CDPoint bottomPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointReferenceBottom, 0, ref refPoint, ref curPoint);
            double bottomPlateHeight = refPoint.Y - bottomPoint.Y;
            foreach (NozzleInputModel eachNozzle in drawNozzle)
            {
                if (eachNozzle.DrainType == "lower")
                {
                    double underHeight = GetDrainLowerUnderHeight(eachNozzle,refPoint);
                    eachNozzle.H = "-" + underHeight.ToString();
                    eachNozzle.HRSort = - underHeight;

                }
                // Sort Value
                if (eachNozzle.Position == "shell")
                {
                    eachNozzle.HRSort = eachNozzle.HRSort - bottomPlateHeight;
                    eachNozzle.H = eachNozzle.HRSort.ToString();
                }
                else if (eachNozzle.Position == "roof")
                {
                    eachNozzle.HRSort = eachNozzle.HRSort - bottomPlateHeight;
                    eachNozzle.R = eachNozzle.HRSort.ToString();
                }


            }

            // Nozzle List : Sort
            List<NozzleInputModel> drawArrangeNozzle = drawNozzle.OrderBy(x => x.HRSort).ThenBy(x=>x.LR).ThenBy(x=>x.Position).ToList();

            // Nozzle Start Point List : Nozzle
            List<Point3D> NozzlePointList = new List<Point3D>();

            // Nozzle Start Point List : Line
            List<Point3D> NozzleLinePointList = new List<Point3D>();


            // Manhole 점검
            // - Shell Manhole : 1 개 : 중앙 절반
            // - Shell Cleanout : 1 개 : 중앙 절반
            // Manhole, cleanout 동시
            // - Shell Manhole : 1개 : 중앙 절반
            // - shell Cleanout : 1개 : 오른족 이동 전체
            double cleanoutWidth = 0;
            double manholeSize = 0;
            if(CheckManholeCleanout(drawArrangeNozzle,out manholeSize))
                cleanoutWidth = manholeSize + 1000; // Default gap

            // Nozzle : Create Model
            foreach (NozzleInputModel eachNozzle in drawArrangeNozzle)
            {
                // Start Point
                Point3D newNozzlePoint = GetPositionPoint(refPoint, eachNozzle.Position, eachNozzle.LR, eachNozzle.HRSort, 0, sizeNominalId, centerTopHeight, shellSpacing);
                NozzlePointList.Add(newNozzlePoint);

                Point3D newNozzleLinePoint =GetPositionLinePoint(refPoint,newNozzlePoint,eachNozzle, eachNozzle.Position, eachNozzle.LR, eachNozzle.HRSort, 0, sizeNominalId, centerTopHeight, shellSpacing);

                NozzleLinePointList.Add(newNozzleLinePoint);

                // Start Point : Adjust : Drain Type : Lower
                //if (eachNozzle.DrainType=="lower")
                //    SetDrainLowerPoint(eachNozzle, refPoint, ref newNozzlePoint, ref newNozzleLinePoint);


                List<Entity> customEntity = CreateFlangeAll(ref nozzleEntities, refPoint, newNozzlePoint,newNozzleLinePoint, eachNozzle, sizeNominalId, centerTopHeight,selScaleValue, cleanoutWidth);

                // Create Model : OutLine
                customOutlineEntity.AddRange(customEntity);
            }

            // Layer
            //styleService.SetLayerListEntity(ref customOutlineEntity, selLayerName);


            nozzleEntities.outlineList.AddRange(customOutlineEntity);

            // Nozzel Mark Point List : Create Mark Position Arrangement 
            List<Point3D> leaderMarkPointList = GetArrangeLeaderMarkPosition(  drawArrangeNozzle, selLeaderCircleSize, multiColumnValue, sizeNominalId, centerTopHeight, shellSpacing, refPoint);

            // Nozzle : Create Mark
            int indexArrange = -1;
            foreach (NozzleInputModel eachNozzle in drawArrangeNozzle)
            {
                indexArrange++;
                Point3D drawPoint = leaderMarkPointList[indexArrange];
                Dictionary<string, List<Entity>> customEntity = CreateNozzleLeader(drawPoint, eachNozzle, selNozzleFontSize, selLeaderCircleSize);
                customMarkEntity.AddRange(customEntity[CommonGlobal.NozzleMark]);
                customTextEntity.AddRange(customEntity[CommonGlobal.NozzleText]);

            }
            styleService.SetLayerListEntity(ref customMarkEntity, selLayerName);
            styleService.SetLayerListTextEntity(ref customTextEntity, selLayerName);
            nozzleEntities.nozzleMarkList.AddRange(customMarkEntity);
            nozzleEntities.nozzleTextList.AddRange(customTextEntity);

            // Nozzle : Create Line
            List<Entity> customLineEntity = CreateNozzleLeaderLine(refPoint, shellSpacing, drawArrangeNozzle, leaderMarkPointList, NozzleLinePointList);
            //List<Entity> customLineEntity = CreateNozzleLeaderLine(refPoint, shellSpacing, drawArrangeNozzle, leaderMarkPointList, NozzlePointList);
            styleService.SetLayerListEntity(ref customLineEntity, selLayerName);
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

        public bool CheckManholeCleanout(List<NozzleInputModel> selNozzleList,out double manholeSize)
        {
            double manholeCount = 0;
            double cleanoutCount = 0;
            manholeSize = 0;
            foreach (NozzleInputModel eachNozzle in selNozzleList)
            {
                if (eachNozzle.Position == "shell")
                {
                    if (eachNozzle.Manhole == "yes")
                    {
                        if (eachNozzle.LR == "center")
                        {
                            manholeCount++;
                            foreach(ReinforcingPadModel eachPad in assemblyData.ReinforcingPadList)
                            {
                                if (eachPad.Pipe == eachNozzle.Size)
                                {
                                    double wValue = valueService.GetDoubleValue(eachPad.W);
                                    if (wValue > 0)
                                        manholeSize = wValue / 2;
                                    break;
                                }
                            }
                        }
                    }
                    else if (eachNozzle.CleanOut == "yes")
                    {
                        cleanoutCount++;
                    }
                }
            }

            if (cleanoutCount > 0 & manholeCount > 0)
                return true;
            else
                return false;

        }

        #region Nozzel : Create Model



        // Division : Flange Style
        private List<Entity> CreateFlangeAll(ref DrawEntityModel selDrawEntities, CDPoint refPoint, Point3D drawPoint, Point3D darwStartPoint, NozzleInputModel selNozzle, double selSizeNominalID, double selCenterTopHeight,double scaleValue,double selCleanoutWidth)
        {
            string checkValue = "yes";
            Entity newBlock = null;
            List<Entity> customEntity = new List<Entity>();

            #region Block
            // Draw Point = DrawStartPoint
            NozzleBlock_Type nozzlePlane = CreateFlange_Block(refPoint, darwStartPoint, selNozzle,ref newBlock);
            #endregion

            switch (nozzlePlane)
            {
                case NozzleBlock_Type.OnlyBlock:
                    // only Block : 위치 지정하기 : 4가지

                    break;


                case NozzleBlock_Type.Other:
                    // 없음

                    break;

                case NozzleBlock_Type.Flange:


                    if(selNozzle.Manhole== checkValue && selNozzle.Position=="shell")
                    {
                        // Manhole : Pad 모양 : 형상 그려야 함
                        customEntity.AddRange(CreateManholeCenter(refPoint, drawPoint, darwStartPoint, selNozzle, selSizeNominalID, selCenterTopHeight, scaleValue));
                    }
                    else if (selNozzle.CleanOut == checkValue)
                    {
                        // CLEAR OUT
                        customEntity.AddRange(CreateCleanout(refPoint, drawPoint, darwStartPoint, selNozzle, selSizeNominalID, selCenterTopHeight, scaleValue, selCleanoutWidth));
                    }
                    else
                    {
                        // Manhole
                        if (selNozzle.Manhole == checkValue)
                        {
                            if(selNozzle.Position=="shell" && selNozzle.LR == "center")
                            {
                                // Manhole : Pad
                            }
                            else
                            {
                                // Manhole : Flange : Roof에만 위치함
                                customEntity.AddRange(CreateManhole(refPoint, drawPoint, darwStartPoint, selNozzle, selSizeNominalID, selCenterTopHeight, scaleValue));
                            }
                        }
                        else
                        {
                            #region Flange
                            string seriesValue = selNozzle.ASMESeries.Replace(" ", "").ToLower().Replace("series", "");
                            string typeValue = selNozzle.Type;
                            string ratingValue = selNozzle.Rating.Replace("#", "");
                            if (seriesValue.Contains("a"))
                            {
                                if (selNozzle.Rating.Contains("150"))
                                {
                                    FlangeOHFSeriesAModel newNozzle = null;
                                    foreach (FlangeOHFSeriesAModel eachNozzle in assemblyData.FlangeOHFSeriesAList)
                                    {
                                        if (eachNozzle.NPS == selNozzle.Size)
                                        {
                                            newNozzle = eachNozzle;
                                            customEntity = CreateFlangeSeriesA(refPoint, drawPoint, darwStartPoint, selNozzle, newNozzle, selSizeNominalID, selCenterTopHeight, scaleValue);
                                            break;
                                        }
                                    }
                                }
                                else if (selNozzle.Rating.Contains("300"))
                                {
                                    FlangeTHSeriesAModel newNozzle = null;
                                    foreach (FlangeTHSeriesAModel eachNozzle in assemblyData.FlangeTHSeriesAist)
                                    {
                                        if (eachNozzle.NPS == selNozzle.Size)
                                        {
                                            newNozzle = eachNozzle;
                                            customEntity = CreateFlangeSeriesA(refPoint, drawPoint, darwStartPoint, selNozzle, TransNozzleATHtoOHF(newNozzle), selSizeNominalID, selCenterTopHeight, scaleValue);
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (seriesValue.Contains("b"))
                            {
                                if (selNozzle.Rating.Contains("150"))
                                {
                                    FlangeOHFSeriesBModel newNozzle = null;
                                    foreach (FlangeOHFSeriesBModel eachNozzle in assemblyData.FlangeOHFSeriesBList)
                                    {
                                        if (eachNozzle.NPS == selNozzle.Size)
                                        {
                                            newNozzle = eachNozzle;
                                            customEntity = CreateFlangeSeriesB(refPoint, drawPoint, darwStartPoint, selNozzle, newNozzle, selSizeNominalID, selCenterTopHeight, scaleValue);
                                            break;
                                        }
                                    }
                                }
                                else if (selNozzle.Rating.Contains("300"))
                                {
                                    FlangeTHSeriesBModel newNozzle = null;
                                    foreach (FlangeTHSeriesBModel eachNozzle in assemblyData.FlangeTHSeriesBist)
                                    {
                                        if (eachNozzle.NPS == selNozzle.Size)
                                        {
                                            newNozzle = eachNozzle;
                                            customEntity = CreateFlangeSeriesB(refPoint, drawPoint, darwStartPoint, selNozzle, TransNozzleBTHtoOHF(newNozzle), selSizeNominalID, selCenterTopHeight, scaleValue);
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (typeValue.Contains("lwn"))
                            {
                                if (selNozzle.Rating.Contains("150"))
                                {
                                    FlangeOHFLWNModel newNozzle = null;
                                    foreach (FlangeOHFLWNModel eachNozzle in assemblyData.FlangeOHFLWNList)
                                    {
                                        if (eachNozzle.NPS == selNozzle.Size)
                                        {
                                            newNozzle = eachNozzle;
                                            customEntity = CreateFlangeLWN(refPoint, drawPoint, darwStartPoint, selNozzle, newNozzle, selSizeNominalID, selCenterTopHeight, scaleValue);
                                            break;
                                        }
                                    }
                                }
                                else if (selNozzle.Rating.Contains("300"))
                                {
                                    FlangeTHLWNModel newNozzle = null;
                                    foreach (FlangeTHLWNModel eachNozzle in assemblyData.FlangeTHLWNList)
                                    {
                                        if (eachNozzle.NPS == selNozzle.Size)
                                        {
                                            newNozzle = eachNozzle;
                                            customEntity = CreateFlangeLWN(refPoint, drawPoint, darwStartPoint, selNozzle, TransNozzleLWNTHtoOHF(newNozzle), selSizeNominalID, selCenterTopHeight, scaleValue);
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (selNozzle.Rating.Contains("150"))
                                {
                                    FlangeOHFModel newNozzle = null;
                                    foreach (FlangeOHFModel eachNozzle in assemblyData.FlangeOHFList)
                                    {
                                        if (eachNozzle.NPS == selNozzle.Size)
                                        {
                                            newNozzle = eachNozzle;
                                            customEntity = CreateFlange(refPoint, drawPoint, darwStartPoint, selNozzle, newNozzle, selSizeNominalID, selCenterTopHeight, scaleValue);
                                            break;
                                        }
                                    }
                                }
                                else if (selNozzle.Rating.Contains("300"))
                                {
                                    FlangeTHModel newNozzle = null;
                                    foreach (FlangeTHModel eachNozzle in assemblyData.FlangeTHList)
                                    {
                                        if (eachNozzle.NPS == selNozzle.Size)
                                        {
                                            newNozzle = eachNozzle;
                                            customEntity = CreateFlange(refPoint, drawPoint, darwStartPoint, selNozzle, TransNozzleTHtoOHF(newNozzle), selSizeNominalID, selCenterTopHeight, scaleValue);
                                            break;
                                        }
                                    }
                                }
                            }
                            #endregion
                        }


                        #region Rotation
                        double rotateValue = 0;
                        switch (selNozzle.Position)
                        {
                            case "shell":
                                switch (selNozzle.LR)
                                {
                                    case "left":
                                        rotateValue = UtilityEx.DegToRad(90);
                                        foreach (Entity eachEntity in customEntity)
                                            eachEntity.Rotate(rotateValue, Vector3D.AxisZ, darwStartPoint);
                                        if (newBlock != null)
                                            newBlock.Rotate(rotateValue, Vector3D.AxisZ, darwStartPoint);
                                        break;
                                    case "right":
                                        rotateValue = UtilityEx.DegToRad(-90);
                                        foreach (Entity eachEntity in customEntity)
                                            eachEntity.Rotate(rotateValue, Vector3D.AxisZ, darwStartPoint);
                                        if (newBlock != null)
                                            newBlock.Rotate(rotateValue, Vector3D.AxisZ, darwStartPoint);
                                        break;
                                    case "center":
                                        break;
                                }

                                break;

                            case "roof":
                                break;
                        }
                        #endregion




                        // Internal
                        double couplingHeight = 0;
                        customEntity.AddRange(CreateInternal(refPoint, drawPoint, selNozzle, couplingHeight,scaleValue));

                    }



                    break;


            }




            // Block Add
            if (newBlock != null)
                selDrawEntities.blockList.Add(newBlock);


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
        private List<Entity> CreateFlangeSeriesA(CDPoint refPoint, Point3D drawPoint, Point3D drawStartPoint, NozzleInputModel selNozzle,FlangeOHFSeriesAModel drawNozzle, double selSizeNominalID, double selCenterTopHeight, double selScaleValue)
        {
            double G = valueService.GetDoubleValue(drawNozzle.G);
            double OD = valueService.GetDoubleValue(drawNozzle.OD);
            double BCD = valueService.GetDoubleValue(drawNozzle.BCD);
            double R = 0;
            double RRF = valueService.GetDoubleValue(drawNozzle.RRF);
            double RFF = valueService.GetDoubleValue(drawNozzle.RFF);
            double H = valueService.GetDoubleValue(drawNozzle.H);
            double A = valueService.GetDoubleValue(drawNozzle.A);
            double B = 0;
            double BWN = valueService.GetDoubleValue(drawNozzle.BWN);
            double BBF = valueService.GetDoubleValue(drawNozzle.BBF);
            double C = valueService.GetDoubleValue(drawNozzle.C);
            double facingC = 0;

            // Facing : Default : Rf
            bool facingAdd = false;
            if (selNozzle.Facing.Contains("ff"))
            {
                R = RFF;
                facingC = 0;
            }
            else
            {
                facingAdd = true;
                facingC = C;
                R = RRF;
            }

            // Type : Nothing

            List<Entity> customEntity = new List<Entity>();


            List<Point3D> currentPoint = new List<Point3D>();
            currentPoint.Add(GetSumPoint(drawStartPoint, 0, 0));

            // Facing
            if (facingAdd)
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Pipe(out currentPoint, currentPoint[0], 1, R, facingC, true, true, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));

            // Flange
            customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out currentPoint, currentPoint[0], 1, BCD, OD, BWN - facingC, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = !facingAdd, twoEx = true }));
            // Neck
            customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Neck(out currentPoint, currentPoint[0], 1, G, H, A - BWN, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));

            // Reinf Pady -> 먼저 그릴 것 : Neck 길이 조절
            double couplingHeight = 0;
            if (selNozzle.RePadType != "")
            {
                customEntity.AddRange(CreateReinforcingPAD(refPoint, drawPoint, selNozzle, selSizeNominalID, selScaleValue, ref couplingHeight));
            }

            // PipeNeck
            double flangeLength = A;
            double neckLength = Point3D.Distance(drawStartPoint, drawPoint) - flangeLength;
            if (selNozzle.Position == "roof")
            {
                double pipeSlope = valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope);
                if (selNozzle.LR == "left")
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPoint, currentPoint[0], 1, G, neckLength, pipeSlope, 0, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));
                else if (selNozzle.LR == "right")
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPoint, currentPoint[0], 1, G, neckLength, -pipeSlope, 0, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, twoEx = true }));
            }
            else if (selNozzle.Position == "shell")
            {
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPoint, currentPoint[0], 1, G, neckLength, 0, 0, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));
            }

            // Cover : Blind Flange
            if (selNozzle.Cover == "yes")
            {
                List<Point3D> coverPoint = new List<Point3D>();
                coverPoint.Add(GetSumPoint(drawStartPoint, 0, 0));
                coverPoint.Add(GetSumPoint(drawStartPoint, 0, 0));
                if (facingAdd)
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Pipe(out coverPoint, coverPoint[0], 0, R, facingC, true, true, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out coverPoint, coverPoint[1], 0, BCD, OD, BBF - facingC, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true, twoEx = true }));
            }



            return customEntity;
        }
        private List<Entity> CreateFlangeSeriesB(CDPoint refPoint, Point3D drawPoint, Point3D drawStartPoint, NozzleInputModel selNozzle, FlangeOHFSeriesBModel drawNozzle, double selSizeNominalID, double selCenterTopHeight, double selScaleValue)
        {
            double G = valueService.GetDoubleValue(drawNozzle.G);
            double OD = valueService.GetDoubleValue(drawNozzle.OD);
            double BCD = valueService.GetDoubleValue(drawNozzle.BCD);
            double R = 0;
            double RRF = valueService.GetDoubleValue(drawNozzle.RRF);
            double RFF = valueService.GetDoubleValue(drawNozzle.RFF);
            double H = valueService.GetDoubleValue(drawNozzle.H);
            double A = valueService.GetDoubleValue(drawNozzle.A);
            double B = 0;
            double BWN = valueService.GetDoubleValue(drawNozzle.BWN);
            double BBF = valueService.GetDoubleValue(drawNozzle.BBF);
            double C = valueService.GetDoubleValue(drawNozzle.C);
            double facingC = 0;

            // Facing : Default : Rf
            bool facingAdd = false;
            if (selNozzle.Facing.Contains("ff"))
            {
                R = RFF;
                facingC = 0;
            }
            else
            {
                facingAdd = true;
                facingC = C;
                R = RRF;
            }

            // Type : Nothing

            List<Entity> customEntity = new List<Entity>();


            List<Point3D> currentPoint = new List<Point3D>();
            currentPoint.Add(GetSumPoint(drawStartPoint, 0, 0));

            // Facing
            if (facingAdd)
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Pipe(out currentPoint, currentPoint[0], 1, R, facingC, true, true, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));

            // Flange
            customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out currentPoint, currentPoint[0], 1, BCD, OD, BWN - facingC, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx= !facingAdd, twoEx = true }));
            // Neck
            customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Neck(out currentPoint, currentPoint[0], 1, G, H, A-BWN, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx=true }));

            // Reinf Pady -> 먼저 그릴 것 : Neck 길이 조절
            double couplingHeight = 0;
            if (selNozzle.RePadType != "")
            {
                customEntity.AddRange(CreateReinforcingPAD(refPoint, drawPoint, selNozzle, selSizeNominalID, selScaleValue, ref couplingHeight));
            }

            // PipeNeck
            double flangeLength = A;
            double neckLength = Point3D.Distance(drawStartPoint, drawPoint) - flangeLength;
            if (selNozzle.Position == "roof")
            {
                double pipeSlope = valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope);
                if (selNozzle.LR == "left")
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPoint, currentPoint[0], 1, G, neckLength, pipeSlope, 0, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));
                else if (selNozzle.LR == "right")
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPoint, currentPoint[0], 1, G, neckLength, -pipeSlope, 0, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, twoEx = true }));
            }
            else if (selNozzle.Position == "shell")
            {
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPoint, currentPoint[0], 1, G, neckLength, 0, 0, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));
            }

            // Cover : Blind Flange
            if (selNozzle.Cover == "yes")
            {
                List<Point3D> coverPoint = new List<Point3D>();
                coverPoint.Add(GetSumPoint(drawStartPoint, 0, 0));
                coverPoint.Add(GetSumPoint(drawStartPoint, 0, 0));
                if (facingAdd)
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Pipe(out coverPoint, coverPoint[0], 0, R, facingC, true, true, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out coverPoint, coverPoint[1], 0, BCD, OD, BBF - facingC, 0, new DrawCenterLineModel() { scaleValue = selScaleValue,oneEx=true, twoEx = true }));
            }





            return customEntity;
        }
        private List<Entity> CreateFlangeLWN(CDPoint refPoint, Point3D drawPoint, Point3D drawStartPoint, NozzleInputModel selNozzle, FlangeOHFLWNModel drawNozzle, double selSizeNominalID, double selCenterTopHeight,double selScaleValue)
        {

            double G = valueService.GetDoubleValue(drawNozzle.G);
            double OD = valueService.GetDoubleValue(drawNozzle.OD);
            double BCD = valueService.GetDoubleValue(drawNozzle.BCD);
            double R = 0;
            double RRF = valueService.GetDoubleValue(drawNozzle.RRF);
            double RFF = valueService.GetDoubleValue(drawNozzle.RFF);
            double H = valueService.GetDoubleValue(drawNozzle.H);
            double B = valueService.GetDoubleValue(drawNozzle.B);
            double C = valueService.GetDoubleValue(drawNozzle.C);
            double facingC = 0;

            // Facing : Default : Rf
            bool facingAdd = false;
            if (selNozzle.Facing.Contains("ff"))
            {
                R = RFF;
                facingC = 0;
            }
            else
            {
                facingAdd = true;
                facingC = C;
                R = RRF;
            }

            // Internal Point
            Point3D currentInternalPoint = GetSumPoint(drawStartPoint, 0, -Point3D.Distance(drawPoint, drawStartPoint));// 아래쪽으로 이동

            List<Entity> customEntity = new List<Entity>();


            List<Point3D> currentPoint = new List<Point3D>();
            currentPoint.Add(GetSumPoint(drawStartPoint, 0, 0));

            // Facing
            if (facingAdd)
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Pipe(out currentPoint, currentPoint[0], 1, R, facingC, true, true, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));

            // Flange
            customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out currentPoint, currentPoint[0], 1, BCD, OD, B-facingC, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = !facingAdd, twoEx =true }));


            // PipeNeck
            double flangeLength = A;
            double neckLength = Point3D.Distance(drawStartPoint, drawPoint) - flangeLength;
            double pipeSlope = valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope);

            if (selNozzle.Position == "roof")
            {
                if (selNozzle.LR == "right")
                {
                    pipeSlope = -pipeSlope;
                }
            }
            else if (selNozzle.Position == "shell")
                pipeSlope = 0;

            // Internal 전용
            if (selNozzle.InternalPipe == "yes" || selNozzle.DrainType != "")
            {
                double newNeckLength = GetInternalPipeLengthAll(selNozzle, refPoint, drawPoint);
                neckLength += newNeckLength;
                pipeSlope = 0;
            }


            // Reinf Pady -> 먼저 그릴 것 : Neck 길이 조절
            double couplingHeight = 0;
            if (selNozzle.RePadType != "")
            {
                customEntity.AddRange(CreateReinforcingPAD(refPoint, drawPoint, selNozzle, selSizeNominalID, selScaleValue, ref couplingHeight));
            }

            // Neck
            customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPoint, currentPoint[0], 1, G, neckLength, pipeSlope, 0, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));

            // PipeNeck
            double flangeLength = B;
            double neckLength = Point3D.Distance(drawStartPoint, drawPoint)-flangeLength;
            if (selNozzle.Position == "roof")
            {
                double pipeSlope = valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope);
                if (selNozzle.LR == "left")
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPoint, currentPoint[0], 1, H, neckLength, pipeSlope, 0, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));
                else if(selNozzle.LR=="right")
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPoint, currentPoint[0], 1, H, neckLength, -pipeSlope, 0, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, twoEx=true }));
            }
            else if (selNozzle.Position == "shell")
            {
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPoint, currentPoint[0], 1, H, neckLength, 0, 0, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));
            }

            // Cover : Blind Flange
            if (selNozzle.Cover == "yes")
            {
                List<Point3D> coverPoint = new List<Point3D>();
                coverPoint.Add(GetSumPoint(drawStartPoint, 0, 0));
                coverPoint.Add(GetSumPoint(drawStartPoint, 0, 0));
                if (facingAdd)
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Pipe(out coverPoint, coverPoint[0], 0, R, facingC, true, true, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out coverPoint, coverPoint[1], 0, BCD, OD, B - facingC, 0, new DrawCenterLineModel() { scaleValue = selScaleValue,oneEx=true, twoEx = true }));
            }





            return customEntity;
        }
        private List<Entity> CreateFlange(CDPoint refPoint, Point3D drawPoint, Point3D drawStartPoint, NozzleInputModel selNozzle, FlangeOHFModel drawNozzle, double selSizeNominalID, double selCenterTopHeight, double selScaleValue)
        {
            double G = valueService.GetDoubleValue(drawNozzle.G);
            double OD = valueService.GetDoubleValue(drawNozzle.OD);
            double BCD = valueService.GetDoubleValue(drawNozzle.BCD);
            double R = 0;
            double RRF = valueService.GetDoubleValue(drawNozzle.RRF);
            double RFF = valueService.GetDoubleValue(drawNozzle.RFF);
            double H = valueService.GetDoubleValue(drawNozzle.H);
            double A = 0;
            double AWN = valueService.GetDoubleValue(drawNozzle.AWN);
            double ASO = valueService.GetDoubleValue(drawNozzle.ASO);
            double B = valueService.GetDoubleValue(drawNozzle.B);
            double C = valueService.GetDoubleValue(drawNozzle.C);
            double facingC = 0;

            // Facing : Default : Rf
            bool facingAdd = false;
            if (selNozzle.Facing.Contains("ff"))
            {
                R = RFF;
                facingC = 0;
            }
            else
            {
                facingAdd = true;
                facingC = C;
                R = RRF;
            }
            if (selNozzle.Type.Contains("wn"))
            {
                A = AWN;
            }
            else
            {
                A = ASO;
            }

            // Internal Point
            Point3D currentInternalPoint = GetSumPoint(drawStartPoint, 0, -Point3D.Distance(drawPoint, drawStartPoint));// 아래쪽으로 이동

            List<Entity> customEntity = new List<Entity>();


            List<Point3D> currentPoint = new List<Point3D>();
            currentPoint.Add(GetSumPoint(drawStartPoint, 0, 0));


            // Facing
            if (facingAdd)
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Pipe(out currentPoint, currentPoint[0], 1, R, facingC, true, true, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));

            // Flange
            customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out currentPoint, currentPoint[0], 1, BCD, OD, B - facingC, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = !facingAdd, twoEx = true }));
            // Neck
            customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Neck(out currentPoint, currentPoint[0], 1, G, H, A - B, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));



            // PipeNeck
            double flangeLength = A;
            double neckLength = Point3D.Distance(drawStartPoint, drawPoint) - flangeLength;
            double pipeSlope = valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope);

            if (selNozzle.Position == "roof")
            {
                if (selNozzle.LR == "right")
                {
                    pipeSlope = -pipeSlope;
                }
            }
            else if (selNozzle.Position == "shell")
                pipeSlope = 0;

            // Internal 전용
            if (selNozzle.InternalPipe=="yes" || selNozzle.DrainType != "")
            {
                double newNeckLength = GetInternalPipeLengthAll(selNozzle, refPoint, drawPoint);
                neckLength += newNeckLength;
                pipeSlope = 0;
            }


            // Reinf Pady -> 먼저 그릴 것 : Neck 길이 조절
            double couplingHeight = 0;
            if (selNozzle.RePadType != "")
            {
                customEntity.AddRange(CreateReinforcingPAD(refPoint, GetSumPoint(currentInternalPoint, 0,0), selNozzle, selSizeNominalID, selScaleValue, ref couplingHeight));
            }

            // Neck
            customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPoint, currentPoint[0], 1, G, neckLength, pipeSlope, 0, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));

            // Cover : Blind Flange
            if (selNozzle.Cover == "yes")
            {
                List<Point3D> coverPoint = new List<Point3D>();
                coverPoint.Add(GetSumPoint(drawStartPoint, 0, 0));
                coverPoint.Add(GetSumPoint(drawStartPoint, 0, 0));
                if (facingAdd)
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Pipe(out coverPoint, coverPoint[0], 0, R, facingC, true, true, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out coverPoint, coverPoint[1], 0, BCD, OD, B - facingC, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true, twoEx = true }));
            }


            // Internal : Pipe Joint // SO, RF 
            if (selNozzle.PipeJoint != "")
            {
                List<Entity> pipeJointList = new List<Entity>();
                List<Point3D> pipeJointPoint = new List<Point3D>();

                // joint Gap
                double jointGap = GetPipeJointAll(selNozzle);
                Point3D jointRealPoint = GetSumPoint(currentInternalPoint, 0, -jointGap);
                pipeJointPoint.Add(jointRealPoint);

                // Facing : RF
                pipeJointList.AddRange(nozzleBlock.DrawReference_Nozzle_Pipe(out pipeJointPoint, pipeJointPoint[0], 1, R, C, true, true, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));
                // Flange
                pipeJointList.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out pipeJointPoint, pipeJointPoint[0], 1, BCD, OD, B - C, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = !facingAdd, twoEx = true }));
                // Neck
                pipeJointList.AddRange(nozzleBlock.DrawReference_Nozzle_Neck(out pipeJointPoint, pipeJointPoint[0], 1, G, H, ASO - B, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));

                // Mirror : 위로
                List<Entity> pipeJointListMirror = new List<Entity>();
                pipeJointListMirror.AddRange(SetMirrorFunction(Plane.XZ, pipeJointList, jointRealPoint.X, jointRealPoint.Y, true));

                customEntity.AddRange(pipeJointList);
                customEntity.AddRange(pipeJointListMirror);
            }


            return customEntity;
        }
        #region Drain Type
        private void SetDrainLowerPoint(NozzleInputModel selNozzle,CDPoint refPoint,ref Point3D drawPoint, ref Point3D drawStartPoint)
        {

            double underValue = GetDrainLowerUnderHeight(selNozzle, refPoint);

            drawStartPoint.Y = refPoint.Y - underValue;
            double drainLowerOD = GetDrainLowerFoundationOD(selNozzle, refPoint, drawPoint);
            drawStartPoint.X = refPoint.X - drainLowerOD;

            drawPoint.Y = refPoint.Y - underValue;


        }
        private double GetDrainLowerUnderHeight(NozzleInputModel selNozzle,CDPoint refPoint)
        {
            double returnValue = 0;
            DrainLowerModel newModel = GetDrainLower(selNozzle);
            if (newModel != null)
            {
                returnValue= valueService.GetDoubleValue(newModel.C);
            }
            return returnValue;
        }

        #endregion

        #region Internal Pipe Length
        private double GetInternalPipeLengthAll(NozzleInputModel selNozzle,CDPoint refPoint, Point3D drawPoint)
        {
            double internalLength = 0;
            if (selNozzle.DrainType != "")
            {
                internalLength = GetInternalPipeLengthByDrain(selNozzle, refPoint, drawPoint);
            }
            else
            {
                internalLength = GetInternalPipeLengthByInternal(selNozzle, refPoint,drawPoint);
            }

            return internalLength;
        }

        private double GetInternalPipeLengthByDrain(NozzleInputModel selNozzle, CDPoint refPoint, Point3D drawPoint)
        {
            double internalLength = 0;
            if (selNozzle.DrainType == "sump")
            {
                DrainSumpModel newModel = GetDrainSump(selNozzle);
                ElbowModel newElbow = GetElbow(selNozzle);
                if (newModel != null && newElbow != null)
                {
                    internalLength = valueService.GetDoubleValue(newModel.C) - valueService.GetDoubleValue(newElbow.LRA);
                }

            }
            else if (selNozzle.DrainType == "internal pipe")
            {
                DrainInternalModel newModel = GetDrainInternalPipe(selNozzle);
                if (newModel != null)
                    internalLength = valueService.GetDoubleValue(newModel.A) + valueService.GetDoubleValue(newModel.A1);
            }
            else if (selNozzle.DrainType == "lower")
            {
                DrainLowerModel newModel = GetDrainLower(selNozzle);
                ElbowModel newElbow = GetElbow(selNozzle);
                if (newModel != null && newElbow != null)
                {
                    internalLength = valueService.GetDoubleValue(newModel.B) - valueService.GetDoubleValue(newElbow.LRA);
                }
            }

            return internalLength;
        }
        private double GetDrainLowerFoundationOD(NozzleInputModel selNozzle, CDPoint refPoint, Point3D drawPoint)
        {
            double returnValue = 0;
            DrainLowerModel newModel = GetDrainLower(selNozzle);
            if (newModel != null)
            {
                // Foundtation O.D +100
                // Bottom Plate Type 에 따라 다름 : 전체
                returnValue = valueService.GetDoubleValue(newModel.A1);
                double foundationOD = publicFunService.GetBottomFocuntionOD(assemblyData.BottomInput[0].AnnularPlate) + 100;

                returnValue += foundationOD;

            }
            return returnValue;
        }


        private double GetInternalPipeLengthByInternal(NozzleInputModel selNozzle,CDPoint refPoint, Point3D drawPoint)
        {
            double internalLength = 0;
            InOutInternalPipe newInOutPipe = GetInletOutletPipe(selNozzle);
            if (newInOutPipe != null)
            {
                if (selNozzle.Position == "shell")
                {
                    if (selNozzle.PipeJoint.Contains("flange"))
                    {
                        internalLength = valueService.GetDoubleValue(newInOutPipe.A) + valueService.GetDoubleValue(newInOutPipe.B);
                    }
                    else if (selNozzle.PipeJoint.Contains("welding"))
                    {
                        internalLength = valueService.GetDoubleValue(newInOutPipe.A);
                    }
                    else
                    {
                        // 없으면 welding으로 간주
                        internalLength = valueService.GetDoubleValue(newInOutPipe.A);
                    }
                }
                else if (selNozzle.Position == "roof")
                {
                    if (selNozzle.InternalPipeBended == "yes")
                    {
                        InternalBendedTypeModel newBended= GetInletOutletPipeBended(selNozzle);
                        ElbowModel newElbow = GetElbow(selNozzle);
                        if (newBended != null && newElbow !=null)
                        {
                            internalLength = valueService.GetDoubleValue(newBended.I);
                            // 무조건 45
                            internalLength = internalLength - valueService.GetDoubleValue(newElbow.LRB);
                        }
                    }
                    else
                    {
                        internalLength = drawPoint.Y - refPoint.Y - valueService.GetDoubleValue(newInOutPipe.F);

                    }
                }


            }
            return internalLength;
        }
        #endregion

        #region Pipe Joint : Gap
        private double GetPipeJointAll(NozzleInputModel selNozzle)
        {
            double jointGap = 0;
            if (selNozzle.DrainType != "")
            {
                jointGap = GetPipeJointGapByDrain(selNozzle);
            }
            else
            {
                jointGap = GetPipeJointGapByInternalPipe(selNozzle);
            }
            return jointGap;
        }

        private double GetPipeJointGapByInternalPipe(NozzleInputModel selNozzle)
        {
            double jointGap = 0;
            InOutInternalPipe newInOutPipe = GetInletOutletPipe(selNozzle);
            if (newInOutPipe != null)
            {
                if (selNozzle.Position == "shell")
                    jointGap = valueService.GetDoubleValue(newInOutPipe.A);
                else if (selNozzle.Position == "roof")
                    jointGap = valueService.GetDoubleValue(newInOutPipe.E);
            }
            return jointGap;
        }
        public double GetPipeJointGapByDrain(NozzleInputModel selNozzle)
        {
            double jointGap = 0;
            if (selNozzle.DrainType == "sump")
            {
                DrainSumpModel newModel = GetDrainSump(selNozzle);
                if (newModel != null)
                    jointGap = valueService.GetDoubleValue(newModel.D);
                    
            }
            else if (selNozzle.DrainType=="internal pipe")
            {
                DrainInternalModel newModel = GetDrainInternalPipe(selNozzle);
                if (newModel != null)
                    jointGap = valueService.GetDoubleValue(newModel.A);
            }

            return jointGap;
        }
        #endregion

        #region Nozzel Other Table
        private ElbowModel GetElbow(NozzleInputModel selNozzle)
        {
            ElbowModel returnValue = null;
            string pipeSize = selNozzle.Size;
            foreach (ElbowModel eachPipe in assemblyData.ElbowList)
            {
                if (pipeSize == eachPipe.NPS)
                {
                    returnValue = eachPipe;
                    break;
                }
            }
            return returnValue;
        }
        private InOutInternalPipe GetInletOutletPipe(NozzleInputModel selNozzle)
        {
            InOutInternalPipe returnValue = null; 
            string pipeSize = selNozzle.Size;
            foreach (InOutInternalPipe eachPipe in assemblyData.InOutInternalPipeList)
            {
                if(pipeSize == eachPipe.NPS)
                {
                    returnValue = eachPipe;
                    break;
                }
            }
            return returnValue;
        }
        private InternalBendedTypeModel GetInletOutletPipeBended(NozzleInputModel selNozzle)
        {
            InternalBendedTypeModel returnValue = null;
            string pipeSize = selNozzle.Size;
            foreach (InternalBendedTypeModel eachPipe in assemblyData.InternalBendedList)
            {
                if (pipeSize == eachPipe.NPS)
                {
                    returnValue = eachPipe;
                    break;
                }
            }
            return returnValue;
        }
        private DrainSumpModel GetDrainSump(NozzleInputModel selNozzle)
        {
            DrainSumpModel returnValue = null;
            string pipeSize = selNozzle.Size;
            foreach (DrainSumpModel eachPipe in assemblyData.DrainSumpList)
            {
                if (pipeSize == eachPipe.NPS)
                {
                    returnValue = eachPipe;
                    break;
                }
            }
            return returnValue;
        }
        private DrainInternalModel GetDrainInternalPipe(NozzleInputModel selNozzle)
        {
            DrainInternalModel returnValue = null;
            string pipeSize = selNozzle.Size;
            foreach (DrainInternalModel eachPipe in assemblyData.DrainInternalList)
            {
                if (pipeSize == eachPipe.NPS)
                {
                    returnValue = eachPipe;
                    break;
                }
            }
            return returnValue;
        }
        private DrainLowerModel GetDrainLower(NozzleInputModel selNozzle)
        {
            DrainLowerModel returnValue = null;
            string pipeSize = selNozzle.Size;
            foreach (DrainLowerModel eachPipe in assemblyData.DrainLowerList)
            {
                if (pipeSize == eachPipe.NPS)
                {
                    returnValue = eachPipe;
                    break;
                }
            }
            return returnValue;
        }

        private UBoltModel GetUBolt(NozzleInputModel selNozzle)
        {
            UBoltModel returnValue = null;
            string pipeSize = selNozzle.Size;
            foreach (UBoltModel eachPipe in assemblyData.UBoltList)
            {
                if (pipeSize == eachPipe.NPS)
                {
                    returnValue = eachPipe;
                    break;
                }
            }
            return returnValue;
        }
        private AngleSizeModel GetAngle(string angelSize)
        {
            angelSize = angelSize.Replace(" ", "");
            AngleSizeModel returnValue = null;
            foreach (AngleSizeModel eachPipe in assemblyData.AngleIList)
            {
                if (angelSize == eachPipe.SIZE)
                {
                    returnValue = eachPipe;
                    break;
                }
            }
            return returnValue;
        }


        #endregion

        public List<Entity> CreateUBlot(out List<Point3D> selOutputPointList, Point3D selPoint1, int selPointNumber, NozzleInputModel selNozzle, double selDistance,double selPadThickness, double selPadGap, double selRotate = 0, DrawCenterLineModel selCenterLine = null)
        {
            List<Entity> newList = new List<Entity>();

            UBoltModel uboltModel = GetUBolt(selNozzle);
            if (uboltModel != null)
            {
                AngleSizeModel angleModel = GetAngle(uboltModel.SupportAngle);
                if (angleModel != null)
                {
                    double AngleWidth = valueService.GetDoubleValue(angleModel.A);
                    double AngleT = valueService.GetDoubleValue(angleModel.t);

                    double P = valueService.GetDoubleValue(uboltModel.P);
                    double Q = valueService.GetDoubleValue(uboltModel.Q);
                    double S = valueService.GetDoubleValue(uboltModel.S);
                    double D3 = valueService.GetDoubleValue(uboltModel.D3);

                    double PHalf = P / 2;
                    double fullLength = selDistance + Q;
                    double fullLengthAngle = fullLength - selPadThickness;
                    double padWidth = AngleWidth + selPadGap * 2;
                    double leftAngelWidth = S;
                    double rightAngleWidth = AngleWidth - S;
                    double holeRadius = D3 / 2;

                    // WP : Left Lower
                    Point3D WP = new Point3D(selPoint1.X, selPoint1.Y);
                    Point3D WPRotate = WP;

                    List<Point3D> outputPointList = new List<Point3D>();
                    outputPointList.Add(GetSumPoint(WP, 0, 0));
                    // Pad
                    List<Entity> pipe0 = nozzleBlock.DrawReference_Nozzle_Pipe(out outputPointList, outputPointList[0], 0, padWidth, selPadThickness, true, true, 0, new DrawCenterLineModel() { scaleValue = selCenterLine.scaleValue, zeroEx = true});


                    // Drawing Shape
                    Line line00 = null;
                    Line line01 = null;

                    line00 = new Line(GetSumPoint(outputPointList[1], -leftAngelWidth, 0), GetSumPoint(outputPointList[1], rightAngleWidth, 0));
                    line01 = new Line(GetSumPoint(outputPointList[1], -leftAngelWidth, fullLengthAngle), GetSumPoint(outputPointList[1], rightAngleWidth, fullLengthAngle));

                    Line exLine01 = new Line(GetSumPoint(line00.StartPoint, 0, 0), GetSumPoint(line01.StartPoint, 0, 0));
                    Line exLine02 = new Line(GetSumPoint(line00.EndPoint, 0, 0), GetSumPoint(line01.EndPoint, 0, 0));



                    newList.AddRange(new Line[] { exLine01, exLine02 });

                    newList.Add(line00);
                    newList.Add(line01);

                    styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);


                    //Pad : Add
                    newList.AddRange(pipe0);

                    List<Entity> circle01 = nozzleBlock.DrawReference_Hole(GetSumPoint(outputPointList[1], 0, fullLengthAngle -Q - P / 2), holeRadius, 0, new DrawCenterLineModel() { scaleValue = selCenterLine.scaleValue, zeroEx = true });
                    List<Entity> circle02 = nozzleBlock.DrawReference_Hole(GetSumPoint(outputPointList[1], 0, fullLengthAngle -Q + P / 2), holeRadius, 0, new DrawCenterLineModel() { scaleValue = selCenterLine.scaleValue, zeroEx = true });
                    newList.AddRange(circle01);
                    newList.AddRange(circle02);

                    // hidden
                    Line leftHidden = new Line(GetSumPoint(line00.StartPoint, AngleT, 0), GetSumPoint(line01.StartPoint, AngleT, 0));
                    styleService.SetLayer(ref leftHidden, layerService.LayerHiddenLine);
                    newList.Add(leftHidden);

                    Line centerLine00=null;
                    // Center Line
                    if (selCenterLine != null)
                    {
                        if (selCenterLine.centerLine)
                        {
                            // 기본 형상
                            Point3D zeroPoint = GetSumPoint(outputPointList[1], 0, 0);
                            Point3D onePoint = GetSumPoint(outputPointList[1], 0, fullLengthAngle);
                            centerLine00 = new Line(zeroPoint, onePoint);

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
                                translatePoint = GetSumPoint(outputPointList[0], 0, 0);
                            else if (selPointNumber == 1)
                                translatePoint = GetSumPoint(centerLine00.EndPoint, 0, 0);
                        }
                        if (translatePoint != null)
                            SetTranslate(ref newList, WP, translatePoint);

                    }

                    selOutputPointList = new List<Point3D>();
                    selOutputPointList.Add(line00.MidPoint);
                    selOutputPointList.Add(line01.MidPoint);


                }
            }

            selOutputPointList = new List<Point3D>();

            return newList;
        }


        private void SetTranslate(ref List<Entity> selList, Point3D refPoint, Point3D currentPoint)
        {
            double distanceY = refPoint.Y - currentPoint.Y;
            double distanceX = refPoint.X - currentPoint.X;
            Vector3D tempMovement = new Vector3D(distanceX, distanceY);
            foreach (Entity eachEntity in selList)
                eachEntity.Translate(tempMovement);
        }

        private List<Entity> CreateManhole(CDPoint refPoint, Point3D drawPoint, Point3D drawStartPoint, NozzleInputModel selNozzle, double selSizeNominalID, double selCenterTopHeight, double selScaleValue)
        {

            List<Entity> customEntity = new List<Entity>();

            string nozzleSize = selNozzle.Size;
            RoofManholeModel newManhole = null;
            foreach (RoofManholeModel eachManhole in assemblyData.RoofManholeList)
            {
                if (nozzleSize == eachManhole.NPS)
                {
                    newManhole = eachManhole;
                    break;
                }
            }

            if (newManhole != null)
            {
                double D1 = valueService.GetDoubleValue(newManhole.D1);
                double BCD = valueService.GetDoubleValue(newManhole.BCD);
                double D2 = valueService.GetDoubleValue(newManhole.D2);
                double L = valueService.GetDoubleValue(newManhole.L);

                double roofToManhole = valueService.GetDoubleValue(newManhole.RoofToManhole);
                double topToManhole = valueService.GetDoubleValue(newManhole.TopToManhole);
                double manholeThickness = valueService.GetDoubleValue(newManhole.ManholeThickness);

                double T6 = 6;
                double manholeOD = D1 + T6 * 2;
               

                List<Point3D> currentPoint = new List<Point3D>();
                currentPoint.Add(GetSumPoint(drawStartPoint, 0, 0));

                // Flange
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out currentPoint, currentPoint[0], 1, BCD, D2, T6, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true, twoEx = true }));

                // PipeNeck
                double flangeLength = T6;
                double neckLength = Point3D.Distance(drawStartPoint, drawPoint) - flangeLength;
                // Pad
                double padThickness = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].RoofPlateThickness);
                double pipeSlope = valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope);
                double padBottomGap = valueService.GetHypotenuseByWidth(pipeSlope,padThickness);
                if (selNozzle.Position == "roof")
                {

                    if (selNozzle.LR == "left")
                    {
                        customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPoint, currentPoint[0], 1, manholeOD, neckLength, pipeSlope, 0, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));
                        customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PadSlope(out currentPoint, GetSumPoint(currentPoint[0],0,padBottomGap), 0, manholeOD, L, padThickness, true, pipeSlope, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));
                    }
                    else if (selNozzle.LR == "right")
                    {
                        customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPoint, currentPoint[0], 1, manholeOD, neckLength, -pipeSlope, 0, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, twoEx = true }));
                        customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PadSlope(out currentPoint, GetSumPoint(currentPoint[0], 0, padBottomGap), 0, manholeOD, L, padThickness, true, -pipeSlope, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));
                    }

                }
                else if (selNozzle.Position == "shell")
                {
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPoint, currentPoint[0], 1, manholeOD, neckLength, 0, 0, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PadSlope(out currentPoint, GetSumPoint(currentPoint[0], 0, padBottomGap), 0, manholeOD, L, padThickness, true, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));
                }




                // Cover : Blind Flange
                if (selNozzle.Cover == "yes")
                {
                    List<Point3D> coverPoint = new List<Point3D>();
                    coverPoint.Add(GetSumPoint(drawStartPoint, 0, 0));
                    coverPoint.Add(GetSumPoint(drawStartPoint, 0, 0));
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out coverPoint, coverPoint[1], 0, BCD, D2,T6, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true, twoEx = true }));
                }


            }

            return customEntity;
        }

        private List<Entity> CreateCleanout(CDPoint refPoint, Point3D drawPoint, Point3D drawStartPoint, NozzleInputModel selNozzle, double selSizeNominalID, double selCenterTopHeight, double selScaleValue, double selCleanoutWidth)
        {

            List<Entity> customEntity = new List<Entity>();

            string nozzleSize = selNozzle.Size;
            ShellCleanOutModel newCleanout = null;
            foreach (ShellCleanOutModel eachCleanout in assemblyData.ShellCleanOutList)
            {
                if (nozzleSize == eachCleanout.SizeInch)
                {
                    newCleanout = eachCleanout;
                    break;
                }
            }

            if (newCleanout != null)
            {
                double H = valueService.GetDoubleValue(newCleanout.H);
                double B = valueService.GetDoubleValue(newCleanout.B);
                double W = valueService.GetDoubleValue(newCleanout.W);
                double R1 = valueService.GetDoubleValue(newCleanout.R1);
                double R2 = valueService.GetDoubleValue(newCleanout.R2);
                double E = valueService.GetDoubleValue(newCleanout.E);
                double F3 = valueService.GetDoubleValue(newCleanout.F3);
                double L = valueService.GetDoubleValue(newCleanout.L);
                double F2 = valueService.GetDoubleValue(newCleanout.F2);
                double G = valueService.GetDoubleValue(newCleanout.G);

        



                List<Point3D> currentPoint = new List<Point3D>();
                currentPoint.Add(GetSumPoint(refPoint, selSizeNominalID/2 + selCleanoutWidth, 0));
                Point3D newDrawPoint = GetSumPoint(refPoint, selSizeNominalID / 2 + selCleanoutWidth +W/2, 0);

                Line lineBottom = new Line(GetSumPoint(newDrawPoint, 0, 0), GetSumPoint(newDrawPoint, W / 2, 0));
                customEntity.Add(lineBottom);
                styleService.SetLayer(ref lineBottom, layerService.LayerOutLine);

                //01
                Line lineTop1 = new Line(GetSumPoint(newDrawPoint, 0, L), GetSumPoint(newDrawPoint, W / 2, L));
                Line lineRight1 = new Line(GetSumPoint(newDrawPoint, W / 2, L), GetSumPoint(newDrawPoint, W / 2, 0));
                Arc arcFillet1;
                if (Curve.Fillet(lineTop1, lineRight1, R2, false, false, true, true, out arcFillet1))
                    customEntity.Add(arcFillet1);
                styleService.SetLayer(ref arcFillet1, layerService.LayerOutLine);
                if (arcFillet1.StartPoint.X > newDrawPoint.X)
                {
                    Line lineAdd1 = new Line(GetSumPoint(newDrawPoint, 0, L), arcFillet1.StartPoint);
                    styleService.SetLayer(ref lineAdd1, layerService.LayerOutLine);
                    customEntity.Add(lineAdd1);
                }
                if (arcFillet1.EndPoint.Y > newDrawPoint.Y)
                {
                    Line lineAdd1 = new Line(GetSumPoint(newDrawPoint, W / 2, 0), arcFillet1.EndPoint);
                    styleService.SetLayer(ref lineAdd1, layerService.LayerOutLine);
                    customEntity.Add(lineAdd1);
                }


                Line lineTop2 = new Line(GetSumPoint(newDrawPoint, 0, H+F3), GetSumPoint(newDrawPoint, B/2 +F3, H + F3));
                Line lineRight2 = new Line(GetSumPoint(newDrawPoint, B / 2 + F3, H + F3), GetSumPoint(newDrawPoint, B / 2 + F3, 0));
                Arc arcFillet2;
                if (Curve.Fillet(lineTop2, lineRight2, R1, false, false, true, true, out arcFillet2))
                    customEntity.Add(arcFillet2);
                styleService.SetLayer(ref arcFillet2, layerService.LayerOutLine);
                if (arcFillet2.StartPoint.X > newDrawPoint.X)
                {
                    Line lineAdd1 = new Line(GetSumPoint(newDrawPoint, 0, H+F3), arcFillet2.StartPoint);
                    styleService.SetLayer(ref lineAdd1, layerService.LayerOutLine);
                    customEntity.Add(lineAdd1);
                }
                if (arcFillet2.EndPoint.Y > newDrawPoint.Y)
                {
                    Line lineAdd1 = new Line(GetSumPoint(newDrawPoint, B/2+F3, 0), arcFillet2.EndPoint);
                    styleService.SetLayer(ref lineAdd1, layerService.LayerOutLine);
                    customEntity.Add(lineAdd1);
                }


                Line lineTop3 = new Line(GetSumPoint(newDrawPoint, 0, H + F3 -E), GetSumPoint(newDrawPoint, B / 2 + F3 - E, H + F3 - E));
                Line lineRight3 = new Line(GetSumPoint(newDrawPoint, B / 2 + F3 - E, H + F3 - E), GetSumPoint(newDrawPoint, B / 2 + F3 - E, 0));
                Arc arcFillet3;
                if (Curve.Fillet(lineTop3, lineRight3, R1, false, false, true, true, out arcFillet3))
                    customEntity.Add(arcFillet3);
                styleService.SetLayer(ref arcFillet3, layerService.LayerCenterLine);
                if (arcFillet3.StartPoint.X > newDrawPoint.X)
                {
                    Line lineAdd1 = new Line(GetSumPoint(newDrawPoint, 0, H + F3-E), arcFillet3.StartPoint);
                    styleService.SetLayer(ref lineAdd1, layerService.LayerCenterLine);
                    customEntity.Add(lineAdd1);
                }
                if (arcFillet3.EndPoint.Y > newDrawPoint.Y)
                {
                    Line lineAdd1 = new Line(GetSumPoint(newDrawPoint, B / 2 + F3-E, 0), arcFillet3.EndPoint);
                    styleService.SetLayer(ref lineAdd1, layerService.LayerCenterLine);
                    customEntity.Add(lineAdd1);
                }


                Line lineTop4 = new Line(GetSumPoint(newDrawPoint, 0, H ), GetSumPoint(newDrawPoint, B / 2 , H));
                Line lineRight4 = new Line(GetSumPoint(newDrawPoint, B / 2, H ), GetSumPoint(newDrawPoint, B / 2 , 0));
                Arc arcFillet4;
                if (Curve.Fillet(lineTop4, lineRight4, R1, false, false, true, true, out arcFillet4))
                    customEntity.Add(arcFillet4);
                styleService.SetLayer(ref arcFillet4, layerService.LayerHiddenLine);
                if (arcFillet4.StartPoint.X > newDrawPoint.X)
                {
                    Line lineAdd1 = new Line(GetSumPoint(newDrawPoint, 0, H ), arcFillet4.StartPoint);
                    styleService.SetLayer(ref lineAdd1, layerService.LayerHiddenLine);
                    customEntity.Add(lineAdd1);
                }
                if (arcFillet4.EndPoint.Y > newDrawPoint.Y)
                {
                    Line lineAdd1 = new Line(GetSumPoint(newDrawPoint, B / 2 , 0), arcFillet4.EndPoint);
                    styleService.SetLayer(ref lineAdd1, layerService.LayerHiddenLine);
                    customEntity.Add(lineAdd1);
                }




                //Mirror
                if (selCleanoutWidth>0)
                {
                    Plane pl1 = Plane.YZ;
                    pl1.Origin.X = newDrawPoint.X;
                    pl1.Origin.Y = newDrawPoint.Y;
                    Mirror customMirror = new Mirror(pl1);
                    List<Entity> mirrorList = new List<Entity>();
                    foreach (Entity eachEntity in customEntity)
                    {
                        Entity newEntity = (Entity)eachEntity.Clone();
                        newEntity.TransformBy(customMirror);
                        mirrorList.Add(newEntity);
                    }
                    customEntity.AddRange(mirrorList);
                }

            }

            return customEntity;
        }

        private List<Entity> CreateManholeCenter(CDPoint refPoint, Point3D drawPoint, Point3D drawStartPoint, NozzleInputModel selNozzle, double selSizeNominalID, double selCenterTopHeight, double selScaleValue)
        {

            List<Entity> customEntity = new List<Entity>();

            string nozzleSize = selNozzle.Size;
            ShellManholeModel newManhole = null;
            foreach (ShellManholeModel eachManhole in assemblyData.ShellManholeList)
            {
                if (nozzleSize == eachManhole.NPS)
                {
                    newManhole = eachManhole;
                    break;
                }
            }

            if (newManhole != null)
            {
                double D1 = valueService.GetDoubleValue(newManhole.D1);
                double BCD = valueService.GetDoubleValue(newManhole.BCD);
                double D2 = valueService.GetDoubleValue(newManhole.D2);
                double L = valueService.GetDoubleValue(newManhole.L);


                //List<Point3D> currentPoint = new List<Point3D>();
                //currentPoint.Add(GetSumPoint(drawStartPoint, 0, 0));

                //Line lineBottom = new Line(GetSumPoint(drawPoint, 0, 0), GetSumPoint(drawPoint, W / 2, 0));
                //customEntity.Add(lineBottom);
                //styleService.SetLayer(ref lineBottom, layerService.LayerOutLine);

                ////01
                //Line lineTop1 = new Line(GetSumPoint(drawPoint, 0, L), GetSumPoint(drawPoint, W / 2, L));
                //Line lineRight1 = new Line(GetSumPoint(drawPoint, W / 2, L), GetSumPoint(drawPoint, W / 2, 0));
                //Arc arcFillet1;
                //if (Curve.Fillet(lineTop1, lineRight1, R2, false, false, true, true, out arcFillet1))
                //    customEntity.Add(arcFillet1);
                //styleService.SetLayer(ref arcFillet1, layerService.LayerOutLine);
                //if (arcFillet1.StartPoint.X > drawPoint.X)
                //{
                //    Line lineAdd1 = new Line(GetSumPoint(drawPoint, 0, L), arcFillet1.StartPoint);
                //    styleService.SetLayer(ref lineAdd1, layerService.LayerOutLine);
                //    customEntity.Add(lineAdd1);
                //}
                //if (arcFillet1.EndPoint.Y > drawPoint.Y)
                //{
                //    Line lineAdd1 = new Line(GetSumPoint(drawPoint, W / 2, 0), arcFillet1.EndPoint);
                //    styleService.SetLayer(ref lineAdd1, layerService.LayerOutLine);
                //    customEntity.Add(lineAdd1);
                //}


                //Line lineTop2 = new Line(GetSumPoint(drawPoint, 0, H + F3), GetSumPoint(drawPoint, B / 2 + F3, H + F3));
                //Line lineRight2 = new Line(GetSumPoint(drawPoint, B / 2 + F3, H + F3), GetSumPoint(drawPoint, B / 2 + F3, 0));
                //Arc arcFillet2;
                //if (Curve.Fillet(lineTop2, lineRight2, R1, false, false, true, true, out arcFillet2))
                //    customEntity.Add(arcFillet2);
                //styleService.SetLayer(ref arcFillet2, layerService.LayerOutLine);
                //if (arcFillet2.StartPoint.X > drawPoint.X)
                //{
                //    Line lineAdd1 = new Line(GetSumPoint(drawPoint, 0, H + F3), arcFillet2.StartPoint);
                //    styleService.SetLayer(ref lineAdd1, layerService.LayerOutLine);
                //    customEntity.Add(lineAdd1);
                //}
                //if (arcFillet2.EndPoint.Y > drawPoint.Y)
                //{
                //    Line lineAdd1 = new Line(GetSumPoint(drawPoint, B / 2 + F3, 0), arcFillet2.EndPoint);
                //    styleService.SetLayer(ref lineAdd1, layerService.LayerOutLine);
                //    customEntity.Add(lineAdd1);
                //}



            }

            return customEntity;
        }


        private List<Entity> CreateReinforcingPAD(CDPoint refPoint, Point3D drawPoint,NozzleInputModel selNozzle,double selSizeNominalID,double selScaleValue,ref double couplingHeight)
        {

            double R = valueService.GetDoubleValue(selNozzle.R);
            double H = valueService.GetDoubleValue(selNozzle.H);
            double padThickness = 0;

            // Roof Slope

            double roofPointHeight = drawPoint.Y - refPoint.Y;


            double roofDegree =valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope);
            double roofDegreeValue = roofDegree;
            double padBottomGap = 0; 
            if (selNozzle.Position == "shell")
            {
                roofDegreeValue = 0;
                if (selNozzle.LR == "left")
                    padThickness = workingPointService.GetShellThicknessAccordingToHeight(selNozzle.H);
                else if (selNozzle.LR == "right")
                    padThickness = workingPointService.GetShellThicknessAccordingToHeight(selNozzle.H);

                padBottomGap = padThickness;
            }
            else if (selNozzle.Position == "roof")
            {
                padThickness = valueService.GetDoubleValue(assemblyData.RoofCompressionRing[0].RoofPlateThickness);
                if (selNozzle.LR == "left")
                    roofDegreeValue = roofDegree;
                else if (selNozzle.LR == "right")
                    roofDegreeValue = -roofDegree;

                padBottomGap= valueService.GetHypotenuseByWidth(roofDegree, padThickness);
            }


            // Neck 아래 부분에 위치
            List<Point3D> padPoint = new List<Point3D>();
            padPoint.Add(GetSumPoint(drawPoint, 0, 0));
            List<Entity> customEntity = new List<Entity>();
            if (selNozzle.RePadType.Contains("dia"))
            {
                foreach(ReinforcingPadModel eachPad in assemblyData.ReinforcingPadList)
                {
                    if (eachPad.Pipe == selNozzle.Size)
                    {
                        customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PadSlopeFlange(out padPoint, GetSumPoint(padPoint[0],0, padBottomGap), 0,valueService.GetDoubleValue( eachPad.DR),valueService.GetDoubleValue( eachPad.L), padThickness,true, roofDegreeValue, new DrawCenterLineModel() { scaleValue = selScaleValue }));
                        break;
                    }
                }
            }else if (selNozzle.RePadType.Contains("cir"))
            {
                foreach (ReinforcingPadModel eachPad in assemblyData.ReinforcingPadList)
                {
                    if (eachPad.Pipe == selNozzle.Size)
                    {
                        customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PadSlopeFlange(out padPoint, GetSumPoint(padPoint[0], 0, padBottomGap), 0, valueService.GetDoubleValue(eachPad.DR), valueService.GetDoubleValue(eachPad.L), padThickness,true, roofDegreeValue, new DrawCenterLineModel() { scaleValue = selScaleValue }));
                        break;
                    }
                }

            }
            else if (selNozzle.RePadType.Contains("coupling"))
            {
                // 아직 미적용
                if (selNozzle.RePadType.Contains("full)"))
                {

                }
                else if (selNozzle.RePadType.Contains("half)"))
                {

                }

            }
            return customEntity;
        }
        private NozzleBlock_Type CreateFlange_Block(CDPoint refPoint, Point3D drawPoint, NozzleInputModel selNozzle, ref Entity blockEntity)
        {
            NozzleBlock_Type returnValue = NozzleBlock_Type.Flange;
            string blockName = "";
            double scaleFactor = 1;
            string checkValue = "yes";

            // Block : 4
            if (selNozzle.RiserPipe == checkValue)
            {
                returnValue = NozzleBlock_Type.OnlyBlock;
                blockName = "";
            }
            else if (selNozzle.InternalLadder == checkValue)
            {
                returnValue = NozzleBlock_Type.OnlyBlock;
                blockName = "";
            }
            else if (selNozzle.WaterSpray == checkValue)
            {
                returnValue = NozzleBlock_Type.OnlyBlock;
                blockName = "";
            }
            else if (selNozzle.FoamConn == checkValue)
            {
                returnValue = NozzleBlock_Type.OnlyBlock;
                blockName = "";
            }
            else
            {
                //Flange 7 : Combi : 1
                if (selNozzle.Mixer == checkValue)
                {
                    blockName = string.Format("BLOCK-MIXER-{0}\"_{1}", selNozzle.Size, selNozzle.Rating);
                }
                else  if (selNozzle.GaugeHatch == checkValue)
                {
                    blockName = string.Format("BLOCK-GQUGE_HATCH-{0}\"_{1}", selNozzle.Size, selNozzle.Rating);
                }
                else if (selNozzle.GooseNeckBirdScreen == checkValue)
                {
                    blockName = string.Format("BLOCK--{0}\"_{1}", selNozzle.Size, selNozzle.Rating);
                }
                else if (selNozzle.FlameArrestor == checkValue && selNozzle.BreatherValve==checkValue)
                {
                    blockName = string.Format("BLOCK-FLAME_ARRESTOR&BREATHER_VALVE-{0}\"_{1}", selNozzle.Size, selNozzle.Rating);
                }
                else if (selNozzle.FlameArrestor == checkValue)
                {
                    blockName = string.Format("BLOCK-FLAME_ARRESTOR-{0}\"_{1}", selNozzle.Size, selNozzle.Rating);
                }
                else if (selNozzle.BreatherValve == checkValue)
                {
                    blockName = string.Format("BLOCK-BREATHER_VALVE-{0}\"_{1}", selNozzle.Size, selNozzle.Rating);
                }
                else if (selNozzle.VacuumReliefValve == checkValue)
                {
                    blockName = string.Format("BLOCK-VACUUM_RELIEF_VALVE-{0}\"_{1}", selNozzle.Size, selNozzle.Rating);
                }
                else if (selNozzle.EmergencyVent == checkValue)
                {
                    blockName = string.Format("BLOCK-EMERGENCY_VENT-{0}\"_{1}", selNozzle.Size, selNozzle.Rating);
                }
                else
                {

                 


                }
            }


            // Block Point 매우 중요함
            blockEntity = null;
            if (drawPoint!=null)
                blockEntity=blockImportService.Draw_ImportBlock(new CDPoint(drawPoint.X,drawPoint.Y,0), blockName, layerService.LayerBlock, scaleFactor);
            return returnValue;
        }

        private List<Entity> CreateInternal(CDPoint refPoint, Point3D drawPoint, NozzleInputModel selNozzle,double couplingHeight,double selScaleValue)
        {
            string checkValue="yes";
            List<Entity> customEntity = new List<Entity>();
            if (selNozzle.InternalPipe== checkValue)
            {
                if (selNozzle.Position == "shell")
                {
                    // Pipejoint -> Fitting : 공통
                    if (selNozzle.DrainType != "")
                    {
                        if (selNozzle.DrainType == "sump")
                        {
                            // Drain Sump
                            customEntity.AddRange(CreateDrainSump(refPoint, drawPoint, selNozzle, couplingHeight, selScaleValue));
                            
                        }
                        else if (selNozzle.DrainType == "internal pipe")
                        {
                            // Drain Internal Pipe
                            customEntity.AddRange(CreateDrainInternalPipe(refPoint, drawPoint, selNozzle, couplingHeight, selScaleValue));

                        }
                        else if (selNozzle.DrainType == "lower")
                        {
                            // Drain Lower
                            customEntity.AddRange(CreateDrainLower(refPoint, drawPoint, selNozzle, couplingHeight, selScaleValue));
                        }
                    }
                    else
                    {
                        customEntity.AddRange(CreateInternalBasic(refPoint, drawPoint, selNozzle, couplingHeight, selScaleValue));

                    }

                }
                else if (selNozzle.Position == "roof")
                {
                    // Pipejoint -> Fitting : 공통
                    if (selNozzle.InternalPipeBended==checkValue)
                    {
                        // Shell Bended Type
                        // 좌측 패드
                        customEntity.AddRange(CreateInternalBended(refPoint, drawPoint, selNozzle, couplingHeight, selScaleValue));
                    }
                    else
                    {
                        // Straight Type
                        // 아래쪽 패드
                    }
                }
            }

            return customEntity;
        }

        public List<Entity> CreateDrainLower(CDPoint refPoint, Point3D drawPoint, NozzleInputModel selNozzle, double couplingHeight,double selScaleValue)
        {
            CDPoint curPoint = new CDPoint();
            CDPoint bottomPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointReferenceBottom, 0, ref refPoint, ref curPoint);

            List<Entity> customEntity = new List<Entity>();
            List<Point3D> currentPointList = new List<Point3D>();

            DrainLowerModel newModel = GetDrainLower(selNozzle);
            ElbowModel newElbow = GetElbow(selNozzle);
            if (newModel!=null && newElbow!=null)
            {
                // 무조건 90
                double elbowA = valueService.GetDoubleValue(newElbow.LRA);
                double OD = valueService.GetDoubleValue(newElbow.OD);

                double B = valueService.GetDoubleValue(newModel.B);
                double C = valueService.GetDoubleValue(newModel.C);
                double D = valueService.GetDoubleValue(newModel.D);
                double E = valueService.GetDoubleValue(newModel.E);
                double C1 = valueService.GetDoubleValue(newModel.C1);
                double B1BarWidth = valueService.GetDoubleValue(newModel.B1BarWidth);

                double tankNominalHalf = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID)/2;
                double bottonThickness = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateThickness);
                currentPointList.Add(GetSumPoint(bottomPoint, B - elbowA, -C));
                List<Entity> elbowList0 = nozzleBlock.DrawReference_Nozzle_Elbow(out currentPointList, currentPointList[0], 1, newElbow, "lr", "90", Utility.DegToRad(180), new DrawCenterLineModel() { scaleValue = 90, zeroEx = true, oneEx = true });
                customEntity.AddRange(elbowList0);

                double pipeLength = 0;
                double pipeSlope = valueService.GetDegreeOfSlope(assemblyData.BottomInput[0].BottomPlateSlope);


                // Flat
                double outWidth = publicFunService.GetBottomFocuntionOD(assemblyData.BottomInput[0].AnnularPlate);
                double bottomThk = valueService.GetDoubleValue(assemblyData.ShellOutput[0].Thickness);
                Point3D flatPointUp = new Point3D();
                Point3D flatPointDown = new Point3D();

                if (assemblyData.BottomInput[0].AnnularPlate == "Yes")
                {
                    pipeSlope = 0;
                    pipeLength = bottomPoint.Y - currentPointList[0].Y;
                    flatPointUp = GetSumPoint(bottomPoint, -outWidth - bottomThk + C1 + (B1BarWidth / 2), 0);
                    flatPointDown = GetSumPoint(bottomPoint, -outWidth - bottomThk + C1 + (B1BarWidth / 2), -C + OD / 2);
                }
                else
                {
                    pipeSlope = pipeSlope;
                    CDPoint bottomPointDown = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomDown, tankNominalHalf - B, ref refPoint, ref curPoint);
                    pipeLength = bottomPointDown.Y - currentPointList[0].Y;

                    flatPointUp = GetSumPoint(bottomPoint, -outWidth - bottomThk + C1 + (B1BarWidth / 2), 0);
                    CDPoint bottomPointFlatUp = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomDown, tankNominalHalf+refPoint.X -flatPointUp.X, ref refPoint, ref curPoint);
                    flatPointUp = new Point3D(bottomPointFlatUp.X, bottomPointFlatUp.Y, 0);
                    flatPointDown = GetSumPoint(bottomPointFlatUp, 0,-C+OD/2);
                }

                

                // pipe
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPointList, currentPointList[0], 1, OD, pipeLength, pipeSlope, 0,Utility.DegToRad(180), new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));
                // pad
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PadSlope(out currentPointList, currentPointList[0], 1, OD, E, bottonThickness, true,  pipeSlope, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));



                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPointList, flatPointDown, 1, B1BarWidth, C- OD/2, pipeSlope, 0, Utility.DegToRad(180),null));
            }



            return customEntity;
        }

        public List<Entity> CreateDrainSump(CDPoint refPoint, Point3D drawPoint, NozzleInputModel selNozzle, double couplingHeight, double selScaleValue)
        {
            CDPoint curPoint = new CDPoint();
            CDPoint bottomPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointReferenceBottom, 0, ref refPoint, ref curPoint);

            List<Entity> customEntity = new List<Entity>();
            List<Point3D> currentPointList = new List<Point3D>();


            double newNeckLength = GetInternalPipeLengthAll(selNozzle, refPoint, drawPoint);

            DrainSumpModel newModel = GetDrainSump(selNozzle);
            ElbowModel newElbow = GetElbow(selNozzle);
            if (newModel != null && newElbow != null)
            {
                // 무조건 90
                double elbowA = valueService.GetDoubleValue(newElbow.LRA);
                double OD = valueService.GetDoubleValue(newElbow.OD);

                double A = valueService.GetDoubleValue(newModel.A);
                double B = valueService.GetDoubleValue(newModel.B);
                double t1 = valueService.GetDoubleValue(newModel.t1);
                double t2 = valueService.GetDoubleValue(newModel.t2);
                double t3 = valueService.GetDoubleValue(newModel.t3);
                double t4 = valueService.GetDoubleValue(newModel.t4);
                double C = valueService.GetDoubleValue(newModel.C);
                double D = valueService.GetDoubleValue(newModel.D);
                double E = valueService.GetDoubleValue(newModel.E);
                double F = valueService.GetDoubleValue(newModel.F);
                double G = valueService.GetDoubleValue(newModel.G);
                double A1 = valueService.GetDoubleValue(newModel.A1);


                double tankNominalHalf = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID) / 2;
                double bottonThickness = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateThickness);
                currentPointList.Add(GetSumPoint(drawPoint, newNeckLength, 0));
                List<Entity> elbowList0 = nozzleBlock.DrawReference_Nozzle_Elbow(out currentPointList, currentPointList[0], 0, newElbow, "lr", "90", Utility.DegToRad(-90), new DrawCenterLineModel() { scaleValue = 90, zeroEx = true });
                customEntity.AddRange(elbowList0);



                CDPoint bottomPointUp = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomDown, tankNominalHalf - newNeckLength - elbowA , ref refPoint, ref curPoint);
                Point3D bottomPointUpUp = GetSumPoint(bottomPointUp, 0, -B+E);
                Point3D buttomSupport = GetSumPoint(bottomPointUp, 0.5, 0);
                double pipeLength = currentPointList[1].Y - bottomPointUpUp.Y;
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPointList, currentPointList[1], 1, OD, pipeLength, 0, 0, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));

                // Support : Bottom
                Point3D buttonPlate = GetSumPoint(currentPointList[0], OD/2+65/2 +0.5, -E);
                List<Point3D> nothingList = new List<Point3D>();
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out nothingList, buttonPlate, 0, 65,F, 0, 0, 0, null));

                // Support : Middle
                double pipeSlope = valueService.GetDegreeOfSlope(assemblyData.BottomInput[0].BottomPlateSlope);
                CDPoint middleSupport = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, tankNominalHalf - G, ref refPoint, ref curPoint);
                Point3D middleUp = new Point3D(middleSupport.X, drawPoint.Y-OD/2);
                double middleDistance = Point3D.Distance(GetSumPoint(middleSupport, 0, 0), middleUp);
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out nothingList, middleUp, 1, 65, middleDistance, pipeSlope, 0, 0, null));

                // Square
                CDPoint bottomPointDownMiddle = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomDown, tankNominalHalf - newNeckLength - elbowA, ref refPoint, ref curPoint);
                CDPoint bottomPointLeft= workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomDown, tankNominalHalf - newNeckLength - elbowA  +A/2 - bottonThickness/2, ref refPoint, ref curPoint);
                CDPoint bottomPointRight = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomDown, tankNominalHalf - newNeckLength - elbowA - A / 2 + bottonThickness/2, ref refPoint, ref curPoint);


                // Bottom
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out nothingList, GetSumPoint(bottomPointDownMiddle,0,-B), 1, A, bottonThickness, 0, 0, 0, null));
                //Bottom Left
                Point3D leftBottomDownPoint =new Point3D(bottomPointLeft.X, GetSumPoint(bottomPointDownMiddle, 0, -B).Y);
                double leftBottonHeight =bottomPointLeft.Y - leftBottomDownPoint.Y;
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out nothingList, leftBottomDownPoint, 1, bottonThickness, leftBottonHeight,  pipeSlope,0, Utility.DegToRad(180), null));
                Point3D rightBottomDownPoint = new Point3D(bottomPointRight.X, GetSumPoint(bottomPointDownMiddle, 0, -B).Y);
                double rightBottonHeight = bottomPointRight.Y - rightBottomDownPoint.Y;
                //Bottom Right
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out nothingList, rightBottomDownPoint, 1, bottonThickness, rightBottonHeight, pipeSlope,0, Utility.DegToRad(180), null));

                // Pad
                CDPoint bottomPadPointUp = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, tankNominalHalf - newNeckLength - elbowA, ref refPoint, ref curPoint);
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PadSlope(out nothingList,GetSumPoint(bottomPadPointUp,0,0), 0, OD, A+A1*2, bottonThickness, true, pipeSlope, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));
            }



            return customEntity;
        }


        public List<Entity> CreateDrainInternalPipe(CDPoint refPoint, Point3D drawPoint, NozzleInputModel selNozzle, double couplingHeight, double selScaleValue)
        {
            CDPoint curPoint = new CDPoint();
            CDPoint bottomPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointReferenceBottom, 0, ref refPoint, ref curPoint);

            List<Entity> customEntity = new List<Entity>();
            List<Point3D> currentPointList = new List<Point3D>();


            double newNeckLength = GetInternalPipeLengthAll(selNozzle, refPoint, drawPoint);

            DrainInternalModel newModel = GetDrainInternalPipe(selNozzle);
            ElbowModel newElbow = GetElbow(selNozzle);
            if (newModel != null && newElbow != null)
            {
                // 무조건 90
                double elbowA = valueService.GetDoubleValue(newElbow.LRA);
                double OD = valueService.GetDoubleValue(newElbow.OD);

                double B = valueService.GetDoubleValue(newModel.B);
                double C = valueService.GetDoubleValue(newModel.C);
                double D = valueService.GetDoubleValue(newModel.A);
                double E = valueService.GetDoubleValue(newModel.A1);


                double tankNominalHalf = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID) / 2;
                double bottonThickness = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateThickness);
                currentPointList.Add(GetSumPoint(drawPoint, newNeckLength, 0));
                List<Entity> elbowList0 = nozzleBlock.DrawReference_Nozzle_Elbow(out currentPointList, currentPointList[0], 0, newElbow, "lr", "90", Utility.DegToRad(-90), new DrawCenterLineModel() { scaleValue = 90, zeroEx = true });
                customEntity.AddRange(elbowList0);



                //CDPoint bottomPointUp = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, tankNominalHalf - newNeckLength - elbowA , ref refPoint, ref curPoint);
                CDPoint bottomPointUp = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, tankNominalHalf - newNeckLength - elbowA - OD / 2 - 65 / 2, ref refPoint, ref curPoint);
                Point3D bottomPointUpUp = GetSumPoint(bottomPointUp, 0, B);
                Point3D buttomSupport = GetSumPoint(bottomPointUp, 0.5, 0);
                double pipeLength = currentPointList[1].Y - bottomPointUpUp.Y;
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPointList, currentPointList[1], 1, OD, pipeLength, 0, 0, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));


                double pipeSlope = valueService.GetDegreeOfSlope(assemblyData.BottomInput[0].BottomPlateSlope);


                double outWidth = publicFunService.GetBottomFocuntionOD(assemblyData.BottomInput[0].AnnularPlate);
                double bottomThk = valueService.GetDoubleValue(assemblyData.ShellOutput[0].Thickness);
                Point3D flatPointUp= new Point3D();
                if (assemblyData.BottomInput[0].AnnularPlate == "Yes")
                {
                    pipeSlope = 0;
                    //flatPointUp = GetSumPoint(refPoint, -outWidth - bottomThk + valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateWidth), 0);
                    buttomSupport.Y = refPoint.Y;
                }
                else
                {

                }

                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPointList, buttomSupport, 0, 65, C, pipeSlope, 0, 0, null));

            }



            return customEntity;
        }


        public List<Entity> CreateInternalBended(CDPoint refPoint, Point3D drawPoint, NozzleInputModel selNozzle, double couplingHeight, double selScaleValue)
        {
            CDPoint curPoint = new CDPoint();
            CDPoint bottomPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointReferenceBottom, 0, ref refPoint, ref curPoint);

            List<Entity> customEntity = new List<Entity>();
            List<Point3D> currentPointList = new List<Point3D>();


            double newNeckLength = GetInternalPipeLengthAll(selNozzle, refPoint, drawPoint);

            double shellHeightThk = 8; //default 마지막 셀 두께
            if (assemblyData.ShellOutput.Count > 0)
            {
                shellHeightThk = valueService.GetDoubleValue(assemblyData.ShellOutput[assemblyData.ShellOutput.Count - 1].Thickness);
            }

            InternalBendedTypeModel newBended = GetInletOutletPipeBended(selNozzle);
            ElbowModel newElbow = GetElbow(selNozzle);
            if (newBended != null && newElbow != null)
            {
                // 무조건 90
                double elbowA = valueService.GetDoubleValue(newElbow.LRA);
                double OD = valueService.GetDoubleValue(newElbow.OD);

                double BD = valueService.GetDoubleValue(newBended.BD);
                double C = valueService.GetDoubleValue(newBended.C);
                double I = valueService.GetDoubleValue(newBended.I);


                currentPointList.Add(GetSumPoint(drawPoint, 0, -newNeckLength));

                List<Entity> elbowList0 = nozzleBlock.DrawReference_Nozzle_Elbow(out currentPointList, currentPointList[0], 0, newElbow, "lr", "45", Utility.DegToRad(-180), new DrawCenterLineModel() { scaleValue = 90, zeroEx = true });
                customEntity.AddRange(elbowList0);

                double shellDistance = currentPointList[1].X - refPoint.X;
                double bendedWidth = shellDistance - BD;

                // 1 : 1 : root 2
                double bendedLength = Math.Sqrt(2) * bendedWidth;

                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPointList, currentPointList[1], 1, OD, bendedLength, Utility.DegToRad(-45), 0, Utility.DegToRad(-45), new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));
                // Bottom
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPointList, GetSumPoint(currentPointList[1], -shellDistance, -shellDistance), 0, C, shellHeightThk, 0, 0, Utility.DegToRad(-90), null));

            }



            return customEntity;
        }

        public List<Entity> CreateInternalBasic(CDPoint refPoint, Point3D drawPoint, NozzleInputModel selNozzle, double couplingHeight, double selScaleValue)
        {
            CDPoint curPoint = new CDPoint();
            CDPoint bottomPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointReferenceBottom, 0, ref refPoint, ref curPoint);

            List<Entity> customEntity = new List<Entity>();
            List<Point3D> currentPointList = new List<Point3D>();


            double newNeckLength = GetInternalPipeLengthAll(selNozzle, refPoint, drawPoint);


            InOutInternalPipe newBended = GetInletOutletPipe(selNozzle);
            ElbowModel newElbow = GetElbow(selNozzle);
            if (newBended != null && newElbow != null)
            {
                // 무조건 90
                double elbowA = valueService.GetDoubleValue(newElbow.LRA);
                double OD = valueService.GetDoubleValue(newElbow.OD);

                double BD = valueService.GetDoubleValue(newBended.OD);
                double A = valueService.GetDoubleValue(newBended.A);
                double B = valueService.GetDoubleValue(newBended.B);
                double C = valueService.GetDoubleValue(newBended.C);
                double D = valueService.GetDoubleValue(newBended.D);
                double E = valueService.GetDoubleValue(newBended.E);
                double F = valueService.GetDoubleValue(newBended.F);
                double FlatBarH = valueService.GetDoubleValue(newBended.FlatBarH);

                double A3 = valueService.GetDoubleValue(newBended.A3);
                double B3 = valueService.GetDoubleValue(newBended.B3);
                double C3 = valueService.GetDoubleValue(newBended.C3);

                double A1 = valueService.GetDoubleValue(newBended.A1);
                double B1 = valueService.GetDoubleValue(newBended.B1);
                double C1 = valueService.GetDoubleValue(newBended.C1);
                double A2 = valueService.GetDoubleValue(newBended.A2);
                double B2 = valueService.GetDoubleValue(newBended.B2);
                double C2 = valueService.GetDoubleValue(newBended.C2);


                double shellHeightThk = 8; //default 마지막 셀 두께
                if (assemblyData.ShellOutput.Count > 0)
                {
                    shellHeightThk = valueService.GetDoubleValue(assemblyData.ShellOutput[assemblyData.ShellOutput.Count - 1].Thickness);
                }


                currentPointList.Add(GetSumPoint(drawPoint, newNeckLength, 0));



                if (selNozzle.OutletDirection == "upward")
                {
                    // Outlet : 위로
                    List<Entity> elbowList0 = nozzleBlock.DrawReference_Nozzle_Elbow(out currentPointList, currentPointList[0], 1, newElbow, "lr", "90", Utility.DegToRad(180), new DrawCenterLineModel() { scaleValue = 90, zeroEx = true });
                    customEntity.AddRange(elbowList0);


                    // Riquid Level
                    double HLL = valueService.GetDoubleValue(assemblyData.GeneralLiquidCapacityWeight[0].HighLiquidLevel);
                    double pipeLength = (refPoint.X + HLL) - currentPointList[0].Y;


                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPointList, currentPointList[0], 1, OD, pipeLength, 0, 0, Utility.DegToRad(180), new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));

                }
                else if (selNozzle.OutletDirection == "downward")
                {
                    // 아래로

                    List<Entity> elbowList0 = nozzleBlock.DrawReference_Nozzle_Elbow(out currentPointList, currentPointList[0], 0, newElbow, "lr", "90", Utility.DegToRad(-90), new DrawCenterLineModel() { scaleValue = 90, zeroEx = true });
                    customEntity.AddRange(elbowList0);

                    double tankNominalHalf = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID) / 2;
                    double shellDistance = currentPointList[1].X - refPoint.X;
                    CDPoint bottomPointUp = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterBottomUp, tankNominalHalf - shellDistance, ref refPoint, ref curPoint);

                    double pipeLength = currentPointList[1].Y - bottomPointUp.Y - F;

                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPointList, currentPointList[1], 1, OD, pipeLength, 0, 0, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));

                    // UBolt
                    Point3D UpperStartPoint = new Point3D(refPoint.X, drawPoint.Y - B1);
                    Point3D lowerEndPoint = new Point3D(refPoint.X, bottomPointUp.Y + B1);
                    double uboltDistance = bottomPointUp.X - refPoint.X;

                    List<Point3D> ublotOutList = new List<Point3D>();
                    customEntity.AddRange(CreateUBlot(out ublotOutList, GetSumPoint(UpperStartPoint, 0, 0), 0, selNozzle, uboltDistance, shellHeightThk, C1, Utility.DegToRad(-90), new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));
                    customEntity.AddRange(CreateUBlot(out ublotOutList, GetSumPoint(lowerEndPoint, 0, 0), 0, selNozzle, uboltDistance, shellHeightThk, C1, Utility.DegToRad(-90), new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));
                    double maxGap = 2500;
                    double middleHeight = UpperStartPoint.Y - lowerEndPoint.Y;
                    double middleDiv = Math.Ceiling(middleHeight / maxGap);
                    double middleDivLength = valueService.IntRound(middleHeight / middleDiv, -1);
                    // 아래 방향
                    for(int i = 1; i < middleDiv ; i++)
                    {
                        customEntity.AddRange(CreateUBlot(out ublotOutList, GetSumPoint(UpperStartPoint, 0, -middleDivLength*i), 0, selNozzle, uboltDistance, shellHeightThk, C1, Utility.DegToRad(-90), new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));
                    }

                }



                // Bottom
                //customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeSlope(out currentPointList, GetSumPoint(currentPointList[1], -shellDistance, -shellDistance), 0, C, shellHeightThk, 0, 0, Utility.DegToRad(-90), null));

            }



            return customEntity;
        }

        #endregion

        #region Translate TH to OHF : #300 to #150
        // Translate TH to OHF
        private FlangeOHFSeriesAModel TransNozzleATHtoOHF(FlangeTHSeriesAModel selNozzzle)
        {
            FlangeOHFSeriesAModel newModel = new FlangeOHFSeriesAModel();
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
        private FlangeOHFSeriesBModel TransNozzleBTHtoOHF(FlangeTHSeriesBModel selNozzzle)
        {
            FlangeOHFSeriesBModel newModel = new FlangeOHFSeriesBModel();
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
        private FlangeOHFLWNModel TransNozzleLWNTHtoOHF(FlangeTHLWNModel selNozzzle)
        {
            FlangeOHFLWNModel newModel = new FlangeOHFLWNModel();
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
        private FlangeOHFModel TransNozzleTHtoOHF(FlangeTHModel selNozzzle)
        {
            FlangeOHFModel newModel = new FlangeOHFModel();
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

            // 매우 중요 :  Drain Lower Type
            beforeValue.Left = -99999;


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
                                    // 직선 : 무조건 시작 포인트
                                    customEntity.AddRange(CreateBrokenLIne(GetSumPoint(nozzlePoint, -nozzleLength, 0), markPoint));

                                    if (midPoint.Left < markPoint.X)
                                        midPoint.Left = markPoint.X;
                                }
                                else if (nozzlePoint.Y > markPoint.Y)
                                {

                                }
                                else if (nozzlePoint.Y < markPoint.Y)
                                {
                                    // Sump 조절시 : 시작 포인트 아님
                                    if(midPoint.Left== -99999999)
                                    {
                                        customEntity.AddRange(CreateBrokenLIne(GetSumPoint(nozzlePoint, -nozzleLength, 0), markPoint));
                                        if (midPoint.Left < markPoint.X)
                                            midPoint.Left = markPoint.X;
                                    }

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
                            //adjPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjLeftShell, newValue, ref refPoint, ref curPoint);
                            adjPoint = new CDPoint(refPoint.X, refPoint.Y+newValue,0);
                            newPoint = GetSumCDPoint(adjPoint, convergenceValue, 0);
                            break;
                        case "right":
                            //adjPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjRightShell, newValue, ref refPoint, ref curPoint);
                            adjPoint = new CDPoint(refPoint.X+selSizeNominalID, refPoint.Y + newValue, 0);
                            newPoint = GetSumCDPoint(adjPoint, convergenceValue, 0);
                            break;
                        case "center":
                            adjPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterBottom, 0, ref refPoint, ref curPoint);
                            newPoint = GetSumCDPoint(adjPoint, 0, 0);
                            break;
                    }
                    break;
                case "roof":

                    // Roof Slope
                    //double roofSlopeHeight = valueService.GetHypotenuseByWidth(assemblyData.RoofCRTInput[0].RoofSlope, assemblyData.RoofCRTInput[0].RoofPlateThickness);
                    //double roofSlopeHeight = roofThickness * Math.Cos(roofSlopeDegree);

                    switch (selLR)
                    {
                        case "left":
                            adjPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, newValue, ref refPoint, ref curPoint);
                            newPoint = GetSumCDPoint( adjPoint,0, convergenceValue);
                            break;
                        case "right":
                            adjPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.AdjCenterRoofDown, newValue, ref refPoint, ref curPoint);
                            newPoint = GetSumCDPoint(adjPoint, newValue * 2, convergenceValue);
                            break;
                        case "center":
                            adjPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterTopDown, 0, ref refPoint, ref curPoint);
                            newPoint = GetSumCDPoint(adjPoint,0,0);
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
                else if (selNozzle.LR == "center")
                {
                    newPoint = GetSumPoint(drawPoint,H, 0);
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
                else if (selNozzle.LR == "center")
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



        private double GetPipeSchdule(string selPosition, string selNPS, string selRacting)
        {
            double returnValue = 0;
            double corrosion = 0;
            string schValue = "";
            if (selPosition == "shell")
            {
                corrosion = valueService.GetDoubleValue( assemblyData.GeneralCorrosionLoading[0].ShellPlate);
                foreach (PipeModel eachPipe in assemblyData.PipeList)
                {
                    if (eachPipe.ShellCorrosionNPS == selNPS)
                    {
                        if (corrosion >= 0)
                            schValue = eachPipe.ShellCorrosion0;
                        if (corrosion >= 1.6)
                            schValue = eachPipe.ShellCorrosion16;
                        if (corrosion >= 3.2)
                            schValue = eachPipe.ShellCorrosion32;
                        if (corrosion >= 4.8)
                            schValue = eachPipe.ShellCorrosion48;
                        if (corrosion >= 6.4)
                            schValue = eachPipe.ShellCorrosion64;
                        break;
                    }
                        
                }
            }
            else if (selPosition == "roof")
            {
                corrosion = valueService.GetDoubleValue(assemblyData.GeneralCorrosionLoading[0].RoofPlate);
                foreach (PipeModel eachPipe in assemblyData.PipeList)
                {
                    if (eachPipe.ShellCorrosionNPS == selNPS)
                    {
                        if (corrosion >= 0)
                            schValue = eachPipe.RoofCorrosion0;
                        if (corrosion >= 1.6)
                            schValue = eachPipe.RoofCorrosion16;
                        if (corrosion >= 3.2)
                            schValue = eachPipe.RoofCorrosion32;
                        if (corrosion >= 4.8)
                            schValue = eachPipe.RoofCorrosion48;
                        if (corrosion >= 6.4)
                            schValue = eachPipe.RoofCorrosion64;
                        break;
                    }

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
        private Point3D GetSumPoint(CDPoint selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
        private Point3D GetSumCDPoint(CDPoint selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
        private Point3D GetSumCDPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }



        private List<Entity> SetMirrorFunction(Plane selPlane, List<Entity> selList, double X, double Y,bool copyCmd=false)
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









































        // 사용안함
        //public DrawEntityModel DrawNozzle_GGG(ref CDPoint refPoint,
        //                      string selPosition,
        //                      string selNozzleType,
        //                      string selNozzlePosition,
        //                      string selNozzleFontSize,
        //                      string selLeaderCircleSize,
        //                      string selMultiColumn)
        //{

        //    DrawEntityModel nozzleEntities = new DrawEntityModel();
        //    POSITION_TYPE newNozzlePosition = CommonMethod.PositionToEnum(selNozzlePosition);

        //    // Shell Spacing
        //    DrawPositionValueModel shellSpacing = new DrawPositionValueModel();
        //    shellSpacing.Left = valueService.GetDoubleValue(SingletonData.GAArea.Dimension.Length);
        //    shellSpacing.Right = valueService.GetDoubleValue(SingletonData.GAArea.Dimension.Length);
        //    shellSpacing.Top = valueService.GetDoubleValue(SingletonData.GAArea.Dimension.Length);
        //    shellSpacing.Bottom = valueService.GetDoubleValue(SingletonData.GAArea.Dimension.Length);

        //    // Reference Position
        //    int refFirstIndex = 0;

        //    //string selSizeTankHeight = assemblyData.GeneralDesignData[refFirstIndex].SizeTankHeight;
        //    string selSizeNominalId = assemblyData.GeneralDesignData[refFirstIndex].SizeNominalID;

        //    double sizeNominalId = valueService.GetDoubleValue(selSizeNominalId);
        //    CDPoint newCurPoint = new CDPoint();
        //    CDPoint centerTopPoint = workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterTopUp, ref refPoint, ref newCurPoint);
        //    double centerTopHeight = centerTopPoint.Y;


        //    // MutilColumn
        //    bool multiColumnValue = selMultiColumn == "true" ? true : false;



        //    // Nozzle List : Adjust : Sort Value
        //    List<NozzleInputModel> drawNozzle = new List<NozzleInputModel>();
        //    foreach (NozzleInputModel eachNozzle in assemblyData.NozzleInputModel)
        //    {
        //        eachNozzle.Position = eachNozzle.Position.ToLower();
        //        eachNozzle.LR = eachNozzle.LR.ToLower();
        //        eachNozzle.Type = eachNozzle.Type.ToLower();
        //        eachNozzle.Facing = eachNozzle.Facing.ToLower();

        //        eachNozzle.ReinforcingPadType = eachNozzle.ReinforcingPadType.ToLower();
        //        eachNozzle.InletOutlet = eachNozzle.InletOutlet.ToLower();
        //        eachNozzle.Internal = eachNozzle.Internal.ToLower();
        //        eachNozzle.DrainType = eachNozzle.DrainType.ToLower();
        //        eachNozzle.OtherFlange = eachNozzle.OtherFlange.ToLower();

        //        // Sort Value
        //        if (eachNozzle.Position == "shell")
        //            eachNozzle.HRSort = valueService.GetDoubleValue(eachNozzle.H);
        //        else if (eachNozzle.Position == "roof")
        //            eachNozzle.HRSort = valueService.GetDoubleValue(eachNozzle.R);


        //        eachNozzle.LR = eachNozzle.LR.ToLower();
        //        drawNozzle.Add(eachNozzle);
        //    }

        //    // Nozzle List : Sort
        //    List<NozzleInputModel> drawArrangeNozzle = drawNozzle.OrderBy(x => x.HRSort).ThenBy(x => x.LR).ThenBy(x => x.Position).ToList();

        //    // Nozzle Start Point List : Nozzle
        //    List<Point3D> NozzlePointList = new List<Point3D>();

        //    // Nozzle Start Point List : Line
        //    List<Point3D> NozzleLinePointList = new List<Point3D>();

        //    // Nozzle : Create Model
        //    foreach (NozzleInputModel eachNozzle in drawArrangeNozzle)
        //    {
        //        // Start Point
        //        Point3D newNozzlePoint = GetPositionPoint(refPoint, eachNozzle.Position, eachNozzle.LR, eachNozzle.HRSort, 0, sizeNominalId, centerTopHeight, shellSpacing);
        //        NozzlePointList.Add(newNozzlePoint);

        //        Point3D newNozzleLinePoint = GetPositionLinePoint(refPoint, newNozzlePoint, eachNozzle, eachNozzle.Position, eachNozzle.LR, eachNozzle.HRSort, 0, sizeNominalId, centerTopHeight, shellSpacing);
        //        NozzleLinePointList.Add(newNozzleLinePoint);
        //        //List<Entity> customEntity = CreateNozzleModelPosition(refPoint, newNozzlePoint, eachNozzle, sizeNominalId,centerTopHeight, shellSpacing);
        //        List<Entity> customEntity = CreateFlangeAll(refPoint, newNozzlePoint, eachNozzle, sizeNominalId, centerTopHeight);

        //        // Create Model : OutLine
        //        nozzleEntities.outlineList.AddRange(customEntity);
        //    }


        //    // Nozzel Mark Point List : Create Mark Position Arrangement 
        //    List<Point3D> leaderMarkPointList = GetArrangeLeaderMarkPosition(drawArrangeNozzle, selLeaderCircleSize, multiColumnValue, sizeNominalId, centerTopHeight, shellSpacing, refPoint);

        //    // Nozzle : Create Mark
        //    int indexArrange = -1;
        //    foreach (NozzleInputModel eachNozzle in drawArrangeNozzle)
        //    {
        //        indexArrange++;
        //        Point3D drawPoint = leaderMarkPointList[indexArrange];
        //        Dictionary<string, List<Entity>> customEntity = CreateNozzleLeader(drawPoint, eachNozzle, selNozzleFontSize, selLeaderCircleSize);
        //        nozzleEntities.nozzleMarkList.AddRange(customEntity[CommonGlobal.NozzleMark]);
        //        nozzleEntities.nozzleTextList.AddRange(customEntity[CommonGlobal.NozzleText]);

        //    }

        //    // Nozzle : Create Line
        //    List<Entity> customLineEntity = CreateNozzleLeaderLine(refPoint, shellSpacing, drawArrangeNozzle, leaderMarkPointList, NozzleLinePointList);
        //    //List<Entity> customLineEntity = CreateNozzleLeaderLine(refPoint, shellSpacing, drawArrangeNozzle, leaderMarkPointList, NozzlePointList);
        //    nozzleEntities.nozzlelineList.AddRange(customLineEntity);


        //    // CDPoint ccc = workingPointService.ContactPoint("topangleroofpoint",ref refPoint,ref newCurPoint);
        //    //Line lineref01 = new Line(new Point3D(ccc.X,ccc.Y),new Point3D(ccc.X,ccc.Y+1000));
        //    //nozzleEntities.outlineList.Add(lineref01);

        //    //CDPoint cccc = workingPointService.ContactPoint("leftroofpoint", ref refPoint, ref newCurPoint);
        //    //Line lineref02 = new Line(new Point3D(cccc.X, cccc.Y), new Point3D(cccc.X, cccc.Y + 1000));
        //    //nozzleEntities.outlineList.Add(lineref02);

        //    //CDPoint ccccc = workingPointService.ContactPoint("leftroofpoint", "70", ref refPoint, ref newCurPoint);
        //    //CDPoint ccccc = workingPointService.ContactPoint("leftroofpoint", "3793.6", ref refPoint, ref newCurPoint);
        //    //Line lineref03 = new Line(new Point3D(ccccc.X, ccccc.Y), new Point3D(ccccc.X, ccccc.Y + 1000));
        //    //nozzleEntities.outlineList.Add(lineref03);

        //    return nozzleEntities;
        //}

        #region Nozzel : Create Model : Sample
        //private List<Entity> CreateNozzleModelPosition(CDPoint refPoint, Point3D startPoint, NozzleInputModel selNozzle, double selSizeNominalID, double selCenterTopHeight, DrawPositionValueModel selShellSpacing)
        //{



        //    //Point3D drawPoint = GetPositionPoint(refPoint, selNozzle.Position, selNozzle.LR, selNozzle.HRSort,0, selSizeNominalID, selCenterTopHeight, selShellSpacing);

        //    // Sample
        //     List<Entity> newNozzle =  CreateNozzleModel(refPoint, startPoint, selNozzle);

        //    //List<Entity> newNozzle = CreateFlangeAll(refPoint, startPoint, selNozzle);

        //    //List<Entity> newNozzle = new List<Entity>();
        //    return newNozzle;
        //}

        //// Sample Nozzle
        //private List<Entity> CreateNozzleModel(CDPoint refPoint, Point3D drawPoint, NozzleInputModel selNozzle)
        //{

        //    // 노즐 겹치 구현 암됨
        //    // Width : 10 + 24 + 40 고정 -> 나중에 가변으로 변경해야 함
        //    double flangeFaceWidth = 10;
        //    double flangeFaceHeight = 80;
        //    double flangeFaceInnerWidth = 10;
        //    double flangePipeWidth = 24;
        //    double flangePipeHeight = flangeFaceHeight - (flangeFaceInnerWidth * 2);
        //    double flangePipeInnerWidth = 18;
        //    double pipeHeight = flangeFaceHeight - (flangePipeInnerWidth * 2);
        //    double pipeWidth = 40;
        //    double fullWidth = flangeFaceWidth + flangePipeWidth + pipeWidth;
        //    double fullHeight = flangeFaceHeight;


        //    // Point Adj
        //    Point3D adjPoint = (Point3D)drawPoint.Clone();
        //    adjPoint.X = drawPoint.X - fullWidth;
        //    adjPoint.Y = drawPoint.Y - (fullHeight / 2);

        //    // Model
        //    Line lineFFa = new Line(GetSumPoint(adjPoint, 0, 0, 0), GetSumPoint(adjPoint, 0, flangeFaceHeight, 0));
        //    Line lineFFb = new Line(GetSumPoint(adjPoint, flangeFaceWidth, 0, 0), GetSumPoint(adjPoint, flangeFaceWidth, flangeFaceHeight, 0));
        //    Line lineFFc = new Line(GetSumPoint(adjPoint, 0, 0, 0), GetSumPoint(adjPoint, flangeFaceWidth, 0, 0));
        //    Line lineFFd = new Line(GetSumPoint(adjPoint, 0, flangeFaceHeight, 0), GetSumPoint(adjPoint, flangeFaceWidth, flangeFaceHeight, 0));

        //    Line lineFPa = new Line(GetSumPoint(adjPoint, flangeFaceWidth, flangeFaceInnerWidth + flangePipeHeight, 0), GetSumPoint(adjPoint, flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth + pipeHeight, 0));
        //    Line lineFPb = new Line(GetSumPoint(adjPoint, flangeFaceWidth, flangeFaceInnerWidth, 0), GetSumPoint(adjPoint, flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0));
        //    Line lineFPc = new Line(GetSumPoint(adjPoint, flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth + pipeHeight, 0), GetSumPoint(adjPoint, flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0));

        //    Line linePa = new Line(GetSumPoint(adjPoint, flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth, 0), GetSumPoint(adjPoint, flangeFaceWidth + flangePipeWidth + pipeWidth, flangePipeInnerWidth, 0));
        //    Line linePb = new Line(GetSumPoint(adjPoint, flangeFaceWidth + flangePipeWidth, flangePipeInnerWidth + pipeHeight, 0), GetSumPoint(adjPoint, flangeFaceWidth + flangePipeWidth + pipeWidth, flangePipeInnerWidth + pipeHeight, 0));

        //    // Entity
        //    List<Entity> customEntity = new List<Entity>();
        //    customEntity.Add(lineFFa);
        //    customEntity.Add(lineFFb);
        //    customEntity.Add(lineFFc);
        //    customEntity.Add(lineFFd);
        //    customEntity.Add(lineFPa);
        //    customEntity.Add(lineFPb);
        //    customEntity.Add(lineFPc);
        //    customEntity.Add(linePa);
        //    customEntity.Add(linePb);

        //    // Rotation
        //    switch (selNozzle.Position)
        //    {
        //        case "shell":
        //            switch (selNozzle.LR)
        //            {
        //                case "left":
        //                    break;
        //                case "right":
        //                    foreach (Entity eachEntity in customEntity)
        //                        eachEntity.Rotate(UtilityEx.DegToRad(-180), Vector3D.AxisZ, drawPoint);
        //                    break;
        //            }
        //            break;
        //        case "roof":
        //            switch (selNozzle.LR)
        //            {
        //                case "left":
        //                case "right":
        //                    foreach (Entity eachEntity in customEntity)
        //                        eachEntity.Rotate(UtilityEx.DegToRad(-90), Vector3D.AxisZ, drawPoint);
        //                    break;
        //            }
        //            break;
        //    }


        //    return customEntity;
        //}


        #endregion


    }
}
