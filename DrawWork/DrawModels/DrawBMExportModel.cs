using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawModels
{
    public class DrawBMExportModel
    {
        public DrawBMExportModel()
        {
            PORNo = "";
            TankNo = "";
            TankName = "";
            Description = "";
            Material = "";

            DimThk = "";
            DimWidth = "";
            DimLength = "";
            DimQty = "";

            UnitWeight = "";

            TankQty = "";
            Margin = "";

            TotalQty = "";
            TotalWeight = "";

            Remarks = "";

        }

        // Excel Output

        public string PORNo { get; set; }
        public string TankNo { get; set; }
        public string TankName { get; set; }
        public string Description { get; set; }
        public string Material { get; set; }

        public string DimThk { get; set; }
        public string DimWidth { get; set; }
        public string DimLength { get; set; }
        public string DimQty { get; set; }

        public string UnitWeight { get; set; }

        public string TankQty { get; set; }

        public string Margin { get; set; }

        public string TotalQty { get; set; }
        public string TotalWeight { get; set; }

        public string Remarks { get; set; }
    }
}
