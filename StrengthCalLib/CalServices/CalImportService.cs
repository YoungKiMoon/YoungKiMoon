using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        //public void Read(string selFile)
        //{
        //    XmlDocument newXDoc = new XmlDocument();

        //    if(!File.Exists(selFile))
        //    {
        //        // 경로 잘못 됨
        //        return;
        //    }

        //    string newText= File.ReadAllText(selFile, Encoding.GetEncoding(1252));

        //    XmlReaderSettings newSetting = new XmlReaderSettings();
        //    newSetting.DtdProcessing = DtdProcessing.Ignore;


        //    XmlReader newReader = XmlReader.Create(selFile, newSetting);

        //    newXDoc.Load(newReader);
        //    XPathNavigator newNav = newXDoc.CreateNavigator();

        //}
    }
}
