using DigTionary.Shared.Database;
using DigTionary.Shared.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigTionary.Shared.Repositories;

internal sealed class CategoryRepository(DigTionaryDbContext _digTionaryDbContext) : ICategoryRepository
{
    public async Task<Category[]> GetCategoriesWithLessonsAsync(CancellationToken cancellationToken)
    {
        if (!_digTionaryDbContext.Categories.Any())
        {
            throw new InvalidDataException("No categories available in the database!");
        }

        return await _digTionaryDbContext.Categories
            .Where(category => category.Lessons.Count > 0)
            .Include(category => category.Lessons)
            .ToArrayAsync(cancellationToken);
    }
}
