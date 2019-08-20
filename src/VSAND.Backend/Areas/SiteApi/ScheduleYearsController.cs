using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VSAND.Backend.Controllers;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.ScheduleYears;
using VSAND.Services.Data.Manage.ScheduleYears;
using VSAND.Services.Data.Manage.Users;
using VSAND.Services.Files;

namespace VSAND.Backend.Areas.SiteApi
{
    [ApiController]
    [Route("SiteApi/[controller]")]
    public class ScheduleYearsController : BaseController
    {
        private readonly IScheduleYearService _scheduleYearService;
        private readonly IFileService _fileService;
        private readonly IExcelService _excelService;
        private readonly IUserService _userService;


        public ScheduleYearsController(IScheduleYearService scheduleYearService, IFileService fileService, IExcelService excelService, IUserService userService) : base(userService)
        {
            _scheduleYearService = scheduleYearService;
            _fileService = fileService;
            _excelService = excelService;
            _userService = userService;
        }

        // GET: SiteApi/ScheduleYears
        [HttpGet("list")]
        public async Task<IEnumerable<ListItem<int>>> GetListAsync()
        {
            return await _scheduleYearService.GetList();
        }

        // GET: SiteApi/ScheduleYears
        [HttpGet("listall")]
        public async Task<IEnumerable<ScheduleYear>> ListAllAsync()
        {
            return await _scheduleYearService.List();
        }

        // GET: SiteApi/ScheduleYears/5
        [HttpGet("{id}", Name = "GetScheduleYearById")]
        public async Task<ScheduleYear> Get(int id)
        {
            return await _scheduleYearService.GetSummary(id);
        }

        // POST: SiteApi/ScheduleYears
        [HttpPost]
        public async Task<ApiResult<ScheduleYear>> Post(ScheduleYear vm)
        {
            var serviceResult = await _scheduleYearService.AddScheduleYearAsync(vm);
            return new ApiResult<ScheduleYear>(serviceResult);
        }

        // POST: SiteApi/ScheduleYears/SetActive
        [HttpGet("SetActive")]
        public async Task<ApiResult<ScheduleYear>> SetActive([FromQuery]int scheduleYearId) {
            var serviceResult = await _scheduleYearService.SetActiveScheduleYearAsync(scheduleYearId);
            return new ApiResult<ScheduleYear>(serviceResult);
        }

        // PUT: SiteApi/ScheduleYears/PowerPoints/5
        [HttpPut("PowerPoints/{ppConfigId}")]
        public async Task<ApiResult<VsandPowerPointsConfig>> InsertOrUpdatePowerPoints(int ppConfigId, [FromBody] VsandPowerPointsConfig ppConfig)
        {
            if (ppConfigId != ppConfig.PPConfigId)
            {
                return null;
            }

            var result = await _scheduleYearService.InsertOrUpdatePowerPoints(ppConfig);
            return new ApiResult<VsandPowerPointsConfig>(result);
        }

        // PUT: SiteApi/ScheduleYears/5/LeagueRules/14
        [HttpPut("{scheduleYearId}/LeagueRules/{leagueRuleId}")]
        public async Task<ApiResult<LeagueRule>> UpdateLeagueRule(int leagueRuleId, [FromBody] LeagueRule leagueRuleVm)
        {
            if (leagueRuleId != leagueRuleVm.LeagueRuleId)
            {
                return null;
            }

            var result = await _scheduleYearService.UpdateLeagueRule(leagueRuleVm);
            return new ApiResult<LeagueRule>(result);
        }

        // POST: SiteApi/ScheduleYears/5/LeagueRules/14/PopulateFromExistingData
        [HttpPost("{scheduleYearId}/LeagueRules/{sportId}/PopulateFromExistingData")]
        public async Task<ApiResult<VsandLeagueRule>> PopulateLeagueRulesFromExistingData(int scheduleYearId, int sportId)
        {
            var result = await _scheduleYearService.PopulateLeagueRulesFromExistingData(scheduleYearId, sportId);
            return new ApiResult<VsandLeagueRule>(result);
        }

