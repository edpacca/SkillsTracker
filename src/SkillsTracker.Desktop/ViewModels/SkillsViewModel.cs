using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkillsTracker.Core.Abstractions;
using SkillsTracker.Core.Models;

namespace SkillsTracker.Desktop.ViewModels;

public partial class SkillsViewModel : ViewModelBase
{
    private readonly ISkillService _skillService;

    [ObservableProperty]
    private ObservableCollection<Skill> _skills = [];

    public SkillsViewModel(ISkillService skillService)
    {
        _skillService = skillService;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        var skills = await _skillService.GetSkillsAsync();
        Skills = new ObservableCollection<Skill>(skills);
    }
}
