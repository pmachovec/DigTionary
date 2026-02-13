using Dictionary.Shared.Database.Entities;

namespace Dictionary.Shared.Services;

internal interface ICzechService
{
    Task<Czech[]> GetCzechsAsync(CancellationToken cancellationToken);

    Task<Czech[]> GetCzechsAsync(ISet<int> lessonsIds, CancellationToken cancellationToken);
}
