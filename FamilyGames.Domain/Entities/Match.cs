namespace FamilyGames.Domain.Entities;

public class Match
{
    public int Id { get; set; }
    public string GameType { get; set; } = string.Empty;
    public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
    public int? Score { get; set; }
    public bool IsWinner { get; set; }

    // Foreign key – pekar tillbaka på Player
    public int PlayerId { get; set; }
    public Player Player { get; set; } = null!;
}