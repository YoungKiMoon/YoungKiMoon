using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DrawSettingLib.SettingModels;

namespace DrawWork.Commons
{
    public class SingletonData
    {
        public SingletonData()
        {

        }

        private static GAAreaModel _GAArea = new GAAreaModel();
        public static GAAreaModel GAArea
        {
            get { return _GAArea; }
            set { _GAArea = value; }
        }

        public static TANK_TYPE TankType = TANK_TYPE.CRT;
    }
}
