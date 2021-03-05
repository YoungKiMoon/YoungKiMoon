using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


using DrawWork.CommandModels;
using DrawWork.ValueServices;
using AssemblyLib.AssemblyModels;

namespace DrawWork.CommandServices
{
    public class TranslateDataService
    {
        private List<string> usingData;
        public AssemblyModel assemblyData;

        private ValueService valueService;
        private TranslateDataModelService translateModelService;



        #region CONSTRUCTOR
        private void ConstructorService()
        {
            assemblyData = new AssemblyModel();
            usingData = new List<string>();

            valueService = new ValueService();
            translateModelService = new TranslateDataModelService();
        }
        public TranslateDataService()
        {
            ConstructorService();
        }
        public TranslateDataService(AssemblyModel selAssembly)
        {
            ConstructorService();
            SetAssemblyData(selAssembly);
        }
        #endregion

        #region AssemblyData
        public void SetAssemblyData(AssemblyModel selAssembly)
        {
            assemblyData = selAssembly;
            translateModelService.SetAssemblyData(selAssembly);
        }
        #endregion

        #region Translate Command
        public List<string[]> TranslateCommand(List<CommandLineModel> selCmd)
        {
            char[] spaceChar = new char[] { ' ' };
            List<string[]> resultCmd = new List<string[]>();
            foreach (CommandLineModel eachCmd in selCmd)
            {
                string eachCmdNew = eachCmd.CommandText.Replace("\t", " ");
                eachCmdNew = eachCmdNew.TrimStart();
                if (eachCmdNew != "")
                {
                    string[] cmdArray = eachCmdNew.Split(spaceChar, StringSplitOptions.RemoveEmptyEntries);
                    resultCmd.Add(cmdArray);
                }
                else
                {
                    resultCmd.Add(null);
                }
            }

            return resultCmd;
        }
        #endregion

        #region Translate Using
        public void TranslateUsing(List<string[]> selCmd)
        {
            // Using
            SetUsingData(selCmd);
        }
        #endregion

