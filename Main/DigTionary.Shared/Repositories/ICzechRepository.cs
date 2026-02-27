using DigTionary.Shared.Database.Entities;

namespace DigTionary.Shared.Repositories;

internal interface ICzechRepository
{
    Task<Czech[]> GetCzechsWithWordsAsync(CancellationToken cancellationToken);

    Task<Czech[]> GetCzechsWithWordsAsync(ISet<int> lessonsIds, CancellationToken cancellationToken);
}
