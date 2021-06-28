using DesignBoard.Models;
using DesignBoard.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            designBoardVersion = "[ Ver 1.16 ]";
            checkList = new CheckListModel();


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
            if (true)
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


                DataList.Add(newModel01);
                DataList.Add(newModel02);
                DataList.Add(newModel03);

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


    }
}
