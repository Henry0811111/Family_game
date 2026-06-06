namespace FamilyGames.Application.Interfaces;

using FamilyGames.Application.DTOs;

public interface IPlayerService
{
    Task<IEnumerable<PlayerDto>> GetAllPlayersAsync();
    Task<PlayerDto?> GetPlayerByIdAsync(int id);
    Task<PlayerDto> CreatePlayerAsync(CreatePlayerDto dto);
    Task<PlayerDto?> UpdatePlayerAsync(int id, UpdatePlayerDto dto);
    Task<bool> DeletePlayerAsync(int id);
}