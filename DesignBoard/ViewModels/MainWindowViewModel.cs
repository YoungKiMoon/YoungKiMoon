using AssemblyLib.AssemblyModels;
using DesignBoard.Models;
using DesignBoard.Utils;
using DrawWork.AssemblyServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace DesignBoard.ViewModels
{
    public class MainWindowViewModel : Notifier
    {
        public string designBoardVersion
        {
            get { return _designBoardVersion; }
            set
            {
                _designBoardVersion = value;
                OnPropertyChanged(nameof(designBoardVersion));
            }
        }
        private string _designBoardVersion;

        public CheckListModel checkList
        {
            get { return _checkList; }
            set
            {
                _checkList = value;
                OnPropertyChanged(nameof(checkList));
            }
        }
        private CheckListModel _checkList;



        public FileModel engineeringFile
        {
            get { return _engineeringFile; }
            set
            {
                _engineeringFile = value;
                OnPropertyChanged(nameof(engineeringFile));
            }
        }
        private FileModel _engineeringFile;


        public string loadingString
        {
            get { return _loadingString; }
            set
            {
                _loadingString = value;
                OnPropertyChanged(nameof(loadingString));
            }
        }
        private string _loadingString;


        // Assembly Model

        public AssemblyModel assemblyData
        {
            get { return _assemblyData; }
            set
            {
                _assemblyData = value;
                OnPropertyChanged(nameof(assemblyData));
            }
        }
        private AssemblyModel _assemblyData;

        #region DATA
        public ObservableCollection<AMECheckModel> AMEList
        {
            get { return _AMEList; }
            set
            {
                _AMEList = value;
                OnPropertyChanged(nameof(AMEList));
            }
        }
        private ObservableCollection<AMECheckModel> _AMEList;

        public ObservableCollection<DataListModel> DataList
        {
            get { return _DataList; }
            set
            {
                _DataList = value;
                OnPropertyChanged(nameof(DataList));
            }
        }
        private ObservableCollection<DataListModel> _DataList;

        public List<ImageModel> ImageList;

        public ObservableCollection<NozzleModel> NozzleList
        {
            get { return _NozzleList; }
            set
            {
                _NozzleList = value;
                OnPropertyChanged(nameof(NozzleList));
            }
        }
        private ObservableCollection<NozzleModel> _NozzleList;

        public ObservableCollection<DataCheckModel> DataCheckList
        {
            get { return _DataCheckList; }
            set
            {
                _DataCheckList = value;
                OnPropertyChanged(nameof(DataCheckList));
            }
        }
        private ObservableCollection<DataCheckModel> _DataCheckList;

        #endregion




        public MainWindowViewModel()
        {
            designBoardVersion = "[ Ver 1.315 ]";
            checkList = new CheckListModel();

            engineeringFile = new FileModel();
            loadingString = "";

            assemblyData = new AssemblyModel();

            SetSampleData();
            SetImageList();

        }
        private void SetImageList()
        {
            // Compression Ring
            List<ImageModel> newImage = new List<ImageModel>();
            newImage.Add(new ImageModel("compressionRing_detail_b", "Detail b"));
            newImage.Add(new ImageModel("compressionRing_detail_d", "Detail d"));
            newImage.Add(new ImageModel("CompressionRing_detail_e(new)", "Detail e"));
            newImage.Add(new ImageModel("compressionRing_detail_i", "Detail i"));
            newImage.Add(new ImageModel("compressionRing_detail_k", "Detail k"));

            ImageList = new List<ImageModel>();
            foreach (ImageModel eachImage in newImage)
                ImageList.Add(new ImageModel("/DesignBoard;component/AssemblyImage/" + eachImage.ImagePath + ".png", eachImage.ImageName));
        }

        private void SetSampleData()
        {
            AMEList = new ObservableCollection<AMECheckModel>();
            if (false)
            {
                AMEList.Add(new AMECheckModel() { Name = "APPLIED CODE", Value = "API-650 13th Edition, March 2020", Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "APPENDICES USED", Value = "E, F, V", Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "SHELL DESIGN", Value = "One Foot (1ft)", Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "ROOF DESIGN", Value = "API 650 & AISC", Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "ROOF STRUCTURE DESIGN", Value = "AISC", Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "DESIGN SP. GR.", Value = "0.88", Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "ROOF TYPE", Value = "CONE", Information = "CRT" });
                AMEList.Add(new AMECheckModel() { Name = "ID of Tank", Value = "32400", Information = "32400 mm" });
                AMEList.Add(new AMECheckModel() { Name = "Height", Value = "19300", Information = "19300 mm" });
                AMEList.Add(new AMECheckModel() { Name = "DESIGN TEMP. (MIN)", Value = "-15", Information = "-15 ºC" });
                AMEList.Add(new AMECheckModel() { Name = "DESIGN TEMP. (MAX)", Value = "85", Information = "85 ºC" });
                AMEList.Add(new AMECheckModel() { Name = "DESIGN INTERNAL PRESSURE", Value = "150", Information = "150 mmh2o" });
                AMEList.Add(new AMECheckModel() { Name = "DESIGN EXTERNAL PRESSURE", Value = "-50", Information = "-50 mmh2o" });
                AMEList.Add(new AMECheckModel() { Name = "WIND SHEAR", Value = "05,159.401", Information = "05,159.401 N" });
                AMEList.Add(new AMECheckModel() { Name = "WIND MOMENT", Value = "3,910,007.716 N-m", Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "SEISMIC SHEAR", Value = "990000000", Information = "Range Error", Error = true });
                AMEList.Add(new AMECheckModel() { Name = "SEISMIC MOMENT (RINGWALL)", Value = "88,607,934.176", Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "SEISMIC MOMENT (SLAB)", Value = "16,266,832.328", Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "DESIGN WIND SPEED", Value = "45 m/s", Information = "Value Error", Error = true });
                AMEList.Add(new AMECheckModel() { Name = "BOTTOM PLATE LENGTH", Value = "", Information = "Blank", Error = true });
                AMEList.Add(new AMECheckModel() { Name = "BOTTOM PLATE WELD TYPE", Value = "Lap joint", Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "BOTTOM ANNULAR TYPE", Value = "Circular", Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "BOTTOM DRIP RING THICKNESS", Value = "75", Information = "" });
            }
            DataList = new ObservableCollection<DataListModel>();
            if (true)
            {
                DataListModel newModel01 = new DataListModel();
                if (true)
                {
                    newModel01.Name = "GENERAL";
                    newModel01.List.Add(new DataModel() { Name = "ROOF TYPE", Value = "CRT", Description = "" });
                    newModel01.List.Add(new DataModel() { Name = "NOMINAL ID", Value = "32400", Description = "" });
                    newModel01.List.Add(new DataModel() { Name = "TANK HEIGHT", Value = "19300", Description = "" });
                    newModel01.List.Add(new DataModel() { Name = "BOTTOM PLATE SLOPE", Value = "1/100", Description = "" });
                    newModel01.List.Add(new DataModel() { Name = "ROOF PLATE SLOPE", Value = "1/16", Description = "" });

                    newModel01.List.Add(new DataModel() { Name = "HIGH HIGH LIQUID LEVEL (HHLL)", Value = "17500", Description = "" });
                    newModel01.List.Add(new DataModel() { Name = "HIGH LIQUID LEVEL (HLL)", Value = "17000", Description = "" });
                    newModel01.List.Add(new DataModel() { Name = "LOW LIQUID LEVEL (LLL)", Value = "2400", Description = "" });
                    newModel01.List.Add(new DataModel() { Name = "LOW LOW LIQUID LEVEL (LLLL)", Value = "2250", Description = "" });
                }
                DataListModel newModel02 = new DataListModel();
                if (true)
                {
                    newModel02.Name = "ROOF";
                    newModel02.List.Add(new DataModel() { Name = "Roof Slope", Value = "1/16", Description = "" });
                    newModel02.List.Add(new DataModel() { Name = "Plate : Thickness ", Value = "6", Description = "" });
                    newModel02.List.Add(new DataModel() { Name = "Plate : Arrangement Type", Value = "Low type", Description = "" });
                    newModel02.List.Add(new DataModel() { Name = "Plate : Weld Type", Value = "Lap joint", Description = "" });
                    newModel02.List.Add(new DataModel() { Name = "Compression Ring Type", Value = "Detail k", Description = "" });

                }
                DataListModel newModel03 = new DataListModel();
                if (true)
                {
                    newModel03.Name = "STRUCTURE";
                    newModel03.List.Add(new DataModel() { Name = "Roof Slope", Value = "1/16", Description = "" });
                    newModel03.List.Add(new DataModel() { Name = "Supporting Type", Value = "Rafter w/Column", Description = "" });
                    newModel03.List.Add(new DataModel() { Name = "Centering OD", Value = "970", Description = "" });
                    newModel03.List.Add(new DataModel() { Name = "Centering Height(H)", Value = "350", Description = "" });


                }

                DataListModel newModel04 = new DataListModel();
                if (true)
                {
                    newModel04.Name = "SHELL";
                    newModel04.List.Add(new DataModel() { Name = "Shell Plate Width", Value = "2720", Description = "" });
                    newModel04.List.Add(new DataModel() { Name = "Shell Plate Max. Length", Value = "10000", Description = "" });
                    newModel04.List.Add(new DataModel() { Name = "Insulation Thickness", Value = "NONE", Description = "" });
                    newModel04.List.Add(new DataModel() { Name = "Insulation Density", Value = "", Description = "" });
                    newModel04.List.Add(new DataModel() { Name = "Insulation Type", Value = "", Description = "" });


                }

                DataListModel newModel05 = new DataListModel();
                if (true)
                {
                    newModel05.Name = "BOTTOM";
                    newModel05.List.Add(new DataModel() { Name = "Slope", Value = "1/100", Description = "" });
                    newModel05.List.Add(new DataModel() { Name = "Thickness", Value = "9", Description = "" });
                    newModel05.List.Add(new DataModel() { Name = "Width", Value = "2438", Description = "" });
                    newModel05.List.Add(new DataModel() { Name = "Length", Value = "6096", Description = "" });
                    newModel05.List.Add(new DataModel() { Name = "Arrangement type", Value = "Cone Up", Description = "" });
                    newModel05.List.Add(new DataModel() { Name = "Weld Type", Value = "Lap joint", Description = "" });
                    newModel05.List.Add(new DataModel() { Name = "Annular Plate", Value = "1831", Description = "" });
                    newModel05.List.Add(new DataModel() { Name = "Annular Thickness", Value = "15", Description = "" });
                    newModel05.List.Add(new DataModel() { Name = "Annular Type", Value = "Circular", Description = "" });



                }

                DataListModel newModel06 = new DataListModel();
                if (true)
                {
                    newModel06.Name = "ACCESS";
                    newModel06.List.Add(new DataModel() { Name = "Spiral Stairway vs Ladder", Value = "Spiral Stairway", Description = "" });
                    newModel06.List.Add(new DataModel() { Name = "Spiral Stairway : Start Angle", Value = "30", Description = "" });
                    newModel06.List.Add(new DataModel() { Name = "Ladder : Start Angle", Value = "", Description = "" });
                    newModel06.List.Add(new DataModel() { Name = "Roof Platform : Elevation", Value = "1000", Description = "" });
                    newModel06.List.Add(new DataModel() { Name = "Foam Maintenance Platform", Value = "YES", Description = "" });


                }
                DataListModel newModel07 = new DataListModel();
                if (true)
                {
                    newModel07.Name = "APPURTENANCES";
                    newModel07.List.Add(new DataModel() { Name = "Name Plate", Value = "Yes", Description = "" });
                    newModel07.List.Add(new DataModel() { Name = "Settlement Check Piece", Value = "Yes", Description = "" });
                    newModel07.List.Add(new DataModel() { Name = "Water Spray", Value = "Yes", Description = "" });
                    newModel07.List.Add(new DataModel() { Name = "Earth Lug", Value = "Yes", Description = "" });
                    newModel07.List.Add(new DataModel() { Name = "Scaffold Cable Support", Value = "Yes", Description = "" });


                }


                DataList.Add(newModel01);
                DataList.Add(newModel02);
                DataList.Add(newModel04);
                DataList.Add(newModel05);
                DataList.Add(newModel03);
                DataList.Add(newModel06);
                DataList.Add(newModel07);

            }

            NozzleList = new ObservableCollection<NozzleModel>();

            DataCheckList = new ObservableCollection<DataCheckModel>();
            if (true)
            {
                DataCheckList.Add(new DataCheckModel() { Type = "INFO", Component = "General", Name = "", Value = "", Information = "85 Data / 5 Blank" });
                DataCheckList.Add(new DataCheckModel() { Type = "ERROR", Component = "General", Name = "Tank Height", Value = "2400", Information = "Minimum : 4000" });
                DataCheckList.Add(new DataCheckModel() { Type = "INFO", Component = "Roof", Name = "", Value = "", Information = "CRT Type" });
                DataCheckList.Add(new DataCheckModel() { Type = "WARNNING", Component = "Wind Girder", Name = "Elevation #2", Value = "40000", Information = "Out of Range" });
                DataCheckList.Add(new DataCheckModel() { Type = "WARNNING", Component = "Compression Ring", Name = "", Value = "", Information = "Blank" });
                DataCheckList.Add(new DataCheckModel() { Type = "ERROR", Component = "Structure", Name = "Rafter #1", Value = "H800x800", Information = "Out of Data" });

            }

        }



        // Assembly Data
        public void SetAssemblyData()
        {
            if (engineeringFile.FullPath!="")
            {
                if (File.Exists(engineeringFile.FullPath))
                {
                    // Assembly
                    AssemblyDataService assemblyService = new AssemblyDataService();
                    //AssemblyModel newTankData = assemblyService.CreateMappingData(ExcelFile.Text);

                    // New Excel Read
                    assemblyData = assemblyService.CreateMappingDataNew(engineeringFile.FullPath);

                    SetAMEList();
                }

            }
        }

        public void SetAMEList()
        {
            AMEList.Clear();

            if (assemblyData.GeneralDesignData.Count > 0)
            {
                AMEList.Add(new AMECheckModel() { Name = "CODE APPLIED", Value = assemblyData.GeneralDesignData[0].CodeApplied, Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "APPENDICES USED", Value = assemblyData.GeneralDesignData[0].AppendicesUsed, Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "SHELL DESIGN", Value = assemblyData.GeneralDesignData[0].ShellDesign, Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "ROOF DESIGN", Value = assemblyData.GeneralDesignData[0].RoofDesign, Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "ROOF STRUCTURE DESIGN", Value = assemblyData.GeneralDesignData[0].RoofStructureDesign, Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "CONTENTS", Value = assemblyData.GeneralDesignData[0].Contents, Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "DESIGN SPECIFIC. GR.", Value = assemblyData.GeneralDesignData[0].DesignSpecGR, Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "MEASUREMENT UNIT", Value = assemblyData.GeneralDesignData[0].MeasurementUnit, Information = "" });

                AMEList.Add(new AMECheckModel() { Name = "ROOF TYPE", Value = assemblyData.GeneralDesignData[0].RoofType, Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "NOMINAL ID", Value = assemblyData.GeneralDesignData[0].SizeNominalID, Information = "mm" });
                AMEList.Add(new AMECheckModel() { Name = "TANK HEIGHT", Value = assemblyData.GeneralDesignData[0].SizeTankHeight, Information = "mm" });

                AMEList.Add(new AMECheckModel() { Name = "DESIGN TEMP. MIN.", Value = assemblyData.GeneralDesignData[0].DesignTempMin, Information = "℃" });
                AMEList.Add(new AMECheckModel() { Name = "DESIGN TEMP. MAX.", Value = assemblyData.GeneralDesignData[0].DesignTempMax, Information = "℃" });
                AMEList.Add(new AMECheckModel() { Name = "DESIGN PRESS. INT.", Value = assemblyData.GeneralDesignData[0].DesignPressInt, Information = "mmH2O" });
                AMEList.Add(new AMECheckModel() { Name = "DESIGN PRESS. EXT.", Value = assemblyData.GeneralDesignData[0].DesignPressExt, Information = "mmH2O" });
                AMEList.Add(new AMECheckModel() { Name = "TEST SPECIFIC. GR.", Value = assemblyData.GeneralDesignData[0].TestSPGR, Information = "" });


                AMEList.Add(new AMECheckModel() { Name = "SHELL PLATE", Value = assemblyData.GeneralMaterialSpecifications[0].ShellPlate, Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "BOTTOM PLATE", Value = assemblyData.GeneralMaterialSpecifications[0].BottomPlate, Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "ANNULAR PLATE", Value = assemblyData.GeneralMaterialSpecifications[0].AnnularPlate, Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "ROOF PLATE", Value = assemblyData.GeneralMaterialSpecifications[0].RoofPlate, Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "FLOATING ROOF PLATE", Value = assemblyData.GeneralMaterialSpecifications[0].FloatingRoofPlate, Information = "" });


                AMEList.Add(new AMECheckModel() { Name = "SHELL PLATE (C.A)", Value = assemblyData.GeneralCorrosionLoading[0].ShellPlate, Information = "mm" });
                AMEList.Add(new AMECheckModel() { Name = "ROOF PLATE (C.A)", Value = assemblyData.GeneralCorrosionLoading[0].RoofPlate, Information = "mm" });
                AMEList.Add(new AMECheckModel() { Name = "BOTTOM PLATE (C.A)", Value = assemblyData.GeneralCorrosionLoading[0].BottomPlate, Information = "mm" });
                AMEList.Add(new AMECheckModel() { Name = "ANNULAR PLATE (C.A)", Value = assemblyData.GeneralCorrosionLoading[0].AnnularPlate, Information = "mm" });
                AMEList.Add(new AMECheckModel() { Name = "NOZZLE (C.A)", Value = assemblyData.GeneralCorrosionLoading[0].Nozzle, Information = "mm" });

                AMEList.Add(new AMECheckModel() { Name = "STRUCTURE (EACH SIDE) (C.A)", Value = assemblyData.GeneralCorrosionLoading[0].StructureEachSide, Information = "mm" });
                AMEList.Add(new AMECheckModel() { Name = "COLUMN (EACH SIDE) (C.A)", Value = assemblyData.GeneralCorrosionLoading[0].ColumnEachSide, Information = "mm" });
                AMEList.Add(new AMECheckModel() { Name = "DECK PLATE UPPER (C.A)", Value = assemblyData.GeneralCorrosionLoading[0].DeskPlateUpper, Information = "mm" });
                AMEList.Add(new AMECheckModel() { Name = "DESK PLATE LOWER (C.A)", Value = assemblyData.GeneralCorrosionLoading[0].DeskPlateLower, Information = "mm" });

                AMEList.Add(new AMECheckModel() { Name = "HIGH HIGH LIQUID LEVEL (HHLL)", Value = assemblyData.GeneralLiquidCapacityWeight[0].HighHIghLiquidLevel, Information = "mm" });
                AMEList.Add(new AMECheckModel() { Name = "HIGH LIQUID LEVEL (HLL)", Value = assemblyData.GeneralLiquidCapacityWeight[0].HighLiquidLevel, Information = "mm" });
                AMEList.Add(new AMECheckModel() { Name = "LOW LIQUID LEVEL (LLL)", Value = assemblyData.GeneralLiquidCapacityWeight[0].LowLiquidLevel, Information = "mm" });
                AMEList.Add(new AMECheckModel() { Name = "LOW LOW LIQUID LEVEL (LLLL)", Value = assemblyData.GeneralLiquidCapacityWeight[0].LowLowLiquidLevel, Information = "mm" });


                AMEList.Add(new AMECheckModel() { Name = "WIND CODE APPLIED", Value = assemblyData.GeneralWind[0].CodeApplied, Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "WIND DESIGN WIND SPEED", Value = assemblyData.GeneralWind[0].DesignWindSpeed, Information = "m/s" });
                AMEList.Add(new AMECheckModel() { Name = "WIND EXPOSURE", Value = assemblyData.GeneralWind[0].Exposure, Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "IMPORTANCE FACTOR", Value = assemblyData.GeneralWind[0].ImportanceFactor, Information = "" });

                AMEList.Add(new AMECheckModel() { Name = "SEISMIC CODE APPLIED", Value = assemblyData.GeneralSeismic[0].CodeApplied, Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "SEISMIC USE GROUP", Value = assemblyData.GeneralSeismic[0].SeismicUseGroup, Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "SEISMIC SITE CLASSIFICATION", Value = assemblyData.GeneralSeismic[0].SiteClassification, Information = "" });
                AMEList.Add(new AMECheckModel() { Name = "SEISMIC IMPORTANCE FACTOR", Value = assemblyData.GeneralSeismic[0].ImportanceFactor, Information = "" });


                AMEList.Add(new AMECheckModel() { Name = "EMPTY WEIGHT", Value = assemblyData.GeneralLiquidCapacityWeight[0].Empty, Information = "ton" });
                AMEList.Add(new AMECheckModel() { Name = "OPERATING WEIGHT", Value = assemblyData.GeneralLiquidCapacityWeight[0].Operating, Information = "ton" });
                AMEList.Add(new AMECheckModel() { Name = "TEST WEIGHT", Value = assemblyData.GeneralLiquidCapacityWeight[0].fullOfWater, Information = "ton" });


                AMEList.Add(new AMECheckModel() { Name = "WIND SHEAR", Value = assemblyData.GeneralCorrosionLoading[0].WindShear, Information = "N" });
                AMEList.Add(new AMECheckModel() { Name = "WIND MOMENT", Value = assemblyData.GeneralCorrosionLoading[0].WindMoment, Information = "N-m" });
                AMEList.Add(new AMECheckModel() { Name = "SEISMIC SHEAR", Value = assemblyData.GeneralCorrosionLoading[0].SeismicShear, Information = "N" });
                AMEList.Add(new AMECheckModel() { Name = "SEISMIC MOMENT (RINGWALL)", Value = assemblyData.GeneralCorrosionLoading[0].SeismicMomentRingWall, Information = "N-m" });
                AMEList.Add(new AMECheckModel() { Name = "SEISMIC MOMENT (SLAB)", Value = assemblyData.GeneralCorrosionLoading[0].SeismicMomentSlab, Information = "N-m" });


                //AMEList.Add(new AMECheckModel() { Name = "DESK", Value = assemblyData.GeneralDesignData[0].CodeApplied, Information = "" });


            }

        }

    }
}
