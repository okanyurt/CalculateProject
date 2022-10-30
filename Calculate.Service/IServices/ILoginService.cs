using Calculate.Data.Models;

namespace Calculate.Service.Services
{
    public interface ILoginService
    {
        Task<User> GetUserIndex(string token);

        Task<User> GetUserLogin(string mobilePhone, string password);
    }
}
