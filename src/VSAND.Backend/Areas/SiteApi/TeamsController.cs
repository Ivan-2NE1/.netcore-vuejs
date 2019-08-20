using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.Teams;
using VSAND.Services.Data.Teams;
using VSAND.Services.Files;

namespace VSAND.Backend.Areas.SiteApi
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("multipart/form-data", "application/x-www-form-urlencoded", "application/json")]
    [Route("SiteApi/[controller]")]
    public class TeamsController : ControllerBase
    {
        private ITeamService _teamService;
        private IFileService _fileService;
        private IExcelService _excelService;

        public TeamsController(ITeamService teamService, IFileService fileService, IExcelService excelService)
        {
            _teamService = teamService;
            _fileService = fileService;
            _excelService = excelService;
        }

        // GET: SiteApi/Teams/Autocomplete
        [HttpGet("Autocomplete")]
        public async Task<IEnumerable<ListItem<int>>> AutocompleteAsync([FromQuery] string q, [FromQuery] int sportId, [FromQuery] int scheduleYearId)
        {
            return await _teamService.AutocompleteAsync(q, sportId, scheduleYearId);
        }

        // GET: SiteApi/Teams/AutocompleteRestore
        [HttpGet("AutocompleteRestore")]
        public async Task<ListItem<int>> AutocompleteRestoreAsync([FromQuery] int teamId)
        {
            return await _teamService.AutocompleteRestoreAsync(teamId);
        }

        // GET: SiteApi/GetTeamId
        [HttpGet("GetTeamId")]
        public async Task<int> GetTeamIdAsync([FromQuery] int schoolId, [FromQuery] int sportId, [FromQuery] int scheduleYearId)
        {
            return await _teamService.GetTeamIdAsync(schoolId, sportId, scheduleYearId);
        }

        //Get siteapi/teams/teamRoster
        [HttpGet("teamRoster/{teamId}")]
        public async Task<IEnumerable<TeamRoster>> GetTeamToster(int teamid)
        {
            var oRet = await _teamService.GetRoster(teamid);
            return oRet;
        }

        //Put siteapi/teams/teamRoster
        [HttpPut("teamRoster/{teamId}")]
        public async Task<ApiResult<VsandTeamRoster>> UpdateTeamRoster(int teamId, [FromBody] VsandTeamRoster teamRoster)
        {
            var oResult = new ApiResult<VsandTeamRoster>();
            if (teamRoster == null || teamRoster.TeamId != teamId)
            {
                oResult.Message = "Roster information mismatch.";
                return oResult;
            }

            var result = await _teamService.SaveRoster(teamRoster);
            oResult = new ApiResult<VsandTeamRoster>(result);
            return oResult;
        }

        //Delete siteapi/teams/teamRoster
        [HttpDelete("teamRoster/{rosterId}")]
        public async Task<ApiResult<VsandTeamRoster>> DeleteTeamRoster(int rosterId)
        {
            var result = await _teamService.DeleteRoster(rosterId);
            return new ApiResult<VsandTeamRoster>(result);
        }

        [HttpGet("corecoveragelist")]
        public async Task<List<ListItem<int>>> CoreCoverageList([FromQuery] int sportId, [FromQuery] int scheduleYearId)
        {
            return await _teamService.CoreCoverageListAsync(sportId, scheduleYearId);
        }

        // GET: SiteApi/Teams/CustomCodeList
        [HttpGet("CustomCodeList")]
        public async Task<List<ListItem<string>>> GetCustomCodeListAsync([FromQuery] string codeName)
        {
            return await _teamService.GetUniqueCustomCodeValues(codeName);
        }

        // GET: SiteApi/Teams/5
        [HttpGet("{teamId}", Name = "GetTeam")]
        public string Get(int teamId)
        {
            return "value";
        }

        // POST: SiteApi/Teams
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT: SiteApi/Teams/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE: SiteApi/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }

        // GET: SiteApi/Teams/5/OosRecord
        [HttpGet("{teamId}/OosRecord")]
        public async Task<TeamOosRecord> GetOosRecord(int teamId)
        {
            return await _teamService.GetOosRecord(teamId);
        }

        // PUT: SiteApi/Teams/5/OosRecord
        [HttpPut("{teamId}/OosRecord")]
        public async Task<ApiResult<TeamOosRecord>> UpdateOosRecord(int teamId, [FromBody] TeamOosRecord teamRecord)
        {
            var oResult = new ApiResult<TeamOosRecord>();
            if (teamRecord == null || teamRecord.TeamId != teamId)
            {
                oResult.Message = "Team information mismatch.";
                return oResult;
            }

            var result = await _teamService.SaveOosRecord(teamRecord);
            oResult = new ApiResult<TeamOosRecord>(result);
            return oResult;
        }

        #region Custom Codes
        [HttpPost("CustomCodes")]
        public async Task<ApiResult<VsandTeamCustomCode>> AddTeamCustomCode([FromBody] VsandTeamCustomCode addCustomCode)
        {
            var result = await _teamService.AddCustomCodeAsync(addCustomCode);
            return new ApiResult<VsandTeamCustomCode>(result);
        }

        [HttpPut("CustomCodes/{customCodeId}")]
        public async Task<ApiResult<VsandTeamCustomCode>> UpdateTeamCustomCode([FromBody] VsandTeamCustomCode chgCustomCode)
        {
            var result = await _teamService.UpdateCustomCodeAsync(chgCustomCode);
            return new ApiResult<VsandTeamCustomCode>(result);
        }

        [HttpDelete("CustomCodes/{customCodeId}")]
        public async Task<ApiResult<VsandTeamCustomCode>> DeleteTeamCustomCode(int customCodeId)
        {
            var result = await _teamService.DeleteCustomCodeAsync(customCodeId);
            return new ApiResult<VsandTeamCustomCode>(result);
        }
        #endregion

        #region Team Custom Code Importer
        [HttpGet("customcodeimporterfilelist")]
        public List<ListItem<string>> CustomCodeImporterFileList()
        {
            var fileList = new List<ListItem<string>>();

            var allowedExtensions = new List<string> { ".xls", ".xlsx" };
            var viewCutoff = DateTime.Now.AddDays(-60);

            var baseDir = new DirectoryInfo("appdata/TeamCodeImport");
            if (baseDir.Exists)
            {
                var files = baseDir.GetFiles();
                foreach (var file in files.Where(f => allowedExtensions.Contains(f.Extension.ToLower()) && f.LastWriteTime > viewCutoff).OrderBy(f => f.Name).ToList())
                {
                    fileList.Add(new ListItem<string>(file.Name, $"{file.Name} (Modified: {file.LastWriteTime.ToString("MM/dd/yyyy")})"));
                }
            }

            return fileList;
        }

        [HttpPost("customcodeimporterupload")]
        public async Task<ServiceResult<string>> CustomcodeImporterUpload([FromForm]TeamCustomCodesUploadRequest request)
        {
            var oResult = new ServiceResult<string>();
            if (request.File.Length <= 0)
            {
                oResult.Success = false;
                oResult.Message = "The uploaded file is empty.";
                return oResult;
            }

            var allowedExtensions = new List<string> { ".xls", ".xlsx" };
            if (!_fileService.IsAllowedType(allowedExtensions, request.File.FileName))
            {
                oResult.Success = false;
                oResult.Message = $"The uploaded file must be one of the following: {string.Join(", ", allowedExtensions)}";
                return oResult;
            }

            var fileExt = _fileService.GetExtension(request.File.FileName);
            var storagePath = "TeamCodeImport";
            var baseDir = new DirectoryInfo(_fileService.GetAppDataStoragePath(storagePath));

            // full path to file in temp location
            string fileName = $"{request.ScheduleYearName}-{request.SportName.Replace(" ", "-")}{fileExt}";
            var saveResult = await _fileService.SaveUploadedFile(storagePath, fileName, true, request.File);
            if (saveResult.Success)
            {
                oResult.Success = true;
                // great! 
                oResult.obj = fileName;
            }
            else
            {
                oResult.Success = false;
                oResult.Message = saveResult.Message;
            }
            return oResult;
        }

        [HttpGet("customcodeimportersheetnames")]
        public List<ListItem<string>> CustomCodeImporterSheetNames([FromQuery(Name = "filename")]string fileName)
        {
            string fullPath = _fileService.GetFullName("TeamCodeImport", fileName);
            return _excelService.ExcelSheetNames(fullPath);
        }

        [HttpGet("customcodeimportersheetdata")]
        public object CustomCodeImporterSheetData([FromQuery(Name = "filename")]string fileName, [FromQuery(Name = "sheetname")]string sheetName)
        {
            string fullPath = _fileService.GetFullName("TeamCodeImport", fileName);
            return _excelService.ExcelSheetToObj(fullPath, sheetName);
        }

        [HttpPost("customcodeimportersave")]
        public async Task<ApiResult<bool>> CustomCodeImporterSave([FromBody] TeamCustomCodesUploadSaveRequest saveRequest)
        {
            var result = await _teamService.BulkSaveTeamCustomCodes(saveRequest.SportId, saveRequest.ScheduleYearId, saveRequest.Codes);
            return new ApiResult<bool>(result);
        }

        #endregion
    }
}
