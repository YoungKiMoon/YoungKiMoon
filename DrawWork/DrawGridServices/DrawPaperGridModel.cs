using DrawSettingLib.Commons;
using DrawSettingLib.SettingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawGridServices
{
    public class DrawPaperGridModel
    {


        public PAPERMAIN_TYPE Name{get;set; }


        public SizeModel Size { get; set; }

        public PointModel Location { get; set; }
        public double ColumnDef { get; set; }
        public double RowDef { get; set; }

        public double Page { get; set; }

        public DrawPaperGridModel()
        {
            Name = PAPERMAIN_TYPE.NotSet;
            Page = 1;
            Size = new SizeModel();
            Location = new PointModel();

            ColumnDef = 1;
            RowDef = 1;


        }


    }
}
