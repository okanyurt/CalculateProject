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

        public void Add(int ProcessNumber, int AccountId, int AccountDetailId, int ProcessTypeId, decimal Price, decimal ProcessPrice, string userId)
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

             _context.Operations.Add(operation);
            _context.SaveChanges();
        }

        public List<Operation> GetAll()
        {
            
            var list =  _context.Operations.Where(x => x.IsEnable == true).ToList();
            return list;
        }

        public async Task<Operation> GetByIdAsync(int id)
        {
            return await _context.Operations.Where(x => x.IsEnable == true && x.Id == id).FirstOrDefaultAsync();
        }

        public void Remove(int id, string userId)
        {
            var operation =  _context.Operations.Find(id);
            if (operation != null)
            {
                var date = DateTime.UtcNow;
                operation.IsEnable = false;
                operation.UpdatedBy = _context.Users.FirstOrDefault(x => x.UserId == userId).Id;
                operation.UpdatedDate = date;

                _context.SaveChanges();
            }
        }

        public void Update(OperationUpdate OperationUpdate, string userId)
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

            _context.SaveChanges();
        }
    }
}
