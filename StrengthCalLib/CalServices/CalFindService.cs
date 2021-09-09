using StrengthCalLib.CalMatchingModels;
using StrengthCalLib.CalModels;
using StrengthCalLib.Commons;
using StrengthCalLib.ValueServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace StrengthCalLib.CalServices
{
    public class CalFindService
    {
        ValueService valueService { get; set; }
        public CalFindService()
        {
            valueService = new ValueService();
        }

        public List<CalLogicModel> GetLogicData()
        {
            List<CalLogicModel> newList = new List<CalLogicModel>();

            return newList;
        }

        public ObservableCollection<CalLogicModel> GetLogicGeneral()
        {
            ObservableCollection<CalLogicModel> newList = new ObservableCollection<CalLogicModel>();

            string General = "General";

            // Default Value

            SearchDirection_Type Front = SearchDirection_Type.FRONT;    // from End : 공백
            SearchDirection_Type Back = SearchDirection_Type.BACK;      // from End : end
            bool InTable = true; // Table : Yes
            bool customLogic = true; // Custom Logic : 초록색
            string dash = "-";
            string TBD = "TBD";

            newList.Add(GetNewLogicModel(true, false, General, "APPLIED CODE", "", "Design Basis", "", Front, false, "", "", dash, "", 7, 6));
            newList.Add(GetNewLogicModel(true, false, General, "APPENDICES USED", "", "Appendices Used", "", Front, InTable, "", "", dash, "", 8, 6));



            return newList;
        }
        private CalLogicModel GetNewLogicModel(bool calData, bool customLogic,
                                                string Sheet, string Part, string Contents, string AMETex1, string AMEText2, SearchDirection_Type SearchDirection,bool InTable,
                                                string AMEOriginValue, string AMEValue, string DefaultValue, string SheetValue, double Row, double Column )
        {
            CalLogicModel newModel = new CalLogicModel();
            
            // 무조건 True
            newModel.CalData = calData;

            // 파란색/ 초록색
            newModel.CustomLogic = customLogic;

            // 시트 명
            newModel.Sheet = Sheet;
            newModel.Part = Part;
            newModel.Contents = Contents;

            newModel.AMEText1 = AMETex1;
            newModel.AMEText2 = AMEText2;

            newModel.SearchDirection = SearchDirection;
            newModel.InTable = InTable;

            // 무조건 공백
            newModel.AMEOriginValue = AMEOriginValue;
            // 무조건 공백
            newModel.AMEValue = AMEValue;
            // 값 있는 데로 : 단, 빨간색 제외
            newModel.DefaultValue = DefaultValue;
            // 무조건 공백
            newModel.SheetValue = SheetValue;


            newModel.Row = Row;
            newModel.Column = Column;

            
            return newModel;
        }


        public void SetFindData(ref ObservableCollection<CalLogicModel> selModelData, List<string> selCalData)
        {
            CalMatchingService matchingService = new CalMatchingService();

            List<string> selCalTableData = GetTableData(ref selCalData);
            StringComparison comp = StringComparison.Ordinal;


            // Share Data : 후행 작업 시 사용됨
            StructureTypeModel structureType = new StructureTypeModel();
            BottomStyleModel bottomType = new BottomStyleModel();
            RoofTypeModel roofType = new RoofTypeModel();
            WindCodeModel windCode = new WindCodeModel();
            AnchorModel anchorBolt = new AnchorModel();
            double tankID = 0;
            string bottomValue = "";
            string anchorBoltStr = "";
            // 선행 작업
            foreach (CalLogicModel eachModel in selModelData)
            {
                if (eachModel.CustomLogic)
                {

                    // Custom Logic : All
                    if (eachModel.SearchDirection == SearchDirection_Type.FRONT)
                    {
                        // Search : AMEText 1 : Front
                        for (int i = 0; i < selCalData.Count; i++)
                        {
                            if (selCalData[i].IndexOf(eachModel.AMEText1, comp) >= 0)
                            {
                                if (eachModel.AMEText1 == "Structure Support Type")
                                {
                                    string tempStructureSupporType = GetOneLineValue01(selCalData[i], eachModel.AMEText1, eachModel.AMEText2);
                                    structureType = matchingService.GetStructureType(tempStructureSupporType);
                                    eachModel.AMEOriginValue = structureType.TABAS;
                                    roofType = matchingService.GetRoofType(structureType.RoofType);
                                    break;
                                }
                                else if (eachModel.AMEText1 == "Bottom Type =")
                                {
                                    string tempBottomType = GetOneLineValue01(selCalData[i], eachModel.AMEText1, eachModel.AMEText2);
                                    bottomType = matchingService.GetBottomType(tempBottomType);
                                    eachModel.AMEOriginValue = bottomType.TABAS;
                                    break;
                                }
                                else if (eachModel.AMEText1 == "Wind Load Basis")
                                {
                                    string tempWindCode = GetOneLineValue01(selCalData[i], eachModel.AMEText1, eachModel.AMEText2);
                                    windCode = matchingService.GetWindCode(tempWindCode);
                                    eachModel.AMEOriginValue = windCode.WindCode;
                                    break;
                                }
                                else if (eachModel.AMEText1 == "Ma-anchor =")
                                {
                                    anchorBoltStr = GetOneLineValue01(selCalData[i], eachModel.AMEText1, eachModel.AMEText2);
                                    anchorBolt = matchingService.GetAnchorNut(anchorBoltStr);
                                    eachModel.AMEOriginValue = anchorBoltStr;
                                    break;
                                }


                            }
                        }
                    }
                    else
                    {
                        // Search : Back
                        for (int i = selCalData.Count - 1; i >= 0; i--)
                        {
                            if (selCalData[i].IndexOf(eachModel.AMEText1, comp) >= 0)
                            {
                                if (eachModel.AMEText1 == "Structure Support Type")
                                {

                                    string tempStructureSupporType = GetOneLineValue01(selCalData[i], eachModel.AMEText1, eachModel.AMEText2);
                                    structureType = matchingService.GetStructureType(tempStructureSupporType);
                                    eachModel.AMEOriginValue = structureType.TABAS;
                                    roofType = matchingService.GetRoofType(structureType.RoofType);
                                    break;
                                }
                                else if (eachModel.AMEText1 == "Bottom Type =")
                                {
                                    string tempBottomType = GetOneLineValue01(selCalData[i], eachModel.AMEText1, eachModel.AMEText2);
                                    bottomType = matchingService.GetBottomType(tempBottomType);
                                    eachModel.AMEOriginValue = bottomType.TABAS;
                                    break;
                                }
                                else if (eachModel.AMEText1 == "Wind Load Basis")
                                {
                                    string tempWindCode = GetOneLineValue01(selCalData[i], eachModel.AMEText1, eachModel.AMEText2);
                                    windCode = matchingService.GetWindCode(tempWindCode);
                                    eachModel.AMEOriginValue = windCode.WindCode;
                                    break;
                                }
                                else if (eachModel.AMEText1 == "Ma-anchor =")
                                {
                                    anchorBoltStr = GetOneLineValue01(selCalData[i], eachModel.AMEText1, eachModel.AMEText2);
                                    anchorBolt = matchingService.GetAnchorNut(anchorBoltStr);
                                    eachModel.AMEOriginValue = anchorBoltStr;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            // 선행 작업 보강 
            foreach (CalLogicModel eachModel in selModelData)
            {

                // Part
                if(eachModel.Part=="Roof Type")
                {
                    eachModel.AMEOriginValue = structureType.RoofType;
                }
                else if (eachModel.Part == "BOTTOM")
                {
                    if (eachModel.Contents == "SLOPE")
                    {
                        eachModel.AMEText1 = roofType.AMEtxt01;
                        eachModel.AMEText2 = roofType.AMEtxt02;
                        eachModel.SearchDirection = roofType.fromEnd;
                    }
                }
                else if (eachModel.Part == "ROOF")
                {
                    if (eachModel.Contents == "SLOPE/RADIUS RATIO")
                    {
                        eachModel.AMEText1 = roofType.AMEtxt01;
                        eachModel.AMEText2 = roofType.AMEtxt02;
                        eachModel.SearchDirection = roofType.fromEnd;
                    }
                }

                // Contents
                if (eachModel.Contents == "ANCHOR NUT")
                {
                    eachModel.AMEOriginValue = anchorBolt.Nut;
                }


                // AME Text 1
                if (eachModel.AMEText1=="Roof Type =")
                {
                    eachModel.AMEText1 = roofType.AMEtxt01;
                    eachModel.AMEText2 = roofType.AMEtxt02;
                    eachModel.SearchDirection= roofType.fromEnd;
                    
                }

                
            }


            // ----------------------------------------
            // Search
            bool findValue = false;
            foreach (CalLogicModel eachModel in selModelData)
            {
                // Search : again : Start Point
                searchAgain:

                findValue = false;
                // AME Text 값 있는 것 부터
                if (eachModel.AMEText1 != "")
                {
                    if (!eachModel.CustomLogic)
                    {
                        // Search Logic
                        if (eachModel.InTable)
                        {
                            // Search Logic : Table
                            if (eachModel.SearchDirection == SearchDirection_Type.FRONT)
                            {
                                // Search : Front
                                for (int i = 0; i < selCalTableData.Count; i++)
                                {
                                    if (selCalTableData[i].IndexOf(eachModel.AMEText1, comp) >= 0)
                                    {
                                        if (eachModel.AMEText1.Contains("Material"))
                                            eachModel.AMEOriginValue = GetMaterial(i, selCalData, eachModel.AMEText1);
                                        else
                                            eachModel.AMEOriginValue = GetOneTableValue01(selCalTableData[i], selCalTableData[i + 1], eachModel.AMEText1, eachModel.AMEText2);


                                        findValue = true;
                                        break;


                                    }
                                }
                            }
                            else
                            {
                                // Search : Back
                                for (int i = selCalTableData.Count - 1; i >= 0; i--)
                                {
                                    if (selCalTableData[i].IndexOf(eachModel.AMEText1, comp) >= 0)
                                    {
                                        if (eachModel.AMEText1.Contains("Material"))
                                            eachModel.AMEOriginValue = GetMaterial(i, selCalData, eachModel.AMEText1);
                                        else
                                            eachModel.AMEOriginValue = GetOneTableValue01(selCalTableData[i], selCalTableData[i + 1], eachModel.AMEText1, eachModel.AMEText2);


                                        findValue = true;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Search Logic : All
                            if (eachModel.SearchDirection == SearchDirection_Type.FRONT)
                            {
                                // Search : AMEText 1 : Front
                                for (int i = 0; i < selCalData.Count; i++)
                                {
                                    if (selCalData[i].IndexOf(eachModel.AMEText1, comp) >=0)
                                    {
                                        if (eachModel.AMEText2 != "")
                                        {
                                            // Search : AMEText 2 : Front
                                            for (int j = i; j < selCalData.Count; j++)
                                            {
                                                if (selCalData[j].IndexOf(eachModel.AMEText2, comp)>=0)
                                                {
                                                    eachModel.AMEOriginValue = GetOneLineValue01(selCalData[i], eachModel.AMEText1, eachModel.AMEText2);


                                                    findValue = true;
                                                    break;

                                                }
                                            }
                                        }
                                        else
                                        {
                                            eachModel.AMEOriginValue = GetOneLineValue01(selCalData[i], eachModel.AMEText1, eachModel.AMEText2);


                                            findValue = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Search : Back
                                for (int i = selCalData.Count - 1; i >= 0; i--)
                                {
                                    if (selCalData[i].IndexOf(eachModel.AMEText1, comp) >= 0)
                                    {
                                        if (eachModel.AMEText2 != "")
                                        {
                                            // Search : AMEText 2 : Front
                                            for (int j = i ; j < selCalData.Count; j++)
                                            {
                                                if (selCalData[j].IndexOf(eachModel.AMEText2, comp) >= 0)
                                                {
                                                    eachModel.AMEOriginValue = GetOneLineValue01(selCalData[i], eachModel.AMEText1, eachModel.AMEText2);


                                                    findValue = true;
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            eachModel.AMEOriginValue = GetOneLineValue01(selCalData[i], eachModel.AMEText1, eachModel.AMEText2);
                                            
                                            
                                            
                                            findValue = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                    }

                }


                // Search : again
                if (findValue == false)
                {
                    if(eachModel.AMEText1.Contains("t-actual ="))
                    {
                        eachModel.AMEText1 = "t.actual =";
                        goto searchAgain;
                    }
                    if(eachModel.AMEText1.Contains("Ma-deck ="))
                    {
                        eachModel.AMEText1 = "Ma-upper-deck =";
                        goto searchAgain;
                    }

                }
            }


            // ----------------------------------------
            // AME Original Data -> AME Value
            foreach(CalLogicModel eachModel in selModelData)
            {
                if (eachModel.AMEOriginValue != "")
                {
                    if (eachModel.AMEOriginValue.IndexOf("mmh2o", comp) >= 0)
                    {
                        string tempValue = "";
                        string tempStr = eachModel.AMEOriginValue.Replace("mmh2o", "").Trim();
                        if (tempStr.Contains("0"))
                            tempValue = "ATM";
                        else if (tempStr.Contains("-0"))
                            tempValue = "-";
                        eachModel.AMEValue = tempValue;
                    }
                    else if (eachModel.AMEOriginValue.IndexOf("mm", comp) >= 0)
                    {
                        eachModel.AMEValue = eachModel.AMEOriginValue.Replace("mm", "").Trim();
                    }
                    else if (eachModel.AMEOriginValue.IndexOf("m", comp) >= 0)
                    {
                        string tempStr= eachModel.AMEOriginValue.Replace("mm", "").Trim(); ;
                        double tempDouble = valueService.GetDoubleValue(tempStr);
                        eachModel.AMEValue = (tempDouble * 1000).ToString();
                    }
                    else if (eachModel.AMEOriginValue.IndexOf("KPa", comp) >= 0)
                    {
                        string tempStr = eachModel.AMEOriginValue.Replace("KPa", "").Trim(); ;
                        double tempDouble = valueService.GetDoubleValue(tempStr);
                        eachModel.AMEValue = valueService.GetRoundUp(tempDouble * 101.97,0).ToString();
                    }
                    else if (eachModel.AMEOriginValue.IndexOf("Kg", comp) >= 0)
                    {
                        string tempStr = eachModel.AMEOriginValue.Replace("Kg", "").Trim(); ;
                        double tempDouble = valueService.GetDoubleValue(tempStr);
                        eachModel.AMEValue = valueService.GetRoundUp(tempDouble * 0.001, 0).ToString();
                    }
                    else if (eachModel.AMEOriginValue.IndexOf("kph", comp) >= 0)
                    {
                        string tempStr = eachModel.AMEOriginValue.Replace("kph", "").Trim(); ;
                        double tempDouble = valueService.GetDoubleValue(tempStr);
                        eachModel.AMEValue = Math.Round(tempDouble * 0.277778, 1,MidpointRounding.AwayFromZero).ToString();
                    }
                    else
                    {
                        eachModel.AMEValue = eachModel.AMEOriginValue;
                    }

                    // Etc
                    if (eachModel.AMEText1.Contains("ID of Tank"))
                        tankID = valueService.GetDoubleValue(eachModel.AMEValue);


                }
            }


            // AME Value -> Sheet Value
            string appliedCode = "";
            double sizeHeight = 0;
            foreach (CalLogicModel eachModel in selModelData)
            {
                // Part
                if (eachModel.Part == "SHELL DESIGN")
                {
                    eachModel.SheetValue = appliedCode + " & " + eachModel.AMEValue;
                }
                else if (eachModel.Part == "APPLIED CODE")
                {
                    appliedCode = eachModel.AMEValue;
                }
                else if (eachModel.AMEText1.Contains("Slope"))
                {

                }
                else if (eachModel.AMEText1.Contains("Rs ="))
                {

                }

                // Contents
                if (eachModel.Contents == "HEIGHT")
                {
                    sizeHeight = valueService.GetDoubleValue(eachModel.Contents);
                }
                else if(eachModel.Contents=="HIGH HIgH LIQUId LEVEL (HHLL)")
                {
                    eachModel.SheetValue = valueService.GetIntRoundUp(sizeHeight * 0.91, 2).ToString();
                }
                else if (eachModel.Contents == "HIGH LIQUID LEVEL (HLL)")
                {
                    eachModel.SheetValue = valueService.GetIntRoundUp(sizeHeight * 0.88, 2).ToString();
                }
                else if (eachModel.Contents == "LOW LIQUID LEVEL (LLL)")
                {
                    eachModel.SheetValue = valueService.GetIntRoundUp(sizeHeight * 0.12, 2).ToString();
                }
                else if (eachModel.Contents == "LOW LOW LIQUID LEVEL (LLLL)")
                {
                    eachModel.SheetValue = valueService.GetIntRoundUp(sizeHeight * 0.11, 2).ToString();
                }

                // Basic
                if (eachModel.AMEValue != "")
                {
                    eachModel.SheetValue = eachModel.AMEValue;
                }
            }



            // Sheet Value || Default -> Value
            foreach (CalLogicModel eachModel in selModelData)
            {


                // Contents
                if (eachModel.Contents.Contains("ANCHOR CHAIR PLATE") ||
                    eachModel.Contents.Contains("ANCHOR WASHER"))
                {
                    if (anchorBoltStr != "")
                        eachModel.DefaultValue = "A383-C";
                    else
                        eachModel.DefaultValue = "-";
                }
                else if (eachModel.Contents.Contains("SHELL (t)"))
                {
                    if (eachModel.AMEOriginValue != "")
                        eachModel.DefaultValue = "YES";
                    else
                        eachModel.DefaultValue = "NO";
                }
                else if (eachModel.Contents.Contains("ROOF (t)"))
                {
                    if (eachModel.AMEOriginValue != "")
                        eachModel.DefaultValue = "YES";
                    else
                        eachModel.DefaultValue = "NO";
                }

                // Value
                if (eachModel.SheetValue != "")
                {
                    eachModel.Value = eachModel.SheetValue;
                }
                else
                {
                    eachModel.Value = eachModel.DefaultValue;
                }


            }


            // AME Origin

        }


        

        // Material
        private string GetMaterial(int startNum , List<string> selCalData,string ameText1)
        {
            string returnValue = "";

            int columnNum = 0;
            string[] headerArray = selCalData[startNum].Split('|');
            for(int i = 0; i < headerArray.Length; i++)
            {
                if (headerArray.Contains(ameText1))
                {
                    columnNum = i;
                    break;
                }
            }

            List<string> sumList = new List<string>();
            for(int i = startNum + 1; i < selCalData.Count; i++)
            {
                if (selCalData[i].Contains('|'))
                {
                    string[] tempArray = selCalData[i].Split('|');
                    sumList.Add(tempArray[columnNum]);
                }
                else
                {
                    break;
                }
            }
            Dictionary<string, string> sumDicList = new Dictionary<string, string>();
            foreach(string eachStr in sumList)
            {
                if (!sumDicList.ContainsKey(eachStr))
                    sumDicList.Add(eachStr, eachStr);
            }

            List<string> sumDicListList = sumDicList.Keys.ToList();
            foreach(string eachStr in sumDicListList)
            {
                string newEachStr = eachStr.Replace("-Normalized", "N");
                if (returnValue == "")
                    returnValue = newEachStr;
                else
                    returnValue = returnValue + "/" + newEachStr;
            }

            return returnValue;
        }

        private string GetOneTableValue01(string selData, string selDataNext, string ameText1, string ameText2)
        {
            string returnValue = "";
            if(ameText1== "Appendices Used")
            {
                if (selData.Contains("|"))
                {
                    string[] tempArray = selData.Split('|');
                    returnValue = tempArray[1].Trim();
                }
            }
            else
            {
                // 다음 행 같은 열 가져오기
                if (selData.Contains("|"))
                {
                    string[] tempArray = selData.Split('|');
                    for(int i = 0; i < tempArray.Length; i++)
                    {
                        if (tempArray[i].Contains(ameText1))
                        {
                            if (selDataNext.Contains("|"))
                            {
                                string[] tempArrayNext = selDataNext.Split('|');
                                string newValue = tempArrayNext[i].Replace(",", "");
                                returnValue = tempArrayNext[i];
                            }
                            break;
                        }
                    }
                }
            }
            return returnValue;
        }

        private string GetOneLineValue01(string selData,string ameText1,string ameText2)
        {
            string returnValue = "";
            string blankStr = " ";

            string splitStr = ameText1;
            if (ameText2 != "")
                splitStr = ameText2;
            string[] selDataArray = selData.Split(new string[] { splitStr }, StringSplitOptions.RemoveEmptyEntries);
            string oneData = "";
            if (selDataArray.Length==1)
                oneData = selDataArray[0];
            if(selDataArray.Length>=2)
                oneData = selDataArray[1];

            oneData = oneData.Replace("=", "");
            oneData = oneData.Replace(":", "");
            oneData = oneData.Replace(",", "");
            oneData = oneData.Trim();

            // 조건 1
            if (ameText1 == "Design SG")
            {
                if (oneData.Contains(")"))
                {
                    string[] tempDataArray = oneData.Split(')');
                    returnValue = tempDataArray[0].Trim();
                }
            }
            else if(ameText1 =="Structure Support Type")
            {
                returnValue= oneData.Trim();
            }
            // 공백으로 나누기
            else
            {
                if (oneData.Contains(blankStr))
                {
                    string[] tempDataArray = oneData.Split(' ');
                    returnValue = tempDataArray[0].Trim();
                }
                else
                {
                    returnValue = oneData.Trim();
                }
            }
            return returnValue;
        }


        private List<string> GetTableData(ref List<string> selData)
        {
            List<string> newData = new List<string>();

            string startTable = "-----tablestart-----";
            string endTable = "-----tableend-----";
            bool isTable = false;
            foreach(string eachData in selData)
            {
                if (eachData.Contains(startTable))
                {
                    isTable = true;
                    newData.Add(startTable);
                }
                else if (eachData.Contains(endTable))
                {
                    isTable = false;
                    newData.Add(endTable);
                }
                else
                {
                    if (isTable)
                        newData.Add(eachData);

                }
                
            }
            return newData;
        }


    }
}
