using DigTionary.Shared.Database.Entities;

namespace DigTionary.Shared.Repositories;

internal interface IWordRepository
{
    Task<IDictionary<string, IEnumerable<Word>>> GetWordsWithCzechsByTextAsync(CancellationToken cancellationToken);

    Task<IDictionary<string, IEnumerable<Word>>> GetWordsWithCzechsByTextAsync(ISet<int> lessonsIds, CancellationToken cancellationToken);
}
