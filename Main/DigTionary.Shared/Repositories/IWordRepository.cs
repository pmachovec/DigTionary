using DigTionary.Shared.Database.Entities;

namespace DigTionary.Shared.Repositories;

internal interface IWordRepository
{
    Task<Word[]> GetWordsWithCzechsAsync(CancellationToken cancellationToken);

    Task<Word[]> GetWordsWithCzechsAsync(ISet<int> lessonsIds, CancellationToken cancellationToken);
}
