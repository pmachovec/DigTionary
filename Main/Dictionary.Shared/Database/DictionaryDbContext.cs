using Dictionary.Shared.Database.Constants;
using Dictionary.Shared.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            _ = czechBuilder.HasKey(czech => czech.Id);
            _ = czechBuilder.Property(czech => czech.Id).ValueGeneratedNever();

            _ = czechBuilder
                .HasMany(czech => czech.Words)
                .WithMany(word => word.Czechs)
                .UsingEntity<Dictionary<string, object>>(
                    TableNames.WORDS_CZECHS,
                    ConfigureWord,
                    ConfigureCzech,
                    ConfigureJoin
                );
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

            _ = wordBuilder
                .HasMany(word => word.Czechs)
                .WithMany(czech => czech.Words)
                .UsingEntity<Dictionary<string, object>>(
                    TableNames.WORDS_CZECHS,
                    ConfigureCzech,
                    ConfigureWord,
                    ConfigureJoin
                );
        });
    }

    private ReferenceCollectionBuilder<Czech, Dictionary<string, object>> ConfigureCzech(EntityTypeBuilder<Dictionary<string, object>> builder) =>
        builder
            .HasOne<Czech>()
            .WithMany()
            .HasForeignKey(ColumnNames.CZECH_ID)
            .OnDelete(DeleteBehavior.Restrict);

    private ReferenceCollectionBuilder<Word, Dictionary<string, object>> ConfigureWord(EntityTypeBuilder<Dictionary<string, object>> builder) =>
        builder
            .HasOne<Word>()
            .WithMany()
            .HasForeignKey(ColumnNames.WORD_ID)
            .OnDelete(DeleteBehavior.Restrict);

    private void ConfigureJoin(EntityTypeBuilder<Dictionary<string, object>> builder)
    {
        _ = builder.HasKey(ColumnNames.WORD_ID, ColumnNames.CZECH_ID);
        _ = builder.ToTable(TableNames.WORDS_CZECHS);
    }
}
