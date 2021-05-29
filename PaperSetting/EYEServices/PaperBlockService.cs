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
using DrawWork.DrawStyleServices;

namespace PaperSetting.EYEServices
{
    public class PaperBlockService
    {
        private Drawings singleDraw = null;

        private TextStyleService textStyle;
        private LayerStyleService layerStyle;

        public PaperBlockService(Drawings selDraw)
        {
            singleDraw = selDraw;
            textStyle = new TextStyleService();
            layerStyle = new LayerStyleService();
        }


        #region Create : Paper : Basic : Block
        public void CreatePaperBlock()
        {
            singleDraw.Blocks.Add(BuildPaperFRAME_A1("PAPER_FRAME_A1"));
            singleDraw.Blocks.Add(BuildPaperFRAME_A2("PAPER_FRAME_A2"));
            singleDraw.Blocks.Add(BuildPaperTitle_Type01("PAPER_TITLE_TYPE01"));

        }
        #endregion


        private Block BuildPaperFRAME_A1(string selName)
        {

            SizeModel selSize = new SizeModel("", Commons.PAPERFORMAT_TYPE.A1_ISO,841,594);

            double paperWidth = selSize.Width;
            double paperHeight = selSize.Height;
            double paperOuterLineMargin = 0;
            double PaperInnerLineMargin = 7;

            Color frameBlue = Color.FromArgb(0, 255, 255);// 하늘

            Block newBL = new Block(selName);

            List<Entity> newLinst = new List<Entity>();

            newLinst.Add(new LinearPath(0, 0, paperWidth - (paperOuterLineMargin * 2), paperHeight - (paperOuterLineMargin * 2)));
            newLinst.Add(new LinearPath(PaperInnerLineMargin, PaperInnerLineMargin, paperWidth - (paperOuterLineMargin * 2) - (PaperInnerLineMargin * 2), paperHeight - (paperOuterLineMargin * 2) - (PaperInnerLineMargin * 2)));

            List<Entity> FrameList = new List<Entity>();

            //Vertical
            double[] lineArrayY = new double[] { 0, 93.5, 82.2, 82.2, 82.2, 82.2, 82.2, 89.5 };
            string[] textArrayY = new string[] { "1", "2", "3", "4", "5", "6", "7" };

            double lineSumY = 0;
            for (int i = 0; i < lineArrayY.Length - 1; i++)
            {
                lineSumY += lineArrayY[i];
                if (lineSumY > 0)
                {
                    newLinst.Add(new Line(0, lineSumY, PaperInnerLineMargin, lineSumY));
                    newLinst.Add(new Line(paperWidth - PaperInnerLineMargin, lineSumY, paperWidth, lineSumY));
                }
                FrameList.Add(new Text(3.5, lineSumY + lineArrayY[i + 1] / 2, textArrayY[i], 2.5) { Alignment = Text.alignmentType.MiddleCenter, StyleName = textStyle.TextROMANS });
                FrameList.Add(new Text(paperWidth - 3.5, lineSumY + lineArrayY[i + 1] / 2, textArrayY[i], 2.5) { Alignment = Text.alignmentType.MiddleCenter, StyleName = textStyle.TextROMANS });

            }
            //Horizontal
            double[] lineArrayX = new double[] { 0, 97, 82.2, 82.2, 82.2, 82.2, 82.2, 82.2, 82.2, 82.2, 86.4 };
            string[] textArrayX = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            double lineSumX = 0;
            for (int i = 0; i < lineArrayX.Length - 1; i++)
            {
                lineSumX += lineArrayX[i];
                if (lineSumX > 0)
                {
                    newLinst.Add(new Line(lineSumX, 0, lineSumX, PaperInnerLineMargin));
                    newLinst.Add(new Line(lineSumX, paperHeight - PaperInnerLineMargin, lineSumX, paperHeight));
                }
                FrameList.Add(new Text(lineSumX + lineArrayX[i + 1] / 2, paperHeight - 3.5, textArrayX[i], 2.5) { Alignment = Text.alignmentType.MiddleCenter,StyleName = textStyle.TextROMANS });
                FrameList.Add(new Text(lineSumX + lineArrayX[i + 1] / 2, 3.5, textArrayX[i], 2.5) { Alignment = Text.alignmentType.MiddleCenter, StyleName = textStyle.TextROMANS });
            }

            foreach(Entity eachEntity in newLinst)
            {
                eachEntity.LayerName = layerStyle.LayerPaper;
                eachEntity.LineTypeMethod = colorMethodType.byLayer;
                eachEntity.ColorMethod = colorMethodType.byEntity;
                eachEntity.Color = frameBlue;
                eachEntity.LineWeightMethod = colorMethodType.byLayer;
            }

            foreach (Entity eachEntity in FrameList)
            {
                eachEntity.LayerName = layerStyle.LayerPaper;
                eachEntity.ColorMethod = colorMethodType.byEntity;
                eachEntity.Color = frameBlue;
            }

            newBL.Entities.AddRange(newLinst);
            newBL.Entities.AddRange(FrameList);

            return newBL;
        }

