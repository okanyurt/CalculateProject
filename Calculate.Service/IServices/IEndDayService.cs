using Calculate.Data.Models;

namespace Calculate.Service.Services
{
    public interface IEndDayService
    {
        Task<List<Operation>> GetAllAsync(int caseId);

        Task<int> AddOperationArsivAsync(List<Operation> operations, string userId);

        Task<int> AddOperationAsync(int caseId, string userId);

        Task<List<Case>> GetCaseAsync(string officeId);
    }
}
