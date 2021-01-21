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
            Width = 0;
            Height = 0;
        }

        public object Clone()
        {
            SizeModel newModel = new SizeModel();
            newModel.Width = Width;
            newModel.Height = Height;
            return newModel;
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
