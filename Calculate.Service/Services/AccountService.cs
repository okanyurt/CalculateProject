using Calculate.Data;
using Calculate.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Calculate.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly DataContext _context;

        public AccountService(DataContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(AccountGet accountCreate, string userId)
        {
            int result = 0;
            int currentUserId = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;
            var date = DateTime.UtcNow.AddHours(3);
            Account _account = new Account();
            _account.Name = accountCreate.Name;
            _account.PhoneNumber= accountCreate.PhoneNumber;
            _account.IdentityNumber = accountCreate.IdentityNumber;
            _account.Note = accountCreate.Note;
            _account.CreatedBy = currentUserId;
            _account.CreatedDate = date;
            _account.UpdatedBy = currentUserId;
            _account.UpdatedDate = date;
            _account.IsEnable = true;
            _account.CaseId = accountCreate.CaseId;

            await _context.Accounts.AddAsync(_account);

            result = await _context.SaveChangesAsync();

            int _accountId = await _context.Accounts.Where(x => x.IsEnable == true && x.UpdatedDate == date).Select(x => x.Id).FirstOrDefaultAsync();

            AccountDetail _accountDetail = new AccountDetail();
            _accountDetail.AccountId = _accountId;
            _accountDetail.BankId = accountCreate.BankId;
            _accountDetail.IbanNumber = accountCreate.IbanNumber;
            _accountDetail.BankAccountNumber = accountCreate.BankAccountNumber;
            _accountDetail.CreatedBy = currentUserId;
            _accountDetail.CreatedDate = date;
            _accountDetail.UpdatedBy = currentUserId;
            _accountDetail.UpdatedDate = date;
            _accountDetail.IsEnable = true;

            await _context.AccountDetails.AddAsync(_accountDetail);
            result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<List<AccountGet>> GetAllAsync()
        {
            var list = from a in _context.Accounts
                       join ad in _context.AccountDetails on a.Id equals ad.AccountId
                       join b in _context.Banks on ad.BankId equals b.Id                      
                       join c in _context.Cases on a.CaseId equals c.Id
                       where a.IsEnable == true
                       orderby a.UpdatedDate descending
                       select new AccountGet
                       {
                           Id = a.Id,
                           Name = a.Name,
                           PhoneNumber = a.PhoneNumber,
                           IdentityNumber = a.IdentityNumber,
                           Note = a.Note,
                           IsEnable = a.IsEnable,
                           CreatedBy = a.CreatedBy,
                           CreatedDate = a.CreatedDate,
                           UpdatedBy = a.UpdatedBy,
                           UpdatedDate = a.UpdatedDate,
                           CaseId = a.CaseId,
                           BankId = ad.BankId,
                           IbanNumber = ad.IbanNumber,
                           BankAccountNumber = ad.BankAccountNumber,
                           CaseName = c.Name,
                           BankName = b.Name
                       };           
            return await list.ToListAsync();
        }

        public async Task<List<Case>> GetAllCaseAsync()
        {
            var list = await _context.Cases.Where(x => x.IsEnable == true).ToListAsync();
            return list;
        }

        public async Task<AccountGet> GetByIdAsync(int id)
        {
            var list = from a in _context.Accounts
                       join ad in _context.AccountDetails on a.Id equals ad.AccountId
                       where a.IsEnable == true && a.Id == id
                       orderby a.UpdatedDate descending
                       select new AccountGet
                       {
                           Id = a.Id,
                           Name = a.Name,
                           PhoneNumber = a.PhoneNumber,
                           IdentityNumber = a.IdentityNumber,
                           Note = a.Note,
                           IsEnable = a.IsEnable,
                           CreatedBy = a.CreatedBy,
                           CreatedDate = a.CreatedDate,
                           UpdatedBy = a.UpdatedBy,
                           UpdatedDate = a.UpdatedDate,
                           CaseId = a.CaseId,
                           BankId = ad.BankId,
                           IbanNumber = ad.IbanNumber,
                           BankAccountNumber = ad.BankAccountNumber
                       };

            return await list.FirstOrDefaultAsync();
        }

        public async Task<int> RemoveAsync(int id, string userId)
        {
            var _account = _context.Accounts.Find(id);
            var _accountDetail = await _context.AccountDetails.Where(x => x.AccountId == id).FirstOrDefaultAsync();
            int result = 0;
            if (_account != null)
            {
                var date = DateTime.UtcNow;
                _account.IsEnable = false;
                _account.UpdatedBy = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;
                _account.UpdatedDate = date;

                result = await _context.SaveChangesAsync();
            }

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

        public async Task<int> UpdateAsync(AccountGet accountUpdate, string userId)
        {
            int result = 0;
            var date = DateTime.UtcNow.AddHours(3);
            var _account = _context.Accounts.Find(accountUpdate.Id);
            var user = _context.Users.FirstOrDefault(x => x.UserId == userId);
            _account.Name = accountUpdate.Name;
            _account.PhoneNumber = accountUpdate.PhoneNumber;
            _account.IdentityNumber = accountUpdate.IdentityNumber;
            _account.Note = accountUpdate.Note;          
            _account.UpdatedBy = user.Id;
            _account.UpdatedDate = date;
            _account.IsEnable = true;
            _account.CaseId = accountUpdate.CaseId;

            result = await _context.SaveChangesAsync();

            int _accountId = await _context.Accounts.Where(x => x.IsEnable == true && x.UpdatedDate == date).Select(x => x.Id).FirstOrDefaultAsync();
           
            var _accountDetail = await _context.AccountDetails.Where(x => x.AccountId == _accountId).FirstOrDefaultAsync();
            _accountDetail.AccountId = _accountId;
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
    }
}
