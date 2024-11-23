
using Microsoft.AspNetCore.Mvc;
using Models;
using Repository;
using System.Net;


namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        [HttpGet("/players")]
        [ProducesResponseType(typeof(IEnumerable<Player>), (int)HttpStatusCode.OK)]
        public IActionResult Index(string birthplace = null)
        {
            var players = _playerRepository.GetPlayers(birthplace);
            return Ok(players);
        }

        [HttpGet("/players/{id}")]
        [ProducesResponseType(typeof(Player), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult Details(int id)
        {
            var player = _playerRepository.GetPlayer(id);
            if (player == null)
            {
                return NotFound();
            }
            return Ok(player);
        }


        [HttpPost("/players")]
        [ProducesResponseType(typeof(Player), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Create([FromBody] Player player)
        {
            if (ModelState.IsValid)
            {
                _playerRepository.AddPlayer(player);
                return CreatedAtAction("Details", new { id = player.Id }, player);
            }
            return BadRequest(new { Message = "Invalid player data.", Errors = ModelState.Values.SelectMany(v => v.Errors) });
        }

    }
}