using LostAndFound.Api.Data;
using LostAndFound.Shared.DTOs;
using LostAndFound.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DropsController : ControllerBase
{
    private readonly AppDbContext _context;
    private static readonly Random _random = new();

    public DropsController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Drop a new thought, secret, advice, or story anonymously
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<DropResponse>> CreateDrop([FromBody] CreateDropRequest request)
    {
        if (!Enum.TryParse<DropCategory>(request.Category, true, out var category))
        {
            return BadRequest("Invalid category. Must be: Thought, Secret, Advice, or Story");
        }

        var drop = new Drop
        {
            Id = Guid.NewGuid(),
            Content = request.Content.Trim(),
            Category = category,
            DroppedAt = DateTime.UtcNow,
            TimesFound = 0
        };

        _context.Drops.Add(drop);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(FindRandom), new { id = drop.Id }, ToResponse(drop));
    }

    /// <summary>
    /// Find a random drop
    /// </summary>
    [HttpGet("random")]
    public async Task<ActionResult<DropResponse>> FindRandom([FromQuery] string? category = null)
    {
        IQueryable<Drop> query = _context.Drops;

        if (!string.IsNullOrEmpty(category) && Enum.TryParse<DropCategory>(category, true, out var cat))
        {
            query = query.Where(d => d.Category == cat);
        }

        var count = await query.CountAsync();
        if (count == 0)
        {
            return NotFound("No drops found. Be the first to leave something behind!");
        }

        var randomIndex = _random.Next(count);
        var drop = await query.Skip(randomIndex).FirstAsync();

        // Increment times found
        drop.TimesFound++;
        await _context.SaveChangesAsync();

        return Ok(ToResponse(drop));
    }

    /// <summary>
    /// Get statistics about drops
    /// </summary>
    [HttpGet("stats")]
    public async Task<ActionResult<object>> GetStats()
    {
        var stats = await _context.Drops
            .GroupBy(d => d.Category)
            .Select(g => new { Category = g.Key.ToString(), Count = g.Count() })
            .ToListAsync();

        var total = await _context.Drops.CountAsync();
        var totalFound = await _context.Drops.SumAsync(d => d.TimesFound);

        return Ok(new
        {
            TotalDrops = total,
            TotalFinds = totalFound,
            ByCategory = stats
        });
    }

    private static DropResponse ToResponse(Drop drop) => new()
    {
        Id = drop.Id,
        Category = drop.Category.ToString(),
        Content = drop.Content,
        DroppedAt = drop.DroppedAt,
        TimesFound = drop.TimesFound
    };
}
