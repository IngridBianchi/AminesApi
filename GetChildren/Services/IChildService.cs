using GetChildren.Models;

namespace GetChildren.Services
{
    public interface IChildService
    {
        Task<List<Child>> GetAllChildrenAsync();
    }
}
