using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VSAND.Services.Data;

namespace VSAND.Backend.Controllers
{
    [Authorize(Policy = "Admin")]
    public class AuditController : Controller
    {
        private readonly IAuditService _auditService;
        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        public async Task<IActionResult> Index([FromQuery] string table, [FromQuery] int id)
        {
            ViewData["AuditTable"] = table;
            ViewData["AuditId"] = id;

            var history = await _auditService.GetAuditHistoryAsync(table, id);
            return View(history);
        }
    }
}