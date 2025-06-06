namespace SkillsTracker.Models.DTOs
{
    public class PagedResponse<T>
    {
        public List<T> Data { get; set; } = [];
        public int TotalCount { get; set; }
    }
}
