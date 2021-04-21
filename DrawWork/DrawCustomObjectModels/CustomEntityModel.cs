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
    public class CustomEntityModel : Entity
    {
        public string AssyNameE = "";
        public CustomEntityModel(Entity another) : base(another)
        {
            
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
        public override EntitySurrogate ConvertToSurrogate()
        {
            throw new NotImplementedException();
        }
        public override Point3D[] EstimateBoundingBox(BlockKeyedCollection blocks, LayerKeyedCollection layers)
        {
            throw new NotImplementedException();
        }
    }
}
