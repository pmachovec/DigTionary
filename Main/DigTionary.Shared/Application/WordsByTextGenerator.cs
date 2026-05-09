using DigTionary.Shared.Domain;

namespace DigTionary.Shared.Application;

internal sealed class WordsByTextGenerator
{
    private WordsByText[]? _wordsByTexts;
    private int _pointer;

    public int Count =>
        _wordsByTexts?.Length ?? throw new InvalidOperationException("Words not set up!");
}
