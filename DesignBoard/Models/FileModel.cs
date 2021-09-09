using DesignBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignBoard.Models
{
    public class FileModel : Notifier
    {

        public FileModel()
        {
            Name = "";
            FullPath = "";
        }

        public string Name
        {
            get { 
                return _Name; 
            }
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string _Name;

        public string FullPath
        {
            get
            {
                return _FullPath;
            }
            set
            {
                _FullPath = value;
                OnPropertyChanged(nameof(FullPath));
            }
        }
        private string _FullPath;
    }
}
