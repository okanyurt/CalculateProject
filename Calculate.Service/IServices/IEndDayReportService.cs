using Calculate.Data.Models;

namespace Calculate.Service.Services
{
    public interface IEndDayReportService
    {
        Task<List<EndDayReport>> GetAllAsync(string _officeId, bool isAdmin);

        Task<List<EndDayReport>> GetAllSelectDateAsync(string _officeId, string _date, bool isAdmin);

        string GetMaxDate();
    }
}
