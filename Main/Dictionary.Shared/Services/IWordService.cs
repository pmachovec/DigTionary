using Dictionary.Shared.Database.Entities;

namespace Dictionary.Shared.Services;

internal interface IWordService
{
    Task<Word[]> GetWordsAsync(CancellationToken cancellationToken);

    Task<Word[]> GetWordsAsync(ISet<int> lessonsIds, CancellationToken cancellationToken);
}
