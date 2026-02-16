using Dictionary.Shared.Database;
using Dictionary.Shared.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Shared.Services;

internal sealed class CzechService(DictionaryDbContext _dictionaryDbContext) : ICzechService
{
    public async Task<Czech[]> GetCzechsWithWordsAsync(CancellationToken cancellationToken)
    {
        if (!_dictionaryDbContext.Czechs.Any())
        {
            throw new InvalidDataException("No Czech words available in the database!");
        }

        return await _dictionaryDbContext.Czechs
            .Include(czech => czech.Words)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Czech[]> GetCzechsWithWordsAsync(ISet<int> lessonsIds, CancellationToken cancellationToken)
    {
        if (!_dictionaryDbContext.Czechs.Any())
        {
            throw new InvalidDataException("No Czech words available in the database!");
        }

        return await _dictionaryDbContext.Czechs
            .Include(czech => czech.Words)
            .Where(czech => czech.Words.Any(word => lessonsIds.Contains(word.LessonId)))
            .ToArrayAsync(cancellationToken);
    }
}
