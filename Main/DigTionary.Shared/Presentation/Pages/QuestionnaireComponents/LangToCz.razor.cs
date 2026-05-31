using DigTionary.Shared.Application;
using DigTionary.Shared.Database.Entities;
using DigTionary.Shared.Domain;
using DigTionary.Shared.Presentation.Constants;
using DigTionary.Shared.Presentation.Pages.QuestionnaireComponents.Shared;
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

    public Navigation Navigation { get; protected set; } = default!;

    protected override void OnInitialized() => GeneratedWordsByText.Add(WordsByTextGenerator.GetNext());

    protected void ShowWord()
    {
        IsLastWordShown = true;
        WordsShownCount++;

        if (WordsShownCount < WordsByTextGenerator.Count)
        {
            Navigation.NextWordButtonDisabled = string.Empty;
        }
        else
        {
            Navigation.NextWordButtonDisabled = CssClasses.DISABLED;
            Navigation.SetDone();
        }
    }

    protected void ClickNextWordButton()
    {
        if (WordPointer == (GeneratedWordsByText.Count - 1))
        {
            // The last retrieved text is currently displayed, retrieve and show a new one.
            // The text is certainly shown, otherwise, the button would be disabled.
            Navigation.NextWordButtonDisabled = CssClasses.DISABLED;
            IsLastWordShown = false;
            GeneratedWordsByText.Add(WordsByTextGenerator.GetNext());
        }
        // else Just display the next generated text.

        Navigation.PreviousWordButtonDisabled = string.Empty;
        WordPointer++;

        if ((WordPointer == (GeneratedWordsByText.Count - 1) && !IsLastWordShown) || WordPointer == (WordsByTextGenerator.Count - 1))
        {
            // The last generated text is currently displayed, disable the Next button.
            Navigation.NextWordButtonDisabled = CssClasses.DISABLED;
        }
    }

    protected void ClickPreviousWordButton()
    {
        Navigation.NextWordButtonDisabled = string.Empty;
        WordPointer--;

        if (WordPointer == 0)
        {
            // The first generated text is currently displayed, disable the Previous button.
            Navigation.PreviousWordButtonDisabled = CssClasses.DISABLED;
        }
    }
}
