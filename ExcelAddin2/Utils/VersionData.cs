using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ExcelAddIn.Utils
{
    public class VersionData
    {
        public VersionData()
        {

        }


        public static string GetVersion()
        {
            string strVersionText = Assembly.GetExecutingAssembly().FullName.Split(',')[1].Trim().Split('=')[1]; ;
            ; string[] versionArray = strVersionText.Split('.');

            return "[v" + versionArray[0] + "." + Convert.ToInt32(versionArray[1]).ToString("000") + "]";
        }

    }
}
