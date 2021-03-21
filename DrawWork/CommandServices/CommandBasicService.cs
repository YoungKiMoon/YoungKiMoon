using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using devDept.Eyeshot;
using devDept.Graphics;
using devDept.Eyeshot.Entities;


using DrawWork.CommandModels;
using DrawWork.DrawModels;
using DrawWork.DrawServices;
using DrawWork.DrawAutomationService;

using AssemblyLib.AssemblyModels;


namespace DrawWork.CommandServices
{
    public class CommandBasicService
    {
        public AssemblyModel assemblyData;
        public BasicCommandModel commandData;

        public TranslateDataService commandTranslate;
        public DrawObjectService drawObject;


        public List<Entity> commandEntities;

        public List<Entity[]> commandDimensionEntities;
        public List<Entity[]> commandNozzleEntities;

        #region CONSTRUCTOR
        public CommandBasicService()
        {
            assemblyData = new AssemblyModel();
            commandData = new BasicCommandModel();
            commandTranslate = new TranslateDataService();
            drawObject = new DrawObjectService();
            commandEntities = new List<Entity>();

            commandDimensionEntities = new List<Entity[]>();
            commandNozzleEntities = new List<Entity[]>();
        }

        public CommandBasicService(List<CommandLineModel> selCommandList,AssemblyModel selAssembly)
        {
            assemblyData = new AssemblyModel();
            commandData = new BasicCommandModel();
            SetCommandData(selCommandList);
            SetAssemblyData(selAssembly);
            commandTranslate = new TranslateDataService(selAssembly);
            drawObject = new DrawObjectService();
            commandEntities = new List<Entity>();

            commandDimensionEntities = new List<Entity[]>();
            commandNozzleEntities = new List<Entity[]>();

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
            commandTranslate.TranslateUsing(commandData.commandListTrans);
            commandData.commandListTransFunciton = commandTranslate.TranslateCommandFunction(commandData.commandListTrans);
            commandTranslate.TranslateModelData(commandData.commandListTransFunciton);

            CDPoint refPoint = commandData.drawPoint.referencePoint;
            CDPoint curPoint = commandData.drawPoint.currentPoint;

            // Create Entity
            foreach (string[] eachCmd in commandData.commandListTransFunciton)
            {
                if (eachCmd == null)
                    continue;

                int dmRepeatCount = commandTranslate.DrawMethod_Repeat(eachCmd);
                dmRepeatCount++;
                for (int dmCount = 1; dmCount <= dmRepeatCount; dmCount++)
                {
                    if (eachCmd != null)
                    {
                        CommandFunctionModel eachFunction = null;
                        DrawObjectLogic(eachCmd,ref refPoint, ref curPoint,out eachFunction);
                        if(eachFunction!=null)
                            commandData.commandListFunction.Add(eachFunction);
                    }
                }
            }



            // Adjust : Dimension
            AutomationService();

            // Sum of Entities
            SumOfEntities();



            commandData.drawPoint.referencePoint = refPoint;
            commandData.drawPoint.currentPoint = curPoint;
        }
        #endregion

        private void AutomationService()
        {
            AutomationDimensionService autoDimService = new AutomationDimensionService();
            
        }

        #region Sum Entities
        private void SumOfEntities()
        {
            // Dimension
            foreach (Entity[] eachEntityArray in commandDimensionEntities)
            {
                commandEntities.AddRange(eachEntityArray);
            }

            // Nozzle
            foreach (Entity[] eachEntityArray in commandNozzleEntities)
            {
                commandEntities.AddRange(eachEntityArray);
            }
        }
        #endregion


        #region DrawLogic
        public void DrawObjectLogic(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint, out CommandFunctionModel selCmdFunction)
        {


            CommandFunctionModel newCmdFunction = new CommandFunctionModel();
            List<CommandPropertiyModel> newCmdProperty = new List<CommandPropertiyModel>();
            string cmdObject = eachCmd[0].ToLower();

            switch (cmdObject)
            {

                case "refpoint":
                    drawObject.DoRefPoint(eachCmd, ref refPoint, ref curPoint);
                    goto case "allways";

                case "point":
                    drawObject.DoPoint(eachCmd, ref refPoint, ref curPoint);
                    goto case "allways";

                case "line":
                    commandEntities.Add(drawObject.DoLine(eachCmd, ref refPoint, ref curPoint));
                    goto case "allways";

                case "linedgree":
                    commandEntities.Add(drawObject.DoLineDgree(eachCmd, ref refPoint, ref curPoint));
                    goto case "allways";

                case "arc":
                    commandEntities.Add(drawObject.DoArc(eachCmd, ref refPoint, ref curPoint));
                    goto case "allways";

                case "text":
                    commandEntities.Add(drawObject.DoText(eachCmd, ref refPoint, ref curPoint));
                    goto case "allways";

                case "rec":
                    Entity[] newRec = drawObject.DoRectangle(eachCmd, ref refPoint, ref curPoint);
                    foreach (Entity eachEntity in newRec)
                        commandEntities.Add(eachEntity);
                    goto case "allways";

                case "rectangle":
                    Entity[] newRect = drawObject.DoRectangle(eachCmd, ref refPoint, ref curPoint);
                    foreach (Entity eachEntity in newRect)
                        commandEntities.Add(eachEntity);
                    goto case "allways";

                // Dimension
                case "dim":
                case "dimline":
                    Entity[] newDim = drawObject.DoDimension(eachCmd, ref refPoint, ref curPoint,out newCmdProperty);
                    commandDimensionEntities.Add(newDim);
                    //foreach (Entity eachEntity in newDim)
                    //    commandEntities.Add(eachEntity);
                    goto case "allways";

                // Block
                case "blocktopangle":
                case "topangle":
                    Entity[] newTopAngle = drawObject.DoBlockTopAngle(eachCmd, ref refPoint, ref curPoint, assemblyData.AngleInput);
                    foreach (Entity eachEntity in newTopAngle)
                        commandEntities.Add(eachEntity);
                    goto case "allways";

                // Nozzle
                case "nozzle":
                    Entity[] newNozzle = drawObject.DoNozzle(eachCmd, ref refPoint, ref curPoint, assemblyData);
                    commandNozzleEntities.Add(newNozzle);
                    //foreach (Entity eachEntity in newNozzle)
                    //    commandEntities.Add(eachEntity);
                    goto case "allways";

                // Contact Point
                case "cp":
                case "cpoint":
                case "contactpoint":
                    drawObject.DoContactPoint(eachCmd, ref refPoint, ref curPoint,assemblyData);
                    goto case "allways";


                // allways
                case "allways":
                    newCmdFunction.Name = cmdObject;
                    newCmdFunction.Properties = newCmdProperty;
                    selCmdFunction = newCmdFunction;
                    break;

                // default
                default:
                    selCmdFunction = null;
                    break;

            }

        }
        #endregion
    }
}
