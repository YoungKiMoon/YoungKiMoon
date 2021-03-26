using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AssemblyLib.AssemblyModels;


namespace DrawWork.CommandServices
{
    public class TranslateDataOutputService
    {
        public void CreateOutputData(AssemblyModel selAssembly)
        {
            // Auto create output data
            // output Data 자동으로 구성하기 : AC로 표시


            // Input list : first index
            int firstIndex = 0;


            #region Structure

            // Structure Column Rafter Output
            selAssembly.StructureColumnRafterOutput.Clear();
            string acRafterSize = selAssembly.StructureInput[firstIndex].RafterSize.Replace(" ","");
            foreach(StructureColumnRafterModel eachRafter in selAssembly.StructureColumnRafter)
            {
                if (eachRafter.Size.Replace(" ", "") == acRafterSize)
                    selAssembly.StructureColumnRafterOutput.Add(eachRafter);
            }
            //Structure 

            #endregion
        }
    }
}
