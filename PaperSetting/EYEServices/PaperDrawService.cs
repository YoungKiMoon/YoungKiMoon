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

namespace PaperSetting.EYEServices
{
    public class PaperDrawService
    {
        #region Service
        ValueService valueService;
        DrawScaleService scaleService;
        LayerStyleService styleService;
        TextStyleService textService;
        #endregion

        #region Property
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
        public PaperDrawService(Model selModel, Drawings selDraw)
        {
            valueService = new ValueService();
            scaleService = new DrawScaleService();
            styleService = new LayerStyleService();
            textService = new TextStyleService();

            singleModel = selModel;
            singleDraw = selDraw;
            oneSheetBlock = new Dictionary<string, DockModel>();

            selectionTitleBlock = "PAPER_TITLE_TYPE01";
        }
        #endregion




        #region Create : Paper : Draw
        public void CreatePaperDraw(ObservableCollection<PaperDwgModel> selPaperCol, AssemblyModel assemblyData,double scaleValue, bool selOneSheet = true)
        {

            singleDraw.Sheets.Clear();
            singleDraw.Blocks.Remove("newView");

            for (int i = singleDraw.Blocks.Count - 1; i > -1; i--)
            {
                Block eachBlock = singleDraw.Blocks[i];
                if (eachBlock.Name.Contains("PAPER_TABLE"))
                {
                    singleDraw.Blocks.Remove(eachBlock);
                }
                if (eachBlock.Name.Contains("PAPER_NOTE"))
                {
                    singleDraw.Blocks.Remove(eachBlock);
                }
            }





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
                string sheetName = eachPaper.Basic.Title;
                //newSheet.Name = eachPaper.Basic.Title;
                //newSheet = CreateSheet(eachPaper);
                Sheet newSheet = new Sheet(linearUnitsType.Millimeters, eachPaper.SheetSize.Width, eachPaper.SheetSize.Height, sheetName);
                //newSheet = new Sheet(linearUnitsType.Millimeters, 200, 200, "GENERAL ASSEMBLY(1/2)");
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


                //ViewPort 임시로 제외
                foreach (PaperViewportModel eachView in eachPaper.ViewPorts)
                {
                    if (sheetIndex == 0)
                        CreateVectorViewGA(newSheet, eachView.ViewPort, scaleValue, assemblyData);
                    //CreateVectorView(newSheet, eachView, eachView.Name);
                    else
                        CreateVectorView2(newSheet, eachView, eachView.Name + sheetIndex.ToString() + eachView.No);
                }


                // 아직 미적용
                //foreach (PaperTableModel eachModel in eachPaper.Tables)
                //    CreateTableBlock(eachModel,newSheet,eachPaper.Basic.No);
                //
                if (eachPaper.Basic.Title == "GENERAL ASSEMBLY(1-2)")
                {
                    CreateTableBlockGADesign(eachPaper.Tables[0], newSheet, eachPaper.Tables[0].No, assemblyData);
                    CreateTableBlockGA(eachPaper.Tables[1], newSheet, eachPaper.Tables[1].No, assemblyData);
                    CreateTableBlockGA2(eachPaper.Tables[2], newSheet, eachPaper.Tables[2].No, assemblyData);
                }
                if (eachPaper.Basic.Title == "GENERAL ASSEMBLY(2-2)")
                {
                    CreateNoteBlockGA(eachPaper.Notes[0], newSheet, eachPaper.Notes[0].No, assemblyData);
                }


                //singleModel.UpdateBoundingBox();
                //singleDraw.Invalidate();
                // 중요한지 테스트 필요함
                singleDraw.ActiveSheet = singleDraw.Sheets[sheetIndex];
                AutomaticAlignmentOfDock(sheetIndex);
                singleDraw.Invalidate();
                sheetIndex++;


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
            singleDraw.Rebuild(singleModel);


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
                targetPoint.X = SingletonData.GAViewPortCenter.X + SingletonData.GAViewPortSize.X / 2;
                targetPoint.Y = SingletonData.GAViewPortCenter.Y + SingletonData.GAViewPortSize.Y / 2;


                Camera newnewCamera = new Camera(targetPoint,0,Viewport.GetCameraRotation(viewType.Top),projectionType.Orthographic,0,1);
                VectorView newnewView = new VectorView(334,
                                                    363,
                                                    newnewCamera,
                                                    scaleService.GetViewScale(scaleValue),
                                                    "newView",
                                                    valueService.GetDoubleValue(selViewPort.SizeX),
                                                    valueService.GetDoubleValue(selViewPort.SizeY)
                                                    );

                newnewView.CenterlinesExtensionAmount = extensionAmount;


                newnewView.KeepEntityColor = true;


                newnewView.LayerName = styleService.LayerViewport;

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
            newBr.LayerName = styleService.LayerPaper;
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

            newBr.LayerName = styleService.LayerPaper;
            // Add : Just One
            selSheet.Entities.Add(newBr);
        }
        private void CreateRevisionBlock(PaperDwgModel selPaper, Sheet selSheet)
        {
            List<Entity> newList = BuildPaperRevision(selPaper, selectionTitleBlock);

            foreach (Entity eachEntity in newList)
                eachEntity.LayerName = styleService.LayerPaper;

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

            newBr.LayerName = styleService.LayerBlock;

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

            newBr.LayerName = styleService.LayerBlock;

            tableBlock.Name += bName;
            newBr.BlockName = tableBlock.Name;

            singleDraw.Blocks.Add(tableBlock);
            selSheet.Entities.Add(newBr);
            oneSheetBlock.Add(newBr.BlockName, selTable.Dock);
        }
        private void CreateTableBlockGA2(PaperTableModel selTable, Sheet selSheet, string bName, AssemblyModel assemblyData)
        {
            BlockReference newBr = BuildPaperTableGA2(selTable, assemblyData, out Block tableBlock);
            newBr.LayerName = styleService.LayerBlock;

            tableBlock.Name += bName;
            newBr.BlockName = tableBlock.Name;

            singleDraw.Blocks.Add(tableBlock);
            selSheet.Entities.Add(newBr);
            oneSheetBlock.Add(newBr.BlockName, selTable.Dock);
        }
        private void CreateTableBlockGADesign(PaperTableModel selTable, Sheet selSheet, string bName, AssemblyModel assemblyData)
        {
            BlockReference newBr = BuildPaperTableGADesign(selTable, assemblyData, out Block tableBlock);

            newBr.LayerName= styleService.LayerBlock;

            tableBlock.Name += bName;
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
                eachEntity.LayerName = styleService.LayerPaper;
                eachEntity.LineTypeMethod = colorMethodType.byLayer;
                eachEntity.ColorMethod = colorMethodType.byLayer;
            }

