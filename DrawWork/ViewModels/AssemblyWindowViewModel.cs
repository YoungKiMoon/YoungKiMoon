using DrawWork.AssemblyModels;
using DrawWork.Models;
using DrawWork.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.ViewModels
{
    public class AssemblyWindowViewModel : Notifier
    {
        public AssemblyModel TankData;

        private ObservableCollection<TreeNodeModel> _TreeData;
        public ObservableCollection<TreeNodeModel> TreeData
        {
            get { return _TreeData; }
            set
            {
                _TreeData = value;
                OnPropertyChanged(nameof(TreeData));
            }
        }

        private ObservableCollection<ShellInputModel> _InputData;
        public ObservableCollection<ShellInputModel> InputData
        {
            get { return _InputData; }
            set
            {
                _InputData = value;
                OnPropertyChanged(nameof(InputData));
            }
        }

        private ObservableCollection<ShellOutputModel> _OutputData;
        public ObservableCollection<ShellOutputModel> OutputData
        {
            get { return _OutputData; }
            set
            {
                _OutputData = value;
                OnPropertyChanged(nameof(OutputData));
            }
        }

        public AssemblyWindowViewModel()
        {
            TankData = new AssemblyModel();
            CreateAssembly(TankData);
        }

        public void CreateAssembly(AssemblyModel selModel)
        {
            TankData = selModel;
            TreeData = new ObservableCollection<TreeNodeModel>();
            InputData = TankData.ShellInput;
            OutputData = TankData.ShellOutput;

            TreeSample();
        }

        public void TreeSample()
        {
            #region Tree Data
            TreeNodeModel sampleRoot = new TreeNodeModel();
            sampleRoot.Name = "TANK Model (Type:CRT)";

            ObservableCollection<TreeNodeModel> sampleChild1 = new ObservableCollection<TreeNodeModel>();
            sampleChild1.Add(new TreeNodeModel() { Name = "Vent with Cage" });
            sampleChild1.Add(new TreeNodeModel() { Name = "Roof Handrails" });
            sampleChild1.Add(new TreeNodeModel() { Name = "000 ..." });
            sampleChild1.Add(new TreeNodeModel() { Name = "000 ..." });

            ObservableCollection<TreeNodeModel> sampleChild2 = new ObservableCollection<TreeNodeModel>();
            sampleChild2.Add(new TreeNodeModel() { Name = "Vent with Cage" });
            sampleChild2.Add(new TreeNodeModel() { Name = "Roof Handrails" });
            sampleChild2.Add(new TreeNodeModel() { Name = "000 ..." });
            sampleChild2.Add(new TreeNodeModel() { Name = "000 ..." });

            ObservableCollection<TreeNodeModel> sampleChild3 = new ObservableCollection<TreeNodeModel>();
            sampleChild3.Add(new TreeNodeModel() { Name = "Vent with Cage" });
            sampleChild3.Add(new TreeNodeModel() { Name = "Roof Handrails" });
            sampleChild3.Add(new TreeNodeModel() { Name = "000 ..." });
            sampleChild3.Add(new TreeNodeModel() { Name = "000 ..." });

            TreeNodeModel samplePNode1 = new TreeNodeModel();
            samplePNode1.Name = "ROOF";
            samplePNode1.Children = sampleChild1;

            TreeNodeModel samplePNode2 = new TreeNodeModel();
            samplePNode2.Name = "SHELL";
            samplePNode2.Children = sampleChild2;

            TreeNodeModel samplePNode3 = new TreeNodeModel();
            samplePNode3.Name = "BOTTOM";
            samplePNode3.Children = sampleChild3;


            sampleRoot.Children.Add(samplePNode1);
            sampleRoot.Children.Add(samplePNode2);
            sampleRoot.Children.Add(samplePNode3);

            TreeData.Add(sampleRoot);
            #endregion
        }
    }
}
