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
        private List<TabItem> _selectedItems = new List<TabItem>();
        private TabViewModel _leftTabViewModel;
        private FileManager _leftTabFileManager;
        private TabViewModel _rightTabViewModel;
        private FileManager _rightTabFileManager;

        public MainWindowViewModel()
        {
            _leftTabViewModel = new TabViewModel();
            _leftTabFileManager = new FileManager(_leftTabViewModel);

            _rightTabViewModel = new TabViewModel();
            _rightTabFileManager = new FileManager(_rightTabViewModel);

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

        public TabViewModel LeftTab { get => _leftTabViewModel; }

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
            _leftTabFileManager.GoToPath(LeftTab.Path);
        }
        public ICommand SelectionChangedCommand { get; set; }
        public void SelectionChanged(object obj)
        {
            _selectedItems = ((IList)obj).Cast<TabItem>().ToList();

            _leftTabViewModel.SelectedCount = _selectedItems.Count;
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
