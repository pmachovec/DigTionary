using DigTionary.Shared.Database.Entities;

namespace DigTionary.Shared.Generators;

internal interface ICzechGenerator
{
    int CzechsCount { get; }

    Task SetUpCzechsAsync(CancellationToken cancellationToken);

    Task SetUpCzechsAsync(ISet<int> lessonsIds, CancellationToken cancellationToken);

    Czech GetNextCzech();
}
