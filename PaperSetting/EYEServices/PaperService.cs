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
using PaperSetting.Models;
using System.Collections.ObjectModel;

namespace PaperSetting.EYEServices
{
    public class PaperService
    {
        #region Property
        private Model singleModel = null;
        private Drawings singleDraw = null;
        
        public PaperService(Model selModel, Drawings selDraw)
        {
            singleModel = selModel;
            singleDraw = selDraw;
        }

        public void CreatePaperDraw( ObservableCollection<PaperDwgModel> selPaperCol)
        {
            Sheet firstSheet = null;

            foreach(PaperDwgModel eachPaper in selPaperCol)
            {
                Sheet newSheet = CreateSheet(eachPaper.Basic);
                if (firstSheet == null)
                    firstSheet = newSheet;


                // Viewport
                eachPaper.ViewPorts;

                // Block
                eachPaper.Notes;
                // Block
                eachPaper.Tables;
                // Block
                eachPaper.Revisions;
                  

            }


            singleDraw.ActiveSheet = firstSheet;
            singleDraw.Invalidate();
        }

        private Sheet CreateSheet(PaperBasicModel selPaper)
        {
            Sheet newSheet = new Sheet(linearUnitsType.Millimeters, 297, 420, selPaper.Title);
            BlockReference newBr = newSheet.BuildA3ISO(out Block a3Block, "A3_ISO");
            SetBlockOfPaperSheet(selPaper,newBr);
            
            // Add : Draw->Sheet->Sheet
            singleDraw.Blocks.Add(a3Block);
            newSheet.Entities.Add(newBr);
            singleDraw.Sheets.Add(newSheet);

            return newSheet;
        }
        private void SetBlockOfPaperSheet(PaperBasicModel selPaper, BlockReference selBr)
        {
            selBr.Attributes["Title"] = new AttributeReference("Sheet of" + selPaper.Title);
            selBr.Attributes["Format"] = new AttributeReference("B3");
            selBr.Attributes["DwgNo"] = new AttributeReference(selPaper.DwgNo);
            selBr.Attributes["Scale"] = new AttributeReference("SCALE: 1:1");
            selBr.Attributes["Sheet"] = new AttributeReference("SHEET 1 OF 1");
        }

        private void CreateVectorView(Sheet selSheet)
        {
            //VectorView ve2 = new VectorView(10, 10, cCa, 0.001, "View2", printRect2);
            VectorView ve2 = new VectorView(20, 0, viewType.Top, 0.001, "View2");
            Camera cc = ve2.Camera;
            Camera dd = new Camera();

            //dd.Target.Z = 45;
            dd.FocalLength = 0;
            //dd.Distance = 1E-06;
            dd.ProjectionMode = projectionType.Orthographic;

            //dd.Distance = 0.000001;

            dd.Rotation.X = 0.5;
            dd.Rotation.Y = 0.5;
            dd.Rotation.Z = 0.5;
            dd.Rotation.W = 0.5;


            //dd.Location.X = 0;
            //dd.Location.Y = 0;
            //dd.Location.Z = 0;


            //dd.Target.Z = 0;

            //dd.ZoomFactor
            dd.Target.X = -20; // 표적
            dd.Target.Y = -40; // 표적
            dd.Target.Z = 0;
            VectorView vv2 = new VectorView(0, 0, dd, 0.001, "View2", 60, 60);// 형상 높이 폭
            //VectorView vv2 = new VectorView(0, 0, dd, 0.001, "View2",printRect2);

            //VectorView vv2 = new VectorView(0, 0, viewType.Top, 0.001, "Sheet2 Front Vector View", 60, 60);

            //vv2.Camera.Target.X = -20;
            //vv2.Camera.Target.Y = -40;
            sheet1.AddViewPlaceHolder(vv2, sModel, sDraw, "Front Vector View2");
        }
    }
}
