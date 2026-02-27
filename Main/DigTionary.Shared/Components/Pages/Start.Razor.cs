using DigTionary.Shared.Components.Pages.Constants;
using DigTionary.Shared.Database.Entities;
using DigTionary.Shared.Generators;
using DigTionary.Shared.Repositories;
using DigTionary.Shared.Resources.Translations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace DigTionary.Shared.Components.Pages;

public class StartBase : ComponentBase
{
    private HashSet<int> _selectedCategoriesIds = default!;
    private HashSet<int> _selectedLessonsIds = default!;

    [Inject]
    private ICategoryRepository CategoryRepository { get; set; } = default!;

    [Inject]
    private ICzechGenerator CzechGenerator { get; set; } = default!;

    [Inject]
    protected IStringLocalizer<DigTionaryTranslations> Localizer { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private QuestionnaireParams QuestionnaireParams { get; set; } = default!;

    protected Category[] Categories { get; private set; } = default!;

    protected string StartButtonDisabled { get; private set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        Categories = [.. await CategoryRepository.GetCategoriesWithLessonsAsync(CancellationToken.None)];

        if (Categories.Length == 0)
        {
            throw new InvalidDataException("No categories with lessons available in the database!");
        }

        _selectedCategoriesIds = [];
        _selectedLessonsIds = [];
        DisableStartButtonWhenNoLessonsSelected();
    }

    protected bool IsCategorySelected(int categoryId) => _selectedCategoriesIds.Contains(categoryId);

    protected bool IsLessonSelected(int lessonId) => _selectedLessonsIds.Contains(lessonId);

    protected void OnCategoryChange(ChangeEventArgs e, Category category)
    {
        if (e.Value is true)
        {
            _ = _selectedCategoriesIds.Add(category.Id);

            foreach (var lesson in category.Lessons)
            {
                _ = _selectedLessonsIds.Add(lesson.Id);
            }
        }
        else
        {
            _ = _selectedCategoriesIds.Remove(category.Id);

            foreach (var lesson in category.Lessons)
            {
                _ = _selectedLessonsIds.Remove(lesson.Id);
            }
        }

        DisableStartButtonWhenNoLessonsSelected();
    }

    protected void OnLessonChange(ChangeEventArgs e, Lesson lesson)
    {
        var isChecked = e.Value is true;
        _ = isChecked ? _selectedLessonsIds.Add(lesson.Id) : _selectedLessonsIds.Remove(lesson.Id);

        // Keep category state in sync - checked only when all its subcategories are checked
        var category = Categories.FirstOrDefault(c => c.Id == lesson.CategoryId);

        if (category is null)
        {
            return;
        }

        var hasSubcategories = category.Lessons.Count != 0;
        var isAllChecked = hasSubcategories && category.Lessons.All(lesson => _selectedLessonsIds.Contains(lesson.Id));
        _ = isAllChecked ? _selectedCategoriesIds.Add(category.Id) : _selectedCategoriesIds.Remove(category.Id);
        DisableStartButtonWhenNoLessonsSelected();
    }

    protected void Start()
    {
        // Comparing categories lengths is enough to determine if all categories are selected.
        QuestionnaireParams.SetUpCzechsTask = _selectedCategoriesIds.Count == Categories.Length
            ? CzechGenerator.SetUpCzechsAsync(CancellationToken.None)
            : CzechGenerator.SetUpCzechsAsync(_selectedLessonsIds, CancellationToken.None);

        NavigationManager.NavigateTo("/questionnaire");
    }

    private void DisableStartButtonWhenNoLessonsSelected() =>
        StartButtonDisabled = _selectedLessonsIds.Count == 0 ? CssClasses.DISABLED : string.Empty;
}
