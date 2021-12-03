using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media;
using FileManagerWpf.Extensions;
using FileManagerWpf.Model;
using FileManagerWpf.View;
using System.Linq;
using FileManagerWpf.Services;

namespace FileManagerWpf.ViewModel
{
    public class TabViewModel : ViewModelBase
    {
        private Brush _borderBrush = Brushes.WhiteSmoke;
        private bool _isActive;
        private Stack<string> _historyBack;
        private Stack<string> _historyForward;
        private string _panelPath;

        private readonly int _tabIndex;
        private FileManager _fileManager;

        public TabViewModel( bool isActive, int tabIndex)
        {
            _fileManager = new FileManager(OnChangesInDirectory);

            UpdateDrivesList();

            var defaultPath = Drives[0].Path;
            _tabIndex = tabIndex;
            _fileManager.SetWatcherPath(defaultPath);            

            CurrentPath = defaultPath;
            PanelPath = defaultPath;
            _historyBack = new Stack<string>();
            _historyForward = new Stack<string>();

            IsActive = isActive;
            
            SetDirectory(defaultPath);
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
        public ObservableCollection<DriveItem> Drives { get; set; } = new ObservableCollection<DriveItem>();
        public string PanelPath 
        {
            get => _panelPath;
            set
            {
                _panelPath = value;
                OnPropertyChanged(nameof(PanelPath));
            }
        }
        public string CurrentPath { get; private set; }
        public Brush BorderBrush
        {
            get => _borderBrush;
            set
            {
                _borderBrush = value;
                OnPropertyChanged(nameof(BorderBrush));
            }
        }
        public ObservableCollection<IItem> Items { get; set; } = new ObservableCollection<IItem>();
        public ObservableCollection<IItem> SelectedItems { get; set; } = new ObservableCollection<IItem>();
        public DriveItem SelectedDriveBase { get; set; }

        #endregion

        public void Open(string path, ItemType type)
        {
            if (type == ItemType.Dir)
            {
                if (!Directory.Exists(path))
                {
                    //TODO Notification Directory does not exist
                    return;
                }
                _historyForward.Clear();
                _historyBack.Push(CurrentPath);                
                               
                SetDirectory(path);                                
            }
            else
            {
                _fileManager.StartProcess(path);
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
                _historyForward.Push(CurrentPath);

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
                _historyBack.Push(CurrentPath);

                SetDirectory(path);
            }
        }
        public void CreateNewFolder()
        {
            var dialogView = new InputDialogView("New Folder", "Enter folder name:", "New Folder");
            if (dialogView.ShowDialog() == true)
                _fileManager.CreateNewFolder(CurrentPath, dialogView.Answer);
        }        
        public void CreateNewFile()
        {            
            var dialogView = new InputDialogView("New File", "Enter file name:", "New File.txt");
            if (dialogView.ShowDialog() == true)
                _fileManager.CreateNewFile(CurrentPath, dialogView.Answer);
        }
        public bool CanRename => SelectedItems.Count == 1;
        public void Rename()
        {
            var selectedItem = SelectedItems[0];

            if (selectedItem.Type == ItemType.Dir)
            {
                var dialogView = new InputDialogView("Rename folder", "Enter new folder name:", selectedItem.Name);
                if (dialogView.ShowDialog() == true)
                {
                    _fileManager.RenameFolder(selectedItem.Path, dialogView.Answer);
                }
            }
            else
            {
                var dialogView = new InputDialogView("Rename file", "Enter new file name:", ((FileItem)selectedItem).NameWithExt);
                if (dialogView.ShowDialog() == true)
                {
                    _fileManager.RenameFile(selectedItem.Path, dialogView.Answer);
                }

            }
        }
        public bool CanCopy => SelectedItems.Count > 0;
        public void Copy(List<IItem> tabItems, string dest)
        {
            MessageBoxResult res = MessageBox
                .Show($"Copy {tabItems.Count} items to {dest}?", "Copy", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Question);

            if (res == MessageBoxResult.Yes)
            {
                foreach (var item in tabItems)
                {
                    if (item.Type == ItemType.Dir)
                        _fileManager.CopyFolder(item.Path, dest);
                    else
                        _fileManager.CopyFile((FileItem)item, dest);
                }
            }
        }
        public bool CanMove => SelectedItems.Count > 0;
        public void Move(List<IItem> tabItems, string dest)
        {
            MessageBoxResult res = MessageBox
                .Show($"Move {tabItems.Count} items to {dest}?", "Move",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (res == MessageBoxResult.Yes)
            {
                foreach (var item in tabItems)
                {
                    if (item.Type == ItemType.Dir)
                        _fileManager.MoveFolder((FolderItem)item, dest);
                    else
                        _fileManager.MoveFile((FileItem)item, dest);
                }
            }

        }
        public bool CanZip => SelectedItems.Count == 1;
        public void Archive()
        {
            var dialogView = new InputDialogView("ArchiveFile", "Enter archive name:", "New archive");
            if (dialogView.ShowDialog() == true)
            {
                var name = dialogView.Answer;
                var item = SelectedItems[0];
                if (item.Type == ItemType.Dir)
                    _fileManager.ArchiveFolder((FolderItem)item, name, CurrentPath);
                else
                    _fileManager.ArchiveFile((FileItem)item, name, CurrentPath);
            }
        }
        public bool CanRemove => SelectedItems.Count > 0;
        public void Remove()
        {
            var selectedItems = SelectedItems.ToList();
            MessageBoxResult res = MessageBox
                .Show($"Remove {selectedItems.Count} items?", "Remove",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            try
            {
                if (res == MessageBoxResult.Yes)
                {
                    foreach (var item in selectedItems)
                    {
                        if (item.Type == ItemType.Dir)
                            _fileManager.RemoveFolder(item.Path);
                        else
                            _fileManager.RemoveFile(item.Path);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool CanViewInfo => SelectedItems.Count > 0;
        public void ViewInfo()
        {
            var selectedItems = SelectedItems.ToList();
            var size = _fileManager.GetSize(selectedItems);

            MessageBox.Show($"Size of {selectedItems.Count} items: {size} bytes", "Info",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        #region Private Methods
        private void UpdateDrivesList()
        {
            Drives.Clear();
            foreach (var drive in _fileManager.GetDrives())
            {
                Drives.Add(drive);
            }
        }        
        private void SetDirectory(string path)
        {
            Items.ClearOnUI();

            //directories
            foreach (var item in _fileManager.GetSubDirectories(this, path))
            {
                Items.AddOnUI(item);
            }
            //files 
            foreach (var item in _fileManager.GetFiles(this, path))
            {
                Items.AddOnUI(item);
            }

            PanelPath = path;
            SelectedItems.Clear();
            CurrentPath = path;
            _fileManager.SetWatcherPath(path);
        }        
        
        #endregion

        private void OnChangesInDirectory()
        {
            SetDirectory(CurrentPath);
        }
        
    }
}
