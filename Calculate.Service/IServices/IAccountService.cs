using Calculate.Data.Models;

namespace Calculate.Service.Services
{
    public interface IAccountService
    {
        Task<List<AccountGet>> GetAllAsync();

        Task<AccountGet> GetByIdAsync(int id);

        Task<int> AddAsync(AccountGet accountCreate, string userId);

        Task<int> UpdateAsync(AccountGet accountUpdate, string userId);

        Task<int> RemoveAsync(int id, string userId);

        Task<List<Case>> GetAllCaseAsync();

        Task<List<Bank>> GetAllBankAsync();
    }
}
