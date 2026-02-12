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

    public async Task<Czech[]> GetCzechsAsync(HashSet<int> lessonsIds, CancellationToken cancellationToken) =>
        await _dictionaryDbContext.Czechs
            .Include(czech => czech.Words)
            .Where(czech => lessonsIds.Any(lessonId => czech.Words.Select(word => word.LessonId).Contains(lessonId)))
            .ToArrayAsync(cancellationToken);
}
