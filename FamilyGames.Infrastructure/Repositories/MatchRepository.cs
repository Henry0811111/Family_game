namespace FamilyGames.Infrastructure.Repositories;

using FamilyGames.Domain.Entities;
using FamilyGames.Domain.Interfaces;
using FamilyGames.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class MatchRepository : GenericRepository<Match>, IMatchRepository
{
    public MatchRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Match>> GetMatchesByPlayerAsync(int playerId) =>
        await _context.Matches
            .Include(m => m.Player)
            .Where(m => m.PlayerId == playerId)
            .ToListAsync();

    public override async Task<IEnumerable<Match>> GetAllAsync() =>
        await _context.Matches
            .Include(m => m.Player)
            .ToListAsync();
}