using DigTionary.Shared.Database;
using DigTionary.Shared.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigTionary.Shared.Repositories;

internal sealed class WordRepository(DigTionaryDbContext _digTionaryDbContext) : IWordRepository
{
    public async Task<IDictionary<string, IEnumerable<Word>>> GetWordsWithCzechsByTextAsync(CancellationToken cancellationToken)
    {
        if (!_digTionaryDbContext.Words.Any())
        {
            throw new InvalidDataException("No main language words available in the database!");
        }

        // The grouping to dictionary can't be done in one LINQ query.
        // You must first retrieve words and then group them in another step.
        // Otherwise, you will get an error about client evaluation of the grouping during runtime.
        var wordsWithCzechs = await _digTionaryDbContext.Words
            .Include(word => word.Czechs)
            .ToArrayAsync(cancellationToken);

        return wordsWithCzechs
            .GroupBy(word => word.Text)
            .ToDictionary(grouping => grouping.Key, grouping => grouping.AsEnumerable());
    }

    public async Task<IDictionary<string, IEnumerable<Word>>> GetWordsWithCzechsByTextAsync(ISet<int> lessonsIds, CancellationToken cancellationToken)
    {
        if (!_digTionaryDbContext.Words.Any())
        {
            throw new InvalidDataException("No main language words available in the database!");
        }

        var wordsWithCzechs = await _digTionaryDbContext.Words
            .Where(word => lessonsIds.Contains(word.LessonId))
            .Include(word => word.Czechs)
            .ToArrayAsync(cancellationToken);

        return wordsWithCzechs
            .GroupBy(word => word.Text)
            .ToDictionary(grouping => grouping.Key, grouping => grouping.AsEnumerable());
    }
}
