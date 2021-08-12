using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawShellDevModel
    {
        public double rowCount { get; set; }
        public double columnCount { get; set; }

        public DrawShellDevAreaModel area { get; set; }
        public DrawShellDevAreaModel areaScale { get; set; }


        public DrawShellDevModel()
        {
            rowCount = 0;
            columnCount = 0;
            area = new DrawShellDevAreaModel();
            areaScale = new DrawShellDevAreaModel();
        }

        public void SetScaleValue(double scaleValue)
        {
            areaScale.oneHeight = area.oneHeight * scaleValue;
            areaScale.oneWidth = area.oneWidth * scaleValue;

            areaScale.pageHeight = area.pageHeight * scaleValue;
            areaScale.pageWidth = area.pageWidth * scaleValue;

            areaScale.sideGap = area.sideGap * scaleValue;
            areaScale.onePageGap = area.onePageGap * scaleValue;
        }
    }

    public class DrawShellDevAreaModel
    {
        public double oneHeight { get; set; }
        public double oneWidth { get; set; }

        public double pageHeight { get; set; }
        public double pageWidth { get; set; }

        public double sideGap { get; set; }
        public double onePageGap { get; set; }
        public DrawShellDevAreaModel()
        {
            oneHeight = 0;
            oneWidth = 0;

            pageHeight = 0;
            pageWidth = 0;

            sideGap = 0;
            onePageGap = 0;
        }
    }
}
