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
using DrawWork.ValueServices;
using DrawSettingLib.SettingServices;
using DrawWork.DrawSacleServices;
using DrawSettingLib.SettingModels;
using DrawSettingLib.Commons;
using devDept.Geometry;
using DrawWork.DrawDetailServices;

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

        public DrawEntityModel drawEntity { get; set; }


        public EntityList commandEntities { get; set; }

        public ValueService valueService;
        //public List<Entity[]> commandDimensionEntities;
        //public List<Entity[]> commandNozzleEntities;


        public PaperAreaService areaService;
        public DrawScaleService scaleService { get; set; }

        public DrawScaleModel scaleData { get; set; }

        public DrawAssyModel currentDrawAssy { get; set; }
        public DrawPointModel currentDrawPoint { get; set; }

        

        #region CONSTRUCTOR
        public CommandBasicService(List<CommandLineModel> selCommandList,AssemblyModel selAssembly,Object selModel)
        {
            CommandBasicDefault(selCommandList, selAssembly, selModel);
        }
        public CommandBasicService(AssemblyModel selAssembly, Object selModel)
        {
            CommandBasicDefault(null, selAssembly, selModel);
        }
        private void CommandBasicDefault(List<CommandLineModel> selCommandList, AssemblyModel selAssembly, Object selModel)
        {
            assemblyData = new AssemblyModel();
            commandData = new BasicCommandModel();

            valueService = new ValueService();

            SetCommandData(selCommandList);
            SetAssemblyData(selAssembly);
            SetModelData(selModel);

            commandTranslate = new TranslateDataService(selAssembly);


            drawObject = new DrawObjectService(selAssembly, singleModel);
            boundaryService = new DrawBoundaryService(selAssembly);


            drawEntity = new DrawEntityModel();
            commandEntities = new EntityList();

            scaleData = new DrawScaleModel();

            currentDrawAssy = new DrawAssyModel();
            currentDrawPoint = new DrawPointModel();

            scaleService = new DrawScaleService();

            areaService = new PaperAreaService();
        }
        #endregion

        #region CommandData
        public void SetCommandData(List<CommandLineModel> selCommandList)
        {
            if (selCommandList == null)
                return;
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





        #region Execute : Create Entity
        public void ExecuteCommand(string selType)
        {
            // Draw Entity Clear
            drawEntity = new DrawEntityModel();

            commandData.commandListTrans = commandTranslate.TranslateCommand(commandData.commandList);
            commandTranslate.TranslateUsing(commandData.commandListTrans);
            commandData.commandListTransFunciton = commandTranslate.TranslateCommandFunction(commandData.commandListTrans);
            commandTranslate.TranslateModelData(commandData.commandListTransFunciton);


            // Create ReferencePoint
            //DrawPointModel drawPoint= GetReferencePoint();
            //CDPoint refPoint = drawPoint.referencePoint;
            //CDPoint curPoint = drawPoint.currentPoint;





            // Create Boundary : Pending
            //boundaryService.CreateBoundary(drawPoint);


           

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
                        Console.WriteLine("CMD : " + string.Join("|",eachCmd));
                        CommandFunctionModel eachFunction = null;
                        DrawObjectLogic(eachCmd,out eachFunction);
                        if(eachFunction!=null)
                            commandData.commandListFunction.Add(eachFunction);
                    }
                }
            }



            // Adjust : Dimension
            AutomationDimensionService autoDimService = new AutomationDimensionService();

            List<Entity> tempRegenList = new List<Entity>();
            tempRegenList.AddRange(autoDimService.SetLineBreakFree(drawEntity));
            //tempRegenList.AddRange(autoDimService.SetLineBreak(singleModel as Model, drawEntity));
            tempRegenList.AddRange(drawEntity.blockList);

            commandEntities.AddRange(tempRegenList);



        }
        #endregion


        #region Scale Size
        public void SetScaleData(DrawAssyModel selDrawAssy,DrawPointModel selDrawPoint)
        {
            CDPoint refPoint = selDrawPoint.referencePoint;
            CDPoint curPoint = selDrawPoint.currentPoint;

            // Real Size : Default
            double tankID = 1000;
            double tankHeight = 1000;
            if (assemblyData.GeneralDesignData.Count > 0)
                tankID = valueService.GetDoubleValue(assemblyData.GeneralDesignData[0].SizeNominalID);


            PaperAreaModel selPaperAreaModel = areaService.GetPaperAreaModel(selDrawAssy.mainName, selDrawAssy.subName, SingletonData.PaperArea);
            switch (selDrawAssy.mainName)
            {
                case PAPERMAIN_TYPE.GA:
                    // GA : Size
                    CDPoint UppderPoint = drawObject.workingPointService.WorkingPoint(WORKINGPOINT_TYPE.PointCenterTopUp, ref refPoint, ref curPoint);
                    tankHeight = UppderPoint.Y - refPoint.Y;
                    SingletonData.GAArea.SetMainAssemblySize(refPoint.X, refPoint.Y, tankID, tankHeight);
                    selPaperAreaModel.ScaleValue = scaleService.GetGAScaleValue(selPaperAreaModel, SingletonData.GAArea);
                    break;

                case PAPERMAIN_TYPE.ORIENTATION:
                    // 500 * 0.4
                    double orientationDim = 170;
                    selPaperAreaModel.ScaleValue = scaleService.GetOrientationScaleValue(selPaperAreaModel.Size.X - orientationDim, selPaperAreaModel.Size.Y - orientationDim, tankID, tankID);
                    // Center
                    selPaperAreaModel.ModelLocation.X = refPoint.X;
                    selPaperAreaModel.ModelLocation.Y = refPoint.Y;
                    break;

                case PAPERMAIN_TYPE.DETAIL:
                    switch (selDrawAssy.subName)
                    {
                        case PAPERSUB_TYPE.HORIZONTALJOINT:
                            if (assemblyData.ShellOutput.Count > 0)
                            {
                                double plateFirstCourse = valueService.GetDoubleValue(assemblyData.ShellOutput[0].Thickness);
                                selPaperAreaModel.ScaleValue = scaleService.GetPlateHorizontalJointScale(plateFirstCourse);
                            }
                            break;

                        case PAPERSUB_TYPE.ONECOURSESHELLPLATE:
                            if (assemblyData.ShellOutput.Count > 0)
                            {
                                double oneCourseWidth = valueService.GetDoubleValue(assemblyData.ShellOutput[0].PlateWidth);
                                double plateWidth = valueService.GetDoubleValue(assemblyData.ShellInput[0].PlateWidth);
                                double plateMaxLength = valueService.GetDoubleValue(assemblyData.ShellInput[0].PlateMaxLength);

                                selPaperAreaModel.ScaleValue = scaleService.GetOneCourseDevelopmentScale(selPaperAreaModel, tankID, oneCourseWidth, plateWidth, plateMaxLength);
                            }
                            break;

                        case PAPERSUB_TYPE.RoofArrange:
                            //작업 필요 함
                            break;
                    }
                    break;
            }


            scaleData.Value = selPaperAreaModel.ScaleValue;
            currentDrawAssy.scaleValue = selPaperAreaModel.ScaleValue;
            // Paper


            // Scale
            //PaperAreaService paperAreaService = new PaperAreaService();
            //scaleData.Value= paperAreaService.UpdateScaleValue(selType,SingletonData.PaperArea, SingletonData.GAArea, tankID,refPoint.X,refPoint.Y);

            // Set Scale Data


        }
        #endregion


        #region Reference Point
        public void SetDrawPoint(DrawPointModel selDrawPoint)
        {
            commandData.drawPoint.referencePoint = selDrawPoint.referencePoint;
            commandData.drawPoint.currentPoint = selDrawPoint.currentPoint;

            currentDrawPoint = selDrawPoint;

            // ReferencePoint -> SingletonData
            SingletonData.RefPoint = (CDPoint)selDrawPoint.referencePoint.Clone();
        }
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
        public void DrawObjectLogic(string[] eachCmd,  out CommandFunctionModel selCmdFunction)
        {
            CDPoint refPoint = currentDrawPoint.referencePoint;
            CDPoint curPoint = currentDrawPoint.currentPoint; 

            CommandFunctionModel newCmdFunction = new CommandFunctionModel();
            List<CommandPropertiyModel> newCmdProperty = new List<CommandPropertiyModel>();
            string cmdObject = eachCmd[0];

            switch (cmdObject)
            {
                // Draw Assy Model
                case "draw":
                case "drawassembly":                    
                    currentDrawAssy = drawObject.GetDrawAssyModel(eachCmd);
                    goto case "allways";

                // RefPoint : Very Import
                case "refpoint":
                    DrawPointModel tempDrawPoint = drawObject.GetDrawPoint(eachCmd);
                    SetDrawPoint(tempDrawPoint);
                    currentDrawAssy.refPoint = new Point3D(tempDrawPoint.referencePoint.X,tempDrawPoint.referencePoint.Y);
                    // Create Scale Value
                    SetScaleData(currentDrawAssy, currentDrawPoint);
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
                    DrawEntityModel newDim = drawObject.DoDimension(eachCmd, ref refPoint, ref curPoint,out newCmdProperty,scaleData.Value);
                    drawEntity.AddDrawEntity(newDim);
                    goto case "allways";

                case "customdim":
                case "customdimline":
                    DrawEntityModel newCustomDim = drawObject.DoDimensionCustom(eachCmd, ref refPoint, ref curPoint,  scaleData.Value);
                    drawEntity.AddDrawEntity(newCustomDim);


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
                    drawEntity.AddDrawEntity(newNozzle);

                    goto case "allways";

                // Nozzle : Orientation
                case "nozzleorientation":
                    DrawEntityModel newNozzleOrientation = drawObject.DoNozzleOrientation(eachCmd, ref refPoint, ref curPoint, scaleData.Value);
                    drawEntity.AddDrawEntity(newNozzleOrientation);
                    goto case "allways";

                case "orientation":
                    Entity[] newOrientation = drawObject.DoBlockOrientation(eachCmd, ref refPoint, ref curPoint, scaleData.Value);
                    if (newOrientation != null)
                        drawEntity.outlineList.AddRange(newOrientation);
                    goto case "allways";

                // Leader : Scale
                case "leader":
                case "leaderline":
                    DrawEntityModel newLeader = drawObject.DoLeader(eachCmd, ref refPoint, ref curPoint, singleModel,scaleData.Value);
                    drawEntity.AddDrawEntity(newLeader);
                    goto case "allways";

                case "leaderlist":
                    DrawEntityModel newLeaderList = drawObject.DoLeaderList(eachCmd, ref refPoint, ref curPoint, singleModel, scaleData.Value);
                    drawEntity.AddDrawEntity(newLeaderList);

                    goto case "allways";

                case "drawdetail":
                    DrawEntityModel newLogicDetail = drawObject.DoBlockDetail(eachCmd, ref refPoint, ref curPoint, singleModel, scaleData.Value);
                    drawEntity.AddDrawEntity(newLogicDetail);
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
