using System;

namespace HrManagementSystem.ServiceLayer.File
{
    public class FileSettings
    {
        public string UploadDirectory { get; set; }
        public long MaxFileSizeBytes { get; set; }
        public string[] AllowedExtensions { get; set; }
        public bool OverwriteExistingFiles { get; set; }
        public string TempDirectory { get; set; }
    }
}