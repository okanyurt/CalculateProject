using Calculate.Data.Models;

namespace Calculate.Service.Services
{
    public interface IReportService
    {
        Task<List<Case>> GetCaseAsync(string officeId, string userId);

        Task<List<ReportGet>> GetAllAsync(int Id);

        Task<Operation> GetCaseTotalAsync(int Id);

        Task<Operation> GetInvestmentTotalAsync(int Id);

        Task<Operation> GetWithdrawalTotalAsync(int Id);

        Task<List<ReportGet>> GetAllForCaseAsync(int Id);

        Task<List<ReportGet>> GetAllInvestmentAsync(int Id);

        Task<List<ReportGet>> GetAllWithdrawalAsync(int Id);
    }
}
