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
        public SizeModel Size { get; set; }

        public PointModel Location { get; set; }

        public DrawPaperGridModel()
        {
            Size = new SizeModel();
            Location = new PointModel();
        }


    }
}
