using DrawLogicLib.Commons;
using DrawLogicLib.FilesServices;
using DrawLogicLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawLogicLib.DrawLogicFileServices
{
    public class DrawLogicDBService
    {

        private TextFileService logicFileService;
        public DrawLogicDBService()
        {
            logicFileService = new TextFileService();
        }

        public string[] GetLogicFile(LogicFile_Type selFile)
        {
            //string selFileAddress = "/DrawLogicLib;component/DrawLogicFiles/";
            string selFileAddress = "";
            switch (selFile)
            {
                case LogicFile_Type.GA:
                    selFileAddress += "GA.txt";
                    break;
                case LogicFile_Type.NozzleOrientation:
                    selFileAddress += "Orientation.txt";
                    break;
            }

            //return logicFileService.GetTextFileArray(selFileAddress);
            return logicFileService.ReadResourceFile(selFileAddress);
        }

        public DrawLogicModel GetLogicCommand(LogicFile_Type selFile)
        {
            DrawLogicModel newLogic = new DrawLogicModel();
            string[] commandArray = GetLogicFile(selFile);
            if(commandArray != null)
            {
                string usingName = "using";
                string refPointName = "refpoint";
                string regionStartName = "#region";
                string regionEndName = "#endregion";

                for(int i=0;i<commandArray.Length;i++)
                {
                    string eachCommand = commandArray[i];
                    if (eachCommand.StartsWith(usingName))
                    {
                        newLogic.UsingList.Add(eachCommand);
                    }
                    else if (eachCommand.StartsWith(refPointName))
                    {
                        newLogic.ReferencePointList.Add(eachCommand);
                    }
                    else if (eachCommand.StartsWith(regionStartName))
                    {
                        DrawCommandModel newRegion = new DrawCommandModel();
                        newRegion.Name = eachCommand.Replace(regionStartName, "").Trim();
                        for(int j = i+1; j < commandArray.Length; j++)
                        {
                            string eachRegion = commandArray[j];
                            if (eachRegion.StartsWith(regionEndName))
                            {
                                i = j;
                                break;
                            }
                            else
                            {
                                newRegion.Command.Add(eachRegion);
                            }
                        }
                        newLogic.CommandList.Add(newRegion);
                    }

                }
            }

            return newLogic;
        }
        
        
    }
}
