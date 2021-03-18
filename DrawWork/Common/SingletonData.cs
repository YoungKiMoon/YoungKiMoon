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

        private static GAAreaModel _GAArea = new GAAreaModel();
        public static GAAreaModel GAArea
        {
            get { return _GAArea; }
            set { _GAArea = value; }
        }
    }
}
