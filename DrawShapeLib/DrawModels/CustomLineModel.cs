using DrawShapeLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawShapeLib.DrawModels
{
    public class CustomLineModel : Notifier
    {
        public CustomLineModel()
        {
            Name = "";
            Description = "";
            Pattern = "";
            Length = "";
        }
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string _Name;
        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        private string _Description;
        public string Pattern
        {
            get { return _Pattern; }
            set
            {
                _Pattern = value;
                OnPropertyChanged(nameof(Pattern));
            }
        }
        private string _Pattern;

        public string Length
        {
            get { return _Length; }
            set
            {
                _Length = value;
                OnPropertyChanged(nameof(Length));
            }
        }
        private string _Length;
    }
}
