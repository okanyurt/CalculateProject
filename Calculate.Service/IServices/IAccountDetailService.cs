using Calculate.Data.Models;

namespace Calculate.Service.Services
{
    public interface IAccountDetailService
    {
        Task<List<AccountDetailGet>> GetAllAsync();

        Task<AccountDetailGet> GetByIdAsync(int id);

        Task<int> AddAsync(AccountDetail accountDetailCreate, string userId);

        Task<int> UpdateAsync(AccountDetail accountDetailUpdate, string userId);

        Task<int> RemoveAsync(int id, string userId);

        Task<List<Bank>> GetAllBankAsync();

        Task<List<Account>> GetAllAccountAsync();
    }
}
