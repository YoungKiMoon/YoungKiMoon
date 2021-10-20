using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using devDept.Eyeshot;

namespace DrawWork.DrawStyleServices
{
    public class LineStyleService
    {
        public LineStyleService()
        {

        }

        public List<LineType> GetDefaultStyle()
        {
            List<LineType> newList = new List<LineType>();
            newList.Add(GetPattern("CENTER2", new float[] { 7.5f, -1.25f, 1.25f, -1.25f },"Center2"));
            newList.Add(GetPattern("PHANTOM2", new float[] { 6.25f, -1.25f, 1.25f, -1.25f, 1.25f, -1.25f },"Phantom"));
            newList.Add(GetPattern("HIDDEN", new float[] {2.5f, -1.25f },"Hidden"));
            newList.Add(GetPattern("CONTINU", null, "Continuous"));
            //newList.Add(GetPattern("CONTINUOUS", null,"Continuous _"));

            return newList;
        }
        public LineType GetPattern(string lineName,float[] selPattern,string selDescription)
        {
            LineType newPattern = new LineType(lineName,selPattern,selDescription);
            return newPattern;
        }


    }
}
