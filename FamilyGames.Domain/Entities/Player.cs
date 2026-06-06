namespace FamilyGames.Domain.Entities;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string AvatarEmoji { get; set; } = "🎮";

    // En Player kan ha många Matches (1-många relation)
    public ICollection<Match> Matches { get; set; } = new List<Match>();
}