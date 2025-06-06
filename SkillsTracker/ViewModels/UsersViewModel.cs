using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using MudBlazor;
using ReactiveUI;
using SkillsTracker.Models;
using SkillsTracker.Services;

namespace SkillsTracker.ViewModels
{
    public class UsersViewModel : ReactiveObject
    {
        private readonly IUserService _userService;

        public UsersViewModel(IUserService userService)
        {
            _userService = userService;

            // Command to load users
            LoadUsersCommand = ReactiveCommand.CreateFromTask<GridState<User>, GridData<User>>(
                LoadUsers
            );

            // Command to add a user
            AddUserCommand = ReactiveCommand.CreateFromTask(AddUser);
        }

        private ObservableCollection<User> _users = [];
        public ObservableCollection<User> Users
        {
            get => _users;
            set => this.RaiseAndSetIfChanged(ref _users, value);
        }

        public ReactiveCommand<GridState<User>, GridData<User>> LoadUsersCommand { get; }
        public ReactiveCommand<Unit, Unit> AddUserCommand { get; }

        public async Task<GridData<User>> LoadUsers(GridState<User> state)
        {
            var page = state.Page;
            var pageSize = state.PageSize;
            var usersPaged = await _userService.GetUsersAsync(page, pageSize, "Id", true);
            Users = new ObservableCollection<User>(usersPaged.Data);
            return new GridData<User>
            {
                Items = usersPaged.Data,
                TotalItems = usersPaged.TotalCount,
            };
        }

        private async Task AddUser()
        {
            var newUser = new User { Name = "John Doe", Email = "john@example.com" };
            await _userService.CreateUserAsync(newUser);
            // await LoadUsers();
        }
    }
}
