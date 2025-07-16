using Dapper;
using Npgsql;
using System.Text.Json;
using System.IO;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Repository
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly QueryService _queryService;
        private readonly ConnectionService _connectionService;

        public PlayerRepository(ApplicationDbContext context, QueryService queryService, ConnectionService connectionService)
        {
            _context = context;
            _queryService = queryService;
            _connectionService = connectionService;
        }

        public async Task<IEnumerable<Player>> GetPlayersAsync(string? birthplace)
        {
            return await _context.Players
                .Where(p => string.IsNullOrEmpty(birthplace) || p.BirthPlace == birthplace)
                .ToListAsync();
        }

        public async Task<Player?> GetPlayerAsync(int id)
        {
            return await _context.Players.FindAsync(id);
        }

        public async Task AddPlayerAsync(Player player)
        {
            await _context.Players.AddAsync(player);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePlayerAsync(Player player)
        {
            await _context.ExecuteInTransactionAsync(async () => 
            {
                _context.Players.Update(player);
                await _context.SaveChangesAsync();
            });
        }

        public async Task DeletePlayerAsync(int id)
        {
            await _context.ExecuteInTransactionAsync(async () => 
            {
                var player = await _context.Players.FindAsync(id);
                if (player != null)
                {
                    _context.Players.Remove(player);
                    await _context.SaveChangesAsync();
                }
            });
        }
    }
}