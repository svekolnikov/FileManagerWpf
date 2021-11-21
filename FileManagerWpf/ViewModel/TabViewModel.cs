using System.Collections.Generic;
using System.Collections.ObjectModel;
using FileManagerWpf.Model;

namespace FileManagerWpf.ViewModel
{
    public class TabViewModel : ViewModelBase
    {
        private string _path = @"C:\\";
        private int _selected;

        public ObservableCollection<Drive> Drives { get; set; } = new ObservableCollection<Drive>();
        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                OnPropertyChanged(nameof(Path));
            }
        }
        public ObservableCollection<TabItem> Items { get; set; } = new ObservableCollection<TabItem>();
        public List<TabItem> SelectedItems { get; set; } = new List<TabItem>();
        public int SelectedCount
        {
            get => _selected;
            set
            {
                _selected = value;
                OnPropertyChanged(nameof(SelectedCount));
            }
        }
    }
}
