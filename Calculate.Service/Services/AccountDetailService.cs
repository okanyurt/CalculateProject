using Calculate.Data;
using Calculate.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Calculate.Service.Services
{
    public class AccountDetailService : IAccountDetailService
    {
        private readonly DataContext _context;

        public AccountDetailService(DataContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(AccountDetail accountDetailCreate, string userId)
        {
            int result = 0;
            int currentUserId = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;
            var date = DateTime.UtcNow.AddHours(3);

            AccountDetail _accountDetail = new AccountDetail();
            _accountDetail.AccountId = accountDetailCreate.AccountId;
            _accountDetail.BankId = accountDetailCreate.BankId;
            _accountDetail.IbanNumber = accountDetailCreate.IbanNumber;
            _accountDetail.BankAccountNumber = accountDetailCreate.BankAccountNumber;
            _accountDetail.CreatedBy = currentUserId;
            _accountDetail.CreatedDate = date;
            _accountDetail.UpdatedBy = currentUserId;
            _accountDetail.UpdatedDate = date;
            _accountDetail.IsEnable = true;

            await _context.AccountDetails.AddAsync(_accountDetail);
            result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<List<AccountDetailGet>> GetAllAsync()
        {
            var list = from a in _context.Accounts
                       join ad in _context.AccountDetails on a.Id equals ad.AccountId
                       join b in _context.Banks on ad.BankId equals b.Id     
                       join c in _context.Cases on a.CaseId equals c.Id
                       where ad.IsEnable == true
                       orderby ad.UpdatedDate descending
                       select new AccountDetailGet
                       {
                           Id = ad.Id,
                           AccountId = ad.AccountId,
                           BankId = ad.BankId,
                           IbanNumber = ad.IbanNumber,
                           BankAccountNumber = ad.BankAccountNumber,
                           IsEnable = ad.IsEnable,
                           CreatedBy = ad.CreatedBy,
                           CreatedDate = ad.CreatedDate,
                           UpdatedBy = ad.UpdatedBy,
                           UpdatedDate = ad.UpdatedDate,          
                           AccountName = a.Name,
                           BankName = b.Name,
                           CaseName = c.Name
                       };
            return await list.ToListAsync();
        }

        public async Task<AccountDetailGet> GetByIdAsync(int id)
        {
            var list = from a in _context.Accounts
                       join ad in _context.AccountDetails on a.Id equals ad.AccountId
                       join b in _context.Banks on ad.BankId equals b.Id
                       join c in _context.Cases on a.CaseId equals c.Id
                       where  ad.Id == id && ad.IsEnable == true
                       orderby ad.UpdatedDate descending
                       select new AccountDetailGet
                       {
                           Id = ad.Id,
                           AccountId = ad.AccountId,
                           BankId = ad.BankId,
                           IbanNumber = ad.IbanNumber,
                           BankAccountNumber = ad.BankAccountNumber,
                           IsEnable = ad.IsEnable,
                           CreatedBy = ad.CreatedBy,
                           CreatedDate = ad.CreatedDate,
                           UpdatedBy = ad.UpdatedBy,
                           UpdatedDate = ad.UpdatedDate,
                           AccountName = a.Name,
                           BankName = b.Name,
                           CaseName = c.Name
                       };

            return await list.FirstOrDefaultAsync();
        }

        public async Task<int> RemoveAsync(int id, string userId)
        {           
            var _accountDetail = await _context.AccountDetails.Where(x => x.Id == id).FirstOrDefaultAsync();
            int result = 0;
            if (_accountDetail != null)
            {
                var date = DateTime.UtcNow;
                _accountDetail.IsEnable = false;
                _accountDetail.UpdatedBy = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;
                _accountDetail.UpdatedDate = date;

                result = await _context.SaveChangesAsync();
            }

            return result;
        }

        public async Task<int> UpdateAsync(AccountDetail accountUpdate, string userId)
        {
            int result = 0;
            var date = DateTime.UtcNow.AddHours(3);
            var _account = _context.Accounts.Find(accountUpdate.Id);
            var user = _context.Users.FirstOrDefault(x => x.UserId == userId);

            var _accountDetail = await _context.AccountDetails.Where(x => x.AccountId == accountUpdate.AccountId).FirstOrDefaultAsync();
            _accountDetail.AccountId = accountUpdate.AccountId;
            _accountDetail.BankId = accountUpdate.BankId;
            _accountDetail.IbanNumber = accountUpdate.IbanNumber;
            _accountDetail.BankAccountNumber = accountUpdate.BankAccountNumber;
            _accountDetail.UpdatedBy = user.Id;
            _accountDetail.UpdatedDate = date;
            _accountDetail.IsEnable = true;

            result = await _context.SaveChangesAsync();

            return result;
        }

        public async Task<List<Bank>> GetAllBankAsync()
        {
            var list = await _context.Banks.ToListAsync();
            return list;
        }

        public async Task<List<Account>> GetAllAccountAsync()
        {
            var list = from a in _context.Accounts
                       join c in _context.Cases on a.CaseId equals c.Id
                       where a.IsEnable == true
                       orderby a.Name ascending
                       select new Account
                       {
                           Id = a.Id,
                           Name = a.Name + "-" + c.Name
                       };
            return await list.ToListAsync();
        }
    }
}
