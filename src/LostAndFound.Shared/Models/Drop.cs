namespace LostAndFound.Shared.Models;

public class Drop
{
    public Guid Id { get; set; }
    public DropCategory Category { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime DroppedAt { get; set; }
    public int TimesFound { get; set; }
}
