using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using DrawWork.CommandModels;
using DrawWork.CommandServices;
using DrawWork.DrawBuilders;

using AssemblyLib.AssemblyModels;

namespace DrawWork.ViewModels
{
    public class MainWindowViewModel
    {

        public AssemblyModel TankData;
        public BasicCommandModel commandData;
        private CommandBasicService commandService;

        public MainWindowViewModel()
        {
            TankData = new AssemblyModel();
            commandData = new BasicCommandModel();
            TankData.CreateSampleAssembly();
            //commandData.CreateSampleCommandModel();

        }

        public void CreateCommandService(Object selModel)
        {
            commandService = new CommandBasicService(commandData.commandList, TankData, selModel);
        }

        public LogicBuilder GetLogicBuilder()
        {
            commandService.SetCommandData(commandData.commandList);
            commandService.ExecuteCommand();
            LogicBuilder logicB = new LogicBuilder(commandService.commandEntities);

            return logicB;
        }
    }
}
