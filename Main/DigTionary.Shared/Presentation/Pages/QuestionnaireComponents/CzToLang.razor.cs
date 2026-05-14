using DigTionary.Shared.Application;
using DigTionary.Shared.Database.Entities;
using DigTionary.Shared.Presentation.Constants;
using DigTionary.Shared.Resources.Translations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace DigTionary.Shared.Presentation.Pages.QuestionnaireComponents;

public class CzToLangBase : ComponentBase
{
    protected const string NAVIGATION_BUTTON_CLASSES = "border border-dark btn btn-lg btn-primary col-auto digtionary-button mb-2 me-2";

    [Inject]
    protected IStringLocalizer<DigTionaryTranslations> Localizer { get; set; } = default!;

    [Inject]
    private ICzechGenerator CzechGenerator { get; set; } = default!;

    protected int CzechPointer;

    protected string ActualCzechText => GeneratedCzechs[CzechPointer].Text;

    protected ICollection<Word> ActualWords => GeneratedCzechs[CzechPointer].Words;

    protected int WordsShownCount { get; private set; }

    protected string Done { get; private set; } = string.Empty;

    protected List<Czech> GeneratedCzechs = [];

    protected bool IsLastWordShown { get; private set; }

    protected string NextWordButtonDisabled { get; private set; } = CssClasses.DISABLED;

    protected string PreviousWordButtonDisabled { get; private set; } = CssClasses.DISABLED;

    protected int WordPointer { get; private set; }

    protected override void OnInitialized() => GeneratedCzechs.Add(CzechGenerator.GetNext());

    protected void ShowWord()
    {
        IsLastWordShown = true;
        WordsShownCount++;

        if (WordsShownCount < CzechGenerator.Count)
        {
            NextWordButtonDisabled = string.Empty;
        }
        else
        {
            NextWordButtonDisabled = CssClasses.DISABLED;
            Done = Localizer[DigTionaryTranslations.Done];
        }
    }

    protected async Task ClickNextWordButtonAsync()
    {
        if (CzechPointer == (GeneratedCzechs.Count - 1))
        {
            // The last retrieved Czech is currently displayed, retrieve and show a new one.
            // The Word of the Czech is certainly shown, otherwise, the button would be disabled.
            NextWordButtonDisabled = CssClasses.DISABLED;
            IsLastWordShown = false;
            GeneratedCzechs.Add(CzechGenerator.GetNext());
        }
        // else Just display the next generated Czech.

        PreviousWordButtonDisabled = string.Empty;
        CzechPointer++;

        if ((CzechPointer == (GeneratedCzechs.Count - 1) && !IsLastWordShown) || CzechPointer == (CzechGenerator.Count - 1))
        {
            // The last generated Czech is currently displayed, disable the Next button.
            NextWordButtonDisabled = CssClasses.DISABLED;
        }
    }

    protected async Task ClickPreviousWordButtonAsync()
    {
        NextWordButtonDisabled = string.Empty;
        CzechPointer--;

        if (CzechPointer == 0)
        {
            // The first generated Czech is currently displayed, disable the Previous button.
            PreviousWordButtonDisabled = CssClasses.DISABLED;
        }
    }
}
