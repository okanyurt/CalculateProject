using Calculate.Data.Models;

namespace Calculate.Service.Services
{
    public interface IEndDayService
    {
        Task<bool> CalculateEndDayAsync(int caseId, string userId, bool isCheckDay);
        Task<List<Case>> GetCaseAsync(string officeId);
    }
}
