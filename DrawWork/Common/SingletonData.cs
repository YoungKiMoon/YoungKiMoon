using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DrawSettingLib.SettingModels;

namespace DrawWork.Common
{
    public class SingletonData
    {
        public SingletonData()
        {

        }

        private static GADrawAreaModel _GADrawArea = new GADrawAreaModel();
        public static GADrawAreaModel GADrawArea
        {
            get { return _GADrawArea; }
            set { _GADrawArea = value; }
        }
    }
}
