using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HrManagementSystem.ServiceLayer.File
{
    public class FileService : IFileService
    {
        private readonly FileSettings _fileSettings;
        private readonly ILogger<FileService> _logger;

        public FileService(IOptions<FileSettings> fileSettings, ILogger<FileService> logger)
        {
            _fileSettings = fileSettings.Value;
            _logger = logger;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string subDirectory = null)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new ArgumentException("File is empty", nameof(file));

                if (file.Length > _fileSettings.MaxFileSizeBytes)
                    throw new ArgumentException("File size exceeds the limit", nameof(file));

                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_fileSettings.AllowedExtensions.Contains(fileExtension))
                    throw new ArgumentException("File type is not allowed", nameof(file));

                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var directoryPath = Path.Combine(_fileSettings.UploadDirectory, subDirectory ?? string.Empty);
                var filePath = Path.Combine(directoryPath, fileName);

                Directory.CreateDirectory(directoryPath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return fileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error uploading file {file?.FileName}");
                throw;
            }
        }

        public async Task<bool> DeleteFileAsync(string fileName, string subDirectory = null)
        {
            try
            {
                var filePath = Path.Combine(_fileSettings.UploadDirectory, subDirectory ?? string.Empty, fileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting file {fileName}");
                throw;
            }
        }

        public async Task<byte[]> DownloadFileAsync(string fileName, string subDirectory = null)
        {
            try
            {
                var filePath = Path.Combine(_fileSettings.UploadDirectory, subDirectory ?? string.Empty, fileName);
                if (!System.IO.File.Exists(filePath))
                    throw new FileNotFoundException($"File {fileName} not found");

                return await System.IO.File.ReadAllBytesAsync(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error downloading file {fileName}");
                throw;
            }
        }

        public async Task<string> GetFileUrlAsync(string fileName, string subDirectory = null)
        {
            // This method should return a URL that can be used to access the file.
            // The exact implementation will depend on your application's setup.
            return $"/api/files/{subDirectory}/{fileName}";
        }

        public async Task<List<FileInfo>> GetFileListAsync(string subDirectory = null)
        {
            try
            {
                var directoryPath = Path.Combine(_fileSettings.UploadDirectory, subDirectory ?? string.Empty);
                var directory = new DirectoryInfo(directoryPath);
                return directory.GetFiles().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting file list for subdirectory {subDirectory}");
                throw;
            }
        }

        public async Task<bool> FileExistsAsync(string fileName, string subDirectory = null)
        {
            var filePath = Path.Combine(_fileSettings.UploadDirectory, subDirectory ?? string.Empty, fileName);
            return System.IO.File.Exists(filePath);
        }

        public async Task<long> GetFileSizeAsync(string fileName, string subDirectory = null)
        {
            try
            {
                var filePath = Path.Combine(_fileSettings.UploadDirectory, subDirectory ?? string.Empty, fileName);
                var fileInfo = new FileInfo(filePath);
                return fileInfo.Length;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting file size for {fileName}");
                throw;
            }
        }

        public async Task MoveFileAsync(string sourceFileName, string destinationFileName, string sourceSubDirectory = null, string destinationSubDirectory = null)
        {
            try
            {
                var sourcePath = Path.Combine(_fileSettings.UploadDirectory, sourceSubDirectory ?? string.Empty, sourceFileName);
                var destinationPath = Path.Combine(_fileSettings.UploadDirectory, destinationSubDirectory ?? string.Empty, destinationFileName);

                System.IO.File.Move(sourcePath, destinationPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error moving file from {sourceFileName} to {destinationFileName}");
                throw;
            }
        }

        public async Task CopyFileAsync(string sourceFileName, string destinationFileName, string sourceSubDirectory = null, string destinationSubDirectory = null)
        {
            try
            {
                var sourcePath = Path.Combine(_fileSettings.UploadDirectory, sourceSubDirectory ?? string.Empty, sourceFileName);
                var destinationPath = Path.Combine(_fileSettings.UploadDirectory, destinationSubDirectory ?? string.Empty, destinationFileName);

                System.IO.File.Copy(sourcePath, destinationPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error copying file from {sourceFileName} to {destinationFileName}");
                throw;
            }
        }

        public async Task<string> GetFileExtensionAsync(string fileName)
        {
            return Path.GetExtension(fileName);
        }

        public async Task<string> GetFileNameWithoutExtensionAsync(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }
    }
}