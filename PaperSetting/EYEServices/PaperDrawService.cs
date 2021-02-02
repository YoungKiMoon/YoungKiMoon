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

namespace PaperSetting.EYEServices
{
    public class PaperDrawService
    {
        #region Property
        private Model singleModel = null;
        private Drawings singleDraw = null;

        private Dictionary<string, DockModel> oneSheetBlock = null;
        #endregion

        #region CONSTRUCTOR
        public PaperDrawService(Model selModel, Drawings selDraw)
        {
            singleModel = selModel;
            singleDraw = selDraw;
            oneSheetBlock = new Dictionary<string, DockModel>();
        }
        #endregion



        #region Create : Paper : Draw
        public void CreatePaperDraw( ObservableCollection<PaperDwgModel> selPaperCol,bool selOneSheet=true)
        {
            
            if (selOneSheet)
            {
                singleDraw.Clear();
            }

            Sheet firstSheet = null;

            int sheetIndex = 0;
            foreach (PaperDwgModel eachPaper in selPaperCol)
            {
                // Sheet
                oneSheetBlock = new Dictionary<string, DockModel>();

                Sheet newSheet = CreateSheet(eachPaper);
                
                if (firstSheet == null)
                    firstSheet = newSheet;

                CreateFrameBlock(eachPaper, newSheet);
                CreateTitleBlock(eachPaper, newSheet);
                CreateRevisionBlock(eachPaper, newSheet);

                foreach (PaperNoteModel eachModel in eachPaper.Notes)
                    CreateNoteBlock(eachModel, newSheet,eachPaper.Basic.No);
                foreach (PaperViewportModel eachView in eachPaper.ViewPorts)
                {
                    if(sheetIndex==0)
                        CreateVectorView(newSheet, eachView, eachView.Name);
                    else
                        CreateVectorView2(newSheet, eachView, eachView.Name + sheetIndex.ToString() + eachView.No);
                }
                    
                foreach (PaperTableModel eachModel in eachPaper.Tables)
                    CreateTableBlock(eachModel,newSheet,eachPaper.Basic.No);


                singleDraw.Invalidate();
                singleDraw.ActiveSheet = singleDraw.Sheets[sheetIndex];
                singleModel.UpdateBoundingBox();
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

        private Sheet CreateSheet(PaperDwgModel selPaper)
        {
            Sheet newSheet = new Sheet(linearUnitsType.Millimeters, selPaper.SheetSize.Width, selPaper.SheetSize.Height, selPaper.Basic.Title);
            
            
            // Add : Sheet
            singleDraw.Sheets.Add(newSheet);

            return newSheet;
        }
        private void CreateFrameBlock(PaperDwgModel selPaper, Sheet selSheet)
        {
            BlockReference newBr = BuildPaperFRAME(out Block frameBlock);
            newBr.BlockName += selPaper.Basic.No.ToString();
            frameBlock.Name += selPaper.Basic.No.ToString();
            // Add : just one
            singleDraw.Blocks.Add(frameBlock);
            selSheet.Entities.Add(newBr);
        }
        private void CreateTitleBlock(PaperDwgModel selPaper, Sheet selSheet)
        {
            BlockReference newBr = BuildPaperTitle(selPaper,out Block titleBlock);
            newBr.Attributes["OWNER"] = new AttributeReference("SAMSUNG ENGINEERING");
            newBr.Attributes["PROJECT"] = new AttributeReference("TAnk Basic Automation System");
            newBr.Attributes["TITLE"] = new AttributeReference(selPaper.Basic.Title);
            newBr.Attributes["DWG. NO."] = new AttributeReference(selPaper.Basic.DwgNo);

            newBr.BlockName += selPaper.Basic.No.ToString();
            titleBlock.Name += selPaper.Basic.No.ToString();
            // Add : Just One
            singleDraw.Blocks.Add(titleBlock);
            selSheet.Entities.Add(newBr);
        }
        private void CreateRevisionBlock(PaperDwgModel selPaper, Sheet selSheet)
        {
            BlockReference newBr = BuildPaperRevision(selPaper, out Block revisionBlock);
            newBr.BlockName += selPaper.Basic.No.ToString();
            revisionBlock.Name+= selPaper.Basic.No.ToString();
            // Add : Just One
            singleDraw.Blocks.Add(revisionBlock);
            selSheet.Entities.Add(newBr);
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

        private BlockReference BuildPaperFRAME( out Block selBlock)
        {
            double paperWidth = 420;
            double paperHeight = 297;
            double paperOuterLineMargin = 10;
            double PaperInnerLineMargin = 5;


            Block newBL = new Block("PAPER_FRAME");
            
            LinearPath rectOuterBox = new LinearPath(0,0,paperWidth-(paperOuterLineMargin * 2), paperHeight-(paperOuterLineMargin * 2));
            rectOuterBox.ColorMethod = colorMethodType.byEntity;
            rectOuterBox.Color = Color.Green;
            rectOuterBox.LineWeight = 0.15f;
            rectOuterBox.LineWeightMethod = colorMethodType.byEntity;
            newBL.Entities.Add(rectOuterBox);

            LinearPath rectInnerBox = new LinearPath(PaperInnerLineMargin, PaperInnerLineMargin, paperWidth - (paperOuterLineMargin * 2) -(PaperInnerLineMargin * 2), paperHeight - (paperOuterLineMargin * 2) - (PaperInnerLineMargin * 2));
            rectInnerBox.ColorMethod = colorMethodType.byEntity;
            rectInnerBox.Color = Color.Green;
            rectInnerBox.LineWeight = 0.5f;
            rectInnerBox.LineWeightMethod = colorMethodType.byEntity;
            newBL.Entities.Add(rectInnerBox);


            BlockReference newBR = new BlockReference(paperOuterLineMargin, paperOuterLineMargin, 0,"PAPER_FRAME",1,1,1,0);

            selBlock = newBL;
            return newBR;
        }
        private BlockReference BuildPaperTitle(PaperDwgModel selPaper,out Block selBlock)
        {
            Block newBl = new Block("PAPER_TITLE");


            LinearPath rectBox = new LinearPath(140, 60);
            rectBox.ColorMethod = colorMethodType.byEntity;
            rectBox.Color = Color.Green;
            newBl.Entities.Add(rectBox);

            Line line1 = new Line(0, 15, 140, 15);
            line1.LineWeight = 0.15f;
            line1.LineWeightMethod = colorMethodType.byEntity;
            newBl.Entities.Add(line1);
            line1 = new Line(0, 30, 140, 30);
            line1.LineWeight = 0.15f;
            line1.LineWeightMethod = colorMethodType.byEntity;
            newBl.Entities.Add(line1);
            line1 = new Line(0, 45, 140, 45);
            line1.LineWeight = 0.15f;
            line1.LineWeightMethod = colorMethodType.byEntity;
            newBl.Entities.Add(line1);

            newBl.Entities.Add(new Text(5, 52, 0, "OWNER", 3));
            newBl.Entities.Add(new Text(5, 37, 0, "PROJECT", 3));
            newBl.Entities.Add(new Text(5, 22, 0, "TITLE", 3));
            newBl.Entities.Add(new Text(5, 7, 0, "DWG. NO.", 3));

            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(30, 50, 0, "OWNER", "OWNER?", 5));
            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(30, 35, 0, "PROJECT", "PROJECT?", 5));
            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(30, 20, 0, "TITLE", "TITLE?", 5));
            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(30, 5, 0, "DWG. NO.", "DWG. NO.?", 5));


