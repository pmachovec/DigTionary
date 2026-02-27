using DigTionary.Shared.Database.Entities;

namespace DigTionary.Shared.Services;

internal interface ICzechService
{
    Task<Czech[]> GetCzechsWithWordsAsync(CancellationToken cancellationToken);

    Task<Czech[]> GetCzechsWithWordsAsync(ISet<int> lessonsIds, CancellationToken cancellationToken);
}
