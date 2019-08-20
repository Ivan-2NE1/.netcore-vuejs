using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;

namespace VSAND.Services.Data
{
    public interface IAuditService
    {
        Task<List<AppxAudit>> GetAuditHistoryAsync(string table, int id);
        List<AppxAudit> GetAuditHistory(string table, int id);
    }
}
