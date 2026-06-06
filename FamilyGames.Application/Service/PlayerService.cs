namespace FamilyGames.Application.Services;

using FamilyGames.Application.DTOs;
using FamilyGames.Application.Interfaces;
using FamilyGames.Domain.Entities;
using FamilyGames.Domain.Interfaces;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;

    // IPlayerRepository injiceras – inte den konkreta klassen
    public PlayerService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<IEnumerable<PlayerDto>> GetAllPlayersAsync()
    {
        var players = await _playerRepository.GetAllWithMatchesAsync();
        return players.Select(MapToDto);
    }

    public async Task<PlayerDto?> GetPlayerByIdAsync(int id)
    {
        var player = await _playerRepository.GetPlayerWithMatchesAsync(id);
        return player is null ? null : MapToDto(player);
    }

    public async Task<PlayerDto> CreatePlayerAsync(CreatePlayerDto dto)
    {
        // Validering – kastas som exception, fångas i controllern
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Player name cannot be empty.");

        if (dto.Age < 1 || dto.Age > 120)
            throw new ArgumentException("Age must be between 1 and 120.");

        var player = new Player
        {
            Name = dto.Name.Trim(),
            Age = dto.Age,
            AvatarEmoji = dto.AvatarEmoji
        };

        await _playerRepository.AddAsync(player);
        return MapToDto(player);
    }

    public async Task<PlayerDto?> UpdatePlayerAsync(int id, UpdatePlayerDto dto)
    {
        var player = await _playerRepository.GetByIdAsync(id);
        if (player is null) return null;

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Player name cannot be empty.");

        player.Name = dto.Name.Trim();
        player.Age = dto.Age;
        player.AvatarEmoji = dto.AvatarEmoji;

        await _playerRepository.UpdateAsync(player);
        return MapToDto(player);
    }

    public async Task<bool> DeletePlayerAsync(int id)
    {
        var player = await _playerRepository.GetByIdAsync(id);
        if (player is null) return false;

        await _playerRepository.DeleteAsync(id);
        return true;
    }

    // Privat hjälpmetod – mappar Entity → DTO
    private static PlayerDto MapToDto(Player p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Age = p.Age,
        AvatarEmoji = p.AvatarEmoji,
        TotalMatches = p.Matches?.Count ?? 0,
        Wins = p.Matches?.Count(m => m.IsWinner) ?? 0
    };
}