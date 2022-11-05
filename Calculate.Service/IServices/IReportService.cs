using Calculate.Data.Models;

namespace Calculate.Service.Services
{
    public interface IReportService
    {
        Task<List<Case>> GetCaseAsync(string officeId);

        Task<List<ReportGet>> GetAllAsync(int Id);

        Task<Operation> GetCaseTotalAsync(int Id);
    }
}
