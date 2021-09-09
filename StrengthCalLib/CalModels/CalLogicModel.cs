using StrengthCalLib.Commons;
using StrengthCalLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrengthCalLib.CalModels
{
    public class CalLogicModel : Notifier
    {
        public CalLogicModel()
        {
            CalData = false;

            AMEOriginValue = "";
            AMEValue = "";
            SheetValue = "";
            DefaultValue = "";
            Value = "";

            Row = 1;
            Column = 1;

            Sheet = "";
            Part = "";
            Contents = "";
            AMEText1 = "";
            AMEText2 = "";
            SearchDirection = SearchDirection_Type.FRONT;
            InTable = false;

            CustomLogic = false;

        }

        public string AMEOriginValue
        {
            get { return _AMEOriginValue; }
            set
            {
                _AMEOriginValue = value;
                OnPropertyChanged(nameof(AMEOriginValue));
            }
        }
        private string _AMEOriginValue;

        public string AMEValue
        {
            get { return _AMEValue; }
            set
            {
                _AMEValue = value;
                OnPropertyChanged(nameof(AMEValue));
            }
        }
        private string _AMEValue;

        public string SheetValue
        {
            get { return _SheetValue; }
            set
            {
                _SheetValue = value;
                OnPropertyChanged(nameof(SheetValue));
            }
        }
        private string _SheetValue;


        public string DefaultValue
        {
            get { return _DefaultValue; }
            set
            {
                _DefaultValue = value;
                OnPropertyChanged(nameof(DefaultValue));
            }
        }
        private string _DefaultValue;

        public string Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                OnPropertyChanged(nameof(Value));
            }
        }
        private string _Value;



        public double Row
        {
            get { return _Row; }
            set
            {
                _Row = value;
                OnPropertyChanged(nameof(Row));
            }
        }
        private double _Row;


        public double Column
        {
            get { return _Column; }
            set
            {
                _Column = value;
                OnPropertyChanged(nameof(Column));
            }
        }
        private double _Column;


        public string Sheet
        {
            get { return _Sheet; }
            set
            {
                _Sheet = value;
                OnPropertyChanged(nameof(Sheet));
            }
        }
        private string _Sheet;

        public string Part
        {
            get { return _Part; }
            set
            {
                _Part = value;
                OnPropertyChanged(nameof(Part));
            }
        }
        private string _Part;

        public string Contents
        {
            get { return _Contents; }
            set
            {
                _Contents = value;
                OnPropertyChanged(nameof(Contents));
            }
        }
        private string _Contents;

        public string AMEText1
        {
            get { return _AMEText1; }
            set
            {
                _AMEText1 = value;
                OnPropertyChanged(nameof(AMEText1));
            }
        }
        private string _AMEText1;

        public string AMEText2
        {
            get { return _AMEText2; }
            set
            {
                _AMEText2 = value;
                OnPropertyChanged(nameof(AMEText2));
            }
        }
        private string _AMEText2;

        public SearchDirection_Type SearchDirection
        {
            get { return _SearchDirection; }
            set
            {
                _SearchDirection = value;
                OnPropertyChanged(nameof(SearchDirection));
            }
        }
        private SearchDirection_Type _SearchDirection;

        public bool InTable
        {
            get { return _InTable; }
            set
            {
                _InTable = value;
                OnPropertyChanged(nameof(InTable));
            }
        }
        private bool _InTable;

        public bool CustomLogic
        {
            get { return _CustomLogic; }
            set
            {
                _CustomLogic = value;
                OnPropertyChanged(nameof(CustomLogic));
            }
        }
        private bool _CustomLogic;

        public bool CalData
        {
            get { return _CalData; }
            set
            {
                _CalData = value;
                OnPropertyChanged(nameof(CalData));
            }
        }
        private bool _CalData;
    }
}
