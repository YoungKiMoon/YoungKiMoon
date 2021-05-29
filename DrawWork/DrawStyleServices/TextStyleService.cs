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
        public string TextROMANS = "ROMANS";

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
            TextStyle newTextStyle = new TextStyle(styleName, "romans",System.Drawing.FontStyle.Regular, widthFactor);
            //TextStyle newTextStyle = new TextStyle(styleName, "", System.Drawing.FontStyle.Regular, widthFactor);
            newTextStyle.FileName = fontFamilyName;

            //newTextStyle.Name = styleName;
            //newTextStyle.WidthFactor = widthFactor;
            return newTextStyle;
        }
    }
}
