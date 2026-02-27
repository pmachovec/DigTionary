using DigTionary.Shared.Database;
using DigTionary.Shared.Database.Entities;
using DigTionary.Shared.Services;
using DigTionary.Shared.Test.Database.Constants;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DigTionary.Shared.Test.Services;

[TestFixture]
internal sealed class CzechServiceTest : IDisposable
{
    private static readonly Czech _czechHorse = new()
    {
        Id = 1,
        Text = "kůň"
    };

    private static readonly Czech _czechGroan = new()
    {
        Id = 2,
        Text = "úpět"
    };

    private static readonly Czech _czechYellowish = new()
    {
        Id = 3,
        Text = "žluťoučký"
    };

    private static readonly Czech _czechNote1 = new()
    {
        Id = 4,
        Text = "známka"
    };

    private static readonly Czech _czechNote2 = new()
    {
        Id = 5,
        Text = "nota"
    };

    private static readonly Czech _czechNote3 = new()
    {
        Id = 6,
        Text = "zpráva"
    };

    private static readonly Word _wordHorse = new()
    {
        Id = 1,
        Article = "das",
        Text = "Pferd",
        Ipa = "pfeːət",
        Notes = "-(e)s, -e",
        LessonId = 1
    };

    private static readonly Word _wordGroan = new()
    {
        Id = 2,
        Text = "stöhnen",
        TextAscii = "stohnen",
        Ipa = "ʃtøːnən",
        LessonId = 2
    };

    private static readonly Word _wordYellowish = new()
    {
        Id = 3,
        Text = "gelblich",
        Ipa = "gɛlbliç",
        LessonId = 3
    };

    private static readonly Word _wordNote1 = new()
    {
        Id = 4,
        Article = "die",
        Text = "Note",
        Ipa = "no:tə",
        Notes = "-, -n",
        LessonId = 4
    };

    private static readonly Word _wordNote2 = new()
    {
        Id = 5,
        Article = "der",
        Text = "Bericht",
        Ipa = "bəriçt",
        Notes = "-(e)s, -e",
        LessonId = 5
    };

    private static readonly Word _wordNote3 = new()
    {
        Id = 6,
        Article = "das",
        Text = "Tonzeichen",
        Ipa = "tɔntsajçən",
        Notes = "-s, -",
        LessonId = 6
    };

