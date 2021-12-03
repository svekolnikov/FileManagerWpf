using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using FileManagerWpf.Model;
using FileManagerWpf.ViewModel;

namespace FileManagerWpf.Services
{
    public class FileManager
    {

        private readonly FileSystemWatcher _watcher;
        private readonly Action _onChanges;

        public FileManager(Action onChanges)
        {            
            _watcher = new FileSystemWatcher();
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
        }      

        public IEnumerable<DriveItem> GetDrives()
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                yield return new DriveItem
                {
                    Path = drive.RootDirectory.FullName,
                    Name = drive.RootDirectory.Name,
                    Size = drive.TotalSize,
                    TotalFreeSpace = drive.TotalFreeSpace,
                    UsedSpace = drive.TotalSize - drive.TotalFreeSpace
                };
            }
        }
        public IEnumerable<FolderItem> GetSubDirectories(TabViewModel tabViewModel, string path)
        {
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException();

            foreach (var dir in Directory.GetDirectories(path))
            {
                var di = new DirectoryInfo(dir);
                yield return new FolderItem
                {
                    TabViewModel = tabViewModel,
                    Path = dir,
                    Size = 0,
                    Name = di.Name,
                    Type = ItemType.Dir,
                    Date = di.CreationTime.ToString(CultureInfo.CurrentCulture)
                };
            }
        }
        public IEnumerable<FileItem> GetFiles(TabViewModel tabViewModel, string path)
        {
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException();

            foreach (var f in Directory.GetFiles(path))
            {
                var fi = new FileInfo(f);
                yield return new FileItem
                {
                    TabViewModel = tabViewModel,
                    Path = fi.FullName,
                    Size = fi.Length,
                    Name = Path.GetFileNameWithoutExtension(fi.FullName),
                    NameWithExt = fi.Name,
                    Ext = fi.Extension.Replace(".",""),
                    Type = ItemType.File,
                    Date = fi.CreationTime.ToString(CultureInfo.CurrentCulture)
                };
            }
        }
        public void CreateNewFolder(string path, string name)
        {
            var pathNewFolder = Path.Combine(path, name);
            Directory.CreateDirectory(pathNewFolder);
        }
        public void CreateNewFile(string path, string name)
        {
            var pathNewFile = Path.Combine(path, name);
            File.Create(pathNewFile);
        }
        public void RenameFolder(string path, string newName)
        {
            var parent = Directory.GetParent(path).FullName;
            var pathNewFolder = Path.Combine(parent, newName);
            Directory.Move(path, pathNewFolder);
        }
        public void RenameFile(string path, string newName)
        {
            var parent = Directory.GetParent(path).FullName;
            var pathNewFile = Path.Combine(parent, newName);
            File.Move(path, pathNewFile);
        }
        public void CopyFolder(string sourceDirName, string destDirName)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourceDirName);
            var dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, true);
            }

            // Copy subdir recursively
            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDirName, subdir.Name);
                CopyFolder(subdir.FullName, tempPath);
            }
        }
        public void CopyFile(FileItem fileBase, string destFolder)
        {           
            var destFilePath = Path.Combine(destFolder, fileBase.NameWithExt);
            File.Copy(fileBase.Path, destFilePath, true);
        }
        public void MoveFolder(FolderItem folderBase, string destDirectory)
        {
            var destPath = Path.Combine(destDirectory, folderBase.Name);
            Directory.Move(folderBase.Path, destPath);
        }
        public void MoveFile(FileItem fileBase, string destDirectory)
        {
            var destPath = Path.Combine(destDirectory, fileBase.NameWithExt);
            File.Move(fileBase.Path, destPath);
        }
        public void ArchiveFile(FileItem item, string name, string destDir)
        {
            var destPath = Path.Combine(destDir, name + ".zip");

            using var archive = ZipFile.Open(destPath,ZipArchiveMode.Create);
            archive.CreateEntryFromFile(item.Path, item.NameWithExt);
        }
        public void ArchiveFolder(FolderItem item, string name, string destDir)
        {
            var destPath = Path.Combine(destDir, name + ".zip");

            ZipFile.CreateFromDirectory(item.Path,destPath);
        }
        public void RemoveFolder(string path)
        {
            Directory.Delete(path, true);
        }
        public void RemoveFile(string path)
        {
            File.Delete(path);
        }
        public long GetSize(List<IItem> items)
        {
            return items.Sum(item => item.Type == ItemType.Dir
                ? GetDirectorySize(item.Path)
                : item.Size);
        }
        public void StartProcess(string path)
        {
            if (!File.Exists(path)) throw new DirectoryNotFoundException();

            var p = new Process();
            p.StartInfo = new ProcessStartInfo(path)
            {
                UseShellExecute = true
            };
            p.Start();
        }       

        public void SetWatcherPath(string path)
        {
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException();

            _watcher.Path = path;
            _watcher.EnableRaisingEvents = true;
        }
        private void OnChanges(object sender, FileSystemEventArgs e)
        {
            _onChanges.Invoke();
        }
        private long GetDirectorySize(string path)
        {
            var dir = new DirectoryInfo(path);
            return dir.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(file => file.Length);
        }
    }
}
