using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawBMNumberModel
    {
        // Symbol : Circle
        public double circleRadius { get; set; }

        public bool visible { get; set; }

        public double textHeight { get; set; }

        public string number { get; set; }

        public DrawBMNumberModel()
        {
            circleRadius = 4;
            //Diameter=8;
            visible = true;
            textHeight = 3;
            number = "";
        }
    }
}
