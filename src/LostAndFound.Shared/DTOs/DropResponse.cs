namespace LostAndFound.Shared.DTOs;

public class DropResponse
{
    public Guid Id { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime DroppedAt { get; set; }
    public int TimesFound { get; set; }
}
