using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkillsTracker.Core.Abstractions;
using SkillsTracker.Core.Models;

namespace SkillsTracker.Desktop.ViewModels;

public partial class TopicsViewModel : ViewModelBase
{
    private readonly ITopicService _topicService;

    [ObservableProperty]
    private ObservableCollection<Topic> _topics = [];

    public TopicsViewModel(ITopicService topicService)
    {
        _topicService = topicService;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        var topics = await _topicService.GetTopicsAsync();
        Topics = new ObservableCollection<Topic>(topics);
    }
}
