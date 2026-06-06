namespace FamilyGames.Tests.Services;

using FamilyGames.Application.DTOs;
using FamilyGames.Application.Services;
using FamilyGames.Domain.Entities;
using FamilyGames.Domain.Interfaces;
using FluentAssertions;
using NSubstitute;

public class MatchServiceTests
{
    private readonly IMatchRepository  _matchRepo;
    private readonly IPlayerRepository _playerRepo;
    private readonly MatchService      _sut;

    public MatchServiceTests()
    {
        _matchRepo  = Substitute.For<IMatchRepository>();
        _playerRepo = Substitute.For<IPlayerRepository>();
        _sut = new MatchService(_matchRepo, _playerRepo);
    }

    
    // Test 9 – Exception: Skapa match för spelare som inte finns
    [Fact]
    public async Task CreateMatchAsync_PlayerNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange – spelare med id 99 finns inte
        _playerRepo.GetByIdAsync(99).Returns((Player?)null);
        var dto = new CreateMatchDto { GameType = "Tennis", PlayerId = 99 };

        // Act
        var act = async () => await _sut.CreateMatchAsync(dto);

        // Assert – KeyNotFoundException med rätt spelar-id i meddelandet
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*99*");
    }

    // Test 10 – Validering: Tomt GameType kastar ArgumentException
    
    [Fact]
    public async Task CreateMatchAsync_EmptyGameType_ThrowsArgumentException()
    {
        // Arrange
        var dto = new CreateMatchDto { GameType = "", PlayerId = 1 };

        // Act
        var act = async () => await _sut.CreateMatchAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Game type*");
    }
}