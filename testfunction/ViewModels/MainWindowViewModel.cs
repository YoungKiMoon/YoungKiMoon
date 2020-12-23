using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testfunction.Functions;
using testfunction.Models;
using testfunction.Utils;

namespace testfunction.ViewModels
{
    public class MainWindowViewModel : Notifier
    {

        // Binding : InputData
        private Shell_In_Model _InputData;
        public Shell_In_Model InputData
        {
            get { return _InputData; }
            set
            {
                _InputData = value;
                OnPropertyChanged(nameof(InputData));
            }
        }

        // Binding : OutputData
        private ObservableCollection<Shell_Out_Model> _OutputData;
        public ObservableCollection<Shell_Out_Model> OutputData
        {
            get { return _OutputData; }
            set
            {
                _OutputData = value;
                OnPropertyChanged(nameof(OutputData));
            }
        }

        public MainWindowViewModel()
        {
            SetBasicData();
        }


        // 기본 데이터
        public void SetBasicData()
        {
            InputData = new Shell_In_Model();
            InputData.ID = "28000";
            InputData.Height = "18520";
            InputData.PLHeight = "9144";
            InputData.PLWidth = "2400";

            OutputData = new ObservableCollection<Shell_Out_Model>();

            
        }
        public void CalculationShell()
        {
            ShellFunction newFunction = new ShellFunction();
            var calOut = newFunction.Cal(InputData);

            OutputData.Clear();
            foreach(Shell_Out_Model eachModel in calOut)
                OutputData.Add(eachModel);

        }
        public void CalculationShell2()
        {
            ShellFunction newFunction = new ShellFunction();
            var calOut = newFunction.CalNew(InputData);

            OutputData.Clear();
            foreach (Shell_Out_Model eachModel in calOut)
                OutputData.Add(eachModel);

        }
        public void CalculationShell3()
        {
            ShellFunction newFunction = new ShellFunction();
            var calOut = newFunction.CalNewNew(InputData);

            OutputData.Clear();
            foreach (Shell_Out_Model eachModel in calOut)
                OutputData.Add(eachModel);

        }
    }
}
