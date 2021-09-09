using StrengthCalLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrengthCalLib.CalMatchingModels
{
    public class AnchorModel : Notifier
    {
        public string Bolt { get; set; }
        public string Nut { get; set; }

        public AnchorModel()
        {
            Bolt = "";
            Nut = "";
        }
    }
}
