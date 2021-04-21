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
    public class CustomLine : Line
    {
        public string AssyName = "";
        public CustomLine(Line another) : base(another)
        {

        }
        public CustomLine(Point3D startPoint, Point3D endPoint) : base(startPoint, endPoint)
        {

        }
    }
}
