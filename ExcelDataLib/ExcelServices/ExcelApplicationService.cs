using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using ExcelDataLib.ExcelModels;
using ExcelDataLib.Utils;

using xlApp = Microsoft.Office.Interop.Excel.Application;
using xlWin = Microsoft.Office.Interop.Excel.Window;

using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.ObjectModel;
using System.Reflection;
using System.ComponentModel;

namespace ExcelDataLib.ExcelServices
{

    /// <summary>
    /// Collection of currently running Excel instances.
    /// </summary>
    public partial class ExcelApplicationService : IEnumerable<xlApp>
    {

        #region Constructors

        /// <summary>Initializes a new instance of the 
        /// <see cref="ExcelAppCollection"/> class.</summary>
        /// <param name="sessionID">Windows sessionID to filter instances by.
        /// If not assigned, uses current session.</param>
        public ExcelApplicationService(Int32? sessionID = null)
        {
            if (sessionID.HasValue && sessionID.Value < -1)
                throw new ArgumentOutOfRangeException("sessionID");

            this.SessionID = sessionID
                ?? Process.GetCurrentProcess().SessionId;
        }

        #endregion

        #region Properties

        /// <summary>Gets the Windows sessionID used to filter instances.
        /// If -1, uses instances from all sessions.</summary>
        /// <value>The sessionID.</value>
        public Int32 SessionID { get; private set; }

        #endregion

        #region Accessors

        /// <summary>Gets the Application associated with a given process.</summary>
        /// <param name="process">The process.</param>
        /// <returns>Application associated with process.</returns>
        /// <exception cref="System.ArgumentNullException">process</exception>
        public xlApp FromProcess(Process process)
        {
            if (process == null)
                throw new ArgumentNullException("process");
            return InnerFromProcess(process);
        }

