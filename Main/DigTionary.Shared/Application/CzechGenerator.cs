using DigTionary.Shared.Database.Entities;
using DigTionary.Shared.Repositories;

namespace DigTionary.Shared.Application;

internal sealed class CzechGenerator(ICzechRepository _czechRepository) : ICzechGenerator
{
    private Czech[]? _czechs;
    private int _pointer;

    public int Count =>
        _czechs?.Length ?? throw new InvalidOperationException("Czech words no set up!");

    public async Task SetUpAsync(CancellationToken cancellationToken)
    {
        _czechs = await _czechRepository.GetCzechsWithWordsAsync(cancellationToken);

        if (_czechs.Length == 0)
        {
            throw new InvalidDataException("No Czech words available in the database!");
        }

        _pointer = _czechs.Length;
    }

    public async Task SetUpAsync(ISet<int> lessonsIds, CancellationToken cancellationToken)
    {
        if (lessonsIds.Count == 0)
        {
            throw new ArgumentException("Empty lessons IDs!");
        }

        _czechs = await _czechRepository.GetCzechsWithWordsAsync(lessonsIds, cancellationToken);

        if (_czechs.Length == 0)
        {
            throw new InvalidDataException("No corresponding Czech words available in the database!");
        }

        _pointer = _czechs.Length;
    }

    public Czech GetNext()
    {
        if (_czechs is null)
        {
            throw new InvalidOperationException("Czech words no set up!");
        }

        if (_pointer <= 0)
        {
            throw new InvalidOperationException("No previous Czech words available!");
        }

        // Get random index of the remaining part of the czechs array and then decrement the counter.
        // This is correct, '_czechCounter--' returns the initial value before the decrement.
        var randomIndex = Random.Shared.Next(_pointer--);

        // Get the czech at the random index position.
        var czech = _czechs[randomIndex];

        // Swap the czech at the end of the remaining part with the one selected by the random index.
        // For the swap, the counter must be already decremented.
        _czechs[randomIndex] = _czechs[_pointer];

        return czech;
    }
}
