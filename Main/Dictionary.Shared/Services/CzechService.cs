using Dictionary.Shared.Database;
using Dictionary.Shared.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Shared.Services;

internal sealed class CzechService(DictionaryDbContext _dictionaryDbContext) : ICzechService
{
    public async Task<Czech[]> GetCzechsAsync(CancellationToken cancellationToken) =>
        await _dictionaryDbContext.Czechs
            .Include(czech => czech.Word)
            .ToArrayAsync(cancellationToken);

    public async Task<Czech[]> GetCzechsAsync(HashSet<int> lessonsIds, CancellationToken cancellationToken) =>
        await _dictionaryDbContext.Czechs
            .Include(czech => czech.Word)
            .Where(czech => lessonsIds.Contains(czech.Word.LessonId))
            .ToArrayAsync(cancellationToken);
}
