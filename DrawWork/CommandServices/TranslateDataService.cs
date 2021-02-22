using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using DrawWork.AssemblyModels;
using DrawWork.CommandModels;
using DrawWork.ValueServices;

namespace DrawWork.CommandServices
{
    public class TranslateDataService
    {
        public AssemblyModel assemblyData;
        private ValueService valueService;

        #region CONSTRUCTOR
        public TranslateDataService()
        {
            assemblyData = new AssemblyModel();
            valueService = new ValueService();
        }
        public TranslateDataService(AssemblyModel selAssembly)
        {
            assemblyData = new AssemblyModel();
            valueService = new ValueService();
            SetAssemblyData(selAssembly);
        }
        #endregion

        #region AssemblyData
        public void SetAssemblyData(AssemblyModel selAssembly)
        {
            assemblyData = selAssembly;
        }
        #endregion

        public List<string[]> TranslateCommand(List<CommandLineModel> selCmd)
        {
            Regex regex = new Regex(" ");
            List<string[]> resultCmd = new List<string[]>();
            foreach (CommandLineModel eachCmd in selCmd)
            {

                string[] cmdArray = regex.Split(eachCmd.CommandText);
                resultCmd.Add(cmdArray);
            }

            return resultCmd;
        }


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

        private string GetModelData(string selCmd)
        {
            string result = "0";

            string relativeStr = "";
            if (selCmd.Contains("@"))
                relativeStr = "@";

            string selCmdNew = selCmd.Replace("@", "");

            string[] selCmdArray = GetModelIndex(selCmdNew);

            string selCmdStr = selCmdArray[0].ToLower();
            int selCmdIndex = GetIndexValue(selCmdArray[1]);


            switch (selCmdStr)
            {
                case "id":
                    result = assemblyData.ShellInput[selCmdIndex].ID;
                    break;
                case "height":
                    result = assemblyData.ShellInput[selCmdIndex].Height;
                    break;
                case "plwidth":
                    result = assemblyData.ShellInput[selCmdIndex].PLHeight;
                    break;
                case "plheight":
                    result = assemblyData.ShellInput[selCmdIndex].PLWidth;
                    break;

                case "course":
                    result = assemblyData.ShellOutput[selCmdIndex].Course;
                    break;
                case "thickness":
                    result = assemblyData.ShellOutput[selCmdIndex].Thickness;
                    break;
                case "startpoint":
                    result = assemblyData.ShellOutput[selCmdIndex].StartPoint;
                    break;
                case "oneplheight":
                    result = assemblyData.ShellOutput[selCmdIndex].OnePLHeight;
                    break;
                case "oneplwidth":
                    result = assemblyData.ShellOutput[selCmdIndex].OnePLWidth;
                    break;
                case "count":
                    result = assemblyData.ShellOutput[selCmdIndex].Count;
                    break;

                default:
                    result = selCmdNew;
                    break;

            }

            return relativeStr + result;
        }

        private string[] GetModelIndex(string selCmd)
        {

            if (!selCmd.Contains("["))
            {
                string[] selArray = new string[2] { selCmd, "0" };
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
            return intValue - 1;
        }

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
    }
}
