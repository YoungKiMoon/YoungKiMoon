using StrengthCalLib.CalMatchingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrengthCalLib.CalServices
{
    public class CalMatchingService
    {
        public List<StructureTypeModel> structureTable { get; set; }
        public List<BottomStyleModel> bottomTable { get; set; }
        public List<RoofTypeModel> roofTable { get; set; }
        public List<WindCodeModel> windCodeTable { get; set; }
        public List<SeismicCodeModel> seismicTable { get; set; }

        public List<AnchorModel> anchorTable { get; set; }
        public CalMatchingService()
        {
            CreateMatchingTable();
        }

        private void CreateMatchingTable()
        {
            CreateStructureTable();
            CreateBottomTable();
            CreateRoofTable();
            CreateWindCodeTable();
            CreateSeismicTable();
            CreateAnchorTable();
        }

        private void CreateStructureTable()
        {
            structureTable = new List<StructureTypeModel>();
        }
        private void CreateBottomTable()
        {
            bottomTable = new List<BottomStyleModel>();
        }
        private void CreateRoofTable()
        {
            roofTable = new List<RoofTypeModel>();
            RoofTypeModel newModel01 = new RoofTypeModel();
            newModel01.Roof = "Cone";
            newModel01.AMEtxt01 = "Slope (Rise / Run)";
            newModel01.AMEtxt02 = "slope =";


            roofTable.Add(newModel01);
        }
        private void CreateWindCodeTable()
        {
            windCodeTable = new List<WindCodeModel>();
        }
        private void CreateSeismicTable()
        {
            seismicTable = new List<SeismicCodeModel>();
        }

        private void CreateAnchorTable()
        {
            anchorTable = new List<AnchorModel>();

        }



        public StructureTypeModel GetStructureType(string ame)
        {
            StructureTypeModel returnValue = new StructureTypeModel();
            foreach(StructureTypeModel eachStructure in structureTable)
            {
                if (eachStructure.AME.Contains(ame))
                {
                    returnValue = eachStructure;
                    break;
                }
            }

            return returnValue;
        }

        public RoofTypeModel GetRoofType(string roof)
        {
            string roofValue = roof;
            if (roof == "CRT")
                roofValue = "Cone";
            else if(roof=="DRT")
                roofValue = "Dome";

            RoofTypeModel returnValue = new RoofTypeModel();
            foreach(RoofTypeModel eachRoof in roofTable)
            {
                if (eachRoof.Roof.Contains(roof))
                {
                    returnValue = eachRoof;
                    break;
                }
            }

            return returnValue;
        }

        public BottomStyleModel GetBottomType(string ame)
        {
            BottomStyleModel returnValue = new BottomStyleModel();
            foreach (BottomStyleModel eachBottom in bottomTable)
            {
                if (eachBottom.AME.Contains(ame))
                {
                    returnValue = eachBottom;
                    break;
                }
            }

            return returnValue;
        }

        public WindCodeModel GetWindCode(string code)
        {
            WindCodeModel returnValue = new WindCodeModel();
            foreach (WindCodeModel eachWind in windCodeTable)
            {
                if (eachWind.WindCode.Contains(code))
                {
                    returnValue = eachWind;
                    break;
                }
            }

            return returnValue;
        }

        public AnchorModel GetAnchorNut(string bolt)
        {
            AnchorModel returnValue = new AnchorModel();
            foreach (AnchorModel eachWind in anchorTable)
            {
                if (eachWind.Bolt.Contains(bolt))
                {
                    returnValue = eachWind;
                    break;
                }
            }

            return returnValue;
        }

    }
}
