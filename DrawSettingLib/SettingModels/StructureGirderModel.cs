using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingModels
{
    public class StructureGirderModel
    {
        public StructureGirderModel()
        {
            Radius = 0;
            AngleFromCenter = 0;

            AngleOne = 0;
            Length = 0;
            Width = 0;
            Height = 0;

            ClipList = new List<StructureClipModel>();

            Size = "";
        }

        public double Radius { get; set; }
        public double AngleFromCenter { get; set; }

        public double AngleOne { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public List<StructureClipModel> ClipList { get; set; }

        // 형상 정보
        public string Size { get; set; }
    }
}
