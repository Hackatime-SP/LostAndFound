using LostAndFound.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Drop> Drops => Set<Drop>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Drop>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Category).HasConversion<string>();
        });
    }

    public void SeedData()
    {
        if (Drops.Any()) return;

        var seedDrops = new List<Drop>
        {
            // Thoughts
            new() { Id = Guid.NewGuid(), Category = DropCategory.Thought, Content = "Sometimes I wonder if the person I'm meant to be friends with walked past me today and I didn't even notice.", DroppedAt = DateTime.UtcNow.AddDays(-5), TimesFound = 3 },
            new() { Id = Guid.NewGuid(), Category = DropCategory.Thought, Content = "The best conversations happen at 2 AM when everyone's too tired to pretend.", DroppedAt = DateTime.UtcNow.AddDays(-3), TimesFound = 7 },
            new() { Id = Guid.NewGuid(), Category = DropCategory.Thought, Content = "I realized today that home isn't a place. It's the feeling when someone truly understands you.", DroppedAt = DateTime.UtcNow.AddHours(-12), TimesFound = 2 },
            
            // Secrets
            new() { Id = Guid.NewGuid(), Category = DropCategory.Secret, Content = "I've been learning to play piano in secret for two years. Next month, I'm going to surprise my mom with her favorite song.", DroppedAt = DateTime.UtcNow.AddDays(-7), TimesFound = 15 },
            new() { Id = Guid.NewGuid(), Category = DropCategory.Secret, Content = "I pretend to hate romantic movies, but I've watched 'Pride and Prejudice' at least 30 times.", DroppedAt = DateTime.UtcNow.AddDays(-2), TimesFound = 8 },
            new() { Id = Guid.NewGuid(), Category = DropCategory.Secret, Content = "I still sleep with my childhood stuffed animal. I'm 34.", DroppedAt = DateTime.UtcNow.AddDays(-1), TimesFound = 4 },
            
            // Advice
            new() { Id = Guid.NewGuid(), Category = DropCategory.Advice, Content = "Call your parents. Even if it's awkward. Even if it's brief. One day you won't be able to.", DroppedAt = DateTime.UtcNow.AddDays(-10), TimesFound = 42 },
            new() { Id = Guid.NewGuid(), Category = DropCategory.Advice, Content = "The version of you that you're embarrassed of from 5 years ago? That person was doing their best. Be kind to them.", DroppedAt = DateTime.UtcNow.AddDays(-4), TimesFound = 23 },
            new() { Id = Guid.NewGuid(), Category = DropCategory.Advice, Content = "You don't have to set yourself on fire to keep others warm.", DroppedAt = DateTime.UtcNow.AddHours(-6), TimesFound = 11 },
            
            // Stories
            new() { Id = Guid.NewGuid(), Category = DropCategory.Story, Content = "When I was 7, I got lost at a fair. A stranger bought me ice cream and helped me find my mom. I never got to thank them. Now I try to be that person for others.", DroppedAt = DateTime.UtcNow.AddDays(-14), TimesFound = 19 },
            new() { Id = Guid.NewGuid(), Category = DropCategory.Story, Content = "My grandfather taught me chess before he passed. Every time I play, I feel like he's sitting across from me, smiling at my mistakes.", DroppedAt = DateTime.UtcNow.AddDays(-6), TimesFound = 12 },
            new() { Id = Guid.NewGuid(), Category = DropCategory.Story, Content = "I met my best friend because we both reached for the last book on a shelf at the same time. That was 15 years ago. We're getting matching tattoos next week.", DroppedAt = DateTime.UtcNow.AddHours(-18), TimesFound = 6 }
        };

        Drops.AddRange(seedDrops);
        SaveChanges();
    }
}
