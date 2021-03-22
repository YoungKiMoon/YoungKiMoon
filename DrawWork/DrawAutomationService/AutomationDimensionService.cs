using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.DrawAutomationService
{
    public class AutomationDimensionService
    {
        public AutomationDimensionService()
        {

        }

        public bool GetDimensionBreak(string targetLine, string referenceLine)
        {
            bool returnValue = false;
            if(targetLine=="outline")
            {
                switch (referenceLine)
                {
                    case "outline":
                        returnValue = false;
                        break;
                    case "centerline":
                        returnValue = false;
                        break;
                    case "dimline":
                        returnValue = false;
                        break;
                    case "dimtext":
                        returnValue = true;
                        break;
                    case "dimlineext":
                        returnValue = false;
                        break;
                    case "leaderline":
                        returnValue = false;
                        break;
                    case "leadertext":
                        returnValue = true;
                        break;
                    case "nozzleline":
                        returnValue = false;
                        break;
                    case "nozzlemark":
                        returnValue = false;
                        break;
                };
            }
            else if(targetLine == "centerline")
            {
                switch (referenceLine)
                {
                    case "outline":
                        returnValue = false;
                        break;
                    case "centerline":
                        returnValue = false;
                        break;
                    case "dimline":
                        returnValue = false;
                        break;
                    case "dimtext":
                        returnValue = true;
                        break;
                    case "dimlineext":
                        returnValue = false;
                        break;
                    case "leaderline":
                        returnValue = false;
                        break;
                    case "leadertext":
                        returnValue = true;
                        break;
                    case "nozzleline":
                        returnValue = false;
                        break;
                    case "nozzlemark":
                        returnValue = false;
                        break;
                };
            }
            else if (targetLine == "dimline")
            {
                switch (referenceLine)
                {
                    case "outline":
                        returnValue = false;
                        break;
                    case "centerline":
                        returnValue = false;
                        break;
                    case "dimline":
                        returnValue = false;
                        break;
                    case "dimtext":
                        returnValue = false;
                        break;
                    case "dimlineext":
                        returnValue = false;
                        break;
                    case "leaderline":
                        returnValue = false;
                        break;
                    case "leadertext":
                        returnValue = false;
                        break;
                    case "nozzleline":
                        returnValue = false;
                        break;
                    case "nozzlemark":
                        returnValue = false;
                        break;
                };
            }
            else if (targetLine == "dimtext")
            {
                switch (referenceLine)
                {
                    case "outline":
                        returnValue = false;
                        break;
                    case "centerline":
                        returnValue = false;
                        break;
                    case "dimline":
                        returnValue = false;
                        break;
                    case "dimtext":
                        returnValue = false;
                        break;
                    case "dimlineext":
                        returnValue = false;
                        break;
                    case "leaderline":
                        returnValue = false;
                        break;
                    case "leadertext":
                        returnValue = false;
                        break;
                    case "nozzleline":
                        returnValue = false;
                        break;
                    case "nozzlemark":
                        returnValue = false;
                        break;
                };
            }
            else if (targetLine == "dimlineext")
            {
                switch (referenceLine)
                {
                    case "outline":
                        returnValue = true;
                        break;
                    case "centerline":
                        returnValue = false;
                        break;
                    case "dimline":
                        returnValue = true;
                        break;
                    case "dimtext":
                        returnValue = true;
                        break;
                    case "dimlineext":
                        returnValue = false;
                        break;
                    case "leaderline":
                        returnValue = true;
                        break;
                    case "leadertext":
                        returnValue = true;
                        break;
                    case "nozzleline":
                        returnValue = true;
                        break;
                    case "nozzlemark":
                        returnValue = true;
                        break;
                };
            }
            else if (targetLine == "leaderline")
            {
                switch (referenceLine)
                {
                    case "outline":
                        returnValue = false;
                        break;
                    case "centerline":
                        returnValue = false;
                        break;
                    case "dimline":
                        returnValue = true;
                        break;
                    case "dimtext":
                        returnValue = true;
                        break;
                    case "dimlineext":
                        returnValue = false;
                        break;
                    case "leaderline":
                        returnValue = false;
                        break;
                    case "leadertext":
                        returnValue = true;
                        break;
                    case "nozzleline":
                        returnValue = false;
                        break;
                    case "nozzlemark":
                        returnValue = true;
                        break;
                };
            }
            else if (targetLine == "leadertext")
            {
                switch (referenceLine)
                {
                    case "outline":
                        returnValue = false;
                        break;
                    case "centerline":
                        returnValue = false;
                        break;
                    case "dimline":
                        returnValue = false;
                        break;
                    case "dimtext":
                        returnValue = false;
                        break;
                    case "dimlineext":
                        returnValue = false;
                        break;
                    case "leaderline":
                        returnValue = false;
                        break;
                    case "leadertext":
                        returnValue = false;
                        break;
                    case "nozzleline":
                        returnValue = false;
                        break;
                    case "nozzlemark":
                        returnValue = false;
                        break;
                };
            }
            else if (targetLine == "nozzleline")
            {
                switch (referenceLine)
                {
                    case "outline":
                        returnValue = true;
                        break;
                    case "centerline":
                        returnValue = false;
                        break;
                    case "dimline":
                        returnValue = true;
                        break;
                    case "dimtext":
                        returnValue = true;
                        break;
                    case "dimlineext":
                        returnValue = true;
                        break;
                    case "leaderline":
                        returnValue = true;
                        break;
                    case "leadertext":
                        returnValue = true;
                        break;
                    case "nozzleline":
                        returnValue = false;
                        break;
                    case "nozzlemark":
                        returnValue = true;
                        break;
                };
            }
            else if (targetLine == "nozzlemark")
            {
                switch (referenceLine)
                {
                    case "outline":
                        returnValue = true;
                        break;
                    case "centerline":
                        returnValue = false;
                        break;
                    case "dimline":
                        returnValue = false;
                        break;
                    case "dimtext":
                        returnValue = false;
                        break;
                    case "dimlineext":
                        returnValue = false;
                        break;
                    case "leaderline":
                        returnValue = false;
                        break;
                    case "leadertext":
                        returnValue = false;
                        break;
                    case "nozzleline":
                        returnValue = false;
                        break;
                    case "nozzlemark":
                        returnValue = false;
                        break;
                };
            }

            return returnValue;
        }
    }
}
