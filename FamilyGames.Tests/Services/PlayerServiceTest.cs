namespace FamilyGames.Tests.Services;

using FamilyGames.Application.DTOs;
using FamilyGames.Application.Services;
using FamilyGames.Domain.Entities;
using FamilyGames.Domain.Interfaces;
using FluentAssertions;
using NSubstitute;

public class PlayerServiceTests
{
    private readonly IPlayerRepository _playerRepo;
    private readonly PlayerService _sut; // sut = System Under Test

    public PlayerServiceTests()
    {
        // Skapa en fejk-version av repository med NSubstitute
        _playerRepo = Substitute.For<IPlayerRepository>();
        _sut = new PlayerService(_playerRepo);
    }

    // ═══════════════════════════════════════════════════════════
    // Test 1 – Happy path: GetAll returnerar mappade DTOs
    // ═══════════════════════════════════════════════════════════
    [Fact]
    public async Task GetAllPlayersAsync_WhenPlayersExist_ReturnsMappedDtos()
    {
        // Arrange – sätt upp testdata och mocka repository
        var players = new List<Player>
        {
            new() { Id = 1, Name = "Alice", Age = 12,
                    Matches = new List<Match> { new() { IsWinner = true } } },
            new() { Id = 2, Name = "Bob", Age = 45,
                    Matches = new List<Match>() }
        };
        _playerRepo.GetAllWithMatchesAsync().Returns(players);

        // Act – anropa metoden som testas
        var result = await _sut.GetAllPlayersAsync();

        // Assert – kontrollera att resultatet är korrekt
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Alice");
        result.First().Wins.Should().Be(1);
        result.First().TotalMatches.Should().Be(1);
    }

    // ═══════════════════════════════════════════════════════════
    // Test 2 – Happy path: Skapa spelare med giltig data
    // ═══════════════════════════════════════════════════════════
    [Fact]
    public async Task CreatePlayerAsync_WithValidData_ReturnsCreatedDto()
    {
        // Arrange
        var dto = new CreatePlayerDto
            { Name = "Charlie", Age = 10, AvatarEmoji = "👦" };

        // Act
        var result = await _sut.CreatePlayerAsync(dto);

        // Assert – kontrollera DTO och att AddAsync anropades exakt en gång
        result.Name.Should().Be("Charlie");
        result.Age.Should().Be(10);
        await _playerRepo.Received(1).AddAsync(
            Arg.Is<Player>(p => p.Name == "Charlie"));
    }

    // ═══════════════════════════════════════════════════════════
    // Test 3 – Edge case: Tomt namn kastar ArgumentException
    // ═══════════════════════════════════════════════════════════
    [Fact]
    public async Task CreatePlayerAsync_WithEmptyName_ThrowsArgumentException()
    {
        // Arrange
        var dto = new CreatePlayerDto { Name = "   ", Age = 10 };

        // Act
        var act = async () => await _sut.CreatePlayerAsync(dto);

        // Assert – förvänta oss ett exception med rätt meddelande
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*name*");
    }

    // ═══════════════════════════════════════════════════════════
    // Test 4 – Validering: Ogiltig ålder kastar ArgumentException
    // ═══════════════════════════════════════════════════════════
    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(121)]
    public async Task CreatePlayerAsync_WithInvalidAge_ThrowsArgumentException(int age)
    {
        // Arrange – Theory kör testet tre gånger med olika åldrar
        var dto = new CreatePlayerDto { Name = "Test", Age = age };

        // Act
        var act = async () => await _sut.CreatePlayerAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Age*");
    }

    // ═══════════════════════════════════════════════════════════
    // Test 5 – Happy path: Uppdatera befintlig spelare
    // ═══════════════════════════════════════════════════════════
    [Fact]
    public async Task UpdatePlayerAsync_WithValidId_ReturnsUpdatedDto()
    {
        // Arrange
        var existing = new Player { Id = 1, Name = "Gammalt Namn", Age = 30 };
        _playerRepo.GetByIdAsync(1).Returns(existing);
        var dto = new UpdatePlayerDto
            { Name = "Nytt Namn", Age = 31, AvatarEmoji = "😎" };

        // Act
        var result = await _sut.UpdatePlayerAsync(1, dto);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Nytt Namn");
        await _playerRepo.Received(1).UpdateAsync(
            Arg.Is<Player>(p => p.Name == "Nytt Namn"));
    }

    // ═══════════════════════════════════════════════════════════
    // Test 6 – Edge case: Uppdatera spelare som inte finns → null
    // ═══════════════════════════════════════════════════════════
    [Fact]
    public async Task UpdatePlayerAsync_WithNonExistentId_ReturnsNull()
    {
        // Arrange – repository returnerar null för id 999
        _playerRepo.GetByIdAsync(999).Returns((Player?)null);
        var dto = new UpdatePlayerDto { Name = "Ghost", Age = 20 };

        // Act
        var result = await _sut.UpdatePlayerAsync(999, dto);

        // Assert – null returneras och UpdateAsync anropas aldrig
        result.Should().BeNull();
        await _playerRepo.DidNotReceive().UpdateAsync(Arg.Any<Player>());
    }

    // ═══════════════════════════════════════════════════════════
    // Test 7 – Happy path: Radera befintlig spelare → true
    // ═══════════════════════════════════════════════════════════
    [Fact]
    public async Task DeletePlayerAsync_WithExistingId_ReturnsTrueAndDeletes()
    {
        // Arrange
        var player = new Player { Id = 1, Name = "Alice" };
        _playerRepo.GetByIdAsync(1).Returns(player);

        // Act
        var result = await _sut.DeletePlayerAsync(1);

        // Assert
        result.Should().BeTrue();
        await _playerRepo.Received(1).DeleteAsync(1);
    }

    // ═══════════════════════════════════════════════════════════
    // Test 8 – Edge case: Radera spelare som inte finns → false
    // ═══════════════════════════════════════════════════════════
    [Fact]
    public async Task DeletePlayerAsync_WithNonExistentId_ReturnsFalse()
    {
        // Arrange
        _playerRepo.GetByIdAsync(999).Returns((Player?)null);

        // Act
        var result = await _sut.DeletePlayerAsync(999);

        // Assert – false returneras och DeleteAsync anropas aldrig
        result.Should().BeFalse();
        await _playerRepo.DidNotReceive().DeleteAsync(Arg.Any<int>());
    }
}