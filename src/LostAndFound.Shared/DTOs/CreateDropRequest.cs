using System.ComponentModel.DataAnnotations;

namespace LostAndFound.Shared.DTOs;

public class CreateDropRequest
{
    [Required]
    [StringLength(2000, MinimumLength = 1, ErrorMessage = "Content must be between 1 and 2000 characters")]
    public string Content { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = string.Empty;
}
