namespace FamilyGames.Application.Services;

using FamilyGames.Application.DTOs;
using FamilyGames.Application.Interfaces;
using FamilyGames.Domain.Entities;
using FamilyGames.Domain.Interfaces;

public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IPlayerRepository _playerRepository;

    public MatchService(IMatchRepository matchRepository,
                        IPlayerRepository playerRepository)
    {
        _matchRepository = matchRepository;
        _playerRepository = playerRepository;
    }

    public async Task<IEnumerable<MatchDto>> GetAllMatchesAsync()
    {
        var matches = await _matchRepository.GetAllAsync();
        return matches.Select(MapToDto);
    }

    public async Task<MatchDto?> GetMatchByIdAsync(int id)
    {
        var match = await _matchRepository.GetByIdAsync(id);
        return match is null ? null : MapToDto(match);
    }

    public async Task<IEnumerable<MatchDto>> GetMatchesByPlayerAsync(int playerId)
    {
        var matches = await _matchRepository.GetMatchesByPlayerAsync(playerId);
        return matches.Select(MapToDto);
    }

    public async Task<MatchDto> CreateMatchAsync(CreateMatchDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.GameType))
            throw new ArgumentException("Game type cannot be empty.");

        // Kontrollera att spelaren finns – kasta annars KeyNotFoundException
        var player = await _playerRepository.GetByIdAsync(dto.PlayerId)
            ?? throw new KeyNotFoundException(
                   $"Player with id {dto.PlayerId} not found.");

        var match = new Match
        {
            GameType  = dto.GameType.Trim(),
            Notes     = dto.Notes,
            Score     = dto.Score,
            IsWinner  = dto.IsWinner,
            PlayerId  = dto.PlayerId,
            Player    = player,
            PlayedAt  = DateTime.UtcNow
        };

        await _matchRepository.AddAsync(match);
        return MapToDto(match);
    }

    public async Task<MatchDto?> UpdateMatchAsync(int id, UpdateMatchDto dto)
    {
        var match = await _matchRepository.GetByIdAsync(id);
        if (match is null) return null;

        if (string.IsNullOrWhiteSpace(dto.GameType))
            throw new ArgumentException("Game type cannot be empty.");

        match.GameType = dto.GameType.Trim();
        match.Notes    = dto.Notes;
        match.Score    = dto.Score;
        match.IsWinner = dto.IsWinner;

        await _matchRepository.UpdateAsync(match);
        return MapToDto(match);
    }

    public async Task<bool> DeleteMatchAsync(int id)
    {
        var match = await _matchRepository.GetByIdAsync(id);
        if (match is null) return false;

        await _matchRepository.DeleteAsync(id);
        return true;
    }

    private static MatchDto MapToDto(Match m) => new()
    {
        Id         = m.Id,
        GameType   = m.GameType,
        PlayedAt   = m.PlayedAt,
        Notes      = m.Notes,
        Score      = m.Score,
        IsWinner   = m.IsWinner,
        PlayerId   = m.PlayerId,
        PlayerName = m.Player?.Name ?? string.Empty
    };
}