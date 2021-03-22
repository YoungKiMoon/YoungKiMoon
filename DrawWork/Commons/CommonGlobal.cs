using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.Commons
{
    public static class CommonGlobal
    {
        public static string OutLine="";
        public static string CenterLine = "";
        public static string DimLine = "";
        public static string DimText = "";
        public static string DimLineExt = "";
        public static string DimArrow = "";
        public static string LeaderLine = "";
        public static string LeaderText = "";
        public static string LeaderArrow = "";
        public static string NozzleLine = "";
        public static string NozzleMark = "";
        public static string NozzleText = "";

        #region CONSTRUCTOR
        static CommonGlobal()
        {
            OutLine = "outline";
            CenterLine = "centerline";
            DimLine = "dimline";
            DimText = "dimtext";
            DimLineExt = "dimlineext";
            DimArrow = "dimarrow";
            LeaderLine = "leaderline";
            LeaderText = "leadertext";
            LeaderArrow = "leaderarrow";
            NozzleLine = "nozzleline";
            NozzleMark = "nozzlemark";
            NozzleText = "nozzletext";
        }
        #endregion
    }
}
