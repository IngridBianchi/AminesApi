using GetAdults.Models;

namespace GetAdults.Services
{
    public interface IAdultService
    {
        Task<List<Adult>> GetAllAdultsAsync();
    }
}
