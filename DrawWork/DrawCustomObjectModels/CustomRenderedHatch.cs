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
using System.Drawing;

namespace DrawWork.DrawCustomObjectModels
{
    public class CustomRenderedHatch : HatchRegion
    {
        public CustomRenderedHatch(HatchRegion another)
              : base(another)
        {
        }

        protected override void DrawWireframe(DrawParams data)
        {
            Pre(data.RenderContext);
            SetRenderedShader(data);

            Color col = Color;
            if (data.Selected)
                col = Color.Yellow;

            data.RenderContext.SetColorDiffuse(col, col);

            Draw(data);

            Post(data.RenderContext);
        }
        public void SetRenderedShader(DrawParams data)
        {
            if (data.ShaderParams != null)
            {
                bool prevLighting = data.ShaderParams.Lighting;
                var prevPrimitiveType = data.ShaderParams.PrimitiveType;

                data.ShaderParams.Lighting = true;
                data.ShaderParams.PrimitiveType = shaderPrimitiveType.Polygon;

                base.SetShader(data);

                data.ShaderParams.Lighting = prevLighting;
                data.ShaderParams.PrimitiveType = prevPrimitiveType;
            }
        }
        protected override void DrawIsocurves(DrawParams data)
        {
            //base.DrawIsocurves(data);
        }
        protected override void DrawForSelection(GfxDrawForSelectionParams data)
        {
            Pre(data.RenderContext);
            base.DrawForSelection(data);
            Post(data.RenderContext);
        }
        private void Post(RenderContextBase context)
        {
            context.PopRasterizerState();
        }
        private void Pre(RenderContextBase context)
        {
            context.PushRasterizerState();
            context.SetState(rasterizerStateType.CCW_PolygonFill_CullFaceBack_PolygonOffset_1_1);
        }

    }
}
