using Dictionary.Shared.Database.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dictionary.Shared.Database.Entities;

[Table(TableNames.CZECHS)]
public sealed class Czech
{
    [Column(ColumnNames.TEXT)]
    public string Text { get; init; } = default!;

    [Column(ColumnNames.WORD_ID)]
    public int WordId { get; init; }

    public Word Word { get; init; } = default!;
}
