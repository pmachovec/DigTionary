using DigTionary.Shared.Database;
using DigTionary.Shared.Database.Entities;
using DigTionary.Shared.Repositories;
using DigTionary.Shared.Test.Database.Constants;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DigTionary.Shared.Test.Repositories;

[TestFixture]
internal sealed class WordRepositoryTest : IDisposable
{
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

    private DigTionaryDbContext _digTionaryDbContext = default!;
    private WordRepository _wordRepository = default!;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<DigTionaryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _digTionaryDbContext = new(options);
        _wordRepository = new(_digTionaryDbContext);
    }

    [TearDown]
    public void TearDown() => Dispose();

    public void Dispose() => _digTionaryDbContext.Database?.EnsureDeleted();

    [Test]
    public async Task GetWordsWithCzechsAsyncTest_WithoutLessonIds_NoWordsInDb_ShouldThrow() =>
        Assert.That(
            async () => await _wordRepository.GetWordsWithCzechsAsync(CancellationToken.None),
            Throws.TypeOf<InvalidDataException>()
        );

    [Test]
    public async Task GetWordsWithCzechsAsyncTest_NoWordsInDb_ShouldThrow() =>
        Assert.That(
            async () => await _wordRepository.GetWordsWithCzechsAsync(new HashSet<int>() { 1, 2, 3 }, CancellationToken.None),
            Throws.TypeOf<InvalidDataException>()
        );

    [Test]
    public async Task GetWordsWithCzechsAsyncTest_WithoutLessonIds_ShouldReturnAllWords()
    {
        FillDatabase();
        var result = await _wordRepository.GetWordsWithCzechsAsync(CancellationToken.None);
        AssertExpectedWords(result, _wordHorse, _wordGroan, _wordYellowish);
    }

    [Test]
    public async Task GetWordsWithCzechsAsyncTest_OneLessonId_DoesNotExistAmongWordLessonIds_ShouldReturnEmpty()
    {
        FillDatabase();
        var result = await _wordRepository.GetWordsWithCzechsAsync(new HashSet<int>() { 99 }, CancellationToken.None);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(0));
    }

    [Test]
    public async Task GetWordsWithCzechsAsyncTest_MultipleLessonIds_DoNotExistAmongWordLessonIds_ShouldReturnEmpty()
    {
        FillDatabase();
        var result = await _wordRepository.GetWordsWithCzechsAsync(new HashSet<int>() { 97, 98, 99 }, CancellationToken.None);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(0));
    }

    [Test]
    public async Task GetWordsWithCzechsAsyncTest_OneLessonId_ExistsAmongWordLessonIds_ShouldReturnWord()
    {
        FillDatabase();
        var result = await _wordRepository.GetWordsWithCzechsAsync(new HashSet<int>() { 1 }, CancellationToken.None);
        AssertExpectedWords(result, _wordHorse);
    }

    [Test]
    public async Task GetWordsWithCzechsAsyncTest_MultipleLessonIds_OneExistsAmongWordLessonIds_ShouldReturnWord()
    {
        FillDatabase();
        var result = await _wordRepository.GetWordsWithCzechsAsync(new HashSet<int>() { 1, 98, 99 }, CancellationToken.None);
        AssertExpectedWords(result, _wordHorse);
    }

    [Test]
    public async Task GetWordsWithCzechsAsyncTest_MultipleLessonIds_SomeExistAmongWordLessonIds_ShouldReturnCorrespondingWords()
    {
        FillDatabase();
        var result = await _wordRepository.GetWordsWithCzechsAsync(new HashSet<int>() { 1, 2, 99 }, CancellationToken.None);
        AssertExpectedWords(result, _wordHorse, _wordGroan);
    }

    [Test]
    public async Task GetWordsWithCzechsAsyncTest_MultipleLessonIds_AllExistsAmongWordLessonIds_ShouldReturnCorrespondingWords()
    {
        FillDatabase();
        var result = await _wordRepository.GetWordsWithCzechsAsync(new HashSet<int>() { 1, 2, 3 }, CancellationToken.None);
        AssertExpectedWords(result, _wordHorse, _wordGroan, _wordYellowish);
    }

    private void FillDatabase()
    {
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
                }
            );

        _digTionaryDbContext.AddRange(
            _wordHorse,
            _wordGroan,
            _wordYellowish,
            _czechHorse,
            _czechGroan,
            _czechYellowish
        );

        _ = _digTionaryDbContext.SaveChanges();
    }

    private static void AssertExpectedWords(Word[]? result, params Word[] expectedWords)
    {
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Length, Is.EqualTo(expectedWords.Length));

        foreach (var expectedWord in expectedWords)
        {
            Assert.That(result, Does.Contain(expectedWord));
            var matchingWord = result.First(word => word.Id == expectedWord.Id);
            Assert.That(expectedWord, Is.EqualTo(matchingWord).UsingPropertiesComparer());
        }
    }
}
