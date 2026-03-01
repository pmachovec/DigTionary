using DigTionary.Shared.Domain;

namespace DigTionary.Shared.Application;

internal interface IWordsByTextGenerator
{
    int Count { get; }

    Task SetUpAsync(CancellationToken cancellationToken);

    Task SetUpAsync(ISet<int> lessonsIds, CancellationToken cancellationToken);

    WordsByText GetNext();
}
