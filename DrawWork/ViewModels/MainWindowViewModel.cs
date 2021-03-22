﻿using System;
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
            commandData.CreateSampleCommandModel();

            commandService = new CommandBasicService(commandData.commandList, TankData);
        }

        

        public LogicBuilder GetLogicBuilder(Object selModel)
        {

            commandService = new CommandBasicService(commandData.commandList, TankData);
            commandService.ExecuteCommand(selModel);
            LogicBuilder logicB = new LogicBuilder(commandService.commandEntities);

            return logicB;
        }
    }
}
