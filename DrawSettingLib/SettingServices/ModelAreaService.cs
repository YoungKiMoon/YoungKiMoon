using DrawSettingLib.SettingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingServices
{
    public class ModelAreaService
    {
        public GAAreaModel GetModelAreaData()
        {
            GAAreaModel newArea = new GAAreaModel();

            //2021-07-16 New Scale
            newArea.Dimension.AreaSize.Width = 50;
            newArea.Dimension.AreaSize.Height = 50;

            newArea.NozzleLeader.AreaSize.Width = 35;
            newArea.NozzleLeader.AreaSize.Height = 35;

            newArea.ShellCourse.AreaSize.Width = 40;
            newArea.ShellCourse.AreaSize.Height = 0;

            return newArea;
        }
    }
}
