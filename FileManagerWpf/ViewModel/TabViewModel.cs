using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media;
using FileManagerWpf.Extensions;
using FileManagerWpf.Model;
using FileManagerWpf.Services;
using FileManagerWpf.View;

namespace FileManagerWpf.ViewModel
{
    public class TabViewModel : ViewModelBase
    {
        private Brush _borderBrush = Brushes.WhiteSmoke;
        private bool _isActive;
        private Stack<string> _historyBack;
        private Stack<string> _historyForward;
        private string _panelPath;

        private FileManager _fileManager;

        public TabViewModel(string path, bool isActive)
        {
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException(nameof(path));

            _fileManager = new FileManager(path, OnChangesInDirectory);

            CurrentPath = path;
            PanelPath = path;
            _historyBack = new Stack<string>();
            _historyForward = new Stack<string>();

            IsActive = isActive;
            Items = new ObservableCollection<TabItem>();
            SelectedItems = new ObservableCollection<TabItem>();

            UpdateDrivesList();            

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
        public void Rename(TabItem tabItem)
        {
            var dialogView = new InputDialogView("Rename", "Enter new name:", tabItem.Name);

            if (dialogView.ShowDialog() == true)
            {
                var name = dialogView.Answer;

                if (tabItem.Type == EntityType.Dir)
                {
                    _fileManager.RenameFolder(tabItem.Path, name);
                }
                else
                {
                    _fileManager.RenameFile(tabItem.Path, name + tabItem.Ext);
                }
            }
            
        }
        public void Copy(List<TabItem> items, string dest)
        {
            MessageBoxResult res = MessageBox
                .Show($"Copy {items.Count} items to {dest}?", "Copy", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Question);

            if (res == MessageBoxResult.Yes)
            {
                foreach (var item in items)
                {
                    if (item.Type == EntityType.Dir)
                        _fileManager.CopyFolder(item, dest);
                    else
                        _fileManager.CopyFile(item, dest);
                }
            }
        }

        public void Move(List<TabItem> tabItems)
        {
        }

        public void Zip(List<TabItem> items)
        {
            var dialogView = new InputDialogView("Zip", "Enter archive name:", "New archive.zip");
            if (dialogView.ShowDialog() == true)
            {
                var name = dialogView.Answer;
            }
        }
        public void Remove(List<TabItem> tabItems)
        {
            try
            {

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ViewInfo(List<TabItem> selectedItems)
        {
        }

        #region Private Methods

        private void UpdateDrivesList()
        {
            Drives.Clear();
            foreach (var drive in _fileManager.GetDrives())
            {
                Drives.AddOnUI(drive);
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
