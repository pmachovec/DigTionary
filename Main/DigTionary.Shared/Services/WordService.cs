using DigTionary.Shared.Database;
using DigTionary.Shared.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigTionary.Shared.Services;

internal sealed class WordService(DigTionaryDbContext _digTionaryDbContext) : IWordService
{
    public async Task<Word[]> GetWordsWithCzechsAsync(CancellationToken cancellationToken)
    {
        if (!_digTionaryDbContext.Words.Any())
        {
            throw new InvalidDataException("No main language words available in the database!");
        }

        return await _digTionaryDbContext.Words
            .Include(word => word.Czechs)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Word[]> GetWordsWithCzechsAsync(ISet<int> lessonsIds, CancellationToken cancellationToken){
        if (!_digTionaryDbContext.Words.Any())
        {
            throw new InvalidDataException("No main language words available in the database!");
        }

        return await _digTionaryDbContext.Words
            .Where(word => lessonsIds.Contains(word.LessonId))
            .Include(word => word.Czechs)
            .ToArrayAsync(cancellationToken);
    }
}
