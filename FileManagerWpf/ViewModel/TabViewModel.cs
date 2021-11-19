using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using FileManagerWpf.Model;
using TabItem = FileManagerWpf.Model.TabItem;

namespace FileManagerWpf.ViewModel
{
    public class TabViewModel
    {
        public ObservableCollection<Drive> Drives { get; set; }
        public string Navigation { get; set; }
        public ObservableCollection<TabItem> Items { get; set; }
        public int SelectedItemsCount { get; set; }
        public List<System.Windows.Controls.TabItem> SelectedItems { get; set; }
    }
}