            BlockReference newBr = new BlockReference(selPaper.SheetSize.Width-140-15, 15, 0, "PAPER_TITLE", 1, 1, 1, 0);

            selBlock = newBl;
            return newBr;

        }
        private BlockReference BuildPaperRevision(PaperDwgModel selPaper, out Block selBlock)
        {
            double[] columnWidths = new double[6] { 10, 20, 50, 20, 20, 20 };

            int rowCount = selPaper.Revisions.Count;
            double fontHeight = 2;
            double rowHeight = 4;
            double blockHeight = rowHeight * (rowCount + 1); // include Header Row
            double blockWidth = 140;

            Block newBl = new Block("PAPER_REVISION");


            LinearPath rectBox = new LinearPath(blockWidth, blockHeight);
            rectBox.ColorMethod = colorMethodType.byEntity;
            rectBox.Color = Color.Green;
            newBl.Entities.Add(rectBox);

            // Column
            double columnWidthSum = 0;
            for (int i = 0; i < columnWidths.Length-1; i++)
            {
                columnWidthSum += columnWidths[i];
                Line colLine = new Line( columnWidthSum, 0, columnWidthSum, blockHeight);
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
            // Header
            double textHeightSum = (rowHeight - fontHeight) / 2;
            double textHeaderWidthSum = 0;
            string[] rowHeaderText = new string[6] { "REV.","DATE","DESCRIPTION","PREPARED","CHECKED","ARRPOVED" };
            for (int i = 0; i < columnWidths.Length; i++)
            {
                double textWidthCenter = columnWidths[i] / 2;
                textHeaderWidthSum += textWidthCenter;

                Text newText = new Text(textHeaderWidthSum, textHeightSum, 0, rowHeaderText[i], fontHeight);
                newText.Alignment = Text.alignmentType.BaselineCenter;
                newBl.Entities.Add(newText);

                textHeaderWidthSum -= textWidthCenter;
                textHeaderWidthSum += columnWidths[i];
            }

            // Text : 
            textHeightSum = rowHeight;
            foreach (PaperRevisionModel eachRev in selPaper.Revisions)
            {
                string[] rowText = new string[6] { eachRev.RevString,eachRev.DateString, eachRev.Description, eachRev.PreparedName, eachRev.CheckedName,eachRev.ApprovedName };

                // Row : Center
                double textHeightCenter = (rowHeight- fontHeight) / 2;
                textHeightSum += textHeightCenter;

                // Column : Center
                double textWidthSum = 0;
                for (int i = 0; i < columnWidths.Length; i++)
                {
                    double textWidthCenter = columnWidths[i] / 2;
                    textWidthSum += textWidthCenter;

                    Text newText = new Text(textWidthSum, textHeightSum,  0, rowText[i], fontHeight);
                    newText.Alignment = Text.alignmentType.BaselineCenter;
                    newBl.Entities.Add(newText);

                    textWidthSum -= textWidthCenter;
                    textWidthSum += columnWidths[i];
                }

                textHeightSum -= textHeightCenter;
                textHeightSum += rowHeight;
            }


            BlockReference newBr = new BlockReference(selPaper.SheetSize.Width - 140 - 15,60+ 15, 0, "PAPER_REVISION", 1, 1, 1, 0);

            selBlock = newBl;
            return newBr;

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
            double rightTopX = 420 - 10 - 5 - 140;
            double rightTopY = 297 - 10 - 5;


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