        #region Translate Command Function
        public List<string[]> TranslateCommandFunction(List<string[]> selCmd)
        {
            List<string[]> newCommand = new List<string[]>();

            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 0;

            bool forStart = false;
            bool forEnd = false;
            int forStartIndex = 0;
            int forEndIndex = 0;

            int startValue = 0;
            int endValue = 0;
            string countStr = "";

            // Function : for
            for (int i = 0; i < selCmd.Count; i++)
            {
                // Find
                if (selCmd[i] == null)
                    continue;

                string[] eachCmd = new string[selCmd[i].Length];
                selCmd[i].CopyTo(eachCmd,0);

                if (eachCmd != null)
                {
                    if (eachCmd.Length > 0)
                    {
                        if (eachCmd[refIndex] == "for")
                        {
                            forStart = true;
                            forStartIndex = i;

                            if (eachCmd.Length > 3)
                            {
                                if (eachCmd[1].Contains("="))
                                {
                                    string[] initializerStr= eachCmd[1].Split(new char[] { '=' });
                                    countStr = initializerStr[0];
                                    // StartCount
                                    if (valueService.CheckIntValue(initializerStr[1]))
                                    {
                                        startValue= valueService.GetIntValue(initializerStr[1]);
                                    }
                                    else
                                    {
                                        string tempStartValue = GetModelDataCal(initializerStr[1]);
                                        startValue = valueService.GetIntValue(tempStartValue);
                                    }


                                }
                                // EndCount
                                if (valueService.CheckIntValue(eachCmd[3]))
                                {
                                    endValue = valueService.GetIntValue(eachCmd[3]);
                                }
                                else
                                {
                                    string tempEndValue = GetModelDataCal(eachCmd[3]);
                                    endValue = valueService.GetIntValue(tempEndValue);
                                }
;
                            }
                        }
                        else if (eachCmd[refIndex] == "next")
                        {
                            forEnd = true;
                            forEndIndex = i;
                        }
                    }
                }

                // Default
                if (!forStart)
                    newCommand.Add(eachCmd);

                // Find For
                if (forStart && forEnd)
                {
                    for(int forCount = startValue; forCount <= endValue; forCount++)
                    {
                        for (int j = forStartIndex + 1; j < forEndIndex; j++)
                        {
                            string[] eachForCmd = new string[selCmd[j].Length];
                            selCmd[j].CopyTo(eachForCmd, 0);
                            if (eachForCmd != null)
                            {
                                for (int k = 0; k < eachForCmd.Length; k++)
                                {
                                    if (eachForCmd[k].Contains(","))
                                    {
                                        string[] eachCommaArray = eachForCmd[k].Split(new char[] { ',' });
                                        string eachCommaStr = "";
                                        foreach(string eachComma in eachCommaArray)
                                        {
                                            if (eachCommaStr != "")
                                                eachCommaStr += ",";
                                            eachCommaStr += GetForCommandValue(eachComma, countStr, forCount.ToString());
                                        }
                                        eachForCmd[k] = eachCommaStr;
                                    }
                                    else
                                    {
                                        eachForCmd[k] = GetForCommandValue(eachForCmd[k], countStr, forCount.ToString());
                                    }
                                    
                                }
                            }
                            newCommand.Add(eachForCmd);
                        }
                    }

                    forStart = false;
                    forEnd = false;
                    forStartIndex = 0;
                    forEndIndex = 0;
                }
            }

            return newCommand;
        }
        private string GetForCommandValue(string selCmd, string selCountStr, string selforCount)
        {
            string returnValue = "";

            string eachCalStr = "";
            string calStr = "";

            foreach (char ch in selCmd)
            {
                calStr = getCalculationCharacter(ch);
                if (calStr != "")
                {
                    returnValue += GetForCommandIndexValue(eachCalStr, selCountStr, selforCount);
                    returnValue += ch;
                    calStr = "";
                    eachCalStr = "";
                }
                else
                {
                    eachCalStr += ch;
                }
            }
            if(eachCalStr!="")
                returnValue += GetForCommandIndexValue(eachCalStr, selCountStr, selforCount);

            if (returnValue == "")
                returnValue = selCmd;

            return returnValue;
        }
        private string GetForCommandIndexValue(string selCmd, string selCountStr, string selforCount)
        {
            string returnValue = "";

            if (selCmd.Contains("["))
            {
                string[] eachForVariableArray = selCmd.Split(new char[] { '[' });

                foreach (string eachVariable in eachForVariableArray)
                {
                    if (eachVariable.Contains("]"))
                    {
                        string[] eachForVariableArrayAfter = eachVariable.Split(new char[] { ']' });
                        if (eachForVariableArrayAfter[0] == selCountStr)
                        {
                            eachForVariableArrayAfter[0] = selforCount;
                            eachForVariableArray[1] = string.Join("]", eachForVariableArrayAfter);
                        }
                        returnValue += "[" + eachForVariableArrayAfter[0] +  "]" + eachForVariableArrayAfter[1];
                    }
                    else
                    {
                        returnValue += eachVariable;
                    }


                }

            }
            else
            {
                if (selCmd == selCountStr)
                {
                    returnValue = selforCount;
                }
            }

            if (returnValue == "")
                returnValue = selCmd;

            return returnValue;
        }
        #endregion

        #region Translate Model
        public void TranslateModelData(List<string[]> selCmd)
        {

            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 2;

            for (int i = 0; i < selCmd.Count; i++)
            {
                string[] eachCmd = selCmd[i];
                if (eachCmd != null)
                {
                    for (int j = refIndex; j < eachCmd.Length; j += 2)
                    {
                        eachCmd[j] = GetModelDataArray(eachCmd[j]);
                    }
                    selCmd[i] = eachCmd;
                }

            }

        }

