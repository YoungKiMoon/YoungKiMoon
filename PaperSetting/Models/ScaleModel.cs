using PaperSetting.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaperSetting.Models
{
    public class ScaleModel:Notifier,ICloneable
    {
        public ScaleModel()
        {
            Value = 0;
            ValueName = "";
        }

        public object Clone()
        {
            ScaleModel newModel = new ScaleModel();
            newModel.Value = Value;
            newModel.ValueName = ValueName;
            return newModel;
        }

        private double _Value;
        public double Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        private string _ValueName;
        public string ValueName
        {
            get { return _ValueName; }
            set
            {
                _ValueName = value;
                OnPropertyChanged(nameof(ValueName));
            }
        }
    }
}
