using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using FileManagerWpf.Commands;
using TabItem = FileManagerWpf.Model.TabItem;

namespace FileManagerWpf.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private int _selectedTab = 0;
        public MainWindowViewModel()
        {
            Tabs = new List<TabViewModel>
            {
                new (@"D:\Test", true),
                new(@"D:\Test\B", false)
            };

            //Commands
            SwitchTabsCommand = new RelayCommand(SwitchTabs);
            DoubleClickCommand = new RelayCommand(DoubleClick);
            SelectionChangedCommand = new RelayCommand(SelectionChanged);

            GoToDriveCommand = new RelayCommand(GoToDrive);
            GoToPathCommand = new RelayCommand(GoToPath);

            GoBackCommand = new RelayCommand(GoBack, param => CanGoBack);
            GoForwardCommand = new RelayCommand(GoForward, param => CanGoForward);
            CreateNewFolderCommand = new RelayCommand(CreateNewFolder);
            CreateNewFileCommand = new RelayCommand(CreateNewFile);

            RenameCommand = new RelayCommand(Rename);
            CopyCommand = new RelayCommand(Copy);
            MoveCommand = new RelayCommand(Move);
            PackToArchiveCommand = new RelayCommand(PackToArchive);
            RemoveCommand = new RelayCommand(Remove);

            QuestionCommand = new RelayCommand(Question);
        }

        public List<TabViewModel> Tabs { get; }

        #region Commands
        public ICommand SwitchTabsCommand { get; set; }
        public void SwitchTabs(object obj)
        {
            Tabs[0].IsActive = !Tabs[0].IsActive;
            Tabs[1].IsActive = !Tabs[1].IsActive;

            _selectedTab = 1 - _selectedTab;
        }
        public ICommand DoubleClickCommand { get; set; }
        public void DoubleClick(object obj)
        {
            var item = (TabItem)obj;
            item?.TabViewModel.Open(item.Path, item.Type);
        }
        public ICommand GoToDriveCommand { get; set; }
        public void GoToDrive(object obj)
        {
            var drivesListName = (string)obj;
            if (drivesListName == "drives1")
            {
                var path = Tabs[0].SelectedDrive.Path;
                Tabs[0].Open(path, Model.EntityType.Dir);
            }
            else if (drivesListName == "drives2")
            {
                var path = Tabs[1].SelectedDrive.Path;
                Tabs[1].Open(path, Model.EntityType.Dir);
            }
        }
        public ICommand GoToPathCommand { get; set; }
        public void GoToPath(object obj)
        {
            var gridName = (string)obj;
            if (gridName == "grid1")
            {
                var path = Tabs[0].Path;
                Tabs[0].Open(path, Model.EntityType.Dir);
            }
            else if (gridName == "grid2")
            {
                var path = Tabs[1].Path;
                Tabs[1].Open(path, Model.EntityType.Dir);
            }
        }
        public ICommand SelectionChangedCommand { get; set; }
        public void SelectionChanged(object obj)
        {
            var selectedItems = ((IList)obj).Cast<TabItem>().ToList();
            if (selectedItems.Count > 0)
            {
                var tab = selectedItems.First().TabViewModel;
                tab.SelectedItems.Clear();
                foreach (var item in selectedItems)
                {
                    tab.SelectedItems.Add(item);
                }
            }
        }
        public ICommand GoBackCommand { get; set; }
        public bool CanGoBack => Tabs[_selectedTab].CanGoBack();
        public bool CanGoForward => Tabs[_selectedTab].CanGoForward();
        public void GoBack(object obj)
        {
            Tabs[_selectedTab].GoBack();
        }
        public ICommand GoForwardCommand { get; set; }
        public void GoForward(object obj)
        {
            Tabs[_selectedTab].GoForward();
        }
        public ICommand CreateNewFolderCommand { get; set; }
        public void CreateNewFolder(object obj)
        {
        }
        public ICommand CreateNewFileCommand { get; set; }
        public void CreateNewFile(object obj)
        {
        }
        public ICommand RenameCommand { get; set; }
        public void Rename(object obj)
        {
        }
        public ICommand CopyCommand { get; set; }
        public void Copy(object obj)
        {
        }
        public ICommand MoveCommand { get; set; }
        public void Move(object obj)
        {
        }
        public ICommand PackToArchiveCommand { get; set; }
        public void PackToArchive(object obj)
        {
        }
        public ICommand RemoveCommand { get; set; }
        public void Remove(object obj)
        {
        }
        public ICommand QuestionCommand { get; set; }
        public void Question(object obj)
        {
        }

        #endregion
    }
}
