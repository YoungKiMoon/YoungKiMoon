using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingModels
{
    public class StructureColumnModel
    {
        public StructureColumnModel()
        {
            Radius = 0;
            AngleFromCenter = 0;
            AngleOne = 0;

            Height = 0;


            Size = 0;
        }

        public double Radius { get; set; }
        public double AngleFromCenter { get; set; }

        public double AngleOne { get; set; }


        public double Height { get; set; }


        // 형상 정보
        public double Size { get; set; }
    }
}
