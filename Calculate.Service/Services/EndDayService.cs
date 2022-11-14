using Calculate.Data;
using Calculate.Data.Enums;
using Calculate.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Calculate.Service.Services
{
    public class EndDayService : IEndDayService
    {
        private readonly DataContext _context;

        public EndDayService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CalculateEndDayAsync(int caseId, string userId, bool isCheckDay)
        {
            bool result = false;
            
            try
            {
                var today = DateTime.UtcNow.AddHours(3).Date;
                var date = DateTime.UtcNow.AddHours(3);

                if (isCheckDay)
                {
                    today = today.AddDays(-1);
                    date = date.AddDays(-1);
                }

                int currentUserId = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;

                var deleteOperationList = await _context.Operations.Where(x => x.IsSystem == true && x.UpdatedDate.Date == date.AddDays(1).Date && x.CaseId==caseId).ToListAsync();

                var deleteOperationArchiveList = await _context.OperationsArchive.Where(x => x.UpdatedDate.Date == date.Date && x.CaseId == caseId).ToListAsync();

                var list = await _context.Operations.Where(x => x.CaseId == caseId && x.IsEnable == true && x.UpdatedDate.Date == today).Select(item => new OperationArchive
                {
                    ProcessNumber = item.ProcessNumber,
                    AccountId = item.AccountId,
                    AccountDetailId = item.AccountDetailId,
                    ProcessTypeId = item.ProcessTypeId,
                    Price = item.Price,
                    ProcessPrice = item.ProcessPrice,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.SpecifyKind(item.CreatedDate, DateTimeKind.Utc), "Turkey Standard Time", "UTC"),
                    UpdatedBy = item.UpdatedBy,
                    UpdatedDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.SpecifyKind(item.UpdatedDate, DateTimeKind.Utc), "Turkey Standard Time", "UTC"),
                    IsEnable = item.IsEnable,
                    CaseId = item.CaseId,
                    ArchiveBy = currentUserId,
                    ArchiveDate = date,
                    OperationId = item.Id,
                    IsSystem = item.IsSystem
                }).ToListAsync();

                if (list == null)
                {
                    return result;
                }

                var devirList = list.GroupBy(x => new { x.CaseId, x.AccountId, x.AccountDetailId }).Select(x => new Operation
                {
                    ProcessNumber = 0,
                    AccountId = x.Key.AccountId,
                    AccountDetailId = x.Key.AccountDetailId,
                    ProcessTypeId = (int)EnumProcessType.DEVİR,
                    Price = x.Sum(y => y.Price) + x.Sum(y => y.ProcessPrice),
                    ProcessPrice = 0,
                    CreatedBy = currentUserId,
                    CreatedDate = date.AddDays(1),
                    UpdatedBy = currentUserId,
                    UpdatedDate = date.AddDays(1),
                    IsEnable = true,
                    CaseId = caseId,
                    IsSystem = true
                }).Where(x => x.Price > 0).ToList();

                if (devirList.Count == 0)
                {
                    return false;
                }

                var trans = _context.Database.BeginTransaction();
                try
                {
                    _context.OperationsArchive.RemoveRange(deleteOperationArchiveList);
                    _context.Operations.RemoveRange(deleteOperationList);
                    await _context.OperationsArchive.AddRangeAsync(list);
                    await _context.Operations.AddRangeAsync(devirList);
                    await _context.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public async Task<List<OperationGet>> GetAllAsync(string _officeId)
        {
            var date = DateTime.UtcNow.AddDays(1).AddHours(3).Date;
            var list = from o in _context.Operations
                       join a in _context.Accounts on o.AccountId equals a.Id
                       join ad in _context.AccountDetails on o.AccountDetailId equals ad.Id
                       join b in _context.Banks on ad.BankId equals b.Id
                       join pt in _context.ProcessTypes on o.ProcessTypeId equals pt.Id
                       join c in _context.Cases on o.CaseId equals c.Id
                       where o.IsEnable == true && c.officeId == Convert.ToInt32(_officeId) && o.UpdatedDate.Date == date && o.ProcessTypeId == 5
                       orderby o.UpdatedDate descending
                       select new OperationGet
                       {
                           Id = o.Id,
                           ProcessNumber = o.ProcessNumber,
                           Account = a.Name,
                           AccountDetail = b.Name,
                           ProcessType = pt.Name,
                           Price = o.Price,
                           ProcessPrice = o.ProcessPrice,
                           IsEnable = o.IsEnable,
                           CreatedBy = o.CreatedBy,
                           CreatedDate = o.CreatedDate,
                           UpdatedBy = o.UpdatedBy,
                           UpdatedDate = o.UpdatedDate,
                           CaseName = c.Name
                       };

            return await list.ToListAsync();
        }

        public async Task<List<Case>> GetCaseAsync(string officeId)
        {
            int _officeId = Convert.ToInt32(officeId);
            var caseList = await _context.Cases.Where(x => x.officeId == _officeId).Select(x => new Case { Id = x.Id, Name = x.Name }).ToListAsync();
            return caseList;
        }
    }
}
