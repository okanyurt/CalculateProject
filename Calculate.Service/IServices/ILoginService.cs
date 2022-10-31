using Calculate.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate.Service.Services
{
    public interface ILoginService
    {
        Task<User> GetUserIndex(string token);

        Task<User> GetUserLogin(string mobilePhone, string password);
    }
}
