using Calculate.Data;
using Calculate.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Calculate.Service.Services
{
    public class OperationService : IOperationService
    {
        private readonly DataContext _context;

        public OperationService(DataContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(int caseId, int ProcessNumber, int AccountId, int AccountDetailId, int ProcessTypeId, decimal Price, decimal ProcessPrice, string userId)
        {
            int currentUserId = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;
            Operation operation = new Operation();
            var date = DateTime.UtcNow;
            operation.ProcessNumber = ProcessNumber;
            operation.AccountId = AccountId;
            operation.AccountDetailId = AccountDetailId;
            operation.ProcessTypeId = ProcessTypeId;
            operation.Price = Price;
            operation.ProcessPrice = ProcessPrice;
            operation.CreatedBy = currentUserId;
            operation.CreatedDate = date;
            operation.UpdatedBy = currentUserId;
            operation.UpdatedDate = date;
            operation.IsEnable = true;
            operation.CaseId = caseId;

            await _context.Operations.AddAsync(operation);

            return await _context.SaveChangesAsync();
        }

        public async Task<List<AccountGetName>> GetAccountAsync(int caseId)
        {
            var accountList = await _context.Accounts.Where(x => x.IsEnable == true && x.CaseId == caseId).Select(x => new AccountGetName { Id = x.Id, Name = x.Name }).ToListAsync();
            return accountList;
        }

        public async Task<List<AccountGetName>> GetAccountIdAsync(string accountName)
        {
            var accountList = await _context.Accounts.Where(x => x.Name == accountName).Select(x => new AccountGetName { Id = x.Id, Name = x.Name }).ToListAsync();
            return accountList;
        }

        public async Task<List<OperationGet>> GetAllAsync()
        {
            var list = from o in _context.Operations
                       join a in _context.Accounts on o.AccountId equals a.Id
                       join ad in _context.AccountDetails on o.AccountDetailId equals ad.Id
                       join b in _context.Banks on ad.BankId equals b.Id
                       join pt in _context.ProcessTypes on o.ProcessTypeId equals pt.Id
                       join c in _context.Cases on o.CaseId equals c.Id
                       where o.IsEnable == true
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

        public async Task<List<Bank>> GetBankAsync(int accountId)
        {
            var bankList = from b in _context.Banks
                           join ad in _context.AccountDetails on b.Id equals ad.BankId
                           where ad.AccountId == accountId && ad.IsEnable == true
                           select new Bank
                           {
                               Id = ad.Id,
                               Name = b.Name
                           };

            return await bankList.ToListAsync();
        }

        public async Task<List<Bank>> GetBankIdAsync(string accountName)
        {
            var bankList = from b in _context.Banks
                           join ad in _context.AccountDetails on b.Id equals ad.BankId
                           where b.Name == accountName && ad.IsEnable == true
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

        public async Task<List<Case>> GetCaseAsync(string officeId)
        {
            int _officeId = Convert.ToInt32(officeId);
            var caseList = await _context.Cases.Where(x => x.officeId == _officeId).Select(x => new Case { Id = x.Id, Name = x.Name }).ToListAsync();
            return caseList;
        }

        public async Task<List<Case>> GetCaseIdAsync(string caseName)
        {
            var caseList = await _context.Cases.Where(x => x.Name == caseName).Select(x => new Case { Id = x.Id, Name = x.Name }).ToListAsync();
            return caseList;
        }

        public async Task<List<ProcessType>> GetProcessTypeAsync()
        {
            var processTypeList = await _context.ProcessTypes.Select(x => new ProcessType { Id = x.Id, Name = x.Name }).ToListAsync();
            return processTypeList;
        }

        public async Task<List<ProcessType>> GetProcessTypeIdAsync(string processTypeName)
        {
            var processTypeList = await _context.ProcessTypes.Where(x => x.Name == processTypeName).Select(x => new ProcessType { Id = x.Id, Name = x.Name }).ToListAsync();
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

        public async Task<bool> SaveUploadExcelAsync(List<OperationUploadExcel> data, string userId)
        {
            try
            {
                var dateTimeNow = DateTime.UtcNow;
                int currentUserId = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;
                var valuee = data.AsEnumerable();
                var query = valuee.Join(_context.Banks, d => d.BankName, b => b.Name, (d, b) => new { d, BankId = b.Id })
                            .Join(_context.Cases, dbb => dbb.d.CaseName, c => c.Name, (dbb, c) => new { dbb, CaseId = c.Id })
                            .Join(_context.Accounts, dbbc => dbbc.dbb.d.Account, a => a.Name, (dbbc, a) => new { dbbc, AccountId = a.Id })
                            .Join(_context.ProcessTypes, dbbca => dbbca.dbbc.dbb.d.ProcessType, p => p.Name, (dbbca, p) => new { dbbca, ProcessTypeId = p.Id })
                            .Select(x => new Operation
                            {
                                ProcessNumber = Convert.ToInt32(x.dbbca.dbbc.dbb.d.ProcessNumber),
                                AccountId = x.dbbca.AccountId,
                                AccountDetailId = x.dbbca.dbbc.dbb.BankId,
                                ProcessTypeId = x.ProcessTypeId,
                                Price = Convert.ToDecimal(x.dbbca.dbbc.dbb.d.Price),
                                ProcessPrice = Convert.ToDecimal(x.dbbca.dbbc.dbb.d.ProcessPrice),
                                IsEnable = true,
                                CreatedDate = dateTimeNow,
                                CreatedBy = currentUserId,
                                UpdatedDate = dateTimeNow,
                                UpdatedBy = currentUserId,
                                CaseId = x.dbbca.dbbc.CaseId
                            });
                var resultData = query.ToList();
                await _context.Operations.AddRangeAsync(resultData);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
                return false;
            }

            return true;
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
