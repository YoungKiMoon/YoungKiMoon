using StrengthCalLib.CalModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace StrengthCalLib.CalServices
{
    public class CalImportService
    {
        public CalImportService()
        {

        }

        public void Read(string selFile)
        {
            if (!File.Exists(selFile))
            {
                // 파일 찾을 수 없음
                return;
            }

            //OpenSettings wordOS = new OpenSettings();
            //wordOS.MarkupCompatibilityProcessSettings = new MarkupCompatibilityProcessSettings(MarkupCompatibilityProcessMode.ProcessAllParts, FileFormatVersions.Office2013);
            //using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(selFile, false, wordOS))
            //{
            //    Body body = wordDoc.MainDocumentPart.Document.Body;
            //}
        }

        // XML Test
        //public void ReadXML(string selFile)
        //{
        //    XmlDocument newXDoc = new XmlDocument();

        //    if (!File.Exists(selFile))
        //    {
        //        // 경로 잘못 됨
        //        return;
        //    }

        //    string newText = File.ReadAllText(selFile, Encoding.GetEncoding(1252));

        //    XmlReaderSettings newSetting = new XmlReaderSettings();
        //    newSetting.DtdProcessing = DtdProcessing.Ignore;


        //    XmlReader newReader = XmlReader.Create(selFile, newSetting);

        //    newXDoc.Load(newReader);
        //    XPathNavigator newNav = newXDoc.CreateNavigator();

        //}


        public ObservableCollection<CalLogicModel> ReadDOC(string selFile)
        {
            ObservableCollection<CalLogicModel> newLogicList = new ObservableCollection<CalLogicModel>();

            if (!File.Exists(selFile))
            {
                // 경로 잘못 됨
                return newLogicList;
            }

            // Encoding.Default == ANSI
            //StreamReader sr = new StreamReader(selFile, Encoding.Default);
            //sr.read
            // Get an encoding for code page 1252 (Western Europe character set).
            Encoding cp1252 = Encoding.GetEncoding(1252);
            string tempDoc = File.ReadAllText(selFile, cp1252);
            string textDoc = HtmlToPlainText(tempDoc);

            if (textDoc != "")
            {
                List<string> listData = GetListData(textDoc);
                CalFindService findService = new CalFindService();

                ObservableCollection<CalLogicModel> generalList= findService.GetLogicGeneral();
                findService.SetFindData(ref generalList, listData);

            }

            return newLogicList;
        }

        public List<string> GetListData(string selData)
        {
            List<string> tempSplitList = new List<string>();

            if (selData.Contains(Environment.NewLine))
            {
                string[] tempSplitData = selData.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                tempSplitList.AddRange(tempSplitData);
            }
            return tempSplitList;
        }

        private string HtmlToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />



            const string lineB = @"<\/{0,1}(b|B)\s{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />

            const string tableStart = @"<table[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string tableEnd = @"<\/{0,1}(table|TABLE)\s{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            const string tableTr = @"<\/{0,1}(tr|TR)\s{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            const string tableTd = @"<\/{0,1}(td|TD)\s{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />

            const string lineUlStart = @"<ul[^>]*(>|$)";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            const string lineUl = @"<\/{0,1}(ul|UL)\s{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />\

            const string lineLi = @"<\/{0,1}(li|LI)\s{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />

            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var lineBRegx = new Regex(lineB, RegexOptions.Multiline);

            var tableStartRegx = new Regex(tableStart, RegexOptions.Multiline);
            var tableEndRegx = new Regex(tableEnd, RegexOptions.Multiline);
            var tableTrRegx = new Regex(tableTr, RegexOptions.Multiline);
            var tableTdRegx = new Regex(tableTd, RegexOptions.Multiline);

            var lineUlStartRegx = new Regex(lineUlStart, RegexOptions.Multiline);
            var lineUlRegx = new Regex(lineUl, RegexOptions.Multiline);

            var lineLiRegx = new Regex(lineLi, RegexOptions.Multiline);



            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = text.Replace(Environment.NewLine, " ");

            
            text = tableStartRegx.Replace(text, Environment.NewLine + "-----tablestart-----" + Environment.NewLine);
            text = tableEndRegx.Replace(text, Environment.NewLine + "-----tableend-----" + Environment.NewLine);
            text = tableTrRegx.Replace(text, Environment.NewLine);
            text = tableTdRegx.Replace(text, "|");

            text = lineLiRegx.Replace(text, Environment.NewLine);
            text = lineUlStartRegx.Replace(text, Environment.NewLine + "-----ulstart-----" + Environment.NewLine);
            text = lineUlRegx.Replace(text, "-----ulend-----");


            //text = lineBRegx.Replace(text, Environment.NewLine + "----------" + Environment.NewLine);





            text = lineBreakRegex.Replace(text, Environment.NewLine);
            
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }
    }
}
