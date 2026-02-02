using Dictionary.Shared.Database.Entities;

namespace Dictionary.Shared.Services;

internal interface ICzechService
{
    Task<Czech[]> GetCzechsAsync(CancellationToken cancellationToken);

    Task<Czech[]> GetCzechsAsync(HashSet<int> lessonsIds, CancellationToken cancellationToken);
}
