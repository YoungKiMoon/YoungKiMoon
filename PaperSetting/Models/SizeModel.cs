using PaperSetting.Commons;
using PaperSetting.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaperSetting.Models
{
    public class SizeModel : Notifier,ICloneable
    {
        public SizeModel()
        {
            Name = "";
            PaperType = PAPERFORMAT_TYPE.A2_ISO; // Default

            Width = 0;
            Height = 0;
        }
        public SizeModel(string selName, PAPERFORMAT_TYPE selPaperType, double selWidth, double selHeight)
        {
            Name = selName;
            PaperType = selPaperType;
            Width = selWidth;
            Height = selHeight;
        }

        public object Clone()
        {
            SizeModel newModel = new SizeModel();
            newModel.Name = Name;
            newModel.PaperType = PaperType;
            newModel.Width = Width;
            newModel.Height = Height;
            return newModel;
        }
        


        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private PAPERFORMAT_TYPE _PaperType;
        public PAPERFORMAT_TYPE PaperType
        {
            get { return _PaperType; }
            set
            {
                _PaperType = value;
                OnPropertyChanged(nameof(PaperType));
            }
        }

        private double _Width;
        public double Width
        {
            get { return _Width; }
            set
            {
                _Width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        private double _Height;
        public double Height
        {
            get { return _Height; }
            set
            {
                _Height = value;
                OnPropertyChanged(nameof(Height));
            }
        }
    }
}
