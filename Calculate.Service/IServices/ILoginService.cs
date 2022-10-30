using Calculate.Data.Models;

namespace Calculate.Service.IServices;

public interface ILoginService
{
    User GetUserIndex(string token);

    User GetUserLogin(string mobilePhone, string password);
}

