using FileManagerWpf.Model;
using FileManagerWpf.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using FileManagerWpf.Extensions;

namespace FileManagerWpf.Services
{
    public class FileManager
    {
        private readonly TabViewModel _tabViewModel;
        private readonly FileSystemWatcher _watcher;
        public FileManager(TabViewModel tabViewModel)
        {
            
            _tabViewModel = tabViewModel;
            UpdateDriveList(_tabViewModel.Drives);
            
            _watcher = new FileSystemWatcher(_tabViewModel.Path);
        }

        public void GoToPath(string path)
        {
            if (Directory.Exists(path))
            {
                AddWatcher(path);
                UpdateItemsList(_tabViewModel.Items, path);
            }
        }

        public void Open(TabItem item)
        {
            if (item.Type == TableEntityType.Dir)
            {
                GoToPath(item.Path);
            }
            else
            {
                var p = new Process();
                p.StartInfo = new ProcessStartInfo(item.Path)
                {
                    UseShellExecute = true
                };
                p.Start();
            }
            
        }

        private void UpdateDriveList(ObservableCollection<Drive> drivesOnView)
        {
            var systemDrives = DriveInfo.GetDrives();
            foreach (var drive in systemDrives)
            {
                drivesOnView.Add(new Drive
                {
                    Name = drive.Name,
                    TotalSize = drive.TotalSize,
                    TotalFreeSpace = drive.TotalFreeSpace,
                    UsedSpace = drive.TotalSize - drive.TotalFreeSpace
                });
            }
        }

        private void UpdateItemsList(ObservableCollection<TabItem> itemsCollection, string path)
        {
            itemsCollection.ClearOnUI();

            //directories
            foreach (var item in GetSubDirectories(path))
            {
                _tabViewModel.Items.AddOnUI(item);
            }
            //files 
            foreach (var item in GetFiles(path))
            {
                _tabViewModel.Items.AddOnUI(item);
            }
        }

        private IEnumerable<TabItem> GetSubDirectories(string path)
        {
            var subDirs = Directory.GetDirectories(path);

            //subdirectories
            foreach (string dir in subDirs)
            {
                var di = new DirectoryInfo(dir);
                yield return new TabItem
                {
                    FileManager = this,
                    Path = dir,
                    Size = "",
                    Name = di.Name,
                    Ext = "<DIR>",
                    Type = TableEntityType.Dir,
                    Date = di.CreationTime.ToString(CultureInfo.CurrentCulture)
                };
            }
        }

        private IEnumerable<TabItem> GetFiles(string path)
        {
            var files = Directory.GetFiles(path);
            foreach (string f in files)
            {
                FileInfo fi = new FileInfo(f);
                yield return new TabItem
                {
                    FileManager = this,
                    Path = fi.FullName,
                    Size = fi.Length.ToString(),
                    Name = Path.GetFileNameWithoutExtension(fi.FullName),
                    Ext = fi.Extension.Replace(".", ""),
                    Type = TableEntityType.File,
                    Date = fi.CreationTime.ToString(CultureInfo.CurrentCulture)
                };
            }
        }

        private void AddWatcher(string path)
        {
            _watcher.Path = path;
            _watcher.NotifyFilter = NotifyFilters.Attributes
                                   | NotifyFilters.CreationTime
                                   | NotifyFilters.DirectoryName
                                   | NotifyFilters.FileName
                                   | NotifyFilters.LastAccess
                                   | NotifyFilters.LastWrite
                                   | NotifyFilters.Security
                                   | NotifyFilters.Size;
            
            _watcher.Changed += OnChanged;
            _watcher.Created += OnChanged;
            _watcher.Deleted += OnChanged;
            _watcher.Renamed += OnChanged;
            _watcher.Filter = "*.*";
            _watcher.EnableRaisingEvents = true;
        }
        
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            UpdateItemsList(_tabViewModel.Items, _watcher.Path);
        }
    }
}
