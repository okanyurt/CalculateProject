using Calculate.Data;
using Calculate.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace Calculate.Service.Services
{
    public class LoginService : ILoginService
    {
        private readonly DataContext _context;

        public LoginService(DataContext context)
        {
            _context = context;
        }
        public async Task<User> GetUserIndex(string token)
        {                     
            return await _context.Users.AsQueryable().Where(x => x.IsEnabled && x.AccessToken == token).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserLogin(string mobilePhone, string password)
        {
            return await _context.Users.AsQueryable().Where(x => x.IsEnabled && x.UserName == mobilePhone && x.PasswordHash == password).FirstOrDefaultAsync();
        }
    }
}
