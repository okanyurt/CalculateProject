using Calculate.Data.Models;

namespace Calculate.Service.Services
{
    public interface IAccountService
    {
        Task<List<AccountGet>> GetAllAsync();

        Task<AccountGet> GetByIdAsync(int id);

        Task<int> AddAsync(Account accountCreate, string userId);

        Task<int> UpdateAsync(Account accountUpdate, string userId);

        Task<int> RemoveAsync(int id, string userId);

        Task<List<Case>> GetAllCaseAsync();
    }
}
