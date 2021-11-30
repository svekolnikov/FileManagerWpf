namespace FileManagerWpf.Model
{
    public class Drive : TabItem
    {
        public long TotalSize { get; set; }
        public long TotalFreeSpace { get; set; }
        public long UsedSpace { get; set; }
    }
}
