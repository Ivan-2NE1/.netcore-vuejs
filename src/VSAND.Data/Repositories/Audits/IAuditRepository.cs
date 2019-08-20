using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;

namespace VSAND.Data.Repositories
{
    public interface IAuditRepository : IRepository<AppxAudit>
    {
        string GetAuditRow(string sTable, string sKeyField, int iKey);

        void AuditChange(string sTable, string sKeyField, int iKey, string sAction, AppxUser user);
        void AuditChange(string sTable, string sKeyField, int iKey, string sAction, string sUser, int iUserId);

        List<AppxAudit> GetAuditHistory(string auditTable, int auditId);

        Task<List<AppxAudit>> GetAuditHistoryAsync(string auditTable, int auditId);
    }
}
