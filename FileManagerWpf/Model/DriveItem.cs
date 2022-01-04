using System.IO;

namespace FileManagerWpf.Model
{
    public class DriveItem : BaseItem
    {
        public string VolumeLabel { get; set; }
        public long TotalFreeSpace { get; set; }
        public long UsedSpace { get; set; }
        public string DriveFormat { get; set; }
        public DriveType DriveType { get; set; }
    }
}
