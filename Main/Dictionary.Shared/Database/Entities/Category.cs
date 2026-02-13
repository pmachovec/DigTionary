using Dictionary.Shared.Database.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dictionary.Shared.Database.Entities;

[Table(TableNames.CATEGORIES)]
public sealed class Category
{
    [Column(ColumnNames.ID)]
    public int Id { get; init; }

    [Column(ColumnNames.NAME)]
    public string Name { get; init; } = default!;

    public ICollection<Lesson> Lessons { get; init; } = [];

    public override bool Equals(object? obj) => obj is Category otherCategory && otherCategory.Id == Id;

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Category left, Category right) => left.Equals(right);

    public static bool operator !=(Category left, Category right) => !(left == right);
}
