using Dictionary.Shared.Database.Entities;

namespace Dictionary.Shared.Services;

internal interface IWordService
{
    Task<Word[]> GetWordsWithCzechsAsync(CancellationToken cancellationToken);

    Task<Word[]> GetWordsWithCzechsAsync(HashSet<int> lessonsIds, CancellationToken cancellationToken);
}
