using AssemblyLib.AssemblyModels;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawSettingLib.Commons;
using DrawSettingLib.SettingModels;
using DrawSettingLib.SettingServices;
using DrawShapeLib.DrawServices;
using DrawWork.AssemblyServices;
using DrawWork.Commons;
using DrawWork.DrawCommonServices;
using DrawWork.DrawModels;
using DrawWork.DrawServices;
using DrawWork.DrawStyleServices;
using DrawWork.ValueServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawDetailServices
{
    public class DrawDetailStructureService
    {

        private Model singleModel;
        private AssemblyModel assemblyData;
        private AssemblyCommonDataService assemblyComData;

        private DrawPublicService drawCommon;

        private ValueService valueService;
        private StyleFunctionService styleService;
        private LayerStyleService layerService;

        private DrawEditingService editingService;
        private DrawShapeServices shapeService;


        private DrawCommonService drawComService;
        private DrawReferenceBlockService refBlockService;
        private DrawWorkingPointService workingPointService;

        private DrawDetailStructureShareService drawShareService;


        public PaperAreaService areaService;


        // Structure
        private StructureService structureService;
        private StructureModel structureCRTModel;


        public DrawDetailStructureService(AssemblyModel selAssembly, object selModel)
        {
            singleModel = selModel as Model;
            assemblyData = selAssembly;
            assemblyComData = new AssemblyCommonDataService(selAssembly);

            drawCommon = new DrawPublicService(selAssembly);
            refBlockService = new DrawReferenceBlockService(selAssembly);
            workingPointService = new DrawWorkingPointService(selAssembly);

            valueService = new ValueService();
            styleService = new StyleFunctionService();
            layerService = new LayerStyleService();


            editingService = new DrawEditingService();
            shapeService = new DrawShapeServices();

            drawComService = new DrawCommonService();
            drawShareService = new DrawDetailStructureShareService(selAssembly,selModel) ;

            areaService = new PaperAreaService();

            structureService = new StructureService();
            structureCRTModel = new StructureModel();
        }

        public DrawEntityModel DrawDetailRoofStructureMain(ref CDPoint refPoint, ref CDPoint curPoint, object selModel, double selScaleValue)
        {
            DrawEntityModel drawList = new DrawEntityModel();

            // Structure Model : CRT
            CreateStructureCRTModel();

            // Structure
            foreach (PaperAreaModel eachModel in SingletonData.PaperArea.AreaList)
            {
                //PaperAreaModel eachModel = areaService.GetPaperAreaModel(eachModel.Name, eachModel.SubName, SingletonData.PaperArea.AreaList);
                // RefPoint
                Point3D referencePoint = new Point3D(eachModel.ReferencePoint.X, eachModel.ReferencePoint.Y);
                //SingletonData.RefPoint = referencePoint;
                // Scale
                double scaleValue = GetStructureCustomScale(eachModel);

                // Structure
                if (eachModel.SubName== PAPERSUB_TYPE.RoofStructureOrientation)               
                {
                    //drawList.AddDrawEntity(DrawDetailRoofStructureOrientation(referencePoint, selModel, scaleValue, eachModel));
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.RoofStructureAssembly)
                {
                    //drawList.AddDrawEntity(DrawDetailRoofStructureAssembly(referencePoint, selModel, scaleValue,eachModel));
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.RafterDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.RafterSideClipDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.CenterRingDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.RafterCenterClipDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.PurlinDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.SectionAA)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.RibPlateDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }


                // Structure Ext
                else if (eachModel.SubName == PAPERSUB_TYPE.CenterRingRafterDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.DetailB)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.SectionCC)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.ViewCC)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.RafterAndReinfPadCrossDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }


                // Structure Column
                else if (eachModel.SubName == PAPERSUB_TYPE.CenterColumnDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.DetailF)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.DrainDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.SectionBB)
                {
                    //drawList.AddDrawEntity(DrawDetailCenterColumn_BB(referencePoint, selModel, scaleValue, eachModel));
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.DetailC)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }


                else if (eachModel.SubName == PAPERSUB_TYPE.SectionEE)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.DetailD)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.GirderBracketDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.Rafter)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.Rafter)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }


                else if (eachModel.SubName == PAPERSUB_TYPE.MidRafter)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.OutterRafter)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.G1Girder)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.DetailA)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.BracketDetail)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }


                else if (eachModel.SubName == PAPERSUB_TYPE.Table1)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.Table2)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.Table3)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.Table4)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.Girder)
                {
                    drawList.AddDrawEntity(DrawDetailAssembly(referencePoint, selModel, scaleValue, eachModel));
                }

            }

            return drawList;
        }



        public void CreateStructureCRTModel()
        {


            // Input : Rafter , Column, Girder
            List<StructureCRTRafterInputModel> rafterInputList = assemblyData.StructureCRTRafterInput.ToList();
            List<StructureCRTColumnInputModel> columnInputList = assemblyData.StructureCRTColumnInput.ToList();
            List<StructureCRTGirderInputModel> girderInputList = assemblyData.StructureCRTGirderInput.ToList();


            // Output : Rafter -> Column Rafter
            List<NColumnRafterModel> rafterOutputList = new List<NColumnRafterModel>();
            foreach (StructureCRTRafterInputModel eachRafter in rafterInputList)
            {
                NColumnRafterModel eachNewRafter = GetRafterModel(eachRafter.Size);
                rafterOutputList.Add(eachNewRafter);
            }

            // Output : Rafter -> H Beam
            List<HBeamModel> girderHbeamList = new List<HBeamModel>();
            foreach (StructureCRTGirderInputModel eachGirder in girderInputList)
            {
                HBeamModel eachHBeam = GetHBeamModel(eachGirder.Size);
                girderHbeamList.Add(eachHBeam);
            }

            // Output : Column -> First Column = Cetner Top Support
            List<NColumnCenterTopSupportModel> columnCenterTopSupportList = new List<NColumnCenterTopSupportModel>();
            foreach (StructureCRTRafterInputModel eachRafter in rafterInputList)
            {
                columnCenterTopSupportList.Add(GetNewColumnCenterTopSupportModel(eachRafter.Size));
            }

            // Output : Column -> First Column = Center Top Support : Pipe
            List<PipeModel> columnPipeList = new List<PipeModel>();
            foreach (StructureCRTColumnInputModel eachColumn in columnInputList)
            {
                PipeModel newModel = GetPipeModel(eachColumn.Size);
                columnPipeList.Add(newModel);
            }

            // Pipe Schedule 적용 안됨

            List<NColumnSideTopSupportModel> columnSideTopSupportList = new List<NColumnSideTopSupportModel>();
            foreach(StructureCRTColumnInputModel eachColumn in columnInputList)
            {
                NColumnSideTopSupportModel newModel=GetNewColumnSideTopSupportModel(eachColumn.Size);
                columnSideTopSupportList.Add(newModel);
            }



            // Tank : Basic Information
            double selTankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double selTankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);

            // Tank : Roof
            double selRoofOD = drawCommon.GetRoofOD();
            double selRoofSlope = valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope);

            // Tank : Bottom
            double selBottomThk = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateThickness);
            double selBottoSlope = valueService.GetDegreeOfSlope(assemblyData.BottomInput[0].BottomPlateSlope);
            double selAnnularInnerWidth = 0;
            if (drawCommon.isAnnular())
                selAnnularInnerWidth = (selTankID - valueService.GetDoubleValue(assemblyData.BottomInput[0].OD)) / 2;
            
            // Tnak : Shell Reduce : k Type Shell  안쪽으로 들어 옴
            double selShellReduce = GetShellReduceByTopAngle();
            

            // Create Structure
            structureCRTModel = structureService.CreateStructureCRTColumn(
                
                                    rafterInputList, columnInputList, girderInputList,

                                    girderHbeamList, columnCenterTopSupportList,
                                    columnSideTopSupportList, columnPipeList,

                                    rafterOutputList,

                                    selTankID,
                                    selTankHeight,
                                    selAnnularInnerWidth,
                                    selRoofOD,
                                    selBottomThk,
                                    selRoofSlope,
                                    selBottoSlope,
                                    selShellReduce   );


        }


        private double GetShellReduceByTopAngle()
        {
            double returnValue = 0;
            if (drawCommon.GetCurrentTopAngleType() == TopAngle_Type.k)
            {
                if (assemblyData.ShellOutput.Count >= 2)
                {
                    int maxCourse = assemblyData.ShellOutput.Count - 1;
                    double lastThk = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness);
                    double lastThkBefore = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse - 1].Thickness);
                    returnValue = (lastThk - lastThkBefore) / 2;
                }
            }

            return returnValue;
        }


        #region Detail Sample
        private double GetStructureCustomScale(PaperAreaModel selModel)
        {
            double returnValue = 1;

            // Custom Scale
            switch (selModel.SubName)
            {
                case PAPERSUB_TYPE.RoofStructureAssembly:
                    //selModel.ScaleValue=1
                    break;
            }

            // AutoScale
            if (selModel.ScaleValue == 0)
                selModel.ScaleValue = 1;
            returnValue = selModel.ScaleValue;
            return returnValue;
        }

        private DrawEntityModel DrawDetailAssembly(Point3D refPoint, object selModel, double scaleValue,PaperAreaModel selPaperModel)
        {
            DrawEntityModel drawList = new DrawEntityModel();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);
            List<Entity> newList = new List<Entity>();



            double testRecSize = 80 * scaleValue;
            double textHeight = 10 * scaleValue;

            Point3D recPoint = GetSumPoint(referencePoint, -testRecSize/2, -testRecSize/2);
            Line testLine01 = new Line(GetSumPoint(recPoint, 0, 0), GetSumPoint(recPoint, 0, testRecSize));
            Line testLine02 = new Line(GetSumPoint(recPoint, 0, testRecSize), GetSumPoint(recPoint, testRecSize, testRecSize));
            Line testLine03 = new Line(GetSumPoint(recPoint, testRecSize, testRecSize), GetSumPoint(recPoint, testRecSize,0));
            Line testLine04 = new Line(GetSumPoint(recPoint, testRecSize, 0), GetSumPoint(recPoint, 0, 0));


            Text text01 = new Text(GetSumPoint(referencePoint, 0, 0), selPaperModel.SubName.ToString(), textHeight);
            text01.Alignment = Text.alignmentType.MiddleCenter;


            newList.Add(testLine01);
            newList.Add(testLine02);
            newList.Add(testLine03);
            newList.Add(testLine04);

            newList.Add(text01);

            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(newList);

            return drawList;
        }
        #endregion





        #region One Size
        public DrawEntityModel DrawDetailCenterColumn_BB(Point3D refPoint, object selModel, double scaleValue, PaperAreaModel selPaperModel)
        {
            DrawEntityModel drawList = new DrawEntityModel();
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);
            List<Entity> newList = new List<Entity>();


            //newList.AddRange(drawShareService.GetCenterColumn_BB(referencePoint, scaleValue));

            newList.AddRange(drawShareService.GetCenterColumn_Detail_C(GetSumPoint(referencePoint,1000,0), scaleValue));
            newList.AddRange(drawShareService.GetCenterColumn_Detail_D(GetSumPoint(referencePoint, 2000, 0), scaleValue)); // 작성중
            newList.AddRange(drawShareService.GetCenterColumn_EE(GetSumPoint(referencePoint, 3000, 0), scaleValue));
            newList.AddRange(drawShareService.GetCenterColumn_Drain_Detail(GetSumPoint(referencePoint, 4000, 0), scaleValue));
            newList.AddRange(drawShareService.GetCenterColumn_BB_Edit(GetSumPoint(referencePoint, 5000, 0), scaleValue));
            

            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(newList);

            return drawList;
        }
        #endregion






        public DrawEntityModel DrawDetailRoofStructureAssembly(Point3D refPoint, object selModel, double scaleValue, PaperAreaModel selPaperModel)
        {
            // List
            DrawEntityModel drawList = new DrawEntityModel();

            // Reference Point
            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            Point3D shellBottomPoint = GetSumPoint(referencePoint, 0, 0);

            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;
            //double roofSlope= valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope);
            double bottomSlope = valueService.GetDegreeOfSlope(assemblyData.BottomInput[0].BottomPlateSlope);

            List<Entity> topAngleList = new List<Entity>();
            List<Entity> shellList = new List<Entity>();
            List<Entity> bottomList = new List<Entity>();
            List<Entity> roofList = new List<Entity>();

            List<Entity> clipList = new List<Entity>();
            List<Entity> rafterList = new List<Entity>();
            List<Entity> columnList = new List<Entity>();

            List<Entity> centerLineList = new List<Entity>();


            List<Entity> etcList = new List<Entity>();

            double shellHeight = 0;
            double bottomCenterOpposite = 0;
            double roofCenterOpposite = 0;

            double shellHeightPercentage = 100;

            double shellSideHeight = 0;
            double centerSideHeight = 0;

            Point3D roofCenterLowerPoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterTopDown, referencePoint); 
            Point3D roofCenterUpperPoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterTopUp, referencePoint); 
            Point3D roofSideLowerPoint= workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofDown, referencePoint); 
            Point3D roofSideUpperPoint= workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofUp, referencePoint);


            Point3D bottomCenterLowerPoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterBottomDown, referencePoint);
            Point3D bottomCenterUpperPoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterBottomUp, referencePoint);
            Point3D bottomSideLowerPoint= GetSumPoint(shellBottomPoint, 0, 0);
            Point3D bottomSideUpperPoint = GetSumPoint(shellBottomPoint, 0, 0);





            List<Point3D> rafterOutputPointList = new List<Point3D>();

            // CenterLine
            double exLength = 6;
            centerLineList.AddRange( editingService.GetCenterLine(GetSumPoint(roofCenterUpperPoint, 0,0), GetSumPoint(bottomCenterLowerPoint, 0, 0), exLength,scaleValue));

            // Shell
            shellList.AddRange(GetShellAssembly(GetSumPoint(referencePoint,0,0))) ;
            // Bottom
            bottomList.AddRange(GetBottomAssembly(GetSumPoint(referencePoint, 0, 0)));
            // TopAngle
            topAngleList.AddRange(GetTopAngleAssembly(GetSumPoint(referencePoint, 0, 0)));
            // Roof
            roofList.AddRange(GetRoofAssembly(GetSumPoint(referencePoint, 0, 0)));





            // Clip
            Point3D roofStartPoint=  workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofDown, referencePoint);
            Point3D clipShellSidePoint = GetClipShellSidePoint(referencePoint);
            StructureRafterModel rafterLastModel = structureService.GetShortRafterInLayer(structureCRTModel.LayerList[structureCRTModel.LayerList.Count - 1].RafterList);
            NRafterSupportClipShellSideModel rafterClipShellSide = GetNewRafterSupportClipShellSideModel(rafterLastModel.Size);
            clipList.AddRange(drawShareService.RafterSideClipDetail(clipShellSidePoint, roofStartPoint, singleModel, scaleValue, rafterClipShellSide).GetDrawEntity());


            // Rafter, Column, Girder
            double centerFromCL = 0;
            


            for (int layerIndex = 0; layerIndex < structureCRTModel.LayerList.Count; layerIndex++)
            {
                StructureLayerModel eachLayer = structureCRTModel.LayerList[layerIndex];
                double columnRadius = eachLayer.Radius;
                Point3D bottomUppderRadiusPoint= workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.AdjCenterBottomUp,columnRadius, referencePoint);

                centerFromCL = eachLayer.Radius;
                rafterList.Add(new Line(GetSumPoint(roofCenterLowerPoint, -centerFromCL, -3000), GetSumPoint(roofCenterLowerPoint, -centerFromCL, 1000))); // 검증 라인

                // Center
                if (layerIndex == 0)
                {
                    StructureColumnModel centerColumn = new StructureColumnModel();
                    if (eachLayer.ColumnList.Count > 0) 
                    {
                        centerColumn = eachLayer.ColumnList[0];

                        // Column : Center Top Support => ref : Rafter Size
                        StructureLayerModel outerLayer = structureCRTModel.LayerList[layerIndex + 1];
                        StructureRafterModel outerRafter = structureService.GetLongRafterInLayer(outerLayer.RafterList);
                        NColumnCenterTopSupportModel nCenterColumn = GetNewColumnCenterTopSupportModel(outerRafter.Size);
                        PipeModel eachPipe = GetPipeModel(nCenterColumn.ColumnSize);
                        // Column
                        Point3D columnCenterTopPoint = GetSumPoint(bottomUppderRadiusPoint, 0, centerColumn.Height);
                        columnList.AddRange(drawShareService.DrawColumnCenterTopSupport(columnCenterTopPoint, singleModel, scaleValue, nCenterColumn, eachPipe).GetDrawEntity());

                        // Column Clip
                        StructureGirderModel eachGirder = new StructureGirderModel();
                        if (eachLayer.GirderList.Count > 0)
                        {
                            eachGirder = eachLayer.GirderList[0];
                            StructureClipModel eachClip = eachGirder.ClipList[0];
                            double columnClipDistance = valueService.GetDoubleValue(nCenterColumn.B);
                            Point3D columnCenterLeftClipPoint = GetSumPoint(bottomUppderRadiusPoint, -columnClipDistance, centerColumn.Height);
                            Point3D columnCenterRightClipPoint = GetSumPoint(bottomUppderRadiusPoint, columnClipDistance, centerColumn.Height);
                            columnList.AddRange(GetClipAssembly(GetSumPoint(columnCenterLeftClipPoint, 0, 0), eachClip.ClipWidth, eachClip.ClipHeight, scaleValue));
                            columnList.AddRange(GetClipAssembly(GetSumPoint(columnCenterRightClipPoint, 0, 0), eachClip.ClipWidth, eachClip.ClipHeight, scaleValue));
                        }

                    }

                }

                // Side
                else
                {
                    // Rafter : Current -> Inner
                    if (eachLayer.RafterList.Count > 0)
                    {

                        StructureRafterModel eachRafter = structureService.GetShortRafterInLayer(eachLayer.RafterList);
                        if(layerIndex==1)
                            eachRafter = structureService.GetLongRafterInLayer(eachLayer.RafterList);

                        NColumnRafterModel nColumnRafter = GetRafterModel(eachRafter.Size);
                        double sideRafterStartX = -eachRafter.InnerRadiusFromCenter;
                        double sideRafterStartY = -valueService.GetOppositeByAdjacent(assemblyComData.RoofSlopeRadian, eachRafter.InnerRadiusFromCenter);
                        rafterList.AddRange(GetRafterAssembly(out rafterOutputPointList, 
                                                GetSumPoint(roofCenterLowerPoint, sideRafterStartX, sideRafterStartY), 
                                                eachRafter.Length, assemblyComData.RoofSlopeRadian, 1, 1, null, nColumnRafter, scaleValue));

                        //rafterList.Add(new Line(GetSumPoint(roofCenterLowerPoint, sideRafterStartX, 0), GetSumPoint(roofCenterLowerPoint, sideRafterStartX, -1000))); // 검증 라인
                    }

                    // Column : Side Top Support
                    if (eachLayer.ColumnList.Count > 0)
                    {
                        StructureColumnModel sidenColumn = eachLayer.ColumnList[0];

                        StructureRafterModel eachRafter = structureService.GetShortRafterInLayer(eachLayer.RafterList);

                        NColumnCenterTopSupportModel nCenterColumn = GetNewColumnCenterTopSupportModel(eachRafter.Size);
                        NColumnSideTopSupportModel nSideColumn = GetNewColumnSideTopSupportModel(nCenterColumn.ColumnSize);
                        PipeModel eachPipe = GetPipeModel(nCenterColumn.ColumnSize);

                        // Column
                        Point3D columnSideTopPoint = GetSumPoint(bottomUppderRadiusPoint, 0, sidenColumn.Height);
                        columnList.AddRange(drawShareService.DrawColumnSideTopSupport(columnSideTopPoint, singleModel, scaleValue, nSideColumn, eachPipe).GetDrawEntity());
                    }
                }

                // Column : Base
                //DrawEntityModel structureBaseDraw = DDSServiceShare.DrawColumnBaseSupport(refPoint, singleModel, scaleValue, baseModel);

            }

            // 
            // 1
            //etcList.AddRange(drawComService.GetColumnCenterTopSupport_TopView(GetSumPoint(referencePoint, 0, 0), scaleValue));
            // 2
            //etcList.AddRange(drawComService.GetColumnCenterTopSupport(GetSumPoint(referencePoint, 1000, 0), scaleValue));
            // 3





            //etcList.AddRange(drawShareService.Rafter(GetSumPoint(referencePoint, 0, tankHeight), scaleValue,));



            drawList.outlineList.AddRange(columnList);

            styleService.SetLayerListEntity(ref rafterList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(rafterList);

            styleService.SetLayerListEntity(ref clipList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(clipList);

            styleService.SetLayerListEntity(ref topAngleList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(topAngleList);

            styleService.SetLayerListEntity(ref topAngleList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(topAngleList);

            styleService.SetLayerListEntity(ref shellList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(shellList);

            styleService.SetLayerListEntity(ref bottomList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(bottomList);

            styleService.SetLayerListEntity(ref roofList, layerService.LayerVirtualLine);
            drawList.outlineList.AddRange(roofList);

            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            drawList.outlineList.AddRange(centerLineList);

            drawList.outlineList.AddRange(etcList);

            return drawList;
        }

        public List<Entity> GetTopAngleAssembly(Point3D refPoint)
        {
            List<Entity> newList = new List<Entity>();
            List<Entity> leftList = new List<Entity>();

            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);


            Point3D anglePoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofDown, referencePoint);

            string selAngleType = assemblyData.RoofCompressionRing[0].CompressionRingType;
            string selAngleSize = assemblyData.RoofCompressionRing[0].AngleSize;
            AngleSizeModel selAngleModel = refBlockService.GetAngleSizeModel(selAngleSize);

            int maxCourse = assemblyData.ShellOutput.Count - 1;
            double lastShellCourseThk = -valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness);

            TopAngle_Type currentAngle = drawCommon.GetCurrentTopAngleType();
            switch (currentAngle)
            {
                case TopAngle_Type.b:
                case TopAngle_Type.d:
                case TopAngle_Type.e:
                    leftList.AddRange(refBlockService.DrawReference_Angle(GetSumCDPoint(anglePoint, 0, 0), selAngleModel));
                    break;
                case TopAngle_Type.i:
                    anglePoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftShellTop, referencePoint);
                    leftList.AddRange(refBlockService.DrawReference_CompressionRingI(GetSumCDPoint(anglePoint,lastShellCourseThk,0)));
                    break;
                case TopAngle_Type.k:
                    anglePoint = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftShellTop, referencePoint);
                    leftList.AddRange(refBlockService.DrawReference_CompressionRingK(GetSumCDPoint(anglePoint, 0, 0)));
                    break;
                

            }


            


            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;
            Point3D mirrorPoint = GetSumPoint(referencePoint, tankIDHalf, 0);

            // 좌측 하단이 기준

            newList.AddRange(leftList);
            newList.AddRange(editingService.GetMirrorEntity(Plane.YZ, leftList, GetSumPoint(mirrorPoint, 0, 0), true));

            return newList;
        }


        public List<Entity> GetRoofAssembly(Point3D refPoint)
        {
            List<Entity> newList = new List<Entity>();
            List<Entity> leftList = new List<Entity>();

            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);
            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;

            Point3D mirrorPoint = GetSumPoint(referencePoint, tankIDHalf, 0);


            Point3D leftRoofDown = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofDown, referencePoint);
            Point3D leftRoofUp = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofUp, referencePoint);
            Point3D centerRoofUp = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterTopUp, referencePoint);
            Point3D centerRoofDown = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterTopDown, referencePoint);


            Line plateLowerLine = new Line(GetSumPoint(leftRoofDown, 0, 0), GetSumPoint(centerRoofDown, 0, 0));
            Line plateLeftLine = new Line(GetSumPoint(leftRoofDown, 0, 0), GetSumPoint(leftRoofUp, 0, 0));
            Line plateUpperLine = new Line(GetSumPoint(leftRoofUp, 0, 0), GetSumPoint(centerRoofUp, 0, 0));


            leftList.Add(plateLowerLine);
            leftList.Add(plateLeftLine);
            leftList.Add(plateUpperLine);



            newList.AddRange(leftList);
            newList.AddRange(editingService.GetMirrorEntity(Plane.YZ, leftList, GetSumPoint(mirrorPoint, 0, 0), true));

            return newList;
        }

        public List<Entity> GetBottomAssembly(Point3D refPoint)
        {
            List<Entity> newList = new List<Entity>();
            List<Entity> bottomPlateLeft = new List<Entity>();
            List<Entity> annularPlateLeft= new List<Entity>();


            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;
            Point3D tankCenterPoint = GetSumPoint(refPoint, tankIDHalf, 0);




            double bottomSlope = valueService.GetDegreeOfSlope(assemblyData.BottomInput[0].BottomPlateSlope);
            double bottomOD = valueService.GetDoubleValue(assemblyData.BottomInput[0].OD);
            double bottomODHalf = bottomOD / 2;
            double bottomThickness = valueService.GetDoubleValue(assemblyData.BottomInput[0].BottomPlateThickness);
            double bottomThicknessHyp = valueService.GetHypotenuseByWidth(bottomSlope, bottomThickness);

            double bottomLeftAdj = 0;
            double bottomCenterOpposite = 0;
            Point3D bottomLeftStartPoint = new Point3D();
            Point3D bottomCenterPoint = new Point3D();
            Point3D bottomCenterUpperPoint = new Point3D();


            List<Point3D> outPointList = new List<Point3D>();
            if (drawCommon.isAnnular())
            {
                double annularOD = valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateOD);
                double annularID = valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateID);
                double annularThickness = valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateThickness);
                double annularWidth = valueService.GetDoubleValue(assemblyData.BottomInput[0].AnnularPlateWidthWidth);
                double annularOutsideProjection = (annularOD - tankID)/2;

                Point3D annularPoint = GetSumPoint(referencePoint, -annularOutsideProjection, 0);
                annularPlateLeft.AddRange(shapeService.GetRectangle(out outPointList, GetSumPoint(annularPoint, 0, 0), annularWidth, annularThickness, 0, 0, 0));


                bottomLeftAdj = valueService.GetAdjacentByHypotenuse(bottomSlope, bottomODHalf);
                bottomCenterOpposite = valueService.GetOppositeByHypotenuse(bottomSlope, bottomODHalf);
                bottomLeftStartPoint = GetSumPoint(tankCenterPoint, -bottomLeftAdj, 0);

                bottomCenterPoint = GetSumPoint(tankCenterPoint, 0, bottomCenterOpposite);
            }
            else
            {

                double bottomCenterOppOfTankID = valueService.GetOppositeByAdjacent(bottomSlope, tankIDHalf);
                bottomCenterPoint = GetSumPoint(tankCenterPoint, 0, bottomCenterOppOfTankID);

                bottomLeftAdj = valueService.GetAdjacentByHypotenuse(bottomSlope, bottomODHalf);
                bottomCenterOpposite = valueService.GetOppositeByHypotenuse(bottomSlope, bottomODHalf);
                bottomLeftStartPoint = GetSumPoint(bottomCenterPoint, -bottomLeftAdj, -bottomCenterOpposite);


            }

            bottomCenterUpperPoint = GetSumPoint(bottomCenterPoint, 0, bottomThicknessHyp);

            Line plateLowerLine = new Line(GetSumPoint(bottomLeftStartPoint, 0, 0), GetSumPoint(bottomCenterPoint, 0, 0));
            Line plateLeftLine = new Line(GetSumPoint(bottomLeftStartPoint, 0, 0), GetSumPoint(bottomLeftStartPoint, 0, bottomThickness));
            plateLeftLine.Rotate(bottomSlope, Vector3D.AxisZ, GetSumPoint(bottomLeftStartPoint, 0, 0));
            Line plateUpperLine = new Line(GetSumPoint(plateLeftLine.EndPoint, 0, 0), GetSumPoint(bottomCenterUpperPoint, 0, 0));


            bottomPlateLeft.Add(plateLowerLine);
            bottomPlateLeft.Add(plateUpperLine);
            bottomPlateLeft.Add(plateLeftLine);

            newList.AddRange(annularPlateLeft);
            newList.AddRange(bottomPlateLeft);
            newList.AddRange(editingService.GetMirrorEntity(Plane.YZ, annularPlateLeft, GetSumPoint(tankCenterPoint, 0, 0), true));
            newList.AddRange(editingService.GetMirrorEntity(Plane.YZ, bottomPlateLeft, GetSumPoint(tankCenterPoint, 0, 0), true));

            return newList;
        }

        public List<Entity> GetShellAssembly(Point3D refPoint)
        {
            List<Entity> newList = new List<Entity>();
            List<Entity> leftList = new List<Entity>();

            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);
            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double tankID= valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;

            Point3D mirrorPoint = GetSumPoint(referencePoint, tankIDHalf, 0);

            // 좌측 하단이 기준


            List<double> shellWidth=drawCommon.GetShellCourseWidthForDrawing();
            List<double> shellThickness = drawCommon.GetShellCourseThickneeForDrawing();

            List<Point3D> outPointList = new List<Point3D>();
            Point3D shellCurrentPoint = GetSumPoint(referencePoint, 0, 0);
            for(int i=0;i<shellWidth.Count;i++)
            {
                double eachWidth = shellWidth[i];
                double eachThickness = shellThickness[i];
                leftList.AddRange(shapeService.GetRectangle(out outPointList, GetSumPoint(shellCurrentPoint, 0, 0), eachThickness, eachWidth, 0, 0, 2));
                shellCurrentPoint = GetSumPoint(outPointList[1],0,0);
            }

            //Line leftID = new Line(GetSumPoint(referencePoint, 0, 0), GetSumPoint(refPoint, 0, tankHeight));
            //leftList.Add(leftID);


            newList.AddRange(leftList);
            newList.AddRange(editingService.GetMirrorEntity(Plane.YZ, leftList, GetSumPoint(mirrorPoint, 0, 0), true));

            return newList;
        }

        public List<Entity> GetStructureRafterAssembly(Point3D refPoint,double scaleValue)
        {
            List<Entity> newList = new List<Entity>();
            List<Entity> leftList = new List<Entity>();

            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);
            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;

            Point3D mirrorPoint = GetSumPoint(referencePoint, tankIDHalf, 0);

            NColumnRafterModel selRafter = new NColumnRafterModel();
            //leftList.AddRange(drawShareService.Rafter(GetSumPoint(referencePoint, 0, tankHeight), scaleValue,selRa));


            Point3D leftRoofDown = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofDown, referencePoint);
            Point3D leftRoofUp = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofUp, referencePoint);
            Point3D centerRoofDown = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterTopUp, referencePoint);
            Point3D centerRoofUp = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointCenterTopDown, referencePoint);


            Line plateLowerLine = new Line(GetSumPoint(leftRoofDown, 0, 0), GetSumPoint(centerRoofDown, 0, 0));
            Line plateLeftLine = new Line(GetSumPoint(leftRoofDown, 0, 0), GetSumPoint(leftRoofUp, 0, 0));
            Line plateUpperLine = new Line(GetSumPoint(leftRoofUp, 0, 0), GetSumPoint(centerRoofUp, 0, 0));


            leftList.Add(plateLowerLine);
            leftList.Add(plateLeftLine);
            leftList.Add(plateUpperLine);



            newList.AddRange(leftList);
            newList.AddRange(editingService.GetMirrorEntity(Plane.YZ, leftList, GetSumPoint(mirrorPoint, 0, 0), true));

            return newList;
        }


        public List<Entity> GetClipAssembly(Point3D refPoint,double selWidth, double selHeight, double scaleValue)
        {
            List<Entity> newList = new List<Entity>();
            
            // Cetner Lower
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            double width = selWidth;
            double widthHalf = width / 2;
            double height = selHeight;

            Line topLine = new Line(GetSumPoint(referencePoint, -widthHalf, height), GetSumPoint(referencePoint, widthHalf, height));
            Line leftLine = new Line(GetSumPoint(referencePoint, -widthHalf, height), GetSumPoint(referencePoint, -widthHalf, 0));
            Line rightLine = new Line(GetSumPoint(referencePoint, widthHalf, height), GetSumPoint(referencePoint, widthHalf, 0));

            newList.Add(topLine);
            newList.Add(leftLine);
            newList.Add(rightLine);
            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            return newList;
        }



        // Model Point
        public Point3D GetClipShellSidePoint(Point3D refPoint)
        {
            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double shellReduce = GetShellReduceByTopAngle();
            Point3D returnPoint = GetSumPoint(refPoint, shellReduce,tankHeight);
            return returnPoint;
        }
        public Point3D GetRafterShellSidePoint(Point3D refPoint)
        {
            double shellGap = 70;// 차후 값을 받아서 변환 해야 함
            double roofSlope = valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope);
            Point3D leftRoofDown = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.PointLeftRoofDown, refPoint);
            leftRoofDown = workingPointService.WorkingPointNew(WORKINGPOINT_TYPE.AdjLeftRoofDown,shellGap, refPoint);
            Point3D returnPoint = GetSumPoint(GetClipShellSidePoint(refPoint), shellGap, valueService.GetOppositeByAdjacent(roofSlope, shellGap));
            return returnPoint;
        }



        public DrawEntityModel DrawDetailRoofStructureOrientation(Point3D refPoint, object selModel, double scaleValue, PaperAreaModel selPaperModel)
        {
            // List
            DrawEntityModel drawList = new DrawEntityModel();

            // Reference Point
            Point3D referencePoint = new Point3D(refPoint.X, refPoint.Y);

            Point3D shellBottomPoint = GetSumPoint(referencePoint, 0, 0);

            double tankHeight = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeTankHeight);
            double tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);
            double tankIDHalf = tankID / 2;
            double roofSlope = valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope);
            double bottomSlope = valueService.GetDegreeOfSlope(assemblyData.BottomInput[0].BottomPlateSlope);


            List<Entity> orientationList = new List<Entity>();

            List<Entity> centerLineList = new List<Entity>();





            for (int layerIndex = 0; layerIndex < structureCRTModel.LayerList.Count; layerIndex++)
            {
                StructureLayerModel eachLayer = structureCRTModel.LayerList[layerIndex];
                double columnRadius = eachLayer.Radius;

                // Center
                if (layerIndex == 0)
                {
                    // Center : Pipe
                    orientationList.AddRange(GetColumnCenterSideTopview(referencePoint,))
                }

                // Side
                else
                {
                    
                    
                }

                


            }











            styleService.SetLayerListEntity(ref orientationList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(orientationList);

            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            drawList.outlineList.AddRange(centerLineList);

            return drawList;



        }

        public List<Entity> GetColumnCenterSideTopview(Point3D refPoint, NColumnCenterTopSupportModel centerModel)
        {
            List<Entity> newList = new List<Entity>();

            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            double B = valueService.GetDoubleValue(centerModel.B);
            double G = valueService.GetDoubleValue(centerModel.G);
            double A1 = valueService.GetDoubleValue(centerModel.A1);

            PipeModel pipeModel = GetPipeModel(centerModel.ColumnSize);
            double pipeRadius = valueService.GetDoubleValue(pipeModel.OD) / 2;

            double centerSupportRadius = pipeRadius + G + A1;
            double clipRadius = B;



            Circle pipeCir = new Circle(GetSumPoint(referencePoint, 0, 0), pipeRadius);
            Circle clipCir = new Circle(GetSumPoint(referencePoint, 0, 0), clipRadius);
            Circle supportCir = new Circle(GetSumPoint(referencePoint, 0, 0), centerSupportRadius);

            styleService.SetLayer(ref pipeCir, layerService.LayerOutLine);
            styleService.SetLayer(ref clipCir, layerService.LayerCenterLine);
            styleService.SetLayer(ref supportCir, layerService.LayerOutLine);
            newList.Add(pipeCir);
            newList.Add(clipCir);
            newList.Add(supportCir);


            return newList;
        }
        



        public List<Entity> GetRafterAssembly(out List<Point3D> selOutputPointList, Point3D refPoint, double selLength, double selRotate, double selRotateCenter, double selTranslateNumber, bool[] selVisibleLine = null,
                            NColumnRafterModel padModel = null,double scaleValue=1)
        {
            selOutputPointList = new List<Point3D>();
            List<Entity> newList = new List<Entity>();
            List<Entity> slotHoleList = new List<Entity>();


            // Model Data
            double A = valueService.GetDoubleValue(padModel.A);    //rafter Lenth
            double A1 = valueService.GetDoubleValue(padModel.A1);   //woking Point to rafter : NOT USED
            double B = valueService.GetDoubleValue(padModel.B);    //rafter Width
            double B1 = valueService.GetDoubleValue(padModel.B1);   //workingPoint부터 rafter까지의 x축 거리
            double C = valueService.GetDoubleValue(padModel.C);    //centering Point to rafter : NOT USED  //rafter to hole : NOT USED
            double C1 = valueService.GetDoubleValue(padModel.C1);   //hole to hole length gap
            double D = valueService.GetDoubleValue(padModel.D);    //hole to hole width gap
            double E = valueService.GetDoubleValue(padModel.E);    //SHELL ID/4 : NOT USED
            double holeDia = valueService.GetDoubleValue(padModel.BoltHoleDia);

            double selHeight = A;

            double t = 6;   //Thk of doubleLine

            // Reference Point : Top Left
            Point3D pointA = GetSumPoint(refPoint, 0, 0);
            Point3D pointB = GetSumPoint(refPoint, selLength, 0);
            Point3D pointC = GetSumPoint(refPoint, selLength, -selHeight);
            Point3D pointD = GetSumPoint(refPoint, 0, -selHeight);

            // Line
            Line lineA = new Line(GetSumPoint(pointA, 0, 0), GetSumPoint(pointB, 0, 0));
            Line lineB = new Line(GetSumPoint(pointB, 0, 0), GetSumPoint(pointC, 0, 0));
            Line lineC = new Line(GetSumPoint(pointC, 0, 0), GetSumPoint(pointD, 0, 0));
            Line lineD = new Line(GetSumPoint(pointD, 0, 0), GetSumPoint(pointA, 0, 0));

            // Inner Line : HBeam,Channel Tpye에 따라서 달라짐
            Line lineAinner = (Line)lineA.Offset(t, Vector3D.AxisZ);
            Line lineCinner = (Line)lineC.Offset(t, Vector3D.AxisZ);

            newList.AddRange(new Line[] { lineA, lineB, lineC, lineD, lineAinner, lineCinner });

            // Slot Hole : Center Line 처리 필요함
            double holeQty = 2;
            // hole Plan

            double holePositionX = B1 + C1 / 2;

            slotHoleList.AddRange(GetHolesQty(GetSumPoint(lineD.MidPoint, holePositionX, 0), holeDia, C1, holeQty, D));
            slotHoleList.AddRange(GetHolesQty(GetSumPoint(lineB.MidPoint, -holePositionX, 0), holeDia, C1, holeQty, D));
            newList.AddRange(slotHoleList);

            if (selRotate != 0)
            {
                Point3D WPRotate = GetSumPoint(pointA, 0, 0);
                if (selRotateCenter == 1)
                    WPRotate = GetSumPoint(pointB, 0, 0);
                else if (selRotateCenter == 2)
                    WPRotate = GetSumPoint(pointC, 0, 0);
                else if (selRotateCenter == 3)
                    WPRotate = GetSumPoint(pointD, 0, 0);

                foreach (Entity eachEntity in newList)
                    eachEntity.Rotate(selRotate, Vector3D.AxisZ, WPRotate);
            }

            // 나중에 맨 앞으로 옮겨야 함
            if (selTranslateNumber > 0)
            {
                Point3D WPTranslate = new Point3D();
                if (selTranslateNumber == 1)
                    WPTranslate = GetSumPoint(pointB, 0, 0);
                else if (selTranslateNumber == 2)
                    WPTranslate = GetSumPoint(pointC, 0, 0);
                else if (selTranslateNumber == 3)
                    WPTranslate = GetSumPoint(pointD, 0, 0);
                editingService.SetTranslate(ref newList, GetSumPoint(refPoint, 0, 0), WPTranslate);

            }
            if (selVisibleLine != null)
            {
                if (selVisibleLine[0] == false)
                    newList.Remove(lineA);
                if (selVisibleLine[1] == false)
                    newList.Remove(lineB);
                if (selVisibleLine[2] == false)
                    newList.Remove(lineC);
                if (selVisibleLine[3] == false)
                    newList.Remove(lineD);
            }

            selOutputPointList.Add(lineA.StartPoint);
            selOutputPointList.Add(lineA.EndPoint);
            selOutputPointList.Add(lineC.StartPoint);
            selOutputPointList.Add(lineC.EndPoint);

            return newList;
        }
        private List<Entity> GetHoles(Point3D refPoint, double holeRadius, double lengthGap, double widthGap = 0)
        {
            //refPoint는 홀과 홀 사이의 중앙인 점을 입력해주시면됩니다
            //lengthGap=0인경우는 2개, lengthGap에 값이 입력된 경우는 홀이 4개로 그려집니다 :)

            List<Entity> holeList = new List<Entity>();
            Point3D workingPoint = GetSumPoint(refPoint, 0, 0);


            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Point3D circleCenterPoint = GetSumPoint(workingPoint, lengthGap / 2 * (2 * i - 1), widthGap / 2 * (1 - 2 * j));

                    Circle hole = new Circle(circleCenterPoint, holeRadius / 2);

                    holeList.AddRange(new Entity[] { hole });
                }
            }

            styleService.SetLayerListEntity(ref holeList, layerService.LayerOutLine);

            return holeList;

        }

        private List<Entity> GetHolesQty(Point3D refPoint, double holeRadius, double lengthGap, double holeQty, double widthGap = 0)
        {
            //refPoint는 홀과 홀 사이의 중앙인 점을 입력해주시면됩니다

            List<Entity> holeList = new List<Entity>();
            Point3D workingPoint = GetSumPoint(refPoint, 0, 0);


            if (holeQty == 2)
            {
                Point3D leftCircleCenterPoint = GetSumPoint(workingPoint, lengthGap / 2, 0, 0);
                Point3D rightCircleCenterPoint = GetSumPoint(workingPoint, -lengthGap / 2, 0, 0);
                Circle leftHole = new Circle(leftCircleCenterPoint, holeRadius / 2);
                Circle rightHole = new Circle(rightCircleCenterPoint, holeRadius / 2);
                holeList.AddRange(new Entity[] { leftHole, rightHole });
            }

            else if (holeQty == 4)
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        Point3D circleCenterPoint = GetSumPoint(workingPoint, lengthGap / 2 * (2 * i - 1), widthGap / 2 * (1 - 2 * j));

                        Circle hole = new Circle(circleCenterPoint, holeRadius / 2);

                        holeList.AddRange(new Entity[] { hole });
                    }
                }
                // hole 순서 : [0]좌측상단, [1]좌측하단, [2]우측상단, [3]우측하단
            }

            return holeList;
        }

        #region Assembly Model : Structure
        private NColumnRafterModel GetRafterModel(string rafterSize)
    {
        NColumnRafterModel newModel = new NColumnRafterModel();
        foreach (NColumnRafterModel eachModel in assemblyData.NColumnRafterList)
        {
            if (eachModel.Size == rafterSize)
            {
                newModel = eachModel;
                break;
            }
        }
        return newModel;
    }

        private NColumnCenterTopSupportModel GetNewColumnCenterTopSupportModel(string selSize)
        {
            NColumnCenterTopSupportModel newModel = new NColumnCenterTopSupportModel();
            foreach (NColumnCenterTopSupportModel eachModel in assemblyData.NColumnCenterTopSupportList)
            {
                if (eachModel.RafterSize == selSize)
                {
                    newModel = eachModel;
                    break;
                }
            }
            return newModel;
        }

        private NColumnSideTopSupportModel GetNewColumnSideTopSupportModel(string selSize)
        {
            NColumnSideTopSupportModel newModel = new NColumnSideTopSupportModel();
            foreach (NColumnSideTopSupportModel eachModel in assemblyData.NColumnSideTopSupportList)
            {
                if (eachModel.ColumnSize == selSize)
                {
                    newModel = eachModel;
                    break;
                }
            }
            return newModel;
        }


        private NRafterSupportClipShellSideModel GetNewRafterSupportClipShellSideModel(string selSize)
        {
            NRafterSupportClipShellSideModel newModel = new NRafterSupportClipShellSideModel();
            foreach (NRafterSupportClipShellSideModel eachModel in assemblyData.NRafterSupportClipShellSideList)
            {
                if (eachModel.RafterSize == selSize)
                {
                    newModel = eachModel;
                    break;
                }
            }
            return newModel;
        }


        private HBeamModel GetHBeamModel(string hBeamSize)
        {
            HBeamModel newModel = new HBeamModel();
            foreach (HBeamModel eachModel in assemblyData.HBeamList)
            {
                if (eachModel.SIZE == hBeamSize)
                {
                    newModel = eachModel;
                    break;
                }
            }
            return newModel;
        }

        private PipeModel GetPipeModel(string selSize)
        {
            PipeModel newModel = new PipeModel();
            foreach (PipeModel eachModel in assemblyData.PipeList)
            {
                if (eachModel.NPS == selSize)
                {
                    newModel = eachModel;
                    break;
                }
            }
            return newModel;
        }



        #endregion

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }
        private CDPoint GetSumCDPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new CDPoint(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }





        #region Lee

        #endregion



    }
}
