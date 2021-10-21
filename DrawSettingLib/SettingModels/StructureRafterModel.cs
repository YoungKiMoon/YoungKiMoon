﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingModels
{
    public class StructureRafterModel
    {
        public StructureRafterModel()
        {
            AngleFromCenter = 0;

            AngleOne = 0;
            Length = 0;

            Size = "";
        }

        public double AngleFromCenter { get; set; }

        public double AngleOne { get; set; }
        public double Length { get; set; }


        // 형상정보
        public string Size { get; set; }
    }
}
