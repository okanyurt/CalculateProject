namespace Calculate.Service.Services
{
    public interface IGenericRemoveService
    {
        Task<int> RemoveAsync(int id, int isMaster, string userId);
    }
}
