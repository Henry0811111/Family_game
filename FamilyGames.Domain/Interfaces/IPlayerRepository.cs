namespace FamilyGames.Domain.Interfaces;

using FamilyGames.Domain.Entities;

// Ärver alla CRUD-metoder från IGenericRepository
// Lägger till Player-specifika metoder
public interface IPlayerRepository : IGenericRepository<Player>
{
    Task<Player?> GetPlayerWithMatchesAsync(int playerId);
    Task<IEnumerable<Player>> GetAllWithMatchesAsync();
}