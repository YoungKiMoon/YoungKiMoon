using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawGridServices
{
    public class DrawPaperGridService
    {
        public List<DrawPaperGridModel> PaperList { get; set; }
        public DrawPaperGridService()
        {
            PaperList = new List<DrawPaperGridModel>();
        }

    }
}