        // POST: SiteApi/ScheduleYears/5/LeagueRules/14/PopulateFromPreviousYear
        [HttpPost("{scheduleYearId}/LeagueRules/{sportId}/PopulateFromPreviousYear")]
        public async Task<ApiResult<VsandLeagueRule>> PopulateLeagueRulesFromPreviousYear(int scheduleYearId, int sportId)
        {
            var result = await _scheduleYearService.PopulateLeagueRulesFromPreviousYear(scheduleYearId, sportId);
            return new ApiResult<VsandLeagueRule>(result);
        }

        #region Schedule File Processing
        [HttpPost("ScheduleFileUpload")]
        public async Task<ServiceResult<VsandScheduleLoadFile>> ScheduleFileUpload([FromForm]ScheduleFileUploadRequest request)
        {
            var oResult = new ServiceResult<VsandScheduleLoadFile>();
            if (request.File.Length <= 0)
            {
                oResult.Success = false;
                oResult.Message = "The uploaded file is empty.";
                return oResult;
            }

            var allowedExtensions = new List<string> { ".xlsx" };
            if (!_fileService.IsAllowedType(allowedExtensions, request.File.FileName))
            {
                oResult.Success = false;
                oResult.Message = $"The uploaded file must be one of the following: {string.Join(", ", allowedExtensions)}";
                return oResult;
            }

            var fileExt = _fileService.GetExtension(request.File.FileName);
            var storagePath = "ScheduleImport";
            var baseDir = new DirectoryInfo(_fileService.GetAppDataStoragePath(storagePath));
            
            // full path to file in temp location
            string fileName = $"{request.ScheduleYearName}-{request.SportName.Replace(" ", "-")}-{DateTime.Now.ToString("yyyyMMddhhmmss")}{fileExt}";
            var saveResult = await _fileService.SaveUploadedFile(storagePath, fileName, true, request.File);
            if (!saveResult.Success)
            {
                oResult.Message = saveResult.Message;
                return oResult;
            }

            string filePath = _fileService.GetFullName(storagePath, fileName);
            long fileSize = _fileService.GetFileSize(filePath);

            var result = await _scheduleYearService.UploadScheduleFile(request.ScheduleYearId, request.SportId, fileName, fileExt, fileSize);
            return result;
            
        }

        [HttpGet("ScheduleFileImport/{fileId}")]
        public async Task<ApiResult<bool>> ScheduleFileImport(int fileId)
        {
            var oRet = new ApiResult<bool>();
            var file = await _scheduleYearService.GetScheduleFileRecord(fileId);
            if (file == null)
            {
                oRet.Message = "Could not load requested file record.";
                return oRet;
            }

            var storagePath = "ScheduleImport";
            string filePath = _fileService.GetFullName(storagePath, file.FileName);

            List<ScheduleLoadFileRow> scheduleRows = _excelService.GetDataToList(filePath, ScheduleLoadFileRow.ImportRow);

            BackgroundJob.Enqueue(() => _scheduleYearService.ImportScheduleFile(file.ScheduleYearId, file.SportId, fileId, scheduleRows));

            oRet.Success = true;
            return oRet;
        }

        [HttpPost("ScheduleFileResolve/{fileId}")]
        public async Task<ApiResult<bool>> AcceptScheduleFileResolve(int fileId, [FromBody] ScheduleFileResolvedItem resolved)
        {
            var result = await _scheduleYearService.ResolveScheduleItem(ApplicationUser, fileId, resolved);
            return new ApiResult<bool>(result);
        }

        [HttpGet("ScheduleFileCommit/{fileId}")]
        public async Task<ApiResult<bool>> ScheduleFileCommit(int fileId)
        {
            var oRet = new ApiResult<bool>();
            var file = await _scheduleYearService.GetScheduleFileRecord(fileId);
            if (file == null)
            {
                oRet.Message = "Could not load requested file record.";
                return oRet;
            }

            BackgroundJob.Enqueue(() => _scheduleYearService.CommitScheduleFile(ApplicationUser.AppxUser, file.ScheduleYearId, file.SportId, fileId));

            oRet.Success = true;
            return oRet;
        }

        #endregion
    }
}
