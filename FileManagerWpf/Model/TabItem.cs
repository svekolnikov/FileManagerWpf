using FileManagerWpf.ViewModel;

namespace FileManagerWpf.Model
{
    public enum EntityType
    {
        Dir,
        File
    }

    public class TabItem
    {
        public TabViewModel TabViewModel { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Date { get; set; }
        public EntityType Type { get; set; }
    }
}
