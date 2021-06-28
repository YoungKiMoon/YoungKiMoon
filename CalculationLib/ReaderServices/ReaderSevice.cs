using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationLib.FilesServices;

namespace CalculationLib.ReaderServices
{
    public class ReaderSevice
    {
        public ReaderSevice()
        {

        }
        public void CreateCalculation()
        {
            string url = @"C:\Users\tree\Desktop\CAD\tabas\AMEData\1.CRT_Calculation.doc";
            TextFileService docService = new TextFileService();
            string calDoc =  docService.GetTextFileString(url);
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(calDoc);
        }
    }
}
