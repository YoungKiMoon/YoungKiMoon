using AssemblyLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyLib.AssemblyModels
{
    public class GeneralDesignDataModel : Notifier
    {
        public GeneralDesignDataModel()
        {
            AppliedCode="";
            ShellDesign = "";
            RoofDesign = "";
            Contetns = "";
            DesignSpecGr = "";

            MeasurementUnit = "";
            RoofType = "";
            SizeNominalID = "";
            SizeTankHeight = "";
            PlateWidth = "";

            PlateMaxLength = "";
            PumpingRatesIn = "";
            PumpingRatesOut = "";
            OperTempMin = "";
            OperTempNor = "";

            OperTempMax = "";
            DesignTempMin = "";
            DesignTempMax = "";
            OperPressInt = "";
            OperPressExt = "";

            DesignPressInt = "";
            DesignPressExt = "";
            VaporPressureMax = "";
            SetPressureEmergencyCoverManshole = "";
            SetPressureBreatherValve = "";

            SetPressureBreatherValveVac = "";
            TestSpGr = "";
            RoofLoadsUniformLive = "";
            RoofLoadsSpecialLoading = "";
            WindVelocity = "";

            RainFallMax = "";
            SnowFallTotalAccumulation = "";
            FoundationType = "";
            InsulationShell = "";
            InsulationRoof = "";

            MDMT = "";
            DMT = "";

        }

        private string _AppliedCode;
        public string AppliedCode
        {
            get { return _AppliedCode; }
            set
            {
                _AppliedCode = value;
                OnPropertyChanged(nameof(AppliedCode));
            }
        }

        private string _ShellDesign;
        public string ShellDesign
        {
            get { return _ShellDesign; }
            set
            {
                _ShellDesign = value;
                OnPropertyChanged(nameof(ShellDesign));
            }
        }

        private string _RoofDesign;
        public string RoofDesign
        {
            get { return _RoofDesign; }
            set
            {
                _RoofDesign = value;
                OnPropertyChanged(nameof(RoofDesign));
            }
        }

        private string _Contetns;
        public string Contetns
        {
            get { return _Contetns; }
            set
            {
                _Contetns = value;
                OnPropertyChanged(nameof(Contetns));
            }
        }

        private string _DesignSpecGr;
        public string DesignSpecGr
        {
            get { return _DesignSpecGr; }
            set
            {
                _DesignSpecGr = value;
                OnPropertyChanged(nameof(DesignSpecGr));
            }
        }

        private string _MeasurementUnit;
        public string MeasurementUnit
        {
            get { return _MeasurementUnit; }
            set
            {
                _MeasurementUnit = value;
                OnPropertyChanged(nameof(MeasurementUnit));
            }
        }


        private string _RoofType;
        public string RoofType
        {
            get { return _RoofType; }
            set
            {
                _RoofType = value;
                OnPropertyChanged(nameof(RoofType));
            }
        }

        private string _SizeNominalID;
        public string SizeNominalID
        {
            get { return _SizeNominalID; }
            set
            {
                _SizeNominalID = value;
                OnPropertyChanged(nameof(SizeNominalID));
            }
        }

        private string _SizeTankHeight;
        public string SizeTankHeight
        {
            get { return _SizeTankHeight; }
            set
            {
                _SizeTankHeight = value;
                OnPropertyChanged(nameof(SizeTankHeight));
            }
        }

        private string _PlateWidth;
        public string PlateWidth
        {
            get { return _PlateWidth; }
            set
            {
                _PlateWidth = value;
                OnPropertyChanged(nameof(PlateWidth));
            }
        }

        private string _PlateMaxLength;
        public string PlateMaxLength
        {
            get { return _PlateMaxLength; }
            set
            {
                _PlateMaxLength = value;
                OnPropertyChanged(nameof(PlateMaxLength));
            }
        }


        private string _PumpingRatesIn;
        public string PumpingRatesIn
        {
            get { return _PumpingRatesIn; }
            set
            {
                _PumpingRatesIn = value;
                OnPropertyChanged(nameof(PumpingRatesIn));
            }
        }

        private string _PumpingRatesOut;
        public string PumpingRatesOut
        {
            get { return _PumpingRatesOut; }
            set
            {
                _PumpingRatesOut = value;
                OnPropertyChanged(nameof(PumpingRatesOut));
            }
        }

        private string _OperTempMin;
        public string OperTempMin
        {
            get { return _OperTempMin; }
            set
            {
                _OperTempMin = value;
                OnPropertyChanged(nameof(OperTempMin));
            }
        }

        private string _OperTempNor;
        public string OperTempNor
        {
            get { return _OperTempNor; }
            set
            {
                _OperTempNor = value;
                OnPropertyChanged(nameof(OperTempNor));
            }
        }

        private string _OperTempMax;
        public string OperTempMax
        {
            get { return _OperTempMax; }
            set
            {
                _OperTempMax = value;
                OnPropertyChanged(nameof(OperTempMax));
            }
        }

        private string _DesignTempMin;
        public string DesignTempMin
        {
            get { return _DesignTempMin; }
            set
            {
                _DesignTempMin = value;
                OnPropertyChanged(nameof(DesignTempMin));
            }
        }

        private string _DesignTempMax;
        public string DesignTempMax
        {
            get { return _DesignTempMax; }
            set
            {
                _DesignTempMax = value;
                OnPropertyChanged(nameof(DesignTempMax));
            }
        }

        private string _OperPressInt;
        public string OperPressInt
        {
            get { return _OperPressInt; }
            set
            {
                _OperPressInt = value;
                OnPropertyChanged(nameof(OperPressInt));
            }
        }

        private string _OperPressExt;
        public string OperPressExt
        {
            get { return _OperPressExt; }
            set
            {
                _OperPressExt = value;
                OnPropertyChanged(nameof(OperPressExt));
            }
        }

        private string _DesignPressInt;
        public string DesignPressInt
        {
            get { return _DesignPressInt; }
            set
            {
                _DesignPressInt = value;
                OnPropertyChanged(nameof(DesignPressInt));
            }
        }

        private string _DesignPressExt;
        public string DesignPressExt
        {
            get { return _DesignPressExt; }
            set
            {
                _DesignPressExt = value;
                OnPropertyChanged(nameof(DesignPressExt));
            }
        }

        private string _VaporPressureMax;
        public string VaporPressureMax
        {
            get { return _VaporPressureMax; }
            set
            {
                _VaporPressureMax = value;
                OnPropertyChanged(nameof(VaporPressureMax));
            }
        }

        private string _SetPressureEmergencyCoverManshole;
        public string SetPressureEmergencyCoverManshole
        {
            get { return _SetPressureEmergencyCoverManshole; }
            set
            {
                _SetPressureEmergencyCoverManshole = value;
                OnPropertyChanged(nameof(SetPressureEmergencyCoverManshole));
            }
        }

        private string _SetPressureBreatherValve;
        public string SetPressureBreatherValve
        {
            get { return _SetPressureBreatherValve; }
            set
            {
                _SetPressureBreatherValve = value;
                OnPropertyChanged(nameof(SetPressureBreatherValve));
            }
        }

        private string _SetPressureBreatherValveVac;
        public string SetPressureBreatherValveVac
        {
            get { return _SetPressureBreatherValveVac; }
            set
            {
                _SetPressureBreatherValveVac = value;
                OnPropertyChanged(nameof(SetPressureBreatherValveVac));
            }
        }


        private string _TestSpGr;
        public string TestSpGr
        {
            get { return _TestSpGr; }
            set
            {
                _TestSpGr = value;
                OnPropertyChanged(nameof(TestSpGr));
            }
        }

        private string _RoofLoadsUniformLive;
        public string RoofLoadsUniformLive
        {
            get { return _RoofLoadsUniformLive; }
            set
            {
                _RoofLoadsUniformLive = value;
                OnPropertyChanged(nameof(RoofLoadsUniformLive));
            }
        }

        private string _RoofLoadsSpecialLoading;
        public string RoofLoadsSpecialLoading
        {
            get { return _RoofLoadsSpecialLoading; }
            set
            {
                _RoofLoadsSpecialLoading = value;
                OnPropertyChanged(nameof(RoofLoadsSpecialLoading));
            }
        }


        private string _WindVelocity;
        public string WindVelocity
        {
            get { return _WindVelocity; }
            set
            {
                _WindVelocity = value;
                OnPropertyChanged(nameof(WindVelocity));
            }
        }

        private string _RainFallMax;
        public string RainFallMax
        {
            get { return _RainFallMax; }
            set
            {
                _RainFallMax = value;
                OnPropertyChanged(nameof(RainFallMax));
            }
        }

        private string _SnowFallTotalAccumulation;
        public string SnowFallTotalAccumulation
        {
            get { return _SnowFallTotalAccumulation; }
            set
            {
                _SnowFallTotalAccumulation = value;
                OnPropertyChanged(nameof(SnowFallTotalAccumulation));
            }
        }

        private string _FoundationType;
        public string FoundationType
        {
            get { return _FoundationType; }
            set
            {
                _FoundationType = value;
                OnPropertyChanged(nameof(FoundationType));
            }
        }


        private string _InsulationShell;
        public string InsulationShell
        {
            get { return _InsulationShell; }
            set
            {
                _InsulationShell = value;
                OnPropertyChanged(nameof(InsulationShell));
            }
        }

        private string _InsulationRoof;
        public string InsulationRoof
        {
            get { return _InsulationRoof; }
            set
            {
                _InsulationRoof = value;
                OnPropertyChanged(nameof(InsulationRoof));
            }
        }

        private string _MDMT;
        public string MDMT
        {
            get { return _MDMT; }
            set
            {
                _MDMT = value;
                OnPropertyChanged(nameof(MDMT));
            }
        }

        private string _DMT;
        public string DMT
        {
            get { return _DMT; }
            set
            {
                _DMT = value;
                OnPropertyChanged(nameof(DMT));
            }
        }

    }
}
