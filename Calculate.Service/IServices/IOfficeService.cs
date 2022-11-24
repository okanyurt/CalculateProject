using Calculate.Data.Models;

namespace Calculate.Service.Services
{
    public interface IOfficeService
    {
        Task<List<Office>> GetAllAsync();

        Task<Office> GetByIdAsync(int id);

        Task<int> AddAsync(Office OfficeCreate, string userId);

        Task<int> UpdateAsync(Office OfficeUpdate, string userId);

        Task<int> RemoveAsync(int id, string userId);
    }
}
