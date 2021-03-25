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
using DrawWork.Commons;

namespace DrawWork.CommandServices
{
    public class CommandBasicService
    {
        public AssemblyModel assemblyData;
        public BasicCommandModel commandData;

        public TranslateDataService commandTranslate;
        public DrawObjectService drawObject;

        public object singleModel;

        public DrawEntityModel drawEntity;

        public List<Entity> commandEntities;

        //public List<Entity[]> commandDimensionEntities;
        //public List<Entity[]> commandNozzleEntities;

        #region CONSTRUCTOR
        public CommandBasicService(List<CommandLineModel> selCommandList,AssemblyModel selAssembly)
        {
            assemblyData = new AssemblyModel();
            commandData = new BasicCommandModel();
            SetCommandData(selCommandList);
            SetAssemblyData(selAssembly);
            commandTranslate = new TranslateDataService(selAssembly);
            drawObject = new DrawObjectService(selAssembly);

            singleModel = null;

            drawEntity = new DrawEntityModel();
            commandEntities = new List<Entity>();

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
        public void ExecuteCommand(Object selModel)
        {
            singleModel = selModel;

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
            AutomationDimensionService autoDimService = new AutomationDimensionService();
            commandEntities.AddRange(autoDimService.SetLineBreak(selModel as Model, drawEntity));

            commandData.drawPoint.referencePoint = refPoint;
            commandData.drawPoint.currentPoint = curPoint;
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
                // Point
                case "refpoint":
                    drawObject.DoRefPoint(eachCmd, ref refPoint, ref curPoint);
                    goto case "allways";

                case "point":
                    drawObject.DoPoint(eachCmd, ref refPoint, ref curPoint);
                    goto case "allways";

                // Contact Point
                case "wp":
                case "cp":
                case "cpoint":
                case "contactpoint":
                    drawObject.DoContactPoint(eachCmd, ref refPoint, ref curPoint);
                    goto case "allways";

                // Object
                case "line":
                    drawEntity.outlineList.Add(drawObject.DoLine(eachCmd, ref refPoint, ref curPoint));
                    goto case "allways";

                case "linedgree":
                    drawEntity.outlineList.Add(drawObject.DoLineDgree(eachCmd, ref refPoint, ref curPoint));
                    goto case "allways";

                case "arc":
                    drawEntity.outlineList.Add(drawObject.DoArc(eachCmd, ref refPoint, ref curPoint));
                    goto case "allways";

                case "text":
                    drawEntity.outlineList.Add(drawObject.DoText(eachCmd, ref refPoint, ref curPoint));
                    goto case "allways";

                case "rec":
                    drawEntity.outlineList.AddRange(drawObject.DoRectangle(eachCmd, ref refPoint, ref curPoint));
                    goto case "allways";

                case "rectangle":
                    drawEntity.outlineList.AddRange(drawObject.DoRectangle(eachCmd, ref refPoint, ref curPoint));
                    goto case "allways";

                // Dimension
                case "dim":
                case "dimline":
                    Dictionary<string, List<Entity>> newDim = drawObject.DoDimension(eachCmd, ref refPoint, ref curPoint,out newCmdProperty);
                    drawEntity.dimlineList.AddRange(newDim[CommonGlobal.DimLine]);
                    drawEntity.dimTextList.AddRange(newDim[CommonGlobal.DimText]);
                    drawEntity.dimlineExtList.AddRange(newDim[CommonGlobal.DimLineExt]);
                    drawEntity.dimArrowList.AddRange(newDim[CommonGlobal.DimArrow]);

                    goto case "allways";

                // Block
                case "blocktopangle":
                case "topangle":
                    drawEntity.outlineList.AddRange(drawObject.DoBlockTopAngle(eachCmd, ref refPoint, ref curPoint));
                    goto case "allways";

                // Nozzle
                case "nozzle":
                    Dictionary<string, List<Entity>> newNozzle = drawObject.DoNozzle(eachCmd, ref refPoint, ref curPoint);
                    drawEntity.outlineList.AddRange(newNozzle[CommonGlobal.OutLine]);
                    drawEntity.nozzlelineList.AddRange(newNozzle[CommonGlobal.NozzleLine]);
                    drawEntity.nozzleMarkList.AddRange(newNozzle[CommonGlobal.NozzleMark]);
                    drawEntity.nozzleTextList.AddRange(newNozzle[CommonGlobal.NozzleText]);
                    goto case "allways";

                // Leader
                case "leader":
                case "leaderline":
                    Dictionary<string, List<Entity>> newLeader = drawObject.DoLeader(eachCmd, ref refPoint, ref curPoint, singleModel);
                    drawEntity.leaderlineList.AddRange(newLeader[CommonGlobal.LeaderLine]);
                    drawEntity.leaderTextList.AddRange(newLeader[CommonGlobal.LeaderText]);
                    drawEntity.leaderArrowList.AddRange(newLeader[CommonGlobal.LeaderArrow]);
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
