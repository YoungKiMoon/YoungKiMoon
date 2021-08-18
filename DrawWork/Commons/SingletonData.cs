﻿using System;
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

        // Model Area : GA
        private static GAAreaModel _GAArea = new GAAreaModel();
        public static GAAreaModel GAArea
        {
            get { return _GAArea; }
            set { _GAArea = value; }
        }

        // Paper Area : List
        public static List<PaperAreaModel> _PaperArea = new List<PaperAreaModel>();
        public static List<PaperAreaModel> PaperArea
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

    }
}
