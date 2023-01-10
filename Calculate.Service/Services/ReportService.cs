using Calculate.Data;
using Calculate.Data.Enums;
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
        public async Task<List<Case>> GetCaseAsync(string officeId, string userId)
        {
            int _officeId = Convert.ToInt32(officeId);
            int roleId = _context.Users.FirstOrDefault(x => x.UserId == userId).RoleID;
            var caseList = new List<Case>();
            int admin = (int)EnumRole.ADMIN;
            if (admin == roleId)
            {
                caseList = await _context.Cases.Where(x => x.IsEnable == true).Select(x => new Case { Id = x.Id, Name = x.Name }).ToListAsync();
            }
            else
            {
                caseList = await _context.Cases.Where(x => x.officeId == _officeId).Select(x => new Case { Id = x.Id, Name = x.Name }).ToListAsync();
            }

            
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
                           Account = a.Name,
                           AccountDetail = b.Name,
                           ProcessType = pt.Name,
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

        public async Task<Operation> GetCaseTotalAsync(int Id)
        {
            var date = DateTime.UtcNow.AddHours(3).Date;
            var caseList = from o in _context.Operations
                           join a in _context.Accounts on o.AccountId equals a.Id
                           where o.UpdatedDate.Date == date && o.IsEnable == true && a.IsEnable == true && o.CaseId == Id
                           group o by new
                           {
                               Case = o.CaseId
                           } into g
                           select new Operation
                           {
                               Price = g.Sum(x => (x.Price+x.ProcessPrice))
                           };

            return await caseList.FirstOrDefaultAsync();
        }


        public async Task<Operation> GetInvestmentTotalAsync(int Id)
        {
            var date = DateTime.UtcNow.AddHours(3).Date;
            var investmentList = from o in _context.Operations
                           where o.UpdatedDate.Date == date && o.IsEnable == true && o.CaseId == Id && o.ProcessTypeId == 1
                           group o by new
                           {
                               Case = o.CaseId
                           } into g
                           select new Operation
                           {
                               Price = g.Sum(x => (x.Price + x.ProcessPrice))
                           };

            return await investmentList.FirstOrDefaultAsync();
        }

        public async Task<Operation> GetWithdrawalTotalAsync(int Id)
        {
            var date = DateTime.UtcNow.AddHours(3).Date;
            var withdrawalList = from o in _context.Operations
                           where o.UpdatedDate.Date == date && o.IsEnable == true && o.CaseId == Id && o.ProcessTypeId == 2
                           group o by new
                           {
                               Case = o.CaseId
                           } into g
                           select new Operation
                           {
                               Price = g.Sum(x => (x.Price)) //x.ProcessPrice
                           };

            return await withdrawalList.FirstOrDefaultAsync();
        }

        public async Task<List<ReportGet>> GetAllForCaseAsync(int Id)
        {
            var today = DateTime.UtcNow.AddHours(3).Date;
            var list = from o in _context.Operations
                       join a in _context.Accounts on o.AccountId equals a.Id
                       join ad in _context.AccountDetails on o.AccountDetailId equals ad.Id
                       join b in _context.Banks on ad.BankId equals b.Id
                       join pt in _context.ProcessTypes on o.ProcessTypeId equals pt.Id
                       join c in _context.Cases on o.CaseId equals c.Id
                       where o.IsEnable == true && a.IsEnable == true && o.CaseId == Id && o.UpdatedDate.Date == today
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
                           ProcessPrice = g.Sum(x => x.o.ProcessPrice),
                           ProcessCount = g.Count()
                       };

            var raporList = await list.ToListAsync();

            // TODO minusProcess ama yatırım var.Neden kullanıldığı da bakılmalı
            List<string> minusProcessCount = new List<string>() { "YATIRIM", "GELEN TRANSFER (+)" };

            raporList = raporList.Select(item => new ReportGet
            {
                Account = item.Account,
                AccountDetail = item.AccountDetail,
                ProcessType = item.ProcessType,
                Price = item.Price,
                ProcessPrice = item.ProcessPrice,
                ProcessCount = minusProcessCount.Contains(item.ProcessType) ? item.ProcessCount : 0 * item.ProcessCount
            }).ToList();

            raporList = raporList.GroupBy(x => new { x.Account, x.AccountDetail }).Select(x => new ReportGet
            {
                Account = x.Key.Account,
                AccountDetail = x.Key.AccountDetail,
                Price = x.Sum(y => y.Price) + x.Sum(y => y.ProcessPrice),
                ProcessPrice = 0,
                ProcessCount = x.Sum(y => y.ProcessCount)
            }).OrderBy(x => x.Account).ToList();

            return raporList;
        }

        public async Task<List<ReportGet>> GetAllInvestmentAsync(int Id)
        {
            var today = DateTime.UtcNow.AddHours(3).Date;
            var list = from o in _context.Operations
                       join a in _context.Accounts on o.AccountId equals a.Id
                       join ad in _context.AccountDetails on o.AccountDetailId equals ad.Id
                       join b in _context.Banks on ad.BankId equals b.Id
                       join pt in _context.ProcessTypes on o.ProcessTypeId equals pt.Id
                       join c in _context.Cases on o.CaseId equals c.Id
                       where o.IsEnable == true && o.CaseId == Id && o.UpdatedDate.Date == today && o.ProcessTypeId == 1
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
                           ProcessPrice = g.Sum(x => x.o.ProcessPrice),
                           ProcessCount = g.Count()
                       };

            var raporList = await list.ToListAsync();

            raporList = raporList.GroupBy(x => new { x.Account, x.AccountDetail }).Select(x => new ReportGet
            {
                Account = x.Key.Account,
                AccountDetail = x.Key.AccountDetail,
                Price = x.Sum(y => y.Price) + x.Sum(y => y.ProcessPrice),
                ProcessPrice = 0,
                ProcessCount = x.Sum(y => y.ProcessCount)
            }).OrderBy(x => x.Account).ToList();

            return raporList;
        }

        public async Task<List<ReportGet>> GetAllWithdrawalAsync(int Id)
        {
            var today = DateTime.UtcNow.AddHours(3).Date;
            var list = from o in _context.Operations
                       join a in _context.Accounts on o.AccountId equals a.Id
                       join ad in _context.AccountDetails on o.AccountDetailId equals ad.Id
                       join b in _context.Banks on ad.BankId equals b.Id
                       join pt in _context.ProcessTypes on o.ProcessTypeId equals pt.Id
                       join c in _context.Cases on o.CaseId equals c.Id
                       where o.IsEnable == true && o.CaseId == Id && o.UpdatedDate.Date == today && o.ProcessTypeId == 2
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
                           ProcessPrice = g.Sum(x => x.o.ProcessPrice),
                           ProcessCount = g.Count()
                       };

            var raporList = await list.ToListAsync();

            raporList = raporList.GroupBy(x => new { x.Account, x.AccountDetail }).Select(x => new ReportGet
            {
                Account = x.Key.Account,
                AccountDetail = x.Key.AccountDetail,
                Price = x.Sum(y => y.Price), //+ x.Sum(y => y.ProcessPrice),
                ProcessPrice = 0,
                ProcessCount = x.Sum(y => y.ProcessCount)
            }).OrderBy(x => x.Account).ToList();

            return raporList;
        }
    }
}
