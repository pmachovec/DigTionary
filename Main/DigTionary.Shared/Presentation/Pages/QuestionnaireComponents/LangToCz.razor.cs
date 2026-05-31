using DigTionary.Shared.Application;
using DigTionary.Shared.Database.Entities;
using DigTionary.Shared.Domain;
using DigTionary.Shared.Presentation.Constants;
using DigTionary.Shared.Resources.Translations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace DigTionary.Shared.Presentation.Pages.QuestionnaireComponents;

public class LangToCzBase : ComponentBase
{
    [Inject]
    protected IStringLocalizer<DigTionaryTranslations> Localizer { get; set; } = default!;

    [Inject]
    private IWordsByTextGenerator WordsByTextGenerator { get; set; } = default!;

    protected int WordPointer;

    protected string ActualWordText => GeneratedWordsByText[WordPointer].Text;

    protected IEnumerable<Word> ActualWords => GeneratedWordsByText[WordPointer].Words;

    protected int WordsShownCount { get; private set; }

    protected string Done { get; private set; } = string.Empty;

    protected List<WordsByText> GeneratedWordsByText = [];

    protected bool IsLastWordShown { get; private set; }

    protected string NextWordButtonDisabled { get; private set; } = CssClasses.DISABLED;

    protected override void OnInitialized() => GeneratedWordsByText.Add(WordsByTextGenerator.GetNext());

    protected void ShowWord()
    {
        IsLastWordShown = true;
        WordsShownCount++;

        if (WordsShownCount < WordsByTextGenerator.Count)
        {
            NextWordButtonDisabled = string.Empty;
        }
        else
        {
            NextWordButtonDisabled = CssClasses.DISABLED;
            Done = Localizer[DigTionaryTranslations.Done];
        }
    }
}
