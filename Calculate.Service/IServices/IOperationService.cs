using Calculate.Data.Models;

namespace Calculate.Service.IServices;

public interface IOperationService
{
    Task<Operation> GetByIdAsync(int id);

    List<OperationGet> GetAll();

    void Add(int ProcessNumber, int AccountId, int AccountDetailId, int ProcessTypeId, decimal Price, decimal ProcessPrice, string userId);

    void Update(OperationUpdate OperationUpdate, string userId);

    void Remove(int id, string userId);

    List<AccountGetName> GetAccount();

    List<Bank> GetBank(int accountId);
}

