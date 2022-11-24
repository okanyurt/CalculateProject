using Calculate.Data.Models;

namespace Calculate.Service.Services
{
    public interface ICaseService
    {
        Task<List<Case>> GetAllAsync();

        Task<Case> GetByIdAsync(int id);

        Task<int> AddAsync(Case CaseCreate, string userId);

        Task<int> UpdateAsync(Case CaseUpdate, string userId);

        Task<int> RemoveAsync(int id, string userId);

        Task<List<Office>> GetAllOfficeAsync();
    }
}
