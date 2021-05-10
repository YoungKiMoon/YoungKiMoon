using DrawLogicLib.Commons;
using DrawLogicLib.FilesServices;
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
            }

            //return logicFileService.GetTextFileArray(selFileAddress);
            return logicFileService.ReadResourceFile(selFileAddress);
        }
        
        
    }
}
