using DigTionary.Shared.Database;
using DigTionary.Shared.Database.Entities;
using DigTionary.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DigTionary.Shared.Test.Repositories;

[TestFixture]
internal sealed class CategoryRepositoryTest : IDisposable
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

    private DigTionaryDbContext _digTionaryDbContext = default!;
    private CategoryRepository _categoryRepository = default!;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<DigTionaryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _digTionaryDbContext = new(options);
        _categoryRepository = new(_digTionaryDbContext);
    }

    [TearDown]
    public void TearDown() => Dispose();

    public void Dispose() => _digTionaryDbContext.Database?.EnsureDeleted();

    [Test]
    public async Task GetCategoriesWithLessonsAsyncTest_NoCategoriesInDb_ShouldThrow() =>
        Assert.That(
            async () => await _categoryRepository.GetCategoriesWithLessonsAsync(CancellationToken.None),
            Throws.TypeOf<InvalidDataException>()
        );

    [Test]
    public async Task GetCategoriesWithLessonsAsyncTest_NoCategoriesWithLessonsInDb_ShouldReturnEmpty()
    {
        _digTionaryDbContext.AddRange(
            _categoryWithoutLessons1,
            _categoryWithoutLessons2,
            _categoryWithoutLessons3
        );

        _ = _digTionaryDbContext.SaveChanges();
        var result = await _categoryRepository.GetCategoriesWithLessonsAsync(CancellationToken.None);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(0));
    }

    [Test]
    public async Task GetCategoriesWithLessonsAsyncTest_OneCategoryWithLessonsInDb_ShouldReturnCategory()
    {
        _digTionaryDbContext.AddRange(
            _categoryWithLessons1,
            _categoryWithoutLessons1,
            _categoryWithoutLessons2,
            _categoryWithoutLessons3,
            _lesson1
        );

        _ = _digTionaryDbContext.SaveChanges();
        var result = await _categoryRepository.GetCategoriesWithLessonsAsync(CancellationToken.None);
        AssertExpectedCategories(result, _categoryWithLessons1);
    }

    [Test]
    public async Task GetCategoriesWithLessonsAsyncTest_MultipleCategoriesWithLessonsInDb_ShouldReturnCorrespondingCategories()
    {
        _digTionaryDbContext.AddRange(
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

        _ = _digTionaryDbContext.SaveChanges();
        var result = await _categoryRepository.GetCategoriesWithLessonsAsync(CancellationToken.None);
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