            foreach (Entity eachEntity in newTextList)
            {
                eachEntity.LayerName = styleService.LayerPaper;
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
            newList.Add(new Line(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = styleService.LayerDimension });
            currentY += titleRowHeight;
            newList.Add(new Line(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = styleService.LayerDimension });
            currentY += rowHeight;
            newList.Add(new Line(GetSumPoint(refPoint, titleRowHeight, currentY), GetSumPoint(refPoint, tableWidth, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = styleService.LayerDimension });

            for (int i = 0; i < defaultCount - 1; i++)
            {
                currentY += rowHeight;
                // 마지막 행
                newList.Add(new Line(GetSumPoint(refPoint, titleRowHeight, currentY), GetSumPoint(refPoint, partOneWidth, currentY)) { Color = Color.White, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = styleService.LayerDimension });
                newList.Add(new Line(GetSumPoint(refPoint, partTwoWidth, currentY), GetSumPoint(refPoint, partThrWidth, currentY)) { Color = Color.White, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = styleService.LayerDimension });
            }
            currentY += rowHeight;
            // 마지막
            newList.Add(new Line(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = styleService.LayerDimension });

            // 열
            double colCount = 0;
            currentX = 0;
            newList.Add(new Line(GetSumPoint(refPoint, currentX, 0), GetSumPoint(refPoint, currentX, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = styleService.LayerDimension });
            foreach (double eachCol in widthList)
            {
                colCount++;
                currentX += eachCol;
                if (colCount == 1)
                    newList.Add(new Line(GetSumPoint(refPoint, currentX, titleRowHeight), GetSumPoint(refPoint, currentX, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = styleService.LayerDimension });
                else if (colCount == 11)
                    newList.Add(new Line(GetSumPoint(refPoint, currentX, 0), GetSumPoint(refPoint, currentX, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = styleService.LayerDimension });
                else
                    newList.Add(new Line(GetSumPoint(refPoint, currentX, titleRowHeight), GetSumPoint(refPoint, currentX, currentY)) { Color = Color.White, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = styleService.LayerDimension });
            }


            // 마지막 텍스트
            Text newTitle01 = new Text(GetSumPoint(refPoint, tableWidth / 2, titleRowHeight / 2), "CONNECTION SCHEDULE", fontHeightTitle);
            newTitle01.Alignment = Text.alignmentType.MiddleCenter;
            newTitle01.StyleName = textService.TextROMANS;
            newTitle01.LayerName = styleService.LayerDimension;
            newTitle01.ColorMethod = colorMethodType.byEntity;
            newTitle01.Color = Color.Yellow;

            Text newTitle02 = new Text(GetSumPoint(refPoint, titleRowHeight / 2, (currentY + titleRowHeight) / 2), "SHELL NOZZLE", fontHeightTitle);
            newTitle02.Alignment = Text.alignmentType.MiddleCenter;
            newTitle02.StyleName = textService.TextROMANS;
            newTitle02.LayerName = styleService.LayerDimension;
            newTitle02.ColorMethod = colorMethodType.byEntity;
            newTitle02.Color = Color.Yellow;
            newTitle02.Rotate(Utility.DegToRad(90), Vector3D.AxisZ, GetSumPoint(refPoint, titleRowHeight / 2, (currentY + titleRowHeight) / 2));

            Text newTitle03 = new Text(GetSumPoint(refPoint, partOneWidth + 10 / 2, (currentY + titleRowHeight + 6) / 2), "-SEE ORIENTATION DWG.-", fontHeight);
            newTitle03.Alignment = Text.alignmentType.MiddleCenter;
            newTitle03.StyleName = textService.TextROMANS;
            newTitle03.LayerName = styleService.LayerDimension;
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
            newList.Add(new Line(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = styleService.LayerDimension });
            currentY += titleRowHeight;
            newList.Add(new Line(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = styleService.LayerDimension });
            currentY += rowHeight;
            newList.Add(new Line(GetSumPoint(refPoint, titleRowHeight, currentY), GetSumPoint(refPoint, tableWidth, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = styleService.LayerDimension });

            for (int i = 0; i < defaultCount - 1; i++)
            {
                currentY += rowHeight;
                // 마지막 행
                newList.Add(new Line(GetSumPoint(refPoint, titleRowHeight, currentY), GetSumPoint(refPoint, partOneWidth, currentY)) { Color = Color.White, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = styleService.LayerDimension });
                newList.Add(new Line(GetSumPoint(refPoint, partTwoWidth, currentY), GetSumPoint(refPoint, partThrWidth, currentY)) { Color = Color.White, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = styleService.LayerDimension });
            }
            currentY += rowHeight;
            // 마지막
            newList.Add(new Line(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = styleService.LayerDimension });

            // 열
            double colCount = 0;
            currentX = 0;
            newList.Add(new Line(GetSumPoint(refPoint, currentX, 0), GetSumPoint(refPoint, currentX, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = styleService.LayerDimension });
            foreach (double eachCol in widthList)
            {
                colCount++;
                currentX += eachCol;
                if (colCount == 1)
                    newList.Add(new Line(GetSumPoint(refPoint, currentX, titleRowHeight), GetSumPoint(refPoint, currentX, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = styleService.LayerDimension });
                else if (colCount == 11)
                    newList.Add(new Line(GetSumPoint(refPoint, currentX, 0), GetSumPoint(refPoint, currentX, currentY)) { Color = Color.Yellow, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = styleService.LayerDimension });
                else
                    newList.Add(new Line(GetSumPoint(refPoint, currentX, titleRowHeight), GetSumPoint(refPoint, currentX, currentY)) { Color = Color.White, ColorMethod = colorMethodType.byEntity, LineTypeMethod = colorMethodType.byLayer, LayerName = styleService.LayerDimension });
            }


            // 마지막 텍스트
            Text newTitle01 = new Text(GetSumPoint(refPoint, tableWidth / 2, titleRowHeight / 2), "CONNECTION SCHEDULE", fontHeightTitle);
            newTitle01.Alignment = Text.alignmentType.MiddleCenter;
            newTitle01.StyleName = textService.TextROMANS;
            newTitle01.LayerName = styleService.LayerDimension;
            newTitle01.ColorMethod = colorMethodType.byEntity;
            newTitle01.Color = Color.Yellow;

            Text newTitle02 = new Text(GetSumPoint(refPoint, titleRowHeight / 2, (currentY + titleRowHeight) / 2), "ROOF NOZZLE", fontHeightTitle);
            newTitle02.Alignment = Text.alignmentType.MiddleCenter;
            newTitle02.StyleName = textService.TextROMANS;
            newTitle02.LayerName = styleService.LayerDimension;
            newTitle02.ColorMethod = colorMethodType.byEntity;
            newTitle02.Color = Color.Yellow;
            newTitle02.Rotate(Utility.DegToRad(90), Vector3D.AxisZ, GetSumPoint(refPoint, titleRowHeight / 2, (currentY + titleRowHeight) / 2));

            Text newTitle03 = new Text(GetSumPoint(refPoint, partOneWidth + 10 / 2, (currentY + titleRowHeight + 6) / 2), "-SEE ORIENTATION DWG.-", fontHeight);
            newTitle03.Alignment = Text.alignmentType.MiddleCenter;
            newTitle03.StyleName = textService.TextROMANS;
            newTitle03.LayerName = styleService.LayerDimension;
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
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 67.5 / 2, currentY + rowHeight / 2), AMEDATA[0], fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + 107.5, currentY + rowHeight / 2), "CONTENTS", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, 132.5 + 42.5 / 2, currentY + rowHeight / 2), AMEDATA[1], fontHeight, 0.95, Text.alignmentType.MiddleCenter));




            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "SIZE (mm)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "INSIDE DIA.", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 25, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 25 + 42.5 / 2, currentY + rowHeight / 2), AMEDATA[2], fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + 107.5, currentY + rowHeight / 2), "HEIGHT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, 132.5, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, 132.5 + 42.5 / 2, currentY + rowHeight / 2), AMEDATA[3], fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "CAPACITY (m3)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "NOMINAL", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 25, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 25 + 42.5 / 2, currentY + rowHeight / 2), AMEDATA[4], fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + 107.5, currentY + rowHeight / 2), "WORKING", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, 132.5, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, 132.5 + 42.5 / 2, currentY + rowHeight / 2), AMEDATA[5], fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "PUMPING RATES", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "IN", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 25, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 25 + 42.5 / 2, currentY + rowHeight / 2), AMEDATA[6], fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + 107.5, currentY + rowHeight / 2), "OUT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, 132.5, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, 132.5 + 42.5 / 2, currentY + rowHeight / 2), AMEDATA[7], fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 51, currentY), GetSumPoint(refPoint, tableOneWidth + 51, currentY + rowHeight)));
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 81, currentY), GetSumPoint(refPoint, tableOneWidth + 81, currentY + rowHeight)));


            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "OPERATING TEMP.", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "(MAX.)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 25 + 26 / 2, currentY + rowHeight / 2), AMEDATA[8], fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 51, currentY + rowHeight / 2), "DESIGN TEMP", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 81, currentY + rowHeight / 2), "(MIN./MAX.)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 106 + 29 / 2, currentY + rowHeight / 2), AMEDATA[9] + "/" + AMEDATA[10], fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 39, currentY), GetSumPoint(refPoint, tableOneWidth + 39, currentY + rowHeight)));
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 67.5, currentY), GetSumPoint(refPoint, tableOneWidth + 67.5, currentY + rowHeight)));
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 81, currentY), GetSumPoint(refPoint, tableOneWidth + 81, currentY + rowHeight)));
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 115, currentY), GetSumPoint(refPoint, tableOneWidth + 115, currentY + rowHeight)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "OPERATING PRESS.", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 39 / 2, currentY + rowHeight / 2), AMEDATA[11] + "/" + AMEDATA[12], fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 39, currentY + rowHeight / 2), "TEST SP. GR.", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 67.5 + 13.5 / 2, currentY + rowHeight / 2), "", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 81, currentY + rowHeight / 2), "DESIGN SP. GR.", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 115 + 20 / 2, currentY + rowHeight / 2), "", fontHeight, 0.95, Text.alignmentType.MiddleCenter));




            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 51, currentY), GetSumPoint(refPoint, tableOneWidth + 51, currentY + rowHeight)));
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 81, currentY), GetSumPoint(refPoint, tableOneWidth + 81, currentY + rowHeight)));


            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "VAPOR PRESSURE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 51 / 2, currentY + rowHeight / 2), "", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 51, currentY + rowHeight / 2), "DESIGN PRESS.", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 81 + 54 / 2, currentY + rowHeight / 2), AMEDATA[13] + "/" + AMEDATA[14], fontHeight, 0.95, Text.alignmentType.MiddleCenter));




            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            // 3행
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY - rowHeight * 2 + rowHeight * 3 / 2), "SET PRESSURE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "EMERGENCY COVER MANHOLE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 60, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 60 + 75 / 2, currentY + rowHeight / 2), AMEDATA[15], fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "BREATHER VALVE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 60, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 60 + 75 / 2, currentY + rowHeight / 2), AMEDATA[16], fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "BREATHER VALVE (VAC.)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 60, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 60 + 75 / 2, currentY + rowHeight / 2), AMEDATA[17], fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            // 2행
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY - rowHeight * 1 + rowHeight * 2 / 2), "CORR. ALLOW. (mm)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "SHELL", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 25, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 25 + 20 / 2, currentY + rowHeight / 2), AMEDATA[18], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 45, currentY + rowHeight / 2), "ROOF", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70 + 20 / 2, currentY + rowHeight / 2), AMEDATA[19], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 90, currentY + rowHeight / 2), "BOTTOM", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 115, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 115 + 20 / 2, currentY + rowHeight / 2), AMEDATA[20], fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "NOZZLE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 25, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 25 + 20 / 2, currentY + rowHeight / 2), AMEDATA[21], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 45, currentY + rowHeight / 2), "STRUCTURE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70 + 20 / 2, currentY + rowHeight / 2), AMEDATA[22], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 90, currentY + rowHeight / 2), "(EACH SIDE)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "SHELL DESIGN", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 135 / 2, currentY + rowHeight / 2), AMEDATA[23], fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "ROOF DESIGN", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 135 / 2, currentY + rowHeight / 2), AMEDATA[24], fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            // 2행
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY - rowHeight * 1 + rowHeight * 2 / 2), "ROOF LOADS", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "UNIFORM LIVE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70 + 65 / 2, currentY + rowHeight / 2), AMEDATA[25], fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "SPECIAL LOADING (PROVIDE SKETCH)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70 + 65 / 2, currentY + rowHeight / 2), AMEDATA[26], fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            // 2행
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY - rowHeight * 1 + rowHeight * 2 / 2), "EARTHQUAKE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), AMEDATA[27], fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 70, currentY + rowHeight / 2), "SEISMIC USE GROUP", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70 + 42, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70 + 42 + 21 / 2, currentY + rowHeight / 2), AMEDATA[28], fontHeight, 0.95, Text.alignmentType.MiddleCenter));





            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "Ss(g)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 15, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 15 + 21 / 2, currentY + rowHeight / 2), AMEDATA[29], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 36, currentY + rowHeight / 2), "Ss(g)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 51, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 51 + 21 / 2, currentY + rowHeight / 2), AMEDATA[30], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 70, currentY + rowHeight / 2), "IMPORTANCE FACTOR", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70 + 42, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 70 + 42 + 21 / 2, currentY + rowHeight / 2), AMEDATA[31], fontHeight, 0.95, Text.alignmentType.MiddleCenter));





            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            // 2행
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY - rowHeight * 1 + rowHeight * 2 / 2), "WIND", fontHeight, 0.95, Text.alignmentType.MiddleLeft));


            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), AMEDATA[32], fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 60 + 75 / 2, currentY + rowHeight / 2), AMEDATA[33], fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "EXPOSURE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 25, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 25 + 35 / 2, currentY + rowHeight / 2), AMEDATA[34], fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 60, currentY + rowHeight / 2), "IMPORTANCE FACTOR", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 103, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 103 + 32 / 2, currentY + rowHeight / 2), AMEDATA[35], fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "SNOW FALL", fontHeight, 0.95, Text.alignmentType.MiddleLeft));


            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "TOTAL ACCUMULATION", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 44, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 44 + 28 / 2, currentY + rowHeight / 2), AMEDATA[36], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 44 + 28, currentY + rowHeight / 2), "RAIN FALL", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 44 + 28 + 24, currentY + rowHeight / 2), "(MAX.)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 44 + 28 + 24 + 13 + 26 / 2, currentY + rowHeight / 2), AMEDATA[37], fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 44 + 28, currentY), GetSumPoint(refPoint, tableOneWidth + 44 + 28, currentY + rowHeight)));
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth + 44 + 28 + 24, currentY), GetSumPoint(refPoint, tableOneWidth + 44 + 28 + 24, currentY + rowHeight)));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "FOUNDATION TYPE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));


            // 사각형
            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth , currentY + 1.2), AMEDATA[33].ToLower().Contains("earth")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 5.5, currentY + rowHeight / 2), "EARTH", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth +30 , currentY + 1.2), AMEDATA[33].ToLower().Contains("concrete ring wall")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth +30+ 5.5, currentY + rowHeight / 2), "CONCRETE RING WALL", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 90, currentY + 1.2), AMEDATA[33].ToLower().Contains("concrete mat")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 90 + 5.5, currentY + rowHeight / 2), "CONCRETE MAT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));


            currentY -= rowHeight;
            newList.Add(GetNewLineYellow(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "ESTIMATED WEIGHT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));



            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "EMPTY", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 16, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 16 + 24 / 2, currentY + rowHeight / 2), AMEDATA[39], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 40, currentY + rowHeight / 2), "OPER.", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 56, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 56 + 24 / 2, currentY + rowHeight / 2), AMEDATA[40], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 80, currentY + rowHeight / 2), "FULL OF WATER", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 111, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 111 + 24 / 2, currentY + rowHeight / 2), AMEDATA[41], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

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
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 17 + 59 / 2, currentY + rowHeight / 2), AMEDATA[42], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 76, currentY + rowHeight / 2), "COLUMN", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 100, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 100 + 35 / 2, currentY + rowHeight / 2), AMEDATA[43], fontHeight, 0.95, Text.alignmentType.MiddleCenter));




            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "BOTTOM", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 17, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 17 + 59 / 2, currentY + rowHeight / 2), AMEDATA[44], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 76, currentY + rowHeight / 2), "ANNULAR", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 100, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 100 + 35 / 2, currentY + rowHeight / 2), AMEDATA[45], fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "ROOF", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 17, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 17 + 59 / 2, currentY + rowHeight / 2), AMEDATA[46], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 76, currentY + rowHeight / 2), "STRUCTURE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 100, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 100 + 35 / 2, currentY + rowHeight / 2), AMEDATA[47], fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "FLANGE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 17, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 17 + 59 / 2, currentY + rowHeight / 2), AMEDATA[48], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 76, currentY + rowHeight / 2), "GASKET", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 100, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 100 + 35 / 2, currentY + rowHeight / 2), AMEDATA[49], fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "BOLT/NUT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 21, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 21 + 42.5 / 2, currentY + rowHeight / 2), AMEDATA[50] + "/" + AMEDATA[51], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 21 + 42.5, currentY + rowHeight / 2), "NOZZLE NECK", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 21 + 42.5 + 28.2, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 21 + 42.5 + 28.2 + 43.5 / 2, currentY + rowHeight / 2), AMEDATA[52], fontHeight, 0.95, Text.alignmentType.MiddleCenter));



            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "ANCHOR BOLT/NUT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 40, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 40 + 95 / 2, currentY + rowHeight / 2), AMEDATA[53] + "/" + AMEDATA[54], fontHeight, 0.95, Text.alignmentType.MiddleCenter));




            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));


            // 2행
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY - rowHeight * 1 + rowHeight * 2 / 2), "BOTTOM PLATE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "TH'K", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 25 / 2, currentY + rowHeight / 2), AMEDATA[55], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 23 + 25, currentY + 1.2), AMEDATA[56].ToLower().Contains("lap joint")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 23 + 25 + 5.5, currentY + rowHeight / 2), "LAP JOINT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 84, currentY + 1.2), AMEDATA[56].ToLower().Contains("butt joint")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 84 + 5.5, currentY + rowHeight / 2), "BUTT JOINT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "SLOPE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 25 / 2, currentY + rowHeight / 2), AMEDATA[57], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 23 + 25, currentY + 1.2), AMEDATA[58].ToLower().Contains("coned up")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 23 + 25 + 5.5, currentY + rowHeight / 2), "CONED UP", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 84, currentY + 1.2), AMEDATA[58].ToLower().Contains("coned down")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 84 + 5.5, currentY + rowHeight / 2), "CONED DOWN", fontHeight, 0.95, Text.alignmentType.MiddleLeft));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "ANNULAR PLATE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "MIN. WIDTH", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 25 / 2, currentY + rowHeight / 2), AMEDATA[59], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 23 + 25, currentY + rowHeight / 2), "TH'K", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 25 + 12, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 25 + 12 + 13 / 2, currentY + rowHeight / 2), AMEDATA[60], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 73, currentY + 1.2), AMEDATA[61].ToLower().Contains("butt joint")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 73 + 5.5, currentY + rowHeight / 2), "BUTT JOINT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 103, currentY + 1.2), AMEDATA[61].ToLower().Contains("lap joint")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 103 + 5.5, currentY + rowHeight / 2), "LAP JOINT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "ROOF PLATE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "TH'K", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 50 / 2, currentY + rowHeight / 2), AMEDATA[62], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 73, currentY + 1.2), AMEDATA[63].ToLower().Contains("butt joint")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 73 + 5.5, currentY + rowHeight / 2), "BUTT JOINT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));


            newList.AddRange(GetSquare(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 103, currentY + 1.2), AMEDATA[63].ToLower().Contains("lap joint")));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 103 + 5.5, currentY + rowHeight / 2), "LAP JOINT", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "FIXED ROOF", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "SLOPE", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 25 / 2, currentY + rowHeight / 2), AMEDATA[64], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

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
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 112 / 2, currentY + rowHeight / 2), AMEDATA[71], fontHeight, 0.95, Text.alignmentType.MiddleCenter));




            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 135 / 2, currentY + rowHeight / 2), "-", fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, tableOneWidth, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "EXTERNAL", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 112 / 2, currentY + rowHeight / 2), AMEDATA[72], fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineWhite(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 135 / 2, currentY + rowHeight / 2), "-", fontHeight, 0.95, Text.alignmentType.MiddleCenter));


            currentY -= rowHeight;
            newList.Add(GetNewLineYellow(GetSumPoint(refPoint, 0, currentY), GetSumPoint(refPoint, tableWidth, currentY)));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap, currentY + rowHeight / 2), "INSULATION (mm)", fontHeight, 0.95, Text.alignmentType.MiddleLeft));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth, currentY + rowHeight / 2), "SHELL", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 44 / 2, currentY + rowHeight / 2), AMEDATA[73], fontHeight, 0.95, Text.alignmentType.MiddleCenter));

            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, leftTextGap + tableOneWidth + 23 + 44, currentY + rowHeight / 2), "ROOF", fontHeight, 0.95, Text.alignmentType.MiddleLeft));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 44 + 23, currentY + rowHeight / 2), ":", fontHeight, 0.95, Text.alignmentType.MiddleCenter));
            newList.Add(GetNewTextWhite(GetSumPoint(refPoint, tableOneWidth + 23 + 44 + 23 + 45 / 2, currentY + rowHeight / 2), AMEDATA[74], fontHeight, 0.95, Text.alignmentType.MiddleCenter));




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
                LayerName = styleService.LayerDimension,
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
                LayerName = styleService.LayerDimension,
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
                LayerName = styleService.LayerDimension 
            };
        }
        private Line GetNewLineWhite(Point3D selPoint1, Point3D selPoint2)
        {
            return new Line(selPoint1, selPoint2)
            {
                Color = Color.White,
                ColorMethod = colorMethodType.byEntity,
                LineTypeMethod = colorMethodType.byLayer,
                LayerName = styleService.LayerDimension
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
                        bottomDic.Add(bNameList[dicCount], eachDock.DockPriority);
                        break;
                    case DOCKPOSITION_TYPE.FLOATING:
                        floatDic.Add(bNameList[dicCount], eachDock.DockPriority);
                        break;
                }
            }

            // right
            var rightDicOrderby= rightDic.OrderBy(num => num.Value).ToDictionary(pair=>pair.Key,pair=>pair.Value);
            double rightTopX = 841 - 7 - 175;
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
                            selBlock.InsertionPoint = new Point3D(rightTopX, rightTopY);

                                


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

            // Float
            //var floatDicOrderby = floatDic.OrderBy(num => num.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            //double topLeftX = 7;
            //double topLeftY = 7;


            //foreach (string eachName in floatDicOrderby.Keys)
            //{
            //    foreach (Entity eachEntity in singleDraw.Sheets[selIndex].Entities)
            //    {
            //        string eachType = eachEntity.GetType().Name;
            //        if (eachType.Contains("BlockReference"))
            //        {
            //            BlockReference selBlock = eachEntity as BlockReference;
            //            selBlock.Regen(new RegenParams(0, singleDraw));
            //            //newText01.Regen(new RegenParams(0, ssModel));
            //            if (eachName == selBlock.BlockName)
            //            {
            //                selBlock.InsertionPoint = new Point3D(bottomLeftX, bottomLeftY);
            //                bottomLeftX += selBlock.BoxSize.X;
            //            }
            //        }
            //        else if (eachType.Contains("VectorView"))
            //        {
            //            VectorView selView = eachEntity as VectorView;
            //            if (eachName == selView.BlockName)
            //            {
            //                selView.InsertionPoint = new Point3D(bottomLeftX + selView.BoxSize.X, bottomLeftY);
            //                bottomLeftY = selView.BoxSize.Y;
            //                bottomLeftX += selView.BoxSize.X;
            //            }
            //        }

            //    }
            //}

         }
        #endregion
    }
}
