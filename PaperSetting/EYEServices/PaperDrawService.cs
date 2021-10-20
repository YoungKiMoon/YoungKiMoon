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
using System.Collections.ObjectModel;

using PaperSetting.Models;
using PaperSetting.Commons;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using DrawWork.ValueServices;
using DrawWork.DrawSacleServices;
using DrawWork.DrawModels;
using DrawWork.DrawStyleServices;
using AssemblyLib.AssemblyModels;
using DrawWork.Commons;
using DrawSettingLib.SettingServices;
using DrawSettingLib.SettingModels;
using DrawSettingLib.Commons;
using DrawWork.DrawGridServices;
using DrawWork.DrawServices;

namespace PaperSetting.EYEServices
{
    public class PaperDrawService
    {
        #region Service
        ValueService valueService;
        DrawScaleService scaleService;
        LayerStyleService layerService;
        TextStyleService textService;
        DrawEditingService editingService;
        StyleFunctionService styleService;

        DrawShapeService shapeService;

        private DrawService drawService;
        #endregion

        #region Property
        public AssemblyModel assemblyData;
        private Model singleModel = null;
        private Drawings singleDraw = null;

        private Dictionary<string, DockModel> oneSheetBlock = null;

        //public AssemblyModel assemblyData;

        public string selectionTitleBlock
        {
            get { return _selectionTitleBlock; }
            set { _selectionTitleBlock = value; }
        }
        public string _selectionTitleBlock;
        #endregion

        #region CONSTRUCTOR
        public PaperDrawService(AssemblyModel selAssembly, Model selModel, Drawings selDraw)
        {

            assemblyData = selAssembly;

            valueService = new ValueService();
            scaleService = new DrawScaleService();
            layerService = new LayerStyleService();
            textService = new TextStyleService();
            shapeService = new DrawShapeService();
            editingService = new DrawEditingService();
            styleService = new StyleFunctionService();

            drawService = new DrawService(null);

            singleModel = selModel;
            singleDraw = selDraw;
            oneSheetBlock = new Dictionary<string, DockModel>();

            selectionTitleBlock = "PAPER_TITLE_TYPE01";
        }
        #endregion


        public void DrawEntity()
        {
            // Sheet 
            singleDraw.Sheets.Clear();

            // Blcok
            for (int i = singleDraw.Blocks.Count; i > 0; i--)
            {
                bool removeSign = false;
                Block eachBlock = singleDraw.Blocks[i-1];
                if (eachBlock.Name.Contains("View"))
                    removeSign = true;
                else if (eachBlock.Name.Contains("PAPER_TABLE"))
                    removeSign = true;
                else if (eachBlock.Name.Contains("PAPER_NOTE"))
                    removeSign = true;
                // Delete
                if (removeSign)
                    singleDraw.Blocks.Remove(eachBlock);

            }
            singleDraw.Invalidate();
            singleDraw.Entities.Regen();
            singleDraw.Rebuild(singleModel);
        }


        public Sheet GetDWGSheet(string sheetName)
        {
            foreach(Sheet eachSheet in singleDraw.Sheets)
            {
                if (eachSheet.Name == sheetName)
                {
                    return eachSheet;
                }
            }

            return null;
        }

        #region Create : Paper : Grid : Draw
        public void CreatePaperGridDraw(ObservableCollection<DrawPaperGridModel> selGridList, ObservableCollection<PaperDwgModel> selPaperList)
        {

            bool gridOutlineVisible = true;
            DrawPaperGridService paperGridService = new DrawPaperGridService();

            // Scale
            double scaleValue = 1;

            foreach (PaperDwgModel eachPaper in selPaperList)
            {
                PAPERMAIN_TYPE eachDwgName = eachPaper.Name;
                double eachDwgPage = eachPaper.Page;

                DrawPaperGridModel eachPaperGrid = paperGridService.GetPaperGrid(selGridList, eachDwgName,eachDwgPage);

                if (eachPaperGrid != null)
                {
                    
                    // Grid Block
                    List<Entity> gridEntity = paperGridService.CreateGridEntity(eachPaperGrid, eachDwgName, eachDwgPage, singleDraw);
                    styleService.SetLayerListEntity(ref gridEntity, layerService.LayerDimension);

                    Sheet eachDWGSheet = GetDWGSheet(eachPaper.Basic.Title + eachPaper.Page);
                    eachDWGSheet.Entities.AddRange(gridEntity);

                    // ViewPort
                    foreach (PaperAreaModel eachArea in SingletonData.PaperArea.AreaList)
                    {
                        // 같은 DWG
                        if (eachArea.DWGName == eachDwgName)
                        {
                            if (eachArea.Page == eachDwgPage)
                            {

                                if (eachArea.visible)
                                {
                                    Console.WriteLine(eachArea.TitleName + eachArea.viewID );
                                    CreateVectorViewDetail(ref eachDWGSheet, eachArea, eachArea.ScaleValue);
                                }
                            }
                        }

                    }
                }

            }


            singleDraw.Entities.Regen();
            singleDraw.Rebuild(singleModel);
        }
        #endregion




        #region Create : Paper : Draw
        public void CreatePaperDraw(ObservableCollection<PaperDwgModel> selPaperCol, AssemblyModel assemblyData, bool selOneSheet = true)
        {


            // Scale
            double scaleValue = 1;
            PaperAreaService paperArea = new PaperAreaService();
            


            //for(int i = 0; i < selPaperCol.Count;i++)
            //{
            //    Sheet newSheet1 = new Sheet(linearUnitsType.Millimeters, 26, 26, "aa/" + i);
            //    singleDraw.Sheets.Add(newSheet1);
            //}

            //Sheet newSheet = null;

            int sheetIndex = 0;
            foreach (PaperDwgModel eachPaper in selPaperCol)
            {
                // Sheet
                oneSheetBlock = new Dictionary<string, DockModel>();

                //string sheetName = "GENERAL A" + sheetIndex.ToString();
                string sheetName = eachPaper.Basic.Title +eachPaper.Page;
                //newSheet.Name = eachPaper.Basic.Title;
                //newSheet = CreateSheet(eachPaper);
                Sheet newSheet = new Sheet(linearUnitsType.Millimeters, eachPaper.SheetSize.Width, eachPaper.SheetSize.Height, sheetName);
                //newSheet = new Sheet(linearUnitsType.Millimeters, 200, 200, "GENERAL ASSEMBLY(1/2)");

                //eachPaper.PaperSheet = newSheet;

                singleDraw.Sheets.Add(newSheet);


                // 완료 : 20210512
                CreateFrameBlock(eachPaper, newSheet);

                // 아직 미적용 : 20210512 적용 예정
                CreateTitleBlock(eachPaper, newSheet);

                // 일반 적용
                CreateRevisionBlock(eachPaper, newSheet);

                // 임시로 제외
                // foreach (PaperNoteModel eachModel in eachPaper.Notes)
                //     CreateNoteBlock(eachModel, newSheet,eachPaper.Basic.No);


                //-> 아래 쪽에 적용
                //foreach (PaperViewportModel eachView in eachPaper.ViewPorts)
                //{
                //    if (sheetIndex == 0)
                //        CreateVectorViewGA(newSheet, eachView.ViewPort, scaleValue, assemblyData);
                //    //CreateVectorView(newSheet, eachView, eachView.Name);
                //    else
                //        CreateVectorView2(newSheet, eachView, eachView.Name + sheetIndex.ToString() + eachView.No);
                //}


                // 아직 미적용
                //foreach (PaperTableModel eachModel in eachPaper.Tables)
                //    CreateTableBlock(eachModel,newSheet,eachPaper.Basic.No);
                //
                if (eachPaper.Basic.Title == "GENERAL ASSEMBLY(1-2)")
                {
                    scaleValue = paperArea.GetPaperScaleValue(PAPERMAIN_TYPE.GA1,PAPERSUB_TYPE.NotSet, SingletonData.PaperArea.AreaList);

                    // ViewPort 임시 컨트롤 
                    CreateVectorViewGA(newSheet, eachPaper.ViewPorts[0].ViewPort, scaleValue, assemblyData);

                    CreateTableBlockGADesign(eachPaper.Tables[0], newSheet, eachPaper.Tables[0].No, assemblyData);
                    CreateTableBlockGA(eachPaper.Tables[1], newSheet, eachPaper.Tables[1].No, assemblyData);
                    CreateTableBlockGA2(eachPaper.Tables[2], newSheet, eachPaper.Tables[2].No, assemblyData);
                    CreateTableBlockGANozzleProjection(eachPaper.Tables[3], newSheet, eachPaper.Tables[3].No, assemblyData);
                    if(assemblyData.BottomInput[0].DripRing.ToLower()=="yes")
                        CreateTableBlockGADripRing(eachPaper.Tables[4], newSheet, eachPaper.Tables[4].No, assemblyData);
                }

                if (eachPaper.Basic.Title == "GENERAL ASSEMBLY(2-2)")
                {
                    CreateNoteBlockGA(eachPaper.Notes[0], newSheet, eachPaper.Notes[0].No, assemblyData);
                }

                if (eachPaper.Basic.Title == "NOZZLE ORIENTATION")
                {
                    scaleValue = paperArea.GetPaperScaleValue(PAPERMAIN_TYPE.ORIENTATION,PAPERSUB_TYPE.NotSet, SingletonData.PaperArea.AreaList);
                    // ViewPort 임시 컨트롤 
                    CreateVectorViewOrientation(newSheet, eachPaper.ViewPorts[0].ViewPort, scaleValue, assemblyData);

                    CreateTableBlockGA(eachPaper.Tables[0], newSheet, eachPaper.Tables[0].No, assemblyData);
                    CreateTableBlockGA2(eachPaper.Tables[1], newSheet, eachPaper.Tables[1].No, assemblyData);
                    CreateTableBlockGANozzleProjection(eachPaper.Tables[2], newSheet, eachPaper.Tables[2].No, assemblyData);
                    CreateTableBlockDirection(eachPaper.Tables[3], newSheet, eachPaper.Tables[3].No, assemblyData);
                    CreateTableBlockDia(eachPaper.Tables[4], newSheet, eachPaper.Tables[4].No, assemblyData);



                }

                if (eachPaper.Name == PAPERMAIN_TYPE.ShellPlateArrangement)
                {

                    if(eachPaper.Tables.Count>0)
                        CreateTableBlockShellPlate(eachPaper.Tables[0], newSheet, eachPaper.Tables[0].No,PAPERMAIN_TYPE.ShellPlateArrangement, assemblyData);
                }

                if (eachPaper.Name == PAPERMAIN_TYPE.BottomPlateCuttingPlan)
                {

                    if (eachPaper.Tables.Count > 0)
                        CreateTableBlockShellPlate(eachPaper.Tables[0], newSheet, eachPaper.Tables[0].No, PAPERMAIN_TYPE.BottomPlateCuttingPlan, assemblyData);
                }
                if (eachPaper.Name == PAPERMAIN_TYPE.RoofPlateCuttingPlan)
                {

                    if (eachPaper.Tables.Count > 0)
                        CreateTableBlockShellPlate(eachPaper.Tables[0], newSheet, eachPaper.Tables[0].No, PAPERMAIN_TYPE.RoofPlateCuttingPlan, assemblyData);
                }


                //singleModel.UpdateBoundingBox();
                //singleDraw.Invalidate();
                // 중요한지 테스트 필요함
                singleDraw.ActiveSheet = singleDraw.Sheets[sheetIndex];
                AutomaticAlignmentOfDock(sheetIndex);
                singleDraw.Invalidate();
                sheetIndex++;

                Console.WriteLine(eachPaper.Name.ToString() + ":" + eachPaper.Page);
            }


            //if (!singleDraw.ActiveSheet.Name.Equals(firstSheet.Name))
            singleDraw.ActiveSheet = singleDraw.Sheets[0];


            singleDraw.ZoomFit();
            //singleDraw.Invalidate();
            singleDraw.ActionMode = actionType.SelectByPick;


            //AutomaticAlignmentOfDock();

            //SolidColorBrush newColor = Brushes.Red;
            //singleDraw.Background.IntermediateColor = newColor;
            //singleDraw.Invalidate();
            singleDraw.Entities.Regen();
            singleDraw.Rebuild(singleModel);


        }


        // Detail View
        public void CreateVectorViewDetail(ref Sheet selSheet, PaperAreaModel selPaper, double scaleValue)
        {

            double extensionAmount = Math.Min(selSheet.Width, selSheet.Height) / 594;

            Point3D targetPoint = new Point3D();
            targetPoint.X = selPaper.ModelCenterLocation.X;
            targetPoint.Y = selPaper.ModelCenterLocation.Y;


            scaleValue = selPaper.ScaleValue;

            string nickName01 = "newViewDetail" + selPaper.viewID;
            string nickName02 = "DetailPlaceHolder" + selPaper.viewID;


            if (selPaper.visible)
            {
                Camera newnewCamera = new Camera(targetPoint, 0, Viewport.GetCameraRotation(viewType.Top), projectionType.Orthographic, 0, 1);
                VectorView newnewView = new VectorView(selPaper.Location.X + selPaper.Size.X / 2,
                                                    selPaper.Location.Y - selPaper.Size.Y / 2,
                                                    newnewCamera,
                                                    scaleService.GetViewScale(scaleValue),
                                                    nickName01,
                                                    selPaper.Size.X,
                                                    selPaper.Size.Y
                                                    );

                newnewView.CenterlinesExtensionAmount = extensionAmount;
                newnewView.KeepEntityColor = true;
                newnewView.LayerName = layerService.LayerViewport;

                selSheet.AddViewPlaceHolder(newnewView, singleModel, singleDraw, nickName02);
            }
            else
            {
                Console.WriteLine("ViewPort : Visible : False : " + selPaper.SubName.ToString());
            }


        }



        public void CreateVectorViewGA(Sheet selSheet, ViewPortModel selViewPort,double scaleValue,AssemblyModel assemData)
        {

            bool first = false;
            //singleDraw.Sheets.Clear();
            //singleDraw.Entities.Clear();
            //singleDraw.Blocks.Remove("newView");
            //singleDraw.Blocks.Remove("DRAW_BLOCK");
            //if (singleDraw.Sheets.Count == 0)
            //{
            //    CreateSheet();
            //    first = true;
            //}
            first = true;



            if (first)
            {


                //double sizeWidth = valueService.GetDoubleValue(assemData.GeneralDesignData[0].SizeNominalID);
                //double sizeHeight = valueService.GetDoubleValue(assemData.GeneralDesignData[0].SizeTankHeight);
                //sizeHeight = 10000 + sizeHeight / 2;
                //sizeWidth = 10000 + sizeWidth / 2;

                double extensionAmount = Math.Min(selSheet.Width, selSheet.Height) / 594;
                //bool visibleValue = false;
                //if (visibleValue)
                //{
                //    Camera newCamera = new Camera();

                //    newCamera.FocalLength = 0;
                //    newCamera.ProjectionMode = projectionType.Orthographic;

                //    newCamera.Rotation.X = 0.5;
                //    newCamera.Rotation.Y = 0.5;
                //    newCamera.Rotation.Z = 0.5;
                //    newCamera.Rotation.W = 0.5;

                //    // 표적 중앙
                //    //newCamera.Target.X = valueService.GetDoubleValue(selViewPort.TargetX);
                //    //newCamera.Target.Y = valueService.GetDoubleValue(selViewPort.TargetY);
                //    newCamera.Target.Z = 0;


                //    newCamera.Target.X = sizeWidth;
                //    newCamera.Target.Y = sizeHeight;








                //    VectorView newView = new VectorView(10000,
                //                                        10000,
                //                                        newCamera,
                //                                        scaleValue,
                //                                        "newView",
                //                                        valueService.GetDoubleValue(selViewPort.SizeX),
                //                                        valueService.GetDoubleValue(selViewPort.SizeY));

                //    newView.CenterlinesExtensionAmount = extensionAmount;

                //}


                Point3D targetPoint = new Point3D();
                //targetPoint.X = SingletonData.GAViewPortCenter.X + SingletonData.GAViewPortSize.X / 2;
                //targetPoint.Y = SingletonData.GAViewPortCenter.Y + SingletonData.GAViewPortSize.Y / 2;

                targetPoint.X = SingletonData.GAArea.ViewCenterPoint.X;
                targetPoint.Y = SingletonData.GAArea.ViewCenterPoint.Y;


                Camera newnewCamera = new Camera(targetPoint,0,Viewport.GetCameraRotation(viewType.Top),projectionType.Orthographic,0,1);
                //VectorView newnewView = new VectorView(valueService.GetDoubleValue(selViewPort.LocationX),
                //                                    valueService.GetDoubleValue(selViewPort.LocationY),
                //                                    newnewCamera,
                //                                    scaleService.GetViewScale(scaleValue),
                //                                    "newView",
                //                                    valueService.GetDoubleValue(selViewPort.SizeX),
                //                                    valueService.GetDoubleValue(selViewPort.SizeY)
                //                                    );

                PaperAreaModel GAPaper= SingletonData.PaperArea.AreaList[0];

                VectorView newnewView = new VectorView(GAPaper.Location.X,
                                                       GAPaper.Location.Y,
                                                        newnewCamera,
                                    scaleService.GetViewScale(scaleValue),
                                    "newView",
                                    GAPaper.Size.X,
                                    GAPaper.Size.Y
                                    );

                newnewView.CenterlinesExtensionAmount = extensionAmount;


                newnewView.KeepEntityColor = true;


                newnewView.LayerName = layerService.LayerViewport;

                //selSheet.AddViewPlaceHolder(newRaster, singleModel, singleDraw, "GAPlaceHolder");
                selSheet.AddViewPlaceHolder(newnewView, singleModel, singleDraw, "GAPlaceHolder");

                //selCount++;
                //selSheet.Entities.Clear();

            }
            else
            {
                Sheet newSheet = singleDraw.Sheets[0];
                VectorView newView = newSheet.Entities[0] as VectorView;
                newView.Camera.Target.X = valueService.GetDoubleValue(selViewPort.TargetX);
                newView.Camera.Target.Y = valueService.GetDoubleValue(selViewPort.TargetY);
                newView.Scale = valueService.GetDoubleValue(selViewPort.ScaleStr);
                //newView.X= valueService.GetDoubleValue(selViewPort.LocationX);
                //newView.Y = valueService.GetDoubleValue(selViewPort.LocationY);
                //newView.Window.Width= Convert.ToSingle( valueService.GetDoubleValue(selViewPort.SizeX));
                //newView.Width = valueService.GetDoubleValue(selViewPort.SizeX);

            }



            ///CDPoint newPoint1 = new CDPoint();
            //double scaleFactor = 1;
            //string blockName = "DRAW_BLOCK";
            //Block newBlock = drawImportBlockService.GetImportBlock(blockName);
            //singleDraw.Blocks.Add(newBlock);

            //BlockReference returnBlock = new BlockReference(0, 0, 0, blockName.ToUpper(), 0);
            //returnBlock.Scale(scaleFactor);

            //returnBlock.LayerName = "LayerPaper";



            //singleDraw.Sheets[0].Entities.Add(returnBlock);

            //ViewBuilder _viewBuilder = new ViewBuilder(singleModel, singleDraw, true, ViewBuilder.operatingType.Queue);
            //_viewBuilder.AddToQueue(singleDraw, true, selSheet);
            //singleDraw.StartWork(_viewBuilder);
            singleDraw.Entities.Regen();


            //singleDraw.UpdateBoundingBox();
            //singleDraw.Invalidate();


            //singleDraw.Rebuild(singleModel);
            //singleDraw.ActiveSheet = singleDraw.Sheets[0];
            //singleDraw.Invalidate();


            //singleDraw.ActiveSheet = selSheet;

            //singleDraw.ZoomFit();
            //singleDraw.ActionMode = actionType.SelectByPick;
            //singleDraw.Invalidate();





        }

