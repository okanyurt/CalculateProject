using Calculate.Data;
using Calculate.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Calculate.Service.Services
{
    public class EndDayReportService : IEndDayReportService
    {
        private readonly DataContext _context;

        public EndDayReportService(DataContext context)
        {
            _context = context;
        }
        public async Task<List<EndDayReport>> GetAllAsync(string _officeId,bool isAdmin)
        {
            int officeId = Convert.ToInt32(_officeId);
            var date = DateTime.UtcNow.AddHours(3).Date;
            var list = from o in _context.OperationsArchive
                       join a in _context.Accounts on o.AccountId equals a.Id
                       join ad in _context.AccountDetails on o.AccountDetailId equals ad.Id
                       join b in _context.Banks on ad.BankId equals b.Id
                       join pt in _context.ProcessTypes on o.ProcessTypeId equals pt.Id
                       join c in _context.Cases on o.CaseId equals c.Id
                       where o.IsEnable == true
                             && ((!isAdmin && c.officeId ==officeId) || isAdmin)
                             && o.UpdatedDate.Date == date
                       group new { o, c, pt } by new
                       {
                           CaseName = c.Name
                       } into g
                       select new EndDayReport
                       {
                           TotalProcessNumber = g.Count(),
                           TotalPayMoney = g.Where(x => x.pt.Id == 1).Sum(x => x.o.Price),
                           TotalTransfer = g.Where(x => x.pt.Id == 4).Sum(x => x.o.Price),
                           TotalWithdraw = g.Where(x => x.pt.Id == 2).Sum(x => x.o.Price),
                           TotalCommission = g.Where(x => x.pt.Id == 3).Sum(x => x.o.Price),
                           TotalBalance = g.Sum(x => x.o.Price) + g.Sum(x => x.o.ProcessPrice),
                           CaseName = g.Key.CaseName
                       };

            return await list.ToListAsync();
        }

        public async Task<List<EndDayReport>> GetAllSelectDateAsync(string _officeId, string _date, bool isAdmin)
        {
            var date = Convert.ToDateTime(_date);
            var unspecified = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Unspecified);
            var specified = DateTime.SpecifyKind(unspecified, DateTimeKind.Utc);
            int officeId = Convert.ToInt32(_officeId);
            var list = from o in _context.OperationsArchive
                       join a in _context.Accounts on o.AccountId equals a.Id
                       join ad in _context.AccountDetails on o.AccountDetailId equals ad.Id
                       join b in _context.Banks on ad.BankId equals b.Id
                       join pt in _context.ProcessTypes on o.ProcessTypeId equals pt.Id
                       join c in _context.Cases on o.CaseId equals c.Id
                       where o.IsEnable == true
                             && ((!isAdmin && c.officeId ==officeId) || isAdmin)
                             && o.UpdatedDate.Date == specified.Date
                       group new { o, c, pt } by new
                       {
                           CaseName = c.Name
                       } into g
                       select new EndDayReport
                       {
                           TotalProcessNumber = g.Count(),
                           TotalPayMoney = g.Where(x => x.pt.Id == 1).Sum(x => x.o.Price),
                           TotalTransfer = g.Where(x => x.pt.Id == 4).Sum(x => x.o.Price),
                           TotalWithdraw = g.Where(x => x.pt.Id == 2).Sum(x => x.o.Price),
                           TotalCommission = g.Where(x => x.pt.Id == 3).Sum(x => x.o.Price),
                           TotalBalance = g.Sum(x => x.o.Price) + g.Sum(x => x.o.ProcessPrice),
                           CaseName = g.Key.CaseName
                       };

            return await list.ToListAsync();
        }

        public  string GetMaxDate()
        {
            var date = _context.OperationsArchive.Select(x => x.UpdatedDate.Date).Distinct().FirstOrDefault();
            return date.ToString("yyyy-MM-dd");
        }
    }
}
