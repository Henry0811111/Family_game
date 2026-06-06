namespace FamilyGames.Application.Interfaces;

using FamilyGames.Application.DTOs;

public interface IMatchService
{
    Task<IEnumerable<MatchDto>> GetAllMatchesAsync();
    Task<MatchDto?> GetMatchByIdAsync(int id);
    Task<IEnumerable<MatchDto>> GetMatchesByPlayerAsync(int playerId);
    Task<MatchDto> CreateMatchAsync(CreateMatchDto dto);
    Task<MatchDto?> UpdateMatchAsync(int id, UpdateMatchDto dto);
    Task<bool> DeleteMatchAsync(int id);
}