using FileManagerWpf.Services;

namespace FileManagerWpf.Model
{
    public enum TableEntityType
    {
        DIR,
        File
    }

    public class TabItem
    {
        public FileManager FileManager { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Ext { get; set; }
        public string Size { get; set; }
        public string Date { get; set; }
        public TableEntityType Type { get; set; }
    }
}
