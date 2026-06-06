namespace FamilyGames.Application.DTOs;

public class MatchDto
{
    public int Id { get; set; }
    public string GameType { get; set; } = string.Empty;
    public DateTime PlayedAt { get; set; }
    public string? Notes { get; set; }
    public int? Score { get; set; }
    public bool IsWinner { get; set; }
    public int PlayerId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
}

public class CreateMatchDto
{
    public string GameType { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int? Score { get; set; }
    public bool IsWinner { get; set; }
    public int PlayerId { get; set; }
}

public class UpdateMatchDto
{
    public string GameType { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int? Score { get; set; }
    public bool IsWinner { get; set; }
}