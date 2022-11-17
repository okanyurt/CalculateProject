using Calculate.Data.Models;

namespace Calculate.Service.Services
{
    public interface IEndDayReportService
    {
        Task<List<EndDayReport>> GetAllAsync(string _officeId);
    }
}
