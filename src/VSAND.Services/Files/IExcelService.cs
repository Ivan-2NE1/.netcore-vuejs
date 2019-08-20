using System;
using System.Collections.Generic;
using System.Data;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.Teams;
using VSAND.Data.ViewModels.Users;

namespace VSAND.Services.Files
{
    public interface IExcelService
    {
        List<ListItem<string>> ExcelSheetNames(string filePath);
        DataTable ExcelSheetToDataTable(string filePath, string sheetName);
        object ExcelSheetToObj(string filePath, string sheetName);

        List<T> GetDataToList<T>(string filePath, Func<IList<string>, IList<string>, T> addRowData);
        List<T> GetDataToList<T>(string filePath, string sheetName, Func<IList<string>, IList<string>, T> addRowData);

        byte[] TeamRosterFile(List<TeamRoster> exportRoster, string fileName);
        byte[] SchoolMasterAccountFile(List<SchoolMasterAccount> exportMasterAccounts, string fileName);
    }
}
