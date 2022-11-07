using Calculate.Data;
using Calculate.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Calculate.Service.Services
{
    public class ReportService : IReportService
    {
        private readonly DataContext _context;

        public ReportService(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Case>> GetCaseAsync(string officeId)
        {
            int _officeId = Convert.ToInt32(officeId);
            var caseList = await _context.Cases.Where(x => x.officeId == _officeId).Select(x => new Case { Id = x.Id, Name = x.Name }).ToListAsync();
            return caseList;
        }

        public async Task<List<ReportGet>> GetAllAsync(int Id)
        {
            var list = from o in _context.Operations
                       join a in _context.Accounts on o.AccountId equals a.Id
                       join ad in _context.AccountDetails on o.AccountDetailId equals ad.Id
                       join b in _context.Banks on ad.BankId equals b.Id
                       join pt in _context.ProcessTypes on o.ProcessTypeId equals pt.Id
                       join c in _context.Cases on o.CaseId equals c.Id
                       where o.IsEnable == true && o.CaseId == Id
                       orderby o.UpdatedDate descending
                       group new { a, b, pt, o } by new
                       {
                           //Id = o.Id,
                           Account = a.Name,
                           AccountDetail = b.Name,
                           ProcessType = pt.Name,
                           //Price = o.Price
                       } into g
                       select new ReportGet
                       {
                          // Id = g.Key.Id,
                           Account = g.Key.Account,
                           AccountDetail = g.Key.AccountDetail,
                           ProcessType = g.Key.ProcessType,
                           Price = g.Sum(x => x.o.Price),
                           ProcessCount = g.Count()
                       };


            return await list.ToListAsync();
        }

        public async Task<Operation> GetCaseTotalAsync(int Id)
        {
            var caseList = from o in _context.Operations                      
                       where o.IsEnable == true && o.CaseId == Id
                       group o  by new
                       {
                           Case = o.CaseId
                       } into g
                       select new Operation
                       {
                           Price = g.Sum(x => x.Price)
                       };


            return await caseList.FirstOrDefaultAsync();
        }


        public async Task<List<ReportGet>> GetAllForCaseAsync(int Id)
        {
            var list = from o in _context.Operations
                       join a in _context.Accounts on o.AccountId equals a.Id
                       join ad in _context.AccountDetails on o.AccountDetailId equals ad.Id
                       join b in _context.Banks on ad.BankId equals b.Id
                       join pt in _context.ProcessTypes on o.ProcessTypeId equals pt.Id
                       join c in _context.Cases on o.CaseId equals c.Id
                       where o.IsEnable == true && o.CaseId == Id
                       orderby o.UpdatedDate descending
                       group new { a, b, pt, o } by new
                       {
                           Account = a.Name,
                           AccountDetail = b.Name,
                           ProcessType = pt.Name
                       } into g
                       select new ReportGet
                       {
                           Account = g.Key.Account,
                           AccountDetail = g.Key.AccountDetail,
                           ProcessType = g.Key.ProcessType,
                           Price = g.Sum(x => x.o.Price),
                           ProcessCount = g.Count()
                       };


            return await list.ToListAsync();
        }
        
    }
}
