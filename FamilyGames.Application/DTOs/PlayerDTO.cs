namespace FamilyGames.Application.DTOs;

public class PlayerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string AvatarEmoji { get; set; } = "🎮";
    public int TotalMatches { get; set; }
    public int Wins { get; set; }
}

public class CreatePlayerDto
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string AvatarEmoji { get; set; } = "🎮";
}

public class UpdatePlayerDto
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string AvatarEmoji { get; set; } = "🎮";
}