        /// <summary>Gets the Application associated with a given processID.</summary>
        /// <param name="processID">The process identifier.</param>
        /// <returns>Application associated with processID.</returns>
        public xlApp FromProcessID(Int32 processID)
        {
            try
            {
                return FromProcess(Process.GetProcessById(processID));
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        /// <summary>Get the Application associated with a given window handle.</summary>
        /// <param name="mainHandle">The window handle.</param>
        /// <returns>Application associated with window handle.</returns>
        public xlApp FromMainWindowHandle(Int32 mainHandle)
        {
            return InnerFromHandle(ChildHandleFromMainHandle(mainHandle));
        }

        /// <summary>Gets the main instance. </summary>
        /// <remarks>This is the oldest running instance.
        /// It will be used if an Excel file is double-clicked in Explorer, etc.</remarks>
        public xlApp PrimaryInstance
        {
            get
            {
                try
                {
                    return Marshal.GetActiveObject(MarshalName) as xlApp;
                }
                catch (COMException)
                {
                    return null;
                }
            }
        }

        /// <summary>Gets the top most instance.</summary>
        /// <value>The top most instance.</value>
        public xlApp TopMostInstance
        {
            get
            {
                var topMost = GetProcesses() //All Excel processes
                    .Select(p => p.MainWindowHandle) //All Excel main window handles
                    .Select(h => new { h = h, z = GetWindowZ(h) }) //Get (handle, z) pair per instance
                    .Where(x => x.z > 0) //Filter hidden instances
                    .OrderBy(x => x.z) //Sort by z value
                    .First(); //Lowest z value

                return FromMainWindowHandle(topMost.h.ToInt32());
            }
        }

        #endregion

        #region Methods

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> 
        /// that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<xlApp> GetEnumerator()
        {
            foreach (var p in GetProcesses())
                yield return FromProcess(p);
        }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        /// <summary>Gets all Excel processes in the current session.</summary>
        /// <returns>Collection of all Excel processing in the current session.</returns>
        public IEnumerable<Process> GetProcesses()
        {

            IEnumerable<Process> result = Process.GetProcessesByName(ProcessName);

            if (this.SessionID >= 0)
                result = result.Where(p => p.SessionId == SessionID);

            return result;
        }

        // 2020-10-09 by Kimd
        // Custom Dic
        public Dictionary<string, List<string>> GetWorkbookDic(xlApp selApp)
        {
            Dictionary<string, List<string>> newDic = new Dictionary<string, List<string>>();

            if (selApp != null)
            {
                foreach (Excel.Workbook eachWork in selApp.Workbooks)
                {
                    List<string> newList = new List<string>();
                    foreach (Excel._Worksheet eachSheet in eachWork.Worksheets)
                    {
                        newList.Add(eachSheet.Name);
                    }
                    newDic.Add(eachWork.Name, newList);
                }
            }

            return newDic;
        }

        public bool CheckTABASExcel(string selWorkbook)
        {
            bool returnValue = false;
            List<object> newWork = GetExcelWorkbookList();
            foreach (Excel.Workbook eachWork in newWork)
            {
                if (eachWork.Name == selWorkbook)
                {
                    ObservableCollection<ExcelWorkSheetModel> newSheets = GetSheetList(eachWork);
                    foreach (ExcelWorkSheetModel eachSheet in newSheets)
                    {
                        if (eachSheet.ExcelSheetName == "AMEdata")
                        {
                            returnValue = true;
                        }
                    }
                        
                }
            }
            return returnValue;
        }

        public bool CheckTABASExcel2(object selWorkbook)
        {
            bool returnValue = false;

            ObservableCollection<ExcelWorkSheetModel> newSheets = GetSheetList(selWorkbook);
            foreach (ExcelWorkSheetModel eachSheet in newSheets)
            {
                if (eachSheet.ExcelSheetName == "AMEdata")
                {
                    returnValue = true;
                }
            }
                
            return returnValue;
        }


        public List<object> GetExcelWorkbookList()
        {
            List<object> newExcelWorkList = new List<object>();
            var newExcelProcess = GetProcesses();
            if (newExcelProcess != null)
            {
                foreach (var eachProcess in newExcelProcess)
                {
                    var newExcel = FromProcess(eachProcess);
                    if (newExcel != null)
                    {
                        foreach (Excel.Workbook eachWork in newExcel.Workbooks)
                        {
                            newExcelWorkList.Add(eachWork);
                        }
                    }
                }
            }
            return newExcelWorkList;

        }

        public ObservableCollection<ExcelWorkSheetModel> GetSheetListAll(string selWorkbook)
        {
            ObservableCollection<ExcelWorkSheetModel> newSheetList = new ObservableCollection<ExcelWorkSheetModel>();

            List<object> newWork = GetExcelWorkbookList();
            foreach (Excel.Workbook eachWork in newWork)
            {
                if (eachWork.Name == selWorkbook)
                {
                    ObservableCollection<ExcelWorkSheetModel> newSheets = GetSheetList(eachWork);
                    foreach (ExcelWorkSheetModel eachSheet in newSheets)
                        newSheetList.Add(eachSheet);
                }

            }

            return newSheetList;
        }


        public ObservableCollection<ExcelWorkSheetModel> GetSheetList(object selWork)
        {
            Excel.Workbook newWork = selWork as Excel.Workbook;
            ObservableCollection<ExcelWorkSheetModel> newList = new ObservableCollection<ExcelWorkSheetModel>();

            foreach (Excel.Worksheet eachSheet in newWork.Worksheets)
            {
                ExcelWorkSheetModel newModel = new ExcelWorkSheetModel();

                // Sheet Area
                int lastRow = eachSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing).Row;
                int lastCol = eachSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing).Column;
                Excel.Range curRange = eachSheet.Cells[1, 1].resize[lastRow, lastCol];
                var curArray = curRange.Value;
                //if(lastRow==1 && lastCol == 1)
                //{
                    

                newModel.RowCount = lastRow;
                newModel.ColumnCount = lastCol;
                newModel.DataArray = curArray;

                newModel.ExcelSheet = eachSheet;
                newModel.ExcelSheetName = eachSheet.Name;

                newList.Add(newModel);
            }

            return newList;

        }

        public void GetSheetData( object selAssembly, ObservableCollection<ExcelWorkSheetModel> selExcelData)
        {
            if(selAssembly != null)
            {
                foreach(PropertyInfo eachPro in selAssembly.GetType().GetProperties())
                {
                    dynamic newModelObject = eachPro.GetValue(selAssembly, null);
                    ObservableCollection<object> newModelCollection = new ObservableCollection<object>(((ICollection)newModelObject).Cast<object>());
                                        
                    if (IsSingleExcelData(newModelCollection))
                    {
                        GetExcelSingleData(selExcelData, newModelCollection);
                    }
                    else
                    {
                        //if (eachPro.Name == "NozzleInputModel")
                        //{
                        //    double a = 1;
                        //}


                        GetExcelMultiData(selExcelData, newModelCollection);
                        IList cc = ((IList)newModelObject);
                        cc.Clear();
                        foreach (var eachVar in newModelCollection)
                            cc.Add(eachVar);

                        eachPro.SetValue(selAssembly, newModelObject, null);
                            
                        
                        
                    }
                    
                }
            }
        }



