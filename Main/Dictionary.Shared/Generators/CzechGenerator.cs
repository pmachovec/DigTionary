using Dictionary.Shared.Database.Entities;
using Dictionary.Shared.Services;

namespace Dictionary.Shared.Generators;

internal sealed class CzechGenerator(ICzechService _czechService) : ICzechGenerator
{
    private Czech[]? _czechs;
    private int _czechCounter;

    public int CzechsCount =>
        _czechs?.Length ?? throw new InvalidOperationException("Questions no set up!");

    public async Task SetUpCzechsAsync(CancellationToken cancellationToken)
    {
        _czechs = await _czechService.GetCzechsAsync(cancellationToken);
        _czechCounter = _czechs.Length;
    }

    public async Task SetUpCzechsAsync(ISet<int> lessonsIds, CancellationToken cancellationToken)
    {
        if (lessonsIds.Count == 0)
        {
            throw new ArgumentException("Empty lessons IDs!");
        }

        _czechs = await _czechService.GetCzechsAsync(lessonsIds, cancellationToken);
        _czechCounter = _czechs.Length;
    }

    public Czech GetNextCzech()
    {
        if (_czechs is null)
        {
            throw new InvalidOperationException("Czech words no set up!");
        }

        if (_czechCounter <= 0)
        {
            throw new InvalidOperationException("No more Czech words available!");
        }

        // Get random index of the remaining part of the czechs array and then decrement the counter.
        // This is correct, '_czechCounter--' returns the initial value before the decrement.
        var randomIndex = Random.Shared.Next(_czechCounter--);

        // Get the czech at the random index position.
        var czech = _czechs[randomIndex];

        // Swap the czech at the end of the remaining part with the one selected by the random index.
        // For the swap, the counter must be already decremented.
        _czechs[randomIndex] = _czechs[_czechCounter];

        return czech;
    }
}