        public void CreateVectorViewOrientation(Sheet selSheet, ViewPortModel selViewPort, double scaleValue, AssemblyModel assemData)
        {

            bool first = false;
            
            first = true;



            if (first)
            {


                
                double extensionAmount = Math.Min(selSheet.Width, selSheet.Height) / 594;


                PaperAreaModel orientationPaper = SingletonData.PaperArea.AreaList[1];

                Point3D targetPoint = new Point3D();
                targetPoint.X = orientationPaper.ModelCenterLocation.X;
                targetPoint.Y = orientationPaper.ModelCenterLocation.Y;





                Camera newnewCamera = new Camera(targetPoint, 0, Viewport.GetCameraRotation(viewType.Top), projectionType.Orthographic, 0, 1);
                VectorView newnewView = new VectorView(orientationPaper.Location.X,
                                                    orientationPaper.Location.Y,
                                                    newnewCamera,
                                                    scaleService.GetViewScale(scaleValue),
                                                    "newView2",
                                                    orientationPaper.Size.X,
                                                    orientationPaper.Size.Y
                                                    );

                newnewView.CenterlinesExtensionAmount = extensionAmount;


                newnewView.KeepEntityColor = true;


                newnewView.LayerName = layerService.LayerViewport;

                //selSheet.AddViewPlaceHolder(newRaster, singleModel, singleDraw, "GAPlaceHolder");
                selSheet.AddViewPlaceHolder(newnewView, singleModel, singleDraw, "OrientationPlaceHolder");

                //selCount++;
                //selSheet.Entities.Clear();

            }

            // Nozzle Title
            // Title
            Point3D referencePoint = new Point3D(280, 40);
            List<Entity> titleList = new List<Entity>();
            Point3D titleBaseCenter1 = GetSumPoint(referencePoint, 0, 7);
            Point3D titleBaseCenter2 = GetSumPoint(referencePoint, 0, 8);
            Point3D titleTextCenter = GetSumPoint(referencePoint, 0, 9);

            double baseLineWidth = 65;
            Line titleBaseLine1 = new Line(GetSumPoint(titleBaseCenter1, -baseLineWidth / 2, 0), GetSumPoint(titleBaseCenter1, baseLineWidth / 2, 0));
            styleService.SetLayer(ref titleBaseLine1, layerService.LayerOutLine);
            Line titleBaseLine2 = new Line(GetSumPoint(titleBaseCenter2, -baseLineWidth / 2, 0), GetSumPoint(titleBaseCenter2, baseLineWidth / 2, 0));
            styleService.SetLayer(ref titleBaseLine2, layerService.LayerDimension);
            Text titleText = new Text(titleTextCenter, "NOZZLE PROJECTION", 4);
            titleText.Alignment = Text.alignmentType.BaselineCenter;
            titleText.ColorMethod = colorMethodType.byEntity;
            titleText.Color = Color.Yellow;
            titleList.AddRange(new Entity[] { titleBaseLine1, titleBaseLine2, titleText });

            selSheet.Entities.AddRange(titleList);


            singleDraw.Entities.Regen();

        }

        private Sheet CreateSheet(PaperDwgModel selPaper)
        {
            Sheet newSheet = new Sheet(linearUnitsType.Millimeters, selPaper.SheetSize.Width, selPaper.SheetSize.Height, selPaper.Basic.Title.ToString());
            singleDraw.Sheets.Add(newSheet);

            return newSheet;
        }
        private void CreateFrameBlock(PaperDwgModel selPaper, Sheet selSheet)
        {
            string sheetFrameName = "";
            switch (selPaper.SheetSize.PaperType)
            {
                case PAPERFORMAT_TYPE.A1_ISO:
                    sheetFrameName = "PAPER_FRAME_A1";
                    break;
                case PAPERFORMAT_TYPE.A2_ISO:
                    sheetFrameName = "PAPER_FRAME_A2";
                    break;

            }
            BlockReference newBr = new BlockReference(0, 0, 0, sheetFrameName, 1, 1, 1, 0);
            newBr.LayerName = layerService.LayerPaper;
            selSheet.Entities.Add(newBr);
        }
        private void CreateTitleBlock(PaperDwgModel selPaper, Sheet selSheet)
        {
            //PAPER_TITLE_TYPE01
            BlockReference newBr = new BlockReference(selPaper.SheetSize.Width - 166 - 7, 7, 0, "PAPER_TITLE_TYPE01", 1, 1, 1, 0);
            newBr.Attributes["OWNER"] = new AttributeReference("SAMSUNG ENGINEERING");
            newBr.Attributes["PROJECT"] = new AttributeReference("TAnk Basic Automation System");
            newBr.Attributes["TITLE"] = new AttributeReference(selPaper.Basic.Title);
            newBr.Attributes["DWG. NO."] = new AttributeReference(selPaper.Basic.DwgNo);

            newBr.LayerName = layerService.LayerPaper;
            // Add : Just One
            selSheet.Entities.Add(newBr);
        }
        private void CreateRevisionBlock(PaperDwgModel selPaper, Sheet selSheet)
        {
            List<Entity> newList = BuildPaperRevision(selPaper, selectionTitleBlock);

            foreach (Entity eachEntity in newList)
                eachEntity.LayerName = layerService.LayerPaper;

            selSheet.Entities.AddRange(newList);
            //selSheet.Entities.Add(new Line(0, 0, 200, 200) { ColorMethod = colorMethodType.byEntity, Color = Color.Red });
        }


        private void CreateNoteBlock(PaperNoteModel selNote, Sheet selSheet, string bName)
        {
            BlockReference newBr = BuildPaperNote(selNote, out Block noteBlock);
            noteBlock.Name += bName;
            newBr.BlockName += noteBlock.Name;


            singleDraw.Blocks.Add(noteBlock);
            selSheet.Entities.Add(newBr);
            oneSheetBlock.Add(newBr.BlockName, selNote.Dock);
        }
        private void CreateNoteBlockGA(PaperNoteModel selNote, Sheet selSheet, string bName, AssemblyModel assemblyData)
        {
            BlockReference newBr = BuildPaperNoteGA(selNote, assemblyData, out Block noteBlock);

            newBr.LayerName = layerService.LayerBlock;

            noteBlock.Name += bName;
            newBr.BlockName = noteBlock.Name;

            if (selNote.Dock.DockPosition == DOCKPOSITION_TYPE.FLOATING)
            {
                newBr.InsertionPoint.X = selNote.Location.X;
                newBr.InsertionPoint.Y = selNote.Location.Y;
            }

            singleDraw.Blocks.Add(noteBlock);
            selSheet.Entities.Add(newBr);
            oneSheetBlock.Add(newBr.BlockName, selNote.Dock);
        }
        private void CreateTableBlock(PaperTableModel selTable, Sheet selSheet, string bName)
        {
            BlockReference newBr = BuildPaperTable(selTable, out Block tableBlock);
            newBr.BlockName += bName;
            tableBlock.Name += bName;

            singleDraw.Blocks.Add(tableBlock);
            selSheet.Entities.Add(newBr);
            oneSheetBlock.Add(newBr.BlockName, selTable.Dock);
        }

        private void CreateTableBlockGA(PaperTableModel selTable, Sheet selSheet, string bName, AssemblyModel assemblyData)
        {
            BlockReference newBr = BuildPaperTableGA(selTable, assemblyData, out Block tableBlock);

            newBr.LayerName = layerService.LayerBlock;

            tableBlock.Name += bName + "_" + singleDraw.Blocks.Count;
            newBr.BlockName = tableBlock.Name;

            singleDraw.Blocks.Add(tableBlock);
            selSheet.Entities.Add(newBr);
            oneSheetBlock.Add(newBr.BlockName, selTable.Dock);
        }
        private void CreateTableBlockGA2(PaperTableModel selTable, Sheet selSheet, string bName, AssemblyModel assemblyData)
        {
            BlockReference newBr = BuildPaperTableGA2(selTable, assemblyData, out Block tableBlock);
            newBr.LayerName = layerService.LayerBlock;

            tableBlock.Name += bName + "_" + singleDraw.Blocks.Count;
            newBr.BlockName = tableBlock.Name;

            singleDraw.Blocks.Add(tableBlock);
            selSheet.Entities.Add(newBr);
            oneSheetBlock.Add(newBr.BlockName, selTable.Dock);
        }
        private void CreateTableBlockGANozzleProjection(PaperTableModel selTable, Sheet selSheet, string bName, AssemblyModel assemblyData)
        {
            BlockReference newBr = BuildPaperTableNozzleProjection(selTable, assemblyData, out Block tableBlock);
            newBr.LayerName = layerService.LayerBlock;

            tableBlock.Name += bName + "_" + singleDraw.Blocks.Count;
            newBr.BlockName = tableBlock.Name;

            singleDraw.Blocks.Add(tableBlock);
            selSheet.Entities.Add(newBr);
            oneSheetBlock.Add(newBr.BlockName, selTable.Dock);
        }

        private void CreateTableBlockGADripRing(PaperTableModel selTable, Sheet selSheet, string bName, AssemblyModel assemblyData)
        {
            BlockReference newBr = BuildPaperTableDripRing(selTable, assemblyData, out Block tableBlock);
            newBr.LayerName = layerService.LayerBlock;

            tableBlock.Name += bName + "_" + singleDraw.Blocks.Count;
            newBr.BlockName = tableBlock.Name;

            singleDraw.Blocks.Add(tableBlock);
            selSheet.Entities.Add(newBr);
            oneSheetBlock.Add(newBr.BlockName, selTable.Dock);
        }

        private void CreateTableBlockDirection(PaperTableModel selTable, Sheet selSheet, string bName, AssemblyModel assemblyData)
        {
            BlockReference newBr = BuildPaperTableDirection(selTable, assemblyData, out Block tableBlock);
            newBr.LayerName = layerService.LayerBlock;

            tableBlock.Name += bName + "_" + singleDraw.Blocks.Count;
            newBr.BlockName = tableBlock.Name;
            //newBr.InsertionPoint.X = 50;
            //newBr.InsertionPoint.Y = 594 - 50;

            singleDraw.Blocks.Add(tableBlock);
            newBr.Regen(new RegenParams(0, singleDraw));
            newBr.InsertionPoint = new Point3D(50, 594 - 50 - newBr.BoxSize.Y);
            selSheet.Entities.Add(newBr);
            oneSheetBlock.Add(newBr.BlockName, selTable.Dock);
        }

        private void CreateTableBlockDia(PaperTableModel selTable, Sheet selSheet, string bName, AssemblyModel assemblyData)
        {
            BlockReference newBr = BuildPaperTableDia(selTable, assemblyData, out Block tableBlock);
            newBr.LayerName = layerService.LayerBlock;

            tableBlock.Name += bName + "_" + singleDraw.Blocks.Count;
            newBr.BlockName = tableBlock.Name;
            //newBr.InsertionPoint.X = 50;
            //newBr.InsertionPoint.Y = 594 - 50;

            singleDraw.Blocks.Add(tableBlock);
            //newBr.Regen(new RegenParams(0, singleDraw));
            //newBr.InsertionPoint = new Point3D(50, 594 - 50 - newBr.BoxSize.Y);
            selSheet.Entities.Add(newBr);
            oneSheetBlock.Add(newBr.BlockName, selTable.Dock);
        }

        

        private void CreateTableBlockGADesign(PaperTableModel selTable, Sheet selSheet, string bName, AssemblyModel assemblyData)
        {
            BlockReference newBr = BuildPaperTableGADesign(selTable, assemblyData, out Block tableBlock);

            newBr.LayerName= layerService.LayerBlock;

            tableBlock.Name += bName;
            newBr.BlockName = tableBlock.Name;

            singleDraw.Blocks.Add(tableBlock);
            selSheet.Entities.Add(newBr);
            oneSheetBlock.Add(newBr.BlockName, selTable.Dock);
        }


        private void CreateTableBlockShellPlate(PaperTableModel selTable, Sheet selSheet, string bName, PAPERMAIN_TYPE selDwgName, AssemblyModel assemblyData)
        {
            BlockReference newBr = BuildPaperTableShellPlateBM(selTable, assemblyData, selDwgName, out Block tableBlock);
            newBr.LayerName = layerService.LayerBlock;

            tableBlock.Name += bName + "_" + singleDraw.Blocks.Count;
            newBr.BlockName = tableBlock.Name;

            singleDraw.Blocks.Add(tableBlock);
            selSheet.Entities.Add(newBr);
            oneSheetBlock.Add(newBr.BlockName, selTable.Dock);
        }



















        private void CreateVectorView(Sheet selSheet, PaperViewportModel selView, string viewName)
        {
            //VectorView ve2 = new VectorView(10, 10, cCa, 0.001, "View2", printRect2);
            //VectorView ve2 = new VectorView(20, 0, viewType.Top, 0.001, "View2");
            //Camera cc = ve2.Camera;

            //dd.ZoomFactor
            //dd.Distance = 0.000001;
            //dd.Location.X = 0;
            //dd.Location.Y = 0;
            //dd.Location.Z = 0;

            ViewPortModel selViewPort = selView.ViewPort;

            Camera newCamera = new Camera();

            newCamera.FocalLength = 0;
            newCamera.ProjectionMode = projectionType.Orthographic;

            newCamera.Rotation.X = 0.5;
            newCamera.Rotation.Y = 0.5;
            newCamera.Rotation.Z = 0.5;
            newCamera.Rotation.W = 0.5;

            newCamera.Target.X = 80; // 표적
            newCamera.Target.Y = 60; // 표적
            newCamera.Target.Z = 0;

            double extensionAmount = Math.Min(selSheet.Width, selSheet.Height) / 400;

            VectorView newView = new VectorView(selViewPort.Location.X,
                                                selViewPort.Location.Y,
                                                newCamera,
                                                0.001,
                                                viewName,
                                                selViewPort.Size.Width,
                                                selViewPort.Size.Height) { CenterlinesExtensionAmount = extensionAmount };// 형상 높이 폭

            selSheet.AddViewPlaceHolder(newView, singleModel, singleDraw, viewName + "PlaceHolder");

            oneSheetBlock.Add(newView.BlockName, selView.Dock);
        }
        private void CreateVectorView2(Sheet selSheet, PaperViewportModel selView, string viewName)
        {
            //VectorView ve2 = new VectorView(10, 10, cCa, 0.001, "View2", printRect2);
            //VectorView ve2 = new VectorView(20, 0, viewType.Top, 0.001, "View2");
            //Camera cc = ve2.Camera;

            //dd.ZoomFactor
            //dd.Distance = 0.000001;
            //dd.Location.X = 0;
            //dd.Location.Y = 0;
            //dd.Location.Z = 0;

            ViewPortModel selViewPort = selView.ViewPort;

            Camera newCamera = new Camera();

            newCamera.FocalLength = 0;
            newCamera.ProjectionMode = projectionType.Orthographic;

            newCamera.Rotation.X = 0.5;
            newCamera.Rotation.Y = 0.5;
            newCamera.Rotation.Z = 0.5;
            newCamera.Rotation.W = 0.5;

            newCamera.Target.X = 190; // 표적
            newCamera.Target.Y = 10; // 표적
            newCamera.Target.Z = 0;

            double extensionAmount = Math.Min(selSheet.Width, selSheet.Height) / 400;

            VectorView newView = new VectorView(selViewPort.Location.X,
                                                selViewPort.Location.Y,
                                                newCamera,
                                                0.001,
                                                viewName,
                                                selViewPort.Size.Width,
                                                selViewPort.Size.Height)
            { CenterlinesExtensionAmount = extensionAmount };// 형상 높이 폭

            selSheet.AddViewPlaceHolder(newView, singleModel, singleDraw, viewName + "PlaceHolder");

            oneSheetBlock.Add(newView.BlockName, selView.Dock);
        }

        #endregion

        #region Build Block

        // 2021-05-12 완료

