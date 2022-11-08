using Calculate.Data;
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

        public async Task<int> AddOperationArsivAsync(List<Operation> operations, string userId)
        {
            int currentUserId = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;
            int result = 0;
            foreach (var item in operations)
            {
                OperationArsiv operationArsiv = new OperationArsiv();
                var date = DateTime.UtcNow;
                operationArsiv.ProcessNumber = item.ProcessNumber;
                operationArsiv.AccountId = item.AccountId;
                operationArsiv.AccountDetailId = item.AccountDetailId;
                operationArsiv.ProcessTypeId = item.ProcessTypeId;
                operationArsiv.Price = item.Price;
                operationArsiv.ProcessPrice = item.ProcessPrice;
                operationArsiv.CreatedBy = currentUserId;
                operationArsiv.CreatedDate = date;
                operationArsiv.UpdatedBy = currentUserId;
                operationArsiv.UpdatedDate = date;
                operationArsiv.IsEnable = item.IsEnable;
                operationArsiv.CaseId = item.CaseId;

                await _context.OperationsArsiv.AddAsync(operationArsiv);
                result = await _context.SaveChangesAsync();
            } 

            return result;
        }
         
        public async Task<int> AddOperationAsync(int caseId, string userId)
        {

            var list =  _context.OperationsArsiv.Where(x => x.CaseId == caseId && x.IsEnable == true).GroupBy(y => new { y.AccountId, y.AccountDetailId, y.ProcessTypeId,y.UpdatedDate }).Select(group => new OperationArsiv
            {
                AccountId = group.Key.AccountId,
                AccountDetailId = group.Key.AccountDetailId,
                ProcessTypeId = group.Key.ProcessTypeId,
                Price = group.Sum(x => x.Price),
                UpdatedDate = group.Key.UpdatedDate
            });

            var groupList = await list.ToListAsync();
            var today = DateTime.Today.Date.ToString("dd-MM-yyyy");
            List<OperationArsiv> operations = new List<OperationArsiv>();
            foreach (var item in groupList)
            {
                if (item.UpdatedDate.ToString("dd-MM-yyyy") == today)
                {
                    operations.Add(item);
                }
            }


            int AccountId = 0;
            int AccountDetailId = 0;
            decimal price = 0;
            int result = 0;
            int currentUserId = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;
            int index = 0;
            foreach (var item in operations)
            {               
                if (AccountId == 0 && AccountDetailId == 0)
                {
                    AccountId = item.AccountId;
                    AccountDetailId = item.AccountDetailId;
                }

                if (item.AccountId == AccountId && item.AccountDetailId == AccountDetailId)
                {
                    if (item.ProcessTypeId == 1 || item.ProcessTypeId == 5 || item.ProcessTypeId == 6)
                    {
                        price += item.Price;
                    }
                    else
                    {
                        price -= item.Price;
                    }
                }

                if ((item.AccountId != AccountId && item.AccountDetailId != AccountDetailId) || operations.Count -1 == index )
                {                  
                    Operation operation = new Operation();
                    var date = DateTime.UtcNow;
                    operation.ProcessNumber = item.ProcessNumber;
                    operation.AccountId = item.AccountId;
                    operation.AccountDetailId = item.AccountDetailId;
                    operation.ProcessTypeId = 5;
                    operation.Price = price;
                    operation.ProcessPrice = item.ProcessPrice;
                    operation.CreatedBy = currentUserId;
                    operation.CreatedDate = date;
                    operation.UpdatedBy = currentUserId;
                    operation.UpdatedDate = date;
                    operation.IsEnable = item.IsEnable;
                    operation.CaseId = item.CaseId;

                    await _context.Operations.AddAsync(operation);
                    result = await _context.SaveChangesAsync();

                    AccountId = item.AccountId;
                    AccountDetailId = item.AccountDetailId;
                    price = 0;
                }

                index++;
            }

            return result;
        }

        public async Task<List<Operation>> GetAllAsync(int caseId)
        {
            var today = DateTime.Today.Date.ToString("dd-MM-yyyy");
            List<Operation> operations = new List<Operation>();

            var list = await _context.Operations.Where(x => x.CaseId == caseId && x.IsEnable == true).ToListAsync();

            foreach (var item in list)
            {
                if (item.UpdatedDate.ToString("dd-MM-yyyy") == today)
                {
                    operations.Add(item);
                }
            }

            return operations;
        }

        public async Task<List<Case>> GetCaseAsync(string officeId)
        {
            int _officeId = Convert.ToInt32(officeId);
            var caseList = await _context.Cases.Where(x => x.officeId == _officeId).Select(x => new Case { Id = x.Id, Name = x.Name }).ToListAsync();
            return caseList;
        }
    }
}
