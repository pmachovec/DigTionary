using Dictionary.Shared.Database;
using Dictionary.Shared.Database.Entities;
using Dictionary.Shared.Services;
using Dictionary.Shared.Test.Database.Constants;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Dictionary.Shared.Test.Services;

[TestFixture]
internal sealed class CzechServiceTest : IDisposable
{
    private static readonly Czech _czech_horse = new()
    {
        Id = 1,
        Text = "kůň"
    };

    private static readonly Czech _czech_groan = new()
    {
        Id = 2,
        Text = "úpět"
    };

    private static readonly Czech _czech_yellowish = new()
    {
        Id = 3,
        Text = "žluťoučký"
    };

    private static readonly Word _word_horse = new()
    {
        Id = 1,
        Article = "das",
        Text = "Pferd",
        Ipa = "pfeːət",
        Notes = "-(e)s, -e",
        LessonId = 1
    };

    private static readonly Word _word_groan = new()
    {
        Id = 2,
        Text = "stöhnen",
        TextAscii = "stohnen",
        Ipa = "ʃtøːnən",
        LessonId = 2
    };

    private static readonly Word _word_yellowish = new()
    {
        Id = 3,
        Text = "gelblich",
        Ipa = "gɛlbliç",
        LessonId = 3
    };

    private DictionaryDbContext _dictionaryDbContext = default!;
    private CzechService _czechService = default!;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<DictionaryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dictionaryDbContext = new(options);

        // Establish the relationship table in the in-memory database.
        // Must be done before adding entities to the database.
        _dictionaryDbContext
            .Set<Dictionary<string, object>>(TableNames.WORDS_CZECHS)
            .AddRange(
                new Dictionary<string, object>
                {
                    [ColumnNames.WORD_ID] = _word_groan.Id,
                    [ColumnNames.CZECH_ID] = _czech_groan.Id
                },
                new Dictionary<string, object>
                {
                    [ColumnNames.WORD_ID] = _word_horse.Id,
                    [ColumnNames.CZECH_ID] = _czech_horse.Id
                },
                new Dictionary<string, object>
                {
                    [ColumnNames.WORD_ID] = _word_yellowish.Id,
                    [ColumnNames.CZECH_ID] = _czech_yellowish.Id
                }
            );

        _ = _dictionaryDbContext.Add(_czech_horse);
        _ = _dictionaryDbContext.Add(_czech_groan);
        _ = _dictionaryDbContext.Add(_czech_yellowish);
        _ = _dictionaryDbContext.Add(_word_horse);
        _ = _dictionaryDbContext.Add(_word_groan);
        _ = _dictionaryDbContext.Add(_word_yellowish);
        _ = _dictionaryDbContext.SaveChanges();
        _czechService = new(_dictionaryDbContext);
    }

    [TearDown]
    public void TearDown() => Dispose();

    public void Dispose() => _dictionaryDbContext.Database?.EnsureDeleted();

    [Test]
    public async Task GetCzechsAsyncTest_WithoutLessonIds_ShouldReturnAllCzechs()
    {
        var result = await _czechService.GetCzechsAsync(CancellationToken.None);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(3));

        var expectedCzechs = new[] { _czech_horse, _czech_groan, _czech_yellowish };

        foreach (var expectedCzech in expectedCzechs)
        {
            Assert.That(result, Does.Contain(expectedCzech));
            var matchingCzech = result.First(czech => czech.Id == expectedCzech.Id);
            Assert.That(expectedCzech, Is.EqualTo(matchingCzech).UsingPropertiesComparer());
        }
    }

    [Test]
    public async Task GetCzechsAsyncTest_OneLessonId_DoesNotExistAmongWordLessonIds_ShouldReturnEmpty()
    {
        var result = await _czechService.GetCzechsAsync(new HashSet<int>() { 99 }, CancellationToken.None);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(0));
    }

    [Test]
    public async Task GetCzechsAsyncTest_MultipleLessonIds_DoNotExistAmongWordLessonIds_ShouldReturnEmpty()
    {
        var result = await _czechService.GetCzechsAsync(new HashSet<int>() { 97, 98, 99 }, CancellationToken.None);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(0));
    }

    [Test]
    public async Task GetCzechsAsyncTest_OneLessonId_ExistsAmongWordLessonIds_ShouldReturnCzech()
    {
        var result = await _czechService.GetCzechsAsync(new HashSet<int>() { 1 }, CancellationToken.None);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(1));
        Assert.That(result[0], Is.EqualTo(_czech_horse).UsingPropertiesComparer());
    }

    [Test]
    public async Task GetCzechsAsyncTest_MultipleLessonIds_OneExistsAmongWordLessonIds_ShouldReturnCzech()
    {
        var result = await _czechService.GetCzechsAsync(new HashSet<int>() { 1, 98, 99 }, CancellationToken.None);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(1));
        Assert.That(result[0], Is.EqualTo(_czech_horse).UsingPropertiesComparer());
    }

    [Test]
    public async Task GetCzechsAsyncTest_MultipleLessonIds_SomeExistAmongWordLessonIds_ShouldReturnCorrespondingCzechs()
    {
        var result = await _czechService.GetCzechsAsync(new HashSet<int>() { 1, 2, 99 }, CancellationToken.None);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(2));

        var expectedCzechs = new[] { _czech_horse, _czech_groan };

        foreach (var expectedCzech in expectedCzechs)
        {
            Assert.That(result, Does.Contain(expectedCzech));
            var matchingCzech = result.First(czech => czech.Id == expectedCzech.Id);
            Assert.That(expectedCzech, Is.EqualTo(matchingCzech).UsingPropertiesComparer());
        }
    }

    [Test]
    public async Task GetCzechsAsyncTest_MultipleLessonIds_AllExistsAmongWordLessonIds_ShouldReturnCorrespondingCzechs()
    {
        var result = await _czechService.GetCzechsAsync(new HashSet<int>() { 1, 2, 3, 97, 98, 99 }, CancellationToken.None);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(3));

        Czech[] expectedCzechs = [_czech_horse, _czech_groan, _czech_yellowish];

        foreach (var expectedCzech in expectedCzechs)
        {
            Assert.That(result, Does.Contain(expectedCzech));
            var matchingCzech = result.First(czech => czech.Id == expectedCzech.Id);
            Assert.That(expectedCzech, Is.EqualTo(matchingCzech).UsingPropertiesComparer());
        }
    }
}
