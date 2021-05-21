using AssemblyLib.AssemblyModels;
using DrawWork.CommandServices;
using DrawWork.Commons;
using DrawWork.DrawServices;
using DrawWork.ValueServices;
using ExcelDataLib.ExcelModels;
using ExcelDataLib.ExcelServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.AssemblyServices
{
    public class AssemblyDataService
    {
        private ValueService valueService;
        public AssemblyDataService()
        {
            valueService = new ValueService();
        }

        public AssemblyModel CreateMappingData(string selFileName)
        {

            AssemblyModel selAssembly = new AssemblyModel();
            selAssembly.CreateMappingAssembly();

            ExcelApplicationService eDataService = new ExcelApplicationService(-1);
            ObservableCollection<ExcelWorkSheetModel> newSheetList= eDataService.GetSheetListAll(selFileName);

            eDataService.GetSheetData(selAssembly, newSheetList);


            // Tank Type
            SetTankType(selAssembly);

            // Roof Top Angle
            SetRoofTopAngeCompressionRing(selAssembly);

            // SetRoofInsulation
            SetRoofInsulation(selAssembly);

            // Adjust List Data
            AdjustmentListData(selAssembly);

            // Create OutputData
            CreateOutputData(selAssembly);




            return selAssembly;
        }

        private void SetTankType(AssemblyModel selAssembly)
        {
            int firstIndex = 0;
            switch (selAssembly.GeneralDesignData[firstIndex].RoofType.ToLower())
            {
                case "crt":
                    SingletonData.TankType = TANK_TYPE.CRT;
                    break;
                case "drt":
                    SingletonData.TankType = TANK_TYPE.DRT;
                    break;
                case "ifrt":
                    SingletonData.TankType = TANK_TYPE.IFRT;
                    break;
                case "efrt_singledeck":
                    SingletonData.TankType = TANK_TYPE.EFRTSingle;
                    break;
                case "efrt_doubledeck":
                    SingletonData.TankType = TANK_TYPE.EFRTDouble;
                    break;
            }
        }

        public void SetRoofTopAngeCompressionRing(AssemblyModel selAssembly)
        {
            int firstIndex = 0;
            selAssembly.RoofCompressionRing.Clear();
            RoofCompressionRingModel newModel = new RoofCompressionRingModel();

            switch (SingletonData.TankType)
            {
                case TANK_TYPE.CRT:
                    newModel.RoofSlope = selAssembly.RoofCRTInput[firstIndex].RoofSlope;
                    newModel.CompressionRingType = selAssembly.RoofCRTInput[firstIndex].CompressionRingType;
                    switch (newModel.CompressionRingType)
                    {
                        case "Detail b":
                            newModel.AngleSize = valueService.GetAllTrim(selAssembly.RoofCRTInput[firstIndex].DeatilBAngleSize);
                            break;
                        case "Detail d":
                            newModel.AngleSize = valueService.GetAllTrim(selAssembly.RoofCRTInput[firstIndex].DetailDAngleSize);
                            break;
                        case "Detail e":
                            newModel.AngleSize = valueService.GetAllTrim(selAssembly.RoofCRTInput[firstIndex].DetailEAngleSize);
                            break;
                        case "Detail i":
                            newModel.ThicknessT1 = valueService.GetAllTrim(selAssembly.RoofCRTInput[firstIndex].DetailIThickness);
                            newModel.WidthB = valueService.GetAllTrim(selAssembly.RoofCRTInput[firstIndex].DetailIWidth);
                            newModel.OutsideProjectionA = valueService.GetAllTrim(selAssembly.RoofCRTInput[firstIndex].DetailIOutsideProjection);
                            newModel.OverlapOfRoofAndCompRingC = valueService.GetAllTrim(selAssembly.RoofCRTInput[firstIndex].DetailIOverlap);
                            break;
                        case "Detail k":
                            newModel.ThicknessC = valueService.GetAllTrim(selAssembly.RoofCRTInput[firstIndex].DetailKThickness);
                            newModel.WidthD = valueService.GetAllTrim(selAssembly.RoofCRTInput[firstIndex].DetailKWidth);
                            newModel.ShellThicknessThickenedT1 = valueService.GetAllTrim(selAssembly.RoofCRTInput[firstIndex].DetailKShellThickness);
                            newModel.ShellWidthThickenedA = valueService.GetAllTrim(selAssembly.RoofCRTInput[firstIndex].DetailKShellWidth);
                            newModel.DistanceFromShellTopCourseB = valueService.GetAllTrim(selAssembly.RoofCRTInput[firstIndex].DetailKDistance);
                            break;
                    }
                    break;
                case TANK_TYPE.DRT:
                    newModel.DomeRadiusRatio = selAssembly.RoofDRTInput[firstIndex].DomeRadiusRatio;
                    newModel.CompressionRingType = selAssembly.RoofDRTInput[firstIndex].CompressionRingType;
                    switch (newModel.CompressionRingType)
                    {
                        case "Detail b":
                            newModel.AngleSize = valueService.GetAllTrim(selAssembly.RoofDRTInput[firstIndex].DeatilBAngleSize);
                            break;
                        case "Detail d":
                            newModel.AngleSize = valueService.GetAllTrim(selAssembly.RoofDRTInput[firstIndex].DetailDAngleSize);
                            break;
                        case "Detail e":
                            newModel.AngleSize = valueService.GetAllTrim(selAssembly.RoofDRTInput[firstIndex].DetailEAngleSize);
                            break;
                        case "Detail i":
                            newModel.ThicknessT1 = valueService.GetAllTrim(selAssembly.RoofDRTInput[firstIndex].DetailIThickness);
                            newModel.WidthB = valueService.GetAllTrim(selAssembly.RoofDRTInput[firstIndex].DetailIWidth);
                            newModel.OutsideProjectionA = valueService.GetAllTrim(selAssembly.RoofDRTInput[firstIndex].DetailIOutsideProjection);
                            newModel.OverlapOfRoofAndCompRingC = valueService.GetAllTrim(selAssembly.RoofDRTInput[firstIndex].DetailIOverlap);
                            break;
                        case "Detail k":
                            newModel.ThicknessC = valueService.GetAllTrim(selAssembly.RoofDRTInput[firstIndex].DetailKThickness);
                            newModel.WidthD = valueService.GetAllTrim(selAssembly.RoofDRTInput[firstIndex].DetailKWidth);
                            newModel.ShellThicknessThickenedT1 = valueService.GetAllTrim(selAssembly.RoofDRTInput[firstIndex].DetailKShellThickness);
                            newModel.ShellWidthThickenedA = valueService.GetAllTrim(selAssembly.RoofDRTInput[firstIndex].DetailKShellWidth);
                            newModel.DistanceFromShellTopCourseB = valueService.GetAllTrim(selAssembly.RoofDRTInput[firstIndex].DetailKDistance);
                            break;
                    }
                    break;
                case TANK_TYPE.IFRT:
                    newModel.RoofSlope = selAssembly.RoofIFRTInput[firstIndex].RoofSlope;
                    newModel.CompressionRingType = selAssembly.RoofIFRTInput[firstIndex].CompressionRingType;
                    switch (newModel.CompressionRingType)
                    {
                        case "Detail b":
                            newModel.AngleSize = valueService.GetAllTrim(selAssembly.RoofIFRTInput[firstIndex].DeatilBAngleSize);
                            break;
                        case "Detail d":
                            newModel.AngleSize = valueService.GetAllTrim(selAssembly.RoofIFRTInput[firstIndex].DetailDAngleSize);
                            break;
                        case "Detail e":
                            newModel.AngleSize = valueService.GetAllTrim(selAssembly.RoofIFRTInput[firstIndex].DetailEAngleSize);
                            break;
                        case "Detail i":
                            newModel.ThicknessT1 = valueService.GetAllTrim(selAssembly.RoofIFRTInput[firstIndex].DetailIThickness);
                            newModel.WidthB = valueService.GetAllTrim(selAssembly.RoofIFRTInput[firstIndex].DetailIWidth);
                            newModel.OutsideProjectionA = valueService.GetAllTrim(selAssembly.RoofIFRTInput[firstIndex].DetailIOutsideProjection);
                            newModel.OverlapOfRoofAndCompRingC = valueService.GetAllTrim(selAssembly.RoofIFRTInput[firstIndex].DetailIOverlap);
                            break;
                        case "Detail k":
                            newModel.ThicknessC = valueService.GetAllTrim(selAssembly.RoofIFRTInput[firstIndex].DetailKThickness);
                            newModel.WidthD = valueService.GetAllTrim(selAssembly.RoofIFRTInput[firstIndex].DetailKWidth);
                            newModel.ShellThicknessThickenedT1 = valueService.GetAllTrim(selAssembly.RoofIFRTInput[firstIndex].DetailKShellThickness);
                            newModel.ShellWidthThickenedA = valueService.GetAllTrim(selAssembly.RoofIFRTInput[firstIndex].DetailKShellWidth);
                            newModel.DistanceFromShellTopCourseB = valueService.GetAllTrim(selAssembly.RoofIFRTInput[firstIndex].DetailKDistance);
                            break;
                    }
                    break;
                case TANK_TYPE.EFRTSingle:
                    newModel.CompressionRingType = selAssembly.RoofEFRTSingleDeck[firstIndex].CompressionRingType;
                    switch (newModel.CompressionRingType)
                    {
                        case "Detail b":
                            newModel.AngleSize = valueService.GetAllTrim(selAssembly.RoofEFRTSingleDeck[firstIndex].DeatilBAngleSize);
                            break;
                        case "Detail d":
                            newModel.AngleSize = valueService.GetAllTrim(selAssembly.RoofEFRTSingleDeck[firstIndex].DetailDAngleSize);
                            break;
                    }
                    break;
                case TANK_TYPE.EFRTDouble:
                    newModel.CompressionRingType = selAssembly.RoofEFRTDoubleDeck[firstIndex].CompressionRingType;
                    switch (newModel.CompressionRingType)
                    {
                        case "Detail b":
                            newModel.AngleSize = valueService.GetAllTrim(selAssembly.RoofEFRTDoubleDeck[firstIndex].DeatilBAngleSize);
                            break;
                        case "Detail d":
                            newModel.AngleSize = valueService.GetAllTrim(selAssembly.RoofEFRTDoubleDeck[firstIndex].DetailDAngleSize);
                            break;
                    }
                    break;
            }

            selAssembly.RoofCompressionRing.Add(newModel);
        }

        public void SetRoofInsulation(AssemblyModel selAssembly)
        {
            int firstIndex = 0;
            RoofInsulationModel newModel = new RoofInsulationModel();
            switch (SingletonData.TankType)
            {
                case TANK_TYPE.CRT:
                    newModel.Required = selAssembly.RoofCRTInput[firstIndex].InsulationRequired;
                    newModel.Thickness = selAssembly.RoofCRTInput[firstIndex].InsulationThickness;
                    newModel.Density = selAssembly.RoofCRTInput[firstIndex].InsulationDensity;
                    newModel.Type = selAssembly.RoofCRTInput[firstIndex].InsulationType;
                    break;
                case TANK_TYPE.DRT:
                    newModel.Required = selAssembly.RoofDRTInput[firstIndex].InsulationRequired;
                    newModel.Thickness = selAssembly.RoofDRTInput[firstIndex].InsulationThickness;
                    newModel.Density = selAssembly.RoofDRTInput[firstIndex].InsulationDensity;
                    newModel.Type = selAssembly.RoofDRTInput[firstIndex].InsulationType;
                    break;
                case TANK_TYPE.IFRT:
                    newModel.Required = selAssembly.RoofIFRTInput[firstIndex].InsulationRequired;
                    newModel.Thickness = selAssembly.RoofIFRTInput[firstIndex].InsulationThickness;
                    newModel.Density = selAssembly.RoofIFRTInput[firstIndex].InsulationDensity;
                    newModel.Type = selAssembly.RoofIFRTInput[firstIndex].InsulationType;
                    break;
                case TANK_TYPE.EFRTSingle:
                    newModel.Required = selAssembly.RoofEFRTSingleDeck[firstIndex].InsulationRequired;
                    newModel.Thickness = selAssembly.RoofEFRTSingleDeck[firstIndex].InsulationThickness;
                    newModel.Density = selAssembly.RoofEFRTSingleDeck[firstIndex].InsulationDensity;
                    newModel.Type = selAssembly.RoofEFRTSingleDeck[firstIndex].InsulationType;
                    break;
                case TANK_TYPE.EFRTDouble:
                    newModel.Required = selAssembly.RoofEFRTDoubleDeck[firstIndex].InsulationRequired;
                    newModel.Thickness = selAssembly.RoofEFRTDoubleDeck[firstIndex].InsulationThickness;
                    newModel.Density = selAssembly.RoofEFRTDoubleDeck[firstIndex].InsulationDensity;
                    newModel.Type = selAssembly.RoofEFRTDoubleDeck[firstIndex].InsulationType;
                    break;
            }
            selAssembly.RoofInsulation.Add(newModel);
        }

        private void CreateOutputData(AssemblyModel selAssembly)
        {
            // Auto create output data
            // output Data 자동으로 구성하기 : AC로 표시


            // Input list : first index
            int firstIndex = 0;

            #region Roof : Angle
            selAssembly.RoofAngleOutput.Clear();
            string acTopAngleType = "";
            string acTopAngleSize = "";

            switch (SingletonData.TankType) 
            {
                case TANK_TYPE.CRT:
                    acTopAngleType = selAssembly.RoofCRTInput[firstIndex].CompressionRingType;
                    switch (acTopAngleType)
                    {
                        case "Detail b":
                            acTopAngleSize = valueService.GetAllTrim(selAssembly.RoofCRTInput[firstIndex].DeatilBAngleSize);
                            break;
                        case "Detail d":
                            acTopAngleSize = valueService.GetAllTrim(selAssembly.RoofCRTInput[firstIndex].DetailDAngleSize);
                            break;
                        case "Detail e":
                            acTopAngleSize = valueService.GetAllTrim(selAssembly.RoofCRTInput[firstIndex].DetailEAngleSize);
                            break;
                    }
                    break;
                case TANK_TYPE.DRT:
                    acTopAngleType = selAssembly.RoofDRTInput[firstIndex].CompressionRingType;
                    switch (acTopAngleType)
                    {
                        case "Detail b":
                            acTopAngleSize = valueService.GetAllTrim(selAssembly.RoofDRTInput[firstIndex].DeatilBAngleSize);
                            break;
                        case "Detail d":
                            acTopAngleSize = valueService.GetAllTrim(selAssembly.RoofDRTInput[firstIndex].DetailDAngleSize);
                            break;
                        case "Detail e":
                            acTopAngleSize = valueService.GetAllTrim(selAssembly.RoofDRTInput[firstIndex].DetailEAngleSize);
                            break;
                    }
                    break;
                case TANK_TYPE.IFRT:
                    acTopAngleType = selAssembly.RoofIFRTInput[firstIndex].CompressionRingType;
                    switch (acTopAngleType)
                    {
                        case "Detail b":
                            acTopAngleSize = valueService.GetAllTrim(selAssembly.RoofIFRTInput[firstIndex].DeatilBAngleSize);
                            break;
                        case "Detail d":
                            acTopAngleSize = valueService.GetAllTrim(selAssembly.RoofIFRTInput[firstIndex].DetailDAngleSize);
                            break;
                        case "Detail e":
                            acTopAngleSize = valueService.GetAllTrim(selAssembly.RoofIFRTInput[firstIndex].DetailEAngleSize);
                            break;
                    }
                    break;
                case TANK_TYPE.EFRTSingle:
                    acTopAngleType = selAssembly.RoofEFRTSingleDeck[firstIndex].CompressionRingType;
                    switch (acTopAngleType)
                    {
                        case "Detail b":
                            acTopAngleSize = valueService.GetAllTrim(selAssembly.RoofEFRTSingleDeck[firstIndex].DeatilBAngleSize);
                            break;
                        case "Detail d":
                            acTopAngleSize = valueService.GetAllTrim(selAssembly.RoofEFRTSingleDeck[firstIndex].DetailDAngleSize);
                            break;
                    }
                    break;
                case TANK_TYPE.EFRTDouble:
                    acTopAngleType = selAssembly.RoofEFRTDoubleDeck[firstIndex].CompressionRingType;
                    switch (acTopAngleType)
                    {
                        case "Detail b":
                            acTopAngleSize = valueService.GetAllTrim(selAssembly.RoofEFRTDoubleDeck[firstIndex].DeatilBAngleSize);
                            break;
                        case "Detail d":
                            acTopAngleSize = valueService.GetAllTrim(selAssembly.RoofEFRTDoubleDeck[firstIndex].DetailDAngleSize);
                            break;
                    }
                    break;
            }

            foreach (AngleSizeModel eachAngle in selAssembly.AngleIList)
            {
                if (valueService.GetAllTrim(eachAngle.SIZE) == acTopAngleSize)
                {
                    selAssembly.RoofAngleOutput.Add(eachAngle);
                    break;
                }
            }

            #endregion


            #region Structure


            // Type
            DrawStructureService StructureDivService = new DrawStructureService();
            string stSupportingType = "";
            switch (SingletonData.TankType)
            {
                case TANK_TYPE.CRT:
                    stSupportingType = selAssembly.StructureCRTInput[0].SupportingType;
                    break;
                case TANK_TYPE.DRT:
                    stSupportingType = selAssembly.StructureDRTInput[0].SupportingType;
                    break;
                case TANK_TYPE.IFRT:
                    stSupportingType = selAssembly.StructureIFRTInput[0].SupportingType;
                    break;
                case TANK_TYPE.EFRTSingle:
                    break;
                case TANK_TYPE.EFRTDouble:
                    break;
            }

            // Structure Related Type
            StructureDivService.SetStructureData(SingletonData.TankType, stSupportingType, acTopAngleType);

            if (SingletonData.TankType == TANK_TYPE.CRT)
            {
                #region CRT
                // Structure Column Rafter Output
                selAssembly.StructureCRTColumnRafterOutput.Clear();
                for (int i = 0; i < selAssembly.StructureCRTRafterInput.Count; i++)
                {
                    string rafterSize = valueService.GetAllTrim(selAssembly.StructureCRTRafterInput[i].Size);
                    if (rafterSize != "")
                    {
                        foreach (StructureCRTColumnRafterModel eachRafter in selAssembly.StructureCRTColumnRafterList)
                        {
                            if (StructureDivService.topAngleType == eachRafter.Type) // top angle
                            {
                                if (valueService.GetAllTrim(eachRafter.SIZE) == rafterSize)
                                {
                                    selAssembly.StructureCRTColumnRafterOutput.Add(eachRafter);
                                    break;
                                }
                            }
                        }
                    }
                }


                // Structure : Clip Shell Side
                selAssembly.StructureCRTClipShellSideOutput.Clear();
                string acRafterSizeLast = valueService.GetAllTrim(selAssembly.StructureCRTRafterInput[selAssembly.StructureCRTRafterInput.Count - 1].Size);
                foreach (StructureClipShellSideModel eachClip in selAssembly.StructureClipShellSideList)
                {
                    if (eachClip.TankType == StructureDivService.tankType)      // Tank Type
                        if (eachClip.Type == StructureDivService.columnType)        // Column type
                        {
                            if (StructureDivService.columnType == "column")
                            {
                                if (eachClip.Angle == StructureDivService.topAngleType)     // Top Angle
                                    if (valueService.GetAllTrim(eachClip.SIZE) == acRafterSizeLast)
                                    {
                                        selAssembly.StructureCRTClipShellSideOutput.Add(eachClip);
                                    }
                            }
                            else
                            {
                                if (valueService.GetAllTrim(eachClip.SIZE) == acRafterSizeLast)
                                {
                                    selAssembly.StructureCRTClipShellSideOutput.Add(eachClip);
                                }
                            }

                        }


                }


                //int acStructureColumnCount = valueService.GetIntValue(selAssembly.StructureInput[firstIndex].ColumnNo);

                // Structure : Column Center Top Support : One
                // Structure : Column Pipe : Column Count
                // structure : Column Base Support : Column Count
                selAssembly.StructureCRTColumnCenterOutput.Clear();
                selAssembly.StructureCRTColumnPipeOutput.Clear();
                selAssembly.StructureCRTColumnBaseSupportOutput.Clear();
                for (int i = 0; i < selAssembly.StructureCRTColumnInput.Count; i++)
                {
                    string eachColumnSize = "";
                    string eachRafterSize = "";
                    if (selAssembly.StructureCRTColumnInput.Count > i)
                        eachColumnSize = valueService.GetAllTrim(selAssembly.StructureCRTColumnInput[i].Size);
                    if (selAssembly.StructureCRTRafterInput.Count > i)
                        eachRafterSize = valueService.GetAllTrim(selAssembly.StructureCRTRafterInput[i].Size);

                    if (i == 0 && eachColumnSize != "" && eachRafterSize != "")
                    {
                        foreach (StructureCRTColumnCenterModel eachCenter in selAssembly.StructureCRTColumnCenterList)
                        {
                            if (valueService.GetAllTrim(eachCenter.COLUMN) == eachColumnSize && valueService.GetAllTrim(eachCenter.SIZE) == eachRafterSize)
                            {
                                selAssembly.StructureCRTColumnCenterOutput.Add(eachCenter);
                                break;
                            }
                        }
                    }
                    if (eachColumnSize != "")
                    {
                        foreach (PipeModel eachPipe in selAssembly.PipeList)
                        {
                            if (eachPipe.NPS == eachColumnSize)
                            {
                                selAssembly.StructureCRTColumnPipeOutput.Add(eachPipe);
                            }
                        }
                        foreach (StructureCRTColumnBaseSupportModel eachBase in selAssembly.StructureCRTColumnBaseSupportList)
                        {
                            if(eachBase.Size==eachColumnSize)
                                selAssembly.StructureCRTColumnBaseSupportOutput.Add(eachBase); // 1개만 있음
                        }
                    }
                }

                // Structure : Column Side Top Support : Column Count -1
                selAssembly.StructureCRTColumnSideOutput.Clear();
                for (int i = 0; i < selAssembly.StructureCRTRafterInput.Count; i++) // 0 에서 부터 시작
                {
                    string rafterSize = valueService.GetAllTrim(selAssembly.StructureCRTRafterInput[i].Size);
                    if (rafterSize != "")
                    {
                        foreach (StructureCRTColumnSideModel eachSide in selAssembly.StructureCRTColumnSideList)
                        {
                            if (valueService.GetAllTrim(eachSide.SIZE) == rafterSize)
                            {
                                selAssembly.StructureCRTColumnSideOutput.Add(eachSide);
                            }
                        }
                    }
                }

                // Structure : Column Girder : Column Count -1
                selAssembly.StructureCRTColumnHBeamOutput.Clear();
                for (int i = 0; i < selAssembly.StructureCRTGirderInput.Count; i++)
                {
                    string girderSize = valueService.GetAllTrim(selAssembly.StructureCRTGirderInput[i].Size);
                    if (girderSize != "")
                    {
                        foreach (HBeamModel eachHBeam in selAssembly.HBeamList)
                        {
                            if (valueService.GetAllTrim(eachHBeam.SIZE) == girderSize)
                            {
                                selAssembly.StructureCRTColumnHBeamOutput.Add(eachHBeam);
                            }
                        }
                    }

                }

                //Structure : Clip Slot Hole -> 각 테이블로 귀속 시킴
                //selAssembly.StructureCRTClipSlotHoleOutput.Clear();
                //foreach (ClipSlotHoleModel eachHole in selAssembly.ClipSlotHoleList)
                //{
                //    if (eachHole.Div == "ColumnCenterTop")
                //    {
                //        selAssembly.StructureClipSlotHoleOutput.Add(eachHole);
                //    }
                //}


                // Structure : Centering
                selAssembly.StructureCRTCenteringOutput.Clear();
                string acRafterSizeFirst = valueService.GetAllTrim(selAssembly.StructureCRTRafterInput[0].Size);
                foreach (StructureCenteringModel eachCentering in selAssembly.StructureCenteringList)
                {
                    if (eachCentering.TankType.Contains(StructureDivService.tankType))
                        if (eachCentering.InEx == StructureDivService.centeringInEx)
                            if (acRafterSizeFirst == eachCentering.SIZE)
                                selAssembly.StructureCRTCenteringOutput.Add(eachCentering);

                }

                // Structure : ClipCenteringSide
                selAssembly.StructureCRTClipCenteringSideOutput.Clear();
                foreach (StructureClipCenteringSideModel eachClip in selAssembly.StructureClipCenteringSideList)
                {
                    if (eachClip.TankType.Contains(StructureDivService.tankType))
                        if (eachClip.SIZE == acRafterSizeFirst)
                            selAssembly.StructureCRTClipCenteringSideOutput.Add(eachClip);
                }

                // Structure : Centering Rafter
                selAssembly.StructureCRTCenteringRaterOutput.Clear();
                foreach (StructureCenteringRafterModel eachRafter in selAssembly.StructureCenteringRafterList)
                {
                    if (eachRafter.TankType == StructureDivService.tankType)
                        if (eachRafter.InEx == StructureDivService.centeringInEx)
                            if (eachRafter.SIZE == acRafterSizeFirst)
                                selAssembly.StructureCRTCenteringRaterOutput.Add(eachRafter);

                }

                // Structure : Center Purlin
                selAssembly.StructureCRTCenteringPurlinOutput.Clear();
                string centeringPurlin = selAssembly.StructureCRTCenterRingInput[firstIndex].PurlinSize;
                foreach (AngleSizeModel eachAngle in selAssembly.AngleIList)
                {
                    if (eachAngle.SIZE == centeringPurlin)
                        selAssembly.StructureCRTCenteringPurlinOutput.Add(eachAngle);
                }

                #endregion
            }
            else if(SingletonData.TankType == TANK_TYPE.DRT)
            {
                #region DRT
                #endregion
            }

            #endregion
        }

        private void AdjustmentListData(AssemblyModel selAssembly)
        {
            // Shell Course
            for (int i = selAssembly.ShellOutput.Count - 1; i >= 0; i--)
            {
                ShellOutputModel newModel = selAssembly.ShellOutput[i];
                if (newModel.PlateWidth == "" &&
                   newModel.Material == "" &&
                   newModel.ImpactTest == "" &&
                   newModel.Thickness == "")
                {
                    selAssembly.ShellOutput.Remove(newModel);
                }
            }

            // Wind Girder
            for (int i = selAssembly.WindGirderOutput.Count - 1; i >= 0; i--)
            {
                WindGirderOutputModel newModel = selAssembly.WindGirderOutput[i];
                if (newModel.Type == "" &&
                   newModel.Material == "" &&
                   newModel.Elevation == "" &&
                   newModel.Size == "")
                {
                    selAssembly.WindGirderOutput.Remove(newModel);
                }
            }

            // CRT : Rafter
            for (int i = selAssembly.StructureCRTRafterInput.Count - 1; i >= 0; i--)
            {
                StructureCRTRafterInputModel newModel = selAssembly.StructureCRTRafterInput[i];
                if (newModel.Radius == "" &&
                   newModel.Qty == "" &&
                   newModel.Size == "" &&
                   newModel.TotalQty == "")
                {
                    selAssembly.StructureCRTRafterInput.Remove(newModel);
                }
            }
            // CRT : Girder
            for (int i = selAssembly.StructureCRTGirderInput.Count - 1; i >= 0; i--)
            {
                StructureCRTGirderInputModel newModel = selAssembly.StructureCRTGirderInput[i];
                if (newModel.Radius == "" &&
                   newModel.Qty == "" &&
                   newModel.Size == "")
                {
                    selAssembly.StructureCRTGirderInput.Remove(newModel);
                }
            }

            // CRT : Column
            for (int i = selAssembly.StructureCRTColumnInput.Count - 1; i >= 0; i--)
            {
                StructureCRTColumnInputModel newModel = selAssembly.StructureCRTColumnInput[i];
                if (newModel.Radius == "" &&
                   newModel.Qty == "" &&
                   newModel.Size == "" &&
                   newModel.Schedule == "")
                {
                    selAssembly.StructureCRTColumnInput.Remove(newModel);
                }
            }

            // DRT : Rafter
            for (int i = selAssembly.StructureDRTRafterInput.Count - 1; i >= 0; i--)
            {
                StructureDRTRafterInputModel newModel = selAssembly.StructureDRTRafterInput[i];
                if (newModel.Radius == "" &&
                   newModel.Qty == "" &&
                   newModel.Size == "" &&
                   newModel.TotalQty == "")
                {
                    selAssembly.StructureDRTRafterInput.Remove(newModel);
                }
            }
            // DRT : Girder
            for (int i = selAssembly.StructureDRTGirderInput.Count - 1; i >= 0; i--)
            {
                StructureDRTGirderInputModel newModel = selAssembly.StructureDRTGirderInput[i];
                if (newModel.Radius == "" &&
                   newModel.Qty == "" &&
                   newModel.Size == "")
                {
                    selAssembly.StructureDRTGirderInput.Remove(newModel);
                }
            }
        }
    }
}
