using Dictionary.Shared.Database.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dictionary.Shared.Database.Entities;

[Table(TableNames.LESSONS)]
internal sealed class Lesson
{
    [Column(ColumnNames.ID)]
    public int Id { get; init; }

    [Column(ColumnNames.NAME)]
    public string Name { get; init; } = default!;

    [Column(ColumnNames.CATEGORY_ID)]
    public int CategoryId { get; init; }

    public Category Category { get; init; } = default!;

    public ICollection<Word> Words { get; init; } = [];
}
