namespace FamilyGames.Domain.Interfaces;

using FamilyGames.Domain.Entities;

// Ärver CRUD + lägger till filtrering per spelare
public interface IMatchRepository : IGenericRepository<Match>
{
    Task<IEnumerable<Match>> GetMatchesByPlayerAsync(int playerId);
}