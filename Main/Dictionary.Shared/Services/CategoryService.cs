using Dictionary.Shared.Database;
using Dictionary.Shared.Database.Entities;

namespace Dictionary.Shared.Services;

internal sealed class CategoryService(DictionaryDbContext _dictionaryDbContext) : ICategoryService
{
    public ISet<Category> Categories => _dictionaryDbContext.Categories.ToHashSet();
}
