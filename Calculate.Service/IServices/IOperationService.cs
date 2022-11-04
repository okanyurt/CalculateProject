using Calculate.Data.Models;

namespace Calculate.Service.Services
{
    public interface IOperationService
    {
        Task<Operation> GetByIdAsync(int id);

        Task<List<OperationGet>> GetAllAsync();

        Task<int> AddAsync(int caseId, int ProcessNumber, int AccountId, int AccountDetailId, int ProcessTypeId, decimal Price, decimal ProcessPrice, string userId);

        Task<int> UpdateAsync(OperationUpdate OperationUpdate, string userId);

        Task<int> RemoveAsync(int id, string userId);

        Task<List<AccountGetName>> GetAccountAsync(int caseId);

        Task<List<Bank>> GetBankAsync(int accountId);

        Task<List<ProcessType>> GetProcessTypeAsync();

        Task<List<Case>> GetCaseAsync(string officeId);

        Task<List<Bank>> GetBankIdAsync(string accountName);

        Task<List<ProcessType>> GetProcessTypeIdAsync(string processTypeName);

        Task<List<Case>> GetCaseIdAsync(string caseName);

        Task<List<AccountGetName>> GetAccountIdAsync(string accountName);

        Task<bool> SaveUploadExcelAsync(List<OperationUploadExcel> operationUploadExcels, string userId);
    }
}

