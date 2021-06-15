using AssemblyLib.AssemblyModels;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawWork.Commons;
using DrawWork.CutomModels;
using DrawWork.DrawModels;
using DrawWork.DrawStyleServices;
using DrawWork.ValueServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawServices
{
    public class DrawNozzleOrientationService
    {
        private AssemblyModel assemblyData;

        private ValueService valueService;
        private DrawWorkingPointService workingPointService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;
        private DrawEditingService editingService;

        private DrawPublicFunctionService publicFunService;

        private DrawNozzleBlockService nozzleBlock;
        private DrawImportBlockService blockImportService;

        public DrawNozzleOrientationService(AssemblyModel selAssembly, Object selModel)
        {
            assemblyData = selAssembly;

            valueService = new ValueService();
            workingPointService = new DrawWorkingPointService(selAssembly);
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();
            editingService = new DrawEditingService();

            publicFunService = new DrawPublicFunctionService();

            nozzleBlock = new DrawNozzleBlockService(selAssembly);
            blockImportService = new DrawImportBlockService(selAssembly, (Model)selModel);
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

                newNozzle.AutoBleederVent = eachNozzle.AutoBleederVent.ToLower();
                newNozzle.RimVent = eachNozzle.RimVent.ToLower();
                newNozzle.RoofDrainSump = eachNozzle.RoofDrainSump.ToLower();
                newNozzle.NozzleOnPlateform = eachNozzle.NozzleOnPlateform.ToLower();

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

                newNozzle.AutoBleederVent = eachNozzle.AutoBleederVent.ToLower();
                newNozzle.RimVent = eachNozzle.RimVent.ToLower();
                newNozzle.RoofDrainSump = eachNozzle.RoofDrainSump.ToLower();
                newNozzle.NozzleOnPlateform = eachNozzle.NozzleOnPlateform.ToLower();

                // Sort Value
                if (newNozzle.Position == "shell")
                    newNozzle.HRSort = valueService.GetDoubleValue(eachNozzle.H);
                else if (newNozzle.Position == "roof")
                    newNozzle.HRSort = valueService.GetDoubleValue(eachNozzle.R);


                newList.Add(newNozzle);
            }

            #region Temp
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
            #endregion

            #region Tank Plan : Adjust
            // EFRT : Roof 노즐 제거
            if (SingletonData.TankType == TANK_TYPE.EFRTSingle ||
               SingletonData.TankType == TANK_TYPE.EFRTDouble)
            {
                for (int i = newList.Count - 1; i >= 0; i--)
                {
                    if (newList[i].Position == "roof")
                    {
                        //if (newList[i].AutoBleederVent == "yes" ||
                        //    newList[i].RimVent == "yes" ||
                        //    newList[i].RoofDrainSump == "yes" ||
                        //    newList[i].NozzleOnPlateform == "yes")
                        if (newList[i].RoofDrainSump == "yes" ||
                            newList[i].NozzleOnPlateform == "yes")
                        {
                            if (newList[i].RoofDrainSump == "yes")
                            {
                                newList[i].Position = "shell";
                                newList[i].LR = "left";
                                newList[i].HRSort = valueService.GetDoubleValue(newList[i].H);
                                double roofDrainSumpR = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID) / 2 + 235;//235는 고정
                                newList[i].R = roofDrainSumpR.ToString();
                            }
                        }
                        else
                        {
                            newList.RemoveAt(i);
                        }

                    }
                }
            }
            // CRT & DRT : FRT 노즐 제거
            else if (SingletonData.TankType == TANK_TYPE.CRT ||
                     SingletonData.TankType == TANK_TYPE.DRT)
            {
                for (int i = newList.Count - 1; i >= 0; i--)
                {
                    if (newList[i].Position == "roof")
                    {
                        if (newList[i].AutoBleederVent == "yes" ||
                            newList[i].RimVent == "yes" ||
                            newList[i].RoofDrainSump == "yes" ||
                            newList[i].NozzleOnPlateform == "yes")
                        {
                            newList.RemoveAt(i);
                        }
                    }
                }
            }
            #endregion

            return newList;
        }
        #endregion

        public DrawEntityModel DrawNozzle_Orientation(ref CDPoint refPoint, double selScaleValue)
        {
            DrawEntityModel nozzleEntities = new DrawEntityModel();

            // Reference Position
            int refFirstIndex = 0;
            double sizeNominalID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[refFirstIndex].SizeNominalID);
            double shellFirstThickness = valueService.GetDoubleValue(assemblyData.ShellOutput[0].Thickness);
            double tankOD = sizeNominalID + shellFirstThickness * 2;
            // Entity
            List<Entity> customOutlineEntity = new List<Entity>();
            List<Entity> customMarkEntity = new List<Entity>();
            List<Entity> customTextEntity = new List<Entity>();

            // Nozzle Array : Option


            List<NozzleInputModel> drawNozzle = GetSumNozzleList();

            // Nozzle List : Sort
            List<NozzleInputModel> drawArrangeNozzle = drawNozzle.OrderBy(x => x.HRSort).ThenBy(x => x.Position).ToList();

            // Nozzle Start Point List : Nozzle
            List<Point3D> NozzlePointList = new List<Point3D>();


            // Shell OD
            Circle circleShell = new Circle(GetSumPoint(refPoint, 0, 0), tankOD / 2);
            // Nozzle : Create Model
            foreach (NozzleInputModel eachNozzle in drawArrangeNozzle)
            {
                Console.WriteLine("Press " + eachNozzle.Description + " " + eachNozzle.Remarks);
                // Start Point : Shell 접점
                Point3D newNozzlePoint = GetPositionPoint(refPoint, eachNozzle.Ort, eachNozzle.R);
                NozzlePointList.Add(newNozzlePoint);

                // Create Model : OutLine
                customOutlineEntity.AddRange(CreateFlangeAll(ref nozzleEntities, ref circleShell, refPoint, newNozzlePoint, eachNozzle, sizeNominalID, selScaleValue));
                Console.WriteLine(eachNozzle.Description + " " + eachNozzle.Remarks);

            }
            nozzleEntities.outlineList.AddRange(customOutlineEntity);


            // Nozzle Mark Position List : Create Mark Position Arragement
            DrawEntityModel leaderMarkLineList = GetArrangeLeaderMarkPosition(ref circleShell, drawArrangeNozzle,  refPoint, tankOD, selScaleValue);

            nozzleEntities.nozzlelineList.AddRange(leaderMarkLineList.nozzlelineList);
            nozzleEntities.nozzleMarkList.AddRange(leaderMarkLineList.nozzleMarkList);
            nozzleEntities.nozzleTextList.AddRange(leaderMarkLineList.nozzleTextList);


            return nozzleEntities;
        }

        private List<Entity> CreateFlangeAll(ref DrawEntityModel selDrawEntities,ref Circle selCircleShell, CDPoint refPoint, Point3D drawPoint,  NozzleInputModel selNozzle, double selSizeNominalID, double scaleValue)
        {
            string checkValue = "yes";
            Entity newBlock = null;
            List<Entity> customEntity = new List<Entity>();

            #region Block
            // Pending issue
            // Draw Point = DrawStartPoint
            NozzleBlock_Type nozzlePlane = CreateFlange_Block(refPoint, drawPoint, selNozzle, ref newBlock);
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


                    
                    // Manhole
                    if (selNozzle.Manhole == checkValue)
                    {
                        customEntity.AddRange(CreateManhole(ref selCircleShell,refPoint, drawPoint, selNozzle, selSizeNominalID,  scaleValue));
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
                                        customEntity = CreateFlangeSeriesA(ref selCircleShell, refPoint, drawPoint,  selNozzle, newNozzle, selSizeNominalID,  scaleValue);
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
                                        customEntity = CreateFlangeSeriesA(ref selCircleShell, refPoint, drawPoint,  selNozzle, TransNozzleATHtoOHF(newNozzle), selSizeNominalID,  scaleValue);
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
                                        customEntity = CreateFlangeSeriesB(ref selCircleShell, refPoint, drawPoint,  selNozzle, newNozzle, selSizeNominalID,  scaleValue);
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
                                        customEntity = CreateFlangeSeriesB(ref selCircleShell, refPoint, drawPoint,  selNozzle, TransNozzleBTHtoOHF(newNozzle), selSizeNominalID,  scaleValue);
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
                                        customEntity = CreateFlangeLWN(ref selCircleShell, refPoint, drawPoint,  selNozzle, newNozzle, selSizeNominalID,  scaleValue);
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
                                        customEntity = CreateFlangeLWN(ref selCircleShell, refPoint, drawPoint,  selNozzle, TransNozzleLWNTHtoOHF(newNozzle), selSizeNominalID,  scaleValue);
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
                                        customEntity = CreateFlange(ref selCircleShell,refPoint, drawPoint,  selNozzle, newNozzle, selSizeNominalID,  scaleValue);
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
                                        customEntity = CreateFlange(ref selCircleShell,refPoint, drawPoint,  selNozzle, TransNozzleTHtoOHF(newNozzle), selSizeNominalID,  scaleValue);
                                        break;
                                    }
                                }
                            }
                        }
                        #endregion




                    }



                    break;


            }




            // Block Add
            if (newBlock != null)
            {
                selDrawEntities.blockList.Add(newBlock);
            }



            return customEntity;
        }


        #region Flange, Manhole
        private List<Entity> CreateManhole(ref Circle selCircleShell,CDPoint refPoint, Point3D drawPoint, NozzleInputModel selNozzle, double selSizeNominalID, double selScaleValue)
        {
            double extSubLength = 2;

            List<Entity> customEntity = new List<Entity>();

            string nozzleSize = selNozzle.Size;
            double nozzleRadius = valueService.GetDoubleValue(selNozzle.R);
            // 시계방향
            double nozzleOrt =-Utility.DegToRad( valueService.GetDoubleValue(selNozzle.Ort));

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
                currentPoint.Add(GetSumPoint(drawPoint, 0, 0));

                // Offset
                Line refCenterLine = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(drawPoint, 0, 0));
                Line refCenterRealLine = (Line)refCenterLine.Clone();
                bool roofTopView = true;
                if (selNozzle.AttachedType == "offset")
                {
                    double offsetValue = valueService.GetDoubleValue(selNozzle.OffsetToCL);
                    refCenterRealLine = (Line)refCenterLine.Offset(offsetValue, Vector3D.AxisZ);// 양수면 아래로
                    drawPoint = GetSumPoint(refCenterRealLine.EndPoint, 0, 0);
                    roofTopView = false;
                }

                //customEntity.Add(refCenterRealLine);

                if (selNozzle.Position == "roof")
                {
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange_Top(out currentPoint, GetSumPoint(drawPoint, 0, 0), 0, manholeOD, BCD, D2, nozzleRadius, nozzleOrt, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, exLength = extSubLength, arcEx = roofTopView, twoEx = true }));
                }
                else if (selNozzle.Position == "shell")
                {
                    // Flange
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out currentPoint, GetSumPoint(drawPoint,0,0), 1, BCD, D2, T6, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true, twoEx = true }));
                    
                    Line refCenterRealLineAdj = (Line)refCenterRealLine.Clone();
                    refCenterRealLineAdj.TrimBy(currentPoint[0], false);
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeTrimByCircle(out currentPoint, ref selCircleShell, ref refCenterRealLineAdj, GetSumPoint(currentPoint[0],0,0), manholeOD, new DrawCenterLineModel() { scaleValue = selScaleValue, exLength = extSubLength, twoEx = true }));
                }

                // Cover : Blind Flange
                if (selNozzle.Position == "shell" && selNozzle.Cover == "yes")
                {
                    List<Point3D> coverPoint = new List<Point3D>();
                    coverPoint.Add(GetSumPoint(drawPoint, 0, 0));
                    coverPoint.Add(GetSumPoint(drawPoint, 0, 0));
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out coverPoint, coverPoint[1], 0, BCD, D2, T6, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true, twoEx = true }));
                }


            }

            return customEntity;
        }
        private List<Entity> CreateFlange(ref Circle selCircleShell, CDPoint refPoint, Point3D drawPoint, NozzleInputModel selNozzle, FlangeOHFModel drawNozzle, double selSizeNominalID,  double selScaleValue)
        {
            double extSubLength = 2;
            double nozzleRadius = valueService.GetDoubleValue(selNozzle.R);
            // 시계방향
            double nozzleOrt = -Utility.DegToRad(valueService.GetDoubleValue(selNozzle.Ort));

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


            List<Entity> customEntity = new List<Entity>();


            List<Point3D> currentPoint = new List<Point3D>();
            currentPoint.Add(GetSumPoint(drawPoint, 0, 0));


            // Offset
            Line refCenterLine = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(drawPoint, 0, 0));
            Line refCenterRealLine = (Line)refCenterLine.Clone();
            bool roofTopView = true;
            if (selNozzle.AttachedType == "offset")
            {
                double offsetValue = valueService.GetDoubleValue(selNozzle.OffsetToCL);
                refCenterRealLine = (Line)refCenterLine.Offset(offsetValue, Vector3D.AxisZ);// 양수면 아래로
                drawPoint = GetSumPoint(refCenterRealLine.EndPoint, 0, 0);
                roofTopView = false;
            }

            if (selNozzle.Position == "roof")
            {
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange_Top(out currentPoint, GetSumPoint(drawPoint, 0, 0), 0, G, BCD,OD, nozzleRadius, nozzleOrt, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, exLength = extSubLength, arcEx = roofTopView, twoEx = true }));
            }
            else
            {
                // Facing
                if (facingAdd)
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Pipe(out currentPoint, currentPoint[0], 1, R, facingC, true, true, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));
                // Flange
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out currentPoint, currentPoint[0], 1, BCD, OD, B - facingC, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = !facingAdd, twoEx = true }));
                // Neck
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Neck(out currentPoint, currentPoint[0], 1, G, H, A - B, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));

                Line refCenterRealLineAdj = (Line)refCenterRealLine.Clone();
                refCenterRealLineAdj.TrimBy(currentPoint[0], false);
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeTrimByCircle(out currentPoint, ref selCircleShell, ref refCenterRealLineAdj, GetSumPoint(currentPoint[0], 0, 0), G, new DrawCenterLineModel() { scaleValue = selScaleValue, exLength = extSubLength, twoEx = true }));
            }



            // Cover : Blind Flange
            if (selNozzle.Position == "shell" && selNozzle.Cover == "yes")
            {
                List<Point3D> coverPoint = new List<Point3D>();
                coverPoint.Add(GetSumPoint(drawPoint, 0, 0));
                coverPoint.Add(GetSumPoint(drawPoint, 0, 0));
                if (facingAdd)
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Pipe(out coverPoint, coverPoint[0], 0, R, facingC, true, true, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out coverPoint, coverPoint[1], 0, BCD, OD, B - facingC, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true, twoEx = true }));
            }




            return customEntity;
        }
        private List<Entity> CreateFlangeLWN(ref Circle selCircleShell, CDPoint refPoint, Point3D drawPoint, NozzleInputModel selNozzle, FlangeOHFLWNModel drawNozzle, double selSizeNominalID, double selScaleValue)
        {
            double extSubLength = 2;
            double nozzleRadius = valueService.GetDoubleValue(selNozzle.R);
            // 시계방향
            double nozzleOrt = -Utility.DegToRad(valueService.GetDoubleValue(selNozzle.Ort));

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


            List<Entity> customEntity = new List<Entity>();

            List<Point3D> currentPoint = new List<Point3D>();
            currentPoint.Add(GetSumPoint(drawPoint, 0, 0));


            // Offset
            Line refCenterLine = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(drawPoint, 0, 0));
            Line refCenterRealLine = (Line)refCenterLine.Clone();
            bool roofTopView = true;
            if (selNozzle.AttachedType == "offset")
            {
                double offsetValue = valueService.GetDoubleValue(selNozzle.OffsetToCL);
                refCenterRealLine = (Line)refCenterLine.Offset(offsetValue, Vector3D.AxisZ);// 양수면 아래로
                drawPoint = GetSumPoint(refCenterRealLine.EndPoint, 0, 0);
                roofTopView = false;
            }


            if (selNozzle.Position == "roof")
            {
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange_Top(out currentPoint, GetSumPoint(drawPoint, 0, 0), 0, G, BCD, OD, nozzleRadius, nozzleOrt, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, exLength = extSubLength, arcEx = roofTopView, twoEx = true }));
            }
            else
            {

                // Facing
                if (facingAdd)
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Pipe(out currentPoint, currentPoint[0], 1, R, facingC, true, true, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));

                // Flange
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out currentPoint, currentPoint[0], 1, BCD, OD, B - facingC, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = !facingAdd, twoEx = true }));

                Line refCenterRealLineAdj = (Line)refCenterRealLine.Clone();
                refCenterRealLineAdj.TrimBy(currentPoint[0], false);
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeTrimByCircle(out currentPoint, ref selCircleShell, ref refCenterRealLineAdj, GetSumPoint(currentPoint[0], 0, 0), G, new DrawCenterLineModel() { scaleValue = selScaleValue, exLength = extSubLength, twoEx = true }));

            }



            // Cover : Blind Flange
            if (selNozzle.Position == "shell" && selNozzle.Cover == "yes")
            {
                List<Point3D> coverPoint = new List<Point3D>();
                coverPoint.Add(GetSumPoint(drawPoint, 0, 0));
                coverPoint.Add(GetSumPoint(drawPoint, 0, 0));
                if (facingAdd)
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Pipe(out coverPoint, coverPoint[0], 0, R, facingC, true, true, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out coverPoint, coverPoint[1], 0, BCD, OD, B - facingC, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true, twoEx = true }));
            }


            return customEntity;
        }
        private List<Entity> CreateFlangeSeriesA(ref Circle selCircleShell, CDPoint refPoint, Point3D drawPoint,NozzleInputModel selNozzle, FlangeOHFSeriesAModel drawNozzle, double selSizeNominalID,  double selScaleValue)
        {

            double extSubLength = 2;
            double nozzleRadius = valueService.GetDoubleValue(selNozzle.R);
            // 시계방향
            double nozzleOrt = -Utility.DegToRad(valueService.GetDoubleValue(selNozzle.Ort));

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


            List<Entity> customEntity = new List<Entity>();


            List<Point3D> currentPoint = new List<Point3D>();
            currentPoint.Add(GetSumPoint(drawPoint, 0, 0));

            // Offset
            Line refCenterLine = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(drawPoint, 0, 0));
            Line refCenterRealLine = (Line)refCenterLine.Clone();
            bool roofTopView = true;
            if (selNozzle.AttachedType == "offset")
            {
                double offsetValue = valueService.GetDoubleValue(selNozzle.OffsetToCL);
                refCenterRealLine = (Line)refCenterLine.Offset(offsetValue, Vector3D.AxisZ);// 양수면 아래로
                drawPoint = GetSumPoint(refCenterRealLine.EndPoint, 0, 0);
                roofTopView = false;
            }


            if (selNozzle.Position == "roof")
            {
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange_Top(out currentPoint, GetSumPoint(drawPoint, 0, 0), 0, G, BCD, OD, nozzleRadius, nozzleOrt, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, exLength = extSubLength, arcEx = roofTopView, twoEx = true }));
            }
            else
            {
                // Facing
                if (facingAdd)
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Pipe(out currentPoint, currentPoint[0], 1, R, facingC, true, true, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));

                // Flange
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out currentPoint, currentPoint[0], 1, BCD, OD, BWN - facingC, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = !facingAdd, twoEx = true }));
                // Neck
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Neck(out currentPoint, currentPoint[0], 1, G, H, A - BWN, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));


                Line refCenterRealLineAdj = (Line)refCenterRealLine.Clone();
                refCenterRealLineAdj.TrimBy(currentPoint[0], false);
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeTrimByCircle(out currentPoint, ref selCircleShell, ref refCenterRealLineAdj, GetSumPoint(currentPoint[0], 0, 0), G, new DrawCenterLineModel() { scaleValue = selScaleValue, exLength = extSubLength, twoEx = true }));
            }


            // Cover : Blind Flange
            if (selNozzle.Position == "shell" && selNozzle.Cover == "yes")
            {
                List<Point3D> coverPoint = new List<Point3D>();
                coverPoint.Add(GetSumPoint(drawPoint, 0, 0));
                coverPoint.Add(GetSumPoint(drawPoint, 0, 0));
                if (facingAdd)
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Pipe(out coverPoint, coverPoint[0], 0, R, facingC, true, true, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out coverPoint, coverPoint[1], 0, BCD, OD, BBF - facingC, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true, twoEx = true }));
            }

            return customEntity;
        }
        private List<Entity> CreateFlangeSeriesB(ref Circle selCircleShell, CDPoint refPoint, Point3D drawPoint, NozzleInputModel selNozzle, FlangeOHFSeriesBModel drawNozzle, double selSizeNominalID, double selScaleValue)
        {

            double extSubLength = 2;
            double nozzleRadius = valueService.GetDoubleValue(selNozzle.R);
            // 시계방향
            double nozzleOrt = -Utility.DegToRad(valueService.GetDoubleValue(selNozzle.Ort));

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


            List<Entity> customEntity = new List<Entity>();


            List<Point3D> currentPoint = new List<Point3D>();
            currentPoint.Add(GetSumPoint(drawPoint, 0, 0));

            // Offset
            Line refCenterLine = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(drawPoint, 0, 0));
            Line refCenterRealLine = (Line)refCenterLine.Clone();
            bool roofTopView = true;
            if (selNozzle.AttachedType == "offset")
            {
                double offsetValue = valueService.GetDoubleValue(selNozzle.OffsetToCL);
                refCenterRealLine = (Line)refCenterLine.Offset(offsetValue, Vector3D.AxisZ);// 양수면 아래로
                drawPoint = GetSumPoint(refCenterRealLine.EndPoint, 0, 0);
                roofTopView = false;
            }

            if (selNozzle.Position == "roof")
            {
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange_Top(out currentPoint, GetSumPoint(drawPoint, 0, 0), 0, G, BCD, OD, nozzleRadius, nozzleOrt, 0, new DrawCenterLineModel() { scaleValue = selScaleValue, exLength = extSubLength, arcEx = roofTopView, twoEx = true }));
            }
            else
            {
                // Facing
                if (facingAdd)
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Pipe(out currentPoint, currentPoint[0], 1, R, facingC, true, true, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true }));

                // Flange
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out currentPoint, currentPoint[0], 1, BCD, OD, BWN - facingC, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = !facingAdd, twoEx = true }));
                // Neck
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Neck(out currentPoint, currentPoint[0], 1, G, H, A - BWN, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));


                Line refCenterRealLineAdj = (Line)refCenterRealLine.Clone();
                refCenterRealLineAdj.TrimBy(currentPoint[0], false);
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_PipeTrimByCircle(out currentPoint, ref selCircleShell, ref refCenterRealLineAdj, GetSumPoint(currentPoint[0], 0, 0), G, new DrawCenterLineModel() { scaleValue = selScaleValue, exLength = extSubLength, twoEx = true }));
            }


            // Cover : Blind Flange
            if (selNozzle.Position == "shell" && selNozzle.Cover == "yes")
            {
                List<Point3D> coverPoint = new List<Point3D>();
                coverPoint.Add(GetSumPoint(drawPoint, 0, 0));
                coverPoint.Add(GetSumPoint(drawPoint, 0, 0));
                if (facingAdd)
                    customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Pipe(out coverPoint, coverPoint[0], 0, R, facingC, true, true, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, zeroEx = true }));
                customEntity.AddRange(nozzleBlock.DrawReference_Nozzle_Flange(out coverPoint, coverPoint[1], 0, BCD, OD, BBF - facingC, nozzleOrt, new DrawCenterLineModel() { scaleValue = selScaleValue, oneEx = true, twoEx = true }));
            }

            return customEntity;
        }

        #endregion




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
                    string seriesValue = selNozzle.ASMESeries.Replace(" ", "").ToLower().Replace("series", "");
                    if (seriesValue.Contains("b"))
                    {
                        blockName = string.Format("BLOCK-MIXER_B_{0}\"_{1}", selNozzle.Size, selNozzle.Rating);
                    }
                    else
                    {
                        blockName = string.Format("BLOCK-MIXER-{0}\"_{1}", selNozzle.Size, selNozzle.Rating);
                    }

                }
                else if (selNozzle.GaugeHatch == checkValue)
                {
                    blockName = string.Format("BLOCK-GAUGE_HATCH-{0}\"_{1}", selNozzle.Size, selNozzle.Rating);

                    // Leader
                    SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                    {
                        leaderPoint = GetSumOutputCDPoint(drawPoint, 0, 0),
                        lineTextList = new List<string>() { "GAUGE HATCH" },
                        Position = "topleft"
                    });

                }
                else if (selNozzle.GooseNeckBirdScreen == checkValue)
                {
                    blockName = string.Format("BLOCK--{0}\"_{1}", selNozzle.Size, selNozzle.Rating);
                }
                else if (selNozzle.FlameArrestor == checkValue && selNozzle.BreatherValve == checkValue)
                {
                    blockName = string.Format("BLOCK-FLAME_ARRESTOR&BREATHER_VALVE-{0}\"_{1}", selNozzle.Size, selNozzle.Rating);
                    // Leader
                    SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                    {
                        leaderPoint = GetSumOutputCDPoint(drawPoint, 0, 0),
                        lineTextList = new List<string>() { "& BREATHER VALVE", "FLAME ARRESTER" },
                        Position = "topleft"
                    });
                }
                else if (selNozzle.FlameArrestor == checkValue)
                {
                    blockName = string.Format("BLOCK-FLAME_ARRESTOR-{0}\"_{1}", selNozzle.Size, selNozzle.Rating);
                    // Leader
                    SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                    {
                        leaderPoint = GetSumOutputCDPoint(drawPoint, 0, 0),
                        lineTextList = new List<string>() { "FLAME ARRESTER" },
                        Position = "topleft"
                    });
                }
                else if (selNozzle.BreatherValve == checkValue)
                {
                    blockName = string.Format("BLOCK-BREATHER_VALVE-{0}\"_{1}", selNozzle.Size, selNozzle.Rating);
                    // Leader
                    SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                    {
                        leaderPoint = GetSumOutputCDPoint(drawPoint, 0, 0),
                        lineTextList = new List<string>() { "BREATHER VALVE" },
                        Position = "topleft"
                    });
                }
                else if (selNozzle.VacuumReliefValve == checkValue)
                {
                    blockName = string.Format("BLOCK-VACUUM_RELIEF_VALVE-{0}\"_{1}", selNozzle.Size, selNozzle.Rating);

                    // Leader
                    SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                    {
                        leaderPoint = GetSumOutputCDPoint(drawPoint, 0, 0),
                        lineTextList = new List<string>() { "RELIEF VALVE", "PRESSURE VACUUM/" },
                        Position = "topleft"
                    });
                }
                else if (selNozzle.EmergencyVent == checkValue)
                {
                    blockName = string.Format("BLOCK-EMERGENCY_VENT-{0}\"_#150", selNozzle.Size);
                    // Leader
                    SingletonData.LeaderPublicList.Add(new LeaderPointModel()
                    {
                        leaderPoint = GetSumOutputCDPoint(drawPoint, 0, 0),
                        lineTextList = new List<string>() { "EMERGENCY VENT", "PRESSURE VACUUM/" },
                        Position = "topleft"
                    });

                }
                else
                {




                }
            }


            // Block Point 매우 중요함
            blockEntity = null;
            if (drawPoint != null)
            {
                blockEntity = blockImportService.Draw_ImportBlock(new CDPoint(drawPoint.X, drawPoint.Y, 0), blockName, layerService.LayerBlock, scaleFactor);
                if (blockEntity != null)
                {
                    if (selNozzle.Mixer == checkValue)
                    {
                        blockEntity.Rotate(Utility.DegToRad(90), Vector3D.AxisZ, new Point3D(drawPoint.X, drawPoint.Y, 0));
                    }
                }
            }

            return returnValue;
        }



        #region Translate TH to OHF : #300 to #150
        // Translate TH to OHF
        private FlangeOHFSeriesAModel TransNozzleATHtoOHF(FlangeTHSeriesAModel selNozzzle)
        {
            FlangeOHFSeriesAModel newModel = new FlangeOHFSeriesAModel();
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
        private DrawEntityModel GetArrangeLeaderMarkPosition(ref Circle selCircleShell, List<NozzleInputModel> drawArrangeNozzle, CDPoint refPoint, double selTankOD, double selScaleValue)
        {

            DrawDimensionService newDim = new DrawDimensionService();

            List<Entity> nozzleLineList = new List<Entity>();
            List<Entity> nozzleMarkList = new List<Entity>();
            List<Entity> nozzleTextList = new List<Entity>();

            
            double markDiameter = newDim.GetNozzleCircleDiameter(selScaleValue);
            double markRidus = markDiameter / 2;
            double markMarginValue = 0.5 * selScaleValue;


            // Circle Array algorithm
            double markStartDistance = 80 * selScaleValue;
            double markDistancedGap = 30 * selScaleValue;
            double labelDistance = 40 * selScaleValue;

            double bendLineGap = 1 * selScaleValue;

            int markArrayLevel = 3;
            int markMaxNum = 3;
            double extendLength = selTankOD +markStartDistance * (markMaxNum+1);

            // Mark Array : Circle
            List <Circle> markCircleLineList = new List<Circle>();
            for(int i = 0; i < markArrayLevel; i++)
            {
                double circleRadius = selTankOD / 2 + markStartDistance + (markDistancedGap * i);
                markCircleLineList.Add( new Circle(GetSumPoint(refPoint, 0, 0), circleRadius));
            }

            // Label : Circle
            Circle labelCircle = new Circle(GetSumPoint(refPoint, 0, 0), selTankOD / 2 + labelDistance);


            int currentLevel = 0;
            int currentLevelNum = 1;
            int currentLevelNumSum = 1;
            Point3D beforeMarkPoint = null;
            Point3D currentMarkPoint = new Point3D();
            List<Circle> overlapCircleList = new List<Circle>();
            double markMarginDiameter = 0;
            double markMarginRadius = 0;
            double bendCircleRadius = markCircleLineList[currentLevel].Radius - markDiameter; ;
            foreach (NozzleInputModel eachNozzle in drawArrangeNozzle)
            {
                string eachNozzleR = eachNozzle.R;
                string eachNozzleTheta = eachNozzle.Ort;
                string eachNozzleOffset = eachNozzle.OffsetToCL;

                // Nozzle Point
                Point3D nozzlePoint = GetPositionPoint(refPoint, eachNozzleTheta, eachNozzleR);
                Line refCenterLine = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(nozzlePoint, 0, 0));
                if (eachNozzle.AttachedType == "offset")
                {
                    double offsetValue = valueService.GetDoubleValue(eachNozzleOffset);
                    Line offsetLine = (Line)refCenterLine.Offset(offsetValue, Vector3D.AxisZ);// 양수면 아래로
                    refCenterLine = offsetLine;
                }
                Point3D newNozzlePoint = GetSumPoint(refCenterLine.EndPoint, 0, 0);

                // Extend Line
                Point3D nozzleExtendPoint = GetPositionPoint(refPoint, eachNozzle.Ort, extendLength.ToString());
                Line refCenterLineExtend = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(nozzleExtendPoint, 0, 0));

                // Overlap : Check
                bool overlapValue = false;
                Point3D newMarkInter = editingService.GetIntersectWidth(markCircleLineList[currentLevel], refCenterLineExtend, 0);
                if (beforeMarkPoint != null)
                {
                    //markMarginDiameter = markDiameter + (markDiameter * (1+currentLevelNum)*2) + (markMarginValue * 2);
                    markMarginDiameter = markDiameter + (markDiameter * 1.4) + (0);
                    markMarginRadius = markMarginDiameter/2;
                    Circle circleOverLap = new Circle(GetSumPoint(beforeMarkPoint, 0, 0), markMarginRadius);

                    //nozzleLineList.Add(circleOverLap); // 검증용
                    overlapCircleList.Add(circleOverLap);
                    foreach(Circle eachCircle in overlapCircleList)
                    {
                        Point3D[] overlapInter = eachCircle.IntersectWith(refCenterLineExtend);
                        if (overlapInter.Length > 0)
                        {
                            overlapValue = true;
                            newMarkInter = editingService.GetIntersectWidth(markCircleLineList[currentLevel], circleOverLap, 1);
                            // Level이 커졌을 경우 : 다시 직선에서 겹치는 부분
                            if (newMarkInter.Y == 0)
                                newMarkInter = editingService.GetIntersectWidth(markCircleLineList[currentLevel], refCenterLineExtend, 0);

                            break;
                        }
                    }
                        
                }

                beforeMarkPoint = GetSumPoint(newMarkInter, 0, 0);

                // Create Mark
                // Create Line
                Circle bendCircle = new Circle(GetSumPoint(refPoint, 0, 0), bendCircleRadius);
                Point3D firstLinePoint = editingService.GetIntersectWidth(refCenterLineExtend, bendCircle, 0);
                if (firstLinePoint.Y == 0)
                {
                    firstLinePoint = GetSumPoint(refPoint, 0, bendCircleRadius + 200);
                }

                Line leaderLineThree = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(newMarkInter, 0, 0));
                Dictionary<string, List<Entity>> newMark = newDim.DrawDimension_NozzleMark(ref leaderLineThree, true, GetSumPoint(newMarkInter, 0, 0), eachNozzle, selScaleValue);
                Point3D twoLinePoint = editingService.GetIntersectWidth(leaderLineThree, bendCircle, 0);
                leaderLineThree.TrimBy(twoLinePoint, true);

                Line leaderLineOne = new Line(GetSumPoint(newNozzlePoint, 0, 0), GetSumPoint(firstLinePoint, 0, 0));
                Line leaderLineTwo = new Line(GetSumPoint(firstLinePoint, 0, 0), GetSumPoint(twoLinePoint, 0, 0));
                nozzleLineList.Add(leaderLineOne);
                nozzleLineList.Add(leaderLineTwo);
                nozzleLineList.Add(leaderLineThree);

                nozzleMarkList.AddRange(newMark[CommonGlobal.NozzleMark]);
                nozzleTextList.AddRange(newMark[CommonGlobal.NozzleText]);

                // Label : Text
                Text labelText = newDim.DrawDimension_NozzleTheta(ref leaderLineOne, ref labelCircle, eachNozzleTheta, selScaleValue);
                if(labelText!=null)
                    nozzleTextList.Add(labelText);

                if (overlapValue)
                {
                    currentLevelNum++;
                    currentLevelNumSum++;
                    bendCircleRadius -= bendLineGap;
                    if (currentLevelNumSum % markMaxNum ==0)
                    //if (currentLevelNum == markMaxNum-1 )
                    {
                        currentLevelNum = 0;
                        currentLevel++;
                        //같은 레벨에 머물게 하기
                        if (currentLevel == markMaxNum)
                            currentLevel = markMaxNum - 1;
                        bendCircleRadius = markCircleLineList[currentLevel].Radius- markDiameter;
                    }
                }
                else
                {
                    
                    overlapCircleList.Clear();
                    currentLevelNum = 1;
                    currentLevelNumSum = 1;
                    currentLevel = 0;
                    bendCircleRadius = markCircleLineList[currentLevel].Radius - markDiameter;
                }




            }


            styleService.SetLayerListEntity(ref nozzleLineList, layerService.LayerDimension);
            styleService.SetLayerListEntity(ref nozzleMarkList, layerService.LayerDimension);
            styleService.SetLayerListEntity(ref nozzleTextList, layerService.LayerDimension);

            DrawEntityModel returnValue = new DrawEntityModel();
            returnValue.nozzlelineList.AddRange(nozzleLineList);
            returnValue.nozzleMarkList.AddRange(nozzleMarkList);
            returnValue.nozzleTextList.AddRange(nozzleTextList);
            return returnValue;
        }
        private bool GetConvergenceValue(double newValue, double beforeValue, double convergenceHeight)
        {
            bool returnValue = false;
            if (convergenceHeight > 0)
                if (newValue - beforeValue <= convergenceHeight)
                    returnValue = true;

            return returnValue;
        }

        #endregion

        #region Common : Position
        private Point3D GetPositionPoint(CDPoint refPoint,string selOrt, string selR)
        {

            double ortRadian = -Utility.DegToRad(valueService.GetDoubleValue(selOrt));
            Line newLine = new Line(GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, 0,valueService.GetDoubleValue( selR)));
            newLine.Rotate(ortRadian, Vector3D.AxisZ,GetSumPoint(refPoint,0,0));
            Point3D newPoint = GetSumPoint(newLine.EndPoint, 0, 0);

            return newPoint;
        }
        private Point3D GetPositionLinePoint(CDPoint refPoint, Point3D drawPoint, NozzleInputModel selNozzle, string selPosition, string selLR, double newValue, double convergenceValue, double selSizeNominalID, double selCenterTopHeight, DrawPositionValueModel selShellSpacing)
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
                    newPoint = GetSumPoint(drawPoint, H, 0);
                }
            }
            else if (selNozzle.Position == "roof")
            {
                if (selNozzle.LR == "left")
                {
                    neckLength = H - roofPointHeight;
                    newPoint = GetSumPoint(drawPoint, 0, neckLength, 0);
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
        private Point3D GetPositionSpacingPoint(CDPoint refPoint, string selPosition, string selLR, double newValue, double convergenceValue, double selSizeNominalID, double selCenterTopHeight, DrawPositionValueModel selShellSpacing)
        {
            Point3D newPoint = newPoint = GetPositionPoint(refPoint, selPosition, selLR );
            switch (selPosition)
            {
                case "shell":
                    switch (selLR)
                    {
                        case "left":
                            //newPoint.X -= selShellSpacing.Left;
                            newPoint.X = refPoint.X - selShellSpacing.Left + convergenceValue;
                            break;
                        case "right":
                            //newPoint.X += selShellSpacing.Right;
                            newPoint.X = refPoint.X + selSizeNominalID + selShellSpacing.Left + convergenceValue;
                            break;
                        case "center":
                            //newPoint.X += selShellSpacing.Right;
                            newPoint.X = refPoint.X - selShellSpacing.Left + convergenceValue;
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
                        case "center":
                            newPoint.Y = selCenterTopHeight + selShellSpacing.Top + convergenceValue;
                            break;
                    }
                    break;
            }
            return newPoint;
        }
        #endregion}

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
        private Point3D GetSumPoint(CDPoint selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
        private CDPoint GetSumOutputCDPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new CDPoint(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);

        }
    }
}