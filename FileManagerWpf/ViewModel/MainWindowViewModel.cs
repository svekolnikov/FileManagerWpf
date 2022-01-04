using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FileManagerWpf.Commands;
using FileManagerWpf.Model;
using FileManagerWpf.Services;

namespace FileManagerWpf.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {        
        private int _selectedTab = 0; // Selected Tab index from Tabs list
        public MainWindowViewModel()
        {
            Tabs = new List<TabViewModel>
            {
                new (true,  0),
                new (false, 1)
            };

            //Commands
            SwitchTabsCommand = new RelayCommand(SwitchTabs);
            DoubleClickCommand = new RelayCommand(DoubleClick);
            SelectionChangedCommand = new RelayCommand(SelectionChanged);

            GoToDriveCommand = new RelayCommand(GoToDrive);
            GoToPathCommand = new RelayCommand(GoToPath);

            GoBackCommand = new RelayCommand(GoBack, _ => CanGoBack);
            GoForwardCommand = new RelayCommand(GoForward, _ => CanGoForward);
            CreateNewFolderCommand = new RelayCommand(CreateNewFolder);
            CreateNewFileCommand = new RelayCommand(CreateNewFile);

            RenameCommand = new RelayCommand(Rename, _ => CanRename);
            ViewInfoCommand = new RelayCommand(ViewInfo, _ => CanViewInfo);
            CopyCommand = new RelayCommand(Copy, _ => CanCopy);
            MoveCommand = new RelayCommand(Move, _ => CanMove);
            ZipCommand = new RelayCommand(Zip, _ => CanZip);
            RemoveCommand = new RelayCommand(Remove, _ => CanRemove);
            BuildReportCommand = new RelayCommand(BuildReport, _ => CanBuildReport);
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
            var item = (IItem)obj;
            item?.TabViewModel.Open(item.Path, item.Type);
        }

        public ICommand GoToDriveCommand { get; set; }
        public void GoToDrive(object obj)
        {
            var drivesListName = (string)obj;
            if (drivesListName == "drives1")
            {
                var path = Tabs[0].SelectedDriveBase.Path;
                Tabs[0].Open(path, Model.ItemType.Dir);
            }
            else if (drivesListName == "drives2")
            {
                var path = Tabs[1].SelectedDriveBase.Path;
                Tabs[1].Open(path, Model.ItemType.Dir);
            }
        }

        public ICommand GoToPathCommand { get; set; }
        public void GoToPath(object obj)
        {
            var gridName = (string)obj;
            if (gridName == "grid1")
            {
                var path = Tabs[0].PanelPath;
                Tabs[0].Open(path, Model.ItemType.Dir);
            }
            else if (gridName == "grid2")
            {
                var path = Tabs[1].PanelPath;
                Tabs[1].Open(path, Model.ItemType.Dir);
            }
        }

        public ICommand SelectionChangedCommand { get; set; }
        public void SelectionChanged(object obj)
        {
            var selectedItems = ((IList)obj).Cast<IItem>().ToList();
            if (selectedItems.Count > 0)
            {
                var tab = selectedItems.First().TabViewModel;
                tab.SelectedItems.Clear();
                foreach (var item in selectedItems) { tab.SelectedItems.Add(item); }                
            }
        }

        public ICommand GoBackCommand { get; set; }
        public bool CanGoBack => Tabs[_selectedTab].CanGoBack();        
        public void GoBack(object obj) => Tabs[_selectedTab].GoBack();

        public ICommand GoForwardCommand { get; set; }
        public bool CanGoForward => Tabs[_selectedTab].CanGoForward();
        public void GoForward(object obj) => Tabs[_selectedTab].GoForward();

        public ICommand CreateNewFolderCommand { get; set; }
        public void CreateNewFolder(object obj) => Tabs[_selectedTab].CreateNewFolder();

        public ICommand CreateNewFileCommand { get; set; }
        public void CreateNewFile(object obj) => Tabs[_selectedTab].CreateNewFile();

        public ICommand RenameCommand { get; set; }
        public bool CanRename => Tabs[_selectedTab].CanRename;
        public void Rename(object obj) => Tabs[_selectedTab].Rename();

        public ICommand CopyCommand { get; set; }
        public bool CanCopy => Tabs[_selectedTab].CanCopy;
        public void Copy(object obj)
        {
            var items = Tabs[_selectedTab].SelectedItems.ToList();
            var dest = Tabs[1 - _selectedTab].CurrentPath;
            Tabs[_selectedTab].Copy(items, dest);            
        }

        public ICommand MoveCommand { get; set; }
        public bool CanMove => Tabs[_selectedTab].CanMove;
        public void Move(object obj)
        {
            var items = Tabs[_selectedTab].SelectedItems.ToList();
            var dest = Tabs[1 - _selectedTab].CurrentPath;
            Tabs[_selectedTab].Move(items, dest);
        }

        public ICommand ZipCommand { get; set; }
        public bool CanZip => Tabs[_selectedTab].CanZip;
        public void Zip(object obj)
        {
            Tabs[_selectedTab].Archive();
        }

        public ICommand RemoveCommand { get; set; }
        public bool CanRemove => Tabs[_selectedTab].CanRemove;
        public void Remove(object obj) => Tabs[_selectedTab].Remove();

        public ICommand ViewInfoCommand { get; set; }
        public bool CanViewInfo => Tabs[_selectedTab].CanViewInfo;
        public void ViewInfo(object obj) => Tabs[_selectedTab].ViewInfo();

        public ICommand BuildReportCommand { get; set; }
        public bool CanBuildReport => true;

        public void BuildReport(object obj) => Tabs[_selectedTab].BuildReport();

        #endregion
    }
}