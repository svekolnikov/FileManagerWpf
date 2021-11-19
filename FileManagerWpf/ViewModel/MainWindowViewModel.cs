using System.Collections.ObjectModel;
using System.Windows.Input;
using FileManagerWpf.Model;
using FileManagerWpf.Utility;

namespace FileManagerWpf.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            LeftItems = new TabViewModel
            {
                Drives = new ObservableCollection<Drive>
                {
                    new() {Letter = "C", Name = "System", TotalSpace = 200, FreeSpace = 20, UsedSpace = 180},
                    new() {Letter = "D", Name = "Data", TotalSpace = 800, FreeSpace = 300, UsedSpace = 500}
                },
                Navigation = @"D:\Music\",
                Items = new ObservableCollection<TabItem>
                {
                    new() {Name = "Folder", Ext = "", Size = "<DIR>", Date = "24/07/2021 00:36"},
                    new() {Name = "Folder", Ext = "", Size = "<DIR>", Date = "25/07/2021 00:36"},
                    new() {Name = "Folder", Ext = "", Size = "<DIR>", Date = "26/07/2021 00:36"},
                    new() {Name = "File", Ext = "iso", Size = "234897", Date = "27/07/2021 00:36"},
                    new() {Name = "File", Ext = "exe", Size = "14778", Date = "28/07/2021 00:36"},
                }
            };
            
            //Commands
            GoBackCommand = new RelayCommand(GoBack);
            GoForwardCommand = new RelayCommand(GoForward);
            CreateNewFolderCommand = new RelayCommand(CreateNewFolder);
            CreateNewFileCommand = new RelayCommand(CreateNewFile);
            RenameCommand = new RelayCommand(Rename);
            CopyCommand = new RelayCommand(Copy);
            MoveCommand = new RelayCommand(Move);
            PackToArchiveCommand = new RelayCommand(PackToArchive);
            RemoveCommand = new RelayCommand(Remove);
            QuestionCommand = new RelayCommand(Question);
        }

        public TabViewModel LeftItems { get; set; }

        #region Commands

        public ICommand GoBackCommand { get; set; }
        public void GoBack(object obj)
        {
        }

        public ICommand GoForwardCommand { get; set; }
        public void GoForward(object obj)
        {
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
