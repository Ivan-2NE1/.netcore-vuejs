using Newtonsoft.Json;
using System.Collections.Generic;

namespace VSAND.Data.ViewModels.Users
{
    public class SchoolMasterAccount
    {
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        [JsonIgnore]
        public bool Valid
        {
            get {
                return !Invalid;
            }
        }

        [JsonIgnore]
        public bool Invalid
        {
            get {
                return SchoolId == 0 || SchoolName == "" || Username == "" || Password == "";
            }
        }

        public static SchoolMasterAccount ImportRow(IList<string> rowData, IList<string> columnNames)
        {
            int.TryParse(rowData[columnNames.IndexOf("schoolid")].Trim(), out int schoolId);

            return new SchoolMasterAccount()
            {
                SchoolId = schoolId,
                SchoolName = rowData[columnNames.IndexOf("schoolname")].Trim(),
                Username = rowData[columnNames.IndexOf("username")].Trim(),
                Password = rowData[columnNames.IndexOf("password")].Trim()
            };
        }
    }
}
