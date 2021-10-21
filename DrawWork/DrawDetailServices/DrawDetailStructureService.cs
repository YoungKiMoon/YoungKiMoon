using AssemblyLib.AssemblyModels;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;
using DrawSettingLib.Commons;
using DrawSettingLib.SettingModels;
using DrawSettingLib.SettingServices;
using DrawShapeLib.DrawServices;
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
                    drawList.AddDrawEntity(DrawDetailRoofStructureOrientation(referencePoint, selModel, scaleValue, eachModel));
                }
                else if (eachModel.SubName == PAPERSUB_TYPE.RoofStructureAssembly)
                {
                    drawList.AddDrawEntity(DrawDetailRoofStructureAssembly(referencePoint, selModel, scaleValue));
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
            NColumnCenterTopSupportModel columnCenterTopSupport = new NColumnCenterTopSupportModel();
            if (rafterInputList.Count > 0)
            {
                StructureCRTRafterInputModel rafter = rafterInputList[0];
                columnCenterTopSupport = GetNewColumnCenterTopSupportModel(rafter.Size);
            }

            // Output : Column -> First Column = Center Top Support : Pipe
            PipeModel columnCenterPipe = new PipeModel();
            if (columnInputList.Count > 0)
            {
                StructureCRTColumnInputModel column = columnInputList[0];
                columnCenterPipe = GetPipeModel(column.Size);
            }
            // Pipe Schedule 적용 안됨



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
            double selShellReduce = 0;
            if (drawCommon.GetCurrentTopAngleType() == TopAngle_Type.k)
            {
                if (assemblyData.ShellOutput.Count >= 2)
                {
                    int maxCourse = assemblyData.ShellOutput.Count - 1;
                    double lastThk = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse].Thickness);
                    double lastThkBefore = valueService.GetDoubleValue(assemblyData.ShellOutput[maxCourse - 1].Thickness);
                    selShellReduce = (lastThk - lastThkBefore) / 2;
                }
            }

            // Create Structure
            structureCRTModel = structureService.CreateStructureCRTColumn(
                
                                    rafterInputList, columnInputList, girderInputList,

                                    girderHbeamList, columnCenterTopSupport, columnCenterPipe,

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



            double testRecSize = 40 * scaleValue;
            double textHeight = 2.5 * scaleValue;

            Point3D recPoint = GetSumPoint(referencePoint, -testRecSize/2, -testRecSize/2);
            Line testLine01 = new Line(GetSumPoint(recPoint, 0, 0), GetSumPoint(recPoint, 0, testRecSize));
            Line testLine02 = new Line(GetSumPoint(recPoint, 0, testRecSize), GetSumPoint(recPoint, testRecSize, testRecSize));
            Line testLine03 = new Line(GetSumPoint(recPoint, testRecSize, testRecSize), GetSumPoint(recPoint, testRecSize,0));
            Line testLine04 = new Line(GetSumPoint(recPoint, testRecSize, 0), GetSumPoint(recPoint, 0, 0));


            Text text01 = new Text(GetSumPoint(referencePoint, 0, 0), selPaperModel.SubName.ToString(), textHeight);


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
            double roofSlope= valueService.GetDegreeOfSlope(assemblyData.RoofCompressionRing[0].RoofSlope);
            double bottomSlope = valueService.GetDegreeOfSlope(assemblyData.BottomInput[0].BottomPlateSlope);

            List<Entity> topAngleList = new List<Entity>();
            List<Entity> shellList = new List<Entity>();
            List<Entity> bottomList = new List<Entity>();
            List<Entity> roofList = new List<Entity>();

            List<Entity> centerLineList = new List<Entity>();


            List<Entity> etcList = new List<Entity>();

            double shellHeight = 0;
            double bottomCenterOpposite = 0;
            double roofCenterOpposite = 0;

            double shellHeightPercentage = 100;

            double shellSideHeight = 0;
            double centerSideHeight = 0;

            Point3D roofCenterLowerPoint = GetSumPoint(shellBottomPoint, 0, 0);
            Point3D roofCenterUpperPoint = GetSumPoint(roofCenterLowerPoint, 0, 0);
            Point3D roofSideLowerPoint= GetSumPoint(shellBottomPoint, 0, 0);
            Point3D roofSideUpperPoint= GetSumPoint(shellBottomPoint, 0, 0);


            Point3D bottomCenterLowerPoint = GetSumPoint(shellBottomPoint, 0, 0);
            Point3D bottomCenterUpperPoint = GetSumPoint(bottomCenterLowerPoint, 0, 0);
            Point3D bottomSideLowerPoint= GetSumPoint(shellBottomPoint, 0, 0);
            Point3D bottomSideUpperPoint = GetSumPoint(shellBottomPoint, 0, 0);

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




            // Lee Test

            #region 임시 데이터 배정
            string sizeName = "C200x80x7.5x11";
            NRafterSupportClipShellSideModel padModel = new NRafterSupportClipShellSideModel();
            foreach (NRafterSupportClipShellSideModel eachModel in assemblyData.NRafterSupportClipShellSideList)
            {
                if (eachModel.RafterSize == sizeName)
                {
                    padModel = eachModel;
                    break;
                }
            }
            #endregion

            #region 임시 데이터 배정 for Rafter
            string sizeName2 = "C250x90x9x13";
            NColumnRafterModel padModel2 = new NColumnRafterModel();
            foreach (NColumnRafterModel eachModel in assemblyData.NColumnRafterList)
            {
                if (eachModel.Size == sizeName2)
                {
                    padModel2 = eachModel;
                    break;
                }
            }
            #endregion


            #region 임시 데이터 배정 for rafter Support Clip Shell side
            string sizeName3 = "C200x80x7.5x11";
            NRafterSupportClipShellSideModel padModel3 = new NRafterSupportClipShellSideModel();
            foreach (NRafterSupportClipShellSideModel eachModel in assemblyData.NRafterSupportClipShellSideList)
            {
                if (eachModel.RafterSize == sizeName3)
                {
                    padModel3 = eachModel;
                    break;
                }
            }
            #endregion


            // Rafter
            etcList.AddRange(drawShareService.Rafter(GetSumPoint(referencePoint, 0, tankHeight), singleModel, scaleValue, padModel2).GetDrawEntity());


            // Clip
            etcList.AddRange(drawShareService.RafterSideClipDetail(GetSumPoint(referencePoint, 0, tankHeight), singleModel, scaleValue, padModel3).GetDrawEntity());


            // 
            // 1
            etcList.AddRange(drawComService.GetColumnCenterTopSupport_TopView(GetSumPoint(referencePoint, 0, 0), scaleValue));
            // 2
            etcList.AddRange(drawComService.GetColumnCenterTopSupport(GetSumPoint(referencePoint, 1000, 0), scaleValue));
            // 3



            

            //etcList.AddRange(drawShareService.Rafter(GetSumPoint(referencePoint, 0, tankHeight), scaleValue,));





            drawList.outlineList.AddRange(etcList);

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










            






            styleService.SetLayerListEntity(ref orientationList, layerService.LayerOutLine);
            drawList.outlineList.AddRange(orientationList);

            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            drawList.outlineList.AddRange(centerLineList);

            return drawList;



        }


        // 형상 정보 가져오기
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