        private bool IsSingleExcelData(ObservableCollection<object> selModelCollection)
        {
            bool returnValue = false;
            foreach(object selModel in selModelCollection)
                foreach (PropertyInfo eachPro in selModel.GetType().GetProperties())
                {
                    var eachModelValue =eachPro.GetValue(selModel, null);
                    if(eachModelValue is string)
                    {
                        string newEachModelValue = eachModelValue as string;
                        if (newEachModelValue.ToLower().StartsWith("single"))
                        {
                            returnValue = true;
                            break;
                        }
                    }
                }
            return returnValue;
        }

        private void GetExcelSingleData(ObservableCollection<ExcelWorkSheetModel> selExcelData, ObservableCollection<object> selModelCollection)
        {
            foreach(object selModel in selModelCollection)
                foreach (PropertyInfo eachModelInfo in selModel.GetType().GetProperties())
                {
                    var eachModelValue = eachModelInfo.GetValue(selModel, null);
                    if (eachModelValue is string)
                    {

                        string newEachModelValue = eachModelValue as string;
                        string transValue = "";
                        if (newEachModelValue.ToLower().Replace(" ", "").Contains("|"))
                        {
                            string[] eachAddress = newEachModelValue.Split(new char[] { '|' });

                            // SheetName, Row, Col
                            if (eachAddress[2] != "" && eachAddress[3] != "" && eachAddress[4] != "")
                            {
                                foreach (ExcelWorkSheetModel eachSheet in selExcelData)
                                {
                                    if (eachSheet.ExcelSheetName == eachAddress[2])
                                    {
                                        if (eachSheet.DataArray != null)
                                        {
                                            int selRow = Convert.ToInt32(eachAddress[3]);
                                            int selCol = Convert.ToInt32(eachAddress[4]);
                                            if (eachSheet.RowCount >= selRow && eachSheet.ColumnCount >= selCol)
                                            {
                                                var tempTransValue = eachSheet.DataArray[selRow, selCol];
                                                if(tempTransValue != null)
                                                {
                                                    transValue = Convert.ToString(tempTransValue);
                                                }
                                                else
                                                {
                                                    transValue = "";
                                                }
                                                
                                                break;
                                            }

                                        }

                                    }
                                }
                            }
                        }
                        eachModelInfo.SetValue(selModel, transValue, null);
                    }
                }

        }

        private void GetExcelMultiData(ObservableCollection<ExcelWorkSheetModel> selExcelData, ObservableCollection<object> selModelCollection)
        {
            object refModel = null;
            object newModel = null;

            string SheetName = "";
            int RowCountCommnad = 0;
            // 모두가 같은 시트라는 전제
            List<Tuple<int, int>> rowcolList = new List<Tuple<int, int>>();
            foreach (object selModel in selModelCollection)
            {
                foreach (PropertyInfo eachModelInfo in selModel.GetType().GetProperties())
                {
                    var eachModelValue = eachModelInfo.GetValue(selModel, null);
                    if (eachModelValue is string)
                    {
                        string newEachModelValue = eachModelValue as string;
                        if (newEachModelValue.ToLower().Replace(" ", "").Contains("|"))
                        {
                            string[] eachAddress = newEachModelValue.Split(new char[] { '|' });

                            if (eachAddress[1] != "")
                                if (eachAddress[1] != "Auto")
                                    RowCountCommnad = Convert.ToInt32(eachAddress[1]);

                            // SheetName, Row, Col
                            if (eachAddress[2] != "" && eachAddress[3] != "" && eachAddress[4] != "")
                            {
                                SheetName = eachAddress[2];
                                int selRow = Convert.ToInt32(eachAddress[3]);
                                int selCol = Convert.ToInt32(eachAddress[4]);
                                rowcolList.Add(new Tuple<int, int>(selRow, selCol));
                            }
                        }
                        // Default Value
                        eachModelInfo.SetValue(selModel, "", null);
                    }
                    else
                    {
                        rowcolList.Add(new Tuple<int, int>(0, 0));
                    }
                }
                refModel = CloneService.CloneObject(selModel);
            }


            selModelCollection.Clear();


            foreach (ExcelWorkSheetModel eachSheet in selExcelData)
            {
                if (eachSheet.ExcelSheetName == SheetName)
                {
                    if (eachSheet.DataArray != null)
                    {
                        bool runSign = true;
                        bool addRow = false;
                        int rowCount = 0;

                        while (runSign)
                        {
                            addRow = true;
                            List<string> newRow = new List<string>();
                            foreach (Tuple<int, int> eachRow in rowcolList)
                            {
                                if (eachRow.Item1==0 && eachRow.Item2 == 0)
                                {
                                    //newRow.Add("!@#$%");
                                    newRow.Add("");
                                }
                                else
                                {
                                    // add Row
                                    int currentRow = eachRow.Item1 + rowCount;

                                    int currentCol = eachRow.Item2;
                                    string transValue = "";
                                    if (eachSheet.RowCount >= currentRow && eachSheet.ColumnCount >= currentCol)
                                    {
                                        var tempTransValue = eachSheet.DataArray[currentRow, currentCol];
                                        if (tempTransValue != null)
                                        {
                                            transValue = Convert.ToString(tempTransValue);
                                        }
                                    }
                                    newRow.Add(transValue);
                                }
                            }

                            if (string.Join("", newRow) == "")
                            {
                                runSign = false;
                                addRow = false;
                            }

                            // rowCount
                            rowCount++;



                            // RowCount Command
                            if (RowCountCommnad > 0)
                            {
                                addRow = true;
                                if (rowCount == RowCountCommnad)
                                    runSign = false;
                                else
                                    runSign = true;
                            }

                            // First : Default
                            if (rowCount == 0)
                                addRow = true;

                            if (addRow)
                            {
                                newModel = CloneService.CloneObject(refModel);
                                int i = 0;
                                foreach(PropertyInfo eachPro in newModel.GetType().GetProperties())
                                {
                                    //if(newRow[i]!= "!@#$%")
                                    //    eachPro.SetValue(newModel, newRow[i], null);
                                    if(eachPro.PropertyType.Name!="Double")
                                        eachPro.SetValue(newModel, newRow[i], null);
                                    i++;
                                    if (newRow.Count == i)
                                        break;
                                }
                                selModelCollection.Add(newModel);
                            }

                            // Exit Roof
                            if (rowCount >= eachSheet.RowCount)
                                runSign = false; 


                        }

                    }

                    break;
                }
            }

        }


