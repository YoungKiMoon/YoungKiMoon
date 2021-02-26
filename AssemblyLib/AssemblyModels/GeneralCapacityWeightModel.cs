﻿using AssemblyLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyLib.AssemblyModels
{
    public class GeneralCapacityWeightModel : Notifier
    {
        public GeneralCapacityWeightModel()
        {

            Empty = "";
            Operating = "";
            FullOfWater = "";
            Insulation = "";
            PlatformLadder = "";
            Others = "";
            Liquid = "";
            PaintingAreaInt = "";
            PaintingAreaExt = "";
            NominalCapacity = "";
            WorkingCapacity = "";
            NetWorkingCapacity = "";
        }

        private string _Empty;
        public string Empty
        {
            get { return _Empty; }
            set
            {
                _Empty = value;
                OnPropertyChanged(nameof(Empty));
            }
        }

        private string _Operating;
        public string Operating
        {
            get { return _Operating; }
            set
            {
                _Operating = value;
                OnPropertyChanged(nameof(Operating));
            }
        }

        private string _FullOfWater;
        public string FullOfWater
        {
            get { return _FullOfWater; }
            set
            {
                _FullOfWater = value;
                OnPropertyChanged(nameof(FullOfWater));
            }
        }

        private string _Insulation;
        public string Insulation
        {
            get { return _Insulation; }
            set
            {
                _Insulation = value;
                OnPropertyChanged(nameof(Insulation));
            }
        }

        private string _PlatformLadder;
        public string PlatformLadder
        {
            get { return _PlatformLadder; }
            set
            {
                _PlatformLadder = value;
                OnPropertyChanged(nameof(PlatformLadder));
            }
        }

        private string _Others;
        public string Others
        {
            get { return _Others; }
            set
            {
                _Others = value;
                OnPropertyChanged(nameof(Others));
            }
        }

        private string _Liquid;
        public string Liquid
        {
            get { return _Liquid; }
            set
            {
                _Liquid = value;
                OnPropertyChanged(nameof(Liquid));
            }
        }

        private string _PaintingAreaInt;
        public string PaintingAreaInt
        {
            get { return _PaintingAreaInt; }
            set
            {
                _PaintingAreaInt = value;
                OnPropertyChanged(nameof(PaintingAreaInt));
            }
        }

        private string _PaintingAreaExt;
        public string PaintingAreaExt
        {
            get { return _PaintingAreaExt; }
            set
            {
                _PaintingAreaExt = value;
                OnPropertyChanged(nameof(PaintingAreaExt));
            }
        }

        private string _NominalCapacity;
        public string NominalCapacity
        {
            get { return _NominalCapacity; }
            set
            {
                _NominalCapacity = value;
                OnPropertyChanged(nameof(NominalCapacity));
            }
        }

        private string _WorkingCapacity;
        public string WorkingCapacity
        {
            get { return _WorkingCapacity; }
            set
            {
                _WorkingCapacity = value;
                OnPropertyChanged(nameof(WorkingCapacity));
            }
        }

        private string _NetWorkingCapacity;
        public string NetWorkingCapacity
        {
            get { return _NetWorkingCapacity; }
            set
            {
                _NetWorkingCapacity = value;
                OnPropertyChanged(nameof(NetWorkingCapacity));
            }
        }

    }








}
