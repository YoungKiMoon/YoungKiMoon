using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfCadCon.CustomControl
{
    public class BaseListView : ListView
    {
        public Type DataType { get; set; }

        public List<string> DataHeader { get; set; }

        public BaseListView()
        {
            DataHeader = new List<string>();
            DataHeader.Add("열1");
            DataHeader.Add("열2");
            DataHeader.Add("열3");
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.Name == "ItemsSource"
                && e.OldValue != e.NewValue
                && e.NewValue != null
                && this.DataType != null)
            {
                CreateColumns(this);
            }
        }

        private static void CreateColumns(BaseListView lv)
        {
            var gridView = new GridView { AllowsColumnReorder = true };

            var properties = lv.DataType.GetProperties();
            int headerIndex = 0;
            foreach (var pi in properties)
            {
                var browsableAttribute = pi.GetCustomAttributes(true).FirstOrDefault(a => a is BrowsableAttribute) as BrowsableAttribute;
                if (browsableAttribute != null && !browsableAttribute.Browsable)
                {
                    continue;
                }

                var binding = new Binding { Path = new PropertyPath(pi.Name), Mode = BindingMode.OneWay };
                //var gridViewColumn = new GridViewColumn() { Header = pi.Name, DisplayMemberBinding = binding };
                var gridViewColumn = new GridViewColumn() { Header = lv.DataHeader[headerIndex], DisplayMemberBinding = binding };
                gridView.Columns.Add(gridViewColumn);
                headerIndex++;
            }

            lv.View = gridView;
        }
    }
}
