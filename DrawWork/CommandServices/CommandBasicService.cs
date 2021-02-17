using DrawWork.CommandModels;
using DrawWork.DrawModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.CommandServices
{
    public class CommandBasicService
    {
        public BasicCommandModel commandData;

        #region CONSTRUCTOR
        public CommandBasicService()
        {
            commandData = new BasicCommandModel();
        }
        #endregion
    }
}
