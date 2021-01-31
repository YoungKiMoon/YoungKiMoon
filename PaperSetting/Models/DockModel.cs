using PaperSetting.Commons;
using PaperSetting.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaperSetting.Models
{
    public class DockModel : Notifier, ICloneable
    {
        public DockModel()
        {
            DockPosition = DOCKPOSITION_TYPE.NONE;
            DockPriority = 0;
            HorizontalAlignment = HORIZONTALALIGNMENT_TYPE.LEFT;
            VerticalAlignment = VERTICALALIGNMENT_TYPE.TOP;
        }

        public object Clone()
        {
            DockModel newModel = new DockModel();
            newModel.DockPosition = DockPosition;
            newModel.DockPriority = DockPriority;
            return newModel;
        }

        private DOCKPOSITION_TYPE _DockPosition;
        public DOCKPOSITION_TYPE DockPosition
        {
            get { return _DockPosition; }
            set
            {
                _DockPosition = value;
                OnPropertyChanged(nameof(DockPosition));
            }
        }

        private int _DockPriority;
        public int DockPriority
        {
            get { return _DockPriority; }
            set
            {
                _DockPriority = value;
                OnPropertyChanged(nameof(DockPriority));
            }
        }

        private HORIZONTALALIGNMENT_TYPE _HorizontalAlignment;
        public HORIZONTALALIGNMENT_TYPE HorizontalAlignment
        {
            get { return _HorizontalAlignment; }
            set
            {
                _HorizontalAlignment = value;
                OnPropertyChanged(nameof(HorizontalAlignment));
            }
        }

        private VERTICALALIGNMENT_TYPE _VerticalAlignment;
        public VERTICALALIGNMENT_TYPE VerticalAlignment
        {
            get { return _VerticalAlignment; }
            set
            {
                _VerticalAlignment = value;
                OnPropertyChanged(nameof(VerticalAlignment));
            }
        }
    }
}
