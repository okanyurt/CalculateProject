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
        User GetUserIndex(string token);

        User GetUserLogin(string mobilePhone, string password);
    }
}
