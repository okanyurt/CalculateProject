using Calculate.Data.Models;

namespace Calculate.Service.Services
{
    public interface IUserService
    {
        Task<List<UserGet>> GetAllAsync();

        Task<UserGet> GetByIdAsync(int id);

        Task<int> AddAsync(User userCreate, string userId);

        Task<int> UpdateAsync(User userUpdate, string userId);

        Task<int> RemoveAsync(int id, string userId);

        Task<List<Office>> GetAllOfficeAsync();
    }
}
