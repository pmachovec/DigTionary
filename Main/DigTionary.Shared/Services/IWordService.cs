using DigTionary.Shared.Database.Entities;

namespace DigTionary.Shared.Services;

internal interface IWordService
{
    Task<Word[]> GetWordsWithCzechsAsync(CancellationToken cancellationToken);

    Task<Word[]> GetWordsWithCzechsAsync(ISet<int> lessonsIds, CancellationToken cancellationToken);
}
