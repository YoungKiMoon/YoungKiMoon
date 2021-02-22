using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DrawWork.Utils;

namespace DrawWork.AssemblyModels
{
    public class AssemblyModel : Notifier
    {
        public AssemblyModel()
        {
            ShellInput = new ObservableCollection<ShellInputModel>();
            ShellOutput = new ObservableCollection<ShellOutputModel>();
        }

        private ObservableCollection<ShellInputModel> _ShellInput;
        public ObservableCollection<ShellInputModel> ShellInput
        {
            get { return _ShellInput; }
            set
            {
                _ShellInput = value;
                OnPropertyChanged(nameof(ShellInput));
            }
        }

        private ObservableCollection<ShellOutputModel> _ShellOutput;
        public ObservableCollection<ShellOutputModel> ShellOutput
        {
            get { return _ShellOutput; }
            set
            {
                _ShellOutput = value;
                OnPropertyChanged(nameof(ShellOutput));
            }
        }

        #region Sample Data
        public void CreateSampleAssembly()
        {
            // Input Data
            ShellInputModel newInput = new ShellInputModel();
            newInput.No = "1";
            newInput.ID = "48000";
            newInput.Height = "16400";
            newInput.PLWidth = "10000";
            newInput.PLHeight = "2400";

            ShellInput.Add(newInput);


            // Output Data
            List<string[]> newCourseList = new List<string[]>();
            string[] newCourseStr1 = new string[7] { "1", "1st", "19", "0", "2400", "9432", "16" };
            string[] newCourseStr2 = new string[7] { "2", "2nd", "17", "3144", "2400", "9432", "16" };
            string[] newCourseStr3 = new string[7] { "3", "3rd", "15", "6288", "2400", "9432", "16" };
            string[] newCourseStr4 = new string[7] { "4", "4th", "12", "0", "2400", "9432", "16" };
            string[] newCourseStr5 = new string[7] { "5", "5th", "10", "3144", "2400", "9432", "16" };
            string[] newCourseStr6 = new string[7] { "6", "6th", "8", "6288", "2400", "9432", "16" };
            string[] newCourseStr7 = new string[7] { "7", "7th", "8", "0", "2000", "9432", "16" };

            newCourseList.Add(newCourseStr1);
            newCourseList.Add(newCourseStr2);
            newCourseList.Add(newCourseStr3);
            newCourseList.Add(newCourseStr4);
            newCourseList.Add(newCourseStr5);
            newCourseList.Add(newCourseStr6);
            newCourseList.Add(newCourseStr7);

            foreach (string[] eachStr in newCourseList)
            {
                ShellOutputModel newCourse = new ShellOutputModel();
                newCourse.No = eachStr[0];
                newCourse.Course = eachStr[1];
                newCourse.Thickness = eachStr[2];
                newCourse.StartPoint = eachStr[3];
                newCourse.OnePLHeight = eachStr[4];
                newCourse.OnePLWidth = eachStr[5];
                newCourse.Count = eachStr[6];

                ShellOutput.Add(newCourse);
            }

        }
        #endregion
    }
}
