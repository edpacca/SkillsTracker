namespace SkillsTracker.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public UsersViewModel UsersViewModel { get; }

    public MainWindowViewModel(UsersViewModel usersViewModel)
    {
        UsersViewModel = usersViewModel;
        _ = usersViewModel.LoadAsync();
    }
}
