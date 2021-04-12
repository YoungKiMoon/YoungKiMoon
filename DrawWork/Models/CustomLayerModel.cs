using DrawWork.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.Models
{
    public class CustomLayerModel : Notifier
    {
        public CustomLayerModel()
        {
            Name = "";
            LineWeight = "";
            LayerColor = "";
            LineTypeName = "";
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
        public string LineWeight
        {
            get { return _LineWeight; }
            set
            {
                _LineWeight = value;
                OnPropertyChanged(nameof(LineWeight));
            }
        }
        private string _LineWeight;
        public string LayerColor
        {
            get { return _LayerColor; }
            set
            {
                _LayerColor = value;
                OnPropertyChanged(nameof(LayerColor));
            }
        }
        private string _LayerColor;

        public string LineTypeName
        {
            get { return _LineTypeName; }
            set
            {
                _LineTypeName = value;
                OnPropertyChanged(nameof(LineTypeName));
            }
        }
        private string _LineTypeName;
    }
}
