using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSAND.Data.Entities;

namespace VSAND.Data.Repositories
{
    public class AuditRepository : Repository<AppxAudit>, IAuditRepository
    {
        private readonly VsandContext _context;
        public AuditRepository(VsandContext context) : base(context)
        {
            _context = context;
        }

        public enum AuditType
        {
            Insert = 0,
            Update = 2,
            Delete = 3
        }

        // non-static methods to use with dependency injection
        public string GetAuditRow(string sTable, string sKeyField, int iKey)
        {
            return GetAuditRow(_context, sTable, sKeyField, iKey);
        }

        public void AuditChange(string sTable, string sKeyField, int iKey, string sAction, AppxUser user)
        {
            AuditChange(_context, sTable, sKeyField, iKey, sAction, user.UserId, user.AdminId);
        }
        public void AuditChange(string sTable, string sKeyField, int iKey, string sAction, string sUser, int iUserId)
        {
            AuditChange(_context, sTable, sKeyField, iKey, sAction, sUser, iUserId);
        }

        public List<AppxAudit> GetAuditHistory(string auditTable, int auditId)
        {
            return GetAuditHistory(_context, auditTable, auditId);
        }

        public async Task<List<AppxAudit>> GetAuditHistoryAsync(string auditTable, int auditId)
        {
            return await GetAuditHistoryAsync(_context, auditTable, auditId);
        }

        // static methods
        public static string GetAuditRow(VsandContext context, string sTable, string sKeyField, int iKey)
        {
            StringBuilder oSB = new StringBuilder();

            if (!context.Database.IsSqlServer())
            {
                return "";
            }

            var oConn = context.Database.GetDbConnection();
            if (oConn != null)
            {
                if (oConn.State == System.Data.ConnectionState.Closed)
                {
                    oConn.Open();
                }
            }

            using (SqlCommand oCmd = new SqlCommand("AppxAudit_GetXmlData", (SqlConnection)oConn))
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.Parameters.AddWithValue("@Table", sTable);
                oCmd.Parameters.AddWithValue("@KeyName", sKeyField);
                oCmd.Parameters.AddWithValue("@Key", iKey);

                using (var oRdr = oCmd.ExecuteReader())
                {
                    while (oRdr.Read())
                    {
                        string sPart = oRdr[0].ToString();
                        oSB.Append(sPart);
                    }
                }
            }

            return oSB.ToString();
        }

        public static void AuditChange(VsandContext context, string sTable, string sKeyField, int iKey, string sAction, string sUser, int iUserId)
        {
            string sAuditData = GetAuditRow(context, sTable, sKeyField, iKey);

            if (!context.Database.IsSqlServer())
            {
                return;
            }

            var oConn = context.Database.GetDbConnection();
            if (oConn != null)
            {
                if (oConn.State == System.Data.ConnectionState.Closed)
                {
                    oConn.Open();
                }
            }

            using (SqlCommand oCmd = new SqlCommand("appxAudit_Change", (SqlConnection)oConn))
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.Parameters.AddWithValue("@Table", sTable);
                oCmd.Parameters.AddWithValue("@KeyName", sKeyField);
                oCmd.Parameters.AddWithValue("@Key", iKey);
                oCmd.Parameters.AddWithValue("@Action", sAction);
                oCmd.Parameters.AddWithValue("@User", sUser);
                oCmd.Parameters.AddWithValue("@UserID", iUserId);
                oCmd.Parameters.AddWithValue("@auditData", sAuditData);

                oCmd.ExecuteNonQuery();
            }
        }

        public static List<AppxAudit> GetAuditHistory(VsandContext context, string auditTable, int auditId)
        {
            return GetAuditHistoryAsync(context, auditTable, auditId).Result;
        }

        public static async Task<List<AppxAudit>> GetAuditHistoryAsync(VsandContext context, string auditTable, int auditId)
        {
            return await context.AppxAudit.Where(a => a.AuditTable == auditTable && a.AuditKey == auditId).OrderByDescending(a => a.CreatedDate).ToListAsync();
        }
    }
}
