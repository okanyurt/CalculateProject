using Calculate.Data.Models;

namespace Calculate.Service.Services
{
    public interface IOperationService
    {
        Task<Operation> GetByIdAsync(int id);

        Task<List<OperationGet>> GetAllAsync(string _officeId, bool isAdmin);

        Task<int> AddAsync(OperationCreate OperationCreate, string userId);

        Task<int> UpdateAsync(OperationUpdate OperationUpdate, string userId);

        Task<int> RemoveAsync(int id, string userId);

        Task<List<AccountGetName>> GetAccountAsync(int caseId);

        Task<List<Bank>> GetBankAsync(int accountId);

        Task<List<ProcessType>> GetProcessTypeAsync();

        Task<List<Case>> GetCaseAsync(string officeId, string roleId);

        Task<bool> SaveUploadExcelAsync(List<OperationUploadExcel> operationUploadExcels, string userId);

        string GetMaxDate();

        Task<List<OperationGet>> GetAllSelectDateAsync(string _officeId, string _date,bool isAdmin);
    }
}

