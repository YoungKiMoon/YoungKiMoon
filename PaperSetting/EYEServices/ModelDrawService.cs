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

namespace PaperSetting.EYEServices
{
    public class ModelDrawService
    {
        #region Property
        private Model singleModel = null;
        #endregion

        #region CONSTRUCTOR
        public ModelDrawService(Model selModel)
        {
            singleModel = selModel;
        }
        #endregion

        public void CreateSample()
        {
            singleModel.Invalidate();

            string LayerDashDot = "DashDot";
            singleModel.Layers.Add(new Layer(LayerDashDot, Color.CornflowerBlue));
            singleModel.Layers[LayerDashDot].Color = Color.Red;
            singleModel.Layers[LayerDashDot].LineWeight = 4;

            singleModel.LineTypes.Add(LayerDashDot, new float[] { 5, -1, 1, -1 });
            singleModel.Layers[LayerDashDot].LineTypeName = LayerDashDot;

            LinearPath rectBox = new LinearPath(140, 100);
            rectBox.ColorMethod = colorMethodType.byEntity;
            rectBox.Color = Color.Green;
            singleModel.Entities.Add(rectBox);

            Line cusL = new Line(0, 100, 70, 120);
            singleModel.Entities.Add(cusL);
            Line cusR = new Line(70, 120, 140, 100);
            singleModel.Entities.Add(cusR);


            //Line cusH = new Line(-500, 0, 500, 0);
            //Line cusV = new Line(0, -500, 0, 500);
            //Line cusHV = new Line(-500, -500, 500, 500);
            //singleModel.Entities.Add(cusH, LayerDashDot);
            //singleModel.Entities.Add(cusV, LayerDashDot);
            //singleModel.Entities.Add(cusHV, LayerDashDot);

            singleModel.Entities.Regen();
            //singleModel.ZoomFit();
            //singleModel.SetView(viewType.Top, true, false);
        
        }
    }
}
