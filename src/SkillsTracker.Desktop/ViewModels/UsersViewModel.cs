using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkillsTracker.Core.Abstractions;
using SkillsTracker.Core.Models;

namespace SkillsTracker.Desktop.ViewModels;

public partial class UsersViewModel : ViewModelBase
{
    private readonly IUserService _userService;

    [ObservableProperty]
    private ObservableCollection<User> _users = [];

    public UsersViewModel(IUserService userService)
    {
        _userService = userService;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        var response = await _userService.GetUsersAsync(page: 0, size: 200);
        Users = new ObservableCollection<User>(response.Data);
    }
}
