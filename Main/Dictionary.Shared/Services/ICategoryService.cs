using Dictionary.Shared.Database.Entities;

namespace Dictionary.Shared.Services;

internal interface ICategoryService
{
    ISet<Category> Categories { get; }
}
