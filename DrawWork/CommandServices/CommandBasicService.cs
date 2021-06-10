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
using DrawWork.DrawAutomationServices;

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

        public DrawBoundaryService boundaryService;

        public object singleModel;

        public DrawEntityModel drawEntity;

        public DrawScaleModel scaleData;
        public List<Entity> commandEntities;

        //public List<Entity[]> commandDimensionEntities;
        //public List<Entity[]> commandNozzleEntities;

        #region CONSTRUCTOR
        public CommandBasicService(List<CommandLineModel> selCommandList,AssemblyModel selAssembly,Object selModel)
        {
            assemblyData = new AssemblyModel();
            commandData = new BasicCommandModel();

            SetCommandData(selCommandList);
            SetAssemblyData(selAssembly);
            SetModelData(selModel);

            commandTranslate = new TranslateDataService(selAssembly);
            

            drawObject = new DrawObjectService(selAssembly, singleModel);
            boundaryService = new DrawBoundaryService(selAssembly);


            drawEntity = new DrawEntityModel();
            commandEntities = new List<Entity>();

            scaleData = new DrawScaleModel();
        }
        public CommandBasicService(AssemblyModel selAssembly, Object selModel)
        {
            assemblyData = new AssemblyModel();
            commandData = new BasicCommandModel();

            //SetCommandData(selCommandList);
            SetAssemblyData(selAssembly);
            SetModelData(selModel);

            commandTranslate = new TranslateDataService(selAssembly);
            

            drawObject = new DrawObjectService(selAssembly, singleModel);
            boundaryService = new DrawBoundaryService(selAssembly);


            drawEntity = new DrawEntityModel();
            commandEntities = new List<Entity>();

            scaleData = new DrawScaleModel();
        }
        #endregion

        #region CommandData
        public void SetCommandData(List<CommandLineModel> selCommandList)
        {
            commandData.commandList = CommandTextToLower(selCommandList);

        }
        private List<CommandLineModel> CommandTextToLower(List<CommandLineModel> selCommandList)
        {
            List<CommandLineModel> newCommandList = new List<CommandLineModel>();
            foreach (CommandLineModel eachCommand in selCommandList)
            {
                newCommandList.Add(new CommandLineModel(eachCommand.CommandText.ToLower()));
            }

            return newCommandList;
        }
        public List<CommandLineModel> CommandTextToLower(string[] selCommandArray)
        {
            List<CommandLineModel> newCommandList = new List<CommandLineModel>();
            if (selCommandArray != null)
                foreach (string eachText in selCommandArray)
                    newCommandList.Add(new CommandLineModel(eachText.ToLower()));

            return newCommandList;
        }
        #endregion

        #region AssemblyData
        public void SetAssemblyData(AssemblyModel selAssembly)
        {
            assemblyData = selAssembly;
        }
        #endregion

        #region ModelData
        public void SetModelData(Object selModel)
        {
            singleModel = selModel;
        }

        #endregion

        #region ReferencePoint
        public void SetDrawPoint(DrawPointModel selDrawPoint)
        {
            commandData.drawPoint.referencePoint = selDrawPoint.referencePoint;
            commandData.drawPoint.currentPoint = selDrawPoint.currentPoint;
        }
        #endregion

        #region Scale
        public void SetScaleData(double selScaleValue)
        {
            scaleData.Value = selScaleValue;
        }
        #endregion

        #region Execute : Create Entity
        public void ExecuteCommand()
        {
            // Draw Entity Clear
            drawEntity = new DrawEntityModel();

            commandData.commandListTrans = commandTranslate.TranslateCommand(commandData.commandList);
            commandTranslate.TranslateUsing(commandData.commandListTrans);
            commandData.commandListTransFunciton = commandTranslate.TranslateCommandFunction(commandData.commandListTrans);
            commandTranslate.TranslateModelData(commandData.commandListTransFunciton);


            // Create ReferencePoint
            DrawPointModel drawPoint= GetReferencePoint();
            CDPoint refPoint = drawPoint.referencePoint;
            CDPoint curPoint = drawPoint.currentPoint;

            // ReferencePoint -> SingletonData
            SingletonData.RefPoint = (CDPoint)refPoint.Clone();


            SetDrawPoint(drawPoint);

            // Create Boundary
            boundaryService.CreateBoundary(drawPoint);

            // Create Entity
            foreach (string[] eachCmd in commandData.commandListTransFunciton)
            {
                if (eachCmd == null)
                    continue;


                Console.WriteLine(string.Join("",eachCmd));


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
            commandEntities.AddRange(autoDimService.SetLineBreak(singleModel as Model, drawEntity));
            
            commandEntities.AddRange(drawEntity.blockList);


        }
        #endregion


        #region Reference Point
        private DrawPointModel GetReferencePoint()
        {
            DrawPointModel drawPoint = new DrawPointModel();
            CDPoint refPoint = new CDPoint();
            CDPoint curPoint = new CDPoint();

            foreach (string[] eachCmd in commandData.commandListTransFunciton)
            {
                if (eachCmd == null)
                    continue;

                string cmdObject = eachCmd[0];
                switch (cmdObject)
                {
                    case "refpoint":
                        drawObject.DoRefPoint(eachCmd, ref refPoint, ref curPoint);
                        break;
                }                
            }

            drawPoint.referencePoint = refPoint;
            drawPoint.currentPoint = curPoint;

            return drawPoint;
        }
        
        #endregion


        #region DrawLogic
        public void DrawObjectLogic(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint, out CommandFunctionModel selCmdFunction)
        {


            CommandFunctionModel newCmdFunction = new CommandFunctionModel();
            List<CommandPropertiyModel> newCmdProperty = new List<CommandPropertiyModel>();
            string cmdObject = eachCmd[0];

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
                case "cp":
                case "cpoint":
                case "contactpoint":
                case "workingpoint":
                    drawObject.DoWorkingPoint(eachCmd, ref refPoint, ref curPoint);
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

                // Dimension : Scale
                case "dim":
                case "dimline":
                    Dictionary<string, List<Entity>> newDim = drawObject.DoDimension(eachCmd, ref refPoint, ref curPoint,out newCmdProperty,scaleData.Value);
                    drawEntity.dimlineList.AddRange(newDim[CommonGlobal.DimLine]);
                    drawEntity.dimTextList.AddRange(newDim[CommonGlobal.DimText]);
                    drawEntity.dimlineExtList.AddRange(newDim[CommonGlobal.DimLineExt]);
                    drawEntity.dimArrowList.AddRange(newDim[CommonGlobal.DimArrow]);
                    goto case "allways";

                case "customdim":
                case "customdimline":
                    Dictionary<string, List<Entity>> newCustomDim = drawObject.DoDimensionCustom(eachCmd, ref refPoint, ref curPoint,  scaleData.Value);
                    drawEntity.dimlineList.AddRange(newCustomDim[CommonGlobal.DimLine]);
                    drawEntity.dimTextList.AddRange(newCustomDim[CommonGlobal.DimText]);
                    drawEntity.dimlineExtList.AddRange(newCustomDim[CommonGlobal.DimLineExt]);
                    drawEntity.dimArrowList.AddRange(newCustomDim[CommonGlobal.DimArrow]);
                    break;

                case "extradim":
                    Dictionary<string, List<Entity>> newExtraDim = drawObject.DoDimensionExtra(eachCmd, ref refPoint, ref curPoint, scaleData.Value);
                    drawEntity.dimlineList.AddRange(newExtraDim[CommonGlobal.DimLine]);
                    drawEntity.dimTextList.AddRange(newExtraDim[CommonGlobal.DimText]);
                    break;

                // Import Block
                case "block":
                case "importblock":
                    BlockReference newImportBlock = drawObject.DoBlockImport(eachCmd, ref refPoint, ref curPoint);
                    if(newImportBlock !=null)
                        drawEntity.blockList.Add(newImportBlock);
                    goto case "allways";

                case "importcustomblock":
                    List<BlockReference> newImportBlockList = drawObject.DoBlockCustomImport(eachCmd, ref refPoint, ref curPoint);
                    if (newImportBlockList.Count>0)
                        drawEntity.blockList.AddRange(newImportBlockList);
                    goto case "allways";

                // Logic Block
                case "logicblock":
                    Entity[] newLogicBlock = drawObject.DoBlockLogic(eachCmd, ref refPoint, ref curPoint, scaleData.Value);
                    if (newLogicBlock != null)
                        drawEntity.outlineList.AddRange(newLogicBlock);
                    goto case "allways";

                // Nozzle
                case "nozzle":
                    DrawEntityModel newNozzle = drawObject.DoNozzle(eachCmd, ref refPoint, ref curPoint, scaleData.Value);
                    drawEntity.outlineList.AddRange(newNozzle.outlineList);
                    drawEntity.nozzlelineList.AddRange(newNozzle.nozzlelineList);
                    drawEntity.nozzleMarkList.AddRange(newNozzle.nozzleMarkList);
                    drawEntity.nozzleTextList.AddRange(newNozzle.nozzleTextList);
                    drawEntity.blockList.AddRange(newNozzle.blockList);
                    goto case "allways";

                // Nozzle : Orientation
                case "nozzleorientation":
                    DrawEntityModel newNozzleOrientation = drawObject.DoNozzleOrientation(eachCmd, ref refPoint, ref curPoint, scaleData.Value);
                    drawEntity.outlineList.AddRange(newNozzleOrientation.outlineList);
                    drawEntity.nozzlelineList.AddRange(newNozzleOrientation.nozzlelineList);
                    drawEntity.nozzleMarkList.AddRange(newNozzleOrientation.nozzleMarkList);
                    drawEntity.nozzleTextList.AddRange(newNozzleOrientation.nozzleTextList);
                    drawEntity.blockList.AddRange(newNozzleOrientation.blockList);
                    goto case "allways";

                case "orientation":
                    Entity[] newOrientation = drawObject.DoBlockOrientation(eachCmd, ref refPoint, ref curPoint, scaleData.Value);
                    if (newOrientation != null)
                        drawEntity.outlineList.AddRange(newOrientation);
                    goto case "allways";

                // Leader : Scale
                case "leader":
                case "leaderline":
                    Dictionary<string, List<Entity>> newLeader = drawObject.DoLeader(eachCmd, ref refPoint, ref curPoint, singleModel,scaleData.Value);
                    drawEntity.leaderlineList.AddRange(newLeader[CommonGlobal.LeaderLine]);
                    drawEntity.leaderTextList.AddRange(newLeader[CommonGlobal.LeaderText]);
                    drawEntity.leaderArrowList.AddRange(newLeader[CommonGlobal.LeaderArrow]);
                    goto case "allways";

                case "leaderlist":
                    Dictionary<string, List<Entity>> newLeaderList = drawObject.DoLeaderList(eachCmd, ref refPoint, ref curPoint, singleModel, scaleData.Value);
                    drawEntity.leaderlineList.AddRange(newLeaderList[CommonGlobal.LeaderLine]);
                    drawEntity.leaderTextList.AddRange(newLeaderList[CommonGlobal.LeaderText]);
                    drawEntity.leaderArrowList.AddRange(newLeaderList[CommonGlobal.LeaderArrow]);
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

            selCmdFunction = newCmdFunction;
        }
        #endregion
    }
}