        private void SetUsingData(List<string[]> selCmd)
        {
            foreach (string[] eachCmd in selCmd)
            {
                if (eachCmd != null)
                {
                    string cmdUsing = eachCmd[0].ToLower();
                    if (cmdUsing == "using")
                    {
                        if (eachCmd.Length > 1)
                        {
                            usingData.Add(eachCmd[1].ToLower());
                        }
                    }
                }
            }
        }
        private string GetModelDataArray(string selCmd)
        {
            string selArrayStr = "";
            if (selCmd.Contains(","))
            {
                string[] selArray = selCmd.Split(',');
                for (int i = 0; i < selArray.Length; i++)
                {
                    if (selArrayStr != "")
                        selArrayStr += ",";
                    selArrayStr += GetModelDataCal(selArray[i].Trim());

                }
            }
            else
            {
                selArrayStr = GetModelDataCal(selCmd);
            }

            return selArrayStr;
        }

        private string GetModelDataCal(string selCmd)
        {
            string calStr = "";
            string newStr = "";
            string newValue = "";
            bool calStart = false;

            foreach (char ch in selCmd)
            {
                calStr = getCalculationCharacter(ch);

                if (calStr != "")
                {
                    calStart = true;

                    newValue += GetModelData(newStr);
                    newValue += calStr;

                    newStr = "";
                    calStr = "";
                }
                else
                {
                    newStr += ch;
                }

            }

            newValue += GetModelData(newStr);
            return newValue;
            /*
            if (calStart)
            {
                double doubleValue = Evaluate(newValue);
                return doubleValue.ToString();
            }
            else
            {
                return newValue;
            }
            */
        }
        private string getCalculationCharacter(char ch)
        {
            string calStr = "";
            switch (ch)
            {
                case '+':
                    calStr = "+";
                    break;
                case '-':
                    calStr = "-";
                    break;
                case '*':
                    calStr = "*";
                    break;
                case '/':
                    calStr = "/";
                    break;
            }
            return calStr;
        }
        private string[] GetModelIndex(string selCmd)
        {

            if (!selCmd.Contains("["))
            {
                // DefaultValue : 1
                string[] selArray = new string[2] { selCmd, "1" }; 
                return selArray;
            }
            else
            {
                string selCmdNew = selCmd.Replace("]", "");
                return selCmdNew.Split('[');
            }

        }
        private int GetIndexValue(string selIndex)
        {
            int intValue = 1;
            if (!int.TryParse(selIndex, out intValue))
                intValue = 1;
            // Array Base : 0 
            return intValue - 1;
        }

        #region Translate Model
        private string GetModelData(string selCmd)
        {
            string result = "0";

            string relativeStr = "";
            if (selCmd.Contains("@"))
                relativeStr = "@";

            string selCmdNew = selCmd.Replace("@", "");

            // 숫자면 그대로 반환


            string[] selCmdArray = GetModelIndex(selCmdNew);

            string selCmdStr = selCmdArray[0].ToLower();
            int selCmdIndex = GetIndexValue(selCmdArray[1]);



            #region Translate Model Switch
            foreach (string eachUsing in usingData)
            {
                result = translateModelService.GetTranslateModelSwitch(eachUsing, selCmdStr, selCmdNew, selCmdIndex);
                if (result != "nothing")
                    break;
            }
            if (result == "nothing")
                result = selCmdNew;
            #endregion


            return relativeStr + result;
        }

        #endregion

        #endregion

        #region Translate Method
        public int DrawMethod_Repeat(string[] eachCmd)
        {
            // 0 : Object
            // 1 : Command
            // 2 : Data
            int refIndex = 1;

            int result = 0;


            for (int j = refIndex; j < eachCmd.Length; j += 2)
            {
                switch (eachCmd[j])
                {
                    case "rep":
                    case "repeat":
                        if (j + 1 <= eachCmd.Length)
                        {
                            double calDouble = valueService.Evaluate(eachCmd[j + 1]);
                            result = Convert.ToInt32(calDouble);
                        }
                        break;

                }

            }

            return result;

        }
        #endregion







	}
}
