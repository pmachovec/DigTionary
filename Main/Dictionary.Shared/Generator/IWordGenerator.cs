using Dictionary.Shared.Database.Entities;

namespace Dictionary.Shared.Generator;

internal interface IWordGenerator
{
    Task SetUpWordsAsync(CancellationToken cancellationToken);

    Task SetUpWordsAsync(HashSet<int> lessonsIds, CancellationToken cancellationToken);

    Word GetNextWord();
}
