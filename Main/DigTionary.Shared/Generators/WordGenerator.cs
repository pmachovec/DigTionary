using DigTionary.Shared.Database.Entities;
using DigTionary.Shared.Repositories;

namespace DigTionary.Shared.Generators;

internal sealed class WordGenerator(IWordRepository _wordRepository) : IWordGenerator
{
    private Word[]? _words;
    private int _wordCounter;

    public async Task SetUpWordsAsync(CancellationToken cancellationToken)
    {
        _words = await _wordRepository.GetWordsWithCzechsAsync(cancellationToken);
        _wordCounter = _words.Length;
    }

    public async Task SetUpWordsAsync(ISet<int> lessonsIds, CancellationToken cancellationToken)
    {
        if (lessonsIds.Count == 0)
        {
            throw new ArgumentException("Empty lessons IDs!");
        }

        _words = await _wordRepository.GetWordsWithCzechsAsync(lessonsIds, cancellationToken);
        _wordCounter = _words.Length;
    }

    public Word GetNextWord()
    {
        if (_words is null)
        {
            throw new InvalidOperationException("Words no set up!");
        }

        if (_wordCounter <= 0)
        {
            throw new InvalidOperationException("No more words available!");
        }

        // Get random index of the remaining part of the words array and then decrement the counter.
        // This is correct, '_wordCounter--' returns the initial value before the decrement.
        var randomIndex = Random.Shared.Next(_wordCounter--);

        // Get the word at the random index position.
        var word = _words[randomIndex];

        // Swap the word at the end of the remaining part with the one selected by the random index.
        // For the swap, the word counter must be already decremented.
        _words[randomIndex] = _words[_wordCounter];

        return word;
    }
}
