namespace DigTionary.Shared.Presentation.Pages;

public sealed class QuestionnaireParams
{
    public bool CzToLang { get; set; } = true;

    public Task SetUpGeneratorTask { get; set; } = Task.CompletedTask;

    public void Reset()
    {
        CzToLang = true;
        SetUpGeneratorTask = Task.CompletedTask;
    }
}
