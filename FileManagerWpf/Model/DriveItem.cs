namespace FileManagerWpf.Model
{
    public class DriveItem : BaseItem
    {
        public long TotalFreeSpace { get; set; }
        public long UsedSpace { get; set; }
    }
}
