using Calculate.Data;
using Calculate.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate.Service.Services
{
    public class LoginService : ILoginService
    {
        private readonly DataContext _context;

        public LoginService(DataContext context)
        {
            _context = context;
        }
        public User GetUserIndex(string token)
        {
           return  _context.Users.Where(x => x.IsEnabled && x.AccessToken == token).FirstOrDefault();
        }

        public User GetUserLogin(string mobilePhone, string password)
        {
            return _context.Users.FirstOrDefault(x => x.IsEnabled && x.UserName == mobilePhone && x.PasswordHash == password);
        }
    }
}
