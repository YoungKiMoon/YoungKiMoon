using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AssemblyLib.AssemblyModels;
using DrawWork.Commons;
using DrawWork.DrawServices;
using DrawWork.ValueServices;

namespace DrawWork.CommandServices
{
    public class TranslateDataOutputService
    {
        private ValueService valueService;
        public TranslateDataOutputService()
        {
            valueService = new ValueService();
        }


    }
}
