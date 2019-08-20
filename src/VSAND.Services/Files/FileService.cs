using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSAND.Data.ViewModels;

namespace VSAND.Services.Files
{
    public class FileService : IFileService
    {
        public string GetAppDataStoragePath(string folderName)
        {
            string folderPath = Path.Combine("appdata", folderName);
            Directory.CreateDirectory(folderPath);
            return folderPath;
        }

        public string GetExtension(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            if (ext == null || string.IsNullOrWhiteSpace(ext))
            {
                return "";
            }
            return ext.ToLowerInvariant();
        }

        public string GetFullName(string folderName, string fileName)
        {
            return Path.Combine(GetAppDataStoragePath(folderName), fileName);
        }

        public long GetFileSize(string folderName, string fileName)
        {
            return GetFileSize(GetFullName(folderName, fileName));
        }

        public long GetFileSize(string fullPath)
        {
            var fileInfo = new FileInfo(fullPath);
            if (fileInfo.Exists)
            {
                return fileInfo.Length;
            }
            return 0;
        }

        public bool IsAllowedType(List<string> allowedExtensions, string fileName)
        {
            if (allowedExtensions == null || !allowedExtensions.Any())
            {
                return true;
            }

            string extension = GetExtension(fileName);
            return allowedExtensions.Any(e => e.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<ServiceResult<string>> SaveUploadedFile(string appDataStoragePath, string fileName, bool allowOverwrite, IFormFile file)
        {
            var oResult = new ServiceResult<string>();

            string basePath = GetAppDataStoragePath(appDataStoragePath);
            string fullPath = Path.Combine(basePath, fileName);
            if (!allowOverwrite && File.Exists(fullPath))
            {
                oResult.Success = false;
                oResult.Message = "A file with the same name already exists";
                return oResult;
            }

            try
            {
                using (var targetStream = File.Create(fullPath))
                {
                    await file.CopyToAsync(targetStream);
                }
                oResult.Success = true;
                oResult.Message = "file created";
                oResult.obj = fileName;
            }
            catch (Exception ex)
            {
                oResult.Success = false;
                oResult.Message = ex.Message;
            }

            return oResult;
        }
    }
}
