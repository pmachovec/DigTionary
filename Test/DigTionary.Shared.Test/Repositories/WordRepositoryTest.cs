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
    private const string PFERD = "Pferd";
    private const string STRAUSS = "Strauß";
    private const string STRAUSS_ASCII = "Strauss";

    private static readonly Word _wordStrauss1 = new()
    {
        Id = 1,
        Article = "der",
        Text = STRAUSS,
        TextAscii = STRAUSS_ASCII,
        Ipa = "ʃtɾaus (ʃtɾɔjzə)",
        Notes = "-es, -ä-e",
        LessonId = 1
    };

    private static readonly Word _wordStrauss2 = new()
    {
        Id = 2,
        Article = "der",
        Text = STRAUSS,
        TextAscii = STRAUSS_ASCII,
        Ipa = "ʃtɾaus",
        Notes = "-es, -e",
        LessonId = 2
    };

    private static readonly Word _wordStrauss3 = new()
    {
        Id = 3,
        Article = "das",
        Text = STRAUSS,
        TextAscii = STRAUSS_ASCII,
        Ipa = "ʃtɾaus",
        Notes = "-es, -e",
        LessonId = 3
    };

    private static readonly Word _wordHorse = new()
    {
        Id = 4,
        Article = "das",
        Text = PFERD,
        Ipa = "pfeːət",
        Notes = "-(e)s, -e",
        LessonId = 4
    };

    private static readonly Czech _czechBouquet = new()
    {
        Id = 1,
        Text = "kytice"
    };

    private static readonly Czech _czechOstrich = new()
    {
        Id = 2,
        Text = "pštros"
    };

    private static readonly Czech _czechNonsense = new()
    {
        Id = 3,
        Text = "NESMYSL VYTVOŘENÝ PRO TESTOVACÍ ÚČELY"
    };

    private static readonly Czech _czechHorse = new()
    {
        Id = 4,
        Text = "kůň"
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
    public async Task GetWordsWithCzechsByTextAsyncTest_WithoutLessonIds_NoWordsInDb_ShouldThrow() =>
        Assert.That(
            async () => await _wordRepository.GetWordsWithCzechsByTextAsync(CancellationToken.None),
            Throws.TypeOf<InvalidDataException>()
        );

    [Test]
    public async Task GetWordsWithCzechsByTextAsyncTest_NoWordsInDb_ShouldThrow() =>
        Assert.That(
            async () => await _wordRepository.GetWordsWithCzechsByTextAsync(new HashSet<int>() { 1, 2, 3 }, CancellationToken.None),
            Throws.TypeOf<InvalidDataException>()
        );

    [Test]
    public async Task GetWordsWithCzechsByTextAsyncTest_WithoutLessonIds_ShouldReturnAll_WithAllWords()
    {
        FillDatabase();
        var result = await _wordRepository.GetWordsWithCzechsByTextAsync(CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));
        AssertExpectedWords(result[STRAUSS], _wordStrauss1, _wordStrauss2, _wordStrauss3);
        AssertExpectedWords(result[PFERD], _wordHorse);
    }

    [Test]
    public async Task GetWordsWithCzechsByTextAsyncTest_OneLessonId_DoesNotExistAmongWordLessonIds_ShouldReturnEmpty()
    {
        FillDatabase();
        var result = await _wordRepository.GetWordsWithCzechsByTextAsync(new HashSet<int>() { 99 }, CancellationToken.None);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetWordsWithCzechsByTextAsyncTest_MultipleLessonIds_DoNotExistAmongWordLessonIds_ShouldReturnEmpty()
    {
        FillDatabase();
        var result = await _wordRepository.GetWordsWithCzechsByTextAsync(new HashSet<int>() { 97, 98, 99 }, CancellationToken.None);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetWordsWithCzechsByTextAsyncTest_OneLessonId_ExistsAmongWordLessonIds_ShouldReturnOne_WithCorrespondingWord()
    {
        FillDatabase();
        var result = await _wordRepository.GetWordsWithCzechsByTextAsync(new HashSet<int>() { 1 }, CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        AssertExpectedWords(result[STRAUSS], _wordStrauss1);
    }

    [Test]
    public async Task GetWordsWithCzechsByTextAsyncTest_MultipleLessonIds_OneExistsAmongWordLessonIds_ShouldReturnOne_WithCorrespondingWord()
    {
        FillDatabase();
        var result = await _wordRepository.GetWordsWithCzechsByTextAsync(new HashSet<int>() { 1, 98, 99 }, CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        AssertExpectedWords(result[STRAUSS], _wordStrauss1);
    }

    [Test]
    public async Task GetWordsWithCzechsByTextAsyncTest_MultipleLessonIds_SomeExistAmongWordLessonIds_ShouldReturnOne_WithCorrespondingWords()
    {
        FillDatabase();
        var result = await _wordRepository.GetWordsWithCzechsByTextAsync(new HashSet<int>() { 1, 2, 99 }, CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        AssertExpectedWords(result[STRAUSS], _wordStrauss1, _wordStrauss2);
    }

    [Test]
    public async Task GetWordsWithCzechsByTextAsyncTest_MultipleLessonIds_AllExistsAmongWordLessonIds_ShouldReturnOne_WithCorrespondingWords()
    {
        FillDatabase();
        var result = await _wordRepository.GetWordsWithCzechsByTextAsync(new HashSet<int>() { 1, 2, 3 }, CancellationToken.None);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        AssertExpectedWords(result[STRAUSS], _wordStrauss1, _wordStrauss2, _wordStrauss3);
    }

    private void FillDatabase()
    {
        _digTionaryDbContext
            .Set<Dictionary<string, object>>(TableNames.WORDS_CZECHS)
            .AddRange(
                new Dictionary<string, object>
                {
                    [ColumnNames.WORD_ID] = _wordStrauss1.Id,
                    [ColumnNames.CZECH_ID] = _czechBouquet.Id
                },
                new Dictionary<string, object>
                {
                    [ColumnNames.WORD_ID] = _wordStrauss2.Id,
                    [ColumnNames.CZECH_ID] = _czechOstrich.Id
                },
                new Dictionary<string, object>
                {
                    [ColumnNames.WORD_ID] = _wordStrauss3.Id,
                    [ColumnNames.CZECH_ID] = _czechNonsense.Id
                },
                new Dictionary<string, object>
                {
                    [ColumnNames.WORD_ID] = _wordHorse.Id,
                    [ColumnNames.CZECH_ID] = _czechHorse.Id
                }
            );

        _digTionaryDbContext.AddRange(
            _wordStrauss1,
            _wordStrauss2,
            _wordStrauss3,
            _wordHorse,
            _czechBouquet,
            _czechOstrich,
            _czechNonsense,
            _czechHorse
        );

        _ = _digTionaryDbContext.SaveChanges();
    }

    private static void AssertExpectedWords(IEnumerable<Word> actualWords, params Word[] expectedWords)
    {
        Assert.That(actualWords, Is.Not.Null);
        Assert.That(actualWords.Count(), Is.EqualTo(expectedWords.Length));

        foreach (var expectedWord in expectedWords)
        {
            Assert.That(actualWords, Does.Contain(expectedWord));
            var matchingWord = actualWords.First(word => word.Id == expectedWord.Id);
            Assert.That(expectedWord, Is.EqualTo(matchingWord).UsingPropertiesComparer());
        }
    }
}
