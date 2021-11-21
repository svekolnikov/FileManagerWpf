namespace FileManagerWpf.Model
{
    public class Drive
    {
        public string VolumeLabel { get; set; }
        public string Name { get; set; }
        public long TotalSize { get; set; }
        public long TotalFreeSpace { get; set; }
        public long UsedSpace { get; set; }
    }
}
