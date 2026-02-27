using DigTionary.Shared.Database;
using DigTionary.Shared.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigTionary.Shared.Services;

internal sealed class CzechService(DigTionaryDbContext _digTionaryDbContext) : ICzechService
{
    public async Task<Czech[]> GetCzechsWithWordsAsync(CancellationToken cancellationToken)
    {
        if (!_digTionaryDbContext.Czechs.Any())
        {
            throw new InvalidDataException("No Czech words available in the database!");
        }

        return await _digTionaryDbContext.Czechs
            .Include(czech => czech.Words)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<Czech[]> GetCzechsWithWordsAsync(ISet<int> lessonsIds, CancellationToken cancellationToken)
    {
        if (!_digTionaryDbContext.Czechs.Any())
        {
            throw new InvalidDataException("No Czech words available in the database!");
        }

        return await _digTionaryDbContext.Czechs
            .Include(czech => czech.Words)
            .Where(czech => czech.Words.Any(word => lessonsIds.Contains(word.LessonId)))
            .ToArrayAsync(cancellationToken);
    }
}
