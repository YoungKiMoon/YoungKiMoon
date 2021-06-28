using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignBoard.Models
{
    public class ImageModel
    {
        public ImageModel()
        {
            ImagePath = "";
            ImageName = "";
        }
        public ImageModel(string selPath, string selName)
        {
            ImagePath = selPath;
            ImageName = selName;
        }

        public string ImagePath { get; set; }
        //private string _ImagePath;

        public string ImageName { get; set; }
        //private string _ImageName;
    }
}
