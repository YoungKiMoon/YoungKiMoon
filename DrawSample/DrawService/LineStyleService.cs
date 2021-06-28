using devDept.Eyeshot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSample.DrawService
{
    public class LineStyleService
    {
        public LineStyleService()
        {

        }

        public List<LinePattern> GetDefaultStyle()
        {
            List<LinePattern> newList = new List<LinePattern>();
            newList.Add(GetPattern("CENTER2", new float[] { 7.5f, -1.25f, 1.25f, -1.25f }, "Center2"));
            newList.Add(GetPattern("PHANTOM2", new float[] { 6.25f, -1.25f, 1.25f, -1.25f, 1.25f, -1.25f }, "Phantom"));
            newList.Add(GetPattern("HIDDEN", new float[] { 2.5f, -1.25f }, "Hidden"));
            newList.Add(GetPattern("CONTINU", null, "Continuous"));
            //newList.Add(GetPattern("CONTINUOUS", null,"Continuous _"));

            return newList;
        }
        public LinePattern GetPattern(string lineName, float[] selPattern, string selDescription)
        {
            LinePattern newPattern = new LinePattern(lineName, selPattern, selDescription);
            return newPattern;
        }


    }
}
