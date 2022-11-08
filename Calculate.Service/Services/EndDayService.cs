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
                var today = DateTime.UtcNow.Date;
                var date = DateTime.UtcNow;

                if (isCheckDay)
                {
                    today = today.AddDays(-1);
                    date = date.AddDays(-1);
                }
                                      
                int currentUserId = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;
                List<int> minusAccount = new List<int>() { (int)EnumProcessType.CEKIM, (int)EnumProcessType.KOMISYON, (int)EnumProcessType.TRANSFER };
                var list = await _context.Operations.Where(x => x.CaseId == caseId && x.IsEnable == true && x.UpdatedDate.Date == today).Select(item => new OperationArchive
                {
                    ProcessNumber = item.ProcessNumber,
                    AccountId = item.AccountId,
                    AccountDetailId = item.AccountDetailId,
                    ProcessTypeId = item.ProcessTypeId,
                    Price = minusAccount.Contains(item.ProcessTypeId) ? -1 * item.Price : item.Price,
                    ProcessPrice = -1 * item.ProcessPrice,
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


                await _context.OperationsArchive.AddRangeAsync(list);

                await _context.Operations.AddRangeAsync(devirList);

                await _context.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public async Task<List<Case>> GetCaseAsync(string officeId)
        {
            int _officeId = Convert.ToInt32(officeId);
            var caseList = await _context.Cases.Where(x => x.officeId == _officeId).Select(x => new Case { Id = x.Id, Name = x.Name }).ToListAsync();
            return caseList;
        }
    }
}
