using FileManagerWpf.Model;
using FileManagerWpf.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace FileManagerWpf.Services
{
    public class FileManager
    {

        private readonly FileSystemWatcher _watcher;
        private readonly Action _onChanges;

        public FileManager(string path, Action onChanges)
        {
            if(!Directory.Exists(path)) throw new DirectoryNotFoundException();
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

            _watcher.Changed += OnChanges;
            _watcher.Created += OnChanges;
            _watcher.Deleted += OnChanges;
            _watcher.Renamed += OnChanges;
            _onChanges = onChanges;
            _watcher.EnableRaisingEvents = true;            
        }      

        public IEnumerable<Drive> GetDrives()
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
        public IEnumerable<TabItem> GetSubDirectories(TabViewModel tabViewModel, string path)
        {
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException();

            foreach (var dir in Directory.GetDirectories(path))
            {
                var di = new DirectoryInfo(dir);
                yield return new TabItem
                {
                    TabViewModel = tabViewModel,
                    Path = dir,
                    Size = "",
                    Name = di.Name,
                    Ext = "<DIR>",
                    Type = EntityType.Dir,
                    Date = di.CreationTime.ToString(CultureInfo.CurrentCulture)
                };
            }
        }
        public IEnumerable<TabItem> GetFiles(TabViewModel tabViewModel, string path)
        {
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException();

            foreach (var f in Directory.GetFiles(path))
            {
                var fi = new FileInfo(f);
                yield return new TabItem
                {
                    TabViewModel = tabViewModel,
                    Path = fi.FullName,
                    Size = fi.Length.ToString(),
                    Name = Path.GetFileNameWithoutExtension(fi.FullName),
                    Ext = fi.Extension,
                    Type = EntityType.File,
                    Date = fi.CreationTime.ToString(CultureInfo.CurrentCulture)
                };
            }
        }
        public void CreateNewFolder(string path, string name)
        {
            try
            {
                var pathNewFolder = Path.Combine(path, name);
                Directory.CreateDirectory(pathNewFolder);
            }
            catch (Exception)
            {
                throw;
            }            
        }
        public void CreateNewFile(string path, string name)
        {
            try
            {
                var pathNewFile = Path.Combine(path, name);
                File.Create(pathNewFile);
            }
            catch (Exception)
            {
                throw;
            }            
        }
        public void RenameFolder(string path, string newName)
        {
            var parent = Directory.GetParent(path).FullName;
            var pathNewFolder = Path.Combine(parent, newName);
            MoveFolder(path, pathNewFolder);
        }
        public void RenameFile(string path, string newName)
        {
            var parent = Directory.GetParent(path).FullName;
            var pathNewFile = Path.Combine(parent, newName);
            MoveFile(path, pathNewFile);
        }
        public void CopyFolder(TabItem path, string dest)
        {
            //Directory.Copy(path, dest);
        }
        public void CopyFile(TabItem file, string destFolder)
        {           
            var destFilePath = Path.Combine(destFolder, file.Name);
            destFilePath += file.Ext;
            File.Copy(file.Path, destFilePath, true);
        }
        public void MoveFolder(string path, string dest)
        {
            Directory.Move(path, dest);
        }
        public void MoveFile(string path, string dest)
        {
            File.Move(path, dest);
        }
        public void StartProcess(string path)
        {
            if (!File.Exists(path)) throw new DirectoryNotFoundException();

            try
            {
                var p = new Process();
                p.StartInfo = new ProcessStartInfo(path)
                {
                    UseShellExecute = true
                };
                p.Start();
            }
            catch (Exception)
            {
                throw;
            }            
        }
        public void SetWatcherPath(string path)
        {
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException();

            _watcher.Path = path;
        }
        private void OnChanges(object sender, FileSystemEventArgs e)
        {
            _onChanges.Invoke();
        }
        
    }
}