        public List<List<string>> GetSheetData(string selSheetName, List<Tuple<int, int>> selDataList, bool isSingle)
        {
            List<List<string>> returnList = new List<List<string>>();

            if (isSingle)
            {

            }
            else
            {

            }

            return returnList;
        }



        #endregion
    }


    public partial class ExcelApplicationService
    {

        #region Methods

        private static xlApp InnerFromProcess(Process p)
        {
            return InnerFromHandle(ChildHandleFromMainHandle(p.MainWindowHandle.ToInt32()));
        }

        private static Int32 ChildHandleFromMainHandle(Int32 mainHandle)
        {
            Int32 handle = 0;
            EnumChildWindows(mainHandle, EnumChildFunc, ref handle);
            return handle;
        }

        private static xlApp InnerFromHandle(Int32 handle)
        {
            xlWin win = null;
            Int32 hr = AccessibleObjectFromWindow(handle, DW_OBJECTID, rrid.ToByteArray(), ref win);
            if (win != null)
            {
                return win.Application;
            }
            else
            {
                return null;
            }

        }

        private static Int32 GetWindowZ(IntPtr handle)
        {
            var z = 0;
            for (IntPtr h = handle; h != IntPtr.Zero; h = GetWindow(h, GW_HWNDPREV))
                z++;
            return z;
        }

        private static Boolean EnumChildFunc(Int32 hwndChild, ref Int32 lParam)
        {
            var buf = new StringBuilder(128);
            GetClassName(hwndChild, buf, 128);
            if (buf.ToString() == ComClassName)
            {
                lParam = hwndChild;
                return false;
            }
            return true;
        }

        #endregion

        #region Extern Methods

        [DllImport("Oleacc.dll")]
        private static extern Int32 AccessibleObjectFromWindow(
            Int32 hwnd, UInt32 dwObjectID, Byte[] riid, ref xlWin ptr);

        [DllImport("User32.dll")]
        private static extern Boolean EnumChildWindows(
            Int32 hWndParent, EnumChildCallback lpEnumFunc, ref Int32 lParam);

        [DllImport("User32.dll")]
        private static extern Int32 GetClassName(
            Int32 hWnd, StringBuilder lpClassName, Int32 nMaxCount);

        [DllImport("User32.dll")]
        private static extern IntPtr GetWindow(IntPtr hWnd, UInt32 uCmd);

        #endregion

        #region Constants & delegates

        private const String MarshalName = "Excel.Application";

        private const String ProcessName = "EXCEL";

        private const String ComClassName = "EXCEL7";

        private const UInt32 DW_OBJECTID = 0xFFFFFFF0;

        private const UInt32 GW_HWNDPREV = 3;
        //3 = GW_HWNDPREV
        //The retrieved handle identifies the window above the specified window in the Z order.
        //If the specified window is a topmost window, the handle identifies a topmost window.
        //If the specified window is a top-level window, the handle identifies a top-level window.
        //If the specified window is a child window, the handle identifies a sibling window.

        private static Guid rrid = new Guid("{00020400-0000-0000-C000-000000000046}");

        private delegate Boolean EnumChildCallback(Int32 hwnd, ref Int32 lParam);
        #endregion
    }

}
