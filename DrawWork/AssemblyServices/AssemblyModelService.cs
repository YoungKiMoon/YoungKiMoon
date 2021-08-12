using AssemblyLib.AssemblyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.AssemblyServices
{
    public class AssemblyModelService
    {
        private AssemblyModel assemblyData;

        public AssemblyModelService(AssemblyModel selAssembly)
        {
            assemblyData = selAssembly;
        }


        public AnchorChairModel GetAnchorChair(string selSize)
        {
            AnchorChairModel retrunValue = null;
            foreach (AnchorChairModel eachModel in assemblyData.AnchorChairList)
            {
                if (eachModel.Size == selSize)
                {
                    retrunValue = eachModel;
                    break;
                }
            }

            return retrunValue;
        }
    }
}
