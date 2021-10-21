using AssemblyLib.AssemblyModels;
using DrawCalculationLib.FunctionServices;
using DrawSettingLib.SettingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingServices
{
    public class StructureService
    {
        public GeometryService geoService;

        public StructureService()
        {
            geoService = new GeometryService();
        }


        public StructureModel CreateStructureCRTColumn(List<StructureCRTRafterInputModel> rafterList,
                                                 List<StructureCRTColumnInputModel> columnList,
                                                 List<StructureCRTGirderInputModel> girderList,
                                                 List<HBeamModel> girderHbeamList)
        {

            // Layer 에 맞게끔 수정 작업
            List<StructureCRTRafterInputModel> newRafterList = new List<StructureCRTRafterInputModel>();
            newRafterList.Add(new StructureCRTRafterInputModel());
            newRafterList.AddRange(rafterList);

            List<StructureCRTColumnInputModel> newColumnList = new List<StructureCRTColumnInputModel>();
            newColumnList.AddRange(columnList);
            newColumnList.Add(new StructureCRTColumnInputModel());

            List<StructureCRTGirderInputModel> newGirderList = new List<StructureCRTGirderInputModel>();
            newGirderList.Add(new StructureCRTGirderInputModel());
            newGirderList.AddRange(girderList);
            newGirderList.Add(new StructureCRTGirderInputModel());

            List<HBeamModel> newGirderHBeamList = new List<HBeamModel>();
            newGirderHBeamList.Add(new HBeamModel());
            newGirderHBeamList.AddRange(girderHbeamList);
            newGirderHBeamList.Add(new HBeamModel());





            StructureModel newStrModel = new StructureModel();

            // 1. Create Layer : Column, Rafter, Girder
            for(int layerIndex = 0; layerIndex < newColumnList.Count; layerIndex++)
            {
                // Layer
                StructureLayerModel newLayer = new StructureLayerModel();
                newLayer.Number = 0;
                
                newLayer.StartAngle = 0;

                // Column
                StructureCRTColumnInputModel eachColumn = newColumnList[layerIndex];
                double columnQty = GetDoubleValue(eachColumn.Qty);
                double columnRadius = GetDoubleValue(eachColumn.Radius);
                double columnSize = GetDoubleValue(eachColumn.Size);
                double columnAngleOne = 0;
                if (columnQty > 0)
                    columnAngleOne = 360 / columnQty;
                for (int columnIndex = 0; columnIndex < columnQty; columnIndex++)
                {
                    StructureColumnModel newColumn = new StructureColumnModel();
                    newColumn.Radius = columnRadius;
                    newColumn.AngleOne = columnAngleOne;
                    newColumn.Height = 0;   // 나중에 구함
                    newColumn.Size = columnSize;

                    newLayer.ColumnList.Add(newColumn);
                }

                // Layer : Radius
                newLayer.Radius = columnRadius;



                // Rafter
                StructureCRTRafterInputModel eachRafter = newRafterList[layerIndex];
                double rafterQty = GetDoubleValue(eachRafter.Qty);
                double rafterRadius = GetDoubleValue(eachRafter.Radius);
                string rafterSize = eachRafter.Size;
                double rafterAngleOne = 0;
                if (rafterQty > 0)
                    rafterAngleOne = 360 / rafterQty;
                for (int rafterIndex = 0; rafterIndex < rafterQty; rafterIndex++)
                {
                    StructureRafterModel newRafter = new StructureRafterModel();
                    newRafter.AngleOne = rafterAngleOne;
                    newRafter.Length = 0; // 나중에 구함
                    newRafter.Size = rafterSize;
                    newLayer.RafterList.Add(newRafter);
                }


                // Girder + Clip
                HBeamModel eachGriderHBeam = newGirderHBeamList[layerIndex];
                double girderWidth = GetDoubleValue(eachGriderHBeam.B);
                double girderHeight = GetDoubleValue(eachGriderHBeam.A);

                StructureCRTGirderInputModel eachGirder = newGirderList[layerIndex];
                double girderQty = GetDoubleValue(eachGirder.Qty);
                double girderRadius = GetDoubleValue(eachGirder.Radius);
                string girderSize = eachGirder.Size;
                double girderAngleOne = 0;
                if (girderQty > 0)
                    girderAngleOne = 360 / girderQty;
                for (int girderIndex = 0; girderIndex < girderQty; girderIndex++)
                {
                    StructureGirderModel newGirder = new StructureGirderModel();
                    newGirder.Radius = girderRadius;
                    newGirder.AngleOne = girderAngleOne;
                    newGirder.Size = girderSize;


                    newGirder.Length = 0; // 나중에 구함
                    newGirder.Width = girderWidth;
                    newGirder.Height = girderHeight;

                    newLayer.GirderList.Add(newGirder);
                }



                newStrModel.LayerList.Add(newLayer);
            }



            // 2. Grider
            foreach(StructureLayerModel eachLayer in newStrModel.LayerList)
            {
                for(int girderIndex = 0; girderIndex < eachLayer.GirderList.Count; girderIndex++)
                {
                    StructureGirderModel eachGirder = eachLayer.GirderList[girderIndex];
                    //double GirderLength = geoService.GetStringLengthByArcAngle(Column1R, Column1Angle) - (GirderReduce * 2); //Radius와 Angle로 현의 길이 구한 후 - (200 * 2)
                }
            }

            // AngleFromCenter를 재 계산



            double roofOD = 32000;
            double columCount = 4;

            // 1. Create Layer
            //newStrModel.LayerList.AddRange(GetLayerList(roofOD,columCount));




            return newStrModel;
        }

        public List<StructureLayerModel> GetLayerList()
        {

            List<StructureLayerModel> newList = new List<StructureLayerModel>();



            return newList;
        }





        public double GetDoubleValue(string selValue)
        {
            // Default Value
            double doubleValue = 0;

            if (!double.TryParse(selValue, out doubleValue))
                doubleValue = 0;
            return doubleValue;
        }
    }
}
