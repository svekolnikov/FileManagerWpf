using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using FileManagerWpf.Model;
using TemplateEngine.Docx;

namespace FileManagerWpf.Services
{
    public sealed class ReportService
    {
        private readonly FileManager _fileManager;

        public ReportService(FileManager fileManager)
        {
            _fileManager = fileManager;
        }

        public void GenerateReport(DriveItem driveItem, string output = "")
        {
            if (driveItem is null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(output))
            {
                output = Path.Combine(Directory.GetCurrentDirectory(), "DriveReport.docx");
            }

            if (File.Exists(output))
            {
                File.Delete(output);
            }

            File.Copy("ReportTemplate.docx", output);

            var valuesToFill = new Content(
                new FieldContent("letter", driveItem.Name.Split(":")[0]),
                new FieldContent("volume-label", driveItem.VolumeLabel),
                new FieldContent("size", driveItem.Size.ToString()),
                new FieldContent("free-space", driveItem.TotalFreeSpace.ToString()),
                new FieldContent("used-space", driveItem.UsedSpace.ToString()),
                new FieldContent("format", driveItem.DriveFormat),
                new FieldContent("type", driveItem.DriveType.ToString())
            );

            using var outputDocument =
                new TemplateProcessor(output)
                    .SetRemoveContentControls(true);
            outputDocument.FillContent(valuesToFill);
            outputDocument.SaveChanges();

            _fileManager.StartProcess(output);
        }
    }

}