        private List<Entity> BuildPaperRevision(PaperDwgModel selPaper, string selTitleName)
        {
            List<Entity> newList = new List<Entity>();
            List<Entity> newTextList = new List<Entity>();

            if (selPaper.Revisions.Count == 0)
                return newList;



            double[] columnWidths = new double[] { 8, 78, 16, 16, 16, 16, 16 };
            double[] columnTextHeight = new double[] { 0.85, 0.85, 0.9, 0.9, 0.9, 0.9, 0.9 };
            string[] rowHeaderText = new string[] { "REV", "DESCRIPTION", "DATE", "DRWN", "CHKD", "REVD", "APVD" };


            Color textYellow = Color.FromArgb(255, 255, 0);// 노란

            int rowCount = selPaper.Revisions.Count;
            double rowHeight = 7;
            double rowHeaderHeight = 10;
            double blockHeight = rowHeight * (rowCount + 1); // include Header Row

            List<double> rowList = new List<double>();
            rowList.Add(rowHeaderHeight);
            for (int i = 0; i < rowCount; i++)
                rowList.Add(rowHeight);

            //매우매우 중요한 Type 에 따라서 사이즈 적용 해야 함
            Block titleBlock = singleDraw.Blocks[selTitleName];
            //double pointX = selPaper.SheetSize.Width - 7 - titleBlock.Entities.BoxSize.X;
            double pointX = selPaper.SheetSize.Width - 7 - 166;
            double pointXX = selPaper.SheetSize.Width - 7;
            //double pointY = titleBlock.Entities.BoxSize.Y+7;
            double pointY = 137 + 7;

            // Row
            double rowHeightSum = pointY;
            for (int i = 0; i < rowList.Count; i++)
            {
                double columnSumWidth = pointX;
                string[] newRevisionStr;
                if (i == 0)
                    newRevisionStr = rowHeaderText;
                else
                {
                    PaperRevisionModel eachRev = selPaper.Revisions[i - 1];
                    newRevisionStr = new string[] { eachRev.RevString, eachRev.Description, eachRev.DateString, eachRev.DRWN, eachRev.CHKD, eachRev.REVD, eachRev.APVD };
                }
                for (int j = 0; j < columnWidths.Length; j++)
                {
                    newTextList.Add(new Text(columnSumWidth + columnWidths[j] / 2, rowHeightSum + rowList[i] / 2, newRevisionStr[j], 2.5) { Alignment = Text.alignmentType.MiddleCenter, WidthFactor = columnTextHeight[j], StyleName = textService.TextROMANS });
                    columnSumWidth += columnWidths[j];
                }

                rowHeightSum += rowList[i];
                newList.Add(new Line(pointX, rowHeightSum, pointXX, rowHeightSum));

            }

            // Column
            double columnWidthSum = pointX;
            for (int i = 0; i < columnWidths.Length; i++)
            {
                newList.Add(new Line(columnWidthSum, pointY, columnWidthSum, rowHeightSum));
                columnWidthSum += columnWidths[i];
            }

            // Line
            foreach (Entity eachEntity in newList)
            {
                eachEntity.LayerName = layerService.LayerPaper;
                eachEntity.LineTypeMethod = colorMethodType.byLayer;
                eachEntity.ColorMethod = colorMethodType.byLayer;
            }

            foreach (Entity eachEntity in newTextList)
            {
                eachEntity.LayerName = layerService.LayerPaper;
                eachEntity.ColorMethod = colorMethodType.byEntity;
                eachEntity.Color = textYellow;

                newList.Add(eachEntity);
            }

            return newList;

        }
        private BlockReference BuildPaperNote(PaperNoteModel selNote, out Block selBlock)
        {

            string[] noteArray = selNote.Note.Split('\n');
            int lineCount = noteArray.Length;
            double fontHeight = 2;
            double lineSpacing = 0.6;
            double blockPadding = 1;
            double textPositionY = 0;
            //double textPositionY = (lineSpacing + fontHeight) * (lineCount - 1);




            Block newBl = new Block("PAPER_NOTE" + selNote.No);



            for (int i = lineCount - 1; i >= 0; i--)
            {
                Text newText = new Text(0, textPositionY, 0, noteArray[i], fontHeight);
                newBl.Entities.Add(newText);
                textPositionY += (lineSpacing + fontHeight);
            }

            LinearPath rectBox = new LinearPath(140, textPositionY);
            rectBox.ColorMethod = colorMethodType.byEntity;
            rectBox.Color = Color.Green;
            newBl.Entities.Add(rectBox);

            BlockReference newBR = new BlockReference(selNote.Location.X, selNote.Location.Y, 0, "PAPER_NOTE" + selNote.No, 1, 1, 1, 0);

            selBlock = newBl;
            return newBR;
        }
        private BlockReference BuildPaperNoteGA(PaperNoteModel selNote, AssemblyModel assemblyData, out Block selBlock)
        {

            double fontHeight = 3.5;
            double fontHeight2 = 2;
            double fontHeightTitle = 5;
            double fontHeightTitleSub = 3;

            double leftTextGap = 1;
            double lineGap = 1;
            double testLineHeight = 7;

            string[] noteArray = selNote.Note.Split('\n');
            int lineCount = noteArray.Length;
            double lineSpacing = 0.6;
            double blockPadding = 1;
            double textPositionY = 0;
            //double textPositionY = (lineSpacing + fontHeight) * (lineCount - 1);



            Point3D refPoint = new Point3D(0, 0);
            List<Entity> newList = new List<Entity>();

            double currentY = 0;
            bool runSign = false;
            foreach (NotesCRTInputModel eachNote in assemblyData.NotesCRTInput)
            {
                if (runSign == true)
                    if (eachNote.PaperName != "")
                        runSign = false;

                if (eachNote.PaperName == "GENERAL ASSEMBLY")
                {
                    runSign = true;
                    // Title
                    Text newTitle = GetNewTextYellow(GetSumPoint(refPoint, leftTextGap, currentY), eachNote.NoteName, fontHeightTitle, 1, Text.alignmentType.BaselineLeft);
                    newTitle.Regen(new RegenParams(0, singleDraw));
                    double titleWidth = newTitle.BoxSize.X;
                    double titleLineWidth = titleWidth + 1 + 1;
                    newList.Add(newTitle);

                    currentY += -lineGap;
                    newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, titleLineWidth, currentY)));
                    currentY += -lineGap;
                    newList.Add(GetNewLineYellow(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, titleLineWidth, currentY)));
                    currentY += -7;
                }

                if (runSign)
                {

                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY), eachNote.Description, fontHeight, 1, Text.alignmentType.BaselineLeft));
                    currentY += -testLineHeight;
                }

            }



            BlockReference newBR = new BlockReference(selNote.Location.X, selNote.Location.Y, 0, "PAPER_NOTE" + selNote.No, 1, 1, 1, 0);

            Block newBl = new Block("PAPER_NOTE" + selNote.No);
            newBl.Entities.AddRange(newList);
            selBlock = newBl;
            return newBR;


        }
        private BlockReference BuildPaperTable(PaperTableModel selTable, out Block selBlock)
        {
            int rowCount = selTable.TableList.Count;
            int columnCount = selTable.TableList[0].Length;

            double fontHeight = 2;
            double rowHeight = 4;
            double blockHeight = rowHeight * rowCount;
            double blockWidth = 140;

            blockWidth = selTable.Size.Width;

            Block newBl = new Block("PAPER_TABLE" + selTable.No);

            LinearPath rectBox = new LinearPath(blockWidth, blockHeight);
            rectBox.ColorMethod = colorMethodType.byEntity;
            rectBox.Color = Color.Green;
            newBl.Entities.Add(rectBox);

            // Column
            double columnWidthSum = 0;
            double columnOneWidth = blockWidth / columnCount;
            for (int i = 0; i < columnCount - 1; i++)
            {
                columnWidthSum += columnOneWidth;
                Line colLine = new Line(columnWidthSum, 0, columnWidthSum, blockHeight);
                colLine.LineWeight = 0.15f;
                colLine.LineWeightMethod = colorMethodType.byEntity;
                newBl.Entities.Add(colLine);
            }
            // Row
            double rowHeightSum = 0;
            for (int i = 0; i < rowCount; i++)
            {
                rowHeightSum += rowHeight;
                Line rowLine = new Line(0, rowHeightSum, blockWidth, rowHeightSum);
                rowLine.LineWeight = 0.15f;
                rowLine.LineWeightMethod = colorMethodType.byEntity;
                newBl.Entities.Add(rowLine);
            }
            // Text : 
            double textHeightSum = 0;
            foreach (string[] eachRow in Enumerable.Reverse(selTable.TableList))
            {
                string[] rowText = eachRow;

                // Row : Center
                double textHeightCenter = (rowHeight - fontHeight) / 2;
                textHeightSum += textHeightCenter;

                // Column : Center
                double textWidthSum = 0;
                for (int i = 0; i < columnCount; i++)
                {
                    double textWidthCenter = columnOneWidth / 2;
                    textWidthSum += textWidthCenter;

                    Text newText = new Text(textWidthSum, textHeightSum, 0, rowText[i], fontHeight);
                    newText.Alignment = Text.alignmentType.BaselineCenter;
                    newBl.Entities.Add(newText);

                    textWidthSum -= textWidthCenter;
                    textWidthSum += columnOneWidth;
                }

                textHeightSum -= textHeightCenter;
                textHeightSum += rowHeight;
            }


            BlockReference newBr = new BlockReference(selTable.Location.X, selTable.Location.Y, 0, "PAPER_TABLE" + selTable.No, 1, 1, 1, 0);

            selBlock = newBl;
            return newBr;

        }
        private BlockReference BuildPaperTableGA(PaperTableModel selTable, AssemblyModel assemblyData, out Block selBlock)
        {
            //int rowCount = selTable.TableList.Count;
            //int columnCount = selTable.TableList[0].Length;

            double fontHeight = 2.5;
            double fontHeight2 = 2;
            double fontHeightTitle = 4;


            List<Entity> newList = new List<Entity>();
            // Default

            Point3D refPoint = new Point3D(0, 0);
            double defaultCount = 8;
            double tableWidth = 281;
            double titleRowHeight = 10;
            double rowHeight = 6;

            List<double> widthList = new List<double>();
            widthList.AddRange(new double[] { 10, 13, 15, 10, 10, 40, 18, 15, 10, 65, 75 });

            double beforeX = 0;
            double beforeY = 0;
            double currentY = 0;
            double currentX = 0;

            double partOneWidth = 10 + 13 + 15 + 10 + 10 + 40 + 18 + 15;
            double partTwoWidth = partOneWidth + 10;
            double partThrWidth = partTwoWidth + 65 + 75;

            if (assemblyData != null)
                if (defaultCount < assemblyData.NozzleShellInputModel.Count)
                    defaultCount = assemblyData.NozzleShellInputModel.Count;

            // 행
            newList.Add(new Line(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = layerService.LayerDimension });
            currentY += titleRowHeight;
            newList.Add(new Line(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = layerService.LayerDimension });
            currentY += rowHeight;
            newList.Add(new Line(GetSumPoint(refPoint, titleRowHeight, currentY), GetSumPoint(refPoint, tableWidth, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = layerService.LayerDimension });

            for (int i = 0; i < defaultCount - 1; i++)
            {
                currentY += rowHeight;
                // 마지막 행
                newList.Add(new Line(GetSumPoint(refPoint, titleRowHeight, currentY), GetSumPoint(refPoint, partOneWidth, currentY)) { Color = Color.White, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = layerService.LayerDimension });
                newList.Add(new Line(GetSumPoint(refPoint, partTwoWidth, currentY), GetSumPoint(refPoint, partThrWidth, currentY)) { Color = Color.White, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = layerService.LayerDimension });
            }
            currentY += rowHeight;
            // 마지막
            newList.Add(new Line(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = layerService.LayerDimension });

            // 열
            double colCount = 0;
            currentX = 0;
            newList.Add(new Line(GetSumPoint(refPoint, currentX, 0), GetSumPoint(refPoint, currentX, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = layerService.LayerDimension });
            foreach (double eachCol in widthList)
            {
                colCount++;
                currentX += eachCol;
                if (colCount == 1)
                    newList.Add(new Line(GetSumPoint(refPoint, currentX, titleRowHeight), GetSumPoint(refPoint, currentX, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = layerService.LayerDimension });
                else if (colCount == 11)
                    newList.Add(new Line(GetSumPoint(refPoint, currentX, 0), GetSumPoint(refPoint, currentX, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = layerService.LayerDimension });
                else
                    newList.Add(new Line(GetSumPoint(refPoint, currentX, titleRowHeight), GetSumPoint(refPoint, currentX, currentY)) { Color = Color.White, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = layerService.LayerDimension });
            }


            // 마지막 텍스트
            Text newTitle01 = new Text(GetSumPoint(refPoint, tableWidth / 2, titleRowHeight / 2), "NOZZLE SCHEDULE", fontHeightTitle);
            newTitle01.Alignment = Text.alignmentType.MiddleCenter;
            newTitle01.StyleName = textService.TextROMANS;
            newTitle01.LayerName = layerService.LayerDimension;
            newTitle01.ColorMethod = colorMethodType.byEntity;
            newTitle01.Color = Color.Yellow;

            Text newTitle02 = new Text(GetSumPoint(refPoint, titleRowHeight / 2, (currentY + titleRowHeight) / 2), "SHELL NOZZLE", fontHeightTitle);
            newTitle02.Alignment = Text.alignmentType.MiddleCenter;
            newTitle02.StyleName = textService.TextROMANS;
            newTitle02.LayerName = layerService.LayerDimension;
            newTitle02.ColorMethod = colorMethodType.byEntity;
            newTitle02.Color = Color.Yellow;
            newTitle02.Rotate(Utility.DegToRad(90), Vector3D.AxisZ, GetSumPoint(refPoint, titleRowHeight / 2, (currentY + titleRowHeight) / 2));

            Text newTitle03 = new Text(GetSumPoint(refPoint, partOneWidth + 10 / 2, (currentY + titleRowHeight + 6) / 2), "-SEE ORIENTATION DWG.-", fontHeight);
            newTitle03.Alignment = Text.alignmentType.MiddleCenter;
            newTitle03.StyleName = textService.TextROMANS;
            newTitle03.LayerName = layerService.LayerDimension;
            newTitle03.ColorMethod = colorMethodType.byEntity;
            newTitle03.Color = Color.White;
            newTitle03.WidthFactor = 0.95;
            newTitle03.Rotate(Utility.DegToRad(90), Vector3D.AxisZ, GetSumPoint(refPoint, partOneWidth + 10 / 2, (currentY + titleRowHeight + 6) / 2));

            newList.Add(newTitle01);
            newList.Add(newTitle02);



            double realCount = 0;
            currentY = 10;
            if (true)
            {
                realCount++;
                int colNumber = 0;
                double colWidth = 0;
                colWidth += widthList[colNumber];
                colNumber++;

                // Start firts column
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), "MARK", fontHeight, 0.95, Text.alignmentType.MiddleCenter));

                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), "SIZE", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), "SCH.", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), "Q'TY", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), "RATING & FACING", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), "\"R\"", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), "\"H\"", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), "ORT.", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), "DESCRIPTION", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), "REMARKS", fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            }


            realCount = 0;
            currentY = 10 + 6;
            if (assemblyData != null)
            {
                newList.Add(newTitle03);
                foreach (NozzleShellInputModel eachNozzle in assemblyData.NozzleShellInputModel)
                {
                    realCount++;
                    int colNumber = 0;
                    double colWidth = 0;
                    colWidth += widthList[colNumber];
                    colNumber++;

                    string seriesString = eachNozzle.ASMESeries.Replace("series", "").Replace(" ", "").ToLower();
                    string newName = "";
                    string newName2 = "";
                    if (seriesString.Contains("a"))
                    {
                        newName = "ASME 16.47";
                        newName2 = "\"SERIES A\" " + eachNozzle.Rating + " " + eachNozzle.Type + "." + eachNozzle.Facing;
                    }
                    else if (seriesString.Contains("b"))
                    {
                        newName = "ASME 16.47";
                        newName2 = "\"SERIES B\" " + eachNozzle.Rating + " " + eachNozzle.Type + "." + eachNozzle.Facing;
                    }

                    // Start firts column
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), eachNozzle.Mark, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

                    colWidth += widthList[colNumber];
                    colNumber++;
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), eachNozzle.Size, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

                    colWidth += widthList[colNumber];
                    colNumber++;
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), eachNozzle.SCH, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

                    colWidth += widthList[colNumber];
                    colNumber++;
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), eachNozzle.Qty, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

                    colWidth += widthList[colNumber];
                    colNumber++;
                    if (newName2 == "")
                    {
                        string flangeName = eachNozzle.Flange;
                        if (flangeName.ToLower().Contains("asme"))
                            flangeName = "ASME";
                        newName2 = flangeName + " " + eachNozzle.Rating + " " + eachNozzle.Type + "." + eachNozzle.Facing;
                        newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), newName2, fontHeight, 0.95, Text.alignmentType.MiddleCenter));
                    }
                    else
                    {
                        newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2 + rowHeight / 4), newName, fontHeight2, 0.95, Text.alignmentType.MiddleCenter));
                        newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 4), newName2, fontHeight2, 0.95, Text.alignmentType.MiddleCenter));
                    }

                    colWidth += widthList[colNumber];
                    colNumber++;
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), eachNozzle.R, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

                    colWidth += widthList[colNumber];
                    colNumber++;
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), eachNozzle.H, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

                    colWidth += widthList[colNumber];
                    colNumber++;

                    colWidth += widthList[colNumber];
                    colNumber++;
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), eachNozzle.Description, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

                    colWidth += widthList[colNumber];
                    colNumber++;
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), eachNozzle.Remarks, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

                    currentY += rowHeight;
                }
            }


            BlockReference newBr = new BlockReference(selTable.Location.X, selTable.Location.Y, 0, "PAPER_TABLE" + selTable.No, 1, 1, 1, 0);

            Block newBl = new Block("PAPER_TABLE_" + selTable.No);
            newBl.Entities.AddRange(newList);
            selBlock = newBl;
            return newBr;

        }
        private BlockReference BuildPaperTableGA2(PaperTableModel selTable, AssemblyModel assemblyData, out Block selBlock)
        {
            //int rowCount = selTable.TableList.Count;
            //int columnCount = selTable.TableList[0].Length;

            double fontHeight = 2.5;
            double fontHeight2 = 2;
            double fontHeightTitle = 4;


            List<Entity> newList = new List<Entity>();
            // Default

            Point3D refPoint = new Point3D(0, 0);
            double defaultCount = 8;
            double tableWidth = 281;
            double titleRowHeight = 10;
            double rowHeight = 6;

            List<double> widthList = new List<double>();
            widthList.AddRange(new double[] { 10, 13, 15, 10, 10, 40, 18, 15, 10, 65, 75 });

            double beforeX = 0;
            double beforeY = 0;
            double currentY = 0;
            double currentX = 0;

            double partOneWidth = 10 + 13 + 15 + 10 + 10 + 40 + 18 + 15;
            double partTwoWidth = partOneWidth + 10;
            double partThrWidth = partTwoWidth + 65 + 75;

            if (assemblyData != null)
                if (defaultCount < assemblyData.NozzleRoofInputModel.Count)
                    defaultCount = assemblyData.NozzleRoofInputModel.Count;

            // 행
            newList.Add(new Line(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = layerService.LayerDimension });
            currentY += titleRowHeight;
            newList.Add(new Line(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = layerService.LayerDimension });
            currentY += rowHeight;
            newList.Add(new Line(GetSumPoint(refPoint, titleRowHeight, currentY), GetSumPoint(refPoint, tableWidth, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = layerService.LayerDimension });

            for (int i = 0; i < defaultCount - 1; i++)
            {
                currentY += rowHeight;
                // 마지막 행
                newList.Add(new Line(GetSumPoint(refPoint, titleRowHeight, currentY), GetSumPoint(refPoint, partOneWidth, currentY)) { Color = Color.White, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = layerService.LayerDimension });
                newList.Add(new Line(GetSumPoint(refPoint, partTwoWidth, currentY), GetSumPoint(refPoint, partThrWidth, currentY)) { Color = Color.White, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = layerService.LayerDimension });
            }
            currentY += rowHeight;
            // 마지막
            newList.Add(new Line(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = layerService.LayerDimension });

            // 열
            double colCount = 0;
            currentX = 0;
            newList.Add(new Line(GetSumPoint(refPoint, currentX, 0), GetSumPoint(refPoint, currentX, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = layerService.LayerDimension });
            foreach (double eachCol in widthList)
            {
                colCount++;
                currentX += eachCol;
                if (colCount == 1)
                    newList.Add(new Line(GetSumPoint(refPoint, currentX, titleRowHeight), GetSumPoint(refPoint, currentX, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = layerService.LayerDimension });
                else if (colCount == 11)
                    newList.Add(new Line(GetSumPoint(refPoint, currentX, 0), GetSumPoint(refPoint, currentX, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = layerService.LayerDimension });
                else
                    newList.Add(new Line(GetSumPoint(refPoint, currentX, titleRowHeight), GetSumPoint(refPoint, currentX, currentY)) { Color = Color.White, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = layerService.LayerDimension });
            }


            // 마지막 텍스트
            Text newTitle01 = new Text(GetSumPoint(refPoint, tableWidth / 2, titleRowHeight / 2), "NOZZLE SCHEDULE", fontHeightTitle);
            newTitle01.Alignment = Text.alignmentType.MiddleCenter;
            newTitle01.StyleName = textService.TextROMANS;
            newTitle01.LayerName = layerService.LayerDimension;
            newTitle01.ColorMethod = colorMethodType.byEntity;
            newTitle01.Color = Color.Yellow;

            Text newTitle02 = new Text(GetSumPoint(refPoint, titleRowHeight / 2, (currentY + titleRowHeight) / 2), "ROOF NOZZLE", fontHeightTitle);
            newTitle02.Alignment = Text.alignmentType.MiddleCenter;
            newTitle02.StyleName = textService.TextROMANS;
            newTitle02.LayerName = layerService.LayerDimension;
            newTitle02.ColorMethod = colorMethodType.byEntity;
            newTitle02.Color = Color.Yellow;
            newTitle02.Rotate(Utility.DegToRad(90), Vector3D.AxisZ, GetSumPoint(refPoint, titleRowHeight / 2, (currentY + titleRowHeight) / 2));

            Text newTitle03 = new Text(GetSumPoint(refPoint, partOneWidth + 10 / 2, (currentY + titleRowHeight + 6) / 2), "-SEE ORIENTATION DWG.-", fontHeight);
            newTitle03.Alignment = Text.alignmentType.MiddleCenter;
            newTitle03.StyleName = textService.TextROMANS;
            newTitle03.LayerName = layerService.LayerDimension;
            newTitle03.ColorMethod = colorMethodType.byEntity;
            newTitle03.Color = Color.White;
            newTitle03.WidthFactor = 0.95;
            newTitle03.Rotate(Utility.DegToRad(90), Vector3D.AxisZ, GetSumPoint(refPoint, partOneWidth + 10 / 2, (currentY + titleRowHeight + 6) / 2));

            newList.Add(newTitle01);
            newList.Add(newTitle02);



            double realCount = 0;
            currentY = 10;
            if (true)
            {
                realCount++;
                int colNumber = 0;
                double colWidth = 0;
                colWidth += widthList[colNumber];
                colNumber++;

                // Start firts column
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), "MARK", fontHeight, 0.95, Text.alignmentType.MiddleCenter));

                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), "SIZE", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), "SCH.", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), "Q'TY", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), "RATING & FACING", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), "\"R\"", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), "\"H\"", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), "ORT.", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), "DESCRIPTION", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), "REMARKS", fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            }


            realCount = 0;
            currentY = 10 + 6;
            if (assemblyData != null)
            {
                newList.Add(newTitle03);
                foreach (NozzleRoofInputModel eachNozzle in assemblyData.NozzleRoofInputModel)
                {
                    realCount++;
                    int colNumber = 0;
                    double colWidth = 0;
                    colWidth += widthList[colNumber];
                    colNumber++;

                    string seriesString = eachNozzle.ASMESeries.Replace("series", "").Replace(" ", "").ToLower();
                    string newName = "";
                    string newName2 = "";
                    if (seriesString.Contains("a"))
                    {
                        newName = "ASME 16.47";
                        newName2 = "\"SERIES A\" " + eachNozzle.Rating + " " + eachNozzle.Type + "." + eachNozzle.Facing;
                    }
                    else if (seriesString.Contains("b"))
                    {
                        newName = "ASME 16.47";
                        newName2 = "\"SERIES B\" " + eachNozzle.Rating + " " + eachNozzle.Type + "." + eachNozzle.Facing;
                    }

                    // Start firts column
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), eachNozzle.Mark, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

                    colWidth += widthList[colNumber];
                    colNumber++;
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), eachNozzle.Size, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

                    colWidth += widthList[colNumber];
                    colNumber++;
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), eachNozzle.SCH, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

                    colWidth += widthList[colNumber];
                    colNumber++;
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), eachNozzle.Qty, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

                    colWidth += widthList[colNumber];
                    colNumber++;
                    if (newName2 == "")
                    {
                        string flangeName = eachNozzle.Flange;
                        if (flangeName.ToLower().Contains("asme"))
                            flangeName = "ASME";
                        newName2 = flangeName + " " + eachNozzle.Rating + " " + eachNozzle.Type + "." + eachNozzle.Facing;
                        newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), newName2, fontHeight, 0.95, Text.alignmentType.MiddleCenter));
                    }
                    else
                    {
                        newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2 + rowHeight / 4), newName, fontHeight2, 0.95, Text.alignmentType.MiddleCenter));
                        newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 4), newName2, fontHeight2, 0.95, Text.alignmentType.MiddleCenter));
                    }

                    colWidth += widthList[colNumber];
                    colNumber++;
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), eachNozzle.R, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

                    colWidth += widthList[colNumber];
                    colNumber++;
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), eachNozzle.H, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

                    colWidth += widthList[colNumber];
                    colNumber++;

                    colWidth += widthList[colNumber];
                    colNumber++;
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), eachNozzle.Description, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

                    colWidth += widthList[colNumber];
                    colNumber++;
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY + rowHeight / 2), eachNozzle.Remarks, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

                    currentY += rowHeight;
                }
            }


            BlockReference newBr = new BlockReference(selTable.Location.X, selTable.Location.Y, 0, "PAPER_TABLE" + selTable.No, 1, 1, 1, 0);

            Block newBl = new Block("PAPER_TABLE_" + selTable.No);
            newBl.Entities.AddRange(newList);
            selBlock = newBl;
            return newBr;

        }


        private BlockReference BuildPaperTableShellPlateBM(PaperTableModel selTable, AssemblyModel assemblyData, PAPERMAIN_TYPE selDwgName,  out Block selBlock)
        {
            //int rowCount = selTable.TableList.Count;
            //int columnCount = selTable.TableList[0].Length;

            double fontHeight = 2.5;
            double fontHeight2 = 2;
            double fontHeightTitle = 4;


            List<Entity> newList = new List<Entity>();
            // Default

            Point3D refPoint = new Point3D(0, 0);
            double tableWidth = 180;
            double titleRowHeight = 10;
            double rowHeight = 7;

            List<double> widthList = new List<double>();
            widthList.AddRange(new double[] { 10, 45, 25, 53, 12, 15, 20 });

            double beforeX = 0;
            double beforeY = 0;
            double currentY = 0;
            double currentX = 0;

            double defaultCount = SingletonData.BMList.Count;

            double tableHeight = titleRowHeight + (rowHeight * defaultCount)+rowHeight;

            // 행
            currentY = tableHeight;
            newList.Add(new Line(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)) );
            currentY -= titleRowHeight;
            newList.Add(new Line(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            for (int i = 0; i < defaultCount ; i++)
            {
                currentY -= rowHeight;
                // 마지막 행
                newList.Add(new Line(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)) );
            }
            currentY -= rowHeight;
            // 마지막
            newList.Add(new Line(GetSumPoint(refPoint, 80, currentY), GetSumPoint(refPoint, 80, currentY+rowHeight)));
            newList.Add(new Line(GetSumPoint(refPoint, 80, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            // 열
            double colCount = 0;
            currentX = 0;
            foreach (double eachCol in widthList)
            {
                colCount++;
                newList.Add(new Line(GetSumPoint(refPoint, currentX, rowHeight), GetSumPoint(refPoint, currentX, tableHeight)));
                currentX += eachCol;

            }

            // 마지막 텍스트
            Text newTitle01 = new Text(GetSumPoint(refPoint, 80 + 53/2, currentY + rowHeight / 2), "TOTAL WEIGHT", fontHeight);
            newTitle01.Alignment = Text.alignmentType.MiddleCenter;
            newTitle01.StyleName = textService.TextROMANS;

            Text newTitle02 = new Text(GetSumPoint(refPoint, tableWidth-20, currentY + rowHeight / 2), "0.0", fontHeight);
            newTitle02.Alignment = Text.alignmentType.MiddleRight;
            newTitle02.StyleName = textService.TextROMANS;

            Text newTitle03 = new Text(GetSumPoint(refPoint, tableWidth - 20 / 2, currentY + rowHeight / 2), "kg", fontHeight);
            newTitle03.Alignment = Text.alignmentType.MiddleCenter;
            newTitle03.StyleName = textService.TextROMANS;


            newList.Add(newTitle01);
            newList.Add(newTitle02);
            newList.Add(newTitle03);



            double realCount = 0;

            if (true)
            {
                realCount++;
                int colNumber = 0;
                double colWidth = 0;

                // Start firts column
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, tableHeight- titleRowHeight / 2), "NO.", fontHeight, 1, Text.alignmentType.MiddleCenter));

                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, tableHeight - titleRowHeight / 2), "NAME", fontHeight, 1, Text.alignmentType.MiddleCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, tableHeight - titleRowHeight / 2), "MAT'L", fontHeight, 1, Text.alignmentType.MiddleCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, tableHeight - titleRowHeight / 2), "DIMENSION", fontHeight, 1, Text.alignmentType.MiddleCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, tableHeight - titleRowHeight / 2 +0.5), "NO. OF", fontHeight2, 1, Text.alignmentType.BaselineCenter));
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, tableHeight - titleRowHeight / 2 -0.5), "1 SET", fontHeight2, 1, Text.alignmentType.TopCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, tableHeight - titleRowHeight / 2 +0.5), "WEIGHT", fontHeight2, 1, Text.alignmentType.BaselineCenter));
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, tableHeight - titleRowHeight / 2 -0.5), "(Kg)", fontHeight2, 1, Text.alignmentType.TopCenter));
                colWidth += widthList[colNumber];
                colNumber++;
                newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, tableHeight - titleRowHeight / 2), "REMARK", fontHeight,1, Text.alignmentType.MiddleCenter));

            }

            currentY = tableHeight-titleRowHeight;
            foreach (DrawBMModel eachBM in SingletonData.BMList)
            {
                if(eachBM.DWGName== selDwgName)
                {
                    realCount++;
                    int colNumber = 0;
                    double colWidth = 0;

                    // Start firts column
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY - rowHeight / 2), eachBM.No, fontHeight, 1, Text.alignmentType.MiddleCenter));

                    colWidth += widthList[colNumber];
                    colNumber++;
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY - rowHeight / 2), eachBM.Name, fontHeight, 1, Text.alignmentType.MiddleCenter));

                    colWidth += widthList[colNumber];
                    colNumber++;
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY - rowHeight / 2), eachBM.Material, fontHeight, 1, Text.alignmentType.MiddleCenter));

                    colWidth += widthList[colNumber];
                    colNumber++;
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY - rowHeight / 2), eachBM.Dimension, fontHeight, 1, Text.alignmentType.MiddleCenter));

                    colWidth += widthList[colNumber];
                    colNumber++;
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY - rowHeight / 2), eachBM.Set, fontHeight, 1, Text.alignmentType.MiddleCenter));

                    colWidth += widthList[colNumber];
                    colNumber++;
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY - rowHeight / 2), eachBM.Weight, fontHeight, 1, Text.alignmentType.MiddleCenter));

                    colWidth += widthList[colNumber];
                    colNumber++;
                    newList.Add(GetNewTextWhite(GetSumPoint(refPoint, colWidth + widthList[colNumber] / 2, currentY - rowHeight / 2), eachBM.Remark, fontHeight, 1, Text.alignmentType.MiddleCenter));


                    currentY -= rowHeight;
                }

            }

            BlockReference newBr = new BlockReference(selTable.Location.X, selTable.Location.Y, 0, "PAPER_TABLE" + selTable.No, 1, 1, 1, 0);

            Block newBl = new Block("PAPER_TABLE_" +selTable.Name + selTable.No);

            styleService.SetLayerListEntity(ref newList, layerService.LayerDimension);
            newBl.Entities.AddRange(newList);
            selBlock = newBl;
            return newBr;

        }


        private BlockReference BuildPaperTableNozzleProjection (PaperTableModel selTable, AssemblyModel assemblyData, out Block selBlock)
        {



            List<Entity> newList = new List<Entity>();
            // Default

            Point3D refPoint = new Point3D(0, 0);

            List<Point3D> outPointList = new List<Point3D>();

            TANK_TYPE refTankType = SingletonData.TankType;
            // Left : Lower

            // Outline
            double boxHeight = 99;
            double boxWidth = 99;
            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            newList.AddRange(shapeService.GetRectangle(out outPointList, GetSumPoint(referencePoint, 0, 0), boxWidth, boxHeight, 0, 0, 3));

            // Tank : Basic
            double tankID = 52;
            double tankRadius = tankID / 2;
            double tankHeight = 48;

            double tankBottomWidth = 60;
            double tankBottomThickness = 1;
            double tankBottomOutWidth = (tankBottomWidth - tankID) / 2;

            double tankRoofWidth = 58;
            double tankRoofHeight = 0;
            double tankRoofOutWidth = (tankRoofWidth - tankID) / 2;


            // TankRoof Height
            switch (refTankType)
            {
                case TANK_TYPE.CRT:
                case TANK_TYPE.DRT:
                case TANK_TYPE.IFRT:
                    tankRoofHeight = 4;
                    break;
                case TANK_TYPE.EFRTSingle:
                case TANK_TYPE.EFRTDouble:
                    tankRoofHeight = 0;
                    break;
            }

            Point3D tankPoint = GetSumPoint(referencePoint, 15.6, 24.1);
            Point3D tankBottomPoint = GetSumPoint(tankPoint, -tankBottomOutWidth, 0);
            Point3D tankRoofPoint = GetSumPoint(tankPoint, 0, tankHeight);
            Point3D tankRoofCenterTopPoint = GetSumPoint(tankRoofPoint, tankRadius, tankRoofHeight);

            // Tank : Shell
            newList.AddRange(shapeService.GetRectangle(out outPointList, GetSumPoint(tankPoint, 0, 0), tankID, tankHeight, 0, 0, 3, new bool[] { false, true, false, true }));

            // Tank : Bottom
            newList.AddRange(shapeService.GetRectangle(out outPointList, GetSumPoint(tankBottomPoint, 0, 0), tankBottomWidth, tankBottomThickness, 0, 0, 0));

            // Tank : Roof : Lower
            newList.Add(new Line(GetSumPoint(tankRoofPoint, -tankRoofOutWidth, 0), GetSumPoint(tankRoofPoint, tankID + tankRoofOutWidth, 0)));
            Line rightRoof = null;
            Arc domeRoof = null;

            // TankRoof Height
            switch (refTankType)
            {
                case TANK_TYPE.CRT:
                case TANK_TYPE.IFRT:
                    newList.Add(new Line(GetSumPoint(tankRoofPoint, -tankRoofOutWidth, 0), GetSumPoint(tankRoofCenterTopPoint, 0, 0)));
                    rightRoof = new Line(GetSumPoint(tankRoofPoint, tankID + tankRoofOutWidth, 0), GetSumPoint(tankRoofCenterTopPoint, 0, 0));
                    newList.Add(rightRoof);
                    break;
                case TANK_TYPE.DRT:
                    domeRoof = new Arc(Plane.XY, GetSumPoint(tankRoofPoint, tankID, 0), GetSumPoint(tankRoofCenterTopPoint, 0, 0), GetSumPoint(tankRoofPoint, 0, 0), false);
                    newList.Add(domeRoof);
                    break;
            }


            // Nozzle
            double roofNozzleRadius = 20;
            double shellNozzleElevation = 10;
            double nozzleHeight = 6;
            double flangeOD = 3;
            double flangeODHalf = flangeOD / 2;
            Point3D roofNozzlePoint = GetSumPoint(tankRoofPoint, tankRadius + roofNozzleRadius, 0);
            Line roofNozzlePipe = new Line(GetSumPoint(roofNozzlePoint, 0, 0), GetSumPoint(roofNozzlePoint, 0, nozzleHeight));
            Line roofNozzleFlange = new Line(GetSumPoint(roofNozzlePoint, -flangeODHalf, nozzleHeight), GetSumPoint(roofNozzlePoint, flangeODHalf, nozzleHeight));
            newList.Add(roofNozzlePipe);
            newList.Add(roofNozzleFlange);

            Point3D[] nozzlePipeInter = null;

            switch (refTankType)
            {
                case TANK_TYPE.CRT:
                case TANK_TYPE.IFRT:
                    nozzlePipeInter = roofNozzlePipe.IntersectWith(rightRoof);
                    if (nozzlePipeInter.Length > 0)
                        roofNozzlePipe.TrimBy(nozzlePipeInter[0], true);
                    break;
                case TANK_TYPE.DRT:
                    nozzlePipeInter = roofNozzlePipe.IntersectWith(domeRoof);
                    if (nozzlePipeInter.Length > 0)
                        roofNozzlePipe.TrimBy(nozzlePipeInter[0], true);
                    break;
            }



            Point3D shellNozzlePoint = GetSumPoint(tankPoint, tankID, shellNozzleElevation);
            Line shellNozzlePipe = new Line(GetSumPoint(shellNozzlePoint, 0, 0), GetSumPoint(shellNozzlePoint, nozzleHeight, 0));
            Line shellNozzleFlange = new Line(GetSumPoint(shellNozzlePoint, nozzleHeight, flangeODHalf), GetSumPoint(shellNozzlePoint, nozzleHeight, -flangeODHalf));
            newList.Add(shellNozzlePipe);
            newList.Add(shellNozzleFlange);


            // Pontoon
            double pontoonWidth = 10;
            double pontoonMinHeight = 2.5;
            double pontoonElevation = 23;
            double pontoonShellWidth = 1.5;
            double pontoonSlopeHeight = 1;
            double pontoonSingleFlatHeight = 1;

            Point3D pontoonLeftPoint = GetSumPoint(tankPoint, pontoonShellWidth, pontoonElevation);
            Point3D pontoonRightPoint = GetSumPoint(tankPoint, tankID - pontoonShellWidth, pontoonElevation);


            double ponLeftNozzleRadius = 10;
            double ponRightNozzleRadius = 20;
            Line leftPonNozzleFlangeAll = null;
            Line leftPonNozzlePipeAll = null; ;
            Line rightPonNozzleFlangeAll = null;
            Line rightPonNozzlePipeAll = null;

            Point3D deckPoint = null;
            Point3D pontoonPoint = null;

            switch (refTankType)
            {
                case TANK_TYPE.IFRT:
                case TANK_TYPE.EFRTSingle:
                    if (true)
                    {
                        Line leftPon1 = new Line(GetSumPoint(pontoonLeftPoint, 0, 0), GetSumPoint(pontoonLeftPoint, pontoonWidth, 0));
                        Line leftPon2 = new Line(GetSumPoint(pontoonLeftPoint, 0, pontoonMinHeight + pontoonSlopeHeight), GetSumPoint(pontoonLeftPoint, pontoonWidth, pontoonMinHeight));
                        Line leftPon3 = new Line(GetSumPoint(pontoonLeftPoint, 0, pontoonMinHeight + pontoonSlopeHeight), GetSumPoint(pontoonLeftPoint, 0, 0));
                        Line leftPon4 = new Line(GetSumPoint(pontoonLeftPoint, pontoonWidth, 0), GetSumPoint(pontoonLeftPoint, pontoonWidth, pontoonMinHeight));

                        Line rightPon1 = new Line(GetSumPoint(pontoonRightPoint, 0, 0), GetSumPoint(pontoonRightPoint, -pontoonWidth, 0));
                        Line rightPon2 = new Line(GetSumPoint(pontoonRightPoint, 0, pontoonMinHeight + pontoonSlopeHeight), GetSumPoint(pontoonRightPoint, -pontoonWidth, pontoonMinHeight));
                        Line rightPon3 = new Line(GetSumPoint(pontoonRightPoint, 0, pontoonMinHeight + pontoonSlopeHeight), GetSumPoint(pontoonRightPoint, 0, 0));
                        Line rightPon4 = new Line(GetSumPoint(pontoonRightPoint, -pontoonWidth, 0), GetSumPoint(pontoonRightPoint, -pontoonWidth, pontoonMinHeight));

                        Line ponFlat = new Line(GetSumPoint(pontoonLeftPoint, pontoonWidth, pontoonSingleFlatHeight), GetSumPoint(pontoonRightPoint, -pontoonWidth, pontoonSingleFlatHeight));
                        newList.AddRange(new Entity[] { leftPon1, leftPon2, leftPon3, leftPon4, rightPon1, rightPon2, rightPon3, rightPon4, ponFlat });

                        // Leader Point
                        deckPoint = GetSumPoint(ponFlat.MidPoint, 2, 0);
                        pontoonPoint = GetSumPoint(leftPon1.StartPoint, 2, 0);

                        // Left Nozzle
                        Point3D ponLeftNozzlePoint = GetSumPoint(pontoonLeftPoint, -pontoonShellWidth + tankRadius - ponLeftNozzleRadius, pontoonSingleFlatHeight);
                        leftPonNozzlePipeAll = new Line(GetSumPoint(ponLeftNozzlePoint, 0, 0), GetSumPoint(ponLeftNozzlePoint, 0, nozzleHeight));
                        leftPonNozzleFlangeAll = new Line(GetSumPoint(ponLeftNozzlePoint, -flangeODHalf, nozzleHeight), GetSumPoint(ponLeftNozzlePoint, flangeODHalf, nozzleHeight));
                        newList.Add(leftPonNozzlePipeAll);
                        newList.Add(leftPonNozzleFlangeAll);


                        // Right Nozzle
                        Point3D ponRightNozzlePointTemp = GetSumPoint(pontoonLeftPoint, -pontoonShellWidth + tankRadius + ponRightNozzleRadius, pontoonSingleFlatHeight);
                        Point3D ponRightNozzlePoint = null;
                        Line vRightNozzleLine = new Line(GetSumPoint(ponRightNozzlePointTemp, 0, 0), GetSumPoint(ponRightNozzlePointTemp, 0, 100));
                        Point3D[] ponRightInter = vRightNozzleLine.IntersectWith(rightPon2);
                        if (ponRightInter.Length > 0)
                        {
                            ponRightNozzlePoint = ponRightInter[0];
                            double nozzleHeightAdj = nozzleHeight - 1;
                            rightPonNozzlePipeAll = new Line(GetSumPoint(ponRightNozzlePoint, 0, 0), GetSumPoint(ponRightNozzlePoint, 0, nozzleHeightAdj));
                            rightPonNozzleFlangeAll = new Line(GetSumPoint(ponRightNozzlePoint, -flangeODHalf, nozzleHeightAdj), GetSumPoint(ponRightNozzlePoint, flangeODHalf, nozzleHeightAdj));
                            newList.Add(rightPonNozzlePipeAll);
                            newList.Add(rightPonNozzleFlangeAll);
                        }

                    }
                    break;
                case TANK_TYPE.EFRTDouble:
                    if (true)
                    {
                        Line leftPon1 = new Line(GetSumPoint(pontoonLeftPoint, 0, 0), GetSumPoint(pontoonLeftPoint, pontoonWidth, 0));
                        Line leftPon2 = new Line(GetSumPoint(pontoonLeftPoint, 0, pontoonMinHeight + pontoonSlopeHeight), GetSumPoint(pontoonLeftPoint, pontoonWidth, pontoonMinHeight));
                        Line leftPon3 = new Line(GetSumPoint(pontoonLeftPoint, 0, pontoonMinHeight + pontoonSlopeHeight), GetSumPoint(pontoonLeftPoint, 0, 0));
                        Line leftPon4 = new Line(GetSumPoint(pontoonLeftPoint, pontoonWidth, 0), GetSumPoint(pontoonLeftPoint, pontoonWidth, pontoonMinHeight));

                        Line rightPon1 = new Line(GetSumPoint(pontoonRightPoint, 0, 0), GetSumPoint(pontoonRightPoint, -pontoonWidth, 0));
                        Line rightPon2 = new Line(GetSumPoint(pontoonRightPoint, 0, pontoonMinHeight + pontoonSlopeHeight), GetSumPoint(pontoonRightPoint, -pontoonWidth, pontoonMinHeight));
                        Line rightPon3 = new Line(GetSumPoint(pontoonRightPoint, 0, pontoonMinHeight + pontoonSlopeHeight), GetSumPoint(pontoonRightPoint, 0, 0));
                        Line rightPon4 = new Line(GetSumPoint(pontoonRightPoint, -pontoonWidth, 0), GetSumPoint(pontoonRightPoint, -pontoonWidth, pontoonMinHeight));

                        Line ponFlat1 = new Line(GetSumPoint(pontoonLeftPoint, pontoonWidth, pontoonMinHeight), GetSumPoint(pontoonRightPoint, -pontoonWidth, pontoonMinHeight));
                        Line ponFlat2 = new Line(GetSumPoint(pontoonLeftPoint, pontoonWidth, 0), GetSumPoint(pontoonRightPoint, -pontoonWidth, 0));
                        newList.AddRange(new Entity[] { leftPon1, leftPon2, leftPon3, leftPon4, rightPon1, rightPon2, rightPon3, rightPon4, ponFlat1, ponFlat2 });

                        // Leader Point
                        deckPoint = GetSumPoint(ponFlat1.MidPoint, 2, 0);
                        pontoonPoint = GetSumPoint(leftPon1.StartPoint, 2, 0);

                        // Left Nozzle
                        Point3D ponLeftNozzlePoint = GetSumPoint(pontoonLeftPoint, -pontoonShellWidth + tankRadius - ponLeftNozzleRadius, pontoonMinHeight);
                        leftPonNozzlePipeAll = new Line(GetSumPoint(ponLeftNozzlePoint, 0, 0), GetSumPoint(ponLeftNozzlePoint, 0, nozzleHeight));
                        leftPonNozzleFlangeAll = new Line(GetSumPoint(ponLeftNozzlePoint, -flangeODHalf, nozzleHeight), GetSumPoint(ponLeftNozzlePoint, flangeODHalf, nozzleHeight));
                        newList.Add(leftPonNozzlePipeAll);
                        newList.Add(leftPonNozzleFlangeAll);

                        // Right Nozzle
                        Point3D ponRightNozzlePointTemp = GetSumPoint(pontoonLeftPoint, -pontoonShellWidth + tankRadius + ponRightNozzleRadius, pontoonSingleFlatHeight);
                        Point3D ponRightNozzlePoint = null;
                        Line vRightNozzleLine = new Line(GetSumPoint(ponRightNozzlePointTemp, 0, 0), GetSumPoint(ponRightNozzlePointTemp, 0, 100));
                        Point3D[] ponRightInter = vRightNozzleLine.IntersectWith(rightPon2);
                        if (ponRightInter.Length > 0)
                        {
                            ponRightNozzlePoint = ponRightInter[0];
                            double nozzleHeightAdj = nozzleHeight - 1;
                            rightPonNozzlePipeAll = new Line(GetSumPoint(ponRightNozzlePoint, 0, 0), GetSumPoint(ponRightNozzlePoint, 0, nozzleHeightAdj));
                            rightPonNozzleFlangeAll = new Line(GetSumPoint(ponRightNozzlePoint, -flangeODHalf, nozzleHeightAdj), GetSumPoint(ponRightNozzlePoint, flangeODHalf, nozzleHeightAdj));
                            newList.Add(rightPonNozzlePipeAll);
                            newList.Add(rightPonNozzleFlangeAll);
                        }
                    }
                    break;
            }


            // Center Line
            double exLength = 2;
            List<Entity> centerLineList = editingService.GetCenterLine(GetSumPoint(tankPoint, tankRadius, -tankBottomThickness), GetSumPoint(tankRoofCenterTopPoint, 0, 0), exLength, 1);
            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);



            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);

            // Dimension
            double dimTextHeight = 2.5;
            double dimCenterLineHeight = 12;
            double dimCenterLineHeight2 = 12;
            Plane planeLeft = new Plane(Point3D.Origin, Vector3D.AxisY, -1 * Vector3D.AxisX); // plane.XY, vertical writing;

            // Dimension : Roof : R
            if (refTankType == TANK_TYPE.IFRT || refTankType == TANK_TYPE.EFRTSingle || refTankType == TANK_TYPE.EFRTDouble)
                dimCenterLineHeight2 = 6;


            List<LinearDim> dimList = new List<LinearDim>();
            List<Entity> leaderList = new List<Entity>();


            Point3D dimLeft1 = ((ICurve)centerLineList[2]).EndPoint;
            Point3D dimRight1 = roofNozzleFlange.MidPoint;
            Point3D dimMid1 = GetSumPoint(dimLeft1, (dimRight1.X - dimLeft1.X) / 2, dimCenterLineHeight);
            dimList.Add(new LinearDim(Plane.XY, dimLeft1, dimRight1, dimMid1, dimTextHeight) { TextOverride = "\"R\"" });

            // Dimension : Shell : R
            Point3D dimLeft2 = GetSumPoint(tankPoint, tankRadius, shellNozzleElevation + flangeODHalf);
            Point3D dimRight2 = GetSumPoint(shellNozzlePoint, nozzleHeight, flangeODHalf);
            Point3D dimMid2 = GetSumPoint(dimLeft2, (dimRight2.X - dimLeft2.X) / 2, dimCenterLineHeight2);
            dimList.Add(new LinearDim(Plane.XY, dimLeft2, dimRight2, dimMid2, dimTextHeight) { TextOverride = "\"R\"", ShowExtLine1 = false });

            // Dimension : Roof : H
            Point3D dimLeft3 = GetSumPoint(roofNozzleFlange.EndPoint, 0, 0);
            Point3D dimRight3 = GetSumPoint(tankPoint, tankID + tankBottomOutWidth, -tankBottomThickness);
            Point3D dimMid3 = GetSumPoint(dimRight3, dimCenterLineHeight * 1.4, (dimLeft3.Y - dimRight3.Y) / 2);
            dimList.Add(new LinearDim(planeLeft, dimLeft3, dimRight3, dimMid3, dimTextHeight) { TextOverride = "\"H\"" });

            // Dimension : Shell : H
            Point3D dimLeft4 = GetSumPoint(shellNozzleFlange.MidPoint, 0, 0);
            Point3D dimRight4 = GetSumPoint(tankPoint, tankID + tankBottomOutWidth, -tankBottomThickness);
            Point3D dimMid4 = GetSumPoint(dimLeft4, dimCenterLineHeight * 0.6, (dimRight4.Y - dimLeft4.Y) / 2);
            dimList.Add(new LinearDim(planeLeft, dimLeft4, dimRight4, dimMid4, dimTextHeight) { TextOverride = "\"H\"", ArrowsLocation = elementPositionType.Inside, ShowExtLine1 = false });

            if (refTankType == TANK_TYPE.IFRT || refTankType == TANK_TYPE.EFRTSingle || refTankType == TANK_TYPE.EFRTDouble)
            {
                dimCenterLineHeight = 10;
                if (refTankType == TANK_TYPE.EFRTDouble)
                    dimCenterLineHeight = dimCenterLineHeight - (2.5 / 2);

                Point3D dimPonLeft1 = leftPonNozzleFlangeAll.MidPoint;
                Point3D dimPonRight1 = GetSumPoint(dimPonLeft1, ponLeftNozzleRadius, 0);
                Point3D dimPonMid1 = GetSumPoint(dimPonLeft1, (dimPonRight1.X - dimPonLeft1.X) / 2, dimCenterLineHeight);
                dimList.Add(new LinearDim(Plane.XY, dimPonLeft1, dimPonRight1, dimPonMid1, dimTextHeight) { TextOverride = "\"R\"", ArrowsLocation = elementPositionType.Inside, ShowExtLine2 = false });

                Point3D dimPonRight2 = rightPonNozzleFlangeAll.MidPoint;
                Point3D dimPonLeft2 = GetSumPoint(dimPonRight2, -ponRightNozzleRadius, 0);
                Point3D dimPonMid2 = new Point3D(GetSumPoint(dimPonLeft2, (dimPonRight2.X - dimPonLeft2.X) / 2, dimCenterLineHeight).X, dimPonMid1.Y);
                dimList.Add(new LinearDim(Plane.XY, dimPonLeft2, dimPonRight2, dimPonMid2, dimTextHeight) { TextOverride = "\"R\"", ShowExtLine1 = false });

                // Dimension : Shell : H
                Point3D dimPonLeft4 = GetSumPoint(leftPonNozzleFlangeAll.EndPoint, 0, 0);
                Point3D dimPonRight4 = GetSumPoint(leftPonNozzlePipeAll.StartPoint, 0, 0);
                Point3D dimPonMid4 = GetSumPoint(dimPonLeft4, dimCenterLineHeight * 0.4, (dimPonRight4.Y - dimPonLeft4.Y) / 2);
                dimList.Add(new LinearDim(planeLeft, dimPonLeft4, dimPonRight4, dimPonMid4, dimTextHeight) { TextOverride = "\"H\"", ArrowsLocation = elementPositionType.Outside, TextLocation = elementPositionType.Outside, ShowExtLine1 = false });

                // Dimension : Shell : H
                Point3D dimPonLeft5 = GetSumPoint(rightPonNozzleFlangeAll.EndPoint, 0, 0);
                Point3D dimPonRight5 = GetSumPoint(rightPonNozzlePipeAll.StartPoint, 0, 0);
                Point3D dimPonMid5 = new Point3D(dimMid4.X, GetSumPoint(dimPonLeft5, dimCenterLineHeight * 0.4, 0).Y);
                dimList.Add(new LinearDim(planeLeft, dimPonLeft5, dimPonRight5, dimPonMid5, dimTextHeight) { TextOverride = "\"H\"", ArrowsLocation = elementPositionType.Outside, TextLocation = elementPositionType.Outside });



                Point3D deckTextPoint = GetSumPoint(deckPoint, 4, 8);
                Leader deckLeader = new Leader(Plane.XY, new Point3D[] { deckPoint, deckTextPoint, GetSumPoint(deckTextPoint, 11, 0) });
                Text deckText = new Text(GetSumPoint(deckTextPoint, 1, 0.5), "DECK", 2.5);
                deckText.Alignment = Text.alignmentType.BaselineLeft;
                leaderList.Add(deckLeader);

                Point3D ponTextPoint = GetSumPoint(pontoonPoint, 3, -14);
                Leader ponLeader = new Leader(Plane.XY, new Point3D[] { pontoonPoint, ponTextPoint, GetSumPoint(ponTextPoint, 18.5, 0) });
                Text ponText = new Text(GetSumPoint(ponTextPoint, 1, 0.5), "PONTOON", 2.5);
                ponText.Alignment = Text.alignmentType.BaselineLeft;
                leaderList.Add(ponLeader);

                styleService.SetLayerListEntity(ref leaderList, layerService.LayerDimension);
                styleService.SetLayer(ref deckText, layerService.LayerDimension);
                styleService.SetLayer(ref ponText, layerService.LayerDimension);
                leaderList.Add(deckText);
                leaderList.Add(ponText);

            }

            foreach (LinearDim eachDim in dimList)
            {
                eachDim.LayerName = layerService.LayerDimension;
                eachDim.LineTypeMethod = colorMethodType.byLayer;
                eachDim.ColorMethod = colorMethodType.byLayer;
            }

            // Text
            List<Entity> textList = new List<Entity>();
            double textHeight = 2.5;
            Point3D textCenter = GetSumPoint(tankPoint, tankID + tankBottomOutWidth + 12 * 1.4 / 2, tankBottomThickness);
            Text newUnderText1 = new Text(GetSumPoint(textCenter, 0, -textHeight * 2 - 0.5), "UNDER", textHeight);
            newUnderText1.Alignment = Text.alignmentType.BottomCenter;
            Text newUnderText2 = new Text(GetSumPoint(textCenter, 0, -textHeight * 3 - 1), "OF BTM.", textHeight);
            newUnderText2.Alignment = Text.alignmentType.BottomCenter;
            textList.Add(newUnderText1);
            textList.Add(newUnderText2);

            styleService.SetLayerListEntity(ref textList, layerService.LayerDimension);

            // Title
            List<Entity> titleList = new List<Entity>();
            Point3D titleBaseCenter1 = GetSumPoint(referencePoint, boxWidth / 2, 7);
            Point3D titleBaseCenter2 = GetSumPoint(referencePoint, boxWidth / 2, 8);
            Point3D titleTextCenter = GetSumPoint(referencePoint, boxWidth / 2, 9);

            double baseLineWidth = 65;
            Line titleBaseLine1 = new Line(GetSumPoint(titleBaseCenter1, -baseLineWidth / 2, 0), GetSumPoint(titleBaseCenter1, baseLineWidth / 2, 0));
            styleService.SetLayer(ref titleBaseLine1, layerService.LayerOutLine);
            Line titleBaseLine2 = new Line(GetSumPoint(titleBaseCenter2, -baseLineWidth / 2, 0), GetSumPoint(titleBaseCenter2, baseLineWidth / 2, 0));
            styleService.SetLayer(ref titleBaseLine2, layerService.LayerDimension);
            Text titleText = new Text(titleTextCenter, "NOZZLE PROJECTION", 4);
            titleText.Alignment = Text.alignmentType.BaselineCenter;
            titleText.ColorMethod = colorMethodType.byEntity;
            titleText.Color = Color.Yellow;
            titleList.AddRange(new Entity[] { titleBaseLine1, titleBaseLine2, titleText });

            newList.AddRange(dimList);
            newList.AddRange(centerLineList);
            newList.AddRange(textList);
            newList.AddRange(leaderList);
            newList.AddRange(titleList);



            BlockReference newBr = new BlockReference(selTable.Location.X, selTable.Location.Y, 0, "PAPER_TABLE" + selTable.No, 1, 1, 1, 0);

            Block newBl = new Block("PAPER_TABLE_" + selTable.No);
            newBl.Entities.AddRange(newList);
            selBlock = newBl;
            return newBr;

        }


        private BlockReference BuildPaperTableDripRing(PaperTableModel selTable, AssemblyModel assemblyData, out Block selBlock)
        {
            Point3D refPoint = new Point3D(0, 0);

            List<Entity> dripRingList = new List<Entity>();

            Point3D referencePoint = GetSumPoint(refPoint, 0, 0);

            ////////////////////////////////////////////// ///////////////////////////////////////////
            dripRingList.AddRange(GetDripRing(referencePoint));

            BlockReference newBr = new BlockReference(selTable.Location.X, selTable.Location.Y, 0, "PAPER_TABLE" + selTable.No, 1, 1, 1, 0);

            Block newBl = new Block("PAPER_TABLE_" + selTable.No);
            newBl.Entities.AddRange(dripRingList);
            selBlock = newBl;
            return newBr;

        }

        public List<Entity> GetDripRing(Point3D refPoint)
        {

            List<Entity> allOutLineList = new List<Entity>();
            List<Entity> outlinesList = new List<Entity>();
            List<Entity> concreteBoxleList = new List<Entity>();
            List<Entity> virtualPlateList = new List<Entity>();
            Point3D referencePoint = GetSumPoint(refPoint, 15, 20);

            //////////////////////////////////////////////////////////
            // CAD DATA
            //////////////////////////////////////////////////////////
            double concreteWidth = 25.2;
            double concreteLength = 40;

            double topPlateWidth = 13.9;
            double topPlateLength = 1.5;
            double topPlateXGap = 9.3;

            double bottomPlateWidth = 2;
            double bottomPlateLength = 15.3;
            double bottomPlateXGap = 3.6;

            double splineGap = 3;

            double DripRIngWidth = 0.5;
            double topDripRIngLength = 29;
            double BottomDripRIngLength = 3;


            //////////////////////////////////////////////////////////
            // Draw 
            //////////////////////////////////////////////////////////

            // Draw Boxes
            Point3D bottomPlateStartPoint = GetSumPoint(referencePoint, bottomPlateXGap, concreteWidth); ;
            List<Entity> bottomPlateList = GetRectangle(bottomPlateStartPoint, bottomPlateWidth, bottomPlateLength, RECTANGLUNVIEW.LEFT);
            virtualPlateList.AddRange(bottomPlateList);

            Point3D topPlateStartPoint = GetSumPoint(bottomPlateStartPoint, topPlateXGap, bottomPlateWidth); ;
            List<Entity> topPlateList = GetRectangle(topPlateStartPoint, topPlateWidth, topPlateLength, RECTANGLUNVIEW.TOP);
            virtualPlateList.AddRange(topPlateList);

            concreteBoxleList = GetRectangle(referencePoint, concreteWidth, concreteLength, RECTANGLUNVIEW.LEFT);
            outlinesList.AddRange(concreteBoxleList);


            // Draw Spline
            List<Point3D> splinePointList = new List<Point3D>();

            Line splineGuideLine = new Line(((Line)concreteBoxleList[2]).StartPoint, ((Line)concreteBoxleList[0]).StartPoint);
            Line splineTopGuide = new Line(splineGuideLine.StartPoint, splineGuideLine.MidPoint);
            Line splineBottomGuide = new Line(splineGuideLine.MidPoint, splineGuideLine.EndPoint);

            splinePointList.Add(GetSumPoint(splineTopGuide.StartPoint, 0, 0));
            splinePointList.Add(GetSumPoint(splineTopGuide.MidPoint, -splineGap, 0));
            splinePointList.Add(GetSumPoint(splineTopGuide.EndPoint, 0, 0));
            splinePointList.Add(GetSumPoint(splineBottomGuide.MidPoint, splineGap, 0));
            splinePointList.Add(GetSumPoint(splineBottomGuide.EndPoint, 0, 0));

            Curve bottomSpline = Curve.CubicSplineInterpolation(splinePointList);
            outlinesList.Add(bottomSpline);


            // Draw Drip Ring
            Point3D topDripRIngStartPoint = GetSumPoint(((Line)bottomPlateList[0]).EndPoint, 0, 0); ;
            List<Entity> topDripRIngList = GetRectangle(topDripRIngStartPoint, DripRIngWidth, topDripRIngLength);
            outlinesList.AddRange(topDripRIngList);

            Point3D bottomDripRIngStartPoint = GetSumPoint(((Line)topDripRIngList[0]).EndPoint, 0, 0); ;
            List<Entity> bottomDripRIngList = GetRectangleLT(bottomDripRIngStartPoint, BottomDripRIngLength, DripRIngWidth);
            outlinesList.AddRange(bottomDripRIngList);


            outlinesList.AddRange(new Entity[] {

            });

            styleService.SetLayerListLine(ref virtualPlateList, layerService.LayerVirtualLine);
            styleService.SetLayerListEntity(ref outlinesList, layerService.LayerOutLine);

            allOutLineList.AddRange(virtualPlateList);
            allOutLineList.AddRange(outlinesList);

            // Draw : Concrete detail - circle(DOT)
            List<Entity> newList = new List<Entity>();
            newList.AddRange(GetTriangle(GetSumPoint(referencePoint, 5, 6), 0.5, 45));
            newList.AddRange(GetTriangle(GetSumPoint(referencePoint, 10, 3), 0.5, 30));
            newList.AddRange(GetTriangle(GetSumPoint(referencePoint, 8, 20), 0.5, 90));
            newList.AddRange(GetTriangle(GetSumPoint(referencePoint, 20, 18), 0.5, 25));
            newList.AddRange(GetTriangle(GetSumPoint(referencePoint, 30, 5), 0.5, 110));
            newList.AddRange(GetTriangle(GetSumPoint(referencePoint, 35, 23), 0.5, 70));

            Point3D[] circlePointList = new Point3D[]{
                GetSumPoint(referencePoint,3,17),
                GetSumPoint(referencePoint,5,22),
                GetSumPoint(referencePoint,10,10),
                GetSumPoint(referencePoint,15,20),
                GetSumPoint(referencePoint,20,5),
                GetSumPoint(referencePoint,23,18),
                GetSumPoint(referencePoint,30,8),
                GetSumPoint(referencePoint,37,5),
                GetSumPoint(referencePoint,33,20),
            };
            newList.AddRange(GetManyCircles(referencePoint, 0.03, circlePointList.ToList()));

            styleService.SetLayerListEntity(ref newList, layerService.LayerOutLine);
            allOutLineList.AddRange(newList);



            // Title
            Point3D referencePointTitle = GetSumPoint(referencePoint, 35, -20);
            List<Entity> titleList = new List<Entity>();
            Point3D titleBaseCenter1 = GetSumPoint(referencePointTitle, 0, 7);
            Point3D titleBaseCenter2 = GetSumPoint(referencePointTitle, 0, 8);
            Point3D titleTextCenter = GetSumPoint(referencePointTitle, 0, 9);

            double baseLineWidth = 30;
            Line titleBaseLine1 = new Line(GetSumPoint(titleBaseCenter1, -baseLineWidth / 2, 0), GetSumPoint(titleBaseCenter1, baseLineWidth / 2, 0));
            styleService.SetLayer(ref titleBaseLine1, layerService.LayerOutLine);
            Line titleBaseLine2 = new Line(GetSumPoint(titleBaseCenter2, -baseLineWidth / 2, 0), GetSumPoint(titleBaseCenter2, baseLineWidth / 2, 0));
            styleService.SetLayer(ref titleBaseLine2, layerService.LayerDimension);
            Text titleText = new Text(titleTextCenter, "DRIP RING", 4);
            titleText.Alignment = Text.alignmentType.BaselineCenter;
            titleText.ColorMethod = colorMethodType.byEntity;
            titleText.Color = Color.Yellow;
            titleList.AddRange(new Entity[] { titleBaseLine1, titleBaseLine2, titleText });
            allOutLineList.AddRange(titleList);



            // Leader
            double scaleValue = 1;
            DrawBMLeaderModel leaderInfoModel2 = new DrawBMLeaderModel() { 
                position = POSITION_TYPE.RIGHT, 
                upperText = "SEAL WELDING", 
                textAlign = POSITION_TYPE.CENTER,
                leaderPointRadian=Utility.DegToRad(60),
                leaderPointLength= 20
                };
            DrawEntityModel leaderInfoList2 = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(topDripRIngStartPoint, 0, DripRIngWidth), leaderInfoModel2, scaleValue);
            allOutLineList.AddRange(leaderInfoList2.GetDrawEntity());

            string dripRingThickness = assemblyData.BottomInput[0].DripRingThickness;
            DrawBMLeaderModel leaderInfoModel3 = new DrawBMLeaderModel()
            {
                position = POSITION_TYPE.RIGHT,
                upperText = "t" + dripRingThickness +" DRIP RING",
                textAlign = POSITION_TYPE.CENTER,
                leaderPointRadian = Utility.DegToRad(60),
                leaderPointLength = 12};
            DrawEntityModel leaderInfoList3 = drawService.Draw_OneLineLeader(ref singleModel, GetSumPoint(topDripRIngStartPoint, topDripRIngLength*5/6, DripRIngWidth), leaderInfoModel3, scaleValue);
            allOutLineList.AddRange(leaderInfoList3.GetDrawEntity());


            double tempDimWidth01 = 7.9;
            DrawDimensionModel dimModelTop01 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.BOTTOM,
                textUpper = "75",
                textLower = "(MIN.)",
                extLineLeftVisible = false,

                dimHeight = 15,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntityTop01 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(topDripRIngStartPoint, topDripRIngLength- tempDimWidth01, 0), GetSumPoint(topDripRIngStartPoint, topDripRIngLength, -BottomDripRIngLength), scaleValue, dimModelTop01);
            allOutLineList.AddRange(dimEntityTop01.GetDrawEntity());

            DrawDimensionModel dimModelTop02 = new DrawDimensionModel()
            {
                position = POSITION_TYPE.RIGHT,
                textUpper = "25",
                textLower = "(MIN.)",
                arrowLeftHeadOut=true,
                arrowRightHeadOut=true,
                dimHeight = 15,
                scaleValue = scaleValue
            };
            DrawEntityModel dimEntityTop02 = drawService.Draw_DimensionDetail(ref singleModel, GetSumPoint(topDripRIngStartPoint, topDripRIngLength + DripRIngWidth, -BottomDripRIngLength), GetSumPoint(topDripRIngStartPoint, topDripRIngLength + DripRIngWidth,0), scaleValue, dimModelTop02);
            allOutLineList.AddRange(dimEntityTop02.GetDrawEntity());


            // S Line
            double sLineLength = bottomPlateWidth;

            List<Entity> newSLineList1 = drawService.breakService.GetSLine(GetSumPoint(topPlateStartPoint, 0, topPlateWidth), topPlateLength, true,0);
            List<Entity> newSLineList2 = drawService.breakService.GetSLine(GetSumPoint(bottomPlateStartPoint, 0, bottomPlateWidth), bottomPlateWidth, false, 90);
            styleService.SetLayerListEntity(ref newSLineList1, layerService.LayerVirtualLine);
            styleService.SetLayerListEntity(ref newSLineList2, layerService.LayerVirtualLine);
            allOutLineList.AddRange(newSLineList1);
            allOutLineList.AddRange(newSLineList2);

            return allOutLineList;
        }
        public enum RECTANGLUNVIEW { NONE, LEFT, RIGHT, TOP, BOTTOM, }
        public List<Entity> GetRectangle(Point3D startPoint, double width, double length, RECTANGLUNVIEW unViewPosition = RECTANGLUNVIEW.NONE)
        {
            // startPoint = LeftBottom point
            List<Entity> rectangleLineList = new List<Entity>();

            if (unViewPosition != RECTANGLUNVIEW.BOTTOM)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, 0), GetSumPoint(startPoint, length, 0)));
            if (unViewPosition != RECTANGLUNVIEW.RIGHT)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, length, width), GetSumPoint(startPoint, length, 0)));
            if (unViewPosition != RECTANGLUNVIEW.TOP)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, width), GetSumPoint(startPoint, length, width)));
            if (unViewPosition != RECTANGLUNVIEW.LEFT)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, width), GetSumPoint(startPoint, 0, 0)));

            return rectangleLineList;
        }
        public List<Entity> GetRectangleLT(Point3D startPoint, double width, double length, RECTANGLUNVIEW unViewPosition = RECTANGLUNVIEW.NONE)
        {
            // startPoint = LeftTop point
            List<Entity> rectangleLineList = new List<Entity>();

            if (unViewPosition != RECTANGLUNVIEW.BOTTOM)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, -width), GetSumPoint(startPoint, length, -width)));
            if (unViewPosition != RECTANGLUNVIEW.RIGHT)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, length, 0), GetSumPoint(startPoint, length, -width)));
            if (unViewPosition != RECTANGLUNVIEW.TOP)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, 0), GetSumPoint(startPoint, length, 0)));
            if (unViewPosition != RECTANGLUNVIEW.LEFT)
                rectangleLineList.Add(new Line(GetSumPoint(startPoint, 0, 0), GetSumPoint(startPoint, 0, -width)));

            return rectangleLineList;
        }
        public List<Entity> GetTriangle(Point3D refPoint, double length, double angle = 0)
        {
            Point3D RefPoint = GetSumPoint(refPoint, 0, 0);
            List<Entity> triangleList = new List<Entity>();

            Line bottomLine = new Line(GetSumPoint(RefPoint, 0, 0), GetSumPoint(RefPoint, length, 0));

            Line leftLine = new Line(GetSumPoint(RefPoint, 0, 0), GetSumPoint(RefPoint, length, 0));
            leftLine.Rotate(Utility.DegToRad(60), Vector3D.AxisZ, RefPoint);

            Line rightLine = new Line(GetSumPoint(RefPoint, 0, 0), GetSumPoint(bottomLine.EndPoint, 0, 0));
            rightLine.Rotate(Utility.DegToRad(-60), Vector3D.AxisZ, bottomLine.EndPoint);

            triangleList.AddRange(new Line[] {
                bottomLine, leftLine, rightLine
            });

            if (angle != 0)
            {
                foreach (Line eachLine in triangleList)
                {
                    eachLine.Rotate(Utility.DegToRad(angle), Vector3D.AxisZ, RefPoint);
                }
            }

            styleService.SetLayerListEntity(ref triangleList, layerService.LayerOutLine);
            return triangleList;
        }
        public List<Entity> GetManyCircles(Point3D refPoint, double Radius, List<Point3D> pointList)
        {
            List<Entity> circleList = new List<Entity>();

            foreach (Point3D eachPoint in pointList)
            {
                Circle eachCircle = new Circle(eachPoint, Radius);
                circleList.Add(eachCircle);
            }

            styleService.SetLayerListEntity(ref circleList, layerService.LayerOutLine);
            return circleList;
        }




        private BlockReference BuildPaperTableDirection(PaperTableModel selTable, AssemblyModel assemblyData, out Block selBlock)
        {



            List<Entity> newList = new List<Entity>();
            // Default
            Point3D referencePoint = new Point3D(0, 0);
            //List<Entity> tempList = CreateNozzleProjection(referencePoint);

            double circleBox = 40;
            double circleOD = 22.5;
            double circleRadius = circleOD / 2;

            Point3D circleCenterPoint = GetSumPoint(referencePoint, circleBox / 2, circleBox / 2);
            Circle centerCircle = new Circle(circleCenterPoint, circleRadius);
            newList.Add(centerCircle);


            Point3D circleCenterTopPoint = GetSumPoint(circleCenterPoint, 0, circleRadius);
            Line centerLine = new Line(GetSumPoint(circleCenterPoint, 0, circleRadius), GetSumPoint(circleCenterPoint, 0, -circleRadius));
            Line leftLine1 = (Line)centerLine.Clone();
            leftLine1.Rotate(Utility.DegToRad(15), Vector3D.AxisZ, circleCenterTopPoint);
            Line rightLine1 = (Line)centerLine.Clone();
            rightLine1.Rotate(-Utility.DegToRad(15), Vector3D.AxisZ, circleCenterTopPoint);

            Point3D leftLineIntetr = editingService.GetIntersectWidth(leftLine1, centerCircle, 1);
            Point3D rightLineIntetr = editingService.GetIntersectWidth(rightLine1, centerCircle, 1);

            Line centerSlope = new Line(GetSumPoint(circleCenterTopPoint, 0, 0), GetSumPoint(leftLineIntetr, 0, 0));
            centerSlope.Rotate(Utility.DegToRad(30), Vector3D.AxisZ, leftLineIntetr);
            Point3D centerLineIntetr = editingService.GetIntersectWidth(centerSlope, centerLine, 0);

            Line dLine1 = new Line(GetSumPoint(circleCenterTopPoint, 0, 0), GetSumPoint(leftLineIntetr, 0, 0));
            Line dLine2 = new Line(GetSumPoint(circleCenterTopPoint, 0, 0), GetSumPoint(rightLineIntetr, 0, 0));
            Line dLine3 = new Line(GetSumPoint(centerLineIntetr, 0, 0), GetSumPoint(leftLineIntetr, 0, 0));
            Line dLine4 = new Line(GetSumPoint(centerLineIntetr, 0, 0), GetSumPoint(rightLineIntetr, 0, 0));
            Line dLine5 = new Line(GetSumPoint(circleCenterTopPoint, 0, 0), GetSumPoint(centerLineIntetr, 0, 0));

            newList.AddRange(new Line[] { dLine1, dLine2, dLine3, dLine4 });


            styleService.SetLayerListEntity(ref newList, layerService.LayerDimension);



            // Center Line
            double exLength = 2;
            List<Entity> centerLineList = new List<Entity>();
            centerLineList.AddRange(editingService.GetCenterLine(GetSumPoint(circleCenterPoint, 0, circleRadius), GetSumPoint(circleCenterPoint, 0, -circleRadius), exLength, 1));
            centerLineList.AddRange(editingService.GetCenterLine(GetSumPoint(circleCenterPoint, circleRadius, 0), GetSumPoint(circleCenterPoint, -circleRadius, 0), exLength, 1));
            styleService.SetLayerListEntity(ref centerLineList, layerService.LayerCenterLine);
            newList.AddRange(centerLineList);

            // hatch
            Triangle cc = new Triangle(circleCenterTopPoint, leftLineIntetr, centerLineIntetr);

            //HatchRegion hatchDirection = new HatchRegion((ICurve)cc );
            //List<ICurve> hatchLine = new List<ICurve>();
            //hatchLine.AddRange(new Line[] { dLine1, dLine2, dLine5 });
            //HatchRegion hatchDirection = new HatchRegion(hatchLine, Plane.XY);
            //hatchDirection.Color = Color.White;
            //hatchDirection.HatchName = "aa";

            //CustomRenderedHatch hhh = new CustomRenderedHatch(hatchDirection);
            //newList.Add(hatchDirection);
            // Text

            double extLength = 4;
            double textHeight = 2.5;
            List<Entity> textList = new List<Entity>();
            Text Text01 = new Text(GetSumPoint(circleCenterPoint, 0, circleRadius + extLength), "0˚", textHeight);
            Text Text02 = new Text(GetSumPoint(circleCenterPoint, circleRadius + extLength + 2, 0), "90˚", textHeight);
            Text Text03 = new Text(GetSumPoint(circleCenterPoint, 0, -(circleRadius + extLength)), "180˚", textHeight);
            Text Text04 = new Text(GetSumPoint(circleCenterPoint, -(circleRadius + extLength + 2), 0), "270˚", textHeight);
            textList.AddRange(new Text[] { Text01, Text02, Text03, Text04 });
            foreach (Text eachText in textList)
            {
                eachText.ColorMethod = colorMethodType.byEntity;
                eachText.Color = Color.Yellow;
                eachText.LayerName = layerService.LayerDimension;
                eachText.Alignment = Text.alignmentType.MiddleCenter;
            }

            Text TextTop = new Text(GetSumPoint(circleCenterPoint, 0, circleRadius + extLength + 5), "P.N", 3.75);
            TextTop.Alignment = Text.alignmentType.MiddleCenter;
            styleService.SetLayer(ref TextTop, layerService.LayerDimension);
            textList.Add(TextTop);
            newList.AddRange(textList);



            BlockReference newBr = new BlockReference(selTable.Location.X, selTable.Location.Y, 0, "PAPER_TABLE" + selTable.No, 1, 1, 1, 0);

            Block newBl = new Block("PAPER_TABLE_" + selTable.No);
            newBl.Entities.AddRange(newList);
            selBlock = newBl;
            return newBr;

        }


        private BlockReference BuildPaperTableDia(PaperTableModel selTable, AssemblyModel assemblyData, out Block selBlock)
        {



            List<Entity> newList = new List<Entity>();
            // Default
            //List<Entity> tempList = CreateNozzleProjection(referencePoint);


            Point3D refPoint = new Point3D(0, 0);


            List<Entity> newTextList = new List<Entity>();
            List<Tuple<string, string>> textList = new List<Tuple<string, string>>();


            textList.Add(new Tuple<string, string>("SP", "ROOF STRUCTURE CENTERLINE"));
            textList.Add(new Tuple<string, string>("NP", "ROOF PLATE CENTERLINE"));
            textList.Add(new Tuple<string, string>("EL", "BOTTOM PLATE CENTERLINE"));
            textList.Add(new Tuple<string, string>("ST", "SPIRAL STARIRWAY START POINT"));
            textList.Add(new Tuple<string, string>("SS", "SETTLEMENT CHECK PIECE START POINT (10EA)"));
            textList.Add(new Tuple<string, string>("BC", "EARTH LUG START POINT (6EA)"));
            textList.Add(new Tuple<string, string>("RC", "NAME PLATE"));
            textList.Add(new Tuple<string, string>("RS", "1st COURSE SHELL PLATE START POINT"));


            double textHeight = 2.5;
            double cirDia = 11.5470;
            double cirRadius = cirDia / 2;
            double leftGap = 10;
            double currentY = cirRadius;
            foreach (Tuple<string, string> eachTuple in textList)
            {
                Point3D referencePoint = GetSumPoint(refPoint, cirRadius, currentY);
                Circle eachCircle = new Circle(GetSumPoint(referencePoint, 0, 0), cirRadius);
                Line vLine1 = new Line(GetSumPoint(referencePoint, 0, cirDia), GetSumPoint(referencePoint, 0, -cirDia));
                Line vLine2 = (Line)vLine1.Clone();
                Line vLine3 = (Line)vLine1.Clone();
                vLine1.Rotate(Utility.DegToRad(30), Vector3D.AxisZ, GetSumPoint(referencePoint, 0, 0));
                vLine2.Rotate(-Utility.DegToRad(30), Vector3D.AxisZ, GetSumPoint(referencePoint, 0, 0));
                vLine3.Rotate(-Utility.DegToRad(90), Vector3D.AxisZ, GetSumPoint(referencePoint, 0, 0));

                Point3D[] vInter1 = eachCircle.IntersectWith(vLine1);
                Point3D[] vInter2 = eachCircle.IntersectWith(vLine2);
                Point3D[] vInter3 = eachCircle.IntersectWith(vLine3);

                Text eachText = new Text(GetSumPoint(referencePoint, 0, 0), eachTuple.Item1, 3.5);
                eachText.Alignment = Text.alignmentType.MiddleCenter;
                styleService.SetLayer(ref eachText, layerService.LayerDimension);
                newTextList.Add(eachText);

                Text eachText2 = new Text(GetSumPoint(referencePoint, leftGap, 0), eachTuple.Item2, textHeight);
                eachText2.Alignment = Text.alignmentType.MiddleLeft;
                styleService.SetLayer(ref eachText2, layerService.LayerDimension);
                newTextList.Add(eachText2);

                newList.Add(new Line(GetSumPoint(vInter1[0], 0, 0), GetSumPoint(vInter2[0], 0, 0)));
                newList.Add(new Line(GetSumPoint(vInter2[0], 0, 0), GetSumPoint(vInter3[1], 0, 0)));
                newList.Add(new Line(GetSumPoint(vInter3[1], 0, 0), GetSumPoint(vInter1[1], 0, 0)));
                newList.Add(new Line(GetSumPoint(vInter1[1], 0, 0), GetSumPoint(vInter2[1], 0, 0)));
                newList.Add(new Line(GetSumPoint(vInter2[1], 0, 0), GetSumPoint(vInter3[0], 0, 0)));
                newList.Add(new Line(GetSumPoint(vInter3[0], 0, 0), GetSumPoint(vInter1[0], 0, 0)));

                styleService.SetLayerListEntity(ref newList, layerService.LayerDimension);
                newList.AddRange(newTextList);

                currentY += cirDia + 0.5;
            }






            BlockReference newBr = new BlockReference(selTable.Location.X, selTable.Location.Y, 0, "PAPER_TABLE" + selTable.No, 1, 1, 1, 0);

            Block newBl = new Block("PAPER_TABLE_" + selTable.No);
            newBl.Entities.AddRange(newList);
            selBlock = newBl;
            return newBr;

        }

        private BlockReference BuildPaperTableGADesign(PaperTableModel selTable, AssemblyModel assemblyData, out Block selBlock)
        {
            //int rowCount = selTable.TableList.Count;
            //int columnCount = selTable.TableList[0].Length;

            List<string> AMEDATA = new List<string>();
            foreach(AMEDataModel eachValue in assemblyData.AMEDataList)
            {
                string oneValue = eachValue.AMEValue;
                if (oneValue == "")
                    oneValue = eachValue.Default;

                AMEDATA.Add(oneValue);
            }



            double fontHeight = 2.5;
            double fontHeight2 = 2;
            double fontHeightTitle = 4;
            double fontHeightTitleSub = 3;


            List<Entity> newList = new List<Entity>();
            // Default

            double tableOneWidth = 40;
            double tableTwoWidth = 135;
            double tableWidth = tableOneWidth + tableTwoWidth;
            double tableHeight = 288;

            double titleHeight = 10;
            double titleSubHeight = 7;
            double rowHeight = 6;

            double currentY = 0;

            double leftTextGap = 2;





            Point3D refPoint = new Point3D(0, tableHeight);

            // 왼쪽 세로
            newList.Add(GetNewLineYellow(GetSumPoint(refPoint, 0, 0), GetSumPoint(refPoint, 0, -tableHeight)));

            currentY -= titleHeight;
            newList.Add(GetNewLineYellow(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextYellow(GetSumPoint(refPoint, tableWidth / 2, currentY + titleHeight / 2), "TANK SPECIFICATION DATA", fontHeightTitle, 1, Text.alignmentType.MiddleCenter));


            currentY -= titleSubHeight;
            newList.Add(GetNewLineYellow(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextYellow(GetSumPoint(refPoint, tableWidth / 2, currentY + titleSubHeight / 2), "1. DESIGN DATA", fontHeightTitleSub, 1, Text.alignmentType.MiddleCenter));


            // 세로선 23개
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableOneWidth, currentY - rowHeight * 23)));

            // 데이터 시작
            currentY -= rowHeight;

            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 107.5, currentY), GetSumPoint(refPoint, 107.5, currentY + rowHeight)));
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 132.5, currentY), GetSumPoint(refPoint, 132.5, currentY + rowHeight)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "APPLIED CODE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 67.5 / 2, currentY + rowHeight / 2), assemblyData.GeneralDesignData[0].CodeApplied, fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + 107.5, currentY + rowHeight / 2), "CONTENTS", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, 132.5 + 42.5 / 2, currentY + rowHeight / 2), assemblyData.GeneralDesignData[0].Contents, fontHeight, 0.95, Text.alignmentType.MiddleCenter));




            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "SIZE (mm)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "INSIDE DIA.", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 25, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 25 + 42.5 / 2, currentY + rowHeight / 2), assemblyData.GeneralDesignData[0].SizeNominalID, fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + 107.5, currentY + rowHeight / 2), "HEIGHT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, 132.5, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, 132.5 + 42.5 / 2, currentY + rowHeight / 2), assemblyData.GeneralDesignData[0].SizeTankHeight, fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "CAPACITY (m3)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "NOMINAL", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 25, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 25 + 42.5 / 2, currentY + rowHeight / 2), assemblyData.GeneralLiquidCapacityWeight[0].NominalCapacity, fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + 107.5, currentY + rowHeight / 2), "WORKING", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, 132.5, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, 132.5 + 42.5 / 2, currentY + rowHeight / 2), assemblyData.GeneralLiquidCapacityWeight[0].WorkingCapacity, fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "PUMPING RATES", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "IN", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 25, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 25 + 42.5 / 2, currentY + rowHeight / 2), assemblyData.GeneralDesignData[0].PumpingRatesIn, fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + 107.5, currentY + rowHeight / 2), "OUT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, 132.5, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, 132.5 + 42.5 / 2, currentY + rowHeight / 2), assemblyData.GeneralDesignData[0].PumpingRatesOut, fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 51, currentY), GetSumPoint(refPoint, tableOneWidth + 51, currentY + rowHeight)));
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 81, currentY), GetSumPoint(refPoint, tableOneWidth + 81, currentY + rowHeight)));


            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "OPERATING TEMP.", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "(MAX.)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 25 + 26 / 2, currentY + rowHeight / 2), assemblyData.GeneralDesignData[0].OperTempMax, fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 51, currentY + rowHeight / 2), "DESIGN TEMP", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 81, currentY + rowHeight / 2), "(MIN./MAX.)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 106 + 29 / 2, currentY + rowHeight / 2), assemblyData.GeneralDesignData[0].DesignTempMin + "/" + assemblyData.GeneralDesignData[0].DesignTempMax, fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 39, currentY), GetSumPoint(refPoint, tableOneWidth + 39, currentY + rowHeight)));
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 67.5, currentY), GetSumPoint(refPoint, tableOneWidth + 67.5, currentY + rowHeight)));
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 81, currentY), GetSumPoint(refPoint, tableOneWidth + 81, currentY + rowHeight)));
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 115, currentY), GetSumPoint(refPoint, tableOneWidth + 115, currentY + rowHeight)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "OPERATING PRESS.", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 39 / 2, currentY + rowHeight / 2), assemblyData.GeneralDesignData[0].OperPressInt + "/" + assemblyData.GeneralDesignData[0].OperPressExt, fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 39, currentY + rowHeight / 2), "TEST SP. GR.", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 67.5 + 13.5 / 2, currentY + rowHeight / 2), assemblyData.GeneralDesignData[0].TestSPGR, fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 81, currentY + rowHeight / 2), "DESIGN SP. GR.", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 115 + 20 / 2, currentY + rowHeight / 2), assemblyData.GeneralDesignData[0].DesignSpecGR, fontHeight, 0.95, Text.alignmentType.MiddleCenter));




            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 51, currentY), GetSumPoint(refPoint, tableOneWidth + 51, currentY + rowHeight)));
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 81, currentY), GetSumPoint(refPoint, tableOneWidth + 81, currentY + rowHeight)));


            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "VAPOR PRESSURE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 51 / 2, currentY + rowHeight / 2), assemblyData.GeneralDesignData[0].VaporPressureMax, fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 51, currentY + rowHeight / 2), "DESIGN PRESS.", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 81 + 54 / 2, currentY + rowHeight / 2), assemblyData.GeneralDesignData[0].DesignPressInt + "/" + assemblyData.GeneralDesignData[0].DesignPressExt, fontHeight, 0.95, Text.alignmentType.MiddleCenter));




            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            // 3행
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY - rowHeight * 2 + rowHeight * 3 / 2), "SET PRESSURE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "EMERGENCY COVER MANHOLE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 60, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 60 + 75 / 2, currentY + rowHeight / 2), assemblyData.GeneralDesignData[0].SetPressureEmerCoverManhole, fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "BREATHER VALVE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 60, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 60 + 75 / 2, currentY + rowHeight / 2), assemblyData.GeneralDesignData[0].SetPressureBreatherValve, fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "BREATHER VALVE (VAC.)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 60, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 60 + 75 / 2, currentY + rowHeight / 2), assemblyData.GeneralDesignData[0].SetPressureBreatherValveVac, fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            // 2행
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY - rowHeight * 1 + rowHeight * 2 / 2), "CORR. ALLOW. (mm)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "SHELL", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 25, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 25 + 20 / 2, currentY + rowHeight / 2), assemblyData.GeneralCorrosionLoading[0].ShellPlate, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 45, currentY + rowHeight / 2), "ROOF", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70 + 20 / 2, currentY + rowHeight / 2), assemblyData.GeneralCorrosionLoading[0].RoofPlate, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 90, currentY + rowHeight / 2), "BOTTOM", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 115, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 115 + 20 / 2, currentY + rowHeight / 2), assemblyData.GeneralCorrosionLoading[0].BottomPlate, fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "NOZZLE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 25, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 25 + 20 / 2, currentY + rowHeight / 2), assemblyData.GeneralCorrosionLoading[0].Nozzle, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 45, currentY + rowHeight / 2), "STRUCTURE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70 + 20 / 2, currentY + rowHeight / 2), assemblyData.GeneralCorrosionLoading[0].StructureEachSide, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 90, currentY + rowHeight / 2), "(EACH SIDE)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "SHELL DESIGN", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 135 / 2, currentY + rowHeight / 2), assemblyData.GeneralMaterialSpecifications[0].ShellPlate, fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "ROOF DESIGN", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 135 / 2, currentY + rowHeight / 2), assemblyData.GeneralMaterialSpecifications[0].RoofPlate, fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            // 2행
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY - rowHeight * 1 + rowHeight * 2 / 2), "ROOF LOADS", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "UNIFORM LIVE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70 + 65 / 2, currentY + rowHeight / 2), assemblyData.GeneralDesignData[0].RoofLoadsUniformLive, fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "SPECIAL LOADING (PROVIDE SKETCH)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70 + 65 / 2, currentY + rowHeight / 2), assemblyData.GeneralDesignData[0].RoofLoadsSpecialLoading, fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            // 2행
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY - rowHeight * 1 + rowHeight * 2 / 2), "EARTHQUAKE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), assemblyData.GeneralSeismic[0].CodeApplied, fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 70, currentY + rowHeight / 2), "SEISMIC USE GROUP", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70 + 42, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70 + 42 + 21 / 2, currentY + rowHeight / 2), assemblyData.GeneralSeismic[0].SeismicUseGroup, fontHeight, 0.95, Text.alignmentType.MiddleCenter));





            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "Ss(g)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 15, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 15 + 21 / 2, currentY + rowHeight / 2), assemblyData.GeneralSeismic[0].Ss, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 36, currentY + rowHeight / 2), "S1(g)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 51, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 51 + 21 / 2, currentY + rowHeight / 2), assemblyData.GeneralSeismic[0].S1, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 70, currentY + rowHeight / 2), "IMPORTANCE FACTOR", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70 + 42, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70 + 42 + 21 / 2, currentY + rowHeight / 2), assemblyData.GeneralSeismic[0].ImportanceFactor, fontHeight, 0.95, Text.alignmentType.MiddleCenter));





            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            // 2행
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY - rowHeight * 1 + rowHeight * 2 / 2), "WIND", fontHeight, 0.95, Text.alignmentType.MiddleLeft));


            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), assemblyData.GeneralWind[0].CodeApplied, fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 60 + 75 / 2, currentY + rowHeight / 2), assemblyData.GeneralWind[0].DesignWindSpeed, fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "EXPOSURE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 25, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 25 + 35 / 2, currentY + rowHeight / 2), assemblyData.GeneralWind[0].Exposure, fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 60, currentY + rowHeight / 2), "IMPORTANCE FACTOR", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 103, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 103 + 32 / 2, currentY + rowHeight / 2), assemblyData.GeneralWind[0].ImportanceFactor, fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "SNOW FALL", fontHeight, 0.95, Text.alignmentType.MiddleLeft));


            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "TOTAL ACCUMULATION", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 44, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 44 + 28 / 2, currentY + rowHeight / 2), assemblyData.GeneralDesignData[0].SnowFallTotalAccumulation, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 44 + 28, currentY + rowHeight / 2), "RAIN FALL", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 44 + 28 + 24, currentY + rowHeight / 2), "(MAX.)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 44 + 28 + 24 + 13 + 26 / 2, currentY + rowHeight / 2), assemblyData.GeneralDesignData[0].RainFallMax, fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 44 + 28, currentY), GetSumPoint(refPoint, tableOneWidth + 44 + 28, currentY + rowHeight)));
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 44 + 28 + 24, currentY), GetSumPoint(refPoint, tableOneWidth + 44 + 28 + 24, currentY + rowHeight)));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "FOUNDATION TYPE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));


            // 사각형
            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth , currentY + 1.2), assemblyData.GeneralDesignData[0].FoundationType.ToLower().Contains("earth")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 5.5, currentY + rowHeight / 2), "EARTH", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth +30 , currentY + 1.2), assemblyData.GeneralDesignData[0].FoundationType.ToLower().Contains("ring")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth +30+ 5.5, currentY + rowHeight / 2), "CONCRETE RING WALL", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 90, currentY + 1.2), assemblyData.GeneralDesignData[0].FoundationType.ToLower().Contains("mat")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 90 + 5.5, currentY + rowHeight / 2), "CONCRETE MAT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));


            currentY -= rowHeight;
            newList.Add(GetNewLineYellow(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "ESTIMATED WEIGHT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));



            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "EMPTY", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 16, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 16 + 24 / 2, currentY + rowHeight / 2), assemblyData.GeneralLiquidCapacityWeight[0].Empty, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 40, currentY + rowHeight / 2), "OPER.", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 56, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 56 + 24 / 2, currentY + rowHeight / 2), assemblyData.GeneralLiquidCapacityWeight[0].Operating, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 80, currentY + rowHeight / 2), "FULL OF WATER", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 111, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 111 + 24 / 2, currentY + rowHeight / 2), assemblyData.GeneralLiquidCapacityWeight[0].fullOfWater, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 40, currentY), GetSumPoint(refPoint, tableOneWidth + 40, currentY + rowHeight)));
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 80, currentY), GetSumPoint(refPoint, tableOneWidth + 80, currentY + rowHeight)));



            // 두번째 시작
            currentY -= titleSubHeight;
            newList.Add(GetNewLineYellow(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextYellow(GetSumPoint(refPoint, tableWidth / 2, currentY + titleSubHeight / 2), "2. CONSTRUCTION DETAILS", fontHeightTitleSub, 1, Text.alignmentType.MiddleCenter));



            // 세로선 23개
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableOneWidth, currentY - rowHeight * 21)));

            // 데이터 시작


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            // 6행
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY - rowHeight * 5 + rowHeight * 6 / 2), "MATERIAL SPECS.", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "SHELL", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 17, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 17 + 59 / 2, currentY + rowHeight / 2), assemblyData.GeneralMaterialSpecifications[0].ShellPlate, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 76, currentY + rowHeight / 2), "COLUMN", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 100, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 100 + 35 / 2, currentY + rowHeight / 2), assemblyData.GeneralMaterialSpecifications[0].Column, fontHeight, 0.95, Text.alignmentType.MiddleCenter));




            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "BOTTOM", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 17, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 17 + 59 / 2, currentY + rowHeight / 2), assemblyData.GeneralMaterialSpecifications[0].BottomPlate, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 76, currentY + rowHeight / 2), "ANNULAR", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 100, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 100 + 35 / 2, currentY + rowHeight / 2), assemblyData.GeneralMaterialSpecifications[0].AnnularPlate, fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "ROOF", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 17, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 17 + 59 / 2, currentY + rowHeight / 2), assemblyData.GeneralMaterialSpecifications[0].RoofPlate, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 76, currentY + rowHeight / 2), "STRUCTURE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 100, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 100 + 35 / 2, currentY + rowHeight / 2), assemblyData.GeneralMaterialSpecifications[0].RoofStructure, fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "FLANGE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 17, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 17 + 59 / 2, currentY + rowHeight / 2), assemblyData.GeneralMaterialSpecifications[0].PlateFlangeCovers, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 76, currentY + rowHeight / 2), "GASKET", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 100, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 100 + 35 / 2, currentY + rowHeight / 2), assemblyData.GeneralMaterialSpecifications[0].Gasket, fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "BOLT/NUT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 21, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 21 + 42.5 / 2, currentY + rowHeight / 2), assemblyData.GeneralMaterialSpecifications[0].BoltNut, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 21 + 42.5, currentY + rowHeight / 2), "NOZZLE NECK", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 21 + 42.5 + 28.2, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 21 + 42.5 + 28.2 + 43.5 / 2, currentY + rowHeight / 2), assemblyData.GeneralMaterialSpecifications[0].NozzleNeckPipe, fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "ANCHOR BOLT/NUT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 40, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 40 + 95 / 2, currentY + rowHeight / 2), assemblyData.GeneralMaterialSpecifications[0].AnchorBlot + "/" + assemblyData.GeneralMaterialSpecifications[0].AnchorNut, fontHeight, 0.95, Text.alignmentType.MiddleCenter));




            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));


            // 2행
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY - rowHeight * 1 + rowHeight * 2 / 2), "BOTTOM PLATE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "TH'K", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 25 / 2, currentY + rowHeight / 2), assemblyData.GeneralMaterialSpecifications[0].BottomPlateThickness, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 23 + 25, currentY + 1.2), assemblyData.GeneralMaterialSpecifications[0].BottomPlateWeldJointType.ToLower().Contains("lap")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 23 + 25 + 5.5, currentY + rowHeight / 2), "LAP JOINT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 84, currentY + 1.2), assemblyData.GeneralMaterialSpecifications[0].BottomPlateWeldJointType.ToLower().Contains("butt")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 84 + 5.5, currentY + rowHeight / 2), "BUTT JOINT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "SLOPE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 25 / 2, currentY + rowHeight / 2), assemblyData.GeneralMaterialSpecifications[0].BottomPlateSlope, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 23 + 25, currentY + 1.2), assemblyData.GeneralMaterialSpecifications[0].BottomPlateBottomStyle.ToLower().Contains("up")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 23 + 25 + 5.5, currentY + rowHeight / 2), "CONED UP", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 84, currentY + 1.2), assemblyData.GeneralMaterialSpecifications[0].BottomPlateBottomStyle.ToLower().Contains("down")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 84 + 5.5, currentY + rowHeight / 2), "CONED DOWN", fontHeight, 0.95, Text.alignmentType.MiddleLeft));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "ANNULAR PLATE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "MIN. WIDTH", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 25 / 2, currentY + rowHeight / 2), assemblyData.GeneralMaterialSpecifications[0].AnnularPlateMinWidth, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 23 + 25, currentY + rowHeight / 2), "TH'K", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 25 + 12, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 25 + 12 + 13 / 2, currentY + rowHeight / 2), assemblyData.GeneralMaterialSpecifications[0].AnnularPlateThickness, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 73, currentY + 1.2), assemblyData.GeneralMaterialSpecifications[0].AnnularPlateWeldJointType.ToLower().Contains("butt")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 73 + 5.5, currentY + rowHeight / 2), "BUTT JOINT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 103, currentY + 1.2), assemblyData.GeneralMaterialSpecifications[0].AnnularPlateWeldJointType.ToLower().Contains("lap")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 103 + 5.5, currentY + rowHeight / 2), "LAP JOINT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "ROOF PLATE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "TH'K", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 50 / 2, currentY + rowHeight / 2), assemblyData.GeneralMaterialSpecifications[0].RoofPlateThickness, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 73, currentY + 1.2), assemblyData.GeneralMaterialSpecifications[0].RoofPlateWeldJointType.ToLower().Contains("butt joint")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 73 + 5.5, currentY + rowHeight / 2), "BUTT JOINT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));


            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 103, currentY + 1.2), assemblyData.GeneralMaterialSpecifications[0].RoofPlateWeldJointType.ToLower().Contains("lap joint")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 103 + 5.5, currentY + rowHeight / 2), "LAP JOINT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "FIXED ROOF", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "SLOPE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 25 / 2, currentY + rowHeight / 2), assemblyData.GeneralMaterialSpecifications[0].RoofPlateSlopeRadiusRatio, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 23 + 25, currentY +1.2 ), AMEDATA[65].ToLower().Contains("column")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 23+25 +5.5, currentY + rowHeight / 2), "COLUMN", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 73, currentY + 1.2), AMEDATA[65].ToLower().Contains("truss")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 73 + 5.5, currentY + rowHeight / 2), "TRUSS", fontHeight, 0.95, Text.alignmentType.MiddleLeft));


            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 93, currentY + 1.2), AMEDATA[65].ToLower().Contains("supported")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 93 + 5.5, currentY + rowHeight / 2), "SELF SUPPORTED", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));


            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "NDE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 135 / 2, currentY + rowHeight / 2), AMEDATA[66], fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            // 3행
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY - rowHeight * 2 + rowHeight * 3 / 2), "LEAK TESTING", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "BOTTOM", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 112 / 2, currentY + rowHeight / 2), AMEDATA[67], fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "ROOF", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 112 / 2, currentY + rowHeight / 2), AMEDATA[68], fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "SHELL", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 112 / 2, currentY + rowHeight / 2), AMEDATA[69], fontHeight, 0.95, Text.alignmentType.MiddleCenter));




            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "STRESS RELIEF", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 135 / 2, currentY + rowHeight / 2), AMEDATA[70], fontHeight, 0.95, Text.alignmentType.MiddleCenter));





            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));


            // 4행
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY - rowHeight * 3 + rowHeight * 4 / 2), "PAINTING/COATING", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "INTERNAL", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 112 / 2, currentY + rowHeight / 2), assemblyData.GeneralLiquidCapacityWeight[0].PaintingAreaInt, fontHeight, 0.95, Text.alignmentType.MiddleCenter));




            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 135 / 2, currentY + rowHeight / 2), "-", fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "EXTERNAL", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 112 / 2, currentY + rowHeight / 2), assemblyData.GeneralLiquidCapacityWeight[0].PaintingAreaExt, fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 135 / 2, currentY + rowHeight / 2), "-", fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineYellow(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "INSULATION (mm)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            string insulationType = "";
            if (assemblyData.GeneralMaterialSpecifications[0].InsulationType != "")
                insulationType = "(" + assemblyData.GeneralMaterialSpecifications[0].InsulationType + ")";
            string insShellThickness = assemblyData.GeneralMaterialSpecifications[0].InsulationShellThickness.Trim().ToLower().Replace("none","");
            string insRoofThickness = assemblyData.GeneralMaterialSpecifications[0].InsulationRoofThickness.Trim().ToLower().Replace("none", "");
            if (insShellThickness != "")
                insShellThickness = insShellThickness + " " + insulationType;
            else
                insShellThickness = "NONE";

            if (insRoofThickness != "")
                insRoofThickness = insRoofThickness + " " + insulationType;
            else
                insRoofThickness = "NONE";

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "SHELL", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 44 / 2, currentY + rowHeight / 2), insShellThickness, fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 23 + 44, currentY + rowHeight / 2), "ROOF", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 44 + 23, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 44 + 23 + 45 / 2, currentY + rowHeight / 2), insRoofThickness, fontHeight, 0.95, Text.alignmentType.MiddleCenter));




            BlockReference newBr = new BlockReference(selTable.Location.X, selTable.Location.Y, 0, "PAPER_TABLE" + selTable.No, 1, 1, 1, 0);

            Block newBl = new Block("PAPER_TABLE_" + selTable.No);
            newBl.Entities.AddRange(newList);
            selBlock = newBl;
            return newBr;

        }
        #endregion

        private List<Entity> GetSquare(Point3D selPoint, bool check = false)
        {
            List<Entity> newList = new List<Entity>();

            double tableWidth = 3.5;
            newList.Add(GetNewLineWhite(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, tableWidth, 0)));
            newList.Add(GetNewLineWhite(GetSumPoint(selPoint, 0, tableWidth), GetSumPoint(selPoint, tableWidth, tableWidth)));
            newList.Add(GetNewLineWhite(GetSumPoint(selPoint, tableWidth, 0), GetSumPoint(selPoint, tableWidth, tableWidth)));
            newList.Add(GetNewLineWhite(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, 0, tableWidth)));

            if (check)
            {
                newList.Add(GetNewLineWhite(GetSumPoint(selPoint, 0, 0), GetSumPoint(selPoint, tableWidth, tableWidth)));
                newList.Add(GetNewLineWhite(GetSumPoint(selPoint, 0, tableWidth), GetSumPoint(selPoint, tableWidth, 0)));
            }

            return newList;
        }


        private Text GetNewTextWhite(Point3D selPoint, string selName,double fontHeight,double WidhtF, Text.alignmentType selAlign)
        {
            return new Text(selPoint, selName, fontHeight)
            {
                Alignment = selAlign,
                StyleName = textService.TextROMANS,
                LayerName = layerService.LayerDimension,
                ColorMethod = colorMethodType.byLayer,
                LineTypeMethod = colorMethodType.byLayer,
                WidthFactor = WidhtF
            };

        }

        private Text GetNewTextYellow(Point3D selPoint, string selName, double fontHeight, double WidhtF, Text.alignmentType selAlign)
        {
            return new Text(selPoint, selName, fontHeight)
            {
                Alignment = selAlign,
                StyleName = textService.TextROMANS,
                LayerName = layerService.LayerDimension,
                ColorMethod = colorMethodType.byEntity,
                Color = Color.Yellow,
                LineTypeMethod = colorMethodType.byLayer,
                WidthFactor = WidhtF
            };

        }

        private Line GetNewLineYellow(Point3D selPoint1, Point3D selPoint2)
        {
            return new Line(selPoint1,selPoint2) {
                Color = Color.Yellow, 
                ColorMethod = colorMethodType.byEntity, 
                LineTypeMethod = colorMethodType.byLayer, 
                LayerName = layerService.LayerDimension 
            };
        }
        private Line GetNewLineWhite(Point3D selPoint1, Point3D selPoint2)
        {
            return new Line(selPoint1, selPoint2)
            {
                Color = Color.White,
                ColorMethod = colorMethodType.byEntity,
                LineTypeMethod = colorMethodType.byLayer,
                LayerName = layerService.LayerDimension
            };
        }

        private Point3D GetSumPoint(Point3D selPoint1, double X, double Y, double Z = 0)
        {
            return new Point3D(selPoint1.X + X, selPoint1.Y + Y, selPoint1.Z + Z);
        }


        #region Automatic Alignment
        private void AutomaticAlignmentOfDock(int selIndex)
        {
            // ActiveSheet

            List<string> bNameList = oneSheetBlock.Keys.ToList();
            List<DockModel> dockList = oneSheetBlock.Values.ToList();

            Dictionary<string, int> leftDic = new Dictionary<string, int>();
            Dictionary<string, int> rightDic = new Dictionary<string, int>();
            Dictionary<string, int> topDic = new Dictionary<string, int>();
            Dictionary<string, int> bottomDic = new Dictionary<string, int>();
            Dictionary<string, int> bottomRightDic = new Dictionary<string, int>();
            Dictionary<string, int> floatDic = new Dictionary<string, int>();
            int dicCount = -1;
            foreach (DockModel eachDock in dockList)
            {
                dicCount++;
                switch (eachDock.DockPosition)
                {
                    case DOCKPOSITION_TYPE.TOP:
                        topDic.Add( bNameList[dicCount], eachDock.DockPriority);
                        break;
                    case DOCKPOSITION_TYPE.LEFT:
                        leftDic.Add(bNameList[dicCount], eachDock.DockPriority);
                        break;
                    case DOCKPOSITION_TYPE.RIGHT:
                        rightDic.Add(bNameList[dicCount], eachDock.DockPriority);
                        break;
                    case DOCKPOSITION_TYPE.BOTTOM:
                        if (eachDock.HorizontalAlignment == HORIZONTALALIGNMENT_TYPE.LEFT)
                        {
                            bottomDic.Add(bNameList[dicCount], eachDock.DockPriority);
                        }
                        else if (eachDock.HorizontalAlignment == HORIZONTALALIGNMENT_TYPE.RIGHT)
                        {
                            bottomRightDic.Add(bNameList[dicCount], eachDock.DockPriority);
                        }
                        break;
                    case DOCKPOSITION_TYPE.FLOATING:
                        floatDic.Add(bNameList[dicCount], eachDock.DockPriority);
                        break;
                }
            }

            // right
            var rightDicOrderby= rightDic.OrderBy(num => num.Value).ToDictionary(pair=>pair.Key,pair=>pair.Value);
            double rightTopX = 841 - 7;
            double rightTopY = 594 - 7;


            foreach (string eachName in rightDicOrderby.Keys)
            {
                foreach (Entity eachEntity in singleDraw.Sheets[selIndex].Entities)
                {
                    string eachType = eachEntity.GetType().Name;
                    if (eachType.Contains("BlockReference"))
                    {
                        BlockReference selBlock = eachEntity as BlockReference;
                        selBlock.Regen(new RegenParams(0, singleDraw));
                        if (eachName == selBlock.BlockName)
                        {
                            if (eachName.Contains("NOTE"))
                                rightTopY -= 0.5;
                            rightTopY -= selBlock.BoxSize.Y;

                            // 임시적용
                            selBlock.InsertionPoint = new Point3D(rightTopX -selBlock.BoxSize.X, rightTopY);

                            if(eachName.Contains("995"))
                                selBlock.InsertionPoint = new Point3D(rightTopX - 281, rightTopY-15);



                        }
                    }
                    else if (eachType.Contains("VectorView"))
                    {
                        VectorView selView = eachEntity as VectorView;
                        if (eachName == selView.BlockName)
                        {
                            rightTopY -= selView.BoxSize.Y;
                            selView.InsertionPoint = new Point3D(rightTopX+selView.BoxSize.X, rightTopY);
                        }
                    }

                }
            }


            // bottom
            var bottomDicOrderby = bottomDic.OrderBy(num => num.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            double bottomLeftX = 7;
            double bottomLeftY = 7;


            foreach (string eachName in bottomDicOrderby.Keys)
            {
                foreach (Entity eachEntity in singleDraw.Sheets[selIndex].Entities)
                {
                    string eachType = eachEntity.GetType().Name;
                    if (eachType.Contains("BlockReference"))
                    {
                        BlockReference selBlock = eachEntity as BlockReference;
                        selBlock.Regen(new RegenParams(0, singleDraw));
                            //newText01.Regen(new RegenParams(0, ssModel));
                        if (eachName == selBlock.BlockName)
                        {
                            selBlock.InsertionPoint = new Point3D(bottomLeftX, bottomLeftY);
                            bottomLeftX += selBlock.BoxSize.X;
                        }
                    }
                    else if (eachType.Contains("VectorView"))
                    {
                        VectorView selView = eachEntity as VectorView;
                        if (eachName == selView.BlockName)
                        {
                            selView.InsertionPoint = new Point3D(bottomLeftX + selView.BoxSize.X, bottomLeftY);
                            bottomLeftY = selView.BoxSize.Y;
                            bottomLeftX += selView.BoxSize.X;
                        }
                    }

                }
            }

            var bottomRightDicOrderby = bottomRightDic.OrderBy(num => num.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            double bottomRightX =841 -166 - 7;
            double bottomRightY = 7;


            foreach (string eachName in bottomRightDicOrderby.Keys)
            {
                foreach (Entity eachEntity in singleDraw.Sheets[selIndex].Entities)
                {
                    string eachType = eachEntity.GetType().Name;
                    if (eachType.Contains("BlockReference"))
                    {
                        BlockReference selBlock = eachEntity as BlockReference;
                        selBlock.Regen(new RegenParams(0, singleDraw));
                        //newText01.Regen(new RegenParams(0, ssModel));
                        if (eachName == selBlock.BlockName)
                        {
                            selBlock.InsertionPoint = new Point3D(bottomRightX- selBlock.BoxSize.X, bottomRightY);
                            bottomRightX += selBlock.BoxSize.X;
                        }
                    }
                    else if (eachType.Contains("VectorView"))
                    {
                        VectorView selView = eachEntity as VectorView;
                        if (eachName == selView.BlockName)
                        {
                            selView.InsertionPoint = new Point3D(bottomRightX + selView.BoxSize.X, bottomRightY);
                            bottomRightY = selView.BoxSize.Y;
                            bottomRightX += selView.BoxSize.X;
                        }
                    }

                }
            }

            //Float
            var floatDicOrderby = floatDic.OrderBy(num => num.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            double topLeftX = 7;
            double topLeftY = 7;


            foreach (string eachName in floatDicOrderby.Keys)
            {
                foreach (Entity eachEntity in singleDraw.Sheets[selIndex].Entities)
                {
                    string eachType = eachEntity.GetType().Name;
                    if (eachType.Contains("BlockReference"))
                    {
                        BlockReference selBlock = eachEntity as BlockReference;
                        selBlock.Regen(new RegenParams(0, singleDraw));
                        //newText01.Regen(new RegenParams(0, ssModel));
                        if (eachName == selBlock.BlockName)
                        {
                            //selBlock.InsertionPoint = new Point3D(bottomLeftX, bottomLeftY);
                            //bottomLeftX += selBlock.BoxSize.X;

                            if (eachName.Contains("445"))
                            {
                                selBlock.InsertionPoint = new Point3D(841 - 166 - 7 -(99), 99+7);
                            }

                        }
                    }
                    else if (eachType.Contains("VectorView"))
                    {
                        VectorView selView = eachEntity as VectorView;
                        if (eachName == selView.BlockName)
                        {
                            selView.InsertionPoint = new Point3D(bottomLeftX + selView.BoxSize.X, bottomLeftY);
                            bottomLeftY = selView.BoxSize.Y;
                            bottomLeftX += selView.BoxSize.X;
                        }
                    }

                }
            }

        }
        #endregion
    }
}
