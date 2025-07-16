using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
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

        [HttpGet]
        public async Task<ApiResponse> GetAllPlayers(string? birthplace = null)
        {
            var players = await _playerRepository.GetPlayersAsync(birthplace);
            return ApiResponse.Success(players);
        }

        [HttpGet("sample")]
        public ApiResponse GetSamplePlayers()
        {
            var samplePlayers = new List<Player>
            {
                new Player { Id = 1, Name = "aefesaf", Score = 1, BirthPlace = null },
                new Player { Id = 2, Name = "string", Score = 0, BirthPlace = "awesfsefpioseufeis8f" }
            };
    
            return ApiResponse.Success(samplePlayers);
        }

        [HttpPost]
        public async Task<ApiResponse> AddPlayer([FromBody] Player player)
        {
            await _playerRepository.AddPlayerAsync(player);
            return ApiResponse.Success(player);
        }
    }
}
