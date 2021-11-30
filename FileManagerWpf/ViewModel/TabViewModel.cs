using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Media;
using FileManagerWpf.Extensions;
using FileManagerWpf.Model;

namespace FileManagerWpf.ViewModel
{
    public class TabViewModel : ViewModelBase
    {
        private string _currentPath;
        private Brush _borderBrush = Brushes.WhiteSmoke;
        private bool _isActive;
        private readonly FileSystemWatcher _watcher;
        private Stack<string> _historyBack;
        private Stack<string> _historyForward;
        private string path;

        public TabViewModel(string path, bool isActive)
        {
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException(nameof(path));

            _currentPath = path;
            Path = path;
            _historyBack = new Stack<string>();
            _historyForward = new Stack<string>();

            IsActive = isActive;
            Items = new ObservableCollection<TabItem>();
            SelectedItems = new ObservableCollection<TabItem>();

            UpdateDrivesList();

            _watcher = new FileSystemWatcher(path);
            _watcher.Filter = "*.*";
            _watcher.NotifyFilter = NotifyFilters.Attributes
                                    | NotifyFilters.CreationTime
                                    | NotifyFilters.DirectoryName
                                    | NotifyFilters.FileName
                                    | NotifyFilters.LastAccess
                                    | NotifyFilters.LastWrite
                                    | NotifyFilters.Security
                                    | NotifyFilters.Size;

            _watcher.Changed += OnChangedItem;
            _watcher.Created += OnChangedItem;
            _watcher.Deleted += OnChangedItem;
            _watcher.Renamed += OnChangedItem;
            _watcher.EnableRaisingEvents = true;

            SetDirectory(path);
        }

        #region Properties
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                BorderBrush = value ? Brushes.ForestGreen : Brushes.WhiteSmoke;
            }
        }
        public ObservableCollection<Drive> Drives { get; set; } = new ObservableCollection<Drive>();
        public string Path 
        {
            get => path;
            set
            {
                path = value;
                OnPropertyChanged(nameof(Path));
            }
        }
        public Brush BorderBrush
        {
            get => _borderBrush;
            set
            {
                _borderBrush = value;
                OnPropertyChanged(nameof(BorderBrush));
            }
        }
        public ObservableCollection<TabItem> Items { get; set; }
        public ObservableCollection<TabItem> SelectedItems { get; set; }
        public Drive SelectedDrive { get; set; }

        #endregion

        public void Open(string path, EntityType type)
        {
            if (type == EntityType.Dir)
            {
                if (!Directory.Exists(path))
                {
                    //TODO Notification Directory does not exist
                    return;
                }
                _historyForward.Clear();
                _historyBack.Push(_currentPath);                
                               
                SetDirectory(path);                                
            }
            else
            {
                StartProcess(path);
            }

        }
        public bool CanGoBack()
        {
            _historyBack.TryPeek(out string p);
            return p != null;
        }
        public void GoBack()
        {
            if (CanGoBack())
            {
                var path = _historyBack.Pop();
                _historyForward.Push(_currentPath);

                SetDirectory(path);
            }
        }
        public bool CanGoForward()
        {
            _historyForward.TryPeek(out string p);
            return p != null;
        }
        public void GoForward()
        {
            if (CanGoForward())
            {
                var path = _historyForward.Pop();
                _historyBack.Push(_currentPath);

                SetDirectory(path);
            }
        }

        #region Private Methods

        private void UpdateDrivesList()
        {
            Drives.Clear();
            foreach (var drive in GetDrives())
            {
                Drives.AddOnUI(drive);
            }
        }
        private IEnumerable<Drive> GetDrives()
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                yield return new Drive
                {
                    Path = drive.RootDirectory.FullName,
                    Name = drive.RootDirectory.Name,
                    TotalSize = drive.TotalSize,
                    TotalFreeSpace = drive.TotalFreeSpace,
                    UsedSpace = drive.TotalSize - drive.TotalFreeSpace
                };
            }
        }
        private void SetDirectory(string path)
        {
            Items.ClearOnUI();

            //directories
            foreach (var item in GetSubDirectories(path))
            {
                Items.AddOnUI(item);
            }
            //files 
            foreach (var item in GetFiles(path))
            {
                Items.AddOnUI(item);
            }

            Path = path;
            SelectedItems.Clear();
            _currentPath = path;
            _watcher.Path = path;
        }
        private IEnumerable<TabItem> GetSubDirectories(string path)
        {
            //subdirectories
            foreach (var dir in Directory.GetDirectories(path))
            {
                var di = new DirectoryInfo(dir);
                yield return new TabItem
                {
                    TabViewModel = this,
                    Path = dir,
                    Size = "",
                    Name = di.Name,
                    Ext = "<DIR>",
                    Type = EntityType.Dir,
                    Date = di.CreationTime.ToString(CultureInfo.CurrentCulture)
                };
            }
        }
        private IEnumerable<TabItem> GetFiles(string path)
        {
            foreach (var f in Directory.GetFiles(path))
            {
                var fi = new FileInfo(f);
                yield return new TabItem
                {
                    TabViewModel = this,
                    Path = fi.FullName,
                    Size = fi.Length.ToString(),
                    Name = System.IO.Path.GetFileNameWithoutExtension(fi.FullName),
                    Ext = fi.Extension.Replace(".", ""),
                    Type = EntityType.File,
                    Date = fi.CreationTime.ToString(CultureInfo.CurrentCulture)
                };
            }
        }
        private void StartProcess(string path)
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(path)
            {
                UseShellExecute = true
            };
            p.Start();
        }

        #endregion

        private void OnChangedItem(object sender, FileSystemEventArgs e)
        {
            SetDirectory(_currentPath);
        }
    }
}
