using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using devDept.Eyeshot;

namespace DrawWork.DrawStyleServices
{
    public class TextStyleService
    {
        public TextStyleService()
        {

        }
        public List<TextStyle> GetDefaultStyle()
        {
            List<TextStyle> newList = new List<TextStyle>();
            newList.Add(GetStyle("ROMANS", "romans.shx"));
            return newList;
        }
        public TextStyle GetStyle(string styleName, string fontFamilyName, double widthFactor=1)
        {
            TextStyle newTextStyle = new TextStyle(styleName, fontFamilyName,System.Drawing.FontStyle.Regular, widthFactor);
            //newTextStyle.Name = styleName;
            //newTextStyle.WidthFactor = widthFactor;
            return newTextStyle;
        }
    }
}
