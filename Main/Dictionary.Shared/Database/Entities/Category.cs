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
}
