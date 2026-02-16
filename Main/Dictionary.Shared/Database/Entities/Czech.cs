using Dictionary.Shared.Database.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dictionary.Shared.Database.Entities;

[Table(TableNames.CZECHS)]
public sealed class Czech
{
    [Column(ColumnNames.ID)]
    public int Id { get; init; }

    [Column(ColumnNames.TEXT)]
    public string Text { get; init; } = default!;

    public ICollection<Word> Words { get; init; } = [];

    public override bool Equals(object? obj) =>
        obj is Czech otherCzech
        && otherCzech.Text == Text;

    public override int GetHashCode() => Text.GetHashCode();

    public static bool operator ==(Czech left, Czech right) => left.Equals(right);

    public static bool operator !=(Czech left, Czech right) => !(left == right);
}