        private Block BuildPaperFRAME_A2(string selName)
        {

            SizeModel selSize = new SizeModel("", Commons.PAPERFORMAT_TYPE.A2_ISO, 594, 420);

            double paperWidth = selSize.Width;
            double paperHeight = selSize.Height;
            double paperOuterLineMargin = 0;
            double PaperInnerLineMargin = 7;

            Color frameBlue = Color.FromArgb(0, 255, 255);// 하늘

            Block newBL = new Block(selName);

            List<Entity> newLinst = new List<Entity>();

            newLinst.Add(new LinearPath(0, 0, paperWidth - (paperOuterLineMargin * 2), paperHeight - (paperOuterLineMargin * 2)));
            newLinst.Add(new LinearPath(PaperInnerLineMargin, PaperInnerLineMargin, paperWidth - (paperOuterLineMargin * 2) - (PaperInnerLineMargin * 2), paperHeight - (paperOuterLineMargin * 2) - (PaperInnerLineMargin * 2)));

            List<Entity> FrameList = new List<Entity>();

            //Vertical
            double[] lineArrayY = new double[] { 0, 93.5, 82.2, 82.2, 82.2, 86.6 };
            string[] textArrayY = new string[] { "1", "2", "3", "4", "5" };

            double lineSumY = 0;
            for (int i = 0; i < lineArrayY.Length - 1; i++)
            {
                lineSumY += lineArrayY[i];
                if (lineSumY > 0)
                {
                    newLinst.Add(new Line(0, lineSumY, PaperInnerLineMargin, lineSumY));
                    newLinst.Add(new Line(paperWidth - PaperInnerLineMargin, lineSumY, paperWidth, lineSumY));
                }
                FrameList.Add(new Text(3.5, lineSumY + lineArrayY[i + 1] / 2, textArrayY[i], 2.5) { Alignment = Text.alignmentType.MiddleCenter, StyleName = textStyle.TextROMANS });
                FrameList.Add(new Text(paperWidth - 3.5, lineSumY + lineArrayY[i + 1] / 2, textArrayY[i], 2.5) { Alignment = Text.alignmentType.MiddleCenter, StyleName = textStyle.TextROMANS });

            }
            //Horizontal
            double[] lineArrayX = new double[] { 0, 91.5, 82.2, 82.2, 82.2, 82.2, 82.2,91.5};
            string[] textArrayX = new string[] { "A", "B", "C", "D", "E", "F", "G" };
            double lineSumX = 0;
            for (int i = 0; i < lineArrayX.Length - 1; i++)
            {
                lineSumX += lineArrayX[i];
                if (lineSumX > 0)
                {
                    newLinst.Add(new Line(lineSumX, 0, lineSumX, PaperInnerLineMargin));
                    newLinst.Add(new Line(lineSumX, paperHeight - PaperInnerLineMargin, lineSumX, paperHeight));
                }
                FrameList.Add(new Text(lineSumX + lineArrayX[i + 1] / 2, paperHeight - 3.5, textArrayX[i], 2.5) { Alignment = Text.alignmentType.MiddleCenter, StyleName = textStyle.TextROMANS });
                FrameList.Add(new Text(lineSumX + lineArrayX[i + 1] / 2, 3.5, textArrayX[i], 2.5) { Alignment = Text.alignmentType.MiddleCenter, StyleName = textStyle.TextROMANS });
            }

            foreach (Entity eachEntity in newLinst)
            {
                eachEntity.LayerName = layerStyle.LayerPaper;
                eachEntity.LineTypeMethod = colorMethodType.byLayer;
                eachEntity.ColorMethod = colorMethodType.byEntity;
                eachEntity.Color = frameBlue;
                eachEntity.LineWeightMethod = colorMethodType.byLayer;
            }

            foreach (Entity eachEntity in FrameList)
            {
                eachEntity.LayerName = layerStyle.LayerPaper;
                eachEntity.ColorMethod = colorMethodType.byEntity;
                eachEntity.Color = frameBlue;
            }

            newBL.Entities.AddRange(newLinst);
            newBL.Entities.AddRange(FrameList);

            return newBL;
        }
        private Block BuildPaperTitle_Type01(string selName)
        {
            Block newBl = new Block(selName);
            List<Entity> newEntity = new List<Entity>();
            // Horizontal
            newEntity.Add(new Line(0, 137, 166, 137));
            newEntity.Add(new Line(0, 122, 166, 122));
            newEntity.Add(new Line(0, 107, 166, 107));
            newEntity.Add(new Line(0, 95, 166, 95));
            newEntity.Add(new Line(0, 80, 166, 80));
            newEntity.Add(new Line(0, 70, 166, 70));
            newEntity.Add(new Line(0, 40, 166, 40));
            newEntity.Add(new Line(0, 30, 166, 30));
            newEntity.Add(new Line(0, 20, 166, 20));

            newEntity.Add(new Line(136, 15, 166, 15));
            newEntity.Add(new Line(0, 10, 136, 10));

            // Vertical
            newEntity.Add(new Line(0, 137, 0, 0));
            newEntity.Add(new Line(83, 80, 83, 70));
            newEntity.Add(new Line(136, 20, 136, 0));
            newEntity.Add(new Line(151, 20, 151, 0));
            newEntity.Add(new Line(68, 10, 68, 0));

            foreach (Entity eachEntity in newEntity)
            {
                eachEntity.ColorMethod = colorMethodType.byLayer;
                eachEntity.LayerName = layerStyle.LayerPaper;
            }

            newBl.Entities.AddRange(newEntity);

            // Text
            Color textYellow = Color.FromArgb(255, 255, 0);// 노란
            Color frameBlue = Color.FromArgb(0, 255, 255);// 하늘

            List<Entity> newText = new List<Entity>();
            newText.Add(new Text(1, 137 - 3.5, "CLIENT", 2.5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.85, ColorMethod = colorMethodType.byEntity, Color = textYellow });
            newText.Add(new Text(1, 107 - 3.5, "PROJECT", 2.5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.85, ColorMethod = colorMethodType.byEntity, Color = textYellow });
            newText.Add(new Text(1, 95 - 3.5, "MANUFACTURER", 2.5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.85, ColorMethod = colorMethodType.byEntity, Color = textYellow });
            newText.Add(new Text(1, 75, "PROJECT NO:", 3.5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.80, ColorMethod = colorMethodType.byEntity, Color = textYellow, Alignment = Text.alignmentType.MiddleLeft });
            newText.Add(new Text(1 + 83, 75, "CONTRACT NO:", 3.5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.80, ColorMethod = colorMethodType.byEntity, Color = textYellow, Alignment = Text.alignmentType.MiddleLeft });
            newText.Add(new Text(1, 70 - 3.5, "TITLE", 2.5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.85, ColorMethod = colorMethodType.byEntity, Color = textYellow });

            newText.Add(new Text(1, 35, "CLIENT V.P NO:", 4) { StyleName = textStyle.TextROMANS, WidthFactor = 0.9, ColorMethod = colorMethodType.byLayer,Alignment = Text.alignmentType.MiddleLeft });
            newText.Add(new Text(1, 25, "SECL VIP NO:", 3.5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.9, ColorMethod = colorMethodType.byEntity, Color = textYellow, Alignment = Text.alignmentType.MiddleLeft });
            newText.Add(new Text(1, 15, "MFR. VIP NO:", 3.5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.9, ColorMethod = colorMethodType.byEntity, Color = textYellow, Alignment = Text.alignmentType.MiddleLeft });

            newText.Add(new Text(1, 5, "SCALE:", 3.5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.9, ColorMethod = colorMethodType.byEntity, Color = textYellow, Alignment = Text.alignmentType.MiddleLeft });
            newText.Add(new Text(1 + 68, 5, "SHEET:", 3.5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.9, ColorMethod = colorMethodType.byEntity, Color = textYellow, Alignment = Text.alignmentType.MiddleLeft });

            newText.Add(new Text(136 + 7.5, 17.5, "REVISION", 2.5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.85, ColorMethod = colorMethodType.byEntity, Color = textYellow, Alignment = Text.alignmentType.MiddleCenter });
            newText.Add(new Text(151 + 7.5, 17.5, "FROMAT", 2.5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.85, ColorMethod = colorMethodType.byEntity, Color = textYellow, Alignment = Text.alignmentType.MiddleCenter });

            foreach (Entity eachEntity in newText)
                eachEntity.LayerName = layerStyle.LayerPaper;

            newBl.Entities.AddRange(newText);

            // Text Embedded
            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(83, 122 + 7.5, 0, "CLIENT", "-", 5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.8,Alignment=Text.alignmentType.MiddleCenter, ColorMethod = colorMethodType.byLayer,LayerName=layerStyle.LayerPaper });
            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(83, 95 + 6, 0, "PROJECT", "TABAS", 5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.8, Alignment = Text.alignmentType.MiddleCenter, ColorMethod = colorMethodType.byLayer, LayerName = layerStyle.LayerPaper });
            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(83, 80 + 7.5,0, "MANUFACTURER", "-", 5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.8, Alignment = Text.alignmentType.MiddleCenter, ColorMethod = colorMethodType.byLayer, LayerName = layerStyle.LayerPaper });
            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(83 - (52.4 / 2), 75, 0, "PROJECTNO", "-", 3.5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.8, Alignment = Text.alignmentType.MiddleCenter, ColorMethod = colorMethodType.byEntity,LayerName=layerStyle.LayerPaper, Color = textYellow });
            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(166 - (52.4 / 2), 75, 0, "CONTRACTNO", "-", 3.5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.8, Alignment = Text.alignmentType.MiddleCenter, ColorMethod = colorMethodType.byEntity, Color = textYellow,LayerName=layerStyle.LayerPaper });
            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(83, 55, 0, "TITLE", "GENERAL ASSEMBLY (1/2)", 4) { StyleName = textStyle.TextROMANS, WidthFactor = 1, Alignment = Text.alignmentType.MiddleCenter, ColorMethod = colorMethodType.byLayer, LayerName = layerStyle.LayerPaper });
            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(83 + 18, 35, 0, "CLIENTVPNO", "-", 4) { StyleName = textStyle.TextROMANS, WidthFactor = 0.7, Alignment = Text.alignmentType.MiddleCenter, ColorMethod = colorMethodType.byLayer, LayerName = layerStyle.LayerPaper });
            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(90, 25, 0, "SELCVIPNO", "-", 3.5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.7, Alignment = Text.alignmentType.MiddleCenter, ColorMethod = colorMethodType.byEntity, Color = textYellow, LayerName = layerStyle.LayerPaper });
            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(90, 15, 0, "MFRVIPNO", "-", 3.5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.7, Alignment = Text.alignmentType.MiddleCenter, ColorMethod = colorMethodType.byEntity, Color = textYellow, LayerName = layerStyle.LayerPaper });
            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(22 +(68/2), 5, 0, "SCALE", "SEE DWG", 3.5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.9, Alignment = Text.alignmentType.MiddleCenter, ColorMethod = colorMethodType.byEntity, Color = textYellow, LayerName = layerStyle.LayerPaper });
            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(68 + 22 +(44/2), 5, 0, "SHEET", "1/1", 3.5) { StyleName = textStyle.TextROMANS, WidthFactor = 0.9, Alignment = Text.alignmentType.MiddleCenter, ColorMethod = colorMethodType.byEntity, Color = textYellow, LayerName = layerStyle.LayerPaper });

            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(136 + 7.5, 7.5, 0, "REVISION", "A", 6) { StyleName = textStyle.TextROMANS, WidthFactor = 0.8, Alignment = Text.alignmentType.MiddleCenter, ColorMethod = colorMethodType.byEntity, Color = frameBlue, LayerName = layerStyle.LayerPaper });
            newBl.Entities.Add(new devDept.Eyeshot.Entities.Attribute(151 + 7.5, 7.5, 0, "FORMAT", "A1", 6) { StyleName = textStyle.TextROMANS, WidthFactor = 0.9, Alignment = Text.alignmentType.MiddleCenter, ColorMethod = colorMethodType.byEntity, Color = frameBlue, LayerName = layerStyle.LayerPaper });


            return newBl;

        }
    }
}
