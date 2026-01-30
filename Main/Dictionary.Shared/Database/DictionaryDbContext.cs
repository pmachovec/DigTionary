using Dictionary.Shared.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Shared.Database;

internal sealed partial class DictionaryDbContext(DbContextOptions<DictionaryDbContext> _options) : DbContext(_options)
{
    public DbSet<Category> Categories { get; set; }

    public DbSet<Czech> Czechs { get; set; }

    public DbSet<Lesson> Lessons { get; set; }

    public DbSet<Word> Words { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Category>(categoryBuilder =>
        {
            _ = categoryBuilder.HasKey(category => category.Id);
            _ = categoryBuilder.Property(category => category.Id).ValueGeneratedNever();
        });

        _ = modelBuilder.Entity<Czech>(czechBuilder =>
        {
            _ = czechBuilder
                .HasKey(czech =>
                    new
                    {
                        czech.Text,
                        czech.WordId
                    }
                );

            _ = czechBuilder
                .HasOne(czech => czech.Word)
                .WithMany(word => word.Czechs)
                .HasForeignKey(czech => czech.WordId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        _ = modelBuilder.Entity<Lesson>(lessonBuilder =>
        {
            _ = lessonBuilder.HasKey(lesson => lesson.Id);
            _ = lessonBuilder.Property(lesson => lesson.Id).ValueGeneratedNever();

            _ = lessonBuilder
                .HasOne(lesson => lesson.Category)
                .WithMany(category => category.Lessons)
                .HasForeignKey(lesson => lesson.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        _ = modelBuilder.Entity<Word>(wordBuilder =>
        {
            _ = wordBuilder.HasKey(word => word.Id);
            _ = wordBuilder.Property(word => word.Id).ValueGeneratedNever();

            _ = wordBuilder
                .HasOne(word => word.Lesson)
                .WithMany(lesson => lesson.Words)
                .HasForeignKey(word => word.LessonId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
