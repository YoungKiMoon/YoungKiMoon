using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using devDept.Eyeshot;

namespace DrawWork.DrawStyleServices
{
    public class LayerStyleService
    {
        public LayerStyleService()
        {

        }

        public List<Layer> GetDefaultStyle()
        {
            List<Layer> newList = new List<Layer>();

            // Model
            newList.Add(GetLayer("LayerCenterLine", Color.Red, "CENTER2", 0.25f, true));
            newList.Add(GetLayer("LayervirtualLine", Color.Green, "PHANTOM2", 0.25f, true));
            newList.Add(GetLayer("LayerOutLine", Color.Yellow, "CONTINU", 0.25f, true));
            newList.Add(GetLayer("LayerHiddenLine", Color.SkyBlue, "HIDDEN", 0.25f, true));
            newList.Add(GetLayer("LayerBasicLine", Color.Black, "CONTINU", 0.25f, true));

            newList.Add(GetLayer("LayerDimension", Color.Black, "CONTINU", 0.25f, true));
            newList.Add(GetLayer("LayerBlock", Color.Black, null, 0.25f, true));
            newList.Add(GetLayer("LayerRevision", Color.Black, "CONTINU", 0.25f, true));
            newList.Add(GetLayer("LayerUncertain", Color.Black, "CONTINU", 0.25f, true));

            // Paper
            newList.Add(GetLayer("LayerViewPort", Color.Black, "CONTINU", 0.25f, true));
            newList.Add(GetLayer("LayerPaper", Color.Black, "CONTINU", 0.25f, true));

            return newList;
        }

        public Layer GetLayer(string selLayerName, Color selLineColor,string selLineTypeName, float selLineWeight,bool selVisible)
        {
            Layer newLayer = new Layer(selLayerName, selLineColor,selLineTypeName,selLineWeight, selVisible);
            return newLayer;
        }
    }
}
