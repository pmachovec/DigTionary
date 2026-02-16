using Dictionary.Shared.Database;
using Dictionary.Shared.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Shared.Services;

internal sealed class WordService(DictionaryDbContext _dictionaryDbContext) : IWordService
{
    public async Task<Word[]> GetWordsWithCzechsAsync(CancellationToken cancellationToken)
    {
        if (!_dictionaryDbContext.Words.Any())
        {
            throw new InvalidDataException("No main language words available in the database!");
        }

        return await _dictionaryDbContext.Words
            .Include(word => word.Czechs)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Word[]> GetWordsWithCzechsAsync(ISet<int> lessonsIds, CancellationToken cancellationToken){
        if (!_dictionaryDbContext.Words.Any())
        {
            throw new InvalidDataException("No main language words available in the database!");
        }

        return await _dictionaryDbContext.Words
            .Where(word => lessonsIds.Contains(word.LessonId))
            .Include(word => word.Czechs)
            .ToArrayAsync(cancellationToken);
    }
}
