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
        public void CreatePaperDraw( ObservableCollection<PaperDwgModel> selPaperCol,bool selOneSheet=true)
        {
            
            singleDraw.Sheets.Clear();
            singleDraw.Blocks.Remove("newView");


            Sheet firstSheet = null;

            int sheetIndex = 0;
            foreach (PaperDwgModel eachPaper in selPaperCol)
            {
                // Sheet
                oneSheetBlock = new Dictionary<string, DockModel>();

                Sheet newSheet = CreateSheet(eachPaper);

                if (firstSheet == null)
                    firstSheet = newSheet;

                // 완료 : 20210512
                CreateFrameBlock(eachPaper, newSheet);

                // 아직 미적용 : 20210512 적용 예정
                CreateTitleBlock(eachPaper, newSheet);
                
                // 일반 적용
                CreateRevisionBlock(eachPaper, newSheet);

                foreach (PaperNoteModel eachModel in eachPaper.Notes)
                    CreateNoteBlock(eachModel, newSheet,eachPaper.Basic.No);

                foreach (PaperViewportModel eachView in eachPaper.ViewPorts)
                {
                    if (sheetIndex == 0)
                        CreateVectorViewGA(newSheet,eachView.ViewPort);
                        //CreateVectorView(newSheet, eachView, eachView.Name);
                    else
                        CreateVectorView2(newSheet, eachView, eachView.Name + sheetIndex.ToString() + eachView.No);
                }
                
                
                // 아직 미적용
                //foreach (PaperTableModel eachModel in eachPaper.Tables)
                //    CreateTableBlock(eachModel,newSheet,eachPaper.Basic.No);
                //



                //singleModel.UpdateBoundingBox();
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

        public void CreateVectorViewGA(Sheet selSheet,ViewPortModel selViewPort)
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
                //Sheet selSheet = singleDraw.Sheets[0];

                Camera newCamera = new Camera();

                newCamera.FocalLength = 0;
                newCamera.ProjectionMode = projectionType.Orthographic;

                newCamera.Rotation.X = 0.5;
                newCamera.Rotation.Y = 0.5;
                newCamera.Rotation.Z = 0.5;
                newCamera.Rotation.W = 0.5;

                // 표적 중앙
                newCamera.Target.X = valueService.GetDoubleValue(selViewPort.TargetX);
                newCamera.Target.Y = valueService.GetDoubleValue(selViewPort.TargetY);
                newCamera.Target.Z = 0;


                double extensionAmount = Math.Min(selSheet.Width, selSheet.Height) / 594;

                VectorView newView = new VectorView(valueService.GetDoubleValue(selViewPort.LocationX),
                                                    valueService.GetDoubleValue(selViewPort.LocationY),
                                                    newCamera,
                                                    valueService.GetDoubleValue(selViewPort.ScaleStr),
                                                    "newView",
                                                    valueService.GetDoubleValue(selViewPort.SizeX),
                                                    valueService.GetDoubleValue(selViewPort.SizeY));

                newView.CenterlinesExtensionAmount = extensionAmount;
                //newView.CenterlinesExtensionAmount = 0;
                //newView.FillRegions = false;
                //newView.FillTexts = false;

                //selSheet.AddViewPlaceHolder(newView, singleModel, singleDraw, "GAPlaceHolder");

                Camera newnewCamera = new Camera(new Point3D(valueService.GetDoubleValue(selViewPort.TargetX), valueService.GetDoubleValue(selViewPort.TargetY), 0),
                                0,
                                Viewport.GetCameraRotation(viewType.Top),
                                projectionType.Orthographic,
                                0,
                                1);
                VectorView newnewView = new VectorView(valueService.GetDoubleValue(selViewPort.LocationX),
                                                    valueService.GetDoubleValue(selViewPort.LocationY),
                                                    newnewCamera,
                                                    scaleService.GetViewScale(valueService.GetDoubleValue(selViewPort.ScaleStr)),
                                                    "newView",
                                                    valueService.GetDoubleValue(selViewPort.SizeX),
                                                    valueService.GetDoubleValue(selViewPort.SizeY)
                                                    );
                newnewView.CenterlinesExtensionAmount = extensionAmount;


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
            Sheet newSheet = new Sheet(linearUnitsType.Millimeters, selPaper.SheetSize.Width, selPaper.SheetSize.Height, selPaper.Basic.Title);
            singleDraw.Sheets.Add(newSheet);
            
            return newSheet;
        }
        private void CreateFrameBlock(PaperDwgModel selPaper, Sheet selSheet)
        {
            BlockReference newBr = new BlockReference(0, 0, 0, "PAPER_FRAME_A1", 1, 1, 1, 0);
            newBr.LayerName = styleService.LayerPaper;
            selSheet.Entities.Add(newBr);
        }
        private void CreateTitleBlock(PaperDwgModel selPaper, Sheet selSheet)
        {
            //PAPER_TITLE_TYPE01
            BlockReference newBr = new BlockReference(selPaper.SheetSize.Width-166-7, 7, 0, "PAPER_TITLE_TYPE01", 1, 1, 1, 0);
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
            selSheet.Entities.AddRange(newList);
            selSheet.Entities.Add(new Line(0, 0, 200, 200) { ColorMethod = colorMethodType.byEntity, Color = Color.Red });
        }


        private void CreateNoteBlock(PaperNoteModel selNote,Sheet selSheet,string bName)
        {
            BlockReference newBr = BuildPaperNote(selNote, out Block noteBlock);
            newBr.BlockName += bName;
            noteBlock.Name += bName;

            singleDraw.Blocks.Add(noteBlock);
            selSheet.Entities.Add(newBr);
            oneSheetBlock.Add(newBr.BlockName, selNote.Dock);
        }
        private void CreateTableBlock(PaperTableModel selTable, Sheet selSheet,string bName)
        {
            BlockReference newBr = BuildPaperTable(selTable, out Block tableBlock);
            newBr.BlockName += bName;
            tableBlock.Name += bName;

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
                                                selViewPort.Size.Height){ CenterlinesExtensionAmount = extensionAmount };// 형상 높이 폭
            
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

        private List<Entity> BuildPaperRevision(PaperDwgModel selPaper,string selTitleName)
        {
            List<Entity> newList = new List<Entity>();
            List<Entity> newTextList = new List<Entity>();

            if (selPaper.Revisions.Count == 0)
                return newList;



            double[] columnWidths = new double[] { 8, 78, 16, 16, 16, 16,16 };
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
                    PaperRevisionModel eachRev = selPaper.Revisions[i-1];
                    newRevisionStr = new string[] { eachRev.RevString, eachRev.Description, eachRev.DateString, eachRev.DRWN, eachRev.CHKD, eachRev.REVD, eachRev.APVD };
                }
                for (int j = 0; j < columnWidths.Length; j++)
                {
                    newTextList.Add(new Text(columnSumWidth + columnWidths[j] / 2, rowHeightSum + rowList[i] / 2, newRevisionStr[j], 2.5) {Alignment=Text.alignmentType.MiddleCenter,WidthFactor= columnTextHeight[j],StyleName= textService.TextROMANS });
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
            foreach(Entity eachEntity in newList)
            {
                eachEntity.LayerName = styleService.LayerPaper;
                eachEntity.LineTypeMethod = colorMethodType.byLayer;
                eachEntity.ColorMethod = colorMethodType.byLayer;
            }

            foreach(Entity eachEntity in newTextList)
            {
                eachEntity.LayerName = styleService.LayerPaper;
                eachEntity.ColorMethod = colorMethodType.byEntity;
                eachEntity.Color = textYellow;

                newList.Add(eachEntity);
            }

            return newList;

        }
        private BlockReference BuildPaperNote(PaperNoteModel selNote,out Block selBlock)
        {

            string[] noteArray = selNote.Note.Split('\n');
            int lineCount = noteArray.Length;
            double fontHeight = 2;
            double lineSpacing = 0.6;
            double blockPadding = 1;
            double textPositionY = 0;
            //double textPositionY = (lineSpacing + fontHeight) * (lineCount - 1);




            Block newBl = new Block("PAPER_NOTE" + selNote.No);



            for (int i = lineCount-1; i >= 0; i--)
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


            BlockReference newBr = new BlockReference(selTable.Location.X,selTable.Location.Y, 0, "PAPER_TABLE" + selTable.No, 1, 1, 1, 0);

            selBlock = newBl;
            return newBr;

        }
        #endregion

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
            double rightTopX = 841 - 10 - 5 - 140;
            double rightTopY = 594 - 10 - 5;


            foreach (string eachName in rightDicOrderby.Keys)
            {
                foreach (Entity eachEntity in singleDraw.Sheets[selIndex].Entities)
                {
                    string eachType = eachEntity.GetType().Name;
                    if (eachType.Contains("BlockReference"))
                    {
                        BlockReference selBlock = eachEntity as BlockReference;
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
            double bottomLeftX = 15;
            double bottomLeftY = 15;


            foreach (string eachName in bottomDicOrderby.Keys)
            {
                foreach (Entity eachEntity in singleDraw.Sheets[selIndex].Entities)
                {
                    string eachType = eachEntity.GetType().Name;
                    if (eachType.Contains("BlockReference"))
                    {
                        BlockReference selBlock = eachEntity as BlockReference;
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

            // Bottom

            // Float
        }
        #endregion
    }
}
