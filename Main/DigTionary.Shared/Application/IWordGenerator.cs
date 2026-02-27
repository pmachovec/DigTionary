using DigTionary.Shared.Database.Entities;

namespace DigTionary.Shared.Application;

internal interface IWordGenerator
{
    Task SetUpWordsAsync(CancellationToken cancellationToken);

    Task SetUpWordsAsync(ISet<int> lessonsIds, CancellationToken cancellationToken);

    Word GetNextWord();
}
