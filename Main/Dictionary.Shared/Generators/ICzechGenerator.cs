using Dictionary.Shared.Database.Entities;

namespace Dictionary.Shared.Generators;

internal interface ICzechGenerator
{
    int CzechsCount { get; }

    Task SetUpCzechsAsync(CancellationToken cancellationToken);

    Task SetUpCzechsAsync(HashSet<int> lessonsIds, CancellationToken cancellationToken);

    Czech GetNextCzech();
}
