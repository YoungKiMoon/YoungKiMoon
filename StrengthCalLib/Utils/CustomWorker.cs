using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StrengthCalLib.Utils
{
    public class CustomWorker
    {
        public BackgroundWorker bgWorker;

        public CustomWorker()
        {
            bgWorker = new BackgroundWorker();
        }

    }
}
