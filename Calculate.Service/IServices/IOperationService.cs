using Calculate.Data.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Calculate.Service.Services
{
    public interface IOperationService
    {
        Task<Operation> GetByIdAsync(int id);

        Task<List<OperationGet>> GetAllAsync();

        Task<int> AddAsync(int ProcessNumber, int AccountId, int AccountDetailId, int ProcessTypeId, decimal Price, decimal ProcessPrice, string userId);

        Task<int> UpdateAsync(OperationUpdate OperationUpdate, string userId);

        Task<int> RemoveAsync(int id, string userId);

        Task<List<AccountGetName>> GetAccountAsync();

        Task<List<Bank>> GetBankAsync(int accountId);

        Task<List<ProcessType>> GetProcessTypeAsync();
    }
}
