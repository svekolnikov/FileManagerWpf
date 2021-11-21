using FileManagerWpf.Model;
using FileManagerWpf.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace FileManagerWpf.Services
{
    public class FileManager
    {
        private readonly TabViewModel _tabViewModel;
        public FileManager(TabViewModel tabViewModel)
        {
            _tabViewModel = tabViewModel;
            UpdateDriveList(_tabViewModel.Drives);
        }

        public void GoToPath(string path)
        {
            if (Directory.Exists(path))
            {
                _tabViewModel.Path = path;
                _tabViewModel.Items.Clear();

                //directories
                foreach (var item in GetSubDirectories(path))
                {
                    _tabViewModel.Items.Add(item);
                }
                //files 
                foreach (var item in GetFiles(path))
                {
                    _tabViewModel.Items.Add(item);
                }
                
            }
        }

        public void Open(TabItem item)
        {
            if (item.Type == TableEntityType.DIR)
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
                    Type = TableEntityType.DIR,
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
    }
}
