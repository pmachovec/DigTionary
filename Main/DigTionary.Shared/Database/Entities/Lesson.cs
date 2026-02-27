using DigTionary.Shared.Database.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigTionary.Shared.Database.Entities;

[Table(TableNames.LESSONS)]
public sealed class Lesson
{
    [Column(ColumnNames.ID)]
    public int Id { get; init; }

    [Column(ColumnNames.NAME)]
    public string Name { get; init; } = default!;

    [Column(ColumnNames.CATEGORY_ID)]
    public int CategoryId { get; init; }

    public Category Category { get; init; } = default!;

    public ICollection<Word> Words { get; init; } = [];

    public override bool Equals(object? obj) => obj is Lesson otherLesson && otherLesson.Id == Id;

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Lesson left, Lesson right) => left.Equals(right);

    public static bool operator !=(Lesson left, Lesson right) => !(left == right);
}
