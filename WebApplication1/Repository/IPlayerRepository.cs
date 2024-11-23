using Models;

namespace Repository
{
    public interface IPlayerRepository
    {
        IEnumerable<Player> GetPlayers(string birthplace);
        Player GetPlayer(int id);
        void AddPlayer(Player player);
        void UpdatePlayer(Player player);
        void DeletePlayer(int id);
    }
}