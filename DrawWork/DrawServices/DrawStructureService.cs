using DrawWork.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawServices
{
    public class DrawStructureService
    {
        public string tankType = "";
        public string columnType = "";
        public string topAngleType = "";
        public string centeringInEx = "";

        public TANK_TYPE originTankType = TANK_TYPE.NoSet;
        public string originStructureType = "";
        public string originTopAngleType = "";

        public DrawStructureService()
        {

        }
        public void SetStructureData(TANK_TYPE selTankType,string selStructureType,string selTopAngleType)
        {
            originTankType = selTankType;
            originStructureType = selStructureType;
            originTopAngleType = selTopAngleType;

            switch (selTankType)
            {
                case TANK_TYPE.CRT:
                    tankType = "CRT";
                    break;
                case TANK_TYPE.DRT:
                    tankType = "DRT";
                    break;

            }

            switch (selStructureType)
            {
                case "Self-supporting":
                    break;
                case "Rafter only (internal)":
                    columnType = "centering";
                    centeringInEx = "internal";
                    break;
                case "Rafter only (external)":
                    columnType = "centering";
                    centeringInEx = "external";
                    break;
                case "Rafter w/Column":
                    columnType = "column";
                    break;
                case "Rafter w/Column & Girder":
                    columnType = "column";
                    break;
            }

            switch (selTopAngleType)
            {
                case "Detail b":
                case "Detail d":
                case "Detail e":
                    topAngleType = "topangle";
                    break;
                case "Detail i":
                case "Detail k":
                    topAngleType = "compressionring";
                    break;
            }
        }



    }
}
