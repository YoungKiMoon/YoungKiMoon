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

namespace PaperSetting.EYEServices
{
    public class PaperDrawService
    {
        #region Property
        private Model singleModel = null;
        private Drawings singleDraw = null;
        #endregion

        #region CONSTRUCTOR
        public PaperDrawService(Model selModel, Drawings selDraw)
        {
            singleModel = selModel;
            singleDraw = selDraw;
        }
        #endregion



        #region Create : Paper : Draw
        public void CreatePaperDraw( ObservableCollection<PaperDwgModel> selPaperCol)
        {
            singleDraw.Sheets.Clear();
            singleDraw.Blocks.Clear();

            Sheet firstSheet = null;

            
            foreach (PaperDwgModel eachPaper in selPaperCol)
            {
                // Sheet
                Sheet newSheet = CreateSheet(eachPaper);
                if (firstSheet == null)
                    firstSheet = newSheet;

                CreateFrameBlock(eachPaper, newSheet);
                CreateTitleBlock(eachPaper, newSheet);
                foreach (PaperNoteModel eachModel in eachPaper.Notes)
                    CreateNoteBlock(eachModel,newSheet);
                foreach (PaperViewportModel eachView in eachPaper.ViewPorts)
                    CreateVectorView(newSheet, eachView.ViewPort, eachView.Name);

                // Revision
                CreateRevisionBlock(eachPaper);

                // Viewport

                // Table : Block
                foreach (PaperTableModel eachModel in eachPaper.Tables)
                    CreateTableBlock(eachModel);


            }


            singleDraw.ActiveSheet = firstSheet;
            singleDraw.ZoomFit();
            singleDraw.Invalidate();
            singleDraw.ActionMode = actionType.SelectByPick;
            //singleDraw.Rebuild(singleModel);
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
            // Add : Just One
            singleDraw.Blocks.Add(titleBlock);
            selSheet.Entities.Add(newBr);
        }
        private void CreateNoteBlock(PaperNoteModel selNote,Sheet selSheet)
        {
            BlockReference newBr = BuildPaperNote(selNote, out Block noteBlock);
            // Add : Just One
            singleDraw.Blocks.Add(noteBlock);
            selSheet.Entities.Add(newBr);
        }

        private void CreateVectorView(Sheet selSheet, ViewPortModel selView, string viewName)
        {
            //VectorView ve2 = new VectorView(10, 10, cCa, 0.001, "View2", printRect2);
            //VectorView ve2 = new VectorView(20, 0, viewType.Top, 0.001, "View2");
            //Camera cc = ve2.Camera;

            //dd.ZoomFactor
            //dd.Distance = 0.000001;
            //dd.Location.X = 0;
            //dd.Location.Y = 0;
            //dd.Location.Z = 0;

            Camera newCamera = new Camera();

            newCamera.FocalLength = 0;
            newCamera.ProjectionMode = projectionType.Orthographic;

            newCamera.Rotation.X = 0.5;
            newCamera.Rotation.Y = 0.5;
            newCamera.Rotation.Z = 0.5;
            newCamera.Rotation.W = 0.5;

            newCamera.Target.X = 0; // 표적
            newCamera.Target.Y = 0; // 표적
            newCamera.Target.Z = 0;

            double extensionAmount = Math.Min(selSheet.Width, selSheet.Height) / 400;

            VectorView newView = new VectorView(selView.Location.X,
                                                selView.Location.Y,
                                                newCamera,
                                                0.001,
                                                viewName,
                                                selView.Size.Width,
                                                selView.Size.Height){ CenterlinesExtensionAmount = extensionAmount };// 형상 높이 폭
            
            selSheet.AddViewPlaceHolder(newView, singleModel, singleDraw, viewName + "PlaceHolder");
        }
        private void CreateTableBlock(PaperTableModel selNote)
        {

        }
        private void CreateRevisionBlock(PaperDwgModel selNote)
        {

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

            newBl.Entities.Add(new Text(10, 52, 0, "OWNER", 3));
            newBl.Entities.Add(new Text(10, 37, 0, "PROJECT", 3));
            newBl.Entities.Add(new Text(10, 22, 0, "TITLE", 3));
            newBl.Entities.Add(new Text(10, 7, 0, "DWG. NO.", 3));

            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(30, 50, 0, "OWNER", "OWNER?", 5));
            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(30, 35, 0, "PROJECT", "PROJECT?", 5));
            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(30, 20, 0, "TITLE", "TITLE?", 5));
            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(30, 5, 0, "DWG. NO.", "DWG. NO.?", 5));


            BlockReference newBr = new BlockReference(selPaper.SheetSize.Width-140-15, 15, 0, "PAPER_TITLE", 1, 1, 1, 0);

            selBlock = newBl;
            return newBr;

        }
        private BlockReference BuildPaperNote(PaperNoteModel selNote,out Block selBlock)
        {

            string[] noteArray = selNote.Note.Split('\n');
            int lineCount = noteArray.Length;
            double fontHeight = 2;
            double lineGap = 0.6;
            double blockPadding = 1;
            double textPositionY = (lineGap+fontHeight)*(lineCount-1);

            Block newBl = new Block("PAPER_NOTE" + selNote.No);

            for(int i = 0; i < lineCount; i++)
            {
                Text newText = new Text(0, textPositionY, 0, noteArray[i], fontHeight);
                newBl.Entities.Add(newText);
                textPositionY -= (lineGap + fontHeight);
            }


            BlockReference newBR = new BlockReference(selNote.Location.X, selNote.Location.Y, 0, "PAPER_NOTE" + selNote.No, 1, 1, 1, 0);

            selBlock = newBl;
            return newBR;
        }
        #endregion
    }
}
