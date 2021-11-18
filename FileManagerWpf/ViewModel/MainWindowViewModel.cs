using System.Collections.ObjectModel;
using FileManagerWpf.Model;

namespace FileManagerWpf.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            LeftItems = new ObservableCollection<ListItem>
            {
                new ListItem {Name = "Папка", Ext = "", Size = "<DIR>", Date = "04/10/2021 12:15"},
                new ListItem {Name = "Папка", Ext = "", Size = "<DIR>", Date = "05/10/2021 12:15"},
                new ListItem{Name = "Папка", Ext = "", Size = "<DIR>", Date = "06/10/2021 12:15"},
                new ListItem{Name = "Файл", Ext = "iso", Size = "24242", Date = "07/10/2021 12:15"},
                new ListItem{Name = "Файл", Ext = "exe", Size = "19876", Date = "08/10/2021 12:15"}
            };
        }

        public ObservableCollection<ListItem> LeftItems { get; set; }
        public ObservableCollection<ListItem> RightItems { get; set; }
    }
}
