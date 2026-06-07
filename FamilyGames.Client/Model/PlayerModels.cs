namespace FamilyGames.Client.Models;

public class PlayerModel
{
    public int    Id          { get; set; }
    public string Name        { get; set; } = string.Empty;
    public int    Age         { get; set; }
    public string AvatarEmoji { get; set; } = "🎮";
    public int    TotalMatches{ get; set; }
    public int    Wins        { get; set; }
}

public class MatchModel
{
    public int     Id         { get; set; }
    public string  GameType   { get; set; } = string.Empty;
    public DateTime PlayedAt  { get; set; }
    public string? Notes      { get; set; }
    public int?    Score      { get; set; }
    public bool    IsWinner   { get; set; }
    public int     PlayerId   { get; set; }
    public string  PlayerName { get; set; } = string.Empty;
}