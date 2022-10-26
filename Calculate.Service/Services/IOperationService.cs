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

        List<Operation> GetAll();

        void Add(int ProcessNumber, int AccountId, int AccountDetailId, int ProcessTypeId, decimal Price, decimal ProcessPrice, string userId);

        void Update(OperationUpdate OperationUpdate, string userId);

        void Remove(int id, string userId);
    }
}