    private DigTionaryDbContext _digTionaryDbContext = default!;
    private CzechService _czechService = default!;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<DigTionaryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _digTionaryDbContext = new(options);
        _czechService = new(_digTionaryDbContext);
    }

    [TearDown]
    public void TearDown() => Dispose();

    public void Dispose() => _digTionaryDbContext.Database?.EnsureDeleted();

    [Test]
    public async Task GetCzechsWithWordsAsyncTest_WithoutLessonIds_NoCzechsInDb_ShouldThrow() =>
        Assert.That(
            async () => await _czechService.GetCzechsWithWordsAsync(CancellationToken.None),
            Throws.TypeOf<InvalidDataException>()
        );

    [Test]
    public async Task GetCzechsWithWordsAsyncTest_NoCzechsInDb_ShouldThrow() =>
        Assert.That(
            async () => await _czechService.GetCzechsWithWordsAsync(new HashSet<int>() { 1, 2, 3 }, CancellationToken.None),
            Throws.TypeOf<InvalidDataException>()
        );

    [Test]
    public async Task GetCzechsWithWordsAsyncTest_WithoutLessonIds_ShouldReturnAllCzechs()
    {
        FillDatabase();
        var result = await _czechService.GetCzechsWithWordsAsync(CancellationToken.None);
        AssertExpectedCzechs(result, _czechHorse, _czechGroan, _czechYellowish, _czechNote1, _czechNote2, _czechNote3);
    }

    [Test]
    public async Task GetCzechsWithWordsAsyncTest_OneLessonId_DoesNotExistAmongWordLessonIds_ShouldReturnEmpty()
    {
        FillDatabase();
        var result = await _czechService.GetCzechsWithWordsAsync(new HashSet<int>() { 99 }, CancellationToken.None);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(0));
    }

    [Test]
    public async Task GetCzechsWithWordsAsyncTest_MultipleLessonIds_DoNotExistAmongWordLessonIds_ShouldReturnEmpty()
    {
        FillDatabase();
        var result = await _czechService.GetCzechsWithWordsAsync(new HashSet<int>() { 97, 98, 99 }, CancellationToken.None);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(0));
    }

    [Test]
    public async Task GetCzechsWithWordsAsyncTest_OneLessonId_ExistsAmongWordLessonIds_ShouldReturnCzech()
    {
        FillDatabase();
        var result = await _czechService.GetCzechsWithWordsAsync(new HashSet<int>() { 1 }, CancellationToken.None);
        AssertExpectedCzechs(result, _czechHorse);
    }

    [Test]
    public async Task GetCzechsWithWordsAsyncTest_MultipleLessonIds_OneExistsAmongWordLessonIds_ShouldReturnCzech()
    {
        FillDatabase();
        var result = await _czechService.GetCzechsWithWordsAsync(new HashSet<int>() { 1, 98, 99 }, CancellationToken.None);
        AssertExpectedCzechs(result, _czechHorse);
    }

    [Test]
    public async Task GetCzechsWithWordsAsyncTest_MultipleLessonIds_SomeExistAmongWordLessonIds_ShouldReturnCorrespondingCzechs()
    {
        FillDatabase();
        var result = await _czechService.GetCzechsWithWordsAsync(new HashSet<int>() { 1, 2, 99 }, CancellationToken.None);
        AssertExpectedCzechs(result, _czechHorse, _czechGroan);
    }

    [Test]
    public async Task GetCzechsWithWordsAsyncTest_MultipleLessonIds_AllExistsAmongWordLessonIds_ShouldReturnCorrespondingCzechs()
    {
        FillDatabase();
        var result = await _czechService.GetCzechsWithWordsAsync(new HashSet<int>() { 1, 2, 3 }, CancellationToken.None);
        AssertExpectedCzechs(result, _czechHorse, _czechGroan, _czechYellowish);
    }

    [Test]
    public async Task GetCzechsWithWordsAsyncTest_SingleLessonId_MatchingMultipleWords_ShouldReturnCorrespondingCzechs()
    {
        FillDatabase();
        var result = await _czechService.GetCzechsWithWordsAsync(new HashSet<int>() { 4 }, CancellationToken.None);
        AssertExpectedCzechs(result, _czechNote1, _czechNote2, _czechNote3);
    }

    [Test]
    public async Task GetCzechsWithWordsAsyncTest_MultipleLessonIds_SomeMatchingMultipleWords_ShouldReturnCorrespondingCzechs()
    {
        FillDatabase();
        var result = await _czechService.GetCzechsWithWordsAsync(new HashSet<int>() { 4, 5, 6, 98, 99 }, CancellationToken.None);
        AssertExpectedCzechs(result, _czechNote1, _czechNote2, _czechNote3);
    }

    private void FillDatabase()
    {
        // Establish the relationship table in the in-memory database.
        // Must be done before adding entities to the database.
        _digTionaryDbContext
            .Set<Dictionary<string, object>>(TableNames.WORDS_CZECHS)
            .AddRange(
                new Dictionary<string, object>
                {
                    [ColumnNames.WORD_ID] = _wordGroan.Id,
                    [ColumnNames.CZECH_ID] = _czechGroan.Id
                },
                new Dictionary<string, object>
                {
                    [ColumnNames.WORD_ID] = _wordHorse.Id,
                    [ColumnNames.CZECH_ID] = _czechHorse.Id
                },
                new Dictionary<string, object>
                {
                    [ColumnNames.WORD_ID] = _wordYellowish.Id,
                    [ColumnNames.CZECH_ID] = _czechYellowish.Id
                },
                new Dictionary<string, object>
                {
                    [ColumnNames.WORD_ID] = _wordNote1.Id,
                    [ColumnNames.CZECH_ID] = _czechNote1.Id
                },
                new Dictionary<string, object>
                {
                    [ColumnNames.WORD_ID] = _wordNote1.Id,
                    [ColumnNames.CZECH_ID] = _czechNote2.Id
                },
                new Dictionary<string, object>
                {
                    [ColumnNames.WORD_ID] = _wordNote1.Id,
                    [ColumnNames.CZECH_ID] = _czechNote3.Id
                },
                new Dictionary<string, object>
                {
                    [ColumnNames.WORD_ID] = _wordNote2.Id,
                    [ColumnNames.CZECH_ID] = _czechNote1.Id
                },
                new Dictionary<string, object>
                {
                    [ColumnNames.WORD_ID] = _wordNote2.Id,
                    [ColumnNames.CZECH_ID] = _czechNote2.Id
                },
                new Dictionary<string, object>
                {
                    [ColumnNames.WORD_ID] = _wordNote2.Id,
                    [ColumnNames.CZECH_ID] = _czechNote3.Id
                },
                new Dictionary<string, object>
                {
                    [ColumnNames.WORD_ID] = _wordNote3.Id,
                    [ColumnNames.CZECH_ID] = _czechNote1.Id
                },
                new Dictionary<string, object>
                {
                    [ColumnNames.WORD_ID] = _wordNote3.Id,
                    [ColumnNames.CZECH_ID] = _czechNote2.Id
                },
                new Dictionary<string, object>
                {
                    [ColumnNames.WORD_ID] = _wordNote3.Id,
                    [ColumnNames.CZECH_ID] = _czechNote3.Id
                }
            );

        _digTionaryDbContext.AddRange(
            _czechHorse,
            _czechGroan,
            _czechYellowish,
            _czechNote1,
            _czechNote2,
            _czechNote3,
            _wordHorse,
            _wordGroan,
            _wordYellowish,
            _wordNote1,
            _wordNote2,
            _wordNote3
        );

        _ = _digTionaryDbContext.SaveChanges();
    }

    private static void AssertExpectedCzechs(Czech[]? result, params Czech[] expectedCzechs)
    {
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Length, Is.EqualTo(expectedCzechs.Length));

        foreach (var expectedCzech in expectedCzechs)
        {
            Assert.That(result, Does.Contain(expectedCzech));
            var matchingCzech = result.First(czech => czech.Id == expectedCzech.Id);
            Assert.That(expectedCzech, Is.EqualTo(matchingCzech).UsingPropertiesComparer());
        }
    }
}
