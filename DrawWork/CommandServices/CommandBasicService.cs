﻿using System;
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

        #region CONSTRUCTOR
        public CommandBasicService()
        {
            assemblyData = new AssemblyModel();
            commandData = new BasicCommandModel();
            commandTranslate = new TranslateDataService();
            drawObject = new DrawObjectService();
            commandEntities = new List<Entity>();
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
                        DrawObjectLogic(eachCmd,ref refPoint, ref curPoint);
                    }
                }
            }
            commandData.drawPoint.referencePoint = refPoint;
            commandData.drawPoint.currentPoint = curPoint;
        }
        #endregion

        #region DrawLogic
        public void DrawObjectLogic(string[] eachCmd, ref CDPoint refPoint, ref CDPoint curPoint)
        {



            string cmdObject = eachCmd[0].ToLower();

            switch (cmdObject)
            {

                case "refpoint":
                    drawObject.DoRefPoint(eachCmd, ref refPoint, ref curPoint);
                    break;

                case "point":
                    drawObject.DoPoint(eachCmd, ref refPoint, ref curPoint);
                    break;

                case "line":
                    commandEntities.Add(drawObject.DoLine(eachCmd, ref refPoint, ref curPoint));
                    break;

                case "linedgree":
                    commandEntities.Add(drawObject.DoLineDgree(eachCmd, ref refPoint, ref curPoint));
                    break;

                case "arc":
                    commandEntities.Add(drawObject.DoArc(eachCmd, ref refPoint, ref curPoint));
                    break;

                case "text":
                    commandEntities.Add(drawObject.DoText(eachCmd, ref refPoint, ref curPoint));
                    break;

                case "rec":
                    Entity[] newRec = drawObject.DoRectangle(eachCmd, ref refPoint, ref curPoint);
                    foreach (Entity eachEntity in newRec)
                        commandEntities.Add(eachEntity);
                    break;

                case "rectangle":
                    Entity[] newRect = drawObject.DoRectangle(eachCmd, ref refPoint, ref curPoint);
                    foreach (Entity eachEntity in newRect)
                        commandEntities.Add(eachEntity);
                    break;

                // Dimension
                case "dim":
                case "dimline":
                    Entity[] newDim = drawObject.DoDimension(eachCmd, ref refPoint, ref curPoint);
                    foreach (Entity eachEntity in newDim)
                        commandEntities.Add(eachEntity);
                    break;

                // Block
                case "blocktopangle":
                case "topangle":
                    Entity[] newTopAngle = drawObject.DoBlockTopAngle(eachCmd, ref refPoint, ref curPoint, assemblyData.AngleInput);
                    foreach (Entity eachEntity in newTopAngle)
                        commandEntities.Add(eachEntity);
                    break;

                // Nozzle
                case "nozzle":
                    Entity[] newNozzle = drawObject.DoNozzle(eachCmd, ref refPoint, ref curPoint, assemblyData);
                    foreach (Entity eachEntity in newNozzle)
                        commandEntities.Add(eachEntity);
                    break;

                // Contact Point
                case "cp":
                case "cpoint":
                case "contactpoint":
                    drawObject.DoContactPoint(eachCmd, ref refPoint, ref curPoint,assemblyData);
                    break;


            }

        }
        #endregion
    }
}
