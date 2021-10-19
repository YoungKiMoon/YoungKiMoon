using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using devDept.Eyeshot;

namespace DrawShapeLib.DrawStyleServices
{
    public class LayerStyleService
    {
        public LayerStyleService()
        {

        }

        public string LayerCenterLine = "LayerCenterLine";
        public string LayerVirtualLine = "LayerVirtualLine";
        public string LayerOutLine = "LayerOutLine";
        public string LayerHiddenLine = "LayerHiddenLine";
        public string LayerBasicLine = "LayerBasicLine";

        public string LayerDimension = "LayerDimension";
        public string LayerBlock = "LayerBlock";
        public string LayerRevision = "LayerRevision";
        public string LayerUncertain = "LayerUncertain";

        public string LayerViewport = "LayerViewport";
        public string LayerPaper = "LayerPaper";


        public List<Layer> GetDefaultStyle()
        {
            List<Layer> newList = new List<Layer>();

            // Model
            newList.Add(GetLayer(LayerCenterLine, Color.Red, "CENTER2", 0.25f, true));
            newList.Add(GetLayer(LayerVirtualLine, Color.FromArgb(0, 128, 0), "PHANTOM2", 0.25f, true));
            newList.Add(GetLayer(LayerOutLine, Color.Yellow, "CONTINU", 0.25f, true));
            //newList.Add(GetLayer(LayerHiddenLine, Color.FromArgb(135, 206, 235), "HIDDEN", 0.25f, true));
            newList.Add(GetLayer(LayerHiddenLine, Color.FromArgb(0, 255, 255), "HIDDEN", 0.25f, true));
            newList.Add(GetLayer(LayerBasicLine, Color.White, "CONTINU", 0.25f, true));

            newList.Add(GetLayer(LayerDimension, Color.White, "CONTINU", 0.25f, true));
            newList.Add(GetLayer(LayerBlock, Color.White, null, 0.25f, true));
            newList.Add(GetLayer(LayerRevision, Color.White, "CONTINU", 0.25f, true));
            newList.Add(GetLayer(LayerUncertain, Color.White, "CONTINU", 0.25f, true));

            // Paper
            newList.Add(GetLayer(LayerViewport, Color.White, "CONTINU", 0.25f, true));
            newList.Add(GetLayer(LayerPaper, Color.White, "CONTINU", 0.25f, true));

            return newList;
        }

        public Layer GetLayer(string selLayerName, Color selLineColor, string selLineTypeName, float selLineWeight, bool selVisible)
        {
            Layer newLayer = new Layer(selLayerName, selLineColor, selLineTypeName, selLineWeight, selVisible);
            return newLayer;
        }
    }
}
