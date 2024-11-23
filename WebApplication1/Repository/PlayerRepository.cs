using Models;

namespace Repository
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly List<Player> _players;

        public PlayerRepository()
        {
            _players = new List<Player>
            {
                new Player { Id = 1, Name = "Cristiano Ronaldo", Age = 38, BirthPlace = "Europe" },
                new Player { Id = 2, Name = "Lionel Messi", Age = 36, BirthPlace = "South America" },
                new Player { Id = 3, Name = "Karim Benzema", Age = 35, BirthPlace = "Europe" },
                new Player { Id = 4, Name = "Erling Haaland", Age = 23, BirthPlace = "Europe" },
                new Player { Id = 5, Name = "Kylian Mbappe", Age = 24, BirthPlace = "Europe" }
            };
        }


        public IEnumerable<Player> GetPlayers(string birthplace)
        {
            if (string.IsNullOrEmpty(birthplace))
            {
                return _players;
            }

            return _players.Where(p => p.BirthPlace.ToLower() == birthplace.ToLower());
        }

        public Player GetPlayer(int id)
        {
            return _players.FirstOrDefault(p => p.Id == id);
        }

        public void AddPlayer(Player player)
        {
            player.Id = _players.Max(p => p.Id) + 1;
            _players.Add(player);
        }


        public void UpdatePlayer(Player player)
        {
            var existingPlayer = _players.FirstOrDefault(p => p.Id == player.Id);
            if (existingPlayer != null)
            {
                existingPlayer.Name = player.Name;
                existingPlayer.Age = player.Age;
                existingPlayer.BirthPlace = player.BirthPlace;
            }
        }

        public void DeletePlayer(int id)
        {
            var player = _players.FirstOrDefault(p => p.Id == id);
            if (player != null)
            {
                _players.Remove(player);
            }
        }
    }
}