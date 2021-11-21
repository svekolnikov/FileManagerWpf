using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FileManagerWpf.Model;
using FileManagerWpf.Services;
using FileManagerWpf.Utility;

namespace FileManagerWpf.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly TabViewModel _tab1ViewModel;
        private FileManager _tab1FileManager;
        private readonly TabViewModel _tab2ViewModel;
        private FileManager _tab2FileManager;

        public MainWindowViewModel()
        {
            _tab1ViewModel = new TabViewModel();
            _tab1FileManager = new FileManager(_tab1ViewModel);

            _tab2ViewModel = new TabViewModel();
            _tab2FileManager = new FileManager(_tab2ViewModel);

            //Commands
            OpenCommand = new RelayCommand(Open);
            SelectionChangedCommand = new RelayCommand(SelectionChanged);

            GoToPathCommand = new RelayCommand(GoToPath);
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

        public TabViewModel Tab1 => _tab1ViewModel;
        public TabViewModel Tab2 => _tab2ViewModel;

        #region Commands
        public ICommand OpenCommand { get; set; }
        public void Open(object obj)
        {
            var item = (TabItem)obj;
            item.FileManager.Open(item);
        }
        public ICommand GoToPathCommand { get; set; }
        public void GoToPath(object obj)
        {
            _tab1FileManager.GoToPath(Tab1.Path);
        }
        public ICommand SelectionChangedCommand { get; set; }
        public void SelectionChanged(object obj)
        {
            //_selectedItems = ((IList)obj).Cast<TabItem>().ToList();

            //_tab1ViewModel.SelectedCount = _selectedItems.Count;
        }


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
