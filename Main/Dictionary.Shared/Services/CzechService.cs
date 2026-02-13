using Dictionary.Shared.Database;
using Dictionary.Shared.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Shared.Services;

internal sealed class CzechService(DictionaryDbContext _dictionaryDbContext) : ICzechService
{
    public async Task<Czech[]> GetCzechsAsync(CancellationToken cancellationToken) =>
        await _dictionaryDbContext.Czechs
            .Include(czech => czech.Words)
            .ToArrayAsync(cancellationToken);

    public async Task<Czech[]> GetCzechsAsync(ISet<int> lessonsIds, CancellationToken cancellationToken) =>
        await _dictionaryDbContext.Czechs
            .Include(czech => czech.Words)
            .Where(czech => czech.Words.Any(word => lessonsIds.Contains(word.LessonId)))
            .ToArrayAsync(cancellationToken);
}
