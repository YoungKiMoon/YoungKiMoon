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
using Microsoft.Win32;

namespace PaperSetting.EYEServices
{
    public class EYECADService
    {
        public Model sModel = null;
        public Drawings sDraw = null;
        public EYECADService()
        {

        }
        public EYECADService(Model selModel, Drawings selDraw)
        {
            sModel = selModel;
            sDraw = selDraw;
        }

        public void TestExport(Model selModel, Drawings selDraw)
        {
            var exportFileDialog = new SaveFileDialog();
            exportFileDialog.Filter = "CAD drawings(*.dwg)| *.dwg|" + "Drawing Exchange Format (*.dxf)|*.dxf";
            exportFileDialog.AddExtension = true;
            exportFileDialog.Title = "Export";
            exportFileDialog.CheckPathExists = true;
            var result = exportFileDialog.ShowDialog();
            if (result == true)
            {

                WriteAutodeskParams wap = new WriteAutodeskParams(selModel, selDraw, false, false);
                WriteFileAsync wa = new WriteAutodesk(wap, exportFileDialog.FileName);
                selModel.StartWork(wa);


                //EnableImportExportButtons(false);
            }
        }
        public void TestExport2(Model selModel, Drawings selDraw)
        {
            var exportFileDialog = new SaveFileDialog();
            exportFileDialog.Filter = "CAD drawings(*.dwg)| *.dwg|" + "Drawing Exchange Format (*.dxf)|*.dxf";
            exportFileDialog.AddExtension = true;
            exportFileDialog.Title = "Export";
            exportFileDialog.CheckPathExists = true;
            var result = exportFileDialog.ShowDialog();
            if (result == true)
            {

                var wa = new WriteAutodesk(new WriteAutodeskParams(selModel, selDraw,false,false) , exportFileDialog.FileName);
                wa.DoWork();

                //EnableImportExportButtons(false);
            }

        }
    }
}
