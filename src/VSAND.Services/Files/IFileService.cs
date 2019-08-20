using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VSAND.Data.ViewModels;

namespace VSAND.Services.Files
{
    public interface IFileService
    {
        string GetExtension(string fileName);
        bool IsAllowedType(List<string> allowedExtensions, string fileName);
        string GetAppDataStoragePath(string folderName);
        string GetFullName(string folderName, string fileName);
        long GetFileSize(string folderName, string fileName);
        long GetFileSize(string fullPath);
        Task<ServiceResult<string>> SaveUploadedFile(string targetPath, string fileName, bool allowOverwrite, IFormFile file);
    }
}
