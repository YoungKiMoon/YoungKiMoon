using DrawWork.Commons;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DrawWork
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            SetDrawSettingInformation();
        }

        private void SetDrawSettingInformation()
        {
            SingletonData.GAArea.Dimension.Length = "5000";
            SingletonData.GAArea.NozzleLeader.Length = "5000";
            SingletonData.GAArea.ShellCourse.Length = "5000";
        }
    }
}
