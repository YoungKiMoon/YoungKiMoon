using devDept.Eyeshot.Entities;
using devDept.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawOnePlateModel
    {
        
        public Point3D InsertPoint { get; set; }
        public double PlateWidth { get; set; }
        public double PlateLength { get; set; }
        public double RemainingLength { get; set; }
        
        public double Requirement { get; set; }

        public List<DrawPlateModel> CuttingPlateList { get; set; }
        public List<string> CuttingPlateNameList { get; set; }
        public List<Entity> PlateOutlineList { get; set; }

        public List<Entity> PlateCuttingList { get; set; }

        public DrawOnePlateModel()
        {
            InsertPoint = null;
            PlateWidth = 0;
            PlateLength = 0;
            RemainingLength = 0;
            Requirement = 0;

            CuttingPlateList = new List<DrawPlateModel>();
            CuttingPlateNameList = new List<string>();
            PlateOutlineList = new List<Entity>();
            PlateCuttingList = new List<Entity>();
        }
    }
}
