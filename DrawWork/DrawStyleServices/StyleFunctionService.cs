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
using DrawWork.DrawModels;

//using Color = System.Drawing.Color;

namespace DrawWork.DrawStyleServices
{
    public class StyleFunctionService
    {
        private TextStyleService textService;
        
        public StyleFunctionService()
        {
            textService = new TextStyleService();
        }

        #region Layer / Text
        public void SetLayer(ref Line selEntity, string selLayerName)
        {
            selEntity.LayerName = selLayerName;
            selEntity.LineTypeMethod = colorMethodType.byLayer;
            selEntity.ColorMethod = colorMethodType.byLayer;
        }
        public void SetLayer(ref Line[] selEntity, string selLayerName)
        {
            foreach (Line eachEntity in selEntity)
            {
                eachEntity.LayerName = selLayerName;
                eachEntity.LineTypeMethod = colorMethodType.byLayer;
                eachEntity.ColorMethod = colorMethodType.byLayer;
            }
        }
        public void SetLayer(ref Arc selEntity, string selLayerName)
        {
            selEntity.LayerName = selLayerName;
            selEntity.LineTypeMethod = colorMethodType.byLayer;
            selEntity.ColorMethod = colorMethodType.byLayer;
        }



        public void SetLayerListLine(ref List<Entity> selEntity, string selLayerName)
        {
            foreach (Line eachEntity in selEntity)
            {
                eachEntity.LayerName = selLayerName;
                eachEntity.LineTypeMethod = colorMethodType.byLayer;
                eachEntity.ColorMethod = colorMethodType.byLayer;
            }
        }

        public void SetLayerListEntity(ref List<Entity> selEntity, string selLayerName)
        {
            foreach (Entity eachEntity in selEntity)
            {
                eachEntity.LayerName = selLayerName;
                eachEntity.LineTypeMethod = colorMethodType.byLayer;
                eachEntity.ColorMethod = colorMethodType.byLayer;
            }
        }
        public void SetLayerListLine(ref Entity[] selEntity, string selLayerName)
        {
            foreach (Line eachEntity in selEntity)
            {
                eachEntity.LayerName = selLayerName;
                eachEntity.LineTypeMethod = colorMethodType.byLayer;
                eachEntity.ColorMethod = colorMethodType.byLayer;
            }
        }


        // BlockReference 관련
        public void SetLayer(ref BlockReference selEntity, string selLayerName)
        {
            selEntity.LayerName = selLayerName;
            selEntity.LineTypeMethod = colorMethodType.byLayer;
            selEntity.ColorMethod = colorMethodType.byLayer;
        }

        // Text 관련
        public void SetLayerListTextEntity(ref List<Entity> selEntity, string selLayerName)
        {
            foreach (Text eachEntity in selEntity)
            {
                eachEntity.LayerName = selLayerName;
                eachEntity.StyleName = textService.TextROMANS;// 강제 적용
                eachEntity.ColorMethod = colorMethodType.byLayer;
            }
        }
        public void SetLayer(ref Text selEntity, string selLayerName)
        {
            selEntity.LayerName = selLayerName;
            selEntity.StyleName = textService.TextROMANS;// 강제 적용
            selEntity.ColorMethod = colorMethodType.byLayer;
        }




        // 임시 : 나중에 변경해야 함 : 2021-05-17
        public void SetLayer(ref Triangle selEntity, string selLayerName)
        {
            selEntity.LayerName = selLayerName;
            selEntity.LineTypeMethod = colorMethodType.byLayer;
            selEntity.ColorMethod = colorMethodType.byLayer;
        }
        #endregion
    }
}
