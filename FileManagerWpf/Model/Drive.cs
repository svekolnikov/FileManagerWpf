namespace FileManagerWpf.Model
{
    public class Drive
    {
        public string Letter { get; set; }
        public string Name { get; set; }
        public long TotalSpace { get; set; }
        public long FreeSpace { get; set; }
        public long UsedSpace { get; set; }
    }
}
