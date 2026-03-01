using DigTionary.Shared.Database.Entities;

namespace DigTionary.Shared.Domain;

internal sealed class WordsByText(string text, IEnumerable<Word> words)
{
    public string Text { get; } = text;

    public IEnumerable<Word> Words { get; } = words;

    public override bool Equals(object? obj) =>
        obj is WordsByText otherWordsByText
        && otherWordsByText.Text == Text;

    public override int GetHashCode() => Text.GetHashCode();

    public static bool operator ==(WordsByText left, WordsByText right) => left.Equals(right);

    public static bool operator !=(WordsByText left, WordsByText right) => !(left == right);
}
