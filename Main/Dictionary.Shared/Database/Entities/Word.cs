using Dictionary.Shared.Database.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dictionary.Shared.Database.Entities;

[Table(TableNames.WORDS)]
public sealed class Word
{
    [Column(ColumnNames.ID)]
    public int Id { get; init; }

    [Column(ColumnNames.ARTICLE)]
    public string? Article { get; init; }

    [Column(ColumnNames.TEXT)]
    public string Text { get; init; } = default!;

    [Column(ColumnNames.TEXT_ASCII)]
    public string? WordAscii { get; init; }

    [Column(ColumnNames.NOTES)]
    public string? Notes { get; init; }

    [Column(ColumnNames.IPA)]
    public string Ipa { get; init; } = default!;

    [Column(ColumnNames.LESSON_ID)]
    public int LessonId { get; init; }

    public Lesson Lesson { get; init; } = default!;

    public ICollection<Czech> Czechs { get; init; } = [];
}
