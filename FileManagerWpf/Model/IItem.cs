using FileManagerWpf.ViewModel;

namespace FileManagerWpf.Model
{
    public  interface IItem
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public ItemType Type { get; set; }
        public TabViewModel TabViewModel { get; set; }
    }
}
