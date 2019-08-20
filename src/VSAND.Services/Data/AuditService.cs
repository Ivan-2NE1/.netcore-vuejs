using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NLog;
using VSAND.Data.Entities;
using VSAND.Data.Repositories;

namespace VSAND.Services.Data
{
    public class AuditService : IAuditService
    {
        NLog.ILogger log = LogManager.GetCurrentClassLogger();

        private IUnitOfWork _uow;

        public AuditService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentException("Unit of Work");
        }

        public async Task<List<AppxAudit>> GetAuditHistoryAsync(string table, int id)
        {
            if (id <= 0)
            {
                return null;
            }
            return await _uow.Audit.GetAuditHistoryAsync(table, id);
        }

        public List<AppxAudit> GetAuditHistory(string table, int id)
        {
            if (id <= 0)
            {
                return null;
            }
            return _uow.Audit.GetAuditHistory(table, id);
        }
    }
}
