using Calculate.Data;
using Calculate.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Calculate.Service.Services
{
    public class OperationService : IOperationService
    {
        private readonly DataContext _context;

        public OperationService(DataContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(int ProcessNumber, int AccountId, int AccountDetailId, int ProcessTypeId, decimal Price, decimal ProcessPrice, string userId)
        {
            Operation operation = new Operation();
            var date = DateTime.UtcNow;
            operation.ProcessNumber = ProcessNumber;
            operation.AccountId = AccountId;
            operation.AccountDetailId = AccountDetailId;
            operation.ProcessTypeId = ProcessTypeId;
            operation.Price = Price;
            operation.ProcessPrice = ProcessPrice;
            operation.CreatedBy = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;
            operation.CreatedDate = date;
            operation.UpdatedBy = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;
            operation.UpdatedDate = date;
            operation.IsEnable = true;

            await _context.Operations.AddAsync(operation);

            return await _context.SaveChangesAsync();
        }

        public async Task<List<AccountGetName>> GetAccountAsync()
        {
            var accountList = await _context.Accounts.Where(x => x.IsEnable == true).Select(x => new AccountGetName { Id = x.Id, Name = x.Name }).ToListAsync();
            return accountList;
        }

        public async Task<List<OperationGet>> GetAllAsync()
        {
            var list = from o in _context.Operations
                       join a in _context.Accounts on o.AccountId equals a.Id
                       join ad in _context.AccountDetails on o.AccountDetailId equals ad.Id
                       join b in _context.Banks on ad.BankId equals b.Id
                       join pt in _context.ProcessTypes on o.ProcessTypeId equals pt.Id
                       where o.IsEnable == true
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
                           UpdatedDate = o.UpdatedDate
                       };

            return await list.ToListAsync();                        
        }

        public async Task<List<Bank>> GetBankAsync(int accountId)
        {
            var bankList = from b in _context.Banks
                           join ad in _context.AccountDetails on b.Id equals ad.BankId
                           where ad.AccountId == accountId && ad.IsEnable == true
                           select new Bank
                           {
                               Id = b.Id,
                               Name = b.Name
                           };

            return await bankList.ToListAsync();
        }

        public async Task<Operation> GetByIdAsync(int id)
        {
            return await _context.Operations.Where(x => x.IsEnable == true && x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<ProcessType>> GetProcessTypeAsync()
        {
            var processTypeList = await _context.ProcessTypes.Select(x => new ProcessType { Id = x.Id, Name = x.Name }).ToListAsync();
            return processTypeList;
        }

        public async Task<int> RemoveAsync(int id, string userId)
        {
            var operation = _context.Operations.Find(id);
            if (operation != null)
            {
                var date = DateTime.UtcNow;
                operation.IsEnable = false;
                operation.UpdatedBy = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;
                operation.UpdatedDate = date;

                return await _context.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<int> UpdateAsync(OperationUpdate OperationUpdate, string userId)
        {
            var operation = _context.Operations.Find(OperationUpdate.Id);
            var date = DateTime.UtcNow;
            operation.ProcessNumber = OperationUpdate.ProcessNumber;
            operation.AccountId = OperationUpdate.AccountId;
            operation.AccountDetailId = OperationUpdate.AccountDetailId;
            operation.ProcessTypeId = OperationUpdate.ProcessTypeId;
            operation.Price = OperationUpdate.Price;
            operation.ProcessPrice = OperationUpdate.ProcessPrice;
            operation.UpdatedBy = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;
            operation.UpdatedDate = date;
            operation.IsEnable = true;

            return await _context.SaveChangesAsync();
        }
    }
}
