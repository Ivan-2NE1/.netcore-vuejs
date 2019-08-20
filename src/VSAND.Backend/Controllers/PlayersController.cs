using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VSAND.Common;
using VSAND.Services.Data.Players;

namespace VSAND.Backend.Controllers
{
    [Route("[controller]")]
    public class PlayersController : Controller
    {
        private IPlayerService _playerService;
        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public async Task<IActionResult> Index([FromQuery(Name = "f")] string firstName, [FromQuery(Name = "l")] string lastName, [FromQuery(Name = "gy")] int graduationYear, [FromQuery(Name = "s")] int schoolId, [FromQuery(Name = "pg")] int pageNumber = 1)
        {
            var pagedResult = await _playerService.Search(firstName, lastName, graduationYear, schoolId, 50, pageNumber);
            ViewData["SearchFirstName"] = firstName;
            ViewData["SearchLastName"] = lastName;
            ViewData["SearchGraduationYear"] = graduationYear;
            ViewData["SearchSchool"] = schoolId;

            ViewData["PagedResultMessage"] = PaginationHelp.PaginationMessage(pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalResults);

            return View(pagedResult);
        }

        [HttpGet("{playerId}")]
        public async Task<IActionResult> PlayerInfo(int playerId)
        {
            var player = await _playerService.GetPlayerAsync(playerId);

            ViewData["PlayerId"] = player.PlayerId;
            ViewData["PlayerName"] = $"{player.FirstName} {player.LastName}";

            return View("Edit", player);
        }

        [HttpGet("{playerId}/Teams")]
        public async Task<IActionResult> Teams(int playerId)
        {
            var player = await _playerService.GetPlayerTeamsAsync(playerId);

            ViewData["PlayerId"] = player.PlayerId;
            ViewData["PlayerName"] = $"{player.FirstName} {player.LastName}";

            return View(player);
        }

        [HttpGet("{playerId}/Stats/{teamId}")]
        public async Task<IActionResult> Stats(int playerId, int teamId)
        {
            var player = await _playerService.GetPlayerStatsAsync(playerId, teamId);

            ViewData["PlayerId"] = player.PlayerId;
            ViewData["PlayerName"] = $"{player.FirstName} {player.LastName}";

            return View(player);
        }
    }
}