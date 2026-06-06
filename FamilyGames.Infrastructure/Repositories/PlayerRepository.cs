namespace FamilyGames.Infrastructure.Repositories;

using FamilyGames.Domain.Entities;
using FamilyGames.Domain.Interfaces;
using FamilyGames.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
{
    public PlayerRepository(AppDbContext context) : base(context) { }

   
    public async Task<Player?> GetPlayerWithMatchesAsync(int playerId) =>
        await _context.Players
            .Include(p => p.Matches)
            .FirstOrDefaultAsync(p => p.Id == playerId);

    
    public async Task<IEnumerable<Player>> GetAllWithMatchesAsync() =>
        await _context.Players
            .Include(p => p.Matches)
            .ToListAsync();
}