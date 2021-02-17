using DrawWork.CommandModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DrawWork.CommandServices
{
    public class TranslateService
    {
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
    }
}
