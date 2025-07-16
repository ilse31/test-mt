using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface IPlayerRepository
    {
        Task<IEnumerable<Player>> GetPlayersAsync(string? birthplace);
        Task<Player?> GetPlayerAsync(int id);
        Task AddPlayerAsync(Player player);
        Task UpdatePlayerAsync(Player player);
        Task DeletePlayerAsync(int id);
    }
}