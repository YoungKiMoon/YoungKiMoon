using DrawSettingLib.SettingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawSettingLib.SettingServices
{
    public class PaperAreaService
    {

        public ScaleCalService scaleCal;
        public PaperAreaService()
        {
            scaleCal = new ScaleCalService();
        }

        public List<PaperAreaModel> GetPaperAreaData()
        {
            List<PaperAreaModel> newAreaList = new List<PaperAreaModel>();

            PaperAreaModel GAView = new PaperAreaModel();
            GAView.Name = "ga";
            GAView.Location.X = 334;
            GAView.Location.Y = 363;
            GAView.Size.X = 600;
            GAView.Size.Y = 400;
            newAreaList.Add(GAView);

            PaperAreaModel Orientation = new PaperAreaModel();
            Orientation.Name = "orientation";
            Orientation.Location.X = 280;
            Orientation.Location.Y = 297;
            Orientation.Size.X = 500;
            Orientation.Size.Y = 570;
            newAreaList.Add(Orientation);

            return newAreaList;
        }

        public double UpdateScaleValue(string selType, List<PaperAreaModel> selList,GAAreaModel selGAArea,double ID, double refX, double refY)
        {
            double returnScale = 1;

            string selTypeName = selType.ToLower();

            // Paper
            foreach (PaperAreaModel eachArea in selList)
            {
                switch (eachArea.Name)
                {
                    case "ga":
                        eachArea.ScaleValue= scaleCal.GetScaleValue(eachArea.Size.X- selGAArea.Dimension.AreaSize.Width*2 - selGAArea.NozzleLeader.AreaSize.Width * 2 - selGAArea.ShellCourse.AreaSize.Width , 
                            eachArea.Size.Y - selGAArea.Dimension.AreaSize.Height * 2 - selGAArea.NozzleLeader.AreaSize.Height * 2, 
                            selGAArea.MainAssembly.BoxSize.Width, selGAArea.MainAssembly.BoxSize.Height);

                        goto case "setScaleValue";
                        
                    case "orientation":
                        // 500 * 0.4
                        double orientationDim = 170;
                        eachArea.ScaleValue = scaleCal.GetScaleValue(eachArea.Size.X- orientationDim, eachArea.Size.Y- orientationDim, ID, ID);
                        // Center
                        eachArea.ModelLocation.X = refX ;
                        eachArea.ModelLocation.Y = refY ;
                        goto case "setScaleValue";

                    case "setScaleValue":
                        if (eachArea.Name == selTypeName)
                            returnScale = eachArea.ScaleValue;
                        break;
                }
            }

            return returnScale;
        }

        public double GetPaperScaleValue(string selType,List<PaperAreaModel> selList)
        {
            double returnScale = 1;

            string selTypeName = selType.ToLower();

            // Paper
            foreach (PaperAreaModel eachArea in selList)
            {
                if (eachArea.Name == selTypeName)
                {
                    returnScale = eachArea.ScaleValue;
                    break;
                }
            }

            return returnScale;
        }
    }
}
