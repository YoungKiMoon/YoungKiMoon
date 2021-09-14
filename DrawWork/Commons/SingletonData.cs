using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DrawSettingLib.SettingModels;
using DrawWork.CutomModels;
using DrawWork.DrawModels;

namespace DrawWork.Commons
{
    public class SingletonData
    {
        public SingletonData()
        {
        }


        // Dimension
        private static List<DrawDimensionsModel> _DimensionsList = new List<DrawDimensionsModel>();
        public static List<DrawDimensionsModel> DimensionsList
        {
            get { return _DimensionsList; }
            set { _DimensionsList = value; }
        }

        // Paper : Grid Visible
        private static bool _PaperGridVisible = false;
        public static bool PaperGridVisible
        {
            get { return _PaperGridVisible; }
            set { _PaperGridVisible = value; }
        }


        // BM Model
        public static List<DrawBMModel> _BMList = new List<DrawBMModel>();
        public static List<DrawBMModel> BMList
        {
            get { return _BMList; }
            set { _BMList = value; }
        }

        // Model Area : GA
        private static GAAreaModel _GAArea = new GAAreaModel();
        public static GAAreaModel GAArea
        {
            get { return _GAArea; }
            set { _GAArea = value; }
        }

        // Paper Area : List
        public static PaperAreaListModel _PaperArea = new PaperAreaListModel();
        public static PaperAreaListModel PaperArea
        {
            get { return _PaperArea; }
            set { _PaperArea = value; }
        }

        public static TANK_TYPE TankType
        {
            get { return _TankType; }
            set { _TankType = value; }
        }
        private static TANK_TYPE _TankType = TANK_TYPE.CRT;


        public static CDPoint RefPoint
        {
            get { return _RefPoint; }
            set { _RefPoint = value; }
        }
        private static CDPoint _RefPoint = null;

        public static List<LeaderPointModel> LeaderPublicList
        {
            get { return _LeaderPublicList; }
            set { _LeaderPublicList = value; }
        }
        private static List<LeaderPointModel> _LeaderPublicList = new List<LeaderPointModel>();

        public static List<DimensionPointModel> DimPublicList
        {
            get { return _DimPublicList; }
            set { _DimPublicList = value; }
        }
        private static List<DimensionPointModel> _DimPublicList = new List<DimensionPointModel>();


        // GA ViewPort
        public static CDPoint GAViewPortSize
        {
            get { return _GAViewPortSize; }
            set { _GAViewPortSize = value; }
        }
        private static CDPoint _GAViewPortSize = new CDPoint();

        public static CDPoint GAViewPortCenter
        {
            get { return _GAViewPortCenter; }
            set { _GAViewPortCenter = value; }
        }
        private static CDPoint _GAViewPortCenter = new CDPoint();

        public static CDPoint OrientationViewPortSize
        {
            get { return _OrientationViewPortSize; }
            set { _OrientationViewPortSize = value; }
        }
        private static CDPoint _OrientationViewPortSize = new CDPoint();

        public static CDPoint OrientationGAViewPortCenter
        {
            get { return _OrientationGAViewPortCenter; }
            set { _OrientationGAViewPortCenter = value; }
        }
        private static CDPoint _OrientationGAViewPortCenter = new CDPoint();



        // Temp
        public static List<string> RoofBottomArrange
        {
            get { return _RoofBottomArrange; }
            set { _RoofBottomArrange = value; }
        }
        private static List<string> _RoofBottomArrange = new List<string>();

        // Temp : Arrange
        public static List<DrawPlateModel> BottomPlateInfo
        {
            get { return _BottomPlateInfo; }
            set { _BottomPlateInfo = value; }
        }
        private static List<DrawPlateModel> _BottomPlateInfo = new List<DrawPlateModel>();
        public static List<DrawPlateModel> RoofPlateInfo
        {
            get { return _RoofPlateInfo; }
            set { _RoofPlateInfo = value; }
        }
        private static List<DrawPlateModel> _RoofPlateInfo = new List<DrawPlateModel>();

        // 사용 안함
        public static List<DrawCuttingAreaModel> BottomCuttingList
        {
            get { return _BottomCuttingList; }
            set { _BottomCuttingList = value; }
        }
        private static List<DrawCuttingAreaModel> _BottomCuttingList = new List<DrawCuttingAreaModel>();

        // 사용 안함
        public static List<DrawCuttingAreaModel> RoofCuttingList
        {
            get { return _RoofCuttingList; }
            set { _RoofCuttingList = value; }
        }
        private static List<DrawCuttingAreaModel> _RoofCuttingList = new List<DrawCuttingAreaModel>();


        public static List<DrawOnePlateModel> BottomPlateList
        {
            get { return _BottomPlateList; }
            set { _BottomPlateList = value; }
        }
        private static List<DrawOnePlateModel> _BottomPlateList = new List<DrawOnePlateModel>();

        public static List<DrawOnePlateModel> RoofPlateList
        {
            get { return _RoofPlateList; }
            set { _RoofPlateList = value; }
        }
        private static List<DrawOnePlateModel> _RoofPlateList = new List<DrawOnePlateModel>();


        public static List<DrawOnePlateModel> BottomAnnularPlateList
        {
            get { return _BottomAnnularPlateList; }
            set { _BottomAnnularPlateList = value; }
        }
        private static List<DrawOnePlateModel> _BottomAnnularPlateList = new List<DrawOnePlateModel>();

        public static List<DrawOnePlateModel> RoofComRingPlateList
        {
            get { return _RoofComRingPlateList; }
            set { _RoofComRingPlateList = value; }
        }
        private static List<DrawOnePlateModel> _RoofComRingPlateList = new List<DrawOnePlateModel>();



        // Method
        public static void Clear()
        {
            LeaderPublicList.Clear();
            DimPublicList.Clear();


            BMList.Clear();


            BottomPlateInfo.Clear();
            RoofPlateInfo.Clear();
            BottomPlateList.Clear();
            RoofPlateList.Clear();


            BottomAnnularPlateList.Clear();
            RoofComRingPlateList.Clear();

            // 사용 안함
            BottomCuttingList.Clear();
            RoofCuttingList.Clear();


        }


    }
}
