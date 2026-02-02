using Dictionary.Shared.Database;
using Dictionary.Shared.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Shared.Services;

internal sealed class CategoryService(DictionaryDbContext _dictionaryDbContext) : ICategoryService
{
    public async Task<IEnumerable<Category>> GetCategoriesWithLessonsAsync(CancellationToken cancellationToken)
    {
        if (!_dictionaryDbContext.Categories.Any())
        {
            throw new InvalidDataException("No categories available in the database!");
        }

        var categoriesWithLessons = await _dictionaryDbContext
            .Categories
            .Where(category => category.Lessons.Count > 0)
            .Include(category => category.Lessons)
            .ToArrayAsync(cancellationToken);

        return categoriesWithLessons.Length > 0
            ? categoriesWithLessons
            : throw new InvalidDataException("No categories with lessons available in the database!");
    }
}
