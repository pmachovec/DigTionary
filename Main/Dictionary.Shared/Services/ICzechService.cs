using Dictionary.Shared.Database.Entities;

namespace Dictionary.Shared.Services;

internal interface ICzechService
{
    Task<Czech[]> GetCzechsWithWordsAsync(CancellationToken cancellationToken);

    Task<Czech[]> GetCzechsWithWordsAsync(ISet<int> lessonsIds, CancellationToken cancellationToken);
}
