using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HrManagementSystem.ServiceLayer.File
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string subDirectory = null);
        Task<bool> DeleteFileAsync(string fileName, string subDirectory = null);
        Task<byte[]> DownloadFileAsync(string fileName, string subDirectory = null);
        Task<string> GetFileUrlAsync(string fileName, string subDirectory = null);
        Task<List<FileInfo>> GetFileListAsync(string subDirectory = null);
        Task<bool> FileExistsAsync(string fileName, string subDirectory = null);
        Task<long> GetFileSizeAsync(string fileName, string subDirectory = null);
        Task MoveFileAsync(string sourceFileName, string destinationFileName, string sourceSubDirectory = null, string destinationSubDirectory = null);
        Task CopyFileAsync(string sourceFileName, string destinationFileName, string sourceSubDirectory = null, string destinationSubDirectory = null);
        Task<string> GetFileExtensionAsync(string fileName);
        Task<string> GetFileNameWithoutExtensionAsync(string fileName);
    }
}