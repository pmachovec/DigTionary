using Dictionary.Shared.Database;
using Dictionary.Shared.Database.Entities;
using Dictionary.Shared.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Dictionary.Shared.Test.Services;

[TestFixture]
internal sealed class CategoryServiceTest : IDisposable
{
    private static readonly Category _categoryWithLessons1 = new()
    {
        Id = 1,
        Name = "Cvičebnice německé slovní zásoby"
    };

    private static readonly Category _categoryWithLessons2 = new()
    {
        Id = 2,
        Name = "Sprechen Sie Deutsch?"
    };

    private static readonly Category _categoryWithLessons3 = new()
    {
        Id = 3,
        Name = "Harry Potter und der Stein der Weisen"
    };

    private static readonly Category _categoryWithoutLessons1 = new()
    {
        Id = 4,
        Name = "Schule"
    };

    private static readonly Category _categoryWithoutLessons2 = new()
    {
        Id = 5,
        Name = "Arbeit"
    };

    private static readonly Category _categoryWithoutLessons3 = new()
    {
        Id = 6,
        Name = "Leben"
    };

    private static readonly Lesson _lesson1 = new()
    {
        Id = 1,
        Name = "Familie",
        CategoryId = _categoryWithLessons1.Id,
        Category = _categoryWithLessons1
    };

    private static readonly Lesson _lesson2 = new()
    {
        Id = 2,
        Name = "Machen etwas",
        CategoryId = _categoryWithLessons2.Id,
        Category = _categoryWithLessons2
    };

    private static readonly Lesson _lesson3 = new()
    {
        Id = 3,
        Name = "Der Junge, der überlebte",
        CategoryId = _categoryWithLessons3.Id,
        Category = _categoryWithLessons3
    };

    private DictionaryDbContext _dictionaryDbContext = default!;
    private CategoryService _categoryService = default!;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<DictionaryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dictionaryDbContext = new(options);
        _categoryService = new(_dictionaryDbContext);
    }

    [TearDown]
    public void TearDown() => Dispose();

    public void Dispose() => _dictionaryDbContext.Database?.EnsureDeleted();

    [Test]
    public async Task GetCategoriesWithLessonsAsyncTest_NoCategoriesInDb_ShouldThrow() =>
        Assert.That(
            async () => await _categoryService.GetCategoriesWithLessonsAsync(CancellationToken.None),
            Throws.TypeOf<InvalidDataException>()
        );

    [Test]
    public async Task GetCategoriesWithLessonsAsyncTest_NoCategoriesWithLessonsInDb_ShouldReturnEmpty()
    {
        _dictionaryDbContext.AddRange(
            _categoryWithoutLessons1,
            _categoryWithoutLessons2,
            _categoryWithoutLessons3
        );

        _ = _dictionaryDbContext.SaveChanges();
        var result = await _categoryService.GetCategoriesWithLessonsAsync(CancellationToken.None);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(0));
    }

    [Test]
    public async Task GetCategoriesWithLessonsAsyncTest_OneCategoryWithLessonsInDb_ShouldReturnCategory()
    {
        _dictionaryDbContext.AddRange(
            _categoryWithLessons1,
            _categoryWithoutLessons1,
            _categoryWithoutLessons2,
            _categoryWithoutLessons3,
            _lesson1
        );

        _ = _dictionaryDbContext.SaveChanges();
        var result = await _categoryService.GetCategoriesWithLessonsAsync(CancellationToken.None);
        AssertExpectedCategories(result, _categoryWithLessons1);
    }

    [Test]
    public async Task GetCategoriesWithLessonsAsyncTest_MultipleCategoriesWithLessonsInDb_ShouldReturnCorrespondingCategories()
    {
        _dictionaryDbContext.AddRange(
            _categoryWithLessons1,
            _categoryWithLessons2,
            _categoryWithLessons3,
            _categoryWithoutLessons1,
            _categoryWithoutLessons2,
            _categoryWithoutLessons3,
            _lesson1,
            _lesson2,
            _lesson3
        );

        _ = _dictionaryDbContext.SaveChanges();
        var result = await _categoryService.GetCategoriesWithLessonsAsync(CancellationToken.None);
        AssertExpectedCategories(result, _categoryWithLessons1, _categoryWithLessons2, _categoryWithLessons3);
    }

    private static void AssertExpectedCategories(Category[]? result, params Category[] expectedCategories)
    {
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Length, Is.EqualTo(expectedCategories.Length));

        foreach (var expectedCategory in expectedCategories)
        {
            Assert.That(result, Does.Contain(expectedCategory));
            var matchingCategory = result.First(category => category.Id == expectedCategory.Id);
            Assert.That(expectedCategory, Is.EqualTo(matchingCategory).UsingPropertiesComparer());
        }
    }
}
