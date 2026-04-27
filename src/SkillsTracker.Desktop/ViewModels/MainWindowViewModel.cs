namespace SkillsTracker.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public UsersViewModel UsersViewModel { get; }
    public SkillsViewModel SkillsViewModel { get; }
    public TopicsViewModel TopicsViewModel { get; }

    public MainWindowViewModel(
        UsersViewModel usersViewModel,
        SkillsViewModel skillsViewModel,
        TopicsViewModel topicsViewModel)
    {
        UsersViewModel = usersViewModel;
        SkillsViewModel = skillsViewModel;
        TopicsViewModel = topicsViewModel;
        _ = usersViewModel.LoadAsync();
        _ = skillsViewModel.LoadAsync();
        _ = topicsViewModel.LoadAsync();
    }
}
