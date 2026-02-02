using Dictionary.Shared.Database.Entities;

namespace Dictionary.Shared.Generators;

internal interface ICzechGenerator
{
    Task SetUpCzechsAsync(CancellationToken cancellationToken);

    Task SetUpCzechsAsync(HashSet<int> lessonsIds, CancellationToken cancellationToken);

    Czech GetNextCzech();
}
