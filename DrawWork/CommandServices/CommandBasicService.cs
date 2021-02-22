using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DrawWork.AssemblyModels;
using DrawWork.CommandModels;
using DrawWork.DrawModels;
using DrawWork.DrawServices;

namespace DrawWork.CommandServices
{
    public class CommandBasicService
    {
        public AssemblyModel assemblyData;
        public BasicCommandModel commandData;

        public TranslateDataService commandTranslate;
        public DrawObjectLogicService drawLogicService;

        #region CONSTRUCTOR
        public CommandBasicService()
        {
            assemblyData = new AssemblyModel();
            commandData = new BasicCommandModel();
            commandTranslate = new TranslateDataService();
            drawLogicService = new DrawObjectLogicService();
        }

        public CommandBasicService(List<CommandLineModel> selCommandList,AssemblyModel selAssembly)
        {
            assemblyData = new AssemblyModel();
            commandData = new BasicCommandModel();
            SetCommandData(selCommandList);
            SetAssemblyData(selAssembly);
            commandTranslate = new TranslateDataService(selAssembly);
            drawLogicService = new DrawObjectLogicService();

        }
        #endregion

        #region CommandData
        public void SetCommandData(List<CommandLineModel> selCommandList)
        {
            commandData.commandList = selCommandList;
        }
        #endregion

        #region AssemblyData
        public void SetAssemblyData(AssemblyModel selAssembly)
        {
            assemblyData = selAssembly;
        }
        #endregion

        #region Execute
        public void ExecuteCommand()
        {
            commandData.commandListTrans = commandTranslate.TranslateCommand(commandData.commandList);

            commandTranslate.TranslateModelData(commandData.commandListTrans);

            CDPoint refPoint = commandData.drawPoint.referencePoint;
            CDPoint curPoint = commandData.drawPoint.currentPoint;

            foreach (string[] eachCmd in commandData.commandListTrans)
            {
                int dmRepeatCount = commandTranslate.DrawMethod_Repeat(eachCmd);
                dmRepeatCount++;
                for (int dmCount = 1; dmCount <= dmRepeatCount; dmCount++)
                {
                    if (eachCmd != null)
                    {
                        drawLogicService.DrawObjectLogic(eachCmd,ref refPoint, ref curPoint);
                    }
                }
            }
            commandData.drawPoint.referencePoint = refPoint;
            commandData.drawPoint.currentPoint = curPoint;
        }
        #endregion
    }
}
