using DigTionary.Shared.Domain;
using DigTionary.Shared.Repositories;

namespace DigTionary.Shared.Application;

internal sealed class WordsByTextGenerator(IWordRepository _wordRepository) : IWordsByTextGenerator
{
    private WordsByText[]? _wordsByTexts;
    private int _pointer;

    public int Count =>
        _wordsByTexts?.Length ?? throw new InvalidOperationException("Words not set up!");

    public async Task SetUpAsync(CancellationToken cancellationToken)
    {
        _wordsByTexts = [.. (await _wordRepository.GetWordsWithCzechsByTextAsync(cancellationToken)).Select(keyValue => new WordsByText(keyValue.Key, keyValue.Value))];

        if (_wordsByTexts.Length == 0)
        {
            throw new InvalidDataException("No words with Czech translations available in the database!");
        }

        _pointer = _wordsByTexts.Length;
    }

    public async Task SetUpAsync(ISet<int> lessonsIds, CancellationToken cancellationToken)
    {
        if (lessonsIds.Count == 0)
        {
            throw new ArgumentException("Empty lessons IDs!");
        }

        _wordsByTexts = [.. (await _wordRepository.GetWordsWithCzechsByTextAsync(lessonsIds, cancellationToken)).Select(keyValue => new WordsByText(keyValue.Key, keyValue.Value))];

        if (_wordsByTexts.Length == 0)
        {
            throw new InvalidDataException("No corresponding words with Czech translations available in the database!");
        }

        _pointer = _wordsByTexts.Length;
    }

    public WordsByText GetNext()
    {
        if (_wordsByTexts is null)
        {
            throw new InvalidOperationException("Words not set up!");
        }

        if (_pointer <= 0)
        {
            throw new InvalidOperationException("No previous words available!");
        }

        // Get random index of the remaining part of the words array and then decrement the pointer.
        // This is correct, '_pointer--' returns the initial value before the decrement.
        var randomIndex = Random.Shared.Next(_pointer--);

        // Get the word at the random index position.
        var wordsByText = _wordsByTexts[randomIndex];

        // Move the last word in the remaining part of the array to the position of the returned word.
        _wordsByTexts[randomIndex] = _wordsByTexts[_pointer];

        return wordsByText;
    }
}
